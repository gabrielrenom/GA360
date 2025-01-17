using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GA360.DAL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addregdatecoure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CertificateDate",
                table: "QualificationCustomerCourseCertificates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedDate",
                table: "QualificationCustomerCourseCertificates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "QualificationCustomerCourseCertificates",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateDate",
                table: "QualificationCustomerCourseCertificates");

            migrationBuilder.DropColumn(
                name: "ExpectedDate",
                table: "QualificationCustomerCourseCertificates");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "QualificationCustomerCourseCertificates");
        }
    }
}
