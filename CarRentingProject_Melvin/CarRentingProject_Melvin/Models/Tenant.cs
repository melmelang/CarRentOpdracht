using System.ComponentModel.DataAnnotations;

namespace CarRentingProject_Melvin.Models
{
    public class Tenant
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
        public DateTime Birthday { get; set; }
        public char GenderId { get; set; }
        public Gender? Gender { get; set; }
    }
}
