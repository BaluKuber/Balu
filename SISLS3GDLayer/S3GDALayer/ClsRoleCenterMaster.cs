 //Module Name1      :   System Admin
/// Screen Name     :   S3GSysAdminRoleCodeMaster_Add
/// Created By      :   Ramesh M
/// Created Date    :   14-May-2010
/// Purpose         :   To insert and update product master details
/// Last updated By :   Gunasekar.K
/// Last Updated On :   12-Oct-2010
/// Purpose         :   To get the Program list based on the Module Code
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
       public class ClsRoleCenterMaster
       {
           #region Initialization
           int intRowsAffected;
           S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable  ObjS3G_RoleCodeMasterDataTable_DAL;
           S3GBusEntity.UserMgtServices.S3G_SYSAD_ModuleMasterDataTable  ObjS3G_MaduleMasterDataTable_DAL;
           S3GBusEntity.UserMgtServices.S3G_SYSAD_ProgramMasterDataTable S3G_SYSAD_ProgramMasterDataTable;
           S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCenterMaster_ListDataTable ObjS3G_RoleCenterMasterList_DataTable_DAL;
           S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable ObjS3G_RoleCodeDataTable_DAL;


           //Code added for getting common connection string  from config file
            Database db;
            public ClsRoleCenterMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

           #endregion
           #region Create Role Center 
           public int FunRoleCodeMasterInsertInt(SerializationMode SerMode, byte[] bytesObjSNXG_RoleCode_MasterDataTable)
           {
               try
               {
                   ObjS3G_RoleCodeMasterDataTable_DAL = (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_RoleCode_MasterDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable));
                   //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                   DbCommand command = db.GetStoredProcCommand("S3G_Insert_RoleCodeMaster_Details");
                   foreach (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterRow  ObjRoleCodeMasterRow in ObjS3G_RoleCodeMasterDataTable_DAL.Rows )
                   {
                       db.AddInParameter(command, "@Role_Center_Code", DbType.String, ObjRoleCodeMasterRow.Role_Center_Code);
                       db.AddInParameter(command, "@Role_Center_Name", DbType.String, ObjRoleCodeMasterRow.Role_Center_Name);
                       db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRoleCodeMasterRow.Company_ID);
                       db.AddInParameter(command, "@Module_ID", DbType.Int32, ObjRoleCodeMasterRow.Module_ID);
                       db.AddInParameter(command, "@Role_Code", DbType.String, ObjRoleCodeMasterRow.Role_Code);
                       db.AddInParameter(command, "@Program_ID", DbType.Int32, ObjRoleCodeMasterRow.Program_ID);
                       if(!ObjRoleCodeMasterRow.IsWFProgram_IdNull())
                       db.AddInParameter(command, "@WFProgram_ID", DbType.Int32, ObjRoleCodeMasterRow.WFProgram_Id);
                       db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjRoleCodeMasterRow.Is_Active);
                       db.AddInParameter(command, "@Created_By", DbType.Int32, ObjRoleCodeMasterRow.Created_By);
                       db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjRoleCodeMasterRow.Modified_By);
                       db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                       db.FunPubExecuteNonQuery(command);
                       if ((int)command.Parameters["@ErrorCode"].Value > 0)
                           intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                   }

               }
               catch (Exception ex)
               {
                   intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
               }
               return intRowsAffected;
           }
           #endregion
           #region Update Role Center
           public int FunPubModifyRoleCodeMaster(SerializationMode SerMode, byte[] bytesObjSNXG_RoleCode_MasterDataTable)
           {
               try
               {
                   ObjS3G_RoleCodeMasterDataTable_DAL = (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_RoleCode_MasterDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable));
                   //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                   DbCommand command = db.GetStoredProcCommand("S3G_Update_RoleCodeMaster_Details");
                   foreach (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterRow ObjRolecodeMasterRow in ObjS3G_RoleCodeMasterDataTable_DAL.Rows)
                   {
                       db.AddInParameter(command, "@Role_Code_ID", DbType.Int32, ObjRolecodeMasterRow.Role_Code_ID);
                       db.AddInParameter(command, "@Role_Center_Code", DbType.String, ObjRolecodeMasterRow.Role_Center_Code);
                       db.AddInParameter(command, "@Role_Center_Name", DbType.String, ObjRolecodeMasterRow.Role_Center_Name);
                       db.AddInParameter(command, "@Module_ID", DbType.Int32, ObjRolecodeMasterRow.Module_ID);
                       db.AddInParameter(command, "@Role_Code", DbType.String, ObjRolecodeMasterRow.Role_Code);
                       db.AddInParameter(command, "@Program_ID", DbType.Int32, ObjRolecodeMasterRow.Program_ID);
                       db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjRolecodeMasterRow.Modified_By);
                       db.AddInParameter(command, "@IS_ACTIVE", DbType.Boolean, ObjRolecodeMasterRow.Is_Active);
                       db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                       db.FunPubExecuteNonQuery(command);
                       if ((int)command.Parameters["@ErrorCode"].Value > 0)
                           intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                   }

               }
               catch (Exception ex)
               {
                   intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
               }
               return intRowsAffected;
           }
           #endregion
           #region Delete Role Code
           public int FunPubDeleteRoleCenterMaster(SerializationMode SerMode, byte[] bytesObjSNXG_RoleCenter_MasterDataTable)
           {
               try
               {
                   ObjS3G_RoleCodeMasterDataTable_DAL = (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_RoleCenter_MasterDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable));
                   //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                   DbCommand command = db.GetStoredProcCommand("[S3G_Delete_RoleCodeMaster_Details]");
                   foreach (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterRow  ObjRoleCodeMasterRow in ObjS3G_RoleCodeMasterDataTable_DAL.Rows)
                   {
                       db.AddInParameter(command, "@Role_Code_ID", DbType.Int32, ObjRoleCodeMasterRow.Role_Code_ID);
                       db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                       db.FunPubExecuteNonQuery(command);
                       if ((int)command.Parameters["@ErrorCode"].Value > 0)
                           intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                   }

               }
               catch (Exception ex)
               {
                   intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

               }
               return intRowsAffected;
           }
           #endregion
           #region Query Role Center Master

           public S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable FunPubQueryRoleCodeMasterDetails(SerializationMode SerMode, byte[] bytesObj_RoleCodeMasterDataTable)
           {
               S3GBusEntity.UserMgtServices DsRoleCodeMaster = new S3GBusEntity.UserMgtServices();
               ObjS3G_RoleCodeMasterDataTable_DAL = (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable)ClsPubSerialize.DeSerialize(bytesObj_RoleCodeMasterDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable));
               try
               {
                   
                   //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                   DbCommand command = db.GetStoredProcCommand("S3G_Get_RoleCodeMaster_Details");
                   foreach (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterRow  ObjRoleCodeMasterRow in ObjS3G_RoleCodeMasterDataTable_DAL.Rows)
                   {
                       if (!ObjRoleCodeMasterRow.IsRole_Center_IDNull())
                       {
                           db.AddInParameter(command, "@Role_Code_ID", DbType.Int32, ObjRoleCodeMasterRow.Role_Center_ID);
                       }
                       if (!ObjRoleCodeMasterRow.IsCompany_IDNull())
                       {
                           db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRoleCodeMasterRow.Company_ID);
                       }
                       if (!ObjRoleCodeMasterRow.IsIs_ActiveNull())
                       {
                           db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjRoleCodeMasterRow.Is_Active);
                       }
                       db.FunPubLoadDataSet(command, DsRoleCodeMaster, DsRoleCodeMaster.S3G_SYSAD_RoleCodeMaster.TableName);
                   }
               }
               catch (Exception ex)
               {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
               }
               return DsRoleCodeMaster.S3G_SYSAD_RoleCodeMaster;

           }
           public S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable FunPubQueryRoleCodeMasterPaging(SerializationMode SerMode, byte[] bytesObjRoleCenterDataTable, out int intTotalRecords, PagingValues ObjPaging)  ///Searching for kali
           {

               intTotalRecords = 0;
               S3GBusEntity.UserMgtServices dsRoleCode = new S3GBusEntity.UserMgtServices();
               //ObjS3G_RoleCenterMasterList_DataTable_DAL = (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_LOBMasterDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable));
               try
               {
                   //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                   DbCommand command = db.GetStoredProcCommand("S3G_Get_RoleCodeMaster_Paging");
                   db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPaging.ProCompany_ID);
                   db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                   db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                   db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                   db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                   db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));

                   db.FunPubLoadDataSet(command, dsRoleCode, dsRoleCode.S3G_SYSAD_RoleCodeMaster.TableName);
                   if ((int)command.Parameters["@TotalRecords"].Value > 0)
                   intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
               }
               catch (Exception ex)
               {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
               }
               return dsRoleCode.S3G_SYSAD_RoleCodeMaster;
           }
           public S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable FunPubCheckRoleCodeMasterDetails(SerializationMode SerMode, byte[] bytesObj_RoleCodeMasterDataTable)
           {
               S3GBusEntity.UserMgtServices DsRoleCodeMaster = new S3GBusEntity.UserMgtServices();
               ObjS3G_RoleCodeMasterDataTable_DAL = (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable)ClsPubSerialize.DeSerialize(bytesObj_RoleCodeMasterDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable));
               try
               {

                   //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                   DbCommand command = db.GetStoredProcCommand("S3G_Chk_RoleCode");
                   foreach (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterRow  ObjRoleCenterMasterRow in ObjS3G_RoleCodeMasterDataTable_DAL.Rows)
                   {
                       if (!ObjRoleCenterMasterRow.IsCompany_IDNull())
                       {
                           db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRoleCenterMasterRow.Company_ID);
                       }
                       if (!ObjRoleCenterMasterRow.IsRole_Center_CodeNull())
                       {
                           db.AddInParameter(command, "@Role_Center_Code", DbType.String, ObjRoleCenterMasterRow.Role_Center_Code);
                       }
                       if (!ObjRoleCenterMasterRow.IsRole_Center_NameNull())
                       {
                           db.AddInParameter(command, "@Role_Center_Name", DbType.String, ObjRoleCenterMasterRow.Role_Center_Name);
                       }
                       db.FunPubLoadDataSet(command,DsRoleCodeMaster,DsRoleCodeMaster.S3G_SYSAD_RoleCodeMaster.TableName);
                   }
               }
               catch (Exception ex)
               {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
               }
               return DsRoleCodeMaster.S3G_SYSAD_RoleCodeMaster;

           }
           #endregion
           #region Query Role Center Master List
          
           #endregion
           #region Query Module Master
           public S3GBusEntity.UserMgtServices.S3G_SYSAD_ModuleMasterDataTable FunPubQueryModuleMasterList(SerializationMode SerMode, byte[] bytesObjSNXG_ModuleMasterDataTable)
           {
               S3GBusEntity.UserMgtServices DsModuleMaster = new S3GBusEntity.UserMgtServices();
               ObjS3G_MaduleMasterDataTable_DAL = (S3GBusEntity.UserMgtServices.S3G_SYSAD_ModuleMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_ModuleMasterDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_ModuleMasterDataTable));
               try
               {
                   //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                   DbCommand command = db.GetStoredProcCommand("S3G_Get_ModuleMaster_Details");
                   foreach (S3GBusEntity.UserMgtServices.S3G_SYSAD_ModuleMasterRow ObjModuleMasterRow in ObjS3G_MaduleMasterDataTable_DAL.Rows)
                   {
                       db.AddInParameter(command, "@MODULE_ID", DbType.Int32, ObjModuleMasterRow.Module_ID);
                       db.FunPubLoadDataSet(command, DsModuleMaster, DsModuleMaster.S3G_SYSAD_ModuleMaster.TableName);
                   }
               }
               catch (Exception ex)
               {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
               }
               return DsModuleMaster.S3G_SYSAD_ModuleMaster;

           }
           #endregion
           #region Query Program Master
           public S3GBusEntity.UserMgtServices.S3G_SYSAD_ProgramMasterDataTable FunPubQueryProgramMasterList(SerializationMode SerMode, byte[] bytesObjSNXG_ProgramMasterDataTable, string sModuleCode)
           {
               S3GBusEntity.UserMgtServices DsProgramMaster = new S3GBusEntity.UserMgtServices();
               S3G_SYSAD_ProgramMasterDataTable = (S3GBusEntity.UserMgtServices.S3G_SYSAD_ProgramMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_ProgramMasterDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_ProgramMasterDataTable));
               try
               {
                   //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                   DbCommand command = db.GetStoredProcCommand("S3G_Get_ProgramMaster_Details");
                   foreach (S3GBusEntity.UserMgtServices.S3G_SYSAD_ProgramMasterRow ObjProgramMasterRow in S3G_SYSAD_ProgramMasterDataTable.Rows)
                   {
                       db.AddInParameter(command, "@PROGRM_ID", DbType.Int32, ObjProgramMasterRow.Program_ID);
                       //--Added by Guna on 12-Oct-2010 To get the programs based on the module code
                       db.AddInParameter(command, "@MODULE_CODE", DbType.String, sModuleCode);
                       //--Endds here
                       db.FunPubLoadDataSet(command, DsProgramMaster, DsProgramMaster.S3G_SYSAD_ProgramMaster.TableName);
                   }
               }
               catch (Exception ex)
               {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
               }
               return DsProgramMaster.S3G_SYSAD_ProgramMaster;

           }
           #endregion
           #region Query Role Code List
           public S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable FunPubQueryRoleCodeList(SerializationMode SerMode, byte[] bytesObjSNXG_RoleCodeListDataTable)
           {
               S3GBusEntity.UserMgtServices dsRoleCodeDetails = new S3GBusEntity.UserMgtServices();
               ObjS3G_RoleCodeDataTable_DAL = (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_RoleCodeListDataTable, SerMode, typeof(S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable));
               try
               {
                   //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                   foreach (S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCode_ListRow ObjRoleCodeListRow in ObjS3G_RoleCodeDataTable_DAL.Rows)
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
           #endregion
       }
    }
}
