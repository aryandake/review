using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.Legacy_App_Code.Certification;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Cert_ClosureTracking : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        UtilitiesVO utilVO = new UtilitiesVO();
        //UtilitiesDAL utilDAL = new UtilitiesDAL();
        CertificationMasterBL certBL = new CertificationMasterBL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListItem li = new ListItem();
                li.Text = "(Select)";
                li.Value = "";

                ddlQuarter.DataSource = utilBL.getDataset("CERTIFICATEQUARTER", strConnectionString);
                ddlQuarter.DataBind();
                ddlQuarter.Items.Insert(0, li);

                hfCurrDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            SearchDetails();
        }

        private void SearchDetails()
        {
            DataTable dtRecords = new DataTable();
            DataTable dtExceptionRecords = new DataTable();
            try
            {
                //if (ddlQuarter.SelectedIndex > 0)
                //{
                pnlGrid.Visible = true;
                string strQTRId = "0";
                if (!string.IsNullOrEmpty(ddlQuarter.SelectedValue))
                {
                    strQTRId = ddlQuarter.SelectedValue;
                }

                if (User.IsInRole("Certification_Compliance_User"))
                {
                    dtRecords = certBL.CERT_getChklistForClosure(Convert.ToInt32(strQTRId), 0, 0, 0, "", strConnectionString);
                }
                else
                {
                    dtRecords = certBL.CERT_getChklistForClosure(Convert.ToInt32(strQTRId), 0, 0, 0, User.Identity.Name, strConnectionString);
                }
                gvChecklist.DataSource = dtRecords;
                gvChecklist.DataBind();

                if (User.IsInRole("Certification_Compliance_User"))
                {
                    dtExceptionRecords = certBL.CERT_getExceptionForClosure(Convert.ToInt32(strQTRId), 0, 0, 0, "", ddlStatus.SelectedValue, strConnectionString);
                }
                else
                {
                    dtExceptionRecords = certBL.CERT_getExceptionForClosure(Convert.ToInt32(strQTRId), 0, 0, 0, User.Identity.Name, ddlStatus.SelectedValue, strConnectionString);
                }
                gvException.DataSource = dtExceptionRecords;
                gvException.DataBind();

                Session["ClosureChecklist"] = dtRecords;
                Session["ClosureException"] = dtExceptionRecords;

                if (dtRecords.Rows.Count > 0)
                {
                    btnExportToExcel.Visible = true;
                }
                else
                {
                    btnExportToExcel.Visible = false;
                }

                if (dtExceptionRecords.Rows.Count > 0)
                {
                    btnExportToExcelExceptions.Visible = true;
                }
                else
                {
                    btnExportToExcelExceptions.Visible = false;
                }
                //}
                //else
                //{
                //    pnlGrid.Visible = false;
                //}
            }
            catch (Exception ex)
            {
                writeError("system exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
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
                writeError("System Exception in LoadChecklistFile():" + ex.Message);
            }
            return dtChecklistDets;
        }

        protected void gvChecklist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                Label lblClosureStatus = (Label)(e.Row.FindControl("lblClosureStatus"));
                Label lblsrno = (Label)(e.Row.FindControl("lblsrno"));
                LinkButton lbSave = (LinkButton)(e.Row.FindControl("lbSave"));

                //TextBox txtClosureDate = (TextBox)(e.Row.FindControl("txtClosureDate1"));
                //RegularExpressionValidator revtxtClosureDate = (RegularExpressionValidator)(e.Row.FindControl("revtxtClosureDate1"));
                //RequiredFieldValidator rfvtxtClosureDate = (RequiredFieldValidator)(e.Row.FindControl("rfvtxtClosureDate1"));
                //RequiredFieldValidator rfvtxtClosureRemarks = (RequiredFieldValidator)(e.Row.FindControl("rfvtxtClosureRemarks1"));
                //CustomValidator cvtxtClosureDate = (CustomValidator)(e.Row.FindControl("cvtxtClosureDate1"));


                //revtxtClosureDate.ValidationGroup = "C" + e.Row.RowIndex.ToString();
                //rfvtxtClosureDate.ValidationGroup = "C" + e.Row.RowIndex.ToString();
                //rfvtxtClosureRemarks.ValidationGroup = "C" + e.Row.RowIndex.ToString();
                //cvtxtClosureDate.ValidationGroup = "C" + e.Row.RowIndex.ToString();

                //lbSave.ValidationGroup = e.Row.RowIndex.ToString();

                int index = Convert.ToInt32(lblsrno.Text);

                lbSave.OnClientClick = "return validateChecklist('" + (index + 1) + "');";

                if (User.IsInRole("Certification_Compliance_User"))
                {
                    lbSave.Visible = false;
                }

            }
        }

        protected void gvChecklist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strCCDId = "", strClosureDate = "", strClosureRemarks = "";
            try
            {
                strCCDId = gvChecklist.SelectedValue.ToString();
                GridViewRow gvr = gvChecklist.SelectedRow;

                TextBox txtClosureDate = (TextBox)gvr.FindControl("txtClosureDate1");
                TextBox txtClosureRemarks = (TextBox)gvr.FindControl("txtClosureRemarks1");

                certBL.CERT_saveChklistClosure(Convert.ToInt32(strCCDId), txtClosureDate.Text, txtClosureRemarks.Text, User.Identity.Name, "CCD", strConnectionString);
                writeError("Record saved successfully...");
                SearchDetails();

            }
            catch (Exception ex)
            {
                writeError("system exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex);
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            int uniqueRowId = 0;
            string strChecklistTable = "", strHtmlTableChecklistDetsRows = "";// strSubmittedOn = "";
                                                                              // string strArmReleasedDate = "", strApprovalDateCompliance = "", strRecievedDate = "", strApprovalDate = "", strFilingDate = "", strDateOfReleased = "";
            DataTable dtChecklistDets;
            DataRow drChecklistDets;
            // DateTime dtDateOfReleased, dtFilingDate, dtApprovalDate, dtRecievdDate, dtArmReleasedDate, dtApprovalDateCompliance;
            dtChecklistDets = (DataTable)Session["ClosureChecklist"];
            string strHtmlTable =
                "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                "<HTML>" +
                //<<Modified By Rahuldeb on 21Mar2020 for Special character issue in Export to excel
                "<HEAD><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />" +
                //>>
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
                          " Reasons for Compliance / Non-Compliance / WIP / Not Applicable" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Checklist File" +
                          " </th> " +

                          " <th class='tabhead' align='center'> " +
                          " Closure Date" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Closure Remarks" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Closed By" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Closed On" +
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

                "<td>" + (drChecklistDets["CCD_CLOSURE_DATE"] == DBNull.Value ? "" : Convert.ToDateTime(drChecklistDets["CCD_CLOSURE_DATE"]).ToString("dd-MMM-yyyy")) + "</td>" +
                "<td>" + drChecklistDets["CCD_CLOSURE_REMARKS"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCD_CLOSURE_BY"].ToString() + "</td>" +
                "<td>" + (drChecklistDets["CCD_CLOSURE_ON"] == DBNull.Value ? "" : Convert.ToDateTime(drChecklistDets["CCD_CLOSURE_ON"]).ToString("dd-MMM-yyyy HH:mm:ss")) + "</td>" +

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

        protected void btnExportToExcelExceptions_Click(object sender, EventArgs e)
        {
            try
            {
                gvException.AllowPaging = false;
                gvException.DataSource = Session["ClosureException"];
                gvException.DataBind();

                //gvException.AllowSorting = false;
                //gvException.Columns[10].Visible = false;
                //gvException.Columns[9].Visible = false;
                //gvException.Columns[7].Visible = true;
                //gvException.Columns[8].Visible = true;

                //PrepareGridViewForExport(gvException);
                //string attachment = "attachment; filename=Checklist Exception Details.xls";
                //Response.ClearContent();
                //Response.AddHeader("content-disposition", attachment);
                //Response.ContentType = "application/ms-excel";
                //StringWriter sw = new StringWriter();

                //HtmlTextWriter htw = new HtmlTextWriter(sw);

                //gvException.RenderControl(htw);

                //string html2 = Regex.Replace(sw.ToString(), @"(<input type=""image""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
                ////html2 = Regex.Replace(html2, @"(<input class=""checkbox""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
                //html2 = Regex.Replace(html2, @"(<a \/?[^>]+>)", @"", RegexOptions.IgnoreCase);
                //html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
                //html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
                //Response.Write(html2.ToString());

                ////Response.Write(sw.ToString());
                //Response.End();


                int uniqueRowId = 0;
                string strChecklistTable = "", strHtmlTableChecklistDetsRows = "";// strSubmittedOn = "";
                                                                                  // string strArmReleasedDate = "", strApprovalDateCompliance = "", strRecievedDate = "", strApprovalDate = "", strFilingDate = "", strDateOfReleased = "";
                DataTable dtChecklistDets;
                DataRow drChecklistDets;
                // DateTime dtDateOfReleased, dtFilingDate, dtApprovalDate, dtRecievdDate, dtArmReleasedDate, dtApprovalDateCompliance;
                dtChecklistDets = (DataTable)Session["ClosureException"];
                string strHtmlTable =
                "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                "<HTML>" +
                //<<Modified By Rahuldeb on 21Mar2020 for Special character issue in Export to excel
                "<HEAD><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />" +
                //>>
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
                          " Department Name" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Deviation (Detailed)" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Regulatory Reference (Detailed)" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Root Cause for the Deviation" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Action taken" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Target Date" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Closure Date" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Closure Remarks" +
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
                    "<td>" + drChecklistDets["CE_EXCEPTION_TYPE"].ToString() + "</td>" +
                    "<td>" + drChecklistDets["CE_DETAILS"].ToString() + "</td>" +
                    "<td>" + drChecklistDets["CE_ROOT_CAUSE_OF_DEVIATION"].ToString() + "</td>" +
                    "<td>" + drChecklistDets["CE_ACTION_TAKEN"].ToString() + "</td>" +
                    "<td>" + (drChecklistDets["CE_TARGET_DATE"] == DBNull.Value ? "" : Convert.ToDateTime(drChecklistDets["CE_TARGET_DATE"]).ToString("dd-MMM-yyyy")) + "</td>" +
                    "<td>" + (drChecklistDets["CE_CLOSURE_DATE"] == DBNull.Value ? "" : Convert.ToDateTime(drChecklistDets["CE_CLOSURE_DATE"]).ToString("dd-MMM-yyyy")) + "</td>" +
                    "<td>" + drChecklistDets["CE_CLOSURE_REMARKS"].ToString() + "</td>" +
                    "</tr>";
                }
                strChecklistTable = strHtmlTable + strHtmlTableChecklistDetsRows + "</table> " +
                "</BODY>" +
                "</HTML>";
                string attachment = "attachment; filename=Checklist Exception Details.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                Response.Write(strChecklistTable.ToString());
                Response.End();


            }
            catch (Exception exp)
            {
                writeError("Exception in btnExportToExcel_Click :" + exp);
            }
        }

        private void PrepareGridViewForExport(Control gv)
        {
            LinkButton lb = new LinkButton();
            Literal l = new Literal();
            string name = String.Empty;

            for (int i = 0; i < gv.Controls.Count; i++)
            {
                if (gv.Controls[i].GetType() == typeof(LinkButton))
                {
                    l.Text = (gv.Controls[i] as LinkButton).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                }
                else if (gv.Controls[i].GetType() == typeof(RadioButtonList))
                {
                    l.Text = (gv.Controls[i] as TextBox).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    gv.Controls.AddAt(i, l);
                }

                //else
                //{
                //    gv.Controls.Remove(gv.Controls[i]);
                //}

                if (gv.Controls[i].HasControls())
                {
                    PrepareGridViewForExport(gv.Controls[i]);
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void gvException_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                Label lblClosureStatus = (Label)(e.Row.FindControl("lblClosureStatus2"));
                LinkButton lbSave = (LinkButton)(e.Row.FindControl("lbSave2"));
                Label lblsrno = (Label)(e.Row.FindControl("lblsrno"));

                //TextBox txtClosureDate = (TextBox)(e.Row.FindControl("txtClosureDate2"));
                //RegularExpressionValidator revtxtClosureDate = (RegularExpressionValidator)(e.Row.FindControl("revtxtClosureDate2"));
                //RequiredFieldValidator rfvtxtClosureDate = (RequiredFieldValidator)(e.Row.FindControl("rfvtxtClosureDate2"));
                //RequiredFieldValidator rfvtxtClosureRemarks = (RequiredFieldValidator)(e.Row.FindControl("rfvtxtClosureRemarks2"));
                //CustomValidator cvtxtClosureDate = (CustomValidator)(e.Row.FindControl("cvtxtClosureDate2"));

                //revtxtClosureDate.ValidationGroup = "D" + e.Row.RowIndex.ToString();
                //rfvtxtClosureDate.ValidationGroup = "D" + e.Row.RowIndex.ToString();
                //rfvtxtClosureRemarks.ValidationGroup = "D" + e.Row.RowIndex.ToString();
                //cvtxtClosureDate.ValidationGroup = "D" + e.Row.RowIndex.ToString();

                //lbSave.ValidationGroup = e.Row.RowIndex.ToString();
                //lbSave.OnClientClick = "return OnClientSaveClick('D" + e.Row.RowIndex.ToString() + "');";


                int index = Convert.ToInt32(lblsrno.Text);

                lbSave.OnClientClick = "return validateChecklistException('" + (index + 1) + "');";

                if (lblClosureStatus.Text.Equals("Closed") || User.IsInRole("Certification_Compliance_User"))
                    lbSave.Visible = false;
                else
                    lbSave.Visible = true;
            }
        }

        protected void gvException_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strCEId = "", strClosureDate = "", strClosureRemarks = "";
            try
            {
                strCEId = gvException.SelectedValue.ToString();
                GridViewRow gvr = gvException.SelectedRow;

                TextBox txtClosureDate = (TextBox)gvr.FindControl("txtClosureDate2");
                TextBox txtClosureRemarks = (TextBox)gvr.FindControl("txtClosureRemarks2");

                if (string.IsNullOrEmpty(txtClosureDate.Text) && string.IsNullOrEmpty(txtClosureRemarks.Text))
                {
                    writeError("Please select closure date and closure remarks.");
                    return;
                }

                certBL.CERT_saveChklistClosure(Convert.ToInt32(strCEId), txtClosureDate.Text, txtClosureRemarks.Text, User.Identity.Name, "CE", strConnectionString);
                writeError("Record saved successfully...");
                SearchDetails();

            }
            catch (Exception ex)
            {
                writeError("system exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex);
            }
        }
    }
}