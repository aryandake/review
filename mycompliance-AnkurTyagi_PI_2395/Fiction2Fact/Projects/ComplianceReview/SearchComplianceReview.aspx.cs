using Fiction2Fact.Legacy_App_Code.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using Fiction2Fact.Legacy_App_Code;
using DocumentFormat.OpenXml.Vml;
using Fiction2Fact.App_Code;
using System.Text;
using System.Data;
using Fiction2Fact.Legacy_App_Code.DAL;
using Fiction2Fact.Legacy_App_Code.VO;
using System.IO;

namespace Fiction2Fact.Projects.Compliance
{
    public partial class ViewComplianceReview : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        RefCodesBLL refBL = new RefCodesBLL();
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillUniverseMaster();
                FillReviewerName();
                FillStatus();
                FillComplianceStatus();

                if (Request.QueryString["Type"] != null)
                {
                    hfType.Value = Request.QueryString["Type"].ToString();
                    if (hfType.Value.Equals("RES"))
                    {
                        hfTypeOfView.Value = "RES";
                        gvComplianceReview.Columns[1].Visible = false;
                        gvComplianceReview.Columns[2].Visible = false;
                        gvComplianceReview.Columns[3].Visible = false;
                        gvComplianceReview.Columns[4].Visible = false;
                        gvComplianceReview.Columns[6].Visible = true;
                        gvComplianceReview.Columns[8].Visible = false;
                        lblHeader.Text = "Search Issue Tracker";
                        searchcompliance();
                    }
                    else if (hfType.Value.Equals("MY"))
                    {
                        gvComplianceReview.Columns[1].Visible = true;
                        gvComplianceReview.Columns[2].Visible = false;
                        gvComplianceReview.Columns[3].Visible = false;
                        gvComplianceReview.Columns[4].Visible = false;
                        gvComplianceReview.Columns[6].Visible = false;
                        gvComplianceReview.Columns[8].Visible = true;
                        lblHeader.Text = "Search Compliance Review";
                        searchcompliance();
                    }
                }
                else
                {
                    gvComplianceReview.Columns[1].Visible = false;
                    gvComplianceReview.Columns[2].Visible = false;
                    gvComplianceReview.Columns[3].Visible = false;
                    gvComplianceReview.Columns[4].Visible = false;
                    gvComplianceReview.Columns[6].Visible = false;
                    gvComplianceReview.Columns[8].Visible = false;
                    lblHeader.Text = "Search Compliance Review";
                }

            }
        }

        void FillComplianceStatus()
        {
            UtilitiesDAL ouBLL = new UtilitiesDAL();
            UtilitiesVO oUV = new UtilitiesVO();
            oUV.setCode("  where SM_TYPE='Compliance Review Status' Order by SM_SORT_ORDER asc");
            ddlComplianceStatus.DataSource = ouBLL.getData("getAllStatus", oUV);
            ddlComplianceStatus.DataBind();
            ddlComplianceStatus.Items.Insert(0, new ListItem("--Select--", ""));
        }

        void FillStatus()
        {
            ddlStatus1.Items.Clear();
            ddlStatus1.DataSource = refBL.getRefCodeDetails("Compliance Review - Status Update", mstrConnectionString);
            ddlStatus1.DataBind();
            ddlStatus1.Items.Insert(0, new ListItem("--Select--", ""));

            ddlStatus2.Items.Clear();
            ddlStatus2.DataSource = refBL.getRefCodeDetails("Compliance Review - Status Update", mstrConnectionString);
            ddlStatus2.DataBind();
            ddlStatus2.Items.Insert(0, new ListItem("--Select--", ""));
        }

        void FillUniverseMaster()
        {
            ddlUniverseReviewed.Items.Clear();
            ddlUniverseReviewed.DataSource = oBLL.Search_Universe_Master(null, "A", strvalue: " Order by CRUM_UNIVERSE_TO_BE_REVIEWED asc");
            ddlUniverseReviewed.DataBind();
            ddlUniverseReviewed.Items.Insert(0, new ListItem("--Select--", ""));
        }

        void FillReviewerName()
        {
            ddlReviewerName.Items.Clear();
            ddlReviewerName.DataSource = oBLL.Search_Reviewer_Master(0, null, null, null, "A");
            ddlReviewerName.DataBind();
            ddlReviewerName.Items.Insert(0, new ListItem("--Select--", ""));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                searchcompliance();
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

        void searchcompliance()
        {
            int intCCR_ID = 0;
            int intCCR_CRUM_ID = ddlUniverseReviewed.SelectedIndex > 0 ? Convert.ToInt32(ddlUniverseReviewed.SelectedItem.Value) : 0;
            int intCCR_CRM_ID = ddlReviewerName.SelectedIndex > 0 ? Convert.ToInt32(ddlReviewerName.SelectedItem.Value) : 0;
            string dtCCR_TENTATIVE_START_DATE = !string.IsNullOrEmpty(txtTentativeStartDT.Text) ? txtTentativeStartDT.Text : null;
            string dtCCR_TENTATIVE_END_DATE = !string.IsNullOrEmpty(txtTentativeEndDT.Text) ? txtTentativeEndDT.Text : null;
            string strCCR_CREATOR = Page.User.Identity.Name;
            string strType = hfType.Value;
            string strStatus = ddlComplianceStatus.SelectedIndex > 0 ? ddlComplianceStatus.SelectedItem.Value : "";
            DataTable dt = new DataTable();
            dt = oBLL.Search_ComplianceReview(intCCR_ID, intCCR_CRUM_ID, intCCR_CRM_ID, dtCCR_TENTATIVE_START_DATE, dtCCR_TENTATIVE_END_DATE, strCCR_CREATOR, strType, strStatus:strStatus);
            if (dt.Rows.Count > 0)
            {
                gvComplianceReview.DataSource = dt;
            }
            else
            {
                gvComplianceReview.DataSource = null;
            }
            gvComplianceReview.EditIndex = -1;
            gvComplianceReview.DataBind();
        }
        protected void gvComplianceReview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblstatus = (Label)e.Row.FindControl("lblStatus");
                LinkButton lnk = (LinkButton)e.Row.FindControl("lnkSetStatus");
                if (lblstatus.Text.ToLower() == "cr_a")
                {
                    lnk.Visible = true;
                }
                else
                {
                    lnk.Visible = false;
                }
            }
        }
        protected void cvEndDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        protected void gvComplianceReview_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strId = gvComplianceReview.SelectedValue.ToString();

                if (hfSelectedOperation.Value == "Edit")
                {
                    Response.Redirect(Global.site_url("Projects/ComplianceReview/AddComplianceReview.aspx?Source=submitRiskReview&Id=" + strId));
                }
                else if (hfSelectedOperation.Value == "View")
                {
                    Response.Redirect(Global.site_url("Projects/ComplianceReview/ViewComplianceReview.aspx?Source=" + hfType.Value + "&Id=" + strId));
                }
                else if (hfSelectedOperation.Value == "Issue")
                {
                    Response.Redirect(Global.site_url("Projects/ComplianceReview/IssueTracker.aspx?Id=" + strId));
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnSubmit_ServerClick(object sender, EventArgs e)
        {
            string strCreateBy = "", strStatus = "", strStatusRemarks = "", strMsg = "";
            int intId = 0;
            bool res = false;
            hfClickCounter.Value = "1";

            try
            {
                res = int.TryParse(hfRiskReviewId.Value, out intId);

                strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                strStatus = hfModalStatus.Value;
                strStatusRemarks = hfModalStatusRem.Value;

                if (strStatus.Equals(""))
                    strMsg += " - Please select status to update.\\r\\n";

                if (strStatusRemarks.Equals(""))
                    strMsg += " - Please enter status remarks.";

                if (!strMsg.Equals(""))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "pop", "alert('" + strMsg + "');", true);
                    return;
                }

                oBLL.submitForOperation(0, "CR_H", "UpdateRecordStatus", strStatus, intRiskReviewDraftId: Convert.ToInt32(hfRiskReviewId.Value), strValue1: strStatusRemarks);

                ClientScript.RegisterStartupScript(this.GetType(), "pop", "alert('Status have been updated successfully.'); document.getElementById('ctl00_ContentPlaceHolder1_btnSearch').click();", true);
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            int uniqueRowId = 0;
            string strChecklistTable = "", strHtmlTableChecklistDetsRows = "";
            DataRow drChecklistDets;
            int intCCR_ID = 0;
            int intCCR_CRUM_ID = ddlUniverseReviewed.SelectedIndex > 0 ? Convert.ToInt32(ddlUniverseReviewed.SelectedItem.Value) : 0;
            int intCCR_CRM_ID = ddlReviewerName.SelectedIndex > 0 ? Convert.ToInt32(ddlReviewerName.SelectedItem.Value) : 0;
            string dtCCR_TENTATIVE_START_DATE = !string.IsNullOrEmpty(txtTentativeStartDT.Text) ? txtTentativeStartDT.Text : null;
            string dtCCR_TENTATIVE_END_DATE = !string.IsNullOrEmpty(txtTentativeEndDT.Text) ? txtTentativeEndDT.Text : null;
            string strCCR_CREATOR = Page.User.Identity.Name;
            string strType = hfType.Value;
            string strStatus = ddlComplianceStatus.SelectedIndex > 0 ? ddlComplianceStatus.SelectedItem.Value : "";
            DataTable dt = new DataTable();
            dt = oBLL.Search_ComplianceReview(intCCR_ID, intCCR_CRUM_ID, intCCR_CRM_ID, dtCCR_TENTATIVE_START_DATE, dtCCR_TENTATIVE_END_DATE, strCCR_CREATOR, strType, strStatus: strStatus);
            if (dt.Rows.Count > 0)
            {
            }

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
                         " Compliance Review No." +
                         " </th> " +
                         " <th class='tabhead' align='center'> " +
                         " Universe to be Reviewed" +
                         " </th> " +
                         " <th class='tabhead' align='center'> " +
                         " Reviewer Name" +
                         " </th> " +
                         " <th class='tabhead' align='center'> " +
                         " Review Type" +
                         " </th> " +
                         " <th class='tabhead' align='center'> " +
                         " Tentative Start Date" +
                         " </th> " +
                         " <th class='tabhead' align='center'> " +
                         " Tentative End Date" +
                         " </th> " +
                         " <th class='tabhead' align='center'> " +
                         " Status" +
                         " </th> " +
                         " <th class='tabhead' align='center'> " +
                         " Review Scope" +
                         " </th> " +
                         " <th class='tabhead' align='center'> " +
                         " Remarks" +
                         " </th> " +
                         " </tr> " +
                         " </thead> ";




            int intChecklistDetsCnt = dt.Rows.Count;
            for (int intCnt = 0; intCnt < intChecklistDetsCnt; intCnt++)
            {
                uniqueRowId = uniqueRowId + 1;
                drChecklistDets = dt.Rows[intCnt];

                string tentativestartdate = string.IsNullOrEmpty(drChecklistDets["CCR_TENTATIVE_START_DATE"].ToString()) ? "" : Convert.ToDateTime(drChecklistDets["CCR_TENTATIVE_START_DATE"].ToString()).ToString("dd-MMM-yyyy");
                string tentativeenddate = string.IsNullOrEmpty(drChecklistDets["CCR_TENTATIVE_END_DATE"].ToString()) ? "" : Convert.ToDateTime(drChecklistDets["CCR_TENTATIVE_END_DATE"].ToString()).ToString("dd-MMM-yyyy");

                strHtmlTableChecklistDetsRows = strHtmlTableChecklistDetsRows + "<tr>" +
                "<td>" + uniqueRowId + "</td>" +
                "<td>" + drChecklistDets["CCR_IDENTIFIER"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CRUM_UNIVERSE_TO_BE_REVIEWED"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CRM_L0_REVIEWER_NAME"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCR_REVIEW_TYPE"].ToString() + "</td>" +
                "<td>" + tentativestartdate + "</td>" +
                "<td>" + tentativeenddate + "</td>" +
                "<td>" + drChecklistDets["SM_DESC"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCR_REVIEW_SCOPE"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCR_REMARKS"].ToString() + "</td>" +
                "</tr>";
            }


            strChecklistTable = strHtmlTable + strHtmlTableChecklistDetsRows + "</table> " +
           "</BODY>" +
           "</HTML>";




            string attachment = "attachment; filename=Compliance Review List.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write(strChecklistTable.ToString());
            Response.End();
        }
    }
}