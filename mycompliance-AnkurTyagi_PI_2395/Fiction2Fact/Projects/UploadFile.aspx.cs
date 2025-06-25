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

using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using System.IO;

namespace Fiction2Fact
{
    public partial class UploadFile : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
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
            bool blnIsExeFile;
            byte[] fileData;

            try
            {
                if (fuException.HasFile)
                {

                    //System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fuException.PostedFile.InputStream);
                    //fileData = binaryReader.ReadBytes(fuException.PostedFile.ContentLength);
                    //blnIsExeFile = UploadedFileContentCheck.IsExeFile(fileData);
                    //if (blnIsExeFile)
                    //{
                    //    writeError("The file you're trying to upload seems to contain malicious content" +
                    //     " and cannot be uploaded.");
                    //    return;
                    //}

                    string strSelectedFile = fuException.FileName;
                    ////<<addde by supriya to check file name correct or not on 09-Jun-2017
                    //string strReturmMsg = "";
                    //Fiction2Fact.Legacy_App_Code.CommonCode cc = new Fiction2Fact.Legacy_App_Code.CommonCode();
                    //strReturmMsg = cc.getFileNameErrors(strSelectedFile);

                    //if (!strReturmMsg.Equals(""))
                    //{
                    //    writeError(strReturmMsg);
                    //    return;
                    //}
                    ////>>
                    //<< Modified by Ramesh more on 13-Mar-2024 CR_1991
                    if (strSelectedFile.Length > 200)
                    {
                        writeError("File Name Exceed 200 Characters");
                        return;
                    }
                    if (strSelectedFile.Contains("!") || strSelectedFile.Contains("@") ||
                              strSelectedFile.Contains("#") || strSelectedFile.Contains("$") ||
                              strSelectedFile.Contains("%") || strSelectedFile.Contains("^") ||
                              strSelectedFile.Contains("&") || strSelectedFile.Contains("'") ||
                              strSelectedFile.Contains("\""))
                    {
                        writeError("Invalid File Name");
                        return;
                    }

                    string strFileExtension = Path.GetExtension(strSelectedFile);
                    if (UploadedFileContentCheck.checkForMaliciousFileFromHeaders(strFileExtension, fuException.FileBytes))
                    {
                        writeError("The file contains malicious content. Kindly check the file and reupload.");
                        return;
                    }


                    string strReturnMsg = UploadedFileContentCheck.checkValidFileForUpload(fuException, "");

                    CommonCode cc = new CommonCode();
                    strReturnMsg += cc.getFileNameErrors(strSelectedFile);
                    if (!strReturnMsg.Equals(""))
                    {
                        writeError(strReturnMsg);
                        return;
                    }

                    if (UploadedFileContentCheck.checkForMultipleExtention(strSelectedFile))
                    {
                        writeError("The file uploaded is multiple extensions.");
                        return;
                    }
                    //>>

                    DataTable dtFile = new DataTable();
                    string strClientFileName = fuException.FileName;
                    DateTime dt = System.DateTime.Now;
                    string strfilename = Authentication.GetUserID(Page.User.Identity.Name) + "_" +
                    dt.ToString("ddMMyyyyHHmmss") + "_" + strClientFileName;
                    string strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["CertificationFilesFolder"].ToString());
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
}