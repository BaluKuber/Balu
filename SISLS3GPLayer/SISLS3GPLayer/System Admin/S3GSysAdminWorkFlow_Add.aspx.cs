//Module Name     :   System Admin
//Screen Name     :   S3GSysAdminWorkflow_Add.aspx
//Created By      :   Kaliraj K
//Created Date    :   28-May-2010
//Purpose         :   To insert and update workflow details
//Last updated By :   Gunasekar.K
//Last updated Date : 13-Oct-2010
//Purpose           : To select the Program based on the module selection
//Last updated By :   Saran.M
//Last updated Date : January-2011
//Purpose           : For Bug fixing

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;

public partial class S3GSysAdminWorkFlow_Add : ApplyThemeForProject
{
    #region Intialization

    SerializationMode SerMode = new SerializationMode();
    Dictionary<string, string> Procparam = null;

    CompanyMgtServices.S3G_SYSAD_WorkflowDataTable ObjS3G_SYSAD_WorkflowDataTable = null;
    CompanyMgtServicesReference.CompanyMgtServicesClient ObjWorkflowMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
    //UserMgtServicesReference.UserMgtServicesClient ObjRoleCenterMasterClient;
    string strXMLCreditScoreDet = "<Root><Details Desc='0' /></Root>";
    StringBuilder strbCreditScoreDet = new StringBuilder();

    string strSelectModule = "Please select the Module";
    string strSelectProgram = "Please select the Program";
    string strExistProgram = "Program is already selected. Please select the new one";
    string strSelectProgramFlowNumber = "Please select the Program Flow Number";
    string strStartNumber = "Starting Work Flow Sequence number must ends with 0";
    int intErrCode = 0;
    int intWorkflowId = 0;
    int intUserId = 0;
    int intCompanyID = 0;
    bool bModify = false;
    bool bQuery = false;
    bool bCreate = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    UserInfo ObjUserInfo = null;
    int inthdUserid;
    string strRedirectPageMC;
    string strMode = string.Empty;
    string strRedirectPage = "../System Admin/S3GSysAdminWorkflow_View.aspx";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminWorkflow_Add.aspx';";
    string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminWorkflow_View.aspx';";
    DataTable dtWorkflow = null;
    DataTable dtWorkflowDeleted = null;
    #endregion

    #region Gridview Workflow
    private void FunPubBindWorkflowSequence(DataTable dtWorkflow)
    {
        gvWorkFlowSequence.DataSource = dtWorkflow;
        gvWorkFlowSequence.DataBind();
        if (dtWorkflow.Rows[0]["WorkFlow_ID"].ToString()=="0")
        {
            gvWorkFlowSequence.Rows[0].Visible = false;
            //ddlLOB.Enabled = true;
            //ddlProductCode.Enabled = true;
        }
        else
        {
            //ddlLOB.Enabled = false;
            //ddlProductCode.Enabled = false;
        }
    }
    private DataTable FunPriGetWorkflowDataTable(string strModule_ID, string strModule_Name, string strProgram_ID, string strProgram_Name, string strProgram_Flow_ID, string strWokfFlow_Sequence_Name)
    {
        DataRow drEmptyRow;
        if (ViewState["ListValues"] == null)
        {
            dtWorkflow = new DataTable();
            dtWorkflow.Columns.Add("WorkFlow_ID");
            dtWorkflow.Columns.Add("Module_ID");
            dtWorkflow.Columns.Add("Module_Name");
            dtWorkflow.Columns.Add("Program_ID");
            dtWorkflow.Columns.Add("Program_Name");
            dtWorkflow.Columns.Add("Program_Flow_ID");
            dtWorkflow.Columns.Add("WokfFlow_Sequence_Name");

            drEmptyRow = dtWorkflow.NewRow();
            drEmptyRow["WorkFlow_ID"] = "0";
            drEmptyRow["Module_ID"] = strModule_ID;
            drEmptyRow["Module_Name"] = strModule_Name;
            drEmptyRow["Program_ID"] = (strProgram_ID.Length == 2) ? ("0" + strProgram_ID) : strProgram_ID; ;
            drEmptyRow["Program_Name"] = strProgram_Name;
            drEmptyRow["Program_Flow_ID"] = strProgram_Flow_ID;
            drEmptyRow["WokfFlow_Sequence_Name"] = strWokfFlow_Sequence_Name;
            dtWorkflow.Rows.Add(drEmptyRow);
            ViewState["ListValues"] = dtWorkflow;
        }
        else if (ViewState["ListValues"] != null)
        {
            dtWorkflow = (DataTable)ViewState["ListValues"];
            if (dtWorkflow.Rows.Count == 0)
            {
                drEmptyRow = dtWorkflow.NewRow();
                drEmptyRow["WorkFlow_ID"] = "0";
                drEmptyRow["Module_ID"] = strModule_ID;
                drEmptyRow["Module_Name"] = strModule_Name;
                drEmptyRow["Program_ID"] = (strProgram_ID.Length == 2) ? ("0" + strProgram_ID) : strProgram_ID; ;
                drEmptyRow["Program_Name"] = strProgram_Name;
                drEmptyRow["Program_Flow_ID"] = strProgram_Flow_ID;
                drEmptyRow["WokfFlow_Sequence_Name"] = strWokfFlow_Sequence_Name;
                dtWorkflow.Rows.Add(drEmptyRow);
                ViewState["ListValues"] = dtWorkflow;
            }
            else
            {
                drEmptyRow = dtWorkflow.NewRow();
                drEmptyRow["WorkFlow_ID"] = Convert.ToInt32(dtWorkflow.Rows[dtWorkflow.Rows.Count - 1]["WorkFlow_ID"]) + 1;
                drEmptyRow["Module_ID"] = strModule_ID;
                drEmptyRow["Module_Name"] = strModule_Name;
                drEmptyRow["Program_ID"] = (strProgram_ID.Length == 2) ? ("0" + strProgram_ID) : strProgram_ID;
                drEmptyRow["Program_Name"] = strProgram_Name;
                drEmptyRow["Program_Flow_ID"] = strProgram_Flow_ID;
                drEmptyRow["WokfFlow_Sequence_Name"] = strWokfFlow_Sequence_Name;
                dtWorkflow.Rows.Add(drEmptyRow);

                if (dtWorkflow.Rows[0]["WorkFlow_ID"].Equals("0"))
                {
                    dtWorkflow.Rows[0].Delete();
                    //gvWorkFlowSequence.Rows[0].Visible = false;
                }
                ViewState["ListValues"] = dtWorkflow;
            }
        }
        return dtWorkflow;
    }
    protected void gvWorkFlowSequence_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            //Module Master
            DropDownList ddlModuleCode_Grd = e.Row.FindControl("ddlModuleCode_Grd") as DropDownList;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            ddlModuleCode_Grd.BindDataTable(SPNames.SYS_ModuleMaster, Procparam, new string[] { "Module_Code", "Module_Code", "Module_Name" });
            Procparam = null;
            //ddlModuleCode.SelectedValue = dtWorkflow.Rows[e.Row.RowIndex]["Module_ID"].ToString();


