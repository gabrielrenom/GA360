using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GA360.DAL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class coursetraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseTrainingCentre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingCentreId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTrainingCentre", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTrainingCentre_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTrainingCentre_TrainingCentres_TrainingCentreId",
                        column: x => x.TrainingCentreId,
                        principalTable: "TrainingCentres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseTrainingCentre_CourseId",
                table: "CourseTrainingCentre",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTrainingCentre_TrainingCentreId",
                table: "CourseTrainingCentre",
                column: "TrainingCentreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseTrainingCentre");
        }
    }
}
