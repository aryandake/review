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

namespace Fiction2Fact.Projects.Certification
{
    public partial class CommonCertContent : System.Web.UI.Page
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
                mvMultiView.ActiveViewIndex = 0;

                if (!CommonCodes.CheckInputValidity(this)) return;
                ShowCertGrid();
            }
        }

        protected void btnViewCancel_Click(object sender, System.EventArgs e)
        {
            mvMultiView.ActiveViewIndex = 0;
            lblMsg.Text = "";
        }

        private void ShowCertGrid()
        {
            try
            {
                int intCId = 0;
                DataTable dsViewCert = new DataTable();
                dsViewCert = certBL.getCertCommonContent();
                gvSearchCertificate.DataSource = dsViewCert;
                gvSearchCertificate.DataBind();

                if ((this.gvSearchCertificate.Rows.Count == 0))
                {
                    this.lblInfo.Text = "No Records found satisfying the criteria.";
                    this.lblInfo.Visible = true;
                }
                else
                {
                    this.lblInfo.Text = String.Empty;
                    this.lblInfo.Visible = false;
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
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void btnEditCancel_Click(object sender, System.EventArgs e)
        {
            mvMultiView.ActiveViewIndex = 0;
            lblMsg.Text = "";
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
                string strContent = null;
                string strCreateBy = null;

                F2FTextBox FCKE_EditCertContents = (F2FTextBox)(fvSearchCertificate.FindControl("FCKE_EditCertContents"));
                strContent = FCKE_EditCertContents.Text;
                if (!Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)).Equals(""))
                {
                    strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                }

                intRowsSaved = certBL.saveCertCommonContent(strCertId, strContent, strCreateBy);
                writeError("Certification Content Updated successfully.");
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

        }
        protected void gvSearchCertificate_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            lblMsg.Text = "";
            DataRow dr;
            string strCertId;
            string strContent;
            DataTable dsView = new DataTable();
            
            try
            {
                if (hfSelectedOperation.Value == "View")
                {
                    strCertId = gvSearchCertificate.SelectedValue.ToString();

                    dsView = certBL.getCertCommonContent();

                    mvMultiView.ActiveViewIndex = 1;
                    fvSearchCertificate.ChangeMode(FormViewMode.ReadOnly);
                    fvSearchCertificate.DataSource = dsView;
                    fvSearchCertificate.DataBind();
                }
                else if (hfSelectedOperation.Value == "Edit")
                {
                    strCertId = gvSearchCertificate.SelectedValue.ToString();

                    dsView = certBL.getCertCommonContent();

                    mvMultiView.ActiveViewIndex = 1;
                    fvSearchCertificate.ChangeMode(FormViewMode.Edit);
                    fvSearchCertificate.DataSource = dsView;
                    fvSearchCertificate.DataBind();
                    dr = dsView.Rows[0];

                    strContent = dr["CCC_CONTENT"].ToString();
                    
                    F2FTextBox FCKE_EditCertContents = (F2FTextBox)(fvSearchCertificate.FindControl("FCKE_EditCertContents"));
                    FCKE_EditCertContents.Text = strContent;
                }
            }
            catch (Exception ex)
            {
                this.lblInfo.Text = ex.Message;
                this.lblInfo.Visible = true;
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}