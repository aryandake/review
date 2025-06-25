using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using Fiction2Fact.Legacy_App_Code.Circulars.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.Circulars;
//[Idunno.AntiCsrf.SuppressCsrfCheck]
namespace Fiction2Fact.Projects.Circulars
{
    public partial class Circulars_ActionableUpdates : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        CircularMasterBLL CircularMasterBLL = new CircularMasterBLL();
        CircUtilitiesBLL circUtilBLL = new CircUtilitiesBLL();
        //UtilitiesBLL utilityBL = new UtilitiesBLL();
        RefCodesBLL rcBL = new RefCodesBLL();
        CommonMethods cm = new CommonMethods();
        //<< Added by ramesh more on 13-Mar-2024 CR_1991
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();
        //>>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    hfCurrDate.Value = System.DateTime.Now.ToString("dd-MMM-yyyy");
                    if (Request.QueryString["CircularId"] != null)
                    {
                        //<< Modified by ramesh more on 13-Mar-2024 CR_1991
                        string strId = Request.QueryString["CircularId"].ToString();
                        hfCircularId.Value = encdec.Decrypt(strId);
                        //hfCircularId.Value = Request.QueryString["CircularId"].ToString();
                        //>>
                    }

                    if (Request.QueryString["ActionableId"] != null)
                    {
                        //<< Modified by ramesh more on 13-Mar-2024 CR_1991
                        string strActionableId = Request.QueryString["ActionableId"].ToString();
                        hfActionableId.Value = encdec.Decrypt(strActionableId);
                        //hfActionableId.Value = Request.QueryString["ActionableId"].ToString();
                        //>>
                    }

                    if (Request.QueryString["Source"] != null)
                    {
                        hfSource.Value = Request.QueryString["Source"].ToString();
                    }

                    if (Request.QueryString["Status"] != null)
                    {
                        hfStatus.Value = Request.QueryString["Status"].ToString();
                    }

