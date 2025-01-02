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
    namespace ClnReceivableMgtServices
    {
        public class ClsPubMarketEntryValue
        {
            int intRowsAffected;
            S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_MarketValueDataTable objMarketValue_DAL;
            //S3GBusEntity.Collection.ClnTransMgtServices.S3G_CLN_MarketValueDetailsDataTable objMarketValueDetails_DAL;
            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubMarketEntryValue()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }
            /// <summary>
            /// Inserting the Market Entry Value Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns>
            /// 
            public int FunPubCreateMarketValueEntry(SerializationMode SerMode, byte[] bytesObjS3G_Colection_MarketEntry_DataTable, out string strMVENumber)
            {
                strMVENumber = "";
                try
                {
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                    objMarketValue_DAL = (S3GBusEntity.Collection.ClnReceivableMgtServices.
                        S3G_CLN_MarketValueDataTable)ClsPubSerialize.
                        DeSerialize(bytesObjS3G_Colection_MarketEntry_DataTable,
                        SerMode, typeof(S3GBusEntity.Collection.
                        ClnReceivableMgtServices.S3G_CLN_MarketValueDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_MarketValueRow objMarketEntryRow in objMarketValue_DAL)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertMarketValueEntry");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objMarketEntryRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objMarketEntryRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objMarketEntryRow.Branch_ID);
                        db.AddInParameter(command, "@Market_Value_ID", DbType.String, objMarketEntryRow.Market_Value_ID);
                        db.AddInParameter(command, "@Option_Value", DbType.Int32, objMarketEntryRow.Option_Value);
                        //db.AddInParameter(command, "@Customer_ID", DbType.Int32, objMarketEntryRow.Customer_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objMarketEntryRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, objMarketEntryRow.Created_On);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, objMarketEntryRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, objMarketEntryRow.Modified_On);
                        db.AddInParameter(command, "@Txn_ID", DbType.Int32, objMarketEntryRow.Txn_ID);
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param;

                            param = new OracleParameter("@XMLDetail",
                                 OracleType.Clob, objMarketEntryRow.XMLDetails.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, objMarketEntryRow.XMLDetails);

                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLDetail", DbType.String, objMarketEntryRow.XMLDetails);
                        }
                        // db.AddInParameter(command, "@Market_Date", DbType.DateTime, objMarketEntryRow.Market_Date);
                        
                        db.AddInParameter(command, "@Is_Active", DbType.String, objMarketEntryRow.Active);
                        db.AddInParameter(command, "@Market_Value_Date", DbType.DateTime, objMarketEntryRow.Market_Value_Date);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@strMVENumber", DbType.String, 100);
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value == 0)
                        {
                            strMVENumber = Convert.ToString(command.Parameters["@strMVENumber"].Value);
                        }
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


            public int FunPubCreateMarketValueEntryGL(SerializationMode SerMode, byte[] bytesObjS3G_Colection_MarketEntry_DataTable, out string strMVENumber)
            {
                strMVENumber = "";
                try
                {
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                    objMarketValue_DAL = (S3GBusEntity.Collection.ClnReceivableMgtServices.
                        S3G_CLN_MarketValueDataTable)ClsPubSerialize.
                        DeSerialize(bytesObjS3G_Colection_MarketEntry_DataTable,
                        SerMode, typeof(S3GBusEntity.Collection.
                        ClnReceivableMgtServices.S3G_CLN_MarketValueDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_MarketValueRow objMarketEntryRow in objMarketValue_DAL)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertMarketValueEntryGL");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objMarketEntryRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objMarketEntryRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objMarketEntryRow.Branch_ID);
                        db.AddInParameter(command, "@Market_Value_ID", DbType.String, objMarketEntryRow.Market_Value_ID);
                        db.AddInParameter(command, "@Option_Value", DbType.Int32, objMarketEntryRow.Option_Value);
                        //db.AddInParameter(command, "@Customer_ID", DbType.Int32, objMarketEntryRow.Customer_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objMarketEntryRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, objMarketEntryRow.Created_On);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, objMarketEntryRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, objMarketEntryRow.Modified_On);
                        db.AddInParameter(command, "@Txn_ID", DbType.Int32, objMarketEntryRow.Txn_ID);
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param;

                            param = new OracleParameter("@XMLDetail",
                                 OracleType.Clob, objMarketEntryRow.XMLDetails.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, objMarketEntryRow.XMLDetails);

                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLDetail", DbType.String, objMarketEntryRow.XMLDetails);
                        }
                        // db.AddInParameter(command, "@Market_Date", DbType.DateTime, objMarketEntryRow.Market_Date);

                        db.AddInParameter(command, "@Is_Active", DbType.String, objMarketEntryRow.Active);
                        db.AddInParameter(command, "@CurrentMarketValue", DbType.String, objMarketEntryRow.CurrentMarketValue);
                        db.AddInParameter(command, "@AssetLevel", DbType.Boolean, objMarketEntryRow.AssetLevel);
                        db.AddInParameter(command, "@AssetMap", DbType.String, objMarketEntryRow.AssetMap);
                        db.AddInParameter(command, "@Market_Value_Date", DbType.DateTime, objMarketEntryRow.Market_Value_Date);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@strMVENumber", DbType.String, 100);
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value == 0)
                        {
                            strMVENumber = Convert.ToString(command.Parameters["@strMVENumber"].Value);
                        }
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;


                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLog.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;

            }


            /// <summary>
            /// To use  Market Entry Value Details 
            /// </summary>
            /// <param name="Market_Value_ID">Pass Market_Value_ID</param>
            /// <returns></returns>

            public DataTable FunMarketEntryForModification(string strMarketValueNo)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_GetMarketEntryValue");
                    db.AddInParameter(command, "@MarketValueNo", DbType.String, strMarketValueNo);
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
