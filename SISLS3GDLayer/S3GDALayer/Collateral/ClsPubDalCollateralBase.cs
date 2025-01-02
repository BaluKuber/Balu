using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace S3GDALayer.Collateral
{
    namespace CollateralMgtServices
    {
        public class ClsPubDalCollateralBase
        {
            protected Database db;

            public ClsPubDalCollateralBase()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }
        }
    }
}