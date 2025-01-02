using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity.Reports;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data;
using S3GBusEntity;
using S3GDALayer.Constants;




namespace S3GDALayer.Reports
{

    public class ClsPubSOA : ClsPubDalReportBase
    {

        public List<ClsPubDropDownList> FunPubGetLOB(int CompanyId, int UserId, int ProgramId)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetLOBDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
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
            }
            return FunPriLoadSelect(dropdownLists);
        }

        public List<ClsPubDropDownList> FunPubBranch(int CompanyId, int UserId, int ProgramId, int LobId)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {

                DbCommand command = db.GetStoredProcCommand(SPNames.GetBranch);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, Is_Active);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, LobId);

                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
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
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }

            return FunPriLoadSelect(dropdownLists);

        }
        public List<ClsPubDropDownList> FunPubGetproduct(int CompanyId, string Customer_ID)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {

                DbCommand command = db.GetStoredProcCommand(SPNames.GetProductBasedCustomer);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.Int32, Customer_ID);
                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
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
            }

            return FunPriLoadSelect(dropdownLists);

        }

        public List<ClsPubPASA> FunPubGetPASA(ClsPubSOASelectionCriteria selection)
        {
            List<ClsPubPASA> PASAs = new List<ClsPubPASA>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetPASADetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, selection.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.Int32, selection.CustomerId);
                if (selection.LobId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, selection.LobId);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, selection.ProgramId);
                if (selection.LocationID1!= string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, selection.LocationID1);
                }
                if (selection.LocationID2!= string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, selection.LocationID2);
                }
                 db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, selection.UserId);
               
                if (selection.ProductId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODUCTID, DbType.Int32, selection.ProductId);
                }
                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubPASA PASA = new ClsPubPASA();
                    PASA.PrimeAccountNo = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNTNUMBER]);
                    PASA.SubAccountNo = StringParse(reader[ClsPubDALConstants.SUBACCOUNTNUMBER]);
                    PASAs.Add(PASA);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return PASAs;
        }


        //public List<ClsPubTransaction> FunPubGetTransactionDetails(int CompanyId, string StartDate, string EndDate, List<ClsPubPASA> PASAs, out decimal OpeningBalance)
        //{
        //    OpeningBalance = 0;
        //    string XMLAccountDetails = FunProGeneratePrimeSubAccount(PASAs);

        //    List<ClsPubTransaction> Trans = new List<ClsPubTransaction>();
        //    try
        //    {
        //        DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETTRANSACTIONDETAILS);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, StartDate);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, EndDate);
        //        db.AddInParameter(command, ClsPubDALConstants.XMLACCOUNTDETAILS, DbType.String, XMLAccountDetails);

        //        IDataReader reader = db.ExecuteReader(command);

        //        List<ClsPubSOAOpeningBalance> soaOpeningBalances = new List<ClsPubSOAOpeningBalance>();

        //        decimal remainingBalance = 0;
        //        while (reader.Read())
        //        {
        //            remainingBalance = OpeningBalance = DecimalParse(reader[ClsPubDALConstants.BALANCE]);
        //        }

        //        reader.NextResult();
        //        while (reader.Read())
        //        {
        //            ClsPubTransaction Tran = new ClsPubTransaction();
        //            Tran.PrimeAccountNo = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNTNUMBER]);
        //            Tran.SubAccountNo = StringParse(reader[ClsPubDALConstants.SUBACCOUNTNUMBER]);
        //            Tran.DocumentDate = reader[ClsPubDALConstants.DOCUMENTDATE].ToString();
        //            Tran.ValueDate = reader[ClsPubDALConstants.VALUEDATE].ToString();
        //            Tran.DocumentReference = StringParse(reader[ClsPubDALConstants.DOCUMENTREFERENCE]);
        //            Tran.Description = StringParse(reader[ClsPubDALConstants.DESCRIPTION]);
        //            Tran.Dues = DecimalParse(reader[ClsPubDALConstants.DUES]);
        //            Tran.Receipts = DecimalParse(reader[ClsPubDALConstants.RECEIPTS]);

        //            //Tran.Dues = reader[ClsPubDALConstants.DUES];
        //            //Tran.Receipts =reader[ClsPubDALConstants.RECEIPTS];
        //            Tran.Balance = DecimalParse(reader[ClsPubDALConstants.BALANCE]);

        //            Tran.Balance = remainingBalance + Tran.Dues - Tran.Receipts;
        //            remainingBalance = Tran.Balance;

        //            Trans.Add(Tran);
        //        }
        //        reader.Close();

        //        //if (Trans.Count > 0)
        //        //{
        //        //    ClsPubTransaction Tran = new ClsPubTransaction();
        //        //    Tran.PrimeAccountNo = string.Empty;
        //        //    Tran.SubAccountNo = string.Empty;
        //        //    Tran.DocumentDate = string.Empty;
        //        //    Tran.ValueDate = string.Empty;
        //        //    Tran.DocumentReference = string.Empty;
        //        //    //Tran.Description = StringParse(reader[ClsPubDALConstants.DESCRIPTION]);
        //        //    Tran.Dues = 0;
        //        //    Tran.Receipts = 0;
        //        //    Tran.Balance = OpeningBalance;
        //        //    Trans.Insert(0, Tran);
        //        //}

        //    }

        //    catch (Exception ex)
        //    {
        //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
        //    }
        //    return Trans;
        //}

        public ClsPubSummary FunPubGetSummaryDetails(int CompanyId, string StartDate, string EndDate, List<ClsPubPASA> PASAs)
        {
            string XMLAccountDetails = FunProGeneratePrimeSubAccount(PASAs);
            ClsPubSummary summary = new ClsPubSummary();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetSummarydetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, EndDate);
                db.AddInParameter(command, ClsPubDALConstants.XMLACCOUNTDETAILS, DbType.String, XMLAccountDetails);

                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    summary.InstallmentDues = DecimalParse(reader[ClsPubDALConstants.INSTALLMENTDUE]);
                    summary.InterestDues = DecimalParse(reader[ClsPubDALConstants.INTERESTDUE]);
                    summary.InsuranceDues = DecimalParse(reader[ClsPubDALConstants.INSURANCEDUE]);
                    summary.ODIDues = DecimalParse(reader[ClsPubDALConstants.ODIDUE]);
                    summary.OtherDues = DecimalParse(reader[ClsPubDALConstants.OTHERDUE]);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return summary;
        }

        public ClsPubMemorandum FunPubGetMemorandumDetails(int CompanyId, string StartDate, string EndDate, List<ClsPubPASA> PASAs)
        {
            string XMLAccountDetails = FunProGeneratePrimeSubAccount(PASAs);
            ClsPubMemorandum memo = new ClsPubMemorandum();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetMemorandumdetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, EndDate);
                db.AddInParameter(command, ClsPubDALConstants.XMLACCOUNTDETAILS, DbType.String, XMLAccountDetails);

                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    memo.ChequeReturnDues = DecimalParse(reader[ClsPubDALConstants.CHEQUERETURNDUE]);
                    memo.DocumentChargesDues = DecimalParse(reader[ClsPubDALConstants.DOCUMENTCHARGESDUE]);
                    memo.ODIDues = DecimalParse(reader[ClsPubDALConstants.ODIDUE]);
                    memo.VerificationChargesDues= DecimalParse(reader[ClsPubDALConstants.VERIFICATIONCHARGESDUE]);
                    memo.OtherDues = DecimalParse(reader[ClsPubDALConstants.OTHERDUE]);
                    
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return memo;
        }

        public ClsPubSOAAsset FunPubGetAssetDetails(int CompanyId, string StartDate, string EndDate, List<ClsPubPASA> PASAs, out decimal openingbalance)
        {
            string XMLAccountDetails = FunProGeneratePrimeSubAccount(PASAs);
            decimal remainingBalance = 0;
            openingbalance = 0;
            ClsPubSOAAsset assets = new ClsPubSOAAsset();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetTransactionDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, EndDate);
                db.AddInParameter(command, ClsPubDALConstants.XMLACCOUNTDETAILS, DbType.String, XMLAccountDetails);

                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
                command.CommandTimeout = 3600;
                IDataReader reader = db.FunPubExecuteReader(command);
                //assets.SOABalance = new List<ClsPubSOABalance>();
                assets.AssetDetails = new List<ClsPubAsset>();
                assets.AccountDetails = new List<ClsPubAsset>();
                assets.Transaction = new List<ClsPubTransaction>();
                assets.Openingbalance = new List<ClsPubSOAOpeningBalance>();
                while (reader.Read())
                {
                    //ClsPubSOABalance soabalance = new ClsPubSOABalance();
                    remainingBalance = openingbalance = DecimalParse(reader[ClsPubDALConstants.SOABAL]);
                    //assets.SOABalance.Add(openingbalance);
                    
                }  
                reader.NextResult();
                while (reader.Read())
                {
                    ClsPubSOAOpeningBalance openingbalnce = new ClsPubSOAOpeningBalance();
                    openingbalnce.PrimeAccountNo = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNTNUMBER]);
                    openingbalnce.SubAccountNo = StringParse(reader[ClsPubDALConstants.SUBACCOUNTNUMBER]);
                    openingbalnce.LobName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    openingbalnce.Balance = DecimalParse(reader[ClsPubDALConstants.SOABALANCE]);
                    openingbalnce.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    openingbalnce.Denomination = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    assets.Openingbalance.Add(openingbalnce);

                }
                reader.NextResult();
                while (reader.Read())
                {
                    
                    //OpeningBalance = 0;
                    //decimal remainingBalance = 0;
                    //while (reader.Read())
                    //{
                    //    remainingBalance = OpeningBalance = DecimalParse(reader[ClsPubDALConstants.BALANCE]);
                    //}
                    ClsPubTransaction Tran = new ClsPubTransaction();
                    Tran.PrimeAccountNo = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNTNUMBER]);
                    Tran.SubAccountNo = StringParse(reader[ClsPubDALConstants.SUBACCOUNTNUMBER]);
                    Tran.DocumentDate = reader[ClsPubDALConstants.DOCUMENTDATE].ToString();
                    Tran.ValueDate = reader[ClsPubDALConstants.VALUEDATE].ToString();
                    Tran.DocumentReference = StringParse(reader[ClsPubDALConstants.DOCUMENTREFERENCE]);
                    Tran.Description = StringParse(reader[ClsPubDALConstants.DESCRIPTION]);
                    Tran.Dues = DecimalParse(reader[ClsPubDALConstants.DUES]);
                    Tran.Receipts = DecimalParse(reader[ClsPubDALConstants.RECEIPTS]);
                    Tran.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);

                   // Tran.Balance = DecimalParse(reader[ClsPubDALConstants.BALANCE]);

                    Tran.Balance = remainingBalance + Tran.Dues - Tran.Receipts;
                    remainingBalance = Tran.Balance;

                    assets.Transaction.Add(Tran);
                }
                reader.NextResult();

                while (reader.Read())
                {
                    ClsPubAsset asset = new ClsPubAsset();
                    asset.LobName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    asset.PrimeAccountNo = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNTNUMBER]);
                    asset.SubAccountNo = StringParse(reader[ClsPubDALConstants.SUBACCOUNTNUMBER]);
                    asset.AssetDesc = StringParse(reader[ClsPubDALConstants.ASSETDETAILS]);
                    asset.RegNo = StringParse(reader[ClsPubDALConstants.SLREGNO]);
                    asset.Terms = StringParse(reader[ClsPubDALConstants.TERMS]);
                    asset.PO_Number = StringParse(reader[ClsPubDALConstants.PO_Number]);
                    asset.PO_Date = StringParse(reader[ClsPubDALConstants.PO_Date]);
                    asset.PI_Number = StringParse(reader[ClsPubDALConstants.PI_Number]);
                    asset.PI_Date = StringParse(reader[ClsPubDALConstants.PI_Date]);
                    asset.VI_Number = StringParse(reader[ClsPubDALConstants.VI_Number]);
                    asset.VI_Date = StringParse(reader[ClsPubDALConstants.VI_Date]);
                    asset.Entity_Name = StringParse(reader[ClsPubDALConstants.Entity_Name]);
                    asset.Asset_Category = StringParse(reader[ClsPubDALConstants.Asset_Category]);
                    asset.Asset_Type = StringParse(reader[ClsPubDALConstants.Asset_Type]);
                    asset.Asset_Sub_Type = StringParse(reader[ClsPubDALConstants.Asset_Sub_Type]);
                    asset.Total_SCH_Amt = DecimalParse(reader[ClsPubDALConstants.Total_SCH_Amt]);
                    assets.AssetDetails.Add(asset);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ClsPubAsset asset = new ClsPubAsset();
                    asset.LobName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    asset.PrimeAccountNo = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNTNUMBER]);
                    asset.SubAccountNo = StringParse(reader[ClsPubDALConstants.SUBACCOUNTNUMBER]);
                    asset.AmountFinanced = DecimalParse(reader[ClsPubDALConstants.AMOUNTFINANCED]);
                    asset.YetToBeBilled = DecimalParse(reader[ClsPubDALConstants.YETTOBEBILLED]);
                    asset.Billed = DecimalParse(reader[ClsPubDALConstants.BILLED]);
                    asset.Balance = DecimalParse(reader[ClsPubDALConstants.BALANCE]);
                    asset.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    asset.InvoiceAmount = DecimalParse(reader[ClsPubDALConstants.INVOICE_AMOUNT]);
                    asset.CapitalisedAmount = DecimalParse(reader[ClsPubDALConstants.CAPITALISEDAMOUNT]);
                    assets.AccountDetails.Add(asset);
                }
                
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return assets;
        }


        public List<ClsPubSummaryAccount> FunPubGetSummaryAccountDetails(int CompanyId, string StartDate, string EndDate, List<ClsPubPASA> PASAs)
        {
            string XMLAccountDetails = FunProGeneratePrimeSubAccount(PASAs);
           List<ClsPubSummaryAccount> accounts = new List<ClsPubSummaryAccount>();
              try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.summary);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, EndDate);
                db.AddInParameter(command, ClsPubDALConstants.XMLACCOUNTDETAILS, DbType.String, XMLAccountDetails);

                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubSummaryAccount account = new ClsPubSummaryAccount();
                    account.GPSSuffix= StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    account.PrimeAccountNo = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNT]);
                    account.SubAccountNo = StringParse(reader[ClsPubDALConstants.SANUM]);
                    account.InstallmentDues = DecimalParse(reader[ClsPubDALConstants.INSTALLMENTDUE]);
                    account.InterestDues = DecimalParse(reader[ClsPubDALConstants.INTERESTDUE]);
                    account.InsuranceDues = DecimalParse(reader[ClsPubDALConstants.INSURANCEDUE]);
                    account.ODIDues = DecimalParse(reader[ClsPubDALConstants.ODIDUE]);
                    account.OtherDues = DecimalParse(reader[ClsPubDALConstants.OTHERDUE]);
                    //account.description = StringParse(reader[ClsPubDALConstants.DESCRIPTION]);
                    //account.summaryaccount = DecimalParse(reader[ClsPubDALConstants.SUMMARYACCOUNT]);
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

        public List<ClsPubMemorandumAccount> FunPubGetMemorandumAccountDetails(int CompanyId, string StartDate, string EndDate, List<ClsPubPASA> PASAs)
        {
            string XMLAccountDetails = FunProGeneratePrimeSubAccount(PASAs);
            List<ClsPubMemorandumAccount> accounts = new List<ClsPubMemorandumAccount>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.getmemoaccount);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, EndDate);
                db.AddInParameter(command, ClsPubDALConstants.XMLACCOUNTDETAILS, DbType.String, XMLAccountDetails);

                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubMemorandumAccount account = new ClsPubMemorandumAccount();
                    account.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    account.PrimeAccountNo = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNT]);
                    account.SubAccountNo = StringParse(reader[ClsPubDALConstants.SANUM]);
                    account.ChequeReturnDues = DecimalParse(reader[ClsPubDALConstants.CHEQUERETURNDUE]);
                    account.DocumentChargesDues = DecimalParse(reader[ClsPubDALConstants.DOCUMENTCHARGESDUE]);
                    account.ODIDues = DecimalParse(reader[ClsPubDALConstants.ODIDUE]);
                    account.VerificationChargesDues = DecimalParse(reader[ClsPubDALConstants.VERIFICATIONCHARGESDUE]);
                    account.OtherDues = DecimalParse(reader[ClsPubDALConstants.OTHERDUE]);
                    //account.description = StringParse(reader[ClsPubDALConstants.DESCRIPTION]);
                    //account.summaryaccount = DecimalParse(reader[ClsPubDALConstants.SUMMARYACCOUNT]);
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