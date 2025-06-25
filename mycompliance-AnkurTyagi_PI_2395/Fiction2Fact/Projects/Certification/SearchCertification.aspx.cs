using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_SearchCertification : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CertificationMasterBL certBL = new CertificationMasterBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ListItem li = new ListItem();
                li.Text = "(Select)";
                li.Value = "";
                ddlSearchDeptName.DataSource = utilBL.getDataset("CERTIFICATEDEPT", strConnectionString);
                ddlSearchDeptName.DataBind();
                ddlSearchDeptName.Items.Insert(0, li);

                ddlQuarter.DataSource = utilBL.getDataset("CERTIFICATEQUARTER", strConnectionString);
                ddlQuarter.DataBind();
                ddlQuarter.Items.Insert(0, li);
            }
            else
            {
            }
        }

        protected void gvChecklist_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvChecklist.PageIndex = e.NewPageIndex;
            gvChecklist.DataSource = Session["dtChecklist"];
            gvChecklist.DataBind();
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            ShowCertifications();
        }
        private void ShowCertifications()
        {
            try
            {
                int intCertId = 0;
                string strDeptName = ddlSearchDeptName.SelectedValue;
                string strQuarter = ddlQuarter.SelectedValue;
                DataTable dt = new DataTable();

                dt = certBL.searchCertificationsChecklist(intCertId, strDeptName, strQuarter, ddlStatus.SelectedValue.ToString(), strConnectionString);
                gvChecklist.DataSource = dt;
                gvChecklist.DataBind();
                Session["dtChecklist"] = dt;

                if ((this.gvChecklist.Rows.Count == 0))
                {
                    this.lblMsg.Text = "No Records found satisfying the criteria.";
                    this.lblMsg.Visible = true;
                    btnExport.Visible = false;
                }
                else
                {
                    this.lblMsg.Text = String.Empty;
                    this.lblMsg.Visible = false;
                    btnExport.Visible = true;
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

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvChecklist.AllowPaging = false;
            gvChecklist.AllowSorting = false;
            gvChecklist.DataSource = (DataTable)Session["dtChecklist"];
            gvChecklist.DataBind();
            CommonCodes.PrepareGridViewForExport(gvChecklist);
            string attachment = "attachment; filename=Certification Checklist.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gvChecklist.RenderControl(htw);

            Response.Write(sw.ToString());
            Response.End();
            gvChecklist.AllowPaging = true;
            gvChecklist.AllowSorting = true;
            gvChecklist.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        //>>

        #region
        protected void gvChecklist_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (Session["dtChecklist"] != null)
            {
                DataTable dt = (DataTable)Session["dtChecklist"];
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
                gvChecklist.DataSource = dvDataView;
                gvChecklist.DataBind();
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
                foreach (DataControlField field in gvChecklist.Columns)
                {
                    if (field.SortExpression.Equals(strSortExpression))
                    {
                        return gvChecklist.Columns.IndexOf(field);
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


    }
}