using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace S3GDALayer.Insurance
{
    namespace InsuranceMgtServices
    {
        public class ClsPubDalInsuranceBase
        {
            protected Database db;

            public ClsPubDalInsuranceBase()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }
        }
    }
}
