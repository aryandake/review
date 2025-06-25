using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using Fiction2Fact.Legacy_App_Code;
using DocumentFormat.OpenXml.Vml;
using Fiction2Fact.App_Code;
using System.Text;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Compliance;
using Org.BouncyCastle.Asn1.Sec;
using System.Web.Security;

namespace Fiction2Fact.Projects.Compliance
{
    public partial class ViewComplianceReview1 : System.Web.UI.Page
    {
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Source"]))
                {
                    hfSource.Value = Request.QueryString["Source"];
                    hfType.Value = Request.QueryString["Source"];
                    if (hfSource.Value.ToLower() == "my")
                    {
                        btnEditComplianceReview1.Visible = false;
                        btnEditComplianceReview1_Bottom.Visible = false;
                        btnAddQueryTop.Visible = true;
                        btnIssueTrackerTop.Visible = true;
                    }
                }

                if (User.IsInRole("CR_Compliance_User"))
                {
                    hfRoleType.Value = "CRI";
                }
                else if (User.IsInRole("CR_Unit_Head"))
                {
                    hfRoleType.Value = "CRUS";
                }
                else
                {
                    hfRoleType.Value = "OTHER";
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    hfCCRId.Value = Request.QueryString["Id"];
                    FillDetails(Convert.ToInt32(hfCCRId.Value));
                    Check_Approval_Status();
                }
                else
                {
                    Response.Redirect("SearchComplianceReview.aspx");
                }
            }

        }


        void FillDetails(int ccrid)
        {
            string strCCR_CREATOR = (new Authentication()).getUserFullName(Page.User.Identity.Name);
            DataTable dt = new DataTable();
            dt = oBLL.Search_ComplianceReview(ccrid, 0, 0, null, null, strCCR_CREATOR, null);
            if (dt.Rows.Count > 0)
            {
                lblBusinessUnits.Text = dt.Rows[0]["ImpactedUnits"].ToString();
                lblCCRId.Text = dt.Rows[0]["CCR_ID"].ToString();
                lblIdentifier.Text = dt.Rows[0]["CCR_IDENTIFIER"].ToString();
                lblRemark.Text = dt.Rows[0]["CCR_REMARKS"].ToString().Replace(Environment.NewLine, "<br />");
                lblReviewerName.Text = dt.Rows[0]["CRM_L0_REVIEWER_NAME"].ToString();
                lblReviewScope.Text = dt.Rows[0]["CCR_REVIEW_SCOPE"].ToString().Replace(Environment.NewLine, "<br />");
                lblStatus.Text = dt.Rows[0]["CCR_REC_STATUS"].ToString();
                lblUniversetoReview.Text = dt.Rows[0]["CRUM_UNIVERSE_TO_BE_REVIEWED"].ToString();
                hfCCRStatus.Value = dt.Rows[0]["CCR_STATUS"].ToString();
                hfRRMId.Value = dt.Rows[0]["CCR_CRM_ID"].ToString();
                hfRRImpactedUnitIds.Value = dt.Rows[0]["CCR_UNIT_IDS"].ToString();
                lblReviewType.Text = dt.Rows[0]["RC_NAME"].ToString();
                lblStatus.Text = dt.Rows[0]["SM_DESC"].ToString();
                lblStatusRemarks.Text = dt.Rows[0]["CCR_REC_STATUS_REMARKS"].ToString().Replace(Environment.NewLine, "<br />");

                lblSubmittedBy.Text = dt.Rows[0]["CCR_CREATOR"].ToString();


                lblSubmittedOn.Text = ((DateTime)dt.Rows[0]["CCR_CREATE_DT"]).ToString("dd-MMM-yyyy hh:mm:ss");

                if (dt.Rows[0]["CCR_TENTATIVE_END_DATE"] != null && dt.Rows[0]["CCR_TENTATIVE_END_DATE"] != DBNull.Value)
                {
                    lblTentativeEndDT.Text = ((DateTime)dt.Rows[0]["CCR_TENTATIVE_END_DATE"]).ToString("dd-MMM-yyyy");
                }
                else
                {
                    lblTentativeEndDT.Text = "";
                }

                if (dt.Rows[0]["CCR_TENTATIVE_START_DATE"] != null && dt.Rows[0]["CCR_TENTATIVE_START_DATE"] != DBNull.Value)
                {
                    lblTentativeStartDT.Text = ((DateTime)dt.Rows[0]["CCR_TENTATIVE_START_DATE"]).ToString("dd-MMM-yyyy");
                }
                else
                {
                    lblTentativeStartDT.Text = "";
                }


                if (dt.Rows[0]["CCR_WORK_STARTED_ON"] != null && dt.Rows[0]["CCR_WORK_STARTED_ON"] != DBNull.Value)
                {
                    lblWorkStartedOn.Text = ((DateTime)dt.Rows[0]["CCR_WORK_STARTED_ON"]).ToString("dd-MMM-yyyy hh:mm:ss");
                }
                else
                {
                    lblWorkStartedOn.Text = "";
                }

                if (dt.Rows[0]["CCR_APPROVAL_ON_UH"] != null && dt.Rows[0]["CCR_APPROVAL_ON_UH"] != DBNull.Value)
                {
                    lblApprovedByUnitHead.Text = ((DateTime)dt.Rows[0]["CCR_APPROVAL_ON_UH"]).ToString("dd-MMM-yyyy hh:mm:ss");
                }
                else
                {
                    lblApprovedByUnitHead.Text = "";
                }

                if (dt.Rows[0]["CCR_APPROVAL_ON_L0"] != null && dt.Rows[0]["CCR_APPROVAL_ON_L0"] != DBNull.Value)
                {
                    lblApprovedByL0.Text = ((DateTime)dt.Rows[0]["CCR_APPROVAL_ON_L0"]).ToString("dd-MMM-yyyy hh:mm:ss");
                }
                else
                {
                    lblApprovedByL0.Text = "";
                }

                #region Circular details

                lblLinkageWithEarilerCircular.Text = dt.Rows[0]["CCR_LINKAGE_WITH_EARLIER_CIRCULAR"] is DBNull ? "" : (dt.Rows[0]["CCR_LINKAGE_WITH_EARLIER_CIRCULAR"].ToString() == "Y" ? "Yes" : "No");
                lblSupersedesorExtends.Text = (dt.Rows[0]["CCR_SOC_EOC"] is DBNull ? "" : dt.Rows[0]["SOC_EOC"].ToString());
                lblOrderCircularSubjectNo.Text = (dt.Rows[0]["CCR_OLD_CIRC_SUB_NO"] is DBNull ? "" : dt.Rows[0]["CCR_OLD_CIRC_SUB_NO"].ToString());

                if (lblLinkageWithEarilerCircular.Text.ToLower() == "yes")
                {
                    trOC.Visible = true;
                    trSOC.Visible = true;
                }
                else
                {
                    trOC.Visible = false;
                    trSOC.Visible = false;
                }

                #endregion

                #region For Fill Scope Documents

                DataTable dt_attachments = new DataTable();
                dt_attachments = oBLL.Search_Compliance_Review_Files(0, ccrid);
                if (dt_attachments.Rows.Count > 0)
                {
                    gvCRAttachment.DataSource = dt_attachments;
                }
                else
                {
                    gvCRAttachment.DataSource = null;
                }
                gvCRAttachment.EditIndex = -1;
                gvCRAttachment.DataBind();

                #endregion

                FillQueries();
                FillIssues();

                if (hfRoleType.Value.Equals("CRI") && hfSource.Value.Equals("MY"))
                {
                    if (hfCCRStatus.Value.ToLower() == "cr_a")
                    {
                        showHideButtons(1);
                    }
                    else if (hfCCRStatus.Value.ToLower() == "cr_b")
                    {
                        showHideButtons(2);
                    }

                }
                else
                {
                    showHideButtons(10);
                }


            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            cancelAndRedirect();
        }

        private void cancelAndRedirect()
        {
            if (hfSource.Value.Equals("MY") || hfSource.Value.Equals("RES"))
            {
                Response.Redirect("SearchComplianceReview.aspx?Type=" + hfSource.Value);
            }
            else if (hfSource.Value.Equals("UH") || hfSource.Value.Equals("L0") || hfSource.Value.Equals("L1") || hfSource.Value.Equals("L2"))
            {
                Response.Redirect("ApprovalComplianceReview.aspx?Type=" + hfSource.Value);
            }
            else if (hfSource.Value.ToLower().Equals("drq"))
            {
                Response.Redirect("SearchDataRequirement.aspx");
            }
            else
            {
                Response.Redirect("SearchComplianceReview.aspx");
            }
        }

        protected void btnEditComplianceReview1_Click(object sender, EventArgs e)
        {
            if (!hfCCRId.Value.Equals(""))
            {
                Response.Redirect(Global.site_url("Projects/ComplianceReview/AddComplianceReview.aspx?Source=submitRiskReview&Id=" + hfCCRId.Value.ToString()));
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(Global.site_url("Projects/ComplianceReview/ViewComplianceReview.aspx?Source=" + hfSource.Value + "&Id=" + hfCCRId.Value));
        }

        void FillQueries()
        {
            if (hfSource.Value.ToLower() == "res")
            {
                gvQueries.DataSource = oBLL.getDRQMDetails(0, Convert.ToInt32(hfCCRId.Value), "", 0, Page.User.Identity.Name, strValue: " and CDQ_STATUS not in ('D')");
            }
            else
            {
                gvQueries.DataSource = oBLL.getDRQMDetails(0, Convert.ToInt32(hfCCRId.Value), "", 0, null, strValue: " and CDQ_STATUS not in ('D')");
            }
            gvQueries.DataBind();
        }

        protected void btnCancel1_Click(object sender, EventArgs e)
        {
            cancelAndRedirect();
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

        void FillIssues()
        {
            try
            {
                int intRRID = Convert.ToInt32(hfCCRId.Value);
                string strCreateBy = Page.User.Identity.Name;
                DataTable dtDraftedIssue = new DataTable();
                if (hfSource.Value.ToLower() == "res")
                {
                    dtDraftedIssue = oBLL.getIssue(0, intRRID, strCreateBy, "View_RES", null, strValue1: " and CI_STATUS not in ('A')");
                }
                else
                {
                    dtDraftedIssue = oBLL.getIssue(0, intRRID, null, null, null, strValue1: " and CI_STATUS not in ('A')");
                }
                gvIssues.DataSource = dtDraftedIssue;
                gvIssues.DataBind();


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

        protected void btnWorkStarted1_Click(object sender, EventArgs e)
        {
            try
            {
                int intRRRowId = 0, intRiskReviewId = 0;
                string strCreateBy = "", strCreator = "";

                bool isRefid = int.TryParse(hfCCRId.Value.ToString(), out intRiskReviewId);
                strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                strCreator = Page.User.Identity.Name;
                intRRRowId = oBLL.submitForOperation(0, hfRoleType.Value.ToString(), "StartWork", strCreateBy, intRiskReviewDraftId: intRiskReviewId);
                ClientScript.RegisterStartupScript(this.GetType(), "Script", "alert('Compliance Review has been initiated successfully.');", true);
                sendComplianceReviewInitiation(intRiskReviewId);
                showHideButtons(2);
                FillDetails(intRiskReviewId);
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void showHideButtons(int intFlag)
        {
            switch (intFlag)
            {
                //if Status = Planned
                case 1:
                    {
                        btnEditComplianceReview1.Visible = false;
                        btnAddQueryTop.Visible = false;
                        btnIssueTrackerTop.Visible = false;
                        btnCancel1.Visible = true;

                        btnEditComplianceReview1_Bottom.Visible = false;
                        btnAddQuery_Bottom.Visible = false;
                        btnIssueTracker_Bottom.Visible = false;
                        btnCancel1_Bottom.Visible = true;

                        if (hfType.Value.Equals("MY"))
                        {
                            btnWorkStarted1.Visible = true;
                            btnWorkStarted1_Bottom.Visible = true;
                        }
                        else if (hfType.Value.Equals("RES"))
                        {
                            btnEditComplianceReview1.Visible = false;
                            btnEditComplianceReview1.Visible = false;
                        }
                        else
                        {
                            btnWorkStarted1.Visible = false;
                            btnWorkStarted1_Bottom.Visible = false;
                        }

                        break;
                    }
                case 2:
                    {
                        btnEditComplianceReview1.Visible = false;
                        btnAddQueryTop.Visible = true;
                        btnIssueTrackerTop.Visible = true;
                        btnCancel1.Visible = true;

                        btnEditComplianceReview1_Bottom.Visible = false;
                        btnAddQuery_Bottom.Visible = true;
                        btnIssueTracker_Bottom.Visible = true;
                        btnCancel1_Bottom.Visible = true;

                        if (hfType.Value.Equals("RES"))
                        {
                            btnEditComplianceReview1.Visible = false;
                            btnEditComplianceReview1.Visible = false;

                            btnAddQueryTop.Visible = false;
                            btnIssueTrackerTop.Visible = false;
                            btnCancel1.Visible = false;

                            btnAddQuery_Bottom.Visible = false;
                            btnIssueTracker_Bottom.Visible = false;
                            btnCancel1_Bottom.Visible = false;
                        }
                        btnWorkStarted1.Visible = false;
                        btnWorkStarted1_Bottom.Visible = false;
                        break;
                    }
                //if Status = Data Requirement raised, pending with Unit(s)
                case 3:
                    {
                        btnEditComplianceReview1.Visible = false;
                        btnWorkStarted1.Visible = false;
                        btnCancel1.Visible = true;

                        btnEditComplianceReview1_Bottom.Visible = false;
                        btnWorkStarted1_Bottom.Visible = false;
                        btnCancel1_Bottom.Visible = true;

                        if (hfType.Value.Equals("MY"))
                        //if (hfType.Value.Equals("RQI"))
                        {
                            btnAddQueryTop.Visible = true;
                            btnIssueTrackerTop.Visible = true;

                            btnAddQuery_Bottom.Visible = true;
                            btnIssueTracker_Bottom.Visible = true;
                        }
                        else if (hfType.Value.Equals("RES"))
                        {
                            btnEditComplianceReview1.Visible = false;

                            btnEditComplianceReview1_Bottom.Visible = false;
                        }
                        else
                        {
                            btnAddQueryTop.Visible = false;
                            btnIssueTrackerTop.Visible = false;

                            btnAddQuery_Bottom.Visible = false;
                            btnIssueTracker_Bottom.Visible = false;
                        }

                        break;
                    }
                //if Status = Query closed
                case 4:
                    {
                        btnEditComplianceReview1.Visible = false;
                        btnWorkStarted1.Visible = false;
                        btnCancel1.Visible = true;

                        btnEditComplianceReview1_Bottom.Visible = false;
                        btnWorkStarted1_Bottom.Visible = false;
                        btnCancel1_Bottom.Visible = true;

                        if (hfType.Value.Equals("MY"))
                        //if (hfType.Value.Equals("RQI"))
                        {
                            btnAddQueryTop.Visible = true;
                            btnIssueTrackerTop.Visible = true;

                            btnAddQuery_Bottom.Visible = true;
                            btnIssueTracker_Bottom.Visible = true;
                        }
                        else if (hfType.Value.Equals("RES"))
                        {
                            btnEditComplianceReview1.Visible = false;

                            btnEditComplianceReview1_Bottom.Visible = false;
                        }
                        else
                        {
                            btnAddQueryTop.Visible = false;
                            btnIssueTrackerTop.Visible = false;

                            btnAddQuery_Bottom.Visible = false;
                            btnIssueTracker_Bottom.Visible = false;
                        }

                        break;
                    }
                //if Status = Query Issue Tracker pending with Unit SPOCs
                case 5:
                    {
                        btnEditComplianceReview1.Visible = false;
                        btnWorkStarted1.Visible = false;
                        //btnAddQueryTop.Visible = true;
                        btnAddQueryTop.Visible = false;
                        //btnIssueTrackerTop.Visible = false;
                        btnCancel1.Visible = true;

                        btnEditComplianceReview1.Visible = false;
                        btnWorkStarted1_Bottom.Visible = false;
                        //btnAddQuery.Visible = true;
                        btnAddQuery_Bottom.Visible = false;
                        //btnIssueTracker.Visible = false;
                        btnCancel1_Bottom.Visible = true;

                        if (hfType.Value.Equals("MY"))
                        //if (hfType.Value.Equals("RQI"))
                        {
                            btnIssueTrackerTop.Visible = true;

                            btnIssueTracker_Bottom.Visible = true;
                        }
                        else if (hfType.Value.Equals("RES"))
                        {
                            btnEditComplianceReview1.Visible = false;

                            btnEditComplianceReview1_Bottom.Visible = false;
                        }
                        else
                        {
                            btnIssueTrackerTop.Visible = false;

                            btnIssueTracker_Bottom.Visible = false;
                        }

                        break;
                    }
                default:
                    {
                        btnEditComplianceReview1.Visible = false;
                        btnWorkStarted1.Visible = false;
                        btnAddQueryTop.Visible = false;
                        btnCancel1.Visible = true;

                        if ((intFlag.Equals(7) || intFlag.Equals(24)) && hfType.Value.Equals("MY"))
                        {
                            btnIssueTrackerTop.Visible = true;
                            btnIssueTracker_Bottom.Visible = true;
                        }
                        else
                        {
                            btnIssueTrackerTop.Visible = false;
                            btnIssueTracker_Bottom.Visible = false;
                        }

                        btnEditComplianceReview1_Bottom.Visible = false;
                        btnWorkStarted1_Bottom.Visible = false;
                        btnAddQuery_Bottom.Visible = false;
                        btnCancel1_Bottom.Visible = true;
                        break;
                    }
            }
        }

        private void sendComplianceReviewInitiation(int intRRId)
        {
            MailContent_ComplianceReview mailContent = new MailContent_ComplianceReview();

            try
            {
                mailContent.ParamMap.Clear();
                mailContent.ParamMap.Add("ConfigId", "1091");
                mailContent.ParamMap.Add("To", "UH");
                mailContent.ParamMap.Add("cc", "L0,L1"); // L0, L1 user
                mailContent.ParamMap.Add("ReviewerMasId", hfRRMId.Value);
                mailContent.ParamMap.Add("RRIds", intRRId);
                mailContent.ParamMap.Add("UnitId", hfRRImpactedUnitIds.Value);
                mailContent.ParamMap.Add("Process", lblUniversetoReview.Text);
                mailContent.ParamMap.Add("Role", "Reviewer");
                mailContent.ParamMap.Add("LoggedInUserName", (new Authentication()).GetUserDetsByEmpCode(Page.User.Identity.Name).Split('|')[0]);
                mailContent.setComplianceReviewMailContent();
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        void Check_Approval_Status()
        {
            if (hfSource.Value.ToLower() == "my")
            {
                if (hfCCRStatus.Value.ToLower() != "cr_a")
                {
                    DataTable dt = new DataTable();
                    dt = oBLL.getComplianceReviewApproval_Status("Get Complaince Status", Convert.ToInt32(hfCCRId.Value));
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["a"].ToString() == "1")
                        {
                            dt = new DataTable();
                            dt = oBLL.getIssue(0, Convert.ToInt32(hfCCRId.Value), null, null, null);
                            if (dt.Rows.Count > 0)
                            {
                                if (hfCCRStatus.Value.ToLower() == "cr_i" || hfCCRStatus.Value.ToLower() == "cr_f" || hfCCRStatus.Value.ToLower() == "cr_g" || hfCCRStatus.Value.ToLower() == "cr_h")
                                {
                                    btnSubmitForApproval.Visible = false;
                                }
                                else
                                {
                                    btnSubmitForApproval.Visible = true;
                                }
                            }
                            else
                            {
                                btnSubmitForApproval.Visible = false;
                            }
                        }
                        else
                        {
                            btnSubmitForApproval.Visible = false;
                        }
                    }
                    else
                    {
                        btnSubmitForApproval.Visible = false;
                    }
                }
            }

            if (hfCCRStatus.Value.ToLower() == "cr_i" || hfCCRStatus.Value.ToLower() == "cr_f" || hfCCRStatus.Value.ToLower() == "cr_g" || hfCCRStatus.Value.ToLower() == "cr_h")
            {
                btnIssueTrackerTop.Visible = false;
                btnEditComplianceReview1.Visible = false;
                btnAddQueryTop.Visible = false;
                btnWorkStarted1.Visible = false;

                btnEditComplianceReview1_Bottom.Visible = false;
                btnWorkStarted1_Bottom.Visible = false;
                btnAddQuery_Bottom.Visible = false;
                btnIssueTracker_Bottom.Visible = false;
            }
        }

        protected void btnSubmitForApproval_Click(object sender, EventArgs e)
        {
            try
            {
                int intRRRowId = 0, intRiskReviewId = 0;
                string strCreateBy = "", strCreator = "";

                bool isRefid = int.TryParse(hfCCRId.Value.ToString(), out intRiskReviewId);
                strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                strCreator = Page.User.Identity.Name;
                intRRRowId = oBLL.submitForOperation(0, "CR_I", "UpdateRecordStatus_All", strCreateBy, intRiskReviewDraftId: intRiskReviewId);

                //For Update Issue Updates
                intRRRowId = oBLL.submitForOperation(0, "P", "UpdateStatusIssue_All", strCreateBy, intRiskReviewDraftId: intRiskReviewId);

                sendComplianceReviewApproval(intRiskReviewId);
                ClientScript.RegisterStartupScript(this.GetType(), "Script", "alert('Compliance Review has been sent for approval successfully.');window.location.href='" + Request.RawUrl + "';", true);
                showHideButtons(2);
                FillDetails(intRiskReviewId);
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void sendComplianceReviewApproval(int intRRId)
        {
            MailContent_ComplianceReview mailContent = new MailContent_ComplianceReview();

            try
            {
                mailContent.ParamMap.Clear();
                mailContent.ParamMap.Add("ConfigId", "1104");
                mailContent.ParamMap.Add("To", "L1");//for reviewer
                mailContent.ParamMap.Add("cc", "L0,UH"); // L0, L1 user
                mailContent.ParamMap.Add("ReviewerMasId", hfRRMId.Value);
                mailContent.ParamMap.Add("RRIds", intRRId);
                mailContent.ParamMap.Add("UnitId", hfRRImpactedUnitIds.Value);
                mailContent.ParamMap.Add("Process", lblUniversetoReview.Text);
                mailContent.ParamMap.Add("Role", "Reviewer");
                mailContent.ParamMap.Add("LoggedInUserName", (new Authentication()).GetUserDetsByEmpCode(Page.User.Identity.Name).Split('|')[0]);
                mailContent.setComplianceReviewMailContent();
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
    }
}