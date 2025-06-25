using System;
using System.Data;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_ViewCertificationContent : System.Web.UI.Page
    {
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
                if (!IsPostBack)
                {
                    hfDeptId.Value = Request.QueryString["DeptId"].ToString();
                    hfType.Value = Request.QueryString["Type"].ToString();
                    hfQuarterId.Value = Request.QueryString["Quarter"].ToString();

                    bindDetails();
                }
            }
            //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
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


        public void bindDetails()
        {
            DataTable dt = new DataTable();

            string strFilterQUery = "", strQuery = "", strField = "";

            if (hfType.Value.Equals("L0"))
            {
                strField = " ,CSSDM_NAME as [DeptName] ";
                strFilterQUery += " INNER JOIN TBL_CERT_SUB_SUB_DEPT_MAS on CSSDM_ID = CERTM_DEPT_ID and CERTM_LEVEL_ID = 0 " +
                                  " and CERTM_DEPT_ID = " + hfDeptId.Value;
            }
            else if (hfType.Value.Equals("L1"))
            {
                strField = " ,CSDM_NAME as [DeptName] ";
                strFilterQUery += " INNER JOIN TBL_CERT_SUB_DEPT_MAS on CSDM_ID = CERTM_DEPT_ID and CERTM_LEVEL_ID = 1 " +
                                  " and CERTM_DEPT_ID = " + hfDeptId.Value;
            }
            else if (hfType.Value.Equals("L2"))
            {
                strField = " ,CDM_NAME as [DeptName] ";
                strFilterQUery += " INNER JOIN TBL_CERT_DEPT_MAS on CDM_ID = CERTM_DEPT_ID and CERTM_LEVEL_ID = 2 " +
                                  " and CERTM_DEPT_ID = " + hfDeptId.Value;
            }

            strFilterQUery += " and  CQM_ID = " + hfQuarterId.Value;

            strQuery = " select *" + strField + " from TBL_CERTIFICATIONS " +
                      " inner join TBL_CERT_MAS on CERTM_ID = CERT_CERTM_ID " +
                      " inner join TBL_CERT_QUARTER_MAS on CERT_CQM_ID = CQM_ID ";

            dt = new DataServer().Getdata(strQuery + strFilterQUery);

            if (dt.Rows.Count > 0)
            {
                lblCertificate.Text = dt.Rows[0]["CERT_CONTENT"].ToString();
                lblDeptName.Text = dt.Rows[0]["DeptName"].ToString();
                lblFromDt.Text = Convert.ToDateTime(dt.Rows[0]["CQM_FROM_DATE"]).ToString("dd-MMM-yyyy");
                lblToDt.Text = Convert.ToDateTime(dt.Rows[0]["CQM_TO_DATE"]).ToString("dd-MMM-yyyy");

                if (hfType.Value.Equals("L1"))
                {
                    lblApprovedby.Text = dt.Rows[0]["CERT_APPROVED_BY_LEVEL1"].ToString();
                    if (dt.Rows[0]["CERT_APPROVED_BY_LEVEL1"].ToString() != "")
                    {
                        lblApprovedOn.Text = Convert.ToDateTime(dt.Rows[0]["CERT_APPROVED_DT_LEVEL1"]).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        lblApprovedOn.Text = "";
                    }
                }
                else if (hfType.Value.Equals("L2"))
                {
                    lblApprovedby.Text = dt.Rows[0]["CERT_APPROVED_BY_LEVEL3"].ToString();
                    if (dt.Rows[0]["CERT_APPROVED_BY_LEVEL3"].ToString() != "")
                    {
                        lblApprovedOn.Text = Convert.ToDateTime(dt.Rows[0]["CERT_APPROVED_DT_LEVEL3"]).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        lblApprovedOn.Text = "";
                    }
                }
            }
        }
    }
}