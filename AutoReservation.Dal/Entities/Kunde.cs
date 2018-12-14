using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
    [Table("Kunde", Schema = "dbo")]
    public class Kunde
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [Column("Nachname")]
        [MaxLength(20)]
        public string Surname { get; set; }

        [Required]
        [Column("Vorname")]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [Column("Geburtsdatum", TypeName = "DateTime2")]
        [MaxLength(7)]
        public DateTime Birthday { get; set; }

        [InverseProperty("KundeId")]
        public ICollection<Reservation> Reservations { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
