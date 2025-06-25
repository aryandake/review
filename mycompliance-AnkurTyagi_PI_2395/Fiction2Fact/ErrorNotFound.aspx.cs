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

namespace Fiction2Fact
{
    public partial class ErrorNotFound : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //<< Added by Amarjeet on 18-Aug-2020
            if (Page.User.Identity.Name.Equals(""))
                Response.Redirect("~/Login.aspx");
            //>>
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx", true);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page.User.Identity.IsAuthenticated)
                Page.ViewStateUserKey = Session.SessionID;
        }
    }
}