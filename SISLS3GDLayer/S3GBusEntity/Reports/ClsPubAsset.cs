using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]  
    [DataContract]
    public class ClsPubAsset
    {
        [DataMember]
        public string LobName { get; set; }

        [DataMember]
        public string PrimeAccountNo { get; set; }

        [DataMember]
        public string SubAccountNo { get; set; }

        [DataMember]
        public string AssetDesc { get; set; }

        [DataMember]
        public string RegNo { get; set; }

        [DataMember]
        public decimal AmountFinanced { get; set; }

        [DataMember]
        public string Terms { get; set; }

        [DataMember]
        public decimal YetToBeBilled { get; set; }

        [DataMember]
        public decimal Billed { get; set; }

        [DataMember]
        public decimal Balance { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }

        [DataMember]
        public string Denomination { get; set; }


        [DataMember]
        public string PO_Number { get; set; }

        [DataMember]
        public string PO_Date { get; set; }

        [DataMember]
        public string PI_Number { get; set; }

        [DataMember]
        public string PI_Date { get; set; }

        [DataMember]
        public string VI_Number { get; set; }

        [DataMember]
        public string VI_Date { get; set; }

        [DataMember]
        public string Entity_Name { get; set; }

        [DataMember]
        public string Asset_Category { get; set; }

        [DataMember]
        public string Asset_Type { get; set; }

        [DataMember]
        public string Asset_Sub_Type { get; set; }

        [DataMember]
        public decimal Total_SCH_Amt { get; set; }

        [DataMember]
        public decimal InvoiceAmount { get; set; }

        [DataMember]
        public decimal CapitalisedAmount { get; set; }
    
    }
}
