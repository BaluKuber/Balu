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
        public class ClsPubDeliveryInstruction
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable ObjDeliveryIns_DAL;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetAssetDetailsDataTable ObjAssetDetails_DAL;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable ObjCustDetails_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubDeliveryInstruction()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion


            #region GetAssetGrid

            public S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetAssetDetailsDataTable FunPubAssetMaster(SerializationMode SerMode, byte[] bytesObjS3G_ORG_AssetMasterDataTable)
            {
                S3GBusEntity.LoanAdmin.LoanAdminMgtServices dsAssetMaster = new S3GBusEntity.LoanAdmin.LoanAdminMgtServices();
                ObjAssetDetails_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetAssetDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_AssetMasterDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetAssetDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetAssetDetails");
                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetAssetDetailsRow ObjAssetMasterRow in ObjAssetDetails_DAL.Rows)
                    {
                        db.AddInParameter(command, "@COMPANY_ID", DbType.Int32, ObjAssetMasterRow.Company_ID);
                        db.AddInParameter(command, "@Entity_ID", DbType.Int32, ObjAssetMasterRow.Entity_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjAssetMasterRow.LOB_ID);
                        db.AddInParameter(command, "@PANum", DbType.String, ObjAssetMasterRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, ObjAssetMasterRow.SANum);

                        db.FunPubLoadDataSet(command, dsAssetMaster, dsAssetMaster.S3G_LOANAD_GetAssetDetails.TableName);
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsAssetMaster.S3G_LOANAD_GetAssetDetails;

            }

            #endregion

            public S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable FunPubCustMaster(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CustMasterDataTable)
            {
                S3GBusEntity.LoanAdmin.LoanAdminMgtServices dsCustMaster = new S3GBusEntity.LoanAdmin.LoanAdminMgtServices();
                ObjCustDetails_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_CustMasterDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetDICustomerDetails");
                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsRow ObjCustMasterRow in ObjCustDetails_DAL.Rows)
                    {
                        db.AddInParameter(command, "@COMPANY_ID", DbType.Int32, ObjCustMasterRow.Company_ID);
                        db.AddInParameter(command, "@PANum", DbType.String, ObjCustMasterRow.PANum);
                        db.FunPubLoadDataSet(command, dsCustMaster, dsCustMaster.S3G_LOANAD_GetDICustomerDetails.TableName);
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsCustMaster.S3G_LOANAD_GetDICustomerDetails;

            }

            #region Create DeliveryIns
            public int FunPubCreateDeliveryIns(SerializationMode SerMode, byte[] bytesObjS3G_ORG_DeliveryInsMasterDataTable, out string strDeliveryNumber_Out)
            {
                strDeliveryNumber_Out = string.Empty;
                try
                {

                    ObjDeliveryIns_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_DeliveryInsMasterDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionRow ObjDeliveryInsRow in ObjDeliveryIns_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertDeliveryInstruction");

                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjDeliveryInsRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjDeliveryInsRow.Branch_ID);
                        db.AddInParameter(command, "@COMPANY_ID", DbType.Int64, ObjDeliveryInsRow.Company_ID);
                        db.AddInParameter(command, "@PANum", DbType.String, ObjDeliveryInsRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, ObjDeliveryInsRow.SANum);
                        db.AddInParameter(command, "@IS_LPO", DbType.Boolean, ObjDeliveryInsRow.IS_LPO);
                        db.AddInParameter(command, "@Customer_ID", DbType.String, ObjDeliveryInsRow.Customer_ID);
                        db.AddInParameter(command, "@Vendor_ID", DbType.String, ObjDeliveryInsRow.Vendor_ID);
                        db.AddInParameter(command, "@DeliveryInstruction_Date", DbType.DateTime, ObjDeliveryInsRow.DeliveryInstruction_Date);

                        /*Parameter name changed for Oracle conversion - Kuppusamy.B - 22-Feb-2012*/
                        //db.AddInParameter(command, "@DeliveryInstruction_Statustype_Code", DbType.Int32, ObjDeliveryInsRow.DeliveryInstruction_Statustype_Code);
                        //db.AddInParameter(command, "@DeliveryInstruction_Status_Code", DbType.Int32, ObjDeliveryInsRow.DeliveryInstruction_Status_Code);
                        db.AddInParameter(command, "@DI_Statustype_Code", DbType.Int32, ObjDeliveryInsRow.DeliveryInstruction_Statustype_Code);
                        db.AddInParameter(command, "@DI_Status_Code", DbType.Int32, ObjDeliveryInsRow.DeliveryInstruction_Status_Code);
                        
                        db.AddInParameter(command, "@Created_BY", DbType.Int32, ObjDeliveryInsRow.Created_By);
                        db.AddInParameter(command, "@Created_ON", DbType.DateTime, ObjDeliveryInsRow.Created_On);
                        db.AddInParameter(command, "@TXN_Id", DbType.Int32, ObjDeliveryInsRow.TXN_Id);
                        db.AddInParameter(command, "@XML_DeliveryDeltails", DbType.String, ObjDeliveryInsRow.XML_DeliveryDeltails);
                        db.AddOutParameter(command, "@Delivery_No", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command,ref trans);
                                strDeliveryNumber_Out = (string)command.Parameters["@Delivery_No"].Value;
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


            #region Cancel DeliveryInstuction
            public int FunCancelDeliveryIns(int DeliveryInstruction_ID, string Flag, out int ErrorCode)
            {
                int intRowsAffected = 0;
                ErrorCode = 0;
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                try
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_CancelDeliveryInstruction");
                    db.AddInParameter(command, "@DeliveryInstruction_ID", DbType.Int32, DeliveryInstruction_ID);
                    db.AddInParameter(command, "@Flag", DbType.String, Flag);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command, ref trans);
                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            {
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            }

                            if (intRowsAffected == 0)
                            {
                                trans.Commit();
                            }
                            else
                            {
                                //trans.Rollback();
                                trans.Commit();

                            }
                               

                            
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction. 
                            intRowsAffected = 50;
                            trans.Rollback();
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                        }
                        conn.Close();
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

            #region Purchase Order
            public int FunPubUpdatePO(SerializationMode SerMode, byte[] bytesObjS3G_ORG_DeliveryInsMasterDataTable )
            {
                try
                {

                    ObjDeliveryIns_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_DeliveryInsMasterDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable));

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionRow ObjDeliveryInsRow in ObjDeliveryIns_DAL.Rows)
                    {
                        DbCommand command = null;
                        if (ObjDeliveryInsRow.PO_Type == 1)
                        {
                            command = db.GetStoredProcCommand("S3G_Org_Update_PO_dtl_MPO");
                        }
                        else
                        {
                            command = db.GetStoredProcCommand("S3G_Org_Update_PO_dtl");
                        }

                        db.AddInParameter(command, "@PO_Hdr_ID", DbType.String, ObjDeliveryInsRow.DeliveryInstruction_ID);
                        db.AddInParameter(command, "@Entity_ID", DbType.String, ObjDeliveryInsRow.Vendor_ID);
                        db.AddInParameter(command, "@XML_PODeltails", DbType.String, ObjDeliveryInsRow.XML_DeliveryDeltails);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjDeliveryInsRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
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
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }

            public int FunPubSavePO(SerializationMode SerMode, byte[] bytesObjS3G_ORG_DeliveryInsMasterDataTable, out string strPO_No)
            {
                strPO_No = String.Empty;
                try
                {
                    ObjDeliveryIns_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_DeliveryInsMasterDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable));

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionRow ObjDeliveryInsRow in ObjDeliveryIns_DAL.Rows)
                    {
                        DbCommand command = null;
                        
                        if (ObjDeliveryInsRow.PO_Type==1)
                        {
                            command = db.GetStoredProcCommand("S3G_Org_InsUpd_PO_dtl_MPO");
                        }
                        else
                        {
                            command = db.GetStoredProcCommand("S3G_Org_InsUpd_PO_dtl");
                        }
                        db.AddInParameter(command, "@PO_Hdr_ID", DbType.Int32, ObjDeliveryInsRow.DeliveryInstruction_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjDeliveryInsRow.Company_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjDeliveryInsRow.Customer_ID);
                        db.AddInParameter(command, "@Entity_ID", DbType.Int32, ObjDeliveryInsRow.Vendor_ID);
                        db.AddInParameter(command, "@XML_PODeltails", DbType.String, ObjDeliveryInsRow.XML_DeliveryDeltails);
                        db.AddInParameter(command, "@Payment_Terms", DbType.String, ObjDeliveryInsRow.Payment_Terms);
                        db.AddInParameter(command, "@Delivery_Terms", DbType.String, ObjDeliveryInsRow.Delivery_Terms);
                        db.AddInParameter(command, "@Warranty_Terms", DbType.String, ObjDeliveryInsRow.Warranty_Terms);
                        db.AddInParameter(command, "@Notes1", DbType.String, ObjDeliveryInsRow.Notes1);
                        db.AddInParameter(command, "@Notes2", DbType.String, ObjDeliveryInsRow.Notes2);
                        db.AddInParameter(command, "@Others", DbType.String, ObjDeliveryInsRow.Others);
                        db.AddInParameter(command, "@EndUseCustomer_Name", DbType.String, ObjDeliveryInsRow.EndUseCustomer_Name);
                        db.AddInParameter(command, "@Customer_PO_Ref_No", DbType.String, ObjDeliveryInsRow.Customer_PO_Ref_No);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjDeliveryInsRow.Created_By);
                        db.AddInParameter(command, "@Location_Code", DbType.String, ObjDeliveryInsRow.Location_Code);
                        db.AddOutParameter(command, "@PO_No", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                strPO_No = command.Parameters["@PO_No"].Value == DBNull.Value ? String.Empty : ((string)command.Parameters["@PO_No"].Value).ToString();
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
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }

            public int FunPubCancelPO(int intPO_Hdr_ID, int intVendorGroup, string strReason_For_Cancellation, out int ErrorCode)
            {
                int intRowsAffected = 0;
                ErrorCode = 0;

                try
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_Org_Cancel_PO_dtl");
                    db.AddInParameter(command, "@PO_Hdr_ID", DbType.Int32, intPO_Hdr_ID);
                    db.AddInParameter(command, "@Vendor_Group", DbType.Int32, intVendorGroup);
                    db.AddInParameter(command, "@Reason_For_Cancellation", DbType.String, strReason_For_Cancellation);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command, ref trans);
                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            {
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            }

                            if (intRowsAffected == 0)
                            {
                                trans.Commit();
                            }
                            else
                            {
                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction. 
                            intRowsAffected = 50;
                            trans.Rollback();
                            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }

            public int FunPubCancel_MPO(int intPO_Hdr_ID, int intVendorGroup, string strReason_For_Cancellation, out int ErrorCode)
            {
                int intRowsAffected = 0;
                ErrorCode = 0;

                try
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_Org_Cancel_PO_dtl_MPO");
                    db.AddInParameter(command, "@PO_Hdr_ID", DbType.Int32, intPO_Hdr_ID);
                    db.AddInParameter(command, "@Vendor_Group", DbType.Int32, intVendorGroup);
                    db.AddInParameter(command, "@Reason_For_Cancellation", DbType.String, strReason_For_Cancellation);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command, ref trans);
                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            {
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            }

                            if (intRowsAffected == 0)
                            {
                                trans.Commit();
                            }
                            else
                            {
                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction. 
                            intRowsAffected = 50;
                            trans.Rollback();
                            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }

            public int FunPubSavetPO_Remin_EMails(string strPODetails, out int ErrorCode)
            {
                int intRowsAffected = 0;
                ErrorCode = 0;

                try
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_Org_Insert_PO_Remin_dtl_Email");
                    db.AddInParameter(command, "@xml_PO_dtl", DbType.String, strPODetails);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command, ref trans);
                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            {
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            }

                            if (intRowsAffected == 0)
                            {
                                trans.Commit();
                            }
                            else
                            {
                                trans.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction. 
                            intRowsAffected = 50;
                            trans.Rollback();
                            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }

            public int FunPubCancelMultiplePO(string strPONumber, int intOption,string strReason_For_Cancellation, out int ErrorCode, out string strPONumberOut)
            {
                int intRowsAffected = 0;
                ErrorCode = 0;
                strPONumberOut = "";

                try
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_Org_Cancel_Multiple_PO");
                    db.AddInParameter(command, "@PO_Number", DbType.String, strPONumber);
                    db.AddInParameter(command, "@Option", DbType.Int32, intOption);
                    db.AddInParameter(command, "@Reason_For_Cancellation", DbType.String, strReason_For_Cancellation);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@PO_Out", DbType.String, -1);
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command, ref trans);
                            strPONumberOut = command.Parameters["@PO_Out"].Value == DBNull.Value ? String.Empty : ((string)command.Parameters["@PO_Out"].Value).ToString();
                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            {
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            }

                            if (intRowsAffected == 0)
                            {
                                trans.Commit();
                            }
                            else
                            {
                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction. 
                            intRowsAffected = 50;
                            trans.Rollback();
                            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        conn.Close();
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

            #region PI

            public int FunPubUpdatePI(SerializationMode SerMode, byte[] bytesObjS3G_ORG_DeliveryInsMasterDataTable)
            {
                try
                {

                    ObjDeliveryIns_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_DeliveryInsMasterDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable));

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionRow ObjDeliveryInsRow in ObjDeliveryIns_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Org_Update_PI_dtl");
                        db.AddInParameter(command, "@PI_Hdr_ID", DbType.String, ObjDeliveryInsRow.DeliveryInstruction_ID);
                        db.AddInParameter(command, "@XML_PIDeltails", DbType.String, ObjDeliveryInsRow.XML_DeliveryDeltails);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
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
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }

            public int FunPubSavePI(SerializationMode SerMode, byte[] bytesObjS3G_ORG_DeliveryInsMasterDataTable, out string strLSQ_No)
            {
                strLSQ_No = String.Empty;
                try
                {

                    ObjDeliveryIns_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_DeliveryInsMasterDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable));

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionRow ObjDeliveryInsRow in ObjDeliveryIns_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Org_InsUpd_PI_dtl");
                        db.AddInParameter(command, "@PI_Hdr_ID", DbType.Int32, ObjDeliveryInsRow.DeliveryInstruction_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjDeliveryInsRow.Company_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjDeliveryInsRow.Customer_ID);
                        db.AddInParameter(command, "@PI_Number", DbType.String, ObjDeliveryInsRow.DeliveryInstruction_No);
                        db.AddInParameter(command, "@PI_Date", DbType.String, ObjDeliveryInsRow.DeliveryInstruction_Date);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjDeliveryInsRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@LSQ_Number", DbType.String, 100);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                strLSQ_No = (string)command.Parameters["@LSQ_Number"].Value;
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
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }

            public int FunPubCancelPI(int intPI_Hdr_ID, string PI_No, string PO_No, out int ErrorCode)
            {
                int intRowsAffected = 0;
                ErrorCode = 0;

                try
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_Org_Cancel_PI_dtl");
                    db.AddInParameter(command, "@PI_Hdr_ID", DbType.Int32, intPI_Hdr_ID);
                    db.AddInParameter(command, "@PI_Number", DbType.String, PI_No);
                    db.AddInParameter(command, "@PO_Number", DbType.String, PO_No);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command, ref trans);
                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            {
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            }

                            if (intRowsAffected == 0)
                            {
                                trans.Commit();
                            }
                            else
                            {
                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction. 
                            intRowsAffected = 50;
                            trans.Rollback();
                            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        conn.Close();
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

            #region VI
            public int FunPubUpdateVI(SerializationMode SerMode, byte[] bytesObjS3G_ORG_DeliveryInsMasterDataTable)
            {
                try
                {

                    ObjDeliveryIns_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_DeliveryInsMasterDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable));

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionRow ObjDeliveryInsRow in ObjDeliveryIns_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Org_Update_VI_dtl");
                        db.AddInParameter(command, "@VI_Hdr_ID", DbType.String, ObjDeliveryInsRow.DeliveryInstruction_ID);
                        db.AddInParameter(command, "@XML_VIDeltails", DbType.String, ObjDeliveryInsRow.XML_DeliveryDeltails);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
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
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }

            public int FunPubSaveVI(SerializationMode SerMode, byte[] bytesObjS3G_ORG_DeliveryInsMasterDataTable, out string strLSQ_No)
            {
                strLSQ_No = String.Empty;
                try
                {

                    ObjDeliveryIns_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_DeliveryInsMasterDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable));

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionRow ObjDeliveryInsRow in ObjDeliveryIns_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Org_InsUpd_VI_dtl");
                        db.AddInParameter(command, "@VI_Hdr_ID", DbType.Int32, ObjDeliveryInsRow.DeliveryInstruction_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjDeliveryInsRow.Company_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjDeliveryInsRow.Customer_ID);
                        db.AddInParameter(command, "@VI_Number", DbType.String, ObjDeliveryInsRow.DeliveryInstruction_No);
                        db.AddInParameter(command, "@VI_Date", DbType.String, ObjDeliveryInsRow.DeliveryInstruction_Date);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjDeliveryInsRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@LSQ_Number", DbType.String, 100);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                strLSQ_No = (string)command.Parameters["@LSQ_Number"].Value;
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
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }

            public int FunPubCancelVI(int intVI_Hdr_ID, string VI_No, string PO_No, out int ErrorCode)
            {
                int intRowsAffected = 0;
                ErrorCode = 0;

                try
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_Org_Cancel_VI_dtl");
                    db.AddInParameter(command, "@VI_Hdr_ID", DbType.Int32, intVI_Hdr_ID);
                    db.AddInParameter(command, "@VI_Number", DbType.String, VI_No);
                    db.AddInParameter(command, "@PO_Number", DbType.String, PO_No);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command, ref trans);
                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            {
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            }

                            if (intRowsAffected == 0)
                            {
                                trans.Commit();
                            }
                            else
                            {
                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction. 
                            intRowsAffected = 50;
                            trans.Rollback();
                            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        conn.Close();
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
        }
    }
}

