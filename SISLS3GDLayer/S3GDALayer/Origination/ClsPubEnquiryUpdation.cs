using System;
using S3GDALayer.S3GAdminServices;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using S3GBusEntity.Origination;

namespace S3GDALayer
{
    namespace EnquiryMgtServices
    {
        [Serializable]
        public class ClsPubEnquiryUpdation
        {
            #region Initialization


            S3GBusEntity.Origination.EnquiryService.S3G_SYSAD_GlobalParameterSetupDataTable ObjGlobalParamPincodeMasterDataTable_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubEnquiryUpdation()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            public S3GBusEntity.Origination.EnquiryService.S3G_ORG_DocumentNumberControlDataTable FunPriGetDocumentNumberControlEnquiryNoDetails(int CompanyID, string docTypeDesc, int LOB_ID, int Branch_ID)
            {

                S3GBusEntity.Origination.EnquiryService dsEnqDocControl = new S3GBusEntity.Origination.EnquiryService();

                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetDocumentControlNo");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, CompanyID);
                    db.AddInParameter(command, "@Document_Type_Code", DbType.String, docTypeDesc);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, LOB_ID);
                    db.AddInParameter(command, "@Branch_ID", DbType.Int32, Branch_ID);
                    //db.LoadDataSet(command, dsEnqDocControl, dsEnqDocControl.S3G_ORG_DocumentNumberControl.TableName);
                    db.FunPubLoadDataSet(command, dsEnqDocControl, dsEnqDocControl.S3G_ORG_DocumentNumberControl.TableName);

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsEnqDocControl.S3G_ORG_DocumentNumberControl;

            }

            public S3GBusEntity.Origination.EnquiryService.S3G_SYSAD_GlobalParameterSetupDataTable FunPubQueryPinCodeGlobalParamListEnquiryUpdation(int intCompanyId, SerializationMode SerMode, byte[] bytesObjSNXG_GlobalParamPicodeMasterDataTable)
            {
                S3GBusEntity.Origination.EnquiryService dsGlobalParamPincodeDetails = new S3GBusEntity.Origination.EnquiryService();
                ObjGlobalParamPincodeMasterDataTable_DAL = (S3GBusEntity.Origination.EnquiryService.S3G_SYSAD_GlobalParameterSetupDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_GlobalParamPicodeMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.EnquiryService.S3G_SYSAD_GlobalParameterSetupDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_GlobalParam_Details_Enquiry_Updation");



                    //foreach (S3GBusEntity.Origination.EnquiryService.S3G_SYSAD_GlobalParameterSetupRow ObjGlobalParamPincodeMasterRow in ObjGlobalParamPincodeMasterDataTable_DAL.Rows)
                    //{
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompanyId);
                    //db.LoadDataSet(command, dsGlobalParamPincodeDetails, dsGlobalParamPincodeDetails.S3G_SYSAD_GlobalParameterSetup.TableName);
                    db.FunPubLoadDataSet(command, dsGlobalParamPincodeDetails, dsGlobalParamPincodeDetails.S3G_SYSAD_GlobalParameterSetup.TableName);

                    //    }



                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsGlobalParamPincodeDetails.S3G_SYSAD_GlobalParameterSetup;
            }

            public int FunPubInsertEnquiryUpdation(SerializationMode SMode, byte[] byteEnquiryService, out string strEntityCode_Out)
            {
                int intRowsAffected = 0;
                string strEntityCode = "";
                try
                {
                    EnquiryService.S3G_ORG_EnquiryUpdationDataTable ObjS3G_ORG_EnquiryUpdationDataTable = new EnquiryService.S3G_ORG_EnquiryUpdationDataTable();
                    ObjS3G_ORG_EnquiryUpdationDataTable = (EnquiryService.S3G_ORG_EnquiryUpdationDataTable)ClsPubSerialize.DeSerialize(byteEnquiryService, SMode, typeof(EnquiryService.S3G_ORG_EnquiryUpdationDataTable));
                    EnquiryService.S3G_ORG_EnquiryUpdationRow ObjEnquiryServiceRow = (EnquiryService.S3G_ORG_EnquiryUpdationRow)ObjS3G_ORG_EnquiryUpdationDataTable.Rows[0];

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");


                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_EnquiryUpdation_Insert");
                    db.AddInParameter(command, "@EnquiryNo", DbType.String, ObjEnquiryServiceRow.Enquiry_No);
                    db.AddInParameter(command, "@EnquiryDate", DbType.DateTime, ObjEnquiryServiceRow.Enquiry_Date);
                    db.AddInParameter(command, "@IsNewCustomer", DbType.String, ObjEnquiryServiceRow.Is_NewCustomer);
                    if (!ObjEnquiryServiceRow.IsCustomer_IDNull())
                    {
                        db.AddInParameter(command, "@Customer_ID", DbType.Int64, ObjEnquiryServiceRow.Customer_ID);
                    }
                    db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjEnquiryServiceRow.Constitution_ID);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjEnquiryServiceRow.Company_ID);
                    db.AddInParameter(command, "@FacilityType", DbType.Int32, ObjEnquiryServiceRow.Facility_Type);
                    db.AddInParameter(command, "@FacilityAmount", DbType.Decimal, ObjEnquiryServiceRow.Facility_Amount);
                    db.AddInParameter(command, "@Currency_ID", DbType.Int32, ObjEnquiryServiceRow.Currency_ID);
                    if (!ObjEnquiryServiceRow.IsAsset_IDNull())
                        db.AddInParameter(command, "@Asset_ID", DbType.Int32, ObjEnquiryServiceRow.Asset_ID);
                    if (!ObjEnquiryServiceRow.IsAsset_TypeNull())
                        db.AddInParameter(command, "@Asset_Type", DbType.Int16, ObjEnquiryServiceRow.Asset_Type);
                    db.AddInParameter(command, "@Remarks", DbType.String, ObjEnquiryServiceRow.Remarks);
                    db.AddInParameter(command, "@Tenure", DbType.Int32, ObjEnquiryServiceRow.Tenure);
                    db.AddInParameter(command, "@TenureType", DbType.Int32, ObjEnquiryServiceRow.Tenure_Type_ID);
                    db.AddInParameter(command, "@InterestType", DbType.Int32, ObjEnquiryServiceRow.Interest_Type_ID);
                    db.AddInParameter(command, "@InterestPercentage", DbType.Decimal, ObjEnquiryServiceRow.Interest_Percentage);
                    if (!ObjEnquiryServiceRow.IsMargin_AmountNull())
                        db.AddInParameter(command, "@MarginAmount", DbType.Decimal, ObjEnquiryServiceRow.Margin_Amount);

