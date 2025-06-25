using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.UI;
using System.Configuration;
using System.Web.Optimization;
using System.Text.RegularExpressions;
using Fiction2Fact.App_Code;

namespace Fiction2Fact
{
    public class Global : HttpApplication
    {
        public static string AppDbType { get; set; } = "MsSQL";
        public static string FileUploadRegex { get; set; } = ConfigurationManager.AppSettings["FileUploadRegex"] == null
            ? "^.+(.pdf|.PDF|.xls|.XLS|.xlsx|.XSLX|.txt|.TXT|.jpg|.JPG|.doc|.DOC|.docx|.DOCX|.wmv|.WMV|.avi|.AVI|.mpeg|.MPEG|.mp3|.MP3|.mp4|.MP4|.wav|.WAV|.pps|.ppsx|.eml|.EML|.ppt|.PPT|.pptx|.PPTX|.gif|.GIF|.JPEG|.jpeg|.png|.PNG|.zip|.ZIP|.msg|.MSG)$"
            : ConfigurationManager.AppSettings["FileUploadRegex"].ToString();

        //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
        private static string GlobalError = "";
        //>>

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //ScriptManager.ScriptResourceMapping.AddDefinition("jquery",
            //new ScriptResourceDefinition
            //{
            //    Path = "~/Content/js/plugins/jquery/jquery-3.3.1.min.js",
            //    DebugPath = "~/Content/js/plugins/jquery/jquery-3.3.1.js",
            //    CdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.1.min.js",
            //    CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.1.js"
            //});
            AppDbType = ConfigurationManager.AppSettings["F2FDatabaseType"].ToString();
            MvcHandler.DisableMvcResponseHeader = true;

        }
        //<< Added by ramesh more on 13-Mar-2024 CR_1991
        //void Session_Start(object sender, EventArgs e)
        //{
        //    // Code that runs when a new session is started
        //    try
        //    {
        //        Session.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings.AllKeys.Any(key => key == "SessionTimeout") ? ConfigurationManager.AppSettings["SessionTimeout"].ToString() : "20");

        //        if (Request.IsSecureConnection == true)
        //        {
        //            Response.Cookies["ASP.NET_SessionId"].Secure = true;
        //            Response.Cookies[".ASPXAUTH"].Secure = true;
        //            Response.Cookies["__CSRFCOOKIE"].Secure = true;

        //            Response.Cookies["ASP.NET_SessionId"].SameSite = SameSiteMode.Strict;
        //            Response.Cookies[".ASPXAUTH"].SameSite = SameSiteMode.Strict;
        //            Response.Cookies["__CSRFCOOKIE"].SameSite = SameSiteMode.Strict;

        //            if (Response.Cookies["AuthToken"] != null)
        //            {
        //                Response.Cookies["AuthToken"].Secure = true;
        //                Response.Cookies["AuthToken"].SameSite = SameSiteMode.Strict;
        //            }
        //        }

        //    }
        //    catch (Exception)
        //    {

        //    }

        //}
        //>>
        
