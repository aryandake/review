using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Fiction2Fact.App_Code;
using Fiction2Fact;

public partial class ViewNewsTickerDetails : System.Web.UI.Page
{
    string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
            if (Page.User.Identity.Name.Equals(""))
            {
                Response.Redirect(Global.site_url("Login.aspx"));
                return;
            }
            //>>

            string sContent;

            SqlDataReader drDetails;
            string strSQL = "";
            int intNTId;

            if (!Page.IsPostBack)
            {
                sContent = "";
                intNTId =Convert.ToInt32(Request.QueryString["NewsId"].ToString());
                if (intNTId == 0)
                {
                  lblContents.Text = "This content couldn't be found.";
                    return;
                }
                else
                {
                    strSQL = "SELECT  NT_DETAILS FROM TBL_NEWS_TICKER WHERE  NT_ID =@NT_ID";
                }
                SqlConnection conn;
                conn = new SqlConnection(mstrConnectionString);

                conn.Open();
                SqlParameter spNTId = new SqlParameter("@NT_ID", intNTId);
                SqlCommand scSQLCommand = new SqlCommand(strSQL, conn);
                scSQLCommand.Parameters.Add(spNTId);
                drDetails = scSQLCommand.ExecuteReader();
                
                while (drDetails.Read())
                {
                    sContent = drDetails["NT_DETAILS"] + "";
                }

                drDetails.Close();
                conn.Close();
                if (sContent == string.Empty)
                    lblContents.Text = "This content couldn't be found.";
                else
                    lblContents.Text = sContent;
            }
        }
        catch (Exception ex)
        {
            F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            writeError("Invalid Parameter input.");
        }
    }
    private void writeError(string strError)
    {
        lblMsg.Text = strError;
        this.lblMsg.ForeColor = System.Drawing.Color.Red;
        lblMsg.Visible = true;
        this.lblMsg.Font.Size = 10;
    }

}
