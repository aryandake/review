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

namespace Fiction2Fact.Projects.Compliance
{
    public partial class ViewDataRequirement : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        RefCodesBLL refBL = new RefCodesBLL();
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();

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
                if (Request.QueryString["DRId"] != null)
                    hfDRId.Value = Request.QueryString["DRId"].ToString();
                else
                    hfDRId.Value = "0";

                if (Request.QueryString["Type"] != null)
                    hfType.Value = Request.QueryString["Type"].ToString();
                else
                    hfType.Value = "";

                if (Request.QueryString["User"] != null)
                    hfUser.Value = Request.QueryString["User"].ToString();
                else
                    hfUser.Value = "";

                if (Request.QueryString["Source"] != null)
                    hfUserType.Value = Request.QueryString["Source"].ToString();
                else
                    hfUserType.Value = "";

                if (Request.QueryString["Src"] != null)
                    hfSource.Value = Request.QueryString["Src"].ToString();
                else
                    hfSource.Value = "";

                getDataRequirementDetails(hfDRId.Value);

                if (hfType.Value.Equals("View"))
                {
                    gvResponse.DataSource = oBLL.getDRQMResponse(Convert.ToInt32(hfDRId.Value), 0, null, null);
                    gvResponse.DataBind();

                    if (hfUser.Value.Equals("RM"))
                        gvResponse.Columns[4].Visible = false;
                }
                else if (hfType.Value.Equals("ViewSent"))
                {
                    gvResponse.DataSource = oBLL.getDRQMResponse(Convert.ToInt32(hfDRId.Value), 0, null, null);
                    gvResponse.DataBind();

                    gvResponse.Columns[4].Visible = false;
                }
            }
        }



        private void getDataRequirementDetails(string strDRId)
        {
            int intDRId = 0;
            bool res = false;
            DataTable dtDRQM = new DataTable();
            DataRow dr;

            try
            {
                res = int.TryParse(strDRId, out intDRId);
                dtDRQM = oBLL.getDRQMDetails(intDRId,0, "",0, null);

                if (dtDRQM.Rows.Count > 0)
                {
                    dr = dtDRQM.Rows[0];
                    lblUnit.Text = dr["CSFM_NAME"].ToString();
                    lblDRQ.Text = dr["CDQ_QUERY_DATA_REQUIREMENT"].ToString().Replace(Environment.NewLine, "<br />");
                    lblPersonRes.Text = dr["CDQ_PERSON_RESPONSIBLE"].ToString();
                    lblRaisedDate.Text = (dr["CDQ_RAISED_DT"] is DBNull ? "" : Convert.ToDateTime(dr["CDQ_RAISED_DT"]).ToString("dd-MMM-yyyy HH:mm:ss"));
                    lblDueDate.Text = (dr["CDQ_EXPIRY_DT"] is DBNull ? "" : Convert.ToDateTime(dr["CDQ_EXPIRY_DT"]).ToString("dd-MMM-yyyy"));
                    lblStatus.Text = dr["Status"].ToString();
                    lblQueryPendingWith.Text = dr["Query pending with"].ToString();
                    lblAgeing.Text = dr["Ageing"].ToString();
                    lblType.Text = dr["CDQ_Type"].ToString();

                    gvAttachments.DataSource = LoadDRQMFileList(intDRId);
                    gvAttachments.DataBind();

                    lblClosedBy.Text = dr["CDQ_CLOSED_BY"].ToString();
                    lblClosedOn.Text = (dr["CDQ_CLOSED_ON"] is DBNull ? "" : Convert.ToDateTime(dr["CDQ_CLOSED_ON"]).ToString("dd-MMM-yyyy HH:mm:ss"));
                    lblClosureRemarks.Text = dr["CDQ_CLOSURE_REMARKS"].ToString().Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }

        protected DataTable LoadDRQMFileList(object Id)
        {
            DataTable dtAttach = new DataTable();
            dtAttach = oBLL.getDRQMFiles(0,null,Convert.ToInt32(Id), "DRQMFile");
            return dtAttach;
        }

        protected DataTable LoadDRQMResponseFileList(object Id)
        {
            DataTable dtAttach = new DataTable();
            dtAttach = oBLL.getDRQMFiles(0, null, Convert.ToInt32(Id), "DRQMResponseFile");
            return dtAttach;
        }
    }
}