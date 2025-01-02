#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Legal & Reposession
/// Screen Name			: Garage Master DAL Class
/// Created By			: Srivatsan.S
/// Created Date		: 20-April-2011
/// Purpose	            : DAL Class for Garage Master methods
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
/// <Revision History>
/// Reprogrammed the module based on the database design provided to us on 21/04/2011
/// <Revision History>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using LegalModule = S3GBusEntity.LegalRepossession;
#endregion

namespace S3GDALayer.LegalRepossession
{
    namespace LegalAndRepossessionMgtServices
    {
        public class ClsPubGarageMaster : ClsPubDalLegalRepossessionBase
        {

            #region Initialization

            int intErrorCode;
            int intRowsAffected;
            LegalModule.LegalRepossessionMgtServices.S3G_LEGAL_Garage_MasterDataTable ObjGarageMasterDataTable_DAL = null;
            LegalModule.LegalRepossessionMgtServices.S3G_LEGAL_Garage_MasterRow ObjGarageMasterRow = null;

            #endregion
            #region Create Garage Master
            /// <summary>
            /// 
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjGarageMasterDataTable"></param>
            /// <returns></returns>
            public int FunPubGarageMasterIns(SerializationMode SerMode, byte[] bytesObjGarageMasterDataTable, out string DSN)
            {
                DSN = string.Empty;
               try
                {
                   
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_InsertGarageMaster);

                    ObjGarageMasterDataTable_DAL = (LegalModule.LegalRepossessionMgtServices.S3G_LEGAL_Garage_MasterDataTable)ClsPubSerialize.DeSerialize(bytesObjGarageMasterDataTable, SerMode, typeof(S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LEGAL_Garage_MasterDataTable));
                    ObjGarageMasterRow = ObjGarageMasterDataTable_DAL.Rows[0] as LegalModule.LegalRepossessionMgtServices.S3G_LEGAL_Garage_MasterRow;
                    
                    db.AddInParameter(command, "@Garage_Name", DbType.String, ObjGarageMasterRow.Garage_Name);                    
                    db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjGarageMasterRow.BranchID);                    
                    db.AddInParameter(command, "@Garage_ID", DbType.Int32, ObjGarageMasterRow.Garage_ID);
                    db.AddInParameter(command, "@Garage_Code", DbType.String, ObjGarageMasterRow.Garage_Code);
                    db.AddInParameter(command, "@Company_id", DbType.Int32, ObjGarageMasterRow.Company_ID);
                    db.AddInParameter(command, "@Garage_owner_id", DbType.String, ObjGarageMasterRow.Garage_Owner_ID);
                    db.AddInParameter(command, "@Garage_owner_Code", DbType.String, ObjGarageMasterRow.Garage_Owner_Code);
                    db.AddInParameter(command, "@GL_Posting_Code", DbType.Int32, ObjGarageMasterRow.GL_Posting_Code);
                    db.AddInParameter(command, "@Square_Feet", DbType.Decimal, ObjGarageMasterRow.Square_Feet);
                    db.AddInParameter(command, "@Rent_Per_SqFeet", DbType.Decimal, ObjGarageMasterRow.Rent_Per_SqFeet);
                    db.AddInParameter(command, "@Frequency_Of_Payment_Type", DbType.Int32, ObjGarageMasterRow.Frequency_Of_Payment_Type);
                    //chandru Modified on 20/03/2012
                    db.AddInParameter(command, "@Frequency_Of_Pay_Type_Code", DbType.Int32, ObjGarageMasterRow.Frequency_Of_Payment_Type_Code);
                    //chandru Modified on 20/03/2012
                    db.AddInParameter(command, "@No_Of_Assets_Garage", DbType.Int32, ObjGarageMasterRow.No_Of_Assets_Garage);
                    db.AddInParameter(command, "@Is_Covered_Parking", DbType.String, ObjGarageMasterRow.Is_Covered_Parking.ToString());
                    db.AddInParameter(command, "@Is_Insurance_cover ", DbType.String, ObjGarageMasterRow.Is_Insurance_Cover.ToString());
                    db.AddInParameter(command, "@Is_Active ", DbType.String, ObjGarageMasterRow.Is_Active.ToString());
                    db.AddInParameter(command, "@Insurance_company", DbType.String, ObjGarageMasterRow.Insurance_Company);
                    db.AddInParameter(command, "@Insurance_Company_Address1", DbType.String, ObjGarageMasterRow.Insurance_Company_Address1);
                    db.AddInParameter(command, "@Insurance_Company_Address2", DbType.String, ObjGarageMasterRow.Insurance_Company_Address2);
                    db.AddInParameter(command, "@Insurance_Company_City", DbType.String, ObjGarageMasterRow.Insurance_Company_City);
                    db.AddInParameter(command, "@Insurance_Company_State", DbType.String, ObjGarageMasterRow.Insurance_Company_State);
                    db.AddInParameter(command, "@Insurance_Company_Pin", DbType.String, ObjGarageMasterRow.Insurance_Company_Pin);
                    db.AddInParameter(command, "@Insurance_Company_Country", DbType.String, ObjGarageMasterRow.Insurance_Company_Country);
                    db.AddInParameter(command, "@Insurance_Company_Telephone", DbType.String, ObjGarageMasterRow.Insurance_Company_Telephone);
                    if (ObjGarageMasterRow.Insurance_Company_Mobile != 0)
                    {
                        db.AddInParameter(command, "@Insurance_Company_Mobile", DbType.Decimal, ObjGarageMasterRow.Insurance_Company_Mobile);
                    }
                    db.AddInParameter(command, "@Insurance_Company_Email_ID", DbType.String, ObjGarageMasterRow.Insurance_Company_Email_ID);
                    db.AddInParameter(command, "@Insurance_Company_Web_Site", DbType.String, ObjGarageMasterRow.Insurance_Company_Web_Site);

