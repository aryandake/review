using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.Certification;
using SelectPdf;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_TestViewCommonCertifications : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CertificationMasterBL certBL = new CertificationMasterBL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["Type"] != null)
                {
                    hfType.Value = Request.QueryString["Type"].ToString();
                }
                else
                {
                    writeError("You're not authorized to access this page.");
                    return;
                }

                if (hfType.Value == "1")
                {
                    lblHeader.Text = "Compliance Certificate (Historical)";
                }
                else if (hfType.Value == "2")
                {
                    lblHeader.Text = "Quarterly Joint Certification (Historical)";
                }
                else
                {
                    writeError("You're not authorized to access this page.");
                    return;
                }

                ListItem li = new ListItem();
                li.Text = "(Select)";
                li.Value = "";
                ddlQuarter.DataSource = utilBL.getDataset("CERTIFICATEQUARTER", strConnectionString);
                ddlQuarter.DataBind();
                ddlQuarter.Items.Insert(0, li);
                mvMultiView.ActiveViewIndex = 0;
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            ShowCommonCertificates();

            gvCommonCert.Columns[5].Visible = false;
            gvCommonCert.Columns[6].Visible = false;
            gvCommonCert.Columns[7].Visible = false;

            if (hfType.Value == "1")
            {
                gvCommonCert.Columns[5].Visible = true;
            }
            else if (hfType.Value == "2")
            {
                gvCommonCert.Columns[6].Visible = true;
                gvCommonCert.Columns[7].Visible = true;
            }
        }

        private void ShowCommonCertificates()
        {
            try
            {
                //<< get Designation.
                string strUser = Authentication.GetUserID(Page.User.Identity.Name);
                string strSql1 = "";
                DataTable dtCC = new DataTable();
                string strRole = hfDesignation.Value;
                string strQuarterQuery = "";
                string strQuarter = ddlQuarter.SelectedValue;
                if (!strQuarter.Equals(""))
                {
                    strQuarterQuery = "and CC_CQM_ID = " + strQuarter;
                    hfQTRId.Value = strQuarter;
                }

                if (hfType.Value == "1")
                {
                    strQuarterQuery += " and  CDM_IS_JOIN_CERTIFICATE = 'CH' ";
                }
                else if (hfType.Value == "2")
                {
                    strQuarterQuery += " and  CDM_IS_JOIN_CERTIFICATE = 'Yes' ";
                }

                strSql1 = "select *,CSM_DESC as [Status] from TBL_COMMON_CERTIFICATIONS " +
                            " inner join TBL_CERT_MAS on CERTM_ID = CC_CERTM_ID  " +
                            " inner join  TBL_CERT_DEPT_MAS on CERTM_DEPT_ID = CDM_ID and CERTM_LEVEL_ID = 3  " +
                            " inner join TBL_CERT_QUARTER_MAS on CQM_ID = CC_CQM_ID " +
                            " inner join TBL_CERT_STATUS_MAS on CSM_NAME = CC_STATUS " +
                            " AND CC_STATUS != 'D' " + strQuarterQuery;

                dtCC = F2FDatabase.F2FGetDataTable(strSql1);

                //DataTable dtCC = utilBL.getDatasetWithConditionInString("CommonCertByQuarter", strQuarterQuery, strConnectionString); 
                gvCommonCert.DataSource = dtCC;
                gvCommonCert.DataBind();
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError(exp.Message);
            }
        }

        private void ShowCertificates(string strQuarter)
        {
            try
            {
                // string strDeptName = "";// ddlSearchDeptName.SelectedValue;
                //string strQuarter = ddlQuarter.SelectedValue;
                string strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                DataTable dsViewCert = new DataTable();
                //<< Commented by Nikhil Adhalikar on 08-Dec-2011
                //dsViewCert = certBL.searchEditCertifications(intCertId, strDeptName, strQuarter,strCreateBy, strConnectionString);
                dsViewCert = utilBL.getDatasetWithConditionInString("SEARCHCERTFORACTIVEQUATER", ddlQuarter.SelectedValue, strConnectionString);
                //>>
                gvCertEdit.DataSource = dsViewCert;
                gvCertEdit.DataBind();

                if ((this.gvCertEdit.Rows.Count == 0))
                {
                    this.lblInfo.Text = "No Records found satisfying the criteria.";
                    this.lblInfo.Visible = true;
                }
                else
                {
                    this.lblInfo.Text = String.Empty;
                    this.lblInfo.Visible = false;
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

        protected void gvCommonCert_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            lblMsg.Text = "";
            string strCCId, strQuery = "";
            string strSql = "", strSql1 = "";
            DataTable dtCC = new DataTable();
            DataSet ds = new DataSet();
            DataSet dsView = new DataSet();
            try
            {
                strCCId = gvCommonCert.SelectedValue.ToString();
                strQuery = "and CC_ID = " + strCCId;

                strSql1 = "select * from TBL_COMMON_CERTIFICATIONS " +
                            " inner join TBL_CERT_QUARTER_MAS on CQM_ID = CC_CQM_ID" +
                            " AND CC_STATUS != 'D' " + strQuery;

                dtCC = F2FDatabase.F2FGetDataTable(strSql1);
                //DataTable dtCC = utilBL.getDatasetWithConditionInString("CommonCertByQuarter", strQuery, strConnectionString);
                DataRow drCC = dtCC.Rows[0];
                lblCCId.Text = drCC["CC_ID"].ToString();
                lblContent.Text = drCC["CC_CONTENT"].ToString();
                lblFromDate.Text = (Convert.ToDateTime(drCC["CQM_FROM_DATE"].ToString())).ToString("dd-MMM-yyyy");
                lblToDate.Text = (Convert.ToDateTime(drCC["CQM_TO_DATE"].ToString())).ToString("dd-MMM-yyyy");
                //lblCHRemarks.Text = drCC["CC_COMP_HEAD_REMARKS"].ToString();
                //lblCAORemarks.Text = drCC["CC_CAO_REMARKS"].ToString();
                //lblCFORemarks.Text = drCC["CC_CFO_REMARKS"].ToString();
                //lblCEORemarks.Text = drCC["CC_CEO_REMARKS"].ToString();

                lblCHRemarks.Text = drCC["CC_CCO_REMARKS"].ToString().Length > 0 ? drCC["CC_CCO_REMARKS"].ToString() : "&nbsp;";

                string strQtrId = drCC["CQM_ID"].ToString();

                string strCHSubDate = drCC["CC_COM_HEAD_SUB_DT"].ToString().Length > 0 ? drCC["CC_COM_HEAD_SUB_DT"].ToString() : "&nbsp;";
                // if(!strCHSubDate.Equals(""))
                //  lblCHSubmittedOn.Text = (Convert.ToDateTime(strCHSubDate)).ToString("dd-MMM-yyyy HH:mm:ss tt");

                string strCAOSubDate = drCC["CC_CAO_SUB_DT"].ToString();
                //if (!strCAOSubDate.Equals(""))
                //lblCAOSubmittedOn.Text = (Convert.ToDateTime(strCAOSubDate)).ToString("dd-MMM-yyyy HH:mm:ss tt");

                //string strCFOSubDate = drCC["CC_CFO_SUB_DT"].ToString();
                //if (!strCFOSubDate.Equals(""))
                //    lblCFOSubmittedOn.Text = (Convert.ToDateTime(strCFOSubDate)).ToString("dd-MMM-yyyy HH:mm:ss");

                //string strCEOSubDate = drCC["CC_CEO_SUB_DT"].ToString();
                //if (!strCEOSubDate.Equals(""))
                //    lblCEOSubmittedOn.Text = (Convert.ToDateTime(strCEOSubDate)).ToString("dd-MMM-yyyy HH:mm:ss");

                string strCCOSubDate = drCC["CC_CCO_SUB_DT"].ToString();
                if (!strCCOSubDate.Equals(""))
                    lblCHSubmittedOn.Text = (Convert.ToDateTime(strCCOSubDate)).ToString("dd-MMM-yyyy HH:mm:ss").Length > 0 ?
                        (Convert.ToDateTime(strCCOSubDate)).ToString("dd-MMM-yyyy HH:mm:ss") : "&nbsp;"; ;

                mvMultiView.ActiveViewIndex = 2;

                ShowCertificates(strQtrId);

                //<<Added By Rahuldeb on 24Sep2019 
                RegulatoryReportingDashboard rrd = new RegulatoryReportingDashboard();
                litRegulatoryFilling.Text = rrd.GetFilingsDashboard_MD_CEO_QID(strQtrId);

                //>>
                //<<Modified by Ankur Tyagi on 01Feb2024 for CR_1945
                strSql = "select TBL_CERT_EXCEPTION.*, (CDM_NAME + ' - ' + CSDM_NAME) as [CDM_NAME] FROM TBL_CERT_EXCEPTION " +
                               " inner join TBL_CERTIFICATIONS on (CE_CERT_ID = CERT_ID) " +
                               " inner join TBL_CERT_MAS on (CERT_CERTM_ID = CERTM_ID) " +
                               " inner join TBL_CERT_SUB_SUB_DEPT_MAS on CERTM_DEPT_ID = CSSDM_ID " +
                               " inner join TBL_CERT_SUB_DEPT_MAS on CSDM_ID = CSSDM_CSDM_ID " +
                               //" inner join TBL_CERT_DEPT_MAS on CSDM_CDM_ID = CDM_ID " +
                               " inner join TBL_CERT_QUARTER_MAS on (CQM_ID = CERT_CQM_ID and CQM_ID = " +
                               strQtrId + ")" +
                               " inner join TBL_CERT_DEPT_MAS on (CSDM_CDM_ID = CDM_ID)";
                //>>
                ds = F2FDatabase.F2FGetDataSet(strSql);
                Session["ExceptionDets"] = ds.Tables[0];
                //gvAllException.DataSource = utilBL.getDataset("AllExceptions", strConnectionString);
                gvAllException.DataSource = ds;
                gvAllException.DataBind();

                //if (hfType.Value == "1")
                //{
                //    trComRem.Visible = true;
                //    trComOn.Visible = true;
                //    trCFRem.Visible = false;
                //    trCFOn.Visible = false;
                //    trCERem.Visible = false;
                //    trCEOn.Visible = false;
                //}

                //if (hfType.Value == "2")
                //{
                //    trComRem.Visible = false;
                //    trComOn.Visible = false;
                //    trCFRem.Visible = true;
                //    trCFOn.Visible = true;
                //    trCERem.Visible = true;
                //    trCEOn.Visible = true;
                //}
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

        protected void gvCertEdit_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            lblMsg.Text = "";
            string strCertId;
            DataSet dsView = new DataSet();
            try
            {
                strCertId = gvCertEdit.SelectedValue.ToString();
                string strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                //<< Commented by Nikhil Adhalikar on 08-Dec-2011
                //dsView = certBL.searchEditCertifications(Convert.ToInt32(strCertId), "", "", strCreateBy, strConnectionString);
                dsView = certBL.searchCertifications(Convert.ToInt32(strCertId), "", "", strConnectionString);
                //>>
                mvMultiView.ActiveViewIndex = 1;
                //fvEditCert.ChangeMode(FormViewMode.ReadOnly);
                DataTable dtCntDr = dsView.Tables[0];
                DataRow drCntDr = dtCntDr.Rows[0];
                string strcertId = drCntDr["CERT_ID"].ToString();
                string strDeptId = drCntDr["CDM_ID"].ToString();
                //DataTable dtExc = utilBL.getDatasetWithCondition("getExceptionByCertId", Convert.ToInt32(strcertId), strConnectionString);
                //gvException.DataSource = dtExc;
                //gvException.DataBind();

                string strQuery = "select  CERT_ID, CDM_ID, " +
                        " CSSDM_ID as DeptId, CSDM_NAME  + ' - ' +  CSSDM_NAME as DeptName, CERT_SUBMITTED_REMARKS as Approval_Comments, " +
                        " CERT_SUBMITTED_ON as Approval_Date, CERT_SUBMITTED_BY as Approval_By , CERT_AUDIT_TRAIL, " +
                        " CASE WHEN CSM_DESC IS NULL THEN 'Pending for Submission' ELSE CSM_DESC end as Status, *, " +

                        " CASE " +
                        " WHEN CERT_STATUS = 'L2R'  THEN CERT_REJECTED_REMARKS_LEVEL1 " +
                        " WHEN CERT_STATUS = 'L3R'  THEN CERT_REJECTED_REMARKS_LEVEL2 " +
                        " WHEN CERT_STATUS = 'L4R'  THEN CERT_REJECTED_REMARKS_LEVEL3 " +
                        " else ' ' END as Rejection_Comments 	" +

                        " from TBL_CERT_DEPT_MAS " +
                        " inner join TBL_CERT_SUB_DEPT_MAS on CSDM_CDM_ID = CDM_ID " +
                        " inner join TBL_CERT_SUB_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID " +
                        " inner join TBL_CERT_MAS on CERTM_DEPT_ID = CSSDM_ID and CERTM_LEVEL_ID = 0 " +
                        " left outer join " +
                        " ( select * from TBL_CERTIFICATIONS " +
                        " left outer join TBL_CERT_STATUS_MAS on CERT_STATUS = CSM_NAME " +
                        " inner join TBL_CERT_QUARTER_MAS ON CERT_CQM_ID=CQM_ID  and CQM_ID = " + ddlQuarter.SelectedValue +
                        " ) AS TAB1 on CERT_CERTM_ID = CERTM_ID where 1=1 and CDM_ID = " + strDeptId;

                gvCertApproval.DataSource = new DataServer().Getdata(strQuery);
                gvCertApproval.DataBind();
                fvEditCert.DataSource = dsView;
                fvEditCert.DataBind();
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

        protected void btnViewCancel_Click(object sender, System.EventArgs e)
        {
            mvMultiView.ActiveViewIndex = 2;
            lblMsg.Text = "";
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            mvMultiView.ActiveViewIndex = 0;
            lblMsg.Text = "";
        }

        //<<Modified by Ankur Tyagi on 23Jan2024 for CR_1945
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            SqlConnection myconnection = new SqlConnection(strConnectionString);
            try
            {
                trfrmDate.Visible = false;
                trtoDate.Visible = false;
                string strHeaderTitle = "<center><b>Compliance Certificate for the Quarter " + lblFromDate.Text + " to " + lblToDate.Text + "</b></center>";
                StringBuilder sb1 = new StringBuilder();
                StringWriter tw1 = new StringWriter(sb1);
                HtmlTextWriter hw1 = new HtmlTextWriter(tw1);

                StringBuilder sb2 = new StringBuilder();
                StringWriter tw2 = new StringWriter(sb2);
                HtmlTextWriter hw2 = new HtmlTextWriter(tw2);
                gvCertEdit.Columns[0].Visible = false;
                gvCertEdit.RenderControl(hw2);
                string strHtmlFuncCert = sb2.ToString();

                trCertID.Visible = false;
                tabCertDets.RenderControl(hw1);
                string strHtmlCertDets = sb1.ToString();
                strHtmlCertDets = strHtmlCertDets.Replace("From Date:", "<b>From Date:</b>").Replace("To Date:", "<b>To Date:</b>")
                    .Replace("Certificate:", "").Replace("Remarks by Compliance Officer:", "<b>Remarks by Compliance Officer:</b><br>");

                HttpContext context = HttpContext.Current;
                string strContent = "<html>" +
                "<head>"
                + "<style type='text/css'>" + ".TableFirstCellLeft" + "{" +
                "padding: 0in 5.4pt 0in 5.4pt;border-left: solid 0.5pt #000;border-bottom: solid 0.5pt #000; " +
                "border-top: solid 0.5pt #000;border-right: solid 0.5pt #000;repeat top; " +
                "font: 12px Verdana;color:#000000;font-weight:bold;text-align:center;text-decoration:none}" +

                ".TableFirstCellRight" + "{" +
                " padding: 0in 5.4pt 0in 5.4pt;border-right: solid 0.5pt #000;border-bottom: solid 0.5pt #000;border-top:" +
                " solid 0.5pt #000; repeat top;font: 12px Verdana;color:#000000;font-weight:bold; " +
                " text-align:center;text-decoration:none}" +

                " .TableCellLeft" + "{" +
                " font: 11px Verdana;padding: 0in 5.4pt 0in 5.4pt;border-left: solid 0.5pt #000; " +
                " border-right: solid 0.5pt #000; " +
                " border-bottom: solid 0.5pt #000;color:#000000}" +

                " .TableCellRight" + "{" +
                " font: 11px Verdana;text-align:left;padding: 0in 5.4pt 0in 5.4pt;border-right: solid 0.5pt #000; " +
                " border-bottom: solid 0.5pt #000;color:#000000;}" +
                " @page Section2 {" +
                " size: 841.7pt 595.45pt; mso - page - orientation:" +
                " landscape; margin: 1.25in 1.0in 1.25in 1.0in; mso - header - margin:.5in; " +
                " mso - footer - margin:.5in; mso - paper - source:0; " +
                " }" +
                "</style></head>" +
                "<body>";
                strContent += "<div class=Section2>";
                //strContent += lblContent.Text;

                strContent += strHeaderTitle;
                strContent += "<br/><br/>" + strHtmlCertDets;

                strContent = strContent +
                           "<br/><br/><center><b>Functional Certification</b></center><center>" + strHtmlFuncCert + "</center>";

                //<<Added by Ankur Tyagi on 05Feb2024 for CR_1948
                dt = utilBL.getDatasetWithConditionInString("getAllNonComplianceChecklistusingQuarterId", ddlQuarter.SelectedValue, strConnectionString);
                if (dt.Rows.Count > 0)
                {
                    strContent += "<br/><br/>" +
                    "<center><b>Non Compliant checklist(s)</b></center>" +
                    "<table border='1' style='border-collapse:collapse;text-align:center' width='100%'> " +
                    " <tr><td class='TableFirstCellLeft'>Sr.No</td>" +
                    "<td class='TableFirstCellRight'><b>Department Name</b></td>" +
                    "<td class='TableFirstCellRight'>Reference Circular / Notification / Act</td>" +
                    "<td class='TableFirstCellRight'>Section/Clause</td>" +
                    "<td class='TableFirstCellRight'>Compliance of/Heading of Compliance checklist</td>" +
                    "<td class='TableFirstCellRight'>Description</td>" +
                    //"<td class='TableFirstCellRight'>Consequences of non Compliance</td>" +
                    //"<td class='TableFirstCellRight'>Frequency</td>" +
                    "<td class='TableFirstCellRight'>Compliance Status</td>" +
                    "<td class='TableFirstCellRight'>Remarks / Reason of non compliance</td>" +
                    "<td class='TableFirstCellRight'>Non-compliant since</td>" +
                    "<td class='TableFirstCellRight'>Action Plan</td>" +
                    //"<td class='TableFirstCellRight'>Action status</td>" +
                    "<td class='TableFirstCellRight'>Target date</td>" +
                    //"<td class='TableFirstCellRight'>File</td>" +
                    "</tr>";
                    string strTargetDate = "";
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        string NCDate = "";
                        dr = dt.Rows[i - 1];

                        if (dr["CCD_TARGET_DATE"] != null && dr["CCD_TARGET_DATE"].ToString() != "")
                        {
                            strTargetDate = Convert.ToDateTime(dr["CCD_TARGET_DATE"].ToString()).ToString("dd-MMM-yyyy");
                        }

                        if (dr["CCD_NC_SINCE_DT"] != null && dr["CCD_NC_SINCE_DT"].ToString() != "")
                        {
                            NCDate = Convert.ToDateTime(dr["CCD_NC_SINCE_DT"].ToString()).ToString("dd-MMM-yyyy");
                        }

                        strContent += "<tr> " +
                        "<td class='TableCellLeft'>" + i + "</td>" +
                        "<td class='TableCellRight'>" + dr["For_Others"].ToString() + "</td>" +
                        "<td class='TableCellRight'>" + dr["CCM_REFERENCE"].ToString() + "</td>" +
                        "<td class='TableCellRight'>" + dr["CCM_CLAUSE"].ToString() + "</td>" +
                        "<td class='TableCellRight'>" + dr["CCM_CHECK_POINTS"].ToString() + "</td>" +
                        "<td class='TableCellRight'>" + dr["CCM_PARTICULARS"].ToString() + "</td>" +
                        //"<td class='TableCellRight'>" + dr["CCM_PENALTY"].ToString() + "</td>" +
                        //"<td class='TableCellRight'>" + dr["CCM_FREQUENCY"].ToString() + "</td>" +
                        "<td class='TableCellRight'>" + dr["Compliance_Status"].ToString() + "</td>" +
                        "<td class='TableCellRight'>" + dr["CCD_REMARKS"].ToString() + "</td>" +
                        "<td class='TableCellRight'>" + NCDate.ToString() + "</td>" +
                        "<td class='TableCellRight'>" + dr["CCD_ACTION_PLAN"].ToString() + "</td>" +
                        //"<td class='TableCellRight'>" + dr["Action_Status"].ToString() + "</td>" +
                        "<td class='TableCellRight'>" + strTargetDate.ToString() + "</td>" +
                        //"<td class='TableCellRight'>" + dr["CCD_SERVER_FILENAME"].ToString() + "</td>" +
                        "</tr>";
                    }
                    strContent += "</table>";
                }
                //>>

                strContent += "<br/><br/><center><b>Compliance Deviations</b></center>";
                dt = (DataTable)Session["ExceptionDets"];
                if (dt.Rows.Count > 0)
                {
                    strContent += //"<br/><br/>" +
                    //"<center><b>Exception(s)</b></center>" +
                    "<table border='1' style='border-collapse:collapse;text-align:center' width='100%'> " +
                    " <tr><td class='TableFirstCellLeft'>Sr.No</td>" +
                    "<td class='TableFirstCellRight'>Department Name</td>" +
                    "<td class='TableFirstCellRight'>Exception Type</td>" +
                    "<td class='TableFirstCellRight'>Exception Details</td>" +
                    "<td class='TableFirstCellRight'>Root Cause Of deviation</td>" +
                    "<td class='TableFirstCellRight'>Action taken / Proposed</td>" +
                    "<td class='TableFirstCellRight'>Target Date</td>" +
                    "<td class='TableFirstCellRight'>File Name</td></tr>";
                    string strTargetDate = "";
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        dr = dt.Rows[i - 1];
                        if (dr["CE_TARGET_DATE"] != null && dr["CE_TARGET_DATE"].ToString() != "")
                        {
                            strTargetDate = Convert.ToDateTime(dr["CE_TARGET_DATE"].ToString()).ToString("dd-MMM-yyyy");
                        }
                        strContent += "<tr> " +
                        "<td class='TableCellLeft'>" + i + "</td>" +
                        "<td class='TableCellRight'>" + dr["CDM_NAME"].ToString() + "</td>" +
                        "<td class='TableCellRight'>" + dr["CE_EXCEPTION_TYPE"].ToString() + "</td>" +
                        "<td class='TableCellRight'>" + dr["CE_DETAILS"].ToString() + "</td>" +
                        "<td class='TableCellRight'>" + dr["CE_ROOT_CAUSE_OF_DEVIATION"].ToString() + "</td>" +
                        "<td class='TableCellRight'>" + dr["CE_ACTION_TAKEN"].ToString() + "</td>" +
                        "<td class='TableCellRight'>" + strTargetDate + "</td>" +
                        "<td class='TableCellRight'>" + dr["CE_CLIENT_FILE_NAME"].ToString() + "</td></tr>";
                    }
                    strContent += "</table>";
                }

                strContent += "</div></body></html>";
                context.Response.Clear();
                Response.Write(strContent);
                context.Response.ContentType = "application/ms-word";
                context.Response.AppendHeader("content-disposition", "attachment; filename=Certificate.doc");
                context.Response.End();
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in btnExport_Click :" + ex.Message);
            }
        }

        protected void btnConvertToPdf_Click(object sender, EventArgs e)
        {
            string strHostingServer = (ConfigurationManager.AppSettings["HostingServer"].ToString());
            string strFontName = ConfigurationManager.AppSettings["FontName"].ToString();
            string strHtmlDocument = "";

            trfrmDate.Visible = false;
            trtoDate.Visible = false;

            StringBuilder sb1 = new StringBuilder();
            StringWriter tw1 = new StringWriter(sb1);
            HtmlTextWriter hw1 = new HtmlTextWriter(tw1);
            trCertID.Visible = false;
            tabCertDets.RenderControl(hw1);
            string strHtmlCertDets = sb1.ToString();
            strHtmlCertDets = strHtmlCertDets.Replace("From Date:", "<b>From Date:</b>").Replace("To Date:", "<b>To Date:</b>")
                    .Replace("Certificate:", "").Replace("Remarks by Compliance Officer:", "<b>Remarks by Compliance Officer:</b><br>");

            StringBuilder sb2 = new StringBuilder();
            StringWriter tw2 = new StringWriter(sb2);
            HtmlTextWriter hw2 = new HtmlTextWriter(tw2);
            gvCertEdit.Columns[0].Visible = false;
            gvCertEdit.RenderControl(hw2);
            string strHtmlFuncCert = sb2.ToString();

            StringBuilder sb3 = new StringBuilder();
            StringWriter tw3 = new StringWriter(sb3);
            HtmlTextWriter hw3 = new HtmlTextWriter(tw3);
            //gvAllException.Columns[1].Visible = false;
            CommonCodes.PrepareGridViewForExport(gvAllException);
            gvAllException.RenderControl(hw3);
            string strHtmlExceptions = sb3.ToString();


            #region CompChklst
            string strContent = "";
            DataRow dr;
            //<<Added by Ankur Tyagi on 05Feb2024 for CR_1948
            DataTable dt = utilBL.getDatasetWithConditionInString("getAllNonComplianceChecklistusingQuarterId", ddlQuarter.SelectedValue, strConnectionString);
            if (dt.Rows.Count > 0)
            {
                strContent += "<table border='1' style='border-collapse:collapse;text-align:center' width='100%'> " +
                " <tr><td class='TableFirstCellLeft'><b>Sr.No</b></td>" +
                "<td class='TableFirstCellRight'><b>Department Name</b></td>" +
                "<td class='TableFirstCellRight'><b>Reference Circular / Notification / Act</b></td>" +
                "<td class='TableFirstCellRight'><b>Section/Clause</b></td>" +
                "<td class='TableFirstCellRight'><b>Compliance of/Heading of Compliance checklist</b></td>" +
                "<td class='TableFirstCellRight'><b>Description</b></td>" +
                //"<td class='TableFirstCellRight'>Consequences of non Compliance</td>" +
                //"<td class='TableFirstCellRight'>Frequency</td>" +
                "<td class='TableFirstCellRight'><b>Compliance Status</b></td>" +
                "<td class='TableFirstCellRight'><b>Remarks / Reason of non compliance</b></td>" +
                "<td class='TableFirstCellRight'><b>Non-compliant since</b></td>" +
                "<td class='TableFirstCellRight'><b>Action Plan</b></td>" +
                //"<td class='TableFirstCellRight'>Action status</td>" +
                "<td class='TableFirstCellRight'><b>Target date</b></td>" +
                //"<td class='TableFirstCellRight'>File</td>" +
                "</tr>";
                string strTargetDate = "";
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    string NCDate = "";
                    dr = dt.Rows[i - 1];

                    if (dr["CCD_TARGET_DATE"] != null && dr["CCD_TARGET_DATE"].ToString() != "")
                    {
                        strTargetDate = Convert.ToDateTime(dr["CCD_TARGET_DATE"].ToString()).ToString("dd-MMM-yyyy");
                    }

                    if (dr["CCD_NC_SINCE_DT"] != null && dr["CCD_NC_SINCE_DT"].ToString() != "")
                    {
                        NCDate = Convert.ToDateTime(dr["CCD_NC_SINCE_DT"].ToString()).ToString("dd-MMM-yyyy");
                    }

                    strContent += "<tr> " +
                    "<td class='TableCellLeft'>" + i + "</td>" +
                    "<td class='TableCellRight'>" + dr["For_Others"].ToString() + "</td>" +
                    "<td class='TableCellRight'>" + dr["CCM_REFERENCE"].ToString() + "</td>" +
                    "<td class='TableCellRight'>" + dr["CCM_CLAUSE"].ToString() + "</td>" +
                    "<td class='TableCellRight'>" + dr["CCM_CHECK_POINTS"].ToString() + "</td>" +
                    "<td class='TableCellRight'>" + dr["CCM_PARTICULARS"].ToString() + "</td>" +
                    //"<td class='TableCellRight'>" + dr["CCM_PENALTY"].ToString() + "</td>" +
                    //"<td class='TableCellRight'>" + dr["CCM_FREQUENCY"].ToString() + "</td>" +
                    "<td class='TableCellRight'>" + dr["Compliance_Status"].ToString() + "</td>" +
                    "<td class='TableCellRight'>" + dr["CCD_REMARKS"].ToString() + "</td>" +
                    "<td class='TableCellRight'>" + NCDate.ToString() + "</td>" +
                    "<td class='TableCellRight'>" + dr["CCD_ACTION_PLAN"].ToString() + "</td>" +
                    //"<td class='TableCellRight'>" + dr["Action_Status"].ToString() + "</td>" +
                    "<td class='TableCellRight'>" + strTargetDate.ToString() + "</td>" +
                    //"<td class='TableCellRight'>" + dr["CCD_SERVER_FILENAME"].ToString() + "</td>" +
                    "</tr>";
                }
                strContent += "</table>";
            }
            #endregion


            string strHeaderTitle = "Compliance Certificate for the Quarter " + lblFromDate.Text + " to " + lblToDate.Text;//ddlQuarter.SelectedItem.Text;

            string strHeaderStyle = "style =\"text-align:center;" +
                                   "color: #4d5355;" +
                                   "font-family:" + strFontName + ";" +
                                   "font-weight:bold;" +
                                   "font-size:18px;\"";


            strHtmlDocument = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                            "<html>" +
                            "<head><link rel='stylesheet' href='file:\\\\\\" + Server.MapPath("../../Content/css/main.css") + "' media='all' />" +
                            "</head>" +
                            "<body>" +
                            //"<center><img src='" + "file:\\\\\\" + Server.MapPath("../../Content/images/logos/Company_Logo.png") + "' height='60px'/></center><br/><br/><table  width='100%'>" +
                            "<tr><td " +
                            strHeaderStyle + ">" + strHeaderTitle + "</td></tr>" +
                            "</table><br/><div style='page-break-after: always;font-size:14px;font-family:" + strFontName + ";font-weight:normal'>" + strHtmlCertDets + "</div>";


            strHtmlDocument = strHtmlDocument +
                           "<div " + strHeaderStyle + ">" +
                           "Functional Certification</div><br/><br/><center>" + strHtmlFuncCert + "</center>";
            //<<Added by Ankur Tyagi on 05Feb2024 for CR_1948
            strHtmlDocument = strHtmlDocument +
                           "<div style='page-break-after: always'></div><div " + strHeaderStyle + ">" +
                           "Non Compliant checklist(s)</div><br/><br/><center>" + strContent + "</center>";
            //>>
            strHtmlDocument = strHtmlDocument +
                           "<div style='page-break-after: always'></div><div " + strHeaderStyle + ">" +
                           "Exception Details</div><br/><br/><center>" + strHtmlExceptions + "</center>";

            strHtmlDocument += "</body></html>";

            SelectPdf.HtmlToPdf converter = new HtmlToPdf();

            //SelectPdf.GlobalProperties.LicenseKey = ConfigurationManager.AppSettings["PdfLicenseKey"].ToString();
            converter.Options.PdfPageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), "A4", true);
            converter.Options.PdfPageOrientation = (PdfPageOrientation)Enum.Parse(
                                                    typeof(PdfPageOrientation), "Landscape", true);
            converter.Options.WebPageWidth = 1024;
            converter.Options.WebPageHeight = 0;
            string baseUrl = strHostingServer;
            //Margin is in points. 1 point is 1/72 of an inch. So for a margin of 1 inch, the point should be 72.
            converter.Options.MarginTop = 10;
            converter.Options.MarginBottom = 10;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            //
            //Footer
            //>>
            string headerUrl = Server.MapPath("~/Pdf_Footer/header.html");
            string footerUrl = Server.MapPath("~/Pdf_Footer/footer.html");
            // instantiate a html to pdf converter object
            // header settings
            converter.Options.DisplayHeader = true;
            converter.Header.DisplayOnFirstPage = true;
            converter.Header.DisplayOnOddPages = true;
            converter.Header.DisplayOnEvenPages = true;
            converter.Header.Height = 50;

            PdfHtmlSection headerHtml = new PdfHtmlSection(headerUrl);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Header.Add(headerHtml);

            // footer settings
            converter.Options.DisplayFooter = true;
            converter.Footer.DisplayOnFirstPage = true;
            converter.Footer.DisplayOnOddPages = true;
            converter.Footer.DisplayOnEvenPages = true;
            converter.Footer.Height = 50;

            PdfHtmlSection footerHtml = new PdfHtmlSection(footerUrl);
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Footer.Add(footerHtml);

            // page numbers can be added using a PdfTextSection object
            PdfTextSection text = new PdfTextSection(0, 10,
                "Page: {page_number} of {total_pages}  ",
                //new System.Drawing.Font(strFontName, 8))
                new System.Drawing.Font(strFontName, 5));
            text.HorizontalAlign = PdfTextHorizontalAlign.Right;
            converter.Footer.Add(text);
            PdfDocument doc = converter.ConvertHtmlString(strHtmlDocument, baseUrl);
            doc.Save(Response, false, "ApprovalDetails.pdf");
            doc.Close();
        }
        //>>
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}