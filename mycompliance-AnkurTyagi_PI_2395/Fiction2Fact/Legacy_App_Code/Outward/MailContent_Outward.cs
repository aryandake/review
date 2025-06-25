using System;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data.SqlClient;
using Fiction2Fact.Legacy_App_Code.Outward.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using System.Web.UI;

namespace Fiction2Fact.Legacy_App_Code.Outward
{
    public class MailContent_Outward
    {
        MailConfigBLL configBl = new MailConfigBLL();
        OutwardUtilitiesBLL outUtilBLL = new OutwardUtilitiesBLL();
        OutwardBL outBL = new OutwardBL();
        private string strTo = "", strCC = "", strBCC = "";
        public Hashtable ParamMap = new Hashtable();

        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSql"].ConnectionString;
        string strTableCellCSS = " style=\"font-size: 12px; font-family: Calibri; border-width: 1px; padding: 8px; border-style: solid;" +
                            "border-color: #bcaf91;\"";

        string strTableHeaderCSS = " style =\"font-size: 12px; font-family: Calibri; background-color: #ded0b0; border-width: 1px; padding: 8px;" +
                                   "border-style: solid; border-color: #bcaf91; text-align: left;\"";

        string strTableCSS = " style=\"font-size: 12px; font-family: Calibri; color: #333333; " +
                            "width: 100%; border-width: 1px; border-color: #bcaf91; border-collapse: collapse;\"";

        public void setOutwardMailContent(string strRole)
        {
            DataRow drMail;
            DataTable dtMail = new DataTable();
            Calendar cl = new Calendar();
            string[] strToList = null, strCCList = null;
            string strSubject = "", strContent = "", strToType = "", strCCType = "", strDesc = "", strType = "",
                strIds = "", strCreatedOn = "", strConfigID = "", strUnitId = "", strUnitName = "",
                strLoggedInUserName = "", strCurrentLevel = "", strNextLevel = "", strActionType = "",
                strsub = "", strModuleCode = "", strPeriod = "", strPeriodEndDate = "",
                strModuewiseToMailIds = "", strCreatorUserId = "", strRAFType = "", strExtraToMailIds = "", strExtraCCMailIds = "";
            try
            {
                if (ParamMap.ContainsKey("ConfigId"))
                {
                    strCreatedOn = cl.TodaysDate.ToString("dd-MMM-yyyy HH:mm:ss");
                    strConfigID = ParamMap["ConfigId"].ToString();
                    dtMail = configBl.searchMailConfig(strConfigID, "", "Outward", mstrConnectionString);
                    if (dtMail.Rows.Count > 0)
                    {
                        drMail = dtMail.Rows[0];

                        strExtraToMailIds = drMail["MCM_FROM"].ToString().Replace(",", "|");
                        strExtraCCMailIds = drMail["MCM_BCC"].ToString().Replace(",", "|");

                        strSubject = drMail["MCM_SUBJECT"].ToString();
                        strContent = drMail["MCM_CONTENT"].ToString();
                        strToType = ParamMap["To"].ToString();
                        strCCType = ParamMap["cc"].ToString();


                        if (ParamMap["departmentName"] != null)
                        {
                            strUnitName = ParamMap["departmentName"].ToString();
                        }

                        if (ParamMap["LoggedInUserName"] != null)
                        {
                            strLoggedInUserName = ParamMap["LoggedInUserName"].ToString();
                        }
                        if (ParamMap["Ids"] != null)
                        {
                            strIds = ParamMap["Ids"].ToString();
                        }
                        if (ParamMap["Subject"] != null)
                        {
                            strsub = ParamMap["Subject"].ToString();
                        }



                        if (ParamMap["ModuleCode"] != null)
                        {
                            strModuleCode = ParamMap["ModuleCode"].ToString();
                        }


                        if (ParamMap["CreatorUserId"] != null)
                        {
                            strCreatorUserId = ParamMap["CreatorUserId"].ToString();
                        }


                        if (strModuleCode.Equals("Outward"))
                        {
                            if (strConfigID.Equals("1095"))
                            {
                                strSubject = strSubject.Replace("%Subject%", strsub);
                                strSubject = strSubject.Replace("%LoggedInUserName%", strLoggedInUserName);

                                strContent = strContent.Replace("%Subject%", strsub);
                                strContent = strContent.Replace("%LoggedInUserName%", strLoggedInUserName);
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("/Projects/Outward/SearchOutward.aspx?type=closure") +
                                                                          "'>Click here</a> to close the outward details.");
                                strContent = strContent.Replace("%Table%", getHTMLTableForOutwardSubmit(strIds, "Submit", strRole, strLoggedInUserName, strConfigID));
                            }
                            if (strConfigID.Equals("1096"))
                            {
                                strSubject = strSubject.Replace("%Subject%", strsub);
                                strSubject = strSubject.Replace("%LoggedInUserName%", strLoggedInUserName);

                                strContent = strContent.Replace("%Subject%", strsub);
                                strContent = strContent.Replace("%LoggedInUserName%", strLoggedInUserName);
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/Outward/SearchOutward.aspx") +
                                                                          "'>Click here</a> to view the outward details.");
                                strContent = strContent.Replace("%Table%", getHTMLTableForOutwardSubmit(strIds, "Submit", strRole, strLoggedInUserName, strConfigID));
                            }
                            if (strConfigID.Equals("1097"))
                            {
                                strSubject = strSubject.Replace("%Subject%", strsub);
                                strSubject = strSubject.Replace("%LoggedInUserName%", strLoggedInUserName);

                                strContent = strContent.Replace("%Subject%", strsub);
                                strContent = strContent.Replace("%LoggedInUserName%", strLoggedInUserName);
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/Outward/SearchOutward.aspx") +
                                                                          "'>Click here</a> to edit outward details.");
                                strContent = strContent.Replace("%Table%", getHTMLTableForOutwardSubmit(strIds, "Submit", strRole, strLoggedInUserName, strConfigID));
                            }
                            if (strConfigID.Equals("1098"))
                            {
                                strSubject = strSubject.Replace("%Subject%", strsub);
                                strSubject = strSubject.Replace("%LoggedInUserName%", strLoggedInUserName);

                                strContent = strContent.Replace("%Subject%", strsub);
                                strContent = strContent.Replace("%LoggedInUserName%", strLoggedInUserName);
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/Outward/SearchOutward.aspx") +
                                                                          "'>Click here</a> to view outward details.");
                                strContent = strContent.Replace("%Table%", getHTMLTableForOutwardSubmit(strIds, "Submit", strRole, strLoggedInUserName, strConfigID));
                            }
                            if (strConfigID.Equals("1099"))
                            {
                                strSubject = strSubject.Replace("%Subject%", strsub);
                                strSubject = strSubject.Replace("%LoggedInUserName%", strLoggedInUserName);

                                strContent = strContent.Replace("%Subject%", strsub);
                                strContent = strContent.Replace("%LoggedInUserName%", strLoggedInUserName);
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/Outward/SearchOutward.aspx") +
                                                                          "'>Click here</a> to view outward details.");
                                strContent = strContent.Replace("%Table%", getHTMLTableForOutwardSubmit(strIds, "Submit", strRole, strLoggedInUserName, strConfigID));
                            }
                        }

