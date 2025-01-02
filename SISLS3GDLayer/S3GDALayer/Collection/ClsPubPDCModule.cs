#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Collection
/// Screen Name			: PDC Entry DAL class
/// Created By			: Irsathameen K
/// Created Date		: 13-Oct-2010
/// Purpose	            : To access challan Rule  db methods
/// Modified By			: SwarnaLatha.B.M
/// Modified Date		: 10-Jan-2011
/// Purpose	            : Table Structure altered.
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
using S3GBusEntity.Collection;
namespace S3GDALayer.Collection
{
    namespace ClnReceiptMgtServices
    {

       public class ClsPubPDCModule
       {
           #region Initialization
             int intRowsAffected;
             S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_PDCModuleDetailsDataTable objPDCModule_DAL;
             S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_PDCModuleDetailsRow ObjPDCModuleRow = null;

             //Code added for getting common connection string  from config file
            Database db;
            public ClsPubPDCModule()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

           #endregion

            #region Create Mode
             public int FunPubCreatePDCModuleDetails(SerializationMode SerMode, byte[] bytesObjS3G_CLN_PDCModuleDataTable, out string strPDCNo, out string strchequeNo, out string strexistingdate)
            {
                try
                {
                      strPDCNo= "";
                      strchequeNo = "";
                      strexistingdate = "";
                      //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                      objPDCModule_DAL = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_PDCModuleDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_CLN_PDCModuleDataTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_PDCModuleDetailsDataTable));
                      ObjPDCModuleRow = objPDCModule_DAL.Rows[0] as S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_PDCModuleDetailsRow;

                      DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertPDCEntry");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPDCModuleRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPDCModuleRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjPDCModuleRow.Branch_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjPDCModuleRow.Created_By);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjPDCModuleRow.Customer_ID);
                        //db.AddInParameter(command, "@XMLPDCEntry", DbType.String, ObjPDCModuleRow.XMLPDCEntry);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XMLPDCEntry", OracleType.Clob,
                                ObjPDCModuleRow.XMLPDCEntry.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjPDCModuleRow.XMLPDCEntry);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLPDCEntry", DbType.String,
                                ObjPDCModuleRow.XMLPDCEntry);
                        }
                        db.AddInParameter(command, "@CollectionDate", DbType.DateTime, ObjPDCModuleRow.PDC_Collection_Date);
                        db.AddInParameter(command, "@EntryDate", DbType.DateTime, ObjPDCModuleRow.PDC_Entry_Date);
                        db.AddInParameter(command, "@PDCNO", DbType.String, ObjPDCModuleRow.PDC_Entry_NO);
                        db.AddInParameter(command, "@PANum", DbType.String, ObjPDCModuleRow.PANum);
                        if (!String.IsNullOrEmpty(ObjPDCModuleRow.SANum))
                            db.AddInParameter(command, "@SANum", DbType.String, ObjPDCModuleRow.SANum);
                        db.AddInParameter(command, "@NoPDC", DbType.Int32, ObjPDCModuleRow.No_Of_PDC);
                        if (!ObjPDCModuleRow.IsTXN_IDNull())
                        {  db.AddInParameter(command, "@TXN_ID", DbType.Int32, ObjPDCModuleRow.TXN_ID);    }
                        if (!ObjPDCModuleRow.IsDrawee_Bank_NameNull())
                        { db.AddInParameter(command, "@Drawee_Bank_Name", DbType.String, ObjPDCModuleRow.Drawee_Bank_Name); }
                        db.AddInParameter(command, "@InstrumentSequence", DbType.Int32, ObjPDCModuleRow.InstrumentSequence);
                        db.AddInParameter(command, "@Instrument_Type", DbType.Int32, ObjPDCModuleRow.Instrument_Type_Code);
                        db.AddInParameter(command, "@Instrument_Type_Code", DbType.Int32, ObjPDCModuleRow.Instrument_Type);
                        db.AddOutParameter(command, "@PDCModule_No", DbType.String, 100);

                        //Added by Sathiyanathan on 23-Sep-2013 for ISFC
                        //Added PDC Nature
                        db.AddInParameter(command, "@PDC_Nature_Code", DbType.Int32, ObjPDCModuleRow.PDC_Nature_Code);
                        db.AddInParameter(command, "@PDC_Nature_Type", DbType.Int32, ObjPDCModuleRow.PDC_Nature_Type);
                        //End Here
                        if (!ObjPDCModuleRow.IsTranche_Header_IdNull())
                            db.AddInParameter(command, "@Tranche_Header_Id", DbType.Int32, ObjPDCModuleRow.Tranche_Header_Id);
                        db.AddInParameter(command, "@PDC_Type", DbType.Int32, ObjPDCModuleRow.PDC_Type);
                        if (!ObjPDCModuleRow.IsPayee_NameNull())
                            db.AddInParameter(command, "@Payee_Name", DbType.String, ObjPDCModuleRow.Payee_Name);
                        db.AddOutParameter(command, "@Cheque_No", DbType.String, 150);
                        db.AddOutParameter(command, "@Existing_Date", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);


