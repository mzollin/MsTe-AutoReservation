using AutoReservation.Common.DataTransferObjects;
using System.Collections.Generic;
using System.ServiceModel;
using AutoReservation.Common.DataTransferObjects.Faults;

namespace AutoReservation.Common.Interfaces
{
    [ServiceContract]
    public interface IAutoReservationService
    {
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void Create(AutoDto car);
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void Create(ReservationDto reservation);
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void Create(KundeDto customer);

        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        AutoDto ReadCar(int id);
        [OperationContract]
        IEnumerable<AutoDto> ReadAllCars();
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        ReservationDto ReadReservation(int id);
        [OperationContract]
        IEnumerable<ReservationDto> ReadAllReservations();
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        KundeDto ReadCustomer(int id);
        [OperationContract]
        IEnumerable<KundeDto> ReadAllCustomers();

        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void Update(AutoDto car);
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void Update(ReservationDto reservation);
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void Update(KundeDto customer);

        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void Delete(AutoDto car);
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void Delete(ReservationDto reservation);
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void Delete(KundeDto customer);

        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        bool IsCarAvailable(int id);
        [OperationContract]
        IEnumerable<AutoDto> GetAllAvailableCars();
    }
}
