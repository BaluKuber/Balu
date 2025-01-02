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
    namespace RuleCardMgtServices
    {
        public class ClsPubPaymentRuleCard
        {
            #region Initialization
            int intErrorCode;

            Entity_Origination.RuleCardMgtServices.S3G_ORG_PaymentRuleCardDataTable ObjPaymentRuleCardDataTable_DAL = null;
            Entity_Origination.RuleCardMgtServices.S3G_ORG_PaymentRuleCardRow ObjPaymentRuleCardRow = null;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubPaymentRuleCard()
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
            public int FunPubCreatePaymentRuleCardInt(SerializationMode SerMode, byte[] bytesObjPaymentRuleCardDataTable)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_ORG_InsertPaymentRuleCard);

                    ObjPaymentRuleCardDataTable_DAL = (Entity_Origination.RuleCardMgtServices.S3G_ORG_PaymentRuleCardDataTable)ClsPubSerialize.DeSerialize(bytesObjPaymentRuleCardDataTable, SerMode, typeof(S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_PaymentRuleCardDataTable));
                    ObjPaymentRuleCardRow = ObjPaymentRuleCardDataTable_DAL.Rows[0] as Entity_Origination.RuleCardMgtServices.S3G_ORG_PaymentRuleCardRow;

                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPaymentRuleCardRow.LOB_ID);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPaymentRuleCardRow.Company_ID);

                    if (!ObjPaymentRuleCardRow.IsProduct_IDNull())
                        db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjPaymentRuleCardRow.Product_ID);

                    if (!ObjPaymentRuleCardRow.IsAccountType_IDNull())
                        db.AddInParameter(command, "@AccountType_ID", DbType.Int32, ObjPaymentRuleCardRow.AccountType_ID);

                    if (!ObjPaymentRuleCardRow.IsEntity_IDNull())
                        db.AddInParameter(command, "@Entity_ID", DbType.Int32, ObjPaymentRuleCardRow.Entity_ID);

                    if (!ObjPaymentRuleCardRow.IsCompensation_PercentageNull())
                        db.AddInParameter(command, "@CompensationPercentage", DbType.Decimal, ObjPaymentRuleCardRow.Compensation_Percentage);

                    if (!ObjPaymentRuleCardRow.IsCompensation_Levy_PatternNull())
                        db.AddInParameter(command, "@CompensationLevyPattern", DbType.String, ObjPaymentRuleCardRow.Compensation_Levy_Pattern);

                    if (!ObjPaymentRuleCardRow.IsLevy_FrequencyNull())
                        db.AddInParameter(command, "@LevyFrequency", DbType.Decimal, ObjPaymentRuleCardRow.Levy_Frequency);

                    if (!ObjPaymentRuleCardRow.IsIs_OnTap_RefinanceNull())
                        db.AddInParameter(command, "@IsOnTapRefinance", DbType.Boolean, ObjPaymentRuleCardRow.Is_OnTap_Refinance);

                    db.AddInParameter(command, "@IsActive", DbType.Boolean, ObjPaymentRuleCardRow.Is_Active);

                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjPaymentRuleCardRow.Created_By);

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

            #region Modify PaymentRuleCard
            /// <summary>
            /// 
            /// </summary>
            /// <param name="intPayment_RuleCard_ID"></param>
            /// <param name="blnIs_Active"></param>
            /// <param name="intUserID"></param>
            /// <returns></returns>
            public int FunPubModifyPaymentRuleCardInt(int intPayment_RuleCard_ID, bool blnIs_Active, int intUserID)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_ORG_UpdatePaymentRuleCard);

                    db.AddInParameter(command, "@Payment_RuleCard_ID", DbType.Int32, intPayment_RuleCard_ID);
                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, blnIs_Active);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, intUserID);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.FunPubExecuteNonQuery(command);
                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }
            #endregion

            #region View PaymentRuleCard
            /// <summary>
            /// 
            /// </summary>
            /// <param name="intPayment_RuleCard_ID"></param>
            /// <returns></returns>
            public Entity_Origination.RuleCardMgtServices.S3G_ORG_PaymentRuleCardDataTable FunPubQueryPaymentRuleCardsDetails(int intPayment_RuleCard_ID)
            {
                Entity_Origination.RuleCardMgtServices dsROIRulesDetails = null;
                try
                {
                    dsROIRulesDetails = new S3GBusEntity.Origination.RuleCardMgtServices();

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_ORG_GetPaymentRuleCard);
                    db.AddInParameter(command, "@Payment_RuleCard_ID", DbType.Int32, intPayment_RuleCard_ID);
                    db.FunPubLoadDataSet(command, dsROIRulesDetails, dsROIRulesDetails.S3G_ORG_PaymentRuleCard.TableName);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsROIRulesDetails.S3G_ORG_PaymentRuleCard;
            }

            public Entity_Origination.RuleCardMgtServices.S3G_ORG_PaymentRuleCardDataTable FunPubQueryPaymentRuleCardsDetailsPaging(int intPayment_RuleCard_ID, out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                Entity_Origination.RuleCardMgtServices dsROIRulesDetails = null;
                try
                {
                    dsROIRulesDetails = new S3GBusEntity.Origination.RuleCardMgtServices();

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_ORG_GetPaymentRuleCard_Paging);
                    db.AddInParameter(command, "@Payment_RuleCard_ID", DbType.Int32, intPayment_RuleCard_ID);

                    db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                    db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                    db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                    db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                    db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));

                    db.FunPubLoadDataSet(command, dsROIRulesDetails, dsROIRulesDetails.S3G_ORG_PaymentRuleCard.TableName);

                    if ((int)command.Parameters["@TotalRecords"].Value > 0)
                        intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsROIRulesDetails.S3G_ORG_PaymentRuleCard;
            }

            #endregion
        }
    }
}
