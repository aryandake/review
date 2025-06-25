using System;
using System.Data;
using System.Web.UI;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class Submissions_DashboardDetailedReport : System.Web.UI.Page
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
                    retQry = " INNER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID " +
                        "INNER JOIN TBL_EM_STM_MAPPING ON SC_STM_ID = ESM_STM_ID " +
                        "INNER JOIN EmployeeMaster ON ESM_EM_ID = EM_ID AND EM_USERNAME = '" + strUsername + "' AND EM_EMAIL = '" + strEmailId + "'";
                }
                else
                {
                    retQry = " LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID ";
                }
                //else if (strUserRole == "Other")
                //{
                //    retQry = " LEFT OUTER JOIN TBL_SUB_SRD_OWNER_MASTER ON SRDOM_SRD_ID = SRD_ID AND SRDOM_EMP_ID = '" + strUsername + "' AND SRDOM_EMAILID = '" + strEmailId + "'" +
                //        " LEFT OUTER JOIN TBL_SUB_ESCALATION ON SE_SRD_ID=SRD_ID AND SE_EMPLOYEE_ID = '" + strUsername + "' AND SE_EMAIL_ID = '" + strEmailId + "'";
                //}
            }
            return retQry;
        }

        public string ReturnDeptCheckQry()
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

                if (strUserRole == "Other")
                {
                    retQry = " AND SRD_ID IN " +
                    " (SELECT SRDOM_SRD_ID FROM TBL_SUB_SRD_OWNER_MASTER WHERE SRDOM_STATUS = 'A' " +
                    " AND SRDOM_SRD_ID = SRD_ID AND SRDOM_EMP_ID = '" + strUsername + "' AND SRDOM_EMAILID = '" + strEmailId + "' " +
                    " UNION " +
                    " SELECT SE_SRD_ID FROM TBL_SUB_ESCALATION WHERE SE_STATUS = 'A' " +
                    " AND SE_SRD_ID = SRD_ID AND SE_EMPLOYEE_ID = '" + strUsername + "' AND SE_EMAIL_ID = '" + strEmailId + "' )";
                }
            }
            return retQry;
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
            strHtmlTable1 += "</table>";
        }

        public void getDetailedComplianceWiseStatus(string strConnectionString)
        {
            string strTrackedBy = "", strReportingFunction = "", strFrequency = ""
                     , strFromDate = "", strToDate = "", strPriority = "", strStatus = "", strHeading = "";
            string strFilter = "", strQry1 = "", strHtmlTable = "", strSubDateAuthority = "", strSubDate = "",
                    strClosureDate = "", strCompliantTable = "", strNonCompliantTable = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;
            int intCnt;

            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Status"]))
                {
                    strStatus = Request.QueryString["Status"].ToString();
                    if (strStatus != "")
                    {
                        //Tracking Department
                        if (strStatus == "T_TDA")
                        {
                            strFilter += @" AND SUB_STATUS = 'S'
                                AND SC_DUE_DATE_TO > CURRENT_TIMESTAMP
                                AND MONTH(SC_DUE_DATE_TO) = MONTH(CURRENT_TIMESTAMP) ";
                        }
                        else if (strStatus == "T_TNCNC")
                        {
                            strFilter += " AND SUB_STATUS = 'S' AND SC_DUE_DATE_TO <= CURRENT_TIMESTAMP ";
                        }
                        else if (strStatus == "T_TNSNC")
                        {
                            strFilter += " AND ISNULL(SUB_STATUS, '') = '' AND SC_DUE_DATE_TO <= CURRENT_TIMESTAMP ";
                        }
                        else if (strStatus == "T_TOWT")
                        {
                            strFilter += @" AND ISNULL(SUB_STATUS, '') = ''
					                    AND SC_DUE_DATE_TO > CURRENT_TIMESTAMP
					                    AND MONTH(SC_DUE_DATE_TO) = MONTH(CURRENT_TIMESTAMP)";
                        }
                        else if (strStatus == "T_CT")
                        {
                            strFilter += @" AND SUB_STATUS = 'C'
							            AND SUB_SUBMITTED_TO_AUTHORITY_ON <= SC_DUE_DATE_TO 
							            AND SUB_YES_NO_NA = 'Y' 
                                        AND MONTH(SC_DUE_DATE_TO) = MONTH(CURRENT_TIMESTAMP)";
                        }
                        else if (strStatus == "T_NCT")
                        {
                            strFilter += @" AND SUB_STATUS = 'C'
							            AND ((SUB_SUBMITTED_TO_AUTHORITY_ON > SC_DUE_DATE_TO 
							            AND SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') 
                                        AND MONTH(SC_DUE_DATE_TO) = MONTH(CURRENT_TIMESTAMP)";
                        }
                        //Reporting Department
                        else if (strStatus == "R_TDA")
                        {
                            strFilter += @" AND ISNULL(SUB_STATUS, '') = ''
					                    AND SC_DUE_DATE_FROM >= CURRENT_TIMESTAMP
					                    AND MONTH(SC_DUE_DATE_FROM) = MONTH(CURRENT_TIMESTAMP) ";
                        }
                        else if (strStatus == "R_TOA")
                        {
                            strFilter += @" AND ISNULL(SUB_STATUS, '') = ''
								AND CAST(SC_DUE_DATE_FROM AS DATE) < CAST(CURRENT_TIMESTAMP AS DATE) ";
                        }
                        else if (strStatus == "R_NCTNS")
                        {
                            strFilter += @" AND SUB_STATUS = 'S'
						                AND ((isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) > SC_DUE_DATE_TO 
						                AND SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') ";
                        }
                        else if (strStatus == "R_CT")
                        {
                            strFilter += @" AND SUB_STATUS = 'C'
							            AND SUB_SUBMITTED_TO_AUTHORITY_ON <= SC_DUE_DATE_TO 
							            AND SUB_YES_NO_NA = 'Y' 
                                        AND MONTH(SC_DUE_DATE_TO) = MONTH(CURRENT_TIMESTAMP)";
                        }
                        else if (strStatus == "R_NCT")
                        {
                            strFilter += @" AND SUB_STATUS = 'C'
							            AND ((SUB_SUBMITTED_TO_AUTHORITY_ON > SC_DUE_DATE_TO 
							            AND SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') 
                                        AND MONTH(SC_DUE_DATE_TO) = MONTH(CURRENT_TIMESTAMP)";
                        }
                    }
                }


                strQry1 = "SELECT *,SUB_CREAT_BY,SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_CLOSED_BY,SUB_CLOSED_ON,SUB_SUBMIT_DATE, " +
                          " case when (DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') then 'Compliant' " +
                          " when ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') then 'Not Compliant' " +
                          " when SUB_YES_NO_NA = 'NA' then 'Not Applicable' " +
                          " else 'Not Yet Submitted' end " +
                          " as Status " +
                          " FROM TBL_SUB_CHKLIST " +
                          " INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                          " INNER JOIN TBL_SUB_TYPE_MAS ON SC_STM_ID = STM_ID " +
                          " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                          ReturnRoleBasedQry() +
                          " where 1 = 1 " + ReturnDeptCheckQry() + strFilter +
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

                    //if (strStatus == "T_CT"|| strStatus == "R_CT")
                    //{
                    //    strCompliantTable = getCompliantSplitUp(strFilter);
                    //    litDetails.Text = strCompliantTable + "<br/>" + strHtmlTable;
                    //}
                    //else if (strStatus == "NCT")
                    //{
                    //    if (!string.IsNullOrEmpty(Request.QueryString["UName"]) & !string.IsNullOrEmpty(Request.QueryString["UMail"]) &
                    //    !string.IsNullOrEmpty(Request.QueryString["Role"]))
                    //    {
                    //        strNonCompliantTable = getNotCompliantSplitUp(strFilter);
                    //        litDetails.Text = strNonCompliantTable + "<br/>" + strHtmlTable;
                    //    }
                    //    else
                    //    {
                    //        litDetails.Text = strHtmlTable;
                    //    }
                    //}
                    //else
                    //{
                    //    litDetails.Text = strHtmlTable;
                    //}

                    litDetails.Text = strHtmlTable;
                    Session["strDetailedReport"] = strHtmlTable;
                }

                if (strStatus == "T_TDA" || strStatus == "R_TDA")
                {
                    strHeading = "- (Tasks Due for Action)";
                }
                else if (strStatus == "T_TNCNC")
                {
                    strHeading = "- (Tasks Not Closed and Non Compliant)"; ;
                }
                else if (strStatus == "T_TNSNC")
                {
                    strHeading = "- (Tasks Not Submitted & Non Compliant)";
                }
                else if (strStatus == "T_TOWT")
                {
                    strHeading = "- (Tasks Open within timelines)";
                }
                else if (strStatus == "T_CT" || strStatus == "R_CT")
                {
                    strHeading = "- (Compliant Tasks)";
                }
                else if (strStatus == "T_NCT" || strStatus == "R_NCT")
                {
                    strHeading = "- (Non Compliant Tasks)";
                }
                else if (strStatus == "R_TOA")
                {
                    strHeading = "- (Tasks Overdue for Action)";
                }
                else if (strStatus == "R_NCTNS")
                {
                    strHeading = "- (Non Compliant Tasks & Not Closed)";
                }
                else
                {
                    strHeading = "- (Total)";
                }

                lblHeading.Text = "Detailed Report for " + strHeading;
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
                                            " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                                            " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " + ReturnCutoffFilterQry() +
                                            " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                            " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                            " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " +
                                            " where SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) <= SC_DUE_DATE_TO " + strFilter;

            string strSubmittedBeforeDueDate = " select count(*) from TBL_SUB_CHKLIST " +
                                           " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                                           " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " + ReturnCutoffFilterQry() +
                                           " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                           " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                           " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID  " +
                                           " where SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_SUBMIT_DATE)) <= SC_DUE_DATE_TO and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) > SC_DUE_DATE_TO " + strFilter;

            string strSubmittedAfterDueDate = " select count(*) from TBL_SUB_CHKLIST " +
                                           " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                                           " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " + ReturnCutoffFilterQry() +
                                           " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                           " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                           " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID   " +
                                           " where SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_SUBMIT_DATE)) > SC_DUE_DATE_TO and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) > SC_DUE_DATE_TO " + strFilter;

            string strNotClosed = " select count(*) from TBL_SUB_CHKLIST " +
                                  " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                                  " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " + ReturnCutoffFilterQry() +
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
                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " + ReturnCutoffFilterQry() +
                              " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID  " +
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

    }
}