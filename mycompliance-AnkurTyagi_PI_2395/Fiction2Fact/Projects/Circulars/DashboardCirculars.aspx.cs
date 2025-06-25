using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.Legacy_App_Code.Circulars.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using System.IO;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Circulars
{
    public partial class Circulars_DashboardCirculars : System.Web.UI.Page
    {
        //UtilitiesBLL UtilitiesBLL = new UtilitiesBLL();
        CircUtilitiesBLL circUtilBLL = new CircUtilitiesBLL();
        RefCodesBLL rcBL = new RefCodesBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    CommonCodes.SetDropDownDataSource(ddlType, circUtilBLL.GetDataTable("getTypeofCircular", new DBUtilityParameter("CDTM_STATUS", "A"), sOrderBy: "CDTM_TYPE_OF_DOC"));
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            try
            {
                Session["CircularNo"] = null;
                Session["CircularFromDate"] = null;
                Session["CircularToDate"] = null;
                Session["CircularType"] = null;
                Session["CircularAuthority"] = null;
                Session["PersonResponsible"] = null;
                Session["ActionTargetFromDate"] = null;
                Session["ActionTargetToDate"] = null;

                getCircularActionablesDashboard();
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        public void writeError(string strMsg)
        {
            lblMsg.Text = strMsg;
            lblMsg.Visible = true;
            lblMsg.CssClass = "label";
        }

        private void getCircularActionablesDashboard()
        {
            DataServer deserv = new DataServer();

            pnlActionableStatus.Visible = true;
            string strTotalQuery = "", strFilter = "", strQNotYetDue = "", strQCompletedWithinTargetDT = "", strQCompletedAfterTargetDT = "",
                strQDueBtNotCompleted = "", strNotYetDue = "", strCompletedWithinTargetDT = "", strCompletedAfterTargetDT = "",
                strDueBtNotCompleted = "", strtotalRecords = "", strScript = "";

            try
            {
                string strHtmlTable = "<table class='table table-bordered footable' width='100%' cellpadding='0' cellspacing='0'>";
                strHtmlTable += "<tr>";
                strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Yet Due</td>";
                strHtmlTable += "<td class= 'DBTableFirstCellRight'>Completed within Target Date</td>";
                strHtmlTable += "<td class= 'DBTableFirstCellRight'>Completed after Target Date</td>";
                strHtmlTable += "<td class= 'DBTableFirstCellRight'>Due but not Completed</td>";
                strHtmlTable += "<td class= 'DBTableFirstCellRight'>Total</td></tr>";

                using (F2FDatabase DB = new F2FDatabase())
                {
                    if (txtCircularNo.Text != "")
                    {
                        strFilter += " and CM_CIRCULAR_NO = @CircNo";
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CircNo", F2FDatabase.F2FDbType.VarChar, txtCircularNo.Text));
                        Session["CircularNo"] = txtCircularNo.Text;
                    }

                    if (txtCircularFromDate.Text != "")
                    {
                        //strFilter += " and CM_DATE = @CmDate";
                        strFilter += " and CM_DATE >= @CmFromDate";
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CmFromDate", F2FDatabase.F2FDbType.VarChar, txtCircularFromDate.Text));
                        Session["CircularFromDate"] = txtCircularFromDate.Text;
                    }

                    if (txtCircularToDate.Text != "")
                    {
                        strFilter += " and CM_DATE <= @CmToDate";
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CmToDate", F2FDatabase.F2FDbType.VarChar, txtCircularToDate.Text));
                        Session["CircularToDate"] = txtCircularToDate.Text;
                    }

                    if (ddlType.SelectedValue != "")
                    {
                        strFilter += " and CDTM_ID = @CdtmId";
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CdtmId", F2FDatabase.F2FDbType.VarChar, ddlType.SelectedValue));
                        Session["CircularType"] = ddlType.SelectedValue;
                    }

                    if (ddlCircularAuthority.SelectedValue != "")
                    {
                        strFilter += "  and CIA_ID = @CiaId";
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CiaId", F2FDatabase.F2FDbType.VarChar, ddlCircularAuthority.SelectedValue));
                        Session["CircularAuthority"] = ddlCircularAuthority.SelectedValue;
                    }

                    if (txtPersonResponsible.Text != "")
                    {
                        strFilter += " and CA_PERSON_RESPONSIBLE like @RespPerson ";
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@RespPerson", F2FDatabase.F2FDbType.VarChar, "%" + txtPersonResponsible.Text + "%"));
                        Session["PersonResponsible"] = txtPersonResponsible.Text;
                    }

                    if (txtFromDate.Text != "")
                    {
                        strFilter += " and CA_TARGET_DATE >= @FDate";
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@FDate", F2FDatabase.F2FDbType.VarChar, txtFromDate.Text));
                        Session["ActionTargetFromDate"] = txtFromDate.Text;
                    }

                    if (txtToDate.Text != "")
                    {
                        strFilter += " and CA_TARGET_DATE <= @TDate";
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@TDate", F2FDatabase.F2FDbType.VarChar, txtToDate.Text));
                        Session["ActionTargetToDate"] = txtToDate.Text;
                    }

                    strTotalQuery = " SELECT  Count(1) " +
                                    " FROM TBL_CIRCULAR_MASTER " +
                                    " left outer JOIN TBL_CIRCULAR_ACTIONABLES  ON  CM_ID=CA_CM_ID  " +
                                    " left outer JOIN TBL_CIRCULAR_ISSUING_AUTHORITIES  on CIA_ID= CM_CIA_ID  " +
                                    " left outer JOIN TBL_CIRCULAR_AREA_MAS  on CAM_ID= CM_CAM_ID  " +
                                    " left outer JOIN TBL_CIRCULAR_FUNCTION_MAS  on CA_CFM_ID= CFM_ID   " +
                                    " INNER JOIN TBL_REF_CODES b ON b.RC_CODE = CA_STATUS AND  b.RC_TYPE = 'Actionable Status' " +
                                    " inner join TBL_CIRCULAR_DOCUMENT_TYPE_MAS on CDTM_ID = CM_CDTM_ID " +
                                    //" Left Outer join TBL_CIRC_COMPLIANCE_SPOCS on CCS_ID = CM_CCS_ID  " +
                                    " WHERE 1 = 1 ";

                    //strtotalRecords = DataServer.ExecuteScalar(strTotalQuery + strFilter).ToString();

                    DB.F2FCommand.CommandText = strTotalQuery + strFilter;
                    DB.OpenConnection();
                    strtotalRecords = DB.F2FCommand.ExecuteScalar().ToString();
                    //if (strtotalRecords != "0")
                    //{
                    if (strtotalRecords != "0")
                        strQNotYetDue = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + ";
                    else
                        strQNotYetDue = "SELECT cast(0.00 as varchar) + '|' + ";

                    strQNotYetDue += " cast(count(1) as varchar) " +
                                       " FROM TBL_CIRCULAR_MASTER " +
                                       " left outer JOIN TBL_CIRCULAR_ACTIONABLES  ON  CM_ID=CA_CM_ID  " +
                                       " left outer JOIN TBL_CIRCULAR_ISSUING_AUTHORITIES  on CIA_ID= CM_CIA_ID  " +
                                       " left outer JOIN TBL_CIRCULAR_AREA_MAS  on CAM_ID= CM_CAM_ID  " +
                                       " left outer JOIN TBL_CIRCULAR_FUNCTION_MAS  on CA_CFM_ID= CFM_ID " +
                                       " INNER JOIN TBL_REF_CODES b ON b.RC_CODE = CA_STATUS AND  b.RC_TYPE = 'Actionable Status'  " +
                                       " inner join TBL_CIRCULAR_DOCUMENT_TYPE_MAS on CDTM_ID = CM_CDTM_ID " +
                                       //" Left Outer join TBL_CIRC_COMPLIANCE_SPOCS on CCS_ID = CM_CCS_ID  " +
                                       " WHERE 1 = 1 and DATEADD(dd, 0, DATEDIFF(dd, 0,CA_TARGET_DATE)) >  DATEADD(dd, 0, DATEDIFF(dd, 0,current_timestamp)) and CA_STATUS = 'P'";

                    if (strtotalRecords != "0")
                        strQCompletedWithinTargetDT = " SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + ";
                    else
                        strQCompletedWithinTargetDT = "SELECT cast(0.00 as varchar) + '|' + ";

                    strQCompletedWithinTargetDT += " cast(count(1) as varchar) " +
                                                 " FROM TBL_CIRCULAR_MASTER " +
                                                 " left outer JOIN TBL_CIRCULAR_ACTIONABLES  ON  CM_ID=CA_CM_ID  " +
                                                 " left outer JOIN TBL_CIRCULAR_ISSUING_AUTHORITIES  on CIA_ID= CM_CIA_ID  " +
                                                 " left outer JOIN TBL_CIRCULAR_AREA_MAS  on CAM_ID= CM_CAM_ID  " +
                                                 " left outer JOIN TBL_CIRCULAR_FUNCTION_MAS  on CA_CFM_ID= CFM_ID   " +
                                                 " INNER JOIN TBL_REF_CODES b ON b.RC_CODE = CA_STATUS AND  b.RC_TYPE = 'Actionable Status'  " +
                                                 " inner join TBL_CIRCULAR_DOCUMENT_TYPE_MAS on CDTM_ID = CM_CDTM_ID " +
                                                 //" Left Outer join TBL_CIRC_COMPLIANCE_SPOCS on CCS_ID = CM_CCS_ID  " +
                                                 " WHERE 1 = 1 and DATEADD(dd, 0, DATEDIFF(dd, 0,CA_COMPLETION_DATE)) <= " +
                                                 " DATEADD(dd, 0, DATEDIFF(dd, 0,CA_TARGET_DATE))" +
                                                 " and CA_STATUS = 'C' ";

                    if (strtotalRecords != "0")
                        strQCompletedAfterTargetDT = "SELECT cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + ";
                    else
                        strQCompletedAfterTargetDT = "SELECT cast(0.00 as varchar) + '|' + ";

                    strQCompletedAfterTargetDT += " cast(count(1) as varchar) " +
                                                " FROM TBL_CIRCULAR_MASTER " +
                                                " left outer JOIN TBL_CIRCULAR_ACTIONABLES  ON  CM_ID=CA_CM_ID  " +
                                                " left outer JOIN TBL_CIRCULAR_ISSUING_AUTHORITIES  on CIA_ID= CM_CIA_ID  " +
                                                " left outer JOIN TBL_CIRCULAR_AREA_MAS  on CAM_ID= CM_CAM_ID  " +
                                                " left outer JOIN TBL_CIRCULAR_FUNCTION_MAS  on CA_CFM_ID= CFM_ID   " +
                                                " INNER JOIN TBL_REF_CODES b ON b.RC_CODE = CA_STATUS AND  b.RC_TYPE = 'Actionable Status'  " +
                                                " inner join TBL_CIRCULAR_DOCUMENT_TYPE_MAS on CDTM_ID = CM_CDTM_ID " +
                                                //" Left Outer join TBL_CIRC_COMPLIANCE_SPOCS on CCS_ID = CM_CCS_ID  " +
                                                " WHERE 1 = 1 and DATEADD(dd, 0, DATEDIFF(dd, 0,CA_COMPLETION_DATE)) >" +
                                                " DATEADD(dd, 0, DATEDIFF(dd, 0,CA_TARGET_DATE)) and CA_STATUS = 'C' ";

                    if (strtotalRecords != "0")
                        strQDueBtNotCompleted = " SELECT  cast(Cast((Count(1)/" + strtotalRecords + ".00 *100.00) as decimal(5,2)) as varchar) + '|' + ";
                    else
                        strQDueBtNotCompleted = "SELECT cast(0.00 as varchar) + '|' + ";

                    strQDueBtNotCompleted += " cast(count(1) as varchar) " +
                                           " FROM TBL_CIRCULAR_MASTER " +
                                           " left outer JOIN TBL_CIRCULAR_ACTIONABLES  ON  CM_ID=CA_CM_ID  " +
                                           " left outer JOIN TBL_CIRCULAR_ISSUING_AUTHORITIES  on CIA_ID= CM_CIA_ID " +
                                           " left outer JOIN TBL_CIRCULAR_AREA_MAS  on CAM_ID= CM_CAM_ID " +
                                           " left outer JOIN TBL_CIRCULAR_FUNCTION_MAS  on CA_CFM_ID= CFM_ID " +
                                           " INNER JOIN TBL_REF_CODES b ON b.RC_CODE = CA_STATUS AND  b.RC_TYPE = 'Actionable Status' " +
                                           " inner join TBL_CIRCULAR_DOCUMENT_TYPE_MAS on CDTM_ID = CM_CDTM_ID " +
                                           //" Left Outer join TBL_CIRC_COMPLIANCE_SPOCS on CCS_ID = CM_CCS_ID " +
                                           " WHERE 1 = 1 and DATEADD(dd, 0, DATEDIFF(dd, 0,CA_TARGET_DATE)) <= " +
                                           " DATEADD(dd, 0, DATEDIFF(dd, 0,current_timestamp))  and CA_STATUS = 'P' ";

                    //strNotYetDue = DataServer.ExecuteScalar(strQNotYetDue + strFilter).ToString();
                    DB.F2FCommand.CommandText = strQNotYetDue + strFilter;
                    strNotYetDue = DB.F2FCommand.ExecuteScalar().ToString();
                    //strCompletedWithinTargetDT = DataServer.ExecuteScalar(strQCompletedWithinTargetDT + strFilter).ToString();
                    DB.F2FCommand.CommandText = strQCompletedWithinTargetDT + strFilter;
                    strCompletedWithinTargetDT = DB.F2FCommand.ExecuteScalar().ToString();
                    //strCompletedAfterTargetDT = DataServer.ExecuteScalar(strQCompletedAfterTargetDT + strFilter).ToString();
                    DB.F2FCommand.CommandText = strQCompletedAfterTargetDT + strFilter;
                    strCompletedAfterTargetDT = DB.F2FCommand.ExecuteScalar().ToString();
                    //strDueBtNotCompleted = DataServer.ExecuteScalar(strQDueBtNotCompleted + strFilter).ToString();
                    DB.F2FCommand.CommandText = strQDueBtNotCompleted + strFilter;
                    strDueBtNotCompleted = DB.F2FCommand.ExecuteScalar().ToString();

                    strScript = "<script type=\"text/javascript\">Morris.Donut({ element: 'donut-chart-color',data: [";
                    strScript += "{value: " + strNotYetDue.Split('|')[0] + ", label: 'Not Yet Due'},";
                    strScript += "{value: " + strCompletedWithinTargetDT.Split('|')[0] + ", label: 'Completed within Target Date'},";
                    strScript += "{value: " + strCompletedAfterTargetDT.Split('|')[0] + ", label: 'Completed after Target Date'},";
                    strScript += "{value: " + strDueBtNotCompleted.Split('|')[0] + ", label: 'Due but not Completed'},";
                    strScript += "], backgroundColor: '#F4F4F4', labelColor: '#000',";
                    strScript += "resize: true,colors: ['#65C3A8','#7ccc32','#ffbf00','#cc0001'],";
                    strScript += "formatter: function (x) { return x + \" % \"}});";
                    strScript += Environment.NewLine + Environment.NewLine;

                    strScript += "Morris.Bar({element: 'bar-color-chart',data: [";
                    strScript += "{x: 'Not Yet Due', y: " + strNotYetDue.Split('|')[1] + "},";
                    strScript += "{x: 'Completed within Target Date', y: " + strCompletedWithinTargetDT.Split('|')[1] + "},";
                    strScript += "{x: 'Completed after Target Date', y: " + strCompletedAfterTargetDT.Split('|')[1] + "},";
                    strScript += "{x: 'Due but not Completed', y: " + strDueBtNotCompleted.Split('|')[1] + "},";
                    strScript += "],xkey: 'x',ykeys: ['y'],labels: ['Y'],resize: true,xLabelAngle: 60,barColors: ";
                    strScript += "function (row, series, type) {if (type === 'bar') {if(row.label == 'Not Yet Due'){return '#65C3A8'}";
                    strScript += "if(row.label == 'Completed within Target Date'){return '#7ccc32'}";
                    strScript += "if(row.label == 'Completed after Target Date'){return '#ffbf00'}" +
                                  "if(row.label == 'Due but not Completed'){return '#cc0001'}" +
                                  "}else {return '#000';}}";
                    strScript += "});";
                    strScript += "</script>";

                    litChart.Text = strScript;


                    strHtmlTable += "<tr><td class= 'DBTableCellLeft'><center><a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
                                    "'DetailedReportCircular.aspx?ReportType=1&Status=ND'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">"
                                    + strNotYetDue.Split('|')[1] +
                                    "</a></center></td>";

                    strHtmlTable += "<td class= 'DBTableCellLeft'><center><a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
                                    "'DetailedReportCircular.aspx?ReportType=1&Status=CWD'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">"
                                    + strCompletedWithinTargetDT.Split('|')[1] + "</a></center></td>";

                    strHtmlTable += "<td class= 'DBTableCellLeft'><center><a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
                                    "'DetailedReportCircular.aspx?ReportType=1&Status=CAD'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">"
                                    + strCompletedAfterTargetDT.Split('|')[1] + "</center></td>";

                    strHtmlTable += "<td class= 'DBTableCellLeft'><center><a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
                                    "'DetailedReportCircular.aspx?ReportType=1&Status=DNS'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">"
                                    + strDueBtNotCompleted.Split('|')[1] + "</center></td>";

                    strHtmlTable += "<td class= 'DBTableCellLeft'><center><a href='#' class='badge rounded-pill badge-soft-pink'  onclick=\"window.open(" +
                                    "'DetailedReportCircular.aspx?ReportType=1&Status=Total'," +
                                    "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">"
                                    + strtotalRecords + "</center></td></tr>";

                    strHtmlTable += "</table><br/><br/>";

                    litActionableStatus.Text = strHtmlTable;
                    //}
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void cvTodate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
    }
}