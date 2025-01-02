using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using System.Data;

namespace S3GServiceLayer.Collection
{
    // NOTE: If you change the interface name "IClnReceiptMgtServicesReference" here, you must also update the reference to "IClnReceiptMgtServicesReference" in Web.config.
    [ServiceContract]
    public interface IClnReceiptMgtServices
    {
        #region Appropriation Logic

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAppropriationLogic(SerializationMode SerMode, byte[] bytesAppLogicDAL_SERLAY, out string strApproID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunAppropriationLogicForModification(string strApproID, int Company_ID);

        #endregion

        #region Temporary Book Maintenance

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateTempBookDetails(SerializationMode SerMode, byte[] bytesTemporaryBookDAL_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateTempBookDetails(SerializationMode SerMode, byte[] byteObjS3G_Collection_TempReceiptBookMaster_DataTable, out string strUsedleafsNumber);

        #endregion

        #region Challan generation
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateChallanGenerationLogic(SerializationMode SerMode, byte[] bytesChallanGenerationDAL_SERLAY, out string strChallanNumber);

        #endregion

        #region "Cheque Return"

        //[OperationContract(Name = "OL_CreateChequeReturns")]
        //[FaultContract(typeof(ClsPubFaultException))]
        //int FunPubCreateChequeReturns(SerializationMode SerMode, byte[] bytesChequeReturnDAL_SERLAY);

        [OperationContract(Name = "OL_CreateChequeReturns_Memerandum")]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateChequeReturns(SerializationMode SerMode, byte[] bytesChequeReturnDAL_SERLAY, byte[] bytesMemorandumbookingDAL_SERLAY, out string strChequeReturnNo);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubChequeAuthorization(SerializationMode SerMode, byte[] bytesChequeReturnDAL_SERLAY);

        [OperationContract(Name = "OL_CreateChequeReturns_Excel")]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateChequeReturnsExcel(SerializationMode SerMode, byte[] bytesChequeReturnExcelDAL_SERLAY, out string strChequeReturnNo);
        #endregion


        #region ECS
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateECSFormat(SerializationMode SerMode, byte[] byteECSFomatService);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyECSFormat(SerializationMode SerMode, byte[] byteECSFomatService);


        #endregion

        #region ECS Process
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateECSProcess(SerializationMode SMode, byte[] byteECSProcesservice, out string strECSNumber);


        #endregion

        #region "Challan Rule Creation"
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateChallanRuleDetails(SerializationMode SerMode, byte[] bytesObjS3G_CLN_ChallanRuleDataTable, out string strChallanNo);
        #endregion


        #region "PDC Entry"
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreatePDCModuleDetails(SerializationMode SerMode, byte[] bytesObjS3G_CLN_PDCModuleDataTable, out string strPDCNo, out string strchequeNo, out string strexistingdate);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //int FunPubModifyPDCModuleDetails(SerializationMode SerMode, byte[] bytesObjS3G_CLN_PDCModuleDataTable);
        #endregion

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubGetPDC(SerializationMode SerMode, byte[] bytesObjAccountDetailsDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreatePDCBulkReceipt(SerializationMode SerMode, byte[] bytePDCFomatService);

        #region Receipt Processing
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateReceipt(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable, out string strReceiptNumber);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateTempReceipt(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable, out string strTempReceiptNumber);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCancelReceipt(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCancelTempReceipt(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateUTPAReceipt(SerializationMode SerMode, byte[] bytesObjUTPAReceiptProcessing_DataTable, out string strUTPAReceiptNumber);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        DataTable FunPubCheckDocDate(string strProcName, int intCompanyId, DateTime dtDocDate, string strRecType);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        DataTable FunPubLoadBookNo(string strProcName, int intOption, int intCompanyId, DateTime dtValueDate, DateTime dtDocDate, int intLobId, int intLocationId);
        #endregion

        #region [Receipt Authorization]

        /// <summary>
        /// Created By Tamilselvan.S
        /// Created Date 14/03/2011
        /// For Receipt Authorization
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="byteobjTable"></param>
        /// <param name="intReceiptID"></param>
        /// <returns></returns>
        [OperationContract(Name = "ReceiptAuthorize_DataTable")]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubReceiptAuthorize(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable, out string strReceiptID);


        /// <summary>
        /// Created By Tamilselvan.S
        /// Created Date 20/04/2011
        /// For Receipt Authorization
        /// </summary>
        /// <param name="intCompany_ID"></param>
        /// <param name="intUser_ID"></param>
        /// <param name="intReceipt_ID"></param>
        /// <param name="strReceiptID"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubReceiptAuthorize(int intCompany_ID, int intUser_ID, int intReceipt_ID, out string strReceiptID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubRevokeReceiptAuthorize(int intCompany_ID, int intUser_ID, int intReceipt_ID, out string strReceiptID);
        #endregion [Receipt Authorization]

        #region [UTPA Receipt Authorization and Revoke]

        /// <summary>
        /// Created By Tamilselvan.S
        /// Created Date 12/10/2011
        /// For UTPA Receipt Authorization
        /// </summary>
        /// <param name="intCompany_ID"></param>
        /// <param name="intUser_ID"></param>
        /// <param name="intUTPAReceipt_ID"></param>
        /// <param name="strUTPAReceiptID"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUTPAReceiptAuthorize(int intCompany_ID, int intUser_ID, int intUTPAReceipt_ID, out string strUTPAReceiptID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubRevokeUTPAReceiptAuthorize(int intCompany_ID, int intUser_ID, int intUTPAReceipt_ID, out string strUTPAReceiptID);

        #endregion [UTPA Receipt Authorization and Revoke]


        //Interface

        #region "PDC Bulk Replacement"
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreatePDCBulkModuleDetails(SerializationMode SerMode, byte[] bytesObjS3G_CLN_PDCModuleDataTable, out string strPDCNo, out string strchequeNo, out string strexistingdate);
        #endregion

        #region "Funder Receipt"

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateFunderReceipt(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable, out string strReceiptNumber);

        #endregion
    }
}
