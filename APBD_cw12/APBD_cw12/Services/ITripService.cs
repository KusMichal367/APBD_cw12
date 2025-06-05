using APBD_cw12.Models;
namespace APBD_cw12.Services;

public interface ITripService
{
    Task<TripPageResponse> GetTripsAsync(int page, int pageSize);
    Task AddClientToTripAsync(int idTrip, InputClientDto inputClient);
}