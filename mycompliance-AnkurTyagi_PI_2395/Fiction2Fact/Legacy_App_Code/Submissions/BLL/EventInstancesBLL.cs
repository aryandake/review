using System.Data;
using Fiction2Fact.Legacy_App_Code.Submissions.DAL;

namespace Fiction2Fact.Legacy_App_Code.Submissions.BLL
{
    public class EventInstancesBLL
    {
        EventInstancesDAL eventInstancesDAL = new EventInstancesDAL();

        public int SaveEventInstances(int intId, string strEvent, string strCompany, string strDateOfEvent, string strDescription,
                                        string strCreateBy, DataTable AssociateWithdt, string mstrConnectionString)
        {
            int retVal = 0;
            retVal = eventInstancesDAL.SaveEventInstances(intId, strEvent, strCompany, strDateOfEvent,
                                        strDescription, strCreateBy, AssociateWithdt, mstrConnectionString);
            return retVal;

        }

        public DataSet SearchEventInstances(int intId, int intEventType, string strEventDate, string mstrConnectionString)
        {
            DataSet dsResult = new DataSet();
            dsResult = eventInstancesDAL.SearchEventInstances(intId, intEventType, strEventDate, mstrConnectionString);
            return dsResult;
        }

        public int deleteEventInstances(int intID, string strConnectionString)
        {
            int retVal = 0;
            retVal = eventInstancesDAL.deleteEventInstances(intID, strConnectionString);
            return retVal;

        }

    }
}
