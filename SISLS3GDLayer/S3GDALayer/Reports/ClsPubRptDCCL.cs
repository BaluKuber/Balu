using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity.Reports;
using Microsoft.Practices.EnterpriseLibrary;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Data;
using S3GDALayer.Constants;
using S3GBusEntity;

namespace S3GDALayer.Reports
{
    public class ClsPubRptDCCL : ClsPubDalReportBase
    {
        #region Load Demand Collection Customer Level LOB
        /// <summary>
        /// To Get the Demand Collection Customer Level LOB
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubGetDemandCustLOB(int CompanyId, int UserId, int ProgramId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetDemandCustLOB);
                //ClsPubDALConstants.SPGETGROUP);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                //IDataReader reader = db.ExecuteReader(command); modified by Sangeetha on 30th April for Oracle Conversion
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

        #region Load Customer Group Name
        /// <summary>
        /// To Get the Customer Group Name
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubGetGroup(int CompanyId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetGroup);
                    //ClsPubDALConstants.SPGETGROUP);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);

                //IDataReader reader = db.ExecuteReader(command); modified by Sangeetha on 30th April for Oracle Conversion
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.GROUPID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.DESCRIPTION]);
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

        #region Load Prime Account Number based on Company ID
        /// <summary>
        /// To Get the Prime Account Number based on Company ID
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubGetDemandPAN(int CompanyId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetDemandPAN);
                    //ClsPubDALConstants.SPGETDEMANDPAN);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);

                //IDataReader reader = db.ExecuteReader(command); modified by Sangeetha on 30th April for Oracle Conversion
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.PANUM]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.PANUM]);
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

        #region Load Sub Account Number
        /// <summary>
        /// To Get the Sub Account Number
        /// </summary>
        /// <param name="PANum"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubGetDemandSAN(string PANum)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetDemandSAN);
                    //ClsPubDALConstants.SPGETDEMANDSAN);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, PANum);

                //IDataReader reader = db.ExecuteReader(command); modified by Sangeetha on 30th April for Oracle Conversion
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();

                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.SANUM]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.SANUM]);
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

        #region Load Customer Group and Prime Account  Number based on Customer
        /// <summary>
        /// To Get the Customer,Customer Group Name and Prime Account  Number based on each other
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="Value"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public ClsPubCustomerGroupPAN FunPubGetCustomerGroupPAN(string Option, string Value, int CompanyId)
        {
            ClsPubCustomerGroupPAN groupPAN = new ClsPubCustomerGroupPAN();
            groupPAN.Customerdetails = new List<ClsPubDropDownList>();
            groupPAN.CustomerGroupdetails = new List<ClsPubDropDownList>();
            groupPAN.Panumdetails = new List<ClsPubDropDownList>();

            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetCustomerGroupPAN);
                    //ClsPubDALConstants.SPGETCUSTOMERGROUPPAN);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMOPTION, DbType.String, Option);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMVALUE, DbType.String, Value);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                //IDataReader reader = db.ExecuteReader(command); modified by Sangeetha on 30th April for Oracle Conversion
                IDataReader reader = db.FunPubExecuteReader(command);

                while (reader.Read())
                {
                    ClsPubDropDownList customer = new ClsPubDropDownList();
                    customer.ID = StringParse(reader[ClsPubDALConstants.CUSTOMERID]);
                    customer.Description = StringParse(reader[ClsPubDALConstants.NAMEOFCUSTOMER]);
                    groupPAN.Customerdetails.Add(customer);
                }
                reader.NextResult();

                while (reader.Read())
                {
                    ClsPubDropDownList customerGroup = new ClsPubDropDownList();
                    customerGroup.ID = StringParse(reader[ClsPubDALConstants.GROUPID]);
                    customerGroup.Description = StringParse(reader[ClsPubDALConstants.GROUPNAME]);
                    groupPAN.CustomerGroupdetails.Add(customerGroup);
                }
                reader.NextResult();

                while (reader.Read())
                {
                    ClsPubDropDownList panum = new ClsPubDropDownList();
                    panum.ID = StringParse(reader[ClsPubDALConstants.PANUM]);
                    panum.Description = StringParse(reader[ClsPubDALConstants.PANUM]);
                    groupPAN.Panumdetails.Add(panum);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }

            return groupPAN;
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

        #region Load Frequency Details
        ///// <summary>
        ///// To get Frequency Details
        ///// </summary>

        //public List<ClsPubFrequencyDetails> FunPubGetFrequencyDetails()
        //{
        //    List<ClsPubFrequencyDetails> FrequencyDetails = new List<ClsPubFrequencyDetails>();
        //    try
        //    {
        //        DbCommand command = db.GetStoredProcCommand(SPNames.GetFrequency);
        //            //ClsPubDALConstants.SPGETFREQUENCY);
        //        IDataReader reader = db.ExecuteReader(command);
        //        while (reader.Read())
        //        {
        //            ClsPubFrequencyDetails FrequencyDetail = new ClsPubFrequencyDetails();
        //            FrequencyDetail.Id = IntParse(reader[ClsPubDALConstants.FREQUENCYID]);
        //            FrequencyDetail.Name = StringParse(reader[ClsPubDALConstants.FREQUENCYNAME]);

        //            FrequencyDetails.Add(FrequencyDetail);
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
        //    }
        //    return FrequencyDetails;
        //}
        #endregion

        //#region To Load Asset Categories
        ///// <summary>
        ///// To Get the Asset Categories
        ///// </summary>
        ///// <param name="CompanyId"></param>
        ///// <param name="AssetTypeId"></param>
        ///// <returns></returns>
        //public List<ClsPubAssetCategories> FunPubGetAssetCategories(int CompanyId, int AssetTypeId)
        //{
        //    List<ClsPubAssetCategories> AssetCategories = new List<ClsPubAssetCategories>();
        //    try
        //    {
        //        DbCommand command = db.GetStoredProcCommand(SPNames.GetAssetCategories);
        //            //ClsPubDALConstants.SPGETASSETCATEGORIES);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
        //        if (AssetTypeId != 0)
        //        {
        //            db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMASSETTYPEID, DbType.Int32, AssetTypeId);
        //        }
        //        IDataReader reader = db.ExecuteReader(command);
        //        while (reader.Read())
        //        {
        //            ClsPubAssetCategories AssetCategorie = new ClsPubAssetCategories();
        //            AssetCategorie.ClassId = IntParse(reader[ClsPubDALConstants.ASSETCLASSID]);
        //            AssetCategorie.AssetClass = StringParse(reader[ClsPubDALConstants.ASSETCLASS]);
        //            AssetCategorie.MakeId = IntParse(reader[ClsPubDALConstants.ASSETMAKEID]);
        //            AssetCategorie.AssetMake = StringParse(reader[ClsPubDALConstants.ASSETMAKE]);
        //            AssetCategorie.TypeId = IntParse(reader[ClsPubDALConstants.ASSETTYPE]);
        //            AssetCategorie.AssetType = StringParse(reader[ClsPubDALConstants.ASSETTYPE]);

        //            AssetCategories.Add(AssetCategorie);
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
        //    }
        //    return AssetCategories;
        //}
        //#endregion

        #region To Load Demand Collection Customer Level Grid
        public List<ClsPubDemandCollection> FunPubGetDCCLDetails(ClsPubDemandParameterDetails Demand)
        {
            List<ClsPubDemandCollection> DemandCollections = new List<ClsPubDemandCollection>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetDCCLDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, Demand.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, Demand.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, Demand.LobId);
                if (Demand.CustomerId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.Int32, Demand.CustomerId);
                }
                if (Demand.GroupId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMGROUPID, DbType.Int32, Demand.GroupId);
                }
                if (Demand.PANum != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, Demand.PANum);
                }
                if (Demand.SANum != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSANUM, DbType.String, Demand.SANum);
                }
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFREQUENCYID, DbType.Int32, Demand.FrequencyId);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMASSETTYPEID, DbType.Int32, Demand.AssetTypeId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFINYEARSTARTMONTHSTARTDATE, DbType.DateTime, Demand.FinYearStartMonthStartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFROMMONTHSTARTDATE, DbType.DateTime, Demand.FromMonthStartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMFROMMONTHPREMONTHENDDATE, DbType.DateTime, Demand.FromMonthPreMonthEndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTOMONTHENDDATE, DbType.DateTime, Demand.ToMonthEndDate);

                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPAREFINYEARSTARTMONTHSTARTDATE, DbType.DateTime, Demand.CompareFinYearStartMonthStartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPAREFROMMONTHSTARTDATE, DbType.DateTime, Demand.CompareFromMonthStartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPAREFROMMONTHPREMONTHENDDATE, DbType.DateTime, Demand.CompareFromMonthPreMonthEndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPARETOMONTHENDDATE, DbType.DateTime, Demand.CompareToMonthEndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Int32, Demand.Denomination);
                //IDataReader reader = db.ExecuteReader(command); modified by Sangeetha on 30th April for Oracle Conversion
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDemandCollection DemandCollection = new ClsPubDemandCollection();
                    //DemandCollection.Frequency = StringParse(reader[ClsPubDALConstants.FREQUENCY]);
                    //DemandCollection.Month = IntParse(reader[ClsPubDALConstants.MONTH]);
                    //DemandCollection.Denomination=
                    DemandCollection.DemandMonth = StringParse(reader[ClsPubDALConstants.DEMANDMONTH]);
                    //DemandCollection.RegionId = IntParse(reader[ClsPubDALConstants.REGIONID]);
                    //DemandCollection.Region = StringParse(reader[ClsPubDALConstants.REGION]);
                    //DemandCollection.BranchId = IntParse(reader[ClsPubDALConstants.BRANCHID]);
                    //DemandCollection.Branch = StringParse(reader[ClsPubDALConstants.BRANCH]);
                    DemandCollection.Location = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    DemandCollection.CustomerName = StringParse(reader[ClsPubDALConstants.CUSTOMERNAME]);
                    DemandCollection.Panum = StringParse(reader[ClsPubDALConstants.PANUM]);
                    DemandCollection.Sanum = StringParse(reader[ClsPubDALConstants.SANUM]);
                    //DemandCollection.ClassId = IntParse(reader[ClsPubDALConstants.ASSETCLASSID]);
                    //DemandCollection.AssetClass = StringParse(reader[ClsPubDALConstants.ASSETCLASS]);
                    //DemandCollection.MakeId = IntParse(reader[ClsPubDALConstants.ASSETMAKEID]);
                    //DemandCollection.AssetMake = StringParse(reader[ClsPubDALConstants.ASSETMAKE]);
                    //DemandCollection.TypeId = IntParse(reader[ClsPubDALConstants.ASSETTYPEID]);
                    //DemandCollection.AssetType = StringParse(reader[ClsPubDALConstants.ASSETTYPE]);
                    DemandCollection.OpeningDemand = DecimalParse(reader[ClsPubDALConstants.OPENINGDEMAND]);
                    DemandCollection.OpeningCollection = DecimalParse(reader[ClsPubDALConstants.OPENINGCOLLECTION]);
                    DemandCollection.OpeningPercentage = DecimalParse(reader[ClsPubDALConstants.OPENINGPERCENTAGE]);
                    DemandCollection.MonthlyDemand = DecimalParse(reader[ClsPubDALConstants.MONTHLYDEMAND]);
                    DemandCollection.MonthlyCollection = DecimalParse(reader[ClsPubDALConstants.MONTHLYCOLLECTION]);
                    DemandCollection.MonthlyPercentage = DecimalParse(reader[ClsPubDALConstants.MONTHLYPERCENTAGE]);
                    DemandCollection.ClosingDemand = DecimalParse(reader[ClsPubDALConstants.CLOSINGDEMAND]);
                    DemandCollection.ClosingCollection = DecimalParse(reader[ClsPubDALConstants.CLOSINGCOLLECTION]);
                    DemandCollection.ClosingPercentage = DecimalParse(reader[ClsPubDALConstants.CLOSINGPERCENTAGE]);
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

        //#region Asset Categories Type
        //public List<ClsPubDropDownList> FunPubGetAssetCategoriesType()
        //{
        //    List<ClsPubDropDownList> AssetCategoriesTypes = new List<ClsPubDropDownList>();
        //    try
        //    {
        //        DbCommand command = db.GetStoredProcCommand(SPNames.GetAssetCategoriesType);
        //            //ClsPubDALConstants.SPGETASSETCATEGORIESTYPE);
        //        IDataReader reader = db.ExecuteReader(command);
        //        while (reader.Read())
        //        {
        //            ClsPubDropDownList AssetCategoriesType = new ClsPubDropDownList();
        //            AssetCategoriesType.ID = StringParse(reader[ClsPubDALConstants.CATEGORIESID]);
        //            AssetCategoriesType.Description = StringParse(reader[ClsPubDALConstants.CATEGORIESNAME]);

        //            AssetCategoriesTypes.Add(AssetCategoriesType);
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
        //    }
        //    return FunPriLoadSelect(AssetCategoriesTypes);
        //}
        //#endregion


        #region Load Group and Prime Account Number based on LOB and Customer 
        /// <summary>
        /// To Load Group and Prime Account Number based on LOB and Customer 
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="Value"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public ClsPubCustomerGroupPAN FunPubGetGroupPAN(ClsPubDCHeaderParams objDCHeaderParams)
        {
            ClsPubCustomerGroupPAN groupPAN = new ClsPubCustomerGroupPAN();
           // groupPAN.Customerdetails = new List<ClsPubDropDownList>();
            groupPAN.LOBdetails = new List<ClsPubDropDownList>();
            groupPAN.CustomerGroupdetails = new List<ClsPubDropDownList>();
            groupPAN.Panumdetails = new List<ClsPubDropDownList>();
            //groupPAN.PANUM = new List<ClsPubDCHeaderParams>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetCustomerGroupPAN);
                //ClsPubDALConstants.SPGETCUSTOMERGROUPPAN);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMOPTION, DbType.String, objDCHeaderParams.Option );
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMVALUE, DbType.String, Value);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, objDCHeaderParams .Company_ID );
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, objDCHeaderParams .Lob_ID );
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID , DbType.Int32, objDCHeaderParams .Customer_ID );
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMGROUPID , DbType.Int32, objDCHeaderParams .Group_ID );
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, objDCHeaderParams.PANum );
                //IDataReader reader = db.ExecuteReader(command); modified by Sangeetha on 30th April for Oracle Conversion
                IDataReader reader = db.FunPubExecuteReader(command);

                if (objDCHeaderParams.Option == "L")
                {
                    while (reader.Read())
                    {
                        ClsPubDropDownList panum = new ClsPubDropDownList();
                        panum.ID = StringParse(reader[ClsPubDALConstants.PANUM]);
                        panum.Description = StringParse(reader[ClsPubDALConstants.PANUM]);
                        groupPAN.Panumdetails.Add(panum);
                    }
                    reader.Close();
                }
                else if (objDCHeaderParams.Option == "LG")
                {
                    while (reader.Read())
                    {
                        ClsPubDropDownList panum = new ClsPubDropDownList();
                        panum.ID = StringParse(reader[ClsPubDALConstants.PANUM]);
                        panum.Description = StringParse(reader[ClsPubDALConstants.PANUM]);
                        groupPAN.Panumdetails.Add(panum);
                    }
                    reader.Close();
                }
                else if (objDCHeaderParams.Option == "P")
                {
                    while (reader.Read())
                    {
                        groupPAN.Lob_ID = StringParse(reader[ClsPubDALConstants.LOBID]);
                        groupPAN.Group_ID = StringParse(reader[ClsPubDALConstants.GROUPID]);
                        groupPAN.Customer_ID = StringParse(reader[ClsPubDALConstants.Customer_NAME]);

                        //ClsPubDCHeaderParams objDC = new ClsPubDCHeaderParams();
                        //objDC.Lob_ID = StringParse(reader[ClsPubDALConstants.LOBID]);
                        //objDC.Group_ID = StringParse(reader[ClsPubDALConstants.GROUPID]);
                        //objDC.Customer_ID  = StringParse(reader[ClsPubDALConstants.Customer_NAME]);
                        //groupPAN.PANUM .Add(objDC);
                    }
                    reader.Close();
                }
                else if (objDCHeaderParams.Option == "C")
                {
                    while (reader.Read())
                    {
                        ClsPubDropDownList customerGroup = new ClsPubDropDownList();
                        customerGroup.ID = StringParse(reader[ClsPubDALConstants.GROUPID]);
                        customerGroup.Description = StringParse(reader[ClsPubDALConstants.GROUPNAME]);
                        groupPAN.CustomerGroupdetails.Add(customerGroup);
                    }
                    reader.NextResult();

                    while (reader.Read())
                    {
                        ClsPubDropDownList panum = new ClsPubDropDownList();
                        panum.ID = StringParse(reader[ClsPubDALConstants.PANUM]);
                        panum.Description = StringParse(reader[ClsPubDALConstants.PANUM]);
                        groupPAN.Panumdetails.Add(panum);
                    }
                    reader.Close();
                }

                else
                {
                    while (reader.Read())
                    {
                        ClsPubDropDownList LOB = new ClsPubDropDownList();
                        LOB.ID = StringParse(reader[ClsPubDALConstants.LOBID]);
                        LOB.Description = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                        groupPAN.LOBdetails.Add(LOB);
                    }
                    reader.NextResult();

                    while (reader.Read())
                    {
                        ClsPubDropDownList customerGroup = new ClsPubDropDownList();
                        customerGroup.ID = StringParse(reader[ClsPubDALConstants.GROUPID]);
                        customerGroup.Description = StringParse(reader[ClsPubDALConstants.GROUPNAME]);
                        groupPAN.CustomerGroupdetails.Add(customerGroup);
                    }
                    reader.NextResult();

                    while (reader.Read())
                    {
                        ClsPubDropDownList panum = new ClsPubDropDownList();
                        panum.ID = StringParse(reader[ClsPubDALConstants.PANUM]);
                        panum.Description = StringParse(reader[ClsPubDALConstants.PANUM]);
                        groupPAN.Panumdetails.Add(panum);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }

            return groupPAN;
        }
        #endregion
    }
}