using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AutoReservation.Dal.Entities
{
    [Table("Reservation", Schema = "dbo")]
    public class Reservation
    {
        [Key]
        public int ReservationsNr { get; set; }


        [Required]
        [ForeignKey(nameof(Auto))]
        public int AutoId { get; set; }

        [Required]
        [ForeignKey(nameof(Kunde))]
        public int KundeId { get; set; }

        [Required]
        [Column("Von", TypeName = "DateTime2")]
        [MaxLength(7)]
        public DateTime From { get; set; }

        [Required]
        [Column("Bis", TypeName = "DateTime2")]
        [MaxLength(7)]
        public DateTime To { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
