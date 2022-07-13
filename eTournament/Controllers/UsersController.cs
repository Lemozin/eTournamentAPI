using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using eTournament.Data.Enums;
using eTournament.Data.RequestReturnModels;
using eTournament.Data.ReturnModels;
using eTournament.Helpers;
using eTournament.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace eTournament.Controllers
{
    public class UsersController : Controller
    {
        private readonly Logic _logic = new();
        private HttpResponseMessage responseMessage = new();

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            IEnumerable<Coach> allCoaches = new List<Coach>();

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            if (token != null) TempData["Token"] = token;

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.GET,
                true,
                true,
                "api/Users/get_all_coaches",
                null,
                token);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                allCoaches = JsonConvert.DeserializeObject<IEnumerable<Coach>>(result);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(allCoaches.ToPagedList(page, 4));
        }

        //GET: Coaches/details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var coachDetails = new Coach();
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
                "api/Users/get_coach_details",
                requestId,
                token);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                coachDetails = JsonConvert.DeserializeObject<Coach>(result);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(coachDetails);
        }

        //GET: Coaches/create
        public IActionResult Create()
        {
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProfilePictureURL,FullName,Bio")] Coach coach)
        {
            if (!ModelState.IsValid) return View(coach);

            var playersTeamsCoachesReqRes = new PlayersTeamsCoachesReqRes();

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            playersTeamsCoachesReqRes.ProfilePictureURL = coach.ProfilePictureURL;
            playersTeamsCoachesReqRes.ProfilePictureName = coach.FullName;
            playersTeamsCoachesReqRes.ProfilePictureBio = coach.Bio;

            var response = new ReturnString();
            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                true,
                false,
                "api/Users/create_coach",
                playersTeamsCoachesReqRes,
                token);
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ReturnString>(result);
            }

            return RedirectToAction(nameof(Index));
        }

        //GET: Coaches/edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var coachDetails = new Coach();
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
                "api/Users/get_coach_details",
                requestId,
                token);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                coachDetails = JsonConvert.DeserializeObject<Coach>(result);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(coachDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePictureURL,FullName,Bio")] Coach coach)
        {
            if (!ModelState.IsValid) return View(coach);

            var playersTeamsCoachesReqResId = new PlayersTeamsCoachesReqResId();
            var playersTeamsCoachesReqRes = new PlayersTeamsCoachesReqRes();

            var response = new ReturnString();

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

            playersTeamsCoachesReqResId.id = id;
            playersTeamsCoachesReqRes.ProfilePictureURL = coach.ProfilePictureURL;
            playersTeamsCoachesReqRes.ProfilePictureName = coach.FullName;
            playersTeamsCoachesReqRes.ProfilePictureBio = coach.Bio;
            playersTeamsCoachesReqResId.PlayersTeamsCoachesReqRes = playersTeamsCoachesReqRes;

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.PUT,
                true,
                false,
                "api/Users/edit_coach",
                playersTeamsCoachesReqResId,
                token);
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ReturnString>(result);
            }

            return RedirectToAction(nameof(Index));
        }

        //GET: Coaches/delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var coachDetails = new Coach();
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
                "api/Users/get_coach_details",
                requestId,
                token);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                coachDetails = JsonConvert.DeserializeObject<Coach>(result);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(coachDetails);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requestId = new RequestIdModel();
            var response = new ReturnString();
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;
            requestId.RequestId = id;

            var token = HttpContext.Session.GetString("Token");

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                true,
                false,
                "api/Users/delete_coach",
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