
using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml.Serialization;
using System.Data.Common;
using System.Data;

namespace S3GDALayer.LegalRepossession
{
    namespace LegalAndRepossessionMgtServices
    {
        public class ClsPubRepossessionRelease : ClsPubDalLegalRepossessionBase
        {
            int intRowsAffected = 0;
            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionReleaseDataTable objS3g_LR_RepoReleaseTable;
            public int FunPubCreateRepoRelease(SerializationMode SerMode, byte[] byteobjS3G_LR_RepoReleaseDataTable, out string strRepoReleaseNo, string[] strCompanyLOBBarnch)
            {
                strRepoReleaseNo = string.Empty;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    objS3g_LR_RepoReleaseTable = (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionReleaseDataTable)ClsPubSerialize.DeSerialize(byteobjS3G_LR_RepoReleaseDataTable, SerMode, typeof(S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionReleaseDataTable));
                    foreach (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionReleaseRow objS3G_LR_LegalRepoReleaseRow in objS3g_LR_RepoReleaseTable.Rows)
                    {
                        DbCommand dbCmd = db.GetStoredProcCommand("S3G_LR_InsertRepossessionRelease");
                        db.AddInParameter(dbCmd, "@Release_ID", DbType.Int32, objS3G_LR_LegalRepoReleaseRow.Repossession_Release_ID);
                        db.AddInParameter(dbCmd, "@Company_ID", DbType.String, strCompanyLOBBarnch[0]);
                        db.AddInParameter(dbCmd, "@LOB_ID", DbType.String, strCompanyLOBBarnch[1]);
                        db.AddInParameter(dbCmd, "@Location_ID", DbType.String, strCompanyLOBBarnch[2]);

                        db.AddInParameter(dbCmd, "@Repossession_Release_Date", DbType.DateTime, objS3G_LR_LegalRepoReleaseRow.Repossession_Release_Date);
                        db.AddInParameter(dbCmd, "@LRN_ID", DbType.Int32, objS3G_LR_LegalRepoReleaseRow.LRN_ID);
                        db.AddInParameter(dbCmd, "@LRN_No", DbType.String, objS3G_LR_LegalRepoReleaseRow.LRN_No);
                        db.AddInParameter(dbCmd, "@Repossession_ID", DbType.Int32, objS3G_LR_LegalRepoReleaseRow.Repossession_ID);
                        db.AddInParameter(dbCmd, "@Repossession_Docket_No", DbType.String, objS3G_LR_LegalRepoReleaseRow.Repossession_Docket_No);
                        db.AddInParameter(dbCmd, "@Released_By", DbType.String, objS3G_LR_LegalRepoReleaseRow.Released_By);
                        db.AddInParameter(dbCmd, "@Created_By", DbType.Int32, objS3G_LR_LegalRepoReleaseRow.Created_By);
                        db.AddOutParameter(dbCmd, "@Repossession_Release_No", DbType.String, 50);
                        db.AddOutParameter(dbCmd, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //string ErrorCode = dbCmd.Parameters[dbCmd.Parameters.Count - 1].ParameterName;
                        
                        using (DbConnection con = db.CreateConnection())
                        {
                            con.Open();
                            DbTransaction trans = con.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(dbCmd, trans);//Modified by Ponnurajesh on 24/3/2012 for Oracle conversion
                                db.FunPubExecuteNonQuery(dbCmd, ref trans);
                                if ((int)dbCmd.Parameters["@ErrorCode"].Value != 0)
                                {
                                    intRowsAffected = (int)dbCmd.Parameters["@ErrorCode"].Value;
                                    trans.Rollback();
                                }
                                else
                                {
                                    strRepoReleaseNo = dbCmd.Parameters["@Repossession_Release_No"].Value.ToString();
                                    trans.Commit();
                                }


                            }
                            catch (Exception ex)
                            {
                                if (dbCmd.Parameters["@ErrorCode"].Value != null)
                                {
                                    intRowsAffected = (int)dbCmd.Parameters["@ErrorCode"].Value;
                                }
                                else if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();

                            }
                            finally
                            {
                                con.Close();
                            }

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
