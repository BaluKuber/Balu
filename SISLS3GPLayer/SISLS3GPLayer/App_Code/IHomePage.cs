using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Security;

/// <summary>
/// Summary description for IHomePage
/// </summary>
public interface IHomePage
{    
        /// <summary>
        /// Assign the Session Values required for Work flow 
        /// </summary>
        void AssisgnApplicationSessionValues();
        void FunpriWorkflowBind(Char ListType);
        void AssignSelectedWorkFlowRecord(char ListType);
        DataTable LoadWorkFlowScreensList(string WFSequenceID);
        void LoadWorkFlowDocument(SessionValues sessionItems);
        void NavigateToWFURL(WorkFlowSession WFValues, FormsAuthenticationTicket Ticket, string ProgramCode);
        void LoadClosedWorkItems();    
   
}
