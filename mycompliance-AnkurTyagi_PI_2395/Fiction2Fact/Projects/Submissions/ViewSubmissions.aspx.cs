using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using System.IO;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class ViewSubmissions : System.Web.UI.Page
    {
        UtilitiesBLL utilityBL = new UtilitiesBLL();
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
        string mstrConnectionString = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == true)
            {
                if (!(Session["SubmissionSelectCommand"] == null))
                {
                    gvSubmissionMaster.DataSource = (DataSet)Session["SubmissionSelectCommand"];
                    gvSubmissionMaster.DataBind();
                }
            }
            else
            {

                ddlSegment.DataSource = utilityBL.getDataset("SUBMISSIONSGMT", mstrConnectionString);
                ddlSegment.DataBind();

                //<<Modified by Ashish Mishra on 10Jul2017
                if (User.IsInRole("FilingAdmin"))
                {
                    hfUserType.Value = "Admin";
                    ddlSubType.DataSource = utilityBL.getDataset("SUBTYPE", mstrConnectionString);
                    ddlSubType.DataBind();
                }
                else if (User.IsInRole("Filing_Sub_Admin"))
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
                mvMultiView.ActiveViewIndex = 0;
                Session["EditSubmission"] = Server.UrlEncode(System.DateTime.Now.ToString());
                if (Request.QueryString["OpType"].ToString() != null)
                {
                    int intType;
                    intType = Convert.ToInt32(Request.QueryString["OpType"].ToString());
                    hfOpType.Value = Request.QueryString["OpType"].ToString();
                }
            }

        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            DataSet dsSearchSubmissions = new DataSet();
            string strFrequency = ddlFrequency.SelectedValue;
            string strSegment = ddlSegment.SelectedValue;
            string strSubType = ddlType.SelectedValue;
            string strDeptType = ddlSubType.SelectedValue;
            string strEvent = ddlEventForSearch.SelectedValue;
            string strStatus = ddlStatus.SelectedValue;
            string strReportingFunction = ddlReportDept.SelectedValue;
            Boolean blnEventAgendaSelected = false;
            string strFilterEvent = "";

            for (int i = 0; i < cblAssociatedWith.Items.Count; i++)
            {
                ListItem li = cblAssociatedWith.Items[i];
                if (li.Selected)
                {
                    blnEventAgendaSelected = true;
                    if (strFilterEvent == "")
                        strFilterEvent = " and (" + strFilterEvent + " SM_EP_ID = " + li.Value;
                    else
                        strFilterEvent = strFilterEvent + " OR SM_EP_ID = " + li.Value;
                }
            }
            if (blnEventAgendaSelected)
            {
                strFilterEvent = strFilterEvent + ") order by SM_EP_ID ";
            }
            dsSearchSubmissions = SubmissionMasterBLL.SearchSubmissions(0, strReportingFunction, strFrequency, strStatus, strSegment, strSubType,
                                                                       strDeptType, strEvent, strFilterEvent,
                                                                       Page.User.Identity.Name.ToString(), hfUserType.Value.ToString(),
                                                                       mstrConnectionString);

            Session["SubmissionSelectCommand"] = dsSearchSubmissions;
            gvSubmissionMaster.DataSource = dsSearchSubmissions;
            gvSubmissionMaster.DataBind();
            if (gvSubmissionMaster.Rows.Count == 0)
            {
                writeError("No Record Found satisfying the criteria.");
            }
            else
            {
                btnExportToExcel.Visible = true;
                hideError();
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
            gvSubmissionMaster.DataSource = (DataSet)(Session["SubmissionSelectCommand"]);
            gvSubmissionMaster.DataBind();
        }

        protected void gvSubmissionMaster_Sorting(object sender, GridViewSortEventArgs e)
        {

            if (!(Session["SubmissionSelectCommand"] == null))
            {


                DataSet ds = new DataSet();
                ds = (DataSet)Session["SubmissionSelectCommand"];
                DataView dvDataView = new DataView(ds.Tables[0]);
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
                gvSubmissionMaster.DataSource = dvDataView;
                gvSubmissionMaster.DataBind();
            }
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

        protected void gvSubmissionMaster_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                LinkButton ibView;
                ibView = ((LinkButton)(e.Row.FindControl("lnkEdit")));
                ibView.PostBackUrl = ("./EditSubmissions.aspx?SubmissionId=" + ((Label)(e.Row.FindControl("lbSmId"))).Text + "&OpType=" + hfOpType.Value);
            }
        }

        //<<Added by rahuldeb on 15Jun2017
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvSubmissionMaster.DataSource = (DataSet)(Session["SubmissionSelectCommand"]);
            gvSubmissionMaster.DataBind();
            gvSubmissionMaster.Columns[1].Visible = false;
            gvSubmissionMaster.Columns[2].Visible = false;
            gvSubmissionMaster.Columns[3].Visible = false;
            gvSubmissionMaster.Columns[4].Visible = false;
            gvSubmissionMaster.Columns[14].Visible = false;
            //>>
            gvSubmissionMaster.AllowPaging = false;
            gvSubmissionMaster.AllowSorting = false;
            //gvSubmissionMaster.DataSource = mdtActivityDetails;
            gvSubmissionMaster.DataBind();
            CommonCodes.PrepareGridViewForExport(gvSubmissionMaster);
            string attachment = "attachment; filename=Submissions.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gvSubmissionMaster.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
            gvSubmissionMaster.AllowPaging = true;
            gvSubmissionMaster.AllowSorting = true;
            gvSubmissionMaster.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

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