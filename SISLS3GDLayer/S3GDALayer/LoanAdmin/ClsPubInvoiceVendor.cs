#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: LoanAdmin
/// Screen Name			: Invoice Vendor DAL class
/// Created By			: Kaliraj K
/// Created Date		: 19-Jul-2010
/// Purpose	            : To access Invoice vendor db methods

/// <Program Summary>
#endregion
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
        public class ClsPubInvoiceVendor
        {
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InvoiceVendorDetailsDataTable ObjInvoice_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubInvoiceVendor()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateInvoiceDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_InvoiceDataTable, ClsSystemJournal ObjSysJournal,out string strErrMsg)
            {
                try
                {
                    strErrMsg=string.Empty;
                    ObjInvoice_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InvoiceVendorDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_InvoiceDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InvoiceVendorDetailsDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_InvoiceVendorDetailsRow ObjInvoiceRow in ObjInvoice_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("[S3G_LOANAD_InsertInvoiceVendorDetails]");
                        db.AddInParameter(command, "@Invoice_ID", DbType.Int32, ObjInvoiceRow.Invoice_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjInvoiceRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjInvoiceRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjInvoiceRow.Branch_ID);
                        db.AddInParameter(command, "@Vendor_ID", DbType.Int32, ObjInvoiceRow.Vendor_ID);
                        //db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjInvoiceRow.Customer_Code);
                        db.AddInParameter(command, "@Ref_Doc_Type", DbType.Int32, ObjInvoiceRow.Ref_Docu_Type);
                        db.AddInParameter(command, "@Ref_Doc_No", DbType.Int32, ObjInvoiceRow.Ref_Docu_No);
                        db.AddInParameter(command, "@Vendor_Invoice_No", DbType.String, ObjInvoiceRow.Vendor_Invoice_No);
                        db.AddInParameter(command, "@Vendor_Invoice_Date", DbType.DateTime, ObjInvoiceRow.Vendor_Invoice_Date);
                        db.AddInParameter(command, "@Book_in_Asset", DbType.Boolean, ObjInvoiceRow.IsBookin_Asset);
                        db.AddInParameter(command, "@Invoice_Image", DbType.String, ObjInvoiceRow.Invoice_Image);
                        db.AddInParameter(command, "@TaxReg_No", DbType.String, ObjInvoiceRow.TaxReg_No);
                        db.AddInParameter(command, "@VAT_No", DbType.String, ObjInvoiceRow.Vat_No);
                        db.AddInParameter(command, "@XMLAssetDetails", DbType.String, ObjInvoiceRow.XMLAssetDetails);
                        db.AddInParameter(command, "@XMLWarrantyDetails", DbType.String, ObjInvoiceRow.XMLWarrantyDetails);
                        db.AddInParameter(command, "@Ref_Doc_SLA", DbType.String, ObjInvoiceRow.Ref_Doc_SLA);

                        //Sys Journal Parameter Declaration
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjInvoiceRow.Customer_Code);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjInvoiceRow.Created_By);
                        db.AddInParameter(command, "@Invoice_Type", DbType.String, ObjInvoiceRow.Invoice_Type);

                        db.AddInParameter(command, "@Narration", DbType.String, ObjSysJournal.Narration);
                        db.AddInParameter(command, "@Value_Date", DbType.String, ObjSysJournal.Value_Date);
                        db.AddInParameter(command, "@Txn_Currency_Code", DbType.String, ObjSysJournal.Txn_Currency_Code);
                        db.AddInParameter(command, "@Global_Dimension1_No", DbType.Int32, ObjSysJournal.Global_Dimension1_No);
                        db.AddInParameter(command, "@JV_Status_Code", DbType.Int32, ObjSysJournal.JV_Status_Code);
                        db.AddInParameter(command, "@Reference_Number", DbType.Int32, ObjSysJournal.Reference_Number);
                        db.AddInParameter(command, "@XMLSysJournal", DbType.String, ObjSysJournal.XMLSysJournal);
                        
                        //db.AddInParameter(command, "@Ref_Doc_SLA", DbType.String, ObjSysJournal.XMLSysJournal);
                        

                        //Sys Journal Code end

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);
                        

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command,ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value != 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                                    trans.Rollback();
                                }
                                else
                                {
                                    strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                            //    if (command.Parameters["@ErrorCode"].Value!=null)
                            //    {
                            //        intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            //        strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                            //    }
                            //    else if (intRowsAffected==0)
                            //    {
                            //        intRowsAffected = 50;
                            //    }
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
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;
            }
        }
    }
}
