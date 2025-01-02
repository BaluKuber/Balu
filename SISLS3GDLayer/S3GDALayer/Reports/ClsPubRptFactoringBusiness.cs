using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity;
using S3GDALayer.Constants;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity.Reports;
using System.Data.Common;
using System.Data.Sql;
using System.Xml.Linq;
using System.Data;

namespace S3GDALayer.Reports
{
    public class ClsPubRptFactoringBusiness : ClsPubDalReportBase
    {
        public List<ClsPubDropDownList> FunPubLOB_FT(int CompanyId, int UserId, int ProgramId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand("S3G_RPT_GetRepayLobDetails_FT");
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, Is_Active);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);


                //IDataReader reader = db.FunPubExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
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


        public ClsPubFactoringBusiness FunPubGetFactoringBusinessDetails(ClsPubFactoringBusinessSelectionCriteria factoringBusinessSelectionCriteria)
        {
            ClsPubFactoringBusiness factoring = new ClsPubFactoringBusiness();
            factoring.FactoringBusiness = new List<ClsPubFactoringBusinessDetails>();
            factoring.FactoringBusinessReport = new List<ClsPubFactoringBusinessReport>();
            //factoring.loblocation = new List<ClsPubLobLocation>();
            // List<ClsPubDisbursementDetails> details = new List<ClsPubDisbursementDetails>();

            try
            {
                DbCommand command = db.GetStoredProcCommand("S3G_GET_BUSINESS_DTL");
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, factoringBusinessSelectionCriteria.CompanyId);

                if (factoringBusinessSelectionCriteria.LobId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, factoringBusinessSelectionCriteria.LobId);
                }
                if (factoringBusinessSelectionCriteria.LocationID != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID, DbType.Int32, factoringBusinessSelectionCriteria.LocationID);
                }
                if (factoringBusinessSelectionCriteria.DenominationId != 0)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Int32, factoringBusinessSelectionCriteria.DenominationId);
                }
                
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, factoringBusinessSelectionCriteria.StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, factoringBusinessSelectionCriteria.EndDate);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, factoringBusinessSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                //if (factoringBusinessSelectionCriteria.IsDetail)
                //{

                //while (reader.Read())
                //{
                //    ClsPubLobLocation location = new ClsPubLobLocation();
                //    location.LobName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                //    location.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                //    disburse.loblocation.Add(location);
                //}
                //reader.NextResult();
                while (reader.Read())
                {
                    ClsPubFactoringBusinessDetails details = new ClsPubFactoringBusinessDetails();
                    //details.FILNo = StringParse(reader[ClsPubDALConstants.FILNO]);
                    details.FILDate = StringParse(reader[ClsPubDALConstants.FILDATE]);
                    details.CustomerName = StringParse(reader[ClsPubDALConstants.CUSTOMERNAME]);
                    details.PrimeAccount = StringParse(reader[ClsPubDALConstants.PANUM]);
                    details.SubAccount = StringParse(reader[ClsPubDALConstants.SANUM]);
                    details.Location = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                    details.Credit_Amount = DecimalParse(reader[ClsPubDALConstants.CREDITAMOUNT]);
                    details.CreditAvailable = DecimalParse(reader[ClsPubDALConstants.CREDITAVAILABLE]);
                    details.InvoiceAmount = DecimalParse(reader[ClsPubDALConstants.INVOICEAMOUNT]);
                    details.DISCOUNT_AMOUNT = DecimalParse(reader[ClsPubDALConstants.DISCOUNTAMOUNT]);
                    details.INTEREST_RATE = DecimalParse(reader[ClsPubDALConstants.INTERESTRATE]);
                    details.VendorName = StringParse(reader[ClsPubDALConstants.VENDORNAME]);
                    //details.InvoiceNo = StringParse(reader[ClsPubDALConstants.INVOICENO]);
                    details.MaturityDate = StringParse(reader[ClsPubDALConstants.MATURITYDATE]);          
                    factoring.FactoringBusiness.Add(details);
                    //detail.Ageing60Days = string.Empty; ;
                    //detail.Ageingabove60Days = string.Empty;

                    //Detail.Ageing30Days = StringParse(reader[ClsPubDALConstants.AGEING30DAYS]);
                    //Detail.Ageing60Days  = StringParse(reader[ClsPubDALConstants.AGEING60DAYS]);
                    //Detail.Ageingabove60Days  = StringParse(reader[ClsPubDALConstants.AGEINGABOVE60DAYS]);

                }
                reader.NextResult();

                while (reader.Read())
                {
                    ClsPubFactoringBusinessReport report = new ClsPubFactoringBusinessReport();
                    //report.FILNo = StringParse(reader[ClsPubDALConstants.FILNO]);
                    report.FILDate = StringParse(reader[ClsPubDALConstants.FILDATE]);
                    report.CustomerName = StringParse(reader[ClsPubDALConstants.CUSTOMERNAME]);
                    report.PrimeAccount = StringParse(reader[ClsPubDALConstants.PANUM]);
                    report.SubAccount = StringParse(reader[ClsPubDALConstants.SANUM]);
                    report.Location = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                    report.Credit_Amount = DecimalParse(reader[ClsPubDALConstants.CREDITAMOUNT]);
                    report.CreditAvailable = DecimalParse(reader[ClsPubDALConstants.CREDITAVAILABLE]);
                    report.InvoiceAmount = DecimalParse(reader[ClsPubDALConstants.INVOICEAMOUNT]);
                    report.DISCOUNT_AMOUNT = DecimalParse(reader[ClsPubDALConstants.DISCOUNTAMOUNT]);
                    report.INTEREST_RATE = DecimalParse(reader[ClsPubDALConstants.INTERESTRATE]);
                    report.VendorName = StringParse(reader[ClsPubDALConstants.VENDORNAME]);
                    //report.InvoiceNo = StringParse(reader[ClsPubDALConstants.INVOICENO]);
                    report.MaturityDate = StringParse(reader[ClsPubDALConstants.MATURITYDATE]);
                    factoring.FactoringBusinessReport.Add(report);

                }

                reader.Close();

                //}
                //else
                //{
                //    while (reader.Read())
                //    {
                //        ClsPubLobLocation location = new ClsPubLobLocation();
                //        location.LobName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                //        location.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                //        disburse.loblocation.Add(location);
                //    }
                //    reader.NextResult();
                //    while (reader.Read())
                //    {
                //        ClsPubDisbursementDetails detail = new ClsPubDisbursementDetails();
                //        detail.LobName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                //        //detail.Region = StringParse(reader[ClsPubDALConstants.REGIONNAME]);
                //        detail.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                //        detail.Product = StringParse(reader[ClsPubDALConstants.PRODUCT]);
                //        detail.ApprovedAmount = DecimalParse(reader[ClsPubDALConstants.APPROVEDAMOUNT]);
                //        detail.PaidAmount = DecimalParse(reader[ClsPubDALConstants.PAIDAMOUNT]);
                //        detail.RemainingAmount = DecimalParse(reader[ClsPubDALConstants.REMAININGAMOUNT]);
                //        detail.AccountYetToBeCreated = DecimalParse(reader[ClsPubDALConstants.ACCOUNTYETAMOUNT]);
                //        detail.ageing0days = DecimalParse(reader[ClsPubDALConstants.AGEING0DAYS]);
                //        detail.ageing30days = DecimalParse(reader[ClsPubDALConstants.AGEING30DAYS]);
                //        detail.ageing60days = DecimalParse(reader[ClsPubDALConstants.AGEING60DAYS]);
                //        detail.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                //        //detail.Ageing60Days = string.Empty;
                //        //detail.Ageingabove60Days = string.Empty;
                //        disburse.Disbursement.Add(detail);
                //        //Detail.Ageing30Days = StringParse(reader[ClsPubDALConstants.AGEING30DAYS]);
                //        //Detail.Ageing60Days  = StringParse(reader[ClsPubDALConstants.AGEING60DAYS]);
                //        //Detail.Ageingabove60Days  = StringParse(reader[ClsPubDALConstants.AGEINGABOVE60DAYS]);

                //    }
                //    reader.Close();
                //}
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return factoring;
        }
    }
}
