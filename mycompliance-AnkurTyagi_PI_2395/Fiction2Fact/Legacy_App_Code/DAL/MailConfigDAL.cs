using System;
using System.Data;
using Fiction2Fact.App_Code;
using static Fiction2Fact.App_Code.F2FDatabase;

namespace Fiction2Fact.Legacy_App_Code.DAL
{
    public class MailConfigDAL
    {
        //<< Added by Supriya on 28-Jan-2013
        public DataTable searchMailConfig(string strMCId, string strType = null, string strModuleName = null, string mstrConnectionString = null)
        {
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "searchMailConfig";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@MCId", F2FDbType.VarChar, strMCId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ModuleName", F2FDbType.VarChar, strModuleName));

                    DataTable dt = new DataTable();
                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in searchMailConfig() " + ex);
            }
            finally
            {
                //myconnection.Close();
            }
        }

        public int saveMailConfigDetails(int intMCId, string strType, string strFrom,
                                            string strTo, string strCC, string strBCC, string strStatus, string strSubject,
                                            string strContent, string strCreateBy, string strModuleName, string mstrConnectionString)
        {
            int retVal = 0;
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "saveMailConfigDetails";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@MCId", F2FDbType.Int32, intMCId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@From", F2FDbType.VarChar, strFrom));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@To", F2FDbType.VarChar, strTo));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CC", F2FDbType.VarChar, strCC));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@BCC", F2FDbType.VarChar, strBCC));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Subject", F2FDbType.VarChar, strSubject));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Content", F2FDbType.VarChar, strContent));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ModuleName", F2FDbType.VarChar, strModuleName));
                    DB.OpenConnection();
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                }

            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in saveMailConfigDetails() " + ex);
            }
            finally
            {
                //myconnection.Close();
            }
            return retVal;
        }

        public int deleteMailConfig(int intMCId, string strCreateBy, string mstrConnectionString)
        {
            int retVal = 0;
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "deleteMailConfig";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@MCId", F2FDbType.Int32, intMCId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    retVal = DB.F2FCommand.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in deleteMailConfig() " + ex);
            }
            finally
            {
                //myconnection.Close();
            }
            return retVal;
        }

        public DataTable getMailConfigType(string mstrConnectionString)
        {
            string strSql = "Select MCM_TYPE from TBL_MAIL_CONFIG_MAS order by MCM_TYPE ";

            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.F2FCommand.CommandText = strSql;
                DataTable dt = new DataTable();
                DB.F2FDataAdapter.Fill(dt);
                return dt;
            }
        }
        //>>
    }
}
