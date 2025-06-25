using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.DAL;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class ViewChecklistData : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CommonMethods cm = new CommonMethods();
        UtilitiesVO objVO = new UtilitiesVO();
        CertificationMasterBL certBL = new CertificationMasterBL();
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
                    writeError("");

                    string strDetails1 = "", strUserName = "";
                    string[] strNameAndEmail = new string[0];
                    Authentication auth = new Authentication();
                    strDetails1 = auth.getUserFullName(Page.User.Identity.Name);
                    strNameAndEmail = strDetails1.Split('|');
                    strUserName = strNameAndEmail[0];
                    hfName.Value = strUserName.ToString();

                    if (Request.QueryString["ChecklistId"] != null)
                    {
                        hfChecklistID.Value = Request.QueryString["ChecklistId"].ToString();
                        string strChecklistId = hfChecklistID.Value;

                        getChecklistByID(strChecklistId);

                    }
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


        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }
        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }


        private void getChecklistByID(string strId)
        {
            try
            {
                DataTable dtChecklistDets = new DataTable();
                DataTable dtChecklist = certBL.getChecklistByCertDetsId(Convert.ToInt32(strId), mstrConnectionString);
                DataRow dr = dtChecklist.Rows[0];
                lblID.Text = dr["CCD_ID"].ToString();

                objVO.setCode(" and CCD_ID = " + lblID.Text);
                dtChecklistDets = utilBL.getData("getChecklistFile", objVO);
                if (dtChecklistDets.Rows.Count > 0)
                {
                    dlAttachedFiles.DataSource = dtChecklistDets;
                    dlAttachedFiles.DataBind();
                }

                lblActRegCirc.Text = dr["CDTM_TYPE_OF_DOC"].ToString();
                lblSectionRegulation.Text = dr["CCM_REFERENCE"].ToString().Replace("\n", "<br />");
                lblTitleOfSectionReq.Text = dr["CCM_CLAUSE"].ToString().Replace("\n", "<br />");
                lblCheckPoint.Text = dr["CCM_CHECK_POINTS"].ToString().Replace("\n", "<br />");
                lblParticulars.Text = dr["CCM_PARTICULARS"].ToString().Replace("\n", "<br />");
                lblPenalty.Text = dr["CCM_PENALTY"].ToString().Replace("\n", "<br />");
                lblTimeLimit.Text = dr["CCM_FREQUENCY"].ToString();
                lblForms.Text = dr["CCM_FORMS"].ToString();
                lblComplianceStatus.Text = dr["Compliance_Status"].ToString();
                lblRemarks.Text = dr["CCD_REMARKS"].ToString().Replace("\n", "<br />");
                lblTargetDate.Text = (dr["CCD_TARGET_DATE"] is DBNull ? "" : Convert.ToDateTime(dr["CCD_TARGET_DATE"]).ToString("dd-MMM-yyyy"));
                lblActionPlan.Text = dr["CCD_ACTION_PLAN"].ToString().Replace("\n", "<br />");

                //lblChecklistFile.Text = dr["CCD_CLIENT_FILENAME"].ToString();

                Session["CertChecklistData"] = dtChecklist;

            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in getChecklistByID:" + exp);
            }

        }
        public DataTable LoadChecklistFile(object strchklstDetsid)
        {
            DataTable dtChecklistDets = new DataTable();
            try
            {

                objVO.setCode(" and CCD_ID = " + strchklstDetsid.ToString());
                dtChecklistDets = utilBL.getData("getChecklistFile", objVO);

            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("System Exception in LoadChecklistFile():" + ex.Message);
            }
            return dtChecklistDets;
        }
        //protected void btnExportToExcel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //gvChecklist.AllowPaging = false;
        //        //gvChecklist.AllowSorting = false;

        //        //gvChecklist.DataSource = Session["CertChecklistData"];
        //        //gvChecklist.DataBind();
        //        //PrepareGridViewForExport(gvChecklist);
        //        //string attachment = "attachment; filename=Checklist Details.xls";
        //        //Response.ClearContent();
        //        //Response.AddHeader("content-disposition", attachment);
        //        //Response.ContentType = "application/ms-excel";
        //        //StringWriter sw = new StringWriter();

        //        //HtmlTextWriter htw = new HtmlTextWriter(sw);

        //        //gvChecklist.RenderControl(htw);

        //        //string html2 = Regex.Replace(sw.ToString(), @"(<input type=""image""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
        //        //html2 = Regex.Replace(html2, @"(<a \/?[^>]+>)", @"", RegexOptions.IgnoreCase);
        //        //html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
        //        //html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
        //        //Response.Write(html2.ToString());
        //        //Response.End();
        //        //gvChecklist.AllowPaging = true;
        //        //gvChecklist.AllowSorting = true;
        //        //gvChecklist.DataBind();
        //    }
        //    catch (Exception exp)
        //    {
        //        writeError("Exception in btnExportToExcel_Click :" + exp);
        //    }
        //}
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}