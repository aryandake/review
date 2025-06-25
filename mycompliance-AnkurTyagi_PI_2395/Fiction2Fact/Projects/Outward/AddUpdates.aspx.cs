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
    public partial class AddUpdates : System.Web.UI.Page
    {
        Authentication au = new Authentication();
        OutwardBL outBL = new OutwardBL();
        CommonMethods cm = new CommonMethods();
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
                DateTime dtCurrDate = DateTime.Now;
                hfCurDate.Value = dtCurrDate.ToString("dd-MMM-yyyy");

                Page.Header.DataBind();
                if (Request.QueryString["Id"] != null)
                {
                    hfOTId.Value = Request.QueryString["Id"].ToString();
                }
            }
        }
        protected void cvUpdate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CSVInjectionPrevention.CheckInputValidity(this)) return;
            try
            {
                string strUpdateDate = "", strCreatedBy = "", strCurrentUserName = "", strRemark = "";
                if (!Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)).Equals(""))
                {
                    strCreatedBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                    //strCurrentUserName = au.GetUserDetails(Page.User.Identity.Name).Split(',')[0].ToString();
                    //strCreatedBy = strCurrentUserName + " (" + Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)) + ")";
                }

                strUpdateDate = cm.getSanitizedString(txtupdateDate.Text);
                strRemark = cm.getSanitizedString(txtremark.Text);
                int Id = Convert.ToInt32(hfOTId.Value);
                string strDocNo = outBL.AddUpdatesOutwardTrackers(Id, strUpdateDate,
                                            strRemark, strCreatedBy);
                writeError("Update added successfully");
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }

        }
        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
            string script = "\r\n<script language=\"javascript\">\r\n" +
                       " alert('" + strError.Replace("'", "\\'") + "');" +
                        //"window.opener.location.reload(false);\r\n" +
                        " window.close()" +
                       "   </script>\r\n";

            ClientScript.RegisterStartupScript(this.GetType(), "return script", script);
        }
    }
}