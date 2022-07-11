using eTournamentAPI.Models;

namespace eTournamentAPI.Data.Services;

public interface IOrdersService
{
    Task StoreOrderAsync(List<ShoppingCartItem> items, string MatchId, string userEmailAddress,
        List<OrderItem> outOrderItems);

    Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string MatchId, string userRole);
}