using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
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


        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
