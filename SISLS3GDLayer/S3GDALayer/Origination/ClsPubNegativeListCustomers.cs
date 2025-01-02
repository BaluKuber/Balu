using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using S3GBusEntity;
using Entity_Origination = S3GBusEntity.Origination;
using System.Data;
using S3GDALayer.S3GAdminServices;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace S3GDALayer.Origination
{
    namespace CashflowMgtServices
    {
        public class ClsPubNegativeListCustomers
        {
            int intErrorCode;
            Entity_Origination.CashflowMgtServices.S3G_ORG_NegCustListDataTable ObjNegCustListDataTable_DAL = null;
            Entity_Origination.CashflowMgtServices.S3G_ORG_NegCustListRow ObjNegCustListRow = null;

            Database db;
            public ClsPubNegativeListCustomers()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateCashFlowRulesInt(SerializationMode SerMode, byte[] bytesObjROIRulesDataTable)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_NegativeListCustomers");

                    ObjNegCustListDataTable_DAL = (Entity_Origination.CashflowMgtServices.S3G_ORG_NegCustListDataTable)ClsPubSerialize.DeSerialize(bytesObjROIRulesDataTable, SerMode, typeof(S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_NegCustListDataTable));
                    ObjNegCustListRow = ObjNegCustListDataTable_DAL.Rows[0] as Entity_Origination.CashflowMgtServices.S3G_ORG_NegCustListRow;


                    db.AddInParameter(command, "@Company_ID", DbType.Int64, ObjNegCustListRow.Company_id);
                    db.AddInParameter(command, "@UserId", DbType.Int64, ObjNegCustListRow.User_Id);
                    db.AddInParameter(command, "@strCustomerList", DbType.String, ObjNegCustListRow.StrCustomerList);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.FunPubExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;

                }
                catch (Exception ex)
                {
                    //intErrorCode = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }

        }
    }
}
