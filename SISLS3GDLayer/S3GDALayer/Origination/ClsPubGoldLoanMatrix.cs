using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using S3GBusEntity;

namespace S3GDALayer.Origination
{
    namespace OrgMasterMgtServices
    {
        public class ClsPubGoldLoanMatrix
        {
            int intRowsAffected;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GoldLoanMatrixDataTable objGLMatrix_DAL;

              //Code added for getting common connection string  from config file
            Database db;
            public ClsPubGoldLoanMatrix()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            /// <summary>
            /// Inserting the Gold Loan Matrix Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns> 
            /// 
            public int FunPubCreateModifyGoldLoanMatrix(SerializationMode SerMode, byte[] bytesObjS3G_Origination_GoldLoanMatrix_DataTable)
            {
                try
                {
                    objGLMatrix_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GoldLoanMatrixDataTable)ClsPubSerialize.
                        DeSerialize(bytesObjS3G_Origination_GoldLoanMatrix_DataTable,
                        SerMode, typeof(S3GBusEntity.Origination.
                        OrgMasterMgtServices.S3G_ORG_GoldLoanMatrixDataTable));

                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_GoldLoanMatrixRow objGLMatrixRow in objGLMatrix_DAL)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertGoldLoanMatrix");
                        db.AddInParameter(command, "@GLMatrix_ID", DbType.Int32, objGLMatrixRow.GLMatrix_ID);
                        db.AddInParameter(command, "@Asset_ID", DbType.Int32, objGLMatrixRow.Asset_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objGLMatrixRow.Company_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, objGLMatrixRow.Constitution_ID);
                        db.AddInParameter(command, "@Product_ID", DbType.Int32, objGLMatrixRow.Product_ID);
                        db.AddInParameter(command, "@Location_Code", DbType.String, objGLMatrixRow.Location_Code);
                        db.AddInParameter(command, "@Unit_ID", DbType.Int32, objGLMatrixRow.Unit_ID);
                        db.AddInParameter(command, "@Eff_Date", DbType.DateTime, objGLMatrixRow.Eff_Date);
                        db.AddInParameter(command, "@Tenure", DbType.Decimal, objGLMatrixRow.Tenure);
                        db.AddInParameter(command, "@Tenure_Type", DbType.Int32, objGLMatrixRow.Tenure_Type);
                        db.AddInParameter(command, "@XMLFundingDtl", DbType.String, objGLMatrixRow.XMLFundingDtl);
                        db.AddInParameter(command, "@XMLRateDtl", DbType.String, objGLMatrixRow.XMLRateDtl);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objGLMatrixRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, objGLMatrixRow.Created_On);
                        
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                if (intRowsAffected > 0)
                                {
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else
                                {
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
                                trans.Rollback();
                                throw ex;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }
        }
    }
}
