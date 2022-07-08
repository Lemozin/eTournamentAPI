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
    /// <summary>
    /// Match controller which is responsible for creating/editing/deleting matches
    /// </summary>
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

        /// <summary>
        /// Get list of all matches
        /// </summary>
        /// <returns>
        /// Returns list of all matches
        /// </returns>
        [HttpGet]
        [Route("get_all_matches")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allMatches = await _service.GetAllAsync(n => n.Team);
            return Ok(allMatches);
        }

        /// <summary>
        /// Filter each match by match name
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>
        /// Returns single filtered match
        /// </returns>
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

        /// <summary>
        /// Get details of a match
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Returns details of a match
        /// </returns>
        [HttpPost]
        [Route("get_match_details_id")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(RequestIdModel id)
        {
            var matchDetail = await _service.GetMatchByIdAsync(id.RequestId);
            return Ok(matchDetail);
        }

        /// <summary>
        /// Get dropdown values (players/coaches/teams) to prepare for create function
        /// </summary>
        /// <returns>
        /// Returns an object of dropdown values to prepare for create
        /// </returns>
        [HttpGet]
        [Route("get_match_dropdown_values")]
        public async Task<IActionResult> GetNewMatchDropdownsValues()
        {
            var matchDropdownsData = await _service.GetNewMatchDropdownsValues();

            return Ok(matchDropdownsData);
        }

        /// <summary>
        /// Creates a match
        /// </summary>
        /// <param name="match"></param>
        /// <returns>
        /// Returns "MatchCreateSuccess" if success
        /// </returns>
        [HttpPost]
        [Route("create_match")]
        public async Task<IActionResult> Create(NewMatchVM match)
        {
            var response = new ReturnString();
            await _service.AddNewMatchAsync(match);
            response.ReturnMessage = "MatchCreateSuccess";
            return Ok(response);
        }

        /// <summary>
        /// Edits a match by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="match"></param>
        /// <returns>
        /// Returns "MatchUpdateSuccess" is updated successfully
        /// </returns>
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
