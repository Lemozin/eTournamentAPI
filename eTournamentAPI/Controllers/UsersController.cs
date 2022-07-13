using eTournamentAPI.Data.RequestReturnModels;
using eTournamentAPI.Data.ReturnModels;
using eTournamentAPI.Data.Services;
using eTournamentAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTournamentAPI.Controllers;

/// <summary>
///     Users controller responsible for adding order to cart, removing order from cart and listing coaches
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    /// <summary>
    ///     Gets list of coaches
    /// </summary>
    /// <returns>
    ///     Returns list of coaches
    /// </returns>
    [HttpGet]
    [Route("get_all_coaches")]
    public async Task<IActionResult> Index()
    {
        var allCoaches = await _service.GetAllAsync();
        return Ok(allCoaches);
    }

    /// <summary>
    ///     Gets details of a coach by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    ///     Returns coach details if success and not found if not success
    /// </returns>
    //GET: Coaches/details/1
    [HttpPost]
    [Route("get_coach_details")]
    public async Task<IActionResult> Details(RequestIdModel request)
    {
        var coachDetails = await _service.GetByIdAsync(request.RequestId);
        if (coachDetails == null) return Ok("NotFound");
        return Ok(coachDetails);
    }

    /// <summary>
    ///     Creates a coach
    /// </summary>
    /// <param name="coach"></param>
    /// <returns>
    ///     Returns "CoachCreateSuccess" if created successfully
    /// </returns>
    [HttpPost]
    [Route("create_coach")]
    public async Task<IActionResult> Create(PlayersTeamsCoachesReqRes playersTeamsCoaches)
    {
        var coach = new Coach();
        var response = new ReturnString();
        coach.ProfilePictureURL = playersTeamsCoaches.ProfilePictureURL;
        coach.FullName = playersTeamsCoaches.ProfilePictureName;
        coach.Bio = playersTeamsCoaches.ProfilePictureBio;

        if (!ModelState.IsValid) return Ok(coach);

        await _service.AddAsync(coach);
        response.ReturnMessage = "CoachCreateSuccess";
        return Ok(response);
    }

    /// <summary>
    ///     Edits a coach by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="coach"></param>
    /// <returns>
    ///     Returns "CoachEditSuccess" if success
    /// </returns>
    [HttpPut]
    [Route("edit_coach")]
    public async Task<IActionResult> Edit(PlayersTeamsCoachesReqResId playersTeamsCoachesReq)
    {
        var coach = new Coach();
        var response = new ReturnString();
        coach.Id = playersTeamsCoachesReq.id;
        coach.ProfilePictureURL = playersTeamsCoachesReq.PlayersTeamsCoachesReqRes.ProfilePictureURL;
        coach.FullName = playersTeamsCoachesReq.PlayersTeamsCoachesReqRes.ProfilePictureName;
        coach.Bio = playersTeamsCoachesReq.PlayersTeamsCoachesReqRes.ProfilePictureBio;

        if (!ModelState.IsValid) return Ok(coach);

        await _service.UpdateAsync(playersTeamsCoachesReq.id, coach);

        response.ReturnMessage = "CoachEditSuccess";

        return Ok(response);
    }

    /// <summary>
    ///     Removes a coach from the db by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    ///     Returns "CoachDeleteSuccess" if removed successfully
    /// </returns>
    [HttpPost]
    [ActionName("Delete")]
    [Route("delete_coach")]
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