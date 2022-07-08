using System;
using eTournament.Data.Cart;
using Microsoft.AspNetCore.Mvc;

namespace eTournament.Data.ViewComponents
{
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartSummary(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            try
            {
                var items = _shoppingCart.GetShoppingCartItems();

                return View(items.Count);
            }
            catch (Exception e) { }

            return View(0);
        }
    }
}