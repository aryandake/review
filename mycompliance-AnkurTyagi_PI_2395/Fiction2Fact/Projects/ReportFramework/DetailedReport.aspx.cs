using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Reflection;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Reports
{
    public partial class ReportFramework_DetailedReport : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        ReportsBLL ReportBLL = new ReportsBLL();

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
                if (Request.QueryString["ReportType"].ToString() != null)
                    hfReportType.Value = Request.QueryString["ReportType"].ToString();

                if (Request.QueryString["DateofReport"].ToString() != null)
                    hfDateofReport.Value = Request.QueryString["DateofReport"].ToString();

                if (Request.QueryString["Filter1"].ToString() != null)
                    hfFilter1.Value = Request.QueryString["Filter1"].ToString();

                if (Request.QueryString["Filter2"].ToString() != null)
                    hfFilter2.Value = Request.QueryString["Filter2"].ToString();

                if (Request.QueryString["Filter3"].ToString() != null)
                    hfFilter3.Value = Request.QueryString["Filter3"].ToString();

                if (Request.QueryString["X"].ToString() != null)
                    hfX.Value = Request.QueryString["X"].ToString();

                if (Request.QueryString["Y"].ToString() != null)
                    hfY.Value = Request.QueryString["Y"].ToString();

                getDetailedReport(hfReportType.Value.ToString(), hfDateofReport.Value.ToString(), hfFilter1.Value.ToString(), hfFilter2.Value.ToString(),
                    hfFilter3.Value.ToString(), hfX.Value.ToString(), hfY.Value.ToString());
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page != null)
            {
                Page.RegisterRequiresViewStateEncryption();
            }
        }


        private void getDetailedReport(string strReportTypeId, string strAsOnDate, string strFilter1, string strFilter2, string strFilter3,
            string strXAxis, string strYAxis)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = ReportBLL.getDetailedReportData(strReportTypeId, strAsOnDate, strFilter1, strFilter2, strFilter3, strXAxis,
                    strYAxis, mstrConnectionString);

                if (dt.Rows.Count > 0)
                {
                    gvRRDetailedData.DataSource = dt;
                    gvRRDetailedData.DataBind();
                    Session["DetailedReport"] = dt;
                    btnExportToExcel.Visible = true;

                    lblEmptyRowMessage.Text = "";
                }
                else
                {
                    lblEmptyRowMessage.Text = "No Records Found...";
                    btnExportToExcel.Visible = false;
                }
            }
            catch (Exception ex)
            {
                writeError("Exception in " + MethodBase.GetCurrentMethod().Name + "(): " + ex.Message);
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvRRDetailedData.AllowPaging = false;
            gvRRDetailedData.AllowSorting = false;
            gvRRDetailedData.DataSource = (Session["DetailedReport"]);
            gvRRDetailedData.DataBind();
            CommonCodes.PrepareGridViewForExport(gvRRDetailedData);
            string attachment = "attachment; filename=DetailedReport.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gvRRDetailedData.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }
    }
}