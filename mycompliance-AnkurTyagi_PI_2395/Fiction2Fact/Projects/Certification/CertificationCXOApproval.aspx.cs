using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.Certification;
using Fiction2Fact.App_Code;
using Fiction2Fact.F2FControls;
using Fiction2Fact.Legacy_App_Code.VO;

namespace Fiction2Fact.Projects.Certification
{
    //[Idunno.AntiCsrf.SuppressCsrfCheck]
    public partial class Certification_CertificationCXOApproval : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ToString();
        UtilitiesBLL utilBL = new UtilitiesBLL();
        UtilitiesVO utilVO = new UtilitiesVO();
        CertificationMasterBL certBL = new CertificationMasterBL();
        private string IsApprovalRejectBtnShow = "";
        private string IsApprovalBtnShow = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["strRowComments"] = "";

                if (Request.QueryString["Type"] != null)
                {
                    hfType.Value = Request.QueryString["Type"].ToString();
                    if (hfType.Value == "L3")
                    {
                        getCertification();

                        if (!string.IsNullOrEmpty(Convert.ToString(Session["ExFHBB"])))
                        {
                            string[] arrVal = Convert.ToString(Session["ExFHBB"]).Split('|');

                            hfDepartmentID.Value = arrVal[0].ToString();
                            hfCertDepartment.Value = arrVal[1].ToString();
                            hfQuarterId.Value = arrVal[2].ToString();

                            pnlCertificationDashboards.Visible = false;
                            pnlCertDetails.Visible = true;

                            string script = "";
                            script += "\r\n <script type=\"text/javascript\">\r\n";
                            script = script + "switchTab('profile');\r\n";
                            script += "</script>\r\n";
                            ClientScript.RegisterStartupScript(this.GetType(), "return script", script);

                            bindApprovalGrid();
                        }

                    }
                    else
                    {
                        this.lblMsg.Text = "No records pending for your approval.";
                        this.lblMsg.Visible = true;
                        pnlCertificationDashboards.Visible = false;
                        pnlCertDetails.Visible = false;
                    }
                }
                else
                {
                    this.lblMsg.Text = "No records pending for your approval.";
                    this.lblMsg.Visible = true;
                    pnlCertificationDashboards.Visible = false;
                    pnlCertDetails.Visible = false;
                }
            }
        }

        private void getCertification()
        {
            DataTable dtCertDashboard;
            DateTime dtCertDate = System.DateTime.Now;

            dtCertDashboard = certBL.getCertificationForPendingApproval("L3", Page.User.Identity.Name.ToString(),
                            strConnectionString);

            gvCertDashboard.DataSource = dtCertDashboard;
            gvCertDashboard.DataBind();

            if ((this.gvCertDashboard.Rows.Count == 0))
            {
                this.lblMsg.Text = "No records pending for your approval.";
                this.lblMsg.Visible = true;
                pnlCertificationDashboards.Visible = false;
                pnlCertDetails.Visible = false;
            }
            else
            {
                this.lblMsg.Text = String.Empty;
                this.lblMsg.Visible = false;
                pnlCertificationDashboards.Visible = true;
                pnlCertDetails.Visible = false;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            getCertification();
            Session["ExFHBB"] = null;
        }

        protected void gvCertDashboard_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string strDeptId = gvCertDashboard.SelectedValue.ToString();

            GridViewRow gvr = gvCertDashboard.SelectedRow;
            Label lblDeptName = (Label)gvr.FindControl("lblDeptName");
            Label lblQuarterId = (Label)gvr.FindControl("lblQuarterId");

            hfQuarterId.Value = lblQuarterId.Text;
            hfDepartmentID.Value = strDeptId;
            hfCertDepartment.Value = lblDeptName.Text.ToString();

            Session["ExFHBB"] = hfDepartmentID.Value + "|" + hfCertDepartment.Value + "|" + hfQuarterId.Value;

            pnlCertificationDashboards.Visible = false;
            pnlCertDetails.Visible = true;
            bindApprovalGrid();
        }

        private void bindApprovalGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow dr1, drQuarter;
                DataTable dtQuarter = new DataTable();
                Authentication auth = new Authentication();
                string strContent = "", strFromDate = "", strQuarter = "", strToDate = "";
                DateTime dtCertDate = System.DateTime.Now;
                string[] strDetailsList;
                string strDetailsId;
                string strUserName = "";
                strUserName = Page.User.Identity.Name.ToString();
                strDetailsId = auth.GetUserDetsByEmpCode(strUserName);
                strDetailsList = strDetailsId.Split('|');
                strUserName = Convert.ToString(strDetailsList[0]);
                string strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                string strDate = DateTime.Now.ToString("dd-MMM-yyyy");

                dt = certBL.getCertificationsApproval(hfDepartmentID.Value, "L3", Page.User.Identity.Name.ToString(),
                    strConnectionString);

                Session["CertificationApprovalGridview"] = dt;
                dr1 = dt.Rows[0];
                strFromDate = (Convert.ToDateTime(dr1["CQM_FROM_DATE"].ToString())).ToString("dd-MMM-yyyy");
                strToDate = (Convert.ToDateTime(dr1["CQM_TO_DATE"].ToString())).ToString("dd-MMM-yyyy");
                gvCertApproval.DataSource = dt;
                gvCertApproval.DataBind();

                string CertIds = "";
                foreach (DataRow dr2 in dt.Rows)
                {
                    CertIds += dr2["CERT_ID"].ToString() + ",";
                }
                if (CertIds.EndsWith(","))
                {
                    CertIds = CertIds.Substring(0, CertIds.Length - 1);
                }

                int intLevel = 2;
                //To fetch the Certification Content in tab 1
                DataTable dtContent = utilBL.getDatasetWithThreeConditionsInString("getCertContentById", hfDepartmentID.Value.ToString(), intLevel, strConnectionString);
                DataRow dr;

                //<<Commented and Added by Rahuldeb on 24Sep2019
                //GetFilingsDashboard(hfDepartmentID.Value, hfCertDepartment.Value);
                RegulatoryReportingDashboard rrd = new RegulatoryReportingDashboard();
                litRegulatoryFilling.Text = rrd.GetFilingsDashboardSingleRow_AQ(hfDepartmentID.Value, hfCertDepartment.Value, "2");
                //>>

                if (dtContent.Rows.Count > 0)
                {
                    dr = dtContent.Rows[0];
                    //<< Commented By Milan Yadav on 17-Apr-2017
                    //strContent = dr["CERTM_TEXT"].ToString();
                    //>>
                    strContent = dr["CERT_CONTENT"].ToString();
                    hfCertMId.Value = dr["CERTM_ID"].ToString();

                    strContent = strContent.Replace("~qtrStartDate", strFromDate);
                    strContent = strContent.Replace("~qtrEndDate", strToDate);
                    strContent = strContent.Replace("~Name", strUserName.ToString());
                    strContent = strContent.Replace("~Department", hfCertDepartment.Value);
                    strContent = strContent.Replace("~Date", dtCertDate.ToString("dd-MMM-yyyy"));
                    lblCertContents.Text = "<div style=\"font-family: Trebuchet MS;\">" + strContent + "</div>";

                    utilVO.setCode(" and CERT_CSSDM_ID=" + hfDepartmentID.Value);
                    dtQuarter = utilBL.getData("getCertificationQuarter", utilVO);
                    if (dtQuarter.Rows.Count > 0)
                    {
                        drQuarter = dtQuarter.Rows[0];
                        strQuarter = (Convert.ToDateTime(drQuarter["CQM_TO_DATE"].ToString())).ToString("dd-MMM-yyyy");
                    }
                    strContent = strContent.Replace("%QuaterDate%", strQuarter);
                    strContent = strContent.Replace("%Date%", strDate);
                }

                if (IsApprovalRejectBtnShow.Equals("True"))
                {
                    btnReject.Visible = true;
                }
                else
                {
                    btnReject.Visible = false;
                }

                if (IsApprovalBtnShow.Equals("False"))
                {
                    btnApprove.Visible = false;
                }
                else
                {
                    btnApprove.Visible = true;
                }

                //<<List of Exceptions.
                DataTable dtExc = utilBL.getDatasetWithConditionInString("getExceptionByCertIdWithDeptName", CertIds, strConnectionString);
                //DataTable dtExc = utilBL.getDataset("getDraftedException", strConnectionString).Tables[0];
                DataRow drExc;
                int intExcCnt = dtExc.Rows.Count;
                string strCEId, strException, strDetails, strClientFile, strServerFile, strRootCause, strActiontaken, strtxtTargetDate, strFHDept, strActionStatus;
                int uniqueRowId = 0;
                string strHtmlTableRows = "";
                string strHtmlTable = "" +
                            " <table class='table table-bordered footable'> " +
                             " <thead> " +
                             " <tr> " +
                             " <th class='tabhead3' align='center'> " +
                             " Sr. No. " +
                             " </th> " +
                             " <th class='tabhead3'> " +
                             " Department " +
                             " </th> " +
                             " <th class='tabhead3'> " +
                             " Deviation (Detailed) " +
                             " </th> " +
                             " <th class='tabhead3'> " +
                             " Regulatory Reference (Detailed) " +
                             " </th> " +
                             " <th class='tabhead3'> " +
                             " Root Cause for the Deviation " +
                             " </th> " +
                             " <th class='tabhead3'> " +
                             " Action taken " +
                             " </th> " +
                             " <th class='tabhead3' width='120px'> " +
                             " Current Status " +
                             " </th> " +
                             " <th class='tabhead3'> " +
                             " Target/Closure Date " +
                             " </th> " +
                             " <th class='tabhead3'> " +
                             " File Name " +
                             " </th> " +
                             " </tr> " +
                             " </thead> ";

                for (int intCnt = 0; intCnt < intExcCnt; intCnt++)
                {
                    uniqueRowId = uniqueRowId + 1;
                    drExc = dtExc.Rows[intCnt];
                    strCEId = drExc["CE_ID"].ToString();
                    strFHDept = drExc["FHDept"].ToString();
                    strException = drExc["CE_EXCEPTION_TYPE"].ToString();
                    strDetails = drExc["CE_DETAILS"].ToString();
                    strClientFile = drExc["CE_CLIENT_FILE_NAME"].ToString();
                    strServerFile = drExc["CE_SERVER_FILE_NAME"].ToString();
                    //Added By Milan Yadav on 05-Feb-2016
                    //<<
                    strRootCause = drExc["CE_ROOT_CAUSE_OF_DEVIATION"].ToString();
                    strActiontaken = drExc["CE_ACTION_TAKEN"].ToString();
                    strActionStatus = drExc["CE_CLOSURE_STATUS"].ToString();
                    if (drExc["CE_CLOSURE_STATUS"].ToString() != "Closed")
                    {
                        if (drExc["CE_TARGET_DATE"] != DBNull.Value)
                        {
                            strtxtTargetDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(drExc["CE_TARGET_DATE"]));
                        }
                        else
                        {
                            strtxtTargetDate = "";
                        }
                    }
                    else
                    {
                        if (drExc["CE_CLOSURE_DATE"] != DBNull.Value)
                        {
                            strtxtTargetDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(drExc["CE_CLOSURE_DATE"]));
                        }
                        else
                        {
                            strtxtTargetDate = "";
                        }
                    }
                    //>>
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

                    strHtmlTableRows = strHtmlTableRows + "<tr>" +
                        "<td class='tabbody3'>" + uniqueRowId + "</td>" +
                        "<td class='tabbody3'><label ID='lblDeptName" + uniqueRowId + "' class = 'textbox1' >" + strFHDept + "</label></td>" +
                        "<td class='tabbody3'><label ID='ExceptionType" + uniqueRowId + "' class = 'textbox1' >" + strException + "</label></td>" +
                        "<td class='tabbody3'><label ID='Details" + uniqueRowId + "' class = 'textbox1' >" + strDetails + "</label></td>" +
                        //Added By Milan Yadav on 05-Feb-2016
                        //<<
                        "<td class='tabbody3'><label ID='RootCause" + uniqueRowId + "' class = 'textbox1'>" + strRootCause + "</label></td>" +
                        "<td class='tabbody3'><label ID='Actiontaken" + uniqueRowId + "' class = 'textbox1'>" + strActiontaken + "</label></td>";
                    //"<td class='tabbody3'><input type='text' ID='txtTargetDate" + uniqueRowId + "'  cellElement.size = '15' maxLength = '100'  class = 'textbox1' value='" + strtxtTargetDate + "'</></td>";

                    string html = "<td class='tabbody3'><select id='ddlActionStatus" + uniqueRowId + "' disabled class='form-select'>" +
                                      "<option value='Select'" + (strActionStatus == "Select" ? " selected" : "") + ">Select</option>" +
                                      "<option value='Open'" + (strActionStatus == "Open" ? " selected" : "") + ">Open</option>" +
                                      "<option value='Closed'" + (strActionStatus == "Closed" ? " selected" : "") + ">Closed</option>" +
                                      "</select></td>";
                    strHtmlTableRows = strHtmlTableRows + html;

                    if (strtxtTargetDate.Equals(""))
                    {
                        strHtmlTableRows = strHtmlTableRows +
                         "<td class='tabbody3'><label ID='txtTargetDate" + uniqueRowId + "' class = 'textbox1'</label></td>";
                    }
                    else
                    {
                        strHtmlTableRows = strHtmlTableRows +
                         "<td class='tabbody3'><label ID='txtTargetDate" + uniqueRowId + "' maxLength = '11' size='15' class = 'textbox1'>" + strtxtTargetDate + "</label></td>";
                    }
                    strHtmlTableRows = strHtmlTableRows + "<td class='tabbody3'><a ID='EX_Filelink" + uniqueRowId + "' href='../DownloadFileCertification.aspx?FileInformation=" + strServerFile + "' >" + strClientFile + "</a></td>";
                    //>>
                    strHtmlTableRows = strHtmlTableRows + "</tr>";
                }

                litControls.Text = strHtmlTable + strHtmlTableRows + " </table> ";
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError(ex.Message);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            DataTable dt = new DataTable();
            string strOperationType = "A";
            updateCertificationData(strOperationType);
            //bindApprovalGrid();
            getCertification();
            writeError("Certificate(s) approved successfully.");
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            DataTable dt = new DataTable();
            string strOperationType = "R";
            updateCertificationData(strOperationType);
            //bindApprovalGrid();
            getCertification();
            writeError("Certificate(s) rejected successfully.");
        }

        private void updateCertificationData(string strOperationType)
        {
            try
            {
                // int intRowsUpdated, intAdvtId;
                string strIds = "", strDeptName = "", strDeptId = "", strQuarter = "", strFunctionName = "";//strRemarks = null,
                string strcreateBy = ((new Authentication()).getUserFullName(Page.User.Identity.Name));
                GridViewRow gvr;
                int intRetId;
                string strRetVal = "";
                for (int intIndex = 0; intIndex < gvCertApproval.Rows.Count; intIndex++)
                {
                    gvr = gvCertApproval.Rows[intIndex];
                    CheckBox RowLevelCheckBox = (CheckBox)(gvr.FindControl("RowLevelCheckBox"));
                    if (RowLevelCheckBox.Checked)
                    {
                        F2FTextBox txtRemarks = (F2FTextBox)(gvr.FindControl("txtRemarks"));
                        Label lblId = (Label)(gvr.FindControl("lblId"));
                        Label lblDepartment = (Label)(gvr.FindControl("lblDepartment"));
                        Label lblQuarter = (Label)(gvr.FindControl("lblQuarter"));
                        Label lblDepartmentId = (Label)(gvr.FindControl("lblDepartmentId"));
                        HiddenField hfFunctionName = (HiddenField)(gvr.FindControl("hfFunctionName"));

                        strDeptName = lblDepartment.Text.ToString();
                        strDeptId = lblDepartmentId.Text.ToString();
                        strQuarter = lblQuarter.Text.ToString();
                        strFunctionName = hfFunctionName.Value.ToString();

                        if (strOperationType.Equals("R"))
                        {
                            strRetVal = certBL.updateCerification(Convert.ToInt32(lblId.Text), strOperationType, txtRemarks.Text.ToString(),
                            strcreateBy, hfType.Value.ToString(), strcreateBy, strcreateBy);

                            SendRejectionMail(lblId.Text, strDeptId, strDeptName, strQuarter);
                        }
                        else if (strOperationType.Equals("A"))
                        {
                            strRetVal = certBL.updateCerification(Convert.ToInt32(lblId.Text), strOperationType, txtRemarks.Text.ToString(),
                                   strcreateBy, hfType.Value.ToString(), strcreateBy, strcreateBy);

                            strIds = strIds + lblId.Text.ToString() + ",";
                        }
                    }
                }

                if (strOperationType.Equals("A"))
                {
                    if (!strIds.Equals(""))
                    {
                        intRetId = certBL.UpdateCertificationForApprover(Convert.ToInt32(hfCertMId.Value), strRetVal, strcreateBy, hfQuarterId.Value,
                        lblCertContents.Text, strConnectionString);

                        strIds = strIds.Remove(strIds.LastIndexOf(','));
                        SendApprovalMailCU(strIds, hfDepartmentID.Value, strFunctionName, strQuarter);
                    }
                }
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError(ex.Message);
            }
        }

        private void SendRejectionMail(string strCertIds, string strDepartmentId, string strDepartment, string strQuarter)
        {
            try
            {
                CommonCode cc = new CommonCode();
                Authentication auth = new Authentication();
                //<<Modified by Ankur Tyagi on 01Feb2024 for CR_1945
                cc.ParamMap.Add("ConfigId", 14);
                cc.ParamMap.Add("To", "Level1");
                cc.ParamMap.Add("cc", "Comp,Level3,Level2");
                cc.ParamMap.Add("SubmittedBy",
                               (new Authentication()).getUserFullName(Page.User.Identity.Name));

                cc.ParamMap.Add("CertDepartmentIdType", "SPOC");

                cc.ParamMap.Add("CertDepartmentId", strDepartmentId);
                cc.ParamMap.Add("CertDepartment", strDepartment);
                cc.ParamMap.Add("Quarter", strQuarter);
                cc.ParamMap.Add("CertIds", strCertIds);
                cc.setCertificationMailContent();
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in SendRejectionMail:" + exp);
            }
        }

        private void SendApprovalMailCU(string strCertIds, string strDepartmentId, string strDepartment, string strQuarter)
        {
            try
            {
                CommonCode cc = new CommonCode();
                Authentication auth = new Authentication();
                //<<Modified by Ankur Tyagi on 01Feb2024 for CR_1945
                cc.ParamMap.Add("ConfigId", 12);
                cc.ParamMap.Add("To", "Comp");
                cc.ParamMap.Add("cc", "Level3,Level2,Level1");
                cc.ParamMap.Add("SubmittedBy", (new Authentication()).getUserFullName(Page.User.Identity.Name));
                cc.ParamMap.Add("CertDepartmentIdType", "FH");
                cc.ParamMap.Add("CertDepartmentId", strDepartmentId);
                cc.ParamMap.Add("CertDepartment", strDepartment);
                cc.ParamMap.Add("Quarter", strQuarter);
                cc.ParamMap.Add("CertIds", strCertIds);
                cc.setCertificationMailContent();
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in SendApprovalMailCU:" + exp);
            }
        }

        protected void btnViewCancel_Click(object sender, System.EventArgs e)
        {
            lblMsg.Text = "";
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

        protected void gvCertApproval_DataBound(object sender, EventArgs e)
        {
            string strRowComments = "";
            string strRowStatus = "";
            CheckBox HeaderLevelCheckBox = (CheckBox)(gvCertApproval.HeaderRow.FindControl("HeaderLevelCheckBox"));
            HeaderLevelCheckBox.Attributes["onclick"] = "ChangeAllCheckBoxStates(this.checked);";
            List<string> ArrayValues = new List<string>();
            ArrayValues.Add(String.Concat("'", HeaderLevelCheckBox.ClientID, "'"));

            GridViewRow gvr;
            F2FTextBox txtRemarks;
            for (int intIndex = 0; intIndex < gvCertApproval.Rows.Count; intIndex++)
            {
                string strId = Convert.ToString(intIndex);
                gvr = gvCertApproval.Rows[intIndex];

                HiddenField hfStatus = (HiddenField)(gvr.FindControl("hfStatus"));
                CheckBox RowLevelCheckBox = (CheckBox)(gvr.FindControl("RowLevelCheckBox"));
                LinkButton lnlEdit = (LinkButton)(gvr.FindControl("lnlEdit"));
                txtRemarks = (F2FTextBox)(gvr.FindControl("txtRemarks"));
                RowLevelCheckBox.Attributes["onclick"] = "onRowCheckedUnchecked('" + RowLevelCheckBox.ClientID + "','" +
                                                        txtRemarks.ClientID + "');";
                ArrayValues.Add(string.Concat("'", RowLevelCheckBox.ClientID, "'"));

                if (!hfStatus.Value.Equals("L2A") && !hfStatus.Value.Equals("L4R") && !hfStatus.Value.Equals("L5R"))
                {
                    RowLevelCheckBox.Enabled = false;
                    lnlEdit.Visible = false;
                }
                else
                {
                    IsApprovalRejectBtnShow = "True";
                    RowLevelCheckBox.Enabled = true;
                    lnlEdit.Visible = true;
                }

                if (!hfStatus.Value.Equals("L2A") && !hfStatus.Value.Equals("L4R") && !hfStatus.Value.Equals("L5R"))
                {
                    IsApprovalBtnShow = "False";
                }

                strRowComments = "','" + txtRemarks.ClientID;
                strRowStatus = "','" + hfStatus.ClientID;

                if (Session["strRowComments"] == null)
                    Session["strRowComments"] = "";

                if (Session["strRowStatus"] == null)
                    Session["strRowStatus"] = "";

                Session["strRowComments"] = Session["strRowComments"].ToString() + strRowComments;
                Session["strRowStatus"] = Session["strRowStatus"].ToString() + strRowStatus;
            }
            CheckBoxIDsArray.Text = ("<script type=\"text/javascript\">" +
                " var commentsIDs =  new Array(' " + Session["strRowComments"].ToString() + "');" +
                " var statusIDs =  new Array('" + Session["strRowStatus"].ToString() + "');" +
                 ("\r\n" + ("<!--" + ("\r\n"
                        + (string.Concat("var CheckBoxIDs =  new Array(", string.Join(",", ArrayValues.ToArray()), ");") + ("\r\n" + ("// -->" + ("\r\n" + "</script>"))))))));
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            string strContent = "<html><body>" + hfContent.Value + "</body></html>";
            context.Response.Clear();
            Response.Write(strContent);
            context.Response.ContentType = "application/ms-word";
            context.Response.AppendHeader("content-disposition", "attachment; filename=Certificate.doc");
            context.Response.End();
        }

        protected void lnlEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string strCertID = ((LinkButton)sender).CommandArgument.ToString();
                Response.Redirect("EditChecklists.aspx?CertID=" + strCertID + "&Type=" + hfType.Value, false);
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("System Exceptionn in lnlEdit_Click() :" + ex.Message);
            }
        }
    }
}