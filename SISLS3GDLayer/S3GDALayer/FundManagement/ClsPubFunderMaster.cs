using System;
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
using S3GDALayer.S3GAdminServices;
using System.Data.OracleClient;

namespace S3GDALayer.FundManagement
{
    namespace FundMgtService
    {
        public class ClsPubFunderMaster
        {
            #region Declaration
            int intRowsAffected;
            S3GBusEntity.FundManagement.FundMgtServices.S3G_Funder_MasterDataTable ObjFundMaster_DataTable = new S3GBusEntity.FundManagement.FundMgtServices.S3G_Funder_MasterDataTable();
            S3GBusEntity.FundManagement.FundMgtServices.S3G_FUNDMGT_FunderApprovalDataTable ObjApproval_DataTable = new S3GBusEntity.FundManagement.FundMgtServices.S3G_FUNDMGT_FunderApprovalDataTable();

            Database db;
            public ClsPubFunderMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create

            public int FunPubCreateOrModifyFunderMaster(SerializationMode SerMode, byte[] bytesObjFunderMaster_DataTable, out string strFunderCode, out Int64 intFunder_No, out string strErrorMsg)
            {
                strFunderCode = strErrorMsg = "";
                intFunder_No = 0;
                try
                {
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    ObjFundMaster_DataTable = (S3GBusEntity.FundManagement.FundMgtServices.S3G_Funder_MasterDataTable)ClsPubSerialize.DeSerialize(bytesObjFunderMaster_DataTable, SerMode, typeof(S3GBusEntity.FundManagement.FundMgtServices.S3G_Funder_MasterDataTable));
                    foreach (S3GBusEntity.FundManagement.FundMgtServices.S3G_Funder_MasterRow ObjFunderRow in ObjFundMaster_DataTable.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_FUNDMGT_INSERT_FUNDERMASTER");

                        db.AddInParameter(command, "@Funder_ID", DbType.Int64, ObjFunderRow.Funder_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjFunderRow.Company_ID);
                        db.AddInParameter(command, "@Funder_Code", DbType.String, ObjFunderRow.Funder_Code);
                        db.AddInParameter(command, "@Funder_Name", DbType.String, ObjFunderRow.Funder_Name);
                        db.AddInParameter(command, "@GL_Code", DbType.String, ObjFunderRow.GL_Code);
                        db.AddInParameter(command, "@Comm_Address1", DbType.String, ObjFunderRow.Comm_Address1);
                        db.AddInParameter(command, "@Comm_City", DbType.String, ObjFunderRow.Comm_City);
                        db.AddInParameter(command, "@Comm_State", DbType.String, ObjFunderRow.Comm_State);
                        db.AddInParameter(command, "@Comm_Country", DbType.String, ObjFunderRow.Comm_Country);
                        db.AddInParameter(command, "@Comm_Pincode", DbType.String, ObjFunderRow.Comm_Pincode);
                        db.AddInParameter(command, "@Comm_Mobile", DbType.String, ObjFunderRow.Comm_Mobile);
                        db.AddInParameter(command, "@Comm_Telephone", DbType.String, ObjFunderRow.Comm_Telephone);
                        db.AddInParameter(command, "@Comm_EMail", DbType.String, ObjFunderRow.Comm_EMail);
                        db.AddInParameter(command, "@Comm_TAN", DbType.String, ObjFunderRow.Comm_TAN);
                        db.AddInParameter(command, "@Comm_TIN", DbType.String, ObjFunderRow.Comm_TIN);
                        db.AddInParameter(command, "@Perm_Address1", DbType.String, ObjFunderRow.Perm_Address1);
                        db.AddInParameter(command, "@Perm_City", DbType.String, ObjFunderRow.Perm_City);
                        db.AddInParameter(command, "@Perm_State", DbType.String, ObjFunderRow.Perm_State);
                        db.AddInParameter(command, "@Perm_Country", DbType.String, ObjFunderRow.Perm_Country);
                        db.AddInParameter(command, "@Perm_Pincode", DbType.String, ObjFunderRow.Perm_Pincode);
                        db.AddInParameter(command, "@Perm_Mobile", DbType.String, ObjFunderRow.Perm_Mobile);
                        db.AddInParameter(command, "@Perm_Telephone", DbType.String, ObjFunderRow.Perm_Telephone);
                        db.AddInParameter(command, "@Perm_EMail", DbType.String, ObjFunderRow.Perm_EMail);
                        db.AddInParameter(command, "@Perm_TAN", DbType.String, ObjFunderRow.Perm_TAN);
                        db.AddInParameter(command, "@Perm_TIN", DbType.String, ObjFunderRow.Perm_TIN);
                        db.AddInParameter(command, "@Remarks", DbType.String, ObjFunderRow.Remarks);
                        db.AddInParameter(command, "@Status_ID", DbType.Int32, ObjFunderRow.Status_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjFunderRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjFunderRow.Created_On);
                        db.AddInParameter(command, "@Short_Name", DbType.String, ObjFunderRow.Short_Name);
                        db.AddInParameter(command, "@Corp_PAN", DbType.String, ObjFunderRow.Corp_PAN);
                        db.AddInParameter(command, "@CGSTIN", DbType.String, ObjFunderRow.CGSTIN);
                        if (!(Convert.ToString(ObjFunderRow.CGST_Reg_Date).Contains("1/1/0001")))
                        {
                            db.AddInParameter(command, "@CGST_Reg_Date", DbType.String, ObjFunderRow.CGST_Reg_Date);
                        }
                        db.AddInParameter(command, "@SGSTIN", DbType.String, ObjFunderRow.SGSTIN);
                        if (!(Convert.ToString(ObjFunderRow.SGST_Reg_Date).Contains("1/1/0001")))
                        {
                            db.AddInParameter(command, "@SGST_Reg_Date", DbType.String, ObjFunderRow.SGST_Reg_Date);
                        }
                        if (!ObjFunderRow.IsXML_BankDtlNull())
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XML_BankDtl", OracleType.Clob,
                                    ObjFunderRow.XML_BankDtl.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjFunderRow.XML_BankDtl);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XML_BankDtl", DbType.String,
                                   ObjFunderRow.XML_BankDtl);
                            }
                        }

