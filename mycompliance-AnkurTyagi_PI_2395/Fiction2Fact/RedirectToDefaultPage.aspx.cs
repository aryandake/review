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
using Fiction2Fact.Legacy_App_Code;
using System.Linq;
using Saml;
using Fiction2Fact.App_Code;
using System.IO;

namespace Fiction2Fact
{
    //[Idunno.AntiCsrf.SuppressCsrfCheck]

    public partial class RedirectToDefaultPage : System.Web.UI.Page
    {
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();
        string strSAMLRedirectionType = ConfigurationManager.AppSettings.AllKeys.Any(key => key == "SAMLRedirectionType") ? ConfigurationManager.AppSettings["SAMLRedirectionType"].ToString() : null;

        protected void Page_Load(object sender, EventArgs e)
        {
            SAMLComsume();
        }

        public void SAMLComsume()
        {
            bool isValid = false;
            string strSAMLCertificate = "", strActiveDirectoryDomain = "", strSAMLEncodedResponse = "";
            try
            {
                WriteLogToFile("Step: 1");

                strSAMLCertificate = (ConfigurationManager.AppSettings["SAMLCertificate"].ToString());
                strActiveDirectoryDomain = (ConfigurationManager.AppSettings["ActiveDirectoryDomain"].ToString());
                var response = new Response(strSAMLCertificate, Request.Form["SAMLResponse"]);

                strSAMLEncodedResponse = Page.Request.Form["SAMLResponse"];

                WriteLogToFile("Step: 2 - Encoded SAML Response: " + strSAMLEncodedResponse);

                isValid = response.IsValid();

                WriteLogToFile("Step: 3 - " + isValid.ToString());

                if (response.IsValid())
                {
                    //There is an issue the this i.e. response.GetNameID(); shall return 
                    //details with JoelFrenandes@etlife.in which is the problem in our Membership we have saved
                    //only userid i.e. JoelFrenandes and we need the split the details and save the same to working properly
                    string strUserIDWithDomain = response.GetNameID();
                    WriteLogToFile("Step: 4 - " + strUserIDWithDomain);
                    //string[] strUserID = strUserIDWithDomain.Split('_');
                    string[] strUserDetails = strUserIDWithDomain.Split('@');
                    System.Web.HttpContext.Current.Session["UserName"] = strUserDetails[0];

                    //<< Added by Ramesh more on 05-Apr-2024 CR_2024 for vapt
                    Session["SessionId"] = System.Web.HttpContext.Current.Session.SessionID;

                    //Logins login = new Logins();
                    //login.Action = "1";
                    //login.UserId = strUserDetails[0];
                    //login.SessionId = System.Web.HttpContext.Current.Session.SessionID;
                    //login.IsLoggedIn = "Y";

                    //CommonMasterBL commBL = new CommonMasterBL();
                    //commBL.insertUpdateLoginSession(login);

                    //<< Modified by Vivek on 16-Dec-2024 for CR_2270
                    if (strSAMLRedirectionType.Equals("1"))
                    {
                        FormsAuthentication.RedirectFromLoginPage(strUserDetails[0], false);
                    }
                    else if (strSAMLRedirectionType.Equals("2"))
                    {
                        Response.Redirect("Default.aspx", false);
                    }
                    //>>

                    //FormsAuthentication.RedirectFromLoginPage((strUserID[0]).ToUpper(), false);

                    //lblID.Text = "UserID = " +strUserID;
                    //Label1.Text = "UserUpn = " +response.GetUpn();
                    //Label2.Text = "UserEmail = " +response.GetEmail();
                    //Label3.Text = "UserFirstName = " + response.GetFirstName();
                    //Label4.Text = "UserLastName = " + response.GetLastName();
                    //Label5.Text = "UserDepartment = " + response.GetDepartment();
                    //Label6.Text = "UserPhone = " + response.GetPhone();
                    //Label7.Text = "Usercompany = " + response.GetCompany();
                    //Label8.Text = "CustomAttribute = " + response.GetCustomAttribute(strUserID);
                }
                else
                {
                    string strSAMLEndPoint = "", struniqueIdentity = "", strAssertionUrl = "";

                    strSAMLEndPoint = (ConfigurationManager.AppSettings["SAMLEndpoint"].ToString());
                    struniqueIdentity = (ConfigurationManager.AppSettings["SAMLUniqueIdentity"].ToString());
                    strAssertionUrl = (ConfigurationManager.AppSettings["SAMLAssertionUrl"].ToString());

                    var request = new AuthRequest(struniqueIdentity, strAssertionUrl);
                    Response.Redirect(request.GetRedirectUrl(strSAMLEndPoint));
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, CommonCode.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        //<< Added by Vivek on 13-Dec-2024 for CR_2270
        public static void WriteLogToFile(string strMessage)
        {
            string sLogFile = AppDomain.CurrentDomain.BaseDirectory + "\\";
            sLogFile += (ConfigurationManager.AppSettings.AllKeys.Contains("SSOLogFileWithPath") ? ConfigurationManager.AppSettings["SSOLogFileWithPath"].ToString() : "SSOLog.txt");
            string strLogMessage = Environment.NewLine + DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss ");
            strLogMessage += "Message: " + strMessage + " ";
            File.AppendAllText(sLogFile, strLogMessage);
        }
        //>>

    }
}