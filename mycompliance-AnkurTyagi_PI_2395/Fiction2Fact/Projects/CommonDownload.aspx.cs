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

public partial class CommonDownload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strType, strFileName, strFolderName = "",
            str, strClientFileName;
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
                strFileName = Request.QueryString["downloadFileName"];
                strClientFileName = Request.QueryString["Filename"];


                if (strType.Equals("ContractCertificate"))
                {
                    strFolderName = "ContractCertificateFiles";
                }
                else if (strType.Equals("ContractPdfDocFiles"))
                {
                    strFolderName = "ContractPdfDocFiles";
                }
                else if (strType.Equals("ContractDocuments"))
                {
                    strFolderName = "ContractDocuments";
                }
                else if (strType.Equals("ContractResponse"))
                {
                    strFolderName = "ContractResponseFiles";
                }
                else if (strType.Equals("ContractTemplate"))
                {
                    strFolderName = "ContractTemplate";
                }
                else if (strType.Equals("Contract"))
                {
                    strFolderName = "ContractFilesFolder";
                }
                else if (strType.Equals("CertificationChecklistTemplate"))
                {
                    strFolderName = "CertificationChecklistTemplate";
                }
                else if (strType.Equals("FileInformation"))
                    strFolderName = "ContractFilesFolder";
                else if (strType.Equals("GeneratedContractDrafts"))
                    strFolderName = "GeneratedContractDrafts";
                else if (strType.Equals("Contract"))
                    strFolderName = "ContractFilesFolder";
                else if (strType.Equals("Checklist"))
                    strFolderName = "ChecklistFiles";
                else if (strType.Equals("ContractChecklistTemplate"))
                    strFolderName = "ContractChecklistTemplate";
                //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                else if (strType.Equals("AddDocDetails"))
                    strFolderName = "FileDetails";
                //>>
                else if (strType.Equals("GeneratedLetters"))
                    strFolderName = "GeneratedLetters";
                //<<Added By Narendra @17Mar2015
                else if (strType.Equals("CD"))
                    strFolderName = "CommercialDetails";
                else if (strType.Equals("LPLD"))
                    strFolderName = "LandlordDetails";
                else if (strType.Equals("ContractRepositoryFiles"))
                    strFolderName = "ContractRepositoryFiles";
                else if (strType.Equals("Circular"))
                    strFolderName = "CircularFilesFolder";
                else if (strType.Equals("ADVT"))
                    strFolderName = "AdvtFileUploadFolder";
                else if (strType.Equals("ChecklistFilesFolder"))
                {
                    strFolderName = "ChecklistFilesFolder";
                }
                else if (strType.Equals("SubmissionFiles"))
                {
                    strFolderName = "SubmissionFiles";
                }
                else if (strType.Equals("HelpDesk"))
                {
                    strFolderName = "HelpDeskFileUploadFolder";
                }
                //<< Added By Ritesh Tak on 24-03-2023 for Compliance Related Folders
                else if (strType.Equals("CRI"))
                {
                    strFolderName = "ComplianceInitiationFileFolder";
                }
                else if (strType.Equals("CRDRQ"))
                {
                    strFolderName = "ComplianceDRQFileFolder";
                }
                else if (strType.Equals("ComplianceIssue"))
                {
                    strFolderName = "ComplianceIssueFileFolder";
                }
                //>>
                //<< Added By Ritesh Tak on 14-04-2023 for Compliance Related Folders
                else if (strType.Equals("ComplianceIssueActionUpdate"))
                {
                    strFolderName = "ComplianceIssueFileFolder";
                }
                //>>
                //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
                else
                {
                    Response.Write("Please provide a file to download.");
                    return;
                }
                //>>


                if (strFolderName != null && strFolderName.Length > 0)
                {
                    string strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings[strFolderName].ToString());
                    string strCompleteFileName = strServerDirectory + "\\" + strFileName;
                    fileInfo = new FileInfo(strCompleteFileName);
                    if (fileInfo != null && fileInfo.Exists)
                    {
                        Response.ClearHeaders();
                        str = fileInfo.Name;
                        //<<Modified by Kiran Kharat on 18Apr2018
                        //while (str.Contains("_"))
                        //{
                        //    str = str.Substring(str.IndexOf('_') + 1);
                        //}
                        // ext = str.Substring(str.IndexOf('_') + 1);
                        // ext = ext.Substring(ext.IndexOf('_') + 1);
                        //>>
                        if (string.IsNullOrEmpty(strClientFileName))
                        {
                            if (str.Split(new char[] { '_' }, 3).Length > 2)
                            {
                                str = str.Split(new char[] { '_' }, 3)[2];
                            }
                        }
                        else
                        {
                            str = strClientFileName;
                        }

                        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + str + "\"");
                        Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                        Response.ContentType = MimeMapping.GetMimeMapping(strCompleteFileName);
                        Response.WriteFile(fileInfo.FullName);
                    }
                    else
                    {
                        Response.Write("Please provide a file to download.");
                    }
                }
                else
                {
                    Response.Write("Folder mapping not exists.");
                }
            }
        }
        catch (Exception ex)
        {
            string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
            Response.Write(sMessage);
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

}
