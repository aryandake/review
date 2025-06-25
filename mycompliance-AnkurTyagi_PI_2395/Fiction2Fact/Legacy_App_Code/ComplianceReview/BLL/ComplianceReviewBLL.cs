using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using DocumentFormat.OpenXml.Spreadsheet;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.Compliance.DAL;
using Fiction2Fact.Legacy_App_Code.Outward.BLL;
using static Fiction2Fact.App_Code.F2FDatabase;

namespace Fiction2Fact.Legacy_App_Code.Compliance.BLL
{

    public class ComplianceReviewBLL
    {
        ComplianceReviewDAL oDAL = new ComplianceReviewDAL();

        #region Compliance Review

        public int SaveComplianceReview(int intCCR_ID, string strCCR_IDENTIFIER, int intCCR_CRUM_ID, string strCCR_UNIT_IDS, string strCCR_QUARTER, DateTime dtCCR_TENTATIVE_START_DATE, DateTime dtCCR_TENTATIVE_END_DATE, string strCCR_STATUS,
           string strCCR_REC_STATUS, string strCCR_REC_STATUS_REMARKS, int intCCR_CRM_ID, string strCCR_REVIEW_SCOPE, string strCCR_REVIEW_OBJECTIVE, string strCCR_REVIEW_STAGE,
           string strCCR_REMARKS, string strCCR_REVIEW_TYPE, DateTime dtCCR_WORK_STARTED_ON, string strCCR_APPROVAL_BY_L1, DateTime dtCCR_APPROVAL_ON_L1, string strCCR_APPROVAL_BY_L2, DateTime dtCCR_APPROVAL_ON_L2, string strCCR_CREATOR, string strCCR_AUDIT_TRAIL,
           string strCCR_CREATE_BY, string strCCR_UPDATE_BY, string strCCR_LINKAGE_WITH_EARLIER_CIRCULAR, string strCCR_SOC_EOC,
            string strCCR_OLD_CIRC_SUB_NO, string strCCR_BASE_ID)
        {
            int retval = 0;
            retval = oDAL.SaveComplianceReview(intCCR_ID, strCCR_IDENTIFIER, intCCR_CRUM_ID, strCCR_UNIT_IDS, strCCR_QUARTER, dtCCR_TENTATIVE_START_DATE, dtCCR_TENTATIVE_END_DATE, strCCR_STATUS,
           strCCR_REC_STATUS, strCCR_REC_STATUS_REMARKS, intCCR_CRM_ID, strCCR_REVIEW_SCOPE, strCCR_REVIEW_OBJECTIVE, strCCR_REVIEW_STAGE,
           strCCR_REMARKS, strCCR_REVIEW_TYPE, dtCCR_WORK_STARTED_ON, strCCR_APPROVAL_BY_L1, dtCCR_APPROVAL_ON_L1, strCCR_APPROVAL_BY_L2, dtCCR_APPROVAL_ON_L2, strCCR_CREATOR, strCCR_AUDIT_TRAIL,
           strCCR_CREATE_BY, strCCR_UPDATE_BY, strCCR_LINKAGE_WITH_EARLIER_CIRCULAR, strCCR_SOC_EOC, strCCR_OLD_CIRC_SUB_NO, strCCR_BASE_ID);
            return retval;
        }


        public DataTable Search_ComplianceReview(int intCCR_ID, int intCCR_CRUM_ID, int intCCR_CRM_ID, string dtCCR_TENTATIVE_START_DATE, string dtCCR_TENTATIVE_END_DATE, string strCCR_CREATE_BY, string strType, string strStatus = null)
        {
            DataTable dt = new DataTable();
            dt = oDAL.Search_ComplianceReview(intCCR_ID, intCCR_CRUM_ID, intCCR_CRM_ID, dtCCR_TENTATIVE_START_DATE, dtCCR_TENTATIVE_END_DATE, strCCR_CREATE_BY, strType, strStatus);
            return dt;
        }

