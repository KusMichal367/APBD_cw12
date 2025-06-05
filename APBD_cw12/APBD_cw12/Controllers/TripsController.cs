using APBD_cw12.Models;
using APBD_cw12.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_cw12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _tripService.GetTripsAsync(page, pageSize);
        return Ok(result);
    }

}