using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GA360.DAL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Propertiesrename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegistrationDate",
                table: "QualificationCustomerCourseCertificates",
                newName: "CourseRegistrationDate");

            migrationBuilder.RenameColumn(
                name: "ExpectedDate",
                table: "QualificationCustomerCourseCertificates",
                newName: "CourseExpectedDate");

            migrationBuilder.RenameColumn(
                name: "CertificateDate",
                table: "QualificationCustomerCourseCertificates",
                newName: "CourseCertificateDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseRegistrationDate",
                table: "QualificationCustomerCourseCertificates",
                newName: "RegistrationDate");

            migrationBuilder.RenameColumn(
                name: "CourseExpectedDate",
                table: "QualificationCustomerCourseCertificates",
                newName: "ExpectedDate");

            migrationBuilder.RenameColumn(
                name: "CourseCertificateDate",
                table: "QualificationCustomerCourseCertificates",
                newName: "CertificateDate");
        }
    }
}
