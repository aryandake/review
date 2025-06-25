using System;
using System.Data;
using System.Configuration;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.App_Code;
using System.Security;
using Fiction2Fact.Legacy_App_Code;
using System.Linq;
using System.Web.Security;

namespace Fiction2Fact
{
    public partial class Default : System.Web.UI.Page
    {
        ReportsBLL reportsBLL = new ReportsBLL();
        public string siteUrl = Global.site_url();
        DataServer d = new DataServer();
        Authentication auth = new Authentication();
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();

        protected void Page_Load(object sender, EventArgs e)
        {
            string CUserRole = "";
            string FUserRole = "";

            if (User.IsInRole("CircularAdmin") || User.IsInRole("CircularUser"))
            {
                CUserRole = "Admin";
            }
            else
            {
                CUserRole = "Other";
            }

            if (User.IsInRole("FilingAdmin") || User.IsInRole("FilingUser") || User.IsInRole("Filing_Sub_Admin"))
            {
                FUserRole = "Admin";
            }
            else
            {
                FUserRole = "Other";
            }

            string strLoggedInUser = Page.User.Identity.Name;
            string EmailId = auth.getEmailIDOnUserId(strLoggedInUser);

            string[] roles = Roles.GetRolesForUser(strLoggedInUser);

            if (roles.Contains("admin", StringComparer.OrdinalIgnoreCase) == true)
            {
                pnlFilDashboard.Visible = true;
                getDashboardCount("Filings", strLoggedInUser, EmailId, FUserRole);
            }
            else if (roles.Any(role => role.StartsWith("certification", StringComparison.OrdinalIgnoreCase)) == true)
            {
                divCardBody.Visible = false;
            }
            else if (roles.Any(role => role.StartsWith("filing", StringComparison.OrdinalIgnoreCase)) == true)
            {
                pnlFilDashboard.Visible = true;
                getDashboardCount("Filings", strLoggedInUser, EmailId, FUserRole);
            }
            else if (roles.Any(role => role.StartsWith("circular", StringComparison.OrdinalIgnoreCase)) == true)
            {
                getDashboardCount("Circular", strLoggedInUser, EmailId, CUserRole);
            }
        }

