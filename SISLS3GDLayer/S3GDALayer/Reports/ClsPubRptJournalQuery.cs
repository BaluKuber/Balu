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
    public class ClsPubRptJournalQuery : ClsPubDalReportBase
    {
        public List<ClsPubTransaction> FunPubGetJournalDetails(ClsPubJournalParameters ObjJournalParameters)
        {

            List<ClsPubTransaction> transactions = new List<ClsPubTransaction>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetJournalDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, ObjJournalParameters.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, ObjJournalParameters.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ObjJournalParameters.ProgramId);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, ObjJournalParameters.LobId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, ObjJournalParameters.LocationId1);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, ObjJournalParameters.LocationId2);

                if (ObjJournalParameters.Panum!=null)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, ObjJournalParameters.Panum);
                }
                if (ObjJournalParameters.Sanum != null)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSANUM, DbType.String, ObjJournalParameters.Sanum);
                }
                if (ObjJournalParameters.LeaseAssetNo !=null)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLEASEASSETNO, DbType.String, ObjJournalParameters.LeaseAssetNo);
                }
                if (ObjJournalParameters.GlCode != null)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMGLCODE, DbType.String, ObjJournalParameters.GlCode);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, ObjJournalParameters.StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, ObjJournalParameters.EndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, ObjJournalParameters.Denomination);
                                
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {

                    ClsPubTransaction Tran = new ClsPubTransaction();
                    Tran.Location = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    Tran.ValueDate = reader[ClsPubDALConstants.VALUEDATE].ToString();
                    Tran.DocumentDate = reader[ClsPubDALConstants.DOCUMENTDATE].ToString();
                    Tran.DocumentReference = StringParse(reader[ClsPubDALConstants.DOCUMENTREFERENCE]);
                    Tran.GlAccount = StringParse(reader[ClsPubDALConstants.GlACCOUNT]);
                    Tran.PrimeAccountNo = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNTNUMBER]);
                    Tran.SubAccountNo = StringParse(reader[ClsPubDALConstants.SUBACCOUNTNUMBER]);
                    Tran.Lan = StringParse(reader[ClsPubDALConstants.LAN]);
                    Tran.Status = StringParse(reader[ClsPubDALConstants.STATUS]);
                    Tran.Dues = DecimalParse(reader[ClsPubDALConstants.DUES]);
                    Tran.Receipts = DecimalParse(reader[ClsPubDALConstants.RECEIPTS]);
                    Tran.Narration = StringParse(reader[ClsPubDALConstants.NARRATION]);
                    Tran.Denomination = StringParse(reader[ClsPubDALConstants.Denomination]);
                    Tran.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    Tran.Due_Flag = StringParse(reader[ClsPubDALConstants.DueFlags]);
                    transactions.Add(Tran);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return transactions;
        }

    }
}
