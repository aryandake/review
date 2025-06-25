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
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using System.IO;

public partial class UploadChecklistFile : System.Web.UI.Page
{
    string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
    Fiction2Fact.Legacy_App_Code.CommonCode cc = new Fiction2Fact.Legacy_App_Code.CommonCode();
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
            hfUniqueRowId.Value = Request.QueryString["rowNo"];
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


    protected void btnProcess_Click(object sender, EventArgs e)
    {
        try
        {
            if (fuException.HasFile)
            {
                DataTable dtFile = new DataTable();
                string strClientFileName = fuException.FileName;
                string strMsg = "";
                ////<<changed By Rahuldeb on 15Jun2017
                //strMsg = cc.getFileNameErrors(strClientFileName);
                //if (!strMsg.Equals(""))
                //{
                //    writeError(strMsg);
                //    return;
                //}
                ////>>
                //<< Modified by Ramesh more on 13-Mar-2024 CR_1991
                if (strClientFileName.Length > 200)
                {
                    writeError("File Name Exceed 200 Characters");
                    return;
                }
                if (strClientFileName.Contains("!") || strClientFileName.Contains("@") ||
                          strClientFileName.Contains("#") || strClientFileName.Contains("$") ||
                          strClientFileName.Contains("%") || strClientFileName.Contains("^") ||
                          strClientFileName.Contains("&") || strClientFileName.Contains("'") ||
                          strClientFileName.Contains("\"") || strClientFileName.Contains(","))
                {
                    writeError("Invalid File Name");
                    return;
                }

                string strFileExtension = Path.GetExtension(strClientFileName);
                if (UploadedFileContentCheck.checkForMaliciousFileFromHeaders(strFileExtension, fuException.FileBytes))
                {
                    writeError("The file contains malicious content. Kindly check the file and reupload.");
                    return;
                }


                string strReturnMsg = UploadedFileContentCheck.checkValidFileForUpload(fuException, "");

                CommonCode cc = new CommonCode();
                strReturnMsg += cc.getFileNameErrors(strClientFileName);
                if (!strReturnMsg.Equals(""))
                {
                    writeError(strReturnMsg);
                    return;
                }

                if (UploadedFileContentCheck.checkForMultipleExtention(strClientFileName))
                {
                    writeError("The file uploaded is multiple extensions.");
                    return;
                }
                //>>

                DateTime dt = System.DateTime.Now;
                string strfilename = Authentication.GetUserID(Page.User.Identity.Name) + "_" +
                dt.ToString("ddMMyyyyHHmmss") + "_" + strClientFileName;
                string strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["ChecklistFilesFolder"].ToString());
                string strCompleteName = strServerDirectory + "\\" + strfilename;
                fuException.SaveAs(strCompleteName);

                hfClientFileName.Value = strClientFileName;
                hfServerFileName.Value = strfilename;

                string script = "\r\n<script language=\"javascript\">\r\n" +
                            " closeFileWindow();" +
                            "   </script>\r\n";

                ClientScript.RegisterStartupScript(this.GetType(), "script", script);

            }
            else
            {
                writeError("Please select a file for uploading.");
                return;
            }

        }
        catch (Exception ex)
        {
            F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            writeError("Exception in btnProcess():" + ex);
        }
    }

    private void writeError(string strError)
    {
        lblMsg.Text = strError;
        lblMsg.Visible = true;
    }
}
