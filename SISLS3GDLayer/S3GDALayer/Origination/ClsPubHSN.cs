#region Namespaces
using System;
using S3GDALayer.S3GAdminServices;
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
#endregion

namespace S3GDALayer.Origination
{
    namespace OrgMasterMgtServices
    {
        public class ClsPubHSN
        {
              #region Initialization
            int intRowsAffected;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_HSNMstDataTable ObjS3G_HSNMaster_DataTable_DAL;
            
            

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubHSN()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion


            #region DML Operation
            public int FunHSNMasterInsertInt(SerializationMode SerMode, byte[] bytesObjSNXG_HSN_MasterDataTable)
            {
                try
                {
                    ObjS3G_HSNMaster_DataTable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_HSNMstDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_HSN_MasterDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_HSNMstDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Insert_HSN_Details");
                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_HSNMstRow ObjHSNMasterRow in ObjS3G_HSNMaster_DataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@HSN_ID", DbType.String, ObjHSNMasterRow.HSN_Id);
                        db.AddInParameter(command, "@HSN_Code", DbType.String, ObjHSNMasterRow.HSN_Code);
                        db.AddInParameter(command, "@HSN_Name", DbType.String, ObjHSNMasterRow.HSN_Desc);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjHSNMasterRow.Company_id);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjHSNMasterRow.Is_Active);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjHSNMasterRow.Created_by);
                        db.AddInParameter(command, "@Code_Type", DbType.Int32, ObjHSNMasterRow.Code_Type);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjHSNMasterRow.Modified_by);
                        db.AddInParameter(command, "@SAC_ID", DbType.String, ObjHSNMasterRow.SAC_Id);
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
                }
                return intRowsAffected;
            }
        
            #endregion
        }
    }
}
