using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.DAL;
using System.Linq;
using AjaxControlToolkit;

namespace Fiction2Fact.Projects
{
    public partial class Temp3 : System.Web.UI.MasterPage
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utlBL = new UtilitiesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            string strMenuString;
            string[] strDetailsList;
            string strDetails;
            string strUserName = "";
            Authentication auth = new Authentication();
            //<< Added by ramesh more on 13-Mar-2024 CR_1991
            if (Session["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();

                    //FormsAuthentication.RedirectToLoginPage();
                    Response.Redirect(Global.site_url("Login.aspx"));
                    return;
                }
            }
            //>>
            if (!Page.IsPostBack)
            {
                if (Page.User.Identity.Name.ToString().Equals("") ||
                    string.IsNullOrEmpty((string)Session["CMP_ID"] ?? ""))
                {
                    //FormsAuthentication.RedirectToLoginPage();
                    //<< Added by ramesh more on 13-Mar-2024 CR_1991
                    Response.Redirect(Global.site_url("Login.aspx"));
                    //>>
                }
                else
                {
                    //if (Session["MenuString"] == null)
                    //{
                    MembershipUser user = null;
                    user = Membership.GetUser(Page.User.Identity.Name);
                    strMenuString = utlBL.roleMenuMapping(user.UserName, Convert.ToInt16(Session["CMP_ID"]), CommonCodes.GetCurrentUrlFileName(Page, true), mstrConnectionString);
                    Session["MenuString"] = strMenuString;
                    //}
                    litMenu.Text = Session["MenuString"].ToString().Replace("{HostingServer}", Global.site_url());
                    UtilitiesDAL.SetProductName(CommonCodes.GetCurrentUrlFileName(Page, true));
                    //ltProductName.Text = (Session["ProductFormalName"]==null) ? "" : Session["ProductFormalName"].ToString();

                    strUserName = Page.User.Identity.Name.ToString();

                    strDetails = auth.GetUserDetsByEmpCode(strUserName);
                    strDetailsList = strDetails.Split('|');
                    strUserName = Convert.ToString(strDetailsList[0]);
                    lblLoginUser.Text = strUserName.ToString();
                    lblLastLogin.Text = Session["LastLoginDt"] == null ? "" : Session["LastLoginDt"].ToString();
                }
            }
            Page.Title = Global.site_title(ContentPlaceHolder1.Page.Title);

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Response.Headers.Remove("Server");
            if (Page.User.Identity.IsAuthenticated)
                Page.ViewStateUserKey = Session.SessionID;
            hfCmpShortName.Value = HttpContext.Current.Session["CMP_SHORT_NAME"] == null ? "Comp" : HttpContext.Current.Session["CMP_SHORT_NAME"].ToString();
        }

        protected void OnLoggedOut(object sender, EventArgs e)
        {
            //Session.Abandon();
            //Response.Redirect(Global.site_url("Login.aspx"));
            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            //FormsAuthentication.RedirectToLoginPage();
            //Response.Redirect(Global.site_url("Login.aspx"));
            //<< Added by ramesh more on 13-Mar-2024 CR_1991
            Response.Redirect(Global.site_url("Login.aspx"));
            //>>
        }
    }
}