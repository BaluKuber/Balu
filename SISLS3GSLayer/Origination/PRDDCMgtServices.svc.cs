#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: PRDDC Master
/// Created By			: Kaliraj K
/// Created Date		: 07-Jun-2010
/// Purpose	            : 
/// Modified By         :  Saranya I
/// <Program Summary>
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GDALayer;
using System.Data;
using S3GBusEntity;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.Origination
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "PRDDCMgtServices" here, you must also update the reference to "PRDDCMgtServices" in Web.config.
    public class PRDDCMgtServices : IPRDDCMgtServices
    {
        int intResult;
        SerializationMode SerMode = SerializationMode.Binary;
        S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDCMaster ObjPRDDCMaster = new S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDCMaster();
        S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDCMaster ObjTAPRDDCMaster = new S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDCMaster();
        //Added by saranya for FIR Master
        S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDCMaster ObjFIRMaster = new S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDCMaster();
        //End
        S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDCMaster ObjLegalMaster = new S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDCMaster();
        S3GDALayer.LoanAdmin.PDDCMgtServices.ClsPubPDDCMaster ObjPDDCMaster = new S3GDALayer.LoanAdmin.PDDCMgtServices.ClsPubPDDCMaster();

        byte[] bytesDataTable;

        #region IPRDDCMgtServices PRDDC Master

        public int FunPubCreatePRDDCDocDetails(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable)
        {
            try
            {
                intResult = ObjPRDDCMaster.FunPubCreatePRDDCDocDetails(SerMode, bytesObjS3G_ORG_PRDDCMasterDataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in PRDDC Code Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        
        /// <summary>
        /// To Create FIR Document Details
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjS3G_ORG_FIRMasterDataTable"></param>
        /// <returns></returns>
        public int FunPubCreateFIRDocDetails(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRMasterDataTable)
        {
            try
            {
                intResult = ObjFIRMaster.FunPubCreateFIRDocDetails(SerMode, bytesObjS3G_ORG_FIRMasterDataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in FIR Code Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubCreatelegalDocDetails(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjS3G_ORG_LegalMasterDataTable)
        {
            try
            {
                intResult = ObjLegalMaster.FunPubCreatelegalDocDetails(SerMode, bytesObjS3G_ORG_LegalMasterDataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in FIR Code Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }
        //END
        public int FunPubCreateOtherDocDetails(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable)
        {
            try
            {
                intResult = ObjPRDDCMaster.FunPubCreateOtherDocDetails(SerMode, bytesObjS3G_ORG_PRDDCMasterDataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in PRDDC Code Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

       
        /// <summary>
        /// To Create FIR Other Doc Details
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjS3G_ORG_FIRMasterDataTable"></param>
        /// <returns></returns>
        public int FunPubCreateFIROtherDocDetails(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRMasterDataTable)
        {
            try
            {
                intResult = ObjFIRMaster.FunPubCreateFIROtherDocDetails(SerMode, bytesObjS3G_ORG_FIRMasterDataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in FIR Code Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }
        //END
        public byte[] FunPubQueryPRDDCDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable)
        {
            DataSet dsPRDDC = null;
            try
            {
                dsPRDDC = ObjPRDDCMaster.FunPubQueryPRDDCDocDetails(SerMode, bytesObjS3G_ORG_PRDDCMasterDataTable);
                bytesDataTable = ClsPubSerialize.Serialize(dsPRDDC, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Getting PRDDC :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            finally
            {
                dsPRDDC.Dispose();
                dsPRDDC = null;
            }
            return bytesDataTable;

        }

       /// <summary>
       /// To Get the FIR Doc Details
       /// </summary>
       /// <param name="SerMode"></param>
       /// <param name="bytesObjS3G_ORG_FIRMasterDataTable"></param>
       /// <returns></returns>
        public byte[] FunPubQueryFIRDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRMasterDataTable)
        {
            DataSet dsPRDDC = null;
            try
            {
                dsPRDDC = ObjFIRMaster.FunPubQueryFIRDocDetails(SerMode, bytesObjS3G_ORG_FIRMasterDataTable);
                bytesDataTable = ClsPubSerialize.Serialize(dsPRDDC, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Getting FIR :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            finally
            {
                dsPRDDC.Dispose();
                dsPRDDC = null;
            }
            return bytesDataTable;

        }
        //END

        public byte[] FunPubQueryPRDDCMaster(SerializationMode SerMode, byte[] bytesObjPRDDCMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable dtPRDDCMaster;
                dtPRDDCMaster = ObjPRDDCMaster.FunPubQueryPRDDCMaster(SerMode, bytesObjPRDDCMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtPRDDCMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }



        public byte[] FunPubQuerylegalDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_LegalMasterDataTable)
        {
            DataSet dsPRDDC = null;
            try
            {
                dsPRDDC = ObjLegalMaster.FunPubQuerylegalDocDetails(SerMode, bytesObjS3G_ORG_LegalMasterDataTable);
                bytesDataTable = ClsPubSerialize.Serialize(dsPRDDC, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Getting FIR :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            finally
            {
                dsPRDDC.Dispose();
                dsPRDDC = null;
            }
            return bytesDataTable;

        }

        public byte[] FunPubQueryPRDDCMasterCombi(SerializationMode SerMode, byte[] bytesObjPRDDCMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable dtPRDDCMaster;
                dtPRDDCMaster = ObjPRDDCMaster.FunPubQueryPRDDCMasterCombi(SerMode, bytesObjPRDDCMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtPRDDCMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        public int FunPubExitsPRDDTrans(SerializationMode SerMode, byte[] bytesObjPRDDTExitsDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster ObjDeletePRDDTMaster = new S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster();
                intResult = ObjDeletePRDDTMaster.FunPubExitsPRDDTrans(SerMode, bytesObjPRDDTExitsDataTable_SERLAY);
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

        #region IPDDCMgtServices PDDC Master

        public int FunPubCreatePDDCDocDetails(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjS3G_LOAN_PDDCMasterDataTable)
        {
            try
            {
                intResult = ObjPDDCMaster.FunPubCreatePDDCDocDetails(SerMode, bytesObjS3G_LOAN_PDDCMasterDataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in PRDDC Code Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubCreateOtherPostDocDetails(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjS3G_ORG_PDDCMasterDataTable)
        {
            try
            {
                intResult = ObjPDDCMaster.FunPubCreateOtherDocDetails(SerMode, bytesObjS3G_ORG_PDDCMasterDataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in PRDDC Code Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public byte[] FunPubQueryPDDCDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PDDCMasterDataTable)
        {
            DataSet dsPRDDC = null;
            try
            {
                dsPRDDC = ObjPDDCMaster.FunPubQueryPDDCDocDetails(SerMode, bytesObjS3G_ORG_PDDCMasterDataTable);
                bytesDataTable = ClsPubSerialize.Serialize(dsPRDDC, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Getting PRDDC :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            finally
            {
                dsPRDDC.Dispose();
                dsPRDDC = null;
            }
            return bytesDataTable;

        }

        //public byte[] FunPubQueryPDDCMaster(SerializationMode SerMode, byte[] bytesObjPDDCMasterDataTable_SERLAY)
        //{
        //    try
        //    {
        //        S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable dtPDDCMaster;
        //        dtPDDCMaster = ObjPDDCMaster.FunPubQueryPDDCDocDetails(SerMode, bytesObjPDDCMasterDataTable_SERLAY);
        //        bytesDataTable = ClsPubSerialize.Serialize(dtPDDCMaster, SerializationMode.Binary);
        //    }
        //    catch (Exception objExp)
        //    {
        //        ClsPubFaultException objFault = new ClsPubFaultException();
        //        objFault.ProReasonRW = objExp.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        //    }
        //    return bytesDataTable;
        //}

        public byte[] FunPubQueryPDDCMasterCombi(SerializationMode SerMode, byte[] bytesObjPDDCMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable dtPDDCMaster;
                dtPDDCMaster = ObjPRDDCMaster.FunPubQueryPRDDCMasterCombi(SerMode, bytesObjPDDCMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtPDDCMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        //public int FunPubExitsPDDTrans(SerializationMode SerMode, byte[] bytesObjPDDTExitsDataTable_SERLAY)
        //{
        //    try
        //    {
        //        S3GDALayer.LoanAdmin.PDDCMgtServices.ClsPubPDDCMaster ObjDeletePDDTMaster = new S3GDALayer.LoanAdmin.PDDCMgtServices.ClsPubPDDCMaster();
        //        intResult = ObjDeletePDDTMaster.FunPubExitsPRDDTrans(SerMode, bytesObjPDDTExitsDataTable_SERLAY);
        //    }
        //    catch (Exception objExp)
        //    {
        //        ClsPubFaultException objFault = new ClsPubFaultException();
        //        objFault.ProReasonRW = "Erro in Cashflow Delete :" + objExp.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        //    }
        //    return intResult;
        //}

        #endregion

        #region PRDDT
        #region GeLookUp
        public byte[] FunPubGetPRDDTLookUp(SerializationMode SerMode, byte[] bytesObjPRDDTLookDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDTLookUpDataTable dtPRDDTLookDetails;
                S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster ObjPRDDTLookDetailsDetails = new S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster();
                dtPRDDTLookDetails = ObjPRDDTLookDetailsDetails.FunPubPRDDTLookup(SerMode, bytesObjPRDDTLookDataTable_SERLAY);
                byte[] bytesPRDDTLookDetails = ClsPubSerialize.Serialize(dtPRDDTLookDetails, SerializationMode.Binary);
                return bytesPRDDTLookDetails;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in PRDDT Look Up Type :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubGetTypeTrans(SerializationMode SerMode, byte[] bytesObjTypeTransDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable dtTypeTransDetails;
                S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster ObjTypeTransDetailsDetails = new S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster();
                dtTypeTransDetails = ObjTypeTransDetailsDetails.FunPubGetTypeTrans(SerMode, bytesObjTypeTransDataTable_SERLAY);
                byte[] bytesTypeTransDetails = ClsPubSerialize.Serialize(dtTypeTransDetails, SerializationMode.Binary);
                return bytesTypeTransDetails;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Type Trans Look Up :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion
        #region Get PRDDT

        public byte[] FunPubGetPRDDTrans(SerializationMode SerMode, byte[] bytesObjPRDDTransDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable dtPRDDTransDetails;
                S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster ObjPRDDTransDetailsDetails = new S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster();
                dtPRDDTransDetails = ObjPRDDTransDetailsDetails.FunPubGetPRDDTrans(SerMode, bytesObjPRDDTransDataTable_SERLAY);
                byte[] bytesPRDDTransDetails = ClsPubSerialize.Serialize(dtPRDDTransDetails, SerializationMode.Binary);
                return bytesPRDDTransDetails;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Type Trans Look Up :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubGetPRDDTransDoc(SerializationMode SerMode, byte[] bytesObjPRDDTransDocDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable dtPRDDTransDocDetails;
                S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster ObjPRDDTransDocDetailsDetails = new S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster();
                dtPRDDTransDocDetails = ObjPRDDTransDocDetailsDetails.FunPubGetPRDDTransDoc(SerMode, bytesObjPRDDTransDocDataTable_SERLAY);
                byte[] bytesPRDDTransDocDetails = ClsPubSerialize.Serialize(dtPRDDTransDocDetails, SerializationMode.Binary);
                return bytesPRDDTransDocDetails;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Type Trans Look Up :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubCreatePRDDTransMaster(SerializationMode SerMode, byte[] bytesObjPRDDTrans_SERLAY, out string strPRDDTNumber_Out)
        {
            try
            {
                S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster ObjPRDDTransMaster = new S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster();
                intResult = ObjPRDDTransMaster.FunPubCreatePRDDTrans(SerMode, bytesObjPRDDTrans_SERLAY,out strPRDDTNumber_Out);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in PRDDTrans Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubModifyPRDDTransMaster(SerializationMode SerMode, byte[] bytesObjPRDDTrans_SERLAY)
        {
            try
            {
                S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster ObjPRDDTransMaster = new S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster();
                intResult = ObjPRDDTransMaster.FunPubModifyPRDDTrans(SerMode, bytesObjPRDDTrans_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in PRDDTrans Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        #endregion



        #endregion


        #region PDDT

        public int FunPubCreatePDDTransMaster(SerializationMode SerMode, byte[] bytesObjPDDTrans_SERLAY, out string strPRDDTNumber_Out)
        {
            try
            {
                S3GDALayer.LoanAdmin.PDDCMgtServices.ClsPubPDDTransaction ObjPDDTransMaster = new S3GDALayer.LoanAdmin.PDDCMgtServices.ClsPubPDDTransaction();
                intResult = ObjPDDTransMaster.FunPubCreatePDDTrans(SerMode, bytesObjPDDTrans_SERLAY, out strPRDDTNumber_Out);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in PRDDTrans Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubModifyPDDTransMaster(SerializationMode SerMode, byte[] bytesObjPDDTrans_SERLAY)
        {
            try
            {
                S3GDALayer.LoanAdmin.PDDCMgtServices.ClsPubPDDTransaction ObjPDDTransMaster = new S3GDALayer.LoanAdmin.PDDCMgtServices.ClsPubPDDTransaction();
                intResult = ObjPDDTransMaster.FunPubModifyPDDTrans(SerMode, bytesObjPDDTrans_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in PRDDTrans Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        #endregion


        #region Trade Advance PRDDC Master

        public int FunPubTACreatePRDDCDocDetails(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable)
        {
            try
            {
                intResult = ObjTAPRDDCMaster.FunPubCreatePRDDCDocDetails(SerMode, bytesObjS3G_ORG_PRDDCMasterDataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in PRDDC Code Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubTACreateOtherDocDetails(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable)
        {
            try
            {
                intResult = ObjTAPRDDCMaster.FunPubCreateOtherDocDetails(SerMode, bytesObjS3G_ORG_PRDDCMasterDataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in PRDDC Code Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public byte[] FunPubTAQueryPRDDCDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable)
        {
            DataSet dsPRDDC = null;
            try
            {
                dsPRDDC = ObjTAPRDDCMaster.FunPubQueryPRDDCDocDetails(SerMode, bytesObjS3G_ORG_PRDDCMasterDataTable);
                bytesDataTable = ClsPubSerialize.Serialize(dsPRDDC, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Getting PRDDC :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            finally
            {
                dsPRDDC.Dispose();
                dsPRDDC = null;
            }
            return bytesDataTable;

        }

        public byte[] FunPubTAQueryPRDDCMaster(SerializationMode SerMode, byte[] bytesObjPRDDCMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable dtPRDDCMaster;
                dtPRDDCMaster = ObjTAPRDDCMaster.FunPubQueryPRDDCMaster(SerMode, bytesObjPRDDCMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtPRDDCMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        public byte[] FunPubTAQueryPRDDCMasterCombi(SerializationMode SerMode, byte[] bytesObjPRDDCMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable dtPRDDCMaster;
                dtPRDDCMaster = ObjTAPRDDCMaster.FunPubQueryPRDDCMasterCombi(SerMode, bytesObjPRDDCMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtPRDDCMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        //public int FunPubExitsPRDDTrans(SerializationMode SerMode, byte[] bytesObjPRDDTExitsDataTable_SERLAY)
        //{
        //    try
        //    {
        //        S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster ObjDeletePRDDTMaster = new S3GDALayer.Origination.PRDDCMgtServices.ClsPubPRDDTMaster();
        //        intResult = ObjDeletePRDDTMaster.FunPubExitsPRDDTrans(SerMode, bytesObjPRDDTExitsDataTable_SERLAY);
        //    }
        //    catch (Exception objExp)
        //    {
        //        ClsPubFaultException objFault = new ClsPubFaultException();
        //        objFault.ProReasonRW = "Erro in Cashflow Delete :" + objExp.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        //    }
        //    return intResult;
        //}

        #endregion

        #region Trade Advance PRDD Transaction
        
        public byte[] FunPubGetTAPRDDTLookUp(SerializationMode SerMode, byte[] bytesObjPRDDTLookDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDTLookUpDataTable dtPRDDTLookDetails;
                S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDTMaster ObjPRDDTLookDetailsDetails = new S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDTMaster();
                dtPRDDTLookDetails = ObjPRDDTLookDetailsDetails.FunPubPRDDTLookup(SerMode, bytesObjPRDDTLookDataTable_SERLAY);
                byte[] bytesPRDDTLookDetails = ClsPubSerialize.Serialize(dtPRDDTLookDetails, SerializationMode.Binary);
                return bytesPRDDTLookDetails;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in PRDDT Look Up Type :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubGetTATypeTrans(SerializationMode SerMode, byte[] bytesObjTypeTransDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable dtTypeTransDetails;
                S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDTMaster ObjTypeTransDetailsDetails = new S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDTMaster();
                dtTypeTransDetails = ObjTypeTransDetailsDetails.FunPubGetTypeTrans(SerMode, bytesObjTypeTransDataTable_SERLAY);
                byte[] bytesTypeTransDetails = ClsPubSerialize.Serialize(dtTypeTransDetails, SerializationMode.Binary);
                return bytesTypeTransDetails;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Type Trans Look Up :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubGetTAPRDDTrans(SerializationMode SerMode, byte[] bytesObjPRDDTransDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable dtPRDDTransDetails;
                S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDTMaster ObjPRDDTransDetailsDetails = new S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDTMaster();
                dtPRDDTransDetails = ObjPRDDTransDetailsDetails.FunPubGetPRDDTrans(SerMode, bytesObjPRDDTransDataTable_SERLAY);
                byte[] bytesPRDDTransDetails = ClsPubSerialize.Serialize(dtPRDDTransDetails, SerializationMode.Binary);
                return bytesPRDDTransDetails;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Type Trans Look Up :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubGetTAPRDDTransDoc(SerializationMode SerMode, byte[] bytesObjPRDDTransDocDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable dtPRDDTransDocDetails;
                S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDTMaster ObjPRDDTransDocDetailsDetails = new S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDTMaster();
                dtPRDDTransDocDetails = ObjPRDDTransDocDetailsDetails.FunPubGetPRDDTransDoc(SerMode, bytesObjPRDDTransDocDataTable_SERLAY);
                byte[] bytesPRDDTransDocDetails = ClsPubSerialize.Serialize(dtPRDDTransDocDetails, SerializationMode.Binary);
                return bytesPRDDTransDocDetails;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Type Trans Look Up :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubCreateTAPRDDTransMaster(SerializationMode SerMode, byte[] bytesObjPRDDTrans_SERLAY, out string strPRDDTNumber_Out)
        {
            try
            {
                S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDTMaster ObjPRDDTransMaster = new S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDTMaster();
                intResult = ObjPRDDTransMaster.FunPubCreatePRDDTrans(SerMode, bytesObjPRDDTrans_SERLAY, out strPRDDTNumber_Out);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in PRDDTrans Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubModifyTAPRDDTransMaster(SerializationMode SerMode, byte[] bytesObjPRDDTrans_SERLAY)
        {
            try
            {
                S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDTMaster ObjPRDDTransMaster = new S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDTMaster();
                intResult = ObjPRDDTransMaster.FunPubModifyPRDDTrans(SerMode, bytesObjPRDDTrans_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in PRDDTrans Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubExitsTAPRDDTrans(SerializationMode SerMode, byte[] bytesObjPRDDTExitsDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDTMaster ObjDeletePRDDTMaster = new S3GDALayer.TradeAdvance.PRDDCMgtServices.ClsPubPRDDTMaster();
                intResult = ObjDeletePRDDTMaster.FunPubExitsPRDDTrans(SerMode, bytesObjPRDDTExitsDataTable_SERLAY);
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

    }

}
