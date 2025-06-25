using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using Fiction2Fact.Projects.Compliance;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact.Projects.ComplianceReview
{
    public partial class EditIssueDetails : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();
        CommonMethods cm = new CommonMethods();
        RefCodesBLL refBL = new RefCodesBLL();
        string script = "";
        int intCnt = 0, intCnt_new = 0;
        int intDraftedFileId = 0, intRefId = 0;

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
                if (!string.IsNullOrEmpty(Request.QueryString["IssueId"]))
                {
                    intDraftedFileId = Convert.ToInt32(Request.QueryString["IssueId"]);
                    lblRDIId.Text = Request.QueryString["IssueId"];
                    hfIssueId.Value = Request.QueryString["IssueId"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["CRId"]))
                {
                    intRefId = Convert.ToInt32(Request.QueryString["CRId"]);
                    hfRefID.Value = Convert.ToString(Request.QueryString["CRId"]);
                }
                FillResponsibleUnit();
                FillIssueStatus();
                FillIssueType();
                FillBusinessUnits();
                litAttachment.Text = getAttachmentHTMLTable() + "</table>";
                FillIssueDetails();
                FillActions();
                
            }
        }

        void FillBusinessUnits()
        {
            DataTable dt = new DataTable();
            dt = oBLL.Search_SubFunction_Master(0, null, strCSFM_Status: "A");
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

            sbAttachmentHTML.Append(" <table id='tblAttachment' width='100%' border='0' bgcolor='#BCC6CC' cellpadding='0' cellspacing='1'> " +
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
        void FillIssueDetails()
        {
            try
            {
                int uniqueRowId = 0;
                string strSPOCReponsibleId = "", strSPOCReponsibleName = "", strSPOCReponsibleEmail = "";
                StringBuilder sbAttachmentHTMLRows = new StringBuilder();
                string strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                DataTable dt = new DataTable();
                DataRow dr, drFiles;
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
                DataTable dtFiles = new DataTable();
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
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
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



                hfDoubleClickFlag.Value = "";

                strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                            "alert('" + strMsg + "');\r\n" +
                            "</script>\r\n";

                ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);

            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            FillIssueDetails();
            FillActions();
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
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