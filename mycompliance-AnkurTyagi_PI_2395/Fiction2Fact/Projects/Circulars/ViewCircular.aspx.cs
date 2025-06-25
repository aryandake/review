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
using Fiction2Fact.Legacy_App_Code.Circulars.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using System.IO;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Circulars
{
    public partial class Circulars_ViewCircular : System.Web.UI.Page
    {
        //UtilitiesBLL UtilitiesBLL = new UtilitiesBLL();
        CircUtilitiesBLL circUtilBLL = new CircUtilitiesBLL();
        CircularMasterBLL CircularMasterBLL = new CircularMasterBLL();
        RefCodesBLL rcBL = new RefCodesBLL();
        CommonMethods cm = new CommonMethods();
        //<< Added by ramesh more on 13-Mar-2024 CR_1991
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();
        //>>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    mvMultiView.ActiveViewIndex = 0;

                    CommonCodes.SetDropDownDataSource(ddlTypeofDocument, circUtilBLL.GetDataTable("getTypeofCircular", new DBUtilityParameter("CDTM_STATUS", "A"), sOrderBy: "CDTM_TYPE_OF_DOC"));

                    CommonCodes.SetDropDownDataSource(ddlDepartment, circUtilBLL.GetDataTable("DEPT", sOrderBy: "CDM_NAME"));

                    CommonCodes.SetCheckboxDataSource(cbAssociatedKeywordsSearch, circUtilBLL.GetDataTable("AssociatedKeywords", new DBUtilityParameter("CKM_STATUS", "A"), sOrderBy: "CKM_NAME"));
                    CommonCodes.SetCheckboxDataSource(cbToBePlacedBeforeSearch, rcBL.getRefCodeDetails("Circulars - New Circular - To be placed before"));

                    //<<Added by Ankur Tyagi on 08-Apr-2024 for 2025
                    bindGrid();
                    //>>
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        protected void lnkReset_Click(object sender, System.EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
        protected String getFileName(object inputFileName)
        {
            string strFileName = "";

            try
            {
                strFileName = inputFileName.ToString();
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return strFileName.Replace("'", "\\'");
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) return;
            try
            {
                //<<Added by Ankur Tyagi on 08-Apr-2024 for 2025
                bindGrid();
                //>>
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void bindGrid()
        {
            string strAssociatedKeywords = "", strToBePlacedBefore = "", strGlobalSearch = "", strCircIdInCommaSeparatedFormat = "";
            DataSet dsViewCircular = new DataSet();
            string strCurUserEmail = (new Authentication()).GetUserDetails(Page.User.Identity.Name).Split(',')[1];
            string strFilterExpression = String.Empty;
            string strIssuingAuthority = ddlSIssuingauthority.SelectedValue;
            string strDepartment = ddlDepartment.SelectedValue;
            string strTopic = ddlSArea.SelectedValue;
            string strCircularNo = cm.getSanitizedString(txtCircularNo.Text);
            string FromDate = txtFromDate.Text;
            string ToDate = txtToDate.Text;
            string strTypeofDocument = ddlTypeofDocument.SelectedValue;
            string strSubject = cm.getSanitizedString(txtSubject.Text);
            string strGist = cm.getSanitizedString(txtGist.Text);
            string strImplications = cm.getSanitizedString(txtImplications.Text);
            string strStatus = ddlStatus.SelectedValue;

            //<< Added by Amarjeet on 26-Jul-2021
            if (!txtGlobalSearch.Text.Equals(""))
            {
                DataTable dtCircularFiles = CircularMasterBLL.SearchCircularDetails(0, strIssuingAuthority,
                strDepartment, strTopic, strCircularNo, FromDate, ToDate, strTypeofDocument, strSubject,
                strGist, strImplications, "", "View", strCurUserEmail, strAssociatedKeywords,
                strToBePlacedBefore, strGlobalSearch, strCircIdInCommaSeparatedFormat, "Yes", strStatus).Tables[0];

                string strFolderPath = Server.MapPath(ConfigurationManager.AppSettings["CircularFilesFolder"].ToString());

                if (dtCircularFiles != null)
                {
                    for (int i = 0; i < dtCircularFiles.Rows.Count; i++)
                    {
                        DataRow dr = dtCircularFiles.Rows[i];
                        strCircIdInCommaSeparatedFormat = (string.IsNullOrEmpty(strCircIdInCommaSeparatedFormat) ? "" : strCircIdInCommaSeparatedFormat + ",") + cm.searchPDFContent(dr["CM_ID"].ToString(), strFolderPath + "\\" + dr["CF_SERVERFILENAME"].ToString(), txtGlobalSearch.Text);
                    }
                }
            }

            strGlobalSearch = cm.getSanitizedString(txtGlobalSearch.Text);

            strAssociatedKeywords = CommonCodes.getCommaSeparatedValuesFromCheckboxList(cbAssociatedKeywordsSearch);
            strToBePlacedBefore = CommonCodes.getCommaSeparatedValuesFromCheckboxList(cbToBePlacedBeforeSearch);
            //>>

            dsViewCircular = CircularMasterBLL.SearchCircularDetails(0, strIssuingAuthority, strDepartment,
                strTopic, strCircularNo, FromDate, ToDate, strTypeofDocument, strSubject, strGist,
                strImplications, "", "View", strCurUserEmail, strAssociatedKeywords, strToBePlacedBefore,
                strGlobalSearch, strCircIdInCommaSeparatedFormat, "", strStatus);

            ViewState["ExportCirculars"] = dsViewCircular.Tables[0];
            DataView dvDataView = new DataView(dsViewCircular.Tables[0]);
            Session["ViewCircular"] = dvDataView;

            gvCircularMaster.DataSource = dvDataView;
            gvCircularMaster.DataBind();
            if ((this.gvCircularMaster.Rows.Count == 0))
            {
                this.lblInfo.Text = "No Records found satisfying the criteria.";
                this.lblInfo.Visible = true;
                btnExportToExcel.Visible = false;
            }
            else
            {
                this.lblInfo.Text = String.Empty;
                this.lblInfo.Visible = false;
                btnExportToExcel.Visible = true;
            }
        }

        private void writeError(string strError)
        {
            lblInfo.Text = strError;
            lblInfo.Visible = true;
            lblInfo.CssClass = "label";
        }

        protected DataTable LoadCircularFileList(object CircularId)
        {
            DataTable dtCircularFiles = new DataTable();

            try
            {
                //dtCircularFiles = UtilitiesBLL.getDatasetWithCondition("CIRCULARFILES", Convert.ToInt32(CircularId), mstrConnectionString);
                dtCircularFiles = circUtilBLL.GetDataTable("CIRCULARFILES", new DBUtilityParameter("CF_CM_ID", CircularId));
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return dtCircularFiles;
        }

        public string replaceHTMLTagsFromLableTooltip(string strContent)
        {
            try
            {
                strContent = HttpUtility.HtmlDecode(strContent);
                strContent = strContent.Replace("</", "\n</");
                strContent = System.Text.RegularExpressions.Regex.Replace(strContent, "<.*?>", String.Empty);
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return strContent;
        }

        protected void gvCircularMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCircularMaster.PageIndex = e.NewPageIndex;
            gvCircularMaster.DataSource = Session["ViewCircular"];
            gvCircularMaster.DataBind();
        }
        protected void gvCircularMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridViewRow gvr;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    gvr = e.Row;
                    DataRowView drv = e.Row.DataItem as System.Data.DataRowView;

                    if (drv != null)
                    {
                        Label lbId = (Label)(gvr.FindControl("lbId"));
                        int intCMId = Convert.ToInt32(lbId.Text);
                        LinkButton lb1 = (LinkButton)(gvr.FindControl("lnkView"));
                        if (!string.IsNullOrEmpty(Convert.ToString(intCMId)))
                        {
                            lb1.Attributes.Add("onclick", "javascript:return showCircularGist('" + encdec.Encrypt(Convert.ToString(intCMId)) + "')");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void gvCircularMaster_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string strCircularId;
            try
            {
                strCircularId = gvCircularMaster.SelectedValue.ToString();
                hfSelectedRecord.Value = strCircularId;
                if (hfSelectedOperation.Value.Equals("View"))
                {
                    Response.Redirect("ViewCircularDetails.aspx?CircularId=" + encdec.Encrypt(strCircularId));
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        //protected void btnExportToExcel_Click(object sender, EventArgs e)
        //{

        //    gvCircularMaster.AllowPaging = false;
        //    gvCircularMaster.AllowSorting = false;
        //    gvCircularMaster.DataBind();
        //    string attachment = "attachment; filename=Circulars.xls";

        //    Response.ClearContent();

        //    Response.AddHeader("content-disposition", attachment);

        //    Response.ContentType = "application/vnd.ms-excel";

        //    string tab = "";

        //    for (int i = 1; i <= gvCircularMaster.Columns.Count - 3; i++)
        //    {
        //        Response.Write(tab + gvCircularMaster.Columns[i].HeaderText);

        //        tab = "\t";

        //    }

        //    Response.Write("\n");

        //    foreach (GridViewRow gvr in gvCircularMaster.Rows)
        //    {
        //        if (gvr.RowType == DataControlRowType.DataRow)
        //        {
        //            tab = "";
        //            Response.Write(tab + ((Label)gvr.FindControl("lblsrno")).Text);
        //            tab = "\t";
        //            Response.Write(tab + ((Label)gvr.FindControl("lblIssAuth")).Text);
        //            tab = "\t";
        //            Response.Write(tab + ((Label)gvr.FindControl("lblCircNo")).Text);
        //            tab = "\t";
        //            Response.Write(tab + ((Label)gvr.FindControl("lblCircularDate")).Text);
        //            tab = "\t";
        //            Response.Write(tab + ((Label)gvr.FindControl("lblTopic")).Text);
        //            tab = "\t";
        //            Response.Write("\n");

        //        }
        //    }

        //    Response.End();
        //    gvCircularMaster.AllowPaging = true;
        //    gvCircularMaster.AllowSorting = true;
        //    gvCircularMaster.DataBind();
        //}


        //public override void VerifyRenderingInServerForm(Control control)
        //{

        //}


        //protected void gvCircularMaster_Sorting(object sender, GridViewSortEventArgs e)
        //{

        //    if (!(Session["ViewCircular"] == null))
        //    {


        //        //DataSet ds = new DataSet();
        //        //ds = (DataSet)Session["ViewCircularSelectCommand"];
        //        //DataView dvDataView = new DataView(ds.Tables[0]);

        //        DataView dvDataView = (DataView)Session["ViewCircular"];
        //        if (Convert.ToString(Session["sort"]) == "" || Convert.ToString(Session["sort"]) == "ASC")
        //        {
        //            e.SortDirection = SortDirection.Ascending;
        //            dvDataView.Sort = (e.SortExpression + (" " + ConvertSortDirectionToSql(e.SortDirection)));
        //            Session["sort"] = "DESC";
        //        }
        //        else
        //        {
        //            e.SortDirection = SortDirection.Descending;
        //            dvDataView.Sort = (e.SortExpression + (" " + ConvertSortDirectionToSql(e.SortDirection)));
        //            Session["sort"] = "ASC";
        //        }
        //        gvCircularMaster.DataSource = dvDataView;
        //        gvCircularMaster.DataBind();
        //    }
        //}

        //private string ConvertSortDirectionToSql(SortDirection SortDirection)
        //{
        //    string m_SortDirection = String.Empty;
        //    switch (SortDirection)
        //    {
        //        case SortDirection.Ascending:
        //            m_SortDirection = "DESC";
        //            break;
        //        case SortDirection.Descending:
        //            m_SortDirection = "ASC";
        //            break;
        //    }
        //    return m_SortDirection;
        //}

        protected void gvCircularMaster_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (Session["ViewCircular"] != null)
            {
                DataTable dt = (DataTable)Session["ViewCircular"];
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
        protected void cvTodate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        public static string IsValidUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute) == true)
            {
                return url;
            }
            else
            {//onclick ='return false; alert('Invalid link!');'
                return "#";
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvCircularMaster.AllowPaging = false;
            gvCircularMaster.AllowSorting = false;
            gvCircularMaster.DataSource = (DataTable)(ViewState["ExportCirculars"]);
            gvCircularMaster.DataBind();

            F2FExcelExport.F2FExportGridViewToExcel(gvCircularMaster, "CircularsList", new int[] { 2 });
            return;
        }

    }
}