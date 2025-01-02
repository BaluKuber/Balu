using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubRptPDCReminderGridDetails
    {
        [DataMember]
        public string CUSTOMER_NAME { get; set; }
        [DataMember]
        public string CUSTOMERNAME { get; set; }
        [DataMember]
        public string PRIMEACCOUNTNO { get; set; }
        [DataMember]
        public string SUBACCOUNTNO { get; set; }
        [DataMember]
        public string LASTCOLLECTEDPDCDATE { get; set; }
        [DataMember]
        public string Comm_Address1 { get; set; }
        [DataMember]
        public string Comm_Address2 { get; set; }
        [DataMember]
        public string Comm_City { get; set; }
        [DataMember]
        public string Comm_State { get; set; }
        [DataMember]
        public string Comm_country { get; set; }
        [DataMember]
        public string Comm_PinCode { get; set; }
        [DataMember]
        public string Comm_Mobile { get; set; } 
        [DataMember]
        public string RepaymentDateFrom { get; set; }
        [DataMember]
        public string RepaymentDateTo { get; set; }
        [DataMember]
        public string CompanyAddress { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string Report_Date { get; set; }
        [DataMember]
        public string CustomerAddress { get; set; }
        [DataMember]
        public string CustomerMail { get; set; }    
    }
}
