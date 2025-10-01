using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventrySystem.Migrations
{
    public partial class AddProductShelfTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First add ProductCount column to Categories if it doesn't exist
            migrationBuilder.AddColumn<int>(
                name: "ProductCount",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Add Capacity column to Shelves if it doesn't exist
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Shelfs",
                type: "int",
                nullable: false,
                defaultValue: 50); // Default capacity of 50 items

            // Trigger for Product Changes
            migrationBuilder.Sql(@"
                CREATE TRIGGER TR_Products_Changes
                ON Products
                AFTER INSERT, UPDATE, DELETE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    -- For Inserts and Updates
                    IF EXISTS (SELECT * FROM inserted)
                    BEGIN
                        -- Update Shelf availability if product is added to a shelf
                        UPDATE s
                        SET IsAvailable = 
                            CASE 
                                WHEN (SELECT COUNT(*) FROM Products WHERE ShelfId = s.Id) >= s.Capacity THEN 0 
                                ELSE 1 
                            END
                        FROM Shelfs s
                        WHERE s.Id IN (SELECT ShelfId FROM inserted WHERE ShelfId IS NOT NULL);

                        -- Update Category product count
                        UPDATE c
                        SET ProductCount = (
                            SELECT COUNT(*) 
                            FROM Products p 
                            WHERE p.CategoryId = c.Id
                        )
                        FROM Categories c
                        WHERE c.Id IN (SELECT CategoryId FROM inserted);
                    END

                    -- For Deletes
                    IF EXISTS (SELECT * FROM deleted)
                    BEGIN
                        -- Update Shelf availability when product is removed
                        UPDATE s
                        SET IsAvailable = 
                            CASE 
                                WHEN (SELECT COUNT(*) FROM Products WHERE ShelfId = s.Id) >= s.Capacity THEN 0 
                                ELSE 1 
                            END
                        FROM Shelfs s
                        WHERE s.Id IN (SELECT ShelfId FROM deleted WHERE ShelfId IS NOT NULL);

                        -- Update Category product count
                        UPDATE c
                        SET ProductCount = (
                            SELECT COUNT(*) 
                            FROM Products p 
                            WHERE p.CategoryId = c.Id
                        )
                        FROM Categories c
                        WHERE c.Id IN (SELECT CategoryId FROM deleted);
                    END
                END
            ");

            // Trigger for Shelf Changes
            migrationBuilder.Sql(@"
                CREATE TRIGGER TR_Shelfs_Changes
                ON Shelfs
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    -- If Shelf capacity or availability changes
                    IF UPDATE(Capacity) OR UPDATE(IsAvailable)
                    BEGIN
                        -- Update Shelf availability based on current products
                        UPDATE s
                        SET IsAvailable = 
                            CASE 
                                WHEN (SELECT COUNT(*) FROM Products WHERE ShelfId = s.Id) >= s.Capacity THEN 0 
                                ELSE 1 
                            END
                        FROM Shelfs s
                        WHERE s.Id IN (SELECT Id FROM inserted);
                    END
                END
            ");

            // Initialize ProductCount for existing categories
            migrationBuilder.Sql(@"
                UPDATE c
                SET ProductCount = (
                    SELECT COUNT(*) 
                    FROM Products p 
                    WHERE p.CategoryId = c.Id
                )
                FROM Categories c
            ");

            // Initialize Shelf availability based on capacity
            migrationBuilder.Sql(@"
                UPDATE s
                SET IsAvailable = 
                    CASE 
                        WHEN (SELECT COUNT(*) FROM Products WHERE ShelfId = s.Id) >= s.Capacity THEN 0 
                        ELSE 1 
                    END
                FROM Shelfs s
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS TR_Products_Changes");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS TR_Shelfs_Changes");
            
            migrationBuilder.DropColumn(
                name: "ProductCount",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Shelfs");
        }
    }
}
