using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using S3GBusEntity;

namespace S3GDALayer.LoanAdmin
{
    namespace AssetMgtServices
    {

        public class ClsPubAssetIdentification
        {
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetIdentificationDataTable objAssetIdentification_DAL;
            S3GBusEntity.LoanAdmin.AssetMgtServices.Shedule_ReportDataTable objSheduleReport_DAL;
            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubAssetIdentification()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            /// <summary>
            /// To get PANum realted customer details
            /// </summary>
            /// <param name="Asset_Verification_No">Pass PAN</param>
            /// <returns></returns>

            public DataTable FunGetPANumCustomer(string strPANum)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetPANumCustomer");
                    db.AddInParameter(command, "@PANum", DbType.String, strPANum);
                    //db.LoadDataSet(command, ObjDS, "dtTable");
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    return (DataTable)ObjDS.Tables["dtTable"];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// To get Invoice realted vendor details
            /// </summary>
            /// <param name="Asset_Verification_No">Pass vendor ID</param>
            /// <returns></returns>

            public DataTable FunGetAssetVendorDetails(int intVendorID)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LOANAD_GetAssetVendorDetails);
                    db.AddInParameter(command, "@Invoice_ID", DbType.Int32, intVendorID);
                    //db.LoadDataSet(command, ObjDS, "dtTable");
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    return (DataTable)ObjDS.Tables["dtTable"];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// To get Invoice realted asset details
            /// </summary>
            /// <param name="Asset_Verification_No">Pass vendor ID</param>
            /// <returns></returns>

            public DataTable FunGetAssetDetailsForVendor(int intInvoiceID,int intPASAID)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetAssetDetailsForInvoice");
                    db.AddInParameter(command, "@Invoice_ID", DbType.Int32, intInvoiceID);
                    db.AddInParameter(command, "@PA_SA_Ref_ID", DbType.Int32, intPASAID);
                    //db.LoadDataSet(command, ObjDS, "dtTable");
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    return (DataTable)ObjDS.Tables["dtTable"];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// To get the PA_SA_Ref_ID
            /// </summary>
            /// <param name="Asset_Verification_No">Pass PAN and SAN number</param>
            /// <returns></returns>

            public DataTable FunGetPASArefID(string intPAN, string intSAN)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetPASARefID");
                    db.AddInParameter(command, "@SANum", DbType.String, intSAN);
                    db.AddInParameter(command, "@PANum", DbType.String, intPAN);
                    //db.LoadDataSet(command, ObjDS, "dtTable");
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    return (DataTable)ObjDS.Tables["dtTable"];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Inserting the Asset Verification details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns>

            public int FunPubCreateAssetIdentification(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_AssetIdentification_DataTable, out string strDuplication, out string strDSNo)
            {
                try
                {
                    strDuplication = "";
                    strDSNo = "";
                    objAssetIdentification_DAL = (S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetIdentificationDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_AssetIdentification_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetIdentificationDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetIdentificationRow objAssetIdentificationRow in objAssetIdentification_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertIdentificationAccountDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objAssetIdentificationRow.Company_ID);
                        db.AddInParameter(command, "@AssetIDNo", DbType.String, objAssetIdentificationRow.Asset_Identification_No);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objAssetIdentificationRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objAssetIdentificationRow.Branch_ID);
                        db.AddInParameter(command, "@AssetDate", DbType.DateTime, objAssetIdentificationRow.Asset_Identification_Date);
                        db.AddInParameter(command, "@VendorID", DbType.Int32, objAssetIdentificationRow.Vendor_ID);
                        db.AddInParameter(command, "@VendorInvoiceID", DbType.Int32, objAssetIdentificationRow.Vendor_Invoice_ID);
                        db.AddInParameter(command, "@PA_SA_ID", DbType.Int32, objAssetIdentificationRow.PA_SA_ID);
                        db.AddInParameter(command, "@XMLAccount", DbType.String, objAssetIdentificationRow.XMLAccount);
                        db.AddInParameter(command, "@CreatedBy", DbType.Int32, objAssetIdentificationRow.Created_By);
                        db.AddInParameter(command, "@CreatedOn", DbType.DateTime, objAssetIdentificationRow.Created_Date);
                        db.AddInParameter(command, "@IsAdd", DbType.Int32, objAssetIdentificationRow.IsAdd);
                        db.AddInParameter(command, "@Is_OL", DbType.Boolean, objAssetIdentificationRow.IsOL);

                        db.AddOutParameter(command, "@Dub_Value", DbType.String, 100);
                        db.AddOutParameter(command, "@DSNO", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command);
                                db.FunPubExecuteNonQuery(command);

                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    strDuplication = (string)command.Parameters["@Dub_Value"].Value;
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
                                    strDSNo = (string)command.Parameters["@DSNO"].Value;
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
                    throw ex;
                }
                return intRowsAffected;
            }

