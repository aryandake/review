using System;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.DirectoryServices;
using System.Web;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using System.Web.Profile;
using Fiction2Fact.App_Code.BLL;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Linq;
using Saml;

namespace Fiction2Fact.Projects
{
    /// <summary>
    /// Authentication method
    /// </summary>
    public partial class Login : System.Web.UI.Page
    {
        string strPath = "", strfilterAttribute = "";
        string strActiveDirectoryDomain = "", strDirectoryEntry = "";
        bool IsCompany = false;
        //<< Added by ramesh more on 13-Mar-2024 CR_1991
        string strIsAppSecure = ConfigurationManager.AppSettings.AllKeys.Any(key => key == "IsApplicationSecure") ? ConfigurationManager.AppSettings["IsApplicationSecure"].ToString() : null;
        //>>

        protected void Page_Load(object sender, EventArgs e)
        {
            //<< Added by ramesh more on 13-Mar-2024 CR_1991
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            //>>
            if (!Page.IsPostBack)
            {
                string strType = "";
                //<< Added by ramesh more on 13-Mar-2024 CR_1991
                //ViewState["LoginGuid"] = Guid.NewGuid().ToString();
                //hfValue.Value = ViewState["LoginGuid"].ToString();
                //txtUsername.Focus();
                if (Request.QueryString["Type"] != null)
                {
                    strType = Request.QueryString["Type"].ToString();

                    if (strType.Equals("1"))
                    {
                        lblMsg.Text = "A user has connected with your credentials from another location. Hence, terminating this session.";
                    }
                    else if (strType.Equals("2"))
                    {
                        lblMsg.Text = "This session has been hijacked.";
                        Session.Clear();
                        Session.Abandon();
                        Session.RemoveAll();
                    }
                }
                txtUsername.Attributes.Add("autocomplete", "off");
                txtPassword.Attributes.Add("autocomplete", "off");
                txtUsername.Focus();
                //>>
            }


        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string strAuthentication = "", strUserId = "", strPassword = ""; ;
            try
            {
                if (!string.IsNullOrEmpty((string)Session["UserType"] ?? ""))
                {
                    Session["UserType"] = null;
                }

                F2FUtilitiesBLL f2fUtil = new F2FUtilitiesBLL();
                IsCompany = f2fUtil.SetSessionCompany();
                if (!IsCompany)
                {
                    writeError("Login Company Details not available");
                    return;
                }
            }
            catch (Exception ex)
            {
                writeError("Error While Login " + ex.Message);
                return;
            }

            try
            {
                strAuthentication = (ConfigurationManager.AppSettings["AuthenticationSetting"].ToString());
                //<< Added by ramesh more on 13-Mar-2024 CR_1991 for VAPT
                strUserId = txtUsername.Text;
                strPassword = DecryptStringAES(txtPassword.Text).ToString();
                //>>
                if (strAuthentication.Equals("MEM"))
                {
                    Membership_Login(strUserId, strPassword);
                }
                else
                {
                    AD_Login(strUserId, strPassword);
                }
            }
            catch (Exception ex)
            {
                writeError("System Exception in btnLogin_Click():" + ex.Message);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page != null)
            {
                Page.RegisterRequiresViewStateEncryption();
            }
        }


