using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AutoReservation.BusinessLayer
{
    public class ReservationManager : ManagerBase
    {
        public IEnumerable<Reservation> GetAll()
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return context.Reservationen.Include(r => r.Kunde)
                                            .Include(r => r.Auto)
                                            .ToList();
            }
        }

        public IEnumerable<Reservation> GetAllForGivenAuto(int id)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return context.Reservationen.Where(r => r.Auto.Id == id)
                                            .Include(r => r.Kunde)
                                            .Include(r => r.Auto)
                                            .ToList();
            }
        } 

        public Reservation GetById(int nr)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return context.Reservationen.Include(r => r.Kunde)
                                            .Include(r => r.Auto)
                                            .FirstOrDefault(a => a.ReservationsNr == nr);
            }
        }

        public void Create(Reservation reservation)
        {
            CheckDateRange(reservation);
            CheckCarAvailability(reservation, this);

            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Attach(reservation);
                context.Entry(reservation).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Update(Reservation reservation)
        {
            CheckDateRange(reservation);
            CheckCarAvailability(reservation, this);

            using (AutoReservationContext context = new AutoReservationContext())
            {
                try
                {
                    context.Attach(reservation);
                    context.Entry(reservation).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw CreateOptimisticConcurrencyException(context, reservation);
                }
            }
        }

        public void Delete(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                try
                {
                    context.Attach(reservation);
                    context.Entry(reservation).State = EntityState.Deleted;
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw CreateOptimisticConcurrencyException(context, reservation);
                }
            }
        }
        
        //public for testing
        public static void CheckDateRange(Reservation reservation)
        {
            if (reservation.From > reservation.To)
            {
                throw new InvalidDateRangeException($"From-date: {reservation.From} must happen before to-date: {reservation.To}");
            }

            var duration = reservation.To.Subtract(reservation.From).TotalHours;
            if (duration < 24)
            {
                throw new InvalidDateRangeException($"Range must be at least 24 hours, but is only {duration} hours.");
            }
        }

        //public for testing
        public static void CheckCarAvailability(Reservation reservation, ReservationManager manager)
        {
            var car = reservation.Auto;
            var relatedReservations = manager.GetAllForGivenAuto(car.Id).ToList();

            if (!relatedReservations.Any())
            {
                Debug.WriteLine($"No reservations for car {car.Id} found, no need to check further.");
                return;
            }

            if (relatedReservations.Count == 1
             && relatedReservations.First().ReservationsNr == reservation.ReservationsNr)
            {
                Debug.WriteLine("There's only one reservation and we modify it, no need to check further.");
                return;
            }

            //remove object we are modifying from our collection
            var resInList = relatedReservations.FirstOrDefault(r => r.ReservationsNr == reservation.ReservationsNr);
            if (resInList != null)
            {
                relatedReservations.Remove(resInList);
            }

            var latestReservation = relatedReservations.OrderBy(r => r.To).Last();
            var firstReservation = relatedReservations.OrderBy(r => r.From).First();
            if (latestReservation.To < reservation.From
             || firstReservation.From > reservation.To)
            {
                Debug.WriteLine("Reserving outside the range of other reservations, no need to check for collisions.");
                return;
            }

            if (relatedReservations.Any(r => r.From <= reservation.From && r.To > reservation.From)
             || relatedReservations.Any(r => r.From >= reservation.From && r.To <= reservation.To)
             || relatedReservations.Any(r => r.From < reservation.To && r.To >= reservation.To))
            {
                throw new AutoUnavailableException(reservation.From, reservation.To);
            }
        }
    }
}