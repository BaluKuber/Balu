#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Global Parameter Setup DAL Class
/// Created By			: Nataraj Y
/// Created Date		: 18-May-2010
/// Purpose	            : 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 25-May-2010
/// Reason              :
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 26-May-2010
/// Reason              : 
/// <Program Summary>
#endregion

#region Namspaces
using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Data.OracleClient;
#endregion

namespace S3GDALayer
{
    /// Added the Name Space For Logical Grouping
    /// This Class belongs CompanyMgtServices to the service group
    namespace CompanyMgtServices
    {
        public class ClsPubGlobalParameterSetup
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParameterSetupDataTable objS3G_SYSAD_GlobalParameterSetupDataTable_DAL;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable objS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubGlobalParameterSetup()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create new Global Parameter Setup
            /// <summary>
            /// Creates a new Set of Global Parameter SetUp by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesobjS3G_SYSAD_GlobalParameterSetupDataTable"></param>
            /// <param name="bytesobjS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>
             public int FunPubCreateGlobalParamInt(SerializationMode SerMode, byte[] bytesobjS3G_SYSAD_GlobalParameterSetupDataTable, byte[] bytesobjS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable)
             {
                try
                {

                    objS3G_SYSAD_GlobalParameterSetupDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParameterSetupDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_SYSAD_GlobalParameterSetupDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParameterSetupDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                   
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParameterSetupRow ObjGlobalParamerSetupRow in objS3G_SYSAD_GlobalParameterSetupDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Insert_GlobalParam_Details");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjGlobalParamerSetupRow.Company_ID);
                        db.AddInParameter(command, "@Currency_ID", DbType.Int32, ObjGlobalParamerSetupRow.Currency_ID);
                        db.AddInParameter(command, "@Currency_Max_Digit", DbType.Decimal, ObjGlobalParamerSetupRow.Currency_Max_Digit);
                        db.AddInParameter(command, "@Currency_Max_Dec_Digit", DbType.Decimal, ObjGlobalParamerSetupRow.Currency_Max_Dec_Digit);
                        db.AddInParameter(command, "@Currency_Effective_Date", DbType.DateTime, ObjGlobalParamerSetupRow.Currency_Effective_Date);
                        db.AddInParameter(command, "@Pincode_Field_Type", DbType.String, ObjGlobalParamerSetupRow.Pincode_Field_Type);
                        db.AddInParameter(command, "@Pincode_Digits", DbType.Decimal, ObjGlobalParamerSetupRow.Pincode_Digits);
                        db.AddInParameter(command, "@Integrated_System", DbType.Boolean, ObjGlobalParamerSetupRow.Integrated_System);
                        db.AddInParameter(command, "@Standalone_System", DbType.Boolean, ObjGlobalParamerSetupRow.Standalone_System);
                       // db.AddInParameter(command, "@Only_MLA", DbType.Boolean, ObjGlobalParamerSetupRow.Only_MLA);
                       // db.AddInParameter(command, "@MLA_SLA", DbType.Boolean, ObjGlobalParamerSetupRow.MLA_SLA);
                        db.AddInParameter(command, "@Display_Date_Format", DbType.String, ObjGlobalParamerSetupRow.Display_Date_Format);
                        db.AddInParameter(command, "@Global_Parm_Is_Active", DbType.Boolean, ObjGlobalParamerSetupRow.Global_Parm_Is_Active);
                        db.AddInParameter(command, "@Global_Parm_Created_By", DbType.Int32, ObjGlobalParamerSetupRow.Global_Parm_Created_By);
                        db.AddInParameter(command, "@Year_Lock_Value", DbType.String, ObjGlobalParamerSetupRow.Year_Lock_Value);
                        db.AddInParameter(command, "@Year_Lock_Option", DbType.Boolean, ObjGlobalParamerSetupRow.Year_Lock_Option);
                        db.AddInParameter(command, "@Month_Value", DbType.String, ObjGlobalParamerSetupRow.Month_Value);
                        db.AddInParameter(command, "@Month_Option", DbType.Boolean, ObjGlobalParamerSetupRow.Month_Option);
                        db.AddInParameter(command, "@Mnth_Param_Hdr_Is_Active", DbType.Boolean, ObjGlobalParamerSetupRow.Mnth_Param_Hdr_Is_Active);
                        db.AddInParameter(command, "@Mnth_Param_Hdr_Created_By", DbType.Int32, ObjGlobalParamerSetupRow.Mnth_Param_Hdr_Created_By);
                        db.AddInParameter(command, "@Account_Credit", DbType.Boolean, ObjGlobalParamerSetupRow.Account_Credit);
                        db.AddInParameter(command, "@opc_bank", DbType.Int32, ObjGlobalParamerSetupRow.OPC_banK);
                        db.AddInParameter(command, "@Days", DbType.Int32, ObjGlobalParamerSetupRow.Days);
                        db.AddInParameter(command, "@Disable_Access_Wrong_pwd", DbType.Int32, ObjGlobalParamerSetupRow.Disable_Access_Wrong_pwd);
                        db.AddInParameter(command, "@Min_pwd_length", DbType.Int32, ObjGlobalParamerSetupRow.Min_pwd_length);
                        db.AddInParameter(command, "@Pwd_Recycle_Itr", DbType.Int32, ObjGlobalParamerSetupRow.Pwd_Recycle_Itr);

                        db.AddInParameter(command, "@Force_Pwd_Change", DbType.Boolean, ObjGlobalParamerSetupRow.Force_Pwd_Change);
                        db.AddInParameter(command, "@Enforce_inital_pwd", DbType.Boolean, ObjGlobalParamerSetupRow.Enforce_inital_pwd);
                        db.AddInParameter(command, "@UpperCase_Char", DbType.Boolean, ObjGlobalParamerSetupRow.UpperCase_Char);
                        db.AddInParameter(command, "@Numeral_Char", DbType.Boolean, ObjGlobalParamerSetupRow.Numeral_Char);
                        db.AddInParameter(command, "@Spec_Char", DbType.Boolean, ObjGlobalParamerSetupRow.Spec_Char);
                        db.AddInParameter(command, "@GST_Effective_From", DbType.DateTime, ObjGlobalParamerSetupRow.GST_Effective_From);


                         S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                         if (enumDBType == S3GDALDBType.ORACLE)
                         {
                             OracleParameter param;
                             param = new OracleParameter("@XML_LOBDeltails",
                                 OracleType.Clob, ObjGlobalParamerSetupRow.XMLLobdetails.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                 DataRowVersion.Default, ObjGlobalParamerSetupRow.XMLLobdetails);
                             command.Parameters.Add(param);
                         }
                         else
                         {
                             db.AddInParameter(command, "@XML_LOBDeltails", DbType.String, 
                                 ObjGlobalParamerSetupRow.XMLLobdetails);
                         }

                        db.AddOutParameter(command, "@Month_End_Params_ID", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command,ref trans);
                                int intGlobal_id = (int)command.Parameters["@Month_End_Params_ID"].Value;
                                if (intGlobal_id != -1)
                                {
                                    objS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable));
                                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParamMonthEnd_DetailsRow ObjGlobalParamMonthEndDetailRow in objS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable_DAL.Rows)
                                    {
                                        command = db.GetStoredProcCommand("S3G_Insert_GlobalParamMonthEnd_Details");
                                        db.AddInParameter(command, "@Month_End_Params_ID", DbType.Int32, intGlobal_id);
                                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjGlobalParamMonthEndDetailRow.Branch_ID);
                                        db.AddInParameter(command, "@Month_Lock", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.Month_Lock);
                                        db.AddInParameter(command, "@Billing", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.Billing);
                                        db.AddInParameter(command, "@Interest_Calculation", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.Interest_Calculation);
                                        db.AddInParameter(command, "@Deliquency", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.Deliquency);
                                        db.AddInParameter(command, "@ODI", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.ODI);
                                        db.AddInParameter(command, "@Demand", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.Demand);
                                        db.AddInParameter(command, "@Depreciation", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.Depreciation);
                                        db.AddInParameter(command, "@Mnth_Param_Dtl_Is_Active", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.Mnth_Param_Dtl_Is_Active);
                                        db.AddInParameter(command, "@Mnth_Param_Dtl_Created_By", DbType.Int32, ObjGlobalParamMonthEndDetailRow.Mnth_Param_Dtl_Created_By);
                                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                                        db.FunPubExecuteNonQuery(command, ref trans);
                                    }
                                }
                                else
                                {
                                    throw new Exception("Error ocuured during Global parameter Setup DML Operation");
                                }
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                trans.Rollback();
                                intRowsAffected = 50;
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                            }
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
            #endregion

            #region Modify Global Parameter Setup
             /// <summary>
             /// Modifies an exsisting Set of Global Parameter SetUp by getting Serialized data table object and serialized mode
             /// </summary>
             /// <param name="SerMode"></param>
             /// <param name="bytesobjS3G_SYSAD_GlobalParameterSetupDataTable"></param>
             /// <param name="bytesobjS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable"></param>
             /// <returns>Error Code (it is 0 if no error is found)</returns>
             public int FunPubModifyGlobalParamInt(SerializationMode SerMode, byte[] bytesobjS3G_SYSAD_GlobalParameterSetupDataTable, byte[] bytesobjS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable)
             {
                 try
                 {

                     objS3G_SYSAD_GlobalParameterSetupDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParameterSetupDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_SYSAD_GlobalParameterSetupDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParameterSetupDataTable));

                     //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                     
                     foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParameterSetupRow ObjGlobalParamerSetupRow in objS3G_SYSAD_GlobalParameterSetupDataTable_DAL.Rows)
                     {
                         DbCommand command = db.GetStoredProcCommand("S3G_Update_GlobalParam_Details");
                         db.AddInParameter(command, "@Global_Parameters_ID", DbType.Int32, ObjGlobalParamerSetupRow.Global_Parameters_ID);
                         db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjGlobalParamerSetupRow.Company_ID);
                         db.AddInParameter(command, "@Currency_ID", DbType.Int32, ObjGlobalParamerSetupRow.Currency_ID);
                         db.AddInParameter(command, "@Currency_Max_Digit", DbType.Decimal, ObjGlobalParamerSetupRow.Currency_Max_Digit);
                         db.AddInParameter(command, "@Currency_Max_Dec_Digit", DbType.Decimal, ObjGlobalParamerSetupRow.Currency_Max_Dec_Digit);
                         db.AddInParameter(command, "@Currency_Effective_Date", DbType.DateTime, ObjGlobalParamerSetupRow.Currency_Effective_Date);
                         db.AddInParameter(command, "@Pincode_Field_Type", DbType.String, ObjGlobalParamerSetupRow.Pincode_Field_Type);
                         db.AddInParameter(command, "@Pincode_Digits", DbType.Decimal, ObjGlobalParamerSetupRow.Pincode_Digits);
                         db.AddInParameter(command, "@Integrated_System", DbType.Boolean, ObjGlobalParamerSetupRow.Integrated_System);
                         db.AddInParameter(command, "@Standalone_System", DbType.Boolean, ObjGlobalParamerSetupRow.Standalone_System);
                         db.AddInParameter(command, "@opc_bank", DbType.Int32, ObjGlobalParamerSetupRow.OPC_banK);
                         //db.AddInParameter(command, "@Only_MLA", DbType.Boolean, ObjGlobalParamerSetupRow.Only_MLA);
                         //db.AddInParameter(command, "@MLA_SLA", DbType.Boolean, ObjGlobalParamerSetupRow.MLA_SLA);
                         db.AddInParameter(command, "@Display_Date_Format", DbType.String, ObjGlobalParamerSetupRow.Display_Date_Format);
                         //db.AddInParameter(command,"@Account_Credit",DbType.Boolean,ObjGlobalParamerSetupRow.
                         db.AddInParameter(command, "@Global_Parm_Is_Active", DbType.Boolean, ObjGlobalParamerSetupRow.Global_Parm_Is_Active);
                         db.AddInParameter(command, "@Global_Parm_Modified_By", DbType.Int32, ObjGlobalParamerSetupRow.Global_Parm_Modified_By);
                         db.AddInParameter(command, "@Year_Lock_Value", DbType.String, ObjGlobalParamerSetupRow.Year_Lock_Value);
                         db.AddInParameter(command, "@Year_Lock_Option", DbType.Boolean, ObjGlobalParamerSetupRow.Year_Lock_Option);
                         db.AddInParameter(command, "@Month_Value", DbType.String, ObjGlobalParamerSetupRow.Month_Value);
                         db.AddInParameter(command, "@Month_Option", DbType.Boolean, ObjGlobalParamerSetupRow.Month_Option);
                         db.AddInParameter(command, "@Mnth_Param_Hdr_Is_Active", DbType.Boolean, ObjGlobalParamerSetupRow.Mnth_Param_Hdr_Is_Active);
                         db.AddInParameter(command, "@Mnth_Param_Hdr_Modified_By", DbType.Int32, ObjGlobalParamerSetupRow.Mnth_Param_Hdr_Modified_By);
                         db.AddInParameter(command, "@Month_End_Params_ID", DbType.Int32, ObjGlobalParamerSetupRow.Month_End_Params_ID);

                         db.AddInParameter(command, "@Days", DbType.Int32, ObjGlobalParamerSetupRow.Days);
                         db.AddInParameter(command, "@Disable_Access_Wrong_pwd", DbType.Int32, ObjGlobalParamerSetupRow.Disable_Access_Wrong_pwd);
                         db.AddInParameter(command, "@Min_pwd_length", DbType.Int32, ObjGlobalParamerSetupRow.Min_pwd_length);
                         db.AddInParameter(command, "@Pwd_Recycle_Itr", DbType.Int32, ObjGlobalParamerSetupRow.Pwd_Recycle_Itr);

                         db.AddInParameter(command, "@Force_Pwd_Change", DbType.Boolean, ObjGlobalParamerSetupRow.Force_Pwd_Change);
                         db.AddInParameter(command, "@Enforce_inital_pwd", DbType.Boolean, ObjGlobalParamerSetupRow.Enforce_inital_pwd);
                         db.AddInParameter(command, "@UpperCase_Char", DbType.Boolean, ObjGlobalParamerSetupRow.UpperCase_Char);
                         db.AddInParameter(command, "@Numeral_Char", DbType.Boolean, ObjGlobalParamerSetupRow.Numeral_Char);
                         db.AddInParameter(command, "@Spec_Char", DbType.Boolean, ObjGlobalParamerSetupRow.Spec_Char);

                         if (!ObjGlobalParamerSetupRow.IsGST_Effective_FromNull())
                             db.AddInParameter(command, "@GST_Effective_From", DbType.DateTime, ObjGlobalParamerSetupRow.GST_Effective_From);

                         S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                         if (enumDBType == S3GDALDBType.ORACLE)
                         {
                             OracleParameter param;
                             param = new OracleParameter("@XML_LOBDeltails",
                                 OracleType.Clob, ObjGlobalParamerSetupRow.XMLLobdetails.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                 DataRowVersion.Default, ObjGlobalParamerSetupRow.XMLLobdetails);
                             command.Parameters.Add(param);
                         }
                         else
                         {
                             db.AddInParameter(command, "@XML_LOBDeltails", DbType.String, 
                                 ObjGlobalParamerSetupRow.XMLLobdetails);
                         }

                         db.AddInParameter(command, "@Account_Credit", DbType.Boolean, ObjGlobalParamerSetupRow.Account_Credit);
                         db.AddOutParameter(command, "@Month_End_ID", DbType.Int32, sizeof(Int64));
                         db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                         using (DbConnection conn = db.CreateConnection())
                         {
                             conn.Open();
                             DbTransaction trans = conn.BeginTransaction();
                             try
                             {
                                 db.FunPubExecuteNonQuery(command,ref trans);
                                 int intGlobal_id = (int)command.Parameters["@Month_End_ID"].Value;
                                 if (intGlobal_id != -1)
                                 {
                                     if ((int)command.Parameters["@ErrorCode"].Value == 0)
                                     {
                                         objS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable));
                                         foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParamMonthEnd_DetailsRow ObjGlobalParamMonthEndDetailRow in objS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable_DAL.Rows)
                                         {
                                             command = db.GetStoredProcCommand("S3G_Update_GlobalParamMonthEnd_Details");
                                             db.AddInParameter(command, "@Month_End_Params_Det_ID", DbType.Int32, ObjGlobalParamMonthEndDetailRow.Month_End_Params_Det_ID);
                                             db.AddInParameter(command, "@Month_End_Params_ID", DbType.Int32, intGlobal_id);
                                             db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjGlobalParamMonthEndDetailRow.Branch_ID);
                                             db.AddInParameter(command, "@Month_Lock", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.Month_Lock);
                                             db.AddInParameter(command, "@Billing", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.Billing);
                                             db.AddInParameter(command, "@Interest_Calculation", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.Interest_Calculation);
                                             db.AddInParameter(command, "@Deliquency", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.Deliquency);
                                             db.AddInParameter(command, "@ODI", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.ODI);
                                             db.AddInParameter(command, "@Demand", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.Demand);
                                             db.AddInParameter(command, "@Depreciation", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.Depreciation);
                                             db.AddInParameter(command, "@Mnth_Param_Dtl_Is_Active", DbType.Boolean, ObjGlobalParamMonthEndDetailRow.Mnth_Param_Dtl_Is_Active);
                                             db.AddInParameter(command, "@Mnth_Param_Dtl_Modified_By", DbType.Int32, ObjGlobalParamMonthEndDetailRow.Mnth_Param_Dtl_Modified_By);
                                             db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                                             db.FunPubExecuteNonQuery(command,ref trans);
                                         }
                                         if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                             intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                          
                                     }
                                     else
                                     {
                                         intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                     }
                                 }
                                 else
                                 {
                                     throw new Exception("Error ocuured during Global parameter Setup DML Operation");
                                 }
                                 trans.Commit();
                             }
                             catch (Exception ex)
                             {
                                 // Roll back the transaction. 
                                 trans.Rollback();
                                 intRowsAffected = 50;
                                  ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                             }
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
             #endregion

            #region Query Global Parameter Details
            /// <summary>
            ///  Gets Global Parameter Details by passing Company Id
            /// </summary>
            /// <param name="intCompantId"></param>
            /// <returns></returns>
             public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParameterSetupDataTable FunPubQueryGPSDetails(int intCompantId,string Year,string Month)
             {
                 S3GBusEntity.CompanyMgtServices dsGPSDetails = new S3GBusEntity.CompanyMgtServices();
                 try
                 {
                     //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                     DbCommand command = db.GetStoredProcCommand("S3G_Get_GlobalParam_Details");
                     db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompantId);
                     db.AddInParameter(command, "@Year_Lock_Value", DbType.String, Year);
                     db.AddInParameter(command, "@Month_Value", DbType.String, Month);
                     db.FunPubLoadDataSet(command, dsGPSDetails, dsGPSDetails.S3G_SYSAD_GlobalParameterSetup.TableName);


                 }
                 catch (Exception ex)
                 {
                      ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                 }
                 return dsGPSDetails.S3G_SYSAD_GlobalParameterSetup;
             }

             public DataSet FunPubQueryGPSDetails_New(int intCompantId, string Year, string Month)
             {
                 DataSet ds=new DataSet();
                 try
                 {
                     //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                     DbCommand command = db.GetStoredProcCommand("S3G_Get_GlobalParam_Details_new");
                     db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompantId);
                     db.AddInParameter(command, "@Year_Lock_Value", DbType.String, Year);
                     db.AddInParameter(command, "@Month_Value", DbType.String, Month);
                     ds = db.FunPubExecuteDataSet(command);


                 }
                 catch (Exception ex)
                 {
                      ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                 }
                 return ds;
             }
             #endregion

        }
    }
}
