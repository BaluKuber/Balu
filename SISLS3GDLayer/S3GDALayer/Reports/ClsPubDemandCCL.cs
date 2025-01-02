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
{//Commenting for getting a build(as you were absent today)....
    public class ClsPubDemandCCL : ClsPubDalReportBase
    {
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
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETGROUP);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);

                IDataReader reader = db.ExecuteReader(command);
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
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETDEMANDPAN);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);

                IDataReader reader = db.ExecuteReader(command);
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
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETDEMANDSAN);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, PANum);

                IDataReader reader = db.ExecuteReader(command);
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

        #region Load Customer,Customer Group Code and Prime Account  Number
        /// <summary>
        /// To Get the Customer,Customer Group Name and Prime Account  Number
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="Value"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public List<ClsPubCustomerGroupPAN> FunPubGetCustomerGroupPAN(string Option, string Value, int CompanyId)
        {
            List<ClsPubCustomerGroupPAN> CustomerGroupPANs = new List<ClsPubCustomerGroupPAN>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETCUSTOMERGROUPPAN);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMOPTION, DbType.String, Option);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMVALUE, DbType.String, Value);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);

                IDataReader reader = db.ExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubCustomerGroupPAN CustomerGroupPAN = new ClsPubCustomerGroupPAN();
                    //CustomerGroupPAN.CustomerId = StringParse(reader[ClsPubDALConstants.CUSTOMERID]);
                    //CustomerGroupPAN.CustomerName = StringParse(reader[ClsPubDALConstants.NAMEOFCUSTOMER]);
                    //CustomerGroupPAN.GroupId = StringParse(reader[ClsPubDALConstants.GROUPID]);
                    //CustomerGroupPAN.GroupName = StringParse(reader[ClsPubDALConstants.DESCRIPTION]);
                    //CustomerGroupPAN.PANum = StringParse(reader[ClsPubDALConstants.PANUM]);
                    CustomerGroupPANs.Add(CustomerGroupPAN);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return CustomerGroupPANs;
        }
        #endregion

        #region Load Frequency Details
        /// <summary>
        /// To get Frequency Details
        /// </summary>

        public List<ClsPubFrequencyDetails> FunPubGetFrequencyDetails()
        {
            List<ClsPubFrequencyDetails> FrequencyDetails = new List<ClsPubFrequencyDetails>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETFREQUENCY);
                IDataReader reader = db.ExecuteReader(command);
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

        #region To Load Asset Categories
        /// <summary>
        /// To Get the Asset Categories
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="AssetTypeId"></param>
        /// <returns></returns>
        public List<ClsPubAssetCategories> FunPubGetAssetCategories(int CompanyId, int AssetTypeId)
        {
            List<ClsPubAssetCategories> AssetCategories = new List<ClsPubAssetCategories>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETASSETCATEGORIES);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                if (AssetTypeId != 0)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMASSETTYPEID, DbType.Int32, AssetTypeId);
                }
                IDataReader reader = db.ExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubAssetCategories AssetCategorie = new ClsPubAssetCategories();
                    AssetCategorie.ClassId = IntParse(reader[ClsPubDALConstants.ASSETCLASSID]);
                    AssetCategorie.AssetClass = StringParse(reader[ClsPubDALConstants.ASSETCLASS]);
                    AssetCategorie.MakeId = IntParse(reader[ClsPubDALConstants.ASSETMAKEID]);
                    AssetCategorie.AssetMake = StringParse(reader[ClsPubDALConstants.ASSETMAKE]);
                    AssetCategorie.TypeId = IntParse(reader[ClsPubDALConstants.ASSETTYPE]);
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
    }
}
