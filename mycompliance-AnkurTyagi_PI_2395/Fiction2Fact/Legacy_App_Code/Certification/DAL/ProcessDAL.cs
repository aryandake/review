using Fiction2Fact.App_Code;
using System.Data;
using static Fiction2Fact.App_Code.F2FDatabase;

namespace Fiction2Fact.Legacy_App_Code.Certification.BLL
{
    public class ProcessDAL
    {
        public DataSet validateUploadedCertData(string strBatchId, string strType, string strCircId, string mstrConnectionString)
        {
            DataSet ds = new DataSet();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "CERT_validateCertificationChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@BatchId", F2FDbType.VarChar, strBatchId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CircId", F2FDbType.Int32, (strCircId.Equals("") ? "0" : strCircId)));
                    DB.F2FDataAdapter.Fill(ds);
                }
                return ds;
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return ds;
        }

        //<< Added by Amarjeet on 27-Jul-2021
        public void uploadChecklistData(string strBatchId, string strType, int intCircId, string strLoggedInUser)
        {
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.OpenConnection();
                    DB.F2FCommand.CommandText = "CERT_uploadCertificationChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;

                    DB.F2FCommand.Parameters.Add(F2FParameter("@BatchId", F2FDbType.VarChar, strBatchId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircId", F2FDbType.Int32, intCircId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LoggedInUsername", F2FDbType.VarChar, strLoggedInUser));

                    DB.F2FCommand.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
        }
        //>>

        public DataSet validateFraudData(string mstrConnectionString)
        {
            DataSet ds = new DataSet();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "validateFraudData";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FDataAdapter.Fill(ds);
                }
                return ds;
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return ds;
        }


        public DataSet validateSecActionableData(string mstrConnectionString)
        {
            DataSet ds = new DataSet();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "validateSecActionableData";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FDataAdapter.Fill(ds);
                }
                return ds;
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return ds;
        }

        //<< Added by Ramesh More on 04-Jul-2024 CR_2114
        public DataTable validateUploadedData(string strBatchId, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "CERT_validateImportCertChklistData";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@BatchId", F2FDbType.VarChar, strBatchId));
                    DB.F2FDataAdapter.Fill(dt);
                }
                return dt;
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return dt;
        }
        public string uploadCertChecklistData(string strBatchId, string strLoggedInUser)
        {
            string retVal = "";
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CERT_uploadCertChecklistData";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@BatchId", F2FDbType.VarChar, strBatchId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LoggedInUserId", F2FDbType.VarChar, strLoggedInUser));

                    DB.F2FCommand.ExecuteScalar();
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
        //>>

    }
}
