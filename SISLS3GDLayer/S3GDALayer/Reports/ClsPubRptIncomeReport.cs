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
    public class ClsPubRptIncomeReport : ClsPubDalReportBase
    {
        #region Load LOB
        /// <summary>
        /// To get the LOB
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubLOBIncome(int CompanyId, int UserId, int Option, int ProgramId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetLOB);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.LOBFILTEROPTION, DbType.Int32, Option);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
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

        #region Load Location1 based on Multi LOB
        /// <summary>
        /// To get the Location1
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubLoc1_MultiLOB(int CompanyId, int UserId, int ProgramId, string LobId)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {

                DbCommand command = db.GetStoredProcCommand("S3G_RPT_LOC1_MultiLOB");
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.XMLLOB_ID, DbType.String, LobId);

                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.LOCATIONID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    dropdownLists.Add(dropDownList);

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }

            return FunPriLoadSelect(dropdownLists);

        }
        #endregion


        #region Load Location2 based on Multi LOB and Location1
        /// <summary>
        /// To get the Location2
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>

        public List<ClsPubDropDownList> FunPubLoc2_MultiLOB(int ProgramId, int UserId, int CompanyId, string LobId, int LocationId)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {

                DbCommand command = db.GetStoredProcCommand("S3G_RPT_LOC2_MultiLOB");
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.XMLLOB_ID, DbType.String, LobId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID, DbType.Int32, LocationId);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.LOCATIONID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    dropdownLists.Add(dropDownList);

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }

            return FunPriLoadSelect(dropdownLists);

        }

        #endregion


        #region Load Income Report Grid
        /// <summary>
        /// To get the Income Report Details
        /// </summary>
        /// <param name="PANum"></param>
        /// <param name="SANum"></param>
        /// <returns></returns>
        /// 
        //public List<ClsPubIncomeReport> FunPubGetIncomeReport(ClsPubIncomeReportParams objIncomeParams)
        public ClsPubIncomeReport FunPubGetIncomeReport(ClsPubIncomeReportParams objIncomeParams)
        {


            //List<ClsPubIncomeReport> ListIncomeReport = new List<ClsPubIncomeReport>();
            ClsPubIncomeReport ListIncomeReport = new ClsPubIncomeReport();
            try
            {
                DbCommand command = db.GetStoredProcCommand("S3G_RPT_IncomeReportDetails");
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, objIncomeParams.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID , DbType.Int32, objIncomeParams.User_ID );
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID , DbType.Int32, objIncomeParams.Program_ID );
                
                //if(objIncomeParams.LOB_ID>0)
                //    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, objIncomeParams.LOB_ID);
                if (objIncomeParams.LOB_ID1  > 0)
                    db.AddInParameter(command, ClsPubDALConstants.LOB_ID1 , DbType.Int32, objIncomeParams.LOB_ID1);
                if (objIncomeParams.LOB_ID2 > 0)
                    db.AddInParameter(command, ClsPubDALConstants.LOB_ID2, DbType.Int32, objIncomeParams.LOB_ID2);
                if (objIncomeParams.LOB_ID3 > 0)
                    db.AddInParameter(command, ClsPubDALConstants.LOB_ID3, DbType.Int32, objIncomeParams.LOB_ID3);
                if (objIncomeParams.LOB_ID4 > 0)
                    db.AddInParameter(command, ClsPubDALConstants.LOB_ID4, DbType.Int32, objIncomeParams.LOB_ID4);
                
                if (objIncomeParams.Location_ID1 > 0)
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1 , DbType.Int32, objIncomeParams.Location_ID1 );
                if (objIncomeParams.Location_ID2 > 0)
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2 , DbType.Int32, objIncomeParams.Location_ID2 );
                db.AddInParameter(command, ClsPubDALConstants.REPORT_TYPE, DbType.Int32, objIncomeParams.Report_Type);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTH , DbType.String, objIncomeParams.CutoffMonth);
                db.AddInParameter(command, ClsPubDALConstants.CutoffMonth_Date, DbType.String, objIncomeParams.CutoffMonth_Date );

                if (objIncomeParams.FinYearstart!=null )
                    db.AddInParameter(command, ClsPubDALConstants.FinYearstart, DbType.String, objIncomeParams.FinYearstart);
                if (objIncomeParams.CurrentFinYear != null)
                    db.AddInParameter(command, ClsPubDALConstants.CurrentFinYear, DbType.String, objIncomeParams.CurrentFinYear);
                if (objIncomeParams.PreviousFinYear != null)
                    db.AddInParameter(command, ClsPubDALConstants.PreviousFinYear, DbType.String, objIncomeParams.PreviousFinYear);
                
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, objIncomeParams.Denomintion);
                db.AddInParameter(command, ClsPubDALConstants.XMLLOB_ID, DbType.String, objIncomeParams.XMLLOB_ID);
                command.CommandTimeout = 600;
                IDataReader reader = db.FunPubExecuteReader(command);
                ListIncomeReport.IncomeDetails = new List<ClsPubIncomeReport>();
                ListIncomeReport.LOB1  = new List<ClsPubIncomeReport>();
                ListIncomeReport.LOB2  = new List<ClsPubIncomeReport>();
                ListIncomeReport.LOB3  = new List<ClsPubIncomeReport>();
                ListIncomeReport.LOB4 = new List<ClsPubIncomeReport>();
                ListIncomeReport.SampleLOB = new List<ClsPubIncomeReport>();
                ListIncomeReport.MainReportLoc = new List<ClsPubIncomeReport>();

                while (reader.Read())
                {
                    ClsPubIncomeReport IncomeReport = new ClsPubIncomeReport();
                    IncomeReport.LOB_ID = IntParse(reader[ClsPubDALConstants.LOBID]);
                    IncomeReport.LOBName  = StringParse(reader[ClsPubDALConstants.LOBNAME ]);
                    IncomeReport.Location_ID1 = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    IncomeReport.LocationName1 = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                    IncomeReport.DataRow1 = StringParse(reader[ClsPubDALConstants.Method_Value]);
                    IncomeReport.Method = StringParse(reader[ClsPubDALConstants.Method]);
                    IncomeReport.Month = DecimalParse(reader[ClsPubDALConstants.MONTH]);
                    IncomeReport.Year = DecimalParse(reader[ClsPubDALConstants.Year]);
                    IncomeReport.DataRow2 = DecimalParse(reader[ClsPubDALConstants.UMFC]);
                    IncomeReport.GPSSuffix  = StringParse(reader[ClsPubDALConstants.Gpssuffix ]);
                    IncomeReport.Denomintion = DecimalParse(reader[ClsPubDALConstants.DENOMINATION]);
                    //ListIncomeReport.Add(IncomeReport);
                    ListIncomeReport.IncomeDetails.Add(IncomeReport);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ClsPubIncomeReport IncomeReport = new ClsPubIncomeReport();

                    IncomeReport.LOB_ID = IntParse(reader[ClsPubDALConstants.LOBID]);
                    IncomeReport.LOBName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    IncomeReport.Location_ID1 = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    IncomeReport.LocationName1 = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                    IncomeReport.DataRow1 = StringParse(reader[ClsPubDALConstants.Method_Value]);
                    IncomeReport.Method = StringParse(reader[ClsPubDALConstants.Method]);
                    IncomeReport.Month = DecimalParse(reader[ClsPubDALConstants.MONTH]);
                    IncomeReport.Year = DecimalParse(reader[ClsPubDALConstants.Year]);
                    IncomeReport.DataRow2 = DecimalParse(reader[ClsPubDALConstants.UMFC]);
                    IncomeReport.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    IncomeReport.Denomintion = DecimalParse(reader[ClsPubDALConstants.DENOMINATION]);
                    ListIncomeReport.LOB1.Add(IncomeReport);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ClsPubIncomeReport IncomeReport = new ClsPubIncomeReport();

                    IncomeReport.LOB_ID = IntParse(reader[ClsPubDALConstants.LOBID]);
                    IncomeReport.LOBName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    IncomeReport.Location_ID1 = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    IncomeReport.LocationName1 = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                    IncomeReport.DataRow1 = StringParse(reader[ClsPubDALConstants.Method_Value]);
                    IncomeReport.Method = StringParse(reader[ClsPubDALConstants.Method]);
                    IncomeReport.Month = DecimalParse(reader[ClsPubDALConstants.MONTH]);
                    IncomeReport.Year = DecimalParse(reader[ClsPubDALConstants.Year]);
                    IncomeReport.DataRow2 = DecimalParse(reader[ClsPubDALConstants.UMFC]);
                    IncomeReport.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    IncomeReport.Denomintion = DecimalParse(reader[ClsPubDALConstants.DENOMINATION]);
                    ListIncomeReport.LOB2.Add(IncomeReport);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ClsPubIncomeReport IncomeReport = new ClsPubIncomeReport();

                    IncomeReport.LOB_ID = IntParse(reader[ClsPubDALConstants.LOBID]);
                    IncomeReport.LOBName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    IncomeReport.Location_ID1 = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    IncomeReport.LocationName1 = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                    IncomeReport.DataRow1 = StringParse(reader[ClsPubDALConstants.Method_Value]);
                    IncomeReport.Method = StringParse(reader[ClsPubDALConstants.Method]);
                    IncomeReport.Month = DecimalParse(reader[ClsPubDALConstants.MONTH]);
                    IncomeReport.Year = DecimalParse(reader[ClsPubDALConstants.Year]);
                    IncomeReport.DataRow2 = DecimalParse(reader[ClsPubDALConstants.UMFC]);
                    IncomeReport.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    IncomeReport.Denomintion = DecimalParse(reader[ClsPubDALConstants.DENOMINATION]);
                    ListIncomeReport.LOB3.Add(IncomeReport);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ClsPubIncomeReport IncomeReport = new ClsPubIncomeReport();

                    IncomeReport.LOB_ID = IntParse(reader[ClsPubDALConstants.LOBID]);
                    IncomeReport.LOBName  = StringParse(reader[ClsPubDALConstants.LOBNAME ]);
                    IncomeReport.Location_ID1 = IntParse(reader[ClsPubDALConstants.LOCATIONID]);
                    IncomeReport.LocationName1 = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                    IncomeReport.DataRow1 = StringParse(reader[ClsPubDALConstants.Method_Value]);
                    IncomeReport.Method = StringParse(reader[ClsPubDALConstants.Method]);
                    IncomeReport.Month = DecimalParse(reader[ClsPubDALConstants.MONTH]);
                    IncomeReport.Year = DecimalParse(reader[ClsPubDALConstants.Year]);
                    IncomeReport.DataRow2 = DecimalParse(reader[ClsPubDALConstants.UMFC]);
                    IncomeReport.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    IncomeReport.Denomintion = DecimalParse(reader[ClsPubDALConstants.DENOMINATION]);
                    ListIncomeReport.LOB4.Add(IncomeReport);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ClsPubIncomeReport IncomeReport = new ClsPubIncomeReport();

                    IncomeReport.LOBName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    IncomeReport.Method = StringParse(reader[ClsPubDALConstants.Method]);
                    IncomeReport.LocationName1 = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                    IncomeReport.Month = DecimalParse(reader[ClsPubDALConstants.MONTH]);
                    IncomeReport.Year = DecimalParse(reader[ClsPubDALConstants.Year]);
                    IncomeReport.DataRow2 = DecimalParse(reader[ClsPubDALConstants.UMFC]);
                    IncomeReport.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    IncomeReport.Denomintion = DecimalParse(reader[ClsPubDALConstants.DENOMINATION]);
                    ListIncomeReport.SampleLOB.Add(IncomeReport);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ClsPubIncomeReport IncomeReport = new ClsPubIncomeReport();

                    //IncomeReport.LOBName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    IncomeReport.Method = StringParse(reader[ClsPubDALConstants.Method]);
                    IncomeReport.LocationName1 = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                    IncomeReport.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    IncomeReport.Denomintion = DecimalParse(reader[ClsPubDALConstants.DENOMINATION]);
                    ListIncomeReport.MainReportLoc.Add(IncomeReport);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return ListIncomeReport;
        }



        #endregion 
    }
}
