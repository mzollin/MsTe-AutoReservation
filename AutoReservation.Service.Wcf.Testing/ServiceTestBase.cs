﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.DataTransferObjects.Faults;
using AutoReservation.Common.Interfaces;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.Service.Wcf.Testing
{
    public abstract class ServiceTestBase
        : TestBase
    {
        protected abstract IAutoReservationService Target { get; }

        #region Read all entities

        [Fact]
        public void GetAutosTest()
        {
            var cars = Target.ReadAllAutos().Count();
            Assert.Equal(4, cars);
        }

        [Fact]
        public void GetKundenTest()
        {
            var customers = Target.ReadAllKunden().Count();
            Assert.Equal(4, customers);
        }

        [Fact]
        public void GetReservationenTest()
        {
            var reservations = Target.ReadAllReservationen().Count();
            Assert.Equal(4, reservations);
        }

        #endregion

        #region Get by existing ID

        [Fact]
        public void GetAutoByIdTest()
        {
            var car = Target.ReadAuto(1);
            Assert.Equal(1, car.Id);
        }

        [Fact]
        public void GetKundeByIdTest()
        {
            var customer = Target.ReadKunde(1);
            Assert.Equal(1, customer.Id);
        }

        [Fact]
        public void GetReservationByNrTest()
        {
            var reservation = Target.ReadReservation(1);
            Assert.Equal(1, reservation.ReservationsNr);
        }

        #endregion

        #region Get by not existing ID

        [Fact]
        public void GetAutoByIdWithIllegalIdTest()
        {
            try
            {
                var car = Target.ReadAuto(42);
            }
            catch (FaultException<AutoReservationFault> e)
            {
                Assert.Equal(AutoReservationFault.RequestedEntityDoesNotExist, e.Detail.ErrorCode);
            }
        }

        [Fact]
        public void GetKundeByIdWithIllegalIdTest()
        {
            try
            {
                var customer = Target.ReadKunde(23);
            }
            catch (FaultException<AutoReservationFault> e)
            {
                Assert.Equal(AutoReservationFault.RequestedEntityDoesNotExist, e.Detail.ErrorCode);
            }
        }

        [Fact]
        public void GetReservationByNrWithIllegalIdTest()
        {
            try
            {
                var car = Target.ReadReservation(666);
            }
            catch (FaultException<AutoReservationFault> e)
            {
                Assert.Equal(AutoReservationFault.RequestedEntityDoesNotExist, e.Detail.ErrorCode);
            }
        }

        #endregion

        #region Insert

        [Fact]
        public void InsertAutoTest()
        {
            var before = Target.ReadAllAutos().Count();
            Target.CreateAuto(new AutoDto { Brand = "VW Passat", DailyRate = 150});
            var after = Target.ReadAllAutos().Count();
            Assert.Equal(after, before + 1);
        }

        [Fact]
        public void InsertKundeTest()
        {
            var before = Target.ReadAllKunden().Count();
            Target.CreateKunde(new KundeDto { Surname = "Wäsche", FirstName = "Andy", Birthday = new DateTime(1989, 01, 02) });
            var after = Target.ReadAllKunden().Count();
            Assert.Equal(after, before + 1);
        }

        [Fact]
        public void InsertReservationTest()
        {
            var before = Target.ReadAllReservationen().Count();
            var car = Target.ReadAuto(2);
            var customer = Target.ReadKunde(3);
            Target.CreateReservation(new ReservationDto { Car = car, Customer = customer,
                                                          From = new DateTime(2120, 01, 10),
                                                          To = new DateTime(2120, 01, 20) });
            var after = Target.ReadAllReservationen().Count();
            Assert.Equal(after, before + 1);
        }

        #endregion

        #region Delete  

        [Fact]
        public void DeleteAutoTest()
        {
            var before = Target.ReadAllAutos().Count();
            Target.DeleteAuto(Target.ReadAuto(1));
            var after = Target.ReadAllAutos().Count();
            Assert.Equal(after, before - 1);
        }

        [Fact]
        public void DeleteKundeTest()
        {
            var before = Target.ReadAllKunden().Count();
            Target.DeleteKunde(Target.ReadKunde(1));
            var after = Target.ReadAllKunden().Count();
            Assert.Equal(after, before - 1);
        }

        [Fact]
        public void DeleteReservationTest()
        {
            var before = Target.ReadAllReservationen().Count();
            Target.DeleteReservation(Target.ReadReservation(1));
            var after = Target.ReadAllReservationen().Count();
            Assert.Equal(after, before - 1);
        }

        #endregion

        #region Update

        [Fact]
        public void UpdateAutoTest()
        {
            var car = Target.ReadAuto(2);
            car.Brand = "VW Passat";
            Target.UpdateAuto(car);
            Assert.Equal("VW Passat", Target.ReadAuto(2).Brand);
        }

        [Fact]
        public void UpdateKundeTest()
        {
            var customer = Target.ReadKunde(1);
            customer.Surname = "Gramm";
            Target.UpdateKunde(customer);
            Assert.Equal("Gramm", Target.ReadKunde(1).Surname);
        }

        [Fact]
        public void UpdateReservationTest()
        {
            var reservation = Target.ReadReservation(4);
            reservation.To = new DateTime(2020, 07, 19);
            Target.UpdateReservation(reservation);
            Assert.Equal(new DateTime(2020, 07, 19), Target.ReadReservation(4).To);
        }

        #endregion

        #region Update with optimistic concurrency violation

        [Fact]
        public void UpdateAutoWithOptimisticConcurrencyTest()
        {
            var car = Target.ReadAuto(2);
            car.Brand = "VW Passat";
            Target.UpdateAuto(car);
            car.Brand = "VW Passnot";

            try
            {
                Target.UpdateAuto(car);
            }
            catch (FaultException<AutoReservationFault> e)
            {
                Assert.Equal(AutoReservationFault.DataHasBeenModifiedInMeantime, e.Detail.ErrorCode);
            }
        }

        [Fact]
        public void UpdateKundeWithOptimisticConcurrencyTest()
        {
            var customer = Target.ReadKunde(1);
            customer.Surname = "Gramm";
            Target.UpdateKunde(customer);
            customer.Surname = "Bolika";

            try
            {
                Target.UpdateKunde(customer);
            }
            catch (FaultException<AutoReservationFault> e)
            {
                Assert.Equal(AutoReservationFault.DataHasBeenModifiedInMeantime, e.Detail.ErrorCode);
            }
        }

        [Fact]
        public void UpdateReservationWithOptimisticConcurrencyTest()
        {
            var reservation = Target.ReadReservation(4);
            reservation.To = new DateTime(2020, 07, 19);
            Target.UpdateReservation(reservation);
            reservation.To = new DateTime(2020, 08, 19);


            try
            {
                Target.UpdateReservation(reservation);
            }
            catch (FaultException<AutoReservationFault> e)
            {
                Assert.Equal(AutoReservationFault.DataHasBeenModifiedInMeantime, e.Detail.ErrorCode);
            }
        }

        #endregion

        #region Insert / update invalid time range

        [Fact]
        public void InsertReservationWithInvalidDateRangeTest()
        {
            var car = Target.ReadAuto(2);
            var customer = Target.ReadKunde(3);
            var reservation = new ReservationDto
            {
                Car = car,
                Customer = customer,
                From = new DateTime(2030, 01, 20),
                To = new DateTime(2030, 01, 10)
            };

            try
            {
                Target.CreateReservation(reservation);
            }
            catch (FaultException<AutoReservationFault> e)
            {
                Assert.Equal(AutoReservationFault.RentalPeriodNotAllowed, e.Detail.ErrorCode);
            }
        }

        [Fact]
        public void InsertReservationWithAutoNotAvailableTest()
        {
            var car = Target.ReadAuto(2);
            var customer = Target.ReadKunde(3);
            var reservation = new ReservationDto
            {
                Car = car,
                Customer = customer,
                From = new DateTime(2020, 01, 10),
                To = new DateTime(2020, 01, 20)
            };

            try
            {
                Target.CreateReservation(reservation);
            }
            catch (FaultException<AutoReservationFault> e)
            {
                Assert.Equal(AutoReservationFault.CarNotAvailable, e.Detail.ErrorCode);
            }
        }

        [Fact]
        public void UpdateReservationWithInvalidDateRangeTest()
        {
            var reservation = Target.ReadReservation(4);
            reservation.To = new DateTime(2020, 05, 19);

            try
            {
                Target.UpdateReservation(reservation);
            }
            catch (FaultException<AutoReservationFault> e)
            {

                Assert.Equal(AutoReservationFault.RentalPeriodNotAllowed, e.Detail.ErrorCode);
            }
        }

        [Fact]
        public void UpdateReservationWithAutoNotAvailableTest()
        {
            var reservation = Target.ReadReservation(1);
            reservation.Car = Target.ReadAuto(2);

            try
            {
                Target.UpdateReservation(reservation);
            }
            catch (FaultException<AutoReservationFault> e)
            {

                Assert.Equal(AutoReservationFault.CarNotAvailable, e.Detail.ErrorCode);
            }
        }

        #endregion

        #region Check Availability

        [Fact]
        public void CheckAvailabilityIsTrueTest()
        {
            Assert.True(Target.IsAutoAvailable(3, new DateTime(2120, 01, 10),
                                                  new DateTime(2120, 01, 20)));
        }

        [Fact]
        public void CheckAvailabilityIsFalseTest()
        {
            Assert.False(Target.IsAutoAvailable(2, new DateTime(2020, 01, 10),
                                                   new DateTime(2020, 01, 20)));
        }

        #endregion
    }
}
