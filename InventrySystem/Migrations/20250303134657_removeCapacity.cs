using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventrySystem.Migrations
{
    /// <inheritdoc />
    public partial class removeCapacity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Shelfs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Shelfs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
