#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Template Master
/// Created By			: M.Saran
/// Created Date		: 30-Jul-2012
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
    namespace DocMgtServices
    {

        public class ClsPubTemplateMaster
        {

            #region Initialization
            int intRowsAffected;
            S3GBusEntity.DocMgtServices.S3G_SYSAD_TemplateDtlsDataTable objS3G_SYSAD_TemplateDtlsDataTable;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubTemplateMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region "Create Template"

            public int FunPubCreateTemplate(SerializationMode SerMode, byte[] bytesS3G_SYSAD_TemplateDtls, out string strTemplateNo)
            {
                strTemplateNo = "";
                try
                {
                   
                    objS3G_SYSAD_TemplateDtlsDataTable = (S3GBusEntity.DocMgtServices.S3G_SYSAD_TemplateDtlsDataTable)ClsPubSerialize.DeSerialize(bytesS3G_SYSAD_TemplateDtls, SerMode, typeof(S3GBusEntity.DocMgtServices.S3G_SYSAD_TemplateDtlsDataTable));
                    foreach (S3GBusEntity.DocMgtServices.S3G_SYSAD_TemplateDtlsRow ObjTmpDtlsRow in objS3G_SYSAD_TemplateDtlsDataTable.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_SYSAD_Ins_Template");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTmpDtlsRow.Company_ID);
                        db.AddInParameter(command, "@Tmp_Doc_Id", DbType.Int32, ObjTmpDtlsRow.Tmp_Doc_Id);
                        if (!ObjTmpDtlsRow.IsTmp_Doc_NoNull())
                            db.AddInParameter(command, "@Tmp_Doc_No ", DbType.String, ObjTmpDtlsRow.Tmp_Doc_No);
                        if (ObjTmpDtlsRow.LOB_Id > 0)
                            db.AddInParameter(command, "@LOB_Id", DbType.Int32, ObjTmpDtlsRow.LOB_Id);
                        if (ObjTmpDtlsRow.Location_Id > 0)
                            db.AddInParameter(command, "@Location_Id", DbType.Int32, ObjTmpDtlsRow.Location_Id);
                        if (ObjTmpDtlsRow.Template_Type_Code != -1)
                            db.AddInParameter(command, "@Template_Type_Code", DbType.Int32, ObjTmpDtlsRow.Template_Type_Code);
                        if (ObjTmpDtlsRow.Mode_of_Mail_Type_Code != -1)
                            db.AddInParameter(command, "@Mode_of_Mail_Type_Code", DbType.Int32, ObjTmpDtlsRow.Mode_of_Mail_Type_Code);
                        if (!ObjTmpDtlsRow.IsDoc_PathNull())
                            db.AddInParameter(command, "@Doc_Path ", DbType.String, ObjTmpDtlsRow.Doc_Path);
                        db.AddInParameter(command, "@Template_language", DbType.String, ObjTmpDtlsRow.Template_language);
                        db.AddInParameter(command, "@Template_Content", DbType.String, ObjTmpDtlsRow.Template_Content);
                        db.AddInParameter(command, "@Translated_Content", DbType.String, ObjTmpDtlsRow.Translated_Content);
                        db.AddInParameter(command, "@Template_Desc", DbType.String, ObjTmpDtlsRow.Template_Desc);
                        db.AddInParameter(command, "@XMLTemplate_Excl", DbType.String, ObjTmpDtlsRow.XMLTemplate_Excl);
                        db.AddInParameter(command, "@Is_Active ", DbType.Boolean, ObjTmpDtlsRow.Is_Active);
                        db.AddInParameter(command, "@Created_By ", DbType.Int32, ObjTmpDtlsRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@DSNO", DbType.String, 50);
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                strTemplateNo = (string)command.Parameters["@DSNO"].Value.ToString();
                                if (intRowsAffected == 0)
                                {
                                    trans.Commit();
                                }
                                else
                                    trans.Rollback();

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
                }
                return intRowsAffected;
            }


            #endregion



        }
    }
}
