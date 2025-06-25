using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Controls;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.App_Code;
using Fiction2Fact.F2FControls;
using iText.Kernel.XMP.Impl;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class Submissions_SearchSubmissionCheckList : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
        UtilitiesBLL utilitiesBL = new UtilitiesBLL();
        UtilitiesBLL utilBLL = new UtilitiesBLL();
        UtilitiesVO utliVo = new UtilitiesVO();
        private int intCnt = 0;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                if (Session["LoadSearchedChecklist"] != null)
                {
                    gvChecklistDetails.DataSource = (DataSet)(Session["LoadSearchedChecklist"]);
                    // gvChecklistDetails.DataBind();
                }
            }
            else
            {
                hfCurDate.Value = System.DateTime.Now.ToString("dd-MMM-yyyy"); //Added By Urvashi Gupta On 28Apr2016
                btnsearch.Attributes.Add("onmouseover", "this.src='../../Content/images/legacy/Search1.png'");
                btnsearch.Attributes.Add("onmouseout", "this.src='../../Content/images/legacy/Search.png'");
                //if (Session["LoadSearchedChecklist"] != null)
                //{
                //    gvChecklistDetails.DataSource = (DataSet)(Session["LoadSearchedChecklist"]);
                //    gvChecklistDetails.DataBind();
                //}

                //if (Session["MonthSelected"] != null)
                // {
                //     str = (string)Session["MonthSelected"];
                //     hfMonth.Value = str;
                //     //gvChecklistDetails.DataBind();
                // }

                if (Request.QueryString["Type"] != null)
                {
                    hfType.Value = Request.QueryString["Type"].ToString();
                }
                if (hfType.Value.Equals("1"))
                    lblTitle.Text = " My Search Submissions Checklist";
                else if (hfType.Value.Equals("2"))
                {
                    lblTitle.Text = "My Search Department Checklist";
                }
                else if (hfType.Value.Equals("3"))
                    lblTitle.Text = "My Search Submissions Checklist";
                else if (hfType.Value.Equals("4"))
                {
                    lblTitle.Text = "My Search Department Checklist";
                    gvChecklistDetails.Columns[2].Visible = false;
                }

                //<< Added By Vivek on 03-Jul-2017
                DataTable dtFinYear = utilitiesBL.getDataset("AllFinYears", mstrConnectionString).Tables[0];
                ddlFinYear.DataSource = dtFinYear;
                ddlFinYear.DataBind();
                ddlFinYear.Items.Insert(0, new ListItem("(Select)", ""));

                for (int i = 0; i < dtFinYear.Rows.Count; i++)
                {
                    if (dtFinYear.Rows[i]["FYM_STATUS"].ToString().Equals("A"))
                    {
                        ddlFinYear.SelectedValue = dtFinYear.Rows[i]["FYM_ID"].ToString();
                    }
                }
                //>>

                ddlSegment.DataSource = utilitiesBL.getDataset("SUBMISSIONSGMT", mstrConnectionString);
                ddlSegment.DataBind();

                ddlEvent.DataSource = utilitiesBL.getDataset("EVENT", mstrConnectionString);
                ddlEvent.DataBind();

                if (hfType.Value.Equals("2"))
                {
                    ddlReportDept.DataSource = utilitiesBL.getDataset("REPORTINGDEPT", mstrConnectionString);
                }
                else if (hfType.Value.Equals("4"))
                {
                    ddlReportDept.DataSource = utilitiesBL.getDatasetWithConditionInString("REPORTINGDEPT_BY_SRDOM_EMP_ID", Page.User.Identity.Name, mstrConnectionString);
                }
                ddlReportDept.DataBind();
            }
        }

        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }


        protected DataTable LoadSubmissionFileList(object ScId)
        {
            DataTable dtSubFiles = new DataTable();
            dtSubFiles = utilitiesBL.getDatasetWithCondition("SUBMISSIONSFILES", Convert.ToInt32(ScId), mstrConnectionString);

            return dtSubFiles;
        }

        protected void gvChecklistDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int intSubmissions;
                string strFilterExpression = "", strFilterEvent = "", strOrderBy = "";
                GridViewRow gvr = gvChecklistDetails.SelectedRow;
                string strSubmissionId = ((Label)(gvr.FindControl("lblSubId"))).Text;
                int intSCId = Convert.ToInt32(gvChecklistDetails.SelectedValue);
                //RadioButtonList rblYNNA = ((RadioButtonList)(gvr.FindControl("rblYesNoNA")));
                DropDownList rblYNNA = ((DropDownList)(gvr.FindControl("ddlYesNoNA")));
                string strRemarks = Convert.ToString(((F2FTextBox)(gvr.FindControl("txtRemarks"))).Text);
                string strUser = Page.User.Identity.Name;
                string strClientFileName = ((HiddenField)(gvr.FindControl("ClientFileName"))).Value.ToString();
                string strServerFileName = ((HiddenField)(gvr.FindControl("ServerFileName"))).Value.ToString();

                //<<Commented by Ashish Mishra on29Jul2017
                //<<Added By Urvashi Gupta On 28Apr2016
                //string strSubDate = ((F2FTextBox)(gvr.FindControl("txtSubDate"))).Text;
                //>>
                //>>

                hfSelectedRecord.Value = strSubmissionId;
                if (hfSelectedOperation.Value.Equals("Save Draft"))
                {
                    if (strSubmissionId != "")
                    {
                        int intSubId = Convert.ToInt32(strSubmissionId);
                        //<<Modified by Ashish Mishra on 27Jul2017 commented strSubDate parameter
                        intSubmissions = SubmissionMasterBLL.saveChecklistDetails(intSubId, intSCId, rblYNNA.SelectedValue,/* strSubDate,*/
                                strRemarks, "", strUser, strClientFileName, strServerFileName, mstrConnectionString);
                    }
                    else
                    {
                        intSubmissions = SubmissionMasterBLL.saveChecklistDetails(0, intSCId, rblYNNA.SelectedValue,/* strSubDate,*/
                                strRemarks, "", strUser, strClientFileName, strServerFileName, mstrConnectionString);
                    }
                    hfDoubleClickFlag.Value = "";
                }
                else if (hfSelectedOperation.Value.Equals("Submit"))
                {
                    if (strSubmissionId != "")
                    {
                        int intSubId = Convert.ToInt32(strSubmissionId);
                        intSubmissions = SubmissionMasterBLL.saveChecklistDetails(intSubId, intSCId, rblYNNA.SelectedValue,/* strSubDate,*/
                                strRemarks, "S", strUser, strClientFileName, strServerFileName, mstrConnectionString);
                    }
                    else
                    {
                        intSubmissions = SubmissionMasterBLL.saveChecklistDetails(0, intSCId, rblYNNA.SelectedValue,/* strSubDate,*/
                                strRemarks, "S", strUser, strClientFileName, strServerFileName, mstrConnectionString);
                    }
                    hfDoubleClickFlagSubmit.Value = "";
                    sendSubmissionMail();
                }



                DataSet dsChecklist = new DataSet();
                if (Session["strFilterExpression"] != null)
                {
                    strFilterExpression = Session["strFilterExpression"].ToString();
                }
                if (Session["strFilterEvent"] != null)
                {
                    strFilterEvent = Session["strFilterEvent"].ToString();
                }
                if (Session["strOrderBy"] != null)
                {
                    strOrderBy = Session["strOrderBy"].ToString();
                }

                //<<Modified by Ankur Tyagi on 18-Apr-2025 for Project Id : 2395
                //Added Global Search
                dsChecklist = SubmissionMasterBLL.SearchComplianceChecklist(strFilterExpression, Authentication.GetUserID(Page.User.Identity.Name),
                    hfType.Value.ToString(), strOrderBy, txtGlobalSearch.Text, mstrConnectionString);
                //>>

                //if (ddlType.SelectedValue == "F")
                //{
                //    dsChecklist = SubmissionMasterBLL.SearchSubmissionChecklistForFix(Authentication.GetUserID(Session["UserId"].ToString()), strFilterExpression, strOrderBy, mstrConnectionString);

                //}
                //else if (ddlType.SelectedValue == "E")
                //{
                //    dsChecklist = SubmissionMasterBLL.SearchSubmissionChecklistForEventBased(Authentication.GetUserID(Session["UserId"].ToString()), strFilterExpression, strFilterEvent, strOrderBy, mstrConnectionString);
                //}

                gvChecklistDetails.DataSource = dsChecklist;
                gvChecklistDetails.DataBind();
                hideShowButtons(dsChecklist.Tables[0]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void hideShowButtons(DataTable dsChecklist)
        {
            int index;
            int tot = dsChecklist.Rows.Count;
            DataRow dr;
            string strServerFileName, strStatus;
            string script = "\r\n<script language=\"javascript\">\r\n";
            for (int i = 0; i < tot; i++)
            {
                index = i + 2;
                dr = dsChecklist.Rows[i];
                strServerFileName = dr["SUB_SERVER_FILE_NAME"].ToString();
                strStatus = dr["SUB_STATUS"].ToString();
                if (!strStatus.Equals("S"))
                {
                    if (strServerFileName.Equals(""))
                    {
                        //script = script +
                        //   " showHideButtons('true'," + i + ");";
                        script = script + "document.getElementById('AttachFileImg" + index + "').style.visibility = 'visible';\r\n";
                    }
                    else
                    {
                        //script = script +
                        //   " showHideButtons('false'," + i + ");";
                        script = script + "document.getElementById('DeleteFileImg" + index + "').style.visibility = 'visible';\r\n";
                    }
                }
                //else
                //{
                //    script = script +
                //           " hideButtons(" + i + ");";
                //}

            }
            script = script + " </script>\r\n";
            if (tot > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "script", script);
            }
        }

        private void sendSubmissionMail()
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
                DataTable dtReportingDeptOwners = new DataTable();
                DataTable dtTrackingDeptOwners = new DataTable();
                DataTable dtComplianceUsers = new DataTable();
                DataServer dserv = new DataServer();

                GridViewRow gvr = gvChecklistDetails.SelectedRow;
                strSubChklistId = gvChecklistDetails.SelectedValue.ToString();
                string strStmID = (((Label)(gvr.FindControl("lbStmId"))).Text);
                string strSrdId = (((Label)(gvr.FindControl("lblSrdId"))).Text);
                String strParticulars = ((Label)(gvr.FindControl("lblParticulars"))).Text;
                String strDepartment = ((Label)(gvr.FindControl("lblstmType"))).Text;
                String strSegment = ((Label)(gvr.FindControl("lblSegment"))).Text;
                string strDuedate = ((Label)(gvr.FindControl("lblDueDateTo"))).Text;
                //string strComplianceStatus = ((RadioButtonList)(gvr.FindControl("rblYesNoNA"))).SelectedItem.Text;
                string strComplianceStatus = ((DropDownList)(gvr.FindControl("ddlYesNoNA"))).SelectedItem.Text;
                string strRemarks = ((F2FTextBox)(gvr.FindControl("txtRemarks"))).Text;

                MailConfigBLL mailBLL = new MailConfigBLL();
                DataTable dtMailConfig = mailBLL.searchMailConfig(null, "Completion of Due Submission");
                if (dtMailConfig == null || dtMailConfig.Rows.Count == 0)
                {
                    writeError("Mail Configuration is not set, could not send mail");
                    return;
                }
                DataRow drMailConfig = dtMailConfig.Rows[0];
                strSubject = drMailConfig["MCM_SUBJECT"].ToString();
                strContent = drMailConfig["MCM_CONTENT"].ToString();

                Mail mm = new Mail();
                //strSubject = "Completion of due Submission";

                //<<Added By Amey Karangutkar on 10-Mar-2018
                utliVo.setCode(strSrdId);
                dtReportingDeptOwners = utilBLL.getData("ReportingDeptAllLevels", utliVo);
                //>>

                dtTrackingDeptOwners = SubmissionMasterBLL.getTrackingDeptOwners(strSubChklistId, mstrConnectionString);

                if (strComplianceStatus == "NA")
                {
                    dtComplianceUsers = dserv.Getdata("select EM_EMPNAME, EM_EMAIL From " +
                                        " EmployeeMaster " +
                                        " inner join TBL_EM_STM_MAPPING on " +
                                        " ESM_EM_ID = EM_ID and ESM_STM_ID = 2 WHERE EM_STATUS='A' ");
                }
                //strTo = new string[dtReportingDeptOwners.Rows.Count];
                //strCC = new string[dtTrackingDeptOwners.Rows.Count];

                strTo = new string[dtTrackingDeptOwners.Rows.Count];
                strCC = new string[dtReportingDeptOwners.Rows.Count + dtComplianceUsers.Rows.Count];

                int i = 0, j = 0;
                foreach (DataRow drReportingDeptOwners in dtReportingDeptOwners.Rows)
                {
                    strCC[i] = Convert.ToString(drReportingDeptOwners["EmailId"]);
                    i = i + 1;
                }
                foreach (DataRow drComplianceUsers in dtComplianceUsers.Rows)
                {
                    strCC[i] = Convert.ToString(drComplianceUsers["EM_EMAIL"]);
                    i = i + 1;
                }
                foreach (DataRow drTrackingDeptOwners in dtTrackingDeptOwners.Rows)
                {
                    strTo[j] = Convert.ToString(drTrackingDeptOwners["EM_EMAIL"]);
                    j = j + 1;
                }
                string strUserName = Page.User.Identity.Name.ToString();
                Authentication auth = new Authentication();
                string strDetails = auth.GetUserDetsByEmpCode(strUserName);
                strDetailsList = strDetails.Split('|');
                string strUser = Convert.ToString(strDetailsList[0]);

                string strSubTable = "<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>" +
                            "<th " + strTableHeaderCSS + ">Department</th>" +
                            "<th " + strTableHeaderCSS + ">Regulator/Authority</th>" +
                            "<th " + strTableHeaderCSS + ">Particulars</th>" +
                            "<th " + strTableHeaderCSS + ">Due date </th>" +
                            "<th " + strTableHeaderCSS + ">Compliance Status </th>" +
                            "<th " + strTableHeaderCSS + ">Remarks</th></tr>" +
                            "<tr><td " + strTableCellCSS + ">" + 1 + "</td>" +
                            "<td " + strTableCellCSS + ">" + strDepartment + "</td>" +
                            "<td " + strTableCellCSS + ">" + strSegment + "</td>" +
                            "<td " + strTableCellCSS + ">" + strParticulars + "</td>" +
                            "<td " + strTableCellCSS + ">" + strDuedate + "</td>" +
                            "<td " + strTableCellCSS + ">" + strComplianceStatus + "</td>" +
                            "<td " + strTableCellCSS + ">" + strRemarks.Replace(Environment.NewLine, "<br />") + "</td>" +
                            "</table>";

                strContent = strContent.Replace("%SubmittedBy%", strUser);
                strContent = strContent.Replace("%SubmissionTable%", strSubTable);
                strContent = strContent.Replace("%Footer%", ConfigurationManager.AppSettings["MailFooter"].ToString());
                mm.sendAsyncMail(strTo, strCC, null, strSubject, strContent);
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void gvChecklistDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                gvr = e.Row;
                System.Data.DataRowView drv = e.Row.DataItem as System.Data.DataRowView;

                if (drv != null)
                {
                    String strStatus = drv["SUB_STATUS"].ToString();
                    int intScId = Convert.ToInt32(drv["SC_ID"]);
                    //RadioButtonList rbl = (RadioButtonList)(gvr.FindControl("rblYesNoNA"));
                    DropDownList ddlYesNo = (DropDownList)(gvr.FindControl("ddlYesNoNA"));
                    F2FTextBox txt = (F2FTextBox)(gvr.FindControl("txtRemarks"));
                    LinkButton lbSave = (LinkButton)(gvr.FindControl("lbSave"));
                    LinkButton lbSubmit = (LinkButton)(gvr.FindControl("lbSubmit"));
                    HiddenField hfValidtionGroup = (HiddenField)(gvr.FindControl("hfValidtionGroup"));
                    Label lblDueDateTo = (Label)(gvr.FindControl("lblDueDateTo"));
                    RequiredFieldValidator rfvRBL = (RequiredFieldValidator)(gvr.FindControl("rfvYesNoNA"));
                    LinkButton lbAttach = (LinkButton)(gvr.FindControl("lbAttach"));
                    LinkButton lnkExtension = (LinkButton)e.Row.FindControl("lnkExtension");
                    lnkExtension.OnClientClick = "return onExtensionClick('" + intScId + "');";

                    string strDueDate = lblDueDateTo.Text;

                    if (drv["SUB_YES_NO_NA"].ToString() != null)
                    {
                        ddlYesNo.SelectedValue = drv["SUB_YES_NO_NA"].ToString();
                    }

                    if (strStatus == "S" || strStatus == "C")
                    {
                        txt.Enabled = false;
                        ddlYesNo.Enabled = false;
                        lbSave.Visible = false;
                        lbSubmit.Visible = false;
                        rfvRBL.Enabled = false;

                        lbAttach.Text = "View";
                        lbAttach.Attributes.Add("onclick", "javascript:return openSubmissionFilesWindow('" + Convert.ToString(intScId) + "','Submission','View')");
                    }
                    else
                    {
                        intCnt++;
                        rfvRBL.ValidationGroup = Convert.ToString(intCnt);
                        lbSave.ValidationGroup = Convert.ToString(intCnt);
                        lbSubmit.ValidationGroup = Convert.ToString(intCnt);
                        hfValidtionGroup.Value = Convert.ToString(intCnt);

                        txt.Enabled = false;
                        ddlYesNo.Enabled = false;
                        lbSave.Visible = false;
                        lbSubmit.Visible = false;


                        //lbAttach.Text = "Attach";
                        //lbAttach.Attributes.Add("onclick", "javascript:return openSubmissionFilesWindow('" + Convert.ToString(intScId) + "','Submission','Attach')");
                        lbAttach.Text = "View";
                        lbAttach.Attributes.Add("onclick", "javascript:return openSubmissionFilesWindow('" + Convert.ToString(intScId) + "','Submission','View')");
                    }

                    //Commented by Ankur Tyagi on 16-May-2025 for Project Id : 2395

                    //int iCPDays = 0;

                    //using (F2FDatabase DB = new F2FDatabase("SELECT CP_VALUE FROM TBL_CONFIG_PARAMS WHERE CP_NAME = 'Filings - Submission not allowed X days before due date'"))
                    //{
                    //    DB.OpenConnection();
                    //    object objTmp = DB.F2FCommand.ExecuteScalar();
                    //    iCPDays = Convert.ToInt32((objTmp == DBNull.Value) ? "20" : objTmp.ToString());
                    //}

                    //int DateDiff = Convert.ToInt32(DataServer.ExecuteScalar("SELECT DATEDIFF(Day, current_timestamp, '" + strDueDate + "') AS DateDiff"));

                    //Date diff not to be checked
                    //if (DateDiff > iCPDays || strStatus == "S" || strStatus == "C")

                    //>>

                    if (strStatus == "S" || strStatus == "C")
                    {
                        lbSubmit.Visible = false;
                    }
                    else
                    {
                        lbSubmit.Visible = true;
                    }
                    //>>

                    if (strStatus != "C")
                    {
                        lnkExtension.Visible = true;
                    }
                    else
                    {
                        lnkExtension.Visible = false;
                    }

                    if (hfType.Value.Equals("1"))
                    {

                    }
                    //lblTitle.Text = " My Search Submissions Checklist";
                    else if (hfType.Value.Equals("2"))
                    {

                    }
                    //lblTitle.Text = "My Search Department Checklist";
                    else if (hfType.Value.Equals("3"))
                    {
                    }
                    //lblTitle.Text = "My Search Submissions Checklist";
                    else if (hfType.Value.Equals("4"))
                    {
                        lbSubmit.Visible = false;
                        lbSave.Visible = false;
                        lnkExtension.Visible = false;
                    }
                    //lblTitle.Text = "My Search Department Checklist";

                }
            }
        }

        //Added By Urvashi Gupta on 28Apr2016 for Submission date
        public void cvSubDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this))
            {
                hfDoubleClickFlag.Value = "";
                return;
            }
            try
            {
                ListItem liChkBoxListItem;
                DataSet dsSearchChecklist = new DataSet();
                string strFixorEvent = ddlType.SelectedValue.ToString();
                string strFilterExpression = String.Empty;
                string strEvent = ddlEvent.SelectedValue;
                string strSegment = ddlSegment.SelectedValue;
                string strMonth = ddlMonth.SelectedValue;
                string strFromdate = txtFromdate.Text;
                string strTodate = txtTodate.Text;
                string strParticular = txtParticulars.Text;
                string strDesc = txtDescription.Text;
                string strReportingFun = ddlReportDept.SelectedValue;
                string strStatus = "";//ddlStatus.SelectedValue.ToString();
                string strFilterEvent = "", strEventAgenda = "";
                string strOrderBy = "";
                bool blnEventAgendaSelected = false;

                for (int i = 0; i <= chkStatus.Items.Count - 1; i++)
                {
                    liChkBoxListItem = chkStatus.Items[i];
                    if (liChkBoxListItem.Selected)
                    {
                        strStatus = strStatus + "'" + liChkBoxListItem.Value + "',";
                    }
                }
                if (!strStatus.Equals(""))
                {
                    strStatus = strStatus.Substring(0, strStatus.Length - 1);
                }
                writeError("");

                //<< Added By Vivek on 03-Jul-2017
                if (!ddlFinYear.SelectedValue.Equals(""))
                {
                    strFilterExpression = strFilterExpression + " And FYM_ID = '" + ddlFinYear.SelectedValue + "'";
                }
                //>>

                if (strFixorEvent != "")
                {
                    strFilterExpression = strFilterExpression + " And SM_SUB_TYPE = '" + strFixorEvent + "'";
                }
                if (strStatus != "")
                {
                    strFilterExpression = strFilterExpression + " And isnull(SUB_STATUS,'P') in (" + strStatus + ") ";
                }
                if (strEvent != "")
                {
                    strFilterExpression = strFilterExpression + " And SM_EM_ID = " + strEvent;
                }
                if (strSegment != "")
                {
                    strFilterExpression = strFilterExpression + " And SSM_ID = " + strSegment;
                }
                if (strMonth != "")
                {
                    strFilterExpression = strFilterExpression + " And (MONTH(SC_DUE_DATE_TO) = " + strMonth + ")";
                }

                if (strFromdate != "")
                {
                    strFilterExpression = strFilterExpression + " And SC_DUE_DATE_TO >= '" + strFromdate + " '";
                }
                if (strTodate != "")
                {
                    strFilterExpression = strFilterExpression + " And SC_DUE_DATE_TO <= '" + strTodate + "'";
                }

                if (strParticular != "")
                {
                    strFilterExpression = strFilterExpression + " And SC_PARTICULARS like '%" + strParticular + "%'";
                }
                if (strDesc != "")
                {
                    strFilterExpression = strFilterExpression + " And SC_DESCRIPTION like '%" + strDesc + "%'";
                }
                if (strReportingFun != "")
                {
                    strFilterExpression = strFilterExpression + " And SM_SRD_ID = " + strReportingFun + "";
                }
                //<< Modified By Vivek on 03-Jul-2017
                //for (int i = 0; i < cblAssociatedWith.Items.Count; i++)
                //{
                //    ListItem li = cblAssociatedWith.Items[i];
                //    if (li.Selected)
                //    {
                //        blnEventAgendaSelected = true;
                //        if (strFilterEvent == "")
                //            strFilterEvent = " and (" + strFilterEvent + " SM_EP_ID = " + li.Value;
                //        else
                //            strFilterEvent = strFilterEvent + " OR SM_EP_ID = " + li.Value;
                //    }
                //}

                for (int i = 0; i < cblAssociatedWith.Items.Count; i++)
                {
                    ListItem li = cblAssociatedWith.Items[i];
                    if (li.Selected)
                    {
                        blnEventAgendaSelected = true;
                        if (strEventAgenda == "")
                            strEventAgenda = li.Value;
                        else
                            strEventAgenda = strEventAgenda + ", " + li.Value;
                    }
                }

                //>>

                if (blnEventAgendaSelected)
                {
                    strFilterExpression = strFilterExpression + " AND SM_EP_ID In (" + strEventAgenda + ") ";
                    //strFilterEvent = strFilterEvent + ")";
                    strOrderBy = " ORDER BY SM_EP_ID,SM_SORT_ORDER,SC_DUE_DATE_TO";
                }
                else
                {
                    strOrderBy = " ORDER BY SC_STM_ID, SC_DUE_DATE_TO";
                }

                //<<Modified by Ankur Tyagi on 18-Apr-2025 for Project Id : 2395
                //Added Global Search
                dsSearchChecklist = SubmissionMasterBLL.SearchComplianceChecklist(strFilterExpression,
                    Authentication.GetUserID(Page.User.Identity.Name), hfType.Value.ToString(), strOrderBy, txtGlobalSearch.Text, mstrConnectionString);
                //>>

                //if (ddlType.SelectedValue == "F")
                //{
                //    dsSearchChecklist = SubmissionMasterBLL.SearchSubmissionChecklistForFix(Authentication.GetUserID(Session["UserId"].ToString()), strFilterExpression, strOrderBy, mstrConnectionString);

                //}
                //else if (ddlType.SelectedValue == "E")
                //{
                //    dsSearchChecklist = SubmissionMasterBLL.SearchSubmissionChecklistForEventBased(Authentication.GetUserID(Session["UserId"].ToString()), strFilterExpression, strFilterEvent, strOrderBy, mstrConnectionString);
                //}

                gvChecklistDetails.DataSource = dsSearchChecklist;
                Session["strFilterExpression"] = strFilterExpression;
                Session["strFilterEvent"] = strFilterEvent;
                Session["strOrderBy"] = strOrderBy;
                Session["LoadSearchedChecklist"] = dsSearchChecklist;
                gvChecklistDetails.DataBind();
                hideShowButtons(dsSearchChecklist.Tables[0]);
                if ((this.gvChecklistDetails.Rows.Count == 0))
                {
                    this.lblInfo.Text = "No checklists found satisfying the criteria.";
                    this.lblInfo.Visible = true;
                    btnExportToExcel.Visible = false;
                }
                else
                {
                    this.lblInfo.Text = String.Empty;
                    this.lblInfo.Visible = false;
                    btnExportToExcel.Visible = true;
                }
                if (hfType.Value.Equals("4"))
                {
                    //gvChecklistDetails.Columns[25].Visible = false;
                    //gvChecklistDetails.Columns[26].Visible = false;
                    //gvChecklistDetails.Columns[27].Visible = false;
                }
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }
        protected void lnkReset_Click(object sender, EventArgs e)
        {
            Session["strFilterExpression"] = null;
            Session["strFilterEvent"] = null;
            Session["strOrderBy"] = null;
            Session["LoadSearchedChecklist"] = null;
            Response.Redirect(Request.RawUrl, false);
        }
        protected void ddlEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ddlEvent.SelectedValue.Equals(""))
            {
                cblAssociatedWith.Items.Clear();
                cblAssociatedWith.DataSource = utilitiesBL.getDatasetWithCondition("EVENTPURPOSE", Convert.ToInt32(ddlEvent.SelectedValue), mstrConnectionString);
                cblAssociatedWith.DataBind();
            }
            else
            {
                cblAssociatedWith.Items.Clear();
                cblAssociatedWith.DataBind();
            }
        }
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                int uniqueRowId = 0;
                string strChecklistTable = "", strHtmlTableChecklistDetsRows = "", strSubmittedOn = "",
                    strSumittedOn = "", strClosedOn = "", strReopenedOn = "", strSubmittedAuthOn = "";
                string strDueDateFrom = "", strDueDateTo = "", strEventDate = "", strSC_EXTENSION_DONE_ON = "";
                DataTable dtChecklistDets;
                DataRow drChecklistDets;
                DateTime dtSubmittedOn, dtDueDateFrom, dtDueDateTo;

                DataSet dtChecklistDetsSets = (DataSet)Session["LoadSearchedChecklist"];
                dtChecklistDets = (DataTable)dtChecklistDetsSets.Tables[0];
                string strHtmlTable =
                    "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                    "<HTML>" +
                    "<HEAD>" +
                    "</HEAD>" +
                    "<BODY>" +

                    " <table id='tblChecklistDets' width='100%' align='left' style='margin-left:20px;' " +
                                " cellpadding='0' cellspacing='1' border='1'> " +
                              " <thead> " +
                              " <tr> " +
                              " <th class='tabhead' align='center'> " +
                              " Sr No. " +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Department " +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Reporting Function " +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Authority " +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Event " +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Agenda " +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Event date " +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Particulars " +
                              " </th> " +
                                " <th class='tabhead' align='center'> " +
                              " Description " +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Frequency" +
                              " </th> " +
                               " <th class='tabhead' align='center'> " +
                              " Internal due date " +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Regulatory due date" +
                              " </th> " +
                              //<<Added by Rahuldeb on 24Sep2019
                              " <th class='tabhead' align='center'> " +
                              " Submitted By" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Submitted On" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Closed By" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Closed On" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Reopened By" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Reopened On" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Reopened Due To" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Reopened Remarks" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Is Extended" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Extension done by" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Extension done on" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Extension remarks" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Submitted to Authority On / Date of Receipt of Data" +
                              " </th> " +
                              //>>
                              " <th class='tabhead' align='center'> " +
                              " Yes/No/NA" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Submission Date" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Remarks" +
                              " </th> " +
                              " <th class='tabhead' align='center'> " +
                              " Attach Files" +
                              " </th> " +
                              " </tr> " +
                              " </thead> ";


                int intChecklistDetsCnt = dtChecklistDets.Rows.Count;
                for (int intCnt = 0; intCnt < intChecklistDetsCnt; intCnt++)
                {
                    uniqueRowId = uniqueRowId + 1;
                    drChecklistDets = dtChecklistDets.Rows[intCnt];
                    strSubmittedOn = "";
                    strEventDate = "";
                    if (DateTime.TryParse(drChecklistDets["SUB_SUBMIT_DATE"].ToString(), out dtSubmittedOn))
                    {
                        strSubmittedOn = dtSubmittedOn.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        strSubmittedOn = "";
                    }

                    if (DateTime.TryParse(drChecklistDets["SC_DUE_DATE_FROM"].ToString(), out dtDueDateFrom))
                    {
                        strDueDateFrom = dtDueDateFrom.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        strDueDateFrom = "";
                    }

                    if (DateTime.TryParse(drChecklistDets["SC_DUE_DATE_TO"].ToString(), out dtDueDateTo))
                    {
                        strDueDateTo = dtDueDateTo.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        strDueDateTo = "";
                    }

                    if (!(drChecklistDets["EI_EVENT_DATE"].Equals(DBNull.Value) || drChecklistDets["EI_EVENT_DATE"].ToString().Equals("")))
                    {
                        strEventDate = Convert.ToDateTime(drChecklistDets["EI_EVENT_DATE"]).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        strEventDate = "";
                    }

                    //<<Added by Rahuldeb on 24Sep2019 strSumittedOn, strClosedOn, strReopenedOn, strSubmittedAuthOn

                    if (!(drChecklistDets["SUB_SUBMIT_DATE"].Equals(DBNull.Value) || drChecklistDets["SUB_SUBMIT_DATE"].ToString().Equals("")))
                    {
                        strSumittedOn = Convert.ToDateTime(drChecklistDets["SUB_SUBMIT_DATE"]).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        strSumittedOn = "";
                    }

                    if (!(drChecklistDets["SUB_CLOSED_ON"].Equals(DBNull.Value) || drChecklistDets["SUB_CLOSED_ON"].ToString().Equals("")))
                    {
                        strClosedOn = Convert.ToDateTime(drChecklistDets["SUB_CLOSED_ON"]).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        strClosedOn = "";
                    }

                    if (!(drChecklistDets["SUB_REOPENED_ON"].Equals(DBNull.Value) || drChecklistDets["SUB_REOPENED_ON"].ToString().Equals("")))
                    {
                        strReopenedOn = Convert.ToDateTime(drChecklistDets["SUB_REOPENED_ON"]).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        strReopenedOn = "";
                    }

                    if (!(drChecklistDets["SUB_SUBMITTED_TO_AUTHORITY_ON"].Equals(DBNull.Value) || drChecklistDets["SUB_SUBMITTED_TO_AUTHORITY_ON"].ToString().Equals("")))
                    {
                        strSubmittedAuthOn = Convert.ToDateTime(drChecklistDets["SUB_SUBMITTED_TO_AUTHORITY_ON"]).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        strSubmittedAuthOn = "";
                    }
                    //>>

                    if (!(drChecklistDets["SC_EXTENSION_DONE_ON"].Equals(DBNull.Value) || drChecklistDets["SC_EXTENSION_DONE_ON"].ToString().Equals("")))
                    {
                        strSC_EXTENSION_DONE_ON = Convert.ToDateTime(drChecklistDets["SC_EXTENSION_DONE_ON"]).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        strSC_EXTENSION_DONE_ON = "";
                    }

                    strHtmlTableChecklistDetsRows = strHtmlTableChecklistDetsRows + "<tr>" +
                    "<td>" + uniqueRowId + "</td>" +
                    "<td>" + drChecklistDets["STM_TYPE"].ToString() + "</td>" +
                    "<td>" + drChecklistDets["SRD_NAME"].ToString() + "</td>" +
                    "<td>" + drChecklistDets["SSM_NAME"].ToString() + "</td>" +
                    "<td>" + drChecklistDets["EM_EVENT_NAME"].ToString() + "</td>" +
                    "<td>" + drChecklistDets["EP_NAME"].ToString() + "</td>" +
                    "<td>" + strEventDate + "</td>" +
                    "<td>" + drChecklistDets["SC_PARTICULARS"].ToString() + "</td>" +
                    "<td>" + drChecklistDets["SC_DESCRIPTION"].ToString() + "</td>" +
                    "<td>" + drChecklistDets["SC_FREQUENCY"].ToString() + "</td>" +
                    "<td>" + strDueDateFrom + "</td>" +
                    "<td>" + strDueDateTo + "</td>" +
                    //<<Added by Rahuldeb on 24Sep2019
                    "<td>" + drChecklistDets["SUB_SUBMITTED_BY"].ToString() + "</td>" +
                    "<td>" + strSumittedOn + "</td>" +
                    "<td>" + drChecklistDets["SUB_CLOSED_BY"].ToString() + "</td>" +
                    "<td>" + strClosedOn + "</td>" +
                    "<td>" + drChecklistDets["SUB_REOPENED_BY"].ToString() + "</td>" +
                    "<td>" + strReopenedOn + "</td>" +

                    "<td>" + drChecklistDets["ReOpenPurpose"].ToString() + "</td>" +
                    "<td>" + drChecklistDets["SUB_REOPEN_COMMENTS"].ToString() + "</td>" +
                    "<td>" + drChecklistDets["SC_IS_EXTENDED"].ToString() + "</td>" +
                    "<td>" + drChecklistDets["SC_EXTENSION_DONE_BY"].ToString() + "</td>" +
                    "<td>" + strSC_EXTENSION_DONE_ON + "</td>" +
                    "<td>" + drChecklistDets["SC_EXTENSION_REMARKS"].ToString() + "</td>" +

                    "<td>" + strSubmittedAuthOn + "</td>" +
                    //>>
                    "<td>" + drChecklistDets["SUB_YES_NO_NA"].ToString() + "</td>" +
                    "<td>" + strSubmittedOn + "</td>" +
                    "<td>" + drChecklistDets["SUB_REMARKS"].ToString() + "</td>" +
                    "<td>" + drChecklistDets["SUB_CLIENT_FILE_NAME"].ToString() + "</td>" +
                    "</tr>";
                }
                strChecklistTable = strHtmlTable + strHtmlTableChecklistDetsRows + "</table> " +
                "</BODY>" +
                "</HTML>";
                string attachment = "attachment; filename=Reports.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";

                Response.Write(strChecklistTable.ToString());
                Response.End();

            }
            catch (Exception exp)
            {
                writeError("Exception in btnSubmitExportToExcel_Click :" + exp);
            }
        }

        protected void gvChecklistDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvChecklistDetails.PageIndex = e.NewPageIndex;
            gvChecklistDetails.DataSource = Session["LoadSearchedChecklist"];
            gvChecklistDetails.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        protected void cvTodate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
        #region
        protected void gvChecklistDetails_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (Session["LoadSearchedChecklist"] != null)
            {
                DataTable dt = ((DataSet)(Session["LoadSearchedChecklist"])).Tables[0];
                DataView dvDataView = new DataView(dt);
                string strSortExpression = "";

                if (ViewState["_SortExpression_"] != null)
                {
                    strSortExpression = ViewState["_SortExpression_"].ToString();
                }

                if (ViewState["_SortDirection_"] == null)
                {
                    dvDataView.Sort = (e.SortExpression + (" " + "ASC"));
                    ViewState["_SortDirection_"] = "ASC";
                }
                else
                {
                    //ONLY IF THE USER HAS CLICKED ON THE SAME COLUMN AGAIN, SHOULD IT BE SORTED IN THE REVERSE ORDER.
                    //IF ANOTHER COLUMN IS SELECTED, IT SHOULD AGAIN BE SORTED IN ASCENDING ORDER. 
                    if (strSortExpression.Equals(e.SortExpression))
                    {
                        if (ViewState["_SortDirection_"].ToString().Equals("ASC"))
                        {
                            dvDataView.Sort = (e.SortExpression + (" " + "DESC"));
                            ViewState["_SortDirection_"] = "DESC";
                        }
                        else if (ViewState["_SortDirection_"].ToString().Equals("DESC"))
                        {
                            dvDataView.Sort = (e.SortExpression + (" " + "ASC"));
                            ViewState["_SortDirection_"] = "ASC";
                        }
                    }
                    else
                    {
                        dvDataView.Sort = (e.SortExpression + (" " + "ASC"));
                        ViewState["_SortDirection_"] = "ASC";
                    }
                }
                ViewState["_SortExpression_"] = e.SortExpression;
                gvChecklistDetails.DataSource = dvDataView;
                gvChecklistDetails.DataBind();
                hideShowButtons(((DataSet)(Session["LoadSearchedChecklist"])).Tables[0]);
            }
        }

        protected void OnRowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex = GetSortColumnIndex();

                if (sortColumnIndex != -1)
                {
                    if (ViewState["_SortDirection_"] != null)
                        AddSortImage(e.Row, ViewState["_SortDirection_"].ToString(), sortColumnIndex);
                }
            }
        }

        private int GetSortColumnIndex()
        {
            // Iterate through the Columns collection to determine the index
            // of the column being sorted.

            string strSortExpression = "";

            if (ViewState["_SortExpression_"] != null)
                strSortExpression = ViewState["_SortExpression_"].ToString();

            if (!strSortExpression.Equals(""))
            {
                foreach (DataControlField field in gvChecklistDetails.Columns)
                {
                    if (field.SortExpression.Equals(strSortExpression))
                    {
                        return gvChecklistDetails.Columns.IndexOf(field);
                    }
                }
            }
            return -1;
        }

        void AddSortImage(GridViewRow headerRow, string strAction, int sortColumnIndex)
        {
            // Create the sorting image based on the sort direction.
            System.Web.UI.WebControls.Image sortImage = new System.Web.UI.WebControls.Image();
            if (strAction.Equals("ASC"))
            {
                sortImage.ImageUrl = "../../Content/images/legacy/view_sort_ascending.png";
                sortImage.AlternateText = "Ascending Order";
            }
            else if (strAction.Equals("DESC"))
            {
                sortImage.ImageUrl = "../../Content/images/legacy/view_sort_descending.png";
                sortImage.AlternateText = "Descending Order";
            }
            headerRow.Cells[sortColumnIndex].Controls.Add(sortImage);
        }
        #endregion

        protected void lbExtension_Click(object sender, EventArgs e)
        {
            //<<Added by Ankur Tyagi on 05-May-2025 for Project Id : 2395
            //DataTable dtSubFiles = utilitiesBL.getDatasetWithConditionInString("SUBMISSIONSFILESFromType",
            //        " and SF_FILE_TYPE = 'OT' and SF_OPERATION_TYPE = 'Extension' AND SF_SC_ID = " + ParseStringToInt(hfSelectedRecord.Value) + " ", mstrConnectionString);
            //if (dtSubFiles.Rows.Count <= 0)
            //{
            //    hfDoubleClickFlag.Value = "";
            //    writeError("Attachment is compulsory for reopen.");
            //    return;
            //}
            //>>

            int updatedRows = SubmissionMasterBLL.updateExtensionDate(ParseStringToInt(hfSelectedRecord.Value), txtExRegulatoryDate.Text,
                txtExRemarks.Text, Getfullname(Page.User.Identity.Name), mstrConnectionString);
            if (updatedRows > 0)
            {
                writeError("Regulatory date extented successfully.");
            }
            txtExRegulatoryDate.Text = string.Empty;
            txtExRemarks.Text = string.Empty;
            hfSelectedRecord.Value = string.Empty;
            hfSelectedOperation.Value = string.Empty;
            gvChecklistDetails.DataSource = (DataSet)Session["LoadSearchedChecklist"];
            gvChecklistDetails.DataBind();
        }

        int ParseStringToInt(string input)
        {
            int result;
            if (int.TryParse(input, out result))
            {
                return result;
            }
            return 0;
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

    }
}