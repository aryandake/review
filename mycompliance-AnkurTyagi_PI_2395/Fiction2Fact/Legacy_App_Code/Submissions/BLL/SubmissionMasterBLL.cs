using System;
using System.Data;
using Fiction2Fact.Legacy_App_Code.Submissions.DAL;

namespace Fiction2Fact.Legacy_App_Code.Submissions.BLL
{
    public class SubmissionMasterBLL
    {

        SubmissionMasterDAL submissionMasterDAL = new SubmissionMasterDAL();

        public int insertSubmissions(int intEditSubmissionId, string strStatus, int intSubType, int intReportDept,
            string strType, int intEvent, int intAssociatedWith, string strParticulars, string strDescription,
            int intStartDays, int intEndDays, string strEscalate, string strFrequency, string strOnceFromDate,
            string strOnceToDate, string strFromWeekDays, string strToWeekDays, string strMonthlyFromDate,
            string strMonthlyTodate, string strQ1fromDate, string strQ1ToDate, string strQ2FromDate,
            string strQ2ToDate, string strQ3FromDate, string strQ3ToDate, string strQ4fFromDate, string strQ4Todate,
            string strFirstHalffromDate, string strFirstHalfToDate, string strSecondtHalffromDate,
            string strSecondtHalffromTo, string strYearlyfromDate, string strYearlyDateTo,
            string strF1Fromdate, string strF1ToDate, string strF2FromDate, string strF2ToDate,
            int intlevel1, int intlevel2, string strEffectiveDate, string strPriority, string strCreateBy, DataTable dtOwners,
            DataTable dtCompany, DataTable dtSegment, DataTable dtReportOwners,
            DataTable mdtEditFileUpload, string strRegulation, string strSection, string strCircId, string strConnectionString,
            //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
            int intLOB, string strWorkflowStatus, string strIsFSAppReq, string strCircularDate, int intlevel0)
        //>>
        {
            int retVal = 0;
            retVal = submissionMasterDAL.insertSubmissions(intEditSubmissionId, strStatus, intSubType,
                intReportDept, strType, intEvent, intAssociatedWith, strParticulars, strDescription, intStartDays,
                intEndDays, strEscalate, strFrequency, strOnceFromDate, strOnceToDate, strFromWeekDays,
                strToWeekDays, strMonthlyFromDate, strMonthlyTodate, strQ1fromDate, strQ1ToDate, strQ2FromDate,
                strQ2ToDate, strQ3FromDate, strQ3ToDate, strQ4fFromDate, strQ4Todate, strFirstHalffromDate,
                strFirstHalfToDate, strSecondtHalffromDate, strSecondtHalffromTo, strYearlyfromDate, strYearlyDateTo,
                strF1Fromdate, strF1ToDate, strF2FromDate, strF2ToDate,
                intlevel1, intlevel2, strEffectiveDate, strPriority, strCreateBy, dtOwners, dtCompany, dtSegment,
                dtReportOwners, mdtEditFileUpload, strRegulation, strSection, strCircId, strConnectionString,
                //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
                intLOB, strWorkflowStatus, strIsFSAppReq, strCircularDate, intlevel0);
            //>>
            return retVal;
        }

