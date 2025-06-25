using AjaxControlToolkit;
using AngleSharp.Dom;
using DocumentFormat.OpenXml.Spreadsheet;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.Compliance;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact.Projects.Compliance
{
    public partial class ApprovalComplianceReview : System.Web.UI.Page
    {
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();
        DataSet ds;
        int intCnt = 0, intCnt1 = 0;
        CommonCode cc = new CommonCode();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Type"]))
                {
                    hfType.Value = Request.QueryString["Type"];
                    if (hfType.Value.ToLower() == "uh")
                    {
                        lblHeader.Text = "Compliance Review Approval - UH";
                    }
                    else if (hfType.Value.ToLower() == "l0")
                    {
                        lblHeader.Text = "Compliance Review - Issue Resubmission";
                    }
                    else if (hfType.Value.ToLower() == "l1")
                    {
                        //lblHeader.Text = "Compliance Review Approval - Level 1 Reviewer";
                        lblHeader.Text = "Compliance Review Approval - Reviewer";
                    }
                    else if (hfType.Value.ToLower() == "l2")
                    {
                        lblHeader.Text = "Compliance Review Approval - Level 2 Reviewer";
                    }
                    FillComplianceGrid(hfType.Value);
                }
                else
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
        }

        void FillComplianceGrid(string strtype)
        {
            DataTable dt = new DataTable();
            dt = oBLL.getComplianceReview_Approval(strtype, Page.User.Identity.Name);
            if (dt.Rows.Count > 0)
            {
                gvComplianceReview.DataSource = dt;
            }
            else
            {
                gvComplianceReview.DataSource = null;
            }
            gvComplianceReview.EditIndex = -1;
            gvComplianceReview.DataBind();
            gvComplianceReview.Columns[0].Visible = false;
        }

        protected void gvComplianceReview_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void gvComplianceReview_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strId = gvComplianceReview.SelectedValue.ToString();
                if (hfSelectedOperation.Value == "View")
                {
                    Response.Redirect(Global.site_url("Projects/ComplianceReview/ViewComplianceReview.aspx?Source=" + hfType.Value + "&Id=" + strId));
                }
                else
                {
                    try
                    {
                        pnlComplianceReview.Visible = false;
                        pnlDraftedIssue.Visible = true;
                        hfComplianceId.Value = strId;
                        FillIssues(strId);
                    }
                    catch (Exception ex)
                    {
                        string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                        writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        void FillIssues(string strid)
        {
            int intRRID = Convert.ToInt32(strid);
            string strCreateBy = Page.User.Identity.Name;
            string strval = "";

            if (hfType.Value.ToLower() == "uh")
            {
                //strval = " and CSFM_UNIT_HEAD_CODE = '" + strCreateBy + "' and CI_STATUS IN('P','L')";
                strval = " and CSFM_UNIT_HEAD_CODE = '" + strCreateBy + "' and CI_STATUS IN('J')";
            }
            else if (hfType.Value.ToLower() == "l0")
            {
                strval = " and CI_STATUS IN('M','K')";
            }
            else if (hfType.Value.ToLower() == "l1")
            {
                //strval = " and CI_STATUS IN('I','0')";
                strval = " and CI_STATUS IN('P')";
            }
            else if (hfType.Value.ToLower() == "l2")
            {
                strval = " and CI_STATUS IN('J')";
            }

            DataTable dtDraftedIssue = new DataTable();
            dtDraftedIssue = oBLL.getIssue(0, intRRID, strCreateBy, "", null, strValue1: strval);
            gvDraftedIssue.DataSource = dtDraftedIssue;
            Session["DraftedIssue"] = "";
            Session["DraftedIssue"] = dtDraftedIssue;
            gvDraftedIssue.DataBind();

            if (gvDraftedIssue.Rows.Count > 0)
            {
                btnApprove.Visible = true;
                btnApprove1.Visible = true;
                btnReject.Visible = true;
                btnReject1.Visible = true;
            }
            else
            {
                btnApprove.Visible = false;
                btnApprove1.Visible = false;
                btnReject.Visible = false;
                btnReject1.Visible = false;
            }

            if (hfType.Value.ToLower() == "uh")
            {
                strval = "";
                strval = " and CSFM_UNIT_HEAD_CODE = '" + strCreateBy + "' ";
                DataTable dtDraftedIssue_check = new DataTable();
                dtDraftedIssue_check = oBLL.getIssue(0, intRRID, strCreateBy, "", null, strValue1: strval);
                if (dtDraftedIssue_check.Rows.Count == dtDraftedIssue.Rows.Count)
                {
                    btnApprove.Visible = true;
                    btnApprove1.Visible = true;
                }
                else
                {
                    btnApprove.Visible = false;
                    btnApprove1.Visible = false;
                }

                btnApprove.OnClientClick = "return approve_issues('', 'Are you sure, you want to review complete the selected issue(s)?')";
                btnApprove1.OnClientClick = "return approve_issues('', 'Are you sure, you want to review complete the selected issue(s)?')";

                btnReject.OnClientClick = "return approve_issues('VComments', 'Are you sure, you want to convey change the selected issue(s)?')";
                btnReject1.OnClientClick = "return approve_issues('VComments', 'Are you sure, you want to convey change the selected issue(s)?')";
            }
            else if (hfType.Value.ToLower() == "l1")
            {
                DataTable dtDraftedIssue_check = new DataTable();
                dtDraftedIssue_check = oBLL.getIssue(0, intRRID, strCreateBy, "", null, strValue1: strval);
                if (dtDraftedIssue_check.Rows.Count == dtDraftedIssue.Rows.Count)
                {
                    btnApprove.Visible = true;
                    btnApprove1.Visible = true;
                }
                else
                {
                    btnApprove.Visible = false;
                    btnApprove1.Visible = false;
                }
                btnReject.OnClientClick = "return approve_issues('VComments', 'Are you sure, you want to convey change the selected issue(s)?')";
                btnReject1.OnClientClick = "return approve_issues('VComments', 'Are you sure, you want to convey change the selected issue(s)?')";

                btnApprove.OnClientClick = "return approve_issues('', 'Are you sure, you want to review complete the selected issue(s)?')";
                btnApprove1.OnClientClick = "return approve_issues('', 'Are you sure, you want to review complete the selected issue(s)?')";
            }
            else if (hfType.Value.ToLower() == "l0")
            {
                btnReject.Visible = false;
                btnReject1.Visible = false;

                btnApprove.Text = "Resubmit for approval";
                btnApprove1.Text = "Resubmit for approval";

                btnApprove.OnClientClick = "return approve_issues('VComments', 'Are you sure, you want to resubmit the selected issue(s)?');";
                btnApprove1.OnClientClick = "return approve_issues('VComments', 'Are you sure, you want to resubmit the selected issue(s)?');";

                gvDraftedIssue.Columns[3].Visible = true;
            }

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

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                int xcount = 0;
                if (gvDraftedIssue.Rows.Count > 0)
                {
                    string strIsValidated = "", strScript = "", strRDIId = "", strRequestorId = "", strSPOCId = "",
                    strUnitId = "", strRRMId = "", strStatus = "", strProcess = "";

                    foreach (GridViewRow gr in gvDraftedIssue.Rows)
                    {
                        HiddenField hfissueid = (HiddenField)gr.FindControl("hfCI_ID");
                        TextBox txtcomment = (TextBox)gr.FindControl("txtComments");
                        CheckBox chk = (CheckBox)gr.FindControl("RowLevelCheckBox");
                        if (chk.Checked)
                        {
                            if (hfType.Value.ToLower() == "uh")
                            {
                                oBLL.ApproveRejectIssue(0, Convert.ToInt32(hfissueid.Value), txtcomment.Text, Page.User.Identity.Name, "A", "UH");

                                //for update in compliance review
                                oBLL.getComplianceReviewApproval_Status("Approved BY UH", Convert.ToInt32(hfComplianceId.Value));

                                xcount = xcount + 1;
                            }
                            else if (hfType.Value.ToLower() == "l0")
                            {
                                oBLL.ApproveRejectIssue(0, Convert.ToInt32(hfissueid.Value), txtcomment.Text, Page.User.Identity.Name, "A", "L0");

                                //for update in compliance review
                                //oBLL.getComplianceReviewApproval_Status("Approved BY L0", Convert.ToInt32(hfComplianceId.Value));

                                xcount = xcount + 1;
                            }
                            else if (hfType.Value.ToLower() == "l1")
                            {
                                oBLL.ApproveRejectIssue(0, Convert.ToInt32(hfissueid.Value), txtcomment.Text, Page.User.Identity.Name, "A", "L1");

                                //for update in compliance review
                                oBLL.getComplianceReviewApproval_Status("Approved BY L1", Convert.ToInt32(hfComplianceId.Value));
                                xcount = xcount + 1;
                            }
                            else if (hfType.Value.ToLower() == "l2")
                            {
                                oBLL.ApproveRejectIssue(0, Convert.ToInt32(hfissueid.Value), txtcomment.Text, Page.User.Identity.Name, "A", "L2");
                                xcount = xcount + 1;
                            }

                            #region For get Issues Details
                            DataTable dt1 = new DataTable();
                            dt1 = oBLL.getIssue(Convert.ToInt32(hfissueid.Value), 0, null, null, null);
                            if (dt1.Rows.Count > 0)
                            {
                                strRDIId = (string.IsNullOrEmpty(strRDIId) ? "" : strRDIId + ",") + dt1.Rows[0]["CI_Id"].ToString();
                                strSPOCId = (string.IsNullOrEmpty(strSPOCId) ? "" : strSPOCId + ",") + dt1.Rows[0]["CI_SPOC_RESPONSIBLE_ID"].ToString();
                                strUnitId = (string.IsNullOrEmpty(strUnitId) ? "" : strUnitId + ",") + dt1.Rows[0]["CI_UNIT_ID"].ToString();
                                strRRMId = (dt1.Rows[0]["CCR_CRM_ID"] is DBNull ? "" : dt1.Rows[0]["CCR_CRM_ID"].ToString());
                                strProcess = (dt1.Rows[0]["CRUM_UNIVERSE_TO_BE_REVIEWED"] is DBNull ? "" : dt1.Rows[0]["CRUM_UNIVERSE_TO_BE_REVIEWED"].ToString());
                            }
                            #endregion
                        }
                    }
                    if (xcount > 0)
                    {
                        if (hfType.Value.ToLower() == "uh")
                        {
                            if (!strRequestorId.Equals("") && strRequestorId.Contains(","))
                                strRequestorId = String.Join(",", cc.RemoveDuplicates(strRequestorId.Split(',')));
                            if (!strSPOCId.Equals("") && strSPOCId.Contains(","))
                                strSPOCId = String.Join(",", cc.RemoveDuplicates(strSPOCId.Split(',')));
                            if (!strUnitId.Equals("") && strUnitId.Contains(","))
                                strUnitId = String.Join(",", cc.RemoveDuplicates(strUnitId.Split(',')));

                            sendMailOnReviewCompleted(strRDIId, "Final", strRequestorId, strSPOCId, strUnitId, strRRMId, strProcess, "UH");
                        }

                        else if (hfType.Value.ToLower() == "l0")
                        {
                            if (!strRequestorId.Equals("") && strRequestorId.Contains(","))
                                strRequestorId = String.Join(",", cc.RemoveDuplicates(strRequestorId.Split(',')));
                            if (!strSPOCId.Equals("") && strSPOCId.Contains(","))
                                strSPOCId = String.Join(",", cc.RemoveDuplicates(strSPOCId.Split(',')));
                            if (!strUnitId.Equals("") && strUnitId.Contains(","))
                                strUnitId = String.Join(",", cc.RemoveDuplicates(strUnitId.Split(',')));

                            sendMailOnReviewCompleted(strRDIId, "Final", strRequestorId, strSPOCId, strUnitId, strRRMId, strProcess, "L0");
                            xcount = xcount + 1;
                        }
                        else if (hfType.Value.ToLower() == "l1")
                        {
                            if (!strRequestorId.Equals("") && strRequestorId.Contains(","))
                                strRequestorId = String.Join(",", cc.RemoveDuplicates(strRequestorId.Split(',')));
                            if (!strSPOCId.Equals("") && strSPOCId.Contains(","))
                                strSPOCId = String.Join(",", cc.RemoveDuplicates(strSPOCId.Split(',')));
                            if (!strUnitId.Equals("") && strUnitId.Contains(","))
                                strUnitId = String.Join(",", cc.RemoveDuplicates(strUnitId.Split(',')));

                            sendMailOnReviewCompleted(strRDIId, "Final", strRequestorId, strSPOCId, strUnitId, strRRMId, strProcess, "L1");
                        }
                        else if (hfType.Value.ToLower() == "l2")
                        {
                            if (!strRequestorId.Equals("") && strRequestorId.Contains(","))
                                strRequestorId = String.Join(",", cc.RemoveDuplicates(strRequestorId.Split(',')));
                            if (!strSPOCId.Equals("") && strSPOCId.Contains(","))
                                strSPOCId = String.Join(",", cc.RemoveDuplicates(strSPOCId.Split(',')));
                            if (!strUnitId.Equals("") && strUnitId.Contains(","))
                                strUnitId = String.Join(",", cc.RemoveDuplicates(strUnitId.Split(',')));

                            sendMailOnReviewCompleted(strRDIId, "Final", strRequestorId, strSPOCId, strUnitId, strRRMId, strProcess, "L2");
                        }
                        if (hfType.Value.ToLower() == "l2")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "MYalert", "alert('Selected issue(s) have been successfully submitted for approval.');window.location.href='" + Request.RawUrl + "';", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "MYalert", "alert('Selected issue(s) approved successfully.');window.location.href='" + Request.RawUrl + "';", true);
                        }
                        FillIssues(hfComplianceId.Value);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MYalert", "alert('No issue(s) selected for submit.');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                string strIsValidated = "", strScript = "", strRDIId = "", strRequestorId = "", strSPOCId = "",
                   strUnitId = "", strRRMId = "", strStatus = "", strProcess = "";
                int xcount = 0;
                if (gvDraftedIssue.Rows.Count > 0)
                {
                    foreach (GridViewRow gr in gvDraftedIssue.Rows)
                    {
                        HiddenField hfissueid = (HiddenField)gr.FindControl("hfCI_ID");
                        TextBox txtcomment = (TextBox)gr.FindControl("txtComments");
                        CheckBox chk = (CheckBox)gr.FindControl("RowLevelCheckBox");
                        if (chk.Checked)
                        {
                            if (hfType.Value.ToLower() == "uh")
                            {
                                oBLL.ApproveRejectIssue(0, Convert.ToInt32(hfissueid.Value), txtcomment.Text, Page.User.Identity.Name, "R", "UH");
                                xcount = xcount + 1;
                            }
                            else if (hfType.Value.ToLower() == "l0")
                            {
                                oBLL.ApproveRejectIssue(0, Convert.ToInt32(hfissueid.Value), txtcomment.Text, Page.User.Identity.Name, "R", "L0");
                                xcount = xcount + 1;
                            }
                            else if (hfType.Value.ToLower() == "l1")
                            {
                                oBLL.ApproveRejectIssue(0, Convert.ToInt32(hfissueid.Value), txtcomment.Text, Page.User.Identity.Name, "R", "L1");
                                xcount = xcount + 1;
                            }
                            else if (hfType.Value.ToLower() == "l2")
                            {
                                oBLL.ApproveRejectIssue(0, Convert.ToInt32(hfissueid.Value), txtcomment.Text, Page.User.Identity.Name, "R", "L2");
                                xcount = xcount + 1;
                            }

                            #region For get Issues Details
                            DataTable dt1 = new DataTable();
                            dt1 = oBLL.getIssue(Convert.ToInt32(hfissueid.Value), 0, null, null, null);
                            if (dt1.Rows.Count > 0)
                            {
                                strRDIId = (string.IsNullOrEmpty(strRDIId) ? "" : strRDIId + ",") + dt1.Rows[0]["CI_Id"].ToString();
                                strSPOCId = (string.IsNullOrEmpty(strSPOCId) ? "" : strSPOCId + ",") + dt1.Rows[0]["CI_SPOC_RESPONSIBLE_ID"].ToString();
                                strUnitId = (string.IsNullOrEmpty(strUnitId) ? "" : strUnitId + ",") + dt1.Rows[0]["CI_UNIT_ID"].ToString();
                                strRRMId = (dt1.Rows[0]["CCR_CRM_ID"] is DBNull ? "" : dt1.Rows[0]["CCR_CRM_ID"].ToString());
                                strProcess = (dt1.Rows[0]["CRUM_UNIVERSE_TO_BE_REVIEWED"] is DBNull ? "" : dt1.Rows[0]["CRUM_UNIVERSE_TO_BE_REVIEWED"].ToString());
                            }
                            #endregion
                        }
                    }
                    if (xcount > 0)
                    {
                        if (!strRequestorId.Equals("") && strRequestorId.Contains(","))
                            strRequestorId = String.Join(",", cc.RemoveDuplicates(strRequestorId.Split(',')));
                        if (!strSPOCId.Equals("") && strSPOCId.Contains(","))
                            strSPOCId = String.Join(",", cc.RemoveDuplicates(strSPOCId.Split(',')));
                        if (!strUnitId.Equals("") && strUnitId.Contains(","))
                            strUnitId = String.Join(",", cc.RemoveDuplicates(strUnitId.Split(',')));

                        sendMailOnChangesConveyed(strRDIId, "Draft", strRequestorId, strSPOCId, strUnitId, strRRMId, strProcess);

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MYalert", "alert('Changes conveyed successfully for selected issue(s).');window.location.href='" + Request.RawUrl + "';", true);
                    }
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ApprovalComplianceReview.aspx?Type=" + hfType.Value);
        }

        protected void gvDraftedIssue_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = (DataTable)(Session["DraftedIssue"]);
            string strMidArray = "";
            string strRowComments = "", strStatus = "", strRfv="";
            int intCntRec = dt.Rows.Count;
            int index = 0;

            if ((e.Row.RowType == DataControlRowType.Header))
            {
                CheckBox HeaderLevelCheckBox = (CheckBox)(e.Row.FindControl("HeaderLevelCheckBox"));
                HeaderLevelCheckBox.Attributes["onClick"] = "ChangeAllCheckBoxStates(this.checked);";
                strMidArray = HeaderLevelCheckBox.ClientID;
                Session["strMidArray"] = strMidArray;
            }
            else if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                CheckBox RowLevelCheckBox = (CheckBox)(e.Row.FindControl("RowLevelCheckBox"));
                TextBox txtComments = (TextBox)(e.Row.FindControl("txtComments"));
                HiddenField hfStatus = (HiddenField)(e.Row.FindControl("hfStatus"));
                LinkButton lnkAddActionPlan = (LinkButton)(e.Row.FindControl("lnkAddActionPlan"));
                LinkButton lnkAddCtrls = (LinkButton)(e.Row.FindControl("lnkAddCtrls"));
                LinkButton lnkControls = (LinkButton)(e.Row.FindControl("lnkControls"));
                RequiredFieldValidator rfv = (RequiredFieldValidator)(e.Row.FindControl("rfv_comments"));
                HiddenField hfActionToBeTaken = (HiddenField)(e.Row.FindControl("hfActionToBeTaken"));
                DataTable dtcontrol = new DataTable();
                Label lblsrno = (Label)(e.Row.FindControl("lblsrno"));
                index = Convert.ToInt32(lblsrno.Text) + 1;

                RowLevelCheckBox.Attributes["onClick"] = "onRowCheckedUnchecked('" + RowLevelCheckBox.ClientID + "','"
                    + txtComments.ClientID + "','" + hfStatus.ClientID + "','" + rfv.ClientID + "');";


                rfv.Enabled = false;
                strMidArray = RowLevelCheckBox.ClientID;
                strMidArray = "','" + strMidArray;
                strRowComments = "','" + txtComments.ClientID;
                strStatus = "','" + hfStatus.ClientID;
                strRfv = "','" + rfv.ClientID;

                if (Session["strRowComments"] == null)
                    Session["strRowComments"] = "";
                if (Session["strStatus"] == null)
                    Session["strStatus"] = "";
                if (Session["strRfv"] == null)
                    Session["strRfv"] = "";

                Session["strMidArray"] = Session["strMidArray"].ToString() + strMidArray;
                Session["strRowComments"] = Session["strRowComments"].ToString() + strRowComments;
                Session["strStatus"] = Session["strStatus"].ToString() + strStatus;
                Session["strRfv"] = Session["strRfv"].ToString() + strRfv;

                DataRowView drv = e.Row.DataItem as DataRowView;
                if (drv != null)
                {
                    //txtComments.Text = drv["RDI_DRAFTED_REMARKS"].ToString();
                }
            }

            intCnt = intCnt + 1;
            if (intCnt == intCntRec + 1)
            {
                if (Session["strMidArray"] != null && Session["strRowComments"] != null && Session["strStatus"] != null)
                {
                    CheckBoxIDsArray.Text = "<script type='text/javascript'>" +
                                            "var CheckBoxIDs =  new Array(' " + Session["strMidArray"].ToString() + "');" +
                                            "var commentsIDs =  new Array(' " + Session["strRowComments"].ToString() + "');" +
                                            "var StatusIDs =  new Array(' " + Session["strStatus"].ToString() + "');" +
                                            "var rfv_commentsIDs =  new Array(' " + Session["strRfv"].ToString() + "');" +
                                            "</script>";
                }
            }
        }




        private void sendMailOnReviewCompleted(string strRDId, string strType, string strInitiator, string strSPOCId, string strUnitId,
         string strRRMId, string strProcess, string strSendTo = "")
        {
            string strRole = "";
            DataTable dtIssues = new DataTable();
            CommonCode cc = new CommonCode();
            MailContent_ComplianceReview mailContent = new MailContent_ComplianceReview();

            try
            {
                mailContent.ParamMap.Clear();
                mailContent.ParamMap.Add("ConfigId", "1097");

                if (strType.Equals("Draft"))
                {
                    if (hfType.Value.Equals("UH"))
                    {
                        strRole = "Unit Head";
                        mailContent.ParamMap.Add("To", "L0");
                        mailContent.ParamMap.Add("cc", "UH,SPOC,L1");
                    }
                    else if (hfType.Value.Equals("RM1"))
                    {
                        strRole = "Reviewer";
                        mailContent.ParamMap.Add("To", "FH");
                        mailContent.ParamMap.Add("cc", "L0,UH");
                    }
                    else if (hfType.Value.Equals("FH"))
                    {
                        strRole = "Function Head";
                        mailContent.ParamMap.Add("To", "L1");
                        mailContent.ParamMap.Add("cc", "FH,L0,UH,SPOC");
                    }
                }
                else
                {
                    if (hfType.Value.Equals("RM2"))
                    {
                        strRole = "Level 1 Reviewer";

                        if (strSendTo.Equals("US"))
                        {
                            mailContent.ParamMap.Add("To", "SPOC");
                            mailContent.ParamMap.Add("cc", "L1,L0,UH");
                        }
                        else if (strSendTo.Equals("UH"))
                        {
                            mailContent.ParamMap.Add("To", "UH");
                            mailContent.ParamMap.Add("cc", "L1,L0,SPOC");
                        }
                        else if (strSendTo.Equals("FH"))
                        {
                            mailContent.ParamMap.Add("To", "FH");
                            mailContent.ParamMap.Add("cc", "L1,L2,UH,L0,SPOC");
                        }
                        else if (strSendTo.Equals("L2"))
                        {
                            mailContent.ParamMap.Add("To", "L2");
                            mailContent.ParamMap.Add("cc", "L1,FH,UH,L0,SPOC");
                        }
                    }
                    else if (hfType.Value.Equals("US"))
                    {
                        strRole = "Unit SPOC";
                        mailContent.ParamMap.Add("To", "UH");
                        mailContent.ParamMap.Add("cc", "SPOC,L1,FH,L0");
                    }
                    else if (hfType.Value.Equals("UH"))
                    {
                        strRole = "Unit Head";
                        mailContent.ParamMap.Add("To", "L0");
                        mailContent.ParamMap.Add("cc", "SPOC,L1,UH");
                    }
                    else if (hfType.Value.Equals("L0"))
                    {
                        strRole = "Reviewer";
                        mailContent.ParamMap.Add("To", "L1");
                        mailContent.ParamMap.Add("cc", "SPOC,L0,UH");
                    }
                    else if (hfType.Value.Equals("L1"))
                    {
                        strRole = "Reviewer";
                        mailContent.ParamMap.Add("To", "UH");
                        mailContent.ParamMap.Add("cc", "L1,SPOC,L0");
                    }
                    else if (hfType.Value.Equals("L2"))
                    {
                        strRole = "Level 2 Reviewer";
                        mailContent.ParamMap.Add("To", "UH,SPOC");
                        mailContent.ParamMap.Add("cc", "L2,L1,L0");
                    }
                }

                mailContent.ParamMap.Add("RequestorId", strInitiator);
                mailContent.ParamMap.Add("SPOCId", strSPOCId);
                mailContent.ParamMap.Add("UnitId", strUnitId);
                mailContent.ParamMap.Add("RDIIds", strRDId);
                mailContent.ParamMap.Add("ReviewerMasId", strRRMId);
                mailContent.ParamMap.Add("Type", hfType.Value);
                mailContent.ParamMap.Add("Role", strRole);
                mailContent.ParamMap.Add("Process", strProcess);
                mailContent.ParamMap.Add("IssueType", strType);
                mailContent.ParamMap.Add("ActionType", "Approval");
                mailContent.ParamMap.Add("SendTo", strSendTo);
                mailContent.ParamMap.Add("LoggedInUserName", (new Authentication()).GetUserDetsByEmpCode(Page.User.Identity.Name).Split('|')[0]);
                mailContent.setComplianceReviewMailContent();
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            FillIssues(hfComplianceId.Value);
        }

        private void sendMailOnChangesConveyed(string strRDId, string strType, string strInitiator, string strSPOCId, string strUnitId,
        string strRRMId, string strProcess)
        {
            string strRole = "";
            DataTable dtIssues = new DataTable();
            CommonCode cc = new CommonCode();
            MailContent_ComplianceReview mailContent = new MailContent_ComplianceReview();

            try
            {
                mailContent.ParamMap.Clear();
                mailContent.ParamMap.Add("ConfigId", "1098");

                if (strType.Equals("Draft"))
                {
                    if (hfType.Value.Equals("UH"))
                    {
                        strRole = "Unit Head";
                        mailContent.ParamMap.Add("To", "L0");
                        mailContent.ParamMap.Add("cc", "UH,L1,SPOC");
                    }
                    else if (hfType.Value.Equals("L1"))
                    {
                        strRole = "Reviewer";

                        mailContent.ParamMap.Add("To", "L0");
                        mailContent.ParamMap.Add("cc", "UH,L1,SPOC");
                    }
                    else if (hfType.Value.Equals("L2"))
                    {
                        strRole = "Reviewer";

                        mailContent.ParamMap.Add("To", "SPOC");
                        mailContent.ParamMap.Add("cc", "UH,L0");
                    }
                    else if (hfType.Value.Equals("FH"))
                    {
                        strRole = "Function Head";
                        mailContent.ParamMap.Add("To", "L0");
                        mailContent.ParamMap.Add("cc", "L1,FH,UH,SPOC");
                    }
                }
                else
                {
                    if (hfType.Value.Equals("RM2"))
                    {
                        strRole = "Level 1 Reviewer";
                        mailContent.ParamMap.Add("To", "L0");
                        mailContent.ParamMap.Add("cc", "L1,UH,SPOC");
                    }
                    else if (hfType.Value.Equals("US"))
                    {
                        strRole = "Unit SPOC";
                        mailContent.ParamMap.Add("To", "L1");
                        mailContent.ParamMap.Add("cc", "SPOC,L0,UH");
                    }
                    else if (hfType.Value.Equals("UH"))
                    {
                        strRole = "Unit Head";
                        mailContent.ParamMap.Add("To", "L0");
                        mailContent.ParamMap.Add("cc", "SPOC,L1,UH");
                    }
                    else if (hfType.Value.Equals("L1"))
                    {
                        strRole = "Reviewer";
                        mailContent.ParamMap.Add("To", "L0");
                        mailContent.ParamMap.Add("cc", "SPOC,L1,UH");
                    }
                    else if (hfType.Value.Equals("FH"))
                    {
                        strRole = "Function Head";
                        mailContent.ParamMap.Add("To", "L1");
                        mailContent.ParamMap.Add("cc", "SPOC,L0,L2,FH,UH");
                    }
                    else if (hfType.Value.Equals("RM3"))
                    {
                        strRole = "Level 2 Reviewer";
                        mailContent.ParamMap.Add("To", "L1");
                        mailContent.ParamMap.Add("cc", "L2,SPOC,L0,FH,UH");
                    }
                }

                mailContent.ParamMap.Add("RequestorId", strInitiator);
                mailContent.ParamMap.Add("SPOCId", strSPOCId);
                mailContent.ParamMap.Add("UnitId", strUnitId);
                mailContent.ParamMap.Add("RDIIds", strRDId);
                mailContent.ParamMap.Add("ReviewerMasId", strRRMId);
                mailContent.ParamMap.Add("Type", hfType.Value);
                mailContent.ParamMap.Add("Role", strRole);
                mailContent.ParamMap.Add("Process", strProcess);
                mailContent.ParamMap.Add("IssueType", strType);
                mailContent.ParamMap.Add("ActionType", "Rejection");
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