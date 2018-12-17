using System.Runtime.Serialization;

namespace AutoReservation.Common.DataTransferObjects.Faults
{
    [DataContract]
    class AutoReservationFault
    {
        #region ErrorCodeConstants
        //Data Integrity
        public const string RequestedEntityDoesNotExist =   "ARE-D-001";
        public const string CannotCreateOrUpdateEntity =    "ARE-D-002";
        public const string DataHasBeenModifiedInMeantime = "ARE-D-003";

        //Faulty Arguments
        public const string RentalPeriodNotAllowed =        "ARE-A-001";
        public const string CarNotAvailable =               "ARE-A-002";
        public const string CustomerNot18YearsOld =         "ARE-A-003";
        #endregion

        [DataMember]
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