        public void updateSubmissions(int intEditSubmissionId, string strStatus, int intSubType, int intReportDept,
            string strType, int intEvent, int intAssociatedWith, string strParticulars, string strDescription,
            int intStartDays, int intEndDays, string strEscalate, string strFrequency, string strOnceFromDate,
            string strOnceToDate, string strFromWeekDays, string strToWeekDays, string strMonthlyFromDate,
            string strMonthlyTodate, string strQ1fromDate, string strQ1ToDate, string strQ2FromDate,
            string strQ2ToDate, string strQ3FromDate, string strQ3ToDate, string strQ4fFromDate, string strQ4Todate,
            string strFirstHalffromDate, string strFirstHalfToDate, string strSecondtHalffromDate,
            string strSecondtHalffromTo, string strYearlyfromDate, string strYearlyDateTo,
            string strF1Fromdate, string strF1ToDate, string strF2FromDate, string strF2ToDate,
            int intlevel1, int intlevel2, string strEffectiveDate, string strPriority, string strCreateBy, DataTable dtOwners,
            DataTable dtCompany, DataTable dtSegment, DataTable dtReportOwners,
            DataTable mdtEditFileUpload, string strRegulation, string strSection, string strConnectionString, int intLOB
            //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
            , string strReasonForEdit, string strIsFSAppReq, string strCircularDate, int intlevel0)
        //>>
        {
            submissionMasterDAL.updateSubmissions(intEditSubmissionId, strStatus, intSubType,
                intReportDept, strType, intEvent, intAssociatedWith, strParticulars, strDescription, intStartDays,
                intEndDays, strEscalate, strFrequency, strOnceFromDate, strOnceToDate, strFromWeekDays,
                strToWeekDays, strMonthlyFromDate, strMonthlyTodate, strQ1fromDate, strQ1ToDate, strQ2FromDate,
                strQ2ToDate, strQ3FromDate, strQ3ToDate, strQ4fFromDate, strQ4Todate, strFirstHalffromDate,
                strFirstHalfToDate, strSecondtHalffromDate, strSecondtHalffromTo, strYearlyfromDate, strYearlyDateTo,
                strF1Fromdate, strF1ToDate, strF2FromDate, strF2ToDate,
                intlevel1, intlevel2, strEffectiveDate, strPriority, strCreateBy, dtOwners, dtCompany, dtSegment,
                dtReportOwners, mdtEditFileUpload, strRegulation, strSection, strConnectionString, intLOB, strReasonForEdit,
                strIsFSAppReq, strCircularDate, intlevel0);
        }

        public int generateSubmissionsWiseChecklist(int intSubId, string strEffectivedt, string strConnectionString)
        {
            int retVal = 0;
            retVal = submissionMasterDAL.generateSubmissionsWiseChecklist(intSubId, strEffectivedt, strConnectionString);
            return retVal;
        }
        public int generateSubmissionsWiseEventChecklist(int intSubId, string strEEM_ID, string strConnectionString)
        {
            int retVal = 0;
            retVal = submissionMasterDAL.generateSubmissionsWiseEventChecklist(intSubId, strEEM_ID, strConnectionString);
            return retVal;
        }

        public int generateEComplySubChklist(int intFinYearId, string strConnectionString)
        {
            int retVal = 0;
            retVal = submissionMasterDAL.generateEComplySubChklist(intFinYearId, strConnectionString);
            return retVal;
        }

        public int TrancateChecklistTables(string strConnectionString)
        {
            int retVal = 0;
            retVal = submissionMasterDAL.TrancateChecklistTables(strConnectionString);
            return retVal;
        }

        public DataSet LoadSubmissionChecklist(String strSelectedMonth, String strUser, String mstrConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            resultDataSet = submissionMasterDAL.LoadSubmissionChecklist(strSelectedMonth, strUser, mstrConnectionString);
            return resultDataSet;
        }

        public DataSet LoadMySubmissionChecklist(String strSelectedMonth, String strEmployeeName, String mstrConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            resultDataSet = submissionMasterDAL.LoadMySubmissionChecklist(strSelectedMonth, strEmployeeName, mstrConnectionString);
            return resultDataSet;
        }

        public int saveChecklistDetails(int intSubId, int intChecklistId, string strYesNoNA, string strRemarks, string strStatus, string strCurrentUser, string mstrConnectionString)
        {
            int intupdated;
            intupdated = submissionMasterDAL.saveChecklistDetails(intSubId, intChecklistId, strYesNoNA, strRemarks, strStatus, strCurrentUser, mstrConnectionString);
            return intupdated;
        }

        public DataSet SearchSubmissionChecklistForFix(String strUser, String strFilterExpression, string strOrderBy, String mstrConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            resultDataSet = submissionMasterDAL.SearchSubmissionChecklistForFix(strUser, strFilterExpression, strOrderBy, mstrConnectionString);
            return resultDataSet;
        }
        public DataSet SearchSubmissionChecklistForEventBased(String strUser, String strFilterCondition, string strFilterEvent, string strOrderBy, String mstrConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            resultDataSet = submissionMasterDAL.SearchSubmissionChecklistForEventBased(strUser, strFilterCondition, strFilterEvent, strOrderBy, mstrConnectionString);
            return resultDataSet;
        }
        public int insertSubmissionFiles(int intSubmissionId, DataTable dt, string strUser, string strOperationType, string mstrConnectionString)
        {
            int InsertedRows = submissionMasterDAL.insertSubmissionFiles(intSubmissionId, dt, strUser, strOperationType, mstrConnectionString);
            return InsertedRows;
        }

