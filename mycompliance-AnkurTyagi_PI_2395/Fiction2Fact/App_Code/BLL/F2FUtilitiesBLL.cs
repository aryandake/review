using Fiction2Fact.App_Code.DAL;
using System.Data;

namespace Fiction2Fact.App_Code.BLL
{
    public class F2FUtilitiesBLL
    {
        private F2FUtilitiesDAL f2fUtil = new F2FUtilitiesDAL();
        public bool SetSessionCompany()
        {
            return f2fUtil.SetSessionCompany();
        }
    }
}