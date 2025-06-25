using Fiction2Fact.Legacy_App_Code.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using Fiction2Fact.Legacy_App_Code;
using DocumentFormat.OpenXml.Vml;
using Fiction2Fact.App_Code;
using System.Text;
using System.Data;
using DocumentFormat.OpenXml.Bibliography;
using System.Net.NetworkInformation;
using Fiction2Fact.Legacy_App_Code.DAL;
using Fiction2Fact.Legacy_App_Code.VO;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Fiction2Fact.Projects.Compliance
{
    public partial class SearchDataRequirement : System.Web.UI.Page
    {
        string script = "";
        int intCnt = 0;
        ComplianceReviewBLL drqmBLL = new ComplianceReviewBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.User.Identity.Name.ToString().Equals(""))
            {
                Response.Redirect(Global.site_url("Login.aspx"));
            }

            if (!IsPostBack)
            {
                Session["strMidArray"] = "";
                string strFilters = "";
                string[] arrVal = null;
                StringBuilder sbQuery = new StringBuilder();

                if (Request.QueryString["Type"] != null)
                    hfType.Value = Request.QueryString["Type"].ToString();
                else
                    hfType.Value = "";
                if (Request.QueryString["SrcType"] != null)
                    hfSrcType.Value = Request.QueryString["SrcType"].ToString();
                else
                    hfSrcType.Value = "";

                if (Request.QueryString["Src"] != null)
                    hfSrc.Value = Request.QueryString["Src"].ToString();
                else
                    hfSrc.Value = "";

                if (Request.QueryString["RefId"] != null)
                    hfRefId.Value = Request.QueryString["RefId"].ToString();
                else
                    hfRefId.Value = "0";

                if (Request.QueryString["Source"] != null)
                    hfSource.Value = Request.QueryString["Source"].ToString();
                else
                    hfSource.Value = "";


                gvDRQM.DataSource = drqmBLL.getDRQMDetails(0, 0, null, 0, Page.User.Identity.Name, strValue: " and CDQ_STATUS='O'");
                gvDRQM.DataBind();


            }

            script += "\r\n <script type=\"text/javascript\">\r\n";
            //script += " isResponseDrafted();\r\n";
            script += "</script>\r\n";
            ClientScript.RegisterStartupScript(this.GetType(), "", script);
        }


        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }

        protected DataTable LoadDRQMFileList(object Id)
        {
            return drqmBLL.getDRQMFiles(0, null, Convert.ToInt32(Id), null);
            hfDoubleClickFlag.Value = "";
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        private void getDRQMDetails()
        {
            DataTable dt = drqmBLL.getDRQMDetails(0, 0, null, 0, Page.User.Identity.Name);
            Session["DRQMResponse"] = dt;
            gvDRQM.DataSource = dt;
            gvDRQM.DataBind();
            hfDoubleClickFlag.Value = "";
        }

        protected void gvDRQM_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvDRQM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strId = gvDRQM.SelectedValue.ToString();
                if (hfSelectedOperation.Value == "View")
                {
                    DataTable dt = new DataTable();
                    dt = drqmBLL.getDRQMDetails(Convert.ToInt32(strId), 0, null, 0, null);
                    if (dt.Rows.Count > 0)
                    {
                        string strcrid = dt.Rows[0]["CDQ_SOURCE_ID"].ToString();
                        Response.Redirect(Global.site_url("Projects/ComplianceReview/ViewComplianceReview.aspx?Source=DRQ&Id=" + strcrid));
                    }
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {

        }
    }
}