using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubPayment
    {
        [DataMember]
        public List<ClsPubBranchAsset> assets { get; set; }

             
        [DataMember]
        public List<ClsPubPaymentDetails> PaymentDetails { get; set; }

        //public ClsPubPayment()
        //{
        //    PaymentDetails = new List<ClsPubPaymentDetails>();
        //}

    }
}
