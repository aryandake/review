using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact.App_Code
{
    public class CommonCodes
    {
        private static string StrDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());

        private static string StrAuthentication = (ConfigurationManager.AppSettings["AuthenticationSetting"].ToString());
        public static string DisplayObjectInfo(Object o)
        {
            StringBuilder sb = new StringBuilder();

            // Include the type of the object
            System.Type type = o.GetType();
            sb.Append("Type: " + type.Name);

            // Include information for each Field
            sb.Append("\r\n\r\nFields:");
            FieldInfo[] fi = type.GetFields();
            if (fi.Length > 0)
            {
                foreach (FieldInfo f in fi)
                {
                    sb.Append("\r\n " + f.ToString() + " = " +
                              f.GetValue(o));
                }
            }
            else
                sb.Append("\r\n None");

            // Include information for each Property
            sb.Append("\r\n\r\nProperties:");
            System.Reflection.PropertyInfo[] pi = type.GetProperties();
            if (pi.Length > 0)
            {
                foreach (PropertyInfo p in pi)
                {
                    sb.Append("\r\n " + p.ToString() + " = " +
                              p.GetValue(o, null));
                }
            }
            else
                sb.Append("\r\n None");

            return sb.ToString();
        }

        public static string dispToDbDate(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(str).ToString("yyyy-MM-dd");
            }
        }
        public static string dispToDbDate(object obj)
        {
            if (obj == null || obj.ToString() == "")
            {

                return null;
            }
            else
            {
                return Convert.ToDateTime(obj).ToString("yyyy-MM-dd");
            }
        }
        public static string dispToDbDateTime(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(str).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public static string dispToDbDateTime(object obj)
        {
            if (obj == null || obj.ToString() == "")
            {

                return null;
            }
            else
            {
                return Convert.ToDateTime(obj).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public static string dispToDbStartDateTime(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(str).ToString("yyyy-MM-dd 00:00:00");
            }
        }
        public static string dispToDbEndDateTime(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(str).ToString("yyyy-MM-dd 23:23:59");
            }
        }

        public static string DbToDispDate(object obj)
        {
            if (obj == DBNull.Value)
            {

                return null;
            }
            else
            {
                return Convert.ToDateTime(obj).ToString("dd-MMM-yyyy");
            }
        }
        public static string DbToDispDateTime(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(str).ToString("dd-MMM-yyyy HH:mm:ss");
            }
        }

        public static string DbToDispDateTime(object obj)
        {
            if (obj == DBNull.Value)
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(obj).ToString("dd-MMM-yyyy HH:mm:ss");
            }
        }

        public static string GetCurrentUrlFileName(System.Web.UI.Page page, bool bFullPath = false)
        {
            if (!string.IsNullOrEmpty(page.Request.Url.AbsolutePath) && !bFullPath)
            {
                return page.Request.Url.AbsolutePath.Split('/')[page.Request.Url.AbsolutePath.Split('/').Length - 1];
            }
            return page.Request.Url.AbsolutePath;
        }

        /// <summary>
        /// This Function Returns all UserIds from AD/MEM filtered with emails
        /// </summary>
        /// <param name="StrEmailFilter">A String to Filter emails</param>
        /// <returns>A String of Comma Separated UserIDs</returns>
        public static string GetUserIdsByEmails(string StrEmailFilter = "")
        {
            List<string> LstUsers = new List<string>();
            if (!string.IsNullOrEmpty(StrEmailFilter))
            {
                string[] ArrEmails = StrEmailFilter.Split(',');
                foreach (string StrEmail in ArrEmails)
                {
                    string strTmpUser = GetUserIdByEmail(StrEmail);
                    if (!string.IsNullOrEmpty(strTmpUser) && !LstUsers.Contains(strTmpUser))
                    {
                        LstUsers.Add(strTmpUser);
                    }
                }
            }
            return string.Join(",", LstUsers.ToArray());
        }

        /// <summary>
        /// This Function Returns all Emails from AD/MEM filtered with StrUserNames
        /// </summary>
        /// <param name="StrUserNames"></param>
        /// <returns>A String of Comma Separated Emails</returns>
        public static string GetEmailsByUserIds(string StrUserNames = "")
        {
            List<string> LstEmails = new List<string>();
            string[] ArrUsers = StrUserNames.Split(',');
            if (!string.IsNullOrEmpty(StrUserNames))
            {
                if (StrAuthentication.Equals("MEM"))
                {
                    foreach (string StrUser in ArrUsers)
                    {
                        MembershipUser TmpUser = Membership.GetUser(StrUser);
                        if (TmpUser != null) { LstEmails.Add(TmpUser.Email); }
                    }
                }
                else
                {
                    StrDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());
                    DirectoryEntry entry = new DirectoryEntry(StrDirectoryEntry);
                    foreach (string StrUser in ArrUsers)
                    {
                        DirectorySearcher adSearcher = new DirectorySearcher(StrDirectoryEntry);
                        adSearcher.Filter = "(&(objectClass=User)(| (displayName = *" + StrUser + "*)(givenName = *" + StrUser + "*)(sn = *" + StrUser + "*)))";
                        SearchResult user = adSearcher.FindOne();
                        LstEmails.Add(GetProperty(user, "mail"));
                    }
                }
            }
            return string.Join(",", LstEmails.ToArray());
        }

        public static string GetUserIdByEmail(string StrEmail)
        {
            string StrUserId = null;
            try
            {
                if (StrAuthentication.Equals("MEM"))
                {
                    return Membership.GetUserNameByEmail(StrEmail);
                }
                else
                {
                    StrDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());
                    DirectoryEntry entry = new DirectoryEntry(StrDirectoryEntry);
                    DirectorySearcher adSearcher = new DirectorySearcher(StrDirectoryEntry);
                    adSearcher.Filter = ("mail=" + StrEmail);
                    SearchResult user = adSearcher.FindOne();
                    StrUserId = GetProperty(user, "name");
                }
                return StrUserId;
            }
            catch (Exception ex)
            {

                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                return StrUserId;
            }
        }
        public static string GetEmailByUserId(string StrUserId)
        {
            string StrEmail = null;
            try
            {
                if (StrAuthentication.Equals("MEM"))
                {
                    MembershipUser user = Membership.GetUser(StrUserId);
                    return user == null ? null : user.Email;
                }
                else
                {
                    StrDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());
                    DirectoryEntry entry = new DirectoryEntry(StrDirectoryEntry);
                    DirectorySearcher adSearcher = new DirectorySearcher(StrDirectoryEntry);
                    adSearcher.Filter = ("mail=" + StrUserId);
                    SearchResult user = adSearcher.FindOne();
                    StrEmail = GetProperty(user, "mail");
                }
                return StrEmail;
            }
            catch (Exception ex)
            {

                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                return StrEmail;
            }
        }

        public static List<string> GetAllEmailByFilter(string StrFilter)
        {
            List<string> emails = new List<string>();
            try
            {
                if (StrFilter.Trim().Length != 0)
                {
                    if (StrAuthentication.Equals("MEM"))
                    {

                        var mu = Membership.FindUsersByEmail("%" + StrFilter.Trim() + "%");
                        foreach (MembershipUser u in mu)
                        {
                            if (!string.IsNullOrEmpty(u.Email))
                            {
                                emails.Add(u.Email);
                            }
                        }
                    }
                    else
                    {
                        StrDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());
                        DirectoryEntry entry = new DirectoryEntry(StrDirectoryEntry);
                        DirectorySearcher adSearcher = new DirectorySearcher(StrDirectoryEntry);
                        adSearcher.Filter = "(&(objectClass=User) (mail=*" + StrFilter + "*))";

                        SearchResultCollection results = adSearcher.FindAll();
                        if (results != null)
                        {
                            foreach (SearchResult res in results)
                            {
                                string TmpMail = GetProperty(res, "mail");
                                if (!string.IsNullOrEmpty(TmpMail))
                                {
                                    emails.Add(TmpMail);
                                }
                            }
                        }
                    }
                }
                return emails;
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                return emails;
            }
        }

        public static string GetProperty(SearchResult searchResult, string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static void PrepareGridViewForExport(Control gv)
        {
            LinkButton lb = new LinkButton();
            Literal l = new Literal();
            string name = String.Empty;
            for (int i = 0; i < gv.Controls.Count; i++)
            {
                if (gv.Controls[i].GetType() == typeof(LinkButton))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                //if (gv.Controls[i].GetType() == typeof(DataList))
                //{
                //    gv.Controls.Remove(gv.Controls[i]);
                //}
                else if (gv.Controls[i].GetType() == typeof(ImageField))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(ImageButton))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(Image))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(HyperLink))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(TextBox))
                {
                    l.Text = (gv.Controls[i] as TextBox).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(CheckBox))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(DropDownList))
                {
                    l.Text = (gv.Controls[i] as DropDownList).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(RequiredFieldValidator))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(RadioButtonList))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(HiddenField))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(Label))
                {
                    l.Text = (gv.Controls[i] as Label).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(DataList))
                {
                    DataList gvChlid = (DataList)gv.Controls[i];
                    List<string> lstFiles = new List<string>();
                    PrepareChlidGridView(gvChlid, ref lstFiles);
                    gv.Controls.Remove(gv.Controls[i]);
                    foreach (string str in lstFiles)
                    {
                        l.Text += (string.IsNullOrEmpty(l.Text) ? "" : "<br style=\"mso-data-placement:same-cell;\">") + str;
                    }
                    gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].HasControls())
                {
                    Control ctrl = gv.Controls[i];
                    PrepareGridViewForExport(ctrl);
                }
            }
        }
        public static void PrepareChlidGridView(Control gv, ref List<string> lstFiles)
        {
            Literal l = new Literal();

            string name = String.Empty;

            for (int i = 0; i < gv.Controls.Count; i++)
            {
                if (gv.Controls[i].GetType() == typeof(LinkButton))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(Image))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }

                else if (gv.Controls[i].GetType() == typeof(HyperLink))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }

                else if (gv.Controls[i].GetType() == typeof(TextBox))
                {
                    lstFiles.Add((gv.Controls[i] as TextBox).Text);
                    gv.Controls.Remove(gv.Controls[i]);

                }

                else if (gv.Controls[i].GetType() == typeof(CheckBox))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }

                else if (gv.Controls[i].GetType() == typeof(DropDownList))
                {
                    lstFiles.Add((gv.Controls[i] as DropDownList).Text);
                    gv.Controls.Remove(gv.Controls[i]);

                }

                else if (gv.Controls[i].GetType() == typeof(RequiredFieldValidator))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }

                else if (gv.Controls[i].GetType() == typeof(ImageField))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }

                else if (gv.Controls[i].GetType() == typeof(RadioButtonList))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }

                else if (gv.Controls[i].GetType() == typeof(HiddenField))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }

                else if (gv.Controls[i].GetType() == typeof(Label))
                {
                    lstFiles.Add((gv.Controls[i] as Label).Text);
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(DataBoundLiteralControl))
                {
                    lstFiles.Add(GetPlainTextFromHTML((gv.Controls[i] as DataBoundLiteralControl).Text));
                    gv.Controls.Remove(gv.Controls[i]);
                }

                else if (gv.Controls[i].HasControls())
                {
                    PrepareChlidGridView(gv.Controls[i], ref lstFiles);
                }
            }
        }
        public static void SetDropDownDataSource(Control ddList, DataTable DtSource = null, string sValueFileld = null, string sTextField = null, string sSelected = null)
        {
            DropDownList ddLst = ddList as DropDownList;
            if (ddLst != null)
            {
                if (DtSource != null)
                {
                    if (DtSource.Rows.Count > 0)
                    {
                        ddLst.DataSource = DtSource;
                        if (!string.IsNullOrEmpty(sValueFileld) && !string.IsNullOrEmpty(sTextField))
                        {
                            ddLst.DataValueField = sValueFileld;
                            ddLst.DataTextField = sTextField;
                        }
                        ddLst.DataBind();
                        ddLst.Items.Insert(0, new ListItem("(Select)", ""));
                    }
                }

                if (!string.IsNullOrEmpty(sSelected))
                {
                    if (null != ddLst.Items.FindByValue(sSelected))
                    {
                        ddLst.SelectedValue = sSelected;
                    }
                }
            }
        }

        public static CheckBoxList SetCheckboxDataSource(CheckBoxList cbList, DataTable DtSource = null, string sValueFileld = null, string sTextField = null, string[] arrValues = null)
        {
            if (cbList != null)
            {
                if (DtSource != null)
                {
                    if (DtSource.Rows.Count > 0)
                    {
                        cbList.DataSource = DtSource;
                        if (!string.IsNullOrEmpty(sValueFileld) && !string.IsNullOrEmpty(sTextField))
                        {
                            cbList.DataValueField = sValueFileld;
                            cbList.DataTextField = sTextField;
                        }
                        cbList.DataBind();
                    }
                }

                cbList = getCheckboxSelectedValuesFromArray(arrValues, cbList);
            }

            return cbList;
        }

        public static ListItem[] GetYesNoDDLItems()
        {
            ListItem[] liYesNo = new ListItem[3];
            int i = 0;
            liYesNo[i++] = new ListItem("(Select)", "");
            liYesNo[i++] = new ListItem("Yes", "Y");
            liYesNo[i++] = new ListItem("No", "N");
            return liYesNo;
        }

        public static string getCommaSeparatedValuesFromCheckboxList(CheckBoxList cbList)
        {
            string strValues = "";
            ListItem liChkBoxListItem;

            if (cbList != null)
            {
                for (int i = 0; i <= cbList.Items.Count - 1; i++)
                {
                    liChkBoxListItem = cbList.Items[i];

                    if (liChkBoxListItem.Selected)
                    {
                        strValues = (string.IsNullOrEmpty(strValues) ? "" : strValues + ",") + liChkBoxListItem.Value;
                    }
                }
            }

            return strValues;
        }

        public static CheckBoxList getCheckboxSelectedValuesFromArray(string[] arrValues, CheckBoxList cblValues)
        {
            if (arrValues != null)
            {
                for (int i = 0; i < arrValues.Length; i++)
                {
                    if (!arrValues[i].Equals(""))
                    {
                        cblValues.Items.FindByValue(arrValues[i]).Selected = true;
                    }
                }
            }

            return cblValues;
        }

        public static string GetPlainTextFromHTML(string HtmlString, bool bRemoveLineBreak = false)
        {
            string strHtmlPattern = "<.*?>";
            var regEx = new Regex("(<script(.+?))|(<style(.+?))", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            HtmlString = regEx.Replace(HtmlString, string.Empty);
            HtmlString = Regex.Replace(HtmlString, strHtmlPattern, string.Empty);
            HtmlString = Regex.Replace(HtmlString, @"\s+", " ");
            if (bRemoveLineBreak)
            {
                HtmlString = Regex.Replace(HtmlString, @"^\s+$[\r\n]*", ""); //, RegexOptions.Multiline);
            }
            return HtmlString;
        }

        public static void SetFilterControlProperties(Control ctrlInput, string strFilterMode, string strFilterType, string strValidChars, string strInValidChars)
        {
            FilteredTextBoxExtender fte = (FilteredTextBoxExtender)ctrlInput;
            fte.ValidChars = strValidChars;
            fte.InvalidChars = strInValidChars;
            fte.FilterMode = strFilterMode.Equals("InvalidChars", StringComparison.CurrentCultureIgnoreCase) ? FilterModes.InvalidChars : FilterModes.ValidChars;

            string[] arrFilterType = strFilterType.Split(',');

            foreach (string strFilter in arrFilterType)
            {
                if (strFilter.Trim().Equals("Custom", StringComparison.CurrentCultureIgnoreCase))
                {
                    fte.FilterType |= FilterTypes.Custom;
                }
                else if (strFilter.Trim().Equals("Numbers", StringComparison.CurrentCultureIgnoreCase))
                {
                    fte.FilterType |= FilterTypes.Numbers;
                }
                else if (strFilter.Trim().Equals("LowercaseLetters", StringComparison.CurrentCultureIgnoreCase))
                {
                    fte.FilterType |= FilterTypes.LowercaseLetters;
                }
                else if (strFilter.Trim().Equals("UppercaseLetters", StringComparison.CurrentCultureIgnoreCase))
                {
                    fte.FilterType |= FilterTypes.UppercaseLetters;
                }
            }
        }

        public static bool IsCsvVulnerable(string str)
        {
            if (str.StartsWith("+") || str.StartsWith("-") || str.StartsWith("=") || str.StartsWith("@") || str.StartsWith("`"))
            {
                return true;
            }
            string strPattern = ConfigurationManager.AppSettings.AllKeys.Contains("CmRegEx") ? ConfigurationManager.AppSettings["CmRegEx"] : @"([\*\#\^\|=<>\/])";
            Regex reg = new Regex(strPattern);
            bool bRes = reg.IsMatch(str);
            return bRes;
        }
        static string sExcludeInputs = "FCKE_";
        public static bool CheckInputValidity(Page p)
        {
            string sScript = string.Empty;
            foreach (string sInput in Global.lstInvalidInputs)
            {
                if (!sInput.Contains(sExcludeInputs) && !sInput.Contains("$hf"))
                {
                    string sErrorSpan = "<br /><span class=\"text-danger\">Invalid Input detected</span>";
                    sScript += "$('" + sErrorSpan + "').insertAfter($('[name=\"" + sInput + "\"]'));";
                }
            }
            Global.lstInvalidInputs = new List<string>();
            if (!string.IsNullOrEmpty(sScript))
            {
                p.ClientScript.RegisterStartupScript(p.GetType(), "", sScript, true);
                return false;
            }
            return true;
        }

        public static void SetDropDownDataSourceForEdit(Control ddList, DataTable DtSource, string sStatus)
        {
            DropDownList ddLst = ddList as DropDownList;
            DataRow dr;
            if (ddLst != null)
            {
                if (DtSource != null)
                {
                    if (DtSource.Rows.Count > 0)
                    {
                        ddLst.DataSource = DtSource;
                        ddLst.DataBind();
                        
                        for (int i = 0; i < DtSource.Rows.Count; i++)
                        {
                            dr = DtSource.Rows[i];
                            if (dr[sStatus].ToString().Equals("I"))
                                ddLst.Items[i].Attributes["disabled"] = "disabled";
                        }

                        ddLst.Items.Insert(0, new ListItem("(Select)", ""));
                    }
                }
            }
        }

        public static CheckBoxList SetCheckboxDataSourceForEdit(CheckBoxList cbList, DataTable DtSource, string sStatus)
        {
            DataRow dr;
            if (cbList != null)
            {
                if (DtSource != null)
                {
                    if (DtSource.Rows.Count > 0)
                    {
                        cbList.DataSource = DtSource;
                        cbList.DataBind();

                        for (int i = 0; i < DtSource.Rows.Count; i++)
                        {
                            dr = DtSource.Rows[i];
                            if (dr[sStatus].ToString().Equals("I"))
                            {
                                cbList.Items[i].Attributes["disabled"] = "disabled";
                                cbList.Items[i].Enabled = false;
                            }
                        }

                    }
                }
            }

            return cbList;
        }


    }
}