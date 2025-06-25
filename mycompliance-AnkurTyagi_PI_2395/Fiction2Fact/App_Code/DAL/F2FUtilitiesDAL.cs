using Fiction2Fact.App_Code;
using System.Data;
using System.Web;

namespace Fiction2Fact.App_Code.DAL
{
    public class F2FUtilitiesDAL
    {
        public F2FUtilitiesDAL()
        {

        }
        public bool SetSessionCompany()
        {
            DataTable dtCompany = new DataTable();
            App_Code.F2FDatabase DB = new App_Code.F2FDatabase();
            DB.F2FCommand.CommandText = "SELECT * FROM TBL_COMPANY WHERE CMP_CURRENT = 'Y'";
            DB.F2FDataAdapter.Fill(dtCompany);
            if (dtCompany.Rows.Count > 1)
            {
                return false;
            }
            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                if (HttpContext.Current.Session["CMP_LOGO"] == null)
                {
                    HttpContext.Current.Session["CMP_LOGO"] = dtCompany.Rows[0]["CMP_LOGO"].ToString();
                }
                HttpContext.Current.Session["CMP_NAME"] = dtCompany.Rows[0]["CMP_NAME"].ToString();
                HttpContext.Current.Session["CMP_SYSTEM_NAME"] = dtCompany.Rows[0]["CMP_SYSTEM_NAME"] as string;
                HttpContext.Current.Session["CMP_SHORT_NAME"] = dtCompany.Rows[0]["CMP_SHORT_NAME"] as string;
                HttpContext.Current.Session["CMP_ID"] = dtCompany.Rows[0]["CMP_ID"].ToString();
                return true;
            }
            return false;
        }

    }
}