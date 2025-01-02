using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace S3GServiceLayer.FundManagement
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFundMgtService" in both code and config file together.
    [ServiceContract]
    public interface IFundMgtService
    {
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateOrModifyFunderMaster(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjFunderMaster_DataTable, out string strFunderCode, out Int64 intFunder_No, out string strErrorMsg);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertApproval(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjApproval_DataTable, out string strApprovalNumber, out Int64 intApproval_No);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateOrModifyTrancheMaster(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjTranche_DataTable, out Int64 intTranche_No, out string strErrorMsg);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateOrModifyNote(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjNote_DataTable, out Int64 intNote_No, out string strErrorMsg, out string StrNoteNumber);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertNoteApproval(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjNote_DataTableAPP, out string strNoteApprovalNumber, out Int64 intNoteApproval_No);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertTmpSancDtl(S3GBusEntity.SerializationMode SerMode, byte[] byteObjSanc_DataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateOrModifyGrossMarginCalculation(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjNote_DataTable, out Int64 intNote_No, out string strErrorMsg, out string StrNoteNumber);


    }
}
