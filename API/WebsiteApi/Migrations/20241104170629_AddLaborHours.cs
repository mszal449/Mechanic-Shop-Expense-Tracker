using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsiteApi.Migrations
{
    /// <inheritdoc />
    public partial class AddLaborHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LaborHours",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LaborHours",
                table: "Jobs");
        }
    }
}
