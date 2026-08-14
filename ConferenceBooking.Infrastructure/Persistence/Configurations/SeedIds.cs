namespace ConferenceBooking.Infrastructure.Persistence.Configurations;

// Fixed ids for the seeded rows. HasData needs values that never change between
// migrations, so these cannot be generated. Deliberately readable so they are easy
// to paste into Swagger while trying the API out.
internal static class SeedIds
{
    public static readonly Guid HallA = new("11111111-1111-1111-1111-111111111111");
    public static readonly Guid HallB = new("22222222-2222-2222-2222-222222222222");
    public static readonly Guid HallC = new("33333333-3333-3333-3333-333333333333");

    public static readonly Guid Projector = new("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    public static readonly Guid WiFi = new("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    public static readonly Guid Sound = new("cccccccc-cccc-cccc-cccc-cccccccccccc");

    public static readonly Guid[] AllHalls = [HallA, HallB, HallC];
    public static readonly Guid[] AllAmenities = [Projector, WiFi, Sound];
}
