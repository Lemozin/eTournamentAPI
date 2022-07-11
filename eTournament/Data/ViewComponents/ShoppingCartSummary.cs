using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using eTournament.Data.Cart;
using eTournament.Data.Enums;
using eTournament.Helpers;
using eTournament.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eTournament.Data.ViewComponents
{
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly ShoppingCart _shoppingCart;
        private Task<HttpResponseMessage> responseMessageNoAsync;
        private readonly Logic _logic = new();

        public ShoppingCartSummary(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            var items = new List<ShoppingCartItem>();

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            responseMessageNoAsync = _logic.GetPostHttpClient(
                RequestMethods.GET,
                true,
                true,
                "api/Orders/get_shopping_cart",
                null,
                token);

            if (responseMessageNoAsync.Result.IsSuccessStatusCode)
            {
                var result = responseMessageNoAsync.Result.Content.ReadAsStringAsync().Result;
                items = JsonConvert.DeserializeObject<List<ShoppingCartItem>>(result);
            }

            return View(items.Count);
        }
    }
}