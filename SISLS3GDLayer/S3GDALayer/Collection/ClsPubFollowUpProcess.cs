using System;
using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Data.OracleClient;
using S3GBusEntity;

namespace S3GDALayer.Collection
{
    namespace ClnDataMgtServices
    {
        public class ClsPubFollowUpProcess
        {
            int intRowsAffected;
            S3GBusEntity.Collection.ClnDataMgtServices.S3G_CLN_FollowUpDataTable objFollowUp_DAl;
            S3GBusEntity.Collection.ClnDataMgtServices.S3G_CLN_CRM_HdrDataTable objCRM_DAl;
            S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_PROSPECTDTLDataTable objCust_DAL;
            S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_ProspectDataTable objPspct;
            S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_LEADDTLDataTable objLead;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubFollowUpProcess()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            /// <summary>
            /// Inserting the Appropriation logic Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns>
            /// 
            public int FunPubCreateFollowUp(SerializationMode SerMode, byte[] bytesObjS3G_Colection_FollowUp_DataTable, out int intFollowUpID, out string strDocNo)
            {
                intFollowUpID = 0;
                strDocNo = "";
                try
                {
                    objFollowUp_DAl = (S3GBusEntity.Collection.ClnDataMgtServices.S3G_CLN_FollowUpDataTable
                        )ClsPubSerialize.
                        DeSerialize(bytesObjS3G_Colection_FollowUp_DataTable,
                        SerMode, typeof(S3GBusEntity.Collection.
                        ClnDataMgtServices.S3G_CLN_FollowUpDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnDataMgtServices.S3G_CLN_FollowUpRow objFollowUpRow in objFollowUp_DAl)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertFollowUp");
                        db.AddInParameter(command, "@FollowUp_ID", DbType.Int32, objFollowUpRow.FollowUp_ID);
                        db.AddInParameter(command, "@FollowUp_No", DbType.String, objFollowUpRow.FollowUp_No);
                        db.AddInParameter(command, "@Date", DbType.DateTime, objFollowUpRow.Date);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, objFollowUpRow.Customer_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objFollowUpRow.Company_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objFollowUpRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, objFollowUpRow.Created_On);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, objFollowUpRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, objFollowUpRow.Modified_On);
                        db.AddInParameter(command, "@PANum", DbType.String, objFollowUpRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, objFollowUpRow.SANum);
                        //db.AddInParameter(command, "@Xml", DbType.String, objFollowUpRow.Xml);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@Xml", OracleType.Clob,
                                objFollowUpRow.Xml.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objFollowUpRow.Xml);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@Xml", DbType.String,
                                objFollowUpRow.Xml);
                        }
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@intFollowUpID", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@DocNo", DbType.String, 200);
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value == 0)
                        {
                            intFollowUpID = Convert.ToInt32(command.Parameters["@intFollowUpID"].Value);
                            strDocNo = Convert.ToString(command.Parameters["@DocNo"].Value);
                        }
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
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

            public int FunPubCreateCRM(SerializationMode SerMode, byte[] bytesObjS3G_Colection_CRM_DataTable, out int intFollowUpID, out string strDocNo, out int intCustomerID)
            {
                intFollowUpID = intCustomerID = 0;
                strDocNo = "";
                try
                {
                    objCRM_DAl = (S3GBusEntity.Collection.ClnDataMgtServices.S3G_CLN_CRM_HdrDataTable
                        )ClsPubSerialize.
                        DeSerialize(bytesObjS3G_Colection_CRM_DataTable,
                        SerMode, typeof(S3GBusEntity.Collection.
                        ClnDataMgtServices.S3G_CLN_CRM_HdrDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnDataMgtServices.S3G_CLN_CRM_HdrRow objCRMRow in objCRM_DAl)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_INSERTCRM_OPC");
                        db.AddInParameter(command, "@CRM_ID", DbType.Int32, objCRMRow.CRM_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objCRMRow.Company_ID);
                        db.AddInParameter(command, "@Group_ID", DbType.Int32, objCRMRow.Group_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, objCRMRow.Customer_ID);
                        db.AddInParameter(command, "@Location_Code", DbType.String, objCRMRow.Location_Code);
                        db.AddInParameter(command, "@Prospect_Title", DbType.Int32, objCRMRow.Prospect_Title);
                        db.AddInParameter(command, "@Prospect_Name", DbType.String, objCRMRow.Prospect_Name);
                        db.AddInParameter(command, "@Address1", DbType.String, objCRMRow.Address1);
                        db.AddInParameter(command, "@Address2", DbType.String, objCRMRow.Address2);
                        db.AddInParameter(command, "@City", DbType.String, objCRMRow.City);
                        db.AddInParameter(command, "@State", DbType.String, objCRMRow.State);
                        db.AddInParameter(command, "@Country", DbType.String, objCRMRow.Country);
                        db.AddInParameter(command, "@Pincode", DbType.String, objCRMRow.Pincode);
                        db.AddInParameter(command, "@Mobile", DbType.String, objCRMRow.Mobile);
                        db.AddInParameter(command, "@Telephone", DbType.String, objCRMRow.Telephone);
                        db.AddInParameter(command, "@EMail", DbType.String, objCRMRow.EMail);
                        db.AddInParameter(command, "@Website", DbType.String, objCRMRow.Website);
                        db.AddInParameter(command, "@SE_Ref", DbType.String, objCRMRow.SE_Ref);
                        db.AddInParameter(command, "@Account_Stat_Type", DbType.Int32, objCRMRow.Account_Stat_Type);
                        db.AddInParameter(command, "@Account_Stat", DbType.Int32, objCRMRow.Account_Stat);
                        db.AddInParameter(command, "@Customer_Stat_Type", DbType.Int32, objCRMRow.Customer_Stat_Type);
                        db.AddInParameter(command, "@Customer_Stat", DbType.Int32, objCRMRow.Customer_Stat);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objCRMRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, objCRMRow.Created_On);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, objCRMRow.Constitution_ID);
                        db.AddInParameter(command, "@Is_MoveEnquiry", DbType.Int32, objCRMRow.Is_MoveEnquiry);
                        db.AddInParameter(command, "@Other_Lead_Source", DbType.String, objCRMRow.Other_Lead_Source);

                        // Added By R. Manikandan  three fields in CRM based on FTW_SISSL12E046_CRM TC_008
                        db.AddInParameter(command, "@Cnt_Per", DbType.String, objCRMRow.Cnt_Per);
                        db.AddInParameter(command, "@Cnt_Per_Ph", DbType.String, objCRMRow.Cnt_Per_Ph);
                        db.AddInParameter(command, "@Cnt_Per_Desig", DbType.String, objCRMRow.Cnt_Per_Desig);

                        db.AddInParameter(command, "@Cutsomer_Type", DbType.String, objCRMRow.Customer_Type);
                        db.AddInParameter(command, "@Posting_GL_Code", DbType.String, objCRMRow.Posting_GL_Code);
                        db.AddInParameter(command, "@Industry_Type", DbType.String, objCRMRow.Industry_Type);
                        db.AddInParameter(command, "@Company_Type", DbType.String, objCRMRow.Company_Type);
                        db.AddInParameter(command, "@Competitor_Info", DbType.String, objCRMRow.Competitor_info);
                        db.AddInParameter(command, "@Funding_Type", DbType.String, objCRMRow.Funding_Type);
                        db.AddInParameter(command, "@Sales_Person", DbType.String, objCRMRow.Sales_Person);
                        db.AddInParameter(command, "@IS_NewLead", DbType.String, objCRMRow.IS_NewLead);

                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_Track", OracleType.Clob,
                                objCRMRow.XML_Track.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objCRMRow.XML_Track);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_Track", DbType.String,
                                objCRMRow.XML_Track);
                        }

                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_PRD_Docs", OracleType.Clob,
                                objCRMRow.XML_PRD_Docs.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objCRMRow.XML_PRD_Docs);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_PRD_Docs", DbType.String,
                                objCRMRow.XML_PRD_Docs);
                        }

                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_POD_Docs", OracleType.Clob,
                                objCRMRow.XML_POD_Docs.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objCRMRow.XML_POD_Docs);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_POD_Docs", DbType.String,
                                objCRMRow.XML_POD_Docs);
                        }

                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_FIR_Docs", OracleType.Clob,
                                objCRMRow.XML_FIR_Docs.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objCRMRow.XML_FIR_Docs);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_FIR_Docs", DbType.String,
                                objCRMRow.XML_FIR_Docs);
                        }

                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_Cons_Docs", OracleType.Clob,
                                objCRMRow.XML_Cons_Docs.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objCRMRow.XML_Cons_Docs);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_Cons_Docs", DbType.String,
                                objCRMRow.XML_Cons_Docs);
                        }

                        db.AddInParameter(command, "@Finance_Mode", DbType.Int32, objCRMRow.Finance_Mode);
                        db.AddInParameter(command, "@LOB", DbType.Int32, objCRMRow.LOB);
                        db.AddInParameter(command, "@LeadSourceType", DbType.Int32, objCRMRow.LeadSourceType);
                        db.AddInParameter(command, "@LeadSource", DbType.Int32, objCRMRow.LeadSource);
                        db.AddInParameter(command, "@Lead_Status", DbType.Int32, objCRMRow.Lead_Status);
                        db.AddInParameter(command, "@Lead_Information", DbType.String, objCRMRow.Lead_Information);
                        db.AddInParameter(command, "@Deleted_Doc", DbType.String, objCRMRow.Deleted_Doc);
                        db.AddInParameter(command, "@Lead_Update_ID", DbType.Int32, objCRMRow.Lead_Update_ID);
                        db.AddInParameter(command, "@Lead_Remarks", DbType.String, objCRMRow.Lead_Remarks);
                        db.AddInParameter(command, "@Lead_Cnt_Name", DbType.String, objCRMRow.Lead_Cnt_Name);
                        db.AddInParameter(command, "@Lead_Cnt_No", DbType.String, objCRMRow.Lead_Cnt_No);
                        db.AddInParameter(command, "@Lead_Cnt_Designation", DbType.String, objCRMRow.Lead_Cnt_Designation);
                        db.AddInParameter(command, "@Lead_Cnt_Email", DbType.String, objCRMRow.Lead_Cnt_Email);
                        db.AddInParameter(command, "@AcctManager1", DbType.Int32, objCRMRow.AcctManager1);
                        db.AddInParameter(command, "@AcctManager2", DbType.Int32, objCRMRow.AcctManager2);

                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_Lead_Asset", OracleType.Clob,
                                objCRMRow.XML_Lead_Asset.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objCRMRow.XML_Lead_Asset);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_Lead_Asset", DbType.String,
                                objCRMRow.XML_Lead_Asset);
                        }

                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_Status_Details", OracleType.Clob,
                                objCRMRow.XML_Lead_Asset.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objCRMRow.XML_Status_Details);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_Status_Details", DbType.String,
                                objCRMRow.XML_Status_Details);
                        }

                        if (!objCRMRow.IsXML_CrossBorderNull())
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XML_CrossBorder", OracleType.Clob,
                                    objCRMRow.XML_CrossBorder.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, objCRMRow.XML_CrossBorder);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XML_CrossBorder", DbType.String,
                                    objCRMRow.XML_CrossBorder);
                            }
                        }

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@Customer_ID_OUT", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@DocNo", DbType.String, 200);

                        using (DbConnection con = db.CreateConnection())
                        {
                            con.Open();
                            DbTransaction trans = con.BeginTransaction();
                            try
                            {

                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);

                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                if (intRowsAffected == 0)
                                {
                                    strDocNo = Convert.ToString(command.Parameters["@DocNo"].Value);
                                    intCustomerID = Convert.ToInt32(command.Parameters["@Customer_ID_OUT"].Value);
                                    trans.Commit();
                                }
                                else
                                {
                                    trans.Rollback();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
                                trans.Rollback();
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            }
                            con.Close();
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


            public int FunPubCreateProspectCust(SerializationMode SerMode, byte[] bytesObjS3G_Colection_Customer_DataTable, out string strDocNo, out int intCustomerID)
            {
                intCustomerID = 0;
                strDocNo = "";
                try
                {
                    objCust_DAL = (S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_PROSPECTDTLDataTable
                        )ClsPubSerialize.
                        DeSerialize(bytesObjS3G_Colection_Customer_DataTable,
                        SerMode, typeof(S3GBusEntity.Collection.
                        ClnDataMgtServices.S3G_ORG_PROSPECTDTLRow));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_PROSPECTDTLRow objCustRow in objCust_DAL)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_INSERTPROSPECTCUST");
                        db.AddInParameter(command, "@CRM_ID", DbType.Int32, objCustRow.CRM_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objCustRow.Company_ID);
                        db.AddInParameter(command, "@Customer_Type_ID", DbType.Int32, objCustRow.Customer_Type_ID);
                        db.AddInParameter(command, "@Company_Type_ID", DbType.Int32, objCustRow.Company_Type_ID);
                        db.AddInParameter(command, "@Industry_ID", DbType.Int32, objCustRow.Industry_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, objCustRow.Constitution_ID);
                        db.AddInParameter(command, "@Title_ID", DbType.Int32, objCustRow.Title_ID);
                        db.AddInParameter(command, "@Customer_Name", DbType.String, objCustRow.Customer_Name);
                        db.AddInParameter(command, "@Contact_name", DbType.String, objCustRow.Contact_name);
                        db.AddInParameter(command, "@Contact_no", DbType.String, objCustRow.Contact_no);
                        db.AddInParameter(command, "@Address1", DbType.String, objCustRow.Address1);
                        db.AddInParameter(command, "@Address2", DbType.String, objCustRow.Address2);
                        db.AddInParameter(command, "@City", DbType.String, objCustRow.City);
                        db.AddInParameter(command, "@State", DbType.Int32, objCustRow.State);
                        db.AddInParameter(command, "@Country", DbType.String, objCustRow.Country);
                        db.AddInParameter(command, "@Pincode", DbType.String, objCustRow.Pincode);
                        db.AddInParameter(command, "@Mobile", DbType.String, objCustRow.Mobile);
                        db.AddInParameter(command, "@Telephone", DbType.String, objCustRow.Telephone);
                        db.AddInParameter(command, "@EMail", DbType.String, objCustRow.EMail);
                        db.AddInParameter(command, "@Website", DbType.String, objCustRow.Website);
                        db.AddInParameter(command, "@Posting_Group_Code", DbType.Int32, objCustRow.Posting_Group_Code);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@Customer_ID", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@CustomerCode", DbType.String, 200);

                        using (DbConnection con = db.CreateConnection())
                        {
                            con.Open();
                            DbTransaction trans = con.BeginTransaction();
                            try
                            {

                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);

                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                if (intRowsAffected == 0)
                                {
                                    strDocNo = Convert.ToString(command.Parameters["@CustomerCode"].Value);
                                    intCustomerID = Convert.ToInt32(command.Parameters["@Customer_ID"].Value);
                                    trans.Commit();
                                }
                                else
                                {
                                    trans.Rollback();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
                                trans.Rollback();
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            }
                            con.Close();
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


            public int FunPubCreateLead(SerializationMode SerMode, byte[] bytesObjS3G_Colection_Customer_DataTable, out string strDocNo, out Int64 intLeadID)
            {
                intLeadID = 0;
                strDocNo = "";
                try
                {
                    objLead = (S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_LEADDTLDataTable
                        )ClsPubSerialize.
                        DeSerialize(bytesObjS3G_Colection_Customer_DataTable,
                        SerMode, typeof(S3GBusEntity.Collection.
                        ClnDataMgtServices.S3G_ORG_LEADDTLRow));

                    foreach (S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_LEADDTLRow objCustRow in objLead)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_INSERTLEADDTL");
                        db.AddInParameter(command, "@Lead_Id", DbType.Int64, objCustRow.Lead_Id);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objCustRow.Company_ID);
                        db.AddInParameter(command, "@CRM_ID", DbType.Int64, objCustRow.CRM_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int64, objCustRow.Customer_ID);
                        db.AddInParameter(command, "@Finance_Mode", DbType.Int32, objCustRow.Finance_Mode);
                        db.AddInParameter(command, "@Lob_ID", DbType.Int32, objCustRow.Lob_ID);
                        db.AddInParameter(command, "@Lead_source_Type", DbType.Int32, objCustRow.Lead_source_Type);
                        db.AddInParameter(command, "@Other_Source_Type", DbType.String, objCustRow.Other_Source_Type);
                        db.AddInParameter(command, "@Lead_Source", DbType.Int32, objCustRow.Lead_Source);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objCustRow.Location_ID);
                        db.AddInParameter(command, "@Acct_Mgr1", DbType.Int32, objCustRow.Acct_Mgr1);
                        db.AddInParameter(command, "@Acct_Mgr2", DbType.Int32, objCustRow.Acct_Mgr2);
                        db.AddInParameter(command, "@Lead_Status", DbType.Int32, objCustRow.Lead_Status);
                        db.AddInParameter(command, "@Lead_Information", DbType.String, objCustRow.Lead_Information);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, objCustRow.Constitution_ID);
                        db.AddInParameter(command, "@Funding_Type", DbType.Int32, objCustRow.Funding_Type);
                        db.AddInParameter(command, "@Sales_Person", DbType.Int32, objCustRow.Sales_Person);
                        db.AddInParameter(command, "@Competitor_Info", DbType.String, objCustRow.Competitor_Info);
                        db.AddInParameter(command, "@Drop_Remarks", DbType.String, objCustRow.Drop_Remarks);
                        db.AddInParameter(command, "@CP_Name", DbType.String, objCustRow.CP_Name);
                        db.AddInParameter(command, "@CP_Phone_No", DbType.String, objCustRow.CP_Phone_No);
                        db.AddInParameter(command, "@CP_Designation", DbType.String, objCustRow.CP_Designation);
                        db.AddInParameter(command, "@CP_Email", DbType.String, objCustRow.CP_Email);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objCustRow.Created_By);
                        db.AddInParameter(command, "@Acc_Status", DbType.Int32, objCustRow.Account_Status);
                        if (!objCustRow.IsXML_LeadDetailsNull())
                        {
                            db.AddInParameter(command, "@XML_LeadDetails", DbType.String, objCustRow.XML_LeadDetails);
                        }

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@Lead_ID_Out", DbType.Int64, sizeof(Int64));
                        db.AddOutParameter(command, "@Lead_No", DbType.String, 200);

                        using (DbConnection con = db.CreateConnection())
                        {
                            con.Open();
                            DbTransaction trans = con.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);

                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                if (intRowsAffected == 0)
                                {
                                    strDocNo = Convert.ToString(command.Parameters["@Lead_No"].Value);
                                    intLeadID = Convert.ToInt64(command.Parameters["@Lead_ID_Out"].Value);
                                    trans.Commit();
                                }
                                else
                                {
                                    trans.Rollback();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
                                trans.Rollback();
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            }
                            con.Close();
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

            public int FunPubCreateProspect(SerializationMode SerMode, byte[] bytesPspct_Data, out Int64 intCRMID)
            {
                intCRMID = 0;
                try
                {
                    objPspct = (S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_ProspectDataTable
                        )ClsPubSerialize.
                        DeSerialize(bytesPspct_Data,
                        SerMode, typeof(S3GBusEntity.Collection.
                        ClnDataMgtServices.S3G_ORG_ProspectRow));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_ProspectRow objCustRow in objPspct)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Org_Insert_CRM");
                        db.AddInParameter(command, "@Option", DbType.Int32, objCustRow.Option);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objCustRow.Company_ID);
                        db.AddInParameter(command, "@CRM_ID", DbType.Int64, objCustRow.CRM_ID);
                        db.AddInParameter(command, "@Prospect_Title", DbType.Int32, objCustRow.Prospect_Title);
                        db.AddInParameter(command, "@Prospect_Name", DbType.String, objCustRow.Prospect_Name);
                        db.AddInParameter(command, "@Cutsomer_Type", DbType.Int32, objCustRow.Cutsomer_Type);
                        db.AddInParameter(command, "@Company_Type", DbType.Int32, objCustRow.Company_Type);
                        db.AddInParameter(command, "@Industry_Type", DbType.Int32, objCustRow.Industry_Type);
                        db.AddInParameter(command, "@Address1", DbType.String, objCustRow.Address1);
                        db.AddInParameter(command, "@Address2", DbType.String, objCustRow.Address2);
                        db.AddInParameter(command, "@City", DbType.String, objCustRow.City);
                        db.AddInParameter(command, "@State", DbType.Int32, objCustRow.State);
                        db.AddInParameter(command, "@Country", DbType.String, objCustRow.Country);
                        db.AddInParameter(command, "@Pincode", DbType.String, objCustRow.Pincode);
                        db.AddInParameter(command, "@Mobile", DbType.String, objCustRow.Mobile);
                        db.AddInParameter(command, "@Telephone", DbType.String, objCustRow.Telephone);
                        db.AddInParameter(command, "@EMail", DbType.String, objCustRow.EMail);
                        db.AddInParameter(command, "@Website", DbType.String, objCustRow.Website);
                        db.AddInParameter(command, "@Prospect_Constitution_ID", DbType.Int32, objCustRow.Prospect_Constitution_ID);
                        db.AddInParameter(command, "@Prospect_Cnt_Person", DbType.String, objCustRow.Prospect_Cnt_Person);
                        db.AddInParameter(command, "@Prospect_Cnt_No", DbType.String, objCustRow.Prospect_Cnt_No);
                        db.AddInParameter(command, "@Prospect_Cnt_Designation", DbType.String, objCustRow.Prospect_Cnt_Designation);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objCustRow.Created_By);

                        db.AddOutParameter(command, "@Error_Code", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@CRM_ID_OUT", DbType.Int64, sizeof(Int64));

                        using (DbConnection con = db.CreateConnection())
                        {
                            con.Open();
                            DbTransaction trans = con.BeginTransaction();
                            try
                            {

                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);

                                intRowsAffected = (int)command.Parameters["@Error_Code"].Value;

                                if (intRowsAffected == 0)
                                {
                                    intCRMID = Convert.ToInt64(command.Parameters["@CRM_ID_OUT"].Value);
                                    trans.Commit();
                                }
                                else
                                {
                                    trans.Rollback();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
                                trans.Rollback();
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            }
                            con.Close();
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

            public int FunPubCreateTrackDetails(SerializationMode SerMode, byte[] bytesTrack_Data, out Int64 intTrackID)
            {
                intTrackID = 0;
                try
                {
                    S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_TRACKDTLDataTable objTrack = (S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_TRACKDTLDataTable
                        )ClsPubSerialize.
                        DeSerialize(bytesTrack_Data,
                        SerMode, typeof(S3GBusEntity.Collection.
                        ClnDataMgtServices.S3G_ORG_TRACKDTLRow));

                    foreach (S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_TRACKDTLRow objRow in objTrack)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_INSERT_TRACKDTL");
                        db.AddInParameter(command, "@Lead_ID", DbType.Int64, objRow.Lead_ID);
                        db.AddInParameter(command, "@Track_Date", DbType.DateTime, objRow.Track_Date);
                        db.AddInParameter(command, "@Track_Type", DbType.Int32, objRow.Track_Type);
                        db.AddInParameter(command, "@Ticket_No", DbType.Int32, objRow.Ticket_No);
                        db.AddInParameter(command, "@Mode", DbType.Int32, objRow.Mode);
                        db.AddInParameter(command, "@From_Type", DbType.Int32, objRow.From_Type);
                        db.AddInParameter(command, "@From_User", DbType.Int32, objRow.From_User);
                        db.AddInParameter(command, "@To_Type", DbType.Int32, objRow.To_Type);
                        db.AddInParameter(command, "@To_User", DbType.Int32, objRow.To_User);
                        db.AddInParameter(command, "@Description", DbType.String, objRow.Description);
                        db.AddInParameter(command, "@Target_Date", DbType.DateTime, objRow.Target_Date);
                        db.AddInParameter(command, "@Ticket_Status", DbType.Int32, objRow.Ticket_Status);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objRow.Created_By);
                        db.AddInParameter(command, "@Track_Detail_ID", DbType.Int64, objRow.Track_Detail_ID);

                        db.AddOutParameter(command, "@Error_Code", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@Track_ID_Out", DbType.Int64, sizeof(Int64));

                        using (DbConnection con = db.CreateConnection())
                        {
                            con.Open();
                            DbTransaction trans = con.BeginTransaction();
                            try
                            {

                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);

                                intRowsAffected = (int)command.Parameters["@Error_Code"].Value;

                                if (intRowsAffected == 0)
                                {
                                    intTrackID = Convert.ToInt64(command.Parameters["@Track_ID_Out"].Value);
                                    trans.Commit();
                                }
                                else
                                {
                                    trans.Rollback();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
                                trans.Rollback();
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            }
                            con.Close();
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

            public int FunPubCreateDocumentDetails(SerializationMode SerMode, byte[] bytesDoc_Data, out Int64 intDocID)
            {
                intDocID = 0;
                try
                {
                    S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_DocumentDtlDataTable objDoc = (S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_DocumentDtlDataTable
                        )ClsPubSerialize.
                        DeSerialize(bytesDoc_Data,
                        SerMode, typeof(S3GBusEntity.Collection.
                        ClnDataMgtServices.S3G_ORG_DocumentDtlRow));

                    foreach (S3GBusEntity.Collection.ClnDataMgtServices.S3G_ORG_DocumentDtlRow objRow in objDoc)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Org_Insert_CRM");
                        db.AddInParameter(command, "@Option", DbType.Int32, objRow.Option);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objRow.Company_ID);
                        db.AddInParameter(command, "@CRM_ID", DbType.Int64, objRow.CRM_ID);
                        db.AddInParameter(command, "@CRM_Doc_ID", DbType.Int64, objRow.CRM_Doc_ID);
                        db.AddInParameter(command, "@Document_Type_ID", DbType.Int32, objRow.Document_Type_ID);
                        db.AddInParameter(command, "@Document_SubType_ID", DbType.Int32, objRow.Document_SubType_ID);
                        db.AddInParameter(command, "@Collected_By", DbType.Int32, objRow.Collected_By);
                        db.AddInParameter(command, "@Document_Value", DbType.String, objRow.Document_Value);
                        if (!objRow.IsCollected_DateNull())
                        {
                            db.AddInParameter(command, "@Collected_Date", DbType.DateTime, objRow.Collected_Date);
                        }
                        db.AddInParameter(command, "@Scanned_By", DbType.Int32, objRow.Scanned_By);
                        if (!objRow.IsScanned_DateNull())
                        {
                            db.AddInParameter(command, "@Scanned_Date", DbType.DateTime, objRow.Scanned_Date);
                        }
                        db.AddInParameter(command, "@Remarks", DbType.String, objRow.Remarks);
                        if (!objRow.IsScan_PathNull())
                        {
                            db.AddInParameter(command, "@Scan_Path", DbType.String, objRow.Scan_Path);
                        }
                        db.AddInParameter(command, "@IS_Collected", DbType.Int32, objRow.IS_Collected);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objRow.Created_By);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int64, objRow.Customer_ID);

                        db.AddOutParameter(command, "@Error_Code", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@CRM_ID_OUT", DbType.Int64, sizeof(Int64));

                        using (DbConnection con = db.CreateConnection())
                        {
                            con.Open();
                            DbTransaction trans = con.BeginTransaction();
                            try
                            {

                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);

                                intRowsAffected = (int)command.Parameters["@Error_Code"].Value;

                                if (intRowsAffected == 0)
                                {
                                    intDocID = Convert.ToInt64(command.Parameters["@CRM_ID_OUT"].Value);
                                    trans.Commit();
                                }
                                else
                                {
                                    trans.Rollback();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
                                trans.Rollback();
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            }
                            con.Close();
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
