using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoriesWeb.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUrl",
                table: "ApplicationSetup",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUrl",
                table: "ApplicationSetup");
        }
    }
}
