#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Customer Master
/// Created By			: Narayanan
/// Created Date		: 28-05-2010
/// <Program Summary>
#endregion

using System;using S3GDALayer.S3GAdminServices;
using System.IO;
using System.Data;
using System.Text;
using S3GBusEntity;
using System.Data.Common;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.OracleClient;

namespace S3GDALayer.Origination
{
    namespace OrgMasterMgtServices
    {
        public class ClsPubCustomerMaster
        {
            int intRowsAffected;
            string strConnectionString = "S3GConnectionString";
            string strStatusLookUp = SPNames.S3G_ORG_GetCustomerLookUp;
            string strCustomerInsertUpDate = SPNames.S3G_ORG_CustomerInsertUpDate;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubCustomerMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            /// <summary>
            /// This method to get common query result
            /// </summary>
            /// <param name="ObjParam">Pass Parameter Objects</param>
            /// <returns>Should object is Datatable</returns>
            public DataTable FunPub_GetS3GStatusLookUp(S3G_Status_Parameters ObjParam)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase(strConnectionString);
                    DbCommand command = db.GetStoredProcCommand(strStatusLookUp);

                    db.AddInParameter(command, "@Option", DbType.Int32, ObjParam.Option);
                    db.AddInParameter(command, "@Param1", DbType.String, ObjParam.Param1);
                    db.AddInParameter(command, "@Param2", DbType.String, ObjParam.Param2);
                    db.AddInParameter(command, "@Param3", DbType.String, ObjParam.Param3);
                    db.AddInParameter(command, "@Param4", DbType.String, ObjParam.Param4);
                    db.AddInParameter(command, "@Param5", DbType.String, ObjParam.Param5);

