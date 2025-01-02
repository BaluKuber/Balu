#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Credit Parameter Transaction
/// Created By			: Narayanan
/// Created Date		: 11-06-2010
/// Modified By			: Prabhu.K
/// Modified Date		: 06-Aug-2010
/// Reason              : Appends few more methods
/// <Program Summary>
#endregion

using System;
using S3GDALayer.S3GAdminServices;
using System.Data;
using S3GBusEntity;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace S3GDALayer.Origination
{
    public class ClsPubCreditParamTransaction
    {
        string strConnectionString = "S3GConnectionString";

        //Code added for getting common connection string  from config file
        Database db;
        public ClsPubCreditParamTransaction()
        {
            db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
        }

        public Int32 FunPubInsertCreditParamTransaction(CreditParameterTransactionEntity ObjCreditParameterTransactionEntity, out string CreditParamNumber)
        {
            CreditParamNumber = "";
            try
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_CreditParameterTransaction_Insert");

                db.AddInParameter(command, "@CompanyId", DbType.Int32, ObjCreditParameterTransactionEntity.CompanyId);
                db.AddInParameter(command, "@AppraisalId", DbType.Int32, ObjCreditParameterTransactionEntity.AppraisalId);
                if (ObjCreditParameterTransactionEntity.CustomerId != 0)
                {
                    db.AddInParameter(command, "@CustomerId", DbType.Int32, ObjCreditParameterTransactionEntity.CustomerId);
                }
                db.AddOutParameter(command, "@CreditParamNumber", DbType.String, 200);
                db.AddInParameter(command, "@CreditParamDate", DbType.DateTime, ObjCreditParameterTransactionEntity.CreditParamDate);
                db.AddInParameter(command, "@CreditParamStatus", DbType.Int32, ObjCreditParameterTransactionEntity.StatusId);
                db.AddInParameter(command, "@Document_Type", DbType.Int32, ObjCreditParameterTransactionEntity.Document_Type);

                if (ObjCreditParameterTransactionEntity.EnquiryResponseId != 0)
                {
                    db.AddInParameter(command, "@EnquiryResponseId", DbType.Int32, ObjCreditParameterTransactionEntity.EnquiryResponseId);
                }
                db.AddInParameter(command, "@CreatedBy", DbType.Int32, ObjCreditParameterTransactionEntity.CreatedBy);
                db.AddInParameter(command, "@CreatedOn", DbType.DateTime, ObjCreditParameterTransactionEntity.CreatedOn);
                db.AddInParameter(command, "@ModifiedBy", DbType.Int32, ObjCreditParameterTransactionEntity.ModifiedBy);
                db.AddInParameter(command, "@ModifiedOn", DbType.DateTime, ObjCreditParameterTransactionEntity.ModifiedOn);

                db.AddInParameter(command, "@XmlScoreDetails", DbType.String, ObjCreditParameterTransactionEntity.XmlScoreDetails);
                db.AddInParameter(command, "@XMLOthers", DbType.String, ObjCreditParameterTransactionEntity.XmlOthers);

                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                //db.ExecuteNonQuery(command);
                db.FunPubExecuteNonQuery(command);

                if ((int)command.Parameters["@ErrorCode"].Value == 0)
                {
                    CreditParamNumber = Convert.ToString(command.Parameters["@CreditParamNumber"].Value);
                }

                return (Int32)command.Parameters["@ErrorCode"].Value;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FunPubInsertCreditParamTransactionScoreDetails(CreditParamterTransScoreDTO ObjCreditParamterTransScoreDTO)
        {
            try
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_CreditParameterTrans_ScoreDetails_Save");

                db.AddInParameter(command, "@CreditParamTrans_ID", DbType.Int32, ObjCreditParamterTransScoreDTO.CreditParamTrans_ID);
                db.AddInParameter(command, "@CreditScore_Item_ID", DbType.Int32, ObjCreditParamterTransScoreDTO.CreditScore_Item_ID);
                db.AddInParameter(command, "@GlobalParameter_ID", DbType.String, ObjCreditParamterTransScoreDTO.GlobalParameter_ID);
                db.AddInParameter(command, "@PercentageImportance", DbType.String, ObjCreditParamterTransScoreDTO.PercentageImportance);
                db.AddInParameter(command, "@RequiredScore", DbType.Int32, ObjCreditParamterTransScoreDTO.RequiredScore);
                db.AddInParameter(command, "@ActualScore", DbType.Int32, ObjCreditParamterTransScoreDTO.ActualScore);
                db.AddInParameter(command, "@Created_By", DbType.Int32, ObjCreditParamterTransScoreDTO.Created_By);
                //db.ExecuteNonQuery(command);
                db.FunPubExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet FunPubGetEnquiryCustomerAppraisalCPT(int intAppraisalType,int intCompanyId,int intAppraisalId)
        //{
        //    string[] strProcName = {"S3G_Org_GetEnquiryCustomerAppraisal_CPT","S3G_Org_GetEnquiryCustomerScore_CPT"};
        //    try
        //    {
        //        DataSet ObjDS = new DataSet();
        //        Database db = DatabaseFactory.CreateDatabase(strConnectionString);
        //        DbCommand command = db.GetStoredProcCommand("S3G_Org_GetEnquiryCustomerAppraisal_CPT");
        //        db.AddInParameter(command, "@AppraisalType", DbType.Int32, intAppraisalType);
        //        db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
        //        db.AddInParameter(command, "@AppraisalId", DbType.Int32, intAppraisalId);
        //        db.LoadDataSet(command, ObjDS, strProcName);
        //        return ObjDS;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet FunPubGetEnquiryCustomerAppraisalCPT(int intAppraisalType, int intCompanyId, int intAppraisalId, int intPageSize, int intCurrentPage, string strSearchValue, string strOrderBy, int intTotalRecords)
        {
            string[] strProcName = { "S3G_Org_GetEnquiryCustomerAppraisal_CPT", "S3G_Org_GetEnquiryCustomerScore_CPT" };
            // string[] strProcName = { "S3G_Org_GetCreditParamTrans_AppDetails1", "S3G_Org_GetEnquiryCustomerScore_CPT" };
            try
            {
                DataSet ObjDS = new DataSet();
                //Database db = DatabaseFactory.CreateDatabase(strConnectionString);
                DbCommand command = db.GetStoredProcCommand("S3G_Org_GetEnquiryCustomerAppraisal_CPT");
                // DbCommand command = db.GetStoredProcCommand("S3G_Org_GetCreditParamTrans_AppDetails1");
                db.AddInParameter(command, "@AppraisalType", DbType.Int32, intAppraisalType);
                db.AddInParameter(command, "@Company_Id", DbType.Int32, intCompanyId);
                db.AddInParameter(command, "@AppraisalId", DbType.Int32, intAppraisalId);
                db.AddInParameter(command, "@PageSize", DbType.Int32, intPageSize);
                db.AddInParameter(command, "@CurrentPage", DbType.Int32, intCurrentPage);
                db.AddInParameter(command, "@SearchValue", DbType.String, strSearchValue);
                db.AddInParameter(command, "@OrderBy", DbType.String, strSearchValue);
                db.AddInParameter(command, "@TotalRecords", DbType.Int32, intTotalRecords);
                //db.LoadDataSet(command, ObjDS, strProcName);
                db.FunPubLoadDataSet(command, ObjDS, strProcName);
                return ObjDS;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet FunPubGetCreditParameterTransaction(int intCreditParamTransId)
        {
            string[] strProcName = { "S3g_Org_GetCreditParameterTransaction", "S3g_Org_GetCreditParameterTransaction_Score" };
            try
            {
                DataSet ObjDS = new DataSet();
                //Database db = DatabaseFactory.CreateDatabase(strConnectionString);
                DbCommand command = db.GetStoredProcCommand("S3g_Org_GetCreditParameterTransaction");
                db.AddInParameter(command, "@CreditParamTransactionId", DbType.Int32, intCreditParamTransId);
                //db.LoadDataSet(command, ObjDS, strProcName);
                db.FunPubLoadDataSet(command, ObjDS, strProcName);
                return ObjDS;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 FunPubModifyCreditParamTransaction(CreditParameterTransactionEntity ObjCreditParameterTransactionEntity)
        {
            try
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_CreditParameterTransaction_Modify");

                db.AddInParameter(command, "@CreditParamTransId", DbType.Int32, ObjCreditParameterTransactionEntity.AppraisalId);
                db.AddInParameter(command, "@ModifiedBy", DbType.Int32, ObjCreditParameterTransactionEntity.ModifiedBy);
                db.AddInParameter(command, "@ModifiedOn", DbType.DateTime, ObjCreditParameterTransactionEntity.ModifiedOn);
                db.AddInParameter(command, "@XmlScoreDetails", DbType.String, ObjCreditParameterTransactionEntity.XmlScoreDetails);
                db.AddInParameter(command, "@XMLOthers", DbType.String, ObjCreditParameterTransactionEntity.XmlOthers);
                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                //db.ExecuteNonQuery(command);
                db.FunPubExecuteNonQuery(command);
                return (Int32)command.Parameters["@ErrorCode"].Value;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
