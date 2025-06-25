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

namespace Fiction2Fact.Projects.Circular
{
    public partial class AddCircularCertChecklists : System.Web.UI.Page
    {
        CircularMasterBLL cirBLL = new CircularMasterBLL();
        //UtilitiesBLL utilBLL = new UtilitiesBLL();
        //UtilitiesVO utilVO = new UtilitiesVO();
        CircUtilitiesBLL circUtilBLL = new CircUtilitiesBLL();
        CommonMethods cm = new CommonMethods();
        int iCirId = 0;

        DataTable dtCertChecklist = new DataTable();

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
                        if (Request.QueryString["CCCId"] != null)
                        {
                            hfCircularCertChecklistId.Value = Request.QueryString["CCCId"].ToString();
                            btnSubmit.Text = "Update Checkpoint";
                            bindCertChecklistData(hfCircularCertChecklistId.Value);
                        }
                        else
                        {
                            if (Request.QueryString["CirID"] == null)
                            {
                                writeError("Oops something went wrong!!!");
                                tblForm.Visible = false;
                                return;
                            }
                            else
                            {
                                hfCirId.Value = Request.QueryString["CirID"].ToString();
                                iCirId = Convert.ToInt32(hfCirId.Value);
                            }
                        }

                        //DataTable dtCertDept = utilBLL.getDatasetWithConditionInString("getCertDeptById", "", mstrConnectionString);
                        DataTable dtCertDept = circUtilBLL.GetDataTable("getCertDeptById", sOrderBy: "CDM_NAME, CSDM_NAME, CSSDM_NAME");
                        CommonCodes.SetDropDownDataSource(ddlDeptName, dtCertDept, "CSSDM_ID", "DeptName");

                        //DataTable dtActRegCirc = utilBLL.getDatasetWithConditionInString("getTypeofCircular", " AND CDTM_STATUS = 'A'", mstrConnectionString);
                        DataTable dtActRegCirc = circUtilBLL.GetDataTable("getTypeofCircular", new DBUtilityParameter("CDTM_STATUS", "A"), sOrderBy: "CDTM_TYPE_OF_DOC");
                        CommonCodes.SetDropDownDataSource(ddlActRegCirc, dtActRegCirc, "CDTM_ID", "CDTM_TYPE_OF_DOC");

