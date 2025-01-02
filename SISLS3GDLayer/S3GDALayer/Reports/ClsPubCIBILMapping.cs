using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORG = S3GBusEntity.Origination;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using S3GBusEntity;
using System.Data;

namespace S3GDALayer.Reports
{
    namespace CIBILMappingMgtServices
    {
        public class ClsPubCIBILMapping 
        {
           Database db;
           public ClsPubCIBILMapping()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }
            int intRowsAffected = 0;
            
            S3GBusEntity.Reports.CIBILMappingMgtServices.S3G_RPT_CIBILMappingDetailsDataTable ObjS3GCIBILMappingDataTable;
            public int FunCreateCIBILMapping(SerializationMode SerMode, byte[] byteCIBILMapping_DataTable, out string strErrMsg)
            {
                ObjS3GCIBILMappingDataTable = (S3GBusEntity.Reports.CIBILMappingMgtServices.S3G_RPT_CIBILMappingDetailsDataTable)ClsPubSerialize.DeSerialize(byteCIBILMapping_DataTable, SerMode, typeof(S3GBusEntity.Reports.CIBILMappingMgtServices.S3G_RPT_CIBILMappingDetailsDataTable));
                
                strErrMsg = string.Empty;
                try
                {
                   // Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LR_InsertCIBILMapping");
                    foreach (S3GBusEntity.Reports.CIBILMappingMgtServices.S3G_RPT_CIBILMappingDetailsRow objCIBILMappingRow in ObjS3GCIBILMappingDataTable)
                    {
                        db.AddInParameter(command, "@Detail_ID", DbType.Int32, objCIBILMappingRow.Detail_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objCIBILMappingRow.Company_ID);
                        db.AddInParameter(command, "@Lookup_Type_Code",DbType.String, objCIBILMappingRow.Lookup_Type_Code);
                        db.AddInParameter(command, "@Look_Up_ID", DbType.Int32, objCIBILMappingRow.Look_Up_ID);
                        db.AddInParameter(command, "@Value_ID", DbType.Int32,objCIBILMappingRow.Value_ID);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.ExecuteNonQuery(command, trans);
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                if ((int)command.Parameters["@ErrorCode"].Value > 1)
                                {
                                    throw new Exception("Error in CIBIL Mapping " + intRowsAffected.ToString());
                                }
                                else
                                {
                                    strErrMsg = Convert.ToString(command.Parameters["@Detail_ID"].Value);
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (command.Parameters["@ErrorCode"].Value != null)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                }
                                else if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();

                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 20; ;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }
        }
    }
}

