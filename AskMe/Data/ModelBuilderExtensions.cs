using AskMe.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AskMe.Data
{
    public static class ModelBuilderExtensions
    {
        public static void SeedRole(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
                new IdentityRole { Name = "Consumer", NormalizedName = "CONSUMER" }
            );
        }

        public static void SeedCateogory(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Name = "Health", CId = 1},
                new Category { Name = "Technology", CId = 2},
                new Category { Name = "Automotive", CId = 3},
                new Category { Name = "Art", CId = 4},
                new Category { Name = "Education", CId = 5},
                new Category { Name = "Career", CId = 6},
                new Category { Name = "Politics", CId= 7}
            );
        }
    }
}