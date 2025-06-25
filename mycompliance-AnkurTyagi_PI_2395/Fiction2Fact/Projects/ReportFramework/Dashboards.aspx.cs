using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Reflection;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using System.Text;
using System.Collections.Generic;

namespace Fiction2Fact.Projects.Reports
{
    public partial class Dashboards : System.Web.UI.Page
    {
        RefCodesBLL rcBL = new RefCodesBLL();
        ReportsBLL reportsBLL = new ReportsBLL();
        ChartJs chartJs = new ChartJs();
        string strChartHTML = "", strChartScript = "", strChartTable = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                strChartScript += "<script type=\"text/javascript\">";
                strChartHTML += "<div>";
                generateChartHTML("Circular", "doughnut");
                generateChartHTML("Filings", "doughnut");
                generateChartHTML("Certifications", "doughnut");
                strChartHTML += "</div>";

                strChartHTML += "<div>";
                generateChartHTML("Circular", "bar");
                generateChartHTML("Filings", "bar");
                generateChartHTML("Certifications", "bar");
                strChartHTML += "</div>";
                strChartScript += "</script>";

                strChartTable += "<div>";
                generateTableHTML("Circular");
                generateTableHTML("Filings");
                generateTableHTML("Certifications");
                strChartTable += "</div>";

                litChart.Text = strChartHTML + "" + strChartScript + "" + strChartTable;
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        private void generateChartHTML(string strModule, string strChartType)
        {
            try
            {
                List<ChartJs.GraphDatasets> lstGraphDatasets = getListOfGraphDatasets(strModule);

                strChartHTML += chartJs.getChartHTML(strModule + (strChartType.Equals("bar") ? "BC" : "DC"), lstGraphDatasets);
                strChartScript += chartJs.getChartScripts(strChartType, strModule + (strChartType.Equals("bar") ? "BC" : "DC"), lstGraphDatasets);
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        private void generateTableHTML(string strModule)
        {
            try
            {
                List<ChartJs.GraphDatasets> lstGraphDatasets = getListOfGraphDatasets(strModule);

                strChartTable += chartJs.getChartTableHTML(strModule, lstGraphDatasets);
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        private List<ChartJs.GraphDatasets> getListOfGraphDatasets(string strModule)
        {
            List<ChartJs.GraphDatasets> lstGraphDatasets = new List<ChartJs.GraphDatasets>();
            string strLableCode = "", strLables = "", strBackgroundColors = "", strDataValues = "";
            try
            {
                DataTable dtLables = rcBL.getRefCodeDetails("Dashboard - " + strModule + " - Labels");

                for (int i = 0; i < dtLables.Rows.Count; i++)
                {
                    DataRow drLables = dtLables.Rows[i];
                    strLableCode = drLables["RC_CODE"].ToString();
                    strLables = drLables["RC_NAME"].ToString();

                    if (strLableCode.Equals("1"))
                        strBackgroundColors = "#009933";
                    else if (strLableCode.Equals("2"))
                        strBackgroundColors = "rgb(255, 205, 86)";
                    else if (strLableCode.Equals("3"))
                        strBackgroundColors = "rgb(54, 162, 235)";
                    else if (strLableCode.Equals("4"))
                        strBackgroundColors = "rgb(255, 99, 132)";

                    if (!strLableCode.Equals(""))
                        strDataValues = reportsBLL.getCountForSingleDashboard(strModule, strLableCode);

                    ChartJs.GraphDatasets objGraphDataset = new ChartJs.GraphDatasets();
                    objGraphDataset.label = strLables;
                    objGraphDataset.labelCode = strLableCode;
                    objGraphDataset.backgroundColor = strBackgroundColors;
                    objGraphDataset.dataValues = strDataValues;
                    lstGraphDatasets.Add(objGraphDataset);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstGraphDatasets;
        }
    }
}