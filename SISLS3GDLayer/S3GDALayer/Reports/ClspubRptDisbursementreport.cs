using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity;
using S3GDALayer.Constants;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity.Reports;
using System.Data.Common;
using System.Data.Sql;
using System.Xml.Linq;
using System.Data;

namespace S3GDALayer.Reports
{
    public class ClspubRptDisbursementreport:ClsPubDalReportBase 
    {
        
        public List<ClsPubDropDownList> FunPubGetRegion(int CompanyId, bool Is_Active)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETREGION);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Int32, Is_Active);
                IDataReader reader = db.ExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.REGIONID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.REGIONNAME]);
                    dropdownLists.Add(dropDownList);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                throw ex;
            }
            return FunPriLoadSelect(dropdownLists);
        }
        public List<ClsPubDropDownList> FunPubGetRegBranch(int CompanyId, int UserId, string Region_Id, bool Is_Active)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {

                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETREGBRANCHDETAILS);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                if (Region_Id != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMREGIONID , DbType.Int32, Region_Id);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Int32, Is_Active);
                IDataReader reader = db.ExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.BRANCHID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.BRANCH]);
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
       
        
    }
}
