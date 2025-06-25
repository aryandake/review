using DocumentFormat.OpenXml.Spreadsheet;
using Fiction2Fact.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using static Fiction2Fact.App_Code.F2FDatabase;

namespace Fiction2Fact.Legacy_App_Code.Submissions.DAL
{
    public class SubmissionMasterDAL
    {
        public int insertSubmissions(int intEditSubmissionId, string strStatus, int intSubType, int intReportDept,
            string strType, int intEvent, int intAssociatedWith, string strParticulars, string strDescription,
            int intStartDays, int intEndDays, string strEscalate, string strFrequency, string strOnceFromDate,
            string strOnceToDate, string strFromWeekDays, string strToWeekDays, string strMonthlyFromDate,
            string strMonthlyTodate, string strQ1fromDate, string strQ1ToDate, string strQ2FromDate,
            string strQ2ToDate, string strQ3FromDate, string strQ3ToDate, string strQ4fFromDate, string strQ4Todate,
            string strFirstHalffromDate, string strFirstHalfToDate, string strSecondtHalffromDate,
            string strSecondtHalffromTo, string strYearlyfromDate, string strYearlyDateTo,
            string strF1Fromdate, string strF1ToDate, string strF2FromDate, string strF2ToDate,
            int intlevel1, int intlevel2, string strEffectiveDate, string strPriority, string strCreateBy,
            DataTable dtOwners, DataTable dtCompany, DataTable dtSegment, DataTable dtRepoOwners,
            DataTable mdtEditFileUpload, string strRegulation, string strSection, string strCircId,
            string strConnectionString, int intLOB, string strWorkflowStatus, string strIsFSAppReq, string strCircularDate, int intlevel0)
        {
            int intSubmissionId = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "SUBM_insertSubmissions";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EditSubmissionId", F2FDbType.Int32, intEditSubmissionId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@StmID", F2FDbType.Int32, intSubType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ReportDeptID", F2FDbType.Int32, intReportDept));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubType", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EventId", F2FDbType.Int32, intEvent));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@PurposeId", F2FDbType.Int32, intAssociatedWith));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Particulars", F2FDbType.VarChar, strParticulars));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Description", F2FDbType.VarChar, strDescription));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@StartDays", F2FDbType.Int32, intStartDays));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EndDays", F2FDbType.Int32, intEndDays));
                    //DB.F2FCommand.Parameters.Add(F2FParameter("@SortOrder", F2FDbType.Int32, 10, intSortOrder));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Escalate", F2FDbType.VarChar, strEscalate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Frequency", F2FDbType.VarChar, strFrequency));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OnceFromDate", F2FDbType.VarChar, strOnceFromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OnceToDate", F2FDbType.VarChar, strOnceToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FromWeekDays", F2FDbType.VarChar, strFromWeekDays));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ToWeekDays", F2FDbType.VarChar, strToWeekDays));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@MonthlyFromDate", F2FDbType.VarChar, strMonthlyFromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@MonthlyTodate", F2FDbType.VarChar, strMonthlyTodate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q1fromDate", F2FDbType.VarChar, strQ1fromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q1ToDate", F2FDbType.VarChar, strQ1ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q2FromDate", F2FDbType.VarChar, strQ2FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q2ToDate", F2FDbType.VarChar, strQ2ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q3FromDate", F2FDbType.VarChar, strQ3FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q3ToDate", F2FDbType.VarChar, strQ3ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q4fFromDate", F2FDbType.VarChar, strQ4fFromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q4Todate", F2FDbType.VarChar, strQ4Todate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstHalffromDate", F2FDbType.VarChar, strFirstHalffromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstHalfToDate", F2FDbType.VarChar, strFirstHalfToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondtHalffromDate", F2FDbType.VarChar, strSecondtHalffromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondtHalffromTo", F2FDbType.VarChar, strSecondtHalffromTo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@YearlyfromDate", F2FDbType.VarChar, strYearlyfromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@YearlyDateTo", F2FDbType.VarChar, strYearlyDateTo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@level1", F2FDbType.Int32, intlevel1));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@level2", F2FDbType.Int32, intlevel2));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Priority", F2FDbType.VarChar, strPriority));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EffectiveDate", F2FDbType.DateTime, strEffectiveDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    //<< Added by Vivek on 22-Jun-2017
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstF1FromDate", F2FDbType.VarChar, strF1Fromdate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstF1ToDate", F2FDbType.VarChar, strF1ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondF2FromDate", F2FDbType.VarChar, strF2FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondF2ToDate", F2FDbType.VarChar, strF2ToDate));
                    //>>
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Regulation", F2FDbType.VarChar, strRegulation));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Section", F2FDbType.VarChar, strSection));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircId", F2FDbType.Int32, (string.IsNullOrEmpty(strCircId) ? 0 : Convert.ToInt32(strCircId))));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LOB", F2FDbType.Int32, intLOB));

                    DB.F2FCommand.Parameters.Add(F2FParameter("@WorkflowStatus", F2FDbType.VarChar, strWorkflowStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@IsFSApprovalReq", F2FDbType.VarChar, strIsFSAppReq));

                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularDate", F2FDbType.VarChar, strCircularDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@level0", F2FDbType.VarChar, intlevel0));

                    intSubmissionId = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                    if (intEditSubmissionId == 0)
                    {
                        insertSubmissionOwners(intSubmissionId, dtOwners, strCreateBy);
                        insertSubmissionCompany(intSubmissionId, dtCompany, strCreateBy);
                        insertSubmissionSegMapping(intSubmissionId, dtSegment, strCreateBy);
                        insertSubReportingOwners(intSubmissionId, dtRepoOwners, strCreateBy);
                        insertSubmissionMasFiles(intSubmissionId, mdtEditFileUpload);
                    }
                    else
                    {
                        insertSubmissionOwners(intEditSubmissionId, dtOwners, strCreateBy);
                        insertSubmissionCompany(intEditSubmissionId, dtCompany, strCreateBy);
                        insertSubmissionSegMapping(intEditSubmissionId, dtSegment, strCreateBy);
                        insertSubReportingOwners(intEditSubmissionId, dtRepoOwners, strCreateBy);
                        insertSubmissionMasFiles(intEditSubmissionId, mdtEditFileUpload);
                    }

                    DB.F2FTransaction.Commit();
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    throw new System.Exception("system exception in InsertSubmissions() " + ex);
                }
            }
            return intSubmissionId;
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
            int intlevel1, int intlevel2, string strEffectiveDate, string strPriority, string strCreateBy,
            DataTable dtOwners, DataTable dtCompany, DataTable dtSegment, DataTable dtRepoOwners,
            DataTable mdtEditFileUpload, string strRegulation, string strSection,
            string strConnectionString, int intLOB, string strReasonForEdit, string strIsFSAppReq, string strCircularDate, int intlevel0)
        {

            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "SUBM_UpdateSubmissionDets";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;

                    DB.F2FCommand.Parameters.Add(F2FParameter("@SMID", F2FDbType.Int32, intEditSubmissionId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubmissionType", F2FDbType.Int32, intSubType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ReportDeptID", F2FDbType.Int32, intReportDept));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Event", F2FDbType.Int32, intEvent));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EventPurpose", F2FDbType.Int32, intAssociatedWith));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Particulars", F2FDbType.VarChar, strParticulars));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Description", F2FDbType.VarChar, strDescription));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@StartDate", F2FDbType.Int32, intStartDays));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EndDate", F2FDbType.Int32, intEndDays));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@TobeEscalate", F2FDbType.VarChar, strEscalate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Frequency", F2FDbType.VarChar, strFrequency));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OnceFromDate", F2FDbType.VarChar, strOnceFromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OnceToDate", F2FDbType.VarChar, strOnceToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FromWeekDays", F2FDbType.VarChar, strFromWeekDays));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ToWeekDays", F2FDbType.VarChar, strToWeekDays));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@MonthlyFromDate", F2FDbType.VarChar, strMonthlyFromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@MonthlyTodate", F2FDbType.VarChar, strMonthlyTodate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q1fromDate", F2FDbType.VarChar, strQ1fromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q1ToDate", F2FDbType.VarChar, strQ1ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q2FromDate", F2FDbType.VarChar, strQ2FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q2ToDate", F2FDbType.VarChar, strQ2ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q3FromDate", F2FDbType.VarChar, strQ3FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q3ToDate", F2FDbType.VarChar, strQ3ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q4FromDate", F2FDbType.VarChar, strQ4fFromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q4Todate", F2FDbType.VarChar, strQ4Todate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstHalffromDate", F2FDbType.VarChar, strFirstHalffromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstHalfToDate", F2FDbType.VarChar, strFirstHalfToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondHalffromDate", F2FDbType.VarChar, strSecondtHalffromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondHalfToDate", F2FDbType.VarChar, strSecondtHalffromTo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@YearlyfromDate", F2FDbType.VarChar, strYearlyfromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@YearlyDateTo", F2FDbType.VarChar, strYearlyDateTo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@level1", F2FDbType.Int32, intlevel1));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@level2", F2FDbType.Int32, intlevel2));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Priority", F2FDbType.VarChar, strPriority));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Effectivedt", F2FDbType.DateTime, strEffectiveDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    //<< Added by Vivek on 29Dec2015
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstF1FromDate", F2FDbType.VarChar, strF1Fromdate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstF1ToDate", F2FDbType.VarChar, strF1ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondF2FromDate", F2FDbType.VarChar, strF2FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondF2ToDate", F2FDbType.VarChar, strF2ToDate));
                    //>>
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Regulation", F2FDbType.VarChar, strRegulation));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Section", F2FDbType.VarChar, strSection));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LOB", F2FDbType.Int32, intLOB));

                    DB.F2FCommand.Parameters.Add(F2FParameter("@ReasonForEdit", F2FDbType.VarChar, strReasonForEdit));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@IsFSApprovalReq", F2FDbType.VarChar, strIsFSAppReq));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularDate", F2FDbType.VarChar, strCircularDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@level0", F2FDbType.VarChar, intlevel0));

                    DB.F2FCommand.ExecuteNonQuery();

                    insertSubmissionOwners(intEditSubmissionId, dtOwners, strCreateBy);
                    //insertSubmissionCompany(intEditSubmissionId, dtCompany, strCreateBy);
                    //insertSubmissionSegMapping(intEditSubmissionId, dtSegment, strCreateBy);
                    insertSubReportingOwners(intEditSubmissionId, dtRepoOwners, strCreateBy);
                    insertSubmissionMasFiles(intEditSubmissionId, mdtEditFileUpload);

                    DB.F2FTransaction.Commit();
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    throw new System.Exception("system exception in updateSubmissions(): " + ex);
                }
            }
        }

        private void insertSubmissionMasFiles(int Id, DataTable dt)
        {
            if (dt != null)
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.OpenConnection();
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        DB.F2FCommand.CommandText = "SUBM_insertSubmissionMasFiles";
                        DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                        DB.F2FCommand.Parameters.Clear();
                        DB.F2FCommand.Parameters.Add(F2FParameter("@SMId", F2FDbType.Int32, Id));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@FileName", F2FDbType.VarChar, dr["FileName"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@ServerFileName", F2FDbType.VarChar, dr["FileNameOnServer"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, dr["Uploaded By"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@FileType", F2FDbType.VarChar, dr["FileTypeShortForm"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@FileDesc", F2FDbType.VarChar, dr["File Description"].ToString()));

                        DB.F2FCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private void insertSubmissionOwners(int submissionId, DataTable dt, string CreateBy)
        {
            int retVal;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    DataRow dr = dt.Rows[i];
                    DB.F2FCommand.CommandText = "SUBM_insertSubmissionOwners";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SMID", F2FDbType.VarChar, submissionId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OwnerId", F2FDbType.Int32, dr["OwnerId"]));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, CreateBy));
                    retVal = DB.F2FCommand.ExecuteNonQuery();
                }
            }
        }
        private void insertSubmissionCompany(int submissionId, DataTable dt, string CreateBy)
        {
            int retVal;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    DataRow dr = dt.Rows[i];
                    DB.F2FCommand.CommandText = "SUBM_insertSubmissionCompanies";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Clear();
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SMID", F2FDbType.VarChar, submissionId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CompanyId", F2FDbType.Int32, dr["CompanyId"]));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, CreateBy));
                    retVal = DB.F2FCommand.ExecuteNonQuery();
                }
            }
        }
        private void insertSubmissionSegMapping(int submissionId, DataTable dt, string CreateBy)
        {
            int retVal;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    DataRow dr = dt.Rows[i];
                    DB.F2FCommand.CommandText = "SUBM_insertSubmissionSegment";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Clear();
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SMID", F2FDbType.VarChar, submissionId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SegmentId", F2FDbType.Int32, dr["SegmentId"]));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, CreateBy));
                    retVal = DB.F2FCommand.ExecuteNonQuery();
                }
            }
        }
        private void insertSubReportingOwners(int submissionId, DataTable dt, string CreateBy)
        {
            int retVal;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    DataRow dr = dt.Rows[i];
                    DB.F2FCommand.CommandText = "SUBM_insertSubReportingOwners";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Clear();
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SMID", F2FDbType.VarChar, submissionId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OwnerId", F2FDbType.Int32, dr["OwnerId"]));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, CreateBy));
                    retVal = DB.F2FCommand.ExecuteNonQuery();
                }
            }
        }

        public int generateSubmissionsWiseChecklist(int intSubId, string strEffectivedt, string strConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_generateSubChklistSubmissionWise";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubmissionId", F2FDbType.Int32, intSubId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EffectiveDate", F2FDbType.DateTime, strEffectivedt));
                    intRowsInserted = DB.F2FCommand.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in generateSubmissionsWiseChecklist() " + ex);
                }
            }
            return intRowsInserted;
        }

        public int generateSubmissionsWiseEventChecklist(int intSubId, string strEEM_ID, string strConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_generateSubmissionsWiseEventChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubmissionId", F2FDbType.Int32, intSubId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EEMID", F2FDbType.VarChar, strEEM_ID));

                    DB.F2FCommand.ExecuteNonQuery();

                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in generateSubmissionsWiseEventChecklist() " + ex);
                }
            }
            return intRowsInserted;
        }

        public int generateEComplySubChklist(int intFinYearId, string strConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_generateEComplySubChklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FinYearId", F2FDbType.Int32, intFinYearId));

                    intRowsInserted = DB.F2FCommand.ExecuteNonQuery();

                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in generateEComplySubChklist() " + ex);
                }
            }
            return intRowsInserted;
        }

        public int TrancateChecklistTables(string strConnectionString)
        {
            int intRowsDeleted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_TrancateChecklistTables";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;

                    intRowsDeleted = DB.F2FCommand.ExecuteNonQuery();

                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in TrancateChecklistTables() " + ex);
                }
            }
            return intRowsDeleted;
        }

        public DataSet LoadSubmissionChecklist(String strSelectedMonth, String strUser, String mstrConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_DepartmentChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SelectedMonth", F2FDbType.VarChar, strSelectedMonth));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UserName", F2FDbType.VarChar, strUser));
                    DB.F2FDataAdapter.Fill(resultDataSet);
                    return resultDataSet;
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in LoadChecklist() " + ex);
                }
            }
        }


        public DataSet LoadMySubmissionChecklist(String strSelectedMonth, String strEmployeeName, String mstrConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_LoadMySubmissionChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EmployeeName", F2FDbType.VarChar, strEmployeeName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SelectedMonth", F2FDbType.VarChar, strSelectedMonth));
                    DB.F2FDataAdapter.Fill(resultDataSet);
                    return resultDataSet;
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in LoadMySubmissionChecklist() " + ex);
                }
            }
        }
        public int saveChecklistDetails(int intSubId, int intChecklistId, string strYesNoNA, string strRemarks, string strStatus,
                                        string strCurrentUser, string mstrConnectionString)
        {
            int intRowsInserted;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {

                    DB.F2FCommand.CommandText = "SUBM_insertChecklistDetails";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubId", F2FDbType.Int32, intSubId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ChecklistId", F2FDbType.Int32, intChecklistId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@YesNoNA", F2FDbType.VarChar, strYesNoNA));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Remarks", F2FDbType.VarChar, strRemarks));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCurrentUser));

                    intRowsInserted = DB.F2FCommand.ExecuteNonQuery();

                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in saveChecklistDetails() " + ex);
                }
            }
            return intRowsInserted;
        }

        public DataSet SearchSubmissionChecklistForFix(String strUser, String strFilterExpression, string strOrderBy, String mstrConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_SearchSubmissionChecklistForFixDate";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FilterCondition", F2FDbType.VarChar, strFilterExpression));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EmployeeName", F2FDbType.VarChar, strUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OrderBy", F2FDbType.VarChar, strOrderBy));
                    DB.F2FDataAdapter.Fill(resultDataSet);
                    return resultDataSet;
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in SearchSubmissionChecklistForFix() " + ex);
                }
            }
        }

        public DataSet SearchSubmissionChecklistForEventBased(String strUser, String strFilterCondition, string strFilterEvent, string strOrderBy, String mstrConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_SearchSubmissionChecklistForEventBased";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FilterCondition", F2FDbType.VarChar, strFilterCondition));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EmployeeName", F2FDbType.VarChar, strUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FilterEvent", F2FDbType.VarChar, strFilterEvent));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OrderBy", F2FDbType.VarChar, strOrderBy));
                    DB.F2FDataAdapter.Fill(resultDataSet);
                    return resultDataSet;
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in SearchSubmissionChecklistForEventBased() " + ex);
                }
            }
        }

        public int insertSubmissionFiles(int intSubmissionId, DataTable dt, string strUser, string strOperationType, string mstrConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    if (dt != null)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            DB.F2FCommand.CommandText = "SUBM_insertSubmissionFiles";
                            DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                            DB.F2FCommand.Parameters.Clear();
                            DB.F2FCommand.Parameters.Add(F2FParameter("@SubmissionId", F2FDbType.Int32, intSubmissionId));
                            DB.F2FCommand.Parameters.Add(F2FParameter("@fileName", F2FDbType.VarChar, dr["file Name"].ToString()));
                            DB.F2FCommand.Parameters.Add(F2FParameter("@fileNameonServer", F2FDbType.VarChar, dr["fileNameonServer"].ToString()));
                            DB.F2FCommand.Parameters.Add(F2FParameter("@Uploader", F2FDbType.VarChar, strUser));
                            DB.F2FCommand.Parameters.Add(F2FParameter("@OperationType", F2FDbType.VarChar, strOperationType));
                            DB.F2FCommand.Parameters.Add(F2FParameter("@FileType", F2FDbType.VarChar, dr["File Type"].ToString()));
                            DB.F2FCommand.Parameters.Add(F2FParameter("@FileDesc", F2FDbType.VarChar, dr["File Description"].ToString()));
                            retVal = DB.F2FCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in insertSubmissionFiles() " + ex);
                }
            }
            return retVal;
        }

        public DataSet SearchSubmissions(int intSubmissionsId, string strReportingFun, string strFrequency, string strStatus, string strSegment, string strSubType, string strDepartment, string
                                            strEvent, string strEventPurpose, string strEmpName, string strType, string mstrConnectionString)
        {
            DataSet dsResults = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_SearchSubmissions";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubmissionsId", F2FDbType.Int32, intSubmissionsId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Frequency", F2FDbType.VarChar, strFrequency));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Segment", F2FDbType.VarChar, strSegment));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubType", F2FDbType.VarChar, strSubType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Department", F2FDbType.VarChar, strDepartment));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Event", F2FDbType.VarChar, strEvent));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EventPurpose", F2FDbType.VarChar, strEventPurpose));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ReportingFun", F2FDbType.VarChar, strReportingFun));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EmpName", F2FDbType.VarChar, strEmpName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FDataAdapter.Fill(dsResults);
                    return dsResults;
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in SearchSubmissions() " + ex);
                }
            }
        }
        //<<Modified by Ashish Mishra on 27Jul2017
        //<< added by prajakta
        public int saveAdminChecklist(int intSubId, string strStatus, string strCurrentUser, string strSubAuthorityDate,
                                        string strReOpenComments, string strModeOfFiling, string strReopenPurpose, string strHygiene, string mstrConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_updateAdminChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubId", F2FDbType.Int32, intSubId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCurrentUser));
                    //<<Added by Ashish Mishra on 27Jul2017
                    if (strSubAuthorityDate.Equals(""))
                        DB.F2FCommand.Parameters.Add(F2FParameter("@SubAuthorityDate", F2FDbType.VarChar, DBNull.Value));
                    else
                        DB.F2FCommand.Parameters.Add(F2FParameter("@SubAuthorityDate", F2FDbType.VarChar, strSubAuthorityDate));
                    //>>
                    //<<Added by Ashish Mishra on 28Jul2017
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ReOpenComments", F2FDbType.VarChar, strReOpenComments));
                    //>>
                    //<<Added by Ashish Mishra on 16Aug2017
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ModeOfFiling", F2FDbType.VarChar, strModeOfFiling));
                    //>>
                    //<<Added by Ankur Tyagi on 05-May-2025 for Project Id : 2395
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ReopenPurpose", F2FDbType.VarChar, strReopenPurpose));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Hygiene", F2FDbType.VarChar, strHygiene));
                    //>>
                    intRowsInserted = DB.F2FCommand.ExecuteNonQuery();

                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intRowsInserted;
        }
        //>>

        public DataSet LoadPastChecklist(String strSelectedMonth, String strSelectedFinYear, string strUserType, string strUserName, string strGlobal, String mstrConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_LoadPastSubmissionChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SelectedMonth", F2FDbType.VarChar, strSelectedMonth));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SelectedFinYear", F2FDbType.VarChar, strSelectedFinYear));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strUserType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EmpName", F2FDbType.VarChar, strUserName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@GlobalSearch", F2FDbType.VarChar, strGlobal));
                    DB.F2FDataAdapter.Fill(resultDataSet);
                    return resultDataSet;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return resultDataSet;
        }
        //>>
        public int inactiveSubmission(int intSubId, string strDuedate, string strStatus, string strDeActRemarks, string strCurrentUser, string mstrConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {

                    DB.F2FCommand.CommandText = "SUBM_inactiveSubmission";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SM_ID", F2FDbType.Int32, intSubId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SC_DUE_DATE_TO", F2FDbType.VarChar, strDuedate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SM_STATUS", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SM_LST_UPD_BY", F2FDbType.VarChar, strCurrentUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DeActRemarks", F2FDbType.VarChar, strDeActRemarks));

                    intRowsInserted = DB.F2FCommand.ExecuteNonQuery();

                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intRowsInserted;
        }


        public int UpdateSubmissionDetails(int intEditSubmissionId, string strStatus, int intSubType, int intReportDept,
            string strType, int intEvent, int intAssociatedWith, string strParticulars, string strDescription,
            int intStartDays, int intEndDays, string strEscalate, string strFrequency, string strOnceFromDate,
            string strOnceToDate, string strFromWeekDays, string strToWeekDays, string strMonthlyFromDate,
            string strMonthlyTodate, string strQ1fromDate, string strQ1ToDate, string strQ2FromDate, string strQ2ToDate,
            string strQ3FromDate, string strQ3ToDate, string strQ4fFromDate, string strQ4Todate, string strFirstHalffromDate,
            string strFirstHalfToDate, string strSecondtHalffromDate, string strSecondtHalffromTo, string strYearlyfromDate,
            string strYearlyDateTo, string strF1Fromdate, string strF1ToDate, string strF2FromDate, string strF2ToDate,
            int intlevel1, int intlevel2, string strEffectiveDate, string strPriority,
            string strCreateBy, string strOpType, string strConnectionString)
        {
            int intSubmissionId = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "SUBM_UpdateSubmissionDetails";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SMID", F2FDbType.Int32, intEditSubmissionId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubmissionType", F2FDbType.Int32, intSubType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ReportDeptID", F2FDbType.Int32, intReportDept));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Event", F2FDbType.Int32, intEvent));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EventPurpose", F2FDbType.Int32, intAssociatedWith));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Particulars", F2FDbType.VarChar, strParticulars));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Description", F2FDbType.VarChar, strDescription));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@StartDate", F2FDbType.Int32, intStartDays));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EndDate", F2FDbType.Int32, intEndDays));
                    //DB.F2FCommand.Parameters.Add(F2FParameter("@SortOrder", F2FDbType.Int32, 10, intSortOrder));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@TobeEscalate", F2FDbType.VarChar, strEscalate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Frequency", F2FDbType.VarChar, strFrequency));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OnceFromDate", F2FDbType.VarChar, strOnceFromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OnceToDate", F2FDbType.VarChar, strOnceToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FromWeekDays", F2FDbType.VarChar, strFromWeekDays));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ToWeekDays", F2FDbType.VarChar, strToWeekDays));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@MonthlyFromDate", F2FDbType.VarChar, strMonthlyFromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@MonthlyTodate", F2FDbType.VarChar, strMonthlyTodate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q1fromDate", F2FDbType.VarChar, strQ1fromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q1ToDate", F2FDbType.VarChar, strQ1ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q2FromDate", F2FDbType.VarChar, strQ2FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q2ToDate", F2FDbType.VarChar, strQ2ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q3FromDate", F2FDbType.VarChar, strQ3FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q3ToDate", F2FDbType.VarChar, strQ3ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q4FromDate", F2FDbType.VarChar, strQ4fFromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q4Todate", F2FDbType.VarChar, strQ4Todate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstHalffromDate", F2FDbType.VarChar, strFirstHalffromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstHalfToDate", F2FDbType.VarChar, strFirstHalfToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondHalffromDate", F2FDbType.VarChar, strSecondtHalffromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondHalfToDate", F2FDbType.VarChar, strSecondtHalffromTo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@YearlyfromDate", F2FDbType.VarChar, strYearlyfromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@YearlyDateTo", F2FDbType.VarChar, strYearlyDateTo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Level1", F2FDbType.Int32, intlevel1));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Level2", F2FDbType.Int32, intlevel2));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Priority", F2FDbType.VarChar, strPriority));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Effectivedt", F2FDbType.VarChar, strEffectiveDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OperationType", F2FDbType.VarChar, strOpType));
                    //<< Added by Vivek on 22-Jun-2017
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstF1FromDate", F2FDbType.VarChar, strF1Fromdate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstF1ToDate", F2FDbType.VarChar, strF1ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondF2FromDate", F2FDbType.VarChar, strF2FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondF2ToDate", F2FDbType.VarChar, strF2ToDate));
                    //>>
                    DB.F2FCommand.ExecuteNonQuery();
                    DB.F2FTransaction.Commit();
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intSubmissionId;
        }

        public DataSet SearchSubmissionsByOwners(int intOwner, int intRepOwner, string mstrConnectionString)
        {
            DataSet dsResults = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_SearchSubmissionsByOwners";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Owner", F2FDbType.Int32, intOwner));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@RepOwner", F2FDbType.Int32, intRepOwner));
                    DB.F2FDataAdapter.Fill(dsResults);
                    return dsResults;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dsResults;
        }

        public int insertSubmissionOwners(int intEMId, int intSMId, string strCreateBy, string strConnectionString)
        {
            int intSubmissionId = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_insertSubmissionOwnersNew";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SMId", F2FDbType.Int32, intSMId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EMId", F2FDbType.Int32, intEMId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    intSubmissionId = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intSubmissionId;
        }

        public int insertSubmissionReportingOwners(int intOwnerId, int intSMId, string strCreateBy, string strConnectionString)
        {
            int intSubmissionId = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_insertSubmissionReportingOwners";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SMId", F2FDbType.Int32, intSMId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OwnerId", F2FDbType.Int32, intOwnerId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    intSubmissionId = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intSubmissionId;
        }

        public int saveFilingDetsInEditMode(int intEditSubmissionId, string strStatus, int intSubType, int intReportDept,
            string strType, int intEvent, int intAssociatedWith, string strParticulars, string strDescription, int intStartDays,
            int intEndDays, string strEscalate, string strFrequency, string strOnceFromDate, string strOnceToDate,
            string strFromWeekDays, string strToWeekDays, string strMonthlyFromDate, string strMonthlyTodate,
            string strQ1fromDate, string strQ1ToDate, string strQ2FromDate, string strQ2ToDate, string strQ3FromDate,
            string strQ3ToDate, string strQ4fFromDate, string strQ4Todate, string strFirstHalffromDate, string strFirstHalfToDate,
            string strSecondtHalffromDate, string strSecondtHalffromTo, string strYearlyfromDate, string strYearlyDateTo,
            string strF1Fromdate, string strF1ToDate, string strF2FromDate, string strF2ToDate,
            int intlevel1, int intlevel2, string strEffectiveDate, string strPriority, string strCreateBy, string strOpType,
            string strConnectionString, DataTable mdtOwners, DataTable mdtRepOwners)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "SUBM_UpdateSubmissionDetails";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SMID", F2FDbType.Int32, intEditSubmissionId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubmissionType", F2FDbType.Int32, intSubType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ReportDeptID", F2FDbType.Int32, intReportDept));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Event", F2FDbType.Int32, intEvent));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EventPurpose", F2FDbType.Int32, intAssociatedWith));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Particulars", F2FDbType.VarChar, strParticulars));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Description", F2FDbType.VarChar, strDescription));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@StartDate", F2FDbType.Int32, intStartDays));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EndDate", F2FDbType.Int32, intEndDays));
                    //DB.F2FCommand.Parameters.Add(F2FParameter("@SortOrder", F2FDbType.Int32, 10, intSortOrder));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@TobeEscalate", F2FDbType.VarChar, strEscalate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Frequency", F2FDbType.VarChar, strFrequency));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OnceFromDate", F2FDbType.VarChar, strOnceFromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OnceToDate", F2FDbType.VarChar, strOnceToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FromWeekDays", F2FDbType.VarChar, strFromWeekDays));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ToWeekDays", F2FDbType.VarChar, strToWeekDays));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@MonthlyFromDate", F2FDbType.VarChar, strMonthlyFromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@MonthlyTodate", F2FDbType.VarChar, strMonthlyTodate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q1fromDate", F2FDbType.VarChar, strQ1fromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q1ToDate", F2FDbType.VarChar, strQ1ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q2FromDate", F2FDbType.VarChar, strQ2FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q2ToDate", F2FDbType.VarChar, strQ2ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q3FromDate", F2FDbType.VarChar, strQ3FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q3ToDate", F2FDbType.VarChar, strQ3ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q4FromDate", F2FDbType.VarChar, strQ4fFromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Q4Todate", F2FDbType.VarChar, strQ4Todate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstHalffromDate", F2FDbType.VarChar, strFirstHalffromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstHalfToDate", F2FDbType.VarChar, strFirstHalfToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondHalffromDate", F2FDbType.VarChar, strSecondtHalffromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondHalfToDate", F2FDbType.VarChar, strSecondtHalffromTo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@YearlyfromDate", F2FDbType.VarChar, strYearlyfromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@YearlyDateTo", F2FDbType.VarChar, strYearlyDateTo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Level1", F2FDbType.Int32, intlevel1));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Level2", F2FDbType.Int32, intlevel2));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Priority", F2FDbType.VarChar, strPriority));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Effectivedt", F2FDbType.VarChar, strEffectiveDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OperationType", F2FDbType.VarChar, strOpType));
                    //<< Added by Vivek on 22-Jun-2017
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstF1FromDate", F2FDbType.VarChar, strF1Fromdate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FirstF1ToDate", F2FDbType.VarChar, strF1ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondF2FromDate", F2FDbType.VarChar, strF2FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SecondF2ToDate", F2FDbType.VarChar, strF2ToDate));
                    //>>
                    DB.F2FCommand.ExecuteNonQuery();
                    deleteOwner(intEditSubmissionId);
                    deleteReportingOwner(intEditSubmissionId);
                    insertSubmissionOwners(intEditSubmissionId, mdtOwners, strCreateBy);
                    insertSubReportingOwners(intEditSubmissionId, mdtRepOwners, strCreateBy);
                    DB.F2FTransaction.Commit();
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intRowsInserted;
        }

        private void deleteOwner(int intId)
        {
            int retVal;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FCommand.CommandText = "SUBM_deleteOwnerBySMID";
                DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                DB.F2FCommand.Parameters.Add(F2FParameter("@SMID", F2FDbType.Int32, intId));
                retVal = DB.F2FCommand.ExecuteNonQuery();
            }
        }
        private void deleteReportingOwner(int intId)
        {
            int retVal;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FCommand.CommandText = "SUBM_deleteRepOwnerBySMID";
                DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                DB.F2FCommand.Parameters.Add(F2FParameter("@SMID", F2FDbType.Int32, intId));
                retVal = DB.F2FCommand.ExecuteNonQuery();
            }
        }

        //<<Modified by Ashish Mishra on 27Jul2017
        public int saveChecklistDetails(int intSubId, int intChecklistId, string strYesNoNA,/* string strSubDate,*/ string strRemarks, string strStatus, string strCurrentUser, string strClientFileName, string strServerFileName, string mstrConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_insertChecklistDetails_New";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubId", F2FDbType.Int32, intSubId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ChecklistId", F2FDbType.Int32, intChecklistId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@YesNoNA", F2FDbType.VarChar, strYesNoNA));
                    //<<Commented by Ashish Mishra on 27Jul2017 
                    //DB.F2FCommand.Parameters.Add(F2FParameter("@SubDate", F2FDbType.VarChar, strSubDate));
                    //>>
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Remarks", F2FDbType.VarChar, strRemarks));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCurrentUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ClientFileName", F2FDbType.VarChar, strClientFileName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ServerFileName", F2FDbType.VarChar, strServerFileName));

                    intRowsInserted = DB.F2FCommand.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intRowsInserted;
        }
        //>>
        public DataSet LoadComplianceChecklist(String strSelectedMonth, String strUser, string strType, int intFinancialYearId,
            int intReportingDeptId, string strStatus, string strGlobalSearch, string strAuthority, string strFrequency, string strConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_LoadComplianceChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SelectedMonth", F2FDbType.VarChar, strSelectedMonth));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UserName", F2FDbType.VarChar, strUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ReportDeptId", F2FDbType.VarChar, intReportingDeptId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    //<<Added by Ankur Tyagi on 21-Apr-2025 for Project Id : 2395
                    DB.F2FCommand.Parameters.Add(F2FParameter("@GlobalSearch", F2FDbType.VarChar, strGlobalSearch));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Authority", F2FDbType.VarChar, strAuthority));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Frequency", F2FDbType.VarChar, strFrequency));
                    //>>
                    //Added By Milan yadav on 28-Apr-2016
                    //>>
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FinancialYearId", F2FDbType.VarChar, intFinancialYearId));
                    //<<
                    DB.F2FDataAdapter.Fill(resultDataSet);
                    return resultDataSet;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return resultDataSet;
        }

        public DataTable getReportingDeptOwners(String strSubChklistId, string strConnectionString)
        {
            DataTable dtReportingDeptOwners = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                string strSQL;
                try
                {
                    strSQL = "select SRDOM_EMP_NAME,SRDOM_EMAILID From " +
                            "TBL_SUB_SRD_OWNER_MASTER " +
                            "inner join TBL_SUB_REPORTING_DEPT on " +
                            "SRD_ID = SRDOM_SRD_ID " +
                            "inner join TBL_SUBMISSIONS_MAS ON SM_SRD_ID = SRD_ID " +
                            "inner join  TBL_SUB_CHKLIST ON SM_ID = SC_SM_ID " +
                            " AND SC_ID = " + strSubChklistId;

                    DB.F2FCommand.CommandText = strSQL;
                    DB.F2FCommand.CommandType = CommandType.Text;
                    DB.F2FDataAdapter.Fill(dtReportingDeptOwners);
                    return dtReportingDeptOwners;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dtReportingDeptOwners;
        }

        public DataTable getTrackingDeptOwners(String strSubChklistId, string strConnectionString)
        {
            DataTable dtTrackingDeptOwners = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                string strSQL;
                try
                {
                    strSQL = "SELECT EM_EMPNAME, EM_EMAIL From " +
                       " EmployeeMaster " +
                       " INNER JOIN TBL_EM_STM_MAPPING ON ESM_EM_ID = EM_ID " +
                       " INNER JOIN TBL_SUBMISSIONS_MAS ON SM_STM_ID = ESM_STM_ID " +
                       " INNER JOIN TBL_SUB_CHKLIST ON SM_ID = SC_SM_ID " +
                       " AND SC_ID = " + strSubChklistId + "" +
                       " WHERE EM_STATUS='A'";

                    DB.F2FCommand.CommandText = strSQL;
                    DB.F2FCommand.CommandType = CommandType.Text;
                    DB.F2FDataAdapter.Fill(dtTrackingDeptOwners);
                    return dtTrackingDeptOwners;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dtTrackingDeptOwners;
        }

        //Added by Bhavik 13Dec2014
        public DataSet SearchComplianceChecklist(string strFilterCondition, string strUser, string strType, string strOrderBy, string strGlobalSearch,
            string mstrConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_SearchComplianceChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FilterCondition", F2FDbType.VarChar, strFilterCondition));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UserName", F2FDbType.VarChar, strUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OrderBy", F2FDbType.VarChar, strOrderBy));
                    //<<Modified by Ankur Tyagi on 18-Apr-2025 for Project Id : 2395
                    DB.F2FCommand.Parameters.Add(F2FParameter("@GlobalSearch", F2FDbType.VarChar, strGlobalSearch));
                    //>>
                    DB.F2FDataAdapter.Fill(resultDataSet);
                    return resultDataSet;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return resultDataSet;
        }


        public DataTable getListOfReports(int intSubmissionsId, string strReportingFun, string strFrequency, string strStatus,
            string strSegment, string strSubType, string strDepartment, string strEvent, string strEventPurpose, string strEmpName,
            string strType, string strCircId, string strGlobal, string mstrConnectionString)
        {
            DataTable dtListOfReports = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_getListOfReports";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubmissionsId", F2FDbType.Int32, intSubmissionsId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Frequency", F2FDbType.VarChar, strFrequency));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Segment", F2FDbType.VarChar, strSegment));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubType", F2FDbType.VarChar, strSubType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Department", F2FDbType.VarChar, strDepartment));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Event", F2FDbType.VarChar, strEvent));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EventPurpose", F2FDbType.VarChar, strEventPurpose));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ReportingFun", F2FDbType.VarChar, strReportingFun));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EmpName", F2FDbType.VarChar, strEmpName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularId", F2FDbType.Int32, (string.IsNullOrEmpty(strCircId) ? 0 : Convert.ToInt32(strCircId))));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@GlobalSearch", F2FDbType.VarChar, strGlobal));
                    DB.F2FDataAdapter.Fill(dtListOfReports);
                    return dtListOfReports;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dtListOfReports;
        }
        //Added By Milan Yadav on 25Apr2016
        //>>
        public int deleteSubmissions(int intSubmissionId, string strDeleteBy, string strConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                DB.F2FCommand.Transaction = DB.F2FTransaction;
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_DeleteSubmissions";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubmissionsId", F2FDbType.Int32, intSubmissionId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DeleteBy", F2FDbType.VarChar, strDeleteBy));

                    retVal = DB.F2FCommand.ExecuteNonQuery();

                    DB.F2FTransaction.Commit();
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retVal;
        }
        //<<

        public DataTable getChecklistForReopenClosureByMonth(string strMonth, string strFinYear, string strUsername, string strUserType,
                                                             string strType, string strStatus, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_getChecklistForReopenClosureByMonth";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Month", F2FDbType.VarChar, strMonth));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FinYear", F2FDbType.VarChar, strFinYear));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EmpName", F2FDbType.VarChar, strUsername));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UserType", F2FDbType.VarChar, strUserType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dt;
        }

        public int saveReportingDepartment(int intSRD_ID, string strSRD_NAME, string strSRD_STATUS, string strCurrentUser, string strReason, string mstrConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_saveReportingDepartment";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRD_ID", F2FDbType.Int32, intSRD_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRD_NAME", F2FDbType.VarChar, strSRD_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRD_STATUS", F2FDbType.VarChar, strSRD_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CurrentUser", F2FDbType.VarChar, strCurrentUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EditReason", F2FDbType.VarChar, strReason));

                    intRowsInserted = DB.F2FCommand.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intRowsInserted;
        }

        public DataTable getReportingDepartment(int intSRD_ID, string strSRD_NAME, string strSRD_STATUS, string strCurrentUser, string strCheckDuplicate, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_getReportingDepartment";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRD_ID", F2FDbType.Int32, intSRD_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRD_NAME", F2FDbType.VarChar, strSRD_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRD_STATUS", F2FDbType.VarChar, strSRD_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CurrentUser", F2FDbType.VarChar, strCurrentUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CheckDuplicate", F2FDbType.VarChar, strCheckDuplicate));
                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dt;
        }

        public int saveReportingUsers(int intSRDOM_ID, int intSRDOM_SRD_ID, string strSRDOM_EMP_NAME, string strSRDOM_EMAILID, string strSRDOM_EMP_ID,
            string strSRDOM_STATUS, string strCurrentUser, string strReason, string mstrConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_saveReportingOwners";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRDOM_ID", F2FDbType.Int32, intSRDOM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRDOM_SRD_ID", F2FDbType.Int32, intSRDOM_SRD_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRDOM_EMP_NAME", F2FDbType.VarChar, strSRDOM_EMP_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRDOM_EMAILID", F2FDbType.VarChar, strSRDOM_EMAILID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRDOM_EMP_ID", F2FDbType.VarChar, strSRDOM_EMP_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRDOM_STATUS", F2FDbType.VarChar, strSRDOM_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CurrentUser", F2FDbType.VarChar, strCurrentUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EditReason", F2FDbType.VarChar, strReason));

                    intRowsInserted = DB.F2FCommand.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intRowsInserted;
        }

        public DataTable getReportingUsers(int intSRDOM_ID, int intSRDOM_SRD_ID, string strSRDOM_EMP_NAME, string strSRDOM_EMAILID, string strSRDOM_EMP_ID,
            string strSRDOM_STATUS, string strCurrentUser, string mstrConnectionString, string strCheckDuplicate)
        {
            DataTable dt = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_getReportingOwners";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRDOM_ID", F2FDbType.Int32, intSRDOM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRDOM_SRD_ID", F2FDbType.Int32, intSRDOM_SRD_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRDOM_EMP_NAME", F2FDbType.VarChar, strSRDOM_EMP_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRDOM_EMAILID", F2FDbType.VarChar, strSRDOM_EMAILID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRDOM_EMP_ID", F2FDbType.VarChar, strSRDOM_EMP_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRDOM_STATUS", F2FDbType.VarChar, strSRDOM_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CurrentUser", F2FDbType.VarChar, strCurrentUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CheckDuplicate", F2FDbType.VarChar, strCheckDuplicate));
                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dt;
        }

        public DataTable getReportingDepartmentDetails(string strSRD_NAME, string strSRDOM_EMP_NAME, string strLevel, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_getReportingDepartmentDetails";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRD_NAME", F2FDbType.VarChar, strSRD_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SRDOM_EMP_NAME", F2FDbType.VarChar, strSRDOM_EMP_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Level", F2FDbType.VarChar, strLevel));
                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dt;
        }

        public int saveReportingEscalations(int intSE_ID, int intSE_SRD_ID, string strSE_FIRST_NAME, string strSE_EMAIL_ID, string strSE_EMPLOYEE_ID,
            string strSE_STATUS, string strSE_LEVEL, string strCurrentUser, string strReason, string mstrConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_saveReportingEscalations";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SE_ID", F2FDbType.Int32, intSE_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SE_SRD_ID", F2FDbType.Int32, intSE_SRD_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SE_FIRST_NAME", F2FDbType.VarChar, strSE_FIRST_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SE_EMAIL_ID", F2FDbType.VarChar, strSE_EMAIL_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SE_EMPLOYEE_ID", F2FDbType.VarChar, strSE_EMPLOYEE_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SE_STATUS", F2FDbType.VarChar, strSE_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SE_LEVEL", F2FDbType.VarChar, strSE_LEVEL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CurrentUser", F2FDbType.VarChar, strCurrentUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EditReason", F2FDbType.VarChar, strReason));

                    intRowsInserted = DB.F2FCommand.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intRowsInserted;
        }

        public DataTable getReportingEscalations(int intSE_ID, int intSE_SRD_ID, string strSE_FIRST_NAME, string strSE_EMAIL_ID, string strSE_EMPLOYEE_ID,
            string strSE_STATUS, string strSE_LEVEL, string strCurrentUser, string mstrConnectionString, string strCheckDuplicate)
        {
            DataTable dt = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_getReportingEscalations";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SE_ID", F2FDbType.Int32, intSE_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SE_SRD_ID", F2FDbType.Int32, intSE_SRD_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SE_FIRST_NAME", F2FDbType.VarChar, strSE_FIRST_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SE_EMAIL_ID", F2FDbType.VarChar, strSE_EMAIL_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SE_EMPLOYEE_ID", F2FDbType.VarChar, strSE_EMPLOYEE_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SE_STATUS", F2FDbType.VarChar, strSE_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SE_LEVEL", F2FDbType.VarChar, strSE_LEVEL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CurrentUser", F2FDbType.VarChar, strCurrentUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CheckDuplicate", F2FDbType.VarChar, strCheckDuplicate));

                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dt;
        }

        public DataTable getTrackingDepartmentDetails(string strSTM_TYPE, string strEM_EMPNAME, string strEM_USERNAME, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_getTrackingDepartmentDetails";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@STM_TYPE", F2FDbType.VarChar, strSTM_TYPE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EM_EMPNAME", F2FDbType.VarChar, strEM_EMPNAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EM_USERNAME", F2FDbType.VarChar, strEM_USERNAME));
                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dt;
        }

        public int saveTrackingDepartment(int intSTM_ID, string strSTM_TYPE, string strSTM_STATUS, string strCurrentUser, string strReason, string mstrConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_saveTrackingDepartment";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@STM_ID", F2FDbType.Int32, intSTM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@STM_TYPE", F2FDbType.VarChar, strSTM_TYPE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@STM_STATUS", F2FDbType.VarChar, strSTM_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CurrentUser", F2FDbType.VarChar, strCurrentUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EditReason", F2FDbType.VarChar, strReason));

                    intRowsInserted = DB.F2FCommand.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intRowsInserted;
        }

        public DataTable getTrackingDepartment(int intSTM_ID, string strSTM_TYPE, string strSTM_STATUS, string strCurrentUser, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_getTrackingDepartment";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@STM_ID", F2FDbType.Int32, intSTM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@STM_TYPE", F2FDbType.VarChar, strSTM_TYPE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@STM_STATUS", F2FDbType.VarChar, strSTM_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CurrentUser", F2FDbType.VarChar, strCurrentUser));
                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dt;
        }

        public int saveEmployeeMaster(int intEM_ID, string strEM_EMPNAME, string strEM_EMAIL, string strEM_USERNAME, string strEM_STATUS,
            string strCurrentUser, string strReason, string mstrConnectionString, int intSTM_ID, int intESM_LEVEL)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_saveEmployeeMaster";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EM_ID", F2FDbType.Int32, intEM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EM_EMPNAME", F2FDbType.VarChar, strEM_EMPNAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EM_EMAIL", F2FDbType.VarChar, strEM_EMAIL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EM_USERNAME", F2FDbType.VarChar, strEM_USERNAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EM_STATUS", F2FDbType.VarChar, strEM_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CurrentUser", F2FDbType.VarChar, strCurrentUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EditReason", F2FDbType.VarChar, strReason));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@STM_ID", F2FDbType.VarChar, intSTM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ESM_LEVEL", F2FDbType.VarChar, intESM_LEVEL));

                    intRowsInserted = DB.F2FCommand.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intRowsInserted;
        }

        public DataTable getEmployeeMaster(int intEM_ID, int intSTM_ID, string strEM_EMPNAME, string strEM_EMAIL, string strEM_USERNAME, string strEM_STATUS, string strCurrentUser, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_getEmployeeMaster";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EM_ID", F2FDbType.Int32, intEM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@STM_ID", F2FDbType.Int32, intSTM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EM_EMPNAME", F2FDbType.VarChar, strEM_EMPNAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EM_EMAIL", F2FDbType.VarChar, strEM_EMAIL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EM_USERNAME", F2FDbType.VarChar, strEM_USERNAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EM_STATUS", F2FDbType.VarChar, strEM_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CurrentUser", F2FDbType.VarChar, strCurrentUser));
                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dt;
        }

        public int saveTrackingDepartmentEmployeeMapping(int intESM_ID, int intESM_EM_ID, int intESM_STM_ID, int intESM_LEVEL, string strCurrentUser, string mstrConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_saveTrackingDepartmentEmployeeMapping";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ESM_ID", F2FDbType.Int32, intESM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ESM_EM_ID", F2FDbType.Int32, intESM_EM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ESM_STM_ID", F2FDbType.Int32, intESM_STM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ESM_LEVEL", F2FDbType.Int32, intESM_LEVEL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CurrentUser", F2FDbType.VarChar, strCurrentUser));

                    intRowsInserted = DB.F2FCommand.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intRowsInserted;
        }

        public DataTable getTrackingDepartmentEmployeeMapping(int intESM_ID, int intESM_EM_ID, int intESM_STM_ID, int intESM_LEVEL, string strCurrentUser, string mstrConnectionString)
        {
            DataTable dt = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_getTrackingDepartmentEmployeeMapping";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ESM_ID", F2FDbType.Int32, intESM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ESM_EM_ID", F2FDbType.Int32, intESM_EM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ESM_STM_ID", F2FDbType.Int32, intESM_STM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ESM_LEVEL", F2FDbType.Int32, intESM_LEVEL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CurrentUser", F2FDbType.VarChar, strCurrentUser));
                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dt;
        }

        //<<Added by Ankur Tyagi on 10-May-2025 for Project Id : 2395
        public DataTable getSubmissionforApproval(int intSMId, String strUser, int intReportingDeptId, string strStatus, string strAuthority,
            string strGlobalSearch, string strConnectionString)
        {
            DataTable dt = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_getSubmissionApproval";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SMID", F2FDbType.VarChar, intSMId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UserName", F2FDbType.VarChar, strUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ReportDeptId", F2FDbType.VarChar, intReportingDeptId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Authority", F2FDbType.VarChar, strAuthority));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@GlobalSearch", F2FDbType.VarChar, strGlobalSearch));
                    DB.F2FDataAdapter.Fill(dt);
                    return dt;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dt;
        }

        public int updateSubmissionApproval(int intSmId, string strStatus, string strRemarks, string strLoggedInUser, string mstrConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_updateSubmissionApproval";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SMId", F2FDbType.Int32, intSmId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Remarks", F2FDbType.VarChar, strRemarks));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LoggedInUser", F2FDbType.VarChar, strLoggedInUser));
                    intRowsInserted = DB.F2FCommand.ExecuteNonQuery();

                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intRowsInserted;
        }

        public DataSet LoadComplianceChecklistForApproval(String strSelectedMonth, String strUser, int intFinancialYearId,
            int intReportingDeptId, string strStatus, string strGlobalSearch, string strAuthority, string strFrequency, string strConnectionString)
        {
            DataSet resultDataSet = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_LoadComplianceChecklistForApproval";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SelectedMonth", F2FDbType.VarChar, strSelectedMonth));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UserName", F2FDbType.VarChar, strUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FinancialYearId", F2FDbType.VarChar, intFinancialYearId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ReportDeptId", F2FDbType.VarChar, intReportingDeptId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@GlobalSearch", F2FDbType.VarChar, strGlobalSearch));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Authority", F2FDbType.VarChar, strAuthority));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Frequency", F2FDbType.VarChar, strFrequency));
                    DB.F2FDataAdapter.Fill(resultDataSet);
                    return resultDataSet;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return resultDataSet;
        }

        public int updateExtensionDate(int intSCId, string strExtensionDate, string strExtensionRemarks,
            string strDoneBy, string strConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "SUBM_updateExtensionDate";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SCId", F2FDbType.VarChar, intSCId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ExtensionDate", F2FDbType.VarChar, strExtensionDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ExtensionRemarks", F2FDbType.VarChar, strExtensionRemarks));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DoneBy", F2FDbType.VarChar, strDoneBy));
                    intRowsInserted = DB.F2FCommand.ExecuteNonQuery();

                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intRowsInserted;
        }
        //>>

    }
}
