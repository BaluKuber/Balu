using System;
using S3GDALayer.S3GAdminServices;
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
using System.Data.OracleClient;

namespace S3GDALayer.Origination
{
    namespace PricingMgtServices
    {
        public class ClsPubPricing
        {
            #region Intialize
            int intRowsAffected;
            S3GBusEntity.Origination.PricingMgtServices.S3G_ORG_PricingDataTable ObjPricing_DAL;
            //S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetMasterDataTable ObjAssetMaster_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubPricing()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion

            public int FunPubCreatePricingInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_Pricing, out string strOfferNumber_Out)
            {
                strOfferNumber_Out = string.Empty;
                try
                {
                    ObjPricing_DAL = (ORG.PricingMgtServices.S3G_ORG_PricingDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_Pricing, SerMode, typeof(ORG.PricingMgtServices.S3G_ORG_PricingDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (ORG.PricingMgtServices.S3G_ORG_PricingRow ObjPricingRow in ObjPricing_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_Insert_Proposal");
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjPricingRow.Company_ID);
                        if (!ObjPricingRow.IsPricing_IDNull())
                            db.AddInParameter(command, "@Pricing_ID", DbType.Int32, ObjPricingRow.Pricing_ID);
                        if (!ObjPricingRow.IsLOB_IDNull())
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPricingRow.LOB_ID);
                        if (!ObjPricingRow.IsBranch_IDNull())
                            db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjPricingRow.Branch_ID);
                        if (!ObjPricingRow.IsMRA_IDNull())
                            db.AddInParameter(command, "@MRA_ID", DbType.Int32, ObjPricingRow.MRA_ID);
                        if (!ObjPricingRow.IsCustomer_IDNull())
                            db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjPricingRow.Customer_ID);
                        if (!ObjPricingRow.IsBusiness_Offer_NumberNull())
                            db.AddInParameter(command, "@Business_Offer_Number", DbType.String, ObjPricingRow.Business_Offer_Number);
                        if (!ObjPricingRow.IsLead_IDNull())
                            db.AddInParameter(command, "@Lead_ID", DbType.Int32, ObjPricingRow.Lead_ID);
                        if (!ObjPricingRow.IsOffer_DateNull())
                            db.AddInParameter(command, "@Offer_Date", DbType.DateTime, ObjPricingRow.Offer_Date);
                        if (!ObjPricingRow.IsFacility_AmountNull())
                            db.AddInParameter(command, "@Facility_Amount", DbType.Decimal, ObjPricingRow.Facility_Amount);
                        if (!ObjPricingRow.IsOffer_Valid_TillNull())
                            db.AddInParameter(command, "@Offer_Valid_Till", DbType.DateTime, ObjPricingRow.Offer_Valid_Till);
                        if (!ObjPricingRow.IsProduct_IDNull())
                            db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjPricingRow.Product_ID);
                        if (!ObjPricingRow.IsConstitution_IDNull())
                            db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjPricingRow.Constitution_ID);
                        if (!ObjPricingRow.IsProposal_TypeNull())
                            db.AddInParameter(command, "@Proposal_Type", DbType.Int32, ObjPricingRow.Proposal_Type);
                        if (!ObjPricingRow.IsAdv_Rent__ApplicabilityNull())
                            db.AddInParameter(command, "@Adv_Rent__Applicability", DbType.Int32, ObjPricingRow.Adv_Rent__Applicability);
                        if (!ObjPricingRow.IsSecu_Deposit_TypeNull())
                            db.AddInParameter(command, "@Secu_Deposit_Type", DbType.Int32, ObjPricingRow.Secu_Deposit_Type);
                        if (!ObjPricingRow.IsSecu_Rent_AmountNull())
                            db.AddInParameter(command, "@Secu_Rent_Amount", DbType.Decimal, ObjPricingRow.Secu_Rent_Amount);
                        if (!ObjPricingRow.IsReturnPatternNull())
                            db.AddInParameter(command, "@ReturnPattern", DbType.Int32, ObjPricingRow.ReturnPattern);
                        if (!ObjPricingRow.IsSeco_Term_ApplicabilityNull())
                            db.AddInParameter(command, "@Seco_Term_Applicability", DbType.Int32, ObjPricingRow.Seco_Term_Applicability);
                        if (!ObjPricingRow.IsOne_Time_FeeNull())
                            db.AddInParameter(command, "@One_Time_Fee", DbType.Decimal, ObjPricingRow.One_Time_Fee);
                        if (!ObjPricingRow.IsRepayment_ModeNull())
                            db.AddInParameter(command, "@Repayment_Mode", DbType.Int32, ObjPricingRow.Repayment_Mode);

