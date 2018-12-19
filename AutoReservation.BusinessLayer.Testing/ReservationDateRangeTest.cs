using AutoReservation.TestEnvironment;
using System;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationDateRangeTest
        : TestBase
    {
        private ReservationManager target;
        private ReservationManager Target => target ?? (target = new ReservationManager());

        private static readonly DateTime BaseDate = new DateTime(2100, 01, 01);
        private static readonly DateTime Exactly24h = BaseDate.AddHours(24);
        private static readonly DateTime MoreThan24h = BaseDate.AddDays(5).AddHours(13);
        private static readonly DateTime LessThan24h = BaseDate.AddHours(23).AddMinutes(59).AddSeconds(59);

        [Fact]
        public void ScenarioOkay01Test_Exactly24Hours()
        {
            var reservation = new Reservation
            {
                AutoId = 1,
                KundeId = 1,
                From = BaseDate,
                To = Exactly24h
            };

            ReservationManager.CheckDateRange(reservation);
        }

        [Fact]
        public void ScenarioOkay02Test_MoreThan24Hours()
        {
            var reservation = new Reservation
            {
                AutoId = 1,
                KundeId = 1,
                From = BaseDate,
                To = MoreThan24h
            };

            ReservationManager.CheckDateRange(reservation);
        }

        [Fact]
        public void ScenarioNotOkay01Test_LessThan24Hours()
        {
            var reservation = new Reservation
            {
                AutoId = 1,
                KundeId = 1,
                From = BaseDate,
                To = LessThan24h
            };

            Action a = () => ReservationManager.CheckDateRange(reservation);
            Assert.Throws<InvalidDateRangeException>(a);
        }

        [Fact]
        public void ScenarioNotOkay02Test_LessThan24Hours_FromToInverted()
        {
            var reservation = new Reservation
            {
                AutoId = 1,
                KundeId = 1,
                From = LessThan24h,
                To = BaseDate
            };

            Action a = () => ReservationManager.CheckDateRange(reservation);
            Assert.Throws<InvalidDateRangeException>(a);
        }

        [Fact]
        public void ScenarioNotOkay03Test_MoreThan24Hours_FromToInverted()
        {
            var reservation = new Reservation
            {
                AutoId = 1,
                KundeId = 1,
                From = MoreThan24h,
                To = BaseDate
            };

            Action a = () => ReservationManager.CheckDateRange(reservation);
            Assert.Throws<InvalidDateRangeException>(a);
        }
    }
}