        public int SaveComplianceReview_with_Files(int intCCR_ID, string strCCR_IDENTIFIER, int intCCR_CRUM_ID, string strCCR_UNIT_IDS, string strCCR_QUARTER, DateTime? dtCCR_TENTATIVE_START_DATE, DateTime? dtCCR_TENTATIVE_END_DATE, string strCCR_STATUS,
         string strCCR_REC_STATUS, string strCCR_REC_STATUS_REMARKS, int intCCR_CRM_ID, string strCCR_REVIEW_SCOPE, string strCCR_REVIEW_OBJECTIVE, string strCCR_REVIEW_STAGE,
         string strCCR_REMARKS, string strCCR_REVIEW_TYPE, DateTime? dtCCR_WORK_STARTED_ON, string strCCR_APPROVAL_BY_L1, DateTime? dtCCR_APPROVAL_ON_L1, string strCCR_APPROVAL_BY_L2, DateTime? dtCCR_APPROVAL_ON_L2, string strCCR_CREATOR, string strCCR_AUDIT_TRAIL,
         string strCCR_CREATE_BY, string strCCR_UPDATE_BY, DataTable dt, string strCCR_LINKAGE_WITH_EARLIER_CIRCULAR, string strCCR_SOC_EOC,
            string strCCR_OLD_CIRC_SUB_NO, string strCCR_BASE_ID)
        {
            int retval = 0;
            retval = oDAL.SaveComplianceReview_with_Files(intCCR_ID, strCCR_IDENTIFIER, intCCR_CRUM_ID, strCCR_UNIT_IDS, strCCR_QUARTER, dtCCR_TENTATIVE_START_DATE, dtCCR_TENTATIVE_END_DATE, strCCR_STATUS,
           strCCR_REC_STATUS, strCCR_REC_STATUS_REMARKS, intCCR_CRM_ID, strCCR_REVIEW_SCOPE, strCCR_REVIEW_OBJECTIVE, strCCR_REVIEW_STAGE,
           strCCR_REMARKS, strCCR_REVIEW_TYPE, dtCCR_WORK_STARTED_ON, strCCR_APPROVAL_BY_L1, dtCCR_APPROVAL_ON_L1, strCCR_APPROVAL_BY_L2, dtCCR_APPROVAL_ON_L2, strCCR_CREATOR, strCCR_AUDIT_TRAIL,
           strCCR_CREATE_BY, strCCR_UPDATE_BY, dt, strCCR_LINKAGE_WITH_EARLIER_CIRCULAR, strCCR_SOC_EOC, strCCR_OLD_CIRC_SUB_NO, strCCR_BASE_ID);
            return retval;
        }

        public DataTable getComplianceReview_Approval(string strType, string strCreatedBy, string strFilter1 = null)
        {
            DataTable dt = new DataTable();
            dt = oDAL.getComplianceReview_Approval(strType, strCreatedBy, strFilter1);
            return dt;
        }

        public DataTable getComplianceReviewApproval_Status(string strOperation, int intCR_ID, string strFilter1 = null)
        {
            DataTable dt = new DataTable();
            dt = oDAL.getComplianceReviewApproval_Status(strOperation, intCR_ID, strFilter1);
            return dt;
        }


        #endregion

        #region Compliance Review Files

        public int Save_Compliance_Review_Files(int intCCRF_ID, int intCCRF_CCR_ID, string strCCRF_TYPE, string strCCRF_FILE_TYPE, string strCCRF_SERVER_FILE_NAME, string strCCRF_CLIENT_FILE_NAME, string strCCRF_DESC, string strCCRF_CREATE_BY, string strCCRF_UPDATE_BY)
        {
            int intCircularId = oDAL.Save_Compliance_Review_Files(intCCRF_ID, intCCRF_CCR_ID, strCCRF_TYPE, strCCRF_FILE_TYPE, strCCRF_SERVER_FILE_NAME, strCCRF_CLIENT_FILE_NAME, strCCRF_DESC, strCCRF_CREATE_BY, strCCRF_UPDATE_BY);
            return intCircularId;
        }

