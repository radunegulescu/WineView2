using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WineView2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddVolumeAndClassifierIdToWine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClasifierId",
                table: "Wines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsInClasifier",
                table: "Wines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Volume",
                table: "Wines",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ClasifierId", "IsInClasifier", "Volume" },
                values: new object[] { 2, true, 2.0 });

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ClasifierId", "IsInClasifier", "Volume" },
                values: new object[] { -1, false, 2.0 });

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ClasifierId", "IsInClasifier", "Volume" },
                values: new object[] { -1, false, 2.0 });

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ClasifierId", "IsInClasifier", "Volume" },
                values: new object[] { -1, false, 2.0 });

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ClasifierId", "IsInClasifier", "Volume" },
                values: new object[] { -1, false, 2.0 });

            migrationBuilder.UpdateData(
                table: "Wines",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ClasifierId", "IsInClasifier", "Volume" },
                values: new object[] { -1, false, 2.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClasifierId",
                table: "Wines");

            migrationBuilder.DropColumn(
                name: "IsInClasifier",
                table: "Wines");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Wines");
        }
    }
}
