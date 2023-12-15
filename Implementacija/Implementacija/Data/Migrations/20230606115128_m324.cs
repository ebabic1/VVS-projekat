using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Implementacija.Data.Migrations
{
    public partial class m324 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "datum",
                table: "Koncerti",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "datum",
                table: "Koncerti");
        }
    }
}
