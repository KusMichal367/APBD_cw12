using APBD_cw12.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_cw12.Repositories;

public class TripRepository: ITripRepository
{
    private readonly TripContext _context;

    public TripRepository(TripContext context)
    {
        _context = context;
    }


    public async Task<int> CountTripsAsync()
    {
        return await _context.Trips.CountAsync();
    }

    public async Task<List<Trip>> GetPageTripAsync(int page, int pageSize)
    {
        if (page <= 0)
            page = 1;

        if (pageSize <= 0)
            pageSize = 10;

        return await _context.Trips
            .Include(t=> t.IdCountries)
            .Include(t => t.ClientTrips).ThenInclude(ct => ct.IdClientNavigation)
            .OrderByDescending(t=> t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Client?> GetClientByPeselAsync(string pesel)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => c.Pesel == pesel);
    }

    public async Task<Trip?> GetTripAsync(int idTrip)
    {
        return await _context.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .FirstOrDefaultAsync(t => t.IdTrip == idTrip);
    }

    public async Task AddClientTripAsync(ClientTrip clientTrip)
    {
        await _context.ClientTrips.AddAsync(clientTrip);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}