        private void getDashboardCount(string strModule, string Username, string EmailId, string UserRole)
        {
            try
            {
                if (strModule.Equals("Circular"))
                {
                    string strDataValues1 = reportsBLL.getCountForSingleDashboard(strModule, "1", Username, EmailId, UserRole);
                    string strDataValues2 = reportsBLL.getCountForSingleDashboard(strModule, "2", Username, EmailId, UserRole);
                    string strDataValues3 = reportsBLL.getCountForSingleDashboard(strModule, "3", Username, EmailId, UserRole);
                    string strDataValues4 = reportsBLL.getCountForSingleDashboard(strModule, "4", Username, EmailId, UserRole);

                    lblCWTDCount.Text = strDataValues1;
                    lblCWTD.Text = "<a href=\"#\" onclick=\"window.open('Projects/Circulars/DetailedReportCircular.aspx?ReportType=1&Status=CWD','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";

                    lblCATDCount.Text = strDataValues2;
                    lblCATD.Text = "<a href=\"#\" onclick=\"window.open('Projects/Circulars/DetailedReportCircular.aspx?ReportType=1&Status=CAD','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";

                    lblNYDCount.Text = strDataValues3;
                    lblNYD.Text = "<a href=\"#\" onclick=\"window.open('Projects/Circulars/DetailedReportCircular.aspx?ReportType=1&Status=ND','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";

                    lblDBNCCount.Text = strDataValues4;
                    lblDBNC.Text = "<a href=\"#\" onclick=\"window.open('Projects/Circulars/DetailedReportCircular.aspx?ReportType=1&Status=DNS','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";
                }
                else if (strModule.Equals("Filings"))
                {
                    if (UserRole == "Admin")
                    {
                        divT_TDA.Visible = true;
                        divT_TNCNC.Visible = true;
                        divT_TNSNC.Visible = true;
                        divT_TOWT.Visible = true;
                        divT_CT.Visible = true;
                        divT_NCT.Visible = true;

                        string strTDACount = reportsBLL.getCountForSingleDashboard(strModule, "1T", Username, EmailId, UserRole);
                        string strTNCNCCount = reportsBLL.getCountForSingleDashboard(strModule, "2T", Username, EmailId, UserRole);
                        string strTNSNCCount = reportsBLL.getCountForSingleDashboard(strModule, "3T", Username, EmailId, UserRole);
                        string strTOWTCount = reportsBLL.getCountForSingleDashboard(strModule, "4T", Username, EmailId, UserRole);
                        string strCTCount = reportsBLL.getCountForSingleDashboard(strModule, "5T", Username, EmailId, UserRole);
                        string strNCTCount = reportsBLL.getCountForSingleDashboard(strModule, "6T", Username, EmailId, UserRole);

                        lblT_TDACount.Text = strTDACount;
                        lblT_TDA.Text = "<a href=\"#\" onclick=\"window.open('Projects/Submissions/DashboardDetailedReport.aspx?ReportType=2&Status=T_TDA" +
                            "&UName=" + encdec.Encrypt(Username) + "&UMail=" + encdec.Encrypt(EmailId) + "&Role=" + encdec.Encrypt(UserRole) + "','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";

                        lblT_TNCNCCount.Text = strTNCNCCount;
                        lblT_TNCNC.Text = "<a href=\"#\" onclick=\"window.open('Projects/Submissions/DashboardDetailedReport.aspx?ReportType=2&Status=T_TNCNC" +
                            "&UName=" + encdec.Encrypt(Username) + "&UMail=" + encdec.Encrypt(EmailId) + "&Role=" + encdec.Encrypt(UserRole) + "','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";

                        lblT_TNSNCCount.Text = strTNSNCCount;
                        lblT_TNSNC.Text = "<a href=\"#\" onclick=\"window.open('Projects/Submissions/DashboardDetailedReport.aspx?ReportType=2&Status=T_TNSNC" +
                            "&UName=" + encdec.Encrypt(Username) + "&UMail=" + encdec.Encrypt(EmailId) + "&Role=" + encdec.Encrypt(UserRole) + "','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";

                        lblT_TOWTCount.Text = strTOWTCount;
                        lblT_TOWT.Text = "<a href=\"#\" onclick=\"window.open('Projects/Submissions/DashboardDetailedReport.aspx?ReportType=2&Status=T_TOWT" +
                            "&UName=" + encdec.Encrypt(Username) + "&UMail=" + encdec.Encrypt(EmailId) + "&Role=" + encdec.Encrypt(UserRole) + "','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";

                        lblT_CTCount.Text = strCTCount;
                        lblT_CT.Text = "<a href=\"#\" onclick=\"window.open('Projects/Submissions/DashboardDetailedReport.aspx?ReportType=2&Status=T_CT" +
                            "&UName=" + encdec.Encrypt(Username) + "&UMail=" + encdec.Encrypt(EmailId) + "&Role=" + encdec.Encrypt(UserRole) + "','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";

                        lblT_NCTCount.Text = strNCTCount;
                        lblT_NCT.Text = "<a href=\"#\" onclick=\"window.open('Projects/Submissions/DashboardDetailedReport.aspx?ReportType=2&Status=T_NCT" +
                            "&UName=" + encdec.Encrypt(Username) + "&UMail=" + encdec.Encrypt(EmailId) + "&Role=" + encdec.Encrypt(UserRole) + "','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";

                    }
                    else
                    {
                        divR_TDA.Visible = true;
                        divR_TOA.Visible = true;
                        divR_NCTNC.Visible = true;
                        divR_CT.Visible = true;
                        divR_NCT.Visible = true;

                        string strTDACount = reportsBLL.getCountForSingleDashboard(strModule, "1R", Username, EmailId, UserRole);
                        string strTOACount = reportsBLL.getCountForSingleDashboard(strModule, "2R", Username, EmailId, UserRole);
                        string strNCTNSCount = reportsBLL.getCountForSingleDashboard(strModule, "3R", Username, EmailId, UserRole);
                        string strCTCount = reportsBLL.getCountForSingleDashboard(strModule, "4R", Username, EmailId, UserRole);
                        string strNCTCount = reportsBLL.getCountForSingleDashboard(strModule, "5R", Username, EmailId, UserRole);

                        lblR_TDACount.Text = strTDACount;
                        lblR_TDA.Text = "<a href=\"#\" onclick=\"window.open('Projects/Submissions/DashboardDetailedReport.aspx?ReportType=2&Status=R_TDA" +
                            "&UName=" + encdec.Encrypt(Username) + "&UMail=" + encdec.Encrypt(EmailId) + "&Role=" + encdec.Encrypt(UserRole) + "','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";

                        lblR_TOACount.Text = strTOACount;
                        lblR_TOA.Text = "<a href=\"#\" onclick=\"window.open('Projects/Submissions/DashboardDetailedReport.aspx?ReportType=2&Status=R_TOA" +
                            "&UName=" + encdec.Encrypt(Username) + "&UMail=" + encdec.Encrypt(EmailId) + "&Role=" + encdec.Encrypt(UserRole) + "','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";

                        lblR_NCTNSCount.Text = strNCTNSCount;
                        lblR_NCTNS.Text = "<a href=\"#\" onclick=\"window.open('Projects/Submissions/DashboardDetailedReport.aspx?ReportType=2&Status=R_NCTNS" +
                            "&UName=" + encdec.Encrypt(Username) + "&UMail=" + encdec.Encrypt(EmailId) + "&Role=" + encdec.Encrypt(UserRole) + "','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";

                        lblR_CTCount.Text = strCTCount;
                        lblR_CT.Text = "<a href=\"#\" onclick=\"window.open('Projects/Submissions/DashboardDetailedReport.aspx?ReportType=2&Status=R_CT" +
                            "&UName=" + encdec.Encrypt(Username) + "&UMail=" + encdec.Encrypt(EmailId) + "&Role=" + encdec.Encrypt(UserRole) + "','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";

                        lblR_NCTCount.Text = strNCTCount;
                        lblR_NCT.Text = "<a href=\"#\" onclick=\"window.open('Projects/Submissions/DashboardDetailedReport.aspx?ReportType=2&Status=R_NCT" +
                            "&UName=" + encdec.Encrypt(Username) + "&UMail=" + encdec.Encrypt(EmailId) + "&Role=" + encdec.Encrypt(UserRole) + "','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\" style =\"color: #fff\">Click here</a>";

                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}