#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: ROI Rules Creation DAL CLass
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
        public class ClsPubROIRules
        {
            #region Initialization
            int intErrorCode;
            Entity_Origination.RuleCardMgtServices.S3G_ORG_ROI_RulesDataTable ObjROIRulesDataTable_DAL = null;
            Entity_Origination.RuleCardMgtServices.S3G_ORG_ROI_RulesRow ObjROIRulesRow = null;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubROIRules()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion

            #region Create ROIRules

            /// <summary>
            /// 
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjROIRulesDataTable"></param>
            /// <returns></returns>
            public int FunPubCreateROIRulesInt(SerializationMode SerMode, byte[] bytesObjROIRulesDataTable)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_ORG_InsertROIRules);

                    ObjROIRulesDataTable_DAL = (Entity_Origination.RuleCardMgtServices.S3G_ORG_ROI_RulesDataTable)ClsPubSerialize.DeSerialize(bytesObjROIRulesDataTable, SerMode, typeof(S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_ROI_RulesDataTable));                    
                    ObjROIRulesRow = ObjROIRulesDataTable_DAL.Rows[0] as Entity_Origination.RuleCardMgtServices.S3G_ORG_ROI_RulesRow;

                    if (!ObjROIRulesRow.IsSerial_NumberNull())
                        db.AddInParameter(command, "@Serial_Number", DbType.Int64, ObjROIRulesRow.Serial_Number);

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjROIRulesRow.Company_ID);

                    if (!ObjROIRulesRow.IsLOB_IDNull())
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjROIRulesRow.LOB_ID);
                    if (!ObjROIRulesRow.IsModel_DescriptionNull())
                        db.AddInParameter(command, "@Model_Description", DbType.String, ObjROIRulesRow.Model_Description);

                    if (!ObjROIRulesRow.IsRate_TypeNull())
                        db.AddInParameter(command, "@Rate_Type", DbType.Int32, ObjROIRulesRow.Rate_Type);

                    if (!ObjROIRulesRow.IsROI_Rule_NumberNull())
                        db.AddInParameter(command, "@ROI_Rule_Number", DbType.String, ObjROIRulesRow.ROI_Rule_Number);

                    if (!ObjROIRulesRow.IsReturn_PatternNull())
                        db.AddInParameter(command, "@Return_Pattern", DbType.Int32, ObjROIRulesRow.Return_Pattern);

                    if (!ObjROIRulesRow.IsTime_ValueNull())
                        db.AddInParameter(command, "@Time_Value", DbType.Int32, ObjROIRulesRow.Time_Value);

                    if (!ObjROIRulesRow.IsFrequencyNull())
                        db.AddInParameter(command, "@Frequency", DbType.Int32, ObjROIRulesRow.Frequency);

                    if (!ObjROIRulesRow.IsRepayment_ModeNull())
                        db.AddInParameter(command, "@Repayment_Mode", DbType.Int32, ObjROIRulesRow.Repayment_Mode);

                    if (!ObjROIRulesRow.IsRateNull())
                        db.AddInParameter(command, "@Rate", DbType.Decimal, ObjROIRulesRow.Rate);
                    if (!ObjROIRulesRow.IsIRR_RestNull())
                        db.AddInParameter(command, "@IRR_Rest", DbType.Int32, ObjROIRulesRow.IRR_Rest);

                    if (!ObjROIRulesRow.IsInterest_CalculationNull())
                        db.AddInParameter(command, "@Interest_Calculation", DbType.Int32, ObjROIRulesRow.Interest_Calculation);
                    if (!ObjROIRulesRow.IsInterest_LevyNull())
                        db.AddInParameter(command, "@Interest_Levy", DbType.Int32, ObjROIRulesRow.Interest_Levy);

                    if (!ObjROIRulesRow.IsRecovery_Pattern_Year1Null())
                        db.AddInParameter(command, "@Recovery_Pattern_Year1", DbType.Decimal, ObjROIRulesRow.Recovery_Pattern_Year1);
                    if (!ObjROIRulesRow.IsRecovery_Pattern_Year2Null())
                        db.AddInParameter(command, "@Recovery_Pattern_Year2", DbType.Decimal, ObjROIRulesRow.Recovery_Pattern_Year2);
                    if (!ObjROIRulesRow.IsRecovery_Pattern_Year3Null())
                        db.AddInParameter(command, "@Recovery_Pattern_Year3", DbType.Decimal, ObjROIRulesRow.Recovery_Pattern_Year3);
                    if (!ObjROIRulesRow.IsRecovery_Pattern_RestNull())
                        db.AddInParameter(command, "@Recovery_Pattern_Rest", DbType.Decimal, ObjROIRulesRow.Recovery_Pattern_Rest);

                    if (!ObjROIRulesRow.IsInsuranceNull())
                        db.AddInParameter(command, "@Insurance", DbType.Int32, ObjROIRulesRow.Insurance);
                    if (!ObjROIRulesRow.IsResidual_ValueNull())
                        db.AddInParameter(command, "@Residual_Value", DbType.Boolean, ObjROIRulesRow.Residual_Value);
                    if (!ObjROIRulesRow.IsMarginNull())
                        db.AddInParameter(command, "@Margin", DbType.Boolean, ObjROIRulesRow.Margin);
                    if (!ObjROIRulesRow.IsMargin_PercentageNull())
                        db.AddInParameter(command, "@Margin_Percentage", DbType.Decimal, ObjROIRulesRow.Margin_Percentage);

                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjROIRulesRow.Is_Active);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjROIRulesRow.Created_By);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.FunPubExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;

                }
                catch (Exception ex)
                {
                    //intErrorCode = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }
            #endregion

            #region Modify ROIRules
            /// <summary>
            /// 
            /// </summary>
            /// <param name="intROIRulesID"></param>
            /// <param name="blnIsActive"></param>
            /// <param name="intUserID"></param>
            /// <returns></returns>
            public int FunPubModifyROIRulesInt(int intROIRulesID, bool blnIsActive, int intUserID)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_ORG_UpdateROIRules);

                    db.AddInParameter(command, "@ID", DbType.Int32, intROIRulesID);
                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, blnIsActive);
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

            #region View ROIRules
            /// <summary>
            /// 
            /// </summary>
            /// <param name="intROIRulesID"></param>
            /// <returns></returns>
            public Entity_Origination.RuleCardMgtServices.S3G_ORG_ROI_RulesDataTable FunPubQueryROIRulesDetails(int intROIRulesID)
            {
                Entity_Origination.RuleCardMgtServices dsROIRulesDetails = null;
                try
                {
                    dsROIRulesDetails = new S3GBusEntity.Origination.RuleCardMgtServices();

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_ORG_GetROIRules);
                    db.AddInParameter(command, "@ROI_Rules_ID", DbType.Int32, intROIRulesID);
                    db.FunPubLoadDataSet(command, dsROIRulesDetails, dsROIRulesDetails.S3G_ORG_ROI_Rules.TableName);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsROIRulesDetails.S3G_ORG_ROI_Rules;
            }

            public Entity_Origination.RuleCardMgtServices.S3G_ORG_ROI_RulesDataTable FunPubQueryROIRulesDetailsPaging(int intROIRulesID, out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                Entity_Origination.RuleCardMgtServices dsROIRulesDetails = null;
                try
                {
                    dsROIRulesDetails = new S3GBusEntity.Origination.RuleCardMgtServices();

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_ORG_GetROIRules_Paging);
                    db.AddInParameter(command, "@ROI_Rules_ID", DbType.Int32, intROIRulesID);
                    db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                    db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                    db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                    db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                    db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));
                    db.FunPubLoadDataSet(command, dsROIRulesDetails, dsROIRulesDetails.S3G_ORG_ROI_Rules.TableName);

                    if ((int)command.Parameters["@TotalRecords"].Value > 0)
                        intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsROIRulesDetails.S3G_ORG_ROI_Rules;
            }
            #endregion
        }
    }
}
