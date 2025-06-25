using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using System.Text.RegularExpressions;
using Fiction2Fact.Legacy_App_Code.Submissions.BLL;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class Submissions_SubmissionEscalations : System.Web.UI.Page
    {
        string strConnectionString = null;
        SubmissionsEscalationsBLL submissionsEscalationsBL = new SubmissionsEscalationsBLL();
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CommonMethods cm = new CommonMethods();
        int intRecordId;
        int intId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getReportingDept();
                bindGrid();
                Session["SaveEscalations"] = Server.UrlEncode(System.DateTime.Now.ToString());
            }
        }

        private void getReportingDept()
        {
            try
            {
                ddlSearchReportDept.DataSource = utilBL.getDataset("REPORTINGDEPT", strConnectionString);
                ddlSearchReportDept.DataBind();

            }
            catch (Exception ex)
            {
                writeError("System exception in getReportingDept(): " + ex.Message);
            }
        }

        private void bindGrid()
        {
            try
            {
                lblMsg.Text = "";
                lbInfo.Text = "";
                DataSet dsRec2 = new DataSet();
                dsRec2 = submissionsEscalationsBL.SearchSubmissionsEscalations("", ddlSearchLevel.SelectedValue,
                    "", strConnectionString, ddlSearchReportDept.SelectedValue);

                Session["Escalation"] = dsRec2.Tables[0];
                gvEscalationMaster.DataSource = dsRec2;
                gvEscalationMaster.DataBind();
            }
            catch (Exception ex)
            {
                writeError("System exception in bindGrid(): " + ex.Message);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["SaveEscalations"] = Session["SaveEscalations"];
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            try
            {
                if (ViewState["SaveEscalations"].ToString() == Session["SaveEscalations"].ToString())
                {
                    SubmissionsEscalationsBLL submissionsEscalationsBL = new SubmissionsEscalationsBLL();
                    string strFirstName = null, strLastName = null, strMiddelName = null, strEmailId = null;
                    string strLevels = null, strCreateBy = null, strEscalationType = null, strReportingDept = null;
                    int intType = 0, intId = 0;

                    strFirstName = cm.getSanitizedString(txtFirstName.Text);
                    strLastName = Convert.ToString(cm.getSanitizedString(txtLastName.Text));
                    strMiddelName = Convert.ToString(cm.getSanitizedString(txtMiddelName.Text));
                    strEmailId = Convert.ToString(cm.getSanitizedString(txtEmailId.Text));
                    strReportingDept = ddlReportDept.SelectedValue;
                    if (!hfSelectedId.Value.Equals(""))
                        intId = Convert.ToInt32(hfSelectedId.Value);
                    else
                        intId = 0;

                    strLevels = Convert.ToString(rblLevels.SelectedValue);
                    strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                    strEscalationType = "";
                    intRecordId = submissionsEscalationsBL.SaveSubmissionsEscalationsMas(intId, strFirstName,
                                            strLastName,
                                            strMiddelName, strEmailId, intType,
                                            strLevels, strCreateBy, strConnectionString,
                                            strEscalationType, strReportingDept);

                    bindGrid();
                    mvMultiView.ActiveViewIndex = 0;
                    if (!intRecordId.Equals(0))
                        writeError("Record saved successfully with Id: " + intRecordId);
                    else
                        writeError("Record updated successfully with Id: " + intId);
                    Session["SaveEscalations"] = Server.UrlEncode(System.DateTime.Now.ToString());
                    clearControls();
                }
                else
                {
                    writeError("Your attempt to refresh the page was blocked as it would lead to duplication of data.");
                }
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }

        private void clearControls()
        {
            txtFirstName.Text = "";
            txtMiddelName.Text = "";
            txtLastName.Text = "";
            txtEmailId.Text = "";
            rblLevels.SelectedIndex = -1;
            ddlReportDept.SelectedIndex = -1;
            hfSelectedId.Value = "";
        }

        private void writeError(string strError)
        {
            lbInfo.Text = strError;
            lbInfo.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            mvMultiView.ActiveViewIndex = 0;
        }

        private void bindData()
        {

            DataTable dtData;
            DataRow drData;
            string strLevel = "";

            mvMultiView.ActiveViewIndex = 1;
            ddlReportDept.DataSource = utilBL.getDataset("REPORTINGDEPT", strConnectionString);
            ddlReportDept.DataBind();

            dtData = utilBL.getDatasetWithCondition("SubEscById", Convert.ToInt32(hfSelectedId.Value), strConnectionString);

            if (dtData.Rows.Count > 0)
            {
                drData = dtData.Rows[0];
                ddlReportDept.SelectedValue = drData["SE_SRD_ID"].ToString();
                txtFirstName.Text = drData["SE_FIRST_NAME"].ToString();
                txtMiddelName.Text = drData["SE_MIDDEL_NAME"].ToString();
                txtLastName.Text = drData["SE_LAST_NAME"].ToString();
                txtEmailId.Text = drData["SE_EMAIL_ID"].ToString();

                strLevel = drData["SE_LEVEL"].ToString();
                if (!strLevel.Equals("") && !strLevel.Equals("0"))
                {
                    rblLevels.SelectedValue = strLevel;
                }
            }
        }

        protected void gvEscalationMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            writeError("");
            hfSelectedId.Value = Convert.ToString(gvEscalationMaster.SelectedValue);
            if (hdfEscalationOperation.Value == "Edit")
            {
                bindData();
            }
            else if (hdfEscalationOperation.Value == "Delete")
            {
                intId = (Convert.ToInt32(hfSelectedId.Value));
                utilBL.getDatasetWithCondition("DeleteSubEsc", intId, strConnectionString);
                bindGrid();
                writeError("Record deleted successfully.");
            }
        }

        protected void btnEditCancel_Click(object sender, EventArgs e)
        {
            mvMultiView.ActiveViewIndex = 0;
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            bindGrid();
        }

        protected void btnAddNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                writeError("");
                clearControls();
                mvMultiView.ActiveViewIndex = 1;
                ListItem li = new ListItem();
                li.Text = "--Select--";
                li.Value = "";

                //<< Added by Vivek on 30-Apr-2020
                ddlReportDept.Items.Clear();
                //>>

                ddlReportDept.DataSource = utilBL.getDataset("REPORTINGDEPT", strConnectionString);
                ddlReportDept.DataBind();
                ddlReportDept.Items.Insert(0, li);
            }
            catch (Exception ex)
            {
                writeError("System exception in btnAddNew_Click : " + ex.Message);
            }
        }

        #region "Gridview Sorting"
        protected void gvEscalationMaster_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (Session["Escalation"] != null)
            {
                DataTable dt = (DataTable)(Session["Escalation"]);
                DataView dvDataView = new DataView(dt);
                string strSortExpression = "";

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
                gvEscalationMaster.DataSource = dvDataView;
                gvEscalationMaster.DataBind();
            }
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
                foreach (DataControlField field in gvEscalationMaster.Columns)
                {
                    if (field.SortExpression.Equals(strSortExpression))
                    {
                        return gvEscalationMaster.Columns.IndexOf(field);
                    }
                }
            }
            return -1;
        }

        void AddSortImage(GridViewRow headerRow, string strAction, int sortColumnIndex)
        {
            // Create the sorting image based on the sort direction.
            Image sortImage = new Image();
            if (strAction.Equals("ASC"))
            {
                sortImage.ImageUrl = "../../Content/images/legacy/view_sort_ascending.png";
                sortImage.AlternateText = "Ascending Order";
            }
            else if (strAction.Equals("DESC"))
            {
                sortImage.ImageUrl = "../../Content/images/legacy/view_sort_descending.png";
                sortImage.AlternateText = "Descending Order";
            }
            headerRow.Cells[sortColumnIndex].Controls.Add(sortImage);
        }
        #endregion


        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvEscalationMaster.AllowPaging = false;
            gvEscalationMaster.AllowSorting = false;
            gvEscalationMaster.Columns[7].Visible = false;
            gvEscalationMaster.Columns[8].Visible = false;
            gvEscalationMaster.DataSource = (DataTable)(Session["Escalation"]);
            gvEscalationMaster.DataBind();
            CommonCodes.PrepareGridViewForExport(gvEscalationMaster);
            string attachment = "attachment; filename=ContractDetails.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvEscalationMaster.RenderControl(htw);

            string html2 = Regex.Replace(sw.ToString(), @"(<input type=""image""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<input class=""checkbox""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<a \/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            Response.Write(html2.ToString());

            //Response.Write(sw.ToString());
            Response.End();
            gvEscalationMaster.Columns[7].Visible = true;
            gvEscalationMaster.Columns[8].Visible = true;
            gvEscalationMaster.AllowPaging = true;
            gvEscalationMaster.AllowSorting = true;
            gvEscalationMaster.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

    }
}