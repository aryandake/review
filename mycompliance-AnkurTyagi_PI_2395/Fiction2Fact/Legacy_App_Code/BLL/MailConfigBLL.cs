using System.Data;
using Fiction2Fact.Legacy_App_Code.DAL;

namespace Fiction2Fact.Legacy_App_Code.BLL
{
    public class MailConfigBLL
    {
        MailConfigDAL mcd = new MailConfigDAL();


        public DataTable searchMailConfig(string strMCId, string strType = null, string strModuleName = null, string mstrConnectionString = null)
        {
            DataTable dtResult = new DataTable();
            dtResult = mcd.searchMailConfig(strMCId, strType, strModuleName, mstrConnectionString);
            return dtResult;
        }

        public int saveMailConfigDetails(int intMCId, string strType, string strFrom,string strTo, string strCC, string strBCC, string strStatus, string strSubject, string strContent, string strCreateBy, string strModuleName, string mstrConnectionString)
        {
            int retVal = 0;
            retVal = mcd.saveMailConfigDetails(intMCId, strType, strFrom, strTo, strCC, strBCC, strStatus, strSubject, strContent, strCreateBy, strModuleName, mstrConnectionString);
            return retVal;
        }

        public int deleteMailConfig(int intMCId, string strCreateBy, string mstrConnectionString)
        {
            int retVal = 0;
            retVal = mcd.deleteMailConfig(intMCId, strCreateBy, mstrConnectionString);
            return retVal;
        }
        public DataTable getMailConfigType(string mstrConnectionString)
        {
            DataTable dtResult = new DataTable();
            dtResult = mcd.getMailConfigType(mstrConnectionString);
            return dtResult;
        }
        //>>

    }
}
