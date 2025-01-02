#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Proforma Master
/// Created By			: Kaliraj K
/// Created Date		: 11-Jul-2010
/// Purpose	            : 

/// <Program Summary>
#endregion
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

namespace S3GDALayer.Origination
{
    namespace ApplicationMgtServices
    {
        public class ClsPubProforma
        {
            int intRowsAffected;
            S3GBusEntity.Origination.ApplicationMgtServices.S3G_ORG_ProformaDataTable ObjProforma_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubProforma()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateProformaDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_ProformaDataTable)
            {
                try
                {
    
                    ObjProforma_DAL = (S3GBusEntity.Origination.ApplicationMgtServices.S3G_ORG_ProformaDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_ProformaDataTable, SerMode, typeof(S3GBusEntity.Origination.ApplicationMgtServices.S3G_ORG_ProformaDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.ApplicationMgtServices.S3G_ORG_ProformaRow ObjProformaRow in ObjProforma_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertProformaDetails");
                        db.AddInParameter(command, "@Proforma_ID", DbType.Int32, ObjProformaRow.Proforma_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjProformaRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjProformaRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjProformaRow.Branch_ID);
                        db.AddInParameter(command, "@Vendor_ID", DbType.Int32, ObjProformaRow.Vendor_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjProformaRow.Customer_ID);
                        db.AddInParameter(command, "@Ref_Doc_Type", DbType.Int32, ObjProformaRow.Ref_Doc_Type);
                        db.AddInParameter(command, "@Ref_Doc_No", DbType.Int32, ObjProformaRow.Ref_Doc_No);
                        db.AddInParameter(command, "@Proforma_No", DbType.String, ObjProformaRow.Proforma_No);
                        db.AddInParameter(command, "@Proforma_Date", DbType.DateTime, ObjProformaRow.Proforma_Date);
                        db.AddInParameter(command, "@Book_in_Asset", DbType.Boolean, ObjProformaRow.Book_in_Asset);
                        db.AddInParameter(command, "@Proforma_Image", DbType.Boolean, ObjProformaRow.Proforma_Image);
                        db.AddInParameter(command, "@TaxReg_No", DbType.String, ObjProformaRow.TaxRegNo);
                        db.AddInParameter(command, "@VAT_No", DbType.String, ObjProformaRow.VATNo);
                        db.AddInParameter(command, "@Scan_Image", DbType.String, ObjProformaRow.ScanPath);
                        db.AddInParameter(command, "@XMLAssetDetails", DbType.String, ObjProformaRow.XMLAssetDetails);
                        db.AddInParameter(command, "@XMLWarrantyDetails", DbType.String, ObjProformaRow.XMLWarrantyDetails);

                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjProformaRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        {
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                        }
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
         }
    }
}
