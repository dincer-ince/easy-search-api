using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySearchApi.Migrations
{
    /// <inheritdoc />
    public partial class upt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dictionaries_Users_userId1",
                table: "Dictionaries");

            migrationBuilder.DropIndex(
                name: "IX_Dictionaries_userId1",
                table: "Dictionaries");

            migrationBuilder.DropColumn(
                name: "userId1",
                table: "Dictionaries");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "Dictionaries",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Dictionaries_userId",
                table: "Dictionaries",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dictionaries_Users_userId",
                table: "Dictionaries",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dictionaries_Users_userId",
                table: "Dictionaries");

            migrationBuilder.DropIndex(
                name: "IX_Dictionaries_userId",
                table: "Dictionaries");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "Dictionaries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "userId1",
                table: "Dictionaries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Dictionaries_userId1",
                table: "Dictionaries",
                column: "userId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Dictionaries_Users_userId1",
                table: "Dictionaries",
                column: "userId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
