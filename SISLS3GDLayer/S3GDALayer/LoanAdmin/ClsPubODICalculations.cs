#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: ODI Calculations DAL Class
/// Created By			: Suresh P
/// Created Date		: 13-Sep-2010
/// Purpose	            : DAL Class for ODI Calculations Methods
/// Last Updated By		: Vjayakumar
/// Last Updated Date   : 21-Jan-2011
/// Reason              : NULL
/// <Program Summary>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using Entity_LoanAdmin = S3GBusEntity.LoanAdmin;
using System.Collections.Generic;


#endregion

namespace S3GDALayer.LoanAdmin
{
    namespace LoanAdminAccMgtServices
    {
        public class ClsPubODICalculations
        {
            Database db;
            public ClsPubODICalculations()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #region Initialization
            int intErrorCode;
            Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_ODICalculationsDataTable ObjODICalculationsDataTable_DAL = null;
            Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_ODICalculationsRow ObjODICalculationsRow = null;
            #endregion

            #region Create ODI Calculations

            /// <summary>
            /// To Insert ODI Calculations and Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjODICalculationsDataTable"></param>
            /// <returns></returns>

            public int FunPubCreateODICalculations(SerializationMode SerMode, byte[] bytesObjODICalculationsDataTable)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LOANAD_InsertODICalculations);

