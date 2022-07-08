using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using eTournament.Data.Cart;
using eTournament.Data.RequestReturnModels;
using eTournament.Data.Services;
using eTournament.Data.ViewModels;
using eTournament.Helpers;
using eTournament.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eTournament.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IMatchService _matchService;
        private readonly IOrdersService _ordersService;
        private readonly ShoppingCart _shoppingCart;
        private readonly Logic _logic = new();
        private HttpResponseMessage responseMessage = new();
        private Task<HttpResponseMessage> responseMessageNoAsync;
        private ShoppingCartVM shoppingCart;

        public OrdersController(IMatchService matchService, ShoppingCart shoppingCart, IOrdersService ordersService)
        {
            _matchService = matchService;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
        }

        public async Task<IActionResult> Index()
        {
            List<Order> orders = new List<Order>();

            responseMessage = await _logic.GetPostHttpClientAsync(
                false,
                true,
                "api/Orders/get_orders",
                null);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                orders = JsonConvert.DeserializeObject<List<Order>>(result);
            }
            
            return View(orders);
        }

        public ShoppingCartVM ShoppingCart()
        {
            var response = new ShoppingCartVM
            {
                ShoppingCart = shoppingCart.ShoppingCart,
                ShoppingCartTotal = shoppingCart.ShoppingCartTotal
            };

            return response;
        }

        public async Task<IActionResult> AddItemToShoppingCart(int id)
        {
            var reqtest = new RequestIdModel();
            var response = new ShoppingCartVM();

            reqtest.RequestId = id;
            responseMessage = await _logic.GetPostHttpClientAsync(
                false,
                false,
                "api/Orders/add_iterms_to_shopping_cart",
                reqtest);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ShoppingCartVM>(result);
            }

            shoppingCart = response;

            if (response != null)
                return RedirectToAction(nameof(ShoppingCart));
            else
                return RedirectToAction("Login", "Account");
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