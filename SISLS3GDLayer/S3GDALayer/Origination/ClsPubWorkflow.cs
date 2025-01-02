#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Workflow Creation DAL CLass
/// Created By			: Suresh P
/// Created Date		: 05-Jul-2010
/// Purpose	            : 
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
using Entity_Origination = S3GBusEntity.Origination;

#endregion

namespace S3GDALayer.Origination
{
    namespace OrgMasterMgtServices
    {
        public class ClsPubWorkflow
        {
            #region Initialization
            bool blnErrorCode = false;
            int intErrorCode;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubWorkflow()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }



            #endregion

            #region  Workflow
            public bool FunPubGetIsWorkFlowSupported(int intCompanyID, int intLOBID, int intProductID, int intModuleID, int intProgramID)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_GET_WorkFlowIsSupported);

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompanyID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, intLOBID);
                    db.AddInParameter(command, "@Product_ID", DbType.Int32, intProductID);
                    db.AddInParameter(command, "@Module_ID", DbType.Int32, intModuleID);
                    db.AddInParameter(command, "@Program_ID", DbType.Int32, intProgramID);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.ExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value == 0)
                        blnErrorCode = true;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return blnErrorCode;
            }
            #endregion

        }
    }
}
