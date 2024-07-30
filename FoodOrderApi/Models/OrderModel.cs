using System.ComponentModel.DataAnnotations;

namespace FoodOrderApi.Models
{
    public class OrderModel
    {
        [Key]
        public int? Id { get; set; }
        public string Orderer { get; set; }
        public string OrdererName { get; set; }
        [Required]
        public string? RestaurantName { get; set; }
        [Required]
        public double? MinCost { get; set; }
        public double? CurrentCost { get; set; }
        [Required]
        public double? DeliveryFee { get; set; }
        public double? MinCostForFreeDelivery { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? AccountNumber { get; set; }
        public bool IsClosed { get; set; }
        //public ICollection<OrderPosition> Positions { get; set; }
    }
}
