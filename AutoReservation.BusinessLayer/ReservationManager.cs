using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
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

        private static void CheckDateRange(Reservation reservation)
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

        private void CheckCarAvailability(Reservation reservation)
        {
            var car = reservation.Auto;
            var relatedReservations = GetAllForGivenAuto(car.Id).ToList();

            if (!relatedReservations.Any())
            {
                Console.WriteLine($"No reservations for car {car.Id} found, don't need to check further.");
                return;
            }

            var latestReservation = relatedReservations.OrderBy(r => r.To).Last();
            if (latestReservation.ReservationsNr == reservation.ReservationsNr)
            {
                Console.WriteLine("Latest reservation is the one we're modifying, don't need to check further.");
                return;
            }

            if (latestReservation.To > reservation.From)
            {
                throw new AutoUnavailableException(reservation.From, latestReservation.To);
            }
        }
    }
}