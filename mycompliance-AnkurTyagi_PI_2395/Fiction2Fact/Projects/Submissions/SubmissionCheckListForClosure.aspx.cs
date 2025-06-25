using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.App_Code;
using Fiction2Fact.F2FControls;
using Fiction2Fact.Legacy_App_Code.DAL;
using System.Text.RegularExpressions;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class SubmissionCheckListForClosure : System.Web.UI.Page
    {
        string strConnectionString = null;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
        RefCodesBLL refBL = new RefCodesBLL();
        UtilitiesBLL utilBLL = new UtilitiesBLL();
        UtilitiesVO utliVo = new UtilitiesVO();

        //<< Added by Vivek on 22-Jun-2017
        string strTableCellCSS = " style=\"font-size: 12px; font-family: Calibri; border-width: " +
                       "1px; padding: 8px; border-style: solid;" +
                       "border-color: #bcaf91;\"";

        string strTableHeaderCSS = " style =\"font-size: 12px; font-family: Calibri; " +
                                "background-color: #ded0b0; border-width: 1px; padding: 8px;" +
                                "border-style: solid; border-color: #bcaf91; text-align: left;\"";

        string strTableCSS = " style=\"font-size: 12px; font-family: Calibri; color: #333333; " +
                                "width: 100%; border-width: 1px; border-color: #bcaf91; border-collapse: collapse;\"";
        //>>



        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hfCurDate.Value = System.DateTime.Now.ToString("dd-MMM-yyyy");

                //<< Added By Milan Yadav on 27Apr2016
                //<<
                ListItem li = new ListItem();
                li.Text = "(Select)";
                li.Value = "";

                //<< Modified By Vivek on 16-Jan-2018
                DataTable dtFinYear = utilBL.getDataset("AllFinYears", strConnectionString).Tables[0];
                ddlFinYear.DataSource = dtFinYear;
                ddlFinYear.DataBind();
                ddlFinYear.Items.Insert(0, li);

                for (int i = 0; i < dtFinYear.Rows.Count; i++)
                {
                    if (dtFinYear.Rows[i]["FYM_STATUS"].ToString().Equals("A"))
                    {
                        ddlFinYear.SelectedValue = dtFinYear.Rows[i]["FYM_ID"].ToString();
                    }
                }
                //>>

                //<<Added by Ashish Mishra on 23Aug2017
                DataTable dtModeofFiling = refBL.getRefCodeDetails("Mode of Filing", strConnectionString);
                Session["ModeofFiling"] = dtModeofFiling;
                //ddlModeOfFiling.DataSource = refBL.getRefCodeDetails("Mode of Filing", strConnectionString);
                //ddlModeOfFiling.DataBind();
                //ddlModeOfFiling.Items.Insert(0, li);
                //>>
                if (User.IsInRole("FilingAdmin") || User.IsInRole("Filing_View_Only") || User.IsInRole("Filing_Sub_Admin"))
                {
                    hfUserType.Value = "Admin";
                }
                else if (User.IsInRole("FilingUser"))
                {
                    hfUserType.Value = "User";
                }
            }
        }

        protected void lbtnJan_Click(object sender, EventArgs e)
        {
            writeError("");
            hfMonth.Value = "1";
            BindGridView();

        }

        protected void lbtnFeb_Click(object sender, EventArgs e)
        {
            writeError("");
            hfMonth.Value = "2";
            BindGridView();
        }

        protected void lbtnMarch_Click(object sender, EventArgs e)
        {
            writeError("");
            hfMonth.Value = "3";
            BindGridView();
        }

        protected void lbtnApr_Click(object sender, EventArgs e)
        {
            writeError("");
            hfMonth.Value = "4";
            BindGridView();
        }

        protected void lbtnMay_Click(object sender, EventArgs e)
        {
            writeError("");
            hfMonth.Value = "5";
            BindGridView();
        }

        protected void lbtnJune_Click(object sender, EventArgs e)
        {
            writeError("");
            hfMonth.Value = "6";
            BindGridView();
        }

        protected void lbtnJuly_Click(object sender, EventArgs e)
        {
            writeError("");
            hfMonth.Value = "7";
            BindGridView();
        }

        protected void lbtnAug_Click(object sender, EventArgs e)
        {
            writeError("");
            hfMonth.Value = "8";
            BindGridView();
        }

        protected void lbtnSep_Click(object sender, EventArgs e)
        {
            writeError("");
            hfMonth.Value = "9";
            BindGridView();
        }

        protected void lbtnOct_Click(object sender, EventArgs e)
        {
            writeError("");
            hfMonth.Value = "10";
            BindGridView();
        }

        protected void lbtnNov_Click(object sender, EventArgs e)
        {
            writeError("");
            hfMonth.Value = "11";
            BindGridView();
        }

        protected void lbtnDec_Click(object sender, EventArgs e)
        {
            writeError("");
            hfMonth.Value = "12";
            BindGridView();

        }

        protected void BindGridView()
        {
            ListItem liChkBoxListItem;
            string strStatus = "";

            for (int i = 0; i <= chkStatus.Items.Count - 1; i++)
            {
                liChkBoxListItem = chkStatus.Items[i];
                if (liChkBoxListItem.Selected)
                {
                    strStatus = strStatus + "" + liChkBoxListItem.Value + ",";
                }
            }
            if (!strStatus.Equals(""))
            {
                strStatus = strStatus.Substring(0, strStatus.Length - 1);
            }
            DataTable dt = SubmissionMasterBLL.getChecklistForReopenClosureByMonth(hfMonth.Value, ddlFinYear.SelectedValue,
                                           Authentication.GetUserID(Page.User.Identity.Name), hfUserType.Value, "", strStatus, strConnectionString);
            gvChecklistDetails.DataSource = dt;

            gvChecklistDetails.DataBind();
            Session["List"] = dt;
            //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
            if (gvChecklistDetails.Rows.Count == 0)
            {
                writeError("No checklist available for selected month.");
                btnExportToExcel.Visible = false;
            }
            else
            {
                writeError("");
                btnExportToExcel.Visible = true;
            }
            //>>
        }

        private void writeError(string strMsg)
        {
            lblMsg.Text = strMsg;
            lblMsg.Visible = true;
        }

        protected DataTable LoadSubmissionFileList(object ScId)
        {
            DataTable dt = utilBL.getDatasetWithCondition("SUBMISSIONSFILES", Convert.ToInt32(ScId), strConnectionString);
            return dt;
        }

        protected void gvChecklistDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //<<Added by Ashish Mishra on 23Aug2017
            ListItem li = new ListItem();
            li.Text = "--Select--";
            li.Value = "";
            string strStatus = "";
            //>>
            GridViewRow gvr;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                gvr = e.Row;
                System.Data.DataRowView drv = e.Row.DataItem as System.Data.DataRowView;

                if (drv != null)
                {
                    //<<Added by Ashish Mishra on 28Aug2017
                    //string strStatus = "";
                    //>>
                    String strSubmissionId = drv["SUB_ID"].ToString();
                    //<<Added by Ashish Mishra on 27Jul2017
                    int intScId = Convert.ToInt32(drv["SC_ID"]);
                    //>>
                    //RadioButtonList rbl = (RadioButtonList)(gvr.FindControl("rblYesNoNA"));
                    DropDownList ddlYesNo = (DropDownList)(gvr.FindControl("ddlYesNoNA"));
                    ddlYesNo.SelectedValue = drv["SUB_YES_NO_NA"].ToString();

                    DropDownList ddl = (DropDownList)(gvr.FindControl("ddlStatus"));
                    LinkButton lbSave = (LinkButton)(gvr.FindControl("lbSave"));
                    //<< Added By Ashish Mishra on 27Jul2017
                    LinkButton lb1 = (LinkButton)(gvr.FindControl("lbAttach"));
                    F2FTextBox txtSubAuthorityDate = (F2FTextBox)(gvr.FindControl("txtSubAuthorityDate"));
                    //>>
                    //<<Added by Ashish Mishra on 28Jul2017 
                    F2FTextBox txtReOpenComments = (F2FTextBox)(gvr.FindControl("txtReOpenComments"));
                    //>>
                    //<<Added by Ashish Mishra on 16Aug2017
                    Image imgSubDate = (Image)(gvr.FindControl("imgSubDate"));
                    DropDownList ddlModeOfFiling = (DropDownList)(gvr.FindControl("ddlModeOfFiling"));
                    ddlModeOfFiling.DataSource = Session["ModeOfFiling"];
                    ddlModeOfFiling.DataBind();
                    ddlModeOfFiling.Items.Insert(0, li);
                    //>>

                    DropDownList ddlHygiene = (DropDownList)(gvr.FindControl("ddlHygiene"));
                    HiddenField hfSUB_HYGIENE_CHECK = (HiddenField)(gvr.FindControl("hfSUB_HYGIENE_CHECK"));
                    ddlHygiene.SelectedValue = hfSUB_HYGIENE_CHECK.Value;

                    Label lblSubmittedBy = (Label)(gvr.FindControl("lblSubmittedBy"));

                    string strLoggedinUser = Page.User.Identity.Name;

                    if (drv["SUB_STATUS"] != null || drv["SUB_STATUS"].ToString() != "")
                    {
                        ddl.SelectedValue = drv["SUB_STATUS"].ToString();
                        //<<Added by Ashish Mishra on 28Aug2017
                        strStatus = drv["SUB_STATUS"].ToString();
                        //>>
                    }


                    if (strStatus.Equals("C") || strSubmissionId == null || strSubmissionId == "")
                    {
                        ddlYesNo.Enabled = false;
                        ddl.Enabled = false;
                        ddlModeOfFiling.Enabled = false;
                        ddlHygiene.Enabled = false;
                        lbSave.Visible = false;
                        //lb1.Visible = false;
                        txtSubAuthorityDate.Enabled = false;
                        txtReOpenComments.Enabled = false;
                        imgSubDate.Visible = false;

                        lb1.Text = "View";
                        lb1.Attributes.Add("onclick", "javascript:return openSubmissionFilesWindow('" + Convert.ToString(intScId) + "','Closure','View')");
                    }
                    else
                    {
                        ddlYesNo.Enabled = false;
                        //lb1.Attributes.Add("onclick", "javascript:return openSubmissionFilesWindow('" + Convert.ToString(intScId) + "')");
                        lb1.Text = "Attach";
                        lb1.Attributes.Add("onclick", "javascript:return openSubmissionFilesWindow('" + Convert.ToString(intScId) + "','Closure','Attach')");
                    }

                    //<<Added by Ashish Mishra on 16Aug2017
                    if (drv["SUB_MODE_OF_FILING"] != null || drv["SUB_MODE_OF_FILING"].ToString() != "")
                    {
                        HiddenField hfSUB_MODE_OF_FILING = (HiddenField)(gvr.FindControl("hfSUB_MODE_OF_FILING"));
                        ddlModeOfFiling.SelectedValue = hfSUB_MODE_OF_FILING.Value;
                    }
                    //>>

                    if (drv["SUB_HYGIENE_CHECK"] != null || drv["SUB_HYGIENE_CHECK"].ToString() != "")
                    {
                        ddlHygiene.SelectedValue = drv["SUB_HYGIENE_CHECK"].ToString();
                    }

                    if (strLoggedinUser.Equals(lblSubmittedBy.Text))
                    {
                        ddl.Enabled = false;
                        lbSave.Visible = false;
                        //lb1.Visible = false;
                        lb1.Text = "View";
                        lb1.Attributes.Add("onclick", "javascript:return openSubmissionFilesWindow('" + Convert.ToString(intScId) + "','Closure','View')");
                    }

                    lbSave.Attributes.Add("OnClick", "javascript:return validateReOpenComments('" + ddl.ClientID + "','" + txtReOpenComments.ClientID + "')");
                }
            }
        }

        protected void gvChecklistDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this))
            {
                hfDoubleClickFlag.Value = "";
                return;
            }
            try
            {
                string strStatus = "", strCreateBy = "";
                GridViewRow gvr = gvChecklistDetails.SelectedRow;
                Label lbl = ((Label)(gvr.FindControl("lblSubId")));
                int intSubmissionId = Convert.ToInt32(lbl.Text);

                DropDownList ddl = (DropDownList)(gvr.FindControl("ddlStatus"));
                strStatus = ddl.SelectedValue;
                if (!Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)).Equals(""))
                {
                    strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                }
                string strModeOfFiling = "", strSubAuthorityDate = "", strHygiene = "";
                //<<Added by Ashish Mishra on 28Jul2017
                if (ddl.SelectedValue == "R" || ddl.SelectedValue == "C" || ddl.SelectedValue == "S")
                {
                    F2FTextBox txtSubAuthorityDate = (F2FTextBox)(gvr.FindControl("txtSubAuthorityDate"));
                    strSubAuthorityDate = txtSubAuthorityDate.Text;
                    DropDownList ddlModeOfFiling = (DropDownList)(gvr.FindControl("ddlModeOfFiling"));
                    strModeOfFiling = ddlModeOfFiling.SelectedValue;

                    DropDownList ddlHygiene = (DropDownList)(gvr.FindControl("ddlHygiene"));
                    strHygiene = ddlHygiene.SelectedValue;

                }
                F2FTextBox txtReOpenComments = (F2FTextBox)(gvr.FindControl("txtReOpenComments"));
                //>>

                //<<Added by Ankur Tyagi on 18-Apr-2025 for Project Id : 2395
                if (ddl.SelectedValue == "C")
                {
                    Label lblSCId = ((Label)(gvr.FindControl("lblSCId")));
                    int intSCId = Convert.ToInt32(lblSCId.Text);
                    //<<Added by Ankur Tyagi on 05-May-2025 for Project Id : 2395
                    string conditionqry = " and SF_FILE_TYPE = 'OT' and SF_OPERATION_TYPE = 'Closure'" +
                            " and SF_CREATE_DT > (SELECT MAX(SF_CREATE_DT) FROM TBL_SUBMISSION_FILES " +
                            " WHERE SF_OPERATION_TYPE IN ('Submission', 'Resubmission') AND SF_SC_ID = " + intSCId + ")" +
                            " AND SF_SC_ID = " + intSCId;

                    DataTable dtSubFiles = utilBL.getDatasetWithConditionInString("SUBMISSIONSFILESFromType",
                             conditionqry, strConnectionString);
                    if (dtSubFiles.Rows.Count <= 0)
                    {
                        hfDoubleClickFlag.Value = "";
                        writeError("Attachment is compulsory for closure.");
                        return;
                    }
                    //>>
                }
                //>>
                
                SubmissionMasterBLL.saveAdminChecklist(intSubmissionId, strStatus, strCreateBy, strSubAuthorityDate, txtReOpenComments.Text,
                                                        strModeOfFiling, "", strHygiene, strConnectionString);

                if (strStatus.Equals("C"))
                {
                    sendClosureMail();
                }
                if (strStatus.Equals("SR"))
                {
                    sendReopenMail();
                }
                hfDoubleClickFlag.Value = "";
                BindGridView();
                writeError("Submission updated successfully.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void gvChecklistDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            //DataView dvDataView = new DataView(utilBL.getDatasetWithCondition("getChecklistForAdminByMonth", Convert.ToInt32(hfMonth.Value), strConnectionString));
            DataTable dt = (DataTable)Session["List"];
            DataView dvDataView = new DataView(dt);
            if (Convert.ToString(Session["sort"]) == "" || Convert.ToString(Session["sort"]) == "ASC")
            {
                e.SortDirection = SortDirection.Ascending;
                dvDataView.Sort = (e.SortExpression + (" " + ConvertSortDirectionToSql(e.SortDirection)));
                Session["sort"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Descending;
                dvDataView.Sort = (e.SortExpression + (" " + ConvertSortDirectionToSql(e.SortDirection)));
                Session["sort"] = "ASC";
            }
            gvChecklistDetails.DataSource = dvDataView;
            gvChecklistDetails.DataBind();
        }

        private string ConvertSortDirectionToSql(SortDirection SortDirection)
        {
            string m_SortDirection = String.Empty;
            switch (SortDirection)
            {
                case SortDirection.Ascending:
                    m_SortDirection = "DESC";
                    break;
                case SortDirection.Descending:
                    m_SortDirection = "ASC";
                    break;
            }
            return m_SortDirection;
        }

        private void sendClosureMail()
        {
            try
            {
                string[] strTo = new string[0];
                string[] strCC;
                string strContent = "";
                string[] strUsers = new string[0];
                string strSubject;
                string[] strDetailsList;
                string strSubChklistId;

                GridViewRow gvr = gvChecklistDetails.SelectedRow;
                strSubChklistId = gvChecklistDetails.SelectedValue.ToString();
                string strStmID = (((Label)(gvr.FindControl("lbStmId"))).Text);
                string strSrdId = (((Label)(gvr.FindControl("lblSrdId"))).Text);
                string strTrackdept = (((Label)(gvr.FindControl("lblstmType"))).Text);
                string strSegment = (((Label)(gvr.FindControl("lblSegment"))).Text);
                string strParticulars = (((Label)(gvr.FindControl("lblParticulars"))).Text);
                string strDescription = (((Label)(gvr.FindControl("lblDescription"))).Text);
                string strFrequnecy = (((Label)(gvr.FindControl("lblFrequency"))).Text);
                string strModeofFiling = ((DropDownList)(gvr.FindControl("ddlModeOfFiling"))).SelectedItem.Text;
                string strSubAuthorityDate = ((F2FTextBox)(gvr.FindControl("txtSubAuthorityDate"))).Text;
                string strReOpenComments = ((F2FTextBox)(gvr.FindControl("txtReOpenComments"))).Text;
                string strDuedate = ((Label)(gvr.FindControl("lblDueDateTo"))).Text;
                //string strComplianceStatus = ((RadioButtonList)(gvr.FindControl("rblYesNoNA"))).SelectedItem.Text;
                string strComplianceStatus = ((DropDownList)(gvr.FindControl("ddlYesNoNA"))).SelectedItem.Text;
                string strHygiene = ((DropDownList)(gvr.FindControl("ddlHygiene"))).SelectedItem.Text;

                MailConfigBLL mailBLL = new MailConfigBLL();
                DataTable dtMailConfig = mailBLL.searchMailConfig(null, "Closure of Submission");
                if (dtMailConfig == null || dtMailConfig.Rows.Count == 0)
                {
                    writeError("Mail Configuration is not set, could not send mail");
                    return;
                }
                DataRow drMailConfig = dtMailConfig.Rows[0];
                strSubject = drMailConfig["MCM_SUBJECT"].ToString();
                strContent = drMailConfig["MCM_CONTENT"].ToString();

                int i = 0, j = 0;
                Mail mm = new Mail();

                DataTable dtL0RD = utilBLL.getDatasetWithTwoConditionInString("getReportingDeptUserFromLevel", strSrdId, "0", strConnectionString);
                strTo = new string[dtL0RD.Rows.Count];
                foreach (DataRow drReportingDeptOwners in dtL0RD.Rows)
                {
                    strTo[i] = Convert.ToString(drReportingDeptOwners["EmailId"]);
                    i = i + 1;
                }

                //L1 Reporting Dept
                //All Tracking Dept

                DataTable dtL1RD = utilBLL.getDatasetWithTwoConditionInString("getReportingDeptUserFromLevel", strSrdId, "1", strConnectionString);
                DataTable dtAllTDU = SubmissionMasterBLL.getTrackingDeptOwners(strSubChklistId, strConnectionString);
                strCC = new string[dtL1RD.Rows.Count + dtAllTDU.Rows.Count];
                foreach (DataRow drL1RD in dtL1RD.Rows)
                {
                    strCC[j] = Convert.ToString(drL1RD["EmailId"]);
                    j = j + 1;
                }
                foreach (DataRow drTrackingDeptOwners in dtAllTDU.Rows)
                {
                    strCC[j] = Convert.ToString(drTrackingDeptOwners["EM_EMAIL"]);
                    j = j + 1;
                }


                string strUserName = Page.User.Identity.Name.ToString();
                Authentication auth = new Authentication();
                string strDetails = auth.GetUserDetsByEmpCode(strUserName);
                strDetailsList = strDetails.Split('|');
                string strUser = Convert.ToString(strDetailsList[0]);

                string strSubTable = "<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>" +
                            "<th " + strTableHeaderCSS + ">Department</th>" +
                            "<th " + strTableHeaderCSS + ">Reporting to</th>" +
                            "<th " + strTableHeaderCSS + ">Particulars</th>" +
                            //"<th " + strTableHeaderCSS + ">Description</th>" +
                            "<th " + strTableHeaderCSS + ">Regulatory Due date </th>" +
                            "<th " + strTableHeaderCSS + ">Compliance Status </th>" +
                            "<th " + strTableHeaderCSS + ">Hygiene Check</th>" +
                            "<th " + strTableHeaderCSS + ">Mode of Filing</th>" +
                            "<th " + strTableHeaderCSS + ">Frequency</th>" +
                            "<th " + strTableHeaderCSS + ">Submitted to Authority On / Date of Receipt of Data</th>" +
                            "<th " + strTableHeaderCSS + ">Comments</th></tr>" +
                            "<tr><td " + strTableCellCSS + ">" + 1 + "</td>" +
                            "<td " + strTableCellCSS + ">" + strTrackdept + "</td>" +
                            "<td " + strTableCellCSS + ">" + strSegment + "</td>" +
                            "<td " + strTableCellCSS + ">" + strParticulars + "</td>" +
                            //"<td " + strTableCellCSS + ">" + strDescription + "</td>" +
                            "<td " + strTableCellCSS + ">" + strDuedate + "</td>" +
                            "<td " + strTableCellCSS + ">" + strComplianceStatus + "</td>" +
                            "<td " + strTableCellCSS + ">" + strHygiene + "</td>" +
                            "<td " + strTableCellCSS + ">" + strModeofFiling + "</td>" +
                            "<td " + strTableCellCSS + ">" + strFrequnecy + "</td>" +
                            "<td " + strTableCellCSS + ">" + strSubAuthorityDate + "</td>" +
                            "<td " + strTableCellCSS + ">" + strReOpenComments.Replace(Environment.NewLine, "<br />") + "</td></tr>" +
                            "</table>";

                strSubject = strSubject.Replace("%Authority%", strSegment);
                strContent = strContent.Replace("%Authority%", strSegment);
                strContent = strContent.Replace("%SubmittedBy%", Getfullname(strUser) + " on " + System.DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));
                strContent = strContent.Replace("%SubmissionTable%", strSubTable);
                strContent = strContent.Replace("%Footer%", ConfigurationManager.AppSettings["MailFooter"].ToString());
                strContent = strContent.Replace("%Link%", "<a href=" + Global.site_url("Projects/Submissions/ComplianceChecklist.aspx?Type=4") + " target=\"_blank\">Click here</a>");

                mm.sendAsyncMail(strTo, strCC, null, strSubject, strContent);
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        private void sendReopenMail()
        {
            try
            {
                string[] strTo = new string[0];
                string[] strCC;
                string strContent = "";
                string[] strUsers = new string[0];
                string strSubject;
                string[] strDetailsList;
                string[] strSubmittedByDetailsList;
                string strSubChklistId;

                GridViewRow gvr = gvChecklistDetails.SelectedRow;
                strSubChklistId = gvChecklistDetails.SelectedValue.ToString();
                string strStmID = (((Label)(gvr.FindControl("lbStmId"))).Text);
                string strSrdId = (((Label)(gvr.FindControl("lblSrdId"))).Text);
                string strTrackdept = (((Label)(gvr.FindControl("lblstmType"))).Text);
                string strSegment = (((Label)(gvr.FindControl("lblSegment"))).Text);
                string strParticulars = (((Label)(gvr.FindControl("lblParticulars"))).Text);
                string strDescription = (((Label)(gvr.FindControl("lblDescription"))).Text);
                string strFrequnecy = (((Label)(gvr.FindControl("lblFrequency"))).Text);
                string strModeofFiling = ((DropDownList)(gvr.FindControl("ddlModeOfFiling"))).SelectedItem.Text;
                string strSubAuthorityDate = ((F2FTextBox)(gvr.FindControl("txtSubAuthorityDate"))).Text;
                string strReOpenComments = ((F2FTextBox)(gvr.FindControl("txtReOpenComments"))).Text;
                string strDuedate = ((Label)(gvr.FindControl("lblDueDateTo"))).Text;
                //string strComplianceStatus = ((RadioButtonList)(gvr.FindControl("rblYesNoNA"))).SelectedItem.Text;
                string strComplianceStatus = ((DropDownList)(gvr.FindControl("ddlYesNoNA"))).SelectedItem.Text;
                string strReportingDept = (((Label)(gvr.FindControl("lblReportingDept"))).Text);
                string strSubmittedBy = (((Label)(gvr.FindControl("lblSubmittedBy"))).Text);
                string lblSubmittedBy1 = (((Label)(gvr.FindControl("lblSubmittedBy1"))).Text);

                MailConfigBLL mailBLL = new MailConfigBLL();
                DataTable dtMailConfig = mailBLL.searchMailConfig(null, "Send back Submission");
                if (dtMailConfig == null || dtMailConfig.Rows.Count == 0)
                {
                    writeError("Mail Configuration is not set, could not send mail");
                    return;
                }
                DataRow drMailConfig = dtMailConfig.Rows[0];
                strSubject = drMailConfig["MCM_SUBJECT"].ToString();
                strContent = drMailConfig["MCM_CONTENT"].ToString();

                int i = 0, j = 0;
                Mail mm = new Mail();
                Authentication auth = new Authentication();

                DataTable dtL0RD = utilBLL.getDatasetWithTwoConditionInString("getReportingDeptUserFromLevel", strSrdId, "0", strConnectionString);
                strTo = new string[dtL0RD.Rows.Count];
                foreach (DataRow drReportingDeptOwners in dtL0RD.Rows)
                {
                    strTo[i] = Convert.ToString(drReportingDeptOwners["EmailId"]);
                    i = i + 1;
                }

                //L1 Reporting Dept
                //All Tracking Dept

                DataTable dtL1RD = utilBLL.getDatasetWithTwoConditionInString("getReportingDeptUserFromLevel", strSrdId, "1", strConnectionString);
                DataTable dtAllTDU = SubmissionMasterBLL.getTrackingDeptOwners(strSubChklistId, strConnectionString);
                strCC = new string[dtL1RD.Rows.Count + dtAllTDU.Rows.Count];
                foreach (DataRow drL1RD in dtL1RD.Rows)
                {
                    strCC[j] = Convert.ToString(drL1RD["EmailId"]);
                    j = j + 1;
                }
                foreach (DataRow drTrackingDeptOwners in dtAllTDU.Rows)
                {
                    strCC[j] = Convert.ToString(drTrackingDeptOwners["EM_EMAIL"]);
                    j = j + 1;
                }

                string strUserName = Page.User.Identity.Name.ToString();

                string strDetails = auth.GetUserDetsByEmpCode(strUserName);
                strDetailsList = strDetails.Split('|');
                string strUser = Convert.ToString(strDetailsList[0]);

                strSubject = strSubject.Replace("%Department%", strReportingDept);
                strSubject = strSubject.Replace("%SubmittedBy%", Getfullname(strUser));

                strContent = strContent.Replace("%Department%", strReportingDept);
                strContent = strContent.Replace("%SubmittedBy%", Getfullname(strUser) + " on " + System.DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));

                string strSubTable = "<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>" +
                            "<th " + strTableHeaderCSS + ">Department</th>" +
                            "<th " + strTableHeaderCSS + ">Reporting to</th>" +
                            "<th " + strTableHeaderCSS + ">Particulars</th>" +
                            //"<th " + strTableHeaderCSS + ">Description</th>" +
                            "<th " + strTableHeaderCSS + ">Regulatory Due date </th>" +
                            "<th " + strTableHeaderCSS + ">Compliance Status </th>" +
                            "<th " + strTableHeaderCSS + ">Frequency</th>" +
                            "<th " + strTableHeaderCSS + ">Comments</th></tr>" +
                            "<tr><td " + strTableCellCSS + ">" + 1 + "</td>" +
                            "<td " + strTableCellCSS + ">" + strTrackdept + "</td>" +
                            "<td " + strTableCellCSS + ">" + strSegment + "</td>" +
                            "<td " + strTableCellCSS + ">" + strParticulars + "</td>" +
                            //"<td " + strTableCellCSS + ">" + strDescription + "</td>" +
                            "<td " + strTableCellCSS + ">" + strDuedate + "</td>" +
                            "<td " + strTableCellCSS + ">" + strComplianceStatus + "</td>" +
                            "<td " + strTableCellCSS + ">" + strFrequnecy + "</td>" +
                            "<td " + strTableCellCSS + ">" + strReOpenComments.Replace(Environment.NewLine, "<br />") + "</td></tr>" +
                            "</table>";

                strSubject = strSubject.Replace("%Authority%", strSegment);

                strContent = strContent.Replace("%Authority%", strSegment);
                strContent = strContent.Replace("%SubmissionTable%", strSubTable);
                strContent = strContent.Replace("%Footer%", ConfigurationManager.AppSettings["MailFooter"].ToString());
                strContent = strContent.Replace("%Link%", "<a href=" + Global.site_url("Projects/Submissions/ComplianceChecklist.aspx?Type=4") + " target=\"_blank\">Click here</a>");

                mm.sendAsyncMail(strTo, strCC, null, strSubject, strContent);
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        public static string Getfullname(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                Authentication auth = new Authentication();
                return auth.getUserFullName(s);
            }
            else
            {
                return "";
            }
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindGridView();
        }
        //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvChecklistDetails.AllowPaging = false;
            gvChecklistDetails.AllowSorting = false;
            BindGridView();
            string attachment = "attachment; filename=Reports.xls";

            Response.ClearContent();

            Response.AddHeader("content-disposition", attachment);

            Response.ContentType = "application/ms-excel";

            //string tab = "";
            string strHTML = "";
            strHTML = "<table border=\"1\"> <tr>";
            for (int i = 1; i <= gvChecklistDetails.Columns.Count; i++)
            {
                if ((i.Equals(1)) || (i.Equals(3)) || (i.Equals(17)) || (i.Equals(18)) || (i.Equals(19))
                    || (i.Equals(27)))
                {
                }
                else
                {
                    strHTML += "<td><b>" + gvChecklistDetails.Columns[i].HeaderText;
                    strHTML += "</b></td>";
                }
            }
            strHTML += "</tr>";

            foreach (GridViewRow gvr in gvChecklistDetails.Rows)
            {
                //<< Added by Vivek on 27-Jun-2017
                string strYesNoNA = "";
                //RadioButtonList rblYesNoNA = (RadioButtonList)(gvr.FindControl("rblYesNoNA"));
                DropDownList ddlYesNoNA = (DropDownList)(gvr.FindControl("ddlYesNoNA"));

                if ((ddlYesNoNA.SelectedItem != null) && (ddlYesNoNA.SelectedItem.Text != "-Select-"))
                {
                    strYesNoNA = ddlYesNoNA.SelectedItem.Text;
                }
                //>>

                strHTML += "<tr>";
                strHTML += "<td>" + ((Label)gvr.FindControl("lblSrNo")).Text + "</td>";
                strHTML += "<td>" + ((Label)gvr.FindControl("lblstmType")).Text + "</td>";
                strHTML += "<td>" + ((Label)gvr.FindControl("lblSegment")).Text + "</td>";
                strHTML += "<td>" + ((Label)gvr.FindControl("lblReference1")).Text.Replace(Environment.NewLine, "<br/>") + "</td>";
                strHTML += "<td>" + ((Label)gvr.FindControl("lblSection1")).Text.Replace(Environment.NewLine, "<br/>") + "</td>";
                strHTML += "<td>" + ((Label)gvr.FindControl("lblParticulars1")).Text.Replace(Environment.NewLine, "<br/>") + "</td>";
                strHTML += "<td>" + ((Label)gvr.FindControl("lblDescription1")).Text.Replace(Environment.NewLine, "<br/>") + "</td>";
                strHTML += "<td>" + ((Label)gvr.FindControl("lblFrequency")).Text + "</td>";
                strHTML += "<td>" + ((Label)gvr.FindControl("lblDueDateFrom")).Text + "</td>";
                strHTML += "<td>" + ((Label)gvr.FindControl("lblDueDateTo")).Text + "</td>";
                strHTML += "<td>" + strYesNoNA + "&nbsp;</td>";
                strHTML += "<td>" + ((Label)gvr.FindControl("lblRemarks")).Text.Replace(Environment.NewLine, "<br/>") + "&nbsp;</td>";
                strHTML += "<td>" + ((Label)gvr.FindControl("lblSubmittedBy")).Text + "</td>";
                strHTML += "<td>" + ((Label)gvr.FindControl("lblSubmittedOn")).Text + "</td>";
                strHTML += "<td>" + ((DropDownList)gvr.FindControl("ddlHygiene")).SelectedItem.Text + "</td>";
                strHTML += "<td>" + ((DropDownList)gvr.FindControl("ddlModeOfFiling")).SelectedItem.Text + "</td>";

                strHTML += "<td>" + ((F2FTextBox)gvr.FindControl("txtSubAuthorityDate")).Text + "</td>";

                strHTML += "<td>" + ((Label)gvr.FindControl("lblReOpenComments")).Text.Replace(Environment.NewLine, "<br/>") + "</td>";
                strHTML += "<td>" + ((F2FTextBox)gvr.FindControl("txtReOpenComments")).Text + "</td>";

                strHTML += "<td>" + ((DropDownList)gvr.FindControl("ddlStatus")).SelectedItem.Text + "</td>";

                strHTML += "</tr>";

            }
            strHTML += "</table>";

            string html2 = Regex.Replace(strHTML, @"(<input type=""image""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<input class=""checkbox""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<a \/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            Response.Write(html2.ToString());

            Response.End();
            gvChecklistDetails.AllowPaging = true;
            gvChecklistDetails.AllowSorting = true;
            gvChecklistDetails.DataBind();
        }
        //>>
    }
}