using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    //[Idunno.AntiCsrf.SuppressCsrfCheck]
    public partial class Certification_ChecklistView : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CertificationMasterBL certBL = new CertificationMasterBL();
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (User.IsInRole("Certification_Admin") || User.IsInRole("Certification_Compliance_User"))
                {
                    if (Request.QueryString["CID"] != null)
                    {
                        hfCId.Value = encdec.Decrypt(Request.QueryString["CID"].ToString());
                        getDetails();
                    }
                }
                else
                {
                    lblMsg.Text = "You are not authorized to view this record.";
                }
            }
        }

        private void getDetails()
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
                }
                else
                {
                    dtCR = dtC.Rows[0];
                    lblChecklistNo.Text = dtCR["CCM_ID"].ToString();
                    lblDepartment.Text = dtCR["CSSDM_NAME"].ToString();
                    //<< Added by Amarjeet on 23-Jul-2021
                    lblActRegCird.Text = dtCR["CDTM_TYPE_OF_DOC"].ToString().Replace("\n", "</br>");
                    //>>
                    lblReference.Text = dtCR["CCM_REFERENCE"].ToString().Replace("\n", "</br>");
                    lblTitleofSection.Text = dtCR["CCM_CLAUSE"].ToString().Replace("\n", "</br>");
                    lblParticulars.Text = dtCR["CCM_PARTICULARS"].ToString().Replace("\n", "</br>");
                    lblCheckpoints.Text = dtCR["CCM_CHECK_POINTS"].ToString().Replace("\n", "</br>");
                    //<< Added by Amarjeet on 23-Jul-2021
                    lblPenalty.Text = dtCR["CCM_PENALTY"].ToString().Replace("\n", "</br>");
                    //>>
                    lblFrequency.Text = dtCR["CCM_FREQUENCY"].ToString().Replace("\n", "</br>");
                    //<< Added by Amarjeet on 23-Jul-2021
                    lblForms.Text = dtCR["CCM_FORMS"].ToString().Replace("\n", "</br>");
                    //>>
                    //lblDueDate.Text = dtCR["CCM_DUE_DATE"].ToString().Replace(Environment.NewLine, "</br>");
                    //lblSourceDepartment.Text = dtCR["CCM_SOURCE_DEPT"].ToString().Replace(Environment.NewLine, "</br>");
                    //lblDeptRespFurnish.Text = dtCR["CCM_DEPT_RESP_FURNISH"].ToString().Replace(Environment.NewLine, "</br>");
                    //lblDeptRespSubmit.Text = dtCR["CCM_DEPT_RESP_SUBMITTING"].ToString().Replace(Environment.NewLine, "</br>");
                    //lblTobeFiledWith.Text = dtCR["CCM_TO_BE_FILLED_WITH"].ToString().Replace(Environment.NewLine, "</br>");

                    lblEffectiveFrom.Text = dtCR["Effective From"].ToString();
                    if (!dtCR["CCM_EFFECTIVE_TO"].ToString().Equals(""))
                        lblEffectiveTo.Text = Convert.ToDateTime(dtCR["CCM_EFFECTIVE_TO"].ToString()).ToString("dd-MMM-yyyy");
                    if (dtCR["CCM_STATUS"].ToString() != null && dtCR["CCM_STATUS"].ToString() != "")
                    {
                        if (dtCR["CCM_STATUS"].ToString() == "A")
                            lblActIAct.Text = "Active";
                        else
                            lblActIAct.Text = "InActive";
                    }
                    lblRemark.Text = dtCR["CCM_REMARK"].ToString().Replace("\n", "</br>");

                }
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in getDetails :" + exp.Message);
            }
        }

        protected void btnBack_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("SearchChecklist.aspx");
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
            string script = "\r\n<script language=\"javascript\">\r\n" +
                       " alert('" + strError.Replace("'", "\\'") + "');" +
                       "   </script>\r\n";

            ClientScript.RegisterStartupScript(this.GetType(), "script", script);
        }
    }
}