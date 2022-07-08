using System;
using System.Linq;
using System.Threading.Tasks;
using eTournamentAPI.Data.RequestReturnModels;
using eTournamentAPI.Data.ReturnModels;
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

        [HttpPost]
        [Route("get_match_by_filter")]
        [AllowAnonymous]
        public async Task<IActionResult> Filter(RequestStringModel searchString)
        {
            var allMatches = await _service.GetAllAsync(n => n.Team);

            if (!string.IsNullOrEmpty(searchString.RequestString))
            {
                var filteredResultNew = allMatches.Where(n =>
                    string.Equals(n.Name, searchString.RequestString, StringComparison.CurrentCultureIgnoreCase) ||
                    string.Equals(n.Description, searchString.RequestString, StringComparison.CurrentCultureIgnoreCase)).ToList();

                return Ok(filteredResultNew);
            }

            return Ok(allMatches);
        }

        [HttpPost]
        [Route("get_match_details_id")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(RequestIdModel id)
        {
            var matchDetail = await _service.GetMatchByIdAsync(id.RequestId);
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
            var response = new ReturnString();
            await _service.AddNewMatchAsync(match);
            response.ReturnMessage = "MatchCreateSuccess";
            return Ok(response);
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
