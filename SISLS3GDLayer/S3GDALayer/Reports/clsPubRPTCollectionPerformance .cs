using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using S3GDALayer.Constants;
using S3GBusEntity.Reports;

namespace S3GDALayer.Reports
{
    public class clsPubRPTCollectionPerformance:ClsPubDalReportBase
    {

        public List<ClsPubDropDownList> FunPubGetLevelDetails(int CompanyId, int UserId,int LOBID)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETLEVELDETAILS);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, LOBID);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.Hierachy]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.Location_Description]);
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

        public List<ClsPubDropDownList> FunPubGetLevelValueDetails(int Hierarchy_Type, int Company_ID)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETLEVELVALUEDETAILS);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, Company_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMHIERARCHYTYPE, DbType.Int32,Hierarchy_Type);

                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.Location_Category_ID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.LocationCat_Description]);
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

        public List<ClsPubDropDownList> FunPubGetAssetTypeDetails(int User_ID, int Company_ID)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETASSETCATEGORIESTYPE);
           //     db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, Company_ID);
         //       db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, User_ID);

                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.CATEGORIESID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.CATEGORIESNAME]);
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


        public List<ClsPubDropDownList> FunPubGetAssetTypeValueDetails(int Asset_Type_ID, int Company_ID)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETASSETTYPEVALUEDETAILS);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, Company_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMASSETTYPEID, DbType.Int32, Asset_Type_ID);

                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.Asset_Category_ID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.LocationCat_Description]);
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


        public List<ClsPubDropDownList> FunPubGetProductsDetails(int Company_ID, int LOB_ID)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETPRODUCTDETAIL);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, Company_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, LOB_ID);

                //IDataReader reader = db.ExecuteReader(command);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.PRODUCTID]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.PRODUCTNAME]);
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

        public ClspubCollectionReturnAmount FunPubGetCollectionAmount(ClsPubPerformance CollectionPerformance)
        {

            ClspubCollectionReturnAmount objCollAmount = new ClspubCollectionReturnAmount();
            objCollAmount.GetChequeReturnAmount = new List<ClsPubChequeReturnAmount>();
            objCollAmount.GetCollectionAmount =new List<ClsPubCollectionPerformance>();
            objCollAmount.GetCollectionAnalysis = new List<ClsPubCollectionAnalysis>();

          //  List<ClsPubCollectionPerformance> CollectionAmountDetails = new List<ClsPubCollectionPerformance>();
            
            
            try
            {
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETCOLLECTIONAMOUNT);
                if (CollectionPerformance.LOB_ID > 0)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, CollectionPerformance.LOB_ID);
                }
                if (CollectionPerformance.Location_ID1 > 0)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, CollectionPerformance.Location_ID1);
                }
                if (CollectionPerformance.Location_ID2 > 0)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, CollectionPerformance.Location_ID2);
                }
                if (CollectionPerformance.Program_ID > 0)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, CollectionPerformance.Program_ID);
                }
                if (CollectionPerformance.User_ID > 0)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, CollectionPerformance.User_ID);
                }
                if (CollectionPerformance.Company_ID > 0)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CollectionPerformance.Company_ID);
                }
                if (CollectionPerformance.Asset_Type > 0)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMASSETTYPE, DbType.Int32, CollectionPerformance.Asset_Type);
                }
                if (CollectionPerformance.Product_ID > 0)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODID, DbType.Int32, CollectionPerformance.Product_ID);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMMODE, DbType.Int32, CollectionPerformance.Mode);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMNORMALFROMEDATE, DbType.String, CollectionPerformance.From_Date);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMNORMALTOEDATE, DbType.String, CollectionPerformance.To_Date);
                if (CollectionPerformance.From_ComDate != "")
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPFROMEDATE, DbType.String, CollectionPerformance.From_ComDate);
                }
                if (CollectionPerformance.To_ComDate != "")
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPTOEDATE, DbType.String, CollectionPerformance.To_ComDate);
                }
                IDataReader reader = db.FunPubExecuteReader(command);

                if (CollectionPerformance.Mode == 0)
                {
                    while (reader.Read())
                    {
                        ClsPubCollectionPerformance CollectionAmountDetail = new ClsPubCollectionPerformance();

                        CollectionAmountDetail.LOB_Name = StringParse(reader[ClsPubDALConstants.LOB_NAME]);
                        CollectionAmountDetail.Location_Name = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                        CollectionAmountDetail.Customer_Name = StringParse(reader[ClsPubDALConstants.Customer_NAME]);
                        CollectionAmountDetail.Product = StringParse(reader[ClsPubDALConstants.PRODUCT]);
                        CollectionAmountDetail.NOD = IntParse(reader[ClsPubDALConstants.NOD]);
                        CollectionAmountDetail.InstallmentDate = StringParse(reader[ClsPubDALConstants.INSTALLMENTDATE]);
                        CollectionAmountDetail.BALDUE = DecimalParse(reader[ClsPubDALConstants.BALDUE]);
                        CollectionAmountDetail.ReceiptDate = StringParse(reader[ClsPubDALConstants.PAID_DATE]);
                        CollectionAmountDetail.PANum = StringParse(reader[ClsPubDALConstants.PANum]);
                        CollectionAmountDetail.SANum = StringParse(reader[ClsPubDALConstants.SANum]);
                        CollectionAmountDetail.Ageing0to30 = DecimalParse(reader[ClsPubDALConstants.Ageing0to30]);
                        CollectionAmountDetail.Ageing31to60 = DecimalParse(reader[ClsPubDALConstants.Ageing31to60]);
                        CollectionAmountDetail.Ageing61to90 = DecimalParse(reader[ClsPubDALConstants.Ageing61to90]);
                        CollectionAmountDetail.AgeingAbove90 = DecimalParse(reader[ClsPubDALConstants.AgeingAbove90]);
                        CollectionAmountDetail.Period = DecimalParse(reader[ClsPubDALConstants.INSTALLMENTAMOUNT]);
                        objCollAmount.GetCollectionAmount.Add(CollectionAmountDetail);
                    }
                }
                else if (CollectionPerformance.Mode == 1)
                {
                    //reader.NextResult();
                    while (reader.Read())
                    {
                        ClsPubChequeReturnAmount ChequeReturAmount = new ClsPubChequeReturnAmount();
                        ChequeReturAmount.LOB_Name = StringParse(reader[ClsPubDALConstants.LOB_NAME]);
                        ChequeReturAmount.Location_Name = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                        ChequeReturAmount.Customer_Name = StringParse(reader[ClsPubDALConstants.Customer_NAME]);
                        ChequeReturAmount.Due_Amount = DecimalParse(reader[ClsPubDALConstants.INSTALLMENTAMOUNT]);
                        ChequeReturAmount.Due_Date = StringParse(reader[ClsPubDALConstants.INSTALLMENTDATE]);
                        ChequeReturAmount.NOD = IntParse(reader[ClsPubDALConstants.NOD]);
                        ChequeReturAmount.Product = StringParse(reader[ClsPubDALConstants.PRODUCT]);
                        ChequeReturAmount.PANum = StringParse(reader[ClsPubDALConstants.PANum]);
                        ChequeReturAmount.SANum = StringParse(reader[ClsPubDALConstants.SANum]);
                        ChequeReturAmount.Ageing0to30 = DecimalParse(reader[ClsPubDALConstants.Ageing0to30]);
                        ChequeReturAmount.Ageing31to60 = DecimalParse(reader[ClsPubDALConstants.Ageing31to60]);
                        ChequeReturAmount.Ageing61to90 = DecimalParse(reader[ClsPubDALConstants.Ageing61to90]);
                        ChequeReturAmount.AgeingAbove90 = DecimalParse(reader[ClsPubDALConstants.AgeingAbove90]);
                        ChequeReturAmount.Chq_Ageing0to30 = DecimalParse(reader[ClsPubDALConstants.Chq_Ageing0to30]);
                        ChequeReturAmount.Chq_Ageing31to60 = DecimalParse(reader[ClsPubDALConstants.Chq_Ageing31to60]);
                        ChequeReturAmount.Chq_Ageing61to90 = DecimalParse(reader[ClsPubDALConstants.Chq_Ageing61to90]);
                        ChequeReturAmount.Chq_AgeingAbove90 = DecimalParse(reader[ClsPubDALConstants.Chq_AgeingAbove90]);
                        objCollAmount.GetChequeReturnAmount.Add(ChequeReturAmount);
                    }

                }
                else if(CollectionPerformance.Mode ==2)
                {
         //    reader.Close();
                //reader.NextResult();
                    while (reader.Read())
                    {

                        ClsPubCollectionAnalysis CollectionAnalysisDetail = new ClsPubCollectionAnalysis();

                        CollectionAnalysisDetail.LOB = StringParse(reader[ClsPubDALConstants.LOB_NAME]);
                        CollectionAnalysisDetail.Location_Name = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                        CollectionAnalysisDetail.CustomerName = StringParse(reader[ClsPubDALConstants.Customer_NAME]);
                        CollectionAnalysisDetail.Product = StringParse(reader[ClsPubDALConstants.PRODUCT]);
                        CollectionAnalysisDetail.AccountNumber = StringParse(reader[ClsPubDALConstants.PANum].ToString());
                        CollectionAnalysisDetail.SubAccountNumber = StringParse(reader[ClsPubDALConstants.SANum]);
                        CollectionAnalysisDetail.FstPrdDue = DecimalParse(reader[ClsPubDALConstants.Period1DueAmount]);
                        CollectionAnalysisDetail.SndPrdDue = DecimalParse(reader[ClsPubDALConstants.Period2DueAmount]);
                        CollectionAnalysisDetail.FstprdClnAmt030 = DecimalParse(reader[ClsPubDALConstants.P1Amount030]);
                        CollectionAnalysisDetail.FstprdClnAmt3160 = DecimalParse(reader[ClsPubDALConstants.P1Amount3160]);
                        CollectionAnalysisDetail.FstprdClnAmt6190 = DecimalParse(reader[ClsPubDALConstants.P1Amount6190]);
                        CollectionAnalysisDetail.FstprdClnAmtAbv90 = DecimalParse(reader[ClsPubDALConstants.P1AmountAbove90]);
                        CollectionAnalysisDetail.SndprdClnAmt030 = DecimalParse(reader[ClsPubDALConstants.P2Amount030]);
                        CollectionAnalysisDetail.SndprdClnAmt3160 = DecimalParse(reader[ClsPubDALConstants.P2Amount3160]);
                        CollectionAnalysisDetail.SndprdClnAmt6190 = DecimalParse(reader[ClsPubDALConstants.P2Amount6190]);
                        CollectionAnalysisDetail.SndprdClnAmtAbv90 = DecimalParse(reader[ClsPubDALConstants.P2AmountAbove90]);
                        objCollAmount.GetCollectionAnalysis.Add(CollectionAnalysisDetail);
                        //  CollectionAnalysisAmountDetails.Add(CollectionAnalysisDetail);
                    }
                }
               
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, ObjCustomers.LOBId);
                //if (ObjCustomers.BranchId != -1)
                //{
                //    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMBRANCHID, DbType.Int32, ObjCustomers.BranchId);
                //}
                //if (ObjCustomers.ProductId != -1)
                //{
                //    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODUCTID, DbType.Int32, ObjCustomers.ProductId);
                //}

                ////db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMBRANCHID, DbType.Int32, ObjCustomers.BranchId);
                ////db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODUCTID, DbType.Int32, ObjCustomers.ProductId);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, ObjCustomers.Start_Date);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, ObjCustomers.End_Date);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTATUS, DbType.Int32, ObjCustomers.Status);


                //IDataReader reader = db.ExecuteReader(command);

                //while (reader.Read())
                //{
                //    ClsPubCustomersDetails CustomersDetail = new ClsPubCustomersDetails();

                //    CustomersDetail.NameofCustomer = StringParse(reader[ClsPubDALConstants.NAMEOFCUSTOMER]);
                //    CustomersDetail.Address = StringParse(reader[ClsPubDALConstants.ADDRESS]);
                //    CustomersDetail.LoanBorrowed = DecimalParse(reader[ClsPubDALConstants.LOANBORROWED]);
                //    CustomersDetail.EnquiryDate = DateTimeParse(reader[ClsPubDALConstants.ENQUIRYDATE]);
                //    CustomersDetail.ApplicationDate = DateTimeParse(reader[ClsPubDALConstants.APPLICATIONDATE]);
                //    CustomersDetail.Constitution = StringParse(reader[ClsPubDALConstants.CONSTITUTION]);
                //    CustomersDetail.PANum = StringParse(reader[ClsPubDALConstants.PANUM]);
                //    CustomersDetail.SANum = StringParse(reader[ClsPubDALConstants.SANUM]);

                //    CustomersDetails.Add(CustomersDetail);
                //}
                //reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return (objCollAmount);
        }

        public List<ClsPubCollectionAnalysis> FunPubgetCollectionAnalysisDtls(ClsPubPerformance CollectionPerformance)
        {
           
         

            List<ClsPubCollectionAnalysis> CollectionAnalysisAmountDetails = new List<ClsPubCollectionAnalysis>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETCOMPARITIVEANALYSIS);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, CollectionPerformance.LOB_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, CollectionPerformance.Location_ID1);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, CollectionPerformance.Location_ID2);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, CollectionPerformance.Program_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, CollectionPerformance.User_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CollectionPerformance.Company_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMASSETTYPE, DbType.Int32, CollectionPerformance.Asset_Type);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODID, DbType.Int32, CollectionPerformance.Product_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMNORMALFROMEDATE, DbType.String, CollectionPerformance.From_Date);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMNORMALTOEDATE, DbType.String, CollectionPerformance.To_Date);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPFROMEDATE, DbType.String, CollectionPerformance.From_ComDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPTOEDATE, DbType.String, CollectionPerformance.To_ComDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMMODE, DbType.String, CollectionPerformance.Mode);
                IDataReader reader = db.FunPubExecuteReader(command);

                while (reader.Read())
                {
                    ClsPubCollectionAnalysis CollectionAnalysisDetail = new ClsPubCollectionAnalysis();

                    CollectionAnalysisDetail.LOB = StringParse(reader[ClsPubDALConstants.LOB_NAME]);
                    CollectionAnalysisDetail.CustomerName = StringParse(reader[ClsPubDALConstants.Customer_NAME]);
                    CollectionAnalysisDetail.AccountNumber = StringParse(reader[ClsPubDALConstants.PANum].ToString());
                    CollectionAnalysisDetail.SubAccountNumber = StringParse(reader[ClsPubDALConstants.SANum]);
                    CollectionAnalysisDetail.Product = StringParse(reader[ClsPubDALConstants.PRODUCT]);
                    CollectionAnalysisDetail.FstPrdDue= DecimalParse(reader[ClsPubDALConstants.Period1DueAmount]);
                    CollectionAnalysisDetail.SndPrdDue= DecimalParse(reader[ClsPubDALConstants.Period2DueAmount]);
                    CollectionAnalysisDetail.FstprdClnAmt030 = DecimalParse(reader[ClsPubDALConstants.P1Amount030]);
                    CollectionAnalysisDetail.FstprdClnAmt3160 = DecimalParse(reader[ClsPubDALConstants.P1Amount3160]);
                    CollectionAnalysisDetail.FstprdClnAmt6190 = DecimalParse(reader[ClsPubDALConstants.P1Amount6190]);
                    CollectionAnalysisDetail.FstprdClnAmtAbv90 = DecimalParse(reader[ClsPubDALConstants.P1AmountAbove90]);
                    CollectionAnalysisDetail.SndprdClnAmt030 = DecimalParse(reader[ClsPubDALConstants.P2Amount030]);
                    CollectionAnalysisDetail.SndprdClnAmt3160 = DecimalParse(reader[ClsPubDALConstants.P2Amount3160]);
                    CollectionAnalysisDetail.SndprdClnAmt6190 = DecimalParse(reader[ClsPubDALConstants.P2Amount6190]);
                    CollectionAnalysisDetail.SndprdClnAmtAbv90 = DecimalParse(reader[ClsPubDALConstants.P2AmountAbove90]);
                    CollectionAnalysisAmountDetails.Add(CollectionAnalysisDetail);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return (CollectionAnalysisAmountDetails);
        }
    }
}
