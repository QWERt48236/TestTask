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

    /// <summary>
    /// Translates constraint violations into <see cref="ConflictException"/>.
    /// Services check for duplicates and dependants before writing, but those checks are
    /// check-then-act: between the check and the commit, a concurrent request can take the
    /// name or add a booking. The database constraint is what actually prevents the bad
    /// write, and without this the caller would see a bare 500 instead of the 409 the
    /// service intended.
    /// </summary>
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

    // An exception filter, so anything unrecognised propagates with its stack intact.
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
