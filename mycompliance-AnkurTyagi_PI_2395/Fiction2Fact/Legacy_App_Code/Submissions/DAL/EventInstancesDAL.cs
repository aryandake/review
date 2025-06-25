using Fiction2Fact.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using static Fiction2Fact.App_Code.F2FDatabase;

namespace Fiction2Fact.Legacy_App_Code.Submissions.DAL
{
    public class EventInstancesDAL
    {
        public int SaveEventInstances(int intId, string strEvent, string strCompany, string strDateOfEvent, string strDescription,
                                        string strCreateBy, DataTable AssociateWithdt, string mstrConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                DB.F2FCommand.Transaction = DB.F2FTransaction;
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_InsertUpdateEventInstance";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, intId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Event", F2FDbType.Int32, Convert.ToInt32(strEvent)));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Company", F2FDbType.Int32, Convert.ToInt32(strCompany)));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DateOfEvent", F2FDbType.DateTime, strDateOfEvent));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Description", F2FDbType.VarChar, strDescription));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));

                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                    if (intId == 0)
                    {
                        insertUpdateEiEpMapping(retVal, AssociateWithdt, strCreateBy, DB);
                    }
                    else
                    {
                        insertUpdateEiEpMapping(intId, AssociateWithdt, strCreateBy, DB);
                    }
                    DB.F2FTransaction.Commit();
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retVal;
        }

        private void insertUpdateEiEpMapping(int intId, DataTable dt, string CreateBy, F2FDatabase DB)
        {
            int retVal;
            if (dt != null)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    DataRow dr = dt.Rows[i];
                    DB.F2FCommand.CommandText = "SUBM_InsertUpdateEiEpMapping";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Clear();
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, intId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@AgendaId", F2FDbType.Int32, dr["AgendaId"]));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, CreateBy));
                    retVal = DB.F2FCommand.ExecuteNonQuery();
                }
            }
        }

        public DataSet SearchEventInstances(int intId, int intEventType, string strEventDate, string mstrConnectionString)
        {
            DataSet dsResults = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_SearchEventInstances";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, intId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EventType", F2FDbType.Int32, intEventType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EventDate", F2FDbType.VarChar, strEventDate));
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

        public int deleteEventInstances(int intID, string strConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                DB.F2FCommand.Transaction = DB.F2FTransaction;
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_DeleteEventInstances";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, intID));

                    retVal = DB.F2FCommand.ExecuteNonQuery();

                    DB.F2FTransaction.Commit();
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retVal;
        }
    }
}
