using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTournamentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eTournamentAPI.Data.Cart
{
    public class ShoppingCart
    {
        public ShoppingCart(AppDbContext context)
        {
            _context = context;
        }

        public AppDbContext _context { get; set; }

        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            var session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<AppDbContext>();

            var cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddItemToCart(Match match)
        {
            var shoppingCartItem =
                _context.ShoppingCartItems.FirstOrDefault(n =>
                    n.Match.Id == match.Id && n.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Match = match,
                    Amount = 1
                };

                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            _context.SaveChanges();
        }

        public void RemoveItemFromCart(Match match)
        {
            var shoppingCartItem =
                _context.ShoppingCartItems.FirstOrDefault(n =>
                    n.Match.Id == match.Id && n.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                    shoppingCartItem.Amount--;
                else
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
            }

            _context.SaveChanges();
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? (ShoppingCartItems = _context.ShoppingCartItems
                .Where(n => n.ShoppingCartId == ShoppingCartId).Include(n => n.Match).ToList());
        }

        public double GetShoppingCartTotal()
        {
            return _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId)
                .Select(n => n.Match.Price * n.Amount).Sum();
        }

        public async Task ClearShoppingCartAsync()
        {
            var items = await _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
            _context.ShoppingCartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