        public DataTable Search_Compliance_Review_Files(int intCCRF_ID, int intCCRF_CCR_ID)
        {
            DataTable dtResults = new DataTable();
            dtResults = oDAL.Search_Compliance_Review_Files(intCCRF_ID, intCCRF_CCR_ID);
            return dtResults;
        }

        public int Delete_Compliance_Review_Files(int intCCRF_ID, int intCCRF_CCR_ID)
        {
            int retval = 0;
            retval = oDAL.Delete_Compliance_Review_Files(intCCRF_ID, intCCRF_CCR_ID);
            return retval;

        }

        #endregion

        #region Universe Master
        public int Save_Universe_Master(int intCRUM_ID, string strCRUM_UNIVERSE_TO_BE_REVIEWED, string strCRUM_STATUS, string strCRUM_BUSINESS_UNITS_INVOLVED, string strCREATE_BY, string strUPDATE_BY)
        {
            int retval = 0;
            retval = oDAL.Save_Universe_Master(intCRUM_ID, strCRUM_UNIVERSE_TO_BE_REVIEWED, strCRUM_STATUS, strCRUM_BUSINESS_UNITS_INVOLVED, strCREATE_BY, strUPDATE_BY);
            return retval;
        }
        public DataSet Search_Universe_Master(string strUniversename, string strStatus, int UniverserId = 0, string strvalue = null)
        {
            DataSet dsResults = new DataSet();
            dsResults = oDAL.Search_Universe_Master(strUniversename, strStatus, UniverserId, strvalue);
            return dsResults;
        }

        #endregion

        #region Reviewer Master

        public int Save_Reviewer_Master(int intCRM_ID, string strCRM_L0_REVIEWER_NT_ID, string strCRM_L0_REVIEWER_NAME, string strCRM_L0_REVIEWER_EMAIL,
           string strCRM_L1_REVIEWER_NT_ID, string strCRM_L1_REVIEWER_NAME, string strCRM_L1_REVIEWER_EMAIL, string strCRM_L2_REVIEWER_NT_ID, string strCRM_L2_REVIEWER_NAME
           , string strCRM_L2_REVIEWER_EMAIL, string strCRM_STATUS, string strCREATE_BY, string strUPDATE_BY)
        {
            int retval = 0;
            retval = oDAL.Save_Reviewer_Master(intCRM_ID, strCRM_L0_REVIEWER_NT_ID, strCRM_L0_REVIEWER_NAME, strCRM_L0_REVIEWER_EMAIL,
            strCRM_L1_REVIEWER_NT_ID, strCRM_L1_REVIEWER_NAME, strCRM_L1_REVIEWER_EMAIL, strCRM_L2_REVIEWER_NT_ID, strCRM_L2_REVIEWER_NAME
           , strCRM_L2_REVIEWER_EMAIL, strCRM_STATUS, strCREATE_BY, strUPDATE_BY);

            return retval;
        }
        public DataTable Search_Reviewer_Master(int intCRM_ID, string strL0_ReviewerName, string strL1_ReviewerName, string strL2_ReviewerName, string strStatus, string strL0_Reviewer_NT_ID = null, string strFilter1=null)
        {
            DataTable dtResults = new DataTable();
            dtResults = oDAL.Search_Reviewer_Master(intCRM_ID, strL0_ReviewerName, strL1_ReviewerName, strL2_ReviewerName, strStatus, strL0_Reviewer_NT_ID, strFilter1);
            return dtResults;
        }

        #endregion


        #region DRQ Master

        public int saveDRQM(int intID, string strSource, int intSourceId, int intUnitId, string strDataRequirementQuery,
          string strPersonResponsible, string strPersonResponsibleId, string strPersonResponsibleName, string strPersonResponsibleEmail, string strCreateBy, string strCreator, string strType, DataTable dtFiles)
        {
            int retval = 0;
            retval = oDAL.saveDRQM(intID, strSource, intSourceId, intUnitId, strDataRequirementQuery, strPersonResponsible, strPersonResponsibleId, strPersonResponsibleName, strPersonResponsibleEmail, strCreateBy, strCreator, strType, dtFiles);
            return retval;
        }