        //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
        protected void Session_Start(Object sender, EventArgs e)
        {
            // << Code modified by ramesh more on 10-Oct-2023 CR_845 for VAPT
            try
            {
                Session.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings.AllKeys.Any(key => key == "SessionTimeout") ? ConfigurationManager.AppSettings["SessionTimeout"].ToString() : "20");

                string strCookiesPathSetting = ConfigurationManager.AppSettings["CookiesPathSetting"] == null ? "1" :
                    ConfigurationManager.AppSettings["CookiesPathSetting"].ToString();

                string strCookiesPath = ConfigurationManager.AppSettings["CookiesPath"] == null ? "/" :
                    ConfigurationManager.AppSettings["CookiesPath"].ToString();

                if (!string.IsNullOrEmpty(strCookiesPathSetting) && strCookiesPathSetting.Equals("1"))
                {
                    Response.Cookies["ASP.NET_SessionId"].Path = strCookiesPath;
                    Response.Cookies[".ASPXAUTH"].Path = strCookiesPath;
                    Response.Cookies["__CSRFCOOKIE"].Path = strCookiesPath;
                }

                if (Request.IsSecureConnection == true)
                {
                    Response.Cookies["ASP.NET_SessionId"].Secure = true;
                    Response.Cookies[".ASPXAUTH"].Secure = true;
                    Response.Cookies["__CSRFCOOKIE"].Secure = true;

                    Response.Cookies["ASP.NET_SessionId"].SameSite = SameSiteMode.Strict;
                    Response.Cookies[".ASPXAUTH"].SameSite = SameSiteMode.Strict;
                    Response.Cookies["__CSRFCOOKIE"].SameSite = SameSiteMode.Strict;

                    if (Response.Cookies["AuthToken"] != null)
                    {
                        Response.Cookies["AuthToken"].Secure = true;
                        Response.Cookies["AuthToken"].SameSite = SameSiteMode.Strict;
                    }
                }
            }
            catch (Exception)
            {

            }
            // >>
        }
        //>>

        public static string site_url(string url = null, bool bVirtualPath = false, bool bLegacy = false)
        {
            //string sSubFolder = ConfigurationManager.AppSettings.AllKeys.Contains("SubFolder") ? ConfigurationManager.AppSettings["SubFolder"].ToString() : null;
            //url = (string.IsNullOrEmpty(sSubFolder) ? "" : sSubFolder + "/") + url;
            //if (bVirtualPath) { return ("~/" + url).Replace("//", "/"); }
            //Uri curURL = HttpContext.Current.Request.Url;
            //return curURL.Scheme + "://" + curURL.Host + ":" + curURL.Port + "/" + url;

            string sSubFolder = ConfigurationManager.AppSettings.AllKeys.Any(key => key == "SubFolder") ? ConfigurationManager.AppSettings["SubFolder"].ToString() : null;
            string strSiteURLType = ConfigurationManager.AppSettings.AllKeys.Any(key => key == "Site_Url_Type") ? ConfigurationManager.AppSettings["Site_Url_Type"].ToString() : "1";
            string strSiteURLLink = ConfigurationManager.AppSettings.AllKeys.Any(key => key == "Site_Url_Link") ? ConfigurationManager.AppSettings["Site_Url_Link"].ToString() : null;

            url = (string.IsNullOrEmpty(sSubFolder) ? "" : sSubFolder + "/") + url;
            if (bVirtualPath) { return ("~/" + url).Replace("//", "/"); }
            Uri curURL = HttpContext.Current.Request.Url;
            //<< Modified by Amarjeet on 19-Nov-2020
            //return curURL.Scheme + "://" + curURL.Host + ":" + curURL.Port + "/" + url;
            string strURL = "";

            if (strSiteURLType.Equals("1"))
                strURL = curURL.Scheme + "://" + curURL.Host + ":" + curURL.Port + "/" + url;
            else if (strSiteURLType.Equals("2"))
                strURL = strSiteURLLink + "/" + url;

            return strURL;
            //>>
        }
        public static string site_title(string sTitle = null)
        {
            string sProductFormalName = string.IsNullOrEmpty(HttpContext.Current.Session["ProductFormalName"] as string) ? "" : HttpContext.Current.Session["ProductFormalName"].ToString() + " :: ";
            string sSystemName = string.IsNullOrEmpty(HttpContext.Current.Session["CMP_SYSTEM_NAME"] as string) ? "" : HttpContext.Current.Session["CMP_SYSTEM_NAME"].ToString() + " : ";
            return sSystemName + sProductFormalName + (string.IsNullOrEmpty(sTitle) ? "" : sTitle + " : ");
        }

        //void Application_Error(object sender, EventArgs e)
        //{
        //    string ENVIRONMENT = ConfigurationManager.AppSettings.AllKeys.Contains("ENVIRONMENT") ? ConfigurationManager.AppSettings["ENVIRONMENT"].ToString() : "PRODUCTTION";
        //    // Code that runs when an unhandled error occurs
        //    if (ENVIRONMENT.ToUpper().Equals("PRODUCTION") || ENVIRONMENT.ToLower().Equals("development"))
        //    {
        //        HttpException ex = Server.GetLastError() as System.Web.HttpException;
        //        string str = "/";
        //        string strPageUrl = "";
        //        string strHostingServer = ConfigurationManager.AppSettings["HostingServer"].ToString();

        //        if (ex != null)
        //        {
        //            if (!ex.Message.ToString().Contains("/favicon.ico"))
        //                F2FLog.Log(ex, HttpUtility.HtmlEncode(Regex.Replace(HttpContext.Current.Request.Url.ToString(), @"<script(.*?)<\/script>", @"", RegexOptions.IgnoreCase)), System.Reflection.MethodBase.GetCurrentMethod().Name);

        //            int errorCode = ex.GetHttpCode();
        //            if (errorCode.Equals(500))
        //            {
        //                if (ex is HttpRequestValidationException)
        //                {
        //                    HttpContext con = HttpContext.Current;
        //                    Response.StatusCode = 200;
        //                    Response.Write(@"<html><head><title>System Exception</title> " +
        //                    "<script> function back() { history.go(-1); } <" + str + "script>" +
        //                    "<style type='text" + str + "css'> " +
        //                    "<!-- " +
        //                    "body { " +
        //                        "margin-left: 0px; " +
        //                        "margin-top: 0px; " +
        //                        "margin-right: 0px; " +
        //                        "margin-bottom: 0px; " +
        //                    "} " +
        //                    ".heading{ " +
        //                        "font-family:'Trebuchet MS', sans-serif; " +
        //                        "font-size:42px; " +
        //                        "color:#333333; " +
        //                        "padding: 15px 0; " +
        //                        "text-align:center; " +
        //                    "} " +
        //                    "p{ " +
        //                        "font-family:'Trebuchet MS', sans-serif; " +
        //                        "font-size:15px; " +
        //                        "color:#666666; " +
        //                        "text-align:center; " +
        //                    "} " +
        //                    "a{ " +
        //                        "font-family:'Trebuchet MS', sans-serif; " +
        //                        "font-size:15px; " +
        //                        "color:#0080C0; " +
        //                        "text-align:center; " +
        //                    "} " +
        //                    "--> " +
        //                    "</style> " +
        //                    "</head>" +
        //                    "<body style='font-family: Trebuchet MS;'>" +
        //                    "<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
        //                    "<tr><td>&nbsp;</td></tr>" +
        //                    "<tr><td align='center' valign='middle'> " +
        //                    "<img src='" + strHostingServer + "/Content/images/oops_icon.png' width='150' height='150' />" +
        //                    "</td></tr>" +
        //                    "<tr><td align='center' class='heading'>Oops!</td> </tr>" +
        //                    "<tr>" +
        //                    "<td align='center'>" +
        //                    "<p>I'm sorry, but HTML entry is not allowed on that page.</p>" +
        //                    "<p>Please make sure that your entries do not contain any angle brackets like &lt; or &gt;." +
        //                    " <br /></p>" +
        //                    "</td></tr>" +
        //                    "<tr><td></td></tr>" +
        //                    "<tr><td align='center'><a href='javascript:back()'>Go back</a></td></tr>" +
        //                    "</table>" +
        //                    "</body></html>");
        //                    Response.End();
        //                }
        //                else
        //                {
        //                    HttpContext con = HttpContext.Current;
        //                    strPageUrl = con.Request.Url.ToString();
        //                    strPageUrl = Regex.Replace(strPageUrl.ToString(), @"<script(.*?)<\/script>", @"", RegexOptions.IgnoreCase);
        //                    strPageUrl = HttpUtility.HtmlEncode(strPageUrl);
        //                    strPageUrl = strPageUrl.Replace("&lt;", "");
        //                    strPageUrl = strPageUrl.Replace("&gt;", "");

        //                    Response.StatusCode = 200;
        //                    Response.Write(@"<html><head><title>System Exception</title> " +
        //                    "<script> function back() { history.go(-1); } <" + str + "script>" +
        //                    "<style type='text" + str + "css'> " +
        //                    "<!-- " +
        //                    "body { " +
        //                        "margin-left: 0px; " +
        //                        "margin-top: 0px; " +
        //                        "margin-right: 0px; " +
        //                        "margin-bottom: 0px; " +
        //                    "} " +
        //                    ".heading{ " +
        //                        "font-family:'Trebuchet MS', sans-serif; " +
        //                        "font-size:42px; " +
        //                        "color:#333333; " +
        //                        "padding: 15px 0; " +
        //                        "text-align:center; " +
        //                    "} " +
        //                    "p{ " +
        //                        "font-family:'Trebuchet MS', sans-serif; " +
        //                        "font-size:15px; " +
        //                        "color:#666666; " +
        //                        "text-align:center; " +
        //                    "} " +
        //                    "a{ " +
        //                        "font-family:'Trebuchet MS', sans-serif; " +
        //                        "font-size:15px; " +
        //                        "color:#0080C0; " +
        //                        "text-align:center; " +
        //                    "} " +
        //                    "--> " +
        //                    "</style> " +
        //                    "</head>" +
        //                    "<body style='font-family: Trebuchet MS;'>" +
        //                    "<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
        //                    "<tr><td>&nbsp;</td></tr>" +
        //                    "<tr><td align='center' valign='middle'> " +
        //                    "<img src='" + strHostingServer + "/Content/images/oops_icon.png' width='150' height='150' />" +
        //                    "</td></tr>" +
        //                    "<tr><td align='center' class='heading'>Oops!</td> </tr>" +
        //                    "<tr>" +
        //                    "<td align='center'>" +
        //                    "<p>I'm sorry, Something went wrong.</p>" +
        //                    //"<p>Page URL : " + strPageUrl + "</p>" +
        //                    //"<p>Error : " + ex.Message + "</p>" +
        //                    "<p>Error Code : " + ex.ErrorCode + "</p>" +
        //                    //"<p>Inner Exception : " + ex.InnerException + "</p>" +
        //                    "</td></tr>" +
        //                    "<tr><td></td></tr>" +
        //                    "<tr><td align='center'><a href='javascript:back()'>Go back</a></td></tr>" +
        //                    "</table>" +
        //                    "</body></html>");
        //                    Response.End();
        //                }
        //            }
        //            else if (errorCode.Equals(403) || errorCode.Equals(404))
        //            {
        //                Response.Redirect("~/ErrorNotFound.aspx", true);
        //            }
        //            else
        //            {
        //                HttpContext con = HttpContext.Current;
        //                strPageUrl = con.Request.Url.ToString();
        //                strPageUrl = Regex.Replace(strPageUrl.ToString(), @"<script(.*?)<\/script>", @"", RegexOptions.IgnoreCase);
        //                strPageUrl = HttpUtility.HtmlEncode(strPageUrl);
        //                strPageUrl = strPageUrl.Replace("&lt;", "");
        //                strPageUrl = strPageUrl.Replace("&gt;", "");

        //                Response.StatusCode = 200;
        //                Response.Write(@"<html><head><title>Error Has Occured.</title> " +
        //                "<script> function back() { history.go(-1); } <" + str + "script>" +
        //                "<style type='text" + str + "css'> " +
        //                "<!-- " +
        //                "body { " +
        //                    "margin-left: 0px; " +
        //                    "margin-top: 0px; " +
        //                    "margin-right: 0px; " +
        //                    "margin-bottom: 0px; " +
        //                "} " +
        //                ".heading{ " +
        //                    "font-family:'Trebuchet MS', sans-serif; " +
        //                    "font-size:42px; " +
        //                    "color:#333333; " +
        //                    "padding: 15px 0; " +
        //                    "text-align:center; " +
        //                "} " +
        //                "p{ " +
        //                    "font-family:'Trebuchet MS', sans-serif; " +
        //                    "font-size:15px; " +
        //                    "color:#666666; " +
        //                    "text-align:center; " +
        //                "} " +
        //                "a{ " +
        //                    "font-family:'Trebuchet MS', sans-serif; " +
        //                    "font-size:15px; " +
        //                    "color:#0080C0; " +
        //                    "text-align:center; " +
        //                "} " +
        //                "--> " +
        //                "</style> " +
        //                "</head>" +
        //                "<body style='font-family: Trebuchet MS;'>" +
        //                "<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
        //                "<tr><td>&nbsp;</td></tr>" +
        //                "<tr><td align='center' valign='middle'> " +
        //                "<img src='" + strHostingServer + "/Content/images/oops_icon.png' width='150' height='150' />" +
        //                "</td></tr>" +
        //                "<tr><td align='center' class='heading'>Oops!</td> </tr>" +
        //                "<tr>" +
        //                "<td align='center'>" +
        //                "<p>Something went wrong.</p>" +
        //                "<p>Please contact your System administrator.</p>" +
        //                //"<p>Page URL : " + strPageUrl + "</p>" +
        //                //"<p>Error : " + ex.Message + "</p>" +
        //                "<p>Error Code : " + ex.ErrorCode + "</p>" +
        //                //"<p>Inner Exception : " + ex.InnerException + "</p>" +
        //                "</td></tr>" +
        //                "<tr><td></td></tr>" +
        //                "<tr><td align='center'><a href='javascript:back()'>Go back</a></td></tr>" +
        //                "</table>" +
        //                "</body></html>");
        //                Response.End();
        //            }
        //        }
        //        else
        //        {
        //            Exception ex1 = Server.GetLastError() as Exception;
        //            if (ex1.Message.Equals("Invalid Session"))
        //            {
        //                FormsAuthentication.SignOut();
        //                Response.StatusCode = 200;
        //                Response.Write(@"<html><head><title>Invalid Session</title> " +
        //                "</head><body style='font-family: Trebuchet MS, Sans-serif;'><br /><center><h1>Invalid Session</h1><br />Please close the browser and login again.</center></body></html>");
        //                Response.End();
        //            }
        //        }
        //    }
        //}

        //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
        protected void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            string strGlobal_ApplicationError_On = ConfigurationManager.AppSettings["Global_ApplicationError_On"] == null ? "1" :
                    ConfigurationManager.AppSettings["Global_ApplicationError_On"].ToString();
            if (strGlobal_ApplicationError_On == "1")
            {
                HttpException ex = Server.GetLastError() as System.Web.HttpException;
                string str = "/";
                string strPageUrl = "", strErrorMsg = "";
                string strHostingServer = Global.site_url();

                if (ex != null)
                {
                    if (!ex.Message.ToString().Contains("/favicon.ico"))
                    {
                        strErrorMsg = F2FLog.Log(ex, HttpUtility.HtmlEncode(Regex.Replace(HttpContext.Current.Request.Url.ToString(), @"<script(.*?)<\/script>", @"", RegexOptions.IgnoreCase)), System.Reflection.MethodBase.GetCurrentMethod().Name);

                        Global.GlobalError = strErrorMsg;
                    }

                    int errorCode = ex.GetHttpCode();
                    if (errorCode.Equals(500))
                    {
                        if (ex is HttpRequestValidationException)
                        {
                            HttpContext con = HttpContext.Current;
                            Response.StatusCode = 200;
                            Response.Write(@"<html><head><title>System Exception</title> " +
                            "<script> function back() { history.go(-1); } <" + str + "script>" +
                            "<style type='text" + str + "css'> " +
                            "<!-- " +
                            "body { " +
                                "margin-left: 0px; " +
                                "margin-top: 0px; " +
                                "margin-right: 0px; " +
                                "margin-bottom: 0px; " +
                            "} " +
                            ".heading{ " +
                                "font-family:'Trebuchet MS', sans-serif; " +
                                "font-size:42px; " +
                                "color:#333333; " +
                                "padding: 15px 0; " +
                                "text-align:center; " +
                            "} " +
                            "p{ " +
                                "font-family:'Trebuchet MS', sans-serif; " +
                                "font-size:15px; " +
                                "color:#666666; " +
                                "text-align:center; " +
                            "} " +
                            "a{ " +
                                "font-family:'Trebuchet MS', sans-serif; " +
                                "font-size:15px; " +
                                "color:#0080C0; " +
                                "text-align:center; " +
                            "} " +
                            "--> " +
                            "</style> " +
                            "</head>" +
                            "<body style='font-family: Trebuchet MS;'>" +
                            "<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                            "<tr><td>&nbsp;</td></tr>" +
                            "<tr><td align='center' valign='middle'> " +
                            "<img src='" + strHostingServer + "/images/oops_icon.png' width='150' height='150' />" +
                            "</td></tr>" +
                            "<tr><td align='center' class='heading'>Oops!</td> </tr>" +
                            "<tr>" +
                            "<td align='center'>" +
                            "<p>I'm sorry, but HTML entry is not allowed on that page.</p>" +
                            "<p>Please make sure that your entries do not contain any angle brackets like &lt; or &gt;." +
                            " <br /></p>" +
                            "<p>" + strErrorMsg + "</p>" +
                            "</td></tr>" +
                            "<tr><td></td></tr>" +
                            "<tr><td align='center'><a href='javascript:back()'>Go back</a></td></tr>" +
                            "</table>" +
                            "</body></html>");
                            Response.End();
                        }
                        else
                        {
                            HttpContext con = HttpContext.Current;
                            strPageUrl = con.Request.Url.ToString();
                            strPageUrl = Regex.Replace(strPageUrl.ToString(), @"<script(.*?)<\/script>", @"", RegexOptions.IgnoreCase);
                            strPageUrl = HttpUtility.HtmlEncode(strPageUrl);
                            strPageUrl = strPageUrl.Replace("&lt;", "");
                            strPageUrl = strPageUrl.Replace("&gt;", "");

                            Response.StatusCode = 200;
                            Response.Write(@"<html><head><title>System Exception</title> " +
                            "<script> function back() { history.go(-1); } <" + str + "script>" +
                            "<style type='text" + str + "css'> " +
                            "<!-- " +
                            "body { " +
                                "margin-left: 0px; " +
                                "margin-top: 0px; " +
                                "margin-right: 0px; " +
                                "margin-bottom: 0px; " +
                            "} " +
                            ".heading{ " +
                                "font-family:'Trebuchet MS', sans-serif; " +
                                "font-size:42px; " +
                                "color:#333333; " +
                                "padding: 15px 0; " +
                                "text-align:center; " +
                            "} " +
                            "p{ " +
                                "font-family:'Trebuchet MS', sans-serif; " +
                                "font-size:15px; " +
                                "color:#666666; " +
                                "text-align:center; " +
                            "} " +
                            "a{ " +
                                "font-family:'Trebuchet MS', sans-serif; " +
                                "font-size:15px; " +
                                "color:#0080C0; " +
                                "text-align:center; " +
                            "} " +
                            "--> " +
                            "</style> " +
                            "</head>" +
                            "<body style='font-family: Trebuchet MS;'>" +
                            "<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                            "<tr><td>&nbsp;</td></tr>" +
                            "<tr><td align='center' valign='middle'> " +
                            "<img src='" + strHostingServer + "/images/oops_icon.png' width='150' height='150' />" +
                            "</td></tr>" +
                            "<tr><td align='center' class='heading'>Oops!</td> </tr>" +
                            "<tr>" +
                            "<td align='center'>" +
                            "<p>I'm sorry, Something went wrong.</p>" +
                            //"<p>Page URL : " + strPageUrl + "</p>" +
                            //"<p>Error : " + ex.Message + "</p>" +
                            //"<p>Error Code : " + ex.ErrorCode + "</p>" +
                            "<p>" + strErrorMsg + "</p>" +
                            //"<p>Inner Exception : " + ex.InnerException + "</p>" +
                            "</td></tr>" +
                            "<tr><td></td></tr>" +
                            "<tr><td align='center'><a href='javascript:back()'>Go back</a></td></tr>" +
                            "</table>" +
                            "</body></html>");
                            Response.End();
                        }
                    }
                    else if (errorCode.Equals(403) || errorCode.Equals(404))
                    {
                        Response.Redirect(site_url("ErrorNotFound.aspx"), true);
                    }
                    else
                    {
                        HttpContext con = HttpContext.Current;
                        strPageUrl = con.Request.Url.ToString();
                        strPageUrl = Regex.Replace(strPageUrl.ToString(), @"<script(.*?)<\/script>", @"", RegexOptions.IgnoreCase);
                        strPageUrl = HttpUtility.HtmlEncode(strPageUrl);
                        strPageUrl = strPageUrl.Replace("&lt;", "");
                        strPageUrl = strPageUrl.Replace("&gt;", "");

                        Response.StatusCode = 200;
                        Response.Write(@"<html><head><title>Error Has Occured.</title> " +
                        "<script> function back() { history.go(-1); } <" + str + "script>" +
                        "<style type='text" + str + "css'> " +
                        "<!-- " +
                        "body { " +
                            "margin-left: 0px; " +
                            "margin-top: 0px; " +
                            "margin-right: 0px; " +
                            "margin-bottom: 0px; " +
                        "} " +
                        ".heading{ " +
                            "font-family:'Trebuchet MS', sans-serif; " +
                            "font-size:42px; " +
                            "color:#333333; " +
                            "padding: 15px 0; " +
                            "text-align:center; " +
                        "} " +
                        "p{ " +
                            "font-family:'Trebuchet MS', sans-serif; " +
                            "font-size:15px; " +
                            "color:#666666; " +
                            "text-align:center; " +
                        "} " +
                        "a{ " +
                            "font-family:'Trebuchet MS', sans-serif; " +
                            "font-size:15px; " +
                            "color:#0080C0; " +
                            "text-align:center; " +
                        "} " +
                        "--> " +
                        "</style> " +
                        "</head>" +
                        "<body style='font-family: Trebuchet MS;'>" +
                        "<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                        "<tr><td>&nbsp;</td></tr>" +
                        "<tr><td align='center' valign='middle'> " +
                        "<img src='" + strHostingServer + "/images/oops_icon.png' width='150' height='150' />" +
                        "</td></tr>" +
                        "<tr><td align='center' class='heading'>Oops!</td> </tr>" +
                        "<tr>" +
                        "<td align='center'>" +
                        "<p>Something went wrong.</p>" +
                        "<p>Please contact your System administrator.</p>" +
                        //"<p>Page URL : " + strPageUrl + "</p>" +
                        //"<p>Error : " + ex.Message + "</p>" +
                        //"<p>Error Code : " + ex.ErrorCode + "</p>" +
                        "<p>" + strErrorMsg + "</p>" +
                        //"<p>Inner Exception : " + ex.InnerException + "</p>" +
                        "</td></tr>" +
                        "<tr><td></td></tr>" +
                        "<tr><td align='center'><a href='javascript:back()'>Go back</a></td></tr>" +
                        "</table>" +
                        "</body></html>");
                        Response.End();
                    }
                }
                else
                {
                    Exception ex1 = Server.GetLastError() as Exception;
                    if (ex1.Message.Equals("Invalid Session"))
                    {
                        FormsAuthentication.SignOut();
                        Response.StatusCode = 200;
                        Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                        Response.Write(@"<html><head><title>Invalid Session</title> " +
                        "</head><body style='font-family: Trebuchet MS, Sans-serif;'><br /><center><h1>Invalid Session</h1><br />Please close the browser and login again.</center></body></html>");
                        Response.End();
                    }
                }
            }
        }
        // >>

        public static class AppSettings
        {
            public static int FullyQualifiedFileNameLength = (ConfigurationManager.AppSettings.AllKeys.Contains("FullyQualifiedFileNameLength") ? Convert.ToInt32(ConfigurationManager.AppSettings["FullyQualifiedFileNameLength"]) : 174);
        }

        //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
        protected void Application_PreSendRequestHeaders()
        {
            //Response.Headers.Remove("Server");
            //Response.Headers.Set("Server", "FooServer");
            Response.Headers.Remove("X-AspNet-Version");
            //Response.Headers.Remove("X-AspNetMvc-Version");
        }
        //>>

        //<< Added by ramesh more on 13-Mar-2024 CR_1991
        public static List<string> lstInvalidInputs = new List<string>();
        protected void Application_BeginRequest()
        {
            string strRequestedURL = HttpContext.Current.Request.Url.OriginalString;

            if (!strRequestedURL.ToLower().Contains("login.aspx"))
            {
                string sPattern = (ConfigurationManager.AppSettings.AllKeys.Contains("InputValidationRegEx") ? ConfigurationManager.AppSettings["InputValidationRegEx"] : @"^[a-zA-Z0-9\s\-\; \,\._\r\n]*$");
                bool bEnableServerSideInputValidation = (ConfigurationManager.AppSettings.AllKeys.Contains("EnableServerSideInputValidation") ? (ConfigurationManager.AppSettings["EnableServerSideInputValidation"].ToString().Equals("true", StringComparison.CurrentCultureIgnoreCase) ? true : false) : false);
                if (bEnableServerSideInputValidation)
                {
                    foreach (string sFormKey in Request.Form.AllKeys.ToArray())
                    {
                        if (!string.IsNullOrEmpty(sFormKey))
                        {
                            if (!sFormKey.StartsWith("__"))
                            {
                                sPattern = HttpUtility.HtmlDecode(sPattern).Replace("\\\\", "\\");
                                Regex reg = new Regex(sPattern);
                                var res = reg.Match(Request.Form[sFormKey].Replace("=", ""));
                                if (!res.Success)
                                {
                                    if (!lstInvalidInputs.Contains(sFormKey)) { lstInvalidInputs.Add(sFormKey); }
                                }
                            }
                        }
                    }
                }
            }
        }
        //>>

        //protected void Application_BeginRequest()
        //{

        //    string strRequestedURL = HttpContext.Current.Request.Url.OriginalString;

        //    if (!strRequestedURL.ToLower().Contains("login.aspx"))
        //    {
        //        string sPattern = (ConfigurationManager.AppSettings.AllKeys.Contains("InputValidationRegEx") ? ConfigurationManager.AppSettings["InputValidationRegEx"] : @"^[a-zA-Z0-9\s\-\; \,\._\r\n]*$");
        //        bool bEnableServerSideInputValidation = (ConfigurationManager.AppSettings.AllKeys.Contains("EnableServerSideInputValidation") ? (ConfigurationManager.AppSettings["EnableServerSideInputValidation"].ToString().Equals("true", StringComparison.CurrentCultureIgnoreCase) ? true : false) : false);
        //        if (bEnableServerSideInputValidation)
        //        {
        //            foreach (string sFormKey in Request.Form.AllKeys.ToArray())
        //            {
        //                if (!sFormKey.StartsWith("__"))
        //                {
        //                    //Regex reg = new Regex(@"^[a-zA-Z0-9-; ,._:/?'&@()\r\n]*$");
        //                    sPattern = HttpUtility.HtmlDecode(sPattern).Replace("\\\\", "\\");
        //                    Regex reg = new Regex(sPattern);
        //                    var res = reg.Match(Request.Form[sFormKey].Replace("=", ""));
        //                    if (!res.Success)
        //                    {
        //                        if (!lstInvalidInputs.Contains(sFormKey)) { lstInvalidInputs.Add(sFormKey); }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }
}