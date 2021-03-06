using System.ComponentModel.DataAnnotations;

namespace CarRentingProject_Melvin.Models
{
    public class Gender
    {
        [Required]
        public char Id { get; set; }

        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }
    }
}
