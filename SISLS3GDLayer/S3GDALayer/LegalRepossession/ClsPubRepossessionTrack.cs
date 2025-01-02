
using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORG = S3GBusEntity.Origination;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using S3GBusEntity;
using System.Data;


namespace S3GDALayer.LegalRepossession
{
    namespace LegalAndRepossessionMgtServices
    {
        public class ClsPubRepossessionTrack : ClsPubDalLegalRepossessionBase
        {

            int intRowsAffected = 0;

            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionTrackDataTable objS3G_LR_Track_DAL;
             public int FunPubCreateLegalRepossessionTrack(SerializationMode SerMode, byte[] bytesTrack_Datatable, out string strErrMsg)
            {
                strErrMsg = string.Empty;
                try
                {
                    objS3G_LR_Track_DAL = (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionTrackDataTable)ClsPubSerialize.DeSerialize(bytesTrack_Datatable, SerMode, typeof(S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionTrackDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LR_InsertReposseionTrack");
                    foreach (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionTrackRow objTrackRow in objS3G_LR_Track_DAL)
                    {
                      
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objTrackRow.Company_ID);
                        db.AddInParameter(command, "@LR_Track_ID", DbType.Int32, objTrackRow.LR_Track_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objTrackRow.Created_By);
                        db.AddInParameter(command, "@LRN_ID", DbType.Int32, objTrackRow.LRN_ID);
                        db.AddInParameter(command, "@LRN_No", DbType.String, objTrackRow.LRN_No);
                        db.AddInParameter(command, "@Repossession_ID", DbType.Int32, objTrackRow.Repossession_ID);
                        db.AddInParameter(command, "@Repossession_Docket_No", DbType.String, objTrackRow.Repossession_Docket_No);
                        db.AddInParameter(command, "@Current_Garage_ID", DbType.Int32, objTrackRow.Current_Garage_ID);
                        db.AddInParameter(command, "@Current_Garage_IN", DbType.DateTime, objTrackRow.Old_Garage_In);
                        db.AddInParameter(command, "@Asset_ID", DbType.Int32, objTrackRow.Asset_ID);
                        db.AddInParameter(command, "@Asset_Code", DbType.String, objTrackRow.Asset_Code);
                        db.AddInParameter(command, "@Asset_Condition", DbType.String, objTrackRow.Asset_Condition);
                        db.AddInParameter(command, "@Inspection_Done_By_Type", DbType.Int32, objTrackRow.Inspection_Done_By_Type_Code);
                        db.AddInParameter(command, "@Inspector_ID", DbType.Int32, objTrackRow.Inspector_ID);
                        db.AddInParameter(command, "@Inspected_Date", DbType.DateTime, objTrackRow.Inspected_Date);                        
                        if(!objTrackRow.IsNew_Garage_IDNull())
                        db.AddInParameter(command, "@New_Garage_ID", DbType.Int32, objTrackRow.New_Garage_ID);
                        if(!objTrackRow.IsNew_Garage_IDNull())
                        db.AddInParameter(command, "@New_Garage_IN", DbType.DateTime , objTrackRow.New_Garage_In );
                        if(!objTrackRow.IsOld_Garage_OutNull())
                        db.AddInParameter(command, "@Old_Garage_Out", DbType.DateTime , objTrackRow.Old_Garage_Out);
                        if (!objTrackRow.IsRemarksNull())
                        db.AddInParameter(command, "@Remarks", DbType.String, objTrackRow.Remarks);
                        if(!objTrackRow.IsTrack_Details_IDNull())
                        db.AddInParameter(command, "@Track_Details_ID", DbType.Int32 , objTrackRow.Track_Details_ID);
                        
                        db.AddOutParameter(command, "@LR_Track_No", DbType.String, 50);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //modified by chandru on 24/03/2012
                                db.FunPubExecuteNonQuery(command, ref trans);
                                //modified by chandru on 24/03/2012
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                if ((int)command.Parameters["@ErrorCode"].Value > 1)
                                {
                                    throw new Exception("Error in ECS " + intRowsAffected.ToString());
                                }
                                else
                                {
                                    strErrMsg = Convert.ToString(command.Parameters["@LR_Track_No"].Value);
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (command.Parameters["@ErrorCode"].Value != null)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                }
                                else if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();

                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 20;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
             }
        }
    }
}
