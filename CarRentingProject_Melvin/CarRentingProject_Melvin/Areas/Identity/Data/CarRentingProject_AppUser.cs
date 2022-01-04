using CarRentingProject_Melvin.Models;
using Microsoft.AspNetCore.Identity;

namespace CarRentingProject_Melvin.Areas.Identity.Data;

// Add profile data for application users by adding properties to the CarRentingProject_AppUser class
public class CarRentingProject_AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public char GenderId { get; set; }
    public Gender? Gender { get; set; }
}
