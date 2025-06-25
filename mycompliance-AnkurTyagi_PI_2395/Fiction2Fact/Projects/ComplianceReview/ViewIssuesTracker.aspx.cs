using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact.Projects.Compliance
{
    public partial class ViewIssuesTracker : System.Web.UI.Page
    {
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();
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
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    hfIssueId.Value = Request.QueryString["Id"];
                    FillIssueDetails();
                    FillActions();
                }
            }
        }

        void FillIssueDetails()
        {
            string strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
            DataTable dt = new DataTable();
            dt = oBLL.getIssue(Convert.ToInt32(hfIssueId.Value), 0, null, null, null);
            DataRow dr = dt.Rows[0];
            lblUnit.Text = (dr["CSFM_NAME"] is DBNull ? "" : dr["CSFM_NAME"].ToString());

            lblIssueTitle.Text = (dr["CI_ISSUE_TITLE"] is DBNull ? "" : dr["CI_ISSUE_TITLE"].ToString());
            lblIssueDescription.Text = (dr["CI_ISSUE_DESC"] is DBNull ? "" : dr["CI_ISSUE_DESC"].ToString().Replace(Environment.NewLine, "<br />"));
            lblIssueType.Text = (dr["IssueType"] is DBNull ? "" : dr["IssueType"].ToString());
            lblIssueStatus.Text = (dr["IssueStatus"] is DBNull ? "" : dr["IssueStatus"].ToString());
            lblSPOCResponsible.Text = (dr["CI_SPOC_RESPONSIBLE"] is DBNull ? "" : dr["CI_SPOC_RESPONSIBLE"].ToString());

            lblManagementRemark.Text = (dr["CI_MNGMT_RESPONSE"] is DBNull ? "" : dr["CI_MNGMT_RESPONSE"].ToString().Replace(Environment.NewLine, "<br />"));
            lblRemarks.Text = (dr["CI_MNGMT_REMARKS"] is DBNull ? "" : dr["CI_MNGMT_REMARKS"].ToString().Replace(Environment.NewLine, "<br />"));

            lblStatus.Text = (dr["DraftIssuesStatus"] is DBNull ? "" : dr["DraftIssuesStatus"].ToString().Replace(Environment.NewLine, "<br />"));


            lblAcceptanceRemark.Text = (dr["CI_ACCEPTANCE_REMARKS"] is DBNull ? "" : dr["CI_ACCEPTANCE_REMARKS"].ToString().Replace(Environment.NewLine, "<br />"));
            lblAcceptedBy.Text = (dr["CI_ACCEPTED_BY"] is DBNull ? "" : dr["CI_ACCEPTED_BY"].ToString());
            lblAccepetedOn.Text = (dr["CI_ACCEPTED_ON"] is DBNull ? "" : ((DateTime)dr["CI_ACCEPTED_ON"]).ToString("dd-MMM-yyyy hh:mm:ss"));
            
            lblRejectedRemarks.Text = (dr["CI_REJECTION_REMARKS"] is DBNull ? "" : dr["CI_REJECTION_REMARKS"].ToString().Replace(Environment.NewLine, "<br />"));
            lblRejectedBy.Text = (dr["CI_REJECTED_BY"] is DBNull ? "" : dr["CI_REJECTED_BY"].ToString());
            lblRejectedOn.Text = (dr["CI_REJECTED_ON"] is DBNull ? "" : ((DateTime)dr["CI_REJECTED_ON"]).ToString("dd-MMM-yyyy hh:mm:ss"));


            //Resubmitted 
            lblResubmittedBy.Text = (dr["CI_APPROVAL_BY_L0"] is DBNull ? "" : dr["CI_APPROVAL_BY_L0"].ToString());
            lblResubmittedOn.Text = (dr["CI_APPROVAL_ON_L0"] is DBNull ? "" : ((DateTime)dr["CI_APPROVAL_ON_L0"]).ToString("dd-MMM-yyyy hh:mm:ss"));
            lblResubmittedRemark.Text = (dr["CI_APPROVAL_REMARKS_L0"] is DBNull ? "" : dr["CI_APPROVAL_REMARKS_L0"].ToString().Replace(Environment.NewLine, "<br />"));



            //for Approval - UH
            lblApprovedByUH.Text = (dr["CI_APPROVAL_BY_UH"] is DBNull ? "" : dr["CI_APPROVAL_BY_UH"].ToString());
            lblApprovedOnUH.Text = (dr["CI_APPROVAL_ON_UH"] is DBNull ? "" : ((DateTime)dr["CI_APPROVAL_ON_UH"]).ToString("dd-MMM-yyyy hh:mm:ss"));
            lblApprovalRemUH.Text = (dr["CI_APPROVAL_REMARKS_UH"] is DBNull ? "" : dr["CI_APPROVAL_REMARKS_UH"].ToString().Replace(Environment.NewLine, "<br />"));

            lblRejectedByUH.Text = (dr["CI_REJECTION_BY_UH"] is DBNull ? "" : dr["CI_REJECTION_BY_UH"].ToString());
            lblRejectedOnUH.Text = (dr["CI_REJECTION_ON_UH"] is DBNull ? "" : ((DateTime)dr["CI_REJECTION_ON_UH"]).ToString("dd-MMM-yyyy hh:mm:ss"));
            lblRejectionRemUH.Text = (dr["CI_REJECTION_REMARKS_UH"] is DBNull ? "" : dr["CI_REJECTION_REMARKS_UH"].ToString().Replace(Environment.NewLine, "<br />"));


            //for Approval - Reviewer
            lblApprovedByRM1.Text = (dr["CI_APPROVAL_BY_L1"] is DBNull ? "" : dr["CI_APPROVAL_BY_L1"].ToString());
            lblApprovedOnRM1.Text = (dr["CI_APPROVAL_ON_L1"] is DBNull ? "" : ((DateTime)dr["CI_APPROVAL_ON_L1"]).ToString("dd-MMM-yyyy hh:mm:ss"));
            lblApprovalRemRM1.Text = (dr["CI_APPROVAL_REMARKS_L1"] is DBNull ? "" : dr["CI_APPROVAL_REMARKS_L1"].ToString().Replace(Environment.NewLine, "<br />"));

            lblRejectedByRM1.Text = (dr["CI_REJECTION_BY_L1"] is DBNull ? "" : dr["CI_REJECTION_BY_L1"].ToString());
            lblRejectedOnRM1.Text = (dr["CI_REJECTION_ON_L1"] is DBNull ? "" : ((DateTime)dr["CI_REJECTION_ON_L1"]).ToString("dd-MMM-yyyy hh:mm:ss"));
            lblRejectionRemRM1.Text = (dr["CI_REJECTION_REMARKS_L1"] is DBNull ? "" : dr["CI_REJECTION_REMARKS_L1"].ToString().Replace(Environment.NewLine, "<br />"));

            lblAuditTrail.Text = (dr["CI_AUDIT_TRAIL"] is DBNull ? "" : dr["CI_AUDIT_TRAIL"].ToString().Replace(Environment.NewLine, "<br />"));

            #region Files
            DataTable dtFiles = new DataTable();
            dtFiles = oBLL.getIssueFiles(Convert.ToInt32(hfIssueId.Value), 0, "ComplianceIssue", "Compliance Review Issue Tracker - File Type");
            if (dtFiles.Rows.Count > 0)
            {
                gvAttachments.DataSource = dtFiles;
            }
            else
            {
                gvAttachments.DataSource = null;
            }
            gvAttachments.DataBind();
            #endregion

        }
        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }
        protected DataTable LoadDraftedFileList(object Id)
        {
            return oBLL.getIssueFiles(Convert.ToInt32(Id), 0, "ComplianceIssue", "Compliance Review Issue Tracker - File Type");
        }

        void FillActions()
        {
            DataTable dt = new DataTable();
            dt = oBLL.getIssueActions(0, Convert.ToInt32(hfIssueId.Value), null, null);
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
    }
}