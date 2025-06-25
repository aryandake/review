using System;
using System.Configuration;
using System.Web.Security;
using System.DirectoryServices;
using Fiction2Fact.App_Code;
using System.Linq;

/// <summary>
/// Summary description for Authentication
/// </summary>
namespace Fiction2Fact.Legacy_App_Code
{
    public class Authentication
    {
        string strDirectoryEntry;

        private string mstrAuthentication = (ConfigurationManager.AppSettings["AuthenticationSetting"].ToString());

        //<< Added by Vivek on 11-Mar-2021
        private static string mstrADUserNamePropertyField = ConfigurationManager.AppSettings.AllKeys.Any(key => key.ToUpper() == "ADUSERNAMEPROPERTYFIELD") ?
                                                ConfigurationManager.AppSettings["ADUserNamePropertyField"].ToString() : "name";
        //>>

        public string GetUserDetails(string strUserId)
        {
            string strEmailId = "", NameAndEmail = "", strName = "",
                strEmail = "";
            try
            {
                if (mstrAuthentication.Equals("MEM"))
                {
                    MembershipUser user;
                    user = Membership.GetUser(strUserId);
                    strEmailId = user.Email;
                    NameAndEmail = strUserId + "," + strEmailId;
                }
                else
                {
                    strDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());
                    DirectoryEntry entry = new DirectoryEntry(strDirectoryEntry);
                    DirectorySearcher dSearch = new DirectorySearcher(entry);

                    strUserId = strUserId.Substring(strUserId.IndexOf("\\") + 1);
                    dSearch.Filter = "(&(samAccountName=" + strUserId + ")(objectCategory=person)(objectClass=user))";
                    foreach (SearchResult sResultSet in dSearch.FindAll())
                    {
                        // Login Name
                        strName = GetProperty(sResultSet, mstrADUserNamePropertyField);
                        strEmail = GetProperty(sResultSet, "mail");
                        NameAndEmail = strName.Replace(",", "") + "," + strEmail;
                    }
                }
                return NameAndEmail;
            }
            catch (Exception ex)
            {
                NameAndEmail = strName + "," + strEmail;
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                return NameAndEmail;
            }
        }

