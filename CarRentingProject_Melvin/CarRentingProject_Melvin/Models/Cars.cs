using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentingProject_Melvin.Models
{
    public class Cars
    {
        public int Id { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }
        [Required]
        public long Mileage { get; set; }
        public int RenterId { get; set; }
        public Renter? Renter { get; set; }
    }
}
