using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.DAL;
using System.Data;

namespace Fiction2Fact.Legacy_App_Code.BLL
{
    public class SubmUtilitiesBLL
    {
        private SubmUtilitiesDAL submUtil = new SubmUtilitiesDAL();

        public DataTable GetDataTable(string strType, DBUtilityParameter oParam1 = null, DBUtilityParameter oParam2 = null, DBUtilityParameter oParam3 = null)
        {
            return submUtil.GetDataTable(strType, oParam1, oParam2, oParam3);
        }
    }
}