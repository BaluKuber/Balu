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
    public class ClsPubRptRepaymentSchedule : ClsPubDalReportBase
    {
        #region Load LOB
        /// <summary>
        /// To get the LOB
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubGetLOB(int CompanyId, int UserId, int ProgramId, string CustomerId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetCustomerWiseLob);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.String, CustomerId);


                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 4/April/2012
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

        #region Load Branch
        /// <summary>
        /// To get the Branch
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="UserId"></param>
        /// <param name="Is_Active"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubGetBranch(int CompanyId, int UserId, int ProgramId, string CustomerId, int LobId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetCustomerWiseBranch);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.String, CustomerId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, LobId);

                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 4/April/2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.LOCATIONID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.LOCATION]);
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
        /// <summary>
        /// To get the Product
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubGetProduct(int CompanyId, string CustomerId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetProductBasedCustomer);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.String, CustomerId);

                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 4/April/2012
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

        #region Get MLA
        /// <summary>
        /// To get the Prime Account Number
        /// </summary>
        /// <param name="ObjPrimeAccount"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubGetMLA(ClsPubPrimeAccountDetails ObjPrimeAccount)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetPASA);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTYPE, DbType.String, ObjPrimeAccount.Type);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, ObjPrimeAccount.CompanyId);
                if (ObjPrimeAccount.LobId != 0)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, ObjPrimeAccount.LobId);
                }
                if (ObjPrimeAccount.locationId != 0)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID, DbType.Int32, ObjPrimeAccount.locationId);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVATED, DbType.Int32, ObjPrimeAccount.IsActivated);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.String, ObjPrimeAccount.CustomerId);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCHECKACCESS, DbType.String, ObjPrimeAccount.CheckAccess);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ObjPrimeAccount.ProgramId);

                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 4/April/2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();

                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.PANUM]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.PANUM]);
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

        #region Get SLA
        /// <summary>
        /// To get th Sub Account Number
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="CompanyId"></param>
        /// <param name="IsActive"></param>
        /// <param name="CustomerId"></param>
        /// <param name="PNum"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubGetSLA(string Type, int CompanyId, string PNum, int IsActive, string CustomerId, int ProgramId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetPASA);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTYPE, DbType.String, Type);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, PNum);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVATED, DbType.Int32, IsActive);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);


                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 4/April/2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();

                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.SANUM]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.SANUM]);
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

        #region Load Asset Details Grid
        /// <summary>
        /// To get the Asset Details
        /// </summary>
        /// <param name="PANum"></param>
        /// <param name="SANum"></param>
        /// <returns></returns>
        public List<ClsPubAssestDetails> FunPubGetAssestDetails(int CompanyId, string PANum, string SANum)
        {
            List<ClsPubAssestDetails> assestDetails = new List<ClsPubAssestDetails>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetAssetDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, PANum);
                if (SANum.Trim() != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSANUM, DbType.String, SANum);
                }

                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 4/April/2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubAssestDetails assestDetail = new ClsPubAssestDetails();
                    assestDetail.AssetDetails = StringParse(reader[ClsPubDALConstants.ASSETDETAILS]);
                    assestDetail.SLRegNo = StringParse(reader[ClsPubDALConstants.SLREGNO]);
                    assestDetail.AmountFinanced = DecimalParse(reader[ClsPubDALConstants.AMOUNTFINANCED]);
                    assestDetail.Irr = DecimalParse(reader[ClsPubDALConstants.IRR]);
                    assestDetail.Terms = StringParse(reader[ClsPubDALConstants.TERMS]);
                    assestDetail.PolicyNo = StringParse(reader[ClsPubDALConstants.POLICYNO]);
                    assestDetail.ValidUpto = reader[ClsPubDALConstants.VALIDUPTO].ToString();
                    assestDetail.Insurer = StringParse(reader[ClsPubDALConstants.INSURER]);
                    assestDetail.PolicyAmount = DecimalParse(reader[ClsPubDALConstants.POLICYAMOUNT]);
                    assestDetail.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    assestDetails.Add(assestDetail);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return assestDetails;
        }
        #endregion

        #region Load Repay Details Grid
        /// <summary>
        /// To get the Repayment Details
        /// </summary>
        /// <param name="PANum"></param>
        /// <param name="SANum"></param>
        /// <returns></returns>

        public List<ClsPubRepayDetails> FunPubGetRepayDetails(int CompanyId, string PANum, string SANum, string Type)
        {
            List<ClsPubRepayDetails> RepayDetails = new List<ClsPubRepayDetails>();
            RepayDetails.Clear();
            
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetRepaymentStructure);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, PANum);
                if (SANum.Trim() != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSANUM, DbType.String, SANum);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTYPE, DbType.String, Type);
                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 4/April/2012
                IDataReader reader = db.FunPubExecuteReader(command);
                //RepayDetails.Clear();
                   while (reader.Read())
                {
                    
                    ClsPubRepayDetails RepayDetail = new ClsPubRepayDetails();
                    RepayDetail.InstallmentNo = IntParse(reader[ClsPubDALConstants.INSTALLMENTNO]);
                    RepayDetail.InstallmentDate = reader[ClsPubDALConstants.INSTALLMENTDATE].ToString();
                    RepayDetail.InstallmentAmount = DecimalParse(reader[ClsPubDALConstants.INSTALLMENTAMOUNT]);
                    RepayDetail.PrincipalAmount = DecimalParse(reader[ClsPubDALConstants.PRINCIPALAMOUNT]);
                    RepayDetail.FinanceCharges = DecimalParse(reader[ClsPubDALConstants.FINANCECHARGES]);
                    RepayDetail.Umfc = DecimalParse(reader[ClsPubDALConstants.UMFC]);
                    RepayDetail.InsuranceAmount = DecimalParse(reader[ClsPubDALConstants.INSURANCEAMOUNT]);
                    RepayDetail.Others = DecimalParse(reader[ClsPubDALConstants.OTHERS]);
                    RepayDetail.VatRecovery = DecimalParse(reader[ClsPubDALConstants.VATRECOVERY]);
                    RepayDetail.TaxSetOff = DecimalParse(reader[ClsPubDALConstants.TAXSETOFF]);
                    RepayDetail.Tax = DecimalParse(reader[ClsPubDALConstants.TAX]);
                    RepayDetail.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);

                    RepayDetails.Add(RepayDetail);
                }
                reader.Dispose();
                reader.Close();
                
              }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return RepayDetails;
            //return RepayDetail;
        }

        public ClsPubHeaderLobBranchProductDetails FunPunGetHeaderLobBranchProductDetails(string PANum)
        {
            ClsPubHeaderLobBranchProductDetails headerLobBranchProductDetails = new ClsPubHeaderLobBranchProductDetails();
            try
            {

                DbCommand command = db.GetStoredProcCommand(SPNames.GetHeaderLobBranchProductDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, PANum);

                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 4/April/2012
                IDataReader reader = db.FunPubExecuteReader(command);

                while (reader.Read())
                {
                    headerLobBranchProductDetails.LobId = IntParse(reader[ClsPubDALConstants.LOBID]);
                    headerLobBranchProductDetails.LocationId = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    headerLobBranchProductDetails.ProductId = IntParse(reader[ClsPubDALConstants.PRODUCTID]);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return headerLobBranchProductDetails;
        }

        #endregion

    }
}
