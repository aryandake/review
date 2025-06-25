using Fiction2Fact.Legacy_App_Code.Circulars.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects
{
    public partial class checkDuplicateCircularNo : System.Web.UI.Page
    {
        CircUtilitiesBLL UtilitiesBLL = new CircUtilitiesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    string CircularNo = Request.QueryString["Cno"];
                    DataTable dt = UtilitiesBLL.GetDataTable("checkduplicateCircularNo", new DBUtilityParameter("", CircularNo));
                    if (dt.Rows.Count > 0)
                    {
                        Response.ContentType = "text/csv";
                        Response.Write("true");
                        Response.End();
                    }
                    else
                    {
                        Response.ContentType = "text/csv";
                        Response.Write("false");
                        Response.End();
                    }
                }
                catch (Exception)
                {

                }
            }
        }
    }
}