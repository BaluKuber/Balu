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
    public class ClsPubRptDemandCollection : ClsPubDalReportBase
    {
        #region Load Frequency Details
        /// <summary>
        /// To get Frequency Details
        /// </summary>
        
        public List<ClsPubFrequencyDetails> FunPubGetFrequencyDetails()
        {
            List<ClsPubFrequencyDetails> FrequencyDetails = new List<ClsPubFrequencyDetails>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetFrequency);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubFrequencyDetails FrequencyDetail = new ClsPubFrequencyDetails();
                    FrequencyDetail.Id = IntParse(reader[ClsPubDALConstants.FREQUENCYID]);
                    FrequencyDetail.Name = StringParse(reader[ClsPubDALConstants.FREQUENCYNAME]);

                    FrequencyDetails.Add(FrequencyDetail);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return FrequencyDetails;
        }
        #endregion 

        #region Load Asset Categories
        /// <summary>
        /// To get Asset Categories
        /// </summary>
        public List<ClsPubAssetCategories> FunPubGetAssetCategories(int CompanyId, int AssetTypeId)
        {
            List<ClsPubAssetCategories> AssetCategories = new List<ClsPubAssetCategories>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetAssetCategories);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                if (AssetTypeId != 0)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMASSETTYPEID, DbType.Int32, AssetTypeId);
                }
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubAssetCategories AssetCategorie = new ClsPubAssetCategories();
                    AssetCategorie.ClassId = IntParse(reader[ClsPubDALConstants.ASSETCLASSID]);
                    AssetCategorie.AssetClass= StringParse(reader[ClsPubDALConstants.ASSETCLASS]);
                    AssetCategorie.MakeId = IntParse(reader[ClsPubDALConstants.ASSETMAKEID]);
                    AssetCategorie.AssetMake = StringParse(reader[ClsPubDALConstants.ASSETMAKE]);
                    AssetCategorie.TypeId = IntParse(reader[ClsPubDALConstants.ASSETTYPEID]);
                    AssetCategorie.AssetType = StringParse(reader[ClsPubDALConstants.ASSETTYPE]);

                    AssetCategories.Add(AssetCategorie);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return AssetCategories;
        }
        #endregion

        #region Load Demand Collection Details

        /// <summary>
        /// To Bind Demand Collection Details
        /// </summary>
        /// <param name="Demand"></param>
        /// <returns></returns>
        public List<ClsPubDemandCollection> FunPubGetDemandCollection(ClsPubDemandParameterDetails Demand)
        {
            List<ClsPubDemandCollection> DemandCollections = new List<ClsPubDemandCollection>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetDemandCollectionDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, Demand.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, Demand.UserId);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, Demand.LobId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, Demand.ProgramId);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, Demand.LocationId1);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, Demand.LocationId2);

                //if (Demand.RegionId != string.Empty)
                //{
                //    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMREGIONID, DbType.Int32, Demand.RegionId);
                //}
                //if (Demand.BranchId != string.Empty)
                //{
                //    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMBRANCHID, DbType.Int32, Demand.BranchId);
                //}
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFINYEARSTARTMONTHSTARTDATE, DbType.DateTime, Demand.FinYearStartMonthStartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFROMMONTHSTARTDATE, DbType.DateTime, Demand.FromMonthStartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFROMMONTHPREMONTHENDDATE, DbType.DateTime, Demand.FromMonthPreMonthEndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTOMONTHENDDATE, DbType.DateTime, Demand.ToMonthEndDate);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPAREFINYEARSTARTMONTHSTARTDATE, DbType.DateTime, Demand.CompareFinYearStartMonthStartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPAREFROMMONTHSTARTDATE, DbType.DateTime, Demand.CompareFromMonthStartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPAREFROMMONTHPREMONTHENDDATE, DbType.DateTime, Demand.CompareFromMonthPreMonthEndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPARETOMONTHENDDATE, DbType.DateTime, Demand.CompareToMonthEndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Int32, Demand.Denomination);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDemandCollection DemandCollection = new ClsPubDemandCollection();
                    DemandCollection.DemandMonth = StringParse(reader[ClsPubDALConstants.DEMANDMONTH]);
                    DemandCollection.Region = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    //DemandCollection.Branch = StringParse(reader[ClsPubDALConstants.BRANCH]);
                    DemandCollection.OpeningDemand = DecimalParse(reader[ClsPubDALConstants.OPENINGDEMAND]);
                    DemandCollection.OpeningCollection = DecimalParse(reader[ClsPubDALConstants.OPENINGCOLLECTION]);
                    DemandCollection.OpeningPercentage = DecimalParse(reader[ClsPubDALConstants.OPENINGPERCENTAGE]);
                    DemandCollection.MonthlyDemand = DecimalParse(reader[ClsPubDALConstants.MONTHLYDEMAND]);
                    DemandCollection.MonthlyCollection = DecimalParse(reader[ClsPubDALConstants.MONTHLYCOLLECTION]);
                    DemandCollection.MonthlyPercentage = DecimalParse(reader[ClsPubDALConstants.MONTHLYPERCENTAGE]);
                    DemandCollection.ClosingDemand = DecimalParse(reader[ClsPubDALConstants.CLOSINGDEMAND]);
                    DemandCollection.ClosingCollection = DecimalParse(reader[ClsPubDALConstants.CLOSINGCOLLECTION]);
                    DemandCollection.ClosingPercentage = DecimalParse(reader[ClsPubDALConstants.CLOSINGPERCENTAGE]);
                    DemandCollection.Denomination = StringParse(reader[ClsPubDALConstants.Denomination]);
                    DemandCollection.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    DemandCollections.Add(DemandCollection);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return DemandCollections;
        }
        #endregion

        #region Asset Wise Demand Collection

        /// <summary>
        /// To Bind Asset Wise Demand Collection Details
        /// </summary>
        /// <param name="Demand"></param>
        /// <returns></returns>
        public List<ClsPubDemandCollection> FunPubGetDemandCollectionDetails(ClsPubDemandParameterDetails Demand)
        {
            List<ClsPubDemandCollection> DemandCollections = new List<ClsPubDemandCollection>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetAssetWiseDemand);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, Demand.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, Demand.LobId);
                if (Demand.RegionId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMREGIONID, DbType.Int32, Demand.RegionId);
                }
                if (Demand.BranchId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMBRANCHID, DbType.Int32, Demand.BranchId);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCLASSID, DbType.Int32, Demand.ClassId);

                if (Demand.AssetTypeId != 0)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMASSETTYPEID, DbType.Int32, Demand.AssetTypeId);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFROMMONTH, DbType.String, Demand.FromMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTOMONTH, DbType.String, Demand.ToMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPREFROMMONTH, DbType.String, Demand.PreFromMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRETOMONTH, DbType.String, Demand.PreToMonth);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Int32, Demand.Denomination);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDemandCollection DemandCollection = new ClsPubDemandCollection();
                    DemandCollection.DemandMonth = StringParse(reader[ClsPubDALConstants.DEMANDMONTH]);
                    DemandCollection.Region = StringParse(reader[ClsPubDALConstants.REGION]);
                    DemandCollection.Branch = StringParse(reader[ClsPubDALConstants.BRANCH]);
                    DemandCollection.ClassId = IntParse(reader[ClsPubDALConstants.ASSETCLASSID]);
                    DemandCollection.AssetClass = StringParse(reader[ClsPubDALConstants.ASSETCLASS]);
                    DemandCollection.Panum = StringParse(reader[ClsPubDALConstants.PANUM]);
                    DemandCollection.Sanum = StringParse(reader[ClsPubDALConstants.SANUM]);
                    DemandCollection.ArrearsAmount = DecimalParse(reader[ClsPubDALConstants.ARREARSAMOUNT]);
                    DemandCollection.OpeningDemand = DecimalParse(reader[ClsPubDALConstants.OPENINGDEMAND]);
                    DemandCollection.OpeningCollection = DecimalParse(reader[ClsPubDALConstants.OPENINGCOLLECTION]);
                    DemandCollection.OpeningPercentage = DecimalParse(reader[ClsPubDALConstants.OPENINGPERCENTAGE]);
                    DemandCollection.MonthlyDemand = DecimalParse(reader[ClsPubDALConstants.MONTHLYDEMAND]);
                    DemandCollection.MonthlyCollection = DecimalParse(reader[ClsPubDALConstants.MONTHLYCOLLECTION]);
                    DemandCollection.MonthlyPercentage = DecimalParse(reader[ClsPubDALConstants.MONTHLYPERCENTAGE]);
                    DemandCollection.ClosingDemand = DecimalParse(reader[ClsPubDALConstants.CLOSINGDEMAND]);
                    DemandCollection.ClosingCollection = DecimalParse(reader[ClsPubDALConstants.CLOSINGCOLLECTION]);
                    DemandCollection.ClosingPercentage = DecimalParse(reader[ClsPubDALConstants.CLOSINGPERCENTAGE]);
                    DemandCollections.Add(DemandCollection);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return DemandCollections;
        }
        #endregion

        #region Asset Categories Types
        /// <summary>
        /// To bind Asset Categories Types
        /// </summary>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubGetAssetCategoriesType()
        {
            List<ClsPubDropDownList> AssetCategoriesTypes = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetAssetCategoriesType);
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
