using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Fiction2Fact.App_Code
{
    public class F2FLog
    {
        public static string F2FEnvironment { get; set; }
		static F2FLog()
		{
            F2FEnvironment = ConfigurationManager.AppSettings.AllKeys.Contains("ENVIRONMENT") ? ConfigurationManager.AppSettings["ENVIRONMENT"].ToString().ToUpper() : "PRODUCTION";
			List<string> _ENVS = new List<string>()
			{
				"DEVELOPMENT",
				"TESTING",
				"PRODUCTION"
			};
			if (!_ENVS.Contains(F2FEnvironment)) { F2FEnvironment = "PRODUCTION"; }
		}
        /// <summary>
        /// This function is to write logs based on Web.Config Setting, either To Text File or to Database or to Both
        /// </summary>
        /// <param name="Ex">Actual Exception occured</param>
        /// <param name="strPage">Name Page or Class</param>
        /// <param name="strFuncton">Name of Function</param>
        /// <param name="strAdditionalInfo">Addition inforation to log if any along with exception</param>
        public static string Log(Exception Ex, string strPage = null, string strFuncton = null, string strAdditionalInfo = null, bool bThrow = true)
        {
            int iLogID = 0;
            try
            {
                string sLogType = ConfigurationManager.AppSettings.AllKeys.Contains("LogOutputTo") ? ConfigurationManager.AppSettings["LogOutputTo"].ToString() : "0";
                switch (sLogType)
                {
                    case "0":
                        iLogID = WriteLogToDatabase(Ex, strPage, strFuncton, strAdditionalInfo);
                        break;
                    case "1":
                        WriteLogToFile(Ex, strPage, strFuncton, strAdditionalInfo);
                        break;
                    case "2":
                        WriteLogToFile(Ex, strPage, strFuncton, strAdditionalInfo);
                        iLogID = WriteLogToDatabase(Ex, strPage, strFuncton, strAdditionalInfo);
                        break;
                    default:
                        iLogID = WriteLogToDatabase(Ex, strPage, strFuncton, strAdditionalInfo);
                        break;
                }
                //if (bThrow) throw Ex;	Disabled throwing application Exception
            }
            catch (Exception ex)
            {
                switch (F2FEnvironment.ToUpper())
                {
                    case "DEVELOPMENT":
                    case "TESTING":
                        if (bThrow) throw ex;
                        break;
                }
            }
            return "Something has gone wrong. Please contact the technical support team" + (iLogID==0 ? "" : " with error code: " + iLogID.ToString());
        }
        private static void WriteLogToFile(Exception Ex, string strPage = null, string strFuncton = null, string strAdditionalInfo = null)
        {
            string sLogFile = AppDomain.CurrentDomain.BaseDirectory + "\\";
            sLogFile += (ConfigurationManager.AppSettings.AllKeys.Contains("ErrorLogFileWithPath") ? ConfigurationManager.AppSettings["ErrorLogFileWithPath"].ToString() : "ErrorLog.txt");
            string strLogMessage = Environment.NewLine + DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss ");
            strLogMessage += (string.IsNullOrEmpty(strPage) ? "" : "File: " + strPage + " ");
            strLogMessage += (string.IsNullOrEmpty(strFuncton) ? "" : "Function: " + strFuncton + " ");
            strLogMessage += "Message: " + Ex.Message + " ";
            strLogMessage += "Stack Trace: " + Ex.StackTrace + " ";
            strLogMessage += (string.IsNullOrEmpty(strAdditionalInfo) ? "" : "Additional Information: " + strAdditionalInfo + " ");
            File.AppendAllText(sLogFile, strLogMessage);
        }
        private static int WriteLogToDatabase(Exception Ex, string strPage = null, string strFuncton = null, string strAdditionalInfo = null)
        {
            try
            {
                using (F2FDatabase DB = new F2FDatabase())
                {
                    string sInsertSQL = "INSERT INTO TBL_F2F_LOG (FL_TIMESTAMP, FL_FILENAME, FL_FUNCTION, FL_MESSAGE, FL_INNER_MESSAGE, FL_STACK_TRACE, FL_ADDITIONAL_INFO) VALUES (@Timestamp, @Filename, @Function, @Message, @InnerMessage, @StackTrace, @AdditionalInfo );";
                    string sLastIDSQL;
                    if (Global.AppDbType == "MsSQL") { sLastIDSQL = "SELECT SCOPE_IDENTITY();"; } //.AppDbType Property from Global.cs to choose Connection
                    else { sLastIDSQL = "SELECT LAST_INSERT_ID();"; }
                    DB.F2FCommand.CommandText = sInsertSQL + sLastIDSQL;
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Timestamp", F2FDatabase.F2FDbType.DateTime, CommonCodes.dispToDbDateTime(DateTime.Now)));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Filename", F2FDatabase.F2FDbType.VarChar, strPage == null ? string.Empty : strPage));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Function", F2FDatabase.F2FDbType.VarChar, strFuncton == null ? string.Empty : strFuncton));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@Message", F2FDatabase.F2FDbType.VarChar, Ex.Message == null ? string.Empty : Ex.Message));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@InnerMessage", F2FDatabase.F2FDbType.VarChar, Ex.InnerException == null ? string.Empty : Ex.InnerException.Message));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@StackTrace", F2FDatabase.F2FDbType.VarChar, Ex.StackTrace == null ? string.Empty : Ex.StackTrace));
                    DB.F2FCommand.Parameters.Add(F2FDatabase.F2FParameter("@AdditionalInfo", F2FDatabase.F2FDbType.VarChar, strAdditionalInfo == null ? string.Empty : strAdditionalInfo));
                    DB.OpenConnection();
                    var retVal = DB.F2FCommand.ExecuteScalar();
                    return retVal == DBNull.Value ? 0 : Convert.ToInt32(retVal.ToString());
                }
            }
            catch (Exception dbLogException)
            {
                throw dbLogException;
            }
        }
    }
}