using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubSanctionDetailsReport
    {
        public List<ClsPubSanctionDetails> Sanction { get; set; }
        public List<ClsPubSanctionDisbursableDetails> Disbursable { get; set; }
        public List<ClsPubSanctionDisbursedDetails> Disbursed { get; set; }

    }
}
