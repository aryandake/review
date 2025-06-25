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
    public partial class EditOutward : System.Web.UI.Page
    {
        private DataTable mdtEditFileUpload;
        OutwardUtilitiesBLL outUtilBLL = new OutwardUtilitiesBLL();
        OutwardBL outBL = new OutwardBL();
        Authentication au = new Authentication();
        RefCodesBLL rcBL = new RefCodesBLL();
        //<< Added by ramesh more on 13-Mar-2024 CR_1991
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();
        CommonMethods cm = new CommonMethods();
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
                //<< --Added by Shwetan on 1 - Sep - 2021
                //ddlHardCopysent.Items.AddRange(CommonCodes.GetYesNoDDLItems());
                CommonCodes.SetDropDownDataSource(ddlAddressor, outUtilBLL.GetDataTable("getOutwardAddressor", new DBUtilityParameter("OAM_IS_ACTIVE", "A"), sOrderBy: "OAM_NAME"));
                ddlRepresentationStatus.DataSource = rcBL.getRefCodeDetails("Outward Tracker - Representations Status", "");
                ddlRepresentationStatus.DataBind();
                ddlRepresentationStatus.Items.Insert(0, new ListItem("(Select)", ""));


                ddlTobeSend.DataSource = rcBL.getRefCodeDetails("Outward Tracker - To be Send");
                ddlTobeSend.DataBind();
                ddlTobeSend.Items.Insert(0, new ListItem("(Select)", ""));

                //>>

                Session["EditOutward"] = Server.UrlEncode(DateTime.Now.ToString());
                Session["EditFileOutward"] = null;

                if (Request.QueryString["Id"] != null)
                {
                    string strId = Request.QueryString["Id"].ToString();
                    //<< Modified by ramesh more on 13-Mar-2024 CR_1991
                    hfOTId.Value = encdec.Decrypt(strId);
                    //>>
                    outwardNo.Visible = true;


                    getTypeOfOutward("Edit");
                    DataTable dt = new DataTable();
                    DataRow dr;
                    int intOTId = Convert.ToInt32(hfOTId.Value);
                    dt = outUtilBLL.GetDataTable("getOutwardDetails", new DBUtilityParameter("OT_ID", intOTId));
                    dr = dt.Rows[0];
                    ddlTypeofOutward.SelectedValue = dr["OT_TYPE_ID"].ToString();
                    //<<-- Added by Shwetan on 1-Sep-2021
                    //txtAddressor.Text = dr["OT_ADDRESSOR"].ToString();
                    ddlAddressor.SelectedValue = dr["OT_ADDRESSOR"].ToString();

                    txtEmailsentDate.Text = CommonCode.DbToDispDate(dr["OT_EMAIL_SENT_DT"].ToString());
                    //ddlHardCopysent.SelectedValue = dr["OT_HARD_COPY"].ToString();
                    //txtCourierRefNo.Text = dr["OT_COURIER_REF_NO"].ToString();
                    //txtCouriersentDate.Text = CommonCode.DbToDispDate(dr["OT_COURIER_SENT_DT"].ToString());
                    ddlRepresentationStatus.SelectedValue = dr["OT_REPRESENTATION_STATUS"].ToString();
                    txtRepresentationDate.Text = CommonCode.DbToDispDate(dr["OT_REPRESENTATION_DT"].ToString());
                    //strCreatedBy = txtCreatedby.Text;
                    //txtCreatedby.Text = dr["OT_CREATE_BY"].ToString();
                    //>>
                    txtAddressee.Text = dr["OT_ADRESSEE"].ToString();
                    txtDocName.Text = dr["OT_DOC_NAME"].ToString();
                    txtOutwardDate.Text = CommonCode.DbToDispDate(dr["OT_DATE"].ToString());
                    ddlAuthority.SelectedValue = dr["OT_REG_AUTH"].ToString();
                    ddlDept.SelectedValue = dr["OT_DEPT_ID"].ToString();
                    txtDocNumber.Text = dr["OT_DOCUMENT_NO"].ToString();
                    txtuserRemark.Text = dr["OT_REMARKS"].ToString();
                    txtCancelRemarks.Text = dr["OT_CANCEL_REMARKS"].ToString();
                   // lblStatus.Text = dr["OT_STAUTS"].ToString();
                    txtexistingOutward.Text = dr["OT_BASE_OUTWARD"].ToString();
                    //if (Roles.IsUserInRole("Outward_Admin"))
                    //{
                    //    txtexistingOutward.Enabled = true;
                    //}
                    txtCourier.Text = dr["OT_COURIER_NAME"].ToString();
                    ddlTobeSend.SelectedValue = dr["OT_SEND_TO"].ToString();
                    //ddlTobeSend.Enabled = false;
                    if (dr["OT_DISPATCH_DT"].Equals(DBNull.Value))
                    {
                        txtDispatchDate.Text = "";
                    }
                    else
                    {
                        txtDispatchDate.Text = Convert.ToDateTime(dr["OT_DISPATCH_DT"]).ToString("dd-MMM-yyyy");
                    }
                   
                    if (ddlTobeSend.SelectedValue == "CE")
                    {
                        EmailSendDate.Visible = true;
                        CourierRef.Visible = true;
                        CourierName.Visible = true;
                        CourierSendDate.Visible = true;
                    }
                    else if (ddlTobeSend.SelectedValue == "C")
                    {
                        EmailSendDate.Visible = false;
                        CourierRef.Visible = true;
                        CourierName.Visible = true;
                        CourierSendDate.Visible = true;
                    }
                    else if (ddlTobeSend.SelectedValue == "C")
                    {
                        EmailSendDate.Visible = true;
                        CourierRef.Visible = false;
                        CourierName.Visible = false;
                        CourierSendDate.Visible = false;
                    }

                    if(ddlTypeofOutward.SelectedValue=="2")
                    {
                        rfvRepresentationStatus.Visible = true;
                        sr.Visible = true;
                    }

                    txtAWDPODNumber.Text = dr["OT_POD_NUMBER"].ToString();
                    DataTable dtFiles = outUtilBLL.GetDataTable("getOutwardFiles", new DBUtilityParameter("OF_OT_ID", intOTId));
                    gvFileUpload.DataSource = dtFiles;
                    gvFileUpload.DataBind();
                    BtnBack.Text = "Back";
                    btnbk.Text = "Back";
                    txtCancelRemarks.Visible = true;
                    //if (Roles.IsUserInRole("Outward_Admin"))
                    //{
                    //    if (dr["OT_STAUTS"].ToString() == "Changes suggested by Compliance")
                    //    {
                    //        ddlTypeofOutward.Enabled = true;
                    //        txtOutwardDate.Enabled = true;
                            
                    //    }
                    //}
                }
                else
                {
                    hfOTId.Value = "0";
                    getTypeOfOutward("New");
                    btnbk.Text = "Add more outward";
                    //<< Added and Commented by shwetan on 01-Sep-2021
                    //txtAddressor.Text = au.getUserFullName(Page.User.Identity.Name);
                    //txtCreatedby.Text = au.getUserFullName(Page.User.Identity.Name);
                    //>>
                    //txtOutwardDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    //rfvDispatchDate.Enabled = false;
                    //rfvPODNumber.Enabled = false;
                    //rfvDispatchDate.Enabled = false;
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
            if (!CSVInjectionPrevention.CheckInputValidity(this)) return;
            try
            {
                if (ViewState["EditOutward"].ToString() == Session["EditOutward"].ToString())
                {
                    string strAddressor = "", strAddressee = "", strDocName = "", strOutwardDate = "", strAuthority = "", strDept = "",
                        strRemarks = "",strClosureRemark="", strPODNumber = "", strDispatchDate = "", strCourierName = "",
                        srtEmailsentDate = "", srtHardCopy = "",strStatus="", strCurrentUserName="",
                        srtRepresentation = "", srtRepresentationStatus = "", srtRepresentationDate = "",
                        strCreatedBy = "", strTobeSend = "", outwardNo="";

                    int intTypeOfOutward;
                    string strId = hfOTId.Value;
                    //<< Added and Commenteds by Shwetan on 01-Sep-2021 
                    //strAddressor = txtAddressor.Text;
                    strAddressor = ddlAddressor.SelectedValue;
                    srtEmailsentDate = txtEmailsentDate.Text;
                    srtHardCopy = "";// ddlHardCopysent.SelectedValue;
                    //srtCourierRefNo = txtCourierRefNo.Text;
                    //srtCourierDate = txtCouriersentDate.Text;
                    srtRepresentation = "";// ddlRepresentation.SelectedValue;
                    srtRepresentationStatus = ddlRepresentationStatus.SelectedValue;
                    srtRepresentationDate = txtRepresentationDate.Text;
                    //strCreatedBy = txtCreatedby.Text;
                    //>>
                    strAddressee = cm.getSanitizedString(txtAddressee.Text);
                    strDocName = cm.getSanitizedString(txtDocName.Text);
                    strOutwardDate = txtOutwardDate.Text;
                    strAuthority = ddlAuthority.SelectedValue;
                    strDept = ddlDept.SelectedValue;
                    intTypeOfOutward = Convert.ToInt32(ddlTypeofOutward.SelectedValue);
                    string strCreator = Convert.ToString(Page.User.Identity.Name);
                   // string strCreateBy = txtCreatedby.Text;
                    //Authentication.GetUserID(Page.User.Identity.Name) + "_" + dtUploadDatetime.ToString("ddMMyyyyHHmmss") + "_" + fuEditFileUpload.FileName;
                    int intId = Convert.ToInt32(strId);
                    strRemarks = cm.getSanitizedString(txtuserRemark.Text);
                    strClosureRemark = cm.getSanitizedString(txtRemarks.Text);
                    int intAuthority = Convert.ToInt32(strAuthority);
                    int intDept = Convert.ToInt32(strDept);
                    strPODNumber = cm.getSanitizedString(txtAWDPODNumber.Text);
                    strDispatchDate = txtDispatchDate.Text;
                    strCourierName = cm.getSanitizedString(txtCourier.Text);
                    if (!ddlTobeSend.SelectedValue.Equals(""))
                    {
                        strTobeSend = ddlTobeSend.SelectedValue.ToString();
                    }

                    strStatus = "Closed";
                    if (!Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)).Equals(""))
                    {
                        //strCurrentUserName = au.GetUserDetails(Page.User.Identity.Name).Split(',')[0].ToString();
                        //strCreatedBy = strCurrentUserName + " (" + Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)) + ")";
                        strCreatedBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                    }
                    
                    string strDocNo = outBL.UpdateOutwardTrackers(intId, strAddressor, strAddressee, strDocName,
                                    strOutwardDate, intAuthority, intDept, strCreatedBy, intTypeOfOutward, strRemarks, strClosureRemark, mdtEditFileUpload,
                                    strPODNumber, strDispatchDate, strCourierName,
                                    srtEmailsentDate, srtHardCopy, strCreator, srtRepresentation,
                                    srtRepresentationStatus, srtRepresentationDate, strTobeSend, strStatus);

                    UtilitiesBLL ubll = new UtilitiesBLL();
                    UtilitiesVO objUVO = new UtilitiesVO();
                    objUVO.setCode(" AND OT_ID =" + intId + " ");
                    DataTable dt = ubll.getData("OutwarNo", objUVO);
                    if (dt != null)
                    {
                        DataRow dr1 = dt.Rows[0];
                        outwardNo = dr1["OT_DOCUMENT_NO"].ToString();
                    }
                    sendMailOnOutwardClosed(outwardNo, strCreatedBy, strDocName);
                    writeError("Outward has been closed successfully.");

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
        private void sendMailOnOutwardClosed(string strDocNo, string logedUser, string strDocName)
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
                mcraf.ParamMap.Add("ConfigId", "1096");
                mcraf.ParamMap.Add("To", "Outward_User");
                mcraf.ParamMap.Add("cc", "Comp_User,Admin_User");
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
                txtCancelRemarks.Visible = true;
                strRemarks = txtCancelRemarks.Text;

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
            lblMsg.Text = "";
            FileInfo fileInfo;
            string strFilePath;
            string strFileName;
            string strCompleteFileName;
            strFilePath = Server.MapPath(ConfigurationManager.AppSettings["OutwardFileFolder"].ToString());
            strFileName = gvFileUpload.SelectedDataKey.Value.ToString();
            strCompleteFileName = (strFilePath + ("\\" + strFileName));
            fileInfo = new FileInfo(strCompleteFileName);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            int intId = Convert.ToInt32(gvFileUpload.SelectedValue);
            outUtilBLL.GetDataTable("deleteOutwardFiles", new DBUtilityParameter("OF_ID", intId));
            int intOTId = Convert.ToInt32(hfOTId.Value);
            DataTable dtFileUpload = outUtilBLL.GetDataTable("getOutwardFiles", new DBUtilityParameter("OF_OT_ID", intOTId));
            gvFileUpload.DataSource = dtFileUpload;
            gvFileUpload.DataBind();
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
                    dr["Uploaded By"] = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));//au.getUserFullName(Page.User.Identity.Name.ToString());
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
            if (Request.QueryString["source"] != null)
            {
                string source = Convert.ToString(Request.QueryString["source"]);
                if (source == "SOC")
                {
                    Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx?type='Closure'"));
                }

            }
            //if (Request.QueryString["type"] != null)
            //{
            //    if (Request.QueryString["Id"] != null)
            //    {
            //        Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx?type='Closure'"));
            //    }
            //    else
            //    {
            //        Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx?type='Closure'"));
            //    }
            //}

            //else
            //{
            //    //Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx"));
            //    if (Request.QueryString["Id"] != null)
            //    {
            //        Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx?type='Closure'"));
            //    }
            //    else
            //    {
            //        Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx?type='Closure'"));
            //    }
            //}
        }

        protected void btnbk_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["source"] != null)
            {
                string source = Convert.ToString(Request.QueryString["source"]);
                if (source == "SOC")
                {
                    Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx?type='Closure'"));
                }

            }
            //if (Request.QueryString["Id"] != null)
            //    Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx"));
            //else
            //    Response.Redirect(Global.site_url("Projects/Outward/AddEditOutward.aspx"));
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect(Global.site_url("Default.aspx"));
        }

        protected void cvEmailsentDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        

        protected void ddlTobeSend_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTobeSend.SelectedValue == "E")
            {
                EmailSendDate.Visible = true;
                rfvEmailsentDate.Visible = true;
                CourierRef.Visible = false;
                CourierName.Visible = false;
                CourierSendDate.Visible = false;
            }
            else if (ddlTobeSend.SelectedValue == "C")
            {
                EmailSendDate.Visible = false;
                rfvEmailsentDate.Visible = false;
                CourierRef.Visible = true;
                CourierName.Visible = true;
                CourierSendDate.Visible = true;
            }
            else if (ddlTobeSend.SelectedValue == "CE")
            {
                CourierRef.Visible = true;
                CourierName.Visible = true;
                CourierSendDate.Visible = true;
                EmailSendDate.Visible = true;
                rfvEmailsentDate.Visible = true;
            }
            else
            {
                EmailSendDate.Visible = false;
                rfvEmailsentDate.Visible = false;
                CourierRef.Visible = false;
                CourierName.Visible = false;
                CourierSendDate.Visible = false;
            }



        }

        protected void ddlRepresentationStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRepresentationStatus.SelectedValue == "Closed")
            {
                Representationdate.Visible = true;
                rfvRepresentationDate.Visible = true;
            }
            else
            {
                Representationdate.Visible = false;
                rfvRepresentationDate.Visible = false;
            }
        }
    }
}