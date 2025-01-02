#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Asset Master
/// Created By			: Nataraj Y
/// Created Date		: 01-Jun-2010
/// Purpose	            : 
/// Modified By         : Nataraj Y
/// Modified Date       : 02-Jun-2010
/// <Program Summary>
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using System.Data;


namespace S3GServiceLayer.Origination
{
    // NOTE: If you change the interface name "IOrgMasterMgtServices" here, you must also update the reference to "IOrgMasterMgtServices" in Web.config.
    [ServiceContract]
    public interface IOrgMasterMgtServices
    {
        #region Target Master
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateTargetMaster(SerializationMode SerMode, byte[] bytesObjTargetMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyTargetMaster(SerializationMode SerMode, byte[] bytesObjTargetMasterDataTable_SERLAY);
        #endregion
        #region Asset Master
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAssetCategoryInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_AssetCategoryDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAssetCodeHL(SerializationMode SerMode, byte[] bytesObjS3G_ORG_AssetMasterDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryAssetCategoryDetails(int intCompanyId, int? intAsset_CatId, int? intAssetType_Id);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAssetCodeInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_AssetMasterDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryAssetDetails(int intCompany_Id, int? intAsset_id, int? intAssetType_Id);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        string FunPubGetLastAssetCategoryCode(int intCompany_Id, string strCategoryCode, int intAssetType_Id);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyAssetCodeInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_AssetMasterDataTable);

        #endregion

        #region Constitution Master
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateConstitutionCode(SerializationMode SerMode, byte[] bytesObjS3G_ORG_ConstitutionMasterDataTable, out string ConstitutionCode);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryConstitutionDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_ConstitutionMasterDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryConstitutionMaster(SerializationMode SerMode, byte[] bytesObjConstitutionMasterDataTable_SERLAY);

        #endregion

        #region EntityMaster
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateEntityInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_Enityt_MasterDataTable, out string strEntityCode);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyEntityInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_Enityt_MasterDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryEntityDetails(int? intCompany_Id, int? intEntity_id, out bool blnTranExists);
        #endregion



        #region TDSMaster
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateTDSInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_TDS_MasterDataTable, out string strTDSCode);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyTDSInt(SerializationMode SerMode, byte[] bytesObjS3G_ORG_TDS_MasterDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryTDSDetails(int? intCompany_Id, int? intTDS_id, out bool blnTranExists);
        #endregion



        #region GLOBAL PARAMETER SETUP

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetGlobalLook(SerializationMode SerMode, byte[] bytesObjGlobalLookDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetGlobalProgram(SerializationMode SerMode, byte[] bytesObjGlobalProgramDataTable_SERLAY);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //int FunPubCreateGlobalParameter(SerializationMode SerMode, byte[] bytesObjGlobalParameterDataTable_SERLAY, byte[] bytesObjGlobalParameterROIDataTable_SERLAY, byte[] bytesObjGlobalParameterLOBDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateGlobalParameter(SerializationMode SerMode, byte[] bytesObjGlobalParameterDataTable_SERLAY);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //int FunPubUpdateGlobalParameter(SerializationMode SerMode, byte[] bytesObjGlobalParameterDataTable_SERLAY, byte[] bytesObjGlobalParameterROIDataTable_SERLAY, byte[] bytesObjGlobalParameterLOBDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateGlobalParameter(SerializationMode SerMode, byte[] bytesObjGlobalParameterDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetGlobalProgramLOB(SerializationMode SerMode, byte[] bytesObjGlobalProgramLOBDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetGlobalMaster(SerializationMode SerMode, byte[] bytesObjGlobalProgramMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetGlobalLOB(SerializationMode SerMode, byte[] bytesObjGlobalLOBMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetGlobalROI(SerializationMode SerMode, byte[] bytesObjGlobalROIMasterDataTable_SERLAY);
        #endregion

        #region CustomerMaster
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCustomerInt(CustomerMasterBusEntity objCustomerMasterEntity, out string strCustomerCodeOut);




        #endregion

        #region Common Method
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        DataTable FunPub_GetS3GStatusLookUp(S3G_Status_Parameters ObjParam);
        #endregion

        #region "Bank Statement Data Capture"

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateBankStatemntListValues(SerializationMode SerMode, byte[] bytesObjS3G_BankStatemnt_LOVDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateBankStatemntData(SerializationMode SerMode, byte[] bytesObjS3G_BankStatemnt_CaptureDataTable, int intUpdateBank_ID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryBankStatemntCopyListValues(int Bank_ID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetBankStatemnt_ID();

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryBankStatemntCaptureData(int CompanyID, int CreatedBy);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubDeleteBankStatemntListvalues(SerializationMode SerMode, byte[] bytesObjS3G_BankStatemnt_LOVDataTable);

        #endregion

        #region Gold Loan Matrix
        //Added by Thangam M on 28/Jul/2012
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateGoldLoanMatrix(SerializationMode SerMode, byte[] bytesObjS3G_Origination_GoldLoanMatrix_DataTable);
        #endregion

        #region "MRA"

        #region Create

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateOrModifyMRACreation(SerializationMode SerMode, byte[] bytesObjMRA_DataTable, out string strMRANumber, out int intMRA_No);

        #endregion

        #region Approval

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertMRAApproval(SerializationMode SerMode, byte[] bytesObjMRAApproval_DataTable, out string strMRAApprovalNumber, out Int64 intMRAApproval_No);

        #endregion

        #endregion

        #region Tax Exemption Master

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateTaxExemption(SerializationMode SerMode, byte[] bytesObjTaxExemptionMasterDataTable_SERLAY, out string Tax_Code, out decimal Balance_Exe_Amount);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryTaxExemption(SerializationMode SerMode, byte[] bytesObjTaxExemptionMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryTaxExemptionPaging(SerializationMode SerMode, byte[] bytesObjTaxExemptionMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryTaxExemptionHistoryPaging(SerializationMode SerMode, byte[] bytesObjTaxExemptionMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);

        #endregion

        #region HSN Master
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunHSNMasterInsertInt(SerializationMode SerMode, byte[] bytesObjHSNMasterDatatable_SERLAY);
        #endregion

    }
}
