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
    public class ClsPubRptFactoringMaturity : ClsPubDalReportBase
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


        public ClsPubFactoringMaturity FunPubGetFactoringMaturityDetails(ClsPubFactoringMaturitySelectionCriteria factoringSelectionCriteria)
        {
            ClsPubFactoringMaturity factoring = new ClsPubFactoringMaturity();
            factoring.FactoringMaturity = new List<ClsPubFactoringMaturityDetails>();
            //factoring.FactoringMaturityReport = new List<ClsPubFactoringMaturityReport>();
            //factoring.loblocation = new List<ClsPubLobLocation>();
            // List<ClsPubDisbursementDetails> details = new List<ClsPubDisbursementDetails>();

            try
            {
                DbCommand command = db.GetStoredProcCommand("S3G_GET_MATURITY_DTL");
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, factoringSelectionCriteria.CompanyId);

                if (factoringSelectionCriteria.LobId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, factoringSelectionCriteria.LobId);
                }
                if (factoringSelectionCriteria.LocationID != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID, DbType.Int32, factoringSelectionCriteria.LocationID);
                }
                
                if (factoringSelectionCriteria.DenominationId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Int32, factoringSelectionCriteria.DenominationId);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDATE, DbType.DateTime, factoringSelectionCriteria.Date);              
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, factoringSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                
                while (reader.Read())
                {
                    ClsPubFactoringMaturityDetails details = new ClsPubFactoringMaturityDetails();
                    details.Location = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                    details.CustomerName = StringParse(reader[ClsPubDALConstants.CUSTOMERNAME]);
                    details.VendorName = StringParse(reader[ClsPubDALConstants.VENDORNAME]);
                    details.PrimeAccount = StringParse(reader[ClsPubDALConstants.PANUM]);
                    details.SubAccount = StringParse(reader[ClsPubDALConstants.SANUM]);
                    details.InvoiceNo = StringParse(reader[ClsPubDALConstants.INVOICENO]);
                    details.InvoiceAmount = DecimalParse(reader[ClsPubDALConstants.INVOICEAMOUNT]);
                    details.MarginAmount = DecimalParse(reader[ClsPubDALConstants.MARGINAMOUNT]);
                    details.FILNo = StringParse(reader[ClsPubDALConstants.FILNO]);
                    details.DiscountDate = StringParse(reader[ClsPubDALConstants.DISCOUNTDATE]);                        
                    details.CreditDays = IntParse(reader[ClsPubDALConstants.CREDITDAYS]);
                    details.MaturityDate = StringParse(reader[ClsPubDALConstants.MATURITYDATE]);
                    details.PrinAmount = DecimalParse(reader[ClsPubDALConstants.PRINAMOUNT]);
                    details.INPUTDATE_1 = StringParse(reader[ClsPubDALConstants.INPUTPARAMDATE]);
                    factoring.FactoringMaturity.Add(details);
                    

                }
                //reader.NextResult();

                //while (reader.Read())
                //{
                //    ClsPubFactoringMaturityReport report = new ClsPubFactoringMaturityReport();
                //    report.Location = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                //    report.CustomerName = StringParse(reader[ClsPubDALConstants.CUSTOMERNAME]);
                //    report.VendorName = StringParse(reader[ClsPubDALConstants.VENDORNAME]);
                //    report.InvoiceNo = StringParse(reader[ClsPubDALConstants.INVOICENO]);
                //    report.InvoiceAmount = DecimalParse(reader[ClsPubDALConstants.INVOICEAMOUNT]);
                //    report.MarginAmount = DecimalParse(reader[ClsPubDALConstants.MARGINAMOUNT]);
                //    report.PrimeAccount = StringParse(reader[ClsPubDALConstants.PANUM]);
                //    report.FILNo = StringParse(reader[ClsPubDALConstants.FILNO]);
                //    report.DiscountDate = StringParse(reader[ClsPubDALConstants.DISCOUNTDATE]);
                //    report.CreditDays = IntParse(reader[ClsPubDALConstants.CREDITDAYS]);
                //    report.MaturityDate = StringParse(reader[ClsPubDALConstants.MATURITYDATE]);
                //    report.PrinAmount = DecimalParse(reader[ClsPubDALConstants.PRINAMOUNT]);
                //    report.INPUTDATE_1 = StringParse(reader[ClsPubDALConstants.INPUTPARAMDATE]);
                //    factoring.FactoringMaturityReport.Add(report);

                //}

                reader.Close();

                
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return factoring;
        }
    }
}
