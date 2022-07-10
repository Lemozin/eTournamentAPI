﻿using System.Collections.Generic;
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
    public class UsersController : Controller
    {
        private readonly Logic _logic = new();
        private readonly IUserService _service;
        private HttpResponseMessage responseMessage = new();

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Coach> allCoaches = new List<Coach>();

            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            TempData["Username"] = username;
            TempData["Role"] = role;

            var token = HttpContext.Session.GetString("Token");

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

            return View(allCoaches);
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProfilePictureURL,FullName,Bio")] Coach coach)
        {
            if (!ModelState.IsValid) return View(coach);

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
                coach);
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

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                false,
                false,
                "api/Users/get_coach_details",
                requestId);

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

            var response = new ReturnString();
            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                false,
                false,
                "api/Users/edit_coach",
                coach);
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ReturnString>(result);
            }

            return View(coach);
        }

        //GET: Coaches/delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var coachDetails = new Coach();
            var requestId = new RequestIdModel();
            requestId.RequestId = id;

            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                false,
                false,
                "api/Users/get_coach_details",
                requestId);

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
            requestId.RequestId = id;
            responseMessage = await _logic.GetPostHttpClientAsync(
                RequestMethods.POST,
                false,
                false,
                "api/Users/delete_coach",
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