using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GA360.DAL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QualificationTrainingCentres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QualificationTrainingCentre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingCentreId = table.Column<int>(type: "int", nullable: false),
                    QualificationId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_QualificationTrainingCentre", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualificationTrainingCentre_Qualifications_QualificationId",
                        column: x => x.QualificationId,
                        principalTable: "Qualifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualificationTrainingCentre_TrainingCentres_TrainingCentreId",
                        column: x => x.TrainingCentreId,
                        principalTable: "TrainingCentres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QualificationTrainingCentre_QualificationId",
                table: "QualificationTrainingCentre",
                column: "QualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_QualificationTrainingCentre_TrainingCentreId",
                table: "QualificationTrainingCentre",
                column: "TrainingCentreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QualificationTrainingCentre");
        }
    }
}