                    db.FunPubLoadDataSet(command, ObjDS, strStatusLookUp);
                    return (DataTable)ObjDS.Tables[strStatusLookUp];


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// To Insert Record into Customer Master
            /// </summary>
            /// <param name="SMode">Pass Serialization Mode</param>
            /// <param name="byteEnquiryService">Pass byte Object</param>
            public int FunPubCreateCustomerInt(CustomerMasterBusEntity ObjCustomerMaster, out string strCustomerCodeOut)
            {
                string strCustomerCode = "";
                try
                {
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    //Database db = DatabaseFactory.CreateDatabase(strConnectionString);
                    DbCommand command = db.GetStoredProcCommand(strCustomerInsertUpDate);

                    db.AddOutParameter(command, "@ID", DbType.Int64, 10);
                    db.AddOutParameter(command, "@CustomerCode", DbType.String, 200);
                    db.AddInParameter(command, "@CustomerType_ID", DbType.String, ObjCustomerMaster.CustomerType_ID);
                    db.AddInParameter(command, "@GroupCode", DbType.String, ObjCustomerMaster.GroupCode);
                    db.AddInParameter(command, "@Groupname", DbType.String, ObjCustomerMaster.Groupname);
                    db.AddInParameter(command, "@IndustryCode", DbType.String, ObjCustomerMaster.IndustryCode);
                    db.AddInParameter(command, "@IndustryName", DbType.String, ObjCustomerMaster.IndustryName);
                    db.AddInParameter(command, "@Constitution_ID", DbType.String, ObjCustomerMaster.Constitution_ID);
                    db.AddInParameter(command, "@BillingAddress", DbType.String, ObjCustomerMaster.BillingAddress);
                    db.AddInParameter(command, "@Title", DbType.String, ObjCustomerMaster.Title);
                    db.AddInParameter(command, "@CustomerName", DbType.String, ObjCustomerMaster.CustomerName);
                    db.AddInParameter(command, "@CustomerPostingGroupCode_ID", DbType.String, ObjCustomerMaster.CustomerPostingGroupCode_ID);
                    db.AddInParameter(command, "@Comm_Address1", DbType.String, ObjCustomerMaster.Comm_Address1);
                    db.AddInParameter(command, "@Comm_Address2", DbType.String, ObjCustomerMaster.Comm_Address2);
                    db.AddInParameter(command, "@Comm_City", DbType.String, ObjCustomerMaster.Comm_City);
                    db.AddInParameter(command, "@Comm_State", DbType.String, ObjCustomerMaster.Comm_State);
                    db.AddInParameter(command, "@Comm_Country", DbType.String, ObjCustomerMaster.Comm_Country);
                    db.AddInParameter(command, "@Comm_PINCode", DbType.String, ObjCustomerMaster.Comm_PINCode);
                    db.AddInParameter(command, "@Comm_Mobile", DbType.String, ObjCustomerMaster.Comm_Mobile);
                    db.AddInParameter(command, "@Comm_Telephone", DbType.String, ObjCustomerMaster.Comm_Telephone);
                    db.AddInParameter(command, "@Comm_Email", DbType.String, ObjCustomerMaster.Comm_Email);
                    db.AddInParameter(command, "@Comm_Website", DbType.String, ObjCustomerMaster.Comm_Website);
                    db.AddInParameter(command, "@TAN", DbType.String, ObjCustomerMaster.TAN);
                    db.AddInParameter(command, "@TIN", DbType.String, ObjCustomerMaster.TIN);
                    db.AddInParameter(command, "@Rental_ScheduleNo", DbType.String, ObjCustomerMaster.Rental_ScheduleNo);
                    db.AddInParameter(command, "@Tranche_No", DbType.String, ObjCustomerMaster.Tranche_No);
                    //db.AddInParameter(command, "@Perm_Country", DbType.String, ObjCustomerMaster.Perm_Country);
                    //db.AddInParameter(command, "@Perm_PINCode", DbType.String, ObjCustomerMaster.Perm_PINCode);
                    //db.AddInParameter(command, "@Perm_Mobile", DbType.String, ObjCustomerMaster.Perm_Mobile);
                    //db.AddInParameter(command, "@Perm_Telephone", DbType.String, ObjCustomerMaster.Perm_Telephone);
                    //db.AddInParameter(command, "@Perm_Email", DbType.String, ObjCustomerMaster.Perm_Email);
                    //db.AddInParameter(command, "@Perm_Website", DbType.String, ObjCustomerMaster.Perm_Website);
                    db.AddInParameter(command, "@Customer_ID", DbType.Int64, ObjCustomerMaster.ID);
                    if (ObjCustomerMaster.Gender != "-1")
                    {
                        db.AddInParameter(command, "@Gender", DbType.String, ObjCustomerMaster.Gender);
                    }

                    if (!(Convert.ToString(ObjCustomerMaster.DateofBirth).Contains("1/1/0001")))
                    {
                        db.AddInParameter(command, "@DateofBirth", DbType.DateTime, ObjCustomerMaster.DateofBirth);
                    }
                    db.AddInParameter(command, "@MaritalStatus_ID", DbType.String, ObjCustomerMaster.MaritalStatus_ID);
                    db.AddInParameter(command, "@Qualification", DbType.String, ObjCustomerMaster.Qualification);
                    db.AddInParameter(command, "@Profession", DbType.String, ObjCustomerMaster.Profession);
                    db.AddInParameter(command, "@SpouseName", DbType.String, ObjCustomerMaster.SpouseName);
                    db.AddInParameter(command, "@Children", DbType.Decimal, ObjCustomerMaster.Children);
                    db.AddInParameter(command, "@TotalDependents", DbType.Decimal, ObjCustomerMaster.TotalDependents);
                    if (!(Convert.ToString(ObjCustomerMaster.WeddingAnniversaryDate).Contains("1/1/0001")))
                    {
                        db.AddInParameter(command, "@WeddingAnniversaryDate", DbType.DateTime, ObjCustomerMaster.WeddingAnniversaryDate);
                    }
                    if (ObjCustomerMaster.HouseORFlat_ID > 0)
                    {
                        db.AddInParameter(command, "@HouseORFlat_ID", DbType.String, ObjCustomerMaster.HouseORFlat_ID);
                    }

                    db.AddInParameter(command, "@ISOwn", DbType.Int16, ObjCustomerMaster.ISOwn);
                    db.AddInParameter(command, "@CurrentMarketValue", DbType.String, ObjCustomerMaster.CurrentMarketValue);
                    db.AddInParameter(command, "@RemainingLoanValue", DbType.String, ObjCustomerMaster.RemainingLoanValue);
                    db.AddInParameter(command, "@TotalNetMorth", DbType.String, ObjCustomerMaster.TotalNetMorth);
                    db.AddInParameter(command, "@PublicCloselyheld_ID", DbType.String, ObjCustomerMaster.PublicCloselyheld_ID);
                    db.AddInParameter(command, "@NoOfDirectors", DbType.Decimal, ObjCustomerMaster.NoOfDirectors);
                    db.AddInParameter(command, "@ListedAtStockExchange", DbType.String, ObjCustomerMaster.ListedAtStockExchange);
                    db.AddInParameter(command, "@PaidupCapital", DbType.Decimal, ObjCustomerMaster.PaidupCapital);
                    db.AddInParameter(command, "@FaceValueofShares", DbType.Decimal, ObjCustomerMaster.FaceValueofShares);
                    db.AddInParameter(command, "@BookValueofShares", DbType.Decimal, ObjCustomerMaster.BookValueofShares);
                    db.AddInParameter(command, "@BusinessProfile", DbType.String, ObjCustomerMaster.BusinessProfile);
                    db.AddInParameter(command, "@Geographicalcoverage", DbType.String, ObjCustomerMaster.Geographicalcoverage);
                    db.AddInParameter(command, "@NoOfBranches", DbType.Decimal, ObjCustomerMaster.NoOfBranches);
                    db.AddInParameter(command, "@GovInstParticipation_ID", DbType.String, ObjCustomerMaster.GovernmentInstitutionalParticipation_ID);
                    db.AddInParameter(command, "@PercentageOfStake", DbType.Decimal, ObjCustomerMaster.PercentageOfStake);
                    db.AddInParameter(command, "@JVPartnerName", DbType.String, ObjCustomerMaster.JVPartnerName);
                    db.AddInParameter(command, "@JVPartnerStake", DbType.Decimal, ObjCustomerMaster.JVPartnerStake);
                    db.AddInParameter(command, "@CEOName", DbType.String, ObjCustomerMaster.CEOName);
                    db.AddInParameter(command, "@CEOAge", DbType.Decimal, ObjCustomerMaster.CEOAge);
                    db.AddInParameter(command, "@CEOExperienceInYears", DbType.Decimal, ObjCustomerMaster.CEOExperienceInYears);
                    if (!(Convert.ToString(ObjCustomerMaster.CEOWeddingDate).Contains("1/1/0001")))
                    {
                        db.AddInParameter(command, "@CEOWeddingDate", DbType.DateTime, ObjCustomerMaster.CEOWeddingDate);
                    }
                    db.AddInParameter(command, "@ResidentialAddress", DbType.String, ObjCustomerMaster.ResidentialAddress);
                    //--------------------------------------------------------------------
                    //Modified By  :  
                    //Reason       :  

                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        if (!string.IsNullOrEmpty(ObjCustomerMaster.XMLBillingDetails))
                        {
                            OracleParameter param;
                            param = new OracleParameter("@XMLBillingDetails",
                                 OracleType.Clob, ObjCustomerMaster.XMLBillingDetails.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, ObjCustomerMaster.XMLBillingDetails);
                            command.Parameters.Add(param);
                        }
                    }
                    else
                    {
                        db.AddInParameter(command, "@XMLBillingDetails", DbType.String, ObjCustomerMaster.XMLBillingDetails);
                    }
                    //---------------------------------------------------------------
                    //Modified By  :  Thanagm M
                    //Reason       :  To add multiple Bank Details   

                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        if (!string.IsNullOrEmpty(ObjCustomerMaster.XmlBankDetails))
                        {
                            OracleParameter param;
                            param = new OracleParameter("@XmlBankDetails",
                                 OracleType.Clob, ObjCustomerMaster.XmlBankDetails.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, ObjCustomerMaster.XmlBankDetails);
                            command.Parameters.Add(param);
                        }
                    }
                    else
                    {
                        db.AddInParameter(command, "@XmlBankDetails", DbType.String, ObjCustomerMaster.XmlBankDetails);
                    }
                    //opc042 start
                    db.AddInParameter(command, "@BillEmailType", DbType.Int32, ObjCustomerMaster.BillEmailType);
                    if (!string.IsNullOrEmpty(ObjCustomerMaster.XmlCustEmailDet))
                    {
                        db.AddInParameter(command, "@XmlCustEmailDet", DbType.String, ObjCustomerMaster.XmlCustEmailDet);
                    }
                    //opc042 end
                    db.AddInParameter(command, "@LOB_ID", DbType.String, ObjCustomerMaster.LOB_ID);
                    db.AddInParameter(command, "@Type_ID", DbType.String, ObjCustomerMaster.Type_ID);
                    if (!(Convert.ToString(ObjCustomerMaster.Date).Contains("1/1/0001")))
                    {
                        db.AddInParameter(command, "@Date", DbType.DateTime, ObjCustomerMaster.Date);
                    }
                    db.AddInParameter(command, "@Reason", DbType.String, ObjCustomerMaster.Reason);
                    if (!(Convert.ToString(ObjCustomerMaster.ReleaseDate).Contains("1/1/0001")))
                    {
                        db.AddInParameter(command, "@ReleaseDate", DbType.DateTime, ObjCustomerMaster.ReleaseDate);
                    }
                    if (!(Convert.ToString(ObjCustomerMaster.ValidUpto).Contains("1/1/0001")))
                    {
                        db.AddInParameter(command, "@ValidUpto", DbType.DateTime, ObjCustomerMaster.ValidUpto);
                    }
                    db.AddInParameter(command, "@Created_By", DbType.String, ObjCustomerMaster.Created_By);
                    db.AddInParameter(command, "@Modified_By", DbType.String, ObjCustomerMaster.Modified_By);
                    db.AddInParameter(command, "@Company_ID", DbType.String, ObjCustomerMaster.Company_ID);
                    db.AddInParameter(command, "@Company_Type_ID", DbType.String, ObjCustomerMaster.Company_Type_ID);
                    //Modified By  :  Saranya I
                    //Reason       :  To add Relation Type   
                    db.AddInParameter(command, "@Customer", DbType.Boolean, ObjCustomerMaster.Customer);
                    db.AddInParameter(command, "@Guarantor1", DbType.Boolean, ObjCustomerMaster.Guarantor1);
                    db.AddInParameter(command, "@Guarantor2", DbType.Boolean, ObjCustomerMaster.Guarantor2);
                    db.AddInParameter(command, "@CoApplicant", DbType.Boolean, ObjCustomerMaster.CoApplicant);
                    //end

