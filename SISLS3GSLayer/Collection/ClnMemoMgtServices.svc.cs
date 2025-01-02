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

namespace S3GServiceLayer.Collection
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "ClnMemoMgtServices" here, you must also update the reference to "ClnMemoMgtServices" in Web.config.
    public class ClnMemoMgtServices : IClnMemoMgtServices
    {
        #region Initialization
        int intResult;
        #endregion


        #region Memorandum Booking
        /// <summary>
        /// Inserting records in Memorandum Booking table
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>
        public int FunPubCreateMemorandumBooking(SerializationMode SMode, byte[] byteObjS3G_Colection_MemorandumBooking, out string strMemoBookNo)
        {
            try
            {
                S3GDALayer.Collection.ClnMemoMgtServices.ClsPubMemorandumBooking objClsPubMemorandumBooking = new S3GDALayer.Collection.ClnMemoMgtServices.ClsPubMemorandumBooking();
                return objClsPubMemorandumBooking.FunPubCreateModifyMemorandumBooking(SMode, byteObjS3G_Colection_MemorandumBooking, out strMemoBookNo);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region MemoMaster
        public int FunPubCreateMemoDetails(SerializationMode SMode, byte[] byteObjS3G_Colection_MemoMaster)
        {
            try
            {
                S3GDALayer.Collection.ClnMemoMgtServices.ClsPubMemoMaster objClsPubMemo = new S3GDALayer.Collection.ClnMemoMgtServices.ClsPubMemoMaster();
                return objClsPubMemo.FunPubCreateMemoDetails(SMode, byteObjS3G_Colection_MemoMaster);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubModifyMemoDetails(SerializationMode SMode, byte[] byteObjS3G_Colection_MemoMaster)
        {
            try
            {
                S3GDALayer.Collection.ClnMemoMgtServices.ClsPubMemoMaster objClsPubMemo = new S3GDALayer.Collection.ClnMemoMgtServices.ClsPubMemoMaster();
                return objClsPubMemo.FunPubModifyMemoDetails(SMode, byteObjS3G_Colection_MemoMaster);
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
