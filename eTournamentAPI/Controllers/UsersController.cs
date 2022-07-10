using eTournamentAPI.Data.RequestReturnModels;
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
    [HttpGet]
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
    public async Task<IActionResult> Create([Bind("ProfilePictureURL,FullName,Bio")] Coach coach)
    {
        if (!ModelState.IsValid) return Ok(coach);

        await _service.AddAsync(coach);
        return Ok("CoachCreateSuccess");
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
    public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePictureURL,FullName,Bio")] Coach coach)
    {
        if (!ModelState.IsValid) return Ok(coach);

        if (id == coach.Id)
        {
            await _service.UpdateAsync(id, coach);
            return Ok("CoachEditSuccess");
        }

        return Ok(coach);
    }

    /// <summary>
    ///     Removes a coach from the db by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    ///     Returns "CoachDeleteSuccess" if removed successfully
    /// </returns>
    [HttpDelete]
    [ActionName("Delete")]
    [Route("delete_coach")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var coachDetails = await _service.GetByIdAsync(id);
        if (coachDetails == null) return Ok("NotFound");

        await _service.DeleteAsync(id);
        return Ok("CoachDeleteSuccess");
    }
}