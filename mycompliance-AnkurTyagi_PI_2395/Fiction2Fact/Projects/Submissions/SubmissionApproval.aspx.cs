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
using Org.BouncyCastle.Asn1.Cmp;
using System.Collections.Generic;
using Microsoft.VisualBasic.ApplicationServices;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class SubmissionApproval : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
        UtilitiesBLL utilitiesBL = new UtilitiesBLL();
        UtilitiesBLL utilBLL = new UtilitiesBLL();
        UtilitiesVO utliVo = new UtilitiesVO();
        RefCodesBLL rcBL = new RefCodesBLL();

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
            if (Page.IsPostBack)
            {
                if (Session["DeptChklistSelectCommand1"] != null)
                {
                    gvApprovalList.DataSource = (DataTable)(Session["DeptChklistSelectCommand1"]);
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
                    ddlSegment.DataSource = utilitiesBL.getDataset("SUBMISSIONSGMT", mstrConnectionString);
                    ddlSegment.DataBind();

                    ddlReportDept.DataSource = utilitiesBL.getDataset("REPORTINGDEPT", mstrConnectionString);
                    ddlReportDept.DataBind();
                    ddlReportDept.Items.Insert(0, new ListItem("(Select an option)", ""));

                    ddlStatus.DataSource = rcBL.getRefCodeDetails("Submisssion Workflow", mstrConnectionString);
                    ddlStatus.DataBind();
                    ddlStatus.Items.Insert(0, new ListItem("(Select an option)", ""));
                }
            }
        }

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            ListItem liChkBoxListItem;
            string strStatus = "";
            int intReportingDeptId = 0;
            string strFinYear = "0";

            try
            {
                if (!ddlReportDept.SelectedValue.Equals(""))
                {
                    intReportingDeptId = Convert.ToInt32(ddlReportDept.SelectedValue);
                }
                string strGlobalSearch = txtGlobalSearch.Text;
                strStatus = ddlStatus.SelectedValue;

                DataTable dt = SubmissionMasterBLL.getSubmissionforApproval(0, Authentication.GetUserID(Page.User.Identity.Name), intReportingDeptId, strStatus,
                    ddlSegment.SelectedValue, strGlobalSearch, mstrConnectionString);

                Session["DeptChklistSelectCommand1"] = dt;
                gvApprovalList.DataSource = dt;
                gvApprovalList.DataBind();

                if (gvApprovalList.Rows.Count > 0)
                {
                    lnkApprove.Visible = true;
                    lnkReject.Visible = true;
                }
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

        int ParseStringToInt(string input)
        {
            int result;
            if (int.TryParse(input, out result))
            {
                return result;
            }
            return 0;
        }

        protected void gvApprovalList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int intOut = 0;
            GridViewRow gvr = gvApprovalList.SelectedRow;
            Label lblSM_ID = (Label)gvr.FindControl("lblSM_ID");

            if (hfSelectedOperation.Value == "Approve")
            {
                try
                {
                    Label lblSM_SUB_TYPE = (Label)gvr.FindControl("lblSM_SUB_TYPE");
                    Label lblSubmissionType = (Label)gvr.FindControl("lblSubmissionType");
                    Label lblSM_EFFECTIVE_DT = (Label)gvr.FindControl("lblSM_EFFECTIVE_DT");
                    Label lblEM_ID = (Label)gvr.FindControl("lblEM_ID");
                    Label lblEvent = (Label)gvr.FindControl("lblEvent");
                    Label lblEP_ID = (Label)gvr.FindControl("lblEP_ID");
                    Label lblAgenda = (Label)gvr.FindControl("lblAgenda");

                    Label lblFrequency = (Label)gvr.FindControl("lblFrequency");
                    Label lblReportFun = (Label)gvr.FindControl("lblReportFun");
                    Label lblSRD_ID = (Label)gvr.FindControl("lblSRD_ID");
                    Label lblSTM_ID = (Label)gvr.FindControl("lblSTM_ID");

                    Label lblMDT = (Label)gvr.FindControl("lblMDT");
                    Label lblOFD = (Label)gvr.FindControl("lblOFD");
                    Label lblOTD = (Label)gvr.FindControl("lblOTD");

                    Label lblQ1DT = (Label)gvr.FindControl("lblQ1DT");
                    Label lblQ2DT = (Label)gvr.FindControl("lblQ2DT");
                    Label lblQ3DT = (Label)gvr.FindControl("lblQ3DT");
                    Label lblQ4DT = (Label)gvr.FindControl("lblQ4DT");

                    Label lblFHDF = (Label)gvr.FindControl("lblFHDF");
                    Label lblFHDT = (Label)gvr.FindControl("lblFHDT");
                    Label lblYDT = (Label)gvr.FindControl("lblYDT");
                    Label lblWDT = (Label)gvr.FindControl("lblWDT");

                    Label lblFFD = (Label)gvr.FindControl("lblFFD");
                    Label lblFTD = (Label)gvr.FindControl("lblFTD");
                    Label lblSFD = (Label)gvr.FindControl("lblSFD");
                    Label lblSTD = (Label)gvr.FindControl("lblSTD");

                    Label lblParticulars1 = (Label)gvr.FindControl("lblParticulars1");
                    Label lblDescription1 = (Label)gvr.FindControl("lblDescription1");

                    Label lblSegment = (Label)gvr.FindControl("lblSegment");

                    int updatedRows = SubmissionMasterBLL.updateSubmissionApproval(ParseStringToInt(lblSM_ID.Text), "B", "", Authentication.GetUserID(Page.User.Identity.Name), mstrConnectionString);
                    if (updatedRows > 0)
                    {
                        //generateChecklist(ParseStringToInt(lblSM_ID.Text), lblSM_SUB_TYPE.Text, lblSM_EFFECTIVE_DT.Text, lblEM_ID.Text, lblEvent.Text,
                        //    lblEP_ID.Text, lblAgenda.Text);

                        //sendMailToReportingOwnerOnCreation(lblSM_SUB_TYPE.Text, lblSubmissionType.Text, lblFrequency.Text, lblReportFun.Text, ParseStringToInt(lblSRD_ID.Text),
                        //    ParseStringToInt(lblSTM_ID.Text), lblMDT.Text, lblOFD.Text, lblOTD.Text, lblQ1DT.Text, lblQ2DT.Text, lblQ3DT.Text, lblQ4DT.Text,
                        //    lblFHDT.Text, lblFHDF.Text, lblYDT.Text, lblWDT.Text, lblFFD.Text, lblFTD.Text, lblSFD.Text, lblSTD.Text,
                        //    lblParticulars1.Text, lblDescription1.Text, lblSegment.Text);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else if (hfSelectedOperation.Value == "Reject")
            {
                int updatedRows = SubmissionMasterBLL.updateSubmissionApproval(ParseStringToInt(lblSM_ID.Text), "C", "", Authentication.GetUserID(Page.User.Identity.Name), mstrConnectionString);
                if (updatedRows > 0)
                {
                    writeError("Submission rejected.");
                }
            }
            lnkSearch_Click(sender, e);
        }

        private void generateChecklist(int intSubmissionMasterId, string strSubmissionType, string strEffectiveDate, string strEventId, string strEventName,
            string strEventPurposeId, string strEventPurposeName)
        {
            DataServer dserv = new DataServer();
            DataTable dt = new DataTable();
            DataRow dr;
            string strId = "";
            if (strSubmissionType == "F")
            {
                int intNoOfrecords;
                intNoOfrecords = SubmissionMasterBLL.generateSubmissionsWiseChecklist(intSubmissionMasterId, strEffectiveDate, mstrConnectionString);
                writeError("Submission details saved successfully.");
            }
            else if (strSubmissionType == "E")
            {
                dt = dserv.Getdata(" Select * From [TBL_EI_EP_MAPPING] " +
                                  " inner join [TBL_EVENT_PURPOSE] on [EEM_EP_ID] = [EP_ID] and [EP_ID] = " + strEventPurposeId +
                                  " inner join [TBL_EVENT_INSTANCES] on [EEM_EI_ID] = [EI_ID] and [EI_EM_ID] = " + strEventId +
                                  " AND EI_EVENT_DATE >= '" + strEffectiveDate + "'");
                if (dt.Rows.Count > 0)
                {
                    int intNoOfrecords;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = dt.Rows[i];

                        strId = strId + dr["EEM_ID"].ToString() + ",";
                    }

                    if (!strId.Equals(""))
                        strId = strId.Substring(0, strId.Length - 1);

                    intNoOfrecords = SubmissionMasterBLL.generateSubmissionsWiseEventChecklist(intSubmissionMasterId, strId, mstrConnectionString);
                    writeError("Submission details saved successfully.");
                }
                else
                {
                    writeError("Submission details saved successfully. But the tasks were not created as there are no entries made for Event "
                        + strEventName + " and Agenda " + strEventPurposeName + " in Event Instances. Please make an entry for the same.");
                }
                //string str
                //intNoOfrecords = SubmissionMasterBLL.generateSubmissionsWiseChecklist(intSubmissionMasterId, strEffectiveDate, mstrConnectionString);
            }
        }

        //<< Added By Vivek on 23-Jun-2017
        private void sendMailToReportingOwnerOnCreation(string strSubmissionType, string strSubmissionTypeFull, string strFrequency, string strReportingDepartment, int intReportingDept,
            int intTrackingDept, string OFD, string OTD,
            string WDF, string WDT,
            string FFFD, string FFTD, string SFFD, string SFTD,
            string MDF, string MDT,
            string Q1DF, string Q2DF, string Q3DF, string Q4DF, string Q1DT, string Q2DT, string Q3DT, string Q4DT,
            string FHDF, string FHDT, string SHDF, string SHDT,
            string YDF, string YDT,
            string strParticulars, string strDescription, string strAuthority)
        {
            try
            {
                Mail mm = new Mail();
                MembershipUser user;
                Authentication auth = new Authentication();
                DateTime dt = System.DateTime.Now;
                string[] strTo;
                string[] strCC;
                string strContent = "", strUserName = "", strUserDetails = "";
                string[] strUsers = new string[0];
                string strSubject, strIntDueDates = "", strRegDueDates = "";

                //L0 User of Reporting Department
                DataTable dtL0RD = utilitiesBL.getDatasetWithTwoConditionInString("getReportingDeptUserFromLevel", Convert.ToString(intReportingDept), "0", mstrConnectionString);
                strTo = new string[dtL0RD.Rows.Count];
                int j = 0;
                if (strTo.GetUpperBound(0) >= 0)
                {
                    foreach (DataRow dr in dtL0RD.Rows)
                    {
                        strTo[j] = Convert.ToString(dr["EmailId"]);
                        j = j + 1;
                    }
                }

                //All Level Users of Tracking dept, L1 of Reporting Department and LoggedInUser
                DataTable dtAllTDU = new DataTable();
                DataTable dtL1RDU = new DataTable();

                dtAllTDU = utilitiesBL.getDatasetWithCondition("OWNERS_MAIL", intTrackingDept, mstrConnectionString);
                dtL1RDU = utilitiesBL.getDatasetWithTwoConditionInString("getReportingDeptUserFromLevel", Convert.ToString(intReportingDept), "1", mstrConnectionString);

                strCC = new string[dtAllTDU.Rows.Count + dtL1RDU.Rows.Count + 1];

                int i = 0;
                if (strCC.GetUpperBound(0) >= 0)
                {
                    foreach (DataRow dr in dtAllTDU.Rows)
                    {
                        strCC[i] = Convert.ToString(dr["EM_EMAIL"]);
                        i = i + 1;
                    }
                    foreach (DataRow dr in dtL1RDU.Rows)
                    {
                        strCC[i] = Convert.ToString(dr["EmailId"]);
                        i = i + 1;
                    }
                }
                user = Membership.GetUser(Page.User.Identity.Name);
                strCC[i] = user.Email;

                strUserDetails = auth.GetUserDetsByEmpCode(Page.User.Identity.Name);
                strUsers = strUserDetails.Split('|');
                strUserName = strUsers[0].ToString();

                if (strSubmissionType.Equals("F"))
                {
                    if (strFrequency == "Only Once")
                    {
                        strIntDueDates += OFD + "<br/>";
                        strRegDueDates += OTD + "<br/>";
                    }
                    else if (strFrequency.Equals("Weekly"))
                    {
                        strIntDueDates += WDF;
                        strRegDueDates += WDT;
                    }
                    else if (strFrequency == "Fortnightly")
                    {
                        strIntDueDates += "First Fortnightly From Date " + FFFD + ".<br/>";
                        strRegDueDates += "First Fortnightly To Date " + FFTD + ".<br/>";
                        strIntDueDates += "Second Fortnightly From Date " + SFFD + ".<br/>";
                        strRegDueDates += "Second Fortnightly To Date " + SFTD + ".";
                    }
                    else if (strFrequency.Equals("Monthly"))
                    {
                        strIntDueDates += MDF + " day of Every Month";
                        strRegDueDates += MDT + " day of Every Month";
                    }
                    else if (strFrequency.Equals("Quarterly"))
                    {
                        strIntDueDates += Q1DF + " for Quater1.<br/>";
                        strIntDueDates += Q2DF + " for Quater2.<br/>";
                        strIntDueDates += Q3DF + " for Quater3.<br/>";
                        strIntDueDates += Q4DF + " for Quater4.";

                        strRegDueDates += Q1DT + " for Quater1.<br/>";
                        strRegDueDates += Q2DT + " for Quater2.<br/>";
                        strRegDueDates += Q3DT + " for Quater3.<br/>";
                        strRegDueDates += Q4DT + " for Quater4.";
                    }
                    else if (strFrequency.Equals("Half Yearly"))
                    {
                        strIntDueDates += FHDF + " for First half.<br/>";
                        strIntDueDates += FHDT + " for Second half.";

                        strRegDueDates += SHDF + " for First half.<br/>";
                        strRegDueDates += SHDT + " for Second half.";
                    }
                    else if (strFrequency.Equals("Yearly"))
                    {
                        strIntDueDates += YDF + " of every year.";
                        strRegDueDates += YDT + " of every year.";
                    }
                    
                    else
                    {
                        strIntDueDates = "";
                        strRegDueDates = "";
                    }
                }

                string strSubTable = "<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>" +
                            "<th " + strTableHeaderCSS + ">Reporting Department</th>" +
                            "<th " + strTableHeaderCSS + ">Reporting To</th>" +
                            "<th " + strTableHeaderCSS + ">Particulars</th>" +
                            "<th " + strTableHeaderCSS + ">Brief Description</th>" +
                            "<th " + strTableHeaderCSS + ">Type</th>" +
                            "<th " + strTableHeaderCSS + ">Frequency</th>";

                if (!strIntDueDates.Equals(""))
                {
                    strSubTable += "<th " + strTableHeaderCSS + ">Internal Due Date</th>";
                }
                if (!strRegDueDates.Equals(""))
                {
                    strSubTable += "<th " + strTableHeaderCSS + ">Regulatory Due Date</th>";
                }
                strSubTable += "</tr>";

                strSubTable += "<tr><td " + strTableCellCSS + ">" + 1 + "</td>" +
                "<td " + strTableCellCSS + ">" + strReportingDepartment + "</td>" +
                "<td " + strTableCellCSS + ">" + strAuthority + "</td>" +
                "<td " + strTableCellCSS + ">" + strParticulars.ToString().Replace(Environment.NewLine, "<br />") + "</td>" +
                "<td " + strTableCellCSS + ">" + strDescription.ToString().Replace(Environment.NewLine, "<br />") + "</td>" +
                "<td " + strTableCellCSS + ">" + strSubmissionTypeFull + "</td>" +
                "<td " + strTableCellCSS + ">" + strFrequency + "</td>";

                if (!strIntDueDates.Equals(""))
                {
                    strSubTable += "<td " + strTableCellCSS + ">" + strIntDueDates + "</td>";
                }
                if (!strRegDueDates.Equals(""))
                {
                    strSubTable += "<td " + strTableCellCSS + ">" + strRegDueDates + "</td>";
                }
                strSubTable += "</table>";

                MailConfigBLL mailBLL = new MailConfigBLL();
                DataTable dtMailConfig = mailBLL.searchMailConfig("1095");
                if (dtMailConfig == null || dtMailConfig.Rows.Count == 0)
                {
                    writeError("Mail Configuration is not set, could not send mail");
                    return;
                }
                DataRow drMailConfig = dtMailConfig.Rows[0];
                strSubject = drMailConfig["MCM_SUBJECT"].ToString();
                strContent = drMailConfig["MCM_CONTENT"].ToString();

                strSubject = strSubject.Replace("%Authority%", strAuthority);
                strSubject = strSubject.Replace("%AddEditType%", "approved");
                strSubject = strSubject.Replace("%Department%", strReportingDepartment);

                strContent = strContent.Replace("%Authority%", strAuthority);
                strContent = strContent.Replace("%AddEditType%", "approved");
                strContent = strContent.Replace("%Department%", strReportingDepartment);
                strContent = strContent.Replace("%SubmittedBy%", Getfullname(strUserName));
                strContent = strContent.Replace("%SubmittedDate%", dt.ToString("dd-MMM-yyyy HH:mm:ss"));
                strContent = strContent.Replace("%SubmissionTable%", strSubTable);
                strContent = strContent.Replace("%Footer%", ConfigurationManager.AppSettings["MailFooter"].ToString());
                strContent = strContent.Replace("%Link%", "<a href=" + Global.site_url("Projects/Submissions/ComplianceChecklist.aspx?Type=4") + " target=\"_blank\">Click here</a>");

                //strContent = strContent + "<br/><br/>" +
                //ConfigurationManager.AppSettings["MailFooter"].ToString() + "</body></html>";

                mm.sendAsyncMail(strTo, strCC, null, strSubject, strContent);
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }
        //>>

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void gvApprovalList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                gvr = e.Row;
                System.Data.DataRowView drv = e.Row.DataItem as System.Data.DataRowView;

                if (drv != null)
                {
                    HiddenField hfCircularId = (HiddenField)(gvr.FindControl("hfCircularId"));
                    LinkButton lnkViewCirc = (LinkButton)(gvr.FindControl("lnkViewCirc"));
                    LinkButton lnkAction = (LinkButton)e.Row.FindControl("lnkAction");
                    Label lblSM_ID = (Label)e.Row.FindControl("lblSM_ID");
                    Label lblAROn = (Label)e.Row.FindControl("lblAROn");
                    LinkButton lbEdit = (LinkButton)e.Row.FindControl("lbEdit");

                    lnkAction.OnClientClick = "return onActionClick('" + ParseStringToInt(lblSM_ID.Text) + "');";
                    lbEdit.OnClientClick = "return editSMId('" + ParseStringToInt(lblSM_ID.Text) + "');";

                    if (hfCircularId.Value.Equals("") || hfCircularId.Value.Equals("0"))
                    {
                        lnkViewCirc.Visible = false;
                    }
                    else
                    {
                        lnkViewCirc.Visible = true;
                    }

                    lnkViewCirc.OnClientClick = "onClientViewCircClick('" + (new SHA256EncryptionDecryption()).Encrypt(hfCircularId.Value) + "');";


                    Label lblSM_WORKFLOW_STATUS = (Label)(gvr.FindControl("lblSM_WORKFLOW_STATUS"));
                    LinkButton lbApprove = (LinkButton)(gvr.FindControl("lbApprove"));
                    LinkButton lbReject = (LinkButton)(gvr.FindControl("lbReject"));
                    CheckBox RowLevelCheckBox = (CheckBox)(gvr.FindControl("RowLevelCheckBox"));

                    if (lblSM_WORKFLOW_STATUS.Text == "A")
                    {
                        lbApprove.Visible = true;
                        lbReject.Visible = true;
                        lnkAction.Visible = true;
                        RowLevelCheckBox.Enabled = true;
                        lbEdit.Visible = true;
                    }
                    else
                    {
                        lbApprove.Visible = false;
                        lbReject.Visible = false;
                        lnkAction.Visible = false;
                        RowLevelCheckBox.Enabled = false;
                        lbEdit.Visible = false;
                    }

                    if (lblAROn.Text == "01-Jan-1900 00:00:00")
                    {
                        lblAROn.Text = "";
                    }


                }
            }
        }

        protected void gvApprovalList_DataBound(object sender, EventArgs e)
        {
            if (gvApprovalList.Rows.Count == 0)
            {
                return;
            }
            CheckBox HeaderLevelCheckBox = (CheckBox)(gvApprovalList.HeaderRow.FindControl("HeaderLevelCheckBox"));
            HeaderLevelCheckBox.Attributes["onclick"] = "ChangeAllCheckBoxStates(this.checked);";
            List<string> ArrayValues = new List<string>();
            ArrayValues.Add(String.Concat("'", HeaderLevelCheckBox.ClientID, "'"));

            GridViewRow gvr;
            //F2FTextBox txtComments;
            for (int intIndex = 0; intIndex < gvApprovalList.Rows.Count; intIndex++)
            {
                string strId = Convert.ToString(intIndex);
                gvr = gvApprovalList.Rows[intIndex];
                CheckBox RowLevelCheckBox = (CheckBox)(gvr.FindControl("RowLevelCheckBox"));
                RowLevelCheckBox.Attributes["onclick"] = "onRowCheckedUnchecked('" + RowLevelCheckBox.ClientID + "');";
                ArrayValues.Add(string.Concat("'", RowLevelCheckBox.ClientID, "'"));
            }

            CheckBoxIDsArray.Text = ("<script type=\"text/javascript\">" + ("\r\n" + ("<!--" + ("\r\n"
                        + (string.Concat("var CheckBoxIDs =  new Array(", string.Join(",", ArrayValues.ToArray()), ");")
                        + ("\r\n" + ("// -->" + ("\r\n" + "</script>"))))))));
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
            gvApprovalList.AllowPaging = false;
            gvApprovalList.AllowSorting = false;
            gvApprovalList.DataBind();
            string attachment = "attachment; filename=Reports.xls";

            Response.ClearContent();

            Response.AddHeader("content-disposition", attachment);

            Response.ContentType = "application/ms-excel";

            //string tab = "";
            string strHTML = "";
            strHTML = "<table border=\"1\"> <tr>";
            for (int i = 1; i <= gvApprovalList.Columns.Count - 4; i++)
            {
                if ((i.Equals(2)) || (i.Equals(3)) || (i.Equals(21)) || (i.Equals(22)) || (i.Equals(23)))
                {
                }
                else
                {
                    strHTML += "<td><b>" + gvApprovalList.Columns[i].HeaderText;
                    strHTML += "</b></td>";
                }
            }
            strHTML += "</tr>";

            foreach (GridViewRow gvr in gvApprovalList.Rows)
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

                    strHTML += "<td>" + ((Label)gvr.FindControl("lblAuthority")).Text + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblReportingDept")).Text + "</td>";
                    strHTML += "<td>" + ((Label)gvr.FindControl("lblTrackingDept")).Text + "</td>";

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
            //for (int i = 1; i <= gvApprovalList.Columns.Count - 4; i++)
            //{
            //    if (!i.Equals(2))
            //    {
            //        Response.Write("<td><b>" + gvApprovalList.Columns[i].HeaderText);
            //        Response.Write("</b></td>");
            //    }

            //    //tab = "\t";

            //}
            //Response.Write("</tr>");

            //foreach (GridViewRow gvr in gvApprovalList.Rows)
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
            gvApprovalList.AllowPaging = true;
            gvApprovalList.AllowSorting = true;
            gvApprovalList.DataBind();
        }

        protected void gvApprovalList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvApprovalList.PageIndex = e.NewPageIndex;
            gvApprovalList.DataSource = Session["DeptChklistSelectCommand1"];
            gvApprovalList.DataBind();
        }

        #region Code for Sorting
        protected void gvApprovalList_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (Session["DeptChklistSelectCommand1"] != null)
            {
                DataTable dt = ((DataTable)(Session["DeptChklistSelectCommand1"]));
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

                gvApprovalList.DataSource = dvDataView;
                gvApprovalList.DataBind();
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
                foreach (DataControlField field in gvApprovalList.Columns)
                {
                    if (field.SortExpression.Equals(strSortExpression))
                    {
                        return gvApprovalList.Columns.IndexOf(field);
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

        protected void lbApprove_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow gvr in gvApprovalList.Rows)
                {
                    CheckBox RowLevelCheckBox = (CheckBox)gvr.FindControl("RowLevelCheckBox");
                    if (RowLevelCheckBox.Checked)
                    {
                        Label lblSM_ID = (Label)gvr.FindControl("lblSM_ID");

                        DataTable dt = SubmissionMasterBLL.getSubmissionforApproval(ParseStringToInt(lblSM_ID.Text), "", 0, "",
                            "", "", mstrConnectionString);

                        string lblSM_SUB_TYPE = dt.Rows[0]["SM_SUB_TYPE"].ToString();
                        string lblSubmissionType = dt.Rows[0]["SubmissionType"].ToString();
                        string lblSM_EFFECTIVE_DT = dt.Rows[0]["SM_EFFECTIVE_DT"].ToString();
                        string lblEM_ID = dt.Rows[0]["EM_ID"].ToString();
                        string lblEvent = dt.Rows[0]["EM_EVENT_NAME"].ToString();
                        string lblEP_ID = dt.Rows[0]["EP_ID"].ToString();
                        string lblAgenda = dt.Rows[0]["EP_NAME"].ToString();

                        string lblFrequency = dt.Rows[0]["SM_FREQUENCY"].ToString();
                        string lblReportFun = dt.Rows[0]["SRD_NAME"].ToString();
                        string lblSRD_ID = dt.Rows[0]["SRD_ID"].ToString();
                        string lblSTM_ID = dt.Rows[0]["SM_STM_ID"].ToString();

                        string lblOFD = dt.Rows[0]["SM_ONLY_ONCE_FROM_DATE"].ToString();
                        string lblOTD = dt.Rows[0]["SM_ONLY_ONCE_TO_DATE"].ToString();

                        string lblWDF = dt.Rows[0]["SM_WEEKLY_DUE_DATE_FROM"].ToString();
                        string lblWDT = dt.Rows[0]["SM_WEEKLY_DUE_DATE_TO"].ToString();

                        string lblFFFD = dt.Rows[0]["SM_FIRST_FORTNIGHTLY_DUE_FROM_DATE"].ToString();
                        string lblFFTD = dt.Rows[0]["SM_FIRST_FORTNIGHTLY_DUE_TO_DATE"].ToString();
                        string lblSFFD = dt.Rows[0]["SM_SECOND_FORTNIGHTLY_DUE_FROM_DATE"].ToString();
                        string lblSFTD = dt.Rows[0]["SM_SECOND_FORTNIGHTLY_DUE_TO_DATE"].ToString();

                        string lblMDF = dt.Rows[0]["SM_MONTHLY_DUE_DATE_FROM"].ToString();
                        string lblMDT = dt.Rows[0]["SM_MONTHLY_DUE_DATE_TO"].ToString();

                        string lblQ1DF = dt.Rows[0]["SM_Q1_DUE_DATE_FROM"].ToString();
                        string lblQ2DF = dt.Rows[0]["SM_Q2_DUE_DATE_FROM"].ToString();
                        string lblQ3DF = dt.Rows[0]["SM_Q3_DUE_DATE_FROM"].ToString();
                        string lblQ4DF = dt.Rows[0]["SM_Q4_DUE_DATE_FROM"].ToString();

                        string lblQ1DT = dt.Rows[0]["SM_Q1_DUE_DATE_TO"].ToString();
                        string lblQ2DT = dt.Rows[0]["SM_Q2_DUE_DATE_TO"].ToString();
                        string lblQ3DT = dt.Rows[0]["SM_Q3_DUE_DATE_TO"].ToString();
                        string lblQ4DT = dt.Rows[0]["SM_Q4_DUE_DATE_TO"].ToString();

                        string lblFHDF = dt.Rows[0]["SM_FIRST_HALF_YR_DUE_DATE_FROM"].ToString();
                        string lblFHDT = dt.Rows[0]["SM_FIRST_HALF_YR_DUE_DATE_TO"].ToString();
                        string lblSHDF = dt.Rows[0]["SM_SECOND_HALF_YR_DUE_DATE_FROM"].ToString();
                        string lblSHDT = dt.Rows[0]["SM_SECOND_HALF_YR_DUE_DATE_TO"].ToString();

                        string lblYDF = dt.Rows[0]["SM_YEARLY_DUE_DATE_FROM"].ToString();
                        string lblYDT = dt.Rows[0]["SM_YEARLY_DUE_DATE_TO"].ToString();


                        string lblParticulars1 = dt.Rows[0]["SM_PERTICULARS"].ToString();
                        string lblDescription1 = dt.Rows[0]["SM_BRIEF_DESCRIPTION"].ToString();

                        string lblAuthority = dt.Rows[0]["Authority"].ToString();

                        int updatedRows = SubmissionMasterBLL.updateSubmissionApproval(ParseStringToInt(lblSM_ID.Text), "B", txtARRemarks.Text, Authentication.GetUserID(Page.User.Identity.Name), mstrConnectionString);
                        if (updatedRows > 0)
                        {
                            generateChecklist(ParseStringToInt(lblSM_ID.Text), lblSM_SUB_TYPE, lblSM_EFFECTIVE_DT, lblEM_ID, lblEvent,
                                lblEP_ID, lblAgenda);

                            sendMailToReportingOwnerOnCreation(lblSM_SUB_TYPE, lblSubmissionType, lblFrequency, lblReportFun, ParseStringToInt(lblSRD_ID),
                                ParseStringToInt(lblSTM_ID), lblOFD, lblOTD,
                                lblWDF, lblWDT,
                                lblFFFD, lblFFTD, lblSFFD, lblSFTD,
                                lblMDF, lblMDT,
                                lblQ1DF, lblQ2DF, lblQ3DF, lblQ4DF, lblQ1DT, lblQ2DT, lblQ3DT, lblQ4DT,
                                lblFHDF, lblFHDT, lblSHDF, lblSHDT,
                                lblYDF, lblYDT,
                                lblParticulars1, lblDescription1, lblAuthority);
                        }
                    }
                }

                txtARRemarks.Text = string.Empty;
                hfSelectedRecord.Value = string.Empty;
                hfDoubleClickFlag.Value = string.Empty;
                writeError("Checklist generated successfully.");
                lnkSearch_Click(sender, e);
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = string.Empty;
                throw new Exception(ex.Message);
            }
        }

        protected void lbReject_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow gvr in gvApprovalList.Rows)
                {
                    CheckBox RowLevelCheckBox = (CheckBox)gvr.FindControl("RowLevelCheckBox");
                    if (RowLevelCheckBox.Checked)
                    {
                        Label lblSM_ID = (Label)gvr.FindControl("lblSM_ID");
                        int updatedRows = SubmissionMasterBLL.updateSubmissionApproval(ParseStringToInt(lblSM_ID.Text), "C", txtARRemarks.Text, Authentication.GetUserID(Page.User.Identity.Name), mstrConnectionString);
                    }
                }
                writeError("Task(s) rejected.");
                txtARRemarks.Text = string.Empty;
                hfSelectedRecord.Value = string.Empty;
                lnkSearch_Click(sender, e);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}