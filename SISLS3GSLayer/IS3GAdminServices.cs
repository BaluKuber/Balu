#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Common
/// Screen Name			: Common DAL Class
/// Created By			: Nataraj Y
/// Created Date		: 10-May-2010
/// Purpose	            : 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 21-Jul-2010
/// Reason              : To implement code review points
/// <Program Summary>
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using S3GBusEntity;
using System.Collections;


namespace S3GServiceLayer
{
    // NOTE: If you change the interface name "IS3GAdminServices" here, you must also update the reference to "IS3GAdminServices" in Web.config.
    [ServiceContract]
    public interface IS3GAdminServices
    {

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubFillDropdown(string strProcName, Dictionary<string, string> dctProcParams);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubFillDataset(string strProcName, Dictionary<string, string> dctProcParams);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        string FunGetScalarValue(string strProcName, Dictionary<string, string> dctProcParams);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        string FunValidateMonthClosure(string strProcName, Dictionary<string, string> dctProcParams);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubFillGridPaging(string strProcName, Dictionary<string, string> dctProcParams, out int intTotalRecords, PagingValues ObjPaging);

        [OperationContract(Name = "FunPubFillGridPagingWithErrorCode")]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubFillGridPaging(string strProcName, Dictionary<string, string> dctProcParams, out int intTotalRecords, out int intErrorCode, PagingValues ObjPaging);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubValidateLogin(string strUserLoginCode, string strPassword, out int intCompanyID, out int intUserID,
            out int intUser_Level_ID, out string strUserName, out string strCompanyName, out string strCompanyCode,
            out DateTime LastLoginDate, out string strTheme, out string strAccess, out string strCountryName,
            out string strUserType, out string strMarquee, out int loginDiff, int SSOEnabled);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubForgetPassword(string strUserLoginCode, string strPassword, int intCompanyID,
            out string strUserName, out string strUserMailID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubValidateDemoLogin(int intNewUser, string strUserLoginCode, string strFullName, string strMobile, string strEmail, string strDesignation, string strCompany_Name, string strBriefAbt, string strCurSys, string strPassword, out int intCompanyID, out int intUserID, out int intUser_Level_ID, out string strUserName, out string strCompanyName, out string strCompanyCode, out DateTime LastLoginDate, out string strTheme, out string strAccess, out string strCountryName, out string strUserType, out string strMarquee);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetUserMenu(int intUserId, string strUserName, string strUserType);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetWorkflowDocuments(int intCompanyId, int intUserId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetWorkflowToDoList(int intCompanyId, int intUserId, bool ShowAll, DateTime? FromDate, DateTime? ToDate);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        void FunPubSysJournalEntry(ClsSystemJournal ObjSysJournal);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubExecuteSQL(Dictionary<string, string> proceParams);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubFillOutputDataTable(string strProcName, Dictionary<string, string> dctProcParams, Dictionary<string, string> dctProcParamsOutput, out byte[] byteOuput);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAuditTrial(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjAuditTrialDatatable_SERLAY);


        #region [Location Master Details]

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertLocationCategory(SerializationMode SerMode, byte[] bytesLocation_Datatable, out string strExistingCode, out string strExistingDescription);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateLocationCategory(SerializationMode SerMode, byte[] bytesLocation_Datatable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryLocationCategoryDetails(int intCompany_Id, int intUser_ID, int intLocationCategory_Id, SerializationMode SerMode);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertLocationMaster(SerializationMode SerMode, byte[] bytesLocation_Datatable, out string strExistingMapping);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateLocationMapping(SerializationMode SerMode, byte[] bytesLocation_Datatable);
        #endregion [Location Master Details]

        #region [Password Validation]
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubPasswordValidation(Int32 intUserID, string strPassword);
        #endregion [Password Validation]


        #region EMI Calculation Save

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubSaveEMICalc(SerializationMode SerMode, byte[] bytesDictionary);

        #endregion

        #region ErrorLog
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        string FunPubSysErrorLog(string strProcName, Dictionary<string, string> dctProcParams);
        #endregion

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertUserLoginDetails(SerializationMode SerMode, byte[] bytesObjPRDDTransDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateLogOutFlags(int intCompanyID, int intUserID, String Session_ID, String UserIDs, int LogOut_Status_Code);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateUserIsActive(DateTime ActiveDateTime, int User_ID, int Company_ID, int Manual_Logged_Out);

    }
}
