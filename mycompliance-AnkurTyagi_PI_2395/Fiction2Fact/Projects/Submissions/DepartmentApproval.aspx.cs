using System;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Fiction2Fact.Controls;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.App_Code;
using Fiction2Fact.F2FControls;
using System.Linq;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class Submissions_DepartmentApproval : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
        UtilitiesBLL utilitiesBL = new UtilitiesBLL();
        UtilitiesBLL utilBLL = new UtilitiesBLL();
        UtilitiesVO utliVo = new UtilitiesVO();

        string[] strDetailsList;
        private int intCnt = 0;



        string strTableCellCSS = " style=\"font-size: 12px; font-family: Calibri; border-width: " +
                        "1px; padding: 8px; border-style: solid;" +
                        "border-color: #bcaf91;\"";

        string strTableHeaderCSS = " style =\"font-size: 12px; font-family: Calibri; " +
                                "background-color: #ded0b0; border-width: 1px; padding: 8px;" +
                                "border-style: solid; border-color: #bcaf91; text-align: left;\"";

        string strTableCSS = " style=\"font-size: 12px; font-family: Calibri; color: #333333; " +
                                "width: 100%; border-width: 1px; border-color: #bcaf91; border-collapse: collapse;\"";

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = null;
            string strMonthSelected = "";
            if (Page.IsPostBack)
            {
                if (Session["DeptChklistSelectCommand"] != null)
                {
                    gvChecklistDetails.DataSource = (DataSet)(Session["DeptChklistSelectCommand"]);
                }
            }

            else
            {
                if (Page.User.Identity.Name.ToString().Equals(""))
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                else
                {
                    hfCurDate.Value = System.DateTime.Now.ToString("dd-MMM-yyyy"); //Added By Urvashi Gupta On 28Apr2016
                    if (Session["MonthSelected"] != null)
                    {
                        str = (string)Session["MonthSelected"];
                        hfMonth.Value = str;
                    }
                    //if (Request.QueryString["Type"] != null)
                    //{
                    //    hfType.Value = Request.QueryString["Type"].ToString();
                    //    //<<Added by Ankur Tyagi on 18-Apr-2025 for Project Id : 2395
                    //    if (hfType.Value == "4")
                    //    {
                    divReportingDept.Visible = false;
                    //    }
                    //    //>>
                    //}

                    //Added By Milan Yadav on 28Apr2016
                    //>>
                    ListItem li = new ListItem();
                    li.Text = "(Select)";
                    li.Value = "";

                    //<< Modified By Vivek on 16-Jan-2018
                    DataTable dtFinYear = new DataTable();

                    dtFinYear = utilitiesBL.getDataset("AllFinYears", mstrConnectionString).Tables[0];
                    ddlFinYear.DataSource = dtFinYear;
                    ddlFinYear.DataBind();
                    ddlFinYear.Items.Insert(0, new ListItem("(Select an option)", ""));

                    for (int i = 0; i < dtFinYear.Rows.Count; i++)
                    {
                        if (dtFinYear.Rows[i]["FYM_STATUS"].ToString().Equals("A"))
                        {
                            ddlFinYear.SelectedValue = dtFinYear.Rows[i]["FYM_ID"].ToString();
                        }
                    }
                    //<<

                    ddlSegment.DataSource = utilitiesBL.getDataset("SUBMISSIONSGMT", mstrConnectionString);
                    ddlSegment.DataBind();

                    ddlReportDept.DataSource = utilitiesBL.getDataset("REPORTINGDEPT", mstrConnectionString);
                    ddlReportDept.DataBind();
                    ddlReportDept.Items.Insert(0, new ListItem("(Select an option)", ""));

                    //if (hfType.Value.Equals("2"))
                    //    lbSelectedMonth.Text = "List of Submissions";
                    //else if (hfType.Value.Equals("4"))
                    //    lbSelectedMonth.Text = "My Submissions";
                    //else if (hfType.Value.Equals("4M"))
                    //{
                    //    hfType.Value = "4";
                    //    strMonthSelected = Request.QueryString["Month"].ToString();

                    //    showListForMonth(Convert.ToInt32(strMonthSelected));
                    //}
                    //else
                    lbSelectedMonth.Text = "Department Approval";
                }
            }
        }

        private void setSubmissionQuery()
        {
            writeError("");
            DataSet dsChecklist = new DataSet();
            ListItem liChkBoxListItem;
            string strStatus = "";
            int intReportingDeptId = 0;
            string strFinYear = "0";
            try
            {
                if (!ddlFinYear.SelectedValue.Equals(""))
                {
                    strFinYear = ddlFinYear.SelectedValue.ToString();
                }
                if (!ddlReportDept.SelectedValue.Equals(""))
                {
                    intReportingDeptId = Convert.ToInt32(ddlReportDept.SelectedValue);
                }
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
                //strStatus = ddlStatus.SelectedValue.ToString();
                //<<Modified by Ankur Tyagi on 21-Apr-2025 for Project Id : 2395
                //Added Global Search
                string strGlobalSearch = "", strAuthority = "", strFrequency = "";

                strAuthority = ddlSegment.SelectedItem.Text;
                strFrequency = ddlFrequency.SelectedValue;
                strGlobalSearch = txtGlobalSearch.Text;

                dsChecklist = SubmissionMasterBLL.LoadComplianceChecklistForApproval(Session["MonthSelected"].ToString(), Authentication.GetUserID(Page.User.Identity.Name),
                    Convert.ToInt32(strFinYear), intReportingDeptId, strStatus, strGlobalSearch, strAuthority, strFrequency, mstrConnectionString);
                //>>
                //<<
                Session["DeptChklistSelectCommand"] = dsChecklist;
                gvChecklistDetails.DataSource = dsChecklist;
                gvChecklistDetails.DataBind();
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }

        protected void lbtnJan_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "1";
            Session["MonthSelected"] = "1";
            setSubmissionQuery();
            lbSelectedMonth.Text = "Department Approval for January";
        }
        protected void lbtnFeb_Click(object sender, EventArgs e)
        {
            lbSelectedMonth.Text = "";
            hfMonth.Value = "2";
            Session["MonthSelected"] = "2";
            setSubmissionQuery();
            lbSelectedMonth.Text = "Department Approval for February";

        }
        protected void lbtnMarch_Click(object sender, EventArgs e)
        {
            lbSelectedMonth.Text = "";
            hfMonth.Value = "3";
            Session["MonthSelected"] = "3";
            setSubmissionQuery();
            lbSelectedMonth.Text = "Department Approval for March";
        }
        protected void lbtnApr_Click(object sender, EventArgs e)
        {
            lbSelectedMonth.Text = "";
            hfMonth.Value = "4";
            Session["MonthSelected"] = "4";
            setSubmissionQuery();
            lbSelectedMonth.Text = "Department Approval for April";
        }
        protected void lbtnMay_Click(object sender, EventArgs e)
        {
            lbSelectedMonth.Text = "";
            hfMonth.Value = "5";
            Session["MonthSelected"] = "5";
            setSubmissionQuery();
            lbSelectedMonth.Text = "Department Approval for May";
        }
        protected void lbtnJune_Click(object sender, EventArgs e)
        {
            lbSelectedMonth.Text = "";
            hfMonth.Value = "6";
            Session["MonthSelected"] = "6";
            setSubmissionQuery();
            lbSelectedMonth.Text = "Department Approval for June";
        }
        protected void lbtnJuly_Click(object sender, EventArgs e)
        {
            lbSelectedMonth.Text = "";
            hfMonth.Value = "7";
            Session["MonthSelected"] = "7";
            setSubmissionQuery();
            lbSelectedMonth.Text = "Department Approval for July";
        }
        protected void lbtnAug_Click(object sender, EventArgs e)
        {
            lbSelectedMonth.Text = "";
            hfMonth.Value = "8";
            Session["MonthSelected"] = "8";
            setSubmissionQuery();
            lbSelectedMonth.Text = "Department Approval for August";
        }
        protected void lbtnSep_Click(object sender, EventArgs e)
        {
            lbSelectedMonth.Text = "";
            hfMonth.Value = "9";
            Session["MonthSelected"] = "9";
            setSubmissionQuery();
            lbSelectedMonth.Text = "Department Approval for September";
        }
        protected void lbtnOct_Click(object sender, EventArgs e)
        {
            lbSelectedMonth.Text = "";
            hfMonth.Value = "10";
            Session["MonthSelected"] = "10";
            setSubmissionQuery();
            lbSelectedMonth.Text = "Department Approval for October";
        }
        protected void lbtnNov_Click(object sender, EventArgs e)
        {
            lbSelectedMonth.Text = "";
            hfMonth.Value = "11";
            Session["MonthSelected"] = "11";
            setSubmissionQuery();
            lbSelectedMonth.Text = "Department Approval for November";
        }
        protected void lbtnDec_Click(object sender, EventArgs e)
        {
            lbSelectedMonth.Text = "";
            hfMonth.Value = "12";
            Session["MonthSelected"] = "12";
            setSubmissionQuery();
            lbSelectedMonth.Text = "Department Approval for December";
        }

        private void showListForMonth(int intMonth)
        {
            string strMonthName = "";

            if (intMonth.Equals(4))
                strMonthName = "April";
            else if (intMonth.Equals(5))
                strMonthName = "May";
            else if (intMonth.Equals(6))
                strMonthName = "June";
            else if (intMonth.Equals(7))
                strMonthName = "July";
            else if (intMonth.Equals(8))
                strMonthName = "August";
            else if (intMonth.Equals(9))
                strMonthName = "September";
            else if (intMonth.Equals(10))
                strMonthName = "October";
            else if (intMonth.Equals(11))
                strMonthName = "November";
            else if (intMonth.Equals(12))
                strMonthName = "December";
            else if (intMonth.Equals(1))
                strMonthName = "January";
            else if (intMonth.Equals(2))
                strMonthName = "February";
            else if (intMonth.Equals(3))
                strMonthName = "March";


            lbSelectedMonth.Text = "";
            hfMonth.Value = intMonth.ToString();
            Session["MonthSelected"] = intMonth.ToString();
            setSubmissionQuery();
            lbSelectedMonth.Text = "Department Approval for " + strMonthName;
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
                    DropDownList ddlYesNo = (DropDownList)(gvr.FindControl("ddlYesNoNA"));
                    F2FTextBox txt = (F2FTextBox)(gvr.FindControl("txtRemarks"));
                    LinkButton lb1 = (LinkButton)(gvr.FindControl("lbAttach"));
                    LinkButton lbSubmit = (LinkButton)(gvr.FindControl("lbSubmit"));
                    Label lblDueDateTo = (Label)(gvr.FindControl("lblDueDateTo"));
                    HiddenField hfValidationGroup = (HiddenField)(gvr.FindControl("hfValidationGroup"));
                    HiddenField ClientFileName = (HiddenField)(gvr.FindControl("ClientFileName"));
                    HiddenField ServerFileName = (HiddenField)(gvr.FindControl("ServerFileName"));
                    ClientFileName.Value = drv["SUB_CLIENT_FILE_NAME"].ToString();
                    ServerFileName.Value = drv["SUB_SERVER_FILE_NAME"].ToString();
                    RequiredFieldValidator rfvRBL = (RequiredFieldValidator)(gvr.FindControl("rfvYesNoNA"));
                    HiddenField hfCircularId = (HiddenField)(gvr.FindControl("hfCircularId"));
                    LinkButton lnkViewCirc = (LinkButton)(gvr.FindControl("lnkViewCirc"));
                    LinkButton lbAttach = (LinkButton)(gvr.FindControl("lbAttach"));

                    if (hfCircularId.Value.Equals("") || hfCircularId.Value.Equals("0"))
                    {
                        lnkViewCirc.Visible = false;
                    }
                    else
                    {
                        lnkViewCirc.Visible = true;
                    }

                    lnkViewCirc.OnClientClick = "onClientViewCircClick('" + (new SHA256EncryptionDecryption()).Encrypt(hfCircularId.Value) + "');";

                    if (drv["SUB_YES_NO_NA"].ToString() != null)
                    {
                        ddlYesNo.SelectedValue = drv["SUB_YES_NO_NA"].ToString();
                    }
                    if (strStatus == "S" || strStatus == "C")
                    {
                        //txt.Enabled = false;
                        //ddlYesNo.Enabled = false;
                        //lbSubmit.Visible = false;

                        //lbAttach.Text = "View";
                        //lbAttach.Attributes.Add("onclick", "javascript:return openSubmissionFilesWindow('" + Convert.ToString(intScId) + "','Submission','View')");
                    }
                    else
                    {
                        intCnt++;
                        rfvRBL.ValidationGroup = Convert.ToString(intCnt);
                        lbSubmit.ValidationGroup = Convert.ToString(intCnt);
                        hfValidationGroup.Value = Convert.ToString(intCnt);

                        //txt.Enabled = false;
                        //ddlYesNo.Enabled = false;
                        //lbSubmit.Visible = false;

                        string lblStatus = Convert.ToString(((Label)(gvr.FindControl("lblStatus"))).Text);
                        lbAttach.Text = "Attach";
                        if (lblStatus != "Reopened")
                        {
                            lbAttach.Attributes.Add("onclick", "javascript:return openSubmissionFilesWindow('" + Convert.ToString(intScId) + "','Submission','Attach')");
                        }
                        else
                        {
                            lbAttach.Attributes.Add("onclick", "javascript:return openSubmissionFilesWindow('" + Convert.ToString(intScId) + "','Resubmission','Attach')");
                        }

                        //lbAttach.Text = "View";
                        //lbAttach.Attributes.Add("onclick", "javascript:return openSubmissionFilesWindow('" + Convert.ToString(intScId) + "','Submission','View')");
                    }

                    if (strStatus == "S" || strStatus == "C")
                    {
                        lbSubmit.Visible = false;
                    }
                    else
                    {
                        lbSubmit.Visible = true;
                    }

                }
            }
        }

        protected void gvChecklistDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dsChecklist = new DataSet();
            if (!CommonCodes.CheckInputValidity(this))
            {
                hfDoubleClickFlag.Value = "";
                return;
            }
            try
            {
                int intSubmissions;
                ListItem liChkBoxListItem;
                GridViewRow gvr = gvChecklistDetails.SelectedRow;
                string strSubmissionId = ((Label)(gvr.FindControl("lblSubId"))).Text;
                int intSCId = Convert.ToInt32(gvChecklistDetails.SelectedValue);
                DropDownList ddlYesNoNA = ((DropDownList)(gvr.FindControl("ddlYesNoNA")));
                string strRemarks = Convert.ToString(((F2FTextBox)(gvr.FindControl("txtRemarks"))).Text);
                string strUser = Page.User.Identity.Name;
                string strClientFileName = ((HiddenField)(gvr.FindControl("ClientFileName"))).Value.ToString();
                string strServerFileName = ((HiddenField)(gvr.FindControl("ServerFileName"))).Value.ToString();
                int intReportingDeptId = 0;
                string strStatus = "";

                //<<Added by Ankur Tyagi on 05-May-2025 for Project Id : 2395
                DataTable dtSubFiles = new DataTable();
                string lblStatus = Convert.ToString(((Label)(gvr.FindControl("lblStatus"))).Text);
                string lblIsFSAppReq = Convert.ToString(((Label)(gvr.FindControl("lblIsFSAppReq"))).Text);
                if (lblStatus != "Reopened")
                {
                    dtSubFiles = utilitiesBL.getDatasetWithConditionInString("SUBMISSIONSFILESFromType",
                       " and SF_FILE_TYPE IN ('RD','HO','FS') AND SF_SC_ID = " + intSCId + " ", mstrConnectionString);

                    var types = dtSubFiles.AsEnumerable()
                      .Select(row => row.Field<string>("SF_FILE_TYPE"))
                      .Distinct()
                      .ToList();

                    bool hasRD = types.Contains("RD");
                    bool hasHO = types.Contains("HO");
                    bool hasFS = types.Contains("FS");

                    if (lblIsFSAppReq != "Y")
                    {
                        if (dtSubFiles.Rows.Count <= 0)
                        {
                            writeError("Attachment is mandatory to submit.");
                            hfDoubleClickFlagSubmit.Value = "";
                            return;
                        }
                        else if (!hasRD)
                        {
                            writeError("Attachment is mandatory to submit.");
                            hfDoubleClickFlagSubmit.Value = "";
                            return;
                        }
                        else if (!hasHO)
                        {
                            writeError("Attachment is mandatory to submit.");
                            hfDoubleClickFlagSubmit.Value = "";
                            return;
                        }
                    }
                    else
                    {
                        if (dtSubFiles.Rows.Count <= 0)
                        {
                            writeError("Attachment is mandatory to submit.");
                            hfDoubleClickFlagSubmit.Value = "";
                            return;
                        }
                        else if (!hasRD)
                        {
                            writeError("Please attach reporting data.");
                            hfDoubleClickFlagSubmit.Value = "";
                            return;
                        }
                        else if (!hasHO)
                        {
                            writeError("Please attach HOD signoff.");
                            hfDoubleClickFlagSubmit.Value = "";
                            return;
                        }
                        else if (!hasFS)
                        {
                            writeError("Please attach finance signoff.");
                            hfDoubleClickFlagSubmit.Value = "";
                            return;
                        }
                    }
                }
                else
                {
                    dtSubFiles = utilitiesBL.getDatasetWithConditionInString("SUBMISSIONSFILESFromType",
                   " and SF_FILE_TYPE IN ('RD','HO','FS') AND SF_SC_ID = " + intSCId + " ", mstrConnectionString);

                    int intRDSubCount = dtSubFiles.AsEnumerable().Count(r => r.Field<string>("SF_FILE_TYPE") == "RD" 
                        && r.Field<string>("SF_OPERATION_TYPE") == "Submission");

                    int intRDReSubCount = dtSubFiles.AsEnumerable().Count(r => r.Field<string>("SF_FILE_TYPE") == "RD" 
                        && r.Field<string>("SF_OPERATION_TYPE") == "Resubmission");

                    int intHOCount = dtSubFiles.AsEnumerable().Count(r => r.Field<string>("SF_FILE_TYPE") == "HO");
                    int intFSCount = dtSubFiles.AsEnumerable().Count(r => r.Field<string>("SF_FILE_TYPE") == "FS");

                    if (dtSubFiles.Rows.Count <= 2)
                    {
                        writeError("Attachment is mandatory to submit.");
                        hfDoubleClickFlagSubmit.Value = "";
                        return;
                    }
                    else if (intRDSubCount < 1)
                    {
                        writeError("Please attach reporting data.");
                        hfDoubleClickFlagSubmit.Value = "";
                        return;
                    }
                    else if (intRDReSubCount < 1)
                    {
                        writeError("Please attach resubmission reporting data.");
                        hfDoubleClickFlagSubmit.Value = "";
                        return;
                    }
                    if (intHOCount < 1)
                    {
                        writeError("Please attach HOD signoff.");
                        hfDoubleClickFlagSubmit.Value = "";
                        return;
                    }

                    if (lblIsFSAppReq == "Y")
                    {
                        if (intFSCount < 1)
                        {
                            writeError("Please attach finance signoff.");
                            hfDoubleClickFlagSubmit.Value = "";
                            return;
                        }
                    }
                }
                //>>

                hfSelectedRecord.Value = strSubmissionId;
                //<<Modified by Ashish Mishra on 27Jul2017 commented parameter strSubDate
                if (hfSelectedOperation.Value.Equals("Submit"))
                {
                    if (strSubmissionId != "")
                    {
                        int intSubId = Convert.ToInt32(strSubmissionId);
                        intSubmissions = SubmissionMasterBLL.saveChecklistDetails(intSubId, intSCId, ddlYesNoNA.SelectedValue,/* strSubDate,*/
                                strRemarks, "S", strUser, strClientFileName, strServerFileName, mstrConnectionString);
                    }
                    else
                    {
                        intSubmissions = SubmissionMasterBLL.saveChecklistDetails(0, intSCId, ddlYesNoNA.SelectedValue,/* strSubDate,*/
                                strRemarks, "S", strUser, strClientFileName, strServerFileName, mstrConnectionString);
                    }
                    //>>
                    hfDoubleClickFlagSubmit.Value = "";
                    sendSubmissionMail();
                }

                if (!ddlReportDept.SelectedValue.Equals(""))
                {
                    intReportingDeptId = Convert.ToInt32(ddlReportDept.SelectedValue);
                }
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
                //strStatus = ddlStatus.SelectedValue.ToString();
                //<<Modified by Ankur Tyagi on 21-Apr-2025 for Project Id : 2395
                //Added Global Search
                dsChecklist = SubmissionMasterBLL.LoadComplianceChecklistForApproval(Session["MonthSelected"].ToString(),
                    Authentication.GetUserID(Page.User.Identity.Name), Convert.ToInt32(ddlFinYear.SelectedValue),
                    intReportingDeptId, strStatus, txtGlobalSearch.Text, ddlSegment.SelectedValue, ddlFrequency.SelectedValue, mstrConnectionString);
                //>>
                gvChecklistDetails.DataSource = dsChecklist;
                gvChecklistDetails.DataBind();
                if (hfSelectedOperation.Value.Equals("Save Draft"))
                    writeError("Submission drafted successfully.");
                else if (hfSelectedOperation.Value.Equals("Submit"))
                    writeError("Submission approved successfully.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                string strSubChklistId;
                DataTable dtReportingDeptOwners = new DataTable();
                DataTable dtTrackingDeptOwners = new DataTable();
                DataTable dtComplianceUsers = new DataTable();
                DataServer dserv = new DataServer();
                MembershipUser user;

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
                DataTable dtMailConfig = mailBLL.searchMailConfig(null, "Due Submission is Approved");
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

                //<<Modified by Rahuldeb on 30Oct2019 the ESM_STM_ID is set to 1 as it is the ID for Compliance
                if (strComplianceStatus == "NA")
                {
                    dtComplianceUsers = dserv.Getdata("select EM_EMPNAME, EM_EMAIL From " +
                                        " EmployeeMaster " +
                                        " inner join TBL_EM_STM_MAPPING on " +
                                        " ESM_EM_ID = EM_ID and ESM_STM_ID = 1 WHERE EM_STATUS='A' ");
                }
                //>>

                dtTrackingDeptOwners = SubmissionMasterBLL.getTrackingDeptOwners(strSubChklistId, mstrConnectionString);

                strTo = new string[dtTrackingDeptOwners.Rows.Count];

                strCC = new string[dtReportingDeptOwners.Rows.Count + dtComplianceUsers.Rows.Count + 1];

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

                user = Membership.GetUser(Page.User.Identity.Name);
                strCC[i] = user.Email;

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
                            "<th " + strTableHeaderCSS + ">Reporting to</th>" +
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

                strContent = strContent.Replace("%SubmittedBy%", Getfullname(strUser) + " on " + System.DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));
                strContent = strContent.Replace("%SubmissionTable%", strSubTable);
                strContent = strContent.Replace("%Footer%", ConfigurationManager.AppSettings["MailFooter"].ToString());
                strContent = strContent.Replace("%Link%", "<a href=" + Global.site_url("Projects/Submissions/SubmissionCheckListForClosure.aspx") + " target=\"_blank\">Click here</a>");

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

        //Added By Urvashi Gupta on 28Apr2016 for Submission date
        public void cvSubDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        //<<Added by Rahuldeb on 17Jun2017
        public void cvRemarks_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
        //>>

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvChecklistDetails.AllowPaging = false;
            gvChecklistDetails.AllowSorting = false;
            gvChecklistDetails.DataBind();
            string attachment = "attachment; filename=Reports.xls";

            Response.ClearContent();

            Response.AddHeader("content-disposition", attachment);

            Response.ContentType = "application/ms-excel";

            //string tab = "";
            string strHTML = "";
            strHTML = "<table border=\"1\"> <tr>";
            for (int i = 1; i <= gvChecklistDetails.Columns.Count - 4; i++)
            {
                if ((i.Equals(2)) || (i.Equals(3)) || (i.Equals(21)) || (i.Equals(22)) || (i.Equals(23)))
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
                if (gvr.RowType == DataControlRowType.DataRow)
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
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblReportFun")).Text + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblSegment")).Text + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblEvent")).Text + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblAgenda")).Text + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lbleventFrom")).Text + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblReference")).Text.Replace(Environment.NewLine, "<br/>") + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblSection")).Text.Replace(Environment.NewLine, "<br/>") + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblParticulars")).Text.Replace(Environment.NewLine, "<br/>") + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblDescription")).Text.Replace(Environment.NewLine, "<br/>") + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblFrequency")).Text + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblDueDateFrom")).Text + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblDueDateTo")).Text + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblSubmittedby")).Text + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblSubmittedOn")).Text + "</td>";
                    strHTML += "<td>" + strYesNoNA + "&nbsp;</td>"; //((RadioButtonList)gvr.FindControl("rblYesNoNA")).SelectedItem.Text + "</td>";
                                                                    //<<Commented by Ashish Mishra on 27Jul2017
                                                                    //strHTML += "<td>" + ((F2FTextBox)gvr.FindControl("txtSubDate")).Text + "</td>";
                                                                    //>>
                    strHTML += "<td>" + ((F2FTextBox)gvr.FindControl("txtRemarks")).Text.Replace(Environment.NewLine, "<br/>") + "&nbsp;</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblClosedby")).Text + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblClosedOn")).Text + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblReopenedBy")).Text + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblReopenedOn")).Text + "</td>";

                    strHTML += "<td>" + ((Label)gvr.FindControl("lblReopenDueTo")).Text + "</td>";

                    strHTML += "<td>" + ((Label)gvr.FindControl("lblReOpenComments")).Text.Replace(Environment.NewLine, "<br/>") + "</td>";
                    strHTML += "</tr>";
                }
            }
            strHTML += "</table>";

            string html2 = Regex.Replace(strHTML, @"(<input type=""image""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<input class=""checkbox""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<a \/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            Response.Write(html2.ToString());

            #region Commented Code
            //Response.Write("<table border=\"1\"> <tr>");
            //for (int i = 1; i <= gvChecklistDetails.Columns.Count - 4; i++)
            //{
            //    if (!i.Equals(2))
            //    {
            //        Response.Write("<td><b>" + gvChecklistDetails.Columns[i].HeaderText);
            //        Response.Write("</b></td>");
            //    }

            //    //tab = "\t";

            //}
            //Response.Write("</tr>");

            //foreach (GridViewRow gvr in gvChecklistDetails.Rows)
            //{
            //    if (gvr.RowType == DataControlRowType.DataRow)
            //    {
            //        Response.Write("<tr>");
            //        Response.Write("<td>" + ( (Label)gvr.FindControl("lblSrNo") ).Text + "</td>");
            //        Response.Write("<td>" + ( (Label)gvr.FindControl("lblstmType") ).Text + "</td>");
            //        Response.Write("<td>" + ( (Label)gvr.FindControl("lblReportFun") ).Text + "</td>");
            //        Response.Write("<td>" + ( (Label)gvr.FindControl("lblSegment") ).Text + "</td>");
            //        Response.Write("<td>" + ( (Label)gvr.FindControl("lblEvent") ).Text + "</td>");
            //        Response.Write("<td>" + ( (Label)gvr.FindControl("lblAgenda") ).Text + "</td>");
            //        Response.Write("<td>" + ( (Label)gvr.FindControl("lbleventFrom") ).Text + "</td>");
            //        Response.Write("<td>" + ( (Label)gvr.FindControl("lblParticulars") ).Text + "</td>");
            //        Response.Write("<td>" + ( (Label)gvr.FindControl("lblDescription") ).Text + "</td>");
            //        Response.Write("<td>" + ( (Label)gvr.FindControl("lblFrequency") ).Text + "</td>"); //Added By Urvashi Gupta On 06May2106
            //        Response.Write("<td>" + ( (Label)gvr.FindControl("lblDueDateFrom") ).Text + "</td>");
            //        Response.Write("<td>" + ( (Label)gvr.FindControl("lblDueDateTo") ).Text + "</td>");
            //        Response.Write("<td>" + ( (RadioButtonList)gvr.FindControl("rblYesNoNA") ).SelectedValue + "</td>");
            //        Response.Write("<td>" + ( (F2FTextBox)gvr.FindControl("txtSubDate") ).Text + "</td>");
            //        Response.Write("<td>" + ( (F2FTextBox)gvr.FindControl("txtRemarks") ).Text + "</td>");
            //        Response.Write("</tr>");
            //    }
            //}
            //Response.Write("</table>");
            #endregion

            Response.End();
            gvChecklistDetails.AllowPaging = true;
            gvChecklistDetails.AllowSorting = true;
            gvChecklistDetails.DataBind();
        }

        protected void gvChecklistDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvChecklistDetails.PageIndex = e.NewPageIndex;
            gvChecklistDetails.DataSource = Session["DeptChklistSelectCommand"];
            gvChecklistDetails.DataBind();
        }

        #region Code for Sorting
        protected void gvChecklistDetails_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (Session["DeptChklistSelectCommand"] != null)
            {
                DataTable dt = ((DataSet)(Session["DeptChklistSelectCommand"])).Tables[0];
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
            Image sortImage = new Image();
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

        //<<Added by Ashish Mishra on 11SEP2017
        protected DataTable LoadSubmissionFileList(object ScId)
        {
            DataTable dt = utilitiesBL.getDatasetWithCondition("SUBMISSIONSFILES", Convert.ToInt32(ScId), mstrConnectionString);
            return dt;
        }
        //>>
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