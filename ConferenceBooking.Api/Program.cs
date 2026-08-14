using ConferenceBooking.Api;
using ConferenceBooking.Application;
using ConferenceBooking.Infrastructure;
using ConferenceBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException(
        "Connection string 'Default' is missing. See the README for how to create appsettings.Development.json.");

builder.Services.AddApiServices();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString);

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // UI only: the document itself comes from AddOpenApi/MapOpenApi above.
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "Conference Booking API v1"));

    // Convenience only: other environments apply migrations explicitly.
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
