using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for ClsPubDemandParameterCCL
/// </summary>
public class ClsPubDemandParameterCCL
{
	
        public int CompanyId { get; set; }

        
        public string LobId { get; set; }

        
        public string LobName { get; set; }

        
        public string CustomerId { get; set; }

        
        public string GroupId { get; set; }

        
        public int LocationId1 { get; set; }

        
        public string RegionName { get; set; }

        
        public string ClassId { get; set; }


        public int LocationId2 { get; set; }

        
        public string BranchName { get; set; }

        
        public string FrequencyId { get; set; }

        
        public string GroupingCriteriaId { get; set; }

        
        public string GroupingCriteriaName { get; set; }

        
        public string PANum { get; set; }

        
        public string SANum { get; set; }

        
        public int AssetTypeId { get; set; }

        
        public string FromMonth { get; set; }

        
        public string ToMonth { get; set; }

        
        public string PreFromMonth { get; set; }

        
        public string PreToMonth { get; set; }

        
        public decimal Denomination { get; set; }

        
        public DateTime FinYearStartMonthStartDate { get; set; }

        
        public DateTime FromMonthStartDate { get; set; }

        
        public DateTime FromMonthPreMonthEndDate { get; set; }

        
        public DateTime ToMonthEndDate { get; set; }

        
        public DateTime CompareFinYearStartMonthStartDate { get; set; }

        
        public DateTime CompareFromMonthStartDate { get; set; }

        
        public DateTime CompareFromMonthPreMonthEndDate { get; set; }

        
        public DateTime CompareToMonthEndDate { get; set; }

        public ClsPubDemandParameterCCL()
        {
            Denomination = 1;
        }
}
