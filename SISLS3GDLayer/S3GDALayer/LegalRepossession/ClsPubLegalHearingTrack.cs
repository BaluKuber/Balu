
using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml.Serialization;
using System.Data.Common;
using System.Data ;


namespace S3GDALayer.LegalRepossession
{
    namespace LegalAndRepossessionMgtServices
    {
        public class ClsPubLegalHearingTrack : ClsPubDalLegalRepossessionBase
        {
            int intRowsAffected = 0;
            S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_LegalHearingTrackDataTable objS3g_LR_LegalHearingTrackDataTable;
            #region Insert
            public int FunPubCreateLegalHearingTrack(SerializationMode SerMode, byte[] byteobjS3G_LR_LegalHearingTrackDataTable, out string strLHRNo)
            {
                strLHRNo =string.Empty ;
                try
                {
                    //Database db= DatabaseFactory .CreateDatabase ("S3GconnectionString");
                    objS3g_LR_LegalHearingTrackDataTable = (S3GBusEntity .LegalRepossession .LegalRepossessionMgtServices .S3G_LR_LegalHearingTrackDataTable )ClsPubSerialize .DeSerialize(byteobjS3G_LR_LegalHearingTrackDataTable ,SerMode ,typeof (S3GBusEntity .LegalRepossession .LegalRepossessionMgtServices .S3G_LR_LegalHearingTrackDataTable ));
                    foreach (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_LegalHearingTrackRow objS3G_LR_LegalHearingTrackRow in objS3g_LR_LegalHearingTrackDataTable.Rows)
                    {
                        DbCommand dbCmd = db.GetStoredProcCommand("S3G_LR_InsertLegalHearingTrack");
                        db.AddInParameter(dbCmd, "@LHR_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.LHR_ID);
                        db.AddInParameter(dbCmd, "@Company_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.Company_ID);
                        db.AddInParameter(dbCmd, "@LOB_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.LOB_ID);
                        db.AddInParameter(dbCmd, "@Location_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.Branch_ID);
                        db.AddInParameter(dbCmd, "@Customer_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.Customer_ID);
                        db.AddInParameter(dbCmd, "@Advocate_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.Advocate_ID);
                        db.AddInParameter(dbCmd, "@LRN_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.LRN_ID);
                        db.AddInParameter(dbCmd, "@LRN_No", DbType.String, objS3G_LR_LegalHearingTrackRow.LRN_No);
                        db.AddInParameter(dbCmd, "@LHR_Date", DbType.DateTime, objS3G_LR_LegalHearingTrackRow.LHR_Date);
                        db.AddInParameter(dbCmd, "@PANum", DbType.String, objS3G_LR_LegalHearingTrackRow.PANum);
                        if (!objS3G_LR_LegalHearingTrackRow.IsSANumNull())
                        {
                            db.AddInParameter(dbCmd, "@SANum", DbType.String, objS3G_LR_LegalHearingTrackRow.SANum);
                        }
                        db.AddInParameter(dbCmd, "@Case_No", DbType.String, objS3G_LR_LegalHearingTrackRow.Case_No);
                        db.AddInParameter(dbCmd, "@Court_Details", DbType.String, objS3G_LR_LegalHearingTrackRow.Court_Details);
                        db.AddInParameter(dbCmd, "@Case_Details", DbType.String, objS3G_LR_LegalHearingTrackRow.Case_Details);
                        //if (!objS3G_LR_LegalHearingTrackRow.IsClosure_DateNull())
                        //{
                        //    db.AddInParameter(dbCmd, "@Closure_Date", DbType.DateTime, objS3G_LR_LegalHearingTrackRow.Closure_Date);
                        //}
                        db.AddInParameter(dbCmd, "@ClosureFlag", DbType.Int32, objS3G_LR_LegalHearingTrackRow.ClosureFlag);
                        db.AddInParameter(dbCmd, "@Closure_Date", DbType.DateTime, objS3G_LR_LegalHearingTrackRow.Closure_Date);
                        
                        if (!objS3G_LR_LegalHearingTrackRow.IsDecreeNull())
                        {
                            db.AddInParameter(dbCmd, "@Decree", DbType.String, objS3G_LR_LegalHearingTrackRow.Decree);

                        }
                        db.AddInParameter(dbCmd, "@Remarks", DbType.String, objS3G_LR_LegalHearingTrackRow.Remarks);
                        if (!objS3G_LR_LegalHearingTrackRow.IsClosure_RemarksNull())
                        {
                            db.AddInParameter(dbCmd, "@Closure_Remarks", DbType.String, objS3G_LR_LegalHearingTrackRow.Closure_Remarks);
                        }
                        db.AddInParameter(dbCmd, "@XmlActivityDetails", DbType.String, objS3G_LR_LegalHearingTrackRow.XmlActivityDetails);
                        db.AddInParameter(dbCmd, "@User_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.Created_By);
                        db.AddInParameter(dbCmd, "@Is_Active", DbType.Boolean, objS3G_LR_LegalHearingTrackRow.Is_Active);
                        db.AddOutParameter(dbCmd, "@LHR_No", DbType.String, 50);
                        db.AddOutParameter(dbCmd, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //string ErrorCode = dbCmd.Parameters[dbCmd.Parameters.Count - 1].ParameterName;
                        //if (enumDBType !=S3GDALDBType .SQL  ) 
                        //{
                        //    ClsPubCommonDB.FunReplaceOracleParameter(ref dbCmd, enumDBType); 
                        //    ClsPubCommonDB.AddCursorOutParameter(ref dbCmd, db.ConnectionString);
                        //} 

                        using (DbConnection con = db.CreateConnection())
                        {
                            con.Open();
                            DbTransaction trans = con.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(dbCmd, trans);
                                //Modified by Ponnurajesh on 21/Mar/2012 for oracle conversion
                                db.FunPubExecuteNonQuery(dbCmd, ref trans);
                                if ((int)dbCmd.Parameters["@ErrorCode"].Value != 0)
                                {
                                    intRowsAffected = (int)dbCmd.Parameters["@ErrorCode"].Value;
                                    trans.Rollback();
                                }
                                else
                                {
                                    strLHRNo = dbCmd.Parameters["@LHR_No"].Value.ToString();
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
                                con.Close ();
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
            #endregion
            #region Modify
            public int FunPubModifyLegalHearingTrack(SerializationMode SerMode, byte[] byteobjS3G_LR_LegalHearingTrackDataTable)
            {
                try
                {
                    //Database  db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    objS3g_LR_LegalHearingTrackDataTable = (S3GBusEntity .LegalRepossession .LegalRepossessionMgtServices .S3G_LR_LegalHearingTrackDataTable )ClsPubSerialize .DeSerialize(byteobjS3G_LR_LegalHearingTrackDataTable ,SerMode ,typeof (S3GBusEntity .LegalRepossession .LegalRepossessionMgtServices .S3G_LR_LegalHearingTrackDataTable ));
                    foreach (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_LegalHearingTrackRow objS3G_LR_LegalHearingTrackRow in objS3g_LR_LegalHearingTrackDataTable.Rows)
                    {
                        
                        DbCommand dbCmd = db.GetStoredProcCommand("S3G_LR_UpdateLegalHearingTrack");
                        
                        db.AddInParameter(dbCmd, "@LHR_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.LHR_ID);
                        db.AddInParameter(dbCmd, "@Company_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.Company_ID);
                        db.AddInParameter(dbCmd, "@LOB_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.LOB_ID);
                        db.AddInParameter(dbCmd, "@Location_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.Branch_ID);
                        db.AddInParameter(dbCmd, "@Customer_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.Customer_ID);
                        db.AddInParameter(dbCmd, "@Advocate_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.Advocate_ID);
                        db.AddInParameter(dbCmd, "@LRN_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.LRN_ID);
                        db.AddInParameter(dbCmd, "@LRN_No", DbType.String, objS3G_LR_LegalHearingTrackRow.LRN_No);
                        db.AddInParameter(dbCmd, "@LHR_Date", DbType.DateTime, objS3G_LR_LegalHearingTrackRow.LHR_Date);
                        
                        db.AddInParameter(dbCmd, "@PANum", DbType.String, objS3G_LR_LegalHearingTrackRow.PANum);
                        if (!objS3G_LR_LegalHearingTrackRow.IsSANumNull())
                        {
                            db.AddInParameter(dbCmd, "@SANum", DbType.String, objS3G_LR_LegalHearingTrackRow.SANum);
                        }
                        db.AddInParameter(dbCmd, "@Case_No", DbType.String, objS3G_LR_LegalHearingTrackRow.Case_No);
                        db.AddInParameter(dbCmd, "@Court_Details", DbType.String, objS3G_LR_LegalHearingTrackRow.Court_Details);
                        db.AddInParameter(dbCmd, "@Case_Details", DbType.String, objS3G_LR_LegalHearingTrackRow.Case_Details);
                        //if (!objS3G_LR_LegalHearingTrackRow.IsClosure_DateNull())
                        //{
                        db.AddInParameter(dbCmd, "@ClosureFlag", DbType.Int32, objS3G_LR_LegalHearingTrackRow.ClosureFlag);
                        db.AddInParameter(dbCmd, "@Closure_Date", DbType.DateTime, objS3G_LR_LegalHearingTrackRow.Closure_Date);
                        //}
                        if (!objS3G_LR_LegalHearingTrackRow.IsDecreeNull())
                        {
                            db.AddInParameter(dbCmd, "@Decree", DbType.String, objS3G_LR_LegalHearingTrackRow.Decree);

                        }
                        db.AddInParameter(dbCmd, "@Remarks", DbType.String, objS3G_LR_LegalHearingTrackRow.Remarks);
                        if (!objS3G_LR_LegalHearingTrackRow.IsClosure_RemarksNull())
                        {
                            db.AddInParameter(dbCmd, "@Closure_Remarks", DbType.String, objS3G_LR_LegalHearingTrackRow.Closure_Remarks);
                        }
                        db.AddInParameter(dbCmd, "@XmlActivityDetails", DbType.String, objS3G_LR_LegalHearingTrackRow.XmlActivityDetails);
                        //Modified by Ponnurajesh on 21/Mar/2012 for oracle conversion
                        //if (enumDBType != S3GDALDBType.SQL) 
                        //{
                        //    dbCmd.Parameters["@XmlActivityDetails"].Value = ClsPubCommonDB.FunPubGetFormatedXML(objS3G_LR_LegalHearingTrackRow.XmlActivityDetails);
                        //    ClsPubCommonDB.FunReplaceOracleParameter(ref dbCmd, enumDBType); 
                        //} 
                        db.AddInParameter(dbCmd, "@User_ID", DbType.Int32, objS3G_LR_LegalHearingTrackRow.Created_By);
                        
                        db.AddInParameter(dbCmd, "@Is_Active", DbType.Boolean, objS3G_LR_LegalHearingTrackRow.Is_Active);
                        db.AddInParameter(dbCmd, "@LHR_No", DbType.String, objS3G_LR_LegalHearingTrackRow.LHR_No);
                        db.AddInParameter(dbCmd, "@DeleteKey", DbType.Int32, objS3G_LR_LegalHearingTrackRow.DeleteKey);

                        db.AddOutParameter(dbCmd, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        
                        using (DbConnection con = db.CreateConnection())
                        {
                            con.Open();
                            DbTransaction trans = con.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery (dbCmd, trans);
                                //Modified by Ponnurajesh on 21/Mar/2012 for oracle conversion
                                db.FunPubExecuteNonQuery(dbCmd, ref trans);
                                if ((int)dbCmd.Parameters["@ErrorCode"].Value != 0)
                                {
                                    intRowsAffected = (int)dbCmd.Parameters["@ErrorCode"].Value;
                                    trans.Rollback();
                                }
                                else
                                {
                                    //strLHRNo = dbCmd.Parameters["@LHR_No"].Value.ToString();
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
                                con.Close ();
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

             
            #endregion
            

        }


    }
}
