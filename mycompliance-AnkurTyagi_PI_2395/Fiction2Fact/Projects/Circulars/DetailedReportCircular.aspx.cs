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
using System.IO;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Circulars
{
    public partial class Circulars_DetailedReportCircular : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                    getdetailsReport(mstrConnectionString);
                }
            }
            catch (Exception ex)
            {
                writeError("Invalid Parameter input." + ex);
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
            lblMsg.Text = strError;
            lblMsg.Visible = true;
            lblMsg.CssClass = "label";
        }

        //<<Added by Ankur Tyagi on 08-Apr-2024 for CR_2041
        public string ReturnCutoffFilterQry()
        {
            string retQry = "";
            string strCutOffDate = F2FDatabase.F2FExecuteScalar(" select CP_VALUE from TBL_CONFIG_PARAMS where CP_NAME = 'Dashboard count Cut-off date'").ToString();
            string strCutOffParam = F2FDatabase.F2FExecuteScalar(" select CP_VALUE from TBL_CONFIG_PARAMS where CP_NAME = 'Circular Cut-off param'").ToString();

            if (!string.IsNullOrEmpty(strCutOffDate))
            {
                retQry = retQry + " AND " + strCutOffParam + " >= '" + strCutOffDate + "' ";
            }
            return retQry;
        }
        //>>

        public void getdetailsReport(string strConnectionString)
        {
            string strReportTypeId = "";
            DataTable dt = new DataTable();

            try
            {
                if (Request.QueryString["ReportType"] != null)
                {
                    strReportTypeId = Request.QueryString["ReportType"].ToString();
                    hfReportType.Value = strReportTypeId;
                }

                if (strReportTypeId.Equals("1"))
                {
                    getCircularActionableReport(strConnectionString);
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void getCircularActionableReport(string strConnectionString)
        {
            string strStatus = "", strHeading = "", strHtmlTable = "", strFilter = "", strQry1 = "", strCircularDate = "", strTargetDate = "",
                strCompletionDate = "";
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
                        if (strStatus == "ND")
                        {
                            strFilter += " and DATEADD(dd, 0, DATEDIFF(dd, 0,CA_TARGET_DATE)) >  DATEADD(dd, 0, DATEDIFF(dd, 0,current_timestamp))" +
                                " and CA_STATUS = 'P' ";
                        }

                        else if (strStatus == "CWD")
                        {
                            strFilter += " and DATEADD(dd, 0, DATEDIFF(dd, 0,CA_COMPLETION_DATE)) <= " +
                                         " DATEADD(dd, 0, DATEDIFF(dd, 0,CA_TARGET_DATE))" +
                                         " and CA_STATUS = 'C'  ";
                        }
                        else if (strStatus == "CAD")
                        {
                            strFilter += " and DATEADD(dd, 0, DATEDIFF(dd, 0,CA_COMPLETION_DATE)) >" +
                                         " DATEADD(dd, 0, DATEDIFF(dd, 0,CA_TARGET_DATE)) and CA_STATUS = 'C'  ";
                        }
                        else if (strStatus == "DNS")
                        {
                            strFilter += " and DATEADD(dd, 0, DATEDIFF(dd, 0,CA_TARGET_DATE)) <= " +
                                         " DATEADD(dd, 0, DATEDIFF(dd, 0,current_timestamp)) and CA_STATUS = 'P'  ";
                        }
                        else if (strStatus == "Total")
                        {
                            strFilter += "  ";
                        }

                        //if (Session["CircularDashBoardFilterQuery"] != null)
                        //{
                        //    strFilter += " " + Session["CircularDashBoardFilterQuery"].ToString();
                        //}

                        using (F2FDatabase DB = new F2FDatabase())
                        {
                            if (Session["CircularNo"] != null)
                            {
                                strFilter += " and CM_CIRCULAR_NO = @CircNo";
                                DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CircNo", F2FDatabase.F2FDbType.VarChar, Session["CircularNo"].ToString()));
                            }

                            if (Session["CircularFromDate"] != null)
                            {
                                strFilter += " and CM_DATE >= @CmFromDate";
                                DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CmFromDate", F2FDatabase.F2FDbType.VarChar, Session["CircularFromDate"].ToString()));
                            }

                            if (Session["CircularToDate"] != null)
                            {
                                strFilter += " and CM_DATE <= @CmToDate";
                                DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CmToDate", F2FDatabase.F2FDbType.VarChar, Session["CircularToDate"].ToString()));
                            }

                            if (Session["CircularType"] != null)
                            {
                                strFilter += " and CDTM_ID = @CdtmId";
                                DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CdtmId", F2FDatabase.F2FDbType.VarChar, Session["CircularType"].ToString()));
                            }

                            if (Session["CircularAuthority"] != null)
                            {
                                strFilter += "  and CIA_ID = @CiaId";
                                DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CiaId", F2FDatabase.F2FDbType.VarChar, Session["CircularAuthority"].ToString()));
                            }

                            if (Session["PersonResponsible"] != null)
                            {
                                strFilter += " and CA_PERSON_RESPONSIBLE like @RespPerson";
                                DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@RespPerson", F2FDatabase.F2FDbType.VarChar, "%" + Session["PersonResponsible"].ToString() + "%"));
                            }

                            if (Session["ActionTargetFromDate"] != null)
                            {
                                strFilter += " and CA_TARGET_DATE >= @FDate";
                                DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@FDate", F2FDatabase.F2FDbType.VarChar, Session["ActionTargetFromDate"].ToString()));
                            }

                            if (Session["ActionTargetToDate"] != null)
                            {
                                strFilter += " and CA_TARGET_DATE <= @TDate";
                                DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@TDate", F2FDatabase.F2FDbType.VarChar, Session["ActionTargetToDate"].ToString()));
                            }

                            strQry1 = " SELECT CA_PERSON_RESPONSIBLE, CA_ID, CA_CM_ID,CA_STATUS, CDTM_TYPE_OF_DOC, CM_CIRCULAR_NO, CM_DATE, " +
                                  " CFM_NAME, CIA_NAME, CAM_NAME, CM_TOPIC, CM_DETAILS ,CM_IMPLICATIONS ,CA_ACTIONABLE, " +
                                  " CA_PERSON_RESPONSIBLE_NAME, CA_PERSON_RESPONSIBLE_ID,CA_PERSON_RESPONSIBLE_EMAIL_ID, " +
                                  " CA_TARGET_DATE, b.RC_NAME as Status,b.RC_CODE, " +
                                  " CA_COMPLETION_DATE,CA_REMARKS,CIA_ID,CDTM_ID " +
                                  " FROM TBL_CIRCULAR_MASTER " +
                                  " left outer JOIN TBL_CIRCULAR_ACTIONABLES  ON  CM_ID=CA_CM_ID  " +
                                  " left outer JOIN TBL_CIRCULAR_ISSUING_AUTHORITIES  on CIA_ID= CM_CIA_ID " +
                                  " left outer JOIN TBL_CIRCULAR_AREA_MAS  on CAM_ID= CM_CAM_ID " +
                                  " left outer JOIN TBL_CIRCULAR_FUNCTION_MAS  on CA_CFM_ID= CFM_ID " +
                                  " INNER JOIN TBL_REF_CODES b ON b.RC_CODE = CA_STATUS AND  b.RC_TYPE = 'Actionable Status' " +
                                  " inner join TBL_CIRCULAR_DOCUMENT_TYPE_MAS on CDTM_ID = CM_CDTM_ID " +
                                  " Left Outer join TBL_CIRC_COMPLIANCE_SPOCS on CCS_ID = CM_CCS_ID " +
                                  " WHERE 1 = 1" + strFilter +
                                  ReturnCutoffFilterQry();

                            DB.F2FCommand.CommandText = strQry1;
                            DB.F2FDataAdapter.Fill(dt);
                        }

                        if (dt.Rows.Count > 0)
                        {
                            btnExportToExcel.Visible = true;
                            strHtmlTable = "<table width='100%' class='table table-bordered footable' cellpadding='0' cellspacing='0' >";
                            strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Sr.No.</td>";
                            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Circular Number</td>";
                            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Circular Date</td>";
                            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Type of Document</td>";
                            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Issuing Authority</td>";
                            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Topic</td>";
                            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Subject of Circular</td>";
                            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Actionable</td>";
                            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Person Responsible User Name</td>";
                            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Target Date</td>";
                            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Completion Date</td>";
                            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Status</td>";
                            strHtmlTable += "<td class= 'DBTableFirstCellRight'>Remarks	</td></tr>";

                            for (intCnt = 0; intCnt < dt.Rows.Count; intCnt++)
                            {
                                dr = dt.Rows[intCnt];

                                strCircularDate = dr["CM_DATE"].Equals(DBNull.Value) ? "&nbsp;" :
                                     Convert.ToDateTime(dr["CM_DATE"]).ToString("dd-MMM-yyyy");

                                strTargetDate = dr["CA_TARGET_DATE"].Equals(DBNull.Value) ? "&nbsp;" :
                                                      Convert.ToDateTime(dr["CA_TARGET_DATE"]).ToString("dd-MMM-yyyy");

                                strCompletionDate = dr["CA_COMPLETION_DATE"].Equals(DBNull.Value) ? "&nbsp;" :
                                                 Convert.ToDateTime(dr["CA_COMPLETION_DATE"]).ToString("dd-MMM-yyyy");

                                strHtmlTable += "<tr><td class= 'DBTableCellRight'>" + (intCnt + 1) + "&nbsp;</td>";
                                strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["CM_CIRCULAR_NO"].ToString() + "&nbsp;</td>";
                                strHtmlTable += "<td class= 'DBTableCellRight'>" + strCircularDate + "&nbsp;</td>";
                                strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["CDTM_TYPE_OF_DOC"].ToString() + "&nbsp;</td>";
                                strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["CIA_NAME"].ToString() + "&nbsp;</td>";
                                strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["CAM_NAME"].ToString() + "&nbsp;</td>";
                                strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["CM_TOPIC"].ToString() + "&nbsp;</td>";
                                strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["CA_ACTIONABLE"].ToString().Replace("\n", "<br />") + "&nbsp;</td>";
                                strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["CA_PERSON_RESPONSIBLE"].ToString() + "&nbsp;</td>";
                                strHtmlTable += "<td class= 'DBTableCellRight'>" + strTargetDate + "&nbsp;</td>";
                                strHtmlTable += "<td class= 'DBTableCellRight'>" + strCompletionDate + "&nbsp;</td>";
                                strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["status"].ToString() + "&nbsp;</td>";
                                strHtmlTable += "<td class= 'DBTableCellRight'>" + dr["CA_REMARKS"].ToString().Replace("\n", "<br />") + "&nbsp;</td></tr>";

                                strCircularDate = ""; strTargetDate = ""; strCompletionDate = "";
                            }
                            strHtmlTable = strHtmlTable + "</table>";

                            litDetails.Text = strHtmlTable;
                            Session["strCircDetailedReport"] = strHtmlTable;
                        }
                    }
                }

                if (strStatus == "ND")
                {
                    strHeading = "- (Not Yet Due)";
                }
                else if (strStatus == "CWD")
                {
                    strHeading = "- (Completed within Target Date)";
                }
                else if (strStatus == "CAD")
                {
                    strHeading = "- (Completed after Target Date)";
                }
                else if (strStatus == "DNS")
                {
                    strHeading = "- (Due but not Completed)";
                }
                else if (strStatus == "Total")
                {
                    strHeading = "- (Total)";
                }
                else
                {
                    strHeading = "- (Total)";
                }
                lblHeading.Text = "Circular Actionable Dashboard " + strHeading;
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            string strHTMLReport = "";
            string attachment, html2;

            if (Session["strCircDetailedReport"] != null)
            {
                strHTMLReport = Session["strCircDetailedReport"].ToString();

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
    }
}