#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Workflow Status and ToDo List
/// Created By			: Prabhu
/// Created Date		: 02-Sep-2010
/// Purpose	            : To Manipulate the Workflow Status and ToDo List
/// Last Updated By		:
/// Last Updated Date   :
/// Reason              :
/// <Program Summary>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using System.Collections.Generic;
#endregion

namespace S3GDALayer
{
    /// Added the Name Space For Logical Grouping
    /// This Class belongs CompanyMgtServices to the service group
    namespace WorkflowMgtService
    {
        public class ClsPubWorkflowStatus
        {
            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubWorkflowStatus()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubInsertWorkflowStatus(WorkFlowStatus objWorkFlowStatus)
            {
                try
                {

                    int intErrorCode = 0;
                    DataSet dsWorkflowMasterDetails = FunPriGetWorkflowMasterDetails(objWorkFlowStatus.intCompanyId, objWorkFlowStatus.intLOBId, objWorkFlowStatus.intProductId, objWorkFlowStatus.intProgramRefNo, objWorkFlowStatus.strWorkSequenceId);
                    if (dsWorkflowMasterDetails.Tables.Count > 0)
                    {
                        if (dsWorkflowMasterDetails.Tables[0].Rows.Count > 0)
                        {
                            objWorkFlowStatus.strWorkPrgId = Convert.ToString(dsWorkflowMasterDetails.Tables[0].Rows[0]["Workflow_Prg_Id"]);
                            if (objWorkFlowStatus.strWorkPrgId.Trim().EndsWith("0"))
                            {
                                objWorkFlowStatus.strTaskStatus = "Initiated";
                            }
                            else
                            {
                                objWorkFlowStatus.strTaskStatus = "Consumed";
                            }
                        }
                    }
                    if (objWorkFlowStatus.strTaskStatus == "Initiated")
                    {
                        if (dsWorkflowMasterDetails.Tables.Count > 0)
                        {
                            if (dsWorkflowMasterDetails.Tables[1].Rows.Count > 0)
                            {
                                intErrorCode = FunPriInsertWorkflowStatusToDo(objWorkFlowStatus);
                            }
                        }
                    }
                    else
                    {
                        DataRow[] drArrWorkflowMaster = dsWorkflowMasterDetails.Tables[1].Select("Workflow_sequnce_Id = '" + objWorkFlowStatus.strWorkSequenceId + "'");
                        if (drArrWorkflowMaster.Length > 0)
                        {
                            intErrorCode = FunPriInsertWorkflowStatusToDo(objWorkFlowStatus);
                        }
                    }
                    return intErrorCode;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            private DataSet FunPriGetWorkflowMasterDetails(int intCompanyId, int intLOBId, int intProductId, int intProgramRefNo, string strWorkflowSequenceId)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3g_SysAd_GetWorkFlowMasterDetails");
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@LOBId", DbType.Int32, intLOBId);
                    db.AddInParameter(command, "@ProductId", DbType.Int32, intProductId);
                    db.AddInParameter(command, "@ProgramRefNo", DbType.Int32, intProgramRefNo);
                    db.AddInParameter(command, "@WorkflowSequenceId", DbType.String, strWorkflowSequenceId);
                    db.LoadDataSet(command, ObjDS, "WorkflowMasterDetails");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public Int32 FunPriInsertWorkflowStatusToDo(WorkFlowStatus objWorkFlowStatus)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3g_SysAd_InsertWorkFlowStatusToDO");
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, objWorkFlowStatus.intCompanyId);
                    db.AddInParameter(command, "@LOBId", DbType.Int32, objWorkFlowStatus.intLOBId);
                    db.AddInParameter(command, "@BranchId", DbType.Int32, objWorkFlowStatus.intBranchId);
                    db.AddInParameter(command, "@UserId", DbType.Int32, objWorkFlowStatus.intUserId);
                    db.AddInParameter(command, "@TaskDocumentNo", DbType.String, objWorkFlowStatus.strTaskDocumentNo);
                    db.AddInParameter(command, "@WorkflowPrgId", DbType.String, objWorkFlowStatus.strWorkPrgId);
                    db.AddInParameter(command, "@TaskStatus", DbType.String, objWorkFlowStatus.strTaskStatus);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.ExecuteNonQuery(command);
                    return (Int32)command.Parameters["@ErrorCode"].Value;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public DataSet FunPubLoadPreDisDocTransaction(string strEnquiryNo, int intCompanyId, int intDocument_Type)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_WORKFLOW_LoadPreDisDocTransaction);
                    db.AddInParameter(command, "@EnquiryNo", DbType.String, strEnquiryNo);
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@Document_Type", DbType.String, intDocument_Type);
                    db.LoadDataSet(command, ObjDS, "WorkflowPreDisDocTransaction");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public DataSet FunPubLoadFIRTransaction(string strEnquiryNo, int intCompanyId, int intDocument_Type, string PANum, string SANum)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_WORKFLOW_LoadFIR");
                    db.AddInParameter(command, "@EnquiryNo", DbType.String, strEnquiryNo);
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@Document_Type", DbType.String, intDocument_Type);
                    if (!string.IsNullOrEmpty(PANum))
                        db.AddInParameter(command, "@PANum", DbType.String, PANum);
                    if (!string.IsNullOrEmpty(SANum))
                        db.AddInParameter(command, "@SANum", DbType.String, SANum);
                    db.LoadDataSet(command, ObjDS, "WorkflowFIR");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public DataSet FunPubLoadProforma(string strEnquiryNo, int intCompanyId, int intDocument_Type, string PANum, string SANum)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_WORKFLOW_Proforma");
                    db.AddInParameter(command, "@EnquiryNo", DbType.String, strEnquiryNo);
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@Document_Type", DbType.String, intDocument_Type);
                    if (!string.IsNullOrEmpty(PANum))
                        db.AddInParameter(command, "@PANum", DbType.String, PANum);
                    if (!string.IsNullOrEmpty(SANum))
                        db.AddInParameter(command, "@SANum", DbType.String, SANum);
                    db.LoadDataSet(command, ObjDS, "WorkflowFIR");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public DataSet FunPubLoadInvoice(string strEnquiryNo, int intCompanyId, int intDocument_Type, string PANum, string SANum)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_WORKFLOW_Invoice");
                    db.AddInParameter(command, "@EnquiryNo", DbType.String, strEnquiryNo);
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@Document_Type", DbType.String, intDocument_Type);
                    if (!string.IsNullOrEmpty(PANum))
                        db.AddInParameter(command, "@PANum", DbType.String, PANum);
                    if (!string.IsNullOrEmpty(SANum))
                        db.AddInParameter(command, "@SANum", DbType.String, SANum);
                    db.LoadDataSet(command, ObjDS, "WorkflowInvoice");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public DataSet FunPubLoadPostDisb(string strEnquiryNo, int intCompanyId, int intDocument_Type, string PANum, string SANum)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_WORKFLOW_PostDisb");
                    db.AddInParameter(command, "@EnquiryNo", DbType.String, strEnquiryNo);
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@Document_Type", DbType.String, intDocument_Type);
                    if (!string.IsNullOrEmpty(PANum))
                        db.AddInParameter(command, "@PANum", DbType.String, PANum);
                    if (!string.IsNullOrEmpty(SANum))
                        db.AddInParameter(command, "@SANum", DbType.String, SANum);
                    db.LoadDataSet(command, ObjDS, "WorkflowPostDisb");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public DataSet FunPubLoadEnquiryCustomerAppraisal(string strEnquiryNo, int intCompanyId, int intDocument_Type)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_WORKFLOW_LoadEnquiryCustomerAppraisal);
                    db.AddInParameter(command, "@EnquiryNo", DbType.String, strEnquiryNo);
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@Document_Type", DbType.String, intDocument_Type);
                    db.LoadDataSet(command, ObjDS, "EnquiryCustomerAppraisal");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public DataSet FunPubLoadCreditGuideTransaction(string strEnquiryNo, int intCompanyId, int intDocument_Type)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_WORKFLOW_LoadCreditGuideTransaction);
                    db.AddInParameter(command, "@EnquiryNo", DbType.String, strEnquiryNo);
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@Document_Type", DbType.String, intDocument_Type);
                    db.LoadDataSet(command, ObjDS, "CreditGuideTransaction");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public DataSet FunPubLoadCreditParameterApproval(string strEnquiryNo, int intCompanyId, int intDocument_Type)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_WORKFLOW_LoadCreditParameterApproval);
                    db.AddInParameter(command, "@EnquiryNo", DbType.String, strEnquiryNo);
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@Document_Type", DbType.String, intDocument_Type);
                    db.LoadDataSet(command, ObjDS, "CreditParameterApproval");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public DataSet FunPubLoadPricing(string strEnquiryNo, int intCompanyId, int intDocument_Type)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_WORKFLOW_LoadPricing);
                    db.AddInParameter(command, "@EnquiryNo", DbType.String, strEnquiryNo);
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@Document_Type", DbType.String, intDocument_Type);
                    db.LoadDataSet(command, ObjDS, "Pricing");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public DataSet FunPubLoadPricingApproval(string strEnquiryNo, int intCompanyId, int intDocument_Type)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_WORKFLOW_LoadPricingApproval);
                    db.AddInParameter(command, "@EnquiryNo", DbType.String, strEnquiryNo);
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@Document_Type", DbType.String, intDocument_Type);
                    db.LoadDataSet(command, ObjDS, "Pricing");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Load Application Approval Based on Enquiry No
            /// </summary>
            /// <param name="strEnquiryNo"></param>
            /// <param name="intCompanyId"></param>
            /// <returns></returns>     
            public DataSet FunPubLoadApplicationApproval(string strApplicationNo, int intCompanyId, int intUserId)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_GEN_GetAppProcessIDFromCode");
                    db.AddInParameter(command, "@Application_Number", DbType.String, strApplicationNo);
                    db.AddInParameter(command, "@Company_Id", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@User_Id", DbType.Int32, intUserId);
                    db.LoadDataSet(command, ObjDS, "ApplicationProcessing");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            public DataSet FunPubGetUserAccess(int intUserId, int intCompanyId)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3g_SysAd_WorkflowUserAccess");
                    db.AddInParameter(command, "@UserId", DbType.Int32, intUserId);
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    db.LoadDataSet(command, ObjDS, "UserAccess");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
