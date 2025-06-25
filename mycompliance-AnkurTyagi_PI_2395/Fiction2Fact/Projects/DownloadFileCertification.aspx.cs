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
using Fiction2Fact;
using Fiction2Fact.App_Code;

namespace FictionFact.Projects
{
    public partial class DownloadFileCertification : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //Page.Theme = "";
            if (Page != null)
            {
                Page.RegisterRequiresViewStateEncryption();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                    string filename = "";
                    string fileDirectory = "";
                    string serverDirectory = "";
                    string filepathOnServer = "";
                    FileInfo fileInfo;

                    if (User.IsInRole("Certification_Admin") || User.IsInRole("Certification_Compliance_User") || User.IsInRole("Certification_Coordinator") || User.IsInRole("Certification_CXO") || User.IsInRole("Certification_HOD"))
                    {
                        fileDirectory = Request.QueryString["FileInformation"];
                    }
                    else
                    {
                        Response.Write("You do not have the rights to download this file.");
                        return;
                    }

                    if (fileDirectory != null && fileDirectory.Length > 0)
                    {

                        filepathOnServer = fileDirectory;
                        serverDirectory = Server.MapPath(ConfigurationManager.AppSettings["CertificationFilesFolder"].ToString());

                        filename = fileDirectory;

                        fileInfo = new FileInfo(serverDirectory + "\\" + filepathOnServer);
                        if (fileInfo != null && fileInfo.Exists)
                        {
                            Response.ClearHeaders();
                            string str = fileInfo.Name;
                            string ext = str.Substring(str.IndexOf('_') + 1);
                            ext = ext.Substring(ext.IndexOf('_') + 1);
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + ext);
                            //Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                            Response.ContentType = "application/octet-stream";


                            Response.WriteFile(fileInfo.FullName);
                        }

                        else
                        {
                            Response.Write("This file does not exist on Server.");
                        }
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
}