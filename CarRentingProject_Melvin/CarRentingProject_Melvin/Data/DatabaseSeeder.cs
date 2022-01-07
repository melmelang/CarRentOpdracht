using CarRentingProject_Melvin.Areas.Identity.Data;
using CarRentingProject_Melvin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRentingProject_Melvin.Data
{
    public class DatabaseSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider, UserManager<CarRentingProject_AppUser> userManager)
        {
            using (var context = new DBContext(
                serviceProvider.GetRequiredService<DbContextOptions<DBContext>>()))
            {
                CarRentingProject_AppUser user = null;
                CarRentingProject_AppUser user2 = null;
                CarRentingProject_AppUser user3 = null;
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
                            Id = 'W',
                            Name = "Woman"
                        },

                        new Gender
                        {
                            Id = '-',
                            Name = "None"
                        }

                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    user = new CarRentingProject_AppUser
                    {
                        FirstName = "Melvin",
                        LastName = "Angeli",
                        UserName = "Melvin.Angeli",
                        Email = "Angeli.melvin@hotmail.com",
                        GenderId = '-',
                        Birthday = DateTime.Now,
                        EmailConfirmed = true,
                    };
                    user2 = new CarRentingProject_AppUser
                    {
                        FirstName = "Antoine",
                        LastName = "Couck",
                        UserName = "Antoine.Couck",
                        Email = "Angeli.melvin@hotmail.com",
                        GenderId = 'M',
                        Birthday = DateTime.Now,
                        EmailConfirmed = true,
                    };
                    user3 = new CarRentingProject_AppUser
                    {
                        FirstName = "Ine",
                        LastName = "DeBast",
                        UserName = "Ine.DeBast",
                        Email = "Angeli.melvin@hotmail.com",
                        GenderId = 'W',
                        Birthday = DateTime.Now,
                        EmailConfirmed = true,
                    };
                    userManager.CreateAsync(user, "Student+1");
                    userManager.CreateAsync(user2, "Student+1");
                    userManager.CreateAsync(user3, "Student+1");
                }

                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new IdentityRole { Id = "Admin", Name = "Admin", NormalizedName = "admin" },
                        new IdentityRole { Id = "Renter", Name = "Renter", NormalizedName = "renter" },
                        new IdentityRole { Id = "Tenant", Name = "Tenant", NormalizedName = "tenant" }
                        );
                    context.SaveChanges();
                }

                if (!context.Renter.Any())
                {
                    context.Renter.AddRange(
                        new Renter
                        {
                            FirstName = user2.FirstName,
                            LastName = user2.LastName,
                            UserName = user2.UserName,
                            Birthday = user2.Birthday,
                            GenderId = user2.GenderId,
                            UserId = user2.Id
                        }
                        );
                    context.SaveChanges();
                }
                if (!context.Tenant.Any())
                {
                    context.Tenant.AddRange(
                        new Tenant
                        {
                            FirstName = user3.FirstName,
                            LastName = user3.LastName,
                            UserName = user3.UserName,
                            Birthday = user3.Birthday,
                            GenderId = user3.GenderId,
                            UserId = user3.Id
                        }
                        );
                    context.SaveChanges();
                }
                if (!context.Cars.Any())
                {
                    context.Cars.AddRange(
                        new Cars
                        {
                            Brand = "Peugot",
                            Model = "3001",
                            Price = 500,
                            ProductionDate = DateTime.Now,
                            Mileage = 15462,
                            RenterId = 1
                        }
                        );
                    context.SaveChanges();
                }
                if (!context.RentedCars.Any())
                {
                    context.RentedCars.AddRange(
                        new RentedCars
                        {
                            CarsId = 1,
                            TenantId = 1,
                            RideTime = DateTime.MaxValue
                        }
                        );
                    context.SaveChanges();
                }
                if (!context.UserRoles.Any())
                {
                    context.UserRoles.AddRange(
                        new IdentityUserRole<string> { RoleId = "Admin", UserId = user.Id },
                        new IdentityUserRole<string> { RoleId = "Renter", UserId = user2.Id },
                        new IdentityUserRole<string> { RoleId = "Tenant", UserId = user3.Id }
                        );
                    context.SaveChanges();
                }
            }
        }
    }
}
