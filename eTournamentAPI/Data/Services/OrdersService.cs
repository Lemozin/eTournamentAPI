using eTournamentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace eTournamentAPI.Data.Services;

public class OrdersService : IOrdersService
{
    private readonly AppDbContext _context;

    public OrdersService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string Email, string userRole)
    {
        var orders = await _context.Orders
            .ToListAsync();

        if (userRole != "Admin") orders = orders.Where(n => n.Email == Email).ToList();

        return orders;
    }

    public async Task StoreOrderAsync(List<ShoppingCartItem> items, string UserId, string userEmailAddress,
        List<OrderItem> outOrderItems)
    {
        var order = new Order
        {
            Email = userEmailAddress
        };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        foreach (var item in items)
        {
            var orderItem = new OrderItem
            {
                Amount = item.Amount,
                MatchId = item.Match.Id,
                OrderId = order.Id,
                Price = item.Match.Price
            };
            outOrderItems.Add(orderItem);
            await _context.OrderItems.AddAsync(orderItem);
        }

        await _context.SaveChangesAsync();
    }
}