                        setMailIds("TO", strToType, strUnitId, "", strCreatorUserId);
                        setMailIds("CC", strCCType, strUnitId, "", strCreatorUserId);

                        if (!strTo.Equals(""))
                        {
                            strTo = strTo.ToString().Replace("||", "|");
                            strTo = strTo.Substring(0, strTo.Length - 1);
                            strTo = strTo + "|" + strExtraToMailIds;
                            strToList = strTo.Split('|');
                        }
                        if (!strCC.Equals(""))
                        {
                            strCC = strCC.ToString().Replace("||", "|");
                            strCC = strCC.Substring(0, strCC.Length - 1);
                            strCC = strCC + "|" + strExtraCCMailIds;
                            strCCList = strCC.Split('|');
                        }

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

                        //strModuewiseToMailIds = CommonCode.getConfigutableParamValue("To Email Ids", "Risk Acceptance Form");

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
        private string getUserMailIds(string strRoleName)
        {
            string strUserMailIds = "";
            string[] strUsers = Roles.GetUsersInRole(strRoleName);

            for (int i = 0; i <= strUsers.GetUpperBound(0); i++)
            {
                MembershipUser membershipUser = Membership.GetUser(strUsers[i]);
                strUserMailIds = strUserMailIds + membershipUser.Email + "|";
            }

            return strUserMailIds;
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

        //<< Added by shwetan on 28-7-2020
        private string getHTMLTableForOutwardSubmit(string strIds, string strAction, string strRole, string loggedUser, string strConfigId)
        {
            string strHTML = "", OT_Date = "", OT_CREATE_ON = "", OT_Suggestion_ON, OT_Deactivate_ON, OT_Deleted_ON, OT_Closed_ON;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;
            if (strConfigId.Equals("1095"))
            {
                strHTML = "<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>" +
                               "<th " + strTableHeaderCSS + ">Outward No.</th>" +
                               "<th " + strTableHeaderCSS + ">Outward Type</th>" +
                               "<th " + strTableHeaderCSS + ">Department</th>" +
                               "<th " + strTableHeaderCSS + ">Document Date</th>" +
                               "<th " + strTableHeaderCSS + ">Subject</th>" +
                               "<th " + strTableHeaderCSS + ">Regulatory Authority</th>" +
                                "<th " + strTableHeaderCSS + ">Function Remarks</th>" +
                               "<th " + strTableHeaderCSS + ">Created By</th>" +
                               "<th " + strTableHeaderCSS + ">Created On</th>" +
                               "<th " + strTableHeaderCSS + ">Status</th>";
                strHTML = strHTML + "</tr>";
            }
            else if (strConfigId.Equals("1096"))
            {
                strHTML = "<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>" +
                               "<th " + strTableHeaderCSS + ">Outward No.</th>" +
                               "<th " + strTableHeaderCSS + ">Outward Type</th>" +
                               "<th " + strTableHeaderCSS + ">Department</th>" +
                               "<th " + strTableHeaderCSS + ">Document Date</th>" +
                               "<th " + strTableHeaderCSS + ">Subject</th>" +
                               "<th " + strTableHeaderCSS + ">Regulatory Authority</th>" +
                               "<th " + strTableHeaderCSS + ">Function Remarks</th>" +
                               "<th " + strTableHeaderCSS + ">From (Sender)</th>" +
                               "<th " + strTableHeaderCSS + ">To (Receiver)</th>" +
                               "<th " + strTableHeaderCSS + ">Sent Via" +
                                "<th " + strTableHeaderCSS + ">Closure Remarks" +
                                "<th " + strTableHeaderCSS + ">Closure By" +
                                "<th " + strTableHeaderCSS + ">Closure On" +
                               "<th " + strTableHeaderCSS + ">Created By</th>" +
                               "<th " + strTableHeaderCSS + ">Created On</th>" +
                               "<th " + strTableHeaderCSS + ">Status</th>";
                strHTML = strHTML + "</tr>";
            }
            else if (strConfigId.Equals("1097"))
            {
                strHTML = "<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>" +
                               "<th " + strTableHeaderCSS + ">Outward No.</th>" +
                                "<th " + strTableHeaderCSS + ">Outward Type</th>" +
                                "<th " + strTableHeaderCSS + ">Department</th>" +
                                "<th " + strTableHeaderCSS + ">Document Date</th>" +
                                "<th " + strTableHeaderCSS + ">Subject</th>" +
                                "<th " + strTableHeaderCSS + ">Regulatory Authority</th>" +
                                "<th " + strTableHeaderCSS + ">Function Remarks</th>" +
                                "<th " + strTableHeaderCSS + ">Revision Suggested Remarks</th>" +
                                "<th " + strTableHeaderCSS + ">Revision Suggested By</th>" +
                                "<th " + strTableHeaderCSS + ">Revision Suggested On</th>" +
                                "<th " + strTableHeaderCSS + ">Created By</th>" +
                                "<th " + strTableHeaderCSS + ">Created On</th>" +
                                "<th " + strTableHeaderCSS + ">Status</th>";
                strHTML = strHTML + "</tr>";
            }
            else if (strConfigId.Equals("1098"))
            {
                strHTML = "<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>" +
                               "<th " + strTableHeaderCSS + ">Outward No.</th>" +
                                "<th " + strTableHeaderCSS + ">Outward Type</th>" +
                                "<th " + strTableHeaderCSS + ">Department</th>" +
                                "<th " + strTableHeaderCSS + ">Document Date</th>" +
                                "<th " + strTableHeaderCSS + ">Subject</th>" +
                                "<th " + strTableHeaderCSS + ">Regulatory Authority</th>" +
                                "<th " + strTableHeaderCSS + ">Function Remarks</th>" +
                                "<th " + strTableHeaderCSS + ">Cancellation Remarks</th>" +
                                "<th " + strTableHeaderCSS + ">Cancellation By</th>" +
                                "<th " + strTableHeaderCSS + ">Cancellation On</th>" +
                                "<th " + strTableHeaderCSS + ">Created By</th>" +
                                "<th " + strTableHeaderCSS + ">Created On</th>" +
                                "<th " + strTableHeaderCSS + ">Status</th>";
                strHTML = strHTML + "</tr>";
            }
            else if (strConfigId.Equals("1099"))
            {
                strHTML = "<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>" +
                               "<th " + strTableHeaderCSS + ">Outward No.</th>" +
                                "<th " + strTableHeaderCSS + ">Outward Type</th>" +
                                "<th " + strTableHeaderCSS + ">Department</th>" +
                                "<th " + strTableHeaderCSS + ">Document Date</th>" +
                                "<th " + strTableHeaderCSS + ">Subject</th>" +
                                "<th " + strTableHeaderCSS + ">Regulatory Authority</th>" +
                                "<th " + strTableHeaderCSS + ">Function Remarks</th>" +
                                "<th " + strTableHeaderCSS + ">Deletion Remarks</th>" +
                                "<th " + strTableHeaderCSS + ">Deletion By</th>" +
                                "<th " + strTableHeaderCSS + ">Deletion On</th>" +
                                "<th " + strTableHeaderCSS + ">Created By</th>" +
                                "<th " + strTableHeaderCSS + ">Created On</th>" +
                                "<th " + strTableHeaderCSS + ">Status</th>";
                strHTML = strHTML + "</tr>";
            }
            if (strConfigId.Equals("1095"))
            {
                ds = outBL.searchOutwardformail(strIds, loggedUser, "Submitted");
                dt = ds.Tables[0];
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i - 1];

                    OT_Date = ((DateTime)dr["OT_DATE"]).ToString("dd-MMM-yyyy");
                    OT_CREATE_ON = ((DateTime)dr["OT_CREATE_DATE"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    strHTML = strHTML + "<tr><td " + strTableCellCSS + ">" + i + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_DOCUMENT_NO"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OTM_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["ODM_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + OT_Date + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_DOC_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["ORAM_NAME"].ToString() + "</td >" +
                                         "<td " + strTableCellCSS + ">" + dr["OT_REMARKS"].ToString() + "</td >" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_CREATE_BY"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + OT_CREATE_ON + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_STAUTS"].ToString() + "</td>";

                    strHTML = strHTML + "</tr>";
                }

            }
            else if (strConfigId.Equals("1096"))
            {
                ds = outBL.searchOutwardformail(strIds, loggedUser, "Closed");
                dt = ds.Tables[0];
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i - 1];

                    OT_Date = ((DateTime)dr["OT_DATE"]).ToString("dd-MMM-yyyy");
                    OT_Closed_ON = ((DateTime)dr["OT_CLOSED_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    OT_CREATE_ON = ((DateTime)dr["OT_CREATE_DATE"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    strHTML = strHTML + "<tr><td " + strTableCellCSS + ">" + i + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_DOCUMENT_NO"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OTM_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["ODM_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + OT_Date + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_DOC_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["ORAM_NAME"].ToString() + "</td>" +
                                       "<td " + strTableCellCSS + ">" + dr["OT_REMARKS"].ToString() + "</td >" +
                                        "<td " + strTableCellCSS + ">" + dr["OAM_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_ADRESSEE"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["sentVia"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_CLOSED_REMARK"].ToString() + "</td>" +
                                       "<td " + strTableCellCSS + ">" + dr["OT_CLOSED_BY"].ToString() + "</td>" +
                                       "<td " + strTableCellCSS + ">" + OT_Closed_ON + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_CREATE_BY"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + OT_CREATE_ON + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_STAUTS"].ToString() + "</td>";

                    strHTML = strHTML + "</tr>";
                }
            }
            else if (strConfigId.Equals("1097"))
            {
                ds = outBL.searchOutwardformail(strIds, loggedUser, "Changes suggested by Compliance");
                dt = ds.Tables[0];
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i - 1];

                    OT_Date = ((DateTime)dr["OT_DATE"]).ToString("dd-MMM-yyyy");
                    OT_CREATE_ON = ((DateTime)dr["OT_CREATE_DATE"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    OT_Suggestion_ON = ((DateTime)dr["OT_SUGGEST_REVISION_DT"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    strHTML = strHTML + "<tr><td " + strTableCellCSS + ">" + i + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_DOCUMENT_NO"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OTM_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["ODM_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + OT_Date + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_DOC_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["ORAM_NAME"].ToString() + "</td >" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_REMARKS"].ToString() + "</td >" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_SUGGEST_REVISION_REMARK"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_SUGGEST_REVISION_BY"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + OT_Suggestion_ON + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_CREATE_BY"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + OT_CREATE_ON + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_STAUTS"].ToString() + "</td>";

                    strHTML = strHTML + "</tr>";
                }

            }
            else if (strConfigId.Equals("1098"))
            {
                ds = outBL.searchOutwardformail(strIds, loggedUser, "Cancelled");
                dt = ds.Tables[0];
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i - 1];

                    OT_Date = ((DateTime)dr["OT_DATE"]).ToString("dd-MMM-yyyy");
                    OT_CREATE_ON = ((DateTime)dr["OT_CREATE_DATE"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    OT_Deactivate_ON = ((DateTime)dr["OT_CANCEL_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    strHTML = strHTML + "<tr><td " + strTableCellCSS + ">" + i + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_DOCUMENT_NO"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OTM_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["ODM_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + OT_Date + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_DOC_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["ORAM_NAME"].ToString() + "</td >" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_REMARKS"].ToString() + "</td >" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_CANCEL_REMARKS"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_CANCEL_BY"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + OT_Deactivate_ON + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_CREATE_BY"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + OT_CREATE_ON + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_STAUTS"].ToString() + "</td>";

                    strHTML = strHTML + "</tr>";
                }

            }
            else if (strConfigId.Equals("1099"))
            {
                ds = outBL.searchOutwardformail(strIds, loggedUser, "Deleted");
                dt = ds.Tables[0];
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i - 1];

                    OT_Date = ((DateTime)dr["OT_DATE"]).ToString("dd-MMM-yyyy");
                    OT_CREATE_ON = ((DateTime)dr["OT_CREATE_DATE"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    OT_Deleted_ON = ((DateTime)dr["OT_DELETED_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    strHTML = strHTML + "<tr><td " + strTableCellCSS + ">" + i + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_DOCUMENT_NO"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OTM_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["ODM_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + OT_Date + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_DOC_NAME"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["ORAM_NAME"].ToString() + "</td >" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_REMARKS"].ToString() + "</td >" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_DELETE_REMARK"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_DELETEED_BY"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + OT_Deleted_ON + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_CREATE_BY"].ToString() + "</td>" +
                                        "<td " + strTableCellCSS + ">" + OT_CREATE_ON + "</td>" +
                                        "<td " + strTableCellCSS + ">" + dr["OT_STAUTS"].ToString() + "</td>";

                    strHTML = strHTML + "</tr>";
                }

            }
            strHTML = strHTML + "</table>";
            return strHTML;
        }
        //>>

        private string getUserMailIdByUserId(string strUserIds)
        {
            string strEmailId = "", strEmailIdArray = "";
            Authentication auth = new Authentication();
            try
            {
                if (!strUserIds.Equals(""))
                {
                    string[] strarr;
                    strarr = strUserIds.Split(',');
                    for (int i = 0; i <= strarr.Length - 1; i++)
                    {
                        MembershipUser membershipUser = Membership.GetUser(strarr[i]);
                        if (membershipUser != null)
                        {
                            strEmailId = membershipUser.Email;
                        }

                        strEmailIdArray = strEmailIdArray + strEmailId + "|";
                    }
                    strEmailIdArray = strEmailIdArray.TrimEnd('|');
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return strEmailIdArray + "|";
        }





        private void setMailIds(string strToCCType, string strType, string strUnitId, string strDeptIdType,
            string strCreatorId)
        {
            string strReturn = "", strUserType = "";
            string[] arrType = null;

            arrType = strType.Split(',');
            if (arrType.Length > 0)
            {
                for (int i = 0; i < arrType.Length; i++)
                {
                    strUserType = arrType[i];

                    switch (strUserType)
                    {
                        case "Comp_User":
                            strReturn = strReturn + getUserMailIds("Outward_Compliance_User");
                            break;

                        case "Admin_User":
                            strReturn = strReturn + getUserMailIds("Outward_Admin");
                            break;

                        case "Outward_User":
                            strReturn = strReturn + getUserMailIds("Outward_User");
                            break;


                    }
                }
            }

            if (strToCCType.Equals("TO"))
                strTo = strReturn;
            else if (strToCCType.Equals("CC"))
                strCC = strReturn;
        }
    }
}