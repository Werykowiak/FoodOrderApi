using System.ComponentModel.DataAnnotations;

namespace FoodOrderApi.Models
{
    public class OrderPositionModel
    {
        [Key]
        public int? Id { get; set; }
        public string UserId { get; set; }
        public string Position { get; set; }
        public string? Comment { get; set; }
        public string? Additives { get; set; }
        public double? Cost { get; set; }
        public int OrderId { get; set; }
        public bool IsPaid {  get; set; }
    }
}
