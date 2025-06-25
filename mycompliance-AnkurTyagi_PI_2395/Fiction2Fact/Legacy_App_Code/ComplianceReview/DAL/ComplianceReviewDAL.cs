using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fiction2Fact.App_Code;
using static Fiction2Fact.App_Code.F2FDatabase;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Spreadsheet;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Fiction2Fact.Legacy_App_Code.Compliance.DAL
{
    public class ComplianceReviewDAL
    {

        //<< Added by Ritesh Tak on 22-03-2023

        CommonMethods cm = new CommonMethods();


        #region Compliance Review 


        public int SaveComplianceReview(int intCCR_ID, string strCCR_IDENTIFIER, int intCCR_CRUM_ID, string strCCR_UNIT_IDS, string strCCR_QUARTER, DateTime dtCCR_TENTATIVE_START_DATE, DateTime dtCCR_TENTATIVE_END_DATE, string strCCR_STATUS,
            string strCCR_REC_STATUS, string strCCR_REC_STATUS_REMARKS, int intCCR_CRM_ID, string strCCR_REVIEW_SCOPE, string strCCR_REVIEW_OBJECTIVE, string strCCR_REVIEW_STAGE,
            string strCCR_REMARKS, string strCCR_REVIEW_TYPE, DateTime dtCCR_WORK_STARTED_ON, string strCCR_APPROVAL_BY_L1, DateTime dtCCR_APPROVAL_ON_L1, string strCCR_APPROVAL_BY_L2, DateTime dtCCR_APPROVAL_ON_L2, string strCCR_CREATOR, string strCCR_AUDIT_TRAIL,
            string strCCR_CREATE_BY, string strCCR_UPDATE_BY, string strCCR_LINKAGE_WITH_EARLIER_CIRCULAR, string strCCR_SOC_EOC,
            string strCCR_OLD_CIRC_SUB_NO, string strCCR_BASE_ID)
        {
            int intCircularId = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CR_CREATEUPDATE_COMP_REVIEW";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;

                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_ID", F2FDbType.Int32, intCCR_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_IDENTIFIER", F2FDbType.VarChar, strCCR_IDENTIFIER));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_CRUM_ID", F2FDbType.Int32, intCCR_CRUM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_UNIT_IDS", F2FDbType.VarChar, strCCR_UNIT_IDS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_QUARTER", F2FDbType.VarChar, strCCR_QUARTER));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_TENTATIVE_START_DATE", F2FDbType.DateTime, dtCCR_TENTATIVE_START_DATE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_TENTATIVE_END_DATE", F2FDbType.DateTime, dtCCR_TENTATIVE_END_DATE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_STATUS", F2FDbType.VarChar, strCCR_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_REC_STATUS", F2FDbType.VarChar, strCCR_REC_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_REC_STATUS_REMARKS", F2FDbType.VarChar, strCCR_REC_STATUS_REMARKS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_CRM_ID", F2FDbType.Int32, intCCR_CRM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_REVIEW_SCOPE", F2FDbType.VarChar, strCCR_REVIEW_SCOPE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_REVIEW_OBJECTIVE", F2FDbType.VarChar, strCCR_REVIEW_OBJECTIVE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_REVIEW_STAGE", F2FDbType.VarChar, strCCR_REVIEW_STAGE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_REMARKS", F2FDbType.VarChar, strCCR_REMARKS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_REVIEW_TYPE", F2FDbType.VarChar, strCCR_REVIEW_TYPE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_WORK_STARTED_ON", F2FDbType.DateTime, dtCCR_WORK_STARTED_ON));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_APPROVAL_BY_L1", F2FDbType.VarChar, strCCR_APPROVAL_BY_L1));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_APPROVAL_ON_L1", F2FDbType.DateTime, dtCCR_APPROVAL_ON_L1));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_APPROVAL_BY_L2", F2FDbType.VarChar, strCCR_APPROVAL_BY_L2));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_APPROVAL_ON_L2", F2FDbType.DateTime, dtCCR_APPROVAL_ON_L2));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_CREATOR", F2FDbType.VarChar, strCCR_CREATOR));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_AUDIT_TRAIL", F2FDbType.VarChar, strCCR_AUDIT_TRAIL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_CREATE_BY", F2FDbType.VarChar, strCCR_CREATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_UPDATE_BY", F2FDbType.VarChar, strCCR_UPDATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_LINKAGE_WITH_EARLIER_CIRCULAR", F2FDbType.VarChar, strCCR_LINKAGE_WITH_EARLIER_CIRCULAR));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_SOC_EOC", F2FDbType.VarChar, strCCR_SOC_EOC));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_OLD_CIRC_SUB_NO", F2FDbType.VarChar, strCCR_OLD_CIRC_SUB_NO));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_BASE_ID", F2FDbType.VarChar, strCCR_BASE_ID));

                    intCircularId = Convert.ToInt32(DB.F2FCommand.ExecuteNonQuery());
                    DB.F2FTransaction.Commit();
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intCircularId;
        }

        public DataTable Search_ComplianceReview(int intCCR_ID, int intCCR_CRUM_ID, int intCCR_CRM_ID, string dtCCR_TENTATIVE_START_DATE, string dtCCR_TENTATIVE_END_DATE, string strCCR_CREATE_BY, string strType, string strStatus = null)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_Search_Comp_Review";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_ID", F2FDbType.Int32, intCCR_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_CRUM_ID", F2FDbType.Int32, intCCR_CRUM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_CRM_ID", F2FDbType.Int32, intCCR_CRM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_TENTATIVE_START_DATE", F2FDbType.VarChar, dtCCR_TENTATIVE_START_DATE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_TENTATIVE_END_DATE", F2FDbType.VarChar, dtCCR_TENTATIVE_END_DATE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_CREATE_BY", F2FDbType.VarChar, strCCR_CREATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
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


        public int SaveComplianceReview_with_Files(int intCCR_ID, string strCCR_IDENTIFIER, int intCCR_CRUM_ID, string strCCR_UNIT_IDS, string strCCR_QUARTER, DateTime? dtCCR_TENTATIVE_START_DATE, DateTime? dtCCR_TENTATIVE_END_DATE, string strCCR_STATUS,
           string strCCR_REC_STATUS, string strCCR_REC_STATUS_REMARKS, int intCCR_CRM_ID, string strCCR_REVIEW_SCOPE, string strCCR_REVIEW_OBJECTIVE, string strCCR_REVIEW_STAGE,
           string strCCR_REMARKS, string strCCR_REVIEW_TYPE, DateTime? dtCCR_WORK_STARTED_ON, string strCCR_APPROVAL_BY_L1, DateTime? dtCCR_APPROVAL_ON_L1, string strCCR_APPROVAL_BY_L2, DateTime? dtCCR_APPROVAL_ON_L2, string strCCR_CREATOR, string strCCR_AUDIT_TRAIL,
           string strCCR_CREATE_BY, string strCCR_UPDATE_BY, DataTable dt, string strCCR_LINKAGE_WITH_EARLIER_CIRCULAR, string strCCR_SOC_EOC,
            string strCCR_OLD_CIRC_SUB_NO, string strCCR_BASE_ID)
        {
            int intCircularId = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CR_CREATEUPDATE_COMP_REVIEW";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;

                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_ID", F2FDbType.Int32, intCCR_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_IDENTIFIER", F2FDbType.VarChar, strCCR_IDENTIFIER));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_CRUM_ID", F2FDbType.Int32, intCCR_CRUM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_UNIT_IDS", F2FDbType.VarChar, strCCR_UNIT_IDS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_QUARTER", F2FDbType.VarChar, strCCR_QUARTER));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_TENTATIVE_START_DATE", F2FDbType.DateTime, dtCCR_TENTATIVE_START_DATE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_TENTATIVE_END_DATE", F2FDbType.DateTime, dtCCR_TENTATIVE_END_DATE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_STATUS", F2FDbType.VarChar, strCCR_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_REC_STATUS", F2FDbType.VarChar, strCCR_REC_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_REC_STATUS_REMARKS", F2FDbType.VarChar, strCCR_REC_STATUS_REMARKS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_CRM_ID", F2FDbType.Int32, intCCR_CRM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_REVIEW_SCOPE", F2FDbType.VarChar, strCCR_REVIEW_SCOPE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_REVIEW_OBJECTIVE", F2FDbType.VarChar, strCCR_REVIEW_OBJECTIVE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_REVIEW_STAGE", F2FDbType.VarChar, strCCR_REVIEW_STAGE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_REMARKS", F2FDbType.VarChar, strCCR_REMARKS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_REVIEW_TYPE", F2FDbType.VarChar, strCCR_REVIEW_TYPE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_WORK_STARTED_ON", F2FDbType.DateTime, dtCCR_WORK_STARTED_ON));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_APPROVAL_BY_L1", F2FDbType.VarChar, strCCR_APPROVAL_BY_L1));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_APPROVAL_ON_L1", F2FDbType.DateTime, dtCCR_APPROVAL_ON_L1));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_APPROVAL_BY_L2", F2FDbType.VarChar, strCCR_APPROVAL_BY_L2));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_APPROVAL_ON_L2", F2FDbType.DateTime, dtCCR_APPROVAL_ON_L2));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_CREATOR", F2FDbType.VarChar, strCCR_CREATOR));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_AUDIT_TRAIL", F2FDbType.VarChar, strCCR_AUDIT_TRAIL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_CREATE_BY", F2FDbType.VarChar, strCCR_CREATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_UPDATE_BY", F2FDbType.VarChar, strCCR_UPDATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_LINKAGE_WITH_EARLIER_CIRCULAR", F2FDbType.VarChar, strCCR_LINKAGE_WITH_EARLIER_CIRCULAR));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_SOC_EOC", F2FDbType.VarChar, strCCR_SOC_EOC));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_OLD_CIRC_SUB_NO", F2FDbType.VarChar, strCCR_OLD_CIRC_SUB_NO));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCR_BASE_ID", F2FDbType.VarChar, strCCR_BASE_ID));
                    intCircularId = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                    if (intCircularId > 0)
                    {
                        #region insert into review files
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                Save_Compliance_Review_Files(0, intCircularId, dr["Type"].ToString(), dr["FileTypeId"].ToString(), dr["ServerFileName"].ToString(), dr["ClientFileName"].ToString(), dr["FileTypeDesc"].ToString(), strCCR_CREATE_BY, strCCR_UPDATE_BY);
                            }
                        }
                        #endregion
                    }
                    DB.F2FTransaction.Commit();
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intCircularId;
        }


        public DataTable getComplianceReview_Approval(string strType, string strCreatedBy, string strFilter1 = null)
        {
            DataTable dt = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_getCompReviewForApproval";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreatedBy", F2FDbType.VarChar, strCreatedBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Filter1", F2FDbType.VarChar, strFilter1));
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

        public DataTable getComplianceReviewApproval_Status(string strOperation, int intCR_ID, string strFilter1 = null)
        {
            DataTable dt = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_Get_Complaince_ApprovalStatus";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Operation", F2FDbType.VarChar, strOperation));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CR_ID", F2FDbType.Int32, intCR_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Filter1", F2FDbType.VarChar, strFilter1));
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

        #endregion

        #region Compliance Review Files

        public int Save_Compliance_Review_Files(int intCCRF_ID, int intCCRF_CCR_ID, string strCCRF_TYPE, string strCCRF_FILE_TYPE, string strCCRF_SERVER_FILE_NAME, string strCCRF_CLIENT_FILE_NAME, string strCCRF_DESC, string strCCRF_CREATE_BY, string strCCRF_UPDATE_BY)
        {
            int intCircularId = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CR_Save_Comp_ReviewFile";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;

                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCRF_ID", F2FDbType.Int32, intCCRF_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCRF_CCR_ID", F2FDbType.VarChar, intCCRF_CCR_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCRF_TYPE", F2FDbType.VarChar, strCCRF_TYPE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCRF_FILE_TYPE", F2FDbType.VarChar, strCCRF_FILE_TYPE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCRF_SERVER_FILE_NAME", F2FDbType.VarChar, strCCRF_SERVER_FILE_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCRF_CLIENT_FILE_NAME", F2FDbType.VarChar, strCCRF_CLIENT_FILE_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCRF_DESC", F2FDbType.VarChar, strCCRF_DESC));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCRF_CREATE_BY", F2FDbType.VarChar, strCCRF_CREATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCRF_UPDATE_BY", F2FDbType.VarChar, strCCRF_UPDATE_BY));
                    intCircularId = Convert.ToInt32(DB.F2FCommand.ExecuteNonQuery());
                    DB.F2FTransaction.Commit();
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intCircularId;
        }

        public DataTable Search_Compliance_Review_Files(int intCCRF_ID, int intCCRF_CCR_ID)
        {
            DataTable dtResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_Search_Comp_ReviewFile";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCRF_ID", F2FDbType.Int32, intCCRF_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCRF_CCR_ID", F2FDbType.VarChar, intCCRF_CCR_ID));
                    DB.F2FDataAdapter.Fill(dtResults);
                    return dtResults;
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dtResults;
        }

        public int Delete_Compliance_Review_Files(int intCCRF_ID, int intCCRF_CCR_ID)
        {
            int retval = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_Delete_Comp_ReviewFiles";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCRF_ID", F2FDbType.Int32, intCCRF_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCRF_CCR_ID", F2FDbType.VarChar, intCCRF_CCR_ID));
                    retval = Convert.ToInt32(DB.F2FCommand.ExecuteNonQuery());
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retval;

        }

        #endregion

        #region Universe Master

        public int Save_Universe_Master(int intCRUM_ID, string strCRUM_UNIVERSE_TO_BE_REVIEWED, string strCRUM_STATUS, string strCRUM_BUSINESS_UNITS_INVOLVED, string strCREATE_BY, string strUPDATE_BY)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CR_SaveUniverse_Mas";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRUM_ID", F2FDbType.Int32, intCRUM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRUM_UNIVERSE_TO_BE_REVIEWED", F2FDbType.VarChar, strCRUM_UNIVERSE_TO_BE_REVIEWED));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRUM_STATUS", F2FDbType.VarChar, strCRUM_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRUM_BUSINESS_UNITS_INVOLVED", F2FDbType.VarChar, strCRUM_BUSINESS_UNITS_INVOLVED));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CREATE_BY", F2FDbType.VarChar, strCREATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UPDATE_BY", F2FDbType.VarChar, strUPDATE_BY));
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteNonQuery());
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
        public DataSet Search_Universe_Master(string strUniversename, string strStatus, int UniverserId = 0, string strvalue = null)
        {
            DataSet dsResults = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_Search_Universe_Mas";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Universe_to_be_reviewed", F2FDbType.VarChar, strUniversename));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Universe_ID", F2FDbType.Int32, UniverserId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Value1", F2FDbType.VarChar, strvalue));
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

        #endregion

        #region Reviewer Master

        public int Save_Reviewer_Master(int intCRM_ID, string strCRM_L0_REVIEWER_NT_ID, string strCRM_L0_REVIEWER_NAME, string strCRM_L0_REVIEWER_EMAIL,
            string strCRM_L1_REVIEWER_NT_ID, string strCRM_L1_REVIEWER_NAME, string strCRM_L1_REVIEWER_EMAIL, string strCRM_L2_REVIEWER_NT_ID, string strCRM_L2_REVIEWER_NAME
            , string strCRM_L2_REVIEWER_EMAIL, string strCRM_STATUS, string strCREATE_BY, string strUPDATE_BY)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CR_SaveReviewerMas";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_ID", F2FDbType.Int32, intCRM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_L0_REVIEWER_NT_ID", F2FDbType.VarChar, strCRM_L0_REVIEWER_NT_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_L0_REVIEWER_NAME", F2FDbType.VarChar, strCRM_L0_REVIEWER_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_L0_REVIEWER_EMAIL", F2FDbType.VarChar, strCRM_L0_REVIEWER_EMAIL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_L1_REVIEWER_NT_ID", F2FDbType.VarChar, strCRM_L1_REVIEWER_NT_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_L1_REVIEWER_NAME", F2FDbType.VarChar, strCRM_L1_REVIEWER_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_L1_REVIEWER_EMAIL", F2FDbType.VarChar, strCRM_L1_REVIEWER_EMAIL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_L2_REVIEWER_NT_ID", F2FDbType.VarChar, strCRM_L2_REVIEWER_NT_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_L2_REVIEWER_NAME", F2FDbType.VarChar, strCRM_L2_REVIEWER_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_L2_REVIEWER_EMAIL", F2FDbType.VarChar, strCRM_L2_REVIEWER_EMAIL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_STATUS", F2FDbType.VarChar, strCRM_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CREATE_BY", F2FDbType.VarChar, strCREATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UPDATE_BY", F2FDbType.VarChar, strUPDATE_BY));
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteNonQuery());
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


        public DataTable Search_Reviewer_Master(int intCRM_ID, string strL0_ReviewerName, string strL1_ReviewerName, string strL2_ReviewerName, string strStatus, string strL0_Reviewer_NT_ID = null, string strFilter1= null)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_Search_Reviewer_Mas";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_ID", F2FDbType.Int32, intCRM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_L0_REVIEWER_NAME", F2FDbType.VarChar, strL0_ReviewerName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_L1_REVIEWER_NAME", F2FDbType.VarChar, strL1_ReviewerName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_L2_REVIEWER_NAME", F2FDbType.VarChar, strL2_ReviewerName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_STATUS", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRM_L0_REVIEWER_NT_ID", F2FDbType.VarChar, strL0_Reviewer_NT_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Filter1", F2FDbType.VarChar, strFilter1));
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

        #endregion


        #region DRQM

        public int saveDRQM(int intID, string strSource, int intSourceId, int intUnitId, string strDataRequirementQuery,
           string strPersonResponsible, string strPersonResponsibleId, string strPersonResponsibleName, string strPersonResponsibleEmail, string strCreateBy, string strCreator, string strType, DataTable dtFiles)
        {
            int retVal = 0;

            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "CR_saveDRQMDetails";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DRId", F2FDbType.Int32, intID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Source", F2FDbType.VarChar, strSource));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SourceId", F2FDbType.Int32, intSourceId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UnitId", F2FDbType.Int32, intUnitId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DataRequirementQuery", F2FDbType.VarChar, strDataRequirementQuery));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@PersonResponsible", F2FDbType.VarChar, strPersonResponsible));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@PersonResponsibleId", F2FDbType.VarChar, strPersonResponsibleId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@PersonResponsibleName", F2FDbType.VarChar, strPersonResponsibleName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@PersonResponsibleEmail", F2FDbType.VarChar, strPersonResponsibleEmail));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Requestor", F2FDbType.VarChar, strCreator));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@QueryType", F2FDbType.VarChar, strType));
                    DB.OpenConnection();

                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());

                    if (intID == 0)
                    {
                        saveDRQMFiles(retVal, strSource, strCreateBy, dtFiles, DB);
                    }
                    else
                    {
                        saveDRQMFiles(intID, strSource, strCreateBy, dtFiles, DB);
                    }
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }

            return retVal;
        }

        private void saveDRQMFiles(int intRefId, string strSource, string strCreateBy, DataTable dtFiles, F2FDatabase DB)
        {
            DataRow dr;
            if (dtFiles != null)
            {
                for (int i = 0; i < dtFiles.Rows.Count; i++)
                {
                    dr = dtFiles.Rows[i];
                    DB.F2FCommand.Parameters.Clear();
                    DB.F2FCommand.CommandText = "CR_Save_DRQ_Files";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Source", F2FDbType.VarChar, strSource));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRDF_Id", F2FDbType.Int32, dr["AttachId"] is DBNull ? 0 : Convert.ToInt32(dr["AttachId"])));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRDF_CDQ_ID", F2FDbType.Int32, intRefId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, dr["Type"] is DBNull ? "" : dr["Type"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FileType", F2FDbType.VarChar, dr["FileTypeId"] is DBNull ? "" : dr["FileTypeId"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FileDesc", F2FDbType.VarChar, dr["FileTypeDesc"] is DBNull ? "" : dr["FileTypeDesc"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ServerFileName", F2FDbType.VarChar, dr["ServerFileName"] is DBNull ? "" : dr["ServerFileName"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ClientFileName", F2FDbType.VarChar, dr["ClientFileName"] is DBNull ? "" : dr["ClientFileName"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.ExecuteNonQuery();
                }
            }
        }

        public DataTable getDRQMDetails(int intCDQ_ID, int intCDQ_SOURCE_ID, string strCDQ_SOURCE, int intCDQ_SFM_ID, string strCDQ_PERSON_RESPONSIBLE, string strValue = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "CR_Search_DRQM_Details";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CDQ_ID", F2FDbType.Int32, intCDQ_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CDQ_SOURCE", F2FDbType.VarChar, strCDQ_SOURCE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CDQ_SOURCE_ID", F2FDbType.Int32, intCDQ_SOURCE_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CDQ_SFM_ID", F2FDbType.Int32, intCDQ_SFM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CDQ_PERSON_RESPONSIBLE", F2FDbType.VarChar, strCDQ_PERSON_RESPONSIBLE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Value", F2FDbType.VarChar, strValue));
                    DataSet ds = new DataSet();
                    DB.F2FDataAdapter.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }

            return dt;
        }

        public DataTable getDRQMFiles(int intCRDF_ID, string strCRDF_SOURCE, int intCRDF_CDQ_ID, string intCRDF_TYPE)
        {
            DataTable dt = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "CR_Search_DRQ_Files";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRDF_ID", F2FDbType.Int32, intCRDF_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRDF_SOURCE", F2FDbType.VarChar, strCRDF_SOURCE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRDF_CDQ_ID", F2FDbType.Int32, intCRDF_CDQ_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CRDF_TYPE", F2FDbType.VarChar, intCRDF_TYPE));
                    DB.F2FDataAdapter.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }

            return dt;
        }


        public void saveDRQMClousreDetails(int intId, string strSource, int intSourceId, string strClosureReamrks, string strCreateBy)
        {
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "CR_saveDRQClosureDetails";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DRId", F2FDbType.Int32, intId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ClosureRemarks", F2FDbType.VarChar, strClosureReamrks));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Source", F2FDbType.VarChar, strSource));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SourceId", F2FDbType.Int32, intSourceId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.OpenConnection();

                    DB.F2FCommand.ExecuteScalar();
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
        }

        #endregion


        #region DRQM Response


        public int saveDRQMResponse(int intId, string strSource, int intDRId, string strUpdateType, string strResponse, string strCreateBy,
          DataTable dtFiles, string strUser)
        {
            int retVal = 0;

            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "CR_saveDRQMResponse";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DRUId", F2FDbType.Int32, intId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DRId", F2FDbType.Int32, intDRId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Source", F2FDbType.VarChar, strSource));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UpdateType", F2FDbType.VarChar, strUpdateType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Response", F2FDbType.VarChar, strResponse));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@User", F2FDbType.VarChar, strUser));
                    DB.OpenConnection();

                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());

                    if (intId == 0)
                    {
                        saveDRQMFiles(retVal, strSource, strCreateBy, dtFiles, DB);
                    }
                    else
                    {
                        saveDRQMFiles(intId, strSource, strCreateBy, dtFiles, DB);
                    }
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }

            return retVal;
        }

        public DataTable getDRQMResponse(int intRefId, int intId, string strSource, string strUser, string strFilter1, string strLoggedInUser,
            string strSourceIdentifier, string strIds)
        {
            DataTable dt = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "CR_getDRQMResponse";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@RefId", F2FDbType.Int32, intRefId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DRUId", F2FDbType.Int32, intId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Source", F2FDbType.VarChar, strSource));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@User", F2FDbType.VarChar, strUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Filter1", F2FDbType.VarChar, strFilter1));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LoggedInUser", F2FDbType.VarChar, strLoggedInUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SourceIdentifier", F2FDbType.VarChar, strSourceIdentifier));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DRIds", F2FDbType.VarChar, strIds));
                    DataSet ds = new DataSet();
                    DB.F2FDataAdapter.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }

            return dt;
        }

        #endregion

        #region Sub Function Master


        public int Save_SUBFunction_Master(int intCSFM_ID, string strCSFM_NAME, string strCSFM_CODE, string strCSFM_HEAD,
           string strCSFM_UNIT_HEAD_CODE, string strCSFM_UNIT_HEAD_EMAIL, string strCSFM_UNIT_SPOC, string strCSFM_UNIT_SPOC_CODE, string strCSFM_UNIT_SPOC_EMAIL
           , string strCSFM_STATUS, string strCSFM_CREATE_BY, string strCSFM_UPDATE_BY)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CR_SaveSubFunctionMaster";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_ID", F2FDbType.Int32, intCSFM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_NAME", F2FDbType.VarChar, strCSFM_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_CODE", F2FDbType.VarChar, strCSFM_CODE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_HEAD", F2FDbType.VarChar, strCSFM_HEAD));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_UNIT_HEAD_CODE", F2FDbType.VarChar, strCSFM_UNIT_HEAD_CODE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_UNIT_HEAD_EMAIL", F2FDbType.VarChar, strCSFM_UNIT_HEAD_EMAIL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_UNIT_SPOC", F2FDbType.VarChar, strCSFM_UNIT_SPOC));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_UNIT_SPOC_CODE", F2FDbType.VarChar, strCSFM_UNIT_SPOC_CODE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_UNIT_SPOC_EMAIL", F2FDbType.VarChar, strCSFM_UNIT_SPOC_EMAIL));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_STATUS", F2FDbType.VarChar, strCSFM_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_CREATE_BY", F2FDbType.VarChar, strCSFM_CREATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_UPDATE_BY", F2FDbType.VarChar, strCSFM_UPDATE_BY));
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteNonQuery());
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


        public DataTable Search_SubFunction_Master(int intCSFM_ID, string strCSFM_NAME, string strCSFM_CODE = null, string strCSFM_Status = null, string strFilter1 = null)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_Search_SubFunction_Mas";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_ID", F2FDbType.Int32, intCSFM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_NAME", F2FDbType.VarChar, strCSFM_NAME));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_CODE", F2FDbType.VarChar, strCSFM_CODE));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSFM_Status", F2FDbType.VarChar, strCSFM_Status));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Filter1", F2FDbType.VarChar, strFilter1));
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


        #endregion


        #region Issue

        public int saveIssueDetails(int intIssueId, int intComplainceReviewId, int intUnitId,
        string strIssueTitle, string strIssueDescription, string strIssueType, string strIssueStatus,
       string strSPOCResponsible, string strSPOCResponsibleId, string strSPOCResponsibleName, string strSPOCResponsibleEmail,
        string strCreateBy, string strRemarks, string strStatus, DataTable dtFiles)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_saveIssueDetails";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IssueId", F2FDatabase.F2FDbType.Int32, intIssueId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ComplainceReviewId", F2FDatabase.F2FDbType.Int32, intComplainceReviewId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@UnitId", F2FDatabase.F2FDbType.Int32, intUnitId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IssueTitle", F2FDatabase.F2FDbType.VarChar, strIssueTitle));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IssueDescription", F2FDatabase.F2FDbType.VarChar, strIssueDescription));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IssueType", F2FDatabase.F2FDbType.VarChar, strIssueType));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IssueStatus", F2FDatabase.F2FDbType.VarChar, strIssueStatus));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@SPOCResponsible", F2FDatabase.F2FDbType.VarChar, strSPOCResponsible));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@SPOCResponsibleId", F2FDatabase.F2FDbType.VarChar, strSPOCResponsibleId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@SPOCResponsibleName", F2FDatabase.F2FDbType.VarChar, strSPOCResponsibleName));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@SPOCResponsibleEmail", F2FDatabase.F2FDbType.VarChar, strSPOCResponsibleEmail));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CreateBy", F2FDatabase.F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Remarks", F2FDatabase.F2FDbType.VarChar, strRemarks));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Status", F2FDatabase.F2FDbType.VarChar, strStatus));
                    DB.OpenConnection();
                    intIssueId = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                    if (intIssueId > 0)
                    {
                        saveIssueFiles(intIssueId, strCreateBy, dtFiles, DB);
                    }
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return intIssueId;
        }


        private void saveIssueFiles(int intComplianceReviewId, string strCreateBy, DataTable dtFiles, F2FDatabase DB)
        {
            DataRow dr;
            if (dtFiles != null)
            {
                for (int i = 0; i < dtFiles.Rows.Count; i++)
                {
                    dr = dtFiles.Rows[i];
                    DB.F2FCommand.Parameters.Clear();
                    DB.F2FCommand.CommandText = "CR_saveIssueFiles";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIF_ID", F2FDatabase.F2FDbType.Int32, dr["AttachId"] is DBNull ? 0 : Convert.ToInt32(dr["AttachId"])));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ComplianceReviewId", F2FDatabase.F2FDbType.Int32, intComplianceReviewId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Type", F2FDatabase.F2FDbType.VarChar, dr["Type"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@FileType", F2FDatabase.F2FDbType.VarChar, dr["FileTypeId"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@FileDesc", F2FDatabase.F2FDbType.VarChar, dr["FileTypeDesc"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ServerFileName", F2FDatabase.F2FDbType.VarChar, dr["ServerFileName"] is DBNull ? "" : dr["ServerFileName"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ClientFileName", F2FDatabase.F2FDbType.VarChar, dr["ClientFileName"] is DBNull ? "" : dr["ClientFileName"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CreateBy", F2FDatabase.F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.ExecuteNonQuery();
                }
            }
        }


        public DataTable getIssue(int intIssueID, int intComplianceReviewId, string strLoggedInUser,
            string strType, string strStatus, string strValue1 = null)
        {
            DataTable dtResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_getIssuedetails";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IssueId", F2FDatabase.F2FDbType.Int32, intIssueID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ComplianceReviewId", F2FDatabase.F2FDbType.Int32, intComplianceReviewId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@LoggedInUser", F2FDatabase.F2FDbType.VarChar, strLoggedInUser));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Type", F2FDatabase.F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Status", F2FDatabase.F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Value", F2FDatabase.F2FDbType.VarChar, strValue1));
                    DB.F2FDataAdapter.Fill(dtResults);
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
                return dtResults;
            }

        }



        public DataTable getIssueFiles(int intIssueId, int intFileId, string strType, string strRC_Type)
        {
            DataTable dt = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "CR_getIssueFiles";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IssueId", F2FDatabase.F2FDbType.Int32, intIssueId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@FileId", F2FDatabase.F2FDbType.Int32, intFileId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Type", F2FDatabase.F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@RC_Type", F2FDatabase.F2FDbType.VarChar, strRC_Type));
                    DB.F2FDataAdapter.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }

            return dt;
        }

        public int submitForOperation(int intCI_ID, string strValue, string strOperation,
           string strCreateBy, int intRiskReviewDraftId = 0, string strLoggedInUser = null, string strValue1 = null)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_submitForOperation";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CI_ID", F2FDatabase.F2FDbType.Int32, intCI_ID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@RDId", F2FDatabase.F2FDbType.Int32, intRiskReviewDraftId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Value", F2FDatabase.F2FDbType.VarChar, strValue));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Value1", F2FDatabase.F2FDbType.VarChar, strValue1));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Operation", F2FDatabase.F2FDbType.VarChar, strOperation));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CreateBy", F2FDatabase.F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@LoggedInUser", F2FDatabase.F2FDbType.VarChar, strLoggedInUser));
                    DB.OpenConnection();
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteNonQuery());

                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retVal;
        }


        public int ChangeIssueStatus(int intIssueId, string strStatus, string strManagementResponse, string strRemarks)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_Issue_ChangeStatus";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IssueId", F2FDatabase.F2FDbType.Int32, intIssueId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Status", F2FDatabase.F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ManagementResponse", F2FDatabase.F2FDbType.VarChar, strManagementResponse));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Remarks", F2FDatabase.F2FDbType.VarChar, strRemarks));
                    DB.OpenConnection();
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteNonQuery());
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retVal;
        }

        public int ApproveRejectIssue(int intCRId, int intIssueId, string strComments, string strCreateBy, string strType, string strUserType)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_approveRejectIssue";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CRId", F2FDatabase.F2FDbType.Int32, intCRId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IssueId", F2FDatabase.F2FDbType.Int32, intIssueId));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Comments", F2FDatabase.F2FDbType.VarChar, strComments));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CreateBy", F2FDatabase.F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Type", F2FDatabase.F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@UserType", F2FDatabase.F2FDbType.VarChar, strUserType));
                    DB.OpenConnection();
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteNonQuery());
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retVal;
        }

        #endregion

        #region Issue Action

        public int saveIssueUpdate(int intCIA_ID, int intCIA_CI_ID, string strCIA_REF_NO,
        int intCIA_SFM_ID, string strCIA_ACTIONABLE, string strCIA_DETAILED_DESC, string strCIA_ACT_TYPE,
       string strCIA_CRITICALITY, string strCIA_STATUS, string strCIA_CREATOR_NAME, string strCIA_CREATOR_NT_ID,
        DateTime? strCIA_TARGET_DT, string strCIA_CLOSURE_BY, DateTime? strCIA_CLOSURE_DT, string strCIA_CLOSURE_REMARKS, string strCIA_REMARKS,
        string strCIA_CREATE_BY, string strCIA_UPDATE_BY, string strCIA_ACTIONABLE_STATUS, DateTime? strCIA_REVISED_TARGET_DT, string strCIA_REVISED_MAP,
        string intCIA_SPECIFIED_PERSON_ID, string strCIA_SPECIFIED_PERSON_NAME, string strCIA_SPECIFIED_PERSON_EMAIL)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_saveIssueActions";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_ID", F2FDatabase.F2FDbType.Int32, intCIA_ID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_CI_ID", F2FDatabase.F2FDbType.Int32, intCIA_CI_ID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_REF_NO", F2FDatabase.F2FDbType.VarChar, strCIA_REF_NO));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_SFM_ID", F2FDatabase.F2FDbType.Int32, intCIA_SFM_ID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_ACTIONABLE", F2FDatabase.F2FDbType.VarChar, strCIA_ACTIONABLE));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_DETAILED_DESC", F2FDatabase.F2FDbType.VarChar, strCIA_DETAILED_DESC));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_ACT_TYPE", F2FDatabase.F2FDbType.VarChar, strCIA_ACT_TYPE));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_CRITICALITY", F2FDatabase.F2FDbType.VarChar, strCIA_CRITICALITY));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_STATUS", F2FDatabase.F2FDbType.VarChar, strCIA_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_CREATOR_NAME", F2FDatabase.F2FDbType.VarChar, strCIA_CREATOR_NAME));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_CREATOR_NT_ID", F2FDatabase.F2FDbType.VarChar, strCIA_CREATOR_NT_ID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_TARGET_DT", F2FDatabase.F2FDbType.DateTime, strCIA_TARGET_DT));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_CLOSURE_BY", F2FDatabase.F2FDbType.VarChar, strCIA_CLOSURE_BY));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_CLOSURE_DT", F2FDatabase.F2FDbType.DateTime, strCIA_CLOSURE_DT));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_CLOSURE_REMARKS", F2FDatabase.F2FDbType.VarChar, strCIA_CLOSURE_REMARKS));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_REMARKS", F2FDatabase.F2FDbType.VarChar, strCIA_REMARKS));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_CREATE_BY", F2FDatabase.F2FDbType.VarChar, strCIA_CREATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_UPDATE_BY", F2FDatabase.F2FDbType.VarChar, strCIA_UPDATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_ACTIONABLE_STATUS", F2FDatabase.F2FDbType.VarChar, strCIA_ACTIONABLE_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_REVISED_TARGET_DT", F2FDatabase.F2FDbType.DateTime, strCIA_REVISED_TARGET_DT));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_REVISED_MAP", F2FDatabase.F2FDbType.VarChar, strCIA_REVISED_MAP));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_SPECIFIED_PERSON_ID", F2FDatabase.F2FDbType.VarChar, intCIA_SPECIFIED_PERSON_ID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_SPECIFIED_PERSON_NAME", F2FDatabase.F2FDbType.VarChar, strCIA_SPECIFIED_PERSON_NAME));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_SPECIFIED_PERSON_EMAIL", F2FDatabase.F2FDbType.VarChar, strCIA_SPECIFIED_PERSON_EMAIL));
                    DB.OpenConnection();
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteNonQuery());
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retVal;
        }



        public DataTable getIssueActions(int intCIA_ID, int intCIA_CI_ID, string strCIA_REF_NO, string strCIA_CREATE_BY, string strCIA_ACTIONABLE_STATUS = null, string strCIA_RESPONSIBLE_PERSON = null,
            string strCIA_TARGET_DT_FROM = null, string strCIA_TARGET_DT_TO = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "CR_getIssueActions";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_ID", F2FDatabase.F2FDbType.Int32, intCIA_ID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_CI_ID", F2FDatabase.F2FDbType.Int32, intCIA_CI_ID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_REF_NO", F2FDatabase.F2FDbType.VarChar, strCIA_REF_NO));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_CREATE_BY", F2FDatabase.F2FDbType.VarChar, strCIA_CREATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_ACTIONABLE_STATUS", F2FDatabase.F2FDbType.VarChar, strCIA_ACTIONABLE_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_RESPONSIBLE_PERSON", F2FDatabase.F2FDbType.VarChar, strCIA_RESPONSIBLE_PERSON));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_TARGET_DT_FROM", F2FDatabase.F2FDbType.VarChar, strCIA_TARGET_DT_FROM));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_TARGET_DT_TO", F2FDatabase.F2FDbType.VarChar, strCIA_TARGET_DT_TO));
                    DB.F2FDataAdapter.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }

            return dt;
        }



        public DataTable getIssueActions_details(string strCIA_CREATE_BY, string strComplianceReviewNo, string strIssue,string strActionable,
            string strCIA_ACTIONABLE_STATUS, string strCIA_RESPONSIBLE_PERSON,
            string strCIA_TARGET_DT_FROM, string strCIA_TARGET_DT_TO, string strFilter1=null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "CR_getIssueActions_Details";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_CREATE_BY", F2FDatabase.F2FDbType.VarChar, strCIA_CREATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_ACTIONABLE_STATUS", F2FDatabase.F2FDbType.VarChar, strCIA_ACTIONABLE_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_RESPONSIBLE_PERSON", F2FDatabase.F2FDbType.VarChar, strCIA_RESPONSIBLE_PERSON));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_TARGET_DT_FROM", F2FDatabase.F2FDbType.VarChar, strCIA_TARGET_DT_FROM));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_TARGET_DT_TO", F2FDatabase.F2FDbType.VarChar, strCIA_TARGET_DT_TO));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ComplianceReviewNo", F2FDatabase.F2FDbType.VarChar, strComplianceReviewNo));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Issue", F2FDatabase.F2FDbType.VarChar, strIssue));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Actionable", F2FDatabase.F2FDbType.VarChar, strActionable));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter1", F2FDatabase.F2FDbType.VarChar, strFilter1));

                    DB.F2FDataAdapter.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }

            return dt;
        }

        public int DeleteIssueActions(int intCIA_ID)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_DeleteIssueAction";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIA_ID", F2FDatabase.F2FDbType.Int32, intCIA_ID));
                    DB.OpenConnection();
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteNonQuery());
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retVal;
        }

        #endregion


        #region Issue Action Update

        public int saveIssueActionUpdate(int intCIAU_ID, int intCIAU_CIA_ID, string strCIAU_UPDATE_TYPE,
        string strCIAU_REVISED_MAP, string strCIAU_ACTIONABLE_STATUS, DateTime? dtCIAU_REVISED_TARGET_DT, string strCIAU_STATUS,
        DateTime? dtCIAU_CLOSURE_DT, string strCIAU_ACTUAL_ACTION_TAKEN,
        string strCIAU_CREATE_BY, string strCIAU_UPDATE_BY, string strCIAU_REMARKS,
        string strClientFileName, string strServerFileName)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CR_SaveIssueActionUpdates";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIAU_ID", F2FDatabase.F2FDbType.Int32, intCIAU_ID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIAU_CIA_ID", F2FDatabase.F2FDbType.Int32, intCIAU_CIA_ID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIAU_UPDATE_TYPE", F2FDatabase.F2FDbType.VarChar, strCIAU_UPDATE_TYPE));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIAU_REVISED_MAP", F2FDatabase.F2FDbType.VarChar, strCIAU_REVISED_MAP));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIAU_ACTIONABLE_STATUS", F2FDatabase.F2FDbType.VarChar, strCIAU_ACTIONABLE_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIAU_REVISED_TARGET_DT", F2FDatabase.F2FDbType.DateTime, dtCIAU_REVISED_TARGET_DT));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIAU_STATUS", F2FDatabase.F2FDbType.VarChar, strCIAU_STATUS));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIAU_CLOSURE_DT", F2FDatabase.F2FDbType.DateTime, dtCIAU_CLOSURE_DT));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIAU_ACTUAL_ACTION_TAKEN", F2FDatabase.F2FDbType.VarChar, strCIAU_ACTUAL_ACTION_TAKEN));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIAU_CREATE_BY", F2FDatabase.F2FDbType.VarChar, strCIAU_CREATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIAU_UPDATE_BY", F2FDatabase.F2FDbType.VarChar, strCIAU_UPDATE_BY));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIAU_REMARKS", F2FDatabase.F2FDbType.VarChar, strCIAU_REMARKS));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ClientFileName", F2FDbType.VarChar, strClientFileName));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ServerFileName", F2FDbType.VarChar, strServerFileName));
                    DB.OpenConnection();
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteNonQuery());
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retVal;
        }

        public DataTable getIssueActionsUpdates(int intCIAU_ID, int intCIAU_CIA_ID, string strFilter1 = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "CR_GetIssueActions_Update";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIAU_ID", F2FDatabase.F2FDbType.Int32, intCIAU_ID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CIAU_CIA_ID", F2FDatabase.F2FDbType.Int32, intCIAU_CIA_ID));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filter1", F2FDatabase.F2FDbType.VarChar, strFilter1));
                    DB.F2FDataAdapter.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }

            return dt;
        }
        #endregion

        #region Utility Functions

        public DataTable GetDataTable(string strType, DBUtilityParameter oParam1 = null, DBUtilityParameter oParam2 = null, DBUtilityParameter oParam3 = null, string sOrderBy = null)
        {
            DataTable dtResult = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase("CR_GetUtilitiesData"))
                {
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Type", F2FDatabase.F2FDbType.VarChar, strType));
                    if (oParam1 != null)
                    {
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CondType1", F2FDatabase.F2FDbType.VarChar, oParam1.CondType));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ColName1", F2FDatabase.F2FDbType.VarChar, oParam1.ColumnName));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Operator1", F2FDatabase.F2FDbType.VarChar, oParam1.Operator));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@FirstVal1", F2FDatabase.F2FDbType.VarChar, oParam1.FirstValue));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@SecondVal1", F2FDatabase.F2FDbType.VarChar, oParam1.SecondValue));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IsSubQuery1", F2FDatabase.F2FDbType.Int32, oParam1.SubQuery));
                    }
                    if (oParam2 != null)
                    {
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CondType2", F2FDatabase.F2FDbType.VarChar, oParam2.CondType));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ColName2", F2FDatabase.F2FDbType.VarChar, oParam2.ColumnName));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Operator2", F2FDatabase.F2FDbType.VarChar, oParam2.Operator));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@FirstVal2", F2FDatabase.F2FDbType.VarChar, oParam2.FirstValue));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@SecondVal2", F2FDatabase.F2FDbType.VarChar, oParam2.SecondValue));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IsSubQuery2", F2FDatabase.F2FDbType.Int32, oParam2.SubQuery));
                    }
                    if (oParam3 != null)
                    {
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@CondType3", F2FDatabase.F2FDbType.VarChar, oParam3.CondType));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@ColName3", F2FDatabase.F2FDbType.VarChar, oParam3.ColumnName));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Operator3", F2FDatabase.F2FDbType.VarChar, oParam3.Operator));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@FirstVal3", F2FDatabase.F2FDbType.VarChar, oParam3.FirstValue));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@SecondVal3", F2FDatabase.F2FDbType.VarChar, oParam3.SecondValue));
                        DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@IsSubQuery3", F2FDatabase.F2FDbType.VarChar, oParam3.SubQuery));
                    }
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@OrderBy", F2FDatabase.F2FDbType.VarChar, sOrderBy));
                    DB.F2FDataAdapter.Fill(dtResult);
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return dtResult;
        }

        #endregion
    }
}