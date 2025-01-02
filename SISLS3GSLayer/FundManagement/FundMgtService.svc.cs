using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using S3GDALayer;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.FundManagement
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "ApprovalMgtServices" here, you must also update the reference to "ApprovalMgtServices" in Web.config.

    public class FundMgtService : IFundMgtService
    {
        int intResult;
        SerializationMode SerMode = SerializationMode.Binary;
        byte[] bytesDataTable;

        #region "Funder Master"

        #region Create
        public int FunPubCreateOrModifyFunderMaster(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjFunderMaster_DataTable, out string strFunderCode, out Int64 intFunder_No, out string strErrorMsg)
        {
            try
            {
                strFunderCode = strErrorMsg = "";
                intFunder_No = 0;
                S3GDALayer.FundManagement.FundMgtService.ClsPubFunderMaster ObjFunderMaster = new S3GDALayer.FundManagement.FundMgtService.ClsPubFunderMaster();
                return ObjFunderMaster.FunPubCreateOrModifyFunderMaster(SerMode, bytesObjFunderMaster_DataTable, out strFunderCode, out intFunder_No, out strErrorMsg);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region Approval
        public int FunPubInsertApproval(SerializationMode SerMode, byte[] bytesObjApproval_DataTable, out string strApprovalNumber, out Int64 intApproval_No)
        {
            try
            {
                strApprovalNumber = string.Empty;
                intApproval_No = 0;
                S3GDALayer.FundManagement.FundMgtService.ClsPubFunderMaster ObjFunderMaster = new S3GDALayer.FundManagement.FundMgtService.ClsPubFunderMaster();
                return ObjFunderMaster.FunPubInsertApproval(SerMode, bytesObjApproval_DataTable, out strApprovalNumber, out intApproval_No);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region Tmp Insert Sanction Detail
        public int FunPubInsertTmpSancDtl(SerializationMode SerMode, byte[] byteObjSanc_DataTable)
        {
            try
            {
                S3GDALayer.FundManagement.FundMgtService.ClsPubFunderMaster ObjFunderMaster = new S3GDALayer.FundManagement.FundMgtService.ClsPubFunderMaster();
                return ObjFunderMaster.FunPubInsertTmpSancDtl(SerMode, byteObjSanc_DataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #endregion

        #region "Tranche Creation"

        #region Create
        public int FunPubCreateOrModifyTrancheMaster(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjTranche_DataTable, out Int64 intTranche_No, out string strErrorMsg)
        {
            try
            {

                intTranche_No = 0;
                S3GDALayer.FundManagement.FundMgtService.ClsPubTrancheCreation ObjtrancheMaster = new S3GDALayer.FundManagement.FundMgtService.ClsPubTrancheCreation();
                return ObjtrancheMaster.FunPubCreateOrModifyTrancheMaster(SerMode, bytesObjTranche_DataTable, out intTranche_No, out strErrorMsg);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion



        #endregion

        #region "Note Creation"

        #region Create
        public int FunPubCreateOrModifyNote(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjNote_DataTable, out Int64 intNote_No, out string strErrorMsg, out string StrNoteNumber)
        {
            try
            {

                intNote_No = 0;
                S3GDALayer.FundManagement.FundMgtService.ClsPubNoteCreation ObjNote = new S3GDALayer.FundManagement.FundMgtService.ClsPubNoteCreation();
                return ObjNote.FunPubCreateOrModifyNote(SerMode, bytesObjNote_DataTable, out intNote_No, out strErrorMsg, out StrNoteNumber);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubCreateOrModifyGrossMarginCalculation(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjGM_DataTable, out Int64 intNote_No, out string strErrorMsg, out string StrNoteNumber)
        {
            try
            {
                intNote_No = 0;
                S3GDALayer.FundManagement.FundMgtService.ClsPubGrossMarginCreation ObjNote = new S3GDALayer.FundManagement.FundMgtService.ClsPubGrossMarginCreation();
                return ObjNote.FunPubCreateOrModifyGrossMarginCalculation(SerMode, bytesObjGM_DataTable, out intNote_No, out strErrorMsg, out StrNoteNumber);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }



        #region "NOTE Approval"

        public int FunPubInsertNoteApproval(SerializationMode SerMode, byte[] bytesObjNote_DataTableAPP, out string strNoteApprovalNumber, out Int64 intNoteApproval_No)
        {
            try
            {
                strNoteApprovalNumber = "";
                intNoteApproval_No = 0;
                S3GDALayer.FundManagement.FundMgtService.ClsPubNoteCreation ObjNote = new S3GDALayer.FundManagement.FundMgtService.ClsPubNoteCreation();
                return ObjNote.FunPubInsertNoteApproval(SerMode, bytesObjNote_DataTableAPP, out strNoteApprovalNumber, out intNoteApproval_No);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion
        #endregion



        #endregion


    }
}
