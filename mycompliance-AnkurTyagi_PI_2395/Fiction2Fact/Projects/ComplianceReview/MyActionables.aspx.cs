using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using Fiction2Fact.Legacy_App_Code.DAL;
using Fiction2Fact.Legacy_App_Code.VO;
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
    public partial class MyActionables : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        RefCodesBLL refBL = new RefCodesBLL();
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
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
            string strStatus = ddlStatus.SelectedIndex > 0 ? ddlStatus.SelectedItem.Value : "";
            string strTentativeStartDate = txtFromDate.Text;
            string strTentativeEndDate = txtToDate.Text;
            string strUser = Page.User.Identity.Name;


            DataTable dt = new DataTable();
            dt = oBLL.getIssueActions(0, 0, null, null, strCIA_ACTIONABLE_STATUS:strStatus,strCIA_RESPONSIBLE_PERSON:strUser,strCIA_TARGET_DT_FROM:strTentativeStartDate,strCIA_TARGET_DT_TO:strTentativeEndDate);
            if (dt.Rows.Count > 0)
            {
                gvActionables.DataSource = dt;
            }
            else
            {
                gvActionables.DataSource = null;
            }
            gvActionables.DataBind();
        }

        protected void cvTodate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

        }

        protected void gvActionables_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvActionables.PageIndex = e.NewPageIndex;
            FillGrid();
        }


        protected void gvActionables_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strIssueId;
            try
            {
                strIssueId = gvActionables.SelectedValue.ToString();
                hfSelectedRecord.Value = strIssueId;
                Label lblActionableId = (Label)(gvActionables.SelectedRow.FindControl("lblActionableId"));
                HiddenField hfStatus = (HiddenField)(gvActionables.SelectedRow.FindControl("hfStatus"));

                if (hfSelectedOperation.Value.Equals("Edit"))
                {
                    Response.Redirect("ActionableUpdates.aspx?Source=MyAct&IssueId=" + strIssueId +
                        "&ActionableId=" + lblActionableId.Text + "&Status=" + hfStatus.Value);
                }
            }
            catch (Exception ex)
            {
                this.lblMsg.Text = ex.Message;
                this.lblMsg.Visible = true;
            }
        }
    }
}