using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using AjaxControlToolkit;
using Fiction2Fact.F2FControls;
using System.Text.RegularExpressions;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_AddSearchCertMas : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CertificationMasterBL certBL = new CertificationMasterBL();
        int certId;

        protected void Page_Load(object sender, EventArgs e)
        {
            DBUtilityParameter db = new DBUtilityParameter("Col", 1);

            if (!Page.IsPostBack)
            {
                //ListItem li = new ListItem();
                //li.Text = "(Select)";
                //li.Value = "";
                //ddlSearchDeptName.DataSource = utilBL.getDataset("CERTIFICATEDEPT", strConnectionString);
                //ddlSearchDeptName.DataBind();
                //ddlSearchDeptName.Items.Insert(0, li);

                //ddlLevelSearch.DataSource = utilBL.getDataset("CertificationDepartmentLevel", strConnectionString);
                //ddlLevelSearch.DataBind();
                //ddlLevelSearch.Items.Insert(0, li);

                //ddlSpocDeptNameSearch.DataSource = utilBL.getDataset("CertificationSpocDepartment", strConnectionString);
                //ddlSpocDeptNameSearch.DataBind();
                //ddlSpocDeptNameSearch.Items.Insert(0, li);

                //ddlUnitHeadSearch.DataSource = utilBL.getDataset("CertificationUnitHeadDepartment", strConnectionString);
                //ddlUnitHeadSearch.DataBind();
                //ddlUnitHeadSearch.Items.Insert(0, li);

                mvMultiView.ActiveViewIndex = 0;
            }
        }

        protected void btnViewCancel_Click(object sender, System.EventArgs e)
        {
            mvMultiView.ActiveViewIndex = 0;
        }
        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            ShowCertGrid();
        }
        private void ShowCertGrid()
        {
            string strDeptName = "", strLevelSearch = "";//, strSpocDeptName = "", strUnitHeadDeptName = ""
                                                         // int intLevel;
            try
            {
                int intCId = 0;
                strLevelSearch = ddlLevelSearch.SelectedValue.ToString();
                strDeptName = ddlDeptName.SelectedValue;
                DataSet dsViewCert = new DataSet();
                dsViewCert = certBL.SearchCert(intCId, strLevelSearch, strDeptName, strConnectionString);
                gvSearchCertificate.DataSource = dsViewCert;
                gvSearchCertificate.DataBind();

                if ((this.gvSearchCertificate.Rows.Count == 0))
                {
                    writeError("No Records found satisfying the criteria.");
                }
                else
                {
                    writeError("");
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void writeError(string strError)
        {
            if (strError != "")
            {
                lblMsg.Text = strError;
                lblMsg.Visible = true;
            }
            else
            {
                lblMsg.Visible = false;
            }
        }

        protected void btnEditCancel_Click(object sender, System.EventArgs e)
        {
            mvMultiView.ActiveViewIndex = 0;
            writeError("");
        }

        protected void btnAddNewRecord_Click(object sender, System.EventArgs e)
        {
            try
            {
                writeError("");
                mvMultiView.ActiveViewIndex = 1;
                fvSearchCertificate.ChangeMode(FormViewMode.Insert);
                //DropDownList qddlInsDeptName;
                //DropDownList ddlLevel;
                //DropDownList ddlSpocDeptName;
                //DropDownList ddlUnitHead;
                //ddlInsDeptName = (DropDownList)(fvSearchCertificate.FindControl("ddlInsDeptName"));
                //ddlLevel = (DropDownList)(fvSearchCertificate.FindControl("ddlLevel"));
                //ddlSpocDeptName = (DropDownList)(fvSearchCertificate.FindControl("ddlSpocDeptName"));
                //ddlUnitHead = (DropDownList)(fvSearchCertificate.FindControl("ddlUnitHead"));
                //ListItem li = new ListItem();
                //li.Text = "(Select)";
                //li.Value = "";

                CascadingDropDown ccdlLevel = (CascadingDropDown)(fvSearchCertificate.FindControl("cddLevel"));
                if (ccdlLevel != null)
                {
                    ccdlLevel.SelectedValue = "";
                }
                //ddlInsDeptName.SelectedIndex = ddlLevel.SelectedIndex = -1;
                //ddlLevel.ClearSelection();
                //ddlDeptName.ClearSelection();
                //cddLevelSearch.SelectedValue = "";


                //ddlInsDeptName.DataSource = utilBL.getDataset("CERTIFICATEDEPT", strConnectionString);
                //ddlInsDeptName.DataBind();
                //ddlInsDeptName.Items.Insert(0, li);

                //Added By Milan Yadav on 20-Jan-2016
                //>>
                //ddlLevel.DataSource = utilBL.getDataset("CertificationDepartmentLevel", strConnectionString);
                //ddlLevel.DataBind();
                //ddlLevel.Items.Insert(0, li);

                //ddlSpocDeptName.DataSource = utilBL.getDataset("CertificationSpocDepartment", strConnectionString);
                //ddlSpocDeptName.DataBind();
                //ddlSpocDeptName.Items.Insert(0, li);

                //ddlUnitHead.DataSource = utilBL.getDataset("CertificationUnitHeadDepartment", strConnectionString);
                //ddlUnitHead.DataBind();
                //ddlUnitHead.Items.Insert(0, li);
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnInsSave_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            InsertNewCertification();
        }

        protected void InsertNewCertification()
        {
            try
            {
                string strDepartment = "";
                string strContents;
                string strCreateBy;
                int intLevel;
                DropDownList ddlInsDeptName;
                DropDownList ddlLevel;
                //DropDownList ddlSpocDeptName;
                //DropDownList ddlUnitHead;

                F2FTextBox FCKE_InsCertContents = (F2FTextBox)(fvSearchCertificate.FindControl("FCKE_InsCertContents"));
                ddlInsDeptName = (DropDownList)(fvSearchCertificate.FindControl("ddlDeptName"));
                //Added By Milan yadav on 20-Jan-2016
                //<<
                ddlLevel = (DropDownList)(fvSearchCertificate.FindControl("ddlLevelSearch"));
                //ddlSpocDeptName = (DropDownList)(fvSearchCertificate.FindControl("ddlSpocDeptName"));
                //ddlUnitHead = (DropDownList)(fvSearchCertificate.FindControl("ddlUnitHead"));

                //if (ddlLevel.SelectedValue.ToString() =="0")
                //{
                //    strDepartment=ddlSpocDeptName.SelectedValue;
                //}
                //else if (ddlLevel.SelectedValue.ToString() == "1")
                //{
                //    strDepartment=ddlUnitHead.SelectedValue;
                //}
                //else if (ddlLevel.SelectedValue.ToString() =="2")
                //{
                //    strDepartment = ddlInsDeptName.SelectedValue;
                //}
                //>>
                strDepartment = ddlInsDeptName.SelectedValue;
                strContents = FCKE_InsCertContents.Text;
                strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                intLevel = Convert.ToInt32(ddlLevel.SelectedValue);

                CommonMethods cm = new CommonMethods();
                if (cm.checkDuplicate("TBL_CERT_MAS", "CERTM_DEPT_ID", strDepartment, " AND CERTM_LEVEL_ID = '" + intLevel + "'") == true)
                {
                    writeError("Duplicate entry. Please enter different quater details.");
                    return;
                }


                certId = certBL.saveCertificationMas(0, strDepartment, intLevel, strContents, strCreateBy, strConnectionString);
                FCKE_InsCertContents.Text = "";
                mvMultiView.ActiveViewIndex = 0;
                ShowCertGrid();
                writeError("Certification details saved successfully with Id " + certId + ".");
                hfDoubleClickFlag.Value = string.Empty;
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnInsCancel_Click(object sender, System.EventArgs e)
        {
            mvMultiView.ActiveViewIndex = 0;
        }
        protected void btnUpdate_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            updateCertDetails();
            mvMultiView.ActiveViewIndex = 0;
            ShowCertGrid();
        }
        private void updateCertDetails()
        {
            try
            {
                int intRowsSaved;
                string strCertId = gvSearchCertificate.SelectedValue.ToString();
                string strDeptName = null;
                string strContent = null;
                string strCreateBy = null;
                string strDepartment = null;
                DropDownList ddlLevel;
                DropDownList ddlSpocDeptName;
                DropDownList ddlUnitHead;
                int intLevel;

                strDeptName = Convert.ToString(((DropDownList)(fvSearchCertificate.FindControl("ddlEditDeptName"))).SelectedValue);
                TextBox FCKE_EditCertContents = (TextBox)(fvSearchCertificate.FindControl("FCKE_EditCertContents"));
                strContent = FCKE_EditCertContents.Text;

                strContent = Regex.Replace(strContent.ToString(), @"<script(.*?)<\/script>", @"", RegexOptions.IgnoreCase);
                strContent = Regex.Replace(strContent.ToString(), @"=alert", @"", RegexOptions.IgnoreCase);

                // Remove <p>&nbsp;</p> or <p>    </p> or <p><strong>&nbsp;</strong></p>
                string pattern = @"<p class=""MsoBodyText2"" style=""margin-left:36px; text-align:justify; text-indent:-9.0pt"">&nbsp;</p>";
                string result = Regex.Replace(strContent, pattern, "", RegexOptions.IgnoreCase);

                if (!Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)).Equals(""))
                {
                    strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                }

                ddlLevel = (DropDownList)(fvSearchCertificate.FindControl("ddlEditLevel"));
                ddlSpocDeptName = (DropDownList)(fvSearchCertificate.FindControl("ddlEditSpocDeptName"));
                ddlUnitHead = (DropDownList)(fvSearchCertificate.FindControl("ddlEditUnitHead"));


                if (ddlLevel.SelectedValue.ToString() == "0")
                {
                    strDepartment = ddlSpocDeptName.SelectedValue;
                }
                else if (ddlLevel.SelectedValue.ToString() == "1")
                {
                    strDepartment = ddlUnitHead.SelectedValue;
                }
                else if (ddlLevel.SelectedValue.ToString() == "2")
                {
                    strDepartment = strDeptName;
                }
                else if (ddlLevel.SelectedValue.ToString() == "3")
                {
                    strDepartment = strDeptName;
                }

                intLevel = Convert.ToInt32(ddlLevel.SelectedValue.ToString());
                intRowsSaved = certBL.saveCertificationMas(Convert.ToInt32(strCertId), strDepartment, intLevel, strContent, strCreateBy, strConnectionString);
                writeError("Certification Details Updated successfully.");
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

        }
        protected void gvSearchCertificate_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            writeError("");
            DataRow dr;
            string strCertId;
            string strContent, strDeptName;
            string strLevel, strLevelSearch = "", strDepartmentName = "";
            DataSet dsView = new DataSet();
            DropDownList ddlEditDeptName;
            DropDownList ddlEditLevel;
            DropDownList ddlEditSpocDeptName;
            DropDownList ddlEditUnitHead;

            strLevelSearch = ddlLevelSearch.SelectedValue.ToString();
            strDepartmentName = ddlDeptName.SelectedValue;

            try
            {
                if (hfSelectedOperation.Value == "View")
                {
                    strCertId = gvSearchCertificate.SelectedValue.ToString();

                    dsView = certBL.SearchCert(Convert.ToInt32(strCertId), strLevelSearch, strDepartmentName, strConnectionString);

                    mvMultiView.ActiveViewIndex = 1;
                    fvSearchCertificate.ChangeMode(FormViewMode.ReadOnly);
                    fvSearchCertificate.DataSource = dsView;
                    fvSearchCertificate.DataBind();
                }
                else if (hfSelectedOperation.Value == "Edit")
                {
                    strCertId = gvSearchCertificate.SelectedValue.ToString();

                    dsView = certBL.SearchCert(Convert.ToInt32(strCertId), strLevelSearch, strDepartmentName, strConnectionString);

                    mvMultiView.ActiveViewIndex = 1;
                    fvSearchCertificate.ChangeMode(FormViewMode.Edit);
                    fvSearchCertificate.DataSource = dsView;
                    fvSearchCertificate.DataBind();
                    dr = dsView.Tables[0].Rows[0];

                    ddlEditDeptName = (DropDownList)(fvSearchCertificate.FindControl("ddlEditDeptName"));
                    ddlEditLevel = (DropDownList)(fvSearchCertificate.FindControl("ddlEditLevel"));
                    ddlEditSpocDeptName = (DropDownList)(fvSearchCertificate.FindControl("ddlEditSpocDeptName"));
                    ddlEditUnitHead = (DropDownList)(fvSearchCertificate.FindControl("ddlEditUnitHead"));
                    strContent = dr["CERTM_TEXT"].ToString();
                    strDeptName = dr["CERTM_DEPT_ID"].ToString();
                    strLevel = dr["CERTM_LEVEL_ID"].ToString();

                    ListItem li = new ListItem();
                    li.Text = "(Select)";
                    li.Value = "";

                    TextBox FCKE_EditCertContents = (TextBox)(fvSearchCertificate.FindControl("FCKE_EditCertContents"));
                    FCKE_EditCertContents.Text = strContent;


                    ddlEditLevel.DataSource = utilBL.getDataset("CertificationDepartmentLevel", strConnectionString);
                    ddlEditLevel.DataBind();
                    ddlEditLevel.Items.Insert(0, li);
                    ddlEditLevel.SelectedValue = strLevel;

                    //Added By Milan Yadav on 20-Jan-2016
                    //>>
                    string script = "";
                    script += "\r\n <script type=\"text/javascript\">\r\n";
                    script = script + "onLevelEditChange();\r\n";
                    script += "</script>\r\n";
                    ClientScript.RegisterStartupScript(this.GetType(), "return script", script);


                    ddlEditDeptName.DataSource = utilBL.getDataset("CERTIFICATEDEPT", strConnectionString);
                    ddlEditDeptName.DataBind();
                    ddlEditDeptName.Items.Insert(0, li);
                    ddlEditDeptName.SelectedValue = strDeptName;

                    ddlEditSpocDeptName.DataSource = utilBL.getDataset("CertificationSpocDepartment", strConnectionString);
                    ddlEditSpocDeptName.DataBind();
                    ddlEditSpocDeptName.Items.Insert(0, li);
                    ddlEditSpocDeptName.SelectedValue = strDeptName;

                    ddlEditUnitHead.DataSource = utilBL.getDataset("CertificationUnitHeadDepartment", strConnectionString);
                    ddlEditUnitHead.DataBind();
                    ddlEditUnitHead.Items.Insert(0, li);
                    ddlEditUnitHead.SelectedValue = strDeptName;

                }
                else if (hfSelectedOperation.Value == "Delete")
                {
                    deleteCert();
                    ShowCertGrid();
                }
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void deleteCert()
        {
            int intCertId = Convert.ToInt32(gvSearchCertificate.SelectedValue.ToString());
            certBL.deleteCertificate(intCertId, strConnectionString);
            writeError("Record has been successfully deleted.");
        }

    }
}