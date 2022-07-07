using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using eTournament.Data.RequestReturnModels;
using eTournament.Data.Services;
using eTournament.Data.Static;
using eTournament.Data.ViewModels;
using eTournament.Helpers;
using eTournament.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace eTournament.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class MatchesController : Controller
    {
        private readonly IMatchService _service;
        private TournamentAPI _api = new();
        private HttpClient client = new();
        private HttpResponseMessage responseMessage = new();

        public MatchesController(IMatchService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Match> matches = new List<Match>();
            client = _api.Initial();
            responseMessage = await client.GetAsync("api/Matches/get_all_matches");

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                matches = JsonConvert.DeserializeObject<IEnumerable<Match>>(result);
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
            var requestString = new RequestModel();
            requestString.RequestId = 0;
            requestString.RequestString = searchString;
            var json = JsonConvert.SerializeObject(requestString);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            responseMessage = await client.PostAsync("api/Matches/get_match_by_filter", data);

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
            Match matchDetails = new Match();
            var requestId = new RequestModel();
            requestId.RequestId = id;
            requestId.RequestString = "";
            client = _api.Initial();
            var json = JsonConvert.SerializeObject(requestId);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            responseMessage = await client.PostAsync("api/Matches/get_match_details_id", data);

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
            if (!ModelState.IsValid)
            {
                var matchDropdownsData = await _service.GetNewMatchDropdownsValues();

                ViewBag.Teams = new SelectList(matchDropdownsData.Teams, "Id", "Name");
                ViewBag.Coaches = new SelectList(matchDropdownsData.Coaches, "Id", "FullName");
                ViewBag.Players = new SelectList(matchDropdownsData.Players, "Id", "FullName");

                return View(match);
            }

            await _service.AddNewMatchAsync(match);
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
            if (id != match.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var matchDropdownsData = await _service.GetNewMatchDropdownsValues();

                ViewBag.Teams = new SelectList(matchDropdownsData.Teams, "Id", "Name");
                ViewBag.Coaches = new SelectList(matchDropdownsData.Coaches, "Id", "FullName");
                ViewBag.Players = new SelectList(matchDropdownsData.Players, "Id", "FullName");

                return View(match);
            }

            await _service.UpdateMatchAsync(match);
            return RedirectToAction(nameof(Index));
        }
    }
}