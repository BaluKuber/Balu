#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: Month Closure DAL Class
/// Created By			: Suresh P
/// Created Date		: 20-Aug-2010
/// Purpose	            : DAL Class for Month Closure Methods
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
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
#endregion

namespace S3GDALayer.LoanAdmin
{
    namespace LoanAdminAccMgtServices
    {
        public class ClsPubMonthClosure
        {
            #region Initialization

            int intErrorCode;
            Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_MonthClosureDataTable ObjMonthClosureDataTable_DAL = null;
            Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_MonthClosureRow ObjMonthClosureRow = null;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubMonthClosure()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create Month Closure
            /// <summary>
            /// 
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjMonthClosureDataTable"></param>
            /// <returns></returns>
            public int FunPubMonthClosureInt(SerializationMode SerMode, byte[] bytesObjMonthClosureDataTable)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LOANAD_InsertMonthClosure);

                    ObjMonthClosureDataTable_DAL = (Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_MonthClosureDataTable)ClsPubSerialize.DeSerialize(bytesObjMonthClosureDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_MonthClosureDataTable));
                    ObjMonthClosureRow = ObjMonthClosureDataTable_DAL.Rows[0] as Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_MonthClosureRow;

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjMonthClosureRow.Company_ID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjMonthClosureRow.LOB_ID);
                    if (!ObjMonthClosureRow.IsBranch_IDNull())
                        db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjMonthClosureRow.Branch_ID);
                    if (!ObjMonthClosureRow.IsClosure_idNull())
                        db.AddInParameter(command, "@Closure_id", DbType.Int32, ObjMonthClosureRow.Closure_id);

                    db.AddInParameter(command, "@Month_Closure_Date", DbType.DateTime, ObjMonthClosureRow.Month_Closure_Date);
                    db.AddInParameter(command, "@Closure_Month", DbType.String, ObjMonthClosureRow.Closure_Month);
                    db.AddInParameter(command, "@Month_Closure_Type_code", DbType.Int32, ObjMonthClosureRow.Month_Closure_Type_code);
                    db.AddInParameter(command, "@Month_Closure_Type", DbType.Int32, ObjMonthClosureRow.Month_Closure_Type);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjMonthClosureRow.Created_By);
                    db.AddInParameter(command, "@TXN_ID", DbType.Int32, ObjMonthClosureRow.TXN_ID);
                    db.AddInParameter(command, "@Financial_Year", DbType.String, ObjMonthClosureRow.Financial_Year);
                    db.AddInParameter(command, "@XMLParamMonthClosureDet", DbType.String, ObjMonthClosureRow.XMLParamMonthClosureDet);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.ExecuteNonQuery(command);

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
