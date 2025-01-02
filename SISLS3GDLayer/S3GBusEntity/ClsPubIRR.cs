using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace S3GBusEntity
{
    public class ClsPubIRR
    {
        public string strFrequency { get; set; }
        public  int intTenure { get; set; }
        public string strTenureType { get; set; }
        public decimal decPrincipleAmount { get; set; }
        public decimal decRateofInt { get; set; }
        public RepaymentType returnType { get; set; }
        public decimal? decResidualValue { get; set; }
        public DateTime dtimeStartDate { get; set; }
        public DateTime dtimeDocDate { get; set; }
        public int intCashFlow { get; set; }
        public string strCashflowDesc { get; set; }
        public decimal decRoundOff { get; set; }
        public bool blnAccountingIRR { get; set; }
        public bool blnBusinessIRR { get; set; }
        public bool blnCompanyIRR { get; set; }
        public string strTimeVal { get; set; }
        public DataTable dtRepaymentDetails { get; set; }
        public DataTable dtCashInflow { get; set; }
        public DataTable dtCashOutflow { get; set; }
        public string strDateFormat { get; set; }
        public string strIRRrest { get; set; }
        public string strInstallmentType { get; set; }
        public decimal decLimit { get; set; }
        public decimal decCTR { get; set; }
        public decimal decPLR { get; set; }
        public decimal decIRR { get; set; }
        public decimal? decResidualAmount { get; set; }
        public IRRType Irrtype { get; set; }
        public DataTable dtMoratoriumInput { get; set; }
    }
}