                    db.AddInParameter(command, "@Policy_No", DbType.String, ObjGarageMasterRow.Policy_No);
                    db.AddInParameter(command, "@Policy_Amount", DbType.Decimal, ObjGarageMasterRow.Policy_Amount);
                    db.AddInParameter(command, "@Remarks", DbType.String, ObjGarageMasterRow.Remarks);
                    db.AddInParameter(command, "@Garage_Address1", DbType.String, ObjGarageMasterRow.Garage_Address1);
                    db.AddInParameter(command, "@Garage_Address2", DbType.String, ObjGarageMasterRow.Garage_Address2);
                    db.AddInParameter(command, "@Garage_City", DbType.String, ObjGarageMasterRow.Garage_City);
                    db.AddInParameter(command, "@Garage_State", DbType.String, ObjGarageMasterRow.Garage_State);
                    db.AddInParameter(command, "@Garage_Pin", DbType.String, ObjGarageMasterRow.Garage_Pin);
                    db.AddInParameter(command, "@Garage_Country", DbType.String, ObjGarageMasterRow.Garage_Country);
                    db.AddInParameter(command, "@Garage_Telephone", DbType.String, ObjGarageMasterRow.Garage_Telephone);
                    if (ObjGarageMasterRow.Garage_Mobile != 0)
                    {
                        db.AddInParameter(command, "@Garage_Mobile", DbType.Decimal, ObjGarageMasterRow.Garage_Mobile);
                    }
                    db.AddInParameter(command, "@Garage_Email_ID", DbType.String, ObjGarageMasterRow.Garage_Email_ID);
                    db.AddInParameter(command, "@Garage_Web_Site", DbType.String, ObjGarageMasterRow.Garage_Web_Site);
                    db.AddInParameter(command, "@Contact_Person_Name", DbType.String, ObjGarageMasterRow.Contact_Person_Name);
                    db.AddInParameter(command, "@Contact_Person_Address1", DbType.String, ObjGarageMasterRow.Contact_Person_Address1);
                    db.AddInParameter(command, "@Contact_Person_Address2", DbType.String, ObjGarageMasterRow.Contact_Person_Address2);
                    db.AddInParameter(command, "@Contact_Person_City", DbType.String, ObjGarageMasterRow.Contact_Person_City);
                    db.AddInParameter(command, "@Contact_Person_State", DbType.String, ObjGarageMasterRow.Contact_Person_State);
                    db.AddInParameter(command, "@Contact_Person_Pin", DbType.String, ObjGarageMasterRow.Contact_Person_Pin);
                    db.AddInParameter(command, "@Contact_Person_Country", DbType.String, ObjGarageMasterRow.Contact_Person_Country);
                    db.AddInParameter(command, "@Contact_Person_Telephone", DbType.String, ObjGarageMasterRow.Contact_Person_Telephone);
                    if (ObjGarageMasterRow.Contact_Person_Mobile != 0)
                    {
                        db.AddInParameter(command, "@Contact_Person_Mobile", DbType.Decimal, ObjGarageMasterRow.Contact_Person_Mobile);
                    }
                    db.AddInParameter(command, "@Contact_Person_Email_ID", DbType.String, ObjGarageMasterRow.Contact_Person_Email_ID);
                    db.AddInParameter(command, "@Contact_Person_Web_Site", DbType.String, ObjGarageMasterRow.Contact_Person_Web_Site);
                    //chandru Modified on 20/03/2012
                    db.AddInParameter(command, "@Contact_Add_Same_Owners", DbType.String, ObjGarageMasterRow.Contact_Address_Same_As_Owners.ToString());
                    //chandru Modified on 20/03/2012
                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjGarageMasterRow.Created_By);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjGarageMasterRow.Modified_By);
                    db.AddOutParameter(command,"@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@DSNO", DbType.String, 100);
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            //Chandru Changed On 20/03/2012
                            db.FunPubExecuteNonQuery(command, ref trans);
                            //db.FunPubExecuteNonQuery(command);
                            //Chandru Changed On 20/03/2012
                            //if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                            if (intErrorCode == 0)
                            {
                                DSN = (string)command.Parameters["@DSNO"].Value;
                                trans.Commit();
                            }
                            else
                            {
                                trans.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction. 
                            trans.Rollback();
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        conn.Close();
                    }
                }
               catch (Exception ex)
               {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
               }
               return intErrorCode;
            }

            #endregion

            /// <summary>
            /// To get the PA_SA_Ref_ID
            /// </summary>
            /// <param name="Asset_Verification_No">Pass PAN and SAN number</param>
            /// <returns></returns>

            public DataSet FunGarageMasterforModify(int GarageID, int CompanyID)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_GetGarageAccountdetails);
                    db.AddInParameter(command, "@Company_id", DbType.Int32, CompanyID);
                    db.AddInParameter(command, "@Garage_ID", DbType.Int32, GarageID);
                    //Chandru Changed On 20/03/2012
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    //Chandru Changed On 20/03/2012
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public DataSet FunGarageAccountDetails(int GarageID, int CompanyID,int Mode)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_GetAccountdetails);
                    db.AddInParameter(command, "@COMPANYID", DbType.Int32, CompanyID);
                    db.AddInParameter(command, "@GARAGEID", DbType.Int32, GarageID);
                    db.AddInParameter(command, "@MODE", DbType.Int32, Mode);
                    //Chandru Changed On 20/03/2012
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    //Chandru Changed On 20/03/2012
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public DataSet FunGetGarageMaster(int CompanyID)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_GetGarageMaster);
                    db.AddInParameter(command, "@Company_id", DbType.Int32, CompanyID);
                    //Chandru Changed On 20/03/2012
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    //Chandru Changed On 20/03/2012
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public int FunPubDeleteGarageDetails(SerializationMode SerMode, byte[] bytesObjS3G_LR_GarageDataTable)
            {
                try
                {

                    ObjGarageMasterDataTable_DAL = (LegalModule.LegalRepossessionMgtServices.S3G_LEGAL_Garage_MasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LR_GarageDataTable, SerMode, typeof(LegalModule.LegalRepossessionMgtServices.S3G_LEGAL_Garage_MasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_DelGarageMaster);
                    foreach (LegalModule.LegalRepossessionMgtServices.S3G_LEGAL_Garage_MasterRow ObjGarageRow in ObjGarageMasterDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_id", DbType.Int32, ObjGarageMasterRow.Company_ID);
                        db.AddInParameter(command, "@Garage_ID", DbType.Int32, ObjGarageMasterRow.Garage_ID);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //Chandru Changed On 20/03/2012
                        db.FunPubExecuteNonQuery(command);
                        //Chandru Changed On 20/03/2012

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                    }

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;

            }
        }
    }
}
