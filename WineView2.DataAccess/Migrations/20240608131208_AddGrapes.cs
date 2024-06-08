using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WineView2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddGrapes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grapes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grapes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GrapeWine",
                columns: table => new
                {
                    GrapesId = table.Column<int>(type: "int", nullable: false),
                    WinesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrapeWine", x => new { x.GrapesId, x.WinesId });
                    table.ForeignKey(
                        name: "FK_GrapeWine_Grapes_GrapesId",
                        column: x => x.GrapesId,
                        principalTable: "Grapes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrapeWine_Wines_WinesId",
                        column: x => x.WinesId,
                        principalTable: "Wines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Grapes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Merlot" },
                    { 2, "Syrah" },
                    { 3, "Airen" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrapeWine_WinesId",
                table: "GrapeWine",
                column: "WinesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrapeWine");

            migrationBuilder.DropTable(
                name: "Grapes");
        }
    }
}
