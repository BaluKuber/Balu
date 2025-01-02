using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubRptDPDGridDetails
    {
        [DataMember]
        public string Column1 { get; set; }  //PANUM
        [DataMember]
        public string Column2 { get; set; } //SANUM
        [DataMember]
        public string Column3 { get; set; } //ARREARS
        [DataMember]
        public string Column4 { get; set; } //BUCKET1
        [DataMember]
        public string Column5 { get; set; } //BUCKET1%
        [DataMember]
        public string Column6 { get; set; } //BUCKET2
        [DataMember]
        public string Column7 { get; set; } //BUCKET2%
        [DataMember]
        public string Column8 { get; set; } //BUCKET3
        [DataMember]
        public string Column9 { get; set; } //BUCKET3%
        [DataMember]
        public string Column10 { get; set; } //BUCKET4
        [DataMember]
        public string Column11 { get; set; } //BUCKET4%
        [DataMember]
        public string Column12 { get; set; } //BUCKET5
        [DataMember]
        public string Column13 { get; set; } //BUCKET5%
        [DataMember]
        public string Column14 { get; set; } //BUCKET6
        [DataMember]
        public string Column15 { get; set; } //BUCKET6%
        [DataMember]
        public string Column16 { get; set; } //BUCKET7
        [DataMember]
        public string Column17 { get; set; } //BUCKET7%
        [DataMember]
        public string Column18 { get; set; } //BUCKET8
        [DataMember]
        public string Column19 { get; set; } //BUCKET8%
        [DataMember]
        public string Column20 { get; set; } //BUCKET9
        [DataMember]
        public string Column21 { get; set; } //BUCKET9%
        [DataMember]
        public string Column22 { get; set; } //TOTAL
        [DataMember]
        public string Column23 { get; set; } //TOTAL%
    }
}
