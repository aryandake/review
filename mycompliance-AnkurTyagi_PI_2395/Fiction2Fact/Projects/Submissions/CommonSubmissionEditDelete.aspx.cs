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
using Fiction2Fact.F2FControls;
using Fiction2Fact.Legacy_App_Code.Circulars.BLL;
using Microsoft.VisualBasic.ApplicationServices;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class Admin_CommonSubmissionEditDelete : System.Web.UI.Page
    {
        UtilitiesBLL utilityBL = new UtilitiesBLL();
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
        private DataTable mdtEditFileUpload;
        RefCodesBLL rcBL = new RefCodesBLL();
        string mstrConnectionString = null;
        CircUtilitiesBLL cBLL = new CircUtilitiesBLL();

        string strTableCellCSS = " style=\"font-size: 12px; font-family: Calibri; border-width: " +
                     "1px; padding: 8px; border-style: solid;" +
                     "border-color: #bcaf91;\"";

        string strTableHeaderCSS = " style =\"font-size: 12px; font-family: Calibri; " +
                                "background-color: #ded0b0; border-width: 1px; padding: 8px;" +
                                "border-style: solid; border-color: #bcaf91; text-align: left;\"";

        string strTableCSS = " style=\"font-size: 12px; font-family: Calibri; color: #333333; " +
                                "width: 100%; border-width: 1px; border-color: #bcaf91; border-collapse: collapse;\"";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == true)
            {
                if (!(Session["EditSubmissionSelectCommand"] == null))
                {
                    //Added by Narendra Naidu on 24-Mar-2015
                    if (!(Session["SubmissionMasEditFiles"] == null))
                    {
                        mdtEditFileUpload = (DataTable)Session["SubmissionMasEditFiles"];
                    }
                    //>>
                }
            }
            else
            {
                if (!(Session["EditSubmissionSelectCommand"] == null))
                {
                    //gvSubmissionMaster.DataSource = (DataSet)Session["EditSubmissionSelectCommand"];
                    //gvSubmissionMaster.DataBind();
                }

                hfCurDate.Value = System.DateTime.Now.ToString("dd-MMM-yyyy");

                //<<Modified by Ashish Mishra on 10Jul2017
                if (User.IsInRole("FilingAdmin"))
                {
                    hfUserType.Value = "Admin";
                    ddlSubType.DataSource = utilityBL.getDataset("SUBTYPE", mstrConnectionString);
                    ddlSubType.DataBind();
                }
                else if (User.IsInRole("Filing_Sub_Admin"))
                {
                    hfUserType.Value = "User";
                    ddlSubType.DataSource = utilityBL.getDatasetWithConditionInString("SUBTYPE", Page.User.Identity.Name.ToString(), mstrConnectionString);
                    ddlSubType.DataBind();
                }
                //>>
                //ddlSubType.DataSource = utilityBL.getDataset("SUBTYPE", mstrConnectionString);
                //ddlSubType.DataBind();

                ddlEventForSearch.DataSource = utilityBL.getDataset("EVENT", mstrConnectionString);
                ddlEventForSearch.DataBind();

                ddlReportDept.DataSource = utilityBL.getDataset("REPORTINGDEPT", mstrConnectionString);
                ddlReportDept.DataBind();

                ddlSegment.DataSource = utilityBL.getDataset("SUBMISSIONSGMT", mstrConnectionString);
                ddlSegment.DataBind();

                mvMultiView.ActiveViewIndex = 0;
                Session["EditSubmission"] = Server.UrlEncode(System.DateTime.Now.ToString());

            }

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["EditSubmission"] = Session["EditSubmission"];
        }

        private void bindGrid()
        {
            DataSet dsSearchSubmissions = new DataSet();
            string strFrequency = ddlFrequency.SelectedValue;
            string strSegment = ddlSegment.SelectedValue;
            string strSubType = ddlType.SelectedValue;
            string strDeptType = ddlSubType.SelectedValue;
            string strEvent = ddlEventForSearch.SelectedValue;
            string strStatus = ddlStatus.SelectedValue;
            string strReportingFunction = ddlReportDept.SelectedValue;
            Boolean blnEventAgendaSelected = false;
            string strFilterEvent = "";

            for (int i = 0; i < cblAssociatedWith.Items.Count; i++)
            {
                ListItem li = cblAssociatedWith.Items[i];
                if (li.Selected)
                {
                    blnEventAgendaSelected = true;
                    if (strFilterEvent == "")
                        strFilterEvent = " and (" + strFilterEvent + " SM_EP_ID = " + li.Value;
                    else
                        strFilterEvent = strFilterEvent + " OR SM_EP_ID = " + li.Value;
                }
            }
            if (blnEventAgendaSelected)
            {
                strFilterEvent = strFilterEvent + ") ";
            }
            dsSearchSubmissions = SubmissionMasterBLL.SearchSubmissions(0, strReportingFunction, strFrequency, strStatus, strSegment, strSubType,
                                                                       strDeptType, strEvent, strFilterEvent, "", "", mstrConnectionString);

            gvSubmissionMaster.DataSource = dsSearchSubmissions;
            gvSubmissionMaster.DataBind();

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            DataSet dsSearchSubmissions = new DataSet();
            string strFrequency = ddlFrequency.SelectedValue;
            string strSegment = ddlSegment.SelectedValue;
            string strSubType = ddlType.SelectedValue;
            string strDeptType = ddlSubType.SelectedValue;
            string strEvent = ddlEventForSearch.SelectedValue;
            string strStatus = ddlStatus.SelectedValue;
            string strReportingFunction = ddlReportDept.SelectedValue;
            Boolean blnEventAgendaSelected = false;
            string strFilterEvent = "";

            for (int i = 0; i < cblAssociatedWith.Items.Count; i++)
            {
                ListItem li = cblAssociatedWith.Items[i];
                if (li.Selected)
                {
                    blnEventAgendaSelected = true;
                    if (strFilterEvent == "")
                        strFilterEvent = " and (" + strFilterEvent + " SM_EP_ID = " + li.Value;
                    else
                        strFilterEvent = strFilterEvent + " OR SM_EP_ID = " + li.Value;
                }
            }
            if (blnEventAgendaSelected)
            {
                strFilterEvent = strFilterEvent + ") order by SM_EP_ID ";
            }
            dsSearchSubmissions = SubmissionMasterBLL.SearchSubmissions(0, strReportingFunction, strFrequency, strStatus, strSegment, strSubType,
                                                                       strDeptType, strEvent, strFilterEvent,
                                                                       Page.User.Identity.Name.ToString(), hfUserType.Value.ToString(),
                                                                       mstrConnectionString);

            Session["EditSubmissionSelectCommand"] = dsSearchSubmissions;
            gvSubmissionMaster.DataSource = dsSearchSubmissions;
            gvSubmissionMaster.DataBind();
            if (gvSubmissionMaster.Rows.Count == 0)
            {
                btnExportToExcel.Visible = false;
                writeError("No Record Found satisfying the criteria.");
            }
            else
            {
                btnExportToExcel.Visible = true;
                hideError();
            }
        }

        protected string LoadSubmissionSegmentName(object SubmissionID)
        {
            DataTable dtSegmentName;
            string strSegmentName = null;
            string strName = null;

            dtSegmentName = utilityBL.getDatasetWithCondition("LOADSUBSEGMENTS", Convert.ToInt32(SubmissionID), mstrConnectionString);
            for (int i = 0; i <= dtSegmentName.Rows.Count - 1; i++)
            {
                strName = dtSegmentName.Rows[i]["SSM_NAME"].ToString();

                if (strSegmentName != null)
                {
                    strSegmentName = strSegmentName + ", " + strName;
                }
                else
                {
                    strSegmentName = strName;
                }
            }
            return strSegmentName;
        }

        private int areSubmissionsDone()
        {
            DataTable dtCount = new DataTable();
            dtCount = utilityBL.getDatasetWithCondition("CHECKSUBMISSIONENTRY", Convert.ToInt32(hfSelectedRecord.Value), mstrConnectionString);

            return dtCount.Rows.Count;
        }

        protected void gvSubmissionMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            Authentication au = new Authentication();
            string strSubmissionId, strSelectedDept, strSelectedEvent;
            // string strOldFileNameOnClient = null;
            // string strOldFileNameOnServer = null;

            DataSet dsEditSubmissions = new DataSet();
            DataRow dr;
            try
            {
                lblInfo.Text = "";
                strSubmissionId = gvSubmissionMaster.SelectedValue.ToString();
                hfSelectedRecord.Value = strSubmissionId;
                GridViewRow gvr = gvSubmissionMaster.SelectedRow;
                //strSubmissionId = ((Label)(gvr.FindControl("lbsmId"))).Text;
                //hfSelectedRecord.Value = strSubmissionId;
                string strSubType = ((HiddenField)(gvr.FindControl("hfSubType"))).Value;
                hfType.Value = strSubType;
                hfEventId.Value = ((Label)(gvr.FindControl("lbEmId"))).Text;
                hfEPId.Value = ((Label)(gvr.FindControl("lbEPId"))).Text;

                if (hdfClientOperation.Value == "Edit")
                {

                    //<<Added by Denil Shah on 14-Apr-2015 for allowing editing even if
                    //records are present in Submissions or Event Instances module.
                    this.lblInfo.Visible = false;
                    mvMultiView.ActiveViewIndex = 1;
                    fvSubmissionMaster.ChangeMode(FormViewMode.Edit);

                    dsEditSubmissions = SubmissionMasterBLL.SearchSubmissions
                                        (Convert.ToInt32(hfSelectedRecord.Value), null, null, null, null,
                                        null, null, null, null, "", "", mstrConnectionString);
                    fvSubmissionMaster.DataSource = dsEditSubmissions;
                    fvSubmissionMaster.DataBind();
                    //>>

                    DropDownList ddlLOB11 = (DropDownList)fvSubmissionMaster.FindControl("ddlLOB");
                    ddlLOB11.Items.Clear();
                    CommonCodes.SetDropDownDataSourceForEdit(ddlLOB11, cBLL.GetDataTable("getLOBList", sOrderBy: "LEM_NAME"), "LEM_STATUS");

                    DropDownList ddlEvent11 = (DropDownList)fvSubmissionMaster.FindControl("ddlEvent");
                    ddlEvent11.Items.Clear();
                    DataSet dsEVENT_EDIT = utilityBL.getDataset("EVENT_EDIT", mstrConnectionString);
                    CommonCodes.SetDropDownDataSourceForEdit(ddlEvent11, dsEVENT_EDIT.Tables[0], "EM_STATUS");

                    DropDownList ddlReportDept11 = (DropDownList)fvSubmissionMaster.FindControl("ddlReportDept");
                    ddlReportDept11.Items.Clear();
                    DataSet dsREPORTINGDEPT_EDIT = utilityBL.getDataset("REPORTINGDEPT_EDIT", mstrConnectionString);
                    CommonCodes.SetDropDownDataSourceForEdit(ddlReportDept11, dsREPORTINGDEPT_EDIT.Tables[0], "SRD_STATUS");
                    ddlReportDept11.Enabled = false;

                    DropDownList ddlSubType11 = (DropDownList)fvSubmissionMaster.FindControl("ddlSubType");
                    ddlSubType11.Items.Clear();
                    DataSet dsSUBTYPE_EDIT = utilityBL.getDataset("SUBTYPE_EDIT", mstrConnectionString);
                    CommonCodes.SetDropDownDataSourceForEdit(ddlSubType11, dsSUBTYPE_EDIT.Tables[0], "STM_STATUS");
                    ddlSubType11.Enabled = false;

                    //if (strSubType == "F")
                    //{
                    //    intCount = areSubmissionsDone();
                    //    if (intCount > 0)
                    //    {
                    //        writeError(intCount + " Submission(s) is/are present for this Entry, So this record cannot be edited.");
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        this.lblInfo.Visible = false;                        
                    //        mvMultiView.ActiveViewIndex = 1;
                    //        fvSubmissionMaster.ChangeMode(FormViewMode.Edit);                      
                    //        dsEditSubmissions = SubmissionMasterBLL.SearchSubmissions(Convert.ToInt32(hfSelectedRecord.Value),null, null, null, null, null, null, null, null, mstrConnectionString);
                    //        fvSubmissionMaster.DataSource = dsEditSubmissions;
                    //        fvSubmissionMaster.DataBind();                       
                    //        //showhideFields();
                    //    }
                    //}
                    //else if (strSubType == "E")
                    //{
                    //    int intEPID = Convert.ToInt32(hfEPId.Value);
                    //    int intEMID =Convert.ToInt32( hfEventId.Value);
                    //    intCount = getEventInstanceEntry(intEPID, intEMID);
                    //    if (intCount > 0)
                    //    {
                    //        writeError(intCount + " record(s) is/are present for this Entry in Event Instances, So this record cannot be edited.");
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        this.lblInfo.Visible = false;
                    //        mvMultiView.ActiveViewIndex = 1;
                    //        fvSubmissionMaster.ChangeMode(FormViewMode.Edit);
                    //        dsEditSubmissions = SubmissionMasterBLL.SearchSubmissions(Convert.ToInt32(hfSelectedRecord.Value),null, null, null, null, null, null, null, null, mstrConnectionString);
                    //        fvSubmissionMaster.DataSource = dsEditSubmissions;
                    //        fvSubmissionMaster.DataBind();
                    //        //showhideFields();
                    //    }
                    //}

                    dr = dsEditSubmissions.Tables[0].Rows[0];
                    strSelectedDept = dr["SM_STM_ID"].ToString();
                    strSelectedEvent = dr["SM_EM_ID"].ToString();
                    ddlLOB11.SelectedValue = dr["SM_LOB_ID"].ToString();
                    DropDownList ddlPriority = (DropDownList)(fvSubmissionMaster.FindControl("ddlPriority"));
                    DropDownList ddlSubType = (DropDownList)(fvSubmissionMaster.FindControl("ddlSubType"));
                    CheckBoxList cbOwners = (CheckBoxList)(fvSubmissionMaster.FindControl("cbOwners"));
                    CheckBoxList cbDeptOwner = (CheckBoxList)(fvSubmissionMaster.FindControl("cbDeptOwner"));
                    CheckBoxList cblSegment = (CheckBoxList)(fvSubmissionMaster.FindControl("cblSegment"));
                    DropDownList ddlEvent = (DropDownList)(fvSubmissionMaster.FindControl("ddlEvent"));
                    RadioButtonList rblAssociatedWith = (RadioButtonList)(fvSubmissionMaster.FindControl("rblAssociatedWith"));
                    DropDownList ddlReportDept = (DropDownList)(fvSubmissionMaster.FindControl("ddlReportDept"));
                    //<<Added by Narendra Naidu on 26-Mar-2015 for getting Submission Files
                    GridView gvInsertFileUpload = (GridView)fvSubmissionMaster.FindControl("gvInsertFileUpload");
                    DropDownList ddlFileType = (DropDownList)(fvSubmissionMaster.FindControl("ddlFileType"));
                    RadioButtonList rblEscalate = (RadioButtonList)(fvSubmissionMaster.FindControl("rblEscalate"));
                    getSubmissionDocumentsById(Convert.ToInt32(hfSelectedRecord.Value));
                    ddlFileType.DataSource = rcBL.getRefCodeDetails("Submisssion File Type", mstrConnectionString);
                    ddlFileType.DataBind();
                    ddlFileType.Items.Insert(0, new ListItem("(Select an option)", ""));
                    //>>
                    //<<Added by Rahuldeb on 15Jun2017
                    F2FTextBox txtEffectiveDate = (F2FTextBox)(fvSubmissionMaster.FindControl("txtEffectiveDate"));
                    if (dr["SM_EFFECTIVE_DT"] != DBNull.Value)
                    {
                        txtEffectiveDate.Text = Convert.ToDateTime(dr["SM_EFFECTIVE_DT"]).ToString("dd-MMM-yyyy");
                    }
                    //>>

                    F2FTextBox txtCircularDate = (F2FTextBox)(fvSubmissionMaster.FindControl("txtCircularDate"));
                    if (dr["SM_CIRCULAR_DATE"] != DBNull.Value)
                    {
                        txtCircularDate.Text = Convert.ToDateTime(dr["SM_CIRCULAR_DATE"]).ToString("dd-MMM-yyyy");
                    }

                    //<<Added by Rahuldeb on 23Feb2018
                    if (User.IsInRole("FilingAdmin"))
                    {
                        ddlSubType.DataSource = utilityBL.getDataset("SUBTYPE", mstrConnectionString);
                        ddlSubType.DataBind();
                    }
                    else if (User.IsInRole("Filing_Sub_Admin"))
                    {
                        ddlSubType.DataSource = utilityBL.getDatasetWithConditionInString("SUBTYPE", Page.User.Identity.Name.ToString(), mstrConnectionString);
                        ddlSubType.DataBind();
                    }
                    //>>

                    ddlSubType.SelectedValue = strSelectedDept;
                    //cbOwners.DataSource = utilityBL.getDatasetWithCondition("OWNERS", Convert.ToInt32(strSelectedDept), mstrConnectionString);
                    //cbOwners.DataBind();
                    // cbCompany.DataSource = utilityBL.getDataset("COMPANY", mstrConnectionString);
                    //cbCompany.DataBind();
                    cblSegment.DataSource = utilityBL.getDataset("SUBMISSIONSGMT", mstrConnectionString);
                    cblSegment.DataBind();
                    ddlEvent.DataSource = utilityBL.getDataset("EVENT", mstrConnectionString);
                    ddlEvent.DataBind();
                    ddlEvent.SelectedValue = strSelectedEvent;
                    ddlPriority.SelectedValue = dr["SM_PRIORITY"].ToString();

                    ddlReportDept.DataSource = utilityBL.getDataset("REPORTINGDEPT", mstrConnectionString);
                    ddlReportDept.DataBind();
                    ddlReportDept.SelectedValue = dr["SM_SRD_ID"].ToString();
                    cbDeptOwner.DataSource = utilityBL.getDatasetWithCondition("REPORTINGOWNERS", Convert.ToInt32(ddlReportDept.SelectedValue), mstrConnectionString);
                    cbDeptOwner.DataBind();
                    bindSegments(cblSegment);
                    //   bindCompany(cbCompany);
                    bindOwners(cbOwners);
                    bindDeptOwners(cbDeptOwner);
                    //bindAgenda(rblAssociatedWith, ddlEvent);
                    HiddenField hfFreq = (HiddenField)(fvSubmissionMaster.FindControl("hfFreq"));

                    if (hfType.Value == "E")
                    {
                        rblAssociatedWith.Items.Clear();
                        rblAssociatedWith.DataSource = utilityBL.getDatasetWithCondition("EVENTPURPOSE", Convert.ToInt32(ddlEvent.SelectedValue), mstrConnectionString);
                        rblAssociatedWith.DataBind();
                        rblAssociatedWith.SelectedValue = hfEPId.Value;
                    }

                    string strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                            "\r\nshowhideTypeBased('" + hfType.Value + "')" +
                            "\r\nshowhideOtherFrequencyPanels('" + hfFreq.Value + "')\r\n" +
                            "\r\nshowhideEscalationDaysSection()\r\n" +
                            "</script>\r\n";

                    ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);

                }
                else if (hdfClientOperation.Value == "Delete")
                {
                    DeleteSubmissionEntry();
                }
            }

            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        private void bindSegments(CheckBoxList cblSegment)
        {
            DataTable dtSegmentName;
            string strName = null;

            dtSegmentName = utilityBL.getDatasetWithCondition("BINDSUBSEGMENTS", Convert.ToInt32(hfSelectedRecord.Value), mstrConnectionString);
            for (int i = 0; i <= dtSegmentName.Rows.Count - 1; i++)
            {
                strName = dtSegmentName.Rows[i]["SSM_CSGM_ID"].ToString();
                cblSegment.Items.FindByValue(strName).Selected = true;
            }
        }

        //private void bindCompany(CheckBoxList cbCompany)
        //{
        //    DataTable dtCompanyName;
        //    string strName = null;

        //    dtCompanyName = utilityBL.getDatasetWithCondition("BINDCOMPANY", Convert.ToInt32(hfSelectedRecord.Value), mstrConnectionString);
        //    for (int i = 0; i <= dtCompanyName.Rows.Count - 1; i++)
        //    {
        //        strName = dtCompanyName.Rows[i]["SCM_CM_ID"].ToString();
        //        cbCompany.Items.FindByValue(strName).Selected = true;
        //    }
        //}

        private void bindOwners(CheckBoxList cbOwners)
        {
            DataTable dtOwnerName;

            dtOwnerName = utilityBL.getDatasetWithCondition("BINDOWNERS", Convert.ToInt32(hfSelectedRecord.Value), mstrConnectionString);
            //for (int i = 0; i <= dtOwnerName.Rows.Count - 1; i++)
            //{
            //    strName = dtOwnerName.Rows[i]["SMO_EM_ID"].ToString();
            //    cbOwners.Items.FindByValue(strName).Selected = true;
            //}
        }

        private void bindDeptOwners(CheckBoxList cbOwners)
        {
            DataTable dtOwnerName;
            dtOwnerName = utilityBL.getDatasetWithCondition("BINDREPORTINGOWNERS", Convert.ToInt32(hfSelectedRecord.Value), mstrConnectionString);
            //for (int i = 0; i <= dtOwnerName.Rows.Count - 1; i++)
            //{
            //    strName = dtOwnerName.Rows[i]["SRO_SRDOM_ID"].ToString();
            //    li = cbOwners.Items.FindByValue(strName);
            //    if (li != null)
            //    {
            //        li.Selected = true;
            //    }
            //}
        }

        private void DeleteSubmissionEntry()
        {
            try
            {
                GridViewRow gvr = gvSubmissionMaster.SelectedRow;
                string strSubType = ((HiddenField)(gvr.FindControl("hfSubType"))).Value;
                int intCount;
                string strSubmissionId = hfSelectedRecord.Value;
                if (strSubType == "F")
                {
                    intCount = areSubmissionsDone();
                    if (intCount > 0)
                    {
                        writeError(intCount + " Submission(s) is/are present for this Entry, So this record cannot be deleted.");
                        return;
                    }
                }
                else if (strSubType == "E")
                {
                    string strEPID = ((HiddenField)(gvr.FindControl("hfEPId"))).Value;
                    string strEmID = ((HiddenField)(gvr.FindControl("hfEPId"))).Value;


                    int intEPID = Convert.ToInt32(strEPID == "" ? "0" : strEPID);
                    int intEMID = Convert.ToInt32(strEmID == "" ? "0" : strEmID);
                    intCount = getEventInstanceEntry(intEPID, intEMID);
                    if (intCount > 0)
                    {
                        writeError(intCount + " Entry present for this Submissions in Event Instances, So this record cannot be deleted.");
                        return;
                    }
                }
                //Added By Milan Yadav on 25Apr2016
                //>>
                SubmissionMasterBLL.deleteSubmissions(Convert.ToInt32(hfSelectedRecord.Value), Authentication.GetUserID(Page.User.Identity.Name), mstrConnectionString);
                //<<
                //Commented By Milan Yadav on 25Apr2016
                //>>
                // utilityBL.getDatasetWithCondition("DELETESUBMISSION",Convert.ToInt32(hfSelectedRecord.Value),mstrConnectionString);
                //<<
                mvMultiView.ActiveViewIndex = 0;
                writeError("Record has been successfully deleted.");
                bindGrid();
            }
            catch (Exception ex)
            {
                this.lblInfo.Text = ex.Message;
                this.lblInfo.Visible = true;
            }
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            if (ViewState["EditSubmission"].ToString() == Session["EditSubmission"].ToString())
            {
                //<<
                editSubmissionDetails();
                //>
                mvMultiView.ActiveViewIndex = 0;
                Session["SubmissionMasEditFiles"] = null;
                Session["EditSubmission"] = Server.UrlEncode(System.DateTime.Now.ToString());
            }
            else
            {
                mvMultiView.ActiveViewIndex = 0;
                writeError("Your attempt to refresh the page was blocked as it would lead to duplication of data.");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session["SubmissionMasEditFiles"] = null;
            mvMultiView.ActiveViewIndex = 0;
        }

        private void editSubmissionDetails()
        {
            try
            {
                int intLOB;
                string strEffectiveDate = null, strParticulars = null, strDescription = null, strPriority = null;
                string strEscalate = null, strType = null;
                string strFrequency = null, strOnceFromDate = null, strOnceToDate = null, strFromWeekDays = null;
                string strF1Fromdate = null, strF1ToDate = null, strF2FromDate = null, strF2ToDate = null;
                string strToWeekDays = null, strMonthlyFromDate = null, strMonthlyTodate = null, strQ1fromDate = null, strQ1ToDate = null, strQ2FromDate = null, strQ2ToDate = null;
                string strQ3FromDate = null, strQ3ToDate = null, strQ4fFromDate = null, strQ4Todate = null, strFirstHalffromDate = null, strFirstHalfToDate = null, strSecondtHalffromDate = null;
                string strSecondtHalffromTo = null, strYearlyfromDate = null, strYearlyDateTo = null, strCreateBy = null, strTrackingDept = "",
                    strReportingDepartment = "", strRegulation = null, strSection = null;
                int intSubType, intReportingDept, intEvent = 0, intAssociatedWith = 0, intStartDays = 0, intEndDays = 0, intlevel1 = 0, intlevel2 = 0;
                int intSubmisionMasterId = Convert.ToInt32(fvSubmissionMaster.SelectedValue);

                strType = Convert.ToString(((RadioButtonList)(fvSubmissionMaster.FindControl("rblType"))).SelectedValue);

                strEffectiveDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtEffectiveDate"))).Text);
                intSubType = Convert.ToInt32(((DropDownList)(fvSubmissionMaster.FindControl("ddlSubType"))).SelectedValue);
                intReportingDept = Convert.ToInt32(((DropDownList)(fvSubmissionMaster.FindControl("ddlReportDept"))).SelectedValue);

                strParticulars = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtPerticulars"))).Text);
                strDescription = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtDescription"))).Text);
                strPriority = Convert.ToString(((DropDownList)(fvSubmissionMaster.FindControl("ddlPriority"))).SelectedValue);
                F2FTextBox txtlevel1 = (F2FTextBox)(fvSubmissionMaster.FindControl("txtlevel1"));
                F2FTextBox txtlevel2 = (F2FTextBox)(fvSubmissionMaster.FindControl("txtlevel2"));

                strReportingDepartment = ((DropDownList)(fvSubmissionMaster.FindControl("ddlReportDept"))).SelectedItem.Text;
                strTrackingDept = ((DropDownList)(fvSubmissionMaster.FindControl("ddlSubType"))).SelectedItem.Text;
                strRegulation = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtReference"))).Text);
                strSection = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtSection"))).Text);
                if (strType == "E")
                {
                    intEvent = Convert.ToInt32(((DropDownList)(fvSubmissionMaster.FindControl("ddlEvent"))).SelectedValue);
                    intAssociatedWith = Convert.ToInt32(((RadioButtonList)(fvSubmissionMaster.FindControl("rblAssociatedWith"))).SelectedValue);
                    intStartDays = Convert.ToInt32(((F2FTextBox)(fvSubmissionMaster.FindControl("txtStartDays"))).Text);
                    intEndDays = Convert.ToInt32(((F2FTextBox)(fvSubmissionMaster.FindControl("txtEndDays"))).Text);
                    //string strSortOrder = ((F2FTextBox)(fvSubmissionMaster.FindControl("txtSortOrder"))).Text;

                    //if (strSortOrder != "")
                    //{
                    //    intSortOrder = Convert.ToInt32(strSortOrder);
                    //}
                }
                else if (strType == "F")
                {
                    RadioButtonList rbl;
                    rbl = ((RadioButtonList)(fvSubmissionMaster.FindControl("rblFrequency")));
                    strFrequency = Convert.ToString(rbl.Text);

                    if (strFrequency == "Only Once")
                    {
                        strOnceFromDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtOnceFromDate"))).Text);
                        strOnceToDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtOnceToDate"))).Text);
                    }
                    else if (strFrequency == "Weekly")
                    {
                        strFromWeekDays = Convert.ToString(((DropDownList)(fvSubmissionMaster.FindControl("ddlFromWeekDays"))).SelectedValue);
                        strToWeekDays = Convert.ToString(((DropDownList)(fvSubmissionMaster.FindControl("ddlTOWeekDays"))).SelectedValue);
                    }
                    else if (strFrequency == "Quarterly")
                    {
                        strQ1fromDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtQ1fromDate"))).Text);
                        strQ1ToDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtQ1ToDate"))).Text);
                        strQ2FromDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtQ2fromDate"))).Text);
                        strQ2ToDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtQ2ToDate"))).Text);
                        strQ3FromDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtQ3fromDate"))).Text);
                        strQ3ToDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtQ3ToDate"))).Text);
                        strQ4fFromDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtQ4fFromDate"))).Text);
                        strQ4Todate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtQ4ToDate"))).Text);
                    }
                    else if (strFrequency == "Monthly")
                    {
                        strMonthlyFromDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtMonthlyFromDate"))).Text);
                        strMonthlyTodate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtMonthlyTodate"))).Text);
                    }
                    else if (strFrequency == "Half Yearly")
                    {
                        strFirstHalffromDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtFirstHalffromDate"))).Text);
                        strFirstHalfToDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtFirstHalfToDate"))).Text);
                        strSecondtHalffromDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtSecondtHalffromDate"))).Text);
                        strSecondtHalffromTo = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtSecondtHalffromTo"))).Text);
                    }
                    else if (strFrequency == "Yearly")
                    {
                        strYearlyfromDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtYearlyfromDate"))).Text);
                        strYearlyDateTo = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtYearlyDateTo"))).Text);
                    }
                    else if (strFrequency == "Fortnightly")
                    {
                        strF1Fromdate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtFortnightly1FromDate"))).Text);
                        strF1ToDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtFortnightly1ToDate"))).Text);
                        strF2FromDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtFortnightly2FromDate"))).Text);
                        strF2ToDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtFortnightly2ToDate"))).Text);
                    }
                }
                if (txtlevel1.Text != "")
                {
                    intlevel1 = Convert.ToInt32(txtlevel1.Text);
                }

                if (txtlevel2.Text != "")
                {
                    intlevel2 = Convert.ToInt32(txtlevel2.Text);
                }

                int intlevel0 = 0;
                F2FTextBox txtlevel0 = (F2FTextBox)(fvSubmissionMaster.FindControl("txtlevel0"));
                if (txtlevel0.Text != "")
                {
                    intlevel0 = Convert.ToInt32(txtlevel0.Text);
                }
                string strCircularDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtCircularDate"))).Text);
                if (strCircularDate == "")
                {
                    strCircularDate = null;
                }
                string strFSApproval = ((DropDownList)(fvSubmissionMaster.FindControl("ddlFSAppReq"))).SelectedValue;

                strEscalate = Convert.ToString(((RadioButtonList)(fvSubmissionMaster.FindControl("rblEscalate"))).SelectedValue);
                strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                //utilityBL.getDatasetWithCondition("DELETESUBSEGMENT", intSubmisionMasterId, mstrConnectionString);
                utilityBL.getDatasetWithCondition("DELETESUBOWNRES", intSubmisionMasterId, mstrConnectionString);
                utilityBL.getDatasetWithCondition("DELETEREPORTINGOWNERS", intSubmisionMasterId, mstrConnectionString);

                //intLOB = Convert.ToInt32(((DropDownList)(fvSubmissionMaster.FindControl("ddlLOB"))).SelectedValue);
                intLOB = 0;
                //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                string strReasonForEdit = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtReasonForEdit"))).Text);
                //>>
                SubmissionMasterBLL.updateSubmissions(intSubmisionMasterId, "Active", intSubType,
                                    intReportingDept,
                                    strType, intEvent, intAssociatedWith, strParticulars, strDescription,
                                    intStartDays,
                                    intEndDays, strEscalate, strFrequency, strOnceFromDate, strOnceToDate,
                                    strFromWeekDays, strToWeekDays, strMonthlyFromDate,
                                    strMonthlyTodate, strQ1fromDate,
                                    strQ1ToDate, strQ2FromDate, strQ2ToDate, strQ3FromDate,
                                    strQ3ToDate, strQ4fFromDate,
                                    strQ4Todate, strFirstHalffromDate, strFirstHalfToDate,
                                    strSecondtHalffromDate, strSecondtHalffromTo, strYearlyfromDate, strYearlyDateTo,
                                    strF1Fromdate, strF1ToDate, strF2FromDate, strF2ToDate,
                                    intlevel1, intlevel2, strEffectiveDate, strPriority, strCreateBy,
                                    getSubmissionOwnerdt(),
                                    null,//getSubmissionCompanydt(), 
                                    null,//getSubmissionSegmentdt(), 
                                    getSubReportingOwnersdt(),
                                    mdtEditFileUpload, strRegulation, strSection, mstrConnectionString, intLOB, strReasonForEdit,
                                    strFSApproval, strCircularDate, intlevel0);

                CheckBoxList cblSegment = (CheckBoxList)(fvSubmissionMaster.FindControl("cblSegment"));
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

                //generateChecklist(intSubmisionMasterId,strEffectiveDate,strType);
                sendMailToTrackingOnModification(strReportingDepartment, intReportingDept, intSubType, strTrackingDept, strReasonForEdit, strReportingTo);
                bindGrid();
                writeError("Record has been updated successfully.");
            }
            catch (Exception ex)
            {
                writeError(ex.Message);

            }
        }


        private void sendMailToTrackingOnModification(string strReportingDepartment, int intReportingDept, int intTrackingDept,
            string strTrackingDept, string strReasonForEdit, string strReportingTo)
        {
            try
            {
                Authentication auth = new Authentication();
                DateTime dt = System.DateTime.Now;
                string[] strTo;
                string[] strCC;
                string strContent = "", strUserName = "", strUserDetails = "";
                string[] strUsers = new string[0];
                string strSubject, strDueDates = "";
                string strFrequency = "";
                MembershipUser user;

                RadioButtonList rblType = (RadioButtonList)(fvSubmissionMaster.FindControl("rblType"));
                RadioButtonList rblFrequency = (RadioButtonList)(fvSubmissionMaster.FindControl("rblFrequency"));

                F2FTextBox txtMonthlyTodate = (F2FTextBox)(fvSubmissionMaster.FindControl("txtMonthlyTodate"));

                F2FTextBox txtOnceFromDate = (F2FTextBox)(fvSubmissionMaster.FindControl("txtOnceFromDate"));
                F2FTextBox txtOnceToDate = (F2FTextBox)(fvSubmissionMaster.FindControl("txtOnceToDate"));

                F2FTextBox txtQ1ToDate = (F2FTextBox)(fvSubmissionMaster.FindControl("txtQ1ToDate"));
                F2FTextBox txtQ2ToDate = (F2FTextBox)(fvSubmissionMaster.FindControl("txtQ2ToDate"));
                F2FTextBox txtQ3Todate = (F2FTextBox)(fvSubmissionMaster.FindControl("txtQ3Todate"));
                F2FTextBox txtQ4Todate = (F2FTextBox)(fvSubmissionMaster.FindControl("txtQ4Todate"));

                F2FTextBox txtFirstHalfToDate = (F2FTextBox)(fvSubmissionMaster.FindControl("txtFirstHalfToDate"));
                F2FTextBox txtSecondtHalffromTo = (F2FTextBox)(fvSubmissionMaster.FindControl("txtSecondtHalffromTo"));

                F2FTextBox txtYearlyDateTo = (F2FTextBox)(fvSubmissionMaster.FindControl("txtYearlyDateTo"));

                DropDownList ddlToWeekDays = (DropDownList)(fvSubmissionMaster.FindControl("ddlToWeekDays"));

                F2FTextBox txtFortnightly1FromDate = (F2FTextBox)(fvSubmissionMaster.FindControl("txtFortnightly1FromDate"));
                F2FTextBox txtFortnightly1ToDate = (F2FTextBox)(fvSubmissionMaster.FindControl("txtFortnightly1ToDate"));
                F2FTextBox txtFortnightly2FromDate = (F2FTextBox)(fvSubmissionMaster.FindControl("txtFortnightly2FromDate"));
                F2FTextBox txtFortnightly2ToDate = (F2FTextBox)(fvSubmissionMaster.FindControl("txtFortnightly2ToDate"));

                F2FTextBox txtPerticulars = (F2FTextBox)(fvSubmissionMaster.FindControl("txtPerticulars"));
                F2FTextBox txtDescription = (F2FTextBox)(fvSubmissionMaster.FindControl("txtDescription"));

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
                dtL1RDU = utilityBL.getDatasetWithTwoConditionInString("getReportingDeptUserFromLevel", Convert.ToString(intReportingDept), "1", mstrConnectionString);

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
                    strFrequency = rblFrequency.SelectedItem.Text.ToString();

                    if (strFrequency.Equals("Monthly"))
                    {
                        strDueDates = txtMonthlyTodate.Text + " day of Every Month";
                    }
                    else if (strFrequency == "Only Once")
                    {
                        strDueDates += txtOnceFromDate.Text + "<br/>";
                        strDueDates += txtOnceToDate.Text + "<br/>";
                    }
                    else if (strFrequency.Equals("Quarterly"))
                    {
                        strDueDates += txtQ1ToDate.Text + " for Quater1.<br/>";
                        strDueDates += txtQ2ToDate.Text + " for Quater2.<br/>";
                        strDueDates += txtQ3Todate.Text + " for Quater3.<br/>";
                        strDueDates += txtQ4Todate.Text + " for Quater4.";
                    }
                    else if (strFrequency.Equals("Half Yearly"))
                    {
                        strDueDates += txtFirstHalfToDate.Text + " for First half.<br/>";
                        strDueDates += txtSecondtHalffromTo.Text + " for Second half.";
                    }
                    else if (strFrequency.Equals("Yearly"))
                    {
                        strDueDates += txtYearlyDateTo.Text + " of every year.";
                    }
                    else if (strFrequency.Equals("Weekly"))
                    {
                        strDueDates += "Every " + ddlToWeekDays.SelectedItem.Text;
                    }
                    else if (strFrequency == "Fortnightly")
                    {
                        strDueDates += "First Fortnightly From Date " + txtFortnightly1FromDate.Text + ".<br/>";
                        strDueDates += "First Fortnightly To Date " + txtFortnightly1ToDate.Text + ".<br/>";
                        strDueDates += "Second Fortnightly From Date " + txtFortnightly2FromDate.Text + ".<br/>";
                        strDueDates += "Second Fortnightly To Date " + txtFortnightly2ToDate.Text + ".";
                    }
                    else
                    {
                        strDueDates = "";
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

                if (!strDueDates.Equals(""))
                {
                    strSubTable += "<th " + strTableHeaderCSS + ">Due Date</th>";
                    //strContent = strContent + "<th " + strTableHeaderCSS + ">Due Date</th>";
                }
                strSubTable += "</tr>" +
                "<tr><td " + strTableCellCSS + ">" + 1 + "</td>" +
                "<td " + strTableCellCSS + ">" + strReportingDepartment + "</td>" +
                "<td " + strTableCellCSS + ">" + strReportingTo + "</td>" +
                "<td " + strTableCellCSS + ">" + txtPerticulars.Text.ToString().Replace(Environment.NewLine, "<br />") + "</td>" +
                "<td " + strTableCellCSS + ">" + txtDescription.Text.ToString().Replace(Environment.NewLine, "<br />") + "</td>" +
                "<td " + strTableCellCSS + ">" + rblType.SelectedItem.Text.ToString() + "</td>" +
                "<td " + strTableCellCSS + ">" + strFrequency + "</td>";

                if (!strDueDates.Equals(""))
                {
                    strSubTable += "<td " + strTableCellCSS + ">" + strDueDates + "</td>";
                    //strContent = strContent + "<td " + strTableCellCSS + ">" + strDueDates + "</td>";
                }
                strSubTable += "</table>";
                //strContent = strContent + "</table>";

                strSubject = strSubject.Replace("%Authority%", strReportingTo);
                strSubject = strSubject.Replace("%AddEditType%", "Modified");
                strSubject = strSubject.Replace("%Department%", strTrackingDept);

                strContent = strContent.Replace("%Authority%", strReportingTo);
                strContent = strContent.Replace("%AddEditType%", "Modified");
                strContent = strContent.Replace("%Department%", strTrackingDept);
                strContent = strContent.Replace("%Reason%", "Reason for Edit: " + strReasonForEdit);

                strContent = strContent.Replace("%SubmissionTable%", strSubTable);
                strContent = strContent.Replace("Kindly take action on the same.", "");
                strContent = strContent.Replace("%Footer%", ConfigurationManager.AppSettings["MailFooter"].ToString());
                strContent = strContent.Replace("%Link%", "<a href=" + Global.site_url("Projects/Submissions/SubmissionCheckListForClosure.aspx") + " target=\"_blank\">Click here</a>");

                //strContent = strContent + "<br/><br/>" +
                //ConfigurationManager.AppSettings["MailFooter"].ToString() + "</body></html>";

                mm.sendAsyncMail(strTo, strCC, null, strSubject, strContent);
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        private void generateChecklist(int intSubmissionMasterId, string strEffectiveDate, string strSubmissionType)
        {
            DataServer dserv = new DataServer();
            DataTable dt = new DataTable();
            DataRow dr;
            string strId = "";

            if (strSubmissionType == "F")
            {
                int intNoOfrecords;
                utilityBL.getDatasetWithCondition("DELETESUBCHECKLIST", intSubmissionMasterId, mstrConnectionString);
                intNoOfrecords = SubmissionMasterBLL.generateSubmissionsWiseChecklist(intSubmissionMasterId, strEffectiveDate, mstrConnectionString);
            }
            else if (strSubmissionType == "E")
            {
                dt = dserv.Getdata(" select * from [TBL_EI_EP_MAPPING] " +
                                  " inner join [TBL_EVENT_PURPOSE] on [EEM_EP_ID] = [EP_ID] and [EP_EM_ID] = " +
                                  Convert.ToString(((DropDownList)(fvSubmissionMaster.FindControl("ddlEvent"))).SelectedValue) +
                                  " inner join [TBL_EVENT_INSTANCES] on [EEM_EI_ID] = [EI_ID] and [EI_EM_ID] = " +
                                  Convert.ToString(((DropDownList)(fvSubmissionMaster.FindControl("ddlEvent"))).SelectedValue));


                if (dt.Rows.Count > 0)
                {
                    int intNoOfrecords;
                    dr = dt.Rows[0];

                    strId = dr["EEM_ID"].ToString();
                    DataServer.ExecuteSql("delete from TBL_SUB_CHKLIST where SC_SM_ID = " + intSubmissionMasterId +
                                        " and SC_EEM_ID in (select EEM_ID FROM TBL_EI_EP_MAPPING WHERE EEM_EI_ID=" + strId + ")");
                    intNoOfrecords = SubmissionMasterBLL.generateSubmissionsWiseEventChecklist(intSubmissionMasterId, strId, mstrConnectionString);
                    //writeError("Submission details saved successfully.");
                }
                else
                {
                    //writeError("Submission details saved successfully. But the Tasks were not Created as there are No entry made for " + ddlEvent.SelectedItem.Text + " in Event Instances" +
                    //    "<br/>The Task shall be created Once the Entry is made in Event Instance for the " + ddlEvent.SelectedItem.Text + " and Agenda " + rblAssociatedWith.SelectedItem.Text);
                }
                //string str
                //intNoOfrecords = SubmissionMasterBLL.generateSubmissionsWiseChecklist(intSubmissionMasterId, strEffectiveDate, mstrConnectionString);
            }
        }

        private DataTable getSubmissionOwnerdt()
        {
            CheckBoxList cbOwners = ((CheckBoxList)(fvSubmissionMaster.FindControl("cbOwners")));
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
            CheckBoxList cbCompany = ((CheckBoxList)(fvSubmissionMaster.FindControl("cbCompany")));
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
            CheckBoxList cblSegment = ((CheckBoxList)(fvSubmissionMaster.FindControl("cblSegment")));
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
            dt = new DataTable();
            CheckBoxList cbDeptOwner = ((CheckBoxList)(fvSubmissionMaster.FindControl("cbDeptOwner")));

            dt.Columns.Add(new DataColumn("OwnerId", typeof(string)));
            //for (int i = 0; i <= cbDeptOwner.Items.Count - 1; i++)
            //{

            //    liChkBoxListItem = cbDeptOwner.Items[i];
            //    if (liChkBoxListItem.Selected)
            //    {
            //        dr = dt.NewRow();
            //        dr["OwnerId"] = liChkBoxListItem.Value;
            //        dt.Rows.Add(dr);
            //    }
            //}
            return dt;
        }

        protected void fvSubmissionMaster_DataBound(object sender, EventArgs e)
        {
            showhideFields();

        }

        protected void ddlEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rblAssociatedWith = (RadioButtonList)fvSubmissionMaster.FindControl("rblAssociatedWith");
            rblAssociatedWith.Items.Clear();
            DropDownList ddlEvent = (DropDownList)fvSubmissionMaster.FindControl("ddlEvent");
            rblAssociatedWith.DataSource = utilityBL.getDatasetWithCondition("EVENTPURPOSE", Convert.ToInt32(ddlEvent.SelectedValue), mstrConnectionString);
            rblAssociatedWith.DataBind();

            string script = "\r\n<script language=\"javascript\">\r\n" +
       "   // alert('h');\r\n" +
       "    var hf = document.getElementById('hfFixedOrEvent');\r\n" +
       "   // alert('hi');\r\n" +
       "    var elem = document.getElementById('FixedDateBaseSection');\r\n" +
       "    //alert(elem);\r\n" +
       "    var elem1 = document.getElementById('EventBasedSection');\r\n" +
       "    //alert('hi4');\r\n" +
       "    if (hf.value == 'E')\r\n" +
       "       { \r\n" +
       "        // alert('hi2');\r\n" +
       "               \r\n" +
       "            elem.style.display = 'block';\r\n" +
       "           elem.style.visibility = 'visible';\r\n" +
       "           elem1.style.display = 'none';\r\n" +
       "           elem1.style.visibility = 'hidden';\r\n" +
       "       }\r\n" +
       "       else\r\n" +
       "       {         \r\n" +
       "           elem.style.display = 'none';\r\n" +
       "           elem.style.visibility = 'hidden';\r\n" +
       "           elem1.style.display = 'block';\r\n" +
       "           elem1.style.visibility = 'visible';\r\n" +
       "       }\r\n" +
       "   </script>\r\n";
            ClientScript.RegisterStartupScript(this.GetType(), "script", script);

        }

        private int getEventInstanceEntry(int intEP, int intEM)
        {
            DataTable dtEventInstance = new DataTable();
            dtEventInstance = utilityBL.getDatasetWithMoreCondition("CHECKEVENTINSTANCEENTRY", intEP, intEM, mstrConnectionString);

            return dtEventInstance.Rows.Count;
        }

        private void showhideFields()
        {
            try
            {
                GridViewRow gvrRow = gvSubmissionMaster.SelectedRow;
                string strFrequency, strType;
                strFrequency = ((Label)(gvrRow.FindControl("lbFrequency"))).Text;
                strType = ((HiddenField)(gvrRow.FindControl("hfSubType"))).Value;
                RadioButtonList rbl;
                rbl = ((RadioButtonList)(fvSubmissionMaster.FindControl("rblFrequency")));
                string script;
                if (strType == "F")
                {
                    rbl.SelectedValue = strFrequency;
                    script = "\r\n<script language=\"javascript\">\r\n" +

                   "    var elem = document.getElementById('FixedDateBaseSection');\r\n" +
                   "    var elem1 = document.getElementById('EventBasedSection');\r\n" +

                   "           elem.style.display = 'block';\r\n" +
                   "           elem.style.visibility = 'visible';\r\n" +
                   "           elem1.style.display = 'none';\r\n" +
                   "           elem1.style.visibility = 'hidden';\r\n" +
                   " showhideOtherFrequencyPanels('" + rbl.ClientID + "');" +
                   "   </script>\r\n";

                }
                else
                {

                    script = "\r\n<script language=\"javascript\">\r\n" +

                               "    var elem = document.getElementById('FixedDateBaseSection');\r\n" +
                               "    var elem1 = document.getElementById('EventBasedSection');\r\n" +

                               "           elem.style.display = 'none';\r\n" +
                               "           elem.style.visibility = 'hidden';\r\n" +
                               "           elem1.style.display = 'block';\r\n" +
                               "           elem1.style.visibility = 'visible';\r\n" +

                               " showhideOtherFrequencyPanels('" + rbl.ClientID + "');" +
                               "   </script>\r\n";
                }

                RadioButtonList rblType = ((RadioButtonList)(fvSubmissionMaster.FindControl("rblType")));
                rblType.Attributes["onClick"] = "showhideTypeBased('" + rblType.ClientID + "')";
                rbl.Attributes["onClick"] = "showhideOtherFrequencyPanels('" + rbl.ClientID + "')";
                ClientScript.RegisterStartupScript(this.GetType(), "script", script);
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        protected void ddlEventForSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblInfo.Text = "";
            cblAssociatedWith.Items.Clear();
            if (!ddlEventForSearch.SelectedValue.Equals(""))
            {
                cblAssociatedWith.DataSource = utilityBL.getDatasetWithCondition("EVENTPURPOSE", Convert.ToInt32(ddlEventForSearch.SelectedValue), mstrConnectionString);
                cblAssociatedWith.DataBind();
            }
        }

        private void bindAgenda(RadioButtonList rlbAgenda, DropDownList ddlEvent)
        {
            if (hfType.Value == "E")
            {
                rlbAgenda.Items.Clear();
                rlbAgenda.DataSource = utilityBL.getDatasetWithCondition("EVENTPURPOSE", Convert.ToInt32(ddlEvent.SelectedValue), mstrConnectionString);
                rlbAgenda.DataBind();
                rlbAgenda.SelectedValue = hfEPId.Value;
            }
        }

        protected void ddlSubType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList cbOwners = (CheckBoxList)fvSubmissionMaster.FindControl("cbOwners");
            DropDownList ddlStmID = ((DropDownList)(fvSubmissionMaster.FindControl("ddlSubType")));
            cbOwners.Items.Clear();
            if (!ddlStmID.SelectedValue.Equals(""))
            {
                cbOwners.DataSource = utilityBL.getDatasetWithCondition("OWNERS", Convert.ToInt32(ddlStmID.SelectedValue), mstrConnectionString);
                cbOwners.DataBind();
            }
        }

        protected void ddlReportDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList cbDeptOwner = (CheckBoxList)fvSubmissionMaster.FindControl("cbDeptOwner");
            DropDownList ddlReportDept = ((DropDownList)(fvSubmissionMaster.FindControl("ddlReportDept")));
            cbDeptOwner.Items.Clear();
            cbDeptOwner.DataSource = utilityBL.getDatasetWithCondition("REPORTINGOWNERS", Convert.ToInt32(ddlReportDept.SelectedValue), mstrConnectionString);
            cbDeptOwner.DataBind();
        }


        protected void gvSubmissionMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSubmissionMaster.PageIndex = e.NewPageIndex;
            gvSubmissionMaster.DataSource = (DataSet)(Session["EditSubmissionSelectCommand"]);
            gvSubmissionMaster.DataBind();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            //<<Changed By Rahuldeb on 15Jun2017
            gvSubmissionMaster.DataSource = (DataSet)(Session["EditSubmissionSelectCommand"]);
            gvSubmissionMaster.DataBind();
            gvSubmissionMaster.Columns[4].Visible = false;
            gvSubmissionMaster.Columns[5].Visible = false;
            //>>
            gvSubmissionMaster.AllowPaging = false;
            gvSubmissionMaster.AllowSorting = false;
            //gvSubmissionMaster.DataSource = mdtActivityDetails;
            gvSubmissionMaster.DataBind();
            CommonCodes.PrepareGridViewForExport(gvSubmissionMaster);
            string attachment = "attachment; filename=Submissions.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvSubmissionMaster.RenderControl(htw);

            Response.Write(sw.ToString());
            Response.End();
            gvSubmissionMaster.AllowPaging = true;
            gvSubmissionMaster.AllowSorting = true;
            gvSubmissionMaster.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        //<< Added by Narendra Naidu on 24-Mar-2015 for File Upload
        protected void btnAddAttachment_Click(object sender, System.EventArgs e)
        {
            lblInfo.Text = "";
            Authentication au = new Authentication();
            DataRow dr;
            FileUpload fuEditFileUpload = (FileUpload)(fvSubmissionMaster.FindControl("fuEditFileUpload"));

            string strSelectedFile;
            if ((fuEditFileUpload.HasFile))
            {
                strSelectedFile = fuEditFileUpload.FileName;
                //if (strSelectedFile.Contains("!") || strSelectedFile.Contains("@") ||
                //                strSelectedFile.Contains("#") || strSelectedFile.Contains("$") ||
                //                strSelectedFile.Contains("%") || strSelectedFile.Contains("^") ||
                //                strSelectedFile.Contains("&") || strSelectedFile.Contains("'") ||
                //                strSelectedFile.Contains("\""))
                //{
                //    writeError("File Name can't have special character.");
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
                    GridView gvInsertFileUpload = (GridView)fvSubmissionMaster.FindControl("gvInsertFileUpload");
                    DropDownList ddlFileType = (DropDownList)(fvSubmissionMaster.FindControl("ddlFileType"));
                    F2FTextBox txtFileDesc = ((F2FTextBox)(fvSubmissionMaster.FindControl("txtFileDesc")));

                    strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["SubmissionFiles"].ToString());
                    strFileNameOnClient = fuEditFileUpload.FileName;
                    dtUploadDatetime = System.DateTime.Now;
                    strFileNameOnServer = Authentication.GetUserID(Page.User.Identity.Name) + "_" + dtUploadDatetime.ToString("ddMMyyyyHHmmss") + "_" + fuEditFileUpload.FileName;
                    fuEditFileUpload.SaveAs(strServerDirectory + "\\\\" + strFileNameOnServer);

                    if ((mdtEditFileUpload == null))
                    {
                        initFileUpload();
                    }

                    dr = mdtEditFileUpload.NewRow();
                    dr["FileName"] = strFileNameOnClient;
                    dr["FileNameOnServer"] = strFileNameOnServer;
                    dr["Uploaded By"] = au.getUserFullName(Page.User.Identity.Name.ToString());
                    dr["Uploaded On"] = dtUploadDatetime.ToString("dd-MMM-yyyy HH:mm:ss");
                    dr["FileTypeShortForm"] = ddlFileType.SelectedValue.ToString();
                    dr["File Description"] = txtFileDesc.Text.ToString();
                    dr["FileType"] = ddlFileType.SelectedItem.Text.ToString();

                    mdtEditFileUpload.Rows.Add(dr);
                    gvInsertFileUpload.DataSource = mdtEditFileUpload;
                    gvInsertFileUpload.DataBind();
                    Session["SubmissionMasEditFiles"] = mdtEditFileUpload;

                    ddlFileType.SelectedIndex = -1;
                    txtFileDesc.Text = "";
                    showhideFields();

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

        private void initFileUpload()
        {
            mdtEditFileUpload = new DataTable();
            mdtEditFileUpload.Columns.Add(new DataColumn("FileName", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("FileNameOnServer", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("Uploaded By", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("Uploaded On", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("FileTypeShortForm", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("File Description", typeof(string)));
            mdtEditFileUpload.Columns.Add(new DataColumn("FileType", typeof(string)));
        }

        protected void gvInsertFileUpload_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblInfo.Text = "";
            FileInfo fileInfo;
            string strFilePath;
            string strFileName;
            string strCompleteFileName;

            GridView gvInsertFileUpload = (GridView)(fvSubmissionMaster.FindControl("gvInsertFileUpload"));
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
            showhideFields();
        }

        protected void gvAlreadyUploadedFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblInfo.Text = "";
            FileInfo fileInfo;
            string strFilePath;
            string strCompleteFileName;
            GridView gvAlreadyUploadedFiles = (GridView)(fvSubmissionMaster.FindControl("gvAlreadyUploadedFiles"));
            strFilePath = Server.MapPath(ConfigurationManager.AppSettings["SubmissionFiles"].ToString());
            FileUpload fuEditFileUpload = (FileUpload)(fvSubmissionMaster.FindControl("fuEditFileUpload"));
            HiddenField hfServerFileName = (HiddenField)gvAlreadyUploadedFiles.SelectedRow.FindControl("hfServerFileName");

            strCompleteFileName = (strFilePath + ("\\" + hfServerFileName.Value));
            fileInfo = new FileInfo(strCompleteFileName);
            int intId = Convert.ToInt32(gvAlreadyUploadedFiles.SelectedValue);
            utilityBL.getDatasetWithCondition("deleteSubmissionFilesById", intId, mstrConnectionString);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            int intSMId = Convert.ToInt32(hfSelectedRecord.Value);
            DataTable dtEditFiles = utilityBL.getDatasetWithCondition("getSubmisssionMasFiles", intSMId, mstrConnectionString);
            gvAlreadyUploadedFiles.DataSource = dtEditFiles;
            gvAlreadyUploadedFiles.DataBind();
            showhideFields();

        }

        private void getSubmissionDocumentsById(int intID)
        {
            try
            {
                GridView gvAlreadyUploadedFiles = (GridView)(fvSubmissionMaster.FindControl("gvAlreadyUploadedFiles"));
                DataTable dtAttachment = utilityBL.getDatasetWithCondition("getSubmisssionMasFiles", intID, mstrConnectionString);
                gvAlreadyUploadedFiles.DataSource = dtAttachment;
                gvAlreadyUploadedFiles.DataBind();
            }
            catch (Exception exp)
            {
                writeError("Exception in getSubmissionDocumentsById :" + exp.Message);
            }
        }

        #region Gridview Sorting
        protected void gvSubmissionMaster_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (Session["EditSubmissionSelectCommand"] != null)
            {

                DataSet ds = (DataSet)Session["EditSubmissionSelectCommand"];
                DataTable dt = ds.Tables[0];
                DataView dvDataView = new DataView(dt);
                string strSortExpression = "";

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

                gvSubmissionMaster.DataSource = dvDataView;
                gvSubmissionMaster.DataBind();

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
                foreach (DataControlField field in gvSubmissionMaster.Columns)
                {
                    if (field.SortExpression.Equals(strSortExpression))
                    {
                        return gvSubmissionMaster.Columns.IndexOf(field);
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
        #endregion Gridview Sorting

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