                        bindCertChecklistDataGrid();
                    }
                    catch (Exception ex)
                    {
                        hfDoubleClick.Value = "";
                        string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                        writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
                    }
                }

                if (!hfCircularCertChecklistId.Value.Equals(""))
                {
                    string script = "document.getElementById('divCertChklist').style.display = 'none';" +
                                    "document.getElementById('divCertChklist').style.visibility = 'hidden';";
                    ClientScript.RegisterStartupScript(this.GetType(), "HideCertChecklist", script, true);
                }
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
            txtReference.Text = txtTitleofSection.Text = txtCheckpoints.Text = string.Empty;
            txtParticulars.Text = txtPenalty.Text = txtFrequency.Text = txtForms.Text = txtEffectiveFromDate.Text = string.Empty;
            ddlDeptName.SelectedIndex = ddlActRegCirc.SelectedIndex = -1;
        }

        private void bindCertChecklistData(string strCircCertChecklistId = "")
        {
            DataTable dtCertChecklist = new DataTable();

            try
            {
                bool res = int.TryParse(hfCirId.Value, out iCirId);

                dtCertChecklist = cirBLL.SearchCircularCertChecklist(iCirId, strCircCertChecklistId);

                if (dtCertChecklist.Rows.Count > 0)
                {
                    DataRow dr = dtCertChecklist.Rows[0];

                    ddlDeptName.SelectedValue = (dr["CCC_DEPT_ID"] is DBNull ? "" : dr["CCC_DEPT_ID"].ToString());
                    //ddlActRegCirc.SelectedValue = (dr["CCC_ACT_REGULATION_CIRCULAR"] is DBNull ? "" : dr["CCC_ACT_REGULATION_CIRCULAR"].ToString());
                    //txtReference.Text = dr["CCC_REFERENCE"].ToString();
                    txtTitleofSection.Text = dr["CCC_CLAUSE"].ToString();
                    txtCheckpoints.Text = dr["CCC_CHECK_POINTS"].ToString();
                    txtParticulars.Text = dr["CCC_PARTICULARS"].ToString();
                    txtPenalty.Text = dr["CCC_PENALTY"].ToString();
                    txtFrequency.Text = dr["CCC_FREQUENCY"].ToString();
                    txtForms.Text = dr["CCC_FORMS"].ToString();
                    txtEffectiveFromDate.Text = (dr["CCC_EFFECTIVE_FROM"] is DBNull ? "" : Convert.ToDateTime(dr["CCC_EFFECTIVE_FROM"]).ToString("dd-MMM-yyyy"));
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

            string strUser = Authentication.GetUserID(Page.User.Identity.Name);

            DataTable dtCircularCertChecklist = new DataTable();
            dtCircularCertChecklist.Columns.Add(new DataColumn("Id", typeof(string)));
            dtCircularCertChecklist.Columns.Add(new DataColumn("DeptId", typeof(string)));
            dtCircularCertChecklist.Columns.Add(new DataColumn("ActRegCirc", typeof(string)));
            dtCircularCertChecklist.Columns.Add(new DataColumn("Reference", typeof(string)));
            dtCircularCertChecklist.Columns.Add(new DataColumn("Clause", typeof(string)));
            dtCircularCertChecklist.Columns.Add(new DataColumn("CheckPoints", typeof(string)));
            dtCircularCertChecklist.Columns.Add(new DataColumn("Particulars", typeof(string)));
            dtCircularCertChecklist.Columns.Add(new DataColumn("Penalty", typeof(string)));
            dtCircularCertChecklist.Columns.Add(new DataColumn("Frequency", typeof(string)));
            dtCircularCertChecklist.Columns.Add(new DataColumn("Forms", typeof(string)));
            dtCircularCertChecklist.Columns.Add(new DataColumn("EffectiveFrom", typeof(string)));

            DataRow dr = dtCircularCertChecklist.NewRow();
            dr["Id"] = string.IsNullOrEmpty(hfCircularCertChecklistId.Value) ? "0" : hfCircularCertChecklistId.Value;
            dr["DeptId"] = ddlDeptName.SelectedValue;
            dr["ActRegCirc"] = ddlActRegCirc.SelectedValue;
            dr["Reference"] = cm.getSanitizedString(txtReference.Text);
            dr["Clause"] = cm.getSanitizedString(txtTitleofSection.Text);
            dr["CheckPoints"] = cm.getSanitizedString(txtCheckpoints.Text);
            dr["Particulars"] = cm.getSanitizedString(txtParticulars.Text);
            dr["Penalty"] = cm.getSanitizedString(txtPenalty.Text);
            dr["Frequency"] = cm.getSanitizedString(txtFrequency.Text);
            dr["Forms"] = cm.getSanitizedString(txtForms.Text);
            dr["EffectiveFrom"] = txtEffectiveFromDate.Text;
            dtCircularCertChecklist.Rows.Add(dr);

            try
            {
                bool res = int.TryParse(hfCirId.Value, out iCirId);

                cirBLL.insertCircularCertChecklist(iCirId, dtCircularCertChecklist, strUser);
                ClearForm();
                if (hfCircularCertChecklistId.Value != "")
                    writeError("Circular Certification Checklist updated successfully...");
                else
                    writeError("Circular Certification Checklist added to the list...");

                if (hfCirId.Value.Equals(""))
                {
                    string script = "alert('Circular Certification Checklist updated successfully...');" +
                                    "closeWindowRef();";
                    ClientScript.RegisterStartupScript(this.GetType(), "CloseCertChecklist", script, true);
                }

                hfCircularCertChecklistId.Value = "";
                btnSubmit.Text = "Save & Add New";
                bindCertChecklistDataGrid();
                hfDoubleClick.Value = "";
            }
            catch (Exception ex)
            {
                hfDoubleClick.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void gvCertChecklist_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (hfSelectedOperation.Value == "Delete")
                {
                    string id = gvCertChecklist.SelectedDataKey.Value.ToString();
                    cirBLL.DeleteCircularCertChecklist(id);
                    writeError("Circular Certification Checklist Deleted...");
                    bindCertChecklistDataGrid();
                }
                else if (hfSelectedOperation.Value == "Edit")
                {
                    btnSubmit.Text = "Update Certification Checkpoints";
                    hfCircularCertChecklistId.Value = gvCertChecklist.SelectedDataKey.Value.ToString();
                    bindCertChecklistData(hfCircularCertChecklistId.Value);
                }
            }
            catch (Exception ex)
            {
                hfDoubleClick.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void bindCertChecklistDataGrid()
        {
            int intCricularId = 0;
            bool res = int.TryParse(hfCirId.Value, out intCricularId);

            try
            {
                dtCertChecklist = null;
                dtCertChecklist = cirBLL.SearchCircularCertChecklist(intCricularId, hfCircularCertChecklistId.Value);

                DataTable dtCirc = cirBLL.SearchCircularDetails(intCricularId, "", "", "", "", "", "", "", "", "", "", "", "", "", "").Tables[0];
                if (dtCirc.Rows.Count > 0)
                {
                    ddlActRegCirc.SelectedValue = dtCirc.Rows[0]["CM_CDTM_ID"].ToString();
                    string strCircularNo = dtCirc.Rows[0]["CM_CIRCULAR_NO"].ToString().Trim();
                    string strCircularSubject = dtCirc.Rows[0]["CM_TOPIC"].ToString().Trim();
                    //txtReference.Text = (strCircularSubject.EndsWith(".") ? strCircularSubject : strCircularSubject + ".") + (strCircularNo.EndsWith(".") ? strCircularNo : strCircularNo + ".");
                    txtReference.Text = strCircularSubject + " (" + strCircularNo + ")";
                }

                gvCertChecklist.DataSource = dtCertChecklist;
                gvCertChecklist.DataBind();
            }
            catch (Exception ex)
            {
                hfDoubleClick.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void gvCertChecklist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HiddenField hfStatus = (HiddenField)(e.Row.FindControl("hfStatus"));
                    LinkButton lnkEdit = (LinkButton)(e.Row.FindControl("lnkEdit"));
                    LinkButton lnkDelete = (LinkButton)(e.Row.FindControl("lnkDelete"));

                    if (hfStatus.Value.Equals("4"))
                    {
                        lnkEdit.Visible = false;
                        lnkDelete.Visible = false;
                    }
                    else
                    {
                        lnkEdit.Visible = true;
                        lnkDelete.Visible = true;
                    }
                }
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