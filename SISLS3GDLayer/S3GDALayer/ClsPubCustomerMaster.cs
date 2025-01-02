#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Customer Master
/// Created By			: Narayanan
/// Created Date		: 28-05-2010
/// <Program Summary>
#endregion

using System;using S3GDALayer.S3GAdminServices;
using System.IO;
using System.Data;
using System.Text;
using S3GBusEntity;
using System.Data.Common;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Runtime.Serialization.Formatters.Binary;

namespace S3GDALayer
{
    public class ClsPubCustomerMaster
    {
        string _S3G_ORG_GeStatusLookUp = "S3G_ORG_GeStatusLookUp";

        public DataTable FunPub_GetS3GStatusLookUp(S3G_Status_Parameters ObjParam)
        {
            try
            {
                DataSet ObjDS = new DataSet();
                Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetSqlStringCommand(_S3G_ORG_GeStatusLookUp);

                db.AddInParameter(command, "@Option", DbType.Int32, ObjParam.Option);
                db.AddInParameter(command, "@Param1", DbType.String, ObjParam.Param1);
                db.AddInParameter(command, "@Param2", DbType.String, ObjParam.Param2);
                db.AddInParameter(command, "@Param3", DbType.String, ObjParam.Param3);

                db.LoadDataSet(command, ObjDS, _S3G_ORG_GeStatusLookUp);
                return (DataTable)ObjDS.Tables[_S3G_ORG_GeStatusLookUp];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
