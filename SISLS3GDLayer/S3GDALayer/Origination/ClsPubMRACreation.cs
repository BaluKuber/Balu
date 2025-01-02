using System;
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
using S3GDALayer.S3GAdminServices;

namespace S3GDALayer.Origination
{
    namespace OrgMasterMgtServices
    {
        public class ClsPubMRACreation
        {
            #region Declaration
            int intRowsAffected;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_MRADataTable ObjMRA_DataTable = new S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_MRADataTable();
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_MRAAPPROVALDataTable ObjMRAApproval_DataTable = new S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_MRAAPPROVALDataTable();


            Database db;
            public ClsPubMRACreation()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region "Create Mode"

            public int FunPubCreateOrModifyMRACreation(SerializationMode SerMode, byte[] bytesObjMRA_DataTable, out string strMRANumber, out int intMRA_No)
            {
                strMRANumber = "";
                intMRA_No = 0;
                try
                {
                    ObjMRA_DataTable = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_MRADataTable)ClsPubSerialize.DeSerialize(bytesObjMRA_DataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_MRADataTable));
                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_MRARow ObjMRARow in ObjMRA_DataTable.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_INSERT_MRA");

                        db.AddInParameter(command, "@MRA_ID", DbType.Int64, ObjMRARow.MRA_ID);
                        db.AddInParameter(command, "@MRA_Number", DbType.String, ObjMRARow.MRA_Number);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int64, ObjMRARow.Customer_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjMRARow.Company_ID);
                        db.AddInParameter(command, "@MRA_Creation_Date", DbType.DateTime, ObjMRARow.MRA_Creation_Date);
                        if (!ObjMRARow.IsMRA_Effective_DateNull())
                            db.AddInParameter(command, "@MRA_Effective_Date", DbType.DateTime, ObjMRARow.MRA_Effective_Date);
                        db.AddInParameter(command, "@STD_Format", DbType.Int32, ObjMRARow.Standard_Format);
                        db.AddInParameter(command, "@Signed_Received", DbType.Int32, ObjMRARow.SignedReceived);
                        db.AddInParameter(command, "@Signatory_Name", DbType.String, ObjMRARow.Signatory_Name);
                        db.AddInParameter(command, "@Signatory_Designation", DbType.String, ObjMRARow.Signatory_Designation);
                        db.AddInParameter(command, "@Signatory_Contact_No", DbType.String, ObjMRARow.Signatory_Number);
                        db.AddInParameter(command, "@Signatory_Mail", DbType.String, ObjMRARow.Signatory_Email);
                        db.AddInParameter(command, "@Authorization_Basis", DbType.Int32, ObjMRARow.Authorization_Basis);
                        if (!ObjMRARow.IsAuthorization_DateNull())
                            db.AddInParameter(command, "@Authorization_Date", DbType.DateTime, ObjMRARow.Authorization_Date);
                        db.AddInParameter(command, "@Board_Resolution", DbType.Int32, ObjMRARow.Board_Resolution);
                        if (!ObjMRARow.IsBoard_Resolution_DateNull())
                            db.AddInParameter(command, "@Board_Resolution_Date", DbType.DateTime, ObjMRARow.Board_Resolution_Date);
                        db.AddInParameter(command, "@Account_Manager_1", DbType.Int32, ObjMRARow.Account_Manager_1);
                        db.AddInParameter(command, "@Account_Manager_2", DbType.Int32, ObjMRARow.Account_Manager_2);
                        db.AddInParameter(command, "@Regional_Manager", DbType.Int32, ObjMRARow.Regional_Manager);
                        db.AddInParameter(command, "@OPC_Notice", DbType.Int32, ObjMRARow.OPC_Notice);
                        db.AddInParameter(command, "@OPC_to_Customer", DbType.Int32, ObjMRARow.OPC_to_Customer);
                        db.AddInParameter(command, "@Customer_to_OPC", DbType.Int32, ObjMRARow.Customer_to_OPC);
                        db.AddInParameter(command, "@Auto_Extension_rental", DbType.Int32, ObjMRARow.Auto_Extension_rental);
                        db.AddInParameter(command, "@Auto_Extension_term", DbType.Int32, ObjMRARow.Auto_Extension_term);
                        db.AddInParameter(command, "@Auto_Extension_Conditions", DbType.String, ObjMRARow.Auto_Extension_Conditions);
                        db.AddInParameter(command, "@Interim_Rent_Applicable", DbType.Int32, ObjMRARow.Interim_Rent_Applicable);
                        db.AddInParameter(command, "@Interim_Rent_Basis", DbType.Int32, ObjMRARow.Interim_Rent_Basis);
                        db.AddInParameter(command, "@Insurance_Conditions", DbType.String, ObjMRARow.Insurance_Conditions);
                        db.AddInParameter(command, "@Termination_Conditions", DbType.String, ObjMRARow.Termination_Conditions);
                        db.AddInParameter(command, "@Customer_Notice_Period", DbType.Int32, ObjMRARow.Customer_Notice_Period);
                        db.AddInParameter(command, "@OPC_Notice_Period", DbType.Int32, ObjMRARow.OPC_Notice_Period);
                        db.AddInParameter(command, "@Foreclosure_Rate", DbType.Decimal, ObjMRARow.Foreclosure_Rate);
                        db.AddInParameter(command, "@Break_Cost", DbType.Decimal, ObjMRARow.Break_Cost);
                        db.AddInParameter(command, "@Overdue_Rate", DbType.Decimal, ObjMRARow.Overdue_Rate);
                        db.AddInParameter(command, "@Arbitration_Clause", DbType.Int32, ObjMRARow.Arbitration_Clause);
                        if (!ObjMRARow.IsClause_NumberNull())
                            db.AddInParameter(command, "@Clause_Number", DbType.String, ObjMRARow.Clause_Number);
                        db.AddInParameter(command, "@MRA_Amended", DbType.Int32, ObjMRARow.MRA_Amended);
                        if (!ObjMRARow.IsAmendment_DateNull())
                            db.AddInParameter(command, "@Amendment_Date", DbType.DateTime, ObjMRARow.Amendment_Date);
                        db.AddInParameter(command, "@Remarks", DbType.String, ObjMRARow.Remarks);
                        db.AddInParameter(command, "@MRA_Status", DbType.Int32, ObjMRARow.MRA_Status);
                        db.AddInParameter(command, "@USRID", DbType.Int32, ObjMRARow.Created_By);
                        db.AddInParameter(command, "@TXNDT", DbType.DateTime, ObjMRARow.Created_On);
                        db.AddInParameter(command, "@InterimRentRate", DbType.Double, ObjMRARow.InterimRentRate);

