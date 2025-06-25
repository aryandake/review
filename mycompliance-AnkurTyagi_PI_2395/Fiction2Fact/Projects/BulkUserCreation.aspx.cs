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
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using Fiction2Fact;

public partial class BulkUserCreation1 : System.Web.UI.Page
{
    string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //<<Added by Ankur Tyagi on 19Mar2024 for CR_1991
            if (Page.User.Identity.Name.Equals(""))
            {
                Response.Redirect(Global.site_url("Login.aspx"));
                return;
            }
            //>>
            if (!Page.IsPostBack)
            {
                insertRiskChampionInBulk();

                insertUnitHead1InBulk();

                insertFunctionHeadInBulk();

            }
        }
        //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
        catch (Exception ex)
        {
            string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
            writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
        }
        //>>
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (Page != null)
        {
            Page.RegisterRequiresViewStateEncryption();
        }
    }


    private void insertRiskChampionInBulk()
    {
        try
        {
            //lblMsg.Text = "";
            //lblID.Text = "";
            DataTable dtUserList = new DataTable();
            DataRow drUserList;
            string strSql = "";
            string strUserId = "", strUserEmail = "";
            string strName = "", strDepartment = "", strTelephoneNumber = "", strTitle = "";

            string strUserEmailFromDB = "";

            strSql = " SELECT distinct CSSDM_USER_ID, CSSDM_EMAIL_ID " +
                     " from TBL_CERT_SUB_SUB_DEPT_MAS ";

            dtUserList = F2FDatabase.F2FGetDataTable(strSql);

            string strAcctCreationDetails = "";
            if (dtUserList.Rows.Count > 0)
            {
                for (int k = 0; k < dtUserList.Rows.Count; k++)
                {
                    drUserList = dtUserList.Rows[k];

                    strUserId = drUserList["CSSDM_USER_ID"].ToString();// 
                    strUserEmailFromDB = drUserList["CSSDM_EMAIL_ID"].ToString();//

                    Authentication au = new Authentication();
                    string strUserDetails = au.GetDetailsForUserCreation(strUserId);
                    if (!strUserDetails.Equals(""))
                    {
                        string[] strSplit = strUserDetails.Split('|');
                        strName = strSplit[0];
                        strDepartment = strSplit[1];
                        strUserEmail = strSplit[2];
                        strTelephoneNumber = strSplit[3];
                        strTitle = strSplit[4];

                        MembershipCreateStatus createStatus;
                        Membership.CreateUser(strUserId, "pass@123", strUserEmail,
                            "What is the application name?", "LCMS", true, out createStatus);

                        if (!strUserEmail.Equals(strUserEmailFromDB) && !strUserEmail.Equals(""))
                        {
                            strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                    "AD E-mail Id (" + strUserEmail + ") is not matching DB E-mail Id (" + strUserEmailFromDB + "). <br/>";
                        }

                        switch (createStatus)
                        {
                            case MembershipCreateStatus.Success:
                                //strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                //    "The user account was successfully created! <br/>";
                                Roles.AddUserToRole(strUserId, "Certification_Function_Spoc");//  HO_Audit_Function_Head     HO_Audit_Risk_Champion  HO_Audit_Issue_Updater                       

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
                                if (!Roles.IsUserInRole(strUserId, "Certification_Function_Spoc"))
                                {
                                    Roles.AddUserToRole(strUserId, "Certification_Function_Spoc");
                                }
                                break;

                            case MembershipCreateStatus.DuplicateEmail:
                                strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                    "There already exists a user with this email address (" + strUserEmail + "). <br/>";

                                if (!Roles.IsUserInRole(strUserId, "Certification_Function_Spoc"))
                                {
                                    Roles.AddUserToRole(strUserId, "Certification_Function_Spoc");
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
            writeError("insertRiskChampionInBulk : User Saved successfully. <br/>" + strAcctCreationDetails);

        }
        catch (Exception ex)
        {
            F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            writeError("InsertRiskChampionInBulk : Error while Creating new user:" + ex.Message);
        }
    }


    private void insertUnitHead1InBulk()
    {
        try
        {
            //lblMsg.Text = "";
            //lblID.Text = "";
            DataTable dtUserList = new DataTable();
            DataRow drUserList;
            string strSql = "";
            string strUserId = "", strUserEmail = "";
            string strName = "", strDepartment = "", strTelephoneNumber = "", strTitle = "";

            string strUserEmailFromDB = "";

            strSql = " select distinct CSDM_USER_ID, CSDM_EMAIL_ID " +
                    " from TBL_CERT_SUB_DEPT_MAS";

            dtUserList = F2FDatabase.F2FGetDataTable(strSql);
            string strAcctCreationDetails = "";
            if (dtUserList.Rows.Count > 0)
            {
                for (int k = 0; k < dtUserList.Rows.Count; k++)
                {
                    drUserList = dtUserList.Rows[k];

                    strUserId = drUserList["CSDM_USER_ID"].ToString();// 
                    strUserEmailFromDB = drUserList["CSDM_EMAIL_ID"].ToString();//

                    Authentication au = new Authentication();
                    string strUserDetails = au.GetDetailsForUserCreation(strUserId);
                    if (!strUserDetails.Equals(""))
                    {
                        string[] strSplit = strUserDetails.Split('|');
                        strName = strSplit[0];
                        strDepartment = strSplit[1];
                        strUserEmail = strSplit[2];
                        strTelephoneNumber = strSplit[3];
                        strTitle = strSplit[4];

                        MembershipCreateStatus createStatus;
                        Membership.CreateUser(strUserId, "pass@123", strUserEmail,
                            "What is the application name?", "LCMS", true, out createStatus);

                        if (!strUserEmail.Equals(strUserEmailFromDB) && !strUserEmail.Equals(""))
                        {
                            strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                    "AD E-mail Id (" + strUserEmail + ") is not matching DB E-mail Id (" + strUserEmailFromDB + "). <br/>";
                        }

                        switch (createStatus)
                        {
                            case MembershipCreateStatus.Success:
                                //strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                //    "The user account was successfully created! <br/>";
                                Roles.AddUserToRole(strUserId, "Certification_Unit_Head");//  HO_Audit_Function_Head     HO_Audit_Risk_Champion  HO_Audit_Issue_Updater                       

                                //Roles.AddUserToRole(strUserId,  "HO_Audit_Issue_Updater");
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
                                if (!Roles.IsUserInRole(strUserId, "Certification_Unit_Head"))
                                {
                                    Roles.AddUserToRole(strUserId, "Certification_Unit_Head");
                                }
                                break;

                            case MembershipCreateStatus.DuplicateEmail:
                                strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                    "There already exists a user with this email address (" + strUserEmail + "). <br/>";

                                if (!Roles.IsUserInRole(strUserId, "Certification_Unit_Head"))
                                {
                                    Roles.AddUserToRole(strUserId, "Certification_Unit_Head");
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
            writeError("insertUnitHead1InBulk : User Saved successfully. <br/>" + strAcctCreationDetails);

        }
        catch (Exception ex)
        {
            F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            writeError("insertUnitHead1InBulk : Error while Creating new user:" + ex.Message);
        }
    }

    private void insertFunctionHeadInBulk()
    {
        try
        {
            //lblMsg.Text = "";
            //lblID.Text = "";
            DataTable dtUserList = new DataTable();
            DataRow drUserList;
            string strSql = "";
            string strUserId = "", strUserEmail = "";
            string strName = "", strDepartment = "", strTelephoneNumber = "", strTitle = "";

            string strUserEmailFromDB = "";

            strSql = "select distinct CDM_CXO_USERID, CDM_CXO_EMAILID " +
                    "from TBL_CERT_DEPT_MAS ";

            dtUserList = F2FDatabase.F2FGetDataTable(strSql);

            string strAcctCreationDetails = "";
            if (dtUserList.Rows.Count > 0)
            {
                for (int k = 0; k < dtUserList.Rows.Count; k++)
                {
                    drUserList = dtUserList.Rows[k];

                    strUserId = drUserList["CDM_CXO_USERID"].ToString();// 
                    strUserEmailFromDB = drUserList["CDM_CXO_EMAILID"].ToString();//

                    Authentication au = new Authentication();
                    string strUserDetails = au.GetDetailsForUserCreation(strUserId);
                    if (!strUserDetails.Equals(""))
                    {
                        string[] strSplit = strUserDetails.Split('|');
                        strName = strSplit[0];
                        strDepartment = strSplit[1];
                        strUserEmail = strSplit[2];
                        strTelephoneNumber = strSplit[3];
                        strTitle = strSplit[4];

                        MembershipCreateStatus createStatus;
                        Membership.CreateUser(strUserId, "pass@123", strUserEmail,
                            "What is the application name?", "LCMS", true, out createStatus);

                        if (!strUserEmail.Equals(strUserEmailFromDB) && !strUserEmail.Equals(""))
                        {
                            strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                    "AD E-mail Id (" + strUserEmail + ") is not matching DB E-mail Id (" + strUserEmailFromDB + "). <br/>";
                        }

                        switch (createStatus)
                        {
                            case MembershipCreateStatus.Success:
                                //strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                //    "The user account was successfully created! <br/>";
                                Roles.AddUserToRole(strUserId, "Certification_Function_Head");//  HO_Audit_Function_Head     HO_Audit_Risk_Champion  HO_Audit_Issue_Updater                       

                                //Roles.AddUserToRole(strUserId,  "HO_Audit_Issue_Updater");
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
                                if (!Roles.IsUserInRole(strUserId, "Certification_Function_Head"))
                                {
                                    Roles.AddUserToRole(strUserId, "Certification_Function_Head");
                                }
                                break;

                            case MembershipCreateStatus.DuplicateEmail:
                                strAcctCreationDetails = strAcctCreationDetails + strUserId + ": " +
                                    "There already exists a user with this email address (" + strUserEmail + "). <br/>";

                                if (!Roles.IsUserInRole(strUserId, "Certification_Function_Head"))
                                {
                                    Roles.AddUserToRole(strUserId, "Certification_Function_Head");
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
            writeError("insertFunctionHeadInBulk : User Saved successfully. <br/>" + strAcctCreationDetails);

        }
        catch (Exception ex)
        {
            F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            writeError("insertFunctionHeadInBulk : Error while Creating new user:" + ex.Message);
        }
    }



    public void writeError(string strMsg)
    {
        lblMsg.Text = strMsg;
    }
}
