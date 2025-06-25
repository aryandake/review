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
using Fiction2Fact.Legacy_App_Code.Circulars;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using System.Collections.Generic;
using System.Web.Helpers;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.App_Code;
using Fiction2Fact.F2FControls;
using System.Text;

namespace Fiction2Fact.Projects.Circulars
{
    public partial class Circulars_EditCircular : System.Web.UI.Page
    {
        private DataTable mdtCircularFileUpload;
        string mstrConnectionString = null;
        CircularMasterBLL CircularMasterBLL = new CircularMasterBLL();
        //UtilitiesBLL utilityBL = new UtilitiesBLL();
        CircUtilitiesBLL circUtilBLL = new CircUtilitiesBLL();
        //UtilitiesVO utlVO = new UtilitiesVO();
        RefCodesBLL rcBL = new RefCodesBLL();
        CommonMethods cm = new CommonMethods();
        DataTable dtList = new DataTable();
        DataTable dtAdditionalMailTOList = new DataTable();
        DataTable dtAdditionalMailCCList = new DataTable();
        SHA256EncryptionDecryption en = new SHA256EncryptionDecryption();

        private void UpdateCircular(string strStatus = "")
        {
            try
            {
                int intCircularId, intSpocFromComplianceFn;
                string strNameofCircular = null, strCircularDetails, strCircularDate, strCircularNo, strDownloadRefNo = null, strSubDocumentType;
                string strTopic, strImplication, strIssueLink, strAssociatedKeywords = "", strLinkageWithEarlierCircular = "", strSOCEOC = "",
                    strOldCircSubNo = "", strOldCircId = "", strAuditCommitteeToApprove = "", strToBePlacedBefore = "", strDetails = "",
                    strNameOfThePolicy = "", strFrequency = "", strCircEffDate = "", strBroadcastDate = "", strSpocFromComplianceFn = "";
                string strUser = Authentication.GetUserID(Page.User.Identity.Name);
                int intCircularAuthority, intArea, intDepartment, intTypeofCircular;

                CheckBoxList cblIntimations = ((CheckBoxList)(fvCircularMaster.FindControl("cbSubmissions")));

                int intCircularMasterId = Convert.ToInt32(fvCircularMaster.SelectedValue);
                intTypeofCircular = Convert.ToInt32(((DropDownList)(fvCircularMaster.FindControl("ddlTypeofCircular"))).SelectedValue);
                strSubDocumentType = ((DropDownList)(fvCircularMaster.FindControl("ddlSubTypeofCircular"))).SelectedValue;
                intCircularAuthority = Convert.ToInt32(((DropDownList)(fvCircularMaster.FindControl("ddlCircularAuthority"))).SelectedValue);
                hfCircularAuthority.Value = ((DropDownList)(fvCircularMaster.FindControl("ddlCircularAuthority"))).SelectedItem.ToString();
                intArea = Convert.ToInt32(((DropDownList)(fvCircularMaster.FindControl("ddlArea"))).SelectedValue);
                intDepartment = Convert.ToInt32(((DropDownList)(fvCircularMaster.FindControl("ddlDepartment"))).SelectedValue);
                strCircularNo = cm.getSanitizedString(((F2FTextBox)(fvCircularMaster.FindControl("txtCircularNo"))).Text);
                strCircularDate = cm.getSanitizedString(((F2FTextBox)(fvCircularMaster.FindControl("txtCircularDate"))).Text);
                hfCircularDate.Value = strCircularDate;
                strCircEffDate = cm.getSanitizedString(((F2FTextBox)(fvCircularMaster.FindControl("txtCircEffDate"))).Text);
                strTopic = cm.getSanitizedString(((F2FTextBox)(fvCircularMaster.FindControl("txtTopic"))).Text);
                hfTopic.Value = strTopic;
                F2FTextBox FCKE_EditorImplications = (F2FTextBox)(fvCircularMaster.FindControl("FCKE_EditorImplications"));
                strImplication = FCKE_EditorImplications.Text;
                strIssueLink = cm.getSanitizedString(((F2FTextBox)(fvCircularMaster.FindControl("txtLink"))).Text);
                F2FTextBox FCKE_EditCircularDetails = (F2FTextBox)(fvCircularMaster.FindControl("FCKE_EditCircularDetails"));
                strCircularDetails = FCKE_EditCircularDetails.Text;
                intSpocFromComplianceFn = Convert.ToInt32(((DropDownList)(fvCircularMaster.FindControl("ddlSpocFromCompFn"))).SelectedValue);
                //utilityBL.getDatasetWithCondition("DELETEINTIMATIONS", intCircularMasterId, mstrConnectionString);
                circUtilBLL.GetDataTable("DELETEINTIMATIONS", new DBUtilityParameter("CMI_CM_ID", intCircularMasterId));

                //<< Added by Amarjeet on 26-Jul-2021
                CheckBoxList cbAssociatedKeywords = ((CheckBoxList)(fvCircularMaster.FindControl("cbAssociatedKeywords")));
                strAssociatedKeywords = CommonCodes.getCommaSeparatedValuesFromCheckboxList(cbAssociatedKeywords);
                //>>

                //<< Added by Amarjeet on 04-Aug-2021
                strLinkageWithEarlierCircular = ((DropDownList)(fvCircularMaster.FindControl("ddlLinkageWithEarlierCircular"))).SelectedValue;
                strSOCEOC = ((DropDownList)(fvCircularMaster.FindControl("ddlSOCEOC"))).SelectedValue;
                //strOldCircSubNo = ((F2FTextBox)(fvCircularMaster.FindControl("txtOldCircSubjectNo"))).Text;
                //strOldCircId = ((HiddenField)(fvCircularMaster.FindControl("hfOldCircularId"))).Value;

                if (strLinkageWithEarlierCircular.Equals("N"))
                {
                    strSOCEOC = "";
                    strOldCircSubNo = "";
                    strOldCircId = "";
                }
                else
                {
                    DataTable dt = (DataTable)ViewState["Data"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        strOldCircId = (string.IsNullOrEmpty(strOldCircId) ? "" : strOldCircId + ",") + dr["CircularName"].ToString();
                        strOldCircSubNo = (string.IsNullOrEmpty(strOldCircSubNo) ? "" : strOldCircSubNo + ",") + dr["Name"].ToString();
                    }
                }

                loadExistingCircularList(strOldCircId);
                //>>

                strAuditCommitteeToApprove = ((DropDownList)(fvCircularMaster.FindControl("ddlRequirementForTheBoard"))).SelectedValue;
                CheckBoxList cbToBePlacedBefore = ((CheckBoxList)(fvCircularMaster.FindControl("cbToBePlacedBefore")));
                strToBePlacedBefore = CommonCodes.getCommaSeparatedValuesFromCheckboxList(cbToBePlacedBefore);
                strDetails = cm.getSanitizedString(((F2FTextBox)(fvCircularMaster.FindControl("txtDetails"))).Text);
                strNameOfThePolicy = cm.getSanitizedString(((F2FTextBox)(fvCircularMaster.FindControl("txtNameOfThePolicy"))).Text);
                strFrequency = cm.getSanitizedString(((F2FTextBox)(fvCircularMaster.FindControl("txtFrequency"))).Text);

                if (strAuditCommitteeToApprove.Equals("N"))
                {
                    strToBePlacedBefore = "";
                    strDetails = "";
                    strNameOfThePolicy = "";
                    strFrequency = "";
                }

                if (strStatus.Equals("B"))
                    strBroadcastDate = Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy HH:mm:ss");

                intCircularId = CircularMasterBLL.SaveCircular(intCircularMasterId, intCircularAuthority, intArea, intDepartment,
                    strCircularNo, strDownloadRefNo, strTopic, strImplication, strIssueLink, strNameofCircular, strCircularDetails,
                    strCircularDate, strUser, intTypeofCircular, getCircularSegmentdt(), getCircularIntimationdt(),
                    getCircularAdditionalMailsdt(), getAttachmentdt(), null, strSpocFromComplianceFn, strSubDocumentType,
                    strAssociatedKeywords, strLinkageWithEarlierCircular, strSOCEOC, strOldCircSubNo, strOldCircId, strCircEffDate,
                    strAuditCommitteeToApprove, strToBePlacedBefore, strDetails, strNameOfThePolicy, strFrequency, strBroadcastDate,0);

                writeError("Circular Details Updated Successfully.");

                hfDoubleClickFlag.Value = "";
                hfSelectedRecord.Value = "";
                hfSelectedOperation.Value = "";
                bindCircularsGrid();
                mvMultiView.ActiveViewIndex = 0;
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        protected void lnkReset_Click(object sender, System.EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        #region for Old Circular Subject/No.
        private void reloadDataListViewState()
        {
            try
            {
                dtList.Columns.Add("Name");
                dtList.Columns.Add("CircularName");
                ViewState["Data"] = dtList;
                DataList dlUserList = (DataList)(fvCircularMaster.FindControl("dlUserList"));
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
                DataList dlUserList = (DataList)(fvCircularMaster.FindControl("dlUserList"));
                if (!strOldCircularIds.Equals(""))
                {
                    dtRecords = CircularMasterBLL.SearchCircular(Convert.ToInt32(hfSelectedRecord.Value), "", "", "", "", "", "", "",
                    "", "", "OldCircular", "", "", "", "", strCircularIds: strOldCircularIds).Tables[0];
                }
                if (dtRecords.Rows.Count > 0)
                {
                    dlUserList.DataSource = dtRecords;
                    dlUserList.DataBind();
                    ViewState["Data"] = dtRecords;
                }
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
                DataList dlUserList = (DataList)(fvCircularMaster.FindControl("dlUserList"));
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
            try
            {
                HiddenField hfOldCircularId = (HiddenField)(fvCircularMaster.FindControl("hfOldCircularId"));
                F2FTextBox txtOldCircSubjectNo = (F2FTextBox)(fvCircularMaster.FindControl("txtOldCircSubjectNo"));
                LinkButton btn = (LinkButton)sender;
                DataListItem dataitem = (DataListItem)btn.NamingContainer;

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
                HiddenField hfOldCircularId = (HiddenField)(fvCircularMaster.FindControl("hfOldCircularId"));
                F2FTextBox txtOldCircSubjectNo = (F2FTextBox)(fvCircularMaster.FindControl("txtOldCircSubjectNo"));
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

        #region for Additional Mail Ids TO
        private void reloadAdditionalMailToDataListViewState()
        {
            try
            {
                dtAdditionalMailTOList.Columns.Add("EmailId");
                ViewState["AdditionalMailTOData"] = dtAdditionalMailTOList;
                DataList dlAdditionalEmailsTO = (DataList)(fvCircularMaster.FindControl("dlAdditionalEmailsTO"));
                dlAdditionalEmailsTO.DataSource = dtAdditionalMailTOList;
                dlAdditionalEmailsTO.DataBind();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void loadExistingAdditionalMailToList(string strCircularId)
        {
            DataTable dtRecords = new DataTable();

            try
            {
                DataList dlAdditionalEmailsTO = (DataList)(fvCircularMaster.FindControl("dlAdditionalEmailsTO"));
                if (!strCircularId.Equals(""))
                {
                    dtRecords = circUtilBLL.GetDataTable("GetCircularAdditionalMails",
                        new DBUtilityParameter("CAM_MAIL_TYPE", "TO"), new DBUtilityParameter("CAM_CM_ID", strCircularId));
                }

                if (dtRecords.Rows.Count > 0)
                {
                    dlAdditionalEmailsTO.DataSource = dtRecords;
                    dlAdditionalEmailsTO.DataBind();
                    ViewState["AdditionalMailTOData"] = dtRecords;
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void SaveAndBindAdditionalMailToDatatable(DataTable dt)
        {
            try
            {
                DataList dlAdditionalEmailsTO = (DataList)(fvCircularMaster.FindControl("dlAdditionalEmailsTO"));
                ViewState["AdditionalMailTOData"] = dtAdditionalMailTOList;
                dlAdditionalEmailsTO.DataSource = dtAdditionalMailTOList;
                dlAdditionalEmailsTO.DataBind();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnRemoveAdditionalEmailsTo_Click(object sender, EventArgs e)
        {
            try
            {
                HiddenField hfAdditionalEmailsTO = (HiddenField)(fvCircularMaster.FindControl("hfAdditionalEmailsTO"));
                F2FTextBox txtAdditionalEmailsTO = (F2FTextBox)(fvCircularMaster.FindControl("txtAdditionalEmailsTO"));
                LinkButton btn = (LinkButton)sender;
                DataListItem dataitem = (DataListItem)btn.NamingContainer;

                int index = dataitem.ItemIndex;
                dtAdditionalMailTOList = (DataTable)ViewState["AdditionalMailTOData"];
                dtAdditionalMailTOList.Rows.RemoveAt(index);
                SaveAndBindAdditionalMailToDatatable(dtAdditionalMailTOList);
                txtAdditionalEmailsTO.Text = string.Empty;
                txtAdditionalEmailsTO.Focus();
                hfAdditionalEmailsTO.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void hfAdditionalEmailsTO_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                HiddenField hfAdditionalEmailsTO = (HiddenField)(fvCircularMaster.FindControl("hfAdditionalEmailsTO"));
                F2FTextBox txtAdditionalEmailsTO = (F2FTextBox)(fvCircularMaster.FindControl("txtAdditionalEmailsTO"));
                dtAdditionalMailTOList = (DataTable)ViewState["AdditionalMailTOData"];
                DataRow dr = dtAdditionalMailTOList.NewRow();
                dr["EmailId"] = txtAdditionalEmailsTO.Text;
                dtAdditionalMailTOList.Rows.Add(dr);
                SaveAndBindAdditionalMailToDatatable(dtAdditionalMailTOList);
                txtAdditionalEmailsTO.Text = "";
                hfAdditionalEmailsTO.Value = "";
                txtAdditionalEmailsTO.Focus();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        #endregion

        #region for Additional Mail Ids CC
        private void reloadAdditionalMailCCDataListViewState()
        {
            try
            {
                dtAdditionalMailCCList.Columns.Add("EmailId");
                ViewState["AdditionalMailCCData"] = dtAdditionalMailCCList;
                DataList dlAdditionalEmails = (DataList)(fvCircularMaster.FindControl("dlAdditionalEmails"));
                dlAdditionalEmails.DataSource = dtAdditionalMailCCList;
                dlAdditionalEmails.DataBind();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void loadExistingAdditionalMailCCList(string strCircularId)
        {
            DataTable dtRecords = new DataTable();

            try
            {
                DataList dlAdditionalEmails = (DataList)(fvCircularMaster.FindControl("dlAdditionalEmails"));
                if (!strCircularId.Equals(""))
                {
                    dtRecords = circUtilBLL.GetDataTable("GetCircularAdditionalMails",
                        new DBUtilityParameter("CAM_MAIL_TYPE", "CC"), new DBUtilityParameter("CAM_CM_ID", strCircularId));
                }

                if (dtRecords.Rows.Count > 0)
                {
                    dlAdditionalEmails.DataSource = dtRecords;
                    dlAdditionalEmails.DataBind();
                    ViewState["AdditionalMailCCData"] = dtRecords;
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void SaveAndBindAdditionalMailCCDatatable(DataTable dt)
        {
            try
            {
                DataList dlAdditionalEmails = (DataList)(fvCircularMaster.FindControl("dlAdditionalEmails"));
                ViewState["AdditionalMailCCData"] = dtAdditionalMailCCList;
                dlAdditionalEmails.DataSource = dtAdditionalMailCCList;
                dlAdditionalEmails.DataBind();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnRemoveAdditionalEmailsCC_Click(object sender, EventArgs e)
        {
            try
            {
                HiddenField hfAdditionalEmailsCC = (HiddenField)(fvCircularMaster.FindControl("hfAdditionalEmailsCC"));
                F2FTextBox txtAdditionalEmails = (F2FTextBox)(fvCircularMaster.FindControl("txtAdditionalEmails"));
                LinkButton btn = (LinkButton)sender;
                DataListItem dataitem = (DataListItem)btn.NamingContainer;

                int index = dataitem.ItemIndex;
                dtAdditionalMailCCList = (DataTable)ViewState["AdditionalMailCCData"];
                dtAdditionalMailCCList.Rows.RemoveAt(index);
                SaveAndBindAdditionalMailCCDatatable(dtAdditionalMailCCList);
                txtAdditionalEmails.Text = string.Empty;
                txtAdditionalEmails.Focus();
                hfAdditionalEmailsCC.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void hfAdditionalEmailsCC_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                HiddenField hfAdditionalEmailsCC = (HiddenField)(fvCircularMaster.FindControl("hfAdditionalEmailsCC"));
                F2FTextBox txtAdditionalEmails = (F2FTextBox)(fvCircularMaster.FindControl("txtAdditionalEmails"));
                dtAdditionalMailCCList = (DataTable)ViewState["AdditionalMailCCData"];
                DataRow dr = dtAdditionalMailCCList.NewRow();
                dr["EmailId"] = txtAdditionalEmails.Text;
                dtAdditionalMailCCList.Rows.Add(dr);
                SaveAndBindAdditionalMailCCDatatable(dtAdditionalMailCCList);
                txtAdditionalEmails.Text = "";
                hfAdditionalEmailsCC.Value = "";
                txtAdditionalEmails.Focus();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        #endregion

        private DataTable getAttachmentdt()
        {
            string strAttachment = hfAttachment.Value;
            string[] strarrAttachment, strarrFields;
            string strTemp;
            DataTable dtAttachment = new DataTable();
            DataRow dr;

            try
            {
                string strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["CircularFilesFolder"].ToString());

                dtAttachment.Columns.Add(new DataColumn("AttachId", typeof(string)));
                dtAttachment.Columns.Add(new DataColumn("ServerFileName", typeof(string)));
                dtAttachment.Columns.Add(new DataColumn("ClientFileName", typeof(string)));
                dtAttachment.Columns.Add(new DataColumn("ServerFilePath", typeof(string)));

                strarrAttachment = strAttachment.Split('~');
                for (int i = 0; i < strarrAttachment.Length - 1; i++)
                {
                    strTemp = strarrAttachment[i];
                    strarrFields = strTemp.Split('|');
                    dr = dtAttachment.NewRow();

                    dr["AttachId"] = strarrFields[0];
                    dr["ClientFileName"] = strarrFields[1];
                    dr["ServerFileName"] = strarrFields[2];
                    string strCompleteName = strServerDirectory + "\\" + strarrFields[2];
                    dr["ServerFilePath"] = strCompleteName;
                    dtAttachment.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return dtAttachment;
        }

        protected String getFileName(object inputFileName)
        {
            string strFileName = "";

            try
            {
                strFileName = inputFileName.ToString();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return strFileName.Replace("'", "\\'");
        }

        private void writeError(string strError)
        {
            lblInfo.Text = strError;
            lblInfo.Visible = true;
        }

        private void hideError()
        {
            lblInfo.Text = "";
            lblInfo.Visible = false;
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (Page.IsPostBack)
                {
                    DataSet dsFunction = new DataSet();

                    if (User.IsInRole("CircularAdmin"))
                    {
                        hfUserType.Value = "CircularAdmin";
                    }

                    if (!(Session["EditCircularSelectCommand"] == null))
                    {
                        gvCircularMaster.DataSource = (DataSet)Session["EditCircularSelectCommand"];
                        gvCircularMaster.DataBind();
                    }
                    
                    hideError();
                }
                else
                {
                    hfCurDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                    mvMultiView.ActiveViewIndex = 0;
                    ddlSearchTypeofCircular.DataSource = null;

                    CommonCodes.SetDropDownDataSource(ddlSearchTypeofCircular, circUtilBLL.GetDataTable("getTypeofCircular", sOrderBy: "CDTM_TYPE_OF_DOC"));
                    CommonCodes.SetDropDownDataSource(ddlSpocFromCompFnSearch, circUtilBLL.GetDataTable("getSpocFromComplianceFunction", new DBUtilityParameter("CCS_STATUS", "A"), sOrderBy: "CCS_NAME"));

                    Session["EditCircular"] = Server.UrlEncode(DateTime.Now.ToString());

                    if (User.IsInRole("CircularAdmin"))
                    {
                        Button1.Visible = true;
                    }
                    bindCircularsGrid();
                    CommonCodes.SetCheckboxDataSource(cbAssociatedKeywordsSearch, circUtilBLL.GetDataTable("AssociatedKeywords", new DBUtilityParameter("CKM_STATUS", "A"), sOrderBy: "CKM_NAME"));
                    CommonCodes.SetCheckboxDataSource(cbToBePlacedBeforeSearch, rcBL.getRefCodeDetails("Circulars - New Circular - To be placed before"));
                }
                fvCircularMaster.Visible = true;
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["EditCircular"] = Session["EditCircular"];
        }

        protected void cvTodate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        private void bindCircularsGrid()
        {
            try
            {
                DataSet dsCircular = new DataSet();
                string strAssociatedKeywords = "", strToBePlacedBefore = "", strGlobalSearch = "", strCircIdInCommaSeparatedFormat = "";
                string strIssuingAuthority = ddlSIssuingauthority.SelectedValue;
                string strSegment = null;
                string strDepartment = null;
                string strarea = ddlSArea.SelectedValue;
                string strCircularNo = null;
                string strDownloadRefNo = null;
                string strTopic = null;
                string FromDate = txtFromDate.Text;
                string ToDate = txtToDate.Text;
                string TypeOfDocument = ddlSearchTypeofCircular.SelectedValue;
                string strSpocFromCompliancefn = ddlSpocFromCompFnSearch.SelectedValue;
                string strActionableHaveBeenLoggedIn = ddlActionableHaveBeenLogged.SelectedValue;
                string strStatus = ddlStatus.SelectedValue;

                //<< Added by Amarjeet on 26-Jul-2021
                if (!txtGlobalSearch.Text.Equals(""))
                {
                    DataTable dtCircularFiles = CircularMasterBLL.SearchCircular(0, strIssuingAuthority,
                        strSegment, strDepartment, strarea, strCircularNo, strDownloadRefNo, strTopic, FromDate,
                        ToDate, "Edit", TypeOfDocument, strSpocFromCompliancefn, strActionableHaveBeenLoggedIn,
                        strAssociatedKeywords, strToBePlacedBefore, strGlobalSearch,
                        strCircIdInCommaSeparatedFormat, "Yes", strStatus).Tables[0];

                    string strFolderPath = Server.MapPath(ConfigurationManager.AppSettings["CircularFilesFolder"].ToString());

                    if (dtCircularFiles != null)
                    {
                        for (int i = 0; i < dtCircularFiles.Rows.Count; i++)
                        {
                            DataRow dr = dtCircularFiles.Rows[i];
                            strCircIdInCommaSeparatedFormat = (string.IsNullOrEmpty(strCircIdInCommaSeparatedFormat) ? "" : strCircIdInCommaSeparatedFormat + ",") + cm.searchPDFContent(dr["CM_ID"].ToString(), strFolderPath + "\\" + dr["CF_SERVERFILENAME"].ToString(), txtGlobalSearch.Text);
                        }
                    }
                }

                strGlobalSearch = cm.getSanitizedString(txtGlobalSearch.Text);

                strAssociatedKeywords = CommonCodes.getCommaSeparatedValuesFromCheckboxList(cbAssociatedKeywordsSearch);
                strToBePlacedBefore = CommonCodes.getCommaSeparatedValuesFromCheckboxList(cbToBePlacedBeforeSearch);
                //>>

                dsCircular = CircularMasterBLL.SearchCircular(0, strIssuingAuthority, strSegment, strDepartment,
                    strarea, strCircularNo, strDownloadRefNo, strTopic, FromDate, ToDate, "Edit", TypeOfDocument,
                    strSpocFromCompliancefn, strActionableHaveBeenLoggedIn, strAssociatedKeywords,
                    strToBePlacedBefore, strGlobalSearch, strCircIdInCommaSeparatedFormat, "", strStatus);

                gvCircularMaster.DataSource = dsCircular;
                Session["EditCircularSelectCommand"] = dsCircular;
                gvCircularMaster.DataBind();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnDeactivate_ServerClick(object sender, EventArgs e)
        {
            try
            {
                circUtilBLL.GetDataTable("deactivateCircular", new DBUtilityParameter("CM_ID", hfSelectedRecord.Value, oSecondValue: txtDeactivationRemarks.Text),
                        new DBUtilityParameter("1", "1", oSecondValue: (new Authentication().getUserFullName(Page.User.Identity.Name))));

                ClientScript.RegisterStartupScript(this.GetType(), "displayDeactivationSuccessMessage", "alert('Circular deactivated successfully.');", true);

                bindCircularsGrid();
                txtDeactivationRemarks.Text = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            try
            {
                bindCircularsGrid();

                if ((gvCircularMaster.Rows.Count == 0))
                {
                    lblInfo.Text = "No Records found satisfying the criteria.";
                    lblInfo.Visible = true;
                    btnExportToExcel.Visible = false;
                }
                else
                {
                    lblInfo.Text = String.Empty;
                    lblInfo.Visible = false;
                    btnExportToExcel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        public string replaceHTMLTagsFromLableTooltip(string strContent)
        {
            try
            {
                strContent = HttpUtility.HtmlDecode(strContent);
                strContent = strContent.Replace("</", "\n</");
                strContent = System.Text.RegularExpressions.Regex.Replace(strContent, "<.*?>", String.Empty);
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return strContent;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CommonCodes.CheckInputValidity(this))
                {
                    return;
                }
                if (Session["EditCircular"].ToString() == ViewState["EditCircular"].ToString())
                {
                    UpdateCircular();
                    ClearSession();
                    fvCircularMaster.Visible = false;
                    Session["EditCircular"] = Server.UrlEncode(DateTime.Now.ToString());
                }
                else
                {
                    mvMultiView.ActiveViewIndex = 0;
                    writeError("Your attempt to refresh the page was blocked as it would lead to duplication of data.");
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvMultiView.ActiveViewIndex = 0;
            hfSelectedOperation.Value = "";
            hfSelectedRecord.Value = "";
            ClearSession();
        }

        protected void gvCircularFileUpload_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FileInfo fileInfo;
                GridView gvFileUpload;
                gvFileUpload = ((GridView)(fvCircularMaster.FindControl("gvCircularFileUpload")));
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
                mdtCircularFileUpload.Rows.RemoveAt(gvFileUpload.SelectedIndex);
                gvFileUpload.DataSource = mdtCircularFileUpload;
                gvFileUpload.DataBind();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void gvCircularMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strCircularId, strSelectedAuth, strSubTypeofCircular, strSpocFromCompFn = "", strSelectedDept,
                strHtmlTableAttachmentRows = "", strAssociatedKeyword = "", strLinkageWithEarlierCircular = "", strSOCEOC = "",
                strOldCircSubNo = "", strOldCircId = "", strAuditCommitteeToApprove = "", strDetails = "", strNameOfThePolicy = "",
                strFrequency = "";
            string[] strarrAssociatedKeyword;
            DataRow dr;

            try
            {
                DataSet dsEditCircular = new DataSet();
                DataSet dsCircularDetails = new DataSet();
                Hashtable ParamMap = new Hashtable();
                int uniqueRowId = 0;

                strCircularId = gvCircularMaster.SelectedValue.ToString();
                hfSelectedRecord.Value = strCircularId;

                if (hfSelectedOperation.Value.Equals("Edit"))
                {
                    Response.Redirect("NewCircularMaster.aspx?CMId=" + en.Encrypt(hfSelectedRecord.Value), false);
                    return;
                    dsEditCircular = CircularMasterBLL.SearchCircular(Convert.ToInt32(hfSelectedRecord.Value), "", "", "",
                        "", "", "", "", "", "", "Edit", "", "", "", "");
                    ParamMap.Add("CMId", hfSelectedRecord.Value);
                    dsCircularDetails = CircularMasterBLL.getCircularDetails(ParamMap, mstrConnectionString);

                    fvCircularMaster.DataSource = dsEditCircular;
                    dr = dsEditCircular.Tables[0].Rows[0];
                    //hfBroadcastDate.Value = dr["CM_BROADCAST_DATE"].ToString();
                    strSelectedAuth = dr["CM_CIA_ID"].ToString();
                    strSelectedDept = dr["CM_CDM_ID"].ToString();
                    strSpocFromCompFn = dr["CM_CCS_ID"].ToString();
                    strSubTypeofCircular = dr["CM_CSDTM_ID"].ToString();
                    strAssociatedKeyword = dr["CM_ASSOCIATED_KEY"].ToString();
                    strLinkageWithEarlierCircular = (dr["CM_LINKAGE_WITH_EARLIER_CIRCULAR"] is DBNull ? "" : dr["CM_LINKAGE_WITH_EARLIER_CIRCULAR"].ToString());
                    strSOCEOC = (dr["CM_SOC_EOC"] is DBNull ? "" : dr["CM_SOC_EOC"].ToString());
                    strOldCircSubNo = (dr["CM_OLD_CIRC_SUB_NO"] is DBNull ? "" : dr["CM_OLD_CIRC_SUB_NO"].ToString());
                    strOldCircId = (dr["CM_BASE_ID"] is DBNull ? "" : dr["CM_BASE_ID"].ToString());
                    strAuditCommitteeToApprove = (dr["CM_AUDIT_COMMITTEE_TO_APPROVE"] is DBNull ? "" : dr["CM_AUDIT_COMMITTEE_TO_APPROVE"].ToString());
                    strDetails = (dr["CM_REMARKS"] is DBNull ? "" : dr["CM_REMARKS"].ToString());
                    strNameOfThePolicy = (dr["CM_NAME_OF_THE_POLICY"] is DBNull ? "" : dr["CM_NAME_OF_THE_POLICY"].ToString());
                    strFrequency = (dr["CM_FREQUENCY"] is DBNull ? "" : dr["CM_FREQUENCY"].ToString());
                    hfIsRegulatoryReportingAdded.Value = dr["IsRegulatoryReportingAdded"].ToString();

                    fvCircularMaster.DataBind();
                    this.lblInfo.Visible = false;
                    mvMultiView.ActiveViewIndex = 1;
                    fvCircularMaster.ChangeMode(FormViewMode.Edit);
                    DropDownList ddlDept = (DropDownList)(fvCircularMaster.FindControl("ddlDepartment"));
                    CheckBoxList cbSubmissions = (CheckBoxList)(fvCircularMaster.FindControl("cbSubmissions"));
                    CheckBoxList cbAssociatedKeywords = (CheckBoxList)(fvCircularMaster.FindControl("cbAssociatedKeywords"));
                    GridView gvFileUpload = (GridView)(fvCircularMaster.FindControl("gvFileUpload"));
                    DropDownList ddlSpocFromCompFn = (DropDownList)(fvCircularMaster.FindControl("ddlSpocFromCompFn"));
                    DropDownList ddlSubTypeofCircular = (DropDownList)(fvCircularMaster.FindControl("ddlSubTypeofCircular"));
                    //<< Added by Amarjeet on 04-Aug-2021
                    DropDownList ddlLinkageWithEarlierCircular = (DropDownList)(fvCircularMaster.FindControl("ddlLinkageWithEarlierCircular"));
                    DropDownList ddlSOCEOC = (DropDownList)(fvCircularMaster.FindControl("ddlSOCEOC"));
                    F2FTextBox txtOldCircSubjectNo = (F2FTextBox)(fvCircularMaster.FindControl("txtOldCircSubjectNo"));
                    HiddenField hfOldCircularId = (HiddenField)(fvCircularMaster.FindControl("hfOldCircularId"));
                    //>>
                    DropDownList ddlRequirementForTheBoard = (DropDownList)(fvCircularMaster.FindControl("ddlRequirementForTheBoard"));
                    CheckBoxList cbToBePlacedBefore = (CheckBoxList)(fvCircularMaster.FindControl("cbToBePlacedBefore"));
                    F2FTextBox txtDetails = (F2FTextBox)(fvCircularMaster.FindControl("txtDetails"));
                    F2FTextBox txtNameOfThePolicy = (F2FTextBox)(fvCircularMaster.FindControl("txtNameOfThePolicy"));
                    F2FTextBox txtFrequency = (F2FTextBox)(fvCircularMaster.FindControl("txtFrequency"));
                    CheckBoxList cbSendMailFor = (CheckBoxList)(fvCircularMaster.FindControl("cbSendMailFor"));

                    //ddlDept.DataSource = utilityBL.getDataset("DEPT", mstrConnectionString);
                    //ddlDept.DataBind();
                    //ddlDept.SelectedValue = strSelectedDept;
                    CommonCodes.SetDropDownDataSource(ddlDept, circUtilBLL.GetDataTable("DEPT", sOrderBy: "CDM_NAME"), sSelected: strSelectedDept);

                    //cbSubmissions.DataSource = utilityBL.getDataset("CIRCULARINTIMATIONS", mstrConnectionString);
                    //cbSubmissions.DataBind();
                    CommonCodes.SetCheckboxDataSource(cbSubmissions, circUtilBLL.GetDataTable("CIRCULARINTIMATIONS", sOrderBy: "CIM_TYPE"));
                    bindIntimationDetails(cbSubmissions, hfSelectedRecord.Value);

                    //ddlSpocFromCompFn.DataSource = utilityBL.getDatasetWithConditionInString("getSpocFromComplianceFunction", " AND CCS_STATUS = 'A'", mstrConnectionString);
                    //ddlSpocFromCompFn.DataBind();
                    //ddlSpocFromCompFn.SelectedValue = strSpocFromCompFn;
                    CommonCodes.SetDropDownDataSource(ddlSpocFromCompFn, circUtilBLL.GetDataTable("getSpocFromComplianceFunction", new DBUtilityParameter("CCS_STATUS", "A"), sOrderBy: "CCS_NAME"), sSelected: strSpocFromCompFn);

                    //ddlSubTypeofCircular.DataSource = utilityBL.getDatasetWithConditionInString("getSubTypeofCircular", "", mstrConnectionString);
                    //ddlSubTypeofCircular.Items.Add(li);
                    //ddlSubTypeofCircular.DataBind();
                    //ddlSubTypeofCircular.SelectedValue = strSubTypeofCircular;
                    CommonCodes.SetDropDownDataSource(ddlSubTypeofCircular, rcBL.getRefCodeDetails("Circular Document subtype"), sSelected: strSubTypeofCircular);

                    //<< Added by Amarjeet on 26-Jul-2021
                    //cbAssociatedKeywords.DataSource = utilityBL.getDataset("AssociatedKeywords", mstrConnectionString);
                    //cbAssociatedKeywords.DataBind();
                    strarrAssociatedKeyword = strAssociatedKeyword.Split(',');
                    //cbAssociatedKeywords = CommonCodes.getCheckboxSelectedValuesFromArray(strarrAssociatedKeyword, cbAssociatedKeywords);
                    CommonCodes.SetCheckboxDataSource(cbAssociatedKeywords, circUtilBLL.GetDataTable("AssociatedKeywords", new DBUtilityParameter("CKM_STATUS", "A"), sOrderBy: "CKM_NAME"), arrValues: strarrAssociatedKeyword);
                    //>>

                    //<< Added by Amarjeet on 04-Aug-2021
                    ddlLinkageWithEarlierCircular.Items.AddRange(CommonCodes.GetYesNoDDLItems());
                    ddlLinkageWithEarlierCircular.SelectedValue = strLinkageWithEarlierCircular;

                    //ddlSOCEOC.DataSource = rcBL.getRefCodeDetails("Circulars - New Circular - supersedes or extension/amendment to the old circular", mstrConnectionString);
                    //ddlSOCEOC.DataBind();
                    //ddlSOCEOC.Items.Insert(0, new ListItem("(Select)", ""));
                    //ddlSOCEOC.SelectedValue = strSOCEOC;
                    CommonCodes.SetDropDownDataSource(ddlSOCEOC, rcBL.getRefCodeDetails("Circulars - New Circular - supersedes or extension/amendment to the old circular"), sSelected: strSOCEOC);

                    //txtOldCircSubjectNo.Text = strOldCircSubNo;
                    //hfOldCircularId.Value = strOldCircId;

                    reloadDataListViewState();
                    loadExistingCircularList(strOldCircId);
                    //>>

                    ddlRequirementForTheBoard.Items.AddRange(CommonCodes.GetYesNoDDLItems());
                    ddlRequirementForTheBoard.SelectedValue = strAuditCommitteeToApprove;

                    //cbToBePlacedBefore.DataSource = rcBL.getRefCodeDetails("Circulars - New Circular - To be placed before", mstrConnectionString);
                    //cbToBePlacedBefore.DataBind();

                    //if (dr["CM_TO_BE_PLACED_BEFORE"] != DBNull.Value)
                    //{
                    //    cbToBePlacedBefore = CommonCodes.getCheckboxSelectedValuesFromArray(dr["CM_TO_BE_PLACED_BEFORE"].ToString().Split(','), cbToBePlacedBefore);
                    //}

                    CommonCodes.SetCheckboxDataSource(cbToBePlacedBefore, rcBL.getRefCodeDetails("Circulars - New Circular - To be placed before"), arrValues: (dr["CM_TO_BE_PLACED_BEFORE"] != DBNull.Value ? (dr["CM_TO_BE_PLACED_BEFORE"].ToString().Split(',')) : null));

                    txtDetails.Text = strDetails;
                    txtNameOfThePolicy.Text = strNameOfThePolicy;
                    txtFrequency.Text = strFrequency;

                    //<< TO
                    reloadAdditionalMailToDataListViewState();
                    loadExistingAdditionalMailToList(hfSelectedRecord.Value);
                    //>>

                    //<< CC
                    reloadAdditionalMailCCDataListViewState();
                    loadExistingAdditionalMailCCList(hfSelectedRecord.Value);
                    //>>

                    string strHtmlTable1 = "<table class=\"table table-bordered footable\" id='tblAttachment' width='100%'> " +
                                  " <thead> " +
                                  " <tr> " +
                                  " <th class='tabhead3'> " +
                                  " <input type='checkbox' ID='HeaderLevelCheckBoxAttachment' onclick = 'return onAttachmentHeaderRowChecked()'/> " +
                                  " </th>  " +
                                  " <th class='tabhead3' align='center'> " +
                                  " Attachment Name " +
                                  " </th> " +
                                  " </tr> " +
                                  " </thead> ";

                    DataTable dtAttachment = dsCircularDetails.Tables[1];
                    DataRow drAttachment;
                    int intdtAttachmentCnt = dtAttachment.Rows.Count;
                    for (int intCnt = 0; intCnt < intdtAttachmentCnt; intCnt++)
                    {
                        uniqueRowId = uniqueRowId + 1;
                        drAttachment = dtAttachment.Rows[intCnt];
                        strHtmlTableAttachmentRows = strHtmlTableAttachmentRows + "<tr><td class='tabbody3'>" +
                            "<input type='hidden' ID='uniqueRowId" + uniqueRowId + "' value='" + uniqueRowId + "' />" +
                            "<input type='hidden' ID='attachId" + uniqueRowId + "' value='" +
                            drAttachment["CF_ID"].ToString() + "' />" +
                            "<input type='checkbox' ID='checkAttachment" + uniqueRowId + "' />" +
                            "</td><td>" +
                            "<input type='hidden' ID='attachClientFileName" + uniqueRowId + "' value='" +
                            drAttachment["CF_FILENAME"].ToString() + "'/>" +
                            "<input type='hidden' ID='attachServerFileName" + uniqueRowId + "' value='" +
                            drAttachment["CF_SERVERFILENAME"].ToString() + "'/>" +
                            "<a id='attachfilelink" + uniqueRowId +
                            "'href='../CommonDownload.aspx?type=Circular&downloadFileName=" +
                            drAttachment["CF_SERVERFILENAME"].ToString() + "'>" +
                            drAttachment["CF_FILENAME"].ToString() + "</a>" +
                            "</td>" +
                            "</tr>";
                    }
                    Literal litAttachment = (Literal)(fvCircularMaster.FindControl("litAttachment"));
                    litAttachment.Text = strHtmlTable1 + strHtmlTableAttachmentRows + "</table>";

                    bindActionablesGrid();
                    bindCertChecklistsGrid();

                    CommonCodes.SetCheckboxDataSource(cbSendMailFor, rcBL.getRefCodeDetails("Circular - Send mail for"));
                }
                else if (hfSelectedOperation.Value.Equals("Delete"))
                {
                    //<< created by subodh on 5-Jul-2010 to delete using SP.
                    CircularMasterBLL.deleteCircular(Convert.ToInt32(hfSelectedRecord.Value), mstrConnectionString);
                    //>>

                    //added by prajakta salvi on 17-Jun-10
                    GridViewRow gvr = gvCircularMaster.SelectedRow;
                    FileInfo fileInfo;
                    string strFileName = "", strFilePath = "", strCompleteFileName = "";
                    DataList dlCircularFiles;
                    dlCircularFiles = ((DataList)(gvr.FindControl("dlCircularFiles")));
                    for (int i = 0; i < dlCircularFiles.Items.Count; i++)
                    {
                        strFileName = ((Label)(dlCircularFiles.Items[i].FindControl("lblServerFileName"))).Text;
                        strFilePath = Server.MapPath(ConfigurationManager.AppSettings["CircularFilesFolder"].ToString());
                        strCompleteFileName = (strFilePath + ("\\" + strFileName));
                        fileInfo = new FileInfo(strCompleteFileName);
                        if (fileInfo.Exists)
                        {
                            fileInfo.Delete();
                        }
                    }

                    bindCircularsGrid();
                    writeError("Record Deleted Successfully.");
                    //>>
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void bindActionablesGrid()
        {
            try
            {
                Hashtable ParamMap = new Hashtable();
                ParamMap.Add("CMId", string.IsNullOrEmpty(hfSelectedRecord.Value) ? "0" : hfSelectedRecord.Value);
                DataSet dsCircularDetails = CircularMasterBLL.getCircularDetails(ParamMap, mstrConnectionString);
                GridView gvActionables = (GridView)fvCircularMaster.FindControl("gvActionables");
                gvActionables.DataSource = dsCircularDetails.Tables[0];
                gvActionables.DataBind();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void bindCertChecklistsGrid()
        {
            int intCircularId = 0;
            bool res = int.TryParse(hfSelectedRecord.Value, out intCircularId);

            try
            {
                GridView gvCertChecklists = (GridView)fvCircularMaster.FindControl("gvCertChecklists");

                gvCertChecklists.DataSource = null;
                gvCertChecklists.DataBind();

                if (!intCircularId.Equals(0))
                {
                    gvCertChecklists.DataSource = CircularMasterBLL.SearchCircularCertChecklist(intCircularId, "");
                    gvCertChecklists.DataBind();
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void gvFileUpload_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FileInfo fileInfo;
            GridView gvFileUpload;
            GridViewRow gvrRow;

            try
            {
                gvFileUpload = ((GridView)(fvCircularMaster.FindControl("gvFileUpload")));
                string strFilePath;
                string strFileName;
                string strCompleteFileName;
                gvrRow = gvFileUpload.Rows[gvFileUpload.SelectedIndex];
                strFileName = ((Label)(gvrRow.FindControl("lblServerFileName"))).Text;
                strFilePath = Server.MapPath(ConfigurationManager.AppSettings["CircularFilesFolder"].ToString());
                strCompleteFileName = (strFilePath + ("\\" + strFileName));

                fileInfo = new FileInfo(strCompleteFileName);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
                int intValue = Convert.ToInt32(gvFileUpload.SelectedDataKey.Value);
                //utilityBL.getDatasetWithCondition("DELETECIRCULARFILES", intValue, mstrConnectionString);
                circUtilBLL.GetDataTable("DELETECIRCULARFILES", new DBUtilityParameter("CF_ID", intValue));
                //gvFileUpload.DataSource = utilityBL.getDatasetWithCondition("CIRCULARFILES", Convert.ToInt32(hfSelectedRecord.Value), mstrConnectionString);
                gvFileUpload.DataSource = circUtilBLL.GetDataTable("CIRCULARFILES", new DBUtilityParameter("CF_CM_ID", hfSelectedRecord.Value));
                gvFileUpload.DataBind();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void ClearSession()
        {
            mdtCircularFileUpload = null;
        }

        protected void bindIntimationDetails(CheckBoxList cbSubmissions, string CircularId)
        {
            DataTable dtIntimationName;
            string strName = null;
            try
            {
                //dtIntimationName = utilityBL.getDatasetWithCondition("CIRCULARINTIMATION", Convert.ToInt32(hfSelectedRecord.Value), mstrConnectionString);
                dtIntimationName = circUtilBLL.GetDataTable("CIRCULARINTIMATION", new DBUtilityParameter("CMI_CM_ID", hfSelectedRecord.Value));
                for (int i = 0; i <= dtIntimationName.Rows.Count - 1; i++)
                {
                    strName = dtIntimationName.Rows[i]["CMI_CIM_ID"].ToString();
                    cbSubmissions.Items.FindByValue(strName).Selected = true;
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected DataTable LoadCircularFileList(object CircularId)
        {
            DataTable dtCircularFiles = new DataTable();

            try
            {
                //dtCircularFiles = utilityBL.getDatasetWithCondition("CIRCULARFILES", Convert.ToInt32(CircularId), mstrConnectionString);
                dtCircularFiles = circUtilBLL.GetDataTable("CIRCULARFILES", new DBUtilityParameter("CF_CM_ID", CircularId));
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return dtCircularFiles;
        }

        protected void fvCircularMaster_DataBound(object sender, EventArgs e)
        {
            DataTable dtFiles = new DataTable();
            try
            {
                GridView gvUploadedFiles = (GridView)(fvCircularMaster.FindControl("gvFileUpload"));
                //dtFiles = utilityBL.getDatasetWithCondition("CIRCULARFILES", Convert.ToInt32(hfSelectedRecord.Value), mstrConnectionString);
                dtFiles = circUtilBLL.GetDataTable("CIRCULARFILES", new DBUtilityParameter("CF_CM_ID", hfSelectedRecord.Value));
                gvUploadedFiles.DataSource = dtFiles;
                gvUploadedFiles.DataBind();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void gvCircularMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCircularMaster.PageIndex = e.NewPageIndex;
            gvCircularMaster.DataSource = (DataSet)(Session["EditCircularSelectCommand"]);
            gvCircularMaster.DataBind();
        }

        private DataTable getCircularSegmentdt()
        {
            // CheckBoxList cblSegments = (CheckBoxList)(fvCircularMaster.FindControl("cblSegment"));
            DataTable dt = new DataTable();
            dt = new DataTable();

            dt.Columns.Add(new DataColumn("SegmentId", typeof(string)));
            //for (int i = 0; i <= cblSegments.Items.Count - 1; i++)
            //{

            //    liChkBoxListItem = cblSegments.Items[i];
            //    if (liChkBoxListItem.Selected)
            //    {
            //        dr = dt.NewRow();
            //        dr["SegmentId"] = liChkBoxListItem.Value;
            //        dt.Rows.Add(dr);

            //    }
            //}
            return dt;

        }

        private DataTable getCircularIntimationdt()
        {
            CheckBoxList cbSubmissions = (CheckBoxList)(fvCircularMaster.FindControl("cbSubmissions"));
            ListItem liChkBoxListItem;
            DataTable dt = new DataTable();
            DataRow dr;

            try
            {
                dt.Columns.Add(new DataColumn("IntimationId", typeof(string)));
                for (int i = 0; i <= cbSubmissions.Items.Count - 1; i++)
                {

                    liChkBoxListItem = cbSubmissions.Items[i];
                    if (liChkBoxListItem.Selected)
                    {
                        dr = dt.NewRow();
                        dr["IntimationId"] = liChkBoxListItem.Value;
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return dt;
        }

        #region
        protected void gvCircularMaster_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (Session["EditCircularSelectCommand"] != null)
            {

                DataSet ds = (DataSet)Session["EditCircularSelectCommand"];
                DataTable dt = ds.Tables[0];
                DataView dvDataView = new DataView(dt);
                string strSortExpression = "";

                //ViewState["_SortDirection_"]
                if (ViewState["_SortExpression_"] != null)
                {
                    strSortExpression = ViewState["_SortExpression_"].ToString();
                }

                if (ViewState["_SortDirection_"] == null)
                {
                    dvDataView.Sort = (e.SortExpression + (" " + "ASC"));
                    ViewState["_SortDirection_"] = "ASC";
                }
                else
                {
                    //ONLY IF THE USER HAS CLICKED ON THE SAME COLUMN AGAIN, SHOULD IT BE SORTED IN THE REVERSE ORDER.
                    //IF ANOTHER COLUMN IS SELECTED, IT SHOULD AGAIN BE SORTED IN ASCENDING ORDER. 
                    if (strSortExpression.Equals(e.SortExpression))
                    {
                        if (ViewState["_SortDirection_"].ToString().Equals("ASC"))
                        {
                            dvDataView.Sort = (e.SortExpression + (" " + "DESC"));
                            ViewState["_SortDirection_"] = "DESC";
                        }
                        else if (ViewState["_SortDirection_"].ToString().Equals("DESC"))
                        {
                            dvDataView.Sort = (e.SortExpression + (" " + "ASC"));
                            ViewState["_SortDirection_"] = "ASC";
                        }
                    }
                    else
                    {
                        dvDataView.Sort = (e.SortExpression + (" " + "ASC"));
                        ViewState["_SortDirection_"] = "ASC";
                    }


                }
                ViewState["_SortExpression_"] = e.SortExpression;
                gvCircularMaster.DataSource = dvDataView;
                gvCircularMaster.DataBind();
            }
        }

        protected void OnRowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex = GetSortColumnIndex();

                if (sortColumnIndex != -1)
                {
                    if (ViewState["_SortDirection_"] != null)
                        AddSortImage(e.Row, ViewState["_SortDirection_"].ToString(), sortColumnIndex);
                }
            }
        }

        private int GetSortColumnIndex()
        {
            // Iterate through the Columns collection to determine the index
            // of the column being sorted.

            string strSortExpression = "";

            if (ViewState["_SortExpression_"] != null)
                strSortExpression = ViewState["_SortExpression_"].ToString();

            if (!strSortExpression.Equals(""))
            {
                foreach (DataControlField field in gvCircularMaster.Columns)
                {
                    if (field.SortExpression.Equals(strSortExpression))
                    {
                        return gvCircularMaster.Columns.IndexOf(field);
                    }
                }
            }
            return -1;
        }

        void AddSortImage(GridViewRow headerRow, string strAction, int sortColumnIndex)
        {
            // Create the sorting image based on the sort direction.
            Image sortImage = new Image();
            if (strAction.Equals("ASC"))
            {
                sortImage.ImageUrl = "../../Content/images/legacy/view_sort_ascending.png";
                sortImage.AlternateText = "Ascending Order";
            }
            else if (strAction.Equals("DESC"))
            {
                sortImage.ImageUrl = "../../Content/images/legacy/view_sort_descending.png";
                sortImage.AlternateText = "Descending Order";
            }
            headerRow.Cells[sortColumnIndex].Controls.Add(sortImage);
        }
        #endregion

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            //gvCircularMaster.AllowPaging = false;
            //gvCircularMaster.AllowSorting = false;
            //gvCircularMaster.Columns[2].Visible = false;
            //gvCircularMaster.Columns[3].Visible = false;
            //gvCircularMaster.Columns[4].Visible = false;
            //gvCircularMaster.Columns[33].Visible = false;
            ////gvCircularMaster.DataSource = mdtActivityDetails;
            //gvCircularMaster.DataBind();
            //CommonCodes.PrepareGridViewForExport(gvCircularMaster);
            //string attachment = "attachment; filename=Checklist.xls";
            //Response.ClearContent();
            //Response.AddHeader("content-disposition", attachment);
            //Response.ContentType = "application/ms-excel";
            //StringWriter sw = new StringWriter();

            //HtmlTextWriter htw = new HtmlTextWriter(sw);

            //gvCircularMaster.RenderControl(htw);

            //Response.Write(sw.ToString());
            //Response.End();
            //gvCircularMaster.AllowPaging = true;
            //gvCircularMaster.AllowSorting = true;
            //gvCircularMaster.Columns[2].Visible = true;
            //gvCircularMaster.Columns[3].Visible = true;
            //gvCircularMaster.Columns[14].Visible = true;
            //gvCircularMaster.DataBind();

            gvCircularMaster.AllowPaging = false;
            gvCircularMaster.AllowSorting = false;
            gvCircularMaster.DataSource = (DataSet)(Session["EditCircularSelectCommand"]);
            gvCircularMaster.DataBind();

            F2FExcelExport.F2FExportGridViewToExcel(gvCircularMaster, "CircularsList", new int[] { 2, 3, 4, 5, 33 });
            return;
        }

        protected void btnSaveAndBroadCast_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            try
            {
                UpdateCircular("B");
                SendCircularBroadCast();
                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        //private void SendCircularBroadCast()
        private string SendCircularBroadCast()
        {
            MailContent_Circulars mail = new MailContent_Circulars();
            ListItem liChkBoxListItem;
            DataTable dtIntimationOwners = new DataTable();
            string strMailTo = "", strMailCC = "", strMsg = "";
            int intAttachmentCount = 0;

            Boolean isAdditionalMails = false, isIntimations = false;
            DataTable dtAttachment = getAttachmentdt();

            string strCirIssueType = "revised", strAttachments = "", strAttachmentNames = "", strCirImplications = null,
                strCirAttachment = null;

            try
            {
                //<< to update intimation details before broadcast mail sent
                DataTable dtIntimations = getCircularAdditionalMailsdt();
                CircularMasterBLL.insertCircularAdditionalMails((hfSelectedRecord.Value.Equals("") ? 0 : Convert.ToInt32(hfSelectedRecord.Value)),
                    dtIntimations, Authentication.GetUserID(Page.User.Identity.Name));
                //>>

                CheckBoxList cblIntimations = ((CheckBoxList)(fvCircularMaster.FindControl("cbSubmissions")));
                F2FTextBox FCKE_EditCircularDetails = (F2FTextBox)(fvCircularMaster.FindControl("FCKE_EditCircularDetails"));
                F2FTextBox FCKE_EditorImplications = (F2FTextBox)(fvCircularMaster.FindControl("FCKE_EditorImplications"));
                F2FTextBox txtTopic = (F2FTextBox)(fvCircularMaster.FindControl("txtTopic"));
                F2FTextBox txtCircularDate = (F2FTextBox)(fvCircularMaster.FindControl("txtCircularDate"));
                DropDownList ddlCircularAuthority = (DropDownList)(fvCircularMaster.FindControl("ddlCircularAuthority"));
                Label lblSendMailAuditTrail = (Label)(fvCircularMaster.FindControl("lblSendMailAuditTrail"));
                F2FTextBox txtReasonForBroadcast = (F2FTextBox)(fvCircularMaster.FindControl("txtReasonForBroadcast"));

                if (dtAttachment != null)
                {
                    if (dtAttachment.Rows.Count > 0)
                    {
                        string strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["CircularFilesFolder"].ToString());

                        foreach (DataRow dr in dtAttachment.Rows)
                        {
                            strAttachments = (string.IsNullOrEmpty(strAttachments) ? "" : strAttachments + ",") + strServerDirectory + "\\" + dr["ServerFileName"];
                            strAttachmentNames = (string.IsNullOrEmpty(strAttachmentNames) ? "" : strAttachmentNames + ",") + dr["ClientFileName"].ToString();
                        }

                        strCirAttachment = "Supporting documents are attached herewith.";
                        intAttachmentCount = dtAttachment.Rows.Count;
                    }
                }

                DataTable dtAdditionalMailTOData = (DataTable)ViewState["AdditionalMailTOData"];
                for (int intCount1 = 0; intCount1 < dtAdditionalMailTOData.Rows.Count; intCount1++)
                {
                    DataRow dr = dtAdditionalMailTOData.Rows[intCount1];
                    strMailTo = (string.IsNullOrEmpty(strMailTo) ? "" : strMailTo + ",") + dr["EmailId"].ToString();
                }

                DataTable dtAdditionalMailCCData = (DataTable)ViewState["AdditionalMailCCData"];
                for (int intCount1 = 0; intCount1 < dtAdditionalMailCCData.Rows.Count; intCount1++)
                {
                    DataRow dr = dtAdditionalMailCCData.Rows[intCount1];
                    strMailCC = (string.IsNullOrEmpty(strMailCC) ? "" : strMailCC + ",") + dr["EmailId"].ToString();

                    isAdditionalMails = true;
                }

                for (int i = 0; i <= cblIntimations.Items.Count - 1; i++)
                {
                    liChkBoxListItem = cblIntimations.Items[i];

                    if (liChkBoxListItem.Selected)
                    {
                        isIntimations = true;
                        //dtIntimationOwners = utilityBL.getDatasetWithCondition("CIRCINTIMATIONOWNERS", Convert.ToInt32(liChkBoxListItem.Value), mstrConnectionString);
                        dtIntimationOwners = circUtilBLL.GetDataTable("CIRCINTIMATIONOWNERS", new DBUtilityParameter("CIU_CIM_ID", liChkBoxListItem.Value));

                        foreach (DataRow dr in dtIntimationOwners.Rows)
                        {
                            strMailTo = (string.IsNullOrEmpty(strMailTo) ? "" : strMailTo + ",") + dr["CIU_EMAIL_ID"].ToString();
                        }
                    }
                }

                if ((isAdditionalMails) || (isIntimations))
                {
                    strCirImplications = string.IsNullOrEmpty(FCKE_EditorImplications.Text) ? "" : "Implications:<br />" + FCKE_EditorImplications.Text;

                    DataTable dtAcion = getActionableDataTable();
                    DataTable dtCertChecklist = getCertChecklistDataTable();

                    mail.ParamMap.Clear();
                    mail.ParamMap.Add("ConfigId", 28);
                    mail.ParamMap.Add("To", "ProvidedAsParam");
                    mail.ParamMap.Add("ToEmailIds", strMailTo);
                    mail.ParamMap.Add("cc", "CircularAdmin,CircularUser,ProvidedAsParam1");
                    mail.ParamMap.Add("CCEmailIds", strMailCC);
                    mail.ParamMap.Add("CirIssueType", strCirIssueType);
                    mail.ParamMap.Add("CirSubject", txtTopic.Text);
                    mail.ParamMap.Add("CirIssAuthority", ddlCircularAuthority.SelectedItem.Text);
                    mail.ParamMap.Add("CirDate", txtCircularDate.Text);
                    mail.ParamMap.Add("CirSummary", FCKE_EditCircularDetails.Text);
                    mail.ParamMap.Add("CirImplications", strCirImplications);
                    mail.ParamMap.Add("CirAttachment", strCirAttachment);
                    mail.ParamMap.Add("AttachmentCount", intAttachmentCount);
                    mail.ParamMap.Add("Attachments", strAttachments);
                    mail.ParamMap.Add("AttachmentNames", strAttachmentNames);
                    mail.setCircularMailContent(dtAcion, dtCertChecklist);

                    //writeError("Circular broadcasted successfully.");
                    strMsg = "Circular broadcasted successfully.";

                    //<< Update Broadcast date and audit trail
                    circUtilBLL.GetDataTable("updateBroadcastDate", new DBUtilityParameter("CM_ID", hfSelectedRecord.Value));
                    circUtilBLL.GetDataTable("updateBroadcastAuditTrail", new DBUtilityParameter("CM_ID", hfSelectedRecord.Value),
                        new DBUtilityParameter("1", "1", oSecondValue: (new Authentication().getUserFullName(Page.User.Identity.Name))),
                        new DBUtilityParameter("1", "1", oSecondValue: txtReasonForBroadcast.Text));
                    DataSet dsEditCircular = CircularMasterBLL.SearchCircular(Convert.ToInt32(hfSelectedRecord.Value), "", "", "",
                        "", "", "", "", "", "", "Edit", "", "", "", "");
                    lblSendMailAuditTrail.Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(dsEditCircular.Tables[0].Rows[0]["CM_AUDIT_TRAIL"].ToString().Replace("\r\n", "<br>")));
                    //>>
                }

                if (!isIntimations)
                    strMsg = "Please select \"Intimated To\" to broadcast the circular.";

                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                strMsg = "Exception in broadcast: ";
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
                strMsg += sMessage;
            }

            return strMsg;
        }

        public void sendCircularActionableMail(DataTable dtActionable, string strSpocFromComplianceFnId)
        {
            DataTable dtSpocFromcompliance = new DataTable();
            string strActionableIds = "", strMailTo = "", strMailCC = "";
            MailContent_Circulars mail = new MailContent_Circulars();

            try
            {
                DropDownList ddlCircularAuthority = (DropDownList)(fvCircularMaster.FindControl("ddlCircularAuthority"));
                F2FTextBox txtTopic = (F2FTextBox)(fvCircularMaster.FindControl("txtTopic"));
                F2FTextBox txtCircularDate = (F2FTextBox)(fvCircularMaster.FindControl("txtCircularDate"));
                DropDownList ddlTypeofCircular = (DropDownList)fvCircularMaster.FindControl("ddlTypeofCircular");

                if (dtActionable.Rows.Count > 0)
                {
                    for (int intFilter = 0; intFilter < dtActionable.Rows.Count; intFilter++)
                    {
                        DataRow drFilter;
                        drFilter = dtActionable.Rows[intFilter];

                        strActionableIds = (string.IsNullOrEmpty(strActionableIds) ? "" : strActionableIds + ",") + drFilter["Id"].ToString();
                        strMailTo = (string.IsNullOrEmpty(strMailTo) ? "" : strMailTo + ",") + drFilter["PerRespEmailId"].ToString();
                        strMailCC = (string.IsNullOrEmpty(strMailCC) ? "" : strMailCC + ",") + drFilter["ReportMgrEmailId"].ToString();
                    }

                    dtSpocFromcompliance = circUtilBLL.GetDataTable("getSpocFromComplianceFunction",
                        new DBUtilityParameter("CCS_STATUS", "A"), new DBUtilityParameter("CCS_ID", strSpocFromComplianceFnId),
                        sOrderBy: "CCS_NAME");

                    foreach (DataRow dr in dtSpocFromcompliance.Rows)
                    {
                        strMailCC = (string.IsNullOrEmpty(strMailCC) ? "" : strMailCC + ",") + dr["CCS_EMAIL_ID"].ToString();
                    }

                    mail.ParamMap.Add("ConfigId", 29);
                    mail.ParamMap.Add("To", "ProvidedAsParam");
                    mail.ParamMap.Add("ToEmailIds", strMailTo);
                    mail.ParamMap.Add("cc", "CircularAdmin,CircularUser,ProvidedAsParam1");
                    mail.ParamMap.Add("CCEmailIds", strMailCC);
                    mail.ParamMap.Add("CirType", ddlTypeofCircular.SelectedItem.Text);
                    mail.ParamMap.Add("CirSubject", txtTopic.Text);
                    mail.ParamMap.Add("CirIssAuthority", ddlCircularAuthority.SelectedItem.Text);
                    mail.ParamMap.Add("CirDate", txtCircularDate.Text);
                    mail.setCircularMailContent(dtActionable);

                    //<< Update IsMailSent "Yes" for the actionables mail sent where IsMailSent is "No"
                    circUtilBLL.GetDataTable("updateIsMailSentForActionables",
                            new DBUtilityParameter("CA_ID", strActionableIds, "IN", oSubQuery: 1),
                            new DBUtilityParameter("CA_IS_MAIL_SENT", "N"));
                    //>>
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        public void sendCircularCertChecklistMail(DataTable dtCircCertChecklist)
        {
            try
            {
                MailContent_Circulars mail = new MailContent_Circulars();

                DropDownList ddlTypeofCircular = (DropDownList)fvCircularMaster.FindControl("ddlTypeofCircular");

                DataTable dtDistinctData = new DataView(dtCircCertChecklist).ToTable(true, "DeptName");

                if (dtCircCertChecklist.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDistinctData.Rows.Count; i++)
                    {
                        string strDeptName = "", strMailTo = "", strCCCId = "";
                        DataRow dr = dtDistinctData.Rows[i];
                        strDeptName = dr["DeptName"].ToString();

                        DataView dvData = new DataView(dtCircCertChecklist);
                        dvData.RowFilter = "DeptName = '" + strDeptName + "'";

                        DataTable dtFilteredData = dvData.ToTable();

                        strMailTo = (string.IsNullOrEmpty(strMailTo) ? "" : strMailTo + ",") + dtFilteredData.Rows[0]["FHEmailId"].ToString();
                        strMailTo = (string.IsNullOrEmpty(strMailTo) ? "" : strMailTo + ",") + dtFilteredData.Rows[0]["UHEmailId"].ToString();
                        strMailTo = (string.IsNullOrEmpty(strMailTo) ? "" : strMailTo + ",") + dtFilteredData.Rows[0]["SPOCEmailId"].ToString();

                        for (int j = 0; j < dtFilteredData.Rows.Count; j++)
                        {
                            DataRow drFilter = dtFilteredData.Rows[j];
                            strCCCId = (string.IsNullOrEmpty(strCCCId) ? "" : strCCCId + ",") + drFilter["Id"].ToString();
                        }

                        mail.ParamMap.Clear();
                        mail.ParamMap.Add("ConfigId", 1085);
                        mail.ParamMap.Add("To", "ProvidedAsParam"); // Certification_Function_Head, Certification_Unit_Head, and Certification_Coordinator (SPOC)
                        mail.ParamMap.Add("ToEmailIds", strMailTo);
                        mail.ParamMap.Add("cc", "CircularAdmin,CircularUser"); // CirularUser, and CircularAdmin
                        mail.ParamMap.Add("CirType", ddlTypeofCircular.SelectedItem.Text);
                        mail.ParamMap.Add("CirSubject", hfTopic.Value);
                        mail.ParamMap.Add("CirIssAuthority", hfCircularAuthority.Value);
                        mail.ParamMap.Add("CirDate", hfCircularDate.Value);
                        mail.setCircularMailContent(dtFilteredData);

                        //<< Update IsMailSent "Yes" for the checkpoints mail sent where IsMailSent is "No"
                        circUtilBLL.GetDataTable("updateIsMailSentForCertChecklists",
                                new DBUtilityParameter("CCC_ID", strCCCId, "IN", oSubQuery: 1),
                                new DBUtilityParameter("CCC_IS_MAIL_SENT", "N"));
                        //>>
                    }
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void BtnSaveAddActionables_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateCircular();
                string script = "window.open('AddCircularActionable.aspx?CirId=" + hfSelectedRecord.Value + "','_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "AddActionable", script, true);
                GridView gvCircularMaster = (GridView)fvCircularMaster.FindControl("gvCircularMaster");
                gvCircularMaster_SelectedIndexChanged(gvCircularMaster, new EventArgs());
                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void BtnSaveAddCertChecklist_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateCircular();
                string script = "window.open('AddCircularCertChecklists.aspx?CirId=" + hfSelectedRecord.Value + "','_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "AddCertChecklist", script, true);
                GridView gvCircularMaster = (GridView)fvCircularMaster.FindControl("gvCircularMaster");
                gvCircularMaster_SelectedIndexChanged(gvCircularMaster, new EventArgs());
                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        //<< Added by Amarjeet on 27-Jul-2021
        protected void btnUploadChecklist_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateCircular();
                string script = "window.open('../Certification/UploadChecklistData.aspx?Type=CIRC&CircId=" + hfSelectedRecord.Value + "','_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "UploadCertChecklist", script, true);
                GridView gvCircularMaster = (GridView)fvCircularMaster.FindControl("gvCircularMaster");
                gvCircularMaster_SelectedIndexChanged(gvCircularMaster, new EventArgs());
                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        //>>

        protected DataTable getActionableDataTable()
        {
            DataTable dtAcion = new DataTable();

            try
            {
                dtAcion.Columns.Add(new DataColumn("Id", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("Actionable", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("ResFuncName", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("PerResp", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("PerRespUserId", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("PerRespUserName", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("PerRespEmailId", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("ReportMgr", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("ReportMgrId", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("ReportMgrUserName", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("ReportMgrEmailId", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("TargetDate", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("ComplDate", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("Status", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("StatusName", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("CommType", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("Function", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("Remarks", typeof(string)));
                dtAcion.Columns.Add(new DataColumn("IsMailSent", typeof(string)));

                GridView gvActionables = (GridView)fvCircularMaster.FindControl("gvActionables");
                Hashtable ParamMap = new Hashtable();
                ParamMap.Add("CMId", hfSelectedRecord.Value);
                DataSet dsCircularDetails = CircularMasterBLL.getCircularDetails(ParamMap, mstrConnectionString);
                DataTable dtActionables = dsCircularDetails.Tables[0];

                foreach (DataRow drAct in dtActionables.Rows)
                {
                    DataRow dr = dtAcion.NewRow();
                    dr["Id"] = drAct["CA_ID"].ToString();
                    dr["Actionable"] = drAct["CA_ACTIONABLE"].ToString();
                    dr["ResFuncName"] = drAct["CFM_NAME"].ToString();
                    dr["PerResp"] = drAct["CA_PERSON_RESPONSIBLE"].ToString();
                    dr["PerRespUserId"] = drAct["CA_PERSON_RESPONSIBLE_ID"].ToString();
                    dr["PerRespUserName"] = drAct["CA_PERSON_RESPONSIBLE_NAME"].ToString();
                    dr["PerRespEmailId"] = drAct["CA_PERSON_RESPONSIBLE_EMAIL_ID"].ToString();
                    dr["TargetDate"] = drAct["CA_TARGET_DATE"].ToString();
                    dr["ComplDate"] = drAct["CA_COMPLETION_DATE"].ToString();
                    dr["Status"] = drAct["RC_NAME"].ToString();
                    dr["StatusName"] = drAct["RC_NAME"].ToString();
                    dr["CommType"] = null;
                    dr["Function"] = null;
                    dr["Remarks"] = drAct["CA_REMARKS"].ToString();
                    dr["ReportMgr"] = drAct["CA_REPORTING_MANAGER"].ToString();
                    dr["ReportMgrId"] = drAct["CA_Reporting_Mgr_ID"].ToString();
                    dr["ReportMgrUserName"] = drAct["CA_Reporting_Mgr_Name"].ToString();
                    dr["ReportMgrEmailId"] = drAct["CA_Reporting_Mgr_EMAIL_ID"].ToString();
                    dr["IsMailSent"] = (drAct["CA_IS_MAIL_SENT"] is DBNull ? "N" : drAct["CA_IS_MAIL_SENT"].ToString());
                    dtAcion.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return dtAcion;
        }

        protected DataTable getCertChecklistDataTable()
        {
            DataTable dtChklist = new DataTable();

            try
            {
                dtChklist.Columns.Add(new DataColumn("Id", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("DeptId", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("DeptName", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("SPOCEmailId", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("UHEmailId", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("FHEmailId", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("ActRegCirc", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("Reference", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("Clause", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("CheckPoints", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("Particulars", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("Penalty", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("Frequency", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("Forms", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("EffectiveFrom", typeof(string)));
                dtChklist.Columns.Add(new DataColumn("IsMailSent", typeof(string)));

                int intCircularId = 0;
                bool res = int.TryParse(hfSelectedRecord.Value, out intCircularId);

                DataTable dtCertChecklist = null;

                if (!intCircularId.Equals(0))
                    dtCertChecklist = CircularMasterBLL.SearchCircularCertChecklist(intCircularId, "");

                if (dtCertChecklist != null)
                {
                    foreach (DataRow drCertChecklist in dtCertChecklist.Rows)
                    {
                        DataRow dr = dtChklist.NewRow();

                        dr["Id"] = drCertChecklist["CCC_ID"].ToString();
                        dr["DeptId"] = drCertChecklist["CCC_DEPT_ID"].ToString();
                        dr["DeptName"] = drCertChecklist["DeptName"].ToString();
                        dr["SPOCEmailId"] = drCertChecklist["CSSDM_EMAIL_ID"].ToString();
                        dr["UHEmailId"] = drCertChecklist["CSDM_EMAIL_ID"].ToString();
                        dr["FHEmailId"] = drCertChecklist["CDM_CXO_EMAILID"].ToString();
                        dr["ActRegCirc"] = drCertChecklist["CDTM_TYPE_OF_DOC"].ToString();
                        dr["Reference"] = drCertChecklist["CCC_REFERENCE"].ToString();
                        dr["Clause"] = drCertChecklist["CCC_CLAUSE"].ToString();
                        dr["CheckPoints"] = drCertChecklist["CCC_CHECK_POINTS"].ToString();
                        dr["Particulars"] = drCertChecklist["CCC_PARTICULARS"].ToString();
                        dr["Penalty"] = drCertChecklist["CCC_PENALTY"].ToString();
                        dr["Frequency"] = drCertChecklist["CCC_FREQUENCY"].ToString();
                        dr["Forms"] = drCertChecklist["CCC_FORMS"].ToString();
                        dr["EffectiveFrom"] = (drCertChecklist["CCC_EFFECTIVE_FROM"] is DBNull ? "" : Convert.ToDateTime(drCertChecklist["CCC_EFFECTIVE_FROM"]).ToString("dd-MMM-yyyy"));
                        dr["IsMailSent"] = (drCertChecklist["CCC_IS_MAIL_SENT"] is DBNull ? "N" : drCertChecklist["CCC_IS_MAIL_SENT"].ToString());

                        dtChklist.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return dtChklist;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            GridView gvCircularMaster = (GridView)fvCircularMaster.FindControl("gvCircularMaster");
            gvCircularMaster_SelectedIndexChanged(gvCircularMaster, new EventArgs());
        }

        protected void BtnSendActionableMails_Click(object sender, EventArgs e)
        {
            try
            {
                sendActionableMails();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnSendNewActionablesMail_Click(object sender, EventArgs e)
        {
            try
            {
                sendActionableMails("NEW");
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private string sendActionableMails(string strNewAll = "")
        {
            string strMsg = "";

            try
            {
                DataTable dtAcion = getActionableDataTable();

                if (strNewAll.Equals("NEW"))
                {
                    DataView dvData = new DataView(dtAcion);
                    dvData.RowFilter = "IsMailSent = 'N'";
                    dtAcion = dvData.ToTable();
                }

                DropDownList ddlSpocFromCompFn = (DropDownList)fvCircularMaster.FindControl("ddlSpocFromCompFn");
                string intSpocFromComplianceFn = ddlSpocFromCompFn.SelectedValue.ToString();

                if (dtAcion != null && dtAcion.Rows.Count != 0)
                {
                    sendCircularActionableMail(dtAcion, intSpocFromComplianceFn);

                    //<< Update audit trail
                    if (strNewAll.Equals("NEW"))
                    {
                        strMsg = "Mail sent for newly added actionables.";

                        circUtilBLL.GetDataTable("updateNewActionableAuditTrail", new DBUtilityParameter("CM_ID", hfSelectedRecord.Value),
                        new DBUtilityParameter("1", "1", oSecondValue: (new Authentication().getUserFullName(Page.User.Identity.Name))));
                    }
                    else
                    {
                        strMsg = "Mail sent for all the actionables.";

                        circUtilBLL.GetDataTable("updateAllActionableAuditTrail", new DBUtilityParameter("CM_ID", hfSelectedRecord.Value),
                        new DBUtilityParameter("1", "1", oSecondValue: (new Authentication().getUserFullName(Page.User.Identity.Name))));
                    }
                    //>>
                }
                else
                {
                    if (strNewAll.Equals("NEW"))
                        strMsg = "Please add new actionables to send mail.";
                    else
                        strMsg = "Please add actionables to send mail.";
                }

                //writeError(strMsg);
                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return strMsg;
        }

        protected void BtnSendCircCertChecklistMails_Click(object sender, EventArgs e)
        {
            try
            {
                //UpdateCircular();
                sendChecklistMails();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void BtnSendNewCircCertChecklistMails_Click(object sender, EventArgs e)
        {
            try
            {
                sendChecklistMails("NEW");
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        //private void sendChecklistMails(string strNewAll = "")
        private string sendChecklistMails(string strNewAll = "")
        {
            string strMsg = "";

            try
            {
                DataTable dtChklist = getCertChecklistDataTable();

                if (strNewAll.Equals("NEW"))
                {
                    DataView dvData = new DataView(dtChklist);
                    dvData.RowFilter = "IsMailSent = 'N'";
                    dtChklist = dvData.ToTable();
                }

                if (dtChklist != null && dtChklist.Rows.Count != 0)
                {
                    sendCircularCertChecklistMail(dtChklist);

                    //<< Update audit trail
                    if (strNewAll.Equals("NEW"))
                    {
                        strMsg = "Mail sent for newly added checkpoints.";

                        circUtilBLL.GetDataTable("updateNewCertChecklistAuditTrail", new DBUtilityParameter("CM_ID", hfSelectedRecord.Value),
                        new DBUtilityParameter("1", "1", oSecondValue: (new Authentication().getUserFullName(Page.User.Identity.Name))));
                    }
                    else
                    {
                        strMsg = "Mail sent for all the checkpoints.";

                        circUtilBLL.GetDataTable("updateAllCertChecklistAuditTrail", new DBUtilityParameter("CM_ID", hfSelectedRecord.Value),
                        new DBUtilityParameter("1", "1", oSecondValue: (new Authentication().getUserFullName(Page.User.Identity.Name))));
                    }
                    //>>
                }
                else
                {
                    if (strNewAll.Equals("NEW"))
                        strMsg = "Please add new checkpoints to send mail.";
                    else
                        strMsg = "Please add checkpoints to send mail.";
                }

                //writeError(strMsg);
                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return strMsg;
        }

        private DataTable getCircularAdditionalMailsdt()
        {
            DataTable dt = new DataTable();
            DataRow dr;

            try
            {
                dt.Columns.Add(new DataColumn("MailType", typeof(string)));
                dt.Columns.Add(new DataColumn("AdditionalMailId", typeof(string)));

                //<< TO
                DataTable dtAdditionalMailTOData = (DataTable)ViewState["AdditionalMailTOData"];

                for (int i = 0; i < dtAdditionalMailTOData.Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["MailType"] = "TO";
                    dr["AdditionalMailId"] = dtAdditionalMailTOData.Rows[i]["EmailId"].ToString();
                    dt.Rows.Add(dr);
                }
                //>>

                //<< CC
                DataTable dtAdditionalMailCCData = (DataTable)ViewState["AdditionalMailCCData"];

                for (int i = 0; i < dtAdditionalMailCCData.Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["MailType"] = "CC";
                    dr["AdditionalMailId"] = dtAdditionalMailCCData.Rows[i]["EmailId"].ToString();
                    dt.Rows.Add(dr);
                }
                //>>
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return dt;
        }

        protected void gvCircularMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if ((e.Row.RowType == DataControlRowType.DataRow))
                {
                    Label lblCircularId = (Label)(e.Row.FindControl("lblCircularId"));
                    HiddenField hfStatus = (HiddenField)(e.Row.FindControl("hfStatus"));
                    LinkButton lbDeactivate = (LinkButton)(e.Row.FindControl("lbDeactivate"));
                    LinkButton lbEdit = (LinkButton)(e.Row.FindControl("lbEdit"));
                    LinkButton lbDelete = (LinkButton)(e.Row.FindControl("lbDelete"));
                    //<< Added by ramesh more on 14-Mar-2024 CR_1991
                    LinkButton lnkViewCircular = (LinkButton)(e.Row.FindControl("lnkView"));

                    lnkViewCircular.OnClientClick = "return onViewDetailClick('" + en.Encrypt(lblCircularId.Text) + "');";
                    //>>
                    if (hfStatus.Value.Equals("I"))
                    {
                        lbEdit.Visible = false;
                        lbDelete.Visible = false;
                        lbDeactivate.Visible = false;
                    }
                    else
                    {
                        lbEdit.Visible = true;
                        lbDelete.Visible = true;
                        lbDeactivate.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnAddRegReporting_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateCircular();
                string script = "window.open('../Submissions/CommonSubmission.aspx?CircId=" + hfSelectedRecord.Value + "&Type=CIRC','_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "AddRegReporting", script, true);
                GridView gvCircularMaster = (GridView)fvCircularMaster.FindControl("gvCircularMaster");
                gvCircularMaster_SelectedIndexChanged(gvCircularMaster, new EventArgs());
                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void lnkListOfReports_Click(object sender, EventArgs e)
        {
            try
            {
                string script = "window.open('../Submissions/ListOfReports.aspx?Type=CIRC&CircId=" + hfSelectedRecord.Value + "','_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "ViewRegReporting", script, true);
                GridView gvCircularMaster = (GridView)fvCircularMaster.FindControl("gvCircularMaster");
                gvCircularMaster_SelectedIndexChanged(gvCircularMaster, new EventArgs());
                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnSubmit_ServerClick(object sender, EventArgs e)
        {
            string strMsg = "";
            try
            {
                CheckBoxList cbSendMailFor = ((CheckBoxList)(fvCircularMaster.FindControl("cbSendMailFor")));
                F2FTextBox txtReasonForBroadcast = ((F2FTextBox)(fvCircularMaster.FindControl("txtReasonForBroadcast")));

                foreach (ListItem listItem in cbSendMailFor.Items)
                {
                    if (listItem.Selected)
                    {
                        if (listItem.Value.Equals("B"))
                            strMsg += SendCircularBroadCast() + "\\r\\n";
                        else if (listItem.Value.Equals("NCC"))
                            strMsg += sendChecklistMails("NEW") + "\\r\\n";
                        else if (listItem.Value.Equals("ACC"))
                            strMsg += sendChecklistMails() + "\\r\\n";
                        else if (listItem.Value.Equals("NA"))
                            strMsg += sendActionableMails("NEW") + "\\r\\n";
                        else if (listItem.Value.Equals("AA"))
                            strMsg += sendActionableMails() + "\\r\\n";
                    }
                }

                if (!strMsg.Equals(""))
                {
                    string script = "alert('" + strMsg + "');";
                    ClientScript.RegisterStartupScript(this.GetType(), "displayMailMessage", script, true);
                }

                cbSendMailFor.ClearSelection();
                txtReasonForBroadcast.Text = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        public static string IsValidUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute) == true)
            {
                return url;
            }
            else
            {//onclick ='return false; alert('Invalid link!');'
                return "#";
            }
        }

    }
}