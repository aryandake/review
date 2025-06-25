using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Data.SqlClient;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.App_Code;

/// <summary>
/// Summary description for Fiction2Fact.CommonCode
/// </summary>
namespace Fiction2Fact.Legacy_App_Code
{
    public class CommonCode
    {
        string strTableCellCSS = " style=\"font-size: 12px; font-family: Calibri; border-width: " +
                   "1px; padding: 8px; border-style: solid;" +
                   "border-color: #bcaf91;\"";

        string strTableHeaderCSS = " style =\"font-size: 12px; font-family: Calibri; " +
                        "background-color: #ded0b0; border-width: 1px; padding: 8px;" +
                        "border-style: solid; border-color: #bcaf91; text-align: left;\"";

        string strTableCSS = " style=\"font-size: 12px; font-family: Calibri; color: #333333; " +
                    "width: 100%; border-width: 1px; border-color: #bcaf91; border-collapse: collapse;\"";

        public static string HELP_DESK_STATUS_SUBMITTED = "S";
        public static string HELP_DESK_STATUS_WORK_STARTED = "WS";
        public static string HELP_DESK_STATUS_CLOSED = "C";
        public static string HELP_DESK_STATUS_ALLOCATED = "A";
        public static string HELP_DESK_STATUS_REOPENED = "RO";

        string strRemarks = "";

        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();
        private string strTo = "", strCC = "";

        public Hashtable ParamMap = new Hashtable();
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        MailConfigBLL configBl = new MailConfigBLL();
        UtilitiesBLL utilBl = new UtilitiesBLL();

        public string getMonthNameFromNo(int intMonth)
        {
            string strMonth = "";
            switch (intMonth)
            {
                case 1:
                    strMonth = "Jan";
                    break;
                case 2:
                    strMonth = "Feb";
                    break;
                case 3:
                    strMonth = "Mar";
                    break;
                case 4:
                    strMonth = "Apr";
                    break;
                case 5:
                    strMonth = "May";
                    break;
                case 6:
                    strMonth = "Jun";
                    break;
                case 7:
                    strMonth = "Jul";
                    break;
                case 8:
                    strMonth = "Aug";
                    break;
                case 9:
                    strMonth = "Sep";
                    break;
                case 10:
                    strMonth = "Oct";
                    break;
                case 11:
                    strMonth = "Nov";
                    break;
                case 12:
                    strMonth = "Dec";
                    break;

            }
            return strMonth;

        }

        public string getCurrentFinancialYear()
        {
            string strFinYear = "";
            string strCurrDate = System.DateTime.Now.ToString("dd-MMM-yyyy");
            string strCurrYear = System.DateTime.Now.ToString("yyyy");
            string strCurrMonth = System.DateTime.Now.ToString("MMM");
            if (strCurrMonth.Equals("Jan") || strCurrMonth.Equals("Feb") || strCurrMonth.Equals("Mar"))
            {
                strFinYear = (Convert.ToInt32(strCurrYear.Substring(2, 2)) - 1) + "-" + strCurrYear.Substring(2, 2);
            }
            else
            {
                strFinYear = strCurrYear.Substring(2, 2) + "-" + (Convert.ToInt32(strCurrYear.Substring(2, 2)) + 1);
            }
            return strFinYear;
        }

        public string[] RemoveDuplicates(string[] inputArray)
        {
            int length = inputArray.Length;
            for (int i = 0; i < length; i++)
            {
                for (int j = (i + 1); j < length;)
                {
                    if (inputArray[i] == inputArray[j])
                    {
                        for (int k = j; k < length - 1; k++)
                            inputArray[k] = inputArray[k + 1];
                        length--;
                    }
                    else
                        j++;
                }
            }

            string[] distinctArray = new string[length];
            for (int i = 0; i < length; i++)
                distinctArray[i] = inputArray[i];

            return distinctArray;
        }

        //Added By Supriya on 12-Jun-2017
        public string getFileNameErrors(string strClientFileName)
        {
            string strReturmMsg = "";

            if (strClientFileName.Length > Global.AppSettings.FullyQualifiedFileNameLength)
            {
                strReturmMsg += "File Name length exceeds permissible length.";
            }
            if (strClientFileName.Contains("&"))
            {
                strReturmMsg += " File Name can't have special character '&'.";
            }
            if (strClientFileName.Contains("#"))
            {
                strReturmMsg += " File Name Shall not have special character '#'.";
            }
            if (strClientFileName.Contains("\'"))
            {
                strReturmMsg += " File Name can't have special character '.";
            }
            if (strClientFileName.Contains("!"))
            {
                strReturmMsg += " File Name can't have special character !.";
            }
            if (strClientFileName.Contains("@"))
            {
                strReturmMsg += " File Name can't have special character @.";
            }
            //if (strClientFileName.Contains(","))
            //{
            //    strReturmMsg += " File Name contains ' , ' which is not allowed.";
            //}

            return strReturmMsg;
        }

