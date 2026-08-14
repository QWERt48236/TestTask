using ConferenceBooking.Application.Exceptions;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Application.Pricing;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Services;

public class PricingService : IPricingService
{
    private const int MoneyDecimals = 2;

    private readonly ITimeBandRepository _bands;

    public PricingService(ITimeBandRepository bands)
    {
        _bands = bands;
    }

    public async Task<PriceBreakdown> CalculateAsync(
        Hall hall,
        DateTime start,
        TimeSpan duration,
        IReadOnlyCollection<Amenity> amenities,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(hall);
        ArgumentNullException.ThrowIfNull(amenities);

        if (duration <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(duration), duration, "Duration must be greater than zero.");
        }

        var end = start + duration;

        // The walk works in one day's clock times, so midnight has to be ruled out here.
        if (end.Date != start.Date)
        {
            throw new OutsideBusinessHoursException(
                $"A booking must start and finish on the same day. " +
                $"Requested {start:yyyy-MM-dd HH\\:mm} to {end:yyyy-MM-dd HH\\:mm}.");
        }

        // Not cached, so an edited band takes effect immediately.
        var bands = await _bands.GetAllAsync(cancellationToken);

        var segments = new List<PriceSegment>();
        var cursor = start;

        while (cursor < end)
        {
            var band = BandAt(bands, cursor)
                ?? throw new OutsideBusinessHoursException(
                    $"No pricing band covers {cursor:HH\\:mm}, so the hall cannot be booked then.");

            var next = NextBoundaryAfter(bands, cursor, end);
            var hours = (decimal)(next - cursor).TotalHours;
            var rate = Round(band.RateFor(hall.BaseHourlyRate));

            segments.Add(new PriceSegment(band.Name, cursor, next, hours, rate, Round(rate * hours)));
            cursor = next;
        }

        // Sum the rounded segments so the breakdown reconciles with the total.
        return new PriceBreakdown(
            segments,
            segments.Sum(s => s.Amount),
            Round(amenities.Sum(a => a.Price)));
    }

    // Null means the instant is outside business hours.
    private static TimeBand? BandAt(IReadOnlyList<TimeBand> bands, DateTime moment)
    {
        var time = TimeOnly.FromDateTime(moment);

        return bands.Where(b => b.Covers(time))
            .OrderBy(b => b.Priority)
            .FirstOrDefault();
    }

    // Every band's start and end, not just the active one's: Peak opens inside Standard.
    private static DateTime NextBoundaryAfter(IReadOnlyList<TimeBand> bands, DateTime moment, DateTime limit)
    {
        var date = moment.Date;
        var next = limit;

        foreach (var band in bands)
        {
            foreach (var boundary in (ReadOnlySpan<TimeOnly>)[band.Start, band.End])
            {
                var candidate = date + boundary.ToTimeSpan();

                if (candidate > moment && candidate < next)
                {
                    next = candidate;
                }
            }
        }

        return next;
    }

    private static decimal Round(decimal amount) =>
        Math.Round(amount, MoneyDecimals, MidpointRounding.AwayFromZero);
}
