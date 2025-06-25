using Fiction2Fact.App_Code;
using System.Data;
using System.Web;

namespace Fiction2Fact.Legacy_App_Code.Circulars.DAL
{
    public class CircUtilitiesDAL
    {
        public CircUtilitiesDAL()
        {

        }

        public DataTable GetDataTable(string strType, DBUtilityParameter oParam1 = null, DBUtilityParameter oParam2 = null, DBUtilityParameter oParam3 = null, string sOrderBy = null)
        {
            DataTable dtResult = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase("CIRC_GetUtilitiesData"))
                {
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Type", F2FDatabase.F2FDbType.VarChar, strType));
                    if (oParam1 != null)
                    {
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CondType1", F2FDatabase.F2FDbType.VarChar, oParam1.CondType));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ColName1", F2FDatabase.F2FDbType.VarChar, oParam1.ColumnName));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Operator1", F2FDatabase.F2FDbType.VarChar, oParam1.Operator));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@FirstVal1", F2FDatabase.F2FDbType.VarChar, oParam1.FirstValue));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@SecondVal1", F2FDatabase.F2FDbType.VarChar, oParam1.SecondValue));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IsSubQuery1", F2FDatabase.F2FDbType.Int32, oParam1.SubQuery));
                    }
                    if (oParam2 != null)
                    {
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CondType2", F2FDatabase.F2FDbType.VarChar, oParam2.CondType));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ColName2", F2FDatabase.F2FDbType.VarChar, oParam2.ColumnName));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Operator2", F2FDatabase.F2FDbType.VarChar, oParam2.Operator));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@FirstVal2", F2FDatabase.F2FDbType.VarChar, oParam2.FirstValue));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@SecondVal2", F2FDatabase.F2FDbType.VarChar, oParam2.SecondValue));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IsSubQuery2", F2FDatabase.F2FDbType.Int32, oParam2.SubQuery));
                    }
                    if (oParam3 != null)
                    {
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CondType3", F2FDatabase.F2FDbType.VarChar, oParam3.CondType));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ColName3", F2FDatabase.F2FDbType.VarChar, oParam3.ColumnName));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Operator3", F2FDatabase.F2FDbType.VarChar, oParam3.Operator));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@FirstVal3", F2FDatabase.F2FDbType.VarChar, oParam3.FirstValue));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@SecondVal3", F2FDatabase.F2FDbType.VarChar, oParam3.SecondValue));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IsSubQuery3", F2FDatabase.F2FDbType.VarChar, oParam3.SubQuery));
                    }

                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@OrderBy", F2FDatabase.F2FDbType.VarChar, sOrderBy));

                    DB.F2FDataAdapter.Fill(dtResult);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return dtResult;
        }
    }
}