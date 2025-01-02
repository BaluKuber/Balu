using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using S3GDALayer;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel.Activation;

namespace S3GServiceLayer
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "CommonMail" here, you must also update the reference to "CommonMail" in Web.config.
    public class CommonMail : ICommonMail
    {
        public void FunSendMail(ClsPubCOM_Mail ObjCOM_Mail_SERLAY)
        {
            try
            {
                ClsPubMail ObjMail = new ClsPubMail();
                ObjMail.FunSendMail(ObjCOM_Mail_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                //opc038 start
                if (objExp.InnerException!=null)
                {
                    objFault.ProReasonRW = "Unable to send Email To " + ObjCOM_Mail_SERLAY.ProTORW + " " + objExp.Message.ToString() + " " + objExp.InnerException.Message;
                }
                else
                {
                    objFault.ProReasonRW = "Unable to send Email To " + ObjCOM_Mail_SERLAY.ProTORW + " " + objExp.Message.ToString();
                }
                //opc038 end
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public string[] GetSuggestion(string prefixText, int count)
        {
            try
            {
                ClsPubMail ObjMail = new ClsPubMail();
                string[] strReturn = ObjMail.GetData(prefixText, count);
                return strReturn;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
       
    }
}
