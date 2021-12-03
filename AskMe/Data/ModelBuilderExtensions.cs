using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AskMe.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
                new IdentityRole { Name = "Consumer", NormalizedName = "CONSUMER" }
            );
        }
    }
}