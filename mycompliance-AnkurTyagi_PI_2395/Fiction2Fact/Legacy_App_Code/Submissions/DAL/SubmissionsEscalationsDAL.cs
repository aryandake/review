using Fiction2Fact.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using static Fiction2Fact.App_Code.F2FDatabase;

namespace Fiction2Fact.Legacy_App_Code.Submissions.DAL
{
    public class SubmissionsEscalationsDAL
    {
        public int SaveSubmissionsEscalationsMas(int intId, string strFirstName, string strLastName, string strMiddelName, string strEmailId, int intType, string strLevels, string strCreateBy, string strConnectionString, string strEscalationType,string strReportingDept)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                DB.F2FCommand.Transaction = DB.F2FTransaction;
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_insertSubmissionsEscalations";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.VarChar, intId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstName", F2FDbType.VarChar, strFirstName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LastName", F2FDbType.VarChar, strLastName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@MiddelName", F2FDbType.VarChar, strMiddelName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EmailId", F2FDbType.VarChar, strEmailId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type1", F2FDbType.Int32, intType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Levels", F2FDbType.VarChar, strLevels));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EscalationType", F2FDbType.VarChar, strEscalationType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ReportingDept", F2FDbType.VarChar, strReportingDept));
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                    DB.F2FTransaction.Commit();

                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Commit();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retVal;
        }

        public DataSet SearchSubmissionsEscalations(string strDepartment, string strLevel, string strType, string mstrConnectionString, string strReportingDept)
        {
            DataSet dsResults = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_SearchSubmissionsEscalations";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Level", F2FDbType.VarChar, strLevel));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Department", F2FDbType.VarChar, strDepartment));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ReportingDept", F2FDbType.VarChar, strReportingDept));
                    DB.F2FDataAdapter.Fill(dsResults);
                    return dsResults;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dsResults;
        }
    }
}
