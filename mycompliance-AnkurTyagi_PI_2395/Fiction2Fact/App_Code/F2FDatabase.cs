//using Fiction2Fact.Legacy_App_Code;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI;

namespace Fiction2Fact.App_Code
{
    public class F2FDatabase : IDisposable
    {
        private static string f2FDatabaseType;
        private string f2FConnectionString;
        private DbConnection f2FConnection;
        private DbCommand f2FCommand;
        private DbCommandBuilder f2FCommandBuilder;
        private DbDataAdapter f2FDataAdapter;
        private DbDataReader f2FDataReader;
        private DbParameter f2FDataParameter;
        private DbParameterCollection f2FDataParameterCollection;
        private DbTransaction f2FTransaction;

        public static string F2FDatabaseType
        {
            get { return f2FDatabaseType; }
            set { f2FDatabaseType = value; }
        }

        public string F2FConnectionString
        {
            get { return f2FConnectionString; }
            set { f2FConnectionString = value; }
        }


        public DbCommand F2FCommand
        {
            get { return f2FCommand; }
            set { f2FCommand = value; }
        }

        public DbCommandBuilder F2FCommandBuilder
        {
            get { return f2FCommandBuilder; }
            set { f2FCommandBuilder = value; }
        }

        public DbDataAdapter F2FDataAdapter
        {
            get { return f2FDataAdapter; }
            set { f2FDataAdapter = value; }
        }

        public DbDataReader F2FDataReader
        {
            get { return f2FDataReader; }
            set { f2FDataReader = value; }
        }

        public DbParameter F2FDataParameter
        {
            get { return f2FDataParameter; }
            set { f2FDataParameter = value; }
        }

        public DbParameterCollection F2FDataParameterCollection
        {
            get { return f2FDataParameterCollection; }
            set { f2FDataParameterCollection = value; }
        }

        public DbTransaction F2FTransaction
        {
            get { return f2FTransaction; }
            set { f2FTransaction = value; }
        }

        public DbConnection F2FConnection
        {
            get { return f2FConnection; }
            set { f2FConnection = value; }
        }

        public static Dictionary<string, int> F2FMsDbType
        {
            get { return f2FMsDbType; }
            set { f2FMsDbType = value; }
        }

        public static Dictionary<string, int> F2FMyDbType
        {
            get { return f2FMyDbType; }
            set { f2FMyDbType = value; }
        }

        public static string F2FSqlStartDelimiter
        {
            get
            {
                switch (F2FDatabaseType)
                {
                    case "MsSQL":
                        return "[";
                    case "MySQL":
                        return "`";
                    default:
                        return "";
                }
            }

        }

        public static string F2FSqlEndDelimiter
        {
            get
            {
                switch (F2FDatabaseType)
                {
                    case "MsSQL":
                        return "]";
                    case "MySQL":
                        return "`";
                    default:
                        return "";
                }
            }
        }

        public F2FDatabase(string sql = null)
        {
            F2FDatabaseType = ConfigurationManager.AppSettings["F2FDatabaseType"].ToString();
            F2FConnectionString = ConfigurationManager.ConnectionStrings[F2FDatabaseType].ToString();
            switch (F2FDatabaseType)
            {
                case "MsSQL":
                    F2FConnection = new SqlConnection(F2FConnectionString);
                    F2FCommand = new SqlCommand(sql, (SqlConnection)F2FConnection);
                    F2FDataAdapter = new SqlDataAdapter((SqlCommand)F2FCommand);
                    F2FDataParameter = new SqlParameter();
                    F2FCommandBuilder = new SqlCommandBuilder((SqlDataAdapter)F2FDataAdapter);
                    break;
                case "MySQL":
                    F2FConnection = new MySqlConnection(F2FConnectionString);
                    F2FCommand = new MySqlCommand(sql, (MySqlConnection)F2FConnection);
                    F2FDataAdapter = new MySqlDataAdapter((MySqlCommand)F2FCommand);
                    F2FDataParameter = new MySqlParameter();
                    F2FCommandBuilder = new MySqlCommandBuilder((MySqlDataAdapter)F2FDataAdapter);
                    break;
            }
        }

        static F2FDatabase()
        {
            F2FDatabaseType = ConfigurationManager.AppSettings["F2FDatabaseType"].ToString();
        }

        public static DbParameter F2FParameter(string paramName, string dbType, object paramValue)
        {
            DbParameter param = null;
            switch (F2FDatabaseType)
            {
                case "MsSQL":
                    param = new SqlParameter(paramName, (SqlDbType)F2FMsDbType[dbType.ToString()]);
                    param.Value = paramValue;
                    break;
                case "MySQL":
                    param = new MySqlParameter(paramName, (MySqlDbType)F2FMyDbType[dbType.ToString()]);
                    if (dbType.Equals(F2FDbType.DateTime))
                        param.Value = CommonCodes.dispToDbDateTime(paramValue);
                    else
                    {
                        param.Value = paramValue;
                    }
                    break;
            }
            return param;
        }

