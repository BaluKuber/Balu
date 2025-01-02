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
    public class ClsPubVendorDetails : ClsPubDalReportBase
    {
        public List<ClsPubTransaction> FunPubGetVendorDetails(int CompanyId,string EntityCode, string StartDate, string EndDate,decimal Denomination, out decimal OpeningBalance)
        {
            List<ClsPubTransaction> transactions = new List<ClsPubTransaction>();
            
                DbCommand command = db.GetStoredProcCommand(SPNames.GetVendorDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENTITYCODE, DbType.String, EntityCode);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, EndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, Denomination);
            
                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 3/April/2012
                IDataReader reader = db.FunPubExecuteReader(command);

                OpeningBalance = 0;

                while (reader.Read())
                {
                    OpeningBalance =DecimalParse (reader[ClsPubDALConstants.SOABALANCE]);
                }
                reader.NextResult();
                while (reader.Read())
                {

                    ClsPubTransaction Tran = new ClsPubTransaction();
                    Tran.PrimeAccountNo = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNTNUMBER]);
                    Tran.SubAccountNo = StringParse(reader[ClsPubDALConstants.SUBACCOUNTNUMBER]);
                    Tran.DocumentDate = reader[ClsPubDALConstants.DOCUMENTDATE].ToString();
                    Tran.ValueDate = reader[ClsPubDALConstants.VALUEDATE].ToString();
                    Tran.DocumentReference = StringParse(reader[ClsPubDALConstants.DOCUMENTREFERENCE]);
                    Tran.Description = StringParse(reader[ClsPubDALConstants.DESCRIPTION]);
                    Tran.Dues = DecimalParse(reader[ClsPubDALConstants.DUES]);
                    Tran.Receipts = DecimalParse(reader[ClsPubDALConstants.RECEIPTS]);
                    Tran.Balance = DecimalParse(reader[ClsPubDALConstants.BALANCE]);
                    Tran.Denomination = StringParse(reader[ClsPubDALConstants.Denomination]);
                    Tran.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);

                    transactions.Add(Tran);
                }
            return transactions;
        }
    }
}
