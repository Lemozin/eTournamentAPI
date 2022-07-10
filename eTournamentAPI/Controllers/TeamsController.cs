using eTournamentAPI.Data.RequestReturnModels;
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
    public async Task<IActionResult> Create([Bind("Logo,Name,Description")] Team team)
    {
        if (!ModelState.IsValid) return Ok(team);
        await _service.AddAsync(team);
        return Ok("TeamCreateSuccess");
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
    public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,Name,Description")] Team team)
    {
        if (!ModelState.IsValid) return Ok(team);
        await _service.UpdateAsync(id, team);
        return Ok("TeamEditSuccess");
    }

    /// <summary>
    ///     Deletes a team by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    ///     Returns "TeamDeleteSuccess" if deleted successfully
    /// </returns>
    [HttpDelete]
    [ActionName("Delete")]
    [Route("delete_team")]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var teamDetails = await _service.GetByIdAsync(id);
        if (teamDetails == null) return Ok("NotFound");

        await _service.DeleteAsync(id);
        return Ok("TeamDeleteSuccess");
    }
}