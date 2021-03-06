﻿// <auto-generated />
using System;
using Danske.Producer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Danske.Producer.Infrastructure.Migrations.Migrations
{
    [DbContext(typeof(TaxesDbContext))]
    [Migration("20201106181919_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Danske.Producer.Domain.Tax.Tax", b =>
                {
                    b.Property<string>("Municipality")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<byte>("PeriodType")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("PeriodStart")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PeriodEnd")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Result")
                        .HasColumnName("Tax")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Municipality", "PeriodType", "PeriodStart", "PeriodEnd");

                    b.ToTable("Taxes");
                });
#pragma warning restore 612, 618
        }
    }
}
