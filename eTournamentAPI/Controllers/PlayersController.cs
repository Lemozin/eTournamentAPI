﻿using System.Threading.Tasks;
using eTournamentAPI.Data.Services;
using eTournamentAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTournamentAPI.Controllers
{
    /// <summary>
    /// Player controller which is responsible for creating/updating/removing a player
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayersService _service;

        public PlayersController(IPlayersService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets list of registered players
        /// </summary>
        /// <returns>
        /// Returns list of players
        /// </returns>
        [HttpGet]
        [Route("get_all_players")]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        /// <summary>
        /// Creates a player by supplying a FullName,ProfilePictureURL(image url),Bio
        /// </summary>
        /// <param name="player"></param>
        /// <returns>
        /// Returns "PlayerCreateSuccess" if created successfully
        /// </returns>
        [HttpPost]
        [Route("create_player")]
        public async Task<IActionResult> Create([Bind("FullName,ProfilePictureURL,Bio")] Player player)
        {
            if (!ModelState.IsValid) return Ok(player);
            await _service.AddAsync(player);
            return Ok("PlayerCreateSuccess");
        }

        /// <summary>
        /// Gets details of a player by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Returns player details
        /// </returns>
        //Get: Players/Details/1
        [HttpGet]
        [Route("get_player_details")]
        public async Task<IActionResult> Details(int id)
        {
            var playerDetails = await _service.GetByIdAsync(id);

            if (playerDetails == null) return Ok("NotFound");
            return Ok(playerDetails);
        }

        /// <summary>
        /// Updates/edits a player by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="player"></param>
        /// <returns>
        /// Returns "PlayerEditSuccess" if updated successfully
        /// </returns>
        [HttpPost]
        [Route("edit_player")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePictureURL,Bio")] Player player)
        {
            if (!ModelState.IsValid) return Ok(player);
            await _service.UpdateAsync(id, player);
            return Ok("PlayerEditSuccess");
        }

        /// <summary>
        /// Delete a player by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Returns "PlayerDeleteSuccess" if deleted successfully
        /// </returns>
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
