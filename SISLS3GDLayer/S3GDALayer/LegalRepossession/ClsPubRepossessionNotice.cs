#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: LegalRepossession
/// Screen Name			: Repossession Notice DAL Class
/// Created By			: Muni Kavitha
/// Created Date		: 22-Apr-2011
/// Purpose	            : DAL Class for Repossession Notice Methods
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

namespace S3GDALayer.LegalRepossession
{
    namespace LegalAndRepossessionMgtServices
    {
        public class ClsPubRepossessionNotice : ClsPubDalLegalRepossessionBase
        {
            #region Initialization

            int intErrorCode=0;


            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices dsRepossessionNotice = null;

            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoticeDataTable ObjRepossessionNoticeDataTable_DAL = null;
            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoticeRow ObjRepossessionNoticeRow = null;

           

            #endregion

            #region Create Repossession Notice
            /// <summary>
            /// Insert Repossession Notice Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjRepossessionNoticeDataTable"></param>
            /// <returns></returns>
            public int FunPubCreateRepossessionNotice(SerializationMode SerMode, byte[] bytesObjRepossessionNoticeDataTable, out string strRN_No, out int strRN_ID)
            {
                strRN_No = "";
                strRN_ID = 0;
                try
                {

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LR_InsertReposseionNotice);
                    ObjRepossessionNoticeDataTable_DAL = (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoticeDataTable)ClsPubSerialize.DeSerialize(bytesObjRepossessionNoticeDataTable, SerMode, typeof(S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoticeDataTable));
                    ObjRepossessionNoticeRow = ObjRepossessionNoticeDataTable_DAL.Rows[0] as S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoticeRow;

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRepossessionNoticeRow.Company_ID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjRepossessionNoticeRow.LOB_ID);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjRepossessionNoticeRow.Branch_ID);
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjRepossessionNoticeRow.Customer_ID);
                    db.AddInParameter(command, "@LRN_ID", DbType.Int32, ObjRepossessionNoticeRow.LRN_ID);
                    db.AddInParameter(command, "@LRN_No", DbType.String, ObjRepossessionNoticeRow.LRN_No);
                    db.AddInParameter(command, "@FollowUp_Description", DbType.String, ObjRepossessionNoticeRow.Follow_Up_Description);
                    if (ObjRepossessionNoticeRow.RN_ID != 0)
                        db.AddInParameter(command, "@RN_ID", DbType.Int32, ObjRepossessionNoticeRow.RN_ID);
                    if ((ObjRepossessionNoticeRow.RN_No != "") || (ObjRepossessionNoticeRow.RN_No != null))
                        db.AddInParameter(command, "@RN_No", DbType.String, ObjRepossessionNoticeRow.RN_No);

                    db.AddInParameter(command, "@PANum", DbType.String, ObjRepossessionNoticeRow.PANum);
                    if ((ObjRepossessionNoticeRow.SANum != "") || (ObjRepossessionNoticeRow.SANum != null))
                        db.AddInParameter(command, "@SANum", DbType.String, ObjRepossessionNoticeRow.SANum);
                    db.AddInParameter(command, "@RN_Date", DbType.DateTime, ObjRepossessionNoticeRow.Repossession_Notice_Date);

                    //db.AddInParameter(command, "@FollowUp_Detail_ID", DbType.Int32, ObjRepossessionNoticeRow.FollowUp_Detail_ID);

                    db.AddInParameter(command, "@XMLParamtRepossesNoticeDet", DbType.String, ObjRepossessionNoticeRow.XMLParamtRepossesNoticeDet);




                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjRepossessionNoticeRow.Created_By);
                    db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjRepossessionNoticeRow.Created_On);

                    db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjRepossessionNoticeRow.Modified_By);
                    db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjRepossessionNoticeRow.Modified_On);

                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@strRN_No", DbType.String, 100);
                    db.AddOutParameter(command, "@strRN_ID", DbType.Int32, sizeof(Int32));

                    ////        db.ExecuteNonQuery(command);

                    ////        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                    ////            intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                    ////        if ((string)command.Parameters["@strRN_No"].Value !="")
                    ////            strRN_No = Convert.ToString(command.Parameters["@strRN_No"].Value);
                    ////        if ((int)command.Parameters["@strRN_ID"].Value > 0)
                    ////            strRN_ID = (int)command.Parameters["@strRN_ID"].Value;

                    ////    }
                    ////    catch (Exception ex)
                    ////    {
                    ////         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    ////    }
                    ////    return intErrorCode;
                    ////}
                    ////#endregion

                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            //Chandru modified on 22/03/2012
                            db.FunPubExecuteNonQuery(command, ref trans);
                            //Chandru modified on 22/03/2012

                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            {
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                            }
                            else
                            {
                                strRN_No = Convert.ToString(command.Parameters["@strRN_No"].Value);
                                strRN_ID = (int)command.Parameters["@strRN_ID"].Value;
                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (intErrorCode == 0)
                                intErrorCode = 50;
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
                    intErrorCode = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                }
                return intErrorCode;

            #endregion


            }


            //#region To Get RepossessionNotice Details

            //public S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoticeDataTable FunPubQueryRepossessionNotice(SerializationMode SerMode, byte[] bytesObjRepossessionNoticeDataTable)
            //{
            //    dsRepossessionNotice = new S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices();
            //    ObjRepossessionNoticeDataTable_DAL = (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoticeDataTable)ClsPubSerialize.DeSerialize(bytesObjRepossessionNoticeDataTable, SerMode, typeof(S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoticeDataTable));

              
            //    Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
            //    DbCommand command = db.GetStoredProcCommand("S3g_LR_GetRepossessNoticeDetails");
            //    try
            //    {
            //        foreach (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionNoticeRow ObjLegalRepossessionNoticeRow in ObjRepossessionNoticeDataTable_DAL.Rows)
            //        {
            //            db.AddInParameter(command, "@Company_ID", DbType.Int32 , ObjLegalRepossessionNoticeRow .Company_ID);
            //            db.AddInParameter(command, "@RN_ID", DbType.Int32, ObjLegalRepossessionNoticeRow.RN_ID);
            //            db.LoadDataSet(command, dsRepossessionNotice, dsRepossessionNotice.S3G_LR_RepossessionNotice.TableName);
            //        }
            //    }
            //    catch (Exception exp)
            //    {
            //         ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
            //    }
            //    return dsRepossessionNotice.S3G_LR_RepossessionNotice;
            //}
            //#endregion

        }
    }
}
