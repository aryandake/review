using System;
using System.Configuration;
using System.Web.UI;
using System.IO;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    //[Idunno.AntiCsrf.SuppressCsrfCheck]
    public partial class Certification_DeleteChecklistFile : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;

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

                    string strQuery = "", strId = "";
                    hfUniqueRowId.Value = Request.QueryString["rowNo"];
                    hfServerFileName.Value = Request.QueryString["filename"];
                    hfType.Value = Request.QueryString["Type"];

                    //<<delete file from file system.
                    FileInfo fileInfo;
                    string fileDirectory = Request.QueryString["filename"];
                    if (Request.QueryString["Id"] != null)
                    {
                        strId = Request.QueryString["Id"];
                    }

                    if (fileDirectory != null && fileDirectory.Length > 0)
                    {

                        string serverDirectory = Server.MapPath(ConfigurationManager.AppSettings["ChecklistFilesFolder"].ToString());
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

                    if (hfType.Value.Equals("ChecklistFile"))
                    {
                        strQuery = "update TBL_CERT_CHECKLIST_DETS set CCD_CLIENT_FILENAME ='', CCD_SERVER_FILENAME ='' where CCD_ID =" + strId;
                    }
                    F2FDatabase.F2FExecuteNonQuery(strQuery);

                }
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in deleteFile():" + ex);
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

}