#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Legal & Reposession
/// Screen Name			: Sales Bid Entry DAL Class
/// Created By			: Srivatsan.S
/// Created Date		: 28-April-2011
/// Purpose	            : This class file acts as a data access layer containing the necessary functions for Sales Bid Entry
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
        public class ClsPubSalesBidEntry : ClsPubDalLegalRepossessionBase
       {
           #region Initialization
           LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleBidDataTable objSalesBidEntryDatatable_DAL = null;
           LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleBidRow objSalesBidEntryRow = null;
           int intErrorCode;
           int intRowsAffected;
           #endregion

           public int FunPubInsertSalesBidEntry(SerializationMode SerMode, byte[] bytesObjSalesBidEntryDataTable, out string DSN)
           {
               DSN = "";
               try
               {
                   objSalesBidEntryDatatable_DAL =(LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleBidDataTable)ClsPubSerialize.DeSerialize(bytesObjSalesBidEntryDataTable, SerializationMode.Binary,typeof(LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleBidDataTable));
                   objSalesBidEntryRow= objSalesBidEntryDatatable_DAL.Rows[0] as LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleBidRow;
                   //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                   DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_INSERTSALEBID);
                   db.AddInParameter(command, "@SALE_BID_ID", DbType.Int32, objSalesBidEntryRow.Sale_Bid_ID);
                   db.AddInParameter(command, "@COMPANY_ID", DbType.Int32,objSalesBidEntryRow.Company_ID);
                   db.AddInParameter(command, "@LOB_ID", DbType.Int32, objSalesBidEntryRow.LOB_ID);
                   db.AddInParameter(command, "@LOCATION_ID", DbType.Int32, objSalesBidEntryRow.Branch_ID);
                   db.AddInParameter(command, "@CUSTOMER_ID", DbType.Int32, objSalesBidEntryRow.Customer_ID);
                   db.AddInParameter(command, "@SALE_NOTIFICATION_ID", DbType.Int32, objSalesBidEntryRow.Sale_Notification_ID);
                   db.AddInParameter(command, "@PANUM", DbType.String, objSalesBidEntryRow.PANum);
                   db.AddInParameter(command, "@SANUM", DbType.String, objSalesBidEntryRow.SANum);
                   db.AddInParameter(command, "@IS_ACTIVE", DbType.String, objSalesBidEntryRow.Is_Active.ToString());
                   db.AddInParameter(command, "@CREATED_BY", DbType.Int32, objSalesBidEntryRow.Created_By);
                   db.AddInParameter(command, "@MODIFIED_BY", DbType.Int32, objSalesBidEntryRow.Modified_By);
                   db.AddInParameter(command, "@XMLBIDDETAILS", DbType.String, objSalesBidEntryRow.XMLBIDDETAILS);
                   db.AddOutParameter(command, "@ERRORCODE", DbType.Int32, sizeof(Int32));
                   db.AddOutParameter(command, "@SALE_BID_NO", DbType.String,25);
                   using (DbConnection conn= db.CreateConnection())
                   {
                       conn.Open();
                       DbTransaction trans = conn.BeginTransaction();
                       try
                       {
                           //Modified by chandru on 26/03/2012
                           db.FunPubExecuteNonQuery(command, ref trans);
                           //Modified by chandru on 26/03/2012
                           DSN = (string)command.Parameters["@SALE_BID_NO"].Value;
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

           public int FunPubUpdateSalesBidEntry(SerializationMode SerMode, byte[] bytesObjSalesBidEntryDataTable)
           {
             try
               {
                   objSalesBidEntryDatatable_DAL = (LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleBidDataTable)ClsPubSerialize.DeSerialize(bytesObjSalesBidEntryDataTable, SerializationMode.Binary, typeof(LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleBidDataTable));
                   objSalesBidEntryRow = objSalesBidEntryDatatable_DAL.Rows[0] as LegalModule.LegalRepossessionMgtServices.S3G_LR_SaleBidRow;
                   //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                   DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_UPDATESALEBID);
                   db.AddInParameter(command, "@SALE_BID_ID", DbType.Int32, objSalesBidEntryRow.Sale_Bid_ID);
                   db.AddInParameter(command, "@COMPANY_ID", DbType.Int32, objSalesBidEntryRow.Company_ID);
                   db.AddInParameter(command, "@LOB_ID", DbType.Int32, objSalesBidEntryRow.LOB_ID);
                   db.AddInParameter(command, "@LOCATION_ID", DbType.Int32, objSalesBidEntryRow.Branch_ID);
                   db.AddInParameter(command, "@CUSTOMER_ID", DbType.Int32, objSalesBidEntryRow.Customer_ID);
                   db.AddInParameter(command, "@SALE_NOTIFICATION_ID", DbType.Int32, objSalesBidEntryRow.Sale_Notification_ID);
                   db.AddInParameter(command, "@PANUM", DbType.String, objSalesBidEntryRow.PANum);
                   db.AddInParameter(command, "@SANUM", DbType.String, objSalesBidEntryRow.SANum);
                   db.AddInParameter(command, "@IS_ACTIVE", DbType.String, objSalesBidEntryRow.Is_Active.ToString());
                   db.AddInParameter(command, "@CREATED_BY", DbType.Int32, objSalesBidEntryRow.Created_By);
                   db.AddInParameter(command, "@MODIFIED_BY", DbType.Int32, objSalesBidEntryRow.Modified_By);
                   db.AddInParameter(command, "@XMLBIDDETAILS", DbType.String, objSalesBidEntryRow.XMLBIDDETAILS);
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
                           //if ((int)command.Parameters["@ErrorCode"].Value > 0)
                               intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
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
           public DataSet FunGetSalesBidDetails(int CompanyID,int SalesBidID)
           {
               try
               {
                   DataSet ObjDS = new DataSet();
                   //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                   DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_GetSaleBidDetails);
                   db.AddInParameter(command, "@Company_id", DbType.Int32, CompanyID);
                   db.AddInParameter(command, "@Sale_Bid_ID", DbType.Int32, SalesBidID);
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