        public DataTable getDRQMDetails(int intCDQ_ID, int intCDQ_SOURCE_ID, string strCDQ_SOURCE, int intCDQ_SFM_ID, string strCDQ_PERSON_RESPONSIBLE, string strValue = null)
        {
            DataTable dtResults = new DataTable();
            dtResults = oDAL.getDRQMDetails(intCDQ_ID, intCDQ_SOURCE_ID, strCDQ_SOURCE, intCDQ_SFM_ID, strCDQ_PERSON_RESPONSIBLE, strValue);
            return dtResults;

        }

        public DataTable getDRQMFiles(int intCRDF_ID, string strCRDF_SOURCE, int intCRDF_CDQ_ID, string intCRDF_TYPE)
        {
            DataTable dtResults = new DataTable();
            dtResults = oDAL.getDRQMFiles(intCRDF_ID, strCRDF_SOURCE, intCRDF_CDQ_ID, intCRDF_TYPE);
            return dtResults;
        }

        public void saveDRQMClousreDetails(int intId, string strSource, int intSourceId, string strClosureReamrks, string strCreateBy)
        {
            oDAL.saveDRQMClousreDetails(intId, strSource, intSourceId, strClosureReamrks, strCreateBy);
        }

        #endregion

        #region DRQM Response

        public int saveDRQMResponse(int intId, string strSource, int intDRId, string strUpdateType, string strResponse, string strCreateBy,
           DataTable dtFiles, string strUser)
        {
            return oDAL.saveDRQMResponse(intId, strSource, intDRId, strUpdateType, strResponse, strCreateBy, dtFiles, strUser);
        }

        public DataTable getDRQMResponse(int intRefId, int intId, string strSource, string strUser = null, string strFilter1 = null,
            string strLoggedInUser = null, string strSourceIdentifier = null, string strIds = null)
        {
            return oDAL.getDRQMResponse(intRefId, intId, strSource, strUser, strFilter1, strLoggedInUser, strSourceIdentifier,
                strIds);
        }

        #endregion

        #region Sub Function Master

        public int Save_SUBFunction_Master(int intCSFM_ID, string strCSFM_NAME, string strCSFM_CODE, string strCSFM_HEAD,
          string strCSFM_UNIT_HEAD_CODE, string strCSFM_UNIT_HEAD_EMAIL, string strCSFM_UNIT_SPOC, string strCSFM_UNIT_SPOC_CODE, string strCSFM_UNIT_SPOC_EMAIL
          , string strCSFM_STATUS, string strCSFM_CREATE_BY, string strCSFM_UPDATE_BY)
        {
            int retval = 0;
            retval = oDAL.Save_SUBFunction_Master(intCSFM_ID, strCSFM_NAME, strCSFM_CODE, strCSFM_HEAD,
           strCSFM_UNIT_HEAD_CODE, strCSFM_UNIT_HEAD_EMAIL, strCSFM_UNIT_SPOC, strCSFM_UNIT_SPOC_CODE, strCSFM_UNIT_SPOC_EMAIL
          , strCSFM_STATUS, strCSFM_CREATE_BY, strCSFM_UPDATE_BY);
            return retval;
        }
        public DataTable Search_SubFunction_Master(int intCSFM_ID, string strCSFM_NAME, string strCSFM_CODE = null, string strCSFM_Status = null, string strFilter1 = null)
        {
            DataTable dt = new DataTable();
            dt = oDAL.Search_SubFunction_Master(intCSFM_ID, strCSFM_NAME, strCSFM_CODE, strCSFM_Status, strFilter1);
            return dt;
        }

        #endregion

        #region Issue 

