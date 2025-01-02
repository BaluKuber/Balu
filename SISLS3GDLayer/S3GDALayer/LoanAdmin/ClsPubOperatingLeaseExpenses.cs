#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: LoanAdmin
/// Screen Name			: Operating Lease Expenses DAL class
/// Created By			: Irsathameen K
/// Created Date		: 02-09-2010
/// Purpose	            : To access Operating Lease Asset  db methods

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
using System.Data.OracleClient;
using S3GBusEntity;

namespace S3GDALayer.LoanAdmin
{
     namespace LoanAdminAccMgtServices
    {
      public class ClsPubOperatingLeaseExpenses
        {
            int intRowsAffected;    
            S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_OperatingLeaseExpensesDetailsDataTable ObjOperatingLeaseExpenses_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubOperatingLeaseExpenses()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateOperatingLeaseExpensesDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_OperatingLeaseExpensesDataTable, out string strOLENo, out string strErrMsg)
            {
                try
                {
                    strErrMsg = string.Empty;
                    strOLENo = "";
                    ObjOperatingLeaseExpenses_DAL = (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_OperatingLeaseExpensesDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_OperatingLeaseExpensesDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_OperatingLeaseExpensesDetailsDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_OperatingLeaseExpensesDetailsRow ObjOperatingLeaseExpensesRow in ObjOperatingLeaseExpenses_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertOperatingLeaseExpenses");                        
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjOperatingLeaseExpensesRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjOperatingLeaseExpensesRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjOperatingLeaseExpensesRow.Branch_ID);
                        if (ObjOperatingLeaseExpensesRow.Customer_ID!=0)
                        { db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjOperatingLeaseExpensesRow.Customer_ID);  }
                        //db.AddInParameter(command, "@SJV_No", DbType.String, ObjOperatingLeaseExpensesRow.SJV_No);
                        //db.AddInParameter(command, "@SJV_Date", DbType.DateTime, ObjOperatingLeaseExpensesRow.SJV_Date);
                        db.AddInParameter(command, "@Lease_Asset_No", DbType.String, ObjOperatingLeaseExpensesRow.Lease_Asset_No);
                        db.AddInParameter(command, "@OL_Expenses_Date", DbType.DateTime, ObjOperatingLeaseExpensesRow.Operation_Lease_Expenses_Date);
                        db.AddInParameter(command, "@Debit_Type_Code", DbType.Int32, ObjOperatingLeaseExpensesRow.Debit_Type_Code);
                        db.AddInParameter(command, "@Debit_Type", DbType.Int32, ObjOperatingLeaseExpensesRow.Debit_Type);
                        db.AddInParameter(command, "@Expenditure_Type_Code", DbType.Int32, ObjOperatingLeaseExpensesRow.Expenditure_Type_Code);
                        db.AddInParameter(command, "@Expenditure_Type", DbType.Int32, ObjOperatingLeaseExpensesRow.Expenditure_Type);
                        db.AddInParameter(command, "@Bill_No", DbType.String, ObjOperatingLeaseExpensesRow.Bill_No);
                        db.AddInParameter(command, "@Bill_Date", DbType.DateTime, ObjOperatingLeaseExpensesRow.Bill_Date);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjOperatingLeaseExpensesRow.Created_By);
                        //db.AddInParameter(command, "@XMLOLEDetails", DbType.String, ObjOperatingLeaseExpensesRow.XMLOLEDetails);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XMLOLEDetails", OracleType.Clob,
                                ObjOperatingLeaseExpensesRow.XMLOLEDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjOperatingLeaseExpensesRow.XMLOLEDetails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLOLEDetails", DbType.String,
                                ObjOperatingLeaseExpensesRow.XMLOLEDetails);
                        }
                        db.AddOutParameter(command,"@Operation_Lease_Expenses_No", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);//Journal
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
				  db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
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
                                    trans.Commit();
                                    strOLENo = (string)command.Parameters["@Operation_Lease_Expenses_No"].Value;
                                }
                            }
                            catch (Exception ex)
                            {
                                // To identify if journal entry is failed
                                if (command.Parameters["@ErrorCode"].Value != null)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                                }
                                else if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                    strErrMsg = ex.Message;
                                }
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();
                            }
                            finally
                            { conn.Close(); }
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
