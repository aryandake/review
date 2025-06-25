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
using Fiction2Fact.Legacy_App_Code.Circulars.BLL;

namespace Fiction2Fact.Projects.Compliance
{
    public partial class AddComplianceReview : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        RefCodesBLL refBL = new RefCodesBLL();
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();
        CircularMasterBLL CircularMasterBLL = new CircularMasterBLL();
        string script = "";
        CommonMethods cm = new CommonMethods();
        DataTable dtList = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                litAttachment.Text = getAttachmentHTMLTable() + "</table>";

                CalendarExtender1.StartDate = DateTime.Now;
                CalendarExtender2.StartDate = DateTime.Now;

                FillStatus();
                FillReviewType();
                FillUniverseMaster();
                FillReviewerName();
                FillBusinessUnits();

                ddlLinkageWithEarlierCircular.Items.AddRange(CommonCodes.GetYesNoDDLItems());
                CommonCodes.SetDropDownDataSource(ddlSOCEOC, refBL.getRefCodeDetails("Circulars - New Circular - supersedes or extension/amendment to the old circular"));
                reloadDataListViewState();

                if (Request.QueryString["Source"] != null)
                {
                    hfSource.Value = Request.QueryString["Source"].ToString();
                }

                if (Request.QueryString["Id"] != null)
                {
                    hfCCR_ID.Value = Request.QueryString["Id"].ToString();
                    hfSelectedRecord.Value = hfCCR_ID.Value;
                }

