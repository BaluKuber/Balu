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
    namespace ContractMgtServices
    {
        public class ClsPubPreMatureClosure
        {
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDataTable objAccountClosure_DAL;
            S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDetailsDataTable objClosureDetails_DAL;
            S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureRow objAccountClosureRow;
            S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDetailsRow objClosureDetailsRow;
            S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_CancelClosureDataTable objCancelClaosure_DAL;
            S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_CancelClosureRow objClosureRow;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubPreMatureClosure()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            /// <summary>
            /// To get PANum , SANum Related Account Details
            /// </summary>
            /// <param name="Asset_Verification_No">Pass PAN, SAN</param>
            /// <returns></returns>

            public DataSet FunGetAccountDetailsForClosure(string strPANum, string strSANum)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    string[] strTables = { "dtTable", "dtTable1" };

                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetAccountDetailsForClosure");
                    db.AddInParameter(command, "@PANum", DbType.String, strPANum);
                    db.AddInParameter(command, "@SANum", DbType.String, strSANum);
                    db.FunPubLoadDataSet(command, ObjDS, "strTables");
                    return (DataSet)ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            /// <summary>
            /// Inserting the Account Closure
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns>

            public int FunPubCreateAccountClosure(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_AccountClosure_DataTable, out string strAccClosureNo, out int intClosureDetailId)
            {
                strAccClosureNo = "";
                intClosureDetailId = 0;
                try
                {

                    objAccountClosure_DAL = (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_AccountClosure_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDataTable));
                    //objClosureDetails_DAL = (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_AccountClosure_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDetailsDataTable));


                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureRow objAccountClosureRow in objAccountClosure_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertPreMatureClosure");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objAccountClosureRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objAccountClosureRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objAccountClosureRow.Branch_ID);
                        db.AddInParameter(command, "@Closure_No", DbType.String, objAccountClosureRow.Closure_No);
                        db.AddInParameter(command, "@PANum", DbType.String, objAccountClosureRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, objAccountClosureRow.SANum);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, objAccountClosureRow.Customer_ID);
                        db.AddInParameter(command, "@Closure_Date", DbType.DateTime, objAccountClosureRow.Closure_Date);
                        db.AddInParameter(command, "@CreatedBy", DbType.Int32, objAccountClosureRow.Created_By);
                        db.AddInParameter(command, "@ClsoureAmount", DbType.Decimal, objAccountClosureRow.Closure_Amount);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, objAccountClosureRow.User_ID);
                        db.AddInParameter(command, "@Tranche_ID", DbType.Int32, objAccountClosureRow.Tranche_id);
                        db.AddInParameter(command, "@Fund_closure_date", DbType.DateTime, objAccountClosureRow.Fund_Closure_Date);
                        db.AddInParameter(command, "@OPC_Closure_Rate", DbType.Decimal, objAccountClosureRow.OPC_Closure_Rate);
                        db.AddInParameter(command, "@OPC_Breaking_Rate", DbType.Decimal, objAccountClosureRow.OPC_Breaking_Rate);
                        db.AddInParameter(command, "@Fund_Closure_Rate", DbType.Decimal, objAccountClosureRow.Fund_Closure_Rate);
                        db.AddInParameter(command, "@Fund_Breaking_Rate", DbType.Decimal, objAccountClosureRow.Fund_Breaking_Rate);
                        db.AddInParameter(command, "@IS_AMF", DbType.String, objAccountClosureRow.IS_AMF);
                        db.AddInParameter(command, "@IS_VAT", DbType.String, objAccountClosureRow.IS_VAT);
                        db.AddInParameter(command, "@IS_ST", DbType.String, objAccountClosureRow.IS_ST);
                        db.AddInParameter(command, "@NOC_date", DbType.DateTime, objAccountClosureRow.NOC_date);
                        db.AddInParameter(command, "@fund_Xml", DbType.String, objAccountClosureRow.fund_Xml);
                        db.AddInParameter(command, "@XMLParamAccountClosureDet", DbType.String, objAccountClosureRow.XMLAccountClosureDetails);
                        db.AddInParameter(command, "@IS_WAIV", DbType.String, objAccountClosureRow.IS_WAIV);
                        //Added By Chandru K On 24-Sep-2013 For ISFC
                        //db.AddInParameter(command, "@WriteOff", DbType.Boolean, objAccountClosureRow.WriteOff);
                        //db.AddInParameter(command, "@WriteOff_Amount", DbType.Decimal, objAccountClosureRow.WriteOff_Amount);
                        //db.AddInParameter(command, "@Remarks", DbType.String, objAccountClosureRow.Remarks);
                        //End
                        db.AddOutParameter(command, "@DSNO", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@Closure_Details_ID", DbType.Int32, sizeof(Int64));

                        command.CommandTimeout = 180;
                        db.FunPubExecuteNonQuery(command);

                        strAccClosureNo = (string)command.Parameters["@DSNO"].Value;
                        intClosureDetailId = (Int32)command.Parameters["@Closure_Details_ID"].Value;
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


            /// <summary>
            /// Inserting the Account Closure
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns>

            public int FunPubCreateAccountClosureDetails(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_AccountClosure_DataTable)
            {
                try
                {

                    objClosureDetails_DAL = (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_AccountClosure_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDetailsDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDetailsRow objAccountClosureRow in objClosureDetails_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertPreMatureClosureDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objAccountClosureRow.Company_ID);
                        db.AddInParameter(command, "@PANum", DbType.String, objAccountClosureRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, objAccountClosureRow.SANum);
                        db.AddInParameter(command, "@Closure_Type_Code", DbType.Int32, objAccountClosureRow.Closure_Type_Code);
                        db.AddInParameter(command, "@Closure_Type", DbType.Int32, objAccountClosureRow.Closure_Type);
                        db.AddInParameter(command, "@Cashflow", DbType.Decimal, objAccountClosureRow.Cashflow_Component);
                        db.AddInParameter(command, "@ClosureRate", DbType.Decimal, objAccountClosureRow.Closure_Rate);
                        db.AddInParameter(command, "@PayableAmt", DbType.Decimal, objAccountClosureRow.Payable_Amount);
                        db.AddInParameter(command, "@ReceivedAmt", DbType.Decimal, objAccountClosureRow.Received_Amount);
                        db.AddInParameter(command, "@ClosureStatusType", DbType.Int32, objAccountClosureRow.Closure_Status_Type_Code);
                        db.AddInParameter(command, "@ClosureStatusCode", DbType.Int32, objAccountClosureRow.Closure_Status_Code);
                        db.AddInParameter(command, "@WaivedAmt", DbType.Decimal, objAccountClosureRow.Waived_Amount);
                        db.AddInParameter(command, "@ClosureAmt", DbType.Decimal, objAccountClosureRow.Closure_Amount);
                        db.AddInParameter(command, "@Closure_Date", DbType.DateTime, objAccountClosureRow.Closure_Date);
                        db.AddInParameter(command, "@Remarks", DbType.String, objAccountClosureRow.Remarks);
                        db.AddInParameter(command, "@Closure_ID", DbType.String, objAccountClosureRow.Closure_ID);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

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

            /// <summary>
            /// To Cancel the account closure 
            /// </summary>
            /// <param name="Asset_Verification_No">Pass vendor ID</param>
            /// <returns></returns>

            public int FunPubCancelAccountClosure(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_CancelClosure_DataTable)
            {
                try
                {

                    objCancelClaosure_DAL = (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_CancelClosureDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_CancelClosure_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_CancelClosureDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_CancelClosureRow objAccountClosureRow in objCancelClaosure_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_CancelClosure");
                        db.AddInParameter(command, "@ClosureNo", DbType.String, objAccountClosureRow.Closure_No);
                        db.AddInParameter(command, "@PANum", DbType.String, objAccountClosureRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, objAccountClosureRow.SANum);
                        db.AddInParameter(command, "@Closure_Type", DbType.String, objAccountClosureRow.Closure_Type);
                        db.AddInParameter(command, "@Company_ID", DbType.String, objAccountClosureRow.Company_ID);
                        db.AddInParameter(command, "@User_Id", DbType.Int32, objAccountClosureRow.User_Id);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

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
        }

    }
}
