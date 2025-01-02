#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Company Creation DAL Class
/// Created By			: Nataraj Y
/// Created Date		: 10-May-2010
/// Purpose	            : 

/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 10-May-2010
/// Reason              : System Admin Company Module Developement

/// Last Updated By		: Suresh P
/// Last Updated Date   : 10-May-2010
/// Reason              : System Admin LOB Module Developement

/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 17-May-2010
/// Reason              : Global Parameter Setup
/// 
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
    // NOTE: If you change the class name "CompanyMgtServices" here, you must also update the reference to "CompanyMgtServices" in Web.config.
    public class CompanyMgtServices : ICompanyMgtServices
    {
        int intResult;
        byte[] bytesDataTable;

        #region ICompanyMgtServices_Company
        /// <summary>
        /// Calling DAL Layer's Create Company Method (FunPubCreateCompanyInt) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjCompanyMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>
        public int FunPubCreateCompany(SerializationMode SerMode, byte[] bytesObjCompanyMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubCompanyMaster ObjCompanyMaster = new S3GDALayer.CompanyMgtServices.ClsPubCompanyMaster();
                intResult = ObjCompanyMaster.FunPubCreateCompanyInt(SerMode, bytesObjCompanyMasterDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Company Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }


        /// <summary>
        /// Calling DAL Layer's Modify Company Method (FunPubModifyCompanyInt) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjCompanyMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>
        public int FunPubModifyCompany(SerializationMode SerMode, byte[] bytesObjCompanyMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubCompanyMaster ObjCompanyMaster = new S3GDALayer.CompanyMgtServices.ClsPubCompanyMaster();
                intResult = ObjCompanyMaster.FunPubModifyCompanyInt(SerMode, bytesObjCompanyMasterDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Company Modification :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }


        /// <summary>
        /// Calling DAL Layer's Delete Company Method (FunPubDeleteCompanyInt) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjCompanyMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>
        public int FunPubDeleteCompany(SerializationMode SerMode, byte[] bytesObjCompanyMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubCompanyMaster ObjCompanyMaster = new S3GDALayer.CompanyMgtServices.ClsPubCompanyMaster();
                intResult = ObjCompanyMaster.FunPubDeleteCompanyInt(SerMode, bytesObjCompanyMasterDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Company Deletion :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjCompanyMasterDataTable_SERLAY"></param>
        /// <returns></returns>
        public byte[] FunPubQueryCompany(SerializationMode SerMode, byte[] bytesObjCompanyMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable dtCompanyMaster;
                S3GDALayer.CompanyMgtServices.ClsPubCompanyMaster ObjCompanyMaster = new S3GDALayer.CompanyMgtServices.ClsPubCompanyMaster();
                dtCompanyMaster = ObjCompanyMaster.FunPubQueryCompanyMasterList(SerMode, bytesObjCompanyMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtCompanyMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Company Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }
        /// <summary>
        /// To Get Company List
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public byte[] FunPubGetCompany_List(int CompanyId)
        {
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMasterDataTable dtCompanyMaster_list;
                S3GDALayer.CompanyMgtServices.ClsPubCompanyMaster ObjCompanyMaster = new S3GDALayer.CompanyMgtServices.ClsPubCompanyMaster();
                dtCompanyMaster_list = ObjCompanyMaster.FunPubGetCompanyList(CompanyId);
                bytesDataTable = ClsPubSerialize.Serialize(dtCompanyMaster_list, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Company Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }
        #endregion

        #region ICompanyMgtServices_Product
        public int FunPubCreateProduct(SerializationMode SerMode, byte[] bytesObjProductMasterDatatable_SERLAY)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubProductMaster objProductMAster = new S3GDALayer.CompanyMgtServices.ClsPubProductMaster();
                return objProductMAster.FunProductMasterInsertInt(SerMode, bytesObjProductMasterDatatable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Product Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public int FunPubModifyProduct(SerializationMode SerMode, byte[] bytesObjProductMasterDatatable_SERLAY)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubProductMaster objProductMAster = new S3GDALayer.CompanyMgtServices.ClsPubProductMaster();
                return objProductMAster.FunProductMasterUpdateInt(SerMode, bytesObjProductMasterDatatable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Product Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubDeleteProduct(SerializationMode SerMode, byte[] bytesObjProductMasterDatatable_SERLAY)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubProductMaster objProductMAster = new S3GDALayer.CompanyMgtServices.ClsPubProductMaster();
                return objProductMAster.FunProductMasterDeleteInt(SerMode, bytesObjProductMasterDatatable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Product Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubQueryProduct(SerializationMode SerMode, byte[] bytesObjProductMasterDatatable_SERLAY)
        {
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable dtProductMasterview;
                S3GDALayer.CompanyMgtServices.ClsPubProductMaster ObjProductMaster = new S3GDALayer.CompanyMgtServices.ClsPubProductMaster();
                dtProductMasterview = ObjProductMaster.FunPubQueryProductMasterList(SerMode, bytesObjProductMasterDatatable_SERLAY);
                byte[] bytesObjProductMaster = ClsPubSerialize.Serialize(dtProductMasterview, SerializationMode.Binary);
                return bytesObjProductMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Product Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunPubQueryProductPaging(SerializationMode SerMode, byte[] bytesObjProductMasterDatatable_SERLAYint, out int intTotalRecords, PagingValues ObjPaging)
        {
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable dtProductMasterview;
                S3GDALayer.CompanyMgtServices.ClsPubProductMaster ObjProductMaster = new S3GDALayer.CompanyMgtServices.ClsPubProductMaster();
                dtProductMasterview = ObjProductMaster.FunPubQueryProductPaging(SerMode, bytesObjProductMasterDatatable_SERLAYint, out intTotalRecords, ObjPaging);
                byte[] bytesObjProductMaster = ClsPubSerialize.Serialize(dtProductMasterview, SerializationMode.Binary);
                return bytesObjProductMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Product Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region ICompanyMgtServices_LOB

        /// <summary>
        /// Calling DAL Layer's Create LOB Method (FunPubCreateLOBInt) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjLOBMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>    
        public int FunPubCreateLOB(SerializationMode SerMode, byte[] bytesObjLOBMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubLOBMaster ObjLOBMaster = new S3GDALayer.CompanyMgtServices.ClsPubLOBMaster();
                intResult = ObjLOBMaster.FunPubCreateLOBInt(SerMode, bytesObjLOBMasterDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in LOB Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;

        }

        /// <summary>
        /// Calling DAL Layer's Modify LOB Method (FunPubModifyLOBInt) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjLOBMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>   
        public int FunPubModifyLOB(SerializationMode SerMode, byte[] bytesObjLOBMasterDataTable_SERLAY)
        {
            try
            {

                S3GDALayer.CompanyMgtServices.ClsPubLOBMaster ObjLOBMaster = new S3GDALayer.CompanyMgtServices.ClsPubLOBMaster();
                intResult = ObjLOBMaster.FunPubModifyLOBInt(SerMode, bytesObjLOBMasterDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in LOB Modification :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        /// <summary>
        /// Calling DAL Layer's Query LOB Method (FunPubQueryLOBDetails) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjLOBMasterDataTable_SERLAY"></param>
        /// <returns></returns>
        public byte[] FunPubQueryLOB(SerializationMode SerMode, byte[] bytesObjLOBMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable dtLOBMaster;
                S3GDALayer.CompanyMgtServices.ClsPubLOBMaster ObjLOBMaster = new S3GDALayer.CompanyMgtServices.ClsPubLOBMaster();
                dtLOBMaster = ObjLOBMaster.FunPubQueryLOBDetails(SerMode, bytesObjLOBMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtLOBMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in LOB Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        /// <summary>
        /// Calling DAL Layer's Query LOB Method for Bind DropdownList(FunPubQueryLOBList) 
        /// with Ser Mode and Bytes as Input       
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjLOBMasterDataTable_SERLAY"></param>
        /// <returns></returns>
        public byte[] FunPubQueryLOB_LIST(SerializationMode SerMode, byte[] bytesObjLOBMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable dtLOBMaster;
                S3GDALayer.CompanyMgtServices.ClsPubLOBMaster ObjLOBMaster = new S3GDALayer.CompanyMgtServices.ClsPubLOBMaster();
                dtLOBMaster = ObjLOBMaster.FunPubQueryLOBList(SerMode, bytesObjLOBMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtLOBMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in LOB Querying for Bind DropdownList:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        /// <summary>
        /// Calling DAL Layer's Query LOB Method for Bind DropdownList(FunPubQueryLOBList) 
        /// with Ser Mode and Bytes as Input       
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjLOBMasterDataTable_SERLAY"></param>
        /// <returns></returns>
        public byte[] FunPubQueryLOBPaging(SerializationMode SerMode, byte[] bytesObjLOBMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBPagingMasterDataTable dtLOBMaster;
                S3GDALayer.CompanyMgtServices.ClsPubLOBMaster ObjLOBMaster = new S3GDALayer.CompanyMgtServices.ClsPubLOBMaster();
                dtLOBMaster = ObjLOBMaster.FunPubQueryLOBPaging(SerMode, bytesObjLOBMasterDataTable_SERLAY, out intTotalRecords, ObjPaging);
                bytesDataTable = ClsPubSerialize.Serialize(dtLOBMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in LOB  Paging for Bind Grid:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        #endregion

        #region ICompanyMgtServices_Region
        /// <summary>
        /// Calling DAL Layer's Create Region Method (FunPubCreateRegionInt) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjCompanyMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>

        public int FunPubCreateRegion(SerializationMode SerMode, byte[] bytesObjRegionMasterDataTable_SERLAY)
        {
            S3GDALayer.CompanyMgtServices.ClsPubRegionMaster objRegionMaster = new S3GDALayer.CompanyMgtServices.ClsPubRegionMaster();
            return objRegionMaster.FunPubCreateRegionInt(SerMode, bytesObjRegionMasterDataTable_SERLAY);
        }


        /// <summary>
        /// Calling DAL Layer's Modify Region Method (FunPubModifyRegionInt) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjCompanyMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>
        public int FunPubModifyRegion(SerializationMode SerMode, byte[] bytesObjRegionMasterDataTable_SERLAY)
        {
            S3GDALayer.CompanyMgtServices.ClsPubRegionMaster ObjRegionMaster = new S3GDALayer.CompanyMgtServices.ClsPubRegionMaster();
            return ObjRegionMaster.FunPubModifyRegionInt(SerMode, bytesObjRegionMasterDataTable_SERLAY);
        }

        /// <summary>
        /// Calling DAL Layer's Get Region Method (FunPubQueryRegionInt) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjCompanyMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>        
        public byte[] FunPubQueryRegion(SerializationMode SerMode, byte[] bytesObjRegionMasterDatatable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            byte[] bytesObjRegionMaster;
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable dtRegionMasterview;
                S3GDALayer.CompanyMgtServices.ClsPubRegionMaster ObjRegionMaster = new S3GDALayer.CompanyMgtServices.ClsPubRegionMaster();
                dtRegionMasterview = ObjRegionMaster.FunPubQueryRegionInt(SerMode, bytesObjRegionMasterDatatable_SERLAY, out intTotalRecords, ObjPaging);
                bytesObjRegionMaster = ClsPubSerialize.Serialize(dtRegionMasterview, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Region Master for Bind Grid:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesObjRegionMaster;
        }

        public byte[] FunPubQueryRegion_View(SerializationMode SerMode, byte[] bytesObjRegionMasterDatatable_SERLAY)
        {
            byte[] bytesObjRegionMaster;
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable dtRegionMasterview;
                S3GDALayer.CompanyMgtServices.ClsPubRegionMaster ObjRegionMaster = new S3GDALayer.CompanyMgtServices.ClsPubRegionMaster();
                dtRegionMasterview = ObjRegionMaster.FunPubQueryRegionInt_View(SerMode, bytesObjRegionMasterDatatable_SERLAY);
                bytesObjRegionMaster = ClsPubSerialize.Serialize(dtRegionMasterview, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Region Master for Bind Grid:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesObjRegionMaster;
        }
       
        #endregion

        #region ICompanuMgtServices_Branch
        /// <summary>
        /// Calling DAL Layer's Create Branch Method (FunPubCreateBranchInt) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjCompanyMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>

        public int FunPubCreateBranch(SerializationMode SerMode, byte[] bytesObjBranchMasterDataTable_SERLAY)
        {
            S3GDALayer.CompanyMgtServices.ClsPubBranchMaster objBranchMaster = new S3GDALayer.CompanyMgtServices.ClsPubBranchMaster();
            return objBranchMaster.FunPubCreateBranchInt(SerMode, bytesObjBranchMasterDataTable_SERLAY);
        }
        public int FunPubModifyBranch(SerializationMode SerMode, byte[] bytesObjBranchMasterDataTable_SERLAY)
        {
            S3GDALayer.CompanyMgtServices.ClsPubBranchMaster ObjBranchMaster = new S3GDALayer.CompanyMgtServices.ClsPubBranchMaster();
            return ObjBranchMaster.FunPubModifyBranchInt(SerMode, bytesObjBranchMasterDataTable_SERLAY);
        }

        public byte[] FunPubQueryBranch(SerializationMode SerMode, byte[] bytesObjBranchMasterDatatable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            byte[] bytesObjBranchMaster;
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable dtBranchMasterview;
                S3GDALayer.CompanyMgtServices.ClsPubBranchMaster ObjBranchMaster = new S3GDALayer.CompanyMgtServices.ClsPubBranchMaster();
                dtBranchMasterview = ObjBranchMaster.FunPubQueryBranchInt(SerMode, bytesObjBranchMasterDatatable_SERLAY, out intTotalRecords, ObjPaging);
                bytesObjBranchMaster = ClsPubSerialize.Serialize(dtBranchMasterview, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Branch Master for Bind Grid:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesObjBranchMaster;
        }


        // services
        public byte[] FunPubQueryBranch_List(SerializationMode SerMode, byte[] bytesObjBranchMasterDatatable_SERLAY)
        {
            byte[] bytesObjBranchMaster;

            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable dtBranchMasterview;
            S3GDALayer.CompanyMgtServices.ClsPubBranchMaster ObjBranchMaster = new S3GDALayer.CompanyMgtServices.ClsPubBranchMaster();
            dtBranchMasterview = ObjBranchMaster.FunPubQueryBranchInt_List(SerMode, bytesObjBranchMasterDatatable_SERLAY);
            bytesObjBranchMaster = ClsPubSerialize.Serialize(dtBranchMasterview, SerializationMode.Binary);
            return bytesObjBranchMaster;
        }


        public byte[] FunPubRegionCode_LIST(SerializationMode SerMode, byte[] bytesObjRegionCodeListDataTable_SERLAY)
        {
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionCodeDataTable dtRegionCodeList;
            S3GDALayer.CompanyMgtServices.ClsPubBranchMaster ObjRegionCodeMaster = new S3GDALayer.CompanyMgtServices.ClsPubBranchMaster();
            dtRegionCodeList = ObjRegionCodeMaster.FunPubRegionCodeInt(SerMode, bytesObjRegionCodeListDataTable_SERLAY);
            byte[] bytesRegionCodeMaster = ClsPubSerialize.Serialize(dtRegionCodeList, SerializationMode.Binary);
            return bytesRegionCodeMaster;
        }

        public byte[] FunPubRegionName(SerializationMode SerMode, byte[] bytesObjRegionNameDataTable_SERLAY)
        {
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_Region_NameDataTable dtRegionName;
            S3GDALayer.CompanyMgtServices.ClsPubBranchMaster ObjRegionNameMaster = new S3GDALayer.CompanyMgtServices.ClsPubBranchMaster();
            dtRegionName = ObjRegionNameMaster.FunPubRegionNameInt(SerMode, bytesObjRegionNameDataTable_SERLAY);
            byte[] bytesRegionNameMaster = ClsPubSerialize.Serialize(dtRegionName, SerializationMode.Binary);
            return bytesRegionNameMaster;
        }

        public byte[] FunPubBranch_List(SerializationMode SerMode, byte[] bytesObjBranchListDataTable_SERLAY)
        {
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable dtBracnhList;
            S3GDALayer.CompanyMgtServices.ClsPubBranchMaster ObjBrachMaster = new S3GDALayer.CompanyMgtServices.ClsPubBranchMaster();
            dtBracnhList = ObjBrachMaster.FunPubBranchList(SerMode, bytesObjBranchListDataTable_SERLAY);
            byte[] bytesBranchMaster = ClsPubSerialize.Serialize(dtBracnhList, SerializationMode.Binary);
            return bytesBranchMaster;
        }
        #endregion

        #region ICompanyMgtServices_GlobalParameterSetup
        /// <summary>
        /// To save Global Param set up by getting to tables byte structure and returning integer
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesobjS3G_SYSAD_GlobalParameterSetupDataTable_SERALY"></param>
        /// <param name="bytesobjS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable_SERALY"></param>
        /// <returns></returns>
        public int FunPubCreateGlobalParamSetUp(SerializationMode SerMode, byte[] bytesobjS3G_SYSAD_GlobalParameterSetupDataTable_SERALY, byte[] bytesobjS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable_SERALY)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubGlobalParameterSetup ObjGlobalSetup = new S3GDALayer.CompanyMgtServices.ClsPubGlobalParameterSetup();
                intResult = ObjGlobalSetup.FunPubCreateGlobalParamInt(SerMode, bytesobjS3G_SYSAD_GlobalParameterSetupDataTable_SERALY, bytesobjS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable_SERALY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Global Parameter Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubModifyGlobalParamSetUp(SerializationMode SerMode, byte[] bytesobjS3G_SYSAD_GlobalParameterSetupDataTable_SERALY, byte[] bytesobjS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable_SERALY)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubGlobalParameterSetup ObjGlobalSetup = new S3GDALayer.CompanyMgtServices.ClsPubGlobalParameterSetup();
                intResult = ObjGlobalSetup.FunPubModifyGlobalParamInt(SerMode, bytesobjS3G_SYSAD_GlobalParameterSetupDataTable_SERALY, bytesobjS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable_SERALY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Global Parameter Modification :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }
        /// <summary>
        /// To get Global Param details with Company id as Input
        /// </summary>
        /// <param name="intCompanyId"></param>
        /// <returns></returns>
        public byte[] FunPubQueryGPSDetails(int intCompanyId, string Year, string Month)
        {
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_GlobalParameterSetupDataTable dtGPSDetails;
                S3GDALayer.CompanyMgtServices.ClsPubGlobalParameterSetup ObjGlobalSetup = new S3GDALayer.CompanyMgtServices.ClsPubGlobalParameterSetup();
                dtGPSDetails = ObjGlobalSetup.FunPubQueryGPSDetails(intCompanyId, Year, Month);
                bytesDataTable = ClsPubSerialize.Serialize(dtGPSDetails, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in GPS Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        public byte[] FunPubQueryGPSDetails_New(int intCompanyId, string Year, string Month)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                
                S3GDALayer.CompanyMgtServices.ClsPubGlobalParameterSetup ObjGlobalSetup = new S3GDALayer.CompanyMgtServices.ClsPubGlobalParameterSetup();
                ds = ObjGlobalSetup.FunPubQueryGPSDetails_New(intCompanyId, Year, Month);
                bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
                
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in GPS Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        #endregion

        #region ICompanyMgtServices_Customer Alert
        public int FunPubCreateCustomerAlert(SerializationMode SerMode, byte[] bytesObjCustomerAlertDatatable_SERLAY)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubCustomerAlert ObjCustomerAlert = new S3GDALayer.CompanyMgtServices.ClsPubCustomerAlert();
                return ObjCustomerAlert.FunCreateCustomerAlert(SerMode, bytesObjCustomerAlertDatatable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Cutomer Alert Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public int FunPubModifyCustomerAlert(SerializationMode SerMode, byte[] bytesObjCustomerAlertDatatable_SERLAY)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubCustomerAlert ObjCustomerAlert = new S3GDALayer.CompanyMgtServices.ClsPubCustomerAlert();
                return ObjCustomerAlert.FunUpdateCustomerAlert(SerMode, bytesObjCustomerAlertDatatable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Cutomer Alert Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public int FunPubDeleteCustomerAlert(SerializationMode SerMode, byte[] bytesObjCustomerAlertDatatable_SERLAY)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubCustomerAlert ObjCustomerAlert = new S3GDALayer.CompanyMgtServices.ClsPubCustomerAlert();
                return ObjCustomerAlert.FunPubDeleteCustomerAlert(SerMode, bytesObjCustomerAlertDatatable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Cutomer Alert Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunPubQueryEventTypeMaster(SerializationMode SerMode, byte[] bytesObjEventTypeDatatable_SERLAY)
        {
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_EventTypeDataTable dtEventType;
                S3GDALayer.CompanyMgtServices.ClsPubCustomerAlert ObjEventType = new S3GDALayer.CompanyMgtServices.ClsPubCustomerAlert();
                dtEventType = ObjEventType.FunPubQueryEventTypeMasterList(SerMode, bytesObjEventTypeDatatable_SERLAY);
                byte[] byteEventtype = ClsPubSerialize.Serialize(dtEventType, SerializationMode.Binary);
                return byteEventtype;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Cutomer Alert Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunPubQueryEntityTypeMaster(SerializationMode SerMode, byte[] bytesObjEntityTypeDatatable_SERLAY)
        {
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_EntityTypeDataTable dtEntityType;
                S3GDALayer.CompanyMgtServices.ClsPubCustomerAlert ObjEntityType = new S3GDALayer.CompanyMgtServices.ClsPubCustomerAlert();
                dtEntityType = ObjEntityType.FunPubQueryEntityTypeMasterList(SerMode, bytesObjEntityTypeDatatable_SERLAY);
                byte[] byteEntitytype = ClsPubSerialize.Serialize(dtEntityType, SerializationMode.Binary);
                return byteEntitytype;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Cutomer Alert Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunPubQueryCustomerAlert(SerializationMode SerMode, byte[] bytesObjCustomerAlertDatatable_SERLAY)
        {
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable dtcustomeralert;
                S3GDALayer.CompanyMgtServices.ClsPubCustomerAlert Objcustomeralert = new S3GDALayer.CompanyMgtServices.ClsPubCustomerAlert();
                dtcustomeralert = Objcustomeralert.FunPubQueryCustomerAlert(SerMode, bytesObjCustomerAlertDatatable_SERLAY);
                byte[] byteCustomerAlert = ClsPubSerialize.Serialize(dtcustomeralert, SerializationMode.Binary);
                return byteCustomerAlert;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Cutomer Alert Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunPubQueryCustomerAlertPaging(SerializationMode SerMode, byte[] bytesObjCustomerAlertDatatable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable dtcustomeralert;
                S3GDALayer.CompanyMgtServices.ClsPubCustomerAlert Objcustomeralert = new S3GDALayer.CompanyMgtServices.ClsPubCustomerAlert();
                dtcustomeralert = Objcustomeralert.FunPubQueryCustomerAlertPaging(SerMode, bytesObjCustomerAlertDatatable_SERLAY, out intTotalRecords, ObjPaging);
                byte[] byteCustomerAlert = ClsPubSerialize.Serialize(dtcustomeralert, SerializationMode.Binary);
                return byteCustomerAlert;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Erro in Cutomer Alert Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region ICompanyMgtServices_Workflow

        /// <summary>
        /// Calling DAL Layer's Create workflow Method (FunPubCreateWorkflow) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjWorkflowMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>    
        public int FunPubCreateWorkflow(SerializationMode SerMode, byte[] bytesObjWorkflowMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubWorkflow ObjWorkflowMaster = new S3GDALayer.CompanyMgtServices.ClsPubWorkflow();
                intResult = ObjWorkflowMaster.FunPubCreateWorkflow(SerMode, bytesObjWorkflowMasterDataTable_SERLAY);
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
        /// Calling DAL Layer's Query workflow Method (FunPubQueryWorkflowDetails) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjWorkflowMasterDataTable_SERLAY"></param>
        /// <returns></returns>
        public byte[] FunPubQueryWorkflow(SerializationMode SerMode, byte[] bytesObjWorkflowMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_WorkflowDataTable dtWorkflowMaster;
                S3GDALayer.CompanyMgtServices.ClsPubWorkflow ObjWorkflowMaster = new S3GDALayer.CompanyMgtServices.ClsPubWorkflow();
                dtWorkflowMaster = ObjWorkflowMaster.FunPubQueryWorkflow(SerMode, bytesObjWorkflowMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtWorkflowMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        public string FunPubGetCompanyCode(int intCompanyID, string strCompanyCode)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubCompanyMaster ObjWorkflowMaster = new S3GDALayer.CompanyMgtServices.ClsPubCompanyMaster();
                ObjWorkflowMaster.FunPubQueryCompanyName(intCompanyID, out strCompanyCode);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return strCompanyCode;
        }
        #endregion

        #region Hierachy Master
        // ----Hierachy Master Creation(Created By- Irsathameen K)-------
        // ----Created On 03-May-2011------
        //Begin

        public int FunPubCreateHierachyMasterDetails(SerializationMode SerMode, byte[] bytesObjS3G_Sys_HierachyMAsterDataTable)
        {
            try
            {
                S3GDALayer.CompanyMgtServices.ClsPubHierachyMaster objPubHierachyMaster = new S3GDALayer.CompanyMgtServices.ClsPubHierachyMaster();
                return objPubHierachyMaster.FunPubCreateHierachyMasterDetails(SerMode, bytesObjS3G_Sys_HierachyMAsterDataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }
        //End
        #endregion


        public int FunPubCreateManualLookup(SerializationMode SerMode, byte[] bytesObjManualLookupDataTable_SERLAY)
        {
            try
            {

                S3GDALayer.ClsPubManualLookup ObjManualLookup = new S3GDALayer.ClsPubManualLookup();
                intResult = ObjManualLookup.FunPubCreateManualLookupDetails(SerMode, bytesObjManualLookupDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }


        public int FunPubModifyManualLookup(SerializationMode SerMode, byte[] bytesObjManualLookupDataTable_SERLAY)
        {
            try
            {

                S3GDALayer.ClsPubManualLookup ObjManualLookup = new S3GDALayer.ClsPubManualLookup();
                intResult = ObjManualLookup.FunPubModifyManualLookupDetails(SerMode, bytesObjManualLookupDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        } 
    }
}
