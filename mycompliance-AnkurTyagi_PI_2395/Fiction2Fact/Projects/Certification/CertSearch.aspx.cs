using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code.DAL;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.Legacy_App_Code.Certification;
using SelectPdf;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_CertSearch : System.Web.UI.Page
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
                //<<Commented by Denil Shah as departments are now populated based on level.
                //ddlSearchDeptName.DataSource = utilBL.getDatasetWithConditionInString("getCertDeptById", "", strConnectionString);
                //ddlSearchDeptName.DataBind();
                //ddlSearchDeptName.Items.Insert(0, li);
                //>>
                //Changes By Milan Yadav on 08-Jul-2016 to get all Quarter 
                //>>
                ddlQuarter.DataSource = utilBL.getDataset("CERTIFICATEQUARTER", strConnectionString);
                //<<
                //ddlQuarter.DataSource = utilBL.getDataset("CERTQUARTERS", strConnectionString);
                ddlQuarter.DataBind();
                ddlQuarter.Items.Insert(0, li);

                mvMultiView.ActiveViewIndex = 0;
            }
            else
            {
            }
        }
        protected void btnViewCancel_Click(object sender, System.EventArgs e)
        {
            mvMultiView.ActiveViewIndex = 0;
            lblMsg.Text = "";
        }
        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            ShowCertifications();
        }
        private void ShowCertifications()
        {
            try
            {
                int intCertId = 0, intLevel;
                //<<Changed by Denil Shah on 12-Apr-2017
                //string strDeptName = ddlSearchDeptName.SelectedValue;
                string strDeptName = ddlDeptName.SelectedValue;
                //>>
                string strQuarter = ddlQuarter.SelectedValue;
                DataSet dsViewCert = new DataSet();
                //<<Changed by Denil Shah on 12-Apr-2017 to introduce search based on the level.
                intLevel = Convert.ToInt32(ddlLevelSearch.SelectedValue);
                //>>
                hfLevel.Value = intLevel.ToString();

                //dsViewCert = certBL.searchEditCertifications(intCertId, strDeptName, strQuarter, "", intLevel, strConnectionString);
                dsViewCert = certBL.searchEditCertifications(intCertId, strDeptName, strQuarter, "", intLevel, strConnectionString);
                gvSearchCert.DataSource = dsViewCert;
                gvSearchCert.DataBind();

                //<<Commented by Denil Shah on 12-Apr-2017 as it doesn't serve any purpose.
                //<< get Departments not certified.
                //DataTable dtDept = utilBL.getDataset("getDeptNotCertified", strConnectionString).Tables[0];
                //DataRow drDept;
                //int intRowsCnt = dtDept.Rows.Count;
                //string strDeptNotCert = "";
                //for (int cnt = 0; cnt < intRowsCnt; cnt++)
                //{
                //    drDept = dtDept.Rows[cnt];
                //    strDeptNotCert = strDeptNotCert + drDept["CSSDM_NAME"].ToString() + ", ";
                //}
                //if (!strDeptNotCert.Equals(""))
                //    strDeptNotCert = strDeptNotCert.Remove(strDeptNotCert.LastIndexOf(','));
                ////>>

                //if (!strDeptNotCert.Equals(""))
                //{
                //    lblInfo.Text = "Following department(s) are yet to submit their certification: "
                //        + strDeptNotCert + ".";
                //    return;
                //}
                //>>
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

        protected void gvSearchCert_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            lblMsg.Text = "";
            // DataRow dr;
            string strCertId, strDeptId, strQuarterId, strDeptName = "";
            DataSet dsView = new DataSet();
            DataTable dtChecklist = new DataTable();
            GridViewRow gvr;
            string strLevel;
            try
            {
                if (hfSelectedOperation.Value == "View")
                {
                    strCertId = gvSearchCert.SelectedValue.ToString();
                    gvr = gvSearchCert.SelectedRow;
                    strLevel = ((HiddenField)(gvr.FindControl("hfLevel"))).Value;
                    strDeptId = ((HiddenField)(gvr.FindControl("hfDeptId"))).Value;
                    strQuarterId = ((HiddenField)(gvr.FindControl("hfQuarterId"))).Value;
                    strDeptName = ((Label)(gvr.FindControl("lblId"))).Text;

                    dsView = certBL.searchEditCertifications(Convert.ToInt32(strCertId), "", "", "", Convert.ToInt32(strLevel), strConnectionString);
                    mvMultiView.ActiveViewIndex = 1;
                    fvEditCert.ChangeMode(FormViewMode.ReadOnly);
                    Session["CertificationDetails"] = dsView;
                    DataTable dtCntDr = dsView.Tables[0];
                    DataRow drCntDr = dtCntDr.Rows[0];
                    string strcertId = drCntDr["CERT_ID"].ToString();
                    hfContent.Value = drCntDr["CERT_CONTENT"].ToString();

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

                    //<<Commented by Denil Shah on 12-Apr-2017 as con
                    // dtChecklist = utilBL.getDatasetWithCondition("CERT_getChecklistByCertId", Convert.ToInt32(strCertId), strConnectionString);
                    //Added By milan yadav on 01-June-2016
                    //>>
                    //dtChecklist = certBL.getChecklistByCertId(Convert.ToInt32(strCertId), strConnectionString);

                    dtChecklist = certBL.viewPastConsolidateChecklist(strCertId, strDeptId, strQuarterId, strLevel, "", strConnectionString);
                    //<<//<<

                    gvChecklist.DataSource = dtChecklist;
                    gvChecklist.DataBind();
                    Session["CertChecklist1"] = dtChecklist;

                    RegulatoryReportingDashboard rrd = new RegulatoryReportingDashboard();
                    litRegulatoryFilling.Text = rrd.GetFilingsDashboardSingleRow_QID(strDeptId, strDeptName, strLevel, strQuarterId);


                    //DataTable dtAllChecklist = utilBL.getDatasetWithCondition("getAllChecklistByCertId", Convert.ToInt32(strCertId), strConnectionString);
                    //Added By milan yadav on 01-June-2016
                    //>>
                    //DataTable dtAllChecklist = certBL.getAllChecklistByCertificationId(Convert.ToInt32(strCertId), strConnectionString);
                    ////<<
                    //gvAllChecklist.DataSource = dtAllChecklist;
                    //gvAllChecklist.DataBind();
                    //Session["CertAllChecklist"] = dtAllChecklist;

                    //DataTable dtAllExceptions = utilBL.getDatasetWithCondition("getAllCertExceptions", Convert.ToInt32(strCertId), strConnectionString);
                    //gvAllExceptions.DataSource = dtAllExceptions;
                    //gvAllExceptions.DataBind();
                    //Session["CertAllExceptions"] = dtAllExceptions;

                }
                else if (hfSelectedOperation.Value == "Delete")
                {
                    int intCertId;
                    intCertId = Convert.ToInt32(gvSearchCert.SelectedDataKey.Value);
                    certBL.deleteCertContent(intCertId, strConnectionString);
                    ShowCertifications();
                    writeError("Record has been successfully deleted.");
                }
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                this.lblInfo.Text = ex.Message;
                this.lblInfo.Visible = true;
            }
        }

        //protected void btnExport_Click(object sender, EventArgs e)
        //{
        //    HttpContext context = HttpContext.Current;
        //    string strContent = "<html><body style=\" font-family: Trebuchet MS;\">" + hfContent.Value + "</body></html>";
        //    context.Response.Clear();
        //    Response.Write(strContent);
        //    context.Response.ContentType = "application/ms-word";
        //    context.Response.AppendHeader("content-disposition", "attachment; filename=Certificate.doc");
        //    context.Response.End();
        //}
        //>>
        //protected void btnAllExportToExcel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        gvAllChecklist.AllowPaging = false;
        //        gvAllChecklist.AllowSorting = false;

        //        gvAllChecklist.DataSource = Session["CertAllChecklist"];
        //        gvAllChecklist.DataBind();
        //        PrepareGridViewForExport(gvAllChecklist);
        //        string attachment = "attachment; filename=All Checklist Details.xls";
        //        Response.ClearContent();
        //        Response.AddHeader("content-disposition", attachment);
        //        Response.ContentType = "application/ms-excel";
        //        StringWriter sw = new StringWriter();

        //        HtmlTextWriter htw = new HtmlTextWriter(sw);

        //        gvAllChecklist.RenderControl(htw);

        //        string html2 = Regex.Replace(sw.ToString(), @"(<input type=""image""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
        //        //html2 = Regex.Replace(html2, @"(<input class=""checkbox""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
        //        html2 = Regex.Replace(html2, @"(<a \/?[^>]+>)", @"", RegexOptions.IgnoreCase);
        //        html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
        //        html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
        //        Response.Write(html2.ToString());

        //        //Response.Write(sw.ToString());
        //        Response.End();
        //        gvAllChecklist.AllowPaging = true;
        //        gvAllChecklist.AllowSorting = true;
        //        gvAllChecklist.DataBind();
        //    }
        //    catch (Exception exp)
        //    {
        //        writeError("Exception in btnExportToExcel_Click :" + exp);
        //    }
        //}


        //Added by Milan Yadav on 04-Mar-2016
        //<<
        //protected void btnAllExportToExcel_Click(object sender, EventArgs e)
        //{
        //    int uniqueRowId = 0;
        //    string strChecklistTable = "", strHtmlTableChecklistDetsRows = "";//strSubmittedOn = ""
        //                                                                      //  string strArmReleasedDate = "", strApprovalDateCompliance = "", strRecievedDate = "", strApprovalDate = "", strFilingDate = "", strDateOfReleased = "";
        //    DataTable dtChecklistDets;
        //    DataRow drChecklistDets;
        //  //  DateTime dtDateOfReleased, dtFilingDate, dtApprovalDate, dtRecievdDate, dtArmReleasedDate, dtApprovalDateCompliance;
        //    dtChecklistDets = (DataTable)Session["CertAllChecklist"];
        //    string strHtmlTable =
        //        "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
        //        "<HTML>" +
        //        "<HEAD>" +
        //        "</HEAD>" +
        //        "<BODY>" +

        //        " <table id='tblChecklistDets' width='100%' align='left' style='margin-left:20px;' " +
        //                    " cellpadding='0' cellspacing='1' border='1'> " +
        //                  " <thead> " +
        //                  " <tr> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Serial Number " +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Title of Section/Requirement" +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Section/Regulation Rule/Circulars " +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Time Limit" +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Compliance Status" +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Remarks" +
        //                  " </th> " +
        //                   " <th class='tabhead' align='center'> " +
        //                  " Checklist File" +
        //                  " </th> " +
        //                  " </tr> " +
        //                  " </thead> ";
        //    int intChecklistDetsCnt = dtChecklistDets.Rows.Count;
        //    for (int intCnt = 0; intCnt < intChecklistDetsCnt; intCnt++)
        //    {
        //        uniqueRowId = uniqueRowId + 1;
        //        drChecklistDets = dtChecklistDets.Rows[intCnt];
        //       // strSubmittedOn = "";

        //        strHtmlTableChecklistDetsRows = strHtmlTableChecklistDetsRows + "<tr>" +
        //        "<td>" + uniqueRowId + "</td>" +
        //        "<td>" + drChecklistDets["CCM_CLAUSE"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["CCM_REFERENCE"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["CCM_FREQUENCY"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["Compliance_Status"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["CCD_REMARKS"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["CCD_CLIENT_FILENAME"].ToString() + "</td>" +
        //        "</tr>";
        //    }
        //    strChecklistTable = strHtmlTable + strHtmlTableChecklistDetsRows + "</table> " +
        //    "</BODY>" +
        //    "</HTML>";
        //    string attachment = "attachment; filename=All Checklist Details.xls";
        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", attachment);
        //    Response.ContentType = "application/ms-excel";
        //    Response.Write(strChecklistTable.ToString());
        //    Response.End();


        //}
        //>>



        //Added by Milan Yadav on 14-Oct-2016
        //<<

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            int uniqueRowId = 0;
            string strChecklistTable = "", strHtmlTableChecklistDetsRows = "";// strSubmittedOn = "";
                                                                              // string strArmReleasedDate = "", strApprovalDateCompliance = "", strRecievedDate = "", strApprovalDate = "", strFilingDate = "", strDateOfReleased = "";
            DataTable dtChecklistDets;
            DataRow drChecklistDets;
            // DateTime dtDateOfReleased, dtFilingDate, dtApprovalDate, dtRecievdDate, dtArmReleasedDate, dtApprovalDateCompliance;
            dtChecklistDets = (DataTable)Session["CertChecklist1"];
            string strHtmlTable =
                "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                "<HTML>" +
                "<HEAD>" +
                "</HEAD>" +
                "<BODY>" +

                " <table id='tblChecklistDets' width='100%' align='left' class='table table-bordered footable' " +
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
                //strSubmittedOn = "";

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

        //protected void btnExportToExcelAllExceptions_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        gvAllExceptions.AllowPaging = false;
        //        gvAllExceptions.AllowSorting = false;

        //        gvAllExceptions.DataSource = Session["CertAllExceptions"];
        //        gvAllExceptions.DataBind();
        //        PrepareGridViewForExport(gvAllExceptions);
        //        string attachment = "attachment; filename=All Exceptions Details.xls";
        //        Response.ClearContent();
        //        Response.AddHeader("content-disposition", attachment);
        //        Response.ContentType = "application/ms-excel";
        //        StringWriter sw = new StringWriter();

        //        HtmlTextWriter htw = new HtmlTextWriter(sw);

        //        gvAllExceptions.RenderControl(htw);

        //        string html2 = Regex.Replace(sw.ToString(), @"(<input type=""image""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
        //        //html2 = Regex.Replace(html2, @"(<input class=""checkbox""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
        //        html2 = Regex.Replace(html2, @"(<a \/?[^>]+>)", @"", RegexOptions.IgnoreCase);
        //        html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
        //        html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
        //        Response.Write(html2.ToString());

        //        //Response.Write(sw.ToString());
        //        Response.End();
        //        gvAllExceptions.AllowPaging = true;
        //        gvAllExceptions.AllowSorting = true;
        //        gvAllExceptions.DataBind();
        //    }
        //    catch (Exception exp)
        //    {
        //        writeError("Exception in btnExportToExcel_Click :" + exp);
        //    }
        //}

        //Added by Milan Yadav on 04-Mar-2016
        //<<
        //protected void btnExportToExcelAllExceptions_Click(object sender, EventArgs e)
        // {
        //     int uniqueRowId = 0;
        //     string strChecklistTable = "", strHtmlTableChecklistDetsRows = "", strSubmittedOn = "";
        //     string strArmReleasedDate = "", strApprovalDateCompliance = "", strRecievedDate = "", strApprovalDate = "", strFilingDate = "", strDateOfReleased = "";
        //     DataTable dtChecklistDets;
        //     DataRow drChecklistDets;
        //     DateTime dtDateOfReleased, dtFilingDate, dtApprovalDate, dtRecievdDate, dtArmReleasedDate, dtApprovalDateCompliance;
        //     dtChecklistDets = (DataTable)Session["CertAllExceptions"];
        //     string strHtmlTable =
        //         "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
        //         "<HTML>" +
        //         "<HEAD>" +
        //         "</HEAD>" +
        //         "<BODY>" +

        //         " <table id='tblChecklistDets' width='100%' align='left' style='margin-left:20px;' " +
        //                     " cellpadding='0' cellspacing='1' border='1'> " +
        //                   " <thead> " +
        //                   " <tr> " +
        //                   " <th class='tabhead' align='center'> " +
        //                   " Serial Number " +
        //                   " </th> " +
        //                   " <th class='tabhead' align='center'> " +
        //                   " Title of Section/Requirement" +
        //                   " </th> " +
        //                   " <th class='tabhead' align='center'> " +
        //                   " Section/Regulation Rule/Circulars " +
        //                   " </th> " +
        //                   " <th class='tabhead' align='center'> " +
        //                   " Time Limit" +
        //                   " </th> " +
        //                   " <th class='tabhead' align='center'> " +
        //                   " Compliance Status" +
        //                   " </th> " +
        //                   " <th class='tabhead' align='center'> " +
        //                   " Remarks" +
        //                   " </th> " +
        //                    " <th class='tabhead' align='center'> " +
        //                   " Checklist File" +
        //                   " </th> " +
        //                   " </tr> " +
        //                   " </thead> ";


        //     int intChecklistDetsCnt = dtChecklistDets.Rows.Count;
        //     for (int intCnt = 0; intCnt < intChecklistDetsCnt; intCnt++)
        //     {
        //         uniqueRowId = uniqueRowId + 1;
        //         drChecklistDets = dtChecklistDets.Rows[intCnt];
        //         strSubmittedOn = "";

        //         strHtmlTableChecklistDetsRows = strHtmlTableChecklistDetsRows + "<tr>" +
        //         "<td>" + uniqueRowId + "</td>" +
        //         "<td>" + drChecklistDets["CCM_CLAUSE"].ToString() + "</td>" +
        //         "<td>" + drChecklistDets["CCM_REFERENCE"].ToString() + "</td>" +
        //         "<td>" + drChecklistDets["CCM_FREQUENCY"].ToString() + "</td>" +
        //         "<td>" + drChecklistDets["Compliance_Status"].ToString() + "</td>" +
        //         "<td>" + drChecklistDets["CCD_REMARKS"].ToString() + "</td>" +
        //         "<td>" + drChecklistDets["CCD_CLIENT_FILENAME"].ToString() + "</td>" +
        //         "</tr>";
        //     }
        //     strChecklistTable = strHtmlTable + strHtmlTableChecklistDetsRows + "</table> " +
        //     "</BODY>" +
        //     "</HTML>";
        //     string attachment = "attachment; filename=All Exceptions Details.xls";
        //     Response.ClearContent();
        //     Response.AddHeader("content-disposition", attachment);
        //     Response.ContentType = "application/ms-excel";
        //     Response.Write(strChecklistTable.ToString());
        //     Response.End();
        // }
        //>>

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

        protected void btnExportToDoc_Click(object sender, EventArgs e)
        {
            //>>
            string strHostingServer = (ConfigurationManager.AppSettings["HostingServer"].ToString());

            int uniqueRowId = 0;
            int uniqueRowIdException = 0;
            string strHtmlTableChecklistDetsRows = "";
            string strHtmlDocument = "";
            string strHtmlException = "";
            string strTargetDate = "";
            string strccdTargetDate = "";
            string strFontName = ConfigurationManager.AppSettings["FontName"].ToString();
            DataTable dtChecklistDeatils = new DataTable();
            dtChecklistDeatils = (DataTable)Session["CertChecklist1"];
            DataRow drChecklistDets;
            DataTable dtChecklistDeatils1 = new DataTable();
            DataSet ds = new DataSet();
            DataTable dtException = new DataTable();
            DataSet dsException = new DataSet();
            DataRow drChecklistDets1;
            DataRow drException;
            string strHtmlCertificationDets = "", strHtmlTableExceptionDetsRows = "", strSpocSubmittedOn = "", strHODSubmittedOn = "",
                strCUSubmittedOn = "", strCXOSubmittedOn = "", strUnitName = "", strExCoSubmittedOn="";

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
                            "<center><table class='table table-bordered footable'  width='100%'>" +
                            "<tr><td " +
                            strHeaderStyle + ">" + strHeaderTitle + "</td></tr>" +
                            "</table><br/></center>";


            int intChecklistDetsCnt1 = dtChecklistDeatils1.Rows.Count;
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
                    strCUSubmittedOn = Convert.ToDateTime(drChecklistDets1["CERT_APPROVED_DT_LEVEL2"].ToString()).ToString("dd-MMM-yyyy hh:mm:ss tt");
                }
                if (drChecklistDets1["CERT_APPROVED_DT_LEVEL3"] != null && drChecklistDets1["CERT_APPROVED_DT_LEVEL3"].ToString() != "")
                {
                    strCXOSubmittedOn = Convert.ToDateTime(drChecklistDets1["CERT_APPROVED_DT_LEVEL3"].ToString()).ToString("dd-MMM-yyyy hh:mm:ss tt");
                }
                if (drChecklistDets1["CERT_APPROVED_DT_LEVEL4"] != null && drChecklistDets1["CERT_APPROVED_DT_LEVEL4"].ToString() != "")
                {
                    strExCoSubmittedOn = Convert.ToDateTime(drChecklistDets1["CERT_APPROVED_DT_LEVEL4"].ToString()).ToString("dd-MMM-yyyy hh:mm:ss tt");
                }
                strUnitName = drChecklistDets1["DeptName"].ToString();
                strHtmlCertificationDets =

                    " <table id='tblCertificationDets' table border='0'  class='table table-bordered footable' " +
                    "cellpadding='2' cellspacing='2' width='100%'> " +
                    " <tr style=\"background-color: #ffffff;\"> " +
                    " <td width='30%' " + strTabHeadStyle + "> " +
                    " Function Name:" +
                    " </td> " +
                    " <td width='70%' " + strTabBodyStyle + "> " +
                    " " + drChecklistDets1["DeptName"] + " " +
                    " </td></tr>";
                if (hfLevel.Value == "0")
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
                else if (hfLevel.Value == "1")
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
                else if (hfLevel.Value == "2")
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
                else if (hfLevel.Value == "3")
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
                //<<Modified by Ankur Tyagi on 02Feb2024 for CR_1945
                " <th " + strTableHeaderStyle + "> " +
                " Target Date" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Action Plan" +
                " </th> " +
                //>>
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
                                                "<td " + strTableCellStyle + ">" + uniqueRowId + "</td>" +
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

          // strHtmlDocument = strHtmlDocument + strHtmlTableChecklistDetsRows + strHtmlTableExceptionDetsRows+"</table>" + "</body>" +
          "</html>";
            //>>Exception
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
                " Target/Closure Date" +
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
                    strTargetDate = Convert.ToDateTime(drException["CE_TARGET_DATE"].ToString()).ToString("dd-MMM-yyyy hh:mm:ss tt");
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

        protected void btnConvertToPdf_Click(object sender, EventArgs e)
        {
            string strHostingServer = (ConfigurationManager.AppSettings["HostingServer"].ToString());

            int uniqueRowId = 0;
            int uniqueRowIdException = 0;
            string strHtmlTableChecklistDetsRows = "";
            string strHtmlDocument = "";
            string strHtmlException = "";
            string strTargetDate = "";
            string strccdTargetDate = "";
            string strFontName = ConfigurationManager.AppSettings["FontName"].ToString();
            DataTable dtChecklistDeatils = new DataTable();
            dtChecklistDeatils = (DataTable)Session["CertChecklist1"];
            DataRow drChecklistDets;
            DataTable dtChecklistDeatils1 = new DataTable();
            DataSet ds = new DataSet();
            DataTable dtException = new DataTable();
            DataSet dsException = new DataSet();
            DataRow drChecklistDets1;
            DataRow drException;
            string strHtmlCertificationDets = "", strHtmlTableExceptionDetsRows = "", strSpocSubmittedOn = "", strHODSubmittedOn = "",
                strCUSubmittedOn = "", strCXOSubmittedOn = "", strUnitName = "", strExCoSubmittedOn = "";

            string strQtrFromDate = "", strQtrToDate = "";

            strQtrFromDate = ((Label)fvEditCert.FindControl("lblQtrFromDT")).Text;
            strQtrToDate = ((Label)fvEditCert.FindControl("lblQtrToDT")).Text;

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

            string strHeaderTitle = "Compliance Certificate for the Quarter " + strQtrFromDate + " to " + strQtrToDate;//ddlQuarter.SelectedItem.Text;

            strHtmlDocument = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                            "<html>" +
                            "<head>" +
                            "</head>" +
                            "<body>" +
                            //"<center><img src='" + "file:\\\\\\" + Server.MapPath("../../Content/images/logos/Company_Logo.png") + "' height='60px'/></center><br/><br/><center><table  width='100%'>" +
                            "<tr><td " +
                            strHeaderStyle + ">" + strHeaderTitle + "</td></tr>" +
                            "</table><br/></center>";


            int intChecklistDetsCnt1 = dtChecklistDeatils1.Rows.Count;
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
                    strCUSubmittedOn = Convert.ToDateTime(drChecklistDets1["CERT_APPROVED_DT_LEVEL2"].ToString()).ToString("dd-MMM-yyyy hh:mm:ss tt");
                }
                if (drChecklistDets1["CERT_APPROVED_DT_LEVEL3"] != null && drChecklistDets1["CERT_APPROVED_DT_LEVEL3"].ToString() != "")
                {
                    strCXOSubmittedOn = Convert.ToDateTime(drChecklistDets1["CERT_APPROVED_DT_LEVEL3"].ToString()).ToString("dd-MMM-yyyy hh:mm:ss tt");
                }
                strUnitName = drChecklistDets1["DeptName"].ToString();
                strHtmlCertificationDets =

                    " <table id='tblCertificationDets' table border='0'  class='table table-bordered footable' " +
                    "cellpadding='2' cellspacing='2' width='100%'> " +
                    " <tr style=\"background-color: #ffffff;\"> " +
                    " <td width='30%' " + strTabHeadStyle + "> " +
                    " Function Name:" +
                    " </td> " +
                    " <td width='70%' " + strTabBodyStyle + "> " +
                    " " + drChecklistDets1["DeptName"] + " " +
                    " </td></tr>";


                if (hfLevel.Value == "0")
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
                else if (hfLevel.Value == "1")
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
                else if (hfLevel.Value == "2")
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
                else if (hfLevel.Value == "3")
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
                //<<Modified by Ankur Tyagi on 02Feb2024 for CR_1945
                " <th " + strTableHeaderStyle + "> " +
                " Target Date" +
                " </th> " +
                " <th " + strTableHeaderStyle + "> " +
                " Action Plan" +
                " </th> " +
                //>>
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
                                                "<td " + strTableCellStyle + ">" + uniqueRowId + "</td>" +
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

          // strHtmlDocument = strHtmlDocument + strHtmlTableChecklistDetsRows + strHtmlTableExceptionDetsRows+"</table>" + "</body>" +
          "</html>";
            //>>Exception
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
                " Target/Closure Date" +
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
            SelectPdf.HtmlToPdf converter = new HtmlToPdf();

            //SelectPdf.GlobalProperties.LicenseKey = ConfigurationManager.AppSettings["PdfLicenseKey"].ToString();
            converter.Options.PdfPageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), "A4", true);
            converter.Options.PdfPageOrientation = (PdfPageOrientation)Enum.Parse(
                                                    typeof(PdfPageOrientation), "Portrait", true);
            converter.Options.WebPageWidth = 1024;
            converter.Options.WebPageHeight = 0;
            string baseUrl = strHostingServer;
            //Margin is in points. 1 point is 1/72 of an inch. So for a margin of 1 inch, the point should be 72.
            converter.Options.MarginTop = 10;
            converter.Options.MarginBottom = 10;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            //
            //Footer
            //>>
            string headerUrl = Server.MapPath("~/Pdf_Footer/header.html");
            string footerUrl = Server.MapPath("~/Pdf_Footer/footer.html");
            // instantiate a html to pdf converter object
            // header settings
            converter.Options.DisplayHeader = true;
            converter.Header.DisplayOnFirstPage = true;
            converter.Header.DisplayOnOddPages = true;
            converter.Header.DisplayOnEvenPages = true;
            converter.Header.Height = 50;

            PdfHtmlSection headerHtml = new PdfHtmlSection(headerUrl);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Header.Add(headerHtml);

            // footer settings
            converter.Options.DisplayFooter = true;
            converter.Footer.DisplayOnFirstPage = true;
            converter.Footer.DisplayOnOddPages = true;
            converter.Footer.DisplayOnEvenPages = true;
            converter.Footer.Height = 50;

            PdfHtmlSection footerHtml = new PdfHtmlSection(footerUrl);
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Footer.Add(footerHtml);

            // page numbers can be added using a PdfTextSection object
            PdfTextSection text = new PdfTextSection(0, 10,
                "Page: {page_number} of {total_pages}  ",
                //new System.Drawing.Font(strFontName, 8))
                new System.Drawing.Font(strFontName, 5));
            text.HorizontalAlign = PdfTextHorizontalAlign.Right;
            converter.Footer.Add(text);
            PdfDocument doc = converter.ConvertHtmlString(strContent, baseUrl);
            doc.Save(Response, false, "ApprovalDetails.pdf");
            doc.Close();
        }

    }
}