        public string GetDetailsForUserCreation(string strUserId)
        {
            string strEmailId = "";
            string strUserDetails = "";
            string strUserName = "", strDepartment = "", strEmail = "";
            string strContact = "", strDesignation = "";
            try
            {
                if (mstrAuthentication.Equals("MEM"))
                {
                    MembershipUser user;
                    user = Membership.GetUser(strUserId);
                    strEmailId = user.Email;
                    strUserDetails = strUserId + "|" + strDepartment + "|" + strEmailId + "|" +
                                     strContact + "|" + strDesignation;
                }
                else
                {
                    strDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());
                    DirectoryEntry entry = new DirectoryEntry(strDirectoryEntry);
                    DirectorySearcher dSearch = new DirectorySearcher(entry);
                    strUserId = strUserId.Substring(strUserId.IndexOf("\\") + 1);
                    dSearch.Filter = "(&(samAccountName=" + strUserId + ")(objectCategory=person)(objectClass=user))";
                    foreach (SearchResult sResultSet in dSearch.FindAll())
                    {
                        strUserName = GetProperty(sResultSet, mstrADUserNamePropertyField);
                        strDepartment = GetProperty(sResultSet, "department");
                        strEmail = GetProperty(sResultSet, "mail");
                        strContact = GetProperty(sResultSet, "telephonenumber");
                        strDesignation = GetProperty(sResultSet, "title");
                    }
                    strUserDetails = strUserName.Replace(",", "") + "|" + strDepartment + "|" + strEmail + "|" +
                                     strContact + "|" + strDesignation;
                }
                return strUserDetails;
            }
            catch (Exception ex)
            {
                strUserDetails = strUserName + "|" + strDepartment + "|" + strEmail + "|" +
                                     strContact + "|" + strDesignation;
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                return strUserDetails;
            }
        }

        public static string GetUserIDWithDomain(string strUserId)
        {
            try
            {
                string strADDomain = (ConfigurationManager.AppSettings["ActiveDirectoryDomain"].ToString());
                return strADDomain + "\\" + strUserId;
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public static string GetUserID(string strUserId)
        {
            try
            {
                string strUserName;
                return strUserName = strUserId.Substring(strUserId.IndexOf("\\") + 1);
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
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

        public string getUserFullName(string strUserId)
        {
            string strUserFullName = "", strUserName = "";
            try
            {
                if (mstrAuthentication.Equals("MEM"))
                {
                    return strUserId + "(" + strUserId + ")";
                }
                else
                {
                    if (!Convert.ToString(strUserId.Substring(strUserId.IndexOf("\\") + 1)).Equals(""))
                    {
                        strDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());
                        DirectoryEntry entry = new DirectoryEntry(strDirectoryEntry);
                        DirectorySearcher dSearch = new DirectorySearcher(entry);
                        strUserId = strUserId.Substring(strUserId.IndexOf("\\") + 1);
                        dSearch.Filter = "(&(samAccountName=" + strUserId + ")(objectCategory=person)(objectClass=user))";
                        foreach (SearchResult sResultSet in dSearch.FindAll())
                        {
                            strUserName = GetProperty(sResultSet, mstrADUserNamePropertyField);
                        }
                        strUserFullName = strUserName.Replace(",", "") + "(" + strUserId.Substring(strUserId.IndexOf("\\") + 1) + ")";
                    }
                    else
                    {
                        strUserFullName = "";
                    }
                }
                return strUserFullName;
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        public string getEmailIDOnUserId(string strUserId)
        {
            string strEmailId = "";

            if (mstrAuthentication.Equals("MEM"))
            {
                MembershipUser user;
                user = Membership.GetUser(strUserId);
                if (user != null)
                {
                    strEmailId = user.Email;
                }
            }
            else
            {
                strDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());
                DirectoryEntry entry = new DirectoryEntry(strDirectoryEntry);
                DirectorySearcher dSearch = new DirectorySearcher(entry);
                strUserId = strUserId.Substring(strUserId.IndexOf("\\") + 1);

                dSearch.Filter = "(&(samAccountName=" + strUserId + ")(objectCategory=person)(objectClass=user))";

                foreach (SearchResult sResultSet in dSearch.FindAll())
                {
                    strEmailId = GetProperty(sResultSet, "mail");
                }
            }
            return strEmailId;
        }

        public string GetUserDetsByEmpCode(string strEmpCode)
        {
            string strUserDetails = "", strUserName = "", strEmail = "",
                strDepartment = "", strContact = "", strDesignation = "";
            try
            {
                if (mstrAuthentication.Equals("MEM"))
                {
                    MembershipUser user;
                    user = Membership.GetUser(strEmpCode);
                    strEmail = user.Email;
                    strUserDetails = strEmpCode + "|" + strEmail;
                }
                else
                {
                    strDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());
                    DirectoryEntry entry = new DirectoryEntry(strDirectoryEntry);
                    DirectorySearcher dSearch = new DirectorySearcher(entry);
                    strEmpCode = strEmpCode.Substring(strEmpCode.IndexOf("\\") + 1);

                    dSearch.Filter = "(&(samAccountName=" + strEmpCode + ")(objectCategory=person)(objectClass=user))";

                    foreach (SearchResult sResultSet in dSearch.FindAll())
                    {
                        strUserName = GetProperty(sResultSet, mstrADUserNamePropertyField);
                        strDepartment = GetProperty(sResultSet, "department");
                        strEmail = GetProperty(sResultSet, "mail");
                        strContact = GetProperty(sResultSet, "telephonenumber");
                        strDesignation = GetProperty(sResultSet, "title");
                    }
                    strUserDetails = strUserName.Replace(",", "") + "|" + strEmail;
                }
                return strUserDetails;
            }
            catch (Exception ex)
            {
                strUserDetails = strUserName + "|" + strEmail;
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                return strUserDetails;
            }
        }

        //<< Added by Archana Gosavi on 29-Mar-2017
        public string getUserName(string strUserId)
        {
            string strUserFullName = "",
                strUserName = "";
            //return strUserId;
            try
            {
                if (mstrAuthentication.Equals("MEM"))
                {
                    MembershipUser user;
                    user = Membership.GetUser(strUserId);
                    strUserFullName = strUserId;
                }
                else
                {
                    if (!Convert.ToString(strUserId.Substring(strUserId.IndexOf("\\") + 1)).Equals(""))
                    {
                        strDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());
                        DirectoryEntry entry = new DirectoryEntry(strDirectoryEntry);
                        DirectorySearcher dSearch = new DirectorySearcher(entry);
                        strUserId = strUserId.Substring(strUserId.IndexOf("\\") + 1);
                        dSearch.Filter = "(&(samAccountName=" + strUserId + ")(objectCategory=person)(objectClass=user))";

                        foreach (SearchResult sResultSet in dSearch.FindAll())
                        {
                            strUserName = GetProperty(sResultSet, mstrADUserNamePropertyField);
                        }
                        strUserFullName = strUserName.Replace(",", "");
                    }
                    else
                    {
                        strUserFullName = "";
                    }
                }
                return strUserFullName;
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                return strUserId;
                throw;
            }
        }
        //>>

        public static string getUnAuthUserDetsFromLDAP(string strUserId, string strReqUserData)
        {
            string strDetails = "", blnIsLDAPIntegrated = "", strDirectoryEntry = "";
            MembershipUser user;

            blnIsLDAPIntegrated = ConfigurationManager.AppSettings["IsLDAPIntegrated"].ToString();

            if (blnIsLDAPIntegrated.Equals("0"))   //To fetch details from Membership
            {
                user = Membership.GetUser(strUserId);

                if (user != null)
                {
                    if (strReqUserData.Equals("Email"))
                    {
                        strDetails = user.Email;
                    }
                    else if (strReqUserData.Equals("Name"))
                    {
                        strDetails = user.UserName;
                    }
                    else if (strReqUserData.Equals("Designation"))
                    {
                        strDetails = "Employee";
                    }
                    else if (strReqUserData.Equals("Department"))
                    {
                        strDetails = "FCU";
                    }
                    else if (strReqUserData.Equals("Location"))
                    {
                        strDetails = "Mumbai";
                    }
                    else if (strReqUserData.Equals("ContactNo"))
                    {
                        strDetails = "1234567890";
                    }
                    else if (strReqUserData.Equals("SubDepartment"))
                    {
                        strDetails = "FCU Team";
                    }
                    else if (strReqUserData.Equals("All"))
                    {
                        strDetails = user.UserName + "," + user.Email + "," + "1234567890" + "," + "FCU" + "," +
                                    "Deputy Manager" + "," + "Mumbai" + "," + "FCU";
                    }
                }
            }
            if (blnIsLDAPIntegrated.Equals("1"))        //To fetch details from WebService
            {
                string strUserName = "", strDepartment = "", strEmail = "", strDesignation = "", strContact = "", strLocation = "", strSubDepartment = "";

                strDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());
                DirectoryEntry entry = new DirectoryEntry(strDirectoryEntry);
                DirectorySearcher dSearch = new DirectorySearcher(entry);
                strUserId = strUserId.Substring(strUserId.IndexOf("\\") + 1);

                dSearch.Filter = "(&(samAccountName=" + strUserId + ")(objectCategory=person)(objectClass=user))";

                foreach (SearchResult sResultSet in dSearch.FindAll())
                {
                    strUserName = GetProperty(sResultSet, mstrADUserNamePropertyField);
                    strDepartment = GetProperty(sResultSet, "department");
                    strEmail = GetProperty(sResultSet, "mail");
                    strContact = GetProperty(sResultSet, "telephonenumber");
                    strDesignation = GetProperty(sResultSet, "title");
                    strLocation = GetProperty(sResultSet, "officelocation");
                    //strSubDepartment = GetProperty(sResultSet, "");
                }

                if (strReqUserData.Equals("Email"))
                {
                    strDetails = strEmail;
                }
                else if (strReqUserData.Equals("Name"))
                {
                    strDetails = strUserName.Replace(",", "");
                }
                else if (strReqUserData.Equals("Designation"))
                {
                    strDetails = strDesignation;
                }
                else if (strReqUserData.Equals("Department"))
                {
                    strDetails = strDepartment;
                }
                else if (strReqUserData.Equals("Location"))
                {
                    strDetails = strLocation;
                }
                else if (strReqUserData.Equals("ContactNo"))
                {
                    strDetails = strContact;
                }
                else if (strReqUserData.Equals("SubDepartment"))
                {
                    strDetails = strSubDepartment;
                }
                else if (strReqUserData.Equals("All"))
                {
                    strDetails = strUserName.Replace(",", "") + "," + strEmail + "," + strContact + "," + strDepartment + "," +
                                strDesignation + "," + strLocation + "," + strSubDepartment;
                }
            }
            return strDetails;
        }
    }
}