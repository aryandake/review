using Fiction2Fact.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Fiction2Fact.Helpers
{
    public class F2FHelper : HtmlHelper
    {
        public F2FHelper(ViewContext viewContext, IViewDataContainer viewDataContainer) : base(viewContext, viewDataContainer)
        {
            
        }

        public F2FHelper(ViewContext viewContext, IViewDataContainer viewDataContainer, RouteCollection routeCollection) : base(viewContext, viewDataContainer, routeCollection)
        {
        }
        public static string GetSessionMsg(string sSessionKey = null, bool BPlainText=false)
        {
            Type sessionKeys = typeof(F2FConstants.SessionMsgKeys);
            string sMvcHtmlString = null;
            if (string.IsNullOrEmpty(sSessionKey))
            {
                foreach (var key in sessionKeys.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic))
                {
                    string sSessionValue = key.GetValue(null).ToString();
                    if (!string.IsNullOrEmpty(sSessionValue) && HttpContext.Current.Session[sSessionValue] != null)
                    {
                        TagBuilder tbSpan = new TagBuilder("span");
                        switch (sSessionValue)
                        {
                            case "SaveMsgError":
                                tbSpan.AddCssClass("alert-danger f2f-danger");
                                break;
                            case "SaveMsgSuccess":
                            default:
                                tbSpan.AddCssClass("alert-success f2f-success");
                                break;
                        }
                        if (!BPlainText)
                        {
                            tbSpan.SetInnerText(HttpContext.Current.Session[sSessionValue].ToString());
                            sMvcHtmlString += (string.IsNullOrEmpty(sMvcHtmlString) ? "" : "<br>") + (new MvcHtmlString(tbSpan.ToString())).ToString();
                        }
                        else
                        {
                            sMvcHtmlString += HttpContext.Current.Session[sSessionValue].ToString() + "<br>";
                        }
                        HttpContext.Current.Session.Remove(sSessionValue);
                    }
                }
            }
            else
            {
                if (!BPlainText)
                {
                    TagBuilder tbSpan = new TagBuilder("span");
                    switch (sSessionKey)
                    {
                        case "SaveMsgError":
                            tbSpan.AddCssClass("alert-danger f2f-danger");
                            break;
                        case "SaveMsgSuccess":
                        default:
                            tbSpan.AddCssClass("alert-success f2f-success");
                            break;
                    }
                    tbSpan.SetInnerText((HttpContext.Current.Session[sSessionKey] == null) ? "" : HttpContext.Current.Session[sSessionKey].ToString());
                    HttpContext.Current.Session.Remove(sSessionKey);
                    sMvcHtmlString += (string.IsNullOrEmpty(sMvcHtmlString) ? "" : "<br>") + (new MvcHtmlString(tbSpan.ToString())).ToString();
                }
                else
                {
                    sMvcHtmlString = (HttpContext.Current.Session[sSessionKey]==null) ? "" : HttpContext.Current.Session[sSessionKey].ToString();
                }
            }
            HttpContext.Current.Session.Remove(sSessionKey);
            return sMvcHtmlString;
        }
        public static void SetSessionKey(string sessionKey, object sessionValue)
        {
            HttpContext.Current.Session[sessionKey] = sessionValue;
        }
        public static object GetSessionKey(string sessionKey)
        {
            if (HttpContext.Current.Session[sessionKey] == null)
            {
                return null;
            }
            return HttpContext.Current.Session[sessionKey];
        }
    }
}