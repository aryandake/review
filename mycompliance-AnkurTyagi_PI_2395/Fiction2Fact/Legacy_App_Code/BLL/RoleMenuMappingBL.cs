using System.Data;
using Fiction2Fact.Legacy_App_Code.DAL;

namespace Fiction2Fact.Legacy_App_Code.BLL
{
    public class RoleMenuMappingBL
    {
        RoleMenuMappingDAL rmmDAL = new RoleMenuMappingDAL();

        public DataSet getRolesUnmapped(string strRoleID, string mstrConnectionString)
        {
            DataSet ds;
            ds = rmmDAL.getRolesUnmapped(strRoleID, mstrConnectionString);
            return ds;
        }
        public DataSet getRolesMapped(string strRoleID, string mstrConnectionString)
        {
            DataSet ds;
            ds = rmmDAL.getRolesMapped(strRoleID, mstrConnectionString);
            return ds;
        }
        public void saveRoleMenuMap(string strRole, int intMenuId, string mstrConnectionString)
        {
            rmmDAL.saveRoleMenuMap(strRole, intMenuId, mstrConnectionString);

        }
        public void deleteRoleMenuMap(string strRole, int intMenuId, string mstrConnectionString)
        {
            rmmDAL.deleteRoleMenuMap(strRole, intMenuId, mstrConnectionString);

        }
        public string roleMenuMapping(string strUser, string mstrConnectionString)
        {
            string retVal = "";
            retVal = rmmDAL.roleMenuMapping(strUser, mstrConnectionString);
            return retVal;
        }
        public DataTable searchUserManagementDets(string strUserId, string strEmail, string strRole,
        string strType, string mstrConnectionString)
        {
            return rmmDAL.searchUserManagementDets(strUserId, strEmail, strRole, strType, mstrConnectionString);
        }
    }
}
