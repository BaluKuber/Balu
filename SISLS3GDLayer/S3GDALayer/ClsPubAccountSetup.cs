#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Account Creation DAL Class
/// Created By			: Kannan RC
/// Created Date		: 12-May-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 12-May-2010
/// Reason              : System Admin Module Developement
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

namespace S3GDALayer
{
    namespace AccountMgtServices
    {
        public class ClsPubAccountSetup
        {
            #region Initialization
            int intRowsAffected;            
            S3GBusEntity.AccountMgtServices.S3G_SYSAD_Account_Code_DetailsDataTable ObjAccountDescDataTable_DAL;
            S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountCodeDescDataTable ObjAccountDescIntDataTable_DAL;
            S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable ObjAccountDetailsDataTable_DAL;
            S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountSetupMasterDataTable ObjAccountMasterDetailsDataTable_DAL;


            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubAccountSetup()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion   
                                    
          

            #region Get Account desc

            public S3GBusEntity.AccountMgtServices.S3G_SYSAD_Account_Code_DetailsDataTable FunPubGetAccountDesc(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_AccountDescMasterDataTable)
            {
                S3GBusEntity.AccountMgtServices dsAccDescDetails = new S3GBusEntity.AccountMgtServices();
                ObjAccountDescDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_Account_Code_DetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_AccountDescMasterDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_Account_Code_DetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Account_Code_Details");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_Account_Code_DetailsRow ObjAcctDescMasterRow in ObjAccountDescDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Account_Tab_ID", DbType.Int32, ObjAcctDescMasterRow.Account_Tab_ID);
                        //db.LoadDataSet(command, dsAccDescDetails, dsAccDescDetails.S3G_SYSAD_Account_Code_Details.TableName);
                        db.FunPubLoadDataSet(command, dsAccDescDetails, dsAccDescDetails.S3G_SYSAD_Account_Code_Details.TableName);
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsAccDescDetails.S3G_SYSAD_Account_Code_Details;

            }
            #endregion

            #region Insert Account Desc
            public int FunPubCreateAcctDesc(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_AccountDescDataTable)
            {
                try
                {
                    ObjAccountDescIntDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountCodeDescDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_AccountDescDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountCodeDescDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Insert_Asset_Desc");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountCodeDescRow ObjAcctDescMasterRow in ObjAccountDescIntDataTable_DAL.Rows)
                    {

                        db.AddInParameter(command, "@Account_Tab_ID", DbType.Int32, ObjAcctDescMasterRow.Account_Tab_ID);
                        db.AddInParameter(command, "@Account_Code_Desc", DbType.String, ObjAcctDescMasterRow.Account_Code_Desc);
                        db.AddInParameter(command, "@Account_Flag", DbType.String, ObjAcctDescMasterRow.Account_Flag);                        
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                    }
                }
                catch (Exception exp)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;
            }

            #endregion

            #region GetAccountdetails
            public S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable FunPubGetAccountDetails(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_AccountDetailsMasterDataTable)
            {
                S3GBusEntity.AccountMgtServices dsAccDetails = new S3GBusEntity.AccountMgtServices();
                ObjAccountDetailsDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_AccountDetailsMasterDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Account_Setup_Details");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountDetailsRow ObjAcctDetailsRow in ObjAccountDetailsDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Account_Setup_ID", DbType.Int32, ObjAcctDetailsRow.Account_Setup_ID);
                        db.AddInParameter(command, "@Account_Tab_ID", DbType.Int32, ObjAcctDetailsRow.Account_Tab_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjAcctDetailsRow.Company_ID);
                        //db.LoadDataSet(command, dsAccDetails, dsAccDetails.S3G_SYSAD_AccountDetails.TableName);
                        db.FunPubLoadDataSet(command, dsAccDetails, dsAccDetails.S3G_SYSAD_AccountDetails.TableName);
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsAccDetails.S3G_SYSAD_AccountDetails;

            }

