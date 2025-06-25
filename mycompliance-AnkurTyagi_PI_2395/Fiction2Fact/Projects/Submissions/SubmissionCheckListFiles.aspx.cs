using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class Submissions_SubmissionCheckListFiles : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
        UtilitiesBLL utilitiesBL = new UtilitiesBLL();
        RefCodesBLL rcBL = new RefCodesBLL();
        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }
        private DataTable mdtSubmissionFileUpload;
        private void initFileUploadDT()
        {
            mdtSubmissionFileUpload = new DataTable();
            //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
            mdtSubmissionFileUpload.Columns.Add(new DataColumn("File Type", typeof(string)));
            mdtSubmissionFileUpload.Columns.Add(new DataColumn("File Type Name", typeof(string)));
            mdtSubmissionFileUpload.Columns.Add(new DataColumn("File Description", typeof(string)));
            //>>
            mdtSubmissionFileUpload.Columns.Add(new DataColumn("File Name", typeof(string)));
            mdtSubmissionFileUpload.Columns.Add(new DataColumn("FileNameOnServer", typeof(string)));
            mdtSubmissionFileUpload.Columns.Add(new DataColumn("Uploaded by", typeof(string)));
            mdtSubmissionFileUpload.Columns.Add(new DataColumn("Uploaded on", typeof(string)));
        }
        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        private void hideError()
        {
            lblMsg.Visible = false;
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
            revSubmissionFiles.ValidationExpression = ConfigurationManager.AppSettings["FileUploadRegex"];
            if (Page.IsPostBack)
            {
                if (!(Session["SubFileUploadDT"] == null))
                {
                    mdtSubmissionFileUpload = (DataTable)(Session["SubFileUploadDT"]);
                    gvFileUpload.DataSource = mdtSubmissionFileUpload;
                    gvFileUpload.DataBind();
                }
            }
            else
            {
                //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                if (!string.IsNullOrEmpty(Request.QueryString["SCId"]) && !string.IsNullOrEmpty(Request.QueryString["OperationType"])
                     && !string.IsNullOrEmpty(Request.QueryString["Type"]))
                {
                    DataTable dtFileType = rcBL.getRefCodeDetails("Submisssion Operation Type", mstrConnectionString);
                    ddlType.DataSource = dtFileType;
                    ddlType.DataBind();
                    ddlType.Items.Insert(0, new ListItem("(Select)", ""));
                    //>>
                    hfSubmissionCheklistId.Value = Convert.ToString(Request.QueryString["SCId"]);
                    if (Request.QueryString["Type"] == "View")
                    {
                        lblPageHeader.InnerText = "View Submission Files";
                        lblPageNote.Visible = false;
                        pnlUpload.Visible = false;
                        gvUploadedFile.Columns[1].Visible = false;
                    }
                    else
                    {
                        lblPageNote.Visible = true;
                        gvUploadedFile.Columns[1].Visible = true;
                        //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                        if (Request.QueryString["OperationType"] == "Submission")
                        {
                            lblPageHeader.InnerText = "Add attachment for Submission";
                        }
                        else if (Request.QueryString["OperationType"] == "Extension")
                        {
                            lblPageHeader.InnerText = "Add attachment for Extension";
                        }
                        else if (Request.QueryString["OperationType"] == "Closure")
                        {
                            lblPageHeader.InnerText = "Add attachment for Closure";
                        }
                        else if (Request.QueryString["OperationType"] == "Reopen")
                        {
                            lblPageHeader.InnerText = "Add attachment for Reopen";
                        }
                        else if (Request.QueryString["OperationType"] == "Resubmission")
                        {
                            lblPageHeader.InnerText = "Add attachment for Resubmission";
                        }
                        //>>
                        pnlUpload.Visible = true;
                    }
                }
                else
                {
                    String strscript = "<script language=javascript>window.close();</script>";
                    if (!ClientScript.IsStartupScriptRegistered("clientScript"))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "clientScript", strscript);
                    }
                }

                fuSubmissionFiles.Attributes.Add("onkeydown", "javascript:return false;");
                fuSubmissionFiles.Attributes.Add("onkeypress", "javascript:return false;");
                Session["SubFileUploadDT"] = null;

                DataTable dtSubFiles = new DataTable();
                dtSubFiles = utilitiesBL.getDatasetWithCondition("SUBMISSIONSFILES_With_RefCode", Convert.ToInt32(hfSubmissionCheklistId.Value), mstrConnectionString);
                gvUploadedFile.DataSource = dtSubFiles;
                gvUploadedFile.DataBind();
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


        protected void btnUploadSubFiles_Click(object sender, EventArgs e)
        {
            // code on upload button
            DataRow dr;
            string strSelectedFile;
            string strCompleteFileName;
            if ((fuSubmissionFiles.HasFile == true))
            {
                try
                {
                    // ***    Upload file to server
                    strSelectedFile = fuSubmissionFiles.FileName;

                    ////<<addde by supriya to check file name correct or not on 09-Jun-2017
                    //string strReturmMsg = "";
                    //CommonCode cc = new CommonCode();
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
                        hfDoubleClickFlag.Value = "";
                        return;
                    }
                    if (strSelectedFile.Contains("!") || strSelectedFile.Contains("@") ||
                              strSelectedFile.Contains("#") || strSelectedFile.Contains("$") ||
                              strSelectedFile.Contains("%") || strSelectedFile.Contains("^") ||
                              strSelectedFile.Contains("&") || strSelectedFile.Contains("'") ||
                              strSelectedFile.Contains("\""))
                    {
                        writeError("Invalid File Name");
                        hfDoubleClickFlag.Value = "";
                        return;
                    }

                    string strFileExtension = Path.GetExtension(strSelectedFile);
                    if (UploadedFileContentCheck.checkForMaliciousFileFromHeaders(strFileExtension, fuSubmissionFiles.FileBytes))
                    {
                        writeError("The file contains malicious content. Kindly check the file and reupload.");
                        hfDoubleClickFlag.Value = "";
                        return;
                    }


                    string strReturnMsg = UploadedFileContentCheck.checkValidFileForUpload(fuSubmissionFiles, "");

                    CommonCode cc = new CommonCode();
                    strReturnMsg += cc.getFileNameErrors(strSelectedFile);
                    if (!strReturnMsg.Equals(""))
                    {
                        writeError(strReturnMsg);
                        hfDoubleClickFlag.Value = "";
                        return;
                    }

                    if (UploadedFileContentCheck.checkForMultipleExtention(strSelectedFile))
                    {
                        writeError("The file uploaded is multiple extensions.");
                        hfDoubleClickFlag.Value = "";
                        return;
                    }
                    //>>

                    string strFileNameOnClient;
                    string strFileNameOnServer;
                    string strServerDirectory;
                    DateTime dtUploadDatetime;
                    strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["SubmissionFiles"].ToString());
                    strFileNameOnClient = fuSubmissionFiles.FileName;
                    dtUploadDatetime = System.DateTime.Now;
                    strFileNameOnServer = Authentication.GetUserID(Page.User.Identity.Name) + "_" + dtUploadDatetime.ToString("ddMMyyyyHHmmss") + "_" + fuSubmissionFiles.FileName;
                    fuSubmissionFiles.SaveAs(strServerDirectory + "\\" + strFileNameOnServer);

                    strCompleteFileName = strServerDirectory + "\\" + strFileNameOnServer;
                    //File Encryption Part;
                    //<<Commented by Ashish Mishra on 21Aug2017
                    //fileInfo = new FileInfo(strCompleteFileName);
                    //strOutFile = strCompleteFileName + ".e";
                    //strEncryptedFileName = strFileNameOnServer + ".e";
                    //CryptoHelp.EncryptFile(strCompleteFileName, strOutFile, strPassWord);
                    //>>

                    if (mdtSubmissionFileUpload == null)
                    {
                        initFileUploadDT();
                    }
                    dr = mdtSubmissionFileUpload.NewRow();
                    //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                    dr["File Type"] = ddlType.SelectedValue;
                    dr["File Type Name"] = ddlType.SelectedItem.Text;
                    dr["File Description"] = txtDescription.Text;
                    //>>
                    dr["File Name"] = strFileNameOnClient;
                    dr["FileNameOnServer"] = strFileNameOnServer;
                    dr["Uploaded by"] = Getfullname(Page.User.Identity.Name);
                    dr["Uploaded on"] = dtUploadDatetime.ToString("dd-MMM-yyyy HH:mm:ss");
                    mdtSubmissionFileUpload.Rows.Add(dr);
                    gvFileUpload.DataSource = mdtSubmissionFileUpload;
                    gvFileUpload.DataBind();
                    Session["SubFileUploadDT"] = mdtSubmissionFileUpload;
                    //if (fileInfo.Exists)
                    //{
                    //    fileInfo.Delete();
                    //}
                    hideError();
                }
                catch (Exception ex)
                {
                    writeError(("In btnUploadSubFiles_Click: " + ex.Message));
                }
            }
            else
            {
                writeError("Please select a file for uploading.");
            }
            hfDoubleClickFlag.Value = "";
            //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
            ddlType.SelectedIndex = 0;
            txtDescription.Text = string.Empty;
            //>>
        }
        protected void gvFileUpload_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (((e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowType == DataControlRowType.Header)))
            {
                //e.Row.Cells[2].Visible = false;
                //e.Row.Cells[3].Visible = false;
            }
        }


        protected void gvFileUpload_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileInfo fileInfo;
            string strFilePath;
            string strFileName;
            string strCompleteFileName;
            strFilePath = Server.MapPath(ConfigurationManager.AppSettings["SubmissionFiles"].ToString());
            strFileName = gvFileUpload.SelectedDataKey.Value.ToString();
            strCompleteFileName = (strFilePath + ("\\" + strFileName));
            fileInfo = new FileInfo(strCompleteFileName);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            mdtSubmissionFileUpload.Rows.RemoveAt(gvFileUpload.SelectedIndex);
            gvFileUpload.DataSource = mdtSubmissionFileUpload;
            gvFileUpload.DataBind();
            hideError();

        }

        protected void gvUploadedFile_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FileInfo fileInfo;
            DataTable dtSubFiles = new DataTable();
            GridViewRow gvrRow;
            string strFilePath;
            string strFileName;
            string strCompleteFileName;
            gvrRow = gvUploadedFile.Rows[gvUploadedFile.SelectedIndex];
            strFileName = ((Label)(gvrRow.FindControl("lblServerFileName"))).Text;
            strFilePath = Server.MapPath(ConfigurationManager.AppSettings["SubmissionFiles"].ToString());
            strCompleteFileName = (strFilePath + ("\\" + strFileName));
            fileInfo = new FileInfo(strCompleteFileName);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            int intValue = Convert.ToInt32(gvUploadedFile.SelectedDataKey.Value);
            utilitiesBL.getDatasetWithCondition("DELETESUBFILES", intValue, mstrConnectionString);
            dtSubFiles = utilitiesBL.getDatasetWithCondition("SUBMISSIONSFILES_With_RefCode", Convert.ToInt32(hfSubmissionCheklistId.Value), mstrConnectionString);
            gvUploadedFile.DataSource = dtSubFiles;
            gvUploadedFile.DataBind();
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            insertSubmissionFiles();
            ClearSession();
            //String strscript = "<script language=javascript>window.opener.location=window.opener.location; window.close();</script>";
            String strscript = "<script language=javascript>window.close();</script>";
            if (!ClientScript.IsStartupScriptRegistered("clientScript"))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "clientScript", strscript);
            }
        }

        private void insertSubmissionFiles()
        {
            try
            {
                int intSubmissionFiles;
                int intSCId = Convert.ToInt32(hfSubmissionCheklistId.Value);
                string strUser = Authentication.GetUserID(Page.User.Identity.Name);
                if (!(mdtSubmissionFileUpload == null))
                {
                    //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                    string strOperationType = Request.QueryString["OperationType"];
                    //>>
                    intSubmissionFiles = SubmissionMasterBLL.insertSubmissionFiles(intSCId, mdtSubmissionFileUpload, strUser, strOperationType, mstrConnectionString);
                }

                writeError("Data saved Successfully");
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }
        private void ClearSession()
        {
            Session["SubFileUploadDT"] = null;
            mdtSubmissionFileUpload = null;
        }
        public static string Getfullname(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                Authentication auth = new Authentication();
                return auth.getUserFullName(s);
            }
            else
            {
                return "";
            }
        }
    }
}