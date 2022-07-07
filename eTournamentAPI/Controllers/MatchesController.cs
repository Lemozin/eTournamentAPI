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
