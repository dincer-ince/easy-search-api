using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySearchApi.Migrations
{
    /// <inheritdoc />
    public partial class newDataTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "numberOfWords",
                table: "Documents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfDocuments",
                table: "Dictionaries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "totalNumberOfWords",
                table: "Dictionaries",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "numberOfWords",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "NumberOfDocuments",
                table: "Dictionaries");

            migrationBuilder.DropColumn(
                name: "totalNumberOfWords",
                table: "Dictionaries");
        }
    }
}
