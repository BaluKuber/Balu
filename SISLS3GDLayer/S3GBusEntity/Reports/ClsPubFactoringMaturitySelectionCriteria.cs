﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubFactoringMaturitySelectionCriteria
    {
        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string LobId { get; set; }

        [DataMember]
        public string LocationID { get; set; }        

        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public string DenominationId { get; set; }

        //[DataMember]
        //public bool IsDetail { get; set; }




    }
}