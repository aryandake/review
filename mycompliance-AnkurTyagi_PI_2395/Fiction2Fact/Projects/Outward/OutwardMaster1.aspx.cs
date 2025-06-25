using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact.Projects.Outward
{
    public partial class OutwardMaster1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(Global.site_url("Projects/Admin/CommonMasterEntryPage.aspx?Type=16"));
        }
    }
}