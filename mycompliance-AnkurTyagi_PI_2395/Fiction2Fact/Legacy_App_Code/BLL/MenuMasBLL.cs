using System.Data;
using Fiction2Fact.Legacy_App_Code.DAL;

/// <summary>
/// Summary description for MenuMasBLL
/// </summary>

namespace Fiction2Fact.Legacy_App_Code.BLL
{
    public class MenuMasBLL
    {
        MenuMasDAL objDAL = new MenuMasDAL();

        public int saveMenuMasDetails(int intId, string strMenuName, int intLevel, int intParentId, int intSortOrder,
            string strMenuURL, string strStatus, string strCreateBy)
        {
            int retval;
            retval = objDAL.saveMenuMasDetails(intId, strMenuName, intLevel, intParentId, intSortOrder,
                        strMenuURL, strStatus, strCreateBy);
            return retval;
        }

        public DataTable searchMenuDetails(int intId, int intLevel, int intParentId, string strMenuName,
            string strMenuURL, string strStatus)
        {
            DataTable dt = new DataTable();
            dt = objDAL.searchMenuDetails(intId, intLevel, intParentId, strMenuName, strMenuURL, strStatus);
            return dt;
        }

        public DataTable getParentLevelMenu(int intLevel, int intParentId, string strStatus)
        {
            DataTable dt = new DataTable();
            dt = objDAL.getParentLevelMenu(intLevel, intParentId, strStatus);
            return dt;
        }

        public int updateSortOrderforMenu(int intCurrMenuId, int intCurrSortOrder, int intPrevNextMenuId, int intPrevNextSortOrder)
        {
            int retval;
            retval = objDAL.updateSortOrderforMenu(intCurrMenuId, intCurrSortOrder, intPrevNextMenuId, intPrevNextSortOrder);
            return retval;
        }
    }
}