using eTournamentAPI.Data.RequestReturnModels;
using eTournamentAPI.Data.ReturnModels;
using eTournamentAPI.Data.Services;
using eTournamentAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTournamentAPI.Controllers;

/// <summary>
///     Player controller which is responsible for creating/updating/removing a team
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _service;

    public TeamsController(ITeamService service)
    {
        _service = service;
    }

    /// <summary>
    ///     Gets list of registered teams
    /// </summary>
    /// <returns>
    ///     Returns list of teams
    /// </returns>
    [HttpGet]
    [Route("get_all_teams")]
    public async Task<IActionResult> Index()
    {
        var allTeamss = await _service.GetAllAsync();
        return Ok(allTeamss);
    }

    /// <summary>
    ///     Creates a team by supplying a Logo(image url),Name,Description
    /// </summary>
    /// <param name="team"></param>
    /// <returns>
    ///     Returns "TeamCreateSuccess" if created successfully
    /// </returns>
    [HttpPost]
    [Route("create_team")]
    public async Task<IActionResult> Create(PlayersTeamsCoachesReqRes playersTeamsCoaches)
    {
        var team = new Team();
        var response = new ReturnString();
        team.Logo = playersTeamsCoaches.ProfilePictureURL;
        team.Name = playersTeamsCoaches.ProfilePictureName;
        team.Description = playersTeamsCoaches.ProfilePictureBio;

        if (!ModelState.IsValid) return Ok(team);

        await _service.AddAsync(team);
        response.ReturnMessage = "TeamCreateSuccess";
        return Ok(response);
    }

    /// <summary>
    ///     Gets details of a team by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    ///     Returns filtered team details
    /// </returns>
    //Get: Teams/Details/1
    [HttpPost]
    [Route("get_team_details")]
    public async Task<IActionResult> Details(RequestIdModel request)
    {
        var teamDetails = await _service.GetByIdAsync(request.RequestId);
        if (teamDetails == null) return Ok("NotFound");
        return Ok(teamDetails);
    }

    /// <summary>
    ///     Edits a team by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="team"></param>
    /// <returns>
    ///     Returns "TeamEditSuccess" if updated successfully
    /// </returns>
    [HttpPut]
    [Route("edit_team")]
    public async Task<IActionResult> Edit(PlayersTeamsCoachesReqResId playersTeamsCoachesReq)
    {
        var team = new Team();
        var response = new ReturnString();
        team.Id = playersTeamsCoachesReq.id;
        team.Logo = playersTeamsCoachesReq.PlayersTeamsCoachesReqRes.ProfilePictureURL;
        team.Name = playersTeamsCoachesReq.PlayersTeamsCoachesReqRes.ProfilePictureName;
        team.Description = playersTeamsCoachesReq.PlayersTeamsCoachesReqRes.ProfilePictureBio;

        if (!ModelState.IsValid) return Ok(team);
        await _service.UpdateAsync(playersTeamsCoachesReq.id, team);

        response.ReturnMessage = "TeamEditSuccess";

        return Ok(response);
    }

    /// <summary>
    ///     Deletes a team by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    ///     Returns "TeamDeleteSuccess" if deleted successfully
    /// </returns>
    [HttpPost]
    [ActionName("Delete")]
    [Route("delete_team")]
    public async Task<IActionResult> DeleteConfirm(RequestIdModel requestId)
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