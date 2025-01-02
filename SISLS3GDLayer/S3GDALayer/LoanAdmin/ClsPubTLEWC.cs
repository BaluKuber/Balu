#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: ORG
/// Screen Name			: PRDDT Creation DAL Class
/// Created By			: Kannan RC
/// Created Date		: 10-Sep-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 
/// Reason              : Orgination Module Developement
/// <Program Summary>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Data.OracleClient;
#endregion


namespace S3GDALayer.LoanAdmin
{
    namespace LoanAdminMgtServices
    {
        public class ClsPubTLEWC
        {
            #region
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOAND_InsTLEWCTopupDataTable ObjIns_Datatable_DAL;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOAND_InsWCTopupDataTable ObjInsWC_Datatable_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubTLEWC()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion

            #region Create
            public int FunPubCreateTLEWC(SerializationMode SerMode, byte[] bytesobjS3G_SYSAD_TLEWCDataTable, out string strTLENumber_Out)
            {
                strTLENumber_Out = string.Empty;
                try
                {

                    ObjIns_Datatable_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOAND_InsTLEWCTopupDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_SYSAD_TLEWCDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOAND_InsTLEWCTopupDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOAND_InsTLEWCTopupRow ObjTLEWCMasterRow in ObjIns_Datatable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertTLEWCGLSL");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTLEWCMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjTLEWCMasterRow.LOB_ID);
                        //db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjTLEWCMasterRow.Branch_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjTLEWCMasterRow.Branch_ID);
                        db.AddInParameter(command, "@PANum", DbType.String, ObjTLEWCMasterRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, ObjTLEWCMasterRow.SANum);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjTLEWCMasterRow.Customer_ID);
                        db.AddInParameter(command, "@TLE_WC_Date", DbType.DateTime, ObjTLEWCMasterRow.TLE_WC_Date);
                        db.AddInParameter(command, "@Topup_Status_Type_Code", DbType.Int32, ObjTLEWCMasterRow.Topup_Status_Type_Code);
                        db.AddInParameter(command, "@Topup_Status_Code", DbType.Int32, ObjTLEWCMasterRow.Topup_Status_Code);
                        db.AddInParameter(command, "@Current_Finance_Required", DbType.Decimal, ObjTLEWCMasterRow.Current_Finance_Required);
                        db.AddInParameter(command, "@Start_Instalment", DbType.Int32, ObjTLEWCMasterRow.Start_Instalment);
                        db.AddInParameter(command, "@Instalment_Amount", DbType.Decimal, ObjTLEWCMasterRow.Instalment_Amount);
                        db.AddInParameter(command, "@Account_Link_Key", DbType.Int32, ObjTLEWCMasterRow.Account_Link_Key);
                        db.AddInParameter(command, "@CREATED_BY", DbType.Int32, ObjTLEWCMasterRow.Created_By);
                        db.AddInParameter(command, "@CREATED_ON", DbType.DateTime, ObjTLEWCMasterRow.Created_On);
                        db.AddInParameter(command, "@TXN_Id", DbType.Int32, ObjTLEWCMasterRow.Txn_ID);
                        //db.AddInParameter(command, "@XML_TopupDeltails", DbType.String, ObjTLEWCMasterRow.XML_TopupDeltails);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_TopupDeltails", OracleType.Clob,
                                ObjTLEWCMasterRow.XML_TopupDeltails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjTLEWCMasterRow.XML_TopupDeltails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_TopupDeltails", DbType.String,
                                ObjTLEWCMasterRow.XML_TopupDeltails);
                        }
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@TLEWC_No", DbType.String, 100);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                 db.FunPubExecuteNonQuery(command,ref trans);
                                strTLENumber_Out = (string)command.Parameters["@TLEWC_No"].Value;
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
                            catch (Exception exp)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
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
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;

            }


            #endregion