                        db.AddInParameter(command, "@City", DbType.String, ObjMRARow.City);
                        db.AddInParameter(command, "@Jurisdiction", DbType.Int32, ObjMRARow.Jurisdictaion);
                        db.AddInParameter(command, "@Lessee_Notice", DbType.Int32, ObjMRARow.Lessee_Give_Notice);

                        db.AddInParameter(command, "@Invoice_Grace_Type", DbType.Int32, ObjMRARow.Invoice_Grace_Period_Type);
                        db.AddInParameter(command, "@Invoice_Grace_Period", DbType.Int32, ObjMRARow.Invoice_Grace_Period);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@MRANumber", DbType.String, 50);
                        db.AddOutParameter(command, "@MRA_ID_OUT", DbType.Int32, sizeof(Int32));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                // db.ExecuteNonQuery(command, trans);

                                db.FunPubExecuteNonQuery(command, ref trans);

                                // if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                strMRANumber = Convert.ToString(command.Parameters["@MRANumber"].Value);
                                intMRA_No = (int)command.Parameters["@MRA_ID_OUT"].Value;
                                //if (intRowsAffected == 0)
                                trans.Commit();
                                //else
                                //    trans.Rollback();

                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
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
                catch (Exception objException)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(objException);
                }
                return intRowsAffected;
            }

            public int FunPubInsertMRAApproval(SerializationMode SerMode, byte[] bytesObjMRAApproval_DataTable, out string strMRAApprovalNumber, out Int64 intMRAApproval_No)
            {
                strMRAApprovalNumber = "";
                intMRAApproval_No = 0;
                try
                {
                    ObjMRAApproval_DataTable = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_MRAAPPROVALDataTable)ClsPubSerialize.DeSerialize(bytesObjMRAApproval_DataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_MRAAPPROVALDataTable));
                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_MRAAPPROVALRow ObjMRAApprovalRow in ObjMRAApproval_DataTable.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_INSERT_MRAAPPROVAL");

                        db.AddInParameter(command, "@MRAApproval_ID", DbType.Int32, ObjMRAApprovalRow.MRAApproval_ID);
                        db.AddInParameter(command, "@MRA_ID", DbType.Int32, ObjMRAApprovalRow.MRA_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjMRAApprovalRow.Company_ID);
                        db.AddInParameter(command, "@Approver_ID", DbType.Int32, ObjMRAApprovalRow.Approver_ID);
                        db.AddInParameter(command, "@Approval_Date", DbType.DateTime, ObjMRAApprovalRow.Approval_Date);
                        db.AddInParameter(command, "@Remarks", DbType.String, ObjMRAApprovalRow.Remarks);
                        db.AddInParameter(command, "@Approval_Serial_Number", DbType.Int32, ObjMRAApprovalRow.Approval_Serial_Number);
                        db.AddInParameter(command, "@Action_ID", DbType.Int32, ObjMRAApprovalRow.Action_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjMRAApprovalRow.Created_By);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjMRAApprovalRow.LOB_ID);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@MRAApprovalNumber", DbType.String, 50);
                        db.AddOutParameter(command, "@MRAApproval_ID_OUT", DbType.Int64, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                // db.ExecuteNonQuery(command, trans);

                                db.FunPubExecuteNonQuery(command, ref trans);

                                // if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                strMRAApprovalNumber = Convert.ToString(command.Parameters["@MRAApprovalNumber"].Value);
                                intMRAApproval_No = (Int64)command.Parameters["@MRAApproval_ID_OUT"].Value;
                                //if (intRowsAffected == 0)
                                trans.Commit();
                                //else
                                //    trans.Rollback();

                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
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
                catch (Exception objException)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(objException);
                }
                return intRowsAffected;
            }

            #endregion

        }
    }
}
