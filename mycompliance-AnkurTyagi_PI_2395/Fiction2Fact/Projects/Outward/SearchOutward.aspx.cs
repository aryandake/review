using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Fiction2Fact.Legacy_App_Code.Outward.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.Outward;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.VO;

namespace Fiction2Fact.Projects.Outward
{
    public partial class SearchOutward : System.Web.UI.Page
    {
        OutwardUtilitiesBLL outUtilBLL = new OutwardUtilitiesBLL();
        OutwardBL outwardBL = new OutwardBL();
        private int mintGridSelectedRowNo;
        private DataTable mdtFileUpload;
        CommonMethods cm = new CommonMethods();
        Authentication au = new Authentication();
        string strOutwardFilter = "";
        string[] strarrVal;
        string Ot_ID;
        //<< Added by ramesh more on 13-Mar-2024 CR_1991
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();
        //>>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if(Request.QueryString["type"] !=null)
                {
                    hftype.Value = Convert.ToString(Request.QueryString["type"]);
                    lblHeadrName.Text = "Search Outward Closure Tracker";
                }
                if (!(Session["ViewOutward"] == null))
                {
                    gvOutward.DataSource = (DataSet)(Session["ViewOutward"]);
                    gvOutward.DataBind();
                }
                if (!(Session["FileUploadDT"] == null))
                {
                    mdtFileUpload = (DataTable)Session["FileUploadDT"];
                }
                btnSearch.Attributes.Add("onmouseover", "this.src='../images/Search1.png'");
                btnSearch.Attributes.Add("onmouseout", "this.src='../images/Search.png'");
                btnExportToExcel.Attributes.Add("onmouseover", "this.src='../images/Export-To-Excel1.png'");
                btnExportToExcel.Attributes.Add("onmouseout", "this.src='../images/Export-To-Excel.png'");

                txtDocNo.Focus();


                ddlDept.DataSource = outUtilBLL.GetDataTable("OutwardDepartment", sOrderBy: "ODM_NAME");
                ddlDept.DataBind();
                ddlDept.Items.Insert(0, new ListItem("(Select)", ""));

                ddlOutwardType.DataSource = outUtilBLL.GetDataTable("getTypeofOutward", sOrderBy: "OTM_NAME");
                ddlOutwardType.DataBind();
                ddlOutwardType.Items.Insert(0, new ListItem("(Select)", ""));

                mvMultiView.ActiveViewIndex = 0;
                Session["EditOutward"] = null;
                