                    ObjODICalculationsDataTable_DAL = (Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_ODICalculationsDataTable)ClsPubSerialize.DeSerialize(bytesObjODICalculationsDataTable, SerMode, typeof(Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_ODICalculationsDataTable));
                    ObjODICalculationsRow = ObjODICalculationsDataTable_DAL.Rows[0] as Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_ODICalculationsRow;

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjODICalculationsRow.Company_ID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjODICalculationsRow.LOB_ID);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjODICalculationsRow.Branch_ID);

                    if (!ObjODICalculationsRow.IsGrace_PeriodNull())
                        db.AddInParameter(command, "@Grace_Period", DbType.Int32, ObjODICalculationsRow.Grace_Period);

                    if (!ObjODICalculationsRow.IsODI_Intrest_RateNull())
                        db.AddInParameter(command, "@ODI_Intrest_Rate", DbType.Decimal, ObjODICalculationsRow.ODI_Intrest_Rate);

                    if (!ObjODICalculationsRow.IsODI_Calculation_DateNull())
                        db.AddInParameter(command, "@ODI_Calculation_Date", DbType.DateTime, ObjODICalculationsRow.ODI_Calculation_Date);

                    if (!ObjODICalculationsRow.IsODI_Calculation_LevelNull())
                        db.AddInParameter(command, "@ODI_Calculation_Level", DbType.String, ObjODICalculationsRow.ODI_Calculation_Level);

                    if (!ObjODICalculationsRow.IsLast_Calculated_DateNull())
                        db.AddInParameter(command, "@Last_Calculated_Date", DbType.DateTime, ObjODICalculationsRow.Last_Calculated_Date);

                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjODICalculationsRow.Created_By);

                    if (!ObjODICalculationsRow.IsPANumNull())
                        db.AddInParameter(command, "@PANum", DbType.String, ObjODICalculationsRow.PANum);

                    if (!ObjODICalculationsRow.IsSANumNull())
                        db.AddInParameter(command, "@SANum", DbType.String, ObjODICalculationsRow.SANum);

                    if (!ObjODICalculationsRow.IsODI_AmountNull())
                        db.AddInParameter(command, "@ODI_Amount", DbType.Decimal, ObjODICalculationsRow.ODI_Amount);

                    if (!ObjODICalculationsRow.IsDue_DateNull())
                        db.AddInParameter(command, "@Due_Date", DbType.DateTime, ObjODICalculationsRow.Due_Date);

                    if (!ObjODICalculationsRow.IsCollection_DateNull())
                        db.AddInParameter(command, "@Collection_Date", DbType.DateTime, ObjODICalculationsRow.Collection_Date);
                    if (!ObjODICalculationsRow.IsDue_AmountNull())
                        db.AddInParameter(command, "@Due_Amount", DbType.Decimal, ObjODICalculationsRow.Due_Amount);

                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    //db.ExecuteNonQuery(command);
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

            #region Get ODI Calculations
            /// <summary>
            ///  To Get the ODI Calculations and Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjODICalculationsDataTable"></param>
            /// <returns></returns>
            public Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_ODICalculationsDataTable FunPubQueryODICalculations(SerializationMode SerMode, byte[] bytesObjODICalculationsDataTable)
            {
                S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices dsODICalculationsDetails = new S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices();
                ObjODICalculationsDataTable_DAL = (Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_ODICalculationsDataTable)ClsPubSerialize.DeSerialize(bytesObjODICalculationsDataTable, SerMode, typeof(Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_ODICalculationsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    //Database db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LOANAD_GetODICalculations);
                    ObjODICalculationsRow = ObjODICalculationsDataTable_DAL.Rows[0] as Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_ODICalculationsRow;

                    db.AddInParameter(command, "@ODI_Calculation_ID", DbType.Int32, ObjODICalculationsRow.ODI_Calculation_ID);
                    //db.LoadDataSet(command, dsODICalculationsDetails, dsODICalculationsDetails.S3G_LOANAD_ODICalculations.TableName);
                    db.FunPubLoadDataSet(command, dsODICalculationsDetails, dsODICalculationsDetails.S3G_LOANAD_ODICalculations.TableName);

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsODICalculationsDetails.S3G_LOANAD_ODICalculations;

            }
            #endregion

            public DataSet FunPubGetService(int intCompanyId, string ServiceName, DateTime Schedule_Date)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3g_LoanAd_GetSerivce");
                    //db.AddInParameter(command, "@CompanyId", DbType.Int32, intCompanyId);
                    //db.AddInParameter(command, "@Schedule_Date", DbType.DateTime, Schedule_Date);
                    db.AddInParameter(command, "@Service_Name", DbType.String, ServiceName);
                    //db.LoadDataSet(command, ObjDS, "Service");
                    db.FunPubLoadDataSet(command, ObjDS, "Service");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public string FunPubRevokeODI(int intCompanyId, string ODIId, int intUserId)
            {
                int intErrorCode = 0;
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LoanAd_RevokeOverDueInterest");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@ODI_Calculation_ID", DbType.String, ODIId);
                    db.AddInParameter(command, "@User_Id", DbType.Int32, intUserId);

                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    //db.ExecuteNonQuery(command);
                    db.FunPubExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return intErrorCode.ToString();
            }


            //public int FunPubSaveODISchedule(Dictionary<string, string> dctProcParams)
            public int FunPubSaveODISchedule(SerializationMode SerMode, byte[] bytesDictionary)
            {
                int strErrorCode = 0;

                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    Dictionary<string, string> dctProcParams = new Dictionary<string, string>();
                    dctProcParams = (Dictionary<string, string>)ClsPubSerialize.DeSerialize(bytesDictionary, SerMode, typeof(Dictionary<string, string>));

                    DbCommand command = db.GetStoredProcCommand("S3g_LoanAd_ODIScheduleInsert");
                    foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                    {
                        db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                    }
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    //db.ExecuteNonQuery(command);
                    db.FunPubExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        strErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return strErrorCode;

            }


            //public int FunPubSaveODI(Dictionary<string, string> dctProcParams,out string strErrorLog)
            //Source modified by Tamilselvan.S on 27/01/2011
            public int FunPubSaveODI(SerializationMode SerMode, byte[] bytesObjODICalculationsDataTable, out string strErrorLog)
            {
                int strErrorCode = 0;
                strErrorLog = "";
                try
                {
                    Dictionary<string, string> dctProcParams = new Dictionary<string, string>();
                    dctProcParams = (Dictionary<string, string>)ClsPubSerialize.DeSerialize(bytesObjODICalculationsDataTable, SerMode, typeof(Dictionary<string, string>));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_LoanAd_InsertOverDueInterest");
                    foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                    {
                        db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                    }
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@ErrorLog", DbType.String, 20000);

                    //db.ExecuteNonQuery(command);
                    db.FunPubExecuteNonQuery(command);

                    //if ((int)command.Parameters["@ErrorCode"].Value == 0)
                    //{
                    strErrorLog = Convert.ToString(command.Parameters["@ErrorLog"].Value);
                    //}
                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        strErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return strErrorCode;

            }
        }
    }
}
