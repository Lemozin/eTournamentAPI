using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using eTournament.Data;
using eTournament.Data.Enums;
using eTournament.Data.RequestReturnModels;
using eTournament.Data.ViewModels;
using eTournament.Helpers;
using eTournament.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eTournament.Controllers
{
    public class AccountController : Controller
    {
        private readonly TournamentAPI _api = new();
        private readonly AppDbContext _context;
        private readonly Logic _logic = new();
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private HttpClient client = new();
        private StringContent data;
        private string json;
        private HttpResponseMessage responseMessage = new();

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        public async Task<IActionResult> Users()
        {
            IEnumerable<ApplicationUser> users = new List<ApplicationUser>();

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                true,
                true,
                "api/Account/list_all_users",
                null,
                token);

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
            var token = new ReturnString();
            if (!ModelState.IsValid) return View(loginVM);

            client = _api.Initial();
            json = JsonConvert.SerializeObject(loginVM);
            data = new StringContent(json, Encoding.UTF8, "application/json");

            responseMessage = await client.PostAsync("api/Account/get_authorization_token", data);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                token = JsonConvert.DeserializeObject<ReturnString>(result);
                TempData["Token"] = token.ReturnMessage;

                HttpContext.Session.SetString("Token", token.ReturnMessage);

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
            json = JsonConvert.SerializeObject(registerVM);
            data = new StringContent(json, Encoding.UTF8, "application/json");
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
            HttpContext.Session.SetString("Token", "");

            HttpContext.Session.SetString("LoggedOut", "True");

            TempData["Username"] = null;
            TempData["Role"] = null;

            return RedirectToAction("Index", "Matches");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }
    }
}