using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.Circulars.DAL;
using System.Data;

namespace Fiction2Fact.Legacy_App_Code.Circulars.BLL
{
    public class CircUtilitiesBLL
    {
        private CircUtilitiesDAL f2fUtil = new CircUtilitiesDAL();

        public DataTable GetDataTable(string strType, DBUtilityParameter oParam1 = null, DBUtilityParameter oParam2 = null, DBUtilityParameter oParam3 = null, string sOrderBy = null)
        {
            return f2fUtil.GetDataTable(strType, oParam1, oParam2, oParam3, sOrderBy);
        }
    }
}