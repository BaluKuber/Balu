using System;using S3GDALayer.S3GAdminServices;
using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using CollateralModule=S3GBusEntity.Collateral;
using S3GBusEntity;
namespace S3GDALayer.Collateral
{
    namespace CollateralMgtServices
    {
        public class ClsPubCollateralSale : ClsPubDalCollateralBase
        {
            #region Initialization
            CollateralModule.CollateralMgtServices.S3G_CLT_CollateralSaleDataTable objCollateralSaleDataTable;
            CollateralModule.CollateralMgtServices.S3G_CLT_CollateralSaleRow objCollateralSaleRow;

            CollateralModule.CollateralMgtServices.S3G_CLT_CollateralReleaseDataTable objCollateralReleaseDataTable;
            CollateralModule.CollateralMgtServices.S3G_CLT_CollateralReleaseRow objCollateralReleaseRow;
            int IntErrorCode = 0;
            #endregion

            public int FunPubInsertCollateralSale(byte[] byteobjCollateralSale, SerializationMode SerMode, out string CollateralSaleNo)
            {
                CollateralSaleNo="";
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand dbcmd = db.GetStoredProcCommand(SPNames.S3G_CLT_InsertCollaterSale);
                    objCollateralSaleDataTable= ( CollateralModule.CollateralMgtServices.S3G_CLT_CollateralSaleDataTable)ClsPubSerialize.DeSerialize(byteobjCollateralSale,SerMode,typeof(CollateralModule.CollateralMgtServices.S3G_CLT_CollateralSaleDataTable));
                    objCollateralSaleRow = objCollateralSaleDataTable.Rows[0] as CollateralModule.CollateralMgtServices.S3G_CLT_CollateralSaleRow;
                    db.AddInParameter(dbcmd, "@COMPANY_ID", DbType.Int32, objCollateralSaleRow.Company_ID);
                    db.AddInParameter(dbcmd, "@Collateral_Capture_ID", DbType.Int32, objCollateralSaleRow.Collateral_Capture_ID);
                    db.AddInParameter(dbcmd, "@Collateral_Tran_No", DbType.String, objCollateralSaleRow.Collateral_Tran_No);
                    db.AddInParameter(dbcmd, "@Collateral_Sale_Date", DbType.DateTime, objCollateralSaleRow.Collateral_Sale_Date);
                    db.AddInParameter(dbcmd, "@CREATED_BY", DbType.Int32, objCollateralSaleRow.Created_By);
                    db.AddInParameter(dbcmd, "@XMLSALEDETAILS", DbType.String, objCollateralSaleRow.XMLSALEDETAILS);
                    db.AddOutParameter(dbcmd, "@Collateral_Sale_No", DbType.String, 100);
                    db.AddOutParameter(dbcmd, "@ERRORCODE", DbType.Int32, sizeof(Int32));
                    using (DbConnection con = db.CreateConnection())
                    {
                        con.Open();
                        DbTransaction trans = con.BeginTransaction();
                        try
                        {
                            
                            db.FunPubExecuteNonQuery(dbcmd,ref trans);
                            IntErrorCode = (int)dbcmd.Parameters["@ERRORCODE"].Value;
                            if (IntErrorCode == 0)
                            {
                                CollateralSaleNo = dbcmd.Parameters["@Collateral_Sale_No"].Value.ToString();
                                trans.Commit();
                            }
                            else
                            {
                                trans.Rollback();
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        con.Close();
                    }

                }
                catch (Exception ex)
                {

                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return IntErrorCode;
            }

           /// <summary>
           /// To insert Colletral Release Details
           /// Added by Saranya
           /// </summary>
           /// <param name="byteobjCollateralSale"></param>
           /// <param name="SerMode"></param>
           /// <param name="CollateralSaleNo"></param>
           /// <returns></returns>
            public int FunPubInsertCollateralRelease(byte[] byteobjCollateralRelease, SerializationMode SerMode, out string CollateralReleaseNo)
            {
                CollateralReleaseNo = "";
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand dbcmd = db.GetStoredProcCommand(SPNames.S3G_CLT_InsertCollaterRelease);
                    objCollateralReleaseDataTable = (CollateralModule.CollateralMgtServices.S3G_CLT_CollateralReleaseDataTable)ClsPubSerialize.DeSerialize(byteobjCollateralRelease, SerMode, typeof(CollateralModule.CollateralMgtServices.S3G_CLT_CollateralReleaseDataTable));
                    objCollateralReleaseRow = objCollateralReleaseDataTable.Rows[0] as CollateralModule.CollateralMgtServices.S3G_CLT_CollateralReleaseRow;
                    db.AddInParameter(dbcmd, "@COMPANY_ID", DbType.Int32, objCollateralReleaseRow.Company_ID);
                    db.AddInParameter(dbcmd, "@Collateral_Capture_ID", DbType.Int32, objCollateralReleaseRow.Collateral_Capture_ID);
                    db.AddInParameter(dbcmd, "@Collateral_Tran_No", DbType.String, objCollateralReleaseRow.Collateral_Tran_No);
                    db.AddInParameter(dbcmd, "@Collateral_Release_Date", DbType.DateTime, objCollateralReleaseRow.Collateral_Release_Date);
                    db.AddInParameter(dbcmd, "@CREATED_BY", DbType.Int32, objCollateralReleaseRow.Created_By);
                    db.AddInParameter(dbcmd, "@XMLRELEASEDETAILS", DbType.String, objCollateralReleaseRow.XMLRELEASEDETAILS);
                    db.AddOutParameter(dbcmd, "@Collateral_Release_No", DbType.String, 100);
                    db.AddOutParameter(dbcmd, "@ERRORCODE", DbType.Int32, sizeof(Int32));
                    using (DbConnection con = db.CreateConnection())
                    {
                        con.Open();
                        DbTransaction trans = con.BeginTransaction();
                        try
                        {

                            db.FunPubExecuteNonQuery(dbcmd,ref trans);
                            IntErrorCode = (int)dbcmd.Parameters["@ERRORCODE"].Value;
                            if (IntErrorCode == 0)
                            {
                                CollateralReleaseNo = dbcmd.Parameters["@Collateral_Release_No"].Value.ToString();
                                trans.Commit();
                            }
                            else
                            {
                                trans.Rollback();
                            }

                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        con.Close();
                    }

                }
                catch (Exception ex)
                {

                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return IntErrorCode;
            }
            //End Here
            public int FunPubUpdateCollateralSale(byte[] byteobjCollateralSale, SerializationMode SerMode,int ColSaleID)
            {
                objCollateralSaleDataTable =(CollateralModule.CollateralMgtServices.S3G_CLT_CollateralSaleDataTable) ClsPubSerialize.DeSerialize(byteobjCollateralSale, SerMode, typeof(CollateralModule.CollateralMgtServices.S3G_CLT_CollateralSaleRow));
                objCollateralSaleRow = objCollateralSaleDataTable.Rows[0] as CollateralModule.CollateralMgtServices.S3G_CLT_CollateralSaleRow;
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand dbcmd = db.GetStoredProcCommand(SPNames.S3G_CLT_UpdateCollaterSale);
                db.AddInParameter(dbcmd,"@COMPANY_ID", DbType.Int32,objCollateralSaleRow.Company_ID);
                db.AddInParameter(dbcmd, "@Collateral_SALE_ID", DbType.Int32, ColSaleID);
                db.AddInParameter(dbcmd, "@Collateral_Capture_ID", DbType.Int32, objCollateralSaleRow.Collateral_Capture_ID);
                db.AddInParameter(dbcmd, "@Collateral_Tran_No", DbType.String, objCollateralSaleRow.Collateral_Tran_No);
                db.AddInParameter(dbcmd, "@Collateral_Sale_Date", DbType.DateTime, objCollateralSaleRow.Collateral_Sale_Date);
                db.AddInParameter(dbcmd, "@MODIFIED_BY", DbType.Int32, objCollateralSaleRow.Modified_By);
                db.AddInParameter(dbcmd, "@XMLSALEDETAILS", DbType.String, objCollateralSaleRow.XMLSALEDETAILS);
                db.AddOutParameter(dbcmd, "@ERRORCODE", DbType.Int32, sizeof(Int32));
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        db.FunPubExecuteNonQuery(dbcmd,ref trans);
                        IntErrorCode = (int)dbcmd.Parameters["@ERRORCODE"].Value;
                        if (IntErrorCode == 0)
                        {
                            trans.Commit();
                        }
                        else
                        {
                            trans.Rollback();

                        }

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    }
                }
                return IntErrorCode;
            }


            /// <summary>
            /// To Update Colletral Release Details
            /// Added by Saranya
            /// </summary>
            /// <param name="byteobjCollateralSale"></param>
            /// <param name="SerMode"></param>
            /// <param name="ColSaleID"></param>
            /// <returns></returns>
            public int FunPubUpdateCollateralRelease(byte[] byteobjCollateralRelease, SerializationMode SerMode, int ColReleaseID)
            {
                objCollateralReleaseDataTable = (CollateralModule.CollateralMgtServices.S3G_CLT_CollateralReleaseDataTable)ClsPubSerialize.DeSerialize(byteobjCollateralRelease, SerMode, typeof(CollateralModule.CollateralMgtServices.S3G_CLT_CollateralReleaseRow));
                objCollateralReleaseRow = objCollateralReleaseDataTable.Rows[0] as CollateralModule.CollateralMgtServices.S3G_CLT_CollateralReleaseRow;
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand dbcmd = db.GetStoredProcCommand(SPNames.S3G_CLT_UpdateCollaterRelease);
                db.AddInParameter(dbcmd, "@COMPANY_ID", DbType.Int32, objCollateralReleaseRow.Company_ID);
                db.AddInParameter(dbcmd, "@Collateral_Release_ID", DbType.Int32, ColReleaseID);
                db.AddInParameter(dbcmd, "@Collateral_Capture_ID", DbType.Int32, objCollateralReleaseRow.Collateral_Capture_ID);
                db.AddInParameter(dbcmd, "@Collateral_Tran_No", DbType.String, objCollateralReleaseRow.Collateral_Tran_No);
                db.AddInParameter(dbcmd, "@Collateral_Release_Date", DbType.DateTime, objCollateralReleaseRow.Collateral_Release_Date);
                db.AddInParameter(dbcmd, "@MODIFIED_BY", DbType.Int32, objCollateralReleaseRow.Modified_By);
                db.AddInParameter(dbcmd, "@XMLRELEASEDETAILS", DbType.String, objCollateralReleaseRow.XMLRELEASEDETAILS);
                db.AddOutParameter(dbcmd, "@ERRORCODE", DbType.Int32, sizeof(Int32));
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        db.FunPubExecuteNonQuery(dbcmd,ref trans);
                        IntErrorCode = (int)dbcmd.Parameters["@ERRORCODE"].Value;
                        if (IntErrorCode == 0)
                        {
                            trans.Commit();
                        }
                        else
                        {
                            trans.Rollback();

                        }

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    }
                }
                return IntErrorCode;
            }
            //End Here
            public int FunPubUpdateCollateralSaleInvoicedetails(byte[] byteobjCollateralSale, SerializationMode SerMode, int ColSaleID)
            {
                objCollateralSaleDataTable = (CollateralModule.CollateralMgtServices.S3G_CLT_CollateralSaleDataTable)ClsPubSerialize.DeSerialize(byteobjCollateralSale, SerMode, typeof(CollateralModule.CollateralMgtServices.S3G_CLT_CollateralSaleRow));
                objCollateralSaleRow = objCollateralSaleDataTable.Rows[0] as CollateralModule.CollateralMgtServices.S3G_CLT_CollateralSaleRow;
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand dbcmd = db.GetStoredProcCommand(SPNames.S3G_CLT_UpdateCollaterSaleInvoicedetails);
                db.AddInParameter(dbcmd, "@COMPANY_ID", DbType.Int32, objCollateralSaleRow.Company_ID);
                db.AddInParameter(dbcmd, "@Collateral_SALE_ID", DbType.Int32, ColSaleID);
                db.AddInParameter(dbcmd, "@Collateral_Capture_ID", DbType.Int32, objCollateralSaleRow.Collateral_Capture_ID);
                db.AddInParameter(dbcmd, "@Collateral_Tran_No", DbType.String, objCollateralSaleRow.Collateral_Tran_No);
                db.AddInParameter(dbcmd, "@Collateral_Sale_Date", DbType.DateTime, objCollateralSaleRow.Collateral_Sale_Date);
                db.AddInParameter(dbcmd, "@MODIFIED_BY", DbType.Int32, objCollateralSaleRow.Modified_By);
                db.AddInParameter(dbcmd, "@XMLSALEDETAILS", DbType.String, objCollateralSaleRow.XMLSALEDETAILS);
                db.AddInParameter(dbcmd, "@XMLINVDETAILS", DbType.String, objCollateralSaleRow.XMLINVDETAILS);
                db.AddInParameter(dbcmd, "@XMLRECEIPTDETAILS", DbType.String, objCollateralSaleRow.XMLRECEIPTDETAILS);
                db.AddOutParameter(dbcmd, "@ERRORCODE", DbType.Int32, sizeof(Int32));
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        db.FunPubExecuteNonQuery(dbcmd,ref trans);
                        IntErrorCode = (int)dbcmd.Parameters["@ERRORCODE"].Value;
                        if (IntErrorCode == 0)
                        {
                            trans.Commit();
                        }
                        else
                        {
                            trans.Rollback();

                        }

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    }
                }
                return IntErrorCode;
            }
            public DataSet FunPubGetColSaleDetails(int ColSaleId, int CompanyID)
            {
                DataSet DsColSaleDetails = new DataSet();
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand dbcmd = db.GetStoredProcCommand(SPNames.S3G_CLT_GetCollateralSaleDetails);
                db.AddInParameter(dbcmd, "@COLLATERAL_SALE_ID", DbType.Int32, ColSaleId);
                db.AddInParameter(dbcmd, "@COMPANY_ID", DbType.Int32, CompanyID);
                db.FunPubLoadDataSet(dbcmd, DsColSaleDetails, "ColSaleTables");
                return DsColSaleDetails;
            }

            /// <summary>
            /// To get Collateral Release Details
            /// Added by Saranya
            /// </summary>
            /// <param name="ColSaleId"></param>
            /// <param name="CompanyID"></param>
            /// <returns></returns>
            public DataSet FunPubGetColReleaseDetails(int ColReleaseId, int CompanyID)
            {
                DataSet DsColReleaseDetails = new DataSet();
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand dbcmd = db.GetStoredProcCommand(SPNames.S3G_CLT_GetCollateralReleaseDetails);
                db.AddInParameter(dbcmd, "@COLLATERAL_RELEASE_ID", DbType.Int32, ColReleaseId);
                db.AddInParameter(dbcmd, "@COMPANY_ID", DbType.Int32, CompanyID);
                db.FunPubLoadDataSet(dbcmd, DsColReleaseDetails, "ColSaleTables");
                return DsColReleaseDetails;
            }
        }
      
    }
}
