using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text.RegularExpressions;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class ListOfReports : System.Web.UI.Page
    {
        UtilitiesBLL utilityBL = new UtilitiesBLL();
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
        string mstrConnectionString = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == true)
            {
                if (!(Session["ListOfReportsSelectCommand"] == null))
                {
                    gvSubmissionMaster.DataSource = (DataTable)Session["ListOfReportsSelectCommand"];
                    gvSubmissionMaster.DataBind();
                }
            }
            else
            {
                ddlSegment.DataSource = utilityBL.getDataset("SUBMISSIONSGMT", mstrConnectionString);
                ddlSegment.DataBind();

                if (Request.QueryString["Type"] != null)
                {
                    hfType.Value = Request.QueryString["Type"].ToString();
                }

                if (Request.QueryString["CircId"] != null)
                {
                    hfCircId.Value = Request.QueryString["CircId"].ToString();
                }

                bool flag = false;

                if ((hfType.Value.Equals("RR") && (hfCircId.Value.Equals("") || hfCircId.Value.Equals("0"))) ||
                    (hfType.Value.Equals("CIRC") && (!hfCircId.Value.Equals("") && !hfCircId.Value.Equals("0"))))
                {
                    flag = true;
                }

                if (!flag)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Script", "alert('Invalid Type'); window.location.href = '" + Global.site_url() + "';", true);
                }

                //<<Modified by Ashish Mishra on 10Jul2017
                //<<Modified by Amey Karangutkar on 12-Jul-2018
                //<< Added Filing_View_Only
                if (User.IsInRole("FilingAdmin") || User.IsInRole("Filing_View_Only") || User.IsInRole("Filing_Sub_Admin") ||
                    (hfType.Value.Equals("CIRC") && (!hfCircId.Value.Equals("") && !hfCircId.Value.Equals("0"))))
                {
                    hfUserType.Value = "Admin";
                    ddlSubType.DataSource = utilityBL.getDataset("SUBTYPE", mstrConnectionString);
                    ddlSubType.DataBind();
                }
                else if (User.IsInRole("FilingUser"))
                {
                    hfUserType.Value = "User";
                    ddlSubType.DataSource = utilityBL.getDatasetWithConditionInString("SUBTYPE", Page.User.Identity.Name.ToString(), mstrConnectionString);
                    ddlSubType.DataBind();
                }
                //>>
                //ddlSubType.DataSource = utilityBL.getDataset("SUBTYPE", mstrConnectionString);
                //ddlSubType.DataBind();

                ddlEventForSearch.DataSource = utilityBL.getDataset("EVENT", mstrConnectionString);
                ddlEventForSearch.DataBind();
                ddlReportDept.DataSource = utilityBL.getDataset("REPORTINGDEPT", mstrConnectionString);
                ddlReportDept.DataBind();

                if (hfType.Value.Equals("CIRC") && flag)
                {
                    btnSearch_Click(sender, e);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            DataTable dtListOfReports = new DataTable();
            string strFrequency = ddlFrequency.SelectedValue;
            string strSegment = ddlSegment.SelectedValue;
            string strSubType = ddlType.SelectedValue;
            string strDeptType = ddlSubType.SelectedValue;
            string strEvent = ddlEventForSearch.SelectedValue;
            string strStatus = ddlStatus.SelectedValue;
            string strReportingFunction = ddlReportDept.SelectedValue;
            string strEventAgenda = "";

            for (int i = 0; i < cblAssociatedWith.Items.Count; i++)
            {
                ListItem li = cblAssociatedWith.Items[i];
                if (li.Selected)
                {
                    if (strEventAgenda == "")
                        strEventAgenda = li.Value;
                    else
                        strEventAgenda = strEventAgenda + ", " + li.Value;
                }
            }

            dtListOfReports = SubmissionMasterBLL.getListOfReports(0, strReportingFunction, strFrequency, strStatus, strSegment,
                strSubType, strDeptType, strEvent, strEventAgenda, Page.User.Identity.Name.ToString(), hfUserType.Value.ToString(),
                hfCircId.Value, txtGlobalSearch.Text, mstrConnectionString);

            Session["ListOfReportsSelectCommand"] = dtListOfReports;
            gvSubmissionMaster.DataSource = dtListOfReports;
            gvSubmissionMaster.DataBind();
            if (gvSubmissionMaster.Rows.Count == 0)
            {
                writeError("No Record Found satisfying the criteria.");
            }
            else
            {
                hideError();
                btnExportToExcel.Visible = true;
            }
        }

        protected string LoadSubmissionSegmentName(object SubmissionID)
        {
            DataTable dtSegmentName;
            string strSegmentName = null;
            string strName = null;

            dtSegmentName = utilityBL.getDatasetWithCondition("LOADSUBSEGMENTS", Convert.ToInt32(SubmissionID), mstrConnectionString);
            for (int i = 0; i <= dtSegmentName.Rows.Count - 1; i++)
            {
                strName = dtSegmentName.Rows[i]["SSM_NAME"].ToString();

                if (strSegmentName != null)
                {
                    strSegmentName = strSegmentName + ", " + strName;
                }
                else
                {
                    strSegmentName = strName;
                }
            }
            return strSegmentName;
        }


        private void writeError(string strError)
        {
            lblInfo.Text = strError;
            lblInfo.Visible = true;
        }

        private void hideError()
        {
            lblInfo.Text = "";
            lblInfo.Visible = false;
        }


        protected void ddlEventForSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblInfo.Text = "";
            cblAssociatedWith.Items.Clear();
            if (!ddlEventForSearch.SelectedValue.Equals(""))
            {
                cblAssociatedWith.DataSource = utilityBL.getDatasetWithCondition("EVENTPURPOSE", Convert.ToInt32(ddlEventForSearch.SelectedValue), mstrConnectionString);
                cblAssociatedWith.DataBind();
            }
        }

        protected void gvSubmissionMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSubmissionMaster.PageIndex = e.NewPageIndex;
            gvSubmissionMaster.DataSource = (DataTable)(Session["ListOfReportsSelectCommand"]);
            gvSubmissionMaster.DataBind();
        }
        protected void lnkReset_Click(object sender, EventArgs e)
        {
            Session["ListOfReportsSelectCommand"] = null;
            Response.Redirect(Request.RawUrl, false);
        }
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvSubmissionMaster.AllowPaging = false;
            gvSubmissionMaster.AllowSorting = false;
            gvSubmissionMaster.Columns[0].Visible = false;
            gvSubmissionMaster.DataSource = (DataTable)Session["ListOfReportsSelectCommand"];
            gvSubmissionMaster.DataBind();
            CommonCodes.PrepareGridViewForExport(gvSubmissionMaster);
            string attachment = "attachment; filename=ListOfReports.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gvSubmissionMaster.RenderControl(htw);
            string html2 = Regex.Replace(sw.ToString(), @"(<input type=""image""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<input class=""checkbox""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<a \/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            Response.Write(html2.ToString());
            //Response.Write(sw.ToString());
            Response.End();
            gvSubmissionMaster.Columns[0].Visible = true;
            gvSubmissionMaster.AllowPaging = true;
            gvSubmissionMaster.AllowSorting = true;
            gvSubmissionMaster.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        #region "Gridview Sorting"
        protected void gvSubmissionMaster_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (Session["ListOfReportsSelectCommand"] != null)
            {
                DataTable dt = (DataTable)(Session["ListOfReportsSelectCommand"]);
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
                gvSubmissionMaster.DataSource = dvDataView;
                gvSubmissionMaster.DataBind();
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
                foreach (DataControlField field in gvSubmissionMaster.Columns)
                {
                    if (field.SortExpression.Equals(strSortExpression))
                    {
                        return gvSubmissionMaster.Columns.IndexOf(field);
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

        #region Commented Code
        //protected void btnExportToExcel_Click(object sender, EventArgs e)
        //{
        //    gvSubmissionMaster.AllowPaging = false;
        //    gvSubmissionMaster.AllowSorting = false;
        //    gvSubmissionMaster.DataBind();
        //    string attachment = "attachment; filename=ListOfReports.xls";

        //    Response.ClearContent();

        //    Response.AddHeader("content-disposition", attachment);

        //    Response.ContentType = "application/vnd.ms-excel";

        //    string tab = "";

        //    Response.Write("<table border=\"1\"> <tr>");
        //    for (int i = 1; i <= gvSubmissionMaster.Columns.Count - 1; i++)
        //    {
        //        Response.Write("<td><b>" + gvSubmissionMaster.Columns[i].HeaderText);
        //        Response.Write("</b></td>");
        //    }

        //    Response.Write("</tr>");

        //    foreach (GridViewRow gvr in gvSubmissionMaster.Rows)
        //    {
        //        if (gvr.RowType == DataControlRowType.DataRow)
        //        {
        //            Response.Write("<tr>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblSrNo")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblSmId")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblstmType")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblReportFunc")).Text + "</td>");

        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblEvent")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblAgenda")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblParticulars")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblDescription")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblEffectiveDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblPriority")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblSegment")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblFrequency")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblToBeEscalated")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblLevel1EscDays")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblLevel2EscDays")).Text + "</td>");

        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblOnlyOnceFromDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblOnlyOnceToDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblAnnualFromDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblAnnualToDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblMonthlyFromDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblMonthlyToDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblQ1FromDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblQ1ToDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblQ2FromDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblQ2ToDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblQ3FromDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblQ3ToDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblQ4FromDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblQ4ToDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblHY1FromDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblHY1ToDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblHY2FromDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblHY2ToDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblWeeklyFromDate")).Text + "</td>");
        //            Response.Write("<td>" + ((Label)gvr.FindControl("lblWeeklyToDate")).Text + "</td>");

        //            Response.Write("</tr>");
        //        }
        //    }

        //    Response.Write("</table>");
        //    Response.End();
        //    gvSubmissionMaster.AllowPaging = true;
        //    gvSubmissionMaster.AllowSorting = true;
        //    gvSubmissionMaster.DataBind();
        //}
        #endregion
    }
}