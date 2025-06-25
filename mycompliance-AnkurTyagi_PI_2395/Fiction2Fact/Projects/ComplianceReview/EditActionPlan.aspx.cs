using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact.Projects.ComplianceReview
{
    public partial class EditActionPlan : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        RefCodesBLL refBLL = new RefCodesBLL();
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();
        string script = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
            if (Page.User.Identity.Name.Equals(""))
            {
                Response.Redirect(Global.site_url("Login.aspx"));
                return;
            }
            //>>

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ActionPlanId"]))
                {
                    hfId.Value = Request.QueryString["ActionPlanId"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["IssueId"]))
                {
                    hfIssueId.Value = Request.QueryString["IssueId"];
                }
                FillBusinessUnits();
                ddlTypeofAction.DataSource = refBLL.getRefCodeDetails("Compliance Review Issue Common Action - Action Type", mstrConnectionString);
                ddlTypeofAction.DataBind();
                ddlTypeofAction.Items.Insert(0, new ListItem("-- Select --", ""));

                ddlActionPlanStatus.DataSource = refBLL.getRefCodeDetails("Compliance Review Issue Common Action - Action Status", mstrConnectionString);
                ddlActionPlanStatus.DataBind();
                ddlActionPlanStatus.Items.Insert(0, new ListItem("-- Select --", ""));
                ddlActionPlanStatus.Attributes["onchange"] = "onStatusChanges()";
                FillActionPlanDetails();
            }
        }
        void FillBusinessUnits()
        {
            DataTable dt = new DataTable();
            dt = oBLL.Search_SubFunction_Master(0, null,strCSFM_Status: "A");
            if (dt.Rows.Count > 0)
            {
                ddlUnitId.DataSource = dt;
            }
            else
            {
                ddlUnitId.DataSource = null;
            }
            ddlUnitId.DataBind();
            ddlUnitId.Items.Insert(0, new ListItem("--Select--", ""));
        }
        void FillActionPlanDetails()
        {
            DataTable dt = new DataTable();
            dt = oBLL.getIssueActions(Convert.ToInt32(hfId.Value), 0, null, null);
            if (dt.Rows.Count > 0)
            {
                hfRefId.Value = dt.Rows[0]["CIA_CI_ID"].ToString();
                ddlUnitId.SelectedValue = dt.Rows[0]["CIA_SFM_ID"].ToString();
                txtMitigationPlan.Text = dt.Rows[0]["CIA_ACTIONABLE"].ToString();
                ddlTypeofAction.SelectedValue = dt.Rows[0]["CIA_ACT_TYPE"].ToString();
                txtRemarks.Text = dt.Rows[0]["CIA_REMARKS"].ToString();
                ddlActionPlanStatus.SelectedValue = dt.Rows[0]["CIA_ACTIONABLE_STATUS"].ToString();
                hfRecStatus.Value = dt.Rows[0]["CIA_STATUS"].ToString();
                txtPersonResponsible.Text = dt.Rows[0]["CIA_SPECIFIED_PERSON_ID"].ToString();

                string strSPOCReponsibleId = (dt.Rows[0]["CIA_SPECIFIED_PERSON_ID"] is DBNull ? "" : dt.Rows[0]["CIA_SPECIFIED_PERSON_ID"].ToString());
                string strSPOCReponsibleName = (dt.Rows[0]["CIA_SPECIFIED_PERSON_NAME"] is DBNull ? "" : dt.Rows[0]["CIA_SPECIFIED_PERSON_NAME"].ToString());
                string strSPOCReponsibleEmail = (dt.Rows[0]["CIA_SPECIFIED_PERSON_EMAIL"] is DBNull ? "" : dt.Rows[0]["CIA_SPECIFIED_PERSON_EMAIL"].ToString());
                hfResponsiblePersonId.Value = strSPOCReponsibleId + "|" + strSPOCReponsibleName + "|" + strSPOCReponsibleEmail;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "myalert", "onStatusChanges();", true);

                if (!string.IsNullOrEmpty(dt.Rows[0]["CIA_TARGET_DT"].ToString()))
                {
                    txtTargetDate.Text = Convert.ToDateTime(dt.Rows[0]["CIA_TARGET_DT"].ToString()).ToString("dd-MMM-yyyy");
                }
                if (!string.IsNullOrEmpty(dt.Rows[0]["CIA_CLOSURE_DT"].ToString()))
                {
                    txtClosureDate.Text = Convert.ToDateTime(dt.Rows[0]["CIA_CLOSURE_DT"].ToString()).ToString("dd-MMM-yyyy");
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfId.Value))
            {
                try
                {

                    #region For Updation
                    int intCIA_ID = Convert.ToInt32(hfId.Value), intCIA_CI_ID, intCIA_SFM_ID;
                    string strCIA_REF_NO = "", strCIA_ACTIONABLE = "", strCIA_DETAILED_DESC = "", strCIA_ACT_TYPE = "",
                    strCIA_CRITICALITY = "", strCIA_STATUS = "", strCIA_CREATOR_NAME = "", strCIA_CREATOR_NT_ID = "", strCIA_CLOSURE_BY = "",
                    strCIA_CLOSURE_REMARKS = "", strCIA_REMARKS, strCIA_CREATE_BY = "", strCIA_UPDATE_BY = "", strCIA_ACTIONABLE_STATUS = "",
                    strCIA_REVISED_MAP = "", strCIA_SPECIFIED_PERSON_NAME = "", strCIA_SPECIFIED_PERSON_EMAIL = "", intCIA_SPECIFIED_PERSON_ID = "";
                    DateTime? strCIA_TARGET_DT = new DateTime?(), strCIA_REVISED_TARGET_DT = new DateTime?(), strCIA_CLOSURE_DT = new DateTime?();


                    intCIA_SPECIFIED_PERSON_ID = hfResponsiblePersonId.Value.Split('|')[0];
                    strCIA_SPECIFIED_PERSON_NAME = hfResponsiblePersonId.Value.Split('|')[1];
                    strCIA_SPECIFIED_PERSON_EMAIL = hfResponsiblePersonId.Value.Split('|')[2];

                    intCIA_CI_ID = Convert.ToInt32(hfIssueId.Value);
                    intCIA_SFM_ID = Convert.ToInt32(ddlUnitId.SelectedItem.Value);
                    strCIA_ACTIONABLE = txtMitigationPlan.Text;
                    strCIA_ACT_TYPE = ddlTypeofAction.SelectedItem.Value;
                    strCIA_CREATOR_NAME = Page.User.Identity.Name;
                    strCIA_REMARKS = txtRemarks.Text;
                    strCIA_CREATE_BY = Page.User.Identity.Name;
                    strCIA_UPDATE_BY = Page.User.Identity.Name;
                    strCIA_ACTIONABLE_STATUS = ddlActionPlanStatus.SelectedItem.Value;

                    if (!string.IsNullOrEmpty(txtTargetDate.Text))
                    {
                        strCIA_TARGET_DT = Convert.ToDateTime(txtTargetDate.Text);
                    }

                    if (!string.IsNullOrEmpty(txtClosureDate.Text))
                    {
                        strCIA_CLOSURE_DT = Convert.ToDateTime(txtClosureDate.Text);
                    }

                    int x = oBLL.saveIssueUpdate(intCIA_ID, intCIA_CI_ID, strCIA_REF_NO,
                intCIA_SFM_ID, strCIA_ACTIONABLE, strCIA_DETAILED_DESC, strCIA_ACT_TYPE,
                strCIA_CRITICALITY, strCIA_STATUS, strCIA_CREATOR_NAME, strCIA_CREATOR_NT_ID,
                strCIA_TARGET_DT, strCIA_CLOSURE_BY, strCIA_CLOSURE_DT, strCIA_CLOSURE_REMARKS, strCIA_REMARKS,
                strCIA_CREATE_BY, strCIA_UPDATE_BY, strCIA_ACTIONABLE_STATUS, strCIA_REVISED_TARGET_DT, strCIA_REVISED_MAP,
                Convert.ToString(intCIA_SPECIFIED_PERSON_ID), strCIA_SPECIFIED_PERSON_NAME, strCIA_SPECIFIED_PERSON_EMAIL);
                    if (x > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MYalert", "alert('Action plan updated successfully.');onCloseClick();", true);
                    }
                    #endregion


                    ClearControls();
                }
                catch (System.Exception ex)
                {
                    string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
                }
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }
        protected void cvTargetDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        protected void cvClosureDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
        void ClearControls()
        {
            txtClosureDate.Text = string.Empty;
            txtMitigationPlan.Text = string.Empty;
            txtPersonResponsible.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtTargetDate.Text = string.Empty;
            ddlActionPlanStatus.SelectedIndex = 0;
            ddlTypeofAction.SelectedIndex = 0;
            ddlUnitId.SelectedIndex = 0;
            hfResponsiblePersonId.Value = string.Empty;
            hfSelectedOperation.Value = string.Empty;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MYalert", "onCloseClick();", true);
        }
    }
}