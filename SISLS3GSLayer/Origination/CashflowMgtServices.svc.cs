#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orignation
/// Screen Name			: Cashflow Services ( Currency Creation Service Class)
/// Created By			: Kannan RC
/// Created Date		: 01-Jun-2010
/// Purpose	            : 
/// Last Updated By		: 
/// Last Updated Date   : 
/// Reason              : 
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using S3GDALayer;
using System.ServiceModel.Activation;
#endregion

namespace S3GServiceLayer.Origination
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "CashflowMgtServices" here, you must also update the reference to "CashflowMgtServices" in Web.config.
    public class CashflowMgtServices : ICashflowMgtServices
    {
        int intResult = 0;

        #region LIST
        //public byte[] FunPubGetCashflowflagList(SerializationMode SerMode, byte[] bytesObjCashflowflagListDataTable_SERLAY)
        //{
        //    try
        //    {
        //        S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_CashflowFlagListDataTable dtCashflowFlagListDetails;
        //        S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster ObjCashflowFlagListDetailsDetails = new S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster();
        //        dtCashflowFlagListDetails = ObjCashflowFlagListDetailsDetails.FunPubCashFlowFlagList(SerMode, bytesObjCashflowflagListDataTable_SERLAY);
        //        byte[] bytesCashflowListDetails = ClsPubSerialize.Serialize(dtCashflowFlagListDetails, SerializationMode.Binary);
        //        return bytesCashflowListDetails;
        //    }
        //    catch (Exception objExp)
        //    {
        //        ClsPubFaultException objFault = new ClsPubFaultException();
        //        objFault.ProReasonRW = "Erro in Cashflow List :" + objExp.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        //    }
        //}


        public byte[] FunPubGetGLSLCodeList(SerializationMode SerMode, byte[] bytesObjGLSLCodeListDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GLSLCodeListDataTable dtGLSLCodeListDetails;
                S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster ObjGLSLCodeListDetailsDetails = new S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster();
                dtGLSLCodeListDetails = ObjGLSLCodeListDetailsDetails.FunPubGLSLList(SerMode, bytesObjGLSLCodeListDataTable_SERLAY);
                byte[] bytesGLSLCodeListDetails = ClsPubSerialize.Serialize(dtGLSLCodeListDetails, SerializationMode.Binary);
                return bytesGLSLCodeListDetails;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in GLSL List :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion
        #region GetProgramNumber
        public byte[] FunPubGetProgramNumber(SerializationMode SerMode, byte[] bytesObjProgramNumnerDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetProgramDataTable dtProgramNumberDetails;
                S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster ObjProgramNumberDetailsDetails = new S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster();
                dtProgramNumberDetails = ObjProgramNumberDetailsDetails.FunPubProgramMaster(SerMode, bytesObjProgramNumnerDataTable_SERLAY);
                byte[] bytesProgramNumberDetails = ClsPubSerialize.Serialize(dtProgramNumberDetails, SerializationMode.Binary);
                return bytesProgramNumberDetails;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Program Number :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region GetCashflowLook
        public byte[] FunPubGetCashflowLook(SerializationMode SerMode, byte[] bytesObjCashflowLookDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CashflowMgtServices.S3G_Status_LookUPDataTable dtCashflowLookDetails;
                S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster ObjCashflowLookDetailsDetails = new S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster();
                dtCashflowLookDetails = ObjCashflowLookDetailsDetails.FunPubCashflowTypeMaster(SerMode, bytesObjCashflowLookDataTable_SERLAY);
                byte[] bytesCashflowLookDetails = ClsPubSerialize.Serialize(dtCashflowLookDetails, SerializationMode.Binary);
                return bytesCashflowLookDetails;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in cashflowType :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region Create CFM
        public int FunPubCreateCashflowMaster(SerializationMode SerMode, byte[] bytesObjCashflowDataTable_SERLAY, out string strCashflowNumber_Out)
        {
            try
            {
                S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster ObjCashflowMaster = new S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster();
                intResult = ObjCashflowMaster.FunPubCreateCashflow(SerMode, bytesObjCashflowDataTable_SERLAY, out strCashflowNumber_Out);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Cashflow Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }
        #endregion

        #region Modify CFM

        public int FunPubModifyCashflowMaster(SerializationMode SerMode, byte[] bytesObjCashflowDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster ObjCashflowMaster = new S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster();
                intResult = ObjCashflowMaster.FunPubModifyCashflow(SerMode, bytesObjCashflowDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Cashflow Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }
        #endregion

        #region Delete CFM

        public int FunPubDeleteCashflowMaster(SerializationMode SerMode, byte[] bytesObjDeleteCashflowDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster ObjDeleteCashflowMaster = new S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster();
                intResult = ObjDeleteCashflowMaster.FunPubDeleteCashflow(SerMode, bytesObjDeleteCashflowDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Cashflow Delete :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }


        #endregion

        #region Get CFM
        public byte[] FunPubGetCashflowDetails(SerializationMode SerMode, byte[] bytesObjCashflowDetailsDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetCashflowDetailsDataTable dtCashflowDetails;
                S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster ObjCashflowDetails = new S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster();
                dtCashflowDetails = ObjCashflowDetails.FunPubGetCashflowDetails(SerMode, bytesObjCashflowDetailsDataTable_SERLAY);
                byte[] bytesCashflowDetails = ClsPubSerialize.Serialize(dtCashflowDetails, SerializationMode.Binary);
                return bytesCashflowDetails;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in cashflow details :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubGetCashflowMapping(SerializationMode SerMode, byte[] bytesObjCashflowMappingDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CashflowMgtServices.S3G_ORG_GetCashflowMappingDataTable dtCashflowMapping;
                S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster ObjCashflowMapping = new S3GDALayer.Origination.CashflowMgtServices.ClsPubCashflowMaster();
                dtCashflowMapping = ObjCashflowMapping.FunPubGetCashflowMapping(SerMode, bytesObjCashflowMappingDataTable_SERLAY);
                byte[] bytesCashflowMapping = ClsPubSerialize.Serialize(dtCashflowMapping, SerializationMode.Binary);
                return bytesCashflowMapping;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in cashflow details Mapping :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public int FunPubCreateCashFlowRulesInt(SerializationMode SerMode, byte[] bytesObjROIRulesDataTable_SERLAY)
        {
            try
            {

                S3GDALayer.Origination.CashflowMgtServices.ClsPubNegativeListCustomers ObjCashflowRules = new S3GDALayer.Origination.CashflowMgtServices.ClsPubNegativeListCustomers();
                intResult = ObjCashflowRules.FunPubCreateCashFlowRulesInt(SerMode, bytesObjROIRulesDataTable_SERLAY);



            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in List Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        #endregion
    }
}

    

