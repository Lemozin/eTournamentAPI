using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using eTournamentAPI.Data;
using eTournamentAPI.Data.ReturnModels;
using eTournamentAPI.Data.Static;
using eTournamentAPI.Data.ViewModels;
using eTournamentAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace eTournamentAPI.Controllers;

/// <summary>
///     account controller which deals with users(login,register etc)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _context;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        AppDbContext context,
        IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _config = config;
    }

    /// <summary>
    ///     Get list of all users
    /// </summary>
    /// <returns>
    ///     Returns list of registered users
    /// </returns>
    [HttpGet]
    [Route("list_all_users")]
    public async Task<IActionResult> Users()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    /// <summary>
    ///     Login/gets authorization token
    /// </summary>
    /// <param name="loginVM"></param>
    /// <returns>
    ///     returns authorization token
    /// </returns>
    [HttpPost]
    [Route("get_authorization_token")]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        var responseMsg = new ReturnString();
        if (!string.IsNullOrEmpty(loginVM.EmailAddress) &&
            !string.IsNullOrEmpty(loginVM.Password))
        {
            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        var loggedInUser = _userManager.FindByEmailAsync(loginVM.EmailAddress);

                        var fullNames = loggedInUser.Result.FullName.Split(' ');
                        var firstName = fullNames[0];
                        var surname = fullNames[1];

                        var userId = _userManager.FindByEmailAsync(ClaimTypes.NameIdentifier);

                        var claims = new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, userId.Id.ToString()),
                            new Claim(ClaimTypes.Email, loggedInUser.Result.Email),
                            new Claim(ClaimTypes.GivenName, firstName),
                            new Claim(ClaimTypes.Surname, surname),
                            new Claim(ClaimTypes.Role, loggedInUser.Result.NormalizedUserName)
                        };

                        var token = new JwtSecurityToken
                        (
                            _config["Jwt:Issuer"],
                            _config["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(30),
                            notBefore: DateTime.UtcNow,
                            signingCredentials: new SigningCredentials(
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                                SecurityAlgorithms.HmacSha256)
                        );

                        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                        responseMsg.ReturnMessage = tokenString;

                        return Ok(responseMsg);
                    }

                    return NotFound();
                }

                return NotFound();
            }

            return NotFound();
        }

        return BadRequest("Invalid user credentials");
    }

    /// <summary>
    ///     Registers user to db
    /// </summary>
    /// <param name="registerVM"></param>
    /// <returns>
    ///     returns success if registered successfully
    /// </returns>
    [HttpPost]
    [Route("register_user")]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        var returnMsg = new ReturnString();
        var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
        if (user != null)
        {
            returnMsg.ReturnMessage = "This email address is already in use";
            return Ok(returnMsg);
        }

        var newUser = new ApplicationUser
        {
            FullName = registerVM.FullName,
            Email = registerVM.EmailAddress,
            UserName = registerVM.EmailAddress
        };
        var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

        if (newUserResponse.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            returnMsg.ReturnMessage = "RegisterCompleted";
            return Ok(returnMsg);
        }

        var errorMsg = string.Empty;
        foreach (var error in newUserResponse.Errors) errorMsg = error.Description;
        return BadRequest(errorMsg);
    }
}