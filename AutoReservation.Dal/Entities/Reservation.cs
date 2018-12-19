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

        private Auto _auto;
        [ForeignKey("AutoId")]
        public virtual Auto Auto {
            get { return this._auto; }
            set
            {
                this._auto = value;
                this.AutoId = value.Id;
            }
        }

        [Required]
        public int KundeId { get; set; }

        private Kunde _kunde;
        [ForeignKey("KundeId")]
        public virtual Kunde Kunde
        {
            get { return this._kunde; }
            set
            {
                this._kunde = value;
                this.KundeId = value.Id;
            }
        }

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

        public override bool Equals(object obj)
        {
            var reservation = obj as Reservation;
            return reservation != null &&
                   ReservationsNr == reservation.ReservationsNr &&
                   AutoId == reservation.AutoId &&
                   KundeId == reservation.KundeId &&
                   From == reservation.From &&
                   To == reservation.To;
        }

        public override int GetHashCode()
        {
            var hashCode = 21937253;
            hashCode = hashCode * -1521134295 + ReservationsNr.GetHashCode();
            hashCode = hashCode * -1521134295 + AutoId.GetHashCode();
            hashCode = hashCode * -1521134295 + KundeId.GetHashCode();
            hashCode = hashCode * -1521134295 + From.GetHashCode();
            hashCode = hashCode * -1521134295 + To.GetHashCode();
            return hashCode;
        }
    }
}