                        if (!ObjFunderRow.IsXML_CustomerDtlNull())
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XML_CustomerDtl", OracleType.Clob,
                                    ObjFunderRow.XML_CustomerDtl.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjFunderRow.XML_CustomerDtl);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XML_CustomerDtl", DbType.String,
                                   ObjFunderRow.XML_CustomerDtl);
                            }
                        }

                        if (!ObjFunderRow.IsXML_LimitConslidationNull())
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XML_LimitConslidation", OracleType.Clob,
                                    ObjFunderRow.XML_LimitConslidation.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjFunderRow.XML_LimitConslidation);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XML_LimitConslidation", DbType.String,
                                   ObjFunderRow.XML_LimitConslidation);
                            }
                        }

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);
                        db.AddOutParameter(command, "@Funder_ID_OUT", DbType.Int64, sizeof(Int64));
                        db.AddOutParameter(command, "@Funder_Code_Out", DbType.String, 30);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);

                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                strFunderCode = Convert.ToString(command.Parameters["@Funder_Code_Out"].Value);
                                intFunder_No = (Int64)command.Parameters["@Funder_ID_OUT"].Value;
                                strErrorMsg = Convert.ToString(command.Parameters["@ErrorMsg"].Value);

                                trans.Commit();

                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                intRowsAffected = 50;
                                trans.Rollback();
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                    }
                }
                catch (Exception objException)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(objException);
                }
                return intRowsAffected;
            }


            #endregion

            #region Approval

            public int FunPubInsertApproval(SerializationMode SerMode, byte[] bytesObjApproval_DataTable, out string strApprovalNumber, out Int64 intApproval_No)
            {
                strApprovalNumber = "";
                intApproval_No = 0;
                try
                {
                    ObjApproval_DataTable = (S3GBusEntity.FundManagement.FundMgtServices.S3G_FUNDMGT_FunderApprovalDataTable)ClsPubSerialize.DeSerialize(bytesObjApproval_DataTable, SerMode, typeof(S3GBusEntity.FundManagement.FundMgtServices.S3G_FUNDMGT_FunderApprovalDataTable));
                    foreach (S3GBusEntity.FundManagement.FundMgtServices.S3G_FUNDMGT_FunderApprovalRow ObjApprovalRow in ObjApproval_DataTable.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_INSERT_FunderAPPROVAL");

                        db.AddInParameter(command, "@FunderApproval_ID", DbType.Int64, ObjApprovalRow.FunderApproval_ID);
                        db.AddInParameter(command, "@Funder_ID", DbType.Int64, ObjApprovalRow.Funder_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjApprovalRow.Company_ID);
                        db.AddInParameter(command, "@Approver_ID", DbType.Int32, ObjApprovalRow.Approver_ID);
                        db.AddInParameter(command, "@Approval_Date", DbType.DateTime, ObjApprovalRow.Approval_Date);
                        db.AddInParameter(command, "@Remarks", DbType.String, ObjApprovalRow.Remarks);
                        db.AddInParameter(command, "@Approval_Serial_Number", DbType.Int32, ObjApprovalRow.Approval_Serial_Number);
                        db.AddInParameter(command, "@Action_ID", DbType.Int32, ObjApprovalRow.Action_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjApprovalRow.Created_By);
                        if (!ObjApprovalRow.IsLOB_IDNull())
                        {
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjApprovalRow.LOB_ID);
                        }

                        if (!ObjApprovalRow.IsXML_Sanction_DtlNull())
                        {
                            db.AddInParameter(command, "@XML_Sanction_Dtl", DbType.String, ObjApprovalRow.XML_Sanction_Dtl);
                        }
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjApprovalRow.Customer_ID);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@ApprovalNumber", DbType.String, 50);
                        db.AddOutParameter(command, "@Approval_ID_OUT", DbType.Int64, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                // db.ExecuteNonQuery(command, trans);

                                db.FunPubExecuteNonQuery(command, ref trans);

                                // if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                strApprovalNumber = Convert.ToString(command.Parameters["@ApprovalNumber"].Value);
                                intApproval_No = (Int64)command.Parameters["@Approval_ID_OUT"].Value;
                                //if (intRowsAffected == 0)
                                trans.Commit();
                                //else
                                //    trans.Rollback();

                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                intRowsAffected = 50;
                                trans.Rollback();
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }
                catch (Exception objException)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(objException);
                }
                return intRowsAffected;
            }

            #endregion

            #region "Tmp Insert Sanction Detail"

            public int FunPubInsertTmpSancDtl(SerializationMode SerMode, byte[] byteObjSanc_DataTable)
            {
                try
                {
                    S3GBusEntity.FundManagement.FundMgtServices.S3G_FND_SanctionDtlDataTable ObjSanc_DataTable;
                    ObjSanc_DataTable = (S3GBusEntity.FundManagement.FundMgtServices.S3G_FND_SanctionDtlDataTable)ClsPubSerialize.DeSerialize(byteObjSanc_DataTable, SerMode, typeof(S3GBusEntity.FundManagement.FundMgtServices.S3G_FND_SanctionDtlDataTable));
                    foreach (S3GBusEntity.FundManagement.FundMgtServices.S3G_FND_SanctionDtlRow ObjRow in ObjSanc_DataTable.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_FMGT_INSERT_TMPSANCDTL");

                        db.AddInParameter(command, "@Funder_Detail_ID ", DbType.Int64, ObjRow.Funder_Detail_ID);
                        db.AddInParameter(command, "@Option ", DbType.Int32, ObjRow.Option);
                        db.AddInParameter(command, "@Funder_ID ", DbType.Int64, ObjRow.Funder_ID);
                        db.AddInParameter(command, "@Customer_ID ", DbType.Int64, ObjRow.Customer_ID);
                        db.AddInParameter(command, "@Enduser_ID ", DbType.Int32, ObjRow.EndUser_ID);
                        db.AddInParameter(command, "@PV_Method_ID ", DbType.Int32, ObjRow.PV_Method_ID);
                        db.AddInParameter(command, "@Asset_Category_ID ", DbType.Int32, ObjRow.Asset_Category_ID);
                        db.AddInParameter(command, "@Sanction_No ", DbType.String, ObjRow.Sanction_No);
                        db.AddInParameter(command, "@Sanction_Limit ", DbType.Decimal, ObjRow.Sanction_Limit);
                        db.AddInParameter(command, "@Balance_Limit ", DbType.Decimal, ObjRow.Balance_Limit);
                        db.AddInParameter(command, "@Sanction_Date ", DbType.DateTime, ObjRow.Sanction_Date);
                        db.AddInParameter(command, "@Expiry_Date ", DbType.DateTime, ObjRow.Expiry_Date);
                        db.AddInParameter(command, "@Discount_Rate ", DbType.Decimal, ObjRow.Discount_Rate);
                        db.AddInParameter(command, "@Processing_Fee_Perc ", DbType.Decimal, ObjRow.Processing_Fee_Perc);
                        db.AddInParameter(command, "@Processing_Fee_Amt ", DbType.Decimal, ObjRow.Processing_Fee_Amt);
                        db.AddInParameter(command, "@Foreclosure_Rate ", DbType.Decimal, ObjRow.ForeClosure_Rate);
                        db.AddInParameter(command, "@Misc_Charges ", DbType.Decimal, ObjRow.Misc_Charges);
                        db.AddInParameter(command, "@Tenure ", DbType.Int32, ObjRow.Tenure);
                        db.AddInParameter(command, "@ODI_Rate ", DbType.Decimal, ObjRow.ODI_Rate);
                        db.AddInParameter(command, "@Collateral_Details ", DbType.String, ObjRow.Collateral_Details);
                        db.AddInParameter(command, "@Remarks ", DbType.String, ObjRow.Remarks);
                        db.AddInParameter(command, "@Cheque_Rtn_Charge ", DbType.Decimal, ObjRow.Cheque_Rtn_Charge);
                        db.AddInParameter(command, "@Processing_Fee_ST ", DbType.Decimal, ObjRow.Processing_Fee_ST);
                        db.AddInParameter(command, "@Document_Path ", DbType.String, ObjRow.Document_Path);
                        db.AddInParameter(command, "@Status_ID ", DbType.Int32, ObjRow.Status_ID);
                        db.AddInParameter(command, "@Is_New ", DbType.Int32, ObjRow.Is_New);
                        db.AddInParameter(command, "@Is_Update ", DbType.Int32, ObjRow.Is_Update);
                        db.AddInParameter(command, "@Is_Delete ", DbType.Int32, ObjRow.Is_Delete);
                        db.AddInParameter(command, "@Created_By ", DbType.Int32, ObjRow.Created_By);
                        db.AddInParameter(command, "@Location_ID ", DbType.Int32, ObjRow.Location_ID);

                        db.AddInParameter(command, "@Upfront_Int", DbType.Decimal, ObjRow.UpfrontInterest);
                        db.AddInParameter(command, "@Net_Discount_Rate", DbType.Decimal, ObjRow.NetDiscountRate);
                        db.AddInParameter(command, "@Discount_Processing", DbType.Boolean, ObjRow.DiscProcessingFee);
                        db.AddInParameter(command, "@Discount_Upfront", DbType.Boolean, ObjRow.DiscUpfrontInterest);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                intRowsAffected = 50;
                                trans.Rollback();
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }
                catch (Exception objException)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(objException);
                }
                return intRowsAffected;
            }

            #endregion
        }
    }
}
