using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.IO;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.Certification;
using Fiction2Fact.App_Code;
using System.Web.UI.WebControls;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_CommonCertification_CCO : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ToString();
        int intCId;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CertificationMasterBL certBL = new CertificationMasterBL();
        CommonMethods cm = new CommonMethods();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strFromDate = "";
                string strToDate = "";
                DataSet dsDates;
                DataSet ds = new DataSet();
                DataTable dtDates, dtStatus;
                DataRow drDates;
                DateTime dtCertDate = System.DateTime.Now;
                string strUser = Authentication.GetUserID(Page.User.Identity.Name);
                string strCEOName = "", strCEODate = "";
                Authentication auth = new Authentication();

                if (!Page.IsPostBack)
                {
                    MvCert.ActiveViewIndex = 0;

                    if (User.IsInRole("Certification_CO_User"))
                    {
                        hfDesignation.Value = "CCO";
                        strCEOName = auth.getUserFullName(Page.User.Identity.Name);
                        //strDesg = "Chief Compliance Officer";
                        strCEODate = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
                    }
                    else
                    {
                        hfDesignation.Value = "";
                    }

                    string strRole = hfDesignation.Value;
                    if (!strRole.Equals("CCO"))
                    {
                        PnlCertStatus.Visible = false;
                        lblMsg.Text = "You're not authorized to access this page.";
                        return;
                    }

                    dsDates = utilBL.getDataset("CERTQUARTERS", strConnectionString);
                    dtDates = dsDates.Tables[0];
                    if (dtDates.Rows.Count == 1)
                    {
                        drDates = dtDates.Rows[0];
                        strFromDate = (Convert.ToDateTime(drDates["CQM_FROM_DATE"].ToString())).ToString("dd-MMM-yyyy");
                        strToDate = (Convert.ToDateTime(drDates["CQM_TO_DATE"].ToString())).ToString("dd-MMM-yyyy");
                        hfQuarterEndDt.Value = strToDate.ToString();
                        hfQuarterId.Value = drDates["CQM_ID"].ToString();
                    }

                    //dtStatus = utilBL.getDataset("COMMONCERTSTATUS", strConnectionString).Tables[0];
                    dtStatus = new DataServer().Getdata("SELECT * FROM TBL_COMMON_CERTIFICATIONS " +
                                " INNER JOIN TBL_CERT_QUARTER_MAS on CQM_ID = CC_CQM_ID and CQM_STATUS = 'A' " +
                                " INNER JOIN TBL_CERT_MAS on CC_CERTM_ID = CERTM_ID and CERTM_LEVEL_ID = 3 " +
                                " INNER JOIN TBL_CERT_DEPT_MAS on CERTM_DEPT_ID = CDM_ID and CDM_IS_JOIN_CERTIFICATE = 'CH' ");

                    if (dtStatus.Rows.Count > 0)
                    {
                        string strContent1 = "";
                        DataRow drStatus = dtStatus.Rows[0];
                        string strStatus = drStatus["CC_STATUS"].ToString();
                        hfCCId.Value = drStatus["CC_ID"].ToString();
                        hfCertMId.Value = drStatus["CC_CERTM_ID"].ToString();
                        strContent1 = drStatus["CC_CONTENT"].ToString();
                        strContent1 = strContent1.Replace("~qtrStartDate", strFromDate);
                        strContent1 = strContent1.Replace("~qtrEndDate", strToDate);
                        strContent1 = strContent1.Replace("~Date", System.DateTime.Now.ToString("dd-MMM-yyyy"));

                        if (hfDesignation.Value == "CCO")
                        {
                            strContent1 = strContent1.Replace("~CCOName", strCEOName);
                            strContent1 = strContent1.Replace("~CCOSubmissiondate", strCEODate);
                        }
                        string preContent = "<div style='font-family: Trebuchet MS;'><div style='font-family: Trebuchet MS;'><div style='font-family: Trebuchet MS;'>";
                        string postContent = "</div></div></div>";
                        lblCertContents.Text = preContent + strContent1 + postContent;
                        hfQuarterId.Value = drStatus["CC_CQM_ID"].ToString();
                        string strCHRemarks = drStatus["CC_COMP_HEAD_REMARKS"].ToString();
                        string strCAORemarks = drStatus["CC_CAO_REMARKS"].ToString();
                        string strCFORemarks = drStatus["CC_CFO_REMARKS"].ToString();
                        string strCEORemarks = drStatus["CC_CEO_REMARKS"].ToString();
                        string strCCORemarks = drStatus["CC_CCO_REMARKS"].ToString();
                        string strCFOSubmitDate = "", strCEOSubmitDate = "", strCCOSubmitDate = "";

                        if (drStatus["CC_CFO_SUB_DT"] != DBNull.Value)
                        {
                            strCFOSubmitDate = Convert.ToDateTime(drStatus["CC_CFO_SUB_DT"]).ToString("dd-MMM-yyyy HH:mm:ss");
                        }

                        if (drStatus["CC_CEO_SUB_DT"] != DBNull.Value)
                        {
                            strCEOSubmitDate = Convert.ToDateTime(drStatus["CC_CEO_SUB_DT"]).ToString("dd-MMM-yyyy HH:mm:ss");
                        }

                        if (drStatus["CC_CCO_SUB_DT"] != DBNull.Value)
                        {
                            strCCOSubmitDate = Convert.ToDateTime(drStatus["CC_CCO_SUB_DT"]).ToString("dd-MMM-yyyy HH:mm:ss");
                        }

                        if (!((strRole.Equals("CCO") && strStatus.Equals("D")) ||
                            (strRole.Equals("CCO") && strStatus.Equals("PFA"))))
                        {
                            string strStatusMessage = "";
                            switch (strStatus)
                            {
                                case "PFA":
                                    strStatusMessage = "'Pending with Chief Compliance Officer(CCO) for Submission'";
                                    break;
                                case "CCCO":
                                    strStatusMessage = "'Certified by Chief Compliance Officer'";
                                    break;
                                case "CCFO":
                                    strStatusMessage = "'Certified by Chief Financial Officer(CFO)'";
                                    break;
                                case "CCEO":
                                    strStatusMessage = "'Certified by Chief Executive Officer'";
                                    break;
                            }
                            PnlCertStatus.Visible = false;
                            lblMsg.Text = "The current status of the joint certification is " + strStatusMessage
                            + ". You can " +
                            "view the same under 'View CCO Certification'.";
                            return;
                        }

                        txtRemarks.Text = strCCORemarks;

                        //if (strRole.Equals("CFO"))
                        //{
                        //    //pnlCHRemarks.Visible = true;
                        //    //lblCHRemarks.Text = strCHRemarks;
                        //    //pnlCAORemarks.Visible = true;
                        //    //lblCAORemarks.Text = strCAORemarks;
                        //    txtRemarks.Text = strCFORemarks;
                        //}
                        //else if (strRole.Equals("CEO"))
                        //{
                        //    //pnlCHRemarks.Visible = true;
                        //    //lblCHRemarks.Text = strCHRemarks;
                        //    //pnlCAORemarks.Visible = true;
                        //    //lblCAORemarks.Text = strCAORemarks;
                        //    pnlCFORemarks.Visible = true;
                        //    lblCFORemarks.Text = strCFORemarks;
                        //    txtRemarks.Text = strCEORemarks;
                        //    lblSubmittedOn.Text = strCFOSubmitDate;
                        //}

                        ShowIndividualCerts();
                    }
                    //<<Creating the certification for the first time.
                    else
                    {
                        //if (!(strRole.Equals("CH")))
                        //{
                        PnlCertStatus.Visible = false;
                        lblMsg.Text = "The joint Certification is yet to be rolled out.";
                        return;
                        //}
                    }

                    DataTable dtExcetion = new DataTable();

                    //gvAllException.DataSource = utilBL.getDataset("AllExceptions", strConnectionString);
                    dtExcetion = new DataServer().Getdata(" select *, " +
                                    " CDM_NAME + ' - ' + CSDM_NAME as [FHDept], " +
                                    " CSDM_NAME as [UHDept] " +
                                    " from [TBL_CERT_EXCEPTION] " +
                                    " inner join [TBL_CERTIFICATIONS] on [CERT_ID] = [CE_CERT_ID] " +
                                    " inner join TBL_CERT_MAS on CERT_CERTM_ID = CERTM_ID " +
                                    " inner join TBL_CERT_SUB_SUB_DEPT_MAS on CERTM_DEPT_ID = CSSDM_ID " +
                                    " inner join TBL_CERT_SUB_DEPT_MAS on CSDM_ID = CSSDM_CSDM_ID " +
                                    " inner join TBL_CERT_DEPT_MAS on CDM_ID = CSDM_CDM_ID " +
                                    " where CERT_CQM_ID =  " + hfQuarterId.Value);
                    gvAllException.DataSource = dtExcetion;
                    gvAllException.DataBind();
                    Session["AllExceptions"] = dtExcetion;

                    foreach (GridViewRow gr in gvAllException.Rows)
                    {
                        Label txtCE_CLOSURE_STATUS = (Label)gr.FindControl("txtCE_CLOSURE_STATUS");
                        Label txtTargetDate = (Label)gr.FindControl("txtTargetDate");

                        HiddenField hfTargetDate = (HiddenField)gr.FindControl("hfTargetDate");
                        HiddenField hfClosureDate = (HiddenField)gr.FindControl("hfClosureDate");

                        if (txtCE_CLOSURE_STATUS.Text != "Closed")
                        {
                            if (!string.IsNullOrEmpty(hfTargetDate.Value))
                            {
                                txtTargetDate.Text = hfTargetDate.Value;
                            }
                            else
                            {
                                txtTargetDate.Text = "";
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(hfClosureDate.Value))
                            {
                                txtTargetDate.Text = hfClosureDate.Value;
                            }
                            else
                            {
                                txtTargetDate.Text = "";
                            }
                        }
                    }

                    //<<Commented And Added by Rahuldeb on 24Sep2019
                    RegulatoryReportingDashboard rrd = new RegulatoryReportingDashboard();
                    litRegulatoryFilling.Text = rrd.GetFilingsDashboard_MD_CEO_AQ();
                    //GetFilingsDashboard();
                    //>>
                    getCountWiseReports();
                }
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError(ex.Message.ToString());
            }
        }

        private void ShowIndividualCerts()
        {
            try
            {
                DataTable dtView = new DataTable();
                string strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                DataSet dsViewCert = new DataSet();

                dsViewCert = utilBL.getDataset("SEARCHCERTFORACTIVEQUATER", strConnectionString);
                gvCertView.DataSource = dsViewCert;
                gvCertView.DataBind();

                if (gvCertView.Rows.Count == 0)
                {
                    lblGrid.Text = "No Records found satisfying the criteria.";
                    lblGrid.Visible = true;

                }
                else
                {
                    lblGrid.Text = String.Empty;
                    lblGrid.Visible = false;
                }
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError(exp.Message);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void gvCertView_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            lblMsg.Text = "";
            DataRow dr;
            string strCertId;
            DataSet dsView = new DataSet();
            DataTable dtView = new DataTable();
            try
            {
                strCertId = gvCertView.SelectedValue.ToString();
                string strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                dtView = utilBL.getDatasetWithCondition("GETCOMMONCERTIFICATIONS", Convert.ToInt32(strCertId), strConnectionString);
                dsView = certBL.searchCertifications(Convert.ToInt32(strCertId), "", "", strConnectionString);
                if (dsView.Tables.Count > 0)
                {
                    dtView = dsView.Tables[0];
                }
                dr = dtView.Rows[0];
                MvCert.ActiveViewIndex = 1;
                lblCId.Text = dr["CERT_ID"].ToString();
                lblDept.Text = dr["CDM_NAME"].ToString();
                lblContent.Text = dr["CERT_CONTENT"].ToString();
                lblRmks.Text = dr["CERT_APPROVED_REMARKS_LEVEL3"].ToString() == "" ? "&nbsp;" : dr["CERT_APPROVED_REMARKS_LEVEL3"].ToString();
                lblSubmittedBy.Text = dr["CERT_APPROVED_BY_LEVEL3"].ToString();
                if (dr["CERT_APPROVED_DT_LEVEL3"] != DBNull.Value)
                {
                    lblSubmittedDt.Text = (Convert.ToDateTime(dr["CERT_APPROVED_DT_LEVEL3"].ToString())).ToString("dd-MMM-yyyy HH:mm:ss tt");
                }
                string strcertId = dr["CERT_ID"].ToString();
                string strDeptId = dr["CDM_ID"].ToString();
                //DataTable dtExc = utilBL.getDatasetWithCondition("getExceptionByCertId", Convert.ToInt32(strcertId), strConnectionString);
                //gvException.DataSource = dtExc;
                //gvException.DataBind();

                gvCertApproval.DataSource = certBL.getCertificationsApproval(strDeptId, "L4", Page.User.Identity.Name.ToString(),
                strConnectionString);
                gvCertApproval.DataBind();
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                this.lblMsg.Text = ex.Message;
                this.lblMsg.Visible = true;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            SaveDetails("S");
            sendCertificateMail(intCId, lblCertContents.Text, " accepted ");
        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            SaveDetails("D");
        }

        protected void SaveDetails(string strStatus)
        {
            try
            {
                int intCCId = 0;
                string strContent;
                string strRemarks;
                string strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                strContent = lblCertContents.Text;
                strRemarks = cm.getSanitizedString(txtRemarks.Text);
                string strRole = hfDesignation.Value;
                if (!hfCCId.Value.Equals(""))
                {
                    intCCId = Convert.ToInt32(hfCCId.Value);
                }
                intCId = certBL.saveCommonCertification(intCCId, Convert.ToInt32(hfCertMId.Value),
                                Convert.ToInt32(hfQuarterId.Value),
                                strContent, strRemarks, strStatus, strRole, strCreateBy, strConnectionString);

                if (strStatus.Equals("D"))
                    writeError("Compliance Certification saved successfully.");
                else if (strStatus.Equals("S"))
                {
                    writeError("Compliance Certification submitted successfully.");
                    PnlCertStatus.Visible = false;
                }

            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Error in btnAccept_Click: " + exp.Message);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            MvCert.ActiveViewIndex = 0;
        }

        //<< Added by Nikhil Adhalikar on 07-Dec-2011 for sending mails to next level Designation.
        private void sendCertificateMail(int intCId, string strContent, string strStatus)
        {
            try
            {
                string[] strUsers = new string[0];
                string[] strTo = new string[1];
                string[] strCC = new string[0];
                string strHostingServer, strFooter;
                DataTable dtNextDesg = new DataTable();
                DateTime dt = System.DateTime.Now;
                Mail mm = new Mail();
                strHostingServer = ConfigurationManager.AppSettings["HostingServer"].ToString();
                strFooter = ConfigurationManager.AppSettings["MailFooter"].ToString();
                CommonCode cc = new CommonCode();
                Authentication auth = new Authentication();
                //strFullDesg = "Chief Compliance Officer (CCO)";
                //<<Modified by Ankur Tyagi on 29Mar2024 for CR_2005
                cc.ParamMap.Add("To", "CertAdmin");
                cc.ParamMap.Add("cc", "CCO");
                //>>
                cc.ParamMap.Add("ConfigId", 22);
                cc.ParamMap.Add("DESG", "Compliance Officer");

                cc.ParamMap.Add("SubmittedBy", (new Authentication()).getUserFullName(Page.User.Identity.Name));
                cc.ParamMap.Add("Quarter", hfQuarterEndDt.Value);
                cc.setCertificationMailContent();

                #region Commented Code
                //strSubject = "Joint Certification approved by " + strFullDesg + " for the quarter ended " + hfQuarterEndDt.Value.ToString();

                //if (!strNextLevelDesg.Equals(""))
                //{
                //    dtNextDesg = utilBL.getDatasetWithConditionInString("getApprovalDets", strNextLevelDesg,
                //                    strConnectionString);
                //    if (dtNextDesg.Rows.Count > 0)
                //    {
                //        drNextDesg = dtNextDesg.Rows[0];
                //        hfEmailId.Value = drNextDesg["CDO_EMAIL_ID"].ToString();
                //        strTo[0] = hfEmailId.Value.ToString();
                //    }
                //}

                //strUsers = Roles.GetUsersInRole("Certification_Admin");
                //strCC = new string[strUsers.Length];

                //for (int intCount2 = 0; intCount2 < strUsers.Length; intCount2++)
                //{
                //    user = Membership.GetUser(strUsers[intCount2]);
                //    strCC[intCount2] = user.Email;
                //}
                ////+ Authentication.GetUserID(Page.User.Identity.Name) 

                //if (!strNextLevelDesg.Equals(""))
                //{
                //    strMailContent = "<html><head><title>Certification Approval</title></head> " +
                //                  "<body style=\"font-size: 10pt; font-family: Verdana\"> <br /> The Certificate " +
                //                  " has been Approved on " + dt.ToString("MMM dd,yyyy") + " at " +
                //                  dt.ToString("hh.mm tt") +
                //                   " by " + strFullDesg + 
                //                   ".<br />" + "<br />It is now pending for your approval. To view and approve the certification, please click on the below mentioned link: " + 
                //                   " <br/> " +
                //                   "<a href ="+ strHostingServer + "/Certification/CommonCertification.aspx> " +
                //                   "Click here</a> (URL: " + strHostingServer + "/Certification/CommonCertification.aspx)<br>" +
                //                   strFooter + "<br />THIS IS AN AUTO GENERATED MAIL PLEASE DO NOT REPLY BACK.<br /></body></html>";

                //    mm.sendAsyncMail(strTo, strCC, null, strSubject, strMailContent);
                //}
                //else
                //{
                //    strMailContent = "<html><head><title>Certification Approval</title></head> " +
                //                 "<body style=\"font-size: 10pt; font-family: Verdana\"> <br /> The Joint Certification " +
                //                 " has been Approved on " + dt.ToString("MMM dd,yyyy") + " at " +
                //                 dt.ToString("hh.mm tt") +
                //                  " by " + strFullDesg +
                //                  ".<br />" + "<br />To view the certification, please click on the below mentioned link: " +
                //                  "<br/> " +
                //                  "<a href =" + strHostingServer + "/Certification/ViewCommonCertifications.aspx> " +
                //                  "Click here</a> (URL: " + strHostingServer + "/Certification/ViewCommonCertifications.aspx)<br>" +
                //                  strFooter + "<br />THIS IS AN AUTO GENERATED MAIL PLEASE DO NOT REPLY BACK.<br /></body></html>";

                //    mm.sendAsyncMail(strCC, null, null, strSubject, strMailContent);
                //}
                #endregion
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Error while sending mail: " + ex.Message);
            }
        }

        private void getCountWiseReports()
        {
            string strHTMLReport = "", strQuarterIdForTotalRow = "";
            DataTable dtCertifyingDepts = new DataTable();
            DataTable dtQuarters = new DataTable();

            DataRow dr1, dr2, dr3, dr4, dr5;
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            DataTable dt5 = new DataTable();

            string strQry1, strQry2, strQry3, strQry4, strQry5;

            string strDeptId = "", strQuarterId = "", strSelectedQuarterId = "";
            string strDeptName = "", strQuarter = "", strSubQuery = "";
            string strDeptSQL = "", strQuartersSQL = "";
            int intCompliant = 0, intNonCompliant = 0, intNotYetApplicable = 0, intWorkInProgress = 0;
            int intTotalCompliant = 0, intTotalNonCompliant = 0, intTotalNotYetApplicable = 0,
                intTotalWorkInProgress = 0;
            int intRowWiseTotal = 0, intGrandTotal = 0;
            try
            {
                strHTMLReport = strHTMLReport
                      + "<table width='100%' cellpadding='0' cellspacing='0'>" +
                      "<tr><td class='DBTableTopHeader'>Certification count wise reports</td></tr>" +
                      "</table><br/>";

                strHTMLReport = strHTMLReport + "<table width='100%'class='table table-bordered footable' cellpadding='0' cellspacing='0'><tr>" +

                    "<th>Function/Unit/Sub-Unit Name</td>" +

                    "<th>Quarter Name</td>" +

                    "<th>Compliant</td>" +

                    "<th>Not Compliant</td>" +

                    "<th>Not yet applicable</td>" +

                    "<th>Work in progress</td>" +

                    "<th>Total</td>" +

                    "</tr>";
                //

                strDeptSQL = "SELECT CDM_ID as DeptId, CDM_NAME as DeptName FROM TBL_CERT_DEPT_MAS where isnull(CDM_IS_JOIN_CERTIFICATE,'') != 'Yes' ";


                strQuartersSQL = "SELECT CQM_ID, replace(convert(varchar, CQM_FROM_DATE, 106), ' ', '-') + ' to ' + " +
                                " replace(convert(varchar, CQM_TO_DATE, 106), ' ', '-') as Quarter " +
                                " FROM TBL_CERT_QUARTER_MAS ";

                strSelectedQuarterId = hfQuarterId.Value;

                if (!strSelectedQuarterId.Equals(""))
                {
                    strQuartersSQL = strQuartersSQL + " where CQM_ID = " + strSelectedQuarterId;
                }
                strQuartersSQL = strQuartersSQL + " order by CQM_ID";

                dtCertifyingDepts = F2FDatabase.F2FGetDataTable(strDeptSQL);
                dtQuarters = F2FDatabase.F2FGetDataTable(strQuartersSQL);

                for (int intCnt1 = 0; intCnt1 < dtCertifyingDepts.Rows.Count; intCnt1++)
                {
                    strDeptId = dtCertifyingDepts.Rows[intCnt1]["DeptId"].ToString();
                    strDeptName = dtCertifyingDepts.Rows[intCnt1]["DeptName"].ToString();

                    for (int intCnt2 = 0; intCnt2 < dtQuarters.Rows.Count; intCnt2++)
                    {
                        strQuarterId = dtQuarters.Rows[intCnt2]["CQM_ID"].ToString();
                        strQuarter = dtQuarters.Rows[intCnt2]["Quarter"].ToString();

                        //Compliant
                        strQry1 = "select count(*) as [Count] " +
                                    " from TBL_CERT_CHECKLIST_DETS " +
                                    " INNER JOIN TBL_CERT_CHECKLIST_MAS ON CCD_CCM_ID = CCM_ID " +    //added by Hari on 12 Oct 2016
                                    " INNER JOIN TBL_CERTIFICATIONS ON CCD_CERT_ID = CERT_ID " +
                                    " and CERT_CQM_ID = " + strQuarterId +
                                    " and CCD_YES_NO_NA = 'C'" +
                                    " inner join TBL_CERT_MAS ON CERTM_ID = CERT_CERTM_ID " +
                                    " inner join TBL_CERT_SUB_SUB_DEPT_MAS ON CERTM_DEPT_ID = CSSDM_ID " +
                                    " inner join TBL_CERT_SUB_DEPT_MAS ON CSSDM_CSDM_ID = CSDM_ID " +
                                    " inner join TBL_CERT_DEPT_MAS ON CSDM_CDM_ID = CDM_ID ";

                        //Not Compliant
                        strQry2 = "select count(*) as [Count] " +
                                " from TBL_CERT_CHECKLIST_DETS " +
                                " INNER JOIN TBL_CERT_CHECKLIST_MAS ON CCD_CCM_ID = CCM_ID " +  //added by Hari on 12 Oct 2016
                                " INNER JOIN TBL_CERTIFICATIONS ON CCD_CERT_ID = CERT_ID " +
                                " and CERT_CQM_ID = " + strQuarterId +
                                " and CCD_YES_NO_NA = 'N'" +
                                " inner join TBL_CERT_MAS ON CERTM_ID = CERT_CERTM_ID " +
                                " inner join TBL_CERT_SUB_SUB_DEPT_MAS ON CERTM_DEPT_ID = CSSDM_ID " +
                                " inner join TBL_CERT_SUB_DEPT_MAS ON CSSDM_CSDM_ID = CSDM_ID " +
                                " inner join TBL_CERT_DEPT_MAS ON CSDM_CDM_ID = CDM_ID ";


                        //Not yet applicable
                        strQry3 = "select count(*) as [Count] " +
                                " from TBL_CERT_CHECKLIST_DETS " +
                                " INNER JOIN TBL_CERT_CHECKLIST_MAS ON CCD_CCM_ID = CCM_ID " + //added by Hari on 12 Oct 2016
                                " INNER JOIN TBL_CERTIFICATIONS ON CCD_CERT_ID = CERT_ID " +
                                " and CERT_CQM_ID = " + strQuarterId +
                                " and CCD_YES_NO_NA = 'NA'" +
                                " inner join TBL_CERT_MAS ON CERTM_ID = CERT_CERTM_ID " +
                                " inner join TBL_CERT_SUB_SUB_DEPT_MAS ON CERTM_DEPT_ID = CSSDM_ID " +
                                " inner join TBL_CERT_SUB_DEPT_MAS ON CSSDM_CSDM_ID = CSDM_ID " +
                                " inner join TBL_CERT_DEPT_MAS ON CSDM_CDM_ID = CDM_ID ";

                        //Work in progress
                        strQry4 = "select count(*) as [Count] " +
                                " from TBL_CERT_CHECKLIST_DETS " +
                                " INNER JOIN TBL_CERT_CHECKLIST_MAS ON CCD_CCM_ID = CCM_ID " + //added by Hari on 12 Oct 2016
                                " INNER JOIN TBL_CERTIFICATIONS ON CCD_CERT_ID = CERT_ID " +
                                " and CERT_CQM_ID = " + strQuarterId +
                                " and CCD_YES_NO_NA = 'W'" +
                                " inner join TBL_CERT_MAS ON CERTM_ID = CERT_CERTM_ID " +
                                " inner join TBL_CERT_SUB_SUB_DEPT_MAS ON CERTM_DEPT_ID = CSSDM_ID " +
                                " inner join TBL_CERT_SUB_DEPT_MAS ON CSSDM_CSDM_ID = CSDM_ID " +
                                " inner join TBL_CERT_DEPT_MAS ON CSDM_CDM_ID = CDM_ID ";

                        strQry5 = "select count(*) as [Count] " +
                                " from TBL_CERT_CHECKLIST_DETS " +
                                " INNER JOIN TBL_CERT_CHECKLIST_MAS ON CCD_CCM_ID = CCM_ID " + //added by Hari on 12 Oct 2016
                                " INNER JOIN TBL_CERTIFICATIONS ON CCD_CERT_ID = CERT_ID " +
                                " and CERT_CQM_ID = " + strQuarterId +
                                //" and CCD_YES_NO_NA = 'W'" +
                                " inner join TBL_CERT_MAS ON CERTM_ID = CERT_CERTM_ID " +
                                " inner join TBL_CERT_SUB_SUB_DEPT_MAS ON CERTM_DEPT_ID = CSSDM_ID " +
                                " inner join TBL_CERT_SUB_DEPT_MAS ON CSSDM_CSDM_ID = CSDM_ID " +
                                " inner join TBL_CERT_DEPT_MAS ON CSDM_CDM_ID = CDM_ID ";


                        strSubQuery = " and  CDM_ID  = " + strDeptId;
                        dt1 = new DataTable();
                        dt1 = F2FDatabase.F2FGetDataTable(strQry1 + strSubQuery);
                        for (int intCnt = 0; intCnt < dt1.Rows.Count; intCnt++)
                        {
                            dr1 = dt1.Rows[intCnt];

                            intCompliant = Convert.ToInt32(dr1["Count"]);
                            intTotalCompliant = intTotalCompliant + intCompliant;
                        }
                        dt2 = new DataTable();
                        dt2 = F2FDatabase.F2FGetDataTable(strQry2 + strSubQuery);
                        for (int intCnt = 0; intCnt < dt2.Rows.Count; intCnt++)
                        {
                            dr2 = dt2.Rows[intCnt];

                            intNonCompliant = Convert.ToInt32(dr2["Count"]);
                            intTotalNonCompliant = intTotalNonCompliant + intNonCompliant;
                        }
                        dt3 = new DataTable();
                        dt3 = F2FDatabase.F2FGetDataTable(strQry3 + strSubQuery);
                        for (int intCnt = 0; intCnt < dt3.Rows.Count; intCnt++)
                        {
                            dr3 = dt3.Rows[intCnt];

                            intNotYetApplicable = Convert.ToInt32(dr3["Count"]);
                            intTotalNotYetApplicable = intTotalNotYetApplicable + intNotYetApplicable;
                        }
                        dt4 = new DataTable();
                        dt4 = F2FDatabase.F2FGetDataTable(strQry4 + strSubQuery);
                        for (int intCnt = 0; intCnt < dt4.Rows.Count; intCnt++)
                        {
                            dr4 = dt4.Rows[intCnt];

                            intWorkInProgress = Convert.ToInt32(dr4["Count"]);
                            intTotalWorkInProgress = intTotalWorkInProgress + intWorkInProgress;
                        }
                        dt5 = new DataTable();
                        dt5 = F2FDatabase.F2FGetDataTable(strQry5 + strSubQuery);
                        for (int intCnt = 0; intCnt < dt5.Rows.Count; intCnt++)
                        {
                            dr5 = dt5.Rows[intCnt];

                            intRowWiseTotal = Convert.ToInt32(dr5["Count"]);
                            intGrandTotal = intGrandTotal + intRowWiseTotal;
                        }

                        //intRowWiseTotal = intCompliant + intNonCompliant + intNotYetApplicable + intWorkInProgress;

                        strHTMLReport = strHTMLReport +
                            "<tr><td class='DBTableCellLeft'>" + strDeptName + "</td>" +
                            "<td class='DBTableCellRight'>" + strQuarter + "</td>" +
                            "<td class='DBTableCellRight'>" +
                            "<a class='badge rounded-pill badge-soft-pink' href='#' onclick=\"window.open(" +
                            "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=1&QtrId=" + strQuarterId +
                            "&DeptId=" + strDeptId + "&Type=C'," +
                            "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750,resizable=1');return false;\">" +
                            intCompliant + "</a>" +
                            "</td>" +

                            "<td class='DBTableCellRight'>" +
                            "<a class='badge rounded-pill badge-soft-pink' href='#' onclick=\"window.open(" +
                            "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=1&QtrId=" + strQuarterId +
                            "&DeptId=" + strDeptId + "&Type=N'," +
                            "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750,resizable=1');return false;\">" +
                            intNonCompliant + "</a>" +
                            "</td>" +

                            "<td class='DBTableCellRight'>" +
                            "<a class='badge rounded-pill badge-soft-pink' href='#' onclick=\"window.open(" +
                            "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=1&QtrId=" + strQuarterId +
                            "&DeptId=" + strDeptId + "&Type=NA'," +
                            "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750 ,resizable=1');return false;\">" +
                            intNotYetApplicable + "</a>" +
                            "</td>" +

                            "<td class='DBTableCellRight'>" +
                            "<a class='badge rounded-pill badge-soft-pink' href='#' onclick=\"window.open(" +
                            "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=1&QtrId=" + strQuarterId +
                            "&DeptId=" + strDeptId + "&Type=W'," +
                            "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750 ,resizable=1');return false;\">" +
                            intWorkInProgress + "</a>" +
                            "</td>" +

                            "<td class='DBTableCellRight'>" +
                            "<a class='badge rounded-pill badge-soft-pink' href='#' onclick=\"window.open(" +
                            "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=1&QtrId=" + strQuarterId +
                            "&DeptId=" + strDeptId + "&Type=RWT'," +
                            "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750 ,resizable=1');return false;\">" +
                            intRowWiseTotal + "</a>" +
                            "</td>" +

                            "</tr>";
                    }
                }

                //Total row at the end.
                //intGrandTotal = intTotalCompliant + intTotalNonCompliant + intTotalNotYetApplicable + intTotalWorkInProgress;

                if (strSelectedQuarterId.Equals(""))
                {
                    strQuarterIdForTotalRow = "&QtrId=0";
                }
                else
                {
                    strQuarterIdForTotalRow = "&QtrId=" + strSelectedQuarterId;
                }

                strHTMLReport = strHTMLReport +
                        "<tr><td class='DBTableCellLeft' colspan='2'>Total</td>" +

                        "<td class='DBTableCellRight'>" +
                        "<a class='badge rounded-pill badge-soft-pink' href='#' onclick=\"window.open(" +
                        "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=1" + strQuarterIdForTotalRow +
                        "&DeptId=0" + "&Type=C'," +
                        "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750 ,resizable=1');return false;\">" +
                        intTotalCompliant + "</a>" +
                        "</td>" +

                        "<td class='DBTableCellRight'>" +
                        "<a class='badge rounded-pill badge-soft-pink' href='#' onclick=\"window.open(" +
                                                //"'CertDashboardDets.aspx?DeptName="+strDeptName + "&ReportType=" + ddlDeptType.SelectedValue + "&QtrId=0" +
                                                "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=1" + strQuarterIdForTotalRow +
                        "&DeptId=0" + "&Type=N'," +
                        "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750 ,resizable=1');return false;\">" +
                        intTotalNonCompliant + "</a>" +
                        "</td>" +

                        "<td class='DBTableCellRight'>" +
                        "<a class='badge rounded-pill badge-soft-pink' href='#' onclick=\"window.open(" +
                        "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=1" + strQuarterIdForTotalRow +
                        "&DeptId=0" + "&Type=NA'," +
                        "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750,resizable=1');return false;\">" +
                        intTotalNotYetApplicable + "</a>" +
                        "</td>" +

                        "<td class='DBTableCellRight'>" +
                        "<a class='badge rounded-pill badge-soft-pink' href='#' onclick=\"window.open(" +
                        "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=1" + strQuarterIdForTotalRow +
                        "&DeptId=0" + "&Type=W'," +
                        "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750 ,resizable=1');return false;\">" +
                        intTotalWorkInProgress + "</a>" +
                        "</td>" +

                        "<td class='DBTableCellRight'>" +
                        "<a class='badge rounded-pill badge-soft-pink' href='#' onclick=\"window.open(" +
                        "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=1" + strQuarterIdForTotalRow +
                        "&DeptId=0" + "&Type=GrandTotal'," +
                        "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750 ,resizable=1');return false;\">" +
                        intGrandTotal + "</a>" +
                        "</td>" +

                        "</tr>";

                strHTMLReport = strHTMLReport + "</table></BODY></HTML>";
                litSummary.Text = strHTMLReport;
                Session["CertDashboard"] = strHTMLReport;
            }
            catch (System.Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                throw new System.Exception("system exception in showDashboard(): " + ex);
            }
        }

        protected void btnExportToExcel_Click(object sender, System.EventArgs e)
        {
            gvAllException.DataSource = (DataTable)(Session["AllExceptions"]);
            gvAllException.DataBind();
            CommonCodes.PrepareGridViewForExport(gvAllException);
            string attachment = "attachment; filename=ExceptionDetails.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvAllException.RenderControl(htw);

            string html2 = Regex.Replace(sw.ToString(), @"(<input type=""image""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<input class=""checkbox""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<a \/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            Response.Write(html2.ToString());

            //Response.Write(sw.ToString());
            Response.End();
            gvAllException.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

    }
}