#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: LoanAdmin
/// Screen Name			: SpecificationRevision DAL Class
/// Created By			: S.Kannan
/// Created Date		: 01-SEP-2010
/// Purpose	            : DAL Class for Account Specification Methods
/// <Program Summary>
#endregion

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
using System.Data.OracleClient;


namespace S3GDALayer.LoanAdmin
{
    namespace ContractMgtServices
    {
        public class ClsPubSpecificationRevision
        {


            #region Declaration
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_SpecificRevisionDataTable ObjSpecificationRevision_DataTable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_SpecificRevisionDataTable();
            S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_RepayDetailsIRRArchiveHeaderDataTable ObjRepayDetailsIRRArchiveHeader_DataTable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_RepayDetailsIRRArchiveHeaderDataTable();
            #endregion

            Database db;
            public ClsPubSpecificationRevision()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #region Create Mode+-
            // this is to insert the  - SQL table
            public int FunPubCreateOrModifySpecificationRevision(SerializationMode SerMode, byte[] bytesObjSpecificationRevision_DataTable, ClsSystemJournal ObjSysJournal, out string strRevisionNumber)
            {
                strRevisionNumber = string.Empty;
                try
                {
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                    ObjSpecificationRevision_DataTable = (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_SpecificRevisionDataTable)ClsPubSerialize.DeSerialize(bytesObjSpecificationRevision_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_SpecificRevisionDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertSpecificationRevision");
                    foreach (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_SpecificRevisionRow ObjSpecificationRevisionRow in ObjSpecificationRevision_DataTable.Rows)
                    {
                        if (ObjSpecificationRevisionRow.Company_ID != -1)
                            db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjSpecificationRevisionRow.Company_ID);
                        if (ObjSpecificationRevisionRow.LOB_ID != -1)
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjSpecificationRevisionRow.LOB_ID);
                        if (ObjSpecificationRevisionRow.Branch_ID != -1)
                        {
                            //   db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjSpecificationRevisionRow.Branch_ID);
                            db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjSpecificationRevisionRow.Branch_ID);
                        }
                        db.AddInParameter(command, "@PANum", DbType.String, ObjSpecificationRevisionRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, ObjSpecificationRevisionRow.SANum);
                        db.AddInParameter(command, "@Account_Revision_Number", DbType.String, ObjSpecificationRevisionRow.Account_Revision_Number);
                        db.AddInParameter(command, "@Account_Revision_Date", DbType.DateTime, ObjSpecificationRevisionRow.Account_Revision_Date);
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            db.AddInParameter(command, "@Acc_Rev_Effective_Date", DbType.DateTime, ObjSpecificationRevisionRow.Account_Revision_Effective_Date);
                        }
                        else
                        {
                            db.AddInParameter(command, "@Account_Revision_Effective_Date", DbType.DateTime, ObjSpecificationRevisionRow.Account_Revision_Effective_Date);
                        }
                        db.AddInParameter(command, "@Revised_Finance_Amount", DbType.Decimal, ObjSpecificationRevisionRow.Revised_Finance_Amount);
                        db.AddInParameter(command, "@Revised_Rate", DbType.Decimal, ObjSpecificationRevisionRow.Revised_Rate);
                        db.AddInParameter(command, "@Revised_Tenure", DbType.Decimal, ObjSpecificationRevisionRow.Revised_Tenure);
                        db.AddInParameter(command, "@Accounting_IRR", DbType.Decimal, ObjSpecificationRevisionRow.Accounting_IRR);
                        db.AddInParameter(command, "@Business_IRR", DbType.Decimal, ObjSpecificationRevisionRow.Business_IRR);
                        db.AddInParameter(command, "@Company_IRR", DbType.Decimal, ObjSpecificationRevisionRow.Company_IRR);
                        db.AddInParameter(command, "@Revision_Status_Type_Code", DbType.Decimal, ObjSpecificationRevisionRow.Revision_Status_Type_Code);
                        db.AddInParameter(command, "@Revision_Status_Code", DbType.Decimal, ObjSpecificationRevisionRow.Revision_Status_Code);
                        if (!ObjSpecificationRevisionRow.IsResidual_ValueNull())
                            db.AddInParameter(command, "@Residual_Value", DbType.Decimal, ObjSpecificationRevisionRow.Residual_Value);
                        if (!ObjSpecificationRevisionRow.IsResidual_AmountNull())
                            db.AddInParameter(command, "@Residual_Amount", DbType.Decimal, ObjSpecificationRevisionRow.Residual_Amount);
                        if (!ObjSpecificationRevisionRow.IsRevision_FeeNull())
                            db.AddInParameter(command, "@Revision_Fee", DbType.Decimal, ObjSpecificationRevisionRow.Revision_Fee);

