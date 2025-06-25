using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code.DAL;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code;

namespace Fiction2Fact.Projects.Certification
{
    //[Idunno.AntiCsrf.SuppressCsrfCheck]
    public partial class Certification_ViewChecklist : System.Web.UI.Page
    {

        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ToString();
        UtilitiesBLL utilBL = new UtilitiesBLL();
        UtilitiesVO objVO = new UtilitiesVO();
        UtilitiesBLL utilBLL = new UtilitiesBLL();
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
                if (!IsPostBack)
                {

                    if (!Request.QueryString["CertID"].Equals(""))
                    {
                        hfcertId.Value = Request.QueryString["CertID"].ToString();
                        bindCertificationData();
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


        //<<Added By Vivek on 04-Jul-2017
        private void bindCertificationData()
        {
            try
            {
                DataSet dsView = certBL.searchEditCertifications(Convert.ToInt32(hfcertId.Value), "", "", "", -1, strConnectionString);
                DataTable dt = dsView.Tables[0];
                DataRow dr;

                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];

                    lblDeptName.Text = dr["DeptName"].ToString();

                    if (dr["CQM_FROM_DATE"] != DBNull.Value)
                        lblFromDt.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dr["CQM_FROM_DATE"]));

                    if (dr["CQM_TO_DATE"] != DBNull.Value)
                        lblToDt.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dr["CQM_TO_DATE"]));

                    lblRemarks.Text = dr["CERT_REMARKS"].ToString().Replace("\n", "<br />");
                    lblStatus.Text = dr["Status"].ToString();
                    lblSubmittedBy.Text = dr["CERT_SUBMITTED_BY"].ToString();
                    lblCertificate.Text = dr["CERT_CONTENT"].ToString();

