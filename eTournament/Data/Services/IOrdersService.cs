using System.Collections.Generic;
using System.Threading.Tasks;
using eTournament.Models;

namespace eTournament.Data.Services
{
    public interface IOrdersService
    {
        Task StoreOrderAsync(List<ShoppingCartItem> items, string MatchId, string userEmailAddress);
        Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string MatchId, string userRole);
    }
}