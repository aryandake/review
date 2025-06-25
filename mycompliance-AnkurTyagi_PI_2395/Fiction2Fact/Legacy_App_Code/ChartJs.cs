using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Fiction2Fact.Legacy_App_Code
{
    public class ChartJs
    {
        public class GraphDatasets
        {
            public string label { get; set; }
            public string labelCode { get; set; }
            public string backgroundColor { get; set; }
            public string dataValues { get; set; }
        }

        public string getChartScripts(string strType, string strCanvas, List<GraphDatasets> lstDatasets)
        {
            StringBuilder sbScript = new StringBuilder();

            string strLabel = "", strBackgroundColors = "", strDataValues = "";

            try
            {
                for (int i = 0; i < lstDatasets.Count; i++)
                {
                    GraphDatasets graphDatasets = lstDatasets[i];
                    strLabel = (string.IsNullOrEmpty(strLabel) ? "" : strLabel + ",") + "'" + graphDatasets.label + "'";
                    strDataValues = (string.IsNullOrEmpty(strDataValues) ? "" : strDataValues + ",") + graphDatasets.dataValues;
                    strBackgroundColors = (string.IsNullOrEmpty(strBackgroundColors) ? "" : strBackgroundColors + ",") + "'" + graphDatasets.backgroundColor + "'";
                }

                //sbScript.Append("<script type=\"text/javascript\">");
                sbScript.Append("    let " + strCanvas + " = new Chart(document.getElementById('" + strCanvas + "'), {");
                sbScript.Append("        type: '" + strType + "',");
                sbScript.Append("        data: {");
                sbScript.Append("            labels: [" + strLabel + "],");
                sbScript.Append("            datasets: [{");
                sbScript.Append("                data: [" + strDataValues + "],");
                sbScript.Append("                backgroundColor: [" + strBackgroundColors + "],");
                sbScript.Append("                hoverOffset: 4");
                sbScript.Append("            }]");
                sbScript.Append("        },");
                sbScript.Append("        options: {");

                if (strType.Equals("bar"))
                {
                    sbScript.Append("            scales: {");
                    sbScript.Append("                y: {");
                    sbScript.Append("                    beginAtZero: true");
                    sbScript.Append("                },");
                    sbScript.Append("                xAxes: {");
                    sbScript.Append("                    display: false");
                    sbScript.Append("                }");
                    sbScript.Append("            },");
                }

                sbScript.Append("            plugins: {");
                sbScript.Append("                legend: {");
                sbScript.Append("                    labels: {");
                sbScript.Append("                        boxWidth: 0,");
                sbScript.Append("                        boxHeight: 0");
                sbScript.Append("                    },");
                sbScript.Append("                    display: false");
                sbScript.Append("                }");
                sbScript.Append("            }");
                sbScript.Append("        }");
                sbScript.Append("    });");
                //sbScript.Append("</script>");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sbScript.ToString();
        }

        public string getChartHTML(string strCanvas, List<GraphDatasets> lstDatasets)
        {
            StringBuilder sbHTML = new StringBuilder();

            try
            {
                sbHTML.Append("<div class=\"third widget\">");
                sbHTML.Append("    <h3>" + strCanvas.Substring(0, strCanvas.Length - 2) + "</h3>");
                sbHTML.Append("    <div class=\"canvas-container\">");
                sbHTML.Append("        <canvas id=\"" + strCanvas + "\"></canvas>");
                sbHTML.Append("    </div>");
                sbHTML.Append("    <div class=\"chart-legend\">");
                sbHTML.Append("        <ul>");

                for (int i = 0; i < lstDatasets.Count; i++)
                {
                    GraphDatasets graphDatasets = lstDatasets[i];
                    sbHTML.Append("            <li class=\"val" + graphDatasets.labelCode + "\">" + graphDatasets.label + "</li>");
                }

                sbHTML.Append("        </ul>");
                sbHTML.Append("    </div>");
                sbHTML.Append("</div>");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sbHTML.ToString();
        }

        public string getChartTableHTML(string strModule, List<GraphDatasets> lstDatasets)
        {
            StringBuilder sbHTML = new StringBuilder();
            int intTotal = 0;
            string strTotalLink = "";

            try
            {
                sbHTML.Append("<div class=\"third widget\">");
                sbHTML.Append("    <h3>" + strModule + "</h3>");
                sbHTML.Append("    <div class=\"canvas-container\">");
                sbHTML.Append("        <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");

                for (int i = 0; i < lstDatasets.Count; i++)
                {
                    GraphDatasets graphDatasets = lstDatasets[i];

                    string strLink = "", strCode = "";
                    int dataValue = 0;
                    dataValue = (string.IsNullOrEmpty(graphDatasets.dataValues) ? 0 : Convert.ToInt32(graphDatasets.dataValues));

                    if (strModule.Equals("Circular"))
                    {
                        strCode = graphDatasets.labelCode.Equals("1") ? "CWD" : (graphDatasets.labelCode.Equals("2") ? "CAD" : (graphDatasets.labelCode.Equals("3") ? "ND" : (graphDatasets.labelCode.Equals("4") ? "DNS" : "")));
                        strLink = "<a href=\"#\" onclick=\"window.open('../Circulars/DetailedReportCircular.aspx?ReportType=1&Status=" + strCode + "','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" + dataValue + "</a>";
                    }
                    else if (strModule.Equals("Filings"))
                    {
                        strCode = graphDatasets.labelCode.Equals("1") ? "Y" : (graphDatasets.labelCode.Equals("2") ? "N" : (graphDatasets.labelCode.Equals("3") ? "ND" : (graphDatasets.labelCode.Equals("4") ? "DN" : "")));
                        strLink = "<a href=\"#\" onclick=\"window.open('../Submissions/DetailedReport.aspx?ReportType=2&Status=" + strCode + "','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" + dataValue + "</a>";
                    }
                    else if (strModule.Equals("Certifications"))
                    {
                        strCode = graphDatasets.labelCode.Equals("1") ? "C" : (graphDatasets.labelCode.Equals("2") ? "N" : (graphDatasets.labelCode.Equals("3") ? "NA" : (graphDatasets.labelCode.Equals("4") ? "W" : "")));
                        strLink = "<a href=\"#\" onclick=\"window.open('../Certification/CertDashboardDets.aspx?ReportType=2&Type=" + strCode + "','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" + dataValue + "</a>";
                    }

                    sbHTML.Append("            <tr>");
                    sbHTML.Append("                <td class=\"DBTableFirstCellRight\" style=\"text-align: left; background-color: " + graphDatasets.backgroundColor + "; width: 80%;\">" + graphDatasets.label + "</td>");
                    sbHTML.Append("                <td class=\"DBTableCellRight\">" + strLink + "</td>");
                    sbHTML.Append("            </tr>");


                    intTotal += dataValue;
                }

                if (strModule.Equals("Circular"))
                    strTotalLink = "<a href=\"#\" onclick=\"window.open('Projects/Circulars/DetailedReportCircular.aspx?ReportType=1&Status=Total','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" + intTotal + "</a>";
                else if (strModule.Equals("Filings"))
                    strTotalLink = "<a href=\"#\" onclick=\"window.open('Projects/Submissions/DetailedReport.aspx?ReportType=2','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" + intTotal + "</a>";
                else if (strModule.Equals("Certifications"))
                    strTotalLink = "<a href=\"#\" onclick=\"window.open('Projects/Certification/CertDashboardDets.aspx?ReportType=2','FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" + intTotal + "</a>";

                sbHTML.Append("            <tr>");
                sbHTML.Append("                <td class=\"DBTableFirstCellRight\" style=\"text-align: left; background-color: #627b86; width: 80%;\">Total</td>");
                sbHTML.Append("                <td class=\"DBTableCellRight\">" + strTotalLink + "</td>");
                sbHTML.Append("            </tr>");
                sbHTML.Append("        </table>");
                sbHTML.Append("    </div>");
                sbHTML.Append("</div>");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sbHTML.ToString();
        }
    }
}