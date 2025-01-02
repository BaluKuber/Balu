#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: ORG
/// Screen Name			: Global Parameter Creation DAL Class
/// Created By			: Kannan RC
/// Created Date		: 30-Jun-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 
/// Reason              : Loan Admin Module Developement
/// <Program Summary>
#endregion

#region Namespaces
using System;
using S3GDALayer.S3GAdminServices;
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
using System.Data.OracleClient;
#endregion

namespace S3GDALayer.LoanAdmin
{
    namespace LoanAdminMgtServices
    {
        public class ClsPubGlobalParameterSetup
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_lOANAD_InsGLOBALParametersDataTable ObjGlobalParameerIns_DAL;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetLOBDetailsDataTable ObjGetGlobalParameer_DAL;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetGlobalParameterSetupDataTable ObjGetGlobalParameter_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubGlobalParameterSetup()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Insert GP
            public int FunPubCreateGlobalParameter(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_GlobalParameterMasterDataTable)
            {
                try
                {
                    ObjGlobalParameerIns_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_lOANAD_InsGLOBALParametersDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_GlobalParameterMasterDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_lOANAD_InsGLOBALParametersDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_lOANAD_InsGLOBALParametersRow ObjGlobalParameterMasterRow in ObjGlobalParameerIns_DAL.Rows)
                    {
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertGlobalParameter");
                        //db.AddInParameter(command, "@COMPANY_ID", DbType.Int64, ObjGlobalParameterMasterRow.Company_ID);
                        //db.AddInParameter(command, "@LOB_ID", DbType.Boolean, ObjGlobalParameterMasterRow.LOB_ID);
                        //db.AddInParameter(command, "@Product_ID", DbType.Boolean, ObjGlobalParameterMasterRow.Product_ID);
                        //db.AddInParameter(command, "@Module_ID", DbType.Boolean, ObjGlobalParameterMasterRow.Module_ID);
                        //db.AddInParameter(command, "@Param_Set_Desc", DbType.String, ObjGlobalParameterMasterRow.Param_Set_Desc);
                        //db.AddInParameter(command, "@Param_Set_Date", DbType.DateTime, ObjGlobalParameterMasterRow.Param_Set_Date);
                        //db.AddInParameter(command, "@Txn_ID", DbType.Int32, ObjGlobalParameterMasterRow.Txn_ID);
                        //db.AddInParameter(command, "@CREATED_BY", DbType.Int32, ObjGlobalParameterMasterRow.CREATED_BY);
                        //db.AddInParameter(command, "@CREATED_ON", DbType.DateTime, ObjGlobalParameterMasterRow.CREATED_ON);
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param;
                            param = new OracleParameter("@XML_GlobalParamDeltails",
                                 OracleType.Clob, ObjGlobalParameterMasterRow.XML_GlobalParamDeltails.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, ObjGlobalParameterMasterRow.XML_GlobalParamDeltails);
                            command.Parameters.Add(param);

                            if (ObjGlobalParameterMasterRow.XML_ParamDeltails != "")
                            {
                                param = new OracleParameter("@XML_ParamDeltails",
                                 OracleType.Clob, ObjGlobalParameterMasterRow.XML_ParamDeltails.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                 DataRowVersion.Default, ObjGlobalParameterMasterRow.XML_ParamDeltails);
                                command.Parameters.Add(param);
                            }
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_GlobalParamDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_GlobalParamDeltails);
                            db.AddInParameter(command, "@XML_ParamDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_ParamDeltails);
                        }
                        /*
                        db.AddInParameter(command, "@XML_GapDaysLOBDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_GapDaysLOBDeltails);
                        db.AddInParameter(command, "@XML_GapDaysDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_GapDaysDeltails);
                        db.AddInParameter(command, "@XML_IncomeLOBDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_IncomeLOBDeltails);
                        db.AddInParameter(command, "@XML_IncomeDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_IncomeDeltails);
                        db.AddInParameter(command, "@XML_InsRecLOBDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_InsRecLOBDeltails);
                        db.AddInParameter(command, "@XML_InsRecDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_InsRecDeltails);
                        db.AddInParameter(command, "@XML_RevisionLOBDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_RevisionLOBDeltails);
                        db.AddInParameter(command, "@XML_RevisionDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_RevisionDeltails);
                        db.AddInParameter(command, "@XML_OverDueLOBDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_OverDueLOBDeltails);                                                
                        db.AddInParameter(command, "@XML_OverDueDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_OverDueDeltails);
                        db.AddInParameter(command, "@XML_ExtraDueDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_ExtraDueDeltails);
                        db.AddInParameter(command, "@XML_ExtraDueLOBDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_ExtraDueLOBDeltails);
                        */
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
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


                }
                catch (Exception exp)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;
            }


            #endregion

