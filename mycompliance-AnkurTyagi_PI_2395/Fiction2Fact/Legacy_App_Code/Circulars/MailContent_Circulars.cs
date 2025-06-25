using System;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Collections;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.App_Code;
using System.Text;

namespace Fiction2Fact.Legacy_App_Code.Circulars
{
    public class MailContent_Circulars
    {
        MailConfigBLL mailBLL = new MailConfigBLL();

        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSql"].ConnectionString;

        string strTableCellCSS = " style=\"font-size: 12px; font-family: Calibri; border-width: 1px; padding: 8px; border-style: solid;" +
                            "border-color: #bcaf91;\"";

        string strTableHeaderCSS = " style =\"font-size: 12px; font-family: Calibri; background-color: #ded0b0; border-width: 1px; padding: 8px;" +
                                   "border-style: solid; border-color: #bcaf91; text-align: left;\"";

        string strTableCSS = " style=\"font-size: 12px; font-family: Calibri; color: #333333; " +
                            "width: 100%; border-width: 1px; border-color: #bcaf91; border-collapse: collapse;\"";

        private string strTo = "", strCC = "";
        private string strFooter = ConfigurationManager.AppSettings["MailFooter"].ToString();
        public Hashtable ParamMap = new Hashtable();

        public void setCircularMailContent(DataTable dt = null, DataTable dt1 = null)
        {
            DataRow drMail;
            DataTable dtMail = new DataTable();
            Calendar cl = new Calendar();
            string[] strToList = null, strCCList = null, strAttachments = null, strAttachmentNames = null;
            string strSubject = "", strContent = "", strToType = "", strCCType = "", strCircularId = "",
                strCirNo = "", strCreatorId = "", strConfigID = "", strCreatedOn = "", strCirType = "", strCirSubject = "",
                strCirIssAuthority = "", strCirDate = "", strApprovalType = "", strLoggedInUserName = "", strCirIssueType = "",
                strCirSummary = "", strCirImplications = "", strCirAttachment = "", strCirActionable = "",
                strResponsiblePerson = "", strTargetDate = "", strCompletionDate = "", strUpdateStatus = "",
                strUpdateType = "", strUpdateDetails = "", strCirActionableId = "", strRevisedTargetDate = "",
                strActionableClosureDate = "";
            int intAttachmentCount = 0;
            bool res = false;

            try
            {
                if (ParamMap.ContainsKey("ConfigId"))
                {
                    strCreatedOn = cl.TodaysDate.ToString("dd-MMM-yyyy");

                    strConfigID = ParamMap["ConfigId"].ToString();
                    dtMail = mailBLL.searchMailConfig(strConfigID, "", "Circ");

                    if (dtMail.Rows.Count > 0)
                    {
                        drMail = dtMail.Rows[0];

                        if (ParamMap["ApprovalType"] != null)
                        {
                            strApprovalType = ParamMap["ApprovalType"].ToString();
                        }

                        if (ParamMap["LoggedInUserName"] != null)
                        {
                            strLoggedInUserName = ParamMap["LoggedInUserName"].ToString();
                        }

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

                        if (ParamMap["CirType"] != null)
                        {
                            strCirType = ParamMap["CirType"].ToString();
                        }

                        if (ParamMap["CirSubject"] != null)
                        {
                            strCirSubject = ParamMap["CirSubject"].ToString();
                        }

                        if (ParamMap["CirIssAuthority"] != null)
                        {
                            strCirIssAuthority = ParamMap["CirIssAuthority"].ToString();
                        }

                        if (ParamMap["CirDate"] != null)
                        {
                            strCirDate = ParamMap["CirDate"].ToString();
                        }

                        if (ParamMap["CirIssueType"] != null)
                        {
                            strCirIssueType = ParamMap["CirIssueType"].ToString();
                        }

                        if (ParamMap["CirSummary"] != null)
                        {
                            strCirSummary = ParamMap["CirSummary"].ToString();
                        }

                        if (ParamMap["CirImplications"] != null)
                        {
                            strCirImplications = ParamMap["CirImplications"].ToString();
                        }

                        if (ParamMap["CirAttachment"] != null)
                        {
                            strCirAttachment = ParamMap["CirAttachment"].ToString();
                        }

                        if (ParamMap["AttachmentCount"] != null)
                        {
                            res = int.TryParse(ParamMap["AttachmentCount"].ToString(), out intAttachmentCount);
                        }

                        if (ParamMap["Attachments"] != null)
                        {
                            strAttachments = ParamMap["Attachments"].ToString().Split(',');
                        }

                        if (ParamMap["AttachmentNames"] != null)
                        {
                            strAttachmentNames = ParamMap["AttachmentNames"].ToString().Split(',');
                        }

                        if (ParamMap["CreatorId"] != null)
                        {
                            strCreatorId = ParamMap["CreatorId"].ToString();
                        }

                        if (ParamMap["RevisedTargetDate"] != null)
                        {
                            strRevisedTargetDate = ParamMap["RevisedTargetDate"].ToString();
                        }

                        if (ParamMap["ActionableClosureDate"] != null)
                        {
                            strActionableClosureDate = ParamMap["ActionableClosureDate"].ToString();
                        }

                        strSubject = drMail["MCM_SUBJECT"].ToString();
                        strContent = drMail["MCM_CONTENT"].ToString();

                        strToType = ParamMap["To"].ToString();
                        strCCType = ParamMap["cc"].ToString();

                        strSubject = strSubject.Replace("%CirType%", strCirType);
                        strSubject = strSubject.Replace("%CirSubject%", strCirSubject);

                        strContent = strContent.Replace("%CirType%", strCirType);
                        strContent = strContent.Replace("%CirIssAuthority%", strCirIssAuthority);
                        strContent = strContent.Replace("%CirDate%", strCirDate);
                        strContent = strContent.Replace("%CirSubject%", strCirSubject);

                        if (strConfigID.Equals("1085"))
                        {
                            strContent = strContent.Replace("%CirCertChecklists%", getHTMLTableForCertChecklist(dt));
                            strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/Circulars/MyCertChecklists.aspx") + "'>Click here</a>");
                        }
                        else if (strConfigID.Equals("1086"))
                        {
                            strSubject = strSubject.Replace("%CirApprovalType%", (strApprovalType.Equals("A") ? "accepted" : "rejected"));

                            strContent = strContent.Replace("%CirApprovalType%", (strApprovalType.Equals("A") ? "accepted" : "rejected"));
                            strContent = strContent.Replace("%CirCertChecklists%", getHTMLTableForCertChecklist(dt, strLoggedInUserName, strApprovalType));
                            strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/Circulars/CertChecklistsApproval.aspx") + "'>Click here</a>");
                        }
                        else if (strConfigID.Equals("1087"))
                        {
                            strContent = strContent.Replace("%CirCertChecklists%", getHTMLTableForCertChecklist(dt, strLoggedInUserName, strApprovalType));
                            strContent = strContent.Replace("%Link%", "<a href='" + Global.site_url("Projects/Circulars/MyCertChecklists.aspx") + "'>Click here</a>");
                        }
                        else if (strConfigID.Equals("1088"))
                        {
                            strContent = strContent.Replace("%CirCertChecklists%", getHTMLTableForCertChecklist(dt, strLoggedInUserName, strApprovalType));
                        }
                        else if (strConfigID.Equals("29"))
                        {
                            strContent = strContent.Replace("%CirActionPoints%", getHTMLTableForActionables(dt));
                            strContent = strContent.Replace("%CirActionLink%", "<a href=" + Global.site_url("Projects/Circulars/MyActionables.aspx") + " target=\"_blank\">Click here</a>");
                        }
                        else if (strConfigID.Equals("1091"))
                        {
                            strContent = strContent.Replace("%CirActionPoints%", getHTMLTableForActionables(dt));
                            strContent = strContent.Replace("%CirActionLink%", "<a href=" + Global.site_url("Projects/Circulars/MyActionables.aspx") + " target=\"_blank\">Click here</a>");
                        }
                        else if (strConfigID.Equals("1092"))
                        {
                            strContent = strContent.Replace("%CirActionPoints%", getHTMLTableForActionables(dt));
                            strContent = strContent.Replace("%CirActionLink%", "<a href=" + Global.site_url("Projects/Circulars/MyActionables.aspx") + " target=\"_blank\">Click here</a>");
                        }
                        else if (strConfigID.Equals("28"))
                        {
                            string strActionableTable = getHTMLTableForActionables(dt);
                            string strChecklistTable = getHTMLTableForCertChecklist(dt1);
                            strContent = strContent.Replace("%CirIssueType%", strCirIssueType);
                            strContent = strContent.Replace("%CirSummary%", strCirSummary);
                            strContent = strContent.Replace("%CirImplications%", strCirImplications);
                            strContent = strContent.Replace("%CirAttachment%", strCirAttachment);
                            strContent = strContent.Replace("%CirActionPoints%", string.IsNullOrEmpty(strActionableTable) ? "" : "Actionables" + strActionableTable);
                            strContent = strContent.Replace("%CirCertChecklists%", (string.IsNullOrEmpty(strChecklistTable) ? "" : "Certification Checklist" + strChecklistTable));
                            strContent = strContent.Replace("%CirActionPoints%", "");
                            strContent = strContent.Replace("%CirCertChecklists%", "");
                            //Projects / Circulars / ViewCircular.aspx
                            strContent = strContent.Replace("%Link%", "<a href=" + Global.site_url("Projects/Circulars/ViewCircular.aspx") + " target=\"_blank\">Click here</a>");
                        }
                        else if (strConfigID.Equals("30"))
                        {
                            strSubject = strSubject.Replace("%UpdatedBy%", strLoggedInUserName);

                            strContent = strContent.Replace("%UpdatedBy%", strLoggedInUserName);
                            strContent = strContent.Replace("%UpdatedDate%", strCreatedOn);
                            strContent = strContent.Replace("%CirNo%", strCirNo);
                            strContent = strContent.Replace("%CirActionable%", strCirActionable.Replace(Environment.NewLine, "<br />"));
                            strContent = strContent.Replace("%ResponsiblePerson%", strResponsiblePerson);
                            strContent = strContent.Replace("%TargetDate%", strTargetDate);
                            strContent = strContent.Replace("%CompletionDate%", strCompletionDate);
                            strContent = strContent.Replace("%UpdateStatus%", strUpdateStatus);
                            strContent = strContent.Replace("%UpdateType%", strUpdateType);
                            strContent = strContent.Replace("%UpdateDetails%", strUpdateDetails.Replace(Environment.NewLine, "<br />"));
                            strContent = strContent.Replace("%RevisedTargetDate%", strRevisedTargetDate);
                            strContent = strContent.Replace("%ActionableClosureDate%", strActionableClosureDate);
                            strContent = strContent.Replace("%ActionLink%", "<a href=\"" + Global.site_url("Projects/Circulars/EditCircularActionables.aspx?ActionableId=" + strCirActionableId +
                                        "&CircularId=" + strCircularId) + "\" target=\"_blank\">Click here</a> to view the actionable.");
                        }

                        strContent = strContent.Replace("%Footer%", strFooter);

                        setMailIds("TO", strToType, strCreatorId);
                        setMailIds("CC", strCCType, strCreatorId);

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

                        if (intAttachmentCount.Equals(0))
                            mm.sendAsyncMail(strToList, strCCList, null, strSubject, strContent);
                        else
                            mm.sendMailWithAttach(strToList, strCCList, null, strSubject, strContent, strAttachments, strAttachmentNames);
                    }
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }
        }

        private string getHTMLTableForCertChecklist(DataTable dt, string strLoggedInUserName = "", string strApprovalType = "")
        {
            StringBuilder sbHTML = new StringBuilder();
            DataRow dr;

            try
            {
                if (dt.Rows.Count > 0)
                {
                    sbHTML.Append("<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr. No.</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Department Name</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Act/Regulation/Circular</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Reference Circular / Notification / Act</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Section/Clause</th>");
                    //sbHTML.Append("<th " + strTableHeaderCSS + ">Compliance of/Heading of Compliance checklist</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Particulars</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Description</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Consequences of non Compliance</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Frequency</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Forms</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Effective From</th>");

                    if (strApprovalType.Equals("A"))
                    {
                        sbHTML.Append("<th " + strTableHeaderCSS + ">Accepted by</th>");
                        sbHTML.Append("<th " + strTableHeaderCSS + ">Accepted On</th>");
                        sbHTML.Append("<th " + strTableHeaderCSS + ">Acceptance Remarks</th>");
                    }
                    else if (strApprovalType.Equals("R"))
                    {
                        sbHTML.Append("<th " + strTableHeaderCSS + ">Rejected by</th>");
                        sbHTML.Append("<th " + strTableHeaderCSS + ">Rejected On</th>");
                        sbHTML.Append("<th " + strTableHeaderCSS + ">Rejection Remarks</th>");
                    }
                    else if (strApprovalType.Equals("MTCC") || strApprovalType.Equals("STS"))
                    {
                        sbHTML.Append("<th " + strTableHeaderCSS + ">Compliance Remarks</th>");
                    }

                    sbHTML.Append("</tr>");

                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        dr = dt.Rows[i - 1];
                        sbHTML.Append("<tr><td " + strTableCellCSS + ">" + i + "</td>");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["DeptName"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["ActRegCirc"].ToString().Replace("\n", "<br />") + "</td>");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["Reference"].ToString().Replace("\n", "<br />") + "</td>");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["Clause"].ToString().Replace("\n", "<br />") + "</td>");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["CheckPoints"].ToString().Replace("\n", "<br />") + "</td>");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["Particulars"].ToString().Replace("\n", "<br />") + "</td>");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["Penalty"].ToString().Replace("\n", "<br />") + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["Frequency"].ToString().Replace("\n", "<br />") + "</td>");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["Forms"].ToString().Replace("\n", "<br />") + "</td>");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (string.IsNullOrEmpty(dr["EffectiveFrom"] as string) ? "" : CommonCodes.DbToDispDate(dr["EffectiveFrom"].ToString())) + "</td>");

                        if (strApprovalType.Equals("A") || strApprovalType.Equals("R"))
                        {
                            sbHTML.Append("<td " + strTableCellCSS + ">" + strLoggedInUserName + "</td >");
                            sbHTML.Append("<td " + strTableCellCSS + ">" + CommonCodes.DbToDispDateTime(DateTime.Now.ToString()) + "</td>");
                            sbHTML.Append("<td " + strTableCellCSS + ">" + dr["Comments"].ToString().Replace("\n", "<br />") + "</td>");
                        }
                        else if (strApprovalType.Equals("MTCC") || strApprovalType.Equals("STS"))
                        {
                            sbHTML.Append("<td " + strTableCellCSS + ">" + dr["Comments"].ToString().Replace("\n", "<br />") + "</td>");
                        }

                        sbHTML.Append("</tr>");
                    }

                    sbHTML.Append("</table>");
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }

            return sbHTML.ToString();
        }

        private string getHTMLTableForActionables(DataTable dt)
        {
            StringBuilder sbHTML = new StringBuilder();
            DataRow dr;

            try
            {
                if (dt.Rows.Count > 0)
                {
                    sbHTML.Append("<table " + strTableCSS + "><tr><th " + strTableHeaderCSS + ">Sr. No.</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Actionable</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Responsible Function</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Person/Function Responsible</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Status</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Target Date</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Completion Date</th>");
                    sbHTML.Append("<th " + strTableHeaderCSS + ">Remarks</th>");
                    sbHTML.Append("</tr>");

                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        dr = dt.Rows[i - 1];
                        sbHTML.Append("<tr><td " + strTableCellCSS + ">" + i + "</td>");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["Actionable"].ToString().Replace("\n", "<br />") + "</td>");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["ResFuncName"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["PerResp"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["StatusName"].ToString() + "</td >");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (string.IsNullOrEmpty(dr["TargetDate"] as string) ? "" : CommonCodes.DbToDispDate(dr["TargetDate"].ToString())) + "</td>");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + (string.IsNullOrEmpty(dr["ComplDate"] as string) ? "" : CommonCodes.DbToDispDate(dr["ComplDate"].ToString())) + "</td>");
                        sbHTML.Append("<td " + strTableCellCSS + ">" + dr["Remarks"].ToString().Replace("\n", "<br />") + "</td>");
                        sbHTML.Append("</tr>");
                    }

                    sbHTML.Append("</table>");
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                if (F2FLog.F2FEnvironment != "PRODUCTION") throw ex;
            }

            return sbHTML.ToString();
        }

        private string getUserMailIdByUserId(string strUserId)
        {
            string strUserMailId = "";
            MembershipUser membershipUser = Membership.GetUser(strUserId);
            if (membershipUser != null)
            {
                strUserMailId = membershipUser.Email + "|";
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

        private void setMailIds(string strToCCType, string strType, string strCreatorId)
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

                        case "CircularUser":
                            strReturn = strReturn + getUserMailIds("CircularUser");
                            break;

                        case "CircularAdmin":
                            strReturn = strReturn + getUserMailIds("CircularAdmin");
                            break;

                        case "CertCompUser":
                            strReturn = strReturn + getUserMailIds("Certification_Compliance_User");
                            break;

                        case "CertAdmin":
                            strReturn = strReturn + getUserMailIds("Certification_Admin");
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