using System.Data;
using Fiction2Fact.Legacy_App_Code.DAL;
using Fiction2Fact.Legacy_App_Code.VO;
/// <summary>
/// Summary description for MailLogBLL
/// </summary>
namespace Fiction2Fact.Legacy_App_Code.BLL
{
    public class MailLogBLL
    {
        MailLogVO mlVo = new MailLogVO();
        MailLogDAL mlDAl = new MailLogDAL();

        public MailLogBLL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataTable getFailureMails(MailLogVO mlVo)
        {
            DataTable dt = new DataTable();
            dt = mlDAl.getFailureMails(mlVo);
            return dt;
        }

        public int insertIntoMailLog(MailLogVO mlVo)
        {
            int retval;
            retval = mlDAl.insertIntoMailLog(mlVo);
            return retval;
        }
    }
}