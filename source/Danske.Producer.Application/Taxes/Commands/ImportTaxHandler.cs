using Danske.Producer.Domain.Tax;
using Danske.Producer.Enums;
using Danske.Producer.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Danske.Producer.Application.Taxes.Commands
{
    public interface IImportTaxHandler
    {
        ValueTask ImportTaxes(Stream stream);
    }

    public class ImportTaxHandler : IImportTaxHandler
    {
        private readonly TaxesDbContext _dbContext;

        public ImportTaxHandler(TaxesDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async ValueTask ImportTaxes(Stream stream)
        {
            var dataTable = GetTableFromExcel(stream);

            var sqlConnection = GetSqlConnection();
            using var sqlBulkCopy = new SqlBulkCopy(sqlConnection);
            await sqlConnection.OpenAsync();

            sqlBulkCopy.DestinationTableName = GetTableName();

            try
            {
                await sqlBulkCopy.WriteToServerAsync(dataTable);
                await sqlConnection.CloseAsync();
            }
            catch (SqlException exception)
            {
                if (exception.Number == 2627)
                {
                    throw new ApplicationException("1 or more identical record already exists in database");
                }

                await sqlConnection.CloseAsync();

                throw;
            }
        }

        private DataTable GetTableFromExcel(Stream inputStream)
        {
            try
            {
                var dataTable = new DataTable();
                using var package = new ExcelPackage(inputStream);
                var workSheet = package.Workbook.Worksheets.First();

                foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                {
                    dataTable.Columns.Add(firstRowCell.Text);
                }

                for (var rowNum = 2; rowNum <= workSheet.Dimension.End.Row; rowNum++)
                {
                    var wsRow = workSheet.Cells[rowNum, 1, rowNum, workSheet.Dimension.End.Column];
                    var dataTableRow = dataTable.Rows.Add();

                    foreach (var excelCell in wsRow)
                    {
                        var columnIndex = excelCell.Start.Column - 1;
                        AssignCells(dataTableRow, excelCell, columnIndex);
                    }
                }

                return dataTable;
            }
            catch (InvalidDataException)
            {
                throw new ApplicationException("Wrong file format, endpoint only accepts .xlsx files");
            }
        }

        private void AssignCells(DataRow dataTableRow, ExcelRangeBase excelCell, int columnIndex)
        {
            if (columnIndex == 1)
            {
                var isParseSuccessful = Enum.TryParse(excelCell.Text, out PeriodType periodType);

                if (!isParseSuccessful)
                    throw new ApplicationException("Second column must contain PeriodType");

                dataTableRow[columnIndex] = (int)periodType;
            }
            else if (columnIndex == 2 || columnIndex == 3)
            {
                var isParseSuccessful = DateTime.TryParse(excelCell.Text, out var date);

                if (!isParseSuccessful)
                    throw new ApplicationException("Third and fourth columns must contain Date");

                dataTableRow[columnIndex] = date;
            }
            else if (columnIndex == 4)
            {
                var isParseSuccessful = decimal.TryParse(excelCell.Text, out var number);

                if (!isParseSuccessful)
                    throw new ApplicationException("Fifth column must contain number");

                dataTableRow[columnIndex] = number;
            }
            else
                dataTableRow[columnIndex] = excelCell.Text;
        }

        private SqlConnection GetSqlConnection()
        {
            var connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
            return new SqlConnection(connectionString);
        }

        private string GetTableName() =>
            _dbContext.Model.FindEntityType(typeof(Tax)).GetTableName();
    }
}