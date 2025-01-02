#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Enquiry Response
/// Created By			: Narayanan
/// Created Date		: 
/// <Program Summary>
#endregion

using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using S3GBusEntity;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.OracleClient;

namespace S3GDALayer.Origination
{
    public class ClsPubEnquiryResponse
    {
        string strConnectionString = "S3GConnectionString";
        //Database objDatabase = DatabaseFactory.CreateDatabase("S3GconnectionString");
        DbConnection objConnection;
        DbTransaction objTransaction;
        //Code added for getting common connection string  from config file
        Database db;
        Database objDatabase;
        public ClsPubEnquiryResponse()
        {
            db = objDatabase = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();

        }

        public DataSet FunPubGetEnquiryResponse(int intEnquiryResponseId)
        {
            string[] strProcName = { "S3g_Org_EnquiryResponse", "S3G_ORG_EnquiryResponseAlertDetails", "S3G_ORG_FollowUp", "S3G_ORG_FollowUpDetail", "S3G_ORG_EnquiryResponseRepayTermsDetails", "S3G_ORG_EnquiryResponseOfferDetails", "S3G_ORG_EnquiryResponseOfferPaymentDetails", "S3G_ORG_EnquiryResponseOfferROIDetails" };
            try
            {
                DataSet ObjDS = new DataSet();
                //Database db = DatabaseFactory.CreateDatabase(strConnectionString);
                DbCommand command = db.GetStoredProcCommand("S3g_Org_GetEnquiryResponse");
                db.AddInParameter(command, "@EnquiryResponseId", DbType.Int32, intEnquiryResponseId);
                //db.LoadDataSet(command, ObjDS, strProcName);
                db.FunPubLoadDataSet(command, ObjDS, strProcName);
                return ObjDS;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 FunPubModifyEnquiryResponse(S3GBusEntity.EnquiryResponse.EnquiryResponseEntity objEnquiryResponseEntity)
        {
            try
            {
                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                Int32 intErrorCode = 0;
                FunPriOpenConnection();
                DbCommand objCommand = objDatabase.GetStoredProcCommand("S3G_ORG_UpdateEnquiryResponse");
                objDatabase.AddInParameter(objCommand, "@EnquiryResponseID", DbType.Int32, objEnquiryResponseEntity.EnquiryResponse_ID);
                if (objEnquiryResponseEntity.Repay_Accounting_IRR != null)
                {
                    objDatabase.AddInParameter(objCommand, "@RepayAccountingIRR", DbType.Decimal, objEnquiryResponseEntity.Repay_Accounting_IRR);
                }
                if (objEnquiryResponseEntity.Repay_Company_IRR != null)
                {
                    objDatabase.AddInParameter(objCommand, "@RepayCompanyIRR", DbType.Decimal, objEnquiryResponseEntity.Repay_Company_IRR);
                }
                if (objEnquiryResponseEntity.Repay_Business_IRR != null)
                {
                    objDatabase.AddInParameter(objCommand, "@RepayBusinessIRR", DbType.Decimal, objEnquiryResponseEntity.Repay_Business_IRR);
                }
                //added on 24-Nov-2011 by Saran as per the observation raised by malolan start.
                if (objEnquiryResponseEntity.Repay_Business_IRR != null)
                {
                    objDatabase.AddInParameter(objCommand, "@Status", DbType.Int32, objEnquiryResponseEntity.Status);
                }
                if (objEnquiryResponseEntity.EnquiryResponseDetailId != null)
                {
                    objDatabase.AddInParameter(objCommand, "@EnquiryResponseDetailId", DbType.Int32, objEnquiryResponseEntity.EnquiryResponseDetailId);
                }
                if (objEnquiryResponseEntity.Offer_ROI_Rules_ID > 0)
                    objDatabase.AddInParameter(objCommand, "@OfferROIRulesID", DbType.Int32, objEnquiryResponseEntity.Offer_ROI_Rules_ID);
                if (objEnquiryResponseEntity.Offer_Payment_RuleCard_ID > 0)
                    objDatabase.AddInParameter(objCommand, "@OfferPaymentRuleCardID", DbType.Int32, objEnquiryResponseEntity.Offer_Payment_RuleCard_ID);
                objDatabase.AddInParameter(objCommand, "@OfferResidualValue", DbType.Decimal, objEnquiryResponseEntity.Offer_ResidualValue);
                objDatabase.AddInParameter(objCommand, "@OfferResidualValueAmount", DbType.Decimal, objEnquiryResponseEntity.Offer_ResidualValueAmount);
                objDatabase.AddInParameter(objCommand, "@OfferMargin", DbType.Decimal, objEnquiryResponseEntity.Offer_Margin);
                objDatabase.AddInParameter(objCommand, "@OfferMarginAmount", DbType.Decimal, objEnquiryResponseEntity.Offer_Margin_Amount);

                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    /*  added by senthilkumar p on 07/mar/2012 */
                    OracleParameter param, param1, param2, param3, param4, param5, param6;
                    if (objEnquiryResponseEntity.XML_ROIRULE != null)
                    {
                        param = new OracleParameter("@XML_ROIRULE",
                             OracleType.Clob, objEnquiryResponseEntity.XML_ROIRULE.Length,
                             ParameterDirection.Input, true, 0, 0, String.Empty,
                              DataRowVersion.Default, objEnquiryResponseEntity.XML_ROIRULE);
                        objCommand.Parameters.Add(param);
                    }

                    if (objEnquiryResponseEntity.XmlAlertDetails != null)
                    {
                        param1 = new OracleParameter("@XmlAlertDetails",
                             OracleType.Clob, objEnquiryResponseEntity.XmlAlertDetails.Length,
                             ParameterDirection.Input, true, 0, 0, String.Empty,
                              DataRowVersion.Default, objEnquiryResponseEntity.XmlAlertDetails);
                        objCommand.Parameters.Add(param1);
                    }

                    if (objEnquiryResponseEntity.XmlFollowDetails != null)
                    {
                        param2 = new OracleParameter("@XmlFollowDetails",
                             OracleType.Clob, objEnquiryResponseEntity.XmlFollowDetails.Length,
                             ParameterDirection.Input, true, 0, 0, String.Empty,
                              DataRowVersion.Default, objEnquiryResponseEntity.XmlFollowDetails);
                        objCommand.Parameters.Add(param2);
                    }
                    if (objEnquiryResponseEntity.XmlOutFlowDetails != null)
                    {
                    param3 = new OracleParameter("@XmlOutFlowDetails",
                         OracleType.Clob, objEnquiryResponseEntity.XmlOutFlowDetails.Length,
                         ParameterDirection.Input, true, 0, 0, String.Empty,
                          DataRowVersion.Default, objEnquiryResponseEntity.XmlOutFlowDetails);
                    objCommand.Parameters.Add(param3);
                    }

                    if (objEnquiryResponseEntity.XmlCashInflowDetails != null)
                    {
                        param4 = new OracleParameter("@XmlCashInflowDetails",
                             OracleType.Clob, objEnquiryResponseEntity.XmlCashInflowDetails.Length,
                             ParameterDirection.Input, true, 0, 0, String.Empty,
                              DataRowVersion.Default, objEnquiryResponseEntity.XmlCashInflowDetails);
                        objCommand.Parameters.Add(param4);
                    }

                    if (objEnquiryResponseEntity.XmlRepaymentDetails != null)
                    {
                        param5 = new OracleParameter("@XmlRepaymentDetails",
                             OracleType.Clob, objEnquiryResponseEntity.XmlRepaymentDetails.Length,
                             ParameterDirection.Input, true, 0, 0, String.Empty,
                              DataRowVersion.Default, objEnquiryResponseEntity.XmlRepaymentDetails);
                        objCommand.Parameters.Add(param5);
                    }
                    if (objEnquiryResponseEntity.XML_REPAYSTRUCTURE != null)
                    {
                        param6 = new OracleParameter("@XML_REPAYSTRUCTURE",
                             OracleType.Clob, objEnquiryResponseEntity.XML_REPAYSTRUCTURE.Length,
                             ParameterDirection.Input, true, 0, 0, String.Empty,
                              DataRowVersion.Default, objEnquiryResponseEntity.XML_REPAYSTRUCTURE);
                        objCommand.Parameters.Add(param6);
                    }
                    /*  added by senthilkumar p on 07/mar/2012 */

                }
                else
                {
                    objDatabase.AddInParameter(objCommand, "@XML_ROIRULE", DbType.String, objEnquiryResponseEntity.XML_ROIRULE);

                    //added on 24-Nov-2011 by Saran as per the observation raised by malolan End.

                    objDatabase.AddInParameter(objCommand, "@XmlAlertDetails", DbType.String, objEnquiryResponseEntity.XmlAlertDetails);
                    objDatabase.AddInParameter(objCommand, "@XmlFollowDetails", DbType.String, objEnquiryResponseEntity.XmlFollowDetails);
                    objDatabase.AddInParameter(objCommand, "@XmlOutFlowDetails", DbType.String, objEnquiryResponseEntity.XmlOutFlowDetails);
                    objDatabase.AddInParameter(objCommand, "@XmlCashInflowDetails", DbType.String, objEnquiryResponseEntity.XmlCashInflowDetails);
                    objDatabase.AddInParameter(objCommand, "@XmlRepaymentDetails", DbType.String, objEnquiryResponseEntity.XmlRepaymentDetails);
                    //objDatabase.AddInParameter(objCommand, "@XML_ROIRULE", DbType.String, objEnquiryResponseEntity.XML_ROIRULE);
                    objDatabase.AddInParameter(objCommand, "@XML_REPAYSTRUCTURE", DbType.String, objEnquiryResponseEntity.XML_REPAYSTRUCTURE);
                }

                objDatabase.AddInParameter(objCommand, "@ModifiedBy", DbType.Int32, objEnquiryResponseEntity.Responded_By);
                objDatabase.AddOutParameter(objCommand, "@ErrorCode", DbType.Int32, sizeof(Int64));

                //objDatabase.ExecuteNonQuery(objCommand, objTransaction);

                objDatabase.FunPubExecuteNonQuery(objCommand, ref objTransaction);

                if (Convert.ToInt32(objCommand.Parameters["@ErrorCode"].Value) == 0)
                    intErrorCode = Convert.ToInt32(objCommand.Parameters["@ErrorCode"].Value);
                objCommand.Parameters.Clear();
                FunPriCommitTransaction();
                return intErrorCode;

            }
            catch (Exception ex)
            {
                FunPriRollbackTransaction();
                throw ex;
            }
        }

        public Int32 FunPubInsertEnquiryResponse(S3GBusEntity.EnquiryResponse.EnquiryResponseEntity objEnquiryResponseEntity, out string strEnquiryResponse)
        {
            int intErrorCode = 0;
            string strEnquiryResponseId = "";
            S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
            try
            {

                FunPriOpenConnection();
                DbCommand objCommand = objDatabase.GetStoredProcCommand("S3G_ORG_InsertEnquiryResponse");
                objDatabase.AddOutParameter(objCommand, "@EnquiryResponseID", DbType.Int32, sizeof(int));
                objDatabase.AddInParameter(objCommand, "@EnquiryNo", DbType.String, objEnquiryResponseEntity.Enquiry_No);
                objDatabase.AddInParameter(objCommand, "@CompanyID", DbType.Int32, objEnquiryResponseEntity.Company_ID);
                objDatabase.AddInParameter(objCommand, "@CustomerID", DbType.String, objEnquiryResponseEntity.Customer_ID);
                objDatabase.AddInParameter(objCommand, "@RespondedBy", DbType.Int32, objEnquiryResponseEntity.Responded_By);
                objDatabase.AddInParameter(objCommand, "@FinanceAmountSought", DbType.Decimal, objEnquiryResponseEntity.Finance_Amount_Sought);
                objDatabase.AddInParameter(objCommand, "@ResidualMarginAmount", DbType.Decimal, objEnquiryResponseEntity.Residual_Margin_Amount);
                objDatabase.AddInParameter(objCommand, "@LOBID", DbType.Int32, objEnquiryResponseEntity.LOB_ID);
                objDatabase.AddInParameter(objCommand, "@ProductID", DbType.Int32, objEnquiryResponseEntity.Product_ID);
                objDatabase.AddInParameter(objCommand, "@Location_ID", DbType.Int32, objEnquiryResponseEntity.Branch_ID);
                objDatabase.AddInParameter(objCommand, "@WorkFlowSequence", DbType.Int32, objEnquiryResponseEntity.WorkFlow_Sequence);
                if (objEnquiryResponseEntity.Offer_ROI_Rules_ID > 0)
                    objDatabase.AddInParameter(objCommand, "@OfferROIRulesID", DbType.Int32, objEnquiryResponseEntity.Offer_ROI_Rules_ID);
                if (objEnquiryResponseEntity.Offer_Payment_RuleCard_ID > 0)
                    objDatabase.AddInParameter(objCommand, "@OfferPaymentRuleCardID", DbType.Int32, objEnquiryResponseEntity.Offer_Payment_RuleCard_ID);
                objDatabase.AddInParameter(objCommand, "@OfferResidualValue", DbType.Decimal, objEnquiryResponseEntity.Offer_ResidualValue);
                objDatabase.AddInParameter(objCommand, "@OfferResidualValueAmount", DbType.Decimal, objEnquiryResponseEntity.Offer_ResidualValueAmount);
                objDatabase.AddInParameter(objCommand, "@OfferMargin", DbType.Decimal, objEnquiryResponseEntity.Offer_Margin);
                objDatabase.AddInParameter(objCommand, "@OfferMarginAmount", DbType.Decimal, objEnquiryResponseEntity.Offer_Margin_Amount);
                objDatabase.AddInParameter(objCommand, "@RepayBlockDepriciation", DbType.Decimal, objEnquiryResponseEntity.Repay_Block_Depriciation);
                objDatabase.AddInParameter(objCommand, "@RepayBookDepriciation", DbType.Decimal, objEnquiryResponseEntity.Repay_Book_Depriciation);
                if (objEnquiryResponseEntity.Repay_Accounting_IRR != null)
                {
                    objDatabase.AddInParameter(objCommand, "@RepayAccountingIRR", DbType.Decimal, objEnquiryResponseEntity.Repay_Accounting_IRR);
                }
                if (objEnquiryResponseEntity.Repay_Company_IRR != null)
                {
                    objDatabase.AddInParameter(objCommand, "@RepayCompanyIRR", DbType.Decimal, objEnquiryResponseEntity.Repay_Company_IRR);
                }
                if (objEnquiryResponseEntity.Repay_Business_IRR != null)
                {
                    objDatabase.AddInParameter(objCommand, "@RepayBusinessIRR", DbType.Decimal, objEnquiryResponseEntity.Repay_Business_IRR);
                }
                objDatabase.AddInParameter(objCommand, "@RepaySummary", DbType.Int32, objEnquiryResponseEntity.Repay_Summary);
                objDatabase.AddInParameter(objCommand, "@EnquiryResponseDetailId", DbType.Int32, objEnquiryResponseEntity.EnquiryResponseDetailId);
                objDatabase.AddInParameter(objCommand, "@Status", DbType.Int32, objEnquiryResponseEntity.Status);
                objDatabase.AddInParameter(objCommand, "@ApplicationNumber", DbType.Int32, objEnquiryResponseEntity.ApplicationNumber);
                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    /*  added by senthilkumar p on 07/mar/2012 */
                    OracleParameter param, param1, param2, param3, param4, param5, param6;
                    if (objEnquiryResponseEntity.XML_ROIRULE != null)
                    {
                        param = new OracleParameter("@XML_ROIRULE",
                             OracleType.Clob, objEnquiryResponseEntity.XML_ROIRULE.Length,
                             ParameterDirection.Input, true, 0, 0, String.Empty,
                              DataRowVersion.Default, objEnquiryResponseEntity.XML_ROIRULE);
                        objCommand.Parameters.Add(param);
                    }

                    if (objEnquiryResponseEntity.XmlAlertDetails != null)
                    {
                        param1 = new OracleParameter("@XmlAlertDetails",
                             OracleType.Clob, objEnquiryResponseEntity.XmlAlertDetails.Length,
                             ParameterDirection.Input, true, 0, 0, String.Empty,
                              DataRowVersion.Default, objEnquiryResponseEntity.XmlAlertDetails);
                        objCommand.Parameters.Add(param1);
                    }

                    if (objEnquiryResponseEntity.XmlFollowDetails != null)
                    {
                        param2 = new OracleParameter("@XmlFollowDetails",
                             OracleType.Clob, objEnquiryResponseEntity.XmlFollowDetails.Length,
                             ParameterDirection.Input, true, 0, 0, String.Empty,
                              DataRowVersion.Default, objEnquiryResponseEntity.XmlFollowDetails);
                        objCommand.Parameters.Add(param2);
                    }
                    if (objEnquiryResponseEntity.XmlOutFlowDetails != null)
                    {
                        param3 = new OracleParameter("@XmlOutFlowDetails",
                             OracleType.Clob, objEnquiryResponseEntity.XmlOutFlowDetails.Length,
                             ParameterDirection.Input, true, 0, 0, String.Empty,
                              DataRowVersion.Default, objEnquiryResponseEntity.XmlOutFlowDetails);
                        objCommand.Parameters.Add(param3);
                    }

                    if (objEnquiryResponseEntity.XmlCashInflowDetails != null)
                    {
                        param4 = new OracleParameter("@XmlCashInflowDetails",
                             OracleType.Clob, objEnquiryResponseEntity.XmlCashInflowDetails.Length,
                             ParameterDirection.Input, true, 0, 0, String.Empty,
                              DataRowVersion.Default, objEnquiryResponseEntity.XmlCashInflowDetails);
                        objCommand.Parameters.Add(param4);
                    }

                    if (objEnquiryResponseEntity.XmlRepaymentDetails != null)
                    {
                        param5 = new OracleParameter("@XmlRepaymentDetails",
                             OracleType.Clob, objEnquiryResponseEntity.XmlRepaymentDetails.Length,
                             ParameterDirection.Input, true, 0, 0, String.Empty,
                              DataRowVersion.Default, objEnquiryResponseEntity.XmlRepaymentDetails);
                        objCommand.Parameters.Add(param5);
                    }
                    if (objEnquiryResponseEntity.XML_REPAYSTRUCTURE != null)
                    {
                        param6 = new OracleParameter("@XML_REPAYSTRUCTURE",
                             OracleType.Clob, objEnquiryResponseEntity.XML_REPAYSTRUCTURE.Length,
                             ParameterDirection.Input, true, 0, 0, String.Empty,
                              DataRowVersion.Default, objEnquiryResponseEntity.XML_REPAYSTRUCTURE);
                        objCommand.Parameters.Add(param6);
                    }
                    /*  added by senthilkumar p on 07/mar/2012 */

                }
                else
                {
                    objDatabase.AddInParameter(objCommand, "@XML_ROIRULE", DbType.String, objEnquiryResponseEntity.XML_ROIRULE);

                    //added on 24-Nov-2011 by Saran as per the observation raised by malolan End.

                    objDatabase.AddInParameter(objCommand, "@XmlAlertDetails", DbType.String, objEnquiryResponseEntity.XmlAlertDetails);
                    objDatabase.AddInParameter(objCommand, "@XmlFollowDetails", DbType.String, objEnquiryResponseEntity.XmlFollowDetails);
                    objDatabase.AddInParameter(objCommand, "@XmlOutFlowDetails", DbType.String, objEnquiryResponseEntity.XmlOutFlowDetails);
                    objDatabase.AddInParameter(objCommand, "@XmlCashInflowDetails", DbType.String, objEnquiryResponseEntity.XmlCashInflowDetails);
                    objDatabase.AddInParameter(objCommand, "@XmlRepaymentDetails", DbType.String, objEnquiryResponseEntity.XmlRepaymentDetails);
                    //objDatabase.AddInParameter(objCommand, "@XML_ROIRULE", DbType.String, objEnquiryResponseEntity.XML_ROIRULE);
                    objDatabase.AddInParameter(objCommand, "@XML_REPAYSTRUCTURE", DbType.String, objEnquiryResponseEntity.XML_REPAYSTRUCTURE);
                } objDatabase.AddOutParameter(objCommand, "@ErrorCode", DbType.Int32, sizeof(int));


                //objDatabase.ExecuteNonQuery(objCommand, objTransaction);

                objDatabase.FunPubExecuteNonQuery(objCommand, ref objTransaction);

                if (objCommand.Parameters["@EnquiryResponseID"].Value != null)
                {
                    strEnquiryResponseId = Convert.ToString(objCommand.Parameters["@EnquiryResponseID"].Value);
                    intErrorCode = 0;
                }
                objCommand.Parameters.Clear();
                FunPriCommitTransaction();
                strEnquiryResponse = strEnquiryResponseId;
                return intErrorCode;

            }
            catch (Exception ex)
            {
                intErrorCode = -1;
                FunPriRollbackTransaction();
                throw ex;
            }
        }

        private void FunPriOpenConnection()
        {
            try
            {
                objConnection = objDatabase.CreateConnection();
                objConnection.Open();
                objTransaction = objConnection.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FunPriRollbackTransaction()
        {
            try
            {
                if (objTransaction != null)
                {
                    objTransaction.Rollback();
                    objTransaction.Dispose();
                    objConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FunPriCommitTransaction()
        {
            try
            {
                objTransaction.Commit();
                objConnection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
