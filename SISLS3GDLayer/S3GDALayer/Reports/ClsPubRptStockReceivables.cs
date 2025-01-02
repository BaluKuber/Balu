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
    public class ClsPubRptStockReceivables : ClsPubDalReportBase
    {
        public List<ClsPubStockReceivableDetails> FunPubGetStockReceivablesDetails(ClsPubStockReceivableparameters Stock)
        {
            List<ClsPubStockReceivableDetails> StockReceivables = new List<ClsPubStockReceivableDetails>();
            try
            {
                DbCommand command = db.GetStoredProcCommand("S3G_RPT_STOCKRECEIVABLEDETAILSNEW_CUST");
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, Stock.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, Stock.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, Stock.LobId);
                if (Stock.LocationId1 != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, Stock.LocationId1);
                }
                if (Stock.LocationId2 != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, Stock.LocationId2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMREFERENCEID, DbType.Int32, Stock.REFERENCE_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTID, DbType.Int32, Stock.CUST_ID);
                
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.String, Stock.StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.String, Stock.EndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Int32, Stock.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMGROUPID, DbType.Int32, Stock.GROUP_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMINDUSTRYID, DbType.Int32, Stock.INDUSTRY_ID);
               
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFROMMONTH, DbType.String, Stock.FromMonth);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTOMONTH, DbType.String, Stock.ToMonth);
                //IDataReader reader = db.ExecuteReader(command); modified by Sangeetha on 28th April for Oracle Conversion
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubStockReceivableDetails StockReceivable = new ClsPubStockReceivableDetails();
                  //  StockReceivable.Month = IntParse(reader[ClsPubDALConstants.MONTH]);
                    //StockReceivable.LOBId = IntParse(reader[ClsPubDALConstants.LOBID]);
                    StockReceivable.LOB = StringParse(reader[ClsPubDALConstants.LOB]);
                    StockReceivable.LOB_ID = IntParse(reader[ClsPubDALConstants.LOB_ID]);
                    StockReceivable.LOCATION_ID = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    StockReceivable.LOCATION_NAME = StringParse(reader[ClsPubDALConstants.LOCATION_NAME]);
                    StockReceivable.CUSTOMER_ID = IntParse(reader[ClsPubDALConstants.CUSTOMER_ID]);
                    StockReceivable.CustomerCodeName = StringParse(reader[ClsPubDALConstants.CUSTOMERCODENAME]);
                    //StockReceivable.GROUP_ID = IntParse(reader[ClsPubDALConstants.GROUP_ID]);
                    //StockReceivable.GROUP_NAME = StringParse(reader[ClsPubDALConstants.GROUP_NAME]);
                    //StockReceivable.INDUSTRY_ID = IntParse(reader[ClsPubDALConstants.INDUSTRY_ID]);
                    //StockReceivable.INDUSTRY_NAME = StringParse(reader[ClsPubDALConstants.INDUSTRY_NAME]);
                    StockReceivable.PANum = StringParse(reader[ClsPubDALConstants.PANUM]);
                    StockReceivable.SANum = StringParse(reader[ClsPubDALConstants.SANUM]);
                    StockReceivable.GrossStock = DecimalParse(reader[ClsPubDALConstants.GROSSSTOCK]);
                    StockReceivable.UMFC = DecimalParse(reader[ClsPubDALConstants.UMFC]);
                    StockReceivable.BilledUncollectedPrincipal = DecimalParse(reader[ClsPubDALConstants.BILLEDUNCOLLECTEDPRINCIPAL]);
                    StockReceivable.BilledUncollectedFC = DecimalParse(reader[ClsPubDALConstants.BILLEDUNCOLLECTEDFC]);
                    StockReceivable.NoofInstallment = IntParse(reader[ClsPubDALConstants.NOOFINSTALLMENT]);
                    StockReceivable.NetStock = DecimalParse(reader[ClsPubDALConstants.NETSTOCK]);
                    StockReceivables.Add(StockReceivable);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return StockReceivables;
        }
    }
}
