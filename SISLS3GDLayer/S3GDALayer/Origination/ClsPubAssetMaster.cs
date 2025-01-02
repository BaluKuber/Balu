#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Asset Master
/// Created By			: Nataraj Y
/// Created Date		: 01-Jun-2010
/// Purpose	            : 
/// Modified By         : Nataraj Y
/// Modified Date       : 02-Jun-2010
/// Modified By         : Nataraj Y
/// Modified Date       : 03-Jun-2010
/// Modified By         : Nataraj Y
/// Modified Date       : 04-Jun-2010
/// Modified By         : Nataraj Y
/// Modified Date       : 05-Jun-2010
/// <Program Summary>
#endregion

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
        public class ClsPubAssetMaster
        {
            #region Intialize
            int intRowsAffected;
            string strCategoryCode = String.Empty;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetCategoryDataTable ObjAssetCategory_DAL;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetMasterDataTable ObjAssetMaster_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubAssetMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Creation Asset Category Code & Asset Code
            /// <summary>
            /// To create a new asset category
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_AssetCategoryDataTable"></param>
            /// <returns></returns>
            public int FunPubCreateAssetCategoryInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_AssetCategoryDataTable)
            {
                try
                {

                    ObjAssetCategory_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetCategoryDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_AssetCategoryDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetCategoryDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetCategoryRow ObjAssetCategoryRow in ObjAssetCategory_DAL.Rows)
                            {
                                DbCommand command = db.GetStoredProcCommand("S3G_ORG_Ins_Asset_Master");
                                db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjAssetCategoryRow.Company_ID);
                                db.AddInParameter(command, "@User_Id", DbType.Int32, ObjAssetCategoryRow.Created_By);
                                db.AddInParameter(command, "@Category_ID", DbType.Int32, ObjAssetCategoryRow.Category_ID);
                                db.AddInParameter(command, "@Category_Description", DbType.String, ObjAssetCategoryRow.Category_Description);
                                if (!ObjAssetCategoryRow.IsXMLDetailsNull())
                                    db.AddInParameter(command, "@XmlDetail", DbType.String, ObjAssetCategoryRow.XMLDetails);
                                db.AddInParameter(command, "@IS_Active", DbType.Boolean, ObjAssetCategoryRow.Is_Active);
                                db.AddInParameter(command, "@HSN_ID", DbType.Int32, ObjAssetCategoryRow.HSN_Id);
                                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                                db.FunPubExecuteNonQuery(command, ref trans);

                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    //throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }

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
            /// <summary>
            /// TO create a new asset code 
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_AssetMasterDataTable"></param>
            /// <returns></returns>
            public int FunPubCreateAssetCodeInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_AssetMasterDataTable)
            {
                try
                {

                    ObjAssetMaster_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_AssetMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetMasterRow ObjAssetMasterRow in ObjAssetMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertAssetMasterDetails");
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjAssetMasterRow.Company_ID);
                        db.AddInParameter(command, "@Class_ID", DbType.Int32, ObjAssetMasterRow.Class_ID);
                        db.AddInParameter(command, "@Make_ID", DbType.Int32, ObjAssetMasterRow.Make_ID);
                        db.AddInParameter(command, "@Type_ID", DbType.Int32, ObjAssetMasterRow.Type_ID);
                        db.AddInParameter(command, "@Model_ID", DbType.Int32, ObjAssetMasterRow.Model_ID);
                        db.AddInParameter(command, "@AssetType_ID", DbType.Int32, ObjAssetMasterRow.AssetType_ID);
                        db.AddInParameter(command, "@Code", DbType.String, ObjAssetMasterRow.Asset_Code);
                        db.AddInParameter(command, "@Description", DbType.String, ObjAssetMasterRow.Asset_Description);
                        if (!ObjAssetMasterRow.IsBook_Depreciation_RateNull())
                        {
                            db.AddInParameter(command, "@BookDepreciationCategory", DbType.String, ObjAssetMasterRow.Book_Depreciation_Category);
                        }
                        if (!ObjAssetMasterRow.IsBook_Depreciation_RateNull())
                        {
                            db.AddInParameter(command, "@BookDepreciationRate", DbType.Double, ObjAssetMasterRow.Book_Depreciation_Rate);
                        }
                        if (!ObjAssetMasterRow.IsBlock_Depreciation_RateNull())
                        {
                            db.AddInParameter(command, "@BlockDepreciationCategory", DbType.String, ObjAssetMasterRow.Block_Depreciation_Category);
                        }
                        if (!ObjAssetMasterRow.IsBlock_Depreciation_RateNull())
                        {
                            db.AddInParameter(command, "@BlockDepreciationRate", DbType.Double, ObjAssetMasterRow.Block_Depreciation_Rate);
                        }
                        if (!ObjAssetMasterRow.IsGuideLine_ValueNull())
                        {
                            db.AddInParameter(command, "@GuideLineValue", DbType.Double, ObjAssetMasterRow.GuideLine_Value);
                        }
                        db.AddInParameter(command, "@ISActive", DbType.Boolean, ObjAssetMasterRow.IS_Active);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjAssetMasterRow.Created_By);
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
            #endregion

            #region Modify Asset Code
            /// <summary>
            /// To update a asset code
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_AssetMasterDataTable"></param>
            /// <returns></returns>
            public int FunPubModifyAssetCodeInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_AssetMasterDataTable)
            {
                try
                {

                    ObjAssetMaster_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_AssetMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetMasterRow ObjAssetMasterRow in ObjAssetMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_UpdateAssetMasterDetails");
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjAssetMasterRow.Company_ID);
                        db.AddInParameter(command, "@Asset_ID", DbType.Int32, ObjAssetMasterRow.Asset_Master_ID);
                        db.AddInParameter(command, "@Code", DbType.String, ObjAssetMasterRow.Asset_Code);
                        db.AddInParameter(command, "@Description", DbType.String, ObjAssetMasterRow.Asset_Description);
                        if (!ObjAssetMasterRow.IsBook_Depreciation_CategoryNull())
                        {
                            db.AddInParameter(command, "@BookDepreciationCategory", DbType.String, ObjAssetMasterRow.Book_Depreciation_Category);
                        }
                        if (!ObjAssetMasterRow.IsBook_Depreciation_RateNull())
                        {
                            db.AddInParameter(command, "@BookDepreciationRate", DbType.Double, ObjAssetMasterRow.Book_Depreciation_Rate);
                        }
                        if (!ObjAssetMasterRow.IsBlock_Depreciation_CategoryNull())
                        {
                            db.AddInParameter(command, "@BlockDepreciationCategory", DbType.String, ObjAssetMasterRow.Block_Depreciation_Category);
                        }
                        if (!ObjAssetMasterRow.IsBlock_Depreciation_RateNull())
                        {
                            db.AddInParameter(command, "@BlockDepreciationRate", DbType.Double, ObjAssetMasterRow.Block_Depreciation_Rate);
                        }
                        if (!ObjAssetMasterRow.IsGuideLine_ValueNull())
                        {
                            db.AddInParameter(command, "@GuideLineValue", DbType.Double, ObjAssetMasterRow.GuideLine_Value);
                        }
                        db.AddInParameter(command, "@ISActive", DbType.Boolean, ObjAssetMasterRow.IS_Active);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjAssetMasterRow.Modified_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //                        db.ExecuteNonQuery(command);
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

            #region Query Asset Category Code & Asset Code
            /// <summary>
            /// to get all asset category code
            /// </summary>
            /// <param name="intCompany_Id"></param>
            /// <returns></returns>
            public DataSet FunPubQueryAssetCategoryDetails(int intCompany_Id, int? intAsset_CatId, int? intAssetType_Id)
            {
                DataSet dsAssetCaterory = new DataSet();
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetAssetCategoryDetails");
                    db.AddInParameter(command, "@Company_Id", DbType.Int32, intCompany_Id);
                    if (intAsset_CatId.HasValue)
                    {
                        db.AddInParameter(command, "@Asset_Category_ID", DbType.Int32, intAsset_CatId);
                    }
                    if (intAssetType_Id.HasValue)
                    {
                        db.AddInParameter(command, "@AssetType_ID", DbType.Int32, intAssetType_Id);
                    }
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    //dsAssetCaterory = db.ExecuteDataSet(command);
                    dsAssetCaterory = db.FunPubExecuteDataSet(command);

                    //if (dsAssetCaterory.Tables[1].Rows.Count > 0 && dsAssetCaterory.Tables[1].Rows[0][0].ToString() == "DUMMY")
                    //{
                    //    dsAssetCaterory.Tables.RemoveAt(1);
                    //    dsAssetCaterory.Tables.RemoveAt(1);
                    //    dsAssetCaterory.Tables.RemoveAt(1);
                    //}

                    if (command.Parameters.Contains("@ErrorCode") && (int)command.Parameters["@ErrorCode"].Value > 0)
                        throw new Exception("Error in Getting Asset Category Code details");

                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsAssetCaterory;
            }
            /// <summary>
            /// To get asset details 
            /// </summary>
            /// <param name="intCompany_Id"></param>
            /// <param name="intAssed_Id"></param>
            /// <returns></returns>
            public S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetMasterDataTable FunPubQueryAssetDetails(int intCompany_Id, int? intAssed_Id, int? intAssetType_Id)
            {
                S3GBusEntity.Origination.OrgMasterMgtServices dsAssetDetails = new S3GBusEntity.Origination.OrgMasterMgtServices();

                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetAssetDetails");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_Id);
                    if (intAssed_Id.HasValue && intAssed_Id.Value != 0)
                    {
                        db.AddInParameter(command, "@Asset_ID", DbType.Int32, intAssed_Id.Value);
                    }
                    if (intAssetType_Id.HasValue)
                    {
                        db.AddInParameter(command, "@AssetType_Id", DbType.Int32, intAssetType_Id.Value);
                    }
                    //db.LoadDataSet(command, dsAssetDetails, dsAssetDetails.S3G_ORG_AssetMaster.TableName);
                    db.FunPubLoadDataSet(command, dsAssetDetails, dsAssetDetails.S3G_ORG_AssetMaster.TableName);

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsAssetDetails.S3G_ORG_AssetMaster;
            }
            #endregion

            #region Query last generated Asset Category Code
            /// <summary>
            /// To get last generated asset code
            /// </summary>
            /// <param name="intCompany_Id"></param>
            /// <param name="strCategoryCode"></param>
            /// <returns></returns>
            public string FunPubGetLastAssetCategoryCode(int intCompany_Id, string strCategoryCode, int intAssetType_ID)
            {

                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetLastAssetCategoryCode");
                    db.AddInParameter(command, "@Company_Id", DbType.Int32, intCompany_Id);
                    db.AddInParameter(command, "@CategoryType", DbType.String, strCategoryCode);
                    // db.AddInParameter(command, "@AssetType_ID", DbType.Int32, intAssetType_ID);
                    db.AddOutParameter(command, "@Code", DbType.String, 3);
                    //                   db.ExecuteNonQuery(command);
                    db.FunPubExecuteNonQuery(command);
                    if (command.Parameters["@Code"].Value.ToString().Length > 0)
                    {
                        strCategoryCode = (string)command.Parameters["@Code"].Value;
                    }
                    else
                    {
                        strCategoryCode = "";
                    }


                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return strCategoryCode;
            }
            #endregion
        }
    }
}
