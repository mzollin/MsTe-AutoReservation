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

        [InverseProperty("AutoId")]
        public ICollection<Reservation> Reservations { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }
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
    }
}
