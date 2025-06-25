using System.Data;
using Fiction2Fact.Legacy_App_Code.DAL;


namespace Fiction2Fact.Legacy_App_Code.BLL
{
    public class RefCodesBLL
    {
        RefCodesDAL rcd = new RefCodesDAL();

        public DataTable searchRefCode(string strRefID, string strRefType, string strRefName, string strRefCode, string mstrConnectionString)
        {
            DataTable dtResult = new DataTable();
            dtResult = rcd.searchRefCode(strRefID, strRefType, strRefName, strRefCode,
                                    mstrConnectionString);
            return dtResult;
        }

        public int saveRefCode(int intRefID, string strRefType, string strRefName, string strRefCode,
                                    string strCreateBy, string strSortOrder = null, string strStatus = null)
        {
            int retVal = 0;
            retVal = rcd.saveRefCode(intRefID, strRefType, strRefName, strRefCode,
                                     strCreateBy, strSortOrder, strStatus);
            return retVal;
        }

        public int deleteRefCode(int intRefID, string strCreateBy, string mstrConnectionString)
        {
            int retVal = 0;
            retVal = rcd.deleteRefCode(intRefID, strCreateBy, mstrConnectionString);
            return retVal;

        }

        public DataTable getRefCodeDetails(string strRefType, string mstrConnectionString = null)
        {
            DataTable dtResult = new DataTable();
            dtResult = rcd.getRefCodeDetails(strRefType, mstrConnectionString);
            return dtResult;
        }
    }
}
