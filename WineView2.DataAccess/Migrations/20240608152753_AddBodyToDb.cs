using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WineView2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBodyToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.CreateTable(
                name: "Bodies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bodies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Bodies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Light" },
                    { 2, "Medium" },
                    { 3, "Heavy" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bodies");

            migrationBuilder.InsertData(
                table: "Wines",
                columns: new[] { "Id", "ClasifierId", "ColorId", "ImageUrl", "IsInClasifier", "Name", "Price", "Price10", "Price5", "StyleId", "Volume", "WineryId" },
                values: new object[,]
                {
                    { 1, 2, 1, "", true, "Fortune of Time", 90.0, 80.0, 85.0, 1, 2.0, 1 },
                    { 2, -1, 1, "", false, "Dark Skies", 30.0, 20.0, 25.0, 1, 2.0, 1 },
                    { 3, -1, 1, "", false, "Vanish in the Sunset", 50.0, 35.0, 40.0, 1, 2.0, 1 },
                    { 4, -1, 2, "", false, "Cotton Candy", 65.0, 55.0, 60.0, 2, 2.0, 2 },
                    { 5, -1, 2, "", false, "Rock in the Ocean", 27.0, 20.0, 25.0, 2, 2.0, 2 },
                    { 6, -1, 3, "", false, "Leaves and Wonders", 23.0, 20.0, 22.0, 2, 2.0, 3 }
                });
        }
    }
}
