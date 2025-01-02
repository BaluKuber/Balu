using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace S3GDALayer.LegalRepossession
{
    namespace LegalAndRepossessionMgtServices
    {
        public class ClsPubDalLegalRepossessionBase
        {
            protected Database db;

            public ClsPubDalLegalRepossessionBase()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }
        }
    }
}
