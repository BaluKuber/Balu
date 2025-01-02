using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
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
    public class ClsPubRptLANRegister : ClsPubDalReportBase
    {
        #region LOB

        public List<ClsPubDropDownList> FunPubGetLOB(int CompanyId, int UserId, int ProgramId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LANOperatingLease);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, Is_Active);

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
            return (dropDownLists);//FunPriLoadSelect
        }
        #endregion

        #region Get MLA
        /// <summary>
        /// To get the Prime Account Number
        /// </summary>
        /// <param name="ObjPrimeAccount"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubGetMLA(ClsPubPrimeAccountDetails ObjPrimeAccount)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetLanpanSanDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTYPE, DbType.String, ObjPrimeAccount.Type);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, ObjPrimeAccount.CompanyId);
                if (ObjPrimeAccount.LobId != 0)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, ObjPrimeAccount.LobId);
                }
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATION_ID, DbType.Int32, ObjPrimeAccount.locationId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVATED, DbType.Int32, ObjPrimeAccount.IsActivated);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, ObjPrimeAccount.IsActive);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.String, ObjPrimeAccount.CustomerId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCHECKACCESS, DbType.String, ObjPrimeAccount.CheckAccess);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ObjPrimeAccount.ProgramId);
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

        #region Get SLA
        /// <summary>
        /// To get the Sub Account Number
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="CompanyId"></param>
        /// <param name="IsActive"></param>
        /// <param name="CustomerId"></param>
        /// <param name="PNum"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubGetSLA(string Type, int CompanyId, string LobId, string CustomerId, string PNum, int IsActive, int ProgramId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetLanpanSanDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTYPE, DbType.String, Type);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, LobId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVATED, DbType.Int32, IsActive);
                // db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, IsActive);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, PNum);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);

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

        //#region LAN NUMBER

        //public List<ClsPubDropDownList> FunPubGetLANNumber(ClsPubLANRegisterinput LANInput)
        //{
        //    List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
        //    try
        //    {
        //        DbCommand command = db.GetStoredProcCommand(SPNames.S3G_RPT_GetLANNumber);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, LANInput.CompanyId);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, LANInput.USER_ID);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, LANInput.LOB_ID);
        //        //if (LANInput.Customer_ID.Trim() != string.Empty)
        //        //{
        //        //    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.Int32, LANInput.Customer_ID);
        //        //}
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.Int32, LANInput.Customer_ID);
        //        //if (LANInput.PrimeAccountNo.Trim() != string.Empty)
        //        //{}
        //        //if (LANInput.SubAccountNo.Trim() != string.Empty)
        //        //{}
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, LANInput.PrimeAccountNo);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSANUM, DbType.String, LANInput.SubAccountNo);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, LANInput.LOCATION_ID1);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, LANInput.LOCATION_ID2);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMOPTION, DbType.Int32, LANInput.Option);
        //        //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);


        //        //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, Is_Active);

        //        IDataReader reader = db.ExecuteReader(command);

        //        while (reader.Read())
        //        {
        //            ClsPubDropDownList dropDownList = new ClsPubDropDownList();
        //            //dropDownList.ID = StringParse(reader[ClsPubDALConstants.LEASEASSETNUMBER]);
        //            dropDownList.Description = StringParse(reader[ClsPubDALConstants.LEASEASSETNUMBER]);
        //            dropDownLists.Add(dropDownList);
        //        }
        //        reader.Close();
        //    }

        //    catch (Exception ex)
        //    {

        //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

        //    }
        //    return (dropDownLists);//FunPriLoadSelect
        //}


        //#endregion

        public ClsPubLeaseAssetRegisterDetails FunPubGetLanDetails(ClsPubLANRegisterinput ObjLanParameters)
        {

            ClsPubLeaseAssetRegisterDetails LeaseDetails = new ClsPubLeaseAssetRegisterDetails();
            LeaseDetails.LANDetails= new List<ClsPubLANRegisterDetails>();
            LeaseDetails.LocationLan=new List<ClsPubLobLocation>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.LeaseAssetRegisterDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, ObjLanParameters.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, ObjLanParameters.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ObjLanParameters.ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, ObjLanParameters.LobId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, ObjLanParameters.LocationId1);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, ObjLanParameters.LocationId2);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLANNOFROM, DbType.String, ObjLanParameters.LanFrom);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLANNOTO, DbType.String, ObjLanParameters.LanTo);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, ObjLanParameters.Panum);
                //db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSANUM, DbType.String, ObjLanParameters.Sanum);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMNORMALFROMEDATE, DbType.DateTime, ObjLanParameters.FromDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMNORMALTOEDATE, DbType.DateTime, ObjLanParameters.ToDate);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMDENOMINATION, DbType.Decimal, ObjLanParameters.Denomination);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMOPTION, DbType.Int32, ObjLanParameters.Option);
                
                //Modified By Chandrasekar K On 30-July-2012
                //IDataReader reader = db.ExecuteReader(command);
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                    {
                        ClsPubLobLocation location = new ClsPubLobLocation();
                        location.Lan = StringParse(reader[ClsPubDALConstants.Lan]);
                        location.Location = StringParse(reader[ClsPubDALConstants.LOCATION]);
                        LeaseDetails.LocationLan.Add(location);
                    }
                    reader.NextResult();
                while (reader.Read())
                {

                    ClsPubLANRegisterDetails LeaseDetail = new ClsPubLANRegisterDetails();

                    LeaseDetail.DocumentDate = reader[ClsPubDALConstants.DOCUMENTDATE].ToString();
                    LeaseDetail.DocumentNo = StringParse(reader[ClsPubDALConstants.DOCUMENTNUMBER]);
                    LeaseDetail.Lan = StringParse(reader[ClsPubDALConstants.LAN]);
                    LeaseDetail.Panum = StringParse(reader[ClsPubDALConstants.PANUM]);
                    LeaseDetail.Sanum = StringParse(reader[ClsPubDALConstants.SANUM]);
                    LeaseDetail.ValueDate= reader[ClsPubDALConstants.VALUEDATE].ToString();
                    LeaseDetail.Description = StringParse(reader[ClsPubDALConstants.DESCRIPTION]);
                    
                    LeaseDetail.Debit = DecimalParse(reader[ClsPubDALConstants.DEBIT]);
                    LeaseDetail.Credit = DecimalParse(reader[ClsPubDALConstants.CREDIT]);
                    LeaseDetail.Balance = DecimalParse(reader[ClsPubDALConstants.BALANCE]);
                    LeaseDetail.Denomination = StringParse(reader[ClsPubDALConstants.Denomination]);
                    LeaseDetail.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    LeaseDetail.Location = StringParse(reader[ClsPubDALConstants.LOCATION]);

                    LeaseDetails.LANDetails.Add(LeaseDetail);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return LeaseDetails;
        }


        public List<ClsPubLANdetails> FunPubGetLanAssetDetails(int Company_Id, string Lan)
        {

            List<ClsPubLANdetails> LanAssetDetails = new List<ClsPubLANdetails>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetLANDescriptiondetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, Company_Id);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLAN, DbType.String, Lan);
             
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubLANdetails LanAssetDetail = new ClsPubLANdetails();
                    
                    LanAssetDetail.Lan = StringParse(reader[ClsPubDALConstants.Lan]);
                    LanAssetDetail.AssetDescription = StringParse(reader[ClsPubDALConstants.AssetDescription]);
                    LanAssetDetail.RegnNumber = StringParse(reader[ClsPubDALConstants.RegnNumber]);
                    LanAssetDetail.ChasisNumber = StringParse(reader[ClsPubDALConstants.ChasisNumber]);
                    LanAssetDetail.EngineNumber = StringParse(reader[ClsPubDALConstants.EngineNumber]);
                    LanAssetDetail.SerialNumber = StringParse(reader[ClsPubDALConstants.SerialNumber]);
                    LanAssetDetail.PerformingStatus = StringParse(reader[ClsPubDALConstants.PerformingStatus]);
                    LanAssetDetail.AvailabilityStatus = StringParse(reader[ClsPubDALConstants.AvailabilityStatus]);
                    LanAssetDetail.LanBookingUpto = reader[ClsPubDALConstants.LanBookingUpto].ToString();
                    LanAssetDetail.Location = StringParse(reader[ClsPubDALConstants.LOCATION]);
                 
                    LanAssetDetails.Add(LanAssetDetail);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return LanAssetDetails;
        }
    }
}
