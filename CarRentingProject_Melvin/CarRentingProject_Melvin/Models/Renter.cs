using Studentenbeheer.Models;
using System.ComponentModel.DataAnnotations;

namespace CarRentingProject_Melvin.Models
{
    public class Renter
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly Birthday { get; set; }
        [Required]
        public char GenderId { get; set; }
        public Gender? Gender { get; set; }
    }
}
