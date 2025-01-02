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
    public class ClsPubRptDisbursement : ClsPubDalReportBase
    {
        #region Load LOB
        
        public List<ClsPubDropDownList> FunPubLOB(int CompanyId, int UserId,int ProgramId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetLOB);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, Is_Active);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);


                //IDataReader reader = db.FunPubExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
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

        public List<ClsPubDropDownList> FunPubGetRegion(int CompanyId, bool Is_Active,int UserId)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetRegion);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Int32, Is_Active);
                if(UserId != null && UserId >0) 
                     db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                //IDataReader reader = db.FunPubExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
                IDataReader reader = db.FunPubExecuteReader(command);
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

                DbCommand command = db.GetStoredProcCommand(SPNames.GetBranchBasedRegion);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                if (Region_Id != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMREGIONID, DbType.Int32, Region_Id);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Int32, Is_Active);
                //IDataReader reader = db.FunPubExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
                IDataReader reader = db.FunPubExecuteReader(command);
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
        public List<ClsPubDropDownList> FunPubGetLocation2(int ProgramId, int UserId, int CompanyId, int LobId,int LocationId)
        {
            List<ClsPubDropDownList> dropdownLists = new List<ClsPubDropDownList>();
            try
            {

                DbCommand command = db.GetStoredProcCommand(SPNames.getlocation2);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, LobId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID, DbType.Int32, LocationId);
                //IDataReader reader = db.FunPubExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 30/mar/2012
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
        public ClsPubDisbursement FunPubGetDisburseDetails(ClsPubDisburseSelectionCriteria disburseSelectionCriteria)
        {
            ClsPubDisbursement disburse = new ClsPubDisbursement();
            disburse.Disbursement = new List<ClsPubDisbursementDetails>();
            disburse.loblocation = new List<ClsPubLobLocation>();
             // List<ClsPubDisbursementDetails> details = new List<ClsPubDisbursementDetails>();
              
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetDisbursementDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, disburseSelectionCriteria.CompanyId);

                if (disburseSelectionCriteria.LobId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, disburseSelectionCriteria.LobId);
                }
                if (disburseSelectionCriteria.LocationID1 != string.Empty)
                {

                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, disburseSelectionCriteria.LocationID1);
                }
                if (disburseSelectionCriteria.LocationID2 != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, disburseSelectionCriteria.LocationID2);
                }
                if (disburseSelectionCriteria.ProductId != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODUCTID, DbType.Int32, disburseSelectionCriteria.ProductId);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, disburseSelectionCriteria.StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, disburseSelectionCriteria.EndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISSUMMARY, DbType.Boolean, disburseSelectionCriteria.IsDetail);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, disburseSelectionCriteria.UserId);
                IDataReader reader = db.FunPubExecuteReader(command);
                if (disburseSelectionCriteria.IsDetail)
                {

                    while (reader.Read())
                    {
                        ClsPubLobLocation location = new ClsPubLobLocation();
                        location.LobName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                        location.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                        disburse.loblocation.Add(location);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        ClsPubDisbursementDetails detail = new ClsPubDisbursementDetails();
                        detail.ApplicationProcessId = IntParse(reader[ClsPubDALConstants.APPLICATIONPROCESSID]);
                        detail.LobName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                        //detail.Region = StringParse(reader[ClsPubDALConstants.REGIONNAME]);
                        detail.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                        detail.Product = StringParse(reader[ClsPubDALConstants.PRODUCT]);
                        //detail.Location = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                        detail.ApplicationNumber = StringParse(reader[ClsPubDALConstants.APPLICATIONNO]);
                        detail.CustomerName = StringParse(reader[ClsPubDALConstants.CUSTOMERNAME]);
                        detail.PrimeAccount = StringParse(reader[ClsPubDALConstants.PANUM]);
                        detail.SubAccount = StringParse(reader[ClsPubDALConstants.SANUM]);
                        detail.ApprovedAmount = DecimalParse(reader[ClsPubDALConstants.APPROVEDAMOUNT]);
                        detail.RemainingAmount = DecimalParse(reader[ClsPubDALConstants.REMAININGAMOUNT]);
                        detail.AccountYetToBeCreated = DecimalParse(reader[ClsPubDALConstants.ACCOUNTYETAMOUNT]);
                        detail.PaidAmount = DecimalParse(reader[ClsPubDALConstants.PAIDAMOUNT]);
                        detail.ageing0days = DecimalParse(reader[ClsPubDALConstants.AGEING0DAYS]);
                        detail.ageing30days = DecimalParse(reader[ClsPubDALConstants.AGEING30DAYS]);
                        detail.ageing60days = DecimalParse(reader[ClsPubDALConstants.AGEING60DAYS]);
                        detail.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                        disburse.Disbursement.Add(detail);
                        //detail.Ageing60Days = string.Empty; ;
                        //detail.Ageingabove60Days = string.Empty;
                       
                        //Detail.Ageing30Days = StringParse(reader[ClsPubDALConstants.AGEING30DAYS]);
                        //Detail.Ageing60Days  = StringParse(reader[ClsPubDALConstants.AGEING60DAYS]);
                        //Detail.Ageingabove60Days  = StringParse(reader[ClsPubDALConstants.AGEINGABOVE60DAYS]);

                    }

                     reader.Close();

                }
                else
                {
                    while (reader.Read())
                    {
                        ClsPubLobLocation location = new ClsPubLobLocation();
                        location.LobName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                        location.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                        disburse.loblocation.Add(location);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        ClsPubDisbursementDetails detail = new ClsPubDisbursementDetails();
                        detail.LobName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
                        //detail.Region = StringParse(reader[ClsPubDALConstants.REGIONNAME]);
                        detail.Branch = StringParse(reader[ClsPubDALConstants.LOCATIONNAME]);
                        detail.Product = StringParse(reader[ClsPubDALConstants.PRODUCT]);
                        detail.ApprovedAmount = DecimalParse(reader[ClsPubDALConstants.APPROVEDAMOUNT]);
                        detail.PaidAmount = DecimalParse(reader[ClsPubDALConstants.PAIDAMOUNT]);
                        detail.RemainingAmount = DecimalParse(reader[ClsPubDALConstants.REMAININGAMOUNT]);
                        detail.AccountYetToBeCreated = DecimalParse(reader[ClsPubDALConstants.ACCOUNTYETAMOUNT]);
                        detail.ageing0days = DecimalParse(reader[ClsPubDALConstants.AGEING0DAYS]);
                        detail.ageing30days = DecimalParse(reader[ClsPubDALConstants.AGEING30DAYS]);
                        detail.ageing60days = DecimalParse(reader[ClsPubDALConstants.AGEING60DAYS]);
                        detail.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                        //detail.Ageing60Days = string.Empty;
                        //detail.Ageingabove60Days = string.Empty;
                        disburse.Disbursement.Add(detail);
                        //Detail.Ageing30Days = StringParse(reader[ClsPubDALConstants.AGEING30DAYS]);
                        //Detail.Ageing60Days  = StringParse(reader[ClsPubDALConstants.AGEING60DAYS]);
                        //Detail.Ageingabove60Days  = StringParse(reader[ClsPubDALConstants.AGEINGABOVE60DAYS]);

                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return disburse;
        }
        //public List<ClsPubDisbursement> FunPubGetDisburseAbstract(int CompanyId, string LOB_ID, string Region_Id, string Branch_ID, string Product_Id, string EndDate)
        //{
        //    List<ClsPubDisbursement> Abstracts = new List<ClsPubDisbursement>();
        //    try
        //    {
        //        DbCommand command = db.GetStoredProcCommand(ClsPubDALConstants.SPGETDISBURSEMENT);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
        //        if (LOB_ID != string.Empty)
        //        {
        //            db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, LOB_ID);
        //        }
        //        if (Branch_ID != string.Empty)
        //        {

        //            db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMBRANCHID, DbType.Int32, Branch_ID);
        //        }
        //        if (Region_Id != string.Empty)
        //        {
        //            db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMREGIONID, DbType.Int32, Region_Id);
        //        }
        //        if (Product_Id != string.Empty)
        //        {
        //            db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODUCTID, DbType.Int32, Product_Id);
        //        }
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, EndDate);
        //        IDataReader reader = db.FunPubExecuteReader(command);
        //        while (reader.Read())
        //        {

        //            ClsPubDisbursement Abstract = new ClsPubDisbursement();
        //            Abstract.LobName = StringParse(reader[ClsPubDALConstants.LOBNAME]);
        //            Abstract.Region = StringParse(reader[ClsPubDALConstants.REGIONNAME]);
        //            Abstract.Branch = StringParse(reader[ClsPubDALConstants.BRANCHNAME]);
        //            Abstract.Product = StringParse(reader[ClsPubDALConstants.PRODUCT]);
        //            Abstract.ApprovedAmount = StringParse(reader[ClsPubDALConstants.APPROVEDAMOUNT]);
        //            Abstract.PaidAmount = StringParse(reader[ClsPubDALConstants.PAIDAMOUNT]);
        //            Abstract.RemainingAmount = StringParse(reader[ClsPubDALConstants.REMAININGAMOUNT]);
        //            //Detail.Ageing30Days = StringParse(reader[ClsPubDALConstants.AGEING30DAYS]);
        //            //Detail.Ageing60Days  = StringParse(reader[ClsPubDALConstants.AGEING60DAYS]);
        //            //Detail.Ageingabove60Days  = StringParse(reader[ClsPubDALConstants.AGEINGABOVE60DAYS]);
        //            Abstract.AccountYetToBeCreated = string.Empty; ;
        //            Abstract.Ageing60Days = string.Empty; ;
        //            Abstract.Ageingabove60Days = string.Empty;
        //            Abstracts.Add(Abstract);
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
        //    }
        //    return Abstracts; ;
        //}
    }
}
