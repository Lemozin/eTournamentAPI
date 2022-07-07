using System.Security.Claims;
using System.Threading.Tasks;
using eTournamentAPI.Data.Cart;
using eTournamentAPI.Data.Services;
using eTournamentAPI.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTournamentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
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

        [Authorize]
        [HttpGet]
        [Route("get_orders")]
        public async Task<IActionResult> Index()
        {
            var MatchId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(MatchId, userRole);
            return Ok(orders);
        }

        [Authorize]
        [HttpGet]
        [Route("get_shopping_cart")]
        public IActionResult ShoppingCart()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var response = new ShoppingCartVM
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [Route("add_iterms_to_shopping_cart")]
        public async Task<IActionResult> AddItemToShoppingCart(int id)
        {
            var item = await _matchService.GetMatchByIdAsync(id);

            if (item != null) _shoppingCart.AddItemToCart(item);
            return Ok("ItemAddSuccess");
        }

        [Authorize]
        [HttpDelete]
        [Route("remove_iterms_from_shopping_cart")]
        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await _matchService.GetMatchByIdAsync(id);

            if (item != null) _shoppingCart.RemoveItemFromCart(item);
            return Ok("ItemRemoveSuccess");
        }

        [Authorize]
        [HttpPost]
        [Route("complete_order")]
        public async Task<IActionResult> CompleteOrder()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            var MatchId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

            await _ordersService.StoreOrderAsync(items, MatchId, userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync();

            return Ok("OrderCompleted");
        }
    }
}
