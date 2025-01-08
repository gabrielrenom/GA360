using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GA360.DAL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QAN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IDX_TrainingCentres_Id",
                table: "TrainingCentres");

            migrationBuilder.DropIndex(
                name: "IDX_QualificationStatuses_Id",
                table: "QualificationStatuses");

            migrationBuilder.DropIndex(
                name: "IDX_Qualifications_Id",
                table: "Qualifications");

            migrationBuilder.DropIndex(
                name: "IDX_Courses_Id",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IDX_Certificates_Id",
                table: "Certificates");

            migrationBuilder.RenameIndex(
                name: "IDX_QCCC_QualificationStatusId",
                table: "QualificationCustomerCourseCertificates",
                newName: "IX_QualificationCustomerCourseCertificates_QualificationStatusId");

            migrationBuilder.RenameIndex(
                name: "IDX_QCCC_QualificationId",
                table: "QualificationCustomerCourseCertificates",
                newName: "IX_QualificationCustomerCourseCertificates_QualificationId");

            migrationBuilder.RenameIndex(
                name: "IDX_QCCC_CustomerId",
                table: "QualificationCustomerCourseCertificates",
                newName: "IX_QualificationCustomerCourseCertificates_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IDX_QCCC_CourseId",
                table: "QualificationCustomerCourseCertificates",
                newName: "IX_QualificationCustomerCourseCertificates_CourseId");

            migrationBuilder.RenameIndex(
                name: "IDX_QCCC_CertificateId",
                table: "QualificationCustomerCourseCertificates",
                newName: "IX_QualificationCustomerCourseCertificates_CertificateId");

            migrationBuilder.RenameIndex(
                name: "IDX_Customer_TrainingCentreId",
                table: "Customers",
                newName: "IX_Customers_TrainingCentreId");

            migrationBuilder.AddColumn<string>(
                name: "QAN",
                table: "Qualifications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QAN",
                table: "Qualifications");

            migrationBuilder.RenameIndex(
                name: "IX_QualificationCustomerCourseCertificates_QualificationStatusId",
                table: "QualificationCustomerCourseCertificates",
                newName: "IDX_QCCC_QualificationStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_QualificationCustomerCourseCertificates_QualificationId",
                table: "QualificationCustomerCourseCertificates",
                newName: "IDX_QCCC_QualificationId");

            migrationBuilder.RenameIndex(
                name: "IX_QualificationCustomerCourseCertificates_CustomerId",
                table: "QualificationCustomerCourseCertificates",
                newName: "IDX_QCCC_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_QualificationCustomerCourseCertificates_CourseId",
                table: "QualificationCustomerCourseCertificates",
                newName: "IDX_QCCC_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_QualificationCustomerCourseCertificates_CertificateId",
                table: "QualificationCustomerCourseCertificates",
                newName: "IDX_QCCC_CertificateId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_TrainingCentreId",
                table: "Customers",
                newName: "IDX_Customer_TrainingCentreId");

            migrationBuilder.CreateIndex(
                name: "IDX_TrainingCentres_Id",
                table: "TrainingCentres",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IDX_QualificationStatuses_Id",
                table: "QualificationStatuses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IDX_Qualifications_Id",
                table: "Qualifications",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IDX_Courses_Id",
                table: "Courses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IDX_Certificates_Id",
                table: "Certificates",
                column: "Id");
        }
    }
}
