using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Text.RegularExpressions;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Submissions_DetailedReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
            if (!Page.IsPostBack)
            {
                try
                {
                    getdetailsReport(mstrConnectionString);
                }
                catch (Exception ex)
                {
                    //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                    string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    //>>
                    writeError("Invalid Parameter input." + ex);
                }
            }
        }

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

            if (Request.QueryString["ReportType"] != null)
            {
                strReportTypeId = Request.QueryString["ReportType"].ToString();
                hfReportType.Value = strReportTypeId;
            }

            if (strReportTypeId.Equals("2"))
            {
                getDetailedComplianceWiseStatus(strConnectionString);
            }
            if (strReportTypeId.Equals("1"))
            {
                getDetailedReportDeptWise(strConnectionString);
            }
            //<<Added by Rahuldeb on 09Mar2018
            if (strReportTypeId.Equals("3"))
            {
                getMonthlyComplianceReport(strConnectionString);
            }
            //>>
        }

        public void getDetailedReportDeptWise(string strConnectionString)
        {
            string strReportingFunction = "", strFrequency = ""
                     , strFromDate = "", strToDate = "", strPriority = "", strStatus = "", strHeading = "", strDeptId = "";
            string strFilter = "", strQry1 = "", strHtmlTable = "", strDeptName = "", strSubDateAuthority = "", strSubDate = "",
                    strClosureDate = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;
            int intCnt;

            string strCertFilingDashboardWhereClause = "";

            try
            {
                using (F2FDatabase DB = new F2FDatabase("Select CP_VALUE from TBL_CONFIG_PARAMS where CP_ID = 12 "))
                {
                    DB.OpenConnection();
                    strCertFilingDashboardWhereClause = DB.F2FCommand.ExecuteScalar().ToString();
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("system exception in getDetailedReportDeptWise() " + ex);
            }

            try
            {
                if (Request.QueryString["STMID"] != null)
                {
                    strDeptId = Request.QueryString["STMID"].ToString();
                }
                if (Request.QueryString["DeptName"] != null)
                {
                    strDeptName = Request.QueryString["DeptName"].ToString();
                }
                if (Request.QueryString["Status"] != null)
                {
                    strStatus = Request.QueryString["Status"].ToString();
                    if (strStatus.Equals("Y"))
                    {
                        strFilter += " and DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_STATUS in ('S','C') and SUB_YES_NO_NA = 'Y' ";
                    }
                    else if (strStatus.Equals("N"))
                    {
                        strFilter += " and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') " +
                                        " and SUB_STATUS in ('S','C') ";
                    }
                    else if (strStatus.Equals("NA"))
                    {
                        strFilter += " and SUB_YES_NO_NA = '" + strStatus + "' and SUB_STATUS in ('S','C') ";
                    }
                    else if (strStatus == "SR")
                    {
                        strFilter += " and SUB_STATUS in ('R','SR') ";//SUB_STATUS in ('C','S')
                    }
                    //else
                    //{
                    //    strFilter += " and isnull(SUB_STATUS,'') != 'R' ";
                    //}
                }

                if (Request.QueryString["FromDate"] != null)
                {
                    strFromDate = Request.QueryString["FromDate"].ToString();
                    if (strFromDate != "")
                    {
                        strFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
                    }
                }

                if (Request.QueryString["ToDate"] != null)
                {
                    strToDate = Request.QueryString["ToDate"].ToString();
                    if (strToDate != "")
                    {
                        strFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
                    }
                }

                if (Request.QueryString["SRDID"] != null)
                {
                    strReportingFunction = Request.QueryString["SRDID"].ToString();
                    if (strReportingFunction != "")
                    {
                        strFilter += " and SM_SRD_ID in (" + strReportingFunction + " ) ";
                    }
                }

                if (Request.QueryString["Frequency"] != null)
                {
                    strFrequency = Request.QueryString["Frequency"].ToString();
                    if (strFrequency != "")
                    {
                        strFilter += " and SM_FREQUENCY = '" + strFrequency + "' ";
                    }
                }

                if (Request.QueryString["Priority"] != null)
                {
                    strPriority = Request.QueryString["Priority"].ToString();
                    if (strPriority != "")
                    {
                        strFilter += " and SC_PRIORITY = '" + strPriority + "' ";
                    }
                }

                strQry1 = " select  *,case when SUB_YES_NO_NA = 'Y' then 'Compliant' " +
                          " when SUB_YES_NO_NA = 'N' then 'Not Compliant' " +
                          " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                          " else 'Not Yet Submitted' end " +
                          " as Status " +
                          " from  TBL_SUB_CHKLIST " +
                          " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID" +
                          " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                          " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                          " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                          " where 1=1 " + strFilter + " " + strCertFilingDashboardWhereClause;
                dt = F2FDatabase.F2FGetDataTable(strQry1);
                if (dt.Rows.Count > 0)
                {
                    btnExportToExcel.Visible = true;
                    strHtmlTable = "<table width='100%' class='table table-bordered footable'>";
                    strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Sr.No.</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Tracking Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reporting Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Particulars</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Description</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Frequency</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Due Date From</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Due Date To</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Remarks</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted By</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted in System on</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submission Date to Authority</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed By</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Closed On</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Yes/No/NA</td></tr>";


                    for (intCnt = 0; intCnt < dt.Rows.Count; intCnt++)
                    {
                        dr = dt.Rows[intCnt];

                        strSubDate = dr["SUB_SUBMIT_DATE"].Equals(DBNull.Value) ? "&nbsp;" :
                                     Convert.ToDateTime(dr["SUB_SUBMIT_DATE"]).ToString("dd-MMM-yyyy HH:mm:ss");

                        strSubDateAuthority = dr["SUB_SUBMITTED_TO_AUTHORITY_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                              Convert.ToDateTime(dr["SUB_SUBMITTED_TO_AUTHORITY_ON"]).ToString("dd-MMM-yyyy");

                        strClosureDate = dr["SUB_CLOSED_ON"].Equals(DBNull.Value) ? "&nbsp;" :
                                         Convert.ToDateTime(dr["SUB_CLOSED_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");

                        strHtmlTable += "<tr><td class= 'DBTableCellRight'>" + (intCnt + 1) + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["STM_TYPE"].ToString() + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SRD_NAME"].ToString() + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_PARTICULARS"].ToString().Replace("\n", "<br />") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_DESCRIPTION"].ToString().Replace("\n", "<br />") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_FREQUENCY"].ToString() + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_FROM"]).ToString("dd-MMM-yyyy") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_TO"]).ToString("dd-MMM-yyyy") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_REMARKS"].ToString().Replace("\n", "<br />") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CREAT_BY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDate + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strSubDateAuthority + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_CLOSED_BY"].ToString() + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + strClosureDate + "&nbsp;</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["status"].ToString() + "</td></tr>";
                    }

                    strHtmlTable = strHtmlTable + "</table>";
                    litDetails.Text = strHtmlTable;
                    Session["strDetailedReport"] = strHtmlTable;
                }

                if (strStatus == "Y")
                {
                    strHeading = "- (Compliant)";
                    lblHeading.Text = "Regulatory Filling - " + strDeptName + " for " + strHeading;
                }
                else if (strStatus == "N")
                {
                    strHeading = "- (Not Compliant)";
                    lblHeading.Text = "Regulatory Filling - " + strDeptName + " for " + strHeading;
                }
                else if (strStatus == "NA")
                {
                    strHeading = "- (Not Applicable)";
                    lblHeading.Text = "Regulatory Filling - " + strDeptName + " for " + strHeading;
                }
                else if (strStatus == "SR")
                {
                    strHeading = "- (Reopened / Sent back)";
                    lblHeading.Text = "- Regulatory Filling - " + strDeptName + " for " + strHeading;
                }
                else
                {
                    lblHeading.Text = "Regulatory Filling - " + strDeptName;
                }


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
            string strFilter = "", strQry1 = "", strHtmlTable = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;
            int intCnt;
            try
            {
                if (Request.QueryString["Status"] != null)
                {
                    strStatus = Request.QueryString["Status"].ToString();
                    if (strStatus != "")
                    {
                        strFilter += " and SUB_YES_NO_NA = '" + strStatus + "' ";
                    }
                    else
                    {
                        strFilter += " and SUB_ID is null ";
                    }
                }

                if (Request.QueryString["FromDate"] != null)
                {
                    strFromDate = Request.QueryString["FromDate"].ToString();
                    if (strFromDate != "")
                    {
                        strFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
                    }
                }

                if (Request.QueryString["ToDate"] != null)
                {
                    strToDate = Request.QueryString["ToDate"].ToString();
                    if (strToDate != "")
                    {
                        strFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
                    }
                }

                if (Request.QueryString["SRDID"] != null)
                {
                    strReportingFunction = Request.QueryString["SRDID"].ToString();
                    if (strReportingFunction != "")
                    {
                        strFilter += " and SM_SRD_ID = " + strReportingFunction + " ";
                    }
                }

                if (Request.QueryString["STMID"] != null)
                {
                    strTrackedBy = Request.QueryString["STMID"].ToString();
                    if (strTrackedBy != "")
                    {
                        strFilter += " and SM_STM_ID = " + strTrackedBy + " ";
                    }
                }

                if (Request.QueryString["Frequency"] != null)
                {
                    strFrequency = Request.QueryString["Frequency"].ToString();
                    if (strFrequency != "")
                    {
                        strFilter += " and SM_FREQUENCY = '" + strFrequency + "' ";
                    }
                }

                if (Request.QueryString["Priority"] != null)
                {
                    strPriority = Request.QueryString["Priority"].ToString();
                    if (strPriority != "")
                    {
                        strFilter += " and SC_PRIORITY = '" + strPriority + "' ";
                    }
                }

                strQry1 = "SELECT *,case when SUB_YES_NO_NA = 'Y' then 'Compliant' " +
                          "when SUB_YES_NO_NA = 'N' then 'Not Compliant' " +
                          "when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                          "else 'Not Yet Submitted' end " +
                          "as Status " +
                          "FROM TBL_SUB_CHKLIST " +
                          "INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                          "INNER JOIN TBL_SUB_TYPE_MAS ON SC_STM_ID = STM_ID " +
                          "INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                          "LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where 1=1 " + strFilter;

                using (F2FDatabase DB = new F2FDatabase(strQry1))
                {
                    DB.F2FDataAdapter.Fill(dt);
                }

                if (dt.Rows.Count > 0)
                {
                    btnExportToExcel.Visible = true;
                    strHtmlTable = "<table width='100%' cellpadding='0' cellspacing='0'>";
                    strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Sr.No.</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Tracking Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reporting Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Particulars</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Description</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Frequency</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Due Date From</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Due Date To</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Remarks</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Yes/No/NA</td></tr>";


                    for (intCnt = 0; intCnt < dt.Rows.Count; intCnt++)
                    {
                        dr = dt.Rows[intCnt];
                        strHtmlTable += "<tr><td class= 'DBTableCellRight'>" + (intCnt + 1) + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["STM_TYPE"].ToString() + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SRD_NAME"].ToString() + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_PARTICULARS"].ToString().Replace("\n", "<br />") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_DESCRIPTION"].ToString().Replace("\n", "<br />") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_FREQUENCY"].ToString() + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_FROM"]).ToString("dd-MMM-yyyy") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_TO"]).ToString("dd-MMM-yyyy") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_REMARKS"].ToString().Replace("\n", "<br />") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["status"].ToString() + "</td></tr>";
                    }

                    strHtmlTable = strHtmlTable + "</table>";
                    litDetails.Text = strHtmlTable;
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
                else
                {
                    strHeading = "- (Not Yet Submitted)";
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
            string strFilter = "", strQry1 = "", strHtmlTable = "", strMonthYear = "";
            ;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;
            int intCnt;

            try
            {
                if (Request.QueryString["Status"] != null)
                {
                    strStatus = Request.QueryString["Status"].ToString();
                    if (strStatus != "")
                    {
                        strFilter += " and SUB_YES_NO_NA = '" + strStatus + "' ";
                    }
                    else
                    {
                        strFilter += " and SUB_ID is null ";
                    }
                }

                if (Request.QueryString["FromDate"] != null)
                {
                    strFromDate = Request.QueryString["FromDate"].ToString();
                    if (strFromDate != "")
                    {
                        strFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
                    }
                }

                if (Request.QueryString["ToDate"] != null)
                {
                    strToDate = Request.QueryString["ToDate"].ToString();
                    if (strToDate != "")
                    {
                        strFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
                    }
                }

                if (Request.QueryString["SRDID"] != null)
                {
                    strReportingFunction = Request.QueryString["SRDID"].ToString();
                    if (strReportingFunction != "")
                    {
                        strFilter += " and SM_SRD_ID = " + strReportingFunction + " ";
                    }
                }

                if (Request.QueryString["STMID"] != null)
                {
                    strTrackedBy = Request.QueryString["STMID"].ToString();
                    if (strTrackedBy != "")
                    {
                        strFilter += " and SM_STM_ID = " + strTrackedBy + " ";
                    }
                }

                if (Request.QueryString["Frequency"] != null)
                {
                    strFrequency = Request.QueryString["Frequency"].ToString();
                    if (strFrequency != "")
                    {
                        strFilter += " and SM_FREQUENCY = '" + strFrequency + "' ";
                    }
                }

                if (Request.QueryString["Priority"] != null)
                {
                    strPriority = Request.QueryString["Priority"].ToString();
                    if (strPriority != "")
                    {
                        strFilter += " and SC_PRIORITY = '" + strPriority + "' ";
                    }
                }

                if (Request.QueryString["MonthYear"] != null)
                {
                    strMonthYear = Request.QueryString["MonthYear"].ToString();
                    if (strMonthYear != "")
                    {
                        strFilter += " and month(SC_DUE_DATE_TO) = " + strMonthYear.Split('-')[0] + " and year(SC_DUE_DATE_TO) = '" + strMonthYear.Split('-')[1] + "' ";
                    }
                }

                strQry1 = "SELECT *,case when SUB_YES_NO_NA = 'Y' then 'Compliant' " +
                             "when SUB_YES_NO_NA = 'N' then 'Not Compliant' " +
                             "when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                             "else 'Not Yet Submitted' end " +
                             "as Status " +
                             "FROM TBL_SUB_CHKLIST " +
                             "INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                             "INNER JOIN TBL_SUB_TYPE_MAS ON SC_STM_ID = STM_ID " +
                             "INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                             "LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where 1=1 " + strFilter;

                dt = F2FDatabase.F2FGetDataTable(strQry1);

                if (dt.Rows.Count > 0)
                {
                    btnExportToExcel.Visible = true;
                    strHtmlTable = "<table width='100%' cellpadding='0' cellspacing='0'>";
                    strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Sr.No.</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Tracking Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reporting Function</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Particulars</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Description</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Frequency</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Due Date From</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Due Date To</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Remarks</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Yes/No/NA</td></tr>";


                    for (intCnt = 0; intCnt < dt.Rows.Count; intCnt++)
                    {
                        dr = dt.Rows[intCnt];
                        strHtmlTable += "<tr><td class= 'DBTableCellRight'>" + (intCnt + 1) + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["STM_TYPE"].ToString() + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SRD_NAME"].ToString() + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_PARTICULARS"].ToString().Replace("\n", "<br />") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_DESCRIPTION"].ToString().Replace("\n", "<br />") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SC_FREQUENCY"].ToString() + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_FROM"]).ToString("dd-MMM-yyyy") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + Convert.ToDateTime(dr["SC_DUE_DATE_TO"]).ToString("dd-MMM-yyyy") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["SUB_REMARKS"].ToString().Replace("\n", "<br />") + "</td>";
                        strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["status"].ToString() + "</td></tr>";
                    }

                    strHtmlTable = strHtmlTable + "</table>";
                    litDetails.Text = strHtmlTable;
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
                else
                {
                    strHeading = "- (Not Yet Submitted)";
                }

                lblHeading.Text = "Monthly Compliance Report " + strHeading;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("system exception in getCriticalitySummaryReport(): " + ex);
            }
        }
        //>>

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            string strHTMLReport = "";
            string attachment, html2;

            if (Session["strDetailedReport"] != null)
            {
                strHTMLReport = Session["strDetailedReport"].ToString();
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
    }
}