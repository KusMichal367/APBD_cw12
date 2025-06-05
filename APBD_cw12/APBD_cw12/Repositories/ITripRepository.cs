using APBD_cw12.Models;

namespace APBD_cw12.Repositories;

public interface ITripRepository
{
    Task<int> CountTripsAsync();
    Task<List<Trip>> GetPageTripAsync(int page, int pageSize);

    Task<Client?> GetClientByPeselAsync(string pesel);
    Task<Trip?> GetTripAsync(int idTrip);
    Task AddClientTripAsync(ClientTrip clientTrip);
    Task SaveChangesAsync();


}