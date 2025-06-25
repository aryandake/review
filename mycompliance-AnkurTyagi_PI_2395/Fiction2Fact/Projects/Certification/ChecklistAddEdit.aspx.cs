using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    //[Idunno.AntiCsrf.SuppressCsrfCheck]
    public partial class Certification_ChecklistAddEdit : System.Web.UI.Page
    {
        string strId = "";
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CertificationMasterBL certBL = new CertificationMasterBL();
        CommonMethods cm = new CommonMethods();
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlSearchDeptName.DataSource = utilBL.getDatasetWithConditionInString("getCertDeptById", "", strConnectionString);
                ddlSearchDeptName.DataBind();
                ddlSearchDeptName.Items.Insert(0, new ListItem("(Select)", ""));

                ddlActRegCirc.DataSource = utilBL.getDatasetWithConditionInString("getTypeofCircular", " AND CDTM_STATUS = 'A'", strConnectionString);
                ddlActRegCirc.DataBind();
                ddlActRegCirc.Items.Insert(0, new ListItem("(Select)", ""));

                //Added By Milan Yadav on 27-Sep-2016
                //>>
                if (Request.QueryString["Type"] != null)
                {
                    hftype.Value = Request.QueryString["Type"].ToString();
                }
                //<<

                //Edit existing one.
                string strCId = Request.QueryString["CID"];
                if (Request.QueryString["CID"] != null)
                {
                    hfSrc.Value = encdec.Decrypt(Request.QueryString["CID"].ToString());
                }
                Int32 intCId;

                if (strCId != null)
                {
                    Boolean blnIsQueryStringCorrect = Int32.TryParse(encdec.Decrypt(strCId), out intCId);
                    if (!blnIsQueryStringCorrect)
                        writeError("Certification Compliance Checklist Id is not in proper format.");
                    else
                    {
                        hfCId.Value = encdec.Decrypt(strCId);
                        lblHeader.Text = "Edit Certification Compliance Checklist";
                        if (!getDetails())
                            return;
                    }
                }
                else
                {
                    lblHeader.Text = "New Certification Compliance Checklist";
                }
            }
        }

        private bool getDetails()
        {
            try
            {
                DataTable dtC = new DataTable();
                DataRow dtCR;
                int intCId = Convert.ToInt32(hfCId.Value);

                dtC = utilBL.getDatasetWithTwoConditionInString("GetCertChecklistDetailById",
                          "", hfCId.Value, strConnectionString);

                if (dtC.Rows.Count == 0)
                {
                    writeError("No records found.");
                    return false;
                }
                else
                {
                    dtCR = dtC.Rows[0];
                    ddlSearchDeptName.SelectedValue = dtCR["CCM_CSSDM_ID"].ToString();
                    //<< Added by Amarjeet on 23-Jul-2021
                    ddlActRegCirc.SelectedValue = dtCR["CCM_ACT_REGULATION_CIRCULAR"].ToString();
                    //>>
                    txtReference.Text = dtCR["CCM_REFERENCE"].ToString();
                    txtTitleofSection.Text = dtCR["CCM_CLAUSE"].ToString();
                    txtParticulars.Text = dtCR["CCM_PARTICULARS"].ToString();
                    txtCheckpoints.Text = dtCR["CCM_CHECK_POINTS"].ToString();
                    txtFrequency.Text = dtCR["CCM_FREQUENCY"].ToString();
                    //<< Added by Amarjeet on 23-Jul-2021
                    txtForms.Text = dtCR["CCM_FORMS"].ToString();
                    //>>
                    //txtDueDate.Text = dtCR["CCM_DUE_DATE"].ToString();
                    txtPenalty.Text = dtCR["CCM_PENALTY"].ToString();

                    //txtSourceDepartment.Text = dtCR["CCM_SOURCE_DEPT"].ToString();
                    //txtDeptRespFurnish.Text = dtCR["CCM_DEPT_RESP_FURNISH"].ToString();
                    //txtDeptRespSubmit.Text = dtCR["CCM_DEPT_RESP_SUBMITTING"].ToString();
                    //txtTobeFiledWith.Text = dtCR["CCM_TO_BE_FILLED_WITH"].ToString();

                    txtEffectiveFromDate.Text = dtCR["Effective From"].ToString();
                    if (!dtCR["CCM_EFFECTIVE_TO"].ToString().Equals(""))
                        txtEffectiveToDate.Text = Convert.ToDateTime(dtCR["CCM_EFFECTIVE_TO"].ToString()).ToString("dd-MMM-yyyy");
                    ddlActInAct.SelectedValue = dtCR["CCM_STATUS"].ToString();
                    txtRemarks.Text = dtCR["CCM_REMARK"].ToString();
                    return true;
                }
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in getDetails :" + exp.Message);
                return false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            try
            {
                int intChecklistID = 0, intChecklistRowId = 0;
                string strDepartmentName = null;// strRelevantTo = null, strActRegulation = null,strNatureOfCompliance = null,
                string strEffectiveFrom = null, strCreateBy = null;
                string strEffectiveTo = null, strStatus = null, strRemark = null;
                string strReference = null, strTitleofSection = null, strSelfAssessmentStatus = null, strCheckpoints = null, strFrequency = null, strDueDate = null,
                 strSourceDept = null, strDeptRespFurnish = null, strDeptRespSubmitting = null, strTobeFilledwith = null, strPenalty = null, strActRegCirc = null,
                 strForms = null;

                strDepartmentName = ddlSearchDeptName.SelectedValue;
                strActRegCirc = ddlActRegCirc.SelectedValue;
                strReference = cm.getSanitizedString(txtReference.Text);
                strTitleofSection = cm.getSanitizedString(txtTitleofSection.Text);
                strSelfAssessmentStatus = cm.getSanitizedString(txtParticulars.Text);
                strCheckpoints = txtCheckpoints.Text;
                strFrequency = txtFrequency.Text;
                strForms = cm.getSanitizedString(txtForms.Text);
                strPenalty = txtPenalty.Text;
                //strDueDate = txtDueDate.Text;
                //strSourceDept = txtSourceDepartment.Text;
                //strDeptRespFurnish = txtDeptRespFurnish.Text;
                //strDeptRespSubmitting = txtDeptRespSubmit.Text;
                //strTobeFilledwith = txtTobeFiledWith.Text;
                strEffectiveFrom = txtEffectiveFromDate.Text;
                strEffectiveTo = txtEffectiveToDate.Text;
                strStatus = ddlActInAct.SelectedValue;
                strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                strRemark = cm.getSanitizedString(txtRemarks.Text);

                //new record to be inserted.
                if (hfCId.Value.Equals(""))
                    intChecklistID = 0;
                //Modify Records
                else
                    intChecklistID = Convert.ToInt32(hfCId.Value);

                if (hftype.Value.Equals("Copy"))
                {
                    intChecklistID = 0;
                }

                intChecklistRowId = certBL.saveCertificationChecklist(intChecklistID, strDepartmentName, strReference, 
                    strTitleofSection, strSelfAssessmentStatus, strCheckpoints, strFrequency, strDueDate, strSourceDept, 
                    strDeptRespFurnish, strDeptRespSubmitting, strTobeFilledwith, strEffectiveFrom, strStatus, strEffectiveTo, 
                    strCreateBy, strRemark, strPenalty, strActRegCirc, strForms, strConnectionString);

                if (hfCId.Value.Equals(""))
                    strId = intChecklistRowId.ToString();
                else
                    strId = hfCId.Value.ToString();

                
                if (hftype.Value.Equals("Copy"))
                {
                    writeError("New Compliance Checklist Clause created successfully.");
                }
                else
                {
                    if (hfCId.Value.Equals(""))
                    {
                        //writeError("New Compliance Checklist created successfully with Id : " + strId + ".");
                        writeError("New Compliance Checklist Clause created successfully.");
                    }
                    else
                    {
                        writeError("Details updated successfully for Compliance Checklist Id : " + strId + ".");
                    }
                }

                btnSubmit.Visible = false;
                btnBack.Visible = true;
                pnlChecklistEdit.Visible = false;
                pnlAdd.Visible = true;
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError(ex.Message);
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
            //string script = "\r\n<script language=\"javascript\">\r\n" +
            //           " alert('" + strError.Replace("'", "\\'") + "');" +
            //           "   </script>\r\n";

            //ClientScript.RegisterStartupScript(this.GetType(), "script", script);
        }

        protected void btnBack_Click(object sender, System.EventArgs e)
        {
            //if (hfSrc.Value.Equals(""))
            //    Response.Redirect(Global.site_url("Default.aspx"));
            //else
            //    Response.Redirect("SearchNullVoidCase.aspx?Type=" + hfSrc.Value.ToString());SearchChecklist.aspx
            Response.Redirect("SearchChecklist.aspx");
        }

        //Added by Vivek on 04-Feb-2016
        protected void cvEffectiveEndDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
    }
}