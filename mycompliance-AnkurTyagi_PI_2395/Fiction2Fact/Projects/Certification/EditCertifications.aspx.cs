using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.DAL;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.Legacy_App_Code.Certification;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_EditCertifications : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        UtilitiesVO utilVO = new UtilitiesVO();
        CertificationMasterBL certBL = new CertificationMasterBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ListItem li = new ListItem();
                li.Text = "(Select)";
                li.Value = "";
                ddlQuarter.DataSource = utilBL.getDataset("CERTIFICATEQUARTER", strConnectionString);
                ddlQuarter.DataBind();
                ddlQuarter.Items.Insert(0, li);
                mvMultiView.ActiveViewIndex = 0;
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            ShowCertificates();
        }

        private void ShowCertificates()
        {
            try
            {
                int intCertId = 0, intLevel;
                string strQuarter = ddlQuarter.SelectedValue;
                string strDeptName = ddlDeptName.SelectedValue;
                string strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                intLevel = Convert.ToInt32(ddlLevelSearch.SelectedValue);
                DataSet dsViewCert = new DataSet();
                //dsViewCert = certBL.searchEditCertifications(intCertId, strDeptName, strQuarter, strCreateBy, strConnectionString);
                dsViewCert = certBL.viewPastCertifications(intCertId, strDeptName, strQuarter, strCreateBy, intLevel, strConnectionString);
                gvCertEdit.DataSource = dsViewCert;
                gvCertEdit.DataBind();

                if ((this.gvCertEdit.Rows.Count == 0))
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
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError(exp.Message);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void gvCertEdit_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            lblMsg.Text = "";
            string strCertId;
            DataSet dsView = new DataSet();
            string strDeptId, strQuarterId, strDeptName;
            string strLevel;
            DataTable dtChecklist = new DataTable();
            GridViewRow gvr;
            try
            {
                strCertId = gvCertEdit.SelectedValue.ToString();
                gvr = gvCertEdit.SelectedRow;
                strLevel = ((HiddenField)(gvr.FindControl("hfLevel"))).Value;
                strDeptId = ((HiddenField)(gvr.FindControl("hfDeptId"))).Value;
                strQuarterId = ((HiddenField)(gvr.FindControl("hfQuarterId"))).Value;
                strDeptName = ((Label)(gvr.FindControl("lblId"))).Text;

                string strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));

                // dsView = certBL.searchEditCertifications(Convert.ToInt32(strCertId), "", "", strCreateBy, strConnectionString);
                dsView = certBL.viewPastCertifications(Convert.ToInt32(strCertId), "", "", strCreateBy, Convert.ToInt32(strLevel), strConnectionString);

                mvMultiView.ActiveViewIndex = 1;
                fvEditCert.ChangeMode(FormViewMode.ReadOnly);
                Session["CertificationDetails"] = dsView;
                DataTable dtCntDr = dsView.Tables[0];
                DataRow drCntDr = dtCntDr.Rows[0];
                string strcertId = drCntDr["CERT_ID"].ToString();
                hfContent.Value = drCntDr["CERT_CONTENT"].ToString();

                string strFromDate = (Convert.ToDateTime(dsView.Tables[0].Rows[0]["CQM_FROM_DATE"].ToString())).ToString("dd-MMM-yyyy");
                string strToDate = (Convert.ToDateTime(dsView.Tables[0].Rows[0]["CQM_TO_DATE"].ToString())).ToString("dd-MMM-yyyy");

                RegulatoryReportingDashboard rrd = new RegulatoryReportingDashboard();
                litRegulatoryFilling.Text = rrd.GetFilingsDashboardSingleRow_QID(strDeptId, strDeptName, strLevel, strQuarterId);

                //DataTable dtExc = utilBL.getDatasetWithCondition("getExceptionByCertId", Convert.ToInt32(strcertId), strConnectionString);
                //<< Added By Milan Yadav on 24-Apr-2017
                DataTable dtExc = certBL.viewConsolidateExceptions(strCertId, strDeptId, strQuarterId, strLevel, "", strConnectionString);

                //>>
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

                fvEditCert.DataSource = dsView;
                fvEditCert.DataBind();

                //Added By milan yadav on 01-June-2016
                //>>
                //dtChecklist = certBL.getChecklistByCertId(Convert.ToInt32(strCertId), strConnectionString);
                //<<Added By Milan Yadav on 15-Apr-2017
                dtChecklist = certBL.viewPastConsolidateChecklist(strCertId, strDeptId, strQuarterId, strLevel, "", strConnectionString);

                //<<
                gvChecklist.DataSource = dtChecklist;
                gvChecklist.DataBind();
                Session["SearchCertChecklist"] = dtChecklist;
                //Added By milan yadav on 01-June-2016
                //>>
                //DataTable dtAllChecklist = certBL.getAllChecklistByCertificationId(Convert.ToInt32(strCertId), strConnectionString);
                //<<
                //gvAllChecklist.DataSource = dtAllChecklist;
                //gvAllChecklist.DataBind();
                //Session["CertAllChecklist"] = dtAllChecklist;

                //DataTable dtAllExceptions = utilBL.getDatasetWithCondition("getAllCertExceptions", Convert.ToInt32(strCertId), strConnectionString);
                //gvAllExceptions.DataSource = dtAllExceptions;
                //gvAllExceptions.DataBind();
                //Session["CertAllExceptions"] = dtAllExceptions;

            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                this.lblMsg.Text = ex.Message;
                this.lblMsg.Visible = true;
            }
        }

        protected void btnViewCancel_Click(object sender, System.EventArgs e)
        {
            mvMultiView.ActiveViewIndex = 0;
            lblMsg.Text = "";
        }

        //Added by Milan Yadav on 014-Oct-2016
        //<<
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

            int uniqueRowId = 0;
            string strChecklistTable = "", strHtmlTableChecklistDetsRows = "";
            DataTable dtChecklistDets;
            DataRow drChecklistDets;
            dtChecklistDets = (DataTable)Session["SearchCertChecklist"];
            string strHtmlTable =
                "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                "<HTML>" +
                "<HEAD>" +
                "</HEAD>" +
                "<BODY>" +

                " <table id='tblChecklistDets' classs='table table-bordered footable' width='100%' align='left' " +
                            " cellpadding='0' cellspacing='1' border='1'> " +
                          " <thead> " +
                          " <tr> " +
                          " <th class='tabhead' align='center'> " +
                          " Serial Number " +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Department Name" +
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
                //if (DateTime.TryParse(drChecklistDets[""].ToString(), out dtDateOfReleased))
                //{
                //    strDateOfReleased = dtDateOfReleased.ToString("dd-MMM-yyyy");
                //}

                strHtmlTableChecklistDetsRows = strHtmlTableChecklistDetsRows + "<tr>" +
                "<td>" + uniqueRowId + "</td>" +
                "<td>" + drChecklistDets["DeptName"].ToString() + "</td>" +
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
                utilVO.setCode(" and CCD_ID = " + strchklstDetsid.ToString());
                dtChecklistDets = utilBL.getData("getChecklistFile", utilVO);
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

        //<<Modified by Ankur Tyagi on 01Feb2024 for CR_1945
        protected void btnExportToDoc_Click(object sender, EventArgs e)
        {
            string strHostingServer = (ConfigurationManager.AppSettings["HostingServer"].ToString());

            int uniqueRowId = 0;
            int uniqueRowIdException = 0;
            string strHtmlTableChecklistDetsRows = "";
            string strHtmlDocument = "";
            string strHtmlException = "";
            string strccdTargetDate = "";
            string strTargetDate = "";
            string strFontName = ConfigurationManager.AppSettings["FontName"].ToString();
            DataTable dtChecklistDeatils = new DataTable();
            dtChecklistDeatils = (DataTable)Session["SearchCertChecklist"];
            DataRow drChecklistDets;
            DataTable dtChecklistDeatils1 = new DataTable();
            DataSet ds = new DataSet();
            DataTable dtException = new DataTable();
            DataSet dsException = new DataSet();
            DataRow drChecklistDets1;
            DataRow drException;
            string strHtmlCertificationDets = "", strHtmlTableExceptionDetsRows = "", strSpocSubmittedOn = "", strHODSubmittedOn = "",
                strCUSubmittedOn = "", strCXOSubmittedOn = "", strUnitName = "", strExCoSubmittedOn = "";

            ds = (DataSet)Session["CertificationDetails"];
            dtChecklistDeatils1 = ds.Tables[0];

            //dsException =(DataSet)Session["CertExceptions"];
            //dtException = dsException.Tables[0];
            dtException = (DataTable)Session["CertExceptions"];
            string strHeaderStyle =

                "style =\"text-align:center;" +
                "color: #4d5355;" +
                "font-family:" + strFontName + ";" +
                "font-weight:bold;" +
                "font-size:18px;\"";

            string strTableHeaderStyle =
                "style =\"text-align:justify;" +
                "padding: 4px 2px;" +
                "color: #FFFFFF;" +
                "background: #0083ca repeat top;" +
                "font-family:" + strFontName + ";" +
                "font-weight:bold;" +
                "font-size:14px;\"";

            string strTableCellStyle = "style =\"text-align: left;" + //left//justify
                "vertical-align:top;" +
                "padding: 4px 4px 4px 4px;" +
                "color: #4d5355;" + //#000000
                "background: #ffffff repeat top;" +
                "font-family:" + strFontName + ";" +
                "font-weight:normal;" +
                "font-size:14px;" +
                "\"";
            string strTabHeadStyle = "style =\"text-align:justify;" +
                                    " text-align:left;" +
                                    " padding: 4px 2px; " +
                                    " color: #4d5355;" +
                                    " background: #ebebeb repeat top;" +
                                    " font-family:" + strFontName + ";" +
                                    " font-weight:normal;" +
                                    " font-size:14px;" +
                                    "\"";

            string strTabBodyStyle = "style =\"" +
                                    "padding: 5px;" +
                                    "background: #F8F8F8;" +
                                    "font-family:" + strFontName + ";" +
                                    "font-weight:normal;" +
                                    "font-size:14px;" +
                                    "text-align:left;" +
                                    "color:#4d5355;" +
                                    "\"";

            string strHeaderTitle = "Compliance Certificate for the Quarter " + ddlQuarter.SelectedItem.Text;

            strHtmlDocument = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                            "<html>" +
                            "<head>" +
                            "</head>" +
                            "<body>" +
                            "<center><table class='table table-bordered footable' width='100%'>" +
                            "<tr><td " +
                            strHeaderStyle + ">" + strHeaderTitle + "</td></tr>" +
                            "</table><br/></center>";


            int intChecklistDetsCnt1 = dtChecklistDeatils1.Rows.Count;
            uniqueRowId = 0;
            for (int intCnt = 0; intCnt < intChecklistDetsCnt1; intCnt++)
            {
                uniqueRowId = uniqueRowId + 1;
                drChecklistDets1 = dtChecklistDeatils1.Rows[intCnt];

                if (drChecklistDets1["CERT_SUBMITTED_ON"] != null && drChecklistDets1["CERT_SUBMITTED_ON"].ToString() != "")
                {
                    strSpocSubmittedOn = Convert.ToDateTime(drChecklistDets1["CERT_SUBMITTED_ON"].ToString()).ToString("dd-MMM-yyyy hh:mm:ss tt");
                }
                if (drChecklistDets1["CERT_APPROVED_DT_LEVEL1"] != null && drChecklistDets1["CERT_APPROVED_DT_LEVEL1"].ToString() != "")
                {
                    strHODSubmittedOn = Convert.ToDateTime(drChecklistDets1["CERT_APPROVED_DT_LEVEL1"].ToString()).ToString("dd-MMM-yyyy hh:mm:ss tt");
                }
                if (drChecklistDets1["CERT_APPROVED_DT_LEVEL2"] != null && drChecklistDets1["CERT_APPROVED_DT_LEVEL2"].ToString() != "")
                {
                    strCXOSubmittedOn = Convert.ToDateTime(drChecklistDets1["CERT_APPROVED_DT_LEVEL2"].ToString()).ToString("dd-MMM-yyyy hh:mm:ss tt");
                }
                if (drChecklistDets1["CERT_APPROVED_DT_LEVEL3"] != null && drChecklistDets1["CERT_APPROVED_DT_LEVEL3"].ToString() != "")
                {
                    strCUSubmittedOn = Convert.ToDateTime(drChecklistDets1["CERT_APPROVED_DT_LEVEL3"].ToString()).ToString("dd-MMM-yyyy hh:mm:ss tt");
                }
                if (drChecklistDets1["CERT_APPROVED_DT_LEVEL4"] != null && drChecklistDets1["CERT_APPROVED_DT_LEVEL4"].ToString() != "")
                {
                    strExCoSubmittedOn = Convert.ToDateTime(drChecklistDets1["CERT_APPROVED_DT_LEVEL4"].ToString()).ToString("dd-MMM-yyyy hh:mm:ss tt");
                }
                strUnitName = drChecklistDets1["DeptName"].ToString();
                strHtmlCertificationDets =

                    " <table id='tblCertificationDets' table border='0' class='table table-bordered footable' " +
                    "cellpadding='2' cellspacing='2' width='100%'> " +
                    " <tr style=\"background-color: #ffffff;\"> " +
                    " <td width='30%' " + strTabHeadStyle + "> " +
                    " Department Name:" +
                    " </td> " +
                    " <td width='70%' " + strTabBodyStyle + "> " +
                    " " + drChecklistDets1["DeptName"] + " " +
                    " </td></tr>";

                if (ddlLevelSearch.SelectedItem.Value == "0")
                {
                    strHtmlCertificationDets += "<tr> <td " + strTabHeadStyle + "> " +
                    " Submitted By:" +
                    " </td> " +
                    " <td " + strTabBodyStyle + "> " +
                    " " + drChecklistDets1["CERT_SUBMITTED_BY"].ToString() + " " +
                    " </td></tr>" +
                    " <tr><td " + strTabHeadStyle + "> " +
                    " Submitted On:" +
                    " </td> " +
                    " <td " + strTabBodyStyle + "> " +
                    " " + strSpocSubmittedOn + " " +
                    " </td></tr>";
                }
                else if (ddlLevelSearch.SelectedItem.Value == "1")
                {
                    strHtmlCertificationDets += "<tr> <td " + strTabHeadStyle + "> " +
                    " Approved By (Unit Head):" +
                    " </td> " +
                    " <td " + strTabBodyStyle + "> " +
                    " " + drChecklistDets1["CERT_APPROVED_BY_LEVEL1"].ToString() + " " +
                    " </td></tr>" +
                    "<tr> <td " + strTabHeadStyle + "> " +
                    " Approved On (Unit Head):" +
                    " </td> " +
                    " <td " + strTabBodyStyle + "> " +
                    " " + strHODSubmittedOn + " " +
                    " </td></tr>";
                }
                else if (ddlLevelSearch.SelectedItem.Value == "2")
                {
                    strHtmlCertificationDets += "<tr> <td " + strTabHeadStyle + "> " +
                        " Approved By (Function Head):" +
                        " </td> " +
                        " <td " + strTabBodyStyle + "> " +
                        " " + drChecklistDets1["CERT_APPROVED_BY_LEVEL2"].ToString() + " " +
                        " </td></tr>" +
                        "<tr> <td " + strTabHeadStyle + "> " +
                        " Approved On (Function Head):" +
                        " </td> " +
                        " <td " + strTabBodyStyle + "> " +
                        " " + strCUSubmittedOn + " " +
                        " </td></tr>";

                    strHtmlCertificationDets += "<tr> <td " + strTabHeadStyle + "> " +
                    " Approved By (Compliance User):" +
                    " </td> " +
                    " <td " + strTabBodyStyle + "> " +
                    " " + drChecklistDets1["CERT_APPROVED_BY_LEVEL3"].ToString() + " " +
                    " </td></tr>" +
                    "<tr> <td " + strTabHeadStyle + "> " +
                    " Approved On (Compliance User):" +
                    " </td> " +
                    " <td " + strTabBodyStyle + "> " +
                    " " + strCXOSubmittedOn + " " +
                    " </td></tr>";
                }
                else if (ddlLevelSearch.SelectedItem.Value == "3")
                {
                    strHtmlCertificationDets += "<tr> <td " + strTabHeadStyle + "> " +
                    " Approved By (Executive Member):" +
                    " </td> " +
                    " <td " + strTabBodyStyle + "> " +
                    " " + drChecklistDets1["CERT_APPROVED_BY_LEVEL4"].ToString() + " " +
                    " </td></tr>" +
                    "<tr> <td " + strTabHeadStyle + "> " +
                    " Approved On (Executive Member):" +
                    " </td> " +
                    " <td " + strTabBodyStyle + "> " +
                    " " + strExCoSubmittedOn + " " +
                    " </td></tr>";
                }


                strHtmlCertificationDets += "<tr> <td text-align: center; colspan='2'" + strTabHeadStyle + "> " +
                //" Certificate" +
                " </td> </tr>" +
                " <tr><td  colspan='2'" +
                strTabBodyStyle +
                "> " +
                "<div style='text-align: left;" +
                "font-family:" + strFontName + ";" +
                "background: #F8F8F8;" +
                "font-weight:normal;" +
                "font-size:12px;'>" +
                " " + hfContent.Value + " " +
                "</div>" +
                " </td></tr></table><br/>";
            }

            strHtmlDocument = strHtmlDocument + strHtmlCertificationDets;

            strHtmlDocument = strHtmlDocument +
                "<div style='text-align: center;" +
                "font-family:" + strFontName + ";" +
                "font-weight:bold;" +
                "font-size:18px;" +
                "page-break-before: always;'>" +
                "Compliance Checklist</div><br/><br/>" +
                "<table id='tblChecklistDets' border='0' class='table table-bordered footable' " +
                " cellpadding='2' cellspacing='1'> " +
                " <thead> " +
                " <tr> " +
                " <th " + strTableHeaderStyle + "> " +
                " Serial Number " +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Reference Circular/Notification/Act" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Section/Clause" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Compliance of/Heading of Compliance checklist" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Description" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Consequences of non Compliance" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Frequency" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Compliance Status" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Remarks" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Target Date" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Action Plan" +
                " </th> " +
                " </tr> " +
                " </thead>";

            int intChecklistDetsCnt = dtChecklistDeatils.Rows.Count;
            uniqueRowId = 0;
            for (int intCnt = 0; intCnt < intChecklistDetsCnt; intCnt++)
            {
                uniqueRowId = uniqueRowId + 1;
                drChecklistDets = dtChecklistDeatils.Rows[intCnt];

                //<<Modified by Ankur Tyagi on 02Feb2024 for CR_1945
                if (drChecklistDets["CCD_TARGET_DATE"] != null && drChecklistDets["CCD_TARGET_DATE"].ToString() != "")
                {
                    strccdTargetDate = Convert.ToDateTime(drChecklistDets["CCD_TARGET_DATE"].ToString()).ToString("dd-MMM-yyyy");
                }
                //>>

                strHtmlTableChecklistDetsRows = strHtmlTableChecklistDetsRows + "<tr>" +
                                               "<td " + strTableCellStyle + ">" + (intCnt + 1) + "</td>" +
                                               "<td " + strTableCellStyle + ">" + drChecklistDets["CCM_REFERENCE"].ToString().Replace("\n", "<br />") + "</td>" +
                                               "<td " + strTableCellStyle + ">" + drChecklistDets["CCM_CLAUSE"].ToString().Replace("\n", "<br />") + "</td>" +
                                               "<td " + strTableCellStyle + ">" + drChecklistDets["CCM_CHECK_POINTS"].ToString().Replace("\n", "<br />") + "</td>" +
                                               "<td " + strTableCellStyle + ">" + drChecklistDets["CCM_PARTICULARS"].ToString().Replace("\n", "<br />") + "</td>" +
                                               "<td " + strTableCellStyle + ">" + drChecklistDets["CCM_PENALTY"].ToString().Replace("\n", "<br />") + "</td>" +
                                               "<td " + strTableCellStyle + ">" + drChecklistDets["CCM_FREQUENCY"].ToString().Replace("\n", "<br />") + "</td>" +
                                               "<td " + strTableCellStyle + ">" + drChecklistDets["Compliance_Status"].ToString().Replace("\n", "<br />") + "</td>" +
                                               "<td " + strTableCellStyle + ">" + drChecklistDets["CCD_REMARKS"].ToString().Replace("\n", "<br />") + "</td>" +
                                               //<<Modified by Ankur Tyagi on 02Feb2024 for CR_1945
                                               "<td " + strTableCellStyle + ">" + strccdTargetDate + "</td>" +
                                               "<td " + strTableCellStyle + ">" + drChecklistDets["CCD_ACTION_PLAN"].ToString().Replace("\n", "<br />") + "</td>" +
                                               //>>
                                               "</tr>";
            }

            strHtmlDocument = strHtmlDocument + strHtmlTableChecklistDetsRows + "</table>" + "</body>" +

          "</html>";
            strHtmlException = strHtmlException +
                "<div style='text-align: center;" +
                "font-family:" + strFontName + ";" +
                "font-weight:bold;" +
                "font-size:18px;" +
                "page-break-before: always;'>" +
                "Compliance Deviations</div><br/><br/>" +
                "<table id='tblException' width='100%' border='0' class='table table-bordered footable' " +
                " cellpadding='2' cellspacing='1'> " +
                " <thead> " +
                " <tr> " +
                " <th " + strTableHeaderStyle + "> " +
                " Serial Number " +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Attached File " +
                " </th> " +
                 " <th " + strTableHeaderStyle + "> " +
                " Deviation (Detailed)" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Regulatory Reference (Detailed)" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Root Cause for the Deviation" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Action taken" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Current Status" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Target Date" +
                " </th> " +
                " </tr> " +
                " </thead>";
            int intExceptionCnt = dtException.Rows.Count;
            for (int intCnt = 0; intCnt < intExceptionCnt; intCnt++)
            {
                uniqueRowIdException = uniqueRowIdException + 1;
                drException = dtException.Rows[intCnt];

                if (drException["CE_TARGET_DATE"] != null && drException["CE_TARGET_DATE"].ToString() != "")
                {
                    strTargetDate = Convert.ToDateTime(drException["CE_TARGET_DATE"].ToString()).ToString("dd-MMM-yyyy");
                }
                strHtmlTableExceptionDetsRows = strHtmlTableExceptionDetsRows + "<tr>" +
                                                "<td " + strTableCellStyle + ">" + uniqueRowIdException + "</td>" +
                                                "<td " + strTableCellStyle + ">" + drException["CE_CLIENT_FILE_NAME"].ToString().Replace("\n", "<br />") + "</td>" +
                                                "<td " + strTableCellStyle + ">" + drException["CE_EXCEPTION_TYPE"].ToString().Replace("\n", "<br />") + "</td>" +
                                                "<td " + strTableCellStyle + ">" + drException["CE_DETAILS"].ToString().Replace("\n", "<br />") + "</td>" +
                                                "<td " + strTableCellStyle + ">" + drException["CE_ROOT_CAUSE_OF_DEVIATION"].ToString().Replace("\n", "<br />") + "</td>" +
                                                "<td " + strTableCellStyle + ">" + drException["CE_ACTION_TAKEN"].ToString().Replace("\n", "<br />") + "</td>" +
                                                "<td " + strTableCellStyle + ">" + drException["CE_CLOSURE_STATUS"].ToString().Replace("\n", "<br />") + "</td>" +
                                                "<td " + strTableCellStyle + ">" + strTargetDate + "</td>" +
                                                "</tr>";
            }
            //<<
            strHtmlException = strHtmlException + strHtmlTableExceptionDetsRows + "</table>" + "</body>" + "</html>";

            //<<
            HttpContext context = HttpContext.Current;

            string strContent = "";
            if (strHtmlTableExceptionDetsRows.Equals(""))
            {
                strContent = "<html><body>" + strHtmlDocument + "</body></html>";
            }
            else
            {
                strContent = "<html><body>" + strHtmlDocument + "<br/><br/>" + strHtmlException + "</body></html>";

            }
            context.Response.Clear();

            Response.Write(strContent);
            context.Response.ContentType = "application/ms-word";
            context.Response.AppendHeader("content-disposition", "attachment; filename=ComplianceCertificate_" + strUnitName + ".doc");
            context.Response.End();

        }
        //>>

    }

}