#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: LoanAdmin
/// Screen Name			: Lease Asset Sale DAL class
/// Created By			: Irsathameen K
/// Created Date		: 28-Jul-2010
/// Purpose	            : To access Lease Asset Sale  db methods

/// <Program Summary>
#endregion

using System;
using S3GDALayer.S3GAdminServices;
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
using System.Data.OracleClient;


namespace S3GDALayer.LoanAdmin
{
    namespace AssetMgtServices
    {

        public class ClsPubLeaseAssetSale
        {
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_LeaseAssetSaleDataTable ObjLeaseAssetSale_DAL;
            S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LAD_LASDTLDataTable ObjLAS_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubLeaseAssetSale()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateLeaseAssetSaleDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_LeaseAssetSaleDataTable, out string strLASNo)
            {
                try
                {
                    strLASNo = "";
                    ObjLeaseAssetSale_DAL = (S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_LeaseAssetSaleDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_LeaseAssetSaleDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_LeaseAssetSaleDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_LeaseAssetSaleRow ObjLeaseAssetSaleRow in ObjLeaseAssetSale_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertLeaseAssetSaleDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjLeaseAssetSaleRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjLeaseAssetSaleRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjLeaseAssetSaleRow.Branch_ID);
                        if (!ObjLeaseAssetSaleRow.IsCustomer_IDNull())
                            db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjLeaseAssetSaleRow.Customer_ID);
                        if (!ObjLeaseAssetSaleRow.IsPANumNull())
                            db.AddInParameter(command, "@PANum", DbType.String, ObjLeaseAssetSaleRow.PANum);
                        if (!ObjLeaseAssetSaleRow.IsSANumNull())
                            db.AddInParameter(command, "@SANum", DbType.String, ObjLeaseAssetSaleRow.SANum);
                        if (!ObjLeaseAssetSaleRow.IsLeaseAssetSales_Type_CodeNull())
                            db.AddInParameter(command, "@LeaseAssetSales_Type_Code", DbType.Int32, ObjLeaseAssetSaleRow.LeaseAssetSales_Type_Code);
                        db.AddInParameter(command, "@LeaseAssetSales_Code", DbType.Int32, ObjLeaseAssetSaleRow.LeaseAssetSales_Code);
                        db.AddInParameter(command, "@LeaseAssetSales_Date", DbType.DateTime, ObjLeaseAssetSaleRow.LeaseAssetSales_Date);
                        if (!ObjLeaseAssetSaleRow.IsEntity_Type_CodeNull())
                            db.AddInParameter(command, "@Entity_Type_Code", DbType.Int32, ObjLeaseAssetSaleRow.Entity_Type_Code);
                        if (!ObjLeaseAssetSaleRow.IsEntity_TypeNull())
                            db.AddInParameter(command, "@Entity_Type", DbType.Int32, ObjLeaseAssetSaleRow.Entity_Type);
                        if (!ObjLeaseAssetSaleRow.IsLeaseAssetSales_Invoice_IDNull())
                            db.AddInParameter(command, "@LeaseAssetSales_Invoice_ID", DbType.Int32, ObjLeaseAssetSaleRow.LeaseAssetSales_Invoice_ID);
                        if (!ObjLeaseAssetSaleRow.IsAccount_Closure_DateNull())
                        { db.AddInParameter(command, "@Account_Closure_Date", DbType.DateTime, ObjLeaseAssetSaleRow.Account_Closure_Date); }
                        if (!ObjLeaseAssetSaleRow.IsLeaseAssetSales_StatusType_CodeNull())
                        { db.AddInParameter(command, "@LASales_StatusType_Code", DbType.Int32, ObjLeaseAssetSaleRow.LeaseAssetSales_StatusType_Code); }
                        if (!ObjLeaseAssetSaleRow.IsLeaseAssetSales_Status_CodeNull())
                        { db.AddInParameter(command, "@LeaseAssetSales_Status_Code", DbType.Int32, ObjLeaseAssetSaleRow.LeaseAssetSales_Status_Code); }
                        if (!ObjLeaseAssetSaleRow.IsAsset_IDNull())
                            db.AddInParameter(command, "@Asset_ID", DbType.Int32, ObjLeaseAssetSaleRow.Asset_ID);
                        if (!ObjLeaseAssetSaleRow.IsLANNoNull())
                            db.AddInParameter(command, "@LANNo", DbType.String, ObjLeaseAssetSaleRow.LANNo);
                        if (!ObjLeaseAssetSaleRow.IsNo_Of_UnitsNull())
                            db.AddInParameter(command, "@No_Of_Units", DbType.Int32, ObjLeaseAssetSaleRow.No_Of_Units);
                        if (!ObjLeaseAssetSaleRow.IsResidual_ValueNull())
                            db.AddInParameter(command, "@Residual_Value", DbType.Decimal, ObjLeaseAssetSaleRow.Residual_Value);
                        if (!ObjLeaseAssetSaleRow.IsActual_ValuesNull())
                        { db.AddInParameter(command, "@Actual_Values", DbType.Decimal, ObjLeaseAssetSaleRow.Actual_Values); }
                        if (!ObjLeaseAssetSaleRow.IsService_TaxNull())
                        { db.AddInParameter(command, "@Service_Tax", DbType.Decimal, ObjLeaseAssetSaleRow.Service_Tax); }
                        if (!ObjLeaseAssetSaleRow.IsVATNull())
                            db.AddInParameter(command, "@VAT", DbType.Decimal, ObjLeaseAssetSaleRow.VAT);
                        if (!ObjLeaseAssetSaleRow.IsCustomer_NameNull())
                            db.AddInParameter(command, "@Customer_Name", DbType.String, ObjLeaseAssetSaleRow.Customer_Name);
                        if (!ObjLeaseAssetSaleRow.IsAddress1Null())
                            db.AddInParameter(command, "@Address1", DbType.String, ObjLeaseAssetSaleRow.Address1);
                        if (!ObjLeaseAssetSaleRow.IsAddress2Null())
                            db.AddInParameter(command, "@Address2", DbType.String, ObjLeaseAssetSaleRow.Address2);
                        if (!ObjLeaseAssetSaleRow.IsCityNull())
                            db.AddInParameter(command, "@City", DbType.String, ObjLeaseAssetSaleRow.City);
                        if (!ObjLeaseAssetSaleRow.IsStateNull())
                            db.AddInParameter(command, "@State", DbType.String, ObjLeaseAssetSaleRow.State);
                        if (!ObjLeaseAssetSaleRow.IsCountryNull())
                            db.AddInParameter(command, "@Country", DbType.String, ObjLeaseAssetSaleRow.Country);
                        if (!ObjLeaseAssetSaleRow.IsPincodeNull())
                            db.AddInParameter(command, "@Pincode", DbType.String, ObjLeaseAssetSaleRow.Pincode);
                        if (!ObjLeaseAssetSaleRow.IsTelephoneNull())
                            db.AddInParameter(command, "@Telephone", DbType.String, ObjLeaseAssetSaleRow.Telephone);
                        if (!ObjLeaseAssetSaleRow.IsMobileNull())
                            db.AddInParameter(command, "@Mobile", DbType.String, ObjLeaseAssetSaleRow.Mobile);
                        if (!ObjLeaseAssetSaleRow.IsEmailNull())
                            db.AddInParameter(command, "@Email", DbType.String, ObjLeaseAssetSaleRow.Email);
                        db.AddInParameter(command, "@LAS_ID", DbType.Int32, ObjLeaseAssetSaleRow.LAS_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjLeaseAssetSaleRow.Created_By);
                        db.AddOutParameter(command, "@LeaseAssetSales_No", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else if ((int)command.Parameters["@ErrorCode"].Value < 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    if (intRowsAffected == -1)
                                        throw new Exception("Document Sequence no not-defined");
                                    if (intRowsAffected == -2)
                                        throw new Exception("Document Sequence no exceeds defined limit");
                                }
                                else
                                {
                                    trans.Commit();
                                    strLASNo = Convert.ToString(command.Parameters["@LeaseAssetSales_No"].Value);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();
                            }
                            finally
                            { conn.Close(); }
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


            public int FunPubCreateLASDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_LeaseAssetSaleDataTable, out string strLASNo)
            {
                try
                {
                    strLASNo = "";
                    ObjLAS_DAL = (S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LAD_LASDTLDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_LeaseAssetSaleDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LAD_LASDTLDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LAD_LASDTLRow ObjLASRow in ObjLAS_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LAD_InsertLeaseAstSale_OPC");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjLASRow.Company_ID);
                        db.AddInParameter(command, "@LAS_ID", DbType.Int64, ObjLASRow.LAS_ID);
                        db.AddInParameter(command, "@Lob_ID", DbType.Int32, ObjLASRow.Lob_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjLASRow.Location_ID);
                        db.AddInParameter(command, "@LAS_Type", DbType.Int32, ObjLASRow.LAS_Type);
                        db.AddInParameter(command, "@LAS_Date", DbType.DateTime, ObjLASRow.LAS_Date);
                        db.AddInParameter(command, "@LAS_Number", DbType.String, ObjLASRow.LAS_Number);
                        db.AddInParameter(command, "@Entity_Type", DbType.Int32, ObjLASRow.Entity_Type);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int64, ObjLASRow.Customer_ID);
                        if (!ObjLASRow.IsFile_PathNull())
                            db.AddInParameter(command, "@File_Path", DbType.String, ObjLASRow.File_Path);
                        if (!ObjLASRow.IsXML_TransferDtlNull())
                            db.AddInParameter(command, "@XML_TransferDtl", DbType.String, ObjLASRow.XML_TransferDtl);
                        if (!ObjLASRow.IsXML_DisposeDtlNull())
                            db.AddInParameter(command, "@XML_DisposeDtl", DbType.String, ObjLASRow.XML_DisposeDtl);

                        db.AddInParameter(command, "@Due_Date", DbType.DateTime, ObjLASRow.Due_Date);

                        if (!ObjLASRow.IsXML_PasaRefDtlNull())
                        {
                            S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param;
                                param = new OracleParameter("@XML_PasaRefDtl", OracleType.Clob,
                                    ObjLASRow.XML_PasaRefDtl.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjLASRow.XML_PasaRefDtl);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XML_PasaRefDtl", DbType.String, ObjLASRow.XML_PasaRefDtl);
                            }
                        }

                        if (!ObjLASRow.IsTranche_IDNull())
                            db.AddInParameter(command, "@Tranche_ID", DbType.Int64, ObjLASRow.Tranche_ID);

                        if (!ObjLASRow.IsAggregate_AmountNull())
                            db.AddInParameter(command, "@Aggregate_Amount", DbType.Double, ObjLASRow.Aggregate_Amount);

                        if (!ObjLASRow.IsAsset_FlagNull())
                            db.AddInParameter(command, "@Asset_Flag", DbType.Int32, ObjLASRow.Asset_Flag);

                        db.AddInParameter(command, "@TCS_Not_App", DbType.Int32, ObjLASRow.TCS_Not_App);

                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjLASRow.Created_By);
                        db.AddOutParameter(command, "@LeaseAssetSales_No", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else if ((int)command.Parameters["@ErrorCode"].Value < 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    if (intRowsAffected == -1)
                                        throw new Exception("Document Sequence no not-defined");
                                    if (intRowsAffected == -2)
                                        throw new Exception("Document Sequence no exceeds defined limit");
                                }
                                else
                                {
                                    trans.Commit();
                                    strLASNo = Convert.ToString(command.Parameters["@LeaseAssetSales_No"].Value);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();
                            }
                            finally
                            { conn.Close(); }
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
