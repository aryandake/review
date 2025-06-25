using Fiction2Fact.Legacy_App_Code.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using Fiction2Fact.Legacy_App_Code;
using DocumentFormat.OpenXml.Vml;
using Fiction2Fact.App_Code;
using System.Text;
using System.Data;
using DocumentFormat.OpenXml.Bibliography;
using System.Net.NetworkInformation;
using Fiction2Fact.Legacy_App_Code.DAL;
using Fiction2Fact.Legacy_App_Code.VO;
using AngleSharp.Io;
using Fiction2Fact.Legacy_App_Code.Compliance;

namespace Fiction2Fact.Projects.Compliance
{
    public partial class AddResponse : System.Web.UI.Page
    {
        RefCodesBLL refBLL = new RefCodesBLL();
        ComplianceReviewBLL drqmBLL = new ComplianceReviewBLL();
        string script = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.User.Identity.Name.ToString().Equals(""))
            {
                Response.Redirect(Global.site_url("Login.aspx"));
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                    hfId.Value = Request.QueryString["Id"].ToString();
                else
                    hfId.Value = "0";

                if (Request.QueryString["DRId"] != null)
                    hfDRId.Value = Request.QueryString["DRId"].ToString();
                else
                    hfDRId.Value = "0";

                if (Request.QueryString["Type"] != null)
                    hfType.Value = Request.QueryString["Type"].ToString();
                else
                    hfType.Value = "";

                if (Request.QueryString["User"] != null)
                    hfUser.Value = Request.QueryString["User"].ToString();
                else
                    hfUser.Value = "";

                if (Request.QueryString["Source"] != null)
                    hfUserType.Value = Request.QueryString["Source"].ToString();
                else
                    hfUserType.Value = "";

                if (Request.QueryString["Src"] != null)
                    hfSource.Value = Request.QueryString["Src"].ToString();
                else
                    hfSource.Value = "";

                if (hfType.Value.Equals("Add"))
                {
                    mvDRQMResponse.ActiveViewIndex = 0;
                    lblHeader.Text = "Add Response";
                    if (hfUser.Value.Equals("US"))
                    {
                        gvEditResponse.Visible = true;

                        gvEditResponse.DataSource = drqmBLL.getDRQMResponse(Convert.ToInt32(hfDRId.Value),
                        Convert.ToInt32(hfId.Value), hfSource.Value, hfUser.Value, hfType.Value);
                        gvEditResponse.DataBind();

                        gvEditResponse.Columns[1].Visible = true;
                        gvEditResponse.Columns[5].Visible = true;
                    }
                    else
                    {
                        gvEditResponse.Visible = true;

                        gvEditResponse.DataSource = drqmBLL.getDRQMResponse(Convert.ToInt32(hfDRId.Value),
                        Convert.ToInt32(hfId.Value), hfSource.Value, hfUser.Value, hfType.Value);
                        gvEditResponse.DataBind();

                        gvEditResponse.Columns[1].Visible = false;
                        gvEditResponse.Columns[5].Visible = false;
                    }
                }
                else if (hfType.Value.Equals("View"))
                {
                    mvDRQMResponse.ActiveViewIndex = 1;
                    lblHeader.Text = "View Response";
                    gvResponse.DataSource = drqmBLL.getDRQMResponse(Convert.ToInt32(hfDRId.Value),
                        Convert.ToInt32(hfId.Value), hfSource.Value, hfUser.Value);
                    gvResponse.DataBind();

                    if (hfUser.Value.Equals("RM"))
                        gvResponse.Columns[4].Visible = false;
                }
                else if (hfType.Value.Equals("ViewSent"))
                {
                    mvDRQMResponse.ActiveViewIndex = 1;
                    lblHeader.Text = "View Response";
                    gvResponse.DataSource = drqmBLL.getDRQMResponse(Convert.ToInt32(hfDRId.Value),
                        Convert.ToInt32(hfId.Value), hfSource.Value, "RM");
                    gvResponse.DataBind();

                    gvResponse.Columns[4].Visible = false;
                }

                DataTable dt = drqmBLL.getDRQMDetails(Convert.ToInt32(hfDRId.Value), 0, null, 0, null);

                if (dt.Rows.Count > 0)
                {
                    lblQuery.Text = dt.Rows[0]["CDQ_QUERY_DATA_REQUIREMENT"].ToString().Replace(Environment.NewLine, "<br />");
                }

                ddlUpdateType.DataSource = refBLL.getRefCodeDetails("ComplianceReview - DRQ - Update Type");
                ddlUpdateType.DataBind();
                ddlUpdateType.Items.Insert(0, new ListItem("-- Select --", ""));

                litAttachment.Text = getAttachmentHTMLTable() + "</table>";

                if (hfUser.Value.Equals("RM"))
                    btnSave.Text = "Save";
                else if (hfUser.Value.Equals("US"))
                    btnSave.Text = "Save";
            }

