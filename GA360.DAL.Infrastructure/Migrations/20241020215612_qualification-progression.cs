using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GA360.DAL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class qualificationprogression : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Progression",
                table: "QualificationCustomerCourseCertificates",
                newName: "QualificationProgression");

            migrationBuilder.AddColumn<int>(
                name: "CourseProgression",
                table: "QualificationCustomerCourseCertificates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseProgression",
                table: "QualificationCustomerCourseCertificates");

            migrationBuilder.RenameColumn(
                name: "QualificationProgression",
                table: "QualificationCustomerCourseCertificates",
                newName: "Progression");
        }
    }
}
