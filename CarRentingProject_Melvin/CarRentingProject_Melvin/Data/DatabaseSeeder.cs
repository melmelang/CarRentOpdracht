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
                CarRentingProject_AppUser admin = null;
                CarRentingProject_AppUser renter = null;
                context.Database.EnsureCreated();

                if (!context.Languages.Any())
                {
                    context.Languages.AddRange(
                        new Language() { AppLangId = "-", AppLangName = "-", AppCultures = "-", AppIsSystemLang = false },
                        new Language() { AppLangId = "fr", AppLangName = "Français", AppCultures = "BE;FR", AppIsSystemLang = true },
                        new Language() { AppLangId = "nl", AppLangName = "Nederlands", AppCultures = "BE;NL", AppIsSystemLang = true },
                        new Language() { AppLangId = "en", AppLangName = "English", AppCultures = "UK;US", AppIsSystemLang = true }
                        );
                }

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
                    CarRentingProject_AppUser dummy = new CarRentingProject_AppUser
                    {
                        Id = "-",
                        FirstName = "-",
                        LastName = "-",
                        UserName = "-",
                        Email = "?@?.?",
                        GenderId = '-',
                        AcceptCookie = false,
                        Birthday = DateTime.MinValue,
                        AppLangId = "-"
                    };
                    context.Users.Add(dummy);
                    context.SaveChanges();
                    admin = new CarRentingProject_AppUser
                    {
                        FirstName = "Melvin",
                        LastName = "Angeli",
                        UserName = "Melvin.Angeli",
                        Email = "Angeli.melvin@hotmail.com",
                        GenderId = '-',
                        AcceptCookie = false,
                        Birthday = DateTime.Now,
                        EmailConfirmed = true,
                        AppLangId = "fr"
                    };
                    renter = new CarRentingProject_AppUser
                    {
                        FirstName = "Antoine",
                        LastName = "Couck",
                        UserName = "Antoine.Couck",
                        Email = "Angeli.melvin@hotmail.com",
                        GenderId = 'M',
                        AcceptCookie = false,
                        Birthday = DateTime.Now,
                        EmailConfirmed = true,
                        AppLangId = "nl"
                    };
                    userManager.CreateAsync(admin, "Student+1");
                    userManager.CreateAsync(renter, "Student+1");
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
                            FirstName = renter.FirstName,
                            LastName = renter.LastName,
                            UserName = renter.UserName,
                            Birthday = renter.Birthday,
                            GenderId = renter.GenderId,
                            UserId = renter.Id
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
                if (!context.UserRoles.Any())
                {
                    context.UserRoles.AddRange(
                        new IdentityUserRole<string> { RoleId = "Admin", UserId = admin.Id },
                        new IdentityUserRole<string> { RoleId = "Renter", UserId = renter.Id }
                        );
                    context.SaveChanges();
                }

                List<string> supportedLanguages = new List<string>();
                Language.AppAllLang = context.Languages.ToList();
                Language.AppLangWiki = new Dictionary<string, Language>();
                Language.AppSystemLang = new List<Language>();

                supportedLanguages.Add("nl-BE");
                foreach (Language l in Language.AppAllLang)
                {

                    // key not found = ligne en dessous mettre au dessus du if 
                    Language.AppLangWiki[l.AppLangId] = l;


                    if (l.AppLangId != "-")
                    {

                        if (l.AppIsSystemLang)
                            Language.AppSystemLang.Add(l);
                        supportedLanguages.Add(l.AppLangId);
                        string[] even = l.AppCultures.Split(";");
                        foreach (string e in even)
                        {
                            supportedLanguages.Add(l.AppLangId + "-" + e);
                        }
                    }
                }
                Language.AppSuppLang = supportedLanguages.ToArray();
            }
        }
    }
}
