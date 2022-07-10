using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Logic _logic = new();
        private readonly ShoppingCart _shoppingCart;
        private Task<HttpResponseMessage> responseMessage;

        public ShoppingCartSummary(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            try
            {
                var items = new List<ShoppingCartItem>();
                var username = HttpContext.Session.GetString("Username");
                var role = HttpContext.Session.GetString("Role");
                var shoppingCartTotal = 0.0;

                TempData["Username"] = username;
                TempData["Role"] = role;

                var token = HttpContext.Session.GetString("Token");
                responseMessage = _logic.GetPostHttpClient(
                    RequestMethods.GET,
                    true,
                    true,
                    "api/Orders/get_shopping_cart",
                    null,
                    token);

                if (responseMessage.Result.IsSuccessStatusCode)
                {
                    var result = responseMessage.Result.Content.ReadAsStringAsync().Result;
                    items = JsonConvert.DeserializeObject<List<ShoppingCartItem>>(result);
                }

                return View(items.Count());
            }
            catch (Exception e)
            {
            }

            return View(0);
        }
    }
}