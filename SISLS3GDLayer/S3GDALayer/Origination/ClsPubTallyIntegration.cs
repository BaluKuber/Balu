#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Payment Rule Card Creation DAL Class
/// Created By			: Suresh P
/// Created Date		: 01-Jun-2010
/// Purpose	            : 
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
using Entity_Origination = S3GBusEntity.Origination;
#endregion

namespace S3GDALayer.Origination
{
    namespace ApplicationMgtServices
    {
        public class ClsPubTallyIntegration
        {
            #region Initialization
            int intErrorCode;

            Entity_Origination.ApplicationMgtServices.S3G_ORG_Tally_Integ_HdrDataTable ObjTallyDataTable_DAL = null;
            Entity_Origination.ApplicationMgtServices.S3G_ORG_Tally_Integ_HdrRow ObjTallyDataRow = null;

            Entity_Origination.RuleCardMgtServices.S3G_ORG_PaymentRuleCardDataTable ObjPaymentRuleCardDataTable_DAL = null;
            Entity_Origination.RuleCardMgtServices.S3G_ORG_PaymentRuleCardRow ObjPaymentRuleCardRow = null;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubTallyIntegration()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion

            #region Create PaymentRuleCard
            /// <summary>
            /// 
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjPaymentRuleCardDataTable"></param>
            /// <returns></returns>
            public int FunPubCreateTallyIntegDetails(SerializationMode SerMode, byte[] bytesObjTallyDataTable)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Org_Insert_Tally_Integ_Det");

                    ObjTallyDataTable_DAL = (Entity_Origination.ApplicationMgtServices.S3G_ORG_Tally_Integ_HdrDataTable)ClsPubSerialize.DeSerialize(bytesObjTallyDataTable, SerMode, typeof(S3GBusEntity.Origination.ApplicationMgtServices.S3G_ORG_Tally_Integ_HdrDataTable));
                    ObjTallyDataRow = ObjTallyDataTable_DAL.Rows[0] as Entity_Origination.ApplicationMgtServices.S3G_ORG_Tally_Integ_HdrRow;

                    db.AddInParameter(command, "@Tally_Integ_Type", DbType.Int32, ObjTallyDataRow.Tally_Integ_Type);
                    db.AddInParameter(command, "@XMLRSDet", DbType.String, ObjTallyDataRow.XML_RS_Details);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTallyDataRow.Company_ID);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjTallyDataRow.Created_By);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command,ref trans);

                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                    trans.Rollback();
                                    throw new Exception("Error thrown Error No" + intErrorCode.ToString());

                                }
                                else
                                {
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intErrorCode == 0)
                                    intErrorCode = 50;
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
                return intErrorCode;
            }

            #endregion

          
        }
    }
}
