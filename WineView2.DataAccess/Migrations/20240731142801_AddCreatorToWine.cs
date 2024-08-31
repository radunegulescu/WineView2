using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WineView2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorToWine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Wines",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Wines_ApplicationUserId",
                table: "Wines",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wines_AspNetUsers_ApplicationUserId",
                table: "Wines",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wines_AspNetUsers_ApplicationUserId",
                table: "Wines");

            migrationBuilder.DropIndex(
                name: "IX_Wines_ApplicationUserId",
                table: "Wines");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Wines");
        }
    }
}
