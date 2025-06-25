using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Fiction2Fact.Legacy_App_Code.Circulars.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using System.Web.Services;
using System.Collections.Generic;
using System.Web.Helpers;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.Circulars;
using System.Text;

namespace Fiction2Fact.Projects.Circulars
{
    public partial class NewCircularMaster : System.Web.UI.Page
    {
        private DataTable mdtCircularFileUpload;
        CommonMethods cm = new CommonMethods();
        string mstrConnectionString = null;
        CircularMasterBLL CircularMasterBLL = new CircularMasterBLL();
        RefCodesBLL rcBL = new RefCodesBLL();
        CircUtilitiesBLL UtilitiesBLL = new CircUtilitiesBLL();

        string mstrIssuer, mstrIssuerlink, mstrSubject, mstrCircularDate, mstrCircularDetails, mstrImplication;

        DataTable dtActionables = new DataTable();
        DataTable dtCertChecklist = new DataTable();
        DataTable dtList = new DataTable();
        DataTable dtAdditionalMailTOList = new DataTable();
        DataTable dtAdditionalMailCCList = new DataTable();
        SHA256EncryptionDecryption en = new SHA256EncryptionDecryption();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    txtCircularNo.Attributes["onchange"] = "checkDuplicateCircularNo();test();";
                    txtCircularNo.Attributes["onblur"] = "checkDuplicateCircularNo();test();";
                    txtCircularNo.Attributes["onkeyup"] = "checkDuplicateCircularNo();test();";

                    txtOldCircSubjectNo.Attributes.Add("readonly", "readonly");
                    hfCurDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");

                    if (!string.IsNullOrEmpty(Request.QueryString["CMId"]))
                    {
                        hfSelectedRecord.Value = en.Decrypt(Request.QueryString["CMId"]);
                        BindDrodownCheckboxandDatatableForEdit();
                        bindCircularDetails();
                    }
                    else
                    {
                        BindDrodownCheckboxandDatatable();
                    }

                }
                else
                {
                    if (!(Session["CircularFileUploadDT"] == null))
                    {
                        mdtCircularFileUpload = (DataTable)Session["CircularFileUploadDT"];
                    }
                }

