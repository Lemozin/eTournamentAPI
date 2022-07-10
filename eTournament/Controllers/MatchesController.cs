using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using eTournament.Data.Enums;
using eTournament.Data.RequestReturnModels;
using eTournament.Data.Services;
using eTournament.Data.Static;
using eTournament.Data.ViewModels;
using eTournament.Helpers;
using eTournament.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace eTournament.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class MatchesController : Controller
    {
        private readonly TournamentAPI _api = new();
        private readonly Logic _logic = new();
        private readonly IMatchService _service;
        private HttpClient client = new();
        private StringContent data;
        private string json;
        private HttpResponseMessage responseMessage = new();

        public MatchesController(IMatchService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Match> matches = new List<Match>();
            var userVM = new UserVM();

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.GET,
                false,
                true,
                "api/Matches/get_all_matches");

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                matches = JsonConvert.DeserializeObject<IEnumerable<Match>>(result);

                if (matches != null)
                {
                    var token = TempData["Token"];
                    var userView = new UserVM();

                    if (token != null)
                        if (!string.IsNullOrEmpty(token.ToString()))
                        {
                            var claims = _logic.ExtractClaims(token.ToString());

                            userVM = new UserVM
                            {
                                Username = claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                                EmailAddress = claims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                                GivenName = claims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                                Surname = claims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                                Role = claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                            };

                            if (userVM.Username != null)
                            {
                                TempData["Username"] = userVM.GivenName;
                                HttpContext.Session.SetString("Username", userVM.GivenName);
                            }

                            if (userVM.Role != null)
                            {
                                TempData["Role"] = userVM.Role;
                                HttpContext.Session.SetString("Role", userVM.Role);
                            }
                        }
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(matches);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            IEnumerable<Match> matches = new List<Match>();
            client = _api.Initial();
            var requestString = new RequestStringModel();
            requestString.RequestString = searchString;

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                false,
                false,
                "api/Matches/get_match_by_filter",
                matches);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                matches = JsonConvert.DeserializeObject<IEnumerable<Match>>(result);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Index", matches);
        }

        //GET: Matches/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var matchDetails = new Match();
            var requestId = new RequestIdModel();
            requestId.RequestId = id;

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                false,
                false,
                "api/Matches/get_match_details_id",
                requestId);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                matchDetails = JsonConvert.DeserializeObject<Match>(result);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(matchDetails);
        }

        //GET: Matches/Create
        public async Task<IActionResult> Create()
        {
            var matchDropdownsData = await _service.GetNewMatchDropdownsValues();

            ViewBag.Teams = new SelectList(matchDropdownsData.Teams, "Id", "Name");
            ViewBag.Coaches = new SelectList(matchDropdownsData.Coaches, "Id", "FullName");
            ViewBag.Players = new SelectList(matchDropdownsData.Players, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewMatchVM match)
        {
            var newMatchDropdownsVM = new NewMatchDropdownsVM();
            var response = new ReturnString();
            if (!ModelState.IsValid)
            {
                var matchDropdownsData = await _service.GetNewMatchDropdownsValues();

                ViewBag.Teams = new SelectList(matchDropdownsData.Teams, "Id", "Name");
                ViewBag.Coaches = new SelectList(matchDropdownsData.Coaches, "Id", "FullName");
                ViewBag.Players = new SelectList(matchDropdownsData.Players, "Id", "FullName");

                return View(match);
            }

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                true,
                false,
                "api/Matches/create_match",
                match,
                token.ToString());
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ReturnString>(result);
            }

            return RedirectToAction(nameof(Index));
        }

        //GET: Matches/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var matchDetails = await _service.GetMatchByIdAsync(id);
            if (matchDetails == null) return View("NotFound");

            var response = new NewMatchVM
            {
                Id = matchDetails.Id,
                Name = matchDetails.Name,
                Description = matchDetails.Description,
                Price = matchDetails.Price,
                StartDate = matchDetails.StartDate,
                EndDate = matchDetails.EndDate,
                ImageURL = matchDetails.ImageURL,
                MatchCategory = matchDetails.MatchCategory,
                TeamId = matchDetails.TeamId,
                CoachId = matchDetails.CoachId,
                PlayerIds = matchDetails.Players_Matches.Select(n => n.PlayerId).ToList()
            };

            var matchDropdownsData = await _service.GetNewMatchDropdownsValues();
            ViewBag.Teams = new SelectList(matchDropdownsData.Teams, "Id", "Name");
            ViewBag.Coaches = new SelectList(matchDropdownsData.Coaches, "Id", "FullName");
            ViewBag.Players = new SelectList(matchDropdownsData.Players, "Id", "FullName");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewMatchVM match)
        {
            var newMatchDropdownsVM = new NewMatchDropdownsVM();
            var response = new ReturnString();

            if (id != match.Id) return View("NotFound");

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            if (!ModelState.IsValid)
            {
                responseMessage = await _logic.GetPostHttpClientAsync(
                    RequestMethods.POST,
                    false,
                    true,
                    "api/Matches/get_match_dropdown_values",
                    null,
                    token.ToString());

                if (responseMessage.IsSuccessStatusCode)
                {
                    var result = responseMessage.Content.ReadAsStringAsync().Result;
                    newMatchDropdownsVM = JsonConvert.DeserializeObject<NewMatchDropdownsVM>(result);
                }

                ViewBag.Teams = new SelectList(newMatchDropdownsVM.Teams, "Id", "Name");
                ViewBag.Coaches = new SelectList(newMatchDropdownsVM.Coaches, "Id", "FullName");
                ViewBag.Players = new SelectList(newMatchDropdownsVM.Players, "Id", "FullName");

                return View(match);
            }

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                true,
                false,
                "api/Matches/edit_match",
                match);
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ReturnString>(result);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}