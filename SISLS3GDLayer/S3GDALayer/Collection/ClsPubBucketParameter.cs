#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Collection
/// Screen Name			: Bucket Parameter DAL class
/// Created By			: Irsathameen K
/// Created Date		: 11-Oct-2010
/// Purpose	            : To access Bucket Parameter  db methods

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

namespace S3GDALayer.Collection
{
    namespace ClnReceivableMgtServices
    {

        public class ClsPubBucketParameter
        {
            int intRowsAffected;
            S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_BucketParameterDetailsDataTable objBucketParameter_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubBucketParameter()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }



            public int FunPubCreateBucketParameterDetails(SerializationMode SerMode, byte[] bytesObjS3G_CLN_BucketParameterDataTable, out string strBuckNo)
            {
                try
                {
                    strBuckNo = "";

                    objBucketParameter_DAL = (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_BucketParameterDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_CLN_BucketParameterDataTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_BucketParameterDetailsDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_BucketParameterDetailsRow ObjBucketpatameterRow in objBucketParameter_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertBucketParameter");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjBucketpatameterRow.Company_ID);
                        if (!ObjBucketpatameterRow.IsLOB_IDNull())
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjBucketpatameterRow.LOB_ID);
                        if (!ObjBucketpatameterRow.IsBranch_IDNull())
                            db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjBucketpatameterRow.Branch_ID);

                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjBucketpatameterRow.Created_By);
                        db.AddInParameter(command, "@XMLDays", DbType.String, ObjBucketpatameterRow.XMLDays);
                        db.AddInParameter(command, "@XMLValue", DbType.String, ObjBucketpatameterRow.XMLValue);
                        db.AddInParameter(command, "@XMLCategory", DbType.String, ObjBucketpatameterRow.XMLCategory);
                        db.AddInParameter(command, "@Buck_ID", DbType.Int32, ObjBucketpatameterRow.Buck_ID);
                        db.AddInParameter(command, "@Is_Active", DbType.Int32, ObjBucketpatameterRow.Is_Active);
                        db.AddOutParameter(command, "@Buck_No", DbType.String, 100);
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
                                   // throw new Exception("Error thrown Error No" + intRowsAffected.ToString());  //throw is hide by Tamilselvan.S on 14/07/2011
                                }
                                else if ((int)command.Parameters["@ErrorCode"].Value < 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    //if (intRowsAffected == -1)  //throw is hide by Tamilselvan.S on 14/07/2011
                                    //    throw new Exception("Document Sequence no not-defined");
                                    //if (intRowsAffected == -2)
                                    //    throw new Exception("Document Sequence no exceeds defined limit");
                                }
                                else
                                {
                                    trans.Commit();
                                    strBuckNo = Convert.ToString(command.Parameters["@Buck_No"].Value);
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
                    if (intRowsAffected == 0)  //Added by Tamilselvan.S on 14/07/2011
                        intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;
            }

        }
    }
}
