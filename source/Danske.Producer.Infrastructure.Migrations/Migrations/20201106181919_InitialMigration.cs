using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Danske.Producer.Infrastructure.Migrations.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    Municipality = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    PeriodType = table.Column<byte>(nullable: false),
                    PeriodStart = table.Column<DateTime>(nullable: false),
                    PeriodEnd = table.Column<DateTime>(nullable: false),
                    Tax = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => new { x.Municipality, x.PeriodType, x.PeriodStart, x.PeriodEnd });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Taxes");
        }
    }
}
