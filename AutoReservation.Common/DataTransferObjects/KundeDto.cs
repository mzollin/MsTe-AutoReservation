using System;
using System.Runtime.Serialization;

namespace AutoReservation.Common.DataTransferObjects
{
    [DataContract]
    public class KundeDto
    {
        [DataMember]
        public DateTime Birthday { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string FirstName { get; set; }

        // keep for comparing while doing concurrent updates/deletes on DB
        [DataMember]
        public byte[] RowVersion { get; set; }

        public KundeDto() { } //default ctor for property injection
        public KundeDto(KundeDto customer)
        {
            Birthday = customer.Birthday;
            Id = customer.Id;
            Surname = customer.Surname;
            FirstName = customer.FirstName;
            RowVersion = customer.RowVersion;
        }

        public override string ToString()
            => $"{Id}; {Surname}; {FirstName}; {Birthday}; {RowVersion}";
    }
}
