using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GA360.DAL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class adedtrainingcentre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainingCentreId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TrainingCentreId",
                table: "Customers",
                column: "TrainingCentreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_TrainingCentres_TrainingCentreId",
                table: "Customers",
                column: "TrainingCentreId",
                principalTable: "TrainingCentres",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_TrainingCentres_TrainingCentreId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_TrainingCentreId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TrainingCentreId",
                table: "Customers");
        }
    }
}