        public string getConStringForImportExcelFile(string strCompleteFileName, string strSheetName, string strFileExtension)
        {
            string strMachineConfiguration = "", conString = "";
            strMachineConfiguration = (ConfigurationManager.AppSettings["MachineConfiguration"].ToString());

            if (strMachineConfiguration.Equals("32bit"))
            {
                if (strFileExtension.Equals(".xls"))
                {
                    conString = "('Microsoft.JET.OLEDB.4.0', " +
                       "'" + "Excel 8.0;Database=" + strCompleteFileName + ";HDR=YES;IMEX=1;ImportMixedTypes=Text'" +
                       ", [" + strSheetName + "$])";
                }
                else if (strFileExtension.Equals(".xlsx"))
                {
                    conString = "('Microsoft.ACE.OLEDB.12.0', " +
                         "'" + "Excel 12.0;Database=" + strCompleteFileName + ";HDR=YES;IMEX=1;ImportMixedTypes=Text'" +
                         ", [" + strSheetName + "$])";
                }


            }
            else if (strMachineConfiguration.Equals("64bit"))
            {
                conString = " ('Microsoft.ACE.OLEDB.12.0', " +
                        "'" + "Excel 12.0;Database=" + strCompleteFileName + ";HDR=YES;IMEX=1;ImportMixedTypes=Text'" +
                        ", [" + strSheetName + "$])";
            }
            return conString;

        }

        public string getConStringForFetchExcelFileNoColumnHeader(string strCompleteFileName, string strSheetName, string strFileExtension)
        {
            string strMachineConfiguration = "", conString = "";
            strMachineConfiguration = (ConfigurationManager.AppSettings["MachineConfiguration"].ToString());

            if (strMachineConfiguration.Equals("32bit"))
            {
                if (strFileExtension.Equals(".xls"))
                {
                    conString = "Provider=Microsoft.JET.OLEDB.4.0;" +
                     "Data Source=" + strCompleteFileName + ";" +
                     "Extended Properties='Excel 8.0;HDR=YES;IMEX=1;ImportMixedTypes=Text'";
                }
                else if (strFileExtension.Equals(".xlsx"))
                {
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                     "Data Source=" + strCompleteFileName + ";" +
                     "Extended Properties='Excel 12.0;HDR=YES;IMEX=1;ImportMixedTypes=Text'";
                }
            }
            else if (strMachineConfiguration.Equals("64bit"))
            {
                conString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                      "Data Source=" + strCompleteFileName + ";" +
                      "Extended Properties='Excel 12.0;HDR=YES;IMEX=1;ImportMixedTypes=Text'";
            }
            return conString;
        }

        public string getConStringForFetchExcelFile(string strCompleteFileName, string strSheetName, string strFileExtension)
        {
            string strMachineConfiguration = "", conString = "";
            strMachineConfiguration = (ConfigurationManager.AppSettings["MachineConfiguration"].ToString());

            if (strMachineConfiguration.Equals("32bit"))
            {
                if (strFileExtension.Equals(".xls"))
                {
                    conString = "Provider=Microsoft.JET.OLEDB.4.0;" +
                     "Data Source=" + strCompleteFileName + ";" +
                     "Extended Properties='Excel 8.0;HDR=NO;IMEX=1;ImportMixedTypes=Text'";
                }
                else if (strFileExtension.Equals(".xlsx"))
                {
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                     "Data Source=" + strCompleteFileName + ";" +
                     "Extended Properties='Excel 12.0;HDR=NO;IMEX=1;ImportMixedTypes=Text'";
                }
            }
            else if (strMachineConfiguration.Equals("64bit"))
            {
                conString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                      "Data Source=" + strCompleteFileName + ";" +
                      "Extended Properties='Excel 12.0;HDR=NO;IMEX=1;ImportMixedTypes=Text'";
            }
            return conString;
        }

        public static string testFun()
        {
            return "<tr>" +
                    "<td class='tabhead3'>Test</td>" +
                    "<td class='tabbody3'>Test</td>" +
                    "</tr>";
        }

        public static string dispToDbDate(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(str).ToString("yyyy-MM-dd");
            }
        }

        public static string dispToDbDate(object obj)
        {
            if (obj == null)
            {

                return null;
            }
            else
            {
                return Convert.ToDateTime(obj).ToString("yyyy-MM-dd");
            }
        }

