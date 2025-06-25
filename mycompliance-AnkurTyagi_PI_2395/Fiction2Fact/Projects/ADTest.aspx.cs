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
using System.DirectoryServices;

public partial class ADTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strDirectoryEntry = "LDAP://asia.intl.cigna.com";//"LDAP://10.150.1.28"; //"LDAP://bsli.com/DC=bsli,DC=com";//LDAP://aegonreligare.com/DC=aegonreligare,DC=com
        DirectoryEntry entry = new DirectoryEntry(strDirectoryEntry);
        DirectorySearcher dSearch = new DirectorySearcher(entry);
        string strUserDetails = "";
        string strUserName = "", strDepartment = "", strEmail = "";
        string strContact = "", strDesignation = "";
        string strUserId = "priyanka";//"BSLI\INOS002233", "INOS002233"//AEGONRELIGARE\99595

        strUserId = strUserId.Substring(strUserId.IndexOf("\\") + 1);
        dSearch.Filter = "(&(samAccountName=" + strUserId + ")(objectCategory=person)(objectClass=user))";
        foreach (SearchResult sResultSet in dSearch.FindAll())
        {
            strUserName = GetProperty(sResultSet, "name");
            strDepartment = GetProperty(sResultSet, "department");
            strEmail = GetProperty(sResultSet, "mail");
            strContact = GetProperty(sResultSet, "telephonenumber");
            strDesignation = GetProperty(sResultSet, "title");
        }
        strUserDetails = strUserName + "|" + strDepartment + "|" + strContact + "|" +
                         strEmail + "|" + strDesignation;
        Response.Write(strUserDetails);
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
}
