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
    public partial class AddEditIssuesDetails : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();
        CommonMethods cm = new CommonMethods();
        RefCodesBLL refBL = new RefCodesBLL();
        string script = "";
        int intCnt = 0, intCnt_new = 0;

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
                Session["strMidArray"] = "";

                DataTable dt = new DataTable();
                string strCreator = Page.User.Identity.Name;
                if (Request.QueryString["Type"] != null)
                {
                    hfType.Value = Request.QueryString["Type"].ToString();
                }

                if (Request.QueryString["Source"] != null)
                {
                    hfSource.Value = Request.QueryString["Source"].ToString();
                }

                if (Request.QueryString["ViewType"] != null)
                {
                    hfViewType.Value = Request.QueryString["ViewType"].ToString();
                }

                if (Request.QueryString["Id"] != null)
                {
                    hfRDIId.Value = Request.QueryString["Id"].ToString();
                }

                if (Request.QueryString["RefId"] != null)
                    hfRefID.Value = Request.QueryString["RefId"].ToString();
                else
                    hfRefID.Value = "0";

                if (Request.QueryString["RRStatus"] != null)
                    hfRRStatus.Value = Request.QueryString["RRStatus"].ToString();

                FillResponsibleUnit();
                FillIssueStatus();
                FillIssueType();
                mvIssueTracker.ActiveViewIndex = 0;
                litAttachment.Text = getAttachmentHTMLTable() + "</table>";

                if (hfType.Value.Equals("Add"))
                {
                    string strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                    mvIssueTracker.ActiveViewIndex = 0;
                    lblHeader.Text = "Add/Edit Issue Details";

                    DataTable dtdrafted = new DataTable();
                    dtdrafted = oBLL.getIssue(0, Convert.ToInt32(hfRefID.Value), strCreateBy, "CREATED_SELF", null);
                    Session["DraftedIssue"] = dtdrafted;
                    gvDraftedIssue.DataSource = dtdrafted;
                    gvDraftedIssue.DataBind();

                    if (intCnt_new > 0)
                    {
                        btnSubmitForIdentification.Visible = true;
                        btnSubmitForIdentification1.Visible = true;
                    }
                    else
                    {
                        btnSubmitForIdentification.Visible = false;
                        btnSubmitForIdentification1.Visible = false;
                    }
                }
                else if (hfType.Value.Equals("View"))
                {
                    mvIssueTracker.ActiveViewIndex = 1;
                    lblHeader.Text = "View Issue Details";

                }



            }
        }

        void FillIssueStatus()
        {
            ddlIsueStatus.DataSource = refBL.getRefCodeDetails("Compliance Review Issue Tracker - Status", mstrConnectionString);
            ddlIsueStatus.DataBind();
            ddlIsueStatus.Items.Insert(0, new ListItem("(Select an option)", ""));
        }
        void FillIssueType()
        {
            ddlIssueType.DataSource = refBL.getRefCodeDetails("Compliance Review Issue Tracker - Issue Type", mstrConnectionString);
            ddlIssueType.DataBind();
            ddlIssueType.Items.Insert(0, new ListItem("(Select an option)", ""));
        }


        void FillResponsibleUnit()
        {
            ddlUnitId.DataSource = oBLL.Search_SubFunction_Master(0, null, strCSFM_Status: "A", strFilter1: " and CSFM_ID in (Select items from Split((Select CCR_UNIT_IDS from tbl_CR_COMP_REVIEWS where CCR_ID=" + hfRefID.Value + "),','))");
            ddlUnitId.DataBind();
            ddlUnitId.Items.Insert(0, new ListItem("(Select an option)", ""));
        }


        private string getAttachmentHTMLTable()
        {
            StringBuilder sbAttachmentHTML = new StringBuilder();

            sbAttachmentHTML.Append(" <table id='tblAttachment' width='100%' class='table table-bordered footable'> " +
                                    " <thead> " +
                                    " <tr> " +
                                    " <th class='contentBody' style=\"text-align:left;\"> " +
                                    " <input type='checkbox' ID='HeaderLevelCheckBoxAttachment' onclick = 'return onAttachmentHeaderRowChecked()'/> " +
                                    " </th>  " +
                                    " <th class='contentBody' style=\"text-align:left;\"> " +
                                    " File Type " +
                                    " </th> " +
                                    " <th class='contentBody' style=\"text-align:left;\"> " +
                                    " File Description " +
                                    " </th> " +
                                    " <th class='contentBody' style=\"text-align:left;\"> " +
                                    " Attachment Name " +
                                    " </th> " +
                                    " </tr> " +
                                    " </thead> ");

            return sbAttachmentHTML.ToString();
        }

        private DataTable getAttachmentdt()
        {
            string strAttachment = hfAttachment.Value;
            string[] strarrAttachment, strarrFields;
            string strTemp;
            DataRow dr;

            DataTable dtAttachment = new DataTable();
            dtAttachment.Columns.Add(new DataColumn("AttachId", typeof(string)));
            dtAttachment.Columns.Add(new DataColumn("ServerFileName", typeof(string)));
            dtAttachment.Columns.Add(new DataColumn("ClientFileName", typeof(string)));
            dtAttachment.Columns.Add(new DataColumn("FileTypeId", typeof(string)));
            dtAttachment.Columns.Add(new DataColumn("FileTypeName", typeof(string)));
            dtAttachment.Columns.Add(new DataColumn("FileTypeDesc", typeof(string)));
            dtAttachment.Columns.Add(new DataColumn("Type", typeof(string)));

            strarrAttachment = strAttachment.Split('~');
            for (int i = 0; i < strarrAttachment.Length - 1; i++)
            {
                strTemp = strarrAttachment[i];
                strarrFields = strTemp.Split('|');
                dr = dtAttachment.NewRow();

                dr["AttachId"] = strarrFields[0];
                dr["ClientFileName"] = strarrFields[1];
                dr["ServerFileName"] = strarrFields[2];
                dr["FileTypeId"] = strarrFields[3];
                dr["FileTypeName"] = strarrFields[4];
                dr["FileTypeDesc"] = strarrFields[5].ToString();
                dr["Type"] = "ComplianceIssue";
                dtAttachment.Rows.Add(dr);
            }
            return dtAttachment;
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

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            updateDetails("1");
        }

        protected void updateDetails(string strStatus)
        {
            try
            {
                lblMsg.Text = "";
                DataTable dtFiles = new DataTable();

                int intRDIRowId = 0, intRDIId = 0, intUnitId = 0, intRRID = 0;
                string strRDIId = "", strIssueTitle = "", strIssueDescription = "", strIssueType = "",
                       strIssueStatus = "", strCreateBy = "", strMsg = "", strSPOCReponsibleId = "",
                       strSPOCReponsibleName = "", strSPOCReponsibleEmail = "", strScript = "", strRemarks = "";

                strRDIId = lblRDIId.Text.ToString();

                if (!strRDIId.Equals(""))
                    intRDIId = Convert.ToInt32(strRDIId);

                bool res = int.TryParse(hfRDIId.Value, out intRDIId);
                bool res1 = int.TryParse(hfRefID.Value, out intRRID);

                if (!ddlUnitId.SelectedValue.Equals(""))
                    intUnitId = Convert.ToInt32(ddlUnitId.SelectedValue);

                if (!ddlIsueStatus.SelectedValue.Equals(""))
                    strIssueStatus = ddlIsueStatus.SelectedValue;
                if (!ddlIssueType.SelectedValue.Equals(""))
                    strIssueType = ddlIssueType.SelectedValue;

                dtFiles = getAttachmentdt();

                strIssueDescription = txtIssueDescription.Text;
                strIssueTitle = txtIssueTitle.Text;
                strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                strRemarks = txtRemarks.Text;

                #region Responsible Person
                if (hfResPersonEdit.Value.Equals(""))
                {
                    if (!hfResponsibleSPOCId.Value.Equals("") && !hfResponsibleSPOCId.Value.Equals("||"))
                    {
                        strSPOCReponsibleId = hfResponsibleSPOCId.Value.Split('|')[0];
                        strSPOCReponsibleName = hfResponsibleSPOCId.Value.Split('|')[1];
                        strSPOCReponsibleEmail = hfResponsibleSPOCId.Value.Split('|')[2];
                    }
                    else
                    {
                        strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                                    "alert('Please enter correct details for SPOC responsible.');\r\n" +
                                    "</script>\r\n";

                        ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);

                        hfDoubleClickFlag.Value = "";
                        return;
                    }
                }
                else
                {
                    if (hfFlag.Value.Equals("true"))
                    {
                        if (hfResponsibleSPOCId.Value.Equals(""))
                        {
                            strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                                            "alert('Please enter correct details for SPOC responsible.');\r\n" +
                                            "</script>\r\n";

                            ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);

                            hfDoubleClickFlag.Value = "";
                            return;
                        }
                        else
                        {
                            strSPOCReponsibleId = hfResponsibleSPOCId.Value.Split('|')[0];
                            strSPOCReponsibleName = hfResponsibleSPOCId.Value.Split('|')[1];
                            strSPOCReponsibleEmail = hfResponsibleSPOCId.Value.Split('|')[2];
                        }
                    }
                    else
                    {
                        strSPOCReponsibleId = hfResPersonEdit.Value.Split('|')[0];
                        strSPOCReponsibleName = hfResPersonEdit.Value.Split('|')[1];
                        strSPOCReponsibleEmail = hfResPersonEdit.Value.Split('|')[2];
                    }
                }
                #endregion

                intRDIRowId = oBLL.saveIssueDetails(intRDIId, intRRID, intUnitId, strIssueTitle, strIssueDescription,
                strIssueType, strIssueStatus, txtSPOCResponsible.Text, strSPOCReponsibleId, strSPOCReponsibleName, strSPOCReponsibleEmail,
                strCreateBy, strRemarks, "A", dtFiles);

                if (intRDIRowId == 0)
                    intRDIRowId = intRDIId;
                if (!hfRDIId.Value.Equals("") && !hfRDIId.Value.Equals("0"))
                    strMsg = "Issue Details have been updated successfully.";
                else
                    strMsg = "Issue Details have been added successfully.";

                DataTable dtDraftedIssue = new DataTable();
                dtDraftedIssue = oBLL.getIssue(0, intRRID, strCreateBy, "CREATED_SELF", null);
                gvDraftedIssue.DataSource = dtDraftedIssue;
                Session["DraftedIssue"] = "";
                Session["DraftedIssue"] = dtDraftedIssue;
                gvDraftedIssue.DataBind();

                if (intCnt_new > 0)
                {
                    btnSubmitForIdentification.Visible = true;
                    btnSubmitForIdentification1.Visible = true;
                }
                else
                {
                    btnSubmitForIdentification.Visible = false;
                    btnSubmitForIdentification1.Visible = false;
                }
                clearContents();
                hfDoubleClickFlag.Value = "";

                strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                            "alert('" + strMsg + "');\r\n" +
                            "document.getElementById('btnRefresh').click();\r\n" +
                            "</script>\r\n";

                ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);

                //hfIsEdit.Value = "";
            }
            catch (Exception ex)
            {
                //hfIsEdit.Value = "";
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void clearContents()
        {
            hfDoubleClickFlag.Value = "";
            lblRDIId.Text = "";
            hfRDIId.Value = "";
            ddlUnitId.SelectedIndex = -1;
            txtIssueTitle.Text = "";
            txtIssueDescription.Text = "";
            ddlIssueType.SelectedIndex = -1;
            ddlIsueStatus.SelectedIndex = -1;
            hfInherentRiskRating.Value = "";
            hfAttachment.Value = "";
            hfResponsibleSPOCId.Value = "";
            txtSPOCResponsible.Text = "";
        }


        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            string strSource = hfSource.Value;
            string strType = hfType.Value;
            string strViewType = hfViewType.Value;
            string strURL = "";
            clearContents();

            try
            {
                if (strViewType.Equals("1"))
                {
                    string message = "this.window.close(); window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnRefresh').click(); ";
                    ScriptManager.RegisterClientScriptBlock(sender as Control, this.GetType(), "alert", message, true);
                }
                else
                {
                    strURL = Global.site_url("Projects/ComplianceReview/SearchComplianceReview.aspx" +
                                            "?Type=" + strSource);

                    string message = "this.window.close(); window.opener.location='" + strURL + "'; ";
                    ScriptManager.RegisterClientScriptBlock(sender as Control, this.GetType(), "alert", message, true);
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void gvDraftedIssue_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            DataTable dt = (DataTable)(Session["DraftedIssue"]);
            string strMidArray = "", strRowComments = "";
            int intCntRec = dt.Rows.Count;

            if ((e.Row.RowType == DataControlRowType.Header))
            {
                CheckBox HeaderLevelCheckBox = (CheckBox)(e.Row.FindControl("HeaderLevelCheckBox"));
                HeaderLevelCheckBox.Attributes["onClick"] = "ChangeAllCheckBoxStates(this.checked);";
                strMidArray = HeaderLevelCheckBox.ClientID;
                Session["strMidArray"] = strMidArray;

            }
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                HiddenField hfStatus = (HiddenField)(e.Row.FindControl("hfStatus"));
                HiddenField hfmgmtres = (HiddenField)(e.Row.FindControl("hfManagementResponse"));

                CheckBox RowLevelCheckBox = (CheckBox)(e.Row.FindControl("RowLevelCheckBox"));
                LinkButton lnkEdit = (LinkButton)(e.Row.FindControl("lnkEdit"));
                LinkButton lnkCopy = (LinkButton)(e.Row.FindControl("lnkCopy"));

                LinkButton lnkStatus = (LinkButton)(e.Row.FindControl("lnkSetStatus"));


                RowLevelCheckBox.Attributes["onClick"] = "onRowCheckedUnchecked('" + RowLevelCheckBox.ClientID + "');";
                RowLevelCheckBox.Visible = false;



                #region Old Code

                //if (hfStatus.Value.ToLower() == "f" || hfStatus.Value.ToLower() == "i" || hfStatus.Value.ToLower() == "h"
                //    || hfStatus.Value.ToLower() == "j" || hfStatus.Value.ToLower() == "k" || hfStatus.Value.ToLower() == "l"
                //    || hfStatus.Value.ToLower() == "m" || hfStatus.Value.ToLower() == "n" || hfStatus.Value.ToLower() == "o" || hfStatus.Value.ToLower() == "g")
                //{
                //    lnkStatus.Visible = false;
                //    lnkEdit.Visible = false;
                //}
                //else
                //{
                //    lnkStatus.Visible = true;
                //    lnkEdit.Visible = false;

                //    if (!string.IsNullOrEmpty(hfmgmtres.Value))
                //    {
                //        lnkStatus.Visible = true;
                //    }
                //    else
                //    {
                //        lnkStatus.Visible = false;
                //    }
                //} 
                #endregion

                if (hfStatus.Value.ToLower()=="e")
                {
                    lnkStatus.Visible = true;
                    lnkEdit.Visible = false;

                    if (!string.IsNullOrEmpty(hfmgmtres.Value))
                    {
                        lnkStatus.Visible = true;
                    }
                    else
                    {
                        lnkStatus.Visible = false;
                    }
                }
                else
                {
                    lnkStatus.Visible = false;
                    lnkEdit.Visible = false;
                }

                if (hfStatus.Value.ToLower().Equals("a"))
                {
                    RowLevelCheckBox.Visible = true;
                    lnkStatus.Visible = false;
                    lnkEdit.Visible = true;
                    intCnt_new = intCnt_new + 1;
                }


                strMidArray = RowLevelCheckBox.ClientID;
                strMidArray = "','" + strMidArray;

                Session["strMidArray"] = Session["strMidArray"].ToString() + strMidArray;
            }
            intCnt = intCnt + 1;
            if (intCnt == intCntRec + 1)
                CheckBoxIDsArray.Text = "<script type='text/javascript'>" +
                                        "var CheckBoxIDs =  new Array('" + Session["strMidArray"].ToString() + "');" +
                                        "</script>";
        }

        protected void gvDraftedIssue_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dtFiles = new DataTable();
            DataRow dr, drFiles;
            bool res = false;
            int intDraftedFileId = 0, intRefId = 0, uniqueRowId = 0;
            string strSPOCReponsibleId = "", strSPOCReponsibleName = "", strSPOCReponsibleEmail = "";
            StringBuilder sbAttachmentHTMLRows = new StringBuilder();
            string strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);

            try
            {
                res = int.TryParse(hfRefID.Value, out intRefId);
                res = int.TryParse(gvDraftedIssue.SelectedValue.ToString(), out intDraftedFileId);

                if (hfSelectedOperation.Value == "Delete")
                {
                    //processBL.deleteCommonActionables(intId);

                    string strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                                       "alert('Issues have been deleted successfully.');\r\n" +
                                       "</script>\r\n";

                    ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);

                    dt = oBLL.getIssue(0, intRefId, strCreateBy, "CREATED_SELF", null);
                    gvDraftedIssue.DataSource = dt;
                    gvDraftedIssue.DataBind();
                    if (intCnt_new > 0)
                    {
                        btnSubmitForIdentification.Visible = true;
                        btnSubmitForIdentification1.Visible = true;
                    }
                    else
                    {
                        btnSubmitForIdentification.Visible = false;
                        btnSubmitForIdentification1.Visible = false;
                    }
                }
                else if (hfSelectedOperation.Value == "Edit")
                {
                    //hfIsEdit.Value = "E";
                    dt = oBLL.getIssue(intDraftedFileId, intRefId, strCreateBy, "CREATED_SELF", "");
                    dr = dt.Rows[0];
                    lblRDIId.Text = (dr["CI_ID"] is DBNull ? "0" : dr["CI_ID"].ToString()); ;
                    hfRDIId.Value = (dr["CI_ID"] is DBNull ? "0" : dr["CI_ID"].ToString());
                    ddlUnitId.SelectedValue = (dr["CI_UNIT_ID"] is DBNull ? "" : dr["CI_UNIT_ID"].ToString());

                    txtIssueTitle.Text = (dr["CI_ISSUE_TITLE"] is DBNull ? "" : dr["CI_ISSUE_TITLE"].ToString());
                    txtIssueDescription.Text = (dr["CI_ISSUE_DESC"] is DBNull ? "" : dr["CI_ISSUE_DESC"].ToString());
                    ddlIssueType.SelectedValue = (dr["CI_ISSUE_TYPE"] is DBNull ? "" : dr["CI_ISSUE_TYPE"].ToString());
                    ddlIsueStatus.SelectedValue = (dr["CI_ISSUE_STATUS"] is DBNull ? "" : dr["CI_ISSUE_STATUS"].ToString());
                    txtSPOCResponsible.Text = (dr["CI_SPOC_RESPONSIBLE"] is DBNull ? "" : dr["CI_SPOC_RESPONSIBLE"].ToString());
                    strSPOCReponsibleId = (dr["CI_SPOC_RESPONSIBLE_ID"] is DBNull ? "" : dr["CI_SPOC_RESPONSIBLE_ID"].ToString());
                    strSPOCReponsibleName = (dr["CI_SPOC_RESPONSIBLE_NAME"] is DBNull ? "" : dr["CI_SPOC_RESPONSIBLE_NAME"].ToString());
                    strSPOCReponsibleEmail = (dr["CI_SPOC_RESPONSIBLE_EMAIL"] is DBNull ? "" : dr["CI_SPOC_RESPONSIBLE_EMAIL"].ToString());
                    hfResPersonEdit.Value = strSPOCReponsibleId + "|" + strSPOCReponsibleName + "|" + strSPOCReponsibleEmail;
                    hfFlag.Value = "";

                    #region Files
                    script += "\r\n <script type=\"text/javascript\">\r\n";
                    litAttachment.Text = getAttachmentHTMLTable();
                    dtFiles = oBLL.getIssueFiles(intDraftedFileId, 0, "ComplianceIssue", "Compliance Review Issue Tracker - File Type");
                    for (int i = 0; i < dtFiles.Rows.Count; i++)
                    {
                        uniqueRowId = uniqueRowId + 1;
                        drFiles = dtFiles.Rows[i];

                        sbAttachmentHTMLRows.Append(" <tr><td class='contentBody'> " +
                                                    " <input type='hidden' ID='uniqueRowId" + uniqueRowId + "' value='" + uniqueRowId + "' /> " +
                                                    " <input type='hidden' ID='attachId" + uniqueRowId + "' value='" + drFiles["CIF_ID"].ToString() + "' /> " +
                                                    " <input type='checkbox' ID='checkAttachment" + uniqueRowId + "' values='0' /></td> " +
                                                    " <td class='contentBody'> " +
                                                    " <input type='hidden' ID='attachClientFileName" + uniqueRowId + "' value='" + drFiles["CIF_CLIENT_FILE_NAME"].ToString() + "'/> " +
                                                    " <input type='hidden' ID='attachServerFileName" + uniqueRowId + "' value='" + drFiles["CIF_SERVER_FILE_NAME"].ToString() + "'/> " +
                                                    " <input type='hidden' ID='attachFileTypeID" + uniqueRowId + "' value='" + drFiles["CIF_FILE_TYPE"].ToString() + "'/> " +
                                                    " <input type='hidden' ID='attachFileDesc" + uniqueRowId + "' value='" + drFiles["CIF_DESC"].ToString() + "'/> " + drFiles["FileType"].ToString() + " </td> " +
                                                    " <td class='contentBody'>" + drFiles["CIF_DESC"].ToString().Replace(Environment.NewLine, "<br />") + "</td> " +
                                                    " <td class='contentBody'><a id='attachfilelink" + uniqueRowId + "' href='../CommonDownload.aspx?type=ComplianceIssue&downloadFileName=" + drFiles["CIF_SERVER_FILE_NAME"].ToString() +
                                                    "&Filename=" + drFiles["CIF_CLIENT_FILE_NAME"].ToString() + "'>" + drFiles["CIF_CLIENT_FILE_NAME"].ToString() + "</a></td></tr> ");
                    }

                    litAttachment.Text += sbAttachmentHTMLRows.ToString() + "</table>";

                    script += "</script>\r\n";
                    ClientScript.RegisterStartupScript(this.GetType(), "return script", script);
                    #endregion
                }
                else if (hfSelectedOperation.Value == "Copy")
                {
                    dt = oBLL.getIssue(intDraftedFileId, intRefId, strCreateBy, "CREATED_SELF", "");
                    dr = dt.Rows[0];
                    txtIssueTitle.Text = (dr["CI_ISSUE_TITLE"] is DBNull ? "" : dr["CI_ISSUE_TITLE"].ToString());
                    txtIssueDescription.Text = (dr["CI_ISSUE_DESC"] is DBNull ? "" : dr["CI_ISSUE_DESC"].ToString());
                    ddlIssueType.SelectedValue = (dr["CI_ISSUE_TYPE"] is DBNull ? "" : dr["CI_ISSUE_TYPE"].ToString());
                    ddlIsueStatus.SelectedValue = (dr["CI_ISSUE_STATUS"] is DBNull ? "" : dr["CI_ISSUE_STATUS"].ToString());

                    hfFlag.Value = "";
                    hfRDIId.Value = "";

                    #region Files
                    script += "\r\n <script type=\"text/javascript\">\r\n";
                    litAttachment.Text = getAttachmentHTMLTable();
                    dtFiles = oBLL.getIssueFiles(intDraftedFileId, 0, "ComplianceIssue", "Compliance Review Issue Tracker - File Type");
                    for (int i = 0; i < dtFiles.Rows.Count; i++)
                    {
                        uniqueRowId = uniqueRowId + 1;
                        drFiles = dtFiles.Rows[i];

                        sbAttachmentHTMLRows.Append(" <tr><td class='contentBody'> " +
                                                    " <input type='hidden' ID='uniqueRowId" + uniqueRowId + "' value='" + uniqueRowId + "' /> " +
                                                    " <input type='hidden' ID='attachId" + uniqueRowId + "' value='" + drFiles["CIF_ID"].ToString() + "' /> " +
                                                    " <input type='checkbox' ID='checkAttachment" + uniqueRowId + "' values='0' /></td> " +
                                                    " <td class='contentBody'> " +
                                                    " <input type='hidden' ID='attachClientFileName" + uniqueRowId + "' value='" + drFiles["CIF_CLIENT_FILE_NAME"].ToString() + "'/> " +
                                                    " <input type='hidden' ID='attachServerFileName" + uniqueRowId + "' value='" + drFiles["CIF_SERVER_FILE_NAME"].ToString() + "'/> " +
                                                    " <input type='hidden' ID='attachFileTypeID" + uniqueRowId + "' value='" + drFiles["CIF_FILE_TYPE"].ToString() + "'/> " +
                                                    " <input type='hidden' ID='attachFileDesc" + uniqueRowId + "' value='" + drFiles["CIF_DESC"].ToString() + "'/> " + drFiles["CIF_FILE_TYPE"].ToString() + " </td> " +
                                                    " <td class='contentBody'>" + drFiles["CIF_DESC"].ToString().Replace(Environment.NewLine, "<br />") + "</td> " +
                                                     " <td class='contentBody'><a id='attachfilelink" + uniqueRowId + "' href='../CommonDownload.aspx?type=ComplianceIssue&downloadFileName=" + drFiles["CIF_SERVER_FILE_NAME"].ToString() +
                                                    "&Filename=" + drFiles["CIF_CLIENT_FILE_NAME"].ToString() + "'>" + drFiles["CIF_CLIENT_FILE_NAME"].ToString() + "</a></td></tr> ");
                    }

                    litAttachment.Text += sbAttachmentHTMLRows.ToString() + "</table>";

                    script += "</script>\r\n";
                    ClientScript.RegisterStartupScript(this.GetType(), "return script", script);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";

                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }


        protected void btnSubmitForIdentification_Click(object sender, EventArgs e)
        {
            int intRRId = 0, intRDIId = 0;
            bool res = false;
            string strLoggedInUser = "", strIsValidated = "false", strMsg = "";
            string strRDIID = "", strRDIID1 = "";

            try
            {
                res = int.TryParse(hfRefID.Value, out intRRId);
                res = int.TryParse(hfRDIId.Value, out intRDIId);
                strLoggedInUser = (new Authentication()).getUserFullName(Page.User.Identity.Name);

                foreach (GridViewRow gvr in gvDraftedIssue.Rows)
                {
                    int intRDIID = 0;
                    HiddenField hfStatus = (HiddenField)(gvr.FindControl("hfStatus"));
                    HiddenField hfRDIID = (HiddenField)(gvr.FindControl("hfRDIID"));
                    CheckBox RowLevelCheckBox = (CheckBox)gvr.FindControl("RowLevelCheckBox");
                    bool val = int.TryParse(hfRDIID.Value.ToString(), out intRDIID);
                    if (RowLevelCheckBox.Checked)
                    {
                        if (hfStatus.Value.ToLower().Equals("a"))
                        {
                            strIsValidated = "true";
                            //break;
                        }
                        if (strIsValidated.Equals("true"))
                        {
                            oBLL.submitForOperation(intRDIID, "B", "SendToUS", strLoggedInUser, intRDIID);
                            strRDIID += intRDIID.ToString() + ",";
                        }
                    }
                }
                if (strIsValidated.Equals("true"))
                {
                    if (!strRDIID.Equals(""))
                    {
                        strRDIID1 = strRDIID.Substring(0, strRDIID.Length - 1);

                        //for change status of compliance review [Issue report released]
                        int intRiskReviewId = 0;
                        bool isRefid = int.TryParse(hfRefID.Value.ToString(), out intRiskReviewId);
                        string strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                        string strCreator = Page.User.Identity.Name;
                        int intRRRowId = oBLL.submitForOperation(0, "CR_D", "UpdateRecordStatus1", strCreateBy, intRiskReviewDraftId: intRiskReviewId);

                        sendMailOnIssuesSubmission(intRiskReviewId);
                    }
                    strMsg = "Selected Issue details have been submitted successfully";
                    ClientScript.RegisterStartupScript(this.GetType(), "pop", "alert('" + strMsg + ".'); document.getElementById('btnCancel').click();", true);
                }
                else
                {
                    strMsg = "All Issues are submitted, please add new issues to submit";

                    ClientScript.RegisterStartupScript(this.GetType(), "pop", "alert('" + strMsg + ".');", true);
                }
                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";

                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }


        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (hfType.Value.Equals("Add"))
                Response.Redirect("AddEditIssuesDetails.aspx?RefId=" + hfRefID.Value + "&Type=" + hfType.Value +
                    "&Source=" + hfSource.Value + "&ViewType=" + hfViewType.Value + "&RRStatus=" + hfRRStatus.Value);
            else if (hfType.Value.Equals("View"))
                Response.Redirect("AddEditIssuesDetails.aspx?RefId=" + hfRefID.Value + "&Type=" + hfType.Value);
        }

        protected void btnAccept_ServerClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfIssueId.Value))
            {
                //for accept issue
                int intIssueId = 0;
                bool isRefid = int.TryParse(hfIssueId.Value.ToString(), out intIssueId);
                string strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                string strCreator = Page.User.Identity.Name;
                int intRRRowId = oBLL.submitForOperation(intIssueId, "F", "IssueAccept", strCreateBy, strValue1: hfModalStatus_txt.Value);
                sendMailOnIssuesAcceptance(Convert.ToString(intIssueId));
                ClientScript.RegisterStartupScript(this.GetType(), "pop", "alert('Accepted Successfully..');", true);
                txtStatusRemark.Text = string.Empty;
                btnRefresh_Click(sender, e);
            }
        }

        protected void btnReject_ServerClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfIssueId.Value))
            {
                //for accept issue
                int intIssueId = 0;
                bool isRefid = int.TryParse(hfIssueId.Value.ToString(), out intIssueId);
                string strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                string strCreator = Page.User.Identity.Name;
                int intRRRowId = oBLL.submitForOperation(intIssueId, "G", "IssueReject", strCreateBy, strValue1: hfModalStatus_txt.Value);
                sendMailOnIssuesRejection(Convert.ToString(intIssueId));
                ClientScript.RegisterStartupScript(this.GetType(), "pop", "alert('Rejected Successfully..');", true);
                txtStatusRemark.Text = string.Empty;
                btnRefresh_Click(sender, e);
            }
        }


        private void sendMailOnIssuesSubmission(int intRRId)
        {
            string strUnitId = "", strUnitName = "", strInitiator = "", strRRMId = "", strProcess = "";
            DataTable dt = new DataTable();
            DataTable dtDistinctUnits = new DataTable();
            MailContent_ComplianceReview mailContent = new MailContent_ComplianceReview();
            string strLoggedInUser = (new Authentication()).getUserFullName(Page.User.Identity.Name);
            try
            {
                dt = oBLL.getIssue(0, intRRId, strLoggedInUser, "CREATED_SELF", null);
                strRRMId = dt.Rows[0]["CCR_CRM_ID"].ToString();
                strProcess = dt.Rows[0]["CRUM_UNIVERSE_TO_BE_REVIEWED"].ToString();

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
                    //mailContent.ParamMap.Add("ConfigId", "1096");
                    mailContent.ParamMap.Add("ConfigId", "1106");
                    mailContent.ParamMap.Add("To", "SPOC"); // Unit Head
                    mailContent.ParamMap.Add("cc", "UH,L0,L1"); // SPOC Responsible, L0 User and L1 User
                    mailContent.ParamMap.Add("SPOCId", Page.User.Identity.Name);
                    //mailContent.ParamMap.Add("RequestorId", strInitiator);
                    mailContent.ParamMap.Add("ReviewerMasId", strRRMId);
                    mailContent.ParamMap.Add("RDIIds", strRDIIds);
                    mailContent.ParamMap.Add("RRIds", intRRId);
                    mailContent.ParamMap.Add("UnitId", strUnitId);
                    mailContent.ParamMap.Add("UnitName", strUnitName);
                    mailContent.ParamMap.Add("Type", "US");
                    mailContent.ParamMap.Add("Role", "Unit SPOC");
                    mailContent.ParamMap.Add("Process", strProcess);
                    mailContent.ParamMap.Add("ActionType", "SubmitIssueTracker");
                    mailContent.ParamMap.Add("LoggedInUserName", (new Authentication()).GetUserDetsByEmpCode(Page.User.Identity.Name).Split('|')[0]);
                    mailContent.setComplianceReviewMailContent();
                }

                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";

                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }



        private void sendMailOnIssuesRejection(string issueids)
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
                    mailContent.ParamMap.Add("ConfigId", "1102");
                    mailContent.ParamMap.Add("To", "SPOC"); // SPOC
                    mailContent.ParamMap.Add("cc", "Requestor,UH"); // Requestor, UH
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

        private void sendMailOnIssuesAcceptance(string issueids)
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
                    mailContent.ParamMap.Add("ConfigId", "1103");
                    mailContent.ParamMap.Add("To", "SPOC"); // Creator
                    mailContent.ParamMap.Add("cc", "UH,L1,L0"); // SPOC Responsible, UH and L1 User
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