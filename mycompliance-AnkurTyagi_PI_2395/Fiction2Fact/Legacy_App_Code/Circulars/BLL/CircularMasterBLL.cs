using System.Data;
using System.Collections;
using Fiction2Fact.Legacy_App_Code.Circulars.DAL;

namespace Fiction2Fact.Legacy_App_Code.Circulars.BLL
{
    public class CircularMasterBLL
    {

        CircularMasterDAL circularMasterDAL = new CircularMasterDAL();

        //<< Modified by Amarjeet on 26-Jul-2021
        public int SaveCircular(int CircularId, int ciaId, int cAmId, int deptId, string circularNo,
            string downloadRefId, string topic, string inplication, string issuingLink, string name,
            string details, string circularDate, string createBy, int typeofCircular, DataTable dtSegment,
            DataTable dtIntimations, DataTable dtAddMails, DataTable dtFiles, DataTable dtAcion, string strSpocFromComplianceFn,
            string SubtypeOfDocument, string strAssociatedKeywords, string strLinkageWithEarlierCircular, string strSOCEOC,
            string strOldCirSubNo, string strOldCircularId, string strCircEffDate, string strAuditCommitteeToApprove,
            string strToBePlacedBefore, string strDetails, string strNameOfThePolicy, string strFrequency, string strBroadcastDate, int LOB)
        {
            int retVal = 0;
            retVal = circularMasterDAL.SaveCircular(CircularId, ciaId, cAmId, deptId, circularNo,
                downloadRefId, topic, inplication, issuingLink, name, details, circularDate,
                createBy, typeofCircular, dtSegment, dtIntimations, dtAddMails, dtFiles, dtAcion, strSpocFromComplianceFn,
                SubtypeOfDocument, strAssociatedKeywords, strLinkageWithEarlierCircular, strSOCEOC, strOldCirSubNo, strOldCircularId,
                strCircEffDate, strAuditCommitteeToApprove, strToBePlacedBefore, strDetails, strNameOfThePolicy, strFrequency,
                strBroadcastDate, LOB);
            return retVal;
        }
        //>>

        public DataSet SearchCircular(int circularId, string strIssuingAuthority, string strSegment, string strDepartment,
            string strarea, string strCircularNo, string strDownloadRefNo, string strTopic, string FromDate, string ToDate,
            string ActionType, string typeOfDocument, string strSpocFromCompliancefn, string strActionableHaveBennLogged,
            string strAssociatedKeywords, string strToBePlacedBefore = null, string strGlobalSearch = null,
            string strCircularIds = null, string strIsFileToBeSearched = null, string strStatus = null)
        {
            DataSet dsResult = new DataSet();
            dsResult = circularMasterDAL.SearchCircular(circularId, strIssuingAuthority, strSegment, strDepartment, strarea,
                strCircularNo, strDownloadRefNo, strTopic, FromDate, ToDate, ActionType, typeOfDocument, strSpocFromCompliancefn,
                strActionableHaveBennLogged, strAssociatedKeywords, strToBePlacedBefore, strGlobalSearch,
                strCircularIds, strIsFileToBeSearched, strStatus);
            return dsResult;
        }

        public DataSet getCircularDetails(Hashtable paramTable, string mstrConnectionString)
        {
            return circularMasterDAL.getCircularDetails(paramTable, mstrConnectionString);
        }
        //<<Added By Subodh Deolekar on 05-Jul-2010 for Audit Trail Module.

        public int deleteCircular(int intCircularId, string strConnectionString)
        {
            int retVal = 0;
            retVal = circularMasterDAL.deleteCircular(intCircularId, strConnectionString);
            return retVal;

        }
        //>>

        //Added by Supriya on 20-Nov-2014
        public DataTable SearchCircularActionable(int intCircular, string strCircularNo, string FromDate,
            string ToDate, string strStatus, string strActionableId, string strUsername, string mstrConnectionString)
        {
            DataTable dsResult = new DataTable();
            dsResult = circularMasterDAL.SearchCircularActionable(intCircular, strCircularNo,
                                            FromDate, ToDate, strStatus, strActionableId, strUsername, mstrConnectionString);
            return dsResult;
        }


        public int saveCircularActionableUpdates(int Id, int intActionableId, string strUpdateType, string strRemarks,
            string strRevisedTargetDate, string strActionableClosureDate, string strFileNameOnClient,
            string strFileNameOnServer, string strCreateBy)
        {
            int retVal = 0;
            retVal = circularMasterDAL.saveCircularActionableUpdates(Id, intActionableId, strUpdateType, strRemarks,
                strRevisedTargetDate, strActionableClosureDate, strFileNameOnClient, strFileNameOnServer, strCreateBy);
            return retVal;
        }

