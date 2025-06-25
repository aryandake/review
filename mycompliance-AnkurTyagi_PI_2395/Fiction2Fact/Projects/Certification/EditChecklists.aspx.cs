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
using Fiction2Fact.F2FControls;

namespace Fiction2Fact.Projects.Certification
{
    //[Idunno.AntiCsrf.SuppressCsrfCheck]
    public partial class Certification_EditChecklists : System.Web.UI.Page
    {

        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ToString();
        int intCertId;
        DataTable dtException;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        UtilitiesVO utilVO = new UtilitiesVO();
        UtilitiesBLL utilBLL = new UtilitiesBLL();
        CertificationMasterBL certBL = new CertificationMasterBL();
        CommonMethods cm = new CommonMethods();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PreviousPage"] = Request.UrlReferrer;
                hfCurrentDate.Value = System.DateTime.Now.ToString("dd-MMM-yyyy");

                if (Request.QueryString["Type"] != null)
                {
                    hfType.Value = Request.QueryString["Type"].ToString();
                }

                if (!Request.QueryString["CertID"].Equals(""))
                {
                    intCertId = Convert.ToInt32(Request.QueryString["CertID"].ToString());
                    hfCertId.Value = intCertId.ToString();
                }

                if (!intCertId.Equals(0) && !intCertId.Equals(""))
                {
                    getCertificationChecklist(intCertId);
                }
                else
                {
                    btnChklistSubmit.Visible = false;
                    btnSubmit.Visible = false;
                    imgAdd.Visible = false;
                    imgDelete.Visible = false;
                    tblRejctionRemarks.Visible = false;
                }
            }
        }

        private void getCertificationChecklist(int intCertId)
        {
            string strStatusWiseRejection = "", strFromDate = "", strToDate = "", strStatus = "", strRejectionDt = "", strRejectedBy = "";
            DataTable dtChecklist = new DataTable();
            DataRow dr;
            try
            {
                DataTable dt = utilBL.getDatasetWithCondition("getCertIdWiseCertification", intCertId, strConnectionString);
                //DataTable dt = certBL.getChecklistByCertId(intCertId, strConnectionString);

                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                    strStatus = dr["CERT_STATUS"].ToString();
                    //if (strStatus.Equals("D") || strStatus.Equals("L1R") || strStatus.Equals("L2R") || strStatus.Equals("L3R"))
                    //{
                    hfQuarterEndDt.Value = (Convert.ToDateTime(dr["CQM_TO_DATE"].ToString())).ToString("dd-MMM-yyyy");
                    hfCertDepartment.Value = dr["CSSDM_NAME"].ToString();
                    txtRemarks.Text = dr["CERT_REMARKS"].ToString();
                    hfQuarterId.Value = dr["CERT_CQM_ID"].ToString();
                    strFromDate = (Convert.ToDateTime(dr["CQM_FROM_DATE"].ToString())).ToString("dd-MMM-yyyy");
                    strToDate = (Convert.ToDateTime(dr["CQM_TO_DATE"].ToString())).ToString("dd-MMM-yyyy");

                    hfQuarter.Value = strFromDate + " to " + strToDate;

                    if (strStatus.Equals("L1R") || strStatus.Equals("L2R") || strStatus.Equals("L3R"))
                    {
                        if (strStatus.Equals("L2R"))
                        {
                            strStatusWiseRejection = "CERT_REJECTED_REMARKS_LEVEL1";
                            strRejectionDt = "CERT_REJECTED_DT_LEVEL1";
                            strRejectedBy = "CERT_REJECTED_BY_LEVEL1";
                        }
                        else if (strStatus.Equals("L3R"))
                        {
                            strStatusWiseRejection = "CERT_REJECTED_REMARKS_LEVEL2";
                            strRejectionDt = "CERT_REJECTED_DT_LEVEL2";
                            strRejectedBy = "CERT_REJECTED_BY_LEVEL2";
                        }

                        writeError("Certification is rejected by " + dr[strRejectedBy] + " on " +
                            String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(dr[strRejectionDt])) + " , so kindly do the changes and resubmit the certification.");
                        lblRejectionRemarks.Text = dr[strStatusWiseRejection].ToString().Replace(Environment.NewLine, "<br />");
                    }
                    //<<List of Exceptions.
                    DataTable dtExc = utilBL.getDatasetWithCondition("getExceptionByCertId", Convert.ToInt32(hfCertId.Value), strConnectionString);
                    //DataTable dtExc = utilBL.getDataset("getDraftedException", strConnectionString).Tables[0];
                    DataRow drExc;
                    int intExcCnt = dtExc.Rows.Count;
                    string strCEId, strException, strDetails, strClientFile, strServerFile, strRootCause, strActiontaken, strtxtTargetDate, strActionStatus;
                    int uniqueRowId = 0;
                    string strHtmlTableRows = "";
                    string strHtmlTable = " <table id='tblException' class='table table-bordered footable' width='100%'> " +
                                 " <thead> " +
                                 " <tr> " +
                                 " <th class='tabhead3' align='center'> " +
                                 " <input type='checkbox' ID='HeaderLevelCheckBox' onclick = 'return onHeaderRowChecked()' /> " +
                                 " </th> " +
                                 " <th class='tabhead3'> " +
                                 " Attach " +
                                 " </th> " +
                                 " <th class='tabhead3'> " +
                                 " File Name " +
                                 " </th> " +
                                 " <th class='tabhead3'> " +
                                 " Deviation (Detailed) " +
                                 " </th> " +
                                 " <th class='tabhead3'> " +
                                 " Regulatory Reference (Detailed) " +
                                 " </th> " +
                                 " <th class='tabhead3'> " +
                                 " Root Cause for the Deviation " +
                                 " </th> " +
                                 " <th class='tabhead3'> " +
                                 " Action taken " +
                                 " </th> " +
                                 " <th class='tabhead3'> " +
                                 " Current Status " +
                                 " </th> " +
                                 " <th class='tabhead3'> " +
                                 "Target/Closure Date (eg. 01-Jan-2025)" +
                                 " </th> " +
                                 " </tr> " +
                                 " </thead> ";

                    for (int intCnt = 0; intCnt < intExcCnt; intCnt++)
                    {
                        uniqueRowId = uniqueRowId + 1;
                        drExc = dtExc.Rows[intCnt];
                        strCEId = drExc["CE_ID"].ToString();
                        strException = drExc["CE_EXCEPTION_TYPE"].ToString();
                        strDetails = drExc["CE_DETAILS"].ToString();
                        strClientFile = drExc["CE_CLIENT_FILE_NAME"].ToString();
                        strServerFile = drExc["CE_SERVER_FILE_NAME"].ToString();
                        //Added By Milan Yadav on 05-Feb-2016
                        //<<
                        strRootCause = drExc["CE_ROOT_CAUSE_OF_DEVIATION"].ToString();
                        strActiontaken = drExc["CE_ACTION_TAKEN"].ToString();
                        strActionStatus = drExc["CE_CLOSURE_STATUS"].ToString();
                        if (drExc["CE_CLOSURE_STATUS"].ToString() != "Closed")
                        {
                            if (drExc["CE_TARGET_DATE"] != DBNull.Value)
                            {
                                strtxtTargetDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(drExc["CE_TARGET_DATE"]));
                            }
                            else
                            {
                                strtxtTargetDate = "";
                            }
                        }
                        else
                        {
                            if (drExc["CE_CLOSURE_DATE"] != DBNull.Value)
                            {
                                strtxtTargetDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(drExc["CE_CLOSURE_DATE"]));
                            }
                            else
                            {
                                strtxtTargetDate = "";
                            }
                        }
                        //>>
                        string strVisibilityAttach = "style='visibility:hidden'";
                        string strVisibilityDelete = "style='visibility:hidden'";

                        if (strClientFile.Equals(""))
                        {
                            strVisibilityAttach = "style='visibility:visible'";
                            strVisibilityDelete = "style='visibility:hidden'";
                        }
                        else
                        {
                            strVisibilityAttach = "style='visibility:hidden'";
                            strVisibilityDelete = "style='visibility:visible'";
                        }

                        strHtmlTableRows = strHtmlTableRows + "<tr><td class='tabbody3'>" +
                        "<input type='hidden' ID='uniqueRowId" + uniqueRowId + "' value='" + uniqueRowId + "' /><input type='hidden' ID='certId" + uniqueRowId + "' value='" + strCEId + "' /><input type='checkbox' ID='cbException" + uniqueRowId + "' /></td>" +
                        "<td class='tabbody3'><input type='hidden' ID='ServerFileName" + uniqueRowId + "' value='" + strServerFile +
                        "' /><input type='hidden' ID='ClientFileName" + uniqueRowId + "' value='" + strClientFile + "' /><a ID='EX_AttachFileImg" +
                        uniqueRowId + "' onclick='return openpopupExceptionAttachments(" + uniqueRowId + ")' " + strVisibilityAttach +
                        "><img border='0' src='../../Content/images/legacy/attach.png' /></a> " +
                        " <a ID='EX_DeleteFileImg" + uniqueRowId + "' onclick='return deleteExceptionFile(" + uniqueRowId + ")' " + strVisibilityDelete + "><img border='0' src='../../Content/images/legacy/delete.gif' /></a></td>" +
                        "<td class='tabbody3'><a ID='EX_Filelink" + uniqueRowId + "' href='../DownloadFileCertification.aspx?FileInformation=" +
                        strServerFile + "' >" + strClientFile + "</a></td>" +
                        "<td class='tabbody3'><textarea ID='ExceptionType" + uniqueRowId + "' cols='25' rows='3' maxLength = '4000' class = 'form-control' >" +
                        strException + "</textarea></td>" +
                        "<td class='tabbody3'><textarea ID='Details" + uniqueRowId + "' cols='25' rows='3' maxLength = '4000'  class = 'form-control' >"
                        + strDetails + "</textarea></td>" +
                        //Added By Milan Yadav on 05-Feb-2016
                        //<<
                        "<td class='tabbody3'><textarea ID='RootCause" + uniqueRowId + "'  cols='25' rows='3' maxLength = '4000'  class = 'form-control'>" + strRootCause + "</textarea></td>" +
                        "<td class='tabbody3'><textarea ID='Actiontaken" + uniqueRowId + "'  cols='25' rows='3' maxLength = '4000'  class = 'form-control'>" + strActiontaken + "</textarea></td>";
                        //"<td class='tabbody3'><input type='text' ID='txtTargetDate" + uniqueRowId + "'  cellElement.size = '15' maxLength = '100'  class = 'form-control' value='" + strtxtTargetDate + "'</></td>";

                        string html = "<td class='tabbody3'><select id='ddlActionStatus" + uniqueRowId + "' width='120px' class='form-select'>" +
                                      "<option value='Select'" + (strActionStatus == "Select" ? " selected" : "") + ">Select</option>" +
                                      "<option value='Open'" + (strActionStatus == "Open" ? " selected" : "") + ">Open</option>" +
                                      "<option value='Closed'" + (strActionStatus == "Closed" ? " selected" : "") + ">Closed</option>" +
                                      "</select></td>";
                        strHtmlTableRows = strHtmlTableRows + html;

                        if (strtxtTargetDate.Equals(""))
                        {
                            strHtmlTableRows = strHtmlTableRows +
                             "<td class='tabbody3'><input type='text' ID='txtTargetDate" + uniqueRowId + "' readonly='readonly' width='120px' maxLength = '11' size='15' class = 'form-control targetDate' value=''/></td>";
                        }
                        else
                        {
                            strHtmlTableRows = strHtmlTableRows +
                             "<td class='tabbody3'><input type='text' ID='txtTargetDate" + uniqueRowId + "' readonly='readonly' width='120px' maxLength = '11' size='15' class = 'form-control targetDate' value='" + strtxtTargetDate + "'/></td>";
                        }

                        //>>
                        strHtmlTableRows = strHtmlTableRows + "</tr>";
                    }

                    litControls.Text = strHtmlTable + strHtmlTableRows + " </table> ";


                    DataSet dsDates = utilBL.getDataset("CERTQUARTERS", strConnectionString);
                    DataRow drDates;
                    DataTable dtDates = dsDates.Tables[0];

                    if (dtDates.Rows.Count == 1)
                    {
                        drDates = dtDates.Rows[0];
                        strFromDate = (Convert.ToDateTime(drDates["CQM_FROM_DATE"].ToString())).ToString("dd-MMM-yyyy");
                        strToDate = (Convert.ToDateTime(drDates["CQM_TO_DATE"].ToString())).ToString("dd-MMM-yyyy");
                        hfQuarterEndDt.Value = strToDate.ToString();
                        hfQuarterId.Value = drDates["CQM_ID"].ToString();
                    }
                    //>>
                    hfQuarter.Value = strFromDate + " to " + strToDate;
                    //lblHeader.Text += " for " + hfCertDepartment.Value + "(" + strFromDate + " to " + strToDate + ")";
                }

                //utilVO.setCode(" and CCD_CERT_ID in (" + intCertId + ")");
                //dtChecklist = utilBLL.getData("getCertificationChecklist", utilVO);///CRC_NAME  as hecklistStatus
                dtChecklist = certBL.getChecklistByCertId(intCertId, strConnectionString);

                gvChecklist.DataSource = dtChecklist;
                gvChecklist.DataBind();
                Session["Checklist"] = dtChecklist;

                foreach (GridViewRow gr in gvChecklist.Rows)
                {
                    LinkButton lnkViewCirc = (LinkButton)gr.FindControl("lnkViewCirc");
                    HiddenField hfCircularId = (HiddenField)gr.FindControl("hfCircularId");
                    if (string.IsNullOrEmpty(hfCircularId.Value))
                    {
                        lnkViewCirc.Visible = false;
                    }
                    else
                    {
                        lnkViewCirc.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("System Exception in getCertificationChecklist(): " + ex.Message);
            }
        }
        //Added by Milan Yadav on 20-Jun-2016
        //<<
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

            int uniqueRowId = 0;
            string strChecklistTable = "", strHtmlTableChecklistDetsRows = "";
            DataTable dtChecklistDets;
            DataRow drChecklistDets;
            dtChecklistDets = (DataTable)Session["Checklist"];
            string strHtmlTable =
                "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                "<HTML>" +
                "<HEAD>" +
                "</HEAD>" +
                "<BODY>" +

                " <table id='tblChecklistDets' width='100%'  class='table table-bordered footable' align='left' style='margin-left:20px;' " +
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
        protected void btnSubmit_click(object sender, EventArgs args)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            try
            {
                saveCertification();
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("System Exception in btnSubmit_click: " + ex.Message);
            }
        }

        private void saveCertification()
        {
            string strRemarks = "";
            string strCreateBy = ((new Authentication()).getUserFullName(Page.User.Identity.Name));
            strRemarks = cm.getSanitizedString(txtRemarks.Text);
            string ExceptionNames = hfExceptions.Value;
            string[] strarrExcp, strarrExcpFields;
            string strTemp;
            DataRow dr;
            initExceptiondt();
            strarrExcp = ExceptionNames.Split('~');
            for (int i = 0; i < strarrExcp.Length - 1; i++)
            {
                strTemp = strarrExcp[i];
                strarrExcpFields = strTemp.Split('|');
                dr = dtException.NewRow();
                dr["ID"] = strarrExcpFields[1];
                dr["Exception Type"] = strarrExcpFields[2];
                dr["Details"] = strarrExcpFields[3];
                dr["Client File Name"] = strarrExcpFields[4];
                dr["Server File Name"] = strarrExcpFields[5];
                //Added By Milan Yadav on 05-Feb-2016
                //<<
                dr["RootCause"] = strarrExcpFields[6];
                dr["Actiontaken"] = strarrExcpFields[7];
                dr["Actionstatus"] = strarrExcpFields[8];
                if (strarrExcpFields[9] != null)
                {
                    dr["TargetDate"] = strarrExcpFields[9];
                }
                else
                {
                    dr["TargetDate"] = "";
                }
                //>>
                dtException.Rows.Add(dr);
            }
            //Added by Bhavik @ 10-Sep-2013
            int intCertId = 0;
            if (!hfCertId.Value.Equals(""))
            {
                intCertId = Convert.ToInt32(hfCertId.Value);
            }

            certBL.saveCertification(intCertId, 0, 0,
                            "", strRemarks, "", strCreateBy, dtException, getChecklistDets(), "", Page.User.Identity.Name);

            //if (strStatus.Equals("D"))
            //{
            //    getCertification();
            //    writeError("Certificate saved in draft mode successfully.");
            //}
            //else if (strStatus.Equals("S"))
            //{
            //    pnlCertificationDashboards.Visible = true;
            //    PnlCertStatus.Visible = false;
            //    getCertification();
            writeError("Certificate submitted successfully.");
            getCertificationChecklist(intCertId);
            //}
        }

        private void initExceptiondt()
        {
            dtException = new DataTable();
            dtException.Columns.Add(new DataColumn("ID", typeof(string)));
            dtException.Columns.Add(new DataColumn("Exception Type", typeof(string)));
            dtException.Columns.Add(new DataColumn("Details", typeof(string)));
            dtException.Columns.Add(new DataColumn("Client File Name", typeof(string)));
            dtException.Columns.Add(new DataColumn("Server File Name", typeof(string)));
            //Added By Milan Yadav on 05-Feb-2016
            //<<
            dtException.Columns.Add(new DataColumn("RootCause", typeof(string)));
            dtException.Columns.Add(new DataColumn("Actiontaken", typeof(string)));
            dtException.Columns.Add(new DataColumn("Actionstatus", typeof(string)));
            dtException.Columns.Add(new DataColumn("TargetDate", typeof(string)));
            //>>

        }


        public DataTable getChecklistDets()
        {
            DataTable dt_d = new DataTable();
            dt_d.Columns.Add(new DataColumn("ChecklistDetsId", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ChecklistMasId", typeof(string)));
            dt_d.Columns.Add(new DataColumn("CertificationId", typeof(string)));
            dt_d.Columns.Add(new DataColumn("YesNoNa", typeof(string)));
            dt_d.Columns.Add(new DataColumn("Remarks", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ClientFileName", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ServerFileName", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ActionPlan", typeof(string)));
            dt_d.Columns.Add(new DataColumn("TargetDate", typeof(string)));
            //<<Added by Ankur Tyagi on 05Feb2024 for CR_1948
            dt_d.Columns.Add(new DataColumn("NCSinceDate", typeof(string)));
            //>>

            foreach (GridViewRow gvr in gvChecklist.Rows)
            {
                DataRow dr;
                Label lblChecklistDetsId = (Label)gvr.FindControl("lblChecklistDetsId");
                DropDownList rbyesnona = (DropDownList)gvr.FindControl("rbyesnona");
                F2FTextBox txtRemarks = (F2FTextBox)gvr.FindControl("txtRemarks");
                HiddenField hfClientFileName = (HiddenField)gvr.FindControl("ClientFileName");
                HiddenField hfServerFileName = (HiddenField)gvr.FindControl("ServerFileName");
                F2FTextBox txtActionPlan = (F2FTextBox)gvr.FindControl("txtActionPlan");
                F2FTextBox txtTargetDate = (F2FTextBox)gvr.FindControl("txtTargetDate");
                //<<Added by Ankur Tyagi on 05Feb2024 for CR_1948
                F2FTextBox txtNCSinceDate = (F2FTextBox)gvr.FindControl("txtNCSinceDate");
                //>>
                string strChecklistMasId = gvChecklist.DataKeys[gvr.RowIndex].Values[0].ToString();
                dr = dt_d.NewRow();
                dr["ChecklistDetsId"] = lblChecklistDetsId.Text;
                dr["ChecklistMasId"] = strChecklistMasId;
                dr["YesNoNa"] = rbyesnona.SelectedValue;
                dr["Remarks"] = txtRemarks.Text;
                dr["ClientFileName"] = hfClientFileName.Value;
                dr["ServerFileName"] = hfServerFileName.Value;
                dr["ActionPlan"] = txtActionPlan.Text;
                dr["TargetDate"] = txtTargetDate.Text;
                //<<Added by Ankur Tyagi on 05Feb2024 for CR_1948
                dr["NCSinceDate"] = txtNCSinceDate.Text;
                //>>

                dt_d.Rows.Add(dr);
            }
            return dt_d;
        }

        protected void gvChecklist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RefCodesBLL refbl = new RefCodesBLL();
            ListItem li = new ListItem();
            li.Text = "(Select)";
            li.Value = "";
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                Label lblaction = (Label)(e.Row.FindControl("lblaction"));
                DropDownList rbyesnona = (DropDownList)(e.Row.FindControl("rbyesnona"));

                rbyesnona.DataSource = refbl.getRefCodeDetails("Certification Compliance Status", strConnectionString);
                rbyesnona.DataBind();
                rbyesnona.Items.Insert(0, li);
                if (!lblaction.Text.Equals(""))
                {
                    rbyesnona.SelectedValue = lblaction.Text;
                }

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

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void btnClose_click(object sender, EventArgs e)
        {
            //string script = "\r\n<script language=\"javascript\">\r\n" +
            //                       " closeFileWindow();" +
            //                       "   </script>\r\n";
            //ClientScript.RegisterStartupScript(this.GetType(), "script", script);

            //if (!hfType.Value.Equals(""))
            //{
            //    Response.Redirect(Global.site_url("Projects/Certification/CertificationApproval.aspx?Type=" + hfType.Value));
            //}
            //else if (ViewState["PreviousPage"] != null)
            //{
            //    Response.Redirect(ViewState["PreviousPage"].ToString());
            //}

            if (hfType.Value.Equals("L2"))
            {
                Response.Redirect(Global.site_url("Projects/Certification/CertificationApproval.aspx?Type=" + hfType.Value));
            }
            else if (hfType.Value.Equals("L3"))
            {
                Response.Redirect(Global.site_url("Projects/Certification/CertificationCXOApproval.aspx?Type=" + hfType.Value));
            }
            else if (hfType.Value.Equals("L4"))
            {
                Response.Redirect(Global.site_url("Projects/Certification/CertificationCXOApproval.aspx?Type=" + hfType.Value));
            }
            else if (hfType.Value.Equals("L5"))
            {
                Response.Redirect(Global.site_url("Projects/Certification/CertificationExCoApproval.aspx?Type=" + hfType.Value));
            }
            else if (ViewState["PreviousPage"] != null)
            {
                Response.Redirect(ViewState["PreviousPage"].ToString());
            }
        }

        protected void cvTargetDt_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
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


        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    string strApplicationLawPop="",strObservationPop="",strRootCausePop="",strActionTakenPop ="",
        //        strTargetDatePop = "", strCreateBy = "", strClientFileName = "", strServerFileName="";
        //    int retval;
        //    try
        //    {
        //        if (!hfCertId.Value.Equals(""))
        //        {
        //            intCertId = Convert.ToInt32(hfCertId.Value);
        //        }
        //        //initExceptiondt();
        //        //DataRow dr;
        //        //for (int i = 0; i < gvExceptionDetails.Rows.Count; i++)
        //        //{
        //        //    DataRow exRow = (DataRow)gvExceptionDetails.Rows[i];
        //        //    exRow = dtException.NewRow();
        //        //    dr["ID"] = exRow[1];
        //        //    dr["Exception Type"] = exRow[2];
        //        //    dr["Details"] = exRow[3];
        //        //    dr["Client File Name"] = exRow[4];
        //        //    dr["Server File Name"] = exRow[5];
        //        //    //Added By Milan Yadav on 05-Feb-2016
        //        //    //<<
        //        //    dr["RootCause"] = exRow[6];
        //        //    dr["Actiontaken"] = exRow[7];
        //        //    dr["TargetDate"] = exRow[8];
        //        //    //>>
        //        //    dtException.Rows.Add(dr);
        //        //}
        //        strApplicationLawPop = txtApplicationLawPop.Text;
        //        strObservationPop = txtObservationPop.Text;
        //        strRootCausePop = txtRootCausePop.Text;
        //        strActionTakenPop = txtActionTakenPop.Text;
        //        strTargetDatePop = txtTargetDatePop.Text;
        //        strCreateBy = Page.User.Identity.Name;
        //        retval = certBL.insertException(intCertId, strApplicationLawPop, strObservationPop, strClientFileName, strServerFileName, strRootCausePop,
        //                  strActionTakenPop,strTargetDatePop,strCreateBy,strConnectionString);
        //        if (retval.Equals(1))
        //        {
        //            lblMsg.Text = "Exception added successfully.";
        //        }
        //        getCertificationChecklist(intCertId);
        //    }
        //    catch (Exception ex)
        //    {
        //        writeError("System Exception in btnSave_Click:" + ex.Message);
        //    }
        //}
    }
}