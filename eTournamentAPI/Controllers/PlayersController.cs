﻿using eTournamentAPI.Data.RequestReturnModels;
using eTournamentAPI.Data.RequestReturnModels;
using eTournamentAPI.Data.ReturnModels;
using eTournamentAPI.Data.Services;
using eTournamentAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTournamentAPI.Controllers;

/// <summary>
///     Player controller which is responsible for creating/updating/removing a player
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PlayersController : ControllerBase
{
    private readonly IPlayersService _service;

    public PlayersController(IPlayersService service)
    {
        _service = service;
    }

    /// <summary>
    ///     Gets list of registered players
    /// </summary>
    /// <returns>
    ///     Returns list of players
    /// </returns>
    [HttpGet]
    [Route("get_all_players")]
    public async Task<IActionResult> Index()
    {
        var data = await _service.GetAllAsync();
        return Ok(data);
    }

    /// <summary>
    ///     Creates a player by supplying a FullName,ProfilePictureURL(image url),Bio
    /// </summary>
    /// <param name="player"></param>
    /// <returns>
    ///     Returns "PlayerCreateSuccess" if created successfully
    /// </returns>
    [HttpPost]
    [Route("create_player")]
    public async Task<IActionResult> Create(PlayersTeamsCoachesReqRes playersTeamsCoaches)
    {
        var player = new Player();
        var response = new ReturnString();
        player.ProfilePictureURL = playersTeamsCoaches.ProfilePictureURL;
        player.FullName = playersTeamsCoaches.ProfilePictureName;
        player.Bio = playersTeamsCoaches.ProfilePictureBio;

        if (!ModelState.IsValid) return Ok(player);

        await _service.AddAsync(player);
        response.ReturnMessage = "PlayerCreateSuccess";
        return Ok(response);
    }

    /// <summary>
    ///     Gets details of a player by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    ///     Returns player details
    /// </returns>
    //Get: Players/Details/1
    [HttpPost]
    [Route("get_player_details")]
    public async Task<IActionResult> Details(RequestIdModel request)
    {
        var playerDetails = await _service.GetByIdAsync(request.RequestId);

        if (playerDetails == null) return Ok("NotFound");
        return Ok(playerDetails);
    }

    /// <summary>
    ///     Updates/edits a player by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="player"></param>
    /// <returns>
    ///     Returns "PlayerEditSuccess" if updated successfully
    /// </returns>
    [HttpPut]
    [Route("edit_player")]
    public async Task<IActionResult> Edit(PlayersTeamsCoachesReqResId playersTeamsCoachesReq)
    {
        var player = new Player();
        var response = new ReturnString();
        player.Id = playersTeamsCoachesReq.id;
        player.ProfilePictureURL = playersTeamsCoachesReq.PlayersTeamsCoachesReqRes.ProfilePictureURL;
        player.FullName = playersTeamsCoachesReq.PlayersTeamsCoachesReqRes.ProfilePictureName;
        player.Bio = playersTeamsCoachesReq.PlayersTeamsCoachesReqRes.ProfilePictureBio;

        if (!ModelState.IsValid) return Ok(player);
        await _service.UpdateAsync(playersTeamsCoachesReq.id, player);

        response.ReturnMessage = "PlayerEditSuccess";

        return Ok(response);
    }

    /// <summary>
    ///     Delete a player by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    ///     Returns "PlayerDeleteSuccess" if deleted successfully
    /// </returns>
    [HttpPost]
    [ActionName("Delete")]
    [Route("delete_player")]
    public async Task<IActionResult> DeleteConfirmed(RequestIdModel requestId)
    {
        var response = new ReturnString();
        var playerDetails = await _service.GetByIdAsync(requestId.RequestId);

        response.ReturnMessage = "NotFound";

        if (playerDetails == null) return Ok(response);

        await _service.DeleteAsync(requestId.RequestId);
        response.ReturnMessage = "PlayerDeleteSuccess";

        return Ok(response);
    }
}