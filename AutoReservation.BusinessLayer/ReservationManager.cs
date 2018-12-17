using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoReservation.BusinessLayer
{
    public class ReservationManager : ManagerBase
    {
        public IEnumerable<Reservation> ReadAll()
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return context.Reservationen.ToList();
            }
        }

        public IEnumerable<Reservation> ReadAllForGivenAuto(int id)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return context.Reservationen.Where(r => r.AutoId == id).ToList();
            }
        } 

        public Reservation Read(int nr)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return context.Reservationen.FirstOrDefault(a => a.ReservationsNr == nr);
            }
        }

        public void Create(Reservation reservation)
        {
            CheckDateRange(reservation);
            CheckCarAvailability(reservation);

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
            CheckCarAvailability(reservation);

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

        private void CheckDateRange(Reservation reservation)
        {
            if (reservation.From > reservation.To)
            {
                throw new InvalidDateRangeException($"From-date: {reservation.From} must happen before to-date: {reservation.To}");
            }

            var duration = reservation.To.Subtract(reservation.From).Hours;
            if (duration < 24)
            {
                throw new InvalidDateRangeException($"Range must be at least 24 hours, but is only {duration} hours.");
            }
        }

        private void CheckCarAvailability(Reservation reservation)
        {
            var car = reservation.Auto;
            var relatedReservations = ReadAllForGivenAuto(car.Id).ToList();

            if (!relatedReservations.Any())
            {
                Console.WriteLine($"No reservations for car {car.Id} found, don't need to check further.");
                return;
            }

            var latestReservation = relatedReservations.OrderBy(r => r.To).Last();
            if (latestReservation.To > reservation.From)
            {
                throw new AutoUnavailableException(reservation.From, latestReservation.To);
            }
        }
    }
}