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
}