                if (Session["OutwardFilter"] != null)
                {
                    strOutwardFilter = Session["OutwardFilter"].ToString();
                    strarrVal = strOutwardFilter.Split('|');
                    txtDocNo.Text = strarrVal[0];
                    ddlOutwardType.SelectedValue = strarrVal[1];
                    ddlDept.SelectedValue = strarrVal[2];
                    txtDocumentName.Text = strarrVal[3];
                    ddlStatus.SelectedValue = strarrVal[4];
                    txtFromDate.Text = strarrVal[5];
                    txtToDate.Text = strarrVal[6];
                    txtGlobalSearch.Text = strarrVal[7];
                    UpdateGrid();
                }
                else
                {
                    UpdateGrid();
                }
            }
           
        }
        private void UpdateGrid()
        {
            string strStatus = null, strType = null, strFlag=null
                , strGlobalSearch = "", FileName = ""; string strAddressor;
            string strLoggedInUser = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
            strStatus = "";

            if (Roles.IsUserInRole("Outward_Admin"))
            {
                strType = "1";
            }
            else if (Roles.IsUserInRole("Outward_User"))
            {
                strType = "2";
            }
            else if (Roles.IsUserInRole("Outward_Compliance_User"))
            {
                strType = "3";
            }
            string strDocNo = txtDocNo.Text;
            strAddressor = "";

            string strAddressee = "";
            string strDocumentName = txtDocumentName.Text;
            string strOutwardType = ddlOutwardType.SelectedValue;
            string strOutwardDate = "";
            string strRegAuth = "";
            string strDept = ddlDept.SelectedValue;
            string FromDate = txtFromDate.Text;
            string ToDate = txtToDate.Text;
            if (Request.QueryString["type"] != null)
            {
                if(Roles.IsUserInRole("Outward_Admin"))
                {
                    //strStatus = "Submitted,Changes suggested by Compliance";
                    strFlag = "3";
                }
                else if (Roles.IsUserInRole("Outward_Compliance_User"))
                {
                    hftype.Value = Convert.ToString(Request.QueryString["type"]);
                    ddlStatus.SelectedValue = "Submitted";
                    strStatus = ddlStatus.SelectedValue;
                    strFlag = "1";
                }
                
            }
            else
            {
                strStatus = ddlStatus.SelectedValue;
                strFlag = "2";
            }
            
            FileName = "";
            DataSet dsView1 = new DataSet();

           

            strGlobalSearch = cm.getSanitizedString(txtGlobalSearch.Text);

            dsView1 = outwardBL.SearchOutwards(strDocNo, strAddressor, strAddressee,
                                            strDocumentName, strOutwardType, strOutwardDate,
                                            strRegAuth, strDept, strStatus, strType, strLoggedInUser,
                                            strGlobalSearch ,FromDate, ToDate, FileName,strFlag);

            Session["ViewOutward"] = dsView1;
            gvOutward.DataSource = dsView1;
            gvOutward.DataBind();

            hfOutwardIdForSearch.Value = "";

            
            lblInfo.Visible = true;
            if(dsView1.Tables[0].Rows.Count>0)
            {
                string strCount = dsView1.Tables[0].Rows.Count.ToString();
                lblInfo.Text = "Total No. of Records : " + strCount;
                btnExportToExcel.Visible = true;
            }
            else
            {
                this.lblInfo.Text = "No Records found satisfying the criteria.";
                btnExportToExcel.Visible = false;
            }
           
        }
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Session["ViewOutward"];
            gvOutward.AllowPaging = false;
            gvOutward.AllowSorting = false;
            gvOutward.DataSource = ds.Tables[0];
            gvOutward.DataBind();
            if (Roles.IsUserInRole("Outward_User"))
            {
                gvOutward.Columns[0].Visible = false;
                gvOutward.Columns[1].Visible = false;
            }
            else if (Roles.IsUserInRole("Outward_Admin"))
            {
                gvOutward.Columns[0].Visible = false;
                gvOutward.Columns[1].Visible = false;
                gvOutward.Columns[2].Visible = false;
                gvOutward.Columns[3].Visible = false;
                gvOutward.Columns[4].Visible = false;
            }
            else if (Roles.IsUserInRole("Outward_Compliance_User"))
            {
                gvOutward.Columns[0].Visible = false;
                gvOutward.Columns[1].Visible = false;
                gvOutward.Columns[2].Visible = false;
            }
            //PrepareGridViewForExport(gvOutward);
            string attachment = "attachment; filename=Outwards_details.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvOutward.RenderControl(htw);

            Response.Write(sw.ToString());
            Response.End();
            gvOutward.AllowPaging = true;
            gvOutward.AllowSorting = true;
            gvOutward.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        protected DataTable LoadOutwardsFileList(object Id)
        {
            int attachId = Convert.ToInt32(Id);
            DataTable dtFiles = outUtilBLL.GetDataTable("getOutwardFiles", new DBUtilityParameter("OF_OT_ID", attachId));
            return dtFiles;
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
                    gv.Controls.Remove(gv.Controls[i]);
                }
                if (gv.Controls[i].GetType() == typeof(DataList))
                {
                    gv.Controls.Remove(gv.Controls[i]);
                }

                if (gv.Controls[i].HasControls())
                {
                    PrepareGridViewForExport(gv.Controls[i]);
                }
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                strOutwardFilter = txtDocNo.Text + "|" + ddlOutwardType.SelectedValue + "|" + ddlDept.SelectedValue
                    + "|" + txtDocumentName.Text + "|" + ddlStatus.SelectedValue + "|" + txtFromDate.Text
                   + "|" + txtToDate.Text + "|" + txtGlobalSearch.Text;

                Session["OutwardFilter"] = strOutwardFilter;
                UpdateGrid();
            }
            catch (Exception ex)
            {
                this.lblInfo.Text = ex.Message;
                this.lblInfo.ForeColor = System.Drawing.Color.Red;
                this.lblInfo.Visible = true;
            }
        }

        protected void gvOutward_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string strId, strLinkedOutwardId;
            lblInfo.Text = "";
            try
            {
                strId = gvOutward.SelectedValue.ToString();
                strLinkedOutwardId = ((HiddenField)(gvOutward.SelectedRow.FindControl("hfLinkedOutwardId"))).Value;
                Session["OutwardId"] = strId;
                hfSelectedRecord.Value = strId;
                mintGridSelectedRowNo = Convert.ToInt32(gvOutward.SelectedIndex);
                DataList dlUpdates = new DataList();
                DataTable dtFiles = new DataTable();
                DataTable dtEditFiles = new DataTable();
                DataSet dsView = new DataSet();
                
                if (hfSelectedOperation.Value == "View")
                {
                    if (Request.QueryString["type"] != null)
                    {
                        //<< Modified by ramesh more on 13-Mar-2024 CR_1991
                        Response.Redirect(Global.site_url("Projects/Outward/ViewOutward.aspx?Id=" + encdec.Encrypt(hfSelectedRecord.Value) + "&source=SOC"));
                        //>>
                    }
                    else
                    {
                        //<< Modified by ramesh more on 13-Mar-2024 CR_1991
                        Response.Redirect(Global.site_url("Projects/Outward/ViewOutward.aspx?Id=" + encdec.Encrypt(hfSelectedRecord.Value) + "&source=SO"));
                        //>>
                    }
                }

                else if (hfSelectedOperation.Value == "ViewLinkedOutward")
                {
                    if (Request.QueryString["type"] != null)
                    {
                        Response.Redirect(Global.site_url("Projects/Outward/ViewOutward.aspx?Id=" + encdec.Encrypt(strLinkedOutwardId) + "&source=SOC"));
                    }
                    else
                    {
                        Response.Redirect(Global.site_url("Projects/Outward/ViewOutward.aspx?Id=" + encdec.Encrypt(strLinkedOutwardId) + "&source=SO"));
                    }
                }

                else if (hfSelectedOperation.Value == "Edit")
                {
                    if (Roles.IsUserInRole("Outward_User"))
                    {
                        Response.Redirect(Global.site_url("Projects/Outward/AddEditOutward.aspx?Id=" + encdec.Encrypt(hfSelectedRecord.Value)));
                        
                    }
                    else if (Roles.IsUserInRole("Outward_Admin") || Roles.IsUserInRole("Outward_Compliance_User"))
                    {
                        Response.Redirect(Global.site_url("Projects/Outward/EditOutward.aspx?Id=" + encdec.Encrypt(hfSelectedRecord.Value)+ "&source=SOC"));
                    }
                    
                }
                else if (hfSelectedOperation.Value == "Delete")
                {
                    deleteOutward();
                    UpdateGrid();
                }
                else if (hfSelectedOperation.Value == "Revision")
                {
                    string url = "SuggestRevision.aspx?Id=" + hfSelectedRecord.Value;
                    string s = "window.open('" + url + "', 'popup_window', 'width=800,height=300,left=0,top=0,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }

            }
            catch (Exception ex)
            {
                this.lblInfo.Text = ex.Message;
                this.lblInfo.Visible = true;
            }
        }

        private void deleteOutward()
        {
            try
            {
                DataTable dt = new DataTable();
                int id = Convert.ToInt32(gvOutward.SelectedValue);
                outUtilBLL.GetDataTable("deleteOutwardTracker", new DBUtilityParameter("OT_ID", id));
                writeError("Outward has been successfully deleted.");
                //UpdateGrid();
            }
            catch (Exception ex)
            {
                this.lblInfo.Text = ex.Message;
                this.lblInfo.Visible = true;
            }
        }

        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
            string script = "\r\n<script language=\"javascript\">\r\n" +
                       " alert('" + strError.Replace("'", "\\'") + "');" +
                       "   </script>\r\n";

            ClientScript.RegisterStartupScript(this.GetType(), "script", script);
        }

        protected void btnCancel1_Click(object sender, System.EventArgs e)
        {
            mvMultiView.ActiveViewIndex = 0;
            ViewState["EditOutward"] = null;
        }

        protected void gvOutward_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOutward.PageIndex = e.NewPageIndex;
            gvOutward.DataSource = Session["ViewOutward"];
            gvOutward.DataBind();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["EditOutward"] = Session["EditOutward"];

        }

        #region Gridview Sorting
        protected void gvOutward_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (Session["ViewOutward"] != null)
            {

                DataSet ds = (DataSet)Session["ViewOutward"];
                DataTable dt = ds.Tables[0];
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
                gvOutward.DataSource = dvDataView;
                gvOutward.DataBind();
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
            string strSortExpression = "";

            if (ViewState["_SortExpression_"] != null)
                strSortExpression = ViewState["_SortExpression_"].ToString();

            if (!strSortExpression.Equals(""))
            {
                foreach (DataControlField field in gvOutward.Columns)
                {
                    if (field.SortExpression.Equals(strSortExpression))
                    {
                        return gvOutward.Columns.IndexOf(field);
                    }
                }
            }
            return -1;
        }

        void AddSortImage(GridViewRow headerRow, string strAction, int sortColumnIndex)
        {
            Image sortImage = new Image();
            if (strAction.Equals("ASC"))
            {
                sortImage.ImageUrl = "../images/view_sort_ascending.png";
                sortImage.AlternateText = "Ascending Order";
            }
            else if (strAction.Equals("DESC"))
            {
                sortImage.ImageUrl = "../images/view_sort_descending.png";
                sortImage.AlternateText = "Descending Order";
            }
            headerRow.Cells[sortColumnIndex].Controls.Add(sortImage);
        }
        #endregion Gridview Sorting

        protected void gvOutward_DataBound(object sender, EventArgs e)
        {
            if (Roles.IsUserInRole("Outward_Admin"))
            {
                //gvOutward.Columns[2].Visible = false;
            }
            else if (Roles.IsUserInRole("Outward_User"))
            {
                gvOutward.Columns[2].Visible = false;
                gvOutward.Columns[3].Visible = false;
                gvOutward.Columns[4].Visible = false;
            }
            else if (Roles.IsUserInRole("Outward_Compliance_User"))
            {
                gvOutward.Columns[2].Visible = true;
                gvOutward.Columns[3].Visible = false;
                gvOutward.Columns[4].Visible = false;

            }
            
        }

        protected void gvOutward_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            
            GridViewRow gvr;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                gvr = e.Row;
                DataRowView drv = e.Row.DataItem as System.Data.DataRowView;

                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Label lblCreator = (Label)e.Row.FindControl("lblCreator");
                LinkButton lnkEdit = (LinkButton)(e.Row.FindControl("lnkEdit"));
                LinkButton lnkDelete = (LinkButton)(e.Row.FindControl("lnkDeleteOutward"));
                LinkButton lnkDeactivate = (LinkButton)(e.Row.FindControl("lnkDeactivate"));

                //HiddenField hflon = (HiddenField)(e.Row.FindControl("hflo"));
                //LinkButton lnkLinkedOutward = (LinkButton)(e.Row.FindControl("lnkView1"));
                //UtilitiesBLL ubll = new UtilitiesBLL();
                //UtilitiesVO objUVO = new UtilitiesVO();
                //string val = hflon.Value;
                //if (val != "")
                //{
                //    objUVO.setCode(" AND OT_DOCUMENT_NO ='" + hflon.Value + "'");
                //    DataTable dt = ubll.getData("OutwardId", objUVO);
                //    if (dt != null)
                //    {
                //        DataRow dr1 = dt.Rows[0];
                //        Ot_ID = dr1["OT_ID"].ToString();

                //    }

                //    lnkLinkedOutward.OnClientClick = "return onViewClick1('" + Ot_ID + "');";
                //}
                Label HFOTID = (Label)(e.Row.FindControl("lbId"));
                LinkButton lnkRevision = (LinkButton)(e.Row.FindControl("lnkRevisioSuggested"));
               
                TextBox txtCanxRemarks = (TextBox)(e.Row.FindControl("txtCanxRemarks"));
                if (drv != null)
                {
                    Label lbId = (Label)(gvr.FindControl("lbId"));
                    int intOTId = Convert.ToInt32(lbId.Text);
                    LinkButton lb1 = (LinkButton)(gvr.FindControl("lnkView"));
                    lb1.Attributes.Add("onclick", "javascript:return showOutwardGist('" + Convert.ToString(intOTId) + "')");

                   
                }
                if (Roles.IsUserInRole("Outward_Admin"))
                {
                    //if (lblStatus.Text.Equals("Pending") || lblStatus.Text.Equals("Cancelled"))
                    lnkEdit.Visible = false;
                    //lnkDelete.Visible = false;
                    lnkDeactivate.Visible = false;
                    lnkRevision.Visible = false;
                    if (lblStatus.Text.Equals("Submitted"))
                    {
                        lnkEdit.Visible = true;
                        //lnkDelete.Visible = true;
                        lnkDeactivate.Visible = true;
                        //lnkRevision.Visible = true;
                    }
                    if (lblStatus.Text.Equals("Deleted"))
                    {
                        lnkDelete.Visible = false;
                        
                    }

                    if (lblStatus.Text.Equals("Changes suggested by Compliance"))
                    {
                        lnkEdit.Visible = true;
                        //lnkDelete.Visible = true;
                        //lnkDeactivate.Visible = true;
                    }



                }
                else if (Roles.IsUserInRole("Outward_User"))
                {
                    lnkEdit.Visible = false;
                    lnkDeactivate.Visible = false;
                    //if ((lblStatus.Text.Equals("Pending")))
                    //{
                    //    lnkEdit.Visible = true;

                    //}
                    if (lblStatus.Text.Equals("Changes suggested by Compliance"))
                    {
                        lnkEdit.Visible = true;
                        // lnkCancel.Visible = false;
                    }


                }
                
                else if (Roles.IsUserInRole("Outward_Compliance_User"))
                {
                    lnkEdit.Visible = false;
                    lnkRevision.Visible = false;
                    if ((lblStatus.Text.Equals("Submitted")))
                    {
                        lnkEdit.Visible = true;
                        lnkRevision.Visible = true;

                    }
                    if (lblStatus.Text.Equals("Deleted"))
                    {
                        lnkDelete.Visible = false;
                        
                    }

                }


            }
           
        }
        protected void btnCancellation_ServerClick(object sender, EventArgs e)
        {
            string strId = "", strRemarks = "", strCurrentUserName="", strCreateBy="", outwardNo = "", docName = "";
            try
            {
                //string strCreateBy = Convert.ToString(Page.User.Identity.Name);
                if (!Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)).Equals(""))
                {
                    //strCurrentUserName = au.GetUserDetails(Page.User.Identity.Name).Split(',')[0].ToString();
                    //strCreateBy = strCurrentUserName + " (" + Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)) + ")";
                    strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                }
                string CancelOn = DateTime.Now.ToString();
                strId = hfSelectedRecord.Value;
                int intId = Convert.ToInt32(strId);
                strRemarks = txtDeactRemarks.Text;
                outwardBL.cancelOutwardTrackers(intId, strRemarks, strCreateBy, CancelOn);
                UtilitiesBLL ubll = new UtilitiesBLL();
                UtilitiesVO objUVO = new UtilitiesVO();
                objUVO.setCode(" AND OT_ID =" + intId + " ");
                DataTable dt = ubll.getData("OutwarNo", objUVO);
                if (dt != null)
                {
                    DataRow dr1 = dt.Rows[0];
                    outwardNo = dr1["OT_DOCUMENT_NO"].ToString();
                    docName = dr1["OT_DOC_NAME"].ToString();
                }
                //string docName= ubll.outno("docName", objUVO);
                sendMailOnOutwardDeactivation(outwardNo, strCreateBy, docName);
                ClientScript.RegisterStartupScript(this.GetType(), "displayDeactivationSuccessMessage", "alert('Outward cancelled successfully.');", true);

                UpdateGrid();
                txtDeactRemarks.Text = "";
            }
            catch (Exception ex)
            {
                // hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        protected void btndelete_ServerClick(object sender, EventArgs e)
        {
            string strId = "", strRemarks = "", strCurrentUserName = "", strCreateBy = "", outwardNo = "", docName = "";
            try
            {
                //string strCreateBy = Convert.ToString(Page.User.Identity.Name);
                if (!Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)).Equals(""))
                {
                    strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                    //strCurrentUserName = au.GetUserDetails(Page.User.Identity.Name).Split(',')[0].ToString();
                    //strCreateBy = strCurrentUserName + " (" + Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)) + ")";
                }
                string deleteOn = DateTime.Now.ToString();
                strId = hfSelectedRecord.Value;
                int intId = Convert.ToInt32(strId);
                strRemarks = txtdeletionRemark.Text;
                outwardBL.deleteOutwardTrackers(intId, strRemarks, strCreateBy, deleteOn);
                //UtilitiesBLL ubll = new UtilitiesBLL();
                //UtilitiesVO objUVO = new UtilitiesVO();
                //objUVO.setCode(" AND OT_ID =" + intId + " ");
                //DataTable dt = ubll.getData("OutwarNo", objUVO);
                //if (dt != null)
                //{
                //    DataRow dr1 = dt.Rows[0];
                //    outwardNo = dr1["OT_DOCUMENT_NO"].ToString();
                //    docName = dr1["OT_DOC_NAME"].ToString();
                //}
                ////string docName= ubll.outno("docName", objUVO);
                //sendMailOnOutwardDeletetion(outwardNo, strCreateBy, docName);
                ClientScript.RegisterStartupScript(this.GetType(), "displayDeactivationSuccessMessage", "alert('Outward deleted successfully.');", true);

                UpdateGrid();
                txtDeactRemarks.Text = "";
            }
            catch (Exception ex)
            {
                // hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnrevision_ServerClick(object sender, EventArgs e)
        {
            string strId = "", strRevisionRemarks = "", strCurrentUserName = "", strCreateBy = "", outwardNo="", docName="";
            try
            {
                //string strCreateBy = Convert.ToString(Page.User.Identity.Name);
                if (!Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)).Equals(""))
                {
                    //strCurrentUserName = au.GetUserDetails(Page.User.Identity.Name).Split(',')[0].ToString();
                    //strCreateBy = strCurrentUserName + " (" + Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name)) + ")";
                    strCreateBy = Convert.ToString(Authentication.GetUserID(Page.User.Identity.Name));
                }
                string RevisionOn = DateTime.Now.ToString();
                strId = hfSelectedRecord.Value;
                int intId = Convert.ToInt32(strId);
                strRevisionRemarks = txtrevisionRemark.Text;
                outwardBL.revisionOutwardTrackers(intId, strRevisionRemarks, strCreateBy, RevisionOn);
                UtilitiesBLL ubll = new UtilitiesBLL();
                UtilitiesVO objUVO = new UtilitiesVO();
                objUVO.setCode(" AND OT_ID =" + intId + " ");
                DataTable dt = ubll.getData("OutwarNo", objUVO);
                if(dt!=null)
                {
                    DataRow dr1 = dt.Rows[0];
                    outwardNo = dr1["OT_DOCUMENT_NO"].ToString();
                    docName = dr1["OT_DOC_NAME"].ToString();
                }
                //string docName= ubll.outno("docName", objUVO);
                sendMailOnOutwardRevision(outwardNo, strCreateBy, docName);
                ClientScript.RegisterStartupScript(this.GetType(), "displayRevisionSuccessMessage", "alert('Outward revision suggested successfully.');", true);

                UpdateGrid();
                txtDeactRemarks.Text = "";
            }
            catch (Exception ex)
            {
                // hfDoubleClickFlag.Value = "";
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        private void sendMailOnOutwardRevision(string strDocNo, string logedUser, string strDocName)
        {
            Authentication auth = new Authentication();
            string strMailConfigId = "", strUserId = "", strUserDetails = "", strUserName = "", strEmail = "";
            try
            {
                strUserId = Page.User.Identity.Name;
                strUserDetails = auth.GetUserDetsByEmpCode(strUserId);
                strUserName = strUserDetails.Split('|')[0];
                strEmail = strUserDetails.Split('|')[1];

                MailContent_Outward mcraf = new MailContent_Outward();
                mcraf.ParamMap.Add("ConfigId", "1097");
                mcraf.ParamMap.Add("To", "Outward_User");
                mcraf.ParamMap.Add("cc", "Comp_User,Admin_User");
                mcraf.ParamMap.Add("ModuleCode", "Outward");
                mcraf.ParamMap.Add("Ids", strDocNo);
                mcraf.ParamMap.Add("Subject", strDocName);
                mcraf.ParamMap.Add("CreatorUserId", strUserId);
                mcraf.ParamMap.Add("LoggedInUserName", logedUser);
                mcraf.setOutwardMailContent("");
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void sendMailOnOutwardDeletetion(string strDocNo, string logedUser, string strDocName)
        {
            Authentication auth = new Authentication();
            string strMailConfigId = "", strUserId = "", strUserDetails = "", strUserName = "", strEmail = "";
            try
            {
                strUserId = Page.User.Identity.Name;
                strUserDetails = auth.GetUserDetsByEmpCode(strUserId);
                strUserName = strUserDetails.Split('|')[0];
                strEmail = strUserDetails.Split('|')[1];

                MailContent_Outward mcraf = new MailContent_Outward();
                mcraf.ParamMap.Add("ConfigId", "1099");
                mcraf.ParamMap.Add("To", "Comp_User");
                mcraf.ParamMap.Add("cc", "Outward_User,Admin_User");
                mcraf.ParamMap.Add("ModuleCode", "Outward");
                mcraf.ParamMap.Add("Ids", strDocNo);
                mcraf.ParamMap.Add("Subject", strDocName);
                mcraf.ParamMap.Add("CreatorUserId", strUserId);
                mcraf.ParamMap.Add("LoggedInUserName", logedUser);
                mcraf.setOutwardMailContent("");
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        private void sendMailOnOutwardDeactivation(string strDocNo, string logedUser, string strDocName)
        {
            Authentication auth = new Authentication();
            string strMailConfigId = "", strUserId = "", strUserDetails = "", strUserName = "", strEmail = "";
            try
            {
                strUserId = Page.User.Identity.Name;
                strUserDetails = auth.GetUserDetsByEmpCode(strUserId);
                strUserName = strUserDetails.Split('|')[0];
                strEmail = strUserDetails.Split('|')[1];

                MailContent_Outward mcraf = new MailContent_Outward();
                mcraf.ParamMap.Add("ConfigId", "1098");
                mcraf.ParamMap.Add("To", "Outward_User");
                mcraf.ParamMap.Add("cc", "Admin_User,Comp_User");
                mcraf.ParamMap.Add("ModuleCode", "Outward");
                mcraf.ParamMap.Add("Ids", strDocNo);
                mcraf.ParamMap.Add("Subject", strDocName);
                mcraf.ParamMap.Add("CreatorUserId", strUserId);
                mcraf.ParamMap.Add("LoggedInUserName", logedUser);
                mcraf.setOutwardMailContent("");
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        protected void btnclose_ServerClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#divModal').modal('hide');", true);
        }
        protected DataTable LoadLastRevisionSuggested(object Id)
        {
            int requestId = Convert.ToInt32(Id);
            DataTable dtFiles = outUtilBLL.GetDataTable("getOutwardFiles", new DBUtilityParameter("OF_OT_ID", requestId));
            return dtFiles;
        }
        protected void cvTodate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        protected void btnreset_Click(object sender, EventArgs e)
        {
            txtDocNo.Text = string.Empty;
            ddlOutwardType.SelectedValue = null;
           txtDocumentName.Text = string.Empty;
            ddlStatus.SelectedValue = null;
             ddlDept.SelectedValue = null;
           txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            txtGlobalSearch.Text = string.Empty;
            
        }
        protected void btnclosed_Click(object sender, EventArgs e)
        {
            txtrevisionRemark.Text = string.Empty;
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }
        protected void btnbk_Click(object sender, EventArgs e)
        {
            txtDeactRemarks.Text = string.Empty;
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }
    }

}