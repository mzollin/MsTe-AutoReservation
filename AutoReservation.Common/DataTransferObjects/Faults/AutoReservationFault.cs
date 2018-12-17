using System.Runtime.Serialization;

namespace AutoReservation.Common.DataTransferObjects.Faults
{
    [DataContract]
    class AutoReservationFault
    {
        #region ErrorCodeConstants
        //Data Integrity
        public const string RequestedEntityDoesNotExist =   "ARE-D-001";
        public const string DatabaseTimestampsNotMatching = "ARE-D-002";
        public const string NegativeDateRange =             "ARE-D-003";
        public const string CarNotAvailable =               "ARE-D-004";
        public const string BirthdayInTheFuture =           "ARE-D-005";

        //Faulty Arguments
        public const string LuxuryCarMissingBaseRate =      "ARE-A-001";
        public const string CarMustNotHaveBaseRate =        "ARE-A-002";
        public const string CustomerNot18YearsOld =         "ARE-A-004";
        #endregion

        [DataMember]
        public string ErrorCode { get; set; }
    }
}
