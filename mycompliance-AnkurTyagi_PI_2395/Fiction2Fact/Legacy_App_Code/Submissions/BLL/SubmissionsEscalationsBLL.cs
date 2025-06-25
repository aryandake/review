using System.Data;
using Fiction2Fact.Legacy_App_Code.Submissions.DAL;

namespace Fiction2Fact.Legacy_App_Code.Submissions.BLL
{
    public class SubmissionsEscalationsBLL
    {
        SubmissionsEscalationsDAL submissionsEscalationsDAL = new SubmissionsEscalationsDAL();

        public int SaveSubmissionsEscalationsMas(int intId, string strFirstName, string strLastName, string strMiddelName, string strEmailId, int intType, string strLevels, string strCreateBy, string strConnectionString, string strEscalationType, string strReportingDept)
        {
            int retVal = 0;
            retVal = submissionsEscalationsDAL.SaveSubmissionsEscalationsMas(intId, strFirstName, strLastName,
                                    strMiddelName, strEmailId, intType,
                                     strLevels, strCreateBy, strConnectionString, strEscalationType,strReportingDept);
            return retVal;

        }


        public DataSet SearchSubmissionsEscalations(string strDepartment, string strLevel, string strType, string mstrConnectionString, string strReportingDept)
        {
            DataSet dsResult = new DataSet();
            dsResult = submissionsEscalationsDAL.SearchSubmissionsEscalations(strDepartment, strLevel, strType, mstrConnectionString, strReportingDept);
            return dsResult;
        }

       

    }
}
