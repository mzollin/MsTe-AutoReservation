using System;

namespace AutoReservation.BusinessLayer.Exceptions
{
    public class AutoUnavailableException : Exception
    {
        public AutoUnavailableException(DateTime unavailableFrom, DateTime earliestFrom)
            : base($"Auto is not available from: {unavailableFrom}, earliest availability is {earliestFrom}") { }
    }
}