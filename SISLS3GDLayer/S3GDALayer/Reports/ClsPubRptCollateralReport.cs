using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using S3GBusEntity.Reports;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.ObjectBuilder2;
using S3GDALayer.Constants;

namespace S3GDALayer.Reports
{
    public class ClsPubRptCollateralReport : ClsPubDalReportBase
    {
        public List<ClsPubCollateralReport> FunPubGetCollateralReport(ClsPubCollateralHeader headerDetails)
        {
            List<ClsPubCollateralReport> CollateralDetails = new List<ClsPubCollateralReport>();

            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.S3G_RPT_GetCollateralReport);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, headerDetails.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, headerDetails.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.Int32, headerDetails.CustomerId);
                if (headerDetails.LOBId != "-1" && headerDetails.LOBId != "0" && headerDetails.LOBId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, headerDetails.LOBId);
                }
                if (headerDetails.LocationId1 != "-1" && headerDetails.LocationId1 != "0" && headerDetails.LocationId1 != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, headerDetails.LocationId1);
                }
                if (headerDetails.LocationId2 != "-1" && headerDetails.LocationId2 != "0" && headerDetails.LocationId2 != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, headerDetails.LocationId2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, headerDetails.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOLLATERALTYPEID, DbType.Int32, headerDetails.CollateralTypeId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOLLATERALSTATUSID, DbType.Int32, headerDetails.StatusId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, headerDetails.StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, headerDetails.EndDate);                
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubCollateralReport clspubReportDetails=new ClsPubCollateralReport();
                    clspubReportDetails.LineOfBusiness = StringParse(reader[ClsPubDALConstants.LOB]);
                    clspubReportDetails.Location = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    clspubReportDetails.CustomerName = StringParse(reader[ClsPubDALConstants.Customer_NAME]);
                    //customerGlanceDetail.Region = StringParse(reader[ClsPubDALConstants.REGION]);
                    //customerGlanceDetail.Branch = StringParse(reader[ClsPubDALConstants.BRANCHNAME]);
                    clspubReportDetails.ReferenceNo = StringParse(reader[ClsPubDALConstants.REF_NO]);
                    clspubReportDetails.AssetDesc = StringParse(reader[ClsPubDALConstants.ASSET_DESC]);
                    clspubReportDetails.Units = StringParse(reader[ClsPubDALConstants.UNITS]);
                    clspubReportDetails.Value = StringParse(reader[ClsPubDALConstants.VALUE]);
                    clspubReportDetails.City = StringParse(reader[ClsPubDALConstants.CITY]);
                    clspubReportDetails.Address = StringParse(reader[ClsPubDALConstants.ADDRESS]);
                    clspubReportDetails.Storage1 = StringParse(reader[ClsPubDALConstants.STORAGE1]);
                    clspubReportDetails.Storage2 = StringParse(reader[ClsPubDALConstants.STORAGE2]);
                    clspubReportDetails.Storage3 = StringParse(reader[ClsPubDALConstants.STORAGE3]);
                    clspubReportDetails.Status = StringParse(reader[ClsPubDALConstants.STATUS]);
                    clspubReportDetails.StatusDate = StringParse(reader[ClsPubDALConstants.STATUS_DATE]);
                    clspubReportDetails.GPSSUFFIX = DecimalParse(reader[ClsPubDALConstants.Gpssuffix]);
                    clspubReportDetails.CompanyName=StringParse(reader[ClsPubDALConstants.COMPANYNAME]);
                    clspubReportDetails.CompanyAddress = StringParse(reader[ClsPubDALConstants.COMPANYADDRESS]);
                    clspubReportDetails.CollateralType = StringParse(reader[ClsPubDALConstants.COLLATERALTYPE]);
                    //customerGlanceDetail.Pending=DecimalParse(reader[ClsPubDALConstants.PENDING]);
                    CollateralDetails.Add(clspubReportDetails);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(e);
                throw e;
            }
            return CollateralDetails;
        }

        public List<ClsPubDropDownList> FunPubGetCollateralType(int intCompanyID)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.S3G_RPT_GetCollateralType);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, intCompanyID);                                
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.COLLATERALTYPEID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.COLLATERALTYPENAME]);
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
    }
}
