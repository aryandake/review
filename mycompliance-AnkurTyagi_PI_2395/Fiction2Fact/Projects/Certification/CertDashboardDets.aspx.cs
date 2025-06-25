using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    //[Idunno.AntiCsrf.SuppressCsrfCheck]
    public partial class Certification_CertDashboardDets : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CertificationMasterBL certBL = new CertificationMasterBL();
        //string strTableFirstCellLeft = " style =\" font-size: 12px; font-family: Trebuchet MS; " +
        //                    " background-color: #ded0b0; border-width: 1px; padding: 8px;" +
        //                    " text-decoration:none;  " +
        //                    " border-style: solid; border-color: #bcaf91; text-align: center;\"";

        //string strTableCellLeft = " style =\" font-size: 12px; font-family: Trebuchet MS; font-weight:bold; " +
        //                        " border-width: 1px; padding: 8px; border-right: solid 0.5pt #DDDDDD; " +
        //                        " text-decoration:none; border-bottom: solid 0.5pt #DDDDDD;  " +
        //                        " border-left: solid 0.5pt #DDDDDD; text-align: center; \"";

        //string strTableCellRight = "style =\" font: 11px Trebuchet MS; text-align:center; padding: 0in 5.4pt 0in 5.4pt; " +
        //                        " border-right: solid 0.5pt #DDDDDD; border-bottom: solid 0.5pt #DDDDDD; \"";


        string strTitleOfSection;
        string strSection;
        string strTimeLimit;
        string strParticulars;
        string strPenalty;
        string strCheckpoint;
        string strTypeReport = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //<<Added by Ankur Tyagi on 19Mar2024 for CR_1991
                if (Page.User.Identity.Name.Equals(""))
                {
                    Response.Redirect(Global.site_url("Login.aspx"));
                    return;
                }
                //>>
                if (!Page.IsPostBack)
                {
                    showDashboardDetails();
                }
            }
            //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
            //>>
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page != null)
            {
                Page.RegisterRequiresViewStateEncryption();
            }
        }


        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            showDashboardDetails();
        }

        private void showDashboardDetails()
        {
            string strHTMLReport = "";
            DataTable dtCertifyingDepts = new DataTable();
            DataTable dtQuarters = new DataTable();

            DataRow dr1;
            DataTable dt1 = new DataTable();


            string strQry1;

            string strDeptId = "", strQuarterId = "", strQuarterIdSQL = "", strDeptName = "", strReportType = "";
            string strTypeSQL = "";
            string strSubQuery = "";// strQuarter = "",
                                    // string strDate = "";
            string strType = "";
            if (Request.QueryString["QtrId"] != null)
            {
                strQuarterId = Request.QueryString["QtrId"].ToString();
                if (!strQuarterId.Equals("0"))
                    strQuarterIdSQL = " and CERT_CQM_ID = " + strQuarterId;
            }

            if (Request.QueryString["Type"] != null)
            {
                strType = Request.QueryString["Type"].ToString();
                strTypeReport = strType.ToString();
                if (!strType.Equals("RWT") && !strType.Equals("GrandTotal"))
                    strTypeSQL = " and CCD_YES_NO_NA = '" + strType + "'";
            }

            try
            {
                strHTMLReport = strHTMLReport
                      + "<table  width='100%' class='table table-bordered footable' cellpadding='0' cellspacing='0'>" +
                      "<tr><td class='DBTableTopHeader'>Certification count wise reports</td></tr>" +
                      "</table><br/>";

                strHTMLReport = strHTMLReport + "<table width='100%' class='table table-bordered footable' ><tr>" + "<td class='DBTableFirstCellLeft' >Department</td>" +
                    "<td class='DBTableFirstCellLeft' >Quarter Name</td>" +

                    " <td class='DBTableFirstCellLeft'>Reference Circular/Notification/Act</td> " +
                    " <td class='DBTableFirstCellLeft'>Section/Clause</td> " +
                    " <td class='DBTableFirstCellLeft'>Compliance of/Heading of Compliance checklist</td> " +
                    " <td class='DBTableFirstCellLeft'>Description</td> " +
                    " <td class='DBTableFirstCellLeft'>Consequences of non Compliance</td> " +
                    " <td class='DBTableFirstCellLeft'>Frequency</td> " +
                    "<td class='DBTableFirstCellLeft'>Compliance Status</td>" +
                    "<td class='DBTableFirstCellLeft'>Effective From</td>" +
                    //"<td class='DBTableFirstCellLeft'>Status</td>" +
                    "<td class='DBTableFirstCellLeft'>Remarks</td>" +
                    "</tr>";

                // strQry1 = "select CSSDM_NAME + ' - ' + CSDM_NAME + ' - ' + CDM_NAME as DeptName, " +
                strQry1 = "select CDM_NAME + ' - ' + CSDM_NAME as DeptName, " +
                  " *,Replace(CONVERT(VARCHAR(11), CCM_EFFECTIVE_FROM, 106), ' ', '-') as [Effective From] , " +
                  "CASE WHEN CCD_YES_NO_NA = 'C'  THEN 'Compliant' " +
                  "WHEN CCD_YES_NO_NA = 'N'  THEN 'Not Compliant' " +
                  "WHEN CCD_YES_NO_NA = 'N'  THEN 'Not Compliant' " +
                  "WHEN CCD_YES_NO_NA = 'NA'  THEN 'Not yet applicable' " +
                  "WHEN CCD_YES_NO_NA = 'W'  THEN 'Work in progress' " +
                  "else ' ' END as 'Compliance Status',convert(varchar, CQM_FROM_DATE, 106) + ' to ' + " +
                  " convert(varchar, CQM_TO_DATE, 106) as Quarter " +
                  " from TBL_CERT_CHECKLIST_DETS " +
                  " INNER JOIN TBL_CERTIFICATIONS ON CCD_CERT_ID = CERT_ID " +
                  " inner join TBL_CERT_QUARTER_MAS ON CQM_ID = CERT_CQM_ID " + strQuarterIdSQL +
                  strTypeSQL +
                  " inner join TBL_CERT_CHECKLIST_MAS on CCD_CCM_ID = CCM_ID" +
                  " inner join TBL_CERT_MAS ON CERTM_ID = CERT_CERTM_ID " +
                  " inner join TBL_CERT_SUB_SUB_DEPT_MAS ON CERTM_DEPT_ID = CSSDM_ID " +
                  " inner join TBL_CERT_SUB_DEPT_MAS ON CSSDM_CSDM_ID = CSDM_ID " +
                  " inner join TBL_CERT_DEPT_MAS ON CSDM_CDM_ID = CDM_ID ";

                if (Request.QueryString["ReportType"] != null)
                {
                    strReportType = Request.QueryString["ReportType"].ToString();

                    if (Request.QueryString["DeptId"] != null)
                        strDeptId = Request.QueryString["DeptId"].ToString();
                    else
                        strDeptId = "0";

                    if (Request.QueryString["DeptName"] != null)
                        strDeptName = Request.QueryString["DeptName"].ToString();
                    else
                        strDeptName = "";

                    if (!strDeptId.Equals("0"))
                    {
                        if (strReportType.Equals("1"))
                        {
                            strSubQuery = " and CDM_ID  = " + strDeptId;
                        }
                        else if (strReportType.Equals("2"))
                        {
                            strSubQuery = " and CSDM_ID = " + strDeptId;
                        }
                        else if (strReportType.Equals("3"))
                        {
                            strSubQuery = " and CSSDM_ID = " + strDeptId;
                        }
                    }
                }
                using (F2FDatabase Db = new F2FDatabase(strQry1 + strSubQuery))
                {
                    Db.F2FDataAdapter.Fill(dt1);
                }
                Session["AllCehecklistDetails"] = dt1;
                for (int intCnt = 0; intCnt < dt1.Rows.Count; intCnt++)
                {
                    dr1 = dt1.Rows[intCnt];
                    if (strDeptId.Equals("0"))
                    {
                        strTitleOfSection = dr1["CCM_CLAUSE"].ToString().Length > 100 ? (dr1["CCM_CLAUSE"].ToString()).Substring(0, 100).Replace("\n", "<br />") + " ..." : (dr1["CCM_CLAUSE"].ToString()).Replace("\n", "<br />");
                        strSection = dr1["CCM_REFERENCE"].ToString().Length > 100 ? (dr1["CCM_REFERENCE"].ToString()).Substring(0, 100).Replace("\n", "<br />") + " ..." : (dr1["CCM_REFERENCE"].ToString()).Replace("\n", "<br />");
                        strTimeLimit = dr1["CCM_FREQUENCY"].ToString().Length > 100 ? (dr1["CCM_FREQUENCY"].ToString()).Substring(0, 100).Replace("\n", "<br />") + " ..." : (dr1["CCM_FREQUENCY"].ToString()).Replace("\n", "<br />");
                        strParticulars = dr1["CCM_PARTICULARS"].ToString().Length > 100 ? (dr1["CCM_PARTICULARS"].ToString()).Substring(0, 100).Replace("\n", "<br />") + " ..." : (dr1["CCM_PARTICULARS"].ToString()).Replace("\n", "<br />");
                        strPenalty = dr1["CCM_PENALTY"].ToString().Length > 100 ? (dr1["CCM_PENALTY"].ToString()).Substring(0, 100).Replace("\n", "<br />") + " ..." : (dr1["CCM_PENALTY"].ToString()).Replace("\n", "<br />");
                        strCheckpoint = dr1["CCM_CHECK_POINTS"].ToString().Length > 100 ? (dr1["CCM_CHECK_POINTS"].ToString()).Substring(0, 100).Replace("\n", "<br />") + " ..." : (dr1["CCM_CHECK_POINTS"].ToString()).Replace("\n", "<br />");


                        strHTMLReport = strHTMLReport +
                    "<tr><td class='DBTableCellLeft'>" + dr1["DeptName"].ToString() + "</td>" +
                    "<td class='DBTableCellRight'>" + dr1["Quarter"].ToString() + "</td>" +

                    "<td class='DBTableCellRight'>" +
                        strSection +
                    "</td>" +

                    "<td class='DBTableCellRight'>" +
                    // start - added by Hari on 3 oct 2016
                    "<a href='#' onclick=\"window.open(" +
                            "'ViewChecklistData.aspx?ChecklistId=" + dr1["CCD_ID"].ToString() +
                            "&Type=NA'," +
                            "'FILE1', 'location=0,status=0,scrollbars=1,width=700,height=500,resizable=1');return false;\">" +
                            strTitleOfSection + "</a>" +
                    //end - added by Hari on 3 oct 2016            
                    //   + dr1["CCM_CLAUSE"].ToString() + commented by Hari on 3 oct 2016

                    "</td>" +

                    "<td class='DBTableCellRight'>" +
                        strCheckpoint +
                    "</td>" +

                    "<td class='DBTableCellRight'>" +
                        strParticulars +
                    "</td>" +

                    "<td class='DBTableCellRight'>" +
                        strPenalty +
                    "</td>" +

                    "<td class='DBTableCellRight'>" +
                        strTimeLimit +
                    "</td>" +

                    "<td class='DBTableCellRight'>" +
                        strTimeLimit +
                    "</td>" +

                        "<td class='DBTableCellRight'>" +
                        dr1["Compliance Status"].ToString() +
                    "</td>" +

                    "<td class='DBTableCellRight'>" +
                        dr1["Effective From"].ToString() +
                    "</td>" +

                    //"<td class='DBTableCellRight'>" +
                    //    dr1["CCM_STATUS"].ToString() +
                    //"</td>" +

                    "<td class='DBTableCellRight'>" +
                        dr1["CCD_REMARKS"].ToString() +
                    "</td>" +

                    "</tr>";

                    }
                    else
                    {
                        strTitleOfSection = dr1["CCM_CLAUSE"].ToString().Length > 100 ? (dr1["CCM_CLAUSE"].ToString()).Substring(0, 100).Replace("\n", "<br />") + " ..." : (dr1["CCM_CLAUSE"].ToString()).Replace("\n", "<br />");
                        strSection = dr1["CCM_REFERENCE"].ToString().Length > 100 ? (dr1["CCM_REFERENCE"].ToString()).Substring(0, 100).Replace("\n", "<br />") + " ..." : (dr1["CCM_REFERENCE"].ToString()).Replace("\n", "<br />");
                        strTimeLimit = dr1["CCM_FREQUENCY"].ToString().Length > 100 ? (dr1["CCM_FREQUENCY"].ToString()).Substring(0, 100).Replace("\n", "<br />") + " ..." : (dr1["CCM_FREQUENCY"].ToString()).Replace("\n", "<br />");
                        strParticulars = dr1["CCM_PARTICULARS"].ToString().Length > 100 ? (dr1["CCM_PARTICULARS"].ToString()).Substring(0, 100).Replace("\n", "<br />") + " ..." : (dr1["CCM_PARTICULARS"].ToString()).Replace("\n", "<br />");
                        strPenalty = dr1["CCM_PENALTY"].ToString().Length > 100 ? (dr1["CCM_PENALTY"].ToString()).Substring(0, 100).Replace("\n", "<br />") + " ..." : (dr1["CCM_PENALTY"].ToString()).Replace("\n", "<br />");
                        strCheckpoint = dr1["CCM_CHECK_POINTS"].ToString().Length > 100 ? (dr1["CCM_CHECK_POINTS"].ToString()).Substring(0, 100).Replace("\n", "<br />") + " ..." : (dr1["CCM_CHECK_POINTS"].ToString()).Replace("\n", "<br />");

                        strHTMLReport = strHTMLReport +
                   "<tr><td class='DBTableCellLeft'>" + strDeptName + "</td>" +
                   "<td class='DBTableCellRight'>" + dr1["Quarter"].ToString() + "</td>" +

                   "<td class='DBTableCellRight'>" +
                      strSection +
                   "</td>" +

                   "<td class='DBTableCellRight'>"
                     // start - added by Hari on 3 oct 2016
                     +
                   "<a href='#' onclick=\"window.open(" +
                           "'ViewChecklistData.aspx?ChecklistId=" + dr1["CCD_ID"].ToString() +
                           "&Type=NA'," +
                           "'FILE1', 'location=0,status=0,scrollbars=1,width=700,height=500,resizable=1');return false;\">" +
                           strTitleOfSection + "</a>" +
                   //end - added by Hari on 3 oct 2016
                   //+ dr1["CCM_CLAUSE"].ToString() + commented by Hari on 3 oct 2016
                   "</td>" +

                   "<td class='DBTableCellRight'>" +
                        strCheckpoint +
                    "</td>" +

                    "<td class='DBTableCellRight'>" +
                        strParticulars +
                    "</td>" +

                    "<td class='DBTableCellRight'>" +
                        strPenalty +
                    "</td>" +

                   "<td class='DBTableCellRight'>" +
                      strTimeLimit +
                   "</td>" +

                       "<td class='DBTableCellRight'>" +
                       dr1["Compliance Status"].ToString() +
                   "</td>" +

                   "<td class='DBTableCellRight'>" +
                       dr1["Effective From"].ToString() +
                   "</td>" +

                   "<td class='DBTableCellRight'>" +
                       dr1["CCD_REMARKS"].ToString() +
                   "</td>" +

                   "</tr>";
                    }

                }

                strHTMLReport = strHTMLReport + "</table></BODY></HTML>";
                litDetails.Text = strHTMLReport;
            }
            catch (System.Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                throw new System.Exception("system exception in showDashboard(): " + ex);
            }
        }

        //protected void btnExportToExcel_Click(object sender, EventArgs e)
        //{
        //    string strHTMLReport = litDetails.Text.ToString();
        //    string attachment = "attachment; filename=Details.xls";
        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", attachment);
        //    Response.ContentType = "application/ms-excel";
        //    //start - added by Hari on 3 oct 2016
        //    string style = @"<style> TABLE { border: 1px hairline #333; } TD { border: 1px hairline #333; } </style> ";
        //    Response.Write(style);
        //    //end - added by Hari on 3 oct 2016
        //    Response.Write(strHTMLReport.ToString());
        //    Response.End();
        //}
        protected void btnExportToExcel_Click(object sender, System.EventArgs e)
        {
            int uniqueRowId = 0;
            string strChecklistTable = "", strHtmlTableChecklistDetsRows = "", strDepartmentName = "", strDeptId = "";
            strDepartmentName = Request.QueryString["DeptName"].ToString();
            DataTable dtChecklistDets = new DataTable();
            dtChecklistDets = (DataTable)Session["AllCehecklistDetails"];
            if (Request.QueryString["Type"] != null)
            {
                strTypeReport = Request.QueryString["Type"].ToString();

            }
            DataRow drChecklistDets;
            string strHtmlTable =
            "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
            "<HTML>" +
            "<HEAD>" +
            "</HEAD>" +
            "<BODY>" +

            " <table id='tblChecklistDets' width='100%' class='table table-bordered footable' align='left' style='margin-left:20px;' " +
                        " cellpadding='0' cellspacing='1' border='1'> " +
                      " <thead> " +
                      " <tr> " +
                      " <th class='tabhead' align='center'> " +
                      " Serial Number " +
                      " </th> " +
                      " <th class='tabhead' align='center'> " +
                      "Department" +
                      " </th> " +
                      " <th class='tabhead' align='center'> " +
                      "Quarter Name" +
                      " </th> " +
                       " <th class='tabhead' align='center'> " +
                      " Title of Section/Requirement" +
                      " </th> " +
                      " <th class='tabhead' align='center'> " +
                      " Section/Regulation Rule/Circulars " +
                      " </th> " +
                      " <th class='tabhead' align='center'> " +
                      " Time Limit" +
                      " </th> " +
                      " <th class='tabhead' align='center'> " +
                      " Compliance Status" +
                      " </th> " +
                      " <th class='tabhead' align='center'> " +
                      "Effective From" +
                      " </th> " +
                      " <th class='tabhead' align='center'> " +
                      " Remarks" +
                      " </th> " +
                      " </tr> " +
                      " </thead> ";
            int intChecklistDetsCnt = dtChecklistDets.Rows.Count;
            for (int intCnt = 0; intCnt < intChecklistDetsCnt; intCnt++)
            {
                uniqueRowId = uniqueRowId + 1;
                drChecklistDets = dtChecklistDets.Rows[intCnt];
                strDeptId = Request.QueryString["DeptId"].ToString();
                strHtmlTableChecklistDetsRows = strHtmlTableChecklistDetsRows + "<tr>" +
                "<td>" + uniqueRowId + "</td>";
                // "<td>" + strDepartmentName + "</td>" +
                if (strDeptId.Equals("0"))
                {
                    strHtmlTableChecklistDetsRows = strHtmlTableChecklistDetsRows +
                                                      "<td>" + drChecklistDets["DeptName"].ToString() + "</td>";
                }
                else
                {
                    strHtmlTableChecklistDetsRows = strHtmlTableChecklistDetsRows +
                                       "<td>" + strDepartmentName + "</td>";
                }
                strHtmlTableChecklistDetsRows = strHtmlTableChecklistDetsRows +
            //"<td>" + drChecklistDets["DeptName"].ToString() + "</td>" +
            "<td>" + drChecklistDets["Quarter"].ToString() + "</td>" +
            "<td>" + drChecklistDets["CCM_CLAUSE"].ToString() + "</td>" +
            "<td>" + drChecklistDets["CCM_REFERENCE"].ToString() + "</td>" +
            "<td>" + drChecklistDets["CCM_FREQUENCY"].ToString() + "</td>" +
            "<td>" + drChecklistDets["Compliance Status"].ToString() + "</td>" +
            "<td>" + drChecklistDets["Effective From"].ToString() + "</td>" +
            "<td>" + drChecklistDets["CCD_REMARKS"].ToString() + "</td>" +
            "</tr>";
            }
            strChecklistTable = strHtmlTable + strHtmlTableChecklistDetsRows + "</table> " +
            "</BODY>" +
            "</HTML>";
            string attachment = "attachment; filename=All Checklist Details.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write(strChecklistTable.ToString());
            Response.End();

        }
    }
}