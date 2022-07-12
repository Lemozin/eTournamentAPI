using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using eTournament.Data.Cart;
using eTournament.Data.Enums;
using eTournament.Data.RequestReturnModels;
using eTournament.Data.ReturnModels;
using eTournament.Data.ViewModels;
using eTournament.Helpers;
using eTournament.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace eTournament.Controllers
{
    public class OrdersController : Controller
    {
        private readonly Logic _logic = new();
        private readonly ShoppingCart _shoppingCart;
        private HttpResponseMessage responseMessage = new();
        private Task<HttpResponseMessage> responseMessageNoAsync;

        public OrdersController(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var orders = new List<Order>();

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            if (token != null) TempData["Token"] = token;

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.GET,
                true,
                true,
                "api/Orders/get_orders",
                null,
                token);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                orders = JsonConvert.DeserializeObject<List<Order>>(result);
            }

            return View(orders.ToPagedList(page, 4));
        }

        public async Task<IActionResult> ShoppingCart()
        {
            var items = new List<ShoppingCartItem>();

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");
            var shoppingCartTotal = 0.0;

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");
            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.GET,
                true,
                true,
                "api/Orders/get_shopping_cart",
                null,
                token);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                items = JsonConvert.DeserializeObject<List<ShoppingCartItem>>(result);
            }

            _shoppingCart.ShoppingCartItems = items.ToList();

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.GET,
                true,
                true,
                "api/Orders/get_shopping_cart_total",
                null,
                token);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                shoppingCartTotal = JsonConvert.DeserializeObject<double>(result);
            }

            var response = new ShoppingCartVM
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = shoppingCartTotal
            };

            return View(response);
        }

        public async Task<IActionResult> AddItemToShoppingCart(int id)
        {
            var reqtest = new RequestIdModel();
            var response = new ShoppingCartVM();

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            reqtest.RequestId = id;
            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                true,
                false,
                "api/Orders/add_iterms_to_shopping_cart",
                reqtest,
                token);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ShoppingCartVM>(result);
            }

            if (response != null)
                return RedirectToAction(nameof(ShoppingCart));
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var reqtest = new RequestIdModel();
            var response = new ReturnString();

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            reqtest.RequestId = id;
            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                true,
                false,
                "api/Orders/remove_iterms_from_shopping_cart",
                reqtest,
                token);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ReturnString>(result);
            }

            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> CompleteOrder()
        {
            var items = new List<ShoppingCartItem>();
            var response = new ReturnString();

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");
            var email = HttpContext.Session.GetString("Email");
            var shoppingCartTotal = 0.0;

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.GET,
                true,
                true,
                "api/Orders/get_shopping_cart",
                null,
                token);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                items = JsonConvert.DeserializeObject<List<ShoppingCartItem>>(result);
            }

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                true,
                true,
                "api/Orders/complete_order",
                null,
                token);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ReturnString>(result);
            }

            return View("OrderCompleted");
        }
    }
}