using APBD_cw12.Models;

namespace APBD_cw12.Repositories;

public interface ITripRepository
{
    Task<int> CountTripsAsync();
    Task<List<Trip>> GetPageTripAsync(int page, int pageSize);
}