using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class CommonMasters_MailConfigMas : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        MailConfigBLL mcBL = new MailConfigBLL();
        CommonMethods cm = new CommonMethods();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvMailConfig.ActiveViewIndex = 0;
                //ddlConfigType.DataSource = rcBL.getRefCodeDetails(strRCTypeSubmission, mstrConnectionString);
                //ddlConfigType.DataBind();
                //ddlConfigType.Items.Insert(0, new ListItem("(Select an option)", ""));
                //ddlConfigType.Focus();
            }
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                lblMsg.Text = "";
                int intMCId = 0;
                string strType;
                string strFrom = "";
                string strTo = "";
                string strCC = "";
                string strBCC = "";
                string strSubject;
                string strContent;
                string strCreateBy;
                string strModuleName = "";
                string strStatus = "";
                int intMCRowId;
                string strMCId;

                strMCId = lblID.Text.ToString();
                if (!strMCId.Equals(""))
                    intMCId = Convert.ToInt32(strMCId);

                //strType = ddlType.SelectedValue;
                strType = cm.getSanitizedString(txtType.Text);
                //strFrom = txtFrom.Text;
                //strTo = txtTo.Text;
                //strCC = txtCC.Text;
                //strBCC = txtBCC.Text;
                strSubject = cm.getSanitizedString(txtSubject.Text);
                //strContent = txtContent.Text;
                strContent = FCKContent.Text;
                strCreateBy = Convert.ToString(Page.User.Identity.Name);

                intMCRowId = mcBL.saveMailConfigDetails(intMCId, strType, strFrom, strTo, strCC, strBCC, strStatus, strSubject, strContent, strCreateBy, strModuleName, mstrConnectionString);

                updateMailConfigGrid();
                writeError("Record Saved Successfully with Id: " + intMCRowId.ToString());
                mvMailConfig.ActiveViewIndex = 0;
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in btnSave_Click :" + exp);
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            updateMailConfigGrid();
            //ddlConfigType.Focus();
        }

        private void updateMailConfigGrid()
        {
            try
            {
                lblMsg.Text = "";
                lblID.Text = "";
                DataTable dtMailConfig;
                string strMCId = cm.getSanitizedString(txtSearchMailConfigId.Text);
                string strType = cm.getSanitizedString(txtConfigType.Text);//ddlConfigType.SelectedValue;

                dtMailConfig = mcBL.searchMailConfig(strMCId, strType);
                Session["MailConfig"] = dtMailConfig;
                gvMailConfig.DataSource = dtMailConfig;
                gvMailConfig.DataBind();

                if (gvMailConfig.Rows.Count == 0)
                {
                    writeError("No Records Found Satisfying this Criteria.");

                }
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in updateMailConfig()" + exp);
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
                    MailConfigDDL();
                    strMCId = gvMailConfig.SelectedValue.ToString();
                    dtMailConfig = mcBL.searchMailConfig(strMCId);

                    dr = dtMailConfig.Rows[0];
                    mvMailConfig.ActiveViewIndex = 1;

                    lblID.Text = dr["MCM_ID"].ToString();
                    //ddlType.SelectedValue = dr["MCM_TYPE"].ToString();
                    txtType.Text = dr["MCM_TYPE"].ToString();
                    //txtFrom.Text = dr["MCM_FROM"].ToString();
                    //txtTo.Text = dr["MCM_TO"].ToString();
                    //txtCC.Text = dr["MCM_CC"].ToString();
                    //txtBCC.Text = dr["MCM_BCC"].ToString();
                    txtSubject.Text = dr["MCM_SUBJECT"].ToString();
                    //txtContent.Text = dr["MCM_CONTENT"].ToString();
                    FCKContent.Text = dr["MCM_CONTENT"].ToString();
                }
                if (hfSelectedOperation.Value == "Delete")
                {
                    try
                    {
                        int intMCId = Convert.ToInt32(gvMailConfig.SelectedDataKey.Value);
                        string strCreateBy = Convert.ToString(Page.User.Identity.Name);
                        DataTable dt = new DataTable();

                        mcBL.deleteMailConfig(intMCId, strCreateBy, mstrConnectionString);
                        updateMailConfigGrid();
                        writeError("Mail Config Deleted Successfully.");
                    }
                    catch (Exception exp)
                    {
                        writeError("Exception in gvMailConfig_SelectedIndexChanged()" + exp.Message);
                        throw;
                    }
                }
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
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
            lblID.Text = "";
            MailConfigDDL();
            //ddlType.SelectedValue = null;
            //txtFrom.Text = "";
            //txtTo.Text = "";
            //txtCC.Text = "";
            //txtBCC.Text = "";
            txtSubject.Text = "";
            //txtContent.Text = "";
            FCKContent.Text = "";
            //ddlType.Focus();
        }

        private void MailConfigDDL()
        {
            //ddlType.DataSource = rcBL.getRefCodeDetails(strRCTypeSubmission, mstrConnectionString);
            //ddlType.DataBind();
            //ddlType.Items.Insert(0, new ListItem("(Select an option)", ""));
        }

        protected void gvMailConfig_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMailConfig.PageIndex = e.NewPageIndex;
            gvMailConfig.DataSource = (DataTable)(Session["MailConfig"]);
            gvMailConfig.DataBind();
        }



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



    }
}