            #region Update
            public int FunPubUpdateTLEWC(SerializationMode SerMode, byte[] bytesobjS3G_SYSAD_TLEWCDataTable)
            {
                //strTLENumber_Out = string.Empty;
                try
                {

                    ObjIns_Datatable_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOAND_InsTLEWCTopupDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_SYSAD_TLEWCDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOAND_InsTLEWCTopupDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOAND_InsTLEWCTopupRow ObjTLEWCMasterRow in ObjIns_Datatable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_UpdateTLEWC");
                        db.AddInParameter(command, "@TLEWCTopup_ID", DbType.Int32, ObjTLEWCMasterRow.TLEWCTopup_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTLEWCMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjTLEWCMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjTLEWCMasterRow.Branch_ID);
                        //db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjTLEWCMasterRow.Branch_ID);
                        db.AddInParameter(command, "@PANum", DbType.String, ObjTLEWCMasterRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, ObjTLEWCMasterRow.SANum);
                        //db.AddInParameter(command, "@TLE_WC_No", DbType.String, ObjTLEWCMasterRow.TLEWC);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjTLEWCMasterRow.Customer_ID);
                        db.AddInParameter(command, "@TLE_WC_Date", DbType.DateTime, ObjTLEWCMasterRow.TLE_WC_Date);
                        db.AddInParameter(command, "@Topup_Status_Type_Code", DbType.Int32, ObjTLEWCMasterRow.Topup_Status_Type_Code);
                        db.AddInParameter(command, "@Topup_Status_Code", DbType.Int32, ObjTLEWCMasterRow.Topup_Status_Code);
                        db.AddInParameter(command, "@Current_Finance_Required", DbType.Decimal, ObjTLEWCMasterRow.Current_Finance_Required);
                        db.AddInParameter(command, "@Start_Instalment", DbType.Int32, ObjTLEWCMasterRow.Start_Instalment);
                        db.AddInParameter(command, "@Instalment_Amount", DbType.Decimal, ObjTLEWCMasterRow.Instalment_Amount);
                        db.AddInParameter(command, "@Account_Link_Key", DbType.Int32, ObjTLEWCMasterRow.Account_Link_Key);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjTLEWCMasterRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjTLEWCMasterRow.Modified_On);
                        db.AddInParameter(command, "@TXN_Id", DbType.Int32, ObjTLEWCMasterRow.Txn_ID);
                        //db.AddInParameter(command, "@XML_TopupDeltails", DbType.String, ObjTLEWCMasterRow.XML_TopupDeltails);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_TopupDeltails", OracleType.Clob,
                                ObjTLEWCMasterRow.XML_TopupDeltails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjTLEWCMasterRow.XML_TopupDeltails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_TopupDeltails", DbType.String,
                                ObjTLEWCMasterRow.XML_TopupDeltails);
                        }
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));


                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                 db.FunPubExecuteNonQuery(command,ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else
                                {
                                    trans.Commit();
                                }

                            }
                            catch (Exception exp)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
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
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;

            }
            #endregion

            #region CAncel Topup
            public int FunPubCancelTopupTLEWC(int intTLEWCID, int intCompanyId, int intUserId)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_CancelingTopUp");
                    db.AddInParameter(command, "@TLEWCTopup_ID", DbType.Int32, intTLEWCID);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@User_ID", DbType.Int32, intUserId);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                             db.FunPubExecuteNonQuery(command,ref trans);
                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            {
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                            }
                            else
                            {
                                trans.Commit();
                            }
                        }
                        catch (Exception exp)
                        {
                            if (intRowsAffected == 0)
                                intRowsAffected = 50;
                             ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                            trans.Rollback();
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;
            }
            #endregion

            #region Create WC
            public int FunPubCreateWC(SerializationMode SerMode, byte[] bytesobjS3G_SYSAD_WCDataTable, out string strTLENumber_Out)
            {
                strTLENumber_Out = string.Empty;
                try
                {

                    ObjInsWC_Datatable_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOAND_InsWCTopupDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_SYSAD_WCDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOAND_InsWCTopupDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOAND_InsWCTopupRow ObjTLEWCMasterRow in ObjInsWC_Datatable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertTLEWC");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTLEWCMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjTLEWCMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjTLEWCMasterRow.Branch_ID);
                        db.AddInParameter(command, "@PANum", DbType.String, ObjTLEWCMasterRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, ObjTLEWCMasterRow.SANum);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjTLEWCMasterRow.Customer_ID);
                        db.AddInParameter(command, "@TLE_WC_Date", DbType.DateTime, ObjTLEWCMasterRow.TLE_WC_Date);
                        db.AddInParameter(command, "@Topup_Status_Type_Code", DbType.Int32, ObjTLEWCMasterRow.Topup_Status_Type_Code);
                        db.AddInParameter(command, "@Topup_Status_Code", DbType.Int32, ObjTLEWCMasterRow.Topup_Status_Code);
                        db.AddInParameter(command, "@Current_Finance_Required", DbType.Int32, ObjTLEWCMasterRow.Current_Finance_Required);
                        db.AddInParameter(command, "@Start_Instalment", DbType.Int32, ObjTLEWCMasterRow.Start_Instalment);
                        db.AddInParameter(command, "@Instalment_Amount", DbType.Decimal, ObjTLEWCMasterRow.Instalment_Amount);
                        db.AddInParameter(command, "@Account_Link_Key", DbType.Int32, ObjTLEWCMasterRow.Account_Link_Key);
                        db.AddInParameter(command, "@CREATED_BY", DbType.Int32, ObjTLEWCMasterRow.Created_By);
                        db.AddInParameter(command, "@CREATED_ON", DbType.DateTime, ObjTLEWCMasterRow.Created_On);
                        db.AddInParameter(command, "@TXN_Id", DbType.Int32, ObjTLEWCMasterRow.Txn_ID);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@TLEWC_No", DbType.String, 100);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                 db.FunPubExecuteNonQuery(command,ref trans);
                                strTLENumber_Out = (string)command.Parameters["@TLEWC_No"].Value;
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
                            catch (Exception exp)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
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
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;

            }
            #endregion

            #region UpdateWC
            public int FunPubUpdateWC(SerializationMode SerMode, byte[] bytesobjS3G_SYSAD_WCDataTable)
            {
                //strTLENumber_Out = string.Empty;
                try
                {

                    ObjInsWC_Datatable_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOAND_InsWCTopupDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_SYSAD_WCDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOAND_InsWCTopupDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOAND_InsWCTopupRow ObjTLEWCMasterRow in ObjInsWC_Datatable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_UpdateWC");
                        db.AddInParameter(command, "@TLEWCTopup_ID", DbType.Int32, ObjTLEWCMasterRow.TLEWCTopup_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTLEWCMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjTLEWCMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjTLEWCMasterRow.Branch_ID);
                        db.AddInParameter(command, "@PANum", DbType.String, ObjTLEWCMasterRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, ObjTLEWCMasterRow.SANum);
                        //db.AddInParameter(command, "@TLE_WC_No", DbType.String, ObjTLEWCMasterRow.TLEWC);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjTLEWCMasterRow.Customer_ID);
                        db.AddInParameter(command, "@TLE_WC_Date", DbType.DateTime, ObjTLEWCMasterRow.TLE_WC_Date);
                        db.AddInParameter(command, "@Topup_Status_Type_Code", DbType.Int32, ObjTLEWCMasterRow.Topup_Status_Type_Code);
                        db.AddInParameter(command, "@Topup_Status_Code", DbType.Int32, ObjTLEWCMasterRow.Topup_Status_Code);
                        db.AddInParameter(command, "@Current_Finance_Required", DbType.Int32, ObjTLEWCMasterRow.Current_Finance_Required);
                        db.AddInParameter(command, "@Start_Instalment", DbType.Int32, ObjTLEWCMasterRow.Start_Instalment);
                        db.AddInParameter(command, "@Instalment_Amount", DbType.Decimal, ObjTLEWCMasterRow.Instalment_Amount);
                        db.AddInParameter(command, "@Account_Link_Key", DbType.Int32, ObjTLEWCMasterRow.Account_Link_Key);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjTLEWCMasterRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjTLEWCMasterRow.Modified_On);
                        db.AddInParameter(command, "@TXN_Id", DbType.Int32, ObjTLEWCMasterRow.Txn_ID);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));


                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                 db.FunPubExecuteNonQuery(command,ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else
                                {
                                    trans.Commit();
                                }

                            }
                            catch (Exception exp)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
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
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;

            }
            #endregion

        }
    }
}
