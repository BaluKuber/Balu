
using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;

namespace S3GDALayer.EnquiryMgtServices
{
    public class ClsPubEnquiryAssignment
    {

        #region Local Varaiables

        string _RouterInsertSP = "S3G_ORG_Router_Insert";
        string _RouterUpdateSP = "S3G_ORG_Router_Update";
        string _EnquiryUpdateDetails = "SELECT * FROM S3G_DV_GetEnquiryUpdateDetails WHERE EnquiryUpdateID=";
        string _S3G_ORG_DV_GetEnquiryUpdateDetails = "S3G_DV_GetEnquiryUpdateDetails";
        string _S3G_DV_GetEnquiryUpdateTOAssignment = "SELECT * FROM S3G_DV_GetEnquiryUpdateTOAssignment";
        string _RouterSelect = "SELECT LOB_ID,Location_Id Branch_ID,WorkFlow_Seq_ID,Product_ID FROM S3G_ORG_Router inner join s3g_sysad_locationmaster LOC on LOC.location_code = S3G_ORG_Router.Location_Code WHERE EnquiryUpdation_ID=";
        //
        string _S3G_ORG_GetEnquiryUpdateAssignDetails = "S3G_ORG_GetEnquiryUpdateAssignDetails";

        //Code added for getting common connection string  from config file
        Database db;
        public ClsPubEnquiryAssignment()
        {
            db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
        }

        #endregion

