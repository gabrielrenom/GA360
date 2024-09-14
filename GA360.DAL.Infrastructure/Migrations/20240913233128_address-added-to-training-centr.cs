using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GA360.DAL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addressaddedtotrainingcentr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TrainingCentres_AddressId",
                table: "TrainingCentres",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingCentres_Addresses_AddressId",
                table: "TrainingCentres",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingCentres_Addresses_AddressId",
                table: "TrainingCentres");

            migrationBuilder.DropIndex(
                name: "IX_TrainingCentres_AddressId",
                table: "TrainingCentres");
        }
    }
}
