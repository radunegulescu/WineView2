using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WineView2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddWineryToWine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WineryId",
                table: "Wines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 1,
                column: "WineryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 2,
                column: "WineryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 3,
                column: "WineryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 4,
                column: "WineryId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 5,
                column: "WineryId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 6,
                column: "WineryId",
                value: 3);

            migrationBuilder.CreateIndex(
                name: "IX_Wines_WineryId",
                table: "Wines",
                column: "WineryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wines_Wineries_WineryId",
                table: "Wines",
                column: "WineryId",
                principalTable: "Wineries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wines_Wineries_WineryId",
                table: "Wines");

            migrationBuilder.DropIndex(
                name: "IX_Wines_WineryId",
                table: "Wines");

            migrationBuilder.DropColumn(
                name: "WineryId",
                table: "Wines");
        }
    }
}
