using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.App_Code;
using System.Text;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class Submissions_Dashboard : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        SubmissionMasterBLL SubmissionMasterBLL = new SubmissionMasterBLL();
        UtilitiesBLL utilityBL = new UtilitiesBLL();
        UtilitiesBLL utilBLL = new UtilitiesBLL();
        UtilitiesVO utilVO = new UtilitiesVO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlSubType.DataSource = utilityBL.getDataset("SUBTYPE", mstrConnectionString);
                ddlSubType.DataBind();

                ddlReportDept.DataSource = utilityBL.getDataset("REPORTINGDEPT", mstrConnectionString);
                ddlReportDept.DataBind();

                //txtFromdate.Text = "01-Apr-2023";
                //txtTodate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            string strReportType = "";
            strReportType = ddlReportType.SelectedValue.ToString();

            if (strReportType.Equals("1"))
            {
                getTrackingDeptWiseCompliedNotCompliedCount();
                divgraphheading.InnerText = "Tracking Function wise Compliant, Non-Compliant, Not Applicable & Not Submitted Tasks Count";
            }
            else if (strReportType.Equals("2"))
            {
                getComplianceWiseStatus();
            }
            //<<Added by Rahuldeb on 09Mar2018
            else if (strReportType.Equals("3"))
            {
                getMonthlyComplianceReport();
            }
            //>>
            else if (strReportType.Equals("4"))
            {
                getRegulatoryDashboardforPrevMonth();
            }
            else if (strReportType.Equals("5"))
            {
                getReportingDeptWiseCompliedNotCompliedCount();
                divgraphheading.InnerText = "Reporting Function wise Compliant, Non-Compliant, Not Applicable & Not Submitted Tasks Count";
            }
        }
        #region Old Code
        //private void getTrackingDeptWiseCompliedNotCompliedCount()
        //{
        //    DataTable dt = new DataTable();
        //    DataRow dr;
        //    SqlConnection myconnection = new SqlConnection(mstrConnectionString);
        //    string strDeptName = "", strDeptId = "",
        //         strChart = "", strHtmlTable = "";
        //    string strReportingFunction = "", strFrequency = ""
        //           , strFromDate = "", strToDate = "", strPriority = "", strFilterQuery = "";
        //    int intCompliedCount, intNonCompliedCount, intNACount, intNotYetDue = 0,
        //        intTotalCount, intDueandNotSubmitted = 0, intOverAllCount = 0;
        //    dt = (utilBLL.getDataSet("TrackingDept")).Tables[0];

        //    strHtmlTable = "<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'>";
        //    strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Department</td>";
        //    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Compliant</td>";
        //    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Compliant</td>";
        //    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Applicable</td>";
        //    //strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted but not Closed</td>";
        //    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Due but not Submitted</td>";
        //    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Yet Due</td>";
        //    //strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Yet Submitted</td>";
        //    //<<Added by Kiran Kharat On 20Mar2018
        //    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Total</td></tr>";
        //    //>>
        //    pnlHideShow("pnlDeptWiseBarchart");
        //    strChart = "<script type=\"text/javascript\"> Morris.Bar({element: 'bar-chart' ,data: [";
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            dr = dt.Rows[i];
        //            strDeptName = dr["STM_TYPE"].ToString();
        //            strDeptId = dr["STM_ID"].ToString();


        //            //strQuery1 = " select  STM_TYPE,SUB_YES_NO_NA,count(1) as TotCount,STM_ID from  TBL_SUB_CHKLIST " +
        //            //            " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  and STM_ID = " + strDeptId +
        //            //            " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
        //            //            " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
        //            //            " where SUB_STATUS in('S','C') ";//and 

        //            if (!ddlReportDept.SelectedValue.Equals(""))
        //            {
        //                strFilterQuery += " and SM_SRD_ID = " + ddlReportDept.SelectedValue;
        //                //strQuery1 += " and SM_SRD_ID = " + ddlReportDept.SelectedValue;
        //                strReportingFunction = ddlReportDept.SelectedValue;
        //            }

        //            if (!ddlFrequency.SelectedValue.Equals(""))
        //            {
        //                strFilterQuery += " and SM_FREQUENCY = '" + ddlFrequency.SelectedValue + "' ";
        //                //strQuery1 += " and SM_FREQUENCY = '" + ddlFrequency.SelectedValue + "' ";
        //                strFrequency = ddlFrequency.SelectedValue;
        //            }

        //            if (!txtFromdate.Text.Equals(""))
        //            {
        //                strFilterQuery += " and SC_DUE_DATE_TO >= '" + txtFromdate.Text + "' ";
        //                //strQuery1 += " and SC_DUE_DATE_FROM >= '" + txtFromdate.Text + "' ";
        //                strFromDate = txtFromdate.Text;
        //            }

        //            if (!txtTodate.Text.Equals(""))
        //            {
        //                strFilterQuery += " and SC_DUE_DATE_TO <= '" + txtTodate.Text + "' ";
        //                //strQuery1 += " and SC_DUE_DATE_TO <= '" + txtTodate.Text + "' ";
        //                strToDate = txtTodate.Text;
        //            }

        //            if (!ddlPriority.SelectedValue.Equals(""))
        //            {
        //                strFilterQuery += " and SC_PRIORITY = '" + ddlPriority.SelectedValue + "' ";
        //                //strQuery1 += " and SC_PRIORITY = '" + ddlPriority.SelectedValue + "' ";
        //                strPriority = ddlPriority.SelectedValue;
        //            }

        //            //strQuery1 += " group by STM_TYPE,STM_ID,SUB_YES_NO_NA order by STM_TYPE ASC ";

        //            //DataTable dtCount = new DataTable();
        //            //SqlCommand cmd = new SqlCommand(strQuery1, myconnection);
        //            //SqlDataAdapter searchDataAdaptor = new SqlDataAdapter(cmd);
        //            //searchDataAdaptor.Fill(dtCount);


        //            intCompliedCount = 0;
        //            intNonCompliedCount = 0;
        //            intNACount = 0;
        //            intTotalCount = 0;
        //            intNotYetDue = 0;
        //            intDueandNotSubmitted = 0;
        //            //intCompliedTotalCount = 0;
        //            //intNonCompliedTotalCount = 0;
        //            //intNATotalCount = 0;
        //            //intNotSubmittedTotal = 0;
        //            //intOverAllCount = 0;


        //            string strCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
        //                                      " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
        //                                      " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID and SM_STM_ID = " + strDeptId +
        //                                      " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
        //                                      " where SUB_STATUS in ('S','C') " + strFilterQuery +
        //                                      " and DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y' ";


        //            string strNonCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
        //                                      " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
        //                                      " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID and SM_STM_ID = " + strDeptId +
        //                                      " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
        //                                      " where SUB_STATUS in ('S','C') " + strFilterQuery +
        //                                      //<<Commented By Amey Karangutkar on 28-Jul-2018
        //                                      //" and (isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) > SC_DUE_DATE_TO OR SUB_YES_NO_NA = 'N') ";
        //                                      //>>
        //                                      " and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') ";

        //            string strNACount = " select count(1) from TBL_SUB_CHKLIST " +
        //                                     " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
        //                                     " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID and SM_STM_ID = " + strDeptId +
        //                                     " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
        //                                     " where SUB_STATUS in ('S','C') " + strFilterQuery + " and SUB_YES_NO_NA = 'NA'";

        //            //string strTotalSubmitted = " select Count(1) from TBL_SUB_CHKLIST " +
        //            //                            " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
        //            //                            " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //            //                            " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
        //            //                            " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //            //                            " where SM_STM_ID =  " + strDeptId + " and SUB_STATUS in ('S','R','SR') " + strFilterQuery;

        //            string strQDueandNotSubmitted = " select Count(1) from TBL_SUB_CHKLIST " +
        //                                        " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
        //                                        " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                                        " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
        //                                        " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //                                        " where SM_STM_ID = " + strDeptId +
        //                                        " and SC_DUE_DATE_TO <= current_timestamp and (SUB_STATUS is null or SUB_STATUS = '')" +
        //                                        strFilterQuery;

        //            string strNotYetDue = " select Count(1) from TBL_SUB_CHKLIST " +
        //                                       " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
        //                                       " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                                       " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
        //                                       " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //                                       " where SM_STM_ID = " + strDeptId + " and SC_DUE_DATE_TO > current_timestamp" + strFilterQuery;

        //            string strQTotal = " select Count(*) from TBL_SUB_CHKLIST " +
        //                                " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
        //                                " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                                " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
        //                                " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //                                " where SM_STM_ID = " + strDeptId + " " + strFilterQuery + " and isnull(SUB_STATUS,'') != 'R' ";

        //            #region CommentedCode
        //            //if (dtCount.Rows.Count > 0)
        //            //{
        //            //    for (int j = 0; j < dtCount.Rows.Count; j++)
        //            //    {
        //            //        dr1 = dtCount.Rows[j];
        //            //        if (dr1["SUB_YES_NO_NA"].Equals("Y"))
        //            //        {
        //            //            intCompliedCount = Convert.ToInt32(dr1["TotCount"].ToString());
        //            //            intCompliedTotalCount = intCompliedTotalCount + intCompliedCount;
        //            //        }
        //            //        else if (dr1["SUB_YES_NO_NA"].Equals("N"))
        //            //        {
        //            //            intNonCompliedCount = Convert.ToInt32(dr1["TotCount"].ToString());
        //            //            intNonCompliedTotalCount = intNonCompliedTotalCount + intNonCompliedCount;
        //            //        }
        //            //        else if (dr1["SUB_YES_NO_NA"].Equals("NA"))
        //            //        {
        //            //            intNACount = Convert.ToInt32(dr1["TotCount"].ToString());
        //            //            intNATotalCount = intNATotalCount + intNACount;
        //            //        }
        //            //        //else
        //            //        //{
        //            //        //    intNotSubmitted = Convert.ToInt32(dr1["TotCount"].ToString());
        //            //        //    intNotSubmittedTotal = intNotSubmittedTotal + intNotSubmitted;
        //            //        //}

        //            //        //<<Added by Kiran Kharat On 20Mar2018
        //            //        //intTotalCount = intCompliedCount + intNonCompliedCount + intNACount + intNotSubmitted;
        //            //    }
        //            #endregion
        //            intCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strCompliedCount));
        //            intNonCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strNonCompliedCount));
        //            intNACount = Convert.ToInt32(DataServer.ExecuteScalar(strNACount));
        //            //intTotalSubmitted = Convert.ToInt32(DataServer.ExecuteScalar(strTotalSubmitted));
        //            intNotYetDue = Convert.ToInt32(DataServer.ExecuteScalar(strNotYetDue));
        //            intDueandNotSubmitted = Convert.ToInt32(DataServer.ExecuteScalar(strQDueandNotSubmitted));
        //            intTotalCount = Convert.ToInt32(DataServer.ExecuteScalar(strQTotal));

        //            intOverAllCount = intOverAllCount + intTotalCount;
        //            //>>
        //            strChart += "{x: '" + strDeptName + "', Y: " + intCompliedCount + " , N: " + intNonCompliedCount + " ," +
        //                        " NA: " + intNACount + " ,  ND: " + intNotYetDue + ", DN: " + intDueandNotSubmitted + "},";

        //            strHtmlTable += "<tr><td class= 'DBTableCellLeft'><center>" + strDeptName + "</center></td>";
        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                            "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
        //                            "'DetailedReport.aspx?ReportType=1&Status=Y&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
        //                            "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                            intCompliedCount +
        //                            "</a>" +
        //                            "</td>";

        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                            "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
        //                            "'DetailedReport.aspx?ReportType=1&Status=N&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
        //                            "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                            intNonCompliedCount +
        //                            "</a>" +
        //                            "</td>";

        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                            "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
        //                            "'DetailedReport.aspx?ReportType=1&Status=NA&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
        //                            "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                            +intNACount +
        //                            "</a>" +
        //                            "</td>";

        //            //strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //            //               "<a href='#'  onclick=\"window.open(" +
        //            //               "'DetailedReport.aspx?ReportType=1&Status=S&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
        //            //               "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //            //               "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //            //               intTotalSubmitted +
        //            //               "</a>" +
        //            //               "</td>";

        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                          "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
        //                          "'DetailedReport.aspx?ReportType=1&Status=DN&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
        //                          "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                          "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                          intDueandNotSubmitted +
        //                          "</a>" +
        //                          "</td>";

        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                           "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
        //                           "'DetailedReport.aspx?ReportType=1&Status=ND&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
        //                           "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                           "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                           intNotYetDue +
        //                           "</a>" +
        //                           "</td>";

        //            //strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //            //                "<a href='#'  onclick=\"window.open(" +
        //            //                "'DetailedReport.aspx?ReportType=1&Status=&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
        //            //                "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //            //                "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //            //                +intNotSubmitted +
        //            //                "</a>" +
        //            //                "</td>";

        //            //<<Added by Kiran Kharat On 20Mar2018
        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                           "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
        //                           "'DetailedReport.aspx?ReportType=1&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
        //                           "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                           "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                           +intTotalCount +
        //                           "</a>" +
        //                           "</td></tr>";
        //            //>>
        //            //}

        //            #region CommentedCode
        //            //    strQuery2 = "   select  count(1)as NonCompliedCount from TBL_SUB_TYPE_MAS " +
        //            //                "  left outer join TBL_SUBMISSIONS_MAS on  SM_STM_ID = STM_ID " +
        //            //                "  inner  join TBL_SUB_CHKLIST on SC_SM_ID = SM_ID  and STM_ID = " + strDeptId + " " +
        //            //                "  inner  join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] and SUB_YES_NO_NA = 'N'  ";

        //            //    DataTable dtNonCompliedCount = new DataTable();
        //            //    SqlCommand cmd1 = new SqlCommand(strQuery2, myconnection);
        //            //    SqlDataAdapter searchDataAdaptor1 = new SqlDataAdapter(cmd1);
        //            //    searchDataAdaptor1.Fill(dtNonCompliedCount);

        //            //    if (dtNonCompliedCount.Rows.Count > 0)
        //            //    {
        //            //        for (int k = 0; k < dtNonCompliedCount.Rows.Count; k++)
        //            //        {
        //            //            dr2 = dtNonCompliedCount.Rows[k];
        //            //            strNonCompliedCount = strNonCompliedCount + dr2["NonCompliedCount"].ToString() + ",";
        //            //        }
        //            //    }
        //            //}

        //            //strQuery3 = "   select  STM_TYPE,SUB_YES_NO_NA,count(1) as NotSubmitted,STM_ID from  TBL_SUB_CHKLIST " +
        //            //            "   inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID " +
        //            //            "   inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
        //            //            "   left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] where SUB_YES_NO_NA is NULL" +
        //            //            "   group by STM_TYPE,STM_ID,SUB_YES_NO_NA order by STM_TYPE ASC ";

        //            //DataTable dtNotSubmittedCount = new DataTable();
        //            //SqlCommand cmd2 = new SqlCommand(strQuery3, myconnection);
        //            //SqlDataAdapter searchDataAdaptor2 = new SqlDataAdapter(cmd2);
        //            //searchDataAdaptor2.Fill(dtNotSubmittedCount);

        //            //if (dtNotSubmittedCount.Rows.Count > 0)
        //            //{
        //            //    for (int l = 0; l < dtNotSubmittedCount.Rows.Count; l++)
        //            //    {
        //            //        dr2 = dtNotSubmittedCount.Rows[l];

        //            //        strNotSubmitted = strNotSubmitted + dr2["NotSubmitted"].ToString() + ",";
        //            //    }

        //            //}
        //            #endregion

        //        }

        //        #region commented code of total
        //        // //<<Added by Kiran Kharat On 20Mar2018
        //        // strHtmlTable += "<tr><td class= 'DBTableCellLeft'><center>Total</center></td>";
        //        // strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //        //                        "<a href='#'  onclick=\"window.open(" +
        //        //                        "'DetailedReport.aspx?ReportType=1&Status=Y&FromDate=" + strFromDate + "&ToDate=" + strToDate  +
        //        //                        "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //        //                        "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //        //                        +intCompliedTotalCount +
        //        //                        "</a>" +
        //        //                        "</td>";

        //        // strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //        //                         "<a href='#'  onclick=\"window.open(" +
        //        //                         "'DetailedReport.aspx?ReportType=1&Status=N&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
        //        //                         "&SRDID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //        //                         "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //        //                         +intNonCompliedTotalCount +
        //        //                         "</a>" +
        //        //                         "</td>";

        //        // strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //        //                         "<a href='#'  onclick=\"window.open(" +
        //        //                         "'DetailedReport.aspx?ReportType=1&Status=NA&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
        //        //                         "&SRDID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //        //                         "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //        //                         +intNATotalCount +
        //        //                         "</a>" +
        //        //                         "</td>";

        //        // strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //        //                         "<a href='#'  onclick=\"window.open(" +
        //        //                         "'DetailedReport.aspx?ReportType=1&Status=&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
        //        //                         "&SRDID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //        //                         "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //        //                         +intNotSubmittedTotal +
        //        //                         "</a>" +
        //        //                         "</td>";

        //        // strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //        //                          "<a href='#'  onclick=\"window.open(" +
        //        //                          "'DetailedReport.aspx?ReportType=1&FromDate=" + strFromDate + "&ToDate=" + strToDate  +
        //        //                          "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //        //                          "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //        //                          +intOverAllCount +
        //        //                          "</a>" +
        //        //                          "</td>";
        //        ////>>
        //        #endregion
        //        strHtmlTable += "</table>";
        //        litDeptWiseBarchart.Text = strHtmlTable;
        //        strChart = strChart.Substring(0, strChart.Length - 1);
        //        strChart += " ], xkey: 'x', ykeys: ['Y','N','NA','ND','DN'], " +
        //                    " labels: ['Complied','Not Complied','Not Applicable','Not Yet Due','Due but not Submitted']," +
        //                    " barColors: ['#3e95cd','#8e5ea2','#267DD4','#65C3A8','#8F002C'], xLabelAngle: 0, resize: true});";
        //        strChart += "</script>";
        //        litChart.Text = strChart;
        //    }
        //}
        //private void getComplianceWiseStatus()
        //{
        //    string strtotalRecords = "", strHtmlTable = "";
        //    string strTrackedBy = "", strReportingFunction = "", strFrequency = ""
        //            , strFromDate = "", strToDate = "", strPriority = "";
        //    string strFilter = "";
        //    string strCompliantPercent = "", strNonCompliantPercent = "", strNotApplicable = "", strNotSubmitted = "", strDueandNotSubmitted = "", strNotYetDue = "";
        //    string strQCompliantPercent = "", strQNonCompliantPercent = "", strQNotYetDue = "", strQDueandNotSubmitted = "",
        //           strQNotApplicable = "", strQNotSubmitted = "", strQTotal = "", strTotal = "";
        //    string strScript = "";
        //    string strQueryTotal = "SELECT count(1) FROM TBL_SUB_CHKLIST " +
        //                "INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
        //                "LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where 1=1";

        //    DataTable dtValueInPercentage = new DataTable();
        //    if (ddlSubType.SelectedValue != "")
        //    {
        //        strTrackedBy = ddlSubType.SelectedValue;
        //        strFilter += " and SM_STM_ID = " + strTrackedBy + " ";
        //    }
        //    if (ddlReportDept.SelectedValue != "")
        //    {
        //        strReportingFunction = ddlReportDept.SelectedValue;
        //        strFilter += " and SM_SRD_ID = " + strReportingFunction + " ";
        //    }
        //    if (ddlFrequency.SelectedValue != "")
        //    {
        //        strFrequency = ddlFrequency.SelectedValue;
        //        strFilter += " and SM_FREQUENCY = '" + strFrequency + "' ";
        //    }
        //    if (txtFromdate.Text != "")
        //    {
        //        strFromDate = txtFromdate.Text;
        //        strFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
        //    }
        //    if (txtTodate.Text != "")
        //    {
        //        strToDate = txtTodate.Text;
        //        strFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
        //    }
        //    if (ddlPriority.SelectedValue != "")
        //    {
        //        strPriority = ddlPriority.SelectedValue;
        //        strFilter += " and SC_PRIORITY = '" + strPriority + "' ";
        //    }

        //    try
        //    {
        //        strtotalRecords = DataServer.ExecuteScalar(strQueryTotal + strFilter).ToString();

        //        if (strtotalRecords != "0")
        //        {
        //            strQCompliantPercent = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //                    " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
        //                    " INNER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID AND " +
        //                    " DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE),SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y' " +
        //                    " AND SUB_STATUS in ('S','C') ";

        //            strQNonCompliantPercent = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //                    " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
        //                    " INNER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where SUB_STATUS in ('S','C')" +
        //                    " AND ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') " +
        //                    " AND SUB_STATUS in ('S','C') ";

        //            strQNotApplicable = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //                    "FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
        //                    "INNER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where SUB_YES_NO_NA = 'NA' AND SUB_STATUS in ('S','C')";

        //            strQNotSubmitted = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //                    "FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
        //                    "LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where (SUB_STATUS is null or SUB_STATUS = '')";

        //            //strQTotalSubmitted = " select  cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //            //        " from TBL_SUB_CHKLIST " +
        //            //        " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //            //        " INNER join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //            //        " AND SUB_STATUS in ('S','R','SR')";

        //            strQNotYetDue = " select  cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //                    " from TBL_SUB_CHKLIST " +
        //                    " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //                    " where SC_DUE_DATE_TO >= current_timestamp";

        //            strQDueandNotSubmitted = " select  cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //                    " from TBL_SUB_CHKLIST " +
        //                    " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //                    " where SC_DUE_DATE_TO <= current_timestamp and (SUB_STATUS is null or SUB_STATUS = '')";

        //            strQTotal = " select  cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //                    " from TBL_SUB_CHKLIST " +
        //                    " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //                    " where 1=1 and isnull(SUB_STATUS,'') != 'R' ";

        //            strCompliantPercent = DataServer.ExecuteScalar(strQCompliantPercent + strFilter).ToString();
        //            strNonCompliantPercent = DataServer.ExecuteScalar(strQNonCompliantPercent + strFilter).ToString();
        //            strNotApplicable = DataServer.ExecuteScalar(strQNotApplicable + strFilter).ToString();
        //            strNotSubmitted = DataServer.ExecuteScalar(strQNotSubmitted + strFilter).ToString();
        //            //strSubmitted = DataServer.ExecuteScalar(strQTotalSubmitted + strFilter).ToString();
        //            strNotYetDue = DataServer.ExecuteScalar(strQNotYetDue + strFilter).ToString();
        //            strDueandNotSubmitted = DataServer.ExecuteScalar(strQDueandNotSubmitted + strFilter).ToString();
        //            //<<Added by Kiran Kharat On 20Mar2018
        //            //////intTotalCount = Convert.ToInt32(strCompliantPercent.Split('|')[1]) + Convert.ToInt32(strNonCompliantPercent.Split('|')[1]) + Convert.ToInt32(strNotApplicable.Split('|')[1]) + Convert.ToInt32(strNotSubmitted.Split('|')[1]);
        //            //>>

        //            Double CompliantandNotApplicablePercent = Convert.ToDouble(strCompliantPercent.Split('|')[0]) + Convert.ToDouble(strNotApplicable.Split('|')[0]);
        //            strTotal = DataServer.ExecuteScalar(strQTotal + strFilter).ToString();
        //            strScript = "<script type=\"text/javascript\">Morris.Donut({ element: 'donut-chart-color',data: [";
        //            strScript += "{value: " + CompliantandNotApplicablePercent + ", label: 'Compliant/Not Applicable'},";
        //            strScript += "{value: " + strNonCompliantPercent.Split('|')[0] + ", label: 'Not Compliant'},";
        //            //strScript += "{value: " + strNotApplicable.Split('|')[0] + ", label: 'Not Applicable'},";
        //            //strScript += "{value: " + strNotSubmitted.Split('|')[0] + ", label: 'Not Yet Submitted'},";
        //            //strScript += "{value: " + strSubmitted.Split('|')[0] + ", label: 'Submitted but not closed'},";
        //            strScript += "{value: " + strNotYetDue.Split('|')[0] + ", label: 'Not Yet Due'},";
        //            strScript += "{value: " + strDueandNotSubmitted.Split('|')[0] + ", label: 'Due but not Submitted'},";
        //            strScript += "], backgroundColor: '#F4F4F4', labelColor: '#000',";
        //            strScript += "resize: true,colors: ['#009933','#ffcc00','#65C3A8','#8F002C'],";
        //            strScript += "formatter: function (x) { return x + \" % \"}});";
        //            strScript += Environment.NewLine + Environment.NewLine;

        //            int CompliantandNotApplicableCount = Convert.ToInt32(strCompliantPercent.Split('|')[1]) + Convert.ToInt32(strNotApplicable.Split('|')[1]);
        //            strScript += "Morris.Bar({element: 'bar-color-chart',data: [";
        //            strScript += "{x: 'Compliant/Not Applicable', y: " + CompliantandNotApplicableCount + "},";
        //            strScript += "{x: 'Not Compliant', y: " + strNonCompliantPercent.Split('|')[1] + "},";
        //            //strScript += "{x: 'Not Applicable', y: " + strNotApplicable.Split('|')[1] + "},";
        //            //strScript += "{x: 'Submitted but not closed', y: " + strSubmitted.Split('|')[1] + "},";
        //            strScript += "{x: 'Not Yet Due', y: " + strNotYetDue.Split('|')[1] + "},";
        //            strScript += "{x: 'Due but not Submitted', y: " + strDueandNotSubmitted.Split('|')[1] + "},";
        //            //strScript += "{x: 'Not Yet Submitted', y: " + strNotSubmitted.Split('|')[1] + "},";
        //            strScript += "],xkey: 'x',ykeys: ['y'],labels: ['Y'],resize: true,xLabelAngle: 60,barColors: ";
        //            strScript += "function (row, series, type) {if (type === 'bar') {if(row.label == 'Compliant/Not Applicable'){return '#009933'}";
        //            strScript += "if(row.label == 'Not Compliant'){return '#ffcc00'}";//if(row.label == 'Not Applicable'){return '#0033cc'}";
        //            strScript += //"if(row.label == 'Not Yet Submitted'){return '#ff0000'}"+
        //                         //"if(row.label == 'Submitted but not closed'){return '#54BB72'}" +
        //                "if(row.label == 'Not Yet Due'){return '#65C3A8'}" +
        //                "if(row.label == 'Due but not Submitted'){return '#8F002C'}" +
        //                          "}else {return '#000';}}";
        //            strScript += "});";
        //            strScript += "</script>";
        //            litChart.Text = strScript;

        //            strHtmlTable = "<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'>";
        //            strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Compliant</td>";
        //            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Compliant</td>";
        //            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Applicable</td>";
        //            //strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted but not Closed</td>";
        //            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Due but not Submitted</td>";
        //            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Yet Due</td>";
        //            //strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Yet Submitted</td>";
        //            //<<Added by Kiran Kharat on 20Mar2018 
        //            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Total</td></tr>";
        //            //>>

        //            strHtmlTable += "<tr><td class= 'DBTableCellLeft'>" +
        //                            "<center><a href='#'  class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
        //                            "'DetailedReport.aspx?ReportType=2&Status=Y&FromDate=" + strFromDate + "&ToDate=" + strToDate +
        //                            "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                            strCompliantPercent.Split('|')[1] +
        //                            "</a>" +
        //                            "</center></td>";

        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                            "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
        //                            "'DetailedReport.aspx?ReportType=2&Status=N&FromDate=" + strFromDate + "&ToDate=" + strToDate +
        //                            "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                            strNonCompliantPercent.Split('|')[1] +
        //                            "</a>" +
        //                            "</td>";

        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                            "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
        //                            "'DetailedReport.aspx?ReportType=2&Status=NA&FromDate=" + strFromDate + "&ToDate=" + strToDate +
        //                            "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                            strNotApplicable.Split('|')[1] +
        //                            "</a>" +
        //                            "</td>";

        //            //strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //            //               "<a href='#'  onclick=\"window.open(" +
        //            //               "'DetailedReport.aspx?ReportType=2&Status=S&FromDate=" + strFromDate + "&ToDate=" + strToDate +
        //            //               "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //            //               "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //            //              strSubmitted.Split('|')[1] +
        //            //               "</a>" +
        //            //               "</td>";

        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                           "<a href='#'  class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
        //                           "'DetailedReport.aspx?ReportType=2&Status=DN&FromDate=" + strFromDate + "&ToDate=" + strToDate +
        //                           "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                           "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                            strDueandNotSubmitted.Split('|')[1] +
        //                           "</a>" +
        //                           "</td>";

        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                          "<a href='#'  class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
        //                          "'DetailedReport.aspx?ReportType=2&Status=ND&FromDate=" + strFromDate + "&ToDate=" + strToDate +
        //                          "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                          "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                           strNotYetDue.Split('|')[1] +
        //                          "</a>" +
        //                          "</td>";

        //            //strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //            //                "<a href='#'  onclick=\"window.open(" +
        //            //                "'DetailedReport.aspx?ReportType=2&Status=&FromDate=" + strFromDate + "&ToDate=" + strToDate +
        //            //                "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //            //                "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //            //                strNotSubmitted.Split('|')[1] +
        //            //                "</a>" +
        //            //                "</td>";

        //            //<<Added by Kiran Kharat On 20Mar2018
        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                           "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
        //                           "'DetailedReport.aspx?ReportType=2&FromDate=" + strFromDate + "&ToDate=" + strToDate +
        //                           "&SRDID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                           "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                           strTotal.Split('|')[1] +
        //                           "</a>" +
        //                           "</td></tr>";
        //            //>>
        //            strHtmlTable += "</table><br/><br/>";
        //            litComplianceStatus.Text = strHtmlTable;
        //            pnlHideShow("pnlComplianceStatus");
        //        }
        //        else
        //        {
        //            pnlComplianceStatus.Visible = false;
        //            writeError("No data found.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        writeError("system Exception in getComplianceWiseStatus(): " + ex);
        //    }

        //}
        //private void getMonthlyComplianceReport()
        //{
        //    DataServer objds = new DataServer();
        //    string strTrackedBy = "", strReportingFunction = "", strFrequency = ""
        //            , strFromDate = "", strToDate = "", strPriority = "", strFilter = "", strtotalRecords = "", strToDateFilter = "", strFromDateFilter = "";
        //    string strCompliantPercent = "", strNonCompliantPercent = "", strNotApplicable = "", strNotSubmitted = "", strDueandNotSubmitted = "", strNotYetDue = ""; ;
        //    string strQCompliantPercent = "", strQNonCompliantPercent = "", strQNotApplicable = "", strQNotSubmitted = "", strQNotYetDue = "",
        //           strQDueandNotSubmitted = "", strQTotal = "", strTotal = "";
        //    string strMonth = "", strYear = "";

        //    string strFilterIn = "";
        //    string strTotalQuery = "";
        //    string strScript = "";
        //    DataRow drTotals;
        //    pnlHideShow("pnlMonthlyCompliance");

        //    string strHtmlTable = "<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'>";
        //    strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Year</td>";
        //    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Compliant</td>";
        //    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Compliant</td>";
        //    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Applicable</td>";
        //    //strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted but not Closed</td>";

        //    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Due but not Submitted</td>";
        //    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Yet Due</td>";
        //    //strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Yet Submitted</td>";
        //    //<<Added by Kiran Kharat On 20Mar2018
        //    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Total</td></tr>";
        //    //>>

        //    if (ddlSubType.SelectedValue != "")
        //    {
        //        strTrackedBy = ddlSubType.SelectedValue;
        //        strFilter += " and SM_STM_ID = " + strTrackedBy + " ";
        //    }
        //    if (ddlReportDept.SelectedValue != "")
        //    {
        //        strReportingFunction = ddlReportDept.SelectedValue;
        //        strFilter += " and SM_SRD_ID = " + strReportingFunction + " ";
        //    }
        //    if (ddlFrequency.SelectedValue != "")
        //    {
        //        strFrequency = ddlFrequency.SelectedValue;
        //        strFilter += " and SM_FREQUENCY = '" + strFrequency + "' ";
        //    }
        //    if (txtFromdate.Text != "")
        //    {
        //        strFromDate = txtFromdate.Text;
        //        //strFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
        //        strFromDateFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
        //    }
        //    if (txtTodate.Text != "")
        //    {
        //        strToDate = txtTodate.Text;
        //        //strFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
        //        strToDateFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
        //    }
        //    if (ddlPriority.SelectedValue != "")
        //    {
        //        strPriority = ddlPriority.SelectedValue;
        //        strFilter += " and SC_PRIORITY = '" + strPriority + "' ";
        //    }

        //    //strTotalQuery = "SELECT count(1) as [Total Submissions],month(SC_DUE_DATE_TO) as [Month], year(SC_DUE_DATE_TO) as [Year]  FROM TBL_SUB_CHKLIST " +
        //    //       "INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
        //    //       "LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where 1=1 and isnull(month(SC_DUE_DATE_TO),'') != '' " + strFilter +
        //    //       " and isnull(SUB_STATUS,'') != 'R' " +
        //    //       "group by month(SC_DUE_DATE_TO), year(SC_DUE_DATE_TO)";

        //    strTotalQuery = "SELECT count(1) as [Total Submissions], month(SC_DUE_DATE_TO) as [Month], " +
        //        " year(SC_DUE_DATE_TO) as [Year]  FROM TBL_SUB_CHKLIST " +
        //       "INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
        //       "INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
        //       "LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where 1=1 " +
        //       "and isnull(month(SC_DUE_DATE_TO),'') != '' " + strFilter + strFromDateFilter + strToDateFilter +
        //       "group by month(SC_DUE_DATE_TO), year(SC_DUE_DATE_TO) " +
        //       "order by year(SC_DUE_DATE_TO), month(SC_DUE_DATE_TO) ";

        //    DataTable dtTotals = new DataTable();
        //    dtTotals = objds.Getdata(strTotalQuery);

        //    if (dtTotals.Rows.Count > 0)
        //    {
        //        strScript = "<script type=\"text/javascript\">Morris.Bar({ element: 'stacked-bars',data: [";

        //        for (int i = 0; i < dtTotals.Rows.Count; i++)
        //        {
        //            drTotals = dtTotals.Rows[i];
        //            strtotalRecords = drTotals["Total Submissions"].ToString();
        //            strMonth = drTotals["Month"].ToString();
        //            strYear = drTotals["Year"].ToString();

        //            strFilterIn = " and month(SC_DUE_DATE_TO) = " + strMonth + " and year(SC_DUE_DATE_TO) = '" + strYear + "' ";
        //            string strFilterIn1 = " and month(SC_DUE_DATE_TO) = " + strMonth + " ";

        //            strQCompliantPercent = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //                    " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
        //                    " LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO " +
        //                    " and SUB_YES_NO_NA = 'Y' AND SUB_STATUS in ('S','C') ";

        //            strQNonCompliantPercent = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //                    " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
        //                    " LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where SUB_STATUS in ('S','C') " +
        //                    " and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') " +
        //                    " AND SUB_STATUS in ('S','C') ";

        //            strQNotApplicable = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //                    "FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
        //                    "LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where SUB_YES_NO_NA = 'NA' AND SUB_STATUS in ('S','C')";

        //            strQNotSubmitted = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //                    "FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
        //                    "LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where (SUB_STATUS is null or SUB_STATUS = '')";
        //            //
        //            //strQTotalSubmitted = " select  cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //            //       " from TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
        //            //       " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //            //       " where SUB_STATUS in ('S','R','SR')";

        //            strQNotYetDue = " select  cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //                    " from TBL_SUB_CHKLIST " +
        //                    " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //                    " where SC_DUE_DATE_TO >= current_timestamp";

        //            strQDueandNotSubmitted = " select  cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //                   " from TBL_SUB_CHKLIST " +
        //                   " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                   " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //                   " where SC_DUE_DATE_TO <= current_timestamp and (SUB_STATUS is null or SUB_STATUS = '')";

        //            strQTotal = " select  cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
        //                    " from TBL_SUB_CHKLIST " +
        //                    " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //                    " where 1=1 and isnull(SUB_STATUS,'') != 'R' ";


        //            strCompliantPercent = DataServer.ExecuteScalar(strQCompliantPercent + strFilter + strFilterIn).ToString();
        //            strNonCompliantPercent = DataServer.ExecuteScalar(strQNonCompliantPercent + strFilter + strFilterIn).ToString();
        //            strNotApplicable = DataServer.ExecuteScalar(strQNotApplicable + strFilter + strFilterIn).ToString();
        //            strNotSubmitted = DataServer.ExecuteScalar(strQNotSubmitted + strFilter + strFilterIn).ToString();
        //            //strSubmitted = DataServer.ExecuteScalar(strQTotalSubmitted + strFilter + strFilterIn).ToString();
        //            strNotYetDue = DataServer.ExecuteScalar(strQNotYetDue + strFilter + strFilterIn).ToString();
        //            strDueandNotSubmitted = DataServer.ExecuteScalar(strQDueandNotSubmitted + strFilter + strFilterIn).ToString();
        //            strTotal = DataServer.ExecuteScalar(strQTotal + strFilter + strFilterIn).ToString();
        //            //<<Added by Kiran Kharat On 20Mar2018
        //            //intTotalCount = Convert.ToInt32(strCompliantPercent.Split('|')[1]) + Convert.ToInt32(strNonCompliantPercent.Split('|')[1]) + Convert.ToInt32(strNotApplicable.Split('|')[1]) + Convert.ToInt32(strNotSubmitted.Split('|')[1]);
        //            //>>

        //            double dblCompliantNotApplicablePercent = Convert.ToDouble(strCompliantPercent.Split('|')[0]) + Convert.ToDouble(strNotApplicable.Split('|')[0]);
        //            strScript += "{x: '" + getMonthName(strMonth) + '-' + strYear + "',";
        //            strScript += " a: " + strCompliantPercent.Split('|')[0] + ",";
        //            strScript += " b: " + strNonCompliantPercent.Split('|')[0] + ",";
        //            //strScript += " c: " + strNotApplicable.Split('|')[0] + ",";
        //            //strScript += " d: " + strNotSubmitted.Split('|')[0] + ",";
        //            //strScript += " e: " + strSubmitted.Split('|')[0] + ",";
        //            strScript += " g: " + strDueandNotSubmitted.Split('|')[0] + ",";
        //            strScript += " f: " + strNotYetDue.Split('|')[0] + "},";

        //            strHtmlTable += "<tr><td class= 'DBTableCellLeft'><center><b>" + getMonthName(strMonth) + '-' + strYear + "</b></center></td>";
        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                            "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
        //                            "'DetailedReport.aspx?ReportType=3&Status=Y&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
        //                            "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                            strCompliantPercent.Split('|')[1] +
        //                            "</a></td>";
        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                            "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
        //                            "'DetailedReport.aspx?ReportType=3&Status=N&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
        //                            "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                            strNonCompliantPercent.Split('|')[1] +
        //                            "</a></td>";
        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                            "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
        //                            "'DetailedReport.aspx?ReportType=3&Status=NA&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
        //                            "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                            strNotApplicable.Split('|')[1] +
        //                            "</a></td>";
        //            //strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //            //              "<a href='#'  onclick=\"window.open(" +
        //            //              "'DetailedReport.aspx?ReportType=3&Status=S&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
        //            //              "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //            //              "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //            //              strSubmitted.Split('|')[1] +
        //            //              "</a></td>";
        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                        "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
        //                        "'DetailedReport.aspx?ReportType=3&Status=DN&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
        //                        "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                        "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                        strDueandNotSubmitted.Split('|')[1] +
        //                        "</a></td>";

        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                          "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
        //                          "'DetailedReport.aspx?ReportType=3&Status=ND&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
        //                          "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                          "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                          strNotYetDue.Split('|')[1] +
        //                          "</a></td>";

        //            //strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //            //                "<a href='#'  onclick=\"window.open(" +
        //            //                "'DetailedReport.aspx?ReportType=3&Status=&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
        //            //                "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //            //                "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //            //                strNotSubmitted.Split('|')[1] +
        //            //                "</a></td>";
        //            //<<Added by Kiran Kharat On 20Mar2018
        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                           "<a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
        //                           "'DetailedReport.aspx?ReportType=3&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
        //                           "&SRDID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
        //                           "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                           strTotal.Split('|')[1] +
        //                           "</a>" +
        //                           "</td></tr>";
        //            //>>
        //        }
        //        strHtmlTable += "</table>";
        //        strScript = strScript.Substring(0, strScript.Length - 1);
        //        strScript += "],xkey: 'x',ykeys: ['a', 'b','f','g'],labels: ['Compliant/Not Applicable', 'Non-Compliant','Not Yet Due','Due but not Submitted'],";
        //        strScript += "barColors: ['#009933','#ffcc00','#65C3A8','#8F002C'], xLabelAngle: 60, stacked: true, resize: true});</script>";
        //        litChart.Text = strScript;
        //        litMonthlyReport.Text = strHtmlTable;
        //    }
        //    else
        //    {
        //        pnlMonthlyCompliance.Visible = false;
        //        writeError("No data found.");
        //    }
        //}
        //public void getRegulatoryDashboardforPrevMonth()
        //{
        //    DataServer dataServer = new DataServer();
        //    DataTable dtDistinctTrackReptDept;
        //    string strQuery = "", strContent = "", strHostingServer = "", strTrackingDeptId = "", strReportingDeptId = "",
        //           strTrackDeptName = "", strReptDeptName = "", strHTML = "", strFilter = "", strTrackedBy = "", strReportingFunction = "",
        //           strFrequency = "", strPriority = "";

        //    DataRow drDistinctTrackReptDept;
        //    int intTotalTasks = 0, intClosedBeforeDuedate = 0, intSubmittedBeforeDuedate = 0, intNotClosed = 0, intSubmittedAfterDuedate = 0, intNotSubmitted = 0;

        //    strHostingServer = ConfigurationManager.AppSettings["HostingServer"].ToString();
        //    SqlConnection myconnection = new SqlConnection(mstrConnectionString);

        //    string strMonth = DateTime.Now.AddMonths(-1).ToString("MM");
        //    string strCurrMonth = DateTime.Now.AddMonths(-1).ToString("MMMM yyyy");

        //    pnlHideShow("pnlRegulatoryreportingPrevmonth");
        //    lblCurrMnth.Text = strCurrMonth;


        //    if (ddlSubType.SelectedValue != "")
        //    {
        //        strTrackedBy = ddlSubType.SelectedValue;
        //        strFilter += " and SM_STM_ID = " + strTrackedBy + " ";
        //    }
        //    if (ddlReportDept.SelectedValue != "")
        //    {
        //        strReportingFunction = ddlReportDept.SelectedValue;
        //        strFilter += " and SM_SRD_ID = " + strReportingFunction + " ";
        //    }
        //    if (ddlFrequency.SelectedValue != "")
        //    {
        //        strFrequency = ddlFrequency.SelectedValue;
        //        strFilter += " and SM_FREQUENCY = '" + strFrequency + "' ";
        //    }
        //    if (ddlPriority.SelectedValue != "")
        //    {
        //        strPriority = ddlPriority.SelectedValue;
        //        strFilter += " and SC_PRIORITY = '" + strPriority + "' ";
        //    }
        //    try
        //    {
        //        strQuery = " SELECT Distinct SM_SRD_ID, SRD_NAME, SC_STM_ID, STM_TYPE  " +
        //                   " FROM TBL_SUB_CHKLIST " +
        //                   " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Apr-2023' " +
        //                   " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                   " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " +
        //                    " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " + strFilter +
        //                   " order by STM_TYPE,SRD_NAME ASC ";

        //        //<< Modified by Amarjeet on 15-Jul-2021
        //        //dtDistinctTrackReptDept = new DataTable();
        //        //cmdDistinctTrackReptDept = new SqlCommand(strQuery, myconnection);
        //        //sdaDistinctTrackReptDept = new SqlDataAdapter(cmdDistinctTrackReptDept);
        //        //sdaDistinctTrackReptDept.Fill(dtDistinctTrackReptDept);
        //        dtDistinctTrackReptDept = dataServer.Getdata(strQuery);
        //        //>>

        //        if (dtDistinctTrackReptDept.Rows.Count > 0)
        //        {

        //            strHTML += "<table class='table table-bordered footable' id=\"tfhover\"  style=\"font-size: 12px; color: #333333; " +
        //                            "width: 100%; border-width: 1px; border-color: #bcaf91; border-collapse: collapse;\">" +
        //                            "<tr style=\"background-color: #ffffff;\">" +
        //                            "<th>" +
        //                            "Sr. No.</th>" +

        //                            "<th>" +
        //                            "Tracking Department</th>" +

        //                            "<th>" +
        //                            "Reporting Department</th>" +

        //                            "<th>" +
        //                            "Total Tasks For " + strCurrMonth + "</th>" +

        //                            "<th>" +
        //                            "Submitted and Closed within System Due Date</th>" +

        //                            "<th>" +
        //                            "Submitted before System Due date, but Closed after System Due Date</th>" +

        //                            "<th>" +
        //                            "Submitted and Closed after System Due Date</th>" +

        //                            "<th>" +
        //                            "Submitted but not yet Closed</th>" +

        //                            "<th>" +
        //                            "Not Yet Submitted</th></tr>";

        //            for (int i = 0; i < dtDistinctTrackReptDept.Rows.Count; i++)
        //            {
        //                drDistinctTrackReptDept = dtDistinctTrackReptDept.Rows[i];
        //                strTrackingDeptId = drDistinctTrackReptDept["SC_STM_ID"].ToString();
        //                strReportingDeptId = drDistinctTrackReptDept["SM_SRD_ID"].ToString();
        //                strTrackDeptName = drDistinctTrackReptDept["STM_TYPE"].ToString();
        //                strReptDeptName = drDistinctTrackReptDept["SRD_NAME"].ToString();

        //                string strTotalCountQuery = " select count(*) from TBL_SUB_CHKLIST " +
        //                                            " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Apr-2023' " +
        //                                            " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                                            " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
        //                                            " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID  " +
        //                                            " where  month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE()))" +
        //                                            " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
        //                                            " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
        //                                            " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId;

        //                string strClosedBeforeDueDate = " select count(*) from TBL_SUB_CHKLIST " +
        //                                                " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Apr-2023' " +
        //                                                " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                                                " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
        //                                                " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //                                                " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " +
        //                                                " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
        //                                                " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
        //                                                " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
        //                                                " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
        //                                                " and SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) <= SC_DUE_DATE_TO";

        //                string strSubmittedBeforeDueDate = " select count(*) from TBL_SUB_CHKLIST " +
        //                                               " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Apr-2023' " +
        //                                               " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                                               " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
        //                                               " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //                                               " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID  " +
        //                                               " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
        //                                               " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
        //                                               " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
        //                                               " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
        //                                               " and SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_SUBMIT_DATE)) <= SC_DUE_DATE_TO and " +
        //                                               "DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) > SC_DUE_DATE_TO";

        //                string strSubmittedAfterDueDate = " select count(*) from TBL_SUB_CHKLIST " +
        //                                               " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Apr-2023' " +
        //                                               " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                                               " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
        //                                               " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //                                               " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID   " +
        //                                               " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
        //                                               " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
        //                                               " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
        //                                               " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
        //                                               " and SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_SUBMIT_DATE)) > SC_DUE_DATE_TO and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) > SC_DUE_DATE_TO";

        //                string strNotClosed = " select count(*) from TBL_SUB_CHKLIST " +
        //                                      " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Apr-2023' " +
        //                                      " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                                      " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
        //                                      " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //                                      " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " +
        //                                      " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
        //                                      " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
        //                                      " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
        //                                      " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
        //                                      " and SUB_STATUS = 'S' ";

        //                string strNotSubmitted = " select count(*) from TBL_SUB_CHKLIST " +
        //                                      " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Apr-2023' " +
        //                                      " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                                      " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
        //                                      " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " +
        //                                      " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
        //                                      " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
        //                                      " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
        //                                      " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
        //                                      " and not exists (SELECT SUB_ID FROM TBL_SUBMISSIONS " +
        //                                      " WHERE SUB_SC_ID = SC_ID AND SUB_STATUS in('S','C')) ";




        //                intTotalTasks = Convert.ToInt32(DataServer.ExecuteScalar(strTotalCountQuery));
        //                intClosedBeforeDuedate = Convert.ToInt32(DataServer.ExecuteScalar(strClosedBeforeDueDate));
        //                intSubmittedBeforeDuedate = Convert.ToInt32(DataServer.ExecuteScalar(strSubmittedBeforeDueDate));
        //                intSubmittedAfterDuedate = Convert.ToInt32(DataServer.ExecuteScalar(strSubmittedAfterDueDate));
        //                intNotClosed = Convert.ToInt32(DataServer.ExecuteScalar(strNotClosed));
        //                intNotSubmitted = Convert.ToInt32(DataServer.ExecuteScalar(strNotSubmitted));

        //                strHTML += "<tr style=\"background-color: #ffffff;\">" +
        //                            "<td style=\"font-size: 12px;width:2.5%;  border-width: 1px; padding: 8px; border-style: solid;" +
        //                            "border-color: #bcaf91;\">" + (i + 1) + "</td>" +

        //                            "<td style=\"font-size: 12px;width:17.5%;  border-width: 1px; padding: 8px; border-style: solid;" +
        //                            "border-color: #bcaf91;\">" + strTrackDeptName + "</td>" +

        //                            "<td style=\"font-size: 12px;width:17.5%;  border-width: 1px; padding: 8px; border-style: solid;" +
        //                            "border-color: #bcaf91;\">" + strReptDeptName + "</td>" +

        //                            "<td style=\"font-size: 12px;width:10%;  border-width: 1px; padding: 8px; border-style: solid;" +
        //                            "border-color: #bcaf91;\">" +
        //                            "<a target='_blank' href='DetailedReport.aspx?ReportType=4&SRDID=" + strReportingDeptId + "&STMID=" + strTrackingDeptId +
        //                            "&Type=1'>" +
        //                            +intTotalTasks +
        //                            "</a>" +
        //                            "</td>" +

        //                            "<td style=\"font-size: 12px;width:10%;  border-width: 1px; padding: 8px; border-style: solid;" +
        //                            "border-color: #bcaf91;\">" +
        //                            "<a target='_blank' href='DetailedReport.aspx?ReportType=4&SRDID=" + strReportingDeptId + "&STMID=" + strTrackingDeptId +
        //                            "&Type=2'>" +
        //                            +intClosedBeforeDuedate +
        //                            "</a>" +
        //                            "</td>" +

        //                            "<td style=\"font-size: 12px;width:10%;  border-width: 1px; padding: 8px; border-style: solid;" +
        //                            "border-color: #bcaf91;\">" +
        //                            "<a target='_blank' href='DetailedReport.aspx?ReportType=4&SRDID=" + strReportingDeptId + "&STMID=" + strTrackingDeptId +
        //                            "&Type=3'>" +
        //                            +intSubmittedBeforeDuedate +
        //                            "</a>" +
        //                            "</td>" +

        //                            "<td style=\"font-size: 12px;width:10%;  border-width: 1px; padding: 8px; border-style: solid;" +
        //                            "border-color: #bcaf91;\">" +
        //                            "<a target='_blank' href='DetailedReport.aspx?ReportType=4&SRDID=" + strReportingDeptId + "&STMID=" + strTrackingDeptId +
        //                            "&Type=4'>" +
        //                            +intSubmittedAfterDuedate + "</td>" +

        //                            "<td style=\"font-size: 12px;width:10%;  border-width: 1px; padding: 8px; border-style: solid;" +
        //                            "border-color: #bcaf91;\">" +
        //                            "<a target='_blank' href='DetailedReport.aspx?ReportType=4&SRDID=" + strReportingDeptId + "&STMID=" + strTrackingDeptId +
        //                            "&Type=5'>" +
        //                            +intNotClosed +
        //                            "</a>" +
        //                            "</td>" +

        //                            "<td style=\"font-size: 12px;width:10%;  border-width: 1px; padding: 8px; border-style: solid;" +
        //                            "border-color: #bcaf91;\">" +
        //                            "<a target='_blank' href='DetailedReport.aspx?ReportType=4&SRDID=" + strReportingDeptId + "&STMID=" + strTrackingDeptId +
        //                            "&Type=6'>" +
        //                            +intNotSubmitted +
        //                            "</a>" +
        //                            "</td></tr>";
        //            }


        //            strContent = strContent + strHTML + "</table>";


        //            litChart.Text = strContent;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        writeError("Error in getRegulatoryDashboardforPrevMonth() 2: " + ex.Message);
        //    }

        //} 
        #endregion

        //<<Added by Ankur Tyagi on 16-May-2024 for CR_2070
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

        #region New Code <<Added by Ankur Tyagi on 18Dec2023

        private void getComplianceWiseStatus()
        {
            string strtotalRecords = "", strHtmlTable = "";
            string strTrackedBy = "", strReportingFunction = "", strFrequency = ""
                    , strFromDate = "", strToDate = "", strPriority = "";
            string strFilter = "", strQueryPercentage = "";
            string strCompliantPercent = "", strNonCompliantPercent = "", strNotApplicable = "", strNotSubmitted = "", strSubmitted = "",
                   strDueandNotSubmitted = "", strNotYetDue = "", strToDateFilter = "", strFromDateFilter = "";
            int intTotalCount;
            string strQCompliantPercent = "", strQNonCompliantPercent = "", strQNotYetDue = "", strQTotalSubmitted = "", strQDueandNotSubmitted = "",
                   strQNotApplicable = "", strQNotSubmitted = "", strQTotal = "", strTotal = "", strTotalSubmitted = "",
                   strQDelayedFiling = "", strDelayedFiling = "";
            string strScript = "";
            string strQueryTotal = @"SELECT Count(1) FROM TBL_SUB_CHKLIST
                                INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID
                                INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID
                                LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID WHERE 1 = 1";

            DataTable dtValueInPercentage = new DataTable();
            if (ddlSubType.SelectedValue != "")
            {
                strTrackedBy = ddlSubType.SelectedValue;
                strFilter += " and SM_STM_ID = " + strTrackedBy + " ";
            }
            if (ddlReportDept.SelectedValue != "")
            {
                strReportingFunction = ddlReportDept.SelectedValue;
                strFilter += " and SM_SRD_ID = " + strReportingFunction + " ";
            }
            if (ddlFrequency.SelectedValue != "")
            {
                strFrequency = ddlFrequency.SelectedValue;
                strFilter += " and SM_FREQUENCY = '" + strFrequency + "' ";
            }
            if (txtFromdate.Text != "")
            {
                strFromDate = txtFromdate.Text;
                strFromDateFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
            }
            if (txtTodate.Text != "")
            {
                strToDate = txtTodate.Text;
                strToDateFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
            }
            if (ddlPriority.SelectedValue != "")
            {
                strPriority = ddlPriority.SelectedValue;
                strFilter += " and SC_PRIORITY = '" + strPriority + "' ";
            }

            try
            {
                //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                strtotalRecords = DataServer.ExecuteScalar(strQueryTotal + strFilter + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()).ToString();
                //>>

                if (strtotalRecords != "0")
                {
                    strQCompliantPercent = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                            " INNER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID WHERE SUB_STATUS in ('S','C') " +
                            " AND (CAST(ISNULL(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) AS DATE)) <= CAST(SC_DUE_DATE_TO AS DATE) " +
                            " AND SUB_YES_NO_NA = 'Y' ";

                    strQNonCompliantPercent = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID=SM_SRD_ID " +
                            " INNER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where SUB_STATUS in ('S','C')" +
                            " AND SUB_YES_NO_NA = 'N' ";

                    strQDelayedFiling = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                            " INNER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where " +
                            " CAST(ISNULL(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) AS DATE) > CAST(SC_DUE_DATE_TO AS DATE) " +
                            " AND SUB_YES_NO_NA = 'Y' AND SUB_STATUS in ('S','C') ";

                    strQNotApplicable = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                            " INNER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID " +
                            " where SUB_YES_NO_NA = 'NA' AND SUB_STATUS in ('S','C') ";

                    strQNotSubmitted = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            "FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                            "LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where (SUB_STATUS is null or SUB_STATUS = '')";

                    strQNotYetDue = " select Count(1) " +
                            " from TBL_SUB_CHKLIST " +
                            " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID = SM_SRD_ID " +
                            " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID WHERE 1 = 1";

                    //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                    if (string.IsNullOrEmpty(txtTodate.Text))
                    {
                        strQNotYetDue += " AND CAST(SC_DUE_DATE_TO AS DATE) > CAST(CURRENT_TIMESTAMP AS DATE) and isnull(SUB_STATUS, '') = '' ";
                    }
                    else
                    {
                        strQNotYetDue += " AND CAST(SC_DUE_DATE_TO AS DATE) > CAST('" + txtTodate.Text + "' AS DATE) and isnull(SUB_STATUS, '') = '' ";
                    }
                    //>>

                    strQDueandNotSubmitted = " select  cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            " from TBL_SUB_CHKLIST " +
                            " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                            " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                            " WHERE isnull(SUB_STATUS, '') = '' ";

                    //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                    if (string.IsNullOrEmpty(txtTodate.Text))
                    {
                        strQDueandNotSubmitted += " AND CAST(SC_DUE_DATE_TO AS DATE) <= CAST(CURRENT_TIMESTAMP AS DATE) ";
                    }
                    else
                    {
                        strQDueandNotSubmitted += " AND CAST(SC_DUE_DATE_TO AS DATE) <= CAST('" + txtTodate.Text + "' AS DATE) ";
                    }
                    //>>

                    string strQRopnSR = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                            " INNER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID " +
                            " WHERE ISNULL(SUB_STATUS,'') in ('R','SR') ";

                    //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                    //Header - Reopened/Sent back
                    // SUB_Status in (R, SR)
                    strQTotal = " SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            " FROM TBL_SUB_CHKLIST " +
                            " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                            " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                            //" where 1=1 and isnull(SUB_STATUS,'') != 'R' ";
                            " WHERE 1 = 1 ";

                    //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                    strCompliantPercent = DataServer.ExecuteScalar(strQCompliantPercent + strFilter + strFromDateFilter + strToDateFilter+ ReturnCutoffFilterQry()).ToString();
                    strNonCompliantPercent = DataServer.ExecuteScalar(strQNonCompliantPercent + strFilter + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()).ToString();
                    strNotApplicable = DataServer.ExecuteScalar(strQNotApplicable + strFilter + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()).ToString();
                    strNotSubmitted = DataServer.ExecuteScalar(strQNotSubmitted + strFilter + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()).ToString();
                    //strSubmitted = DataServer.ExecuteScalar(strQTotalSubmitted + strFilter).ToString();
                    strNotYetDue = DataServer.ExecuteScalar(strQNotYetDue + strFilter + ReturnCutoffFilterQry()).ToString();
                    strDueandNotSubmitted = DataServer.ExecuteScalar(strQDueandNotSubmitted + strFilter + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()).ToString();
                    strDelayedFiling = DataServer.ExecuteScalar(strQDelayedFiling + strFilter + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()).ToString();
                    string strRopnSR = DataServer.ExecuteScalar(strQRopnSR + strFilter + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()).ToString();
                    //>>

                    //<<Added by Kiran Kharat On 20Mar2018
                    //////intTotalCount = Convert.ToInt32(strCompliantPercent.Split('|')[1]) + Convert.ToInt32(strNonCompliantPercent.Split('|')[1]) + Convert.ToInt32(strNotApplicable.Split('|')[1]) + Convert.ToInt32(strNotSubmitted.Split('|')[1]);
                    //>>

                    Double CompliantandNotApplicablePercent = Convert.ToDouble(strCompliantPercent.Split('|')[0]) + Convert.ToDouble(strNotApplicable.Split('|')[0]);
                    //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                    strTotal = DataServer.ExecuteScalar(strQTotal + strFilter + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()).ToString();
                    //>>

                    strScript = "<script type=\"text/javascript\">Morris.Donut({ element: 'donut-chart-color',data: [";
                    strScript += "{value: " + CompliantandNotApplicablePercent + ", label: 'Compliant/Not Applicable'},";
                    strScript += "{value: " + strDelayedFiling.Split('|')[0] + ", label: 'Delayed Filings'},";
                    strScript += "{value: " + strNonCompliantPercent.Split('|')[0] + ", label: 'Not Compliant'},";
                    //strScript += "{value: " + strNotApplicable.Split('|')[0] + ", label: 'Not Applicable'},";
                    //strScript += "{value: " + strNotSubmitted.Split('|')[0] + ", label: 'Not Yet Submitted'},";
                    //strScript += "{value: " + strSubmitted.Split('|')[0] + ", label: 'Submitted but not closed'},";
                    //<<Added by Ankur Tyagi on 25Nov2023 for CR_894
                    strScript += "{value: " + strRopnSR.Split('|')[0] + ", label: 'Reopened / Sent back'},";
                    //strScript += "{value: " + strNotYetDue.Split('|')[0] + ", label: 'Not Yet Due'},";
                    //>>
                    strScript += "{value: " + strDueandNotSubmitted.Split('|')[0] + ", label: 'Due but not Submitted'},";
                    strScript += "], backgroundColor: '#F4F4F4', labelColor: '#000',";
                    strScript += "resize: true,colors: ['#009933','#C45850','red','#65C3A8','#8F002C'],";
                    strScript += "formatter: function (x) { return x + \" % \"}});";
                    strScript += Environment.NewLine + Environment.NewLine;

                    int CompliantandNotApplicableCount = Convert.ToInt32(strCompliantPercent.Split('|')[1]) + Convert.ToInt32(strNotApplicable.Split('|')[1]);
                    strScript += "Morris.Bar({element: 'bar-color-chart',data: [";
                    strScript += "{x: 'Compliant/Not Applicable', y: " + CompliantandNotApplicableCount + "},";
                    strScript += "{x: 'Delayed Filings', y: " + strDelayedFiling.Split('|')[1] + "},";
                    strScript += "{x: 'Not Compliant', y: " + strNonCompliantPercent.Split('|')[1] + "},";
                    //strScript += "{x: 'Not Applicable', y: " + strNotApplicable.Split('|')[1] + "},";
                    //strScript += "{x: 'Submitted but not closed', y: " + strSubmitted.Split('|')[1] + "},";
                    //<<Added by Ankur Tyagi on 25Nov2023 for CR_894
                    strScript += "{x: 'Reopened / Sent back', y: " + strRopnSR.Split('|')[1] + "},";
                    //strScript += "{x: 'Not Yet Due', y: " + strNotYetDue.Split('|')[1] + "},";
                    //>>
                    strScript += "{x: 'Due but not Submitted', y: " + strDueandNotSubmitted.Split('|')[1] + "},";
                    //strScript += "{x: 'Not Yet Submitted', y: " + strNotSubmitted.Split('|')[1] + "},";
                    strScript += "],xkey: 'x',ykeys: ['y'],labels: ['Y'],resize: true,xLabelAngle: 60,barColors: ";
                    strScript += "function (row, series, type) {if (type === 'bar') {if(row.label == 'Compliant/Not Applicable'){return '#009933'}";
                    strScript += "if(row.label == 'Not Compliant'){return 'red'}";//Rahuldeb on 18Jan2021 changed #ffcc00 to red
                                                                                  //if(row.label == 'Not Applicable'){return '#0033cc'}";
                    strScript += //"if(row.label == 'Not Yet Submitted'){return '#ff0000'}"+
                                 //"if(row.label == 'Submitted but not closed'){return '#54BB72'}" +
                                 //"if(row.label == 'Not Yet Due'){return '#65C3A8'}" +
                        "if(row.label == 'Due but not Submitted'){return '#8F002C'}" +
                        "if(row.label == 'Delayed Filings'){return '#C45850'}" +
                                  "}else {return '#000';}}";
                    strScript += "});";
                    strScript += "</script>";
                    litChart.Text = strScript;

                    strHtmlTable = "<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'>";
                    strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Compliant</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Delayed Filings</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Compliant</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Applicable</td>";
                    //strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted but not Closed</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Due but not Submitted</td>";
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Yet Due</td>";
                    //strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Yet Submitted</td>";
                    //<<Added by Ankur Tyagi on 25Nov2023 for CR_894
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reopened / Sent back</td>";
                    //>>
                    //<<Added by Kiran Kharat on 20Mar2018 
                    strHtmlTable += "<td class= 'DBTableFirstCellRight'>Total (excluding Not yet due)</td></tr>";
                    //>>

                    strHtmlTable += "<tr><td class= 'DBTableCellLeft'>" +
                                    "<center><a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=2&Status=Y&FromDate=" + strFromDate + "&ToDate=" + strToDate +
                                    "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    strCompliantPercent.Split('|')[1] +
                                    "</a>" +
                                    "</center></td>";

                    strHtmlTable += "<td class= 'DBTableCellLeft'>" +
                                    "<center><a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=2&Status=DC&FromDate=" + strFromDate + "&ToDate=" + strToDate +
                                    "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    strDelayedFiling.Split('|')[1] +
                                    "</a>" +
                                    "</center></td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=2&Status=N&FromDate=" + strFromDate + "&ToDate=" + strToDate +
                                    "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    strNonCompliantPercent.Split('|')[1] +
                                    "</a>" +
                                    "</td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=2&Status=NA&FromDate=" + strFromDate + "&ToDate=" + strToDate +
                                    "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    strNotApplicable.Split('|')[1] +
                                    "</a>" +
                                    "</td>";

                    //strHtmlTable += "<td class= 'DBTableCellRight'>" +
                    //               "<a href='#'  onclick=\"window.open(" +
                    //               "'DetailedReport.aspx?ReportType=2&Status=S&FromDate=" + strFromDate + "&ToDate=" + strToDate +
                    //               "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                    //               "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                    //              strSubmitted.Split('|')[1] +
                    //               "</a>" +
                    //               "</td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                   "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                   "'DetailedReport.aspx?ReportType=2&Status=DN&FromDate=" + strFromDate + "&ToDate=" + strToDate +
                                   "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                   "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    strDueandNotSubmitted.Split('|')[1] +
                                   "</a>" +
                                   "</td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                  "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                  "'DetailedReport.aspx?ReportType=2&Status=ND&FromDate=" + strFromDate + "&ToDate=" + strToDate +
                                  "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                  "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                   //strNotYetDue.Split('|')[1] +
                                   strNotYetDue +
                                  "</a>" +
                                  "</td>";

                    //strHtmlTable += "<td class= 'DBTableCellRight'>" +
                    //                "<a href='#'  onclick=\"window.open(" +
                    //                "'DetailedReport.aspx?ReportType=2&Status=&FromDate=" + strFromDate + "&ToDate=" + strToDate +
                    //                "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                    //                "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                    //                strNotSubmitted.Split('|')[1] +
                    //                "</a>" +
                    //                "</td>";
                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                   "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                   "'DetailedReport.aspx?ReportType=2&Status=SR&FromDate=" + strFromDate + "&ToDate=" + strToDate +
                                   "&SRDID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                   "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                   strRopnSR.Split('|')[1] +
                                   //intTotalCount +
                                   "</a>" +
                                   "</td>";
                    //<<Added by Kiran Kharat On 20Mar2018
                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                   "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                   "'DetailedReport.aspx?ReportType=2&FromDate=" + strFromDate + "&ToDate=" + strToDate +
                                   "&SRDID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                   "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                   strTotal.Split('|')[1] +
                                   //intTotalCount +
                                   "</a>" +
                                   "</td></tr>";
                    //>>
                    strHtmlTable += "</table><br/><br/>";
                    litComplianceStatus.Text = strHtmlTable;
                    pnlHideShow("pnlComplianceStatus");
                }
                else
                {
                    pnlComplianceStatus.Visible = false;
                    writeError("No data found.");
                }
            }
            catch (Exception ex)
            {
                writeError("system Exception in getComplianceWiseStatus(): " + ex);
            }

        }
        private void getTrackingDeptWiseCompliedNotCompliedCount()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            StringBuilder sbHtmlTable = new StringBuilder();
            SqlConnection myconnection = new SqlConnection(mstrConnectionString);
            string strDeptName = "", strDeptId = "", strChart = "", strHtmlTable = "";
            string strReportingFunction = "", strFrequency = ""
                   , strFromDate = "", strToDate = "", strPriority = "", strFilterQuery = "", strToDateFilter = "", strFromDateFilter = "";
            int intCompliedCount, intNonCompliedCount, intNACount, intNotYetDue = 0, intDelayedCount = 0, intNotSubmitted = 0, intTotalSubmitted = 0,
                intTotalCount, intDueandNotSubmitted = 0, intOverAllCount = 0;

            dt = (utilBLL.getDataSet("TrackingDept")).Tables[0];

            sbHtmlTable.Append("<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'>");
            sbHtmlTable.Append("<tr><td class= 'DBTableFirstCellRight'>Department</td>");
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Delayed Filings</td>");
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Compliant</td>");
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Not Compliant</td>");
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Not Applicable</td>");
            //strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted but not Closed</td>";
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Due but not Submitted</td>");
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Not Yet Due</td>");
            //strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Yet Submitted</td>";
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Reopened / Sent back</td>");
            //<<Added by Kiran Kharat On 20Mar2018
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Total(excluding Not yet due)</td></tr>");
            //>>
            pnlHideShow("pnlDeptWiseBarchart");
            strChart = "<script type=\"text/javascript\"> Morris.Bar({element: 'bar-chart' ,data: [";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    strDeptName = dr["STM_TYPE"].ToString();
                    strDeptId = dr["STM_ID"].ToString();

                    //strQuery1 = " select  STM_TYPE,SUB_YES_NO_NA,count(1) as TotCount,STM_ID from  TBL_SUB_CHKLIST " +
                    //            " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  and STM_ID = " + strDeptId +
                    //            " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                    //            " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                    //            " where SUB_STATUS in('S','C') ";//and 

                    if (!ddlReportDept.SelectedValue.Equals(""))
                    {
                        strFilterQuery += " and SM_SRD_ID = " + ddlReportDept.SelectedValue;
                        //strQuery1 += " and SM_SRD_ID = " + ddlReportDept.SelectedValue;
                        strReportingFunction = ddlReportDept.SelectedValue;
                    }

                    if (!ddlFrequency.SelectedValue.Equals(""))
                    {
                        strFilterQuery += " and SM_FREQUENCY = '" + ddlFrequency.SelectedValue + "' ";
                        //strQuery1 += " and SM_FREQUENCY = '" + ddlFrequency.SelectedValue + "' ";
                        strFrequency = ddlFrequency.SelectedValue;
                    }

                    if (!txtFromdate.Text.Equals(""))
                    {
                        strFromDateFilter += " and SC_DUE_DATE_TO >= '" + txtFromdate.Text + "' ";
                        //strQuery1 += " and SC_DUE_DATE_FROM >= '" + txtFromdate.Text + "' ";
                        strFromDate = txtFromdate.Text;
                    }

                    if (!txtTodate.Text.Equals(""))
                    {
                        strToDateFilter += " and SC_DUE_DATE_TO <= '" + txtTodate.Text + "' ";
                        //strQuery1 += " and SC_DUE_DATE_TO <= '" + txtTodate.Text + "' ";
                        strToDate = txtTodate.Text;
                    }

                    if (!ddlPriority.SelectedValue.Equals(""))
                    {
                        strFilterQuery += " and SC_PRIORITY = '" + ddlPriority.SelectedValue + "' ";
                        //strQuery1 += " and SC_PRIORITY = '" + ddlPriority.SelectedValue + "' ";
                        strPriority = ddlPriority.SelectedValue;
                    }

                    //strQuery1 += " group by STM_TYPE,STM_ID,SUB_YES_NO_NA order by STM_TYPE ASC ";

                    //DataTable dtCount = new DataTable();
                    //SqlCommand cmd = new SqlCommand(strQuery1, myconnection);
                    //SqlDataAdapter searchDataAdaptor = new SqlDataAdapter(cmd);
                    //searchDataAdaptor.Fill(dtCount);

                    intCompliedCount = 0;
                    intNonCompliedCount = 0;
                    intNACount = 0;
                    intNotSubmitted = 0;
                    intTotalCount = 0;
                    intNotYetDue = 0;
                    intTotalSubmitted = 0;
                    intDueandNotSubmitted = 0;
                    //intCompliedTotalCount = 0;
                    //intNonCompliedTotalCount = 0;
                    //intNATotalCount = 0;
                    //intNotSubmittedTotal = 0;
                    //intOverAllCount = 0;

                    string strCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
                    " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID " +
                    " inner  join TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID and SM_STM_ID = " + strDeptId +
                    " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID  " +
                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                    " WHERE SUB_STATUS in ('S','C') " +
                    " AND (CAST(ISNULL(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) AS DATE)) <= CAST(SC_DUE_DATE_TO AS DATE) " +
                    " AND SUB_YES_NO_NA = 'Y' ";

                    string strNonCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
                    " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID " +
                    " inner  join TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID and SM_STM_ID = " + strDeptId +
                    " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                    " where SUB_STATUS in ('S', 'C') " +
                    //<<Commented By Amey Karangutkar on 28-Jul-2018
                    //" and (isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) > SC_DUE_DATE_TO OR SUB_YES_NO_NA = 'N') ";
                    //>>
                    //Commented and added by Rahuldeb on 10Mar2022
                    //" and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') ";
                    " and SUB_YES_NO_NA = 'N' ";
                    //>>

                    string strNACount = " select count(1) from TBL_SUB_CHKLIST " +
                    " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                    " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID and SM_STM_ID = " + strDeptId +
                    " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID  " +
                    " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                    " where SUB_STATUS in ('S','C') and SUB_YES_NO_NA = 'NA' ";

                    string strQDueandNotSubmitted = " select Count(1) from TBL_SUB_CHKLIST " +
                    " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                    " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                    " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                    " where SM_STM_ID = " + strDeptId +
                    //" and SC_DUE_DATE_TO <= current_timestamp AND isnull(SUB_STATUS,'') = ''" +
                    " AND isnull(SUB_STATUS,'') = ''";

                    //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                    if (string.IsNullOrEmpty(txtTodate.Text))
                    {
                        strQDueandNotSubmitted += " AND CAST(SC_DUE_DATE_TO AS DATE) <= CAST(CURRENT_TIMESTAMP AS DATE) ";
                    }
                    else
                    {
                        strQDueandNotSubmitted += " AND CAST(SC_DUE_DATE_TO AS DATE) <= CAST('" + txtTodate.Text + "' AS DATE) ";
                    }
                    //>>

                    string strNotYetDue = " select Count(1) from TBL_SUB_CHKLIST " +
                    " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                    " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                    " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                    //" where SM_STM_ID = " + strDeptId + " and SC_DUE_DATE_TO > current_timestamp" + strFilterQuery;
                    " where SM_STM_ID = " + strDeptId;

                    //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                    if (string.IsNullOrEmpty(txtTodate.Text))
                    {
                        strNotYetDue += " AND CAST(SC_DUE_DATE_TO AS DATE) > CAST(CURRENT_TIMESTAMP AS DATE) and isnull(SUB_STATUS, '') = '' ";
                    }
                    else
                    {
                        strNotYetDue += " AND CAST(SC_DUE_DATE_TO AS DATE) > CAST('" + txtTodate.Text + "' AS DATE) and isnull(SUB_STATUS, '') = '' ";
                    }
                    //>>

                    //<<Added by Rahuldeb on 10Mar2022
                    string strDelayedFilings = " select count(1) from TBL_SUB_CHKLIST " +
                    " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                    " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID and SM_STM_ID = " + strDeptId +
                    " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID  " +
                    " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                    " where SUB_STATUS in ('S','C') " +
                    " and CAST(ISNULL(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) AS DATE) > CAST(SC_DUE_DATE_TO AS DATE) and SUB_YES_NO_NA = 'Y' ";
                    //>>

                    string strQRopnSR = " SELECT Count(*) " +
                                        " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                                        " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                                        " INNER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where ISNULL(SUB_STATUS,'') in ('R','SR')" +
                                        " AND SM_STM_ID = " + strDeptId;

                    string strQTotal = " select Count(*) from TBL_SUB_CHKLIST " +
                                        " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                                        " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                        " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                        " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                        //" where SM_STM_ID = " + strDeptId + " " + strFilterQuery + " and isnull(SUB_STATUS,'') != 'R' ";
                                        " where SM_STM_ID = " + strDeptId;

                    //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
                    intCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strCompliedCount + strFilterQuery + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()));
                    intNonCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strNonCompliedCount + strFilterQuery + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()));
                    intNACount = Convert.ToInt32(DataServer.ExecuteScalar(strNACount + strFilterQuery + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()));
                    //intTotalSubmitted = Convert.ToInt32(DataServer.ExecuteScalar(strTotalSubmitted));
                    intNotYetDue = Convert.ToInt32(DataServer.ExecuteScalar(strNotYetDue + strFilterQuery + ReturnCutoffFilterQry()));
                    intDueandNotSubmitted = Convert.ToInt32(DataServer.ExecuteScalar(strQDueandNotSubmitted + strFilterQuery + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()));
                    int intRopnSR = Convert.ToInt32(DataServer.ExecuteScalar(strQRopnSR + strFilterQuery + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()));
                    intTotalCount = Convert.ToInt32(DataServer.ExecuteScalar(strQTotal + strFilterQuery + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()));
                    //<<Added by Rahuldeb on 10Mar2022
                    intDelayedCount = Convert.ToInt32(DataServer.ExecuteScalar(strDelayedFilings + strFilterQuery + strFromDateFilter + strToDateFilter + ReturnCutoffFilterQry()));
                    //>>
                    //>>
                    intOverAllCount = intOverAllCount + intTotalCount;
                    //>>
                    strChart += "{x: '" + strDeptName + "', DC: '" + intDelayedCount + "', Y: " + intCompliedCount + " , N: " + intNonCompliedCount + " ," +
                                " NA: " + intNACount + " ,  ND: " + intNotYetDue + ", DN: " + intDueandNotSubmitted + ", SR:" + intRopnSR + "},";

                    sbHtmlTable.Append("<tr><td class= 'DBTableCellLeft'><center>" + strDeptName + "</center></td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=1&Status=DC&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                    "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    intDelayedCount +
                                    "</a>" +
                                    "</td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=1&Status=Y&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                    "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    intCompliedCount +
                                    "</a>" +
                                    "</td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                    "<a href='#'class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=1&Status=N&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                    "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    intNonCompliedCount +
                                    "</a>" +
                                    "</td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=1&Status=NA&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                    "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    +intNACount +
                                    "</a>" +
                                    "</td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                  "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                  "'DetailedReport.aspx?ReportType=1&Status=DN&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                  "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                  "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                  intDueandNotSubmitted +
                                  "</a>" +
                                  "</td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                   "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                   "'DetailedReport.aspx?ReportType=1&Status=ND&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                   "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                   "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                   intNotYetDue +
                                   "</a>" +
                                   "</td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                   "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                   "'DetailedReport.aspx?ReportType=1&Status=SR&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                   "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                   "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">"
                                   + intRopnSR +
                                   "</a>" +
                                   "</td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                   "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                   "'DetailedReport.aspx?ReportType=1&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                   "&SRDID=" + strReportingFunction + "&STMID=" + strDeptId + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                   "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                   +intTotalCount +
                                   "</a>" +
                                   "</td></tr>");

                }


                sbHtmlTable.Append("</table>");
                litDeptWiseBarchart.Text = sbHtmlTable.ToString();
                strChart = strChart.Substring(0, strChart.Length - 1);
                strChart += " ], xkey: 'x', ykeys: ['DC','Y','N','NA','ND','DN','SR'], " +
                            " labels: ['Delayed Filings','Compliant','Not Compliant','Not Applicable','Not Yet Due','Due but not Submitted','Reopened / Sent back']," +
                            " barColors: ['#C45850','#009933','Red','#267DD4','#65C3A8','#8F002C','#508ec4'], xLabelAngle: -45, overflow:'visible',resize: true});";//Rahuldeb on 18Jan2020 Changed #8e5ea2 to red & #3e95cd changed to #009933
                strChart += "</script>";
                litChart.Text = strChart;
            }
        }
        private void getMonthlyComplianceReport()
        {
            DataServer objds = new DataServer();
            string strTrackedBy = "", strReportingFunction = "", strFrequency = ""
                    , strFromDate = "", strToDate = "", strPriority = "", strFilter = "", strtotalRecords = "", strToDateFilter = "", strFromDateFilter = "";
            string strCompliantPercent = "", strNonCompliantPercent = "", strNotApplicable = "", strNotSubmitted = "", strDueandNotSubmitted = "", strSubmitted = "", strNotYetDue = ""; ;
            string strQCompliantPercent = "", strQNonCompliantPercent = "", strQNotApplicable = "", strQNotSubmitted = "", strQNotYetDue = "",
                   strQDueandNotSubmitted = "", strQTotalSubmitted = "", strQTotal = "", strTotal = "",
                   strQDelayedFilings = "", strDelayedFilings = "";
            string strMonth = "", strYear = "";

            int intTotalCount;
            string strFilterIn = "";
            string strTotalQuery = "";
            string strScript = "";
            DataRow drTotals;
            pnlHideShow("pnlMonthlyCompliance");

            string strHtmlTable = "<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'>";
            strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Year</td>";
            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Compliant</td>";
            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Delayed Filings</td>";
            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Compliant</td>";
            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Applicable</td>";
            //strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted but not Closed</td>";

            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Due but not Submitted</td>";
            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Yet Due</td>";
            //strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Yet Submitted</td>";
            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Reopened / Sent back</td>";
            //<<Added by Kiran Kharat On 20Mar2018
            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Total (excluding Not yet due)</td></tr>";
            //>>

            if (ddlSubType.SelectedValue != "")
            {
                strTrackedBy = ddlSubType.SelectedValue;
                strFilter += " and SM_STM_ID = " + strTrackedBy + " ";
            }
            if (ddlReportDept.SelectedValue != "")
            {
                strReportingFunction = ddlReportDept.SelectedValue;
                strFilter += " and SM_SRD_ID = " + strReportingFunction + " ";
            }
            if (ddlFrequency.SelectedValue != "")
            {
                strFrequency = ddlFrequency.SelectedValue;
                strFilter += " and SM_FREQUENCY = '" + strFrequency + "' ";
            }
            if (txtFromdate.Text != "")
            {
                strFromDate = txtFromdate.Text;
                strFromDateFilter += " and SC_DUE_DATE_TO >= '" + strFromDate + "' ";
            }
            if (txtTodate.Text != "")
            {
                strToDate = txtTodate.Text;
                strToDateFilter += " and SC_DUE_DATE_TO <= '" + strToDate + "' ";
            }
            if (ddlPriority.SelectedValue != "")
            {
                strPriority = ddlPriority.SelectedValue;
                strFilter += " and SC_PRIORITY = '" + strPriority + "' ";
            }

            strTotalQuery = "SELECT count(1) as [Total Submissions], month(SC_DUE_DATE_TO) as [Month], " +
                    " year(SC_DUE_DATE_TO) as [Year]  FROM TBL_SUB_CHKLIST " +
                   "INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                   "INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                   "LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where 1=1 " +
                   "and isnull(month(SC_DUE_DATE_TO),'') != '' " + strFilter + strFromDateFilter + strToDateFilter +
                   //" and isnull(SUB_STATUS,'') != 'R' " +
                   "group by month(SC_DUE_DATE_TO), year(SC_DUE_DATE_TO) " +
                   //Added by Rahuldeb on 18Jan2020 to order the Monthly report
                   "order by year(SC_DUE_DATE_TO), month(SC_DUE_DATE_TO) ";
            //>>

            DataTable dtTotals = new DataTable();
            dtTotals = objds.Getdata(strTotalQuery);

            if (dtTotals.Rows.Count > 0)
            {
                strScript = "<script type=\"text/javascript\">Morris.Bar({ element: 'stacked-bars',data: [";

                for (int i = 0; i < dtTotals.Rows.Count; i++)
                {
                    drTotals = dtTotals.Rows[i];
                    strtotalRecords = drTotals["Total Submissions"].ToString();
                    strMonth = drTotals["Month"].ToString();
                    strYear = drTotals["Year"].ToString();

                    strFilterIn = " and month(SC_DUE_DATE_TO) = " + strMonth + " and year(SC_DUE_DATE_TO) = '" + strYear + "' ";
                    string strFilterIn1 = " and month(SC_DUE_DATE_TO) = " + strMonth + " ";

                    strQCompliantPercent = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                            " LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID " +
                            " where (CAST(ISNULL(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) AS DATE)) <= CAST(SC_DUE_DATE_TO AS DATE) " +
                            " and SUB_YES_NO_NA = 'Y' AND SUB_STATUS in ('S','C') ";

                    strQDelayedFilings = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                            " LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID " +
                            " where SUB_STATUS in ('S','C') " +
                            " and CAST(ISNULL(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) AS DATE) > CAST(SC_DUE_DATE_TO AS DATE) " +
                            " and SUB_YES_NO_NA = 'Y' ";

                    strQNonCompliantPercent = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                            " LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where SUB_STATUS in ('S','C') " +
                            " and (SUB_YES_NO_NA = 'N') " +
                            " AND SUB_STATUS in ('S','C') ";

                    strQNotApplicable = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                            " LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID " +
                            " where SUB_YES_NO_NA = 'NA' AND SUB_STATUS in ('S','C')";

                    strQNotSubmitted = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                            " LEFT OUTER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID " +
                            " where (SUB_STATUS is null or SUB_STATUS = '')";
                    //
                    //strQTotalSubmitted = " select  cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                    //       " from TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                    //       " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                    //       " where SUB_STATUS in ('S','R','SR')";

                    //strQNotYetDue = " select  cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(18,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                    strQNotYetDue = " select Count(1) " +
                            " from TBL_SUB_CHKLIST " +
                            " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                            " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                            //" where SC_DUE_DATE_TO >= current_timestamp";
                            " where 1 = 1";
                    //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                    if (string.IsNullOrEmpty(txtTodate.Text))
                    {
                        strQNotYetDue += " AND CAST(SC_DUE_DATE_TO AS DATE) > CAST(CURRENT_TIMESTAMP AS DATE) and isnull(SUB_STATUS, '') = '' ";
                    }
                    else
                    {
                        strQNotYetDue += " AND CAST(SC_DUE_DATE_TO AS DATE) > CAST('" + txtTodate.Text + "' AS DATE) and isnull(SUB_STATUS, '') = '' ";
                    }
                    //>>

                    strQDueandNotSubmitted = " select  cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                           " from TBL_SUB_CHKLIST " +
                           " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                           " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                           " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                           //" where SC_DUE_DATE_TO <= current_timestamp and (SUB_STATUS is null or SUB_STATUS = '')";
                           " where isnull(SUB_STATUS, '') = ''";

                    //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                    if (string.IsNullOrEmpty(txtTodate.Text))
                    {
                        strQDueandNotSubmitted += " AND CAST(SC_DUE_DATE_TO AS DATE) <= CAST(CURRENT_TIMESTAMP AS DATE) ";
                    }
                    else
                    {
                        strQDueandNotSubmitted += " AND CAST(SC_DUE_DATE_TO AS DATE) <= CAST('" + txtTodate.Text + "' AS DATE) ";
                    }
                    //>>

                    string strQRopnSR = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                            " INNER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where ISNULL(SUB_STATUS,'') in ('R','SR')";

                    strQTotal = " select  cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + cast(count(1) as varchar) " +
                            " from TBL_SUB_CHKLIST " +
                            " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                            " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                            //" where 1=1 and isnull(SUB_STATUS,'') != 'R' ";
                            " where 1=1 ";


                    strCompliantPercent = DataServer.ExecuteScalar(strQCompliantPercent + strFilter + strFilterIn + strFromDateFilter + strToDateFilter).ToString();
                    strDelayedFilings = DataServer.ExecuteScalar(strQDelayedFilings + strFilter + strFilterIn + strFromDateFilter + strToDateFilter).ToString();
                    strNonCompliantPercent = DataServer.ExecuteScalar(strQNonCompliantPercent + strFilter + strFilterIn + strFromDateFilter + strToDateFilter).ToString();
                    strNotApplicable = DataServer.ExecuteScalar(strQNotApplicable + strFilter + strFilterIn + strFromDateFilter + strToDateFilter).ToString();
                    strNotSubmitted = DataServer.ExecuteScalar(strQNotSubmitted + strFilter + strFilterIn + strFromDateFilter + strToDateFilter).ToString();
                    //strSubmitted = DataServer.ExecuteScalar(strQTotalSubmitted + strFilter + strFilterIn).ToString();
                    //strNotYetDue = DataServer.ExecuteScalar(strQNotYetDue + strFilter + strFilterIn).ToString();
                    strNotYetDue = DataServer.ExecuteScalar(strQNotYetDue + strFilter + strFilterIn1).ToString();
                    strDueandNotSubmitted = DataServer.ExecuteScalar(strQDueandNotSubmitted + strFilter + strFilterIn + strFromDateFilter + strToDateFilter).ToString();
                    string strRopnSR = DataServer.ExecuteScalar(strQRopnSR + strFilter + strFilterIn + strFromDateFilter + strToDateFilter).ToString();
                    strTotal = DataServer.ExecuteScalar(strQTotal + strFilter + strFilterIn + strFromDateFilter + strToDateFilter).ToString();


                    //<<Added by Kiran Kharat On 20Mar2018
                    //intTotalCount = Convert.ToInt32(strCompliantPercent.Split('|')[1]) + Convert.ToInt32(strNonCompliantPercent.Split('|')[1]) + Convert.ToInt32(strNotApplicable.Split('|')[1]) + Convert.ToInt32(strNotSubmitted.Split('|')[1]);
                    //>>

                    double dblCompliantNotApplicablePercent = Convert.ToDouble(strCompliantPercent.Split('|')[0]) + Convert.ToDouble(strNotApplicable.Split('|')[0]);
                    strScript += "{x: '" + getMonthName(strMonth) + '-' + strYear + "',";
                    strScript += " a: " + strCompliantPercent.Split('|')[0] + ",";
                    strScript += " b: " + strDelayedFilings.Split('|')[0] + ",";
                    strScript += " c: " + strNonCompliantPercent.Split('|')[0] + ",";
                    //strScript += " c: " + strNotApplicable.Split('|')[0] + ",";
                    //strScript += " d: " + strNotSubmitted.Split('|')[0] + ",";
                    //strScript += " e: " + strSubmitted.Split('|')[0] + ",";
                    strScript += " d: " + strDueandNotSubmitted.Split('|')[0] + ",";
                    strScript += " e: " + strRopnSR.Split('|')[0] + "},";
                    //strScript += " f: " + strNotYetDue.Split('|')[0] + "},";

                    strHtmlTable += "<tr><td class= 'DBTableCellLeft'><center><b>" + getMonthName(strMonth) + '-' + strYear + "</b></center></td>";
                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=3&Status=Y&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
                                    "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    strCompliantPercent.Split('|')[1] +
                                    "</a></td>";
                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=3&Status=DC&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
                                    "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    strDelayedFilings.Split('|')[1] +
                                    "</a></td>";
                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=3&Status=N&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
                                    "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    strNonCompliantPercent.Split('|')[1] +
                                    "</a></td>";
                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=3&Status=NA&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
                                    "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    strNotApplicable.Split('|')[1] +
                                    "</a></td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                "<a href='#'class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
                                "'DetailedReport.aspx?ReportType=3&Status=DN&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
                                "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                strDueandNotSubmitted.Split('|')[1] +
                                "</a></td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                  "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                  "'DetailedReport.aspx?ReportType=3&Status=ND&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
                                  "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                  "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                  //strNotYetDue.Split('|')[1] +
                                  strNotYetDue +
                                  "</a></td>";

                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                  "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                  "'DetailedReport.aspx?ReportType=3&Status=SR&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
                                  "&SRDID=" + strReportingFunction + "&STMID=" + strTrackedBy + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                  "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                  strRopnSR.Split('|')[1] +
                                  "</a></td>";

                    //<<Added by Kiran Kharat On 20Mar2018
                    strHtmlTable += "<td class= 'DBTableCellRight'>" +
                                   "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                   "'DetailedReport.aspx?ReportType=3&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&MonthYear=" + strMonth + "-" + strYear +
                                   "&SRDID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                   "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                   strTotal.Split('|')[1] +
                                   "</a>" +
                                   "</td></tr>";
                    //>>
                }
                strHtmlTable += "</table>";
                strScript = strScript.Substring(0, strScript.Length - 1);
                strScript += "],xkey: 'x',ykeys: ['a','b','c','d','e'],labels: ['Compliant/Not Applicable','Delayed Filings','Non-Compliant','Due but not Submitted','Reopened / Sent back'],";
                strScript += "barColors: ['#009933','#C45850','Red','#8F002C','#508ec4'], xLabelAngle: 60, stacked: true, resize: true});</script>";//Rahuldeb on 18Jan2020 #ffcc00 to red
                litChart.Text = strScript;
                litMonthlyReport.Text = strHtmlTable;
            }
            else
            {
                pnlMonthlyCompliance.Visible = false;
                writeError("No data found.");
            }
        }
        public void getRegulatoryDashboardforPrevMonth()
        {
            DataServer dataServer = new DataServer();
            DataTable dtDistinctTrackReptDept;
            string strQuery = "", strContent = "", strHostingServer = "", strTrackingDeptId = "", strReportingDeptId = "",
                   strTrackDeptName = "", strReptDeptName = "", strHTML = "", strFilter = "", strTrackedBy = "", strReportingFunction = "",
                   strFrequency = "", strPriority = "";

            DataRow drDistinctTrackReptDept;
            int intTotalTasks = 0, intClosedBeforeDuedate = 0, intSubmittedBeforeDuedate = 0, intNotClosed = 0, intSubmittedAfterDuedate = 0, intNotSubmitted = 0;

            strHostingServer = ConfigurationManager.AppSettings["HostingServer"].ToString();
            SqlConnection myconnection = new SqlConnection(mstrConnectionString);

            string strMonth = DateTime.Now.AddMonths(-1).ToString("MM");
            string strCurrMonth = DateTime.Now.AddMonths(-1).ToString("MMMM yyyy");

            pnlHideShow("pnlRegulatoryreportingPrevmonth");
            lblCurrMnth.Text = strCurrMonth;


            if (ddlSubType.SelectedValue != "")
            {
                strTrackedBy = ddlSubType.SelectedValue;
                strFilter += " and SM_STM_ID = " + strTrackedBy + " ";
            }
            if (ddlReportDept.SelectedValue != "")
            {
                strReportingFunction = ddlReportDept.SelectedValue;
                strFilter += " and SM_SRD_ID = " + strReportingFunction + " ";
            }
            if (ddlFrequency.SelectedValue != "")
            {
                strFrequency = ddlFrequency.SelectedValue;
                strFilter += " and SM_FREQUENCY = '" + strFrequency + "' ";
            }
            if (ddlPriority.SelectedValue != "")
            {
                strPriority = ddlPriority.SelectedValue;
                strFilter += " and SC_PRIORITY = '" + strPriority + "' ";
            }
            try
            {
                strQuery = " SELECT Distinct SM_SRD_ID, SRD_NAME, SC_STM_ID, STM_TYPE  " +
                           " FROM TBL_SUB_CHKLIST " +
                           " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Apr-2023' " +
                           " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                           " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " +
                            " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " + strFilter +
                           " order by STM_TYPE,SRD_NAME ASC ";

                //<< Modified by Amarjeet on 15-Jul-2021
                //dtDistinctTrackReptDept = new DataTable();
                //cmdDistinctTrackReptDept = new SqlCommand(strQuery, myconnection);
                //sdaDistinctTrackReptDept = new SqlDataAdapter(cmdDistinctTrackReptDept);
                //sdaDistinctTrackReptDept.Fill(dtDistinctTrackReptDept);
                dtDistinctTrackReptDept = dataServer.Getdata(strQuery);
                //>>

                if (dtDistinctTrackReptDept.Rows.Count > 0)
                {

                    strHTML += "<table class='table table-bordered footable' id=\"tfhover\"  style=\"font-size: 12px; color: #333333; " +
                                    "width: 100%; border-width: 1px; border-color: #bcaf91; border-collapse: collapse;\">" +
                                    "<tr style=\"background-color: #ffffff;\">" +
                                    "<th>" +
                                    "Sr. No.</th>" +

                                    "<th>" +
                                    "Tracking Department</th>" +

                                    "<th>" +
                                    "Reporting Department</th>" +

                                    "<th>" +
                                    "Total Tasks For " + strCurrMonth + "</th>" +

                                    "<th>" +
                                    "Submitted and Closed within System Due Date</th>" +

                                    "<th>" +
                                    "Submitted before System Due date, but Closed after System Due Date</th>" +

                                    "<th>" +
                                    "Submitted and Closed after System Due Date</th>" +

                                    "<th>" +
                                    "Submitted but not yet Closed</th>" +

                                    "<th>" +
                                    "Not Yet Submitted</th></tr>";

                    for (int i = 0; i < dtDistinctTrackReptDept.Rows.Count; i++)
                    {
                        drDistinctTrackReptDept = dtDistinctTrackReptDept.Rows[i];
                        strTrackingDeptId = drDistinctTrackReptDept["SC_STM_ID"].ToString();
                        strReportingDeptId = drDistinctTrackReptDept["SM_SRD_ID"].ToString();
                        strTrackDeptName = drDistinctTrackReptDept["STM_TYPE"].ToString();
                        strReptDeptName = drDistinctTrackReptDept["SRD_NAME"].ToString();

                        string strTotalCountQuery = " select count(*) from TBL_SUB_CHKLIST " +
                                                    " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Apr-2023' " +
                                                    " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                                    " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                                    " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID  " +
                                                    " where  month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE()))" +
                                                    " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                                                    " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                                                    " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId;

                        string strClosedBeforeDueDate = " select count(*) from TBL_SUB_CHKLIST " +
                                                        " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Apr-2023' " +
                                                        " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                                        " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                                        " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                                        " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " +
                                                        " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                                                        " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                                                        " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                                                        " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
                                                        " and SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) <= SC_DUE_DATE_TO";

                        string strSubmittedBeforeDueDate = " select count(*) from TBL_SUB_CHKLIST " +
                                                       " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Apr-2023' " +
                                                       " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                                       " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                                       " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                                       " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID  " +
                                                       " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                                                       " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                                                       " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                                                       " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
                                                       " and SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_SUBMIT_DATE)) <= SC_DUE_DATE_TO and " +
                                                       "DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) > SC_DUE_DATE_TO";

                        string strSubmittedAfterDueDate = " select count(*) from TBL_SUB_CHKLIST " +
                                                       " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Apr-2023' " +
                                                       " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                                       " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                                       " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                                       " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID   " +
                                                       " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                                                       " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                                                       " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                                                       " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
                                                       " and SUB_STATUS = 'C' and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_SUBMIT_DATE)) > SC_DUE_DATE_TO and DATEADD(dd, 0, DATEDIFF(dd, 0,SUB_CLOSED_ON)) > SC_DUE_DATE_TO";

                        string strNotClosed = " select count(*) from TBL_SUB_CHKLIST " +
                                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Apr-2023' " +
                                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                              " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                              " inner join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " +
                                              " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                                              " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                                              " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                                              " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
                                              " and SUB_STATUS = 'S' ";

                        string strNotSubmitted = " select count(*) from TBL_SUB_CHKLIST " +
                                              " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID and SC_DUE_DATE_TO >= '01-Apr-2023' " +
                                              " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                              " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                              " INNER JOIN TBL_FIN_YEAR_MAS ON SC_FYM_ID = FYM_ID " +
                                              " where month(SC_DUE_DATE_TO) = month(DATEADD(month, -1, GETDATE())) " +
                                              " and year(SC_DUE_DATE_TO) = case when month(getdate()) = 1 " +
                                              " then year(DATEADD(year, -1, getdate())) else year(getdate())end " +
                                              " and SC_STM_ID = " + strTrackingDeptId + " and SM_SRD_ID = " + strReportingDeptId +
                                              " and not exists (SELECT SUB_ID FROM TBL_SUBMISSIONS " +
                                              " WHERE SUB_SC_ID = SC_ID AND SUB_STATUS in('S','C')) ";




                        intTotalTasks = Convert.ToInt32(DataServer.ExecuteScalar(strTotalCountQuery));
                        intClosedBeforeDuedate = Convert.ToInt32(DataServer.ExecuteScalar(strClosedBeforeDueDate));
                        intSubmittedBeforeDuedate = Convert.ToInt32(DataServer.ExecuteScalar(strSubmittedBeforeDueDate));
                        intSubmittedAfterDuedate = Convert.ToInt32(DataServer.ExecuteScalar(strSubmittedAfterDueDate));
                        intNotClosed = Convert.ToInt32(DataServer.ExecuteScalar(strNotClosed));
                        intNotSubmitted = Convert.ToInt32(DataServer.ExecuteScalar(strNotSubmitted));

                        strHTML += "<tr style=\"background-color: #ffffff;\">" +
                                    "<td style=\"font-size: 12px;width:2.5%;  border-width: 1px; padding: 8px; border-style: solid;" +
                                    "border-color: #bcaf91;\">" + (i + 1) + "</td>" +

                                    "<td style=\"font-size: 12px;width:17.5%;  border-width: 1px; padding: 8px; border-style: solid;" +
                                    "border-color: #bcaf91;\">" + strTrackDeptName + "</td>" +

                                    "<td style=\"font-size: 12px;width:17.5%;  border-width: 1px; padding: 8px; border-style: solid;" +
                                    "border-color: #bcaf91;\">" + strReptDeptName + "</td>" +

                                    "<td style=\"font-size: 12px;width:10%;  border-width: 1px; padding: 8px; border-style: solid;" +
                                    "border-color: #bcaf91;\">" +
                                    "<a target='_blank' href='DetailedReport.aspx?ReportType=4&SRDID=" + strReportingDeptId + "&STMID=" + strTrackingDeptId +
                                    "&Type=1'>" +
                                    +intTotalTasks +
                                    "</a>" +
                                    "</td>" +

                                    "<td style=\"font-size: 12px;width:10%;  border-width: 1px; padding: 8px; border-style: solid;" +
                                    "border-color: #bcaf91;\">" +
                                    "<a target='_blank' href='DetailedReport.aspx?ReportType=4&SRDID=" + strReportingDeptId + "&STMID=" + strTrackingDeptId +
                                    "&Type=2'>" +
                                    +intClosedBeforeDuedate +
                                    "</a>" +
                                    "</td>" +

                                    "<td style=\"font-size: 12px;width:10%;  border-width: 1px; padding: 8px; border-style: solid;" +
                                    "border-color: #bcaf91;\">" +
                                    "<a target='_blank' href='DetailedReport.aspx?ReportType=4&SRDID=" + strReportingDeptId + "&STMID=" + strTrackingDeptId +
                                    "&Type=3'>" +
                                    +intSubmittedBeforeDuedate +
                                    "</a>" +
                                    "</td>" +

                                    "<td style=\"font-size: 12px;width:10%;  border-width: 1px; padding: 8px; border-style: solid;" +
                                    "border-color: #bcaf91;\">" +
                                    "<a target='_blank' href='DetailedReport.aspx?ReportType=4&SRDID=" + strReportingDeptId + "&STMID=" + strTrackingDeptId +
                                    "&Type=4'>" +
                                    +intSubmittedAfterDuedate + "</td>" +

                                    "<td style=\"font-size: 12px;width:10%;  border-width: 1px; padding: 8px; border-style: solid;" +
                                    "border-color: #bcaf91;\">" +
                                    "<a target='_blank' href='DetailedReport.aspx?ReportType=4&SRDID=" + strReportingDeptId + "&STMID=" + strTrackingDeptId +
                                    "&Type=5'>" +
                                    +intNotClosed +
                                    "</a>" +
                                    "</td>" +

                                    "<td style=\"font-size: 12px;width:10%;  border-width: 1px; padding: 8px; border-style: solid;" +
                                    "border-color: #bcaf91;\">" +
                                    "<a target='_blank' href='DetailedReport.aspx?ReportType=4&SRDID=" + strReportingDeptId + "&STMID=" + strTrackingDeptId +
                                    "&Type=6'>" +
                                    +intNotSubmitted +
                                    "</a>" +
                                    "</td></tr>";
                    }


                    strContent = strContent + strHTML + "</table>";


                    litChart.Text = strContent;
                }
            }
            catch (Exception ex)
            {
                writeError("Error in getRegulatoryDashboardforPrevMonth() 2: " + ex.Message);
            }

        }
        private void getReportingDeptWiseCompliedNotCompliedCount()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            StringBuilder sbHtmlTable = new StringBuilder();
            SqlConnection myconnection = new SqlConnection(mstrConnectionString);
            string strDeptName = "", strDeptId = "", strChart = "", strHtmlTable = "";
            string strReportingFunction = "", strFrequency = ""
                   , strFromDate = "", strToDate = "", strPriority = "", strFilterQuery = "", strToDateFilter = "", strFromDateFilter = "";
            int intCompliedCount, intNonCompliedCount, intNACount, intNotYetDue = 0, intDelayedCount = 0, intNotSubmitted = 0, intTotalSubmitted = 0,
                intTotalCount, intDueandNotSubmitted = 0, intOverAllCount = 0;

            dt = (utilBLL.getDataSet("ReportingDept")).Tables[0];

            sbHtmlTable.Append("<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'>");
            sbHtmlTable.Append("<tr><td class= 'DBTableFirstCellRight'>Department</td>");
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Delayed Filings</td>");
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Compliant</td>");
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Not Compliant</td>");
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Not Applicable</td>");
            //strHtmlTable += "<td class= 'DBTableFirstCellRight'>Submitted but not Closed</td>";
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Due but not Submitted</td>");
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Not Yet Due</td>");
            //strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Yet Submitted</td>";
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Reopened / Sent back</td>");
            //<<Added by Kiran Kharat On 20Mar2018
            sbHtmlTable.Append("<td class= 'DBTableFirstCellRight'>Total(excluding Not yet due)</td></tr>");
            //>>
            pnlHideShow("pnlDeptWiseBarchart");
            strChart = "<script type=\"text/javascript\"> Morris.Bar({element: 'bar-chart' ,data: [";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    strDeptName = dr["SRD_NAME"].ToString();
                    strDeptId = dr["SRD_ID"].ToString();

                    //strQuery1 = " select  STM_TYPE,SUB_YES_NO_NA,count(1) as TotCount,STM_ID from  TBL_SUB_CHKLIST " +
                    //            " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  and STM_ID = " + strDeptId +
                    //            " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
                    //            " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                    //            " where SUB_STATUS in('S','C') ";//and 

                    if (!ddlReportDept.SelectedValue.Equals(""))
                    {
                        strFilterQuery += " and SM_SRD_ID = " + ddlReportDept.SelectedValue;
                        //strQuery1 += " and SM_SRD_ID = " + ddlReportDept.SelectedValue;
                        strReportingFunction = ddlReportDept.SelectedValue;
                    }

                    if (!ddlFrequency.SelectedValue.Equals(""))
                    {
                        strFilterQuery += " and SM_FREQUENCY = '" + ddlFrequency.SelectedValue + "' ";
                        //strQuery1 += " and SM_FREQUENCY = '" + ddlFrequency.SelectedValue + "' ";
                        strFrequency = ddlFrequency.SelectedValue;
                    }

                    if (!txtFromdate.Text.Equals(""))
                    {
                        strFromDateFilter += " and SC_DUE_DATE_TO >= '" + txtFromdate.Text + "' ";
                        //strQuery1 += " and SC_DUE_DATE_FROM >= '" + txtFromdate.Text + "' ";
                        strFromDate = txtFromdate.Text;
                    }

                    if (!txtTodate.Text.Equals(""))
                    {
                        strToDateFilter += " and SC_DUE_DATE_TO <= '" + txtTodate.Text + "' ";
                        //strQuery1 += " and SC_DUE_DATE_TO <= '" + txtTodate.Text + "' ";
                        strToDate = txtTodate.Text;
                    }

                    if (!ddlPriority.SelectedValue.Equals(""))
                    {
                        strFilterQuery += " and SC_PRIORITY = '" + ddlPriority.SelectedValue + "' ";
                        //strQuery1 += " and SC_PRIORITY = '" + ddlPriority.SelectedValue + "' ";
                        strPriority = ddlPriority.SelectedValue;
                    }

                    //strQuery1 += " group by STM_TYPE,STM_ID,SUB_YES_NO_NA order by STM_TYPE ASC ";

                    //DataTable dtCount = new DataTable();
                    //SqlCommand cmd = new SqlCommand(strQuery1, myconnection);
                    //SqlDataAdapter searchDataAdaptor = new SqlDataAdapter(cmd);
                    //searchDataAdaptor.Fill(dtCount);

                    intCompliedCount = 0;
                    intNonCompliedCount = 0;
                    intNACount = 0;
                    intNotSubmitted = 0;
                    intTotalCount = 0;
                    intNotYetDue = 0;
                    intTotalSubmitted = 0;
                    intDueandNotSubmitted = 0;
                    //intCompliedTotalCount = 0;
                    //intNonCompliedTotalCount = 0;
                    //intNATotalCount = 0;
                    //intNotSubmittedTotal = 0;
                    //intOverAllCount = 0;

                    string strCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
                    " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID " +
                    " inner  join TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID and SM_SRD_ID = " + strDeptId +
                    " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID  " +
                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                    " WHERE SUB_STATUS in ('S','C') " +
                    " AND (CAST(ISNULL(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) AS DATE)) <= CAST(SC_DUE_DATE_TO AS DATE) " +
                    " AND SUB_YES_NO_NA = 'Y' ";

                    string strNonCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
                    " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID " +
                    " inner  join TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID and SM_SRD_ID = " + strDeptId +
                    " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                    " where SUB_STATUS in ('S', 'C') " +
                    //<<Commented By Amey Karangutkar on 28-Jul-2018
                    //" and (isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) > SC_DUE_DATE_TO OR SUB_YES_NO_NA = 'N') ";
                    //>>
                    //Commented and added by Rahuldeb on 10Mar2022
                    //" and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') ";
                    " and SUB_YES_NO_NA = 'N' ";
                    //>>

                    string strNACount = " select count(1) from TBL_SUB_CHKLIST " +
                    " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                    " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID and SM_SRD_ID = " + strDeptId +
                    " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID  " +
                    " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                    " where SUB_STATUS in ('S','C') and SUB_YES_NO_NA = 'NA' ";

                    string strQDueandNotSubmitted = " select Count(1) from TBL_SUB_CHKLIST " +
                    " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                    " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                    " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                    " where SM_SRD_ID = " + strDeptId +
                    //" and SC_DUE_DATE_TO <= current_timestamp AND isnull(SUB_STATUS,'') = ''" +
                    " AND isnull(SUB_STATUS,'') = ''";

                    //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                    if (string.IsNullOrEmpty(txtTodate.Text))
                    {
                        strQDueandNotSubmitted += " AND CAST(SC_DUE_DATE_TO AS DATE) <= CAST(CURRENT_TIMESTAMP AS DATE) ";
                    }
                    else
                    {
                        strQDueandNotSubmitted += " AND CAST(SC_DUE_DATE_TO AS DATE) <= CAST('" + txtTodate.Text + "' AS DATE) ";
                    }
                    //>>

                    string strNotYetDue = " select Count(1) from TBL_SUB_CHKLIST " +
                    " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                    " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                    " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                    " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                    //" where SM_STM_ID = " + strDeptId + " and SC_DUE_DATE_TO > current_timestamp" + strFilterQuery;
                    " where SM_SRD_ID = " + strDeptId;

                    //<<Modified by Ankur Tyagi on 25Nov2023 for CR_894
                    if (string.IsNullOrEmpty(txtTodate.Text))
                    {
                        strNotYetDue += " AND CAST(SC_DUE_DATE_TO AS DATE) > CAST(CURRENT_TIMESTAMP AS DATE) and isnull(SUB_STATUS, '') = '' ";
                    }
                    else
                    {
                        strNotYetDue += " AND CAST(SC_DUE_DATE_TO AS DATE) > CAST('" + txtTodate.Text + "' AS DATE) and isnull(SUB_STATUS, '') = '' ";
                    }
                    //>>

                    //<<Added by Rahuldeb on 10Mar2022
                    string strDelayedFilings = " select count(1) from TBL_SUB_CHKLIST " +
                    " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
                    " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID and SM_SRD_ID = " + strDeptId +
                    " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID  " +
                    " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
                    " where SUB_STATUS in ('S','C') " +
                    " and CAST(ISNULL(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE) AS DATE) > CAST(SC_DUE_DATE_TO AS DATE) and SUB_YES_NO_NA = 'Y' ";
                    //>>

                    string strQRopnSR = " SELECT Count(*) " +
                                        " FROM TBL_SUB_CHKLIST INNER JOIN TBL_SUBMISSIONS_MAS ON SC_SM_ID = SM_ID " +
                                        " INNER JOIN TBL_SUB_REPORTING_DEPT on  SRD_ID=SM_SRD_ID " +
                                        " INNER JOIN TBL_SUBMISSIONS ON SUB_SC_ID = SC_ID where ISNULL(SUB_STATUS,'') in ('R','SR')" +
                                        " AND SM_SRD_ID = " + strDeptId;

                    string strQTotal = " select Count(*) from TBL_SUB_CHKLIST " +
                                        " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
                                        " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
                                        " INNER JOIN TBL_SUB_REPORTING_DEPT on SRD_ID = SM_SRD_ID " +
                                        " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
                                        //" where SM_STM_ID = " + strDeptId + " " + strFilterQuery + " and isnull(SUB_STATUS,'') != 'R' ";
                                        " where SM_SRD_ID = " + strDeptId;

                    intCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strCompliedCount + strFilterQuery + strFromDateFilter + strToDateFilter));
                    intNonCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strNonCompliedCount + strFilterQuery + strFromDateFilter + strToDateFilter));
                    intNACount = Convert.ToInt32(DataServer.ExecuteScalar(strNACount + strFilterQuery + strFromDateFilter + strToDateFilter));
                    //intTotalSubmitted = Convert.ToInt32(DataServer.ExecuteScalar(strTotalSubmitted));
                    intNotYetDue = Convert.ToInt32(DataServer.ExecuteScalar(strNotYetDue + strFilterQuery));
                    intDueandNotSubmitted = Convert.ToInt32(DataServer.ExecuteScalar(strQDueandNotSubmitted + strFilterQuery + strFromDateFilter + strToDateFilter));
                    int intRopnSR = Convert.ToInt32(DataServer.ExecuteScalar(strQRopnSR + strFilterQuery + strFromDateFilter + strToDateFilter));
                    intTotalCount = Convert.ToInt32(DataServer.ExecuteScalar(strQTotal + strFilterQuery + strFromDateFilter + strToDateFilter));
                    //<<Added by Rahuldeb on 10Mar2022
                    intDelayedCount = Convert.ToInt32(DataServer.ExecuteScalar(strDelayedFilings + strFilterQuery + strFromDateFilter + strToDateFilter));
                    //>>
                    intOverAllCount = intOverAllCount + intTotalCount;
                    //>>
                    strChart += "{x: '" + strDeptName + "', DC: '" + intDelayedCount + "', Y: " + intCompliedCount + " , N: " + intNonCompliedCount + " ," +
                                " NA: " + intNACount + " ,  ND: " + intNotYetDue + ", DN: " + intDueandNotSubmitted + ", SR:" + intRopnSR + "},";

                    sbHtmlTable.Append("<tr><td class= 'DBTableCellLeft'><center>" + strDeptName + "</center></td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=1A&Status=DC&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                    "&SRDID=" + strDeptId + "&STMID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    intDelayedCount +
                                    "</a>" +
                                    "</td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=1A&Status=Y&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                    "&SRDID=" + strDeptId + "&STMID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    intCompliedCount +
                                    "</a>" +
                                    "</td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                    "<a href='#'class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=1A&Status=N&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                    "&SRDID=" + strDeptId + "&STMID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    intNonCompliedCount +
                                    "</a>" +
                                    "</td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                    "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                    "'DetailedReport.aspx?ReportType=1A&Status=NA&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                    "&SRDID=" + strDeptId + "&STMID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                    +intNACount +
                                    "</a>" +
                                    "</td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                  "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                  "'DetailedReport.aspx?ReportType=1A&Status=DN&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                  "&SRDID=" + strDeptId + "&STMID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                  "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                  intDueandNotSubmitted +
                                  "</a>" +
                                  "</td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                   "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                   "'DetailedReport.aspx?ReportType=1A&Status=ND&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                   "&SRDID=" + strDeptId + "&STMID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                   "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                   intNotYetDue +
                                   "</a>" +
                                   "</td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                   "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                   "'DetailedReport.aspx?ReportType=1A&Status=SR&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                   "&SRDID=" + strDeptId + "&STMID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                   "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">"
                                   + intRopnSR +
                                   "</a>" +
                                   "</td>");

                    sbHtmlTable.Append("<td class= 'DBTableCellRight'>" +
                                   "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                   "'DetailedReport.aspx?ReportType=1A&FromDate=" + strFromDate + "&ToDate=" + strToDate + "&DeptName=" + strDeptName +
                                   "&SRDID=" + strDeptId + "&STMID=" + strReportingFunction + "&Frequency=" + strFrequency + "&Priority=" + strPriority + "'," +
                                   "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
                                   +intTotalCount +
                                   "</a>" +
                                   "</td></tr>");

                }


                sbHtmlTable.Append("</table>");
                litDeptWiseBarchart.Text = sbHtmlTable.ToString();
                strChart = strChart.Substring(0, strChart.Length - 1);
                strChart += " ], xkey: 'x', ykeys: ['DC','Y','N','NA','ND','DN','SR'], " +
                            " labels: ['Delayed Filings','Compliant','Not Compliant','Not Applicable','Not Yet Due','Due but not Submitted','Reopened / Sent back']," +
                            " barColors: ['#C45850','#009933','Red','#267DD4','#65C3A8','#8F002C','#508ec4'], xLabelAngle: -45, overflow:'visible',resize: true});";//Rahuldeb on 18Jan2020 Changed #8e5ea2 to red & #3e95cd changed to #009933
                strChart += "</script>";
                litChart.Text = strChart;
            }
        }

        #endregion

        private void writeError(string strMsg)
        {
            lblMsg.Visible = true;
            lblMsg.Text = strMsg;
        }
        private void pnlHideShow(string strPnl)
        {
            if (strPnl == "pnlComplianceStatus")
            {
                pnlComplianceStatus.Visible = true;
                pnlDeptWiseBarchart.Visible = false;
                pnlMonthlyCompliance.Visible = false;
                pnlRegulatoryreportingPrevmonth.Visible = false;
                lblCurrMnth.Visible = false;
            }
            else if (strPnl == "pnlDeptWiseBarchart")
            {
                pnlComplianceStatus.Visible = false;
                pnlDeptWiseBarchart.Visible = true;
                pnlMonthlyCompliance.Visible = false;
                pnlRegulatoryreportingPrevmonth.Visible = false;
                lblCurrMnth.Visible = false;
            }
            else if (strPnl == "pnlMonthlyCompliance")
            {
                pnlComplianceStatus.Visible = false;
                pnlDeptWiseBarchart.Visible = false;
                pnlMonthlyCompliance.Visible = true;
                pnlRegulatoryreportingPrevmonth.Visible = false;
                lblCurrMnth.Visible = false;
            }
            else if (strPnl == "pnlRegulatoryreportingPrevmonth")
            {
                pnlComplianceStatus.Visible = false;
                pnlDeptWiseBarchart.Visible = false;
                pnlMonthlyCompliance.Visible = false;
                pnlRegulatoryreportingPrevmonth.Visible = true;
                lblCurrMnth.Visible = true;
            }
        }

        //<<Added by Rahuldeb on 09Mar2018
        private string getMonthName(string strMonth)
        {
            string strMonthName = "";

            switch (strMonth)
            {
                case "1":
                    strMonthName = "Jan";
                    break;
                case "2":
                    strMonthName = "Feb";
                    break;
                case "3":
                    strMonthName = "Mar";
                    break;
                case "4":
                    strMonthName = "Apr";
                    break;
                case "5":
                    strMonthName = "May";
                    break;
                case "6":
                    strMonthName = "Jun";
                    break;
                case "7":
                    strMonthName = "Jul";
                    break;
                case "8":
                    strMonthName = "Aug";
                    break;
                case "9":
                    strMonthName = "Sep";
                    break;
                case "10":
                    strMonthName = "Oct";
                    break;
                case "11":
                    strMonthName = "Nov";
                    break;
                case "12":
                    strMonthName = "Dec";
                    break;
            }
            return strMonthName;
        }
        //>>
        protected void lnkReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl, false);
        }
    }
}