                        db.AddInParameter(command, "@Amt_Based_On", DbType.Decimal, ObjPricingRow.Amt_Based_On);
                        db.AddInParameter(command, "@PROCESSING_AMT_BASED_ON", DbType.Decimal, ObjPricingRow.PROCESSING_AMT_BASED_ON);
                        db.AddInParameter(command, "@OneTime_Processing", DbType.String, "");

                        if (!ObjPricingRow.IsProcessing_Fee_PerNull())
                            db.AddInParameter(command, "@Processing_Fee_Per", DbType.Decimal, ObjPricingRow.Processing_Fee_Per);
                        if (!ObjPricingRow.IsPROCESSING_FEE_RATENull())
                            db.AddInParameter(command, "@PROCESSING_FEE_RATE", DbType.Decimal, ObjPricingRow.PROCESSING_FEE_RATE);
                        if (!ObjPricingRow.IsONE_TIME_RATENull())
                            db.AddInParameter(command, "@ONE_TIME_RATE", DbType.Decimal, ObjPricingRow.ONE_TIME_RATE);
                        if (!ObjPricingRow.IsVAT_Rebate_ApplicabilityNull())
                            db.AddInParameter(command, "@VAT_Rebate_Applicability", DbType.Int32, ObjPricingRow.VAT_Rebate_Applicability);
                        if (!ObjPricingRow.IsRemarksNull())
                            db.AddInParameter(command, "@Remarks", DbType.String, ObjPricingRow.Remarks);

                        if (!ObjPricingRow.IsGuaranteed_EOTNull())
                            db.AddInParameter(command, "@Guaranteed_EOT", DbType.String, ObjPricingRow.Guaranteed_EOT);
                        if (!ObjPricingRow.IsGuaranteed_EOT_AppNull())
                            db.AddInParameter(command, "@Guaranteed_EOT_App", DbType.String, ObjPricingRow.Guaranteed_EOT_App);

                        if (!ObjPricingRow.IsSecurity_Depost_onNull())
                            db.AddInParameter(command, "@Security_Depost_on", DbType.Int32, ObjPricingRow.Security_Depost_on);

                        if (!ObjPricingRow.IsCustomer_IRRNull())
                            db.AddInParameter(command, "@Customer_IRR", DbType.Decimal, ObjPricingRow.Customer_IRR);
                        if (!ObjPricingRow.IsStatus_IDNull())
                            db.AddInParameter(command, "@Status_ID", DbType.Int32, ObjPricingRow.Status_ID);
                        if (!ObjPricingRow.IsRound_NoNull())
                            db.AddInParameter(command, "@Round_No", DbType.Int32, ObjPricingRow.Round_No);
                        if (!ObjPricingRow.IsAuth_IDNull())
                            db.AddInParameter(command, "@Auth_ID", DbType.String, ObjPricingRow.Auth_ID);
                        if (!ObjPricingRow.IsXMLPrimaryGridNull())
                            db.AddInParameter(command, "@XMLPrimaryGrid", DbType.String, ObjPricingRow.XMLPrimaryGrid);
                        if (!ObjPricingRow.IsXMLSecondaryGridNull())
                            db.AddInParameter(command, "@XMLSecondaryGrid", DbType.String, ObjPricingRow.XMLSecondaryGrid);
                        if (!ObjPricingRow.IsXMLEUCDtlsNull())

                        if (!ObjPricingRow.IsXMLRebateDetGridNull())
                            db.AddInParameter(command, "@XMLRebateDetGrid", DbType.String, ObjPricingRow.XMLRebateDetGrid);

                        db.AddInParameter(command, "@Is_Guaranteed_EOT_Appli", DbType.Int32, ObjPricingRow.Is_Guaranteed_EOT_Appli);

                        db.AddInParameter(command, "@Rebate_Discount_Appl", DbType.Int32, ObjPricingRow.Rebate_Discount_Appl);

                        db.AddInParameter(command, "@Addi_Rebate_Discount_Appl", DbType.Int32, ObjPricingRow.Addi_Rebate_Discount_Appl);

                        if (!ObjPricingRow.IsRebate_Struc_AllocationNull())
                            db.AddInParameter(command, "@Rebate_Struc_Allocation", DbType.Int32, ObjPricingRow.Rebate_Struc_Allocation);

                        if (!ObjPricingRow.IsRebate_Discount_PercNull())
                            db.AddInParameter(command, "@Rebate_Discount_Perc", DbType.Decimal, ObjPricingRow.Rebate_Discount_Perc);

                        if (!ObjPricingRow.IsAddi_Rebate_Discount_PercNull())
                            db.AddInParameter(command, "@Addi_Rebate_Discount_Perc", DbType.Decimal, ObjPricingRow.Addi_Rebate_Discount_Perc);

