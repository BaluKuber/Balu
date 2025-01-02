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

    public class ClsPubRptAssetPerformance : ClsPubDalReportBase
    {

        #region Load LOB
        /// <summary>
        /// To get the LOB
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubLOBAssetPerf(int CompanyId, int UserId, int Option, int ProgramId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetLOB);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.LOBFILTEROPTION, DbType.Int32, Option);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                //Modified By Chandrasekar K On 31-July-2012
                //IDataReader reader = db.ExecuteReader(command);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.LOBID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    dropDownLists.Add(dropDownList);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return FunPriLoadSelect(dropDownLists);
        }
        #endregion
        

        #region Load Asset Details Grid
        /// <summary>
        /// To get the Asset Details
        /// </summary>
        /// <param name="PANum"></param>
        /// <param name="SANum"></param>
        /// <returns></returns>
        /// 
        public  List<ClsPubAssetPerformance> FunPubGetAssestPerformance(ClsPubAssetPerfParam objAssetParams)
        {
            
            List<ClsPubAssetPerformance> ListAssetperformance = new List<ClsPubAssetPerformance>();
             
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetAssetPerformance);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, objAssetParams.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID , DbType.Int32, objAssetParams.LOB_ID);
                db.AddInParameter(command, ClsPubDALConstants.IRRTYPE, DbType.Int32, objAssetParams.IRR_Type);
                db.AddInParameter(command, ClsPubDALConstants.REPORT_TYPE, DbType.Int32, objAssetParams.Report_Type);
                db.AddInParameter(command, ClsPubDALConstants.CURRENT_DATE, DbType.String, objAssetParams.CurrentDate);
                db.AddInParameter(command, ClsPubDALConstants.CURRENT_MONTH_STARTDATE, DbType.String, objAssetParams.CurrentMonthStartDate);
                db.AddInParameter(command, ClsPubDALConstants.CURRENT_YEAR_STARTDATE , DbType.String, objAssetParams.FinYearStartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, objAssetParams.Denomintion);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, objAssetParams.User_ID);
                //Modified By Chandrasekar K On 31-July-2012
                //IDataReader reader = db.ExecuteReader(command);
                IDataReader reader = db.FunPubExecuteReader(command);
                
                while (reader.Read())
                {
                    ClsPubAssetPerformance Assetperformance = new ClsPubAssetPerformance();
//                    Assetperformance.Asset_Code =StringParse(reader[ClsPubDALConstants.PERFASSETCODE ]);
                    Assetperformance.LOB = StringParse(reader[ClsPubDALConstants.LOB ]);
                    Assetperformance.AssetClass = StringParse(reader[ClsPubDALConstants.ASSETCLASS]);
                    Assetperformance.AssetMake = StringParse(reader[ClsPubDALConstants.ASSETMAKE]);
                    Assetperformance.NewOrOld= StringParse(reader[ClsPubDALConstants.NewOrOld]);
                    Assetperformance.BCD = DecimalParse(reader[ClsPubDALConstants.BCD]);
                    Assetperformance.BCDAmt = DecimalParse(reader[ClsPubDALConstants.BCDAMT]);
                    Assetperformance.BDD = DecimalParse(reader[ClsPubDALConstants.BDD]);
                    Assetperformance.BDDAmt = DecimalParse(reader[ClsPubDALConstants.BDDAMT]);
                    Assetperformance.BCM = DecimalParse(reader[ClsPubDALConstants.BCM]);
                    Assetperformance.BCMAmt = DecimalParse(reader[ClsPubDALConstants.BCMAMT]);
                    Assetperformance.BDM = DecimalParse(reader[ClsPubDALConstants.BDM]);
                    Assetperformance.BDMAmt = DecimalParse(reader[ClsPubDALConstants.BDMAMT]);
                    Assetperformance.BCY = DecimalParse(reader[ClsPubDALConstants.BCY]);
                    Assetperformance.BCYAmt = DecimalParse(reader[ClsPubDALConstants.BCYAMT]);
                    Assetperformance.BDY = DecimalParse(reader[ClsPubDALConstants.BDY]);
                    Assetperformance.BDYAmt = DecimalParse(reader[ClsPubDALConstants.BDYAMT]);
                    Assetperformance.DataRow1  = StringParse(reader[ClsPubDALConstants.DataRow1]);
                    Assetperformance.DataRow2  = StringParse(reader[ClsPubDALConstants.DataRow2]);
                    Assetperformance.ReportType  = StringParse(reader[ClsPubDALConstants.ReportType]);
                    ListAssetperformance.Add(Assetperformance);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return ListAssetperformance;
        }
        #endregion 
         
       #region For WIRR Calculation Service
        /// <summary>
        /// To Calculate WIRR through service and save in table
        /// </summary>
        public int FunPubBuildService()
        {
            int i = 0;
            try
            {
                string FYMN = S3GDALayer.Common.ClsIniFileAccess.FunPubFinYearStartMonth();

                int FinYearStartMonth = Convert.ToInt32(FYMN); //Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]);
                string strCurrentDate = DateTime.Now.ToString("yyyyMMdd");
                int year = int.Parse(strCurrentDate.Substring(0, 4));
                int Month = int.Parse(strCurrentDate.Substring(4, 2));
                string DaysinMonth = System.DateTime.DaysInMonth(year, Month).ToString();
                string CurrentDate = DateTime.Now.ToString("MM/dd/yyyy");

                string FinancialYear = "";
                if (FinYearStartMonth < Month)
                {
                    FinancialYear = Convert.ToString(year);
                }
                else
                {
                    FinancialYear = Convert.ToString(year - 1);
                }
                string CurrentMonthStart = Month + "/" + "1" + "/" + year;
                string CurrentMonthEnd = Month + "/" + DaysinMonth + "/" + year;
                string FinYearStartDate = FinYearStartMonth + "/" + "1" + "/" + FinancialYear;
                DbCommand command = db.GetStoredProcCommand("S3G_RPT_CalcWIRRFromService");
                db.AddInParameter(command, "@Company_ID", DbType.Int32, 1);
                db.AddInParameter(command, "@CurrentDate", DbType.String, CurrentMonthEnd);
                db.AddInParameter(command, "@CurrentMonthStartDate", DbType.String, CurrentMonthStart);
                db.AddInParameter(command, "@CurrentYearStartDate", DbType.String, FinYearStartDate);
                //Modified By Chandrasekar K On 31-July-2012
                //db.ExecuteNonQuery(command);
                db.FunPubExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return i;
        }
        #endregion
        

    }
}
