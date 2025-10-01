using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventrySystem.Migrations
{
    /// <inheritdoc />
    public partial class addRoleConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "DateCreated", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "310754fe-2c04-4f63-9e3e-348f0e988990", null, new DateTime(2024, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "USER" },
                    { "b327e659-d135-4faa-a43d-f02b743f9872", null, new DateTime(2024, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Data Entry", "DATA ENTRY" },
                    { "eea44dd0-d656-44b4-8f1e-aaa18f40ab1e", null, new DateTime(2024, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "310754fe-2c04-4f63-9e3e-348f0e988990");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b327e659-d135-4faa-a43d-f02b743f9872");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eea44dd0-d656-44b4-8f1e-aaa18f40ab1e");
        }
    }
}
