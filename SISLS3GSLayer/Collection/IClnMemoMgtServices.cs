#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Collection
/// Screen Name			: Memo Master
/// Created By			: Kannan RC
/// Created Date		: 11-oCT-2010
/// Purpose	            : WCF Interface class for defining Invoice vendor details methods

/// <Program Summary>
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;

namespace S3GServiceLayer.Collection
{
    // NOTE: If you change the interface name "IClnMemoMgtServices" here, you must also update the reference to "IClnMemoMgtServices" in Web.config.
    [ServiceContract]
    public interface IClnMemoMgtServices
    {

        #region Memorandum Booking
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateMemorandumBooking(SerializationMode SMode, byte[] byteObjS3G_Colection_MemorandumBooking, out string strMemoBookNo);
        #endregion

        #region MemoMaster

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateMemoDetails(SerializationMode SMode, byte[] byteObjS3G_Colection_MemoMaster);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyMemoDetails(SerializationMode SMode, byte[] byteObjS3G_Colection_MemoMaster);
        #endregion

    }
}
