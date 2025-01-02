using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using S3GBusEntity.Reports;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.ObjectBuilder2;
using S3GDALayer.Constants;

namespace S3GDALayer.Reports
{
   public class ClsPubRptPDCReminder : ClsPubDalReportBase
    {
        #region GetPDCLOBDetails
       public List<ClsPubDropDownList> FunPubGetPDCLOBDetails(int CompanyId, int UserId,int ProgramId)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetLOBDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, Is_Active);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                //IDataReader reader = db.ExecuteReader(command); modified by sangeetha for Oracle Conversion - 27th Apr 2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.LOBID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    dropdownLists.Add(dropDownList);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                throw ex;
            }
            return FunPriLoadSelect(dropdownLists);
        }

       public List<ClsPubDropDownList> FunPubGetLocationDetails(int CompanyId, int UserId, int ProgramId, int LobId, int LocationId)
       {
           List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
           try
           {
               DbCommand command = db.GetStoredProcCommand(SPNames.getlocation2);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, LobId);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID, DbType.Int32, LocationId);
               //IDataReader reader = db.ExecuteReader(command); modified by sangeetha for Oracle Conversion - 27th Apr 2012
               IDataReader reader = db.FunPubExecuteReader(command);
               while (reader.Read())
               {
                   ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                   dropDownList.ID = StringParse(reader[ClsPubDALConstants.LOCATIONID]);
                   dropDownList.Description = StringParse(reader[ClsPubDALConstants.LOCATION]);
                   dropdownLists.Add(dropDownList);
               }
               reader.Close();
           }
           catch (Exception ae)
           {
                ClsPubCommErrorLogDal.CustomErrorRoutine(ae);
               throw ae;
           }
           return FunPriLoadSelect(dropdownLists);
       }

        public List<ClsPubPDCDocumentPathDetails> FunPubGetPDCDocPathDetails(int intCompanyId, int LobId,int intProgramId)
        {
            List<ClsPubPDCDocumentPathDetails> objlist = new List<ClsPubPDCDocumentPathDetails>();
            try
            {                
                DbCommand command = db.GetStoredProcCommand(SPNames.PDCDocumentPathDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, intCompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, LobId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, intProgramId);
                //IDataReader reader = db.ExecuteReader(command); modified by sangeetha for Oracle Conversion - 27th Apr 2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubPDCDocumentPathDetails objPDCDocumentPath = new ClsPubPDCDocumentPathDetails();
                    objPDCDocumentPath.DocumentPathID = StringParse(reader[ClsPubDALConstants.PDCDOCPATHID]);
                    objPDCDocumentPath.DocumentPath = StringParse(reader[ClsPubDALConstants.PDCDOCPATH]);
                    objlist.Add(objPDCDocumentPath);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(e);
                throw e;
            }
            return objlist;
        }
        #endregion

        #region GetPDCBranchDetails

        public List<ClsPubDropDownList> FunPubGetPDCBranchDetails(int CompanyId, int UserId, int ProgramId,string LobId,bool Is_Active)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetPDCReminderBranchDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, Is_Active);
                if(LobId!=string.Empty)
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.String, LobId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                //IDataReader reader = db.ExecuteReader(command); modified by sangeetha for Oracle Conversion - 27th Apr 2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.BRANCHID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.BRANCH]);
                    dropdownLists.Add(dropDownList);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                throw ex;
            }
            return FunPriLoadSelect(dropdownLists);
        }
        #endregion

        #region GetPDCReminderGridDetails
        public ClsPubPDCReminderDetails FunPubGetPDCReminderGridDetails(ClsPubRptPDCReminderHeaderDetails objpdcheaderdetails)
        {
            ClsPubPDCReminderDetails ReminderDetails = new ClsPubPDCReminderDetails();
            ReminderDetails.GridDetails = new List<ClsPubRptPDCReminderGridDetails>();
            ReminderDetails.AssetDetails = new List<ClsPubPDCReminderAssetDetails>();
            ReminderDetails.CustomerList = new List<ClsPubCustomerList>();
            //List<ClsPubRptPDCReminderGridDetails> griddetails = new List<ClsPubRptPDCReminderGridDetails>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetPDCReminderGridDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, objpdcheaderdetails.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, objpdcheaderdetails.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, objpdcheaderdetails.LOBId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, objpdcheaderdetails.LocationId1);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, objpdcheaderdetails.LocationId2);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, objpdcheaderdetails.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime,(objpdcheaderdetails.StartDate));
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime,(objpdcheaderdetails.EndDate));
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOB, DbType.String, (objpdcheaderdetails.LineOfBusiness));
                //IDataReader reader = db.ExecuteReader(command); modified by sangeetha for Oracle Conversion - 27th Apr 2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubRptPDCReminderGridDetails griddetail = new ClsPubRptPDCReminderGridDetails();
                    griddetail.CUSTOMER_NAME = StringParse(reader[ClsPubDALConstants.CUSTOMERNAME]);
                    griddetail.CUSTOMERNAME = StringParse(reader[ClsPubDALConstants.CUSTOMERNAM]);
                    griddetail.PRIMEACCOUNTNO = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNTNUMBER]);
                    griddetail.SUBACCOUNTNO = StringParse(reader[ClsPubDALConstants.SUBACCOUNTNUMBER]);
                    griddetail.LASTCOLLECTEDPDCDATE =StringParse(reader[ClsPubDALConstants.LASTCOLLECTEDPDCDATE]);
                    griddetail.Comm_Address1 = StringParse(reader[ClsPubDALConstants.COMM_ADDRESS1]);
                    griddetail.Comm_Address2 = StringParse(reader[ClsPubDALConstants.COMM_ADDRESS2]);
                    griddetail.Comm_City = StringParse(reader[ClsPubDALConstants.COMM_CITY]);
                    griddetail.Comm_State = StringParse(reader[ClsPubDALConstants.COMM_STATE]);                    
                    griddetail.Comm_country = StringParse(reader[ClsPubDALConstants.COMM_COUNTRY]);
                    griddetail.Comm_PinCode = StringParse(reader[ClsPubDALConstants.COMM_PINCODE]);
                    griddetail.Comm_Mobile = StringParse(reader[ClsPubDALConstants.COMM_MOBILE]);
                    griddetail.RepaymentDateFrom=StringParse(reader[ClsPubDALConstants.FUTUREPDCDATEFROM]);
                    griddetail.RepaymentDateTo = StringParse(reader[ClsPubDALConstants.FUTUREPDCDATETO]);                    
                    griddetail.Report_Date = StringParse(reader[ClsPubDALConstants.REPORTDATE]);
                    griddetail.CompanyName = StringParse(reader[ClsPubDALConstants.COMPANYNAME]);
                    griddetail.CustomerMail = StringParse(reader[ClsPubDALConstants.CUSTOMER_MAIL]);
                    ReminderDetails.GridDetails.Add(griddetail);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ClsPubPDCReminderAssetDetails AssetDetail = new ClsPubPDCReminderAssetDetails();
                    AssetDetail.PRIMEACCOUNTNO = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNTNUMBER]);
                    AssetDetail.SUBACCOUNTNO = StringParse(reader[ClsPubDALConstants.SUBACCOUNTNUMBER]);
                    AssetDetail.AssetMake = StringParse(reader[ClsPubDALConstants.ASSETMAKE]);
                    AssetDetail.AssetType = StringParse(reader[ClsPubDALConstants.ASSETTYPE]);
                    AssetDetail.RegistrationNumber = StringParse(reader[ClsPubDALConstants.REGISTRATIONNO]);
                    ReminderDetails.AssetDetails.Add(AssetDetail);
                }

                reader.NextResult();
                while (reader.Read())
                {
                    ClsPubCustomerList objlist = new ClsPubCustomerList();
                    objlist.CustomerName = StringParse(reader[ClsPubDALConstants.CUSTOMERNAME]);
                    objlist.CustomerMail = StringParse(reader[ClsPubDALConstants.CUSTOMER_MAIL]);
                    ReminderDetails.CustomerList.Add(objlist);
                }
                reader.Close();
            }
            catch (Exception exc)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(exc);
                throw exc;
            }
            return ReminderDetails;
        }
        #endregion
    }
}
