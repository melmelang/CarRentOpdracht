using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CarRentingProject_Melvin.Areas.Identity.Data;
using CarRentingProject_Melvin.Models;

namespace CarRentingProject_Melvin.Data
{
    public class DatabaseSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider, UserManager<CarRentingProject_AppUser> userManager)
        {
            using (var context = new DBContext(
                serviceProvider.GetRequiredService<DbContextOptions<DBContext>>()))
            {
                context.Database.EnsureCreated();

                if (!context.Gender.Any())
                {
                    context.Gender.AddRange(

                        new Gender
                        {
                            Id = 'M',
                            Name = "Man"
                        },

                        new Gender
                        {
                            Id = 'V',
                            Name = "Vrouw"
                        },

                        new Gender
                        {
                            Id = '-',
                            Name = "None"
                        }

                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
