using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity.Reports;

namespace S3GDALayer.Reports
{
    public class ClsPubDalReportBase
    {
        protected Database db;

        public ClsPubDalReportBase()
        {
            db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
        }

        protected Decimal DecimalParse(object val)
        {
            Decimal returnVal = 0;
            try
            {
                if (val != DBNull.Value)
                {
                    returnVal = Decimal.Parse(val.ToString());
                }
            }
            catch { }
            return returnVal;
        }

        protected int IntParse(object val)
        {
            int returnVal = 0;
            try
            {
                if (val != DBNull.Value)
                {
                    returnVal = int.Parse(val.ToString());
                }
            }
            catch { }
            return returnVal;
        }

        protected Boolean BooleanParse(object val)
        {
            Boolean returnVal = false;
            try
            {
                if (val != DBNull.Value)
                {
                    returnVal = Boolean.Parse(val.ToString());
                }
            }
            catch { }
            return returnVal;
        }

        protected string DateTimeParse(object val)
        {
            string returnVal = string.Empty;
            try
            {
                if (val != DBNull.Value)
                {
                    returnVal = DateTime.Parse(val.ToString()).ToShortDateString();
                }
            }
            catch { }
            return returnVal;
        }

        protected string StringParse(object val)
        {
            if (val == DBNull.Value)
            {
                return string.Empty;
            }
            else
            {
                return val.ToString().Trim();
            }
        }

        protected long LongParse(object val)
        {
            long returnVal = 0;
            try
            {
                if (val != DBNull.Value)
                {
                    returnVal = long.Parse(val.ToString());
                }
            }
            catch { }
            return returnVal;
        }

        protected List<ClsPubDropDownList> FunPriLoadSelect(List<ClsPubDropDownList> dropdownlists)
        {
            ClsPubDropDownList dropDownList = new ClsPubDropDownList();
            dropDownList.ID = "-1";
            dropDownList.Description = "--Select--";
            dropdownlists.Insert(0, dropDownList);
            return dropdownlists;
        }

        protected string FunProGeneratePrimeSubAccount(List<ClsPubPASA> PASAs)
        {
            StringBuilder strbXml = new StringBuilder();
            strbXml.Append("<Root>");

            foreach (ClsPubPASA pasa in PASAs)
            {
                strbXml.Append(" <Details ");
                strbXml.Append("PRIMEACCOUNTNO='" + pasa.PrimeAccountNo.Trim() + "'");
                strbXml.Append(" ");

                string saNum = pasa.SubAccountNo.Trim();
                if (saNum == string.Empty)
                {
                    saNum = pasa.PrimeAccountNo.Trim() + "DUMMY";
                }
                strbXml.Append("SUBACCOUNTNO='" + saNum + "'");
                strbXml.Append(" ");
                strbXml.Append("/>");
            }

            strbXml.Append("</Root>");

            return strbXml.ToString();
        }
    }
}
