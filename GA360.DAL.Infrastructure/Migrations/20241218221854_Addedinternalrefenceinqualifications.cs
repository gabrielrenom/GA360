using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GA360.DAL.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addedinternalrefenceinqualifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwardingBody",
                table: "Qualifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalReference",
                table: "Qualifications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwardingBody",
                table: "Qualifications");

            migrationBuilder.DropColumn(
                name: "InternalReference",
                table: "Qualifications");
        }
    }
}
