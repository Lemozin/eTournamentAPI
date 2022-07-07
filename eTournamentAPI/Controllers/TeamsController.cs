using System.Threading.Tasks;
using eTournamentAPI.Data.Services;
using eTournamentAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTournamentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _service;

        public TeamsController(ITeamService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("get_all_teams")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var allTeamss = await _service.GetAllAsync();
            return Ok(allTeamss);
        }

        [HttpPost]
        [Route("create_team")]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Logo,Name,Description")] Team team)
        {
            if (!ModelState.IsValid) return Ok(team);
            await _service.AddAsync(team);
            return Ok("TeamCreateSuccess");
        }

        //Get: Teams/Details/1
        [AllowAnonymous]
        [HttpGet]
        [Route("get_team_details")]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var teamDetails = await _service.GetByIdAsync(id);
            if (teamDetails == null) return Ok("NotFound");
            return Ok(teamDetails);
        }

        [HttpPost]
        [Route("edit_team")]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,Name,Description")] Team team)
        {
            if (!ModelState.IsValid) return Ok(team);
            await _service.UpdateAsync(id, team);
            return Ok("TeamEditSuccess");
        }

        [HttpDelete]
        [ActionName("Delete")]
        [Route("delete_team")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var teamDetails = await _service.GetByIdAsync(id);
            if (teamDetails == null) return Ok("NotFound");

            await _service.DeleteAsync(id);
            return Ok("TeamDeleteSuccess");
        }
    }
}
