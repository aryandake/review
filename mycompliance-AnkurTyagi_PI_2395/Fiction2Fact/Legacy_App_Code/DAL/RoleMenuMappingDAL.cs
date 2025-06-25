using System;
using System.Data;
using Fiction2Fact.App_Code;
using static Fiction2Fact.App_Code.F2FDatabase;
using System.Configuration;

namespace Fiction2Fact.Legacy_App_Code.DAL
{
    public class RoleMenuMappingDAL
    {
        string F2FDatabaseType = ConfigurationManager.AppSettings["F2FDatabaseType"].ToString();
        public DataSet getRolesUnmapped(string strRoleID, string mstrConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            try
            {
                using (F2FDatabase DB = new F2FDatabase("getRolesUnmapped"))
                {
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    string guid = null;
                    switch (F2FDatabaseType)
                    {
                        case "MsSQL":
                            guid = new Guid(strRoleID).ToString();
                            break;
                        case "MySQL":
                            guid = strRoleID;
                            break;
                    }

                    //cmdInsert.Parameters.Add("RoleID", SqlDbType.VarChar).Value = guid;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("RoleID", F2FDbType.VarChar, guid));
                    DB.F2FDataAdapter.Fill(resultDataSet);
                    return resultDataSet;
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in getRolesUnmapped() " + ex);
            }
            finally
            {

            }
        }
        public DataSet getRolesMapped(string strRoleID, string mstrConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            try
            {
                using (F2FDatabase DB = new F2FDatabase("getRolesMapping"))
                {
                    string guid = null;
                    switch (F2FDatabaseType)
                    {
                        case "MsSQL":
                            guid = new Guid(strRoleID).ToString();
                            break;
                        case "MySQL":
                            guid = strRoleID;
                            break;
                    }
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("RoleId", F2FDbType.VarChar, guid));
                    DB.F2FDataAdapter.Fill(resultDataSet);
                }
                return resultDataSet;
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in getRolesMapped() " + ex);
            }
            finally
            {

            }
        }
        public int saveRoleMenuMap(string strRole, int intMenuId, string mstrConnectionString)
        {
            int intReturnId = 0;
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "saveRoleMenuMap";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    string guid = null;
                    switch (F2FDatabaseType)
                    {
                        case "MsSQL":
                            guid = new Guid(strRole).ToString();
                            break;
                        case "MySQL":
                            guid = strRole;
                            break;
                    }
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("MenuId", F2FDbType.Int32, intMenuId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("Role", F2FDbType.VarChar, guid));
                    DB.OpenConnection();
                    intReturnId = Convert.ToInt32(DB.F2FCommand.ExecuteNonQuery());
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in saveRoleMenuMap() " + ex);
            }
            finally
            {

            }

            return intReturnId;
        }
        public void deleteRoleMenuMap(string strRole, int intMenuId, string mstrConnectionString)
        {
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "deleteRoleMenuMap";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    string guid = null;
                    switch (F2FDatabaseType)
                    {
                        case "MsSQL":
                            guid = new Guid(strRole).ToString();
                            break;
                        case "MySQL":
                            guid = strRole;
                            break;
                    }
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("MenuId", F2FDbType.Int32, intMenuId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("Role", F2FDbType.VarChar, guid));
                    DB.OpenConnection();
                    DB.F2FCommand.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in deleteRoleMenuMap() " + ex);
            }
            finally
            {

            }
        }

        public string roleMenuMapping(string strUser, string mstrConnectionString)
        {
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    if (strUser.Equals(""))
                        DB.F2FCommand.CommandText = "getMenuForUnAuthUsers";
                    else
                    {
                        DB.F2FCommand.CommandText = "getMenu";
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Username", F2FDbType.VarChar, strUser));
                    }
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    IDataParameter op = F2FDatabase.F2FParameter("@MenuString", F2FDbType.VarChar, null, ParameterDirection.Output, 15000);
                    DB.F2FDataParameter.Value = DB.F2FCommand.Parameters.Add(op);
                    DB.OpenConnection();
                    DB.F2FCommand.ExecuteReader();
                    return Convert.ToString(op.Value);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in roleMenuMapping(): " + ex);
            }
            finally
            {

            }
        }
       public DataTable searchUserManagementDets(string strUserId, string strEmail, string strRole,
       string strType, string mstrConnectionString)
        {
            DataTable dtResults = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "searchUserManagementDets";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@UserName", F2FDatabase.F2FDbType.VarChar, strUserId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Email", F2FDatabase.F2FDbType.VarChar, strEmail));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Role", F2FDatabase.F2FDbType.VarChar, strRole));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Type", F2FDatabase.F2FDbType.VarChar, strType));

                    DB.F2FDataAdapter.Fill(dtResults);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }

            return dtResults;
        }
    }
}
