using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOrderApi.Migrations
{
    /// <inheritdoc />
    public partial class _0807 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Orderer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RestaurantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinCost = table.Column<double>(type: "float", nullable: false),
                    DeliveryFee = table.Column<double>(type: "float", nullable: false),
                    MinCostForFreeDelivery = table.Column<double>(type: "float", nullable: true),
                    PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderPositionModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Additives = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPositionModel", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderModel");

            migrationBuilder.DropTable(
                name: "OrderPositionModel");
        }
    }
}
