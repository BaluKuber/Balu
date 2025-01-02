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
    public class ClsPubRptPaymentDemandCollectionNPA : ClsPubDalReportBase
    {
        //public List<ClsPubPaymentDCNPADetails> FunPubGetPaymentDemandCollectionNPA(ClsPubPaymentDCNPAParameters Payment)
        public ClsPubCumulative FunPubGetPaymentDemandCollectionNPA(ClsPubPaymentDCNPAParameters Payment)
        {
            //List<ClsPubPaymentDCNPADetails> PaymentDCs = new List<ClsPubPaymentDCNPADetails>();
            ClsPubCumulative cumulative = new ClsPubCumulative();
            cumulative.PaymentDCNPADetails = new List<ClsPubPaymentDCNPADetails>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.Payment_DC_NPA);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, Payment.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, Payment.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, Payment.ProgramId);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, Payment.LobId);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, Payment.LocationId1);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, Payment.LocationId2);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODUCTID, DbType.Int32, Payment.ProductId);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTOMONTHSTARTDATE, DbType.DateTime, Payment.ToMonthStartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTOMONTHENDDATE, DbType.DateTime, Payment.ToMonthEndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTOMONTHPREVIOUSMONTHSTARTDATE, DbType.DateTime, Payment.PreToMonthStartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTOMONTHPREVIOUSMONTHENDDATE, DbType.DateTime, Payment.PreToMonthEndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFORMMONTHSTARTDATE, DbType.DateTime, Payment.FromMonthStartDate);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFROMMONTHPREVIOUSYEARSTARTDATE, DbType.DateTime, Payment.PreYearFromMonthStartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTOMONTHPREVIOUSYEARENDDATE, DbType.DateTime, Payment.PreYearToMonthEndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFINYEARSTARTMONTHSTARTDATE, DbType.DateTime, Payment.FinYearStartMonthStartDate);
                
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPREFROMMONTH, DbType.String, Payment.PreFromMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFROMMONTH, DbType.String, Payment.FromMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTOMONTH, DbType.String, Payment.ToMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMOPENINGMONTH, DbType.String, Payment.OpeningMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Int32, Payment.Denomination);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubPaymentDCNPADetails PaymentDC = new ClsPubPaymentDCNPADetails();

                    PaymentDC.Month = IntParse(reader[ClsPubDALConstants.MONTH]);
                    PaymentDC.LobId = IntParse(reader[ClsPubDALConstants.LOBID]);
                    PaymentDC.Lob = StringParse(reader[ClsPubDALConstants.LOB]);
                    PaymentDC.RegionId = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    PaymentDC.Region = StringParse(reader[ClsPubDALConstants.LOCATION]);
                   
                    PaymentDC.ProductId = IntParse(reader[ClsPubDALConstants.PRODUCTID]);
                    PaymentDC.Product = StringParse(reader[ClsPubDALConstants.PRODUCTS]);
                    PaymentDC.GrowthPercentageLastMonth = DecimalParse(reader[ClsPubDALConstants.GROWTHPERCENTAGELASTMONTH]);
                    PaymentDC.Cumulative = DecimalParse(reader[ClsPubDALConstants.CUMULATIVE]);
                    PaymentDC.GrowthPercentageLastYear = DecimalParse(reader[ClsPubDALConstants.GROWTHPERCENTAGELASTYEAR]);
                    PaymentDC.ArrearDemand = DecimalParse(reader[ClsPubDALConstants.ARREARDEMAND]);
                    PaymentDC.ArrearCollection = DecimalParse(reader[ClsPubDALConstants.ARREARCOLLECTION]);
                    PaymentDC.ArrearCollectionPercentage = DecimalParse(reader[ClsPubDALConstants.ARREARCOLLECTIONPERCENTAGE]);
                    PaymentDC.CurrentDemand = DecimalParse(reader[ClsPubDALConstants.CURRENTDEMAND]);
                    PaymentDC.CurrentCollection = DecimalParse(reader[ClsPubDALConstants.CURRENTCOLLECTION]);
                    PaymentDC.CurrentPercentage = DecimalParse(reader[ClsPubDALConstants.CURRENTPERCENTAGE]);
                    PaymentDC.TotalDemand = DecimalParse(reader[ClsPubDALConstants.TOTALDEMAND]);
                    PaymentDC.TotalCollection = DecimalParse(reader[ClsPubDALConstants.TOTALCOLLECTION]);
                    PaymentDC.TotalPercentage = DecimalParse(reader[ClsPubDALConstants.TOTALPERCENTAGE]);
                    PaymentDC.BadDebts = 0;
                    //PaymentDC.BadDebts = DecimalParse(reader[ClsPubDALConstants.BADDEBTS]);
                    PaymentDC.Stock = DecimalParse(reader[ClsPubDALConstants.STOCK]);
                    PaymentDC.Npa = DecimalParse(reader[ClsPubDALConstants.NPA]);
                    PaymentDC.NpaPercentage = DecimalParse(reader[ClsPubDALConstants.NPAPERCENTAGE]);
                    PaymentDC.ClosingArrear = DecimalParse(reader[ClsPubDALConstants.CLOSINGARREAR]);
                    PaymentDC.ArrearPercentage = DecimalParse(reader[ClsPubDALConstants.ARREARPERCENTAGE]);
                    PaymentDC.Denomination = StringParse(reader[ClsPubDALConstants.Denomination]);
                    PaymentDC.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    //PaymentDCs.Add(PaymentDC);

                    cumulative.PaymentDCNPADetails.Add(PaymentDC);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    cumulative.Cumm = DecimalParse(reader[ClsPubDALConstants.CUMU]);
                    cumulative.CA_Exists = IntParse(reader[ClsPubDALConstants.CA_Exists]);
                    
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return cumulative;
        }
    }
}
