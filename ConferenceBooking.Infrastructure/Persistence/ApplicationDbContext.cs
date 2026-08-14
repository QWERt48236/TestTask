using ConferenceBooking.Application.Exceptions;
using ConferenceBooking.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    // SQL Server error numbers.
    private const int DuplicateKey = 2601;
    private const int UniqueConstraint = 2627;
    private const int ForeignKeyConflict = 547;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Hall> Halls => Set<Hall>();

    public DbSet<Amenity> Amenities => Set<Amenity>();

    public DbSet<Booking> Bookings => Set<Booking>();

    public DbSet<TimeBand> TimeBands => Set<TimeBand>();

    // Service pre-checks race; the constraint is the real guard, and untranslated it is a 500.
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (TryTranslate(ex, out var conflict))
        {
            throw conflict;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    // Used as an exception filter, so anything unrecognised keeps its original stack.
    private static bool TryTranslate(DbUpdateException exception, out ConflictException conflict)
    {
        conflict = exception.InnerException is SqlException sql
            ? sql.Number switch
            {
                DuplicateKey or UniqueConstraint =>
                    new ConflictException("That change conflicts with an existing record."),
                ForeignKeyConflict =>
                    new ConflictException("That record is referenced by other data and cannot be changed or deleted."),
                _ => null!,
            }
            : null!;

        return conflict is not null;
    }
}
