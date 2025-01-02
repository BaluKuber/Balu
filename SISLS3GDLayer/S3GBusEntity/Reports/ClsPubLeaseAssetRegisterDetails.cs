using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
/// <summary>
/// Summary description for ClsPubEnquiryLocation
/// </summary>
/// 
namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubLeaseAssetRegisterDetails
    {
        [DataMember]
        public List<ClsPubLANRegisterDetails> LANDetails { get; set; }

        [DataMember]
        public List<ClsPubLobLocation> LocationLan { get; set; }

    }
}
