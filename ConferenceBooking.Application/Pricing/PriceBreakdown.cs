namespace ConferenceBooking.Application.Pricing;

// Hours is fractional, so a 40-minute segment is charged 40 minutes.
public record PriceSegment(
    string BandName,
    DateTime Start,
    DateTime End,
    decimal Hours,
    decimal EffectiveHourlyRate,
    decimal Amount);

// Segments always sum exactly to BaseAmount.
public record PriceBreakdown(
    IReadOnlyList<PriceSegment> Segments,
    decimal BaseAmount,
    decimal AmenitiesAmount)
{
    public decimal TotalAmount => BaseAmount + AmenitiesAmount;
}