                    if (!ObjEnquiryServiceRow.IsResidual_ValueNull())
                        db.AddInParameter(command, "@ResidualValue", DbType.Decimal, ObjEnquiryServiceRow.Residual_Value);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjEnquiryServiceRow.Created_By);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjEnquiryServiceRow.Modified_By);

                    /* Code Added By Ganapathy on 28/Sep/2013 BEGINS */

                    if (ObjEnquiryServiceRow.Is_NewCustomer == true)
                    {

                        db.AddInParameter(command, "@Customer_Name", DbType.String, ObjEnquiryServiceRow.Customer_Name);
                        db.AddInParameter(command, "@Address", DbType.String, ObjEnquiryServiceRow.Address);
                        db.AddInParameter(command, "@City", DbType.String, ObjEnquiryServiceRow.City);
                        db.AddInParameter(command, "@State", DbType.String, ObjEnquiryServiceRow.State);
                        db.AddInParameter(command, "@Country", DbType.String, ObjEnquiryServiceRow.Country);
                        db.AddInParameter(command, "@PINCode", DbType.String, ObjEnquiryServiceRow.PINCode_ZipCode);
                        db.AddInParameter(command, "@Mobile", DbType.String, ObjEnquiryServiceRow.Mobile);
                        db.AddInParameter(command, "@EMail", DbType.String, ObjEnquiryServiceRow.EMail);

                    }
                    /* Code Added By Ganapathy on 28/Sep/2013 ENDS */


                    db.AddOutParameter(command, "@EnquiryNo1", DbType.String, 100);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, 4);
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            //db.ExecuteNonQuery(command);
                            db.FunPubExecuteNonQuery(command);

                            strEntityCode = (string)command.Parameters["@EnquiryNo1"].Value;
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
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                }

                strEntityCode_Out = strEntityCode;
                return intRowsAffected;

            }

            public S3GBusEntity.Origination.EnquiryService.S3G_ORG_ExistCustomerMasterDataTable FunPubQueryExistCustomerListEnquiryUpdation(int intCustomerID, int intCompanyId)
            {

                S3GBusEntity.Origination.EnquiryService dsExistCustomer = new S3GBusEntity.Origination.EnquiryService();

                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Exist_Customer_Details_Enquiry_Updation");
                    db.AddInParameter(command, "@CustomerID", DbType.String, intCustomerID);
                    db.AddInParameter(command, "@CompanyID", DbType.Int32, intCompanyId);

                    //db.LoadDataSet(command, dsExistCustomer, dsExistCustomer.S3G_ORG_ExistCustomerMaster.TableName);
                    db.FunPubLoadDataSet(command, dsExistCustomer, dsExistCustomer.S3G_ORG_ExistCustomerMaster.TableName);

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsExistCustomer.S3G_ORG_ExistCustomerMaster;


            }

            public S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryUpdationDataTable FunPubQueryEnquiryUpdation(int intEnquiryUpdationId, int intCompanyId)
            {

                S3GBusEntity.Origination.EnquiryService dsEnquiryUpdation = new S3GBusEntity.Origination.EnquiryService();

                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Org_GetEnquiryUpdation");
                    db.AddInParameter(command, "@EnquiryUpdationId", DbType.String, intEnquiryUpdationId);
                    db.AddInParameter(command, "@CompanyID", DbType.Int32, intCompanyId);

                    //db.LoadDataSet(command, dsEnquiryUpdation, dsEnquiryUpdation.S3G_ORG_EnquiryUpdation.TableName);
                    db.FunPubLoadDataSet(command, dsEnquiryUpdation, dsEnquiryUpdation.S3G_ORG_EnquiryUpdation.TableName);

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsEnquiryUpdation.S3G_ORG_EnquiryUpdation;


            }

            public int FunPubModifyEnquiryUpdation(SerializationMode SMode, byte[] byteEnquiryService)
            {
                int intRowsAffected = 50;

                try
                {
                    EnquiryService.S3G_ORG_EnquiryUpdationDataTable ObjS3G_ORG_EnquiryUpdationDataTable = new EnquiryService.S3G_ORG_EnquiryUpdationDataTable();
                    ObjS3G_ORG_EnquiryUpdationDataTable = (EnquiryService.S3G_ORG_EnquiryUpdationDataTable)ClsPubSerialize.DeSerialize(byteEnquiryService, SMode, typeof(EnquiryService.S3G_ORG_EnquiryUpdationDataTable));
                    EnquiryService.S3G_ORG_EnquiryUpdationRow ObjEnquiryServiceRow = (EnquiryService.S3G_ORG_EnquiryUpdationRow)ObjS3G_ORG_EnquiryUpdationDataTable.Rows[0];

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_EnquiryUpdation_Modify");

                    db.AddInParameter(command, "@EnquiryUpdationId", DbType.Int32, ObjEnquiryServiceRow.EnquiryUpdation_ID);
                    db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjEnquiryServiceRow.Company_ID);
                    db.AddInParameter(command, "@CustomerID", DbType.Int32, ObjEnquiryServiceRow.Customer_ID);
                    db.AddInParameter(command, "@Address", DbType.String, ObjEnquiryServiceRow.Address);
                    db.AddInParameter(command, "@Address2", DbType.String, ObjEnquiryServiceRow.Address2);


                    /* Code Added By Ganapathy on 28/Sep/2013 BEGINS */

                    if (ObjEnquiryServiceRow.Is_NewCustomer == true)
                    {

                        db.AddInParameter(command, "@Customer_Name", DbType.String, ObjEnquiryServiceRow.Customer_Name);
                        //db.AddInParameter(command, "@Address", DbType.String, ObjEnquiryServiceRow.Address);
                        db.AddInParameter(command, "@City", DbType.String, ObjEnquiryServiceRow.City);
                        db.AddInParameter(command, "@State", DbType.String, ObjEnquiryServiceRow.State);
                        db.AddInParameter(command, "@Country", DbType.String, ObjEnquiryServiceRow.Country);
                        db.AddInParameter(command, "@PINCode", DbType.String, ObjEnquiryServiceRow.PINCode_ZipCode);
                        db.AddInParameter(command, "@Mobile", DbType.String, ObjEnquiryServiceRow.Mobile);
                        db.AddInParameter(command, "@EMail", DbType.String, ObjEnquiryServiceRow.EMail);

                    }
                    /* Code Added By Ganapathy on 28/Sep/2013 ENDS */

                    db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjEnquiryServiceRow.Constitution_ID);
                    db.AddInParameter(command, "@FacilityType", DbType.Int32, ObjEnquiryServiceRow.Facility_Type);
                    db.AddInParameter(command, "@FacilityAmount", DbType.Decimal, ObjEnquiryServiceRow.Facility_Amount);
                    db.AddInParameter(command, "@Currency_ID", DbType.Int32, ObjEnquiryServiceRow.Currency_ID);
                    if (!ObjEnquiryServiceRow.IsAsset_IDNull())
                        db.AddInParameter(command, "@Asset_ID", DbType.Int32, ObjEnquiryServiceRow.Asset_ID);
                    if (!ObjEnquiryServiceRow.IsAsset_TypeNull())
                        db.AddInParameter(command, "@Asset_Type", DbType.Int16, ObjEnquiryServiceRow.Asset_Type);
                    db.AddInParameter(command, "@Remarks", DbType.String, ObjEnquiryServiceRow.Remarks);
                    db.AddInParameter(command, "@Tenure", DbType.Int32, ObjEnquiryServiceRow.Tenure);
                    db.AddInParameter(command, "@TenureType", DbType.Int32, ObjEnquiryServiceRow.Tenure_Type_ID);
                    db.AddInParameter(command, "@InterestType", DbType.Int32, ObjEnquiryServiceRow.Interest_Type_ID);
                    db.AddInParameter(command, "@InterestPercentage", DbType.Decimal, ObjEnquiryServiceRow.Interest_Percentage);
                    if (!ObjEnquiryServiceRow.IsMargin_AmountNull())
                        db.AddInParameter(command, "@MarginAmount", DbType.Decimal, ObjEnquiryServiceRow.Margin_Amount);

                    if (!ObjEnquiryServiceRow.IsResidual_ValueNull())
                        db.AddInParameter(command, "@ResidualValue", DbType.Decimal, ObjEnquiryServiceRow.Residual_Value);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjEnquiryServiceRow.Modified_By);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, 4);

                    //db.ExecuteNonQuery(command);
                    db.FunPubExecuteNonQuery(command);


                    // if ((int)command.Parameters["@ErrorCode"].Value > 0)
                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;


                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                }


                return intRowsAffected;

            }
        }

    }
}
