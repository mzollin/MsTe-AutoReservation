using AutoReservation.Common.DataTransferObjects;
using System.Collections.Generic;
using System.ServiceModel;

namespace AutoReservation.Common.Interfaces
{
    [ServiceContract]
    public interface IAutoReservationService
    {
        [OperationContract]
        void Create(AutoDto car);
        [OperationContract]
        void Create(ReservationDto reservation);
        [OperationContract]
        void Create(KundeDto customer);

        [OperationContract]
        AutoDto ReadCar(int id);
        [OperationContract]
        IEnumerable<AutoDto> ReadAllCars();
        [OperationContract]
        ReservationDto ReadReservation(int id);
        [OperationContract]
        IEnumerable<ReservationDto> ReadAllReservations();
        [OperationContract]
        KundeDto ReadCustomer(int id);
        [OperationContract]
        IEnumerable<KundeDto> ReadAllCustomers();

        [OperationContract]
        void Update(AutoDto car);
        [OperationContract]
        void Update(ReservationDto reservation);
        [OperationContract]
        void Update(KundeDto customer);

        [OperationContract]
        void Delete(AutoDto car);
        [OperationContract]
        void Delete(ReservationDto reservation);
        [OperationContract]
        void Delete(KundeDto customer);

        [OperationContract]
        bool IsCarAvailable(int id);
        [OperationContract]
        IEnumerable<AutoDto> GetAllAvailableCars();
    }
}
