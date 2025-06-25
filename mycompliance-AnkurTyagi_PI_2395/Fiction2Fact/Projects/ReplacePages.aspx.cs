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
using Fiction2Fact;

public partial class ReplacePages_Test : System.Web.UI.Page
{
    string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
        if (Page.User.Identity.Name.Equals(""))
        {
            Response.Redirect(Global.site_url("Login.aspx"));
            return;
        }
        //>>
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (Page != null)
        {
            Page.RegisterRequiresViewStateEncryption();
        }
    }


    private void writeError(string strError)
    {
        lblMsg.Text = strError;
        this.lblMsg.ForeColor = System.Drawing.Color.Red;
        lblMsg.Visible = true;
        this.lblMsg.Font.Size = 12;
    }
    protected void btnUpload_Click(object sender, System.EventArgs e)
    {
        try
        {
            //FileInfo fileInfo;
            string strFolderPath = "", strFileName = "", strCompleteFileName="";
            strFolderPath =Server.MapPath("~\\" + txtFolder.Text);
            strFileName = fuFileUpload.FileName;

            strCompleteFileName = (strFolderPath + ("\\" + strFileName));
            //fileInfo = new FileInfo(strCompleteFileName);

            fuFileUpload.SaveAs(strFolderPath + "\\" + strFileName);
            writeError("File Upload Successfully");
            //if (fileInfo.Exists)
            //{

            //}
           

        }
        catch (System.Exception ex)
        {
            F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            throw new System.Exception("system exception in btnUpload_Click() " + ex);
        }
        finally
        {
        }

    }
}