                bindActionablesGrid();
                bindCertChecklistsGrid();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        //SetDropDownDataSourceForEdit
        private void BindDrodownCheckboxandDatatable()
        {
            //ddlLOB.Items.Clear();
            //CommonCodes.SetDropDownDataSource(ddlLOB, UtilitiesBLL.GetDataTable("getLOBList", new DBUtilityParameter("LEM_STATUS", "A"), sOrderBy: "LEM_NAME"));
            //if (ddlLOB.Items.Count <= 2)
            //{
            //    ddlLOB.SelectedIndex = 1;
            //}
            ddlTypeofCircular.Items.Clear();
            CommonCodes.SetDropDownDataSource(ddlTypeofCircular, UtilitiesBLL.GetDataTable("getTypeofCircular", new DBUtilityParameter("CDTM_STATUS", "A"), sOrderBy: "CDTM_TYPE_OF_DOC"));
            if (ddlTypeofCircular.Items.Count <= 2)
            {
                ddlTypeofCircular.SelectedIndex = 1;
            }
            ddlSubTypeofCircular.Items.Clear();
            CommonCodes.SetDropDownDataSource(ddlSubTypeofCircular, rcBL.getRefCodeDetails("Circular Document subtype"));
            if (ddlSubTypeofCircular.Items.Count >= 2)
            {
                ddlSubTypeofCircular.SelectedIndex = 2;
            }
            //CommonCodes.SetDropDownDataSource(ddlSpocFromCompFn, UtilitiesBLL.GetDataTable("getSpocFromComplianceFunction", new DBUtilityParameter("CCS_STATUS", "A"), sOrderBy: "CCS_NAME"));
            //if (ddlSpocFromCompFn.Items.Count <= 2)
            //{
            //    ddlSpocFromCompFn.SelectedIndex = 1;
            //}
            cbSpocFromCompFn.Items.Clear();
            CommonCodes.SetCheckboxDataSource(cbSpocFromCompFn, UtilitiesBLL.GetDataTable("getSpocFromComplianceFunction", new DBUtilityParameter("CCS_STATUS", "A"), sOrderBy: "CCS_NAME"));
            cbSubmissions.Items.Clear();
            CommonCodes.SetCheckboxDataSource(cbSubmissions, UtilitiesBLL.GetDataTable("CIRCULARINTIMATIONS", new DBUtilityParameter("CIM_STATUS", "A"), sOrderBy: "CIM_TYPE"));
            cbAssociatedKeywords.Items.Clear();
            CommonCodes.SetCheckboxDataSource(cbAssociatedKeywords, UtilitiesBLL.GetDataTable("AssociatedKeywords", new DBUtilityParameter("CKM_STATUS", "A"), sOrderBy: "CKM_NAME"));
            ddlDepartment.Items.Clear();
            CommonCodes.SetDropDownDataSource(ddlDepartment, UtilitiesBLL.GetDataTable("DEPT", new DBUtilityParameter("CDM_STATUS", "A"), sOrderBy: "CDM_NAME"));
            if (ddlDepartment.Items.Count <= 2)
            {
                ddlDepartment.SelectedIndex = 1;
            }
            //<< Added by Amarjeet on 03-Aug-2021
            ddlLinkageWithEarlierCircular.Items.AddRange(CommonCodes.GetYesNoDDLItems());
            ddlSOCEOC.Items.Clear();
            CommonCodes.SetDropDownDataSource(ddlSOCEOC, rcBL.getRefCodeDetails("Circulars - New Circular - supersedes or extension/amendment to the old circular"));
            if (ddlSOCEOC.Items.Count <= 2)
            {
                ddlSOCEOC.SelectedIndex = 1;
            }
            //>>
            ddlRequirementForTheBoard.Items.Clear();
            ddlRequirementForTheBoard.Items.AddRange(CommonCodes.GetYesNoDDLItems());
            if (ddlRequirementForTheBoard.Items.Count >= 2)
            {
                ddlRequirementForTheBoard.SelectedIndex = 2;
            }
            cbToBePlacedBefore.Items.Clear();
            CommonCodes.SetCheckboxDataSource(cbToBePlacedBefore, rcBL.getRefCodeDetails("Circulars - New Circular - To be placed before"));
            cbSendMailFor.Items.Clear();
            CommonCodes.SetCheckboxDataSource(cbSendMailFor, rcBL.getRefCodeDetails("Circular - Send mail for"));

            Session["CircularFileUploadDT"] = null;

            litAttachment.Text = "<table class=\"table table-bordered footable\" id='tblAttachment' width='100%'> " +
                          " <thead> " +
                          " <tr> " +
                          " <th class='tabhead3'> " +
                          " <input type='checkbox' ID='HeaderLevelCheckBoxAttachment' onclick = 'return onAttachmentHeaderRowChecked()'/> " +
                          " </th>  " +
                          " <th class='tabhead3' align='center'> " +
                          " Attachment Name " +
                          " </th> " +
                          " </tr> " +
                          " </thead> " +
                          " </table> ";

            //reloadDataListViewState();
            reloadAdditionalMailToDataListViewState();
            reloadAdditionalMailCCDataListViewState();
        }
        private void BindDrodownCheckboxandDatatableForEdit()
        {
            ddlLOB.Items.Clear();
            CommonCodes.SetDropDownDataSourceForEdit(ddlLOB, UtilitiesBLL.GetDataTable("getLOBList", sOrderBy: "LEM_NAME"), "LEM_STATUS");
            ddlTypeofCircular.Items.Clear();
            CommonCodes.SetDropDownDataSourceForEdit(ddlTypeofCircular, UtilitiesBLL.GetDataTable("getTypeofCircular", sOrderBy: "CDTM_TYPE_OF_DOC"), "CDTM_STATUS");
            ddlSubTypeofCircular.Items.Clear();
            CommonCodes.SetDropDownDataSourceForEdit(ddlSubTypeofCircular, rcBL.getRefCodeDetails("Circular Document subtype"), "RC_STATUS");
            //CommonCodes.SetDropDownDataSourceForEdit(ddlSpocFromCompFn, UtilitiesBLL.GetDataTable("getSpocFromComplianceFunction", sOrderBy: "CCS_NAME"), "CCS_STATUS");
            ddlDepartment.Items.Clear();
            CommonCodes.SetDropDownDataSourceForEdit(ddlDepartment, UtilitiesBLL.GetDataTable("DEPT", sOrderBy: "CDM_NAME"), "CDM_STATUS");
            ddlSOCEOC.Items.Clear();
            CommonCodes.SetDropDownDataSourceForEdit(ddlSOCEOC, rcBL.getRefCodeDetails("Circulars - New Circular - supersedes or extension/amendment to the old circular"), "RC_STATUS");
            cbSpocFromCompFn.Items.Clear();
            CommonCodes.SetCheckboxDataSourceForEdit(cbSpocFromCompFn, UtilitiesBLL.GetDataTable("getSpocFromComplianceFunction", sOrderBy: "CCS_NAME"), "CCS_STATUS");
            cbSubmissions.Items.Clear();
            CommonCodes.SetCheckboxDataSourceForEdit(cbSubmissions, UtilitiesBLL.GetDataTable("CIRCULARINTIMATIONS", sOrderBy: "CIM_TYPE"), "CIM_STATUS");
            cbAssociatedKeywords.Items.Clear();
            CommonCodes.SetCheckboxDataSourceForEdit(cbAssociatedKeywords, UtilitiesBLL.GetDataTable("AssociatedKeywords", sOrderBy: "CKM_NAME"), "CKM_STATUS");
            //<< Added by Amarjeet on 03-Aug-2021
            ddlLinkageWithEarlierCircular.Items.Clear();
            ddlLinkageWithEarlierCircular.Items.AddRange(CommonCodes.GetYesNoDDLItems());
            //>>
            ddlRequirementForTheBoard.Items.Clear();
            ddlRequirementForTheBoard.Items.AddRange(CommonCodes.GetYesNoDDLItems());
            cbToBePlacedBefore.Items.Clear();
            CommonCodes.SetCheckboxDataSourceForEdit(cbToBePlacedBefore, rcBL.getRefCodeDetails("Circulars - New Circular - To be placed before"), "RC_STATUS");
            cbSendMailFor.Items.Clear();
            CommonCodes.SetCheckboxDataSource(cbSendMailFor, rcBL.getRefCodeDetails("Circular - Send mail for"), "RC_STATUS");

            Session["CircularFileUploadDT"] = null;

            litAttachment.Text = "<table class=\"table table-bordered footable\" id='tblAttachment' width='100%'> " +
                          " <thead> " +
                          " <tr> " +
                          " <th class='tabhead3'> " +
                          " <input type='checkbox' ID='HeaderLevelCheckBoxAttachment' onclick = 'return onAttachmentHeaderRowChecked()'/> " +
                          " </th>  " +
                          " <th class='tabhead3' align='center'> " +
                          " Attachment Name " +
                          " </th> " +
                          " </tr> " +
                          " </thead> " +
                          " </table> ";

            //reloadDataListViewState();
            reloadAdditionalMailToDataListViewState();
            reloadAdditionalMailCCDataListViewState();
        }
        private void bindActionablesGrid()
        {
            try
            {
                Hashtable ParamMap = new Hashtable();
                ParamMap.Add("CMId", string.IsNullOrEmpty(hfSelectedRecord.Value) ? "0" : hfSelectedRecord.Value);
                DataSet dsCircularDetails = CircularMasterBLL.getCircularDetails(ParamMap, mstrConnectionString);
                dtActionables = dsCircularDetails.Tables[0];
                gvActionables.DataSource = dtActionables;
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
                if (!intCircularId.Equals(0))
                {
                    dtCertChecklist = CircularMasterBLL.SearchCircularCertChecklist(intCircularId, "");
                    gvCertChecklists.DataSource = dtCertChecklist;
                    gvCertChecklists.DataBind();
                }
                else
                {
                    gvCertChecklists.DataSource = null;
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

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["CMId"]))
            {
                hfSelectedRecord.Value = en.Decrypt(Request.QueryString["CMId"]);
                BindDrodownCheckboxandDatatableForEdit();
                bindCircularDetails();
            }
            else
            {
                BindDrodownCheckboxandDatatable();
                bindCircularDetails();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable dt = UtilitiesBLL.GetDataTable("checkduplicateCircularNo", new DBUtilityParameter("", txtCircularNo.Text));
                if (hfDuplicateFlag.Value == "Y")
                {
                    hfDoubleClickFlag.Value = "";
                    writeError("Circular no. already exists please enter different circular number.");
                    return;
                }
                //<<Added by Ankur Tyagi on 02Apr2024 for CR_2011
                string chkHiddenFieldForSQLInjection = ConfigurationManager.AppSettings["CheckHiddenField"];
                if (chkHiddenFieldForSQLInjection == "Y")
                {
                    if (!CommonCodes.CheckInputValidity(this))
                    {
                        hfDoubleClickFlag.Value = "";
                        return;
                    }
                }
                //>>

                InsertCircularMaster();
                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void InsertCircularMaster(string strStatus = "")
        {
            string strNameofCircular = null, strAssociatedKeywords = "", strToBePlacedBefore = "",
                strLinkageWithEarlierCircular = "", strSOCEOC = "", strOldCircSubNo = "", strOldCircId = "", strCircEffDate = "",
                strAuditCommitteeToApprove = "", strDetails = "", strNameOfThePolicy = "", strFrequency = "", strBroadcastDate = "",
                strSpocFromComplianceFn = "";
            int intCircularAuthority, intCircularId, intCircularMasterId, intLOB;
            int intArea;
            int intDepartment;
            int intTypeofCircular, intSpocFromComplianceFn;
            string SubtypeOfDocument;

            try
            {
                string strUser = Authentication.GetUserID(Page.User.Identity.Name), strCircularNo = null, strDownloadRefNo = null;
                mstrIssuer = Convert.ToString(ddlCircularAuthority.SelectedItem);
                intCircularAuthority = Convert.ToInt32(ddlCircularAuthority.SelectedValue);
                intDepartment = Convert.ToInt32(ddlDepartment.SelectedValue);
                intArea = Convert.ToInt32(ddlArea.SelectedValue);
                strCircularNo = cm.getSanitizedString(txtCircularNo.Text);
                mstrCircularDate = cm.getSanitizedString(txtCircularDate.Text);
                mstrIssuerlink = cm.getSanitizedString(txtLink.Text);
                mstrSubject = cm.getSanitizedString(txtTopic.Text);
                mstrImplication = FCKE_Implications.Text;
                mstrCircularDetails = FCKE_CircularDetails.Text;
                intTypeofCircular = Convert.ToInt32(ddlTypeofCircular.SelectedValue);
                //intSpocFromComplianceFn = Convert.ToInt32(ddlSpocFromCompFn.SelectedValue);

                strSpocFromComplianceFn = CommonCodes.getCommaSeparatedValuesFromCheckboxList(cbSpocFromCompFn);

                //intLOB = Convert.ToInt32(ddlLOB.SelectedValue);

                if (hfSelectedRecord.Value.Equals(""))
                {
                    intCircularMasterId = 0;
                }
                else
                {
                    intCircularMasterId = Convert.ToInt32(hfSelectedRecord.Value);
                }

                SubtypeOfDocument = ddlSubTypeofCircular.SelectedValue.ToString();

                //<< Added by Amarjeet on 26-Jul-2021
                strAssociatedKeywords = CommonCodes.getCommaSeparatedValuesFromCheckboxList(cbAssociatedKeywords);
                //>>

                //<< Added by Amarjeet on 04-Aug-2021
                strLinkageWithEarlierCircular = ddlLinkageWithEarlierCircular.SelectedValue;

                if (strLinkageWithEarlierCircular.Equals("Y"))
                {
                    strSOCEOC = ddlSOCEOC.SelectedValue;

                    strOldCircSubNo = hfCircularSubjects.Value;
                    strOldCircId = hfCircularID.Value;

                    //DataTable dt = (DataTable)ViewState["Data"];
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    DataRow dr = dt.Rows[i];
                    //    strOldCircId = (string.IsNullOrEmpty(strOldCircId) ? "" : strOldCircId + ",") + dr["CircularName"].ToString();
                    //    strOldCircSubNo = (string.IsNullOrEmpty(strOldCircSubNo) ? "" : strOldCircSubNo + ",") + dr["Name"].ToString();
                    //}
                }
                //>>

                strCircEffDate = txtCircEffDate.Text;
                strAuditCommitteeToApprove = ddlRequirementForTheBoard.SelectedValue;

                if (strAuditCommitteeToApprove.Equals("Y"))
                {
                    strToBePlacedBefore = CommonCodes.getCommaSeparatedValuesFromCheckboxList(cbToBePlacedBefore);
                    strDetails = txtDetails.Text;
                    strNameOfThePolicy = txtNameOfThePolicy.Text;
                    strFrequency = txtFrequency.Text;
                }

                if (strStatus.Equals("B"))
                    strBroadcastDate = Convert.ToDateTime(DateTime.Now).ToString("dd-MMM-yyyy HH:mm:ss");

                intCircularId = CircularMasterBLL.SaveCircular(intCircularMasterId, intCircularAuthority, intArea, intDepartment,
                    strCircularNo, strDownloadRefNo, mstrSubject, mstrImplication, mstrIssuerlink, strNameofCircular,
                    mstrCircularDetails, mstrCircularDate, strUser, intTypeofCircular, getCircularSegmentdt(), getCircularIntimationdt(),
                    getCircularAdditionalMailsdt(), getAttachmentdt(), null, strSpocFromComplianceFn, SubtypeOfDocument,
                    strAssociatedKeywords, strLinkageWithEarlierCircular, strSOCEOC, strOldCircSubNo, strOldCircId, strCircEffDate,
                    strAuditCommitteeToApprove, strToBePlacedBefore, strDetails, strNameOfThePolicy, strFrequency, strBroadcastDate, 0);

                writeError("Circular saved successfully with Id: "
                                   + intCircularId + ". Please use the Broadcast feature to release the document to the stakeholders.");

                btnSendMailFor.Visible = true;
                trSelectAllto.Visible = true;
                trTobeintimate.Visible = true;
                txtAdditionalEmails.Visible = true;
                trAdditionalMail.Visible = true;
                trAdditionalMailTo.Visible = true;
                hfDoubleClickFlag.Value = "";
                hfSelectedRecord.Value = intCircularId.ToString();
                bindCircularDetails();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void bindCircularDetails()
        {
            string strHtmlTableAttachmentRows = "";
            DataRow drGetData;
            DataSet dsEditCircular = new DataSet();
            DataSet dsCircularDetails = new DataSet();
            Hashtable ParamMap = new Hashtable();
            int uniqueRowId = 0;

            try
            {
                dsEditCircular = CircularMasterBLL.SearchCircular(Convert.ToInt32(hfSelectedRecord.Value), "", "", "",
                "", "", "", "", "", "", "Edit", "", "", "", "");
                ParamMap.Add("CMId", hfSelectedRecord.Value);
                dsCircularDetails = CircularMasterBLL.getCircularDetails(ParamMap, mstrConnectionString);
                drGetData = dsEditCircular.Tables[0].Rows[0];
                //<<Added by Ankur Tyagi on 19Mar2024 for CR_1996
                if (!string.IsNullOrEmpty(drGetData["CM_CSDTM_ID"].ToString()))
                {
                    ddlSubTypeofCircular.SelectedValue = drGetData["CM_CSDTM_ID"].ToString();
                }
                if (!string.IsNullOrEmpty(drGetData["CM_CDM_ID"].ToString()))
                {
                    ddlDepartment.SelectedValue = drGetData["CM_CDM_ID"].ToString();
                }
                //ddlCircularAuthority.SelectedValue = drGetData["CM_CIA_ID"].ToString();
                if (!string.IsNullOrEmpty(drGetData["CM_CIA_ID"].ToString()))
                {
                    cddIssuingAuthority.SelectedValue = drGetData["CM_CIA_ID"].ToString();
                }
                if (!string.IsNullOrEmpty(drGetData["CM_CDTM_ID"].ToString()))
                {
                    ddlTypeofCircular.SelectedValue = drGetData["CM_CDTM_ID"].ToString();
                }
                //ddlArea.SelectedValue = drGetData["CM_CAM_ID"].ToString();
                if (!string.IsNullOrEmpty(drGetData["CM_CAM_ID"].ToString()))
                {
                    cddTopic.SelectedValue = drGetData["CM_CAM_ID"].ToString();
                }
                //ddlSpocFromCompFn.SelectedValue = drGetData["CM_CCS_ID"].ToString();
                txtCircularNo.Text = drGetData["CM_CIRCULAR_NO"].ToString();
                if (!string.IsNullOrEmpty(drGetData["CM_DATE"].ToString()))
                {
                    txtCircularDate.Text = Convert.ToDateTime(drGetData["CM_DATE"]).ToString("dd-MMM-yyyy");
                }
                if (!string.IsNullOrEmpty(drGetData["CM_CIRC_EFF_DATE"].ToString()))
                {
                    txtCircEffDate.Text = Convert.ToDateTime(drGetData["CM_CIRC_EFF_DATE"]).ToString("dd-MMM-yyyy");
                }
                //>>
                txtTopic.Text = drGetData["CM_TOPIC"].ToString();
                FCKE_CircularDetails.Text = drGetData["CM_DETAILS"].ToString();
                FCKE_Implications.Text = drGetData["CM_IMPLICATIONS"].ToString();
                txtLink.Text = drGetData["CM_ISSUING_LINK"].ToString();
                //ddlLOB.SelectedValue = drGetData["CM_LEM_ID"].ToString();
                lblSendMailAuditTrail.Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(dsEditCircular.Tables[0].Rows[0]["CM_AUDIT_TRAIL"].ToString().Replace("\r\n", "<br>")));

                if (drGetData["CM_CCS_ID"] != DBNull.Value)
                {
                    cbSpocFromCompFn = CommonCodes.SetCheckboxDataSource(cbSpocFromCompFn, arrValues: drGetData["CM_CCS_ID"].ToString().Split(','));
                }

                if (drGetData["CM_ASSOCIATED_KEY"] != DBNull.Value)
                {
                    cbAssociatedKeywords = CommonCodes.SetCheckboxDataSource(cbAssociatedKeywords, arrValues: drGetData["CM_ASSOCIATED_KEY"].ToString().Split(','));
                }

                //<< Added by Amarjeet on 04-Aug-2021
                CommonCodes.SetDropDownDataSource(ddlLinkageWithEarlierCircular, sSelected: (drGetData["CM_LINKAGE_WITH_EARLIER_CIRCULAR"] is DBNull ? "" : drGetData["CM_LINKAGE_WITH_EARLIER_CIRCULAR"].ToString()));
                CommonCodes.SetDropDownDataSource(ddlSOCEOC, sSelected: (drGetData["CM_SOC_EOC"] is DBNull ? "" : drGetData["CM_SOC_EOC"].ToString()));
                string strOldCircularIds = (drGetData["CM_BASE_ID"] is DBNull ? "" : drGetData["CM_BASE_ID"].ToString());

                txtOldCircSubjectNo.Text = drGetData["CM_OLD_CIRC_SUB_NO"].ToString();
                hfCircularSubjects.Value = drGetData["CM_OLD_CIRC_SUB_NO"].ToString();
                hfCircularID.Value = drGetData["CM_BASE_ID"].ToString();

                //loadExistingCircularList(strOldCircularIds);
                //>>

                CommonCodes.SetDropDownDataSource(ddlRequirementForTheBoard, sSelected: (drGetData["CM_AUDIT_COMMITTEE_TO_APPROVE"] is DBNull ? "" : drGetData["CM_AUDIT_COMMITTEE_TO_APPROVE"].ToString()));
                if (drGetData["CM_TO_BE_PLACED_BEFORE"] != DBNull.Value)
                {
                    cbToBePlacedBefore = CommonCodes.SetCheckboxDataSource(cbToBePlacedBefore, arrValues: drGetData["CM_TO_BE_PLACED_BEFORE"].ToString().Split(','));
                }
                txtDetails.Text = (drGetData["CM_REMARKS"] is DBNull ? "" : drGetData["CM_REMARKS"].ToString().Replace("\n", "<br />"));
                txtNameOfThePolicy.Text = (drGetData["CM_NAME_OF_THE_POLICY"] is DBNull ? "" : drGetData["CM_NAME_OF_THE_POLICY"].ToString());
                txtFrequency.Text = (drGetData["CM_FREQUENCY"] is DBNull ? "" : drGetData["CM_FREQUENCY"].ToString());

                bindIntimationDetails(cbSubmissions, hfSelectedRecord.Value);
                loadExistingAdditionalMailToList(hfSelectedRecord.Value);
                loadExistingAdditionalMailCCList(hfSelectedRecord.Value);

                string strHtmlTable1 = "<table id='tblAttachment' class=\"table table-bordered footable\"  width='100%'> " +
                      " <thead> " +
                      " <tr> " +
                      " <th class='tabhead3'> " +
                      " <input type='checkbox' ID='HeaderLevelCheckBoxAttachment' onclick = 'return onAttachmentHeaderRowChecked()'/> " +
                      " </th> " +
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
                        "</td><td class='tabbody3'>" +
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

                litAttachment.Text = strHtmlTable1 + strHtmlTableAttachmentRows + "</table>";

                btnSendMailFor.Visible = true;
                trSelectAllto.Visible = true;
                trTobeintimate.Visible = true;
                txtAdditionalEmails.Visible = true;
                trAdditionalMail.Visible = true;
                trAdditionalMailTo.Visible = true;
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected DataTable getActionableDataTable()
        {
            DataTable dtAcion = new DataTable();

            try
            {
                bindActionablesGrid();
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
                //strarrActionables = strActopnables.Split('~');
                //for (int i = 0; i < strarrActionables.Length - 1; i++)
                dtActionables = (DataTable)gvActionables.DataSource;
                foreach (DataRow drAct in dtActionables.Rows)
                {
                    //strTemp = strarrActionables[i];
                    //strarrFields = strTemp.Split('|');
                    DataRow dr = dtAcion.NewRow();
                    dr["Id"] = drAct["CA_ID"].ToString(); //strarrFields[0];
                    dr["Actionable"] = drAct["CA_ACTIONABLE"].ToString(); //strarrFields[1];
                    dr["ResFuncName"] = drAct["CFM_NAME"].ToString();
                    dr["PerResp"] = drAct["CA_PERSON_RESPONSIBLE"].ToString(); //strarrFields[2];
                    dr["PerRespUserId"] = drAct["CA_PERSON_RESPONSIBLE_ID"].ToString(); //strarrFields[2];
                    dr["PerRespUserName"] = drAct["CA_PERSON_RESPONSIBLE_NAME"].ToString(); //strarrFields[3];
                    dr["PerRespEmailId"] = drAct["CA_PERSON_RESPONSIBLE_EMAIL_ID"].ToString(); //strarrFields[4];
                    dr["TargetDate"] = drAct["CA_TARGET_DATE"].ToString(); //strarrFields[5];
                    dr["ComplDate"] = drAct["CA_COMPLETION_DATE"].ToString(); //strarrFields[6];
                    dr["Status"] = drAct["RC_NAME"].ToString(); //strarrFields[7];
                    dr["StatusName"] = drAct["RC_NAME"].ToString(); //strarrFields[7];
                    dr["CommType"] = null;
                    dr["Function"] = null;
                    dr["Remarks"] = drAct["CA_REMARKS"].ToString(); //strarrFields[8];
                    dr["ReportMgr"] = drAct["CA_REPORTING_MANAGER"].ToString(); //strarrFields[9];
                    dr["ReportMgrId"] = drAct["CA_Reporting_Mgr_ID"].ToString(); //strarrFields[9];
                    dr["ReportMgrUserName"] = drAct["CA_Reporting_Mgr_Name"].ToString(); //strarrFields[10];
                    dr["ReportMgrEmailId"] = drAct["CA_Reporting_Mgr_EMAIL_ID"].ToString(); //strarrFields[11];
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
                bindCertChecklistsGrid();
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

                dtCertChecklist = (DataTable)gvCertChecklists.DataSource;
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
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return dtChklist;
        }

        protected void bindIntimationDetails(CheckBoxList cbSubmissions, string CircularId)
        {
            DataTable dtIntimationName;
            string strName = null;

            try
            {
                dtIntimationName = UtilitiesBLL.GetDataTable("CIRCULARINTIMATION", new DBUtilityParameter("CMI_CM_ID", hfSelectedRecord.Value));
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

        protected void cvTodate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        private DataTable getCircularSegmentdt()
        {
            DataTable dt = new DataTable();

            try
            {
                dt.Columns.Add(new DataColumn("SegmentId", typeof(string)));
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return dt;
        }

        private DataTable getCircularIntimationdt()
        {
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

        private DataTable getAttachmentdt()
        {
            DataTable dtAttachment = new DataTable();
            string strAttachment = hfAttachment.Value;
            string[] strarrAttachment, strarrFields;
            string strTemp;
            DataRow dr;

            try
            {
                dtAttachment.Columns.Add(new DataColumn("AttachId", typeof(string)));
                dtAttachment.Columns.Add(new DataColumn("ServerFileName", typeof(string)));
                dtAttachment.Columns.Add(new DataColumn("ClientFileName", typeof(string)));

                strarrAttachment = strAttachment.Split('~');
                for (int i = 0; i < strarrAttachment.Length - 1; i++)
                {
                    strTemp = strarrAttachment[i];
                    strarrFields = strTemp.Split('|');
                    dr = dtAttachment.NewRow();

                    dr["AttachId"] = strarrFields[0];
                    dr["ClientFileName"] = strarrFields[1];
                    dr["ServerFileName"] = strarrFields[2];
                    dtAttachment.Rows.Add(dr);
                }
                Session["CircularFileUploadDT"] = dtAttachment;
                mdtCircularFileUpload = dtAttachment;
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return dtAttachment;
        }

        private void initFileUploadDetailedReportsDT()
        {
            mdtCircularFileUpload = new DataTable();
            mdtCircularFileUpload.Columns.Add(new DataColumn("File Name", typeof(string)));
            mdtCircularFileUpload.Columns.Add(new DataColumn("FileNameOnServer", typeof(string)));
            mdtCircularFileUpload.Columns.Add(new DataColumn("User Name", typeof(string)));
            mdtCircularFileUpload.Columns.Add(new DataColumn("Upload Datetime", typeof(string)));
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void btnBroadCast_Click(object sender, System.EventArgs e)
        {
            try
            {
                writeError("");
                //<< Added by Rahuldeb on 28Feb2017 as link of files were not getting displayed in the mail.
                getAttachmentdt();
                InsertCircularMaster("B");
                //>> 
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

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            Session["CircularFileUploadDT"] = null;
            mdtCircularFileUpload = null;
            if (!string.IsNullOrEmpty(Request.QueryString["CMId"]))
            {
                Response.Redirect(Global.site_url("Projects/Circulars/EditCircular.aspx"), false);
            }
            else
            {
                Response.Redirect(Global.site_url("Default.aspx"), false);
            }
        }

        private void clearControls()
        {
            ddlCircularAuthority.SelectedIndex = -1;
            ddlDepartment.SelectedIndex = -1;
            txtLink.Text = "";
            txtTopic.Text = "";
            //txtImplications.Text = "";
            txtCircularDate.Text = "";
            txtCircEffDate.Text = "";
            //txtCircularDetails.Text = "";
            ddlArea.SelectedIndex = -1;
            FCKE_CircularDetails.Text = "";
            FCKE_Implications.Text = "";
            //ddlSubArea.SelectedIndex = -1;
            // txtCircularName.Text = "";
            txtCircularNo.Text = "";
            //txtDownloadRefNo.Text = "";
            // cblSegments.SelectedIndex = -1;
            cbSubmissions.SelectedIndex = -1;
            txtAdditionalEmails.Text = "";
        }

        protected void BtnSendActionableMails_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable dtAcion = getActionableDataTable();
                //string intSpocFromComplianceFn = ddlSpocFromCompFn.SelectedValue.ToString();
                //string strMsg = "";

                //if (dtAcion != null && dtAcion.Rows.Count != 0)
                //{
                //    sendCircularActionableMail(dtAcion, intSpocFromComplianceFn);
                //    strMsg = "Mail sent for all the actionables.";
                //}
                //else
                //{
                //    strMsg = "Please add actionables to send mail.";
                //}

                //writeError(strMsg);

                sendActionableMails();
                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void BtnSendCircCertChecklistMails_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable dtChklist = getCertChecklistDataTable();

                //if (dtChklist != null && dtChklist.Rows.Count != 0)
                //{
                //    sendCircularCertChecklistMail(dtChklist);
                //    writeError("Mail sent for all the checkpoints.");
                //}
                //else
                //{
                //    writeError("Please add checkpoints to send mail.");
                //}

                //sendChecklistMails();
                hfDoubleClickFlag.Value = "";
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

                string strSpocFromComplianceFn = CommonCodes.getCommaSeparatedValuesFromCheckboxList(cbSpocFromCompFn);

                if (dtAcion != null && dtAcion.Rows.Count != 0)
                {
                    sendCircularActionableMail(dtAcion, strSpocFromComplianceFn);

                    //<< Update audit trail
                    if (strNewAll.Equals("NEW"))
                    {
                        strMsg = "Mail sent for newly added actionables.";

                        UtilitiesBLL.GetDataTable("updateNewActionableAuditTrail", new DBUtilityParameter("CM_ID", hfSelectedRecord.Value),
                        new DBUtilityParameter("1", "1", oSecondValue: (new Authentication().getUserFullName(Page.User.Identity.Name))));
                    }
                    else
                    {
                        strMsg = "Mail sent for all the actionables.";

                        UtilitiesBLL.GetDataTable("updateAllActionableAuditTrail", new DBUtilityParameter("CM_ID", hfSelectedRecord.Value),
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

        //private string sendChecklistMails(string strNewAll = "")
        //{
        //    string strMsg = "";

        //    try
        //    {
        //        DataTable dtChklist = getCertChecklistDataTable();

        //        if (strNewAll.Equals("NEW"))
        //        {
        //            DataView dvData = new DataView(dtChklist);
        //            dvData.RowFilter = "IsMailSent = 'N'";
        //            dtChklist = dvData.ToTable();
        //        }

        //        if (dtChklist != null && dtChklist.Rows.Count != 0)
        //        {
        //            sendCircularCertChecklistMail(dtChklist);

        //            //<< Update audit trail
        //            if (strNewAll.Equals("NEW"))
        //            {
        //                strMsg = "Mail sent for newly added checkpoints.";

        //                UtilitiesBLL.GetDataTable("updateNewCertChecklistAuditTrail", new DBUtilityParameter("CM_ID", hfSelectedRecord.Value),
        //                new DBUtilityParameter("1", "1", oSecondValue: (new Authentication().getUserFullName(Page.User.Identity.Name))));
        //            }
        //            else
        //            {
        //                strMsg = "Mail sent for all the checkpoints.";

        //                UtilitiesBLL.GetDataTable("updateAllCertChecklistAuditTrail", new DBUtilityParameter("CM_ID", hfSelectedRecord.Value),
        //                new DBUtilityParameter("1", "1", oSecondValue: (new Authentication().getUserFullName(Page.User.Identity.Name))));
        //            }
        //            //>>
        //        }
        //        else
        //        {
        //            if (strNewAll.Equals("NEW"))
        //                strMsg = "Please add new checkpoints to send mail.";
        //            else
        //                strMsg = "Please add checkpoints to send mail.";
        //        }

        //        //writeError(strMsg);
        //        hfDoubleClickFlag.Value = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        hfDoubleClickFlag.Value = "";
        //        string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
        //        writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
        //    }

        //    return strMsg;
        //}

        private string WrappableText(string source)
        {
            string nwln = Environment.NewLine;
            return "<p>" +
            source.Replace(nwln + nwln, "</p><p>").Replace(nwln, "<br />") + "</p>";
        }

        protected void BtnSaveAddCertChecklist_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CommonCodes.CheckInputValidity(this))
                {
                    return;
                }
                InsertCircularMaster();
                string script = "window.open('AddCircularCertChecklists.aspx?CirId=" + hfSelectedRecord.Value + "','_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "AddCertChecklist", script, true);

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
                if (!CommonCodes.CheckInputValidity(this))
                {
                    return;
                }
                InsertCircularMaster();
                string script = "window.open('../Certification/UploadChecklistData.aspx?Type=CIRC&CircId=" + hfSelectedRecord.Value + "','_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "UploadCertChecklist", script, true);
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

        #region //<< Commented by Amarjeet on 13-Jul-2021 sendCircularActionableMail
        //public void sendCircularActionableMail(DataTable dtActionable, string strSpocFromComplianceFnId)
        //{
        //    DataTable dt_filterdata = new DataTable();
        //    DataTable dtSpocFromcompliance = new DataTable();
        //    string strSpocFromComplianceMailId = "";

        //    string strCirName = null, strCirIssAuthority = null, strCirDate = null, strFooter = ConfigurationManager.AppSettings["MailFooter"].ToString();

        //    dtSpocFromcompliance = UtilitiesBLL.getDatasetWithConditionInString("getSpocFromComplianceFunction", " AND CCS_STATUS = 'A' and CCS_ID = " + strSpocFromComplianceFnId + "", mstrConnectionString);

        //    foreach (DataRow dr in dtSpocFromcompliance.Rows)
        //    {
        //        strSpocFromComplianceMailId = strSpocFromComplianceMailId + "," + dr["CCS_EMAIL_ID"].ToString();
        //    }

        //    if (dtActionable.Rows.Count > 0)
        //    {
        //        try
        //        {
        //            string[] strCC = new string[1];
        //            string[] strUsers = new string[0];
        //            string[] strAdminUsers = new string[0];

        //            strUsers = Roles.GetUsersInRole("CircularUser");
        //            strAdminUsers = Roles.GetUsersInRole("CircularAdmin");

        //            DataTable dtDistinctPerson = dtActionable.DefaultView.ToTable(true, "PerRespUserName");
        //            string strPersonrespID = "";
        //            for (int i = 0; i < dtDistinctPerson.Rows.Count; i++)
        //            {
        //                strPersonrespID = strPersonrespID + dtDistinctPerson.Rows[i]["PerRespUserName"].ToString() + ",";
        //            }
        //            if (!strPersonrespID.Equals(""))
        //            {
        //                strPersonrespID = strPersonrespID.Remove(strPersonrespID.Length - 1);
        //            }

        //            string[] strTo = new string[1];
        //            string strMailTo = "", strMailCC = "";
        //            string strSubject = "";
        //            string strContent = "";
        //            Mail mm = new Mail();
        //            string strHostingServer = ConfigurationManager.AppSettings["HostingServer"].ToString();

        //            MailConfigBLL mailBLL = new MailConfigBLL();
        //            DataTable dtMailConfig = mailBLL.searchMailConfig(null, "Circular Actionables");
        //            if (dtMailConfig == null || dtMailConfig.Rows.Count == 0)
        //            {
        //                writeError("Mail Configuration is not set, could not send mail");
        //                return;
        //            }
        //            DataRow drMailConfig = dtMailConfig.Rows[0];
        //            strSubject = drMailConfig["MCM_SUBJECT"].ToString();
        //            strContent = drMailConfig["MCM_CONTENT"].ToString();

        //            strSubject = strSubject.Replace("%CirType%", ddlTypeofCircular.SelectedItem.Text);
        //            strCirIssAuthority = ddlCircularAuthority.SelectedItem.Text;
        //            strCirDate = txtCircularDate.Text;
        //            strCirName = txtTopic.Text;

        //            string strActionableTableHeader = "", strActionableDetails = "", strActionableContent = "";
        //            strActionableTableHeader =
        //            "<table id=\"tfhover\" border=\"1\"  style=\"font-size: 12px; color: #333333; " +
        //            "width: 100%; border-width: 1px; border-color: #bcaf91; border-collapse: collapse;\">" +
        //            "<tr style=\"background-color: #ffffff;\">" +
        //            "<th style =\"font-size: 12px; background-color: #ded0b0; border-width: 1px; padding: 8px;" +
        //            "border-style: solid; border-color: #bcaf91; text-align: left;\">" +
        //            "Sr. No.</th>" +

        //            "<th style =\"font-size: 12px; background-color: #ded0b0; border-width: 1px; padding: 8px;" +
        //            "border-style: solid; border-color: #bcaf91; text-align: left;\">" +
        //            "Person/Function Responsible</th>" +

        //            "<th style =\"font-size: 12px; background-color: #ded0b0; border-width: 1px; padding: 8px;" +
        //            "border-style: solid; border-color: #bcaf91; text-align: left;\">" +
        //            "Actionable</th>" +

        //            "<th style =\"font-size: 12px; background-color: #ded0b0; border-width: 1px; padding: 8px;" +
        //            "border-style: solid; border-color: #bcaf91; text-align: left;\">" +
        //            "Status </th>" +

        //            "<th style =\"font-size: 12px; background-color: #ded0b0; border-width: 1px; padding: 8px;" +
        //            "border-style: solid; border-color: #bcaf91; text-align: left;\">" +
        //            "Target Date </th>" +

        //            "<th style =\"font-size: 12px; background-color: #ded0b0; border-width: 1px; padding: 8px;" +
        //            "border-style: solid; border-color: #bcaf91; text-align: left;\">" +
        //            "Completion Date </th>" +

        //            "<th style =\"font-size: 12px; background-color: #ded0b0; border-width: 1px; padding: 8px;" +
        //            "border-style: solid; border-color: #bcaf91; text-align: left;\">" +
        //            "Remarks </th></tr>";

        //            for (int intFilter = 0; intFilter < dtActionable.Rows.Count; intFilter++)
        //            {
        //                DataRow drFilter;
        //                drFilter = dtActionable.Rows[intFilter];
        //                strActionableDetails = strActionableDetails +
        //                    " <tr style=\"background-color: #ffffff;\">" +
        //                    "<td style=\"font-size: 12px; border-width: 1px; padding: 8px; border-style: solid;" +
        //                    "border-color: #bcaf91;\"> " + (intFilter + 1) + ". </td>" +

        //                    "<td style=\"font-size: 12px; border-width: 1px; padding: 8px; border-style: solid;" +
        //                    "border-color: #bcaf91;\">" + drFilter["PerRespUserName"].ToString() + "</td>" +

        //                    "<td style=\"font-size: 12px; border-width: 1px; padding: 8px; border-style: solid;" +
        //                    "border-color: #bcaf91;\">" + drFilter["Actionable"].ToString().Replace(Environment.NewLine, "</br>") + "</td>" +


        //                    "<td style=\"font-size: 12px; border-width: 1px; padding: 8px; border-style: solid;" +
        //                        "border-color: #bcaf91;\">" + drFilter["Status"].ToString() + "</td>" +

        //                    "<td style=\"font-size: 12px; border-width: 1px; padding: 8px; border-style: solid;" +
        //                        "border-color: #bcaf91;\">" + (string.IsNullOrEmpty(drFilter["TargetDate"] as string) ? "" : CommonCodes.DbToDispDate(drFilter["TargetDate"].ToString())) + "</td>" +
        //                        "<td style=\"font-size: 12px; border-width: 1px; padding: 8px; border-style: solid;" +
        //                      "border-color: #bcaf91;\">" + (string.IsNullOrEmpty(drFilter["ComplDate"] as string) ? "" : CommonCodes.DbToDispDate(drFilter["ComplDate"].ToString())) + "</td>";

        //                strActionableDetails = strActionableDetails + "<td style=\"font-size: 12px; border-width: 1px; padding: 8px; border-style: solid;" +
        //                "border-color: #bcaf91;\">" + drFilter["Remarks"].ToString().Replace(Environment.NewLine, "</br>") + "</td></tr>";
        //                strMailTo = strMailTo + ',' + drFilter["PerRespEmailId"].ToString();
        //                strMailCC = strMailCC + ',' + drFilter["ReportMgrEmailId"].ToString();
        //            }
        //            if (!strActionableDetails.Equals(""))
        //            {
        //                strActionableContent = strActionableTableHeader + strActionableDetails + " </table>";
        //            }

        //            strSubject = strSubject.Replace("%CirName%", strCirName);

        //            strContent = strContent.Replace("%CirType%", ddlTypeofCircular.SelectedItem.Text);
        //            strContent = strContent.Replace("%CirIssAuthority%", strCirIssAuthority);
        //            strContent = strContent.Replace("%CirDate%", strCirDate);
        //            strContent = strContent.Replace("%CirSubject%", strCirName);
        //            strContent = strContent.Replace("%CirActionPoints%", strActionableContent);
        //            //strContent = strContent.Replace("%CirActionLink%", "<a href=" + Global.site_url("Projects/Circulars/MyActionables.aspx") + " target=\"_blank\">Click here</a> to view your actionables.");
        //            strContent = strContent.Replace("%CirActionLink%", "");
        //            strContent = strContent.Replace("%Footer%", strFooter);

        //            if (!strMailTo.Equals(""))
        //            {
        //                strMailTo = strMailTo.Substring(1);
        //                strTo = strMailTo.Split(',');
        //            }
        //            //<<Added By Rahuldeb to concatinate the Compliance Spoc Email ID in the CC Mail
        //            strMailCC = strMailCC + strSpocFromComplianceMailId;
        //            //>>
        //            if (!strMailCC.Equals(""))
        //            {
        //                strMailCC = strMailCC.Substring(1);
        //                strCC = strMailCC.Split(',');
        //            }

        //            mm.sendAsyncMail(strTo, strCC, null, strSubject, strContent);
        //            //}
        //        }
        //        catch (Exception ex)
        //        {
        //            writeError("Error while sending Actionable Mail : " + ex.Message);
        //        }
        //    }
        //}
        #endregion

        protected void btnAddRegReporting_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CommonCodes.CheckInputValidity(this))
                {
                    return;
                }
                InsertCircularMaster();
                string script = "window.open('../Submissions/CommonSubmission.aspx?CircId=" + hfSelectedRecord.Value + "&Type=CIRC','_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "AddRegReporting", script, true);
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
                if (!CommonCodes.CheckInputValidity(this))
                {
                    return;
                }
                string script = "window.open('../Submissions/ListOfReports.aspx?Type=CIRC&CircId=" + hfSelectedRecord.Value + "','_blank');";
                ClientScript.RegisterStartupScript(this.GetType(), "ViewRegReporting", script, true);
                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        //<< Added by Amarjeet on 21-Aug-2021
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

        #region for Additional Mail Ids TO
        private void reloadAdditionalMailToDataListViewState()
        {
            try
            {
                dtAdditionalMailTOList.Columns.Add("EmailId");
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

        private void loadExistingAdditionalMailToList(string strCircularId)
        {
            DataTable dtRecords = new DataTable();

            try
            {
                if (!strCircularId.Equals(""))
                {
                    dtRecords = UtilitiesBLL.GetDataTable("GetCircularAdditionalMails",
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
                if (!strCircularId.Equals(""))
                {
                    dtRecords = UtilitiesBLL.GetDataTable("GetCircularAdditionalMails",
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

        protected void BtnSendNewCircCertChecklistMails_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable dtChklist = getCertChecklistDataTable();
                //DataView dvData = new DataView(dtChklist);
                //dvData.RowFilter = "IsMailSent = 'N'";
                //dtChklist = dvData.ToTable();

                //if (dtChklist != null && dtChklist.Rows.Count != 0)
                //{
                //    sendCircularCertChecklistMail(dtChklist);
                //    writeError("Mail sent for newly added checkpoints.");
                //}
                //else
                //{
                //    writeError("Please add new checkpoints to send mail.");
                //}

                //sendChecklistMails("NEW");
                hfDoubleClickFlag.Value = "";
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
                //DataTable dtAcion = getActionableDataTable();
                //DataView dvData = new DataView(dtAcion);
                //dvData.RowFilter = "IsMailSent = 'N'";
                //dtAcion = dvData.ToTable();

                //string intSpocFromComplianceFn = ddlSpocFromCompFn.SelectedValue.ToString();
                //if (dtAcion != null && dtAcion.Rows.Count != 0)
                //{
                //    sendCircularActionableMail(dtAcion, intSpocFromComplianceFn);
                //    writeError("Mail sent for newly added actionables.");
                //}
                //else
                //{
                //    writeError("Please add new actionables to send mail.");
                //}

                sendActionableMails("NEW");
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

        public void sendCircularActionableMail(DataTable dtActionable, string strSpocFromComplianceFnId)
        {
            DataTable dtSpocFromcompliance = new DataTable();
            string strActionableIds = "", strMailTo = "", strMailCC = "";

            MailContent_Circulars mail = new MailContent_Circulars();

            if (dtActionable.Rows.Count > 0)
            {
                try
                {
                    for (int intFilter = 0; intFilter < dtActionable.Rows.Count; intFilter++)
                    {
                        DataRow drFilter;
                        drFilter = dtActionable.Rows[intFilter];

                        strActionableIds = (string.IsNullOrEmpty(strActionableIds) ? "" : strActionableIds + ",") + drFilter["Id"].ToString();
                        strMailTo = (string.IsNullOrEmpty(strMailTo) ? "" : strMailTo + ",") + drFilter["PerRespEmailId"].ToString();
                        strMailCC = (string.IsNullOrEmpty(strMailCC) ? "" : strMailCC + ",") + drFilter["ReportMgrEmailId"].ToString();
                    }

                    //dtSpocFromcompliance = UtilitiesBLL.GetDataTable("getSpocFromComplianceFunction", new DBUtilityParameter("CCS_STATUS", "A"), new DBUtilityParameter("CCS_ID", strSpocFromComplianceFnId), sOrderBy: "CCS_NAME");
                    dtSpocFromcompliance = UtilitiesBLL.GetDataTable("getSpocFromComplianceFunction", new DBUtilityParameter("CCS_STATUS", "A"), new DBUtilityParameter("CCS_ID", strSpocFromComplianceFnId, "IN", null, "AND", 1), sOrderBy: "CCS_NAME");

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
                    UtilitiesBLL.GetDataTable("updateIsMailSentForActionables",
                            new DBUtilityParameter("CA_ID", strActionableIds, "IN", oSubQuery: 1),
                            new DBUtilityParameter("CA_IS_MAIL_SENT", "N"));
                    //>>
                }
                catch (Exception ex)
                {
                    hfDoubleClickFlag.Value = "";
                    string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
                }
            }
        }

        public void sendCircularCertChecklistMail(DataTable dtCircCertChecklist)
        {
            MailContent_Circulars mail = new MailContent_Circulars();

            if (dtCircCertChecklist.Rows.Count > 0)
            {
                try
                {
                    DataTable dtDistinctData = new DataView(dtCircCertChecklist).ToTable(true, "DeptName");

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
                        mail.ParamMap.Add("CirSubject", txtTopic.Text);
                        mail.ParamMap.Add("CirIssAuthority", ddlCircularAuthority.SelectedItem.Text);
                        mail.ParamMap.Add("CirDate", txtCircularDate.Text);
                        mail.setCircularMailContent(dtFilteredData);

                        //<< Update IsMailSent "Yes" for the checkpoints mail sent where IsMailSent is "No"
                        UtilitiesBLL.GetDataTable("updateIsMailSentForCertChecklists",
                                new DBUtilityParameter("CCC_ID", strCCCId, "IN", oSubQuery: 1),
                                new DBUtilityParameter("CCC_IS_MAIL_SENT", "N"));
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
        }

        public void Clear()
        {
            ddlCircularAuthority.SelectedIndex = -1;
            ddlDepartment.SelectedIndex = -1;
            ddlArea.SelectedIndex = -1;
            txtCircularNo.Text = "";
            txtCircularDate.Text = "";
            txtCircEffDate.Text = "";
            txtLink.Text = "";
            txtTopic.Text = "";
            FCKE_Implications.Text = "";
            FCKE_CircularDetails.Text = "";
            ddlTypeofCircular.SelectedIndex = -1;
            //ddlSpocFromCompFn.SelectedIndex = -1;
            cbSubmissions.SelectedIndex = -1;
            cbSpocFromCompFn.SelectedIndex = -1;
        }

        //private void SendCircularBroadCast()
        private string SendCircularBroadCast()
        {
            Hashtable ParamMap = new Hashtable();
            DataSet dsCircularDetails = new DataSet();
            MailContent_Circulars mail = new MailContent_Circulars();
            ListItem liChkBoxListItem;
            DataTable dtIntimationOwners = new DataTable();
            string strMailTo = "", strMailCC = "", strMsg = "";
            int intAttachmentCount = 0;

            Boolean isAdditionalMails = false, isIntimations = false;
            //ParamMap.Add("CMId", hfSelectedRecord.Value);
            //dsCircularDetails = CircularMasterBLL.getCircularDetails(ParamMap, mstrConnectionString);
            DataTable dtAttachment = getAttachmentdt();
            //DataTable dtAttachment = dsCircularDetails.Tables[1];

            string strCirIssueType = "issued", strAttachments = "", strAttachmentNames = "", strCirImplications = null,
                strCirAttachment = null;

            try
            {


                //<< to update intimation details before broadcast mail sent
                DataTable dtIntimations = getCircularAdditionalMailsdt();
                CircularMasterBLL.insertCircularAdditionalMails((hfSelectedRecord.Value.Equals("") ? 0 : Convert.ToInt32(hfSelectedRecord.Value)),
                    dtIntimations, Authentication.GetUserID(Page.User.Identity.Name));
                //>>

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

                for (int i = 0; i <= cbSubmissions.Items.Count - 1; i++)
                {
                    liChkBoxListItem = cbSubmissions.Items[i];

                    if (liChkBoxListItem.Selected)
                    {
                        isIntimations = true;
                        //dtIntimationOwners = UtilitiesBLL.getDatasetWithCondition("CIRCINTIMATIONOWNERS", Convert.ToInt32(liChkBoxListItem.Value), mstrConnectionString);
                        dtIntimationOwners = UtilitiesBLL.GetDataTable("CIRCINTIMATIONOWNERS", new DBUtilityParameter("CIU_CIM_ID", liChkBoxListItem.Value));

                        foreach (DataRow dr in dtIntimationOwners.Rows)
                        {
                            strMailTo = (string.IsNullOrEmpty(strMailTo) ? "" : strMailTo + ",") + dr["CIU_EMAIL_ID"].ToString();
                        }
                    }
                }

                if ((isAdditionalMails) || (isIntimations))
                {
                    strCirImplications = string.IsNullOrEmpty(FCKE_Implications.Text) ? "" : "Implications:<br />" + FCKE_Implications.Text;

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
                    mail.ParamMap.Add("CirSummary", FCKE_CircularDetails.Text);
                    mail.ParamMap.Add("CirImplications", strCirImplications);
                    mail.ParamMap.Add("CirAttachment", strCirAttachment);
                    mail.ParamMap.Add("AttachmentCount", intAttachmentCount);
                    mail.ParamMap.Add("Attachments", strAttachments);
                    mail.ParamMap.Add("AttachmentNames", strAttachmentNames);
                    mail.setCircularMailContent(dtAcion, dtCertChecklist);
                    //mail.setCircularMailContent();

                    //writeError("Circular broadcasted successfully.");
                    strMsg = "Circular broadcasted successfully.";

                    //<< Update Broadcast date and audit trail
                    if (isIntimations)
                    {
                        UtilitiesBLL.GetDataTable("updateBroadcastDate", new DBUtilityParameter("CM_ID", hfSelectedRecord.Value));
                        UtilitiesBLL.GetDataTable("updateBroadcastAuditTrail", new DBUtilityParameter("CM_ID", hfSelectedRecord.Value),
                            new DBUtilityParameter("1", "1", oSecondValue: (new Authentication().getUserFullName(Page.User.Identity.Name))),
                            new DBUtilityParameter("1", "1", oSecondValue: txtReasonForBroadcast.Text));
                    }
                    DataSet dsEditCircular = CircularMasterBLL.SearchCircular(Convert.ToInt32(hfSelectedRecord.Value), "", "", "",
                        "", "", "", "", "", "", "Edit", "", "", "", "");
                    lblSendMailAuditTrail.Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(dsEditCircular.Tables[0].Rows[0]["CM_AUDIT_TRAIL"].ToString().Replace("\r\n", "<br>")));
                    //>>
                }

                if (!isIntimations && !isAdditionalMails)
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

        protected void btnSubmit_ServerClick(object sender, EventArgs e)
        {
            string strMsg = "";
            try
            {
                foreach (ListItem listItem in cbSendMailFor.Items)
                {
                    if (listItem.Selected)
                    {
                        if (listItem.Value.Equals("B"))
                            strMsg += SendCircularBroadCast() + "\\r\\n";
                        //else if (listItem.Value.Equals("NCC"))
                        //strMsg += sendChecklistMails("NEW") + "\\r\\n";
                        //else if (listItem.Value.Equals("ACC"))
                        //strMsg += sendChecklistMails() + "\\r\\n";
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
                hfDoubleClickFlag.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        [WebMethod]
        public static List<string> GetResponsiblePersonAU(string strName)
        {
            List<string> lstNames = new List<string>();
            lstNames.Add("asd");
            return lstNames;
        }

    }
}