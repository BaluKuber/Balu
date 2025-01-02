#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: DoC Path Screen
/// Created By			: Nataraj Y
/// Created Date		: 28-May-2010
/// Purpose	            : 

/// <Program Summary>
#endregion

#region NameSpaces
using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
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
    /// Added the Name Space For Logical Grouping
    /// This Class belongs CompanyMgtServices to the service group
    namespace DocMgtServices
    {

        public class ClsPubDocumentPath
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationPathDataTable ObjS3G_SYSAD_DocumentationPathDataTable_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubDocumentPath()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create New Document Path
            /// <summary>
            /// 
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesS3G_SYSAD_DocumentationPath"></param>
            /// <returns></returns>
            public int FunPubCreateDocumentPathInt(SerializationMode SerMode, byte[] bytesS3G_SYSAD_DocumentationPath)
            {
                try
                {
                    ObjS3G_SYSAD_DocumentationPathDataTable_DAL = (S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationPathDataTable)ClsPubSerialize.DeSerialize(bytesS3G_SYSAD_DocumentationPath, SerMode, typeof(S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationPathDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationPathRow ObjDocPathRow in ObjS3G_SYSAD_DocumentationPathDataTable_DAL.Rows)
                     {
                         DbCommand command = db.GetStoredProcCommand("S3G_Insert_DocPath_Details");
                         db.AddInParameter(command,"@Document_Path",DbType.String, ObjDocPathRow.Document_Path);
                         db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjDocPathRow.LOB_ID);
                         db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjDocPathRow.Company_ID);
                         db.AddInParameter(command, "@Role_Code_ID", DbType.Int32, ObjDocPathRow.Role_Code_ID);
                         db.AddInParameter(command, "@Document_Flag_ID", DbType.Int32, ObjDocPathRow.Document_Flag_ID);
                         db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjDocPathRow.Is_Active);
                         db.AddInParameter(command, "@Created_By", DbType.Int32, ObjDocPathRow.Created_By);
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
                }
                 return intRowsAffected;
            }
            #endregion

            #region Modify Exsisting Document Path
            /// <summary>
            /// 
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesS3G_SYSAD_DocumentationPath"></param>
            /// <returns></returns>
            public int FunPubModifyDocumentPathInt(SerializationMode SerMode, byte[] bytesS3G_SYSAD_DocumentationPath)
            {
                try
                {
                    ObjS3G_SYSAD_DocumentationPathDataTable_DAL = (S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationPathDataTable)ClsPubSerialize.DeSerialize(bytesS3G_SYSAD_DocumentationPath, SerMode, typeof(S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationPathDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationPathRow ObjDocPathRow in ObjS3G_SYSAD_DocumentationPathDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Update_DocPath_Details");
                        db.AddInParameter(command, "@Document_Path_ID", DbType.String, ObjDocPathRow.Document_Path_ID);
                        db.AddInParameter(command, "@Document_Path", DbType.String, ObjDocPathRow.Document_Path);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjDocPathRow.LOB_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjDocPathRow.Company_ID);
                        db.AddInParameter(command, "@Role_Code_ID", DbType.Int32, ObjDocPathRow.Role_Code_ID);
                        db.AddInParameter(command, "@Document_Flag_ID", DbType.Int32, ObjDocPathRow.Document_Flag_ID);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjDocPathRow.Is_Active);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjDocPathRow.Modified_By);
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
                }
                return intRowsAffected;
            }
            #endregion

            #region GetDocFlag List
            /// <summary>
            /// 
            /// </summary>
            /// <param name="intDocument_Flag_ID"></param>
            /// <returns></returns>
            public S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationFlagDataTable FunPubGetDocFlagList(int intDocument_Flag_ID)
            {
                S3GBusEntity.DocMgtServices dsDocFlagDetails = new S3GBusEntity.DocMgtServices();

                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_DocumentationFlag_List");

                    db.AddInParameter(command, "@Document_Flag_ID", DbType.Int32, intDocument_Flag_ID);

                    db.FunPubLoadDataSet(command, dsDocFlagDetails, dsDocFlagDetails.S3G_SYSAD_DocumentationFlag.TableName);


                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsDocFlagDetails.S3G_SYSAD_DocumentationFlag;
            }

            public S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationPathDataTable FunPubGetDocPathPaging(out int intTotalRecords, PagingValues ObjPaging)
            {
                S3GBusEntity.DocMgtServices dsDocPathDetails = new S3GBusEntity.DocMgtServices();
                intTotalRecords = 0;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_DocumentationPath_Paging");

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPaging.ProCompany_ID);
                    db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                    db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                    db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                    db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                    db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));
                    db.FunPubLoadDataSet(command, dsDocPathDetails, dsDocPathDetails.S3G_SYSAD_DocumentationPath.TableName);
                    if ((int)command.Parameters["@TotalRecords"].Value > 0)
                        intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsDocPathDetails.S3G_SYSAD_DocumentationPath;
            }
            #endregion

            #region GetDocFlag List
            /// <summary>
            /// 
            /// </summary>
            /// <param name="intDocument_Flag_ID"></param>
            /// <returns></returns>
            public S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationPathDataTable FunPubGetDocPathDetails(int intCompany_Id,int intDocument_Path_ID)
            {
                S3GBusEntity.DocMgtServices dsDocPathDetails = new S3GBusEntity.DocMgtServices();

                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_DocumentationPath_details");
                    db.AddInParameter(command, "@Document_Path_ID", DbType.Int32, intDocument_Path_ID);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_Id);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.FunPubLoadDataSet(command, dsDocPathDetails, dsDocPathDetails.S3G_SYSAD_DocumentationPath.TableName);

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsDocPathDetails.S3G_SYSAD_DocumentationPath;
            }
            #endregion
        }
    }
}
