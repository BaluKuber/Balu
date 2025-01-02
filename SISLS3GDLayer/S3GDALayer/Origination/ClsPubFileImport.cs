using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using S3GBusEntity;
using System.Data.Common;
using S3GDALayer.S3GAdminServices;
using System.Data.OracleClient;

namespace S3GDALayer.Origination
{
    public class ClsPubFileImport
    {
        S3GBusEntity.Origination.FileImport.S3G_ORG_FileUploadDataTable ObjFileImportDetails_DAL;
        S3GBusEntity.Origination.FileImport.S3G_ORG_FileSaveDataTable ObjFileSaveDetails_DAL;

          Database db;
          int intRowsAffected;
          public ClsPubFileImport()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

          public int FunPubCreateFileUploadInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FileUploadDataTable, out Int32 Upload_ID)
          {
              Upload_ID = 0;
              try
              {                  
                  ObjFileImportDetails_DAL = (S3GBusEntity.Origination.FileImport.S3G_ORG_FileUploadDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_FileUploadDataTable, SerMode, typeof(S3GBusEntity.Origination.FileImport.S3G_ORG_FileUploadDataTable));
                  using (DbConnection conn = db.CreateConnection())
                  {
                      conn.Open();
                      DbTransaction trans = conn.BeginTransaction();
                      try
                      {
                          foreach (S3GBusEntity.Origination.FileImport.S3G_ORG_FileUploadRow ObjFileUploadRow in ObjFileImportDetails_DAL.Rows)                              
                          {
                              DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertFileUploadDetails");
                              db.AddInParameter(command, "@Program_ID", DbType.Int32, ObjFileUploadRow.Program_ID);
                              db.AddInParameter(command, "@File_Name", DbType.String, ObjFileUploadRow.File_Name);
                              db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjFileUploadRow.Customer_ID);                              
                              db.AddInParameter(command, "@Upload_path", DbType.String, ObjFileUploadRow.Upload_path);                                                            
                              db.AddInParameter(command, "@Upload_by", DbType.String, ObjFileUploadRow.Upload_by);
                              db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                              db.AddOutParameter(command, "@Upload_ID", DbType.Int32, sizeof(Int64));
                              S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                              if (enumDBType == S3GDALDBType.ORACLE)
                              {
                                  OracleParameter param;
                                  if (ObjFileUploadRow != null)
                                  {
                                      param = new OracleParameter("@XML_FileList", OracleType.Clob,
                                          ObjFileUploadRow.XML_FileList.Length, ParameterDirection.Input, true,
                                          0, 0, String.Empty, DataRowVersion.Default, ObjFileUploadRow.XML_FileList);
                                      command.Parameters.Add(param);
                                  }
                              }
                              else
                              {
                                  db.AddInParameter(command, "@XML_FileList", DbType.String, ObjFileUploadRow.XML_FileList);
                              }
                              db.FunPubExecuteNonQuery(command, ref trans);

                              if ((int)command.Parameters["@ErrorCode"].Value > 0)
                              {
                                  intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                  throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                              }
                              Upload_ID = (Int32)command.Parameters["@Upload_ID"].Value;
                          }
                          trans.Commit();                          
                      }
                      catch (Exception ex)
                      {
                          if (intRowsAffected == 0)
                              intRowsAffected = 50;
                          ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                          trans.Rollback();
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
              }
              return intRowsAffected;
          }

          public int FunPubSaveFileUploadInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FileSaveDataTable, out string LSQ_Number,out string Error_Msg)
          {
              LSQ_Number = "";
              Error_Msg = "";
              try
              {
                  ObjFileSaveDetails_DAL = (S3GBusEntity.Origination.FileImport.S3G_ORG_FileSaveDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_FileSaveDataTable, SerMode, typeof(S3GBusEntity.Origination.FileImport.S3G_ORG_FileSaveDataTable));
                  using (DbConnection conn = db.CreateConnection())
                  {
                      conn.Open();
                      DbTransaction trans = conn.BeginTransaction();
                      try
                      {
                          foreach (S3GBusEntity.Origination.FileImport.S3G_ORG_FileSaveRow ObjFileSaveRow in ObjFileSaveDetails_DAL.Rows)
                          {
                              DbCommand command = db.GetStoredProcCommand("S3G_ORG_SaveUploadedFile");

                              db.AddInParameter(command, "@Upload_ID", DbType.Int32, ObjFileSaveRow.Upload_ID);
                              db.AddInParameter(command, "@Program_ID", DbType.Int32, ObjFileSaveRow.Program_ID);
                              db.AddInParameter(command, "@User_ID", DbType.Int32, ObjFileSaveRow.User_ID);
                              db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjFileSaveRow.Company_ID);
                              db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                              db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);
                              db.AddOutParameter(command, "@LSQ_Number", DbType.String, sizeof(Int64));
                              
                              command.CommandTimeout = 3600;
                              db.FunPubExecuteNonQuery(command, ref trans);

                              if ((int)command.Parameters["@ErrorCode"].Value > 0)
                              {
                                  intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                              }
                              LSQ_Number = command.Parameters["@LSQ_Number"].Value.ToString();
                              Error_Msg = command.Parameters["@ErrorMsg"].Value.ToString();
                          }
                          trans.Commit();
                      }
                      catch (Exception ex)
                      {
                          if (intRowsAffected == 0)
                              intRowsAffected = 50;
                          ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                          trans.Rollback();
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
              }
              return intRowsAffected;
          }
    }
}
