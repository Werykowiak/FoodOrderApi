using System.ComponentModel.DataAnnotations;

namespace FoodOrderApi.Models
{
    public class OrderModel
    {
        [Key]
        public int? Id { get; set; }
        public string Orderer { get; set; }
        [Required]
        public string? RestaurantName { get; set; }
        [Required]
        public double? MinCost { get; set; }
        [Required]
        public double? DeliveryFee { get; set; }
        public double? MinCostForFreeDelivery { get; set; }
        [Required]
        public int? PhoneNumber { get; set; }
        [Required]
        public int? AccountNumber { get; set; }
        //public ICollection<OrderPosition> Positions { get; set; }
    }
}
