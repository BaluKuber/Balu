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

namespace S3GDALayer.Insurance
{
    namespace InsuranceMgtServices
    {
        public class ClsPubAssetInsuranceEntry : ClsPubDalInsuranceBase
        {
            #region Initialization  
            int intRowsAffected;
            S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable objAssetInsuranceEntry_DAL;
            #endregion

            #region AssetEntry
            public int FunPubCreateAssetEntryDetails(SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetInsuranceEntry_DataTable, out string strAINSENO)
            {
                strAINSENO = "";
                try
                {
                    objAssetInsuranceEntry_DAL = (S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable)
                        ClsPubSerialize.DeSerialize(byteObjS3G_Insurance_AssetInsuranceEntry_DataTable, SerMode,
                        typeof(S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryRow objAssetEntryRow in objAssetInsuranceEntry_DAL)
                    {
                        
                        DbCommand command = db.GetStoredProcCommand("S3G_INS_InsertAssetpolicyDetails");
                        
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objAssetEntryRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objAssetEntryRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objAssetEntryRow.Branch_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, objAssetEntryRow.Customer_ID);

                        db.AddInParameter(command, "@PANum", DbType.String, objAssetEntryRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, objAssetEntryRow.SANum);
                        db.AddInParameter(command, "@Ins_Done_By", DbType.String, objAssetEntryRow.Ins_Done_By);
                        db.AddInParameter(command, "@Tranche_No", DbType.String, objAssetEntryRow.Tranche_No);
                        db.AddInParameter(command, "@RS_No", DbType.Int32, objAssetEntryRow.RS_No);

                        if (!objAssetEntryRow.IsIns_Company_IdNull())
                        {
                            db.AddInParameter(command, "@Ins_Company_Id", DbType.Int64, objAssetEntryRow.Ins_Company_Id);
                        }

                        db.AddInParameter(command, "@Ins_Company_Name", DbType.String, objAssetEntryRow.Ins_Company_Name);
                        db.AddInParameter(command, "@Ins_Company_Address", DbType.String, objAssetEntryRow.Ins_Company_Address);
                        if (!objAssetEntryRow.IsIns_Company_Address2Null())
                        {
                            db.AddInParameter(command, "@Ins_Company_Address2", DbType.String, objAssetEntryRow.Ins_Company_Address2);
                        }
                                            
                        db.AddInParameter(command, "@Ins_Company_City", DbType.String, objAssetEntryRow.Ins_Company_City);
                        db.AddInParameter(command, "@Ins_Company_State", DbType.String, objAssetEntryRow.Ins_Company_State);
                        db.AddInParameter(command, "@Ins_Company_Pin", DbType.String, objAssetEntryRow.Ins_Company_Pin);
                        db.AddInParameter(command, "@Ins_Company_Country", DbType.String, objAssetEntryRow.Ins_Company_Country);
                        db.AddInParameter(command, "@Ins_Company_Telephone", DbType.String, objAssetEntryRow.Ins_Company_Telephone);
                        if (!objAssetEntryRow.IsIns_Company_MobileNull())
                        {
                            db.AddInParameter(command, "@Ins_Company_Mobile", DbType.Int64, objAssetEntryRow.Ins_Company_Mobile);
                        }

                        db.AddInParameter(command, "@Ins_Company_Email_ID", DbType.String, objAssetEntryRow.Ins_Company_Email_ID);
                        db.AddInParameter(command, "@Ins_Company_Web_Site", DbType.String, objAssetEntryRow.Ins_Company_Web_Site);

                        db.AddInParameter(command, "@Pay_Type", DbType.Int32, objAssetEntryRow.Pay_Type);

                        if (!objAssetEntryRow.IsInstallment_From_DateNull())
                        {
                            db.AddInParameter(command, "@Installment_From_Date", DbType.DateTime, objAssetEntryRow.Installment_From_Date);
                            db.AddInParameter(command, "@Installment_To_Date", DbType.DateTime, objAssetEntryRow.Installment_To_Date);
                        }
                        db.AddInParameter(command, "@Account_Link_Key", DbType.Int32, objAssetEntryRow.Account_Link_Key);
                        db.AddInParameter(command, "@UserId", DbType.Int32, objAssetEntryRow.UserId);
                        db.AddInParameter(command, "@XMLPolicyDetail", DbType.String, objAssetEntryRow.XmlPolicyDetails);
                        db.AddInParameter(command, "@Insurance_Type", DbType.String, objAssetEntryRow.Insurance_Type);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@AINSENO", DbType.String, 200);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                               db.FunPubExecuteNonQuery(command,ref  trans);
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                if (intRowsAffected > 0)
                                {
                                    throw new Exception("Error thrown Error No " + intRowsAffected.ToString());
                                }
                                else
                                {
                                    strAINSENO = (string)command.Parameters["@AINSENO"].Value;
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
                                trans.Rollback();
                                //throw ex;
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

                    throw ex;
                }

                return intRowsAffected;
            }
            public int FunPubUpdateAssetEntryDetails(SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetInsuranceEntry_DataTable)
            {
                int intErrorCode = 0;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_INS_UpdateAssetpolicyDetails");
                    objAssetInsuranceEntry_DAL = (S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable)
                       ClsPubSerialize.DeSerialize(byteObjS3G_Insurance_AssetInsuranceEntry_DataTable, SerMode,
                       typeof(S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable));
                    foreach (S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryRow objAssetEntryRow in objAssetInsuranceEntry_DAL)
                    {
                        db.AddInParameter(command, "@AINSEId", DbType.Int64, objAssetEntryRow.Account_Ins_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objAssetEntryRow.Company_ID);
                        db.AddInParameter(command, "@UserId", DbType.Int32, objAssetEntryRow.UserId);
                       
                        if (!objAssetEntryRow.IsInstallment_From_DateNull())
                        {
                            db.AddInParameter(command, "@Installment_From_Date", DbType.DateTime, objAssetEntryRow.Installment_From_Date);
                            db.AddInParameter(command, "@Installment_To_Date", DbType.DateTime, objAssetEntryRow.Installment_To_Date);
                        }

                        if (!objAssetEntryRow.IsIns_Company_IdNull())
                        {
                            db.AddInParameter(command, "@Ins_Company_Id", DbType.Int64, objAssetEntryRow.Ins_Company_Id);
                        }

                        db.AddInParameter(command, "@XMLPolicyDetail", DbType.String, objAssetEntryRow.XmlPolicyDetails);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                       // db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);
                        if (command.Parameters["@ErrorCode"].Value != null)
                        {
                            intErrorCode = Convert.ToInt32(command.Parameters["@ErrorCode"].Value);
                        }
                    }
                     return intErrorCode;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            public int FunPubUpdateInvoiceEntryDetails(SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetInsuranceEntry_DataTable)
            {
                int intErrorCode = 0;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_INS_UpdateInvoiceDetails");
                    objAssetInsuranceEntry_DAL = (S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable)
                       ClsPubSerialize.DeSerialize(byteObjS3G_Insurance_AssetInsuranceEntry_DataTable, SerMode,
                       typeof(S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable));
                    foreach (S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryRow objAssetEntryRow in objAssetInsuranceEntry_DAL)
                    {

                        db.AddInParameter(command, "@XmlInvoiceDetails", DbType.String, objAssetEntryRow.XmlInvoiceDetails);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        // db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);
                        if (command.Parameters["@ErrorCode"].Value != null)
                        {
                            intErrorCode = Convert.ToInt32(command.Parameters["@ErrorCode"].Value);
                        }
                    }
                    return intErrorCode;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            #endregion
        }
    }
}
