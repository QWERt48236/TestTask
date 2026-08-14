namespace ConferenceBooking.Application.Exceptions;

// The request is well formed but clashes with the current state of the data.
public class ConflictException : Exception
{
    public ConflictException(string message)
        : base(message)
    {
    }
}
