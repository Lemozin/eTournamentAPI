using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eTournamentAPI.Data;
using eTournamentAPI.Data.RequestReturnModels;
using eTournamentAPI.Data.ReturnModels;
using eTournamentAPI.Data.Static;
using eTournamentAPI.Data.ViewModels;
using eTournamentAPI.Helpers;
using eTournamentAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace eTournamentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public AccountController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            AppDbContext context,
            IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpGet]
        [Route("list_login_users")]
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("get_authorization_token")]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var responseString = new ReturnString();
            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        var token = _jwtAuthenticationManager.Authenticate(loginVM.EmailAddress);
                        responseString.ReturnMessage = token;
                        return Ok(responseString);
                    }

                    return Unauthorized();
                }

                return Ok(loginVM);
            }

            return NotFound();
        }

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
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            returnMsg.ReturnMessage = "RegisterCompleted";
            return Ok(returnMsg);
        }
    }
}