        protected void Membership_Login(string strUserId, string strPassword)
        {
            try
            {
                string lastLogin = null;
                MembershipUser mu = Membership.GetUser(strUserId);
                if (mu != null)
                {
                    lastLogin = mu.LastLoginDate.ToString();

                    if (IsCompany && Membership.ValidateUser(strUserId, strPassword))
                    {
                        Session["UserName"] = strUserId;
                        Session["UserID"] = strUserId;
                        //<< code Added by ramesh more on 13-Mar-2024 CR_1991 for VAPT
                        // createa a new GUID and save into the session
                        string guid = Guid.NewGuid().ToString();
                        Session["AuthToken"] = guid;
                        // now create a new cookie with this guid value
                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                        FormsAuthentication.SetAuthCookie(strUserId, false);
                        if (strIsAppSecure.Equals("Y"))
                        {
                            if (Response.Cookies[".ASPXAUTH"] != null)
                            {
                                Response.Cookies[".ASPXAUTH"].Secure = true;
                                Response.Cookies[".ASPXAUTH"].SameSite = SameSiteMode.Strict;
                            }

                            if (Response.Cookies["AuthToken"] != null)
                            {
                                Response.Cookies["AuthToken"].Secure = true;
                                Response.Cookies["AuthToken"].SameSite = SameSiteMode.Strict;
                            }
                        }

                        FormsAuthentication.RedirectFromLoginPage(strUserId, false);
                        SingleSessionPreparation.CreateAndStoreSessionToken(strUserId);
                        // >>
                        string strLastLoginDt = null;
                        //strLastActivityDate = Membership.GetUser(txtUsername.Text).LastActivityDate.ToString();
                        if (!lastLogin.Equals(""))
                        {
                            strLastLoginDt = String.Format("{0:dd-MMM-yyyy hh:mm:ss tt}", Convert.ToDateTime(lastLogin));
                        }
                        Session["LastLoginDt"] = strLastLoginDt;
                    }
                    else
                    {
                        lblMsg.Text = "Invalid username/password.";
                        txtUsername.Text = "";
                        txtPassword.Text = "";
                    }
                }
                else
                {
                    lblMsg.Text = "Invalid username/password. Kindly re-enter the details.";
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //writeError("System Exception in Membership_Login() :" + ex.Message);
            }
        }
        //<<Added by Supriya on 04-Sep-2014

        private void writeError(string strMsg)
        {
            this.lblMsg.Text = strMsg;
            this.lblMsg.Visible = true;
        }

        protected void lbViewCircular_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)Session["UserType"] ?? ""))
            {
                Session["UserType"] = "Anonymous";
            }
            Response.Redirect("Projects/Circulars/ViewCircular.aspx");
        }

        //Active directory  Login 
        protected void AD_Login(string strUserId, string strPassword)
        {
            string lastLogin = "", strLastLoginDt = "";
            try
            {
                if (IsAuthenticated(strActiveDirectoryDomain, strUserId, strPassword))
                {
                    MembershipUser user = Membership.GetUser(strUserId);
                    if (user != null)
                    {
                        lastLogin = user.LastLoginDate.ToString();
                        if (!lastLogin.Equals(""))
                            strLastLoginDt = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(lastLogin));
                        Session["LastLoginDt"] = strLastLoginDt;

                        Session["UserID"] = strUserId;
                        if (Membership.ValidateUser(txtUsername.Text, "pass@123"))
                        {
                            //<< code Added by ramesh more on 13-Mar-2024 CR_1991 for VAPT
                            // createa a new GUID and save into the session
                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;
                            // now create a new cookie with this guid value
                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                            FormsAuthentication.SetAuthCookie(strUserId, false);
                            if (strIsAppSecure.Equals("Y"))
                            {
                                if (Response.Cookies[".ASPXAUTH"] != null)
                                {
                                    Response.Cookies[".ASPXAUTH"].Secure = true;
                                    Response.Cookies[".ASPXAUTH"].SameSite = SameSiteMode.Strict;
                                }

                                if (Response.Cookies["AuthToken"] != null)
                                {
                                    Response.Cookies["AuthToken"].Secure = true;
                                    Response.Cookies["AuthToken"].SameSite = SameSiteMode.Strict;
                                }
                            }

                            // >>
                            FormsAuthentication.RedirectFromLoginPage((strUserId).ToUpper(), false);
                            SingleSessionPreparation.CreateAndStoreSessionToken(strUserId);
                        }
                        else
                        {
                            //Here the last login date & time shall  not be visible.
                            writeError("Error in authenticating the user credentials.");
                            txtUsername.Text = "";
                            txtPassword.Text = "";
                        }
                    }
                    else
                    {
                        //This user is not existent in membership and is from AD.
                        string strName = "", strEmailId = "", strDepartment = "", strTelephoneNumber = "", strTitle = "";
                        Authentication au = new Authentication();
                        string strUserDetails = au.GetDetailsForUserCreation(strUserId);
                        if (!strUserDetails.Equals(""))
                        {
                            string[] strSplit = strUserDetails.Split('|');
                            strName = strSplit[0];
                            strDepartment = strSplit[1];
                            strEmailId = strSplit[2];
                            strTelephoneNumber = strSplit[3];
                            strTitle = strSplit[4];

                            Membership.CreateUser(strUserId, "pass@123", strEmailId);
                            ProfileBase profile = ProfileBase.Create(strUserId, true);
                            profile.SetPropertyValue("name", strName);
                            profile.SetPropertyValue("department", strDepartment);
                            profile.SetPropertyValue("telephonenumber", strTelephoneNumber);
                            profile.SetPropertyValue("title", strTitle);
                            profile.Save();
                            if (Membership.ValidateUser(strUserId, "pass@123"))
                            {
                                //once this is done, the last login date & time shall be visible.
                                FormsAuthentication.RedirectFromLoginPage((strUserId).ToUpper(), false);
                                //SingleSessionPreparation.CreateAndStoreSessionToken(strUserId);
                            }
                        }
                        else
                        {
                            writeError("Error in fetching user details from Active Directory.");
                            txtUsername.Text = "";
                            txtPassword.Text = "";
                        }
                    }
                    //>>
                }
                else
                {
                    writeError("Invalid Username or Password.");
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                }
            }
            catch (Exception e1)
            {
                writeError("Error in AD_Login(): " + e1.Message);
            }
        }

        private bool IsAuthenticated(string strDomain, string strUserId, string strpwd)
        {
            //string strDomainAndUserName = Authentication.GetUserIDWithDomain(strUserId);
            string strDomainAndUserName = strUserId;
            strDirectoryEntry = (ConfigurationManager.AppSettings["DirectoryEntry"].ToString());
            strPath = strDirectoryEntry;
            DirectoryEntry dtEntry = new DirectoryEntry(strPath, strDomainAndUserName, strpwd);
            try
            {
                object obj = dtEntry.NativeObject;
                DirectorySearcher dsSearch = new DirectorySearcher(dtEntry);
                dsSearch.Filter = "(SAMAccountName=" + strUserId + ")";
                dsSearch.PropertiesToLoad.Add("cn");
                SearchResult srResult = dsSearch.FindOne();

                if (srResult.Equals(""))
                {
                    return false;
                }
                strPath = srResult.Path;

                strfilterAttribute = srResult.Properties["cn"].ToString();
                System.Web.HttpContext.Current.Session["UserName"] = strfilterAttribute;
                return true;
            }
            catch (Exception e)
            {
                writeError("Error authenticating user." + e.Message);
                txtPassword.Text = "";
                txtUsername.Text = "";
                return false;
            }
        }

        //<< code Added by ramesh more on 13-Mar-2024 CR_1991 for VAPT
        public static string DecryptStringAES(string cipherText)
        {

            var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        //>>
        public void LoginSSO()
        {
            string strSAMLEndPoint = "", strUniequeIdentity = "", strAssertionUrl = "";

            strSAMLEndPoint = (ConfigurationManager.AppSettings["SAMLEndpoint"].ToString());
            strUniequeIdentity = (ConfigurationManager.AppSettings["SAMLUniqueIdentity"].ToString());
            strAssertionUrl = (ConfigurationManager.AppSettings["SAMLAssertionUrl"].ToString());

            var request = new AuthRequest(strUniequeIdentity, strAssertionUrl);
            Response.Redirect(request.GetRedirectUrl(strSAMLEndPoint), false);
        }
        //>>

        protected void btnSSOLogin_Click(object sender, EventArgs e)
        {
            LoginSSO();
        }
    }
}