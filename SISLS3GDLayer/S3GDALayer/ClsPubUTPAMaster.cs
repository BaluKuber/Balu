#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: UTPA Master Creation DAL Class
/// Created By			: Suresh P
/// Created Date		: 19-May-2010
/// Purpose	            : 
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
#endregion

namespace S3GDALayer
{
    /// Added the Name Space For Logical Grouping
    /// This Class belongs CompanyMgtServices to the service group
    namespace TPAMgtServices
    {
        public class ClsPubUTPAMaster
        {
            #region Initialization
            int intErrorCode;

            S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable ObjUTPAMasterDataTable_DAL;
            S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable ObjUTPAProgramAccessDataTable_DAL;


            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubUTPAMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create New UTPA

            /// <summary>
            /// Creates a new UTPA by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_UTPAMasterDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>
            public int FunPubCreateUTPAInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_UTPAMasterDataTable, out int intUTPAID_Out)
            {
                int intUTPA_ID = 0;
                try
                {
                    ObjUTPAMasterDataTable_DAL = (S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_UTPAMasterDataTable, SerMode, typeof(S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    //foreach (S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterRow ObjUTPAMasterRow in ObjUTPAMasterDataTable_DAL.Rows)
                    //{
                    S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterRow ObjUTPAMasterRow = ObjUTPAMasterDataTable_DAL.Rows[0] as S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterRow;
                    DbCommand command = db.GetStoredProcCommand("S3G_Insert_UTPA_Details");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjUTPAMasterRow.Company_ID);
                    db.AddInParameter(command, "@Region_ID", DbType.Int32, ObjUTPAMasterRow.Region_ID);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjUTPAMasterRow.Branch_ID);
                    db.AddInParameter(command, "@UTPA_Type_ID", DbType.Int32, ObjUTPAMasterRow.UTPA_Type_ID);
                    db.AddInParameter(command, "@UTPA_Code", DbType.String, ObjUTPAMasterRow.UTPA_Code);
                    db.AddInParameter(command, "@UTPA_Name", DbType.String, ObjUTPAMasterRow.UTPA_Name);
                    db.AddInParameter(command, "@UTPA_Login_Code", DbType.String, ObjUTPAMasterRow.UTPA_Login_Code); //Added by saranya on 12-03-2012 to insert UTPA Login Code
                    db.AddInParameter(command, "@Address1", DbType.String, ObjUTPAMasterRow.Address1);
                    db.AddInParameter(command, "@Address2", DbType.String, ObjUTPAMasterRow.Address2);
                    db.AddInParameter(command, "@City", DbType.String, ObjUTPAMasterRow.City);
                    db.AddInParameter(command, "@State", DbType.String, ObjUTPAMasterRow.State);
                    db.AddInParameter(command, "@GL_Code", DbType.String, ObjUTPAMasterRow.GL_Code);
                    db.AddInParameter(command, "@Password", DbType.String, ObjUTPAMasterRow.Password);
                    db.AddInParameter(command, "@Country", DbType.String, ObjUTPAMasterRow.Country);
                    db.AddInParameter(command, "@Pincode", DbType.String, ObjUTPAMasterRow.Pincode);
                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjUTPAMasterRow.Is_Active);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjUTPAMasterRow.Created_By);
                    db.AddInParameter(command, "@Entity_ID", DbType.Int32, ObjUTPAMasterRow.Entity_ID);

                    //Out Parameters
                    db.AddOutParameter(command, "@UTPA_ID", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    //db.ExecuteNonQuery(command);
                    db.FunPubExecuteNonQuery(command);


                    intUTPA_ID = Convert.ToInt32(command.Parameters["@UTPA_ID"].Value);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                    //}
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                intUTPAID_Out = intUTPA_ID;
                return intErrorCode;
            }

            public int FunPubCreateUTPAProgramAccessInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_UTPAProgramAccessDataTable)
            {

                try
                {
                    ObjUTPAProgramAccessDataTable_DAL = (S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_UTPAProgramAccessDataTable, SerMode, typeof(S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    //foreach (S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterRow ObjUTPAMasterRow in ObjUTPAMasterDataTable_DAL.Rows)
                    //{
                    S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessRow ObjUTPAProgramAccessRow = ObjUTPAProgramAccessDataTable_DAL.Rows[0] as S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessRow;

                    DbCommand command = db.GetStoredProcCommand("S3G_Insert_UTPAProgramAccess_Details");

                    db.AddInParameter(command, "@UTPA_ID", DbType.Int32, ObjUTPAProgramAccessRow.UTPA_ID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjUTPAProgramAccessRow.LOB_ID);
                    db.AddInParameter(command, "@Role_Code_ID", DbType.Int32, ObjUTPAProgramAccessRow.Role_Code_ID);
                    db.AddInParameter(command, "@Add_Access", DbType.Boolean, ObjUTPAProgramAccessRow.Add_Access);
                    db.AddInParameter(command, "@Modify_Access", DbType.Boolean, ObjUTPAProgramAccessRow.Modify_Access);
                    db.AddInParameter(command, "@Delete_Access", DbType.Boolean, ObjUTPAProgramAccessRow.Delete_Access);
                    db.AddInParameter(command, "@Query", DbType.Boolean, ObjUTPAProgramAccessRow.Query);
                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjUTPAProgramAccessRow.Is_Active);

                    if (!ObjUTPAProgramAccessRow.IsRole_Center_CodeNull())
                    {
                        db.AddInParameter(command, "@Role_Center_Code", DbType.String, ObjUTPAProgramAccessRow.Role_Center_Code);
                    }
                    if (!ObjUTPAProgramAccessRow.IsRole_Center_NameNull())
                    {
                        db.AddInParameter(command, "@Role_Center_Name", DbType.String, ObjUTPAProgramAccessRow.Role_Center_Name);
                    }
                    if (!ObjUTPAProgramAccessRow.IsRole_CodeNull())
                    {
                        db.AddInParameter(command, "@Role_Code", DbType.String, ObjUTPAProgramAccessRow.Role_Code);
                    }
                    if (!ObjUTPAProgramAccessRow.IsProgram_IDNull())
                    {
                        db.AddInParameter(command, "@Program_ID", DbType.Int32, ObjUTPAProgramAccessRow.Program_ID);
                    }
                    if (!ObjUTPAProgramAccessRow.IsProgram_CodeNull())
                    {
                        db.AddInParameter(command, "@Program_COde", DbType.String, ObjUTPAProgramAccessRow.Program_Code);
                    }
                    if (!ObjUTPAProgramAccessRow.IsProgram_NameNull())
                    {
                        db.AddInParameter(command, "@Program_Name", DbType.String, ObjUTPAProgramAccessRow.Program_Name);
                    }
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    db.FunPubExecuteNonQuery(command);
                    //db.ExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                    //}
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }


            #endregion

            #region Modify UTPA Details

            /// <summary>
            /// Modifies an Exsisting UTPA by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_UTPAMasterDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>
            /// 
            public int FunPubModifyUTPAInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_UTPAMasterDataTable)
            {
                try
                {
                    ObjUTPAMasterDataTable_DAL = (S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_UTPAMasterDataTable, SerMode, typeof(S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterRow ObjUTPAMasterRow = ObjUTPAMasterDataTable_DAL.Rows[0] as S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterRow;

                    DbCommand command = db.GetStoredProcCommand("S3G_Update_UTPA_Details");
                    db.AddInParameter(command, "@UTPA_ID", DbType.Int32, ObjUTPAMasterRow.UTPA_ID);
                    db.AddInParameter(command, "@Password", DbType.String, ObjUTPAMasterRow.Password);
                    db.AddInParameter(command, "@GL_Code", DbType.String, ObjUTPAMasterRow.GL_Code);
                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjUTPAMasterRow.Is_Active);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjUTPAMasterRow.Modified_By);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    db.FunPubExecuteNonQuery(command);
                    //db.ExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }

            public int FunPubModifyUTPAProgramAccessInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_UTPAProgramAccessDataTable)
            {
                try
                {

                    ObjUTPAProgramAccessDataTable_DAL = (S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_UTPAProgramAccessDataTable, SerMode, typeof(S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable));
                    S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessRow ObjUTPAProgramAccessRow = ObjUTPAProgramAccessDataTable_DAL.Rows[0] as S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessRow;

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Update_UTPAProgramAccess_Details");

                    db.AddInParameter(command, "@UTPA_ID", DbType.Int32, ObjUTPAProgramAccessRow.UTPA_ID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjUTPAProgramAccessRow.LOB_ID);
                    db.AddInParameter(command, "@Role_Code_ID", DbType.Int32, ObjUTPAProgramAccessRow.Role_Code_ID);
                    db.AddInParameter(command, "@Add_Access", DbType.Boolean, ObjUTPAProgramAccessRow.Add_Access);
                    db.AddInParameter(command, "@Modify_Access", DbType.Boolean, ObjUTPAProgramAccessRow.Modify_Access);
                    db.AddInParameter(command, "@Delete_Access", DbType.Boolean, ObjUTPAProgramAccessRow.Delete_Access);
                    db.AddInParameter(command, "@Query", DbType.Boolean, ObjUTPAProgramAccessRow.Query);
                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, true);

                    /*if (!ObjUTPAProgramAccessRow.IsRole_Center_CodeNull())
                    {
                        db.AddInParameter(command, "@Role_Center_Code", DbType.String, ObjUTPAProgramAccessRow.Role_Center_Code);
                    }
                    if (!ObjUTPAProgramAccessRow.IsRole_Center_NameNull())
                    {
                        db.AddInParameter(command, "@Role_Center_Name", DbType.String, ObjUTPAProgramAccessRow.Role_Center_Name);
                    }
                    if (!ObjUTPAProgramAccessRow.IsRole_CodeNull())
                    {
                        db.AddInParameter(command, "@Role_Code", DbType.String, ObjUTPAProgramAccessRow.Role_Code);
                    }
                    if (!ObjUTPAProgramAccessRow.IsProgram_IDNull())
                    {
                        db.AddInParameter(command, "@Program_ID", DbType.Int32, ObjUTPAProgramAccessRow.Program_ID);
                    }
                    if (!ObjUTPAProgramAccessRow.IsProgram_CodeNull())
                    {
                        db.AddInParameter(command, "@Program_COde", DbType.String, ObjUTPAProgramAccessRow.Program_Code);
                    }
                    if (!ObjUTPAProgramAccessRow.IsProgram_NameNull())
                    {
                        db.AddInParameter(command, "@Program_Name", DbType.String, ObjUTPAProgramAccessRow.Program_Name);
                    }*/

                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    db.FunPubExecuteNonQuery(command);
                    //db.ExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }


            public int FunPubDeleteUTPAProgramAccessInt(int intUTPA_Prog_Access_ID)
            {
                try
                {

                    //ObjUTPAProgramAccessDataTable_DAL = (S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_UTPAProgramAccessDataTable, SerMode, typeof(S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable));
                    //S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessRow ObjUTPAProgramAccessRow = ObjUTPAProgramAccessDataTable_DAL.Rows[0] as S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessRow;

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Delete_UTPAProgramAccess_Details");

                    db.AddInParameter(command, "@UTPA_Prog_Access_ID", DbType.Int32, intUTPA_Prog_Access_ID);

                    /*if (!ObjUTPAProgramAccessRow.IsRole_Center_CodeNull())
                    {
                        db.AddInParameter(command, "@Role_Center_Code", DbType.String, ObjUTPAProgramAccessRow.Role_Center_Code);
                    }
                    if (!ObjUTPAProgramAccessRow.IsRole_Center_NameNull())
                    {
                        db.AddInParameter(command, "@Role_Center_Name", DbType.String, ObjUTPAProgramAccessRow.Role_Center_Name);
                    }
                    if (!ObjUTPAProgramAccessRow.IsRole_CodeNull())
                    {
                        db.AddInParameter(command, "@Role_Code", DbType.String, ObjUTPAProgramAccessRow.Role_Code);
                    }
                    if (!ObjUTPAProgramAccessRow.IsProgram_IDNull())
                    {
                        db.AddInParameter(command, "@Program_ID", DbType.Int32, ObjUTPAProgramAccessRow.Program_ID);
                    }
                    if (!ObjUTPAProgramAccessRow.IsProgram_CodeNull())
                    {
                        db.AddInParameter(command, "@Program_COde", DbType.String, ObjUTPAProgramAccessRow.Program_Code);
                    }
                    if (!ObjUTPAProgramAccessRow.IsProgram_NameNull())
                    {
                        db.AddInParameter(command, "@Program_Name", DbType.String, ObjUTPAProgramAccessRow.Program_Name);
                    }*/

                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    //db.ExecuteNonQuery(command);
                    db.FunPubExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }

            #endregion

            #region Query UTPA Details

            /// <summary>
            /// Gets a company details using Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjSNXG_UTPAMasterDataTable"></param>
            /// <returns>Datatable containing Company details</returns>

            public S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable FunPubQueryUTPADetails(SerializationMode SerMode, byte[] bytesObjSNXG_UTPAMasterDataTable)
            {
                S3GBusEntity.TPAMgtServices dsUTPADetails = new S3GBusEntity.TPAMgtServices();
                ObjUTPAMasterDataTable_DAL = (S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_UTPAMasterDataTable, SerMode, typeof(S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterRow ObjUTPAMasterRow in ObjUTPAMasterDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Get_UTPA_Details");

                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjUTPAMasterRow.Company_ID);

                        if (!ObjUTPAMasterRow.IsUTPA_IDNull())
                        {
                            db.AddInParameter(command, "@UTPA_ID", DbType.Int32, ObjUTPAMasterRow.UTPA_ID);
                        }
                        //db.LoadDataSet(command, dsUTPADetails, dsUTPADetails.S3G_SYSAD_UTPAMaster.TableName);
                        db.FunPubLoadDataSet(command, dsUTPADetails, dsUTPADetails.S3G_SYSAD_UTPAMaster.TableName);
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsUTPADetails.S3G_SYSAD_UTPAMaster;
            }

            public S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable FunPubQueryUTPAProgramAccessDetails(SerializationMode SerMode, byte[] bytesObjSNXG_UTPAMasterDataTable)
            {

                ObjUTPAProgramAccessDataTable_DAL = (S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_UTPAMasterDataTable, SerMode, typeof(S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable));
                S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessRow ObjUTPAMasterRow = ObjUTPAProgramAccessDataTable_DAL.Rows[0] as S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessRow;

                S3GBusEntity.TPAMgtServices dsutpaDetails = new S3GBusEntity.TPAMgtServices();
                try
                {


                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_UTPAProgramAccess_Details");
                    db.AddInParameter(command, "@UTPA_ID", DbType.Int32, ObjUTPAMasterRow.UTPA_ID);
                    if (!ObjUTPAMasterRow.IsUTPA_Prog_Access_IDNull())
                    {
                        db.AddInParameter(command, "@UTPA_Prog_Access_ID", DbType.Int32, ObjUTPAMasterRow.UTPA_Prog_Access_ID);
                    }
                    db.FunPubLoadDataSet(command, dsutpaDetails, dsutpaDetails.S3G_SYSAD_UTPAProgramAccess.TableName);
                    //db.LoadDataSet(command, dsutpaDetails, dsutpaDetails.S3G_SYSAD_UTPAProgramAccess.TableName);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsutpaDetails.S3G_SYSAD_UTPAProgramAccess;
            }

            public S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable FunPubQueryUTPADetailsPaging(SerializationMode SerMode, byte[] bytesObjSNXG_UTPAMasterDataTable, out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                S3GBusEntity.TPAMgtServices dsUTPADetails = new S3GBusEntity.TPAMgtServices();

                ObjUTPAMasterDataTable_DAL = (S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_UTPAMasterDataTable, SerMode, typeof(S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterRow ObjUTPAMasterRow in ObjUTPAMasterDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Get_UTPA_Paging");

                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPaging.ProCompany_ID);
                        if (!ObjUTPAMasterRow.IsUTPA_IDNull())
                        {
                            db.AddInParameter(command, "@UTPA_ID", DbType.Int32, ObjUTPAMasterRow.UTPA_ID);
                        }
                        db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                        db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                        db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                        db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                        db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));



                        //db.LoadDataSet(command, dsUTPADetails, dsUTPADetails.S3G_SYSAD_UTPAMaster.TableName);
                        db.FunPubLoadDataSet(command, dsUTPADetails, dsUTPADetails.S3G_SYSAD_UTPAMaster.TableName);
                        if ((int)command.Parameters["@TotalRecords"].Value > 0)
                            intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsUTPADetails.S3G_SYSAD_UTPAMaster;
            }


            #endregion
        }
    }
}
