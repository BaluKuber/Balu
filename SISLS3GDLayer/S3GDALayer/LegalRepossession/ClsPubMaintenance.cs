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
        public class ClsPubMaintenance : ClsPubDalLegalRepossessionBase
        {



            int intRowsAffected;
            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_Garage_MaintDataTable ObjS3G_LR_MAINT_DAL;
            public int FunCreateMaintenance(SerializationMode SerMode, byte[] bytesMaint_Datatable, out string strGARAGEMAINTNO)
            {
                strGARAGEMAINTNO = "";
               
                try
                {
                    ObjS3G_LR_MAINT_DAL = (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_Garage_MaintDataTable)
                        ClsPubSerialize.DeSerialize(bytesMaint_Datatable, SerMode, typeof(S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_Garage_MaintDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    Database db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();

                    foreach (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_Garage_MaintRow objMaintRow in ObjS3G_LR_MAINT_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LR_INS_GARAGE_MAINT");

                        db.AddInParameter(command, "@Garage_ID", DbType.Int32, objMaintRow.Garage_ID);
                        db.AddInParameter(command, "@Garage_Maint_ID", DbType.Int32, objMaintRow.Garage_Maint_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objMaintRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objMaintRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objMaintRow.Branch_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, objMaintRow.Customer_ID);
                        db.AddInParameter(command, "@UserId", DbType.Int32, objMaintRow.UserId);


                        db.AddInParameter(command, "@XMLGarageDetail", DbType.String, objMaintRow.XmlGarageDetails);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                       
                        db.AddOutParameter(command, "@GARAGEMAINTNO", DbType.String, 200);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);//Modified on 22/mar/2012 by Ponnurajesh for oracle conversion
                                db.FunPubExecuteNonQuery(command, ref trans);
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                if ((int)command.Parameters["@ErrorCode"].Value > 1)
                                {
                                    throw new Exception("Error in Garage_Maint " + intRowsAffected.ToString());
                                }
                                else
                                {
                                    strGARAGEMAINTNO = Convert.ToString(command.Parameters["@GARAGEMAINTNO"].Value);
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
                    intRowsAffected = 20; ;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }
        }
    }
}
