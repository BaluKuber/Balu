using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net.Mail;
namespace S3GBusEntity
{
    [Serializable]
    public class ClsPubCOM_Mail
    {
        #region properties

        public string ProFromRW
        {
            get;
            set;
        }
        public string ProDisplayName
        {
            get;
            set;
        }
        public string ProTORW
        {
            get;
            set;
        }
        public string ProCCRW
        {
            get;
            set;
        }
        public string ProBCCRW
        {
            get;
            set;
        }
        public string ProSubjectRW
        {
            get;
            set;
        }
        public string ProMessageRW
        {
            get;
            set;
        }
        public ArrayList ProFileAttachementRW
        {
            get;
            set;
        }
        //opc038 start
        public string ProImgPathRW
        {
            get;
            set;
        }
        //end
        #endregion
    }
}
