using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class Submissions_ViewAllSubmissions : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = null;
            if (Page.IsPostBack)
            {
                if (Session["PastChklistSelectCommand"] != null)
                {
                    gvChecklistDetails.DataSource = (DataSet)(Session["PastChklistSelectCommand"]);
                    //gvChecklistDetails.DataBind();
                }
            }
            else
            {
                //if (Session["PastChklistSelectCommand"] != null)
                //{
                //    gvChecklistDetails.DataSource = (DataSet)(Session["PastChklistSelectCommand"]);
                //    gvChecklistDetails.DataBind();
                //}
                hfCurDate.Value = System.DateTime.Now.ToString("dd-MMM-yyyy");//Added By Urvashi Gupta On 28Apr2016
                if (Session["PastChecklistMonthSelected"] != null)
                {
                    str = (string)Session["PastChecklistMonthSelected"];
                    hfMonth.Value = str;
                }

                ListItem li = new ListItem();
                li.Text = "(Select)";
                li.Value = "";

                DataSet ds = utilBL.getDataset("AllFinYears", mstrConnectionString);
                ddlFinYear.DataSource = ds;
                ddlFinYear.DataBind();
                ddlFinYear.Items.Insert(0, li);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ddlFinYear.Items.Count > 1)
                        {
                            ddlFinYear.SelectedIndex = 1;
                        }
                    }
                }
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
            Session["PastChecklistMonthSelected"] = "1";
            setSubmissionQuery();
        }
        protected void lbtnFeb_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "2";
            Session["PastChecklistMonthSelected"] = "2";
            setSubmissionQuery();

        }
        protected void lbtnMarch_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "3";
            Session["PastChecklistMonthSelected"] = "3";
            setSubmissionQuery();

        }
        protected void lbtnApr_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "4";
            Session["PastChecklistMonthSelected"] = "4";
            setSubmissionQuery();

        }
        protected void lbtnMay_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "5";
            Session["PastChecklistMonthSelected"] = "5";
            setSubmissionQuery();

        }
        protected void lbtnJune_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "6";
            Session["PastChecklistMonthSelected"] = "6";
            setSubmissionQuery();

        }
        protected void lbtnJuly_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "7";
            Session["PastChecklistMonthSelected"] = "7";
            setSubmissionQuery();
        }

        protected void lbtnAug_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "8";
            Session["PastChecklistMonthSelected"] = "8";
            setSubmissionQuery();
        }
        protected void lbtnSep_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "9";
            Session["PastChecklistMonthSelected"] = "9";
            setSubmissionQuery();
        }
        protected void lbtnOct_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "10";
            Session["PastChecklistMonthSelected"] = "10";
            setSubmissionQuery();
        }
        protected void lbtnNov_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "11";
            Session["PastChecklistMonthSelected"] = "11";
            setSubmissionQuery();
        }
        protected void lbtnDec_Click(object sender, EventArgs e)
        {
            hfMonth.Value = "12";
            Session["PastChecklistMonthSelected"] = "12";
            setSubmissionQuery();
        }


        private void setSubmissionQuery()
        {
            string strUserType = "", strUserName = "";
            DataSet dsChecklist = new DataSet();

            //<<Modified by Amey Karangutkar on 12-Jul-2018
            //<< Added Filing_View_Only
            //<<Modified by Ankur Tyagi on 28-Apr-2025 for Project Id : 2395
            //Added FilingSubAdmin
            if (User.IsInRole("FilingAdmin") || User.IsInRole("Filing_View_Only"))
            {
                strUserType = "Admin";
            }
            //>>
            else if (User.IsInRole("FilingUser") || User.IsInRole("Filing_Sub_Admin"))
            {
                strUserType = "Comp";
            }
            //>>
            strUserName = Page.User.Identity.Name.ToString();
            dsChecklist = SubmissionMasterBLL.LoadPastChecklist(Session["PastChecklistMonthSelected"].ToString(), ddlFinYear.SelectedValue,
                                                               strUserType, strUserName, txtGlobalSearch.Text, mstrConnectionString);
            Session["PastChklistSelectCommand"] = dsChecklist;
            gvChecklistDetails.DataSource = dsChecklist;
            gvChecklistDetails.DataBind();
            //hideShowButtons(dsChecklist.Tables[0]);
            if (gvChecklistDetails.Rows.Count == 0)
            {
                writeError("No checklist available for selected Month.");
            }
            else
            {
                writeError("");
            }
        }
        //private void hideShowButtons(DataTable dsChecklist)
        //{
        //    int tot = dsChecklist.Rows.Count;
        //    DataRow dr;
        //    string strServerFileName, strStatus;
        //    string script = "\r\n<script language=\"javascript\">\r\n";
        //    for (int i = 0; i < tot; i++)
        //    {
        //        dr = dsChecklist.Rows[i];
        //        strServerFileName = dr["SUB_SERVER_FILE_NAME"].ToString();
        //        strStatus = dr["SUB_STATUS"].ToString();
        //        if (!strStatus.Equals("S"))
        //        {
        //            if (strServerFileName.Equals(""))
        //            {
        //                script = script +
        //                   " showHideButtons('true'," + i + ");";
        //            }
        //            else
        //            {
        //                script = script +
        //                   " showHideButtons('false'," + i + ");";
        //            }
        //        }
        //        else
        //        {
        //            script = script +
        //                   " hideButtons(" + i + ");";
        //        }

        //    }
        //    script = script + " </script>\r\n";
        //    if (tot > 0)
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), "script", script);
        //    }
        //}

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        #region commented Code
        //protected void gvChecklistDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    GridViewRow gvr;
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        gvr = e.Row;
        //        System.Data.DataRowView drv = e.Row.DataItem as System.Data.DataRowView;

        //        if (drv != null)
        //        {
        //            //String strStatus = drv["SUB_STATUS"].ToString();
        //            //int intScId = Convert.ToInt32(drv["SC_ID"]);
        //            //RadioButtonList rbl = (RadioButtonList)(gvr.FindControl("rblYesNoNA"));
        //            //F2FTextBox txt = (F2FTextBox)(gvr.FindControl("txtRemarks"));
        //            //LinkButton lbSave = (LinkButton)(gvr.FindControl("lbSave"));
        //            //LinkButton lb1 = (LinkButton)(gvr.FindControl("lbAttach"));
        //            //LinkButton lbSubmit = (LinkButton)(gvr.FindControl("lbSubmit"));

        //            //<< Added By Vivek on 23-Jun-2017
        //            //HiddenField hfValidtionGroup = (HiddenField)(gvr.FindControl("hfValidtionGroup"));
        //            //RequiredIfValidatorRadioButtonList RequiredIfValidatorRemarks1 = (RequiredIfValidatorRadioButtonList)(gvr.FindControl("RequiredIfValidatorRemarks1"));
        //            //RequiredIfValidatorRadioButtonList RequiredIfValidatorRemarks2 = (RequiredIfValidatorRadioButtonList)(gvr.FindControl("RequiredIfValidatorRemarks2"));
        //            //>>

        //            //<<Commented by Ashish Mishra on 27Jul2017
        //            //<< Added By Urvashi Gupta On 28Apr2016 for Submission Date field
        //            //F2FTextBox txtSubDate = (F2FTextBox)(gvr.FindControl("txtSubDate"));
        //            //ImageButton imgSubDate = (ImageButton)(gvr.FindControl("imgSubDate"));

        //            //RequiredFieldValidator rfvSubDate = (RequiredFieldValidator)(gvr.FindControl("rfvSubDate"));
        //            //RequiredFieldValidator rfvRBL = (RequiredFieldValidator)(gvr.FindControl("rfvYesNoNA"));
        //            //RegularExpressionValidator revSubDate = (RegularExpressionValidator)(gvr.FindControl("revSubDate"));
        //            //CustomValidator cvSubDate = (CustomValidator)(gvr.FindControl("cvSubDate"));
        //            //CalendarExtender ceSubDate = (CalendarExtender)(e.Row.FindControl("ceSubDate"));

        //            //Page.ClientScript.RegisterExpandoAttribute(cvSubDate.ClientID, "Subdate",txtSubDate.ClientID, false);
        //            //>>
        //            //>>

        //            //if (!drv["SUB_YES_NO_NA"].ToString().Equals(""))
        //            //{
        //            //    rbl.SelectedValue = drv["SUB_YES_NO_NA"].ToString();
        //            //}
        //            //<<Modified by Ashish Mishra on 28Aug2017 (added strStatus == "C")
        //            if (strStatus == "S" || strStatus == "C")
        //            {
        //                //txt.Enabled = false;
        //                //rbl.Enabled = false;
        //                //lbSave.Visible = false;
        //                //lbSubmit.Visible = false;

        //                //<<Commented by Ashish Mishra on 27Jul2017
        //                //<<Added By Urvashi Gupta On 28Apr2016
        //                //txtSubDate.Enabled = false;
        //                //imgSubDate.Enabled = false;
        //                //rfvRBL.Enabled = false;
        //                //rfvSubDate.Enabled = false;
        //                //revSubDate.Enabled = false;
        //                //cvSubDate.Enabled = false;
        //                //ceSubDate.EnabledOnClient = false;
        //                //>>
        //                //>>
        //                //RequiredIfValidatorRemarks1.Enabled = false;
        //                //RequiredIfValidatorRemarks2.Enabled = false;
        //            }
        //            else
        //            {
        //                intCnt++;
        //                //rfvRBL.ValidationGroup = Convert.ToString(intCnt);
        //                //lbSave.ValidationGroup = Convert.ToString(intCnt);

        //                //<<Commented by Ashish Mishra on 27Jul2017
        //                //Added By Urvashi Gupta On 28Apr2016
        //                //rfvSubDate.ValidationGroup = Convert.ToString(intCnt);
        //                //revSubDate.ValidationGroup = Convert.ToString(intCnt);
        //                //cvSubDate.ValidationGroup = Convert.ToString(intCnt);
        //                //lbSubmit.ValidationGroup = Convert.ToString(intCnt);
        //                //>>
        //                //>>

        //                //RequiredIfValidatorRemarks1.ValidationGroup = Convert.ToString(intCnt);
        //                //RequiredIfValidatorRemarks2.ValidationGroup = Convert.ToString(intCnt);
        //                //hfValidtionGroup.Value = Convert.ToString(intCnt);

        //                //txt.Enabled = true;
        //                //rbl.Enabled = true;
        //                //lbSave.Visible = true;
        //                //lbSubmit.Visible = true;
        //                //lb1.Attributes.Add("onclick", "javascript:return openSubmissionFilesWindow('" + Convert.ToString(intScId) + "')");
        //            }
        //        }
        //    }
        //}
        #endregion
        //Added By Urvashi Gupta on 28Apr2016 for Submission date
        public void cvSubDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        //Added By Urvashi Gupta on 05May2016 
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

            gvChecklistDetails.AllowPaging = false;
            gvChecklistDetails.AllowSorting = false;
            gvChecklistDetails.DataBind();
            string attachment = "attachment; filename=Reports.xls";

            Response.ClearContent();

            Response.AddHeader("content-disposition", attachment);

            Response.ContentType = "application/vnd.ms-excel";

            Response.Write("<table border=\"1\"> <tr>");

            for (int i = 1; i <= gvChecklistDetails.Columns.Count - 3; i++)
            {
                Response.Write("<td><b>" + gvChecklistDetails.Columns[i].HeaderText);
                Response.Write("</b></td>");
            }

            Response.Write("</tr>");

            foreach (GridViewRow gvr in gvChecklistDetails.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    Response.Write("<tr>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblSrNo")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblstmType")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblReportFun")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblSegment")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblReference")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblSection")).Text + "</td>");

                    Response.Write("<td>" + ((Label)gvr.FindControl("lblParticulars")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblDescription")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblDueDateFrom")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblDueDateTo")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblYesNoNa")).Text + "</td>");
                    //Response.Write("<td>" + ((F2FTextBox)gvr.FindControl("txtSubDate")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblStatus")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblSubmitby")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblSubmittedOn")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblRemarks")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblClosedBy")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblClosedOn")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblReopenedBy")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblReopenedOn")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblComments")).Text + "</td>");
                    Response.Write("<td>" + ((Label)gvr.FindControl("lblSubAuthorityDate")).Text + "</td>");
                    Response.Write("</tr>");
                }
            }
            Response.Write("</table>");
            Response.End();
            gvChecklistDetails.AllowPaging = true;
            gvChecklistDetails.AllowSorting = true;
            gvChecklistDetails.DataBind();
        }
        #region commented code
        //protected void gvChecklistDetails_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int intSubmissions;
        //        GridViewRow gvr = gvChecklistDetails.SelectedRow;
        //        string strSubmissionId = ((Label)(gvr.FindControl("lblSubId"))).Text;
        //        int intSCId = Convert.ToInt32(gvChecklistDetails.SelectedValue);
        //        //RadioButtonList rblYNNA = ((RadioButtonList)(gvr.FindControl("rblYesNoNA")));
        //        //string strRemarks = Convert.ToString(((F2FTextBox)(gvr.FindControl("txtRemarks"))).Text);
        //        //string strUser = Authentication.GetUserID(Page.User.Identity.Name);
        //        string strUser = Page.User.Identity.Name;
        //        string strClientFileName = ((HiddenField)(gvr.FindControl("ClientFileName"))).Value.ToString();
        //        string strServerFileName = ((HiddenField)(gvr.FindControl("ServerFileName"))).Value.ToString();

        //        //<<Commented by Ashish Mishra on 27Jul2017
        //        //<<Added By Urvashi Gupta On 28Apr2016
        //        //string strSubDate = ((F2FTextBox)(gvr.FindControl("txtSubDate"))).Text;
        //        //>>
        //        //>>

        //        //hfSelectedRecord.Value = strSubmissionId;
        //        //if (hfSelectedOperation.Value.Equals("Save Draft"))
        //        //{
        //        //    if (strSubmissionId != "")
        //        //    {
        //        //        int intSubId = Convert.ToInt32(strSubmissionId);
        //        //        //<< Modified by Ashish Mishra on 27Jul2017
        //        //        intSubmissions = SubmissionMasterBLL.saveChecklistDetails(intSubId, intSCId,
        //        //            rblYNNA.SelectedValue,/* strSubDate,*/ strRemarks, "", strUser, strClientFileName, strServerFileName, mstrConnectionString);
        //        //    }
        //        //    else
        //        //    {
        //        //        intSubmissions = SubmissionMasterBLL.saveChecklistDetails(0, intSCId,
        //        //            rblYNNA.SelectedValue,/* strSubDate,*/ strRemarks, "", strUser, strClientFileName, strServerFileName, mstrConnectionString);
        //        //    }
        //        //    hfDoubleClickFlag.Value = "";
        //        //}
        //        //else if (hfSelectedOperation.Value.Equals("Submit"))
        //        //{
        //        //    if (strSubmissionId != "")
        //        //    {
        //        //        int intSubId = Convert.ToInt32(strSubmissionId);
        //        //        intSubmissions = SubmissionMasterBLL.saveChecklistDetails(intSubId, intSCId, rblYNNA.SelectedValue,/* strSubDate,*/ 
        //        //            strRemarks, "S", strUser, strClientFileName, strServerFileName, mstrConnectionString);
        //        //    }
        //        //    else
        //        //    {
        //        //        intSubmissions = SubmissionMasterBLL.saveChecklistDetails(0, intSCId, rblYNNA.SelectedValue,/* strSubDate,*/ 
        //        //            strRemarks, "S", strUser, strClientFileName, strServerFileName, mstrConnectionString);
        //        //    }
        //        //    hfDoubleClickFlagSubmit.Value = "";
        //        //}
        //        setSubmissionQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        #endregion
        protected DataTable LoadSubmissionFileList(object ScId)
        {
            DataTable dt = utilBL.getDatasetWithCondition("SUBMISSIONSFILES", Convert.ToInt32(ScId), mstrConnectionString);
            return dt;
        }

        //<<Commented By Urvashi Gupta On 05May2016

        //protected void btnExportToExcel_Click(object sender, EventArgs e)
        //{

        //    gvChecklistDetails.AllowPaging = false;
        //    gvChecklistDetails.AllowSorting = false;
        //    gvChecklistDetails.DataBind();
        //    string attachment = "attachment; filename=Reports.xls";

        //    Response.ClearContent();

        //    Response.AddHeader("content-disposition", attachment);

        //    Response.ContentType = "application/vnd.ms-excel";

        //    string tab = "";

        //    for (int i = 2; i <= gvChecklistDetails.Columns.Count - 4; i++)
        //    {
        //        Response.Write(tab + gvChecklistDetails.Columns[i].HeaderText);

        //        tab = "\t";

        //    }

        //    Response.Write("\n");

        //    foreach (GridViewRow gvr in gvChecklistDetails.Rows)
        //    {
        //        if (gvr.RowType == DataControlRowType.DataRow)
        //        {
        //            tab = "";

        //            Response.Write(tab + ((Label)gvr.FindControl("lblstmType")).Text);
        //            tab = "\t";
        //            Response.Write(tab + ((Label)gvr.FindControl("lblReportFun")).Text);
        //            tab = "\t";
        //            Response.Write(tab + ((Label)gvr.FindControl("lblSegment")).Text);
        //            tab = "\t";
        //            Response.Write(tab + ((Label)gvr.FindControl("lblParticulars")).Text);
        //            tab = "\t";
        //            Response.Write(tab + ((Label)gvr.FindControl("lblDescription")).Text);
        //            tab = "\t";
        //            Response.Write(tab + ((Label)gvr.FindControl("lblDueDateFrom")).Text);
        //            tab = "\t";
        //            Response.Write(tab + ((Label)gvr.FindControl("lblDueDateTo")).Text);
        //            tab = "\t";
        //            Response.Write("\n");

        //        }
        //    }

        //    Response.End();
        //    gvChecklistDetails.AllowPaging = true;
        //    gvChecklistDetails.AllowSorting = true;
        //    gvChecklistDetails.DataBind();
        //}
        //>>

    }
}