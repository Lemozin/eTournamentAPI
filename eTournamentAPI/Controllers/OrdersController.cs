using System.Security.Claims;
using eTournamentAPI.Data.Cart;
using eTournamentAPI.Data.RequestReturnModels;
using eTournamentAPI.Data.ReturnModels;
using eTournamentAPI.Data.Services;
using eTournamentAPI.Data.ViewModels;
using eTournamentAPI.Helpers;
using eTournamentAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTournamentAPI.Controllers;

/// <summary>
///     Orders controller responsible for adding order to cart, removing order from cart and listing orders
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class OrdersController : ControllerBase
{
    private readonly IMatchService _matchService;
    private readonly IOrdersService _ordersService;
    private readonly ShoppingCart _shoppingCart;
    private readonly IEmailService _emailService;
    private readonly Logic _logic = new();

    public OrdersController(IMatchService matchService, ShoppingCart shoppingCart, IOrdersService ordersService, IEmailService emailService)
    {
        _matchService = matchService;
        _shoppingCart = shoppingCart;
        _ordersService = ordersService;
        _emailService = emailService;
    }

    /// <summary>
    ///     Get list of orders
    /// </summary>
    /// <returns>
    ///     Returns list of orders
    /// </returns>
    [HttpGet]
    [Route("get_orders")]
    public async Task<IActionResult> Index()
    {
        var MatchId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(MatchId, userRole);
        return Ok(orders);
    }

    /// <summary>
    ///     Get list of shopping cart items
    /// </summary>
    /// <returns>
    ///     Returns an object of shopping cart to view
    /// </returns>
    [HttpGet]
    [Route("get_shopping_cart")]
    public IActionResult ShoppingCart()
    {
        var items = _shoppingCart.GetShoppingCartItems();

        return Ok(items);
    }

    /// <summary>
    ///     Gets shopping cart total
    /// </summary>
    /// <returns>
    ///     Returns shopping cart total
    /// </returns>
    [HttpGet]
    [Route("get_shopping_cart_total")]
    public IActionResult ShoppingCartTotal()
    {
        var ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal();

        return Ok(ShoppingCartTotal);
    }

    /// <summary>
    ///     Add items to shopping cart by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    ///     Returns shopping cart if added successfully and not found if item was not found
    /// </returns>
    [HttpPost]
    [Route("add_iterms_to_shopping_cart")]
    public async Task<IActionResult> AddItemToShoppingCart(RequestIdModel id)
    {
        var item = await _matchService.GetMatchByIdAsync(id.RequestId);
        var response = new ShoppingCartVM();

        if (item != null) _shoppingCart.AddItemToCart(item);
        else
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    ///     Remove item from shopping cart
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    ///     Returns "ItemRemoveSuccess" if removed successfully
    /// </returns>
    [HttpPost]
    [Route("remove_iterms_from_shopping_cart")]
    public async Task<IActionResult> RemoveItemFromShoppingCart(RequestIdModel id)
    {
        var response = new ReturnString();

        var item = await _matchService.GetMatchByIdAsync(id.RequestId);

        if (item != null) _shoppingCart.RemoveItemFromCart(item);

        response.ReturnMessage = "ItemRemoveSuccess";

        return Ok(response);
    }

    /// <summary>
    ///     Clears item from cart
    /// </summary>
    /// <returns>
    ///     Returns "OrderCompleted" if cleared successfully
    /// </returns>
    [HttpPost]
    [Route("complete_order")]
    public async Task<IActionResult> CompleteOrder()
    {
        IEnumerable<EmailSMTPCredentials> emailCredential = new List<EmailSMTPCredentials>();
        var body = string.Empty;
        var port = 0;
        var host = string.Empty;
        var usernameSMTP = string.Empty;
        var passwordSMTP = string.Empty;
        var items = _shoppingCart.GetShoppingCartItems();
        var MatchId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

        await _ordersService.StoreOrderAsync(items, MatchId, userEmailAddress);
        await _shoppingCart.ClearShoppingCartAsync();

        emailCredential = await _emailService.GetAllAsync();
        foreach (var credetial in emailCredential)
        {
            port = credetial.Port;
            host = credetial.Host;
            usernameSMTP = credetial.Username;
            passwordSMTP = credetial.Password;
        }

        _logic.SendCompletedOrderEmail(
            "New orders created",
            userEmailAddress,
            port,
            host,
            usernameSMTP,
            passwordSMTP);

        return Ok("OrderCompleted");
    }
}