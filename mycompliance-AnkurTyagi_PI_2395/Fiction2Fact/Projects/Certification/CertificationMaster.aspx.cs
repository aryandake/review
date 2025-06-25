using Fiction2Fact.Legacy_App_Code;
using System;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_CertificationMaster : System.Web.UI.Page
    {
        //<<Added by Ankur Tyagi on 19Mar2024 for CR_1991
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();

        protected void Page_Load(object sender, EventArgs e)
        {
            //string URL = Global.site_url("Projects/Admin/CommonMasterEntryPage.aspx?Type=" + encdec.Encrypt("10"));
            string URL = Global.site_url("Projects/Admin/CommonMasterEntryPage.aspx?Type=10");
            Response.Redirect(URL, false);

        }
        //>>
    }
}