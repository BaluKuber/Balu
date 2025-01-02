
using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORG = S3GBusEntity.Origination;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using S3GBusEntity;
using System.Data;


namespace S3GDALayer.LegalRepossession
{
    namespace LegalAndRepossessionMgtServices
    {
        public class ClsPubRepossession : ClsPubDalLegalRepossessionBase
        {
            
            int intRowsAffected = 0;
  
            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionDataTable ObjS3G_LR_REP_DAL;

            public int FunCreateRepossesion(SerializationMode SerMode, byte[] bytesRepo_Datatable, out string strErrMsg)
            {
                strErrMsg = string.Empty;
                try
                {
                
                ObjS3G_LR_REP_DAL = (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionDataTable)ClsPubSerialize.DeSerialize( bytesRepo_Datatable,SerMode, typeof(S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionDataTable));
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_LR_InsertRepossession");
               foreach (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionRow ObjRepRow in ObjS3G_LR_REP_DAL)
                {
                    db.AddInParameter(command, "@Repossession_ID", DbType.Int32, ObjRepRow.Repossession_ID);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRepRow.Company_ID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjRepRow.LOB_ID);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjRepRow.Branch_ID);
                    db.AddInParameter(command, "@PANum", DbType.String, ObjRepRow.PANum);
                    db.AddInParameter(command, "@SANum", DbType.String, ObjRepRow.SANum);
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjRepRow.Customer_ID);
                    db.AddInParameter(command, "@LRN_ID", DbType.Int32, ObjRepRow.LRN_ID);
                    db.AddInParameter(command, "@LRN_No", DbType.String, ObjRepRow.LRN_No);
                    db.AddInParameter(command, "@Repossession_Docket_Date", DbType.DateTime, ObjRepRow.Repossession_Docket_Date);
                    db.AddInParameter(command, "@Remarks", DbType.String, ObjRepRow.Remarks);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjRepRow.Created_By);
                    db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjRepRow.Created_On);
                    //db.AddInParameter(command, "@LR_Asset_ID", DbType.Int32, ObjRepRow.LR_Asset_ID);
                    //db.AddInParameter(command, "IS_Active", DbType.Boolean, ObjRepRow.IS_Active);
                    db.AddInParameter(command, "@Asset_Number", DbType.Int32, ObjRepRow.Asset_Number);
                    db.AddInParameter(command, "@Asset_Code", DbType.String, ObjRepRow.Asset_Code);
                    db.AddInParameter(command, "@Informed_To_Police", DbType.String, ObjRepRow.Informed_To_Police);
                    db.AddInParameter(command, "@Repossession_Date", DbType.DateTime, ObjRepRow.Repossession_Date);
                    db.AddInParameter(command, "@Repossession_Place", DbType.String, ObjRepRow.Repossession_Place);
                    db.AddInParameter(command, "@Asset_Condition", DbType.String, ObjRepRow.Asset_Condition);
                    db.AddInParameter(command, "@Repossession_By", DbType.String, ObjRepRow.Repossession_By);
                    db.AddInParameter(command, "@Repossesser_ID", DbType.Int32, ObjRepRow.Repossesser_ID);
                    db.AddInParameter(command, "@Garage_ID", DbType.Int32, ObjRepRow.Garage_ID);
                    db.AddInParameter(command, "@Garage_In", DbType.DateTime, ObjRepRow.Garage_In);
                    db.AddInParameter(command, "@Asset_Inventory_Details", DbType.String, ObjRepRow.Asset_Inventory_Details);
                    db.AddInParameter(command, "@Current_Market_Value", DbType.Decimal, ObjRepRow.Current_Market_Value);
                    db.AddInParameter(command, "@Repossession_Expences", DbType.String, ObjRepRow.Repossession_Expences);
                    db.AddOutParameter(command,"@Repossession_Docket_No", DbType.String, 50);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                     using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);//Modified on 22/mar/2012 by Ponnurajesh for oracle conversion
                                db.FunPubExecuteNonQuery(command, ref trans);
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                if ((int)command.Parameters["@ErrorCode"].Value > 1)
                                {
                                    throw new Exception("Error in Repossession " + intRowsAffected.ToString());
                                }
                                else
                                {
                                    strErrMsg = Convert.ToString(command.Parameters["@Repossession_Docket_No"].Value);
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (command.Parameters["@ErrorCode"].Value != null)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                }
                                else if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
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
                    intRowsAffected = 20;;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
             return intRowsAffected;
             }
        }
    }
}