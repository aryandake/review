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
using System.Threading;
using Fiction2Fact.App_Code;
using Fiction2Fact;

public partial class NewsTickerDownloadFile : System.Web.UI.Page
{
    string mStrOutFile;

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        Page.Theme = "";
    }

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
            //string filename = "";
            string fileDirectory = "";
            string serverDirectory = "";
            string filepathOnServer = "";
            FileInfo fileInfo;
            string strFileFolder;
            int index;
            string strPassword = "abcd", strCompleteFileName;

            fileDirectory = Request.QueryString["FileInformation"];
            strFileFolder = Request.QueryString["Folder"];
            try
            {
                if (fileDirectory != null && fileDirectory.Length > 0)
                {
                    char[] charsToTrim = { '\\' };

                    serverDirectory = Server.MapPath(ConfigurationManager.AppSettings[strFileFolder].ToString());
                    filepathOnServer = serverDirectory + "\\" + fileDirectory;
                    strCompleteFileName = serverDirectory + "\\" + fileDirectory;
                    index = strCompleteFileName.LastIndexOf(".e");
                    mStrOutFile = strCompleteFileName.Substring(0, index);
                    CryptoHelp.DecryptFile(strCompleteFileName, mStrOutFile, strPassword);
                    fileInfo = new FileInfo(mStrOutFile);
                    //<<Modified by Ankur Tyagi on 20Mar2024 for CR_1991
                    if (fileInfo != null && fileInfo.Exists)
                    {
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
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
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                Response.Write(sMessage);
            }
        }
    }

    //protected void Page_Unload(object sender, EventArgs e)
    //{

    //    FileInfo fileInfo;
    //    fileInfo = new FileInfo(mStrOutFile);
    //    //if (fileInfo != null)
    //    //{
    //    //    fileInfo.Delete();
    //    //}

    //}
}
