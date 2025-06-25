using Fiction2Fact.App_Code;
using System;
using System.Data;
using System.Data.SqlClient;
using static Fiction2Fact.App_Code.F2FDatabase;

namespace Fiction2Fact.Legacy_App_Code.Certification.DAL
{
    public class CertificationMasterDAL
    {
        public int saveCertificationMas(int cID, string strDepartment, int intLevel, string strContents, string strCreateBy, string strConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CERT_saveCertificationMas";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@cID", F2FDbType.Int32, cID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strDepartment", F2FDbType.VarChar, strDepartment));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strContents", F2FDbType.VarChar, strContents));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strCreateBy", F2FDbType.VarChar, strCreateBy));
                    //Added by milan Yadav on 20-Feb-2016
                    //<<
                    DB.F2FCommand.Parameters.Add(F2FParameter("@intLevel", F2FDbType.VarChar, intLevel));
                    //>>
                    DB.OpenConnection();
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
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

        public string saveCertification(int intCertId, int intCertMasId, int intCertQuarterId, string strContent, string strRemarks,
            string strStatus, string strCreateBy, DataTable dtNewException, DataTable dtChecklist, string strsubDepartment,
            string strLoggedInUser)
        {
            string retVal = "";
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CERT_saveCertification";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertId", F2FDbType.Int32, intCertId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertMasId", F2FDbType.Int32, intCertMasId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertQuarterId", F2FDbType.Int32, intCertQuarterId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strContent", F2FDbType.VarChar, strContent));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strRemarks", F2FDbType.VarChar, strRemarks));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strStatus", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strCreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@subDepartment", F2FDbType.VarChar, strsubDepartment));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LoggedInUserId", F2FDbType.VarChar, strLoggedInUser));

                    retVal = DB.F2FCommand.ExecuteScalar().ToString();

                    if (intCertId != 0)
                    {
                        insertExceptionDetails(intCertId, dtNewException, strCreateBy);
                        insertChecklistDetails(intCertId, dtChecklist, strCreateBy);
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
            return retVal;
        }
        //>>


        private void insertExceptionDetails(int Id, DataTable dt, string CreateBy)
        {
            int retVal;
            if (dt != null)
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.OpenConnection();
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        DB.F2FCommand.CommandText = "CERT_insertExceptionDetails";
                        DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                        DB.F2FCommand.Parameters.Clear();
                        DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, dr["ID"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@intCertId", F2FDbType.Int32, Id));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@strExceptionType", F2FDbType.VarChar, dr["Exception Type"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@strDetails", F2FDbType.VarChar, dr["Details"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@strClientFileName", F2FDbType.VarChar, dr["Client File Name"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@strServerFileName", F2FDbType.VarChar, dr["Server File Name"].ToString()));
                        //Added By Milan Yadav On 05-Feb-2016
                        DB.F2FCommand.Parameters.Add(F2FParameter("@strRootCause", F2FDbType.VarChar, dr["RootCause"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@strActionTaken", F2FDbType.VarChar, dr["Actiontaken"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@Actionstatus", F2FDbType.VarChar, dr["Actionstatus"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@strTargetDate", F2FDbType.DateTime, ((string)dr["TargetDate"] == "" ? null : dr["TargetDate"].ToString())));
                        //>>
                        DB.F2FCommand.Parameters.Add(F2FParameter("@strCreateBy", F2FDbType.VarChar, CreateBy));
                        retVal = DB.F2FCommand.ExecuteNonQuery();
                    }
                }
            }
        }
        //>>


        public void insertChecklistDetails(int Id, DataTable dt, string CreatedBy)
        {
            int retVal;
            if (dt != null)
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.OpenConnection();
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        DB.F2FCommand.CommandText = "CERT_saveCertificationChecklistDetails";
                        DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                        DB.F2FCommand.Parameters.Clear();
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CDId", F2FDbType.VarChar, dr["ChecklistDetsId"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CertificationID", F2FDbType.Int32, Id));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@ChecklistMasID", F2FDbType.Int32, dr["ChecklistMasId"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, CreatedBy));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@YesNoNa", F2FDbType.VarChar, dr["YesNoNa"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@Remarks", F2FDbType.VarChar, dr["Remarks"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@ClientFileName", F2FDbType.VarChar, dr["ClientFileName"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@ServerFileName", F2FDbType.VarChar, dr["ServerFileName"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@ActionPlan", F2FDbType.VarChar, dr["ActionPlan"].ToString()));
                        DB.F2FCommand.Parameters.Add(F2FParameter("@TargetDate", F2FDbType.DateTime, dr["TargetDate"].ToString() == "" ? null : dr["TargetDate"].ToString()));
                        //<<Added by Ankur Tyagi on 05Feb2024 for CR_1948
                        DB.F2FCommand.Parameters.Add(F2FParameter("@NCSinceDate", F2FDbType.DateTime, dr["NCSinceDate"].ToString() == "" ? null : dr["NCSinceDate"].ToString()));
                        //>>
                        retVal = DB.F2FCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public DataSet SearchCert(int intCId, string intLevel, string strDeptName, string strConnectionString)
        {
            DataSet dsResults = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_SearchCert";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.VarChar, Convert.ToString(intCId)));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@IntLevel", F2FDbType.VarChar, intLevel));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strDeptName", F2FDbType.VarChar, strDeptName));
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

        public DataSet searchCertifications(int intCertId, string strDeptName, string strQuarter, string strConnectionString)
        {
            DataSet dsResults = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_searchCertifications";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.VarChar, Convert.ToString(intCertId)));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strDeptName", F2FDbType.VarChar, strDeptName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strQuarter", F2FDbType.VarChar, strQuarter));
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

        //<<Changed by Denil Shah on 12-Apr-2017 to include Level while doing the search.
        public DataSet searchEditCertifications(int intCertId, string strDeptName, string strQuarter,
            string strCreateBy, int intLevel,
            string strConnectionString)
        {
            DataSet dsResults = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_searchEditCertifications";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.VarChar, Convert.ToString(intCertId)));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strDeptName", F2FDbType.VarChar, strDeptName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strQuarter", F2FDbType.VarChar, strQuarter));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strCreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@intLevel", F2FDbType.Int32, intLevel));
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

        public int deleteCertificate(int intCertId, string strConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CERT_deleteCertificate";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@intCertId", F2FDbType.Int32, intCertId));
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

        public int deleteCertContent(int intCertId, string strConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CERT_deleteCertContent";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@intCertId", F2FDbType.Int32, intCertId));
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


        public int saveCommonCertification(int intCertId, int intCertMasId, int intCertQuarterId, string strContent,
                                    string strRemarks, string strStatus, string strRole,
                                    string strCreateBy, string strConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CERT_saveCommonCertification";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertId", F2FDbType.Int32, intCertId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertMasId", F2FDbType.Int32, intCertMasId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertQuarterId", F2FDbType.Int32, intCertQuarterId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strContent", F2FDbType.VarChar, strContent));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strRemarks", F2FDbType.VarChar, strRemarks));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strStatus", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strCreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Role", F2FDbType.VarChar, strRole));
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
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
        //>>

        //Added By Bhavik on 10-Sep-2013
        public int saveCertificationChecklist
            (int intChecklistID, string strDepartmentID, string strReference, string strTitleofSection, string strSelfAssessmentStatus,
            string strCheckpoints, string strFrequency, string strDueDate, string strSourceDept, string strDeptRespFurnish,
            string strDeptRespSubmitting, string strTobeFilledwith, string strEffectiveDateFrom, string strStatus,
            string strEffectiveDateTo, string strCreatedBy, string strRemark, string strPenalty, string strActRegCirc,
            string strForms, string strConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CERT_saveCertificationChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CId", F2FDbType.Int32, intChecklistID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DepartmentID", F2FDbType.Int32, strDepartmentID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Reference", F2FDbType.VarChar, strReference));

                    DB.F2FCommand.Parameters.Add(F2FParameter("@TitleofSection", F2FDbType.VarChar, strTitleofSection));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Particulars", F2FDbType.VarChar, strSelfAssessmentStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Checkpoints", F2FDbType.VarChar, strCheckpoints));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Frequency", F2FDbType.VarChar, strFrequency));
                    //DB.F2FCommand.Parameters.Add(F2FParameter("@DueDate", F2FDbType.VarChar, strDueDate));
                    //DB.F2FCommand.Parameters.Add(F2FParameter("@SourceDept", F2FDbType.VarChar, strSourceDept));
                    //DB.F2FCommand.Parameters.Add(F2FParameter("@DeptRespFurnish", F2FDbType.VarChar, strDeptRespFurnish));
                    //DB.F2FCommand.Parameters.Add(F2FParameter("@DeptRespSubmitting", F2FDbType.VarChar, strDeptRespSubmitting));
                    //DB.F2FCommand.Parameters.Add(F2FParameter("@TobeFilledwith", F2FDbType.VarChar, strTobeFilledwith));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Penalty", F2FDbType.VarChar, strPenalty));

                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreatedBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Remark", F2FDbType.VarChar, strRemark));

                    if (strEffectiveDateFrom == null || strEffectiveDateFrom.Equals(""))
                    {
                        DB.F2FCommand.Parameters.Add(F2FParameter("@EffectiveFrom", F2FDbType.VarChar, DBNull.Value));
                    }
                    else
                    {
                        DB.F2FCommand.Parameters.Add(F2FParameter("@EffectiveFrom", F2FDbType.VarChar, strEffectiveDateFrom));
                    }


                    if (strEffectiveDateTo == null || strEffectiveDateTo.Equals(""))
                    {
                        DB.F2FCommand.Parameters.Add(F2FParameter("@EffectiveTo", F2FDbType.VarChar, DBNull.Value));
                    }
                    else
                    {
                        DB.F2FCommand.Parameters.Add(F2FParameter("@EffectiveTo", F2FDbType.VarChar, strEffectiveDateTo));
                    }

                    //<< Added by Amarjeet on 23-Jul-2021
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ActRegCirc", F2FDbType.VarChar, strActRegCirc));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Forms", F2FDbType.VarChar, strForms));
                    //>>

                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
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


        public DataTable getCertificationsForApproval(string strId, string strStatus, string strUsername, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_getCertificationsForApproval";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.VarChar, strId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Username", F2FDbType.VarChar, strUsername));
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


        public string updateCerification(int intCertId, string strOperationType, string strRemarks, string strCreateBy, string strType,
            string strLoggedInUserId, string strUserName)
        {
            string retVal = "";
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CERT_updateCerification";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertId", F2FDbType.Int32, intCertId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OperationType", F2FDbType.VarChar, strOperationType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Remarks", F2FDbType.VarChar, strRemarks));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LoggedInUserId", F2FDbType.VarChar, strLoggedInUserId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UserName", F2FDbType.VarChar, strUserName));
                    retVal = DB.F2FCommand.ExecuteScalar().ToString();
                    //DB.F2FCommand.ExecuteScalar().ToString();
                    DB.F2FTransaction.Commit();
                }
                catch (Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retVal;
        }

        public DataTable searchCertificationsChecklist(int intCertId, string strDeptName, string strQuarter, string strStatus, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_searchCertificationsChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.VarChar, Convert.ToString(intCertId)));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strDeptName", F2FDbType.VarChar, strDeptName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strQuarter", F2FDbType.VarChar, strQuarter));
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

        public DataTable getCertificationForPendingApproval(string strType, string strLoggedInUser, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_getCertificationForPendingApproval";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LoggedInUser", F2FDbType.VarChar, strLoggedInUser));
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

        public DataTable getCertificationsChecklist(string strCertId, string strDeptId, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_getCertificationsChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertId", F2FDbType.VarChar, strCertId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DeptId", F2FDbType.VarChar, strDeptId));
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

        public DataTable getCertificationsApproval(string strDeptId, string strType, string strUsername, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_getCertificationsApproval";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DeptId", F2FDbType.VarChar, strDeptId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Username", F2FDbType.VarChar, strUsername));
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

        //Modified By Milan Yadav on 13-Sep-2016
        public int saveCertificationQuarterMas(int intId, string strFromDt, string strToDt, string strDueDt, string strStatus, string strCreateBy, string strConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.F2FCommand.CommandText = "CERT_saveCertificationQuarterMas";
                try
                {
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, intId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FromDate", F2FDbType.VarChar, strFromDt));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ToDate", F2FDbType.VarChar, strToDt));
                    //Added By Milan Yadav on 13-Sep-2016
                    //>>
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DueDate", F2FDbType.VarChar, strDueDt));
                    //<<
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Creator", F2FDbType.VarChar, strCreateBy));
                    DB.OpenConnection();
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retVal;
        }

        public DataTable searchCertificationQuarterMas(string strId, string strFromDt, string strToDt, string strStatus, string strConnectionString)
        {
            DataTable dt = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_searchCertificationQuarterMas";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.VarChar, strId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FromDate", F2FDbType.VarChar, strFromDt));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ToDate", F2FDbType.VarChar, strToDt));
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

        public int generateQuarterlyCertification(int intQuarterId, string strConnectionString)
        {

            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_generateQuarterlyCertification";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@QuarterId", F2FDbType.Int32, intQuarterId));
                    DB.OpenConnection();
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

        //Added by Vivek on 20Nov2015
        public int saveSubCertificationDeptMas(int intId, string strCertDept, string strCertSubDeptName, string strUserId, string strUserName, string strEmailId,
                                   string strCreateBy, DataTable dtCertSubDepartment, string strConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CERT_saveSubCertificationDeptMas";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, intId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertDept", F2FDbType.Int32, Convert.ToInt32(strCertDept)));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertSubDept", F2FDbType.VarChar, strCertSubDeptName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UserId", F2FDbType.VarChar, strUserId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UserName", F2FDbType.VarChar, strUserName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EmailId", F2FDbType.VarChar, strEmailId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));

                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                    if (intId != 0)
                    {
                        insertCertSubDepartmentDetails(intId, dtCertSubDepartment, strCreateBy);
                    }
                    else
                    {
                        insertCertSubDepartmentDetails(retVal, dtCertSubDepartment, strCreateBy);
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
            return retVal;
        }

        private void insertCertSubDepartmentDetails(int Id, DataTable dt, string CreateBy)
        {
            int retVal;
            if (dt != null)
            {
                try
                {
                    using (F2FDatabase DB = new F2FDatabase())
                    {
                        DB.OpenConnection();
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            DB.F2FCommand.CommandText = "CERT_saveSubSubCertificationDeptMas";
                            DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                            DB.F2FCommand.Parameters.Clear();
                            DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, dr["Id"].ToString()));
                            DB.F2FCommand.Parameters.Add(F2FParameter("@CertDeptId", F2FDbType.Int32, Id));
                            DB.F2FCommand.Parameters.Add(F2FParameter("@DeptName", F2FDbType.VarChar, dr["DeptName"].ToString()));
                            DB.F2FCommand.Parameters.Add(F2FParameter("@UserId", F2FDbType.VarChar, dr["UserId"].ToString()));
                            DB.F2FCommand.Parameters.Add(F2FParameter("@UserName", F2FDbType.VarChar, dr["UserName"].ToString()));
                            DB.F2FCommand.Parameters.Add(F2FParameter("@EmailId", F2FDbType.VarChar, dr["EmailId"].ToString()));
                            DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, CreateBy));
                            retVal = DB.F2FCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
        }

        public int insertException(int intCertId, string strApplicationLawPop, string strObservationPop, string strClientFileName,
            string strServerFileName, string strRootCausePop, string strActionTakenPop, string strTargetDatePop, string strCreateBy,
            string strConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_insertExceptionDetails";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, 0));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@intCertId", F2FDbType.Int32, intCertId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strExceptionType", F2FDbType.VarChar, strApplicationLawPop));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strDetails", F2FDbType.VarChar, strObservationPop));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strClientFileName", F2FDbType.VarChar, strClientFileName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strServerFileName", F2FDbType.VarChar, strServerFileName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strRootCause", F2FDbType.VarChar, strRootCausePop));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strActionTaken", F2FDbType.VarChar, strActionTakenPop));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strTargetDate", F2FDbType.DateTime, strTargetDatePop));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strCreateBy", F2FDbType.VarChar, strCreateBy));
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

        //Added By Milan yadav on 1-June-2016
        //>>
        public DataTable GetCertChecklistDetailById(string strDepartmentId, string strCertChecklistMasId, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_getCertChecklistDetailById";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DepartmentId", F2FDbType.VarChar, strDepartmentId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertChecklistMasId", F2FDbType.VarChar, strCertChecklistMasId));
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

        public DataTable getCertIdWiseCertification(int intCertId, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_getCertIdWiseCertification";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertId", F2FDbType.Int32, intCertId));
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

        public DataTable getChecklistByCertId(int intCertId, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_getChecklistByCertId";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertId", F2FDbType.Int32, intCertId));
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

        public DataTable getAllChecklistByCertificationId(int intCertId, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_getAllChecklistByCertificationId";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertId", F2FDbType.Int32, intCertId));
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

        //Added By Milan Yadav on 28-Jun-2016
        //>>
        public int generateSeparateQuarterlyCertification(int intQuarterId, int intDepartmentId, string strConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_generateSeparateQuarterlyCertification";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@QuarterId", F2FDbType.Int32, intQuarterId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DepartmentId", F2FDbType.Int32, intDepartmentId));
                    DB.OpenConnection();
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
        //<<
        //<<
        //Added By Milan Yadav on 27-Sep-2016
        //>>
        public int deActivateCertificationChecklist
            (int intChecklistID, string strStatus, string strEffectiveDateTo, string strCreatedBy, string strRemark, string strConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CERT_deactivateCertificationChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CId", F2FDbType.Int32, intChecklistID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreatedBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Remark", F2FDbType.VarChar, strRemark));
                    if (strEffectiveDateTo == null || strEffectiveDateTo.Equals(""))
                    {
                        DB.F2FCommand.Parameters.Add(F2FParameter("@EffectiveTo", F2FDbType.VarChar, DBNull.Value));
                    }
                    else
                    {
                        DB.F2FCommand.Parameters.Add(F2FParameter("@EffectiveTo", F2FDbType.VarChar, strEffectiveDateTo));
                    }

                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
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
        //Added By Milan Yadav on 27-Sep-2016
        //>>
        public DataTable getChecklistByCertDetsId(int intCertDetsId, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_getChecklistByCertDetsId";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CheckDetsId", F2FDbType.Int32, intCertDetsId));
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
        //<<
        //Added By Milan Yadav on 20-Oct-2016
        //>>
        public DataSet viewPastCertifications(int intCertId, string strDeptName, string strQuarter, string strCreateBy, int intlevel,
         string strConnectionString)
        {
            DataSet dsResults = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_viewPastCertifications";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.VarChar, Convert.ToString(intCertId)));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strDeptName", F2FDbType.VarChar, strDeptName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strQuarter", F2FDbType.VarChar, strQuarter));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@strLoggedInUser", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@intLevel", F2FDbType.VarChar, intlevel));
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
        //<<

        //<< Added by Denil Shah on 12-Apr-2017

        public DataTable viewPastConsolidateChecklist(string strCertId, string strDeptId, string strQuarterId, string strLevel,
            string strUsername, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_viewPastConsolidateChecklist";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertId", F2FDbType.VarChar, strCertId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DeptId", F2FDbType.VarChar, strDeptId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@QuarterId", F2FDbType.VarChar, strQuarterId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Level", F2FDbType.VarChar, strLevel));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Username", F2FDbType.VarChar, strUsername));
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


        //<<Added By Milan Yadav on 15-Apr-2017

        public int UpdateCertificationForApprover
           (int intCertId, string strStatus, string strCreatedBy, string strQuarterId,
            string strContent, string strConnectionString)
        {
            int retVal = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_UpdateCertForApprover";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertTMId", F2FDbType.Int32, intCertId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Content", F2FDbType.VarChar, strContent));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LoggedInUserId", F2FDbType.VarChar, strCreatedBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@QuarterId", F2FDbType.Int32, Convert.ToInt32(strQuarterId)));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.OpenConnection();
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return retVal;
        }
        //>>

        //<< Added By Milan Yadav on 24-Apr-2017

        public DataTable viewConsolidateExceptions(string strCertId, string strDeptId, string strQuarterId, string strLevel,
            string strUsername, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_viewConsolidateExceptions";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CertId", F2FDbType.VarChar, strCertId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DeptId", F2FDbType.VarChar, strDeptId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@QuarterId", F2FDbType.VarChar, strQuarterId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Level", F2FDbType.VarChar, strLevel));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Username", F2FDbType.VarChar, strUsername));
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
        //>>

        public int generateSeparateQuarterlyJointCertification(int intQuarterId, int intDepartmentId, string strConnectionString)
        {
            int intRowsInserted = 0;
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_generateSeparateQuarterlyJointCertification";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@QuarterId", F2FDbType.Int32, intQuarterId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DepartmentId", F2FDbType.Int32, intDepartmentId));
                    DB.OpenConnection();
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
        public string ValidateCertificationRollout(int iQtrId, int iDeptId)
        {
            string strRetVal = null;
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "CERT_ValidateCertificationRollout";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@QuarterId", F2FDbType.Int32, iQtrId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DeptId", F2FDbType.Int32, iDeptId));
                    var outParam = F2FParameter("@RetVal", F2FDbType.VarChar, null, ParameterDirection.Output, 1000);
                    DB.F2FCommand.Parameters.Add(outParam);
                    DB.OpenConnection();
                    DB.F2FCommand.ExecuteNonQuery();
                    return outParam.Value.ToString();
                }
                catch (System.Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
                return strRetVal;
            }
        }

        //<< Common Certification content
        public DataTable getCertCommonContent()
        {
            DataTable dt = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "Select * from TBL_CERT_COMMON_CONTENT";
                    DB.F2FCommand.CommandType = CommandType.Text;
                    DB.F2FDataAdapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }

            return dt;
        }

        public int saveCertCommonContent(string strId, string strContent, string strCreator)
        {
            int intRetVal = 0;

            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "saveCertCommonContent";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.VarChar, strId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Content", F2FDbType.VarChar, strContent));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Creator", F2FDbType.VarChar, strCreator));

                    intRetVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar().ToString());

                    DB.F2FTransaction.Commit();
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }

            return intRetVal;
        }
        //>>

        public DataTable CERT_getChklistForClosure(int intQtrId, int intCSSDM_ID, int intCSDM_ID, int intCDM_ID, string strUserId, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CERT_getChklistForClosure";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@QTRId", F2FDbType.Int32, intQtrId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSSDM_ID", F2FDbType.Int32, intCSSDM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSDM_ID", F2FDbType.Int32, intCSDM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CDM_ID", F2FDbType.Int32, intCDM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UserID", F2FDbType.VarChar, strUserId));

                    DB.F2FDataAdapter.Fill(dsResults);
                    return dsResults;
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dsResults;
        }

        public void CERT_saveChklistClosure(int intCCDId, string strClosureDate, string strClosureRemarks, string strUsername, string strType, string strConnectionString)
        {
            int intRetVal = 0;

            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CERT_saveChklistClosure";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CCDId", F2FDbType.Int32, intCCDId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ClosureDate", F2FDbType.VarChar, strClosureDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ClosureRemarks", F2FDbType.VarChar, strClosureRemarks));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Username", F2FDbType.VarChar, strUsername));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    intRetVal = DB.F2FCommand.ExecuteNonQuery();

                    DB.F2FTransaction.Commit();
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
        }

        public DataTable CERT_getExceptionForClosure(int intQtrId, int intCSSDM_ID, int intCSDM_ID, int intCDM_ID, 
            string strUserId, string strStatus, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "CERT_getExceptionForClosure";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@QTRId", F2FDbType.Int32, intQtrId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSSDM_ID", F2FDbType.Int32, intCSSDM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CSDM_ID", F2FDbType.Int32, intCSDM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CDM_ID", F2FDbType.Int32, intCDM_ID));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UserID", F2FDbType.VarChar, strUserId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    DB.F2FDataAdapter.Fill(dsResults);
                    return dsResults;
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
                return dsResults;
            }
        }

        //<<Added by Ankur Tyagi on 23Jan2024
        public DataTable Cert_getRegulatoryRecords(int intQtrId, int intLevel, int intDeptId,
            string strIsExportedToPDForDoc, string strConnectionString)
        {
            DataTable dsResults = new DataTable();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "Cert_getRegulatoryRecords";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FParameter("@QTRId", F2FDbType.Int32, intQtrId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Level", F2FDbType.Int32, intLevel));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DeptId", F2FDbType.Int32, intDeptId));

                    DB.F2FDataAdapter.Fill(dsResults);
                    return dsResults;
                }
                catch (System.Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }
            return dsResults;
        }
        //>>

    }
}
