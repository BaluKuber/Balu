#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: LegalRepossession
/// Screen Name			: Sale Notification
/// Created By			: Irsathameen K
/// Created Date		: 28-Apr-2011
/// Purpose	            : To access Sale Notification db methods
/// Modified By			: 
/// Modified Date		:
/// Purpose	            : 
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
using System.Data.OracleClient;

namespace S3GDALayer.LegalRepossession
{
    namespace LegalAndRepossessionMgtServices
    {
        public class ClsPubSaleNotification : ClsPubDalLegalRepossessionBase
        {
             int intRowsAffected;
            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_SaleNotificationDetailsDataTable ObjSaleNotification_DAL;
            
            
            public int FunPubCreateSaleNotificationDetails(SerializationMode SerMode, byte[] bytesObjS3G_LR_SaleNotificationDataTable, out string strSNNo)
            {
                try
                {
                    
                    strSNNo = "";
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    ObjSaleNotification_DAL = (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices .S3G_LR_SaleNotificationDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LR_SaleNotificationDataTable, SerMode, typeof(S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_SaleNotificationDetailsDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_SaleNotificationDetailsRow ObjSaleNotificationRow in ObjSaleNotification_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LR_InsertSaleNotificationDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjSaleNotificationRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjSaleNotificationRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjSaleNotificationRow.Branch_ID);                     
                        db.AddInParameter(command, "@SNDate", DbType.DateTime, ObjSaleNotificationRow.SNDate);
                        db.AddInParameter(command, "@PANum", DbType.String, ObjSaleNotificationRow.PANum);                       
                        db.AddInParameter(command, "@SANum", DbType.String, ObjSaleNotificationRow.SANum);                      
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjSaleNotificationRow.Customer_ID);
                        db.AddInParameter(command, "@RepossNo", DbType.String, ObjSaleNotificationRow.RepossNo);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjSaleNotificationRow.Is_Active);

                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param;
                            param = new OracleParameter("@XMLSaleBidDetails",
                                OracleType.Clob, ObjSaleNotificationRow.XMLSaleBidDetails.Length,
                                ParameterDirection.Input, true, 0, 0, String.Empty,
                                DataRowVersion.Default, ObjSaleNotificationRow.XMLSaleBidDetails);
                            command.Parameters.Add(param);

                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLSaleBidDetails", DbType.String, ObjSaleNotificationRow.XMLSaleBidDetails);
                        }
                        db.AddInParameter(command, "@SN_ID", DbType.Int32, ObjSaleNotificationRow.SN_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjSaleNotificationRow.Created_By);
                        db.AddOutParameter(command, "@SNNo", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));                       
                        //Added by Ponnurajesh on 24/march/2012 for Oracle conversion
                        //string ErrorCode = command.Parameters[command.Parameters.Count - 1].ParameterName;
                         using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);//Modified by Ponnurajesh on 24/March/2012 for Oracle conversion
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
                                else
                                {                                    
                                    trans.Commit();
                                    if(!string.IsNullOrEmpty(command.Parameters["@SNNo"].Value.ToString()) )
                                    strSNNo = (string)command.Parameters["@SNNo"].Value;
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
                            {  conn.Close();   }
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
