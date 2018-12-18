using System.Configuration;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace AutoReservation.Dal
{
    public class AutoReservationContext
        : DbContext
    {
        public DbSet<Auto> Autos { get; set; }
        public DbSet<Kunde> Kunden { get; set; }
        public DbSet<Reservation> Reservationen { get; set; }


        public static readonly LoggerFactory LoggerFactory = new LoggerFactory(
            new[] { new ConsoleLoggerProvider((_, logLevel) => logLevel >= LogLevel.Information, true) }
        );

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .EnableSensitiveDataLogging()
                    .UseLoggerFactory(LoggerFactory) // Warning: Do not create a new ILoggerFactory instance each time
                    .UseSqlServer(ConfigurationManager.ConnectionStrings[nameof(AutoReservationContext)].ConnectionString);
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auto>()
                .HasDiscriminator<int>("AutoKlasse")
                .HasValue<StandardAuto>(2)
                .HasValue<MittelklasseAuto>(1)
                .HasValue<LuxusklasseAuto>(0);


            modelBuilder.HasSequence<int>("AutoIds", schema: "dbo")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.Entity<Auto>()
                .Property(a => a.Id)
                .HasDefaultValueSql("NEXT VALUE FOR dbo.AutoIds");


            modelBuilder.HasSequence<int>("KundeIds", schema: "dbo")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.Entity<Kunde>()
                .Property(k => k.Id)
                .HasDefaultValueSql("NEXT VALUE FOR dbo.KundeIds");


            modelBuilder.HasSequence<int>("Reservationsnummern", schema: "dbo")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.Entity<Reservation>()
                .Property(r => r.ReservationsNr)
                .HasDefaultValueSql("NEXT VALUE FOR dbo.Reservationsnummern");
        }
    }
}
