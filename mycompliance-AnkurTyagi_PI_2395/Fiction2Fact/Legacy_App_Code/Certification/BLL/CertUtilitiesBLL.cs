using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.Certification.DAL;
using System.Data;

namespace Fiction2Fact.Legacy_App_Code.Certification.BLL
{
    public class CertUtilitiesBLL
    {
        private CertUtilitiesDAL f2fUtil = new CertUtilitiesDAL();

        public DataTable GetDataTable(string strType, DBUtilityParameter oParam1 = null, DBUtilityParameter oParam2 = null, DBUtilityParameter oParam3 = null)
        {
            return f2fUtil.GetDataTable(strType, oParam1, oParam2, oParam3);
        }
    }
}