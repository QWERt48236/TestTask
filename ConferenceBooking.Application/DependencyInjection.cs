using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConferenceBooking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IHallService, HallService>();

        // Scoped, not singleton: it reads the bands through a scoped repository.
        services.AddScoped<IPricingService, PricingService>();

        return services;
    }
}
