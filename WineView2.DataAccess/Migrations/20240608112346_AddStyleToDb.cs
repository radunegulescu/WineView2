using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WineView2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddStyleToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StyleId",
                table: "Wines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Styles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Styles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Styles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Dry" },
                    { 2, "Sweet" }
                });

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 1,
                column: "StyleId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 2,
                column: "StyleId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 3,
                column: "StyleId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 4,
                column: "StyleId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 5,
                column: "StyleId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 6,
                column: "StyleId",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_Wines_StyleId",
                table: "Wines",
                column: "StyleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wines_Styles_StyleId",
                table: "Wines",
                column: "StyleId",
                principalTable: "Styles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wines_Styles_StyleId",
                table: "Wines");

            migrationBuilder.DropTable(
                name: "Styles");

            migrationBuilder.DropIndex(
                name: "IX_Wines_StyleId",
                table: "Wines");

            migrationBuilder.DropColumn(
                name: "StyleId",
                table: "Wines");
        }
    }
}