        public DataSet SearchSubmissions(int intSubmissionsId, string strReportingFun, string strFrequency, string strStatus, string strSegment,
                                         string strSubType, string strDepartment, string strEvent, string strEventPurpose,
                                          string strEmpName, string strType, string mstrConnectionString)
        {
            DataSet dsSearchSubmissions = new DataSet();

            dsSearchSubmissions = submissionMasterDAL.SearchSubmissions(intSubmissionsId, strReportingFun, strFrequency, strStatus, strSegment,
                                                                        strSubType, strDepartment, strEvent, strEventPurpose,
                                                                         strEmpName, strType, mstrConnectionString);
            return dsSearchSubmissions;
        }
        //<<Modified by Ashish Mishra on 27Jul2017
        //<< added by prajakta
        public int saveAdminChecklist(int intSubId, string strStatus, string strCurrentUser, string strSubAuthorityDate,
                                    string strReOpenComments, string strModeOfFiling, string strReopenPurpose, string strHygiene, string mstrConnectionString)
        {
            int intupdated;
            intupdated = submissionMasterDAL.saveAdminChecklist(intSubId, strStatus, strCurrentUser, strSubAuthorityDate,
                                                        strReOpenComments, strModeOfFiling,
                                                        //<<Added by Ankur Tyagi on 05-May-2025 for Project Id : 2395
                                                        strReopenPurpose, strHygiene,
                                                        //>>
                                                        mstrConnectionString);
            return intupdated;
        }
        //>>
        public DataSet LoadPastChecklist(String strSelectedMonth, String strSelectedFinYear, string strUserType, string strUserName, string strGlobal, String mstrConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            resultDataSet = submissionMasterDAL.LoadPastChecklist(strSelectedMonth, strSelectedFinYear, strUserType, strUserName, strGlobal, mstrConnectionString);
            return resultDataSet;
        }

        public int inactiveSubmission(int intSubId, string strDuedate, string strStatus, string strDeActRemarks, string strCurrentUser, string mstrConnectionString)
        {
            int intupdated;
            intupdated = submissionMasterDAL.inactiveSubmission(intSubId, strDuedate, strStatus, strDeActRemarks, strCurrentUser, mstrConnectionString);
            return intupdated;
        }
        //>>

        public int UpdateSubmissionDetails(int intEditSubmissionId, string strStatus, int intSubType, int intReportDept,
            string strType, int intEvent, int intAssociatedWith, string strParticulars, string strDescription,
            int intStartDays, int intEndDays, string strEscalate, string strFrequency, string strOnceFromDate,
            string strOnceToDate, string strFromWeekDays, string strToWeekDays, string strMonthlyFromDate, string strMonthlyTodate,
            string strQ1fromDate, string strQ1ToDate, string strQ2FromDate, string strQ2ToDate, string strQ3FromDate,
            string strQ3ToDate, string strQ4fFromDate, string strQ4Todate, string strFirstHalffromDate,
            string strFirstHalfToDate, string strSecondtHalffromDate, string strSecondtHalffromTo, string strYearlyfromDate,
            string strYearlyDateTo, string strF1Fromdate, string strF1ToDate, string strF2FromDate, string strF2ToDate,
            int intlevel1, int intlevel2, string strEffectiveDate, string strPriority,
            string strCreateBy, string strOpType, string strConnectionString)
        {
            int retVal = 0;
            retVal = submissionMasterDAL.UpdateSubmissionDetails(intEditSubmissionId, strStatus, intSubType, intReportDept,
                strType, intEvent, intAssociatedWith, strParticulars, strDescription, intStartDays, intEndDays,
                strEscalate, strFrequency, strOnceFromDate, strOnceToDate, strFromWeekDays, strToWeekDays, strMonthlyFromDate,
                strMonthlyTodate, strQ1fromDate, strQ1ToDate, strQ2FromDate, strQ2ToDate, strQ3FromDate, strQ3ToDate,
                strQ4fFromDate, strQ4Todate, strFirstHalffromDate, strFirstHalfToDate, strSecondtHalffromDate,
                strSecondtHalffromTo, strYearlyfromDate, strYearlyDateTo,
                strF1Fromdate, strF1ToDate, strF2FromDate, strF2ToDate,
                intlevel1, intlevel2, strEffectiveDate,
                strPriority, strCreateBy, strOpType, strConnectionString);
            return retVal;

        }

