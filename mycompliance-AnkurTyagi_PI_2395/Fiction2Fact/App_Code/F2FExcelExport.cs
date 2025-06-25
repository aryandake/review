using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact.App_Code
{
    public static class F2FExcelExport
    {
        /// <summary>
        /// This function is to get Dataset containing tables as per pagination and single table in dataset if pagnation is disabled
        /// </summary>
        /// <param name="gvExport">Gridview to be exported to Excel</param>
        /// <param name="arrHideCols">Column indexes as Array to skip in export</param>
        /// <param name="bPagination">Enable disable pagination</param>
        /// <returns></returns>
        public static DataSet GetDataFromGridForExcelExport(GridView gvExport, int[] arrHideCols = null, bool bPagination = false)
        {
            DataSet dsExport = new DataSet();
            gvExport.AllowPaging = bPagination;
            int iRowCnt = 1;
            for (int iPage = 0; iPage < gvExport.PageCount; iPage++)
            {
                gvExport.PageIndex = iPage;
                gvExport.DataBind();
                //CommonCodes.PrepareGridViewForExport(gvExport);
                DataTable dt = new DataTable("Sheet" + (dsExport.Tables.Count + 1).ToString());
                //foreach (TableCell cell in gvContractDraftQueries.HeaderRow.Cells)
                //DataColumn dcSrNo = new DataColumn("Sr.No.", typeof(int));
                //dcSrNo.AutoIncrement = true;
                //dcSrNo.AutoIncrementSeed = iRowCnt;
                //dcSrNo.AutoIncrementStep = 1;
                //dt.Columns.Add(dcSrNo);
                for (int iCol = 0; iCol < gvExport.HeaderRow.Cells.Count; iCol++)
                {
                    if(iCol < gvExport.Columns.Count)
                    {
                        string strHeaderText = gvExport.Columns[iCol].HeaderText;
                        string txtText = gvExport.HeaderRow.Cells[iCol].Text;
                        if (!arrHideCols.Contains(iCol) && gvExport.Columns[iCol].Visible)
                        {
                            string strColumnName = HttpUtility.HtmlDecode(string.IsNullOrEmpty(strHeaderText) ? txtText : strHeaderText);
                            if (!dt.Columns.Contains(strColumnName)) dt.Columns.Add(strColumnName);
                        }
                    }
                }
                foreach (GridViewRow row in gvExport.Rows)
                {
                    dt.Rows.Add();
                    int iCol = 0;
                    for (int j = 0; j < row.Cells.Count; j++)
                    {
                        if (j < gvExport.Columns.Count && iCol < dt.Columns.Count)
                        {
                            if (!arrHideCols.Contains(j) && gvExport.Columns[j].Visible)
                            {
                                dt.Rows[dt.Rows.Count - 1][iCol++] = HttpUtility.HtmlDecode(GetGridViewCellContent(row.Cells[j]));
                            }
                        }
                    }
                    iRowCnt++;
                }
                dsExport.Tables.Add(dt);
            }
            return dsExport;
        }

        /// <summary>
        /// Get Content of GridViewCell
        /// </summary>
        /// <param name="dcfCell">GridViewCell</param>
        /// <returns></returns>
        public static string GetGridViewCellContent(Control dcfCell)
        {
            string strReturn = "";
            if (dcfCell.HasControls())
            {
                foreach(Control ctrl in dcfCell.Controls)
                {
                    if (ctrl.HasControls()) { strReturn += GetGridViewCellContent(ctrl); }
                    else { strReturn += GetControlText(ctrl); }
                }
            }else
            {
                strReturn += GetControlText(dcfCell);
            }
            return strReturn;
        }

        /// <summary>
        /// Get Plain text from GridViewCell
        /// </summary>
        /// <param name="ctlControl">Control to Extract Text from</param>
        /// <returns></returns>
        public static string GetControlText(Control ctlControl)
        {
            string strReturn = "";
            if (ctlControl.GetType() == typeof(LinkButton))
            {
                LinkButton ctr = (LinkButton)ctlControl;
                strReturn = ctr.Visible ? ctr.Text : "";
            }
            else if (ctlControl.GetType() == typeof(HyperLink))
            {
                HyperLink ctr = (HyperLink)ctlControl;
                strReturn = ctr.Visible ? ctr.Text : "";
            }
            else if (ctlControl.GetType() == typeof(TextBox))
            {
                TextBox ctr = (TextBox)ctlControl;
                strReturn = ctr.Visible ? ctr.Text : "";
            }
            else if (ctlControl.GetType() == typeof(CheckBox))
            {
                CheckBox ctr = (CheckBox)ctlControl;
                strReturn = ctr.Visible ? ctr.Text : "";
            }
            else if (ctlControl.GetType() == typeof(Label))
            {
                Label ctr = (Label)ctlControl;
                strReturn = ctr.Visible ? ctr.Text : "";
            }
            else if(ctlControl.GetType() == typeof(DataControlFieldCell))
            {
                DataControlFieldCell ctr = (DataControlFieldCell)ctlControl;
                strReturn = ctr.Visible ? ctr.Text : "";
            }
            else if(ctlControl.GetType() == typeof(DataBoundLiteralControl))
            {
                DataBoundLiteralControl ctr = (DataBoundLiteralControl)ctlControl;
                strReturn = ctr.Visible ? ctr.Text : "";
            }
            else if(ctlControl.GetType() == typeof(LiteralControl))
            {
                LiteralControl ctr = (LiteralControl)ctlControl;
                strReturn = ctr.Visible ? ctr.Text : "";
            }
            else if(ctlControl.GetType() == typeof(Literal))
            {
                Literal ctr = (Literal)ctlControl;
                strReturn = ctr.Visible ? ctr.Text : "";
            }
            else if (ctlControl.GetType() == typeof(DataList))
            {
                DataList gvChlid = (DataList)ctlControl;
                List<string> lstFiles = new List<string>();
                CommonCodes.PrepareChlidGridView(gvChlid, ref lstFiles);
                foreach (string str in lstFiles)
                {
                    strReturn += (string.IsNullOrEmpty(strReturn) ? "" : Environment.NewLine) + str;
                }
                
            }
            return CommonCodes.GetPlainTextFromHTML(strReturn);
        }

        /// <summary>
        /// This function is to Export GridView to Excel (.xlsx), Export Excel file will be sent to browser.
        /// </summary>
        /// <param name="gvToExport">GridView to export to Excel</param>
        /// <param name="sFileName">File name of exported file, Default is "Export.xlsx"</param>
        /// <param name="arrHideCols">Integer array of Column indexes to be hide(not to export) in Excel, Default all visible columns will be exported</param>
        /// <param name="bPagination">Export with or without pagination, Default is false</param>
        public static string F2FExportGridViewToExcel(GridView gvToExport, string sFileName = null, int[] arrHideCols = null, bool bPagination = false)
        {
            string strRetMsg = "";
            try
            {
                arrHideCols = arrHideCols ?? new int[0];
                sFileName = sFileName + ".xlsx" ?? "Export.xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    DataSet dsExport = GetDataFromGridForExcelExport(gvToExport, arrHideCols, bPagination);
                    foreach (DataTable dt in dsExport.Tables)
                    {
                        var ws = wb.Worksheets.Add(dt);
                        ws.Columns(1, dt.Columns.Count).AdjustToContents();
                    }
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    string attach = "attachment;filename=" + sFileName;
                    HttpContext.Current.Response.AddHeader("content-disposition", attach);
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.End();
                    }
                }
                strRetMsg = sFileName + " exported successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return strRetMsg;
        }
    }
}