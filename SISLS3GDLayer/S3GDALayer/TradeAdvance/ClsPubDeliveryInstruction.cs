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

namespace S3GDALayer.TradeAdvance
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
                        DbCommand command = db.GetStoredProcCommand("S3G_TA_InsertDeliveryInstruction");

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
                    DbCommand command = db.GetStoredProcCommand("S3G_TA_CancelDeliveryInstruction");
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

        }
    }
}

