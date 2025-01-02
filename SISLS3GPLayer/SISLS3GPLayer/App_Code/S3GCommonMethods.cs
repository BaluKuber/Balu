using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ServiceModel;

/// <summary>
/// Summary description for S3GCommonMethods
/// Created By  : Rajendran
/// Purpose     : Separate the 
/// </summary>
public static partial class Utility
{
    public static string GetTableScalarValue(string strProcName, Dictionary<string, string> dictProcParam)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjAdminService = null;        
        try
        {
            ObjAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
            string ScalarValue = ObjAdminService.FunGetScalarValue(strProcName, dictProcParam);            
            return ScalarValue;
        }
        catch (FaultException ex)
        {
            
        }
        finally
        {
            ObjAdminService.Close();                       
        }
        return null;
    }

    public static string ValidateMonthClosure(string strProcName, Dictionary<string, string> dictProcParam)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjAdminService = null;
        try
        {
            ObjAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
            string ScalarValue = ObjAdminService.FunValidateMonthClosure(strProcName, dictProcParam);
            return ScalarValue;
        }
        catch (FaultException ex)
        {

        }
        finally
        {
            ObjAdminService.Close();
        }
        return null;
    } 

    /// <summary>
    /// Navigate to Work FLOW URL
    /// </summary>
    /// <param name="WFValues"></param>
    /// <param name="Ticket"></param>
    /// <param name="ProgramCode"></param>
    public static string NavigateToWFURL( out string PageName, S3GModules pModule, string ProgramCode,string wfId)
    {
        WorkFlowSession WFValues = new WorkFlowSession();
        PageName = "";
        DataRow[] NavigationURL = WFValues.WorkFlowScreens.Select("Program_Ref_No=" + ProgramCode);
        if(NavigationURL.Length==0)
            NavigationURL = WFValues.WorkFlowScreens.Select("Workflow_Prg_Id=" + ProgramCode);
        string ReturnURL = string.Empty;
        if (NavigationURL.Length > 0) // URL EXISTS
        {
            switch (pModule)
            {
                case S3GModules.SysAdmin:
                    break;
                case S3GModules.Orgination:
                    //ReturnURL = "~/Origination/" + NavigationURL[0]["WFUrl"].ToString() + wfId + "&qsMode=W";
                    PageName = NavigationURL[0]["Program_Name"].ToString();
                    break;
                case S3GModules.LoanAdmin:
                    break;
                case S3GModules.Collection:
                    break;
                case S3GModules.Insurance:
                    break;
                default:
                    break;
            }
        }
        return ReturnURL;
    }
    

    public enum S3GModules
    {
        SysAdmin,
        Orgination,
        LoanAdmin,
        Collection,
        Insurance

    }    
}