                        if (ObjSpecificationRevisionRow.Created_By != -1)
                            db.AddInParameter(command, "@Created_By", DbType.Int32, ObjSpecificationRevisionRow.Created_By);
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param;
                            param = new OracleParameter("@RepaymentStructure",
                                 OracleType.Clob, ObjSpecificationRevisionRow.RepaymentStructure.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, ObjSpecificationRevisionRow.RepaymentStructure);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@RepaymentStructure", DbType.String, ObjSpecificationRevisionRow.RepaymentStructure);
                        }
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param;
                            param = new OracleParameter("@PostingEntries",
                                 OracleType.Clob, ObjSpecificationRevisionRow.PostingEntries.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, ObjSpecificationRevisionRow.PostingEntries);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@PostingEntries", DbType.String, ObjSpecificationRevisionRow.PostingEntries);
                        }
                        db.AddInParameter(command, "@Txn_ID", DbType.Int32, 1);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@RevisionDocumentNumber", DbType.String, 50);
                        //db.AddInParameter(command, "@Account_Link_Key", DbType.Decimal, ObjSpecificationRevisionRow.Account_Link_Key);
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                if (intRowsAffected > 0)
                                    trans.Commit();
                                else
                                    trans.Rollback();

                                if ((!(string.IsNullOrEmpty(command.Parameters["@RevisionDocumentNumber"].Value.ToString()))))
                                    strRevisionNumber = (string)command.Parameters["@RevisionDocumentNumber"].Value;

                            }
                            catch (Exception ex)
                            {
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();
                            }
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


            // S3G_LOANAD_InsertSpecificationRevisionMasterDetails
            public int FunPubCreateSpecificationRevisionMasterDetails(SerializationMode SerMode, byte[] bytesObjRepayDetailsIRRArchiveHeader_DataTable, out string RevisionDocumentNumber)
            {
                RevisionDocumentNumber = string.Empty;
                try
                {
                    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_InsertSpecificationRevisionMasterDetailsDataTable ObjRepayDetailsIRRArchiveHeader_DataTable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_InsertSpecificationRevisionMasterDetailsDataTable();

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertSpecificationRevisionMasterDetails");
                    foreach (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_InsertSpecificationRevisionMasterDetailsRow ObjSpecificationRevisionArcheiveRow in ObjRepayDetailsIRRArchiveHeader_DataTable.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjSpecificationRevisionArcheiveRow.LOB_ID);
                        if (Convert.ToInt32(ObjSpecificationRevisionArcheiveRow.Branch_ID) != 0)
                        {
                            //db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Branch_ID);
                            db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Branch_ID);
                        }
                        db.AddInParameter(command, "@PANum", DbType.String, ObjSpecificationRevisionArcheiveRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, ObjSpecificationRevisionArcheiveRow.SANum);
                        db.AddInParameter(command, "@Account_Revision_Number", DbType.String, ObjSpecificationRevisionArcheiveRow.Account_Revision_Number);
                        db.AddInParameter(command, "@Account_Revision_Date", DbType.DateTime, ObjSpecificationRevisionArcheiveRow.Account_Revision_Date);
                        db.AddInParameter(command, "@Account_Revision_Effective_Date", DbType.DateTime, ObjSpecificationRevisionArcheiveRow.Account_Revision_Effective_Date);
                        db.AddInParameter(command, "@Revised_Finance_Amount", DbType.Decimal, ObjSpecificationRevisionArcheiveRow.Revised_Finance_Amount);
                        db.AddInParameter(command, "@Revised_Rate", DbType.Decimal, ObjSpecificationRevisionArcheiveRow.Revised_Rate);
                        db.AddInParameter(command, "@Revised_Tenure", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Revised_Tenure);
                        db.AddInParameter(command, "@Revision_Status_Type_Code", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Revised_Tenure);
                        db.AddInParameter(command, "@Revision_Status_Code", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Revision_Status_Code);
                        if (ObjSpecificationRevisionArcheiveRow.Created_By != -1)
                            db.AddInParameter(command, "@Created_By", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjSpecificationRevisionArcheiveRow.Created_On);
                        if (ObjSpecificationRevisionArcheiveRow.Modified_By != -1)
                            db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Modified_By);
                        db.AddInParameter(command, "@Modified_Date", DbType.DateTime, ObjSpecificationRevisionArcheiveRow.Modified_Date);
                        db.AddInParameter(command, "@Txn_ID", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Txn_ID);
                        db.AddInParameter(command, "@XMLArchDetailsFromUI", DbType.String, ObjSpecificationRevisionArcheiveRow.XMLArchDetailsFromUI);
                        db.AddInParameter(command, "@XMLArchDetailsToArchive ", DbType.String, ObjSpecificationRevisionArcheiveRow.XMLArchDetailsToArchive);
                        db.AddInParameter(command, "@Revision_Type_Code", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Revision_Type_Code);
                        db.AddInParameter(command, "@Revision_Type", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Revision_Type);
                        db.AddInParameter(command, "@Repay_ID", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Repay_ID);


                        db.AddOutParameter(command, "@RevisionDocumentNumber", DbType.String, 50);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                intRowsAffected = 50;
                                trans.Rollback();
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            }
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

            // this is to insert the Archive details.
            public int FunPubCreateOrModifySpecificationArchive(SerializationMode SerMode, byte[] bytesObjRepayDetailsIRRArchiveHeader_DataTable)
            {
                try
                {
                    ObjRepayDetailsIRRArchiveHeader_DataTable = (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_RepayDetailsIRRArchiveHeaderDataTable)ClsPubSerialize.DeSerialize(bytesObjRepayDetailsIRRArchiveHeader_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_RepayDetailsIRRArchiveHeaderDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertArchiveDetails");
                    foreach (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_RepayDetailsIRRArchiveHeaderRow ObjSpecificationRevisionArcheiveRow in ObjRepayDetailsIRRArchiveHeader_DataTable.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjSpecificationRevisionArcheiveRow.LOB_ID);
                        //db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Branch_ID);
                        if (ObjSpecificationRevisionArcheiveRow.Revision_ID != 0)
                            db.AddInParameter(command, "@Revision_ID", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Revision_ID);
                        db.AddInParameter(command, "@Revision_Type_Code", DbType.String, ObjSpecificationRevisionArcheiveRow.Revision_Type_Code);

                        db.AddInParameter(command, "@Revision_Type", DbType.String, ObjSpecificationRevisionArcheiveRow.Revision_Type);
                        db.AddInParameter(command, "@PANum", DbType.String, ObjSpecificationRevisionArcheiveRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, ObjSpecificationRevisionArcheiveRow.SANum);
                        db.AddInParameter(command, "@Repay_ID", DbType.Int32, ObjSpecificationRevisionArcheiveRow.Repay_ID);
                        db.AddInParameter(command, "@Business_IRR", DbType.Decimal, ObjSpecificationRevisionArcheiveRow.Business_IRR);
                        db.AddInParameter(command, "@Company_IRR", DbType.Decimal, ObjSpecificationRevisionArcheiveRow.Company_IRR);
                        db.AddInParameter(command, "@Accounting_IRR", DbType.Decimal, ObjSpecificationRevisionArcheiveRow.Accounting_IRR);
                        db.AddInParameter(command, "@Archive_Date", DbType.DateTime, ObjSpecificationRevisionArcheiveRow.Archive_Date);

                        db.AddInParameter(command, "@XMLArchDetails", DbType.String, ObjSpecificationRevisionArcheiveRow.XMLArchDetails);



                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {

                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
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


            #endregion

        }
    }
}
