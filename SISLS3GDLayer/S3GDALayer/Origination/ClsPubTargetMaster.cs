#region Page Header
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Target Master DAL Class
/// Created By			: Ponnurajesh C
/// Created Date		: 16-April-2012
/// Purpose	            : To insert and modify Target Master details 
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
#endregion

using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using Entity_Collection = S3GBusEntity.Origination;

namespace S3GDALayer.Origination
{
    namespace OrgMasterMgtServices
    {
        public class ClsPubTargetMaster
        {
            #region Initialization
            int intRowsAffected;
            S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
            int intErrorCode;
            Entity_Collection.OrgMasterMgtServices dsTargetMasterDetails = null;
            Entity_Collection.OrgMasterMgtServices.S3G_ORG_TargetMasterDataTable ObjTargetMasterDataTable_DAL = null;
            Entity_Collection.OrgMasterMgtServices.S3G_ORG_TargetMasterRow ObjTargetMasterRow = null;
            Entity_Collection.OrgMasterMgtServices.S3G_ORG_TargetMasterDetailsDataTable ObjTargetMasterDetailsDataTable_DAL = null;
            Database db;
            public ClsPubTargetMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }
            #endregion


            #region Create Target Master
            /// <summary>
            /// Insert Target Master
            /// </summary>
           
            public int FunPubCreateTargetMasterInt(SerializationMode SerMode, byte[] bytesObjTargetMasterDataTable)
            {
                try
                {

                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertTargetMaster");
                    ObjTargetMasterDataTable_DAL = (Entity_Collection.OrgMasterMgtServices.S3G_ORG_TargetMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjTargetMasterDataTable, SerMode, typeof(Entity_Collection.OrgMasterMgtServices.S3G_ORG_TargetMasterDataTable));
                    ObjTargetMasterRow = ObjTargetMasterDataTable_DAL.Rows[0] as Entity_Collection.OrgMasterMgtServices.S3G_ORG_TargetMasterRow;
             
                     db.AddInParameter(command, "@Company_ID",DbType.Int32,ObjTargetMasterRow.Company_ID);
                     db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjTargetMasterRow.LOB_ID);
                     db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjTargetMasterRow.Location_ID);
                     db.AddInParameter(command, "@Marketing_Type", DbType.Int32, ObjTargetMasterRow.Marketing_Type);


                     if (!ObjTargetMasterRow.IsMarketing_CodeNull() && !string.IsNullOrEmpty(ObjTargetMasterRow.Marketing_Code))
                     {

                         db.AddInParameter(command, "@Marketing_Code", DbType.Int32, ObjTargetMasterRow.Marketing_Code);
                     }
                     db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjTargetMasterRow.Is_Active);
                     db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjTargetMasterRow.Product_ID);
                     db.AddInParameter(command, "@Created_By", DbType.Int32, ObjTargetMasterRow.Created_By);
                     db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjTargetMasterRow.Created_On);
                     db.AddInParameter(command, "@XMLParamTargetmasterDet", DbType.String, ObjTargetMasterRow.XMLParamTargetmasterDet);
                     db.AddInParameter(command, "@XMLParamCpyTargetmasterDet", DbType.String, ObjTargetMasterRow.XMLParamCpyTargetmasterDet);
                     db.AddInParameter(command, "@Marketing_Typecpy", DbType.Int32, ObjTargetMasterRow.Marketing_Typecpy);
                     db.AddInParameter(command, "@Business_Target_No", DbType.String, ObjTargetMasterRow.Business_Target_No);
                     db.AddInParameter(command, "@Frequency_Type", DbType.Int32, ObjTargetMasterRow.Frequency_Type);
                     db.AddInParameter(command, "@Portfolio_IRR", DbType.Decimal, ObjTargetMasterRow.Portfolio_IRR);
                     db.AddInParameter(command, "@Asset_Category", DbType.Int32, ObjTargetMasterRow.Asset_Category);
                     db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                     db.FunPubExecuteNonQuery(command);

                     if ((int)command.Parameters["@ErrorCode"].Value > 0)
                         intErrorCode = (int)command.Parameters["@ErrorCode"].Value;

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }
            #endregion

            #region Modify Target Master
            /// <summary>
            /// Update Target Master
            /// </summary>
            public int FunPubModifyTargetMasterInt(SerializationMode SerMode, byte[] bytesObjTargetMasterDataTable)
            {
                try
                {

                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_UpdateTargetMaster");
                    ObjTargetMasterDataTable_DAL = (Entity_Collection.OrgMasterMgtServices.S3G_ORG_TargetMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjTargetMasterDataTable, SerMode, typeof(Entity_Collection.OrgMasterMgtServices.S3G_ORG_TargetMasterDataTable));
                    ObjTargetMasterRow = ObjTargetMasterDataTable_DAL.Rows[0] as Entity_Collection.OrgMasterMgtServices.S3G_ORG_TargetMasterRow;

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTargetMasterRow.Company_ID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjTargetMasterRow.LOB_ID);
                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjTargetMasterRow.Is_Active);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjTargetMasterRow.Created_By);
                    db.AddInParameter(command, "@Business_Target_No", DbType.String, ObjTargetMasterRow.Business_Target_No);
                    if (!ObjTargetMasterRow.IsMarketing_CodeNull() && !string.IsNullOrEmpty(ObjTargetMasterRow.Marketing_Code))
                    {
                        db.AddInParameter(command, "@Marketing_Code", DbType.Int32, ObjTargetMasterRow.Marketing_Code);
                    }
                    db.AddInParameter(command, "@XMLParamTargetmasterDet", DbType.String, ObjTargetMasterRow.XMLParamTargetmasterDet);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.FunPubExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }
            #endregion

        }
    }
}
