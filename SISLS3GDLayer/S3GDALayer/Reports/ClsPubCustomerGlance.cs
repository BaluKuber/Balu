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
    public class ClsPubCustomerGlance : ClsPubDalReportBase
    {

        #region GetHeaderDetails

        public List<ClsPubDropDownList> FunPubGetRegionDetails(int CompanyId, int UserId)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetLOBDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.REGIONID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.REGIONNAME]);
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

        public List<ClsPubDropDownList> FunPubGetProductDetails(int CompanyId,int LobId)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetCustomerAtaGlanceProductDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, LobId);

                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.PRODUCTID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.PRODUCTNAME]);
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

        public List<ClsPubDropDownList> FunPubGetBranchDetails(int CompanyId, int UserId, int RegionId, Boolean Is_Active)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetCustomerAtaGlanceBranchDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMREGIONID, DbType.Int32, RegionId);              
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, Is_Active);

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
        public List<ClsPubDropDownList> FunPubGetLOB(int CompanyId, int UserId, bool Is_Active, string CustomerId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.CustomerBasedLOBForCG);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, Is_Active);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.String, CustomerId);


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
        #region GetCustomerAccountDetailsGrid
        public List<ClsPubCustomerGlanceDetails> FunPubGetCustomerAtAGlanceDetails(ClsPubCustomerGlanceHeaderDetails headerDetails,List<ClsPubPASA> PASAs)
        {
            string XMLAccountDetails = FunProGeneratePrimeSubAccount(PASAs);
            List<ClsPubCustomerGlanceDetails> customerGlanceDetails = new List<ClsPubCustomerGlanceDetails>();

            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetCustomerAtaGlanceDetailsNew);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, headerDetails.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, headerDetails.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.Int32, headerDetails.CustomerId);
                if (headerDetails.LOBId != "-1" && headerDetails.LOBId != "0" && headerDetails.LOBId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, headerDetails.LOBId);
                }
                if (headerDetails.RegionId != "-1" && headerDetails.RegionId != "0" && headerDetails.RegionId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, headerDetails.RegionId);
                }
                if (headerDetails.BranchId != "-1" && headerDetails.BranchId != "0" && headerDetails.BranchId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, headerDetails.BranchId);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, headerDetails.ProgramId);
                if (headerDetails.ProductId != "-1" && headerDetails.ProductId != "0" && headerDetails.ProductId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODUCTID, DbType.Int32, headerDetails.ProductId);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, headerDetails.StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, headerDetails.EndDate);
                db.AddInParameter(command, ClsPubDALConstants.XMLACCOUNTDETAILS, DbType.String, XMLAccountDetails);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubCustomerGlanceDetails customerGlanceDetail = new ClsPubCustomerGlanceDetails();
                    customerGlanceDetail.LOB = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    //customerGlanceDetail.Region = StringParse(reader[ClsPubDALConstants.REGION]);
                    //customerGlanceDetail.Branch = StringParse(reader[ClsPubDALConstants.BRANCHNAME]);
                    customerGlanceDetail.Location = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    customerGlanceDetail.Product = StringParse(reader[ClsPubDALConstants.PRODUCTNAME]);
                    customerGlanceDetail.Status = StringParse(reader[ClsPubDALConstants.STATUS]);
                    customerGlanceDetail.Primeac = StringParse(reader[ClsPubDALConstants.PANUM]);
                    customerGlanceDetail.Subac = StringParse(reader[ClsPubDALConstants.SANUM]);
                    customerGlanceDetail.DisbursedAmount = DecimalParse(reader[ClsPubDALConstants.DISBURSEDAMOUNT]);
                    customerGlanceDetail.AppliedAmt = DecimalParse(reader[ClsPubDALConstants.APPLIEDAMOUNT]);
                    customerGlanceDetail.CollateralValue = DecimalParse(reader[ClsPubDALConstants.COLLATERALVALUE]);
                    customerGlanceDetail.SancAmt = DecimalParse(reader[ClsPubDALConstants.SANCTIONEDAMOUNT]);
                    customerGlanceDetail.GrossExposure = DecimalParse(reader[ClsPubDALConstants.GROSSEXPOSURE]);
                    customerGlanceDetail.NetExposure = DecimalParse(reader[ClsPubDALConstants.NETEXPOSURE]);
                    customerGlanceDetail.ODIDue = DecimalParse(reader[ClsPubDALConstants.ODIDUE]);
                    customerGlanceDetail.MemoDue = DecimalParse(reader[ClsPubDALConstants.MEMODUE]);
                    customerGlanceDetail.Others = DecimalParse(reader[ClsPubDALConstants.OTHERDUE]);
                    customerGlanceDetail.Dues = DecimalParse(reader[ClsPubDALConstants.DUES]);
                    customerGlanceDetail.Collected = DecimalParse(reader[ClsPubDALConstants.COLLECTED]);
                    customerGlanceDetail.AverageDueDates = DecimalParse(reader[ClsPubDALConstants.AVERAGEDUEDATES]);
                    customerGlanceDetail.GPSSUFFIX = DecimalParse(reader[ClsPubDALConstants.Gpssuffix]);
                    //customerGlanceDetail.Pending=DecimalParse(reader[ClsPubDALConstants.PENDING]);
                    customerGlanceDetails.Add(customerGlanceDetail);
                }
                reader.Close();                
            }
            catch (Exception e)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(e);
                throw e;
            }
            return customerGlanceDetails;
        }

        public List<ClsPubAsset> FunPubGetAssetDetails(int CompanyId, string StartDate, string EndDate, List<ClsPubPASA> PASAs)
        {
            string XMLAccountDetails = FunProGeneratePrimeSubAccount(PASAs);

            List<ClsPubAsset> ASSETS = new List<ClsPubAsset>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETCUSTOMERGLANCEASSETDETAILS);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, EndDate);
                db.AddInParameter(command, ClsPubDALConstants.XMLACCOUNTDETAILS, DbType.String, XMLAccountDetails);

                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubAsset ASSET = new ClsPubAsset();
                    ASSET.LobName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    ASSET.PrimeAccountNo = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNTNUMBER]);
                    ASSET.SubAccountNo = StringParse(reader[ClsPubDALConstants.SUBACCOUNTNUMBER]);
                    ASSET.AssetDesc = StringParse(reader[ClsPubDALConstants.ASSETDETAILS]);
                    ASSET.RegNo = StringParse(reader[ClsPubDALConstants.SLREGNO]);
                    ASSET.AmountFinanced = DecimalParse(reader[ClsPubDALConstants.AMOUNTFINANCED]);
                    ASSET.Terms = StringParse(reader[ClsPubDALConstants.TERMS]);
                    ASSET.YetToBeBilled = DecimalParse(reader[ClsPubDALConstants.YETTOBEBILLED]);
                    ASSET.Billed = DecimalParse(reader[ClsPubDALConstants.BILLED]);
                    ASSET.Balance = DecimalParse(reader[ClsPubDALConstants.BALANCE]);
                    //ASSET.YetToBeBilled = 0;
                    //ASSET.Billed = 0;
                    //ASSET.Balance = 0;

                    ASSETS.Add(ASSET);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return ASSETS;
        }
        #endregion
        
    }
}
