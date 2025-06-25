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
using Fiction2Fact.Legacy_App_Code.Outward.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Outward;
using Fiction2Fact.Legacy_App_Code.VO;

namespace Fiction2Fact.Projects.Outward
{
    public partial class AddEditOutward : System.Web.UI.Page
    {
        private DataTable mdtEditFileUpload;
        OutwardUtilitiesBLL outUtilBLL = new OutwardUtilitiesBLL();
        OutwardBL outBL = new OutwardBL();
        Authentication au = new Authentication();
        CommonMethods cm = new CommonMethods();
        RefCodesBLL rcBL = new RefCodesBLL();
        //<< Added by ramesh more on 13-Mar-2024 CR_1991
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();
        //>>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DateTime dtCurrDate = DateTime.Now;
                hfCurDate.Value = dtCurrDate.ToString("dd-MMM-yyyy");

                ddlAuthority.DataSource = outUtilBLL.GetDataTable("OutwardAuthority", sOrderBy: "ORAM_NAME");
                ddlAuthority.DataBind();
                ddlAuthority.Items.Insert(0, new ListItem("(Select)", ""));

                ddlDept.DataSource = outUtilBLL.GetDataTable("OutwardDepartment", sOrderBy: "ODM_NAME");
                ddlDept.DataBind();
                ddlDept.Items.Insert(0, new ListItem("(Select)", ""));
                
                Session["EditOutward"] = Server.UrlEncode(DateTime.Now.ToString());
                Session["EditFileOutward"] = null;

                if (Request.QueryString["Id"] != null)
                {
                    string strId = Request.QueryString["Id"].ToString();
                    //<< Modified by ramesh more on 13-Mar-2024 CR_1991
                    hfOTId.Value = encdec.Decrypt(strId);
                    //>>
                    attacment2.Visible = true;
                    attacment3.Visible = true;
                    pnlFiles.Visible = true;
                    outwardNo.Visible = true;
                    pnlFiles.Visible = true;
                   

                    getTypeOfOutward("Edit");
                    DataTable dt = new DataTable();
                    DataRow dr;
                    int intOTId = Convert.ToInt32(hfOTId.Value);
                    dt = outUtilBLL.GetDataTable("getOutwardDetails", new DBUtilityParameter("OT_ID", intOTId));
                    dr = dt.Rows[0];
                    ddlTypeofOutward.SelectedValue = dr["OT_TYPE_ID"].ToString();
                    txtDocName.Text = dr["OT_DOC_NAME"].ToString();
                    txtOutwardDate.Text = CommonCode.DbToDispDate(dr["OT_DATE"].ToString());
                    ddlAuthority.SelectedValue = dr["OT_REG_AUTH"].ToString();
                    ddlDept.SelectedValue = dr["OT_DEPT_ID"].ToString();
                    txtDocNumber.Text = dr["OT_DOCUMENT_NO"].ToString();
                    txtRemarks.Text = dr["OT_REMARKS"].ToString();
                    txtexistingOutward.Text = dr["OT_BASE_OUTWARD"].ToString();
                    //txtexistingOutward.ReadOnly = true;
                    //gvInsertFileUpload.DataSource = outUtilBLL.GetDataTable("OutwardFiles", new DBUtilityParameter("OF_OT_ID", intOTId));
                    //gvInsertFileUpload.DataBind();//OutwardFiles

                    BtnBack.Text = "Back";
                    btnbk.Text = "Back";


                    //hfstatus.Value= dr["OT_STAUTS"].ToString();
                    //hfcreator.Value= dr["OT_CREATOR"].ToString();
                }
                else
                {
                    hfOTId.Value = "0";
                    getTypeOfOutward("New");
                    btnbk.Text = "Add more outward";
                    
                }

