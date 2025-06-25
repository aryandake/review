using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Compliance;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact.Projects.Compliance
{
    public partial class IssueTracker : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();
        CommonMethods cm = new CommonMethods();
        RefCodesBLL refBL = new RefCodesBLL();
        string script = "";
        int intCnt = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    hfCR_ID.Value = Request.QueryString["Id"];
                    FillIssues();
                }
            }
        }


        void FillIssues()
        {
            try
            {
                int intRRID = Convert.ToInt32(hfCR_ID.Value);
                string strCreateBy = Page.User.Identity.Name;
                DataTable dtDraftedIssue = new DataTable();
                dtDraftedIssue = oBLL.getIssue(0, intRRID, strCreateBy, "View_RES", null, strValue1: " and CI_STATUS in ('B','C','D','G')");
                gvComplianceReview.DataSource = dtDraftedIssue;
                Session["DraftedIssue"] = "";
                Session["DraftedIssue"] = dtDraftedIssue;
                gvComplianceReview.DataBind();

                if (intCnt > 0)
                {
                    btnSubmit.Visible = true;
                    btnSubmit1.Visible = true;
                }
                else
                {
                    btnSubmit.Visible = false;
                    btnSubmit1.Visible = false;
                }

            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;

        }

        protected DataTable LoadDraftedFileList(object Id)
        {
            return oBLL.getIssueFiles(Convert.ToInt32(Id), 0, "ComplianceIssue", "Compliance Review Issue Tracker - File Type");
        }

        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (gvComplianceReview.Rows.Count > 0)
            {
                int xcount = 0;
                string strissueids = "";
                foreach (GridViewRow gr in gvComplianceReview.Rows)
                {
                    HiddenField hf = (HiddenField)gr.FindControl("hfStatus");
                    HiddenField hfciid = (HiddenField)gr.FindControl("hfCI_ID");
                    if (hf.Value.ToLower() == "d")
                    {
                        int intissueid = Convert.ToInt32(hfciid.Value);
                        string strStatus = "E";
                        int x = oBLL.ChangeIssueStatus(intissueid, strStatus, null, null);
                        if (x > 0)
                        {
                            string strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                            int intRRRowId = oBLL.submitForOperation(intissueid, null, "IssueAuditTrail", strCreateBy,strValue1:" Issue Response Submitted By : ");
                            if (intRRRowId > 0)
                            {
                                strissueids = (string.IsNullOrEmpty(strissueids) ? "" : strissueids + ",") + Convert.ToString(intissueid);
                                xcount = xcount + 1;
                            }
                        }
                    }
                }
                if (xcount > 0)
                {
                    //for update status in Compliance Review
                    int intRiskReviewId = 0;
                    bool isRefid = int.TryParse(hfCR_ID.Value.ToString(), out intRiskReviewId);
                    string strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                    string strCreator = Page.User.Identity.Name;
                    int intRRRowId = oBLL.submitForOperation(0, "CR_E", "UpdateRecordStatus_All", strCreateBy, intRiskReviewDraftId: intRiskReviewId);
                    if (intRRRowId > 0)
                    {
                        sendMailOnIssuesSubmission(strissueids);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MYalert", "alert('Selected issue(s) are submitted successfully..');window.location.href='" + Request.RawUrl + "';", true);
                    }
                }
                FillIssues();
            }
        }

        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            FillIssues();
        }

        protected void gvComplianceReview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hf = (HiddenField)e.Row.FindControl("hfStatus");
                if (hf.Value.ToLower() == "d")
                {
                    intCnt = intCnt + 1;
                }
                LinkButton lnk = (LinkButton)e.Row.FindControl("lnkAddIssueTracker");
                if (hf.Value.ToLower() == "d" || hf.Value.ToLower() == "c" || hf.Value.ToLower() == "b" || hf.Value.ToLower() == "g")
                {
                    lnk.Visible = true;
                }
                else
                {
                    lnk.Visible = false;
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Projects/ComplianceReview/SearchComplianceReview.aspx?Type=RES");
        }



        private void sendMailOnIssuesSubmission(string issueids)
        {
            string strUnitId = "", strUnitName = "", strInitiator = "", strRRMId = "", strProcess = "";
            DataTable dt = new DataTable();
            DataTable dtDistinctUnits = new DataTable();
            MailContent_ComplianceReview mailContent = new MailContent_ComplianceReview();
            string strLoggedInUser = (new Authentication()).getUserFullName(Page.User.Identity.Name);
            try
            {
                dt = oBLL.getIssue(0, 0, null, null, null, strValue1: " and CI_ID in (" + issueids + ")");
                strRRMId = dt.Rows[0]["CCR_CRM_ID"].ToString();
                strProcess = dt.Rows[0]["CRUM_UNIVERSE_TO_BE_REVIEWED"].ToString();
                int intRRId = Convert.ToInt32(dt.Rows[0]["CCR_ID"].ToString());
                DataView dv = new DataView(dt);
                dtDistinctUnits = dv.ToTable(true, "CCR_UNIT_IDS");

                for (int i = 0; i < dtDistinctUnits.Rows.Count; i++)
                {
                    string strRDIIds = "";
                    DataRow dr = dtDistinctUnits.Rows[i];

                    strUnitId = dr["CCR_UNIT_IDS"].ToString();
                    DataView dvData = new DataView(dt);
                    dvData.RowFilter = "CCR_UNIT_IDS = '" + strUnitId + "'";
                    DataTable dtFilteredData = dvData.ToTable();

                    for (int j = 0; j < dtFilteredData.Rows.Count; j++)
                    {
                        strRDIIds = (string.IsNullOrEmpty(strRDIIds) ? "" : strRDIIds + ",") + dtFilteredData.Rows[j]["CI_ID"].ToString();
                        strUnitName = dtFilteredData.Rows[j]["CSFM_NAME"].ToString();
                        strInitiator = dtFilteredData.Rows[j]["CCR_CREATOR"].ToString();
                    }

                    mailContent.ParamMap.Clear();
                    mailContent.ParamMap.Add("ConfigId", "1101");
                    mailContent.ParamMap.Add("To", "L0"); // Creator
                    mailContent.ParamMap.Add("cc", "UH,L1,SPOC"); // SPOC Responsible, UH and L1 User
                    mailContent.ParamMap.Add("SPOCId", Page.User.Identity.Name);
                    mailContent.ParamMap.Add("RequestorId", strInitiator);
                    mailContent.ParamMap.Add("ReviewerMasId", strRRMId);
                    mailContent.ParamMap.Add("RDIIds", strRDIIds);
                    mailContent.ParamMap.Add("RRIds", intRRId);
                    mailContent.ParamMap.Add("UnitId", strUnitId);
                    mailContent.ParamMap.Add("UnitName", strUnitName);
                    mailContent.ParamMap.Add("Type", "US");
                    mailContent.ParamMap.Add("Role", "Unit SPOC");
                    mailContent.ParamMap.Add("Process", strProcess);
                    mailContent.ParamMap.Add("ActionType", "SubmitIssueResponse");
                    mailContent.ParamMap.Add("LoggedInUserName", (new Authentication()).GetUserDetsByEmpCode(Page.User.Identity.Name).Split('|')[0]);
                    mailContent.setComplianceReviewMailContent();
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
    }
}