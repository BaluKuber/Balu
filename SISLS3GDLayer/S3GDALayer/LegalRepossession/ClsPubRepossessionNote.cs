using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
namespace S3GDALayer.LegalRepossession
{
    namespace LegalAndRepossessionMgtServices
    {
        public class ClsPubRepossessionNote : ClsPubDalLegalRepossessionBase
        {
            #region Initialization
            int intRowsAffected;
            S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoteDataTable objRepossessionNote_DAL;
            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoteRow objRepossessionNoteRow = null;
            //S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_ApprovalDataTable objLRApproval_DataTable;
            //S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_ApprovalRow objLRApproval_Row = null;
            #endregion

            #region Create New Repossession Note
            public int FunPubCreateReposissionNote(SerializationMode SerMode, byte[] bytesObjS3G_LEGAL_RepossessionNote_DataTable, out string strDSNo)
            {
                strDSNo = "";
                try
                {
                    /*****   DATATABLES OBJECT  *****/
                    objRepossessionNote_DAL = (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoteDataTable)
                       ClsPubSerialize.DeSerialize(bytesObjS3G_LEGAL_RepossessionNote_DataTable, SerMode,
                       typeof(S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoteDataTable));
                    //objLRApproval_DataTable = (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_ApprovalDataTable)
                    //    ClsPubSerialize.DeSerialize(bytesObjS3G_LEGAL_RepossessionApproval_DataTable, SerMode,
                    //    typeof(S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_ApprovalDataTable));
                    /*****   DATAROW OBJECT  *****/
                    objRepossessionNoteRow = objRepossessionNote_DAL.Rows[0] as
                       S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoteRow;
                    //objLRApproval_Row = objLRApproval_DataTable.Rows[0] as
                    //    S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_ApprovalRow;
                    
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_InsertLRNote);
                    //PARAMETER USED FOR INSERT AND MODIFY 
                    db.AddInParameter(command, "@Company_id", DbType.Int32, objRepossessionNoteRow.Company_ID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, objRepossessionNoteRow.LOB_ID);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, objRepossessionNoteRow.Branch_ID);
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, objRepossessionNoteRow.Customer_ID);
                    db.AddInParameter(command, "@PANum", DbType.String, objRepossessionNoteRow.PANum);
                    db.AddInParameter(command, "@SANum", DbType.String, objRepossessionNoteRow.SANum);
                    db.AddInParameter(command, "@LRN_Date", DbType.DateTime, objRepossessionNoteRow.LRN_Date);
                    db.AddInParameter(command, "@LRN_NO", DbType.String, objRepossessionNoteRow.LRN_No);
                    db.AddInParameter(command, "@LRN_ID", DbType.Int32, objRepossessionNoteRow.LRN_ID);
                    db.AddInParameter(command, "@Action_Code", DbType.Int32, objRepossessionNoteRow.Action_Code);
                    db.AddInParameter(command, "@Action_Type_Code", DbType.Int32, objRepossessionNoteRow.Action_Type_Code);
                    db.AddInParameter(command, "@Action_Points", DbType.String, objRepossessionNoteRow.Action_Point);
                    db.AddInParameter(command, "@Followup_Employee_id", DbType.Int32, objRepossessionNoteRow.Followup_Employee_ID);
                    db.AddInParameter(command, "@LR_Status_Code", DbType.Int32, objRepossessionNoteRow.LR_Status_Code);
                    db.AddInParameter(command, "@LR_Status_Type_Code", DbType.Int32, objRepossessionNoteRow.LR_Status_Type_Code);
                    db.AddInParameter(command, "@Legal_Status_Type_Code", DbType.Int32, objRepossessionNoteRow.Legal_Status_Type_Code);
                    db.AddInParameter(command, "@Legal_Status_Code", DbType.Int32, objRepossessionNoteRow.Legal_Status_Code);
                    //Modified by Ponnurajesh on 20/Mar/2012 for oracle conversion
                    //db.AddInParameter(command, "@Repossession_Status_Type_Code", DbType.Int32, objRepossessionNoteRow.Repossession_Status_Type_Code);
                    db.AddInParameter(command, "@Repo_Status_Type_Code", DbType.Int32, objRepossessionNoteRow.Repossession_Status_Type_Code);
                    db.AddInParameter(command, "@Repossession_Status_Code", DbType.Int32, objRepossessionNoteRow.Repossession_Status_Code);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, objRepossessionNoteRow.Created_By);
                    db.AddInParameter(command, "@Created_On", DbType.DateTime, objRepossessionNoteRow.Created_On);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, objRepossessionNoteRow.Modified_By);
                    db.AddInParameter(command, "@Modified_On", DbType.DateTime, objRepossessionNoteRow.Modified_On);
                    //PARAMETERS USER FOR APPROVAL IN THE S3G_LR_APPROVAL TABLE IN MODIFY MODE.
                    db.AddInParameter(command, "@Remarks", DbType.String, objRepossessionNoteRow.Remarks);
                    db.AddInParameter(command, "@Password", DbType.String, objRepossessionNoteRow.Password);
                    db.AddInParameter(command, "@APPROVAL_Status_Code", DbType.String, objRepossessionNoteRow.Approval_Status_Code);
                    db.AddInParameter(command, "@APPROVAL_Status_Type_Code", DbType.String, objRepossessionNoteRow.Approval_Status_Type_Code);
                    db.AddInParameter(command, "@User_ID", DbType.Int32, objRepossessionNoteRow.Modified_By);
                    //db.AddInParameter(command, "@Mapped_LRN_No", DbType.String, objRepossessionNoteRow.Mapped_LRN_No);
                    //db.AddInParameter(command, "@Mapped_LRN_ID", DbType.Int32, objRepossessionNoteRow.Mapped_LRN_ID);

                    // OUT PUT PARAMETERS
                    db.AddOutParameter(command, "@DSNO", DbType.String, 100);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command);
                            if ((int)command.Parameters["@ErrorCode"].Value > 0 || (int)command.Parameters["@ErrorCode"].Value < 0)
                            {
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                //throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                            }
                            if ((int)command.Parameters["@ErrorCode"].Value == 0)
                            {
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                trans.Commit();
                                strDSNo = (string)command.Parameters["@DSNO"].Value;
                            }
                            else
                            {
                                trans.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (intRowsAffected == 0)
                                intRowsAffected = 50;
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
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
                //return Convert.ToInt32(LRReturnNo);
                return intRowsAffected;
            }
            #endregion

            #region Delete Exsisting Repossession Note
            
            public DataSet FunPubQueryLRNDetails(int? intCompany_Id, int? intLRN_id)
            {
                DataSet dsLRNDetails = new DataSet();
                // blnTranExists = false;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LR_GetLRNoteDetails");
                    if (intCompany_Id.HasValue && intCompany_Id.Value != 0)
                    {
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, intCompany_Id);
                    }
                    if (intLRN_id.HasValue && intLRN_id.Value != 0)
                    {
                        db.AddInParameter(command, "@LRN_Id", DbType.Int32, intLRN_id);
                    }
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    //db.AddOutParameter(command, "@RecordExists", DbType.Boolean, sizeof(Boolean));
                    dsLRNDetails = db.FunPubExecuteDataSet(command);
                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        throw new Exception("Error in Getting Entity details");
                    //blnTranExists = (bool)command.Parameters["@RecordExists"].Value;
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }

                return dsLRNDetails;
            }
            
            #endregion

            public DataTable FunPubGetMappedLRNDetails(int intCompanyId, int intLobId, int intBranchId, string strPANum, string strSANum)
            {
                DataSet ObjDS = new DataSet();
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_GetMappedLRNDetails);
                db.AddInParameter(command, "@COMPANY_ID", DbType.Int32, intCompanyId);
                db.AddInParameter(command, "@LOB_ID", DbType.Int32, intLobId);
                db.AddInParameter(command, "@Location_ID", DbType.Int32, intBranchId);
                db.AddInParameter(command, "@PANum", DbType.String, strPANum);
                if (strSANum != string.Empty)
                {
                    db.AddInParameter(command, "@SANUM", DbType.String, strSANum);
                }

                db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                return (DataTable)ObjDS.Tables["dtTable"];
            }
            public DataTable FunPubGetMappedPASADetails(int intCompanyId, int intLobId, int intBranchId, string strPANum, string strSANum)
            {
                DataSet ObjDS = new DataSet();
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_GetMappedPASADetails);
                db.AddInParameter(command, "@COMPANY_ID", DbType.Int32, intCompanyId);
                db.AddInParameter(command, "@LOB_ID", DbType.Int32, intLobId);
                db.AddInParameter(command, "@Location_ID", DbType.Int32, intBranchId);
                db.AddInParameter(command, "@PANum", DbType.String, strPANum);
                if (strSANum != string.Empty)
                {
                    db.AddInParameter(command, "@SANUM", DbType.String, strSANum);
                }

                db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                return (DataTable)ObjDS.Tables["dtTable"];
            }

            public int FunPubGetMappedAccountAssetDetails(int intCompanyId, int intLobId, int intBranchId, string strPANum, string strSANum)
            {
                DataSet ObjDS = new DataSet();
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_GetMappedAccountAssetDetails);
                db.AddInParameter(command, "@COMPANY_ID", DbType.Int32, intCompanyId);
                db.AddInParameter(command, "@LOB_ID", DbType.Int32, intLobId);
                db.AddInParameter(command, "@Location_ID", DbType.Int32, intBranchId);
                db.AddInParameter(command, "@PANum", DbType.String, strPANum);
                if (strSANum != string.Empty)
                {
                    db.AddInParameter(command, "@SANUM", DbType.String, strSANum);
                }
                //int assetCount = (int)db.FunpubExecuteScalar(command);//Modified by Ponnurajesh on 20/Mar/2012 for oracle conversion
                int assetCount = Convert.ToInt32(db.FunPubExecuteScalar(command).ToString());
                return assetCount;
            }

            /// <summary>
            /// To get PANum realted customer details
            /// </summary>
            /// <param name="strPANum"></param>
            /// <returns></returns>
            public DataTable FunGetPANumCustomer(string strPANum)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_GetPANumCustomer);
                    db.AddInParameter(command, "@PANum", DbType.String, strPANum);
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    return (DataTable)ObjDS.Tables["dtTable"];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