        public DataSet SearchSubmissionsByOwners(int intOwner, int intRepOwner, string mstrConnectionString)
        {
            DataSet dsSearchSubmissions = new DataSet();

            dsSearchSubmissions = submissionMasterDAL.SearchSubmissionsByOwners(intOwner, intRepOwner, mstrConnectionString);
            return dsSearchSubmissions;
        }

        public int insertSubmissionOwners(int intEMId, int intSMId, string strCreateBy, string strConnectionString)
        {
            int retVal = 0;
            retVal = submissionMasterDAL.insertSubmissionOwners(intEMId, intSMId, strCreateBy, strConnectionString);
            return retVal;

        }
        public int insertSubmissionReportingOwners(int intOwnerId, int intSMId, string strCreateBy, string strConnectionString)
        {
            int retVal = 0;
            retVal = submissionMasterDAL.insertSubmissionReportingOwners(intOwnerId, intSMId, strCreateBy, strConnectionString);
            return retVal;

        }

        public int saveFilingDetsInEditMode(int intEditSubmissionId, string strStatus, int intSubType, int intReportDept,
            string strType, int intEvent, int intAssociatedWith, string strParticulars, string strDescription,
            int intStartDays, int intEndDays, string strEscalate, string strFrequency, string strOnceFromDate,
            string strOnceToDate, string strFromWeekDays, string strToWeekDays, string strMonthlyFromDate,
            string strMonthlyTodate, string strQ1fromDate, string strQ1ToDate, string strQ2FromDate, string strQ2ToDate,
            string strQ3FromDate, string strQ3ToDate, string strQ4fFromDate, string strQ4Todate, string strFirstHalffromDate,
            string strFirstHalfToDate, string strSecondtHalffromDate, string strSecondtHalffromTo, string strYearlyfromDate,
            string strYearlyDateTo, string strF1Fromdate, string strF1ToDate, string strF2FromDate, string strF2ToDate,
            int intlevel1, int intlevel2, string strEffectiveDate, string strPriority,
            string strCreateBy, string strOpType, string strConnectionString, DataTable mdtOwners, DataTable mdtRepOwners)
        {
            int intupdated = 0;
            intupdated = submissionMasterDAL.saveFilingDetsInEditMode(intEditSubmissionId, strStatus, intSubType, intReportDept,
                strType, intEvent, intAssociatedWith, strParticulars, strDescription, intStartDays, intEndDays, strEscalate,
                strFrequency, strOnceFromDate, strOnceToDate, strFromWeekDays, strToWeekDays, strMonthlyFromDate,
                strMonthlyTodate, strQ1fromDate, strQ1ToDate, strQ2FromDate, strQ2ToDate, strQ3FromDate, strQ3ToDate,
                strQ4fFromDate, strQ4Todate, strFirstHalffromDate, strFirstHalfToDate, strSecondtHalffromDate,
                strSecondtHalffromTo, strYearlyfromDate, strYearlyDateTo,
                strF1Fromdate, strF1ToDate, strF2FromDate, strF2ToDate, intlevel1, intlevel2, strEffectiveDate,
                strPriority, strCreateBy, strOpType, strConnectionString, mdtOwners, mdtRepOwners);
            return intupdated;
        }

