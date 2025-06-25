using Fiction2Fact.Legacy_App_Code.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Fiction2Fact.Legacy_App_Code.BLL
{
    public class ReportsBLL
    {
        ReportsDAL reportDAL = new ReportsDAL();
        public DataTable getReportData(string strReportTypeId, string strAsOnDate, string strFilter1, string strFilter2,

        string strFilter3, string strConnectionString)
        {
            return reportDAL.getReportData(strReportTypeId, strAsOnDate, strFilter1, strFilter2, strFilter3, strConnectionString);
        }

        public DataTable getDetailedReportData(string strReportTypeId, string strAsOnDate, string strFilter1, string strFilter2,
                string strFilter3, string strXAxis, string strYAxis, string strConnectionString)
        {
            return reportDAL.getDetailedReportData(strReportTypeId, strAsOnDate, strFilter1, strFilter2, strFilter3, strXAxis, strYAxis, strConnectionString);
        }

        public DataTable getReportType(string strReportTypeId, string strType, string mstrConnectionString)
        {
            return reportDAL.getReportType(strReportTypeId, strType, mstrConnectionString);
        }

        public void generateReports(string strAsOnDate, string strReportTypeId, string strFilter1, string strFilter2, string strFilter3, string strCreateBy,
                                    string mstrConnectionString)
        {
            reportDAL.generateReports(strAsOnDate, strReportTypeId, strFilter1, strFilter2, strFilter3, strCreateBy, mstrConnectionString);
        }

        public string getCountForSingleDashboard(string strModule, string strType, string strUsername = null, string strEmailId = null, string UserRole = null)
        {
            return reportDAL.getCountForSingleDashboard(strModule, strType, strUsername, strEmailId, UserRole);
        }
    }
}