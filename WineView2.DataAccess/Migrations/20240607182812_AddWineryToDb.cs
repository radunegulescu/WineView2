using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WineView2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddWineryToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wineries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wineries", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Wineries",
                columns: new[] { "Id", "Name", "Region" },
                values: new object[,]
                {
                    { 1, "Cotnari", "Muntenia" },
                    { 2, "Purcari", "Oltenia" },
                    { 3, "Jidvei", "Moldova" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wineries");
        }
    }
}
