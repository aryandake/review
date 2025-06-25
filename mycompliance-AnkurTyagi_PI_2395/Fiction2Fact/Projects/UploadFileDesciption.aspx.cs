using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.BLL;
using System.Configuration;
using System.Data;

namespace Fiction2Fact.Projects
{
    public partial class UploadFileDesciption : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        CommonMethods cm = new CommonMethods();
        RefCodesBLL refBL = new RefCodesBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.User.Identity.Name.ToString().Equals(""))
            {
                Response.Redirect(Global.site_url("Login.aspx"));
            }
            string strModuleType = "", strFileType = "";
            if (!Page.IsPostBack)
            {
                hfType.Value = Request.QueryString["type"].ToString();

                strModuleType = hfType.Value;
                if (hfType.Value.ToString().Equals("CRI"))
                    strFileType = "Compliance Review - File Type";
                else if (hfType.Value.ToString().Equals("CRDRQ"))
                    strFileType = "ComplianceReview - DRQ Files - File Type";
                else if (hfType.Value.ToString().Equals("ComplianceIssue"))
                    strFileType = "Compliance Review Issue Tracker - File Type";

                DataTable dtFile = refBL.getRefCodeDetails(strFileType, mstrConnectionString);
                ddlFileType.DataSource = dtFile;
                ddlFileType.DataValueField = "RC_CODE";
                ddlFileType.DataTextField = "RC_NAME";
                ddlFileType.DataBind();
                ddlFileType.Items.Insert(0, new ListItem("-- Select --", ""));
            }
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            string strServerDirectory = "", strFolderName = "", strCompleteFileName = "",
                strServerFileName = "";
            try
            {
                if (fuException.HasFile)
                {
                    string strClientFileName = fuException.FileName;
                    string strFileTypeID = ddlFileType.SelectedValue;
                    string strFileType = ddlFileType.SelectedItem.Text;
                    string strType = hfType.Value;
                    string strFileDesc = cm.getSanitizedString(txtFileDesc.Text);
                    //string strFileExtension = Path.GetExtension(strClientFileName);
                    DateTime dt = DateTime.Now;


                    //if (UploadedFileContentCheck.checkForMaliciousFileFromHeaders(strFileExtension, fuException.FileBytes))
                    //{
                    //    writeError("The File contains malicious content. Kindly check the file and reupload.");
                    //    return;
                    //}
                    //strReturnMsg = UploadedFileContentCheck.checkValidFileForUpload(fuException, "");

                    //CommonCode cc = new CommonCode();
                    //strReturnMsg += getFileNameErrors(strClientFileName);
                    //if (!strReturnMsg.Equals(""))
                    //{
                    //    writeError(strReturnMsg);
                    //    return;
                    //}
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


                    if (strType.Equals("CRI"))
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


                    strServerFileName = Authentication.GetUserID(Page.User.Identity.Name) + "_" +
                                    dt.ToString("ddMMyyyyHHmmss") + "_" + strClientFileName.Replace(" ", "_");

                    strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings[strFolderName].ToString());
                    strCompleteFileName = strServerDirectory + "\\" + strServerFileName;

                    fuException.SaveAs(strCompleteFileName);

                    hfClientFileName.Value = strClientFileName.Replace(" ", "_");
                    hfServerFileName.Value = strServerFileName;
                    hfFileType.Value = strFileType;
                    hfFileTypeID.Value = strFileTypeID;
                    hfFileDesc.Value = strFileDesc;

                    string script = "\r\n<script language=\"javascript\">\r\n" +
                                    " closeFileWindow();" +
                                    " </script>\r\n";
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
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        private string getFileNameErrors(string strClientFileName)
        {
            string strReturmMsg = "";

            if (strClientFileName.Length > Global.AppSettings.FullyQualifiedFileNameLength)
            {
                strReturmMsg += "File Name length exceeds permissible length.";
            }
            if (strClientFileName.Contains("&"))
            {
                strReturmMsg += " File Name can't have special character '&'.";
            }
            if (strClientFileName.Contains("#"))
            {
                strReturmMsg += " File Name Shall not have special character '#'.";
            }
            if (strClientFileName.Contains("\'"))
            {
                strReturmMsg += " File Name can't have special character '.";
            }
            if (strClientFileName.Contains("!"))
            {
                strReturmMsg += " File Name can't have special character !.";
            }
            if (strClientFileName.Contains("@"))
            {
                strReturmMsg += " File Name can't have special character @.";
            }
            if (strClientFileName.Contains(","))
            {
                strReturmMsg += " File Name can't have special character ,.";
            }

            return strReturmMsg;
        }
    }
}