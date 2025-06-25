using System;
using System.Data;
using Fiction2Fact.App_Code;
using static Fiction2Fact.App_Code.F2FDatabase;

using Fiction2Fact.Legacy_App_Code.VO;
/// <summary>
/// Summary description for MailLogDAL
/// </summary>
namespace Fiction2Fact.Legacy_App_Code.DAL
{
    public class MailLogDAL
    {
        public MailLogDAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataTable getFailureMails(MailLogVO objVO)
        {
            DataTable dt = new DataTable();
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "SearchFailuredMails";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@MailID", F2FDbType.Int32, objVO.getMailId()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@MailStatus", F2FDbType.VarChar, objVO.getMailStatus()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@MailFromDate", F2FDbType.VarChar,objVO.getmailFromDate()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@MailToDate", F2FDbType.VarChar, objVO.getmailToDate()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Subject", F2FDbType.VarChar, objVO.getMailSubject()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Content", F2FDbType.VarChar, objVO.getMailContent()));
                    DB.F2FDataAdapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in getFailureMails() " + ex);
            }
            finally
            {
                //disposeSQLCommand(cmdUpdate);
                //closeConnection();
            }
            return dt;
        }

        public int insertIntoMailLog(MailLogVO objVO)
        {
            int retVal = 0;
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    DB.F2FCommand.CommandText = "insertMailSendStatus";
                    DB.F2FCommand.CommandType = CommandType.StoredProcedure;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@mailID", F2FDbType.Int32, objVO.getMailId()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@mailTO", F2FDbType.VarChar, objVO.getMailTO()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@mailCC", F2FDbType.VarChar, objVO.getMailCC()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@mailBCC", F2FDbType.VarChar, objVO.getMailBCC()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@mailSubject", F2FDbType.VarChar, objVO.getMailSubject()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@mailContent", F2FDbType.VarChar, objVO.getMailContent()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@mailStatus", F2FDbType.VarChar, objVO.getMailStatus()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@mailSentON", F2FDbType.DateTime, objVO.getMailSendOn()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@createBy", F2FDbType.VarChar, objVO.getCreatedBy()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@mailError", F2FDbType.VarChar, objVO.getErrorMsg()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@mailFailCount", F2FDbType.Int32, objVO.getMailFailCount()));
                    //<<Added by Milan Yadav on 17-Mar-2016
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@mailType", F2FDbType.VarChar, objVO.getMailType()));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@mailAttachmets", F2FDbType.VarChar, objVO.getMailAttachments()));
                    //>>
                    DB.OpenConnection();
                    retVal = Convert.ToInt32(DB.F2FCommand.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new System.Exception("system exception in insertIntoMailLog() " + ex);
            }
            finally
            {
                //disposeSQLCommand(DB.F2FCommand);
                //closeConnection();
            }
            return retVal;
        }
    }
}