        public int saveIssueDetails(int intIssueId, int intComplainceReviewId, int intUnitId,
       string strIssueTitle, string strIssueDescription, string strIssueType, string strIssueStatus,
      string strSPOCResponsible, string strSPOCResponsibleId, string strSPOCResponsibleName, string strSPOCResponsibleEmail,
       string strCreateBy, string strRemarks, string strStatus, DataTable dtFiles)
        {
            int retVal = 0;
            retVal = oDAL.saveIssueDetails(intIssueId, intComplainceReviewId, intUnitId,
        strIssueTitle, strIssueDescription, strIssueType, strIssueStatus,
       strSPOCResponsible, strSPOCResponsibleId, strSPOCResponsibleName, strSPOCResponsibleEmail,
        strCreateBy, strRemarks, strStatus, dtFiles);
            return intIssueId;
        }

        public DataTable getIssue(int intIssueID, int intComplianceReviewId, string strLoggedInUser,
            string strType, string strStatus, string strValue1 = null)
        {
            DataTable dt = new DataTable();
            dt = oDAL.getIssue(intIssueID, intComplianceReviewId, strLoggedInUser,
            strType, strStatus, strValue1);
            return dt;
        }

        public int submitForOperation(int intCI_ID, string strValue, string strOperation,
         string strCreateBy, int intRiskReviewDraftId = 0, string strLoggedInUser = null, string strValue1 = null)
        {
            int retVal = 0;
            retVal = oDAL.submitForOperation(intCI_ID, strValue, strOperation, strCreateBy, intRiskReviewDraftId, strLoggedInUser,
                strValue1);
            return retVal;
        }

        public DataTable getIssueFiles(int intIssueId, int intFileId, string strType, string strRC_Type)
        {
            DataTable dt = new DataTable();
            dt = oDAL.getIssueFiles(intIssueId, intFileId, strType, strRC_Type);
            return dt;
        }

        public int ChangeIssueStatus(int intIssueId, string strStatus, string strManagementResponse, string strRemarks)
        {
            int retVal = 0;
            retVal = oDAL.ChangeIssueStatus(intIssueId, strStatus, strManagementResponse, strRemarks);
            return retVal;
        }

        public int ApproveRejectIssue(int intCRId, int intIssueId, string strComments, string strCreateBy, string strType, string strUserType)
        {
            int retVal = 0;
            retVal = oDAL.ApproveRejectIssue(intCRId, intIssueId, strComments, strCreateBy, strType, strUserType);
            return retVal;
        }

        #endregion

        #region Issue Update

        public int saveIssueUpdate(int intCIA_ID, int intCIA_CI_ID, string strCIA_REF_NO,
    int intCIA_SFM_ID, string strCIA_ACTIONABLE, string strCIA_DETAILED_DESC, string strCIA_ACT_TYPE,
   string strCIA_CRITICALITY, string strCIA_STATUS, string strCIA_CREATOR_NAME, string strCIA_CREATOR_NT_ID,
    DateTime? strCIA_TARGET_DT, string strCIA_CLOSURE_BY, DateTime? strCIA_CLOSURE_DT, string strCIA_CLOSURE_REMARKS, string strCIA_REMARKS,
    string strCIA_CREATE_BY, string strCIA_UPDATE_BY, string strCIA_ACTIONABLE_STATUS, DateTime? strCIA_REVISED_TARGET_DT, string strCIA_REVISED_MAP,
    string intCIA_SPECIFIED_PERSON_ID, string strCIA_SPECIFIED_PERSON_NAME, string strCIA_SPECIFIED_PERSON_EMAIL)
        {
            int retval = 0;

            retval = oDAL.saveIssueUpdate(intCIA_ID, intCIA_CI_ID, strCIA_REF_NO,
            intCIA_SFM_ID, strCIA_ACTIONABLE, strCIA_DETAILED_DESC, strCIA_ACT_TYPE,
            strCIA_CRITICALITY, strCIA_STATUS, strCIA_CREATOR_NAME, strCIA_CREATOR_NT_ID,
            strCIA_TARGET_DT, strCIA_CLOSURE_BY, strCIA_CLOSURE_DT, strCIA_CLOSURE_REMARKS, strCIA_REMARKS,
            strCIA_CREATE_BY, strCIA_UPDATE_BY, strCIA_ACTIONABLE_STATUS, strCIA_REVISED_TARGET_DT, strCIA_REVISED_MAP,
            intCIA_SPECIFIED_PERSON_ID, strCIA_SPECIFIED_PERSON_NAME, strCIA_SPECIFIED_PERSON_EMAIL);

            return retval;
        }

