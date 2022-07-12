using eTournamentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace eTournamentAPI.Data.Cart;

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
                Amount = 1,
                Status = 0
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
                n.Match.Id == match.Id && n.Status == 0);

        if (shoppingCartItem != null)
        {
            if (shoppingCartItem.Amount > 1)
            {
                shoppingCartItem.Status = 1;
                shoppingCartItem.Amount--;
            }
            else
            {
                _context.ShoppingCartItems.Remove(shoppingCartItem);
            }
        }

        _context.SaveChanges();
    }

    public List<ShoppingCartItem> GetShoppingCartItems()
    {
        var shoppingCartItems = (from sp in _context.ShoppingCartItems
            where sp.Status == 0
            select sp).Include(n => n.Match).ToList();

        return shoppingCartItems;
    }

    public EmailSMTPCredentials GetEmailSmtpCredentials()
    {
        var emailSmtpCredentials = (from sp in _context.EmailSmtpCredentials
            select sp).FirstOrDefault();

        return emailSmtpCredentials;
    }

    public double GetShoppingCartTotal()
    {
        return _context.ShoppingCartItems.Where(n => n.Status == 0)
            .Select(n => n.Match.Price * n.Amount).Sum();
    }

    public async Task ClearShoppingCartAsync(string Email)
    {
        var items = await _context.ShoppingCartItems.Where(n => n.Status == 0).ToListAsync();
        _context.ShoppingCartItems.RemoveRange(items);

        var orders = await _context.Orders.ToListAsync();

        orders = orders.Where(n => n.Email == Email).ToList();

        if (orders != null)
        {
            foreach (var order in orders) order.Status = 1;
        }

        await _context.SaveChangesAsync();
    }
}