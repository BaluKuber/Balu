using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
   public class ClsPubPDCReminderDetails
    {
        public List<ClsPubRptPDCReminderGridDetails> GridDetails { get; set; }
        public List<ClsPubPDCReminderAssetDetails> AssetDetails { get; set; }
        public List<ClsPubCustomerList> CustomerList { get; set; }
    }
}
