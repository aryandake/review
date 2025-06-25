using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Fiction2Fact.App_Code;
using static Fiction2Fact.App_Code.F2FDatabase;

namespace Fiction2Fact.Legacy_App_Code.Circulars.DAL
{
    public class CircularMasterDAL
    {
        CommonMethods cm = new CommonMethods();

        public int SaveCircular(int CircularId, int CircularAuthId, int CircularAreaId, int deptId,
            string circularNo, string downloadRefId, string topic, string implication, string issuingLink,
            string name, string details, string circularDate, string createBy, int typeofCircular, DataTable dtSegment,
            DataTable dtIntimations, DataTable dtAdditionalMails, DataTable dtFiles, DataTable dtAcion, string strSpocFromComplianceFn,
            string SubtypeOfDocument, string strAssociatedKeywords, string strLinkageWithEarlierCircular, string strSOCEOC,
            string strOldCirSubNo, string strOldCircularId, string strCircEffDate, string strAuditCommitteeToApprove,
            string strToBePlacedBefore, string strDetails, string strNameOfThePolicy, string strFrequency, string strBroadcastDate, int intLOB)
        {
            int intCircularId = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CIRC_insertCircular";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;

                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularId", F2FDbType.Int32, CircularId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularAuthId", F2FDbType.Int32, CircularAuthId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularAreaId", F2FDbType.Int32, CircularAreaId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularDeptId", F2FDbType.Int32, deptId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircNo", F2FDbType.VarChar, circularNo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DownloadRefNo", F2FDbType.VarChar, downloadRefId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircTopic", F2FDbType.VarChar, topic));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircImplications", F2FDbType.VarChar, implication));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircIssLink", F2FDbType.VarChar, issuingLink));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircDetails", F2FDbType.VarChar, details));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircName", F2FDbType.VarChar, name));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircDate", F2FDbType.DateTime, circularDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircEffDate", F2FDbType.DateTime, strCircEffDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircCreatedBy", F2FDbType.VarChar, createBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@TypeofCircular", F2FDbType.Int32, typeofCircular));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SubTypeOfDocument", F2FDbType.VarChar, SubtypeOfDocument));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SpocFromComplianceFn", F2FDbType.VarChar, strSpocFromComplianceFn));
                    //<< Added by Amarjeet on 26-Jul-2021
                    DB.F2FCommand.Parameters.Add(F2FParameter("@AssociatedKeywords", F2FDbType.VarChar, strAssociatedKeywords));
                    //>>
                    //<< Added by Amarjeet on 04-Aug-2021
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LinkageWithEarlierCircular", F2FDbType.VarChar, strLinkageWithEarlierCircular));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SOCEOC", F2FDbType.VarChar, strSOCEOC));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OldCirSubNo", F2FDbType.VarChar, strOldCirSubNo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OldCircularId", F2FDbType.VarChar, (string.IsNullOrEmpty(strOldCircularId) ? "" : strOldCircularId)));
                    //>>
                    DB.F2FCommand.Parameters.Add(F2FParameter("@AuditCommitteeToApprove", F2FDbType.VarChar, strAuditCommitteeToApprove));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ToBePlacedBefore", F2FDbType.VarChar, strToBePlacedBefore));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Details", F2FDbType.VarChar, strDetails));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@NameOfThePolicy", F2FDbType.VarChar, strNameOfThePolicy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Frequency", F2FDbType.VarChar, strFrequency));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@BroadcastDate", F2FDbType.DateTime, (string.IsNullOrEmpty(strBroadcastDate) ? null : strBroadcastDate)));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LOB", F2FDbType.Int32, intLOB)); 

                    intCircularId = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                    if (CircularId == 0)
                    {
                        insertCircularSegMapping(intCircularId, dtSegment, createBy);
                        insertCircularIntimations(intCircularId, dtIntimations, createBy);
                        insertCircularAdditionalMails(intCircularId, dtAdditionalMails, createBy);
                        insertCircularFiles(intCircularId, dtFiles, createBy);
                        insertCircularAcionables(intCircularId, dtAcion, createBy, (new Authentication()).getUserFullName(createBy));
                    }
                    else
                    {
                        insertCircularSegMapping(CircularId, dtSegment, createBy);
                        insertCircularIntimations(CircularId, dtIntimations, createBy);
                        insertCircularFiles(CircularId, dtFiles, createBy);
                        insertCircularAcionables(CircularId, dtAcion, createBy, (new Authentication()).getUserFullName(createBy));
                        insertCircularAdditionalMails(intCircularId, dtAdditionalMails, createBy);
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

        public void insertCircularAcionables(int intcircularId, DataTable dt, string strCreator, string CreateBy)
        {
            if (dt != null)
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.OpenConnection();
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        DB.F2FCommand.CommandText = "CIRC_insertUpdateActionables";
                        DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                        DB.F2FCommand.Parameters.Clear();
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CircularId", F2FDbType.Int32, intcircularId));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@ID", F2FDbType.Int32, dr["Id"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@Actionable", F2FDbType.VarChar, dr["Actionable"].ToString()));

                        DB.F2FCommand.Parameters.Add(F2FParameter("@PerResp", F2FDbType.VarChar, dr["PerResp"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@PerRespUserId", F2FDbType.VarChar, dr["PerRespUserId"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@PerRespUserName", F2FDbType.VarChar, dr["PerRespUserName"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@PerRespEmailId", F2FDbType.VarChar, dr["PerRespEmailId"].ToString()));

                        DB.F2FCommand.Parameters.Add(F2FParameter("@ReportMgr", F2FDbType.VarChar, dr["ReportMgr"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@ReportMgrId", F2FDbType.VarChar, dr["ReportMgrId"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@ReportMgrUserName", F2FDbType.VarChar, dr["ReportMgrUserName"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@ReportMgrEmailId", F2FDbType.VarChar, dr["ReportMgrEmailId"].ToString()));

                        DB.F2FCommand.Parameters.Add(F2FParameter("@TargetDate", F2FDbType.VarChar, dr["TargetDate"].Equals("") ? DBNull.Value : dr["TargetDate"]));
                        //if (dr["ComplDate"].Equals(""))
                        //    DB.F2FCommand.Parameters.Add(F2FParameter("@ComplDate", F2FDbType.VarChar, DBNull.Value));
                        //else
                        //    DB.F2FCommand.Parameters.Add(F2FParameter("@ComplDate", F2FDbType.VarChar, dr["ComplDate"].ToString()));

                        DB.F2FCommand.Parameters.Add(F2FParameter("@RegulatoryDueDate", F2FDbType.VarChar, (string.IsNullOrEmpty(dr["RegulatoryDueDate"].ToString()) ? null : dr["RegulatoryDueDate"].ToString())));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@ComplDate", F2FDbType.VarChar, (string.IsNullOrEmpty(dr["ComplDate"].ToString()) ? null : dr["ComplDate"].ToString())));

                        DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, dr["Status"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CommType", F2FDbType.VarChar, dr["CommType"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@Function", F2FDbType.VarChar, dr["Function"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@Remarks", F2FDbType.VarChar, dr["Remarks"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@Creator", F2FDbType.VarChar, strCreator));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, CreateBy));
                        DB.F2FCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private void insertCircularSegMapping(int intcircularId, DataTable dt, string CreateBy)
        {
            if (dt != null)
            {
                int retVal;
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.OpenConnection();
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        DB.F2FCommand.CommandText = "CIRC_insertCircularSegment";
                        DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                        DB.F2FCommand.Parameters.Clear();
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CircularId", F2FDbType.Int32, intcircularId));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@SegmentId", F2FDbType.Int32, dr["SegmentId"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, CreateBy));
                        retVal = DB.F2FCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private void insertCircularIntimations(int intcircularId, DataTable dt, string CreateBy)
        {
            if (dt != null)
            {
                int retVal;
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.OpenConnection();
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        DB.F2FCommand.CommandText = "CIRC_insertCircularIntimations";
                        DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                        DB.F2FCommand.Parameters.Clear();
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CircularId", F2FDbType.Int32, intcircularId));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@IntimationId", F2FDbType.Int32, dr["IntimationId"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, CreateBy));
                        retVal = DB.F2FCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        //private void insertCircularAdditionalMails(int intcircularId, DataTable dt, string CreateBy)
        public void insertCircularAdditionalMails(int intcircularId, DataTable dt, string CreateBy)
        {
            int retVal;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();

                if (!string.IsNullOrEmpty(intcircularId.ToString()))
                {
                    string delSql = "DELETE FROM TBL_CIRCULAR_ADDITIONAL_EMAILS WHERE CAM_CM_ID = @CirId";
                    DB.F2FCommand.CommandText = delSql;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CirId", F2FDbType.Int32, intcircularId));
                    DB.F2FCommand.ExecuteNonQuery();
                }
                if (dt != null)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        DB.F2FCommand.CommandText = "CIRC_insertCircularAdditionalMails";
                        DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                        DB.F2FCommand.Parameters.Clear();
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CircularId", F2FDbType.Int32, intcircularId));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@MailType", F2FDbType.VarChar, dr["MailType"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@AdditionalMailId", F2FDbType.VarChar, dr["AdditionalMailId"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, CreateBy));
                        retVal = DB.F2FCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private void insertCircularFiles(int intcircularId, DataTable dt, string CreateBy)
        {
            int retVal;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                if (dt != null)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        DB.F2FCommand.CommandText = "CIRC_insertCircularFiles";
                        DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                        DB.F2FCommand.Parameters.Clear();
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CFId", F2FDbType.Int32, dr["AttachId"] == null ? null : dr["AttachId"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CircularId", F2FDbType.Int32, intcircularId));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@ServerFileName", F2FDbType.VarChar, dr["ServerFileName"] == null ? null : dr["ServerFileName"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@FileName", F2FDbType.VarChar, dr["ClientFileName"] == null ? null : dr["ClientFileName"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, CreateBy));
                        //retVal = DB.F2FCommand.ExecuteNonQuery();
                        retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());

                        //if (dr["ServerFileName"].ToString().Contains(".pdf"))
                        //    cm.savePDFContent(intcircularId.ToString(), retVal.ToString(), (string.IsNullOrEmpty(dr["ServerFilePath"].ToString()) ? "" : dr["ServerFilePath"].ToString()), "Circ", CreateBy);
                    }
                }
            }
        }


        public DataSet SearchCircular(int intCircular, string strIssuingAuthority, string strSegment, string strDepartment,
            string strarea, string strCircularNo, string strDownloadRefNo, string strTopic, string FromDate, string ToDate,
            string ActionType, string typeOfDocument, string strSpocFromCompliancefn, string strHaveActionableLoggedIn,
            string strAssociatedKeywords, string strToBePlacedBefore, string strGlobalSearch, string strCircularIds,
            string strIsFileToBeSearched, string strStatus)
        {
            DataSet dsResults = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CIRC_SearchCircular";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularId", F2FDbType.Int32, intCircular));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularAuthId", F2FDbType.VarChar, strIssuingAuthority));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularSegmentId", F2FDbType.VarChar, strSegment));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularAreaId", F2FDbType.VarChar, strarea));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularDeptId", F2FDbType.VarChar, strDepartment));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircNo", F2FDbType.VarChar, strCircularNo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DownloadRefNo", F2FDbType.VarChar, strDownloadRefNo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircTopic", F2FDbType.VarChar, strTopic));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Fromdate", F2FDbType.VarChar, FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Todate", F2FDbType.VarChar, ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ActionType", F2FDbType.VarChar, ActionType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@TypeOfDocument", F2FDbType.VarChar, typeOfDocument));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SpocFromCompliancefn", F2FDbType.VarChar, strSpocFromCompliancefn));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@HaveActionableHasBeenlogged", F2FDbType.VarChar, strHaveActionableLoggedIn));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@AssociatedKeywords", F2FDbType.VarChar, strAssociatedKeywords));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ToBePlacedBefore", F2FDbType.VarChar, strToBePlacedBefore));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@GlobalSearch", F2FDbType.VarChar, strGlobalSearch));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularIds", F2FDbType.VarChar, strCircularIds));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FileSearch", F2FDbType.VarChar, strIsFileToBeSearched));
                    //<<Added by Ankur Tyagi on 06-Jul-2023
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    //>>
                    DB.F2FDataAdapter.Fill(dsResults);
                    return dsResults;
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in SaveCircular() " + ex);
                }
            }
        }

        //<<Added By Subodh Deolekar on 05-Jul-2010 for Audit Trail Module.

        public DataSet getCircularDetails(Hashtable paramTable, string mstrConnectionString)
        {

            DataSet dsResult = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                string strSQL = " select * from TBL_CIRCULAR_ACTIONABLES " +
                                " LEFT JOIN TBL_REF_CODES ON CA_STATUS = RC_CODE AND RC_TYPE = 'Actionable Status' " +
                                " LEFT OUTER JOIN TBL_CIRCULAR_FUNCTION_MAS ON CA_CFM_ID = CFM_ID " +
                                " WHERE CA_CM_ID = @CMId;" +
                                " select * from TBL_CIRCULAR_FILES WHERE CF_CM_ID = @CMId";
                DB.F2FCommand.CommandText = strSQL;
                DB.F2FCommand.Parameters.Add(F2FParameter("@CMId", F2FDbType.VarChar, paramTable["CMId"].ToString()));
                DB.F2FDataAdapter.Fill(dsResult);
            }
            return dsResult;
        }

        public int deleteCircular(int intCircularId, string strConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CIRC_DeleteCircular";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularId", F2FDbType.Int32, intCircularId));

                    retVal = DB.F2FCommand.ExecuteNonQuery();

                    DB.F2FTransaction.Commit();
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    throw new System.Exception("system exception in deleteCircular() " + ex);
                }
            }
            return retVal;
        }


        //Added by Supriya on 20-Nov-2014
        public DataTable SearchCircularActionable(int intCircular, string strCircularNo, string FromDate,
            string ToDate, string strStatus, string strActionableId, string strUsername, string mstrConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CIRC_SearchCircularActionable";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularId", F2FDbType.Int32, intCircular));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircNo", F2FDbType.VarChar, strCircularNo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Fromdate", F2FDbType.VarChar, FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Todate", F2FDbType.VarChar, ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ActionableId", F2FDbType.VarChar, strActionableId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Username", F2FDbType.VarChar, strUsername));
                    DB.F2FDataAdapter.Fill(dsResults);
                    return dsResults;
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in SearchCircularActionable() " + ex);
                }
            }
        }


        public int saveCircularActionableUpdates(int Id, int intActionableId, string strUpdateType, string strRemarks,
            string strRevisedTargetDate, string strActionableClosureDate, string strFileNameOnClient,
            string strFileNameOnServer, string strCreateBy)
        {
            int retVal;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    DB.F2FCommand.CommandText = "CIRC_saveCircularActionableUpdates";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ID", F2FDbType.Int32, Id));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ActionableId", F2FDbType.Int32, intActionableId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UpdateType", F2FDbType.VarChar, strUpdateType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@RevisedTargetDate", F2FDbType.VarChar, (string.IsNullOrEmpty(strRevisedTargetDate) ? null : strRevisedTargetDate)));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ActionableClosureDate", F2FDbType.VarChar, (string.IsNullOrEmpty(strActionableClosureDate) ? null : strActionableClosureDate)));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Remarks", F2FDbType.VarChar, strRemarks));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FileName", F2FDbType.VarChar, strFileNameOnClient));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ServerFileName", F2FDbType.VarChar, strFileNameOnServer));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Creator", F2FDbType.VarChar, strCreateBy));
                    retVal = DB.F2FCommand.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in saveCircularActionableUpdates() " + ex);
                }
            }
            return retVal;
        }



        public DataTable SearchCircularActionableUpdates(int intCircularActId, string mstrConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CIRC_SearchCircularActionableUpdates";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularActId", F2FDbType.Int32, intCircularActId));
                    DB.F2FDataAdapter.Fill(dsResults);
                    return dsResults;
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in SearchCircularActionableUpdates() " + ex);
                }
            }
        }

        //Modify By Milan Yadav on 30-Aug-2016
        public int UpdateCircularActionables(int Id, int intCircularId, string strActionable,
                string strPerRespId, string strPerRespName, string strPerRespEmail, string strTargetDate, string strComplDate, string strStatus,
                string strRemarks, string strClosureRemarks, string strCreateBy, DataTable dtActionablefile,
               string strConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                try
                {
                    int intActionableId;
                    DB.F2FCommand.CommandText = "CIRC_UpdateCircularActionables";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularId", F2FDbType.Int32, intCircularId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ID", F2FDbType.Int32, Id));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Actionable", F2FDbType.VarChar, strActionable));
                    //DB.F2FCommand.Parameters.Add(F2FParameter("@PerResp", F2FDbType.VarChar, strPerResp));
                    //Added By Mian Yadav on 27-Aug-2016
                    //>>
                    DB.F2FCommand.Parameters.Add(F2FParameter("@PerRespId", F2FDbType.VarChar, strPerRespId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@PerRespName", F2FDbType.VarChar, strPerRespName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@PerRespEmailId", F2FDbType.VarChar, strPerRespEmail));
                    //<<
                    DB.F2FCommand.Parameters.Add(F2FParameter("@TargetDate", F2FDbType.VarChar, strTargetDate));
                    if (strComplDate.Equals(""))
                        DB.F2FCommand.Parameters.Add(F2FParameter("@ComplDate", F2FDbType.VarChar, DBNull.Value));
                    else
                        DB.F2FCommand.Parameters.Add(F2FParameter("@ComplDate", F2FDbType.VarChar, strComplDate));

                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Remarks", F2FDbType.VarChar, strRemarks));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ClosureRemarks", F2FDbType.VarChar, strClosureRemarks));
                    DB.F2FCommand.ExecuteNonQuery();
                    intActionableId = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());

                    if (intActionableId == 0)
                    {
                        insertCircularActionableFiles(intActionableId, dtActionablefile);
                    }
                    else
                    {
                        insertCircularActionableFiles(intActionableId, dtActionablefile);
                    }
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in UpdateCircularActionables() " + ex);
                }
            }
            return retVal;
        }
        //>>

        //Modify By Milan Yadav on 25-Aug-2016
        public DataTable SearchCircularActionableNew(string strCircularNumber, string strCircularFromDate, string strCircularToDate,
            string strType, string strIssuingAuthority, string strTopic, string strSubject, string strActionable,
            string strPersonResponsible, string FromDate, string ToDate, string strStatus, string mstrConnectionString)
        {
            DataTable dtResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CIRC_searchCircularActinableList";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularNumber", F2FDbType.VarChar, strCircularNumber));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularFromDate", F2FDbType.VarChar, strCircularFromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularToDate", F2FDbType.VarChar, strCircularToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strType", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strIssueingAuthority", F2FDbType.VarChar, strIssuingAuthority));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strTopic", F2FDbType.VarChar, strTopic));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strSubject", F2FDbType.VarChar, strSubject));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strActionable", F2FDbType.VarChar, strActionable));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@PersonResponsible", F2FDbType.VarChar, strPersonResponsible));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Fromdate", F2FDbType.VarChar, FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Todate", F2FDbType.VarChar, ToDate));
                    //DB.F2FCommand.Parameters.Add(F2FParameter("@TargetDate", F2FDbType.VarChar, strtargetDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FDataAdapter.Fill(dtResults);
                    return dtResults;
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in SearchCircularActionableNew() " + ex);
                }
            }
        }



        public DataSet SearchCircularDetails(int intCircular, string strIssuingAuthority, string strDepartment, 
            string strTopic, string strCircularNo, string FromDate, string ToDate, string strTypeOfDocument, 
            string strSubject, string strGist, string strImplications, string strIntimatedDept, 
            string strActionType, string strCurUserEmail, string strAssociatedKeywords, string strToBePlacedBefore,
            string strGlobalSearch, string strCircularIds, string strIsFileToBeSearched, string strStatus)
        {
            DataSet dsResults = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CIRC_SearchCircularDetails";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularId", F2FDbType.Int32, intCircular));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularAuthId", F2FDbType.VarChar, strIssuingAuthority));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularDeptId", F2FDbType.VarChar, strDepartment));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Topic", F2FDbType.VarChar, strTopic));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircNo", F2FDbType.VarChar, strCircularNo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Fromdate", F2FDbType.VarChar, FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Todate", F2FDbType.VarChar, ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@TypeOfDocument", F2FDbType.VarChar, strTypeOfDocument));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Subject", F2FDbType.VarChar, strSubject));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Gist", F2FDbType.VarChar, strGist));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Implications", F2FDbType.VarChar, strImplications));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@IntimatedDept", F2FDbType.VarChar, strIntimatedDept));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ActionType", F2FDbType.VarChar, strActionType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@IntUserEmail", F2FDbType.VarChar, strCurUserEmail));
                    //<< Added by Amarjeet on 26-Jul-2021
                    DB.F2FCommand.Parameters.Add(F2FParameter("@AssociatedKeywords", F2FDbType.VarChar, strAssociatedKeywords));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ToBePlacedBefore", F2FDbType.VarChar, strToBePlacedBefore));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@GlobalSearch", F2FDbType.VarChar, strGlobalSearch));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularIds", F2FDbType.VarChar, strCircularIds));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FileSearch", F2FDbType.VarChar, strIsFileToBeSearched));
                    //<<Added by Ankur Tyagi on 06-Jul-2023
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    //>>
                    //>>
                    DB.F2FDataAdapter.Fill(dsResults);
                    return dsResults;
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in SearchCircularDetails() " + ex);
                }
            }
        }

        //Added By Milan Yadav on 30-Aug-2016
        //>>
        public void insertCircularActionableFiles(int intActionableId, DataTable dtFiles)
        {
            int retVal;
            if (dtFiles != null)
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.OpenConnection();
                    for (int i = 0; i < dtFiles.Rows.Count; i++)
                    {
                        DB.F2FCommand.CommandText = "CIRC_saveActionableFiles";
                        DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                        DB.F2FCommand.Parameters.Clear();
                        DataRow dr = dtFiles.Rows[i];
                        DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, 0));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CaId", F2FDbType.Int32, intActionableId));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@FileType", F2FDbType.VarChar, dr["Type"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@FileName", F2FDbType.VarChar, dr["FileName"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@ServerFileName", F2FDbType.VarChar, dr["FileNameOnServer"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@Creator", F2FDbType.VarChar, dr["Uploaded By"].ToString()));
                        retVal = DB.F2FCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public void DeleteCircularActionable(string strActId)
        {
            if (!string.IsNullOrEmpty(strActId))
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "DELETE FROM TBL_CIRCULAR_ACTIONABLES WHERE CA_ID = @CaId";
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CaId", F2FDbType.VarChar, strActId));
                    DB.OpenConnection();
                    DB.F2FCommand.ExecuteNonQuery();
                }
            }
        }
        //>>

        //<< Added by Amarjeet on 08-Jul-2021
        public void insertCircularCertChecklist(int intCircularId, DataTable dt, string strCreateBy)
        {
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    DataRow dr = dt.Rows[i];
                    DB.F2FCommand.CommandText = "CIRC_saveCircularCertChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Clear();
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularId", F2FDbType.Int32, intCircularId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCCId", F2FDbType.Int32, dr["Id"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DeptId", F2FDbType.Int32, dr["DeptId"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ActRegCirc", F2FDbType.VarChar, dr["ActRegCirc"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Reference", F2FDbType.VarChar, dr["Reference"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Clause", F2FDbType.VarChar, dr["Clause"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CheckPoints", F2FDbType.VarChar, dr["CheckPoints"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Particulars", F2FDbType.VarChar, dr["Particulars"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Penalty", F2FDbType.VarChar, dr["Penalty"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Frequency", F2FDbType.VarChar, dr["Frequency"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Forms", F2FDbType.VarChar, dr["Forms"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EffectiveFrom", F2FDbType.VarChar, dr["EffectiveFrom"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.ExecuteNonQuery();
                }
            }
        }

        public DataTable SearchCircularCertChecklist(int intCircular, string strCircCertChecklist, string strLoggedInUserName,
            string strType)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CIRC_searchCircularCertChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircularId", F2FDbType.Int32, intCircular));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CircCertChecklistId", F2FDbType.VarChar, strCircCertChecklist));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LoggedInUserName", F2FDbType.VarChar, strLoggedInUserName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FDataAdapter.Fill(dsResults);
                    return dsResults;
                }
                catch (Exception ex)
                {
                    throw new Exception("system exception in SearchCircularCertChecklist() " + ex);
                }
            }
        }

        public void DeleteCircularCertChecklist(string strCircCertChecklistId)
        {
            if (!string.IsNullOrEmpty(strCircCertChecklistId))
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "DELETE FROM TBL_CIRC_CERT_CHECKLISTS WHERE CCC_ID = @CCCId";
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCCId", F2FDbType.VarChar, strCircCertChecklistId));
                    DB.OpenConnection();
                    DB.F2FCommand.ExecuteNonQuery();
                }
            }
        }
        //>>

        //<< Added by Amarjeet on 09-Jul-2021
        public void acceptRejectCertChecklist(DataTable dt, string strStatus, string strCreateBy)
        {
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.OpenConnection();
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        DB.F2FCommand.CommandText = "CIRC_acceptRejectCertChecklist";
                        DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                        DB.F2FCommand.Parameters.Clear();
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CCCId", F2FDbType.Int32, dr["CCCId"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@Remarks", F2FDbType.VarChar, dr["Comments"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@LoggedInUser", F2FDbType.VarChar, strCreateBy));
                        DB.F2FCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("system exception in acceptRejectCertChecklist() " + ex);
            }
        }
        //>>
    }
}
