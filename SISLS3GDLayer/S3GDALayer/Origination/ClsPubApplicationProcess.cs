#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Application Process
/// Created By			: Narayanan
/// Created Date		: 06-07-2010
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
    public class ApplicationProcessDAL
    {
        int intRowsAffected;
        

        //Code added for getting common connection string  from config file
            Database db;
            public ApplicationProcessDAL()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

        public int FunPubCreateApplicationProcessInt(S3GBusEntity.ApplicationProcess.ApplicationProcess ObjApp, out string strAppNumber_Out)
        {
            strAppNumber_Out = string.Empty;
            try
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertApplicationDetails");
                db.AddInParameter(command, "@Application_Process_ID", DbType.Int32, ObjApp.Application_Process_ID);
                db.AddInParameter(command, "@PaymentRuleCardId", DbType.String, ObjApp.Payment_RuleCard_ID);
                db.AddInParameter(command, "@Date", DbType.DateTime, ObjApp.Date);
                db.AddInParameter(command, "@Business_Offer_Number", DbType.String, ObjApp.Business_Offer_Number);
                db.AddInParameter(command, "@Status_ID", DbType.String, ObjApp.Status_ID);
                db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjApp.Customer_ID);
                db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjApp.Company_ID);
                db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjApp.LOB_ID);
                db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjApp.Branch_ID);
                db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjApp.Product_ID);
                db.AddInParameter(command, "@Sales_Person_ID", DbType.Int32, ObjApp.Sales_Person_ID);
                db.AddInParameter(command, "@Business_IRR", DbType.Decimal, ObjApp.Business_IRR);
                db.AddInParameter(command, "@Company_IRR", DbType.Decimal, ObjApp.Company_IRR);
                db.AddInParameter(command, "@Accounting_IRR", DbType.Decimal, ObjApp.Accounting_IRR);
                db.AddInParameter(command, "@Finance_Amount", DbType.Decimal, ObjApp.Finance_Amount);
                db.AddInParameter(command, "@Tenure", DbType.Decimal, ObjApp.Tenure);
                db.AddInParameter(command, "@Tenure_Type", DbType.Int32, ObjApp.Tenure_Type);
                db.AddInParameter(command, "@Margin_Amount", DbType.Decimal, ObjApp.Margin_Amount);
                db.AddInParameter(command, "@Residual_Value", DbType.Decimal, ObjApp.Residual_Value);
                db.AddInParameter(command, "@Refinance_Contract", DbType.Int32, ObjApp.Refinance_Contract);
                db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjApp.Constitution_ID);
                db.AddInParameter(command, "@Lease_Type", DbType.Int32, ObjApp.Lease_Type);
                db.AddInParameter(command, "@Offer_Residual_Value", DbType.Decimal, ObjApp.Offer_Residual_Value);
                db.AddInParameter(command, "@Offer_Residual_Value_Amount", DbType.Int32, ObjApp.Offer_Residual_Value_Amount);
                db.AddInParameter(command, "@Offer_Margin", DbType.Decimal, ObjApp.Offer_Margin);
                db.AddInParameter(command, "@Offer_Margin_Amount", DbType.Decimal, ObjApp.Offer_Margin_Amount);
                db.AddInParameter(command, "@MLA_Applicable", DbType.Int32, ObjApp.MLA_Applicable);
                db.AddInParameter(command, "@MLA_Number", DbType.Int32, ObjApp.MLA_Number);
                db.AddInParameter(command, "@MLA_Validity_To", DbType.String, ObjApp.MLA_Validity_To);
                db.AddInParameter(command, "@MLA_Validity_From", DbType.String, ObjApp.MLA_Validity_From);
                db.AddInParameter(command, "@Created_By", DbType.Int32, ObjApp.Created_By);
                
                //Added By Chandru K On 18-Sep-2013 For ISFC Customization
                //db.AddInParameter(command, "@Mortgage_Type", DbType.Int32, ObjApp.Mortgage_Type);
                //db.AddInParameter(command, "@Mortgage_Fees", DbType.Decimal, ObjApp.Mortgage_Fees);
                //db.AddInParameter(command, "@StepDown_RevisionType", DbType.Int32, ObjApp.StepDown_RevisionType);
                //db.AddInParameter(command, "@XMLMortgage", DbType.String, strXMLMortgage);
                //End

                if (ObjApp.intFBDate != 0)
                {
                    db.AddInParameter(command, "@FBDate", DbType.Int32, ObjApp.intFBDate);
                }

                if (ObjApp.Loan_Type != 0)
                {
                    db.AddInParameter(command, "@Loan_Type", DbType.Int32, ObjApp.Loan_Type);
                }

                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    OracleParameter param;
                    if (ObjApp.XMLRepaymentStructure != null)
                    {
                        param = new OracleParameter("@XML_RepaymentStructure", OracleType.Clob,
                            ObjApp.XMLRepaymentStructure.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XMLRepaymentStructure);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_Constitution != null)
                    {
                    param = new OracleParameter("@XML_Constitution", OracleType.Clob,
                        ObjApp.XML_Constitution.Length, ParameterDirection.Input, true,
                        0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_Constitution);
                    command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_AssetDetails != null)
                    {
                        param = new OracleParameter("@XML_AssetDetails", OracleType.Clob,
                            ObjApp.XML_AssetDetails.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_AssetDetails);
                        command.Parameters.Add(param);
                    }
                    if (ObjApp.XML_AssetLoanHDR!= null)
                    {
                        param = new OracleParameter("@XML_AssetLoanHDR", OracleType.Clob,
                            ObjApp.XML_AssetLoanHDR.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_AssetLoanHDR);
                        command.Parameters.Add(param);
                    }
                    if (ObjApp.XML_AssetLoanDet != null)
                    {
                        param = new OracleParameter("@XML_AssetLoanDet", OracleType.Clob,
                            ObjApp.XML_AssetLoanDet.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_AssetLoanDet);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_AssetLoandoc != null)
                    {
                        param = new OracleParameter("@XML_AssetLoandoc", OracleType.Clob,
                            ObjApp.XML_AssetLoandoc.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_AssetLoandoc);
                        command.Parameters.Add(param);
                    }



                    if (ObjApp.XML_ROIRULE != null)
                    {
                        param = new OracleParameter("@XML_ROIRULE", OracleType.Clob,
                            ObjApp.XML_ROIRULE.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_ROIRULE);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_Inflow != null)
                    {
                        param = new OracleParameter("@XML_Inflow", OracleType.Clob,
                            ObjApp.XML_Inflow.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_Inflow);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_OutFlow != null)
                    {
                        param = new OracleParameter("@XML_OutFlow", OracleType.Clob,
                            ObjApp.XML_OutFlow.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_OutFlow);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_Repayment != null)
                    {
                        param = new OracleParameter("@XML_Repayment", OracleType.Clob,
                            ObjApp.XML_Repayment.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_Repayment);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_Guarantor != null)
                    {
                        param = new OracleParameter("@XML_Guarantor", OracleType.Clob,
                            ObjApp.XML_Guarantor.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_Guarantor);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_Invoice != null)
                    {
                        param = new OracleParameter("@XML_Invoice", OracleType.Clob,
                            ObjApp.XML_Invoice.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_Invoice);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_ALERT != null)
                    {
                        param = new OracleParameter("@XML_ALERT", OracleType.Clob,
                            ObjApp.XML_ALERT.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_ALERT);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_PDD != null)
                    {
                        param = new OracleParameter("@XML_PDD", OracleType.Clob,
                            ObjApp.XML_PDD.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_PDD);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_FollowDetail != null)
                    {
                        param = new OracleParameter("@XML_FollowDetail", OracleType.Clob,
                            ObjApp.XML_FollowDetail.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_FollowDetail);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_Moratorium != null)
                    {
                        param = new OracleParameter("@XML_Moratorium", OracleType.Clob,
                            ObjApp.XML_Moratorium.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_Moratorium);
                        command.Parameters.Add(param);
                    }


                    //Added by saran on 4-Jul-2014 for CR_SISSL12E046_018 start
                    if (ObjApp.XMLRepayDetailsOthers != null)
                    {
                        param = new OracleParameter("@XMLRepayDetailsOthers",
                            OracleType.Clob, ObjApp.XMLRepayDetailsOthers.Length,
                            ParameterDirection.Input, true, 0, 0, String.Empty,
                            DataRowVersion.Default, ObjApp.XMLRepayDetailsOthers);
                        command.Parameters.Add(param);

                    }
                    //Added by saran on 4-Jul-2014 for CR_SISSL12E046_018 end
                }
                else
                {
                    db.AddInParameter(command, "@XML_RepaymentStructure", DbType.String, 
                        ObjApp.XMLRepaymentStructure);
                    db.AddInParameter(command, "@XML_Constitution", DbType.String, ObjApp.XML_Constitution);
                    db.AddInParameter(command, "@XML_AssetDetails", DbType.String, ObjApp.XML_AssetDetails);
                    db.AddInParameter(command, "@XML_AssetLoanDet", DbType.String, ObjApp.XML_AssetLoanDet);
                    db.AddInParameter(command, "@XML_AssetLoandoc", DbType.String, ObjApp.XML_AssetLoandoc);
                    db.AddInParameter(command, "@XML_AssetLoanHDR", DbType.String, ObjApp.XML_AssetLoanHDR);
                    db.AddInParameter(command, "@XML_ROIRULE", DbType.String, ObjApp.XML_ROIRULE);
                    db.AddInParameter(command, "@XML_Inflow", DbType.String, ObjApp.XML_Inflow);
                    db.AddInParameter(command, "@XML_OutFlow", DbType.String, ObjApp.XML_OutFlow);
                    db.AddInParameter(command, "@XML_Repayment", DbType.String, ObjApp.XML_Repayment);
                    db.AddInParameter(command, "@XML_Guarantor", DbType.String, ObjApp.XML_Guarantor);
                    db.AddInParameter(command, "@XML_Invoice", DbType.String, ObjApp.XML_Invoice);
                    db.AddInParameter(command, "@XML_ALERT", DbType.String, ObjApp.XML_ALERT);
                    db.AddInParameter(command, "@XML_PDD", DbType.String, ObjApp.XML_PDD);
                    db.AddInParameter(command, "@XML_FollowDetail", DbType.String, ObjApp.XML_FollowDetail);
                    db.AddInParameter(command, "@XML_Moratorium", DbType.String, ObjApp.XML_Moratorium);
                    //Added by saran on 4-Jul-2014 for CR_SISSL12E046_018 start
                    if (ObjApp.XMLRepayDetailsOthers != null)
                    {
                        db.AddInParameter(command, "@XMLRepayDetailsOthers", DbType.String, ObjApp.XMLRepayDetailsOthers);
                    }
                    //Added by saran on 4-Jul-2014 for CR_SISSL12E046_018 end
                }



                db.AddOutParameter(command, "@Application_Number", DbType.String, 100);
                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        db.FunPubExecuteNonQuery(command, ref trans);
                        
                        if ((int)command.Parameters["@ErrorCode"].Value != 0)
                        {
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                        }
                        else
                        {
                            strAppNumber_Out = (string)command.Parameters["@Application_Number"].Value;
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
                intRowsAffected = 50;
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

            }
            return intRowsAffected;

          
        }

        public int FunPubUpdateApplicationStatus(S3GBusEntity.ApplicationProcess.ApplicationProcess objApplicationProcessEntity)
        {
            int intErrorCode = 0;
            try
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_LoanAd_CancelApplication");

                db.AddInParameter(command, "@ApplicationProcessId", DbType.Int32, objApplicationProcessEntity.Application_Process_ID);
                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                db.FunPubExecuteNonQuery(command);
                if (command.Parameters["@ErrorCode"].Value != null)
                {
                    intErrorCode = Convert.ToInt32(command.Parameters["@ErrorCode"].Value);
                }
                return intErrorCode;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int FunPubCreateApplicationProcessGoldLoanInt(S3GBusEntity.ApplicationProcess.ApplicationProcess ObjApp, out string strAppNumber_Out)
        {
            strAppNumber_Out = string.Empty;
            try
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertApplicationDetailsGL");
                db.AddInParameter(command, "@Application_Process_ID", DbType.Int32, ObjApp.Application_Process_ID);
                db.AddInParameter(command, "@PaymentRuleCardId", DbType.String, ObjApp.Payment_RuleCard_ID);
                db.AddInParameter(command, "@Date", DbType.DateTime, ObjApp.Date);
                db.AddInParameter(command, "@Business_Offer_Number", DbType.String, ObjApp.Business_Offer_Number);
                db.AddInParameter(command, "@Status_ID", DbType.String, ObjApp.Status_ID);
                db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjApp.Customer_ID);
                db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjApp.Company_ID);
                db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjApp.LOB_ID);
                db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjApp.Branch_ID);
                db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjApp.Product_ID);
                db.AddInParameter(command, "@Sales_Person_ID", DbType.Int32, ObjApp.Sales_Person_ID);
                db.AddInParameter(command, "@Business_IRR", DbType.Decimal, ObjApp.Business_IRR);
                db.AddInParameter(command, "@Company_IRR", DbType.Decimal, ObjApp.Company_IRR);
                db.AddInParameter(command, "@Accounting_IRR", DbType.Decimal, ObjApp.Accounting_IRR);
                db.AddInParameter(command, "@Finance_Amount", DbType.Decimal, ObjApp.Finance_Amount);
                db.AddInParameter(command, "@Tenure", DbType.Decimal, ObjApp.Tenure);
                db.AddInParameter(command, "@Tenure_Type", DbType.Int32, ObjApp.Tenure_Type);
                db.AddInParameter(command, "@Margin_Amount", DbType.Decimal, ObjApp.Margin_Amount);
                db.AddInParameter(command, "@Residual_Value", DbType.Decimal, ObjApp.Residual_Value);
                db.AddInParameter(command, "@Refinance_Contract", DbType.Int32, ObjApp.Refinance_Contract);
                db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjApp.Constitution_ID);
                db.AddInParameter(command, "@Lease_Type", DbType.Int32, ObjApp.Lease_Type);
                db.AddInParameter(command, "@Offer_Residual_Value", DbType.Decimal, ObjApp.Offer_Residual_Value);
                db.AddInParameter(command, "@Offer_Residual_Value_Amount", DbType.Int32, ObjApp.Offer_Residual_Value_Amount);
                db.AddInParameter(command, "@Offer_Margin", DbType.Decimal, ObjApp.Offer_Margin);
                db.AddInParameter(command, "@Offer_Margin_Amount", DbType.Decimal, ObjApp.Offer_Margin_Amount);
                db.AddInParameter(command, "@MLA_Applicable", DbType.Int32, ObjApp.MLA_Applicable);
                db.AddInParameter(command, "@MLA_Number", DbType.Int32, ObjApp.MLA_Number);
                db.AddInParameter(command, "@MLA_Validity_To", DbType.String, ObjApp.MLA_Validity_To);
                db.AddInParameter(command, "@MLA_Validity_From", DbType.String, ObjApp.MLA_Validity_From);
                db.AddInParameter(command, "@Created_By", DbType.Int32, ObjApp.Created_By);
                if (ObjApp.intFBDate != 0)
                {
                    db.AddInParameter(command, "@FBDate", DbType.Int32, ObjApp.intFBDate);
                }


                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    OracleParameter param;
                    if (ObjApp.XMLRepaymentStructure != null)
                    {
                        param = new OracleParameter("@XML_RepaymentStructure", OracleType.Clob,
                            ObjApp.XMLRepaymentStructure.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XMLRepaymentStructure);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_Constitution != null)
                    {
                        param = new OracleParameter("@XML_Constitution", OracleType.Clob,
                            ObjApp.XML_Constitution.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_Constitution);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_AssetDetails != null)
                    {
                        param = new OracleParameter("@XML_AssetDetails", OracleType.Clob,
                            ObjApp.XML_AssetDetails.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_AssetDetails);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_ROIRULE != null)
                    {
                        param = new OracleParameter("@XML_ROIRULE", OracleType.Clob,
                            ObjApp.XML_ROIRULE.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_ROIRULE);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_Inflow != null)
                    {
                        param = new OracleParameter("@XML_Inflow", OracleType.Clob,
                            ObjApp.XML_Inflow.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_Inflow);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_OutFlow != null)
                    {
                        param = new OracleParameter("@XML_OutFlow", OracleType.Clob,
                            ObjApp.XML_OutFlow.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_OutFlow);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_Repayment != null)
                    {
                        param = new OracleParameter("@XML_Repayment", OracleType.Clob,
                            ObjApp.XML_Repayment.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_Repayment);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_Guarantor != null)
                    {
                        param = new OracleParameter("@XML_Guarantor", OracleType.Clob,
                            ObjApp.XML_Guarantor.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_Guarantor);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_Invoice != null)
                    {
                        param = new OracleParameter("@XML_Invoice", OracleType.Clob,
                            ObjApp.XML_Invoice.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_Invoice);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_ALERT != null)
                    {
                        param = new OracleParameter("@XML_ALERT", OracleType.Clob,
                            ObjApp.XML_ALERT.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_ALERT);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_PDD != null)
                    {
                        param = new OracleParameter("@XML_PDD", OracleType.Clob,
                            ObjApp.XML_PDD.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_PDD);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_FollowDetail != null)
                    {
                        param = new OracleParameter("@XML_FollowDetail", OracleType.Clob,
                            ObjApp.XML_FollowDetail.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_FollowDetail);
                        command.Parameters.Add(param);
                    }

                    if (ObjApp.XML_Moratorium != null)
                    {
                        param = new OracleParameter("@XML_Moratorium", OracleType.Clob,
                            ObjApp.XML_Moratorium.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjApp.XML_Moratorium);
                        command.Parameters.Add(param);
                    }
                }
                else
                {
                    db.AddInParameter(command, "@XML_RepaymentStructure", DbType.String,
                        ObjApp.XMLRepaymentStructure);
                    db.AddInParameter(command, "@XML_Constitution", DbType.String, ObjApp.XML_Constitution);
                    db.AddInParameter(command, "@XML_AssetDetails", DbType.String, ObjApp.XML_AssetDetails);
                    db.AddInParameter(command, "@XML_ROIRULE", DbType.String, ObjApp.XML_ROIRULE);
                    db.AddInParameter(command, "@XML_Inflow", DbType.String, ObjApp.XML_Inflow);
                    db.AddInParameter(command, "@XML_OutFlow", DbType.String, ObjApp.XML_OutFlow);
                    db.AddInParameter(command, "@XML_Repayment", DbType.String, ObjApp.XML_Repayment);
                    db.AddInParameter(command, "@XML_Guarantor", DbType.String, ObjApp.XML_Guarantor);
                    db.AddInParameter(command, "@XML_Invoice", DbType.String, ObjApp.XML_Invoice);
                    db.AddInParameter(command, "@XML_ALERT", DbType.String, ObjApp.XML_ALERT);
                    db.AddInParameter(command, "@XML_PDD", DbType.String, ObjApp.XML_PDD);
                    db.AddInParameter(command, "@XML_FollowDetail", DbType.String, ObjApp.XML_FollowDetail);
                    db.AddInParameter(command, "@XML_Moratorium", DbType.String, ObjApp.XML_Moratorium);
                }

                db.AddOutParameter(command, "@Application_Number", DbType.String, 100);
                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        db.FunPubExecuteNonQuery(command, ref trans);

                        if ((int)command.Parameters["@ErrorCode"].Value != 0)
                        {
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                        }
                        else
                        {
                            strAppNumber_Out = (string)command.Parameters["@Application_Number"].Value;
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
                intRowsAffected = 50;
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

            }
            return intRowsAffected;


        }

        /*public void FunPub_Insert_ROI(S3GBusEntity.ApplicationProcess.Offer_ROI_Details ObjApp)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_ApplicationProcessOffer_ROI_Details_Save");

                db.AddInParameter(command, "@Application_Process_ID", DbType.Int32, ObjApp.Application_Process_ID);
                db.AddInParameter(command, "@ROI_Rules_ID", DbType.Int32, ObjApp.ROI_Rules_ID);
                db.AddInParameter(command, "@Model_Description", DbType.String, ObjApp.Model_Description);
                db.AddInParameter(command, "@Rate_Type", DbType.Int32, ObjApp.Rate_Type);
                db.AddInParameter(command, "@ROI_Rule_Number", DbType.String, ObjApp.ROI_Rule_Number);
                db.AddInParameter(command, "@Return_Pattern", DbType.Int32, ObjApp.Return_Pattern);
                db.AddInParameter(command, "@Time_Value", DbType.Int32, ObjApp.Time_Value);
                db.AddInParameter(command, "@Frequency", DbType.Int32, ObjApp.Frequency);
                db.AddInParameter(command, "@Repayment_Mode", DbType.Int32, ObjApp.Repayment_Mode);
                db.AddInParameter(command, "@Rate", DbType.Decimal, ObjApp.Rate);
                db.AddInParameter(command, "@IRR_Rest", DbType.Int32, ObjApp.IRR_Rest);
                db.AddInParameter(command, "@Interest_Calculation", DbType.Int32, ObjApp.Interest_Calculation);
                db.AddInParameter(command, "@Interest_Levy", DbType.Int32, ObjApp.Interest_Levy);
                db.AddInParameter(command, "@Recovery_Pattern_Year1", DbType.Decimal, ObjApp.Recovery_Pattern_Year1);
                db.AddInParameter(command, "@Recovery_Pattern_Year2", DbType.Decimal, ObjApp.Recovery_Pattern_Year2);
                db.AddInParameter(command, "@Recovery_Pattern_Year3", DbType.Decimal, ObjApp.Recovery_Pattern_Year3);
                db.AddInParameter(command, "@Recovery_Pattern_Rest", DbType.Decimal, ObjApp.Recovery_Pattern_Rest);
                db.AddInParameter(command, "@Insurance", DbType.Int32, ObjApp.Insurance);
                db.AddInParameter(command, "@Residual_Value", DbType.Int32, ObjApp.Residual_Value);
                db.AddInParameter(command, "@Margin", DbType.Int32, ObjApp.Margin);
                db.AddInParameter(command, "@Margin_Percentage", DbType.Decimal, ObjApp.Margin_Percentage);

                db.FunPubExecuteNonQuery(command, DBTrans);

                Commit();

                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                RollBack();
                throw ex;
            }
        }*/

        /*

        public void FunPub_Insert_ConstituionDocuments(S3GBusEntity.ApplicationProcess.DocumentsDetails ObjApp)
        {
            try
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_ApplicationProcessDocDetails_Save");

                db.AddInParameter(command, "@Application_Process_ID", DbType.Int32, ObjApp.Application_Process_ID);
                db.AddInParameter(command, "@ConstitutionDocumentCategory_ID", DbType.Int32, ObjApp.ConstitutionDocumentCategory_ID);
                db.AddInParameter(command, "@Remarks", DbType.String, ObjApp.Remarks);
                db.AddInParameter(command, "@Is_Collected", DbType.Int32, ObjApp.Is_Collected);
                db.AddInParameter(command, "@Is_Scanned", DbType.Int32, ObjApp.Is_Scanned);
                db.AddInParameter(command, "@Value", DbType.String, ObjApp.Value);
                db.AddInParameter(command, "@Is_FollowUp", DbType.Int32, ObjApp.Is_FollowUp);

                db.FunPubExecuteNonQuery(command, DBTrans);

                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                RollBack();
                throw ex;
            }
        }

        public void FunPub_Insert_AssetDetail(S3GBusEntity.ApplicationProcess.AssetDetails ObjApp)
        {
            try
            {
                //  Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_ApplicationProcessAssetDetails_Save");

                db.AddInParameter(command, "@Application_Process_ID", DbType.Int32, ObjApp.Application_Process_ID);
                db.AddInParameter(command, "@Asset_ID", DbType.Int32, ObjApp.Asset_ID);
                db.AddInParameter(command, "@Required_From", DbType.String, ObjApp.Required_From);
                db.AddInParameter(command, "@No_Of_Units", DbType.Decimal, ObjApp.No_Of_Units);
                db.AddInParameter(command, "@Unit_Value", DbType.Decimal, ObjApp.Unit_Value);
                db.AddInParameter(command, "@Margin_Percentage", DbType.Decimal, ObjApp.Margin_Percentage);
                db.AddInParameter(command, "@Margin_Amount", DbType.Decimal, ObjApp.Margin_Amount);
                db.AddInParameter(command, "@Book_Depreciation_Percentage", DbType.Decimal, ObjApp.Book_Depreciation_Percentage);
                db.AddInParameter(command, "@Block_Depreciation_Percentage", DbType.Decimal, ObjApp.Block_Depreciation_Percentage);
                db.AddInParameter(command, "@Finance_Amount", DbType.Decimal, ObjApp.Finance_Amount);
                db.AddInParameter(command, "@Capital_Portion", DbType.Decimal, ObjApp.Capital_Portion);
                db.AddInParameter(command, "@Non_Capital_Portion", DbType.Decimal, ObjApp.Non_Capital_Portion);
                db.AddInParameter(command, "@Pay_To", DbType.Int32, ObjApp.Pay_To);
                db.AddInParameter(command, "@Payment_Percentage", DbType.Decimal, ObjApp.Payment_Percentage);
                db.AddInParameter(command, "@Is_Proforma", DbType.Int32, ObjApp.Is_Proforma);
                db.AddInParameter(command, "@Entity_ID", DbType.Int32, ObjApp.Entity_ID);

                db.FunPubExecuteNonQuery(command, DBTrans);

                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                RollBack();
                throw ex;
            }
        }
        
        public void FunPub_Insert_PaymentRuleCard(S3GBusEntity.ApplicationProcess.Offer_PaymentRuleCard ObjApp)
        {
            try
            {
                //   Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_ApplicationProcessOffer_PaymentRuleCard_Details_Save");

                db.AddInParameter(command, "@Application_Process_ID", DbType.Int32, ObjApp.Application_Process_ID);
                db.AddInParameter(command, "@Payment_RuleCard_ID", DbType.Int32, ObjApp.Payment_RuleCard_ID);
                //db.AddInParameter(command, "@Payment_Rule_Number", DbType.Int32, ObjApp.Payment_Rule_Number);
                //db.AddInParameter(command, "@AccountType_ID", DbType.Int32, ObjApp.AccountType_ID);
                //db.AddInParameter(command, "@Entity_ID", DbType.Int32, ObjApp.Entity_ID);
                //db.AddInParameter(command, "@Compensation_Percentage", DbType.Int32, ObjApp.Compensation_Percentage);
                //db.AddInParameter(command, "@Compensation_Levy_Pattern", DbType.Int32, ObjApp.Compensation_Levy_Pattern);
                //db.AddInParameter(command, "@Levy_Frequency", DbType.Int32, ObjApp.Levy_Frequency);
                //db.AddInParameter(command, "@Is_OnTap_Refinance", DbType.Int32, ObjApp.Is_OnTap_Refinance);

                db.FunPubExecuteNonQuery(command, DBTrans);

                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                RollBack();
                throw ex;
            }
        }

        public void FunPub_Insert_RepaymentDetails(S3GBusEntity.ApplicationProcess.RepaymentDetails ObjApp)
        {
            try
            {
                //  Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_ApplicationProcessRepayDetails_Save");

                db.AddInParameter(command, "@Application_Process_ID", DbType.Int32, ObjApp.Application_Process_ID);
                db.AddInParameter(command, "@Repayment_CashFlow", DbType.Int32, ObjApp.Repayment_CashFlow);
                db.AddInParameter(command, "@Amount", DbType.Decimal, ObjApp.Amount);
                db.AddInParameter(command, "@Per_Instalment_Amount", DbType.Decimal, ObjApp.Per_Instalment_Amount);
                db.AddInParameter(command, "@Breakup_Percentage", DbType.Decimal, ObjApp.Breakup_Percentage);
                db.AddInParameter(command, "@From_Instalment", DbType.Decimal, ObjApp.From_Instalment);
                db.AddInParameter(command, "@To_Instalment", DbType.Decimal, ObjApp.To_Instalment);
                db.AddInParameter(command, "@From_Date", DbType.String, ObjApp.From_Date);
                db.AddInParameter(command, "@To_Date", DbType.String, ObjApp.To_Date);

                db.FunPubExecuteNonQuery(command, DBTrans);

                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                RollBack();
                throw ex;
            }

        }

        public void FunPub_Insert_GuarantorDetails(S3GBusEntity.ApplicationProcess.GuarantorDetails ObjApp)
        {
            try
            {
                //  Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_ApplicationProcessGuarantorDetails_Save");

                db.AddInParameter(command, "@Application_Process_ID", DbType.Int32, ObjApp.Application_Process_ID);
                db.AddInParameter(command, "@Guarantee_ID", DbType.Int32, ObjApp.Guarantee_ID);
                db.AddInParameter(command, "@Guarantee_Amount", DbType.Int32, ObjApp.Guarantee_Amount);
                db.AddInParameter(command, "@Charge_Sequence", DbType.Int32, ObjApp.Charge_Sequence);
                db.AddInParameter(command, "@Guarantee_Type_ID", DbType.Int32, ObjApp.Guarantee_Type_ID);

                db.FunPubExecuteNonQuery(command, DBTrans);

                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                RollBack();
                throw ex;
            }
        }

        public void FunPub_Insert_Alerts(S3GBusEntity.ApplicationProcess.AlertDetails ObjApp)
        {
            try
            {
                //   Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_ApplicationProcessAlertDetails_Save");

                db.AddInParameter(command, "@Application_Process_ID", DbType.Int32, ObjApp.Application_Process_ID);
                db.AddInParameter(command, "@Alerts_Type", DbType.Int32, ObjApp.Alerts_Type);
                db.AddInParameter(command, "@Alerts_UserContact", DbType.Int32, ObjApp.Alerts_UserContact);
                db.AddInParameter(command, "@Alerts_SMS", DbType.Int32, ObjApp.Alerts_SMS);
                db.AddInParameter(command, "@Alerts_EMail", DbType.Int32, ObjApp.Alerts_EMail);

                db.FunPubExecuteNonQuery(command, DBTrans);

                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                RollBack();
                throw ex;
            }

        }

        public Int32 FunPub_Insert_Follow_Header(S3GBusEntity.ApplicationProcess.FollowUp_Header ObjApp)
        {
            try
            {
                Int32 Id;

                //   Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_Application_FollowUp_Header_Save");

                db.AddInParameter(command, "@Program_ID", DbType.Int32, ObjApp.Program_ID);
                db.AddInParameter(command, "@Program_PK_ID", DbType.Int32, ObjApp.Program_PK_ID);
                db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjApp.LOB_ID);
                db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjApp.Branch_ID);
                db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjApp.Company_ID);
                db.AddInParameter(command, "@Application_Number", DbType.Int32, ObjApp.Application_Number);
                //db.AddInParameter(command, "@Date", DbType.Int32, ObjApp.Date);
                db.AddInParameter(command, "@Created_By", DbType.Int32, ObjApp.Created_By);
                db.AddOutParameter(command, "@FollowID", DbType.Int32, ObjApp.Created_By);

                db.FunPubExecuteNonQuery(command, DBTrans);
                Id = Convert.ToInt32(command.Parameters["@FollowID"].Value);
                
                command.Parameters.Clear();

                return Id;
            }
            catch (Exception ex)
            {
                RollBack();
                throw ex;
            }

        }

        public void FunPub_Insert_Follow_Details(S3GBusEntity.ApplicationProcess.FollowUp_Detail ObjApp)
        {
            try
            {
               // Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_ApplicationProcessFollowUp_Detail_Save");

                db.AddInParameter(command, "@Follow_Up_ID", DbType.Int32, ObjApp.Follow_Up_ID);
                db.AddInParameter(command, "@From_UserID", DbType.Int32, ObjApp.From_UserID);
                db.AddInParameter(command, "@To_UserID", DbType.Int32, ObjApp.To_UserID);
                db.AddInParameter(command, "@Action", DbType.String, ObjApp.Action);
                db.AddInParameter(command, "@Date", DbType.String, ObjApp.Date);
                db.AddInParameter(command, "@Action_Date", DbType.String, ObjApp.Action_Date);
                db.AddInParameter(command, "@Customer_Response", DbType.String, ObjApp.Customer_Response);
                db.AddInParameter(command, "@Remarks", DbType.String, ObjApp.Remarks);
                db.AddInParameter(command, "@Created_By", DbType.Int32, ObjApp.Created_By);

                db.FunPubExecuteNonQuery(command, DBTrans);

                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                RollBack();
                throw ex;
            }

        }

        public void FunPub_Insert_MoratoriumDetails(S3GBusEntity.ApplicationProcess.MoratoriumDetails ObjApp)
        {
            try
            {
               // Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_ApplicationProcessMoratoriumDetails_Save");

                db.AddInParameter(command, "@Application_Process_ID", DbType.Int32, ObjApp.Application_Process_ID);
                db.AddInParameter(command, "@Moratorium_Type", DbType.Int32, ObjApp.Moratorium_Type);
                db.AddInParameter(command, "@From_Date", DbType.String, ObjApp.From_Date);
                db.AddInParameter(command, "@To_Date", DbType.String, ObjApp.To_Date);

                db.FunPubExecuteNonQuery(command, DBTrans);

                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                RollBack();
                throw ex;
            }

        }

        public void FunPub_Insert_OfferDetails_CashFlow(S3GBusEntity.ApplicationProcess.OfferDetails_CashFlow ObjApp, bool DoCommit)
        {
            try
            {
               // Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_ApplicationProcessOfferDetails_Save");

                db.AddInParameter(command, "@Application_Process_ID", DbType.Int32, ObjApp.Application_Process_ID);
                db.AddInParameter(command, "@CashFlow_ID", DbType.Int32, ObjApp.CashFlow_ID);
                db.AddInParameter(command, "@Date", DbType.String, ObjApp.Date);
                db.AddInParameter(command, "@Entity", DbType.Int32, ObjApp.Entity);
                db.AddInParameter(command, "@Amount", DbType.Int32, ObjApp.Amount);
                db.AddInParameter(command, "@InFlow_PayTo", DbType.Int32, ObjApp.InFlow_PayTo);

                db.FunPubExecuteNonQuery(command, DBTrans);
                command.Parameters.Clear();

                if (DoCommit)
                    Commit();
            }
            catch (Exception ex)
            {
                RollBack();
                throw ex;
            }

        }
        
         */

       
        
    }
}
