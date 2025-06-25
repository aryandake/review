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
using Fiction2Fact.Legacy_App_Code.Circulars.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
//[Idunno.AntiCsrf.SuppressCsrfCheck]
namespace Fiction2Fact.Projects.Circulars
{
    public partial class Circulars_EditCircularActionables : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        CircularMasterBLL CircularMasterBLL = new CircularMasterBLL();
        UtilitiesBLL utilityBL = new UtilitiesBLL();
        RefCodesBLL rcBL = new RefCodesBLL();
        DataTable mdtFileUpload;
        CommonMethods cm = new CommonMethods();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataTable dtPersonResponsible, dtActionableStatus;
                if (Request.QueryString["CircularId"] != null)
                {
                    hfCircularId.Value = Request.QueryString["CircularId"].ToString();
                }
                if (Request.QueryString["ActionableId"] != null)
                {
                    hfActionableId.Value = Request.QueryString["ActionableId"].ToString();
                }
                if (Request.QueryString["Source"] != null)
                {
                    hfSource.Value = Request.QueryString["Source"].ToString();
                }

                hfCurDate.Value = System.DateTime.Now.ToString("dd-MMM-yyyy");
                ddlUpdateType.DataSource = rcBL.getRefCodeDetails("Circular Actionable Update Type", mstrConnectionString);
                ddlUpdateType.DataBind();
                ddlUpdateType.Items.Insert(0, new ListItem("(Select an option)", ""));

                dtPersonResponsible = utilityBL.getDatasetWithConditionInString("CircularPersonResponsible", " AND CPRM_IS_ACTIVE ='A' ", mstrConnectionString);

                dtActionableStatus = rcBL.getRefCodeDetails("Actionable Status", mstrConnectionString);
                ddlStatus.DataSource = dtActionableStatus;
                ddlStatus.DataBind();
                ddlStatus.Items.Insert(0, new ListItem("(Select an option)", ""));

