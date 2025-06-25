using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Fiction2Fact.Legacy_App_Code.Reports.DAL;

namespace Fiction2Fact.Legacy_App_Code.Reports.BLL
{
    public class ReportsBLL
    {
        ReportsDAL rdal = new ReportsDAL();

        public DataTable getReportData(string strReportTypeId, string strAsOnDate, string strFilter1, string strFilter2,
                string strFilter3, string strConnectionString = null)
        {
            DataTable dt = new DataTable();
            dt = rdal.getReportData(strReportTypeId, strAsOnDate, strFilter1, strFilter2, strFilter3, strConnectionString);
            return dt;
        }

        public DataTable getDetailedReportData(string strReportTypeId, string strAsOnDate, string strFilter1, string strFilter2,
                string strFilter3, string strXAxis, string strYAxis, string strY2Axis, string strConnectionString = null)
        {
            DataTable dt = new DataTable();
            dt = rdal.getDetailedReportData(strReportTypeId, strAsOnDate, strFilter1, strFilter2, strFilter3, strXAxis, strYAxis, strY2Axis, strConnectionString);
            return dt;
        }

        //<< Added by Vivek on 15-Jun-2020
        public DataSet getDashboardData(string strReportTypeId, string strAODMId, string strFilter1, string strFilter2,
            string strFilter3, string strConnectionString = null)
        {
            DataSet ds = new DataSet();
            ds = rdal.getDashboardData(strReportTypeId, strAODMId, strFilter1, strFilter2, strFilter3, strConnectionString);
            return ds;
        }

        public DataTable getReportType(string strReportTypeId, string strType, string mstrConnectionString = null)
        {
            DataTable dtResults = new DataTable();
            dtResults = rdal.getReportType(strReportTypeId, strType, mstrConnectionString);
            return dtResults;
        }

        public void generateReports(string strAsOnDate, string strReportTypeId, string strFilter1, string strFilter2, string strFilter3, string strCreateBy, string mstrConnectionString = null)
        {
            rdal.generateReports(strAsOnDate, strReportTypeId, strFilter1, strFilter2, strFilter3, strCreateBy, mstrConnectionString);
        }
        //>>
    }
}