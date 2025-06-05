using APBD_cw12.Models;
using APBD_cw12.Repositories;

namespace APBD_cw12.Services;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;

    public TripService(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<TripPageResponse> GetTripsAsync(int page, int pageSize)
    {
        if (page <=0)
            page = 1;
        if(pageSize <= 0)
            pageSize = 10;

        var TripCount = await _tripRepository.CountTripsAsync();
        var allPages = (int)Math.Ceiling(TripCount / (double) pageSize);

        if(allPages == 0)
            allPages = 1;

        var trips = await _tripRepository.GetPageTripAsync(page, pageSize);

        var tripDtos = trips.Select(t => new TripDto
        {
            Name = t.Name,
            Description = t.Description,
            DateFrom = t.DateFrom,
            DateTo = t.DateTo,
            MaxPeople = t.MaxPeople,

            Countries = t.IdCountries.Select(c=>new CountryDto
            {
                Name = c.Name,
            }).ToList(),

            Clients = t.ClientTrips.Select(ct => new ClientDto
            {
                FirstName = ct.IdClientNavigation.FirstName,
                LastName = ct.IdClientNavigation.LastName,
            }).ToList(),
        }).ToList();

        return new TripPageResponse
        {
            PageNumber = page,
            PageSize = pageSize,
            AllPages = allPages,
            Trips = tripDtos
        };
    }
}