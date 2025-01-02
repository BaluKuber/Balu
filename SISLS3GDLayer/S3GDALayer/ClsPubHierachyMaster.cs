#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: SysAdmin
/// Screen Name			: Hierachy Master
/// Created By			: Irsathameen K
/// Created Date		: 03-May-2011
/// Purpose	            : To Create Hierachy Master

/// <Program Summary>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
#endregion

namespace S3GDALayer
{
    namespace CompanyMgtServices
    {
       public  class ClsPubHierachyMaster
        {
            int intRowsAffected;
           //Code added for getting common connection string  from config file
            Database db;
            public ClsPubHierachyMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_HierachyMasterDataTable objHierachyMaster_DAL;

            public int FunPubCreateHierachyMasterDetails(SerializationMode SerMode, byte[] bytesObjS3G_Sys_HierachyMAsterDataTable)
            {
                try
                {
                    objHierachyMaster_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_HierachyMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_Sys_HierachyMAsterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_HierachyMasterDataTable));
                    
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_HierachyMasterRow ObjHierachyMasterRow in objHierachyMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_SYSAD_InsertHierachyMaster");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjHierachyMasterRow.Company_ID);
                        db.AddInParameter(command, "@XMLHierachyDetails", DbType.String, ObjHierachyMasterRow.XMLHierachyDetails);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjHierachyMasterRow.Created_By);                        
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
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }                                
                                else
                                {
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
