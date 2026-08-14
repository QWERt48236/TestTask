namespace ConferenceBooking.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();

        // Both are needed for GlobalExceptionHandler: AddProblemDetails registers the
        // IProblemDetailsService it writes through.
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}
