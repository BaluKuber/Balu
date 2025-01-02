#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Entity Master
/// Created By			: Swarna S
/// Created Date		: 08-Oct-2014
/// Purpose	            : 
/// <Program Summary>
#endregion

using System;
using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORG = S3GBusEntity.Origination;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using S3GBusEntity;
using System.Data.OracleClient;

namespace S3GDALayer.Origination
{
    namespace OrgMasterMgtServices
    {
        public class ClsPubTDSMaster
        {
            #region Intialize
            int intRowsAffected;
            ORG.OrgMasterMgtServices.S3G_ORG_TDS_MasterDataTable ObjTDSMaster_DAL;


            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubTDSMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion

            #region Creating New TDS
            public int FunPubCreateTDSInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_TDS_MasterDataTable, out string strTDSCode_Out)
            {
                string strTDSCode = "";
                try
                {

                    ObjTDSMaster_DAL = (ORG.OrgMasterMgtServices.S3G_ORG_TDS_MasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_TDS_MasterDataTable, SerMode, typeof(ORG.OrgMasterMgtServices.S3G_ORG_TDS_MasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (ORG.OrgMasterMgtServices.S3G_ORG_TDS_MasterRow ObjTDSMasterRow in ObjTDSMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertTDSDetails");
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjTDSMasterRow.Company_ID);
                        db.AddInParameter(command, "@Company_Type", DbType.Int32, ObjTDSMasterRow.Company_Type);
                        db.AddInParameter(command, "@Constitution", DbType.Int32, ObjTDSMasterRow.Constitution);
                        db.AddInParameter(command, "@Effective_From", DbType.DateTime, ObjTDSMasterRow.Effective_From);
                        db.AddInParameter(command, "@Tax_Law_Section", DbType.String, ObjTDSMasterRow.Tax_Law_Section);
                        db.AddInParameter(command, "@Tax_Description", DbType.String, ObjTDSMasterRow.Tax_Description);
                        db.AddInParameter(command, "@GL_Account", DbType.Int32, ObjTDSMasterRow.GL_Account);
                        db.AddInParameter(command, "@SL_Account", DbType.Int32, ObjTDSMasterRow.SL_Account);
                        db.AddInParameter(command, "@RatePercentage", DbType.Decimal, Convert.ToDecimal(ObjTDSMasterRow.RatePercentage));
                        db.AddInParameter(command, "@Tax", DbType.Decimal, Convert.ToDecimal(ObjTDSMasterRow.Tax));
                        db.AddInParameter(command, "@Surcharge", DbType.Decimal, Convert.ToDecimal(ObjTDSMasterRow.Surcharge));
                        db.AddInParameter(command, "@Cess", DbType.Decimal, Convert.ToDecimal(ObjTDSMasterRow.Cess));
                        db.AddInParameter(command, "@EdCess", DbType.Decimal, Convert.ToDecimal(ObjTDSMasterRow.EdCess));
                        db.AddInParameter(command, "@Threshold_Level", DbType.Decimal, Convert.ToDecimal(ObjTDSMasterRow.Threshold_Level));
                        db.AddInParameter(command, "@Gross_up", DbType.Boolean, ObjTDSMasterRow.Gross_up);
                        db.AddInParameter(command, "@Is_active", DbType.Boolean, ObjTDSMasterRow.Is_active);
                        db.AddInParameter(command, "@Res_Status", DbType.Int32, ObjTDSMasterRow.Res_Status);
                        db.AddInParameter(command, "@PAN_Applicability", DbType.Int32, ObjTDSMasterRow.PAN_Applicability);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjTDSMasterRow.Created_By);
                        db.AddOutParameter(command, "@Tax_ID", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, 4);
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                strTDSCode = (string)command.Parameters["@Tax_ID"].Value;
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
                            {
                                conn.Close();
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (intRowsAffected == 0)
                        intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                }
                strTDSCode_Out = strTDSCode;
                return intRowsAffected;
            }
            #endregion

            #region Modifiy TDS
            public int FunPubModifyTDSInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_TDS_MasterDataTable)
            {
                try
                {
                    ObjTDSMaster_DAL = (ORG.OrgMasterMgtServices.S3G_ORG_TDS_MasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_TDS_MasterDataTable, SerMode, typeof(ORG.OrgMasterMgtServices.S3G_ORG_TDS_MasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            foreach (ORG.OrgMasterMgtServices.S3G_ORG_TDS_MasterRow ObjTDSMasterRow in ObjTDSMaster_DAL.Rows)
                            {
                                DbCommand command = db.GetStoredProcCommand("S3G_ORG_UpdateTDSDetails");
                                db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjTDSMasterRow.Company_ID);
                                db.AddInParameter(command, "@Company_Type", DbType.Int32, ObjTDSMasterRow.Company_Type);
                                db.AddInParameter(command, "@Constitution", DbType.Int32, ObjTDSMasterRow.Constitution);
                                db.AddInParameter(command, "@Effective_From", DbType.DateTime, ObjTDSMasterRow.Effective_From);
                                db.AddInParameter(command, "@Tax_Law_Section", DbType.String, ObjTDSMasterRow.Tax_Law_Section);
                                db.AddInParameter(command, "@Tax_Description", DbType.String, ObjTDSMasterRow.Tax_Description);
                                db.AddInParameter(command, "@GL_Account", DbType.Int32, ObjTDSMasterRow.GL_Account);
                                db.AddInParameter(command, "@SL_Account", DbType.Int32, ObjTDSMasterRow.SL_Account);
                                db.AddInParameter(command, "@RatePercentage", DbType.Decimal, Convert.ToDecimal(ObjTDSMasterRow.RatePercentage));
                                db.AddInParameter(command, "@Tax", DbType.Decimal, Convert.ToDecimal(ObjTDSMasterRow.Tax));
                                db.AddInParameter(command, "@Surcharge", DbType.Decimal, Convert.ToDecimal(ObjTDSMasterRow.Surcharge));
                                db.AddInParameter(command, "@Cess", DbType.Decimal, Convert.ToDecimal(ObjTDSMasterRow.Cess));
                                db.AddInParameter(command, "@EdCess", DbType.Decimal, Convert.ToDecimal(ObjTDSMasterRow.EdCess));
                                db.AddInParameter(command, "@Threshold_Level", DbType.Decimal, Convert.ToDecimal(ObjTDSMasterRow.Threshold_Level));
                                db.AddInParameter(command, "@Gross_up", DbType.Boolean, ObjTDSMasterRow.Gross_up);
                                db.AddInParameter(command, "@Is_active", DbType.Boolean, ObjTDSMasterRow.Is_active);
                                db.AddInParameter(command, "@Res_Status", DbType.Int32, ObjTDSMasterRow.Res_Status);
                                db.AddInParameter(command, "@PAN_Applicability", DbType.Int32, ObjTDSMasterRow.PAN_Applicability);
                                db.AddInParameter(command, "@Created_By", DbType.Int32, ObjTDSMasterRow.Created_By);
                                db.AddInParameter(command, "@Tax_ID", DbType.Int32, ObjTDSMasterRow.Tax_ID);
                                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, 4);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value != 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                            }
                            trans.Commit();

                        }
                        catch (Exception ex)
                        {
                            if (intRowsAffected == 0)
                                intRowsAffected = 50;
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
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }

                return intRowsAffected;
            }
            #endregion

            #region Get TDS Details
         
            public DataSet FunPubQueryTDSDetails(int? intCompany_Id, int? intTDS_id, out bool blnTranExists)
            {
                DataSet dsTDSDetails = new DataSet();
                blnTranExists = false;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_ShowTDSDetails");
                    if (intCompany_Id.HasValue && intCompany_Id.Value != 0)
                    {
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, intCompany_Id);
                    }
                    if (intTDS_id.HasValue && intTDS_id.Value != 0)
                    {
                        db.AddInParameter(command, "@Tax_ID", DbType.Int32, intTDS_id);
                    }
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@RecordExists", DbType.Boolean, sizeof(Boolean));
                    //dsEntityDetails = db.ExecuteDataSet(command);

                    //dsEntityDetails = db.FunPubExecuteDataSet(command);

                    db.FunPubLoadDataSet(command, dsTDSDetails, "Table0");

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        throw new Exception("Error in Getting TDS details");
                    blnTranExists = Convert.ToBoolean(command.Parameters["@RecordExists"].Value);
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }

                return dsTDSDetails;
            }
            #endregion

        }
    }
}
