using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Models
{
    public class ApiDbContext: DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
        public DbSet<OrderModel> orders;
        public DbSet<OrderPositionModel> positions;
    }
}
