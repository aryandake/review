using System;
using System.Data;
using Fiction2Fact.App_Code;
using static Fiction2Fact.App_Code.F2FDatabase;
/// <summary>
/// Summary description for MenuMasDAL
/// </summary>
namespace Fiction2Fact.Legacy_App_Code.DAL
{
    public class MenuMasDAL
    {
        public int saveMenuMasDetails(int intId, string strMenuName, int intLevel, int intParentId, int intSortOrder,
            string strMenuURL, string strStatus, string strCreateBy)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.F2FCommand.CommandText = "MM_saveMenuMasDetails";
                try
                {
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, intId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@MenuName", F2FDbType.VarChar, strMenuName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Level", F2FDbType.VarChar, intLevel));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ParentId", F2FDbType.VarChar, (intParentId.Equals(0) ? null : intParentId.ToString())));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SortOrder", F2FDbType.VarChar, intSortOrder));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@MenuURL", F2FDbType.VarChar, strMenuURL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.OpenConnection();
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retVal;
        }

        public DataTable searchMenuDetails(int intId, int intLevel, int intParentId, string strMenuName,
            string strMenuURL, string strStatus)
        {
            DataTable dtResult = new DataTable();
            using (F2FDatabase DB = new F2FDatabase("MM_searchMenuDetails"))
            {
                try
                {
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.VarChar, intId.ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Level", F2FDbType.VarChar, intLevel.ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ParentId", F2FDbType.VarChar, intParentId.ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@MenuName", F2FDbType.VarChar, strMenuName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@MenuURL", F2FDbType.VarChar, strMenuURL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FDataAdapter.Fill(dtResult);
                }
                catch (Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dtResult;
        }

        public DataTable getParentLevelMenu(int intLevel, int intParentId, string strStatus)
        {
            DataTable dtResult = new DataTable();

            using (F2FDatabase DB = new F2FDatabase("MM_getParentLevelMenu"))
            {
                try
                {
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Level", F2FDbType.VarChar, intLevel));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ParentId", F2FDbType.VarChar, intParentId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FDataAdapter.Fill(dtResult);
                }
                catch (Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dtResult;
        }

        public int updateSortOrderforMenu(int intCurrMenuId, int intCurrSortOrder, int intPrevNextMenuId, int intPrevNextSortOrder)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase("MM_updateSortOrderforMenu"))
            {
                try
                {
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CurrMenuId", F2FDbType.Int32, intCurrMenuId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CurrSortOrder", F2FDbType.Int32, intCurrSortOrder));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@PrevNextMenuId", F2FDbType.Int32, intPrevNextMenuId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@PrevNextSortOrder", F2FDbType.Int32, intPrevNextSortOrder));
                    DB.OpenConnection();
                    retVal = DB.F2FCommand.ExecuteNonQuery();
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