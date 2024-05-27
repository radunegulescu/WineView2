using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WineView2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NewInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price5 = table.Column<double>(type: "float", nullable: false),
                    Price10 = table.Column<double>(type: "float", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wines_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Red" },
                    { 2, "Rose" },
                    { 3, "White" }
                });

            migrationBuilder.InsertData(
                table: "Wines",
                columns: new[] { "Id", "ColorId", "ImageUrl", "Name", "Price", "Price10", "Price5" },
                values: new object[,]
                {
                    { 1, 1, "", "Fortune of Time", 90.0, 80.0, 85.0 },
                    { 2, 1, "", "Dark Skies", 30.0, 20.0, 25.0 },
                    { 3, 1, "", "Vanish in the Sunset", 50.0, 35.0, 40.0 },
                    { 4, 2, "", "Cotton Candy", 65.0, 55.0, 60.0 },
                    { 5, 2, "", "Rock in the Ocean", 27.0, 20.0, 25.0 },
                    { 6, 3, "", "Leaves and Wonders", 23.0, 20.0, 22.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wines_ColorId",
                table: "Wines",
                column: "ColorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wines");

            migrationBuilder.DropTable(
                name: "Colors");
        }
    }
}
