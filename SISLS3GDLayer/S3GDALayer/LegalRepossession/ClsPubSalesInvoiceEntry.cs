#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Legal & Reposession
/// Screen Name			: Sales Invoice Entry DAL Class
/// Created By			: Srivatsan.S
/// Created Date		: 04-May-2011
/// Purpose	            : This class file acts as a data access layer containing the necessary functions for Sales Invoice Entry
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
#endregion
#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using LegalModule = S3GBusEntity.LegalRepossession;
#endregion
namespace S3GDALayer.LegalRepossession
{
    namespace LegalAndRepossessionMgtServices
    {
        public class ClsPubSalesInvoiceEntry : ClsPubDalLegalRepossessionBase
        {
            #region Initialization
            LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleInvoiceDataTable objSalesInvoiceEntryDatatable_DAL = null;
            LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleInvoiceRow objSalesInvoiceRow = null;
            int intErrorCode;
            int intRowsAffected;
            #endregion

            public int FunPubInsertSalesInvoiceEntry(SerializationMode SerMode, byte[] bytesObjSalesInvoiceEntryDataTable, out string DSN)
            {
                DSN = "";
                try
                {
                    objSalesInvoiceEntryDatatable_DAL = (LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleInvoiceDataTable)ClsPubSerialize.DeSerialize(bytesObjSalesInvoiceEntryDataTable, SerializationMode.Binary, typeof(LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleInvoiceDataTable));
                    objSalesInvoiceRow = objSalesInvoiceEntryDatatable_DAL.Rows[0] as LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleInvoiceRow;
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_InsertSaleInvoice);
                    db.AddInParameter(command, "@Sale_Invoice_ID", DbType.Int32, objSalesInvoiceRow.Sale_Invoice_ID);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, objSalesInvoiceRow.Company_ID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, objSalesInvoiceRow.LOB_ID);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, objSalesInvoiceRow.Branch_ID);
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, objSalesInvoiceRow.Customer_ID);
                    db.AddInParameter(command, "@LRN_No", DbType.String, objSalesInvoiceRow.LRN_No);
                    db.AddInParameter(command, "@Sale_Notification_ID", DbType.String, objSalesInvoiceRow.Sale_Notification_ID);
                    db.AddInParameter(command, "@SIE_Date", DbType.DateTime, objSalesInvoiceRow.SIE_Date);
                    db.AddInParameter(command, "@Reposssion_Docket_No", DbType.String, objSalesInvoiceRow.Reposssion_Docket_No);
                    db.AddInParameter(command, "@Reposssion_Docket_id", DbType.Int32, objSalesInvoiceRow.Reposssion_Docket_id);
                    db.AddInParameter(command, "@Sale_Notification_Asset_ID", DbType.Int32, objSalesInvoiceRow.Sale_Notification_Asset_ID);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, objSalesInvoiceRow.Created_By);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, objSalesInvoiceRow.Modified_By);
                    db.AddInParameter(command, "@XMLINVDETAILS", DbType.String, objSalesInvoiceRow.XMLInvoiceDetails);
                    db.AddInParameter(command, "@XMLRECEIPTDETAILS", DbType.String, objSalesInvoiceRow.XMLReceiptDetails);
                    db.AddOutParameter(command, "@ERRORCODE", DbType.Int32, sizeof(Int32));
                    db.AddOutParameter(command, "@SIE_NO", DbType.String, 25);
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            //Modified by chandru on 26/03/2012
                            db.FunPubExecuteNonQuery(command, ref trans);
                            //Modified by chandru on 26/03/2012
                            DSN = (string)command.Parameters["@SIE_NO"].Value;
                            if ((int)command.Parameters["@ERRORCODE"].Value > 0)
                            {
                                intRowsAffected = (int)command.Parameters["@ERRORCODE"].Value;
                                throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                            }
                            else if ((int)command.Parameters["@ERRORCODE"].Value < 0)
                            {
                                intRowsAffected = (int)command.Parameters["@ERRORCODE"].Value;
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
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;

            }

            public int FunPubUpdateSalesInvoiceEntry(SerializationMode SerMode, byte[] bytesObjSalesInvoiceEntryDataTable)
            {
                try
                {
                    objSalesInvoiceEntryDatatable_DAL = (LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleInvoiceDataTable)ClsPubSerialize.DeSerialize(bytesObjSalesInvoiceEntryDataTable, SerializationMode.Binary, typeof(LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleInvoiceDataTable));
                    objSalesInvoiceRow = objSalesInvoiceEntryDatatable_DAL.Rows[0] as LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleInvoiceRow;
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_UpdateSaleInvoice);
                    db.AddInParameter(command, "@Sale_Invoice_ID", DbType.Int32, objSalesInvoiceRow.Sale_Invoice_ID);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, objSalesInvoiceRow.Company_ID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, objSalesInvoiceRow.LOB_ID);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, objSalesInvoiceRow.Branch_ID);
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, objSalesInvoiceRow.Customer_ID);
                    db.AddInParameter(command, "@LRN_No", DbType.String, objSalesInvoiceRow.LRN_No);
                    db.AddInParameter(command, "@Sale_Notification_ID", DbType.Int32, objSalesInvoiceRow.Sale_Notification_ID);
                    db.AddInParameter(command, "@SIE_Date", DbType.DateTime, objSalesInvoiceRow.SIE_Date);
                    db.AddInParameter(command, "@Reposssion_Docket_No", DbType.String, objSalesInvoiceRow.Reposssion_Docket_No);
                    db.AddInParameter(command, "@Sale_Notification_Asset_ID", DbType.Int32, objSalesInvoiceRow.Sale_Notification_Asset_ID);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, objSalesInvoiceRow.Created_By);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, objSalesInvoiceRow.Modified_By);
                    db.AddInParameter(command, "@XMLINVDETAILS", DbType.String, objSalesInvoiceRow.XMLInvoiceDetails);
                    db.AddInParameter(command, "@XMLRECEIPTDETAILS", DbType.String, objSalesInvoiceRow.XMLReceiptDetails);
                    db.AddInParameter(command, "@IS_ACTIVE", DbType.String, objSalesInvoiceRow.Is_Active.ToString());
                    db.AddOutParameter(command, "@ERRORCODE", DbType.Int32, sizeof(Int32));
                   using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            //Modified by chandru on 26/03/2012
                            db.FunPubExecuteNonQuery(command, ref trans);
                            //Modified by chandru on 26/03/2012
                            //if ((int)command.Parameters["@ERRORCODE"].Value > 0)
                                intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;
                            if (intErrorCode == 0)
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
                            trans.Rollback();
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;

            }

            public DataSet FunGetSalesInvoiceDetails(int CompanyID, int SalesInvoiceID)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_GetSaleInvoiceDetails);
                    db.AddInParameter(command, "@Company_id", DbType.Int32, CompanyID);
                    db.AddInParameter(command, "@Sale_Invoice_ID", DbType.Int32, SalesInvoiceID);
                    //Modified by chandru on 26/03/2012
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    //Modified by chandru on 26/03/2012
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public DataSet FunGetSaleBidNotificationDetails(int CompanyID, int BranchID,int LOBID, int SalesNotificationID,out int SaleBidID)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_GetSaleBidNotificationDetails);
                    db.AddInParameter(command, "@Company_id", DbType.Int32, CompanyID);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, BranchID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, LOBID);
                    db.AddInParameter(command, "@SN_ID", DbType.Int32, SalesNotificationID);
                    db.AddOutParameter(command, "@SALE_BID_ID", DbType.Int32,sizeof(Int32));
                    //Modified by chandru on 26/03/2012
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    //Modified by chandru on 26/03/2012
                    SaleBidID = Convert.ToInt32(command.Parameters["@SALE_BID_ID"].Value.ToString());
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public DataSet FunGetSaleDebCredGLCode(int CompanyID, string AccCode, int Mode)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_GetGLCodeOnAccCode);
                    db.AddInParameter(command, "@Company_Id", DbType.Int32, CompanyID);
                    db.AddInParameter(command, "@AccCode", DbType.String, AccCode);
                    db.AddInParameter(command, "@Mode", DbType.Int32, Mode);
                    //Modified by chandru on 26/03/2012
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    //Modified by chandru on 26/03/2012
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public DataSet FunGetSaleDebCredSLCode(int CompanyID, string GLCode, int LOBID,int BranchID)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_GetSLCodeOnGLCode);
                    db.AddInParameter(command, "@Company_Id", DbType.Int32, CompanyID);
                    db.AddInParameter(command, "@GL_CODE", DbType.String,GLCode);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, LOBID);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, BranchID);
                    //Modified by chandru on 26/03/2012
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    //Modified by chandru on 26/03/2012
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
