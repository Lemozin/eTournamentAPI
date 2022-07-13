using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using eTournament.Data.Enums;
using eTournament.Data.RequestReturnModels;
using eTournament.Data.ReturnModels;
using eTournament.Data.ViewModels;
using eTournament.Helpers;
using eTournament.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using X.PagedList;

namespace eTournament.Controllers
{
    public class MatchesController : Controller
    {
        private readonly TournamentAPI _api = new();
        private readonly Logic _logic = new();
        private HttpClient client = new();
        private StringContent data;
        private string json;
        private HttpResponseMessage responseMessage = new();

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            IEnumerable<Match> matches = new List<Match>();
            var userVM = new UserVM();

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");
            var toggleLogSession = HttpContext.Session.GetString("LoggedOut");
            TempData["LoggedOut"] = toggleLogSession;

            var token = HttpContext.Session.GetString("Token");
            
            if(token != null) TempData["Token"] = token;

            if (toggleLogSession != null)
            {
                if (!toggleLogSession.Equals("True"))
                {
                    TempData["Username"] = username;
                    TempData["Role"] = role;
                }
            }
            else
            {
                TempData["Username"] = username;
                TempData["Role"] = role;
                TempData["Token"] = token;
            }

            if (!string.IsNullOrEmpty(token))
                responseMessage = await _logic.GetPostHttpClientAsync(
                    RequestMethods.GET,
                    false,
                    true,
                    "api/Matches/get_all_matches",
                    null,
                    token);
            else
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
                    var userView = new UserVM();

                    if (token != null)
                    {
                        if (!string.IsNullOrEmpty(token))
                        {
                            var claims = _logic.ExtractClaims(token);

                            userVM = new UserVM
                            {
                                Username = claims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                                EmailAddress = claims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                                GivenName = claims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                                Surname = claims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                                Role = claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                            };

                            if (toggleLogSession != null)
                            {
                                if (!toggleLogSession.Equals("True"))
                                {
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

                                    if (userVM.EmailAddress != null)
                                        HttpContext.Session.SetString("Email", userVM.EmailAddress);

                                    HttpContext.Session.SetString("Token", token.ToString());
                                }
                            }
                            else
                            {
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

                                if (userVM.EmailAddress != null)
                                    HttpContext.Session.SetString("Email", userVM.EmailAddress);

                                HttpContext.Session.SetString("Token", token.ToString());
                            }

                            HttpContext.Session.SetString("LoggedOut", "");
                        }
                    }
                    else
                    {
                        HttpContext.Session.SetString("LoggedOut", "");
                        TempData["Username"] = null;
                        TempData["Role"] = null;
                        TempData["Token"] = null;
                    }
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(matches.ToPagedList(page, 6));
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
            //var matchDropdownsData = await _service.GetNewMatchDropdownsValues();
            //var username = HttpContext.Session.GetString("Username");
            //var role = HttpContext.Session.GetString("Role");

            //TempData["Username"] = username;
            //TempData["Role"] = role;

            //ViewBag.Teams = new SelectList(matchDropdownsData.Teams, "Id", "Name");
            //ViewBag.Coaches = new SelectList(matchDropdownsData.Coaches, "Id", "FullName");
            //ViewBag.Players = new SelectList(matchDropdownsData.Players, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewMatchVM match)
        {
            var newMatchDropdownsVM = new NewMatchDropdownsVM();
            var response = new ReturnString();
            if (!ModelState.IsValid)
                //var matchDropdownsData = await _service.GetNewMatchDropdownsValues();

                //ViewBag.Teams = new SelectList(matchDropdownsData.Teams, "Id", "Name");
                //ViewBag.Coaches = new SelectList(matchDropdownsData.Coaches, "Id", "FullName");
                //ViewBag.Players = new SelectList(matchDropdownsData.Players, "Id", "FullName");

                return View(match);

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
                token);
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
            //var matchDetails = await _service.GetMatchByIdAsync(id);
            //if (matchDetails == null) return View("NotFound");

            //var response = new NewMatchVM
            //{
            //    Id = matchDetails.Id,
            //    Name = matchDetails.Name,
            //    Description = matchDetails.Description,
            //    Price = matchDetails.Price,
            //    StartDate = matchDetails.StartDate,
            //    EndDate = matchDetails.EndDate,
            //    ImageURL = matchDetails.ImageURL,
            //    MatchCategory = matchDetails.MatchCategory,
            //    TeamId = matchDetails.TeamId,
            //    CoachId = matchDetails.CoachId,
            //    PlayerIds = matchDetails.Players_Matches.Select(n => n.PlayerId).ToList()
            //};

            //var matchDropdownsData = await _service.GetNewMatchDropdownsValues();
            //ViewBag.Teams = new SelectList(matchDropdownsData.Teams, "Id", "Name");
            //ViewBag.Coaches = new SelectList(matchDropdownsData.Coaches, "Id", "FullName");
            //ViewBag.Players = new SelectList(matchDropdownsData.Players, "Id", "FullName");

            return View();
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
                    token);

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