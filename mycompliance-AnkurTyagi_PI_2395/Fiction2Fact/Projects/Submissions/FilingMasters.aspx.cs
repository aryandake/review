using Fiction2Fact.Legacy_App_Code;
using System;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class FilingMasters : System.Web.UI.Page
    {
        //<<Added by Ankur Tyagi on 19Mar2024 for CR_1991
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Redirect(Global.site_url("Projects/Admin/CommonMasterEntryPage.aspx?Type=" + encdec.Encrypt("5")));
            Response.Redirect(Global.site_url("Projects/Admin/CommonMasterEntryPage.aspx?Type=5"));
        }
        //>>
        //Response.Redirect(Global.site_url("Projects/Admin/CommonMasterEntryPage.aspx?Type=5"));

        
    }
}