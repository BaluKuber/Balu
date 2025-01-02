using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
  public class ClsPubChequeReturnAmount
    {
        
        [DataMember]
        public string LOB_Name { get; set; }

        [DataMember]
        public string Location_Name { get; set; }
        [DataMember]
        public string Level { get; set; }

        [DataMember]
        public string Customer_Name { get; set; }

        [DataMember]
        public string PANum { get; set; }
        [DataMember]
        public string SANum { get; set; }
        [DataMember]
        public int NOD { get; set; }

        [DataMember]
        public string AssetType { get; set; }


        [DataMember]
        public string AssetClass { get; set; }

        [DataMember]
        public string Product { get; set; }



        [DataMember]
        public Decimal Ageing0to30 { get; set; }
        [DataMember]
        public Decimal Ageing31to60 { get; set; }
        [DataMember]
        public Decimal Ageing61to90 { get; set; }
        [DataMember]
        public Decimal AgeingAbove90 { get; set; }
        [DataMember]
        public Decimal Chq_Ageing0to30 { get; set; }
        [DataMember]
        public Decimal Chq_Ageing31to60 { get; set; }
        [DataMember]
        public Decimal Chq_Ageing61to90 { get; set; }
        [DataMember]
        public Decimal Chq_AgeingAbove90 { get; set; }
        [DataMember]
        public string Due_Date { get; set; }
        [DataMember]
        public Decimal Due_Amount { get; set; }

    }
}
