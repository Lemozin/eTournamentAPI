using System.Threading.Tasks;
using eTournament.Data.Services;
using eTournament.Data.Static;
using eTournament.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTournament.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class TeamsController : Controller
    {
        private readonly ITeamService _service;

        public TeamsController(ITeamService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allTeamss = await _service.GetAllAsync();
            return View(allTeamss);
        }


        //Get: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Logo,Name,Description")] Team team)
        {
            if (!ModelState.IsValid) return View(team);
            await _service.AddAsync(team);
            return RedirectToAction(nameof(Index));
        }

        //Get: Teams/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var teamDetails = await _service.GetByIdAsync(id);
            if (teamDetails == null) return View("NotFound");
            return View(teamDetails);
        }

        //Get: Teams/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var teamDetails = await _service.GetByIdAsync(id);
            if (teamDetails == null) return View("NotFound");
            return View(teamDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,Name,Description")] Team team)
        {
            if (!ModelState.IsValid) return View(team);
            await _service.UpdateAsync(id, team);
            return RedirectToAction(nameof(Index));
        }

        //Get: Teams/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var teamDetails = await _service.GetByIdAsync(id);
            if (teamDetails == null) return View("NotFound");
            return View(teamDetails);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var teamDetails = await _service.GetByIdAsync(id);
            if (teamDetails == null) return View("NotFound");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}