using System;

namespace AutoReservation.BusinessLayer.Exceptions
{
    public class CustomerNotOfAgeException : Exception
    {
        public CustomerNotOfAgeException(DateTime birth)
            : base($"Customer must be at least 18 years old, but is only {DateTime.Now.Year - birth.Year} years old (born {birth.ToShortDateString()})") { }
    }
}