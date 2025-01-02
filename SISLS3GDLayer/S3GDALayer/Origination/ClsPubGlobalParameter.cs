#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: ORG
/// Screen Name			: Global Parameter Creation DAL Class
/// Created By			: Kannan RC
/// Created Date		: 16-Jun-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 
/// Reason              : Orgination Module Developement
/// <Program Summary>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
#endregion

namespace S3GDALayer.Origination
{
    namespace OrgMasterMgtServices
    {
        public class ClsPubGlobalParameter
        {
            #region Initialization
            int intRowsAffected;
            int intMasterID;
            int intRowsMapping;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Global_LookUPDataTable ObjGlobalLookup_Datatable_DAL;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GetGlobalProgramDataTable ObjGlobalProgam_Datatable_DAL;

            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterSetupDataTable ObjGlobalParameter_Datatable_DAL;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterROIRuleDataTable ObjGlobalParameterROI_Datatable_DAL;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBDataTable ObjGlobalParameterLOB_Datatable_DAL;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBNameDataTable ObjGlobalParameterLOBName_Datatable_DAL;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GetGlobalParameterMasterDataTable ObjGlobalParameterMaster_Datatable_DAL;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalLOBDataTable ObjGlobalLOBMaster_Datatable_DAL;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterROIDataTable ObjGlobalROIMaster_Datatable_DAL;


            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubGlobalParameter()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Lookup
            public S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Global_LookUPDataTable FunPubGlobalLookup(SerializationMode Sermode, byte[] bytesGlobalLookup)
            {
                S3GBusEntity.Origination.OrgMasterMgtServices DsGlobal = new S3GBusEntity.Origination.OrgMasterMgtServices();
                ObjGlobalLookup_Datatable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Global_LookUPDataTable)ClsPubSerialize.DeSerialize(bytesGlobalLookup, Sermode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Global_LookUPDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetGlobalLookUp");
                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Global_LookUPRow ObjGlobalRow in ObjGlobalLookup_Datatable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Option", DbType.Int32, ObjGlobalRow.Lookup_ID);
                        db.AddInParameter(command, "@Param1", DbType.String, ObjGlobalRow.Type);
                    }
                    db.FunPubLoadDataSet(command, DsGlobal, DsGlobal.S3G_Global_LookUP.TableName);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsGlobal.S3G_Global_LookUP;
            }
            #endregion

            #region GetGlobalProgram
            public S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GetGlobalProgramDataTable FunPubGetGlobalProgram(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_GlobalProgramDataTable)
            {
                S3GBusEntity.Origination.OrgMasterMgtServices dsGlobalProgramDetails = new S3GBusEntity.Origination.OrgMasterMgtServices();
                ObjGlobalProgam_Datatable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GetGlobalProgramDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_GlobalProgramDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GetGlobalProgramDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetGlobalProgram");
                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GetGlobalProgramRow ObjGlobalProgramRow in ObjGlobalProgam_Datatable_DAL.Rows)
                    {
                        db.FunPubLoadDataSet(command, dsGlobalProgramDetails, dsGlobalProgramDetails.S3G_ORG_GetGlobalProgram.TableName);
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsGlobalProgramDetails.S3G_ORG_GetGlobalProgram;

            }

            #endregion

