using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GDALayer.Constants;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity.Reports;
using System.Data.Common;
using System.Data.Sql;
using System.Xml.Linq;
using System.Data;
using S3GBusEntity;
using S3GBusEntity.Reports;


namespace S3GDALayer.Reports
{
    public class ClsPubRptBranchPerformance : ClsPubDalReportBase
    {
        public List<ClsPubCollection> FunPubGetCollectionDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            List<ClsPubCollection> collections = new List<ClsPubCollection>();
           try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetBranchCollectionDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                if (branchSelectionCriteria.LocationID1 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, branchSelectionCriteria.LocationID1);
                }
                if (branchSelectionCriteria.LocationID2 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, branchSelectionCriteria.LocationID2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFYEAR, DbType.Int32, branchSelectionCriteria.CutOffYear);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTH, DbType.String, branchSelectionCriteria.CutOffMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFINANCIALYEARFROM, DbType.Int32, branchSelectionCriteria.Financial_Year_From);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubCollection collection = new ClsPubCollection();
                    collection.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    collection.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                    collection.Year = StringParse(reader[ClsPubDALConstants.FINANCIALYEAR]);
                    collection.CurrentCollection = DecimalParse(reader[ClsPubDALConstants.CURRENTCOLLECTION]);
                    collection.ArrearCollection = DecimalParse(reader[ClsPubDALConstants.ARREARCOLLECTION]);
                    collection.TotalCollection = DecimalParse(reader[ClsPubDALConstants.TOTALCOLLECTION]);
                    collections.Add(collection);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return collections;
        }

        public List<ClsPubBranchStock> FunPubGetStockDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            List<ClsPubBranchStock> branchstocks = new List<ClsPubBranchStock>();
           
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetBranchStockDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                if (branchSelectionCriteria.LocationID1 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, branchSelectionCriteria.LocationID1);
                }
                if (branchSelectionCriteria.LocationID2 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, branchSelectionCriteria.LocationID2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTH, DbType.String, branchSelectionCriteria.CutOffMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, branchSelectionCriteria.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {

                    ClsPubBranchStock branchstock = new ClsPubBranchStock();
                    branchstock.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    branchstock.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                    branchstock.StockOnHire = DecimalParse(reader[ClsPubDALConstants.STOCKONHIRE]);
                    branchstock.Arrears = DecimalParse(reader[ClsPubDALConstants.ARREARASONCUTOFFMONTH]);
                    branchstock.ArrearsStock = DecimalParse(reader[ClsPubDALConstants.TOTAL]);
                    branchstock.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                   branchstocks.Add(branchstock);

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return branchstocks;
        }
        public ClsPubPayment FunPubGetPayment(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {

           ClsPubPayment detail = new ClsPubPayment();
            detail.assets = new List<ClsPubBranchAsset>();
            detail.PaymentDetails = new List<ClsPubPaymentDetails>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetBranchPaymentDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                if (branchSelectionCriteria.LocationID1 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, branchSelectionCriteria.LocationID1);
                }
                if (branchSelectionCriteria.LocationID2 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, branchSelectionCriteria.LocationID2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFINANCIALYEARSTARTDATE , DbType.DateTime, branchSelectionCriteria.FYSTARTDATE);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTHSTARTDATE , DbType.DateTime, branchSelectionCriteria.CMSTARTDATE);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTHENDDATE, DbType.DateTime, branchSelectionCriteria.CMENDDATE);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, branchSelectionCriteria.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
               {


                   ClsPubBranchAsset pay = new ClsPubBranchAsset();
                   pay.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                   pay.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                   pay.AllAssetsMonth = DecimalParse(reader[ClsPubDALConstants.MONTHAMT]);
                   pay.AllAssetsYTM = DecimalParse(reader[ClsPubDALConstants.YEARAMT]);
                   pay.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                   detail.assets.Add(pay);
                             
                                    
                }
                reader.NextResult();
                while(reader.Read())
                {
                    ClsPubPaymentDetails paymentdetail = new ClsPubPaymentDetails();
                    paymentdetail.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    paymentdetail.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                    paymentdetail.AssetClass = StringParse(reader[ClsPubDALConstants.BRANCHASSETCLASS]);
                    paymentdetail.AssetClassMonth = DecimalParse(reader[ClsPubDALConstants.UNITMONTH]);
                    paymentdetail.AssetClassYTM = DecimalParse(reader[ClsPubDALConstants.UNITYTM]);
                    detail.PaymentDetails.Add(paymentdetail);
            
                }
            }

             catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return detail;


        
        }
        public List<ClsPubNPA> FunPubGetNPADetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            List<ClsPubNPA> accounts = new List<ClsPubNPA>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetBranchNPADetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                if (branchSelectionCriteria.LocationID1 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, branchSelectionCriteria.LocationID1);
                }
                if (branchSelectionCriteria.LocationID2 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, branchSelectionCriteria.LocationID2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFPREVIOUSMONTH, DbType.Int32, branchSelectionCriteria.CUTOFFPREVIOUSMONTH);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTH, DbType.String, branchSelectionCriteria.CutOffMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, branchSelectionCriteria.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTHENDDATE, DbType.DateTime, branchSelectionCriteria.CMENDDATE);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubNPA account = new ClsPubNPA();
                    //account.RegionId = IntParse(reader[ClsPubDALConstants.REGIONID]);
                    //account.BranchId = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    account.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    account.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                    account.ClassId = IntParse(reader[ClsPubDALConstants.CLASSID]);
                    account.AssetClass = StringParse(reader[ClsPubDALConstants.ASSETACCOUNTCLASS]);
                    account.OpeningNoOfAccounts = DecimalParse(reader[ClsPubDALConstants.OPENINGACCOUNTS]);
                    account.OpeningStock = DecimalParse(reader[ClsPubDALConstants.OPENINGSTOCK]);
                    account.OpeningArrear = DecimalParse(reader[ClsPubDALConstants.OPENINGARREARS]);
                    account.AdditionNoOfAccounts = DecimalParse(reader[ClsPubDALConstants.ADDITIONACCOUNTS]);
                    account.AdditionStock = DecimalParse(reader[ClsPubDALConstants.ADDITIONSTOCK]);
                    account.AdditionArrear = DecimalParse(reader[ClsPubDALConstants.ADDITIONARREARS]);
                    account.DeletionNoOfAccounts = DecimalParse(reader[ClsPubDALConstants.DELETIONACCOUNTS]);
                    account.DeletionStock = DecimalParse(reader[ClsPubDALConstants.DELETIONSTOCK]);
                    account.DeletionArrear = DecimalParse(reader[ClsPubDALConstants.DELETIONARREARS]);
                    account.ClosingNoOfAccounts = DecimalParse(reader[ClsPubDALConstants.CLOSINGACCOUNTS]);
                    account.ClosingStock = DecimalParse(reader[ClsPubDALConstants.CLOSINGSTOCK]);
                    account.ClosingArrear = DecimalParse(reader[ClsPubDALConstants.CLOSINGARREARS]);
                    account.stock = DecimalParse(reader[ClsPubDALConstants.STOCK]);
                    account.Arrears = DecimalParse(reader[ClsPubDALConstants.ARREARS]);
                    account.Col1 = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    //ClsPubNPA account = new ClsPubNPA();
                    //account.AssetClass = "0";
                    //account.OpeningNoOfAccounts = "0";
                    //account.OpeningStock = "0";
                    //account.OpeningArrear = "0";
                    //account.AdditionNoOfAccounts = "0";
                    //account.AdditionStock = "0";
                    //account.AdditionArrear = "0";
                    //account.DeletionNoOfAccounts = "0";
                    //account.DeletionStock = "0";
                    //account.DeletionArrear = "0";
                    //account.ClosingNoOfAccounts = "0";
                    //account.ClosingStock = "0";
                    //account.ClosingArrear = "0";
                    accounts.Add(account);

                }
                reader.Close();

            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return accounts;
        }

        public List<ClsNPAaccount> FunPubGetNPAOpeningaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            List<ClsNPAaccount> accounts = new List<ClsNPAaccount>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetBranchOpeningAccount);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONCODE, DbType.String, branchSelectionCriteria.LocationCode);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCLASSID, DbType.Int32, branchSelectionCriteria.ClassId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFPREVIOUSMONTH, DbType.String, branchSelectionCriteria.CUTOFFPREVIOUSMONTH);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTH, DbType.String, branchSelectionCriteria.CutOffMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, branchSelectionCriteria.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsNPAaccount account = new ClsNPAaccount();
                    //account.RegionId = IntParse(reader[ClsPubDALConstants.REGIONID]);
                    //account.BranchId = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    account.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    account.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                    account.ClassId = IntParse(reader[ClsPubDALConstants.CLASSID]);
                    account.AssetClass = StringParse(reader[ClsPubDALConstants.ASSETACCOUNTCLASS]);
                    account.PrimeAccountNumber = StringParse(reader[ClsPubDALConstants.NPAPRIMEACCOUNTNUMBER]);
                    account.SubAccountNumber = StringParse(reader[ClsPubDALConstants.NPASUBACCOUNTNUMBER]);
                    account.Stock = DecimalParse(reader[ClsPubDALConstants.OPENINGSTOCK]);
                    account.Arrear = DecimalParse(reader[ClsPubDALConstants.OPENINGARREARS]);
                    account.Col1 = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    accounts.Add(account);

                    //ClsNPAaccount account = new ClsNPAaccount();
                    //account.PrimeAccountNumber = "0";
                    //account.SubAccountNumber = "0";
                    //account.Stock = "0";
                    //account.Arrear = "0";
                    //accounts.Add(account);

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return accounts;
        }
        public List<ClsNPAaccount> FunPubGetNPAAddtionaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            List<ClsNPAaccount> accounts = new List<ClsNPAaccount>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetBranchAdditionAccount);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONCODE, DbType.String, branchSelectionCriteria.LocationCode);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCLASSID, DbType.Int32, branchSelectionCriteria.ClassId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFPREVIOUSMONTH, DbType.String, branchSelectionCriteria.CUTOFFPREVIOUSMONTH);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTH, DbType.String, branchSelectionCriteria.CutOffMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, branchSelectionCriteria.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsNPAaccount account = new ClsNPAaccount();
                    //account.RegionId = IntParse(reader[ClsPubDALConstants.REGIONID]);
                    //account.BranchId = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    account.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    account.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                    account.ClassId = IntParse(reader[ClsPubDALConstants.CLASSID]);
                    account.AssetClass = StringParse(reader[ClsPubDALConstants.ASSETACCOUNTCLASS]);
                    account.PrimeAccountNumber = StringParse(reader[ClsPubDALConstants.NPAPRIMEACCOUNTNUMBER]);
                    account.SubAccountNumber = StringParse(reader[ClsPubDALConstants.NPASUBACCOUNTNUMBER]);
                    account.Stock = DecimalParse(reader[ClsPubDALConstants.ADDITIONSTOCK]);
                    account.Arrear = DecimalParse(reader[ClsPubDALConstants.ADDITIONARREARS]);
                    account.Col1 = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    accounts.Add(account);

                    //ClsNPAaccount account = new ClsNPAaccount();
                    //account.PrimeAccountNumber = "0";
                    //account.SubAccountNumber = "0";
                    //account.Stock = "0";
                    //account.Arrear = "0";
                    //accounts.Add(account);

                }

                reader.Close();
            }

            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return accounts;
        }
        public List<ClsNPAaccount> FunPubGetNPADeletionaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            List<ClsNPAaccount> accounts = new List<ClsNPAaccount>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetBranchDeletionAccount);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONCODE, DbType.String, branchSelectionCriteria.LocationCode);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCLASSID, DbType.Int32, branchSelectionCriteria.ClassId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFPREVIOUSMONTH, DbType.String, branchSelectionCriteria.CUTOFFPREVIOUSMONTH);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTH, DbType.String, branchSelectionCriteria.CutOffMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, branchSelectionCriteria.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsNPAaccount account = new ClsNPAaccount();
                    //account.RegionId = IntParse(reader[ClsPubDALConstants.REGIONID]);
                    //account.BranchId = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    account.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    account.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                    account.ClassId = IntParse(reader[ClsPubDALConstants.CLASSID]);
                    account.AssetClass = StringParse(reader[ClsPubDALConstants.ASSETACCOUNTCLASS]);
                    account.PrimeAccountNumber = StringParse(reader[ClsPubDALConstants.NPAPRIMEACCOUNTNUMBER]);
                    account.SubAccountNumber = StringParse(reader[ClsPubDALConstants.NPASUBACCOUNTNUMBER]);
                    account.Stock = DecimalParse(reader[ClsPubDALConstants.DELETIONSTOCK]);
                    account.Arrear = DecimalParse(reader[ClsPubDALConstants.DELETIONARREARS]);
                    account.Col1 = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    accounts.Add(account);

                    //ClsNPAaccount account = new ClsNPAaccount();
                    //account.PrimeAccountNumber = "0";
                    //account.SubAccountNumber = "0";
                    //account.Stock = "0";
                    //account.Arrear = "0";
                    //accounts.Add(account);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return accounts;
        }
        public List<ClsNPAaccount> FunPubGetNPAClosingaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            List<ClsNPAaccount> accounts = new List<ClsNPAaccount>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetBranchClosingAccount);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONCODE, DbType.String, branchSelectionCriteria.LocationCode);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCLASSID, DbType.Int32, branchSelectionCriteria.ClassId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFPREVIOUSMONTH, DbType.String, branchSelectionCriteria.CUTOFFPREVIOUSMONTH);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTH, DbType.String, branchSelectionCriteria.CutOffMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, branchSelectionCriteria.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsNPAaccount account = new ClsNPAaccount();
                    //account.RegionId = IntParse(reader[ClsPubDALConstants.REGIONID]);
                    //account.BranchId = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    account.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    account.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                    account.ClassId = IntParse(reader[ClsPubDALConstants.CLASSID]);
                    account.AssetClass = StringParse(reader[ClsPubDALConstants.ASSETACCOUNTCLASS]);
                    account.PrimeAccountNumber = StringParse(reader[ClsPubDALConstants.NPAPRIMEACCOUNTNUMBER]);
                    account.SubAccountNumber = StringParse(reader[ClsPubDALConstants.NPASUBACCOUNTNUMBER]);
                    account.Stock = DecimalParse(reader[ClsPubDALConstants.CLOSINGSTOCK]);
                    account.Arrear = DecimalParse(reader[ClsPubDALConstants.CLOSINGARREARS]);
                    account.Col1 = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    accounts.Add(account);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return accounts;
        }
        public List<ClsPubCumulativeCollection> FunPubGetCumulativeCollectionDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            List<ClsPubCumulativeCollection> collections = new List<ClsPubCumulativeCollection>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetBranchCumulativeCollection);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                if (branchSelectionCriteria.LocationID1 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, branchSelectionCriteria.LocationID1);
                }
                if (branchSelectionCriteria.LocationID2 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, branchSelectionCriteria.LocationID2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTH, DbType.String, branchSelectionCriteria.CutOffMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFINANCIALYEARFROM, DbType.String, branchSelectionCriteria.Financial_Year_From);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFINANCIALMONTH, DbType.String, branchSelectionCriteria.FinancialMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, branchSelectionCriteria.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubCumulativeCollection collection = new ClsPubCumulativeCollection();
                    collection.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    collection.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                    collection.CumulativeCollection = DecimalParse(reader[ClsPubDALConstants.CUMULATIVECOLLECTION]);
                    collection.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    collections.Add(collection);
                
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return collections;
        }
        public List<ClsPubBranchAccount> FunPubGetAccountDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            List<ClsPubBranchAccount> branchaccounts = new List<ClsPubBranchAccount>();
             try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetBranchNoofAccounts);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                if (branchSelectionCriteria.LocationID1 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, branchSelectionCriteria.LocationID1);
                }
                if (branchSelectionCriteria.LocationID2 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, branchSelectionCriteria.LocationID2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTHENDDATE, DbType.DateTime, branchSelectionCriteria.CMENDDATE);
                 IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubBranchAccount branchaccount = new ClsPubBranchAccount();
                    branchaccount.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    branchaccount.Branch= StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                    branchaccount.Accounts = StringParse(reader[ClsPubDALConstants.ACCOUNTS]);
                    branchaccounts.Add(branchaccount);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return branchaccounts;
        }
        public List<ClsPubRegionBranch> FunPubGetRegionBranchDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            List<ClsPubRegionBranch> branchstocks = new List<ClsPubRegionBranch>();

            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.getregbranch);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                if (branchSelectionCriteria.LocationID1 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, branchSelectionCriteria.LocationID1);
                }
                if (branchSelectionCriteria.LocationID2 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, branchSelectionCriteria.LocationID2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {

                    ClsPubRegionBranch branchstock = new ClsPubRegionBranch();

                    branchstock.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    branchstock.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                     branchstocks.Add(branchstock);

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return branchstocks;
        }

        public List<ClsNPAaccount> FunPubGetTotalNPAAddtionaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            List<ClsNPAaccount> accounts = new List<ClsNPAaccount>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.TotalAdditionAccount);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                if (branchSelectionCriteria.LocationID1 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, branchSelectionCriteria.LocationID1);
                }
                if (branchSelectionCriteria.LocationID2 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, branchSelectionCriteria.LocationID2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFPREVIOUSMONTH, DbType.Int32, branchSelectionCriteria.CUTOFFPREVIOUSMONTH);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTH, DbType.String, branchSelectionCriteria.CutOffMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, branchSelectionCriteria.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                 while (reader.Read())
                {
                    ClsNPAaccount account = new ClsNPAaccount();
                    //account.RegionId = IntParse(reader[ClsPubDALConstants.REGIONID]);
                    //account.BranchId = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    account.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    account.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                    account.ClassId = IntParse(reader[ClsPubDALConstants.CLASSID]);
                    account.AssetClass = StringParse(reader[ClsPubDALConstants.ASSETACCOUNTCLASS]);
                    account.PrimeAccountNumber = StringParse(reader[ClsPubDALConstants.NPAPRIMEACCOUNTNUMBER]);
                    account.SubAccountNumber = StringParse(reader[ClsPubDALConstants.NPASUBACCOUNTNUMBER]);
                    account.Stock = DecimalParse(reader[ClsPubDALConstants.ADDITIONSTOCK]);
                    account.Arrear = DecimalParse(reader[ClsPubDALConstants.ADDITIONARREARS]);
                    account.Col1 = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    accounts.Add(account);

                    //ClsNPAaccount account = new ClsNPAaccount();
                    //account.PrimeAccountNumber = "0";
                    //account.SubAccountNumber = "0";
                    //account.Stock = "0";
                    //account.Arrear = "0";
                    //accounts.Add(account);

                }

                reader.Close();
            }

            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return accounts;
        }

        public List<ClsNPAaccount> FunPubGetTotalNPAOpeningaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            List<ClsNPAaccount> accounts = new List<ClsNPAaccount>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.TotalOpeningAccount);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                if (branchSelectionCriteria.LocationID1 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, branchSelectionCriteria.LocationID1);
                }
                if (branchSelectionCriteria.LocationID2 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, branchSelectionCriteria.LocationID2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFPREVIOUSMONTH, DbType.Int32, branchSelectionCriteria.CUTOFFPREVIOUSMONTH);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTH, DbType.String, branchSelectionCriteria.CutOffMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, branchSelectionCriteria.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsNPAaccount account = new ClsNPAaccount();
                    //account.RegionId = IntParse(reader[ClsPubDALConstants.REGIONID]);
                    //account.BranchId = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    account.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    account.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                    account.ClassId = IntParse(reader[ClsPubDALConstants.CLASSID]);
                    account.AssetClass = StringParse(reader[ClsPubDALConstants.ASSETACCOUNTCLASS]);
                    account.PrimeAccountNumber = StringParse(reader[ClsPubDALConstants.NPAPRIMEACCOUNTNUMBER]);
                    account.SubAccountNumber = StringParse(reader[ClsPubDALConstants.NPASUBACCOUNTNUMBER]);
                    account.Stock = DecimalParse(reader[ClsPubDALConstants.OPENINGSTOCK]);
                    account.Arrear = DecimalParse(reader[ClsPubDALConstants.OPENINGARREARS]);
                    account.Col1 = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    accounts.Add(account);

                    //ClsNPAaccount account = new ClsNPAaccount();
                    //account.PrimeAccountNumber = "0";
                    //account.SubAccountNumber = "0";
                    //account.Stock = "0";
                    //account.Arrear = "0";
                    //accounts.Add(account);

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return accounts;
        }
        public List<ClsNPAaccount> FunPubGetTotalNPADeletionaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            List<ClsNPAaccount> accounts = new List<ClsNPAaccount>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.TotalDeletionAccount);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                if (branchSelectionCriteria.LocationID1 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, branchSelectionCriteria.LocationID1);
                }
                if (branchSelectionCriteria.LocationID2 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, branchSelectionCriteria.LocationID2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFPREVIOUSMONTH, DbType.Int32, branchSelectionCriteria.CUTOFFPREVIOUSMONTH);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTH, DbType.String, branchSelectionCriteria.CutOffMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, branchSelectionCriteria.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsNPAaccount account = new ClsNPAaccount();
                    //account.RegionId = IntParse(reader[ClsPubDALConstants.REGIONID]);
                    //account.BranchId = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    account.Col1=
                    account.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    account.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                    account.ClassId = IntParse(reader[ClsPubDALConstants.CLASSID]);
                    account.AssetClass = StringParse(reader[ClsPubDALConstants.ASSETACCOUNTCLASS]);
                    account.PrimeAccountNumber = StringParse(reader[ClsPubDALConstants.NPAPRIMEACCOUNTNUMBER]);
                    account.SubAccountNumber = StringParse(reader[ClsPubDALConstants.NPASUBACCOUNTNUMBER]);
                    account.Stock = DecimalParse(reader[ClsPubDALConstants.DELETIONSTOCK]);
                    account.Arrear = DecimalParse(reader[ClsPubDALConstants.DELETIONARREARS]);
                    account.Col1 = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    accounts.Add(account);

                    //ClsNPAaccount account = new ClsNPAaccount();
                    //account.PrimeAccountNumber = "0";
                    //account.SubAccountNumber = "0";
                    //account.Stock = "0";
                    //account.Arrear = "0";
                    //accounts.Add(account);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return accounts;
        }
        public List<ClsNPAaccount> FunPubGetTotalNPAClosingaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            List<ClsNPAaccount> accounts = new List<ClsNPAaccount>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.TotalClosingAccount);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, branchSelectionCriteria.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, branchSelectionCriteria.LobId);
                if (branchSelectionCriteria.LocationID1 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, branchSelectionCriteria.LocationID1);
                }
                if (branchSelectionCriteria.LocationID2 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, branchSelectionCriteria.LocationID2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, branchSelectionCriteria.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFPREVIOUSMONTH, DbType.Int32, branchSelectionCriteria.CUTOFFPREVIOUSMONTH);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTH, DbType.String, branchSelectionCriteria.CutOffMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, branchSelectionCriteria.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, branchSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsNPAaccount account = new ClsNPAaccount();
                    //account.RegionId = IntParse(reader[ClsPubDALConstants.REGIONID]);
                    //account.BranchId = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    account.Region = StringParse(reader[ClsPubDALConstants.LOCATIONCODE]);
                    account.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONDESC]);
                    account.ClassId = IntParse(reader[ClsPubDALConstants.CLASSID]);
                    account.AssetClass = StringParse(reader[ClsPubDALConstants.ASSETACCOUNTCLASS]);
                    account.PrimeAccountNumber = StringParse(reader[ClsPubDALConstants.NPAPRIMEACCOUNTNUMBER]);
                    account.SubAccountNumber = StringParse(reader[ClsPubDALConstants.NPASUBACCOUNTNUMBER]);
                    account.Stock = DecimalParse(reader[ClsPubDALConstants.CLOSINGSTOCK]);
                    account.Arrear = DecimalParse(reader[ClsPubDALConstants.CLOSINGARREARS]);
                    account.Col1 = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    accounts.Add(account);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return accounts;
        }

    }
}


        
        
        
        
       
    

 
          

          
               

