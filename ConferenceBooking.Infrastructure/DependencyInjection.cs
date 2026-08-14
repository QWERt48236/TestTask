using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Infrastructure.Persistence;
using ConferenceBooking.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConferenceBooking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("A database connection string is required.", nameof(connectionString));
        }

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IHallRepository, HallRepository>();
        services.AddScoped<IAmenityRepository, AmenityRepository>();
        services.AddScoped<ITimeBandRepository, TimeBandRepository>();

        return services;
    }
}
