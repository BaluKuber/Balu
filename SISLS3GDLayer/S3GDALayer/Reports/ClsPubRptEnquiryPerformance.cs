using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using System.Data.Common;
using S3GBusEntity.Reports;
using S3GDALayer.Constants;
using S3GBusEntity;


namespace S3GDALayer.Reports
{
    public class ClsPubRptEnquiryPerformance : ClsPubDalReportBase
    {

        //S3G_RPT_GetLocCode

        #region Load LocationCode
        /// <summary>
        /// To get the LocationCode
        /// </summary>
        // public List<ClsPubEnquiryPerformanceDetails> FunPubGetEnquiryCode(ClsPubEnquiryParameters Enqcode)
        //{
        //    List<ClsPubEnquiryPerformanceDetails> EDetails = new List<ClsPubEnquiryPerformanceDetails>();
        //    try
        //    {
        //      DbCommand command = db.GetStoredProcCommand(SPNames.S3G_RPT_GETLOCCODE);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, Enqcode.CompanyId);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, Enqcode.USER_ID);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32,Enqcode.PROGRAM_ID);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, Enqcode.LobId);
        //          IDataReader reader = db.ExecuteReader(command);
        //        while (reader.Read())
        //        {
        //            ClsPubEnquiryPerformanceDetails EDetail = new ClsPubEnquiryPerformanceDetails();
        //            EDetail.Location = StringParse(reader[ClsPubDALConstants.LOCATION]);
        //            EDetails.Add(EDetail);
        //        }
        //        reader.Close();
        //    }

        //    catch (Exception ex)
        //    {
        //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
        //    }
        //    return EDetails;
        //}
        #endregion
        //S3G_RPT_GetEnquiryLob

