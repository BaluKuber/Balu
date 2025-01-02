using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity.Reports;
using S3GDALayer.Reports;
using S3GBusEntity;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.Reports
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "ReportAdminMgtServices" here, you must also update the reference to "ReportAdminMgtServices" in Web.config.
    public class ReportAdminMgtServices : IReportAdminMgtServices
    {
        #region LANRegister

        #region Get LOB
        public byte[] FunPubGetLOB(int CompanyId, int UserId,int ProgramId)
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptLANRegister obj = new ClsPubRptLANRegister();
            dropDownLists = obj.FunPubGetLOB(CompanyId, UserId, ProgramId);
            return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Get prime Account Number
        public byte[] FunPubGetMLA(byte[] ObjPrimeAccountBytes)
        {
            try
            {
            ClsPubPrimeAccountDetails ObjPrimeAccount = (ClsPubPrimeAccountDetails)ClsPubSerialize.DeSerialize(ObjPrimeAccountBytes, SerializationMode.Binary, typeof(ClsPubPrimeAccountDetails));
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptLANRegister obj = new ClsPubRptLANRegister();
            dropDownLists = obj.FunPubGetMLA(ObjPrimeAccount);
            return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Get Sub Account Number
        public byte[] FunPubGetSLA(string Type, int CompanyId, string LobId, string CustomerId, string PNum, int IsActive, int ProgramId)
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptLANRegister obj = new ClsPubRptLANRegister();
            dropDownLists = obj.FunPubGetSLA(Type, CompanyId, LobId, CustomerId, PNum, IsActive, ProgramId);
            return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        //#region Get lan number details
        //public byte[] FunPubGetLANNumber(byte[] LANInput)
        //{
        //    try
        //    {
        //        ClsPubLANRegisterinput lANNumberDetails = (ClsPubLANRegisterinput)ClsPubSerialize.DeSerialize(LANInput, SerializationMode.Binary, typeof(ClsPubLANRegisterinput)); ;
        //        List<ClsPubLANRegisterDetails> NumberDetails;
        //        ClsPubRptLANRegister obj = new ClsPubRptLANRegister();
        //        NumberDetails = obj.FunPubGetLANNumber(lANNumberDetails);
        //        return ClsPubSerialize.Serialize(NumberDetails, SerializationMode.Binary);
        //    }
        //    catch (Exception ex)
        //    {
        //        ClsPubFaultException objfaultex = new ClsPubFaultException();
        //        objfaultex.ProReasonRW = ex.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
        //    }
        //}
        //#endregion


        public byte[] FunPubGetLanDetails(byte[] ObjLanBytes)
        {
            try
            {
                ClsPubLANRegisterinput Lan = (ClsPubLANRegisterinput)ClsPubSerialize.DeSerialize(ObjLanBytes, SerializationMode.Binary, typeof(ClsPubLANRegisterinput));
                ClsPubLeaseAssetRegisterDetails LANRegisterDetails;
                ClsPubRptLANRegister obj = new ClsPubRptLANRegister();
                LANRegisterDetails = obj.FunPubGetLanDetails(Lan);
                return ClsPubSerialize.Serialize(LANRegisterDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        #region Get lan number details
        //public byte[] FunPubGetLANNumber(byte[] LANInput)
        //{
        //    try
        //    {
        //        ClsPubLANRegisterinput lANNumberDetails = (ClsPubLANRegisterinput)ClsPubSerialize.DeSerialize(LANInput, SerializationMode.Binary, typeof(ClsPubLANRegisterinput));
        //        List<ClsPubDropDownList> dropDownLists;
        //        ClsPubRptLANRegister obj = new ClsPubRptLANRegister();
        //        dropDownLists = obj.FunPubGetLANNumber(lANNumberDetails);
        //        return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
        //    }
        //    catch (Exception ex)
        //    {
        //        ClsPubFaultException objfaultex = new ClsPubFaultException();
        //        objfaultex.ProReasonRW = ex.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
        //    }
        //}
        #endregion


        #region Get lan register details
        //public byte[] FunPubGetLanDetails(ClsPubLANRegisterinput LANInput) //byte[] LANInput
        //{
        //    //try
        //    //{
        //    //ClsPubLANRegisterinput lANRegisterDetails=(ClsPubLANRegisterinput)ClsPubSerialize.DeSerialize(LANInput, SerializationMode.Binary, typeof(ClsPubLANRegisterinput));;
        //    //List<ClsPubLANRegisterDetails> RegisterDetails;
        //    //ClsPubRptLANRegister obj = new ClsPubRptLANRegister();
        //    //RegisterDetails = obj.FunPubGetLanDetails(lANRegisterDetails);
        //    //return ClsPubSerialize.Serialize(RegisterDetails,SerializationMode.Binary);
        //    //}
        //    try
        //    {
        //        ClsPubLANLocation details = new ClsPubLANLocation();
        //        ClsPubRptLANRegister report = new ClsPubRptLANRegister();
        //        details = report.FunPubGetLanDetails(LANInput);
        //        return ClsPubSerialize.Serialize(details, SerializationMode.Binary);
        //    }
        //    catch (Exception ex)
        //    {
        //        ClsPubFaultException objfaultex = new ClsPubFaultException();
        //        objfaultex.ProReasonRW = ex.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
        //    }
        //}
         #endregion

        #region Get lan asset details
        public byte[] FunPubGetLanAssetDetails(int Company_Id, string Lan)
        {
            try
            {
            List<ClsPubLANdetails> LanAssetDetails;
            ClsPubRptLANRegister obj = new ClsPubRptLANRegister();
            LanAssetDetails = obj.FunPubGetLanAssetDetails(Company_Id, Lan);
            return ClsPubSerialize.Serialize(LanAssetDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion
        #region MIS Report
        public int FunPubCreateScheduleJobForReport(S3GBusEntity.SerializationMode SerMode, byte[] bytesApproval_Datatable, out string strErrMsg, out string strAppDetId)
        {
            try
            {
                S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetIdentification objSheduleJob = new S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetIdentification();
                return objSheduleJob.FunPubCreateScheduleJobForReport(SerMode, bytesApproval_Datatable, out strErrMsg, out strAppDetId);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion
    }
}
 #endregion