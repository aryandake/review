using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Data.OleDb;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using Fiction2Fact.F2FControls;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_CertificationSubmissions : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ToString();
        int intCId = 0;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CertificationMasterBL certBL = new CertificationMasterBL();
        DataTable dtException;
        DataTable dt_checklist = new DataTable();
        CommonMethods cm = new CommonMethods();
        string strUser, strIAgree = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                strUser = Authentication.GetUserID(Page.User.Identity.Name);

                if (!Page.IsPostBack)
                {
                    //getCertification();
                    PnlCertStatus.Visible = false;
                    getDepartmentName();
                }
                else
                {
                    if (Session["Checklist"] != null)
                    {
                        dt_checklist = (DataTable)Session["Checklist"];
                    }
                    else
                    {
                        BindChecklist();
                    }
                }
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Error in Page_Load:" + ex.Message);
            }
        }

        public void getDepartmentName()
        {
            ListItem li = new ListItem();
            li.Text = "(Select)";
            li.Value = "";
            ddlDepartmentName.DataSource = utilBL.getDataset("CERTIFICATEDEPT", strConnectionString);
            ddlDepartmentName.DataBind();
            ddlDepartmentName.Items.Insert(0, li);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            hfCertStatus.Value = "";
            getCertification();
        }

        private void getCertification()
        {
            string strContent;
            string strFromDate = "";
            string strToDate = "";
            string strDesg = "";
            string strName = "";

            DataSet dsDates;
            DataTable dtDates, dtCert;
            DataRow drDates, drowCert;
            DateTime dtCertDate = System.DateTime.Now;
            strContent = "";
            //chkIAgree.Attributes.Add("OnClick", "return onIAgreeChecked()");

            //DataTable dtCntDr = utilBL.getDatasetWithConditionInString("certStatus",
            //    strUser, strConnectionString);
            DataTable dtCntDr = utilBL.getDatasetWithConditionInString("certStatusBbyDeptId",
                ddlDepartmentName.SelectedValue, strConnectionString);

            //<<Certification is not created in TBL_CERTIFICATIONS.
            if (dtCntDr.Rows.Count == 0)
            {

                //<<Get Active Quarter.
                dsDates = utilBL.getDataset("CERTQUARTERS", strConnectionString);
                dtDates = dsDates.Tables[0];
                if (dtDates.Rows.Count == 1)
                {
                    drDates = dtDates.Rows[0];
                    strFromDate = (Convert.ToDateTime(drDates["CQM_FROM_DATE"].ToString())).ToString("dd-MMM-yyyy");
                    strToDate = (Convert.ToDateTime(drDates["CQM_TO_DATE"].ToString())).ToString("dd-MMM-yyyy");
                    hfQuarterEndDt.Value = strToDate.ToString();
                    hfQuarterId.Value = drDates["CQM_ID"].ToString();
                }
                //>>

                //<<Get Content from Certification Master.
                //dtCert = utilBL.getDatasetWithConditionInString("CERTCONTENT", strUser,
                //         strConnectionString);
                dtCert = utilBL.getDatasetWithConditionInString("CERTCONTENTbyDeptId", ddlDepartmentName.SelectedValue,
                         strConnectionString);

                //if (dtCert.Rows.Count == 1)
                if (dtCert.Rows.Count >= 1)
                {
                    PnlCertStatus.Visible = true;
                    drowCert = dtCert.Rows[0];
                    hfCertMId.Value = drowCert["CERTM_ID"].ToString();
                    strName = drowCert["CDO_NAME"].ToString();
                    hfCertDepartment.Value = drowCert["CDM_NAME"].ToString();
                    hfUserFullName.Value = strName;
                    hfDepartmentID.Value = drowCert["CDM_ID"].ToString();

                    strDesg = drowCert["CDO_DESG"].ToString();
                    strContent = drowCert["CERTM_TEXT"].ToString();
                    strContent = strContent.Replace("~qtrstartdate ", strFromDate);
                    strContent = strContent.Replace("~qtrenddate ", strToDate);

                    strContent = strContent.Replace("~name", strName);
                    strContent = strContent.Replace("~desg", strDesg);
                    strContent = strContent.Replace("~date", dtCertDate.ToString("dd-MMM-yyyy"));
                    lblCertContents.Text = strContent;


                    litControls.Text = "<script type='text/javascript' src='../js/Exception.js'></script>" +
                                      " <table id='tblException' width='100%'> " +
                                    " <thead> " +
                                    " <tr> " +
                                    " <th class='tabhead3' align='left' width='2%' > " +
                                    " <input type='checkbox' ID='HeaderLevelCheckBox' onclick = 'return onHeaderRowChecked()' /> " +
                                    " </th> " +
                                    " <th class='tabhead3' width='2%'> " +
                                    " Attach " +
                                    " </th> " +
                                    " <th class='tabhead3' width='10%'> " +
                                    " File Name " +
                                    " </th> " +
                                    " <th class='tabhead3' width='15%'> " +
                                    " Exception Type " +
                                    " </th> " +
                                    " <th class='tabhead3' width='40%'> " +
                                    " Details " +
                                    " </th> " +
                                    " </tr> " +
                                    " </thead> " +
                                    " </table> ";
                    BindChecklist();
                }
                else if (dtCert.Rows.Count == 0)
                {
                    PnlCertStatus.Visible = false;
                    lblMsg.Text = "No Certification content found. Please contact the administrator.";
                }
                //<< Commented by Bhavik @ 1-Oct-2013 for Multiple Certification
                //else
                //{
                //    PnlCertStatus.Visible = false;
                //    lblMsg.Text = "Multiple Certification contents found. Please contact the administrator.";
                //}
                //>>
            }
            //>>
            else
            {
                PnlCertStatus.Visible = true;
                DataRow drCntDr = dtCntDr.Rows[0];
                string strStatus = drCntDr["CERT_STATUS"].ToString();
                hfCertStatus.Value = drCntDr["CERT_STATUS"].ToString();
                //<<Certification is already submitted.
                //if (strStatus.Equals("S"))
                //{
                //    PnlCertStatus.Visible = false;
                //    lblMsg.Text = "You've already certified for the current quarter. You can " +
                //        "view the same under My Certifications (Historical).";
                //}
                //>>
                //<<Certification exists but is in save draft mode.s
                //else 
                if (strStatus.Equals("D"))
                {
                    hfQuarterEndDt.Value = (Convert.ToDateTime(drCntDr["CQM_TO_DATE"].ToString())).ToString("dd-MMM-yyyy");
                    hfCertDepartment.Value = drCntDr["CDM_NAME"].ToString();
                    hfCertId.Value = drCntDr["CERT_ID"].ToString();
                    txtRemarks.Text = drCntDr["CERT_REMARKS"].ToString();
                    strContent = drCntDr["CERT_CONTENT"].ToString();
                    hfQuarterId.Value = drCntDr["CERT_CQM_ID"].ToString();
                    hfCertMId.Value = drCntDr["CERT_CERTM_ID"].ToString();
                    lblCertContents.Text = strContent;
                    hfDepartmentID.Value = drCntDr["CDM_ID"].ToString();

                    //<<List of Exceptions.
                    DataTable dtExc = utilBL.getDataset("getDraftedException", strConnectionString).Tables[0];
                    DataRow drExc;
                    int intExcCnt = dtExc.Rows.Count;
                    string strCEId, strException, strDetails, strClientFile, strServerFile;
                    int uniqueRowId = 0;
                    string strHtmlTableRows = "";
                    string strHtmlTable = "<script type='text/javascript' src='../js/Exception.js'></script>" +
                                " <table id='tblException' width='100%'> " +
                                 " <thead> " +
                                 " <tr> " +
                                 " <th class='tabhead4' align='center'> " +
                                 " <input type='checkbox' ID='HeaderLevelCheckBox' onclick = 'return onHeaderRowChecked()' /> " +
                                 " </th> " +
                                 " <th class='tabhead4'> " +
                                 " Attach " +
                                 " </th> " +
                                 " <th class='tabhead4'> " +
                                 " File Name " +
                                 " </th> " +
                                 " <th class='tabhead4'> " +
                                 " Exception Type " +
                                 " </th> " +
                                 " <th class='tabhead4'> " +
                                 " Details " +
                                 " </th> " +
                                 " </tr> " +
                                 " </thead> ";

                    for (int intCnt = 0; intCnt < intExcCnt; intCnt++)
                    {
                        uniqueRowId = uniqueRowId + intCnt;
                        drExc = dtExc.Rows[intCnt];
                        strCEId = drExc["CE_ID"].ToString();
                        strException = drExc["CE_EXCEPTION_TYPE"].ToString();
                        strDetails = drExc["CE_DETAILS"].ToString();
                        strClientFile = drExc["CE_CLIENT_FILE_NAME"].ToString();
                        strServerFile = drExc["CE_SERVER_FILE_NAME"].ToString();

                        string strVisibilityAttach = "style='visibility:hidden'";
                        string strVisibilityDelete = "style='visibility:hidden'";

                        if (strClientFile.Equals(""))
                        {
                            strVisibilityAttach = "style='visibility:visible'";
                            strVisibilityDelete = "style='visibility:hidden'";
                        }
                        else
                        {
                            strVisibilityAttach = "style='visibility:hidden'";
                            strVisibilityDelete = "style='visibility:visible'";
                        }

                        strHtmlTableRows = strHtmlTableRows + "<tr><td class='tabbody3'>" +
                        "<input type='hidden' ID='uniqueRowId" + uniqueRowId + "' value='" + uniqueRowId + "' /><input type='hidden' ID='certId" + uniqueRowId + "' value='" + strCEId + "' /><input type='checkbox' ID='cbException" + uniqueRowId + "' /></td>" +
                        "<td class='tabbody3'><input type='hidden' ID='ServerFileName" + uniqueRowId + "' value='" + strServerFile + "' /><input type='hidden' ID='ClientFileName" + uniqueRowId + "' value='" + strClientFile + "' /><a ID='AttachFileImg" + uniqueRowId + "' onclick='return openpopupExceptionAttachments(" + uniqueRowId + ")' " + strVisibilityAttach + "><img border='0' src='../../Content/images/legacy/attach.png' /></a> " +
                        " <a ID='DeleteFileImg" + uniqueRowId + "' onclick='return deleteExceptionFile(" + uniqueRowId + ")' " + strVisibilityDelete + "><img border='0' src='../../Content/images/legacy/delete.gif' /></a></td>" +
                        "<td class='tabbody3'><a ID='Filelink" + uniqueRowId + "' href='../DownloadFileCertification.aspx?FileInformation=" + strServerFile + "' >" + strClientFile + "</a></td>" +
                        "<td class='tabbody3'><input size = '50' type='text' ID='ExceptionType" + uniqueRowId + "' maxLength = '100' class = 'textbox1' value='" + strException + "' /></td>" +
                        "<td class='tabbody3'><textarea ID='Details" + uniqueRowId + "' maxLength = '500' cols='50' rows='3' class = 'textbox1'>" + strDetails + "</textarea></td>" +
                        "</tr>";
                    }

                    litControls.Text = strHtmlTable + strHtmlTableRows + " </table> ";
                    BindChecklist();
                }
            }
            //BindChecklist();
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        private void saveCertification(string strStatus)
        {
            string strContent;
            string strRemarks;
            string strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
            strContent = lblCertContents.Text;
            strRemarks = cm.getSanitizedString(txtRemarks.Text);
            string ExceptionNames = hfExceptions.Value;
            string[] strarrExcp, strarrExcpFields;
            string strTemp;
            DataRow dr;
            initExceptiondt();
            strarrExcp = ExceptionNames.Split('~');
            for (int i = 0; i < strarrExcp.Length - 1; i++)
            {
                strTemp = strarrExcp[i];
                strarrExcpFields = strTemp.Split('|');
                dr = dtException.NewRow();
                dr["ID"] = strarrExcpFields[1];
                dr["Exception Type"] = strarrExcpFields[2];
                dr["Details"] = strarrExcpFields[3];
                dr["Client File Name"] = strarrExcpFields[4];
                dr["Server File Name"] = strarrExcpFields[5];
                dtException.Rows.Add(dr);
            }

            //Added by Bhavik @ 10-Sep-2013

            int intCertId = 0;
            if (!hfCertId.Value.Equals(""))
            {
                intCertId = Convert.ToInt32(hfCertId.Value);
            }
            //intCId = certBL.saveCertification(intCertId, Convert.ToInt32(hfCertMId.Value),
            //                Convert.ToInt32(hfQuarterId.Value),
            //                strContent, strRemarks, strStatus, strCreateBy, dtException, strConnectionString);

            //intCId = certBL.saveCertification(intCertId, Convert.ToInt32(hfCertMId.Value),
            //                Convert.ToInt32(hfQuarterId.Value),
            //                strContent, strRemarks, strStatus, strCreateBy, dtException, dt_checklist, strConnectionString);

            certBL.saveCertification(intCertId, Convert.ToInt32(hfCertMId.Value),
                            Convert.ToInt32(hfQuarterId.Value),
                            strContent, strRemarks, strStatus, strCreateBy, dtException, getChecklistDets(), "", Page.User.Identity.Name);

            lblMsg.Text = "Certification saved successfully.";
            PnlCertStatus.Visible = false;
        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            try
            {
                dt_checklist = null;
                saveCertification("D");

            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Error in btnSaveDraft_Click: " + exp.Message);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //BindChecklist();
                saveCertification("S");
                sendCertificateMail(intCId, lblCertContents.Text, " accepted ");

                //<< Code Commented by Bhavik @ 04Aug2014 
                ////<< get Departments not certified.
                //DataTable dtDept = utilBL.getDataset("getDeptNotCertified", strConnectionString).Tables[0];
                //DataRow drDept;

                //if (dtDept.Rows.Count.Equals(0))
                //{
                //    sendCHApprovalMail();
                //}
                //>>
                writeError("Your certification has been submitted to secretarial department successfully. ");
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Error in btnSubmit_Click: " + exp.Message);
            }
        }


        //<< Added by Nikhil Adhalikar on 19-Aug-2011
        private void initExceptiondt()
        {
            dtException = new DataTable();
            dtException.Columns.Add(new DataColumn("ID", typeof(string)));
            dtException.Columns.Add(new DataColumn("Exception Type", typeof(string)));
            dtException.Columns.Add(new DataColumn("Details", typeof(string)));
            dtException.Columns.Add(new DataColumn("Client File Name", typeof(string)));
            dtException.Columns.Add(new DataColumn("Server File Name", typeof(string)));
        }
        //>>
        private void sendCertificateMail(int intCId, string strContent, string strStatus)
        {
            try
            {
                string[] strUsers = new string[0];
                string[] strUsers1 = new string[0];
                string[] strTo = new string[0];
                string[] strCC = new string[1];
                string strSubject;
                string strHostingServer, strFooter;
                string strMailContent, strQuarterEndDt = "", strCertDepartment = "";
                DateTime dt = System.DateTime.Now;
                DataTable dtGetDeptOwner = new DataTable();
                DataRow drGetDeptOwner;
                string strSql = "";
                Mail mm = new Mail();
                MembershipUser user;

                strQuarterEndDt = hfQuarterEndDt.Value.ToString();
                strCertDepartment = hfCertDepartment.Value.ToString();

                strHostingServer = ConfigurationManager.AppSettings["HostingServer"].ToString();
                strFooter = ConfigurationManager.AppSettings["MailFooter"].ToString();
                strSubject = "Certification submission by " + strCertDepartment + " for the quarter ended " + strQuarterEndDt;

                strUsers = Roles.GetUsersInRole("Certification_Admin");
                strTo = new string[strUsers.Length];

                for (int intCount2 = 0; intCount2 < strUsers.Length; intCount2++)
                {
                    user = Membership.GetUser(strUsers[intCount2]);
                    strTo[intCount2] = user.Email;
                }

                //<< Added By Bhavik @ 9-Oct-2013
                try
                {
                    strSql = "select * from TBL_CERT_DEPT_OWNERS where CDO_CDM_ID = " + hfDepartmentID.Value.ToString();
                    dtGetDeptOwner = F2FDatabase.F2FGetDataTable(strSql);
                    drGetDeptOwner = dtGetDeptOwner.Rows[0];
                    strCC[0] = drGetDeptOwner["CDO_EMAIL_ID"].ToString();
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("system exception in GetDepartmentOwners() " + ex);
                }

                //>>

                if (intCId == 0)
                    intCId = Convert.ToInt32(hfCertId.Value);

                strMailContent = "<html><head><title>New Certification Submission</title></head> " +
                                      "<body style=\"font-size: 10pt; font-family: Zurich bt\"> <br /> A new Certification (Id: " + intCId +
                                      ") has been submitted on " + dt.ToString("MMM dd,yyyy") + " at " + dt.ToString("hh.mm tt") +
                                       " by " + Authentication.GetUserID(Page.User.Identity.Name) + ".<br />" + "<br /> The Certification is as follows: <br/> " +
                                       strContent +
                                      " " + strFooter + "<br />THIS IS AN AUTO GENERATED MAIL PLEASE DO NOT REPLY BACK.<br /></body></html>";

                mm.sendAsyncMail(strTo, strCC, null, strSubject, strMailContent);

            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Error while sending mail: " + ex.Message);
            }
        }

        private void sendCHApprovalMail()
        {
            try
            {
                string[] strUsers = new string[0];
                string[] strTo = new string[1];
                string[] strCC = new string[0];
                string strSubject;
                string strHostingServer, strFooter;
                string strMailContent;
                DataRow drNextDesg;
                DataTable dtNextDesg = new DataTable();
                DateTime dt = System.DateTime.Now;
                Mail mm = new Mail();
                MembershipUser user;
                strHostingServer = ConfigurationManager.AppSettings["HostingServer"].ToString();
                strFooter = ConfigurationManager.AppSettings["MailFooter"].ToString();
                string strNextLevelDesg = "CH";

                dtNextDesg = utilBL.getDatasetWithConditionInString("getApprovalDets", strNextLevelDesg,
                                    strConnectionString);
                if (dtNextDesg.Rows.Count > 0)
                {
                    drNextDesg = dtNextDesg.Rows[0];
                    hfEmailId.Value = drNextDesg["CDO_EMAIL_ID"].ToString();
                    strTo[0] = hfEmailId.Value.ToString();

                    strUsers = Roles.GetUsersInRole("Certification_Admin");
                    strCC = new string[strUsers.Length];

                    for (int intCount2 = 0; intCount2 < strUsers.Length; intCount2++)
                    {
                        user = Membership.GetUser(strUsers[intCount2]);
                        strCC[intCount2] = user.Email;
                    }

                    strSubject = "Joint Certification for the quarter ended " + hfQuarterEndDt.Value.ToString() +
                        " pending for your approval.";

                    strMailContent = "<html><head><title>Certification Approval</title></head> " +
                                  "<body style=\"font-size: 10pt; font-family: Zurich bt\"> <br /> " +
                                  "Quarterly ceritifications have been provided by all the function heads." +
                                   "<br /><br />The joint certification is now pending for your approval. " +
                                   " To view and certify the same, " +
                                   "please click on the below mentioned link: " +
                                   " <br/> " +
                                   "<a href =\"" + Global.site_url("Projects/Certification/CommonCertification.aspx\"") + "> " +
                                   "Click here</a> (URL: " + Global.site_url("Projects/Certification/CommonCertification.aspx") + ")<br><br>" +
                                   strFooter + "<br />THIS IS AN AUTO GENERATED MAIL PLEASE DO NOT REPLY BACK.<br /></body></html>";

                    mm.sendAsyncMail(strTo, strCC, null, strSubject, strMailContent);
                }
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Error while sending sendCHApprovalMail : " + ex.Message);
            }
        }

        public void BindChecklist()
        {
            Session["Checklist"] = null;

            dt_checklist = utilBL.getDatasetWithConditionInString("getCertificationChecklistDetails",
                           hfDepartmentID.Value.ToString(), strConnectionString);
            gvChecklist.DataSource = dt_checklist;
            Session["Checklist"] = dt_checklist;
            //strIAgree = "<span class='tabbody3' >I hereby confirm that I have read the key regulatory compliance requirements and have included all deviations in the exception annexure to this certification</span>";
            //    //"<br/><a href='javascript:void(0);' id='demo-basic4' title='Click Here to View the Key Regulatory Checklist' class='linkButton' onclick=\"onChklistClick();return false;\"><b>Click Here to View the Key Regulatory Checklist</b>" +
            //    //    "</a>";
            litAgree.Text = strIAgree;
            gvChecklist.DataBind();
        }

        protected void gvChecklist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                Label lblaction = (Label)(e.Row.FindControl("lblaction"));
                RadioButtonList rbyesnona = (RadioButtonList)(e.Row.FindControl("rbyesnona"));
                if (!lblaction.Text.Equals(""))
                {
                    rbyesnona.SelectedValue = lblaction.Text;
                }
            }
        }

        public DataTable getChecklistDets()
        {
            DataTable dt_d = new DataTable();
            dt_d.Columns.Add(new DataColumn("ChecklistDetsId", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ChecklistMasId", typeof(string)));
            dt_d.Columns.Add(new DataColumn("CertificationId", typeof(string)));
            dt_d.Columns.Add(new DataColumn("YesNoNa", typeof(string)));
            dt_d.Columns.Add(new DataColumn("Remarks", typeof(string)));
            foreach (GridViewRow gvr in gvChecklist.Rows)
            {
                DataRow dr;
                Label lblChecklistDetsId = (Label)gvr.FindControl("lblChecklistDetsId");
                RadioButtonList rbyesnona = (RadioButtonList)gvr.FindControl("rbyesnona");
                F2FTextBox txtRemarks = (F2FTextBox)gvr.FindControl("txtRemarks");
                string strChecklistMasId = gvChecklist.DataKeys[gvr.RowIndex].Values[0].ToString();//gvChecklist.DataKeyNames.ToString();
                dr = dt_d.NewRow();
                dr["ChecklistDetsId"] = lblChecklistDetsId.Text;
                dr["ChecklistMasId"] = strChecklistMasId;
                dr["YesNoNa"] = rbyesnona.SelectedValue;
                dr["Remarks"] = txtRemarks.Text;
                dt_d.Rows.Add(dr);
            }
            return dt_d;
        }

        protected void btnExporttoExcel_Click(object sender, EventArgs e)
        {
            string strTime = System.DateTime.Now.ToString("dd_MMM_yyyy_HH_mm_ss");
            string strSourceDir = Server.MapPath(ConfigurationManager.AppSettings["CertificationChecklistTemplate"].ToString());
            string strDestinationDir = Server.MapPath("~/Temp/");
            if (copyDirectory(strSourceDir, strDestinationDir, strTime))
            {
                DataTable dtdata = (DataTable)Session["Checklist"];
                StringBuilder sb = new StringBuilder();
                if (dtdata.Rows.Count > 0)
                {
                    string fileName = strDestinationDir + "\\" + "CertificationChecklistTemplate_" + strTime + ".xls";
                    string conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HDR=Yes;';";
                    using (OleDbConnection con = new OleDbConnection(conString))
                    {

                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }

                        string strInsert = "";

                        for (int i = 0; i < dtdata.Rows.Count; i++)
                        {
                            DataRow dr = dtdata.Rows[i];
                            string strComplianceStatus = "";
                            if (dr["CCD_YES_NO_NA"].ToString().Equals("C"))
                                strComplianceStatus = "Compliant";
                            else if (dr["CCD_YES_NO_NA"].ToString().Equals("N"))
                                strComplianceStatus = "Not Compliant";
                            else if (dr["CCD_YES_NO_NA"].ToString().Equals("NA"))
                                strComplianceStatus = "Not yet applicable";
                            else if (dr["CCD_YES_NO_NA"].ToString().Equals("W"))
                                strComplianceStatus = "Work in progress";
                            else
                                strComplianceStatus = "";
                            //strInsert = "Insert into [Sheet1$] " +
                            //"([SrNo],[DepartmentId],[CheklistDetsId],[Reference],[Clause],[Particulars],[Checkpoints],[Frequency],[Due Date],[Source Department],[Department responsible for furnishing the data]" +
                            //",[Department responsible for submitting it],[To be filed with],[Select],[Remarks]" +

                            strInsert = "Insert into [Sheet1$] " +
                           "([SrNo],[DepartmentId],[ChecklistMasId],[ChecklistDetsId],[Reference],[Clause],[Particulars],[Frequency]" +
                           ",[To be filed with],[Compliance Status],[Remarks]" +

                            ") values('" + (i + 1) + "'," +
                            "'" + hfDepartmentID.Value.ToString().Replace("'", "''") + "'," +
                            "'" + dr["CCM_ID"].ToString().Replace("'", "''") + "'," +
                            "'" + dr["CCD_ID"].ToString().Replace("'", "''") + "'," +
                            "'" + dr["CCM_REFERENCE"].ToString().Replace("'", "''") + "'," +
                            "'" + dr["CCM_CLAUSE"].ToString().Replace("'", "''") + "'," +
                            "'" + dr["CCM_PARTICULARS"].ToString().Replace("'", "''") + "'," +
                            //"'" + dr["CCM_CHECK_POINTS"].ToString().Replace("'", "''") + "'," +
                            "'" + dr["CCM_FREQUENCY"].ToString().Replace("'", "''") + "'," +
                            //"'" + dr["CCM_DUE_DATE"].ToString().Replace("'", "''") + "'," +
                            //"'" + dr["CCM_SOURCE_DEPT"].ToString().Replace("'", "''") + "'," +
                            //"'" + dr["CCM_DEPT_RESP_FURNISH"].ToString().Replace("'", "''") + "'," +
                            //"'" + dr["CCM_DEPT_RESP_SUBMITTING"].ToString().Replace("'", "''") + "'," +
                            "'" + dr["CCM_TO_BE_FILLED_WITH"].ToString().Replace("'", "''") + "'," +
                            //"'" + dr["CCD_YES_NO_NA"].ToString().Replace("'", "''") + "'," +
                            "'" + strComplianceStatus.ToString().Replace("'", "''") + "'," +
                            "'" + dr["CCD_REMARKS"].ToString().Replace("'", "''") + "')";

                            OleDbCommand cmdIns = new OleDbCommand(strInsert, con);

                            cmdIns.ExecuteNonQuery();
                        }
                        con.Close();
                    }
                    ////Create Downloadable file
                    byte[] content = File.ReadAllBytes(fileName);
                    HttpContext context = HttpContext.Current;

                    context.Response.BinaryWrite(content);

                    context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=Checklist.xls");
                    Context.Response.End();
                }
            }
            else
            {
                lblImportMsg.Text = "Error while Export to Excel Checklist.";
            }
        }

        public bool copyDirectory(string SourceDirectory, string TargetDirectory, string strTime)
        {
            try
            {
                File.Copy(SourceDirectory + "\\" + "CertificationChecklistTemplate.xls", TargetDirectory + "\\" + "CertificationChecklistTemplate_" + strTime + ".xls");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                string strScript = "";
                BindChecklist();
                strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                "alert('Checklist Refresh Sucessfully......');\r\n" +
                    "</script>\r\n";
                ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                lblImportMsg.Text = ex.Message;
            }
        }
    }
}