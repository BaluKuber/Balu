using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity.Reports;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Data;
using S3GDALayer.Constants;
using S3GBusEntity;

namespace S3GDALayer.Reports
{
    public class ClsPubRptCreditScoreTransaction : ClsPubDalReportBase
    {
        #region LOB

        public List<ClsPubDropDownList> FunPubGetLOB(int CompanyId, int UserId, bool Is_Active)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {

                DbCommand command = db.GetStoredProcCommand(SPNames.GetLOBDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, Is_Active);
                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
                IDataReader reader = db.FunPubExecuteReader(command);

                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.LOBID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    dropDownLists.Add(dropDownList);
                }
                reader.Close();

            }

            catch (Exception ex)
            {

                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

            }
            return FunPriLoadSelect(dropDownLists);
        }
        #endregion

        #region Branch
        public List<ClsPubDropDownList> FunPubBranch(int CompanyId, int UserId, bool Is_Active)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {

                DbCommand command = db.GetStoredProcCommand(SPNames.GetBranch);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, Is_Active);


                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
                IDataReader reader = db.FunPubExecuteReader(command);

                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();

                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.BRANCHID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.BRANCH]);
                    dropDownLists.Add(dropDownList);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return FunPriLoadSelect(dropDownLists);
        }
        #endregion

        #region Get Product
        public List<ClsPubDropDownList> FunPubGetProduct(int CompanyId, int LOBId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {

                DbCommand command = db.GetStoredProcCommand(SPNames.S3G_RPT_GetProduct);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, LOBId);

                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
                IDataReader reader = db.FunPubExecuteReader(command);

                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();

                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.PRODUCTID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.PRODUCTNAME]);
                    dropDownLists.Add(dropDownList);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return FunPriLoadSelect(dropDownLists);
        }
        #endregion

        #region Credit Score Details Grid
        public ClsPubCreditScoreTransaction FunPubGetCreditScoreDetails(ClsPubCredit ObjCredit)
        {
            ClsPubCreditScoreTransaction CreditScoreDetails = new ClsPubCreditScoreTransaction();
            CreditScoreDetails.CreditScoreTrans = new List<ClsPubCreditScoreDetails>();
            CreditScoreDetails.Creditlocation = new List<ClsPubLocationCodeCategory>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.CreditScoreDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, ObjCredit.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, ObjCredit.UserId);
                if (ObjCredit.LocationId1 != -1)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, ObjCredit.LocationId1);
                }
                if (ObjCredit.LocationId2 != -1)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, ObjCredit.LocationId2);
                }
                if (ObjCredit.ProductId != -1)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODUCTID, DbType.Int32, ObjCredit.ProductId);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, ObjCredit.LOBId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.String, ObjCredit.Start_Date);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.String, ObjCredit.End_Date);


                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
                IDataReader reader = db.FunPubExecuteReader(command);

                while (reader.Read())
                {
                    ClsPubCreditScoreDetails CreditScoreDetail = new ClsPubCreditScoreDetails();
                    CreditScoreDetail.BRANCH_ID = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    CreditScoreDetail.PRODUCT_ID = IntParse(reader[ClsPubDALConstants.PRODUCTID]);
                    CreditScoreDetail.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                    CreditScoreDetail.Product = StringParse(reader[ClsPubDALConstants.PRODUCT]);
                    CreditScoreDetail.ECAType = StringParse(reader[ClsPubDALConstants.EQCTYPE]);
                    CreditScoreDetail.NoofAccounts = IntParse(reader[ClsPubDALConstants.NOOFACCOUNTS]);
                    CreditScoreDetail.RequiredScore = DecimalParse(reader[ClsPubDALConstants.REQUIREDSCORE]);
                    CreditScoreDetail.ActualScore = DecimalParse(reader[ClsPubDALConstants.ACTUALSCORE]);
                    CreditScoreDetail.Hygiene = DecimalParse(reader[ClsPubDALConstants.HYGIENE]);
                    CreditScoreDetail.Accepted = DecimalParse(reader[ClsPubDALConstants.ACCEPTED]);
                    CreditScoreDetail.Rejected = DecimalParse(reader[ClsPubDALConstants.REJECTED]);
                    CreditScoreDetail.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    //CreditScoreDetails.Add(CreditScoreDetail);
                    CreditScoreDetails.CreditScoreTrans.Add(CreditScoreDetail);
                }
                 reader.NextResult();
                 while (reader.Read())
                 {
                     ClsPubLocationCodeCategory LocCodeCat = new ClsPubLocationCodeCategory();
                     LocCodeCat.LocationCode = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                     LocCodeCat.LocationName = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                     CreditScoreDetails.Creditlocation.Add(LocCodeCat);
                 }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return CreditScoreDetails;
        }
        #endregion

        #region Customer Details Grid Accepted
        public List<ClsPubCustomersDetails> FunPubGetCustomersDetails(ClsPubCredit ObjCustomers)
        {
            List<ClsPubCustomersDetails> CustomersDetails = new List<ClsPubCustomersDetails>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.CustomersDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, ObjCustomers.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, ObjCustomers.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, ObjCustomers.LOBId);
                if (ObjCustomers.LocationId1 != -1)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, ObjCustomers.LocationId1);
                }
                if (ObjCustomers.LocationId2 != -1)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, ObjCustomers.LocationId2);
                }
                if (ObjCustomers.ProductId != -1)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODUCTID, DbType.Int32, ObjCustomers.ProductId);
                }
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMBRANCHID, DbType.Int32, ObjCustomers.BranchId);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODUCTID, DbType.Int32, ObjCustomers.ProductId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.String, ObjCustomers.Start_Date);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.String, ObjCustomers.End_Date);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTATUS, DbType.Int32, ObjCustomers.Status);


                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
                IDataReader reader = db.FunPubExecuteReader(command);

                while (reader.Read())
                {
                    ClsPubCustomersDetails CustomersDetail = new ClsPubCustomersDetails();
                    CustomersDetail.BRANCH_ID = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    CustomersDetail.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                    CustomersDetail.Constitution = StringParse(reader[ClsPubDALConstants.CONSTITUTION]);
                    CustomersDetail.CustomerNameAddress = StringParse(reader[ClsPubDALConstants.CUSTOMERNAMEADDRESS]);
                    CustomersDetail.EnquiryDate = reader[ClsPubDALConstants.ENQUIRYDATE].ToString();
                    CustomersDetail.ApplicationDate = reader[ClsPubDALConstants.APPLICATIONDATE].ToString();
                    //CustomersDetail.Address = StringParse(reader[ClsPubDALConstants.ADDRESS]);
                    CustomersDetail.LoanBorrowed = DecimalParse(reader[ClsPubDALConstants.LOANBORROWED]);
                    //CustomersDetail.EnquiryDate = DateTimeParse(reader[ClsPubDALConstants.ENQUIRYDATE]);
                    //CustomersDetail.ApplicationDate = DateTimeParse(reader[ClsPubDALConstants.APPLICATIONDATE]);
                    CustomersDetail.PANum = StringParse(reader[ClsPubDALConstants.PANUM]);
                    CustomersDetail.SANum = StringParse(reader[ClsPubDALConstants.SANUM]);
                    CustomersDetail.Status = StringParse(reader[ClsPubDALConstants.STATUS]);

                    CustomersDetails.Add(CustomersDetail);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return (CustomersDetails);
        }
        #endregion

        #region Customer Details Grid Rejected
        public List<ClsPubCustomersDetails> FunPubGetRejCustomersDetails(ClsPubCredit ObjRejCustomers)
        {
            List<ClsPubCustomersDetails> CustomersRejDetails = new List<ClsPubCustomersDetails>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetCustRejected);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, ObjRejCustomers.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, ObjRejCustomers.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, ObjRejCustomers.LOBId);
                if (ObjRejCustomers.LocationId1 != -1)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, ObjRejCustomers.LocationId1);
                }
                if (ObjRejCustomers.LocationId2 != -1)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, ObjRejCustomers.LocationId2);
                }
                if (ObjRejCustomers.ProductId != -1)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODUCTID, DbType.Int32, ObjRejCustomers.ProductId);
                }
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMBRANCHID, DbType.Int32, ObjCustomers.BranchId);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODUCTID, DbType.Int32, ObjCustomers.ProductId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.String, ObjRejCustomers.Start_Date);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.String, ObjRejCustomers.End_Date);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTATUS, DbType.Int32, ObjRejCustomers.Status);


                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
                IDataReader reader = db.FunPubExecuteReader(command);

                while (reader.Read())
                {
                    ClsPubCustomersDetails CustomersRejDetail = new ClsPubCustomersDetails();
                    CustomersRejDetail.BRANCH_ID = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    CustomersRejDetail.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                    CustomersRejDetail.Constitution = StringParse(reader[ClsPubDALConstants.CONSTITUTION]);
                    CustomersRejDetail.CustomerNameAddress = StringParse(reader[ClsPubDALConstants.CUSTOMERNAMEADDRESS]);
                    CustomersRejDetail.EnquiryDate = reader[ClsPubDALConstants.ENQUIRYDATE].ToString();
                    CustomersRejDetail.LoanRequested = DecimalParse(reader[ClsPubDALConstants.LOANREQUESTED]);
                    CustomersRejDetails.Add(CustomersRejDetail);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return (CustomersRejDetails);
        }
        #endregion
    }
}