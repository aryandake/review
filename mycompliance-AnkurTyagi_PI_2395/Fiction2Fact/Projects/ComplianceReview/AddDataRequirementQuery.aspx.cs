using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using System.Text;
using Fiction2Fact.Legacy_App_Code.BLL;
using DocumentFormat.OpenXml.Bibliography;
using System.Data;
using System.Configuration;
using Fiction2Fact.Legacy_App_Code.Compliance;

namespace Fiction2Fact.Projects.Compliance
{
    public partial class AddDataRequirementQuery : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        RefCodesBLL refBLL = new RefCodesBLL();
        string script = "";
        int intCnt = 0, intCnt_new = 0;
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.User.Identity.Name.ToString().Equals(""))
            {
                Response.Redirect(Global.site_url("Login.aspx"));
            }
            if (!IsPostBack)
            {
                Session["strMidArray"] = "";

                DataTable dt = new DataTable();
                DataTable dtSource = new DataTable();
                string strUserDets = "";

                if (Request.QueryString["Id"] != null)
                    hfId.Value = Request.QueryString["Id"].ToString();
                else
                    hfId.Value = "0";

                if (Request.QueryString["UnitId"] != null)
                    hfUnitId.Value = Request.QueryString["UnitId"].ToString();

                if (Request.QueryString["RefId"] != null)
                    hfRefId.Value = Request.QueryString["RefId"].ToString();
                else
                    hfRefId.Value = "0";

                if (Request.QueryString["Source"] != null)
                    hfSource.Value = Request.QueryString["Source"].ToString();

                litAttachment.Text = getAttachmentHTMLTable() + "</table>";

                ddlUnit.DataSource = oBLL.Search_SubFunction_Master(0, null, strCSFM_Status: "A");
                ddlUnit.DataBind();
                ddlUnit.Items.Insert(0, new ListItem("(Select an option)", ""));
                ddlUnit.SelectedValue = hfUnitId.Value;

                FillQueryType();

                dt = oBLL.getDRQMDetails(0, Convert.ToInt32(hfRefId.Value), null, 0, null);

                #region ?
                //if (hfSource.Value.Equals("RS"))
                //{
                //    dtSource = rsBLL.getRiskSignOffDetails(Convert.ToInt32(hfRefId.Value), 0, 0, "", "", "", "", "", "", "");
                //    strUserDets = CommonCode.getUserDetailsbyPhoneBook(dtSource.Rows[0]["RS_SPOC_RESPONSIBLE_ID"].ToString());
                //    txtPersonResponsible.Text = strUserDets.Split(';')[0];
                //    hfResponsiblePersonId.Value = strUserDets.Split(';')[1];
                //}
                //else if (hfSource.Value.Equals("DD"))
                //{
                //    dtSource = ddBLL.searchDueDiligence(Convert.ToInt32(hfRefId.Value), "", "", "", "", "", "", "", "", "");
                //    strUserDets = CommonCode.getUserDetailsbyPhoneBook(dtSource.Rows[0]["DD_SPOC_RESPONSIBLE_ID"].ToString());
                //    if (!strUserDets.Equals(""))
                //    {
                //        txtPersonResponsible.Text = strUserDets.Split(';')[0];
                //        hfResponsiblePersonId.Value = strUserDets.Split(';')[1];
                //    }

                //} 
                #endregion

                Session["QueryTracker"] = dt;

                gvDRQM.DataSource = dt;
                gvDRQM.DataBind();
                if (intCnt_new > 0)
                {
                    //btnExportToExcel.Visible = true;
                    btnSubmitAllQueries.Visible = true;
                }
                else
                {
                    //btnExportToExcel.Visible = false;
                    btnSubmitAllQueries.Visible = false;
                }
            }

            script += "\r\n <script type=\"text/javascript\">\r\n";
            //script += " showHideResponsiblUnit();\r\n";
            script += "</script>\r\n";
            ClientScript.RegisterStartupScript(this.GetType(), "", script);
        }


        void FillQueryType()
        {
            ddlQueryType.Items.Clear();
            ddlQueryType.DataSource = refBLL.getRefCodeDetails("ComplianceReview - DRQ - Query Type", mstrConnectionString);
            ddlQueryType.DataBind();
            ddlQueryType.Items.Insert(0, new ListItem("--Select--", ""));
        }
        private string getAttachmentHTMLTable()
        {
            StringBuilder sbAttachmentHTML = new StringBuilder();

            sbAttachmentHTML.Append(" <table id='tblAttachment' width='100%' class='table table-bordered footable'> " +
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
                dr["Type"] = "DRQMFile";
                dtAttachment.Rows.Add(dr);
            }
            return dtAttachment;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtFiles = new DataTable();
                string strMsg = "", strLoggedInUser = "", strPersonReponsibleId = "", strPersonReponsibleName = "",
                    strPersonReponsibleEmail = "", strScript = "";
                int intId = 0, intRefId = 0, intUnitId = 0;
                bool res = false;
                res = int.TryParse(hfId.Value, out intId);
                res = int.TryParse(hfRefId.Value, out intRefId);
                res = int.TryParse(ddlUnit.SelectedValue, out intUnitId);

                dtFiles = getAttachmentdt();

                if (hfResPersonEdit.Value.Equals(""))
                {
                    if (!hfResponsiblePersonId.Value.Equals("") && !hfResponsiblePersonId.Value.Equals("||"))
                    {
                        strPersonReponsibleId = hfResponsiblePersonId.Value.Split('|')[0];
                        strPersonReponsibleName = hfResponsiblePersonId.Value.Split('|')[1];
                        strPersonReponsibleEmail = hfResponsiblePersonId.Value.Split('|')[2];
                    }
                    else
                    {
                        strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                                    "alert('Please enter correct details for person responsible.');\r\n" +
                                    "</script>\r\n";

                        ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);

                        hfDoubleClickFlag.Value = "";
                        return;
                    }
                }
                else
                {
                    if (hfFlag.Value.Equals("true"))
                    {
                        if (hfResponsiblePersonId.Value.Equals(""))
                        {
                            strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                                            "alert('Please enter correct details for person responsible.');\r\n" +
                                            "</script>\r\n";

                            ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);

                            hfDoubleClickFlag.Value = "";
                            return;
                        }
                        else
                        {
                            strPersonReponsibleId = hfResponsiblePersonId.Value.Split('|')[0];
                            strPersonReponsibleName = hfResponsiblePersonId.Value.Split('|')[1];
                            strPersonReponsibleEmail = hfResponsiblePersonId.Value.Split('|')[2];
                        }
                    }
                    else
                    {
                        strPersonReponsibleId = hfResPersonEdit.Value.Split('|')[0];
                        strPersonReponsibleName = hfResPersonEdit.Value.Split('|')[1];
                        strPersonReponsibleEmail = hfResPersonEdit.Value.Split('|')[2];
                    }
                }

                strLoggedInUser = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                string stype = ddlQueryType.SelectedItem.Value;

                intId = oBLL.saveDRQM(intId, hfSource.Value, intRefId, intUnitId, txtQuery.Text, txtPersonResponsible.Text,
                    strPersonReponsibleId, strPersonReponsibleName, strPersonReponsibleEmail, strLoggedInUser, Page.User.Identity.Name, stype, dtFiles);

                dt = oBLL.getDRQMDetails(0, Convert.ToInt32(hfRefId.Value), null, 0, null);

                gvDRQM.DataSource = dt;
                gvDRQM.DataBind();
                if (intCnt_new > 0)
                {
                    //btnExportToExcel.Visible = true;
                    btnSubmitAllQueries.Visible = true;
                }
                else
                {
                    //btnExportToExcel.Visible = false;
                    btnSubmitAllQueries.Visible = false;
                }

                if (!hfId.Value.Equals("") && !hfId.Value.Equals("0"))
                {
                    strMsg = "Query updated successfully.";

                    DataTable dt1 = oBLL.getDRQMDetails(intId, Convert.ToInt32(hfRefId.Value), null, 0, null);

                    if (dt1.Rows[0]["CDQ_IS_MAIL_SENT"].ToString().Equals("Y"))
                    {
                        sendMailOnDRQMUpdation("1092", intId, intRefId, intUnitId, ddlUnit.SelectedItem.Text);
                    }
                }
                else
                {
                    strMsg = "Query added successfully.";
                }

                strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                            "alert('" + strMsg + "');\r\n" +
                            "document.getElementById('btnRefresh').click();\r\n" +
                            "</script>\r\n";

                ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);

                clearField();

                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }


        private void sendMailOnDRQMUpdation(string strConfigId, int intDRId, int intSourceId, int intUnitId, string strUnitName)
        {
            DataTable dt = new DataTable();
            DataTable dtDD = new DataTable();
            string strProcess = "";
            try
            {
                dt = oBLL.getDRQMDetails(intDRId, intSourceId, null, 0, null);
                DataRow dr = dt.Rows[0];

                MailContent_ComplianceReview mailContent = new MailContent_ComplianceReview();

                mailContent.ParamMap.Clear();
                mailContent.ParamMap.Add("ConfigId", strConfigId);

                if (hfSource.Value.Equals("DD"))
                {
                    strProcess = dr["DD_ENTITY_NAME"].ToString();
                }
                else if (hfSource.Value.Equals("RS"))
                {
                    strProcess = dr["RPFRAM_NAME"].ToString();
                }
                else if (hfSource.Value.Equals("RR"))
                {
                    strProcess = dr["CRUM_UNIVERSE_TO_BE_REVIEWED"].ToString();
                }

                if (strConfigId.Equals("2327")) // Creation
                {
                    mailContent.ParamMap.Add("To", "SPOC");
                    mailContent.ParamMap.Add("cc", "Requestor");
                    mailContent.ParamMap.Add("RequestorId", Page.User.Identity.Name);
                }
                else if (strConfigId.Equals("1092")) // Updation
                {
                    mailContent.ParamMap.Add("To", "SPOC");
                    mailContent.ParamMap.Add("cc", "Requestor,Creator,UH");
                    mailContent.ParamMap.Add("CreatorId", Page.User.Identity.Name);
                    mailContent.ParamMap.Add("RequestorId", dr["CDQ_REQUESTOR"].ToString());
                }
                mailContent.ParamMap.Add("SPOCId", dr["CDQ_PERSON_RESPONSIBLE_ID"].ToString());
                mailContent.ParamMap.Add("UnitId", intUnitId);
                mailContent.ParamMap.Add("UnitName", strUnitName);
                mailContent.ParamMap.Add("SourceIds", intSourceId);
                mailContent.ParamMap.Add("Source", dr["Module"].ToString());
                mailContent.ParamMap.Add("SourceIdentifier", dr["Identifier"].ToString());
                mailContent.ParamMap.Add("Ids", intDRId);
                mailContent.ParamMap.Add("Type", "Update");
                mailContent.ParamMap.Add("LoggedInUserName", (new Authentication()).GetUserDetsByEmpCode(Page.User.Identity.Name).Split('|')[0]);
                mailContent.ParamMap.Add("Process", strProcess);
                mailContent.setComplianceReviewMailContent();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void sendMailOnDRQMCreation(string strConfigId, int intSourceId, string strDRID)
        {
            CommonCode cc = new CommonCode();
            DataTable dt = new DataTable();
            MailContent_ComplianceReview mailContent = new MailContent_ComplianceReview();
            string strSPOCId = "", strSource = "", strSourceIdentifier = "", strProcess = "", strUnitIds = "";
            try
            {
                dt = oBLL.getDRQMDetails(0, Convert.ToInt32(intSourceId), null, 0, null, strValue: " and CDQ_IS_MAIL_SENT='N'");
                DataView dv = new DataView(dt);
                dv.RowFilter = "CDQ_ID in(" + strDRID + ")";
                DataTable dtDistinctSPOC = dv.ToTable(true, "CDQ_PERSON_RESPONSIBLE_ID");

                for (int i = 0; i < dtDistinctSPOC.Rows.Count; i++)
                {
                    string strDRIds = "";
                    DataRow dr1 = dtDistinctSPOC.Rows[i];
                    strSPOCId = dr1["CDQ_PERSON_RESPONSIBLE_ID"].ToString();
                    DataView dvData = new DataView(dt);
                    dvData.RowFilter = "CDQ_PERSON_RESPONSIBLE_ID = '" + strSPOCId + "' And CDQ_ID in(" + strDRID + ")";
                    DataTable dtFilteredData = dvData.ToTable();

                    for (int j = 0; j < dtFilteredData.Rows.Count; j++)
                    {
                        strDRIds = (string.IsNullOrEmpty(strDRIds) ? "" : strDRIds + ",") + dtFilteredData.Rows[j]["CDQ_ID"].ToString();
                        strUnitIds = (string.IsNullOrEmpty(strUnitIds) ? "" : strUnitIds + ",") + dtFilteredData.Rows[j]["CDQ_SFM_ID"].ToString();
                        strSource = dtFilteredData.Rows[j]["Module"].ToString();
                        strSourceIdentifier = dtFilteredData.Rows[j]["Identifier"].ToString();
                        if (hfSource.Value.Equals("DD"))
                        {
                            strProcess = dtFilteredData.Rows[j]["DD_ENTITY_NAME"].ToString();
                        }
                        else if (hfSource.Value.Equals("RS"))
                        {
                            strProcess = dtFilteredData.Rows[j]["RPFRAM_NAME"].ToString();
                        }
                        else if (hfSource.Value.Equals("CR"))
                        {
                            strProcess = dtFilteredData.Rows[j]["CRUM_UNIVERSE_TO_BE_REVIEWED"].ToString();
                        }
                    }
                    mailContent.ParamMap.Clear();
                    mailContent.ParamMap.Add("ConfigId", strConfigId);
                    mailContent.ParamMap.Add("To", "SPOC");
                    mailContent.ParamMap.Add("cc", "Requestor,UH");
                    mailContent.ParamMap.Add("RequestorId", Page.User.Identity.Name);
                    mailContent.ParamMap.Add("SPOCId", strSPOCId);
                    mailContent.ParamMap.Add("SourceIds", intSourceId);
                    mailContent.ParamMap.Add("Source", strSource);
                    mailContent.ParamMap.Add("SourceIdentifier", strSourceIdentifier);
                    mailContent.ParamMap.Add("Type", "Submit");
                    mailContent.ParamMap.Add("LoggedInUserName", (new Authentication()).GetUserDetsByEmpCode(Page.User.Identity.Name).Split('|')[0]);
                    mailContent.ParamMap.Add("Process", strProcess);
                    mailContent.ParamMap.Add("DRIds", strDRIds);
                    mailContent.ParamMap.Add("UnitId", strUnitIds);
                    mailContent.setComplianceReviewMailContent();
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }



        private void sendMailOnDRQMClosure(int intDRId, int intSourceId)
        {
            DataTable dt = new DataTable();
            string strProcess = "";
            try
            {
                dt = oBLL.getDRQMDetails(intDRId, 0, null, 0, null);
                DataRow dr = dt.Rows[0];

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
                MailContent_ComplianceReview mailContent = new MailContent_ComplianceReview();
                mailContent.ParamMap.Clear();
                mailContent.ParamMap.Add("ConfigId", "1094");
                mailContent.ParamMap.Add("To", "SPOC");
                mailContent.ParamMap.Add("cc", "Requestor,UH");
                mailContent.ParamMap.Add("RequestorId", Page.User.Identity.Name);
                mailContent.ParamMap.Add("SPOCId", dr["CDQ_PERSON_RESPONSIBLE_ID"].ToString());
                mailContent.ParamMap.Add("UnitId", dr["CSFM_ID"].ToString());
                mailContent.ParamMap.Add("UnitName", dr["CSFM_NAME"].ToString());
                mailContent.ParamMap.Add("SourceIds", intSourceId);
                mailContent.ParamMap.Add("Source", dr["Module"].ToString());
                mailContent.ParamMap.Add("SourceIdentifier", dr["Identifier"].ToString());
                mailContent.ParamMap.Add("Ids", intDRId);
                mailContent.ParamMap.Add("Type", "Closure");
                mailContent.ParamMap.Add("LoggedInUserName", (new Authentication()).GetUserDetsByEmpCode(Page.User.Identity.Name).Split('|')[0]);
                mailContent.ParamMap.Add("Process", strProcess);
                mailContent.setComplianceReviewMailContent();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        private void clearField()
        {
            hfId.Value = "";
            ddlUnit.SelectedIndex = -1;
            txtQuery.Text = "";
            txtPersonResponsible.Text = "";
            hfAttachment.Value = "";
            hfResponsiblePersonId.Value = "";
            hfDoubleClickFlag.Value = "";
        }

        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }

        protected DataTable LoadDRQMFileList(object Id)
        {
            return oBLL.getDRQMFiles(0, null, Convert.ToInt32(Id), null);
        }

        protected void gvDRQM_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dtQueryTracker = (DataTable)(Session["QueryTracker"]);
            string strMidArray = "", strRowComments = "";
            int intCntRec = dtQueryTracker.Rows.Count;
            if ((e.Row.RowType == DataControlRowType.Header))
            {
                CheckBox HeaderLevelCheckBox = (CheckBox)(e.Row.FindControl("HeaderLevelCheckBox"));
                HeaderLevelCheckBox.Attributes["onClick"] = "ChangeAllCheckBoxStates(this.checked);";
                strMidArray = HeaderLevelCheckBox.ClientID;
                Session["strMidArray"] = strMidArray;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField lblStatus = (HiddenField)e.Row.FindControl("hfStatus");
                LinkButton lnkAddClosure = (LinkButton)e.Row.FindControl("lnkAddClosure");
                LinkButton lnkAddRes = (LinkButton)e.Row.FindControl("lnkAddRes");
                LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
                CheckBox RowLevelCheckBox = (CheckBox)(e.Row.FindControl("RowLevelCheckBox"));
                HiddenField hfIsMailSent = (HiddenField)(e.Row.FindControl("hfIsMailSent"));
                HiddenField hfQueryPendingWith = (HiddenField)(e.Row.FindControl("hfQueryPendingWith"));

                RowLevelCheckBox.Attributes["onClick"] = "onRowCheckedUnchecked('" + RowLevelCheckBox.ClientID + "');";

                RowLevelCheckBox.Visible = false;

                if (lblStatus.Value.Equals("O"))
                {
                    lnkAddClosure.Visible = true;
                    lnkAddRes.Visible = true;
                    lnkEdit.Visible = false;
                }
                else if (lblStatus.Value.Equals("C"))
                {
                    lnkAddClosure.Visible = false;
                    lnkAddRes.Visible = false;
                    lnkEdit.Visible = false;
                }
                else
                {
                    lnkAddClosure.Visible = false;
                    lnkAddRes.Visible = false;
                    lnkEdit.Visible = true;
                    RowLevelCheckBox.Visible = true;
                    intCnt_new = intCnt_new + 1;
                }

                //if (hfIsMailSent.Value.Equals("Y"))
                //{
                //    RowLevelCheckBox.Visible = false;
                //}
                //else
                //{
                //    RowLevelCheckBox.Visible = true;
                //}
                strMidArray = RowLevelCheckBox.ClientID;
                strMidArray = "','" + strMidArray;

                Session["strMidArray"] = Session["strMidArray"].ToString() + strMidArray;
            }
            intCnt = intCnt + 1;
            if (intCnt == intCntRec + 1)
                CheckBoxIDsArray.Text = "<script type='text/javascript'>" +
                                        "var CheckBoxIDs =  new Array('" + Session["strMidArray"].ToString() + "');" +
                                        "</script>";
        }

        protected void gvDRQM_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dtFiles = new DataTable();
            DataRow dr, drFiles;
            bool res = false;
            int intId = 0, intRefId = 0, uniqueRowId = 0;
            string strPersonReponsibleId = "", strPersonReponsibleName = "", strPersonReponsibleEmail = "";
            StringBuilder sbAttachmentHTMLRows = new StringBuilder();
            try
            {
                res = int.TryParse(hfRefId.Value, out intRefId);
                res = int.TryParse(gvDRQM.SelectedValue.ToString(), out intId);

                if (hfSelectedOperation.Value == "Edit")
                {
                    dt = oBLL.getDRQMDetails(intId, intRefId, "", 0, null);
                    dr = dt.Rows[0];

                    ddlQueryType.SelectedValue = (dr["CDQ_TYPE"] is DBNull ? "0" : dr["CDQ_TYPE"].ToString());
                    hfId.Value = (dr["CDQ_ID"] is DBNull ? "0" : dr["CDQ_ID"].ToString());
                    ddlUnit.SelectedValue = ((dr["CDQ_SFM_ID"] is DBNull || dr["CDQ_SFM_ID"].Equals(0)) ? "" : dr["CDQ_SFM_ID"].ToString());
                    txtQuery.Text = (dr["CDQ_QUERY_DATA_REQUIREMENT"] is DBNull ? "" : dr["CDQ_QUERY_DATA_REQUIREMENT"].ToString());
                    txtPersonResponsible.Text = (dr["CDQ_PERSON_RESPONSIBLE"] is DBNull ? "" : dr["CDQ_PERSON_RESPONSIBLE"].ToString());
                    strPersonReponsibleId = (dr["CDQ_PERSON_RESPONSIBLE_ID"] is DBNull ? "" : dr["CDQ_PERSON_RESPONSIBLE_ID"].ToString());
                    strPersonReponsibleName = (dr["CDQ_PERSON_REPOSNSIBLE_NAME"] is DBNull ? "" : dr["CDQ_PERSON_REPOSNSIBLE_NAME"].ToString());
                    strPersonReponsibleEmail = (dr["CDQ_PERSON_REPOSNSIBLE_EMAIL"] is DBNull ? "" : dr["CDQ_PERSON_REPOSNSIBLE_EMAIL"].ToString());
                    hfResPersonEdit.Value = strPersonReponsibleId + "|" + strPersonReponsibleName + "|" + strPersonReponsibleEmail;

                    #region Files
                    script += "\r\n <script type=\"text/javascript\">\r\n";
                    litAttachment.Text = getAttachmentHTMLTable();

                    dtFiles = oBLL.getDRQMFiles(0, "", intId, null);

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
                                                    " <input type='hidden' ID='attachFileDesc" + uniqueRowId + "' value='" + drFiles["CRDF_DESC"].ToString() + "'/> " + drFiles["RC_NAME"].ToString() + " </td> " +
                                                    " <td class='contentBody'>" + drFiles["CRDF_DESC"].ToString().Replace(Environment.NewLine, "<br />") + "</td> " +
                                                    " <td class='contentBody'><a id='attachfilelink" + uniqueRowId + "' href='../CommonDownload.aspx?type=CRDRQ&downloadFileName=" + drFiles["CRDF_SERVER_FILE_NAME"].ToString() +
                                                    "&fileName=" + drFiles["CRDF_CLIENT_FILE_NAME"].ToString() + "'>" + drFiles["CRDF_CLIENT_FILE_NAME"].ToString() + "</a></td></tr> ");
                    }

                    litAttachment.Text += sbAttachmentHTMLRows.ToString() + "</table>";

                    script += "</script>\r\n";
                    ClientScript.RegisterStartupScript(this.GetType(), "return script", script);
                    #endregion
                }

            }
            catch
            {
            }
        }


        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddDataRequirementQuery.aspx?RefId=" + hfRefId.Value + "&Source=" + hfSource.Value + "&UnitId=" + hfUnitId.Value);
        }
        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            string strCreateBy = "", strClosure = "";
            int intId = 0, intSourceId = 0;
            bool res = false;
            hfClickCounter.Value = "1";

            try
            {
                res = int.TryParse(hfId.Value, out intId);
                res = int.TryParse(hfRefId.Value, out intSourceId);

                strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                strClosure = hfModalClosure.Value;
                oBLL.saveDRQMClousreDetails(intId, hfSource.Value, intSourceId, strClosure, strCreateBy);

                sendMailOnDRQMClosure(intId, intSourceId);

                ClientScript.RegisterStartupScript(this.GetType(), "pop", "alert('Query have been closed successfully.'); document.getElementById('btnRefresh').click();", true);
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnSubmitAllQueries_Click(object sender, EventArgs e)
        {
            int intRefId = 0;
            string strIsValidated = "false", strScript = "";
            bool res = false;
            res = int.TryParse(hfRefId.Value, out intRefId);
            int intDRID = 0;
            string strDRID = "", strDRID1 = "";
            try
            {
                foreach (GridViewRow gvr in gvDRQM.Rows)
                {
                    HiddenField hfStatus = (HiddenField)gvr.FindControl("hfStatus");
                    HiddenField hfDRID = (HiddenField)(gvr.FindControl("hfDRID"));
                    CheckBox RowLevelCheckBox = (CheckBox)gvr.FindControl("RowLevelCheckBox");
                    bool val = int.TryParse(hfDRID.Value.ToString(), out intDRID);
                    if (RowLevelCheckBox.Checked)
                    {
                        if (hfStatus.Value.Equals("D"))
                        {
                            strIsValidated = "true";
                            //break;
                        }
                        if (strIsValidated.Equals("true"))
                        {
                            oBLL.submitForOperation(intDRID, "CR", "SubmitAllQueries", null, intRiskReviewDraftId: intRefId);
                            strDRID += intDRID.ToString() + ",";
                        }
                    }

                }

                if (strIsValidated.Equals("true"))
                {
                    if (!strDRID.Equals(""))
                        strDRID1 = strDRID.Substring(0, strDRID.Length - 1);
                    sendMailOnDRQMCreation("1093", intRefId, strDRID1);
                    strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                                "alert('Selected Queries have been submitted successfully.');\r\n" +
                                "document.getElementById('btnRefresh').click();\r\n" +
                                "</script>\r\n";
                }
                else
                {
                    strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                                "alert('All queries are submitted, please add new queries to submit.');\r\n" +
                                "</script>\r\n";

                }

                ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);
                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

        }
    }
}