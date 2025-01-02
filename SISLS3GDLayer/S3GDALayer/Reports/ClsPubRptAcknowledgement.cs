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
    public class ClsPubRptAcknowledgement:ClsPubDalReportBase
    {
        #region LOB
        /// <summary>
        /// To Get the LOB Details
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>

        public List<ClsPubDropDownList> FunPubGetPDCLOB(int CompanyId, int UserId, int ProgramId, string CustomerId)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetPDCLOBDetails);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPROGRAMID, DbType.Int32, ProgramId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.Int32, CustomerId);

                //IDataReader reader = db.ExecuteReader(command); modified by sangeetha for Oracle Conversion - 26th Apr 2012
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

        //#region Branch
        ///// <summary>
        ///// To Get the Branch Details
        ///// </summary>
        ///// <param name="CompanyId"></param>
        ///// <param name="UserId"></param>
        ///// <param name="Is_Active"></param>
        ///// <returns></returns>

        //public List<ClsPubDropDownList> FunPubBranch(int CompanyId, int UserId, int ProgramId, string CustomerId,  bool Is_Active)
        //{
        //    List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
        //    try
        //    {

        //        DbCommand command = db.GetStoredProcCommand(SPNames.GetBranch);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, UserId);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, Is_Active);


        //        IDataReader reader = db.ExecuteReader(command);

        //        while (reader.Read())
        //        {
        //            ClsPubDropDownList dropDownList = new ClsPubDropDownList();

        //            dropDownList.ID = StringParse(reader[ClsPubDALConstants.BRANCHID]);
        //            dropDownList.Description = StringParse(reader[ClsPubDALConstants.BRANCH]);
        //            dropDownLists.Add(dropDownList);
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
        //    }
        //    return FunPriLoadSelect(dropDownLists);
        //}
        //#endregion

        #region Prime Account Number
        /// <summary>
        /// To Get the Prime Account Number based on Customer,LOB and Branch
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="CustomerId"></param>
        /// <param name="LobId"></param>
        /// <param name="BranchId"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubPDCAckPAN(ClsPubStockReceivableparameters PDCPAN)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetPDCPAN);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, PDCPAN.CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMUSERID, DbType.Int32, PDCPAN.UserId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.Int32, PDCPAN.CustomerId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, PDCPAN.LobId); 
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID1, DbType.Int32, PDCPAN.LocationId1);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOCATIONID2, DbType.Int32, PDCPAN.LocationId2);

                //IDataReader reader = db.ExecuteReader(command);modified by sangeetha for Oracle Conversion - 26th Apr 2012
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

        #region Sub Account Number
        /// <summary>
        /// To Get the Sub Account Number
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="PNum"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubPDCAckSAN(int CompanyId, string PNum)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetPDCSAN);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, PNum);

                //IDataReader reader = db.ExecuteReader(command); modified by sangeetha for Oracle Conversion - 26th Apr 2012
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

        #region PDC Number
        /// <summary>
        /// To Get the PDC Number
        /// </summary>
        /// <param name="PNum"></param>
        /// <param name="SNum"></param>
        /// <returns></returns>
        public List<ClsPubDropDownList> FunPubPDCNumber(string PNum, string SNum)
        {
            List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.PDCNumber);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM,DbType.String, PNum);
                if (SNum != string.Empty)
                {
                    db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSANUM, DbType.String, SNum);
                }
                //IDataReader reader = db.ExecuteReader(command);modified by sangeetha for Oracle Conversion - 26th Apr 2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubDropDownList dropDownList = new ClsPubDropDownList();
                    dropDownList.ID = StringParse(reader[ClsPubDALConstants.PDCNO]);
                    dropDownList.Description = StringParse(reader[ClsPubDALConstants.PDCNO]);
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

        //#region PDC Date
        ///// <summary>
        ///// To Get the PDC Date
        ///// </summary>
        ///// <param name="PdcNo"></param>
        ///// <returns></returns>
        //public List<ClsPubDropDownList> FunPubPDCDate(string PdcNo)
        //{
        //    List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
        //    try
        //    {
        //        DbCommand command = db.GetStoredProcCommand(SPNames.PDCNumber);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPDCNUMBER, DbType.String, PdcNo);
        //        IDataReader reader = db.ExecuteReader(command);
        //        while (reader.Read())
        //        {
        //            ClsPubDropDownList dropDownList = new ClsPubDropDownList();
        //            dropDownList.ID = StringParse(reader[ClsPubDALConstants.PDCDATE]);
        //            dropDownList.Description = StringParse(reader[ClsPubDALConstants.PDCDATE]);
        //            dropDownLists.Add(dropDownList);
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
        //    }
        //    return FunPriLoadSelect(dropDownLists);
        //}
        //#endregion

        public ClsPubPDCDateLOBBranch FunPubGetHeaderLobBranchDetails(string PANum)
        {
            ClsPubPDCDateLOBBranch headerLobBranchDetails = new ClsPubPDCDateLOBBranch();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.GetPDCLobBranchNo);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, PANum);

                //IDataReader reader = db.ExecuteReader(command);modified by sangeetha for Oracle Conversion - 26th Apr 2012
                IDataReader reader = db.FunPubExecuteReader(command);

                while (reader.Read())
                {
                    headerLobBranchDetails.LOBId = StringParse(reader[ClsPubDALConstants.LOBID]);
                    headerLobBranchDetails.BranchId = StringParse(reader[ClsPubDALConstants.BRANCHID]);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return headerLobBranchDetails;
        }
        //#region Get MLA
        ///// <summary>
        ///// To get the Prime Account Number
        ///// </summary>
        ///// <param name="ObjPrimeAccount"></param>
        ///// <returns></returns>
        //public List<ClsPubDropDownList> FunPubGetMLA(ClsPubPrimeAccountDetails ObjPrimeAccount)
        //{
        //    List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
        //    try
        //    {
        //        DbCommand command = db.GetStoredProcCommand(SPNames.PLASLA_Credit);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTYPE, DbType.String, ObjPrimeAccount.Type);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, ObjPrimeAccount.CompanyId);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, ObjPrimeAccount.IsActive);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.String, ObjPrimeAccount.CustomerId);
        //        if (ObjPrimeAccount.LobId != 0)
        //        {
        //            db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, ObjPrimeAccount.LobId);
        //        }
        //        if (ObjPrimeAccount.BranchId != 0)
        //        {
        //            db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMBRANCHID, DbType.Int32, ObjPrimeAccount.BranchId);
        //        }
        //        IDataReader reader = db.ExecuteReader(command);
        //        while (reader.Read())
        //        {
        //            ClsPubDropDownList dropDownList = new ClsPubDropDownList();

        //            dropDownList.ID = StringParse(reader[ClsPubDALConstants.PANUM]);
        //            dropDownList.Description = StringParse(reader[ClsPubDALConstants.PANUM]);
        //            dropDownLists.Add(dropDownList);
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
        //    }
        //    return FunPriLoadSelect(dropDownLists);
        //}
        //#endregion

        //#region Get SLA
        ///// <summary>
        ///// To get th Sub Account Number
        ///// </summary>
        ///// <param name="Type"></param>
        ///// <param name="CompanyId"></param>
        ///// <param name="IsActive"></param>
        ///// <param name="CustomerId"></param>
        ///// <param name="PNum"></param>
        ///// <returns></returns>
        //public List<ClsPubDropDownList> FunPubGetSLA(string Type, int CompanyId, bool IsActive, string CustomerId, string PNum)
        //{
        //    List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
        //    try
        //    {
        //        DbCommand command = db.GetStoredProcCommand(SPNames.PLASLA_Credit);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMTYPE, DbType.String, Type);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMISACTIVE, DbType.Boolean, IsActive);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, PNum);

        //        IDataReader reader = db.ExecuteReader(command);
        //        while (reader.Read())
        //        {
        //            ClsPubDropDownList dropDownList = new ClsPubDropDownList();

        //            dropDownList.ID = StringParse(reader[ClsPubDALConstants.SANUM]);
        //            dropDownList.Description = StringParse(reader[ClsPubDALConstants.SANUM]);
        //            dropDownLists.Add(dropDownList);
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
        //    }
        //    return FunPriLoadSelect(dropDownLists);
        //}
        //#endregion

        //#region PDC Number
        ///// <summary>
        ///// To Get the PDC Number
        ///// </summary>
        ///// <param name="CompanyId"></param>
        ///// <param name="CustomerId"></param>
        ///// <param name="LOBId"></param>
        ///// <param name="BranchId"></param>
        ///// <returns></returns>
        //public List<ClsPubDropDownList> FunPubGetPDCNO(ClsPubPrimeAccountDetails ObjPDC)
        //{
        //    List<ClsPubDropDownList> dropDownLists = new List<ClsPubDropDownList>();
        //    try
        //    {
        //        DbCommand command = db.GetStoredProcCommand(SPNames.PDCNumber);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, ObjPDC.CompanyId);
        //        db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCUSTOMERID, DbType.String, ObjPDC.CustomerId);
        //        if (ObjPDC.LobId != 0)
        //        {
        //            db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMLOBID, DbType.Int32, ObjPDC.LobId);
        //        }
        //        if (ObjPDC.BranchId != 0)
        //        {
        //            db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMBRANCHID, DbType.Int32, ObjPDC.BranchId);
        //        }
        //        if (ObjPDC.PaNum != string.Empty)
        //        {
        //            db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPANUM, DbType.String, ObjPDC.PaNum);
        //        }
        //        if (ObjPDC.SaNum != string.Empty)
        //        {
        //            db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMSANUM, DbType.String, ObjPDC.SaNum);
        //        }

        //        IDataReader reader = db.ExecuteReader(command);
        //        while (reader.Read())
        //        {
        //            ClsPubDropDownList dropDownList = new ClsPubDropDownList();
        //            dropDownList.ID = StringParse(reader[ClsPubDALConstants.PDCDATE]);
        //            dropDownList.Description = StringParse(reader[ClsPubDALConstants.PDCNO]);
        //            dropDownLists.Add(dropDownList);
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
        //    }
        //    return FunPriLoadSelect(dropDownLists);
        //}
        //#endregion

        #region PDC Details Grid
        /// <summary>
        /// To Get the Details for PDC Grid
        /// </summary>
        /// <param name="PDC_NO"></param>
        /// <returns></returns>
        public List<ClsPubPDCDetails> FunPubGetPDCDetails(string PDC_NO,int CompanyId)
        {
            List<ClsPubPDCDetails> PDCDetails = new List<ClsPubPDCDetails>();
            try
            {
                DbCommand command = db.GetStoredProcCommand(SPNames.PDCDetailsGrid);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMPDCNUMBER, DbType.String, PDC_NO);
                db.AddInParameter(command, ClsPubDALConstants.INPUTPARAMCOMPANYID, DbType.Int32, CompanyId);

                //IDataReader reader = db.ExecuteReader(command);modified by sangeetha for Oracle Conversion - 26th Apr 2012
                IDataReader reader = db.FunPubExecuteReader(command);
                while (reader.Read())
                {
                    ClsPubPDCDetails PDCDetail = new ClsPubPDCDetails();
                    PDCDetail.PDCSNo = IntParse(reader[ClsPubDALConstants.PDCSNO]);
                    PDCDetail.ChequeNumber = LongParse(reader[ClsPubDALConstants.CHEQUENUMBER]);
                    //PDCDetail.ChequeNumber = IntParse(reader[ClsPubDALConstants.CHEQUENUMBER]);
                    PDCDetail.ChequeDate = reader[ClsPubDALConstants.CHEQUEDATE].ToString();
                    //PDCDetail.ChequeDate = StringParse(reader[ClsPubDALConstants.CHEQUEDATE]);
                    PDCDetail.DrawnonBank = StringParse(reader[ClsPubDALConstants.DRAWNBANK]);
                    PDCDetail.BankingDate = reader[ClsPubDALConstants.BANKINGDATE].ToString();
                    PDCDetail.Amount = DecimalParse(reader[ClsPubDALConstants.AMOUNT]);
                    PDCDetail.GPSSuffix = StringParse(reader[ClsPubDALConstants.Gpssuffix]);
                    PDCDetails.Add(PDCDetail);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            }
            return PDCDetails;
        }
        #endregion
    }
}