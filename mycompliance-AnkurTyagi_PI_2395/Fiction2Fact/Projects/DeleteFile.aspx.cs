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
using Fiction2Fact;
using System.IO;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.App_Code;

public partial class UploadFile : System.Web.UI.Page
{
    string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
    UtilitiesBLL utilBL = new UtilitiesBLL();
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
            try
            {
                hfUniqueRowId.Value = Request.QueryString["rowNo"];
                hfServerFileName.Value = Request.QueryString["filename"];
                hfFileId.Value = Request.QueryString["fileId"];
                int intCertId = Convert.ToInt32(hfFileId.Value);
                //if (intCertId != 0)
                //{
                //    utilBL.getDatasetWithCondition("deleteCertException", intCertId, strConnectionString);
                //}

                //<<delete file from file system.
                FileInfo fileInfo;                
                string fileDirectory = Request.QueryString["filename"];
                if (fileDirectory != null && fileDirectory.Length > 0)
                {

                    string serverDirectory = Server.MapPath(ConfigurationManager.AppSettings["CertificationFilesFolder"].ToString());
                    fileInfo = new FileInfo(serverDirectory + "\\" + fileDirectory);
                    if (fileInfo != null && fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                }
                //>>
                string script = "\r\n<script language=\"javascript\">\r\n" +
                            " closeFileWindow();" +
                            "   </script>\r\n";

                ClientScript.RegisterStartupScript(this.GetType(), "script", script);
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError("Exception in deleteFile():" + ex);
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


    private void writeError(string strError)
    {
        lblMsg.Text = strError;
        lblMsg.Visible = true;
    }
    

}