            #region GetLOBNAME
            public S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBNameDataTable FunPubGetGlobalLOBName(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_GlobalLOBNameDataTable)
            {
                S3GBusEntity.Origination.OrgMasterMgtServices dsGlobalLOBName = new S3GBusEntity.Origination.OrgMasterMgtServices();
                ObjGlobalParameterLOBName_Datatable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBNameDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_GlobalLOBNameDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBNameDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetLOBDetails");
                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBNameRow ObjGlobalNameRow in ObjGlobalParameterLOBName_Datatable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjGlobalNameRow.Company_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjGlobalNameRow.User_ID);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjGlobalNameRow.Is_Active);
                        db.FunPubLoadDataSet(command, dsGlobalLOBName, dsGlobalLOBName.S3G_ORG_GlobalParameterLOBName.TableName);
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsGlobalLOBName.S3G_ORG_GlobalParameterLOBName;

            }

            #endregion

            #region Insert GP
            public int FunPubCreateGlobalParameter(SerializationMode SerMode, byte[] bytesObjS3G_ORG_GlobalParameterMasterDataTable)
            {
                try
                {
                    ObjGlobalParameter_Datatable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterSetupDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_GlobalParameterMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterSetupDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");                    

                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterSetupRow ObjGlobalParameterMasterRow in ObjGlobalParameter_Datatable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertGlobalParameter");
                        db.AddInParameter(command, "@COMPANY_ID", DbType.Int64, ObjGlobalParameterMasterRow.Company_ID);
                        db.AddInParameter(command, "@Modification", DbType.Boolean, ObjGlobalParameterMasterRow.Modification);
                        db.AddInParameter(command, "@Enquiry_Number", DbType.Boolean, ObjGlobalParameterMasterRow.Enquiry_Number);
                        db.AddInParameter(command, "@Offer_Number", DbType.Boolean, ObjGlobalParameterMasterRow.Offer_Number);
                        db.AddInParameter(command, "@CSG_Negative", DbType.Boolean, ObjGlobalParameterMasterRow.NegativeVariance);
                        db.AddInParameter(command, "@Application_Number", DbType.Boolean, ObjGlobalParameterMasterRow.Application_Number);
                        db.AddInParameter(command, "@IRR_Details", DbType.Boolean, ObjGlobalParameterMasterRow.IRR_Details);
                        db.AddInParameter(command, "@Amount", DbType.Decimal, ObjGlobalParameterMasterRow.Amount);
                        db.AddInParameter(command, "@Create_Account", DbType.Boolean, ObjGlobalParameterMasterRow.Create_Account);
                        db.AddInParameter(command, "@CREATED_BY", DbType.Int32, ObjGlobalParameterMasterRow.Created_By);
                        db.AddInParameter(command, "@CREATED_ON", DbType.DateTime, ObjGlobalParameterMasterRow.Created_On);
                        db.AddInParameter(command, "@XML_GLOBDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_GLOBDeltails);
                        db.AddInParameter(command, "@XML_GROIDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_GROIDeltails);
                        db.AddInParameter(command, "@XML_CashierAccounts", DbType.String, ObjGlobalParameterMasterRow.XML_CashierAccounts);
                        db.AddInParameter(command, "@Product_Inflow_Adjust", DbType.Boolean, ObjGlobalParameterMasterRow.Product_Inflow_Adjust);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //db.AddOutParameter(command, "@Master_ID", DbType.Int32, sizeof(Int32));
                        db.FunPubExecuteNonQuery(command);
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                        //intMasterID = (int)command.Parameters["@Master_ID"].Value;

                    }

                    //if (intMasterID > 0)
                    //{
                    //    ObjGlobalParameterLOB_Datatable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_GlobalParameterLOBDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBDataTable));
                    //    Database db1 = DatabaseFactory.CreateDatabase("S3GconnectionString");


                    //    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBRow ObjGlobalParameterLOBRow in ObjGlobalParameterLOB_Datatable_DAL.Rows)
                    //    {
                    //        DbCommand command1 = db1.GetStoredProcCommand("S3G_ORG_InsertGlobalParameterLOB");
                    //        db1.AddInParameter(command1, "@Global_Parameter_Org_ID", DbType.Int32, intMasterID);
                    //        db1.AddInParameter(command1, "@LOB_ID", DbType.Int32, ObjGlobalParameterLOBRow.LOB_ID);
                    //        db1.AddInParameter(command1, "@Invoice_Required", DbType.Boolean, ObjGlobalParameterLOBRow.Invoice_Required);
                    //        db1.AddInParameter(command1, "@Depreciation_Method", DbType.String, ObjGlobalParameterLOBRow.Depreciation_Method);
                    //        db1.AddInParameter(command1, "@Corporate_Tax_Rate", DbType.Decimal, ObjGlobalParameterLOBRow.Corporate_Tax_Rate);
                    //        db1.AddInParameter(command1, "@Prime_Lending_Rate", DbType.Decimal, ObjGlobalParameterLOBRow.Prime_Lending_Rate);
                    //        db1.AddInParameter(command1, "@IS_LOB", DbType.Boolean, ObjGlobalParameterLOBRow.IS_LOB);
                    //        db1.AddInParameter(command1, "@CREATED_BY", DbType.Int32, ObjGlobalParameterLOBRow.Created_By);
                    //        db1.AddInParameter(command1, "@CREATED_ON", DbType.DateTime, ObjGlobalParameterLOBRow.Created_On);
                    //        db1.AddOutParameter(command1, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    //        db1.ExecuteNonQuery(command1);
                    //        if ((int)command1.Parameters["@ErrorCode"].Value > 0)
                    //            intRowsMapping = (int)command1.Parameters["@ErrorCode"].Value;

                    //    }
                    //}

                    //if (intMasterID > 0)
                    //{
                    //    ObjGlobalParameterROI_Datatable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterROIRuleDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_GlobalParameterROIDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterROIRuleDataTable));
                    //    Database db2 = DatabaseFactory.CreateDatabase("S3GconnectionString");


                    //    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterROIRuleRow ObjGlobalParameterROIRow in ObjGlobalParameterROI_Datatable_DAL.Rows)
                    //    {
                    //        DbCommand command2 = db2.GetStoredProcCommand("S3G_ORG_InsertGlobalParameterROIRule");
                    //        db2.AddInParameter(command2, "@Global_Parameter_Org_ID", DbType.Int32, intMasterID);
                    //        db2.AddInParameter(command2, "@Program_ID", DbType.Int32, ObjGlobalParameterROIRow.Program_ID);
                    //        db2.AddInParameter(command2, "@IS_Program", DbType.Boolean, ObjGlobalParameterROIRow.IS_Program);
                    //        db2.AddInParameter(command2, "@CREATED_BY", DbType.Int32, ObjGlobalParameterROIRow.Created_By);
                    //        db2.AddInParameter(command2, "@CREATED_ON", DbType.DateTime, ObjGlobalParameterROIRow.Created_On);
                    //        db2.AddOutParameter(command2, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    //        db2.ExecuteNonQuery(command2);
                    //        if ((int)command2.Parameters["@ErrorCode"].Value > 0)
                    //            intRowsMapping = (int)command2.Parameters["@ErrorCode"].Value;

                    //    }
                    //}
                }
                catch (Exception exp)
                {
                    intRowsAffected = 50;
                    //intRowsMapping = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;
                //return intRowsMapping;
            }

            #endregion


            #region Update GP
            public int FunPubUpdateGlobalParameter(SerializationMode SerMode, byte[] bytesObjS3G_ORG_GlobalParameterMasterDataTable)
            {
                try
                {
                    ObjGlobalParameter_Datatable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterSetupDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_GlobalParameterMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterSetupDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    

                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterSetupRow ObjGlobalParameterMasterRow in ObjGlobalParameter_Datatable_DAL.Rows)
                    {

                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_UpdateGlobalParameter");
                        db.AddInParameter(command, "@Global_Parameter_Org_ID", DbType.Int64, ObjGlobalParameterMasterRow.Global_Parameter_Org_ID);
                        db.AddInParameter(command, "@COMPANY_ID", DbType.Int64, ObjGlobalParameterMasterRow.Company_ID);
                        db.AddInParameter(command, "@Modification", DbType.Boolean, ObjGlobalParameterMasterRow.Modification);
                        db.AddInParameter(command, "@Enquiry_Number", DbType.Boolean, ObjGlobalParameterMasterRow.Enquiry_Number);
                        db.AddInParameter(command, "@Offer_Number", DbType.Boolean, ObjGlobalParameterMasterRow.Offer_Number);
                        db.AddInParameter(command, "@CSG_Negative", DbType.Boolean, ObjGlobalParameterMasterRow.NegativeVariance);
                        db.AddInParameter(command, "@Application_Number", DbType.Boolean, ObjGlobalParameterMasterRow.Application_Number);
                        db.AddInParameter(command, "@IRR_Details", DbType.Boolean, ObjGlobalParameterMasterRow.IRR_Details);
                        db.AddInParameter(command, "@Amount", DbType.Decimal, ObjGlobalParameterMasterRow.Amount);
                        db.AddInParameter(command, "@Create_Account", DbType.Boolean, ObjGlobalParameterMasterRow.Create_Account);
                        db.AddInParameter(command, "@Modified_BY", DbType.Int32, ObjGlobalParameterMasterRow.Modified_By);
                        //db.AddInParameter(command, "@Modified_ON", DbType.DateTime, ObjGlobalParameterMasterRow.Modified_On);
                        db.AddInParameter(command, "@XML_GLOBDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_GLOBDeltails);
                        db.AddInParameter(command, "@XML_GROIDeltails", DbType.String, ObjGlobalParameterMasterRow.XML_GROIDeltails);
                        db.AddInParameter(command, "@XML_CashierAccounts", DbType.String, ObjGlobalParameterMasterRow.XML_CashierAccounts);
                        db.AddInParameter(command, "@Product_Inflow_Adjust", DbType.Boolean, ObjGlobalParameterMasterRow.Product_Inflow_Adjust);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.FunPubExecuteNonQuery(command);
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                        //db.AddOutParameter(command, "@Global_Parameter_Org_ID", DbType.Int32, sizeof(Int32));
                        
                        //intMasterID = (int)command.Parameters["@Global_Parameter_Org_ID"].Value;

                    }

                    //if (intMasterID > 0)
                    //{

                    //    ObjGlobalParameterLOB_Datatable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_GlobalParameterLOBDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBDataTable));
                    //    Database db1 = DatabaseFactory.CreateDatabase("S3GconnectionString");


                    //    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBRow ObjGlobalParameterLOBRow in ObjGlobalParameterLOB_Datatable_DAL.Rows)
                    //    {
                    //        DbCommand command1 = db1.GetStoredProcCommand("S3G_ORG_UpdateGlobalParameterLOB");
                    //        db1.AddInParameter(command1, "@Global_Parameter_Org_ID", DbType.Int32, intMasterID);
                    //        db1.AddInParameter(command1, "@LOB_ID", DbType.Int32, ObjGlobalParameterLOBRow.LOB_ID);
                    //        db1.AddInParameter(command1, "@Invoice_Required", DbType.Boolean, ObjGlobalParameterLOBRow.Invoice_Required);
                    //        db1.AddInParameter(command1, "@Depreciation_Method", DbType.String, ObjGlobalParameterLOBRow.Depreciation_Method);
                    //        db1.AddInParameter(command1, "@Corporate_Tax_Rate", DbType.Decimal, ObjGlobalParameterLOBRow.Corporate_Tax_Rate);
                    //        db1.AddInParameter(command1, "@Prime_Lending_Rate", DbType.Decimal, ObjGlobalParameterLOBRow.Prime_Lending_Rate);
                    //        db1.AddInParameter(command1, "@IS_LOB", DbType.Boolean, ObjGlobalParameterLOBRow.IS_LOB);
                    //        db1.AddInParameter(command1, "@Modified_BY", DbType.Int32, ObjGlobalParameterLOBRow.Modified_By);
                    //        db1.AddInParameter(command1, "@Modified_ON", DbType.DateTime, ObjGlobalParameterLOBRow.Modified_On);
                    //        db1.AddOutParameter(command1, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    //        db1.ExecuteNonQuery(command1);
                    //        if ((int)command1.Parameters["@ErrorCode"].Value > 0)
                    //            intRowsMapping = (int)command1.Parameters["@ErrorCode"].Value;

                    //    }


                    //}

                    //if (intMasterID > 0)
                    //{
                    //    ObjGlobalParameterROI_Datatable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterROIRuleDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_GlobalParameterROIDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterROIRuleDataTable));
                    //    Database db2 = DatabaseFactory.CreateDatabase("S3GconnectionString");


                    //    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterROIRuleRow ObjGlobalParameterROIRow in ObjGlobalParameterROI_Datatable_DAL.Rows)
                    //    {
                    //        DbCommand command2 = db2.GetStoredProcCommand("S3G_ORG_UpdateGlobalParameterROIRule");
                    //        db2.AddInParameter(command2, "@Global_Parameter_Org_ID", DbType.Int32, intMasterID);
                    //        db2.AddInParameter(command2, "@Program_ID", DbType.Int32, ObjGlobalParameterROIRow.Program_ID);
                    //        db2.AddInParameter(command2, "@IS_Program", DbType.Boolean, ObjGlobalParameterROIRow.IS_Program);
                    //        db2.AddInParameter(command2, "Modified_By", DbType.Int32, ObjGlobalParameterROIRow.Modified_By);
                    //        db2.AddInParameter(command2, "@Modified_On", DbType.DateTime, ObjGlobalParameterROIRow.Modified_On);
                    //        db2.AddOutParameter(command2, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    //        db2.ExecuteNonQuery(command2);
                    //        if ((int)command2.Parameters["@ErrorCode"].Value > 0)
                    //            intRowsMapping = (int)command2.Parameters["@ErrorCode"].Value;

                    //    }
                    //}
                }
                catch (Exception exp)
                {
                    intRowsAffected = 50;
                    intRowsMapping = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;
                //return intRowsMapping;
            }

            #endregion


            #region  GetGPS
            public S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GetGlobalParameterMasterDataTable FunPubGlobalParameterMaster(SerializationMode Sermode, byte[] bytesGlobalParameterMaster)
            {
                S3GBusEntity.Origination.OrgMasterMgtServices DsGlobalMaster = new S3GBusEntity.Origination.OrgMasterMgtServices();
                ObjGlobalParameterMaster_Datatable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GetGlobalParameterMasterDataTable)ClsPubSerialize.DeSerialize(bytesGlobalParameterMaster, Sermode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GetGlobalParameterMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetGlobalParameterSetup");
                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GetGlobalParameterMasterRow ObjGlobalMasterRow in ObjGlobalParameterMaster_Datatable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Global_Parameter_Org_ID", DbType.Int32, ObjGlobalMasterRow.Global_Parameter_Org_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjGlobalMasterRow.Company_ID);
                        db.FunPubLoadDataSet(command, DsGlobalMaster, DsGlobalMaster.S3G_ORG_GetGlobalParameterMaster.TableName);
                    }

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsGlobalMaster.S3G_ORG_GetGlobalParameterMaster;
            }
            #endregion

            #region  GetLOBGRID
            public S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalLOBDataTable FunPubGlobalLOBMaster(SerializationMode Sermode, byte[] bytesGlobalLOBMaster)
            {
                S3GBusEntity.Origination.OrgMasterMgtServices DsGlobalMasterLOB = new S3GBusEntity.Origination.OrgMasterMgtServices();
                ObjGlobalLOBMaster_Datatable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalLOBDataTable)ClsPubSerialize.DeSerialize(bytesGlobalLOBMaster, Sermode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalLOBDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetGlobalLOB");
                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalLOBRow ObjGlobalMasterLOBRow in ObjGlobalLOBMaster_Datatable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjGlobalMasterLOBRow.Company_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjGlobalMasterLOBRow.User_ID);
                        db.FunPubLoadDataSet(command, DsGlobalMasterLOB, DsGlobalMasterLOB.S3G_ORG_GlobalLOB.TableName);
                    }

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsGlobalMasterLOB.S3G_ORG_GlobalLOB;
            }


            public S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterROIDataTable FunPubGlobalROIMaster(SerializationMode Sermode, byte[] bytesGlobalROIMaster)
            {
                S3GBusEntity.Origination.OrgMasterMgtServices DsGlobalMasterROI = new S3GBusEntity.Origination.OrgMasterMgtServices();
                ObjGlobalROIMaster_Datatable_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterROIDataTable)ClsPubSerialize.DeSerialize(bytesGlobalROIMaster, Sermode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterROIDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetGlobalModifyProgram");
                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GlobalParameterROIRow ObjGlobalMasterROIRow in ObjGlobalROIMaster_Datatable_DAL.Rows)
                    {

                        db.AddInParameter(command, "@Global_Parameter_Org_ID", DbType.Int32, ObjGlobalMasterROIRow.Global_Parameter_Org_ID);
                        db.FunPubLoadDataSet(command, DsGlobalMasterROI, DsGlobalMasterROI.S3G_ORG_GlobalParameterROI.TableName);
                    }

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsGlobalMasterROI.S3G_ORG_GlobalParameterROI;
            }
            #endregion




        }
    }
}
