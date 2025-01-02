#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Scheduled Jobs
/// Screen Name			: Scheduled Jobs DAL Class
/// Created By			: Muni Kavitha
/// Created Date		: 
/// Purpose	            : DAL Class for Scheduled Jobs Methods
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
#endregion

#region Namespaces

using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
#endregion

namespace S3GDALayer.ScheduledJobs
{
    namespace ScheduledJobs
    {
        public class ClsPubScheduledJobs
        {
            #region Initialization
            int intErrorCode = 0;
            S3GBusEntity.ScheduledJobs.ScheduledJobMgtServices.S3G_SYSAD_ScheduledJobsDataTable ObjScheduledJobsDatatable_DAL = null;
            S3GBusEntity.ScheduledJobs.ScheduledJobMgtServices.S3G_SYSAD_ScheduledJobsRow objScheduledJobsRow = null;
            Database db;
            public ClsPubScheduledJobs()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }
            #endregion

            #region Create Scheduled Jobs
            /// <summary>
            /// Insert Scheduled Jobs
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjScheduledJobsDatatable"></param>
            /// <returns></returns>
            public int FunPubCreateScheduledJobs(SerializationMode SerMode, byte[] bytesObjScheduledJobsDatatable, out string strScheduleJobID)
            {
                strScheduleJobID = "";
                int intScheduleJobid = 0;
                try
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_SYSAD_InsertScheduledJobs");

                    ObjScheduledJobsDatatable_DAL = (S3GBusEntity.ScheduledJobs.ScheduledJobMgtServices.S3G_SYSAD_ScheduledJobsDataTable)ClsPubSerialize.DeSerialize(bytesObjScheduledJobsDatatable, SerMode, typeof(S3GBusEntity.ScheduledJobs.ScheduledJobMgtServices.S3G_SYSAD_ScheduledJobsDataTable));
                    objScheduledJobsRow = ObjScheduledJobsDatatable_DAL.Rows[0] as S3GBusEntity.ScheduledJobs.ScheduledJobMgtServices.S3G_SYSAD_ScheduledJobsRow;
                    db.AddInParameter(command, "@Location", DbType.String, objScheduledJobsRow.Location_Code);
                    db.AddInParameter(command, "@JobDescription", DbType.String, objScheduledJobsRow.JobDescription);
                    if(!objScheduledJobsRow.IsFrequencyNull())
                    db.AddInParameter(command, "@Frequency", DbType.Int32, Convert.ToInt32(objScheduledJobsRow.Frequency));
                    if(!objScheduledJobsRow.IsTime_In_MinsNull())
                    db.AddInParameter(command, "@TimeInMinutes", DbType.Int32, objScheduledJobsRow.Time_In_Mins);
                    db.AddInParameter(command, "@StartTime", DbType.String, objScheduledJobsRow.StartDateTime);
                    db.AddInParameter(command, "@Holiday", DbType.Boolean, objScheduledJobsRow.Holiday);
                    db.AddInParameter(command, "@Remarks", DbType.String, objScheduledJobsRow.Remarks);
                    db.AddInParameter(command, "@Job", DbType.Int32, Convert.ToInt32(objScheduledJobsRow.Schedule_Job));
                    db.AddInParameter(command, "@JobNature", DbType.Int32, Convert.ToInt32(objScheduledJobsRow.JobNature));
                    db.AddInParameter(command, "@User_ID", DbType.Int32, Convert.ToInt32(objScheduledJobsRow.Created_By));
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, objScheduledJobsRow.Company_ID);
                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, objScheduledJobsRow.Is_Active);
                    db.AddOutParameter(command, "@Schedule_Job_ID", DbType.Int64, sizeof(Int64));
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int64, sizeof(Int64));
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open(); 
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command, ref trans);


                            if (intScheduleJobid > 0)
                            {
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                            }
                            else
                            {
                                strScheduleJobID = Convert.ToString(command.Parameters["@Schedule_Job_ID"].Value);
                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (intErrorCode == 0)
                                intErrorCode = 50;
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            trans.Rollback();
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    intErrorCode = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                }
                return intErrorCode;



            }

            //public int FunPubExecuteScheduledJob(SerializationMode SerMode,

            #endregion
        }



    }
}
