#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: ORG
/// Screen Name			: Casfflow Creation DAL Class
/// Created By			: Kannan RC
/// Created Date		: 01-Jun-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 
/// Reason              : Orgination Module Developement
/// <Program Summary>
#endregion

#region Namespaces
using System;
using S3GDALayer.S3GAdminServices;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
#endregion

namespace S3GDALayer.Origination
{

    namespace CashflowMgtServices
    {
        public class ClsPubCashflowMaster
        {
            #region Initialization
            int intRowsAffected;
            int intRowsMapping;
            int intMasterID;
            int DNCNo;
            //  S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_CashflowFlagListDataTable ObjCashflowFlagList_DAL;
            S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GLSLCodeListDataTable ObjGLSLCodeList_DAL;
            S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetProgramDataTable ObjGetProgramMaster_DAL;
            S3GBusEntity.Origination.CashflowMgtServices.S3G_Status_LookUPDataTable ObjCashflowType_Datatable_DAL;
            S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_CashflowMasterDataTable ObjCashflowMaster_Datatable_DAL;
            S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_CashFlowMasterProgramMappingDataTable ObjCashflowMapping_Datatable_DAL;
            S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetCashflowDetailsDataTable ObjCashflowDetails;
            S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetCashflowMappingDataTable ObjCashflowMappingDetails;
            S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_DeleteCashFlowMasterDataTable ObjDeleteCashflowDetails;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubCashflowMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion




            #region GLSLList
            public S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GLSLCodeListDataTable FunPubGLSLList(SerializationMode SerMode, byte[] bytesObjS3G_ORG_GLSLListDataTable)
            {
                S3GBusEntity.Origination.CashflowMgtServices dsGLSLList = new S3GBusEntity.Origination.CashflowMgtServices();
                ObjGLSLCodeList_DAL = (S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GLSLCodeListDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_GLSLListDataTable, SerMode, typeof(S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GLSLCodeListDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetGLSLCODEList");
                    foreach (S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GLSLCodeListRow ObjGLSLListRow in ObjGLSLCodeList_DAL.Rows)
                    {
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjGLSLListRow.LOB_ID);
                        //db.LoadDataSet(command, dsGLSLList, dsGLSLList.S3G_ORG_GLSLCodeList.TableName);
                        db.FunPubLoadDataSet(command, dsGLSLList, dsGLSLList.S3G_ORG_GLSLCodeList.TableName);
                    }

                }
                catch (Exception exp)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsGLSLList.S3G_ORG_GLSLCodeList;

            }
            #endregion

            #region CashflowProgram
            public S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetProgramDataTable FunPubProgramMaster(SerializationMode SerMode, byte[] bytesObjS3G_ORG_ProgramMasterDataTable)
            {
                S3GBusEntity.Origination.CashflowMgtServices dsProgramMaster = new S3GBusEntity.Origination.CashflowMgtServices();
                ObjGetProgramMaster_DAL = (S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetProgramDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_ProgramMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetProgramDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetProgramNumber");
                    foreach (S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetProgramRow ObjProgramMasterRow in ObjGetProgramMaster_DAL.Rows)
                    {
                        //db.AddInParameter(command, "@COMPANY_ID", DbType.Int32, ObjProgramMasterRow.Company_ID);
                        //db.LoadDataSet(command, dsProgramMaster, dsProgramMaster.S3G_ORG_GetProgram.TableName);
                        db.FunPubLoadDataSet(command, dsProgramMaster, dsProgramMaster.S3G_ORG_GetProgram.TableName);
                    }

                }
                catch (Exception exp)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsProgramMaster.S3G_ORG_GetProgram;

            }
            #endregion

