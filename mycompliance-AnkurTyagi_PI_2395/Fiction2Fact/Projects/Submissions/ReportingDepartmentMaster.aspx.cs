using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class ReportingDepartmentMaster : System.Web.UI.Page
    {
        UtilitiesBLL utilityBL = new UtilitiesBLL();
        SubmissionMasterBLL osm = new SubmissionMasterBLL();
        string mstrConnectionString;
        DataTable dt = new DataTable();
        Authentication au = new Authentication();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvReportingDepartment.ActiveViewIndex = 0;
                //<<Commented by Ankur Tyagi on 21-Apr-2025 for Project Id : 2395
                //FillDepartmentDDL();
                txtOUserId.Attributes["onchange"] = " return populateUserDetsByCode('RD','0')";
            }
        }
        #region Search Part
        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            try
            {
                writeError("");
                hfLevel.Value = "";
                hfLevelId.Value = "";
                hfSRD_ID.Value = "";

                dt = osm.getReportingDepartmentDetails(txtDepartment.Text, txtName.Text, ddlLevel.SelectedValue, mstrConnectionString);
                gvReportingDepartment.DataSource = dt;
                gvReportingDepartment.DataBind();
                ViewState["gv_Reporting"] = dt;
                if (dt.Rows.Count > 0)
                {
                    btnExportToExcel.Visible = true;
                }
                else
                {
                    btnExportToExcel.Visible = false;
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        protected void gvReportingDepartment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReportingDepartment.PageIndex = e.NewPageIndex;
            gvReportingDepartment.DataSource = (DataTable)ViewState["gv_Reporting"];
            gvReportingDepartment.DataBind();
        }
        protected void gvReportingDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //<<Added by Ankur Tyagi on 21-Apr-2025 for Project Id : 2395
                FillDepartmentDDL_ForEdit();
                //>>
                ddlSType.Enabled = true;
                //Active Index to Add/Edit Screen
                mvReportingDepartment.ActiveViewIndex = 1;
                ClearControls();
                
                //To fill Reportint Department
                dt = osm.getReportingDepartment(Convert.ToInt32(hfSRD_ID.Value), "", "", "", mstrConnectionString);
                txtDepartmentName.Text = dt.Rows[0]["SRD_NAME"].ToString();
                ddlDepartmentStatus.SelectedValue = dt.Rows[0]["SRD_STATUS"].ToString();

                //Checking for Level
                //If Level = 0 then fetching data from SRDOM - Submission Reporting Department Owner Master
                if (hfLevel.Value == "0" && hfLevelId.Value != "0")
                {
                    ddlOLevel.SelectedValue = "0";

                    dt = osm.getReportingUsers(Convert.ToInt32(hfLevelId.Value), 0, "", "", "", "", "", mstrConnectionString);
                    ViewState["DDL"] = dt.Rows[0]["SRDOM_SRD_ID"].ToString();
                    ddlODepartment.SelectedValue = dt.Rows[0]["SRDOM_SRD_ID"].ToString();
                    txtOUserId.Text = dt.Rows[0]["SRDOM_EMP_ID"].ToString();
                    txtOName.Text = dt.Rows[0]["SRDOM_EMP_NAME"].ToString();
                    txtOEmailId.Text = dt.Rows[0]["SRDOM_EMAILID"].ToString();
                    ddlOStatus.SelectedValue = dt.Rows[0]["SRDOM_STATUS"].ToString();
                }
                //If Level = 1,2 then fetching data from SE - Submission Escalation
                else if (hfLevel.Value == "1" || hfLevel.Value == "2" && hfLevelId.Value != "0")
                {
                    ddlOLevel.SelectedValue = hfLevel.Value;

                    dt = osm.getReportingEscalations(Convert.ToInt32(hfLevelId.Value), 0, "", "", "", "", "", "", mstrConnectionString);
                    ViewState["DDL"] = dt.Rows[0]["SE_SRD_ID"].ToString();
                    ddlODepartment.SelectedValue = dt.Rows[0]["SE_SRD_ID"].ToString();
                    txtOUserId.Text = dt.Rows[0]["SE_EMPLOYEE_ID"].ToString();
                    txtOName.Text = dt.Rows[0]["SE_FIRST_NAME"].ToString();
                    txtOEmailId.Text = dt.Rows[0]["SE_EMAIL_ID"].ToString();
                    ddlOStatus.SelectedValue = dt.Rows[0]["SE_STATUS"].ToString();
                }
                ddlSType.SelectedIndex = 0;
                ddlSType_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        #endregion
        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            //<<Added by Ankur Tyagi on 21-Apr-2025 for Project Id : 2395
            FillDepartmentDDL();
            hfLevelId.Value = String.Empty;
            //>>
            writeError("");
            mvReportingDepartment.ActiveViewIndex = 1;
            ddlSType.SelectedIndex = 0;
            ddlSType_SelectedIndexChanged(sender, e);
            ClearControls();
        }
        #region Add/Edit Part
        protected void btnDepartmentSave_Click(object sender, EventArgs e)
        {
            try
            {
                //To Save in SRD - Submission Reporting Department
                if (string.IsNullOrEmpty(hfSRD_ID.Value))
                    dt = osm.getReportingDepartment(0, txtDepartmentName.Text, "", "", "Y", mstrConnectionString);
                if (dt.Rows.Count > 0)
                {
                    writeError("Duplicate Department name found. Please enter different department name.");
                    return;
                }

                string loggeduser = Authentication.GetUserID(Authentication.GetUserID(Page.User.Identity.Name));
                int ret = osm.saveReportingDepartment(hfSRD_ID.Value == "" ? 0 : Convert.ToInt32(hfSRD_ID.Value), txtDepartmentName.Text, ddlDepartmentStatus.SelectedValue
                    , loggeduser, txtDeptReasonForEdit.Text, mstrConnectionString);
                writeError("Record save successfully.");

                if (hfLevel.Value != "")
                {
                    dt = osm.getReportingDepartment(Convert.ToInt32(hfSRD_ID.Value), "", "", "", mstrConnectionString);
                    txtDepartmentName.Text = dt.Rows[0]["SRD_NAME"].ToString();
                    ddlDepartmentStatus.SelectedValue = dt.Rows[0]["SRD_STATUS"].ToString();
                }
                txtDepartmentName.Text = string.Empty;
                ddlDepartmentStatus.SelectedIndex = 0;
                FillDepartmentDDL();
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        protected void lnkUsrSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Save as per the hidden field value
                // null or empty means fresh save
                if (hfLevel.Value == "")
                {
                    string loggeduser = Authentication.GetUserID(Authentication.GetUserID(Page.User.Identity.Name));
                    //To Save in SRDOM - Submission Reporting Department Owner Master
                    if (ddlOLevel.SelectedValue == "0")
                    {
                        if (string.IsNullOrEmpty(hfLevelId.Value))
                            dt = osm.getReportingUsers(0, Convert.ToInt32(ddlODepartment.SelectedValue), "", "", txtOUserId.Text, "", "", mstrConnectionString, "Y");
                        if (dt.Rows.Count > 0)
                        {
                            writeError("Duplicate user id found with this department. Please enter different user id.");
                            return;
                        }

                        int ret = osm.saveReportingUsers(hfLevelId.Value == "" ? 0 : Convert.ToInt32(hfLevelId.Value), Convert.ToInt32(ddlODepartment.SelectedValue)
                            , txtOName.Text, txtOEmailId.Text, txtOUserId.Text, ddlOStatus.SelectedValue, loggeduser, txtOReasonForEdit.Text, mstrConnectionString);
                        writeError("Submitted successfully.");
                    }
                    //To Save in SE - Submission Escalation
                    else if (ddlOLevel.SelectedValue == "1" || ddlOLevel.SelectedValue == "2")
                    {
                        if (string.IsNullOrEmpty(hfLevelId.Value))
                            dt = osm.getReportingEscalations(0, Convert.ToInt32(ddlODepartment.SelectedValue), "", "", txtOUserId.Text, "", "", "", mstrConnectionString, "Y");
                        if (dt.Rows.Count > 0)
                        {
                            writeError("Duplicate user id found with this department. Please enter different user id.");
                            return;
                        }

                        int ret = osm.saveReportingEscalations(hfLevelId.Value == "" ? 0 : Convert.ToInt32(hfLevelId.Value), Convert.ToInt32(ddlODepartment.SelectedValue)
                            , txtOName.Text, txtOEmailId.Text, txtOUserId.Text, ddlOStatus.SelectedValue, ddlOLevel.SelectedValue, loggeduser, txtOReasonForEdit.Text, mstrConnectionString);
                        writeError("Submitted successfully.");

                        if (hfLevel.Value != "")
                        {
                            dt = osm.getReportingEscalations(Convert.ToInt32(hfLevelId.Value), 0, "", "", "", "", "", "", mstrConnectionString);
                            ddlODepartment.SelectedValue = dt.Rows[0]["SE_SRD_ID"].ToString();
                            txtOUserId.Text = dt.Rows[0]["SE_EMPLOYEE_ID"].ToString();
                            txtOName.Text = dt.Rows[0]["SE_FIRST_NAME"].ToString();
                            txtOEmailId.Text = dt.Rows[0]["SE_EMAIL_ID"].ToString();
                            ddlOStatus.SelectedValue = dt.Rows[0]["SE_STATUS"].ToString();
                        }
                    }
                }
                //Edit as per the hidden field value hfLevel in which level is filled on edit click
                else
                {
                    string loggeduser = Authentication.GetUserID(Authentication.GetUserID(Page.User.Identity.Name));
                    //To Save in SRDOM - Submission Reporting Department Owner Master
                    if (hfLevel.Value == "0")
                    {
                        if (ddlODepartment.SelectedValue == "" || ddlODepartment.SelectedValue == null)
                        {
                            writeError("Please select department.");
                            return;
                        }

                        int ret = osm.saveReportingUsers(hfLevelId.Value == "" ? 0 : Convert.ToInt32(hfLevelId.Value), Convert.ToInt32(ddlODepartment.SelectedValue)
                            , txtOName.Text, txtOEmailId.Text, txtOUserId.Text, ddlOStatus.SelectedValue, loggeduser, txtOReasonForEdit.Text, mstrConnectionString);
                        writeError("Submitted successfully.");
                    }
                    //To Save in SE - Submission Escalation
                    else if (hfLevel.Value == "1" || hfLevel.Value == "2")
                    {
                        if (ddlODepartment.SelectedValue == "" || ddlODepartment.SelectedValue == null)
                        {
                            writeError("Please select department.");
                            return;
                        }

                        int ret = osm.saveReportingEscalations(hfLevelId.Value == "" ? 0 : Convert.ToInt32(hfLevelId.Value), Convert.ToInt32(ddlODepartment.SelectedValue)
                            , txtOName.Text, txtOEmailId.Text, txtOUserId.Text, ddlOStatus.SelectedValue, ddlOLevel.SelectedValue, loggeduser, txtOReasonForEdit.Text, mstrConnectionString);
                        writeError("Submitted successfully.");
                    }
                }
                mvReportingDepartment.ActiveViewIndex = 0;
                gvReportingDepartment.DataSource = null;
                gvReportingDepartment.DataBind();
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        protected void lnkReset_Click(object sender, EventArgs e)
        {
            //writeError("");
            //mvReportingDepartment.ActiveViewIndex = 0;
            //ClearControls();
            //gvReportingDepartment.DataSource = null;
            //gvReportingDepartment.DataBind();
            //btnExportToExcel.Visible = false;
            Response.Redirect(Request.RawUrl);
        }
        protected void lnkReset1_Click(object sender, EventArgs e)
        {
            writeError("");
            mvReportingDepartment.ActiveViewIndex = 0;
            ClearControls();
            gvReportingDepartment.DataSource = null;
            gvReportingDepartment.DataBind();
            btnExportToExcel.Visible = false;
        }
        #endregion
        private void writeError(string strError)
        {
            if (strError == "")
            {
                lblMsg.Text = "";
                lblMsg.Visible = false;
            }
            else
            {
                lblMsg.Text = strError;
                lblMsg.Visible = true;

            }
        }
        protected void ddlSType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSType.SelectedValue == "D")
                {
                    pnlDepartment.Visible = true;
                    pnlUsers.Visible = false;
                }
                else if (ddlSType.SelectedValue == "U")
                {
                    pnlUsers.Visible = true;
                    pnlDepartment.Visible = false;
                }
                else
                {
                    pnlDepartment.Visible = false;
                    pnlUsers.Visible = false;
                }

                if (ddlSType.SelectedValue == "D")
                {
                    if (hfLevelId.Value == "")
                    {
                        divDeptReason.Visible = false;
                        rfvDeptReason.Enabled = false;
                    }
                    else
                    {
                        divDeptReason.Visible = true;
                        rfvDeptReason.Enabled = true;
                    }
                }
                else if (ddlSType.SelectedValue == "U")
                {
                    if (hfLevelId.Value == "" || hfLevelId.Value == "0")
                    {
                        divUsrReason.Visible = false;
                        rfvOReasonForEdit.Enabled = false;
                        divULevel.Visible = true;
                        ddlOLevel.Enabled = true;
                        ddlODepartment.Enabled = true;
                    }
                    else
                    {
                        divUsrReason.Visible = true;
                        rfvOReasonForEdit.Enabled = true;
                        divULevel.Visible = true;
                        ddlOLevel.Enabled = false;
                        ddlODepartment.Enabled = false;
                    }
                }
                //<<Commented by Ankur Tyagi on 21-Apr-2025 for Project Id : 2395
                //if (!string.IsNullOrEmpty(Convert.ToString(ViewState["DDL"])))
                //{
                //    FillDepartmentDDL();
                //    ddlODepartment.SelectedValue = Convert.ToString(ViewState["DDL"]);
                //}
                //else
                //{
                //    FillDepartmentDDL();
                //}
                //>>
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        void ClearControls()
        {
            txtName.Text = String.Empty;
            txtDepartment.Text = String.Empty;
            ddlLevel.SelectedIndex = 0;

            txtDepartmentName.Text = String.Empty;
            ddlDepartmentStatus.SelectedIndex = 0;
            if (ddlODepartment.SelectedIndex != -1)
            {
                ddlODepartment.SelectedIndex = 0;
            }
            txtOUserId.Text = String.Empty;
            txtOName.Text = String.Empty;
            txtOEmailId.Text = String.Empty;
            ddlOStatus.SelectedIndex = 0;
            ddlOLevel.SelectedIndex = 0;
            txtDeptReasonForEdit.Text = String.Empty;
            txtOReasonForEdit.Text = String.Empty;
        }
        void FillDepartmentDDL()
        {
            ddlODepartment.Items.Clear();
            //<<Modified by Ankur Tyagi on 21-Apr-2025 for Project Id : 2395
            //DataSet dsREPORTINGDEPT_EDIT = utilityBL.getDataset("REPORTINGDEPT_EDIT", mstrConnectionString);
            DataSet dsREPORTINGDEPT_EDIT = utilityBL.getDataset("REPORTINGDEPT", mstrConnectionString);
            //>>
            CommonCodes.SetDropDownDataSourceForEdit(ddlODepartment, dsREPORTINGDEPT_EDIT.Tables[0], "SRD_STATUS");
        }
        //<<Added by Ankur Tyagi on 21-Apr-2025 for Project Id : 2395
        void FillDepartmentDDL_ForEdit()
        {
            ddlODepartment.Items.Clear();
            DataSet dsREPORTINGDEPT_EDIT = utilityBL.getDataset("REPORTINGDEPT_EDIT", mstrConnectionString);
            CommonCodes.SetDropDownDataSourceForEdit(ddlODepartment, dsREPORTINGDEPT_EDIT.Tables[0], "SRD_STATUS");
        }
        //>>
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvReportingDepartment.AllowPaging = false;
            gvReportingDepartment.AllowSorting = false;
            gvReportingDepartment.DataSource = (DataTable)(ViewState["gv_Reporting"]);
            gvReportingDepartment.DataBind();
            F2FExcelExport.F2FExportGridViewToExcel(gvReportingDepartment, "ReportingDepartmentDetails", new int[] { 1 });
            return;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        #region
        protected void gvReportingDepartment_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (ViewState["gv_Reporting"] != null)
            {
                DataTable dt = (DataTable)ViewState["gv_Reporting"];
                DataView dvDataView = new DataView(dt);
                string strSortExpression = "";

                //ViewState["_SortDirection_"]
                if (ViewState["_SortExpression_"] != null)
                {
                    strSortExpression = ViewState["_SortExpression_"].ToString();
                }

                if (ViewState["_SortDirection_"] == null)
                {
                    dvDataView.Sort = (e.SortExpression + (" " + "ASC"));
                    ViewState["_SortDirection_"] = "ASC";
                }
                else
                {
                    //ONLY IF THE USER HAS CLICKED ON THE SAME COLUMN AGAIN, SHOULD IT BE SORTED IN THE REVERSE ORDER.
                    //IF ANOTHER COLUMN IS SELECTED, IT SHOULD AGAIN BE SORTED IN ASCENDING ORDER. 
                    if (strSortExpression.Equals(e.SortExpression))
                    {
                        if (ViewState["_SortDirection_"].ToString().Equals("ASC"))
                        {
                            dvDataView.Sort = (e.SortExpression + (" " + "DESC"));
                            ViewState["_SortDirection_"] = "DESC";
                        }
                        else if (ViewState["_SortDirection_"].ToString().Equals("DESC"))
                        {
                            dvDataView.Sort = (e.SortExpression + (" " + "ASC"));
                            ViewState["_SortDirection_"] = "ASC";
                        }
                    }
                    else
                    {
                        dvDataView.Sort = (e.SortExpression + (" " + "ASC"));
                        ViewState["_SortDirection_"] = "ASC";
                    }


                }
                ViewState["_SortExpression_"] = e.SortExpression;
                gvReportingDepartment.DataSource = dvDataView;
                gvReportingDepartment.DataBind();
            }
        }
        void AddSortImage(GridViewRow headerRow, string strAction, int sortColumnIndex)
        {
            // Create the sorting image based on the sort direction.
            Image sortImage = new Image();
            if (strAction.Equals("ASC"))
            {
                sortImage.ImageUrl = Fiction2Fact.Global.site_url("Content/images/legacy/view_sort_ascending.png");
                sortImage.AlternateText = "Ascending Order";
            }
            else if (strAction.Equals("DESC"))
            {
                sortImage.ImageUrl = Fiction2Fact.Global.site_url("Content/images/legacy/view_sort_descending.png");
                sortImage.AlternateText = "Descending Order";
            }
            headerRow.Cells[sortColumnIndex].Controls.Add(sortImage);
        }
        protected void OnRowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                int sortColumnIndex = GetSortColumnIndex();

                if (sortColumnIndex != -1)
                {
                    if (ViewState["_SortDirection_"] != null)
                        AddSortImage(e.Row, ViewState["_SortDirection_"].ToString(), sortColumnIndex);
                }
            }
        }
        private int GetSortColumnIndex()
        {
            // Iterate through the Columns collection to determine the index
            // of the column being sorted.

            string strSortExpression = "";

            if (ViewState["_SortExpression_"] != null)
                strSortExpression = ViewState["_SortExpression_"].ToString();

            if (!strSortExpression.Equals(""))
            {
                foreach (DataControlField field in gvReportingDepartment.Columns)
                {
                    if (field.SortExpression.Equals(strSortExpression))
                    {
                        return gvReportingDepartment.Columns.IndexOf(field);
                    }
                }
            }
            return -1;
        }

        #endregion
    }
}