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
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects
{
    public partial class DBQuery_Test : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Visible = true;
            this.lblMsg.Font.Size = 12;
        }
        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            SqlConnection myconnection = new SqlConnection(mstrConnectionString);
            try
            {
                string strSql = "";
                myconnection.Open();
                strSql = txtQuery.Text;

                SqlCommand cmd = new SqlCommand(strSql, myconnection);

                if (rblSelect.SelectedValue.Equals("Y"))
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    ad.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        gvDetails.DataSource = dt;
                        gvDetails.DataBind();
                    }
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }



                writeError("Execute query successfully");
                txtQuery.Text = "";
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in btnSave_Click() " + ex);
            }
            finally
            {
                myconnection.Close();
            }

        }

    }
}