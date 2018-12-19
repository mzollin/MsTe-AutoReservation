using System;
using System.Collections.Generic;
using System.Linq;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationUpdateTest
        : TestBase
    {
        private ReservationManager target;
        private ReservationManager Target => target ?? (target = new ReservationManager());

        [Fact]
        public void CreateReservationTest()
        {
            var existingReservation = Target.GetById(1);

            var newReservation = new Reservation
            {
                Kunde = existingReservation.Kunde,
                Auto = existingReservation.Auto,
                From = new DateTime(2050, 01, 01),
                To = new DateTime(2100, 01, 01)
            };
            var reservations = Target.GetAll();

            Target.Create(newReservation);

            Assert.ProperSuperset(new HashSet<Reservation>(reservations), new HashSet<Reservation>(Target.GetAll()));
            var newReservationFromDb = Target.GetById(reservations.OrderBy(r => r.ReservationsNr).Last().ReservationsNr + 1);
            Assert.NotNull(newReservationFromDb);
            Assert.Equal(newReservation.Kunde.FirstName, newReservationFromDb.Kunde.FirstName);
            Assert.Equal(newReservation.Auto.Brand, newReservationFromDb.Auto.Brand);
            Assert.Equal(newReservation.From, newReservationFromDb.From);
        }

        [Fact]
        public void UpdateReservationTest()
        {
            var reservation = Target.GetById(1);
            var newToDate = new DateTime(2100, 01, 01);
            Assert.NotEqual(newToDate, reservation.To);

            reservation.To = newToDate;
            Target.Update(reservation);

            var modifiedReservation = Target.GetById(reservation.ReservationsNr);
            Assert.Equal(newToDate, modifiedReservation.To);
        }

        [Fact]
        public void DeleteReservationTest()
        {
            var reservation = Target.GetById(1);
            Assert.NotNull(reservation);

            Target.Delete(reservation);

            var deletedReservation = Target.GetById(1);
            Assert.Null(deletedReservation);
        }
    }
}
