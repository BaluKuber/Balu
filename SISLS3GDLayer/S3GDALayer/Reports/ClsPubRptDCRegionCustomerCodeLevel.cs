using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity.Reports;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using S3GDALayer.Constants;

namespace S3GDALayer.Reports
{
   public class ClsPubRptDCRegionCustomerCodeLevel:ClsPubDalReportBase
   {
       #region GetDemandCollectionCustomerCodeLevelDetails
       public List<ClsPubDCRegionCustomerCodeGridDetails> FunPubGetDemandCollectionReginCustomerCodeLevel(ClsPubDemandParameterDetails HeaderDetails)
       {
           List<ClsPubDCRegionCustomerCodeGridDetails> objGrdDetails = new List<ClsPubDCRegionCustomerCodeGridDetails>();
           try
           {
               DbCommand command = db.GetStoredProcCommand(SPNames.GetDemandCollectionRegionwiseDetails);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32,HeaderDetails.CompanyId);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, HeaderDetails.LobId);               
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, HeaderDetails.ProgramId);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, HeaderDetails.UserId);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, HeaderDetails.LocationId1);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, HeaderDetails.LocationId2);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMGROUPINGTYPE, DbType.Int32, HeaderDetails.GroupingCriteriaId);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFINYEARSTARTMONTHSTARTDATE, DbType.DateTime, HeaderDetails.FinYearStartMonthStartDate);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFROMMONTHSTARTDATE, DbType.DateTime, HeaderDetails.FromMonthStartDate);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFROMMONTHPREMONTHENDDATE, DbType.DateTime, HeaderDetails.FromMonthPreMonthEndDate);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTOMONTHENDDATE, DbType.DateTime, HeaderDetails.ToMonthEndDate);

               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPAREFINYEARSTARTMONTHSTARTDATE, DbType.DateTime, HeaderDetails.CompareFinYearStartMonthStartDate);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPAREFROMMONTHSTARTDATE, DbType.DateTime, HeaderDetails.CompareFromMonthStartDate);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPAREFROMMONTHPREMONTHENDDATE, DbType.DateTime, HeaderDetails.CompareFromMonthPreMonthEndDate);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPARETOMONTHENDDATE, DbType.DateTime, HeaderDetails.CompareToMonthEndDate);
               //if (HeaderDetails.AssetTypeId != 0)
               //{
               //    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMASSETTYPEID, DbType.Int32, HeaderDetails.AssetTypeId);
               //}
               //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFREQUENCYID, DbType.Int32, HeaderDetails.FrequencyId);

               //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFROMMONTH, DbType.String, HeaderDetails.FromMonth);
               //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTOMONTH, DbType.String, HeaderDetails.ToMonth);
               db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Int32, HeaderDetails.Denomination);
               //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPREFROMMONTH, DbType.String, HeaderDetails.PreFromMonth);
               //db.AddInParameter(command,ClsPubDALConstants.INPUTPARAMPRETOMONTH,DbType.String,HeaderDetails.PreToMonth);
               IDataReader reader = db.FunPubExecuteReader(command);
               while(reader.Read())
               {
                   ClsPubDCRegionCustomerCodeGridDetails objcustomercodelevel =new ClsPubDCRegionCustomerCodeGridDetails();
                   objcustomercodelevel.DemandMonth = StringParse(reader[ClsPubDALConstants.DEMANDMONTH]);
                   objcustomercodelevel.LobId = StringParse(reader[ClsPubDALConstants.LOBID]);
                   //objcustomercodelevel.Frequency = StringParse(reader[ClsPubDALConstants.FREQUENCY]);
                   objcustomercodelevel.LineOfBusiness=StringParse(reader[ClsPubDALConstants.LOBNAME]);
                   //objcustomercodelevel.RegionId = StringParse(reader[ClsPubDALConstants.REGIONID]);
                   //objcustomercodelevel.Region = StringParse(reader[ClsPubDALConstants.REGION]);
                   objcustomercodelevel.BranchId = StringParse(reader[ClsPubDALConstants.LOCATION_ID]);
                   objcustomercodelevel.Branch = StringParse(reader[ClsPubDALConstants.BRANCH]);
                   objcustomercodelevel.CustomerName=StringParse(reader[ClsPubDALConstants.CUSTOMERNAME]);
                   objcustomercodelevel.CustomerGroupCode=StringParse(reader[ClsPubDALConstants.CUSTOMERGROUPNAME]);
                   objcustomercodelevel.PrimeAccountNo=StringParse(reader[ClsPubDALConstants.PANUM]);
                   objcustomercodelevel.SubAccountNo=StringParse(reader[ClsPubDALConstants.SANUM]);
                   objcustomercodelevel.OpeningDemand = DecimalParse(reader[ClsPubDALConstants.OPENINGDEMAND]);
                   objcustomercodelevel.OpeningCollection = DecimalParse(reader[ClsPubDALConstants.OPENINGCOLLECTION]);
                   objcustomercodelevel.OpeningPercentage = DecimalParse(reader[ClsPubDALConstants.OPENINGPERCENTAGE]);
                   objcustomercodelevel.MonthlyDemand = DecimalParse(reader[ClsPubDALConstants.MONTHLYDEMAND]);
                   objcustomercodelevel.MonthlyCollection = DecimalParse(reader[ClsPubDALConstants.MONTHLYCOLLECTION]);
                   objcustomercodelevel.MonthlyPercentage = DecimalParse(reader[ClsPubDALConstants.MONTHLYPERCENTAGE]);
                   objcustomercodelevel.ClosingDemand = DecimalParse(reader[ClsPubDALConstants.CLOSINGDEMAND]);
                   objcustomercodelevel.ClosingCollection = DecimalParse(reader[ClsPubDALConstants.CLOSINGCOLLECTION]);
                   objcustomercodelevel.ClosingPercentage = DecimalParse(reader[ClsPubDALConstants.CLOSINGPERCENTAGE]);
                   objcustomercodelevel.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                   objGrdDetails.Add(objcustomercodelevel);
               }
           }           
           catch (Exception e)
           {
                ClsPubCommErrorLogDal.CustomErrorRoutine(e);
               throw e;
           }
           return objGrdDetails;
       }
       #endregion
       #region AssetcategoriesType
       public List<ClsPubDropDownList> FunPubGetAssetCategoriesType()
       {
           List<ClsPubDropDownList> AssetCategoriesTypes = new List<ClsPubDropDownList>();
           try
           {
               DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETASSETCATEGORIESTYPE);
               IDataReader reader = db.FunPubExecuteReader(command);
               while (reader.Read())
               {
                   ClsPubDropDownList AssetCategoriesType = new ClsPubDropDownList();
                   AssetCategoriesType.ID = StringParse(reader[ClsPubDALConstants.CATEGORIESID]);
                   AssetCategoriesType.Description = StringParse(reader[ClsPubDALConstants.CATEGORIESNAME]);

                   AssetCategoriesTypes.Add(AssetCategoriesType);
               }
               reader.Close();
           }
           catch (Exception ex)
           {
                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
           }
           return FunPriLoadSelect(AssetCategoriesTypes);
       }
       #endregion
   }
}
