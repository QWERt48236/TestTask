namespace ConferenceBooking.Application.Exceptions;

public class OutsideBusinessHoursException : Exception
{
    public OutsideBusinessHoursException(string message)
        : base(message)
    {
    }
}