                       if (!ObjPricingRow.IsNo_of_Installments_RebateNull())
                            db.AddInParameter(command, "@No_of_Installments_Rebate", DbType.Int32, ObjPricingRow.No_of_Installments_Rebate);

                        if (!ObjPricingRow.IsAddi_No_of_Installments_RebateNull())
                            db.AddInParameter(command, "@Addi_No_of_Installments_Rebate", DbType.Int32, ObjPricingRow.Addi_No_of_Installments_Rebate);

                        if (!ObjPricingRow.IsRebate_Allowed_MethodNull())
                            db.AddInParameter(command, "@Rebate_Allowed_Method", DbType.Int32, ObjPricingRow.Rebate_Allowed_Method);

                        if (!ObjPricingRow.IsAddi_Rebate_Allowed_Method_NNull())
                            db.AddInParameter(command, "@Addi_Rebate_Allowed_Method", DbType.Int32, ObjPricingRow.Addi_Rebate_Allowed_Method_N);

                        db.AddInParameter(command, "@XMLEUCDtls", DbType.String, ObjPricingRow.XMLEUCDtls);

                        db.AddInParameter(command, "@Sec_Dep_App", DbType.Int32, ObjPricingRow.Sec_Dep_App);
                        db.AddInParameter(command, "@Rental_Based_On", DbType.Int32, ObjPricingRow.Rental_Based_On);
                        //opc210 start
                        db.AddInParameter(command, "@Upload_Path", DbType.String, ObjPricingRow.Upload_Path);
                        //opc210 end
                        if (!ObjPricingRow.IsTerm_Sheet_DateNull())
                            db.AddInParameter(command, "@Term_Sheet_Date", DbType.DateTime, ObjPricingRow.Term_Sheet_Date);
                        if (!ObjPricingRow.IsCreated_ByNull())
                            db.AddInParameter(command, "@Created_By", DbType.Int32, ObjPricingRow.Created_By);
                        db.AddInParameter(command, "@Stamp_duty_app", DbType.Int32, ObjPricingRow.Stamp_Duty_App);
                        db.AddInParameter(command, "@Interim_applicable", DbType.Int32, ObjPricingRow.Interim_Applicable);
                        db.AddOutParameter(command, "@Offer_No", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                strOfferNumber_Out = (string)command.Parameters["@Offer_No"].Value;
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
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                }
                return intRowsAffected;
            }

            public int FunPubModifyPricingInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_Pricing)
            {

                try
                {
                    ObjPricing_DAL = (ORG.PricingMgtServices.S3G_ORG_PricingDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_Pricing, SerMode, typeof(ORG.PricingMgtServices.S3G_ORG_PricingDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (ORG.PricingMgtServices.S3G_ORG_PricingRow ObjPricingRow in ObjPricing_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_UpdatePricingDetails");
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjPricingRow.Company_ID);
                        db.AddInParameter(command, "@Pricing_Id", DbType.Int32, ObjPricingRow.Pricing_ID);

                        if (!ObjPricingRow.IsEnquiry_Response_IdNull())
                        {
                            db.AddInParameter(command, "@EnquiryResponse_ID", DbType.Int32, ObjPricingRow.Enquiry_Response_Id);
                        }

                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPricingRow.LOB_ID);
                        //db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjPricingRow.Branch_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjPricingRow.Branch_ID);
                        db.AddInParameter(command, "@FacilityAmount", DbType.Double, ObjPricingRow.Facility_Amount);
                        db.AddInParameter(command, "@Offer_validtill", DbType.DateTime, ObjPricingRow.Offer_Valid_Till);
                        db.AddInParameter(command, "@Tenure", DbType.Int32, ObjPricingRow.Tenure);
                        db.AddInParameter(command, "@Tenure_Type", DbType.String, ObjPricingRow.Tenure_Type);
                        if (!ObjPricingRow.IsEnquiryDateNull())
                        {
                            db.AddInParameter(command, "@Enqdate", DbType.String, ObjPricingRow.EnquiryDate);
                        }
                        db.AddInParameter(command, "@Company_IRR ", DbType.Double, ObjPricingRow.Company_IRR);
                        db.AddInParameter(command, "@Business_IRR", DbType.Double, ObjPricingRow.Business_IRR);
                        db.AddInParameter(command, "@Accounting_IRR", DbType.Double, ObjPricingRow.Accounting_IRR);

                        if (!ObjPricingRow.IsOffer_Residual_ValueNull())
                        {
                            db.AddInParameter(command, "@Residual_Value", DbType.Double, ObjPricingRow.Offer_Residual_Value);
                        }
                        if (!ObjPricingRow.IsOffer_Residual_Value_AmountNull())
                        {
                            db.AddInParameter(command, "@Residual_Amt", DbType.Double, ObjPricingRow.Offer_Residual_Value_Amount);
                        }
                        if (!ObjPricingRow.IsOffer_MarginNull())
                        {
                            db.AddInParameter(command, "@Margin_Value", DbType.Double, ObjPricingRow.Offer_Margin);
                        }
                        if (!ObjPricingRow.IsOffer_Margin_AmountNull())
                        {
                            db.AddInParameter(command, "@Margin_Amount", DbType.Double, ObjPricingRow.Offer_Margin_Amount);
                        }

                        if (!ObjPricingRow.IsGuaranteed_EOTNull())
                            db.AddInParameter(command, "@Guaranteed_EOT", DbType.String, ObjPricingRow.Guaranteed_EOT);
                        if (!ObjPricingRow.IsGuaranteed_EOT_AppNull())
                            db.AddInParameter(command, "@Guaranteed_EOT_App", DbType.String, ObjPricingRow.Guaranteed_EOT_App);

                        if (!ObjPricingRow.IsSecurity_Depost_onNull())
                            db.AddInParameter(command, "@Security_Depost_on", DbType.Int32, ObjPricingRow.Security_Depost_on);

                        db.AddInParameter(command, "@XMLCashInflow", DbType.String, ObjPricingRow.XMLCashInflow);
                        db.AddInParameter(command, "@XMLCashOutflow", DbType.String, ObjPricingRow.XMLCashOutflow);
                        db.AddInParameter(command, "@XMLRepayment", DbType.String, ObjPricingRow.XMLRepayment);
                        db.AddInParameter(command, "@XMLAlerts", DbType.String, ObjPricingRow.XMLAlerts);
                        db.AddInParameter(command, "@XMLFollowUp", DbType.String, ObjPricingRow.XMLFollowUp);
                        if (!ObjPricingRow.IsXMLConsDocDetailsNull())
                        {
                            db.AddInParameter(command, "@XMLConsDocDetails", DbType.String, ObjPricingRow.XMLConsDocDetails);
                        }
                        db.AddInParameter(command, "@XMLROIRule", DbType.String, ObjPricingRow.XMLROIRule);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjPricingRow.Created_By);
                        if (!ObjPricingRow.IsXMLPDDNull())
                        {
                            db.AddInParameter(command, "@XML_PDD", DbType.String, ObjPricingRow.XMLPDD);
                        }
                        if (!ObjPricingRow.IsXMLStructureNull())
                        {
                            S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XML_Structure",
                                    OracleType.Clob, ObjPricingRow.XMLStructure.Length,
                                    ParameterDirection.Input, true, 0, 0, String.Empty,
                                    DataRowVersion.Default, ObjPricingRow.XMLStructure);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XML_Structure",
                                    DbType.String, ObjPricingRow.XMLStructure);
                            }
                        }

                        //Added by saran on 2-Aug-2013 start
                        if (!ObjPricingRow.IsLoan_TypeNull())
                        {
                            db.AddInParameter(command, "@Loan_Type", DbType.Int32, ObjPricingRow.Loan_Type);
                        }
                        //Added by saran on 2-Aug-2013 end

                        //Added by saran on 4-Jul-2014 for CR_SISSL12E046_018 start
                        if (!ObjPricingRow.IsXMLRepayDetailsOthersNull())
                        {
                            S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XMLRepayDetailsOthers",
                                 OracleType.Clob, ObjPricingRow.XMLRepayDetailsOthers.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                 DataRowVersion.Default, ObjPricingRow.XMLRepayDetailsOthers);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XMLRepayDetailsOthers",
                                    DbType.String, ObjPricingRow.XMLRepayDetailsOthers);
                            }

                        }
                        //Added by saran on 4-Jul-2014 for CR_SISSL12E046_018 end

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);

                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
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
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                }
                return intRowsAffected;
            }

            public DataSet FunPubGetPricingDetails(int intPricingId, int intCompanyId)
            {
                DataSet dsPricingDetails = new DataSet();
                try
                {

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetPricingDetails");
                    db.AddInParameter(command, "@Company_Id", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@Pricing_Id", DbType.Int32, intPricingId);
                    dsPricingDetails = db.FunPubExecuteDataSet(command);

                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                }
                return dsPricingDetails;
            }

            public int FunPubWithDrawPricingInt(int intPricingId, int intCompanyId, int intCreatedBy)
            {

                try
                {

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");


                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_WithdrawPricing");
                    db.AddInParameter(command, "@Company_Id", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@Pricing_Id", DbType.Int32, intPricingId);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, intCreatedBy);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command, ref trans);

                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            {
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
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
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                }
                return intRowsAffected;
            }
        }


    }
}