            /// <summary>
            /// To get the PA_SA_Ref_ID
            /// </summary>
            /// <param name="Asset_Verification_No">Pass PAN and SAN number</param>
            /// <returns></returns>

            public DataTable FunGetAssetIdentificationforModify(string strAssetID, int intCompanyID, int intBranchID)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetAssetIdentificationforModify");
                    db.AddInParameter(command, "@AssetIdentificationNo", DbType.String, strAssetID);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompanyID);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, intBranchID);
                    //db.LoadDataSet(command, ObjDS, "dtTable");
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    return (DataTable)ObjDS.Tables["dtTable"];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public int FunPubCreateScheduleJobForReport(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_AssetIdentification_DataTable, out string strDuplication, out string strDSNo)
            {
                try
                {
                    strDuplication = "";
                    strDSNo = "";
                    objSheduleReport_DAL = (S3GBusEntity.LoanAdmin.AssetMgtServices.Shedule_ReportDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_AssetIdentification_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.AssetMgtServices.Shedule_ReportDataTable));
                    foreach (S3GBusEntity.LoanAdmin.AssetMgtServices.Shedule_ReportRow objScheduleRow in objSheduleReport_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_RP_INS_SCH_JOB");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objScheduleRow.Company_Id);
                        db.AddInParameter(command, "@LOB_ID", DbType.String, objScheduleRow.Lob_Id);
                        db.AddInParameter(command, "@Location_ID", DbType.String, objScheduleRow.Location_Id);
                       //if (objScheduleRow.From_Month != null)
                        //    db.AddInParameter(command, "@From_Month", DbType.String, objScheduleRow.From_Month);
                        //if (objScheduleRow.To_Month != null)
                        //    db.AddInParameter(command, "@To_Month", DbType.String, objScheduleRow.To_Month);
                        if (objScheduleRow.From_Date != null)
                            db.AddInParameter(command, "@From_Date", DbType.String, objScheduleRow.From_Date);
                        if (objScheduleRow.To_Date != null)
                            db.AddInParameter(command, "@To_Date", DbType.String, objScheduleRow.To_Date);

                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, objScheduleRow.Customer_ID);
                        db.AddInParameter(command, "@Funder_Id", DbType.Int32, objScheduleRow.Funder_Id);
                        db.AddInParameter(command, "@Vendor_Id", DbType.Int32, objScheduleRow.Vendor_Id);
                        db.AddInParameter(command, "@Asset_Status", DbType.Int32, objScheduleRow.Asset_Status);
                        db.AddInParameter(command, "@Vendor_Invoice_Status", DbType.Int32, objScheduleRow.Vendor_Invoice_Status);
                        db.AddInParameter(command, "@Invoice_Status", DbType.Int32, objScheduleRow.Invoice_Status);
                        db.AddInParameter(command, "@Asset_Category", DbType.Int32, objScheduleRow.Asset_Category);

                        //if (objScheduleRow.Account_Number != null)
                        //    db.AddInParameter(command, "@Account_Number", DbType.String, objScheduleRow.Account_Number);
                        db.AddInParameter(command, "@Report_Id", DbType.Int32, objScheduleRow.Program_Id);
                        db.AddInParameter(command, "@Schedule_At", DbType.String, objScheduleRow.Schedule_At);
                        db.AddInParameter(command, "@Formate", DbType.Int32, objScheduleRow.Formate);
                        db.AddInParameter(command, "@User_Id", DbType.Int32, objScheduleRow.User_Id);
                        db.AddInParameter(command, "@Report_Path", DbType.String, objScheduleRow.ReportPath);
                        //opc002 start
                        db.AddInParameter(command, "@Invoice_Type", DbType.Int32, objScheduleRow.Invoice_Type);
                        //opc002 end
                        //db.AddOutParameter(command, "@Dub_Value", DbType.String, 100);
                        //db.AddOutParameter(command, "@DSNO", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command);
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value; // By Rao - To get exact errorcode.
                                //if ((int)command.Parameters["@ErrorCode"].Value == 0)
                                //{
                                //    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;


                                //}
                                //else
                                //    intRowsAffected = 50;
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                trans.Rollback();
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
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
                    throw ex;
                }
                return intRowsAffected;
            }


            //Added for MIS Report On 01-Aug-2017 = > Code Starts
            public DataSet FunPubMISRptStatus(int intoption, int intScheduler_Id)
            {
                DataSet ds = new DataSet();
                try
                {
                    //DataSet ds = new DataSet();
                    DbCommand Cmd = db.GetStoredProcCommand("S3G_MIS_JobsStatus");
                    db.AddInParameter(Cmd, "@Option", DbType.Int32, intoption);
                    if (intScheduler_Id > 0)
                    {
                        db.AddInParameter(Cmd, "@Scheduler_Id", DbType.Int32, intScheduler_Id);
                    }
                    db.FunPubLoadDataSet(Cmd, ds, "MIS");
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return ds;
            }



            public int FunPubUpdate_Tally_Status(int intoption, int intTally_Integ_Hdr_ID, string strXML_Tally_Data)
            {
                try
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_Org_Update_Tally_Status");
                    db.AddInParameter(command, "@Option", DbType.Int32, intoption);
                    db.AddInParameter(command, "@Tally_Integ_Hdr_ID", DbType.Int32, intTally_Integ_Hdr_ID);
                    db.AddInParameter(command, "@XML_Tally_Data", DbType.String, strXML_Tally_Data);
                    
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command);
                            trans.Commit();
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
                    throw ex;
                }
                return intRowsAffected;
            }


            public DataSet FunPubPIDRptDetails(int intCompanyId, string strFromDate, string strToDate, int intUserID, int intIsExport, string strReportId, string strScheduleId)
            {
                DataSet ds = new DataSet();
                try
                {
                    //DataSet ds = new DataSet();
                    DbCommand Cmd = db.GetStoredProcCommand("S3G_RPT_ReportSchedule");
                    db.AddInParameter(Cmd, "@COMPANY_ID", DbType.Int32, intCompanyId);
                    db.AddInParameter(Cmd, "@InvoicePostingFromDate", DbType.String, strFromDate);
                    db.AddInParameter(Cmd, "@InvoicePostingToDate", DbType.String, strToDate);
                    db.AddInParameter(Cmd, "@User_ID", DbType.Int32, intUserID);
                    db.AddInParameter(Cmd, "@IsExport", DbType.Int32, intIsExport);
                    db.AddInParameter(Cmd, "@REPORT_ID ", DbType.String, strReportId);
                    db.AddInParameter(Cmd, "@SCHEDULER_ID", DbType.String, strScheduleId);
                    db.FunPubLoadDataSet(Cmd, ds, "PIDReportDetails");
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return ds;

            }
        }
    }
}


