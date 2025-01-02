using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity.Reports;
using S3GDALayer.Reports;
using S3GBusEntity;
 

namespace S3GServiceLayer.Reports
{
    // NOTE: If you change the interface name "IReportAdminMgtServices" here, you must also update the reference to "IReportAdminMgtServices" in Web.config.
    [ServiceContract]
    public interface IReportAdminMgtServices
    {
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetLOB(int CompanyId, int UserId,int ProgramId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetMLA(byte[] ObjPrimeAccountBytes);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetSLA(string Type, int CompanyId, string LobId, string CustomerId, string PNum, int IsActive, int ProgramId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetLanDetails(byte[] ObjLanBytes);
        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubGetLANNumber(byte[] LANInput);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubGetLanDetails(ClsPubLANRegisterinput LANInput);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetLanAssetDetails(int Company_Id, string Lan);
        //Added for MIS Report On 08-12-2017 = > Code Starts
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateScheduleJobForReport(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjApprovalDatatable_SERLAY, out string strErrMsg, out string strAppDetId);
        //Added for MIS Report On 08-12-2017 = > Code Ends
 
    }
}
