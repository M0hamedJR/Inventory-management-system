using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventrySystem.Migrations
{
    /// <inheritdoc />
    public partial class addIn_OutColumnInProductTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "In_Out",
                table: "Products",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "In_Out",
                table: "Products");
        }
    }
}
