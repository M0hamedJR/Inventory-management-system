using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventrySystem.Migrations
{
    /// <inheritdoc />
    public partial class addCustomerNameToProductTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "dealer",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "dealer",
                table: "Products");
        }
    }
}
