using System;
using System.Runtime.Serialization;

namespace AutoReservation.Common.DataTransferObjects
{
    [DataContract]
    public class ReservationDto
    {
        [DataMember]
        public DateTime To { get; set; }
        [DataMember]
        public int ReservationsNr { get; set; }
        [DataMember]
        public DateTime From { get; set; }
        [DataMember]
        public AutoDto Car { get; set; }
        [DataMember]
        public KundeDto Customer { get; set; }

        // keep for comparing while doing concurrent updates/deletes on DB
        [DataMember]
        public byte[] RowVersion { get; set; }

        public ReservationDto(ReservationDto reservation)
        {
            To = reservation.To;
            ReservationsNr = reservation.ReservationsNr;
            From = reservation.From;
            Car = reservation.Car;
            Customer = reservation.Customer;
            RowVersion = reservation.RowVersion;
        }

        public override string ToString()
            => $"{ReservationsNr}; {From}; {To}; {Car}; {Customer}";
    }
}
