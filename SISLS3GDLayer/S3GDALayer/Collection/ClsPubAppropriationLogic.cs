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
namespace S3GDALayer.Collection
{
    namespace ClnReceiptMgtServices
    {
        public class ClsPubAppropriationLogic
        {
            int intRowsAffected;
            string strAppNumber = "";
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_AppropriationLogicDataTable objAppropriationLogic_DAl;

              //Code added for getting common connection string  from config file
            Database db;
            public ClsPubAppropriationLogic()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            /// <summary>
            /// Inserting the Appropriation logic Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns>
            /// 
            public int FunPubCreateAppropriationLogic(SerializationMode SerMode, byte[] bytesObjS3G_Colection_AppLogic_DataTable, out string strMVENumber)
            {
                strMVENumber = "";
                try
                {
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                    objAppropriationLogic_DAl = (S3GBusEntity.Collection.ClnReceiptMgtServices.
                        S3G_CLN_AppropriationLogicDataTable)ClsPubSerialize.
                        DeSerialize(bytesObjS3G_Colection_AppLogic_DataTable,
                        SerMode, typeof(S3GBusEntity.Collection.
                        ClnReceiptMgtServices.S3G_CLN_AppropriationLogicDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_AppropriationLogicRow objAppLogicRow in objAppropriationLogic_DAl)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertAppropriationLogic");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objAppLogicRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objAppLogicRow.LOB_ID);
                        db.AddInParameter(command, "@Appropriation_ID", DbType.Int32, objAppLogicRow.Appropriation_ID);
                        db.AddInParameter(command, "@Is_Active", DbType.String,objAppLogicRow.Is_Active);
                        db.AddInParameter(command, "@Appropriation_HorizVert", DbType.Int32, objAppLogicRow.Appropriation_HorizVert);  
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objAppLogicRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, objAppLogicRow.Created_On);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, objAppLogicRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, objAppLogicRow.Modified_On);
                        db.AddInParameter(command, "@Txn_ID", DbType.Int32, objAppLogicRow.Txn_ID);
                        db.AddInParameter(command, "@Is_FullDue", DbType.Int32, objAppLogicRow.Is_FullDue);
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param;
                            
                                param = new OracleParameter("@XMLDetail",
                                     OracleType.Clob, objAppLogicRow.XMLDetails.Length,
                                     ParameterDirection.Input, true, 0, 0, String.Empty,
                                      DataRowVersion.Default, objAppLogicRow.XMLDetails);
                            
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLDetail", DbType.String, objAppLogicRow.XMLDetails);
                        }
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@strAppNumber", DbType.String, 100);
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);

                        if (Convert.ToInt32(command.Parameters["@ErrorCode"].Value) == 0)
                        {
                            strAppNumber = Convert.ToString(command.Parameters["@strAppNumber"].Value);
                        }
                        if (Convert.ToInt32(command.Parameters["@ErrorCode"].Value) > 0)
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
            /// To use Appropriation logic Details 
            /// </summary>
            /// <param name="Market_Value_ID">Pass Market_Value_ID</param>
            /// <returns></returns>

            public DataTable FunAppropriationLogicForModification(string strApproID, int Company_ID)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_GetAppropriationLogic");
                    db.AddInParameter(command, "@AppropriationID", DbType.String, strApproID);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, Company_ID);
                    //db.LoadDataSet(command, ObjDS, "dtTable");
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
