using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.ScheduledJobs
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    // NOTE: If you change the class name "ScheduledJobMgtServices" here, you must also update the reference to "ScheduledJobMgtServices" in Web.config.
    public class ScheduledJobMgtServices : IScheduledJobMgtServices
    {
        #region Scheduled Jobs

        public int FunPubCreateScheduledJobs(SerializationMode SerMode, byte[] bytesObjScheduledJobsDatatable, out string strScheduleJobID)
        {

            try
            {
                S3GDALayer.ScheduledJobs.ScheduledJobs.ClsPubScheduledJobs objScheduledJobs = new S3GDALayer.ScheduledJobs.ScheduledJobs.ClsPubScheduledJobs();
                return objScheduledJobs.FunPubCreateScheduledJobs(SerMode, bytesObjScheduledJobsDatatable, out strScheduleJobID);

            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }


        #endregion
    }
}
