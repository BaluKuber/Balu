using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;

namespace S3GServiceLayer.Collection
{
    // NOTE: If you change the interface name "IClnDataMgtServices" here, you must also update the reference to "IClnDataMgtServices" in Web.config.
    [ServiceContract]
    public interface IClnDataMgtServices
    {
        #region Appropriation Logic

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateFollowUpProcess(SerializationMode SerMode, byte[] bytesFollowUpDAL_SERLAY, out int intFollowUpID, out string strDocNo);

        #endregion

        #region CRM

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCRM(SerializationMode SerMode, byte[] byteCRMService, out int intCRMID, out string strDocNo, out int intCustomerID);


        #region "Create/Update Prospect"

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateProspect(SerializationMode SerMode, byte[] bytesPspct_Data, out Int64 intCRMID);

        #endregion

        #region "Create/Update Lead Details"

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateLead(SerializationMode SerMode, byte[] bytesObjS3G_Colection_Customer_DataTable, out string strDocNo, out Int64 intLeadID);

        #endregion

        #region "Create Track Details"

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateTrackDetails(SerializationMode SerMode, byte[] bytesTrack_Data, out Int64 intTrackID);

        #endregion

        #region "Create Document Details"

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateDocumentDetails(SerializationMode SerMode, byte[] bytesDoc_Data, out Int64 intDocID);

        #endregion

        #endregion

        #region Customer

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateProspectCust(SerializationMode SerMode, byte[] bytesObjS3G_Colection_Customer_DataTable, out string strDocNo, out int intCustomerID);

        #endregion
    }
}
