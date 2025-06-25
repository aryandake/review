using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using Fiction2Fact.F2FControls;
using System.Data.OleDb;
using System.Text;
using System.Web;
using System.IO;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_Certification1 : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ToString();
        UtilitiesBLL utilBL = new UtilitiesBLL();
        UtilitiesVO utilVO = new UtilitiesVO();
        UtilitiesBLL utilBLL = new UtilitiesBLL();
        CertificationMasterBL certBL = new CertificationMasterBL();
        CommonMethods cm = new CommonMethods();
        DataTable dtException;
        DataTable dt_checklist = new DataTable();
        int intCnt = 0;
        int intCounter = 0;
        string strUser, strRetVal = "";
        private string script = "\r\n<script language=\"javascript\">\r\n";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                strUser = Authentication.GetUserID(Page.User.Identity.Name);
                if (!Page.IsPostBack)
                {
                    DateTime dtCurrDate = System.DateTime.Now;
                    hfCurDate.Value = dtCurrDate.ToString("dd-MMM-yyyy");
                    hfCurrentDate.Value = dtCurrDate.ToString("dd-MMM-yyyy");
                    getCertification();
                }
                else
                {
                    writeError("");
                    //if (Session["Checklist"] != null)
                    //{
                    //    dt_checklist = (DataTable)Session["Checklist"];
                    //}
                    //else
                    //{
                    //    BindChecklist();
                    //}
                }
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Error in Page_Load:" + ex.Message);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            getCertification();
        }

        protected void gvCertDashboard_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string strDeptId = gvCertDashboard.SelectedValue.ToString();
            DataTable dtDetCXO = new DataTable();
            DataTable dtDetUnitHead = new DataTable();
            GridViewRow gvr = gvCertDashboard.SelectedRow;
            Label lblCertId = (Label)gvr.FindControl("lblCertId");
            Label lblDeptName = (Label)gvr.FindControl("lblDeptName");
            hfCertId.Value = lblCertId.Text.ToString();
            hfDepartmentID.Value = strDeptId;
            hfCertDepartment.Value = lblDeptName.Text.ToString();

            utilVO.setCode(" and CSSDM_ID = " + hfDepartmentID.Value);
            //utilVO.setCode(hfDepartmentID.Value);
            dtDetCXO = utilBLL.getData("getDeptCXO", utilVO);
            hfCXOName.Value = dtDetCXO.Rows[0]["CDM_CXO_NAME"].ToString();
            hfUnitName.Value = dtDetCXO.Rows[0]["CSDM_NAME"].ToString();
            hfUnitID.Value = dtDetCXO.Rows[0]["CSDM_ID"].ToString();
            hfFunctionId.Value = dtDetCXO.Rows[0]["CDM_ID"].ToString();

            utilVO.setCode(" and CSSDM_ID = " + hfDepartmentID.Value);
            dtDetCXO = utilBLL.getData("getDeptUnitHead", utilVO);
            hfUnitHead.Value = dtDetCXO.Rows[0]["CSDM_EMP_NAME"].ToString();


            pnlCertificationDashboards.Visible = false;
            PnlCertStatus.Visible = true;
            bindCertificationGrid();
        }

        private void getCertification()
        {

            DataTable dtCertDashboard;
            DateTime dtCertDate = System.DateTime.Now;
            lblHeader.Text = "Quarterly Compliance Certification";

            dtCertDashboard = certBL.getCertificationForPendingApproval("L1", Page.User.Identity.Name.ToString(),
                            strConnectionString);

            gvCertDashboard.DataSource = dtCertDashboard;
            gvCertDashboard.DataBind();

            if ((this.gvCertDashboard.Rows.Count == 0))
            {
                this.lblMsg.Text = "No records pending for your approval.";
                this.lblMsg.Visible = true;
                pnlCertificationDashboards.Visible = false;
                PnlCertStatus.Visible = false;
            }
            else
            {
                this.lblMsg.Text = String.Empty;
                this.lblMsg.Visible = false;
                pnlCertificationDashboards.Visible = true;
                PnlCertStatus.Visible = false;
            }
        }

        private void bindCertificationGrid()
        {
            string strStatusWiseRejection = "", strContent, strFromDate = "", strToDate = "", strStatus = "", strRejectionDt = "", strRejectedBy = "";
            DataTable dtChecklist = new DataTable();
            DataRow dr, drDates;
            DateTime dtCertDate = System.DateTime.Now;
            string[] strDetailsList;
            string strDetailsId;
            string strUserName = "";
            //string strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
            string strCreateBy = ((new Authentication()).getUserFullName(Page.User.Identity.Name));
            Authentication auth = new Authentication();
            strUserName = Page.User.Identity.Name.ToString();
            strDetailsId = auth.GetUserDetsByEmpCode(strUserName);
            strDetailsList = strDetailsId.Split('|');
            strUserName = Convert.ToString(strDetailsList[0]);
            //If Certification is exist then 
            if (!hfCertId.Value.ToString().Equals(""))
            {
                //Added By Milan on 01-June-2016
                //>>
                // DataTable dt = utilBL.getDatasetWithCondition("getCertIdWiseCertification", Convert.ToInt32(hfCertId.Value), strConnectionString);

                DataTable dt = certBL.getCertIdWiseCertification(Convert.ToInt32(hfCertId.Value), strConnectionString);
                //<<
                dr = dt.Rows[0];
                strStatus = dr["CERT_STATUS"].ToString();
                //<<Certification exists but is in save draft mode & rejected.            
                if (strStatus.Equals("D") || strStatus.Equals("L1R") || strStatus.Equals("L2R") || strStatus.Equals("L3R") || strStatus.Equals("L4R"))
                {
                    hfQuarterEndDt.Value = (Convert.ToDateTime(dr["CQM_TO_DATE"].ToString())).ToString("dd-MMM-yyyy");
                    hfCertDepartment.Value = dr["CSSDM_NAME"].ToString();
                    txtRemarks.Text = dr["CERT_REMARKS"].ToString();
                    hfQuarterId.Value = dr["CERT_CQM_ID"].ToString();
                    strFromDate = (Convert.ToDateTime(dr["CQM_FROM_DATE"].ToString())).ToString("dd-MMM-yyyy");
                    strToDate = (Convert.ToDateTime(dr["CQM_TO_DATE"].ToString())).ToString("dd-MMM-yyyy");

                    hfQuarter.Value = strFromDate + " to " + strToDate;
                    strContent = dr["CERT_CONTENT"].ToString();
                    //hfCertMId.Value = dr["CERT_CERTM_ID"].ToString();
                    lblCertContents.Text = strContent;
                    //hfDepartmentID.Value = dr["CDM_ID"].ToString();

                    lblHeader.Text = "Quarterly Compliance Certification for " + hfCertDepartment.Value + " (" + strFromDate + " to " + strToDate + ")";

                    if (strStatus.Equals("L1R") || strStatus.Equals("L2R") || strStatus.Equals("L3R") || strStatus.Equals("L4R"))
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
                        else if (strStatus.Equals("L4R"))
                        {
                            strStatusWiseRejection = "CERT_REJECTED_REMARKS_LEVEL2";
                            strRejectionDt = "CERT_REJECTED_DT_LEVEL3";
                            strRejectedBy = "CERT_REJECTED_BY_LEVEL3";
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
                    //"<script type='text/javascript' src='~/Content/js/legacy/Exception.js'></script>" +
                    string strHtmlTable = " <table class='table table-bordered footable' id='tblException' width='100%'> " +
                                 " <thead> " +
                                 " <tr> " +
                                 " <th class='tabhead3' align='center'> " +
                                 " <input type='checkbox' ID='HeaderLevelCheckBox' onclick = 'return onHeaderRowChecked()' /> " +
                                 " </th> " +
                                 " <th class='tabhead3' width='2%'> " +
                                 " Attach " +
                                 " </th> " +
                                 " <th class='tabhead3' width='10%'> " +
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
                                 " <th class='tabhead3' width='10%'> " +
                                 " Target/Closure Date (eg. 01-Jan-2025)" +
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
                        "<input type='hidden' ID='uniqueRowId" + uniqueRowId + "' value='" + uniqueRowId + "' /><input type='hidden' ID='certId" +
                        uniqueRowId + "' value='" + strCEId + "' /><input type='checkbox' ID='cbException" + uniqueRowId + "' /></td>" +
                        "<td class='tabbody3'><input type='hidden' ID='ServerFileName" + uniqueRowId + "' value='" + strServerFile +
                        "' /><input type='hidden' ID='ClientFileName" + uniqueRowId + "' value='" + strClientFile + "' /><a ID='EX_AttachFileImg"
                        + uniqueRowId + "' onclick='return openpopupExceptionAttachments(" + uniqueRowId + ")' " + strVisibilityAttach +
                        "><img border='0' src='../../Content/images/legacy/attach.png' /></a> " +
                        " <a ID='EX_DeleteFileImg" + uniqueRowId + "' onclick='return deleteExceptionFile(" + uniqueRowId + ")' " +
                        strVisibilityDelete + "><img border='0' src='../../Content/images/legacy/delete.gif' /></a></td>" +
                        "<td class='tabbody3'><a ID='EX_Filelink" + uniqueRowId + "' href='../DownloadFileCertification.aspx?FileInformation=" +
                        strServerFile + "' >" + strClientFile + "</a></td>" +
                        "<td class='tabbody3'><textarea ID='ExceptionType" + uniqueRowId + "' cols='25' rows='3' maxLength = '4000' class = 'form-control' >"
                        + strException + "</textarea></td>" +
                        "<td class='tabbody3'><textarea ID='Details" + uniqueRowId + "' cols='25' rows='3' maxLength = '4000'  class = 'form-control' >"
                        + strDetails + "</textarea></td>" +
                        //Added By Milan Yadav on 05-Feb-2016
                        //<<
                        "<td class='tabbody3'><textarea ID='RootCause" + uniqueRowId + "'  cols='25' rows='3' maxLength = '4000'  class = 'form-control'>" + strRootCause + "</textarea></td>" +
                        "<td class='tabbody3'><textarea ID='Actiontaken" + uniqueRowId + "'  cols='25' rows='3' maxLength = '4000'  class = 'form-control'>" + strActiontaken + "</textarea></td>";
                        //"<td class='tabbody3'><input type='text' ID='txtTargetDate" + uniqueRowId + "'  cellElement.size = '15' maxLength = '100'  class = 'textbox1' value='" + strtxtTargetDate + "'</></td>";


                        string html = "<td class='tabbody3'><select id='ddlActionStatus" + uniqueRowId + "' class='form-select'>" +
                                      "<option value='Select'" + (strActionStatus == "Select" ? " selected" : "") + ">Select</option>" +
                                      "<option value='Open'" + (strActionStatus == "Open" ? " selected" : "") + ">Open</option>" +
                                      "<option value='Closed'" + (strActionStatus == "Closed" ? " selected" : "") + ">Closed</option>" +
                                      "</select></td>";
                        strHtmlTableRows = strHtmlTableRows + html;

                        if (strtxtTargetDate.Equals(""))
                        {
                            strHtmlTableRows = strHtmlTableRows +
                             "<td class='tabbody3'><input type='text' ID='txtTargetDate" + uniqueRowId + "' readonly='readonly' maxLength = '11' size='15' class = 'form-control targetDate' value=''/></td>";
                        }
                        else
                        {
                            strHtmlTableRows = strHtmlTableRows +
                             "<td class='tabbody3'><input type='text' ID='txtTargetDate" + uniqueRowId + "' readonly='readonly' maxLength = '11' width='100px' size='15' class = 'form-control targetDate' value='" + strtxtTargetDate + "'/></td>";
                        }
                        //>>
                        strHtmlTableRows = strHtmlTableRows + "</tr>";
                    }

                    litControls.Text = strHtmlTable + strHtmlTableRows + " </table> ";
                }
                else
                {
                    //"<script type='text/javascript' src='../js/Exception.js'></script>" +
                    litControls.Text = " <table class='table table-bordered footable' id='tblException' width='100%'> " +
                                         " <thead> " +
                                         " <tr> " +
                                         " <th class='tabhead3' align='left' width='2%' > " +
                                         " <input type='checkbox' ID='HeaderLevelCheckBox' onclick = 'return onHeaderRowChecked()' /> " +
                                         " </th> " +
                                         " <th class='tabhead3' width='2%'> " +
                                         " Attach " +
                                         " </th> " +
                                         " <th class='tabhead3' width='10%'> " +
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
                                         " <th class='tabhead3' width='10%'> " +
                                         " Target/Closure Date (eg. 01-Jan-2025)" +
                                         " </th> " +
                                         " </tr> " +
                                         " </thead> </table> ";

                    DataSet dsDates = utilBL.getDataset("CERTQUARTERS", strConnectionString);
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
                    lblHeader.Text += " for " + hfCertDepartment.Value + " (" + strFromDate + " to " + strToDate + ")";
                }

                //BindChecklist
                dtChecklist = certBL.getCertificationsChecklist(hfCertId.Value.ToString(), hfDepartmentID.Value.ToString(),
                            strConnectionString);

                gvChecklist.DataSource = dtChecklist;
                Session["Checklist"] = dtChecklist;
                gvChecklist.DataBind();

                //<<Commented and Added by Rahuldeb on 24Sep2019
                //GetFilingsDashboard(hfDepartmentID.Value, hfCertDepartment.Value);
                Fiction2Fact.Legacy_App_Code.Certification.RegulatoryReportingDashboard rrd = new Fiction2Fact.Legacy_App_Code.Certification.RegulatoryReportingDashboard();
                litRegulatoryFilling.Text = rrd.GetFilingsDashboardSingleRow_AQ(hfDepartmentID.Value, hfCertDepartment.Value, "0");
                //>>

                //Added By Milan Yadav on 22-Feb-2016
                //>>
                int intLevel = 0;
                //To fetch the Certification Content in tab 1
                DataTable dtContent = utilBL.getDatasetWithThreeConditionsInString("getCertContentById", hfDepartmentID.Value.ToString(), intLevel, strConnectionString);
                DataRow dr1;
                if (dtContent.Rows.Count > 0)
                {
                    dr1 = dtContent.Rows[0];
                    //lblCertContents.Text = dr1["CERTM_TEXT"].ToString();
                    //Dept = dr1["CSSDM_NAME"].ToString();
                    //<< Commented By Milan Yadav on 17-Apr-2017
                    //strContent = dr1["CERTM_TEXT"].ToString();
                    //>>
                    strContent = dr1["CERT_CONTENT"].ToString();
                    strContent = strContent.Replace("~qtrStartDate", strFromDate);
                    strContent = strContent.Replace("~qtrEndDate", strToDate);
                    strContent = strContent.Replace("~Name", strUserName.ToString());
                    strContent = strContent.Replace("~Department", hfCertDepartment.Value);
                    strContent = strContent.Replace("~Date", dtCertDate.ToString("dd-MMM-yyyy"));
                    lblCertContents.Text = "<div style=\" font-family: Trebuchet MS;\">" + strContent + "</div>";
                }
                //>>
            }
        }

        #region Commented by Ramesh more
        //Added by Milan Yadav on 09-Jun-2016
        //<<
        //protected void btnExportToExcel_Click(object sender, EventArgs e)
        //{

        //    int uniqueRowId = 0;
        //    string strChecklistTable = "", strHtmlTableChecklistDetsRows = "";
        //    DataTable dtChecklistDets;
        //    DataRow drChecklistDets;
        //    dtChecklistDets = (DataTable)Session["Checklist"];
        //    string strHtmlTable =
        //        "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
        //        "<HTML>" +
        //        "<HEAD>" +
        //        "</HEAD>" +
        //        "<BODY>" +

        //        " <table id='tblChecklistDets' width='100%' align='left'  class='table table-bordered footable' " +
        //                    " cellpadding='0' cellspacing='1' border='1'> " +
        //                  " <thead> " +
        //                  " <tr> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Serial Number " +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Act/Regulation/Circular" +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Reference Circular/Notification/Act" +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Section/Clause" +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Compliance of/Heading of Compliance checklist" +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Description" +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Consequences of non Compliance" +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Frequency" +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Forms" +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Compliance Status" +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Remarks" +
        //                  " </th> " +
        //                  " <th class='tabhead' align='center'> " +
        //                  " Checklist File" +
        //                  " </th> " +
        //                  " </tr> " +
        //                  " </thead> ";


        //    int intChecklistDetsCnt = dtChecklistDets.Rows.Count;
        //    for (int intCnt = 0; intCnt < intChecklistDetsCnt; intCnt++)
        //    {
        //        uniqueRowId = uniqueRowId + 1;
        //        drChecklistDets = dtChecklistDets.Rows[intCnt];

        //        strHtmlTableChecklistDetsRows = strHtmlTableChecklistDetsRows + "<tr>" +
        //        "<td>" + uniqueRowId + "</td>" +
        //        "<td>" + drChecklistDets["CDTM_TYPE_OF_DOC"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["CCM_REFERENCE"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["CCM_CLAUSE"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["CCM_CHECK_POINTS"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["CCM_PARTICULARS"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["CCM_PENALTY"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["CCM_FREQUENCY"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["CCM_FORMS"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["ChecklistStatus"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["CCD_REMARKS"].ToString() + "</td>" +
        //        "<td>" + drChecklistDets["CCD_CLIENT_FILENAME"].ToString() + "</td>" +
        //        "</tr>";
        //    }
        //    strChecklistTable = strHtmlTable + strHtmlTableChecklistDetsRows + "</table> " +
        //    "</BODY>" +
        //    "</HTML>";
        //    string attachment = "attachment; filename=Details.xls";
        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", attachment);
        //    Response.ContentType = "application/ms-excel";

        //    Response.Write(strChecklistTable.ToString());
        //    Response.End();


        //}
        //>> 
        #endregion

        //<< Added by Ramesh more on 03-Jul-2024 CR_2114
        protected void btnImportFromExcel_Click(object sender, EventArgs e)
        {
            string url = "ImportComplianceChecklist.aspx";
            string s = "window.open('" + url + "', 'popup_window', 'location=0,status=0,resizable=1,scrollbars=1,width=1000,height=800');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        //>>

        //<< Added by Ramesh more on 04-Jul-2024 CR_2114
        public bool copyDirectory(string SourceDirectory, string TargetDirectory, string strTime)
        {
            try
            {
                File.Copy(SourceDirectory + "\\" + "ComplianceChecklistTemplate.xlsx", TargetDirectory + "\\" + "Cert_" + Page.User.Identity.Name + "_" + strTime + ".xlsx");
                return true;
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
                return false;

            }
        }

        public string getConStringForDownloadExcelFile(string strCompleteFileName, string strFileExtension)
        {
            string strMachineConfiguration = "", conString = "";
            strMachineConfiguration = (ConfigurationManager.AppSettings["MachineConfiguration"].ToString());

            if (strMachineConfiguration.Equals("32bit"))
            {
                if (strFileExtension.ToLower().Equals(".xls"))
                {
                    conString = "Provider=Microsoft.JET.OLEDB.4.0;" +
                     "Data Source=" + strCompleteFileName + ";" +
                     "Extended Properties='Excel 8.0;HDR=YES;'";
                }
                else if (strFileExtension.ToLower().Equals(".xlsx"))
                {
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                     "Data Source=" + strCompleteFileName + ";" +
                     "Extended Properties='Excel 12.0;HDR=YES;'";
                }
            }
            else if (strMachineConfiguration.Equals("64bit"))
            {
                conString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                      "Data Source=" + strCompleteFileName + ";" +
                      "Extended Properties='Excel 12.0;HDR=YES;'";
            }

            return conString;
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            string strTime = System.DateTime.Now.ToString("dd_MMM_yyyy_HH_mm_ss");
            string strSourceDir = Server.MapPath(ConfigurationManager.AppSettings["ChecklistExportTemplate"].ToString());
            string strDestinationDir = Server.MapPath(ConfigurationManager.AppSettings["TempChecklistExportedFiles"].ToString());
            string strMachineConfiguration = ConfigurationManager.AppSettings["MachineConfiguration"].ToString();

            try
            {
                if (copyDirectory(strSourceDir, strDestinationDir, strTime))
                {
                    DataTable dtdata = new DataTable();


                    dtdata = certBL.getCertificationsChecklist(hfCertId.Value.ToString(), hfDepartmentID.Value.ToString(),
                           strConnectionString);

                    StringBuilder sbInsert;

                    if (dtdata.Rows.Count > 0)
                    {
                        string conString = "";
                        string strFileName = "Cert_" + Page.User.Identity.Name + "_" + strTime + ".xlsx";
                        string fileName = strDestinationDir + "\\" + strFileName;

                        string strFileExtension = Path.GetExtension(strFileName).ToLower();
                        conString = getConStringForDownloadExcelFile(fileName, strFileExtension);

                        using (OleDbConnection con = new OleDbConnection(conString))
                        {
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                            }
                            for (int i = 0; i < dtdata.Rows.Count; i++)
                            {

                                sbInsert = new StringBuilder();
                                DataRow dr = dtdata.Rows[i];

                                sbInsert.Append(" Insert Into [Sheet1$] ");
                                sbInsert.Append("( ");
                                sbInsert.Append(" [Serial Number] ");
                                sbInsert.Append(" ,[ChecklistDets Id] ");
                                sbInsert.Append(" ,[ChecklistMas Id] ");
                                sbInsert.Append(" ,[Act/Regulation/Circular] ");
                                sbInsert.Append(" ,[Reference Circular/Notification/Act] ");
                                sbInsert.Append(" ,[Section/Clause] ");
                                sbInsert.Append(" ,[Compliance of/Heading of Compliance checklist] ");
                                sbInsert.Append(" ,[Description] ");//
                                sbInsert.Append(" ,[Consequences of non Compliance] ");
                                sbInsert.Append(" ,[Frequency] ");
                                sbInsert.Append(" ,[Forms] ");
                                sbInsert.Append(" ,[Compliance Status] ");
                                sbInsert.Append(" ,[Remarks / Reason of non compliance] ");
                                sbInsert.Append(" ,[Non-compliant since] ");
                                sbInsert.Append(" ,[Action Plan] ");
                                sbInsert.Append(" ,[Target Date] ");
                                //sbInsert.Append(" ,[Checklist File] ");
                                sbInsert.Append(" ) Values ");
                                sbInsert.Append(" ('" + (i + 1) + "', ");
                                sbInsert.Append(" '" + dr["CCD_ID"].ToString().Replace("'", "''") + "', ");
                                sbInsert.Append(" '" + dr["CCD_CCM_ID"].ToString().Replace("'", "''") + "', ");
                                sbInsert.Append(" '" + dr["CDTM_TYPE_OF_DOC"].ToString().Replace("'", "''") + "', ");
                                sbInsert.Append(" '" + dr["CCM_REFERENCE"].ToString().Replace("'", "''") + "', ");
                                sbInsert.Append(" '" + dr["CCM_CLAUSE"].ToString().Replace("'", "''") + "', ");
                                sbInsert.Append(" '" + dr["CCM_CHECK_POINTS"].ToString().Replace("'", "''") + "', ");
                                sbInsert.Append(" '" + dr["CCM_PARTICULARS"].ToString().Replace("'", "''") + "', ");
                                sbInsert.Append(" '" + dr["CCM_PENALTY"].ToString().Replace("'", "''") + "', ");
                                sbInsert.Append(" '" + dr["CCM_FREQUENCY"].ToString().Replace("'", "''") + "', ");
                                sbInsert.Append(" '" + dr["CCM_FORMS"].ToString().Replace("'", "''") + "', ");
                                sbInsert.Append(" '" + dr["ChecklistStatus"].ToString().Replace("'", "''") + "', ");
                                sbInsert.Append(" '" + dr["CCD_REMARKS"].ToString().Replace("'", "''") + "', ");
                                sbInsert.Append(" '" + dr["CCD_NC_SINCE_DT"].ToString().Replace("'", "''") + "', ");
                                sbInsert.Append(" '" + dr["CCD_ACTION_PLAN"].ToString().Replace("'", "''") + "', ");
                                sbInsert.Append(" '" + dr["CCD_TARGET_DATE"].ToString().Replace("'", "''") + "' ");
                                //sbInsert.Append(" '" + dr["CCD_CLIENT_FILENAME"].ToString().Replace("'", "''") + "' ");
                                sbInsert.Append(" ) ");

                                OleDbCommand cmdIns = new OleDbCommand(sbInsert.ToString(), con);
                                cmdIns.ExecuteNonQuery();
                            }
                            con.Close();
                        }

                        byte[] content = File.ReadAllBytes(fileName);
                        HttpContext context = HttpContext.Current;
                        context.Response.BinaryWrite(content);

                        context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                        context.Response.AppendHeader("Content-Disposition", "attachment; filename=Checklist_" + Page.User.Identity.Name + "_" + strTime + ".xlsx");

                        Context.Response.End();
                    }
                }
                else
                {
                    lblMsg.Text = "Error while exporting the checklist to Excel.";
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);

            }

        }
        //>>

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        private void saveCertification(string strStatus)
        {
            string strRemarks = "";
            //strActionPlan = "", strTargetDate = ""
            //string strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
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

            //intCId = certBL.saveCertification(intCertId, 0, 0, lblCertContents.Text, strRemarks, strStatus, strCreateBy, dtException, getChecklistDets(), hfDepartmentID.Value.ToString(), Page.User.Identity.Name);
            strRetVal = certBL.saveCertification(intCertId, 0, 0, lblCertContents.Text, strRemarks, strStatus, strCreateBy, dtException, getChecklistDets(), hfDepartmentID.Value.ToString(), Page.User.Identity.Name);

            if (strStatus.Equals("D"))
            {
                getCertification();
                writeError("Certificate saved in draft mode successfully.");
            }
            else if (strStatus.Equals("S"))
            {
                pnlCertificationDashboards.Visible = true;
                PnlCertStatus.Visible = false;
                getCertification();
                writeError("Certificate submitted successfully.");
            }
            txtRemarks.Text = String.Empty;
        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this))
            {
                hfDoubleClickFlag.Value = "";
                return;
            }
            try
            {
                saveCertification("D");
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Error in btnSaveDraft_Click: " + exp.Message);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this))
            {
                hfDoubleClickFlag.Value = "";
                return;
            }
            try
            {
                saveCertification("S");
                sendCertificateMail();
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Error in btnSubmit_Click: " + exp.Message);
            }
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

        private void sendCertificateMail()
        {
            try
            {
                //<<Modified by Ankur Tyagi on 01Feb2024 for CR_1945
                CommonCode cc = new CommonCode();
                Authentication auth = new Authentication();
                string LoggedinUser = (new Authentication()).getUserFullName(Page.User.Identity.Name); ;
                if (CheckIfAllCertificatesApproved(hfDepartmentID.Value))
                {
                    if (strRetVal.Split('|')[0] == "L1A")
                    {
                        cc.ParamMap.Add("ConfigId", 10);
                        cc.ParamMap.Add("To", "Level2");
                        cc.ParamMap.Add("cc", "Level1,Comp");
                        cc.ParamMap.Add("SubmittedBy", LoggedinUser);

                        cc.ParamMap.Add("CertDepartmentIdType", "SPOC");

                        cc.ParamMap.Add("CertDepartmentId", hfDepartmentID.Value.ToString());
                        cc.ParamMap.Add("CertDepartment", hfUnitName.Value.ToString());
                        cc.ParamMap.Add("Quarter", hfQuarter.Value.ToString());
                        cc.ParamMap.Add("CXOName", hfCXOName.Value);
                        cc.ParamMap.Add("UnitHeadName", hfUnitHead.Value);
                        cc.ParamMap.Add("UnitID", hfUnitID.Value);

                        cc.setCertificationMailContent();
                    }
                    else if (strRetVal.Split('|')[0] == "L2A")
                    {
                        cc.ParamMap.Add("ConfigId", 11);
                        cc.ParamMap.Add("To", "Level3");
                        cc.ParamMap.Add("cc", "Level2,Level1");
                        cc.ParamMap.Add("SubmittedBy", LoggedinUser);

                        cc.ParamMap.Add("CertDepartmentIdType", "UH");

                        cc.ParamMap.Add("CertDepartmentId", hfDepartmentID.Value);
                        cc.ParamMap.Add("CertDepartment", hfUnitName.Value);
                        cc.ParamMap.Add("Quarter", hfQuarter.Value.ToString());
                        cc.ParamMap.Add("CertIds", strRetVal.Split('|')[1]);
                        cc.ParamMap.Add("CXOName", hfCXOName.Value);
                        cc.ParamMap.Add("FunctionId", hfFunctionId.Value);
                        cc.setCertificationMailContent();
                    }
                    else if (strRetVal.Split('|')[0] == "L3A")
                    {
                        cc.ParamMap.Add("ConfigId", 12);
                        cc.ParamMap.Add("To", "Comp");
                        cc.ParamMap.Add("cc", "Level3,Level2,Level1");
                        cc.ParamMap.Add("SubmittedBy", LoggedinUser);

                        cc.ParamMap.Add("CertDepartmentIdType", "SPOC");

                        cc.ParamMap.Add("CertDepartmentId", hfDepartmentID.Value);
                        cc.ParamMap.Add("CertDepartment", hfCertDepartment.Value);
                        cc.ParamMap.Add("Quarter", hfQuarter.Value);
                        cc.ParamMap.Add("CertIds", hfCertId.Value);
                        cc.setCertificationMailContent();
                    }
                }
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in sendCertificateMail:" + exp);
            }
        }


        private bool CheckIfAllCertificatesApproved(string strSubSubDeptId)
        {
            string strApprovedCnt = "", strTotalCount = "", strSubDeptId = "";

            strSubDeptId = DataServer.ExecuteScalar("select CSSDM_CSDM_ID from TBL_CERT_SUB_SUB_DEPT_MAS where CSSDM_ID = " + strSubSubDeptId).ToString();

            if (strSubDeptId != "" && strSubDeptId != null)
            {
                strTotalCount = DataServer.ExecuteScalar(" select Count(1) from [TBL_CERTIFICATIONS] " +
                                                         " inner join TBL_CERT_QUARTER_MAS on CERT_CQM_ID = CQM_ID and CQM_STATUS = 'A' " +
                                                         " inner join TBL_CERT_MAS on CERTM_ID = CERT_CERTM_ID and CERTM_LEVEL_ID=0 " +
                                                         " inner join TBL_CERT_SUB_SUB_DEPT_MAS on CERTM_DEPT_ID = CSSDM_ID " +
                                                         " inner join TBL_CERT_SUB_DEPT_MAS on CSDM_ID = CSSDM_CSDM_ID and CSDM_ID = "
                                                         + strSubDeptId).ToString();

                strApprovedCnt = DataServer.ExecuteScalar(" select Count(1) from [TBL_CERTIFICATIONS] " +
                                                          " inner join TBL_CERT_QUARTER_MAS on CERT_CQM_ID = CQM_ID and CQM_STATUS = 'A' " +
                                                          " inner join TBL_CERT_MAS on CERTM_ID = CERT_CERTM_ID and CERTM_LEVEL_ID=0 " +
                                                          " inner join TBL_CERT_SUB_SUB_DEPT_MAS on CERTM_DEPT_ID = CSSDM_ID " +
                                                          " inner join TBL_CERT_SUB_DEPT_MAS on CSDM_ID = CSSDM_CSDM_ID and CERT_STATUS in ('L1A','L2A','L3A') " +
                                                          " and CSDM_ID = "
                                                          + strSubDeptId).ToString();

                if (strTotalCount == strApprovedCnt)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected void gvChecklist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = (DataTable)(Session["Checklist"]);
            RefCodesBLL refbl = new RefCodesBLL();
            ListItem li = new ListItem();
            li.Text = "(Select)";
            li.Value = "";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    int intCntRec = dt.Rows.Count;
                    if ((e.Row.RowType == DataControlRowType.DataRow))
                    {
                        Label lblsrno = (Label)(e.Row.FindControl("lblsrno"));
                        int index = Convert.ToInt32(lblsrno.Text) + 1;
                        // Label lblaction = (Label)(e.Row.FindControl("lblaction"));
                        HiddenField hfChecklistDetsId = (HiddenField)e.Row.FindControl("hfChecklistDetsId");
                        // RadioButtonList rbyesnona = (RadioButtonList)(e.Row.FindControl("rbyesnona"));
                        HiddenField hfChecklistFileId = (HiddenField)(e.Row.FindControl("hfChecklistFileId"));
                        HiddenField ClientFileName = (HiddenField)(e.Row.FindControl("ClientFileName"));
                        HiddenField ServerFileName = (HiddenField)(e.Row.FindControl("ServerFileName"));

                        HiddenField hfCircularId = (HiddenField)e.Row.FindControl("hfCircularId");
                        LinkButton lnkViewCirc = (LinkButton)(e.Row.FindControl("lnkViewCirc"));

                        if (hfCircularId.Value.Equals(""))
                        {
                            lnkViewCirc.Visible = false;
                        }
                        else
                        {
                            lnkViewCirc.Visible = true;
                        }

                        lnkViewCirc.OnClientClick = "onClientViewCircClick('" + (new SHA256EncryptionDecryption()).Encrypt(hfCircularId.Value) + "');";

                        //Added By Milan yadav On 08-Jun-2016
                        //>>
                        DropDownList ddrbyesnona = (DropDownList)(e.Row.FindControl("ddlrbyesnona"));
                        ddrbyesnona.DataSource = refbl.getRefCodeDetails("Certification Compliance Status", strConnectionString);
                        ddrbyesnona.DataBind();
                        ddrbyesnona.Items.Insert(0, li);

                        HiddenField hfaction = (HiddenField)e.Row.FindControl("hfaction");
                        //<<
                        //>>Commented By Milan Yadav on 08-June-2016
                        //>>
                        //if (!lblaction.Text.Equals(""))
                        //{
                        //    //rbyesnona.SelectedValue = lblaction.Text;
                        //    ddrbyesnona.SelectedValue= lblaction.Text;
                        //}
                        //<<
                        if (!hfaction.Value.Equals(""))
                        {
                            //rbyesnona.SelectedValue = lblaction.Text;
                            ddrbyesnona.SelectedValue = hfaction.Value;
                        }
                        utilVO.setCode(" and CCD_ID = " + hfChecklistDetsId.Value);
                        DataTable dt1 = utilBLL.getData("getChecklistFile", utilVO);
                        DataRow dr1;
                        if (dt1.Rows.Count > 0)
                        {
                            dr1 = dt1.Rows[0];
                            ClientFileName.Value = dr1["CCD_CLIENT_FILENAME"].ToString();
                            ServerFileName.Value = dr1["CCD_SERVER_FILENAME"].ToString();
                            hfChecklistFileId.Value = dr1["CCD_ID"].ToString();
                        }

                        script = script + "\r\n" +
                        " getFileLink('" + ServerFileName.ClientID + "','" +
                        ClientFileName.ClientID + "'," + index + ");";
                        intCounter = intCounter + 1;
                    }
                    intCnt = intCnt + 1;
                    if (intCnt == intCntRec + 1)
                    {
                        script = script + "</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "script", script);
                    }
                }
            }
        }

        public DataTable getChecklistDets()
        {
            DataTable dt_d = new DataTable();
            dt_d.Columns.Add(new DataColumn("ChecklistDetsId", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ChecklistMasId", typeof(string)));
            dt_d.Columns.Add(new DataColumn("CertificationId", typeof(string)));
            dt_d.Columns.Add(new DataColumn("YesNoNa", typeof(string)));
            dt_d.Columns.Add(new DataColumn("Remarks", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ActionPlan", typeof(string)));
            dt_d.Columns.Add(new DataColumn("TargetDate", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ClientFileName", typeof(string)));
            dt_d.Columns.Add(new DataColumn("ServerFileName", typeof(string)));
            //<<Added by Ankur Tyagi on 05Feb2024 for CR_1948
            dt_d.Columns.Add(new DataColumn("NCSinceDate", typeof(string)));
            //>>

            foreach (GridViewRow gvr in gvChecklist.Rows)
            {
                DataRow dr;
                F2FTextBox txtRemarks = (F2FTextBox)gvr.FindControl("txtRemarks");
                F2FTextBox txtActionPlan = (F2FTextBox)gvr.FindControl("txtActionPlan");
                F2FTextBox txtTargetDate = (F2FTextBox)gvr.FindControl("txtTargetDate");
                HiddenField hfClientFileName = (HiddenField)gvr.FindControl("ClientFileName");
                HiddenField hfServerFileName = (HiddenField)gvr.FindControl("ServerFileName");
                HiddenField hfChecklistDetsId = (HiddenField)gvr.FindControl("hfChecklistDetsId");
                DropDownList ddrbyesnona = (DropDownList)gvr.FindControl("ddlrbyesnona");
                //<<Added by Ankur Tyagi on 05Feb2024 for CR_1948
                F2FTextBox txtNCSinceDate = (F2FTextBox)gvr.FindControl("txtNCSinceDate");
                //>>
                string strChecklistMasId = gvChecklist.DataKeys[gvr.RowIndex].Values[0].ToString();
                dr = dt_d.NewRow();
                dr["ChecklistDetsId"] = hfChecklistDetsId.Value;
                dr["ChecklistMasId"] = strChecklistMasId;
                dr["YesNoNa"] = ddrbyesnona.SelectedValue;
                dr["Remarks"] = cm.getSanitizedString(txtRemarks.Text);
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

        protected void cvTargetDt_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        #region Commented code for Export To Excel

        /*protected void btnExporttoExcel_Click(object sender, EventArgs e)
        {
            string strTime = System.DateTime.Now.ToString("dd_MMM_yyyy_HH_mm_ss");
            string strSourceDir = Server.MapPath(ConfigurationManager.AppSettings["CertificationChecklistTemplate"].ToString());
            string strDestinationDir = Server.MapPath("~/Temp/");
            if (copyDirectory(strSourceDir, strDestinationDir, strTime))
            {
                DataTable dtdata = (DataTable)Session["Checklist"];
                StringBuilder sb = new StringBuilder();
                if (dtdata.Rows.Count > 0)
                {
                    string fileName = strDestinationDir + "\\" + "CertificationChecklistTemplate_" + strTime + ".xls";
                    string conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HDR=Yes;';";
                    using (OleDbConnection con = new OleDbConnection(conString))
                    {

                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }

                        string strInsert = "";

                        for (int i = 0; i < dtdata.Rows.Count; i++)
                        {
                            DataRow dr = dtdata.Rows[i];
                            string strComplianceStatus = "";
                            if (dr["CCD_YES_NO_NA"].ToString().Equals("C"))
                                strComplianceStatus = "Compliant";
                            else if (dr["CCD_YES_NO_NA"].ToString().Equals("N"))
                                strComplianceStatus = "Not Compliant";
                            else if (dr["CCD_YES_NO_NA"].ToString().Equals("NA"))
                                strComplianceStatus = "Not yet applicable";
                            else if (dr["CCD_YES_NO_NA"].ToString().Equals("W"))
                                strComplianceStatus = "Work in progress";
                            else
                                strComplianceStatus = "";

                            strInsert = "Insert into [Sheet1$] " +
                            "([SrNo],[DepartmentId],[ChecklistMasId],[ChecklistDetsId],[Reference],[Clause],[Particulars],[Frequency]," +
                            "[To be filed with],[Compliance Status],[Remarks]" +


                            ") values('" + (i + 1) + "'," +
                            "'" + hfDepartmentID.Value.ToString() + "'," +
                            "'" + dr["CCM_ID"].ToString()+ "'," +
                            "'" + dr["CCD_ID"].ToString() + "'," +
                            "'" + dr["CCM_REFERENCE"].ToString().Replace("'", "''") + "'," +
                            "'" + dr["CCM_CLAUSE"].ToString().Replace("'", "''") + "'," +
                            "'" + dr["CCM_PARTICULARS"].ToString().Replace("'", "''") + "'," +
                            //"'" + dr["CCM_CHECK_POINTS"].ToString().Replace("'", "''") + "'," +
                            "'" + dr["CCM_FREQUENCY"].ToString().Replace("'", "''") + "'," +
                            //"'" + dr["CCM_DUE_DATE"].ToString().Replace("'", "''") + "'," +
                            //"'" + dr["CCM_SOURCE_DEPT"].ToString().Replace("'", "''") + "'," +
                            //"'" + dr["CCM_DEPT_RESP_FURNISH"].ToString().Replace("'", "''") + "'," +
                            //"'" + dr["CCM_DEPT_RESP_SUBMITTING"].ToString().Replace("'", "''") + "'," +
                            "'" + dr["CCM_TO_BE_FILLED_WITH"].ToString().Replace("'", "''") + "'," +
                            //"'" + dr["CCD_YES_NO_NA"].ToString().Replace("'", "''") + "'," +
                            "'" + strComplianceStatus.ToString().Replace("'", "''") + "'," +
                            "'" + dr["CCD_REMARKS"].ToString().Replace("'", "''") + "')";

                            OleDbCommand cmdIns = new OleDbCommand(strInsert, con);

                            cmdIns.ExecuteNonQuery();
                        }
                        con.Close();
                    }
                    ////Create Downloadable file
                    byte[] content = File.ReadAllBytes(fileName);
                    HttpContext context = HttpContext.Current;

                    context.Response.BinaryWrite(content);

                    context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=Checklist.xls");
                    Context.Response.End();
                }
            }
            else
            {
                lblImportMsg.Text = "Error while Export to Excel Checklist.";
            }
        }

        public bool copyDirectory(string SourceDirectory, string TargetDirectory, string strTime)
        {
            try
            {
                File.Copy(SourceDirectory + "\\" + "CertificationChecklistTemplate.xls", TargetDirectory + "\\" + "CertificationChecklistTemplate_" + strTime + ".xls");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                string strScript = "";
                //BindChecklist();
                strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                "alert('Checklist Refresh Sucessfully......');\r\n" +
                    "</script>\r\n";
                ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);
            }
            catch (Exception ex)
            {
                lblImportMsg.Text = ex.Message;
            }
        }*/

        #endregion

        //Commented by Rahuldeb on 24Sep2019
        //Added by Aditya Mahakal on 07-Jan-2019
        //private void GetFilingsDashboard(string strDeptId, string strDeptName)
        //{
        //    DataTable dt = new DataTable();
        //    string strHtmlTable = "";
        //    int intCompliedCount, intNonCompliedCount, intNACount, intNotSubmitted, intNotYetDue = 0, intTotalSubmitted = 0,
        //       intTotalCount, intCompliedTotalCount = 0, intNonCompliedTotalCount = 0, intNATotalCount = 0, intDueandNotSubmitted = 0,
        //       intNotSubmittedTotal = 0, intOverAllCount = 0;
        //    myconnection = new SqlConnection(strConnectionString);
        //    try
        //    {

        //        strHtmlTable = "<table width='100%' cellpadding='0' cellspacing='0'>";
        //        strHtmlTable += "<tr><td class= 'DBTableFirstCellRight'>Department</td>";
        //        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Compliant</td>";
        //        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Compliant</td>";
        //        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Not Applicable</td>";
        //        strHtmlTable += "<td class= 'DBTableFirstCellRight'>Total</td></tr>";

        //        intCompliedCount = 0;
        //        intNonCompliedCount = 0;
        //        intNACount = 0;
        //        intNotSubmitted = 0;
        //        intTotalCount = 0;
        //        intNotYetDue = 0;
        //        intTotalSubmitted = 0;
        //        intDueandNotSubmitted = 0;

        //        myconnection.Open();
        //        int intReptDeptId = Convert.ToInt32(DataServer.ExecuteScalar(" select isnull(SRD_ID,0) from TBL_SUB_REPORTING_DEPT " +
        //            " inner join TBL_SRD_CSSDM_MAPPING ON SRD_ID = SRCM_SRD_ID AND SRCM_CSSDM_ID = " + strDeptId));

        //        String strQMFromDT = (Convert.ToDateTime(DataServer.ExecuteScalar("select CQM_FROM_DATE FROM TBL_CERT_QUARTER_MAS where CQM_STATUS = 'A'"))).ToString("dd-MMM-yyyy");
        //        String strQMToDT = (Convert.ToDateTime(DataServer.ExecuteScalar("select CQM_TO_DATE FROM TBL_CERT_QUARTER_MAS where CQM_STATUS = 'A'"))).ToString("dd-MMM-yyyy");

        //        if (intReptDeptId != 0)
        //        {

        //            string strCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
        //                                             " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
        //                                             " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
        //                                             " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
        //                                             " where SUB_STATUS in ('S','C') and SM_SRD_ID = " + intReptDeptId +
        //                                             " and DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) <= SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y' " +
        //                                             " and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'";


        //            string strNonCompliedCount = " select count(1) from TBL_SUB_CHKLIST " +
        //                                              " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
        //                                              " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID " +
        //                                              " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
        //                                              " where SUB_STATUS in ('S','C') and SM_SRD_ID = " + intReptDeptId +
        //                                              " and ((DATEADD(dd, 0, DATEDIFF(dd, 0,isnull(SUB_SUBMITTED_TO_AUTHORITY_ON,SUB_SUBMIT_DATE))) > SC_DUE_DATE_TO and SUB_YES_NO_NA = 'Y') OR SUB_YES_NO_NA = 'N') " +
        //                                              "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'";

        //            string strNACount = " select count(1) from TBL_SUB_CHKLIST " +
        //                                             " inner  join TBL_SUB_TYPE_MAS on SC_STM_ID = STM_ID  " +
        //                                             " inner  join TBL_SUBMISSIONS_MAS on  SC_SM_ID = SM_ID  " +
        //                                             " left outer join TBL_SUBMISSIONS on SC_ID = [SUB_SC_ID] " +
        //                                             " where SUB_STATUS in ('S','C') and SM_SRD_ID = " + intReptDeptId + " and SUB_YES_NO_NA = 'NA'" +
        //                                              "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'";


        //            string strQTotal = " select Count(*) from TBL_SUB_CHKLIST " +
        //                                        " INNER JOIN TBL_SUB_TYPE_MAS on STM_ID = SC_STM_ID " +
        //                                        " INNER JOIN TBL_SUBMISSIONS_MAS on SC_SM_ID = SM_ID " +
        //                                        " left outer join TBL_SUBMISSIONS on SC_ID = SUB_SC_ID " +
        //                                        " where SM_SRD_ID = " + intReptDeptId + " and  isnull(SUB_STATUS,'') != 'R'" +
        //                                        "and SC_DUE_DATE_TO >= '" + strQMFromDT + "' and SC_DUE_DATE_TO <= '" + strQMToDT + "'";

        //            intCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strCompliedCount));
        //            intNonCompliedCount = Convert.ToInt32(DataServer.ExecuteScalar(strNonCompliedCount));
        //            intNACount = Convert.ToInt32(DataServer.ExecuteScalar(strNACount));

        //            intTotalCount = Convert.ToInt32(DataServer.ExecuteScalar(strQTotal));

        //            strHtmlTable += "<tr><td class= 'DBTableCellLeft'><center>" + strDeptName + "</center></td>";
        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                            "<a href='#'  onclick=\"window.open(" +
        //                            "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=Y&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
        //                            "&SRDID=" + intReptDeptId + "&STMID=&Frequency=&Priority='," +
        //                            "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                            intCompliedCount +
        //                            "</a>" +
        //                            "</td>";

        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                                "<a href='#'  onclick=\"window.open(" +
        //                                "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=N&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
        //                                "&SRDID=" + intReptDeptId + "&STMID=&Frequency=&Priority='," +
        //                                "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                                intNonCompliedCount +
        //                                "</a>" +
        //                                "</td>";

        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                                "<a href='#'  onclick=\"window.open(" +
        //                                "'DetailedReportCertificationFilling.aspx?ReportType=1&Status=NA&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
        //                                "&SRDID=" + intReptDeptId + "&STMID=&Frequency=&Priority='," +
        //                                "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                                +intNACount +
        //                                "</a>" +
        //                                "</td>";

        //            strHtmlTable += "<td class= 'DBTableCellRight'>" +
        //                               "<a href='#'  onclick=\"window.open(" +
        //                               "'DetailedReportCertificationFilling.aspx?ReportType=1&FromDate=" + strQMFromDT + "&ToDate=" + strQMToDT + "&DeptName=" + strDeptName +
        //                               "&SRDID=" + intReptDeptId + "&STMID=&Frequency=&Priority='," +
        //                               "'FILE', 'location=0,status=0,scrollbars=1,width=850,height=950,resizable=yes');return false;\">" +
        //                               +intTotalCount +
        //                               "</a>" +
        //                               "</td></tr>";

        //            strHtmlTable += "</table>";
        //            litRegulatoryFilling.Text = strHtmlTable;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw new System.Exception("system exception in GetFilingsDashboard() " + ex);
        //    }
        //    finally
        //    {
        //        myconnection.Close();
        //    }
        //}
        //>>
    }
}