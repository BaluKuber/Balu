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
/// Last Updated By		: Thalai    
/// Last Updated Date   : 31-Aug-2011
/// Reason              : 1) Password Validation with Encryption in Login Validations Region.
///                     : 2) Create "Password Validation" Region
/// <Program Summary>
#endregion

#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using S3GBusEntity;
using S3GDALayer.S3GAdminServices;
using System.Collections;
using System.ServiceModel.Activation;
#endregion

namespace S3GServiceLayer
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "S3GAdminServices" here, you must also update the reference to "S3GAdminServices" in Web.config.
    public class S3GAdminServices : IS3GAdminServices
    {
        byte[] bytesDataTable;
        byte[] bytesDataSet;
        int intErrorCode;
        #region IS3GAdminServices Members

        #region Login Validations
        /// <summary>
        /// Function to validate the specified user login credentials
        /// </summary>
        /// <param name="strUserLoginCode"></param>
        /// <param name="strPassword"></param>
        /// <param name="intCompanyID"></param>
        /// <param name="intUserID"></param>
        /// <param name="intUser_Level_ID"></param>
        /// <param name="strUserName"></param>
        /// <param name="strCompanyName"></param>
        /// <param name="strCompanyCode"></param>
        /// <param name="LastLoginDate"></param>
        /// <param name="strTheme"></param>
        /// <param name="strAccess"></param>
        /// <returns>Return error code Default i.e if no error it will return 0</returns>
        public int FunPubValidateLogin(string strUserLoginCode, string strPassword, out int intCompanyID,
            out int intUserID, out int intUser_Level_ID, out string strUserName, out string strCompanyName,
            out string strCompanyCode, out DateTime LastLoginDate, out string strTheme, out string strAccess,
            out string strCountryName, out string strUserType, out string strMarquee, out int loginDiff, int SSOEnabled)
        {
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                String StrPlainPassword = strPassword;
                intErrorCode = ObjCLsPubS3GAdmin.FunPubValidateLogin(strUserLoginCode, out strPassword, out intCompanyID,
                    out  intUserID, out intUser_Level_ID, out loginDiff, out strUserName, out  strCompanyName, out strCompanyCode,
                    out LastLoginDate, out strTheme, out strAccess, out strCountryName, out strUserType, out strMarquee,
                    SSOEnabled);
                if (intErrorCode == 0)
                {
                    String StrDescrypt = ClsPubCommCrypto.DecryptText(strPassword);
                    if (StrDescrypt != StrPlainPassword)
                    {
                        intErrorCode = 2; // Password Mismatch
                    }
                }
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Login :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intErrorCode;
        }

        public int FunPubForgetPassword(string strUserLoginCode, string strPassword, int intCompanyID,
            out string strUserName, out string strUserMailID)
        {
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                String StrPlainPassword = strPassword;
                intErrorCode = ObjCLsPubS3GAdmin.FunPubForgetPassword(strUserLoginCode, strPassword,
                    intCompanyID, out strUserName, out strUserMailID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Login :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intErrorCode;
        }

        #endregion

        #region Login Validations
        /// <summary>
        /// Function to validate the specified user login credentials
        /// </summary>
        /// <param name="strUserLoginCode"></param>
        /// <param name="strPassword"></param>
        /// <param name="intCompanyID"></param>
        /// <param name="intUserID"></param>
        /// <param name="intUser_Level_ID"></param>
        /// <param name="strUserName"></param>
        /// <param name="strCompanyName"></param>
        /// <param name="strCompanyCode"></param>
        /// <param name="LastLoginDate"></param>
        /// <param name="strTheme"></param>
        /// <param name="strAccess"></param>
        /// <returns>Return error code Default i.e if no error it will return 0</returns>
        public int FunPubValidateDemoLogin(int intNewUser, string strUserLoginCode, string strFullName, string strMobile, string strEmail, string strDesignation, string strCompany_Name, string strBriefAbt, string strCurSys, string strPassword, out int intCompanyID, out int intUserID, out int intUser_Level_ID, out string strUserName, out string strCompanyName, out string strCompanyCode, out DateTime LastLoginDate, out string strTheme, out string strAccess, out string strCountryName, out string strUserType, out string strMarquee)
        {
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                String StrPlainPassword = strPassword;
                intErrorCode = ObjCLsPubS3GAdmin.FunPubValidateDemoLogin(intNewUser, strUserLoginCode, strFullName, strMobile, strEmail, strDesignation, strCompany_Name, strBriefAbt, strCurSys, out strPassword, out intCompanyID, out  intUserID, out intUser_Level_ID, out strUserName, out  strCompanyName, out strCompanyCode, out LastLoginDate, out strTheme, out strAccess, out strCountryName, out strUserType, out strMarquee);
                if (intErrorCode == 0)
                {
                    String StrDescrypt = ClsPubCommCrypto.DecryptText(strPassword);
                    if (StrDescrypt != StrPlainPassword)
                    {
                        intErrorCode = 2; // Password Mismatch
                    }
                }
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Login :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intErrorCode;
        }
        #endregion

        #region Menu Loading Code
        /// <summary>
        /// Function To get user Menu as Dataset
        /// </summary>
        /// <param name="intUserId">User Id for which menu to be loaded</param>
        /// <param name="strUserName"> User Name of the user</param>
        /// <returns>DataSet</returns>
        public byte[] FunPubGetUserMenu(int intUserId, string strUserName, string strUserType)
        {
            DataSet ds;
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                ds = ObjCLsPubS3GAdmin.FunPubGetUserMenu(intUserId, strUserName, strUserType);
                if (ds != null)
                    bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Menu Loading :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        #endregion

        #region Common Methods

        /// <summary>
        /// Common Method that will exeute the Required Procedure when send with 
        /// respective parameters and will return a datatable
        /// </summary>
        /// <param name="strProcName">Procedure name which will be executed</param>
        /// <param name="dctProcParams">dictionary Containing Parameters for the sp</param>
        /// <returns>Return a DataTable</returns>
        public byte[] FunPubFillDropdown(string strProcName, Dictionary<string, string> dctProcParams)
        {
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                DataTable dtDropDown = ObjCLsPubS3GAdmin.FunPubFillDropdown(strProcName, dctProcParams);
                if (dtDropDown != null)
                    bytesDataTable = ClsPubSerialize.Serialize(dtDropDown, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        //public byte[] FunPubFillOutputDataTable(string strProcName, Dictionary<string, string> dctProcParams, Dictionary<string, string> dctProcParamsOutput, out byte[] byteOuput)
        //{
        //    try
        //    {
        //        ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
        //        ArrayList arrOutput = new ArrayList();
        //        byteOuput = null;
        //        DataTable dtDropDown = ObjCLsPubS3GAdmin.FunPubFillOutputDataTable(strProcName, dctProcParams, dctProcParamsOutput, out arrOutput);
        //        if (dtDropDown != null)
        //            bytesDataTable = ClsPubSerialize.Serialize(dtDropDown, SerializationMode.Binary);
        //        if (arrOutput != null)
        //            byteOuput = ClsPubSerialize.Serialize(arrOutput, SerializationMode.Binary);

        //    }
        //    catch (Exception objExp)
        //    {
        //        ClsPubFaultException objFault = new ClsPubFaultException();
        //        objFault.ProReasonRW = "Error in Filling Dropdown :" + objExp.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        //    }
        //    return bytesDataTable;
        //}

        public byte[] FunPubExecuteSQL(Dictionary<string, string> ProcParams)
        {
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                DataTable dtDropDown = ObjCLsPubS3GAdmin.PubExecuteSQL(ProcParams);
                if (dtDropDown != null)
                    bytesDataTable = ClsPubSerialize.Serialize(dtDropDown, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }
        /// <summary>
        /// Common Method that will exeute the Required Procedure when send with 
        /// respective parameters and will return a DataSet
        /// </summary>
        /// <param name="strProcName">Procedure name which will be executed</param>
        /// <param name="dctProcParams">dictionary Containing Parameters for the sp</param>
        /// <returns>Return a DataSet</returns>
        public byte[] FunPubFillDataset(string strProcName, Dictionary<string, string> dctProcParams)
        {
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                DataSet ObjDataset = ObjCLsPubS3GAdmin.FunPubFillDataset(strProcName, dctProcParams);
                bytesDataSet = ClsPubSerialize.Serialize(ObjDataset, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataSet;
        }

        public string FunGetScalarValue(string strProcName, Dictionary<string, string> dctProcParams)
        {
            string ScalarString = string.Empty;
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                ScalarString = ObjCLsPubS3GAdmin.FunGetScalarValue(strProcName, dctProcParams);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return ScalarString;
        }

        public string FunValidateMonthClosure(string strProcName, Dictionary<string, string> dctProcParams)
        {
            string strLock = string.Empty;
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                strLock = ObjCLsPubS3GAdmin.FunValidateMonthClosure(strProcName, dctProcParams);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return strLock;
        }

        /// <summary>
        /// Function Mainly for Gridview Binding with paging it should have predefined Paging object
        /// passed to it.
        /// </summary>
        /// <param name="strProcName">Procedure name used for grid binding </param>
        /// <param name="dctProcParams">Parameter for the Procedure</param>
        /// <param name="intTotalRecords">Total records that the query return</param>
        /// <param name="ObjPaging"> Paging object that has page related details</param>
        /// <returns>DataTable</returns>
        public byte[] FunPubFillGridPaging(string strProcName, Dictionary<string, string> dctProcParams, out int intTotalRecords, PagingValues ObjPaging)
        {
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                DataTable dtGridPaging = ObjCLsPubS3GAdmin.FunPubFillGridPaging(strProcName, dctProcParams, out intTotalRecords, ObjPaging);
                bytesDataTable = ClsPubSerialize.Serialize(dtGridPaging, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Filling Grid :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        public byte[] FunPubFillGridPaging(string strProcName, Dictionary<string, string> dctProcParams, out int intTotalRecords, out int intErrorCode, PagingValues ObjPaging)
        {
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                DataTable dtGridPaging = ObjCLsPubS3GAdmin.FunPubFillGridPaging(strProcName, dctProcParams, out intTotalRecords, out intErrorCode, ObjPaging);
                bytesDataTable = ClsPubSerialize.Serialize(dtGridPaging, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Filling Grid :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }


        #endregion

        #region WorkflowToDoList
        /// <summary>
        /// Function To get WorkflowToDoList as Dataset
        /// </summary>

        public byte[] FunPubGetWorkflowDocuments(int intCompanyId, int intUserId)
        {
            DataSet ds;
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                ds = ObjCLsPubS3GAdmin.FunPubGetWorkflowDocuments(intCompanyId, intUserId);
                if (ds != null)
                    bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in WorkflowToDoList:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }
        public byte[] FunPubGetWorkflowToDoList(int intCompanyId, int intUserId, bool ShowAll, DateTime? FromDate, DateTime? ToDate)
        {
            DataSet ds;
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                ds = ObjCLsPubS3GAdmin.FunPubGetWorkflowToDoList(intCompanyId, intUserId, ShowAll, FromDate, ToDate);
                if (ds != null)
                    bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in WorkflowToDoList:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        public void FunPubSysJournalEntry(ClsSystemJournal ObjSysJournal)
        {
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                ObjCLsPubS3GAdmin.FunPubSysJournalEntry(ObjSysJournal);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Journal Entry : " + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }

        #endregion

        #region [Location master Details]

        /// <summary>
        /// Created By Tamilselvan.S
        /// Craeted Date 04/05/2011
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesLocation_Datatable"></param>
        /// <param name="strErrorCode"></param>
        public int FunPubInsertLocationCategory(SerializationMode SerMode, byte[] bytesLocation_Datatable, out string strExistingCode, out string strExistingDescription)
        {
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                return ObjCLsPubS3GAdmin.FunPubInsertLocationCategory(SerMode, bytesLocation_Datatable, out strExistingCode, out strExistingDescription);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Location Category Entry : " + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }

        /// <summary>
        /// Created By Tamilselvan.S
        /// Craeted Date 09/05/2011
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesLocation_Datatable"></param>
        /// <param name="strErrorCode"></param>
        public int FunPubUpdateLocationCategory(SerializationMode SerMode, byte[] bytesLocation_Datatable)
        {
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                return ObjCLsPubS3GAdmin.FunPubUpdateLocationCategory(SerMode, bytesLocation_Datatable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Location Category Updation : " + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        /// <summary>
        /// Created By Tamilselvan.S
        /// Craeted Date 06/05/2011 
        /// </summary>
        /// <param name="intCompany_Id"></param>
        /// <param name="intUser_ID"></param>
        /// <param name="intLocationCategory_Id"></param>
        /// <param name="SerMode"></param>
        /// <returns></returns>
        public byte[] FunPubQueryLocationCategoryDetails(int intCompany_Id, int intUser_ID, int intLocationCategory_Id, SerializationMode SerMode)
        {
            S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationCategoryDataTable ObjLocCatTable;
            try
            {
                S3GDALayer.S3GAdminServices.ClsPubS3GAdmin ObjLocCat = new ClsPubS3GAdmin();
                ObjLocCatTable = ObjLocCat.FunPubQueryLocationCategoryDetails(intCompany_Id, intUser_ID, intLocationCategory_Id);
                bytesDataTable = S3GBusEntity.ClsPubSerialize.Serialize(ObjLocCatTable, SerMode);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Retriving Location Category :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;

        }

        /// <summary>
        /// Created by Tamilselvan.S
        /// Created Date 06/05/2011
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesLocation_Datatable"></param>
        /// <param name="strExistingMapping"></param>
        /// <returns></returns>
        public int FunPubInsertLocationMaster(SerializationMode SerMode, byte[] bytesLocation_Datatable, out string strExistingMapping)
        {
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                return ObjCLsPubS3GAdmin.FunPubInsertLocationMaster(SerMode, bytesLocation_Datatable, out strExistingMapping);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Location Category Entry : " + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        /// <summary>
        /// Created By Tamilselvan.S
        /// Craeted Date 09/05/2011
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesLocation_Datatable"></param>
        /// <param name="strErrorCode"></param>
        public int FunPubUpdateLocationMapping(SerializationMode SerMode, byte[] bytesLocation_Datatable)
        {
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                return ObjCLsPubS3GAdmin.FunPubUpdateLocationMapping(SerMode, bytesLocation_Datatable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Location Mapping Updation : " + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion [Location master Details]

        #region "[Audit Trial]"
        public int FunPubCreateAuditTrial(SerializationMode SerMode, byte[] byteObjAuditTrialDataTable_SERLAY)
        {
            try
            {
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                return ObjCLsPubS3GAdmin.FunPubCreateAuditTrial(SerMode, byteObjAuditTrialDataTable_SERLAY);
            }
            catch (Exception objEx)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Audit Trial :" + objEx.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region Password Validation

        public int FunPubPasswordValidation(Int32 intUserID, string strPassword)
        {
            try
            {
                String strPlainPassword = strPassword;
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                intErrorCode = ObjCLsPubS3GAdmin.FunPubPasswordValidation(intUserID, out strPassword);
                if (intErrorCode == 0)
                {
                    String StrDescrypt = ClsPubCommCrypto.DecryptText(strPassword);
                    if (StrDescrypt != strPlainPassword)
                    {
                        intErrorCode = 2; // Password Mismatch
                    }
                }
            }
            catch (Exception objEx)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Password Validation :" + objEx.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intErrorCode;
        }

        #endregion

        #region EMI Calculation

        public int FunPubSaveEMICalc(SerializationMode SerMode, byte[] bytesDictionary)
        {
            int intResult = 0;
            try
            {
                S3GDALayer.S3GAdminServices.ClsPubS3GAdmin ObjS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                intResult = ObjS3GAdmin.FunPubSaveEMICalc(SerMode, bytesDictionary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException ObjFault = new ClsPubFaultException();
                ObjFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(ObjFault, new FaultReason(ObjFault.ProReasonRW));
            }
            return intResult;
        }

        #endregion




        #endregion

        #region ErrorLog
        public string FunPubSysErrorLog(string strProcName, Dictionary<string, string> dctProcParams)
        {
            string strErrLog = string.Empty;
            try
            {
                //ClsSystemErrorLog ObjCLsPubS3GAdmin1 = new S3GDALayer.S3GAdminServices.ClsPubCmnErrorLog();
                ClsPubS3GAdmin ObjCLsPubS3GAdmin = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                ObjCLsPubS3GAdmin.FunPubSysErrorLog(strProcName, dctProcParams);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return strErrLog;
        }
        #endregion
        /// <summary>
        /// By Chandu For User Login Details 7-Aug-2013
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjUserLoginDetailsDataTable_SERLAY"></param>
        /// <returns></returns>
        public int FunPubInsertUserLoginDetails(SerializationMode SerMode, byte[] bytesObjUserLoginDetailsDataTable_SERLAY)
        {
            int intResult = 0;
            try
            {
                S3GDALayer.S3GAdminServices.ClsPubS3GAdmin ObjUserlOGINDetails = new ClsPubS3GAdmin();
                intResult = ObjUserlOGINDetails.FunPubInsUserLoginDet(SerMode, bytesObjUserLoginDetailsDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Target Master:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }
        public int FunPubUpdateLogOutFlags(int intCompanyID, int intUserID, String Session_ID, String UserIDs, int LogOut_Status_Code)
        {
            int intErrorCode = 0;
            try
            {
                S3GDALayer.S3GAdminServices.ClsPubS3GAdmin ObjUserlOGINDetails = new ClsPubS3GAdmin();
                intErrorCode = ObjUserlOGINDetails.FunPubUpdateLogOutFlags(intCompanyID, intUserID, Session_ID, UserIDs, LogOut_Status_Code);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in LogOut :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intErrorCode;
        }
        public int FunPubUpdateUserIsActive(DateTime ActiveDateTime, int User_ID, int Company_ID, int Manual_Logged_Out)
        {
            int intErrorCode = 0;
            try
            {
                S3GDALayer.S3GAdminServices.ClsPubS3GAdmin ObjUserlOGINDetails = new ClsPubS3GAdmin();
                intErrorCode = ObjUserlOGINDetails.FunPubUpdateUserIsActive(ActiveDateTime, User_ID, Company_ID, Manual_Logged_Out);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in MasterPage :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intErrorCode;
        }
        /// <summary>
        /// By Chandu For User Login Details 7-Aug-2013
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjUserLoginDetailsDataTable_SERLAY"></param>
        /// <returns></returns>
    }

}
