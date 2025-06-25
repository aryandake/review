using AjaxControlToolkit;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact.F2FControls
{
    [ToolboxData("<{0}:F2FFilteredEmail runat=server></{0}:F2FFilteredEmail>")]
    [DefaultProperty("ValidChars")]
    [RequiredScript(typeof(CommonToolkitScripts))]
    [TargetControlType(typeof(TextBox))]
    public class F2FFilteredEmail : FilteredTextBoxExtender
    {
        //[ClientPropertyName("filterInterval")]
        //[DefaultValue(250)]
        //[ExtenderControlProperty]
        //public int FilterInterval { get; set; }
        //[ClientPropertyName("filterMode")]
        //[DefaultValue(FilterModes.ValidChars)]
        //[ExtenderControlProperty]
        //public FilterModes FilterMode { get; set; }
        //[ClientPropertyName("filterType")]
        ////[DefaultValue(FilterTypes.Custom)]
        //[ExtenderControlProperty]
        //public FilterTypes FilterType { get; set; }
        //[ClientPropertyName("invalidChars")]
        //[DefaultValue("")]
        //[ExtenderControlProperty]
        //public string InvalidChars { get; set; }
        //[ClientPropertyName("validChars")]
        //[DefaultValue("")]
        //[ExtenderControlProperty]
        //public string ValidChars { get; set; }
        public F2FFilteredEmail()
        {
            string strFilterMode = ConfigurationManager.AppSettings.AllKeys.Contains("EmailFilterMode") ? ConfigurationManager.AppSettings["EmailFilterMode"].ToString() : "ValidChars";
            string strFilterType = ConfigurationManager.AppSettings.AllKeys.Contains("EmailFilterType") ? ConfigurationManager.AppSettings["EmailFilterType"].ToString() : "Custom, Numbers, LowercaseLetters, UppercaseLetters";
            string strValidChars = ConfigurationManager.AppSettings.AllKeys.Contains("EmailValidChars") ? ConfigurationManager.AppSettings["EmailValidChars"].ToString() : "_-.@";
            string strInValidChars = ConfigurationManager.AppSettings.AllKeys.Contains("EmailInvalidChars") ? ConfigurationManager.AppSettings["EmailInvalidChars"].ToString() : "-=<>+|@";
            ValidChars = strValidChars;
            InvalidChars = strInValidChars;
            FilterMode = strFilterMode.Equals("InvalidChars", StringComparison.CurrentCultureIgnoreCase) ? FilterModes.InvalidChars : FilterModes.ValidChars;

            string[] arrFilterType = strFilterType.Split(',');

            foreach (string strFilter in arrFilterType)
            {
                if (strFilter.Trim().Equals("Custom", StringComparison.CurrentCultureIgnoreCase))
                {
                    FilterType |= FilterTypes.Custom;
                }
                else if (strFilter.Trim().Equals("Numbers", StringComparison.CurrentCultureIgnoreCase))
                {
                    FilterType |= FilterTypes.Numbers;
                }
                else if (strFilter.Trim().Equals("LowercaseLetters", StringComparison.CurrentCultureIgnoreCase))
                {
                    FilterType |= FilterTypes.LowercaseLetters;
                }
                else if (strFilter.Trim().Equals("UppercaseLetters", StringComparison.CurrentCultureIgnoreCase))
                {
                    FilterType |= FilterTypes.UppercaseLetters;
                }
            }
        }
        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //}
        //protected override void RenderInnerScript(ScriptBehaviorDescriptor descriptor)
        //{
        //    base.RenderInnerScript(descriptor);
        //}
        //protected override void RenderScriptAttributes(ScriptBehaviorDescriptor descriptor)
        //{
        //    base.RenderScriptAttributes(descriptor);
        //}
        //protected override void OnResolveControlID(ResolveControlEventArgs e)
        //{
        //    base.OnResolveControlID(e);
        //}
        protected override bool CheckIfValid(bool throwException)
        {
            return base.CheckIfValid(false);
        }
    }
}
