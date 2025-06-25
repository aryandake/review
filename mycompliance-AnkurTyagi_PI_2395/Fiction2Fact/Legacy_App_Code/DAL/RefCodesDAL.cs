using System;
using System.Data;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Legacy_App_Code.DAL
{
    public class RefCodesDAL
    {
        public DataTable searchRefCode(string strRefID, string strRefType, string strRefName, string strRefCode,
                                string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "searchRefCode";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@strRefID", F2FDatabase.F2FDbType.VarChar, strRefID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@strRefType", F2FDatabase.F2FDbType.VarChar, strRefType));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@strRefName", F2FDatabase.F2FDbType.VarChar, strRefName));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@strRefCode", F2FDatabase.F2FDbType.VarChar, strRefCode));
                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in searchRefCode() " + ex);
            }
            finally
            {
                //myconnection.Close();
            }
        }

        public int saveRefCode(int intRefID, string strRefType, string strRefName, string strRefCode,
                                    string strCreateBy, string strSortOrder, string strStatus)
        {
            int retVal = 0;
            try
            {
                using(F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "saveRefCode";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@intRefID", F2FDatabase.F2FDbType.Int32, intRefID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@strRefType", F2FDatabase.F2FDbType.VarChar, strRefType));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@strRefCode", F2FDatabase.F2FDbType.VarChar, strRefCode));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@strRefName", F2FDatabase.F2FDbType.VarChar, strRefName));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@strCreateBy", F2FDatabase.F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@strSortOrder", F2FDatabase.F2FDbType.VarChar, strSortOrder));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@strStatus", F2FDatabase.F2FDbType.VarChar, strStatus));
                    DB.OpenConnection();
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in insertRefCode() " + ex);
            }
            finally
            {
                //myconnection.Close();
            }
            return retVal;
        }

        public int deleteRefCode(int intRefID, string strCreateBy, string mstrConnectionString)
        {
            int retVal = 0;
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "deleteRefCode";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@intRefID", F2FDatabase.F2FDbType.Int32, intRefID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@strCreateBy", F2FDatabase.F2FDbType.VarChar, strCreateBy));
                    DB.OpenConnection();
                    retVal = DB.F2FCommand.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in deleteRefCode() " + ex);
            }
            finally
            {
                //myconnection.Close();
            }
            return retVal;
        }

        public DataTable getRefCodeDetails(string strRefType, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "getRefCodeDetails";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@strRefType", F2FDatabase.F2FDbType.VarChar, strRefType));
                    DataSet ds = new DataSet();
                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in getRefCodeDetails() " + ex);
            }
            finally
            {
                //myconnection.Close();
            }
        }


    }
}