        public static DbParameter F2FParameter(string paramName, string dbType, object paramValue, ParameterDirection paramDirection = ParameterDirection.Input, int size = 0)
        {
            DbParameter param = null;
            switch (F2FDatabaseType)
            {
                case "MsSQL":
                    param = new SqlParameter(paramName, (SqlDbType)f2FMsDbType[dbType.ToString()]);
                    param.Value = paramValue;
                    if (size != 0) { param.Size = size; }
                    param.Direction = paramDirection;
                    break;
                case "MySQL":
                    param = new MySqlParameter(paramName, (MySqlDbType)f2FMyDbType[dbType.ToString()]);
                    param.Value = paramValue;
                    if (size != 0) { param.Size = size; }
                    param.Direction = paramDirection;
                    break;
            }
            return param;
        }

        public static DbParameter F2FParameter(string paramName, F2FDbType dbType)
        {
            switch (F2FDatabaseType)
            {
                case "MsSQL":
                    return new SqlParameter(paramName, F2FMsDbType[dbType.ToString()]);
                default:
                    return new MySqlParameter(paramName, F2FMyDbType[dbType.ToString()]);
            }
        }
        
        public static DataTable getDataTable(string sQuery)
        {
            DataTable dtReturn = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase(sQuery))
                {
                    DB.F2FDataAdapter.Fill(dtReturn);
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return dtReturn;
        }

        public static DataSet getDataSet(string sQuery)
        {
            DataSet dtReturn = new DataSet();
            try
            {
                using (F2FDatabase DB = new F2FDatabase(sQuery))
                {
                    DB.F2FDataAdapter.Fill(dtReturn);
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return dtReturn;
        }

        public static object ExecuteNonQuery(string sQuery, CommandType ctType = CommandType.Text)
        {
            object objReturn = null;
            try
            {
                using (F2FDatabase DB = new F2FDatabase(sQuery))
                {
                    DB.F2FCommand.CommandType = ctType;
                    DB.OpenConnection();
                    objReturn = DB.F2FCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return objReturn;
        }
        public static object ExecuteScalar(string sQuery, CommandType ctType = CommandType.Text)
        {
            object objReturn = null;
            try
            {
                using (F2FDatabase DB = new F2FDatabase(sQuery))
                {
                    DB.F2FCommand.CommandType = ctType;
                    DB.OpenConnection();
                    objReturn = DB.F2FCommand.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return objReturn;
        }

        public class F2FDbType
        {
            public static string Int32 = "F2FInt32";
            public static string Int64 = "F2FInt64";
            public static string VarChar = "F2FVarChar";
            public static string nVarChar = "F2FnVarChar";
            public static string Decimal = "F2FDecimal";
            public static string DateTime = "F2FDateTime";
        };

        public static object F2FExecuteNonQuery(string strSql)
        {
            object objResult = null;
            using (F2FDatabase DB = new F2FDatabase(strSql))
            {
                DB.OpenConnection();
                objResult = DB.F2FCommand.ExecuteNonQuery();
            }
            return objResult;
        }
        public static object F2FExecuteScalar(string strSql)
        {
            object objResult = null;
            using (F2FDatabase DB = new F2FDatabase(strSql))
            {
                DB.OpenConnection();
                objResult = DB.F2FCommand.ExecuteScalar();
            }
            return objResult;
        }
        public static DataTable F2FGetDataTable(string strSql, CommandType cmdType = CommandType.Text)
        {
            DataTable dtResult = new DataTable();
            using (F2FDatabase DB = new F2FDatabase(strSql))
            {
                DB.F2FCommand.CommandType = cmdType;
                DB.F2FDataAdapter.Fill(dtResult);
            }
            return dtResult;
        }
        public static DataSet F2FGetDataSet(string strSql, CommandType cmdType = CommandType.Text)
        {
            DataSet dtResult = new DataSet();
            using (F2FDatabase DB = new F2FDatabase(strSql))
            {
                DB.F2FCommand.CommandType = cmdType;
                DB.F2FDataAdapter.Fill(dtResult);
            }
            return dtResult;
        }

        public DbConnection OpenConnection()
        {
            if (F2FConnection.State != ConnectionState.Open)
            {
                F2FConnection.ConnectionString = F2FConnectionString;
                F2FConnection.Open();
            }
            return F2FConnection;
        }
        public void CloseConnection()
        {
            if (F2FConnection.State != ConnectionState.Closed)
            {
                F2FConnection.Close();
            }
            F2FConnection.Dispose();
        }

        private static Dictionary<string, int> f2FMsDbType = new Dictionary<string, int>()
        {
            {"F2FInt32",8},
            {"F2FInt64",8},
            {"F2FVarChar",22},
            {"F2FnVarChar",12},
            {"F2FDecimal",5},
            {"F2FDateTime",4}
        };
        private static Dictionary<string, int> f2FMyDbType = new Dictionary<string, int>()
        {
            {"F2FInt32",3},
            {"F2FInt64",8},
            {"F2FVarChar",254},
            {"F2FnVarChar",254},
            {"F2FDecimal",0},
            {"F2FDateTime",12}
        };

        ~F2FDatabase()
        {
            Dispose(true);
        }
        public void Dispose()
        {
            Dispose(true);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (F2FTransaction != null)
                    {
                        F2FTransaction.Dispose();
                    }
                }
                //F2FTransaction.Dispose();
                F2FCommandBuilder.Dispose();
                F2FCommand.Dispose();
                if (F2FConnection != null)
                {
                    F2FConnection.Dispose();
                }
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~F2FDatabase() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.

            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }


}