            //ddlModuleCode_Grd_SelectedIndexChanged(sender, e);

            //--Commend by Guna on 13-Oct-2010 to select the Program based on the module selection
            
            ////Program Master          
            //DropDownList ddlProgramID_Grd = e.Row.FindControl("ddlProgramID_Grd") as DropDownList;
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Is_Active", "1");
            //ddlProgramID_Grd.BindDataTable(SPNames.SYS_ProgramMaster, Procparam, new string[] { "Program_ID", "Program_Ref_No", "Program_Name" });
            //Procparam = null;

            //--Ends Here

            DropDownList ddlProgramFlowNumber_Grd = e.Row.FindControl("ddlProgramFlowNumber_Grd") as DropDownList;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Program_Ref_No", "0");
            ddlProgramFlowNumber_Grd.BindDataTable("S3G_Get_ProgramWFPrgMapping", Procparam, new string[] { "Workflow_Prg_ID", "Workflow_Prg_ID" });
            Procparam = null;
       
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    LinkButton lnkEdit = e.Row.FindControl("lnkEdit") as LinkButton;
            //    LinkButton btnRemove = e.Row.FindControl("btnRemove") as LinkButton;

            //    if (strMode == "Q")
            //    {
            //        lnkEdit.Enabled = false;
            //        btnRemove.Enabled = false;
            //    }
            //}
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkEdit = e.Row.FindControl("lnkEdit") as LinkButton;
            LinkButton btnRemove = e.Row.FindControl("btnRemove") as LinkButton;