            public S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable FunPubGetAccountDetails(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_AccountDetailsMasterDataTable, out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                S3GBusEntity.AccountMgtServices dsAccDetails = new S3GBusEntity.AccountMgtServices();
                ObjAccountDetailsDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_AccountDetailsMasterDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get__Account_Setup_Details_Paging");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountDetailsRow ObjAcctDetailsRow in ObjAccountDetailsDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Account_Setup_ID", DbType.Int32, ObjAcctDetailsRow.Account_Setup_ID);
                        db.AddInParameter(command, "@Account_Tab_ID", DbType.Int32, ObjAcctDetailsRow.Account_Tab_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjAcctDetailsRow.Company_ID);
                        db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                        db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                        db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                        db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                        db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));               
                        db.FunPubLoadDataSet(command, dsAccDetails, dsAccDetails.S3G_SYSAD_AccountDetails.TableName);
                        //db.LoadDataSet(command, dsAccDetails, dsAccDetails.S3G_SYSAD_AccountDetails.TableName);
                        if ((int)command.Parameters["@TotalRecords"].Value > 0)
                            intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsAccDetails.S3G_SYSAD_AccountDetails;

            }

            #endregion

            #region Create AccountDetails
            public int FunPubCreateAcctDetails(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_AccountDetailsDataTable)
            {
                try
                {
                    ObjAccountMasterDetailsDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountSetupMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_AccountDetailsDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountSetupMasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Insert_AccountSetup");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountSetupMasterRow ObjAcctDetailsMasterRow in ObjAccountMasterDetailsDataTable_DAL.Rows)
                    {

                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjAcctDetailsMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjAcctDetailsMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjAcctDetailsMasterRow.Branch_ID);
                        db.AddInParameter(command, "@Account_Tab_ID", DbType.Int32, ObjAcctDetailsMasterRow.Account_Tab_ID);
                        db.AddInParameter(command, "@Account_Code_Desc_ID", DbType.Int32, ObjAcctDetailsMasterRow.Account_Code_Desc_ID); 
                        db.AddInParameter(command, "@GL_Code", DbType.String, ObjAcctDetailsMasterRow.GL_Code);
                        db.AddInParameter(command, "@SL_Code", DbType.String, ObjAcctDetailsMasterRow.SL_Code);
                        db.AddInParameter(command, "@Level", DbType.String, ObjAcctDetailsMasterRow.Level);                        
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjAcctDetailsMasterRow.Created_By);
                        db.AddInParameter(command, "@Created_On ", DbType.DateTime, ObjAcctDetailsMasterRow.Created_On);
                        db.AddInParameter(command, "@Is_Active ", DbType.Boolean, ObjAcctDetailsMasterRow.Is_Active);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                    }
                }
                catch (Exception exp)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;
            }
            #endregion

            #region Modify AccountSetup
            public int FunPubModifyAcctDetails(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_AccountDetailsDataTable)
            {
                try
                {
                    ObjAccountMasterDetailsDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountSetupMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_AccountDetailsDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountSetupMasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Update_AccountSetup");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountSetupMasterRow ObjAcctDetailsMasterRow in ObjAccountMasterDetailsDataTable_DAL.Rows)
                    {

                        db.AddInParameter(command, "@Account_Setup_ID", DbType.Int32, ObjAcctDetailsMasterRow.Account_Setup_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjAcctDetailsMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjAcctDetailsMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjAcctDetailsMasterRow.Branch_ID);                        
                        db.AddInParameter(command, "@GL_Code", DbType.String, ObjAcctDetailsMasterRow.GL_Code);
                        db.AddInParameter(command, "@SL_Code", DbType.String, ObjAcctDetailsMasterRow.SL_Code);
                        db.AddInParameter(command, "@Level", DbType.String, ObjAcctDetailsMasterRow.Level);
                        db.AddInParameter(command, "@Is_Active ", DbType.Boolean, ObjAcctDetailsMasterRow.Is_Active);
                        db.AddInParameter(command, "@Account_Tab_ID", DbType.Int32, ObjAcctDetailsMasterRow.Account_Tab_ID);
                        db.AddInParameter(command, "@Account_Code_Desc_ID", DbType.Int32, ObjAcctDetailsMasterRow.Account_Code_Desc_ID);                        
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjAcctDetailsMasterRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On ", DbType.DateTime, ObjAcctDetailsMasterRow.Modified_On);                        
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                    }
                }
                catch (Exception exp)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;
            }



            #endregion
        }
    }
}
