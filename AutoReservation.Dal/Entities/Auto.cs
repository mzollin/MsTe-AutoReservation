using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
    [Table("Auto", Schema = "dbo")]
    public abstract class Auto
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [Column("Marke")]
        [MaxLength(20)]
        public string Brand { get; set; }

        [Required]
        [Column("Tagestarif")]
        public int DailyRate { get; set; }

        [InverseProperty("Auto")]
        public ICollection<Reservation> Reservations { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }

        public override bool Equals(object obj)
        {
            var auto = obj as Auto;
            return auto != null &&
                   Id == auto.Id &&
                   Brand == auto.Brand &&
                   DailyRate == auto.DailyRate;
        }

        public override int GetHashCode()
        {
            var hashCode = -385653790;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Brand);
            hashCode = hashCode * -1521134295 + DailyRate.GetHashCode();
            return hashCode;
        }
    }

    public class StandardAuto : Auto
    {

    }

    public class MittelklasseAuto : Auto
    {

    }

    public class LuxusklasseAuto : Auto
    {
        [Required]
        [Column("Basistarif")]
        public int BaseRate { get; set; }

        public override bool Equals(object obj)
        {
            var auto = obj as LuxusklasseAuto;
            return auto != null &&
                   base.Equals(obj) &&
                   BaseRate == auto.BaseRate;
        }

        public override int GetHashCode()
        {
            var hashCode = 1161897520;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + BaseRate.GetHashCode();
            return hashCode;
        }
    }
}
