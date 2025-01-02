#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: RS Charge Maintenance
/// Created By			: Vinodha M
/// Created Date		: 02-Dec-2014   
/// Purpose	            : RS Charge Maintenance DAL Class
/// Last Updated By		: 
/// Last Updated Date   : 
/// Reason              : 
/// <Program Summary>
#endregion

#region Namespaces
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
#endregion


namespace S3GDALayer.LoanAdmin
{
   namespace LoanAdminMgtServices
   {
    public class ClsPubRSChargeMaintenance
    {
        #region Initialization

        int intRowsAffected;
        S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable ObjRS_Charge_MgmtDataTable_DAL;
       
        //Below Code To Get The Connection string from the config file
        Database db;
        public ClsPubRSChargeMaintenance()
        {
            db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
        }
        #endregion

        #region CREATE/UPDATE RS CHARGE MAINTENANCE

        public int FunPubCreateRSChargeMaintenance(SerializationMode SerMode, byte[] bytesObjRS_Charge_MgmtDataTable,out string RSCM_Code)
        {
            try
            {
                RSCM_Code = "";
                ObjRS_Charge_MgmtDataTable_DAL=(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable)ClsPubSerialize.DeSerialize(bytesObjRS_Charge_MgmtDataTable,SerMode,typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable));
                DbCommand command = null;
                foreach(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtRow ObjRS_Charge_MgmtRow in ObjRS_Charge_MgmtDataTable_DAL.Rows)
                {
                    command = db.GetStoredProcCommand("S3G_LAD_RSCM_Insertion");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRS_Charge_MgmtRow.Company_ID);
                    db.AddInParameter(command, "@Lob_ID", DbType.Int32, ObjRS_Charge_MgmtRow.Lob_ID);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjRS_Charge_MgmtRow.Location_ID);
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjRS_Charge_MgmtRow.Customer_ID);
                    db.AddInParameter(command, "@Tranche_ID", DbType.String, ObjRS_Charge_MgmtRow.Tranche_ID);
                    db.AddInParameter(command, "@User_ID", DbType.Int32, ObjRS_Charge_MgmtRow.User_ID);
                    db.AddInParameter(command, "@XMLRSCM", DbType.String, ObjRS_Charge_MgmtRow.XMLRSCMDtls);
                    db.AddInParameter(command, "@RSCM_ID", DbType.Int32, ObjRS_Charge_MgmtRow.RSCM_ID);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@RSCM_Code", DbType.String, 50);

                    db.FunPubExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                    RSCM_Code = (string)command.Parameters["@RSCM_Code"].Value;                    
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

        #endregion

        #region Query RS CHARGE MAINTENANCE

        /// <summary>
        /// Gets a Tax Exemption details using Serialized data table object and serialized mode
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjSNXG_TaxGuideManagementDataTable"></param>
        /// <returns>Datatable containing Tax Exemption details</returns>

        public S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable FunPubQueryRSChargeMaintenance(SerializationMode SerMode, byte[] bytesObjRS_Charge_MgmtDataTable)
        {
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices dsRSChargeMain = new S3GBusEntity.LoanAdmin.LoanAdminMgtServices();
            ObjRS_Charge_MgmtDataTable_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable)ClsPubSerialize.DeSerialize(bytesObjRS_Charge_MgmtDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable));
            try
            {
                DbCommand command = db.GetStoredProcCommand("S3G_LAD_Get_RSCM_Dtls");
                foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtRow ObjRS_Charge_MgmtRow in ObjRS_Charge_MgmtDataTable_DAL.Rows)
                {
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRS_Charge_MgmtRow.Company_ID);
                    db.AddInParameter(command, "@RSCM_ID", DbType.Int32, ObjRS_Charge_MgmtRow.RSCM_ID);
                    db.FunPubLoadDataSet(command, dsRSChargeMain, dsRSChargeMain.S3G_LOANAD_RS_Charge_Mgmt.TableName);
                }
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex);
                throw ex;
            }
            return dsRSChargeMain.S3G_LOANAD_RS_Charge_Mgmt;
        }

        public S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable FunPubQueryRSChargeMaintenancePaging(SerializationMode SerMode, byte[] bytesObjRS_Charge_MgmtDataTable, out int intTotalRecords, PagingValues ObjPaging)
        {
            intTotalRecords = 0;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices dsRSChargeMain = new S3GBusEntity.LoanAdmin.LoanAdminMgtServices();
            ObjRS_Charge_MgmtDataTable_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable)ClsPubSerialize.DeSerialize(bytesObjRS_Charge_MgmtDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable));
            try
            {
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_Get_TaxExmp_Paging");
                foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtRow ObjRS_Charge_MgmtRow in ObjRS_Charge_MgmtDataTable_DAL.Rows)
                {
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRS_Charge_MgmtRow.Company_ID);
                    db.AddInParameter(command, "@RSCM_ID", DbType.Int32, ObjRS_Charge_MgmtRow.RSCM_ID);
                    db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                    db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                    db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                    db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                    db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));
                    db.FunPubLoadDataSet(command, dsRSChargeMain, dsRSChargeMain.S3G_LOANAD_RS_Charge_Mgmt.TableName);
                    if ((int)command.Parameters["@TotalRecords"].Value > 0)
                        intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                }
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex);
                throw ex;
            }
            return dsRSChargeMain.S3G_LOANAD_RS_Charge_Mgmt;
        }
        
        #endregion
    }
   }
}
