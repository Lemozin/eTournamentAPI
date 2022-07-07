using System.Threading.Tasks;
using eTournament.Data.Services;
using eTournament.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTournament.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allCoaches = await _service.GetAllAsync();
            return View(allCoaches);
        }

        //GET: Coaches/details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var coachDetails = await _service.GetByIdAsync(id);
            if (coachDetails == null) return View("NotFound");
            return View(coachDetails);
        }

        //GET: Coaches/create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProfilePictureURL,FullName,Bio")] Coach coach)
        {
            if (!ModelState.IsValid) return View(coach);

            await _service.AddAsync(coach);
            return RedirectToAction(nameof(Index));
        }

        //GET: Coaches/edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var coachDetails = await _service.GetByIdAsync(id);
            if (coachDetails == null) return View("NotFound");
            return View(coachDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePictureURL,FullName,Bio")] Coach coach)
        {
            if (!ModelState.IsValid) return View(coach);

            if (id == coach.Id)
            {
                await _service.UpdateAsync(id, coach);
                return RedirectToAction(nameof(Index));
            }

            return View(coach);
        }

        //GET: Coaches/delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var coachDetails = await _service.GetByIdAsync(id);
            if (coachDetails == null) return View("NotFound");
            return View(coachDetails);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coachDetails = await _service.GetByIdAsync(id);
            if (coachDetails == null) return View("NotFound");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}