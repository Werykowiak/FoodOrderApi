using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodOrderApi.Models;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderPositionController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public OrderPositionController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderPosition
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderPositionModel>>> GetOrderPositionModel()
        {
            return await _context.OrderPositionModel.ToListAsync();
        }

        // GET: api/OrderPosition/5/GetById
        [HttpGet("{id}/GetById")]
        public async Task<ActionResult<OrderPositionModel>> GetOrderPositionModel(int id)
        {
            var orderPositionModel = await _context.OrderPositionModel.FindAsync(id);

            if (orderPositionModel == null)
            {
                return NotFound();
            }

            return orderPositionModel;
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderPositionModel>>> GetOrderPositionsByOrderId(int orderId)
        {
            var orderPositionModel = await _context.OrderPositionModel
                .Where(x => x.OrderId == orderId)
                .ToListAsync();

            if (orderPositionModel == null)
            {
                return NotFound();
            }

            return orderPositionModel;
        }
        [HttpGet("{orderId}/GetUserPositionsForOrder/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderPositionModel>>> GetUserPositionForOrder(int orderId, string userId)
        {
            var orderPositionModel = await _context.OrderPositionModel
                .Where(x => x.OrderId == orderId && x.UserId == userId)
                .ToListAsync();

            if (orderPositionModel == null)
            {
                return NotFound();
            }

            return orderPositionModel;
        }

        

        // PUT: api/OrderPosition/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderPositionModel(int id, OrderPositionModel orderPositionModel)
        {
            if (id != orderPositionModel.Id)
            {
                return BadRequest();
            }
            var oldCost = _context.OrderPositionModel.Find(id).Cost;
            _context.ChangeTracker.Clear();
            //var oldCost = oldPosition.Cost;
            
            _context.Entry(orderPositionModel).State = EntityState.Modified;

            try
            {
                var order = await _context.OrderModel.FindAsync(orderPositionModel.OrderId);
                if (order != null)
                {
                    var newCost = orderPositionModel.Cost - oldCost;
                    order.CurrentCost += newCost;
                }
                //oldPosition = orderPositionModel;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderPositionModelExists(id))
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
        [HttpPut("{id}/SetIsPaid/{isPaid}")]
        public async Task<IActionResult> SetIsPaid(int id,bool isPaid)
        {

            var orderPositionModel = await _context.OrderPositionModel.FindAsync(id);
            orderPositionModel.IsPaid = isPaid;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderPositionModelExists(id))
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
        // POST: api/OrderPosition
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderPositionModel>> PostOrderPositionModel(OrderPositionModel orderPositionModel)
        {
            _context.OrderPositionModel.Add(orderPositionModel);
            var order = await _context.OrderModel.FindAsync(orderPositionModel.OrderId);
            if (order != null)
            {
                order.CurrentCost += orderPositionModel.Cost;
                //_context.Entry(orderPositionModel).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetOrderPositionModel), new { id = orderPositionModel.Id }, orderPositionModel);
        }

        // DELETE: api/OrderPosition/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderPositionModel(int id)
        {
            var orderPositionModel = await _context.OrderPositionModel.FindAsync(id);
            if (orderPositionModel == null)
            {
                return NotFound();
            }
            var order = await _context.OrderModel.FindAsync(orderPositionModel.OrderId);
            if (order != null)
            {
                order.CurrentCost -= orderPositionModel.Cost;
            }
            _context.OrderPositionModel.Remove(orderPositionModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderPositionModelExists(int id)
        {
            return _context.OrderPositionModel.Any(e => e.Id == id);
        }
    }
}