            #region Lookup
            public S3GBusEntity.Origination.CashflowMgtServices.S3G_Status_LookUPDataTable FunPubCashflowTypeMaster(SerializationMode Sermode, byte[] bytesCashflowTypeMaster)
            {
                S3GBusEntity.Origination.CashflowMgtServices DsCashflowType = new S3GBusEntity.Origination.CashflowMgtServices();
                ObjCashflowType_Datatable_DAL = (S3GBusEntity.Origination.CashflowMgtServices.S3G_Status_LookUPDataTable)ClsPubSerialize.DeSerialize(bytesCashflowTypeMaster, Sermode, typeof(S3GBusEntity.Origination.CashflowMgtServices.S3G_Status_LookUPDataTable));
                try
                {
                    // DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetStatusLookUp");
                    foreach (S3GBusEntity.Origination.CashflowMgtServices.S3G_Status_LookUPRow ObjRow in ObjCashflowType_Datatable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Option", DbType.Int32, ObjRow.ID);
                        db.AddInParameter(command, "@Param1", DbType.String, ObjRow.Type);
                    }
                    //db.LoadDataSet(command, DsCashflowType, DsCashflowType.S3G_Status_LookUP.TableName);
                    db.FunPubLoadDataSet(command, DsCashflowType, DsCashflowType.S3G_Status_LookUP.TableName);
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsCashflowType.S3G_Status_LookUP;
            }
            #endregion


            #region Delete Cashflow
            public int FunPubDeleteCashflow(SerializationMode SerMode, byte[] bytesObjS3G_ORG_DeleteCashflowMasterDataTable)
            {
                try
                {

                    ObjDeleteCashflowDetails = (S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_DeleteCashFlowMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_DeleteCashflowMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_DeleteCashFlowMasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_DeleteCashflowDetails");
                    foreach (S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_DeleteCashFlowMasterRow ObjDeleteCashflowMasterRow in ObjDeleteCashflowDetails.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjDeleteCashflowMasterRow.Company_ID);
                        db.AddInParameter(command, "@Cashflow_ID", DbType.Int32, ObjDeleteCashflowMasterRow.CashFlow_ID);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
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

            #region Create Cashflow
            public int FunPubCreateCashflow(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CashflowMasterDataTable, out string strCashflowNumber_Out)
            {
                strCashflowNumber_Out = string.Empty;
                try
                {

                    ObjCashflowMaster_Datatable_DAL = (S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_CashflowMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_CashflowMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_CashflowMasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_CashflowMasterRow ObjCashflowMasterRow in ObjCashflowMaster_Datatable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertCashflowDetails");
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjCashflowMasterRow.LOB_ID);
                        if (!ObjCashflowMasterRow.IsBranch_IDNull())
                        {
                            db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjCashflowMasterRow.Branch_ID);
                        }
                        db.AddInParameter(command, "@COMPANY_ID", DbType.Int64, ObjCashflowMasterRow.COMPANY_ID);
                        db.AddInParameter(command, "@CashFlow_Description", DbType.String, ObjCashflowMasterRow.CashFlow_Description);
                        db.AddInParameter(command, "@Flow_Type", DbType.String, ObjCashflowMasterRow.Flow_Type);
                        db.AddInParameter(command, "@CASHFLOW_FLAG_ID", DbType.Int32, ObjCashflowMasterRow.CASHFLOW_FLAG_ID);
                        db.AddInParameter(command, "@Business_IRR", DbType.Boolean, ObjCashflowMasterRow.Business_IRR);
                        db.AddInParameter(command, "@Accounting_IRR", DbType.Boolean, ObjCashflowMasterRow.Accounting_IRR);
                        db.AddInParameter(command, "@Company_IRR", DbType.Boolean, ObjCashflowMasterRow.Company_IRR);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjCashflowMasterRow.Is_Active);
                        db.AddInParameter(command, "@CREATED_BY", DbType.Int32, ObjCashflowMasterRow.CREATED_BY);
                        db.AddInParameter(command, "@CREATED_ON", DbType.DateTime, ObjCashflowMasterRow.CREATED_ON);

                        if (!ObjCashflowMasterRow.IsCredit_Type_IDNull())
                            db.AddInParameter(command, "@Credit_Type_ID", DbType.Int32, ObjCashflowMasterRow.Credit_Type_ID);
                        if (!ObjCashflowMasterRow.IsCGL_ACCOUNT_CODENull())
                            db.AddInParameter(command, "@CGL_ACCOUNT_CODE", DbType.String, ObjCashflowMasterRow.CGL_ACCOUNT_CODE);
                        if (!ObjCashflowMasterRow.IsCSL_ACCOUNT_CODENull())
                            db.AddInParameter(command, "@CSL_ACCOUNT_CODE", DbType.String, ObjCashflowMasterRow.CSL_ACCOUNT_CODE);
                        if (!ObjCashflowMasterRow.IsCredit_Account_Setup_IDNull())
                            db.AddInParameter(command, "@Credit_Account_Setup_ID", DbType.Int32, ObjCashflowMasterRow.Credit_Account_Setup_ID);

                        if (!ObjCashflowMasterRow.IsDebit_Type_IDNull())
                            db.AddInParameter(command, "@Debit_Type_ID", DbType.Int32, ObjCashflowMasterRow.Debit_Type_ID);
                        if (!ObjCashflowMasterRow.IsDGL_ACCOUNT_CODENull())
                            db.AddInParameter(command, "@DGL_ACCOUNT_CODE", DbType.String, ObjCashflowMasterRow.DGL_ACCOUNT_CODE);
                        if (!ObjCashflowMasterRow.IsDSL_ACCOUNT_CODENull())
                            db.AddInParameter(command, "@DSL_ACCOUNT_CODE", DbType.String, ObjCashflowMasterRow.DSL_ACCOUNT_CODE);
                        if (!ObjCashflowMasterRow.IsDebit_Account_Setup_IDNull())
                            db.AddInParameter(command, "@Debit_Account_Setup_ID", DbType.Int32, ObjCashflowMasterRow.Debit_Account_Setup_ID);


                        if (!ObjCashflowMasterRow.IsRecoveryTypeNull())
                            db.AddInParameter(command, "@RecoveryType", DbType.Int32, ObjCashflowMasterRow.RecoveryType);

                        if (!ObjCashflowMasterRow.IsParent_CashflowNull())
                            db.AddInParameter(command, "@Parent_Cashflow", DbType.Int32, ObjCashflowMasterRow.Parent_Cashflow);

                        db.AddInParameter(command, "@XML_CASHDeltails", DbType.String, ObjCashflowMasterRow.XML_CASHDeltails);
                        /* START - By Siva.k on 10MAR2015 -For Extend Details */
                        db.AddInParameter(command, "@DebitType", DbType.String, ObjCashflowMasterRow.DebitType);
                        db.AddInParameter(command, "@CreditType", DbType.String, ObjCashflowMasterRow.CreditType);
                        db.AddInParameter(command, "@isMulti", DbType.String, ObjCashflowMasterRow.isMulti);
                        db.AddInParameter(command, "@isState", DbType.String, ObjCashflowMasterRow.isState);
                        db.AddInParameter(command, "@XML_ExtendDetails", DbType.String, ObjCashflowMasterRow.XML_ExtendDetails);// 
                        db.AddInParameter(command, "@Multi_Type", DbType.String, ObjCashflowMasterRow.Multi_Type);
                        /* END - By Siva.k on 10MAR2015 -For Extend Details */
                        db.AddOutParameter(command, "@Cashflow_No", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                strCashflowNumber_Out = (string)command.Parameters["@Cashflow_No"].Value;
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
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }

            #endregion


            #region Modify Cashflow
            public int FunPubModifyCashflow(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CashflowMasterDataTable)
            {
                try
                {
                    ObjCashflowMaster_Datatable_DAL = (S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_CashflowMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_CashflowMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_CashflowMasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {


                            foreach (S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_CashflowMasterRow ObjCashflowMasterRow in ObjCashflowMaster_Datatable_DAL.Rows)
                            {

                                DbCommand command = db.GetStoredProcCommand("S3G_ORG_UpdateCashflowDetails");
                                db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjCashflowMasterRow.LOB_ID);
                                if (!ObjCashflowMasterRow.IsBranch_IDNull())
                                {
                                    db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjCashflowMasterRow.Branch_ID);
                                }
                                db.AddInParameter(command, "@CashFlow_ID", DbType.Int32, ObjCashflowMasterRow.CashFlow_ID);
                                db.AddInParameter(command, "@CashflowDebit_ID", DbType.Int32, ObjCashflowMasterRow.CashflowDebit_ID);
                                db.AddInParameter(command, "@CashflowCredit_ID", DbType.Int32, ObjCashflowMasterRow.CashflowCredit_ID);
                                db.AddInParameter(command, "@CFSl_No", DbType.String, ObjCashflowMasterRow.CFSl_No);
                                db.AddInParameter(command, "@COMPANY_ID", DbType.Int32, ObjCashflowMasterRow.COMPANY_ID);
                                db.AddInParameter(command, "@CashFlow_Description", DbType.String, ObjCashflowMasterRow.CashFlow_Description);
                                db.AddInParameter(command, "@Flow_Type", DbType.String, ObjCashflowMasterRow.Flow_Type);
                                db.AddInParameter(command, "@CASHFLOW_FLAG_ID", DbType.Int32, ObjCashflowMasterRow.CASHFLOW_FLAG_ID);
                                db.AddInParameter(command, "@Business_IRR", DbType.Boolean, ObjCashflowMasterRow.Business_IRR);
                                db.AddInParameter(command, "@Accounting_IRR", DbType.Boolean, ObjCashflowMasterRow.Accounting_IRR);
                                db.AddInParameter(command, "@Company_IRR", DbType.Boolean, ObjCashflowMasterRow.Company_IRR);
                                db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjCashflowMasterRow.Is_Active);
                                db.AddInParameter(command, "@MODIFIED_BY", DbType.Int32, ObjCashflowMasterRow.MODIFIED_BY);
                                db.AddInParameter(command, "@MODIFIED_ON", DbType.DateTime, ObjCashflowMasterRow.MODIFIED_ON);
                                //db.AddInParameter(command, "@Debit_Account_Setup_ID", DbType.Int32, ObjCashflowMasterRow.Debit_Account_Setup_ID);
                                //db.AddInParameter(command, "@CGL_ACCOUNT_CODE", DbType.Int32, ObjCashflowMasterRow.CGL_ACCOUNT_CODE);
                                //db.AddInParameter(command, "@Credit_Type_ID", DbType.Int32, ObjCashflowMasterRow.Credit_Type_ID);
                                //db.AddInParameter(command, "@Credit_Account_Setup_ID", DbType.Int32, ObjCashflowMasterRow.Credit_Account_Setup_ID);
                                //db.AddInParameter(command, "@CSL_ACCOUNT_CODE", DbType.Int32, ObjCashflowMasterRow.CSL_ACCOUNT_CODE);
                                //db.AddInParameter(command, "@DGL_ACCOUNT_CODE", DbType.Int32, ObjCashflowMasterRow.DGL_ACCOUNT_CODE);
                                //db.AddInParameter(command, "@Debit_Type_ID", DbType.Int32, ObjCashflowMasterRow.Debit_Type_ID);


                                if (!ObjCashflowMasterRow.IsParent_CashflowNull())
                                    db.AddInParameter(command, "@Parent_Cashflow", DbType.Int32, ObjCashflowMasterRow.Parent_Cashflow);

                                if (!ObjCashflowMasterRow.IsDebit_Type_IDNull())
                                    db.AddInParameter(command, "@Debit_Type_ID", DbType.Int32, ObjCashflowMasterRow.Debit_Type_ID);
                                if (!ObjCashflowMasterRow.IsDGL_ACCOUNT_CODENull())
                                    db.AddInParameter(command, "@DGL_ACCOUNT_CODE", DbType.String, ObjCashflowMasterRow.DGL_ACCOUNT_CODE);
                                if (!ObjCashflowMasterRow.IsDSL_ACCOUNT_CODENull())
                                    db.AddInParameter(command, "@DSL_ACCOUNT_CODE", DbType.String, ObjCashflowMasterRow.DSL_ACCOUNT_CODE);
                                if (!ObjCashflowMasterRow.IsDebit_Account_Setup_IDNull())
                                    db.AddInParameter(command, "@Debit_Account_Setup_ID", DbType.Int32, ObjCashflowMasterRow.Debit_Account_Setup_ID);

                                if (!ObjCashflowMasterRow.IsCredit_Type_IDNull())
                                    db.AddInParameter(command, "@Credit_Type_ID", DbType.Int32, ObjCashflowMasterRow.Credit_Type_ID);
                                if (!ObjCashflowMasterRow.IsCGL_ACCOUNT_CODENull())
                                    db.AddInParameter(command, "@CGL_ACCOUNT_CODE", DbType.String, ObjCashflowMasterRow.CGL_ACCOUNT_CODE);
                                if (!ObjCashflowMasterRow.IsCSL_ACCOUNT_CODENull())
                                    db.AddInParameter(command, "@CSL_ACCOUNT_CODE", DbType.String, ObjCashflowMasterRow.CSL_ACCOUNT_CODE);
                                if (!ObjCashflowMasterRow.IsCredit_Account_Setup_IDNull())
                                    db.AddInParameter(command, "@Credit_Account_Setup_ID", DbType.Int32, ObjCashflowMasterRow.Credit_Account_Setup_ID);

                                db.AddInParameter(command, "@XML_CASHDeltails", DbType.String, ObjCashflowMasterRow.XML_CASHDeltails);
                                /* START - By Siva.k on 10MAR2015 -For Extend Details */
                                db.AddInParameter(command, "@DebitType", DbType.String, ObjCashflowMasterRow.DebitType);
                                db.AddInParameter(command, "@CreditType", DbType.String, ObjCashflowMasterRow.CreditType);
                                db.AddInParameter(command, "@isMulti", DbType.String, ObjCashflowMasterRow.isMulti);
                                db.AddInParameter(command, "@isState", DbType.String, ObjCashflowMasterRow.isState);
                                db.AddInParameter(command, "@XML_ExtendDetails", DbType.String, ObjCashflowMasterRow.XML_ExtendDetails);// 
                                db.AddInParameter(command, "@Multi_Type", DbType.String, ObjCashflowMasterRow.Multi_Type);
                                /* END - By Siva.k on 10MAR2015 -For Extend Details */

                                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                                //db.ExecuteNonQuery(command);
                                db.FunPubExecuteNonQuery(command);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                            }
                            trans.Commit();
                        }
                        catch (Exception exp)
                        {
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
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }
            #endregion

            #region GetCFM
            public S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetCashflowDetailsDataTable FunPubGetCashflowDetails(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_CashflowDetailsDataTable)
            {
                S3GBusEntity.Origination.CashflowMgtServices dsCashflowDetails = new S3GBusEntity.Origination.CashflowMgtServices();
                ObjCashflowDetails = (S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetCashflowDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_CashflowDetailsDataTable, SerMode, typeof(S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetCashflowDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetCashflowDetails");
                    foreach (S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetCashflowDetailsRow ObjCashflowDetailsRow in ObjCashflowDetails.Rows)
                    {
                        db.AddInParameter(command, "@CashFlow_ID", DbType.Int32, ObjCashflowDetailsRow.CashFlow_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCashflowDetailsRow.Company_ID);
                        //db.LoadDataSet(command, dsCashflowDetails, dsCashflowDetails.S3G_ORG_GetCashflowDetails.TableName);
                        db.FunPubLoadDataSet(command, dsCashflowDetails, dsCashflowDetails.S3G_ORG_GetCashflowDetails.TableName);
                    }

                }
                catch (Exception exp)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsCashflowDetails.S3G_ORG_GetCashflowDetails;

            }

            public S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetCashflowMappingDataTable FunPubGetCashflowMapping(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_CashflowMappingDataTable)
            {
                S3GBusEntity.Origination.CashflowMgtServices dsCashflowMapping = new S3GBusEntity.Origination.CashflowMgtServices();
                ObjCashflowMappingDetails = (S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetCashflowMappingDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_CashflowMappingDataTable, SerMode, typeof(S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetCashflowMappingDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetCashflowMappingRow ObjCashflowMappingRow in ObjCashflowMappingDetails.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetCashflowMapping");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCashflowMappingRow.Company_ID);
                        db.AddInParameter(command, "@CashFlow_ID", DbType.Int32, ObjCashflowMappingRow.CashFlow_ID);
                        //db.LoadDataSet(command, dsCashflowMapping, dsCashflowMapping.S3G_ORG_GetCashflowMapping.TableName);
                        db.FunPubLoadDataSet(command, dsCashflowMapping, dsCashflowMapping.S3G_ORG_GetCashflowMapping.TableName);
                    }

                }
                catch (Exception exp)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsCashflowMapping.S3G_ORG_GetCashflowMapping;

            }


            #endregion

        }
    }
}
