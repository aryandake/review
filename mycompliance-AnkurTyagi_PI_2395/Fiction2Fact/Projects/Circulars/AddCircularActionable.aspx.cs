using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using System.Data;
using Fiction2Fact.Legacy_App_Code.Circulars.BLL;
using System.Collections;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.Circulars;

namespace Fiction2Fact.Projects.Circular
{
    public partial class AddCircularActionable : System.Web.UI.Page
    {
        CircularMasterBLL CircularMasterBLL = new CircularMasterBLL();
        RefCodesBLL rcBL = new RefCodesBLL();
        string mstrConnectionString = null;
        CircUtilitiesBLL circUtilBLL = new CircUtilitiesBLL();
        //UtilitiesBLL utilBLL = new UtilitiesBLL();
        //UtilitiesVO utilVO = new UtilitiesVO();
        CircularMasterBLL cirBLL = new CircularMasterBLL();
        CommonMethods cm = new CommonMethods();
        int iCirId = 0;

        DataTable dtActionables = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //<<Added by Ankur Tyagi on 19Mar2024 for CR_1991
                if (Page.User.Identity.Name.Equals(""))
                {
                    Response.Redirect(Global.site_url("Login.aspx"));
                    return;
                }
                //>>
                if (!Page.IsPostBack)
                {
                    try
                    {
                        DataTable dtActionableStatus = rcBL.getRefCodeDetails("Actionable Status", mstrConnectionString);
                        CommonCodes.SetDropDownDataSource(ddlStatus, dtActionableStatus, "RC_CODE", "RC_NAME");
                        if (ddlStatus.Items.Count >= 2)
                        {
                            ddlStatus.SelectedIndex = 2;
                        }

                        DataTable dtResponsibleFunction = circUtilBLL.GetDataTable("CircularFunction", new DBUtilityParameter("CFM_STATUS", "A"), sOrderBy: "CFM_NAME");
                        CommonCodes.SetDropDownDataSource(ddlResFunc, dtResponsibleFunction, "CFM_ID", "CFM_NAME");

                        //<< Modified by Amarjeet on 14-Jul-2021
                        if (Request.QueryString["ActionableId"] != null)
                        {
                            hfActionableId.Value = Request.QueryString["ActionableId"].ToString();
                            btnSubmit.Text = "Update Actionable";
                            bindActionableData(hfActionableId.Value);
                        }
                        else
                        {
                            if (Request.QueryString["CirId"] == null)
                            {
                                writeError("Oops something went wrong!!!");
                                tblForm.Visible = false;
                                return;
                            }
                            else
                            {
                                hfCirId.Value = Request.QueryString["CirId"].ToString();
                                //<< Modified by Amarjeet on 14-Jul-2021
                                //iCirId = Convert.ToInt32(hfCirId.Value);
                                bool res = int.TryParse(hfCirId.Value, out iCirId);
                                //>>
                                bindActionableData();
                            }

                        }
                        //>>

                        hfCurrDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");

                        bindActionableDataGrid();
                    }
                    catch (Exception ex)
                    {
                        hfDoubleClick.Value = "";
                        string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                        writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
                    }
                }

                string script = "";

                if (!hfActionableId.Value.Equals("") && Request.QueryString["ActionableId"] != null)
                {
                    script = "document.getElementById('divActionables').style.display = 'none';" +
                             "document.getElementById('divActionables').style.visibility = 'hidden';";
                }

