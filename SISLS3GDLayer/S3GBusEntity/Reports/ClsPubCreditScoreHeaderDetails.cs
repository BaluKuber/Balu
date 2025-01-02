using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public sealed class ClsPubCreditScoreHeaderDetails
    {
        [DataMember]
        public string Lob { get; set; }

        //[DataMember]
        //public string Location1 { get; set; }

        //[DataMember]
        //public string Location2 { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string Product { get; set; }

        [DataMember]
        public string StartDate { get; set; }

        [DataMember]
        public string EndDate { get; set; }
    }
}