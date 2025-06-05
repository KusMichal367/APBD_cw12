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

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip([FromRoute] int idTrip, [FromBody] InputClientDto inputClient)
    {
        try
        {
            await _tripService.AddClientToTripAsync(idTrip, inputClient);
            return Ok(new
                { message = $"Client {inputClient.FirstName} {inputClient.LastName} was added to trip {idTrip}" });
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }

        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

}