                    if (!hfSource.Value.Equals("MyAct") && !hfSource.Value.Equals("List"))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Script", "alert('Invalid Type'); window.location.href = '" + Global.site_url() + "';", true);
                    }

                    CommonCodes.SetDropDownDataSource(ddlUpdateType, rcBL.getRefCodeDetails("Circular Actionable Update Type"));
                    getDetails();
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                if (hfSource.Value.Equals("List"))
                {
                    Response.Redirect(Global.site_url("Projects/Circulars/CircularList.aspx"));
                }
                else if (hfSource.Value.Equals("MyAct"))
                {
                    Response.Redirect(Global.site_url("Projects/Circulars/MyActionables.aspx"));
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void bindIntimationDetails(CheckBoxList cbSubmissions, string CircularId)
        {
            try
            {
                DataTable dtIntimationName;
                string strName = null;
                //dtIntimationName = utilityBL.getDatasetWithCondition("CIRCULARINTIMATION", Convert.ToInt32(hfCircularId.Value), mstrConnectionString);
                dtIntimationName = circUtilBLL.GetDataTable("CIRCULARINTIMATION", new DBUtilityParameter("CMI_CM_ID", hfCircularId.Value));
                for (int i = 0; i <= dtIntimationName.Rows.Count - 1; i++)
                {
                    strName = dtIntimationName.Rows[i]["CMI_CIM_ID"].ToString();
                    cbSubmissions.Items.FindByValue(strName).Selected = true;
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        protected String getFileName(object inputFileName)
        {
            string strFileName = "";

            try
            {
                strFileName = inputFileName.ToString();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return strFileName.Replace("'", "\\'");
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            //lblMsg.Visible = true;
            string script = "\r\n<script language=\"javascript\">\r\n" +
                       " alert('" + strError.Replace("'", "\\'") + "');" +
                       "   </script>\r\n";

            ClientScript.RegisterStartupScript(this.GetType(), "script", script);
        }

        private void getDetails()
        {
            try
            {
                Hashtable ParamMap = new Hashtable();
                DataTable dt = new DataTable();
                DataRow dr, dr1;
                DataSet dsCircularDetails = new DataSet();

                dt = CircularMasterBLL.SearchCircularActionable(Convert.ToInt32(hfCircularId.Value), "", "", "", "", hfActionableId.Value, "", mstrConnectionString);

                if (dt.Rows.Count == 0)
                    writeError("No records found.");
                else
                {
                    dr = dt.Rows[0];
                    lblCreator.Text = dr["CDM_NAME"].ToString();

                    lblLOB.Text = dr["LEM_NAME"].ToString();

                    lblAuthority.Text = dr["CIA_NAME"].ToString();
                    lblTypeofDocument.Text = dr["CDTM_TYPE_OF_DOC"].ToString();
                    lblTopic.Text = dr["CAM_NAME"].ToString();
                    lblSpocFromcompliance.Text = dr["SPOCName"].ToString();
                    hfSpocFromComplianceFunction.Value = dr["CM_CCS_ID"].ToString();
                    lblCircularNo.Text = dr["CM_CIRCULAR_NO"].ToString();
                    if (dr["CM_DATE"].ToString().Equals(""))
                    {
                        lblCircularDate.Text = "";
                    }
                    else
                    {
                        lblCircularDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dr["CM_DATE"]));
                    }
                    lblSubject.Text = dr["CM_TOPIC"].ToString();
                    lblGist.Text = dr["CM_DETAILS"].ToString().Replace("\n", "<br />");
                    lblImplications.Text = dr["CM_IMPLICATIONS"].ToString().Replace("\n", "<br />");
                    lblLink.Text = dr["CM_ISSUING_LINK"].ToString();

                    //cbSubmissions.DataSource = utilityBL.getDataset("CIRCULARINTIMATIONS", mstrConnectionString);
                    //cbSubmissions.DataBind();

                    //bindIntimationDetails(cbSubmissions, hfCircularId.Value);

                    ParamMap.Add("CMId", hfCircularId.Value);
                    dsCircularDetails = CircularMasterBLL.getCircularDetails(ParamMap, mstrConnectionString);

                    DataTable dtActionable = new DataTable();
                    dtActionable = dt;
                    dr1 = dtActionable.Rows[0];
                    lblActionable.Text = dr1["CA_ACTIONABLE"].ToString().Replace("\n", "<br />");
                    // lblPersonResponsible.Text = dr1["CPRM_NAME"].ToString();
                    lblResponsibleFunction.Text = (dr1["ResponsibleFunction"] is DBNull ? "" : dr1["ResponsibleFunction"].ToString());
                    lblPersonResponsibleUserName.Text = dr1["CA_PERSON_RESPONSIBLE"].ToString();
                    hfPersonResponsibleMailId.Value = dr1["CA_PERSON_RESPONSIBLE_EMAIL_ID"].ToString();
                    lblReportingManager.Text = dr1["CA_REPORTING_MANAGER"].ToString();
                    hfReportingMgrMailId.Value = dr1["CA_Reporting_Mgr_EMAIL_ID"].ToString();
                    if (dr["CA_TARGET_DATE"].ToString().Equals(""))
                    {
                        lblTargetDate.Text = "";
                    }
                    else
                    {
                        lblTargetDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dr["CA_TARGET_DATE"]));
                    }
                    if (dr["CA_COMPLETION_DATE"].ToString().Equals(""))
                    {
                        lblCompletionDate.Text = "";
                    }
                    else
                    {
                        lblCompletionDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dr["CA_COMPLETION_DATE"]));
                    }

                    //lblTargetDate.Text = dr1["CA_TARGET_DATE"].ToString();
                    //lblCompletionDate.Text = dr1["CA_COMPLETION_DATE"].ToString();
                    lblStatus.Text = dr1["Status"].ToString();
                    lblRemarks.Text = dr1["CA_REMARKS"].ToString().Replace("\n", "<br />");
                    lblClosureRemarks.Text = dr1["CA_CLOSURE_REMARKS"].ToString().Replace("\n", "<br />");
                    lblClosureBy.Text = dr1["CA_CLOSED_BY"].ToString();
                    if (dr["CA_CLOSED_ON"].ToString().Equals(""))
                    {
                        lblClosureOn.Text = "";
                    }
                    else
                    {
                        lblClosureOn.Text = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(dr["CA_CLOSED_ON"]));
                    }

                    DataTable dtFiles = new DataTable();
                    dtFiles = dsCircularDetails.Tables[1];
                    gvViewFileUpload.DataSource = dtFiles;
                    gvViewFileUpload.DataBind();

                    DataTable dtCircularActionableUpdates = new DataTable();
                    dtCircularActionableUpdates = CircularMasterBLL.SearchCircularActionableUpdates(Convert.ToInt32(hfActionableId.Value), mstrConnectionString);
                    Session["CircularActionableUpdates"] = dtCircularActionableUpdates;
                    gvCircularActionableUpdates.DataSource = dtCircularActionableUpdates;
                    gvCircularActionableUpdates.DataBind();

                    //DataTable dtCircularActionableFiles;

                    //dtCircularActionableFiles = circUtilBLL.GetDataTable("LoadCircularActionableFileList", new DBUtilityParameter("CAF_CA_ID", hfActionableId.Value));
                    //if (dtCircularActionableFiles.Rows.Count > 0)
                    //{
                    //    gvViewActionableFileUpload.DataSource = dtCircularActionableFiles;
                    //    gvViewActionableFileUpload.DataBind();
                    //}
                }
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void gvCircularActionableUpdates_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCircularActionableUpdates.PageIndex = e.NewPageIndex;
            gvCircularActionableUpdates.DataSource = (DataTable)(Session["CircularActionableUpdates"]);
            gvCircularActionableUpdates.DataBind();
            hfTabberId.Value = "2";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this))
            {
                hfDoubleClickFlag.Value = "";
                return;
            }

            string strClientFileName = "", strCreateBy = "", strRevisedTargetDate = null, strActionableClosureDate = null;
            int intActionableId = 0;
            DateTime dt = DateTime.Now;

            try
            {
                intActionableId = Convert.ToInt32(hfActionableId.Value);

                strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                strClientFileName = fuFileUpload.FileName.ToString();
                //if (strClientFileName.Contains("!") || strClientFileName.Contains("@") ||
                //                     strClientFileName.Contains("#") || strClientFileName.Contains("$") ||
                //                     strClientFileName.Contains("%") || strClientFileName.Contains("^") ||
                //                     strClientFileName.Contains("&") || strClientFileName.Contains("'") ||
                //                     strClientFileName.Contains("\""))
                //{
                //    hfDoubleClickFlag.Value = "";
                //    writeError("File Name can't have special character.");
                //    return;
                //}
                //<< Modified by Ramesh more on 13-Mar-2024 CR_1991
                if (strClientFileName.Length > 200)
                {
                    writeError("File Name Exceed 200 Characters");
                    return;
                }
                if (strClientFileName.Contains("!") || strClientFileName.Contains("@") ||
                          strClientFileName.Contains("#") || strClientFileName.Contains("$") ||
                          strClientFileName.Contains("%") || strClientFileName.Contains("^") ||
                          strClientFileName.Contains("&") || strClientFileName.Contains("'") ||
                          strClientFileName.Contains("\"") || strClientFileName.Contains(","))
                {
                    writeError("Invalid File Name");
                    return;
                }

                string strFileExtension = Path.GetExtension(strClientFileName);
                if (UploadedFileContentCheck.checkForMaliciousFileFromHeaders(strFileExtension, fuFileUpload.FileBytes))
                {
                    writeError("The file contains malicious content. Kindly check the file and reupload.");
                    return;
                }


                string strReturnMsg = UploadedFileContentCheck.checkValidFileForUpload(fuFileUpload, "");

                CommonCode cc = new CommonCode();
                strReturnMsg += cc.getFileNameErrors(strClientFileName);
                if (!strReturnMsg.Equals(""))
                {
                    writeError(strReturnMsg);
                    return;
                }

                if (UploadedFileContentCheck.checkForMultipleExtention(strClientFileName))
                {
                    writeError("The file uploaded is multiple extensions.");
                    return;
                }
                //>>

                string strfilename = Authentication.GetUserID(Page.User.Identity.Name) + "_" +
                     dt.ToString("ddMMyyyyHHmmss") + "_" + fuFileUpload.FileName;
                string strServerDirectory = Server.MapPath(ConfigurationManager.AppSettings["CircularFilesFolder"].ToString());
                string strCompleteName = strServerDirectory + "\\" + strfilename;
                fuFileUpload.SaveAs(strCompleteName);

                if (ddlUpdateType.SelectedValue.Equals("ET")) // Extension in Target Date
                    strRevisedTargetDate = txtDate.Text;
                else if (ddlUpdateType.SelectedValue.Equals("B")) // Actionable Closure Date
                    strActionableClosureDate = txtDate.Text;

                int intId = CircularMasterBLL.saveCircularActionableUpdates(0, intActionableId,
                    ddlUpdateType.SelectedValue.ToString(), cm.getSanitizedString(txtRemarks.Text.ToString()), strRevisedTargetDate,
                    strActionableClosureDate, strClientFileName, strfilename, strCreateBy);

                writeError("Actionable update saved successfully.");

                sendCircularActionableUpdatesMail(hfSpocFromComplianceFunction.Value, hfPersonResponsibleMailId.Value);

                ddlUpdateType.SelectedValue = "";
                txtRemarks.Text = "";
                txtDate.Text = "";
                hfDoubleClickFlag.Value = "";

                DataTable dtCircularActionableUpdates = new DataTable();
                dtCircularActionableUpdates = CircularMasterBLL.SearchCircularActionableUpdates(Convert.ToInt32(hfActionableId.Value), mstrConnectionString);
                Session["CircularActionableUpdates"] = dtCircularActionableUpdates;
                gvCircularActionableUpdates.DataSource = dtCircularActionableUpdates;
                gvCircularActionableUpdates.DataBind();
                getDetails();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void sendCircularActionableUpdatesMail(string strSpocFromComplianceID, string strPersonResponsibleMailId)
        {
            string strComplianceSPOCMailId = "", strMailTo = "", strMailCC = "";

            MailContent_Circulars mail = new MailContent_Circulars();

            try
            {
                string strLoggedInUser = (new Authentication()).getUserFullName(Page.User.Identity.Name);

                DataTable dtSpocFromcompliance = circUtilBLL.GetDataTable("getSpocFromComplianceFunction",
                        new DBUtilityParameter("CCS_STATUS", "A"), new DBUtilityParameter("CCS_ID", strSpocFromComplianceID, "IN", null, "AND", 1),
                        sOrderBy: "CCS_NAME");

                foreach (DataRow dr in dtSpocFromcompliance.Rows)
                {
                    strComplianceSPOCMailId = (string.IsNullOrEmpty(strComplianceSPOCMailId) ? "" : strComplianceSPOCMailId + ",") + dr["CCS_EMAIL_ID"].ToString();
                }

                mail.ParamMap.Add("ConfigId", 30);

                if (hfSource.Value.Equals("MyAct"))
                {
                    // To --> SpocFromComplianceFunction, CircAdmin, CircUser and ReportingManager
                    // CC --> PersonResponsible
                    strMailTo = strComplianceSPOCMailId + "," + hfReportingMgrMailId.Value;
                    strMailCC = strPersonResponsibleMailId;

                    mail.ParamMap.Add("To", "CircularAdmin,CircularUser,ProvidedAsParam");
                    mail.ParamMap.Add("cc", "ProvidedAsParam1");
                }
                else if (hfSource.Value.Equals("List"))
                {
                    // To --> PersonResponsible
                    // CC --> SpocFromComplianceFunction, CircAdmin, CircUser and ReportingManager
                    strMailTo = strPersonResponsibleMailId;
                    strMailCC = strComplianceSPOCMailId + "," + hfReportingMgrMailId.Value;

                    mail.ParamMap.Add("To", "ProvidedAsParam");
                    mail.ParamMap.Add("cc", "CircularAdmin,CircularUser,ProvidedAsParam1");
                }

                mail.ParamMap.Add("ToEmailIds", strMailTo);
                mail.ParamMap.Add("CCEmailIds", strMailCC);
                mail.ParamMap.Add("CircularId", hfCircularId.Value);
                mail.ParamMap.Add("CirNo", lblCircularNo.Text);
                mail.ParamMap.Add("CirActionableId", hfActionableId.Value);
                mail.ParamMap.Add("CirActionable", lblActionable.Text.ToString().Replace("\n", "<br />"));
                mail.ParamMap.Add("ResponsiblePerson", lblPersonResponsibleUserName.Text);
                mail.ParamMap.Add("TargetDate", lblTargetDate.Text);
                mail.ParamMap.Add("CompletionDate", lblCompletionDate.Text);
                mail.ParamMap.Add("UpdateStatus", lblStatus.Text);
                mail.ParamMap.Add("UpdateType", ddlUpdateType.SelectedItem.Text);
                mail.ParamMap.Add("UpdateDetails", txtRemarks.Text.ToString().Replace("\n", "<br />"));
                mail.ParamMap.Add("RevisedTargetDate", (ddlUpdateType.SelectedValue.Equals("ET") ? txtDate.Text : null));
                mail.ParamMap.Add("ActionableClosureDate", (ddlUpdateType.SelectedValue.Equals("B") ? txtDate.Text : null));
                mail.ParamMap.Add("LoggedInUserName", strLoggedInUser);
                mail.setCircularMailContent();
            }
            catch (Exception ex)
            {
                hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void cvdate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
    }
}