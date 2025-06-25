using Fiction2Fact.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using static Fiction2Fact.App_Code.F2FDatabase;

namespace Fiction2Fact.Legacy_App_Code.DAL
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
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "getReportData";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ReportTypeId", F2FDbType.VarChar, strReportTypeId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@AsOnDate", F2FDbType.VarChar, strAsOnDate));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter1", F2FDbType.VarChar, strFilter1));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter2", F2FDbType.VarChar, strFilter2));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter3", F2FDbType.VarChar, strFilter3));
                    DB.F2FDataAdapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return dt;
        }
        //>>

        //<< For poupup after clicking popup
        public DataTable getDetailedReportData(string strReportTypeId, string strAsOnDate, string strFilter1, string strFilter2,
                string strFilter3, string strXAxis, string strYAxis, string strConnectionString)
        {
            DataTable dt = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "getDetailedReport";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ReportTypeId", F2FDbType.VarChar, strReportTypeId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@AsOnDate", F2FDbType.VarChar, strAsOnDate));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter1", F2FDbType.VarChar, strFilter1));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter2", F2FDbType.VarChar, strFilter2));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter3", F2FDbType.VarChar, strFilter3));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@XAxisValue", F2FDbType.VarChar, strXAxis));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@YAxisValue", F2FDbType.VarChar, strYAxis));
                    DB.F2FDataAdapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return dt;
        }
        //>>

        //<< get Report Type dropdown
        public DataTable getReportType(string strReportTypeId, string strType, string mstrConnectionString)
        {
            DataTable dtResults = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "getReportType";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ReportTypeId", F2FDbType.VarChar, strReportTypeId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FDataAdapter.Fill(dtResults);
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return dtResults;
        }
        //>>

        //<< generate report
        public void generateReports(string strAsOnDate, string strReportTypeId, string strFilter1, string strFilter2, string strFilter3, string strCreateBy,
                                    string mstrConnectionString)
        {
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "generate_Reports";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ReportTypeId", F2FDbType.VarChar, strReportTypeId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@AsOnDate", F2FDbType.VarChar, strAsOnDate));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter_1_Value", F2FDbType.VarChar, strFilter1));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter_2_Value", F2FDbType.VarChar, strFilter2));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter_3_Value", F2FDbType.VarChar, strFilter3));
                    DB.OpenConnection();
                    DB.F2FCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
        }
        //>>

        public string getCountForSingleDashboard(string strModule, string strType, string strUsername, string strEmailId, string strUserRole)
        {
            string retVal = "";

            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();

                try
                {
                    DB.F2FCommand.CommandText = "getCountForSingleDashboard";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Module", F2FDbType.VarChar, strModule));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Username", F2FDbType.VarChar, strUsername));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EmailId", F2FDbType.VarChar, strEmailId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UserRole", F2FDbType.VarChar, strUserRole));

                    retVal = DB.F2FCommand.ExecuteScalar().ToString();
                }
                catch (Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }

            return retVal;
        }
    }
}