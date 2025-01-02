#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Workflow Master DAL Class
/// Created By			: Kaliraj K
/// Created Date		: 27-May-2010
/// Purpose	            : 
/// Last Updated By		: 
/// Last Updated Date   : 
/// Reason              : 
/// <Program Summary>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
#endregion

namespace S3GDALayer
{
    /// Added the Name Space For Logical Grouping
    /// This Class belongs CompanyMgtServices to the service group
    namespace CompanyMgtServices
    {
        public class ClsPubWorkflow
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_WorkflowDataTable ObjWorkflowDataTable_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubWorkflow()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }
            
            #endregion

            #region Create New Workflow
            /// <summary>
            /// Creates a new workflow by getting Serialized data table object and serialized mode
            /// Create and update workflow details based on workflow sequence id
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_WorkflowDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>    
            public int FunPubCreateWorkflow(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_WorkflowDataTable)
            {
                try
                {

                    ObjWorkflowDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_WorkflowDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_WorkflowDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_WorkflowDataTable));
                    DbCommand command = null;
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_WorkflowRow ObjWorkflowRow in ObjWorkflowDataTable_DAL.Rows)
                    {
                        command = db.GetStoredProcCommand(SPNames.S3G_WORKFLOW_InsertWorkflow_Details);
                        db.AddInParameter(command, "@Workflow_Sequence_ID", DbType.Int32, ObjWorkflowRow.Workflow_Sequence_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjWorkflowRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjWorkflowRow.LOB_Code);
                        db.AddInParameter(command, "@Product_CODE", DbType.String, ObjWorkflowRow.Product_Code);
                        //db.AddInParameter(command, "@Module_CODE", DbType.String, ObjWorkflowRow.Module_Code);
                        //db.AddInParameter(command, "@Program_ID", DbType.Int32, ObjWorkflowRow.Program_ID);
                        db.AddInParameter(command, "@Workflow_Sequnce", DbType.String, ObjWorkflowRow.Workflow_Sequnce);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjWorkflowRow.Is_Active);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjWorkflowRow.Created_By);
                        db.AddInParameter(command, "@XMLWorkflowParamDet", DbType.String, ObjWorkflowRow.XMLWorkflowParamDet);
                        db.AddOutParameter(command,"@ErrorCode", DbType.Int32, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);

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
            #endregion

            #region Query Workflow Details
            /// <summary>
            /// Gets a workflow details using Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjSNXG_WorkflowManagementDataTable"></param>
            /// <returns>Datatable containing workflow details</returns>

            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_WorkflowDataTable FunPubQueryWorkflow(SerializationMode SerMode, byte[] bytesObjSNXG_WorkflowDataTable)
            {
                S3GBusEntity.CompanyMgtServices dsWorkflow = new S3GBusEntity.CompanyMgtServices();
                ObjWorkflowDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_WorkflowDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_WorkflowDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_WorkflowDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_WORKFLOW_GetWorkflowDetails");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_WorkflowRow ObjWorkflowRow in ObjWorkflowDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjWorkflowRow.Company_ID);
                        db.AddInParameter(command, "@Workflow_Sequence_ID", DbType.Int32, ObjWorkflowRow.Workflow_Sequence_ID);
                        //db.LoadDataSet(command, dsWorkflow, dsWorkflow.S3G_SYSAD_Workflow.TableName);
                        db.FunPubLoadDataSet(command, dsWorkflow, dsWorkflow.S3G_SYSAD_Workflow.TableName);
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsWorkflow.S3G_SYSAD_Workflow;
                
            }
            #endregion

         
        }
    }
}
