using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using System.Data.Common;
using S3GBusEntity.Reports;
using S3GDALayer.Constants;
using S3GBusEntity;

namespace S3GDALayer.Reports
{
   public class ClsPubDPDReport : ClsPubDalReportBase
    {
       public List<ClsPubDPDDetails> FunPubGetDPDBucketDetails(int LOBID, int BUCKETCOUNT) 
        {

            List<ClsPubDPDDetails> DPDDetails = new List<ClsPubDPDDetails>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETBUCKETCOUNT);
                db.AddInParameter(command,ClsPubDALConstants.INPUTPARAMLOBID,DbType.Int32,LOBID);
                db.AddInParameter(command,ClsPubDALConstants.INPUTPARAMBUCKETCOUNT, DbType.Int32,BUCKETCOUNT);
                IDataReader reader = db.ExecuteReader(command);
                while (reader.Read())
                {
                   ClsPubDPDDetails DPDDetail = new ClsPubDPDDetails();
                   DPDDetail.Bucket=IntParse(reader[ClsPubDALConstants.BUCKETCOUNT]);
                   DPDDetail.DaysFrom=IntParse(reader[ClsPubDALConstants.BUCKETFROM]);
                   DPDDetail.DaysTo=IntParse(reader[ClsPubDALConstants.BUCKETTO]);
                   DPDDetails.Add(DPDDetail); 
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex);
            }
            return DPDDetails;
        }

    }
}
