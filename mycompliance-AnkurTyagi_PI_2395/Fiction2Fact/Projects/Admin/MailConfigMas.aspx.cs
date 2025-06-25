using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Admin
{
    public partial class CommonMasters_MailConfigMas : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        MailConfigBLL mcBL = new MailConfigBLL();
        RefCodesBLL rcBL = new RefCodesBLL();
        CommonMethods cm = new CommonMethods();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvMailConfig.ActiveViewIndex = 0;

                ddlSearchModuleName.DataSource = rcBL.getRefCodeDetails("Module Name", mstrConnectionString);
                ddlSearchModuleName.DataBind();
                ddlSearchModuleName.Items.Insert(0, new ListItem("(Select an option)", ""));

                txtMailConfigId.Focus();
            }
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            int intMCId = 0, intMCRowId = 0;
            string strMCId = "", strType = "", strStatus = "", strFrom = "", strTo = "", strCC = "", strBCC = "", strSubject = "", strContent = "", strCreateBy = "",
                strModuleName = "";
            try
            {
                lblMsg.Text = "";

                strMCId = lblID.Text.ToString();
                if (!strMCId.Equals(""))
                    intMCId = Convert.ToInt32(strMCId);

                strType = cm.getSanitizedString(txtDesc.Text.ToString());
                strSubject = cm.getSanitizedString(txtSubject.Text);
                strStatus = ddlStatus.SelectedValue;
                string strFontName = ConfigurationManager.AppSettings["FontName"].ToString();
                strContent = "<div style=\" font-family: " + strFontName + "; font-size: 11pt\">" + FCKContent.Text + "</div>";
                strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);

                strModuleName = ddlModuleName.SelectedValue;

                intMCRowId = mcBL.saveMailConfigDetails(intMCId, strType, strFrom, strTo, strCC, strBCC, strStatus, strSubject, strContent, strCreateBy, strModuleName, mstrConnectionString);

                hfDoubleClickFlag.Value = "";
                updateMailConfigGrid();
                writeError("Record saved successfully.");
                mvMailConfig.ActiveViewIndex = 0;
            }
            catch (Exception exp)
            {
                hfDoubleClickFlag.Value = "";
                writeError("Exception in btnSave_Click :" + exp);
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            updateMailConfigGrid();
            txtMailConfigId.Focus();
        }

        private void updateMailConfigGrid()
        {
            DataTable dtMailConfig;
            string strMCId = "", strType = "", strModuleName = "";
            try
            {
                lblMsg.Text = "";
                lblID.Text = "";
                string tst = GetDotNetVersion.Get45PlusFromRegistry();
                try
                {
                    strMCId = Convert.ToInt32(txtMailConfigId.Text).ToString();
                }
                catch (Exception) { }
                strType = cm.getSanitizedString(txtConfigType.Text.ToString());//ddlConfigType.SelectedValue;
                strModuleName = ddlSearchModuleName.SelectedValue;

                dtMailConfig = mcBL.searchMailConfig(strMCId, strType, strModuleName, mstrConnectionString);
                Session["MailConfig"] = dtMailConfig;
                gvMailConfig.DataSource = dtMailConfig;
                gvMailConfig.DataBind();

                if (gvMailConfig.Rows.Count == 0)
                {
                    imgExcel.Visible = false;
                }
                else
                {
                    imgExcel.Visible = true;
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

        protected void gvMailConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strMCId;
                DataTable dtMailConfig = new DataTable();
                DataRow dr;
                lblMsg.Text = "";

                if (hfSelectedOperation.Value == "Edit")
                {
                    lblID.Visible = true;
                    strMCId = gvMailConfig.SelectedValue.ToString();
                    dtMailConfig = mcBL.searchMailConfig(strMCId, "", "", mstrConnectionString);

                    if (dtMailConfig.Rows.Count > 0)
                    {
                        ddlModuleName.DataSource = rcBL.getRefCodeDetails("Module Name", mstrConnectionString);
                        ddlModuleName.DataBind();
                        ddlModuleName.Items.Insert(0, new ListItem("(Select an option)", ""));

                        dr = dtMailConfig.Rows[0];
                        mvMailConfig.ActiveViewIndex = 1;

                        lblID.Text = dr["MCM_ID"].ToString();

                        ddlModuleName.SelectedValue = dr["MCM_MODULE_NAME"].ToString();

                        txtDesc.Text = dr["MCM_TYPE"].ToString();
                        txtSubject.Text = dr["MCM_SUBJECT"].ToString();
                        ddlStatus.SelectedValue = dr["MCM_REC_STATUS"].ToString();
                        FCKContent.Text = dr["MCM_CONTENT"].ToString();
                    }
                }
                if (hfSelectedOperation.Value == "Delete")
                {
                    try
                    {
                        int intMCId = Convert.ToInt32(gvMailConfig.SelectedDataKey.Value);
                        string strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                        DataTable dt = new DataTable();

                        mcBL.deleteMailConfig(intMCId, strCreateBy, mstrConnectionString);
                        updateMailConfigGrid();
                        writeError("Mail config deleted successfully.");
                    }
                    catch (Exception exp)
                    {
                        writeError("Exception in gvMailConfig_SelectedIndexChanged(): " + exp.Message);
                    }
                }
            }
            catch (Exception exp)
            {
                writeError("Exception in SelectedIndexChange()" + exp);
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            mvMailConfig.ActiveViewIndex = 0;
        }

        protected void btnAddMailConfig_Click(object sender, System.EventArgs e)
        {
            lblMsg.Text = "";
            mvMailConfig.ActiveViewIndex = 1;

            ddlModuleName.DataSource = rcBL.getRefCodeDetails("Module Name", mstrConnectionString);
            ddlModuleName.DataBind();
            ddlModuleName.Items.Insert(0, new ListItem("(Select an option)", ""));

            lblID.Text = "";
            ddlModuleName.SelectedIndex = -1;
            txtDesc.Text = "";
            ddlStatus.SelectedIndex = -1;
            txtSubject.Text = "";
            FCKContent.Text = "";
            txtDesc.Focus();
        }

        protected void gvMailConfig_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMailConfig.PageIndex = e.NewPageIndex;
            gvMailConfig.DataSource = (DataTable)(Session["MailConfig"]);
            gvMailConfig.DataBind();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvMailConfig.AllowPaging = false;
            gvMailConfig.AllowSorting = false;
            gvMailConfig.DataSource = (DataTable)(Session["MailConfig"]);
            gvMailConfig.DataBind();
            CommonCodes.PrepareGridViewForExport(gvMailConfig);
            string attachment = "attachment; filename=MailConfigDetails.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvMailConfig.RenderControl(htw);

            string html2 = Regex.Replace(sw.ToString(), @"(<input type=""image""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<input class=""checkbox""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<a \/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<p/>)", @"<p style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<div >)", @"<div style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            Response.Write(html2.ToString());

            //Response.Write(sw.ToString());
            Response.End();
            gvMailConfig.AllowPaging = true;
            gvMailConfig.AllowSorting = true;
            gvMailConfig.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        #region Code for Sorting
        protected void gvMailConfig_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (!(Session["MailConfig"] == null))
            {
                DataSet ds = new DataSet();
                ds = (DataSet)Session["MailConfig"];
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
                gvMailConfig.DataSource = dvDataView;
                gvMailConfig.DataBind();
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
        #endregion

    }
}