using System.Runtime.Serialization;

namespace AutoReservation.Common.DataTransferObjects
{
    [DataContract]
    public class AutoDto
    {
        [DataMember]
        public int BaseRate { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Brand { get; set; }
        [DataMember]
        public int DailyRate { get; set; }
        [DataMember]
        public AutoKlasse AutoClass { get; set; }

        // keep for comparing while doing concurrent updates/deletes on DB
        [DataMember]
        public byte[] RowVersion { get; set; }

        public AutoDto() { } //default ctor for property injection
        public AutoDto(AutoDto car)
        {
            BaseRate = car.BaseRate;
            Id = car.Id;
            Brand = car.Brand;
            DailyRate = car.DailyRate;
            AutoClass = car.AutoClass;
            RowVersion = car.RowVersion;
        }
        public override string ToString()
            => $"{Id}; {Brand}; {DailyRate}; {BaseRate}; {AutoClass}; {RowVersion}";
    }
}
