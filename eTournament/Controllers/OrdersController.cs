using System.Security.Claims;
using System.Threading.Tasks;
using eTournament.Data.Cart;
using eTournament.Data.Services;
using eTournament.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eTournament.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IMatchService _matchService;
        private readonly IOrdersService _ordersService;
        private readonly ShoppingCart _shoppingCart;

        public OrdersController(IMatchService matchService, ShoppingCart shoppingCart, IOrdersService ordersService)
        {
            _matchService = matchService;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
        }

        public async Task<IActionResult> Index()
        {
            var MatchId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(MatchId, userRole);
            return View(orders);
        }

        public IActionResult ShoppingCart()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var response = new ShoppingCartVM
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View(response);
        }

        public async Task<IActionResult> AddItemToShoppingCart(int id)
        {
            var item = await _matchService.GetMatchByIdAsync(id);

            if (item != null) _shoppingCart.AddItemToCart(item);
            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await _matchService.GetMatchByIdAsync(id);

            if (item != null) _shoppingCart.RemoveItemFromCart(item);
            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> CompleteOrder()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            var MatchId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

            await _ordersService.StoreOrderAsync(items, MatchId, userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync();

            return View("OrderCompleted");
        }
    }
}