                script += "onStatusChange();";
                ClientScript.RegisterStartupScript(this.GetType(), "script", script, true);
            }
            //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
            //>>
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page != null)
            {
                Page.RegisterRequiresViewStateEncryption();
            }
        }

        private void ClearForm()
        {
            txtResponsiblePerson.Text = hfResponsiblePersonId.Value = hfFlag.Value = string.Empty;
            txtReportingMgr.Text = hfReportingManagerId.Value = hfRMFlag.Value = string.Empty;
            txtActionTaken.Text = txtTargetDate.Text = txtRegDueDate.Text = txtCompletionDate.Text = txtRemark.Text = string.Empty;
            ddlResFunc.SelectedIndex = ddlStatus.SelectedIndex = -1;
        }
        private void bindActionableData(string strActId = null)
        {
            DataTable dtActionables = new DataTable();
            DataTable dtSelectedActionable = new DataTable();
            //<< Added by Amarjeet on 15-Jul-2021
            string strResponsiblePersonId = "", strResponsiblePersonName = "", strResponsiblePersonEmail = "",
                strReportingManagerId = "", strReportingManagerName = "", strReportingManagerEmail = "";
            //>>

            try
            {
                //<< Modified by Amarjeet on 14-Jul-2021
                //iCirId = Convert.ToInt32(hfCirId.Value);
                bool res = int.TryParse(hfCirId.Value, out iCirId);
                //>>

                dtActionables = cirBLL.SearchCircularActionable(iCirId, "", "", "", "", strActId, "", mstrConnectionString);
                //gvCompMailBoxActionables.DataSource = dtActionables;
                //gvCompMailBoxActionables.DataBind();

                //dtSelectedActionable = cirBLL.searchCMBActionables(hfActionableId.Value, hfCirId.Value, "", "", "", "", "", mstrConnectionString);

                if (dtActionables.Rows.Count > 0)
                {
                    DataRow dr = dtActionables.Rows[0];
                    if (!string.IsNullOrEmpty(strActId))
                    {
                        txtActionTaken.Text = dr["CA_ACTIONABLE"].ToString();
                        ddlResFunc.SelectedValue = (dr["CA_CFM_ID"] == DBNull.Value ? "" : (dr["CA_CFM_ID"].ToString().Equals("0") ? "" : dr["CA_CFM_ID"].ToString()));
                        string str = (dr["CA_CFM_ID"] == DBNull.Value ? "" : (dr["CA_CFM_ID"].ToString().Equals("0") ? "" : dr["CA_CFM_ID"].ToString()));

                        if (!hfSelectedOperation.Value.Equals("Copy"))
                        {
                            txtResponsiblePerson.Text = dr["CA_PERSON_RESPONSIBLE"] == DBNull.Value ? "" : dr["CA_PERSON_RESPONSIBLE"].ToString();
                            strResponsiblePersonId = dr["CA_PERSON_RESPONSIBLE_ID"].ToString();
                            strResponsiblePersonName = dr["CA_PERSON_RESPONSIBLE_NAME"].ToString();
                            strResponsiblePersonEmail = dr["CA_PERSON_RESPONSIBLE_EMAIL_ID"].ToString();
                            hfResponsiblePersonEdit.Value = strResponsiblePersonId + "|" + strResponsiblePersonName + "|" + strResponsiblePersonEmail;

                            txtReportingMgr.Text = dr["CA_REPORTING_MANAGER"] == DBNull.Value ? "" : dr["CA_REPORTING_MANAGER"].ToString();
                            strReportingManagerId = dr["CA_Reporting_Mgr_ID"].ToString();
                            strReportingManagerName = dr["CA_Reporting_Mgr_Name"].ToString();
                            strReportingManagerEmail = dr["CA_Reporting_Mgr_EMAIL_ID"].ToString();
                            hfReportingManagerEdit.Value = strReportingManagerId + "|" + strReportingManagerName + "|" + strReportingManagerEmail;
                        }

                        ddlStatus.SelectedValue = dr["CA_STATUS"].ToString();
                        txtTargetDate.Text = dr["CA_TARGET_DATE"] == DBNull.Value ? "" : Convert.ToDateTime(dr["CA_TARGET_DATE"]).ToString("dd-MMM-yyyy");
                        txtRegDueDate.Text = dr["CA_REGULATORY_DUE_DATE"] == DBNull.Value ? "" : Convert.ToDateTime(dr["CA_REGULATORY_DUE_DATE"]).ToString("dd-MMM-yyyy");
                        txtCompletionDate.Text = dr["CA_COMPLETION_DATE"] == DBNull.Value ? "" : Convert.ToDateTime(dr["CA_COMPLETION_DATE"]).ToString("dd-MMM-yyyy");
                        txtRemark.Text = dr["CA_REMARKS"].ToString();
                    }
                    //<< Added by Amarjeet on 14-Jul-2021
                    hfSpocFromComplianceFnId.Value = dr["CM_CCS_ID"].ToString();
                    hfCirType.Value = dr["CDTM_TYPE_OF_DOC"].ToString();
                    hfCirSubject.Value = dr["CM_TOPIC"].ToString();
                    hfCirIssAuthority.Value = dr["CIA_NAME"].ToString();
                    hfCirDate.Value = dr["CM_DATE"] == DBNull.Value ? "" : Convert.ToDateTime(dr["CM_DATE"]).ToString("dd-MMM-yyyy");
                    //>>
                }
            }
            catch (Exception ex)
            {
                hfDoubleClick.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this))
            {
                hfDoubleClick.Value = "";
                return;
            }

            string strActionable = null;
            string strRespPersonId = null, strRespPersonName = null, strRespPersonEmail = null;
            string strManagerId = null, strManagerName = null, strManagerEmail = null;
            string strTargetDate = null, strCompletionDate = null, strRegulatoryDueDate = null, strStatus = null;
            string strRemark = null, strResFunc = null;

            string strUser = Authentication.GetUserID(Page.User.Identity.Name);

            strActionable = cm.getSanitizedString(txtActionTaken.Text);
            strResFunc = ddlResFunc.SelectedValue.ToString();

            if (hfResponsiblePersonEdit.Value.Equals(""))
            {
                if (!hfResponsiblePersonId.Value.Equals("") && !hfResponsiblePersonId.Value.Equals("||"))
                {
                    strRespPersonId = hfResponsiblePersonId.Value.Split('|')[0];
                    strRespPersonName = hfResponsiblePersonId.Value.Split('|')[1];
                    strRespPersonEmail = hfResponsiblePersonId.Value.Split('|')[2];
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "pop", "alert('Please enter correct details for Responsible Person.');", true);

                    hfDoubleClick.Value = "";
                    return;
                }
            }
            else
            {
                if (hfFlag.Value.Equals("true"))
                {
                    if (hfResponsiblePersonId.Value.Equals(""))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "pop", "alert('Please enter correct details for Responsible Person.');", true);

                        hfDoubleClick.Value = "";
                        return;
                    }
                    else
                    {
                        strRespPersonId = hfResponsiblePersonId.Value.Split('|')[0];
                        strRespPersonName = hfResponsiblePersonId.Value.Split('|')[1];
                        strRespPersonEmail = hfResponsiblePersonId.Value.Split('|')[2];
                    }
                }
                else
                {
                    strRespPersonId = hfResponsiblePersonEdit.Value.Split('|')[0];
                    strRespPersonName = hfResponsiblePersonEdit.Value.Split('|')[1];
                    strRespPersonEmail = hfResponsiblePersonEdit.Value.Split('|')[2];
                }
            }

            if (hfReportingManagerEdit.Value.Equals("") || hfReportingManagerEdit.Value.Equals("||"))
            {
                if (!hfReportingManagerId.Value.Equals("") && !hfReportingManagerId.Value.Equals("||"))
                {
                    strManagerId = hfReportingManagerId.Value.Split('|')[0];
                    strManagerName = hfReportingManagerId.Value.Split('|')[1];
                    strManagerEmail = hfReportingManagerId.Value.Split('|')[2];
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "pop", "alert('Please enter correct details for Reporting Manager.');", true);

                    hfDoubleClick.Value = "";
                    return;
                }
            }
            else
            {
                if (hfRMFlag.Value.Equals("true"))
                {
                    if (hfReportingManagerId.Value.Equals(""))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "pop", "alert('Please enter correct details for Reporting Manager.');", true);

                        hfDoubleClick.Value = "";
                        return;
                    }
                    else
                    {
                        strManagerId = hfReportingManagerId.Value.Split('|')[0];
                        strManagerName = hfReportingManagerId.Value.Split('|')[1];
                        strManagerEmail = hfReportingManagerId.Value.Split('|')[2];
                    }
                }
                else
                {
                    strManagerId = hfReportingManagerEdit.Value.Split('|')[0];
                    strManagerName = hfReportingManagerEdit.Value.Split('|')[1];
                    strManagerEmail = hfReportingManagerEdit.Value.Split('|')[2];
                }
            }

            strStatus = ddlStatus.SelectedValue.ToString();
            strTargetDate = CommonCodes.dispToDbDateTime(txtTargetDate.Text);
            strRegulatoryDueDate = CommonCodes.dispToDbDateTime(txtRegDueDate.Text);
            strCompletionDate = CommonCodes.dispToDbDateTime(txtCompletionDate.Text);

            strRemark = txtRemark.Text;

            DataTable dtActionable = new DataTable();
            dtActionable.Columns.Add(new DataColumn("Id", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("Actionable", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("ResFuncName", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("PerResp", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("PerRespUserId", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("PerRespUserName", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("PerRespEmailId", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("ReportMgr", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("ReportMgrId", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("ReportMgrUserName", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("ReportMgrEmailId", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("TargetDate", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("RegulatoryDueDate", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("ComplDate", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("Status", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("StatusName", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("CommType", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("Function", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("Remarks", typeof(string)));
            //<< Added by Amarjeet on 14-Jul-2021
            dtActionable.Columns.Add(new DataColumn("SpocFromComplianceFnId", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("CirType", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("CirSubject", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("CirIssAuthority", typeof(string)));
            dtActionable.Columns.Add(new DataColumn("CirDate", typeof(string)));
            //>>
            DataRow dr = dtActionable.NewRow();
            if (hfSelectedOperation.Value == "Copy")
            {
                dr["Id"] = "0";
            }
            else
            {
                dr["Id"] = string.IsNullOrEmpty(hfActionableId.Value) ? "0" : hfActionableId.Value;
            }

            dr["Actionable"] = strActionable;
            dr["ResFuncName"] = ddlResFunc.SelectedItem.Text;
            dr["PerResp"] = cm.getSanitizedString(txtResponsiblePerson.Text);
            dr["PerRespUserId"] = strRespPersonId;
            dr["PerRespUserName"] = strRespPersonName;
            dr["PerRespEmailId"] = strRespPersonEmail;
            dr["TargetDate"] = string.IsNullOrEmpty(strTargetDate) ? "" : strTargetDate;
            dr["RegulatoryDueDate"] = string.IsNullOrEmpty(strRegulatoryDueDate) ? "" : strRegulatoryDueDate;
            dr["ComplDate"] = string.IsNullOrEmpty(strCompletionDate) ? "" : strCompletionDate;
            dr["Status"] = strStatus;
            dr["StatusName"] = ddlStatus.SelectedItem.Text;
            dr["CommType"] = null;
            dr["Function"] = strResFunc;
            dr["Remarks"] = strRemark;
            dr["ReportMgr"] = cm.getSanitizedString(txtReportingMgr.Text);
            dr["ReportMgrId"] = strManagerId;
            dr["ReportMgrUserName"] = strManagerName;
            dr["ReportMgrEmailId"] = strManagerEmail;
            //<< Added by Amarjeet on 14-Jul-2021
            dr["SpocFromComplianceFnId"] = hfSpocFromComplianceFnId.Value;
            dr["CirType"] = hfCirType.Value;
            dr["CirSubject"] = hfCirSubject.Value;
            dr["CirIssAuthority"] = hfCirIssAuthority.Value;
            dr["CirDate"] = hfCirDate.Value;
            //>>
            dtActionable.Rows.Add(dr);

            try
            {
                //<< Modified by Amarjeet on 14-Jul-2021
                //CircularMasterBLL.insertCircularAcionables(Convert.ToInt32(hfCirId.Value), dtActionable, strUser);

                bool res = int.TryParse(hfCirId.Value, out iCirId);

                CircularMasterBLL.insertCircularAcionables(iCirId, dtActionable, strUser, (new Authentication().getUserFullName(strUser)));
                //>>

                if (hfActionableId.Value != "")
                    writeError("Actionable updated successfully...");
                else
                    writeError("Actionable added to the list...");

                //<< Added by Amarjeet on 14-Jul-2021

                if (hfActionableId.Value.Equals(""))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(ViewState["EditRec"])))
                    {
                        sendCircularActionableMail(dtActionable, "N");
                    }
                }
                else
                {
                    sendCircularActionableMail(dtActionable, "E");
                }

                //string script = "alert('Actionable updated successfully...');" +
                //                "closeWindowRef();";
                //ClientScript.RegisterStartupScript(this.GetType(), "CloseActionable", script, true);

                //>>
                ClearForm();
                txtActionTaken.Text = "";
                //ddlRelevantStakeHolder.SelectedValue = "";
                txtTargetDate.Text = "";
                hfActionableId.Value = "";
                btnSubmit.Text = "Add Actionable";
                bindActionableDataGrid();
                if (Request.QueryString["ActionableId"] != null)
                {
                    bindActionableData(Request.QueryString["ActionableId"].ToString());
                }
                hfDoubleClick.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClick.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        //<< Added by Amarjeet on 14-jul-2021
        public void sendCircularActionableMail(DataTable dtActionable, string Type = null)
        {
            DataTable dtSpocFromcompliance = new DataTable();
            string strMailTo = "", strMailCC = "";
            MailContent_Circulars mail = new MailContent_Circulars();

            if (dtActionable.Rows.Count > 0)
            {
                try
                {
                    DataRow dr = dtActionable.Rows[0];

                    strMailTo = dr["PerRespEmailId"].ToString();
                    strMailCC = dr["ReportMgrEmailId"].ToString();

                    //dtSpocFromcompliance = utilBLL.getDatasetWithConditionInString("getSpocFromComplianceFunction", " AND CCS_STATUS = 'A' and CCS_ID = " + dr["SpocFromComplianceFnId"].ToString() + "", mstrConnectionString);
                    //dtSpocFromcompliance = circUtilBLL.GetDataTable("getSpocFromComplianceFunction", new DBUtilityParameter("CCS_STATUS", "A"), new DBUtilityParameter("CCS_ID", dr["SpocFromComplianceFnId"].ToString()), sOrderBy: "CCS_NAME");
                    dtSpocFromcompliance = circUtilBLL.GetDataTable("getSpocFromComplianceFunction", new DBUtilityParameter("CCS_STATUS", "A"), new DBUtilityParameter("CCS_ID", dr["SpocFromComplianceFnId"].ToString(), "IN", null, "AND", 1), sOrderBy: "CCS_NAME");

                    foreach (DataRow dr1 in dtSpocFromcompliance.Rows)
                    {
                        strMailCC = (string.IsNullOrEmpty(strMailCC) ? "" : strMailCC + ",") + dr1["CCS_EMAIL_ID"].ToString();
                    }
                    if (Type == "N")
                    {
                        mail.ParamMap.Add("ConfigId", 29);
                    }
                    else if (Type == "E")
                    {
                        if (ddlStatus.SelectedItem.Value == "P")
                        {
                            mail.ParamMap.Add("ConfigId", 1091);
                        }
                        else
                        {
                            mail.ParamMap.Add("ConfigId", 1092);
                        }
                    }
                    mail.ParamMap.Add("To", "ProvidedAsParam");
                    mail.ParamMap.Add("ToEmailIds", strMailTo);
                    mail.ParamMap.Add("cc", "CircularAdmin,CircularUser,ProvidedAsParam1");
                    mail.ParamMap.Add("CCEmailIds", strMailCC);
                    mail.ParamMap.Add("CirType", dr["CirType"].ToString());
                    mail.ParamMap.Add("CirSubject", dr["CirSubject"].ToString());
                    mail.ParamMap.Add("CirIssAuthority", dr["CirIssAuthority"].ToString());
                    mail.ParamMap.Add("CirDate", dr["CirDate"].ToString());
                    mail.setCircularMailContent(dtActionable);
                }
                catch (Exception ex)
                {
                    hfDoubleClick.Value = "";
                    string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
                }
            }
        }
        //>>

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void gvCompMailBoxActionables_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    HiddenField hfStatus = (HiddenField)e.Row.FindControl("hfStatus");
            //    LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
            //    LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");

            //    if (hfStatus.Value == "D")
            //    {
            //        lnkDelete.Visible = true;
            //    }
            //    else
            //    {
            //        lnkDelete.Visible = false;
            //    }

            //    if (hfStatus.Value == "WSH" || hfStatus.Value == "D")
            //        lnkEdit.Visible = true;
            //    else
            //        lnkEdit.Visible = false;
            //}
        }

        protected void gvCompMailBoxActionables_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtActionables = new DataTable();
            try
            {
                if (hfSelectedOperation.Value == "Delete")
                {
                    //cirBLL.cmb_DeleteActionable(gvCompMailBoxActionables.SelectedValue.ToString(), mstrConnectionString);

                    //dtActionables = cirBLL.searchCMBActionables("", hfCirId.Value, "", "", "", "", "", mstrConnectionString);

                    //gvCompMailBoxActionables.DataSource = dtActionables;
                    //gvCompMailBoxActionables.DataBind();
                }
                else if (hfSelectedOperation.Value == "Edit")
                {
                    //hfActionableId.Value = gvCompMailBoxActionables.SelectedValue.ToString();
                    bindActionableData();
                    ViewState["EditRec"] = "Y";
                }
            }
            catch (Exception ex)
            {
                hfDoubleClick.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnSubmit_Circulate_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            string strLoggedInUser = "", strUserId = "";

            strUserId = Authentication.GetUserID(Page.User.Identity.Name);

            if (Session["UserName"] != null)
                strLoggedInUser = Session["UserName"].ToString();
            else
                //strLoggedInUser = Authentication.getUnAuthUserDetsFromLDAP(strUserId, "Name");
                try
                {
                    //cirBLL.submitDraftedActionables(hfCirId.Value, strLoggedInUser, mstrConnectionString);
                    ClientScript.RegisterStartupScript(GetType(), "ClosePopup", "closeWindowRef();", true);
                }
                catch (Exception ex)
                {
                    hfDoubleClick.Value = "";
                    string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
                }
        }

        protected void gvActionables_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (hfSelectedOperation.Value == "Delete")
                {
                    string id = gvActionables.SelectedDataKey.Value.ToString();
                    CircularMasterBLL.DeleteCircularActionable(id);
                    writeError("Circular Actionable Deleted...");
                    bindActionableDataGrid();
                }
                else if (hfSelectedOperation.Value == "Edit")
                {
                    btnSubmit.Text = "Update Actionable";
                    hfActionableId.Value = gvActionables.SelectedDataKey.Value.ToString();
                    bindActionableData(hfActionableId.Value);
                }
                else if (hfSelectedOperation.Value == "Copy")
                {
                    hfActionableId.Value = gvActionables.SelectedDataKey.Value.ToString();
                    bindActionableData(hfActionableId.Value);
                }
            }
            catch (Exception ex)
            {
                hfDoubleClick.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void gvActionables_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void bindActionableDataGrid()
        {
            try
            {
                Hashtable ParamMap = new Hashtable();
                ParamMap.Add("CMId", hfCirId.Value);
                DataSet dsCircularDetails = CircularMasterBLL.getCircularDetails(ParamMap, mstrConnectionString);
                dtActionables = null;
                dtActionables = dsCircularDetails.Tables[0];
                gvActionables.DataSource = dtActionables;
                gvActionables.DataBind();
            }
            catch (Exception ex)
            {
                hfDoubleClick.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
    }
}