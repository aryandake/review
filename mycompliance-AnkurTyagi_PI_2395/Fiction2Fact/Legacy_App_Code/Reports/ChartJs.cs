using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Fiction2Fact.Legacy_App_Code.Reports
{
    public class ChartJs
    {
        //<< Code to create Dataset
        public class GraphDatasets
        {
            public string label { get; set; }
            public string backgroundColor { get; set; }
            public string borderColor { get; set; }

            public string dataValues { get; set; }
            public List<string> arrDataValues { get; set; }
        }
        //>>

        /// <summary>
        /// Below are the Type of charts can be generated from generateCharts() methods - having multiple background color:
        /// 1) bar
        /// 2) pie
        /// 3) polarArea
        /// 4) doughnut
        /// 5) horizontalBar
        /// </summary>
        /// <param name="strChartId"></param>
        /// <param name="strChartType"></param>
        /// <param name="strChartTitle"></param>
        /// <param name="displayLegend"></param>
        /// <param name="displayTitle"></param>
        /// <param name="strDataLabel"></param>
        /// <param name="arrBackgroundColor"></param>
        /// <param name="arrNames"></param>
        /// <param name="arrValues"></param>
        /// <returns></returns>
        public string generateCharts(string strChartId, string strChartType, string strChartTitle, bool displayLegend, bool displayTitle,
            string strDataLabel, string[] arrBackgroundColor, string[] arrNames, string[] arrValues)
        {
            string strChart = "", strNames = "", strBackgroundColors = "", strValues = "";
            try
            {
                if (arrBackgroundColor.Length == arrNames.Length && arrNames.Length == arrValues.Length)
                {
                    for (int i = 0; i < arrBackgroundColor.Length; i++)
                    {
                        strBackgroundColors = strBackgroundColors + "\"" + arrBackgroundColor[i] + "\", ";
                        strNames = strNames + "\"" + arrNames[i] + "\", ";
                        strValues = strValues + arrValues[i] + ", ";
                    }
                }
                else
                {
                    for (int i = 0; i < arrBackgroundColor.Length; i++)
                    {
                        strBackgroundColors = strBackgroundColors + "\"" + arrBackgroundColor[i] + "\", ";
                    }

                    for (int j = 0; j < arrNames.Length; j++)
                    {
                        strNames = strNames + "\"" + arrNames[j] + "\", ";
                        strValues = strValues + arrValues[j] + ", ";
                    }
                }

                if (!strBackgroundColors.Equals(""))
                    strBackgroundColors = strBackgroundColors.Substring(0, strBackgroundColors.Length - 2);

                if (!strNames.Equals(""))
                    strNames = strNames.Substring(0, strNames.Length - 2);

                if (!strValues.Equals(""))
                    strValues = strValues.Substring(0, strValues.Length - 2);

                //<< Code to create javascript
                strChart = " <script type=\"text/javascript\"> " +
                           " new Chart(document.getElementById(\"" + strChartId + "\"), {" +
                           " type: '" + strChartType + "', data: {" +
                           " labels: [" + strNames + "], " +
                           " datasets: [ {" +
                           " label: \"" + strDataLabel + "\", " +
                           " backgroundColor: [" + strBackgroundColors + "], " +
                           " data: [" + strValues + "] " +
                           " } ] " +
                           " }, " +
                           " options: { " +
                           " legend: { display: " + displayLegend.ToString().ToLower() + " }, " +
                           " title: { " +
                           " display: " + displayTitle.ToString().ToLower() + ", " +
                           " text: '" + strChartTitle + "' " +
                           " } } " +
                           " }); " +
                           " </script> ";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strChart;
        }

        /// <summary>
        /// Below are the Type of charts can be generated from generateCharts() methods - having single background color:
        /// 1) bar
        /// 2) pie
        /// 3) polarArea
        /// 4) doughnut
        /// 5) horizontalBar
        /// </summary>
        /// <param name="strChartId"></param>
        /// <param name="strChartType"></param>
        /// <param name="strChartTitle"></param>
        /// <param name="displayLegend"></param>
        /// <param name="displayTitle"></param>
        /// <param name="strDataLabel"></param>
        /// <param name="strBackgroundColor"></param>
        /// <param name="arrNames"></param>
        /// <param name="arrValues"></param>
        /// <returns></returns>
        public string generateCharts(string strChartId, string strChartType, string strChartTitle, bool displayLegend, bool displayTitle,
            string strDataLabel, string strBackgroundColor, string[] arrNames, string[] arrValues)
        {
            string strChart = "", strNames = "", strValues = "";
            try
            {
                for (int j = 0; j < arrNames.Length; j++)
                {
                    strNames = strNames + "\"" + arrNames[j] + "\", ";
                    strValues = strValues + arrValues[j] + ", ";
                }

                if (!strNames.Equals(""))
                    strNames = strNames.Substring(0, strNames.Length - 2);

                if (!strValues.Equals(""))
                    strValues = strValues.Substring(0, strValues.Length - 2);

                //<< Code to create javascript
                strChart = " <script type=\"text/javascript\"> " +
                           " new Chart(document.getElementById(\"" + strChartId + "\"), {" +
                           " type: '" + strChartType + "', data: {" +
                           " labels: [" + strNames + "], " +
                           " datasets: [ {" +
                           " label: \"" + strDataLabel + "\", " +
                           " backgroundColor: " + strBackgroundColor + ", " +
                           " data: [" + strValues + "] " +
                           " } ] " +
                           " }, " +
                           " options: { " +
                           " legend: { display: " + displayLegend.ToString().ToLower() + " }, " +
                           " title: { " +
                           " display: " + displayTitle.ToString().ToLower() + ", " +
                           " text: '" + strChartTitle + "' " +
                           " } } " +
                           " }); " +
                           " </script> ";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strChart;
        }

        /// <summary>
        /// Below are the Type of charts can be generated from generateCharts() methods - having multiple background color and datatable of Name-Value pair:
        /// 1) bar
        /// 2) pie
        /// 3) polarArea
        /// 4) doughnut
        /// 5) horizontalBar
        /// </summary>
        /// <param name="strChartType"></param>
        /// <param name="strChartId"></param>
        /// <param name="strChartTitle"></param>
        /// <param name="displayLegend"></param>
        /// <param name="displayTitle"></param>
        /// <param name="strDataLabel"></param>
        /// <param name="arrBackgroundColor"></param>
        /// <param name="dtLists"></param>
        /// <returns></returns>
        public string generateCharts(string strChartId, string strChartType, string strChartTitle, bool displayLegend, bool displayTitle,
            string strDataLabel, string[] arrBackgroundColor, DataTable dtLists)
        {
            DataRow dr;
            string strChart = "", strNames = "", strBackgroundColors = "", strValues = "";
            try
            {
                for (int i = 0; i < arrBackgroundColor.Length; i++)
                {
                    strBackgroundColors = strBackgroundColors + "\"" + arrBackgroundColor[i] + "\",";
                }

                if (dtLists.Rows.Count > 0)
                {
                    for (int j = 0; j < dtLists.Rows.Count; j++)
                    {
                        dr = dtLists.Rows[j];

                        strNames = strNames + "\"" + dr[0].ToString() + "\",";
                        strValues = strValues + dr[1].ToString() + ",";
                    }
                }

                if (!strBackgroundColors.Equals(""))
                    strBackgroundColors = strBackgroundColors.Substring(0, strBackgroundColors.Length - 1);

                if (!strNames.Equals(""))
                    strNames = strNames.Substring(0, strNames.Length - 1);

                if (!strValues.Equals(""))
                    strValues = strValues.Substring(0, strValues.Length - 1);

                //<< Code to create javascript
                strChart = " <script type=\"text/javascript\"> " +
                           " new Chart(document.getElementById(\"" + strChartId + "\"), {" +
                           " type: '" + strChartType + "', data: {" +
                           " labels: [" + strNames + "], " +
                           " datasets: [ {" +
                           " label: \"" + strDataLabel + "\", " +
                           " backgroundColor: [" + strBackgroundColors + "], " +
                           " data: [" + strValues + "] " +
                           " } ] " +
                           " }, " +
                           " options: { " +
                           " legend: { display: " + displayLegend.ToString().ToLower() + " }, " +
                           " title: { " +
                           " display: " + displayTitle.ToString().ToLower() + ", " +
                           " text: '" + strChartTitle + "' " +
                           " } } " +
                           " }); " +
                           " </script> ";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strChart;
        }

        /// <summary>
        /// Below are the Type of charts can be generated from generateCharts() methods - having single background color and datatable of Name-Value pair:
        /// 1) bar
        /// 2) pie
        /// 3) polarArea
        /// 4) doughnut
        /// 5) horizontalBar
        /// </summary>
        /// <param name="strChartType"></param>
        /// <param name="strChartId"></param>
        /// <param name="strChartTitle"></param>
        /// <param name="displayLegend"></param>
        /// <param name="displayTitle"></param>
        /// <param name="strDataLabel"></param>
        /// <param name="strBackgroundColor"></param>
        /// <param name="dtLists"></param>
        /// <returns></returns>
        public string generateCharts(string strChartId, string strChartType, string strChartTitle, bool displayLegend, bool displayTitle,
            string strDataLabel, string strBackgroundColor, DataTable dtLists)
        {
            DataRow dr;
            string strChart = "", strNames = "", strValues = "";
            try
            {
                if (dtLists.Rows.Count > 0)
                {
                    for (int j = 0; j < dtLists.Rows.Count; j++)
                    {
                        dr = dtLists.Rows[j];

                        strNames = strNames + "\"" + dr[0].ToString() + "\",";
                        strValues = strValues + dr[1].ToString() + ",";
                    }
                }

                if (!strNames.Equals(""))
                    strNames = strNames.Substring(0, strNames.Length - 1);

                if (!strValues.Equals(""))
                    strValues = strValues.Substring(0, strValues.Length - 1);

                //<< Code to create javascript
                strChart = " <script type=\"text/javascript\"> " +
                           " new Chart(document.getElementById(\"" + strChartId + "\"), {" +
                           " type: '" + strChartType + "', data: {" +
                           " labels: [" + strNames + "], " +
                           " datasets: [ {" +
                           " label: \"" + strDataLabel + "\", " +
                           " backgroundColor: [" + strBackgroundColor + "], " +
                           " data: [" + strValues + "] " +
                           " } ] " +
                           " }, " +
                           " options: { " +
                           " legend: { display: " + displayLegend.ToString().ToLower() + " }, " +
                           " title: { " +
                           " display: " + displayTitle.ToString().ToLower() + ", " +
                           " text: '" + strChartTitle + "' " +
                           " } } " +
                           " }); " +
                           " </script> ";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strChart;
        }

        /// <summary>
        /// This method can be used to generate the group bar chart
        /// </summary>
        /// <param name="strChartType"></param>
        /// <param name="strChartId"></param>
        /// <param name="strChartTitle"></param>
        /// <param name="displayLegend"></param>
        /// <param name="displayTitle"></param>
        /// <param name="arrNames"></param>
        /// <param name="lstDatasets"></param>
        /// <returns></returns>
        public string generateGroupBarCharts(string strChartId, string strChartType, string strChartTitle, bool displayLegend, bool displayTitle,
            string[] arrNames, List<GraphDatasets> lstDatasets)
        {
            string strChart = "", strNames = "", strValues = "", strLabels = "", strBackgroundColor = "", strBorderColor = "",
                strDatasets = "";
            try
            {
                for (int j = 0; j < arrNames.Length; j++)
                {
                    strNames = strNames + "\"" + arrNames[j] + "\", ";
                }

                if (!strNames.Equals(""))
                    strNames = strNames.Substring(0, strNames.Length - 2);

                strChart = " <script type=\"text/javascript\"> " +
                           " new Chart(document.getElementById(\"" + strChartId + "\"), {" +
                           " type: '" + strChartType + "', data: {" +
                           " labels: [" + strNames + "], ";

                if (lstDatasets.Count > 0)
                {
                    strChart += " datasets: [ ";

                    strDatasets = "";
                    for (int i = 0; i < lstDatasets.Count; i++)
                    {
                        GraphDatasets objDatasets = lstDatasets[i];
                        strLabels = objDatasets.label;
                        strBackgroundColor = objDatasets.backgroundColor;
                        strBorderColor = objDatasets.borderColor;

                        //strDatasets += " { " +
                        //              " label: \"" + strLabels + "\", " +
                        //              " backgroundColor: \"" + strBackgroundColor + "\",borderColor: \"#f51b00\", ";

                        strDatasets += " { " +
                                      " label: \"" + strLabels + "\", " +
                                      " backgroundColor: \"" + strBackgroundColor + "\"";

                        if (strChartType == "line")
                        {
                            strDatasets += ",borderColor: \"" + strBorderColor + "\", ";
                        }

                        List<string> lstValues = objDatasets.arrDataValues;
                        if (lstValues.Count > 0)
                        {
                            strDatasets += " data: [";

                            strValues = "";
                            for (int k = 0; k < lstValues.Count; k++)
                            {
                                //DataValues objValue = lstValues[k];

                                strValues += lstValues[k] + ",";
                            }

                            if (!strValues.Equals(""))
                                strValues = strValues.Substring(0, strValues.Length - 1);

                            strDatasets += strValues + "], fill: false,";
                        }

                        strDatasets += " },";
                    }

                    if (!strDatasets.Equals(""))
                        strDatasets = strDatasets.Substring(0, strDatasets.Length - 1);

                    strChart += strDatasets + " ] ";
                }

                strChart += " }, " +
                            " options: { " +
                            " legend: { display: " + displayLegend.ToString().ToLower() + " }, " +
                            " title: { " +
                            " display: " + displayTitle.ToString().ToLower() + ", " +
                            " text: '" + strChartTitle + "' " +
                            " } } " +
                            " }); " +
                            " </script> ";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strChart;
        }

        /// <summary>
        /// This method can be used to generate the Stacked bar chart
        /// </summary>
        /// <param name="strChartType"></param>
        /// <param name="strChartId"></param>
        /// <param name="strChartTitle"></param>
        /// <param name="displayLegend"></param>
        /// <param name="displayTitle"></param>
        /// <param name="arrNames"></param>
        /// <param name="lstDatasets"></param>
        /// <returns></returns>
        public string generateStackedBarCharts(string strChartId, string strChartType, string strChartTitle, bool displayLegend, bool displayTitle,
            string[] arrNames, List<GraphDatasets> lstDatasets)
        {
            string strChart = "", strNames = "", strValues = "", strLabels = "", strBackgroundColor = "",
                strDatasets = "";
            try
            {
                for (int j = 0; j < arrNames.Length; j++)
                {
                    strNames = strNames + "\"" + arrNames[j] + "\", ";
                }

                if (!strNames.Equals(""))
                    strNames = strNames.Substring(0, strNames.Length - 2);

                strChart = " <script type=\"text/javascript\"> " +
                           " new Chart(document.getElementById(\"" + strChartId + "\"), {" +
                           " type: '" + strChartType + "', data: {" +
                           " labels: [" + strNames + "], ";

                if (lstDatasets.Count > 0)
                {
                    strChart += " datasets: [ ";

                    strDatasets = "";
                    for (int i = 0; i < lstDatasets.Count; i++)
                    {
                        GraphDatasets objDatasets = lstDatasets[i];
                        strLabels = objDatasets.label;
                        strBackgroundColor = objDatasets.backgroundColor;
                        strValues = objDatasets.dataValues;

                        strDatasets += " { " +
                                      " label: \"" + strLabels + "\", " +
                                      " backgroundColor: \"" + strBackgroundColor + "\", " +
                                      " data: [" + strValues + "]";

                        #region //<< Commented Code
                        /*
                        List<string> lstValues = objDatasets.arrDataValues;
                        if (lstValues.Count > 0)
                        {
                            strDatasets += " data: [";

                            strValues = "";
                            for (int k = 0; k < lstValues.Count; k++)
                            {
                                strValues += lstValues[k] + ",";
                            }

                            if (!strValues.Equals(""))
                                strValues = strValues.Substring(0, strValues.Length - 1);

                            strDatasets += strValues + "]";
                        }
                        */
                        #endregion

                        strDatasets += " },";
                    }

                    if (!strDatasets.Equals(""))
                        strDatasets = strDatasets.Substring(0, strDatasets.Length - 1);

                    strChart += strDatasets + " ] ";
                }

                strChart += " }, " +
                            " options: { " +
                            " legend: { display: " + displayLegend.ToString().ToLower() + " }, " +
                            " title: { " +
                            " display: " + displayTitle.ToString().ToLower() + ", " +
                            " text: '" + strChartTitle + "' " +
                            " }, " +
                            " responsive: true, " +
                            " scales: { " +
                            " xAxes: [{ stacked: true }], " +
                            " yAxes: [{ stacked: true }] " +
                            " }" +
                            " } " +
                            " }); " +
                            " </script> ";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strChart;
        }

        /// <summary>
        /// Below are the Type of charts can be generated from generateChartsWithDataLists() methods - having single background color 
        /// and can be used with row data:
        /// 1) bar
        /// 2) pie
        /// 3) polarArea
        /// 4) doughnut
        /// 5) horizontalBar
        /// </summary>
        public string generateChartsWithDataLists(string strChartId, string strChartType, string strChartTitle, bool displayLegend,
            bool displayTitle, string strDataLabel, string strBackgroundColor, DataTable dtLists, string strXAxis, string strYAxis)
        {
            DataRow dr;
            string strChart = "", strNames = "", strValues = "";
            try
            {
                if (dtLists.Rows.Count > 0)
                {
                    for (int j = 0; j < dtLists.Rows.Count; j++)
                    {
                        dr = dtLists.Rows[j];

                        strNames = strNames + "\"" + dr[0].ToString() + "\",";
                        strValues = strValues + dr[1].ToString() + ",";
                    }
                }

                if (!strNames.Equals(""))
                    strNames = strNames.Substring(0, strNames.Length - 1);

                if (!strValues.Equals(""))
                    strValues = strValues.Substring(0, strValues.Length - 1);

                //<< Code to create javascript
                strChart = " <script type=\"text/javascript\"> " +
                           " new Chart(document.getElementById(\"" + strChartId + "\"), {" +
                           " type: '" + strChartType + "', data: {" +
                           " labels: [" + strNames + "], " +
                           " datasets: [ {" +
                           " label: \"" + strDataLabel + "\", " +
                           " backgroundColor: [" + strBackgroundColor + "], " +
                           " data: [" + strValues + "] " +
                           " } ] " +
                           " }, " +
                           " options: { " +
                           " legend: { display: " + displayLegend.ToString().ToLower() + " }, " +
                           " title: { " +
                           " display: " + displayTitle.ToString().ToLower() + ", " +
                           " text: '" + strChartTitle + "' " +
                           " } } " +
                           " }); " +
                           " </script> ";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strChart;
        }

        /// <summary>
        /// This method can be used to generate below given type of charts with configurable option hanlded from database level
        /// ChartType => bar, horizontalBar, line, pie, doughnut
        /// </summary>
        /// <param name="strChartId"></param>
        /// <param name="strChartType"></param>
        /// <param name="strChartTitle"></param>
        /// <param name="strXAxisName"></param>
        /// <param name="strYAxisName"></param>
        /// <param name="strLabelNames"></param>
        /// <param name="lstDatasets"></param>
        /// <param name="strDataLabelColor"></param>
        /// <param name="strLegendPosition"></param>
        /// <param name="strChartDataType"></param>
        /// <param name="displayLegend"></param>
        /// <param name="displayTitle"></param>
        /// <returns></returns>
        public string generateAllTypeofCharts(string strChartId, string strChartType, string strChartTitle,
            string strXAxisName, string strYAxisName, string strLabelNames, List<GraphDatasets> lstDatasets,
            string strDataLabelColor, string strLegendPosition, string strChartDataType, bool displayLegend = true,
            bool displayTitle = true)
        {
            string strChart = "", strChartTypeId = "", strChartDatasets = "";
            try
            {
                strChartTypeId = getChartTypeId(strChartType);
                strChartDatasets = getChartDatasets(strChartType, strChartTypeId, lstDatasets);

                strChart = " <script type=\"text/javascript\"> " +
                            " new Chart(document.getElementById(\"" + strChartId + "\"), {" +
                            " type: '" + strChartTypeId + "', data: {" +
                            " labels: [" + strLabelNames + "], " +
                            " datasets: [ " + strChartDatasets + " ] " +
                            " } , " +
                            " options: " +
                            " { " +
                            "   responsive: true, " +
                            "   legend: " +
                            "   { " +
                            "       display: " + displayLegend.ToString().ToLower() + ", " +
                            "       position: '" + (strLegendPosition.Equals("") ? "top" : strLegendPosition) + "' " +
                            "   }, " +
                            "   title: " +
                            "   { " +
                            "       display: " + displayTitle.ToString().ToLower() + ", " +
                            "       text: '" + strChartTitle + "' " +
                            "   }, ";

                #region //<< Code for tooltip and animation
                if (strChartTypeId.ToLower().Equals("pie") || strChartTypeId.ToLower().Equals("doughnut") ||
                    strChartTypeId.ToLower().Equals("polararea"))
                {
                    strChart += "   animation: " +
                                "   { " +
                                "       animateScale: true, " +
                                "       animateRotate: true " +
                                "   }, " +
                                "   tooltips: " +
                                "   { " +
                                "       callbacks: " +
                                "       { " +
                                "           label: function(item, data) " +
                                "           { " +
                                "               console.log(data.labels, item); " +
                                "               return data.datasets[item.datasetIndex].label + \": \" +  " +
                                "               data.labels[item.index] + \": \" + data.datasets[item.datasetIndex].data[item.index]; " +
                                "           } " +
                                "       } " +
                                "   }, ";
                }
                else
                {
                    strChart += "   tooltips: " +
                                "   { " +
                                "       position: 'nearest', " + //nearest, average
                                "       mode: 'index', " + //index, point, x, y
                                "       intersect: false " +
                                "   }, ";
                }
                #endregion

                #region //<< Code to set scale at X-axis and y-axis
                strChart += "   scales: " +
                            "   { " +
                            "       xAxes: [{ " +
                            "                   display: true ";

                if (!strXAxisName.Equals(""))
                {
                    strChart += "               , scaleLabel: " +
                                "               { " +
                                "                   display: true," +
                                "                   labelString: '" + strXAxisName + "' " +
                                "               } ";
                }

                if (strChartType.Equals("VBC_S") || strChartType.Equals("HBC_S") || strChartType.Equals("CBLC_S"))
                {
                    strChart += ", stacked: true, " +
                                " categoryPercentage: 0.5, barPercentage: 0.7 ";
                }

                strChart += "              }], " +
                            "       yAxes: [{ " +
                            "                   display: true ";

                if (!strYAxisName.Equals(""))
                {
                    strChart += "               , scaleLabel: " +
                                "               { " +
                                "                   display: true," +
                                "                   labelString: '" + strYAxisName + "' " +
                                "               } ";
                }

                if (strChartType.Equals("VBC_S") || strChartType.Equals("HBC_S") || strChartType.Equals("CBLC_S"))
                {
                    strChart += ", stacked: true, " +
                                " categoryPercentage: 0.5, barPercentage: 0.7 ";
                }

                strChart += "              }], " +
                            "   }, ";
                #endregion

                #region //<< Code for Plugin of Datalabels
                strChart += "   plugins: " +
                            "   { " +
                            "       datalabels: " +
                            "       { " +
                            "           color: '" + strDataLabelColor + "', " +
                            "           display: function(context) { " +
                            "           return context.dataset.data[context.dataIndex] !== 0; " +
                            "           }, ";

                if (strChartType.Equals("VBC_NS") || strChartType.Equals("HBC_NS") || strChartType.Equals("CBLC_NS"))
                {
                    strChart += "align: 'top', " +
                            " anchor: 'top', ";
                }
                else
                {
                    strChart += "align: 'center', " +
                            " anchor: 'center', ";
                }

                if (strChartType.Equals("PC") || strChartType.Equals("DC"))
                {
                    //strChart += "       formatter: function (value, context) { " +
                    //            "       let sum = 0; " +
                    //            "       let dataArr = context.chart.data.datasets[0].data; " +
                    //            "       dataArr.map(data => { " +
                    //            "       sum += data; " +
                    //            "       }); " +
                    //            "       let percentage = (value*100 / sum).toFixed(2)+'%'; " +
                    //            "       return percentage ; ";

                    strChart += "       formatter: function (value, context) { " +
                                "       let sum = context.dataset._meta[0].total; " +
                                "       let percentage = (value * 100 / sum).toFixed(2) + '%'; " +
                                "       return percentage; ";
                }

                else
                {
                    if (strChartDataType.Equals("P"))
                    {
                        strChart += "       formatter: function(value){ " +
                                    "       return value + '%' ; ";
                    }
                    else
                    {
                        strChart += "       formatter: function(value){ " +
                                    "       return value ; ";
                    }
                }

                strChart += "           } " +
                            "       } " +
                            "   } ";
                #endregion

                strChart += " } " +
                            " }); " +
                            " </script> ";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strChart;
        }

        private string getChartTypeId(string strChartType)
        {
            string strChartTypeId = "";

            if (strChartType.Equals("VBC_NS") || strChartType.Equals("VBC_S") || strChartType.Equals("CBLC_NS") || strChartType.Equals("CBLC_S"))
                strChartTypeId = "bar";
            else if (strChartType.Equals("HBC_NS") || strChartType.Equals("HBC_S"))
                strChartTypeId = "horizontalBar";
            else if (strChartType.Equals("LC_WOF") || strChartType.Equals("LC_WF"))
                strChartTypeId = "line";
            else if (strChartType.Equals("PC"))
                strChartTypeId = "pie";
            else if (strChartType.Equals("DC"))
                strChartTypeId = "doughnut";

            return strChartTypeId;
        }

        private string getChartDatasets(string strChartType, string strChartTypeId, List<GraphDatasets> lstDatasets)
        {
            string strChartDataset = "", strLabels = "", strBackgroundColor = "", strValues = "", strSetLabel = "",
                strSetBackgroundColor = "", strSetBorderColor = "", strSetDataValues = "";

            #region //<< Code to get Datasets
            if (lstDatasets.Count > 0)
            {
                strChartDataset = "";
                for (int i = 0; i < lstDatasets.Count; i++)
                {
                    GraphDatasets objDatasets = lstDatasets[i];
                    strLabels = objDatasets.label;
                    strBackgroundColor = objDatasets.backgroundColor;
                    strValues = objDatasets.dataValues;

                    strSetLabel = " label: \"" + strLabels + "\", ";
                    strSetDataValues = " data: [" + strValues + "]";
                    strSetBorderColor = " borderColor: \"" + strBackgroundColor + "\", ";

                    //<< This is done to handle single or multiple background colors
                    if (strBackgroundColor.Contains(","))
                        strSetBackgroundColor = " backgroundColor: [" + strBackgroundColor + "], ";
                    else
                        strSetBackgroundColor = " backgroundColor: \"" + strBackgroundColor + "\", ";
                    //>>

                    if (strChartType.Contains("CBLC"))     //Combo Bar - Line Chart
                    {
                        strChartDataset += " { " +
                                           " type: 'line', " + strSetLabel +
                                           " fill: false, " + strSetBorderColor +
                                           strSetBackgroundColor + strSetDataValues +
                                           " }," +
                                           " { " +
                                           " type: 'bar', " + strSetLabel +
                                           strSetBackgroundColor + strSetDataValues +
                                           " },";
                    }
                    else
                    {
                        strChartDataset += " { " + strSetLabel;

                        if (strChartTypeId.Equals("line"))
                        {
                            strChartDataset += (strChartType.Equals("LC_WF") ? " fill: true, " : " fill: false, ");
                            strChartDataset += strSetBorderColor;
                        }

                        strChartDataset += strSetBackgroundColor + strSetDataValues + " },";
                    }
                }

                if (!strChartDataset.Equals(""))
                    strChartDataset = strChartDataset.Substring(0, strChartDataset.Length - 1);
            }
            #endregion

            return strChartDataset;
        }

    }
}