using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.Outward.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Fiction2Fact.Legacy_App_Code.Outward.BLL
{
    public class OutwardUtilitiesBLL
    {
        OutwardUtilitiesDAL outUtilDal = new OutwardUtilitiesDAL();

        public DataTable GetDataTable(string strType, DBUtilityParameter oParam1 = null, DBUtilityParameter oParam2 = null, DBUtilityParameter oParam3 = null, string sOrderBy = null)
        {
            return outUtilDal.GetDataTable(strType, oParam1, oParam2, oParam3, sOrderBy);
        }
        
    }
}