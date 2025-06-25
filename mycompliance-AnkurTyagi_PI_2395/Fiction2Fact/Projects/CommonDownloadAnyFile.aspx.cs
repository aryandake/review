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
using System.IO;
using Fiction2Fact.App_Code;
using Fiction2Fact;

public partial class CommonDownloadAnyFile : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (Page != null)
        {
            Page.RegisterRequiresViewStateEncryption();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string strType, strServerFileName, strFolderName = "", strCompleteFileName, strClientFileName, str, ext;
        FileInfo fileInfo;
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
                strType = Request.QueryString["type"];
                strServerFileName = Request.QueryString["downloadFileName"];
                strClientFileName = Request.QueryString["fileName"];

                if (strType.Equals("Circular"))
                    strFolderName = "CircularFilesFolder";
                else if (strType.Equals("Outward"))
                    strFolderName = "OutwardFileFolder";
                else if (strType.Equals("ADVT"))
                    strFolderName = "AdvtFileUploadFolder";
                else if (strType.Equals("ChecklistFilesFolder"))
                    strFolderName = "ChecklistFilesFolder";


                //<< Certification Related Folders
                else if (strType.Equals("Certification"))
                {
                    strFolderName = "CertificationFilesFolder";
                }
                else if (strType.Equals("CertificationChecklistTemplate"))
                {
                    strFolderName = "CertificationChecklistTemplate";
                }
                else if (strType.Equals("CertificationChecklistMasFolder"))
                {
                    strFolderName = "CertificationChecklistMasFolder";
                }
                //>>

                else if (strType.Equals("FileUploadFolder"))
                    strFolderName = "FileUploadFolder";

                else if (strType.Equals("RepositoryFiles"))
                    strFolderName = "RepositoryFiles";
                else if (strType.Equals("FAQ"))
                    strFolderName = "FAQFileUploadFolder";

                else if (strType.Equals("Outward"))
                    strFolderName = "OutwardFileFolder";
                //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
                else
                {
                    Response.Write("Please provide a file to download.");
                    return;
                }
                //>>

                string strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings[strFolderName].ToString());

                if (strServerDirectory != null && strServerDirectory.Length > 0)
                {
                    strCompleteFileName = strServerDirectory + "\\" + strServerFileName;
                    fileInfo = new FileInfo(strCompleteFileName);
                    //<<Modified by Ankur Tyagi on 20Mar2024 for CR_1991
                    if (fileInfo != null && fileInfo.Exists)
                    {
                        Response.ClearHeaders();

                        str = fileInfo.Name;
                        ext = str.Substring(str.IndexOf('_') + 1);
                        ext = ext.Substring(ext.IndexOf('_') + 1);

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + ext);
                        Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                        Response.ContentType = "application/octet-stream";
                        Response.WriteFile(fileInfo.FullName);
                    }
                    else
                    {
                        Response.Write("Please provide a file to download.");
                    }
                    //>>
                }

            }
        }
        catch (Exception ex)
        {
            string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
            Response.Write(sMessage);
        }
    }
}
