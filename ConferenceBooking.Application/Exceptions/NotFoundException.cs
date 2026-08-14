namespace ConferenceBooking.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }

    public static NotFoundException For(string entity, Guid id) =>
        new($"{entity} with id {id} was not found.");
}