                    //BCA Changes - Kuppu - Aug-17-2012 -- Starts
                   // db.AddInParameter(command, "@Family_Name", DbType.String, ObjCustomerMaster.Family_Name);
                    db.AddInParameter(command, "@Short_Name", DbType.String, ObjCustomerMaster.Short_Name);
                    db.AddInParameter(command, "@Notes", DbType.String, ObjCustomerMaster.Notes);
                    db.AddInParameter(command, "@PAN", DbType.String, ObjCustomerMaster.PAN);
                    db.AddInParameter(command, "@CIN", DbType.String, ObjCustomerMaster.CIN);
                    //db.AddInParameter(command, "@TIN", DbType.String, ObjCustomerMaster.TIN);
                    //db.AddInParameter(command, "@TAN", DbType.String, ObjCustomerMaster.TAN);
                    //Ends

                    //BDO Changes - Thangam M - 03-Oct-2012 -- Starts
                    db.AddInParameter(command, "@CreditType", DbType.Int32, ObjCustomerMaster.CreditType);
                    //End here
                    //BDO Changes - Thangam M - 03-Oct-2012 -- Starts
                    db.AddInParameter(command, "@BlockListed", DbType.Boolean, ObjCustomerMaster.IS_BlockListed);
                    //End here
                    //
                    db.AddInParameter(command, "@Hot_Listed", DbType.Boolean, ObjCustomerMaster.IS_HotListed);
                    db.AddInParameter(command, "@Hot_List_Reason", DbType.String, ObjCustomerMaster.HotList_Reason);
                    db.AddInParameter(command, "@Customer_Rating", DbType.String, ObjCustomerMaster.Customer_Rating);
                    db.AddInParameter(command, "@Is_POBlack", DbType.Boolean, ObjCustomerMaster.IS_POBlack);
                    //
                    ////BW changes - Saran - 24-Jul-2013 Start
                    if (ObjCustomerMaster.Stock_Stmt_Frequency != null)
                    {
                        if (ObjCustomerMaster.Stock_Stmt_Frequency > 0)
                            db.AddInParameter(command, "@Stock_Stmt_Frequency", DbType.Int32, ObjCustomerMaster.Stock_Stmt_Frequency);
                    }
                    if (ObjCustomerMaster.Is_BW != null)
                    {
                        if (ObjCustomerMaster.Is_BW > 0)
                            db.AddInParameter(command, "@Is_BW", DbType.Int32, ObjCustomerMaster.Is_BW);
                    }
                    //BW changes - Saran - 24-Jul-2013 End 


                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        if (!string.IsNullOrEmpty(ObjCustomerMaster.XmlConstitutionalDocuments))
                        {
                            OracleParameter param;
                            param = new OracleParameter("@XmlConstitutionalDocuments",
                                 OracleType.Clob, ObjCustomerMaster.XmlConstitutionalDocuments.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, ObjCustomerMaster.XmlConstitutionalDocuments);
                            command.Parameters.Add(param);
                        }
                    }
                    else
                    {
                        db.AddInParameter(command, "@XmlConstitutionalDocuments", DbType.String, ObjCustomerMaster.XmlConstitutionalDocuments);
                    }

                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        if (!string.IsNullOrEmpty(ObjCustomerMaster.XmlTrackDetails))
                        {
                            OracleParameter param;
                            param = new OracleParameter("@XmlTrackDetails",
                                 OracleType.Clob, ObjCustomerMaster.XmlTrackDetails.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, ObjCustomerMaster.XmlTrackDetails);
                            command.Parameters.Add(param);
                        }
                    }
                    else
                    {
                        db.AddInParameter(command, "@XmlTrackDetails", DbType.String, ObjCustomerMaster.XmlTrackDetails);
                    }

                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        if (!string.IsNullOrEmpty(ObjCustomerMaster.XmlCreditDetails))
                        {
                            OracleParameter param;
                            param = new OracleParameter("@XmlCreditDetails",
                                 OracleType.Clob, ObjCustomerMaster.XmlCreditDetails.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, ObjCustomerMaster.XmlCreditDetails);
                            command.Parameters.Add(param);
                        }
                    }
                    else
                    {
                        db.AddInParameter(command, "@XmlCreditDetails", DbType.String, ObjCustomerMaster.XmlCreditDetails);
                    }


                    //Code Added By Ganapathy on 13-Nov-2013 BEGINS

                    if (ObjCustomerMaster.Enquiry_ID != null)
                    {
                        if (ObjCustomerMaster.Enquiry_ID > 0)
                        {
                            db.AddInParameter(command, "@Enquiry_ID", DbType.Int32, ObjCustomerMaster.Enquiry_ID);
                        }
                    }

                    //Code Added By Ganapathy on 13-Nov-2013 ENDS

                    //Added on 26Sep2014 starts here
                    db.AddInParameter(command, "@CRM_ID", DbType.Int64, ObjCustomerMaster.CRM_ID);
                    //End here

                    db.AddInParameter(command, "@XmlPODOMappings", DbType.String, ObjCustomerMaster.XmlPODOMappings);
                    db.AddInParameter(command, "@XmlPODOMappingsDetails", DbType.String, ObjCustomerMaster.XmlPODOMappingsDetails);

                    db.AddInParameter(command, "@Mode", DbType.String, ObjCustomerMaster.Mode);

                    db.AddInParameter(command, "@CGSTIN", DbType.String, ObjCustomerMaster.CGSTIN);
                    if (!(Convert.ToString(ObjCustomerMaster.CGST_Reg_Date).Contains("1/1/0001")))
                    {
                        db.AddInParameter(command, "@CGST_Reg_Date", DbType.String, ObjCustomerMaster.CGST_Reg_Date);
                    }

                    db.AddInParameter(command, "@Invoice_Cov_Letter", DbType.Int32, ObjCustomerMaster.Invoice_Cov_Letter);

                    db.AddInParameter(command, "@Is_Manual_Num", DbType.Int32, ObjCustomerMaster.Is_Manual_Num);

                    //Added by Gomathi on 29/May/2020 for Statewise Billing
                    db.AddInParameter(command, "@Is_Statewisebilling", DbType.Boolean, ObjCustomerMaster.IS_StateWiseBilling);

                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, 1000);
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
                                if (ObjCustomerMaster.Mode.ToUpper() == "INSERT")
                                {
                                    strCustomerCode = Convert.ToString(command.Parameters["@ID"].Value) + "~" + (string)command.Parameters["@CustomerCode"].Value;
                                }
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
                strCustomerCodeOut = strCustomerCode;
                return intRowsAffected;
            }


        }
    }
}
