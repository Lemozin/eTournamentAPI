using System;
using System.Linq;
using System.Threading.Tasks;
using eTournament.Data.Services;
using eTournament.Data.Static;
using eTournament.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eTournament.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class MatchesController : Controller
    {
        private readonly IMatchService _service;

        public MatchesController(IMatchService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allMatches = await _service.GetAllAsync(n => n.Team);
            return View(allMatches);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            var allMatches = await _service.GetAllAsync(n => n.Team);

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResultNew = allMatches.Where(n =>
                    string.Equals(n.Name, searchString, StringComparison.CurrentCultureIgnoreCase) ||
                    string.Equals(n.Description, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

                return View("Index", filteredResultNew);
            }

            return View("Index", allMatches);
        }

        //GET: Matches/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var matchDetail = await _service.GetMatchByIdAsync(id);
            return View(matchDetail);
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