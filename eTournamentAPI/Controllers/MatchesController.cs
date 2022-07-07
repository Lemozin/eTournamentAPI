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
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoles.Admin)]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _service;

        public MatchesController(IMatchService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get_all_matches")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allMatches = await _service.GetAllAsync(n => n.Team);
            return Ok(allMatches);
        }

        [HttpGet]
        [Route("get_match_by_filter")]
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

        [HttpGet]
        [Route("get_match_details_id")]
        public async Task<IActionResult> Details(int id)
        {
            var matchDetail = await _service.GetMatchByIdAsync(id);
            return Ok(matchDetail);
        }

        [HttpGet]
        [Route("get_match_dropdown_values")]
        public async Task<IActionResult> GetNewMatchDropdownsValues()
        {
            var matchDropdownsData = await _service.GetNewMatchDropdownsValues();

            return Ok(matchDropdownsData);
        }

        [HttpPost]
        [Route("create_match")]
        public async Task<IActionResult> Create(NewMatchVM match)
        {
            await _service.AddNewMatchAsync(match);
            return Ok("MatchCreateSuccess");
        }

        [HttpPost]
        [Route("edit_match")]
        public async Task<IActionResult> Edit(int id, NewMatchVM match)
        {
            if (id != match.Id) return BadRequest("NotFound");

            await _service.UpdateMatchAsync(match);
            return Ok("MatchUpdateSuccess");
        }
    }
}
