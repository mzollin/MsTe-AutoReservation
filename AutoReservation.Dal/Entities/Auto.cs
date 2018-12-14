using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
    public class Auto
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

        [Required]
        [Column("AutoKlasse")]
        public int CarClass { get; set; }

        // FIXME: use subclasses because that's why
        public int BaseRate { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
