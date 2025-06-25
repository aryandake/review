using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Legacy_App_Code.Certification
{
    public class RegulatoryReportingDashboard
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;

        string strCertFilingDashboardWhereClause = "";

        public RegulatoryReportingDashboard()
        {
            try
            {
                using (F2FDatabase DB = new F2FDatabase("Select CP_VALUE from TBL_CONFIG_PARAMS where CP_ID = 12 "))
                {
                    DB.OpenConnection();
                    strCertFilingDashboardWhereClause = DB.F2FCommand.ExecuteScalar() as string;
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
        }

        //DashBoard with date filter
        public string GetFilingsDashboardSingleRow(string strDeptId, string strDeptName, string strLevel, string strFromDate, string strToDate)
        {
            DataTable dt = new DataTable();
            string strHtmlTable = "", strReportingDeptId = "";
            int intCompliedCount, intNonCompliedCount, intNACount, intTotalCount;
            string strQuery = "";
            DataTable dtDept = new DataTable();
            try
            {
                //litRegulatoryFilling.Text = "";
                strHtmlTable = "<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'>";
                strHtmlTable += "<tr><th>Department</td>";
                strHtmlTable += "<th>Compliant</td>";
                strHtmlTable += "<th>Not Compliant</td>";
                strHtmlTable += "<th>Not Applicable</td>";
                strHtmlTable += "<th>Total</td></tr>";

                intCompliedCount = 0;
                intNonCompliedCount = 0;
                intNACount = 0;
                intTotalCount = 0;

                if (strLevel == "0")
                    strQuery = " and CSSDM_ID = " + strDeptId;
                else if (strLevel == "1")
                    strQuery = " and CSDM_ID = " + strDeptId;
                else if (strLevel == "2")
                    strQuery = " and CDM_ID = " + strDeptId;

                dtDept = new DataServer().Getdata(" select SRD_ID from TBL_SUB_REPORTING_DEPT " +
                                    " inner join TBL_SRD_CSSDM_MAPPING ON SRD_ID = SRCM_SRD_ID AND SRCM_CSSDM_ID in " +
                                    " ( select CSSDM_ID from TBL_CERT_SUB_SUB_DEPT_MAS " +
                                    " inner join TBL_CERT_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID " +
                                    " inner join TBL_CERT_DEPT_MAS on CDM_ID = CSDM_CDM_ID " +
                                    " where 1=1 " + strQuery + " )");

                String strQMFromDT = strFromDate;//(Convert.ToDateTime(DataServer.ExecuteScalar("select CQM_FROM_DATE FROM TBL_CERT_QUARTER_MAS where CQM_ID = " + strtQuarterID))).ToString("dd-MMM-yyyy");
                String strQMToDT = strToDate;//(Convert.ToDateTime(DataServer.ExecuteScalar("select CQM_TO_DATE FROM TBL_CERT_QUARTER_MAS where CQM_ID = " + strtQuarterID))).ToString("dd-MMM-yyyy");

                strReportingDeptId = "";

                for (int i = 0; i < dtDept.Rows.Count; i++)
                {
                    strReportingDeptId = strReportingDeptId + dtDept.Rows[i]["SRD_ID"].ToString() + ",";
                }

                if (strReportingDeptId != "")
                {

                    strReportingDeptId = strReportingDeptId.Substring(0, strReportingDeptId.Length - 1);

                    string strCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
                                                     " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                     " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                                                     " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                     " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ")" +
                                                     " and DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y' " +
                                                     " and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;


                    string strNonCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
                                                      " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                      " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                                                      " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                      " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ")" +
                                                      " and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') " +
                                                      "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                    string strNACount = " select count(1) from TBL_SUB_CHKLIST " +
                                                     " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                     " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID  " +
                                                     " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                     " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ") and SUB_YES_NO_NA = 'NA'" +
                                                      "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;


                    string strQTotal = " select Count(*) from TBL_SUB_CHKLIST " +
                                                " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                                                " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                                " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                                " where SM_SRD_ID in (" + strReportingDeptId + ") and  isnull(SUB_STATUS,'') != 'R'" +
                                                "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "' " + " " + strCertFilingDashboardWhereClause;

                    intCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strCompliedCount));
                    intNonCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strNonCompliedCount));
                    intNACount = Convert.ToInt32(DataServer.ExecuteScalar(strNACount));

                    intTotalCount = Convert.ToInt32(DataServer.ExecuteScalar(strQTotal));

                    strHtmlTable += "<tr><td class= 'DBTableCellLeft'><center>" + strDeptName + "</center></td>";
                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=Y&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                    "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    intCompliedCount +
                                    "</a>" +
                                    "</td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                        "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                        "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=N&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                        "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                        "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                        intNonCompliedCount +
                                        "</a>" +
                                        "</td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                        "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                        "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=NA&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                        "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                        "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                        +intNACount +
                                        "</a>" +
                                        "</td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                       "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                       "'DetailedReportCertificationFilling.aspx?ReportType=1&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                       "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                       "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                       +intTotalCount +
                                       "</a>" +
                                       "</td></tr>";

                    strHtmlTable += "</table>";
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return strHtmlTable;
        }

        //DashBoard for the Active Quater(AQ)
        public string GetFilingsDashboardSingleRow_AQ(string strDeptId, string strDeptName, string strLevel)
        {
            DataTable dt = new DataTable();
            string strHtmlTable = "", strReportingDeptId = "";
            int intCompliedCount, intNonCompliedCount, intNACount, intTotalCount;
            string strQuery = "";
            DataTable dtDept = new DataTable();
            try
            {
                //litRegulatoryFilling.Text = "";
                strHtmlTable = "<table width='100%' class='table table-bordered footable' cellpadding='0' cellspacing='0'>";
                strHtmlTable += "<tr><th>Department</td>";
                strHtmlTable += "<th>Compliant</td>";
                strHtmlTable += "<th>Not Compliant</td>";
                strHtmlTable += "<th>Not Applicable</td>";
                strHtmlTable += "<th>Not Yet Submitted</td></tr>";

                intCompliedCount = 0;
                intNonCompliedCount = 0;
                intNACount = 0;
                intTotalCount = 0;

                if (strLevel == "0")
                    strQuery = " and CSSDM_ID = " + strDeptId;
                else if (strLevel == "1")
                    strQuery = " and CSDM_ID = " + strDeptId;
                else if (strLevel == "2")
                    strQuery = " and CDM_ID = " + strDeptId;

                dtDept = new DataServer().Getdata(" select SRD_ID from TBL_SUB_REPORTING_DEPT " +
                                    " inner join TBL_SRD_CSSDM_MAPPING ON SRD_ID = SRCM_SRD_ID AND SRCM_CSSDM_ID in " +
                                    " ( select CSSDM_ID from TBL_CERT_SUB_SUB_DEPT_MAS " +
                                    " inner join TBL_CERT_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID " +
                                    " inner join TBL_CERT_DEPT_MAS on CDM_ID = CSDM_CDM_ID " +
                                    " where 1=1 " + strQuery + " )");

                String strQMFromDT = (Convert.ToDateTime(DataServer.ExecuteScalar("select CQM_FROM_DATE FROM TBL_CERT_QUARTER_MAS where CQM_STATUS = 'A' "))).ToString("dd-MMM-yyyy");
                String strQMToDT = (Convert.ToDateTime(DataServer.ExecuteScalar("select CQM_TO_DATE FROM TBL_CERT_QUARTER_MAS where CQM_STATUS = 'A' "))).ToString("dd-MMM-yyyy");

                strReportingDeptId = "";

                for (int i = 0; i < dtDept.Rows.Count; i++)
                {
                    strReportingDeptId = strReportingDeptId + dtDept.Rows[i]["SRD_ID"].ToString() + ",";
                }

                if (strReportingDeptId != "")
                {

                    strReportingDeptId = strReportingDeptId.Substring(0, strReportingDeptId.Length - 1);

                    string strCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
                                                     " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                     " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                                                     " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                     " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ")" +
                                                     " and DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y' " +
                                                     " and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;


                    string strNonCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
                                                      " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                      " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                                                      " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                      " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ")" +
                                                      " and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') " +
                                                      "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                    string strNACount = " select count(1) from TBL_SUB_CHKLIST " +
                                                     " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                     " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID  " +
                                                     " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                     " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ") and SUB_YES_NO_NA = 'NA'" +
                                                      "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;


                    string strQTotal = " select Count(*) from TBL_SUB_CHKLIST " +
                                                " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                                                " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                                " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                                " where SM_SRD_ID in (" + strReportingDeptId + ") and  isnull(SUB_STATUS,'') != 'R'" +
                                                "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                    intCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strCompliedCount));
                    intNonCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strNonCompliedCount));
                    intNACount = Convert.ToInt32(DataServer.ExecuteScalar(strNACount));

                    intTotalCount = Convert.ToInt32(DataServer.ExecuteScalar(strQTotal));

                    strHtmlTable += "<tr><td class= 'DBTableCellLeft'><center>" + strDeptName + "</center></td>";
                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
                                    "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=Y&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                    "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    intCompliedCount +
                                    "</a>" +
                                    "</td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                        "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
                                        "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=N&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                        "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                        "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                        intNonCompliedCount +
                                        "</a>" +
                                        "</td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                        "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
                                        "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=NA&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                        "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                        "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                        +intNACount +
                                        "</a>" +
                                        "</td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                       "<a href='#' class='badge rounded-pill badge-soft-pink'   onclick=\"window.open(" +
                                       "'DetailedReportCertificationFilling.aspx?ReportType=1&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                       "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                       "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                       +intTotalCount +
                                       "</a>" +
                                       "</td></tr>";

                }
                strHtmlTable += "</table>";
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return strHtmlTable;
        }

        //DashBoard for the QuaterID(QID)
        public string GetFilingsDashboardSingleRow_QID(string strDeptId, string strDeptName, string strLevel, string strQuarterId)
        {
            DataTable dt = new DataTable();
            string strHtmlTable = "", strReportingDeptId = "";
            int intCompliedCount, intNonCompliedCount, intNACount, intTotalCount;
            string strQuery = "";
            DataTable dtDept = new DataTable();
            try
            {
                //litRegulatoryFilling.Text = "";
                strHtmlTable = "<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'>";
                strHtmlTable += "<tr><th>Department</td>";
                strHtmlTable += "<th>Compliant</td>";
                strHtmlTable += "<th>Not Compliant</td>";
                strHtmlTable += "<th>Not Applicable</td>";
                strHtmlTable += "<th>Total</td></tr>";

                intCompliedCount = 0;
                intNonCompliedCount = 0;
                intNACount = 0;
                intTotalCount = 0;

                if (strLevel == "0")
                    strQuery = " and CSSDM_ID = " + strDeptId;
                else if (strLevel == "1")
                    strQuery = " and CSDM_ID = " + strDeptId;
                else if (strLevel == "2")
                    strQuery = " and CDM_ID = " + strDeptId;

                dtDept = new DataServer().Getdata(" select SRD_ID from TBL_SUB_REPORTING_DEPT " +
                                    " inner join TBL_SRD_CSSDM_MAPPING ON SRD_ID = SRCM_SRD_ID AND SRCM_CSSDM_ID in " +
                                    " ( select CSSDM_ID from TBL_CERT_SUB_SUB_DEPT_MAS " +
                                    " inner join TBL_CERT_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID " +
                                    " inner join TBL_CERT_DEPT_MAS on CDM_ID = CSDM_CDM_ID " +
                                    " where 1=1 " + strQuery + " )");

                String strQMFromDT = (Convert.ToDateTime(DataServer.ExecuteScalar("select CQM_FROM_DATE FROM TBL_CERT_QUARTER_MAS where CQM_ID = " + strQuarterId))).ToString("dd-MMM-yyyy");
                String strQMToDT = (Convert.ToDateTime(DataServer.ExecuteScalar("select CQM_TO_DATE FROM TBL_CERT_QUARTER_MAS where CQM_ID = " + strQuarterId))).ToString("dd-MMM-yyyy");

                strReportingDeptId = "";

                for (int i = 0; i < dtDept.Rows.Count; i++)
                {
                    strReportingDeptId = strReportingDeptId + dtDept.Rows[i]["SRD_ID"].ToString() + ",";
                }

                if (strReportingDeptId != "")
                {

                    strReportingDeptId = strReportingDeptId.Substring(0, strReportingDeptId.Length - 1);

                    string strCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
                                                     " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                     " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                                                     " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                     " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ")" +
                                                     " and DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y' " +
                                                     " and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;


                    string strNonCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
                                                      " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                      " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                                                      " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                      " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ")" +
                                                      " and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') " +
                                                      "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                    string strNACount = " select count(1) from TBL_SUB_CHKLIST " +
                                                     " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                     " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID  " +
                                                     " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                     " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ") and SUB_YES_NO_NA = 'NA'" +
                                                      "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;


                    string strQTotal = " select Count(*) from TBL_SUB_CHKLIST " +
                                                " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                                                " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                                " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                                " where SM_SRD_ID in (" + strReportingDeptId + ") and  isnull(SUB_STATUS,'') != 'R'" +
                                                "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                    intCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strCompliedCount));
                    intNonCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strNonCompliedCount));
                    intNACount = Convert.ToInt32(DataServer.ExecuteScalar(strNACount));

                    intTotalCount = Convert.ToInt32(DataServer.ExecuteScalar(strQTotal));

                    strHtmlTable += "<tr><td class= 'DBTableCellLeft'><center>" + strDeptName + "</center></td>";
                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=Y&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                    "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    intCompliedCount +
                                    "</a>" +
                                    "</td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                        "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                        "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=N&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                        "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                        "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                        intNonCompliedCount +
                                        "</a>" +
                                        "</td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                        "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
                                        "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=NA&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                        "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                        "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                        +intNACount +
                                        "</a>" +
                                        "</td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                       "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                       "'DetailedReportCertificationFilling.aspx?ReportType=1&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                       "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                       "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                       +intTotalCount +
                                       "</a>" +
                                       "</td></tr>";

                    //strHtmlTable += "</table>";
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            strHtmlTable += "</table>";
            return strHtmlTable;
        }

        //DashBoard for MD CEO Active Quarter
        public string GetFilingsDashboard_MD_CEO_AQ()
        {
            DataTable dt = new DataTable();
            string strHtmlTable = "", strReportingDeptId = "", strDeptId = "", strDeptName = "";
            int intCompliedCount, intNonCompliedCount, intNACount, intTotalCount;
            DataTable dtDept = new DataTable();
            try
            {

                strHtmlTable = "<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'>";
                strHtmlTable += "<tr><th>Department</td>";
                strHtmlTable += "<th>Compliant</td>";
                strHtmlTable += "<th>Not Compliant</td>";
                strHtmlTable += "<th>Not Applicable</td>";
                strHtmlTable += "<th>Not Yet Submitted</td></tr>";

                dt = new DataServer().Getdata(" Select * from TBL_CERT_DEPT_MAS");

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    strDeptName = dt.Rows[j]["CDM_NAME"].ToString();
                    strDeptId = dt.Rows[j]["CDM_ID"].ToString();

                    intCompliedCount = 0;
                    intNonCompliedCount = 0;
                    intNACount = 0;
                    intTotalCount = 0;

                    //myconnection.Open();
                    dtDept = new DataServer().Getdata(" select SRD_ID from TBL_SUB_REPORTING_DEPT " +
                                        " inner join TBL_SRD_CSSDM_MAPPING ON SRD_ID = SRCM_SRD_ID AND SRCM_CSSDM_ID in " +
                                        " ( select CSSDM_ID from TBL_CERT_SUB_SUB_DEPT_MAS " +
                                        " inner join TBL_CERT_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID " +
                                        " inner join TBL_CERT_DEPT_MAS on CDM_ID = CSDM_CDM_ID " +
                                        " where CDM_ID = " + strDeptId + " )");

                    String strQMFromDT = (Convert.ToDateTime(DataServer.ExecuteScalar("select CQM_FROM_DATE FROM TBL_CERT_QUARTER_MAS where CQM_STATUS = 'A'"))).ToString("dd-MMM-yyyy");
                    String strQMToDT = (Convert.ToDateTime(DataServer.ExecuteScalar("select CQM_TO_DATE FROM TBL_CERT_QUARTER_MAS where CQM_STATUS = 'A'"))).ToString("dd-MMM-yyyy");

                    strReportingDeptId = "";

                    for (int i = 0; i < dtDept.Rows.Count; i++)
                    {
                        strReportingDeptId = strReportingDeptId + dtDept.Rows[i]["SRD_ID"].ToString() + ",";
                    }

                    if (strReportingDeptId != "")
                    {

                        strReportingDeptId = strReportingDeptId.Substring(0, strReportingDeptId.Length - 1);

                        string strCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
                                                         " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                         " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                                                         " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                         " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ")" +
                                                         " and DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y' " +
                                                         " and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;


                        string strNonCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
                                                          " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                          " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                                                          " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                          " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ")" +
                                                          " and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') " +
                                                          "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                        string strNACount = " select count(1) from TBL_SUB_CHKLIST " +
                                                         " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                         " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID  " +
                                                         " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                         " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ") and SUB_YES_NO_NA = 'NA'" +
                                                          "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;


                        string strQTotal = " select Count(*) from TBL_SUB_CHKLIST " +
                                                    " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                                                    " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                                    " where SM_SRD_ID in (" + strReportingDeptId + ") and  isnull(SUB_STATUS,'') != 'R'" +
                                                    "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                        intCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strCompliedCount));
                        intNonCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strNonCompliedCount));
                        intNACount = Convert.ToInt32(DataServer.ExecuteScalar(strNACount));

                        intTotalCount = Convert.ToInt32(DataServer.ExecuteScalar(strQTotal));

                        strHtmlTable += "<tr><td class= 'DBTableCellLeft'>" + strDeptName + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                        "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                        "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=Y&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                        "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                        "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                        intCompliedCount +
                                        "</a>" +
                                        "</td>";

                        strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                            "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                            "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=N&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                            "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                            intNonCompliedCount +
                                            "</a>" +
                                            "</td>";

                        strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                            "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                            "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=NA&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                            "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                            +intNACount +
                                            "</a>" +
                                            "</td>";

                        strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                           "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
                                           "'DetailedReportCertificationFilling.aspx?ReportType=1&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                           "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                           "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                           +intTotalCount +
                                           "</a>" +
                                           "</td></tr>";
                    }
                }
                strHtmlTable += "</table>";

            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return strHtmlTable;
        }

        //DashBoard for MD CEO QuarterID
        public string GetFilingsDashboard_MD_CEO_QID(string strQuarterID)
        {
            DataTable dt = new DataTable();
            string strHtmlTable = "", strReportingDeptId = "", strDeptId = "", strDeptName = "";
            int intCompliedCount, intNonCompliedCount, intNACount, intTotalCount;
            DataTable dtDept = new DataTable();
            try
            {

                strHtmlTable = "<table width='100%' class='table table-bordered footable' cellpadding='0' cellspacing='0'>";
                strHtmlTable += "<tr><th>Department</td>";
                strHtmlTable += "<th>Compliant</td>";
                strHtmlTable += "<th>Not Compliant</td>";
                strHtmlTable += "<th>Not Applicable</td>";
                strHtmlTable += "<th>Reopen/Sent back</td>";
                strHtmlTable += "<th>Total</td></tr>";

                dt = new DataServer().Getdata(" Select * from TBL_CERT_DEPT_MAS");

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    strDeptName = dt.Rows[j]["CDM_NAME"].ToString();
                    strDeptId = dt.Rows[j]["CDM_ID"].ToString();

                    intCompliedCount = 0;
                    intNonCompliedCount = 0;
                    intNACount = 0;
                    intTotalCount = 0;

                    //myconnection.Open();
                    dtDept = new DataServer().Getdata(" select SRD_ID from TBL_SUB_REPORTING_DEPT " +
                                        " inner join TBL_SRD_CSSDM_MAPPING ON SRD_ID = SRCM_SRD_ID AND SRCM_CSSDM_ID in " +
                                        " ( select CSSDM_ID from TBL_CERT_SUB_SUB_DEPT_MAS " +
                                        " inner join TBL_CERT_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID " +
                                        " inner join TBL_CERT_DEPT_MAS on CDM_ID = CSDM_CDM_ID " +
                                        " where CDM_ID = " + strDeptId + " )");

                    String strQMFromDT = (Convert.ToDateTime(DataServer.ExecuteScalar("select CQM_FROM_DATE FROM TBL_CERT_QUARTER_MAS where CQM_ID = " + strQuarterID))).ToString("dd-MMM-yyyy");
                    String strQMToDT = (Convert.ToDateTime(DataServer.ExecuteScalar("select CQM_TO_DATE FROM TBL_CERT_QUARTER_MAS where CQM_ID = " + strQuarterID))).ToString("dd-MMM-yyyy");

                    strReportingDeptId = "";

                    for (int i = 0; i < dtDept.Rows.Count; i++)
                    {
                        strReportingDeptId = strReportingDeptId + dtDept.Rows[i]["SRD_ID"].ToString() + ",";
                    }

                    if (strReportingDeptId != "")
                    {

                        strReportingDeptId = strReportingDeptId.Substring(0, strReportingDeptId.Length - 1);

                        string strCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
                                                         " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                         " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                                                         " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                         " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ")" +
                                                         " and DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y' " +
                                                         " and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;


                        string strNonCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
                                                          " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                          " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                                                          " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                          " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ") AND SUB_YES_NO_NA = 'N'" +
                                                          //" and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') " +
                                                          "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                        string strDelayedFiling = " select count(1) from TBL_SUB_CHKLIST " +
                                                          " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                          " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                                                          " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                          " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ") " +
                                                          " and (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') " +
                                                          "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                        string strNACount = " select count(1) from TBL_SUB_CHKLIST " +
                                                         " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                         " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID  " +
                                                         " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                         " where SUB_STATUS in ('S','C') and SM_SRD_ID in (" + strReportingDeptId + ") and SUB_YES_NO_NA = 'NA'" +
                                                          "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                        string strNotSubmittedCount = " select count(1) from TBL_SUB_CHKLIST " +
                                                         " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                         " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID  " +
                                                         " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                         " where (SUB_STATUS is null or SUB_STATUS = '') and SM_SRD_ID in (" + strReportingDeptId + ")" +
                                                          "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                        string strNotYetDueCount = " select count(1) from TBL_SUB_CHKLIST " +
                                                         " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                         " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID  " +
                                                         " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                         " where ISNULL(SUB_STATUS, '') = '' AND CAST(SC_DUE_DATE_TO AS DATE) > CAST('" + strQMToDT + "' AS DATE) and SM_SRD_ID in (" + strReportingDeptId + ")" +
                                                          "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                        string strDueandNotSubmitted = " select count(1) from TBL_SUB_CHKLIST " +
                                                         " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                         " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID  " +
                                                         " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                         " where ISNULL(SUB_STATUS, '') = '' AND CAST(SC_DUE_DATE_TO AS DATE) <= CAST('" + strQMToDT + "' AS DATE) and SM_SRD_ID in (" + strReportingDeptId + ")" +
                                                          "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                        string strRopnSR = " select count(1) from TBL_SUB_CHKLIST " +
                                                         " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                                                         " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID  " +
                                                         " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                                                         " where ISNULL(SUB_STATUS,'') in ('R','SR') and SM_SRD_ID in (" + strReportingDeptId + ")" +
                                                          "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                        string strQTotal = " select Count(*) from TBL_SUB_CHKLIST " +
                                                    " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                                                    " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                                    //" where SM_SRD_ID in (" + strReportingDeptId + ") and  isnull(SUB_STATUS,'') != 'R'" +
                                                    " where SM_SRD_ID in (" + strReportingDeptId + ") " +
                                                    " and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'" + " " + strCertFilingDashboardWhereClause;

                        intCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strCompliedCount));
                        intNonCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strNonCompliedCount));
                        int intDelayedFiling = Convert.ToInt32(DataServer.ExecuteScalar(strDelayedFiling));
                        int intNotSubmittedCount = Convert.ToInt32(DataServer.ExecuteScalar(strNotSubmittedCount));
                        int intNotYetDueCount = Convert.ToInt32(DataServer.ExecuteScalar(strNotYetDueCount));
                        int intDueandNotSubmitted = Convert.ToInt32(DataServer.ExecuteScalar(strDueandNotSubmitted));
                        int intRopnSR = Convert.ToInt32(DataServer.ExecuteScalar(strRopnSR));

                        intNACount = Convert.ToInt32(DataServer.ExecuteScalar(strNACount));

                        intTotalCount = Convert.ToInt32(DataServer.ExecuteScalar(strQTotal));

                        strHtmlTable += "<tr><td class= 'DBTableCellLeft'>" + strDeptName + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                        "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                        "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=Y&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                        "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                        "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                        intCompliedCount +
                                        "</a>" +
                                        "</td>";

                        strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                            "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                            "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=N&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                            "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                            intNonCompliedCount +
                                            "</a>" +
                                            "</td>";

                        strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                            "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
                                            "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=NA&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                            "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                            +intNACount +
                                            "</a>" +
                                            "</td>";

                        strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                            "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
                                            "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=SR&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                            "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                            +intRopnSR +
                                            "</a>" +
                                            "</td>";

                        strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                           "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
                                           "'DetailedReportCertificationFilling.aspx?ReportType=1&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
                                           "&SRDID=" + strReportingDeptId + "&STMID=&Frequency=&Priority='," +
                                           "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                           +intTotalCount +
                                           "</a>" +
                                           "</td></tr>";
                    }
                }
                strHtmlTable += "</table>";

            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return strHtmlTable;
        }
    }
}