﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.DataTransferObjects.Faults;
using AutoReservation.Common.Interfaces;
using AutoReservation.Dal.Entities;

namespace AutoReservation.Service.Wcf
{
    public class AutoReservationService : IAutoReservationService
    {
        private readonly AutoManager _autoManager;
        private readonly KundeManager _kundeManager;
        private readonly ReservationManager _reservationManager;

        public AutoReservationService()
        {
            _autoManager = new AutoManager();
            _kundeManager = new KundeManager();
            _reservationManager = new ReservationManager();
        }

        #region Create
        public void Create(AutoDto car)
        {
            WriteActualMethod();
            _autoManager.Create(car.ConvertToEntity());
        }

        public void Create(ReservationDto reservation)
        {
            WriteActualMethod();

            try
            {
                _reservationManager.Create(reservation.ConvertToEntity());
            }
            catch (InvalidDateRangeException ex)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.RentalPeriodNotAllowed,
                    ErrorMessage = ex.Message
                });
            }
            catch (AutoUnavailableException ex)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.CarNotAvailable,
                    ErrorMessage = ex.Message
                });
            }
        }

        public void Create(KundeDto customer)
        {
            WriteActualMethod();

            try
            {
                _kundeManager.Create(customer.ConvertToEntity());
            }
            catch (CustomerNotOfAgeException ex)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.CustomerNot18YearsOld,
                    ErrorMessage = ex.Message
                });
            }
        }
        #endregion

        #region Read
        public AutoDto ReadCar(int id)
        {
            WriteActualMethod();

            var car = _autoManager.Read(id);
            if (car == null)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.RequestedEntityDoesNotExist
                });
            }

            return car.ConvertToDto();
        }

        public IEnumerable<AutoDto> ReadAllCars()
        {
            WriteActualMethod();
            return _autoManager.ReadAll().ConvertToDtos();
        }

        public ReservationDto ReadReservation(int id)
        {
            WriteActualMethod();

            var reservation = _reservationManager.Read(id);
            if (reservation == null)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.RequestedEntityDoesNotExist
                });
            }

            return reservation.ConvertToDto();
        }

        public IEnumerable<ReservationDto> ReadAllReservations()
        {
            WriteActualMethod();
            return _reservationManager.ReadAll().ConvertToDtos();
        }

        public KundeDto ReadCustomer(int id)
        {
            WriteActualMethod();

            var customer = _kundeManager.Read(id);
            if (customer == null)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.RequestedEntityDoesNotExist
                });
            }

            return customer.ConvertToDto();
        }

        public IEnumerable<KundeDto> ReadAllCustomers()
        {
            WriteActualMethod();
            return _kundeManager.ReadAll().ConvertToDtos();
        }
        #endregion

        #region Update
        public void Update(AutoDto car)
        {
            WriteActualMethod();

            try
            {
                _autoManager.Update(car.ConvertToEntity());
            }
            catch (OptimisticConcurrencyException<Auto> ex)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.DataHasBeenModifiedInMeantime,
                    ErrorMessage = $"Database Entity-State: {ex.MergedEntity.ConvertToDto()}"
                });
            }
        }

        public void Update(ReservationDto reservation)
        {
            WriteActualMethod();

            try
            {
                _reservationManager.Update(reservation.ConvertToEntity());
            }
            catch (OptimisticConcurrencyException<Reservation> ex)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.DataHasBeenModifiedInMeantime,
                    ErrorMessage = $"Database Entity-State: {ex.MergedEntity.ConvertToDto()}"
                });
            }
            catch (InvalidDateRangeException ex)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.RentalPeriodNotAllowed,
                    ErrorMessage = ex.Message
                });
            }
            catch (AutoUnavailableException ex)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.CarNotAvailable,
                    ErrorMessage = ex.Message
                });
            }
        }

        public void Update(KundeDto customer)
        {
            WriteActualMethod();

            try
            {
                _kundeManager.Update(customer.ConvertToEntity());
            }
            catch (OptimisticConcurrencyException<Kunde> ex)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.DataHasBeenModifiedInMeantime,
                    ErrorMessage = $"Database Entity-State: {ex.MergedEntity.ConvertToDto()}"
                });
            }
        }
        #endregion

        #region Delete
        public void Delete(AutoDto car)
        {
            WriteActualMethod();

            try
            {
                _autoManager.Delete(car.ConvertToEntity());
            }
            catch (OptimisticConcurrencyException<Auto> ex)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.DataHasBeenModifiedInMeantime,
                    ErrorMessage = $"Database Entity-State: {ex.MergedEntity.ConvertToDto()}"
                });
            }
        }

        public void Delete(ReservationDto reservation)
        {
            WriteActualMethod();

            try
            {
                _reservationManager.Delete(reservation.ConvertToEntity());
            }
            catch (OptimisticConcurrencyException<Reservation> ex)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.DataHasBeenModifiedInMeantime,
                    ErrorMessage = $"Database Entity-State: {ex.MergedEntity.ConvertToDto()}"
                });
            }
        }

        public void Delete(KundeDto customer)
        {
            WriteActualMethod();

            try
            {
                _kundeManager.Delete(customer.ConvertToEntity());
            }
            catch (OptimisticConcurrencyException<Kunde> ex)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.DataHasBeenModifiedInMeantime,
                    ErrorMessage = $"Database Entity-State: {ex.MergedEntity.ConvertToDto()}"
                });
            }
        }
        #endregion

        public bool IsCarAvailable(int id)
        {
            WriteActualMethod();

            if (_autoManager.Read(id) == null)
            {
                throw new FaultException<AutoReservationFault>(new AutoReservationFault
                {
                    ErrorCode = AutoReservationFault.RequestedEntityDoesNotExist
                });
            }

            return _reservationManager.ReadAllForGivenAuto(id).OrderBy(r => r.To).Last().To < DateTime.Now;
        }

        public IEnumerable<AutoDto> GetAllAvailableCars()
        {
            WriteActualMethod();

            return _reservationManager.ReadAll()
                                      .GroupBy(r => r.AutoId)
                                      .Select(e => e.OrderBy(r => r.To).Last())
                                      .Where(r => r.To < DateTime.Now)
                                      .Select(r => r.Auto)
                                      .ConvertToDtos();
        }

        private static void WriteActualMethod()
            => Console.WriteLine($"Calling: {new StackTrace().GetFrame(1).GetMethod().Name}");
    }
}