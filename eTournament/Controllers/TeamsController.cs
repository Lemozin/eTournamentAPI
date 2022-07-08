using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using eTournament.Data.RequestReturnModels;
using eTournament.Data.Services;
using eTournament.Data.Static;
using eTournament.Helpers;
using eTournament.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eTournament.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class TeamsController : Controller
    {
        private readonly ITeamService _service;
        private HttpResponseMessage responseMessage = new();
        private readonly Logic _logic = new();

        public TeamsController(ITeamService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Team> allTeamss = new List<Team>();

            responseMessage = await _logic.GetPostHttpClientAsync(
                false,
                true,
                "api/Teams/get_all_teams",
                null);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                allTeamss = JsonConvert.DeserializeObject<IEnumerable<Team>>(result);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

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

            var response = new ReturnString();
            responseMessage = await _logic.GetPostHttpClientAsync(
                true,
                false,
                "api/Teams/create_team",
                team);
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ReturnString>(result);
            }

            return RedirectToAction(nameof(Index));
        }

        //Get: Teams/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var teamDetails = new Team();
            var requestId = new RequestIdModel();
            requestId.RequestId = id;

            responseMessage = await _logic.GetPostHttpClientAsync(
                false,
                false,
                "api/Players/get_player_details",
                requestId);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                teamDetails = JsonConvert.DeserializeObject<Team>(result);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(teamDetails);
        }

        //Get: Teams/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var teamDetails = new Team();
            var requestId = new RequestIdModel();
            requestId.RequestId = id;

            responseMessage = await _logic.GetPostHttpClientAsync(
                false,
                false,
                "api/Players/get_player_details",
                requestId);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                teamDetails = JsonConvert.DeserializeObject<Team>(result);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(teamDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,Name,Description")] Team team)
        {
            if (!ModelState.IsValid) return View(team);

            var response = new ReturnString();
            responseMessage = await _logic.GetPostHttpClientAsync(
                true,
                false,
                "api/Team/edit_team",
                team);
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ReturnString>(result);
            }

            return RedirectToAction(nameof(Index));
        }

        //Get: Teams/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var teamDetails = new Team();
            var requestId = new RequestIdModel();
            requestId.RequestId = id;

            responseMessage = await _logic.GetPostHttpClientAsync(
                false,
                false,
                "api/Players/get_player_details",
                requestId);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                teamDetails = JsonConvert.DeserializeObject<Team>(result);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(teamDetails);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var requestId = new RequestIdModel();
            var response = new ReturnString();
            requestId.RequestId = id;
            responseMessage = await _logic.GetPostHttpClientAsync(
                false,
                false,
                "api/Teams/delete_team",
                requestId);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ReturnString>(result);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}