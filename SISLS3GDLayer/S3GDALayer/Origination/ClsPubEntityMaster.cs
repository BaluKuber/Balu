#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Entity Master
/// Created By			: Nataraj Y
/// Created Date		: 05-Jun-2010
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
        public class ClsPubEntityMaster
        {
            #region Intialize
            int intRowsAffected;
            ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterDataTable ObjEntityMaster_DAL;
            ORG.OrgMasterMgtServices.S3G_ORG_EntityBankMappingDataTable ObjEntityBankMappingMaster_DAL;


            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubEntityMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion

            #region Creating New Entity
            public int FunPubCreateEntityInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_Entity_MasterDataTable, out string strEntityCode_Out)
            {
                string strEntityCode = "";
                try
                {

                    ObjEntityMaster_DAL = (ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_Entity_MasterDataTable, SerMode, typeof(ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterRow ObjEnitytMasterRow in ObjEntityMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertEntityDetails");
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjEnitytMasterRow.Company_Id);
                        db.AddInParameter(command, "@EntityType", DbType.Int32, ObjEnitytMasterRow.Entity_Type);
                        db.AddInParameter(command, "@GLPostingCode", DbType.String, ObjEnitytMasterRow.GL_Code);
                        db.AddInParameter(command, "@EntityName", DbType.String, ObjEnitytMasterRow.Entity_Name);
                        db.AddInParameter(command, "@COMPANYTYPE", DbType.String, ObjEnitytMasterRow.Company_Type);
                        db.AddInParameter(command, "@Constitution", DbType.Int32, ObjEnitytMasterRow.Constitution);
                        db.AddInParameter(command, "@SERVICETAXNO", DbType.String, ObjEnitytMasterRow.Service_Tax_Number);
                        db.AddInParameter(command, "@Reg_Status", DbType.Int32, ObjEnitytMasterRow.Reg_Status);
                        db.AddInParameter(command, "@PAN", DbType.String, ObjEnitytMasterRow.PAN);
                        db.AddInParameter(command, "@CIN", DbType.String, ObjEnitytMasterRow.CIN);
                        //db.AddInParameter(command, "@SERVICETAXNO", DbType.String, ObjEnitytMasterRow.SERVICETAXNO); 
                        db.AddInParameter(command, "@Website", DbType.String, ObjEnitytMasterRow.Website);
                        db.AddInParameter(command, "@ResidentialStatus", DbType.Int32, ObjEnitytMasterRow.ResidentialStatus);
                        if (!ObjEnitytMasterRow.IsTax_Account_NumberNull())
                        {
                            db.AddInParameter(command, "@TaxAccountNumber", DbType.String, ObjEnitytMasterRow.Tax_Account_Number);
                        }
                        if (!ObjEnitytMasterRow.IsVAT_NumberNull())
                        {
                            db.AddInParameter(command, "@VAT_Number", DbType.String, ObjEnitytMasterRow.VAT_Number);
                        }
                        if (!ObjEnitytMasterRow.IsROC_NumberNull())
                        {
                            db.AddInParameter(command, "@ROC_Number", DbType.String, ObjEnitytMasterRow.ROC_Number);
                        }
                        if (!ObjEnitytMasterRow.IsIMPEXP_CodeNull())
                        {
                            db.AddInParameter(command, "@IMPEXP_Code", DbType.String, ObjEnitytMasterRow.IMPEXP_Code);
                        }
                        db.AddInParameter(command, "@USRID", DbType.Int32, ObjEnitytMasterRow.USRID);
                        //Added By Thangam M on 14/Feb/2012 to change the XML to CLOB
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XMLBankDetails",
                                   OracleType.Clob, ObjEnitytMasterRow.XMLBank_Details.Length,
                                   ParameterDirection.Input, true, 0, 0, String.Empty,
                                   DataRowVersion.Default, ObjEnitytMasterRow.XMLBank_Details);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLBankDetails", DbType.String, ObjEnitytMasterRow.XMLBank_Details);
                        }



                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XMLAddressDetails",
                                   OracleType.Clob, ObjEnitytMasterRow.XMLBank_Details.Length,
                                   ParameterDirection.Input, true, 0, 0, String.Empty,
                                   DataRowVersion.Default, ObjEnitytMasterRow.XMLBank_Details);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLAddressDetails", DbType.String, ObjEnitytMasterRow.XMLAddressDetails);
                        }


                        //Thangam M on 15/Nov/2012 for Trade Advance
                        // db.AddInParameter(command, "@Is_TradeAdvance", DbType.Int32, ObjEnitytMasterRow.Is_TradeAdvance);
                        //End here
                        db.AddOutParameter(command, "@EntityCode", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, 4);
                        db.AddInParameter(command, "@CGSTIN", DbType.String, ObjEnitytMasterRow.CGSTIN);
                        if (!ObjEnitytMasterRow.IsCGST_Reg_DateNull())
                            db.AddInParameter(command, "@CGST_Reg_Date", DbType.String, ObjEnitytMasterRow.CGST_Reg_Date);
                        db.AddInParameter(command, "@Composition", DbType.Int32, ObjEnitytMasterRow.Composition);

                        db.AddInParameter(command, "@MSME_Registered", DbType.Int32, ObjEnitytMasterRow.MSME_Registered);
                        db.AddInParameter(command, "@Vendor_Type", DbType.Int32, ObjEnitytMasterRow.Vendor_Type);
                        db.AddInParameter(command, "@Certificate_Received", DbType.Int32, ObjEnitytMasterRow.Certificate_Received);

                        db.AddInParameter(command, "@eInvoice", DbType.Int32, ObjEnitytMasterRow.EInvoice);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                strEntityCode = (string)command.Parameters["@EntityCode"].Value;
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
                strEntityCode_Out = strEntityCode;
                return intRowsAffected;
            }
            #endregion

            #region Modifiy Entity
            public int FunPubModifyEntityInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_Entity_MasterDataTable)
            {

                try
                {

                    ObjEntityMaster_DAL = (ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_Entity_MasterDataTable, SerMode, typeof(ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            foreach (ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterRow ObjEnitytMasterRow in ObjEntityMaster_DAL.Rows)
                            {
                                DbCommand command = db.GetStoredProcCommand("S3G_ORG_UpdateEntityDetails");
                                db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjEnitytMasterRow.Company_Id);
                                db.AddInParameter(command, "@EntityType", DbType.Int32, ObjEnitytMasterRow.Entity_Type);
                                db.AddInParameter(command, "@GLPostingCode", DbType.String, ObjEnitytMasterRow.GL_Code);
                                db.AddInParameter(command, "@Entity_Master_ID", DbType.Int32, ObjEnitytMasterRow.Entity_Master_ID);
                                db.AddInParameter(command, "@EntityCode", DbType.String, ObjEnitytMasterRow.Entity_Code);
                                db.AddInParameter(command, "@EntityName", DbType.String, ObjEnitytMasterRow.Entity_Name);
                                db.AddInParameter(command, "@COMPANYTYPE", DbType.String, ObjEnitytMasterRow.Company_Type);
                                db.AddInParameter(command, "@Constitution", DbType.Int32, ObjEnitytMasterRow.Constitution);
                                db.AddInParameter(command, "@SERVICETAXNO", DbType.String, ObjEnitytMasterRow.Service_Tax_Number);
                                db.AddInParameter(command, "@Reg_Status", DbType.Int32, ObjEnitytMasterRow.Reg_Status);
                                db.AddInParameter(command, "@PAN", DbType.String, ObjEnitytMasterRow.PAN);
                                db.AddInParameter(command, "@CIN", DbType.String, ObjEnitytMasterRow.CIN);
                                //db.AddInParameter(command, "@Address", DbType.String, ObjEnitytMasterRow.Address);
                                //if (!ObjEnitytMasterRow.IsAddress2Null())
                                //{
                                //    db.AddInParameter(command, "@Address2", DbType.String, ObjEnitytMasterRow.Address2);
                                //}
                                //db.AddInParameter(command, "@City", DbType.String, ObjEnitytMasterRow.City);
                                //db.AddInParameter(command, "@State", DbType.String, ObjEnitytMasterRow.State);
                                //db.AddInParameter(command, "@Country", DbType.String, ObjEnitytMasterRow.Country);
                                //db.AddInParameter(command, "@PINCode", DbType.String, ObjEnitytMasterRow.PINCode);
                                //if (!ObjEnitytMasterRow.IsMobileNull())
                                //{
                                //    db.AddInParameter(command, "@Mobile", DbType.Decimal, ObjEnitytMasterRow.Mobile);
                                //}
                                //db.AddInParameter(command, "@Telephone", DbType.String, ObjEnitytMasterRow.Telephone);
                                //db.AddInParameter(command, "@Email", DbType.String, ObjEnitytMasterRow.EMail);
                                db.AddInParameter(command, "@Website", DbType.String, ObjEnitytMasterRow.Website);
                                db.AddInParameter(command, "@ResidentialStatus", DbType.Int32, ObjEnitytMasterRow.ResidentialStatus);
                                if (!ObjEnitytMasterRow.IsTax_Account_NumberNull())
                                {
                                    db.AddInParameter(command, "@TaxAccountNumber", DbType.String, ObjEnitytMasterRow.Tax_Account_Number);
                                }
                                if (!ObjEnitytMasterRow.IsVAT_NumberNull())
                                {
                                    db.AddInParameter(command, "@VAT_Number", DbType.String, ObjEnitytMasterRow.VAT_Number);
                                }
                                if (!ObjEnitytMasterRow.IsROC_NumberNull())
                                {
                                    db.AddInParameter(command, "@ROC_Number", DbType.String, ObjEnitytMasterRow.ROC_Number);
                                }
                                if (!ObjEnitytMasterRow.IsIMPEXP_CodeNull())
                                {
                                    db.AddInParameter(command, "@IMPEXP_Code", DbType.String, ObjEnitytMasterRow.IMPEXP_Code);
                                }
                                db.AddInParameter(command, "@USRID", DbType.Int32, ObjEnitytMasterRow.USRID);
                                //Added By Thangam M on 14/Feb/2012 to change the XML to CLOB
                                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                                if (enumDBType == S3GDALDBType.ORACLE)
                                {
                                    OracleParameter param = new OracleParameter("@XMLBankDetails",
                                           OracleType.Clob, ObjEnitytMasterRow.XMLBank_Details.Length,
                                           ParameterDirection.Input, true, 0, 0, String.Empty,
                                           DataRowVersion.Default, ObjEnitytMasterRow.XMLBank_Details);
                                    command.Parameters.Add(param);
                                }
                                else
                                {
                                    db.AddInParameter(command, "@XMLBankDetails", DbType.String, ObjEnitytMasterRow.XMLBank_Details);
                                }

                                if (enumDBType == S3GDALDBType.ORACLE)
                                {
                                    OracleParameter param = new OracleParameter("@XMLAddressDetails",
                                           OracleType.Clob, ObjEnitytMasterRow.XMLBank_Details.Length,
                                           ParameterDirection.Input, true, 0, 0, String.Empty,
                                           DataRowVersion.Default, ObjEnitytMasterRow.XMLBank_Details);
                                    command.Parameters.Add(param);
                                }
                                else
                                {
                                    db.AddInParameter(command, "@XMLAddressDetails", DbType.String, ObjEnitytMasterRow.XMLAddressDetails);
                                }


                                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, 4);
                                db.AddInParameter(command, "@CGSTIN", DbType.String, ObjEnitytMasterRow.CGSTIN);
                                if (!ObjEnitytMasterRow.IsCGST_Reg_DateNull())
                                    db.AddInParameter(command, "@CGST_Reg_Date", DbType.String, ObjEnitytMasterRow.CGST_Reg_Date);
                                db.AddInParameter(command, "@Composition", DbType.Int32, ObjEnitytMasterRow.Composition);

                                db.AddInParameter(command, "@MSME_Registered", DbType.Int32, ObjEnitytMasterRow.MSME_Registered);
                                db.AddInParameter(command, "@Vendor_Type", DbType.Int32, ObjEnitytMasterRow.Vendor_Type);
                                db.AddInParameter(command, "@Certificate_Received", DbType.Int32, ObjEnitytMasterRow.Certificate_Received);

                                db.AddInParameter(command, "@eInvoice", DbType.Int32, ObjEnitytMasterRow.EInvoice);

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

            #region Get Entity Details
            /// <summary>
            /// to get entity code details
            /// </summary>
            /// <param name="intCompany_Id">can be null or 0 to get all entity for all companies</param>
            /// <param name="intEntity_id">can be null or 0 to get all entities</param>
            /// <returns></returns>
            public DataSet FunPubQueryEntityDetails(int? intCompany_Id, int? intEntity_id, out bool blnTranExists)
            {
                DataSet dsEntityDetails = new DataSet();
                blnTranExists = false;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetEntityDetails");
                    if (intCompany_Id.HasValue && intCompany_Id.Value != 0)
                    {
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, intCompany_Id);
                    }
                    if (intEntity_id.HasValue && intEntity_id.Value != 0)
                    {
                        db.AddInParameter(command, "@Entity_Id", DbType.Int32, intEntity_id);
                    }
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@RecordExists", DbType.Boolean, sizeof(Boolean));
                    //dsEntityDetails = db.ExecuteDataSet(command);

                    //dsEntityDetails = db.FunPubExecuteDataSet(command);

                    db.FunPubLoadDataSet(command, dsEntityDetails, "Table0");

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        throw new Exception("Error in Getting Entity details");
                    blnTranExists = Convert.ToBoolean(command.Parameters["@RecordExists"].Value);
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }

                return dsEntityDetails;
            }
            #endregion

        }
    }
}
