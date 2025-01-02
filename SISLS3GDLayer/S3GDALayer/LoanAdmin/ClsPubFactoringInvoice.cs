#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: LoanAdmin
/// Screen Name			: Factoring Invoice Loading DAL class
/// Created By			: Irsathameen K
/// Created Date		: 19-Jul-2010
/// Purpose	            : To access Factoring Invoice  db methods

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
    namespace LoanAdminMgtServices
    {

      public class ClsPubFactoringInvoice
        {
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_FactoringInvoiceDetailsDataTable ObjFactoringInvoice_DAL;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_FT_RetirementDataTable ObjFactoringRetirement_DAL;
          
          
            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubFactoringInvoice()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            public int FunPubCreateFactoringInvoiceDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FactoringInvoiceDataTable, out string strFILNo,out string strInvoiceNo,out string strPartyName)
            {
                try
                {
                    strFILNo = "";
                    strInvoiceNo = "";
                    strPartyName = "";
                    ObjFactoringInvoice_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_FactoringInvoiceDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_FactoringInvoiceDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_FactoringInvoiceDetailsDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_FactoringInvoiceDetailsRow ObjFactoringInvoiceRow in ObjFactoringInvoice_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertFactoringInvoice");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjFactoringInvoiceRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjFactoringInvoiceRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjFactoringInvoiceRow.Branch_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjFactoringInvoiceRow.Customer_ID);
                        db.AddInParameter(command, "@PANum", DbType.String, ObjFactoringInvoiceRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, ObjFactoringInvoiceRow.SANum);                        
                        db.AddInParameter(command, "@Fil_Date", DbType.DateTime, ObjFactoringInvoiceRow.Fil_Date);                        
                        db.AddInParameter(command, "@Invoice_Load_Value", DbType.Decimal, ObjFactoringInvoiceRow.Invoice_Load_Value);
                        db.AddInParameter(command, "@Credit_Days", DbType.Int32, ObjFactoringInvoiceRow.Credit_Days);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjFactoringInvoiceRow.Created_By);
                        db.AddInParameter(command, "@Bill_Type", DbType.Int32, ObjFactoringInvoiceRow.Bill_Type);
                        //db.AddInParameter(command, "@XMLFILDetails", DbType.String, ObjFactoringInvoiceRow.XMLFILDetails);                      
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XMLFILDetails", OracleType.Clob,
                                ObjFactoringInvoiceRow.XMLFILDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjFactoringInvoiceRow.XMLFILDetails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLFILDetails", DbType.String,
                                ObjFactoringInvoiceRow.XMLFILDetails);
                        }
                        db.AddInParameter(command, "@FIL_ID", DbType.Int32, ObjFactoringInvoiceRow.FIL_ID);
                        db.AddOutParameter(command, "@Fil_No", DbType.String, 100);
                        db.AddOutParameter(command, "@Party_Name", DbType.String, 100);
                        db.AddOutParameter(command, "@Invoice_No", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command,  ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
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
                                    strFILNo = (string)command.Parameters["@Fil_No"].Value;                                   
                                    trans.Commit();                                    
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                strInvoiceNo = (string)command.Parameters["@Invoice_No"].Value;
                                strPartyName = (string)command.Parameters["@Party_Name"].Value;
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

            public int FunPubCreateFactoringRetirement(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_FT_RetirementDataTable, out string strFILNo)
            {
                try
                {
                    strFILNo = "";

                    ObjFactoringRetirement_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_FT_RetirementDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_FT_RetirementDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_FT_RetirementDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_FT_RetirementRow ObjFactoringRetirementRow in ObjFactoringRetirement_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertFactoringRetirement");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjFactoringRetirementRow.Company_ID);
                        db.AddInParameter(command, "@FT_Retirement_ID", DbType.Int32, ObjFactoringRetirementRow.FT_Retirement_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjFactoringRetirementRow.User_ID);
                        db.AddInParameter(command, "@FIL_ID", DbType.Int32, ObjFactoringRetirementRow.FIL_ID);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjFactoringRetirementRow.Created_On);

                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_Data", OracleType.Clob,
                                ObjFactoringRetirementRow.XML_Data.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjFactoringRetirementRow.XML_Data);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_Data", DbType.String,
                                ObjFactoringRetirementRow.XML_Data);
                        }
                        db.AddOutParameter(command, "@Fil_No", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
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
                                    strFILNo = (string)command.Parameters["@Fil_No"].Value;
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
