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
using MySql.Data.MySqlClient;
using Fiction2Fact.App_Code;

namespace Fiction2Fact
{
    public partial class DBQuery_Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.SkipAuthorization = true;
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
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    string strSql = "";
                    DB.OpenConnection();
                    DB.F2FCommand.CommandText = strSql = txtQuery.Text;

                    if (rblSelect.SelectedValue.Equals("Y"))
                    {
                        DataSet ds = new DataSet();
                        
                        DB.F2FDataAdapter.Fill(ds);
                        DataTable dt = ds.Tables[0];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            gvDetails.DataSource = dt;
                            gvDetails.DataBind();
                        }
                    }
                    else
                    {
                        DB.F2FCommand.CommandType = CommandType.Text;
                        DB.F2FCommand.ExecuteNonQuery();
                    }
                    writeError("Execute query successfully");
                    txtQuery.Text = "";
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in btnSave_Click() " + ex);
                }
                finally
                {
                    //myconnection.Close();
                }
            }
        }

    }
}