                getDetails();
            }
            else
            {
                if (!(Session["CircularActionableFiles"] == null))
                {
                    mdtFileUpload = (DataTable)Session["CircularActionableFiles"];
                }
            }
        }
        protected void btnBack_Click(object sender, System.EventArgs e)
        {
            mdtFileUpload = null;
            Session["CircularActionableFiles"] = null;
            gvFileUpload.DataSource = mdtFileUpload;
            if (hfSource.Value.Equals("List"))
                Response.Redirect("CircularList.aspx");
            else
                Response.Redirect("MyActionables.aspx");
        }

        /*protected void bindIntimationDetails(CheckBoxList cbSubmissions, string CircularId)
        {
            DataTable dtIntimationName;
            string strName = null;
            dtIntimationName = utilityBL.getDatasetWithCondition("CIRCULARINTIMATION", Convert.ToInt32(hfCircularId.Value), mstrConnectionString);
            for (int i = 0; i <= dtIntimationName.Rows.Count - 1; i++)
            {
                strName = dtIntimationName.Rows[i]["CMI_CIM_ID"].ToString();
                cbSubmissions.Items.FindByValue(strName).Selected = true;
            }
        }*/


        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            //lblMsg.Visible = true;
            string script = "\r\n<script language=\"javascript\">\r\n" +
                       " alert('" + strError.Replace("'", "\\'") + "');" +
                       "   </script>\r\n";

            ClientScript.RegisterStartupScript(this.GetType(), "script", script);
        }

        private void getDetails()
        {
            try
            {
                Hashtable ParamMap = new Hashtable();
                DataTable dt = new DataTable();
                DataRow dr, dr1;
                DataSet dsCircularDetails = new DataSet();

                dt = CircularMasterBLL.SearchCircularActionable(Convert.ToInt32(hfCircularId.Value), "", "", "", "", hfActionableId.Value, "", mstrConnectionString);

                if (dt.Rows.Count == 0)
                    writeError("No records found.");
                else
                {
                    dr = dt.Rows[0];
                    lblCreator.Text = dr["CDM_NAME"].ToString();
                    lblAuthority.Text = dr["CIA_NAME"].ToString();
                    lblTypeofDocument.Text = dr["CDTM_TYPE_OF_DOC"].ToString();
                    lblTopic.Text = dr["CAM_NAME"].ToString();
                    lblCircularNo.Text = dr["CM_CIRCULAR_NO"].ToString();
                    if (dr["CM_DATE"].ToString().Equals(""))
                    {
                        lblCircularDate.Text = "";
                    }
                    else
                    {
                        lblCircularDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dr["CM_DATE"]));
                    }
                    lblSubject.Text = dr["CM_TOPIC"].ToString();
                    lblGist.Text = dr["CM_DETAILS"].ToString().Replace(Environment.NewLine, "<br />");
                    lblImplications.Text = dr["CM_IMPLICATIONS"].ToString().Replace(Environment.NewLine, "<br />");
                    lblLink.Text = dr["CM_ISSUING_LINK"].ToString();

                    //cbSubmissions.DataSource = utilityBL.getDataset("CIRCULARINTIMATIONS", mstrConnectionString);
                    //cbSubmissions.DataBind();
                    //bindIntimationDetails(cbSubmissions, hfCircularId.Value);

                    ParamMap.Add("CMId", hfCircularId.Value);
                    dsCircularDetails = CircularMasterBLL.getCircularDetails(ParamMap, mstrConnectionString);

                    DataTable dtActionable = new DataTable();
                    dtActionable = dt;
                    dr1 = dtActionable.Rows[0];
                    txtActionable.Text = dr1["CA_ACTIONABLE"].ToString();
                    // ddlPersonResponsible.SelectedValue = dr1["CA_PERSON_RESPONSIBLE"].ToString();
                    //lblPersonResponsible.Text = dr1["CPRM_NAME"].ToString();
                    //Added By Milan Yadav on 27-Aug-2016
                    //>>
                    txtPersonResponsibleUserId.Text = dr1["CA_PERSON_RESPONSIBLE_ID"].ToString();
                    txtPersonResponsibleUserName.Text = dr1["CA_PERSON_RESPONSIBLE_NAME"].ToString();
                    txtPersonResponsibleEmailId.Text = dr1["CA_PERSON_RESPONSIBLE_EMAIL_ID"].ToString();
                    hfReportingMgrEmailId.Value = dr1["CA_Reporting_Mgr_EMAIL_ID"].ToString();
                    //<<

                    if (dr1["CA_TARGET_DATE"].ToString().Equals(""))
                    {
                        txtTargetDate.Text = "";
                    }
                    else
                    {
                        txtTargetDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dr1["CA_TARGET_DATE"]));
                    }
                    if (dr1["CA_COMPLETION_DATE"].ToString().Equals(""))
                    {
                        txtCompletionDate.Text = "";
                    }
                    else
                    {
                        txtCompletionDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dr1["CA_COMPLETION_DATE"]));
                    }

                    ddlStatus.SelectedValue = dr1["CA_STATUS"].ToString();
                    //lblStatus.Text = dr1["Status"].ToString();
                    txtCirActRemarks.Text = dr1["CA_REMARKS"].ToString();

                    DataTable dtFiles = new DataTable();
                    dtFiles = dsCircularDetails.Tables[1];
                    gvViewFileUpload.DataSource = dtFiles;
                    gvViewFileUpload.DataBind();

                    DataTable dtCircularActionableUpdates = new DataTable();
                    dtCircularActionableUpdates = CircularMasterBLL.SearchCircularActionableUpdates(Convert.ToInt32(hfActionableId.Value), mstrConnectionString);
                    Session["CircularActionable"] = dtCircularActionableUpdates;
                    gvCircularActionableUpdates.DataSource = dtCircularActionableUpdates;
                    gvCircularActionableUpdates.DataBind();
                    Session["CircularActionableUpdates"] = dtCircularActionableUpdates;
                }
            }
            catch (Exception e)
            {
                writeError("Exception in getDetails : " + e.Message);
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this))
            {
                hfDoubleClickFlag.Value = "";
                return;
            }
            int intActionableId = Convert.ToInt32(hfActionableId.Value);
            string strClientFileName = "", strCreateBy = "";

            strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
            strClientFileName = fuFileUpload.FileName.ToString();
            //if (strClientFileName.Contains("!") || strClientFileName.Contains("@") ||
            //                    strClientFileName.Contains("#") || strClientFileName.Contains("$") ||
            //                    strClientFileName.Contains("%") || strClientFileName.Contains("^") ||
            //                    strClientFileName.Contains("&") || strClientFileName.Contains("'") ||
            //                    strClientFileName.Contains("\""))
            //{
            //    hfDoubleClickFlag.Value = "";
            //    writeError("File Name can't have special character.");
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
            if (UploadedFileContentCheck.checkForMaliciousFileFromHeaders(strFileExtension, fuFileUpload.FileBytes))
            {
                writeError("The file contains malicious content. Kindly check the file and reupload.");
                return;
            }


            string strReturnMsg = UploadedFileContentCheck.checkValidFileForUpload(fuFileUpload, "");

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
            DateTime dt = System.DateTime.Now;

            string strfilename = Authentication.GetUserID(Page.User.Identity.Name) + "_" +
                 dt.ToString("ddMMyyyyHHmmss") + "_" + fuFileUpload.FileName;
            string strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["CircularFilesFolder"].ToString());
            string strCompleteName = strServerDirectory + "\\" + strfilename;
            fuFileUpload.SaveAs(strCompleteName);

            //int intId = CircularMasterBLL.saveCircularActionableUpdates(0,
            //    intActionableId, ddlUpdateType.SelectedValue.ToString(), txtRemarks.Text.ToString(), strClientFileName,
            //    strfilename, strCreateBy, mstrConnectionString);

            writeError("Actionable update saved successfully.");

            sendCircularActionableUpdatesMail();

            ddlUpdateType.SelectedValue = "";
            txtRemarks.Text = "";
            hfDoubleClickFlag.Value = "";

            DataTable dtCircularActionableUpdates = new DataTable();
            dtCircularActionableUpdates = CircularMasterBLL.SearchCircularActionableUpdates(Convert.ToInt32(hfActionableId.Value), mstrConnectionString);
            Session["CircularActionableFiles"] = dtCircularActionableUpdates;
            gvCircularActionableUpdates.DataSource = dtCircularActionableUpdates;
            gvCircularActionableUpdates.DataBind();
            getDetails();
        }


        private void sendCircularActionableUpdatesMail()
        {
            try
            {
                string strSubmittedBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                string[] strUsers = new string[0];
                string[] strUsers1 = new string[0];
                string[] strTo = new string[0];
                string[] strCC = new string[0];
                string strSubject;
                DateTime dt = System.DateTime.Now;
                string strHostingServer, strFooter;
                string strContent;
                DateTime dtn = System.DateTime.Now;
                int intCount3;
                Mail mm = new Mail();
                MembershipUser user;
                strHostingServer = ConfigurationManager.AppSettings["HostingServer"].ToString();
                strFooter = ConfigurationManager.AppSettings["MailFooter"].ToString();

                MailConfigBLL mailBLL = new MailConfigBLL();
                DataTable dtMailConfig = mailBLL.searchMailConfig(null, "Circular Actionable Update");
                if (dtMailConfig == null || dtMailConfig.Rows.Count == 0)
                {
                    writeError("Mail Configuration is not set, could not send mail");
                    return;
                }
                DataRow drMailConfig = dtMailConfig.Rows[0];
                strSubject = drMailConfig["MCM_SUBJECT"].ToString();
                strContent = drMailConfig["MCM_CONTENT"].ToString();

                strSubject = strSubject.Replace("%UpdatedBy%", strSubmittedBy);

                strContent = strContent.Replace("%UpdatedBy%", strSubmittedBy);
                strContent = strContent.Replace("%UpdatedDate%", dt.ToString("dd-MMM-yyyy HH:mm"));
                strContent = strContent.Replace("%CirNo%", lblCircularNo.Text);
                strContent = strContent.Replace("%CirActionable%", txtActionable.Text.ToString().Replace(Environment.NewLine, "<br />"));
                strContent = strContent.Replace("%ResponsiblePerson%", txtPersonResponsibleUserName.Text);
                strContent = strContent.Replace("%TargetDate%", txtTargetDate.Text);
                strContent = strContent.Replace("%CompletionDate%", txtCompletionDate.Text);
                strContent = strContent.Replace("%UpdateStatus%", ddlStatus.SelectedItem.Text);
                strContent = strContent.Replace("%UpdateType%", ddlUpdateType.SelectedItem.Text);
                strContent = strContent.Replace("%UpdateDetails%", txtRemarks.Text);
                strContent = strContent.Replace("%ActionLink%", "<a href=\"" + Global.site_url("Projects/Circulars/EditCircularActionables.aspx?ActionableId=" + hfActionableId.Value +
                            "&CircularId=" + hfCircularId.Value) +
                            "\" target=\"_blank\">Click here</a> to view the actionable.");
                strContent = strContent.Replace("%Footer%", strFooter);

                strUsers = Roles.GetUsersInRole("CircularAdmin");
                strUsers1 = Roles.GetUsersInRole("CircularUser");
                strTo = new string[strUsers.Length + strUsers1.Length + 1];

                int intCounter = 0;
                for (intCount3 = 0; intCount3 < strUsers.Length; intCount3++)
                {
                    user = Membership.GetUser(strUsers[intCount3]);
                    strTo[intCounter] = user.Email;
                    intCounter++;
                }
                for (int inti = 0; inti < strUsers1.Length; inti++)
                {
                    user = Membership.GetUser(strUsers1[inti]);
                    strTo[intCounter] = user.Email;
                    intCounter++;
                }
                strTo[strTo.Length - 1] = txtPersonResponsibleEmailId.Text;
                strCC = new string[2];
                user = Membership.GetUser(Page.User.Identity.Name.ToString());
                strCC[0] = user.Email;
                strCC[1] = hfReportingMgrEmailId.Value;


                //strContent = "<html><head><title>Circular Actionable Updates Mail</title></head> " +
                //                  "<body style=\"font-size: 10pt; font-family: Zurich BT\">Hello All, <br/><br/>" +
                //                  "An update has been added to an actionable by " + strSubmittedBy + " on " + dt.ToString("MMM dd,yyyy") + " at " + dt.ToString("hh.mm tt") +
                //                  ". The actionable and  the update are as below: " +
                //                    "<br/><br/>Circular No. : " + lblCircularNo.Text +
                //                   "<br/><br/>Actionable : " + txtActionable.Text.ToString().Replace(Environment.NewLine, "<br />") +
                //                   //"<br/><br/>Person Responsible : " + ddlPersonResponsible.SelectedItem.Text +
                //                   "<br/><br/>Person Responsible : " + txtPersonResponsibleUserName.Text +
                //                    "<br/><br/>Target Date  : " + txtTargetDate.Text +
                //                    "<br/><br/>Completion Date  : " + txtCompletionDate.Text +
                //                   "<br/><br/>Status : " + ddlStatus.SelectedItem.Text +
                //                   "<br/><br/>Update Type : " + ddlUpdateType.SelectedItem.Text +
                //                   "<br/><br/>Update Details : " + txtRemarks.Text.ToString().Replace(Environment.NewLine, "<br />") +
                //                   "<br/></br><br/>" +
                //                  " " + strFooter + "<br/><br/>THIS IS AN AUTO GENERATED MAIL PLEASE DO NOT REPLY BACK.<br/><br/></body></html>";

                mm.sendAsyncMail(strTo, strCC, null, strSubject, strContent);
            }
            catch (Exception ex)
            {
                writeError("Error while sendCircularActionableUpdatesMail mail: " + ex.Message);
            }
        }


        protected void btnSaveActionable_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this))
            {
                hfDoubleClickFlag.Value = "";
                return;
            }
            try
            {
                string strCreateBy = "";
                strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);

                int intId = CircularMasterBLL.UpdateCircularActionables(Convert.ToInt32(hfActionableId.Value),
                    Convert.ToInt32(hfCircularId.Value), cm.getSanitizedString(txtActionable.Text.ToString()), cm.getSanitizedString(txtPersonResponsibleUserId.Text),
                    cm.getSanitizedString(txtPersonResponsibleUserName.Text), cm.getSanitizedString(txtPersonResponsibleEmailId.Text),
                    txtTargetDate.Text.ToString(), txtCompletionDate.Text.ToString(), ddlStatus.SelectedValue.ToString(),
                    cm.getSanitizedString(txtCirActRemarks.Text.ToString()), cm.getSanitizedString(txtClosureRemarks.Text.ToString()), strCreateBy, mdtFileUpload, mstrConnectionString);//ddlPersonResponsible.SelectedValue.ToString()

                writeError("Actionable saved successfully.");
                mdtFileUpload = null;
                Session["CircularActionableFiles"] = null;
                gvFileUpload.DataSource = mdtFileUpload;
                // gvFileUpload.DataBind();
            }
            catch (Exception ex)
            {
                writeError("Error while btnSaveActionable_Click : " + ex.Message);
            }
        }
        //Added By Milan yadav on 25-Aug-2016
        //>>
        protected void cvTodate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
        //<<
        protected void gvCircularActionableUpdates_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCircularActionableUpdates.PageIndex = e.NewPageIndex;
            gvCircularActionableUpdates.DataSource = (DataTable)(Session["CircularActionableUpdates"]);
            gvCircularActionableUpdates.DataBind();
            hfTabberId.Value = "2";
        }

        //Added By Milan Yadav on 30-Aug-2016
        //>>
        private void initFileUploadDetailedReportsDT()
        {
            mdtFileUpload = new DataTable();
            mdtFileUpload.Columns.Add(new DataColumn("Type", typeof(string)));
            mdtFileUpload.Columns.Add(new DataColumn("FileName", typeof(string)));
            mdtFileUpload.Columns.Add(new DataColumn("FileNameOnServer", typeof(string)));
            mdtFileUpload.Columns.Add(new DataColumn("Uploaded By", typeof(string)));
            mdtFileUpload.Columns.Add(new DataColumn("Uploaded On", typeof(string)));
        }
        protected void btnActionableAttachment_Click(object sender, EventArgs e)
        {
            DataRow dr;
            bool blnIsValid, blnIsExeFile;
            byte[] fileData;
            lblMsg.Text = "";
            string strFileNameOnClient;
            string strFileNameOnServer;
            string strServerDirectory;
            DateTime dtUploadDatetime;
            string strCompleteFileName, strSelectedFile;
            if ((fuActionableFileUpload.HasFile))
            {
                try
                {
                    blnIsValid = UploadedFileContentCheck.IsValidFile(fuActionableFileUpload.PostedFile.FileName);

                    System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fuActionableFileUpload.PostedFile.InputStream);
                    fileData = binaryReader.ReadBytes(fuActionableFileUpload.PostedFile.ContentLength);
                    blnIsExeFile = UploadedFileContentCheck.IsExeFile(fileData);
                    if (blnIsExeFile)
                    {
                        writeError("The file you're trying to upload seems to contain malicious content" +
                         " and cannot be uploaded.");
                        return;
                    }
                    if (!blnIsValid)
                    {
                        writeError("The upload of this file type is not supported.");
                        return;
                    }

                    strSelectedFile = fuActionableFileUpload.FileName;
                    if (strSelectedFile.Length > 50)
                    {
                        writeError("File name cannot exceed 50 characters");
                        return;
                    }
                    else if (strSelectedFile.Contains("!") || strSelectedFile.Contains("@") ||
                                strSelectedFile.Contains("#") || strSelectedFile.Contains("$") ||
                                strSelectedFile.Contains("%") || strSelectedFile.Contains("^") ||
                                strSelectedFile.Contains("&") || strSelectedFile.Contains("'") ||
                                strSelectedFile.Contains("\""))
                    {
                        writeError("File Name can't have special character '&'.");
                        return;
                    }

                    strServerDirectory = Server.MapPath(ConfigurationManager.
                        AppSettings["CircularFilesFolder"].ToString());
                    strFileNameOnClient = fuActionableFileUpload.FileName;
                    dtUploadDatetime = System.DateTime.Now;
                    strFileNameOnServer = Authentication.GetUserID(Page.User.Identity.Name) + "_" +
                        dtUploadDatetime.ToString("ddMMyyyyHHmmss") + "_" + fuActionableFileUpload.FileName;
                    hfFileNameOnServer.Value = strFileNameOnServer;
                    fuActionableFileUpload.SaveAs(strServerDirectory + "\\\\" + strFileNameOnServer);
                    strCompleteFileName = strServerDirectory + "\\" + strFileNameOnServer;
                    if ((mdtFileUpload == null))
                    {
                        initFileUploadDetailedReportsDT();
                    }
                    dr = mdtFileUpload.NewRow();
                    dr["Type"] = "";
                    dr["FileName"] = strFileNameOnClient;
                    dr["FileNameOnServer"] = strFileNameOnServer;
                    dr["Uploaded By"] = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                    dr["Uploaded On"] = dtUploadDatetime.ToString("dd-MMM-yyyy HH:mm:ss");
                    mdtFileUpload.Rows.Add(dr);
                    gvFileUpload.DataSource = mdtFileUpload;
                    gvFileUpload.DataBind();
                    Session["CircularActionableFiles"] = mdtFileUpload;
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
        protected void gvFileUpload_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileInfo fileInfo;
            string strFilePath;
            string strFileName;
            string strCompleteFileName;
            strFilePath = Server.MapPath(ConfigurationManager.AppSettings["CircularFilesFolder"].ToString());
            strFileName = gvFileUpload.SelectedDataKey.Value.ToString();
            strCompleteFileName = (strFilePath + ("\\" + strFileName));
            fileInfo = new FileInfo(strCompleteFileName);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            mdtFileUpload.Rows.RemoveAt(gvFileUpload.SelectedIndex);
            gvFileUpload.DataSource = mdtFileUpload;
            gvFileUpload.DataBind();
            writeError("File Deleted Successfully...");
        }
        protected void gvFileUpload_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (((e.Row.RowType == DataControlRowType.DataRow)
                      || (e.Row.RowType == DataControlRowType.Header)))
            {

                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
            }
        }
        //<<
    }
}