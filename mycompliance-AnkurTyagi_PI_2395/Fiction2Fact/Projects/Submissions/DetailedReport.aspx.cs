using System;
using System.Data;
using System.Web.UI;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class Submissions_DetailedReport : System.Web.UI.Page
    {
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();
        protected void Page_Load(object sender, EventArgs e)
        {
            //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
            if (Page.User.Identity.Name.Equals(""))
            {
                Response.Redirect(Global.site_url("Login.aspx"));
                return;
            }
            //>>

            string mstrConnectionString = null;
            if (!Page.IsPostBack)
            {
                try
                {
                    getdetailsReport(mstrConnectionString);
                }
                catch (Exception ex)
                {
                    writeError("Invalid Parameter input." + ex);
                }
            }
        }

        //<<Added by Ankur Tyagi on 08-Apr-2024 for CR_2041
        public string ReturnCutoffFilterQry()
        {
            string retQry = "";
            string strCutOffDate = F2FDatabase.F2FExecuteScalar(" select CP_VALUE from TBL_CONFIG_PARAMS where CP_NAME = 'Dashboard count Cut-off date'").ToString();
            string strCutOffParam = F2FDatabase.F2FExecuteScalar(" select CP_VALUE from TBL_CONFIG_PARAMS where CP_NAME = 'Filings Cut-off param'").ToString();

            if (!string.IsNullOrEmpty(strCutOffDate))
            {
                retQry = retQry + " AND " + strCutOffParam + " >= '" + strCutOffDate + "' ";
            }
            return retQry;
        }
        //>>

        //<<Added by Ankur Tyagi on 08-Apr-2024 for CR_2025
        public string ReturnRoleBasedQry()
        {
            string strUsername = "", strEmailId = "", strUserRole = "";
            string retQry = "";

            if (!string.IsNullOrEmpty(Request.QueryString["UName"]) & !string.IsNullOrEmpty(Request.QueryString["UMail"]) &
                 !string.IsNullOrEmpty(Request.QueryString["Role"]))
            {
                strUsername = Request.QueryString["UName"];
                strEmailId = Request.QueryString["UMail"];
                strUserRole = Request.QueryString["Role"];

                strUsername = encdec.Decrypt(strUsername);
                strEmailId = encdec.Decrypt(strEmailId);
                strUserRole = encdec.Decrypt(strUserRole);

                if (strUserRole == "Admin")
                {
                    retQry = " INNER JOIN TBL_EM_STM_MAPPING ON SC_STM_ID = ESM_STM_ID " +
                        "INNER JOIN EmployeeMaster ON ESM_EM_ID = EM_ID AND EM_USERNAME = '" + strUsername + "' AND EM_EMAIL = '" + strEmailId + "'";
                }
                else if (strUserRole == "Other")
                {
                    retQry = " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                        "INNER JOIN TBL_SUB_SRD_OWNER_MASTER ON SRD_ID = SRDOM_SRD_ID AND SRDOM_EMP_ID = '" + strUsername + "' AND SRDOM_EMAILID = '" + strEmailId + "'";
                }
            }
            return retQry;
        }
        //>>

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page != null)
            {
                Page.RegisterRequiresViewStateEncryption();
            }
        }

        private void writeError(string strError)
        {

        }

        public void getdetailsReport(string strConnectionString)
        {
            string strReportTypeId = "";
            DataTable dt = new DataTable();
            string strHtmlTable1 = "";
            strHtmlTable1 = "<table width='100%' border='1' class='table table-bordered footable' >";


            if (!string.IsNullOrEmpty(Request.QueryString["ReportType"]))
            {
                strReportTypeId = Request.QueryString["ReportType"].ToString();
                hfReportType.Value = strReportTypeId;
            }

            if (strReportTypeId.Equals("2"))
            {
                getDetailedComplianceWiseStatus(strConnectionString);
                if (string.IsNullOrEmpty(litDetails.Text))
                {
                    litDetails.Text = "No record found";
                    strHtmlTable1 += "<tr><td>No record found</td></tr>";
                }
            }
            if (strReportTypeId.Equals("1"))
            {
                getDetailedReportDeptWise(strConnectionString);
                if (string.IsNullOrEmpty(litDetails.Text))
                {
                    litDetails.Text = "No record found";
                    strHtmlTable1 += "<tr><td>No record found</td></tr>";
                }
            }
            if (strReportTypeId.Equals("1A"))
            {
                getDetailedReportReportingDeptWise(strConnectionString);
                if (string.IsNullOrEmpty(litDetails.Text))
                {
                    litDetails.Text = "No record found";
                    strHtmlTable1 += "<tr><td>No record found</td></tr>";
                }
            }
            //<<Added by Rahuldeb on 09Mar2018
            if (strReportTypeId.Equals("3"))
            {
                getMonthlyComplianceReport(strConnectionString);
                if (string.IsNullOrEmpty(litDetails.Text))
                {
                    litDetails.Text = "No record found";
                    strHtmlTable1 += "<tr><td>No record found</td></tr>";
                }
            }
            //>>
            if (strReportTypeId.Equals("4"))
            {
                getMailDashboard(strConnectionString);
                if (string.IsNullOrEmpty(litDetails.Text))
                {
                    litDetails.Text = "No record found";
                    strHtmlTable1 += "<tr><td>No record found</td></tr>";
                }
            }
            if (strReportTypeId.Equals("5"))
            {
                getNotCompliantSpiltUpDetails(strConnectionString);
                if (string.IsNullOrEmpty(litDetails.Text))
                {
                    litDetails.Text = "No record found";
                    strHtmlTable1 += "<tr><td>No record found</td></tr>";
                }
            }
            if (strReportTypeId.Equals("6"))
            {
                getCompliantSpiltUpDetails(strConnectionString);
                if (string.IsNullOrEmpty(litDetails.Text))
                {
                    litDetails.Text = "No record found";
                    strHtmlTable1 += "<tr><td>No record found</td></tr>";
                }
            }
            if (strReportTypeId.Equals("7"))
            {
                getMailDashboardReportingDeptWise(strConnectionString);
                if (string.IsNullOrEmpty(litDetails.Text))
                {
                    litDetails.Text = "No record found";
                    strHtmlTable1 += "<tr><td>No record found</td></tr>";
                }
            }
            strHtmlTable1 += "</table>";
        }

        public void getMailDashboard(string strConnectionString)
        {
            string strQuery = "", strTrackingDeptId = "", strReportingDeptId = "", strType = "", strHtmlTable = "", strSubDateAuthority = "", strSubDate = "",
                    strClosureDate = "";
            DataTable dt = new DataTable();
            DataRow dr;

            string strMonth = DateTime.Now.AddMonths(-1).ToString("MM");
            string strCurrMonth = DateTime.Now.AddMonths(-1).ToString("MMMM yyyy");
            string strCutOffDate = F2FDatabase.F2FExecuteScalar(" select CP_VALUE from TBL_CONFIG_PARAMS where CP_NAME = 'Regulatory Reporting Tracker - Cut-off date'").ToString();
            try
            {
                //if (Request.QueryString["STMID"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["STMID"]))
                {
                    strTrackingDeptId = Request.QueryString["STMID"].ToString();
                }
                //if (Request.QueryString["SRDID"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["SRDID"]))
                {
                    strReportingDeptId = Request.QueryString["SRDID"].ToString();
                }
                //if (Request.QueryString["Type"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["Type"]))
                {
                    strType = Request.QueryString["Type"].ToString();
                }

                if (strType.Equals("1"))
                {
                    strQuery = " select *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                               " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                               " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                               " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                               " else 'Not Yet Submitted' end " +
                               " as Status  from TBL_SUB_CHKLIST " +
                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                              " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                              " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID   " +
                              " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE()))" +
                              " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                              " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                              " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId;
                }
                else if (strType.Equals("2"))
                {
                    strQuery = " select *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                               " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                               " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                               " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                               " else 'Not Yet Submitted' end " +
                               " as Status  from TBL_SUB_CHKLIST " +
                               " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                               " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                               " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                               " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                               " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID  " +
                               " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                               " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                               " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                               " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
                               " and SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0, SUB_CLOSED_ON)) <= SC_DUE_DATE_TO";
                }
                else if (strType.Equals("3"))
                {
                    strQuery = " select *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                              " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                               " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                               " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                               " else 'Not Yet Submitted' end " +
                               " as Status  from TBL_SUB_CHKLIST " +
                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                              " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                              " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID   " +
                              " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                              " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                              " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                              " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
                              " and SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0, SUB_SUBMIT_DATE)) <= SC_DUE_DATE_TO " +
                              " and DATEADD(dd, 0, DATEDIFF(dd, 0, SUB_CLOSED_ON)) > SC_DUE_DATE_TO";
                }
                else if (strType.Equals("4"))
                {
                    strQuery = " select *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                              " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                               " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                               " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                               " else 'Not Yet Submitted' end " +
                               " as Status  from TBL_SUB_CHKLIST " +
                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '" + strCutOffDate + "' " +
                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                              " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                              " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID   " +
                              " where  month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                              " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                              " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                              " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
                              " and SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0, SUB_SUBMIT_DATE)) > SC_DUE_DATE_TO " +
                              " and DATEADD(dd, 0, DATEDIFF(dd, 0, SUB_CLOSED_ON)) > SC_DUE_DATE_TO ";
                }
                else if (strType.Equals("5"))
                {
                    strQuery = " select *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                              " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                               " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                               " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                               " else 'Not Yet Submitted' end " +
                               " as Status  from TBL_SUB_CHKLIST " +
                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                              " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                              " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID   " +
                              " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                              " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                              " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                              " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
                              " and SUB_STATUS = 'S' ";
                }
                else if (strType.Equals("6"))
                {
                    strQuery = " select *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                              " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                               " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                               " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                               " else 'Not Yet Submitted' end " +
                               " as Status  from TBL_SUB_CHKLIST " +
                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                              " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                              " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID   " +
                              " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                              " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                              " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                              " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
                              " and not exists (SELECT SUB_ID FROM TBL_SUBMISSIONS " +
                              " WHERE SUB_SC_ID = SC_ID AND SUB_STATUS in('S','C')) ";
                }

                dt = F2FDatabase.F2FGetDataTable(strQuery);
                if (dt.Rows.Count > 0)
                {
                    btnExportToExcel.Visible = true;
                    strHtmlTable = "<table width='100%' border='1' class='table table-bordered footable' >";
                    strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Sr.No.</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Tracking Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reporting Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reference Circular / Notification / Act</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Section/Clause</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Particulars</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Description</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Frequency</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Internal due date</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Regulatory due date</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Remarks</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted By</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted in System on</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submission Date to Authority</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed By</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed On</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Status</td></tr>";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = dt.Rows[i];

                        strSubDate = dr["SUB_SUBMIT_DATE"].Equals(DBNull.Value) ? "&nbsp;" :
                                     Convert.ToDateTime(dr["SUB_SUBMIT_DATE"]).ToString("dd-MMM-yyyy HH:mm:ss");

                        strSubDateAuthority = dr["SUB_SUBMITTED_TO_AUTHORITY_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                              Convert.ToDateTime(dr["SUB_SUBMITTED_TO_AUTHORITY_ON"]).ToString("dd-MMM-yyyy");

                        strClosureDate = dr["SUB_CLOSED_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                         Convert.ToDateTime(dr["SUB_CLOSED_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");

                        strHtmlTable += "<tr><td class= 'DBTableCellRight'>" + (i + 1) + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["STM_TYPE"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SRD_NAME"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_ACT_REG_SECTION"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_SECTION_CLAUSE"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_PARTICULARS"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_DESCRIPTION"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_FREQUENCY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_FROM"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_TO"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_REMARKS"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CREAT_BY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDate + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDateAuthority + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CLOSED_BY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strClosureDate + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["status"].ToString() + "&nbsp;</td></tr>";

                        strClosureDate = ""; strSubDateAuthority = ""; strSubDate = "";
                    }

                    strHtmlTable = strHtmlTable + "</table>";
                    litDetails.Text = strHtmlTable;
                    Session["strDetailedReport"] = strHtmlTable;
                }
                lblHeading.Text = "Reportings Dashboard for Month of " + strCurrMonth;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("system exception in getMailDashboard(): " + ex);
            }
        }

        public void getMailDashboardReportingDeptWise(string strConnectionString)
        {
            string strQuery = "", strReportingDeptId = "", strType = "", strHtmlTable = "", strSubDateAuthority = "", strSubDate = "",
                    strClosureDate = "";
            DataTable dt = new DataTable();
            DataRow dr;

            string strMonth = DateTime.Now.AddMonths(-1).ToString("MM");
            string strCurrMonth = DateTime.Now.AddMonths(-1).ToString("MMMM yyyy");
            try
            {
                //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                //if (Request.QueryString["SRDID"] != null)
                //>>
                if (!string.IsNullOrEmpty(Request.QueryString["SRDID"]))
                {
                    strReportingDeptId = Request.QueryString["SRDID"].ToString();
                }
                //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                //if (Request.QueryString["Type"] != null)
                //>>
                if (!string.IsNullOrEmpty(Request.QueryString["Type"]))
                {
                    strType = Request.QueryString["Type"].ToString();
                }

                if (strType.Equals("1"))
                {
                    strQuery = " select *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                               " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                               " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                               " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                               " else 'Not Yet Submitted' end " +
                               " as Status  from TBL_SUB_CHKLIST " +
                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                              " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                              " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID   " +
                              " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE()))" +
                              " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                              " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                              " and SM_SRD_ID = " + strReportingDeptId;
                }
                else if (strType.Equals("2"))
                {
                    strQuery = " select *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                               " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                               " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                               " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                               " else 'Not Yet Submitted' end " +
                               " as Status  from TBL_SUB_CHKLIST " +
                               " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                               " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                               " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                               " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                               " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID  " +
                               " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                               " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                               " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                               " and SM_SRD_ID = " + strReportingDeptId +
                               " and SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0, SUB_CLOSED_ON)) <= SC_DUE_DATE_TO";
                }
                else if (strType.Equals("3"))
                {
                    strQuery = " select *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                              " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                               " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                               " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                               " else 'Not Yet Submitted' end " +
                               " as Status  from TBL_SUB_CHKLIST " +
                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                              " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                              " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID   " +
                              " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                              " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                              " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                              " and SM_SRD_ID = " + strReportingDeptId +
                              " and SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0, SUB_SUBMIT_DATE)) <= SC_DUE_DATE_TO " +
                              " and DATEADD(dd, 0, DATEDIFF(dd, 0, SUB_CLOSED_ON)) > SC_DUE_DATE_TO";
                }
                else if (strType.Equals("4"))
                {
                    strQuery = " select *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                              " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                               " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                               " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                               " else 'Not Yet Submitted' end " +
                               " as Status  from TBL_SUB_CHKLIST " +
                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                              " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                              " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID   " +
                              " where  month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                              " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                              " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                              " and SM_SRD_ID = " + strReportingDeptId +
                              " and SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0, SUB_SUBMIT_DATE)) > SC_DUE_DATE_TO " +
                              " and DATEADD(dd, 0, DATEDIFF(dd, 0, SUB_CLOSED_ON)) > SC_DUE_DATE_TO ";
                }
                else if (strType.Equals("5"))
                {
                    strQuery = " select *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                              " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                               " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                               " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                               " else 'Not Yet Submitted' end " +
                               " as Status  from TBL_SUB_CHKLIST " +
                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                              " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                              " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID   " +
                              " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                              " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                              " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                              " and SM_SRD_ID = " + strReportingDeptId +
                              " and SUB_STATUS = 'S' ";
                }
                else if (strType.Equals("6"))
                {
                    strQuery = " select *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                              " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                               " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                               " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                               " else 'Not Yet Submitted' end " +
                               " as Status  from TBL_SUB_CHKLIST " +
                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                              " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                              " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID   " +
                              " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                              " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                              " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                              " and SM_SRD_ID = " + strReportingDeptId +
                              " and not exists (SELECT SUB_ID FROM TBL_SUBMISSIONS " +
                              " WHERE SUB_SC_ID = SC_ID AND SUB_STATUS in('S','C')) ";
                }

                //dt = dserv.Getdata(strQuery);
                dt = F2FDatabase.F2FGetDataTable(strQuery);
                if (dt.Rows.Count > 0)
                {
                    btnExportToExcel.Visible = true;
                    strHtmlTable = "<table width='100%' border='1' class='table table-bordered footable' >";
                    strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Sr.No.</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Tracking Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reporting Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reference Circular / Notification / Act</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Section/Clause</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Particulars</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Description</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Frequency</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Internal due date</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Regulatory due date</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Remarks</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted By</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted in System on</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submission Date to Authority</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed By</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed On</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Status</td></tr>";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = dt.Rows[i];

                        strSubDate = dr["SUB_SUBMIT_DATE"].Equals(DBNull.Value) ? "&nbsp;" :
                                     Convert.ToDateTime(dr["SUB_SUBMIT_DATE"]).ToString("dd-MMM-yyyy HH:mm:ss");

                        strSubDateAuthority = dr["SUB_SUBMITTED_TO_AUTHORITY_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                              Convert.ToDateTime(dr["SUB_SUBMITTED_TO_AUTHORITY_ON"]).ToString("dd-MMM-yyyy");

                        strClosureDate = dr["SUB_CLOSED_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                         Convert.ToDateTime(dr["SUB_CLOSED_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");

                        strHtmlTable += "<tr><td class= 'DBTableCellRight'>" + (i + 1) + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["STM_TYPE"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SRD_NAME"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_ACT_REG_SECTION"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_SECTION_CLAUSE"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_PARTICULARS"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_DESCRIPTION"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_FREQUENCY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_FROM"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_TO"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_REMARKS"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CREAT_BY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDate + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDateAuthority + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CLOSED_BY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strClosureDate + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["status"].ToString() + "&nbsp;</td></tr>";

                        strClosureDate = ""; strSubDateAuthority = ""; strSubDate = "";
                    }

                    strHtmlTable = strHtmlTable + "</table>";
                    litDetails.Text = strHtmlTable;
                    Session["strDetailedReport"] = strHtmlTable;
                }
                lblHeading.Text = "Reportings Dashboard for Month of " + strCurrMonth;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("system exception in getMailDashboard(): " + ex);
            }
        }

        public void getDetailedReportDeptWise(string strConnectionString)
        {
            string strReportingFunction = "", strFrequency = ""
                     , strFromDate = "", strToDate = "", strPriority = "", strStatus = "", strHeading = "", strDeptId = "";
            string strFilter = "", strQry1 = "", strHtmlTable = "", strDeptName = "", strDeptFilter = "", strSubDateAuthority = "", strSubDate = "",
                    strClosureDate = "", strCompliantTable = "", strNonCompliantTable = "", strToDateFilter = "", strFromDateFilter = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;
            int intCnt;

            try
            {
                //<<Modified by Kiran Kharat on 20Mar2018
                if (!string.IsNullOrEmpty(Request.QueryString["STMID"]))
                {
                    strDeptId = Request.QueryString["STMID"].ToString();
                    strDeptFilter = "and SM_STM_ID = " + strDeptId;
                }
                else
                {
                    strDeptFilter = "";
                }
                //>>
                //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                //if (Request.QueryString["DeptName"] != null)
                //>>
                if (!string.IsNullOrEmpty(Request.QueryString["DeptName"]))
                {
                    strDeptName = Request.QueryString["DeptName"].ToString();
                }
                //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                //if (Request.QueryString["Status"] != null)
                //>>
                if (!string.IsNullOrEmpty(Request.QueryString["Status"]))
                {
                    strStatus = Request.QueryString["Status"].ToString();
                    if (strStatus != "")
                    {
                        //<<Modified by Kiran Kharat on 16Mar2018
                        if (strStatus == "S")
                        {
                            strFilter += "  and SUB_STATUS in ('S','R','SR') ";//SUB_STATUS in ('C','S')
                        }

                        //else if (strStatus == "ND")
                        //{
                        //    strFilter += " and  SC_DUE_DATE_TO >= current_timestamp ";
                        //}
                        //else if (strStatus == "DN")
                        //{
                        //    strFilter += " and SC_DUE_DATE_TO <= current_timestamp and (SUB_STATUS is null or SUB_STATUS = '') ";
                        //}
                        else if (strStatus == "ND")
                        {
                            strFilter += " and isnull(SUB_STATUS, '') = '' ";

                            //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                                //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                                //if (Request.QueryString["ToDate"] != null)
                                //>>
                            if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                            {
                                strToDate = Request.QueryString["ToDate"].ToString();
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) > CAST('" + strToDate + "' AS DATE) ";
                            }
                            else
                            {
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) > CAST(CURRENT_TIMESTAMP AS DATE) ";
                            }
                            //>>
                        }
                        else if (strStatus == "DN")
                        {
                            strFilter += " and isnull(SUB_STATUS, '') = '' ";

                            //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                            if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                            {
                                strToDate = Request.QueryString["ToDate"].ToString();
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) <= CAST('" + strToDate + "' AS DATE) ";
                            }
                            else
                            {
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) <= CAST(CURRENT_TIMESTAMP AS DATE) ";
                            }
                            //>>
                        }
                        else if (strStatus.Equals("Y"))
                        {
                            strFilter += " and DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_STATUS in ('S','C') and SUB_YES_NO_NA = 'Y' ";
                        }
                        else if (strStatus.Equals("N"))
                        {
                            strFilter += " and SUB_YES_NO_NA = 'N' and SUB_STATUS in ('S','C') ";
                            //strFilter += " and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') " +
                            //                " and SUB_STATUS in ('S','C') ";
                        }
                        else if (strStatus.Equals("NA"))
                        {
                            strFilter += " and SUB_YES_NO_NA = '" + strStatus + "' and SUB_STATUS in ('S','C') ";
                        }
                        //else
                        //{
                        //    strFilter += " and isnull(SUB_STATUS,'') != 'R' ";
                        //}
                        else if (strStatus.Equals("DC"))
                        {
                            strFilter += "and SUB_STATUS in ('S','C') and CAST(ISNULL(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) AS DATE) > CAST(SC_DUE_DATE_TO AS DATE) and SUB_YES_NO_NA = 'Y' ";
                        }
                        //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                        else if (strStatus == "SR")
                        {
                            strFilter += " and SUB_STATUS in ('R','SR') ";//SUB_STATUS in ('C','S')
                        }
                    }
                    //else
                    //{
                    //    strFilter += " and SUB_ID is null ";
                    //}
                    //>>
                }

                if (strStatus != "ND")      //Not yet due
                {
                    //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                    //if (Request.QueryString["FromDate"] != null)
                    //>>
                    if (!string.IsNullOrEmpty(Request.QueryString["FromDate"]))
                    {
                        strFromDate = Request.QueryString["FromDate"].ToString();
                        if (strFromDate != "")
                        {
                            strFromDateFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
                        }
                    }
                    //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                    //if (Request.QueryString["ToDate"] != null)
                    //>>
                    if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                    {
                        strToDate = Request.QueryString["ToDate"].ToString();
                        if (strToDate != "")
                        {
                            strToDateFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
                        }
                    }
                }

                //if (Request.QueryString["FromDate"] != null)
                //{
                //    strFromDate = Request.QueryString["FromDate"].ToString();
                //    if (strFromDate != "")
                //    {
                //        strFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
                //    }
                //}

                //if (Request.QueryString["ToDate"] != null)
                //{
                //    strToDate = Request.QueryString["ToDate"].ToString();
                //    if (strToDate != "")
                //    {
                //        strFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
                //    }
                //}

                //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                //if (Request.QueryString["SRDID"] != null)
                //>>
                if (!string.IsNullOrEmpty(Request.QueryString["SRDID"]))
                {
                    strReportingFunction = Request.QueryString["SRDID"].ToString();
                    if (strReportingFunction != "")
                    {
                        strFilter += " and SM_SRD_ID = " + strReportingFunction + " ";
                    }
                }
                //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                //if (Request.QueryString["Frequency"] != null)
                //>>
                if (!string.IsNullOrEmpty(Request.QueryString["Frequency"]))
                {
                    strFrequency = Request.QueryString["Frequency"].ToString();
                    if (strFrequency != "")
                    {
                        strFilter += " and SM_FREQUENCY = '" + strFrequency + "' ";
                    }
                }
                //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                //if (Request.QueryString["Priority"] != null)
                //>>
                if (!string.IsNullOrEmpty(Request.QueryString["Priority"]))
                {
                    strPriority = Request.QueryString["Priority"].ToString();
                    if (strPriority != "")
                    {
                        strFilter += " and SC_PRIORITY = '" + strPriority + "' ";
                    }
                }

                strQry1 = " select  *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                          " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                          " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                          " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                          " else 'Not Yet Submitted' end " +
                          " as Status " +
                          " from  TBL_SUB_CHKLIST " +
                          " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID " +
                          " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " + strDeptFilter +
                          " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                          " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                          " where 1=1 " + strFilter + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry();

                dt = F2FDatabase.F2FGetDataTable(strQry1);

                if (dt.Rows.Count > 0)
                {
                    btnExportToExcel.Visible = true;
                    strHtmlTable = "<table width='100%' border='1' class='table table-bordered footable' >";
                    strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Sr.No.</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Tracking Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reporting Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reference Circular / Notification / Act</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Section/Clause</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Particulars</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Description</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Frequency</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Internal due date</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Regulatory due date</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Remarks</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted By</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted in System on</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submission Date to Authority</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed By</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed On</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Status</td></tr>";


                    for (intCnt = 0; intCnt < dt.Rows.Count; intCnt++)
                    {
                        dr = dt.Rows[intCnt];

                        strSubDate = dr["SUB_SUBMIT_DATE"].Equals(DBNull.Value) ? "&nbsp;" :
                                     Convert.ToDateTime(dr["SUB_SUBMIT_DATE"]).ToString("dd-MMM-yyyy HH:mm:ss");

                        strSubDateAuthority = dr["SUB_SUBMITTED_TO_AUTHORITY_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                              Convert.ToDateTime(dr["SUB_SUBMITTED_TO_AUTHORITY_ON"]).ToString("dd-MMM-yyyy");

                        strClosureDate = dr["SUB_CLOSED_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                         Convert.ToDateTime(dr["SUB_CLOSED_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");

                        strHtmlTable += "<tr><td class= 'DBTableCellRight'>" + (intCnt + 1) + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["STM_TYPE"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SRD_NAME"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_ACT_REG_SECTION"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_SECTION_CLAUSE"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_PARTICULARS"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_DESCRIPTION"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_FREQUENCY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_FROM"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_TO"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_REMARKS"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CREAT_BY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDate + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDateAuthority + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CLOSED_BY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strClosureDate + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["status"].ToString() + "&nbsp;</td></tr>";

                        strClosureDate = ""; strSubDateAuthority = ""; strSubDate = "";
                    }

                    strHtmlTable = strHtmlTable + "</table>";

                    if (strStatus == "Y")
                    {
                        strCompliantTable = getCompliantSplitUp(strFilter + strDeptFilter);
                        litDetails.Text = strCompliantTable + "<br/>" + strHtmlTable;
                    }
                    else if (strStatus == "N")
                    {
                        strNonCompliantTable = getNotCompliantSplitUp(strFilter + strDeptFilter);
                        litDetails.Text = strNonCompliantTable + "<br/>" + strHtmlTable;
                    }
                    else
                    {
                        litDetails.Text = strHtmlTable;
                    }

                    Session["strDetailedReport"] = strHtmlTable;
                }

                if (strStatus == "Y")
                {
                    strHeading = "- (Compliant)";
                }
                else if (strStatus == "N")
                {
                    strHeading = "- (Not Compliant)";
                }
                else if (strStatus == "NA")
                {
                    strHeading = "- (Not Applicable)";
                }
                else if (strStatus == "S")
                {
                    strHeading = "- (Submitted)";
                }
                else if (strStatus == "ND")
                {
                    strHeading = "- (Not Yet Due)";
                }
                else if (strStatus == "DN")
                {
                    strHeading = "- (Due and Not yet Submitted)";
                }
                else if (strStatus == "SR")
                {
                    strHeading = "- (Reopened / Sent back)";
                }
                else
                {
                    strHeading = "- (Total)";
                }

                lblHeading.Text = "Tracking function wise Compliance Status " + strHeading + " for Tracking Function (" + strDeptName + ")";
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("system exception in getCriticalitySummaryReport(): " + ex);
            }
        }

        public void getDetailedComplianceWiseStatus(string strConnectionString)
        {
            string strTrackedBy = "", strReportingFunction = "", strFrequency = ""
                     , strFromDate = "", strToDate = "", strPriority = "", strStatus = "", strHeading = "";
            string strFilter = "", strQry1 = "", strHtmlTable = "", strSubDateAuthority = "", strSubDate = "",
                    strClosureDate = "", strCompliantTable = "", strNonCompliantTable = "", strToDateFilter = "", strFromDateFilter = ""; ;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;
            int intCnt;

            try
            {
                //if (Request.QueryString["Status"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["Status"]))
                {
                    strStatus = Request.QueryString["Status"].ToString();
                    if (strStatus != "")
                    {
                        //<<Modified by Kiran Kharat on 16Mar2018
                        if (strStatus == "S")
                        {
                            strFilter += " and SUB_STATUS in ('S','R','SR') ";//SUB_STATUS in ('C','S')
                        }
                        //>>
                        //else if (strStatus == "ND")
                        //{
                        //    strFilter += " and SC_DUE_DATE_TO >= current_timestamp ";
                        //}
                        //else if (strStatus == "DN")
                        //{
                        //    strFilter += " and SC_DUE_DATE_TO <= current_timestamp and (SUB_STATUS is null or SUB_STATUS = '') ";
                        //}
                        else if (strStatus == "ND")
                        {
                            strFilter += " and isnull(SUB_STATUS, '') = '' ";

                            //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                            if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                            {
                                strToDate = Request.QueryString["ToDate"].ToString();
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) > CAST('" + strToDate + "' AS DATE) ";
                            }
                            else
                            {
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) > CAST(CURRENT_TIMESTAMP AS DATE) ";
                            }
                            //>>
                        }
                        else if (strStatus == "DN")
                        {
                            strFilter += " and isnull(SUB_STATUS, '') = '' ";

                            //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                            if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                            {
                                strToDate = Request.QueryString["ToDate"].ToString();
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) <= CAST('" + strToDate + "' AS DATE) ";
                            }
                            else
                            {
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) <= CAST(CURRENT_TIMESTAMP AS DATE) ";
                            }
                            //>>
                        }
                        else if (strStatus.Equals("Y"))
                        {
                            strFilter += " and DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y' and SUB_STATUS in ('S','C')  ";
                        }
                        else if (strStatus.Equals("N"))
                        {
                            //<<Added by Ankur Tyagi on 08-Apr-2024 for CR_2025
                            if (!string.IsNullOrEmpty(Request.QueryString["UName"]) & !string.IsNullOrEmpty(Request.QueryString["UMail"]) &
                            !string.IsNullOrEmpty(Request.QueryString["Role"]))
                            {
                                strFilter += " and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') " +
                                    " and SUB_STATUS in ('S','C') ";
                            }
                            //>>
                            else
                            {
                                strFilter += " and SUB_YES_NO_NA = 'N' and SUB_STATUS in ('S','C') ";
                            }
                            //strFilter += " and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') " +
                            //    " and SUB_STATUS in ('S','C') ";
                        }
                        else if (strStatus.Equals("NA"))
                        {
                            strFilter += " and SUB_YES_NO_NA = '" + strStatus + "' and SUB_STATUS in ('S','C') ";
                        }
                        //else
                        //{
                        //    strFilter += " and isnull(SUB_STATUS,'') != 'R' ";
                        //}
                        else if (strStatus.Equals("DC"))
                        {
                            strFilter += " and CAST(ISNULL(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) AS DATE) > CAST(SC_DUE_DATE_TO AS DATE) and SUB_YES_NO_NA = 'Y' and SUB_STATUS in ('S','C') ";
                        }
                        //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                        else if (strStatus == "SR")
                        {
                            strFilter += " and SUB_STATUS in ('R','SR') ";//SUB_STATUS in ('C','S')
                        }
                        //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                        else if (strStatus == "OBNS")
                        {
                            strFilter += " and SC_DUE_DATE_TO < CURRENT_TIMESTAMP AND (ISNULL(SUB_STATUS, '') = '')  ";//SUB_STATUS in ('C','S')
                        }
                        else if (strStatus == "NCNC")
                        {
                            strFilter += @" and SUB_STATUS NOT IN ('C') 
								   AND ((DATEADD(dd, 0, DATEDIFF(dd, 0, ISNULL(SUB_SUBMITTED_TO_AUTHORITY_ON, SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO 
								   AND SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') 
								   AND SUB_STATUS NOT IN ('C') ";//SUB_STATUS in ('C','S')
                        }
                        //>>
                    }
                    //else
                    //{
                    //    strFilter += " and SUB_ID is null ";
                    //}
                }

                //if (Request.QueryString["FromDate"] != null)
                //{
                //    strFromDate = Request.QueryString["FromDate"].ToString();
                //    if (strFromDate != "")
                //    {
                //        strFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
                //    }
                //}

                //if (Request.QueryString["ToDate"] != null)
                //{
                //    strToDate = Request.QueryString["ToDate"].ToString();
                //    if (strToDate != "")
                //    {
                //        strFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
                //    }
                //}
                if (strStatus != "ND")
                {
                    //if (Request.QueryString["FromDate"] != null)
                    //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                    if (!string.IsNullOrEmpty(Request.QueryString["FromDate"]))
                    {
                        strFromDate = Request.QueryString["FromDate"].ToString();
                        if (strFromDate != "")
                        {
                            strFromDateFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
                        }
                    }

                    //if (Request.QueryString["ToDate"] != null)
                    //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                    if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                    {
                        strToDate = Request.QueryString["ToDate"].ToString();
                        if (strToDate != "")
                        {
                            strToDateFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
                        }
                    }
                }

                //if (Request.QueryString["SRDID"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["SRDID"]))
                {
                    strReportingFunction = Request.QueryString["SRDID"].ToString();
                    if (strReportingFunction != "")
                    {
                        strFilter += " and SM_SRD_ID = " + strReportingFunction + " ";
                    }
                }

                //if (Request.QueryString["STMID"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["STMID"]))
                {
                    strTrackedBy = Request.QueryString["STMID"].ToString();
                    if (strTrackedBy != "")
                    {
                        strFilter += " and SM_STM_ID = " + strTrackedBy + " ";
                    }
                }

                //if (Request.QueryString["Frequency"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["Frequency"]))
                {
                    strFrequency = Request.QueryString["Frequency"].ToString();
                    if (strFrequency != "")
                    {
                        strFilter += " and SM_FREQUENCY = '" + strFrequency + "' ";
                    }
                }

                //if (Request.QueryString["Priority"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["Priority"]))
                {
                    strPriority = Request.QueryString["Priority"].ToString();
                    if (strPriority != "")
                    {
                        strFilter += " and SC_PRIORITY = '" + strPriority + "' ";
                    }
                }

                strQry1 = "SELECT *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                          "case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                          " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                          " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                          " else 'Not Yet Submitted' end " +
                          " as Status " +
                          "FROM TBL_SUB_CHKLIST " +
                          "INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                          "INNER JOIN TBL_SUB_TYPE_MAS ON SC_STM_ID = STM_ID " +
                          "INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                          "LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID " +
                          //<<Added by Ankur Tyagi on 08-Apr-2024 for CR_2025
                          //ReturnRoleBasedQry() +
                          //>>
                          "where 1 = 1 " + strFilter + strFromDateFilter + strToDateFilter +
                          //<<Added by Ankur Tyagi on 08-Apr-2024 for CR_2041
                          ReturnCutoffFilterQry();
                //>>

                dt = F2FDatabase.F2FGetDataTable(strQry1);

                if (dt.Rows.Count > 0)
                {
                    btnExportToExcel.Visible = true;
                    strHtmlTable = "<table width='100%' border='1' class='table table-bordered footable' >";
                    strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Sr.No.</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Tracking Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reporting Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reference Circular / Notification / Act</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Section/Clause</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Particulars</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Description</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Frequency</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Internal due date</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Regulatory due date</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Remarks</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted By</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted in System on</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submission Date to Authority</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed By</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed On</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Status</td></tr>";


                    for (intCnt = 0; intCnt < dt.Rows.Count; intCnt++)
                    {
                        dr = dt.Rows[intCnt];

                        strSubDate = dr["SUB_SUBMIT_DATE"].Equals(DBNull.Value) ? "&nbsp;" :
                                     Convert.ToDateTime(dr["SUB_SUBMIT_DATE"]).ToString("dd-MMM-yyyy HH:mm:ss");

                        strSubDateAuthority = dr["SUB_SUBMITTED_TO_AUTHORITY_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                              Convert.ToDateTime(dr["SUB_SUBMITTED_TO_AUTHORITY_ON"]).ToString("dd-MMM-yyyy");

                        strClosureDate = dr["SUB_CLOSED_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                         Convert.ToDateTime(dr["SUB_CLOSED_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");

                        strHtmlTable += "<tr><td class= 'DBTableCellRight'>" + (intCnt + 1) + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["STM_TYPE"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SRD_NAME"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_ACT_REG_SECTION"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_SECTION_CLAUSE"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_PARTICULARS"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_DESCRIPTION"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_FREQUENCY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_FROM"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_TO"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_REMARKS"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CREAT_BY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDate + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDateAuthority + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CLOSED_BY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strClosureDate + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["status"].ToString() + "&nbsp;</td></tr>";

                        strClosureDate = ""; strSubDateAuthority = ""; strSubDate = "";
                    }

                    strHtmlTable = strHtmlTable + "</table>";
                    if (strStatus == "Y")
                    {
                        strCompliantTable = getCompliantSplitUp(strFilter);
                        litDetails.Text = strCompliantTable + "<br/>" + strHtmlTable;
                    }
                    else if (strStatus == "N")
                    {
                        //<<Added by Ankur Tyagi on 08-Apr-2024 for CR_2025
                        if (!string.IsNullOrEmpty(Request.QueryString["UName"]) & !string.IsNullOrEmpty(Request.QueryString["UMail"]) &
                        !string.IsNullOrEmpty(Request.QueryString["Role"]))
                        {
                            strNonCompliantTable = getNotCompliantSplitUp(strFilter);
                            litDetails.Text = strNonCompliantTable + "<br/>" + strHtmlTable;
                        }
                        else
                        {
                            litDetails.Text = strHtmlTable;
                        }
                    }
                    else
                    {
                        litDetails.Text = strHtmlTable;
                    }

                    Session["strDetailedReport"] = strHtmlTable;
                }

                if (strStatus == "Y")
                {
                    strHeading = "- (Compliant)";
                }
                else if (strStatus == "N")
                {
                    strHeading = "- (Not Compliant)";
                }
                else if (strStatus == "NA")
                {
                    strHeading = "- (Not Applicable)";
                }
                else if (strStatus == "S")
                {
                    strHeading = "- (Submitted)";
                }
                else if (strStatus == "ND")
                {
                    strHeading = "- (Not Yet Due)";
                }
                else if (strStatus == "DN")
                {
                    strHeading = "- (Due and Not yet Submitted)";
                }
                //<<Added by Ankur Tyagi on 25Nov2023 for CR_894
                else if (strStatus == "SR")
                {
                    strHeading = "- (Reopened / Sent back)";
                }
                //>>
                else
                {
                    strHeading = "- (Total)";
                }

                lblHeading.Text = "Organization-wide Compliance Status " + strHeading;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("system exception in getCriticalitySummaryReport(): " + ex);
            }
        }

        //<<Added by Rahuldeb on 09Mar2018
        private void getMonthlyComplianceReport(string strConnectionString)
        {
            string strTrackedBy = "", strReportingFunction = "", strFrequency = ""
                     , strFromDate = "", strToDate = "", strPriority = "", strStatus = "", strHeading = "";
            string strFilter = "", strQry1 = "", strHtmlTable = "", strMonthYear = "", strSubDateAuthority = "", strSubDate = "",
                    strClosureDate = "", strCompliantTable = "", strNonCompliantTable = "", strToDateFilter = "", strFromDateFilter = "";
            ;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;
            int intCnt;

            try
            {
                //if (Request.QueryString["Status"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["Status"]))
                {
                    strStatus = Request.QueryString["Status"].ToString();
                    if (strStatus != "")
                    {

                        //<<Modified by Kiran Kharat on 16Mar1018
                        if (strStatus == "S")
                        {
                            strFilter += "  and SUB_STATUS in ('S','R','SR')";//SUB_STATUS in ('C','S')
                        }

                        //else if (strStatus == "ND")
                        //{
                        //    strFilter += " and SC_DUE_DATE_TO >= current_timestamp ";
                        //}
                        //else if (strStatus == "DN")
                        //{
                        //    strFilter += " and SC_DUE_DATE_TO <= current_timestamp and (SUB_STATUS is null or SUB_STATUS = '') ";
                        //}
                        else if (strStatus == "ND")
                        {
                            strFilter += " and isnull(SUB_STATUS, '') = '' ";

                            //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                            //if (Request.QueryString["ToDate"] != null)
                            //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                            if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                            {
                                strToDate = Request.QueryString["ToDate"].ToString();
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) > CAST('" + strToDate + "' AS DATE) ";
                            }
                            else
                            {
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) > CAST(CURRENT_TIMESTAMP AS DATE) ";
                            }
                            //>>
                        }
                        else if (strStatus == "DN")
                        {
                            strFilter += " and isnull(SUB_STATUS, '') = '' ";

                            //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                            //if (Request.QueryString["ToDate"] != null)
                            //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                            if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                            {
                                strToDate = Request.QueryString["ToDate"].ToString();
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) <= CAST('" + strToDate + "' AS DATE) ";
                            }
                            else
                            {
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) <= CAST(CURRENT_TIMESTAMP AS DATE) ";
                            }
                            //>>
                        }
                        else if (strStatus.Equals("Y"))
                        {
                            strFilter += " and DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_STATUS in ('S','C') and SUB_YES_NO_NA = 'Y' ";

                        }
                        else if (strStatus.Equals("N"))
                        {
                            strFilter += " and SUB_YES_NO_NA = 'N' and SUB_STATUS in ('S','C') ";
                            //strFilter += " and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') " +
                            //    "and SUB_STATUS in ('S','C')  ";
                        }
                        else if (strStatus.Equals("DC"))
                        {
                            strFilter += " and CAST(ISNULL(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) AS DATE) > CAST(SC_DUE_DATE_TO AS DATE)" +
                                        " and SUB_YES_NO_NA = 'Y' and SUB_STATUS in ('S','C') ";
                        }
                        else if (strStatus.Equals("NA"))
                        {
                            strFilter += " and SUB_YES_NO_NA = '" + strStatus + "' and SUB_STATUS in ('S','C') ";
                        }
                        //else
                        //{
                        //    strFilter += " and isnull(SUB_STATUS,'') != 'R' ";
                        //}
                        //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                        else if (strStatus == "SR")
                        {
                            strFilter += " and SUB_STATUS in ('R','SR') ";//SUB_STATUS in ('C','S')
                        }
                        //>>
                        //>>
                    }
                    //else
                    //{
                    //    strFilter += " and SUB_ID is null ";
                    //}
                }

                //if (Request.QueryString["FromDate"] != null)
                //{
                //    strFromDate = Request.QueryString["FromDate"].ToString();
                //    if (strFromDate != "")
                //    {
                //        strFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
                //    }
                //}

                //if (Request.QueryString["ToDate"] != null)
                //{
                //    strToDate = Request.QueryString["ToDate"].ToString();
                //    if (strToDate != "")
                //    {
                //        strFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
                //    }
                //}
                if (strStatus != "ND")
                {
                    //if (Request.QueryString["FromDate"] != null)
                    //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                    if (!string.IsNullOrEmpty(Request.QueryString["FromDate"]))
                    {
                        strFromDate = Request.QueryString["FromDate"].ToString();
                        if (strFromDate != "")
                        {
                            strFromDateFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
                        }
                    }

                    //if (Request.QueryString["ToDate"] != null)
                    //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                    if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                    {
                        strToDate = Request.QueryString["ToDate"].ToString();
                        if (strToDate != "")
                        {
                            strToDateFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
                        }
                    }
                }

                //if (Request.QueryString["SRDID"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["SRDID"]))
                {
                    strReportingFunction = Request.QueryString["SRDID"].ToString();
                    if (strReportingFunction != "")
                    {
                        strFilter += " and SM_SRD_ID = " + strReportingFunction + " ";
                    }
                }

                //if (Request.QueryString["STMID"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["STMID"]))
                {
                    strTrackedBy = Request.QueryString["STMID"].ToString();
                    if (strTrackedBy != "")
                    {
                        strFilter += " and SM_STM_ID = " + strTrackedBy + " ";
                    }
                }

                //if (Request.QueryString["Frequency"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["Frequency"]))
                {
                    strFrequency = Request.QueryString["Frequency"].ToString();
                    if (strFrequency != "")
                    {
                        strFilter += " and SM_FREQUENCY = '" + strFrequency + "' ";
                    }
                }

                //if (Request.QueryString["Priority"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["Priority"]))
                {
                    strPriority = Request.QueryString["Priority"].ToString();
                    if (strPriority != "")
                    {
                        strFilter += " and SC_PRIORITY = '" + strPriority + "' ";
                    }
                }

                //if (Request.QueryString["MonthYear"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["MonthYear"]))
                {
                    strMonthYear = Request.QueryString["MonthYear"].ToString();
                    if (strMonthYear != "")
                    {
                        //strFilter += " and month(SC_DUE_DATE_TO) = " + strMonthYear.Split('-')[0] +
                        //                " and year(SC_DUE_DATE_TO) = '" + strMonthYear.Split('-')[1] + "' ";
                        if (strStatus != "ND")
                        {
                            strFilter += " and month(SC_DUE_DATE_TO) = " + strMonthYear.Split('-')[0] +
                                        " and year(SC_DUE_DATE_TO) = '" + strMonthYear.Split('-')[1] + "' ";
                        }
                        else
                        {
                            strFilter += " and month(SC_DUE_DATE_TO) = " + strMonthYear.Split('-')[0] + " ";
                        }
                    }
                }

                strQry1 = "SELECT *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                          " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                          " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                          " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                          " else 'Not Yet Submitted' end " +
                          " as Status " +
                          "FROM TBL_SUB_CHKLIST " +
                          "INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                          "INNER JOIN TBL_SUB_TYPE_MAS ON SC_STM_ID = STM_ID " +
                          "INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID = SM_SRD_ID " +
                          "LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where 1=1 " + strFilter + strFromDateFilter + strToDateFilter;

                dt = F2FDatabase.F2FGetDataTable(strQry1);

                if (dt.Rows.Count > 0)
                {
                    btnExportToExcel.Visible = true;
                    strHtmlTable = "<table width='100%' border='1' class='table table-bordered footable' >";
                    strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Sr.No.</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Tracking Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reporting Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reference Circular / Notification / Act</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Section/Clause</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Particulars</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Description</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Frequency</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Internal due date</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Regulatory due date</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Remarks</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted By</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted in System on</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submission Date to Authority</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed By</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed On</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Status</td></tr>";


                    for (intCnt = 0; intCnt < dt.Rows.Count; intCnt++)
                    {
                        dr = dt.Rows[intCnt];

                        strSubDate = dr["SUB_SUBMIT_DATE"].Equals(DBNull.Value) ? "&nbsp;" :
                                     Convert.ToDateTime(dr["SUB_SUBMIT_DATE"]).ToString("dd-MMM-yyyy HH:mm:ss");

                        strSubDateAuthority = dr["SUB_SUBMITTED_TO_AUTHORITY_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                              Convert.ToDateTime(dr["SUB_SUBMITTED_TO_AUTHORITY_ON"]).ToString("dd-MMM-yyyy");

                        strClosureDate = dr["SUB_CLOSED_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                         Convert.ToDateTime(dr["SUB_CLOSED_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");

                        strHtmlTable += "<tr><td class= 'DBTableCellRight'>" + (intCnt + 1) + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["STM_TYPE"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SRD_NAME"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_ACT_REG_SECTION"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_SECTION_CLAUSE"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_PARTICULARS"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_DESCRIPTION"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_FREQUENCY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_FROM"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_TO"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_REMARKS"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CREAT_BY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDate + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDateAuthority + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CLOSED_BY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strClosureDate + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["status"].ToString() + "&nbsp;</td></tr>";

                        strClosureDate = ""; strSubDateAuthority = ""; strSubDate = "";
                    }

                    strHtmlTable = strHtmlTable + "</table>";
                    if (strStatus == "Y")
                    {
                        strCompliantTable = getCompliantSplitUp(strFilter);
                        litDetails.Text = strCompliantTable + "<br/>" + strHtmlTable;
                    }
                    else if (strStatus == "N")
                    {
                        strNonCompliantTable = getNotCompliantSplitUp(strFilter);
                        litDetails.Text = strNonCompliantTable + "<br/>" + strHtmlTable;
                    }
                    else
                    {
                        litDetails.Text = strHtmlTable;
                    }

                    Session["strDetailedReport"] = strHtmlTable;
                }

                if (strStatus == "Y")
                {
                    strHeading = "- (Compliant)";
                }
                else if (strStatus == "N")
                {
                    strHeading = "- (Not Compliant)";
                }
                else if (strStatus == "NA")
                {
                    strHeading = "- (Not Applicable)";
                }
                else if (strStatus == "S")
                {
                    strHeading = "- (Submitted)";
                }
                else if (strStatus == "ND")
                {
                    strHeading = "- (Not Yet Due)";
                }
                else if (strStatus == "DN")
                {
                    strHeading = "- (Due and Not yet Submitted)";
                }
                else
                {
                    strHeading = "- (Total)";
                }

                lblHeading.Text = "Monthly Compliance Report " + strHeading;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("system exception in getCriticalitySummaryReport(): " + ex);
            }
        }
        //>>

        public void getDetailedReportReportingDeptWise(string strConnectionString)
        {
            string strReportingFunction = "", strFrequency = ""
                     , strFromDate = "", strToDate = "", strPriority = "", strStatus = "", strHeading = "", strDeptId = "";
            string strFilter = "", strQry1 = "", strHtmlTable = "", strDeptName = "", strDeptFilter = "", strSubDateAuthority = "", strSubDate = "",
                    strClosureDate = "", strCompliantTable = "", strNonCompliantTable = "", strToDateFilter = "", strFromDateFilter = ""; ;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;
            int intCnt;

            try
            {
                //<<Modified by Kiran Kharat on 20Mar2018
                //if (Request.QueryString["SRDID"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["SRDID"]))
                {
                    strDeptId = Request.QueryString["SRDID"].ToString();
                    strDeptFilter = "and SM_SRD_ID = " + strDeptId;
                }
                else
                {
                    strDeptFilter = "";
                }
                //>>
                //if (Request.QueryString["DeptName"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["DeptName"]))
                {
                    strDeptName = Request.QueryString["DeptName"].ToString();
                }
                //if (Request.QueryString["Status"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["Status"]))
                {
                    strStatus = Request.QueryString["Status"].ToString();
                    if (strStatus != "")
                    {
                        //<<Modified by Kiran Kharat on 16Mar2018
                        if (strStatus == "S")
                        {
                            strFilter += "  and SUB_STATUS in ('S','R','SR') ";//SUB_STATUS in ('C','S')
                        }

                        //else if (strStatus == "ND")
                        //{
                        //    strFilter += " and  SC_DUE_DATE_TO >= current_timestamp ";
                        //}
                        //else if (strStatus == "DN")
                        //{
                        //    strFilter += " and SC_DUE_DATE_TO <= current_timestamp and (SUB_STATUS is null or SUB_STATUS = '') ";
                        //}
                        else if (strStatus == "ND")
                        {
                            strFilter += " and isnull(SUB_STATUS, '') = '' ";

                            //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                            //if (Request.QueryString["ToDate"] != null)
                            //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                            if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                            {
                                strToDate = Request.QueryString["ToDate"].ToString();
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) > CAST('" + strToDate + "' AS DATE) ";
                            }
                            else
                            {
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) > CAST(CURRENT_TIMESTAMP AS DATE) ";
                            }
                            //>>
                        }
                        else if (strStatus == "DN")
                        {
                            strFilter += " and isnull(SUB_STATUS, '') = '' ";

                            //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                            //if (Request.QueryString["ToDate"] != null)
                            //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                            if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                            {
                                strToDate = Request.QueryString["ToDate"].ToString();
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) <= CAST('" + strToDate + "' AS DATE) ";
                            }
                            else
                            {
                                strFilter += " and CAST(SC_DUE_DATE_TO AS DATE) <= CAST(CURRENT_TIMESTAMP AS DATE) ";
                            }
                            //>>
                        }
                        else if (strStatus.Equals("Y"))
                        {
                            strFilter += " and DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_STATUS in ('S','C') and SUB_YES_NO_NA = 'Y' ";
                        }
                        else if (strStatus.Equals("N"))
                        {
                            strFilter += " and SUB_YES_NO_NA = 'N' and SUB_STATUS in ('S','C') ";
                            //strFilter += " and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') " +
                            //                " and SUB_STATUS in ('S','C') ";
                        }
                        else if (strStatus.Equals("NA"))
                        {
                            strFilter += " and SUB_YES_NO_NA = '" + strStatus + "' and SUB_STATUS in ('S','C') ";
                        }
                        //else
                        //{
                        //    strFilter += " and isnull(SUB_STATUS,'') != 'R' ";
                        //}
                        else if (strStatus.Equals("DC"))
                        {
                            strFilter += "and SUB_STATUS in ('S','C') and CAST(ISNULL(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) AS DATE) > CAST(SC_DUE_DATE_TO AS DATE) and SUB_YES_NO_NA = 'Y' ";
                        }
                        //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                        else if (strStatus == "SR")
                        {
                            strFilter += " and SUB_STATUS in ('R','SR') ";//SUB_STATUS in ('C','S')
                        }
                    }
                    //else
                    //{
                    //    strFilter += " and SUB_ID is null ";
                    //}
                    //>>
                }

                //if (Request.QueryString["FromDate"] != null)
                //{
                //    strFromDate = Request.QueryString["FromDate"].ToString();
                //    if (strFromDate != "")
                //    {
                //        strFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
                //    }
                //}

                //if (Request.QueryString["ToDate"] != null)
                //{
                //    strToDate = Request.QueryString["ToDate"].ToString();
                //    if (strToDate != "")
                //    {
                //        strFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
                //    }
                //}
                if (strStatus != "ND")      //Not yet due
                {
                    //if (Request.QueryString["FromDate"] != null)
                    //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                    if (!string.IsNullOrEmpty(Request.QueryString["FromDate"]))
                    {
                        strFromDate = Request.QueryString["FromDate"].ToString();
                        if (strFromDate != "")
                        {
                            strFromDateFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
                        }
                    }

                    //if (Request.QueryString["ToDate"] != null)
                    //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                    if (!string.IsNullOrEmpty(Request.QueryString["ToDate"]))
                    {
                        strToDate = Request.QueryString["ToDate"].ToString();
                        if (strToDate != "")
                        {
                            strToDateFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
                        }
                    }
                }

                //if (Request.QueryString["SRDID"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["SRDID"]))
                {
                    strReportingFunction = Request.QueryString["SRDID"].ToString();
                    if (strReportingFunction != "")
                    {
                        strFilter += " and SM_SRD_ID = " + strReportingFunction + " ";
                    }
                }

                //if (Request.QueryString["Frequency"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["Frequency"]))
                {
                    strFrequency = Request.QueryString["Frequency"].ToString();
                    if (strFrequency != "")
                    {
                        strFilter += " and SM_FREQUENCY = '" + strFrequency + "' ";
                    }
                }

                //if (Request.QueryString["Priority"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["Priority"]))
                {
                    strPriority = Request.QueryString["Priority"].ToString();
                    if (strPriority != "")
                    {
                        strFilter += " and SC_PRIORITY = '" + strPriority + "' ";
                    }
                }

                strQry1 = " select  *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                          " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                          " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                          " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                          " else 'Not Yet Submitted' end " +
                          " as Status " +
                          " from  TBL_SUB_CHKLIST " +
                          " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID " +
                          " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " + strDeptFilter +
                          " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                          " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                          " where 1=1 " + strFilter + strFromDateFilter + strToDateFilter;

                dt = F2FDatabase.F2FGetDataTable(strQry1);

                if (dt.Rows.Count > 0)
                {
                    btnExportToExcel.Visible = true;
                    strHtmlTable = "<table width='100%' border='1' class='table table-bordered footable' >";
                    strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Sr.No.</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Tracking Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reporting Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reference Circular / Notification / Act</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Section/Clause</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Particulars</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Description</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Frequency</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Internal due date</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Regulatory due date</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Remarks</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted By</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted in System on</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submission Date to Authority</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed By</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed On</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Status</td></tr>";


                    for (intCnt = 0; intCnt < dt.Rows.Count; intCnt++)
                    {
                        dr = dt.Rows[intCnt];

                        strSubDate = dr["SUB_SUBMIT_DATE"].Equals(DBNull.Value) ? "&nbsp;" :
                                     Convert.ToDateTime(dr["SUB_SUBMIT_DATE"]).ToString("dd-MMM-yyyy HH:mm:ss");

                        strSubDateAuthority = dr["SUB_SUBMITTED_TO_AUTHORITY_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                              Convert.ToDateTime(dr["SUB_SUBMITTED_TO_AUTHORITY_ON"]).ToString("dd-MMM-yyyy");

                        strClosureDate = dr["SUB_CLOSED_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                         Convert.ToDateTime(dr["SUB_CLOSED_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");

                        strHtmlTable += "<tr><td class= 'DBTableCellRight'>" + (intCnt + 1) + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["STM_TYPE"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SRD_NAME"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_ACT_REG_SECTION"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_SECTION_CLAUSE"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_PARTICULARS"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_DESCRIPTION"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_FREQUENCY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_FROM"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_TO"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_REMARKS"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CREAT_BY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDate + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDateAuthority + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CLOSED_BY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strClosureDate + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["status"].ToString() + "&nbsp;</td></tr>";

                        strClosureDate = ""; strSubDateAuthority = ""; strSubDate = "";
                    }

                    strHtmlTable = strHtmlTable + "</table>";

                    if (strStatus == "Y")
                    {
                        strCompliantTable = getCompliantSplitUp(strFilter + strDeptFilter);
                        litDetails.Text = strCompliantTable + "<br/>" + strHtmlTable;
                    }
                    else if (strStatus == "N")
                    {
                        strNonCompliantTable = getNotCompliantSplitUp(strFilter + strDeptFilter);
                        litDetails.Text = strNonCompliantTable + "<br/>" + strHtmlTable;
                    }
                    else
                    {
                        litDetails.Text = strHtmlTable;
                    }

                    Session["strDetailedReport"] = strHtmlTable;
                }

                if (strStatus == "Y")
                {
                    strHeading = "- (Compliant)";
                }
                else if (strStatus == "N")
                {
                    strHeading = "- (Not Compliant)";
                }
                else if (strStatus == "NA")
                {
                    strHeading = "- (Not Applicable)";
                }
                else if (strStatus == "S")
                {
                    strHeading = "- (Submitted)";
                }
                else if (strStatus == "ND")
                {
                    strHeading = "- (Not Yet Due)";
                }
                else if (strStatus == "DN")
                {
                    strHeading = "- (Due and Not yet Submitted)";
                }
                else if (strStatus == "SR")
                {
                    strHeading = "- (Reopened / Sent back)";
                }
                else
                {
                    strHeading = "- (Total)";
                }

                lblHeading.Text = "Reporting function wise Compliance Status " + strHeading + " for Reporting Function (" + strDeptName + ")";
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("system exception in getCriticalitySummaryReport(): " + ex);
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            string strHTMLReport = "";
            string attachment, html2;

            if (Session["strDetailedReport"] != null)
            {
                strHTMLReport = Session["strDetailedReport"].ToString();

                strHTMLReport = strHTMLReport.Replace("cellspacing='0'", " cellspacing='0' border='1' ");
                attachment = "attachment; filename=DetailedReport.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                //Response.Write(strHTMLReport.ToString());
                //Response.End();

                html2 = "";

                html2 = Regex.Replace(strHTMLReport.ToString(), @"(<a \/?[^>]+>)", @"", RegexOptions.IgnoreCase);
                html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;' />", RegexOptions.IgnoreCase);
                html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;' />", RegexOptions.IgnoreCase);
                Response.Write(html2.ToString());
                Response.End();
            }
        }

        public string getCompliantSplitUp(string strFilter)
        {
            string strtHTMLCompliantSpiltUp = "";
            int intClosedBeforeDuedate = 0, intSubmittedBeforeDuedate = 0, intNotClosed = 0, intSubmittedAfterDuedate = 0;

            string strClosedBeforeDueDate = " select count(*) from TBL_SUB_CHKLIST " +
                                            " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                                            " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                            " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                            " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                            " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " +
                                            " where SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) <= SC_DUE_DATE_TO " + strFilter;

            string strSubmittedBeforeDueDate = " select count(*) from TBL_SUB_CHKLIST " +
                                           " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                                           " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                           " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                           " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                           " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID  " +
                                           " where SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_SUBMIT_DATE)) <= SC_DUE_DATE_TO and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) > SC_DUE_DATE_TO " + strFilter;

            string strSubmittedAfterDueDate = " select count(*) from TBL_SUB_CHKLIST " +
                                           " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                                           " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                           " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                           " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                           " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID   " +
                                           " where SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_SUBMIT_DATE)) > SC_DUE_DATE_TO and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) > SC_DUE_DATE_TO " + strFilter;

            string strNotClosed = " select count(*) from TBL_SUB_CHKLIST " +
                                  " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                                  " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                  " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                  " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                  " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " +
                                  " where SUB_STATUS = 'S'  " + strFilter;

            strtHTMLCompliantSpiltUp = "<table width='100%' border='1' class='table table-bordered footable'>";
            strtHTMLCompliantSpiltUp += "<tr><td class= 'DBTableFirstCellRight'>Submitted and Closed within Due Date</td>";
            strtHTMLCompliantSpiltUp += "<td class= 'DBTableFirstCellRight'>Submitted before Due date, but Closed after Due Date</td>";
            strtHTMLCompliantSpiltUp += "<td class= 'DBTableFirstCellRight'>Submitted and Closed after Due Date</td>";
            strtHTMLCompliantSpiltUp += "<td class= 'DBTableFirstCellRight'>Submitted but not yet Closed</td></tr>";


            intClosedBeforeDuedate = Convert.ToInt32(F2FDatabase.F2FExecuteScalar(strClosedBeforeDueDate));
            intSubmittedBeforeDuedate = Convert.ToInt32(F2FDatabase.F2FExecuteScalar(strSubmittedBeforeDueDate));
            intSubmittedAfterDuedate = Convert.ToInt32(F2FDatabase.F2FExecuteScalar(strSubmittedAfterDueDate));
            intNotClosed = Convert.ToInt32(F2FDatabase.F2FExecuteScalar(strNotClosed));


            strtHTMLCompliantSpiltUp += "<td class= 'DBTableCellRight'><a href='#'  onclick=\"window.open(" +
                                            "'DetailedReport.aspx?ReportType=6&Filter=" + encdec.Encrypt(strFilter) + "&Type=1'," +
                                            "'innerPop', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                            intClosedBeforeDuedate +
                                            "</a></td>";
            strtHTMLCompliantSpiltUp += "<td class= 'DBTableCellRight'><a href='#'  onclick=\"window.open(" +
                                            "'DetailedReport.aspx?ReportType=6&Filter=" + encdec.Encrypt(strFilter) + "&Type=2'," +
                                            "'innerPop', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                            intSubmittedBeforeDuedate +
                                            "</a></td>";
            strtHTMLCompliantSpiltUp += "<td class= 'DBTableCellRight'><a href='#'  onclick=\"window.open(" +
                                            "'DetailedReport.aspx?ReportType=6&Filter=" + encdec.Encrypt(strFilter) + "&Type=3'," +
                                            "'innerPop', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                            intSubmittedAfterDuedate +
                                            "</a></td>";
            strtHTMLCompliantSpiltUp += "<td class= 'DBTableCellRight'><a href='#'  onclick=\"window.open(" +
                                            "'DetailedReport.aspx?ReportType=6&Filter=" + encdec.Encrypt(strFilter) + "&Type=4'," +
                                            "'innerPop', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                            intNotClosed +
                                            "</a></td></tr>";
            strtHTMLCompliantSpiltUp += "</table>";

            return strtHTMLCompliantSpiltUp;
        }

        public string getNotCompliantSplitUp(string strFilter)
        {
            string strtNonHTMLCompliantSpiltUp = "";
            int intTaggedAsNotCompliant = 0, intSubmittedToAuthorityAfterDuedate = 0;
            DataTable dt = new DataTable();
            DataRow dr;

            string strQuery = " select Count(*) as [TotalCount],SUB_YES_NO_NA from TBL_SUB_CHKLIST  " +
                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018'  " +
                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID  INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID  " +
                              " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " + strFilter + " group by SUB_YES_NO_NA order by SUB_YES_NO_NA DESC ";

            dt = F2FDatabase.F2FGetDataTable(strQuery);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    if (dr["SUB_YES_NO_NA"].ToString().Equals("N"))
                    {
                        intTaggedAsNotCompliant = Convert.ToInt32(dr["TotalCount"].ToString());
                    }
                    else
                    {
                        intSubmittedToAuthorityAfterDuedate = Convert.ToInt32(dr["TotalCount"].ToString());
                    }
                }
            }

            strtNonHTMLCompliantSpiltUp = "<center><table width='70%' class='table table-bordered footable'>";
            strtNonHTMLCompliantSpiltUp += "<tr><td class= 'DBTableFirstCellRight' width='50%'>Tagged as Not Compliant</td>";
            strtNonHTMLCompliantSpiltUp += "<td class= 'DBTableFirstCellRight'>Submitted to Authority after Due Date</td></tr>";

            strtNonHTMLCompliantSpiltUp += "<td class= 'DBTableCellRight'>" +
                                           "<a href='#'  onclick=\"window.open(" +
                                            "'DetailedReport.aspx?ReportType=5&Filter=" + encdec.Encrypt(strFilter) + "&Type=1'," +
                                            "'innerPop', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                            intTaggedAsNotCompliant +
                                            "</a></td>";

            strtNonHTMLCompliantSpiltUp += "<td class= 'DBTableCellRight'>" +
                                            "<a href='#'  onclick=\"window.open(" +
                                            "'DetailedReport.aspx?ReportType=5&Filter=" + encdec.Encrypt(strFilter) + "&Type=2'," +
                                            "'innerPop', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                            intSubmittedToAuthorityAfterDuedate +
                                            "</a></tr>";

            strtNonHTMLCompliantSpiltUp += "</table></center>";

            return strtNonHTMLCompliantSpiltUp;
        }

        public void getNotCompliantSpiltUpDetails(string strConnectionString)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;
            int intCnt;
            string strFilter = "", strQuery = "", strHtmlTable = "", strSubDate = "", strSubDateAuthority = "", strClosureDate = "";

            try
            {
                //if (Request.QueryString["Filter"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["Filter"]))
                {
                    strFilter = encdec.Decrypt(Request.QueryString["Filter"].ToString());
                    strQuery = "select *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                              " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                              " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                              " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                              " else 'Not Yet Submitted' end " +
                              " as Status " +
                              " from TBL_SUB_CHKLIST " +
                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018' " +
                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                              " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                              " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " + strFilter;

                    if (Request.QueryString["Type"].ToString() == "1")
                    {
                        strQuery += " and SUB_YES_NO_NA = 'N' ";
                        lblHeading.Text = "Tagged as Not Compliant";
                    }
                    else if (Request.QueryString["Type"].ToString() == "2")
                    {
                        strQuery += " and SUB_YES_NO_NA = 'Y' ";
                        lblHeading.Text = "Submitted to Authority after Due Date";
                    }

                    dt = F2FDatabase.F2FGetDataTable(strQuery);

                    if (dt.Rows.Count > 0)
                    {
                        btnExportToExcel.Visible = true;
                        strHtmlTable = "<table width='100%' border='1' class='table table-bordered footable' >";
                        strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Sr.No.</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Tracking Function</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reporting Function</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reference Circular / Notification / Act</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Section/Clause</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Particulars</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Description</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Frequency</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Internal due date</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Regulatory due date</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Remarks</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted By</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted in System on</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submission Date to Authority</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed By</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed On</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Status</td></tr>";


                        for (intCnt = 0; intCnt < dt.Rows.Count; intCnt++)
                        {
                            dr = dt.Rows[intCnt];

                            strSubDate = dr["SUB_SUBMIT_DATE"].Equals(DBNull.Value) ? "&nbsp;" :
                                         Convert.ToDateTime(dr["SUB_SUBMIT_DATE"]).ToString("dd-MMM-yyyy HH:mm:ss");

                            strSubDateAuthority = dr["SUB_SUBMITTED_TO_AUTHORITY_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                                  Convert.ToDateTime(dr["SUB_SUBMITTED_TO_AUTHORITY_ON"]).ToString("dd-MMM-yyyy");

                            strClosureDate = dr["SUB_CLOSED_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                             Convert.ToDateTime(dr["SUB_CLOSED_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");

                            strHtmlTable += "<tr><td class= 'DBTableCellRight'>" + (intCnt + 1) + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["STM_TYPE"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SRD_NAME"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_ACT_REG_SECTION"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_SECTION_CLAUSE"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_PARTICULARS"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_DESCRIPTION"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_FREQUENCY"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_FROM"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_TO"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_REMARKS"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CREAT_BY"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDate + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDateAuthority + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CLOSED_BY"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + strClosureDate + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["status"].ToString() + "&nbsp;</td></tr>";

                            strClosureDate = ""; strSubDateAuthority = ""; strSubDate = "";
                        }

                        strHtmlTable = strHtmlTable + "</table>";

                        litDetails.Text = strHtmlTable;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void getCompliantSpiltUpDetails(string strConnectionString)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;
            int intCnt;

            string strFilter = "", strQuery = "", strHtmlTable = "", strSubDate = "", strSubDateAuthority = "", strClosureDate = "";

            try
            {
                //if (Request.QueryString["Filter"] != null)
                //<<Modified by Ankur Tyagi on 08-Apr-2024 for CR_2025
                if (!string.IsNullOrEmpty(Request.QueryString["Filter"]))
                {
                    strFilter = encdec.Decrypt(Request.QueryString["Filter"].ToString());
                    strQuery = "select *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE," +
                              " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                              " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                              " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                              " else 'Not Yet Submitted' end " +
                              " as Status " +
                              "from TBL_SUB_CHKLIST  " +
                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Mar-2018'  " +
                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID  INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID  " +
                              " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID ";

                    if (Request.QueryString["Type"].ToString() == "1")
                    {
                        strQuery += " where SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) <= SC_DUE_DATE_TO " + strFilter;
                        lblHeading.Text = "Submitted and Closed within Due Date";
                    }
                    else if (Request.QueryString["Type"].ToString() == "2")
                    {
                        strQuery += " where SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_SUBMIT_DATE)) <= SC_DUE_DATE_TO and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) > SC_DUE_DATE_TO " + strFilter;
                        lblHeading.Text = "Submitted before Due date, but Closed after Due Date";
                    }
                    else if (Request.QueryString["Type"].ToString() == "3")
                    {
                        strQuery += " where SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_SUBMIT_DATE)) > SC_DUE_DATE_TO and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) > SC_DUE_DATE_TO " + strFilter;
                        lblHeading.Text = "Submitted and Closed after Due Date";
                    }
                    else if (Request.QueryString["Type"].ToString() == "4")
                    {
                        strQuery += " where SUB_STATUS = 'S'  " + strFilter;
                        lblHeading.Text = "Submitted but not yet Closed";
                    }
                    dt = F2FDatabase.F2FGetDataTable(strQuery);

                    if (dt.Rows.Count > 0)
                    {
                        btnExportToExcel.Visible = true;
                        strHtmlTable = "<table width='100%' border='1' class='table table-bordered footable' >";
                        strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Sr.No.</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Tracking Function</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reporting Function</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reference Circular / Notification / Act</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Section/Clause</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Particulars</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Description</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Frequency</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Internal due date</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Regulatory due date</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Remarks</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted By</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted in System on</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submission Date to Authority</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed By</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed On</td>";
                        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Status</td></tr>";


                        for (intCnt = 0; intCnt < dt.Rows.Count; intCnt++)
                        {
                            dr = dt.Rows[intCnt];

                            strSubDate = dr["SUB_SUBMIT_DATE"].Equals(DBNull.Value) ? "&nbsp;" :
                                         Convert.ToDateTime(dr["SUB_SUBMIT_DATE"]).ToString("dd-MMM-yyyy HH:mm:ss");

                            strSubDateAuthority = dr["SUB_SUBMITTED_TO_AUTHORITY_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                                  Convert.ToDateTime(dr["SUB_SUBMITTED_TO_AUTHORITY_ON"]).ToString("dd-MMM-yyyy");

                            strClosureDate = dr["SUB_CLOSED_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                             Convert.ToDateTime(dr["SUB_CLOSED_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");

                            strHtmlTable += "<tr><td class= 'DBTableCellRight'>" + (intCnt + 1) + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["STM_TYPE"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SRD_NAME"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_ACT_REG_SECTION"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SM_SECTION_CLAUSE"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_PARTICULARS"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_DESCRIPTION"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_FREQUENCY"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_FROM"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_TO"]).ToString("dd-MMM-yyyy") + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_REMARKS"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CREAT_BY"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDate + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDateAuthority + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CLOSED_BY"].ToString() + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + strClosureDate + "&nbsp;</td>";
                            strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["status"].ToString() + "&nbsp;</td></tr>";

                            strClosureDate = ""; strSubDateAuthority = ""; strSubDate = "";
                        }

                        strHtmlTable = strHtmlTable + "</table>";

                        litDetails.Text = strHtmlTable;
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}