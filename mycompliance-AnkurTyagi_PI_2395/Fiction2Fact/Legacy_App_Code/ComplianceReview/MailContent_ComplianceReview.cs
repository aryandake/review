using DocumentFormat.OpenXml.Bibliography;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Fiction2Fact.Legacy_App_Code.Compliance
{
    public class MailContent_ComplianceReview
    {
        MailConfigBLL configBLL = new MailConfigBLL();
        ComplianceReviewBLL rrBLL = new ComplianceReviewBLL();
        UtilitiesBLL rrUtilBLL = new UtilitiesBLL();

        #region CSS for Table
        string strTableCellCSS = " style=\"font-size: 12px; font-family: Calibri; border-width: " +
          "1px; padding: 8px; border-style: solid;" +
          "border-color: black;\"";

        string strTableHeaderCSS = " style =\"font-size: 12px; font-family: Calibri; " +
                    "background-color: #970933; border-width: 1px; padding: 8px; color: #FFFFFF;" +
                    "border-style: solid; border-color: black; text-align: left;\"";

        string strTableCSS = " style=\"font-size: 12px; font-family: Calibri; color: #333333; " +
                    "width: 100%; border-width: 1px; border-color: black; border-collapse: collapse;\"";

        string strTableCntCSS = " style=\"font-size: 12px; font-family: Calibri; color: #333333; " +
                    "width: 50%; border-width: 1px; border-color: black; border-collapse: collapse;\"";
        #endregion

        private string strTo = "", strCC = "", strBCC = "", strIsValidated = "false";
        public Hashtable ParamMap = new Hashtable();



        public void setComplianceReviewMailContent()
        {
            DataRow drMail;
            DataTable dtMail = new DataTable();
            Calendar cl = new Calendar();
            string[] strToList = null, strCCList = null;
            string strSubject = "", strContent = "", strToType = "", strCCType = "", strType = "", strRDIIds = "", strCRIds = "",
                strConfigID = "", strUnitId = "", strUnitName = "", strLoggedInUserName = "", strSPOCId = "", strIssueType = "",
                strActionType = "", strCreatedOn = "", strModuewiseToMailIds = "", strRequestorId = "", strCreatorId = "",
                strReviewerMasId = "", strIds = "", strProcess = "", strRole = "", strSendTo = "", strIdentitfier = "", strSource = "";

            string strCircularId = "", strCirNo = "", strCirActionable = "",
               strResponsiblePerson = "", strTargetDate = "", strCompletionDate = "", strUpdateStatus = "",
               strUpdateType = "", strUpdateDetails = "", strCirActionableId = "", strRevisedTargetDate = "",
               strActionableClosureDate = "";

            try
            {
                if (ParamMap.ContainsKey("ConfigId"))
                {
                    strCreatedOn = cl.TodaysDate.ToString("dd-MMM-yyyy HH:mm:ss");

                    strConfigID = ParamMap["ConfigId"].ToString();
                    dtMail = configBLL.searchMailConfig(strConfigID, "", "CR");
                    if (dtMail.Rows.Count > 0)
                    {
                        drMail = dtMail.Rows[0];
                        strSubject = drMail["MCM_SUBJECT"].ToString();
                        strContent = drMail["MCM_CONTENT"].ToString();
                        strToType = ParamMap["To"].ToString();
                        strCCType = ParamMap["cc"].ToString();

                        if (ParamMap["CircularId"] != null)
                        {
                            strCircularId = ParamMap["CircularId"].ToString();
                        }
                        if (ParamMap["CirNo"] != null)
                        {
                            strCirNo = ParamMap["CirNo"].ToString();
                        }
                        if (ParamMap["CirActionableId"] != null)
                        {
                            strCirActionableId = ParamMap["CirActionableId"].ToString();
                        }
                        if (ParamMap["CirActionable"] != null)
                        {
                            strCirActionable = ParamMap["CirActionable"].ToString();
                        }
                        if (ParamMap["ResponsiblePerson"] != null)
                        {
                            strResponsiblePerson = ParamMap["ResponsiblePerson"].ToString();
                        }

                        if (ParamMap["TargetDate"] != null)
                        {
                            strTargetDate = ParamMap["TargetDate"].ToString();
                        }

                        if (ParamMap["CompletionDate"] != null)
                        {
                            strCompletionDate = ParamMap["CompletionDate"].ToString();
                        }

                        if (ParamMap["UpdateStatus"] != null)
                        {
                            strUpdateStatus = ParamMap["UpdateStatus"].ToString();
                        }

                        if (ParamMap["UpdateType"] != null)
                        {
                            strUpdateType = ParamMap["UpdateType"].ToString();
                        }

                        if (ParamMap["UpdateDetails"] != null)
                        {
                            strUpdateDetails = ParamMap["UpdateDetails"].ToString();
                        }
                        if (ParamMap["RevisedTargetDate"] != null)
                        {
                            strRevisedTargetDate = ParamMap["RevisedTargetDate"].ToString();
                        }

                        if (ParamMap["ActionableClosureDate"] != null)
                        {
                            strActionableClosureDate = ParamMap["ActionableClosureDate"].ToString();
                        }


                        if (ParamMap["UnitId"] != null)
                        {
                            strUnitId = ParamMap["UnitId"].ToString();
                        }

                        if (ParamMap["UnitName"] != null)
                        {
                            strUnitName = ParamMap["UnitName"].ToString();
                        }

                        if (ParamMap["ActionType"] != null)
                        {
                            strActionType = ParamMap["ActionType"].ToString();
                        }

                        if (ParamMap["LoggedInUserName"] != null)
                        {
                            strLoggedInUserName = ParamMap["LoggedInUserName"].ToString();
                        }

                        if (ParamMap["RDIIds"] != null)
                        {
                            strRDIIds = ParamMap["RDIIds"].ToString();
                        }

                        if (ParamMap["RRIds"] != null)
                        {
                            strCRIds = ParamMap["RRIds"].ToString();
                        }

                        if (ParamMap["Type"] != null)
                        {
                            strType = ParamMap["Type"].ToString();
                        }

                        if (ParamMap["IssueType"] != null)
                        {
                            strIssueType = ParamMap["IssueType"].ToString();
                        }

                        if (ParamMap["CreatorId"] != null)
                        {
                            strCreatorId = ParamMap["CreatorId"].ToString();
                        }

                        if (ParamMap["SPOCId"] != null)
                        {
                            strSPOCId = ParamMap["SPOCId"].ToString();
                        }

                        if (ParamMap["RequestorId"] != null)
                        {
                            strRequestorId = ParamMap["RequestorId"].ToString();
                        }

                        if (ParamMap["ReviewerMasId"] != null)
                        {
                            strReviewerMasId = ParamMap["ReviewerMasId"].ToString();
                        }

                        if (ParamMap["Process"] != null)
                        {
                            strProcess = ParamMap["Process"].ToString();
                        }

                        if (ParamMap["Role"] != null)
                        {
                            strRole = ParamMap["Role"].ToString();
                        }

                        if (ParamMap["SendTo"] != null)
                        {
                            strSendTo = ParamMap["SendTo"].ToString();
                        }

                        if (ParamMap["SourceIds"] != null)
                        {
                            strCRIds = ParamMap["SourceIds"].ToString();
                        }
                        if (ParamMap["Ids"] != null)
                        {
                            strRDIIds = ParamMap["Ids"].ToString();
                        }
                        if (ParamMap["Source"] != null)
                        {
                            strSource = ParamMap["Source"].ToString();
                        }
                        if (ParamMap["SourceIdentifier"] != null)
                        {
                            strIdentitfier = ParamMap["SourceIdentifier"].ToString();
                        }

                        strSubject = strSubject.Replace("%LoggedInUserName%", strLoggedInUserName);
                        strSubject = strSubject.Replace("%UnitName%", strUnitName);
                        strSubject = strSubject.Replace("%Process%", strProcess);
                        strSubject = strSubject.Replace("%Role%", strRole);



                        if (strConfigID.Equals("1091"))
                        {
                            //for initiation compliance review
                            strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/SearchComplianceReview.aspx") +
                                                            "?Type=MY'>Click here</a>");

                            strContent = strContent.Replace("%Table%", getHTMLTableForCRInitiation(strCRIds));
                        }

                        else if (strConfigID.Equals("1093"))
                        {
                            //for data requirment submit
                            strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/SearchDataRequirement.aspx") +
                                                            "?Type=RES'>Click here</a>");

                            strContent = strContent.Replace("%Table%", getHTMLTableForDRCreation(strCRIds, strType));
                        }
                        else if (strConfigID.Equals("1092") || strConfigID.Equals("1094"))
                        {
                            //for data requirment updation and closure
                            strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/SearchComplianceReview.aspx") +
                                                            "'>Click here</a>");

                            strContent = strContent.Replace("%Table%", getHTMLTableForDRCreation(strRDIIds, strType));
                        }

                        else if (strConfigID.Equals("1095"))
                        {
                            strContent = strContent.Replace("%Table%", getHTMLTableForResponseAdded(strRDIIds, strSource, strSPOCId, strType, strIdentitfier));
                            if (strUpdateType.ToLower() == "res")
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/SearchComplianceReview.aspx") +
                                                            "?Type=MY'>Click here</a>");
                            }
                            else if (strUpdateType.ToLower() == "req")
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/SearchDataRequirement.aspx") +
                                                            "'>Click here</a>");
                            }

                        }
                        else if (strConfigID.Equals("1096"))
                        {
                            strContent = strContent.Replace("%Table%", getHTMLTableForIssuesIdentified(strRDIIds, strActionType, "", ""));
                            if (strIsValidated.Equals("false"))
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/SearchComplianceReview.aspx") +
                                                                "'>Click here</a>");
                            }
                            else
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx") +
                                                                "?Type=UH'>Click here</a>");
                            }
                        }
                        else if (strConfigID.Equals("1097"))
                        {
                            if (strType.Equals("UH") && strIssueType.Equals("Draft"))
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx") +
                                                                "?Type=RM1'>Click here</a>");

                                strIds = strRDIIds;
                            }
                            else if (strType.Equals("RM1"))
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx") +
                                                                "?Type=RM2'>Click here</a>");

                                strIds = strCRIds;
                            }
                            else if (strType.Equals("FH") && strIssueType.Equals("Draft"))
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx") +
                                                                "?Type=RM2'>Click here</a>");

                                strIds = strRDIIds;
                            }
                            else if (strType.Equals("RM2"))
                            {
                                if (strSendTo.Equals("US"))
                                {
                                    strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx") +
                                                                "?Type=US'>Click here</a>");
                                }
                                else if (strSendTo.Equals("UH"))
                                {
                                    strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx") +
                                                                "?Type=UH'>Click here</a>");
                                }
                                else if (strSendTo.Equals("FH"))
                                {
                                    strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx") +
                                                                "?Type=FH'>Click here</a>");
                                }
                                else if (strSendTo.Equals("L2"))
                                {
                                    strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx") +
                                                                "?Type=RM3'>Click here</a>");
                                }

                                //strIds = strRRIds;
                                strIds = strRDIIds;
                            }
                            else if (strType.Equals("US"))
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx") +
                                                                "?Type=UH'>Click here</a>");

                                strIds = strRDIIds;
                            }
                            else if (strType.Equals("UH") && strIssueType.Equals("Final"))
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx") +
                                                                "?Type=L0'>Click here</a>");

                                strIds = strRDIIds;
                            }
                            else if (strType.Equals("L0") && strIssueType.Equals("Final"))
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx") +
                                                                "?Type=L1'>Click here</a>");

                                strIds = strRDIIds;
                            }
                            else if (strType.Equals("L1") && strIssueType.Equals("Final"))
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx") +
                                                                "?Type=UH'>Click here</a>");

                                strIds = strRDIIds;
                            }
                            else if (strType.Equals("L2") && strIssueType.Equals("Final"))
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/SearchComplianceReview.aspx") +
                                                                "'>Click here</a>");

                                strIds = strCRIds;
                            }

                            strContent = strContent.Replace("%Table%", getHTMLTableForIssuesIdentified(strIds, strActionType, strIssueType, strType));
                        }
                        else if (strConfigID.Equals("1098"))
                        {
                            if ((strType.Equals("UH") && strIssueType.Equals("Draft")))
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/SearchComplianceReview.aspx") +
                                                                "?Type=L0'>Click here</a>");
                            }
                            else if ((strType.Equals("L1") && strIssueType.Equals("Draft")))
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx") +
                                                                "?Type=L0'>Click here</a>");
                            }
                            else if ((strType.Equals("L2") && strIssueType.Equals("Draft")))
                            {
                                strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx") +
                                                                "?Type=RM2'>Click here</a>");
                            }

                            strContent = strContent.Replace("%Table%", getHTMLTableForIssuesIdentified(strRDIIds, strActionType, strIssueType, strType));
                        }

                        else if (strConfigID.Equals("1099"))
                        {
                            strContent = strContent.Replace("%Table%", getHTMLTableForResponseAdded(strRDIIds, strSource, strSPOCId, strType, strIdentitfier));
                        }

                        else if (strConfigID.Equals("1100"))
                        {
                            strSubject = strSubject.Replace("%UpdatedBy%", strLoggedInUserName);

                            StringBuilder sbHTML = new StringBuilder();
                            sbHTML.Append("<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>");
                            sbHTML.Append("<th " + strTableHeaderCSS + ">Compliance Review No.</th>");
                            sbHTML.Append("<th " + strTableHeaderCSS + ">Actionable</th>");
                            sbHTML.Append("<th " + strTableHeaderCSS + ">Person Responsible</th>");
                            sbHTML.Append("<th " + strTableHeaderCSS + ">Target Date</th>");
                            sbHTML.Append("<th " + strTableHeaderCSS + ">Completion Date</th>");
                            sbHTML.Append("<th " + strTableHeaderCSS + ">Status</th>");
                            sbHTML.Append("<th " + strTableHeaderCSS + ">Update Type</th>");
                            sbHTML.Append("<th " + strTableHeaderCSS + ">Update Details</th>");
                            sbHTML.Append("<th " + strTableHeaderCSS + ">Revised Target Date</th>");
                            sbHTML.Append("<th " + strTableHeaderCSS + ">Actionable Closure Date</th>");
                            sbHTML.Append("<th " + strTableHeaderCSS + ">Update Details</th></tr>");


                            sbHTML.Append("<tr><td " + strTableCellCSS + ">1</td>");
                            sbHTML.Append("<td " + strTableCellCSS + ">" + strCirNo + "</td >");
                            sbHTML.Append("<td " + strTableCellCSS + ">" + (strCirActionable.Length > 200 ? strCirActionable.Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : strCirActionable.Replace(Environment.NewLine, "<br />")) + "</td>");
                            sbHTML.Append("<td " + strTableCellCSS + ">" + strResponsiblePerson + "</td>");
                            sbHTML.Append("<td " + strTableCellCSS + ">" + strTargetDate + "</td>");
                            sbHTML.Append("<td " + strTableCellCSS + ">" + strCompletionDate + "</td>");
                            sbHTML.Append("<td " + strTableCellCSS + ">" + strUpdateStatus + "</td >");
                            sbHTML.Append("<td " + strTableCellCSS + ">" + strUpdateType + "</td>");
                            sbHTML.Append("<td " + strTableCellCSS + ">" + (strUpdateDetails.Length > 200 ? strUpdateDetails.Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : strUpdateDetails.Replace(Environment.NewLine, "<br />")) + "</td >");
                            sbHTML.Append("<td " + strTableCellCSS + ">" + strRevisedTargetDate + "</td>");
                            sbHTML.Append("<td " + strTableCellCSS + ">" + strActionableClosureDate + "</td>");
                            sbHTML.Append("<td " + strTableCellCSS + ">" + strUpdateType + "</td></tr>");
                            sbHTML.Append("</table>");

                            #region Old Code
                            //strContent = strContent.Replace("%UpdatedBy%", strLoggedInUserName);
                            //strContent = strContent.Replace("%UpdatedDate%", strCreatedOn);
                            //strContent = strContent.Replace("%CirNo%", strCirNo);
                            //strContent = strContent.Replace("%Actionable%", strCirActionable.Replace(Environment.NewLine, "<br />"));
                            //strContent = strContent.Replace("%ResponsiblePerson%", strResponsiblePerson);
                            //strContent = strContent.Replace("%TargetDate%", strTargetDate);
                            //strContent = strContent.Replace("%CompletionDate%", strCompletionDate);
                            //strContent = strContent.Replace("%UpdateStatus%", strUpdateStatus);
                            //strContent = strContent.Replace("%UpdateType%", strUpdateType);
                            //strContent = strContent.Replace("%UpdateDetails%", strUpdateDetails.Replace(Environment.NewLine, "<br />"));
                            //strContent = strContent.Replace("%RevisedTargetDate%", strRevisedTargetDate);
                            //strContent = strContent.Replace("%ActionableClosureDate%", strActionableClosureDate); 
                            #endregion
                            strContent = strContent.Replace("%Table%", sbHTML.ToString());
                            strContent = strContent.Replace("%UpdatedBy%", strLoggedInUserName);
                            strContent = strContent.Replace("%UpdatedDate%", strCreatedOn);
                            strContent = strContent.Replace("%ActionLink%", "<a href=\"" + Global.site_url("Projects/ComplianceReview/MyActionables.aspx\">Click here</a> to view the actionable."));
                            //strContent = strContent.Replace("%ActionLink%", "<a href=\"" + Global.site_url("Projects/ComplianceReview/ActionableUpdates.aspx?ActionableId=" + strCirActionableId +
                            //            "&CircularId=" + strCircularId) + "\" target=\"_blank\">Click here</a> to view the actionable.");
                        }


                        else if (strConfigID.Equals("1101"))
                        {
                            strContent = strContent.Replace("%Table%", getHTMLTableForIssuesIdentified(strRDIIds, strActionType, "", ""));
                            strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/SearchComplianceReview.aspx?Type=MY") +
                                                            "'>Click here</a>");

                        }
                        else if (strConfigID.Equals("1102"))
                        {
                            //for reject response of  issue
                            strContent = strContent.Replace("%Table%", getHTMLTableForIssuesIdentified(strRDIIds, strActionType, "", ""));
                            strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/SearchComplianceReview.aspx?Type=RES") +
                                                            "'>Click here</a>");

                        }
                        else if (strConfigID.Equals("1103"))
                        {
                            //for accept response of issue
                            strContent = strContent.Replace("%Table%", getHTMLTableForIssuesIdentified(strRDIIds, strActionType, "", ""));
                            strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/SearchComplianceReview.aspx") +
                                                            "'>Click here</a>");

                        }
                        else if (strConfigID.Equals("1104"))
                        {
                            //for submit for approval compliance review
                            strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/ApprovalComplianceReview.aspx?Type=L1") +
                                                            "'>Click here</a>");

                            strContent = strContent.Replace("%Table%", getHTMLTableForCRInitiation(strCRIds));
                        }

                        else if (strConfigID.Equals("1106"))
                        {
                            strContent = strContent.Replace("%LoggedInUserName%", strLoggedInUserName);
                            strContent = strContent.Replace("%Table%", getHTMLTableForIssuesIdentified(strRDIIds, strActionType, "", ""));
                            strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/ComplianceReview/SearchComplianceReview.aspx?Type=RES") +
                                "'>Click here</a>");
                        }

                        strContent = strContent.Replace("%LoggedInUserName%", strLoggedInUserName);
                        strContent = strContent.Replace("%SourceIdentifier%", strIdentitfier);
                        strContent = strContent.Replace("%Source%", strSource);
                        strContent = strContent.Replace("%Process%", strProcess);

                        setMailIds("TO", strToType, strUnitId, strSPOCId, strCreatorId, strRequestorId, strReviewerMasId);
                        setMailIds("CC", strCCType, strUnitId, strSPOCId, strCreatorId, strRequestorId, strReviewerMasId);

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

                        Mail mm = new Mail();
                        mm.sendAsyncMail(strToList, strCCList, null, strSubject, strContent);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private string getHTMLTableForResponseAdded(string strIds, string strSourceCode, string strSPOCId = "", string strType = "",
           string strSourceIdentifier = "")
        {
            DataTable dt = new DataTable();
            DataRow dr;
            StringBuilder sbHTML = new StringBuilder();

            sbHTML.Append("<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Data Requirement / Query</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Update Type</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Response</th>");
            sbHTML.Append("</tr>");

            if (strType.Equals("US"))
                dt = rrBLL.getDRQMResponse(0, 0, strSourceCode, "US", "RESMailContent", strSPOCId, strSourceIdentifier, strIds);
            else
                dt = rrBLL.getDRQMResponse(0, Convert.ToInt32(strIds), "CR");

            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                dr = dt.Rows[i - 1];

                sbHTML.Append("<tr><td " + strTableCellCSS + ">" + i + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CDQ_QUERY_DATA_REQUIREMENT"].ToString().Length > 200 ? (dr["CDQ_QUERY_DATA_REQUIREMENT"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CDQ_QUERY_DATA_REQUIREMENT"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["UpdateType"].ToString() + "</td >");
                sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CRDU_REMARKS"].ToString().Length > 200 ? (dr["CRDU_REMARKS"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CRDU_REMARKS"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                sbHTML.Append("</tr>");
            }

            sbHTML.Append("</table>");
            return sbHTML.ToString();
        }

        private string getHTMLTableForDRCreation(string strIds, string strType, string strSourceCode = null)
        {
            DataTable dt = new DataTable();
            DataTable dtSubmit = new DataTable();
            DataRow dr;
            StringBuilder sbHTML = new StringBuilder();
            string strRaisedOn = "", strDueOn = "", strClosedOn = "";

            sbHTML.Append("<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Responsible Unit</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Data Requirement / Query</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Person Responsible</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Raised Date</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Due Date</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Ageing</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Status</th>");

            if (strType.Equals("Closure"))
            {
                sbHTML.Append("<th " + strTableHeaderCSS + ">Closed by</th>");
                sbHTML.Append("<th " + strTableHeaderCSS + ">Closed on</th>");
                sbHTML.Append("<th " + strTableHeaderCSS + ">Closure Remarks</th>");
            }

            sbHTML.Append("</tr>");

            if (strType.Equals("Submit"))
            {
                //for get according source id CDQ_Source_ID
                dt = rrBLL.getDRQMDetails(0, Convert.ToInt32(strIds), strSourceCode, 0, null, strValue: " and CDQ_IS_MAIL_SENT='N'");

                if (dt.Rows.Count == 0)
                {
                    return "false";
                }
                else
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        rrBLL.submitForOperation(Convert.ToInt32(dt.Rows[i]["CDQ_ID"].ToString()), "CR", "DR_UpdateIsMailSent", null, intRiskReviewDraftId: Convert.ToInt32(strIds));
                    }
                }
            }
            else
            {
                //for select specific according to CDQ_ID
                dt = rrBLL.getDRQMDetails(Convert.ToInt32(strIds), 0, null, 0, null, strValue: " and CDQ_IS_MAIL_SENT='Y'");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];

                if (dr["CDQ_RAISED_DT"] != null && dr["CDQ_RAISED_DT"] != DBNull.Value)
                    strRaisedOn = ((DateTime)dr["CDQ_RAISED_DT"]).ToString("dd-MMM-yyyy HH:mm:ss");

                if (dr["CDQ_EXPIRY_DT"] != null && dr["CDQ_EXPIRY_DT"] != DBNull.Value)
                    strDueOn = ((DateTime)dr["CDQ_EXPIRY_DT"]).ToString("dd-MMM-yyyy");

                if (dr["CDQ_CLOSED_ON"] != null && dr["CDQ_CLOSED_ON"] != DBNull.Value)
                    strClosedOn = ((DateTime)dr["CDQ_CLOSED_ON"]).ToString("dd-MMM-yyyy");

                sbHTML.Append("<tr><td " + strTableCellCSS + ">" + (i + 1) + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CSFM_NAME"].ToString() + "</td >");
                sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CDQ_QUERY_DATA_REQUIREMENT"].ToString().Length > 200 ? (dr["CDQ_QUERY_DATA_REQUIREMENT"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CDQ_QUERY_DATA_REQUIREMENT"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CDQ_PERSON_RESPONSIBLE"].ToString() + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + strRaisedOn + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + strDueOn + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["Ageing"].ToString() + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["Status"].ToString() + "</td>");

                if (strType.Equals("Closure"))
                {
                    sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CDQ_CLOSED_BY"].ToString() + "</td>");
                    sbHTML.Append("<td " + strTableCellCSS + ">" + strClosedOn + "</td>");
                    sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CDQ_CLOSURE_REMARKS"].ToString().Length > 200 ? (dr["CDQ_CLOSURE_REMARKS"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CDQ_CLOSURE_REMARKS"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                }

                sbHTML.Append("</tr>");
            }

            sbHTML.Append("</table>");
            return sbHTML.ToString();
        }
        public static string getConfigutableParamValue(string strName, string strType = null)
        {
            string strSql = "", strOutput = "";
            DataTable dt = new DataTable();
            try
            {
                strSql = " SELECT * FROM TBL_CONFIG_PARAMS WHERE CP_NAME = '" + strName + "' ";

                if (!strType.Equals("") && strType != null)
                    strSql += " AND CP_TYPE = '" + strType + "' ";

                using (F2FDatabase DB = new F2FDatabase(strSql)) { DB.F2FDataAdapter.Fill(dt); }

                if (dt.Rows.Count > 0)
                {
                    strOutput = dt.Rows[0]["CP_VALUE"].ToString();
                }
            }
            catch (System.Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
            return strOutput;
        }

        private string getUserMailIdByUserId(string strUserId)
        {
            string strUserMailId = "";

            if (strUserId.Contains(","))
            {
                for (int i = 0; i < strUserId.Split(',').Length; i++)
                {
                    MembershipUser membershipUser = Membership.GetUser(strUserId.Split(',')[i]);
                    if (membershipUser != null)
                    {
                        strUserMailId += membershipUser.Email + "|";
                    }
                }
            }
            else
            {
                MembershipUser membershipUser = Membership.GetUser(strUserId);
                if (membershipUser != null)
                {
                    strUserMailId = membershipUser.Email + "|";
                }
            }

            return strUserMailId;
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

        private string getRiskManagementUsers()
        {
            string strRMMailIds = "";

            strRMMailIds = strRMMailIds + getUserMailIds("Risk Management Level 1");
            strRMMailIds = strRMMailIds + getUserMailIds("Risk Management Level 2");
            strRMMailIds = strRMMailIds + getUserMailIds("Risk Management Level 3");
            return strRMMailIds;
        }

        private void setMailIds(string strToCCType, string strType, string strUnitId, string strSPOCId, string strCreatorId,
            string strRequestorId, string strReviewerMasId)
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
                        case "ProvidedAsParam":
                            strReturn = strReturn + ParamMap["ToEmailIds"].ToString();
                            strReturn = strReturn.Replace(",", "|") + "|";
                            break;

                        case "ProvidedAsParam1":
                            strReturn = strReturn + ParamMap["CCEmailIds"].ToString();
                            strReturn = strReturn.Replace(",", "|") + "|";
                            break;

                        case "Creator":
                            strReturn = strReturn + getUserMailIdByUserId(strCreatorId);
                            break;

                        case "SPOC":
                            strReturn = strReturn + getUserMailIdByUserId(strSPOCId);
                            break;

                        case "Requestor":
                            strReturn = strReturn + getUserMailIdByUserId(strRequestorId);
                            break;

                        case "RM Team":
                            strReturn = strReturn + getRiskManagementUsers();
                            break;

                        case "US":
                            strReturn = strReturn + getUserMailIds("Risk_Review_Unit_SPOC");
                            break;

                        case "RC":
                            //strReturn = strReturn + getRiskChampion(strUnitId);
                            break;

                        case "UH":
                            strReturn = strReturn + getUnitHead1(strUnitId);
                            break;

                        case "FH":
                            //strReturn = strReturn + getFunctionHead(strUnitId);
                            break;

                        case "RM1":
                            strReturn = strReturn + getUserMailIds("Risk Management Level 1");
                            break;

                        case "RM2":
                            strReturn = strReturn + getUserMailIds("Risk Management Level 2");
                            break;

                        case "RM3":
                            strReturn = strReturn + getUserMailIds("Risk Management Level 3");
                            break;

                        case "RM4":
                            strReturn = strReturn + getUserMailIds("Risk Management Level 4");
                            break;

                        case "L0":
                            strReturn = strReturn + getReviewerMasUsers(strReviewerMasId, "L0");
                            break;

                        case "L1":
                            strReturn = strReturn + getReviewerMasUsers(strReviewerMasId, "L1");
                            break;

                        case "L2":
                            strReturn = strReturn + getReviewerMasUsers(strReviewerMasId, "L2");
                            break;
                    }
                }
            }

            if (strToCCType.Equals("TO"))
                strTo = strReturn;
            else if (strToCCType.Equals("CC"))
                strCC = strReturn;
        }

        private string getHTMLTableForCRInitiation(string strIds)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            StringBuilder sbHTML = new StringBuilder();
            string strStartDate = "", strEndDate = "";

            sbHTML.Append("<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Compliance Review No. </th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Universe to be Reviewed</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Business Unit(s)</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Reviewer Name</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Review Type</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Tentative Start Date</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Tentative End Date</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Review Scope</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Remarks</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Compliance Review Status</th>");
            sbHTML.Append("</tr>");

            dt = rrBLL.Search_ComplianceReview(Convert.ToInt32(strIds), 0, 0, "", "", "", "");

            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                dr = dt.Rows[i - 1];

                if (dr["CCR_TENTATIVE_START_DATE"] != null && dr["CCR_TENTATIVE_START_DATE"] != DBNull.Value)
                    strStartDate = ((DateTime)dr["CCR_TENTATIVE_START_DATE"]).ToString("dd-MMM-yyyy");
                if (dr["CCR_TENTATIVE_END_DATE"] != null && dr["CCR_TENTATIVE_END_DATE"] != DBNull.Value)
                    strEndDate = ((DateTime)dr["CCR_TENTATIVE_END_DATE"]).ToString("dd-MMM-yyyy");

                sbHTML.Append("<tr><td " + strTableCellCSS + ">" + i + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CCR_IDENTIFIER"].ToString() + "</td >");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CRUM_UNIVERSE_TO_BE_REVIEWED"].ToString() + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["ImpactedUnits"].ToString() + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CRM_L0_REVIEWER_NAME"].ToString() + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["RC_NAME"].ToString() + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + strStartDate + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + strEndDate + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CCR_REVIEW_SCOPE"].ToString().Length > 200 ? (dr["CCR_REVIEW_SCOPE"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CCR_REVIEW_SCOPE"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CCR_REMARKS"].ToString().Length > 200 ? (dr["CCR_REMARKS"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CCR_REMARKS"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["SM_DESC"].ToString() + "</td>");
                sbHTML.Append("</tr>");
            }

            sbHTML.Append("</table>");
            return sbHTML.ToString();
        }

        private string getHTMLTableForIssuesIdentified(string strIds, string strActionType, string strIssueType, string strUserType)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            StringBuilder sbHTML = new StringBuilder();
            string strApprovedOn = "", strRejectedOn = "";

            sbHTML.Append("<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Compliance Review No.</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Responsible Unit</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Issue Title</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Issue Description</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Issue Type</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Issue Status</th>");
            sbHTML.Append("<th " + strTableHeaderCSS + ">Remarks</th>");


            if (strActionType.Equals("Rejection"))
            {
                sbHTML.Append("<th " + strTableHeaderCSS + ">Changes Conveyed By</th>");
                sbHTML.Append("<th " + strTableHeaderCSS + ">Changes Conveyed On</th>");

                if (!strUserType.Equals("RM1"))
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Changes Conveyed Remarks</th>");

                dt = rrBLL.getIssue(0, 0, null, null, null, strValue1: " and CI_ID in (" + strIds + ") ");
            }
            else if (strActionType.Equals("Approval"))
            {
                sbHTML.Append("<th " + strTableHeaderCSS + ">Approval By</th>");
                sbHTML.Append("<th " + strTableHeaderCSS + ">Approval On</th>");

                if (!strUserType.Equals("RM1"))
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Approval Remarks</th>");

                if (strUserType.Equals("US") || strUserType.Equals("UH") || strUserType.Equals("FH") || strUserType.Equals("RM2") || strUserType.Equals("L0") || strUserType.Equals("L1"))
                    dt = rrBLL.getIssue(0, 0, null, null, null, strValue1: " and CI_ID in (" + strIds + ") ");
                else
                    dt = rrBLL.getIssue(0, 0, null, null, null, strValue1: " and CI_CCR_ID in (" + strIds + ") ");
            }
            else if (strActionType.Equals("SendToUS"))
            {
                if (dt.Rows.Count == 0)
                {
                    return "false";
                }
                else
                {
                }
            }
            else if (strActionType.Equals("SubmitIssueTracker"))
            {
                dt = rrBLL.getIssue(0, 0, null, null, "B", strValue1: " and CI_IS_MAIL_SENT='N' and CI_ID in (" + strIds + ") ");
            }
            else if (strActionType.Equals("SubmitIssueResponse"))
            {
                dt = rrBLL.getIssue(0, 0, null, null, null, strValue1: " and CI_ID in (" + strIds + ")"); ;
            }

            sbHTML.Append("<th " + strTableHeaderCSS + ">Status</th>" +
                          "</tr>");

            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                dr = dt.Rows[i - 1];

                sbHTML.Append("<tr><td " + strTableCellCSS + ">" + i + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CCR_IDENTIFIER"].ToString() + "</td >");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CSFM_NAME"].ToString() + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CI_ISSUE_TITLE"].ToString() + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CI_ISSUE_DESC"].ToString().Length > 200 ? (dr["CI_ISSUE_DESC"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CI_ISSUE_DESC"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["IssueType"].ToString() + "</td>");
                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["IssueStatus"].ToString() + "</td >");
                sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CI_REMARKS"].ToString().Length > 200 ? (dr["CI_REMARKS"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CI_REMARKS"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");


                if (!strActionType.Equals("SendToUS") && !strActionType.Equals("SubmitIssueTracker"))
                {
                    //if (strUserType.Equals("UH") && strIssueType.Equals("Draft"))
                    //{

                    //}
                    //else
                    //{
                    //    sbHTML.Append("<td " + strTableCellCSS + ">" + dr["ResidualRisk"].ToString() + "</td >");
                    //    sbHTML.Append("<td " + strTableCellCSS + ">" + dr["ControlType"].ToString() + "</td >");
                    //    sbHTML.Append("<td " + strTableCellCSS + ">" + dr["MakerChecker"].ToString() + "</td>");
                    //    sbHTML.Append("<td " + strTableCellCSS + ">" + dr["ControlNature"].ToString() + "</td>");
                    //    sbHTML.Append("<td " + strTableCellCSS + ">" + dr["ControlOperatingEff"].ToString() + "</td>");
                    //    sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["RDI_APPROVAL_REMARKS_RM1"].ToString().Length > 200 ? (dr["RDI_APPROVAL_REMARKS_RM1"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["RDI_APPROVAL_REMARKS_RM1"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    //    sbHTML.Append("<td " + strTableCellCSS + ">" + dr["ActionToBeTaken"].ToString() + "</td >");
                    //    sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["RDI_REASON_FOR_DROPPING_THE_ISSUE"].ToString().Length > 200 ? (dr["RDI_REASON_FOR_DROPPING_THE_ISSUE"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["RDI_REASON_FOR_DROPPING_THE_ISSUE"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    //}
                }

                if (strActionType.Equals("Rejection") && strIssueType.Equals("Draft"))
                {
                    if (strUserType.Equals("UH"))
                    {
                        if (dr["CI_REJECTION_ON_UH"] != null && dr["CI_REJECTION_ON_UH"] != DBNull.Value)
                            strApprovedOn = ((DateTime)dr["CI_REJECTION_ON_UH"]).ToString("dd-MMM-yyyy");

                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CI_REJECTION_BY_UH"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + strApprovedOn + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CI_REJECTION_REMARKS_UH"].ToString().Length > 200 ? (dr["CI_REJECTION_REMARKS_UH"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CI_REJECTION_REMARKS_UH"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    }

                    if (strUserType.Equals("L0"))
                    {
                        if (dr["CI_REJECTION_ON_L0"] != null && dr["CI_REJECTION_ON_L0"] != DBNull.Value)
                            strApprovedOn = ((DateTime)dr["CI_REJECTION_ON_L0"]).ToString("dd-MMM-yyyy");

                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CI_REJECTION_BY_L0"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + strApprovedOn + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CI_REJECTION_REMARKS_L0"].ToString().Length > 200 ? (dr["CI_REJECTION_REMARKS_L0"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CI_REJECTION_REMARKS_L0"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    }

                    if (strUserType.Equals("L1"))
                    {
                        if (dr["CI_REJECTION_ON_L1"] != null && dr["CI_REJECTION_ON_L1"] != DBNull.Value)
                            strApprovedOn = ((DateTime)dr["CI_REJECTION_ON_L1"]).ToString("dd-MMM-yyyy");

                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CI_REJECTION_BY_L1"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + strApprovedOn + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CI_REJECTION_REMARKS_L1"].ToString().Length > 200 ? (dr["CI_REJECTION_REMARKS_L1"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CI_REJECTION_REMARKS_L1"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    }
                    if (strUserType.Equals("L2"))
                    {
                        if (dr["CI_REJECTION_ON_L2"] != null && dr["CI_REJECTION_ON_L2"] != DBNull.Value)
                            strApprovedOn = ((DateTime)dr["CI_REJECTION_ON_L2"]).ToString("dd-MMM-yyyy");

                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CI_REJECTION_BY_L2"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + strApprovedOn + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CI_REJECTION_REMARKS_L2"].ToString().Length > 200 ? (dr["CI_REJECTION_REMARKS_L2"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CI_REJECTION_REMARKS_L2"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    }
                }
                else if (strActionType.Equals("Rejection") && strIssueType.Equals("Final"))
                {
                    if (strUserType.Equals("UH"))
                    {
                        if (dr["CI_REJECTION_ON_UH"] != null && dr["CI_REJECTION_ON_UH"] != DBNull.Value)
                            strApprovedOn = ((DateTime)dr["CI_REJECTION_ON_UH"]).ToString("dd-MMM-yyyy");

                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CI_REJECTION_BY_UH"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + strApprovedOn + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CI_REJECTION_REMARKS_UH"].ToString().Length > 200 ? (dr["CI_REJECTION_REMARKS_UH"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CI_REJECTION_REMARKS_UH"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    }
                    if (strUserType.Equals("L0"))
                    {
                        if (dr["CI_REJECTION_ON_L0"] != null && dr["CI_REJECTION_ON_L0"] != DBNull.Value)
                            strApprovedOn = ((DateTime)dr["CI_REJECTION_ON_L0"]).ToString("dd-MMM-yyyy");

                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CI_REJECTION_BY_L0"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + strApprovedOn + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CI_REJECTION_REMARKS_L0"].ToString().Length > 200 ? (dr["CI_REJECTION_REMARKS_L0"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CI_REJECTION_REMARKS_L0"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    }

                    if (strUserType.Equals("L1"))
                    {
                        if (dr["CI_APPROVAL_ON_L1"] != null && dr["CI_APPROVAL_ON_L1"] != DBNull.Value)
                            strApprovedOn = ((DateTime)dr["CI_APPROVAL_ON_L1"]).ToString("dd-MMM-yyyy");

                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CI_APPROVAL_BY_L1"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + strApprovedOn + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CI_APPROVAL_REMARKS_L1"].ToString().Length > 200 ? (dr["CI_APPROVAL_REMARKS_L1"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CI_REJECTION_REMARKS_L1"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    }
                    if (strUserType.Equals("L2"))
                    {
                        if (dr["CI_APPROVAL_ON_L2"] != null && dr["CI_APPROVAL_ON_L2"] != DBNull.Value)
                            strApprovedOn = ((DateTime)dr["CI_APPROVAL_ON_L2"]).ToString("dd-MMM-yyyy");

                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CI_APPROVAL_BY_L2"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + strApprovedOn + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CI_APPROVAL_REMARKS_L2"].ToString().Length > 200 ? (dr["CI_APPROVAL_REMARKS_L2"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CI_REJECTION_REMARKS_L2"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    }

                }
                else if (strActionType.Equals("Approval") && strIssueType.Equals("Draft"))
                {
                    if (dr["ApprovalOn"] != null && dr["ApprovalOn"] != DBNull.Value)
                        strApprovedOn = ((DateTime)dr["ApprovalOn"]).ToString("dd-MMM-yyyy");

                    sbHTML.Append("<td " + strTableCellCSS + ">" + dr["ApprovalBy"].ToString() + "</td >" +
                                  "<td " + strTableCellCSS + ">" + strApprovedOn + "</td >");

                    if (!strUserType.Equals("RM1"))
                    {
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["ApprovalRemarks"].ToString().Length > 200 ? (dr["ApprovalRemarks"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["ApprovalRemarks"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    }
                }
                else if (strActionType.Equals("Approval") && strIssueType.Equals("Final"))
                {
                    if (strUserType.Equals("UH"))
                    {
                        if (dr["CI_APPROVAL_ON_UH"] != null && dr["CI_APPROVAL_ON_UH"] != DBNull.Value)
                            strApprovedOn = ((DateTime)dr["CI_APPROVAL_ON_UH"]).ToString("dd-MMM-yyyy");

                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CI_APPROVAL_BY_UH"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + strApprovedOn + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CI_APPROVAL_REMARKS_UH"].ToString().Length > 200 ? (dr["CI_APPROVAL_REMARKS_UH"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CI_APPROVAL_REMARKS_UH"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    }

                    if (strUserType.Equals("L0"))
                    {
                        if (dr["CI_APPROVAL_ON_L0"] != null && dr["CI_APPROVAL_ON_L0"] != DBNull.Value)
                            strApprovedOn = ((DateTime)dr["CI_APPROVAL_ON_L0"]).ToString("dd-MMM-yyyy");

                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CI_APPROVAL_BY_L0"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + strApprovedOn + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CI_APPROVAL_REMARKS_L0"].ToString().Length > 200 ? (dr["CI_APPROVAL_REMARKS_L0"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CI_APPROVAL_REMARKS_L0"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    }

                    if (strUserType.Equals("L1"))
                    {
                        if (dr["CI_APPROVAL_ON_L1"] != null && dr["CI_APPROVAL_ON_L1"] != DBNull.Value)
                            strApprovedOn = ((DateTime)dr["CI_APPROVAL_ON_L1"]).ToString("dd-MMM-yyyy");

                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CI_APPROVAL_BY_L1"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + strApprovedOn + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CI_APPROVAL_REMARKS_L1"].ToString().Length > 200 ? (dr["CI_APPROVAL_REMARKS_L1"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CI_APPROVAL_REMARKS_L1"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    }
                    if (strUserType.Equals("L2"))
                    {
                        if (dr["CI_APPROVAL_ON_L2"] != null && dr["CI_APPROVAL_ON_L2"] != DBNull.Value)
                            strApprovedOn = ((DateTime)dr["CI_APPROVAL_ON_L2"]).ToString("dd-MMM-yyyy");

                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CI_APPROVAL_BY_L2"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + strApprovedOn + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (dr["CI_APPROVAL_REMARKS_L2"].ToString().Length > 200 ? (dr["CI_APPROVAL_REMARKS_L2"] as string).Substring(0, 200).Replace(Environment.NewLine, "<br />") + "..." : dr["CI_APPROVAL_REMARKS_L2"].ToString().Replace(Environment.NewLine, "<br />")) + "</td>");
                    }

                }

                sbHTML.Append("<td " + strTableCellCSS + ">" + dr["DraftIssuesStatus"].ToString() + "</td></tr>");
            }

            sbHTML.Append("</table>");
            return sbHTML.ToString();
        }



        private string getHTMLTableForIssuesIdentifiedCnt(string strIds, string strType, string strUserType)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            StringBuilder sbHTML = new StringBuilder();

            sbHTML.Append("<table " + strTableCntCSS + "><tr><th " + strTableHeaderCSS + ">Sr.No.</th>");

            if (strUserType.Equals("US"))
            {
                sbHTML.Append("<th " + strTableHeaderCSS + ">Pending Issues Count</th>");
                sbHTML.Append("<th " + strTableHeaderCSS + ">SPOC Responsible</th>");
            }

            sbHTML.Append("</tr>");

            if (strType.Equals("Draft"))
            {
                if (strUserType.Equals("US"))
                {
                    //dt = rrUtilBLL.GetDataTable("getPendingDraftIssueTrackerBySPOCCount",
                    //                            new DBUtilityParameter("RDI_RR_ID", strIds));
                }
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i - 1];

                    sbHTML.Append("<tr><td " + strTableCellCSS + ">" + i + "</td>");

                    if (strUserType.Equals("US"))
                    {
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["Cnt"].ToString() + "</td>");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["RDI_SPOC_RESPONSIBLE"].ToString() + "</td>");
                    }

                    sbHTML.Append("</tr>");
                }

                strIsValidated = "false";
            }
            else
            {
                sbHTML.Append("<tr><td " + strTableCellCSS + " colspan='3'> No Issues are pending with SPOCs. </td ></tr>");
                strIsValidated = "true";
            }

            sbHTML.Append("</table>");
            return sbHTML.ToString();
        }


        private string getUnitHead1(string strUnitId)
        {
            string strUH1EmailId = "";
            DataTable dtUH = new DataTable();
            DataRow drUH;

            if (strUnitId.Contains(","))
                dtUH = rrBLL.GetDataTable("getUnits", new DBUtilityParameter("1", " 1 AND CSFM_ID IN (" + strUnitId + ")", oSubQuery: 1));
            else
                dtUH = rrBLL.GetDataTable("getUnits", new DBUtilityParameter("CSFM_ID", strUnitId));

            for (int j = 0; j < dtUH.Rows.Count; j++)
            {
                drUH = dtUH.Rows[j];
                strUH1EmailId += drUH["CSFM_UNIT_HEAD_EMAIL"].ToString() + "|";
            }
            return strUH1EmailId;
        }

        private string getReviewerMasUsers(string strId, string strUserType)
        {
            string strEmailId = "";
            DataRow dr;
            DataTable dt = rrBLL.GetDataTable("getReviewMaster", new DBUtilityParameter("CRM_ID", strId));

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                dr = dt.Rows[j];

                if (strUserType.Equals("L0"))
                    strEmailId = dr["CRM_L0_REVIEWER_EMAIL"].ToString();
                else if (strUserType.Equals("L1"))
                    strEmailId = dr["CRM_L1_REVIEWER_EMAIL"].ToString();
                else if (strUserType.Equals("L2"))
                    strEmailId = dr["CRM_L2_REVIEWER_EMAIL"].ToString();
            }
            return strEmailId + "|";
        }
    }
}