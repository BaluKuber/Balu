#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Tax Exemption Master DAL Class
/// Created By			: Vinodha M
/// Created Date		: 08-Oct-2014   
/// Purpose	            : 
/// Last Updated By		: 
/// Last Updated Date   : 
/// Reason              : 
/// <Program Summary>
#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using System.Data.Common;
using S3GDALayer.S3GAdminServices;
using System.Data;

#endregion

namespace S3GDALayer.Origination
{
    public class ClsPubTaxExemptionMaster
    {
        #region Initialization

        int intRowsAffected;
        S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable ObjTaxExemptionMasterDataTable_DAL;

        //Code added for getting common connection string  from config file
        Database db;
        public ClsPubTaxExemptionMaster()
        {
            db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
        }

        #endregion

        #region Create New Tax Exemption
        /// <summary>
        /// Creates a new Tax Exemption by getting Serialized data table object and serialized mode
        /// Create and update Tax Exemption details based on Tax Exemption No
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjS3G_ORG_TaxExemptionMasterDataTable"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>
        public int FunPubCreateTaxExemption(SerializationMode SerMode, byte[] bytesObjS3G_ORG_TaxExemptionMasterDataTable, out string Tax_Code, out decimal Balance_Exe_Amount)
        {
            try
            {
                Tax_Code = "";
                Balance_Exe_Amount = 0;
                ObjTaxExemptionMasterDataTable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_TaxExemptionMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable));
                DbCommand command = null;
                foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterRow ObjTaxExemptionMasterRow in ObjTaxExemptionMasterDataTable_DAL.Rows)
                {
                    command = db.GetStoredProcCommand("S3G_ORG_Insert_TaxExemption");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTaxExemptionMasterRow.Company_ID);
                    db.AddInParameter(command, "@Tax_ID", DbType.Int32, ObjTaxExemptionMasterRow.Tax_ID);
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjTaxExemptionMasterRow.Customer_ID);
                    if (!ObjTaxExemptionMasterRow.IsState_IDNull())
                        db.AddInParameter(command, "@State_ID", DbType.Int32, ObjTaxExemptionMasterRow.State_ID);
                    db.AddInParameter(command, "@TAN", DbType.String, ObjTaxExemptionMasterRow.TAN);
                    db.AddInParameter(command, "@Tax_Law_Section", DbType.String, ObjTaxExemptionMasterRow.Tax_Law_Section);
                    db.AddInParameter(command, "@Cashflow_ID", DbType.Int32, ObjTaxExemptionMasterRow.Cashflow_ID);
                    db.AddInParameter(command, "@Certificate_No", DbType.String, ObjTaxExemptionMasterRow.Certificate_No);
                    db.AddInParameter(command, "@Exe_Limit_Amount", DbType.Decimal, ObjTaxExemptionMasterRow.Exe_Limit_Amount);
                    db.AddInParameter(command, "@Effective_From", DbType.DateTime, ObjTaxExemptionMasterRow.Effective_From);
                    db.AddInParameter(command, "@Effective_To", DbType.DateTime, ObjTaxExemptionMasterRow.Effective_To);
                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjTaxExemptionMasterRow.Is_Active);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjTaxExemptionMasterRow.Created_By);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@Tax_Code", DbType.String, 50);
                    db.AddOutParameter(command, "@Balance_Exe_Amount", DbType.Decimal, sizeof(Decimal));

                    db.FunPubExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                    Tax_Code = (string)command.Parameters["@Tax_Code"].Value;
                    if((decimal)command.Parameters["@Balance_Exe_Amount"].Value>0)
                    Balance_Exe_Amount = (decimal)command.Parameters["@Balance_Exe_Amount"].Value;
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

        #region Query Tax Exemption Details
        /// <summary>
        /// Gets a Tax Exemption details using Serialized data table object and serialized mode
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjSNXG_TaxGuideManagementDataTable"></param>
        /// <returns>Datatable containing Tax Exemption details</returns>

        public S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable FunPubQueryTaxExemption(SerializationMode SerMode, byte[] bytesObjSNXG_TaxExemptionDataTable)
        {
            S3GBusEntity.Origination.OrgMasterMgtServices dsTaxExemption = new S3GBusEntity.Origination.OrgMasterMgtServices();
            ObjTaxExemptionMasterDataTable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_TaxExemptionDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable));
            try
            {
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_Get_TaxExempDtlByID");
                foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterRow ObjTaxExemptionMasterRow in ObjTaxExemptionMasterDataTable_DAL.Rows)
                {
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTaxExemptionMasterRow.Company_ID);
                    db.AddInParameter(command, "@Tax_ID", DbType.Int32, ObjTaxExemptionMasterRow.Tax_ID);
                    db.FunPubLoadDataSet(command, dsTaxExemption, dsTaxExemption.S3G_ORG_TaxExemptionMaster.TableName);
                }
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                throw ex;
            }
            return dsTaxExemption.S3G_ORG_TaxExemptionMaster;
        }

        public S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable FunPubQueryTaxExemptionPaging(SerializationMode SerMode, byte[] bytesObjSNXG_TaxExemptionDataTable, out int intTotalRecords, PagingValues ObjPaging)
        {
            intTotalRecords = 0;
            S3GBusEntity.Origination.OrgMasterMgtServices dsTaxExemption = new S3GBusEntity.Origination.OrgMasterMgtServices();
            ObjTaxExemptionMasterDataTable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_TaxExemptionDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable));
            try
            {
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_Get_TaxExmp_Paging");
                foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterRow ObjTaxExemptionMasterRow in ObjTaxExemptionMasterDataTable_DAL.Rows)
                {
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTaxExemptionMasterRow.Company_ID);
                    db.AddInParameter(command, "@Tax_ID", DbType.Int32, ObjTaxExemptionMasterRow.Tax_ID);
                    db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                    db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                    db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                    db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                    db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));
                    db.FunPubLoadDataSet(command, dsTaxExemption, dsTaxExemption.S3G_ORG_TaxExemptionMaster.TableName);
                    if ((int)command.Parameters["@TotalRecords"].Value > 0)
                        intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                }
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                throw ex;
            }
            return dsTaxExemption.S3G_ORG_TaxExemptionMaster;
        }

        public S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable FunPubQueryTaxExemptionHistoryPaging(SerializationMode SerMode, byte[] bytesObjSNXG_TaxExemptionDataTable, out int intTotalRecords, PagingValues ObjPaging)
        {
            intTotalRecords = 0;
            S3GBusEntity.Origination.OrgMasterMgtServices dsTaxExemption = new S3GBusEntity.Origination.OrgMasterMgtServices();
            ObjTaxExemptionMasterDataTable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_TaxExemptionDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable));
            try
            {
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_Get_TaxExmHis_Paging");
                foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterRow ObjTaxExemptionMasterRow in ObjTaxExemptionMasterDataTable_DAL.Rows)
                {
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTaxExemptionMasterRow.Company_ID);
                    db.AddInParameter(command, "@Tax_ID", DbType.Int32, ObjTaxExemptionMasterRow.Tax_ID);
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjTaxExemptionMasterRow.Customer_ID);
                    db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                    db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                    db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                    db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                    db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));
                    db.FunPubLoadDataSet(command, dsTaxExemption, dsTaxExemption.S3G_ORG_TaxExemptionMaster.TableName);
                    if ((int)command.Parameters["@TotalRecords"].Value > 0)
                        intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                }
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                throw ex;
            }
            return dsTaxExemption.S3G_ORG_TaxExemptionMaster;
        }
        #endregion
    }
}
