using System.Threading.Tasks;
using eTournamentAPI.Data.Services;
using eTournamentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTournamentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayersService _service;

        public PlayersController(IPlayersService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get_all_players")]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        [HttpPost]
        [Route("create_player")]
        public async Task<IActionResult> Create([Bind("FullName,ProfilePictureURL,Bio")] Player player)
        {
            if (!ModelState.IsValid) return Ok(player);
            await _service.AddAsync(player);
            return Ok("PlayerCreateSuccess");
        }

        //Get: Players/Details/1
        [HttpGet]
        [Route("get_player_details")]
        public async Task<IActionResult> Details(int id)
        {
            var playerDetails = await _service.GetByIdAsync(id);

            if (playerDetails == null) return Ok("NotFound");
            return Ok(playerDetails);
        }

        [HttpPost]
        [Route("edit_player")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePictureURL,Bio")] Player player)
        {
            if (!ModelState.IsValid) return Ok(player);
            await _service.UpdateAsync(id, player);
            return Ok("PlayerEditSuccess");
        }

        [HttpDelete]
        [ActionName("Delete")]
        [Route("delete_player")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playerDetails = await _service.GetByIdAsync(id);

            if (playerDetails == null) return Ok("NotFound");

            await _service.DeleteAsync(id);

            return Ok("PlayerDeleteSuccess");
        }
    }
}
