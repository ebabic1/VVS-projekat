using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Implementacija.Data.Migrations
{
    public partial class mpravaidentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Korisnici",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Korisnici",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Korisnici",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Korisnici",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Korisnici",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Korisnici",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Korisnici");
        }
    }
}
