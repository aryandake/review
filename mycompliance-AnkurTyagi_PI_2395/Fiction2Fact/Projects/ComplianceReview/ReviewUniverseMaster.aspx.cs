using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using Fiction2Fact.Legacy_App_Code.Compliance;
using DocumentFormat.OpenXml.Drawing;

namespace Fiction2Fact.Projects.Compliance
{
    public partial class ReviewUniverseMaster : System.Web.UI.Page
    {
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillGrid();
                mvUniverseMaster.ActiveViewIndex = 0;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string struniverse = txtAreaUniverse.Text;
            string strstatus = ddlStatus.SelectedItem.Value;
            string strloggeduser = Page.User.Identity.Name;
            if (string.IsNullOrEmpty(hfSelectOperation1.Value))
            {
                if (!check_universe_name(txtAreaUniverse.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MYaert", "alert('Area/Universe already exists.');", true);
                    txtAreaUniverse.Focus();
                    return;
                }
                int x = oBLL.Save_Universe_Master(0, struniverse, strstatus, null, strloggeduser, null);
                if (x > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MYaert", "alert('Area/Universe added successfully..');", true);
                }
                ClearControls();
            }
            else
            {
                int x = oBLL.Save_Universe_Master(Convert.ToInt32(hfUniverseId.Value), struniverse, strstatus, null, null, strloggeduser);
                if (x > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MYaert", "alert('Area/Universe updated successfully..');", true);
                }
                ClearControls();
            }
            
        }

        bool check_universe_name(string universename)
        {
            bool retval;
            if (string.IsNullOrEmpty(universename))
            {
                retval = false;
            }
            else
            {
                DataSet dt = new DataSet();
                dt = oBLL.Search_Universe_Master(null, null, strvalue: " and CRUM_UNIVERSE_TO_BE_REVIEWED='"+universename+"'");
                if(dt.Tables.Count>0)
                {
                    if (dt.Tables[0].Rows.Count>0)
                    {
                        retval = false;
                    }
                    else
                    {
                        retval = true;
                    }
                }
                else
                {
                    retval = true;
                }
            }

            return retval;
        }

        void ClearControls()
        {
            txtAreaUniverse.Text = string.Empty;
            ddlStatus.SelectedIndex = 0;
            hfSelectOperation1.Value = string.Empty;
            hfUniverseId.Value = string.Empty;
            FillGrid();
            mvUniverseMaster.ActiveViewIndex = 0;
        }

        void FillGrid()
        {
            DataSet ds = new DataSet();
            string strsearchuniverse = "", strsearchstatus = "";
            strsearchuniverse = string.IsNullOrEmpty(txtSearchAreaUniverse.Text) ? "" : txtSearchAreaUniverse.Text;
            strsearchstatus = ddlSearchStatus.SelectedIndex > 0 ? ddlSearchStatus.SelectedItem.Value : "";
            ds = oBLL.Search_Universe_Master(strsearchuniverse, strsearchstatus, strvalue: " Order by CRUM_ID desc");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvAreaUniverseMaster.DataSource = ds.Tables[0];
                }
                else
                {
                    gvAreaUniverseMaster.DataSource = null;
                }
            }
            else
            {
                gvAreaUniverseMaster.DataSource = null;
            }
            gvAreaUniverseMaster.EditIndex = -1;
            gvAreaUniverseMaster.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        protected void gvAreaUniverseMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strid = gvAreaUniverseMaster.SelectedValue.ToString();
            if (hfSelectOperation1.Value.ToLower() == "edit")
            {
                DataSet ds = new DataSet();
                ds = oBLL.Search_Universe_Master(null, null, UniverserId:Convert.ToInt32(strid));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        hfUniverseId.Value = strid;
                        txtAreaUniverse.Text = ds.Tables[0].Rows[0]["CRUM_UNIVERSE_TO_BE_REVIEWED"].ToString();
                        ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["CRUM_Status"].ToString();
                        mvUniverseMaster.ActiveViewIndex = 1;
                    }
                }
            }
        }

        protected void gvAreaUniverseMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAreaUniverseMaster.PageIndex = e.NewPageIndex;
            FillGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            mvUniverseMaster.ActiveViewIndex = 1;
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

        }
    }
}