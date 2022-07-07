using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using eTournament.Data;
using eTournament.Data.RequestReturnModels;
using eTournament.Data.Static;
using eTournament.Data.ViewModels;
using eTournament.Helpers;
using eTournament.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace eTournament.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private TournamentAPI _api = new();
        private HttpClient client = new();
        private HttpResponseMessage responseMessage = new();

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        public async Task<IActionResult> Users()
        {
            IEnumerable<ApplicationUser> users = new List<ApplicationUser>();
            client = _api.Initial();
            responseMessage = await client.GetAsync("api/Account/list_login_users");

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<IEnumerable<ApplicationUser>>(result);
            }

            return View(users);
        }


        public IActionResult Login()
        {
            return View(new LoginVM());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var signInResult = SignInResult.Failed;
            if (!ModelState.IsValid) return View(loginVM);

            client = _api.Initial();
            var json = JsonConvert.SerializeObject(loginVM);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            responseMessage = await client.PostAsync("api/Account/login", data);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                signInResult = JsonConvert.DeserializeObject<SignInResult>(result);

                return RedirectToAction("Index", "Matches");
            }

            TempData["Error"] = "Wrong credentials. Please, try again!";
            return View(loginVM);
        }


        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            var response = new ReturnString();

            if (!ModelState.IsValid) return View(registerVM);

            client = _api.Initial();
            var json = JsonConvert.SerializeObject(registerVM);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            responseMessage = await client.PostAsync("api/Account/register_user", data);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ReturnString>(result);

                if (!response.ReturnMessage.Equals("RegisterCompleted"))
                {
                    TempData["Error"] = response.ReturnMessage;

                    return View(registerVM);
                }
            }

            return View("RegisterCompleted");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Matches");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }
    }
}