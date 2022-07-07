using System.Threading.Tasks;
using eTournamentAPI.Data;
using eTournamentAPI.Data.Static;
using eTournamentAPI.Data.ViewModels;
using eTournamentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTournamentAPI.Controllers
{
    [Route("api/v3/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }


        public IActionResult Login()
        {
            return Ok(new LoginVM());
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return BadRequest(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded) return Ok(result);
                }
                return BadRequest(loginVM);
            }

            return BadRequest(loginVM);
        }


        public IActionResult Register()
        {
            return Ok(new RegisterVM());
        }

        [HttpPost]
        [Route("register_user")]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return BadRequest(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                return BadRequest(registerVM);
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

            return Ok("RegisterCompleted");
        }

        [HttpPost]
        [Route("log_out")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("LogoutSuccess");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return Ok();
        }
    }
}
