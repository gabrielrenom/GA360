using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GA360.DAL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Prics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Qualifications");

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "QualificationTrainingCentre",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "QualificationTrainingCentre",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Sale",
                table: "QualificationTrainingCentre",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CourseDiscount",
                table: "QualificationCustomerCourseCertificates",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CoursePrice",
                table: "QualificationCustomerCourseCertificates",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CourseSale",
                table: "QualificationCustomerCourseCertificates",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "QualificationDiscount",
                table: "QualificationCustomerCourseCertificates",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "QualificationPrice",
                table: "QualificationCustomerCourseCertificates",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "QualificationSale",
                table: "QualificationCustomerCourseCertificates",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "CourseTrainingCentre",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Sale",
                table: "CourseTrainingCentre",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "QualificationTrainingCentre");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "QualificationTrainingCentre");

            migrationBuilder.DropColumn(
                name: "Sale",
                table: "QualificationTrainingCentre");

            migrationBuilder.DropColumn(
                name: "CourseDiscount",
                table: "QualificationCustomerCourseCertificates");

            migrationBuilder.DropColumn(
                name: "CoursePrice",
                table: "QualificationCustomerCourseCertificates");

            migrationBuilder.DropColumn(
                name: "CourseSale",
                table: "QualificationCustomerCourseCertificates");

            migrationBuilder.DropColumn(
                name: "QualificationDiscount",
                table: "QualificationCustomerCourseCertificates");

            migrationBuilder.DropColumn(
                name: "QualificationPrice",
                table: "QualificationCustomerCourseCertificates");

            migrationBuilder.DropColumn(
                name: "QualificationSale",
                table: "QualificationCustomerCourseCertificates");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "CourseTrainingCentre");

            migrationBuilder.DropColumn(
                name: "Sale",
                table: "CourseTrainingCentre");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Qualifications",
                type: "float",
                nullable: true);
        }
    }
}
