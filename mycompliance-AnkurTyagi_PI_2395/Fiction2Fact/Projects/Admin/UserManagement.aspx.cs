using System;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;

using System.Web.Profile;

using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.BLL;
using System.Collections.Generic;
using Fiction2Fact.App_Code;
using System.Text.RegularExpressions;

namespace Fiction2Fact.Projects.Admin
{
    public partial class UserManagement : System.Web.UI.Page
    {
        MembershipUser newMembershipUser;
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utilBL = new UtilitiesBLL();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListItem li = new ListItem();
                li.Text = "--Select--";
                li.Value = "";
                DataSet ds = utilBL.getDataSetFromSP("sp_getRoles");
                ddlRoles.DataSource = ds.Tables[0];

                ddlRoles.DataBind();
                ddlRoles.Items.Insert(0, li);

                mvUserInformation.SetActiveView(viewOptions);
                //ViewAllUsers();
            }

        }


        //private void ViewAllUsers()
        //{
        ////dsRoles = HRBusinessMethods.LoadAllRoles();

        ////if (dsRoles != null)
        ////{

        ////    lstRoles.Items.Clear();
        ////    lstRoles.DataSource = dsRoles.Tables[0];
        ////    lstRoles.DataTextField = "Rolename";
        ////    lstRoles.DataValueField = "Rolename";
        ////    lstRoles.DataBind();
        ////}

        ////mvUserInformation.SetActiveView(viewOptions);
        ////dsUsers = HRBusinessMethods.LoadAllUsers();

        ////if (dsUsers != null)
        ////{
        ////    dvUsers = new DataView(dsUsers.Tables[0]);
        ////    dvUsers.Sort = "username";
        ////    dgvUsers.DataSource = dvUsers;                  //dsUsers.Tables[0];
        ////    dgvUsers.DataBind();
        ////    Session["UserMngtDT"] = dvUsers;


        ////}
        ////dvUsers = null;
        ////dsUsers = null;
        //    mvUserInformation.SetActiveView(viewOptions);
        //    DataServer ds = new DataServer();
        //    string strQuery = "select dbo.getRoles(vw_aspnet_MembershipUsers.UserId) as RoleName, vw_aspnet_MembershipUsers.*  from  vw_aspnet_MembershipUsers";
        //    DataTable dtUser = ds.Getdata(strQuery);
        //    dgvUsers.DataSource = dtUser;
        //    dgvUsers.DataBind();
        //}

        protected void btnBack_Click(object sender, EventArgs e)
        {
            //ViewAllUsers();
            mvUserInformation.SetActiveView(viewOptions);
            searchUser();
            writeError("");
        }

        private void GetRolesForUser(string username, CheckBoxList chkRolesListBox)
        {
            string[] rolenames;
            try
            {

                if (username != null && username != "")
                {
                    rolenames = Roles.GetRolesForUser(username);
                    if (chkRolesListBox != null || chkRolesListBox.Items.Count > 0)
                    {
                        foreach (ListItem li in chkRolesListBox.Items)
                        {
                            li.Selected = false;
                            foreach (string role in rolenames)
                            {
                                if (li.Text == role)
                                {
                                    li.Selected = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (chkRolesListBox.Items.Count == 0)
                    writeError("No roles defined");
                else
                    writeError("");
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        protected void dgvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string username = "", strSql;
                username = dgvUsers.SelectedValue.ToString();
                hfUser.Value = username;
                DataTable dt;
                DataRow dr;
                if (hfSelectedOperation.Value == "Edit")
                {
                    strSql = " select * from  vw_aspnet_MembershipUsers where UserName='" + username + "'";

                    DataSet ds = new DataSet();
                    (new App_Code.F2FDatabase(strSql)).F2FDataAdapter.Fill(ds);
                    dt = ds.Tables[0];

                    dr = dt.Rows[0];
                    lbUserId.Text = dr["UserName"].ToString();
                    lbEmail.Text = dr["Email"].ToString();

                    DataTable dtRoles = new DataTable();

                    strSql = "select * from vw_aspnet_Roles order by RoleName";


                    DataSet ds1 = new DataSet();
                    (new App_Code.F2FDatabase(strSql)).F2FDataAdapter.Fill(ds1);
                    dtRoles = ds1.Tables[0];

                    lstRoles.Items.Clear();
                    lstRoles.DataSource = dtRoles;
                    lstRoles.DataTextField = "RoleName";
                    lstRoles.DataValueField = "RoleName";
                    lstRoles.DataBind();

                    GetRolesForUser(username, lstRoles);
                    mvUserInformation.SetActiveView(viewUserInformation);
                    ProfileBase NewuserProfile = ProfileBase.Create(lbUserId.Text, true);
                    lblUser.Text = Convert.ToString(NewuserProfile.GetPropertyValue("name"));
                    lblDept.Text = Convert.ToString(NewuserProfile.GetPropertyValue("department"));
                    lblCon.Text = Convert.ToString(NewuserProfile.GetPropertyValue("telephonenumber"));
                    lblDesg.Text = Convert.ToString(NewuserProfile.GetPropertyValue("title"));
                }
                else if (hfSelectedOperation.Value == "Delete")
                {
                    Membership.DeleteUser(username, true);
                    searchUser();
                    //Commented by prajakta 26-Jun-10
                    //DataTable dtUser;
                    //DataServer ds = new DataServer();           
                    //string strUsername = txtSearchUsername.Text;
                    //string strFilterExpression = "", strQuery="";
                    //string strEmail = txtEmail.Text;
                    //if (strUsername != "")
                    //{
                    //    strFilterExpression = strFilterExpression + " And UserName like '%" + strUsername.Replace("'", "''") + "%'";
                    //}
                    //if (strEmail != "")
                    //{
                    //    strFilterExpression = strFilterExpression + " And Email like '%" + strEmail.Replace("'", "''") + "%'";
                    //}
                    //strQuery = " select * from  vw_aspnet_MembershipUsers where (1=1)";

                    //strQuery = strQuery + strFilterExpression;
                    //dtUser = ds.Getdata(strQuery);
                    //dgvUsers.DataSource = dtUser;
                    //dgvUsers.DataBind();
                    writeError("User deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        public static bool IsValidInput(string input)
        {
            // Only allows letters and numbers; no spaces or special characters
            return Regex.IsMatch(input, @"^[a-zA-Z0-9]+$");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            string username = "";
            username = hfUser.Value;
            if (newMembershipUser == null)
                newMembershipUser = Membership.GetUser(username);

            foreach (ListItem li in lstRoles.Items)
            {
                if (li.Selected && !Roles.IsUserInRole(username, li.Text))
                    Roles.AddUserToRole(username, li.Text);
                if (!li.Selected && Roles.IsUserInRole(username, li.Text))
                    Roles.RemoveUserFromRole(username, li.Text);
            }
            searchUser();
            //ViewAllUsers();
            mvUserInformation.SetActiveView(viewOptions);
        }
        private void writeError(string strMsg)
        {
            this.lblInfoMsg.Text = strMsg;
            this.lblInfoMsg.Visible = true;
        }

        private void hideError()
        {
            this.lblInfoMsg.Text = "";
            this.lblInfoMsg.Visible = false;
        }

        protected void dgvUsers_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (!(Session["UserMngtDT"] == null))
            {
                DataView dvDataView = (DataView)Session["UserMngtDT"];
                if (Convert.ToString(Session["Usersort"]) == "" || Convert.ToString(Session["Usersort"]) == "ASC")
                {
                    e.SortDirection = SortDirection.Ascending;
                    dvDataView.Sort = (e.SortExpression + (" " + ConvertSortDirectionToSql(e.SortDirection)));
                    Session["Usersort"] = "DESC";
                }
                else
                {
                    e.SortDirection = SortDirection.Descending;
                    dvDataView.Sort = (e.SortExpression + (" " + ConvertSortDirectionToSql(e.SortDirection)));
                    Session["Usersort"] = "ASC";
                }
                dgvUsers.DataSource = dvDataView;
                dgvUsers.DataBind();
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

        protected void dgvUsers_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            dgvUsers.PageIndex = e.NewPageIndex;
            dgvUsers.DataSource = Session["UserMngtDT"];
            dgvUsers.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            searchUser();
        }

        private void searchUser()
        {
            try
            {
                DataTable dtUser;
                string strUsername = txtSearchUsername.Text;
                string strEmail = txtEmail.Text;
                string strRoleId = ddlRoles.SelectedValue;
                Dictionary<string, string> spParams = new Dictionary<string, string>()
                {
                    { "UserName", strUsername.Replace("'", "''") },
                    { "Email", strEmail.Replace("'", "''") },
                    { "RoleId", strRoleId },
                };
                DataSet ds1 = new DataSet();
                using (App_Code.F2FDatabase DB = new App_Code.F2FDatabase())
                {
                    ds1 = utilBL.getDataSetFromSP("sp_searchUsers", spParams);

                }
                dtUser = ds1.Tables[0];

                DataView dvDataView = new DataView(dtUser);
                Session["UserMngtDT"] = dvDataView;
                dgvUsers.DataSource = dvDataView;
                dgvUsers.DataBind();
                if (dgvUsers.Rows.Count == 0)
                {
                    lblInfoMsg.Text = "No Records found satisfying the criteria.";
                    lblInfoMsg.Visible = true;
                }
                else
                {
                    lblInfoMsg.Text = String.Empty;
                    lblInfoMsg.Visible = false;
                }

            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            lblInfoMsg.Text = "";
            txtuserName.Text = "";
            txtEmailId.Text = "";
            lblDepartment.Text = "";
            lblContact.Text = "";
            lblDesignation.Text = "";
            txtUserID.Text = "";
            writeError("");
            mvUserInformation.SetActiveView(vwNewuser);
            DataTable dtRoles = new DataTable();

            string strSql = "select * from vw_aspnet_Roles  where (Description!= 'Taxation' or Description is null) order by RoleName";

            DataSet dsRes = new DataSet();
            (new App_Code.F2FDatabase(strSql)).F2FDataAdapter.Fill(dsRes);

            dtRoles = dsRes.Tables[0];
            cbAvRoles.Items.Clear();
            cbAvRoles.DataSource = dtRoles;
            cbAvRoles.DataTextField = "Rolename";
            cbAvRoles.DataValueField = "Rolename";
            cbAvRoles.DataBind();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                writeError("");
                lblInfoMsg.Text = "";

                //<<Added by Ankur Tyagi on 28-Apr-2025 for Project Id : 2395
                if (!IsValidInput(txtUserID.Text))
                {
                    writeError("Special character or spaces are not allowed in User Id.");
                    return;
                }
                //>>

                Membership.CreateUser(txtUserID.Text, "pass@123", txtEmailId.Text);
                foreach (ListItem li in cbAvRoles.Items)
                {
                    if (li.Selected)
                    {
                        Roles.AddUserToRole(txtUserID.Text, li.Text);
                    }
                }
                // ViewAllUsers();
                SaveUserSettings(txtUserID.Text);
                txtUserID.Text = "";
                txtEmailId.Text = "";
                searchUser();
                writeError("User Created Successfully.......");
                mvUserInformation.SetActiveView(viewOptions);
            }
            catch (Exception ex)
            {
                writeError("Error while Creating new user:" + ex.Message);
            }
        }

        private void SaveUserSettings(string userName)
        {
            ProfileBase profile = ProfileBase.Create(userName, true);
            profile.SetPropertyValue("name", txtuserName.Text.Trim());
            profile.SetPropertyValue("department", lblDepartment.Text.Trim());
            profile.SetPropertyValue("telephonenumber", lblContact.Text.Trim());
            profile.SetPropertyValue("title", lblDesignation.Text.Trim());
            profile.Save();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            writeError("");
            dgvUsers.AllowPaging = false;
            dgvUsers.AllowSorting = false;
            dgvUsers.Columns[1].Visible = false;
            //ViewAllUsers();
            dgvUsers.DataSource = (DataView)Session["UserMngtDT"];
            dgvUsers.DataBind();
            string attachment = "attachment; filename=Users.xls";

            Response.ClearContent();

            Response.AddHeader("content-disposition", attachment);

            Response.ContentType = "application/vnd.ms-excel";

            string tab = "";

            for (int i = 0; i <= dgvUsers.Columns.Count; i++)
            {
                if (i == 0 || i == 2 || i == 3 || i == 4)
                {
                    Response.Write(tab + dgvUsers.Columns[i].HeaderText);
                    tab = "\t";
                }
            }

            Response.Write("\n");

            foreach (GridViewRow gvr in dgvUsers.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    tab = "";
                    Response.Write(tab + ((Label)gvr.FindControl("lblSrNo")).Text);
                    tab = "\t";
                    Response.Write(tab + ((Label)gvr.FindControl("lblName")).Text);
                    tab = "\t";
                    Response.Write(tab + ((Label)gvr.FindControl("lblEmail")).Text);
                    tab = "\t";
                    Response.Write(tab + ((Label)gvr.FindControl("lblRole")).Text);
                    tab = "\t";
                    Response.Write("\n");

                }
            }

            Response.End();
            dgvUsers.AllowPaging = true;
            dgvUsers.AllowSorting = true;
            dgvUsers.DataBind();
            dgvUsers.Columns[1].Visible = true;
        }

        protected void btnFetch_Click(object sender, EventArgs e)
        {
            string[] strDetailsList;
            string strDetails;
            Authentication auth = new Authentication();
            strDetails = auth.GetDetailsForUserCreation("BSLI\\" + txtUserID.Text);

            strDetailsList = strDetails.Split('|');
            txtuserName.Text = Convert.ToString(strDetailsList[0]);
            lblDepartment.Text = Convert.ToString(strDetailsList[1]);
            txtEmailId.Text = Convert.ToString(strDetailsList[2]);
            lblContact.Text = Convert.ToString(strDetailsList[3]);
            lblDesignation.Text = Convert.ToString(strDetailsList[4]);
        }
    }
}