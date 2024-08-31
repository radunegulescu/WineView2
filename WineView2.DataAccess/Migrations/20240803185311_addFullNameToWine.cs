﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WineView2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addFullNameToWine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Wines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Wines");
        }
    }
}
