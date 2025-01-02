#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Account Management Services ( Currency Creation Service Class)
/// Created By			: Kaliraj K
/// Created Date		: 11-May-2010
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

namespace S3GServiceLayer
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "AccountMgtServices" here, you must also update the reference to "AccountMgtServices" in Web.config.
    public class AccountMgtServices : IAccountMgtServices
    {
        int intResult;
        byte[] bytesDataTable;
        #region IAccountMgtServices Members - Currency Master

        /// <summary>
        /// Calling DAL Layer's Create Currency Method (FunPubCreateCurrency) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjCurrencyMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>
        public int FunPubCreateCurrency(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster ObjCurrencyMaster = new S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster();
                return ObjCurrencyMaster.FunPubCreateCurrencyInt(SerMode, bytesObjCurrencyMasterDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Create Currency :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubQueryCurrencyPaging(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            try
            {
                S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable dtCurrencyMaster;
                S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster ObjCurrencyMaster = new S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster();
                dtCurrencyMaster = ObjCurrencyMaster.FunPubQueryCurrencyPaging(SerMode, bytesObjCurrencyMasterDataTable_SERLAY, out intTotalRecords, ObjPaging);
                byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtCurrencyMaster, SerializationMode.Binary);
                return bytesCurrencyMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Currency Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        /// <summary>
        /// Calling DAL Layer's Modify Currency Method (FunPubModifyCurrency) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjCurrencyMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>
        public int FunPubModifyCurrency(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster ObjCurrencyMaster = new S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster();
                return ObjCurrencyMaster.FunPubModifyCurrencyInt(SerMode, bytesObjCurrencyMasterDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Modify Currency :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        /// <summary>
        /// Calling DAL Layer's Delete Currency Method (FunPubDeleteCurrency) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjCurrencyMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>

        public int FunPubDeleteCurrency(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster ObjCurrencyMaster = new S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster();
                return ObjCurrencyMaster.FunPubDeleteCurrencyInt(SerMode, bytesObjCurrencyMasterDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Delete Currency:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjCurrencyMasterDataTable_SERLAY"></param>
        /// <returns></returns>
        public byte[] FunPubQueryCurrency(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable dtCurrencyMaster;
                S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster ObjCurrencyMaster = new S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster();
                dtCurrencyMaster = ObjCurrencyMaster.FunPubQueryCurrencyMasterList(SerMode, bytesObjCurrencyMasterDataTable_SERLAY);
                byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtCurrencyMaster, SerializationMode.Binary);
                return bytesCurrencyMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Currency Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        /// <summary>
        /// this method return all currencies except the default currency of the company
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjCurrencyMasterDataTable_SERLAY"></param>
        /// <returns></returns>
        public byte[] FunPubQueryExchangeRateCurrency(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable dtCurrencyMaster;
                S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster ObjCurrencyMaster = new S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster();
                dtCurrencyMaster = ObjCurrencyMaster.FunPubQueryCurrencyList(SerMode, bytesObjCurrencyMasterDataTable_SERLAY);
                byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtCurrencyMaster, SerializationMode.Binary);
                return bytesCurrencyMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Currency Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion
        #region AccountSetup
        /// <summary>
        /// get cashflow list details
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjCurrencyMasterDataTable_SERLAY"></param>
        /// <returns></returns>        
        public byte[] FunPubAcctDesc(SerializationMode SerMode, byte[] bytesObjAcctDescDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.AccountMgtServices.S3G_SYSAD_Account_Code_DetailsDataTable dtAcctDesc;
                S3GDALayer.AccountMgtServices.ClsPubAccountSetup ObjAcctDesc = new S3GDALayer.AccountMgtServices.ClsPubAccountSetup();
                dtAcctDesc = ObjAcctDesc.FunPubGetAccountDesc(SerMode, bytesObjAcctDescDataTable_SERLAY);
                byte[] bytesAcctDesc = ClsPubSerialize.Serialize(dtAcctDesc, SerializationMode.Binary);
                return bytesAcctDesc;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Account Desc :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubCreateAcctDesc(SerializationMode SerMode, byte[] bytesObjAcctDescDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.AccountMgtServices.ClsPubAccountSetup ObjAcctDescMaster = new S3GDALayer.AccountMgtServices.ClsPubAccountSetup();
                intResult = ObjAcctDescMaster.FunPubCreateAcctDesc(SerMode, bytesObjAcctDescDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Accoutsetup :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public byte[] FunPubGetAccountDetails(SerializationMode SerMode, byte[] bytesObjAccountDetailsDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable dtAccountDetails;
                S3GDALayer.AccountMgtServices.ClsPubAccountSetup ObjAccountDetails = new S3GDALayer.AccountMgtServices.ClsPubAccountSetup();
                dtAccountDetails = ObjAccountDetails.FunPubGetAccountDetails(SerMode, bytesObjAccountDetailsDataTable_SERLAY);
                byte[] bytesAccountDetails = ClsPubSerialize.Serialize(dtAccountDetails, SerializationMode.Binary);
                return bytesAccountDetails;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Cashflow List :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubCreateAcctSetup(SerializationMode SerMode, byte[] bytesObjAcctSetupDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.AccountMgtServices.ClsPubAccountSetup ObjAcctSetupMaster = new S3GDALayer.AccountMgtServices.ClsPubAccountSetup();
                intResult = ObjAcctSetupMaster.FunPubCreateAcctDetails(SerMode, bytesObjAcctSetupDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Company Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public byte[] FunPubGetAccountDetailsPaging(SerializationMode SerMode, byte[] bytesObjAccountDetailsDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            S3GBusEntity.AccountMgtServices.S3G_SYSAD_AccountDetailsDataTable dtAccountDetails;
            S3GDALayer.AccountMgtServices.ClsPubAccountSetup ObjAccountDetails = new S3GDALayer.AccountMgtServices.ClsPubAccountSetup();
            dtAccountDetails = ObjAccountDetails.FunPubGetAccountDetails(SerMode, bytesObjAccountDetailsDataTable_SERLAY, out intTotalRecords, ObjPaging);
            byte[] bytesAccountDetails = ClsPubSerialize.Serialize(dtAccountDetails, SerializationMode.Binary);
            return bytesAccountDetails;
        }

        public int FunPubModifyAcctSetup(SerializationMode SerMode, byte[] bytesObjAcctSetupModifyDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.AccountMgtServices.ClsPubAccountSetup ObjAcctSetupModifyMaster = new S3GDALayer.AccountMgtServices.ClsPubAccountSetup();
                intResult = ObjAcctSetupModifyMaster.FunPubModifyAcctDetails(SerMode, bytesObjAcctSetupModifyDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Company Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }




        #endregion
        #region ExchangeRate
        public int FunPubCreateExchangeRate(SerializationMode SerMode, byte[] bytesObjAcctDescDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.AccountMgtServices.ClsPubExchangeRateMaster ObjAcctDescMaster = new S3GDALayer.AccountMgtServices.ClsPubExchangeRateMaster();
                intResult = ObjAcctDescMaster.FunPubCreateExchangeMasterInt(SerMode, bytesObjAcctDescDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Company Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }
        public int FunPubModifyExchangeRate(SerializationMode SerMode, byte[] bytesObjAcctDescDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.AccountMgtServices.ClsPubExchangeRateMaster ObjAcctDescMaster = new S3GDALayer.AccountMgtServices.ClsPubExchangeRateMaster();
                intResult = ObjAcctDescMaster.FunPubModifyExchangeMasterInt(SerMode, bytesObjAcctDescDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Company Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }
        public byte[] FunPubQueryExchangeRate(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY)
        {
            try
            {
                /* 
                S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable dtCurrencyMaster;
                S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster ObjCurrencyMaster = new S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster();
                dtCurrencyMaster = ObjCurrencyMaster.FunPubQueryCurrencyMasterList(SerMode, bytesObjCurrencyMasterDataTable_SERLAY);
                byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtCurrencyMaster, SerializationMode.Binary);
                return bytesCurrencyMaster;
                */
                S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable dtCurrencyMaster;
                S3GDALayer.AccountMgtServices.ClsPubExchangeRateMaster ObjCurrencyMaster = new S3GDALayer.AccountMgtServices.ClsPubExchangeRateMaster();
                dtCurrencyMaster = ObjCurrencyMaster.FunPubQueryExchangeRateDetails(SerMode, bytesObjCurrencyMasterDataTable_SERLAY);
                byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtCurrencyMaster, SerializationMode.Binary);
                return bytesCurrencyMaster;


                // intResult = ObjAcctDescMaster.FunPubCreateExchangeMasterInt(SerMode, bytesObjAcctDescDataTable_SERLAY);


            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Currency Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubQueryExchangeRatePaging(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            try
            {
                /* 
                S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable dtCurrencyMaster;
                S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster ObjCurrencyMaster = new S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster();
                dtCurrencyMaster = ObjCurrencyMaster.FunPubQueryCurrencyMasterList(SerMode, bytesObjCurrencyMasterDataTable_SERLAY);
                byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtCurrencyMaster, SerializationMode.Binary);
                return bytesCurrencyMaster;
                */
                S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable dtCurrencyMaster;
                S3GDALayer.AccountMgtServices.ClsPubExchangeRateMaster ObjCurrencyMaster = new S3GDALayer.AccountMgtServices.ClsPubExchangeRateMaster();
                dtCurrencyMaster = ObjCurrencyMaster.FunPubQueryExchangeRatePaging(SerMode, bytesObjCurrencyMasterDataTable_SERLAY, out intTotalRecords, ObjPaging);
                byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtCurrencyMaster, SerializationMode.Binary);
                return bytesCurrencyMaster;


                // intResult = ObjAcctDescMaster.FunPubCreateExchangeMasterInt(SerMode, bytesObjAcctDescDataTable_SERLAY);


            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Currency Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region TaxGuide

        /// <summary>
        /// Calling DAL Layer's Create TaxGuide Method (FunPubCreateTaxGuide) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjTaxGuideMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>    
        public int FunPubCreateTaxGuide(SerializationMode SerMode, byte[] bytesObjTaxGuideMasterDataTable_SERLAY, out string Tax_Code)
        {
            try
            {
                S3GDALayer.AccountMgtServices.ClsPubTaxGuide ObjTaxGuideMaster = new S3GDALayer.AccountMgtServices.ClsPubTaxGuide();
                intResult = ObjTaxGuideMaster.FunPubCreateTaxGuide(SerMode, bytesObjTaxGuideMasterDataTable_SERLAY, out Tax_Code);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;

        }

        /// <summary>
        /// Calling DAL Layer's Query TaxGuide Method (FunPubQueryTaxGuideDetails) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjTaxGuideMasterDataTable_SERLAY"></param>
        /// <returns></returns>
        public byte[] FunPubQueryTaxGuide(SerializationMode SerMode, byte[] bytesObjTaxGuideMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable dtTaxGuideMaster;
                S3GDALayer.AccountMgtServices.ClsPubTaxGuide ObjTaxGuideMaster = new S3GDALayer.AccountMgtServices.ClsPubTaxGuide();
                dtTaxGuideMaster = ObjTaxGuideMaster.FunPubQueryTaxGuide(SerMode, bytesObjTaxGuideMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtTaxGuideMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        public byte[] FunPubQueryTaxGuidePaging(SerializationMode SerMode, byte[] bytesObjTaxGuideMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            intTotalRecords = 0;
            try
            {
                S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable dtTaxGuideMaster;
                S3GDALayer.AccountMgtServices.ClsPubTaxGuide ObjTaxGuideMaster = new S3GDALayer.AccountMgtServices.ClsPubTaxGuide();
                dtTaxGuideMaster = ObjTaxGuideMaster.FunPubQueryTaxGuidePaging(SerMode, bytesObjTaxGuideMasterDataTable_SERLAY, out intTotalRecords, ObjPaging);
                bytesDataTable = ClsPubSerialize.Serialize(dtTaxGuideMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }



        public byte[] FunPubCreateTaxGuideAssetHistoryPaging(SerializationMode SerMode, byte[] bytesObjTaxGuideMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            intTotalRecords = 0;
            try
            {
                S3GBusEntity.AccountMgtServices.S3G_SYSAD_TaxGuideDataTable dtTaxGuideMaster;
                S3GDALayer.AccountMgtServices.ClsPubTaxGuide ObjTaxGuideMaster = new S3GDALayer.AccountMgtServices.ClsPubTaxGuide();
                dtTaxGuideMaster = ObjTaxGuideMaster.FunPubCreateTaxGuideAssetHistoryPaging(SerMode, bytesObjTaxGuideMasterDataTable_SERLAY, out intTotalRecords, ObjPaging);
                bytesDataTable = ClsPubSerialize.Serialize(dtTaxGuideMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        #endregion


        //#region CSPMASTER
        //public int FunPubCreateCSPMaster(SerializationMode SerMode, byte[] bytesObjCSPMasterDataTable_SERLAY)
        //{
        //    try
        //    {
        //        S3GDALayer.AccountMgtServices.ClsPubCSPMaster objCSPMaster = new S3GDALayer.AccountMgtServices.ClsPubCSPMaster();

        //        intResult = objCSPMaster.FunPubCreateCSPMasterInt(SerMode, bytesObjCSPMasterDataTable_SERLAY);
        //    }
        //    catch (Exception objExp)
        //    {
        //        ClsPubFaultException objFault = new ClsPubFaultException();
        //        objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        //    }
        //    return intResult;

        //}

        //public int FunPubModifyCSPMaster(SerializationMode SerMode, byte[] bytesObjCSPMasterDataTable_SERLAY)
        //{
        //    try
        //    {
        //        S3GDALayer.AccountMgtServices.ClsPubCSPMaster objCSPMaster = new S3GDALayer.AccountMgtServices.ClsPubCSPMaster();
        //        intResult = objCSPMaster.FunPubModifyCSPMasterInt(SerMode, bytesObjCSPMasterDataTable_SERLAY);
        //    }
        //    catch (Exception objExp)
        //    {
        //        ClsPubFaultException objFault = new ClsPubFaultException();
        //        objFault.ProReasonRW = "Erro in :" + objExp.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        //    }
        //    return intResult;
        //}

        ////public byte[] FunPubQueryCSPMaster(SerializationMode SerMode, byte[] bytesObjCSPMasterDataTable_SERLAY)
        ////{
        ////    try
        ////    {
        ////        /* 
        ////        S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable dtCurrencyMaster;
        ////        S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster ObjCurrencyMaster = new S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster();
        ////        dtCurrencyMaster = ObjCurrencyMaster.FunPubQueryCurrencyMasterList(SerMode, bytesObjCurrencyMasterDataTable_SERLAY);
        ////        byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtCurrencyMaster, SerializationMode.Binary);
        ////        return bytesCurrencyMaster;
        ////        */
        ////        S3GBusEntity.AccountMgtServices.S3G_SYSAD_CSPMasterDataTable dtCSPMaster;
        ////        S3GDALayer.AccountMgtServices.ClsPubCSPMaster ObjCSPMasterDetails = new S3GDALayer.AccountMgtServices.ClsPubCSPMaster();
        ////        dtCSPMaster = ObjCSPMasterDetails.FunPubQueryCSPDetails(SerMode, bytesObjCSPMasterDataTable_SERLAY);
        ////        byte[] bytesCSPMaster = ClsPubSerialize.Serialize(dtCSPMaster, SerializationMode.Binary);
        ////        return bytesCSPMaster;


        ////        // intResult = ObjAcctDescMaster.FunPubCreateExchangeMasterInt(SerMode, bytesObjAcctDescDataTable_SERLAY);


        ////    }
        ////    catch (Exception objExp)
        ////    {
        ////        ClsPubFaultException objFault = new ClsPubFaultException();
        ////        objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
        ////        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        ////    }
        ////}

        ////public byte[] FunPubQueryCSPMasterPaging(SerializationMode SerMode, byte[] bytesObjCSPMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        ////{
        ////    try
        ////    {
        ////        /* 
        ////        S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable dtCurrencyMaster;
        ////        S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster ObjCurrencyMaster = new S3GDALayer.AccountMgtServices.ClsPubCurrencyMaster();
        ////        dtCurrencyMaster = ObjCurrencyMaster.FunPubQueryCurrencyMasterList(SerMode, bytesObjCurrencyMasterDataTable_SERLAY);
        ////        byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtCurrencyMaster, SerializationMode.Binary);
        ////        return bytesCurrencyMaster;
        ////        */
        ////        S3GBusEntity.AccountMgtServices.S3G_SYSAD_CSPMasterDataTable dtCSPMaster;
        ////        S3GDALayer.AccountMgtServices.ClsPubCSPMaster ObjCSPMasterDetails = new S3GDALayer.AccountMgtServices.ClsPubCSPMaster();
        ////        dtCSPMaster = ObjCSPMasterDetails.FunPubQueryCSPMasterPaging(SerMode, bytesObjCSPMasterDataTable_SERLAY, out intTotalRecords, ObjPaging);
        ////        byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtCSPMaster, SerializationMode.Binary);
        ////        return bytesCurrencyMaster;


        ////        // intResult = ObjAcctDescMaster.FunPubCreateExchangeMasterInt(SerMode, bytesObjAcctDescDataTable_SERLAY);


        ////    }
        ////    catch (Exception objExp)
        ////    {
        ////        ClsPubFaultException objFault = new ClsPubFaultException();
        ////        objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
        ////        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        ////    }
        ////}

        //#endregion
    }
}