                                   if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                           strchequeNo=  (string)command.Parameters["@Cheque_No"].Value;
                                           strPDCNo=Convert.ToString(command.Parameters["@PDCModule_No"].Value);
                                           strexistingdate =  Convert.ToString(command.Parameters["@Existing_Date"].Value);
                                    //throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                           trans.Commit();
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
                                     strPDCNo = Convert.ToString(command.Parameters["@PDCModule_No"].Value);  
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
                            {  conn.Close();   }
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

             #region PDC Bulk Replacement Create Mode
             public int FunPubCreatePDCBulkModuleDetails(SerializationMode SerMode, byte[] bytesObjS3G_CLN_PDCModuleDataTable, out string strPDCNo, out string strchequeNo, out string strexistingdate)
             {
                 try
                 {
                     strPDCNo = "";
                     strchequeNo = "";
                     strexistingdate = "";
                     //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                     objPDCModule_DAL = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_PDCModuleDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_CLN_PDCModuleDataTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_PDCModuleDetailsDataTable));
                     ObjPDCModuleRow = objPDCModule_DAL.Rows[0] as S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_PDCModuleDetailsRow;

                     DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertPDCBulkReplace");
                     db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPDCModuleRow.Company_ID);
                     db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPDCModuleRow.LOB_ID);
                     db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjPDCModuleRow.Branch_ID);
                     db.AddInParameter(command, "@Created_By", DbType.Int32, ObjPDCModuleRow.Created_By);
                     db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjPDCModuleRow.Customer_ID);
                     //db.AddInParameter(command, "@XMLPDCEntry", DbType.String, ObjPDCModuleRow.XMLPDCEntry);
                     S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                     if (enumDBType == S3GDALDBType.ORACLE)
                     {
                         OracleParameter param = new OracleParameter("@XMLPDCEntry", OracleType.Clob,
                             ObjPDCModuleRow.XMLPDCEntry.Length, ParameterDirection.Input, true,
                             0, 0, String.Empty, DataRowVersion.Default, ObjPDCModuleRow.XMLPDCEntry);
                         command.Parameters.Add(param);
                     }
                     else
                     {
                         db.AddInParameter(command, "@XMLPDCEntry", DbType.String,
                             ObjPDCModuleRow.XMLPDCEntry);
                     }
                     db.AddInParameter(command, "@CollectionDate", DbType.DateTime, ObjPDCModuleRow.PDC_Collection_Date);
                     db.AddInParameter(command, "@EntryDate", DbType.DateTime, ObjPDCModuleRow.PDC_Entry_Date);
                     db.AddInParameter(command, "@PDCNO", DbType.String, ObjPDCModuleRow.PDC_Entry_NO);
                     db.AddInParameter(command, "@PANum", DbType.String, ObjPDCModuleRow.PANum);
                     db.AddInParameter(command, "@SANum", DbType.String, ObjPDCModuleRow.SANum);
                     db.AddInParameter(command, "@NoPDC", DbType.Int32, ObjPDCModuleRow.No_Of_PDC);
                     if (!ObjPDCModuleRow.IsTXN_IDNull())
                     { db.AddInParameter(command, "@TXN_ID", DbType.Int32, ObjPDCModuleRow.TXN_ID); }
                     if (!ObjPDCModuleRow.IsDrawee_Bank_NameNull())
                     { db.AddInParameter(command, "@Drawee_Bank_Name", DbType.String, ObjPDCModuleRow.Drawee_Bank_Name); }
                     db.AddInParameter(command, "@InstrumentSequence", DbType.Int32, ObjPDCModuleRow.InstrumentSequence);
                     db.AddInParameter(command, "@Instrument_Type", DbType.Int32, ObjPDCModuleRow.Instrument_Type_Code);
                     db.AddInParameter(command, "@Instrument_Type_Code", DbType.Int32, ObjPDCModuleRow.Instrument_Type);
                     db.AddOutParameter(command, "@PDCModule_No", DbType.String, 100);
                     db.AddOutParameter(command, "@Cheque_No", DbType.String, 150);
                     db.AddOutParameter(command, "@Existing_Date", DbType.String, 100);
                     db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                     using (DbConnection conn = db.CreateConnection())
                     {
                         conn.Open();
                         DbTransaction trans = conn.BeginTransaction();
                         try
                         {
                             db.FunPubExecuteNonQuery(command, ref trans);


                             if ((int)command.Parameters["@ErrorCode"].Value > 0)
                             {
                                 intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                 strchequeNo = (string)command.Parameters["@Cheque_No"].Value;
                                 strPDCNo = Convert.ToString(command.Parameters["@PDCModule_No"].Value);
                                 strexistingdate = Convert.ToString(command.Parameters["@Existing_Date"].Value);
                                 //throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                 trans.Commit();
                             }
                             else if ((int)command.Parameters["@ErrorCode"].Value < 0)
                             {
                                 intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                 //if (intRowsAffected == -1)
                                 //    throw new Exception("Document Sequence no not-defined");
                                 //if (intRowsAffected == -2)
                                 //    throw new Exception("Document Sequence no exceeds defined limit");
                             }
                             else
                             {
                                 trans.Commit();
                                 //strPDCNo = Convert.ToString(command.Parameters["@PDCModule_No"].Value);
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
                 catch (Exception ex)
                 {
                     intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                     throw ex;
                 }
                 return intRowsAffected;
             }
             #endregion

            
        }
    }

}
