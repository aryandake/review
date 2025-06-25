using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Fiction2Fact.App_Code;
using static Fiction2Fact.App_Code.F2FDatabase;
using System.Configuration;
using System.DirectoryServices;

namespace Fiction2Fact.Legacy_App_Code
{
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class AJAXDropdown : System.Web.Services.WebService
    {
        public AJAXDropdown()
        {
        }

        [WebMethod]
        public List<string> getUserDetailsbyPhoneBook(string prefixText)
        {
            string strAuthenticationSetting = ConfigurationManager.AppSettings["AuthenticationSetting"].ToString();
            List<string> list = new List<string>();

            string mstrADUserNamePropertyField = ConfigurationManager.AppSettings.AllKeys.Any(key => key.ToUpper() == "ADUSERNAMEPROPERTYFIELD") ?
                                                    ConfigurationManager.AppSettings["ADUserNamePropertyField"].ToString() : "name";

            if (strAuthenticationSetting.Equals("MEM"))
            {
                //int i = 0;
                DataTable dt = new DataTable();
                DataRow dr;

                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = " SELECT * FROM ( " +
                                                " SELECT [UserName] + ' - IT' as [Name], [UserName] + '|' + [UserName] + '|' + [Email] + '|IT' as [ConcatenateString] " +
                                                " From [aspnet_Users] au " +
                                                " inner join [aspnet_Membership] am on au.UserId = am.UserId " +
                                                " Where [UserName] like @SearchText + '%' ) tab ";

                    DB.F2FCommand.CommandType = CommandType.Text;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@SearchText", F2FDatabase.F2FDbType.VarChar, prefixText));
                    DB.OpenConnection();
                    DB.F2FDataAdapter.Fill(dt);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = dt.Rows[i];

                        string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dr["Name"].ToString(), dr["ConcatenateString"].ToString());
                        list.Add(item);
                    }
                }
            }
            else
            {
                if (prefixText.Trim().Length > 2)
                {
                    string strDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());
                    DirectoryEntry entry = new DirectoryEntry(strDirectoryEntry);
                    DirectorySearcher adSearcher = new DirectorySearcher(strDirectoryEntry);
                    adSearcher.Filter = "(&(objectClass=User) (name=" + prefixText + "*))";

                    SearchResultCollection results = adSearcher.FindAll();
                    if (results != null)
                    {
                        foreach (SearchResult res in results)
                        {
                            string strADUserName = CommonCodes.GetProperty(res, mstrADUserNamePropertyField);
                            string strADUserDepartment = CommonCodes.GetProperty(res, "department");
                            string strADUserId = CommonCodes.GetProperty(res, "samAccountName");
                            string strADUserEmail = CommonCodes.GetProperty(res, "mail");
                            if (!string.IsNullOrEmpty(strADUserName))
                            {
                                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(strADUserName + " - " + strADUserDepartment, strADUserId + "|" + strADUserName + "|" + strADUserEmail + "|" + strADUserDepartment);
                                list.Add(item);
                                //list.Add(string.Format("{0}~{1}", strADUserName + " - " + strADUserDepartment, strADUserId + "|" + strADUserName + "|" + strADUserEmail + "|" + strADUserDepartment));
                            }
                        }
                    }
                }
            }
            return list;
        }

        [WebMethod]
        public List<string> getUserEmailIdbyPhoneBook(string prefixText)
        {
            string strAuthenticationSetting = ConfigurationManager.AppSettings["AuthenticationSetting"].ToString();
            List<string> list = new List<string>();

            string mstrADUserNamePropertyField = ConfigurationManager.AppSettings.AllKeys.Any(key => key.ToUpper() == "ADUSERNAMEPROPERTYFIELD") ?
                                                    ConfigurationManager.AppSettings["ADUserNamePropertyField"].ToString() : "name";

            if (strAuthenticationSetting.Equals("MEM"))
            {
                //int i = 0;
                DataTable dt = new DataTable();
                DataRow dr;

                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = " SELECT * FROM ( " +
                                                " SELECT [Email] as [EmailId] " +
                                                " From [aspnet_Users] au " +
                                                " inner join [aspnet_Membership] am on au.UserId = am.UserId " +
                                                " Where [UserName] like @SearchText + '%' ) tab ";

                    DB.F2FCommand.CommandType = CommandType.Text;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@SearchText", F2FDatabase.F2FDbType.VarChar, prefixText));
                    DB.OpenConnection();
                    DB.F2FDataAdapter.Fill(dt);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = dt.Rows[i];

                        string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dr["EmailId"].ToString(), dr["EmailId"].ToString());
                        list.Add(item);
                    }
                }
            }
            else
            {
                if (prefixText.Trim().Length > 2)
                {
                    string strDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());
                    DirectoryEntry entry = new DirectoryEntry(strDirectoryEntry);
                    DirectorySearcher adSearcher = new DirectorySearcher(strDirectoryEntry);

                    string ADEmailSearchFlag = (ConfigurationManager.AppSettings["ADEmailSearchFlag"].ToString());

                    if (ADEmailSearchFlag == "0")
                        adSearcher.Filter = "(&(objectClass=User) (" + mstrADUserNamePropertyField + "=" + prefixText + "*))";
                    else
                        adSearcher.Filter = "(&(objectClass=User) (mail=" + prefixText + "*))";

                    SearchResultCollection results = adSearcher.FindAll();
                    if (results != null)
                    {
                        foreach (SearchResult res in results)
                        {
                            string strADUserName = CommonCodes.GetProperty(res, mstrADUserNamePropertyField);
                            string strADUserDepartment = CommonCodes.GetProperty(res, "department");
                            string strADUserId = CommonCodes.GetProperty(res, "samAccountName");
                            string strADUserEmail = CommonCodes.GetProperty(res, "mail");
                            if (!string.IsNullOrEmpty(strADUserName))
                            {
                                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(strADUserEmail, strADUserEmail);
                                list.Add(item);
                            }
                        }
                    }
                }
            }
            return list;
        }
    }
}
