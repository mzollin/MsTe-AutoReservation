using System;

namespace AutoReservation.BusinessLayer.Exceptions
{
    public class AutoUnavailableException : Exception
    {
        public AutoUnavailableException(DateTime from, DateTime to)
            : base($"Range from {from} to {to} overlaps at least one other reservation.") { }
    }
}