               // ddlRepresentationStatus.Attributes["onchange"] = "onRepresentationStatusChange()";
            }
            else
            {
                if (!(Session["EditFileOutward"] == null))
                {
                    mdtEditFileUpload = (DataTable)Session["EditFileOutward"];
                }
            }
            string script1 = "";
            script1 += "\r\n <script type=\"text/javascript\">\r\n";
            script1 = script1 + "onRepresentationStatusChange();\r\n";
            script1 += "</script>\r\n";
            ClientScript.RegisterStartupScript(this.GetType(), "return script1", script1);
        }

        public void getTypeOfOutward(string strType)
        {
            if (strType.Equals("Edit"))
                ddlTypeofOutward.DataSource = outUtilBLL.GetDataTable("getTypeofOutward", sOrderBy: "OTM_NAME");
            else
                ddlTypeofOutward.DataSource = outUtilBLL.GetDataTable("getTypeofOutward",
                    new DBUtilityParameter("OTM_IS_ACTIVE", "A"), sOrderBy: "OTM_NAME");

            ddlTypeofOutward.DataBind();
            ddlTypeofOutward.Items.Insert(0, new ListItem("(Select)", ""));
        }

        protected void cvOutwardDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["EditOutward"] = Session["EditOutward"];
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            InsertOutwardDetails("S");
        }

        void InsertOutwardDetails(string status)
        {
            if (!CSVInjectionPrevention.CheckInputValidity(this)) return;
            try
            {
                if (ViewState["EditOutward"].ToString() == Session["EditOutward"].ToString())
                {
                    string strDocName = "", strOutwardDate = "", strAuthority = "", strDept = "",
                        strStatus="", strCreator="", strCurrentUserName="",
                        strCreatedBy = "", strRemarks="", strOldOutward="";

                    int intTypeOfOutward;
                    string strId = hfOTId.Value;
                    strDocName = cm.getSanitizedString(txtDocName.Text);
                    if (!txtOutwardDate.Text.Equals(""))
                        strOutwardDate = txtOutwardDate.Text;
                    strAuthority = ddlAuthority.SelectedValue;
                    strDept = ddlDept.SelectedValue;
                    intTypeOfOutward = Convert.ToInt32(ddlTypeofOutward.SelectedValue);
                    int intId = Convert.ToInt32(strId);
                    int intAuthority = Convert.ToInt32(strAuthority);
                    int intDept = Convert.ToInt32(strDept);
                    strRemarks = cm.getSanitizedString(txtRemarks.Text);
                    strOldOutward = cm.getSanitizedString(txtexistingOutward.Text);
                    hfDocNo.Value = cm.getSanitizedString(txtDocNumber.Text);
                    if (status == "S")
                    {
                        strStatus = "Submitted";
                        if (!Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)).Equals(""))
                        {
                            //strCurrentUserName = au.getUserFullName(Page.User.Identity.Name).Split(',')[0].ToString();
                            //strCreatedBy = strCurrentUserName + " (" + Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)) + ")";
                            //strCreatedBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                            strCreatedBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                        }
                        strCreator = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));//Convert.ToString(Page.User.Identity.Name);

                    }
                  
                    string strDocNo = outBL.SaveOutwardTrackers(intId, strDocName,
                                        strOutwardDate, intAuthority, intDept, strCreatedBy, intTypeOfOutward,  mdtEditFileUpload,
                                        strCreator, strStatus, strRemarks, strOldOutward);

                    

                    if (strDocNo!="")
                    {
                        sendMailOnOutwardSubmission(strDocNo, strCreatedBy, strDocName);
                        writeError("Record has been saved successfully with outward no. " + strDocNo);
                    }
                    else
                    {
                        sendMailOnOutwardSubmission(hfDocNo.Value, strCreatedBy, strDocName);
                        writeError("Record has been updated successfully");
                    }

                    
                    hfDoubleClickFlag.Value = "";
                    pnlOutward.Visible = false;
                    pnlMsg.Visible = true;
                }
                else
                {
                    hfDoubleClickFlag.Value = "";
                    writeError("Your attempt to refresh the page was blocked as it would lead to duplication of data.");
                }
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        private void sendMailOnOutwardSubmission(string strDocNo, string logedUser, string strDocName)
        {
            Authentication auth = new Authentication();
            string strMailConfigId = "", strUserId = "", strUserDetails = "", strUserName = "", strEmail = "";
            try
            {
                strUserId = Page.User.Identity.Name;
                strUserDetails = auth.GetUserDetsByEmpCode(strUserId);
                strUserName = strUserDetails.Split('|')[0];
                strEmail = strUserDetails.Split('|')[1];

                MailContent_Outward mcraf = new MailContent_Outward();
                mcraf.ParamMap.Add("ConfigId", "1095");
                mcraf.ParamMap.Add("To", "Comp_User");
                mcraf.ParamMap.Add("cc", "Outward_User,Admin_User");
                mcraf.ParamMap.Add("ModuleCode", "Outward");
                mcraf.ParamMap.Add("Ids", strDocNo);
                mcraf.ParamMap.Add("Subject", strDocName);
                mcraf.ParamMap.Add("CreatorUserId", strUserId);
                mcraf.ParamMap.Add("LoggedInUserName", logedUser);
                mcraf.setOutwardMailContent("");
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                string strRemarks = "";
                string strId = hfOTId.Value;
                int intId = Convert.ToInt32(strId);
                //txtCancelRemarks.Visible = true;
                strRemarks = "";// txtCancelRemarks.Text;

                outBL.cancelOutwardTrackers(intId, strRemarks,"","");
                //lblStatus.Text = "Cancelled";
                writeError("Outward has been cancelled successfully.");
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }

        }

        protected void gvInsertFileUpload_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            FileInfo fileInfo;
            string strFilePath;
            string strFileName;
            string strCompleteFileName;
            strFilePath = Server.MapPath(ConfigurationManager.AppSettings["OutwardFileFolder"].ToString());
            strFileName = gvInsertFileUpload.SelectedDataKey.Value.ToString();
            strCompleteFileName = (strFilePath + ("\\" + strFileName));
            fileInfo = new FileInfo(strCompleteFileName);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            mdtEditFileUpload.Rows.RemoveAt(gvInsertFileUpload.SelectedIndex);
            gvInsertFileUpload.DataSource = mdtEditFileUpload;
            gvInsertFileUpload.DataBind();
        }

        protected void gvFileUpload_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblMsg.Text = "";
            //FileInfo fileInfo;
            //string strFilePath;
            //string strFileName;
            //string strCompleteFileName;
            //strFilePath = Server.MapPath(ConfigurationManager.AppSettings["OutwardFileFolder"].ToString());
            ////strFileName = gvFileUpload.SelectedDataKey.Value.ToString();
            //strCompleteFileName = (strFilePath + ("\\" + strFileName));
            //fileInfo = new FileInfo(strCompleteFileName);
            //if (fileInfo.Exists)
            //{
            //    fileInfo.Delete();
            //}
            //int intId = Convert.ToInt32(gvFileUpload.SelectedValue);
            //outUtilBLL.GetDataTable("deleteOutwardFiles", new DBUtilityParameter("OF_ID", intId));
            //int intOTId = Convert.ToInt32(hfOTId.Value);
            //DataTable dtFileUpload = outUtilBLL.GetDataTable("getOutwardFiles", new DBUtilityParameter("OF_OT_ID", intOTId));
            //gvFileUpload.DataSource = dtFileUpload;
            //gvFileUpload.DataBind();
        }

        protected void btnAddAttachment_Click(object sender, System.EventArgs e)
        {
            lblMsg.Text = "";
            Authentication au = new Authentication();
            DataRow dr;
            FileInfo fileInfo;
            string strCompleteFileName, strSelectedFile;

            if ((fuEditFileUpload.HasFile))
            {
                strSelectedFile = fuEditFileUpload.FileName;
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
                          strSelectedFile.Contains("\"") || strSelectedFile.Contains(","))
                {
                    writeError("Invalid File Name");
                    return;
                }

                string strFileExtension = Path.GetExtension(strSelectedFile);
                if (UploadedFileContentCheck.checkForMaliciousFileFromHeaders(strFileExtension, fuEditFileUpload.FileBytes))
                {
                    writeError("The file contains malicious content. Kindly check the file and reupload.");
                    return;
                }


                string strReturnMsg = UploadedFileContentCheck.checkValidFileForUpload(fuEditFileUpload, "");

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
                try
                {
                    string strFileNameOnClient;
                    string strFileNameOnServer;
                    string strServerDirectory;
                    DateTime dtUploadDatetime;
                    strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["OutwardFileFolder"].ToString());
                    strFileNameOnClient = fuEditFileUpload.FileName;
                    dtUploadDatetime = System.DateTime.Now;
                    strFileNameOnServer = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)) + "_" + dtUploadDatetime.ToString("ddMMyyyyHHmmss") + "_" + fuEditFileUpload.FileName;
                    hfFileNameOnServer.Value = strFileNameOnServer;
                    fuEditFileUpload.SaveAs(strServerDirectory + "\\\\" + strFileNameOnServer);
                    strCompleteFileName = strServerDirectory + "\\" + strFileNameOnServer;
                    fileInfo = new FileInfo(strCompleteFileName);

                    if ((mdtEditFileUpload == null))
                    {
                        initFileUpload();
                    }
                    dr = mdtEditFileUpload.NewRow();
                    dr["FileName"] = strFileNameOnClient;
                    dr["FileNameOnServer"] = strFileNameOnServer;
                    dr["Uploaded By"] = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));//(new Authentication()).getUserFullName(Page.User.Identity.Name);
                    dr["Uploaded On"] = dtUploadDatetime.ToString("dd-MMM-yyyy HH:mm:ss");
                    mdtEditFileUpload.Rows.Add(dr);
                    gvInsertFileUpload.DataSource = mdtEditFileUpload;
                    gvInsertFileUpload.DataBind();
                    Session["EditFileOutward"] = mdtEditFileUpload;
                }
                catch (Exception ex)
                {
                    writeError(("In btnUpload_Click: " + ex.Message));
                }
            }
            else
            {
                writeError("Please select a file for uploading.");
            }
        }

        protected void gvInsertFileUpload_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (((e.Row.RowType == DataControlRowType.DataRow)
                        || (e.Row.RowType == DataControlRowType.Header)))
            {

                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
            }
        }

        private void initFileUpload()
        {
            mdtEditFileUpload = new DataTable();
            mdtEditFileUpload.Columns.Add(new DataColumn("FileName", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("FileNameOnServer", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("Uploaded By", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("Uploaded On", typeof(string)));
        }

        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
            string script = "\r\n<script language=\"javascript\">\r\n" +
                       " alert('" + strError.Replace("'", "\\'") + "');" +
                       "   </script>\r\n";

            ClientScript.RegisterStartupScript(this.GetType(), "script", script);
        }

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] != null)
                Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx"));
            else
                Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx"));
        }

        protected void btnbk_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] != null)
                Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx"));
            else
                Response.Redirect(Global.site_url("Projects/Outward/AddEditOutward.aspx"));
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx"));
        }

        
 
    }
}