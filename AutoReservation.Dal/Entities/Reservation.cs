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
        public int AutoId { get; set; }
        [ForeignKey("AutoId")]
        public virtual Auto Auto { get; set; }

        [Required]
        public int KundeId { get; set; }
        [ForeignKey("KundeId")]
        public virtual Kunde Kunde { get; set; }

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