                if (hfCCR_ID.Value.Equals("0") || hfCCR_ID.Value.Equals(""))
                {
                    lblHeader.Text = "Initiate Compliance Review";
                }
                else
                {
                    lblHeader.Text = "Edit Compliance Review Initiation";
                    getDetails();
                }
            }
        }

        #region for Old Circular Subject/No.
        private void reloadDataListViewState()
        {
            try
            {
                dtList.Columns.Add("Name");
                dtList.Columns.Add("CircularName");
                ViewState["Data"] = dtList;
                dlUserList.DataSource = dtList;
                dlUserList.DataBind();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void loadExistingCircularList(string strOldCircularIds)
        {
            DataTable dtRecords = new DataTable();
            try
            {
                dtRecords = CircularMasterBLL.SearchCircular(Convert.ToInt32(hfSelectedRecord.Value), "", "", "", "", "", "", "",
                    "", "", "OldCircular", "", "", "", "", strCircularIds: strOldCircularIds).Tables[0];
                dlUserList.DataSource = dtRecords;
                dlUserList.DataBind();
                ViewState["Data"] = dtRecords;
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void SaveAndBindDatatable(DataTable dt)
        {
            try
            {
                ViewState["Data"] = dtList;
                dlUserList.DataSource = dtList;
                dlUserList.DataBind();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void ImageButton1_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            DataListItem dataitem = (DataListItem)btn.NamingContainer;

            try
            {
                int index = dataitem.ItemIndex;
                dtList = (DataTable)ViewState["Data"];
                dtList.Rows.RemoveAt(index);
                SaveAndBindDatatable(dtList);
                txtOldCircSubjectNo.Text = string.Empty;
                txtOldCircSubjectNo.Focus();
                hfOldCircularId.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void hfOldCircularId_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                dtList = (DataTable)ViewState["Data"];
                DataRow dr = dtList.NewRow();
                dr["CircularName"] = hfOldCircularId.Value;
                dr["Name"] = txtOldCircSubjectNo.Text;
                dtList.Rows.Add(dr);
                SaveAndBindDatatable(dtList);
                txtOldCircSubjectNo.Text = "";
                hfOldCircularId.Value = "";
                txtOldCircSubjectNo.Focus();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        #endregion

        void FillBusinessUnits()
        {
            DataTable dt = new DataTable();
            dt = oBLL.Search_SubFunction_Master(0, null,strCSFM_Status:"A");
            if (dt.Rows.Count > 0)
            {
                cblBusinessUnits.DataSource = dt;
            }
            else
            {
                cblBusinessUnits.DataSource = null;
            }
            cblBusinessUnits.DataBind();
        }


        private void getDetails()
        {
            DataRow drRR;
            DataTable dtRR = new DataTable();
            StringBuilder sbAttachmentHTMLRows = new StringBuilder();
            int intCCRId = 0, uniqueRowId = 0;

            try
            {
                hfSelectedOperation.Value = "Edit";
                string strRRId = hfCCR_ID.Value.ToString();
                bool isrrID = int.TryParse(hfCCR_ID.Value.ToString(), out intCCRId);
                string strCCR_CREATOR = Page.User.Identity.Name;

                if (!strRRId.Equals(""))
                {
                    dtRR = oBLL.Search_ComplianceReview(intCCRId, 0, 0, "", "", strCCR_CREATOR,null);
                }

                if (dtRR.Rows.Count > 0)
                {
                    drRR = dtRR.Rows[0];
                    lblCCIRd.Text = drRR["CCR_ID"].ToString();
                    lblCCRRefNo.Text = drRR["CCR_IDENTIFIER"].ToString();
                    ddlActRegulationCircular.SelectedValue = drRR["CCR_CRUM_ID"].ToString();

                    if (cblBusinessUnits.Items.Count > 0)
                    {
                        foreach (string s in drRR["CCR_UNIT_IDS"].ToString().Split(','))
                        {
                            foreach (ListItem li in cblBusinessUnits.Items)
                            {
                                if (s == li.Value)
                                {
                                    li.Selected = true;
                                    break;
                                }
                            }
                        }
                    }

                    ddlReviewerName.SelectedValue = drRR["CCR_CRM_ID"].ToString();
                    ddlStatus.SelectedValue = drRR["CCR_REC_STATUS"].ToString();
                    ddlReviewType.SelectedValue = drRR["CCR_REVIEW_TYPE"].ToString();
                    txtReviewScope.Text = drRR["CCR_REVIEW_SCOPE"].ToString();
                    txtRemarks.Text = drRR["CCR_REMARKS"].ToString();

                    if (drRR["CCR_TENTATIVE_START_DATE"] != null && drRR["CCR_TENTATIVE_START_DATE"] != DBNull.Value)
                    {
                        txtTentativeStartDT.Text = ((DateTime)drRR["CCR_TENTATIVE_START_DATE"]).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        txtTentativeStartDT.Text = "";
                    }
                    if (drRR["CCR_TENTATIVE_END_DATE"] != null && drRR["CCR_TENTATIVE_END_DATE"] != DBNull.Value)
                    {
                        txtTentativeEndDT.Text = ((DateTime)drRR["CCR_TENTATIVE_END_DATE"]).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        txtTentativeEndDT.Text = "";
                    }

                    txtRemarks.Text = drRR["CCR_REMARKS"].ToString();


                    CommonCodes.SetDropDownDataSource(ddlLinkageWithEarlierCircular, sSelected: (drRR["CCR_LINKAGE_WITH_EARLIER_CIRCULAR"] is DBNull ? "" : drRR["CCR_LINKAGE_WITH_EARLIER_CIRCULAR"].ToString()));
                    CommonCodes.SetDropDownDataSource(ddlSOCEOC, sSelected: (drRR["CCR_SOC_EOC"] is DBNull ? "" : drRR["CCR_SOC_EOC"].ToString()));
                    string strOldCircularIds = (drRR["CCR_BASE_ID"] is DBNull ? "" : drRR["CCR_BASE_ID"].ToString());
                    loadExistingCircularList(strOldCircularIds);

                    #region Files
                    script += "\r\n <script type=\"text/javascript\">\r\n";
                    litAttachment.Text = getAttachmentHTMLTable();

                    DataTable dtFiles = oBLL.Search_Compliance_Review_Files(0, intCCRId);

                    for (int i = 0; i < dtFiles.Rows.Count; i++)
                    {
                        uniqueRowId = uniqueRowId + 1;
                        DataRow drFiles = dtFiles.Rows[i];

                        sbAttachmentHTMLRows.Append(" <tr><td class='contentBody'> " +
                                                    " <input type='hidden' ID='uniqueRowId" + uniqueRowId + "' value='" + uniqueRowId + "' /> " +
                                                    " <input type='hidden' ID='attachId" + uniqueRowId + "' value='" + drFiles["CCRF_ID"].ToString() + "' /> " +
                                                    " <input type='checkbox' ID='checkAttachment" + uniqueRowId + "' values='0' /></td> " +
                                                    " <td class='contentBody'> " +
                                                    " <input type='hidden' ID='attachClientFileName" + uniqueRowId + "' value='" + drFiles["CCRF_CLIENT_FILE_NAME"].ToString() + "'/> " +
                                                    " <input type='hidden' ID='attachServerFileName" + uniqueRowId + "' value='" + drFiles["CCRF_SERVER_FILE_NAME"].ToString() + "'/> " +
                                                    " <input type='hidden' ID='attachFileTypeID" + uniqueRowId + "' value='" + drFiles["CCRF_FILE_TYPE"].ToString() + "'/> " +
                                                    " <input type='hidden' ID='attachFileDesc" + uniqueRowId + "' value='" + drFiles["CCRF_DESC"].ToString() + "'/> " + drFiles["RC_NAME"].ToString() + " </td> " +
                                                    " <td class='contentBody'>" + drFiles["CCRF_DESC"].ToString().Replace(Environment.NewLine, "<br />") + "</td> " +
                                                    " <td class='contentBody'><a id='attachfilelink" + uniqueRowId + "' href='../CommonDownload.aspx?type=CRI&downloadFileName=" + drFiles["CCRF_SERVER_FILE_NAME"].ToString() +
                                                    "&fileName=" + drFiles["CCRF_CLIENT_FILE_NAME"].ToString() + "'>" + drFiles["CCRF_CLIENT_FILE_NAME"].ToString() + "</a></td></tr> ");
                    }

                    litAttachment.Text += sbAttachmentHTMLRows.ToString() + "</table>";

                    script += "</script>\r\n";
                    ClientScript.RegisterStartupScript(this.GetType(), "return script", script);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }


        private string getAttachmentHTMLTable()
        {
            StringBuilder sbAttachmentHTML = new StringBuilder();

            sbAttachmentHTML.Append(" <table id='tblAttachment' width='100%' border='0' class='table table-bordered footable' cellpadding='0' cellspacing='1'> " +
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

        void FillStatus()
        {
            UtilitiesDAL ouBLL = new UtilitiesDAL();
            UtilitiesVO oUV = new UtilitiesVO();
            oUV.setCode("  where SM_TYPE='Compliance Review Status' Order by SM_SORT_ORDER asc");
            ddlStatus.DataSource = ouBLL.getData("getAllStatus", oUV);
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("--Select--", ""));
            if (ddlStatus.Items.Count > 0)
            {
                ddlStatus.SelectedIndex = 1;
            }
        }

        void FillReviewType()
        {
            ddlReviewType.Items.Clear();
            ddlReviewType.DataSource = refBL.getRefCodeDetails("Compliance Review - Review Type", mstrConnectionString);
            ddlReviewType.DataBind();
            ddlReviewType.Items.Insert(0, new ListItem("--Select--", ""));
        }

        void FillUniverseMaster()
        {
            ddlActRegulationCircular.Items.Clear();
            ddlActRegulationCircular.DataSource = oBLL.Search_Universe_Master(null, "A", strvalue: " Order by CRUM_UNIVERSE_TO_BE_REVIEWED asc");
            ddlActRegulationCircular.DataBind();
            ddlActRegulationCircular.Items.Insert(0, new ListItem("--Select--", ""));
        }

        void FillReviewerName()
        {
            ddlReviewerName.Items.Clear();
            ddlReviewerName.DataSource = oBLL.Search_Reviewer_Master(0, null, null, null, "A");
            ddlReviewerName.DataBind();
            ddlReviewerName.Items.Insert(0, new ListItem("--Select--", ""));
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
                dr["Type"] = "CRIFile";
                dtAttachment.Rows.Add(dr);
            }
            return dtAttachment;
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtFiles = new DataTable();
                dtFiles = getAttachmentdt();

                #region Get Ciruclar ID
                string strLinkageWithEarlierCircular = "", strSOCEOC = "", strOldCircSubNo = "", strOldCircId = "";
                strLinkageWithEarlierCircular = ddlLinkageWithEarlierCircular.SelectedValue;

                if (strLinkageWithEarlierCircular.Equals("Y"))
                {
                    strSOCEOC = ddlSOCEOC.SelectedValue;

                    DataTable dt = (DataTable)ViewState["Data"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        strOldCircId = (string.IsNullOrEmpty(strOldCircId) ? "" : strOldCircId + ",") + dr["CircularName"].ToString();
                        strOldCircSubNo = (string.IsNullOrEmpty(strOldCircSubNo) ? "" : strOldCircSubNo + ",") + dr["Name"].ToString();
                    }
                } 
                #endregion

                #region Bind Controls

                int intCCR_ID = string.IsNullOrEmpty(hfCCR_ID.Value) ? 0 : Convert.ToInt32(hfCCR_ID.Value);
                string strCCR_IDENTIFIER = null;
                int intCCR_CRUM_ID = Convert.ToInt32(ddlActRegulationCircular.SelectedItem.Value);
                string strCCR_UNIT_IDS = null;

                #region Units
                if (cblBusinessUnits.Items.Count > 0)
                {
                    foreach (ListItem li in cblBusinessUnits.Items)
                    {
                        if (li.Selected)
                        {
                            strCCR_UNIT_IDS += (string.IsNullOrEmpty(strCCR_UNIT_IDS) ? "" : ",") + li.Value;
                        }
                    }
                }
                #endregion

                string strCCR_QUARTER = null;
                DateTime? dtCCR_TENTATIVE_START_DATE = new DateTime?();
                DateTime? dtCCR_TENTATIVE_END_DATE = new DateTime?();

                if (!string.IsNullOrEmpty(txtTentativeEndDT.Text))
                {
                    dtCCR_TENTATIVE_END_DATE = Convert.ToDateTime(txtTentativeEndDT.Text);
                }
                if (!string.IsNullOrEmpty(txtTentativeStartDT.Text))
                {
                    dtCCR_TENTATIVE_START_DATE = Convert.ToDateTime(txtTentativeStartDT.Text);
                }

                string strCCR_STATUS = ddlStatus.SelectedItem.Value;
                string strCCR_REC_STATUS = null;
                string strCCR_REC_STATUS_REMARKS = null;
                int intCCR_CRM_ID = Convert.ToInt32(ddlReviewerName.SelectedItem.Value);
                string strCCR_REVIEW_SCOPE = txtReviewScope.Text;
                string strCCR_REVIEW_OBJECTIVE = null;
                string strCCR_REVIEW_STAGE = null;
                string strCCR_REMARKS = txtRemarks.Text;
                string strCCR_REVIEW_TYPE = ddlReviewType.SelectedItem.Value;
                DateTime? dtCCR_WORK_STARTED_ON = new DateTime?();
                string strCCR_APPROVAL_BY_L1 = null;
                DateTime? dtCCR_APPROVAL_ON_L1 = new DateTime?();
                string strCCR_APPROVAL_BY_L2 = null;
                DateTime? dtCCR_APPROVAL_ON_L2 = new DateTime?();
                string strCCR_CREATOR = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                string strCCR_AUDIT_TRAIL = null;
                string strCCR_CREATE_BY = Page.User.Identity.Name;
                string strCCR_UPDATE_BY = Page.User.Identity.Name;
                
                #endregion
                if (string.IsNullOrEmpty(hfSelectedOperation.Value))
                {
                    //for insertion
                    int x = oBLL.SaveComplianceReview_with_Files(intCCR_ID, strCCR_IDENTIFIER, intCCR_CRUM_ID, strCCR_UNIT_IDS, strCCR_QUARTER, dtCCR_TENTATIVE_START_DATE, dtCCR_TENTATIVE_END_DATE, strCCR_STATUS,
                   strCCR_REC_STATUS, strCCR_REC_STATUS_REMARKS, intCCR_CRM_ID, strCCR_REVIEW_SCOPE, strCCR_REVIEW_OBJECTIVE, strCCR_REVIEW_STAGE,
                   strCCR_REMARKS, strCCR_REVIEW_TYPE, dtCCR_WORK_STARTED_ON, strCCR_APPROVAL_BY_L1, dtCCR_APPROVAL_ON_L1, strCCR_APPROVAL_BY_L2, dtCCR_APPROVAL_ON_L2, strCCR_CREATOR, strCCR_AUDIT_TRAIL,
                   strCCR_CREATE_BY, strCCR_UPDATE_BY, dtFiles, strLinkageWithEarlierCircular, strSOCEOC, strOldCircSubNo, strOldCircId);
                    if (x > 0)
                    {
                        strCCR_IDENTIFIER = "";
                        DataTable dt = new DataTable();
                        dt = oBLL.Search_ComplianceReview(x, 0, 0, null, null, null, null);
                        if(dt.Rows.Count>0)
                        {
                            strCCR_IDENTIFIER = dt.Rows[0]["CCR_IDENTIFIER"].ToString();
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MYalert", "alert('Compliance Review submitted successfully with No. "+ strCCR_IDENTIFIER + "..');window.location.href='SearchComplianceReview.aspx?Type=MY';", true);
                        //writeError("Record Saved Successfully..");
                    }
                }
                else
                {
                    //for updation
                    intCCR_ID = Convert.ToInt32(hfCCR_ID.Value);
                    int x = oBLL.SaveComplianceReview_with_Files(intCCR_ID, strCCR_IDENTIFIER, intCCR_CRUM_ID, strCCR_UNIT_IDS, strCCR_QUARTER, dtCCR_TENTATIVE_START_DATE, dtCCR_TENTATIVE_END_DATE, strCCR_STATUS,
                   strCCR_REC_STATUS, strCCR_REC_STATUS_REMARKS, intCCR_CRM_ID, strCCR_REVIEW_SCOPE, strCCR_REVIEW_OBJECTIVE, strCCR_REVIEW_STAGE,
                   strCCR_REMARKS, strCCR_REVIEW_TYPE, dtCCR_WORK_STARTED_ON, strCCR_APPROVAL_BY_L1, dtCCR_APPROVAL_ON_L1, strCCR_APPROVAL_BY_L2, dtCCR_APPROVAL_ON_L2, strCCR_CREATOR, strCCR_AUDIT_TRAIL,
                   strCCR_CREATE_BY, strCCR_UPDATE_BY, dtFiles, strLinkageWithEarlierCircular, strSOCEOC, strOldCircSubNo, strOldCircId);
                    if (x > 0)
                    {
                        ScriptManager.RegisterStartupScript(this,this.GetType(),"MYalert", "alert('Compliance Review updated successfully..');window.location.href='SearchComplianceReview.aspx?Type=MY';", true);
                        //writeError("Record Updated Successfully..");
                    }
                }
                hfDoubleClickFlag.Value = string.Empty;
                ClearControls();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = string.Empty;
                hfSelectedOperation.Value = string.Empty;
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

        }
        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void cvEndDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
        void ClearControls()
        {
            txtRemarks.Text = string.Empty;
            txtReviewScope.Text = string.Empty;
            txtTentativeEndDT.Text = string.Empty;
            txtTentativeStartDT.Text = string.Empty;
            hfSelectedOperation.Value = string.Empty;
            cblBusinessUnits.Items.Clear();
            FillReviewType();
            FillStatus();
            FillUniverseMaster();
            FillBusinessUnits();
            FillReviewerName();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}