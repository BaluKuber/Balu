using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity.Reports;
using S3GDALayer.Constants;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data;
using S3GBusEntity;


namespace S3GDALayer.Reports
{
    public class ClsPubRptDemandStatement : ClsPubDalReportBase
    {
        public List<ClsPubDropDownList> FunPubGetCategory(int CompanyId)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetDemandStatementCategory);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                 IDataReader reader = db.ExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.CATEGORYID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.CATEGORY]);
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





        public List<ClsPubDropDownList> FunPubGetDebtCollectorCode(ClsPubDemandSelectionCriteria demandselectioncriteria)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetDebtCollectorRuleCard);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, demandselectioncriteria.CompanyId);
                if (demandselectioncriteria.LobId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, demandselectioncriteria.LobId);
                }
                if (demandselectioncriteria.LocationID1 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, demandselectioncriteria.LocationID1);
                }
                if (demandselectioncriteria.LocationID2 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, demandselectioncriteria.LocationID2);
                }

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUTOFFMONTH, DbType.Int32,demandselectioncriteria.DEMANDMONTH);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, demandselectioncriteria.UserId);
                IDataReader reader = db.ExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.DEBTCODE]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.DEBTCODE]);
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

        public List<ClsPubDemandStatement> FunPubGetDemandStatement(ClsPubDemandSelectionCriteria demandselectioncriteria)
        {
            List<ClsPubDemandStatement> demands = new List<ClsPubDemandStatement>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetDemandStatement);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, demandselectioncriteria.CompanyId);
                if (demandselectioncriteria.LobId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, demandselectioncriteria.LobId);
                }
                if (demandselectioncriteria.LocationID1 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, demandselectioncriteria.LocationID1);
                }
                if (demandselectioncriteria.LocationID2 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, demandselectioncriteria.LocationID2);
                }
                if (demandselectioncriteria.CATEGORY_CODE != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCATEGORYCODE, DbType.Int32, demandselectioncriteria.CATEGORY_CODE);
                }
                if (demandselectioncriteria.DEBTCOLLECTOR_CODE != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDEBTCOLLECTORTYPE, DbType.String, demandselectioncriteria.DEBTCOLLECTOR_CODE);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDEMANDMONTH, DbType.Int32, demandselectioncriteria.DEMANDMONTH);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, demandselectioncriteria.enddate);
                if (demandselectioncriteria.option != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMOPTION, DbType.Int32, demandselectioncriteria.option);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, demandselectioncriteria.UserId);
                //IDataReader reader = db.ExecuteReader(command);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDemandStatement demand = new ClsPubDemandStatement();
                    demand.LobName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                    demand.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                    demand.CustomerCode = StringParse(reader[ClsPubDALConstants.CUSTOMERNAM]);
                    demand.PrimeAccountNo = StringParse(reader[ClsPubDALConstants.PRIMEACCOUNT]);
                    demand.SubAccountNo = StringParse(reader[ClsPubDALConstants.SUBACCOUNT]);
                    demand.DebtCollectorCode = StringParse(reader[ClsPubDALConstants.DEBTCOLLECTORCODE]);
                    demand.Category = StringParse(reader[ClsPubDALConstants.CATEGORYNAME]);
                    demand.DueAmount = DecimalParse(reader[ClsPubDALConstants.DUEAMOUNT]);
                    demand.ageing0days = DecimalParse(reader[ClsPubDALConstants.AGEING30DAYS]);
                    demand.ageing30days = DecimalParse(reader[ClsPubDALConstants.AGEING60DAYS]);
                    demand.ageing60days = DecimalParse(reader[ClsPubDALConstants.AGEING90DAYS]);
                    demand.ageingabove90days = DecimalParse(reader[ClsPubDALConstants.AGEINGABOVE90DAYS]);
                    demand.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    demands.Add(demand);
                }
            }

             catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return demands;
        }

       
    }
}
