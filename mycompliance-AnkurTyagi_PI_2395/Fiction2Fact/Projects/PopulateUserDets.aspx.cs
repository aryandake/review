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
using System.Data.SqlClient;
using System.IO;
using Fiction2Fact.Legacy_App_Code;

namespace Fiction2Fact.Projects
{
    public partial class PopulateUserDets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    string strUserCode = Request.QueryString["UserCode"];
                    string strType = Request.QueryString["Type"];
                    string[] strDetailsList;
                    string strDetails;
                    string strUserName = "";
                    string strEmail = "", strDept = "";
                    Authentication auth = new Authentication();
                    if (!strUserCode.Equals(""))
                    {
                        if (strType.Equals("HelpDesk"))
                        {
                            strDetails = auth.GetUserDetails(strUserCode);
                            strDetailsList = strDetails.Split('|');
                            strUserName = Convert.ToString(strDetailsList[0]);
                            strEmail = Convert.ToString(strDetailsList[1]);
                            strDept = "";
                        }
                        else
                        {
                            strDetails = auth.GetDetailsForUserCreation(strUserCode);
                            strDetailsList = strDetails.Split('|');
                            strUserName = Convert.ToString(strDetailsList[0]);
                            strDept = Convert.ToString(strDetailsList[1]);
                            strEmail = Convert.ToString(strDetailsList[2]);
                        }
                    }
                    Response.ContentType = "text/csv";
                    Response.Write(strUserName + "|" + strEmail + "|" + strDept);
                    Response.End();
                    //string script = "\r\n<script language=\"javascript\">\r\n" +
                    //            " window.close();" +
                    //            "   </script>\r\n";

                    //ClientScript.RegisterStartupScript(this.GetType(), "script", script);
                }
                catch (Exception)
                {

                }
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

    }
}