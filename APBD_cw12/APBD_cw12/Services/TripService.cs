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

    public async Task AddClientToTripAsync(int idTrip, InputClientDto inputClient)
    {
        var checkPesel = await _tripRepository.GetClientByPeselAsync(inputClient.PESEL);

        if (checkPesel == null)
        {
            throw new InvalidOperationException($"Client {inputClient.PESEL} does not exist");
        }


        var trip = await _tripRepository.GetTripAsync(idTrip);
        if (trip == null)
        {
            throw new InvalidOperationException($"Trip {idTrip} does not exist");
        }

        if (trip.DateFrom <= DateTime.UtcNow)
        {
            throw new InvalidOperationException("You cannot register to trip that's in progress or ended");
        }

        bool alreadRegistered = trip.ClientTrips.Any(ct => ct.IdClient == checkPesel.IdClient);
        if (alreadRegistered)
        {
            throw new InvalidOperationException($"Client {inputClient.PESEL} already registered to trip {trip.IdTrip}");
        }

        var clientTrip = new ClientTrip
        {
            IdClient = checkPesel.IdClient,
            IdTrip = trip.IdTrip,
            RegisteredAt = DateTime.UtcNow,
            PaymentDate = inputClient.PaymentDate,
        };

        await _tripRepository.AddClientTripAsync(clientTrip);
        await _tripRepository.SaveChangesAsync();
    }
}