        /// <summary>
        /// To Insert Record into Router
        /// </summary>
        /// <param name="SMode">Pass Serialization Mode</param>
        /// <param name="byteEnquiryService">Pass type Object</param>
        public void FunPubInsertRouter(SerializationMode SMode, byte[] byteEnquiryService)
        {
            try
            {
                S3GBusEntity.Origination.EnquiryService.S3G_ORG_RouterDataTable ObjS3G_ORG_RouterDataTable = new S3GBusEntity.Origination.EnquiryService.S3G_ORG_RouterDataTable();
                ObjS3G_ORG_RouterDataTable = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_RouterDataTable)ClsPubSerialize.DeSerialize(byteEnquiryService, SMode, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_RouterDataTable));
                S3GBusEntity.Origination.EnquiryService.S3G_ORG_RouterRow ObjEnquiryServiceRow = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_RouterRow)ObjS3G_ORG_RouterDataTable.Rows[0];

                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                DbCommand command = db.GetStoredProcCommand(_RouterInsertSP);
                db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjEnquiryServiceRow.Company_ID);
                db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjEnquiryServiceRow.LOB_ID);
                db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjEnquiryServiceRow.Branch_ID);
                db.AddInParameter(command, "@EnquiryNumber", DbType.String, ObjEnquiryServiceRow.EnquiryUpdation_ID);
                if (!ObjEnquiryServiceRow.IsWorkFlow_Seq_IDNull())
                    db.AddInParameter(command, "@WorkFlowSeq_ID", DbType.Int32, ObjEnquiryServiceRow.WorkFlow_Seq_ID);
                db.AddInParameter(command, "@INEmployee_ID", DbType.Int32, ObjEnquiryServiceRow.In_Employee_ID);
                db.AddInParameter(command, "@Status", DbType.Boolean, ObjEnquiryServiceRow.Status);
                db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjEnquiryServiceRow.Product_ID);

                db.FunPubExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Update record into Router
        /// </summary>
        /// <param name="SMode">Pass Serialization Mode</param>
        /// <param name="byteEnquiryService">Pass type Object</param>
        public void FunPubUpdateRouter(SerializationMode SMode, byte[] byteEnquiryService)
        {
            try
            {
                S3GBusEntity.Origination.EnquiryService.S3G_ORG_RouterDataTable ObjS3G_ORG_RouterDataTable = new S3GBusEntity.Origination.EnquiryService.S3G_ORG_RouterDataTable();
                ObjS3G_ORG_RouterDataTable = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_RouterDataTable)ClsPubSerialize.DeSerialize(byteEnquiryService, SMode, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_RouterDataTable));
                S3GBusEntity.Origination.EnquiryService.S3G_ORG_RouterRow ObjEnquiryServiceRow = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_RouterRow)ObjS3G_ORG_RouterDataTable.Rows[0];

                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                DbCommand command = db.GetStoredProcCommand(_RouterUpdateSP);

                db.AddInParameter(command, "@EnquiryNumber", DbType.String, ObjEnquiryServiceRow.EnquiryUpdation_ID);
                db.AddInParameter(command, "@OutEmployee_ID", DbType.Int32, ObjEnquiryServiceRow.Out_Employee_ID);
                db.AddInParameter(command, "@Status", DbType.Boolean, ObjEnquiryServiceRow.Status);

                db.FunPubExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To use Get Enquiry Update details
        /// </summary>
        /// <param name="EnquiryNo">Pass Enquiry</param>
        /// <returns></returns>

        public S3GBusEntity.Origination.EnquiryService.S3G_ORG_DV_GetEnquiryUpdateDetailsDataTable FunPubGetEnquiryUpdateDetails(string EnquiryNo)
        {
            try
            {
                S3GBusEntity.Origination.EnquiryService ObjEnquiryService = new S3GBusEntity.Origination.EnquiryService();
                S3GBusEntity.Origination.EnquiryService.S3G_ORG_DV_GetEnquiryUpdateDetailsDataTable ObjS3G_ORG_DV_GetEnquiryUpdateDetailsDataTable = new S3GBusEntity.Origination.EnquiryService.S3G_ORG_DV_GetEnquiryUpdateDetailsDataTable();
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                _EnquiryUpdateDetails = _EnquiryUpdateDetails + "'" + Convert.ToInt32(EnquiryNo) + "'";
                DbCommand command = db.GetSqlStringCommand(_EnquiryUpdateDetails);

                //Method access changed for ORACLE conversion - Kuppusamy.B - 10/11/2011 
                //db.FunPubLoadDataSet(command, ObjEnquiryService, ObjEnquiryService.S3G_ORG_DV_GetEnquiryUpdateDetails.TableName);
                db.FunPubLoadDataSetStringQuery(command, ObjEnquiryService, ObjEnquiryService.S3G_ORG_DV_GetEnquiryUpdateDetails.TableName);

                return ObjEnquiryService.S3G_ORG_DV_GetEnquiryUpdateDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable FunPubGetEnquiryTOAssign()
        {
            try
            {
                DataSet ObjDS = new DataSet();
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetSqlStringCommand(_S3G_DV_GetEnquiryUpdateTOAssignment);

                //db.FunPubLoadDataSetStringQuery(command, ObjDS, _S3G_ORG_DV_GetEnquiryUpdateDetails);
                db.FunPubLoadDataSet(command, ObjDS, _S3G_ORG_DV_GetEnquiryUpdateDetails);

                return (DataTable)ObjDS.Tables[_S3G_ORG_DV_GetEnquiryUpdateDetails];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public S3GBusEntity.Origination.EnquiryService.S3G_RouterDetailsDataTable FunPubGetRouterDetails(string EnquiryNo)
        {
            try
            {
                S3GBusEntity.Origination.EnquiryService ObjEnquiryService = new S3GBusEntity.Origination.EnquiryService();
                S3GBusEntity.Origination.EnquiryService.S3G_RouterDetailsDataTable ObjS3G_ORG_RouterDataTable = new S3GBusEntity.Origination.EnquiryService.S3G_RouterDetailsDataTable();

                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                _RouterSelect = _RouterSelect + "'" + Convert.ToInt32(EnquiryNo) + "'";

                DbCommand command = db.GetSqlStringCommand(_RouterSelect);

                //Method access changed for ORACLE conversion - Kuppusamy.B - 11/11/2011 
                //db.FunPubLoadDataSet(command, ObjEnquiryService, "S3G_RouterDetails");
                db.FunPubLoadDataSetStringQuery(command, ObjEnquiryService, "S3G_RouterDetails");
                ObjS3G_ORG_RouterDataTable = (S3GBusEntity.Origination.EnquiryService.S3G_RouterDetailsDataTable)ObjEnquiryService.Tables["S3G_RouterDetails"];
                return ObjS3G_ORG_RouterDataTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable FunPubGetEnquiryUpdateAssign(string StartDate, string EndDate)
        {
            try
            {
                DataSet ObjDS = new DataSet();
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand(_S3G_ORG_GetEnquiryUpdateAssignDetails);
                db.AddInParameter(command, "@StartDate", DbType.String, StartDate);
                db.AddInParameter(command, "@EndDate", DbType.String, EndDate);
                db.FunPubLoadDataSet(command, ObjDS, _S3G_ORG_GetEnquiryUpdateAssignDetails);
                return (DataTable)ObjDS.Tables[_S3G_ORG_GetEnquiryUpdateAssignDetails];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public class ClsPubCompanyHierarchy
    {
        #region Local variables
        string _S3G_ORG_GetBranchList = "S3G_ORG_GetBranchList";
        string _S3G_ORG_GetUserLOBList = "S3G_ORG_GetUserLOBList";
        string _S3G_ORG_GetLOBProductList = "S3G_ORG_GetLOBProductList";
        string _S3G_ORG_GetWorkFlowListEnqAssignment = "S3G_ORG_GetWorkFlowListEnqAssignment";

        //Code added for getting common connection string  from config file
        Database db;
        public ClsPubCompanyHierarchy()
        {
            db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
        }

        #endregion

        /// <summary>
        /// To Get User Branch List in Active only
        /// </summary>
        /// <param name="ObjCompanyHierarchyEntity">Pass Proper Object</param>
        /// <returns>It should be datatable</returns>
        public DataTable FunPubGetUserBranchList(S3GBusEntity.CompanyHierarchyEntity ObjCompanyHierarchyEntity)
        {
            try
            {
                DataSet ObjDS = new DataSet();
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand(_S3G_ORG_GetBranchList);

                db.AddInParameter(command, "@CompanyID", DbType.Int32, ObjCompanyHierarchyEntity.CompanyID);
                db.AddInParameter(command, "@UserID", DbType.Int32, ObjCompanyHierarchyEntity.UserID);
                db.FunPubLoadDataSet(command, ObjDS, _S3G_ORG_GetBranchList);
                return (DataTable)ObjDS.Tables[_S3G_ORG_GetBranchList];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Get User LOB List in Active only
        /// </summary>
        /// <param name="ObjCompanyHierarchyEntity">Pass Proper Object</param>
        /// <returns>It should be datatable</returns>
        public DataTable FunPubGetUserLOBList(S3GBusEntity.CompanyHierarchyEntity ObjCompanyHierarchyEntity)
        {
            try
            {
                DataSet ObjDS = new DataSet();
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand(_S3G_ORG_GetUserLOBList);

                db.AddInParameter(command, "@CompanyID", DbType.Int32, ObjCompanyHierarchyEntity.CompanyID);
                db.AddInParameter(command, "@UserID", DbType.Int32, ObjCompanyHierarchyEntity.UserID);
                db.FunPubLoadDataSet(command, ObjDS, _S3G_ORG_GetUserLOBList);
                return (DataTable)ObjDS.Tables[_S3G_ORG_GetUserLOBList];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Get User LOB Product List in Active only
        /// </summary>
        /// <param name="ObjCompanyHierarchyEntity">Pass Proper Object</param>
        /// <returns>It should be datatable</returns>
        public DataTable FunPubGetLOBProductList(S3GBusEntity.CompanyHierarchyEntity ObjCompanyHierarchyEntity)
        {
            try
            {
                DataSet ObjDS = new DataSet();
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand(_S3G_ORG_GetLOBProductList);

                db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjCompanyHierarchyEntity.LOBID);
                db.FunPubLoadDataSet(command, ObjDS, _S3G_ORG_GetLOBProductList);
                DataTable dt = new DataTable();
                dt = (DataTable)ObjDS.Tables[_S3G_ORG_GetLOBProductList];
                return (DataTable)ObjDS.Tables[_S3G_ORG_GetLOBProductList];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Get User Workflow sequence List in Active only
        /// </summary>
        /// <param name="ObjCompanyHierarchyEntity">Pass Proper Object</param>
        /// <returns>It should be datatable</returns>
        public DataTable FunPubGetWorkFlowList(S3GBusEntity.CompanyHierarchyEntity ObjCompanyHierarchyEntity)
        {
            try
            {
                DataSet ObjDS = new DataSet();
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand(SPNames.S3G_WORKFLOW_GetWorkFlowListEnqAssignment);
                db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjCompanyHierarchyEntity.LOBID);
                db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjCompanyHierarchyEntity.ProductID);
                db.AddInParameter(command, "@Program_Ref_No", DbType.String, ObjCompanyHierarchyEntity.ProgramCode);
                db.FunPubLoadDataSet(command, ObjDS, _S3G_ORG_GetWorkFlowListEnqAssignment);
                return (DataTable)ObjDS.Tables[_S3G_ORG_GetWorkFlowListEnqAssignment];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

