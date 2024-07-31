using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodOrderApi.Models;
using FoodOrderApi.Hubs;
using Microsoft.AspNet.SignalR;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderModelsController : ControllerBase
    {
        private readonly ApiDbContext _context;
        //private readonly IHubContext<OrderModelHub> _hubContext;

        public OrderModelsController(ApiDbContext context)
        {
            _context = context;
            //_hubContext = hubContext;
        }

        // GET: api/OrderModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrderModel()
        {
            return await _context.OrderModel.ToListAsync();
        }

        [HttpGet("GetAllOrdersId")]
        public async Task<ActionResult<IEnumerable<int?>>> GetAllOrdersId()
        {
            return await _context.OrderModel.Select(x => x.Id).ToListAsync();
        }

        // GET: api/OrderModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetOrderModel(int? id)
        {
            var orderModel = await _context.OrderModel.FindAsync(id);

            if (orderModel == null)
            {
                return NotFound();
            }

            return orderModel;
        }
        // GET: api/OrderModels/5
        [HttpGet("{id}/GetNumberOfDistinctPositions")]
        public async Task<ActionResult<int>> GetNumberOfDistinctPositions(int? id)
        {
            var orderModel = _context.OrderPositionModel
                .Where(x => x.OrderId ==  id)
                .Select(x => x.UserId )
                .Distinct()
                .Count();

            if (orderModel == null)
            {
                return NotFound();
            }

            return orderModel;
        }

        [HttpGet("{orderId}/GetUserCostForOrder/{userId}")]
        public async Task<ActionResult<double>> GetUserCostForOrder(int orderId, string userId)
        {
            var result = await GetOrderModel(orderId);
            var order = result.Value;
            var UserOrdersCosts = await _context.OrderPositionModel
                .Where(x => x.OrderId == orderId && x.UserId == userId)
                .ToListAsync();
            var totalCost = UserOrdersCosts.Sum(x => x.Cost);
            if (UserOrdersCosts == null)
            {
                return NotFound();
            }
            if(order.CurrentCost < order.MinCostForFreeDelivery || order.MinCostForFreeDelivery==null)
            {
                var numberOfUsers = await GetNumberOfDistinctPositions(orderId);
                totalCost += (order.DeliveryFee/numberOfUsers.Value);
            }
            if (order.CurrentCost == 0)
                totalCost = 0;



            return totalCost;
        }
        // PUT: api/OrderModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderModel(int? id, OrderModel orderModel)
        {
            if (id != orderModel.Id)
            {
                
                return BadRequest();
            }

            _context.Entry(orderModel).State = EntityState.Modified;
            /*var oldModel = await _context.OrderModel.FindAsync(id);
            oldModel = */
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderModelExists(id))
                {
                    Console.WriteLine("400");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPut("{id}/ChangeState/{isClosed}")]
        public async Task<IActionResult> ChangeState(int? id,bool isClosed)
        {
            var orderModel = await _context.OrderModel.FindAsync(id);
            orderModel.IsClosed = isClosed;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // POST: api/OrderModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderModel>> PostOrderModel(OrderModel orderModel)
        {
            if (orderModel.CurrentCost == null)
                orderModel.CurrentCost = 0;
            //Console.WriteLine("dodawanie");
            _context.OrderModel.Add(orderModel);

            await _context.SaveChangesAsync();

            //await _hubContext.Clients.All.SendAsync

            return CreatedAtAction(nameof(GetOrderModel), new { id = orderModel.Id }, orderModel);
        }

        // DELETE: api/OrderModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderModel(int? id)
        {
            var orderModel = await _context.OrderModel.FindAsync(id);
            if (orderModel == null)
            {
                return NotFound();
            }
            var orderPositionModel = await _context.OrderPositionModel
                .Where(x => x.OrderId == orderModel.Id)
                .ToListAsync();
            foreach (var orderPosition in orderPositionModel)
            {
                _context.OrderPositionModel.Remove(orderPosition);
            }
            _context.OrderModel.Remove(orderModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderModelExists(int? id)
        {
            return _context.OrderModel.Any(e => e.Id == id);
        }
    }
}