        public DataTable SearchCircularActionableUpdates(int intCircularActId, string mstrConnectionString)
        {
            DataTable dsResult = new DataTable();
            dsResult = circularMasterDAL.SearchCircularActionableUpdates(intCircularActId, mstrConnectionString);
            return dsResult;
        }

        //Modify By Milan Yadav on 27-Aug-2016
        public int UpdateCircularActionables(int Id, int intCircularId, string strActionable,
               string strPerRespId, string strPerRespName, string strPerRespEmail, string strTargetDate, string strComplDate, string strStatus,
               string strRemarks, string strClosureRemarks, string strCreateBy, DataTable dtActionablefile,
              string strConnectionString)
        {
            int retVal = 0;
            retVal = circularMasterDAL.UpdateCircularActionables(Id, intCircularId, strActionable, strPerRespId, strPerRespName, strPerRespEmail,
                strTargetDate, strComplDate, strStatus, strRemarks, strClosureRemarks, strCreateBy, dtActionablefile, strConnectionString);
            return retVal;
        }


        public DataTable SearchCircularActionableNew(string strCircularNumber, string strCircularFromDate, string strCircularToDate,
            string strType, string strIssuingAuthority, string strTopic, string strSubject, string strActionable,
            string strPersonResponsible, string FromDate, string ToDate, string strStatus, string mstrConnectionString)
        {
            DataTable dtResults = new DataTable();
            dtResults = circularMasterDAL.SearchCircularActionableNew(strCircularNumber, strCircularFromDate, strCircularToDate, strType,
                    strIssuingAuthority, strTopic, strSubject, strActionable, strPersonResponsible, FromDate, ToDate,
                    strStatus, mstrConnectionString);
            return dtResults;
        }
        //>>

        //Added by Supriya on 17-Jan-2015
        public DataSet SearchCircularDetails(int intCircular, string strIssuingAuthority, string strDepartment,
            string strTopic, string strCircularNo, string FromDate, string ToDate, string strTypeOfDocument,
            string strSubject, string strGist, string strImplications, string strIntimatedDept,
            string strActionType, string strCurUserEmail, string strAssociatedKeywords,
            string strToBePlacedBefore = null, string strGlobalSearch = null, string strCircularIds = null,
            string strIsFileToBeSearched = null, string strStatus = null)
        {
            DataSet ds = new DataSet();
            ds = circularMasterDAL.SearchCircularDetails(intCircular, strIssuingAuthority, strDepartment, strTopic,
                strCircularNo, FromDate, ToDate, strTypeOfDocument, strSubject, strGist, strImplications,
                strIntimatedDept, strActionType, strCurUserEmail, strAssociatedKeywords, strToBePlacedBefore,
                strGlobalSearch, strCircularIds, strIsFileToBeSearched, strStatus);
            return ds;
        }

        public void insertCircularAcionables(int CircularId, DataTable dtAcion, string strCreator, string createBy)
        {
            circularMasterDAL.insertCircularAcionables(CircularId, dtAcion, strCreator, createBy);
        }
        public void DeleteCircularActionable(string strActId)
        {
            circularMasterDAL.DeleteCircularActionable(strActId);
        }

        //<< Added by Amarjeet on 08-Jul-2021
        public void insertCircularCertChecklist(int intCircularId, DataTable dtCertChecklist, string strCreateBy)
        {
            circularMasterDAL.insertCircularCertChecklist(intCircularId, dtCertChecklist, strCreateBy);
        }

        public DataTable SearchCircularCertChecklist(int intCircular, string strCircCertChecklist, string strLoggedInUserName = null,
            string strType = null)
        {
            DataTable dsResult = new DataTable();
            dsResult = circularMasterDAL.SearchCircularCertChecklist(intCircular, strCircCertChecklist, strLoggedInUserName, strType);
            return dsResult;
        }

        public void DeleteCircularCertChecklist(string strCircCertChecklistId)
        {
            circularMasterDAL.DeleteCircularCertChecklist(strCircCertChecklistId);
        }

        public void acceptRejectCertChecklist(DataTable dt, string strStatus, string strCreateBy)
        {
            circularMasterDAL.acceptRejectCertChecklist(dt, strStatus, strCreateBy);
        }
        //>>

        public void insertCircularAdditionalMails(int intcircularId, DataTable dt, string CreateBy)
        {
            circularMasterDAL.insertCircularAdditionalMails(intcircularId, dt, CreateBy);
        }
    }
}
