using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Services;
using AjaxControlToolkit;
using System.Collections.Specialized;
using System.Collections.Generic;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code;

namespace Fiction2Fact.Projects.Admin
{
    public partial class MenuMaster : System.Web.UI.Page
    {
        MenuMasBLL objBLL = new MenuMasBLL();
        CommonMethods cm = new CommonMethods();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvSearch.ActiveViewIndex = 0;
                gvBindData();
            }
        }

        protected void onSearchLevelChanged(object sender, EventArgs e)
        {
            writeError("");
            bindParentMenuDropdown("S", ddlSearchMenuLevel.SelectedValue);
        }

        protected void onAddLevelChanged(object sender, EventArgs e)
        {
            writeError("");
            bindParentMenuDropdown("A", ddlMenuLevel.SelectedValue);
        }

        private void bindParentMenuDropdown(string strType, string strLevel)
        {
            int intLevel = 0;
            string strStatus = "";
            DataTable dt = new DataTable();
            try
            {
                bool isLevel = int.TryParse(strLevel, out intLevel);
                intLevel = intLevel - 1;

                strStatus = strType.Equals("A") ? "A" : "";

                dt = objBLL.getParentLevelMenu(intLevel, 0, strStatus);
                if (strType.Equals("S"))
                {
                    ddlSearchParentMenu.DataSource = dt;
                    ddlSearchParentMenu.DataBind();
                    ddlSearchParentMenu.Items.Insert(0, new ListItem("--- Select --", ""));
                }
                else if (strType.Equals("A") || strType.Equals("E"))
                {
                    ddlParentMenu.DataSource = dt;
                    ddlParentMenu.DataBind();
                    ddlParentMenu.Items.Insert(0, new ListItem("--- Select --", ""));

                    if (intLevel.Equals(0))
                        rfvParentMenu.Enabled = false;
                    else
                        rfvParentMenu.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                writeError("Exception in bindParentMenuDropdown(): " + ex.Message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            writeError("");
            gvBindData();
        }

        private void gvBindData()
        {
            DataTable dt = new DataTable();
            int intId = 0, intLevel = 0, intParentId = 0;
            string strMenuName = "", strMenuURL = "", strStatus = "";
            try
            {
                bool isLevel = int.TryParse(ddlSearchMenuLevel.SelectedValue, out intLevel);
                bool isParentId = int.TryParse(ddlSearchParentMenu.SelectedValue, out intParentId);

                strMenuName = txtSearchMenuName.Text;
                strMenuURL = txtSearchMenuURL.Text;
                strStatus = ddlSearchStatus.SelectedValue;

                dt = objBLL.searchMenuDetails(intId, intLevel, intParentId, strMenuName, strMenuURL, strStatus);
                Session["MenuMaster"] = dt;
                gvMenuMas.DataSource = dt;
                gvMenuMas.DataBind();

                int intRowCount = dt.Rows.Count;
                if (intRowCount > 0)
                {
                    btnExportToExcel.Visible = true;

                    LinkButton lnkUp = (gvMenuMas.Rows[0].FindControl("lnkUp") as LinkButton);
                    LinkButton lnkDown = (gvMenuMas.Rows[intRowCount - 1].FindControl("lnkDown") as LinkButton);

                    lnkUp.Visible = false;
                    lnkDown.Visible = false;
                }
                else
                    btnExportToExcel.Visible = false;

                mvSearch.ActiveViewIndex = 0;
            }
            catch (Exception ex)
            {
                writeError("system exception in gvBindData(): " + ex.Message);
            }
        }

        protected void gvMenuMas_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRow dr;
            DataTable dt = new DataTable();
            int intMenuId = 0;
            try
            {
                writeError("");
                bool isMenu = int.TryParse(gvMenuMas.SelectedValue.ToString(), out intMenuId);

                if (hfSelectedOperation.Value == "Edit")
                {
                    dt = objBLL.searchMenuDetails(intMenuId, 0, 0, "", "", "");
                    if (dt.Rows.Count > 0)
                    {
                        mvSearch.ActiveViewIndex = 1;

                        dr = dt.Rows[0];
                        lblID.Text = dr["MM_ID"].ToString();
                        txtMenuName.Text = dr["MM_MENU_NAME"].ToString();
                        ddlMenuLevel.SelectedValue = dr["MM_MENU_LEVEL"].ToString();

                        bindParentMenuDropdown("E", ddlMenuLevel.SelectedValue);

                        if (dr["MM_PARENT_ID"] != DBNull.Value)
                            ddlParentMenu.SelectedValue = dr["MM_PARENT_ID"].ToString();

                        txtMenuURL.Text = dr["MM_HTML"].ToString();
                        txtSortOrder.Text = dr["MM_SORT_ORDER"].ToString();
                        ddlStatus.SelectedValue = dr["MM_STATUS"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                writeError("system exception in selectedIndexChange(): " + ex.Message);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            writeError("");
            mvSearch.ActiveViewIndex = 1;
            lblID.Text = "";
            txtMenuName.Text = "";
            ddlMenuLevel.SelectedIndex = -1;
            ddlParentMenu.SelectedIndex = -1;
            txtMenuURL.Text = "";
            txtSortOrder.Text = "";
            ddlStatus.SelectedIndex = -1;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strMenuName = "", strMenuURL = "", strStatus = "", strCreateBy = "";
            int intId = 0, intLevel = 0, intParentId = 0, intSortOrder = 0, intRetVal = 0;
            try
            {
                bool isId = int.TryParse(lblID.Text, out intId);
                bool isLevel = int.TryParse(ddlMenuLevel.SelectedValue, out intLevel);
                bool isParentId = int.TryParse(ddlParentMenu.SelectedValue, out intParentId);
                bool isSortOrder = int.TryParse(cm.getSanitizedString(txtSortOrder.Text), out intSortOrder);

                strMenuName = cm.getSanitizedString(txtMenuName.Text);
                strMenuURL = cm.getSanitizedString(txtMenuURL.Text);
                strStatus = ddlStatus.SelectedValue;
                strCreateBy = Page.User.Identity.Name.ToString();

                intRetVal = objBLL.saveMenuMasDetails(intId, strMenuName, intLevel, intParentId, intSortOrder,
                                strMenuURL, strStatus, strCreateBy);

                if (intId.Equals(0))
                    writeError("Menu added successfully.");
                else
                    writeError("Menu updated successfully. ");

                gvBindData();
                mvSearch.ActiveViewIndex = 0;
            }
            catch (Exception ex)
            {
                writeError("system exception in btnSave_Click(): " + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            writeError("");
            mvSearch.ActiveViewIndex = 0;
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void ChangeSection(object sender, EventArgs e)
        {
            int intCurrRowIndex = 0, intCurrMenuId = 0, intCurrSortOrder = 0, intPrevNextRowIndex = 0,
                intPrevNextMenuId = 0, intPrevNextSortOrder = 0;
            try
            {
                string commandArgument = (sender as LinkButton).CommandArgument;

                //<< Code to change the position of Current Grid on click of Up or Down
                intCurrRowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
                intCurrMenuId = Convert.ToInt32(gvMenuMas.DataKeys[intCurrRowIndex].Value);
                Label lblSortOrder = (gvMenuMas.Rows[intCurrRowIndex].FindControl("lblSortOrder") as Label);
                bool currSortOrder = int.TryParse(lblSortOrder.Text, out intCurrSortOrder);
                intCurrSortOrder = commandArgument == "up" ? intCurrSortOrder - 1 : intCurrSortOrder + 1;
                //>>

                //<< Code to change the position of up or down row of the current row
                intPrevNextRowIndex = commandArgument == "up" ? intCurrRowIndex - 1 : intCurrRowIndex + 1;
                intPrevNextMenuId = Convert.ToInt32(gvMenuMas.DataKeys[intPrevNextRowIndex].Value);
                Label lblPrevNextSortOrder = (gvMenuMas.Rows[intPrevNextRowIndex].FindControl("lblSortOrder") as Label);
                bool prevnextOrder = int.TryParse(lblPrevNextSortOrder.Text, out intPrevNextSortOrder);
                intPrevNextSortOrder = commandArgument == "up" ? intPrevNextSortOrder + 1 : intPrevNextSortOrder - 1;
                //>>

                objBLL.updateSortOrderforMenu(intCurrMenuId, intCurrSortOrder, intPrevNextMenuId, intPrevNextSortOrder);

                gvBindData();
            }
            catch (Exception ex)
            {
                writeError("system exception in ChangeSection: " + ex.Message);
            }
        }

        protected void gvMenuMas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMenuMas.PageIndex = e.NewPageIndex;
            gvMenuMas.DataSource = (DataTable)Session["MenuMaster"];
            gvMenuMas.DataBind();
        }

        #region //<< Code for Export To Excel
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvMenuMas.AllowPaging = false;
            gvMenuMas.AllowSorting = false;
            gvMenuMas.Columns[1].Visible = false;
            gvMenuMas.DataSource = (DataTable)(Session["MenuMaster"]);
            gvMenuMas.DataBind();
            CommonCodes.PrepareGridViewForExport(gvMenuMas);
            string attachment = "attachment; filename=MenuMaster.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";

            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvMenuMas.RenderControl(htw);

            string html2 = Regex.Replace(sw.ToString(), @"(<input type=""image""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<input class=""checkbox""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<a \/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            Response.Write(html2.ToString());

            //Response.Write(sw.ToString());
            Response.End();
            gvMenuMas.AllowPaging = true;
            gvMenuMas.AllowSorting = true;
            gvMenuMas.DataBind();

        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion

    }
}