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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("get_all_coaches")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var allCoaches = await _service.GetAllAsync();
            return Ok(allCoaches);
        }

        //GET: Coaches/details/1
        [AllowAnonymous]
        [HttpGet]
        [Route("get_coach_details")]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var coachDetails = await _service.GetByIdAsync(id);
            if (coachDetails == null) return Ok("NotFound");
            return Ok(coachDetails);
        }

        [HttpPost]
        [Route("create_coach")]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ProfilePictureURL,FullName,Bio")] Coach coach)
        {
            if (!ModelState.IsValid) return Ok(coach);

            await _service.AddAsync(coach);
            return Ok("CoachCreateSuccess");
        }

        [HttpPost]
        [Route("edit_coach")]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePictureURL,FullName,Bio")] Coach coach)
        {
            if (!ModelState.IsValid) return Ok(coach);

            if (id == coach.Id)
            {
                await _service.UpdateAsync(id, coach);
                return Ok("CoachEditSuccess");
            }

            return Ok(coach);
        }

        [HttpDelete]
        [ActionName("Delete")]
        [Route("delete_coach")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coachDetails = await _service.GetByIdAsync(id);
            if (coachDetails == null) return Ok("NotFound");

            await _service.DeleteAsync(id);
            return Ok("CoachDeleteSuccess");
        }
    }
}
