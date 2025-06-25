using System;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.Circulars.BLL;
using System.Linq;
using iText.Kernel.Pdf.Tagutils;
using Microsoft.VisualBasic.ApplicationServices;
using System.Web.Http.Results;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class CommonSubmission : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
        UtilitiesBLL utilityBL = new UtilitiesBLL();
        private DataTable mdtEditFileUpload;
        RefCodesBLL rcBL = new RefCodesBLL();
        CircUtilitiesBLL cBLL = new CircUtilitiesBLL();
        CommonMethods cm = new CommonMethods();
        string strTableCellCSS = " style=\"font-size: 12px; border-width: " +
                        "1px; padding: 8px; border-style: solid;" +
                        "border-color: #bcaf91;\"";

        string strTableHeaderCSS = " style =\"font-size: 12px; " +
                                "background-color: #ded0b0; border-width: 1px; padding: 8px;" +
                                "border-style: solid; border-color: #bcaf91; text-align: left;\"";

        string strTableCSS = " style=\"font-size: 12px; color: #333333; " +
                                "width: 100%; border-width: 1px; border-color: #bcaf91; border-collapse: collapse;\"";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hfCurDate.Value = System.DateTime.Now.ToString("dd-MMM-yyyy");
                revFileUpload1.ValidationExpression = ConfigurationManager.AppSettings["FileUploadRegex"];

                if (Request.QueryString["Type"] != null)
                {
                    hfType.Value = Request.QueryString["Type"].ToString();
                }

                if (Request.QueryString["CircId"] != null)
                {
                    hfCircId.Value = Request.QueryString["CircId"].ToString();
                }

                bool flag = false;

                if (hfType.Value.Equals("RR") || (hfType.Value.Equals("CIRC") && (!hfCircId.Value.Equals("") && !hfCircId.Value.Equals("0"))))
                {
                    flag = true;
                }

                if (!flag)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Script", "alert('Invalid Type'); window.location.href = '" + Global.site_url() + "';", true);
                }

                Session["SubmissionMasFiles"] = null;
                //cbCompany.Items.Clear();
                //cbCompany.DataSource = utilityBL.getDataset("COMPANY", mstrConnectionString);
                //cbCompany.DataBind();

                cblSegment.Items.Clear();
                DataSet dtSegments = utilityBL.getDataset("SUBMISSIONSGMT", mstrConnectionString);
                cblSegment.DataSource = dtSegments;
                cblvSegment.MinimumNumberOfSelectedCheckBoxes = dtSegments.Tables[0].Rows.Count;
                cblSegment.DataBind();


                //<<Modified by Ashish Mishra on 10Jul2017
                if (User.IsInRole("FilingAdmin") || (hfType.Value.Equals("CIRC") && (!hfCircId.Value.Equals("") && !hfCircId.Value.Equals("0"))))
                {
                    ddlSubType.Items.Clear();
                    ddlSubType.DataSource = utilityBL.getDataset("SUBTYPE", mstrConnectionString);
                    ddlSubType.DataBind();
                    ddlSubType.Items.Insert(0, new ListItem("(Select)", ""));
                    if (ddlSubType.Items.Count == 2)
                    {
                        ddlSubType.SelectedIndex = 1;
                    }
                }
                else if (User.IsInRole("Filing_Sub_Admin"))
                {
                    ddlSubType.Items.Clear();
                    ddlSubType.DataSource = utilityBL.getDatasetWithConditionInString("SUBTYPE", Page.User.Identity.Name.ToString(), mstrConnectionString);
                    ddlSubType.DataBind();
                    ddlSubType.Items.Insert(0, new ListItem("(Select)", ""));
                    if (ddlSubType.Items.Count == 2)
                    {
                        ddlSubType.SelectedIndex = 1;
                    }
                }
                //>>
                //ddlSubType.DataSource = utilityBL.getDataset("SUBTYPE", mstrConnectionString);
                //ddlSubType.DataBind();

                cbReportingDept.DataSource = utilityBL.getDataset("REPORTINGDEPT", mstrConnectionString);
                cbReportingDept.DataBind();

                //ddlReportDept.DataSource = utilityBL.getDataset("REPORTINGDEPT", mstrConnectionString);
                //ddlReportDept.DataBind();ddlEvent.Attributes["onchange"] = "showHideErrorIdentified();";

                ddlEvent.DataSource = utilityBL.getDataset("EVENT", mstrConnectionString);
                ddlEvent.DataBind();
                rblFrequency.Attributes["onClick"] = "showhideOtherFrequencyPanels('" + rblFrequency.ClientID + "')";
                rblType.Attributes["onClick"] = "showhideTypeBased('" + rblType.ClientID + "')";

                rblEscalate.Attributes["onClick"] = "showhideEscalationDaysSection()";

                ddlFileType.DataSource = rcBL.getRefCodeDetails("Submisssion File Type", mstrConnectionString);
                ddlFileType.DataBind();
                ddlFileType.Items.Insert(0, new ListItem("(Select an option)", ""));

                if (ddlPriority.Items.Count >= 2)
                {
                    ddlPriority.SelectedIndex = 1;
                }

                CommonCodes.SetDropDownDataSource(ddlLOB, cBLL.GetDataTable("getLOBList", new DBUtilityParameter("LEM_STATUS", "A"), sOrderBy: "LEM_NAME"));
                if (ddlLOB.Items.Count <= 2)
                {
                    ddlLOB.SelectedIndex = 1;
                }

                ddlReportDept.DataSource = utilityBL.getDataset("REPORTINGDEPT", mstrConnectionString);
                ddlReportDept.DataBind();

                if (Request.QueryString["SMId"] != null)
                {
                    hfSMId.Value = Request.QueryString["SMId"].ToString();
                    BindValues(hfSMId.Value);
                    divReportingDeptdd.Visible = true;
                    divAlreadyUploaded.Visible = true;
                }
                else
                {
                    divReportingDeptcb.Visible = true;
                    hfSMId.Value = "0";
                }
            }
            else
            {
                if (!(Session["SubmissionMasFiles"] == null))
                {
                    mdtEditFileUpload = (DataTable)Session["SubmissionMasFiles"];
                }

            }

            string strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                            "\r\nshowhideTypeBased('" + rblType.ClientID + "')" +
                            "\r\nshowhideOtherFrequencyPanels('" + rblFrequency.ClientID + "')\r\n" +
                            "\r\nshowhideEscalationDaysSection()\r\n" +
                            "</script>\r\n";

            ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);

        }

        void BindValues(string strSMId)
        {
            string strFrequency = "", strType = "";

            DataSet dsEditSubmissions = SubmissionMasterBLL.SearchSubmissions
                                        (Convert.ToInt32(strSMId), null, null, null, null,
                                        null, null, null, null, "", "", mstrConnectionString);

            DataRow dr = dsEditSubmissions.Tables[0].Rows[0];

            ddlSubType.SelectedValue = dr["SM_STM_ID"].ToString();

            if (dr["SM_CIRCULAR_DATE"] != DBNull.Value)
            {
                txtCircularDate.Text = Convert.ToDateTime(dr["SM_CIRCULAR_DATE"]).ToString("dd-MMM-yyyy");
            }

            if (dr["SM_EFFECTIVE_DT"] != DBNull.Value)
            {
                txtEffectiveDate.Text = Convert.ToDateTime(dr["SM_EFFECTIVE_DT"]).ToString("dd-MMM-yyyy");
            }

            ddlReportDept.SelectedValue = dr["SM_SRD_ID"].ToString();
            bindSegments(cblSegment, strSMId);

            txtReference.Text = dr["SM_ACT_REG_SECTION"].ToString();
            txtSection.Text = dr["SM_SECTION_CLAUSE"].ToString();
            txtParticulars.Text = dr["SM_PERTICULARS"].ToString();
            txtDescription.Text = dr["SM_BRIEF_DESCRIPTION"].ToString();
            ddlPriority.SelectedValue = dr["SM_PRIORITY"].ToString();

            rblEscalate.SelectedValue = dr["SM_TO_BE_ESC"].ToString();
            txtlevel0.Text = dr["SM_L0_ESCALATION_DAYS"].ToString();
            txtlevel1.Text = dr["SM_L1_ESCALATION_DAYS"].ToString();
            txtlevel2.Text = dr["SM_L2_ESCALATION_DAYS"].ToString();

            ddlFSAppReq.SelectedValue = dr["SM_IS_FS_APPROVAL_REQUIRED"].ToString();

            strType = dr["SM_SUB_TYPE"].ToString();
            rblType.SelectedValue = strType;

            if (strType == "E")
            {
                DataSet dsEVENT_EDIT = utilityBL.getDataset("EVENT_EDIT", mstrConnectionString);
                CommonCodes.SetDropDownDataSourceForEdit(ddlEvent, dsEVENT_EDIT.Tables[0], "EM_STATUS");
                rblAssociatedWith.SelectedValue = dr["SM_EP_ID"].ToString();
                txtStartDays.Text = dr["SM_START_NO_OF_DAYS"].ToString();
                txtEndDays.Text = dr["SM_END_NO_OF_DAYS"].ToString();
            }
            else if (strType == "F")
            {
                strFrequency = dr["SM_FREQUENCY"].ToString();
                rblFrequency.SelectedValue = dr["SM_FREQUENCY"].ToString();

                if (strFrequency == "Only Once")
                {
                    txtOnceFromDate.Text = dr["SM_ONLY_ONCE_FROM_DATE"].ToString();
                    txtOnceToDate.Text = dr["SM_ONLY_ONCE_TO_DATE"].ToString();
                }
                else if (strFrequency == "Weekly")
                {
                    ddlFromWeekDays.SelectedValue = dr["SM_WEEKLY_DUE_DATE_FROM"].ToString();
                    ddlToWeekDays.SelectedValue = dr["SM_WEEKLY_DUE_DATE_TO"].ToString();
                }
                else if (strFrequency == "Fortnightly")
                {
                    txtFortnightly1FromDate.Text = dr["SM_FIRST_FORTNIGHTLY_DUE_FROM_DATE"].ToString();
                    txtFortnightly1ToDate.Text = dr["SM_FIRST_FORTNIGHTLY_DUE_TO_DATE"].ToString();
                    txtFortnightly2FromDate.Text = dr["SM_SECOND_FORTNIGHTLY_DUE_FROM_DATE"].ToString();
                    txtFortnightly2ToDate.Text = dr["SM_SECOND_FORTNIGHTLY_DUE_TO_DATE"].ToString();
                }
                else if (strFrequency == "Quarterly")
                {
                    txtQ1fromDate.Text = dr["SM_Q1_DUE_DATE_FROM"].ToString();
                    txtQ1ToDate.Text = dr["SM_Q1_DUE_DATE_TO"].ToString();
                    txtQ2FromDate.Text = dr["SM_Q2_DUE_DATE_FROM"].ToString();
                    txtQ2ToDate.Text = dr["SM_Q2_DUE_DATE_TO"].ToString();
                    txtQ3FromDate.Text = dr["SM_Q3_DUE_DATE_FROM"].ToString();
                    txtQ3Todate.Text = dr["SM_Q3_DUE_DATE_TO"].ToString();
                    txtQ4FromDate.Text = dr["SM_Q4_DUE_DATE_FROM"].ToString();
                    txtQ4Todate.Text = dr["SM_Q4_DUE_DATE_TO"].ToString();
                }
                else if (strFrequency == "Monthly")
                {
                    txtMonthlyFromDate.Text = dr["SM_MONTHLY_DUE_DATE_FROM"].ToString();
                    txtMonthlyTodate.Text = dr["SM_MONTHLY_DUE_DATE_TO"].ToString();
                }
                else if (strFrequency == "Half Yearly")
                {
                    txtFirstHalffromDate.Text = dr["SM_FIRST_HALF_YR_DUE_DATE_FROM"].ToString();
                    txtFirstHalfToDate.Text = dr["SM_FIRST_HALF_YR_DUE_DATE_TO"].ToString();
                    txtSecondtHalffromDate.Text = dr["SM_SECOND_HALF_YR_DUE_DATE_FROM"].ToString();
                    txtSecondtHalffromTo.Text = dr["SM_SECOND_HALF_YR_DUE_DATE_TO"].ToString();
                }
                else if (strFrequency == "Yearly")
                {
                    txtYearlyfromDate.Text = dr["SM_YEARLY_DUE_DATE_FROM"].ToString();
                    txtYearlyDateTo.Text = dr["SM_YEARLY_DUE_DATE_TO"].ToString();
                }

            }

            getSubmissionDocumentsById(Convert.ToInt32(strSMId));
        }

        private void getSubmissionDocumentsById(int intID)
        {
            try
            {
                DataTable dtAttachment = utilityBL.getDatasetWithCondition("getSubmisssionMasFiles_Edit", intID, mstrConnectionString);
                gvAlreadyUploaded.DataSource = dtAttachment;
                gvAlreadyUploaded.DataBind();
            }
            catch (Exception exp)
            {
                writeError("Exception in getSubmissionDocumentsById :" + exp.Message);
            }
        }

        private void bindSegments(CheckBoxList cblSegment, string strSMId)
        {
            DataTable dtSegmentName;
            string strName = null;

            dtSegmentName = utilityBL.getDatasetWithCondition("BINDSUBSEGMENTS", Convert.ToInt32(strSMId), mstrConnectionString);
            for (int i = 0; i <= dtSegmentName.Rows.Count - 1; i++)
            {
                strName = dtSegmentName.Rows[i]["SSM_CSGM_ID"].ToString();
                cblSegment.Items.FindByValue(strName).Selected = true;
            }
        }

        protected void AddMoreSub_Click(object sender, System.EventArgs e)
        {
            Response.Redirect(Request.RawUrl, false);
            //lblMsg.Text = "";
            //btnAddMore.Visible = false;
            //pnlSubmissions.Visible = true;
            //clearControls();

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this))
            {
                hfDoubleClickFlag.Value = "";
                return;
            }

            InsertSubmissionMaster();
            hfDoubleClickFlag.Value = "";
        }

        protected void btnAddAttachment_Click(object sender, System.EventArgs e)
        {
            lblMsg.Text = "";
            Authentication au = new Authentication();
            CommonCode cc = new CommonCode();
            DataRow dr;
            FileInfo fileInfo;
            string strCompleteFileName, strSelectedFile, strMsg = "";
            if ((fuEditFileUpload.HasFile))
            {
                strSelectedFile = fuEditFileUpload.FileName;

                //<<cahanged By Rahuldeb on 15Jun2017
                //strMsg = cc.getFileNameErrors(strSelectedFile);
                //if (!strMsg.Equals(""))
                //{
                //    writeError(strMsg);
                //    return;
                //}
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
                          strSelectedFile.Contains("\""))
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

                //string strReturnMsg = UploadedFileContentCheck.checkValidFileForUpload(fuEditFileUpload, "");
                //CommonCode ccm = new CommonCode();
                //strReturnMsg += ccm.getFileNameErrors(strSelectedFile);
                //if (!strReturnMsg.Equals(""))
                //{
                //    writeError(strReturnMsg);
                //    return;
                //}

                if (UploadedFileContentCheck.checkForMultipleExtention(strSelectedFile))
                {
                    writeError("The file uploaded is multiple extensions.");
                    return;
                }
                //>>
                //if (strSelectedFile.Contains("&"))
                //{
                //    writeError("File Name can't have special character '&'.");
                //    return;
                //}
                //>>
                try
                {
                    string strFileNameOnClient;
                    string strFileNameOnServer;
                    string strServerDirectory;
                    DateTime dtUploadDatetime;
                    strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["SubmissionFiles"].ToString());
                    strFileNameOnClient = fuEditFileUpload.FileName;
                    dtUploadDatetime = System.DateTime.Now;
                    strFileNameOnServer = Authentication.GetUserID(Page.User.Identity.Name) + "_" + dtUploadDatetime.ToString("ddMMyyyyHHmmss") + "_" + fuEditFileUpload.FileName;
                    hfFileNameOnServer.Value = strFileNameOnServer;
                    fuEditFileUpload.SaveAs(strServerDirectory + "\\\\" + strFileNameOnServer);
                    strCompleteFileName = strServerDirectory + "\\" + strFileNameOnServer;
                    fileInfo = new FileInfo(strCompleteFileName);

                    if ((mdtEditFileUpload == null))
                    {
                        initFileUpload();
                    }
                    dr = mdtEditFileUpload.NewRow();

                    dr["File Type"] = ddlFileType.SelectedItem.Text.ToString();
                    dr["File Description"] = cm.getSanitizedString(txtFileDesc.Text.ToString());
                    dr["FileTypeShortForm"] = ddlFileType.SelectedValue.ToString();
                    dr["FileName"] = strFileNameOnClient;
                    dr["FileNameOnServer"] = strFileNameOnServer;
                    dr["Uploaded By"] = au.getUserFullName(Page.User.Identity.Name.ToString());
                    dr["Uploaded On"] = dtUploadDatetime.ToString("dd-MMM-yyyy HH:mm:ss");
                    mdtEditFileUpload.Rows.Add(dr);
                    gvInsertFileUpload.DataSource = mdtEditFileUpload;
                    gvInsertFileUpload.DataBind();
                    Session["SubmissionMasFiles"] = mdtEditFileUpload;

                    ddlFileType.SelectedValue = "";
                    txtFileDesc.Text = "";
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

            //<<Added by Ankur Tyagi on 28-Apr-2025 for Project Id : 2395
            string strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                            "\r\nonClientSaveClick()\r\n" +
                            "</script>\r\n";

            ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);
            //>>
        }

        private void initFileUpload()
        {
            mdtEditFileUpload = new DataTable();
            mdtEditFileUpload.Columns.Add(new DataColumn("File Type", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("File Description", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("FileTypeShortForm", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("FileName", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("FileNameOnServer", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("Uploaded By", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("Uploaded On", typeof(string)));
        }

        protected void gvInsertFileUpload_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            GridViewRow gvr = gvInsertFileUpload.SelectedRow;
            HiddenField hfSMF_ID = (HiddenField)gvr.FindControl("hfSMF_ID");

            FileInfo fileInfo;
            string strFilePath;
            string strFileName;
            string strCompleteFileName;
            strFilePath = Server.MapPath(ConfigurationManager.AppSettings["SubmissionFiles"].ToString());
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

            if (hfSMF_ID != null)
            {
                int intSM_ID = ParseStringToInt(hfSMF_ID.Value);
                utilityBL.getDatasetWithCondition("deleteSubmissionFilesById", intSM_ID, mstrConnectionString);
            }

            writeError("File deleted successfully.");
        }

        protected void gvAlreadyUploaded_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            GridViewRow gvr = gvAlreadyUploaded.SelectedRow;
            HiddenField hfSMF_ID = (HiddenField)gvr.FindControl("hfSMF_ID");
            int intSM_ID = ParseStringToInt(hfSMF_ID.Value);

            if (intSM_ID > 0)
            {
                utilityBL.getDatasetWithCondition("deleteSubmissionFilesById", intSM_ID, mstrConnectionString);
            }

            writeError("File deleted successfully.");
        }

        private void InsertSubmissionMaster()
        {
            try
            {
                //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                int intA1 = 0, intB1 = 0, intA2 = 0, intB2 = 0;
                //>>

                #region
                //for (int j = 0; j <= cbCompany.Items.Count - 1; j++)
                //{

                //    liChkBoxListItemForValidation = cbCompany.Items[j];
                //    if (liChkBoxListItemForValidation.Selected)
                //    {
                //        blCheckForCompany = true;
                //    }
                //}
                //if (!blCheckForCompany)
                //{
                //    writeError("Please select atleast one company.");
                //    return;
                //}
                //for (int j = 0; j <= cblSegment.Items.Count - 1; j++)
                //{

                //    liChkBoxListItemForValidation = cblSegment.Items[j];
                //    if (liChkBoxListItemForValidation.Selected)
                //    {
                //        blCheeckForSegment = true;
                //    }
                //}
                //if (!blCheeckForSegment)
                //{
                //    writeError("Please select atleast one segment.");
                //    return;
                //}
                #endregion

                if (mdtEditFileUpload == null && gvAlreadyUploaded.Rows.Count == 0)
                {
                    writeError("Please upload all types of attachment before submit.");
                    return;
                }

                int intSubId;
                SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
                int intLOB;
                string strEffectiveDate = null, strParticulars = null, strDescription = null, strPriority;
                string strEscalate = null, strType = null, strReportingDepartment = "";
                string strFrequency = null, strOnceFromDate = null, strOnceToDate = null, strFromWeekDays = null;
                string strF1Fromdate = null, strF1ToDate = null, strF2FromDate = null, strF2ToDate = null;
                string strToWeekDays = null, strMonthlyFromDate = null, strMonthlyTodate = null, strQ1fromDate = null, strQ1ToDate = null, strQ2FromDate = null, strQ2ToDate = null;
                string strQ3FromDate = null, strQ3ToDate = null, strQ4fFromDate = null, strQ4Todate = null, strFirstHalffromDate = null, strFirstHalfToDate = null, strSecondtHalffromDate = null;
                string strSecondtHalffromTo = null, strYearlyfromDate = null, strYearlyDateTo = null, strCreateBy = null, strRegulation = null, strSection = null;
                int intSubType, intReportingDept, intEvent = 0, intAssociatedWith = 0, intStartDays = 0, intEndDays = 0, intlevel1 = 0, intlevel2 = 0;

                #region
                //for (int j = 0; j <= cbOwners.Items.Count - 1; j++)
                //{

                //    liChkBoxListItemForValidation = cbOwners.Items[j];
                //    if (liChkBoxListItemForValidation.Selected)
                //    {
                //        blCheckForOwners = true;
                //    }
                //}
                //if (!blCheckForOwners)
                //{
                //    writeError("Please select atleast one owner.");
                //    return;
                //}
                #endregion

                if (hfSMId.Value != "0")
                {
                    utilityBL.getDatasetWithCondition("DELETESUBSEGMENT", Convert.ToInt32(hfSMId.Value), mstrConnectionString);

                    //<<Added by Ankur Tyagi on 05-May-2025 for Project Id : 2395

                    if (mdtEditFileUpload != null)
                    {
                        intA1 = mdtEditFileUpload.AsEnumerable().Count(r => r.Field<string>("FileTypeShortForm") == "A");
                        intB1 = mdtEditFileUpload.AsEnumerable().Count(r => r.Field<string>("FileTypeShortForm") == "B");
                    }

                    DataTable dtAttachment = utilityBL.getDatasetWithCondition("getSubmisssionMasFiles_Edit", Convert.ToInt32(hfSMId.Value), mstrConnectionString);
                    if (dtAttachment.Rows.Count > 0)
                    {
                        intA2 = dtAttachment.AsEnumerable().Count(r => r.Field<string>("FileTypeShortForm") == "A");
                        intB2 = dtAttachment.AsEnumerable().Count(r => r.Field<string>("FileTypeShortForm") == "B");
                    }
                    int TotalA = intA1 + intA2;
                    int TotalB = intB1 + intB2;
                    if (TotalA < 1)
                    {
                        writeError("Please upload notification before submit.");
                        return;
                    }
                    else if (TotalB < 1)
                    {
                        writeError("Please upload prescribed template before submit.");
                        return;
                    }

                    //>>

                    intLOB = 0;
                    intSubType = Convert.ToInt32(ddlSubType.SelectedValue);
                    intReportingDept = Convert.ToInt32(ddlReportDept.SelectedValue);//Convert.ToInt32(ddlReportDept.SelectedValue);
                    strReportingDepartment = ddlReportDept.SelectedItem.Text;
                    strEffectiveDate = txtEffectiveDate.Text;
                    strParticulars = cm.getSanitizedString(txtParticulars.Text);
                    strDescription = cm.getSanitizedString(txtDescription.Text);
                    strType = rblType.SelectedValue;
                    strPriority = ddlPriority.SelectedValue;
                    strRegulation = cm.getSanitizedString(txtReference.Text);
                    strSection = cm.getSanitizedString(txtSection.Text);

                    if (strType == "E")
                    {
                        intEvent = Convert.ToInt32(ddlEvent.SelectedValue);
                        intAssociatedWith = Convert.ToInt32(rblAssociatedWith.SelectedValue);
                        intStartDays = Convert.ToInt32(txtStartDays.Text);
                        intEndDays = Convert.ToInt32(txtEndDays.Text);
                        //if (txtSortOrder.Text != "")
                        //{
                        //    intSortOrder = Convert.ToInt32(txtSortOrder.Text);
                        //}
                    }

                    else if (strType == "F")
                    {

                        strFrequency = rblFrequency.Text;
                        if (strFrequency == "Only Once")
                        {
                            strOnceFromDate = txtOnceFromDate.Text;
                            strOnceToDate = txtOnceToDate.Text;
                        }
                        else if (strFrequency == "Weekly")
                        {
                            strFromWeekDays = ddlFromWeekDays.SelectedValue;
                            strToWeekDays = ddlToWeekDays.SelectedValue;
                        }
                        else if (strFrequency == "Quarterly")
                        {
                            strQ1fromDate = txtQ1fromDate.Text;
                            strQ1ToDate = txtQ1ToDate.Text;
                            strQ2FromDate = txtQ2FromDate.Text;
                            strQ2ToDate = txtQ2ToDate.Text;
                            strQ3FromDate = txtQ3FromDate.Text;
                            strQ3ToDate = txtQ3Todate.Text;
                            strQ4fFromDate = txtQ4FromDate.Text;
                            strQ4Todate = txtQ4Todate.Text;
                        }
                        else if (strFrequency == "Monthly")
                        {
                            strMonthlyFromDate = txtMonthlyFromDate.Text;
                            strMonthlyTodate = txtMonthlyTodate.Text;
                        }
                        else if (strFrequency == "Half Yearly")
                        {
                            strFirstHalffromDate = txtFirstHalffromDate.Text;
                            strFirstHalfToDate = txtFirstHalfToDate.Text;
                            strSecondtHalffromDate = txtSecondtHalffromDate.Text;
                            strSecondtHalffromTo = txtSecondtHalffromTo.Text;
                        }
                        else if (strFrequency == "Yearly")
                        {
                            strYearlyfromDate = txtYearlyfromDate.Text;
                            strYearlyDateTo = txtYearlyDateTo.Text;
                        }
                        //<<Added By Vivek on 22-Jun-2017
                        else if (strFrequency == "Fortnightly")
                        {
                            strF1Fromdate = txtFortnightly1FromDate.Text;
                            strF1ToDate = txtFortnightly1ToDate.Text;
                            strF2FromDate = txtFortnightly2FromDate.Text;
                            strF2ToDate = txtFortnightly2ToDate.Text;
                        }
                        //>>
                    }


                    if (txtlevel1.Text != "")
                    {
                        intlevel1 = Convert.ToInt32(txtlevel1.Text);
                    }

                    if (txtlevel2.Text != "")
                    {
                        intlevel2 = Convert.ToInt32(txtlevel2.Text);
                    }
                    strEscalate = rblEscalate.SelectedValue;
                    strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                    //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                    string strWorkflowStatus = "";
                    if (strType == "F")
                    {
                        if (strFrequency == "Only Once")
                        {
                            strWorkflowStatus = "B";
                        }
                        else
                        {
                            strWorkflowStatus = "A";
                        }
                    }
                    else
                    {
                        strWorkflowStatus = "A";
                    }
                    //>>
                    string strNotiCirDate = null;
                    if (!string.IsNullOrEmpty(txtCircularDate.Text))
                    {
                        strNotiCirDate = txtCircularDate.Text;
                    }

                    intSubId = SubmissionMasterBLL.insertSubmissions(Convert.ToInt32(hfSMId.Value), "Active", intSubType, intReportingDept,
                                            strType, intEvent, intAssociatedWith, strParticulars, strDescription, intStartDays,
                                            intEndDays, strEscalate, strFrequency, strOnceFromDate, strOnceToDate,
                                            strFromWeekDays, strToWeekDays, strMonthlyFromDate, strMonthlyTodate, strQ1fromDate,
                                            strQ1ToDate, strQ2FromDate, strQ2ToDate, strQ3FromDate, strQ3ToDate, strQ4fFromDate,
                                            strQ4Todate, strFirstHalffromDate, strFirstHalfToDate, strSecondtHalffromDate, strSecondtHalffromTo,
                                            strYearlyfromDate, strYearlyDateTo, strF1Fromdate, strF1ToDate, strF2FromDate, strF2ToDate,
                                            intlevel1, intlevel2, strEffectiveDate, strPriority, strCreateBy,
                                            getSubmissionOwnerdt(),
                                            getSubmissionCompanydt(),
                                            getSubmissionSegmentdt(),
                                            getSubReportingOwnersdt(),
                                            mdtEditFileUpload, strRegulation, strSection, hfCircId.Value, mstrConnectionString,
                                            //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                                            intLOB, strWorkflowStatus, ddlFSAppReq.SelectedValue, strNotiCirDate, ParseStringToInt(txtlevel0.Text));
                    //>>
                    //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                    if (strType == "F")
                    {
                        if (strFrequency == "Only Once")
                        {
                            generateChecklist(intSubId);
                            sendMailToReportingOwnerOnCreation(strReportingDepartment, intReportingDept, intSubType);
                        }
                        else
                        {
                            sendMailToChecker(strReportingDepartment, intSubType);
                            writeError("Submission details saved successfully.");
                            pnlSubmissions.Visible = false;
                            lnkBack.Visible = true;
                        }
                    }
                    else
                    {
                        sendMailToChecker(strReportingDepartment, intSubType);
                        writeError("Submission details saved successfully.");
                        pnlSubmissions.Visible = false;
                        lnkBack.Visible = true;
                    }
                }
                else
                {
                    //<<Added by Ankur Tyagi on 05-May-2025 for Project Id : 2395
                    if (mdtEditFileUpload == null)
                    {
                        writeError("Please upload all types of attachment before submit.");
                        return;
                    }
                    else
                    {
                        bool hasA = mdtEditFileUpload.AsEnumerable().Any(row => row.Field<string>("FileTypeShortForm") == "A");
                        bool hasB = mdtEditFileUpload.AsEnumerable().Any(row => row.Field<string>("FileTypeShortForm") == "B");
                        if (hasA != true)
                        {
                            writeError("Please upload notification before submit.");
                            return;
                        }
                        else if (hasB != true)
                        {
                            writeError("Please upload prescribed template before submit.");
                            return;
                        }
                    }
                    //>>

                    ListItem liChkBoxListItem;
                    for (int i = 0; i <= cbReportingDept.Items.Count - 1; i++)
                    {
                        liChkBoxListItem = cbReportingDept.Items[i];
                        if (liChkBoxListItem.Selected)
                        {
                            //intLOB = Convert.ToInt32(ddlLOB.SelectedValue);
                            intLOB = 0;
                            intSubType = Convert.ToInt32(ddlSubType.SelectedValue);
                            intReportingDept = Convert.ToInt32(liChkBoxListItem.Value);//Convert.ToInt32(ddlReportDept.SelectedValue);
                            strReportingDepartment = liChkBoxListItem.Text;
                            strEffectiveDate = txtEffectiveDate.Text;
                            strParticulars = cm.getSanitizedString(txtParticulars.Text);
                            strDescription = cm.getSanitizedString(txtDescription.Text);
                            strType = rblType.SelectedValue;
                            strPriority = ddlPriority.SelectedValue;
                            strRegulation = cm.getSanitizedString(txtReference.Text);
                            strSection = cm.getSanitizedString(txtSection.Text);

                            if (strType == "E")
                            {
                                intEvent = Convert.ToInt32(ddlEvent.SelectedValue);
                                intAssociatedWith = Convert.ToInt32(rblAssociatedWith.SelectedValue);
                                intStartDays = Convert.ToInt32(txtStartDays.Text);
                                intEndDays = Convert.ToInt32(txtEndDays.Text);
                                //if (txtSortOrder.Text != "")
                                //{
                                //    intSortOrder = Convert.ToInt32(txtSortOrder.Text);
                                //}
                            }

                            else if (strType == "F")
                            {

                                strFrequency = rblFrequency.Text;
                                if (strFrequency == "Only Once")
                                {
                                    strOnceFromDate = txtOnceFromDate.Text;
                                    strOnceToDate = txtOnceToDate.Text;
                                }
                                else if (strFrequency == "Weekly")
                                {
                                    strFromWeekDays = ddlFromWeekDays.SelectedValue;
                                    strToWeekDays = ddlToWeekDays.SelectedValue;
                                }
                                else if (strFrequency == "Quarterly")
                                {
                                    strQ1fromDate = txtQ1fromDate.Text;
                                    strQ1ToDate = txtQ1ToDate.Text;
                                    strQ2FromDate = txtQ2FromDate.Text;
                                    strQ2ToDate = txtQ2ToDate.Text;
                                    strQ3FromDate = txtQ3FromDate.Text;
                                    strQ3ToDate = txtQ3Todate.Text;
                                    strQ4fFromDate = txtQ4FromDate.Text;
                                    strQ4Todate = txtQ4Todate.Text;
                                }
                                else if (strFrequency == "Monthly")
                                {
                                    strMonthlyFromDate = txtMonthlyFromDate.Text;
                                    strMonthlyTodate = txtMonthlyTodate.Text;
                                }
                                else if (strFrequency == "Half Yearly")
                                {
                                    strFirstHalffromDate = txtFirstHalffromDate.Text;
                                    strFirstHalfToDate = txtFirstHalfToDate.Text;
                                    strSecondtHalffromDate = txtSecondtHalffromDate.Text;
                                    strSecondtHalffromTo = txtSecondtHalffromTo.Text;
                                }
                                else if (strFrequency == "Yearly")
                                {
                                    strYearlyfromDate = txtYearlyfromDate.Text;
                                    strYearlyDateTo = txtYearlyDateTo.Text;
                                }
                                //<<Added By Vivek on 22-Jun-2017
                                else if (strFrequency == "Fortnightly")
                                {
                                    strF1Fromdate = txtFortnightly1FromDate.Text;
                                    strF1ToDate = txtFortnightly1ToDate.Text;
                                    strF2FromDate = txtFortnightly2FromDate.Text;
                                    strF2ToDate = txtFortnightly2ToDate.Text;
                                }
                                //>>
                            }


                            if (txtlevel1.Text != "")
                            {
                                intlevel1 = Convert.ToInt32(txtlevel1.Text);
                            }

                            if (txtlevel2.Text != "")
                            {
                                intlevel2 = Convert.ToInt32(txtlevel2.Text);
                            }
                            strEscalate = rblEscalate.SelectedValue;
                            strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                            //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                            string strWorkflowStatus = "";
                            if (strType == "F")
                            {
                                if (strFrequency == "Only Once")
                                {
                                    strWorkflowStatus = "B";
                                }
                                else
                                {
                                    strWorkflowStatus = "A";
                                }
                            }
                            else
                            {
                                strWorkflowStatus = "A";
                            }
                            //>>
                            string strNotiCirDate = null;
                            if (!string.IsNullOrEmpty(txtCircularDate.Text))
                            {
                                strNotiCirDate = txtCircularDate.Text;
                            }

                            intSubId = SubmissionMasterBLL.insertSubmissions(Convert.ToInt32(hfSMId.Value), "Active", intSubType, intReportingDept,
                                                    strType, intEvent, intAssociatedWith, strParticulars, strDescription, intStartDays,
                                                    intEndDays, strEscalate, strFrequency, strOnceFromDate, strOnceToDate,
                                                    strFromWeekDays, strToWeekDays, strMonthlyFromDate, strMonthlyTodate, strQ1fromDate,
                                                    strQ1ToDate, strQ2FromDate, strQ2ToDate, strQ3FromDate, strQ3ToDate, strQ4fFromDate,
                                                    strQ4Todate, strFirstHalffromDate, strFirstHalfToDate, strSecondtHalffromDate, strSecondtHalffromTo,
                                                    strYearlyfromDate, strYearlyDateTo, strF1Fromdate, strF1ToDate, strF2FromDate, strF2ToDate,
                                                    intlevel1, intlevel2, strEffectiveDate, strPriority, strCreateBy,
                                                    getSubmissionOwnerdt(),
                                                    getSubmissionCompanydt(),
                                                    getSubmissionSegmentdt(),
                                                    getSubReportingOwnersdt(),
                                                    mdtEditFileUpload, strRegulation, strSection, hfCircId.Value, mstrConnectionString,
                                                    //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                                                    intLOB, strWorkflowStatus, ddlFSAppReq.SelectedValue, strNotiCirDate, ParseStringToInt(txtlevel0.Text));
                            //>>
                            //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                            if (strType == "F")
                            {
                                if (strFrequency == "Only Once")
                                {
                                    generateChecklist(intSubId);
                                    sendMailToReportingOwnerOnCreation(strReportingDepartment, intReportingDept, intSubType);
                                }
                                else
                                {
                                    sendMailToChecker(strReportingDepartment, intSubType);
                                    writeError("Submission details saved successfully.");
                                    pnlSubmissions.Visible = false;
                                    btnAddMore.Visible = true;
                                }
                            }
                            else
                            {
                                sendMailToChecker(strReportingDepartment, intSubType);
                                writeError("Submission details saved successfully.");
                                pnlSubmissions.Visible = false;
                                btnAddMore.Visible = true;
                            }
                            //>>
                        }
                    }
                }
                Session["SubmissionMasFiles"] = null;
                gvInsertFileUpload.DataSource = null;
                gvInsertFileUpload.DataBind();
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
        int ParseStringToInt(string input)
        {
            int result;
            if (int.TryParse(input, out result))
            {
                return result;
            }
            return 0;
        }
        //>>

        private void generateChecklist(int intSubmissionMasterId)
        {
            string strSubmissionType;
            strSubmissionType = rblType.SelectedValue;
            DataServer dserv = new DataServer();
            DataTable dt = new DataTable();
            DataRow dr;
            string strId = "";
            string strEffectiveDate = txtEffectiveDate.Text;
            if (strSubmissionType == "F")
            {
                int intNoOfrecords;
                intNoOfrecords = SubmissionMasterBLL.generateSubmissionsWiseChecklist(intSubmissionMasterId, strEffectiveDate, mstrConnectionString);
                writeError("Submission details saved successfully.");
            }
            else if (strSubmissionType == "E")
            {
                dt = dserv.Getdata(" Select * From [TBL_EI_EP_MAPPING] " +
                                  " inner join [TBL_EVENT_PURPOSE] on [EEM_EP_ID] = [EP_ID] and [EP_ID] = " + rblAssociatedWith.SelectedValue +
                                  " inner join [TBL_EVENT_INSTANCES] on [EEM_EI_ID] = [EI_ID] and [EI_EM_ID] = " + ddlEvent.SelectedValue +
                                  " AND EI_EVENT_DATE >= '" + strEffectiveDate + "'");
                if (dt.Rows.Count > 0)
                {
                    int intNoOfrecords;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = dt.Rows[i];

                        strId = strId + dr["EEM_ID"].ToString() + ",";
                    }

                    if (!strId.Equals(""))
                        strId = strId.Substring(0, strId.Length - 1);

                    intNoOfrecords = SubmissionMasterBLL.generateSubmissionsWiseEventChecklist(intSubmissionMasterId, strId, mstrConnectionString);
                    writeError("Submission details saved successfully.");
                }
                else
                {
                    writeError("Submission details saved successfully. But the tasks were not created as there are no entries made for Event "
                        + ddlEvent.SelectedItem.Text + " and Agenda " + rblAssociatedWith.SelectedItem.Text + " in Event Instances. Please make an entry for the same.");
                }
                //string str
                //intNoOfrecords = SubmissionMasterBLL.generateSubmissionsWiseChecklist(intSubmissionMasterId, strEffectiveDate, mstrConnectionString);
            }

            //writeError("Submission details saved successfully with submission id: " + intSubmissionMasterId);
            pnlSubmissions.Visible = false;
            btnAddMore.Visible = true;
        }

        //<< Added By Vivek on 23-Jun-2017
        private void sendMailToReportingOwnerOnCreation(string strReportingDepartment, int intReportingDept, int intTrackingDept)
        {
            try
            {
                MembershipUser user;
                Authentication auth = new Authentication();
                DateTime dt = System.DateTime.Now;
                string[] strTo;
                string[] strCC;
                string strContent = "", strUserName = "", strUserDetails = "";
                string[] strUsers = new string[0];
                string strSubject, strIntDueDates = "", strRegDueDates = "";
                string strFrequency = "";

                MailConfigBLL mailBLL = new MailConfigBLL();
                DataTable dtMailConfig = mailBLL.searchMailConfig(null, "Add/Edit Submission");
                if (dtMailConfig == null || dtMailConfig.Rows.Count == 0)
                {
                    writeError("Mail Configuration is not set, could not send mail");
                    return;
                }
                DataRow drMailConfig = dtMailConfig.Rows[0];
                strSubject = drMailConfig["MCM_SUBJECT"].ToString();
                strContent = drMailConfig["MCM_CONTENT"].ToString();

                Mail mm = new Mail();

                //L0 User of Reporting Department
                DataTable dtL0RD = utilityBL.getDatasetWithTwoConditionInString("getReportingDeptUserFromLevel", Convert.ToString(intReportingDept), "0", mstrConnectionString);
                strTo = new string[dtL0RD.Rows.Count];
                int j = 0;
                if (strTo.GetUpperBound(0) >= 0)
                {
                    foreach (DataRow dr in dtL0RD.Rows)
                    {
                        strTo[j] = Convert.ToString(dr["EmailId"]);
                        j = j + 1;
                    }
                }

                //All Level Users of Tracking dept, L1 of Reporting Department and LoggedInUser
                DataTable dtAllTDU = new DataTable();
                DataTable dtL1RDU = new DataTable();

                dtAllTDU = utilityBL.getDatasetWithCondition("OWNERS_MAIL", intTrackingDept, mstrConnectionString);
                dtL1RDU = utilityBL.getDatasetWithTwoConditionInString("getReportingDeptUserFromLevel", Convert.ToString(intReportingDept), "0", mstrConnectionString);

                strCC = new string[dtAllTDU.Rows.Count + dtL1RDU.Rows.Count + 1];

                int i = 0;
                if (strCC.GetUpperBound(0) >= 0)
                {
                    foreach (DataRow dr in dtAllTDU.Rows)
                    {
                        strCC[i] = Convert.ToString(dr["EM_EMAIL"]);
                        i = i + 1;
                    }
                    foreach (DataRow dr in dtL1RDU.Rows)
                    {
                        strCC[i] = Convert.ToString(dr["EmailId"]);
                        i = i + 1;
                    }
                }
                user = Membership.GetUser(Page.User.Identity.Name);
                strCC[i] = user.Email;


                strUserDetails = auth.GetUserDetsByEmpCode(Page.User.Identity.Name);
                strUsers = strUserDetails.Split('|');
                strUserName = strUsers[0].ToString();

                if (rblType.SelectedValue.Equals("F"))
                {
                    strFrequency = rblFrequency.SelectedItem.Value.ToString();

                    if (strFrequency.Equals("Monthly"))
                    {
                        strIntDueDates = txtMonthlyFromDate.Text;
                        strRegDueDates = txtMonthlyTodate.Text;
                    }
                    else if (strFrequency == "Only Once")
                    {
                        strIntDueDates += txtOnceFromDate.Text + "<br/>";
                        strRegDueDates += txtOnceToDate.Text + "<br/>";
                    }
                    else if (strFrequency.Equals("Quarterly"))
                    {
                        strIntDueDates += txtQ1fromDate.Text + " for Quater1.<br/>";
                        strIntDueDates += txtQ2FromDate.Text + " for Quater2.<br/>";
                        strIntDueDates += txtQ3FromDate.Text + " for Quater3.<br/>";
                        strIntDueDates += txtQ4FromDate.Text + " for Quater4.";

                        strRegDueDates += txtQ1ToDate.Text + " for Quater1.<br/>";
                        strRegDueDates += txtQ2ToDate.Text + " for Quater2.<br/>";
                        strRegDueDates += txtQ3Todate.Text + " for Quater3.<br/>";
                        strRegDueDates += txtQ4Todate.Text + " for Quater4.";
                    }
                    else if (strFrequency.Equals("Half Yearly"))
                    {
                        strIntDueDates += txtFirstHalffromDate.Text + " for First half.<br/>";
                        strIntDueDates += txtSecondtHalffromDate.Text + " for Second half.<br/>";

                        strRegDueDates += txtFirstHalfToDate.Text + " for First half.<br/>";
                        strRegDueDates += txtSecondtHalffromTo.Text + " for Second half.<br/>";
                    }
                    else if (strFrequency.Equals("Yearly"))
                    {
                        strIntDueDates += txtYearlyfromDate + " of every year.";
                        strRegDueDates += txtSecondtHalffromTo.Text + " of every year.";
                    }
                    else if (strFrequency.Equals("Weekly"))
                    {
                        strIntDueDates += "Every " + ddlFromWeekDays.SelectedItem.Text;
                        strRegDueDates += "Every " + ddlToWeekDays.SelectedItem.Text;
                    }
                    else if (strFrequency == "Fortnightly")
                    {
                        strIntDueDates += "First Fortnightly From Date " + txtFortnightly1FromDate.Text + ".<br/>";
                        strIntDueDates += "Second Fortnightly From Date " + txtFortnightly2FromDate.Text + ".<br/>";

                        strRegDueDates += "First Fortnightly To Date " + txtFortnightly1ToDate.Text + ".<br/>";
                        strRegDueDates += "Second Fortnightly To Date " + txtFortnightly2ToDate.Text + ".";
                    }
                    else
                    {
                        strIntDueDates = "";
                        strRegDueDates = "";
                    }
                }

                strContent = strContent.Replace("%SubmittedBy%", Getfullname(strUserName));
                strContent = strContent.Replace("%SubmittedDate%", dt.ToString("dd-MMM-yyyy HH:mm:ss"));
                string strSubTable = "<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>" +
                            "<th " + strTableHeaderCSS + ">Reporting Department</th>" +
                            "<th " + strTableHeaderCSS + ">Reporting To</th>" +
                            "<th " + strTableHeaderCSS + ">Particulars</th>" +
                            "<th " + strTableHeaderCSS + ">Brief Description</th>" +
                            "<th " + strTableHeaderCSS + ">Type</th>" +
                            "<th " + strTableHeaderCSS + ">Frequency</th>";
                if (!strIntDueDates.Equals(""))
                {
                    strSubTable += "<th " + strTableHeaderCSS + ">Internal Due Date</th>";
                }
                if (!strRegDueDates.Equals(""))
                {
                    strSubTable += "<th " + strTableHeaderCSS + ">Regulatory Due Date</th>";
                }
                strSubTable += "</tr>";

                ListItem liChkBoxListItem;
                string strReportingTo = "";
                for (int i1 = 0; i1 <= cblSegment.Items.Count - 1; i1++)
                {

                    liChkBoxListItem = cblSegment.Items[i1];
                    if (liChkBoxListItem.Selected)
                    {
                        strReportingTo += liChkBoxListItem.Text + ", ";
                    }
                }
                if (strReportingTo.EndsWith(", "))
                {
                    strReportingTo = strReportingTo.Substring(0, strReportingTo.Length - 2);
                }

                strSubTable += "<tr><td " + strTableCellCSS + ">" + 1 + "</td>" +
                "<td " + strTableCellCSS + ">" + strReportingDepartment + "</td>" +
                "<td " + strTableCellCSS + ">" + strReportingTo + "</td>" +
                "<td " + strTableCellCSS + ">" + txtParticulars.Text.ToString().Replace(Environment.NewLine, "<br />") + "</td>" +
                "<td " + strTableCellCSS + ">" + txtDescription.Text.ToString().Replace(Environment.NewLine, "<br />") + "</td>" +
                "<td " + strTableCellCSS + ">" + rblType.SelectedItem.Text.ToString() + "</td>" +
                "<td " + strTableCellCSS + ">" + strFrequency + "</td>";

                if (!strIntDueDates.Equals(""))
                {
                    strSubTable += "<td " + strTableCellCSS + ">" + strIntDueDates + "</td>";
                }
                if (!strRegDueDates.Equals(""))
                {
                    strSubTable += "<td " + strTableCellCSS + ">" + strRegDueDates + "</td>";
                }
                strSubTable += "</table>";

                strSubject = strSubject.Replace("%Authority%", strReportingTo);
                strSubject = strSubject.Replace("%AddEditType%", "created");
                strSubject = strSubject.Replace("%Department%", strReportingDepartment);

                strContent = strContent.Replace("%Authority%", strReportingTo);
                strContent = strContent.Replace("%AddEditType%", "created");
                strContent = strContent.Replace("%Department%", strReportingDepartment);
                strContent = strContent.Replace("%Reason%", "");
                strContent = strContent.Replace("%SubmissionTable%", strSubTable);
                strContent = strContent.Replace("%Footer%", ConfigurationManager.AppSettings["MailFooter"].ToString());
                strContent = strContent.Replace("%Link%", "<a href=" + Global.site_url("Projects/Submissions/ComplianceChecklist.aspx?Type=4") + " target=\"_blank\">Click here</a>");

                //strContent = strContent + "<br/><br/>" +
                //ConfigurationManager.AppSettings["MailFooter"].ToString() + "</body></html>";

                mm.sendAsyncMail(strTo, strCC, null, strSubject, strContent);
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }
        //>>

        //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
        private void sendMailToChecker(string strReportingDepartment, int intTrackingDept)
        {
            try
            {
                MembershipUser user;
                Authentication auth = new Authentication();
                DateTime dt = System.DateTime.Now;
                string[] strTo;
                string[] strCC;
                string strContent = "", strUserName = "", strUserDetails = "";
                string[] strUsers = new string[0];
                string strSubject, strIntDueDates = "", strRegDueDates = "";
                string strFrequency = "";

                MailConfigBLL mailBLL = new MailConfigBLL();
                DataTable dtMailConfig = mailBLL.searchMailConfig(null, "Add/Edit Submission");
                if (dtMailConfig == null || dtMailConfig.Rows.Count == 0)
                {
                    writeError("Mail Configuration is not set, could not send mail");
                    return;
                }
                DataRow drMailConfig = dtMailConfig.Rows[0];
                strSubject = drMailConfig["MCM_SUBJECT"].ToString();
                strContent = drMailConfig["MCM_CONTENT"].ToString();

                Mail mm = new Mail();

                string[] strFA = Roles.GetUsersInRole("FilingAdmin");
                string[] strFSA = Roles.GetUsersInRole("Filing_Sub_Admin");
                string[] mergedArray = strFA.Concat(strFSA).ToArray();

                strTo = new string[mergedArray.Length];
                for (int i = 0; i < mergedArray.Length; i++)
                {
                    user = Membership.GetUser(mergedArray[i]);
                    strTo[i] = user.Email;
                }
                strTo = strTo.Where(email => email != auth.getEmailIDOnUserId(Page.User.Identity.Name)).ToArray();

                strCC = new string[1];
                user = Membership.GetUser(Page.User.Identity.Name);
                strCC[0] = user.Email;

                strUserDetails = auth.GetUserDetsByEmpCode(Page.User.Identity.Name);
                strUsers = strUserDetails.Split('|');
                strUserName = strUsers[0].ToString();

                if (rblType.SelectedValue.Equals("F"))
                {
                    strFrequency = rblFrequency.SelectedItem.Value.ToString();

                    if (strFrequency.Equals("Monthly"))
                    {
                        strIntDueDates = txtMonthlyFromDate.Text;
                        strRegDueDates = txtMonthlyTodate.Text;
                    }
                    else if (strFrequency == "Only Once")
                    {
                        strIntDueDates += txtOnceFromDate.Text + "<br/>";
                        strRegDueDates += txtOnceToDate.Text + "<br/>";
                    }
                    else if (strFrequency.Equals("Quarterly"))
                    {
                        strIntDueDates += txtQ1fromDate.Text + " for Quater1.<br/>";
                        strIntDueDates += txtQ2FromDate.Text + " for Quater2.<br/>";
                        strIntDueDates += txtQ3FromDate.Text + " for Quater3.<br/>";
                        strIntDueDates += txtQ4FromDate.Text + " for Quater4.";

                        strRegDueDates += txtQ1ToDate.Text + " for Quater1.<br/>";
                        strRegDueDates += txtQ2ToDate.Text + " for Quater2.<br/>";
                        strRegDueDates += txtQ3Todate.Text + " for Quater3.<br/>";
                        strRegDueDates += txtQ4Todate.Text + " for Quater4.";
                    }
                    else if (strFrequency.Equals("Half Yearly"))
                    {
                        strIntDueDates += txtFirstHalffromDate.Text + " for First half.<br/>";
                        strIntDueDates += txtSecondtHalffromDate.Text + " for Second half.<br/>";

                        strRegDueDates += txtFirstHalfToDate.Text + " for First half.<br/>";
                        strRegDueDates += txtSecondtHalffromTo.Text + " for Second half.<br/>";
                    }
                    else if (strFrequency.Equals("Yearly"))
                    {
                        strIntDueDates += txtYearlyfromDate + " of every year.";
                        strRegDueDates += txtSecondtHalffromTo.Text + " of every year.";
                    }
                    else if (strFrequency.Equals("Weekly"))
                    {
                        strIntDueDates += "Every " + ddlFromWeekDays.SelectedItem.Text;
                        strRegDueDates += "Every " + ddlToWeekDays.SelectedItem.Text;
                    }
                    else if (strFrequency == "Fortnightly")
                    {
                        strIntDueDates += "First Fortnightly From Date " + txtFortnightly1FromDate.Text + ".<br/>";
                        strIntDueDates += "Second Fortnightly From Date " + txtFortnightly2FromDate.Text + ".<br/>";

                        strRegDueDates += "First Fortnightly To Date " + txtFortnightly1ToDate.Text + ".<br/>";
                        strRegDueDates += "Second Fortnightly To Date " + txtFortnightly2ToDate.Text + ".";
                    }
                    else
                    {
                        strIntDueDates = "";
                        strRegDueDates = "";
                    }
                }

                strContent = strContent.Replace("%SubmittedBy%", Getfullname(strUserName));
                strContent = strContent.Replace("%SubmittedDate%", dt.ToString("dd-MMM-yyyy HH:mm:ss"));
                string strSubTable = "<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>" +
                            "<th " + strTableHeaderCSS + ">Particulars</th>" +
                            "<th " + strTableHeaderCSS + ">Brief Description</th>" +
                            "<th " + strTableHeaderCSS + ">Type</th>" +
                            "<th " + strTableHeaderCSS + ">Frequency</th>";

                if (!strIntDueDates.Equals(""))
                {
                    strSubTable += "<th " + strTableHeaderCSS + ">Internal Due Date</th>";
                }
                if (!strRegDueDates.Equals(""))
                {
                    strSubTable += "<th " + strTableHeaderCSS + ">Regulatory Due Date</th>";
                }
                strSubTable += "</tr>" +
                "<tr><td " + strTableCellCSS + ">" + 1 + "</td>" +
                "<td " + strTableCellCSS + ">" + txtParticulars.Text.ToString().Replace(Environment.NewLine, "<br />") + "</td>" +
                "<td " + strTableCellCSS + ">" + txtDescription.Text.ToString().Replace(Environment.NewLine, "<br />") + "</td>" +
                "<td " + strTableCellCSS + ">" + rblType.SelectedItem.Text.ToString() + "</td>" +

                "<td " + strTableCellCSS + ">" + strFrequency + "</td>";

                if (!strIntDueDates.Equals(""))
                {
                    strSubTable += "<td " + strTableCellCSS + ">" + strIntDueDates + "</td>";
                }
                if (!strRegDueDates.Equals(""))
                {
                    strSubTable += "<td " + strTableCellCSS + ">" + strRegDueDates + "</td>";
                }
                strSubTable += "</table>";

                ListItem liChkBoxListItem;
                string strReportingTo = "";
                for (int i1 = 0; i1 <= cblSegment.Items.Count - 1; i1++)
                {

                    liChkBoxListItem = cblSegment.Items[i1];
                    if (liChkBoxListItem.Selected)
                    {
                        strReportingTo += liChkBoxListItem.Text + ", ";
                    }
                }
                if (strReportingTo.EndsWith(", "))
                {
                    strReportingTo = strReportingTo.Substring(0, strReportingTo.Length - 2);
                }

                strSubject = strSubject.Replace("%Authority%", strReportingTo);
                strSubject = strSubject.Replace("%AddEditType%", "added");
                strSubject = strSubject.Replace("%Department%", strReportingDepartment);

                strContent = strContent.Replace("%Authority%", strReportingTo);
                strContent = strContent.Replace("%AddEditType%", "added");
                strContent = strContent.Replace("%Department%", strReportingDepartment);
                strContent = strContent.Replace("%Reason%", "");
                strContent = strContent.Replace("%SubmissionTable%", strSubTable);
                strContent = strContent.Replace("%Footer%", ConfigurationManager.AppSettings["MailFooter"].ToString());
                if (strFrequency == "Only Once")
                {
                    strContent = strContent.Replace("%Link%", "<a href=" + Global.site_url("Projects/Submissions/ComplianceChecklist.aspx?Type=4") + " target=\"_blank\">Click here</a>");
                }
                else
                {
                    strContent = strContent.Replace("%Link%", "<a href=" + Global.site_url("Projects/Submissions/SubmissionApproval.aspx") + " target=\"_blank\">Click here</a>");
                }

                mm.sendAsyncMail(strTo, strCC, null, strSubject, strContent);
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }
        //>>

        private DataTable getSubmissionOwnerdt()
        {
            DataTable dt = new DataTable();
            dt = new DataTable();

            dt.Columns.Add(new DataColumn("OwnerId", typeof(string)));
            //for (int i = 0; i <= cbOwners.Items.Count - 1; i++)
            //{
            //    liChkBoxListItem = cbOwners.Items[i];
            //    if (liChkBoxListItem.Selected)
            //    {
            //        dr = dt.NewRow();
            //        dr["OwnerId"] = liChkBoxListItem.Value;
            //        dt.Rows.Add(dr);
            //    }
            //}
            return dt;
        }
        private DataTable getSubmissionCompanydt()
        {
            DataTable dt = new DataTable();
            dt = new DataTable();

            dt.Columns.Add(new DataColumn("CompanyId", typeof(string)));
            //for (int i = 0; i <= cbCompany.Items.Count - 1; i++)
            //{

            //    liChkBoxListItem = cbCompany.Items[i];
            //    if (liChkBoxListItem.Selected)
            //    {
            //        dr = dt.NewRow();
            //        dr["CompanyId"] = liChkBoxListItem.Value;
            //        dt.Rows.Add(dr);
            //    }
            //}
            return dt;
        }
        private DataTable getSubmissionSegmentdt()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            ListItem liChkBoxListItem;
            dt = new DataTable();

            dt.Columns.Add(new DataColumn("SegmentId", typeof(string)));
            for (int i = 0; i <= cblSegment.Items.Count - 1; i++)
            {

                liChkBoxListItem = cblSegment.Items[i];
                if (liChkBoxListItem.Selected)
                {
                    dr = dt.NewRow();
                    dr["SegmentId"] = liChkBoxListItem.Value;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        private DataTable getSubReportingOwnersdt()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            ListItem liChkBoxListItem;
            dt = new DataTable();

            dt.Columns.Add(new DataColumn("OwnerId", typeof(string)));
            for (int i = 0; i <= cbDeptOwner.Items.Count - 1; i++)
            {

                liChkBoxListItem = cbDeptOwner.Items[i];
                if (liChkBoxListItem.Selected)
                {
                    dr = dt.NewRow();
                    dr["OwnerId"] = liChkBoxListItem.Value;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            if (Request.QueryString["SMId"] != null)
            {
                Response.Redirect("~/Projects/Submissions/SubmissionApproval.aspx");
            }
            else
            {
                Response.Redirect("~/default.aspx");
            }
        }

        //protected void ddlSubType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!ddlSubType.SelectedValue.Equals(""))
        //    {
        //        cbOwners.Items.Clear();
        //        cbOwners.DataSource = utilityBL.getDatasetWithCondition("OWNERS", Convert.ToInt32(ddlSubType.SelectedValue), mstrConnectionString);
        //        cbOwners.DataBind();
        //    }
        //    else
        //    {
        //        cbOwners.Items.Clear();
        //    }
        //}

        //protected void ddlReportDept_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!ddlReportDept.SelectedValue.Equals(""))
        //    {
        //        cbDeptOwner.Items.Clear();
        //        cbDeptOwner.DataSource = utilityBL.getDatasetWithCondition("REPORTINGOWNERS", Convert.ToInt32(ddlReportDept.SelectedValue), mstrConnectionString);
        //        cbDeptOwner.DataBind();
        //    }
        //    else
        //    {
        //        cbDeptOwner.Items.Clear();
        //    }
        //}

        protected void ddlEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ddlEvent.SelectedValue.Equals(""))
            {
                rblAssociatedWith.Items.Clear();
                rblAssociatedWith.DataSource = utilityBL.getDatasetWithCondition("EVENTPURPOSE_ACTIVE", Convert.ToInt32(ddlEvent.SelectedValue), mstrConnectionString);
                rblAssociatedWith.DataBind();
            }
            else
            {
                rblAssociatedWith.Items.Clear();
            }
            //     string script = "\r\n<script language=\"javascript\">\r\n" +
            //"   // alert('h');\r\n" +
            //"    var hf = document.getElementById('hfFixedOrEvent');\r\n" +
            //"   // alert('hi');\r\n" +
            //"    var elem = document.getElementById('FixedDateBaseSection');\r\n" +
            //"    //alert(elem);\r\n" +
            //"    var elem1 = document.getElementById('EventBasedSection');\r\n" +
            //"    //alert('hi4');\r\n" +
            //"    if (hf.value == 'E')\r\n" +
            //"       { \r\n" +
            //"        // alert('hi2');\r\n" +
            //"               \r\n" +
            //"            elem.style.display = 'block';\r\n" +
            //"           elem.style.visibility = 'visible';\r\n" +
            //"           elem1.style.display = 'none';\r\n" +
            //"           elem1.style.visibility = 'hidden';\r\n" +
            //"       }\r\n" +
            //"       else\r\n" +
            //"       {         \r\n" +
            //"           elem.style.display = 'none';\r\n" +
            //"           elem.style.visibility = 'hidden';\r\n" +
            //"           elem1.style.display = 'block';\r\n" +
            //"           elem1.style.visibility = 'visible';\r\n" +
            //"       }\r\n" +
            //"   </script>\r\n";

            //ClientScript.RegisterStartupScript(this.GetType(), "script", script);
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        private void clearControls()
        {
            ddlSubType.SelectedIndex = -1;
            txtParticulars.Text = "";
            txtDescription.Text = "";
            rblType.SelectedIndex = -1;
            ddlEvent.SelectedIndex = -1;
            txtOnceFromDate.Text = "";
            txtOnceToDate.Text = "";
            ddlFromWeekDays.SelectedIndex = -1;
            ddlToWeekDays.SelectedIndex = -1;
            txtQ1fromDate.Text = "";
            txtQ1ToDate.Text = "";
            txtQ2FromDate.Text = "";
            txtQ2ToDate.Text = "";
            txtQ3FromDate.Text = "";
            txtQ3Todate.Text = "";
            txtQ4FromDate.Text = "";
            txtQ4Todate.Text = "";
            txtMonthlyFromDate.Text = "";
            txtMonthlyTodate.Text = "";
            txtFirstHalffromDate.Text = "";
            txtFirstHalfToDate.Text = "";
            txtSecondtHalffromDate.Text = "";
            txtSecondtHalffromTo.Text = "";
            txtYearlyfromDate.Text = "";
            txtYearlyDateTo.Text = "";
            txtlevel1.Text = "";
            txtlevel2.Text = "";
            cblSegment.SelectedIndex = -1;
            rblFrequency.SelectedIndex = -1;
            txtStartDays.Text = "";
            txtEndDays.Text = "";
            //txtSortOrder.Text = "";
            rblEscalate.SelectedIndex = -1;
            //cbOwners.SelectedIndex = -1;
            //  cbCompany.SelectedIndex = -1;
            rblAssociatedWith.Items.Clear();
            //cbOwners.Items.Clear();
            txtEffectiveDate.Text = "";
            cbDeptOwner.Items.Clear();
            //ddlReportDept.SelectedIndex = -1;
            cbReportingDept.SelectedIndex = -1;
            ddlPriority.SelectedIndex = -1;

            //<<Added by Vivek on 30-Apr-2020
            txtReference.Text = "";
            txtSection.Text = "";
            //>>
        }

        protected void cvdate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
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
        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }
    }
}