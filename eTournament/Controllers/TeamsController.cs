using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using eTournament.Data.Enums;
using eTournament.Data.RequestReturnModels;
using eTournament.Data.Services;
using eTournament.Helpers;
using eTournament.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eTournament.Controllers
{
    public class TeamsController : Controller
    {
        private readonly Logic _logic = new();
        private readonly ITeamService _service;
        private HttpResponseMessage responseMessage = new();

        public TeamsController(ITeamService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Team> allTeamss = new List<Team>();

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.GET,
                true,
                true,
                "api/Teams/get_all_teams",
                null,
                token);

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
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Logo,Name,Description")] Team team)
        {
            if (!ModelState.IsValid) return View(team);

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            var response = new ReturnString();
            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                false,
                false,
                "api/Teams/create_team",
                team,
                token);
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

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                true,
                false,
                "api/Teams/get_team_details",
                requestId,
                token);

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

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                true,
                false,
                "api/Teams/get_team_details",
                requestId,
                token);

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

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            var response = new ReturnString();
            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                true,
                false,
                "api/Team/edit_team",
                team,
                token);
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

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                true,
                false,
                "api/Teams/get_team_details",
                requestId,
                token);

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

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                true,
                false,
                "api/Teams/delete_team",
                requestId,
                token);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ReturnString>(result);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}