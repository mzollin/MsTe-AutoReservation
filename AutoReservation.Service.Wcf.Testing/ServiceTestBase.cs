using System;
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
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void DeleteKundeTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void DeleteReservationTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        #endregion

        #region Update

        [Fact]
        public void UpdateAutoTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void UpdateKundeTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void UpdateReservationTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        #endregion

        #region Update with optimistic concurrency violation

        [Fact]
        public void UpdateAutoWithOptimisticConcurrencyTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void UpdateKundeWithOptimisticConcurrencyTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void UpdateReservationWithOptimisticConcurrencyTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        #endregion

        #region Insert / update invalid time range

        [Fact]
        public void InsertReservationWithInvalidDateRangeTest()
        {
            throw new NotImplementedException("Test not implemented.");
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
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void UpdateReservationWithAutoNotAvailableTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        #endregion

        #region Check Availability

        [Fact]
        public void CheckAvailabilityIsTrueTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        [Fact]
        public void CheckAvailabilityIsFalseTest()
        {
            throw new NotImplementedException("Test not implemented.");
        }

        #endregion
    }
}
