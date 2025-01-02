#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Escalation Creation DAL Class
/// Created By			: Kannan RC
/// Created Date		: 19-May-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 
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
    namespace UserMgtServices
    {

        public class ClsPubEscalation
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable ObjRoleCodeListTable_DAL;
            S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleUser_ListDataTable ObjRoleUserListTable_DAL;
            S3GBusEntity.UserMgtServices.S3G_SYSAD_EscalationMasterDataTable ObjEscalationMasterTable_DAL;
            S3GBusEntity.UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable ObjGetEscalationMasterTable_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubEscalation()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion

            #region RoleCodeList

            public S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable FunPubRoleCodeList(SerializationMode SerMode, byte[] bytesObjSNXG_RoleCodeListDataTable)
            {
                S3GBusEntity.UserMgtServices dsRoleCodeDetails = new S3GBusEntity.UserMgtServices();
                ObjRoleCodeListTable_DAL = (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_RoleCodeListDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCode_ListRow ObjRoleCodeListRow in ObjRoleCodeListTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Get_RoleCode_List");
                        if (!ObjRoleCodeListRow.IsCompany_IDNull())
                        {
                            db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRoleCodeListRow.Company_ID);
                        }
                        db.FunPubLoadDataSet(command, dsRoleCodeDetails, dsRoleCodeDetails.S3G_SYSAD_RoleCode_List.TableName);

                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsRoleCodeDetails.S3G_SYSAD_RoleCode_List;
            }



            public S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleUser_ListDataTable FunPubRoleUserList(SerializationMode SerMode, byte[] bytesObjSNXG_RoleUserListDataTable)
            {
                S3GBusEntity.UserMgtServices dsRoleUserDetails = new S3GBusEntity.UserMgtServices();
                ObjRoleUserListTable_DAL = (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleUser_ListDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_RoleUserListDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleUser_ListDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_RoleUser_List");
                    foreach (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleUser_ListRow ObjRoleUserListRow in ObjRoleUserListTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Role_Code_ID", DbType.Int32, ObjRoleUserListRow.Role_Code_ID);


                        db.FunPubLoadDataSet(command, dsRoleUserDetails, dsRoleUserDetails.S3G_SYSAD_RoleUser_List.TableName);

                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsRoleUserDetails.S3G_SYSAD_RoleUser_List;
            }
            #endregion


            #region Create
            public int FunPubCreateEscalation(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_EscalationMasterDataTable)
            {
                try
                {
                    ObjEscalationMasterTable_DAL = (S3GBusEntity.UserMgtServices.S3G_SYSAD_EscalationMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_EscalationMasterDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_EscalationMasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Insert_EscalationDetails");
                    foreach (S3GBusEntity.UserMgtServices.S3G_SYSAD_EscalationMasterRow ObjEscalationMasterRow in ObjEscalationMasterTable_DAL.Rows)
                    {                        
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjEscalationMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjEscalationMasterRow.LOB_ID);                        
                        //db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjEscalationMasterRow.Created_On);
                        //db.AddInParameter(command, "@Escalation_ID", DbType.Int32, ObjEscalationMasterRow.Escalation_ID);                                                
                        db.AddInParameter(command, "@Role_Code_ID", DbType.Int32, ObjEscalationMasterRow.Role_Code_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjEscalationMasterRow.User_ID);
                        if (!ObjEscalationMasterRow.IsNL_DaysNull())
                        {
                            db.AddInParameter(command, "@NL_Days", DbType.Decimal, ObjEscalationMasterRow.NL_Days);
                            db.AddInParameter(command, "@NL_SMS", DbType.Boolean, ObjEscalationMasterRow.NL_SMS);
                            db.AddInParameter(command, "@NL_Email", DbType.Boolean, ObjEscalationMasterRow.NL_Email);
                            db.AddInParameter(command, "@NL_CC", DbType.Boolean, ObjEscalationMasterRow.NL_CC);
                        }
                        if (!ObjEscalationMasterRow.IsHL_DaysNull())
                        {
                            db.AddInParameter(command, "@HL_Days", DbType.Decimal, ObjEscalationMasterRow.HL_Days);
                            db.AddInParameter(command, "@HL_SMS", DbType.Boolean, ObjEscalationMasterRow.HL_SMS);
                            db.AddInParameter(command, "@HL_Email", DbType.Boolean, ObjEscalationMasterRow.HL_Email);
                            db.AddInParameter(command, "@HL_CC1", DbType.Boolean, ObjEscalationMasterRow.HL_CC1);
                            db.AddInParameter(command, "@HL_CC2", DbType.Boolean, ObjEscalationMasterRow.HL_CC2);
                        }
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjEscalationMasterRow.Is_Active);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjEscalationMasterRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
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

            #region Update Escalation

            public int FunPubModifyEscalation(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_EscalationMasterDataTable)
            {
                try
                {
                    ObjEscalationMasterTable_DAL = (S3GBusEntity.UserMgtServices.S3G_SYSAD_EscalationMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_EscalationMasterDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_EscalationMasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Update_EscalationDetails");
                    foreach (S3GBusEntity.UserMgtServices.S3G_SYSAD_EscalationMasterRow ObjEscalationMasterRow in ObjEscalationMasterTable_DAL.Rows)
                    {

                        db.AddInParameter(command, "@Escalation_ID", DbType.Int32, ObjEscalationMasterRow.Escalation_ID);
                        db.AddInParameter(command, "@Role_Code_ID", DbType.Int32, ObjEscalationMasterRow.Role_Code_ID);
                        if (!ObjEscalationMasterRow.IsNL_DaysNull())
                        {
                            db.AddInParameter(command, "@NL_Days", DbType.Decimal, ObjEscalationMasterRow.NL_Days);
                            db.AddInParameter(command, "@NL_SMS", DbType.Boolean, ObjEscalationMasterRow.NL_SMS);
                            db.AddInParameter(command, "@NL_Email", DbType.Boolean, ObjEscalationMasterRow.NL_Email);
                            db.AddInParameter(command, "@NL_CC", DbType.Boolean, ObjEscalationMasterRow.NL_CC);
                        }
                        if (!ObjEscalationMasterRow.IsHL_DaysNull())
                        {
                            db.AddInParameter(command, "@HL_Days", DbType.Decimal, ObjEscalationMasterRow.HL_Days);
                            db.AddInParameter(command, "@HL_SMS", DbType.Boolean, ObjEscalationMasterRow.HL_SMS);
                            db.AddInParameter(command, "@HL_Email", DbType.Boolean, ObjEscalationMasterRow.HL_Email);
                            db.AddInParameter(command, "@HL_CC1", DbType.Boolean, ObjEscalationMasterRow.HL_CC1);
                            db.AddInParameter(command, "@HL_CC2", DbType.Boolean, ObjEscalationMasterRow.HL_CC2);
                        }
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjEscalationMasterRow.Is_Active);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjEscalationMasterRow.Modified_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
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


            #region Get Escalation

            public S3GBusEntity.UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable FunPubQueryEscalation(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_EscalationMasterDataTable)
            {
                S3GBusEntity.UserMgtServices dsEscalationDetails = new S3GBusEntity.UserMgtServices();
                ObjGetEscalationMasterTable_DAL = (S3GBusEntity.UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_EscalationMasterDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_EscalationDetails");
                    foreach (S3GBusEntity.UserMgtServices.S3G_SYSAD_Get_EscalationDetailsRow ObjEscalationMasterRow in ObjGetEscalationMasterTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Escalation_ID", DbType.Int32, ObjEscalationMasterRow.Escalation_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjEscalationMasterRow.Company_ID);
                        db.FunPubLoadDataSet(command, dsEscalationDetails, dsEscalationDetails.S3G_SYSAD_Get_EscalationDetails.TableName);
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsEscalationDetails.S3G_SYSAD_Get_EscalationDetails;

            }


            public S3GBusEntity.UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable FunPubQueryEscalationPaging(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_EscalationMasterDataTable, out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                S3GBusEntity.UserMgtServices dsEscalationDetails = new S3GBusEntity.UserMgtServices();
                ObjGetEscalationMasterTable_DAL = (S3GBusEntity.UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_EscalationMasterDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Escalation_Paging");
                    foreach (S3GBusEntity.UserMgtServices.S3G_SYSAD_Get_EscalationDetailsRow ObjEscalationMasterRow in ObjGetEscalationMasterTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjEscalationMasterRow.Company_ID);
                        db.AddInParameter(command, "@Escalation_ID", DbType.Int32, ObjEscalationMasterRow.Escalation_ID);
                        db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                        db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                        db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                        db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                        db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));
                        db.FunPubLoadDataSet(command, dsEscalationDetails, dsEscalationDetails.S3G_SYSAD_Get_EscalationDetails.TableName);
                        if ((int)command.Parameters["@TotalRecords"].Value > 0)
                            intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsEscalationDetails.S3G_SYSAD_Get_EscalationDetails;

            }

        #endregion

            #region Delete Escalation
            public int FunPubDeleteEscalation(int inEscalation_ID)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Delete_EscalationDetails");
                    db.AddInParameter(command, "@Escalation_ID", DbType.Int32, inEscalation_ID);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.FunPubExecuteNonQuery(command);
                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;

            }

            #endregion

        }
    }
}
