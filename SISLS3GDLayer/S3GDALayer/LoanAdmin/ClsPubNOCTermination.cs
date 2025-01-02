#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: LoanAdmin
/// Screen Name			:  NOC Termination DAL class
/// Created By			: Irsathameen K
/// Created Date		: 07-sep-2010
/// Purpose	            : Stpre Noc Termination Details

/// <Program Summary>
#endregion

using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using S3GBusEntity;

namespace S3GDALayer.LoanAdmin
{
    namespace LoanAdminMgtServices
    {
        public class ClsPubNOCTermination
        {
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_NOCTerminationDetailsDataTable ObjNOCTermination_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubNOCTermination()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateNocTerminationDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_NOCTerminationDataTable, out string strNOCNo)
            {
                try
                {
                    strNOCNo = "";
                    ObjNOCTermination_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_NOCTerminationDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_NOCTerminationDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_NOCTerminationDetailsDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_NOCTerminationDetailsRow ObjNOCTerminationRow in ObjNOCTermination_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertNOCTerminationDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjNOCTerminationRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjNOCTerminationRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjNOCTerminationRow.Branch_ID);                     
                        db.AddInParameter(command, "@NOC_Date", DbType.DateTime, ObjNOCTerminationRow.NOC_Date);
                        db.AddInParameter(command, "@PANum", DbType.String, ObjNOCTerminationRow.PANum);                       
                        db.AddInParameter(command, "@SANum", DbType.String, ObjNOCTerminationRow.SANum);                      
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjNOCTerminationRow.Customer_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjNOCTerminationRow.Created_By);
                        db.AddOutParameter(command, "@NOC_No", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                         using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
				db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else if ((int)command.Parameters["@ErrorCode"].Value < 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    if (intRowsAffected == -1)
                                        throw new Exception("Document Sequence no not-defined");
                                    if (intRowsAffected == -2)
                                        throw new Exception("Document Sequence no exceeds defined limit");
                                }
                                else
                                {                                    
                                    trans.Commit();
                                    strNOCNo = (string)command.Parameters["@NOC_No"].Value;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();
                            }
                            finally
                            {  conn.Close();   }
                        }
                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;
            }
        }
    }
}
