using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubCollectionAnalysis
    {
        [DataMember]
        public string LOB { get; set; }

        [DataMember]
        public string Level { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public int AccountNumber { get; set; }

        [DataMember]
        public string SubAccountNumber { get; set; }

        [DataMember]
        public string AssetClass { get; set; }

        [DataMember]
        public string AssetType { get; set; }

        [DataMember]
        public string Product { get; set; }

        [DataMember]
        public Decimal FstPrdDue { get; set; }

        [DataMember]
        public Decimal SndPrdDue { get; set; }

        [DataMember]
        public Decimal FstprdClnAmt030 { get; set; }
        
        [DataMember]
        public Decimal SndprdClnAmt030 { get; set; }

        [DataMember]
        public Decimal FstprdClnAmt3160 { get; set; }


        [DataMember]
        public Decimal SndprdClnAmt3160 { get; set; }

        [DataMember]
        public Decimal FstprdClnAmt6190 { get; set; }

        [DataMember]
        public Decimal SndprdClnAmt6190 { get; set; }

        [DataMember]
        public Decimal FstprdClnAmtAbv90 { get; set; }

        [DataMember]
        public Decimal SndprdClnAmtAbv90 { get; set; }

      }
}
