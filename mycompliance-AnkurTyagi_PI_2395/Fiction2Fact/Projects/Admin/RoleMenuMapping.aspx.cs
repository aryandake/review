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
using System.Data.SqlClient;

using System.Web.Profile;

using Fiction2Fact.Legacy_App_Code.BLL;

namespace Fiction2Fact.Projects.Admin
{
    public partial class Admin_RoleMenuMapping : System.Web.UI.Page
    {
        UtilitiesBLL utilBL = new UtilitiesBLL();
        RoleMenuMappingBL roleBL = new RoleMenuMappingBL();
        public string F2FDatabaseType = ConfigurationManager.AppSettings["F2FDatabaseType"].ToString();
        string mstrConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            mstrConnectionString = ConfigurationManager.ConnectionStrings[F2FDatabaseType].ConnectionString;
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            if (!Page.IsPostBack)
            {
                DataSet ds = utilBL.getDataSetFromSP("sp_getRoles");
                ddlRole.DataSource = ds.Tables[0];
                //ddlRole.DataSource = utilBL.getDataSet("Roles");
                ddlRole.DataBind();
                ddlRole.Items.Insert(0, new ListItem("Select Role", ""));
            }
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strRoleId = ddlRole.SelectedValue;

            if (strRoleId.Equals(""))
            {
                lstMenu.Items.Clear();
                lstMapping.Items.Clear();
            }
            else
            {
                BindAllList();
            }
        }

        private void BindAllList()
        {
            string strRoleID = ddlRole.SelectedValue;
            lstMenu.DataSource = roleBL.getRolesUnmapped(strRoleID, mstrConnectionString);
            lstMenu.DataBind();
            lstMapping.DataSource = roleBL.getRolesMapped(strRoleID, mstrConnectionString);
            lstMapping.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            if (!lstMenu.SelectedValue.Equals(""))
            {
                foreach (ListItem li in lstMenu.Items)
                {
                    if (li.Selected)
                    {
                        int intMenuId = 0;
                        intMenuId = Convert.ToInt32(li.Value);
                        string strRole = ddlRole.SelectedValue;
                        roleBL.saveRoleMenuMap(strRole, intMenuId, mstrConnectionString);
                    }
                }
            }
            //int intMenuId = 0;
            //if (!lstMenu.SelectedValue.Equals(""))
            //{
            //    intMenuId = Convert.ToInt32(lstMenu.SelectedValue);
            //    string strRole = ddlRole.SelectedValue;
            //    roleBL.saveRoleMenuMap(strRole, intMenuId, mstrConnectionString);
            //    BindAllList();
            //}
            else
            {
                writeError("Please select a Menu Item for mapping.");
            }
            BindAllList();
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (!lstMapping.SelectedValue.Equals(""))
            {
                foreach (ListItem li in lstMapping.Items)
                {
                    if (li.Selected)
                    {
                        int intMenuId = 0;
                        intMenuId = Convert.ToInt32(li.Value);
                        string strRole = ddlRole.SelectedValue;
                        roleBL.deleteRoleMenuMap(strRole, intMenuId, mstrConnectionString);
                    }
                }
            }
            //int intMenuId = 0;
            //if (!lstMapping.SelectedValue.Equals(""))
            //{
            //    intMenuId = Convert.ToInt32(lstMapping.SelectedValue);
            //    string strRole = ddlRole.SelectedValue;
            //    roleBL.deleteRoleMenuMap(strRole, intMenuId, mstrConnectionString);
            //    BindAllList();
            //}
            else
            {
                writeError("Please select a Menu Item for unmapping.");
            }
            BindAllList();
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = false;
            if (!strError.Equals(""))
            {
                string script = "\r\n<script language=\"javascript\">\r\n" +
                           " alert('" + strError.Replace("'", "\\'") + "');" +
                           "   </script>\r\n";

                ClientScript.RegisterStartupScript(this.GetType(), "script", script);
            }
        }
    }
}