            if (strMode == "Q")
            {
                lnkEdit.Enabled = false;
                btnRemove.Enabled = false;
            }
        }

    }
    protected void gvWorkFlowSequence_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Add")
        {

            if (!(ddlLOB.SelectedIndex > 0))
            {
                Utility.FunShowAlertMsg(this.Page, "Select the Line of Business");
                return;
            }
            if (!(ddlProductCode.SelectedIndex > 0))
            {
                Utility.FunShowAlertMsg(this.Page, "Select the Product");
                return;
            }
            
            dtWorkflow = (DataTable)ViewState["ListValues"];

            DropDownList ddlModuleCode_Grd = (DropDownList)gvWorkFlowSequence.FooterRow.FindControl("ddlModuleCode_Grd");
            DropDownList ddlProgramID_Grd = (DropDownList)gvWorkFlowSequence.FooterRow.FindControl("ddlProgramID_Grd");
            DropDownList ddlProgramFlowNumber_Grd = (DropDownList)gvWorkFlowSequence.FooterRow.FindControl("ddlProgramFlowNumber_Grd");

            if (ddlModuleCode_Grd != null)
            {
                if (!(ddlModuleCode_Grd.SelectedIndex > 0))
                {
                    Utility.FunShowAlertMsg(this.Page, "Please select the Module");
                    return;
                }
            }
            if (ddlProgramID_Grd != null)
            {
                if (!(ddlProgramID_Grd.SelectedIndex > 0))
                {
                    Utility.FunShowAlertMsg(this.Page, "Select the Program");
                    return;
                }
            }
            if (ddlProgramFlowNumber_Grd != null)
            {
                if (!(ddlProgramFlowNumber_Grd.SelectedIndex > 0))
                {
                    Utility.FunShowAlertMsg(this.Page, "Select the Program Flow Number");
                    return;
                }
            }
            string strModule_ID = ddlModuleCode_Grd.SelectedValue;
            string strModule_Name = ddlModuleCode_Grd.SelectedItem.Text;
            string strProgram_ID = ddlProgramID_Grd.SelectedValue;
            string strProgram_Name = ddlProgramID_Grd.SelectedItem.Text;
            string strProgram_Flow_ID = ddlProgramFlowNumber_Grd.SelectedValue;
            string strWokfFlow_Sequence_Name ="";
            if(!string.IsNullOrEmpty(txtWorkflowSequence.Text))
                strWokfFlow_Sequence_Name = txtWorkflowSequence.Text+strModule_ID + strProgram_Flow_ID;
            else
                strWokfFlow_Sequence_Name = strModule_ID  + strProgram_Flow_ID;

            if (strModule_ID.Equals("0"))
            {
                Utility.FunShowAlertMsg(this.Page, strSelectModule);
                ddlModuleCode_Grd.Focus();
                return;
            }

            if (Convert.ToInt32(strProgram_ID) == 0)
            {
                Utility.FunShowAlertMsg(this.Page, strSelectProgram);
                ddlProgramID_Grd.Focus();
                return;
            }
            else
            {

                //int qryExists = (from workflow in dtWorkflow.AsEnumerable()
                //                 where workflow.Field<string>("Module_ID") == strModule_ID.ToString()
                //                 && workflow.Field<string>("Program_ID") == strProgram_ID.ToString()
                //                 && workflow.Field<string>("Program_Flow_ID") == strProgram_Flow_ID.ToString()
                //                 select workflow).Count();

                // Linq has Commented to avoid exception while updating the record.

                if (FunIsExists(dtWorkflow, strProgram_ID,strProgram_Flow_ID,strModule_ID) > 1)
                {
                    Utility.FunShowAlertMsg(this.Page, strExistProgram);
                    ddlProgramID_Grd.Focus();
                    return;
                }
            }
            if (strProgram_Flow_ID.Equals("0"))
            {
                Utility.FunShowAlertMsg(this.Page, strSelectProgramFlowNumber);
                ddlProgramFlowNumber_Grd.Focus();
                return;
            }


            string strprogam = ddlProgramFlowNumber_Grd.SelectedItem.ToString();
            string strtable = string.Empty;
            if (dtWorkflow.Rows.Count == 1 && (dtWorkflow.Rows[0]["Program_Flow_ID"].ToString() == "0"))
            {
                if (strprogam.Substring(2, 1) != "0")
                {
                    Utility.FunShowAlertMsg(this.Page, "Program flow number should end with zero");
                    return;
                }
            }

            if (strprogam.Substring(2, 1) == "0")
            {
                for (int i = 0; i < dtWorkflow.Rows.Count; i++)
                {
                    if (dtWorkflow.Rows[i]["Program_Flow_ID"].ToString() != "0")
                    {
                        strtable = dtWorkflow.Rows[i]["Program_Flow_ID"].ToString();
                        if (strtable.Substring(2, 1) == "0")
                        {
                            Utility.FunShowAlertMsg(this.Page, "Program flow number cannot have values ending with zero other than flow number");
                            return;
                        }
                    }
                    
                }
            }

            foreach (GridViewRow gr in gvWorkFlowSequence.Rows)
            {
                //Label lb  lProFlow = (Label)gr.FindControl("lblProgramFlowNumber");                

                //string[] str_Product = txtRowproduct.Text.Split('-');
                //string[] str_ddlProduct = ddlProduct.SelectedItem.Text.Split('-');
                //if (str_Product[0].ToString().Trim().ToLower() == str_DDLProduct[0].ToString().Trim().ToLower())
                //{

                //if (ddlLob.SelectedItem.Text == txtRowlob.Text && str_Product[0].ToString().Trim().ToLower() == str_ddlProduct[0].ToString().Trim().ToLower())
                //    ddlProduct.SelectedItem.Text == txtRowproduct.Text)
                //{
                    //cvGlobal.ErrorMessage = "Selected Line of Business and Product already exists";
                    //cvGlobal.IsValid = false;
                //    Utility.FunShowAlertMsg(this.Page, "Selected Line of Business and Product already exists");
                //    return;
                //}
            }

            int intPrev_Id = 0, intCurrent_Id;

            intPrev_Id = Convert.ToInt32(dtWorkflow.Rows[dtWorkflow.Rows.Count - 1]["Program_Flow_ID"].ToString());
            if (Convert.ToInt32(strProgram_Flow_ID) <= intPrev_Id)
            {
                Utility.FunShowAlertMsg(this.Page, "Program Flow Number should be in sequence.");
                ddlProgramFlowNumber_Grd.Focus();
                return;
            }

            //Convert.ToInt32(dtWorkflow.Rows[dtWorkflow.Rows.Count - 1]["WorkFlow_ID"]) + 1;
            dtWorkflow = FunPriGetWorkflowDataTable(strModule_ID, strModule_Name, strProgram_ID, strProgram_Name, strProgram_Flow_ID, strWokfFlow_Sequence_Name);
            if (dtWorkflow.Rows.Count > 1)
            {
                if (dtWorkflow.Rows[0]["WorkFlow_ID"].ToString() == "0")
                {
                    dtWorkflow.Rows.RemoveAt(0);
                }
            }
            FunPubBindWorkflowSequence(dtWorkflow);

            /*if (dtWorkflow.Rows[0]["WorkFlow_ID"].ToString() == string.Empty && intWorkflowId == 0)
            {
                dtWorkflow.Rows[0].Delete();
            }*/
        }
    }
    protected void gvWorkFlowSequence_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvWorkFlowSequence.ShowFooter = false;
        gvWorkFlowSequence.EditIndex = e.NewEditIndex;
        dtWorkflow = (DataTable)ViewState["ListValues"];
        FunPubBindWorkflowSequence(dtWorkflow);

        //DropDownList ddlProgramID_Grd = (DropDownList)gvWorkFlowSequence.Rows[e.NewEditIndex].FindControl("ddlProgramID_Grd");        
        

        DataRow drEditRow = dtWorkflow.Rows[e.NewEditIndex];

        DropDownList ddlModuleCode_Grd = (DropDownList)gvWorkFlowSequence.Rows[e.NewEditIndex].FindControl("ddlModuleCode_Grd");
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        ddlModuleCode_Grd.BindDataTable(SPNames.SYS_ModuleMaster, Procparam, new string[] { "Module_Code", "Module_Code", "Module_Name" });
        Procparam = null;
        ddlModuleCode_Grd.SelectedValue = drEditRow["Module_ID"].ToString();
        
        DropDownList ddlProgramID_Grd = (DropDownList)gvWorkFlowSequence.Rows[e.NewEditIndex].FindControl("ddlProgramID_Grd");
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@Module_Code", ddlModuleCode_Grd.SelectedValue);
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        ddlProgramID_Grd.BindDataTable(SPNames.SYS_ProgramMaster, Procparam, new string[] { "Program_ID", "Program_Name" });
        Procparam = null;
        //ddlProgramCode(Convert.ToString(ddlModuleCode_Grd.SelectedValue));
        ddlProgramID_Grd.SelectedValue = Convert.ToInt32(drEditRow["Program_ID"]).ToString();
        
        DropDownList ddlProgramFlowNumber_Grd = (DropDownList)gvWorkFlowSequence.Rows[e.NewEditIndex].FindControl("ddlProgramFlowNumber_Grd");
        Procparam = new Dictionary<string, string>();
        //Procparam.Add("@Program_Ref_No", ddlProgramID_Grd.SelectedItem.Text.Split('-')[0].Trim());
        Procparam.Add("@Program_Ref_No", ddlProgramID_Grd.SelectedValue);
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        ddlProgramFlowNumber_Grd.BindDataTable("S3G_Get_ProgramWFPrgMapping", Procparam, new string[] { "Workflow_Prg_ID", "Workflow_Prg_ID" });
        Procparam = null;

        ddlProgramFlowNumber_Grd.SelectedValue = Convert.ToInt32(drEditRow["Program_Flow_ID"]).ToString();
        gvWorkFlowSequence.Columns[gvWorkFlowSequence.Columns.Count - 1].Visible = false;
    }
    protected void gvWorkFlowSequence_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvWorkFlowSequence.EditIndex = -1;
        gvWorkFlowSequence.ShowFooter = true;
        dtWorkflow = (DataTable)ViewState["ListValues"];
        FunPubBindWorkflowSequence(dtWorkflow);
        gvWorkFlowSequence.Columns[gvWorkFlowSequence.Columns.Count - 1].Visible = true;
    }
    protected void gvWorkFlowSequence_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if (!(ddlLOB.SelectedIndex > 0))
        {
            Utility.FunShowAlertMsg(this.Page, "Select the Line of Business");
            return;
        }
        if (!(ddlProductCode.SelectedIndex > 0))
        {
            Utility.FunShowAlertMsg(this.Page, "Select the Product");
            return;
        }
        
        dtWorkflow = (DataTable)ViewState["ListValues"];

        GridViewRow gvRow = gvWorkFlowSequence.Rows[e.RowIndex];

        DropDownList ddlModuleCode_Grd = (DropDownList)gvRow.FindControl("ddlModuleCode_Grd");
        DropDownList ddlProgramID_Grd = (DropDownList)gvRow.FindControl("ddlProgramID_Grd");
        DropDownList ddlProgramFlowNumber_Grd = (DropDownList)gvRow.FindControl("ddlProgramFlowNumber_Grd");
        if (ddlModuleCode_Grd != null)
        {
            if (!(ddlModuleCode_Grd.SelectedIndex > 0))
            {
                Utility.FunShowAlertMsg(this.Page, "Select the Module");
                return;
            }
        }
        if (ddlProgramID_Grd != null)
        {
            if (!(ddlProgramID_Grd.SelectedIndex > 0))
            {
                Utility.FunShowAlertMsg(this.Page, "Select the Program");
                return;
            }
        }
        if (ddlProgramFlowNumber_Grd != null)
        {
            if (!(ddlProgramFlowNumber_Grd.SelectedIndex > 0))
            {
                Utility.FunShowAlertMsg(this.Page, "Select the Program Flow Number");
                return;
            }
        }
        string strModule_ID = ddlModuleCode_Grd.SelectedValue;
        string strModule_Name = ddlModuleCode_Grd.SelectedItem.Text;
        string strProgram_ID = ddlProgramID_Grd.SelectedValue;
        string strProgram_Name = ddlProgramID_Grd.SelectedItem.Text;
        string strProgram_Flow_ID = ddlProgramFlowNumber_Grd.SelectedValue;
        string strWokfFlow_Sequence_Name = "";
        string strprogam = ddlProgramFlowNumber_Grd.SelectedItem.ToString();
        if (!string.IsNullOrEmpty(txtWorkflowSequence.Text))
            strWokfFlow_Sequence_Name = txtWorkflowSequence.Text + strModule_ID + strProgram_Flow_ID;
        else
            strWokfFlow_Sequence_Name = strModule_ID + strProgram_Flow_ID;

      /*  if(!string.IsNullOrEmpty(txtWorkflowSequence.Text))
            strWokfFlow_Sequence_Name = txtWorkflowSequence.Text+strModule_ID + strProgram_Name.Split('-')[0].ToString().Trim() + strProgram_Flow_ID;
        else
            strWokfFlow_Sequence_Name = strModule_ID + strProgram_Name.Split('-')[0].ToString().Trim() + strProgram_Flow_ID;*/
        //txtSequenceID.Text = strWokfFlow_Sequence_Name;

        if (e.RowIndex == 0)
        {
            if (strprogam.Substring(2, 1) != "0")
            {
                Utility.FunShowAlertMsg(this.Page, "Program flow number should end with zero");
                return;
            }
        }
        else
        {
            if (strprogam.Substring(2, 1) == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "Program flow number cannot have values ending with zero other than first flow number");
                return;
            }
        }
      
        if (strModule_ID.Equals("0"))
        {
            Utility.FunShowAlertMsg(this.Page, strSelectModule);
            ddlModuleCode_Grd.Focus();
            return;
        }

        if (Convert.ToInt32(strProgram_ID) == 0)
        {
            Utility.FunShowAlertMsg(this.Page, strSelectProgram);
            ddlProgramID_Grd.Focus();
            return;
        }
        else
        {



            //int qryExists = (from workflow in dtWorkflow.AsEnumerable()
            //                 where workflow.Field<string>("Module_ID") == strModule_ID.ToString()
            //                 && workflow.Field<string>("Program_ID") == strProgram_ID.ToString()
            //                 && workflow.Field<string>("Program_Flow_ID") == strProgram_Flow_ID.ToString()
            //                 select workflow).Count();
            //if (qryExists > 1)
            if(FunIsExists(dtWorkflow,strProgram_ID,strProgram_Flow_ID,strModule_ID) > 1)
            {
                Utility.FunShowAlertMsg(this.Page, strExistProgram);
                ddlProgramID_Grd.Focus();
                return;
            }
        }
        if (strProgram_Flow_ID.Equals("0"))
        {
            Utility.FunShowAlertMsg(this.Page, strSelectProgramFlowNumber);
            ddlProgramFlowNumber_Grd.Focus();
            return;
        }


        int intPrev_Id = 0, intCurrent_Id;
        if (e.RowIndex != 0)
            intPrev_Id = Convert.ToInt32(dtWorkflow.Rows[e.RowIndex - 1]["Program_Flow_ID"].ToString());
        if (e.RowIndex == dtWorkflow.Rows.Count - 1)
        {
            intCurrent_Id = 0;
        }
        else
        {
            intCurrent_Id = Convert.ToInt32(dtWorkflow.Rows[e.RowIndex + 1]["Program_Flow_ID"].ToString());
        }
        if (Convert.ToInt32(strProgram_Flow_ID) <= intPrev_Id)
        {
            Utility.FunShowAlertMsg(this.Page, "Program Flow Number should be in sequence.");
            ddlProgramFlowNumber_Grd.Focus();
            return;
        }
        if (intCurrent_Id > 0)
        {
            if (Convert.ToInt32(strProgram_Flow_ID) >= intCurrent_Id)
            {
                Utility.FunShowAlertMsg(this.Page, "Program Flow Number should be in sequence.");
                ddlProgramFlowNumber_Grd.Focus();
                return;
            }
        }
        DataRow drow = dtWorkflow.Rows[e.RowIndex];
        drow.BeginEdit();
        drow["Module_ID"] = strModule_ID;
        drow["Module_Name"] = strModule_Name;
        drow["Program_ID"] = strProgram_ID;
        drow["Program_Name"] = strProgram_Name;
        drow["Program_Flow_ID"] = strProgram_Flow_ID;
        drow["WokfFlow_Sequence_Name"] = strWokfFlow_Sequence_Name;
        drow.EndEdit();
        ViewState["ListValues"] = dtWorkflow;
        gvWorkFlowSequence.EditIndex = -1;
        gvWorkFlowSequence.ShowFooter = true;
        dtWorkflow = (DataTable)ViewState["ListValues"];
        FunPubBindWorkflowSequence(dtWorkflow);
        gvWorkFlowSequence.Columns[gvWorkFlowSequence.Columns.Count - 1].Visible = true;
    }
    protected void gvWorkFlowSequence_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataRow drEmptyRow_Deleted;

        //if (e.RowIndex >= 0)
        //{
        //int intWorkFlow_ID = Convert.ToInt32(gvWorkFlowSequence.DataKeys[e.RowIndex]["WorkFlow_ID"]);
        
        dtWorkflow = (DataTable)ViewState["ListValues"];
        string strWorkFlowSeq = txtWorkflowSequence.Text;
        Procparam = new Dictionary<string, string>();
        // Procparam.Add("@Program_Ref_No", ddlProgramID_Grd.SelectedItem.Text.Split('-')[0].ToString().Trim());
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@WorkFlowSeq", strWorkFlowSeq.ToString());
        if (strWorkFlowSeq == Utility.GetTableScalarValue("S3G_SYSAD_CheckWorkFlowExist", Procparam))
        {
            Utility.FunShowAlertMsg(this, "Cannot delete Workflow already defined");
            return;
        }


        dtWorkflow.Rows.RemoveAt(e.RowIndex);
        ViewState["ListValues"] = dtWorkflow;

        if (dtWorkflow.Rows.Count == 0)
        {
            dtWorkflow = FunPriGetWorkflowDataTable("0", "0", "0", "0", "0", "0");
           
        }
        else
        {
            dtWorkflow = (DataTable)ViewState["ListValues"];
        }
        FunPubBindWorkflowSequence(dtWorkflow);
        gvWorkFlowSequence.ShowFooter = true;

        /*
        if (ViewState["WorkflowDeleted"] == null)
        {
            dtWorkflowDeleted = dtWorkflow.Clone();
        }
        else if (ViewState["WorkflowDeleted"] != null)
        {
            dtWorkflowDeleted = (DataTable)ViewState["WorkflowDeleted"];
        }
        drEmptyRow_Deleted = dtWorkflowDeleted.NewRow();
        drEmptyRow_Deleted = dtWorkflow.Rows[e.RowIndex];
        ViewState["WorkflowDeleted"] = dtWorkflowDeleted;
        */


        /////////////////


        /////////////////

        //}
        /*
        ViewState["ListValues"] = dtWorkflow;


        drEmptyRow = dtWorkflow.NewRow();
        drEmptyRow["WorkFlow_ID"] = Convert.ToInt32(dtWorkflow.Rows[dtWorkflow.Rows.Count - 1]["WorkFlow_ID"]) + 1;
        drEmptyRow["Module_ID"] = strModule_ID;
        drEmptyRow["Module_Name"] = strModule_Name;
        drEmptyRow["Program_ID"] = strProgram_ID;
        drEmptyRow["Program_Name"] = strProgram_Name;
        drEmptyRow["Program_Flow_ID"] = strProgram_Flow_ID;
        drEmptyRow["WokfFlow_Sequence_Name"] = strWokfFlow_Sequence_Name;
        dtWorkflow.Rows.Add(drEmptyRow);

        if (dtWorkflow.Rows[0]["WorkFlow_ID"].Equals("0"))
        {
            dtWorkflow.Rows[0].Delete();
            //gvWorkFlowSequence.Rows[0].Visible = false;
        }
        ViewState["WorkflowDeleted"] = dtWorkflowDeleted;
        */




        //gvWorkFlowSequence.DeleteRow(e.RowIndex);
    }
    protected void ddlProgramID_Grd_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlProgramID_Grd;
        DropDownList ddlProgramFlowNumber_Grd;
        if (gvWorkFlowSequence.FooterRow.Visible)
        {
            ddlProgramID_Grd = (DropDownList)gvWorkFlowSequence.FooterRow.FindControl("ddlProgramID_Grd");
            ddlProgramFlowNumber_Grd = (DropDownList)gvWorkFlowSequence.FooterRow.FindControl("ddlProgramFlowNumber_Grd");
        }
        else
        {
            ddlProgramID_Grd = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlProgramID_Grd.Parent.Parent;
            ddlProgramFlowNumber_Grd = (DropDownList)gvWorkFlowSequence.Rows[gvRow.RowIndex].FindControl("ddlProgramFlowNumber_Grd");
        }
        //DropDownList ddlProgramID_Grd = (DropDownList)gvWorkFlowSequence.FooterRow.FindControl("ddlProgramID_Grd");
        Procparam = new Dictionary<string, string>();
       // Procparam.Add("@Program_Ref_No", ddlProgramID_Grd.SelectedItem.Text.Split('-')[0].ToString().Trim());
        Procparam.Add("@Program_Ref_No", ddlProgramID_Grd.SelectedValue);
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        ddlProgramFlowNumber_Grd.BindDataTable("S3G_Get_ProgramWFPrgMapping", Procparam, new string[] { "Workflow_Prg_ID", "Workflow_Prg_ID" });
        Procparam = null;
       // ddlProgramID();
    }

    //private void ddlProgramID()
    //{
    //    DropDownList ddlProgramFlowNumber_Grd = (DropDownList)gvWorkFlowSequence.FooterRow.FindControl("ddlProgramFlowNumber_Grd");

    //    DropDownList ddlProgramID_Grd = (DropDownList)gvWorkFlowSequence.FooterRow.FindControl("ddlProgramID_Grd");
    //    Procparam = new Dictionary<string, string>();
    //    Procparam.Add("@Program_Ref_No", ddlProgramID_Grd.SelectedItem.Text.Split('-')[0].ToString().Trim());
    //    ddlProgramFlowNumber_Grd.BindDataTable("S3G_Get_ProgramWFPrgMapping", Procparam, new string[] { "Workflow_Prg_ID", "Workflow_Prg_ID" });
    //    Procparam = null;
    //}

    //--Added by Guna on 13-Oct-2010 to select the Program based on the module selection
    protected void ddlModuleCode_Grd_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlModuleCode_Grd;
       
        if (gvWorkFlowSequence.FooterRow.Visible)
        {
           ddlModuleCode_Grd = (DropDownList)gvWorkFlowSequence.FooterRow.FindControl("ddlModuleCode_Grd");
           ddlProgramCode(Convert.ToString(ddlModuleCode_Grd.SelectedValue));
        }
        else
        {
            DropDownList ddlProgramID_Grd;
            ddlModuleCode_Grd = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlModuleCode_Grd.Parent.Parent;
            ddlProgramID_Grd = (DropDownList)gvWorkFlowSequence.Rows[gvRow.RowIndex].FindControl("ddlProgramID_Grd");
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Module_Code", Convert.ToString(ddlModuleCode_Grd.SelectedValue));
            ddlProgramID_Grd.BindDataTable(SPNames.SYS_ProgramMaster, Procparam, new string[] { "Program_ID", "Program_Name" });  
        }
           
       
    }

    private void ddlProgramCode(string  intModulecode)
    {
        DropDownList ddlProgramID_Grd = (DropDownList)gvWorkFlowSequence.FooterRow.FindControl("ddlProgramID_Grd");        
        DropDownList ddlModuleCode_Grd = (DropDownList)gvWorkFlowSequence.FooterRow.FindControl("ddlModuleCode_Grd");
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@Module_Code", intModulecode.ToString());
        ddlProgramID_Grd.BindDataTable(SPNames.SYS_ProgramMaster, Procparam, new string[] { "Program_ID", "Program_Name" });        
        Procparam = null;
    }
    //--Ends here
    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            strMode = Request.QueryString.Get("qsMode");
            if (fromTicket != null)
            {
                intWorkflowId = Convert.ToInt32(fromTicket.Name);
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }

        }

        if (!IsPostBack)
        {
            dtWorkflow = FunPriGetWorkflowDataTable("0", "0", "0", "0", "0", "0");
            FunPubBindWorkflowSequence(dtWorkflow);

            FunPubQueryCompanyCode();
            FunPriBindLOBModuleProduct();
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            if (((intWorkflowId > 0)) && (strMode == "M"))
            {
                FunPriDisableControls(1);
            }

            else if (((intWorkflowId > 0)) && (strMode == "Q"))
            {
                FunPriDisableControls(-1);
            }
            else
            {
                FunPriDisableControls(0);
            }

        }
    }
    private void FunPubQueryCompanyCode()
    {
        try
        {
            string strCompanyName = string.Empty;
            string strCompanyCode = null;
            strCompanyCode = ObjWorkflowMasterClient.FunPubGetCompanyCode(intCompanyID, strCompanyCode);
            hdnCompanyCode.Value = strCompanyCode;
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }
    /// <summary>
    /// This is used to bind lob,product,module and program details in dropdownlist
    /// </summary>
    private void FunPriBindLOBModuleProduct()
    {
        try
        {

            //LOB List

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (intWorkflowId == 0)
            {
                Procparam.Add("@Is_Active", "1");
            }
            if (intWorkflowId == 0)
            {
                Procparam.Add("@User_ID", Convert.ToString(intUserId));
            }
            Procparam.Add("@Program_ID", "17");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlProductCode.Items.Insert(0, new ListItem("--Select--", "0"));
            
            
          /*  //Module Master
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Is_Active", "1");
            ddlModuleCode.BindDataTable(SPNames.SYS_ModuleMaster, Procparam, new string[] { "Module_Code", "Module_Code", "Module_Name" });

            //Program Master
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Is_Active", "1");
            ddlProgramID.BindDataTable(SPNames.SYS_ProgramMaster, Procparam, new string[] { "Program_ID", "Program_Ref_No", "Program_Name" });
            */
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }
    protected void ddlLOB_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriLoadProduct(Convert.ToInt32(ddlLOB.SelectedValue));
        if (ViewState["ListValues"] != null)
        {
            dtWorkflow = (DataTable)ViewState["ListValues"];
            if (!(dtWorkflow.Rows[0]["WorkFlow_ID"].Equals("0")))
            {
                ViewState["ListValues"] = null;
                dtWorkflow = FunPriGetWorkflowDataTable("0", "0", "0", "0", "0", "0");
                gvWorkFlowSequence.DataSource = dtWorkflow;
                gvWorkFlowSequence.DataBind();
                gvWorkFlowSequence.Rows[0].Visible = false;
            }
                        
          
        }
        

    }
    private void FunPriLoadProduct(int intLOB_ID)
    {
        //Product Code
        Procparam = new Dictionary<string, string>();
        //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        Procparam.Add("@LOB_ID", intLOB_ID.ToString());
        if (intWorkflowId == 0)
        {
            Procparam.Add("@Is_Active", "1");
        }

        ddlProductCode.BindDataTable("S3G_ORG_GetLOBProductList", Procparam, new string[] { "Code", "Code", "Product_Name" });
        string strCode = ddlLOB.SelectedItem.Text;
        hdnLOB.Value = strCode.Substring(0, strCode.LastIndexOf("  -"));
        txtWorkflowSequence.Text = "";
    }
    /// <summary>
    /// This is used to get workflow sequence based on company,lob,product,module and program
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void ddlProgramID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if ((ddlLOB.SelectedIndex > 0) && (ddlProductCode.SelectedIndex > 0) && (ddlModuleCode.SelectedIndex > 0) && (ddlProgramID.SelectedIndex > 0))
    //        txtWorkflowSequence.Text = hdnCompanyCode.Value + " - " + ddlLOB.SelectedValue + " - " + ddlProductCode.SelectedValue + " - " + ddlModuleCode.SelectedValue + " - " + ddlProgramID.SelectedItem.Text.Substring(0, 3);
    //}
    #endregion

    #region Page Events

    /// <summary>
    /// This is used to save workflow details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (intWorkflowId == 0)
            {
                if (!FunPriGenerateWorkflowXMLDet())
                {
                    return;
                }
            }
            else if (intWorkflowId > 0)
            {
                if (!FunPriGenerateWorkflowXMLDetailsModify())
                {
                    return;
                }
            }
            ObjS3G_SYSAD_WorkflowDataTable = new CompanyMgtServices.S3G_SYSAD_WorkflowDataTable();
            CompanyMgtServices.S3G_SYSAD_WorkflowRow ObjWorkflowRow;
            ObjWorkflowRow = ObjS3G_SYSAD_WorkflowDataTable.NewS3G_SYSAD_WorkflowRow();
            ObjWorkflowRow.Workflow_Sequence_ID = intWorkflowId;
            ObjWorkflowRow.Company_ID = intCompanyID;
            ObjWorkflowRow.LOB_Code = ddlLOB.SelectedValue;
            ObjWorkflowRow.Product_Code = ddlProductCode.SelectedValue;
            ObjWorkflowRow.Workflow_Sequnce = txtWorkflowSequence.Text;
            ObjWorkflowRow.XMLWorkflowParamDet = strXMLCreditScoreDet;
            ObjWorkflowRow.Is_Active = chkActive.Checked;
            ObjWorkflowRow.Created_By = intUserId;
            ObjS3G_SYSAD_WorkflowDataTable.AddS3G_SYSAD_WorkflowRow(ObjWorkflowRow);
            intErrCode = ObjWorkflowMasterClient.FunPubCreateWorkflow(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_WorkflowDataTable, SerMode));

            if (intErrCode == 0)
            {
                if (intWorkflowId > 0)
                {
                    if (FunCheckLobisvalid(ddlLOB.SelectedValue))
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here
                        Utility.FunShowAlertMsg(this.Page, "Work Flow details updated successfully", strRedirectPage);
                    }
                    else
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('You dont have rights fot the selected Line of Business')", true);
                    }
                }
                else
                {
                    strAlert = "Work Flow details added successfully";
                    strAlert += @"\n\nWould you like to add one more work flow?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                    //Utility.FunShowAlertMsg(this.Page, "Work Flow details added successfully", strRedirectPage);
                }
            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Work Flow already exists and active for the selected combination");
            }
            else if (intErrCode == 2)
            {
                Utility.FunShowAlertMsg(this.Page, "Work Flow Name already exist");
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + "window.location.href='" + strRedirectPage + "';", true);
            lblErrorMessage.Text = string.Empty;

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjWorkflowMasterClient.Close();
        }
    }
    /// <summary>
    /// This is used to redirect page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }
    /// <summary>
    /// This is used to clear data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        //ddlLOB.Enabled = true;
        //ddlProductCode.Enabled = true;
        //ddlLOB.SelectedIndex = 0;
        //ddlProductCode.SelectedIndex = 0;
        ////ddlModuleCode.SelectedIndex = 0;
        ////ddlProgramID.SelectedIndex = 0;
        //txtWorkflowSequence.Text = "";
        //chkActive.Checked = true;
        //if (ViewState["ListValues"] != null)
        //{
        //    gvWorkFlowSequence.EditIndex = -1;
        //    dtWorkflow = (DataTable)ViewState["ListValues"];
        //     ViewState["ListValues"] = null;
        //        dtWorkflow = FunPriGetWorkflowDataTable("0", "0", "0", "0", "0", "0");
        //        gvWorkFlowSequence.DataSource = dtWorkflow;
        //        gvWorkFlowSequence.DataBind();
                
        //        gvWorkFlowSequence.Rows[0].Visible = false;
            


        //} // Code commented by Rao on 27 Jan 2012 to fix clear button issue in Work Flow.

        Response.Redirect("../System Admin/S3GSysAdminWorkflow_Add.aspx");
    }
    #endregion

    #region Page Methods
    /// <summary>
    /// This method is used to display User details
    /// </summary>
    private void FunGetWorkflowDetails()
    {
        try
        {

            ObjS3G_SYSAD_WorkflowDataTable = new CompanyMgtServices.S3G_SYSAD_WorkflowDataTable();
            CompanyMgtServices.S3G_SYSAD_WorkflowRow ObjWorkflowRow;
            ObjWorkflowRow = ObjS3G_SYSAD_WorkflowDataTable.NewS3G_SYSAD_WorkflowRow();
            ObjWorkflowRow.Company_ID = intCompanyID;
            ObjWorkflowRow.Workflow_Sequence_ID = intWorkflowId;
            ObjS3G_SYSAD_WorkflowDataTable.AddS3G_SYSAD_WorkflowRow(ObjWorkflowRow);

            byte[] byteWorkflowDetails = ObjWorkflowMasterClient.FunPubQueryWorkflow(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_WorkflowDataTable, SerMode));
            ObjS3G_SYSAD_WorkflowDataTable = (CompanyMgtServices.S3G_SYSAD_WorkflowDataTable)ClsPubSerialize.DeSerialize(byteWorkflowDetails, SerMode, typeof(CompanyMgtServices.S3G_SYSAD_WorkflowDataTable));

            ddlLOB.SelectedValue = ObjS3G_SYSAD_WorkflowDataTable.Rows[0]["LOB_ID"].ToString();
            FunPriLoadProduct(Convert.ToInt32(ddlLOB.SelectedValue));


            ddlProductCode.SelectedValue = ObjS3G_SYSAD_WorkflowDataTable.Rows[0]["Product_Code"].ToString();
            txtWorkflowSequence.Text = ObjS3G_SYSAD_WorkflowDataTable.Rows[0]["Workflow_Sequnce_ID"].ToString();
            hdnID.Value = ObjS3G_SYSAD_WorkflowDataTable.Rows[0]["Created_By"].ToString();
            if (ObjS3G_SYSAD_WorkflowDataTable.Rows[0]["Is_Active"].ToString() == "True")
                chkActive.Checked = true;
            else
                chkActive.Checked = false;

            FunGetWorkflowProgramMappingDetails(intWorkflowId);

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjWorkflowMasterClient.Close();
        }
    }
    private void FunGetWorkflowProgramMappingDetails(int intWorkflowId)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Workflow_ID", Convert.ToString(intWorkflowId));
        dtWorkflow = Utility.GetDefaultData("S3G_WORKFLOW_ProgramMapping_Details", Procparam);
        ViewState["ListValues"] = dtWorkflow;
        FunPubBindWorkflowSequence(dtWorkflow);
    }
    #endregion
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode                
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                chkActive.Enabled = false;
                chkActive.Checked = true;

                break;

            case 1: // Modify Mode
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                FunGetWorkflowDetails();
                btnClear.Enabled = false;
                ddlLOB.Enabled = false;
                ddlProductCode.Enabled = false;
                //ddlModuleCode.Enabled = false;
                //ddlProgramID.Enabled = false;
                //FunGetWorkflowDetails();
                break;

            case -1:// Query Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                FunGetWorkflowDetails();
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                txtWorkflowSequence.Enabled = true;
                txtWorkflowSequence.ReadOnly = true;
                chkActive.Enabled = false;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                if (gvWorkFlowSequence != null)
                {
                    gvWorkFlowSequence.Columns[gvWorkFlowSequence.Columns.Count - 1].Visible = false;
                    gvWorkFlowSequence.Columns[gvWorkFlowSequence.Columns.Count - 2].Visible = false;
                }
                gvWorkFlowSequence.FooterRow.Visible = false;
                if (bClearList)
                {
                    ddlLOB.ClearDropDownList();
                    ddlProductCode.ClearDropDownList();
                    //ddlModuleCode.ClearDropDownList();
                    //ddlProgramID.ClearDropDownList();
                }
                break;
        }

    }
    private bool FunPriGenerateWorkflowXMLDet()
    {
        int intRowCount = 0;
        dtWorkflow = (DataTable)ViewState["ListValues"];
        if (dtWorkflow.Rows.Count == 1)
        {
            if (dtWorkflow.Rows[0]["WorkFlow_ID"].ToString().Equals("0"))
            {

                /*DropDownList ddlModuleCode_Grd = (DropDownList)gvWorkFlowSequence.FooterRow.FindControl("ddlModuleCode_Grd");
                DropDownList ddlProgramID_Grd = (DropDownList)gvWorkFlowSequence.FooterRow.FindControl("ddlProgramID_Grd");
                DropDownList ddlProgramFlowNumber_Grd = (DropDownList)gvWorkFlowSequence.FooterRow.FindControl("ddlProgramFlowNumber_Grd");

                string strModule_ID = ddlModuleCode_Grd.SelectedValue;
                string strModule_Name = ddlModuleCode_Grd.SelectedItem.Text;
                string strProgram_ID = ddlProgramID_Grd.SelectedValue;
                string strProgram_Name = ddlProgramID_Grd.SelectedItem.Text;
                string strProgram_Flow_ID = ddlProgramFlowNumber_Grd.SelectedValue;
                string strWokfFlow_Sequence_Name = strModule_ID + strProgram_Name.Split('-')[0].ToString().Trim() + strProgram_Flow_ID;

                if (strModule_ID.Equals("0"))
                {
                    Utility.FunShowAlertMsg(this.Page, strSelectModule);
                    ddlModuleCode_Grd.Focus();
                    return false;
                }

                if (Convert.ToInt32(strProgram_ID) == 0)
                {
                    Utility.FunShowAlertMsg(this.Page, strSelectProgram);
                    ddlProgramID_Grd.Focus();
                    return false;
                }
                if (strProgram_Flow_ID.Equals("0"))
                {
                    Utility.FunShowAlertMsg(this.Page, strSelectProgramFlowNumber);
                    ddlProgramFlowNumber_Grd.Focus();
                    return false;
                }*/
                Utility.FunShowAlertMsg(this.Page, "Add atleast one workflow sequence!");
                return false;
            }
        }
        strbCreditScoreDet.Append("<Root>");

        foreach (DataRow drow in dtWorkflow.Rows)
        {
            if (intRowCount == 0)
            {
                if (Convert.ToInt32(drow["Program_Flow_ID"].ToString()) % 10 != 0)
                {
                    Utility.FunShowAlertMsg(this.Page, strStartNumber);
                    return false;
                }
                intRowCount = 1;
            }
            strbCreditScoreDet.Append(" <Details ");
            strbCreditScoreDet.Append(" Workflow_Prg_Mapping_ID = '" + drow["WorkFlow_ID"].ToString() + "'");
            strbCreditScoreDet.Append(" Module_ID = '" + drow["Module_ID"].ToString() + "'");
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@ProgramflowName",drow["Program_ID"].ToString());
            DataTable dtnew=new DataTable();
            dtnew = Utility.GetDefaultData("S3G_GetProgramrefno_Details", Procparam);
            strbCreditScoreDet.Append(" Program_Ref_No = '" + dtnew.Rows[0]["Program_Ref_No"].ToString() + "'");
            strbCreditScoreDet.Append(" Workflow_Prg_ID= '" + drow["Program_Flow_ID"].ToString() + "'");
            strbCreditScoreDet.Append(" Workflow_Sequence= '" + drow["WokfFlow_Sequence_Name"].ToString() + "'");
            strbCreditScoreDet.Append(" RowState = 'N'");
            strbCreditScoreDet.Append(" />");
            dtnew.Dispose();
        }
        strbCreditScoreDet.Append("</Root>");
        strXMLCreditScoreDet = strbCreditScoreDet.ToString();
        return true;

    }
    private bool FunPriGenerateWorkflowXMLDetailsModify()
    {
        int intRowCount = 0;
        dtWorkflow = (DataTable)ViewState["ListValues"];
        if (dtWorkflow.Rows.Count == 1)
        {
            if (dtWorkflow.Rows[0]["WorkFlow_ID"].ToString().Equals("0"))
            {
                Utility.FunShowAlertMsg(this.Page, "Add atleast one workflow sequence!");
                return false;
            }
        }
        strbCreditScoreDet.Append("<Root>");

        foreach (DataRow drow in dtWorkflow.Rows)
        {
            if (intRowCount == 0)
            {
                if (Convert.ToInt32(drow["Program_Flow_ID"].ToString()) % 10 != 0)
                {
                    Utility.FunShowAlertMsg(this.Page, strStartNumber);
                    return false;
                }
                intRowCount = 1;
            }


            strbCreditScoreDet.Append(" <Details ");
            strbCreditScoreDet.Append(" Workflow_Prg_Mapping_ID = '" + drow["WorkFlow_ID"].ToString() + "'");
            strbCreditScoreDet.Append(" Module_ID = '" + drow["Module_ID"].ToString() + "'");
            //strbCreditScoreDet.Append(" Program_Ref_No = '" + drow["Program_Name"].ToString().Split('-')[0].ToString().Trim() + "'");
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@ProgramflowName", drow["Program_ID"].ToString());
            DataTable dtnew = new DataTable();
            dtnew = Utility.GetDefaultData("S3G_GetProgramrefno_Details", Procparam);
            strbCreditScoreDet.Append(" Program_Ref_No = '" + dtnew.Rows[0]["Program_Ref_No"].ToString() + "'");
            strbCreditScoreDet.Append(" Workflow_Prg_ID= '" + drow["Program_Flow_ID"].ToString() + "'");
            strbCreditScoreDet.Append(" Workflow_Sequence= '" + drow["WokfFlow_Sequence_Name"].ToString() + "'");
            if (drow.RowState == DataRowState.Added)
            {
                strbCreditScoreDet.Append(" RowState = 'N'");
            }
            else if (drow.RowState == DataRowState.Modified)
            {
                strbCreditScoreDet.Append(" RowState = 'M'");
            }
            else if (drow.RowState == DataRowState.Deleted)
            {
                strbCreditScoreDet.Append(" RowState = 'D'");
            }
            else if (drow.RowState == DataRowState.Unchanged)
            {
                strbCreditScoreDet.Append(" RowState = 'O'");
            }


            strbCreditScoreDet.Append(" />");
        }
        strbCreditScoreDet.Append("</Root>");
        strXMLCreditScoreDet = strbCreditScoreDet.ToString();
        return true;
    }

    private bool FunCheckLobisvalid(string strLobId)
    {
        Dictionary<string, string> Procparm = new Dictionary<string, string>();
        Procparm.Add("@Company_ID", intCompanyID.ToString());
        Procparm.Add("@User_Id", intUserId.ToString());
        Procparm.Add("@LOB_ID", strLobId);
        Procparm.Add("@Program_ID", "17");          // Added by Senthil 12/Oct/2011
        if (intWorkflowId == 0)
        {
            Procparm.Add("@Is_Active", "1");
        }
        Procparm.Add("@Is_UserLobActive", "1");
        DataTable dtBool = Utility.GetDefaultData("S3G_GetUserLobMapping", Procparm);
        if (dtBool.Rows[0]["EXISTS"].ToString() == "1")
            return true;
        else
            return false;

    }


    private int FunIsExists(DataTable dt, string PrgId, string Flow_ID, string Module)
    { 
        //int qryExists = (from workflow in dtWorkflow.AsEnumerable()
        //                     where workflow.Field<string>("Module_ID") == strModule_ID.ToString()
        //                     && workflow.Field<string>("Program_ID") == strProgram_ID.ToString()
        //                     && workflow.Field<string>("Program_Flow_ID") == strProgram_Flow_ID.ToString()
        //                     select workflow).Count();

        DataView dv = new DataView();
        string strCon = "Module_ID='" + Module + "' and Program_ID=" + PrgId + " and Program_Flow_ID=" + Flow_ID;
        dv = new DataView(dt, strCon, "", DataViewRowState.CurrentRows);

        return dv.Count;

    }

}



