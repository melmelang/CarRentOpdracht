using System.ComponentModel.DataAnnotations;

namespace CarRentingProject_Melvin.Models
{
    public class RentedCars
    {
        public int Id { get; set; }
        public int CarsId { get; set; }
        public Cars? Cars { get; set; }
        public int? TenantId { get; set; }
        public Tenant? Tenant { get; set; }
        [Required]
        public DateTime RideTime { get; set; }
    }
}