            #region Update GP
            public int FunPubModifyGlobalParameter(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_GlobalParameterMasterDataTable)
            {
                try
                {
                    ObjGlobalParameerIns_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_lOANAD_InsGLOBALParametersDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_GlobalParameterMasterDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_lOANAD_InsGLOBALParametersDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_lOANAD_InsGLOBALParametersRow ObjGlobalParameterMasterRow in ObjGlobalParameerIns_DAL.Rows)
                    {
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_UpdateGlobalParameter");

                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param;
                            param = new OracleParameter("@XML_GlobalParamDeltails",
                                 OracleType.Clob, ObjGlobalParameterMasterRow.XML_GlobalParamDeltails.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, ObjGlobalParameterMasterRow.XML_GlobalParamDeltails);
                            command.Parameters.Add(param);

                            if (ObjGlobalParameterMasterRow.XML_ParamDeltails != "")
                            {
                                param = new OracleParameter("@XML_ParamDeltails",
                                 OracleType.Clob, ObjGlobalParameterMasterRow.XML_ParamDeltails.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                 DataRowVersion.Default, ObjGlobalParameterMasterRow.XML_ParamDeltails);
                                command.Parameters.Add(param);
                            }
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_GlobalParamDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_GlobalParamDeltails);
                            db.AddInParameter(command, "@XML_ParamDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_ParamDeltails);
                        }
                        //db.AddInParameter(command, "@XML_GapDaysLOBDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_GapDaysLOBDeltails);
                        //db.AddInParameter(command, "@XML_GapDaysDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_GapDaysDeltails);
                        //db.AddInParameter(command, "@XML_IncomeLOBDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_IncomeLOBDeltails);
                        //db.AddInParameter(command, "@XML_IncomeDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_IncomeDeltails);
                        //db.AddInParameter(command, "@XML_InsRecLOBDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_InsRecLOBDeltails);
                        //db.AddInParameter(command, "@XML_InsRecDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_InsRecDeltails);
                        //db.AddInParameter(command, "@XML_RevisionLOBDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_RevisionLOBDeltails);
                        //db.AddInParameter(command, "@XML_RevisionDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_RevisionDeltails);
                        //db.AddInParameter(command, "@XML_OverDueLOBDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_OverDueLOBDeltails);
                        //db.AddInParameter(command, "@XML_OverDueDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_OverDueDeltails);
                        //db.AddInParameter(command, "@XML_ExtraDueDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_ExtraDueDeltails);
                        //db.AddInParameter(command, "@XML_ExtraDueLOBDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_ExtraDueLOBDeltails);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
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


                }
                catch (Exception exp)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;
            }


            public S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetLOBDetailsDataTable FunPubGetGlobalLOBName(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_GlobalLOBNameDataTable)
            {
                S3GBusEntity.LoanAdmin.LoanAdminMgtServices dsGlobalLOBName = new S3GBusEntity.LoanAdmin.LoanAdminMgtServices();
                ObjGetGlobalParameer_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetLOBDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_GlobalLOBNameDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetLOBDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetLOBDetails");
                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetLOBDetailsRow ObjGlobalNameRow in ObjGetGlobalParameer_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjGlobalNameRow.Company_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjGlobalNameRow.User_ID);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjGlobalNameRow.Is_Active);
                        db.FunPubLoadDataSet(command, dsGlobalLOBName, dsGlobalLOBName.S3G_LOANAD_GetLOBDetails.TableName);
                    }

                }
                catch (Exception exp)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsGlobalLOBName.S3G_LOANAD_GetLOBDetails;

            }

            #endregion

            #region GetGPS
            public S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetGlobalParameterSetupDataTable FunPubGetGlobalParam(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_GlobalParamDataTable)
            {
                S3GBusEntity.LoanAdmin.LoanAdminMgtServices dsGlobalLOBName = new S3GBusEntity.LoanAdmin.LoanAdminMgtServices();
                ObjGetGlobalParameter_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetGlobalParameterSetupDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_GlobalParamDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetGlobalParameterSetupDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetGlobalParameterSetup");
                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetGlobalParameterSetupRow ObjGlobalParamNameRow in ObjGetGlobalParameter_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjGlobalParamNameRow.Company_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjGlobalParamNameRow.User_ID);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjGlobalParamNameRow.Is_Active);
                        //  db.AddInParameter(command, "@Param_Set_ID", DbType.Int32, ObjGlobalParamNameRow.Param_Set_ID);
                        db.FunPubLoadDataSet(command, dsGlobalLOBName, dsGlobalLOBName.S3G_LOANAD_GetGlobalParameterSetup.TableName);
                    }

                }
                catch (Exception exp)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsGlobalLOBName.S3G_LOANAD_GetGlobalParameterSetup;

            }
            #endregion

        }
    }
}
