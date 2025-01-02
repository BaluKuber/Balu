using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using S3GDALayer;
using System.Data;
using System.ServiceModel.Activation;

namespace S3GServiceLayer
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class WorkflowMgtService : IWorkflowMgtService
    {
        byte[] bytesDataTable;
        public int FunPubInsertWorkflowStatus(WorkFlowStatus objWorkflowStatus)
        {
            S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus objWorkflowStatusClient = new S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus();
            return objWorkflowStatusClient.FunPubInsertWorkflowStatus(objWorkflowStatus);
        }

        public byte[] FunPubLoadPreDisDocTransaction(string strEnquiryNo, int intCompanyId,int intDocument_Type)
        {
            DataSet ds;
            S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus objWorkflowStatusClient = new S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus();
            ds = objWorkflowStatusClient.FunPubLoadPreDisDocTransaction(strEnquiryNo, intCompanyId,intDocument_Type);
            if (ds != null)
                bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            return bytesDataTable;

        }


        public byte[] FunPubLoadFIRTransaction(string strEnquiryNo, int intCompanyId, int intDocument_Type,string PANum,string SANum)
        {
            DataSet ds;
            S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus objWorkflowStatusClient = new S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus();
            ds = objWorkflowStatusClient.FunPubLoadFIRTransaction(strEnquiryNo, intCompanyId, intDocument_Type,PANum,SANum);
            if (ds != null)
                bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            return bytesDataTable;

        }

        public byte[] FunPubLoadProforma(string strEnquiryNo, int intCompanyId, int intDocument_Type, string PANum, string SANum)
        {
            DataSet ds;
            S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus objWorkflowStatusClient = new S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus();
            ds = objWorkflowStatusClient.FunPubLoadProforma(strEnquiryNo, intCompanyId, intDocument_Type, PANum, SANum);
            if (ds != null)
                bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            return bytesDataTable;

        }

        public byte[] FunPubLoadInvoice(string strEnquiryNo, int intCompanyId, int intDocument_Type, string PANum, string SANum)
        {
            DataSet ds;
            S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus objWorkflowStatusClient = new S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus();
            ds = objWorkflowStatusClient.FunPubLoadInvoice(strEnquiryNo, intCompanyId, intDocument_Type, PANum, SANum);
            if (ds != null)
                bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            return bytesDataTable;

        }

        public byte[] FunPubLoadPostDisb(string strEnquiryNo, int intCompanyId, int intDocument_Type, string PANum, string SANum)
        {
            DataSet ds;
            S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus objWorkflowStatusClient = new S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus();
            ds = objWorkflowStatusClient.FunPubLoadPostDisb(strEnquiryNo, intCompanyId, intDocument_Type, PANum, SANum);
            if (ds != null)
                bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            return bytesDataTable;

        }


        public byte[] FunPubLoadEnquiryCustomerAppraisal(string strEnquiryNo, int intCompanyId, int intDocument_Type)
        {
            DataSet ds;
            S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus objWorkflowStatusClient = new S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus();
            ds = objWorkflowStatusClient.FunPubLoadEnquiryCustomerAppraisal(strEnquiryNo, intCompanyId, intDocument_Type);
            if (ds != null)
                bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            return bytesDataTable;

        }

        public byte[] FunPubLoadCreditGuideTransaction(string strEnquiryNo, int intCompanyId, int intDocument_Type)
        {
            DataSet ds;
            S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus objWorkflowStatusClient = new S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus();
            ds = objWorkflowStatusClient.FunPubLoadCreditGuideTransaction(strEnquiryNo, intCompanyId, intDocument_Type);
            if (ds != null)
                bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            return bytesDataTable;
        }
        public byte[] FunPubLoadCreditParameterApproval(string strEnquiryNo, int intCompanyId, int intDocument_Type)
        {
            DataSet ds;
            S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus objWorkflowStatusClient = new S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus();
            ds = objWorkflowStatusClient.FunPubLoadCreditParameterApproval(strEnquiryNo, intCompanyId, intDocument_Type);
            if (ds != null)
                bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            return bytesDataTable;
        }

        public byte[] FunPubLoadPricing(string strEnquiryNo, int intCompanyId, int intDocument_Type)
        {
            DataSet ds;
            S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus objWorkflowStatusClient = new S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus();
            ds = objWorkflowStatusClient.FunPubLoadPricing(strEnquiryNo, intCompanyId, intDocument_Type);
            if (ds != null)
                bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            return bytesDataTable;
        }
        public byte[] FunPubLoadPricingApproval(string strEnquiryNo, int intCompanyId, int intDocument_Type)
        {
            DataSet ds;
            S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus objWorkflowStatusClient = new S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus();
            ds = objWorkflowStatusClient.FunPubLoadPricingApproval(strEnquiryNo, intCompanyId,intDocument_Type);
            if (ds != null)
                bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            return bytesDataTable;
        }
        public byte[] FunPubGetUserAccess(int intUserId, int intCompanyId)
        {
            DataSet ds;
            S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus objWorkflowStatusClient = new S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus();
            ds = objWorkflowStatusClient.FunPubGetUserAccess(intUserId, intCompanyId);
            if (ds != null)
                bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            return bytesDataTable;
        }

        public byte[] FunPubLoadApplicationApproval(string strApplicationNo, int intCompanyId,int intUserId)
        {
            DataSet ds;
            S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus objWorkflowStatusClient = new S3GDALayer.WorkflowMgtService.ClsPubWorkflowStatus();
            ds = objWorkflowStatusClient.FunPubLoadApplicationApproval(strApplicationNo, intCompanyId, intUserId);
            if (ds != null)
                bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            return bytesDataTable;
        }
        
    }
}
