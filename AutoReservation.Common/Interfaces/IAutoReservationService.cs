using AutoReservation.Common.DataTransferObjects;
using System.Collections.Generic;
using System.ServiceModel;
using AutoReservation.Common.DataTransferObjects.Faults;

namespace AutoReservation.Common.Interfaces
{
    [ServiceContract]
    public interface IAutoReservationService
    {
        #region Create
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void CreateAuto(AutoDto car);
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void CreateReservation(ReservationDto reservation);
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void CreateKunde(KundeDto customer);
        #endregion

        #region Read
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        AutoDto ReadAuto(int id);
        [OperationContract]
        IEnumerable<AutoDto> ReadAllAutos();
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        ReservationDto ReadReservation(int id);
        [OperationContract]
        IEnumerable<ReservationDto> ReadAllReservationen();
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        KundeDto ReadKunde(int id);
        [OperationContract]
        IEnumerable<KundeDto> ReadAllKunden();
        #endregion

        #region Update
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void UpdateAuto(AutoDto car);
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void UpdateReservation(ReservationDto reservation);
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void UpdateKunde(KundeDto customer);
        #endregion

        #region Delete
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void DeleteAuto(AutoDto car);
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void DeleteReservation(ReservationDto reservation);
        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        void DeleteKunde(KundeDto customer);
        #endregion

        [OperationContract]
        [FaultContract(typeof(AutoReservationFault))]
        bool IsAutoAvailable(int id);
        [OperationContract]
        IEnumerable<AutoDto> GetAllAvailableAutos();
    }
}
