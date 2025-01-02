using System;
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
using S3GDALayer.S3GAdminServices;

namespace S3GDALayer.Origination
{
    namespace OrgMasterMgtServices
    {
        public class ClsPubAssetMasterHL
        {
            int intRowsAffected;
            string strCategoryCode = String.Empty;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_AssetMaster_HLDataTable objassetmst;


            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubAssetMasterHL()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }
            public int FunPubCreateAssetCodeInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_AssetMasterDataTable)
            {
                try
                {

                    objassetmst = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_AssetMaster_HLDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_AssetMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_AssetMaster_HLDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_AssetMaster_HLRow ObjAssetMasterRow in objassetmst.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_Ins_Asset_MST_HL");
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjAssetMasterRow.Company_id);
                        db.AddInParameter(command, "@User_id", DbType.Int32, ObjAssetMasterRow.User_id);
                        db.AddInParameter(command, "@Asset_id", DbType.Int32, ObjAssetMasterRow.Asset_id);
                        db.AddInParameter(command, "@Asset_Code", DbType.String, ObjAssetMasterRow.Asset_Code);
                        db.AddInParameter(command, "@Asset_Type", DbType.String, ObjAssetMasterRow.Asset_Type);
                        db.AddInParameter(command, "@Asset_Desc", DbType.String, ObjAssetMasterRow.Asset_Desc);
                        db.AddInParameter(command, "@Is_Active", DbType.Int32, ObjAssetMasterRow.Is_Active);
                        db.AddInParameter(command, "@XmlDetail", DbType.String, ObjAssetMasterRow.@XmlDetail);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
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

        }
    }
}
