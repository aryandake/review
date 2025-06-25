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
using Fiction2Fact.Legacy_App_Code;

namespace Fiction2Fact.Projects.Outward
{
    public partial class OutwardMasters : System.Web.UI.Page
    {
        //<<Added by Ankur Tyagi on 19Mar2024 for CR_1991
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Redirect(Global.site_url("Projects/Admin/CommonMasterEntryPage.aspx?Type=" + encdec.Encrypt("16")));
            Response.Redirect(Global.site_url("Projects/Admin/CommonMasterEntryPage.aspx?Type=16"));
        }
        //>>
        //Response.Redirect(Global.site_url("Projects/Admin/CommonMasterEntryPage.aspx?Type=16"));
    }
}