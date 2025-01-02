#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: TaxGuide Master DAL Class
/// Created By			: Kaliraj K
/// Created Date		: 29-May-2010
/// Purpose	            : 
/// Last Updated By		: 
/// Last Updated Date   : 
/// Reason              : 
/// <Program Summary>
#endregion

#region Namespaces
using System;
using S3GDALayer.S3GAdminServices;
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
    /// Added the Name Space For Logical Grouping
    /// This Class belongs AccountMgtServices to the service group
    namespace AccountMgtServices
    {
        public class ClsPubTaxGuide
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable ObjTaxGuideDataTable_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubTaxGuide()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }
            
            #endregion

            #region Create New TaxGuide
            /// <summary>
            /// Creates a new TaxGuide by getting Serialized data table object and serialized mode
            /// Create and update TaxGuide details based on TaxGuide sequence id
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_TaxGuideDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>
            public int FunPubCreateTaxGuide(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_TaxGuideDataTable, out string Tax_Code)
            {
                try
                {
                    Tax_Code = "";

                    ObjTaxGuideDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_TaxGuideDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable));
                    DbCommand command = null;
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideRow ObjTaxGuideRow in ObjTaxGuideDataTable_DAL.Rows)
                    {
                        command = db.GetStoredProcCommand("S3G_Insert_TaxGuide_Details");                        
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTaxGuideRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjTaxGuideRow.LOB_ID);
                        db.AddInParameter(command, "@Tax_Class", DbType.Int32, ObjTaxGuideRow.Tax_Class);
                        db.AddInParameter(command, "@TaxType_ID", DbType.Int32, ObjTaxGuideRow.Tax_Type_ID);
                        db.AddInParameter(command, "@State_ID", DbType.Int32, ObjTaxGuideRow.State_ID);
                        db.AddInParameter(command, "@Tax_Description", DbType.String, ObjTaxGuideRow.Tax_Description);
                        if (!ObjTaxGuideRow.IsRatePercentageNull())
                        db.AddInParameter(command, "@RatePercentage", DbType.Decimal, ObjTaxGuideRow.RatePercentage);
                        if (!ObjTaxGuideRow.IsTaxNull())
                        db.AddInParameter(command, "@Tax", DbType.Decimal, ObjTaxGuideRow.Tax);
                        if (!ObjTaxGuideRow.IsSurchargeNull())
                        db.AddInParameter(command, "@Surcharge", DbType.Decimal, ObjTaxGuideRow.Surcharge);
                        if (!ObjTaxGuideRow.IsCessNull())
                        db.AddInParameter(command, "@Cess", DbType.Decimal, ObjTaxGuideRow.Cess);
                        if (!ObjTaxGuideRow.IsEducational_CessNull())
                            db.AddInParameter(command, "@Educational_Cess", DbType.Decimal, ObjTaxGuideRow.Educational_Cess);
                        if (!ObjTaxGuideRow.IsAdditional_taxNull())
                        db.AddInParameter(command, "@Additional_tax", DbType.Decimal, ObjTaxGuideRow.Additional_tax);
                        if (!ObjTaxGuideRow.IsAdditional_tax_nameNull())
                            db.AddInParameter(command, "@Additional_tax_name", DbType.String, ObjTaxGuideRow.Additional_tax_name);
                        if (!ObjTaxGuideRow.IsAdditional_tax_OnNull())
                        db.AddInParameter(command, "@Additional_tax_On", DbType.Int32, ObjTaxGuideRow.Additional_tax_On);
                        db.AddInParameter(command, "@Effective_From", DbType.DateTime, ObjTaxGuideRow.Effective_From);
                        if (!ObjTaxGuideRow.IsPosting_GL_CodeNull())
                            db.AddInParameter(command, "@GLCode", DbType.Int32, ObjTaxGuideRow.Posting_GL_Code);
                        if (!ObjTaxGuideRow.IsSL_CodeNull())
                            db.AddInParameter(command, "@SLCode", DbType.Int32, ObjTaxGuideRow.SL_Code);                        
                        if (!ObjTaxGuideRow.IsReverse_charge_typeNull())
                            db.AddInParameter(command, "@Reverse_charge_type", DbType.Int32, ObjTaxGuideRow.Reverse_charge_type);
                        if (!ObjTaxGuideRow.IsZone_IDNull())
                            db.AddInParameter(command, "@Zone_ID", DbType.Int32, ObjTaxGuideRow.Zone_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjTaxGuideRow.Created_By);                        
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjTaxGuideRow.Is_Active);
                        db.AddInParameter(command, "@XMLAsset", DbType.String, ObjTaxGuideRow.XMLAsset);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@Tax_Code", DbType.String, 50);
                        db.AddInParameter(command, "@TaxGuide_ID", DbType.Int32, ObjTaxGuideRow.Tax_Guide_ID);
                        
                        //call id 4078 on may 25,2016 by vinodha m
                        if (!ObjTaxGuideRow.IsOther_Tax_IDNull())
                        db.AddInParameter(command, "@Other_Tax_ID", DbType.Int32, ObjTaxGuideRow.Other_Tax_ID);
                        if (!ObjTaxGuideRow.IsOther_Tax_NameNull())
                        db.AddInParameter(command, "@Other_Tax_Name", DbType.String, ObjTaxGuideRow.Other_Tax_Name);
                        if (!ObjTaxGuideRow.IsOther_Tax_OnNull())
                        db.AddInParameter(command, "@Other_Tax_On", DbType.Int32, ObjTaxGuideRow.Other_Tax_On);
                        if (!ObjTaxGuideRow.IsOther_Tax_RateNull())
                        db.AddInParameter(command, "@Other_Tax_Rate", DbType.Decimal, ObjTaxGuideRow.Other_Tax_Rate);

                        db.AddInParameter(command, "@XMLHSN", DbType.String, ObjTaxGuideRow.XMLHSN);
                        if (!ObjTaxGuideRow.IsXML_ServicesNull())
                            db.AddInParameter(command, "@XMLServices", DbType.String, ObjTaxGuideRow.XML_Services);
                        if (!ObjTaxGuideRow.IsService_TypeNull())
                            db.AddInParameter(command, "@Service_Type", DbType.Int32, ObjTaxGuideRow.Service_Type);
                        db.AddInParameter(command, "@Abatement", DbType.Decimal, ObjTaxGuideRow.Abatement);

                        //db.ExecuteNonQuery(command);
                        
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                        if(!ObjTaxGuideRow.IsTax_CodeNull())
                        Tax_Code = (string)command.Parameters["@Tax_Code"].Value;
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
            #endregion

            #region Query TaxGuide Details
            /// <summary>
            /// Gets a TaxGuide details using Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjSNXG_TaxGuideManagementDataTable"></param>
            /// <returns>Datatable containing TaxGuide details</returns>

            public S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable FunPubQueryTaxGuide(SerializationMode SerMode, byte[] bytesObjSNXG_TaxGuideDataTable)
            {
                S3GBusEntity.AccountMgtServices dsTaxGuide = new S3GBusEntity.AccountMgtServices();
                ObjTaxGuideDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_TaxGuideDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable));
                try
                {                    
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_TaxGuide_Details");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideRow ObjTaxGuideRow in ObjTaxGuideDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTaxGuideRow.Company_ID);
                        db.AddInParameter(command, "@TaxGuide_ID", DbType.Int32, ObjTaxGuideRow.Tax_Code_ID);                       
                        db.FunPubLoadDataSet(command, dsTaxGuide, dsTaxGuide.S3G_SYSAD_TaxGuide.TableName);
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsTaxGuide.S3G_SYSAD_TaxGuide;
                
            }

            public S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable FunPubQueryTaxGuidePaging(SerializationMode SerMode, byte[] bytesObjSNXG_TaxGuideDataTable, out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                S3GBusEntity.AccountMgtServices dsTaxGuide = new S3GBusEntity.AccountMgtServices();
                ObjTaxGuideDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_TaxGuideDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_TaxGuide_Paging");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideRow ObjTaxGuideRow in ObjTaxGuideDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTaxGuideRow.Company_ID);
                        db.AddInParameter(command, "@TaxCode_ID", DbType.Int32, ObjTaxGuideRow.Tax_Code_ID);
                        db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                        db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                        db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                        db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                        db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));
                       // db.LoadDataSet(command, dsTaxGuide, dsTaxGuide.S3G_SYSAD_TaxGuide.TableName);
                        db.FunPubLoadDataSet(command, dsTaxGuide, dsTaxGuide.S3G_SYSAD_TaxGuide.TableName);
                        if ((int)command.Parameters["@TotalRecords"].Value > 0)
                            intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsTaxGuide.S3G_SYSAD_TaxGuide;

            }

            public S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable FunPubCreateTaxGuideAssetHistoryPaging(SerializationMode SerMode, byte[] bytesObjSNXG_TaxGuideDataTable, out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                S3GBusEntity.AccountMgtServices dsTaxGuide = new S3GBusEntity.AccountMgtServices();
                ObjTaxGuideDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_TaxGuideDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_TaxGuide_GetAHPaging");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideRow ObjTaxGuideRow in ObjTaxGuideDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTaxGuideRow.Company_ID);                        
                        db.AddInParameter(command, "@Tax_Type_ID", DbType.Int32, ObjTaxGuideRow.Tax_Type_ID);
                        db.AddInParameter(command, "@State_ID", DbType.Int32, ObjTaxGuideRow.State_ID);                        
                        db.AddInParameter(command, "@TaxGuide_ID", DbType.Int32, ObjTaxGuideRow.Tax_Guide_ID);
                        db.AddInParameter(command, "@Asset_Category_ID", DbType.Int32, ObjTaxGuideRow.Asset_Category_ID);
                        db.AddInParameter(command, "@Mode", DbType.String, ObjTaxGuideRow.Str_Mode);
                        db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                        db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);                        
                        db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                        db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));                        
                        db.FunPubLoadDataSet(command, dsTaxGuide, dsTaxGuide.S3G_SYSAD_TaxGuide.TableName);
                        if ((int)command.Parameters["@TotalRecords"].Value > 0)
                            intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                    }
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsTaxGuide.S3G_SYSAD_TaxGuide;

            }

            #endregion

         
        }
    }
}
