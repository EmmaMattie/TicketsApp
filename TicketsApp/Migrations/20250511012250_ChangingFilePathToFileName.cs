using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangingFilePathToFileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageFilePath",
                table: "Event",
                newName: "ImageFileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageFileName",
                table: "Event",
                newName: "ImageFilePath");
        }
    }
}
