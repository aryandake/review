using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Fiction2Fact.App_Code;
using static Fiction2Fact.App_Code.F2FDatabase;

namespace Fiction2Fact.Legacy_App_Code.Outward.DAL
{
    public class OutwardDAL
    {
        public string SaveOutwardTrackers(int intId, string strDocName,
                            string strOutwardDate, int intAuthority, int intDept, string strCreateBy, int intTypeOfoutward,
                           DataTable mdtEditFileUpload, string strCreator,string strStatus, string strRemarks, string strOldOutward)
        {
            string strConcatString = "", strDocNo = "";
            int retVal;

            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();

                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "OT_insertUpdateOutwardTrackers";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;

                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, intId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DocName", F2FDbType.VarChar, strDocName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OutwardDate", F2FDbType.DateTime, strOutwardDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Authority", F2FDbType.Int32, intAuthority));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Dept", F2FDbType.Int32, intDept));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@TypeofOutward", F2FDbType.Int32, intTypeOfoutward));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OutwardStatus", F2FDbType.VarChar, strStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Creator", F2FDbType.VarChar, strCreator));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Remarks", F2FDbType.VarChar, strRemarks));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OldOutward", F2FDbType.VarChar, strOldOutward));
                    strConcatString = Convert.ToString(DB.F2FCommand.ExecuteScalar());

                    if (intId == 0)
                    {
                        retVal = Convert.ToInt32(strConcatString.Split('|')[0]);
                        insertOutwardFiles(retVal, mdtEditFileUpload, DB);
                        strDocNo = strConcatString.Split('|')[1];
                    }
                    else
                    {
                        insertOutwardFiles(intId, mdtEditFileUpload, DB);
                        strDocNo = "";
                    }

                    DB.F2FTransaction.Commit();
                }
                catch (Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }

            return strDocNo;
        }

        private void insertOutwardFiles(int Id, DataTable dt, F2FDatabase DB)
        {
            if (dt != null)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    DataRow dr = dt.Rows[i];
                    DB.F2FCommand.CommandText = "OT_insertOutwardFiles";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Clear();
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OTId", F2FDbType.Int32, Id));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@fileName", F2FDbType.VarChar, dr["fileName"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@fileNameonServer", F2FDbType.VarChar, dr["fileNameonServer"].ToString()));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, dr["Uploaded By"].ToString()));

                    DB.F2FCommand.ExecuteNonQuery();
                }
            }
        }


        public string UpdateOutwardTrackers(int intId, string strAddressor, string strAddressee, string strDocName,
                            string strOutwardDate, int intAuthority, int intDept, string strCreateBy, int intTypeOfoutward,
                            string strRemarks, string strClosureRemark, DataTable mdtEditFileUpload,
                            string strPODNumber, string strDispatchDate, string strCourierName,
                            string strEmailSentDate, string strHardCopy, string strCreator,
                            string strRepresentation, string strRepresentationStatus,
                            string strRepresentationDate, string strTobeSend, string strStatus)
        {
            string strConcatString = "", strDocNo = "";
            int retVal;

            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();

                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "OT_UpdateOutwardTrackers";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;

                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, intId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Addressor", F2FDbType.VarChar, strAddressor));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Addressee", F2FDbType.VarChar, strAddressee));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DocName", F2FDbType.VarChar, strDocName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OutwardDate", F2FDbType.VarChar, strOutwardDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Authority", F2FDbType.Int32, intAuthority));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Dept", F2FDbType.Int32, intDept));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@TypeofOutward", F2FDbType.Int32, intTypeOfoutward));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Remarks", F2FDbType.VarChar, strRemarks));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ClosureRemarks", F2FDbType.VarChar, strClosureRemark));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@PODNumber", F2FDbType.VarChar, strPODNumber));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DispatchDate", F2FDbType.VarChar, (strDispatchDate.Equals("")) ? null : strDispatchDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CourierName", F2FDbType.VarChar, strCourierName));
                    //<< Added by Shwetan on 01-Sep-2021
                    DB.F2FCommand.Parameters.Add(F2FParameter("@EmailSentDate", F2FDbType.VarChar, (strEmailSentDate.Equals("") ? null : strEmailSentDate)));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@HardCopy", F2FDbType.VarChar, strHardCopy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Creator", F2FDbType.VarChar, strCreator));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Representation", F2FDbType.VarChar, strRepresentation));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@RepresentationStatus", F2FDbType.VarChar, strRepresentationStatus));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@RepresentationDate", F2FDbType.VarChar, strRepresentationDate.Equals("") ? null : strRepresentationDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@TobeSend", F2FDbType.VarChar, strTobeSend.Equals("") ? null : strTobeSend));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OutwardStatus", F2FDbType.VarChar, strStatus));
                    
                    //>>
                    strConcatString = Convert.ToString(DB.F2FCommand.ExecuteScalar());

                    if (intId == 0)
                    {
                        retVal = Convert.ToInt32(strConcatString.Split('|')[0]);
                        insertOutwardFiles(retVal, mdtEditFileUpload, DB);
                        strDocNo = strConcatString.Split('|')[1];
                    }
                    else
                    {
                        insertOutwardFiles(intId, mdtEditFileUpload, DB);
                        strDocNo = "";
                    }

                    DB.F2FTransaction.Commit();
                }
                catch (Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }

            return strDocNo;
        }

        public string SuggestRevisionOutwardTrackers(string strId, string strSuggestRemark, string strCreateBy, string strStatus)
        {
            string strConcatString = "", strDocNo = "";
            int retVal;

            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();

                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "OT_SuggestRevisionOutwardTrackers";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;

                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.VarChar, strId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@SuggestRemark", F2FDbType.VarChar, strSuggestRemark));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));

                    strConcatString = Convert.ToString(DB.F2FCommand.ExecuteScalar());
                    DB.F2FTransaction.Commit();
                }
                catch (Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }

            return strDocNo;
        }

        public string AddUpdatesOutwardTrackers(int intId, string UpdateDate, string Remark, string strCreateBy)
        {
            string strConcatString = "", strDocNo = "";
            int retVal;

            using (F2FDatabase DB = new F2FDatabase())
            {
                DB.OpenConnection();
                DB.F2FTransaction = DB.F2FConnection.BeginTransaction();

                try
                {
                    DB.F2FCommand.Transaction = DB.F2FTransaction;
                    DB.F2FCommand.CommandText = "OT_AddUpdateOutwardTrackers";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;

                    DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, intId));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@UpdateDate", F2FDbType.DateTime, UpdateDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Remark", F2FDbType.VarChar, Remark));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@CreateBy", F2FDbType.VarChar, strCreateBy));

                    strConcatString = Convert.ToString(DB.F2FCommand.ExecuteScalar());
                    DB.F2FTransaction.Commit();
                }
                catch (Exception ex)
                {
                    DB.F2FTransaction.Rollback();
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }

            return strDocNo;
        }

        
        public void cancelOutwardTrackers(int Id, string Remarks, string strCreateBy, string CancelOn)
        {
            using (F2FDatabase DB = new F2FDatabase())
            {
                //string strSQL = " UPDATE [TBL_OUTWARD_TRACKING] SET [OT_STAUTS] = 'Cancelled', " +
                //                " [OT_CANCEL_REMARKS]= '" + Remarks + "' " +
                //                " where OT_ID = " + Id;

                string strSQL = " UPDATE [TBL_OUTWARD_TRACKING] SET [OT_STAUTS] = 'Cancelled', " +
                                " [OT_CANCEL_REMARKS] = @Remarks " +
                                " ,[OT_CANCEL_BY]=@strCreateBy , [OT_CANCEL_ON]=@CancelOn" +
                                "  where OT_ID = @Id ";

                DB.OpenConnection();
                DB.F2FCommand.CommandText = strSQL;

                DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, Id));
                DB.F2FCommand.Parameters.Add(F2FParameter("@Remarks", F2FDbType.VarChar, Remarks));
                DB.F2FCommand.Parameters.Add(F2FParameter("@strCreateBy", F2FDbType.VarChar, strCreateBy));
                DB.F2FCommand.Parameters.Add(F2FParameter("@CancelOn", F2FDbType.VarChar, CancelOn));
                DB.F2FCommand.ExecuteNonQuery();
            }
        }
        public void deleteOutwardTrackers(int Id, string Remarks, string strCreateBy, string DeleteOn)
        {
            using (F2FDatabase DB = new F2FDatabase())
            {
                //string strSQL = " UPDATE [TBL_OUTWARD_TRACKING] SET [OT_STAUTS] = 'Cancelled', " +
                //                " [OT_CANCEL_REMARKS]= '" + Remarks + "' " +
                //                " where OT_ID = " + Id;

                string strSQL = " UPDATE [TBL_OUTWARD_TRACKING] SET [OT_STAUTS] = 'Deleted', " +
                                " [OT_DELETE_REMARK] = @Remarks " +
                                " ,[OT_DELETEED_BY]=@strCreateBy , [OT_DELETED_ON]=@DeleteOn" +
                                "  where OT_ID = @Id ";

                DB.OpenConnection();
                DB.F2FCommand.CommandText = strSQL;

                DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, Id));
                DB.F2FCommand.Parameters.Add(F2FParameter("@Remarks", F2FDbType.VarChar, Remarks));
                DB.F2FCommand.Parameters.Add(F2FParameter("@strCreateBy", F2FDbType.VarChar, strCreateBy));
                DB.F2FCommand.Parameters.Add(F2FParameter("@DeleteOn", F2FDbType.VarChar, DeleteOn));
                DB.F2FCommand.ExecuteNonQuery();
            }
        }
        

        public void revisionOutwardTrackers(int Id, string RevisionRemarks, string strCreateBy, string RevisionOn)
        {
            using (F2FDatabase DB = new F2FDatabase())
            {
                string strSQL = " UPDATE [TBL_OUTWARD_TRACKING] SET [OT_STAUTS] = 'Changes suggested by Compliance', " +
                                " [OT_SUGGEST_REVISION_REMARK] = @RevisionRemarks " +
                                " ,[OT_SUGGEST_REVISION_BY]=@strCreateBy , [OT_SUGGEST_REVISION_DT]=@RevisionOn" +
                                "  where OT_ID = @Id ";

                DB.OpenConnection();
                DB.F2FCommand.CommandText = strSQL;

                DB.F2FCommand.Parameters.Add(F2FParameter("@Id", F2FDbType.Int32, Id));
                DB.F2FCommand.Parameters.Add(F2FParameter("@RevisionRemarks", F2FDbType.VarChar, RevisionRemarks));
                DB.F2FCommand.Parameters.Add(F2FParameter("@strCreateBy", F2FDbType.VarChar, strCreateBy));
                DB.F2FCommand.Parameters.Add(F2FParameter("@RevisionOn", F2FDbType.VarChar, RevisionOn));
                DB.F2FCommand.ExecuteNonQuery();
            }
        }
        
        public DataSet SearchOutwards(string strDocNo, string strAddressor, string strAddressee,
                                            string strDocumentName, string strOutwardType, string strOutwardDate,
                                            string strRegAuth, string strDept, string strStatus, string strType,
                                            string strLoggedInUser, string strGlobalSearch,string FromDate,
                                            string ToDate,string filename, string strFlag)
        {
            DataSet dsResults = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "OT_searchOutwardTrackers";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;

                    DB.F2FCommand.Parameters.Add(F2FParameter("@TypeofOutward", F2FDbType.VarChar, strOutwardType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DocNo", F2FDbType.VarChar, strDocNo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Addressor", F2FDbType.VarChar, strAddressor.Replace("'", "''")));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Addressee", F2FDbType.VarChar, strAddressee.Replace("'", "''")));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DocName", F2FDbType.VarChar, strDocumentName));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@FrmOutwardDate", F2FDbType.VarChar, strOutwardDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@ToOutwardDate", F2FDbType.VarChar, ""));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@RegulAuthority", F2FDbType.VarChar, strRegAuth));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@DeptId", F2FDbType.VarChar, strDept));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Status", F2FDbType.VarChar, strStatus));
                    // << Added by ramesh more on 15-Dec-2023 CR_1909
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Type", F2FDbType.VarChar, strType));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LoggedInUser", F2FDbType.VarChar, strLoggedInUser));

                    // >>
                    //<< Added by shwetan on 13-Sep-2021
                    DB.F2FCommand.Parameters.Add(F2FParameter("@GlobalSearch", F2FDbType.VarChar, strGlobalSearch));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Fromdate", F2FDbType.VarChar, FromDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Todate", F2FDbType.VarChar, ToDate));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OutwardsIds", F2FDbType.VarChar, ""));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@OutwardsFiles", F2FDbType.VarChar, filename));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@Flag", F2FDbType.VarChar, strFlag));
                    //>>
                    DB.F2FDataAdapter.Fill(dsResults);
                    return dsResults;
                }
                catch (Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }

            return dsResults;
        }

        public DataSet searchOutwardformail(string strDocNo, string strLoggedInUser, string strstatus)
        {
            DataSet dsResults = new DataSet();
            using (F2FDatabase DB = new F2FDatabase())
            {
                try
                {
                    DB.F2FCommand.CommandText = "OT_searchOutwardTrackersforMail";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;

                    DB.F2FCommand.Parameters.Add(F2FParameter("@DocNo", F2FDbType.VarChar, strDocNo));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@LoggedInUser", F2FDbType.VarChar, strLoggedInUser));
                    DB.F2FCommand.Parameters.Add(F2FParameter("@status", F2FDbType.VarChar, strstatus));
                    DB.F2FDataAdapter.Fill(dsResults);
                    return dsResults;
                }
                catch (Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
                }
            }

            return dsResults;
        }
    }
}
