using System;
//using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using System.Collections.Generic;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_AddSeparateChecklist : System.Web.UI.Page
    {
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CertificationMasterBL certBL = new CertificationMasterBL();
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        DataTable dtQuarter = new DataTable();
        DataTable dtDepartment = new DataTable();
        string strFromDt = "", strToDt = "", strQuarter = "";
        int intRowsInserted = 0;
        ListItem liChkBoxListItem;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvSeparateChecklistrMaster.ActiveViewIndex = 0;
                ListItem li = new ListItem();
                li.Text = "(Select)";
                li.Value = "";
                bind();
                //dtDepartment = utilBL.getDatasetWithConditionInString("getCertDeptByDepId", "", strConnectionString);
                //if (dtDepartment.Rows.Count > 0)
                //{
                //    cbSearchDeptName.DataSource = dtDepartment;
                //    cbSearchDeptName.DataBind();
                //    imgSave.Visible = true;
                //}
                //else
                //{
                //    imgSave.Visible = true;
                //}

                DataSet dsQuarter = utilBL.getDataset("CERTQUARTERS", strConnectionString);
                if (dsQuarter.Tables[0].Rows.Count > 0)
                {
                    ddlActiveQuarter.DataSource = dsQuarter;
                    ddlActiveQuarter.DataBind();
                    dtQuarter = dsQuarter.Tables[0];
                    strFromDt = dtQuarter.Rows[0]["CQM_FROM_DATE"].ToString();
                    strToDt = dtQuarter.Rows[0]["CQM_TO_DATE"].ToString();
                    hfQuarter.Value = dtQuarter.Rows[0]["CQM_ID"].ToString();

                }

            }
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            DataSet dsQuarter = utilBL.getDataset("CERTQUARTERS", strConnectionString);
            dtQuarter = dsQuarter.Tables[0];
            //Added By Milan Yadav on 05-apr-2017
            if (dtQuarter.Rows.Count > 0)
            {
                strQuarter = dtQuarter.Rows[0]["Quarter"].ToString();
                bool bReturn = false;
                lblMsg.Text = "";
                List<string> lstMsgs = new List<string>();
                for (int i = 0; i <= cbSearchDeptName.Items.Count - 1; i++)
                {
                    liChkBoxListItem = cbSearchDeptName.Items[i];
                    if (liChkBoxListItem.Selected)
                    {
                        if (!isCCOCertification(liChkBoxListItem.Value))
                        {
                            string strResult = certBL.ValidateCertificationRollout(Convert.ToInt32(hfQuarter.Value), Convert.ToInt32(liChkBoxListItem.Value));
                            string[] arrResult = strResult.Split('~');
                            if (!arrResult[0].Equals("true"))
                            {
                                lstMsgs.Add(arrResult[1]);
                                bReturn = true;
                            }
                        }
                    }
                }
                if (bReturn)
                {
                    writeError(string.Join("|", lstMsgs));
                    return;
                }
                try
                {
                    for (int i = 0; i <= cbSearchDeptName.Items.Count - 1; i++)
                    {
                        liChkBoxListItem = cbSearchDeptName.Items[i];
                        if (liChkBoxListItem.Selected)
                        {
                            if (isJoinCertification(liChkBoxListItem.Value))
                            {
                                intRowsInserted = certBL.generateSeparateQuarterlyJointCertification(Convert.ToInt32(hfQuarter.Value), Convert.ToInt32(liChkBoxListItem.Value), strConnectionString);
                                if (!intRowsInserted.Equals(0))
                                {
                                    sendCertificateMailDepartmentForJointCertification(Convert.ToInt32(liChkBoxListItem.Value), strQuarter);
                                }
                                else
                                {
                                    writeError("Certification not present for the selected department.");
                                    cbSearchDeptName.SelectedIndex = -1;
                                }
                            }
                            else if (isCCOCertification(liChkBoxListItem.Value))
                            {
                                intRowsInserted = certBL.generateSeparateQuarterlyJointCertification(Convert.ToInt32(hfQuarter.Value), Convert.ToInt32(liChkBoxListItem.Value), strConnectionString);
                                if (!intRowsInserted.Equals(0))
                                {
                                    sendCertificateMailDepartmentForCCOCertification(Convert.ToInt32(liChkBoxListItem.Value), strQuarter);
                                }
                                else
                                {
                                    writeError("Certification not present for the selected department.");
                                    cbSearchDeptName.SelectedIndex = -1;
                                }
                            }
                            else
                            {
                                intRowsInserted = certBL.generateSeparateQuarterlyCertification(Convert.ToInt32(hfQuarter.Value), Convert.ToInt32(liChkBoxListItem.Value), strConnectionString);
                                if (!intRowsInserted.Equals(0))
                                {
                                    sendCertificateMailDepartmentWise(Convert.ToInt32(liChkBoxListItem.Value), strQuarter);
                                }
                                else
                                {
                                    writeError("Certification checklist not present for the selected department.");
                                    cbSearchDeptName.SelectedIndex = -1;
                                }
                            }
                        }
                    }
                    bind();
                    if (!intRowsInserted.Equals(0))
                    {
                        writeError("Certification checklist generated for the selected quarter.");
                        cbSearchDeptName.SelectedIndex = -1;
                    }
                }
                catch (Exception exp)
                {
                    //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                    string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    //>>
                    writeError("btnSave_Click :" + exp);
                }
                hfDoubleClickFlag.Value = String.Empty;
            }
            else
            {
                writeError("There is no active quarter available . Kindly active previous quarter or add new quarter.");
            }
        }

        public bool isJoinCertification(string strDeptId)
        {
            DataTable dt = new DataTable();
            dt = new DataServer().Getdata("select * from [TBL_CERT_DEPT_MAS] where CDM_IS_JOIN_CERTIFICATE = 'Yes' and CDM_ID = " + strDeptId);

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isCCOCertification(string strDeptId)
        {
            DataTable dt = new DataTable();
            dt = new DataServer().Getdata("select * from [TBL_CERT_DEPT_MAS] where CDM_IS_JOIN_CERTIFICATE = 'CH' and CDM_ID = " + strDeptId);

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void writeError(string strError, bool bAlert = true)
        {
            lblMsg.Text = strError.Replace("|", "<br/>");
            lblMsg.Visible = true;
            if (bAlert)
            {
                string script = "\r\n<script language=\"javascript\">\r\n" +
                    " alert('" + strError.Replace("'", "\\'").Replace("|", "\\n") + "');" +
                    "   </script>\r\n";
                ClientScript.RegisterStartupScript(this.GetType(), "script", script);
            }
        }

        private void sendCertificateMailDepartmentForJointCertification(int intDepartment, string strQuarter)
        {
            try
            {
                CommonCode cc = new CommonCode();
                Authentication auth = new Authentication();
                cc.ParamMap.Add("ConfigId", 18);
                cc.ParamMap.Add("To", "CFO");
                cc.ParamMap.Add("cc", "CEO,CertAdmin,Comp");
                cc.ParamMap.Add("SubmittedBy", (new Authentication()).getUserFullName(Page.User.Identity.Name));

                cc.ParamMap.Add("CertDepartmentIdType", "FH");
                cc.ParamMap.Add("CertDepartmentId", intDepartment);
                cc.ParamMap.Add("CertDepartment", "");
                cc.ParamMap.Add("Quarter", strQuarter);
                cc.setCertificationMailContent();
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in sendCertificateMail:" + exp);
            }
        }

        private void sendCertificateMailDepartmentForCCOCertification(int intDepartment, string strQuarter)
        {
            try
            {
                CommonCode cc = new CommonCode();
                Authentication auth = new Authentication();
                cc.ParamMap.Add("ConfigId", 21);
                cc.ParamMap.Add("To", "CCO");
                cc.ParamMap.Add("cc", "CertAdmin");
                cc.ParamMap.Add("SubmittedBy", (new Authentication()).getUserFullName(Page.User.Identity.Name));

                cc.ParamMap.Add("CertDepartmentIdType", "FH");
                cc.ParamMap.Add("CertDepartmentId", intDepartment);
                cc.ParamMap.Add("CertDepartment", "");
                cc.ParamMap.Add("Quarter", strQuarter);
                cc.setCertificationMailContent();
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in sendCertificateMail:" + exp);
            }
        }

        private void sendCertificateMailDepartmentWise(int intDepartment, string strQuarter)
        {
            try
            {
                CommonCode cc = new CommonCode();
                Authentication auth = new Authentication();
                cc.ParamMap.Add("ConfigId", 16);
                cc.ParamMap.Add("To", "Level1");
                cc.ParamMap.Add("cc", "CertAdmin");
                cc.ParamMap.Add("SubmittedBy", (new Authentication()).getUserFullName(Page.User.Identity.Name));

                cc.ParamMap.Add("CertDepartmentIdType", "FH");
                cc.ParamMap.Add("CertDepartmentId", intDepartment);
                cc.ParamMap.Add("CertDepartment", "");
                cc.ParamMap.Add("Quarter", strQuarter);
                cc.setCertificationMailContent();
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in sendCertificateMail:" + exp);
            }
        }

        public void bind()
        {
            cbSearchDeptName.Items.Clear();
            dtDepartment = utilBL.getDatasetWithConditionInString("getCertDeptByDepId", "", strConnectionString);
            if (dtDepartment.Rows.Count > 0)
            {

                cbSearchDeptName.DataSource = dtDepartment;
                cbSearchDeptName.DataBind();
                imgSave.Visible = true;
            }
            else
            {
                imgSave.Visible = false;
            }
        }

    }
}