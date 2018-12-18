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

        [InverseProperty("Kunde")]
        public ICollection<Reservation> Reservations { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }

        public override bool Equals(object obj)
        {
            var kunde = obj as Kunde;
            return kunde != null &&
                   Id == kunde.Id &&
                   Surname == kunde.Surname &&
                   FirstName == kunde.FirstName &&
                   Birthday == kunde.Birthday;
        }

        public override int GetHashCode()
        {
            var hashCode = -1684391524;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Surname);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
            hashCode = hashCode * -1521134295 + Birthday.GetHashCode();
            return hashCode;
        }
    }
}
