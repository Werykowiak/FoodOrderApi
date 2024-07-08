using Microsoft.EntityFrameworkCore;
using FoodOrderApi.Models;

namespace FoodOrderApi.Models
{
    public class ApiDbContext: DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
        public DbSet<OrderPositionModel> positions;
        public DbSet<FoodOrderApi.Models.OrderModel> OrderModel { get; set; } = default!;
        public DbSet<FoodOrderApi.Models.OrderPositionModel> OrderPositionModel { get; set; } = default!;
    }
}
