using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Reflection;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;

namespace Fiction2Fact.Projects.Reports
{
    public partial class ReportFramework_Reports : System.Web.UI.Page
    {
        UtilitiesBLL utilBLL = new UtilitiesBLL();
        ReportsBLL ReportsBLL = new ReportsBLL();

        string mstrConnectionString = null;

        string strTableFirstCellLeft = "font-size: 12px; font-family: Trebuchet MS; " +
                        " background-COLOR: #C7222A; border-width: 1px; padding: 8px;" +
                        " text-decoration:none; COLOR: white; " +
                        " border-style: solid; border-COLOR: brown; text-align: center;";

        string strTableCellLeft = "font-size: 12px; font-family: Trebuchet MS; " +
                                " border-width: 1px; padding: 8px; text-align: center; " +
                                " text-decoration:none; border-top-color: white; " +
                                " border-style: solid; border-COLOR: brown;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Type"] != null)
                {
                    hfType.Value = Request.QueryString["Type"].ToString();
                }

                populateDropdownList();

                btnExportToExcel.Visible = false;
            }
        }

        private void populateDropdownList()
        {
            try
            {
                ddlReportType.DataSource = ReportsBLL.getReportType("", "", mstrConnectionString);
                ddlReportType.DataTextField = "RTM_NAME";
                ddlReportType.DataValueField = "RTM_ID";
                ddlReportType.DataBind();
                ddlReportType.Items.Insert(0, new ListItem("---------------- (Select an option) ----------------", ""));
            }
            catch (Exception ex)
            {
                writeError("Exception in " + MethodBase.GetCurrentMethod().Name + "(): " + ex.Message);
            }
        }

        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr, dr1;
            string strRRTMId = "", strFilter1Name = "", strFilter2Name = "", strFilter3Name = "";
            dt = ReportsBLL.getReportType(ddlReportType.SelectedValue, "", mstrConnectionString);
            dr1 = dt.Rows[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                if (ddlReportType.SelectedValue == dr["RTM_ID"].ToString())
                {
                    strRRTMId = dr["RTM_ID"].ToString();
                    strFilter1Name = dr["RTM_FILTER_1_NAME"].ToString();
                    strFilter2Name = dr["RTM_FILTER_2_NAME"].ToString();
                    strFilter3Name = dr["RTM_FILTER_3_NAME"].ToString();
                }
            }

            if (ddlReportType.SelectedValue != "")
            {
                if (ddlReportType.SelectedValue == strRRTMId)
                {
                    ddlFilter1.DataSource = ReportsBLL.getReportType(ddlReportType.SelectedValue, "Filter1", mstrConnectionString);
                    ddlFilter1.DataTextField = "Text";
                    ddlFilter1.DataValueField = "Value";
                    ddlFilter1.DataBind();
                    if (!strFilter1Name.Equals(""))
                        ddlFilter1.Items.Insert(0, new ListItem("---- (Select " + strFilter1Name + ") ----", ""));

                    ddlFilter2.DataSource = ReportsBLL.getReportType(ddlReportType.SelectedValue, "Filter2", mstrConnectionString);
                    ddlFilter2.DataTextField = "Text";
                    ddlFilter2.DataValueField = "Value";
                    ddlFilter2.DataBind();
                    if (!strFilter2Name.Equals(""))
                        ddlFilter2.Items.Insert(0, new ListItem("---- (Select " + strFilter2Name + ") ----", ""));

                    ddlFilter3.DataSource = ReportsBLL.getReportType(ddlReportType.SelectedValue, "Filter3", mstrConnectionString);
                    ddlFilter3.DataTextField = "Text";
                    ddlFilter3.DataValueField = "Value";
                    ddlFilter3.DataBind();
                    if (!strFilter3Name.Equals(""))
                        ddlFilter3.Items.Insert(0, new ListItem("---- (Select " + strFilter3Name + ") ----", ""));
                }
            }
            else
            {
                ddlFilter1.DataSource = "";
                ddlFilter1.DataBind();

                ddlFilter2.DataSource = "";
                ddlFilter2.DataBind();

                ddlFilter3.DataSource = "";
                ddlFilter3.DataBind();
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        private void getReport(string strReportType)
        {
            int inti = 0;
            string strCellColorStyle = "", strValue = "";
            bool flag = true;

            DataTable dt = new DataTable();
            DataTable dtCPValue = new DataTable();

            try
            {
                //dtCPValue = utilBLL.getDataset("getRRReportWithNoColor", mstrConnectionString).Tables[0];

                //if (dtCPValue.Rows.Count > 0)
                //{
                //    string[] strCPValues = dtCPValue.Rows[0]["CP_VALUE"].ToString().Split(',');

                //    foreach (string strCPValue in strCPValues)
                //    {
                //        if (strReportType.Equals(strCPValue))
                //        {
                //            flag = false;
                //            break;
                //        }
                //    }
                //}

                dt = ReportsBLL.getReportData(strReportType, txtDateOfReport.Text, ddlFilter1.SelectedValue,
                    ddlFilter2.SelectedValue, ddlFilter3.SelectedValue, mstrConnectionString);

                //gvResult.DataSource = dt;
                //gvResult.DataBind();
                if (dt.Rows.Count > 0)
                {
                    string strX = "";
                    string html = "<table class=\"mGrid1\" border='0' width='100%' cellpadding='0' cellspacing='0'>";
                    //add header row
                    html += "<tr>";
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (dt.Columns[i].ColumnName.Contains("X_ID") || dt.Columns[i].ColumnName.Contains("Y_ID")
                            || dt.Columns[i].ColumnName.Contains("COLOR"))
                        {
                            continue;
                        }

                        html += "<td style='" + strTableFirstCellLeft + "'>" + dt.Columns[i].ColumnName + "</td>";
                    }
                    html += "</tr>";

                    //add rows
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int intColor = 0;
                        strX = "";
                        html += "<tr>";
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Rows[i][j].ToString().Equals(""))
                                strValue = "0";
                            else
                                strValue = dt.Rows[i][j].ToString();

                            if (dt.Columns[j].ColumnName.Contains("X_ID"))
                            {
                                strX = dt.Rows[i][j].ToString();
                                continue;
                            }

                            if (dt.Columns[j].ColumnName.Contains("Y_ID") || dt.Columns[j].ColumnName.Contains("COLOR"))
                            {
                                continue;
                            }

                            //if (flag == true)
                            //{
                            //    if (!dt.Columns[j].ColumnName.Contains("CB") && !dt.Columns[j].ColumnName.Contains("Branch Name"))
                            //    {
                            //        if (dt.Rows[i]["ENTIRE ROW COLOR"] != DBNull.Value)
                            //        {
                            //            strCellColorStyle = dt.Rows[i]["ENTIRE ROW COLOR"].ToString();
                            //        }
                            //        else
                            //        {
                            //            if (dt.Columns[j].ColumnName.Equals("Total Customer Count") || dt.Columns[j].ColumnName.Equals("Total Amount")
                            //            || dt.Columns[j].ColumnName.Equals("Percentage"))
                            //            {
                            //                if (dt.Rows[i]["TOTAL BUCKET COLOR"] != DBNull.Value)
                            //                    strCellColorStyle = dt.Rows[i]["TOTAL BUCKET COLOR"].ToString();
                            //                else
                            //                    strCellColorStyle = "#FFFFFF";
                            //            }
                            //            else
                            //            {
                            //                if (dt.Rows[i]["R_BUCKET_" + intColor + "_COLOR"] != DBNull.Value)
                            //                    strCellColorStyle = dt.Rows[i]["R_BUCKET_" + intColor + "_COLOR"].ToString();
                            //                else
                            //                    strCellColorStyle = "#FFFFFF";
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        strCellColorStyle = "#FFFFFF";
                            //    }
                            //}

                            //if (dt.Columns[j].ColumnName.Contains("CB"))
                            //{
                            //    if (dt.Rows[i]["ENTIRE ROW COLOR"] != DBNull.Value)
                            //        strCellColorStyle = dt.Rows[i]["ENTIRE ROW COLOR"].ToString();
                            //    else
                            //        strCellColorStyle = "#FFFFFF";
                            //}

                            if (dt.Rows[i][j].Equals("Total"))
                            {
                                string Total = dt.Rows[i][j].ToString();
                                inti = i;

                                if (flag == true)
                                {
                                    //if (dt.Rows[i]["ENTIRE ROW COLOR"] != DBNull.Value)
                                    //    strCellColorStyle = dt.Rows[i]["ENTIRE ROW COLOR"].ToString();

                                    if (inti != 0)
                                    {
                                        //html += "<td style='background-color:" + strCellColorStyle + ";" + strTableCellLeft + " color: white;'>" + strValue.ToString() + "</td>";
                                        html += "<td style='background-color:" + strCellColorStyle + ";" + strTableCellLeft + " '>" + strValue.ToString() + "</td>";
                                        continue;
                                    }
                                }
                            }

                            //This part belongs fo the bottom total 
                            //if (flag == true)
                            //{
                            //    if (inti != 0)
                            //    {
                            //        html += "<td style='background-color:" + strCellColorStyle + ";" + strTableCellLeft + " color: white;'>" + strValue.ToString() + "</td>";
                            //        continue;
                            //    }
                            //}

                            //if (dt.Columns[j].ColumnName.Contains("Count") && !dt.Columns[j].ColumnName.Contains("Total"))
                            if (dt.Columns[j].ColumnName.Contains("Count") && !dt.Columns[j].ColumnName.Contains("Percentage"))
                            {
                                if (dt.Columns[j].ColumnName.Contains("Total"))
                                {
                                    html += "<td style='background-color:" + strCellColorStyle + "; cursor: pointer;" + strTableCellLeft + "; border-top-style:none !important;'" +
                                        "onclick=\"window.open(" +
                                        "'DetailedReport.aspx?ReportType=" + ddlReportType.SelectedValue + "&DateofReport=" + txtDateOfReport.Text + "&Filter1=" + ddlFilter1.SelectedValue + "" +
                                        "&Filter2=" + ddlFilter2.SelectedValue + "&Filter3=" + ddlFilter3.SelectedValue + "&X=&Y=" + dt.Rows[i]["Y_ID"] + "', 'FILE'," +
                                        "'location=0,status=0,scrollbars=1,resizable=1,width=1000," +
                                        "height=650');return false;\">" +
                                        "<a style='color: darkblue !important; font-weight: bold;' href='#' >" + strValue + "</a>" +
                                        "</td>";
                                }
                                else
                                {
                                    html += "<td style='background-color:" + strCellColorStyle + "; cursor: pointer; border-right-style: none !important; border-top-style: none !important;" + strTableCellLeft + "'" +
                                        "onclick=\"window.open(" +
                                        "'DetailedReport.aspx?ReportType=" + ddlReportType.SelectedValue + "&DateofReport=" + txtDateOfReport.Text + "&Filter1=" + ddlFilter1.SelectedValue + "" +
                                        "&Filter2=" + ddlFilter2.SelectedValue + "&Filter3=" + ddlFilter3.SelectedValue + "&X=" + strX + "&Y=" + dt.Rows[i]["Y_ID"] + "', 'FILE'," +
                                        "'location=0,status=0,scrollbars=1,resizable=1,width=1000," +
                                        "height=650');return false;\">" +
                                        "<a style='color: darkblue !important; font-weight: bold;' href='#' >" + strValue + "</a>" +
                                        "</td>";
                                }

                                //html += "<td style='background-color:" + strCellColorStyle + "; cursor: pointer; border-right-style: none !important; border-top-style: none !important;" + strTableCellLeft + "'" +
                                //        "onclick=\"window.open(" +
                                //        "'DetailedReport.aspx?ReportType=" + ddlReportType.SelectedValue + "&DateofReport=" + txtDateOfReport.Text + "&Filter1=" + ddlFilter1.SelectedValue + "" +
                                //        "&Filter2=" + ddlFilter2.SelectedValue + "&Filter3=" + ddlFilter3.SelectedValue + "&X=" + strX + "&Y=" + dt.Rows[i]["Y_ID"] + "', 'FILE'," +
                                //        "'location=0,status=0,scrollbars=1,resizable=1,width=1000," +
                                //        "height=650');return false;\">" +
                                //        "<a style='color: darkblue !important; font-weight: bold;' href='#' >" + strValue + "</a>" +
                                //        "</td>";
                            }
                            else
                            {
                                if (j == 0)
                                    html += "<td style='background-color:#FFFFFF;" + strTableCellLeft + "; border-top-style:none !important; border-right-style:none !important'>" + strValue.ToString() + "</td>";
                                else
                                {
                                    //if (dt.Columns[j].ColumnName.Contains("Percentage"))
                                    //    html += "<td style='background-color:" + strCellColorStyle + ";" + strTableCellLeft + "; border-top-style:none !important; border-left-style:none !important'>" + strValue.ToString() + "</td>";
                                    //else
                                    //    html += "<td style='background-color:" + strCellColorStyle + ";" + strTableCellLeft + "; border-top-style:none !important;'>" + strValue.ToString() + "</td>";

                                    html += "<td style='background-color:" + strCellColorStyle + ";" + strTableCellLeft + "; border-top-style:none !important; border-left-style:none !important'>" + strValue.ToString() + "</td>";
                                }
                            }

                            if (dt.Columns[j].ColumnName.Contains("Percentage"))
                                intColor++;
                        }

                        html += "</tr>";
                        inti = 0;
                    }

                    html += "</table>";

                    litSummary.Text = html;

                    Session["ViewReport"] = html;

                    btnExportToExcel.Visible = true;
                }
                else
                {
                    litSummary.Text = "<div class=\"container\"><span style=\"COLOR: red; font-size: 17px; font-family: Trebuchet MS;\">No records found...</span>";
                    Session["ViewReport"] = "";
                    btnExportToExcel.Visible = false;
                }
            }
            catch (Exception ex)
            {
                writeError("Exception in " + MethodBase.GetCurrentMethod().Name + "(): " + ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            writeError("");

            DataTable dt = new DataTable();
            string strReportTypeId = "", strCreateBy = "";

            strReportTypeId = ddlReportType.SelectedValue;

            if (Session["UserName"] != null)
                strCreateBy = Session["UserName"].ToString();
            else
                strCreateBy = Authentication.getUnAuthUserDetsFromLDAP(Page.User.Identity.Name, "Name");

            try
            {
                dt = ReportsBLL.getReportData(ddlReportType.SelectedValue, txtDateOfReport.Text, "", "", "", mstrConnectionString);
                if (dt.Rows.Count > 0)
                {
                    getReport(strReportTypeId);
                }
                else
                {
                    ReportsBLL.generateReports(txtDateOfReport.Text, strReportTypeId, ddlFilter1.SelectedValue, ddlFilter2.SelectedValue, ddlFilter3.SelectedValue,
                        strCreateBy, mstrConnectionString);

                    getReport(strReportTypeId);
                }

                //qaBL.generateReports(txtDateOfReport.Text, strReportTypeId, ddlFilter1.SelectedValue, ddlFilter2.SelectedValue, ddlFilter3.SelectedValue,
                //        strCreateBy, mstrConnectionString);

                //getReport(strReportTypeId);
            }
            catch (Exception ex)
            {
                writeError("Exception in " + MethodBase.GetCurrentMethod().Name + "(): " + ex.Message);
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            string strHTMLReport = Session["ViewReport"].ToString();
            string attachment = "attachment; filename=ReportFor_" + txtDateOfReport.Text.Replace("-", "") + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write(Regex.Replace(strHTMLReport.ToString(), "</?(a|A).*?>", ""));
            Response.End();
        }
    }
}