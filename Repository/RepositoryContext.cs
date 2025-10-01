using Entities.Identity;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository
{
    public class RepositoryContext : IdentityDbContext<User, UserRole, string>
    {
        public RepositoryContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().ToTable(t => t.HasTrigger("tr_dbo_Products_b0792f02-af10-4efb-8d4d-cf1b736d8bff_Sender"));
            modelBuilder.Entity<Category>().ToTable(t => t.HasTrigger("tr_dbo_Category_b0792f02-af10-4efb-8d4d-cf1b736d8bff_Sender"));
            modelBuilder.Entity<Shelf>().ToTable(t => t.HasTrigger("tr_dbo_Shelf_b0792f02-af10-4efb-8d4d-cf1b736d8bff_Sender"));

            modelBuilder.Entity<Product>()
           .HasOne(d => d.Category)
           .WithMany(c => c.Products)
           .HasForeignKey(d => d.CategoryId)
           .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Product>()
                .HasOne(d => d.Warehouse)
                .WithMany(s => s.Products)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(s => s.Shelf)
                .WithMany(s => s.Products)
                .HasForeignKey(d => d.ShelfId)
                .OnDelete(DeleteBehavior.Restrict);           

            modelBuilder.Entity<Shelf>()
               .HasOne(d => d.Category)
               .WithMany(c => c.Shelfs)
               .HasForeignKey(d => d.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Shelf>()
               .HasOne(d => d.Warehouse)
               .WithMany(c => c.Shelfs)
               .HasForeignKey(d => d.Warehouse_Id)
               .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Shelf> Shelfs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
    }
}
