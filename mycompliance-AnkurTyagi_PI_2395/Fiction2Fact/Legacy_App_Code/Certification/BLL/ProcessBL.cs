using System.Data;

namespace Fiction2Fact.Legacy_App_Code.Certification.BLL
{
    public class ProcessBL
    {
        ProcessDAL prDAL = new ProcessDAL();

        public DataSet validateUploadedCertData(string strBatchId, string strType, string strCircId, string mstrConnectionString)
        {
            return prDAL.validateUploadedCertData(strBatchId, strType, strCircId, mstrConnectionString);
        }

        //<< Added by Amarjeet on 27-Jul-2021
        public void uploadChecklistData(string strBatchId, string strType, int intCircId, string strLoggedInUser)
        {
            prDAL.uploadChecklistData(strBatchId, strType, intCircId, strLoggedInUser);
        }
        //>>

        public DataSet validateFraudData(string mstrConnectionString)
        {
            return prDAL.validateFraudData(mstrConnectionString);
        }
        public DataSet validateSecActionableData(string mstrConnectionString)
        {
            return prDAL.validateSecActionableData(mstrConnectionString);
        }

        //<< Added by Ramesh More on 04-Jul-2024 CR_2114
        public DataTable validateUploadedData(string strBatchId, string mstrConnectionString)
        {
            return prDAL.validateUploadedData(strBatchId, mstrConnectionString);
        }

        public string uploadCertChecklistData(string strBatchId, string strLoggedInUser)
        {
            return prDAL.uploadCertChecklistData(strBatchId, strLoggedInUser);
        }
        //>>

    }
}