                    if (dr["CERT_SUBMITTED_ON"] != DBNull.Value)
                        lblSubmittedOn.Text = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(dr["CERT_SUBMITTED_ON"]));

                    lblSubmittedRemarks.Text = dr["CERT_SUBMITTED_REMARKS"].ToString().Replace("\n", "<br />");

                    //UH
                    lblHODApprovedBy.Text = dr["CERT_APPROVED_BY_LEVEL1"].ToString();
                    if (dr["CERT_APPROVED_DT_LEVEL1"] != DBNull.Value)
                        lblHODApprovedOn.Text = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(dr["CERT_APPROVED_DT_LEVEL1"]));
                    lblHODApprovedRemarks.Text = dr["CERT_APPROVED_REMARKS_LEVEL1"].ToString().Replace("\n", "<br />");
                    lblHODRejectedBy.Text = dr["CERT_REJECTED_BY_LEVEL1"].ToString();
                    if (dr["CERT_REJECTED_DT_LEVEL1"] != DBNull.Value)
                        lblHODRejectedOn.Text = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(dr["CERT_REJECTED_DT_LEVEL1"]));
                    lblHODRejectedRemarks.Text = dr["CERT_REJECTED_REMARKS_LEVEL1"].ToString().Replace("\n", "<br />");

                    //FH
                    lblCXOApprovedBy.Text = dr["CERT_APPROVED_BY_LEVEL2"].ToString();

                    if (dr["CERT_APPROVED_DT_LEVEL2"] != DBNull.Value)
                        lblCXOApprovedOn.Text = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(dr["CERT_APPROVED_DT_LEVEL2"]));

                    lblCXOApprovedRemarks.Text = dr["CERT_APPROVED_REMARKS_LEVEL2"].ToString().Replace("\n", "<br />");
                    lblCXORejectedBy.Text = dr["CERT_REJECTED_BY_LEVEL2"].ToString();
                    lblCXORejectedRemarks.Text = dr["CERT_REJECTED_REMARKS_LEVEL2"].ToString().Replace("\n", "<br />");
                    if (dr["CERT_REJECTED_DT_LEVEL2"] != DBNull.Value)
                        lblCXORejectedOn.Text = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(dr["CERT_REJECTED_DT_LEVEL2"]));

                    //CU
                    lblCUby.Text = dr["CERT_APPROVED_BY_LEVEL3"].ToString();
                    if (dr["CERT_APPROVED_DT_LEVEL3"] != DBNull.Value)
                        lblCUon.Text = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(dr["CERT_APPROVED_DT_LEVEL3"]));
                    lblCURSby.Text = dr["CERT_REJECTED_BY_LEVEL3"].ToString();
                    if (dr["CERT_REJECTED_DT_LEVEL3"] != DBNull.Value)
                        lblCURSon.Text = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(dr["CERT_REJECTED_DT_LEVEL3"]));
                    lblCURS.Text = dr["CERT_REJECTED_REMARKS_LEVEL3"].ToString().Replace("\n", "<br />");

                    //ECO
                    lblECOApprovedBy.Text = dr["CERT_APPROVED_BY_LEVEL4"].ToString();

                    if (dr["CERT_APPROVED_DT_LEVEL4"] != DBNull.Value)
                        lblECOApprovedOn.Text = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(dr["CERT_APPROVED_DT_LEVEL4"]));

                    lblECOApprovedRemarks.Text = dr["CERT_APPROVED_REMARKS_LEVEL4"].ToString().Replace("\n", "<br />");
                    lblECORejectedBy.Text = dr["CERT_REJECTED_BY_LEVEL4"].ToString();
                    lblECORejectedRemarks.Text = dr["CERT_REJECTED_REMARKS_LEVEL4"].ToString().Replace("\n", "<br />");
                    if (dr["CERT_REJECTED_DT_LEVEL4"] != DBNull.Value)
                        lblECORejectedOn.Text = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(dr["CERT_REJECTED_DT_LEVEL4"]));

                    lblAuditTrail.Text = dr["CERT_AUDIT_TRAIL"].ToString().Replace("\n", "<br />");

                    hfContent.Value = dr["CERT_CONTENT"].ToString();

                    DataTable dtExc = utilBL.getDatasetWithConditionInString("getExceptionByCertIdWithDeptName", hfcertId.Value, strConnectionString);
                    gvException.DataSource = dtExc;
                    gvException.DataBind();
                    Session["CertExceptions"] = dtExc;

                    foreach (GridViewRow gr in gvException.Rows)
                    {
                        Label txtCE_CLOSURE_STATUS = (Label)gr.FindControl("txtCE_CLOSURE_STATUS");
                        Label txtTargetDate = (Label)gr.FindControl("txtTargetDate");

                        HiddenField hfTargetDate = (HiddenField)gr.FindControl("hfTargetDate");
                        HiddenField hfClosureDate = (HiddenField)gr.FindControl("hfClosureDate");

                        if (txtCE_CLOSURE_STATUS.Text != "Closed")
                        {
                            if (!string.IsNullOrEmpty(hfTargetDate.Value))
                            {
                                txtTargetDate.Text = hfTargetDate.Value;
                            }
                            else
                            {
                                txtTargetDate.Text = "";
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(hfClosureDate.Value))
                            {
                                txtTargetDate.Text = hfClosureDate.Value;
                            }
                            else
                            {
                                txtTargetDate.Text = "";
                            }
                        }
                    }

                    // DataTable dtChecklist = utilBL.getDatasetWithCondition("getChecklistByCertId", Convert.ToInt32(hfcertId.Value), strConnectionString);
                    //<< Added By milan yadav on 01-June-2016
                    DataTable dtChecklist = certBL.getChecklistByCertId(Convert.ToInt32(hfcertId.Value), strConnectionString);
                    //>>
                    gvChecklist.DataSource = dtChecklist;
                    gvChecklist.DataBind();
                    Session["CertChecklist"] = dtChecklist;

                }

            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in bindCertificationData(): " + ex.Message);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void BtnClose_click(object sender, EventArgs e)
        {
            string script = "\r\n<script language=\"javascript\">\r\n" +
                                   " closeFileWindow();" +
                                   "   </script>\r\n";

            ClientScript.RegisterStartupScript(this.GetType(), "script", script);
        }


        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

            int uniqueRowId = 0;
            string strChecklistTable = "", strHtmlTableChecklistDetsRows = "";
            DataTable dtChecklistDets;
            DataRow drChecklistDets;
            dtChecklistDets = (DataTable)Session["CertChecklist"];
            string strHtmlTable =
                "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                "<HTML>" +
                "<HEAD>" +
                "</HEAD>" +
                "<BODY>" +

                " <table id='tblChecklistDets' width='100%' align='left' style='margin-left:20px;' " +
                            " cellpadding='0' cellspacing='1' border='1'> " +
                          " <thead> " +
                          " <tr> " +
                          " <th class='tabhead' align='center'> " +
                          " Serial Number " +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Reference Circular/Notification/Act" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Section/Clause" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Compliance of/Heading of Compliance checklist" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Description" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Consequences of non Compliance" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Frequency" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Compliance Status" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Remarks" +
                          " </th> " +
                           " <th class='tabhead' align='center'> " +
                          " Checklist File" +
                          " </th> " +
                          " </tr> " +
                          " </thead> ";


            int intChecklistDetsCnt = dtChecklistDets.Rows.Count;
            for (int intCnt = 0; intCnt < intChecklistDetsCnt; intCnt++)
            {
                uniqueRowId = uniqueRowId + 1;
                drChecklistDets = dtChecklistDets.Rows[intCnt];

                strHtmlTableChecklistDetsRows = strHtmlTableChecklistDetsRows + "<tr>" +
                "<td>" + uniqueRowId + "</td>" +
                "<td>" + drChecklistDets["CCM_REFERENCE"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_CLAUSE"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_CHECK_POINTS"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_PARTICULARS"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_PENALTY"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_FREQUENCY"].ToString() + "</td>" +
                "<td>" + drChecklistDets["Compliance_Status"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCD_REMARKS"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCD_CLIENT_FILENAME"].ToString() + "</td>" +
                "</tr>";
            }
            strChecklistTable = strHtmlTable + strHtmlTableChecklistDetsRows + "</table> " +
            "</BODY>" +
            "</HTML>";
            string attachment = "attachment; filename=Details.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";

            Response.Write(strChecklistTable.ToString());
            Response.End();

        }
        //>>

        protected void btnExportToExcelExceptions_Click(object sender, EventArgs e)
        {
            try
            {
                gvException.AllowPaging = false;
                gvException.AllowSorting = false;

                gvException.DataSource = Session["CertExceptions"];
                gvException.DataBind();
                CommonCodes.PrepareGridViewForExport(gvException);
                string attachment = "attachment; filename=Checklist Exception Details.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();

                HtmlTextWriter htw = new HtmlTextWriter(sw);

                gvException.RenderControl(htw);

                string html2 = Regex.Replace(sw.ToString(), @"(<input type=""image""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
                //html2 = Regex.Replace(html2, @"(<input class=""checkbox""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
                html2 = Regex.Replace(html2, @"(<a \/?[^>]+>)", @"", RegexOptions.IgnoreCase);
                html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
                html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
                Response.Write(html2.ToString());

                //Response.Write(sw.ToString());
                Response.End();
                gvException.AllowPaging = true;
                gvException.AllowSorting = true;
                gvException.DataBind();
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in btnExportToExcel_Click :" + exp);
            }
        }


        public override void VerifyRenderingInServerForm(Control control)
        {

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

        protected void gvChecklist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkViewCirc = (LinkButton)(e.Row.FindControl("lnkViewCirc"));
                HiddenField hfCircularId = (HiddenField)(e.Row.FindControl("hfCircularId"));

                if (hfCircularId.Value.Equals(""))
                {
                    lnkViewCirc.Visible = false;
                }
                else
                {
                    lnkViewCirc.Visible = true;
                }

                lnkViewCirc.OnClientClick = "onClientViewCircClick('" + (new SHA256EncryptionDecryption()).Encrypt(hfCircularId.Value) + "');";
            }
        }
    }
}