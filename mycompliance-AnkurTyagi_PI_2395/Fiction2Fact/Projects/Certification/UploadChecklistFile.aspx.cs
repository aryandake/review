using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_UploadChecklistFile : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        Fiction2Fact.Legacy_App_Code.CommonCode cc = new Fiction2Fact.Legacy_App_Code.CommonCode();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //<<Added by Ankur Tyagi on 19Mar2024 for CR_1991
                if (Page.User.Identity.Name.Equals(""))
                {
                    Response.Redirect(Global.site_url("Login.aspx"));
                    return;
                }
                //>>
                if (!Page.IsPostBack)
                {
                    hfType.Value = Request.QueryString["Type"];
                    hfUniqueRowId.Value = Request.QueryString["rowNo"];
                }
            }
            //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
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


        protected void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuException.HasFile)
                {
                    //blnIsValid = UploadedFileContentCheck.IsValidFile(fuException.PostedFile.FileName);

                    //System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fuException.PostedFile.InputStream);
                    //fileData = binaryReader.ReadBytes(fuException.PostedFile.ContentLength);
                    //blnIsExeFile = UploadedFileContentCheck.IsExeFile(fileData);
                    //if (blnIsExeFile)
                    //{
                    //    writeError("The file you're trying to upload seems to contain malicious content" +
                    //     " and cannot be uploaded.");
                    //    return;
                    //}
                    //if (!blnIsValid)
                    //{
                    //    writeError("The upload of this file type is not supported.");
                    //    return;
                    //}

                    string clientFileName = fuException.FileName;
                    string strMsg = "";
                    //<<Modified By Ankur Tyagi on 19Jan2024
                    strMsg = cc.getFileNameErrors(clientFileName);
                    if (!strMsg.Equals(""))
                    {
                        writeError(strMsg);
                        return;
                    }
                    //>>

                    DataTable dtFile = new DataTable();
                    DateTime dt = System.DateTime.Now;
                    string strFileName = Authentication.GetUserID(Page.User.Identity.Name) + "_"
                    + dt.ToString("ddMMyyyyHHmmss") + "_" + clientFileName;
                    string strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["ChecklistFilesFolder"].ToString());
                    string strCompleteName = strServerDirectory + "\\" + strFileName;
                    fuException.SaveAs(strCompleteName);

                    hfClientFileName.Value = clientFileName;
                    hfServerFileName.Value = strFileName;

                    string script = "\r\n<script language=\"javascript\">\r\n" +
                                 " closeFileWindow();" +
                                 "   </script>\r\n";

                    ClientScript.RegisterStartupScript(this.GetType(), "script", script);

                }
                else
                {
                    writeError("Please select file for uploading.");
                    return;
                }
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in btnProcess_Click() :" + ex.Message);
            }
        }

        public void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }
    }
}