        public static string dispToDbDateTime(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(str).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public static string dispToDbDateTime(object obj)
        {
            if (obj == null || obj.ToString() == "")
            {

                return null;
            }
            else
            {
                return Convert.ToDateTime(obj).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public static string DbToDispDate(object obj)
        {
            if (obj == DBNull.Value || obj.Equals(""))
            {

                return null;
            }
            else
            {
                return Convert.ToDateTime(obj).ToString("dd-MMM-yyyy");
            }
        }

        public static string DbToDispDateTime(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(str).ToString("dd-MMM-yyyy HH:mm:ss");
            }
        }

        public static string DbToDispDateTime(object obj)
        {
            if (obj == DBNull.Value)
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(obj).ToString("dd-MMM-yyyy HH:mm:ss");
            }
        }

        public void setCertificationMailContent()
        {
            DataTable UpdatesFeedbackDT = new DataTable();
            DataTable dtRC = new DataTable();
            DataTable dtTo = new DataTable();
            DataTable dtTo1 = new DataTable();
            DataTable dtCC = new DataTable();
            DataTable dtCIMDets = new DataTable();
            DataTable dtMail = new DataTable();
            DataTable dtUnit = new DataTable();
            DataTable dtIAPDets = new DataTable();
            DataRow drMail;
            string strSubject = "", strContent = "", strToType = "", strCCType = "";
            string[] strToList = null, strCCList = null;// strBCCList = null;
            string[] strToTempList = new string[1];
            string strCreatedOn = "", strConfigID = "", strUnitHeadName = "", strCXOName = "", strCertIds = "";
            string strCertDepartmentId = "", strCertDepartment = "", strQuarterEndDt = "", strSubmittedBy = "",
                strQuarter = "", strFunctionId = "", strUnitID = "", strDesg = "", strDepartmentIdType = "", strFooter = "";

            try
            {
                strCreatedOn = System.DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");

                if (ParamMap.ContainsKey("ConfigId"))
                {
                    strConfigID = ParamMap["ConfigId"].ToString();

                    dtMail = configBl.searchMailConfig(strConfigID, "", "", mstrConnectionString);
                    if (dtMail.Rows.Count > 0)
                    {
                        drMail = dtMail.Rows[0];
                        strSubject = drMail["MCM_SUBJECT"].ToString();
                        strContent = "<div style=\"font-family: Zurich bt; font-size: 11pt\">" + drMail["MCM_CONTENT"].ToString() + "</div>";
                        strToType = ParamMap["To"].ToString();
                        strCCType = ParamMap["cc"].ToString();

                        if (ParamMap["CertDepartmentId"] != null)
                        {
                            strCertDepartmentId = ParamMap["CertDepartmentId"].ToString();
                        }

                        if (ParamMap["CertDepartmentIdType"] != null)
                        {
                            strDepartmentIdType = ParamMap["CertDepartmentIdType"].ToString();
                        }

                        if (ParamMap["CertDepartment"] != null)
                        {
                            strCertDepartment = ParamMap["CertDepartment"].ToString();
                        }
                        if (ParamMap["QuarterEndDt"] != null)
                        {
                            strQuarterEndDt = ParamMap["QuarterEndDt"].ToString();
                        }
                        if (ParamMap["SubmittedBy"] != null)
                        {
                            strSubmittedBy = ParamMap["SubmittedBy"].ToString();
                        }
                        if (ParamMap["Quarter"] != null)
                        {
                            strQuarter = ParamMap["Quarter"].ToString();
                        }
                        if (ParamMap["CertIds"] != null)
                        {
                            strCertIds = ParamMap["CertIds"].ToString();
                        }
                        if (ParamMap["CXOName"] != null)
                        {
                            strCXOName = ParamMap["CXOName"].ToString();
                        }
                        if (ParamMap["UnitHeadName"] != null)
                        {
                            strUnitHeadName = ParamMap["UnitHeadName"].ToString();
                        }
                        if (ParamMap["UnitID"] != null)
                        {
                            strUnitID = ParamMap["UnitID"].ToString();
                        }
                        if (ParamMap["FunctionId"] != null)
                        {
                            strFunctionId = ParamMap["FunctionId"].ToString();
                        }
                        if (ParamMap["DESG"] != null)
                        {
                            strDesg = ParamMap["DESG"].ToString();
                        }

                        strFooter = ConfigurationManager.AppSettings["MailFooter"].ToString();
                        strContent = strContent.Replace("%Footer%", strFooter);

                        //Certificate Submitted by Level 1
                        if (strConfigID.Equals("10") || strConfigID.Equals("11") || strConfigID.Equals("12") ||
                            strConfigID.Equals("1090") || strConfigID.Equals("2098"))
                        {
                            strSubject = strSubject.Replace("%CertDepartment%", strCertDepartment);
                            strSubject = strSubject.Replace("%Quarter%", strQuarter);
                            strSubject = strSubject.Replace("%SubmittedBy%", strSubmittedBy);

                            strContent = strContent.Replace("%CertDepartment%", strCertDepartment);
                            strContent = strContent.Replace("%Quarter%", strQuarter);
                            strContent = strContent.Replace("%SubmittedBy%", strSubmittedBy);
                            strContent = strContent.Replace("%Date%", strCreatedOn);
                            strContent = strContent.Replace("%CXO%", strCXOName);

                            if (strConfigID.Equals("10"))
                            {
                                strContent = strContent.Replace("%User%", strUnitHeadName);
                                strContent = strContent.Replace("%Link%",
                                  "<a href='" + Global.site_url("Projects/Certification/CertificationApproval.aspx?Type=L2") + "'>Click here</a>");

                                strContent = strContent.Replace("%Table%", getHTMLTable2(strCertIds, "CertApproval", "L1", strUnitID));
                            }
                            else if (strConfigID.Equals("11"))
                            {
                                strContent = strContent.Replace("%User%", strCXOName);
                                strContent = strContent.Replace("%Link%",
                                  "<a href='" + Global.site_url("Projects/Certification/CertificationCXOApproval.aspx?Type=L3") + "'>Click here</a>");

                                strContent = strContent.Replace("%Table%", getHTMLTable2(strCertIds, "CertApproval", "L2", strCertDepartmentId));
                            }
                            else if (strConfigID.Equals("12"))
                            {
                                strContent = strContent.Replace("%Link%",
                                  "<a href='" + Global.site_url("Projects/Certification/CertificationCUApproval.aspx?Type=L4") + "'>Click here</a>");
                                //<<Modified by Ankur Tyagi on 15-May-2025 for Project Id : 2395
                                //L4 to L5
                                //strContent = strContent.Replace("%Table%", getHTMLTable(strCertIds, "CertApproval", "L5", ""));
                                strContent = strContent.Replace("%Table%", getHTMLTable2(strCertIds, "CertApproval", "L2", strCertDepartmentId));
                                //>>
                            }
                            else if (strConfigID.Equals("1090"))
                            {
                                //<<Modified by Ankur Tyagi on 15-May-2025 for Project Id : 2395
                                //Link change
                                strContent = strContent.Replace("%Link%",
                                  "<a href='" + Global.site_url("Projects/Certification/CertificationExCoApproval.aspx?Type=L5") + "'>Click here</a>");

                                strContent = strContent.Replace("%Table%", getHTMLTable(strCertIds, "CertApproval", "L4", ""));
                                //>>
                            }
                            else if (strConfigID.Equals("2098"))
                            {
                                strContent = strContent.Replace("%Link%",
                                  "<a href='" + Global.site_url("Projects/Certification/CommonCertification_CCO.aspx") + "'>Click here</a>");

                                strContent = strContent.Replace("%Table%", getHTMLTable(strCertIds, "CertApproval", "L5", ""));
                            }
                        }

                        else if (strConfigID.Equals("13") || strConfigID.Equals("14") || strConfigID.Equals("1094")
                             || strConfigID.Equals("1097"))
                        {
                            strSubject = strSubject.Replace("%CertDepartment%", strCertDepartment);
                            strSubject = strSubject.Replace("%Quarter%", strQuarter);
                            strSubject = strSubject.Replace("%SubmittedBy%", strSubmittedBy);

                            strContent = strContent.Replace("%Quarter%", strQuarter);
                            strContent = strContent.Replace("%CertDepartment%", strCertDepartment);
                            strContent = strContent.Replace("%SubmittedBy%", strSubmittedBy);
                            strContent = strContent.Replace("%Date%", strCreatedOn);
                            //strContent = strContent.Replace("%User%", strSPOCName);

                            if (strConfigID.Equals("13"))
                            {
                                strContent = strContent.Replace("%Link%",
                                  "<a href='" + Global.site_url("Projects/Certification/Certification.aspx") + "'>Click here</a>");

                                strContent = strContent.Replace("%Table%", getHTMLTable(strCertIds, "CertRejection", "L1", ""));
                            }
                            else if (strConfigID.Equals("14"))
                            {
                                strContent = strContent.Replace("%Link%",
                                  "<a href='" + Global.site_url("Projects/Certification/Certification.aspx") + "'>Click here</a>");

                                strContent = strContent.Replace("%Table%", getHTMLTable(strCertIds, "CertRejection", "L3", ""));
                            }
                            else if (strConfigID.Equals("1094"))
                            {
                                //<<Modified by Ankur Tyagi on 15-May-2025 for Project Id : 2395
                                //Link Change
                                strContent = strContent.Replace("%Link%",
                                  "<a href='" + Global.site_url("Projects/Certification/CertificationCXOApproval.aspx?Type=L3") + "'>Click here</a>");

                                strContent = strContent.Replace("%Table%", getHTMLTable(strCertIds, "CertRejection", "L2", ""));
                                //>>
                            }
                            else if (strConfigID.Equals("1097"))
                            {
                                //<<Modified by Ankur Tyagi on 15-May-2025 for Project Id : 2395
                                //Link Change
                                strContent = strContent.Replace("%Link%",
                                  "<a href='" + Global.site_url("Projects/Certification/CertificationCXOApproval.aspx?Type=L3") + "'>Click here</a>");

                                strContent = strContent.Replace("%Table%", getHTMLTable(strCertIds, "CertRejection", "L4", ""));
                                //>>
                            }
                        }
                        else if (strConfigID.Equals("15"))
                        {
                            strSubject = strSubject.Replace("%Quarter%", strQuarter);
                            strContent = strContent.Replace("%Quarter%", strQuarter);

                            strContent = strContent.Replace("%Link%",
                                  "<a href='" + Global.site_url("Projects/Certification/Certification.aspx") + "'>Click here</a>");
                        }

                        else if (strConfigID.Equals("16"))
                        {
                            strSubject = strSubject.Replace("%Quarter%", strQuarter);
                            strContent = strContent.Replace("%Quarter%", strQuarter);

                            strContent = strContent.Replace("%Link%",
                                  "<a href='" + Global.site_url("Projects/Certification/Certification.aspx") + "'>Click here</a>");
                        }

                        else if (strConfigID.Equals("18"))
                        {
                            strSubject = strSubject.Replace("%Quarter%", strQuarter);
                            strContent = strContent.Replace("%Quarter%", strQuarter);

                            strContent = strContent.Replace("%Link%",
                                  "<a href='" + Global.site_url("Projects/Certification/CommonCertification.aspx") + "'>Click here</a>");
                        }

                        else if (strConfigID.Equals("19") || strConfigID.Equals("20") || strConfigID.Equals("22"))
                        {
                            strSubject = strSubject.Replace("%QuarterendDate%", strQuarter);
                            strSubject = strSubject.Replace("%Desg%", strDesg);

                            strContent = strContent.Replace("%Quarter%", strQuarter);
                            strContent = strContent.Replace("%SubmittedBy%", strSubmittedBy);
                            strContent = strContent.Replace("%Desg%", strDesg);
                            strContent = strContent.Replace("%currDate%", DateTime.Now.ToString("dd-MMM-yyyy"));

                            if (strConfigID.Equals("19"))
                            {
                                strContent = strContent.Replace("%Link%",
                                      "<a href='" + Global.site_url("Projects/Certification/CommonCertification.aspx") + "'>Click here</a>");
                            }
                            else if (strConfigID.Equals("20"))
                            {
                                strContent = strContent.Replace("%Link%",
                                      "<a href='" + Global.site_url("Projects/Certification/ViewCommonCertifications.aspx?Type=2") + "'>Click here</a>");
                            }
                            else if (strConfigID.Equals("22"))
                            {
                                strContent = strContent.Replace("%Link%",
                                      "<a href='" + Global.site_url("Projects/Certification/ViewCommonCertifications.aspx?Type=1") + "'>Click here</a>");
                            }
                        }
                        //<<Added by Rahuldeb on 26Sep2019
                        else if (strConfigID.Equals("21"))
                        {
                            strSubject = strSubject.Replace("%Quarter%", strQuarter);
                            strContent = strContent.Replace("%Quarter%", strQuarter);

                            strContent = strContent.Replace("%Link%",
                                  "<a href='" + Global.site_url("Projects/Certification/CommonCertification_CCO.aspx") + "'>Click here</a>");
                        }

                        //>>

                        setMailIds("TO", strToType, strCertDepartmentId, strDepartmentIdType);
                        setMailIds("CC", strCCType, strCertDepartmentId, strDepartmentIdType);

                        if (!strTo.Equals(""))
                        {
                            strTo = strTo.ToString().Replace("||", "|");
                            strTo = strTo.Substring(0, strTo.Length - 1);
                            strToList = strTo.Split('|');
                        }
                        if (!strCC.Equals(""))
                        {
                            strCC = strCC.ToString().Replace("||", "|");
                            strCC = strCC.Substring(0, strCC.Length - 1);
                            strCCList = strCC.Split('|');
                        }
                        //>>                

                        string[] distinctToArray = null;
                        string[] distinctCCArray = null;

                        if (strToList != null)
                        {
                            if (strToList.Length > 0)
                            {
                                distinctToArray = RemoveDuplicates(strToList);
                            }
                        }

                        if (strCCList != null)
                        {
                            if (strCCList.Length > 0)
                            {
                                distinctCCArray = RemoveDuplicates(strCCList);
                            }
                        }

                        Mail mm = new Mail();
                        mm.sendAsyncMail(distinctToArray, distinctCCArray, null, strSubject, strContent);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string getHTMLTable(string strCertIds, string strType, string strUsersRole, string strCertDepartmentId)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            string strSql = "";

            string strHTML = "<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + " width='5%'>Sr.No.</th>" +
                               "<th " + strTableHeaderCSS + " width='10%'>Department Name</th>" +
                               "<th " + strTableHeaderCSS + " width='10%'>Quarter</th>";

            if (strType.Equals("CertRejection"))
            {
                strHTML = strHTML + "<th " + strTableHeaderCSS + " width='75%'>Revision Suggested</th>";
            }
            else if (strType.Equals("CertApproval"))
            {
                //<<Modified by Ankur Tyagi on 15-May-2025 for Project Id : 2395
                // Added L4 and L5
                //<< Added by Archana Gosavi on 31-Mar-2017
                if (strUsersRole.Equals("L1") || strUsersRole.Equals("L2") || strUsersRole.Equals("L3") || strUsersRole.Equals("L4") || strUsersRole.Equals("L5"))
                //>>
                {
                    strHTML = strHTML + "<th " + strTableHeaderCSS + " width='75%'>Remarks</th>";
                }
                //>>
            }

            strHTML = strHTML + "</tr>";

            //strSql = " select CSSDM_NAME as DeptName, "+
            //    " REPLACE(CONVERT(VARCHAR, CQM_FROM_DATE, 106), ' ', '-')  +' to ' + REPLACE(CONVERT(VARCHAR, CQM_TO_DATE, 106), ' ', '-') as Quarter "+
            //    " , * from TBL_CERTIFICATIONS " +
            //    " inner join TBL_CERT_SUB_SUB_DEPT_MAS on CERT_CSSDM_ID = CSSDM_ID "+
            //    " inner join TBL_CERT_QUARTER_MAS ON CERT_CQM_ID=CQM_ID  and CQM_STATUS = 'A' "+
            //    " where 1=1 "; 

            strSql = " select CSSDM_NAME as DeptName, " +
               " REPLACE(CONVERT(VARCHAR, CQM_FROM_DATE, 106), ' ', '-')  +' to ' + REPLACE(CONVERT(VARCHAR, CQM_TO_DATE, 106), ' ', '-') as Quarter " +
               " , * from TBL_CERTIFICATIONS " +
               " inner join TBL_CERT_MAS on CERT_CERTM_ID = CERTM_ID " +
               " inner join TBL_CERT_SUB_SUB_DEPT_MAS on CERTM_DEPT_ID = CSSDM_ID and CERTM_LEVEL_ID = 0 " +
               " inner join TBL_CERT_QUARTER_MAS ON CERT_CQM_ID=CQM_ID  and CQM_STATUS = 'A' " +
               " where 1=1 ";

            if (!strCertIds.Equals(""))
                strSql = strSql + " and CERT_ID in (" + strCertIds + ")";

            if (!strCertDepartmentId.Equals(""))
                strSql = strSql + " and CSSDM_ID in (" + strCertDepartmentId + ")";

            if (!strSql.Equals(""))
            {
                using (F2FDatabase DB = new F2FDatabase(strSql))
                    DB.F2FDataAdapter.Fill(dt);
            }

            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                dr = dt.Rows[i - 1];
                strHTML = strHTML + "<tr><td " + strTableCellCSS + ">" + i + "</td>" +
                                    "<td " + strTableCellCSS + ">" + dr["DeptName"].ToString() + "</td>" +
                                    "<td " + strTableCellCSS + ">" + dr["Quarter"].ToString() + "</td>";

                if (strType.Equals("CertRejection"))
                {
                    if (strUsersRole.Equals("L1"))
                    {
                        strHTML = strHTML + "<td " + strTableCellCSS + ">" + dr["CERT_REJECTED_REMARKS_LEVEL1"].ToString().Replace("\n", "<br />") + "</td>";
                    }
                    else if (strUsersRole.Equals("L2"))
                    {
                        strHTML = strHTML + "<td " + strTableCellCSS + ">" + dr["CERT_REJECTED_REMARKS_LEVEL2"].ToString().Replace("\n", "<br />") + "</td>";
                    }
                    else if (strUsersRole.Equals("L3"))
                    {
                        strHTML = strHTML + "<td " + strTableCellCSS + ">" + dr["CERT_REJECTED_REMARKS_LEVEL3"].ToString().Replace("\n", "<br />") + "</td>";
                    }
                    //<<Added by Ankur Tyagi on 15-May-2025 for Project Id : 2395
                    else if (strUsersRole.Equals("L4"))
                    {
                        strHTML = strHTML + "<td " + strTableCellCSS + ">" + dr["CERT_REJECTED_REMARKS_LEVEL4"].ToString().Replace("\n", "<br />") + "</td>";
                    }
                    //>>
                }
                else if (strType.Equals("CertApproval"))
                {
                    if (strUsersRole.Equals("L1"))
                    {
                        strHTML = strHTML + "<td " + strTableCellCSS + ">" + dr["CERT_SUBMITTED_REMARKS"].ToString().Replace("\n", "<br />") + "</td>";
                    }
                    else if (strUsersRole.Equals("L2"))
                    {
                        strHTML = strHTML + "<td " + strTableCellCSS + ">" + dr["CERT_APPROVED_REMARKS_LEVEL1"].ToString().Replace("\n", "<br />") + "</td>";
                    }
                    else if (strUsersRole.Equals("L3"))
                    {
                        strHTML = strHTML + "<td " + strTableCellCSS + ">" + dr["CERT_APPROVED_REMARKS_LEVEL2"].ToString().Replace("\n", "<br />") + "</td>";
                    }
                    //<<Added by Ankur Tyagi on 15-May-2025 for Project Id : 2395
                    else if (strUsersRole.Equals("L4"))
                    {
                        strHTML = strHTML + "<td " + strTableCellCSS + ">" + dr["CERT_APPROVED_REMARKS_LEVEL3"].ToString().Replace("\n", "<br />") + "</td>";
                    }
                    else if (strUsersRole.Equals("L5"))
                    {
                        strHTML = strHTML + "<td " + strTableCellCSS + ">" + dr["CERT_APPROVED_REMARKS_LEVEL4"].ToString().Replace("\n", "<br />") + "</td>";
                    }
                    //>>
                }

                strHTML = strHTML + "</tr>";
            }

            strHTML = strHTML + "</table>";
            return strHTML;
        }

        private string getHTMLTable2(string strCertIds, string strType, string strUsersRole, string strCertDepartmentId)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            string strSql = "";

            string strHTML = "<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + " width='5%'>Sr.No.</th>" +
                               "<th " + strTableHeaderCSS + " width='10%'>Department Name</th>" +
                               "<th " + strTableHeaderCSS + " width='10%'>Quarter</th>";

            if (strType.Equals("CertRejection"))
            {
                strHTML = strHTML + "<th " + strTableHeaderCSS + " width='75%'>Revision Suggested</th>";
            }
            else if (strType.Equals("CertApproval"))
            {
                //<< Added by Archana Gosavi on 31-Mar-2017
                if (strUsersRole.Equals("L1"))
                {
                    strHTML = strHTML + "<th " + strTableHeaderCSS + " width='75%'>Remarks</th>";
                }
                else if (strUsersRole.Equals("L2"))
                {
                    strHTML = strHTML + "<th " + strTableHeaderCSS + " width='75%'>Remarks</th>";
                }
                //>>
            }

            strHTML = strHTML + "</tr>";
            //strSql = " select CSSDM_NAME as DeptName, "+
            //    " REPLACE(CONVERT(VARCHAR, CQM_FROM_DATE, 106), ' ', '-')  +' to ' + REPLACE(CONVERT(VARCHAR, CQM_TO_DATE, 106), ' ', '-') as Quarter "+
            //    " , * from TBL_CERTIFICATIONS " +
            //    " inner join TBL_CERT_SUB_SUB_DEPT_MAS on CERT_CSSDM_ID = CSSDM_ID "+
            //    " inner join TBL_CERT_QUARTER_MAS ON CERT_CQM_ID=CQM_ID  and CQM_STATUS = 'A' "+
            //    " where 1=1 "; 

            strSql = " select CSSDM_NAME as DeptName, " +
               " REPLACE(CONVERT(VARCHAR, CQM_FROM_DATE, 106), ' ', '-')  +' to ' + REPLACE(CONVERT(VARCHAR, CQM_TO_DATE, 106), ' ', '-') as Quarter " +
               " , * from TBL_CERTIFICATIONS " +
               " inner join TBL_CERT_MAS on CERT_CERTM_ID = CERTM_ID " +
               " inner join TBL_CERT_SUB_SUB_DEPT_MAS on CERTM_DEPT_ID = CSSDM_ID and CERTM_LEVEL_ID = 0 " +
               " inner join TBL_CERT_SUB_DEPT_MAS on CSDM_ID = CSSDM_CSDM_ID " +
               " inner join TBL_CERT_DEPT_MAS on CDM_ID = CSDM_CDM_ID " +
               " inner join TBL_CERT_QUARTER_MAS ON CERT_CQM_ID=CQM_ID  and CQM_STATUS = 'A' " +
               " where 1=1 ";

            if (strUsersRole.Equals("L1"))
            {
                strSql = strSql + " and CSDM_ID = " + strCertDepartmentId + "";
            }
            else if (strUsersRole.Equals("L2"))
            {
                strSql = strSql + " and CDM_ID = " + strCertDepartmentId + "";
            }

            if (!strSql.Equals(""))
            {
                using (F2FDatabase DB = new F2FDatabase(strSql))
                {
                    DB.F2FDataAdapter.Fill(dt);
                }
            }

            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                dr = dt.Rows[i - 1];
                strHTML = strHTML + "<tr><td " + strTableCellCSS + ">" + i + "</td>" +
                                    "<td " + strTableCellCSS + ">" + dr["DeptName"].ToString() + "</td>" +
                                    "<td " + strTableCellCSS + ">" + dr["Quarter"].ToString() + "</td>";

                if (strType.Equals("CertRejection"))
                {
                    if (strUsersRole.Equals("L1"))
                    {
                        strHTML = strHTML + "<td " + strTableCellCSS + ">" + dr["CERT_REJECTED_REMARKS_LEVEL1"].ToString().Replace("\n", "<br />") + "</td>";
                    }
                    else if (strUsersRole.Equals("L2"))
                    {
                        strHTML = strHTML + "<td " + strTableCellCSS + ">" + dr["CERT_REJECTED_REMARKS_LEVEL2"].ToString().Replace("\n", "<br />") + "</td>";
                    }
                }
                else if (strType.Equals("CertApproval"))
                {
                    if (strUsersRole.Equals("L1"))
                    {
                        strHTML = strHTML + "<td " + strTableCellCSS + ">" + dr["CERT_SUBMITTED_REMARKS"].ToString().Replace("\n", "<br />") + "</td>";
                    }
                    else if (strUsersRole.Equals("L2"))
                    {
                        strHTML = strHTML + "<td " + strTableCellCSS + ">" + dr["CERT_APPROVED_REMARKS_LEVEL1"].ToString().Replace("\n", "<br />") + "</td>";
                    }
                    else if (strUsersRole.Equals("L3"))
                    {
                        strHTML = strHTML + "<td " + strTableCellCSS + ">" + dr["CERT_APPROVED_REMARKS_LEVEL2"].ToString().Replace("\n", "<br />") + "</td>";
                    }
                }

                strHTML = strHTML + "</tr>";
            }

            strHTML = strHTML + "</table>";
            return strHTML;
        }

        private void setMailIds(string strToCCType, string strType, string strCertDepartmentId, string strDepartmentIdType)
        {
            string[] arrType = null;
            string strReturn = "", strUserType = "";
            arrType = strType.Split(',');

            if (arrType.Length > 0)
            {
                for (int i = 0; i < arrType.Length; i++)
                {
                    strUserType = arrType[i];
                    switch (strUserType)
                    {
                        case "ProvidedAsParam":
                            strReturn += ParamMap["EmailIds"].ToString();
                            strReturn = strReturn.Replace(",", "|") + "|";
                            break;

                        case "Comp":
                            strReturn += getUserDetails("Certification_Compliance_User", "EmailId");
                            break;

                        case "ComplianceUser":
                            strReturn += getUserDetails("Certification_Compliance_User", "EmailId");
                            break;

                        case "CFO":
                            strReturn += getUserDetails("Certification_CFO_User", "EmailId");
                            break;

                        case "CEO":
                            strReturn += getUserDetails("Certification_CEO_User", "EmailId");
                            break;

                        case "CCO":
                            strReturn += getUserDetails("Certification_CO_User", "EmailId");
                            break;

                        case "CertAdmin":
                            strReturn += getRequiredDetails("CertDeptCertAdmin", strCertDepartmentId, "CertAdmin_Email", "");
                            break;

                        case "Level1":
                            strReturn += getRequiredDetails("CertDeptLevel1_EmailIdByIds", strCertDepartmentId, "Level1_Email", strDepartmentIdType);
                            break;

                        case "Level2":
                            strReturn += getRequiredDetails("CertDeptLevel2_EmailIdByIds", strCertDepartmentId, "Level2_Email", strDepartmentIdType);
                            break;

                        case "Level3":
                            strReturn += getRequiredDetails("CertDeptLevel3_EmailIdByIds", strCertDepartmentId, "Level3_Email", strDepartmentIdType);
                            break;

                        case "EC":
                            strReturn += getRequiredDetails("CertDeptLevel3_EmailIdByIds", strCertDepartmentId, "EC_Email", strDepartmentIdType);
                            break;
                    }
                }
            }

            if (strToCCType.Equals("TO"))
                strTo = strReturn;
            else if (strToCCType.Equals("CC"))
                strCC = strReturn;
        }

        private string getUserDetails(string strRoleName, string strDetailType)
        {
            string[] strarr;
            string strReturnString = "", strNames = "";
            string[] strUsers = Roles.GetUsersInRole(strRoleName);
            Authentication auth = new Authentication();

            for (int i = 0; i <= strUsers.GetUpperBound(0); i++)
            {
                MembershipUser membershipUser = Membership.GetUser(strUsers[i]);

                if (strDetailType.Equals("EmailId"))
                    strReturnString = strReturnString + membershipUser.Email + "|";
                else if (strDetailType.Equals("Name"))
                    strReturnString = strReturnString + membershipUser.UserName + ",";
            }

            if (strDetailType.Equals("Name"))
            {
                if (!strReturnString.Equals(""))
                {
                    strReturnString = strReturnString.Substring(0, strReturnString.Length - 1);

                    strarr = strReturnString.Split(',');

                    for (int i = 0; i <= strarr.Length - 1; i++)
                    {
                        strNames = strNames + (auth.GetUserDetsByEmpCode(strarr[i]).Split('|')[0]) + ",";
                    }
                }
                return strNames;
            }
            else
            {
                return strReturnString;
            }
        }

        private string getRequiredDetails(string strQueryType, string strFilter, string strOutputType, string strDepartmentIdType)
        {
            string strReturnString = "";
            try
            {
                DataTable dt = new DataTable();
                DataRow dr;
                string strSql = "", strColumnName = "";
                string strSplitCondition = "";

                if (strQueryType.Equals("CertDeptLevel1_EmailIdByIds") || strQueryType.Equals("CertDeptLevel2_EmailIdByIds") ||
                    strQueryType.Equals("CertDeptLevel3_EmailIdByIds"))
                {
                    if (strQueryType.Equals("CertDeptLevel1_EmailIdByIds"))
                        strSql = " SELECT TBL_CERT_SUB_SUB_DEPT_MAS.* ";
                    else if (strQueryType.Equals("CertDeptLevel2_EmailIdByIds"))
                        strSql = " SELECT DISTINCT TBL_CERT_SUB_DEPT_MAS.* ";
                    else if (strQueryType.Equals("CertDeptLevel3_EmailIdByIds"))
                        strSql = " SELECT DISTINCT TBL_CERT_DEPT_MAS.* ";

                    strSql += " FROM TBL_CERT_SUB_SUB_DEPT_MAS " +
                            " INNER JOIN TBL_CERT_SUB_DEPT_MAS ON CSSDM_CSDM_ID = CSDM_ID " +
                            " INNER JOIN TBL_CERT_DEPT_MAS ON CSDM_CDM_ID = CDM_ID " +
                            " WHERE 1 = 1 ";

                    if (!strFilter.Equals(""))
                    {
                        if (strDepartmentIdType.Equals("SPOC"))
                        {
                            strSql = strSql + " and CSSDM_ID in (" + strFilter + ")";
                        }
                        else if (strDepartmentIdType.Equals("UH"))
                        {
                            strSql = strSql + " and CSDM_ID in (" + strFilter + ")";
                        }
                        else if (strDepartmentIdType.Equals("FH"))
                        {
                            strSql = strSql + " and CDM_ID in (" + strFilter + ")";
                        }
                    }
                }
                else if (strQueryType.Equals("CertDeptCertAdmin"))
                {
                    strSql = " select * from aspnet_Users inner join aspnet_Membership " +
                            " on aspnet_Membership.UserId=aspnet_Users.UserId " +
                            " inner join aspnet_UsersInRoles on aspnet_Users.UserId= aspnet_UsersInRoles.UserId " +
                            " inner join aspnet_Roles on aspnet_UsersInRoles.RoleId=aspnet_Roles.RoleId " +
                            " and RoleName = 'Certification_Admin'";
                }

                if (strOutputType.Equals("CertAdmin_Email"))
                {
                    strColumnName = "Email";
                    strSplitCondition = "|";
                }
                //<<

                //<<Get the column name from which the data is to be fetched.
                if (strOutputType.Equals("Level1_Email"))
                {
                    strColumnName = "CSSDM_EMAIL_ID";
                    strSplitCondition = "|";
                }
                else if (strOutputType.Equals("Level2_Email"))
                {
                    strColumnName = "CSDM_EMAIL_ID";
                    strSplitCondition = "|";
                }
                else if (strOutputType.Equals("Level3_Email"))
                {
                    strColumnName = "CDM_CXO_EMAILID";
                    strSplitCondition = "|";
                }
                else if (strOutputType.Equals("EC_Email"))
                {
                    strColumnName = "CDM_EC_EMAILID";
                    strSplitCondition = "|";
                }
                //>>

                //<< Loop the data-table and get the list based on the column name. 
                if (!strSql.Equals(""))
                {
                    using (F2FDatabase Db = new F2FDatabase(strSql))
                    {
                        Db.F2FDataAdapter.Fill(dt);
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = dt.Rows[i];
                        if (!dr[strColumnName].ToString().Equals(""))
                            strReturnString += dr[strColumnName] + strSplitCondition;
                    }
                }
                //>>
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //closeConnection();
            }
            return strReturnString;
        }
        //<<Added by Ankur Tyagi on 15-May-2025 for Project Id : 2395
        public static string GetCurrentUrlFileName(System.Web.UI.Page page, bool bFullPath = false)
        {
            if (!string.IsNullOrEmpty(page.Request.Url.AbsolutePath) && !bFullPath)
            {
                return page.Request.Url.AbsolutePath.Split('/')[page.Request.Url.AbsolutePath.Split('/').Length - 1];
            }
            return page.Request.Url.AbsolutePath;
        }
        //>>
    }
}
