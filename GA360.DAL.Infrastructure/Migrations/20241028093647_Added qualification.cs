using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GA360.DAL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addedqualification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QualificationStatusId",
                table: "QualificationCustomerCourseCertificates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QualificationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModyfiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualificationStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QualificationCustomerCourseCertificates_QualificationStatusId",
                table: "QualificationCustomerCourseCertificates",
                column: "QualificationStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_QualificationCustomerCourseCertificates_QualificationStatuses_QualificationStatusId",
                table: "QualificationCustomerCourseCertificates",
                column: "QualificationStatusId",
                principalTable: "QualificationStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QualificationCustomerCourseCertificates_QualificationStatuses_QualificationStatusId",
                table: "QualificationCustomerCourseCertificates");

            migrationBuilder.DropTable(
                name: "QualificationStatuses");

            migrationBuilder.DropIndex(
                name: "IX_QualificationCustomerCourseCertificates_QualificationStatusId",
                table: "QualificationCustomerCourseCertificates");

            migrationBuilder.DropColumn(
                name: "QualificationStatusId",
                table: "QualificationCustomerCourseCertificates");
        }
    }
}
