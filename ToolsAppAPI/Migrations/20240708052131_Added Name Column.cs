using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToolsAppAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Templates");
        }
    }
}