            script += "\r\n <script type=\"text/javascript\">\r\n";
            //script += " onStatusChanges();\r\n";
            script += "</script>\r\n";
            ClientScript.RegisterStartupScript(this.GetType(), "", script);
        }


        private string getAttachmentHTMLTable()
        {
            StringBuilder sbAttachmentHTML = new StringBuilder();

            sbAttachmentHTML.Append(" <table id='tblAttachment' width='100%' class='table table-bordered footable' cellpadding='0' cellspacing='1'> " +
                                    " <thead> " +
                                    " <tr> " +
                                    " <th class='contentBody' style=\"text-align:left;\"> " +
                                    " <input type='checkbox' ID='HeaderLevelCheckBoxAttachment' onclick = 'return onAttachmentHeaderRowChecked()'/> " +
                                    " </th>  " +
                                    " <th class='contentBody' style=\"text-align:left;\"> " +
                                    " File Type " +
                                    " </th> " +
                                    " <th class='contentBody' style=\"text-align:left;\"> " +
                                    " File Description " +
                                    " </th> " +
                                    " <th class='contentBody' style=\"text-align:left;\"> " +
                                    " Attachment Name " +
                                    " </th> " +
                                    " </tr> " +
                                    " </thead> ");

            return sbAttachmentHTML.ToString();
        }

        private DataTable getAttachmentdt()
        {
            string strAttachment = hfAttachment.Value;
            string[] strarrAttachment, strarrFields;
            string strTemp;
            DataRow dr;

            DataTable dtAttachment = new DataTable();
            dtAttachment.Columns.Add(new DataColumn("AttachId", typeof(string)));
            dtAttachment.Columns.Add(new DataColumn("ServerFileName", typeof(string)));
            dtAttachment.Columns.Add(new DataColumn("ClientFileName", typeof(string)));
            dtAttachment.Columns.Add(new DataColumn("FileTypeId", typeof(string)));
            dtAttachment.Columns.Add(new DataColumn("FileTypeName", typeof(string)));
            dtAttachment.Columns.Add(new DataColumn("FileTypeDesc", typeof(string)));
            dtAttachment.Columns.Add(new DataColumn("Type", typeof(string)));

            strarrAttachment = strAttachment.Split('~');
            for (int i = 0; i < strarrAttachment.Length - 1; i++)
            {
                strTemp = strarrAttachment[i];
                strarrFields = strTemp.Split('|');
                dr = dtAttachment.NewRow();

                dr["AttachId"] = strarrFields[0];
                dr["ClientFileName"] = strarrFields[1];
                dr["ServerFileName"] = strarrFields[2];
                dr["FileTypeId"] = strarrFields[3];
                dr["FileTypeName"] = strarrFields[4];
                dr["FileTypeDesc"] = strarrFields[5].ToString();
                dr["Type"] = "DRQMResponseFile";
                dtAttachment.Rows.Add(dr);
            }
            return dtAttachment;
        }

        private void clearField()
        {
            hfId.Value = "";
            ddlUpdateType.SelectedIndex = -1;
            txtResponse.Text = "";
            litAttachment.Text = "";
            hfDoubleClickFlag.Value = "";
        }

        private void writeError(string strError)
        {
            if (hfType.Value.Equals("Add"))
            {
                lblMsg.Text = strError;
                lblMsg.Visible = true;
            }
            else if (hfType.Value.Equals("View"))
            {
                lblMsg1.Text = strError;
                lblMsg1.Visible = true;
            }
        }

        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }

        protected DataTable LoadDRQMFileList(object Id)
        {
            return drqmBLL.getDRQMFiles(0, null, Convert.ToInt32(Id), "DRQMResponseFile");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtFiles = new DataTable();
                string strLoggedInUser = "", strScript = "";
                int intId = 0, intDRId = 0;
                bool res = int.TryParse(hfId.Value, out intId);
                bool res1 = int.TryParse(hfDRId.Value, out intDRId);

                dtFiles = getAttachmentdt();

                strLoggedInUser = (new Authentication()).getUserFullName(Page.User.Identity.Name);

                intId = drqmBLL.saveDRQMResponse(intId, "CR", intDRId, ddlUpdateType.SelectedValue, txtResponse.Text,
                    strLoggedInUser, dtFiles, hfUser.Value);

                clearField();

                if (hfUser.Value.Equals("RM"))
                {
                    sendMailOnDRQMResponse(intId, intDRId);

                    drqmBLL.submitForOperation(0, null, "SubmitAllQueriesResponse", "RM", intRiskReviewDraftId: intDRId);

                    gvEditResponse.Visible = true;

                    gvEditResponse.DataSource = drqmBLL.getDRQMResponse(intDRId, 0, hfSource.Value, hfUser.Value, hfType.Value);
                    gvEditResponse.DataBind();

                    gvEditResponse.Columns[1].Visible = false;
                    gvEditResponse.Columns[5].Visible = false;

                    strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                                "alert('Response have been sent successfully.');\r\n" +
                                "window.close();\r\n" +
                                "window.opener.document.getElementById('btnRefresh').click();\r\n" +
                                "</script>\r\n";
                }
                else
                {
                    //sendMailOnDRQMResponse(intId, intDRId);
                    sendMailOnDRQMResponse(Convert.ToString(intId));
                    drqmBLL.submitForOperation(0, null, "SubmitAllQueriesResponse", "US", intRiskReviewDraftId: intDRId);

                    gvEditResponse.Visible = true;

                    gvEditResponse.DataSource = drqmBLL.getDRQMResponse(intDRId, 0, hfSource.Value, hfUser.Value, hfType.Value);
                    gvEditResponse.DataBind();

                    gvEditResponse.Columns[1].Visible = false;
                    gvEditResponse.Columns[5].Visible = true;

                    strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                                "alert('Response has been submitted successfully.');\r\n" +
                                "window.close();\r\n" +
                                "window.opener.location.reload(true);\r\n" +
                                "</script>\r\n";
                }

                ClientScript.RegisterStartupScript(this.GetType(), "script;", strScript);

                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void gvEditResponse_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dtFiles = new DataTable();
            DataRow dr, drFiles;
            bool res = false;
            int intId = 0, intDRId = 0, uniqueRowId = 0;
            StringBuilder sbAttachmentHTMLRows = new StringBuilder();

            try
            {
                res = int.TryParse(hfDRId.Value, out intDRId);
                res = int.TryParse(gvEditResponse.SelectedValue.ToString(), out intId);

                dt = drqmBLL.getDRQMResponse(intDRId, intId, hfSource.Value);
                dr = dt.Rows[0];

                hfId.Value = (dr["CRDU_ID"] is DBNull ? "0" : dr["CRDU_ID"].ToString());
                ddlUpdateType.SelectedValue = (dr["CRDU_UPDATE_TYPE"] is DBNull ? "" : dr["CRDU_UPDATE_TYPE"].ToString());
                txtResponse.Text = (dr["CRDU_REMARKS"] is DBNull ? "" : dr["CRDU_REMARKS"].ToString());

                #region Files
                script += "\r\n <script type=\"text/javascript\">\r\n";
                litAttachment.Text = getAttachmentHTMLTable();

                dtFiles = drqmBLL.getDRQMFiles(0, null, intId, "DRQMResponseFile");

                for (int i = 0; i < dtFiles.Rows.Count; i++)
                {
                    uniqueRowId = uniqueRowId + 1;
                    drFiles = dtFiles.Rows[i];

                    sbAttachmentHTMLRows.Append(" <tr><td class='contentBody'> " +
                                                    " <input type='hidden' ID='uniqueRowId" + uniqueRowId + "' value='" + uniqueRowId + "' /> " +
                                                    " <input type='hidden' ID='attachId" + uniqueRowId + "' value='" + drFiles["CRDF_ID"].ToString() + "' /> " +
                                                    " <input type='checkbox' ID='checkAttachment" + uniqueRowId + "' values='0' /></td> " +
                                                    " <td class='contentBody'> " +
                                                    " <input type='hidden' ID='attachClientFileName" + uniqueRowId + "' value='" + drFiles["CRDF_CLIENT_FILE_NAME"].ToString() + "'/> " +
                                                    " <input type='hidden' ID='attachServerFileName" + uniqueRowId + "' value='" + drFiles["CRDF_SERVER_FILE_NAME"].ToString() + "'/> " +
                                                    " <input type='hidden' ID='attachFileTypeID" + uniqueRowId + "' value='" + drFiles["CRDF_FILE_TYPE"].ToString() + "'/> " +
                                                    " <input type='hidden' ID='attachFileDesc" + uniqueRowId + "' value='" + drFiles["CRDF_DESC"].ToString() + "'/> " + drFiles["CRDF_FILE_TYPE"].ToString() + " </td> " +
                                                    " <td class='contentBody'>" + drFiles["CRDF_DESC"].ToString().Replace(Environment.NewLine, "<br />") + "</td> " +
                                                    " <td class='contentBody'><a id='attachfilelink" + uniqueRowId + "' href='../CommonDownload.aspx?type=CRDRQ&downloadFileName=" + drFiles["CRDF_SERVER_FILE_NAME"].ToString() +
                                                    "&fileName=" + drFiles["CRDF_CLIENT_FILE_NAME"].ToString() + "'>" + drFiles["CRDF_CLIENT_FILE_NAME"].ToString() + "</a></td></tr> ");
                }

                litAttachment.Text += sbAttachmentHTMLRows.ToString() + "</table>";

                script += "</script>\r\n";
                ClientScript.RegisterStartupScript(this.GetType(), "return script", script);
                #endregion
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";

                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void gvEditResponse_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkEdit = (LinkButton)(e.Row.FindControl("lnkEdit"));
                HiddenField hfStatus = (HiddenField)(e.Row.FindControl("hfStatus"));

                if (hfStatus.Value.Equals("Sent"))
                {
                    lnkEdit.Visible = false;
                }
                else
                {
                    lnkEdit.Visible = true;
                }
            }
        }


        private void sendMailOnDRQMResponse(int intDRUId, int intDRId)
        {
            string strProcess = "";
            DataTable dt = new DataTable();

            try
            {
                dt = drqmBLL.getDRQMDetails(intDRId, 0, "", 0, null);
                DataRow dr = dt.Rows[0];

                MailContent_ComplianceReview mailContent = new MailContent_ComplianceReview();
                mailContent.ParamMap.Clear();
                mailContent.ParamMap.Add("ConfigId", "1095");

                if (hfSource.Value.Equals("DD"))
                {
                    strProcess = dr["DD_ENTITY_NAME"].ToString();
                }
                else if (hfSource.Value.Equals("RS"))
                {
                    strProcess = dr["RPFRAM_NAME"].ToString();
                }
                else if (hfSource.Value.Equals("CR"))
                {
                    strProcess = dr["CRUM_UNIVERSE_TO_BE_REVIEWED"].ToString();
                }

                if (hfUserType.Value.Equals("REQ"))
                {
                    mailContent.ParamMap.Add("To", "SPOC");
                    mailContent.ParamMap.Add("cc", "Requestor,UH");
                    mailContent.ParamMap.Add("RequestorId", Page.User.Identity.Name);
                    mailContent.ParamMap.Add("SPOCId", dr["CDQ_PERSON_RESPONSIBLE_ID"].ToString());
                    mailContent.ParamMap.Add("UpdateType", "REQ");
                }
                else if (hfUserType.Value.Equals("RES"))
                {
                    mailContent.ParamMap.Add("To", "Requestor");
                    mailContent.ParamMap.Add("cc", "SPOC");
                    mailContent.ParamMap.Add("RequestorId", dr["CDQ_REQUESTOR"].ToString());
                    mailContent.ParamMap.Add("SPOCId", Page.User.Identity.Name);
                    mailContent.ParamMap.Add("UpdateType", "RES");
                }
                mailContent.ParamMap.Add("UnitId", dr["CSFM_ID"].ToString());
                mailContent.ParamMap.Add("UnitName", dr["CSFM_NAME"].ToString());
                mailContent.ParamMap.Add("Source", dr["Module"].ToString());
                mailContent.ParamMap.Add("SourceIdentifier", dr["Identifier"].ToString());
                mailContent.ParamMap.Add("Ids", intDRUId);
                mailContent.ParamMap.Add("Process", strProcess);
                mailContent.ParamMap.Add("LoggedInUserName", (new Authentication()).GetUserDetsByEmpCode(Page.User.Identity.Name).Split('|')[0]);
                mailContent.setComplianceReviewMailContent();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }


        private void sendMailOnDRQMResponse(string strDRIds)
        {
            string strProcess = "", strRequestorId = "", strSource = "", strSourceIdentifier = "", strUnitId = "";
            DataTable dt = new DataTable();
            MailContent_ComplianceReview mailContent = new MailContent_ComplianceReview();

            try
            {
                dt = drqmBLL.getDRQMDetails(Convert.ToInt32(hfDRId.Value),0,null,0,null);
                DataView dv = new DataView(dt);
                DataTable dtDistinctRequestor = dv.ToTable(true, "CDQ_REQUESTOR");

                for (int i = 0; i < dtDistinctRequestor.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    strRequestorId = dr["CDQ_REQUESTOR"].ToString();

                    DataView dvData = new DataView(dt);
                    dvData.RowFilter = "CDQ_REQUESTOR = '" + strRequestorId + "'";

                    DataTable dtFilteredData = dvData.ToTable(true, "Identifier");

                    for (int j = 0; j < dtFilteredData.Rows.Count; j++)
                    {
                        strSourceIdentifier = dtFilteredData.Rows[j]["Identifier"].ToString();

                        DataView dvSrcData = new DataView(dt);
                        dvSrcData.RowFilter = "Identifier = '" + strSourceIdentifier + "'";

                        DataTable dtSrcFilteredData = dvSrcData.ToTable();

                        strSource = dtSrcFilteredData.Rows[0]["Module"].ToString();
                        strProcess = dtSrcFilteredData.Rows[0]["CRUM_UNIVERSE_TO_BE_REVIEWED"].ToString();
                        //strUnitId = dtSrcFilteredData.Rows[0]["DR_SFM_ID"].ToString();

                        mailContent.ParamMap.Clear();
                        mailContent.ParamMap.Add("ConfigId", "1099");
                        mailContent.ParamMap.Add("To", "Requestor");
                        mailContent.ParamMap.Add("cc", "SPOC,UH");
                        mailContent.ParamMap.Add("RequestorId", strRequestorId);
                        mailContent.ParamMap.Add("SPOCId", Page.User.Identity.Name);
                        mailContent.ParamMap.Add("Source", strSource);
                        mailContent.ParamMap.Add("SourceIdentifier", strSourceIdentifier);
                        mailContent.ParamMap.Add("Process", strProcess);
                        mailContent.ParamMap.Add("Type", "RM");
                        mailContent.ParamMap.Add("Ids", strDRIds);
                        mailContent.ParamMap.Add("UnitId", strUnitId);
                        mailContent.ParamMap.Add("LoggedInUserName", (new Authentication()).GetUserDetsByEmpCode(Page.User.Identity.Name).Split('|')[0]);
                        mailContent.setComplianceReviewMailContent();
                    }
                }

                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
    }
}