using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using eTournament.Data.RequestReturnModels;
using eTournament.Data.Services;
using eTournament.Helpers;
using eTournament.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eTournament.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayersService _service;
        private HttpResponseMessage responseMessage = new();
        private readonly Logic _logic = new();
        public PlayersController(IPlayersService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Player> players = new List<Player>();

            responseMessage = await _logic.GetPostHttpClientAsync(
                false,
                true,
                "api/Players/get_all_players",
                null);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                players = JsonConvert.DeserializeObject<IEnumerable<Player>>(result);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
            return View(players);
        }

        //Get: Players/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName,ProfilePictureURL,Bio")] Player player)
        {
            if (!ModelState.IsValid) return View(player);

            var response = new ReturnString();
            responseMessage = await _logic.GetPostHttpClientAsync(
                false,
                false,
                "api/Players/create_player",
                player);
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ReturnString>(result);
            }

            return RedirectToAction(nameof(Index));
        }

        //Get: Players/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var playerDetails = new Player();
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
                playerDetails = JsonConvert.DeserializeObject<Player>(result);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(playerDetails);
        }

        //Get: Players/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var playerDetails = new Player();
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
                playerDetails = JsonConvert.DeserializeObject<Player>(result);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(playerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePictureURL,Bio")] Player player)
        {
            if (!ModelState.IsValid) return View(player);

            var response = new ReturnString();
            responseMessage = await _logic.GetPostHttpClientAsync(
                false,
                false,
                "api/Players/edit_player",
                player);
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ReturnString>(result);
            }

            return RedirectToAction(nameof(Index));
        }

        //Get: Players/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var playerDetails = new Player();
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
                playerDetails = JsonConvert.DeserializeObject<Player>(result);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(playerDetails);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requestId = new RequestIdModel(); 
            var response = new ReturnString();
            requestId.RequestId = id;
            responseMessage = await _logic.GetPostHttpClientAsync(
                false,
                false,
                "api/Players/delete_player",
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