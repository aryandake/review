using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code;
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
    public partial class ActionableList : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        RefCodesBLL refBL = new RefCodesBLL();
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillStatus();
                FillGrid();
            }
        }
        void FillStatus()
        {
            ddlStatus.DataSource = refBL.getRefCodeDetails("Compliance Review Issue Common Action - Action Status", mstrConnectionString);
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("-- Select --", ""));
        }

        void FillGrid()
        {
            try
            {
                string strActionable_Status = ddlStatus.SelectedIndex > 0 ? ddlStatus.SelectedItem.Value : "";
                string strReponsiableStatus = string.IsNullOrEmpty(txtPersonResponsible.Text) ? "" : txtPersonResponsible.Text;
                string strTargetDate_From = string.IsNullOrEmpty(txtFromDate.Text) ? "" : txtFromDate.Text;
                string strTargeDate_To = string.IsNullOrEmpty(txtToDate.Text) ? "" : txtToDate.Text;
                string strComplainceReview = string.IsNullOrEmpty(txtComplianceReviewNo.Text) ? "" : txtComplianceReviewNo.Text;
                string strIssue = string.IsNullOrEmpty(txtIssue.Text) ? "" : txtIssue.Text;
                string strActionable = string.IsNullOrEmpty(txtActionable.Text) ? "" : txtActionable.Text;
                string reviewer = " and CCR_CRM_ID in (Select CRM_ID from TBL_CR_REVIEWER_MAS where CRM_L0_REVIEWER_NAME='"+Page.User.Identity.Name+"')";
                DataTable dt = new DataTable();
                dt = oBLL.getIssueActions_details(null, strComplainceReview, strIssue, strActionable, strActionable_Status, strReponsiableStatus,
                    strTargeDate_To, strTargeDate_To, strFilter1: reviewer);
                Session["ActionableList"] = dt;
                if (dt.Rows.Count > 0)
                {
                    gvComplianceActionableList.DataSource = dt;
                }
                else
                {
                    gvComplianceActionableList.DataSource = null;
                }
                gvComplianceActionableList.EditIndex = -1;
                gvComplianceActionableList.DataBind();
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        protected void cvTodate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
        protected void cvCircToDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
        public void writeError(string strMsg)
        {
            lblMsg.Text = strMsg;
            lblMsg.Visible = true;
            lblMsg.CssClass = "label";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                FillGrid();
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

        }

        protected void gvComplianceActionableList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvComplianceActionableList.PageIndex = e.NewPageIndex;
            gvComplianceActionableList.DataSource = (DataTable)Session["ActionableList"];
            gvComplianceActionableList.DataBind();
        }


        protected void gvComplianceActionableList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                HiddenField hfStatus = (HiddenField)(e.Row.FindControl("hfStatus"));
                LinkButton lbCloseActionable = (LinkButton)(e.Row.FindControl("lbCloseActionable"));

                if (hfStatus.Value.Equals("C"))
                    lbCloseActionable.Visible = false;
                else
                    lbCloseActionable.Visible = true;
            }
        }

        protected void gvComplianceActionableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strActionableId = gvComplianceActionableList.SelectedValue.ToString();
                Label lblIssueId = (Label)(gvComplianceActionableList.SelectedRow.FindControl("lblIssueId"));
                HiddenField hfStatus = (HiddenField)(gvComplianceActionableList.SelectedRow.FindControl("hfStatus"));

                if (hfSelectedOperation.Value.Equals("View"))
                {
                    Response.Redirect(Global.site_url("Projects/ComplianceReview/ActionableUpdates.aspx?Source=List&IssueId=" + lblIssueId.Text +
                        "&ActionableId=" + strActionableId + "&Status=" + hfStatus.Value));
                }
                else if (hfSelectedOperation.Value.Equals("Edit"))
                {
                    string script = "window.open('AddCircularActionable.aspx?CirId=" + lblIssueId.Text + "&ActionableId=" + strActionableId + "','_blank');";
                    ClientScript.RegisterStartupScript(this.GetType(), "EditActionable", script, true);
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnCloseActionable_ServerClick(object sender, EventArgs e)
        {
            try
            {
                int x=oBLL.submitForOperation(Convert.ToInt32(hfActionableId.Value),txtClosureRemarks.Text, "Close_Action", (new Authentication()).getUserFullName(Page.User.Identity.Name));
                if (x>0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "displaySucessMessage", "alert('Actionable closed successfully.');", true);
                }
                txtClosureRemarks.Text = "";
                hfClosureRemarks.Value = "";
                txtCompDate.Text = "";
                hfClosureDate.Value = "";
                FillGrid();
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}