using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Legacy_App_Code.Reports.DAL
{
    public class ReportsDAL
    {
        //<< get report on view
        public DataTable getReportData(string strReportTypeId, string strAsOnDate, string strFilter1, string strFilter2,
                string strFilter3, string strConnectionString)
        {
            DataTable dt = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase("getReportData"))
                {
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ReportTypeId", F2FDatabase.F2FDbType.VarChar, strReportTypeId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@AsOnDate", F2FDatabase.F2FDbType.VarChar, strAsOnDate));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter1", F2FDatabase.F2FDbType.VarChar, strFilter1));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter2", F2FDatabase.F2FDbType.VarChar, strFilter2));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter3", F2FDatabase.F2FDbType.VarChar, strFilter3));
                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception("Exception in " + MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        //>>

        //<< For poupup after clicking popup
        public DataTable getDetailedReportData(string strReportTypeId, string strAsOnDate, string strFilter1, string strFilter2,
                string strFilter3, string strXAxis, string strYAxis, string strY2Axis, string strConnectionString)
        {
            DataTable dt = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase("getDetailedReport"))
                {
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ReportTypeId", F2FDatabase.F2FDbType.VarChar, strReportTypeId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@AsOnDate", F2FDatabase.F2FDbType.VarChar, strAsOnDate));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter1", F2FDatabase.F2FDbType.VarChar, strFilter1));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter2", F2FDatabase.F2FDbType.VarChar, strFilter2));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter3", F2FDatabase.F2FDbType.VarChar, strFilter3));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@XAxisValue", F2FDatabase.F2FDbType.VarChar, strXAxis));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@YAxisValue", F2FDatabase.F2FDbType.VarChar, strYAxis));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Y2AxisValue", F2FDatabase.F2FDbType.VarChar, strY2Axis));
                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception("Exception in " + MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
        }
        //>>

        //<< get Report Type dropdown
        public DataTable getReportType(string strReportTypeId, string strType, string mstrConnectionString)
        {
            DataTable dtResults = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase("getReportType"))
                {
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ReportTypeId", F2FDatabase.F2FDbType.VarChar, strReportTypeId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Type", F2FDatabase.F2FDbType.VarChar, strType));
                    DB.F2FDataAdapter.Fill(dtResults);
                    return dtResults;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("system exception in getReportType(): " + ex.Message);
            }
        }
        //>>

        //<< generate report
        public void generateReports(string strAsOnDate, string strReportTypeId, string strFilter1, string strFilter2, string strFilter3, string strCreateBy,
                                    string mstrConnectionString)
        {
            try
            {
                using (F2FDatabase DB = new F2FDatabase("generate_Reports"))
                {
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ReportTypeId", F2FDatabase.F2FDbType.VarChar, strReportTypeId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@AsOnDate", F2FDatabase.F2FDbType.VarChar, strAsOnDate));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CreateBy", F2FDatabase.F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter_1_Value", F2FDatabase.F2FDbType.VarChar, strFilter1));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter_2_Value", F2FDatabase.F2FDbType.VarChar, strFilter2));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter_3_Value", F2FDatabase.F2FDbType.VarChar, strFilter3));
                    DB.OpenConnection();
                    DB.F2FCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in " + MethodBase.GetCurrentMethod().Name + "(): " + ex.Message);
            }
        }
        //>>
        //<< Added by Vivek on 15-Jun-2020
        public DataSet getDashboardData(string strReportTypeId, string strAsOnDate, string strFilter1, string strFilter2,
            string strFilter3, string strConnectionString)
        {
            DataSet ds = new DataSet();
            try
            {
                using (F2FDatabase DB = new F2FDatabase("getChartData"))
                {
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ReportTypeId", F2FDatabase.F2FDbType.VarChar, strReportTypeId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@AsOnDate", F2FDatabase.F2FDbType.VarChar, strAsOnDate));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter1", F2FDatabase.F2FDbType.VarChar, strFilter1));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter2", F2FDatabase.F2FDbType.VarChar, strFilter2));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter3", F2FDatabase.F2FDbType.VarChar, strFilter3));
                    DB.F2FDataAdapter.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("system Exception in getDashboardData(): " + ex);
            }
        }
        //>>

    }
}