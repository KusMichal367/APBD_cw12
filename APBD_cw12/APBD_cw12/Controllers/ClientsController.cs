using APBD_cw12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD_cw12.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ClientsController: ControllerBase
{
    private readonly TripContext _context;

    public ClientsController(TripContext context)
    {
        _context = context;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c=> c.IdClient == idClient);

        if (client == null)
        {
            return NotFound( new { message = $"Client {idClient} not found" });
        }

        if (client.ClientTrips.Any())
        {
            return BadRequest(new { message = $"Client {idClient} is registered on trips" });
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Client {idClient} deleted" });
    }
}