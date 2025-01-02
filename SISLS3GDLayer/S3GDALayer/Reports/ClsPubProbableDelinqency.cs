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
    public class ClsPubProbableDelinqency : ClsPubDalReportBase
    {
        public List<ClsPubProbableDelinq> FunPubGetProbableDelinq(ClsPubProbDelinqParam DelinqPar)
        {
            List<ClsPubProbableDelinq> DelinqParam = new List<ClsPubProbableDelinq>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetProbableDelinq);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, DelinqPar.Company_Id);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, DelinqPar.Lob_Id);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID, DbType.Int32, DelinqPar.Location_Id);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDEMANDMONTH, DbType.String, DelinqPar.DemandMonth);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubProbableDelinq pddlnq = new ClsPubProbableDelinq();
                    pddlnq.Location_Name = StringParse(reader[ClsPubDALConstants.LOCATION_NAME]);
                    pddlnq.Customer_Name = StringParse(reader[ClsPubDALConstants.CUSTOMERNAME]);
                    pddlnq.PANum = StringParse(reader[ClsPubDALConstants.PANUM]);
                    pddlnq.SANum = StringParse(reader[ClsPubDALConstants.SANUM]);
                    pddlnq.Arrear_Due=DecimalParse(reader[ClsPubDALConstants.ARREARDUE]);
                    pddlnq.Current_Due=DecimalParse(reader[ClsPubDALConstants.CURRENTDUE]);
                    
                    DelinqParam.Add(pddlnq);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return DelinqParam;
        }
    }
}
