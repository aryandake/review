using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_CertificationDashboard : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CertificationMasterBL certBL = new CertificationMasterBL();
        //string strTableFirstCellLeft = " style =\" font-size: 12px; font-family: Trebuchet MS; " +
        //                     " background-color: #ded0b0; border-width: 1px; padding: 8px;" +
        //                     " text-decoration:none;  " +
        //                     " border-style: solid; border-color: #bcaf91; text-align: center;\"";

        //string strTableCellLeft = " style =\" font-size: 12px; font-family: Trebuchet MS; font-weight:bold; " +
        //                        " border-width: 1px; padding: 8px; border-right: solid 0.5pt #DDDDDD; " +
        //                        " text-decoration:none; border-bottom: solid 0.5pt #DDDDDD;  " +
        //                        " border-left: solid 0.5pt #DDDDDD; text-align: center; \"";

        //string strTableCellRight = "style =\" font: 11px Trebuchet MS; text-align:center; padding: 0in 5.4pt 0in 5.4pt; " +
        //                        " border-right: solid 0.5pt #DDDDDD; border-bottom: solid 0.5pt #DDDDDD; \"";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ListItem li = new ListItem();
                li.Text = "(Select)";
                li.Value = "";

                ddlQuarter.DataSource = utilBL.getDataset("CERTIFICATEQUARTER", strConnectionString);
                ddlQuarter.DataBind();
                ddlQuarter.Items.Insert(0, li);

                ddlType.Attributes["Onchange"] = " return showReportParam()";
                btnSearch.Attributes["onclick"] = " return validateParams('" + ddlType.ClientID + "','" + ddlDeptType.ClientID + "', '"
                    + ddlQuarter.ClientID + "')";
            }
            else
            {
                string script = "\r\n<script language=\"javascript\">\r\n" +
                           " showReportParam();" +
                           "   </script>\r\n";

                ClientScript.RegisterStartupScript(this.GetType(), "script", script);
            }

        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            if (ddlType.SelectedValue.Equals("1"))
            {
                getStatusWiseReports();
            }
            else if (ddlType.SelectedValue.Equals("2"))
            {
                getCountWiseReports();
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
                      + "<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'>" +
                      "<tr><td class='DBTableTopHeader'>Certification count wise reports</td></tr>" +
                      "</table><br/>";

                strHTMLReport = strHTMLReport + "<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'><tr>" +

                    "<td class='DBTableFirstCellRight'>Function/Unit/Sub-Unit Name</td>" +

                    "<td class='DBTableFirstCellRight'>Quarter Name</td>" +

                    "<td class='DBTableFirstCellRight'>Compliant</td>" +

                    "<td class='DBTableFirstCellRight'>Not Compliant</td>" +

                    "<td class='DBTableFirstCellRight'>Not yet applicable</td>" +

                    "<td class='DBTableFirstCellRight'>Work in progress</td>" +

                    "<td class='DBTableFirstCellRight'>Total</td>" +

                    "</tr>";
                //
                if (ddlDeptType.SelectedValue.Equals("1"))
                {
                    strDeptSQL = "SELECT CDM_ID as DeptId, CDM_NAME as DeptName FROM TBL_CERT_DEPT_MAS where isnull(CDM_IS_JOIN_CERTIFICATE,'') != 'Yes' ";
                }
                else if (ddlDeptType.SelectedValue.Equals("2"))
                {
                    strDeptSQL = "SELECT CSDM_ID as DeptId, CSDM_NAME + ' - ' + CDM_NAME as DeptName " +
                                 " FROM  TBL_CERT_SUB_DEPT_MAS " +
                                 " inner join TBL_CERT_DEPT_MAS on CSDM_CDM_ID = CDM_ID ";
                }

                else if (ddlDeptType.SelectedValue.Equals("3"))
                {
                    strDeptSQL = "SELECT CSSDM_ID as DeptId, CDM_NAME + ' - ' + CSDM_NAME as DeptName " +
                                " FROM  TBL_CERT_SUB_SUB_DEPT_MAS " +
                                " inner join TBL_CERT_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID " +
                                " inner join TBL_CERT_DEPT_MAS on CSDM_CDM_ID = CDM_ID ";
                }

                strQuartersSQL = "SELECT CQM_ID, replace(convert(varchar, CQM_FROM_DATE, 106), ' ', '-') + ' to ' + " +
                                " replace(convert(varchar, CQM_TO_DATE, 106), ' ', '-') as Quarter " +
                                " FROM TBL_CERT_QUARTER_MAS ";

                strSelectedQuarterId = ddlQuarter.SelectedValue.ToString();

                if (!strSelectedQuarterId.Equals(""))
                {
                    strQuartersSQL = strQuartersSQL + " where CQM_ID = " + strSelectedQuarterId;
                }
                strQuartersSQL = strQuartersSQL + " order by CQM_ID";
                using(F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = strDeptSQL;
                    DB.F2FDataAdapter.Fill(dtCertifyingDepts);
                    DB.F2FCommand.CommandText = strQuartersSQL;
                    DB.F2FDataAdapter.Fill(dtQuarters);
                }
                //cmdDept = new SqlCommand(strDeptSQL, myconnection);
                //adDept = new SqlDataAdapter(cmdDept);
                //dtCertifyingDepts = new DataTable();
                //adDept.Fill(dtCertifyingDepts);

                //cmdQuarters = new SqlCommand(strQuartersSQL, myconnection);
                //adQuarters = new SqlDataAdapter(cmdQuarters);
                //dtQuarters = new DataTable();
                //adQuarters.Fill(dtQuarters);

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
                                " and (CCD_YES_NO_NA IS NOT NULL OR CCD_YES_NO_NA != '')" +
                                " inner join TBL_CERT_MAS ON CERTM_ID = CERT_CERTM_ID " +
                                " inner join TBL_CERT_SUB_SUB_DEPT_MAS ON CERTM_DEPT_ID = CSSDM_ID " +
                                " inner join TBL_CERT_SUB_DEPT_MAS ON CSSDM_CSDM_ID = CSDM_ID " +
                                " inner join TBL_CERT_DEPT_MAS ON CSDM_CDM_ID = CDM_ID ";

                        if (ddlDeptType.SelectedValue.Equals("1"))
                        {
                            strSubQuery = " and  CDM_ID  = " + strDeptId;
                        }
                        else if (ddlDeptType.SelectedValue.Equals("2"))
                        {
                            strSubQuery = " and  CSDM_ID = " + strDeptId;

                        }
                        else if (ddlDeptType.SelectedValue.Equals("3"))
                        {
                            strSubQuery = " and CSSDM_ID  = " + strDeptId;
                        }

                        using(F2FDatabase DB = new F2FDatabase())
                        {
                            dt1 = new DataTable();
                            DB.F2FCommand.CommandText = strQry1 + strSubQuery;
                            DB.F2FDataAdapter.Fill(dt1);

                            for (int intCnt = 0; intCnt < dt1.Rows.Count; intCnt++)
                            {
                                dr1 = dt1.Rows[intCnt];

                                intCompliant = Convert.ToInt32(dr1["Count"]);
                                intTotalCompliant = intTotalCompliant + intCompliant;
                            }

                            dt2 = new DataTable();
                            DB.F2FCommand.CommandText = strQry2 + strSubQuery;
                            DB.F2FDataAdapter.Fill(dt2);
                            for (int intCnt = 0; intCnt < dt2.Rows.Count; intCnt++)
                            {
                                dr2 = dt2.Rows[intCnt];

                                intNonCompliant = Convert.ToInt32(dr2["Count"]);
                                intTotalNonCompliant = intTotalNonCompliant + intNonCompliant;
                            }

                            dt3 = new DataTable();
                            DB.F2FCommand.CommandText = strQry3 + strSubQuery;
                            DB.F2FDataAdapter.Fill(dt3);
                            for (int intCnt = 0; intCnt < dt3.Rows.Count; intCnt++)
                            {
                                dr3 = dt3.Rows[intCnt];

                                intNotYetApplicable = Convert.ToInt32(dr3["Count"]);
                                intTotalNotYetApplicable = intTotalNotYetApplicable + intNotYetApplicable;
                            }

                            dt4 = new DataTable();
                            DB.F2FCommand.CommandText = strQry4 + strSubQuery;
                            DB.F2FDataAdapter.Fill(dt4);
                            for (int intCnt = 0; intCnt < dt4.Rows.Count; intCnt++)
                            {
                                dr4 = dt4.Rows[intCnt];

                                intWorkInProgress = Convert.ToInt32(dr4["Count"]);
                                intTotalWorkInProgress = intTotalWorkInProgress + intWorkInProgress;
                            }

                            dt5 = new DataTable();
                            DB.F2FCommand.CommandText = strQry5 + strSubQuery;
                            DB.F2FDataAdapter.Fill(dt5);
                            for (int intCnt = 0; intCnt < dt5.Rows.Count; intCnt++)
                            {
                                dr5 = dt5.Rows[intCnt];

                                intRowWiseTotal = Convert.ToInt32(dr5["Count"]);
                                intGrandTotal = intGrandTotal + intRowWiseTotal;
                            }
                        }
                        
                        strHTMLReport = strHTMLReport +
                            "<tr><td class='DBTableCellLeft'>" + strDeptName + "</td>" +
                            "<td class='DBTableCellRight'>" + strQuarter + "</td>" +
                            "<td class='DBTableCellRight'>" +
                            "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                            "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=" + ddlDeptType.SelectedValue + "&QtrId=" + strQuarterId +
                            "&DeptId=" + strDeptId + "&Type=C'," +
                            "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750,resizable=1');return false;\">" +
                            intCompliant + "</a>" +
                            "</td>" +

                            "<td class='DBTableCellRight'>" +
                            "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                            "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=" + ddlDeptType.SelectedValue + "&QtrId=" + strQuarterId +
                            "&DeptId=" + strDeptId + "&Type=N'," +
                            "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750,resizable=1');return false;\">" +
                            intNonCompliant + "</a>" +
                            "</td>" +

                            "<td class='DBTableCellRight'>" +
                            "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                            "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=" + ddlDeptType.SelectedValue + "&QtrId=" + strQuarterId +
                            "&DeptId=" + strDeptId + "&Type=NA'," +
                            "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750 ,resizable=1');return false;\">" +
                            intNotYetApplicable + "</a>" +
                            "</td>" +

                            "<td class='DBTableCellRight'>" +
                            "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                            "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=" + ddlDeptType.SelectedValue + "&QtrId=" + strQuarterId +
                            "&DeptId=" + strDeptId + "&Type=W'," +
                            "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750 ,resizable=1');return false;\">" +
                            intWorkInProgress + "</a>" +
                            "</td>" +

                            "<td class='DBTableCellRight'>" +
                            "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                            "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=" + ddlDeptType.SelectedValue + "&QtrId=" + strQuarterId +
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
                        "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                        "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=" + ddlDeptType.SelectedValue + strQuarterIdForTotalRow +
                        "&DeptId=0" + "&Type=C'," +
                        "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750 ,resizable=1');return false;\">" +
                        intTotalCompliant + "</a>" +
                        "</td>" +

                        "<td class='DBTableCellRight'>" +
                        "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                                                //"'CertDashboardDets.aspx?DeptName="+strDeptName + "&ReportType=" + ddlDeptType.SelectedValue + "&QtrId=0" +
                                                "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=" + ddlDeptType.SelectedValue + strQuarterIdForTotalRow +
                        "&DeptId=0" + "&Type=N'," +
                        "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750 ,resizable=1');return false;\">" +
                        intTotalNonCompliant + "</a>" +
                        "</td>" +

                        "<td class='DBTableCellRight'>" +
                        "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                        "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=" + ddlDeptType.SelectedValue + strQuarterIdForTotalRow +
                        "&DeptId=0" + "&Type=NA'," +
                        "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750,resizable=1');return false;\">" +
                        intTotalNotYetApplicable + "</a>" +
                        "</td>" +

                        "<td class='DBTableCellRight'>" +
                        "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                        "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=" + ddlDeptType.SelectedValue + strQuarterIdForTotalRow +
                        "&DeptId=0" + "&Type=W'," +
                        "'FILE', 'location=0,status=0,scrollbars=1,width=1000,height=750 ,resizable=1');return false;\">" +
                        intTotalWorkInProgress + "</a>" +
                        "</td>" +

                        "<td class='DBTableCellRight'>" +
                        "<a href='#' class='badge rounded-pill badge-soft-pink' onclick=\"window.open(" +
                        "'CertDashboardDets.aspx?DeptName=" + strDeptName + "&ReportType=" + ddlDeptType.SelectedValue + strQuarterIdForTotalRow +
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

        private void getStatusWiseReports()
        {
            string strHTMLReport = "";
            DataTable dtCertifyingDepts = new DataTable();
            DataTable dtQuarters = new DataTable();

            DataRow dr2;
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            
            string strQry1;
            
            try
            {

                strHTMLReport = strHTMLReport
                                 + "<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'>" +
                                 "<tr><td class='DBTableFirstCellRight'>Certification Status wise reports</tr>" +
                                 "</table><br/>";

                strHTMLReport = strHTMLReport + "<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'><tr>" +

                    "<td class='DBTableFirstCellRight'>Sr.No.</td>" +

                    "<td class='DBTableFirstCellRight'>Department Name</td>" +

                     "<td class='DBTableFirstCellRight'>Quarter</td>" +

                    "<td class='DBTableFirstCellRight'>Status</td>" +

                    "<td class='DBTableFirstCellRight'>Pending With Whom</td>" +

                    "</tr>";


                strQry1 = " select CSSDM_ID, CDM_NAME + ' - ' + CSDM_NAME as DeptName ,CSM_DESC, " +
                          " [dbo].[getPendingWithWhomCertification](CERT_STATUS, CSSDM_ID) as PendingwithWhom, " +
                          " CERT_ID,  CERT_STATUS  as [Status],  REPLACE(CONVERT(VARCHAR, CQM_FROM_DATE, 106), ' ', '-') " +
                          " +' to ' + REPLACE(CONVERT(VARCHAR, CQM_TO_DATE, 106), ' ', '-')  as Quarter , " +
                          " TBL_CERT_SUB_SUB_DEPT_MAS.* " +
                          " from TBL_CERTIFICATIONS  " +
                          " inner join TBL_CERT_MAS ON CERTM_ID = [CERT_CERTM_ID] and CERTM_LEVEL_ID=0" +
                          " inner join TBL_CERT_SUB_SUB_DEPT_MAS ON CERTM_DEPT_ID = CSSDM_ID " +
                          " inner join TBL_CERT_SUB_DEPT_MAS ON CSSDM_CSDM_ID = CSDM_ID " +
                          " inner join TBL_CERT_DEPT_MAS ON CSDM_CDM_ID = CDM_ID " +
                          " inner join TBL_CERT_QUARTER_MAS on CQM_ID = CERT_CQM_ID " +
                          " and CQM_ID = " + ddlQuarter.SelectedValue.ToString() +
                          " inner join TBL_CERT_STATUS_MAS on CERT_STATUS = CSM_NAME  order by DeptName ";
                dt1 = new DataTable();
                using (F2FDatabase DB = new F2FDatabase(strQry1))
                {
                    DB.F2FDataAdapter.Fill(dt1);
                }
                for (int intCnt = 0; intCnt < dt1.Rows.Count; intCnt++)
                {
                    dr2 = dt1.Rows[intCnt];

                    strHTMLReport = strHTMLReport +
                        "<tr>" +
                        "<td class='DBTableCellLeft'>" + (intCnt + 1) + "</td>" +
                        "<td class='DBTableCellRight'>" + dr2["DeptName"].ToString() + "</td>" +
                        "<td class='DBTableCellRight'>" + dr2["Quarter"].ToString() + "</td>" +
                        "<td class='DBTableCellRight'>" + dr2["CSM_DESC"].ToString() + "</td>" +
                        //"<td class='DBTableCellRight'>" + dr2["PendingwithWhom"].ToString() + "</td>" +
                        "<td class='DBTableCellRight'>" + ((dr2["PendingwithWhom"].ToString() == "") || (dr2["PendingwithWhom"].ToString() == null) ? " " : dr2["PendingwithWhom"].ToString()) + "</td>" +
                        "</tr>";



                }

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


        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            string strHTMLReport = Session["CertDashboard"].ToString();
            string attachment = "attachment; filename=Details.xls";
            string style = @"<style> TABLE { border: 1px solid black; } TD { border: 1px solid black; } </style> ";

            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write(style);
            Response.Write(strHTMLReport.ToString());
            Response.End();
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }
    }
}