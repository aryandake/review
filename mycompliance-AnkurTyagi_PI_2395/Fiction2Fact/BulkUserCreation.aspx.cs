using Fiction2Fact.Legacy_App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact
{
    public partial class BulkUserCreation : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            //<<Added by Ankur Tyagi on 19Mar2024 for CR_1991
            if (Page.User.Identity.Name.Equals(""))
            {
                Response.Redirect(Global.site_url("Login.aspx"));
                return;
            }
            //>>
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            insertRDeptInBulk();
            insertTRDeptInBulk();
        }

        private void insertRDeptInBulk()
        {
            try
            {
                //lblMsg.Text = "";
                //lblID.Text = "";
                DataTable dtUserList = new DataTable();
                DataRow drUserList;
                SqlConnection myconnection = new SqlConnection(strConnectionString);
                string strSql = "";
                string strUserId = "", strUserEmail = "";
                string strName = "", strDepartment = "", strTelephoneNumber = "", strTitle = "";

                string strUserEmailFromDB = "";

                strSql = "select distinct [SRDOM_EMP_ID], [SRDOM_EMAILID] " +
                    "from TBL_SUB_SRD_OWNER_MASTER ";


                SqlCommand mycommand = new SqlCommand(strSql, myconnection);
                myconnection.Open();
                SqlDataAdapter searchDataAdaptor = new SqlDataAdapter(mycommand);
                searchDataAdaptor.Fill(dtUserList);
                myconnection.Close();

                string strAcctCreationDetails = "";
                if (dtUserList.Rows.Count > 0)
                {
                    for (int k = 0; k < dtUserList.Rows.Count; k++)
                    {
                        drUserList = dtUserList.Rows[k];

                        strUserId = drUserList["SRDOM_EMP_ID"].ToString();// 
                        strUserEmailFromDB = drUserList["SRDOM_EMAILID"].ToString();//

                        Authentication au = new Authentication();
                        string strUserDetails = au.GetDetailsForUserCreation(strUserId);
                        if (!strUserDetails.Equals(""))
                        {
                            MembershipCreateStatus createStatus;
                            Membership.CreateUser(strUserId, "pass@123", strUserEmailFromDB,
                                "What is the application name?", "LCMP", true, out createStatus);

                            //if (!strUserEmail.Equals(strUserEmailFromDB) && !strUserEmail.Equals(""))
                            //{
                            //    strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                            //            "AD E-mail Id (" + strUserEmail + ") is not matching DB E-mail Id (" + strUserEmailFromDB + "). <br/>";
                            //}

                            switch (createStatus)
                            {
                                case MembershipCreateStatus.Success:
                                    //strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                    //    "The user account was successfully created! <br/>";
                                    Roles.AddUserToRole(strUserId, "Filing_Reporting_Dept_User");//  HO_Audit_Function_Head     HO_Audit_Risk_Champion  HO_Audit_Issue_Updater                       

                                    ProfileBase profile = ProfileBase.Create(strUserId, true);
                                    profile.SetPropertyValue("name", strName);

                                    profile.SetPropertyValue("department", strDepartment);
                                    profile.SetPropertyValue("telephonenumber", strTelephoneNumber);
                                    profile.SetPropertyValue("title", strTitle);
                                    profile.Save();

                                    break;


                                case MembershipCreateStatus.DuplicateUserName:
                                    strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                        "There already exists a user with this username. <br/>";
                                    if (!Roles.IsUserInRole(strUserId, "Filing_Reporting_Dept_User"))
                                    {
                                        Roles.AddUserToRole(strUserId, "Filing_Reporting_Dept_User");
                                    }
                                    break;

                                case MembershipCreateStatus.DuplicateEmail:
                                    strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                        "There already exists a user with this email address (" + strUserEmail + "). <br/>";

                                    if (!Roles.IsUserInRole(strUserId, "Filing_Reporting_Dept_User"))
                                    {
                                        Roles.AddUserToRole(strUserId, "Filing_Reporting_Dept_User");
                                    }
                                    break;

                                case MembershipCreateStatus.InvalidEmail:
                                    strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                        "The email address you provided in invalid (" + strUserEmail + "). <br/>";
                                    break;

                                case MembershipCreateStatus.InvalidAnswer:
                                    strAcctCreationDetails = strAcctCreationDetails + strUserId + ": "
                                        + "The security answer was invalid. <br/>";
                                    break;

                                case MembershipCreateStatus.InvalidPassword:
                                    strAcctCreationDetails = strAcctCreationDetails + strUserId + ": <br/>" +
                                        "The password you provided is invalid. It must be seven characters long and have at least one non-alphanumeric character.";
                                    break;

                                default:
                                    strAcctCreationDetails = strAcctCreationDetails + strUserId + ": <br/>" +
                                        "There was an unknown error; the user account was NOT created.";
                                    break;
                            }
                        }
                    }
                }
                writeError("insertRDeptInBulk : User Saved successfully. <br/>" + strAcctCreationDetails);

            }
            catch (Exception ex)
            {
                writeError("insertRDeptInBulk : Error while Creating new user:" + ex.Message);
            }
        }

        private void insertTRDeptInBulk()
        {
            try
            {
                //lblMsg.Text = "";
                //lblID.Text = "";
                DataTable dtUserList = new DataTable();
                DataRow drUserList;
                SqlConnection myconnection = new SqlConnection(strConnectionString);
                string strSql = "";
                string strUserId = "", strUserEmail = "";
                string strName = "", strDepartment = "", strTelephoneNumber = "", strTitle = "";

                string strUserEmailFromDB = "";

                strSql = "select distinct [EM_USERNAME], [EM_EMAIL] " +
                    "from EmployeeMaster ";


                SqlCommand mycommand = new SqlCommand(strSql, myconnection);
                myconnection.Open();
                SqlDataAdapter searchDataAdaptor = new SqlDataAdapter(mycommand);
                searchDataAdaptor.Fill(dtUserList);
                myconnection.Close();

                string strAcctCreationDetails = "";
                if (dtUserList.Rows.Count > 0)
                {
                    for (int k = 0; k < dtUserList.Rows.Count; k++)
                    {
                        drUserList = dtUserList.Rows[k];

                        strUserId = drUserList["EM_USERNAME"].ToString();// 
                        strUserEmailFromDB = drUserList["EM_EMAIL"].ToString();//

                        Authentication au = new Authentication();
                        string strUserDetails = au.GetDetailsForUserCreation(strUserId);
                        if (!strUserDetails.Equals(""))
                        {
                            MembershipCreateStatus createStatus;
                            Membership.CreateUser(strUserId, "pass@123", strUserEmailFromDB,
                                "What is the application name?", "LCMP", true, out createStatus);

                            //if (!strUserEmail.Equals(strUserEmailFromDB) && !strUserEmail.Equals(""))
                            //{
                            //    strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                            //            "AD E-mail Id (" + strUserEmail + ") is not matching DB E-mail Id (" + strUserEmailFromDB + "). <br/>";
                            //}

                            switch (createStatus)
                            {
                                case MembershipCreateStatus.Success:
                                    //strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                    //    "The user account was successfully created! <br/>";
                                    Roles.AddUserToRole(strUserId, "FilingUser");//  HO_Audit_Function_Head     HO_Audit_Risk_Champion  HO_Audit_Issue_Updater                       

                                    ProfileBase profile = ProfileBase.Create(strUserId, true);
                                    profile.SetPropertyValue("name", strName);

                                    profile.SetPropertyValue("department", strDepartment);
                                    profile.SetPropertyValue("telephonenumber", strTelephoneNumber);
                                    profile.SetPropertyValue("title", strTitle);
                                    profile.Save();

                                    break;


                                case MembershipCreateStatus.DuplicateUserName:
                                    strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                        "There already exists a user with this username. <br/>";
                                    if (!Roles.IsUserInRole(strUserId, "FilingUser"))
                                    {
                                        Roles.AddUserToRole(strUserId, "FilingUser");
                                    }
                                    break;

                                case MembershipCreateStatus.DuplicateEmail:
                                    strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                        "There already exists a user with this email address (" + strUserEmail + "). <br/>";

                                    if (!Roles.IsUserInRole(strUserId, "FilingUser"))
                                    {
                                        Roles.AddUserToRole(strUserId, "FilingUser");
                                    }
                                    break;

                                case MembershipCreateStatus.InvalidEmail:
                                    strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                        "The email address you provided in invalid (" + strUserEmail + "). <br/>";
                                    break;

                                case MembershipCreateStatus.InvalidAnswer:
                                    strAcctCreationDetails = strAcctCreationDetails + strUserId + ": "
                                        + "The security answer was invalid. <br/>";
                                    break;

                                case MembershipCreateStatus.InvalidPassword:
                                    strAcctCreationDetails = strAcctCreationDetails + strUserId + ": <br/>" +
                                        "The password you provided is invalid. It must be seven characters long and have at least one non-alphanumeric character.";
                                    break;

                                default:
                                    strAcctCreationDetails = strAcctCreationDetails + strUserId + ": <br/>" +
                                        "There was an unknown error; the user account was NOT created.";
                                    break;
                            }
                        }
                    }
                }
                writeError("insertRDeptInBulk : User Saved successfully. <br/>" + strAcctCreationDetails);

            }
            catch (Exception ex)
            {
                writeError("insertRDeptInBulk : Error while Creating new user:" + ex.Message);
            }
        }

        public void writeError(string strMsg)
        {
            lblMsg.Text = lblMsg.Text + strMsg;
        }

    }
}