        public DataTable getIssueActions(int intCIA_ID, int intCIA_CI_ID, string strCIA_REF_NO, string strCIA_CREATE_BY, string strCIA_ACTIONABLE_STATUS = null, string strCIA_RESPONSIBLE_PERSON = null,
            string strCIA_TARGET_DT_FROM = null, string strCIA_TARGET_DT_TO = null)
        {
            DataTable dt = new DataTable();
            dt = oDAL.getIssueActions(intCIA_ID, intCIA_CI_ID, strCIA_REF_NO, strCIA_CREATE_BY, strCIA_ACTIONABLE_STATUS, strCIA_RESPONSIBLE_PERSON, strCIA_TARGET_DT_FROM, strCIA_TARGET_DT_TO);
            return dt;
        }

        public DataTable getIssueActions_details(string strCIA_CREATE_BY, string strComplianceReviewNo, string strIssue, string strActionable,
            string strCIA_ACTIONABLE_STATUS, string strCIA_RESPONSIBLE_PERSON,
            string strCIA_TARGET_DT_FROM, string strCIA_TARGET_DT_TO, string strFilter1 = null)
        {
            DataTable dt = new DataTable();
            dt = oDAL.getIssueActions_details(strCIA_CREATE_BY, strComplianceReviewNo, strIssue, strActionable,
            strCIA_ACTIONABLE_STATUS, strCIA_RESPONSIBLE_PERSON,strCIA_TARGET_DT_FROM, strCIA_TARGET_DT_TO, strFilter1);
            return dt;
        }

        public int DeleteIssueActions(int intCIA_ID)
        {
            int retVal = 0;
            retVal = oDAL.DeleteIssueActions(intCIA_ID);
            return retVal;
        }


        #endregion

        #region Issue Action Updates

        public int saveIssueActionUpdate(int intCIAU_ID, int intCIAU_CIA_ID, string strCIAU_UPDATE_TYPE,
       string strCIAU_REVISED_MAP, string strCIAU_ACTIONABLE_STATUS, DateTime? dtCIAU_REVISED_TARGET_DT, string strCIAU_STATUS,
       DateTime? dtCIAU_CLOSURE_DT, string strCIAU_ACTUAL_ACTION_TAKEN,
       string strCIAU_CREATE_BY, string strCIAU_UPDATE_BY, string strCIAU_REMARKS, string strClientFileName, string strServerFileName)
        {
            int retVal = 0;
            retVal = oDAL.saveIssueActionUpdate(intCIAU_ID, intCIAU_CIA_ID, strCIAU_UPDATE_TYPE,
       strCIAU_REVISED_MAP, strCIAU_ACTIONABLE_STATUS, dtCIAU_REVISED_TARGET_DT, strCIAU_STATUS,
      dtCIAU_CLOSURE_DT, strCIAU_ACTUAL_ACTION_TAKEN,
       strCIAU_CREATE_BY, strCIAU_UPDATE_BY, strCIAU_REMARKS, strClientFileName, strServerFileName);
            return retVal;
        }

        public DataTable getIssueActionsUpdates(int intCIAU_ID, int intCIAU_CIA_ID, string strFilter1 = null)
        {
            DataTable dt = new DataTable();
            dt = oDAL.getIssueActionsUpdates(intCIAU_ID, intCIAU_CIA_ID, strFilter1);
            return dt;
        }

        #endregion

        #region Common Utility Functions

        public DataTable GetDataTable(string strType, DBUtilityParameter oParam1 = null, DBUtilityParameter oParam2 = null, DBUtilityParameter oParam3 = null, string sOrderBy = null)
        {
            return oDAL.GetDataTable(strType, oParam1, oParam2, oParam3, sOrderBy);
        }

        #endregion
    }
}