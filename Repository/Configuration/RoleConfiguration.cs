using Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasData(
              new UserRole
              {
                  Id = "eea44dd0-d656-44b4-8f1e-aaa18f40ab1e",
                  Name = "Admin",
                  NormalizedName = "ADMIN",
                  DateCreated = new DateTime(2024, 10, 13),
              },
              new UserRole
              {
                  Id = "310754fe-2c04-4f63-9e3e-348f0e988990",
                  Name = "User",
                  NormalizedName = "USER",
                  DateCreated = new DateTime(2024, 10, 13),
              },
              new UserRole
              {
                  Id = "b327e659-d135-4faa-a43d-f02b743f9872",
                  Name = "Data Entry",
                  NormalizedName = "DATA ENTRY",
                  DateCreated = new DateTime(2024, 10, 13),
              }
            );
        }
    }
}