        //<<Modified by Ashish Mishra on 27Jul2017
        public int saveChecklistDetails(int intSubId, int intChecklistId, string strYesNoNA,/* string strSubDate,*/ string strRemarks, string strStatus, string strCurrentUser, string strClientFileName, string strServerFileName, string mstrConnectionString)
        {
            int intupdated;
            intupdated = submissionMasterDAL.saveChecklistDetails(intSubId, intChecklistId, strYesNoNA,/* strSubDate,*/ strRemarks, strStatus, strCurrentUser, strClientFileName, strServerFileName, mstrConnectionString);
            return intupdated;
        }
        //>>
        //<<Modified By Milan Yadav On 28Apr2016
        public DataSet LoadComplianceChecklist(String strSelectedMonth, String strUser, string strType, int intFinancialYearId,
            int intReportingDeptId, string strStatus, string strGlobalSearch, string strAuthority, string strFrequency, string strConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            resultDataSet = submissionMasterDAL.LoadComplianceChecklist(strSelectedMonth, strUser, strType, intFinancialYearId,
                intReportingDeptId, strStatus, strGlobalSearch, strAuthority, strFrequency, strConnectionString);
            return resultDataSet;
        }
        //>>

        //<<Commented By Milan Yadav On 28Apr2016

        //public DataSet LoadComplianceChecklist(String strSelectedMonth, String strUser, string strType, string strConnectionString)
        //{
        //    DataSet resultDataSet = new DataSet();
        //    resultDataSet = submissionMasterDAL.LoadComplianceChecklist(strSelectedMonth, strUser, strType, strConnectionString);
        //    return resultDataSet;
        //}

        //>>
        public DataTable getReportingDeptOwners(String strSubChklistId, string strConnectionString)
        {
            return submissionMasterDAL.getReportingDeptOwners(strSubChklistId, strConnectionString);
        }

        public DataTable getTrackingDeptOwners(String strSubChklistId, string strConnectionString)
        {
            return submissionMasterDAL.getTrackingDeptOwners(strSubChklistId, strConnectionString);
        }

        //Added by Bhavik 13Dec2014
        //<<Modified by Ankur Tyagi on 18-Apr-2025 for Project Id : 2395
        //Added Global Search
        public DataSet SearchComplianceChecklist(string strFilterCondition, string strUser, string strType, string strOrderBy, string strGlobalSearch,
            string mstrConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            resultDataSet = submissionMasterDAL.SearchComplianceChecklist(strFilterCondition, strUser, strType, strOrderBy, strGlobalSearch, mstrConnectionString);
            return resultDataSet;
        }
        //>>

        public DataTable getListOfReports(int intSubmissionsId, string strReportingFun, string strFrequency, string strStatus,
            string strSegment, string strSubType, string strDepartment, string strEvent, string strEventPurpose, string strEmpName,
            string strType, string strCircId, string strGlobal, string mstrConnectionString)
        {
            DataTable dtListOfReports = new DataTable();

            dtListOfReports = submissionMasterDAL.getListOfReports(intSubmissionsId, strReportingFun, strFrequency, strStatus,
                strSegment, strSubType, strDepartment, strEvent, strEventPurpose, strEmpName, strType, strCircId, strGlobal,
                mstrConnectionString);
            return dtListOfReports;
        }

        //Added By Milan Yadav on 25Apr2016
        //>>
        public int deleteSubmissions(int intSubmissionId, string strDeleteBy, string strConnectionString)
        {
            int retVal = 0;
            retVal = submissionMasterDAL.deleteSubmissions(intSubmissionId, strDeleteBy, strConnectionString);
            return retVal;

        }

        public DataTable getChecklistForReopenClosureByMonth(string strMonth, string strFinYear, string strUsername, string strUserType,
                                                             string strType, string strStatus, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            dt = submissionMasterDAL.getChecklistForReopenClosureByMonth(strMonth, strFinYear, strUsername, strUserType, strType, strStatus,
                                                                            mstrConnectionString);
            return dt;
        }

        //<<

        public int saveReportingDepartment(int intSRD_ID, string strSRD_NAME, string strSRD_STATUS, string strCurrentUser, string strReason, string mstrConnectionString)
        {
            int retVal = 0;
            retVal = submissionMasterDAL.saveReportingDepartment(intSRD_ID, strSRD_NAME, strSRD_STATUS, strCurrentUser, strReason, mstrConnectionString);
            return retVal;

        }

