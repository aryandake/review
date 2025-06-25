using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Compliance;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact.Projects.ComplianceReview
{
    public partial class ActionableUpdates : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        ComplianceReviewBLL oBLL= new ComplianceReviewBLL();
        RefCodesBLL rcBL = new RefCodesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    DateTime dtCurrentDt = System.DateTime.Now;
                    hfCurrDate.Value = dtCurrentDt.ToString("dd-MMM-yyyy");
                    if (Request.QueryString["IssueId"] != null)
                    {
                        hfIssueId.Value = Request.QueryString["IssueId"].ToString();
                    }

                    if (Request.QueryString["ActionableId"] != null)
                    {
                        hfActionableId.Value = Request.QueryString["ActionableId"].ToString();
                    }

                    if (Request.QueryString["Source"] != null)
                    {
                        hfSource.Value = Request.QueryString["Source"].ToString();
                    }

                    if (Request.QueryString["Status"] != null)
                    {
                        hfStatus.Value = Request.QueryString["Status"].ToString();
                    }

                    if (!hfSource.Value.Equals("MyAct") && !hfSource.Value.Equals("List"))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Script", "alert('Invalid Type'); window.location.href = '" + Global.site_url() + "';", true);
                    }

                    CommonCodes.SetDropDownDataSource(ddlUpdateType, rcBL.getRefCodeDetails("Compliance Review Actionable Update Type"));
                    getDetails();
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            //lblMsg.Visible = true;
            string script = "\r\n<script language=\"javascript\">\r\n" +
                       " alert('" + strError.Replace("'", "\\'") + "');" +
                       "   </script>\r\n";

            ClientScript.RegisterStartupScript(this.GetType(), "script", script);
        }

        protected String getFileName(object inputFileName)
        {
            string strFileName = "";
            try
            {
                strFileName = inputFileName.ToString();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return strFileName.Replace("'", "\\'");
        }

        private void getDetails()
        {
            try
            {
                Hashtable ParamMap = new Hashtable();
                DataTable dt = new DataTable();
                DataRow dr, dr1;
                DataSet dsCircularDetails = new DataSet();

                dt = oBLL.getIssue(Convert.ToInt32(hfIssueId.Value),0,null,null,null);

                if (dt.Rows.Count == 0)
                    writeError("No records found.");
                else
                {
                    dr = dt.Rows[0];
                    lblCreator.Text = dr["CI_CREATE_BY"].ToString();
                    lblIssueUnit.Text = dr["CSFM_NAME"].ToString();
                    lblIssueTitle.Text = dr["CI_ISSUE_TITLE"].ToString();
                    lblIssueDescription.Text = dr["CI_ISSUE_DESC"].ToString();
                    lblIssueType.Text = dr["IssueType"].ToString();
                    lblIssueStatus.Text = dr["IssueStatus"].ToString();
                    lblSPOCResponsible.Text = dr["CI_SPOC_RESPONSIBLE_NAME"].ToString();

                    DataTable dtActionable = new DataTable();
                    dtActionable = oBLL.getIssueActions(Convert.ToInt32(hfActionableId.Value), 0, null, null);
                    dr1 = dtActionable.Rows[0];
                    lblActionTypeOfAction.Text = dr1["ActionType"].ToString().Replace("\n", "<br />");
                    lblActionActionable.Text = dr1["CIA_ACTIONABLE"].ToString().Replace("\n", "<br />");
                    lblActionUnitResponsible.Text = dr1["CSFM_NAME"].ToString();
                    hfPersonResponsibleMailId.Value = dr1["CIA_SPECIFIED_PERSON_EMAIL"].ToString();
                    lblActionPersonResponsible.Text = dr1["CIA_SPECIFIED_PERSON_NAME"].ToString();
                    if (dr1["CIA_TARGET_DT"].ToString().Equals(""))
                    {
                        lblActionTargetDate.Text = "";
                    }
                    else
                    {
                        lblActionTargetDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dr1["CIA_TARGET_DT"]));
                    }

                    lblActionStatus.Text = dr1["ActionStatus"].ToString();
                    lblActionRemarks.Text = dr1["CIA_REMARKS"].ToString().Replace("\n", "<br />");
                    lblActionClosureRemarks.Text = dr1["CIA_CLOSURE_REMARKS"].ToString().Replace("\n", "<br />");
                    lblActionClosureBy.Text = dr1["CIA_CLOSURE_BY"].ToString();
                    if (dr1["CIA_CLOSURE_DT"].ToString().Equals(""))
                    {
                        lblActionClosureOn.Text = "";
                    }
                    else
                    {
                        lblActionClosureOn.Text = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(dr1["CIA_CLOSURE_DT"]));
                    }

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

                    FillGrid();

                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void FillGrid()
        {
            DataTable dtActionableUpdates = new DataTable();
            dtActionableUpdates = oBLL.getIssueActionsUpdates(0, Convert.ToInt32(hfActionableId.Value));
            Session["ActionableUpdates"] = dtActionableUpdates;
            gvActionableUpdates.DataSource = dtActionableUpdates;
            gvActionableUpdates.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this))
            {
                hfDoubleClickFlag.Value = "";
                return;
            }

            string strClientFileName = "", strCreateBy = "", strRevisedTargetDate = null, strActionableClosureDate = null;
            int intActionableId = 0;
            DateTime dt = DateTime.Now;

            try
            {
                intActionableId = Convert.ToInt32(hfActionableId.Value);

                strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                strClientFileName = fuFileUpload.FileName.ToString();
                //if (strClientFileName.Contains("!") || strClientFileName.Contains("@") ||
                //                     strClientFileName.Contains("#") || strClientFileName.Contains("$") ||
                //                     strClientFileName.Contains("%") || strClientFileName.Contains("^") ||
                //                     strClientFileName.Contains("&") || strClientFileName.Contains("'") ||
                //                     strClientFileName.Contains("\""))
                //{
                //    hfDoubleClickFlag.Value = "";
                //    writeError("File Name can't have special character.");
                //    return;
                //}
                //<< Modified by Ramesh more on 13-Mar-2024 CR_1991
                if (strClientFileName.Length > 200)
                {
                    writeError("File Name Exceed 200 Characters");
                    return;
                }
                if (strClientFileName.Contains("!") || strClientFileName.Contains("@") ||
                          strClientFileName.Contains("#") || strClientFileName.Contains("$") ||
                          strClientFileName.Contains("%") || strClientFileName.Contains("^") ||
                          strClientFileName.Contains("&") || strClientFileName.Contains("'") ||
                          strClientFileName.Contains("\"") || strClientFileName.Contains(","))
                {
                    writeError("Invalid File Name");
                    return;
                }

                string strFileExtension = Path.GetExtension(strClientFileName);
                if (UploadedFileContentCheck.checkForMaliciousFileFromHeaders(strFileExtension, fuFileUpload.FileBytes))
                {
                    writeError("The file contains malicious content. Kindly check the file and reupload.");
                    return;
                }


                string strReturnMsg = UploadedFileContentCheck.checkValidFileForUpload(fuFileUpload, "");

                CommonCode cc = new CommonCode();
                strReturnMsg += cc.getFileNameErrors(strClientFileName);
                if (!strReturnMsg.Equals(""))
                {
                    writeError(strReturnMsg);
                    return;
                }

                if (UploadedFileContentCheck.checkForMultipleExtention(strClientFileName))
                {
                    writeError("The file uploaded is multiple extensions.");
                    return;
                }
                //>>

                string strfilename = Authentication.GetUserID(Page.User.Identity.Name) + "_" +
                     dt.ToString("ddMMyyyyHHmmss") + "_" + fuFileUpload.FileName;
                string strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["ComplianceIssueFileFolder"].ToString());
                string strCompleteName = strServerDirectory + "\\" + strfilename;
                fuFileUpload.SaveAs(strCompleteName);

                DateTime? dtClosureDate = new DateTime?();
                DateTime? dtRevisedTargetDate = new DateTime?();
                if (ddlUpdateType.SelectedValue.Equals("ET")) // Extension in Target Date
                {
                    dtRevisedTargetDate = Convert.ToDateTime(txtDate.Text);
                }
                else if (ddlUpdateType.SelectedValue.Equals("AC")) // Actionable Closure Date
                {
                    dtClosureDate = Convert.ToDateTime(txtDate.Text);
                }
                int intId = oBLL.saveIssueActionUpdate(0,Convert.ToInt32(hfActionableId.Value),ddlUpdateType.SelectedItem.Value,null,null, dtRevisedTargetDate, null, dtClosureDate,null, strCreateBy,null,txtRemarks.Text,strClientFileName, strfilename);
                if (intId > 0)
                {
                    writeError("Actionable update saved successfully.");
                }
                sendActionableUpdatesMail();

                ddlUpdateType.SelectedValue = "";
                txtRemarks.Text = "";
                txtDate.Text = "";
                hfDoubleClickFlag.Value = "";
                FillGrid();
                getDetails();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void gvActionableUpdates_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvActionableUpdates.PageIndex = e.NewPageIndex;
            gvActionableUpdates.DataSource = (DataTable)(Session["ActionableUpdates"]);
            gvActionableUpdates.DataBind();
            hfTabberId.Value = "2";
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                if (hfSource.Value.Equals("List"))
                {
                    Response.Redirect(Global.site_url("Projects/ComplianceReview/ActionableList.aspx"));
                }
                else if (hfSource.Value.Equals("MyAct"))
                {
                    Response.Redirect(Global.site_url("Projects/ComplianceReview/MyActionables.aspx"));
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void sendActionableUpdatesMail()
        {
            string strComplianceSPOCMailId = "", strMailTo = "", strMailCC = "";

            MailContent_ComplianceReview mail = new MailContent_ComplianceReview();

            try
            {
                string strLoggedInUser = (new Authentication()).getUserFullName(Page.User.Identity.Name);

                mail.ParamMap.Add("ConfigId", 1100);

                DataTable dtnew = new DataTable();
                dtnew = oBLL.getIssueActions_details(null, null, null, null, null, null, null, null, strFilter1: " and CIA_ID='" + hfActionableId.Value + "'");
                string reviewer_emailid = "";
                string strPersonResponsibleMailId = dtnew.Rows[0]["CIA_SPECIFIED_PERSON_EMAIL"].ToString();
                #region Complaince user email id

                DataTable dt_review = new DataTable();
                dt_review = oBLL.Search_Reviewer_Master(Convert.ToInt32(dtnew.Rows[0]["CCR_CRM_ID"].ToString()),null,null,null,null);
                if(dt_review.Rows.Count>0)
                {
                    reviewer_emailid = dt_review.Rows[0]["CRM_L0_REVIEWER_EMAIL"].ToString();
                }

                #endregion
                

                if (hfSource.Value.Equals("MyAct"))
                {
                    // To --> Compliance User
                    // CC --> Submitted By User
                    strMailTo = reviewer_emailid;
                    strMailCC = strPersonResponsibleMailId;

                    mail.ParamMap.Add("To", "ProvidedAsParam");
                    mail.ParamMap.Add("cc", "ProvidedAsParam1");
                }
                else if (hfSource.Value.Equals("List"))
                {
                    // To --> Submitted By User
                    // CC --> Compliance User
                    strMailTo = strPersonResponsibleMailId;
                    strMailCC = reviewer_emailid;

                    mail.ParamMap.Add("To", "ProvidedAsParam");
                    mail.ParamMap.Add("cc", "ProvidedAsParam1");
                }

                

                DataRow dr = dtnew.Rows[0];

                mail.ParamMap.Add("ToEmailIds", strMailTo);
                mail.ParamMap.Add("CCEmailIds", strMailCC);
                mail.ParamMap.Add("CircularId", dr["CCR_ID"].ToString());
                mail.ParamMap.Add("CirNo", dr["CCR_IDENTIFIER"].ToString());
                mail.ParamMap.Add("CirActionableId", dr["CI_ID"].ToString());
                mail.ParamMap.Add("CirActionable", lblActionActionable.Text.ToString().Replace("\n", "<br />"));
                mail.ParamMap.Add("ResponsiblePerson", lblActionPersonResponsible.Text);
                mail.ParamMap.Add("TargetDate", lblActionTargetDate.Text);
                mail.ParamMap.Add("CompletionDate", lblActionClosureOn.Text);
                mail.ParamMap.Add("UpdateStatus", lblActionStatus.Text);
                mail.ParamMap.Add("UpdateType", ddlUpdateType.SelectedItem.Text);
                mail.ParamMap.Add("UpdateDetails", txtRemarks.Text.ToString().Replace("\n", "<br />"));
                mail.ParamMap.Add("RevisedTargetDate", (ddlUpdateType.SelectedValue.Equals("ET") ? txtDate.Text : null));
                mail.ParamMap.Add("ActionableClosureDate", (ddlUpdateType.SelectedValue.Equals("AC") ? txtDate.Text : null));
                mail.ParamMap.Add("LoggedInUserName", strLoggedInUser);
                mail.setComplianceReviewMailContent();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void cvTargetDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        protected void cvClosureDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
    }
}