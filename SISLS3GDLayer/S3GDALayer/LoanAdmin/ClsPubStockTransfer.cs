using System;
using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity.Reports;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Data;
using S3GDALayer.Constants;
using S3GBusEntity;


namespace S3GDALayer.LoanAdmin
{
    public class ClsPubStockTransfer
    {
        int intRowsAffected = 0;
        S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LAD_Stock_MstDataTable ObjStockMasterDataTable = null;

        //Code added for getting common connection string  from config file
        Database db;
        public ClsPubStockTransfer()
        {
            db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
        }

        public int FunPubCreateStockMaster(SerializationMode SerMode, byte[] bytesObjStockMasterDetails_DataTable, out Int32 intStock, out string strErrorMsg, out string StrDocumentNumber)
        {
            strErrorMsg = "";
            intStock = 0;
            StrDocumentNumber = "";
            try
            {

                ObjStockMasterDataTable = (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LAD_Stock_MstDataTable)ClsPubSerialize.DeSerialize(bytesObjStockMasterDetails_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LAD_Stock_MstDataTable));

                foreach (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LAD_Stock_MstRow ObjInvoiceGroupMasterDataRow in ObjStockMasterDataTable.Rows)
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_LAD_Ins_StockMst");
                    db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjInvoiceGroupMasterDataRow.Company_Id);
                    db.AddInParameter(command, "@Stock_Hdr_Id", DbType.Int32, ObjInvoiceGroupMasterDataRow.Stock_Hdr_Id);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjInvoiceGroupMasterDataRow.Created_By);
                    db.AddInParameter(command, "@Customer_Id", DbType.Int32, ObjInvoiceGroupMasterDataRow.Customer_id);
                    db.AddInParameter(command, "@Vendor_Id", DbType.Int32, ObjInvoiceGroupMasterDataRow.Vendor_Id);
                    db.AddInParameter(command, "@From_Delivery_State", DbType.Int32, ObjInvoiceGroupMasterDataRow.From_Delivery_State);
                    db.AddInParameter(command, "@To_Delivery_State", DbType.Int32, ObjInvoiceGroupMasterDataRow.To_Delivery_State);
                    if (!ObjInvoiceGroupMasterDataRow.IsInvoice_From_DateNull())
                        db.AddInParameter(command, "@Invoice_From_Date", DbType.DateTime, ObjInvoiceGroupMasterDataRow.Invoice_From_Date);
                    if (!ObjInvoiceGroupMasterDataRow.IsInvoice_To_DateNull())
                    db.AddInParameter(command, "@Invoice_To_Date", DbType.DateTime, ObjInvoiceGroupMasterDataRow.Invoice_To_Date);
                    db.AddInParameter(command, "@Invoice_Number", DbType.String, ObjInvoiceGroupMasterDataRow.Invoice_Number);
                    db.AddInParameter(command, "@Invoice_Type", DbType.String, ObjInvoiceGroupMasterDataRow.Invoice_Type);
                    db.AddInParameter(command, "@PIVI_hdr_Id", DbType.Int32, ObjInvoiceGroupMasterDataRow.PIVI_Hdr_Id);
                    db.AddInParameter(command, "@pa_sa_ref_Id", DbType.Int32, ObjInvoiceGroupMasterDataRow.Pa_sa_Ref_Id);
                    db.AddInParameter(command, "@document_Date", DbType.DateTime, ObjInvoiceGroupMasterDataRow.Document_Date);
                    db.AddInParameter(command, "@Type", DbType.String, ObjInvoiceGroupMasterDataRow.Type);

                    db.AddInParameter(command, "@Ship_From", DbType.String, ObjInvoiceGroupMasterDataRow.Ship_From);
                    db.AddInParameter(command, "@Ship_To", DbType.String, ObjInvoiceGroupMasterDataRow.Ship_To);

                    db.AddInParameter(command, "@Ship_From_GSTIN", DbType.String, ObjInvoiceGroupMasterDataRow.Ship_From_GSTIN);
                    db.AddInParameter(command, "@Ship_To_GSTIN", DbType.String, ObjInvoiceGroupMasterDataRow.Ship_To_GSTIN);

                    db.AddInParameter(command, "@Ship_From_State", DbType.Int32, ObjInvoiceGroupMasterDataRow.Ship_From_State);
                    db.AddInParameter(command, "@Ship_To_State", DbType.Int32, ObjInvoiceGroupMasterDataRow.Ship_To_State);
                    db.AddInParameter(command, "@Ship_From_Pin", DbType.String, ObjInvoiceGroupMasterDataRow.Ship_From_Pin);
                    db.AddInParameter(command, "@Ship_To_Pin", DbType.String, ObjInvoiceGroupMasterDataRow.Ship_To_Pin);

                    db.AddInParameter(command, "@Note", DbType.String, ObjInvoiceGroupMasterDataRow.Note);

                    db.AddInParameter(command, "@To_Customer_Id", DbType.Int32, ObjInvoiceGroupMasterDataRow.To_Customer_Id);

                    db.AddInParameter(command, "@Transfer_Type", DbType.Int32, ObjInvoiceGroupMasterDataRow.Transfer_Type);

                    db.AddInParameter(command, "@Xml_Stock_Details", DbType.String, ObjInvoiceGroupMasterDataRow.Xml_Stock_Details);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                    // db.AddOutParameter(command, "@ErrorMsg", DbType.String, 1000);
                    db.AddOutParameter(command, "@Stock_ID_OUT", DbType.Int32, sizeof(Int32));
                    db.AddOutParameter(command, "@Document_number", DbType.String, 1000);
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
                                intStock = (int)command.Parameters["@Stock_ID_OUT"].Value;
                                StrDocumentNumber = (string)command.Parameters["@Document_number"].Value;

                                trans.Rollback();
                            }
                            else
                            {
                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            intRowsAffected = 50;
                            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            trans.Rollback();
                        }
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
    }
}
