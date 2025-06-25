using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using Fiction2Fact.Legacy_App_Code.Circulars.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Circulars
{
    public partial class Circulars_MyActionables : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        CircularMasterBLL CircularMasterBLL = new CircularMasterBLL();
        UtilitiesBLL utilityBL = new UtilitiesBLL();
        RefCodesBLL rcBL = new RefCodesBLL();
        //<< Added by ramesh more on 13-Mar-2024 CR_1991
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();
        //>>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataTable dtActionableStatus = new DataTable();
                dtActionableStatus = rcBL.getRefCodeDetails("Actionable Status", mstrConnectionString);

                ddlStatus.DataSource = dtActionableStatus;
                ddlStatus.DataBind();
                ddlStatus.Items.Insert(0, new ListItem("(Select an option)", ""));

                if (Session["CircularActionable"] != null)
                {
                    DataTable dtPrevious;
                    string[] arr = Convert.ToString(Session["CircularActionableFields"]).Split('|');
                    ddlStatus.SelectedValue = arr[0]; txtFromDate.Text = arr[1]; txtToDate.Text = arr[2];
                    gvCircularMaster.DataSource = (DataTable)Session["CircularActionable"];
                    gvCircularMaster.DataBind();
                }

            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            try
            {
                string strStatus = ddlStatus.SelectedValue;
                string FromDate = txtFromDate.Text;
                string ToDate = txtToDate.Text;
                string strUser = Authentication.GetUserID(Page.User.Identity.Name);
                DataTable dtCircular = new DataTable();

                dtCircular = CircularMasterBLL.SearchCircularActionable(0, "", FromDate, ToDate, strStatus, "", strUser, mstrConnectionString);
                Session["CircularActionableFields"] = strStatus + "|" + FromDate + "|" + ToDate;
                Session["CircularActionable"] = dtCircular;
                gvCircularMaster.DataSource = dtCircular;
                gvCircularMaster.DataBind();
                if ((this.gvCircularMaster.Rows.Count == 0))
                {
                    this.lblMsg.Text = "No Records found satisfying the criteria.";
                    this.lblMsg.Visible = true;
                    btnExportToExcel.Visible = false;
                }
                else
                {
                    this.lblMsg.Text = String.Empty;
                    this.lblMsg.Visible = false;
                    btnExportToExcel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                this.lblMsg.Text = ex.Message;
                this.lblMsg.Visible = true;
            }
        }

        protected void lnkReset_Click(object sender, System.EventArgs e)
        {
            Session["CircularActionableFields"] = null;
            Session["CircularActionable"] = null; 
            Response.Redirect(Request.RawUrl);
        }
        protected void gvCircularMaster_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string strCircularId;
            try
            {
                strCircularId = gvCircularMaster.SelectedValue.ToString();
                hfSelectedRecord.Value = strCircularId;
                Label lblActionableId = (Label)(gvCircularMaster.SelectedRow.FindControl("lblActionableId"));
                HiddenField hfStatus = (HiddenField)(gvCircularMaster.SelectedRow.FindControl("hfStatus"));
                //<< Modified by ramesh more on 14-Mar-2024 CR_1991
                if (hfSelectedOperation.Value.Equals("Edit"))
                {
                    Response.Redirect("ActionableUpdates.aspx?Source=MyAct&CircularId=" + encdec.Encrypt(strCircularId) +
                        "&ActionableId=" + encdec.Encrypt(lblActionableId.Text) + "&Status=" + hfStatus.Value);
                }
                //>>
            }
            catch (Exception ex)
            {
                this.lblMsg.Text = ex.Message;
                this.lblMsg.Visible = true;
            }
        }
        protected void cvTodate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void gvCircularMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCircularMaster.PageIndex = e.NewPageIndex;
            gvCircularMaster.DataSource = (DataTable)(Session["CircularActionable"]);
            gvCircularMaster.DataBind();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

            gvCircularMaster.AllowPaging = false;
            gvCircularMaster.AllowSorting = false;
            gvCircularMaster.Columns[0].Visible = false;
            gvCircularMaster.Columns[15].Visible = false;
            gvCircularMaster.DataSource = (DataTable)Session["CircularActionable"];
            gvCircularMaster.DataBind();
            CommonCodes.PrepareGridViewForExport(gvCircularMaster);
            string attachment = "attachment; filename=Actionable Details.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvCircularMaster.RenderControl(htw);

            Response.Write(sw.ToString());
            Response.End();
            gvCircularMaster.AllowPaging = true;
            gvCircularMaster.AllowSorting = true;
            gvCircularMaster.Columns[0].Visible = true;
            gvCircularMaster.Columns[15].Visible = true;
            gvCircularMaster.DataBind();
        }
        protected DataTable LoadCircularActionableFileList(object CircularId)
        {
            DataTable dtCircularFiles;
            dtCircularFiles = utilityBL.getDatasetWithCondition("LoadCircularActionableFileList", Convert.ToInt32(CircularId), mstrConnectionString);
            return dtCircularFiles;
        }

        #region
        protected void gvCircularMaster_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (Session["CircularActionable"] != null)
            {
                DataTable dt = (DataTable)Session["CircularActionable"];
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
                gvCircularMaster.DataSource = dvDataView;
                gvCircularMaster.DataBind();
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
                foreach (DataControlField field in gvCircularMaster.Columns)
                {
                    if (field.SortExpression.Equals(strSortExpression))
                    {
                        return gvCircularMaster.Columns.IndexOf(field);
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