using AjaxControlToolkit;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact.F2FControls
{
    [ToolboxData("<{0}:F2FFilteredTextbox runat=server></{0}:F2FFilteredTextbox>")]
    [DefaultProperty("ValidChars")]
    [RequiredScript(typeof(CommonToolkitScripts))]
    [TargetControlType(typeof(TextBox))]
    public class F2FFilteredHyperlinks : FilteredTextBoxExtender
    {
        public F2FFilteredHyperlinks()
        {
            string strFilterMode = ConfigurationManager.AppSettings.AllKeys.Contains("HyperlinkFilterMode") ? ConfigurationManager.AppSettings["HyperlinkFilterMode"].ToString() : "ValidChars";
            string strFilterType = ConfigurationManager.AppSettings.AllKeys.Contains("HyperlinkFilterType") ? ConfigurationManager.AppSettings["HyperlinkFilterType"].ToString() : "Custom, Numbers, LowercaseLetters, UppercaseLetters";
            string strValidChars = ConfigurationManager.AppSettings.AllKeys.Contains("HyperlinkValidChars") ? ConfigurationManager.AppSettings["HyperlinkValidChars"].ToString() : "–;— ,-=?._&/";
            string strInValidChars = ConfigurationManager.AppSettings.AllKeys.Contains("HyperlinkInvalidChars") ? ConfigurationManager.AppSettings["HyperlinkInvalidChars"].ToString() : "<>+|@";
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

        protected override bool CheckIfValid(bool throwException)
        {
            return base.CheckIfValid(false);
        }
    }
}