        public DataTable getReportingDepartment(int intSRD_ID, string strSRD_NAME, string strSRD_STATUS, string strCurrentUser, string mstrConnectionString, string strCheckDuplicate = null)
        {
            DataTable dt = new DataTable();
            dt = submissionMasterDAL.getReportingDepartment(intSRD_ID, strSRD_NAME, strSRD_STATUS, strCurrentUser, strCheckDuplicate, mstrConnectionString);
            return dt;
        }

        public int saveReportingUsers(int intSRDOM_ID, int intSRDOM_SRD_ID, string strSRDOM_EMP_NAME, string strSRDOM_EMAILID, string strSRDOM_EMP_ID,
            string strSRDOM_STATUS, string strCurrentUser, string strReason, string mstrConnectionString)
        {
            int retVal = 0;
            retVal = submissionMasterDAL.saveReportingUsers(intSRDOM_ID, intSRDOM_SRD_ID, strSRDOM_EMP_NAME, strSRDOM_EMAILID, strSRDOM_EMP_ID,
                strSRDOM_STATUS, strCurrentUser, strReason, mstrConnectionString);
            return retVal;

        }

        public DataTable getReportingUsers(int intSRDOM_ID, int intSRDOM_SRD_ID, string strSRDOM_EMP_NAME, string strSRDOM_EMAILID, string strSRDOM_EMP_ID,
            string strSRDOM_STATUS, string strCurrentUser, string mstrConnectionString, string strCheckDuplicate = null)
        {
            DataTable dt = new DataTable();
            dt = submissionMasterDAL.getReportingUsers(intSRDOM_ID, intSRDOM_SRD_ID, strSRDOM_EMP_NAME, strSRDOM_EMAILID, strSRDOM_EMP_ID,
                strSRDOM_STATUS, strCurrentUser, mstrConnectionString, strCheckDuplicate);
            return dt;
        }

        public DataTable getReportingDepartmentDetails(string strSRD_NAME, string strSRDOM_EMP_NAME, string strLevel, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            dt = submissionMasterDAL.getReportingDepartmentDetails(strSRD_NAME, strSRDOM_EMP_NAME, strLevel, mstrConnectionString);
            return dt;
        }

        public int saveReportingEscalations(int intSE_ID, int intSE_SRD_ID, string strSE_FIRST_NAME, string strSE_EMAIL_ID, string strSE_EMPLOYEE_ID,
            string strSE_STATUS, string strSE_LEVEL, string strCurrentUser, string strReason, string mstrConnectionString)
        {
            int retVal = 0;
            retVal = submissionMasterDAL.saveReportingEscalations(intSE_ID, intSE_SRD_ID, strSE_FIRST_NAME, strSE_EMAIL_ID, strSE_EMPLOYEE_ID,
                strSE_STATUS, strSE_LEVEL, strCurrentUser, strReason, mstrConnectionString);
            return retVal;

        }

        public DataTable getReportingEscalations(int intSE_ID, int intSE_SRD_ID, string strSE_FIRST_NAME, string strSE_EMAIL_ID, string strSE_EMPLOYEE_ID,
            string strSE_STATUS, string strSE_LEVEL, string strCurrentUser, string mstrConnectionString, string strCheckDuplicate = null)
        {
            DataTable dt = new DataTable();
            dt = submissionMasterDAL.getReportingEscalations(intSE_ID, intSE_SRD_ID, strSE_FIRST_NAME, strSE_EMAIL_ID, strSE_EMPLOYEE_ID,
                strSE_STATUS, strSE_LEVEL, strCurrentUser, mstrConnectionString, strCheckDuplicate);
            return dt;
        }

        public DataTable getTrackingDepartmentDetails(string strSTM_TYPE, string strEM_EMPNAME, string strEM_USERNAME, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            dt = submissionMasterDAL.getTrackingDepartmentDetails(strSTM_TYPE, strEM_EMPNAME, strEM_USERNAME, mstrConnectionString);
            return dt;
        }

        public int saveTrackingDepartment(int intSTM_ID, string strSTM_TYPE, string strSTM_STATUS, string strCurrentUser, string strReason, string mstrConnectionString)
        {
            int retVal = 0;
            retVal = submissionMasterDAL.saveTrackingDepartment(intSTM_ID, strSTM_TYPE, strSTM_STATUS, strCurrentUser, strReason, mstrConnectionString);
            return retVal;

        }

