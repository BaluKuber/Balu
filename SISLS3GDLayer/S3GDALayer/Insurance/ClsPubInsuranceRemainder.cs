using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System;using S3GDALayer.S3GAdminServices;
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
namespace S3GDALayer.Insurance
{
    namespace InsuranceMgtServices
    {

        public class ClsPubInsuranceRemainder : ClsPubDalInsuranceBase
        {
            #region Initialization
            int intErrorCode;
            S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable objAssetInsuranceEntry_DAL;
            #endregion

            #region InsuranceRemainder
            public int FunPubModifyInsDetails(SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetInsuranceEntry_DataTable)
            {
                
                try
                {
                    objAssetInsuranceEntry_DAL = (S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable)
                        ClsPubSerialize.DeSerialize(byteObjS3G_Insurance_AssetInsuranceEntry_DataTable, SerMode,
                        typeof(S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryRow objAssetEntryRow in objAssetInsuranceEntry_DAL)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_INS_UpdateInsuranceDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objAssetEntryRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objAssetEntryRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objAssetEntryRow.Branch_ID);
                        db.AddInParameter(command, "@XMLACCOUNTDETAILS", DbType.String, objAssetEntryRow.XmlPolicyDetails);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.ExecuteNonQuery(command);
                       // db.FunPubExecuteNonQuery(command);
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                    }
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
