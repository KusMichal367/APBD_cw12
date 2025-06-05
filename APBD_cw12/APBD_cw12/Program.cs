using Microsoft.EntityFrameworkCore;
using APBD_cw12.Models;
using APBD_cw12.Repositories;
using APBD_cw12.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<ITripService, TripService>();

builder.Services.AddDbContext<TripContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers(); 

app.Run();