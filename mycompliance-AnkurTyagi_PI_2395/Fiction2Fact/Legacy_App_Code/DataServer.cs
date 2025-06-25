using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data.Common;
using System.Collections.Generic;
using System.Threading;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Legacy_App_Code
{
    public class DataServer
    {
        private static DbConnection _connection = null;
        protected static F2FDatabase DB = new F2FDatabase();
        public static DbConnection Connection
        {
            get
            {
                if (DataServer._connection == null || DataServer._connection.State == ConnectionState.Closed || DataServer._connection.State == ConnectionState.Broken)
                {
                    DataServer._connection = DB.OpenConnection();

                }
                return DataServer._connection;
            }
        }

        public DataTable Getdata(string sql)
        {
            DataSet ds = new DataSet();
            try
            {
                using (DB.OpenConnection())
                {
                    DB.F2FCommand.CommandText = sql;
                    DB.F2FDataAdapter.Fill(ds);
                }

                if (ds.Tables.Count == 0)
                {
                    throw new Exception("No Records Found");
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            finally
            {
                Connection.Close();
                Connection.Dispose();
            }

            return ds.Tables[0];
        }

        public static DataRow LoadRow(string TableName, string idCol, int value)
        {
            string sql = "Select * from " + TableName + " Where " + idCol + "=" + value.ToString();
            DataTable dt = new DataTable(TableName);
            using (DB.OpenConnection())
            {
                DB.F2FCommand.CommandText = sql;
            }
            DB.F2FDataAdapter.Fill(dt);

            if (dt.Rows.Count == 1)
            {
                return dt.Rows[0];
            }
            else
                return dt.NewRow();
        }

        public static void SaveRow(DataRow row, string tableName, F2FDatabase f2fDB, string primaryKeyCol, int objectIdentity)
        {
            try
            {
                DataTable dt = new DataTable(tableName);
                f2fDB.F2FDataAdapter.Fill(dt);

                if (objectIdentity > 0)
                {
                    dt.Select(primaryKeyCol + "=" + objectIdentity.ToString())[0].ItemArray = row.ItemArray;
                    LoadParameters(row, f2fDB);

                    f2fDB.F2FDataAdapter.AcceptChangesDuringUpdate = true;
                    f2fDB.F2FDataAdapter.Update(dt);
                }
                else
                {
                    //Check here
                    dt = row.Table;
                    //row.SetAdded();
                    dt.Rows.Add(row);

                    LoadParameters(row, f2fDB);

                    f2fDB.F2FDataAdapter.AcceptChangesDuringUpdate = true;
                    f2fDB.F2FDataAdapter.Update(dt);

                    row[primaryKeyCol] = DataServer.ExecuteScalar("Select IDENT_CURRENT('" + tableName + "')");
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") { Trace.Write(ex); throw ex; }

            }
        }

        public static void DeleteRow(DataRow row, string tableName, F2FDatabase f2fDB, string primaryKeyCol, int objectIdentity)
        {
            try
            {
                if (objectIdentity == 0) throw new Exception("Object Identity can not be 0");

                DataTable dt = new DataTable(tableName);
                f2fDB.F2FDataAdapter.Fill(dt);

                //dt.Select(primaryKeyCol + "=" + objectIdentity.ToString())[0].ItemArray = row.ItemArray;
                dt.Select(primaryKeyCol + "=" + objectIdentity.ToString())[0].Delete();
                LoadParameters(row, f2fDB);

                f2fDB.F2FDataAdapter.Update(dt.Select(null, null, DataViewRowState.Deleted));
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") { Trace.Write(ex); throw ex; }
            }
        }

        private static void LoadParameters(DataRow row, F2FDatabase f2fDB)
        {
            string colVar = "";
            SqlParameter param = null;
            foreach (DataColumn col in row.Table.Columns)
            {
                colVar = "@" + col.ColumnName;
                if (f2fDB.F2FCommand.CommandText.Contains(colVar))
                {
                    param = new SqlParameter();
                    //param.DbType = (DbType)col.DataType;

                    if (col.DataType.Name == "Byte[]")
                        param.DbType = DbType.Binary;

                    param.ParameterName = colVar;
                    param.Value = row[col.ColumnName] == null ? DBNull.Value : row[col.ColumnName];

                    //param = new SqlParameter(colVar, row[col.ColumnName] == null ? DBNull.Value : row[col.ColumnName]);

                    f2fDB.F2FCommand.Parameters.Add(param);
                }
            }
        }

        public static int ExecuteSql(string sql)
        {
            int i = 0;
            try
            {
                DB.F2FCommand.CommandText = sql;
                DB.OpenConnection();
                i = DB.F2FCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") { throw ex; }
            }
            finally
            {
                Connection.Close();
                Connection.Dispose();
            }
            return i;
        }

        public static int ExecuteSql(string sql, F2FDatabase f2fDB)
        {
            int i = 0;
            try
            {
                DB.F2FCommand.CommandText = sql;
                DB.OpenConnection();
                i = DB.F2FCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") { Trace.Write(ex); throw ex; }
            }
            finally
            {
                Connection.Close();
                Connection.Dispose();
            }
            return i;
        }

        public static object ExecuteScalar(string sql)
        {
            Object obj = null;
            try
            {
                DB.F2FCommand.CommandText = sql;
                DB.OpenConnection();
                obj = DB.F2FCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") { Trace.Write(ex); throw ex; }
            }
            finally
            {
                Connection.Close();
                Connection.Dispose();
            }
            return obj;
        }
    }

    public class ConnectionArgs
    {
        private string _connectionString = "";

        protected ConnectionArgs()
        {

        }

        public ConnectionArgs(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public string ConnectionString
        {
            get { return GetConnectionString(); }
        }

        /// <summary>
        /// This method is called from the ConnectionString's get accessor; override this method to return 
        /// custom connection string.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetConnectionString()
        {
            return _connectionString;
        }
    }

    public class Database
    {
        private int _transactionDepth = 0;
        public int TransactionDepth
        {
            get
            {
                return _transactionDepth;
            }
        }

        private ConnectionArgs _connectionArgs = null;

        public ConnectionArgs Args
        {
            get { return _connectionArgs; }
            set { _connectionArgs = value; }
        }

        public Database(ConnectionArgs args)
        {
            this.Args = args;
        }

        private void Initialise()
        {

        }

        public string ConnectionString
        {
            get { return Args.ConnectionString; }
        }

        public SqlConnection GetOpenConnection()
        {
            //SqlConnection genericDBConnection;
            //genericDBConnection = new SqlConnection(this.ConnectionString);
            //// genericDBConnection.ConnectionString = this.Args.ConnectionString;
            //genericDBConnection.Open();
            return (SqlConnection) DataServer.Connection;
        }

        public DbCommandBuilder GetCommandBuilder(SqlDataAdapter dataAdapter)
        {
            DbCommandBuilder result = null;
            {
                result = new SqlCommandBuilder(dataAdapter);
            }
            return result;
        }

        public static Database GetDefaultDatabaseInstance()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
            ConnectionArgs connectionArgs = new ConnectionArgs(connectionString);
            Database database = new Database(connectionArgs);
            return database;
        }

        #region Transaction Methods
        private static ActiveTransactions _activeTransactions = new ActiveTransactions();
        private static ActiveTransactions ActiveTransactions
        {
            get { return Database._activeTransactions; }
            set { Database._activeTransactions = value; }
        }

        private string GetTransactionSearchKey()
        {
            string format = "Thread:{0}, Connection:{1}";
            return string.Format(format, Thread.CurrentThread.ManagedThreadId.ToString(), this.ConnectionString);
        }

        public void BeginTransaction()
        {
            SqlTransaction transaction = null;

            if (!IsInComplusTransaction)
            {
                lock (Database._activeTransactions)
                {
                    string key = GetTransactionSearchKey();
                    if (!Database.ActiveTransactions.ContainsKey(key))
                    {
                        transaction = GetOpenConnection().BeginTransaction();
                        Database.ActiveTransactions.Add(GetTransactionSearchKey(), transaction);
                    }
                    else
                    {
                        SqlTransaction t = Database.ActiveTransactions[key];
                        transaction = GetOpenConnection().BeginTransaction();
                    }
                }
            }
            //return transaction;
        }

        public void CommitTransaction()
        {
            SqlTransaction t = GetTransaction();
            if (t != null)
            {
                t.Commit();
                if (this.TransactionDepth == 0)
                {
                    if (t.Connection.State == ConnectionState.Open)
                    {
                        t.Connection.Close();
                    }
                    Database.ActiveTransactions.Remove(GetTransactionSearchKey());
                }
            }
            else
            {
                throw new Exception("No active transaction found");
            }
        }

        public void RollBackTransaction()
        {
            SqlTransaction t = GetTransaction();
            if (t != null)
            {
                t.Rollback();
                if (t.Connection.State == ConnectionState.Open)
                {
                    t.Connection.Close();
                }
                Database.ActiveTransactions.Remove(GetTransactionSearchKey());
            }
            else
            {
                throw new Exception("No active transaction found");
            }

        }

        public bool IsTransactionInProgress()
        {
            if (!IsInComplusTransaction)
            {
                string key = GetTransactionSearchKey();
                if (Database.ActiveTransactions.ContainsKey(key))
                {
                    SqlTransaction t = Database.ActiveTransactions[key];
                    return this.TransactionDepth > 0;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsInComplusTransaction
        {
            get
            {
                return System.EnterpriseServices.ContextUtil.IsInTransaction;
            }
        }

        internal void ParticipateInTransaction(SqlCommand command)
        {
            SqlTransaction t = GetTransaction();
            if (t != null)
            {
                command.Connection = GetOpenConnection();
                command.Transaction = GetOpenConnection().BeginTransaction();
            }
            else
            {
                command.Connection = this.GetOpenConnection();
            }
        }

        internal SqlTransaction GetTransaction()
        {
            return IsTransactionInProgress() ? Database.ActiveTransactions[GetTransactionSearchKey()] : null;
        }

        #endregion


    }

    class ActiveTransactions : Dictionary<string, SqlTransaction>
    {

    }
}