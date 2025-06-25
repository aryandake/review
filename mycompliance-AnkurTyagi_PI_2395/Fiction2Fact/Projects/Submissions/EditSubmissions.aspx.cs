using System;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using Fiction2Fact.F2FControls;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class EditSubmissions : System.Web.UI.Page
    {
        UtilitiesBLL utilitybl = new UtilitiesBLL();
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
        string mstrConnectionString = null;

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
            if (!Page.IsPostBack)
            {
                try
                {
                    int intSubmissionId, intType;
                    intSubmissionId = Convert.ToInt32(Request.QueryString["SubmissionId"].ToString());
                    intType = Convert.ToInt32(Request.QueryString["OpType"].ToString());
                    hfOpType.Value = Convert.ToString(intType);
                    hfSubId.Value = Convert.ToString(intSubmissionId);
                    if (intType == 1)
                    {
                        DataSet dsSubmissions = new DataSet();
                        pnlInactive.Visible = true;
                        dsSubmissions = SubmissionMasterBLL.SearchSubmissions(intSubmissionId, null, null, null, null, null, null, null, null, "", "", mstrConnectionString);
                        fvSubmissionMaster.DataSource = dsSubmissions;
                        fvSubmissionMaster.DataBind();
                        fvSubmissionMaster.ChangeMode(FormViewMode.Edit);
                        bindDetails(dsSubmissions.Tables[0].Rows[0]);
                    }
                    else if (intType == 3)
                    {
                        DataSet dsSubmissions = new DataSet();
                        pnlDetails.Visible = true;
                        dsSubmissions = SubmissionMasterBLL.SearchSubmissions(intSubmissionId, null, null, null, null, null, null, null, null, "", "", mstrConnectionString);
                        fvDetails.DataSource = dsSubmissions;
                        fvDetails.DataBind();
                        fvDetails.ChangeMode(FormViewMode.Edit);
                        //<<Added by prajakta                    

                        DataRow dr = dsSubmissions.Tables[0].Rows[0];
                        string strdept = dr["SM_STM_ID"].ToString();
                        DropDownList ddlEditSubType = (DropDownList)fvDetails.FindControl("ddlEditSubType");
                        ddlEditSubType.DataSource = utilitybl.getDataset("TYPE", mstrConnectionString);
                        ddlEditSubType.DataBind();
                        ddlEditSubType.Items.Insert(0, new ListItem("--Select--", "0"));
                        ddlEditSubType.SelectedValue = strdept;

                        DropDownList ddlEditReportDept = (DropDownList)fvDetails.FindControl("ddlEditReportDept");
                        ddlEditReportDept.DataSource = utilitybl.getDataset("REPORTINGDEPT", mstrConnectionString);
                        ddlEditReportDept.DataBind();
                        ddlEditReportDept.Items.Insert(0, new ListItem("--Select--", "0"));
                        string strRepDeptId = dr["SM_SRD_ID"].ToString();
                        if (!(strRepDeptId.Equals("")))
                            ddlEditReportDept.SelectedValue = strRepDeptId;

                        //CheckBoxList cbOwners = (CheckBoxList)fvDetails.FindControl("cbOwners");
                        //cbOwners.DataSource = utilitybl.getDatasetWithCondition("OWNERS", Convert.ToInt32(strdept), mstrConnectionString);
                        //cbOwners.DataBind();
                        //DataTable dtOwner = utilitybl.getDatasetWithCondition("TrackedByOwners", intSubmissionId, mstrConnectionString);
                        //string strName = "", strName2 = "";
                        //for (int i = 0; i <= dtOwner.Rows.Count - 1; i++)
                        //{
                        //    strName = dtOwner.Rows[i]["EM_ID"].ToString();
                        //    cbOwners.Items.FindByValue(strName).Selected = true;
                        //}
                        //CheckBoxList cbDeptOwner = (CheckBoxList)fvDetails.FindControl("cbDeptOwner");
                        //cbDeptOwner.DataSource = utilitybl.getDatasetWithCondition("REPORTINGOWNERS", Convert.ToInt32(strRepDeptId), mstrConnectionString);
                        //cbDeptOwner.DataBind();
                        //DataTable dtOwner2 = utilitybl.getDatasetWithCondition("ReportingDeptOwners", intSubmissionId, mstrConnectionString);

                        //for (int i = 0; i <= dtOwner2.Rows.Count - 1; i++)
                        //{
                        //    strName2 = dtOwner2.Rows[i]["SRDOM_ID"].ToString();
                        //    cbDeptOwner.Items.FindByValue(strName2).Selected = true;
                        //}

                        //>>
                    }
                    else if (intType == 4)
                    {
                        DataSet dsSubmissions = new DataSet();
                        pnlDuedates.Visible = true;
                        dsSubmissions = SubmissionMasterBLL.SearchSubmissions(intSubmissionId, null, null, null, null, null, null, null, null, "", "", mstrConnectionString);
                        fvSubmissionMaster.DataSource = dsSubmissions;
                        fvSubmissionMaster.DataBind();
                        fvSubmissionMaster.ChangeMode(FormViewMode.Edit);
                        bindDetails(dsSubmissions.Tables[0].Rows[0]);

                    }
                    else
                    {
                        writeError("No such Operation is Available");
                    }
                }
                catch (Exception ex)
                {
                    writeError("Exception" + ex.Message);
                }
            }
        }


        protected void ddlEvent_DataBound(object sender, EventArgs e)
        {
            string strType = ((RadioButtonList)(fvSubmissionMaster.FindControl("rblType"))).SelectedValue;
            if (strType.Equals("E"))
            {
                DropDownList ddlEvent;
                RadioButtonList rblAgenda;
                string strEventID, strAgendaID;
                strEventID = ((Label)(fvSubmissionMaster.FindControl("lblEvent"))).Text;
                strAgendaID = ((Label)(fvSubmissionMaster.FindControl("lbAgenda"))).Text;
                ddlEvent = ((DropDownList)(fvSubmissionMaster.FindControl("ddlEvent")));
                rblAgenda = ((RadioButtonList)(fvSubmissionMaster.FindControl("rblAssociatedWith")));
                ddlEvent.SelectedValue = strEventID;
                rblAgenda.Items.Clear();
                rblAgenda.DataSource = utilitybl.getDatasetWithCondition("EVENTPURPOSE", Convert.ToInt32(strEventID), mstrConnectionString);
                rblAgenda.DataBind();
                if (strAgendaID != "")
                {
                    rblAgenda.SelectedValue = strAgendaID;
                }
            }
        }
        private void bindSegments(CheckBoxList cblSegment)
        {
            DataTable dtSegmentName;
            string strName = null;

            dtSegmentName = utilitybl.getDatasetWithCondition("BINDSUBSEGMENTS", Convert.ToInt32(hfSubId.Value), mstrConnectionString);
            for (int i = 0; i <= dtSegmentName.Rows.Count - 1; i++)
            {
                strName = dtSegmentName.Rows[i]["SSM_CSGM_ID"].ToString();
                cblSegment.Items.FindByValue(strName).Selected = true;
            }
        }

        //private void bindOwners(CheckBoxList cbOwners)
        //{
        //    DataTable dtOwnerName;
        //    string strName = null;

        //    dtOwnerName = utilitybl.getDatasetWithCondition("BINDOWNERS", Convert.ToInt32(hfSubId.Value), mstrConnectionString);
        //    for (int i = 0; i <= dtOwnerName.Rows.Count - 1; i++)
        //    {
        //        strName = dtOwnerName.Rows[i]["SMO_EM_ID"].ToString();
        //        cbOwners.Items.FindByValue(strName).Selected = true;
        //    }
        //}

        private void bindDetails(DataRow dr)
        {
            string strSelectedDept, strSelectedEvent;
            strSelectedDept = dr["SM_STM_ID"].ToString();
            strSelectedEvent = dr["SM_EM_ID"].ToString();
            DropDownList ddlPriority = (DropDownList)(fvSubmissionMaster.FindControl("ddlPriority"));
            DropDownList ddlSubType = (DropDownList)(fvSubmissionMaster.FindControl("ddlSubType"));
            //CheckBoxList cbOwners = (CheckBoxList)(fvSubmissionMaster.FindControl("cbOwners"));
            //CheckBoxList cbDeptOwner = (CheckBoxList)(fvSubmissionMaster.FindControl("cbDeptOwner"));
            CheckBoxList cblSegment = (CheckBoxList)(fvSubmissionMaster.FindControl("cblSegment"));
            DropDownList ddlEvent = (DropDownList)(fvSubmissionMaster.FindControl("ddlEvent"));
            RadioButtonList rblAssociatedWith = (RadioButtonList)(fvSubmissionMaster.FindControl("rblAssociatedWith"));
            DropDownList ddlReportDept = (DropDownList)(fvSubmissionMaster.FindControl("ddlReportDept"));
            ddlSubType.DataSource = utilitybl.getDataset("TYPE", mstrConnectionString);
            ddlSubType.DataBind();
            ddlSubType.SelectedValue = strSelectedDept;
            //cbOwners.DataSource = utilitybl.getDatasetWithCondition("OWNERS", Convert.ToInt32(strSelectedDept), mstrConnectionString);
            //cbOwners.DataBind();
            cblSegment.DataSource = utilitybl.getDataset("SUBMISSIONSGMT", mstrConnectionString);
            cblSegment.DataBind();
            ddlEvent.DataSource = utilitybl.getDataset("EVENT_EDIT", mstrConnectionString);
            ddlEvent.DataBind();
            ddlEvent.SelectedValue = strSelectedEvent;
            ddlPriority.SelectedValue = dr["SM_PRIORITY"].ToString();

            ddlReportDept.DataSource = utilitybl.getDataset("REPORTINGDEPT", mstrConnectionString);
            ddlReportDept.DataBind();
            ddlReportDept.SelectedValue = dr["SM_SRD_ID"].ToString();
            //if (!ddlReportDept.SelectedValue.Equals("0"))
            //{
            //    cbDeptOwner.DataSource = utilitybl.getDatasetWithCondition("REPORTINGOWNERS", Convert.ToInt32(ddlReportDept.SelectedValue), mstrConnectionString);
            //    cbDeptOwner.DataBind();
            //    bindDeptOwners(cbDeptOwner);
            //}
            bindSegments(cblSegment);
            //bindOwners(cbOwners);

            showhideFields();
        }

        private void bindAgenda(RadioButtonList rlbAgenda, DropDownList ddlEvent)
        {
            if (hfType.Value == "E")
            {
                rlbAgenda.Items.Clear();
                rlbAgenda.DataSource = utilitybl.getDatasetWithCondition("EVENTPURPOSE", Convert.ToInt32(ddlEvent.SelectedValue), mstrConnectionString);
                rlbAgenda.DataBind();
                // rlbAgenda.SelectedValue = hfEPId.Value;
            }
        }

        private void bindDeptOwners(CheckBoxList cbOwners)
        {
            DataTable dtOwnerName;
            string strName = null;

            dtOwnerName = utilitybl.getDatasetWithCondition("BINDREPORTINGOWNERS", Convert.ToInt32(hfSubId.Value), mstrConnectionString);
            for (int i = 0; i <= dtOwnerName.Rows.Count - 1; i++)
            {
                strName = dtOwnerName.Rows[i]["SRO_SRDOM_ID"].ToString();
                cbOwners.Items.FindByValue(strName).Selected = true;
            }
        }

        protected void ddlEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rblAssociatedWith = (RadioButtonList)fvSubmissionMaster.FindControl("rblAssociatedWith");
            rblAssociatedWith.Items.Clear();
            DropDownList ddlEvent = (DropDownList)fvSubmissionMaster.FindControl("ddlEvent");
            if (!ddlEvent.SelectedValue.Equals(""))
            {
                rblAssociatedWith.DataSource = utilitybl.getDatasetWithCondition("EVENTPURPOSE", Convert.ToInt32(ddlEvent.SelectedValue), mstrConnectionString);
                rblAssociatedWith.DataBind();
            }

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

        private void showhideFields()
        {
            try
            {
                string strFrequency, strType;
                strFrequency = ((Label)(fvSubmissionMaster.FindControl("lbFreq"))).Text;
                strType = ((RadioButtonList)(fvSubmissionMaster.FindControl("rblType"))).SelectedValue;
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
        private void writeError(string strError)
        {
            lblInfo.Text = strError;
            lblInfo.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            try
            {
                string strEffectiveDate = null, strParticulars = null, strDescription = null, strPriority = null;
                string strEscalate = null, strType = null;
                string strFrequency = null, strOnceFromDate = null, strOnceToDate = null, strFromWeekDays = null;
                string strF1Fromdate = null, strF1ToDate = null, strF2FromDate = null, strF2ToDate = null;
                string strToWeekDays = null, strMonthlyFromDate = null, strMonthlyTodate = null, strQ1fromDate = null, strQ1ToDate = null, strQ2FromDate = null, strQ2ToDate = null;
                string strQ3FromDate = null, strQ3ToDate = null, strQ4fFromDate = null, strQ4Todate = null, strFirstHalffromDate = null, strFirstHalfToDate = null, strSecondtHalffromDate = null;
                string strSecondtHalffromTo = null, strYearlyfromDate = null, strYearlyDateTo = null;
                int intSubType, intReprtFunId, intEvent = 0, intAssociatedWith = 0, intStartDays = 0, intEndDays = 0, intlevel1 = 0, intlevel2 = 0;
                int intSubmisionMasterId = Convert.ToInt32(fvSubmissionMaster.SelectedValue);
                int intRowsUpdated;


                strType = Convert.ToString(((RadioButtonList)(fvSubmissionMaster.FindControl("rblType"))).SelectedValue);

                strEffectiveDate = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtEffectiveDt"))).Text);
                intSubType = Convert.ToInt32(((DropDownList)(fvSubmissionMaster.FindControl("ddlSubType"))).SelectedValue);
                intReprtFunId = Convert.ToInt32(((DropDownList)(fvSubmissionMaster.FindControl("ddlReportDept"))).SelectedValue);

                strParticulars = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtPerticulars"))).Text);
                strDescription = Convert.ToString(((F2FTextBox)(fvSubmissionMaster.FindControl("txtDescription"))).Text);
                strPriority = Convert.ToString(((DropDownList)(fvSubmissionMaster.FindControl("ddlPriority"))).SelectedValue);
                if (strType == "E")
                {
                    intEvent = Convert.ToInt32(((DropDownList)(fvSubmissionMaster.FindControl("ddlEvent"))).SelectedValue);
                    intAssociatedWith = Convert.ToInt32(((RadioButtonList)(fvSubmissionMaster.FindControl("rblAssociatedWith"))).SelectedValue);
                    intStartDays = Convert.ToInt32(((F2FTextBox)(fvSubmissionMaster.FindControl("txtStartDays"))).Text);
                    intEndDays = Convert.ToInt32(((F2FTextBox)(fvSubmissionMaster.FindControl("txtEndDays"))).Text);

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
                intlevel1 = Convert.ToInt32(((F2FTextBox)(fvSubmissionMaster.FindControl("txtlevel1"))).Text);
                intlevel2 = Convert.ToInt32(((F2FTextBox)(fvSubmissionMaster.FindControl("txtlevel2"))).Text);
                strEscalate = Convert.ToString(((RadioButtonList)(fvSubmissionMaster.FindControl("rblEscalate"))).SelectedValue);
                intRowsUpdated = SubmissionMasterBLL.UpdateSubmissionDetails(intSubmisionMasterId, "", intSubType, intReprtFunId,
                                strType, intEvent, intAssociatedWith, strParticulars, strDescription, intStartDays, intEndDays,
                                strEscalate, strFrequency, strOnceFromDate, strOnceToDate, strFromWeekDays, strToWeekDays,
                                strMonthlyFromDate, strMonthlyTodate, strQ1fromDate, strQ1ToDate, strQ2FromDate, strQ2ToDate,
                                strQ3FromDate, strQ3ToDate, strQ4fFromDate, strQ4Todate,
                                strFirstHalffromDate, strFirstHalfToDate, strSecondtHalffromDate, strSecondtHalffromTo, strYearlyfromDate, strYearlyDateTo,
                                strF1Fromdate, strF1ToDate, strF2FromDate, strF2ToDate,
                                intlevel1, intlevel2, strEffectiveDate, strPriority, Authentication.GetUserID(Page.User.Identity.Name), "4", mstrConnectionString);

                writeError("Details updated successfully");
                pnlDuedates.Visible = false;
            }
            catch (Exception ex)
            {
                writeError(ex.Message);

            }

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewSubmissions.aspx?OpType=4");
        }
        protected void btnCancel1_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewSubmissions.aspx?OpType=1");
        }
        protected void btnInactive_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            int intChecklistCount, intCount;
            intCount = areSubmissionsDone();
            if (intCount > 0)
            {
                intChecklistCount = checkedForChecklistEntries();
                writeError("there are " + intChecklistCount + " Checklist entries are available for this Compliance.Kindly delete them manually");
                pnlBack.Visible = false;
            }
            else
            {
                lblInfo.Text = "";
                UpdateStatus();
                sendMailToTrackingOnModification();
                writeError("The selected submission has been marked as Inactive.");
                txtEffectiveDt.Text = "";
                pnlInactive.Visible = false;
                pnlBack.Visible = true;
            }
        }

        private void sendMailToTrackingOnModification()
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
                int intReportingDept = 0;

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

                DropDownList ddlSubType = (DropDownList)(fvSubmissionMaster.FindControl("ddlSubType"));
                DropDownList ddlReportDept = (DropDownList)(fvSubmissionMaster.FindControl("ddlReportDept"));

                int intTrackingDept = 0;

                if (ddlSubType.SelectedValue.Equals(""))
                {
                    intTrackingDept = 0;
                }
                else
                {
                    intTrackingDept = Convert.ToInt32(ddlSubType.SelectedValue);
                }

                MailConfigBLL mailBLL = new MailConfigBLL();
                DataTable dtMailConfig = mailBLL.searchMailConfig(null, "Deactivate Submission");
                if (dtMailConfig == null || dtMailConfig.Rows.Count == 0)
                {
                    writeError("Mail Configuration is not set, could not send mail");
                    return;
                }
                DataRow drMailConfig = dtMailConfig.Rows[0];
                strSubject = drMailConfig["MCM_SUBJECT"].ToString();
                strContent = drMailConfig["MCM_CONTENT"].ToString();

                Mail mm = new Mail();

                strSubject = strSubject.Replace("%Department%", ddlSubType.SelectedItem.Text);
                //strSubject = "Submission Deactivated for " + ddlSubType.SelectedItem.Text + " Department.";
                DataTable dtEscalation = new DataTable();
                dtEscalation = utilitybl.getDatasetWithCondition("OWNERS_MAIL", intTrackingDept, mstrConnectionString);

                strTo = new string[dtEscalation.Rows.Count];
                int i = 0;
                if (strTo.GetUpperBound(0) >= 0)
                {
                    foreach (DataRow dr in dtEscalation.Rows)
                    {
                        strTo[i] = Convert.ToString(dr["EM_EMAIL"]);
                        i = i + 1;
                    }
                }

                strCC = new string[1];
                strCC[0] = "";
                //object oSLTEmail = DataServer.ExecuteScalar(" select SLT_EMAIL_ID from TBL_SUB_SLT_USERS where SLT_ID = 2");
                //strCC[0] = (oSLTEmail == null ? "" : oSLTEmail.ToString());

                //if (ddlReportDept.SelectedValue != "")
                //{
                //    intReportingDept = Convert.ToInt32(ddlReportDept.SelectedValue);
                //}
                //DataTable dt1 = utilitybl.getDatasetWithCondition("REPORTINGOWNERS_AllLevels", intReportingDept, mstrConnectionString);
                //strCC = new string[dt1.Rows.Count];
                //int j = 0;
                //if (strCC.GetUpperBound(0) >= 0)
                //{
                //    foreach (DataRow dr in dt1.Rows)
                //    {
                //        strCC[j] = Convert.ToString(dr["EmailId"]);
                //        j = j + 1;
                //    }
                //}


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

                string strSubTable = "<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>" +
                            "<th " + strTableHeaderCSS + ">Reporting Department</th>" +
                            "<th " + strTableHeaderCSS + ">Particulars</th>" +
                            "<th " + strTableHeaderCSS + ">Brief Description</th>" +
                            "<th " + strTableHeaderCSS + ">Type</th>" +
                            "<th " + strTableHeaderCSS + ">Frequency</th>";

                if (!strDueDates.Equals(""))
                {
                    strSubTable += "<th " + strTableHeaderCSS + ">Due Date</th>";
                }
                strSubTable += "</tr>" +
                "<tr><td " + strTableCellCSS + ">" + 1 + "</td>" +
                "<td " + strTableCellCSS + ">" + ddlReportDept.SelectedItem.Text + "</td>" +
                "<td " + strTableCellCSS + ">" + txtPerticulars.Text.ToString().Replace(Environment.NewLine, "<br />") + "</td>" +
                "<td " + strTableCellCSS + ">" + txtDescription.Text.ToString().Replace(Environment.NewLine, "<br />") + "</td>" +
                "<td " + strTableCellCSS + ">" + rblType.SelectedItem.Text.ToString() + "</td>" +
                "<td " + strTableCellCSS + ">" + strFrequency + "</td>";

                if (!strDueDates.Equals(""))
                {
                    strSubTable += "<td " + strTableCellCSS + ">" + strDueDates + "</td>";
                }
                strSubTable += "</table>";

                strContent = strContent.Replace("%SubmittedBy%", Getfullname(strUserName));
                strContent = strContent.Replace("%Department%", ddlSubType.SelectedItem.Text);
                strContent = strContent.Replace("%SubmissionTable%", strSubTable);
                strContent = strContent.Replace("%Footer%", ConfigurationManager.AppSettings["MailFooter"].ToString());

                mm.sendAsyncMail(strTo, strCC, null, strSubject, strContent);
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        private int checkedForChecklistEntries()
        {
            DataTable dtCount = new DataTable();
            dtCount = utilitybl.getDatasetWithTwoConditionInString("checkedForChecklistEntries", hfSubId.Value, txtEffectiveDt.Text, mstrConnectionString);
            return dtCount.Rows.Count;
        }

        private void UpdateStatus()
        {
            int intID;
            intID = SubmissionMasterBLL.inactiveSubmission(Convert.ToInt32(hfSubId.Value), txtEffectiveDt.Text, "Inactive", txtDeActivationRemarks.Text, Authentication.GetUserID(Page.User.Identity.Name), mstrConnectionString);

        }


        private int areSubmissionsDone()
        {

            DataTable dtCount = new DataTable();
            DataRow dr;
            dtCount = utilitybl.getDatasetWithTwoConditionInString("areSubmissionsDone", hfSubId.Value, txtEffectiveDt.Text, mstrConnectionString);
            dr = dtCount.Rows[0];
            return Convert.ToInt32(dr["cnt"].ToString());
        }

        protected void btnUpdateDets_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            try
            {
                int intID;
                string strEffectivedate, strParticulars = null, strDescription = null, strPriority = null;
                string strEscalate = null, strTrackedByDept = null, strReportingDept = null;
                int intSubmisionMasterId = Convert.ToInt32(fvDetails.SelectedValue);
                int intlevel1, intlevel2;
                strEffectivedate = Convert.ToString(((F2FTextBox)(fvDetails.FindControl("txtEffectDt"))).Text);
                //<Added by prajakta
                strTrackedByDept = Convert.ToString(((DropDownList)(fvDetails.FindControl("ddlEditSubType"))).SelectedValue);
                strReportingDept = Convert.ToString(((DropDownList)(fvDetails.FindControl("ddlEditReportDept"))).SelectedValue);
                //>>
                strPriority = Convert.ToString(((DropDownList)(fvDetails.FindControl("ddlPriority"))).SelectedValue);
                strParticulars = Convert.ToString(((F2FTextBox)(fvDetails.FindControl("txtPerticulars"))).Text);
                strDescription = Convert.ToString(((F2FTextBox)(fvDetails.FindControl("txtDescription"))).Text);
                strPriority = Convert.ToString(((DropDownList)(fvDetails.FindControl("ddlPriority"))).SelectedValue);
                intlevel1 = Convert.ToInt32(((F2FTextBox)(fvDetails.FindControl("txtlevel1"))).Text);
                intlevel2 = Convert.ToInt32(((F2FTextBox)(fvDetails.FindControl("txtlevel2"))).Text);
                strEscalate = Convert.ToString(((RadioButtonList)(fvDetails.FindControl("rblEscalate"))).SelectedValue);
                //added by prajakta
                //utilitybl.getDatasetWithCondition("DELETESUBOWNRES", intSubmisionMasterId, mstrConnectionString);
                //utilitybl.getDatasetWithCondition("DELETEREPORTINGOWNERS", intSubmisionMasterId, mstrConnectionString);
                //>>

                //strExec = "exec [dbo].[UpdateSubmissionDetails] " + " " + intSubmisionMasterId + ", '" + " " + "', '" + strEffectivedate + "','" + strPriority + "','" + strParticulars + "', " +
                //                "'" + strDescription + "','" + strEscalate + "', '" + strStatutary + "'," + intlevel1 + "," + intlevel2 + ",'" + "" + "','" + "" + "'," + 0 + "," + 0 + "," +
                //               " " + 0 + ",   " + 0 + ", '" + "" + "', '" + "" + "','" + "" + "', '" + "" + "', '" + "" + "'," +
                //               " '" + "" + "', '" + "" + "','" + "" + "',' " + "" + "',' " + "" + "',' " + "" + "',' " + "" + "',' " + "" + "'," +
                //               " '" + "" + "', '" + "" + "',' " + "" + "',' " + "" + "',' " + "" + "'," +
                //               " '" + "" + "', '" + "" + "','" + Page.User.Identity.Name + "',3";
                intID = SubmissionMasterBLL.saveFilingDetsInEditMode(intSubmisionMasterId, "", Convert.ToInt32(strTrackedByDept), Convert.ToInt32(strReportingDept), "", 0, 0, strParticulars, strDescription, 0, 0, strEscalate, "", "",
                                "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                                intlevel1, intlevel2, strEffectivedate, strPriority, Authentication.GetUserID(Page.User.Identity.Name), "3", mstrConnectionString, getSubmissionOwnerdt(), getSubReportingOwnersdt());

                writeError("Details updated successfully with Submission Id: " + intSubmisionMasterId);

                ((F2FTextBox)(fvDetails.FindControl("txtPerticulars"))).Text = "";
                ((F2FTextBox)(fvDetails.FindControl("txtDescription"))).Text = "";
                ((DropDownList)(fvDetails.FindControl("ddlPriority"))).SelectedIndex = -1;
                ((F2FTextBox)(fvDetails.FindControl("txtlevel1"))).Text = "";
                ((F2FTextBox)(fvDetails.FindControl("txtlevel2"))).Text = "";
                ((RadioButtonList)(fvDetails.FindControl("rblEscalate"))).SelectedIndex = -1;
                pnlDetails.Visible = false;
                btnBackToSearch.Visible = true;
                btnBackToSearch.PostBackUrl = ("./ViewSubmissions.aspx?OpType=3");
            }
            catch (Exception ex)
            {
                writeError(ex.Message);

            }
        }


        protected void btnCancel2_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewSubmissions.aspx?OpType=3");
        }

        //<< added by prajakta

        protected void ddlEditSubType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlEditSubType = (DropDownList)fvDetails.FindControl("ddlEditSubType");
            CheckBoxList cbOwners = (CheckBoxList)fvDetails.FindControl("cbOwners");
            if (!ddlEditSubType.SelectedValue.Equals(""))
            {

                cbOwners.Items.Clear();
                cbOwners.DataSource = utilitybl.getDatasetWithCondition("OWNERS", Convert.ToInt32(ddlEditSubType.SelectedValue), mstrConnectionString);
                cbOwners.DataBind();
            }
            else
            {
                cbOwners.Items.Clear();
            }
        }

        protected void ddlEditReportDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlEditReportDept = (DropDownList)fvDetails.FindControl("ddlEditReportDept");
            CheckBoxList cbDeptOwner = (CheckBoxList)fvDetails.FindControl("cbDeptOwner");
            if (!ddlEditReportDept.SelectedValue.Equals(""))
            {
                cbDeptOwner.Items.Clear();
                cbDeptOwner.DataSource = utilitybl.getDatasetWithCondition("REPORTINGOWNERS", Convert.ToInt32(ddlEditReportDept.SelectedValue), mstrConnectionString);
                cbDeptOwner.DataBind();
            }
            else
            {
                cbDeptOwner.Items.Clear();
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewSubmissions.aspx?OpType=1");
        }


        private DataTable getSubmissionOwnerdt()
        {
            //CheckBoxList cbOwners = ((CheckBoxList)(fvDetails.FindControl("cbOwners")));
            DataTable dt = new DataTable();
            DataRow dr;
            //ListItem liChkBoxListItem;
            //dt = new DataTable();

            //dt.Columns.Add(new DataColumn("OwnerId", typeof(string)));
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

        private DataTable getSubReportingOwnersdt()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            //ListItem liChkBoxListItem;
            //dt = new DataTable();
            //CheckBoxList cbDeptOwner = ((CheckBoxList)(fvDetails.FindControl("cbDeptOwner")));

            //dt.Columns.Add(new DataColumn("OwnerId", typeof(string)));
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

        //>>
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