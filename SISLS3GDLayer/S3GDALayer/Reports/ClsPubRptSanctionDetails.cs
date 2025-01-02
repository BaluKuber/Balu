using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity.Reports;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Data;
using S3GDALayer.Constants;
using S3GBusEntity;


namespace S3GDALayer.Reports
{
    public class ClsPubRptSanctionDetails : ClsPubDalReportBase
    {

        #region Get Sanction Details
        public ClsPubSumOfSanctionDetails FunPubGetSanctionDetails(ClsPubSanctionParameterDetails ObjSanction)
        {

            ClsPubSumOfSanctionDetails sumOfSanctionDetails = new ClsPubSumOfSanctionDetails();


            sumOfSanctionDetails.SanctionDetails = new List<ClsPubSanctionDetails>();


            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.SanctionDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, ObjSanction.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, ObjSanction.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ObjSanction.ProgramId);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, ObjSanction.LobId);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, ObjSanction.LocationId1);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, ObjSanction.LocationId2);


                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODUCTID, DbType.Int32, ObjSanction.ProductId);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, ObjSanction.StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, ObjSanction.EndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, ObjSanction.Denomination);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISSUMMARY, DbType.Boolean, ObjSanction.IsDetail);

                IDataReader reader = db.FunPubExecuteReader(command);
                if (ObjSanction.IsDetail)
                {
                    S3GDALDBType S3GDALType = Common.ClsIniFileAccess.S3G_DBType;
                    if (S3GDALType == S3GDALDBType.ORACLE)
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(0).ToUpper() == "TEST")
                            {
                                continue;
                            }
                        }
                        reader.NextResult();
                    }
                    while (reader.Read())
                    {                        
                        ClsPubSanctionDetails SanctionDetail = new ClsPubSanctionDetails();

                        SanctionDetail.APPLICATION_PROCESS_ID = IntParse(reader[ClsPubDALConstants.APPLICATIONPROCESSID]);
                        SanctionDetail.REGION = StringParse(reader[ClsPubDALConstants.LOCATION]);
                        SanctionDetail.PRODUCT_NAME = StringParse(reader[ClsPubDALConstants.PRODUCT]);
                        SanctionDetail.APPLICATIONNO = StringParse(reader[ClsPubDALConstants.APPLICATIONNO]);
                        SanctionDetail.CUSTOMERNAME = StringParse(reader[ClsPubDALConstants.CUSTOMERNAM]);
                        SanctionDetail.PANUM = StringParse(reader[ClsPubDALConstants.PANUM]);
                        SanctionDetail.SANUM = StringParse(reader[ClsPubDALConstants.SANUM]);
                        SanctionDetail.FINANCEAMOUNTOFFERED = DecimalParse(reader[ClsPubDALConstants.FINANCEAMOUNTOFFERED]);
                        SanctionDetail.FINANCEAMOUNTSOUGHT = DecimalParse(reader[ClsPubDALConstants.FINANCEAMOUNTSOUGHT]);
                        SanctionDetail.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);

                        sumOfSanctionDetails.SanctionDetails.Add(SanctionDetail);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        ClsPubSanctionDisbursableDetails disbursable = new ClsPubSanctionDisbursableDetails();
                        disbursable.APPLICATION_PROCESS_ID = IntParse(reader[ClsPubDALConstants.APPLICATIONPROCESSID]);
                        disbursable.PANUM = StringParse(reader[ClsPubDALConstants.PANUM]);
                        disbursable.SANUM = StringParse(reader[ClsPubDALConstants.SANUM]);
                        disbursable.DISBURSABLE_AMOUNT = DecimalParse(reader[ClsPubDALConstants.DISBURSABLEAMOUNT]);
                        disbursable.DISBURSABLE_DATE = reader[ClsPubDALConstants.DISBURSABLE_DATE].ToString();
                        disbursable.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);


                        ClsPubSanctionDetails detail = sumOfSanctionDetails.SanctionDetails.Find(ClsDetail => ClsDetail.PANUM == disbursable.PANUM && ClsDetail.SANUM == disbursable.SANUM && ClsDetail.APPLICATION_PROCESS_ID == disbursable.APPLICATION_PROCESS_ID);
                        if (detail != null)
                            detail.Disbursable.Add(disbursable);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        ClsPubSanctionDisbursedDetails disbursed = new ClsPubSanctionDisbursedDetails();
                        disbursed.APPLICATION_PROCESS_ID = IntParse(reader[ClsPubDALConstants.APPLICATIONPROCESSID]);
                        disbursed.PANUM = StringParse(reader[ClsPubDALConstants.PANUM]);
                        disbursed.SANUM = StringParse(reader[ClsPubDALConstants.SANUM]);

                        disbursed.DISBURSED_NO = StringParse(reader[ClsPubDALConstants.DISBURSEDNO]);
                        disbursed.DISBURSED_DATE = reader[ClsPubDALConstants.DISBURSEDDATE].ToString();
                        disbursed.DISBURSED_AMOUNT = DecimalParse(reader[ClsPubDALConstants.DISBURSEDAMT]);
                        disbursed.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);


                        ClsPubSanctionDetails detail = sumOfSanctionDetails.SanctionDetails.Find(ClsDetail => ClsDetail.PANUM == disbursed.PANUM && ClsDetail.SANUM == disbursed.SANUM && ClsDetail.APPLICATION_PROCESS_ID == disbursed.APPLICATION_PROCESS_ID);
                        if (detail != null)
                            detail.Disbursed.Add(disbursed);

                    }
                    //Header
                    reader.NextResult();
                    while (reader.Read())
                    {
                        sumOfSanctionDetails.TOTALFINANCEAMOUNTSOUGHT = DecimalParse(reader[ClsPubDALConstants.FINANCEAMOUNTSOUGHT]);
                        sumOfSanctionDetails.TOTALFINANCEAMOUNTOFFERED = DecimalParse(reader[ClsPubDALConstants.FINANCEAMOUNTOFFERED]);

                    }
                    //disbursable
                    reader.NextResult();
                    while (reader.Read())
                    {
                        sumOfSanctionDetails.TOTALDISBURSABLE_AMOUNT = DecimalParse(reader[ClsPubDALConstants.DISBURSABLEAMOUNT]);
                    }
                    //disbursed

                    reader.NextResult();
                    while (reader.Read())
                    {
                        sumOfSanctionDetails.TOTALDISBURSED_AMOUNT = DecimalParse(reader[ClsPubDALConstants.DISBURSEDAMT]);
                    }
                    reader.Close();
                }
                else
                {
                    while (reader.Read())
                    {
                        ClsPubSanctionDetails SanctionDetail = new ClsPubSanctionDetails();
                        SanctionDetail.REGION = StringParse(reader[ClsPubDALConstants.LOCATION]);

                        SanctionDetail.PRODUCT_NAME = StringParse(reader[ClsPubDALConstants.PRODUCT]);
                        SanctionDetail.FINANCEAMOUNTOFFERED = DecimalParse(reader[ClsPubDALConstants.FINANCEAMOUNTOFFERED]);
                        SanctionDetail.FINANCEAMOUNTSOUGHT = DecimalParse(reader[ClsPubDALConstants.FINANCEAMOUNTSOUGHT]);
                        SanctionDetail.DISBURSABLE_AMOUNT = DecimalParse(reader[ClsPubDALConstants.DISBURSABLEAMOUNT]);
                        SanctionDetail.DISBURSED_AMOUNT = DecimalParse(reader[ClsPubDALConstants.DISBURSEDAMT]);
                        SanctionDetail.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);

                        sumOfSanctionDetails.SanctionDetails.Add(SanctionDetail);
                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return sumOfSanctionDetails;
        }
        #endregion

        #region Get Denominations
        public List<ClsPubDropDownList> GetDenominations()
        {

            List<ClsPubDropDownList> denominations = new List<ClsPubDropDownList>();
            ClsPubDropDownList denomination;

            denomination = new ClsPubDropDownList();
            denomination.ID = "1";
            denomination.Description = "Actual";
            denominations.Add(denomination);

            denomination = new ClsPubDropDownList();
            denomination.ID = "1000";
            denomination.Description = "Thousands";
            denominations.Add(denomination);

            denomination = new ClsPubDropDownList();
            denomination.ID = "100000";
            denomination.Description = "Lakhs";
            denominations.Add(denomination);

            denomination = new ClsPubDropDownList();
            denomination.ID = "1000000";
            denomination.Description = "Millions";
            denominations.Add(denomination);

            denomination = new ClsPubDropDownList();
            denomination.ID = "10000000";
            denomination.Description = "Crores";
            denominations.Add(denomination);

            denomination = new ClsPubDropDownList();
            denomination.ID = "1000000000";
            denomination.Description = "Billions";
            denominations.Add(denomination);

            return denominations;
        }
        #endregion

        #region Get Sanction Details for Report
        public ClsPubSanctionDetailsReport FunPubGetSanctionReport(ClsPubSanctionParameterDetails ObjSanction)
        {

            ClsPubSanctionDetailsReport SanctionDetailsReport = new ClsPubSanctionDetailsReport();


            SanctionDetailsReport.Sanction = new List<ClsPubSanctionDetails>();
            SanctionDetailsReport.Disbursable = new List<ClsPubSanctionDisbursableDetails>();
            SanctionDetailsReport.Disbursed = new List<ClsPubSanctionDisbursedDetails>();

            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.SanctionDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, ObjSanction.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, ObjSanction.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ObjSanction.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, ObjSanction.LobId);


                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, ObjSanction.LocationId1);


                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, ObjSanction.LocationId2);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODUCTID, DbType.Int32, ObjSanction.ProductId);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, ObjSanction.StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, ObjSanction.EndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, ObjSanction.Denomination);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISSUMMARY, DbType.Boolean, ObjSanction.IsDetail);

                IDataReader reader = db.ExecuteReader(command);
                if (ObjSanction.IsDetail)
                {
                    while (reader.Read())
                    {
                        ClsPubSanctionDetails SanctionDetail = new ClsPubSanctionDetails();

                        SanctionDetail.APPLICATION_PROCESS_ID = IntParse(reader[ClsPubDALConstants.APPLICATIONPROCESSID]);
                        SanctionDetail.REGION = StringParse(reader[ClsPubDALConstants.LOCATION]);

                        SanctionDetail.PRODUCT_NAME = StringParse(reader[ClsPubDALConstants.PRODUCT]);
                        SanctionDetail.APPLICATIONNO = StringParse(reader[ClsPubDALConstants.APPLICATIONNO]);
                        SanctionDetail.CUSTOMERNAME = StringParse(reader[ClsPubDALConstants.CUSTOMERNAM]);
                        SanctionDetail.PANUM = StringParse(reader[ClsPubDALConstants.PANUM]);
                        SanctionDetail.SANUM = StringParse(reader[ClsPubDALConstants.SANUM]);
                        SanctionDetail.FINANCEAMOUNTOFFERED = DecimalParse(reader[ClsPubDALConstants.FINANCEAMOUNTOFFERED]);
                        SanctionDetail.FINANCEAMOUNTSOUGHT = DecimalParse(reader[ClsPubDALConstants.FINANCEAMOUNTSOUGHT]);
                        SanctionDetail.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);

                        SanctionDetailsReport.Sanction.Add(SanctionDetail);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        ClsPubSanctionDisbursableDetails disbursable = new ClsPubSanctionDisbursableDetails();
                        disbursable.APPLICATION_PROCESS_ID = IntParse(reader[ClsPubDALConstants.APPLICATIONPROCESSID]);
                        disbursable.PANUM = StringParse(reader[ClsPubDALConstants.PANUM]);
                        disbursable.SANUM = StringParse(reader[ClsPubDALConstants.SANUM]);
                        disbursable.DISBURSABLE_AMOUNT = DecimalParse(reader[ClsPubDALConstants.DISBURSABLEAMOUNT]);
                        disbursable.DISBURSABLE_DATE = reader[ClsPubDALConstants.DISBURSABLE_DATE].ToString();
                        disbursable.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                        SanctionDetailsReport.Disbursable.Add(disbursable);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        ClsPubSanctionDisbursedDetails disbursed = new ClsPubSanctionDisbursedDetails();
                        disbursed.APPLICATION_PROCESS_ID = IntParse(reader[ClsPubDALConstants.APPLICATIONPROCESSID]);
                        disbursed.PANUM = StringParse(reader[ClsPubDALConstants.PANUM]);
                        disbursed.SANUM = StringParse(reader[ClsPubDALConstants.SANUM]);

                        disbursed.DISBURSED_NO = StringParse(reader[ClsPubDALConstants.DISBURSEDNO]);
                        disbursed.DISBURSED_DATE = reader[ClsPubDALConstants.DISBURSEDDATE].ToString();
                        disbursed.DISBURSED_AMOUNT = DecimalParse(reader[ClsPubDALConstants.DISBURSEDAMT]);
                        disbursed.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);

                        SanctionDetailsReport.Disbursed.Add(disbursed);

                    }
                    reader.Close();
                }
                else
                {
                    while (reader.Read())
                    {
                        ClsPubSanctionDetails SanctionDetail = new ClsPubSanctionDetails();
                        SanctionDetail.REGION = StringParse(reader[ClsPubDALConstants.LOCATION]);

                        SanctionDetail.PRODUCT_NAME = StringParse(reader[ClsPubDALConstants.PRODUCT]);
                        SanctionDetail.FINANCEAMOUNTOFFERED = DecimalParse(reader[ClsPubDALConstants.FINANCEAMOUNTOFFERED]);
                        SanctionDetail.FINANCEAMOUNTSOUGHT = DecimalParse(reader[ClsPubDALConstants.FINANCEAMOUNTSOUGHT]);
                        SanctionDetail.DISBURSABLE_AMOUNT = DecimalParse(reader[ClsPubDALConstants.DISBURSABLEAMOUNT]);
                        SanctionDetail.DISBURSED_AMOUNT = DecimalParse(reader[ClsPubDALConstants.DISBURSEDAMT]);
                        SanctionDetail.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);

                        SanctionDetailsReport.Sanction.Add(SanctionDetail);
                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return SanctionDetailsReport;
        }
        #endregion
    }

    
}