        #region Load LOB
        /// <summary>
        /// To get the LOB
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubGetLOB(int CompanyId, int UserId, bool Is_Active, int ProgramId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.S3G_RPT_GetEnquiryLob);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, Is_Active);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.String, CustomerId);


                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 9/April/2012
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
        //public List<ClsPubEnquiryPerformanceDetails> FunPubGetEnquiryDetails(ClsPubEnquiryParameters Enq)
        //{
        //    List<ClsPubEnquiryPerformanceDetails> EnquiryDetails = new List<ClsPubEnquiryPerformanceDetails>();

        public ClsPubEnquiryLocation FunPubGetEnquiryDetails(ClsPubEnquiryParameters Enq)
        {
            ClsPubEnquiryLocation disburse = new ClsPubEnquiryLocation();
            disburse.Disbursement = new List<ClsPubEnquiryPerformanceDetails>();
            disburse.loblocation = new List<ClsPubLobLocation>();

            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetEnquiryPerformanceDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, Enq.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, Enq.USER_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, Enq.PROGRAM_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, Enq.LobId);
                if (Enq.LOCATION_ID1.Trim() != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, Enq.LOCATION_ID1);
                }
                if (Enq.LOCATION_ID2.Trim() != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, Enq.LOCATION_ID2);
                }
                if (Enq.ProductId.Trim() != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODID, DbType.Int32, Enq.ProductId);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, Enq.StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, Enq.EndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, Enq.Denomination);


                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 9/April/2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubLobLocation location = new ClsPubLobLocation();
                    //location.LobName = IntParse(reader[ClsPubDALConstants.LOCATION_ID]);
                    location.Branch = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    disburse.loblocation.Add(location);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ClsPubEnquiryPerformanceDetails EnquiryDetail = new ClsPubEnquiryPerformanceDetails();

                    EnquiryDetail.Location = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    EnquiryDetail.LocationId = IntParse(reader[ClsPubDALConstants.LOCATION_ID]);
                    //EnquiryDetail.Branch = StringParse(reader[ClsPubDALConstants.BRANCH]);
                    //EnquiryDetail.BranchId = StringParse(reader[ClsPubDALConstants.BRANCHID]);
                    //EnquiryDetail.Branch = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    //EnquiryDetail.BranchId = StringParse(reader[ClsPubDALConstants.LOCATION_ID]);
                    EnquiryDetail.Product = StringParse(reader[ClsPubDALConstants.PRODUCTS]);
                    EnquiryDetail.ProductId = StringParse(reader[ClsPubDALConstants.PRODUCTID]);
                    EnquiryDetail.ReceivedCount = IntParse(reader[ClsPubDALConstants.RECEIVEDCOUNT]);
                    EnquiryDetail.ReceivedValue = DecimalParse(reader[ClsPubDALConstants.RECEIVEDVALUE]);
                    EnquiryDetail.SuccessfulCount = IntParse(reader[ClsPubDALConstants.SUCCESSFULCOUNT]);
                    EnquiryDetail.SuccessfulValue = DecimalParse(reader[ClsPubDALConstants.SUCCESSFULVALUE]);
                    EnquiryDetail.UnderFollowupCount = IntParse(reader[ClsPubDALConstants.UNDERFOLLOWUPCOUNT]);
                    EnquiryDetail.UnderFollowupValue = DecimalParse(reader[ClsPubDALConstants.UNDERFOLLOWUPVALUE]);
                    EnquiryDetail.RejectedCount = IntParse(reader[ClsPubDALConstants.REJECTEDCOUNT]);
                    EnquiryDetail.RejectedValue = DecimalParse(reader[ClsPubDALConstants.REJECTEDVALUE]);
                    //EnquiryDetail.Denomination = StringParse(reader[ClsPubDALConstants.Denomination]);
                    EnquiryDetail.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    //EnquiryDetails.Add(EnquiryDetail);
                    disburse.Disbursement.Add(EnquiryDetail);
                   
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return disburse;
        }
        public List<ClsPubEnquiryCount> FunPubGetEnquiryRecCount(ClsPubEnquiryParameters Enqparam)
        {
            List<ClsPubEnquiryCount> Enqcounts = new List<ClsPubEnquiryCount>();
            try
            {

                DbCommand command = db.GetStoredProcCommand(SPNames.GetEnquiryReceivedDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, Enqparam.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, Enqparam.USER_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, Enqparam.PROGRAM_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, Enqparam.LobId);
                if (Enqparam.LOCATION_ID1.Trim() != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, Enqparam.LOCATION_ID1);
                }
                if (Enqparam.LOCATION_ID2.Trim() != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, Enqparam.LOCATION_ID2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODID, DbType.Int32, Enqparam.ProductId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, Enqparam.StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, Enqparam.EndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMOPTION, DbType.Int32, Enqparam.Option);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, Enqparam.Denomination);
                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 9/April/2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubEnquiryCount Enqcount = new ClsPubEnquiryCount();
                    Enqcount.Product = StringParse(reader[ClsPubDALConstants.PRODUCTS]);
                    Enqcount.ProductId = StringParse(reader[ClsPubDALConstants.PRODUCTID]);
                    Enqcount.Location = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    Enqcount.LocationId = IntParse(reader[ClsPubDALConstants.LOCATION_ID]);
                    Enqcount.EnqNo = StringParse(reader[ClsPubDALConstants.ENQUIRYNUMBER]);
                    Enqcount.EnquiryDate = reader[ClsPubDALConstants.ENQUIRYDAT].ToString();
                    Enqcount.CustomerName = StringParse(reader[ClsPubDALConstants.CUSTOMERNAM]);
                    Enqcount.AssetDetails = StringParse(reader[ClsPubDALConstants.ASSETDETAILS]);
                    Enqcount.FacilityAmount = StringParse(reader[ClsPubDALConstants.FACILITYAMOUNT]);
                    Enqcount.Status = StringParse(reader[ClsPubDALConstants.STATUS]);
                    Enqcounts.Add(Enqcount);
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return Enqcounts;
        }
        public List<ClsPubEnquiryCount> FunPubGetEnquirySucCount(ClsPubEnquiryParameters EnqSuc)
        {
            List<ClsPubEnquiryCount> EnqSuccess = new List<ClsPubEnquiryCount>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetEnquirySuccessfulDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, EnqSuc.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, EnqSuc.USER_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, EnqSuc.PROGRAM_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, EnqSuc.LobId);
                if (EnqSuc.LOCATION_ID1.Trim() != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, EnqSuc.LOCATION_ID1);
                }
                if (EnqSuc.LOCATION_ID2.Trim() != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, EnqSuc.LOCATION_ID2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODID, DbType.Int32, EnqSuc.ProductId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, EnqSuc.StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, EnqSuc.EndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMOPTION, DbType.Int32, EnqSuc.Option);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, EnqSuc.Denomination);
                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 9/April/2012
                IDataReader reader = db.FunPubExecuteReader(command); ;
                while (reader.Read())
                {
                    ClsPubEnquiryCount EnqSucs = new ClsPubEnquiryCount();
                    EnqSucs.Product = StringParse(reader[ClsPubDALConstants.PRODUCTS]);
                    EnqSucs.ProductId = StringParse(reader[ClsPubDALConstants.PRODUCTID]);
                    EnqSucs.Location = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    EnqSucs.LocationId = IntParse(reader[ClsPubDALConstants.LOCATION_ID]);
                    EnqSucs.EnqNo = StringParse(reader[ClsPubDALConstants.ENQUIRYNUMBER]);
                    EnqSucs.EnquiryDate = reader[ClsPubDALConstants.ENQUIRYDAT].ToString();
                    EnqSucs.CustomerName = StringParse(reader[ClsPubDALConstants.CUSTOMERNAM]);
                    EnqSucs.AssetDetails = StringParse(reader[ClsPubDALConstants.ASSETDETAILS]);
                    EnqSucs.FacilityAmount = StringParse(reader[ClsPubDALConstants.FACILITYAMOUNT]);
                    EnqSucs.PrimeAccNo = StringParse(reader[ClsPubDALConstants.PRIMEACCNO]);
                    EnqSucs.SubAccNo = StringParse(reader[ClsPubDALConstants.SUBACCNO]);
                    EnqSuccess.Add(EnqSucs);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return EnqSuccess;
        }
        public List<ClsPubEnquiryCount> FunPubGetEnquiryUnderFollowupCount(ClsPubEnquiryParameters EnqFollow)
        {
            List<ClsPubEnquiryCount> EnqFollowups = new List<ClsPubEnquiryCount>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetEnquiryFollowupDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, EnqFollow.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, EnqFollow.USER_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, EnqFollow.PROGRAM_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, EnqFollow.LobId);
                if (EnqFollow.LOCATION_ID1.Trim() != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, EnqFollow.LOCATION_ID1);
                }
                if (EnqFollow.LOCATION_ID2.Trim() != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, EnqFollow.LOCATION_ID2);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODID, DbType.Int32, EnqFollow.ProductId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, EnqFollow.StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, EnqFollow.EndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMOPTION, DbType.Int32, EnqFollow.Option);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, EnqFollow.Denomination);
                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 9/April/2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubEnquiryCount EnqFollowup = new ClsPubEnquiryCount();
                    EnqFollowup.Product = StringParse(reader[ClsPubDALConstants.PRODUCTS]);
                    EnqFollowup.ProductId = StringParse(reader[ClsPubDALConstants.PRODUCTID]);
                    EnqFollowup.Location = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    EnqFollowup.LocationId = IntParse(reader[ClsPubDALConstants.LOCATION_ID]);
                    EnqFollowup.EnqNo = StringParse(reader[ClsPubDALConstants.ENQUIRYNUMBER]);
                    EnqFollowup.EnquiryDate = reader[ClsPubDALConstants.ENQUIRYDAT].ToString();
                    EnqFollowup.CustomerName = StringParse(reader[ClsPubDALConstants.CUSTOMERNAM]);
                    EnqFollowup.AssetDetails = StringParse(reader[ClsPubDALConstants.ASSETDETAILS]);
                    EnqFollowup.FacilityAmount = StringParse(reader[ClsPubDALConstants.FACILITYAMOUNT]);
                    EnqFollowup.Stage = StringParse(reader[ClsPubDALConstants.STAGE]);
                    EnqFollowups.Add(EnqFollowup);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return EnqFollowups;
        }
        public List<ClsPubEnquiryCount> FunPubGetEnquiryRejectedCount(ClsPubEnquiryParameters EnqRej)
        {
            List<ClsPubEnquiryCount> EnqRejects = new List<ClsPubEnquiryCount>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetEnquiryRejectedDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, EnqRej.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, EnqRej.LobId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, EnqRej.USER_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, EnqRej.PROGRAM_ID);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODID, DbType.Int32, EnqRej.ProductId);
                if (EnqRej.LOCATION_ID1.Trim() != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, EnqRej.LOCATION_ID1);
                }
                if (EnqRej.LOCATION_ID2.Trim() != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, EnqRej.LOCATION_ID2);
                }
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMBRANCHID, DbType.Int32, EnqRej.BranchId);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPRODID, DbType.Int32, EnqRej.ProductId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSTARTDATE, DbType.DateTime, EnqRej.StartDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMENDDATE, DbType.DateTime, EnqRej.EndDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMOPTION, DbType.Int32, EnqRej.Option);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, EnqRej.Denomination);
                //IDataReader reader = db.ExecuteReader(command);//modified by ponnurajesh for Oracle Conversion on 9/April/2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubEnquiryCount EnqReject = new ClsPubEnquiryCount();
                    EnqReject.Product = StringParse(reader[ClsPubDALConstants.PRODUCTS]);
                    EnqReject.ProductId = StringParse(reader[ClsPubDALConstants.PRODUCTID]);
                    EnqReject.Location = StringParse(reader[ClsPubDALConstants.LOCATION]);
                    EnqReject.LocationId = IntParse(reader[ClsPubDALConstants.LOCATION_ID]);
                    EnqReject.EnqNo = StringParse(reader[ClsPubDALConstants.ENQUIRYNUMBER]);
                    EnqReject.EnquiryDate = reader[ClsPubDALConstants.ENQUIRYDAT].ToString();
                    EnqReject.CustomerName = StringParse(reader[ClsPubDALConstants.CUSTOMERNAM]);
                    EnqReject.AssetDetails = StringParse(reader[ClsPubDALConstants.ASSETDETAILS]);
                    EnqReject.FacilityAmount = StringParse(reader[ClsPubDALConstants.FACILITYAMOUNT]);
                    EnqReject.Remarks = StringParse(reader[ClsPubDALConstants.REMARKS]);
                    EnqRejects.Add(EnqReject);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return EnqRejects;
        }

    }
}