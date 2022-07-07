using System;
using System.Linq;
using System.Threading.Tasks;
using eTournamentAPI.Data.Services;
using eTournamentAPI.Data.Static;
using eTournamentAPI.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eTournamentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class MatchesController : ControllerBase
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
            return Ok(allMatches);
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

                return Ok(filteredResultNew);
            }

            return Ok(allMatches);
        }

        //GET: Matches/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var matchDetail = await _service.GetMatchByIdAsync(id);
            return Ok(matchDetail);
        }

        //GET: Matches/Create
        public async Task<IActionResult> Create()
        {
            var matchDropdownsData = await _service.GetNewMatchDropdownsValues();

            return Ok(matchDropdownsData);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(NewMatchVM match)
        {
            if (!ModelState.IsValid)
            {
                var matchDropdownsData = await _service.GetNewMatchDropdownsValues();

                return Ok(match);
            }

            await _service.AddNewMatchAsync(match);
            return Ok(nameof(Index));
        }


        //GET: Matches/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var matchDetails = await _service.GetMatchByIdAsync(id);
            if (matchDetails == null) return BadRequest("NotFound");

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

            return Ok(response);
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(int id, NewMatchVM match)
        {
            if (id != match.Id) return BadRequest("NotFound");

            if (!ModelState.IsValid)
            {
                var matchDropdownsData = await _service.GetNewMatchDropdownsValues();

                return Ok(match);
            }

            await _service.UpdateMatchAsync(match);
            return Ok(nameof(Index));
        }
    }
}