        public DataTable getTrackingDepartment(int intSTM_ID, string strSTM_TYPE, string strSTM_STATUS, string strCurrentUser, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            dt = submissionMasterDAL.getTrackingDepartment(intSTM_ID, strSTM_TYPE, strSTM_STATUS, strCurrentUser, mstrConnectionString);
            return dt;
        }

        public int saveEmployeeMaster(int intEM_ID, string strEM_EMPNAME, string strEM_EMAIL, string strEM_USERNAME, string strEM_STATUS,
            string strCurrentUser, string strReason, string mstrConnectionString, int intSTM_ID = 0, int intESM_LEVEL = 0)
        {
            int retVal = 0;
            retVal = submissionMasterDAL.saveEmployeeMaster(intEM_ID, strEM_EMPNAME, strEM_EMAIL, strEM_USERNAME, strEM_STATUS, strCurrentUser,
                strReason, mstrConnectionString, intSTM_ID, intESM_LEVEL);
            return retVal;

        }

        public DataTable getEmployeeMaster(int intEM_ID, int intSTM_ID, string strEM_EMPNAME, string strEM_EMAIL, string strEM_USERNAME, string strEM_STATUS, string strCurrentUser, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            dt = submissionMasterDAL.getEmployeeMaster(intEM_ID, intSTM_ID, strEM_EMPNAME, strEM_EMAIL, strEM_USERNAME, strEM_STATUS, strCurrentUser, mstrConnectionString);
            return dt;
        }

        public int saveTrackingDepartmentEmployeeMapping(int intESM_ID, int intESM_EM_ID, int intESM_STM_ID, int intESM_LEVEL, string strCurrentUser, string mstrConnectionString)
        {
            int retVal = 0;
            retVal = submissionMasterDAL.saveTrackingDepartmentEmployeeMapping(intESM_ID, intESM_EM_ID, intESM_STM_ID, intESM_LEVEL, strCurrentUser, mstrConnectionString);
            return retVal;

        }

        public DataTable getTrackingDepartmentEmployeeMapping(int intESM_ID, int intESM_EM_ID, int intESM_STM_ID, int intESM_LEVEL, string strCurrentUser, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            dt = submissionMasterDAL.getTrackingDepartmentEmployeeMapping(intESM_ID, intESM_EM_ID, intESM_STM_ID, intESM_LEVEL, strCurrentUser, mstrConnectionString);
            return dt;
        }

        //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
        public DataTable getSubmissionforApproval(int intSMId, String strUser, int intReportingDeptId, string strStatus, string strType,
            string strGlobalSearch, string strConnectionString)
        {
            DataTable dt = new DataTable();
            dt = submissionMasterDAL.getSubmissionforApproval(intSMId, strUser, intReportingDeptId, strStatus, strType,
                strGlobalSearch, strConnectionString);
            return dt;
        }

        public int updateSubmissionApproval(int intSubId, string strStatus, string strRemarks, string strLoggedInUser, string mstrConnectionString)
        {
            int intupdated;
            intupdated = submissionMasterDAL.updateSubmissionApproval(intSubId, strStatus, strRemarks, strLoggedInUser, mstrConnectionString);
            return intupdated;
        }

        public DataSet LoadComplianceChecklistForApproval(String strSelectedMonth, String strUser, int intFinancialYearId,
            int intReportingDeptId, string strStatus, string strGlobalSearch, string strAuthority, string strFrequency, string strConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            resultDataSet = submissionMasterDAL.LoadComplianceChecklistForApproval(strSelectedMonth, strUser, intFinancialYearId,
                intReportingDeptId, strStatus, strGlobalSearch, strAuthority, strFrequency, strConnectionString);
            return resultDataSet;
        }

        public int updateExtensionDate(int intSCId, string strExtensionDate, string strExtensionRemarks,
            string strDoneBy, string strConnectionString)
        {
            int intupdated;
            intupdated = submissionMasterDAL.updateExtensionDate(intSCId, strExtensionDate, strExtensionRemarks, strDoneBy, strConnectionString);
            return intupdated;
        }
        //>>

    }
}
