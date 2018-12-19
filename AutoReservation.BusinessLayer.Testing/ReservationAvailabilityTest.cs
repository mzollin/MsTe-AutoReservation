using System;
using System.Linq;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationAvailabilityTest
        : TestBase
    {
        private ReservationManager target;
        private ReservationManager Target => target ?? (target = new ReservationManager());

        private readonly Auto PreparedCar;
        private readonly Kunde PreparedKunde;

        public ReservationAvailabilityTest()
        {
            var reservationA = Target.GetById(1);
            reservationA.From = DateTime.Today;
            reservationA.To = DateTime.Today.AddDays(3);
            Target.Update(reservationA);

            var reservationB = new Reservation
            {
                From = DateTime.Today.AddDays(7),
                To = DateTime.Today.AddDays(10),
                Auto = reservationA.Auto,
                Kunde = reservationA.Kunde
            };
            Target.Create(reservationB);

            PreparedCar = reservationA.Auto;
            PreparedKunde = reservationA.Kunde;
        }

        [Fact]
        public void ScenarioOkay01Test_NoReservationsForCarYet()
        {
            var car = new StandardAuto
            {
                Brand = "PaddyWaggon",
                DailyRate = 120
            };

            var autoManager = new AutoManager();
            autoManager.Create(car);

            var reservation = new Reservation
            {
                Auto = autoManager.GetAll().OrderBy(c => c.Id).Last(),
                KundeId = 1,
                From = DateTime.Today,
                To = DateTime.Today.AddDays(2)
            };

            ReservationManager.CheckCarAvailability(reservation, Target);
        }

        [Fact]
        public void ScenarioOkay02Test_ModifyingOnlyReservation()
        {
            var reservationen = Target.GetAllForGivenAuto(3).ToList();
            Assert.Equal(1, reservationen.Count);

            var reservation = reservationen.First();
            reservation.To = new DateTime(2100, 1, 1);

            ReservationManager.CheckCarAvailability(reservation, Target);
        }

        [Fact]
        public void ScenarioOkay03Test_ReservationOutsideRange_AfterLatestTo()
        {
            var res = new Reservation
            {
                From = DateTime.Today.AddDays(10),
                To = DateTime.Today.AddDays(12),
                Auto = PreparedCar,
                Kunde = PreparedKunde
            };
            Target.Create(res);
        }


        [Fact]
        public void ScenarioOkay04Test_ReservationOutsideRange_BeforeFirstFrom()
        {
            var res = new Reservation
            {
                From = DateTime.Today.AddDays(-2),
                To = DateTime.Today,
                Auto = PreparedCar,
                Kunde = PreparedKunde
            };
            Target.Create(res);
        }

        [Fact]
        public void ScenarioOkay05Test_ReservationInsideRange_BetweenOthers()
        {
            var res = new Reservation
            {
                From = DateTime.Today.AddDays(4),
                To = DateTime.Today.AddDays(6),
                Auto = PreparedCar,
                Kunde = PreparedKunde
            };
            Target.Create(res);
        }

        [Fact]
        public void ScenarioNotOkay01Test_AnyToAfterReservationFrom()
        {
            var res = new Reservation
            {
                From = DateTime.Today.AddDays(2),
                To = DateTime.Today.AddDays(5),
                Auto = PreparedCar,
                Kunde = PreparedKunde
            };
            Action a = () => Target.Create(res);

            Assert.Throws<AutoUnavailableException>(a);
        }

        [Fact]
        public void ScenarioNotOkay02Test_AnyFromBeforeReservationTo()
        {
            var res = new Reservation
            {
                From = DateTime.Today.AddDays(5),
                To = DateTime.Today.AddDays(8),
                Auto = PreparedCar,
                Kunde = PreparedKunde
            };
            Action a = () => Target.Create(res);

            Assert.Throws<AutoUnavailableException>(a);
        }

        [Fact]
        public void ScenarioNotOkay03Test_AnyCompleteOverlapWithReservation()
        {
            var res = new Reservation
            {
                From = DateTime.Today.AddDays(-1),
                To = DateTime.Today.AddDays(4),
                Auto = PreparedCar,
                Kunde = PreparedKunde
            };
            Action a = () => Target.Create(res);

            Assert.Throws<AutoUnavailableException>(a);
        }

        [Fact]
        public void ScenarioNotOkay04Test_AnyPerfectOverlapWithReservation()
        {
            var res = new Reservation
            {
                From = DateTime.Today,
                To = DateTime.Today.AddDays(3),
                Auto = PreparedCar,
                Kunde = PreparedKunde
            };
            Action a = () => Target.Create(res);

            Assert.Throws<AutoUnavailableException>(a);
        }

        [Fact]
        public void ScenarioNotOkay05Test_AnyInnerOverlapWithReservation()
        {
            var res = new Reservation
            {
                From = DateTime.Today.AddDays(1),
                To = DateTime.Today.AddDays(2),
                Auto = PreparedCar,
                Kunde = PreparedKunde
            };
            Action a = () => Target.Create(res);

            Assert.Throws<AutoUnavailableException>(a);
        }
    }
}
