using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Data.SqlClient;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_CertificationChecklistDetails : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utilBL = new UtilitiesBLL();

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
                    getChecklistDetails(mstrConnectionString);
                }
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Invalid Parameter input.");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page != null)
            {
                Page.RegisterRequiresViewStateEncryption();
            }
        }


        private void writeError(string strError)
        {

        }

        public void getChecklistDetails(string strConnectionString)
        {
            string strId = "";
            DataTable dt_checklist = new DataTable();
            try
            {
                if (Request.QueryString["Id"] != null)
                {
                    strId = Request.QueryString["Id"].ToString();
                    dt_checklist = utilBL.getDatasetWithConditionInString("ChecklistDetails",
                          strId, strConnectionString);
                    if (dt_checklist.Rows.Count > 0)
                    {
                        gvChecklist.DataSource = dt_checklist;
                    }
                    else
                    {
                        gvChecklist.DataSource = null;
                    }
                    gvChecklist.DataBind();
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("system exception in getChecklistDetails(): " + ex);
            }
        }
    }
}