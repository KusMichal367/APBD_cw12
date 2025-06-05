namespace APBD_cw12.Models;

public class CountryDto
{
    public String Name { get; set; }
}

public class ClientDto
{
    public String FirstName { get; set; }
    public String LastName { get; set; }
}

public class TripDto
{
    public String Name { get; set; }
    public String Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }

    public List<CountryDto> Countries { get; set; }
    public List<ClientDto> Clients { get; set; }
}

public class TripPageResponse
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }

    public List<TripDto> Trips { get; set; }
}