using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.App_Code;
using System.IO;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class TrackingDepartmentMaster : System.Web.UI.Page
    {
        UtilitiesBLL utilityBL = new UtilitiesBLL();
        SubmissionMasterBLL osm = new SubmissionMasterBLL();
        string mstrConnectionString;
        DataTable dt = new DataTable();
        Authentication au = new Authentication();
        CommonMethods cm = new CommonMethods();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvReportingDepartment.ActiveViewIndex = 0;
                //<<Commented by Ankur Tyagi on 21-Apr-2025 for Project Id : 2395
                //FillTrackingDepartmentDDL();
                //>>
                txtOUserId.Attributes["onchange"] = " return populateUserDetsByCode('TD','0')";
            }
        }
        #region Search Part
        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            try
            {
                writeError("");
                hfSTM_ID.Value = "";
                hfEM_ID.Value = "";
                hfESM_ID.Value = "";

                dt = osm.getTrackingDepartmentDetails(txtDepartment.Text, txtName.Text, txtUserId.Text, mstrConnectionString);
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
                FillTrackingDepartmentDDL_ForEdit();
                //>>
                ClearControls();
                ddlSType.Enabled = true;
                //Active Index to Add/Edit Screen
                writeError("");
                mvReportingDepartment.ActiveViewIndex = 1;
                
                //To fill Reporting Department
                dt = osm.getTrackingDepartment(Convert.ToInt32(hfSTM_ID.Value), "", "", "", mstrConnectionString);
                txtDepartmentName.Text = dt.Rows[0]["STM_TYPE"].ToString();
                ddlDepartmentStatus.SelectedValue = dt.Rows[0]["STM_STATUS"].ToString();

                if (hfEM_ID.Value != "0")
                {
                    dt = osm.getEmployeeMaster(Convert.ToInt32(hfEM_ID.Value), Convert.ToInt32(hfSTM_ID.Value), "", "", "", "", "", mstrConnectionString);
                    ViewState["DDL"] = dt.Rows[0]["ESM_STM_ID"].ToString();
                    ddlODepartment.SelectedValue = dt.Rows[0]["ESM_STM_ID"].ToString();
                    txtOUserId.Text = dt.Rows[0]["EM_USERNAME"].ToString();
                    txtOName.Text = dt.Rows[0]["EM_EMPNAME"].ToString();
                    txtOEmailId.Text = dt.Rows[0]["EM_EMAIL"].ToString();
                    ddlOStatus.SelectedValue = dt.Rows[0]["EM_STATUS"].ToString();
                    ddlOLevel.SelectedValue = dt.Rows[0]["ESM_LEVEL"].ToString();
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
            FillTrackingDepartmentDDL();
            hfEM_ID.Value = String.Empty;
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
            string loggeduser = Authentication.GetUserID(Authentication.GetUserID(Page.User.Identity.Name));
            int ret = osm.saveTrackingDepartment(hfSTM_ID.Value == "" ? 0 : Convert.ToInt32(hfSTM_ID.Value), txtDepartmentName.Text, ddlDepartmentStatus.SelectedValue
                , loggeduser, txtDeptReasonForEdit.Text, mstrConnectionString);
            writeError("Submitted successfully.");

            if (hfSTM_ID.Value != "")
            {
                dt = osm.getTrackingDepartment(Convert.ToInt32(hfSTM_ID.Value), "", "", "", mstrConnectionString);
                txtDepartmentName.Text = dt.Rows[0]["STM_TYPE"].ToString();
                ddlDepartmentStatus.SelectedValue = dt.Rows[0]["STM_STATUS"].ToString();
            }
            txtDepartmentName.Text = string.Empty;
            ddlDepartmentStatus.SelectedIndex = 0;
            FillTrackingDepartmentDDL();
        }
        protected void lnkUsrSave_Click(object sender, EventArgs e)
        {
            //Save as per the hidden field value
            // null or empty means fresh save
            if (hfSTM_ID.Value == "")
            {
                dt = osm.getEmployeeMaster(0, Convert.ToInt32(ddlODepartment.SelectedValue), "", "", txtOUserId.Text, "", "", mstrConnectionString);
                if (dt.Rows.Count > 0)
                {
                    writeError("User id already exists with respect to this tracking department.");
                    return;
                }

                string loggeduser = Authentication.GetUserID(Authentication.GetUserID(Page.User.Identity.Name));
                if (hfEM_ID.Value == "")
                {
                    int ret = osm.saveEmployeeMaster(hfEM_ID.Value == "" ? 0 : Convert.ToInt32(hfEM_ID.Value), cm.getSanitizedString(txtOName.Text),
                        cm.getSanitizedString(txtOEmailId.Text), cm.getSanitizedString(txtOUserId.Text), ddlOStatus.SelectedValue, loggeduser, cm.getSanitizedString(txtOReasonForEdit.Text), mstrConnectionString,
                        ddlODepartment.SelectedValue == "" ? 0 : Convert.ToInt32(ddlODepartment.SelectedValue),
                        ddlOLevel.SelectedValue == "" ? 0 : Convert.ToInt32(ddlOLevel.SelectedValue));
                }

                writeError("Record save successfully.");
            }
            //Edit as per the hidden field value hfLevel in which level is filled on edit click
            else
            {
                if (ddlODepartment.SelectedValue != hfSTM_ID.Value)
                {
                    dt = osm.getEmployeeMaster(0, Convert.ToInt32(ddlODepartment.SelectedValue), "", "", txtOUserId.Text, "", "", mstrConnectionString);
                    if (dt.Rows.Count > 0)
                    {
                        writeError("User id already exists with respect to this tracking department.");
                        return;
                    }
                }

                string loggeduser = Authentication.GetUserID(Authentication.GetUserID(Page.User.Identity.Name));

                if (hfSTM_ID.Value != ddlODepartment.SelectedValue && hfESM_ID.Value != "")
                {
                    int ret1 = osm.saveTrackingDepartmentEmployeeMapping(
                        hfESM_ID.Value == "" ? 0 : Convert.ToInt32(hfESM_ID.Value),
                        hfEM_ID.Value == "" ? 0 : Convert.ToInt32(hfEM_ID.Value),
                        ddlODepartment.SelectedValue == "" ? 0 : Convert.ToInt32(ddlODepartment.SelectedValue),
                        ddlOLevel.SelectedValue == "" ? 0 : Convert.ToInt32(ddlOLevel.SelectedValue),
                        loggeduser, mstrConnectionString);
                }

                int ret = osm.saveEmployeeMaster(hfEM_ID.Value == "" ? 0 : Convert.ToInt32(hfEM_ID.Value), txtOName.Text,
                    txtOEmailId.Text, txtOUserId.Text, ddlOStatus.SelectedValue, loggeduser, txtOReasonForEdit.Text, mstrConnectionString);
                writeError("Submitted successfully.");
            }
            mvReportingDepartment.ActiveViewIndex = 0;
            gvReportingDepartment.DataSource = null;
            gvReportingDepartment.DataBind();
        }
        protected void lnkReset_Click(object sender, EventArgs e)
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
                if (hfSTM_ID.Value == "")
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
                if (hfEM_ID.Value == "" || hfEM_ID.Value == "0")
                {
                    divUsrReason.Visible = false;
                    rfvOReasonForEdit.Enabled = false;
                    ddlOLevel.Enabled = true;
                    ddlODepartment.Enabled = true;
                }
                else
                {
                    divUsrReason.Visible = true;
                    rfvOReasonForEdit.Enabled = true;
                    ddlOLevel.Enabled = false;
                    ddlODepartment.Enabled = false;
                }
            }
            //<<Commented by Ankur Tyagi on 21-Apr-2025 for Project Id : 2395
            //if (!string.IsNullOrEmpty(Convert.ToString(ViewState["DDL"])))
            //{
            //    FillTrackingDepartmentDDL();
            //    ddlODepartment.SelectedValue = Convert.ToString(ViewState["DDL"]);
            //}
            //else
            //{
            //    FillTrackingDepartmentDDL();
            //}
            //>>
        }
        void ClearControls()
        {
            txtName.Text = String.Empty;
            txtDepartment.Text = String.Empty;
            txtUserId.Text = String.Empty;

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
        void FillTrackingDepartmentDDL()
        {
            ddlODepartment.Items.Clear();
            //<<Modified by Ankur Tyagi on 21-Apr-2025 for Project Id : 2395
            //DataSet dsTYPE_EDIT = utilityBL.getDataset("TYPE_EDIT", mstrConnectionString);
            DataSet dsTYPE_EDIT = utilityBL.getDataset("SUBTYPE", mstrConnectionString);
            //>>
            CommonCodes.SetDropDownDataSourceForEdit(ddlODepartment, dsTYPE_EDIT.Tables[0], "STM_STATUS");
        }
        //<<Added by Ankur Tyagi on 21-Apr-2025 for Project Id : 2395
        void FillTrackingDepartmentDDL_ForEdit()
        {
            ddlODepartment.Items.Clear();
            DataSet dsTYPE_EDIT = utilityBL.getDataset("TYPE_EDIT", mstrConnectionString);
            CommonCodes.SetDropDownDataSourceForEdit(ddlODepartment, dsTYPE_EDIT.Tables[0], "STM_STATUS");
        }
        //>>
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvReportingDepartment.AllowPaging = false;
            gvReportingDepartment.AllowSorting = false;
            gvReportingDepartment.DataSource = (DataTable)(ViewState["gv_Reporting"]);
            gvReportingDepartment.DataBind();
            F2FExcelExport.F2FExportGridViewToExcel(gvReportingDepartment, "TrackingDepartmentList", new int[] { 1 });
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