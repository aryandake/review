using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.Outward.BLL;

namespace Fiction2Fact.Projects.Outward
{
    public partial class SuggestRevision : System.Web.UI.Page
    {
        Authentication au = new Authentication();
        OutwardBL outBL = new OutwardBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
            if (Page.User.Identity.Name.Equals(""))
            {
                Response.Redirect(Global.site_url("Login.aspx"));
                return;
            }
            //>>

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    hfOTId.Value = Request.QueryString["Id"].ToString();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strSuggestedRemark = "", strCreatedBy = "", strCurrentUserName = "", strStatus="";
            if (!Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)).Equals(""))
            {
                strCurrentUserName = au.GetUserDetails(Page.User.Identity.Name).Split(',')[0].ToString();
                strCreatedBy = strCurrentUserName + " (" + Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)) + ")";
            }
            strStatus = "Changes suggested by Compliance";
            strSuggestedRemark = txtremark.Text;
            string strDocNo = outBL.SuggestRevisionOutwardTrackers(hfOTId.Value, strSuggestedRemark,
                                        strCreatedBy, strStatus);
            writeError("Record has been saved successfully with outward no. " + strDocNo);

        }
        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
            string script = "\r\n<script language=\"javascript\">\r\n" +
                       " alert('" + strError.Replace("'", "\\'") + "');" +
                        " window.close()" +
                       "   </script>\r\n";

            ClientScript.RegisterStartupScript(this.GetType(), "return script", script);
        }
        
     }
}