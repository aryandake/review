using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.Circulars.BLL;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using System.IO;
using Fiction2Fact.App_Code;
using System.Web.UI.HtmlControls;
using Fiction2Fact.Legacy_App_Code.Circulars;

namespace Fiction2Fact.Projects.Circulars
{
    public partial class Circulars_CircularList : System.Web.UI.Page
    {
        CircUtilitiesBLL circUtilBLL = new CircUtilitiesBLL();
        //UtilitiesBLL UtilitiesBLL = new UtilitiesBLL();
        string mstrConnectionString = null;
        RefCodesBLL rcBL = new RefCodesBLL();
        CommonMethods cm = new CommonMethods();
        CircularMasterBLL cirBLL = new CircularMasterBLL();
        //<< Added by ramesh more on 13-Mar-2024 CR_1991
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();
        //>>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HtmlMeta tag = new HtmlMeta();
                tag.HttpEquiv = "X-UA-Compatible";
                tag.Content = "IE=edge";
                this.Page.Header.Controls.AddAt(0, tag);

                if (!IsPostBack)
                {
                    CommonCodes.SetDropDownDataSource(ddlType, circUtilBLL.GetDataTable("getTypeofCircular", new DBUtilityParameter("CDTM_STATUS", "A"), sOrderBy: "CDTM_TYPE_OF_DOC"));
                    CommonCodes.SetDropDownDataSource(ddlStatus, rcBL.getRefCodeDetails("Actionable Status"));

                    hfCurrDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                    if (Session["PopulateonLoad"] == null)
                    {
                        //getActionableList();
                    }
                    else
                    {
                        DataTable dtPrevious;
                        string[] arr = Convert.ToString(Session["PopulateControlonLoad"]).Split('|');
                        txtCircularNo.Text = arr[0]; txtCircFromDate.Text = arr[1]; txtCircToDate.Text = arr[2]; ddlType.SelectedValue = arr[3];
                        ddlCircularAuthority.SelectedValue = arr[4]; txtTopic.Text = arr[5]; txtSubjectofCircular.Text = arr[6];
                        txtActionable.Text = arr[7]; txtPersonResponsible.Text = arr[8]; txtFromDate.Text = arr[9]; txtToDate.Text = arr[10];
                        gvCircularActionableList.DataSource = (DataTable)Session["PopulateonLoad"];
                        gvCircularActionableList.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void cvTodate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
        protected void cvCircToDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
        protected void lnkReset_Click(object sender, System.EventArgs e)
        {
            Session["PopulateonLoad"] = null;
            Response.Redirect(Request.RawUrl);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this))
            {
                return;
            }
            try
            {
                getActionableList();
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        public void getActionableList()
        {
            try
            {
                lblMsg.Text = "";
                CircularMasterBLL cmBL = new CircularMasterBLL();
                DataTable dt_s = new DataTable();

                if (Convert.ToString(Session["ActionableList"]) == "")
                {
                    string FromDate = txtFromDate.Text;
                    string ToDate = txtToDate.Text;
                    dt_s = cmBL.SearchCircularActionableNew(cm.getSanitizedString(txtCircularNo.Text), cm.getSanitizedString(txtCircFromDate.Text), cm.getSanitizedString(txtCircToDate.Text), ddlType.SelectedValue, ddlCircularAuthority.SelectedValue,
                        cm.getSanitizedString(txtTopic.Text), cm.getSanitizedString(txtSubjectofCircular.Text), cm.getSanitizedString(txtActionable.Text), txtPersonResponsible.Text, FromDate, ToDate,
                        ddlStatus.SelectedValue, mstrConnectionString);
                    Session["ActionableList"] = dt_s;
                    Session["PopulateControlonLoad"] = txtCircularNo.Text + "|" + txtCircFromDate.Text + "|" + txtCircToDate.Text + "|" + ddlType.SelectedValue + "|" + ddlCircularAuthority.SelectedValue + "|" + txtTopic.Text + "|" + txtSubjectofCircular.Text + "|" +
                        txtActionable.Text + "|" + txtPersonResponsible.Text + "|" + FromDate + "|" + ToDate;
                    Session["PopulateonLoad"] = dt_s;
                    gvCircularActionableList.DataSource = dt_s;
                    gvCircularActionableList.DataBind();
                }
                else
                {
                    gvCircularActionableList.DataSource = (DataTable)Session["ActionableList"];
                    gvCircularActionableList.DataBind();
                }
                if (dt_s.Rows.Count > 0)
                    btnExportToExcel.Visible = true;
                else
                    btnExportToExcel.Visible = false;
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected DataTable LoadCircularActionableFileList(object CircularId)
        {
            DataTable dtCircularFiles = new DataTable();

            try
            {
                //dtCircularFiles = UtilitiesBLL.getDatasetWithCondition("LoadCircularActionableFileList", Convert.ToInt32(CircularId), mstrConnectionString);
                dtCircularFiles = circUtilBLL.GetDataTable("LoadCircularActionableFileList", new DBUtilityParameter("CAF_CA_ID", CircularId));
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }

            return dtCircularFiles;
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
        protected void gvCircularActionableList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                string strActionableId = gvCircularActionableList.SelectedValue.ToString();
                Label lblCircularMasId = (Label)(gvCircularActionableList.SelectedRow.FindControl("lblCircularMasId"));
                HiddenField hfStatus = (HiddenField)(gvCircularActionableList.SelectedRow.FindControl("hfStatus"));

                if (hfSelectedOperation.Value.Equals("View"))
                {
                    Response.Redirect(Global.site_url("Projects/Circulars/ActionableUpdates.aspx?Source=List&CircularId=" + encdec.Encrypt(lblCircularMasId.Text) +
                        "&ActionableId=" + encdec.Encrypt(strActionableId) + "&Status=" + hfStatus.Value));
                }
                else if (hfSelectedOperation.Value.Equals("Edit"))
                {
                    //<< Modified by Amarjeet on 14-Jul-2021
                    //Response.Redirect(Global.site_url("Projects/Circulars/EditCircularActionables.aspx?Source=List&CircularId=" + lblCircularMasId.Text + "&ActionableId=" + strActionableId));
                    string script = "window.open('AddCircularActionable.aspx?CirId=" + lblCircularMasId.Text + "&ActionableId=" + strActionableId + "','_blank');";
                    ClientScript.RegisterStartupScript(this.GetType(), "EditActionable", script, true);
                    //>>
                }
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        public void writeError(string strMsg)
        {
            lblMsg.Text = strMsg;
            lblMsg.Visible = true;
            lblMsg.CssClass = "label";
        }

        protected void gvCircularActionableList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCircularActionableList.PageIndex = e.NewPageIndex;
            gvCircularActionableList.DataSource = (DataTable)(Session["ActionableList"]);
            gvCircularActionableList.DataBind();
        }

        protected void gvCircularActionableList_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (Session["ActionableList"] != null)
            {
                DataTable dt = (DataTable)Session["ActionableList"];
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
                gvCircularActionableList.DataSource = dvDataView;
                gvCircularActionableList.DataBind();
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
                foreach (DataControlField field in gvCircularActionableList.Columns)
                {
                    if (field.SortExpression.Equals(strSortExpression))
                    {
                        return gvCircularActionableList.Columns.IndexOf(field);
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

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvCircularActionableList.AllowPaging = false;
            gvCircularActionableList.AllowSorting = false;
            gvCircularActionableList.Columns[0].Visible = false;
            gvCircularActionableList.Columns[1].Visible = false;
            gvCircularActionableList.Columns[2].Visible = false;
            gvCircularActionableList.Columns[17].Visible = false;
            gvCircularActionableList.DataSource = (DataTable)Session["ActionableList"];
            gvCircularActionableList.DataBind();
            CommonCodes.PrepareGridViewForExport(gvCircularActionableList);
            string attachment = "attachment; filename=Actionable List.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvCircularActionableList.RenderControl(htw);

            Response.Write(sw.ToString());
            Response.End();
            gvCircularActionableList.AllowPaging = true;
            gvCircularActionableList.AllowSorting = true;
            gvCircularActionableList.Columns[0].Visible = true;
            gvCircularActionableList.Columns[1].Visible = true;
            gvCircularActionableList.Columns[2].Visible = false;
            gvCircularActionableList.Columns[17].Visible = true;
            gvCircularActionableList.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        //<< Added by Amarjeet on 14-Jul-2021
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                getActionableList();
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }

        protected void btnCloseActionable_ServerClick(object sender, EventArgs e)
        {
            try
            {
                //circUtilBLL.GetDataTable("closeCircularActionable",
                //    new DBUtilityParameter("CA_ID", hfActionableId.Value, oSecondValue: txtClosureRemarks.Text),
                //    new DBUtilityParameter("1", 1, oSecondValue: txtCompDate.Text),
                //    new DBUtilityParameter("1", 1, oSecondValue: (new Authentication()).getUserFullName(Page.User.Identity.Name)));

                //ClientScript.RegisterStartupScript(this.GetType(), "displaySucessMessage", "alert('Actionable closed successfully.');", true);

                #region MyRegion

                string strActionable = null;
                string strRespPersonId = null, strRespPersonName = null, strRespPersonEmail = null;
                string strManagerId = null, strManagerName = null, strManagerEmail = null;
                string strTargetDate = null, strCompletionDate = null, strRegulatoryDueDate = null, strStatus = null;
                string strRemark = null, strResFunc = null;

                DataTable dt = cirBLL.SearchCircularActionable(0, "", "", "", "", hfActionableId.Value, "", mstrConnectionString);

                DataTable dtActionable = new DataTable();
                dtActionable.Columns.Add(new DataColumn("Id", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("Actionable", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("ResFuncName", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("PerResp", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("PerRespUserId", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("PerRespUserName", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("PerRespEmailId", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("ReportMgr", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("ReportMgrId", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("ReportMgrUserName", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("ReportMgrEmailId", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("TargetDate", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("RegulatoryDueDate", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("ComplDate", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("Status", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("StatusName", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("CommType", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("Function", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("Remarks", typeof(string)));
                //<< Added by Amarjeet on 14-Jul-2021
                dtActionable.Columns.Add(new DataColumn("SpocFromComplianceFnId", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("CirType", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("CirSubject", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("CirIssAuthority", typeof(string)));
                dtActionable.Columns.Add(new DataColumn("CirDate", typeof(string)));
                //>>
                DataRow dr = dtActionable.NewRow();

                dr["Id"] = string.IsNullOrEmpty(hfActionableId.Value) ? "0" : hfActionableId.Value;
                dr["Actionable"] = dt.Rows[0]["CA_ACTIONABLE"].ToString();
                dr["ResFuncName"] = dt.Rows[0]["ResponsibleFunction"].ToString();
                dr["PerResp"] = dt.Rows[0]["CA_PERSON_RESPONSIBLE"].ToString();
                dr["PerRespUserId"] = dt.Rows[0]["CA_PERSON_RESPONSIBLE_ID"].ToString();
                dr["PerRespUserName"] = dt.Rows[0]["CA_PERSON_RESPONSIBLE_NAME"].ToString(); ;
                dr["PerRespEmailId"] = dt.Rows[0]["CA_PERSON_RESPONSIBLE_EMAIL_ID"].ToString(); ;
                dr["TargetDate"] = string.IsNullOrEmpty(dt.Rows[0]["CA_TARGET_DATE"].ToString()) ? "" : dt.Rows[0]["CA_TARGET_DATE"].ToString();
                dr["RegulatoryDueDate"] = string.IsNullOrEmpty(dt.Rows[0]["CA_REGULATORY_DUE_DATE"].ToString()) ? "" : dt.Rows[0]["CA_REGULATORY_DUE_DATE"].ToString();
                dr["ComplDate"] = string.IsNullOrEmpty(dt.Rows[0]["CA_COMPLETION_DATE"].ToString()) ? "" : dt.Rows[0]["CA_COMPLETION_DATE"].ToString();
                dr["Status"] = dt.Rows[0]["Status"].ToString();
                dr["StatusName"] = dt.Rows[0]["Status"].ToString();
                dr["CommType"] = null;
                dr["Function"] = strResFunc;
                dr["Remarks"] = strRemark;
                dr["ReportMgr"] = dt.Rows[0]["CA_Reporting_Mgr_Name"].ToString();
                dr["ReportMgrId"] = dt.Rows[0]["CA_Reporting_Mgr_ID"].ToString();
                dr["ReportMgrUserName"] = dt.Rows[0]["CA_Reporting_Mgr_Name"].ToString();
                dr["ReportMgrEmailId"] = dt.Rows[0]["CA_Reporting_Mgr_EMAIL_ID"].ToString();
                //<< Added by Amarjeet on 14-Jul-2021
                dr["SpocFromComplianceFnId"] = dt.Rows[0]["CM_CCS_ID"].ToString();
                dr["CirType"] = dt.Rows[0]["CDTM_TYPE_OF_DOC"].ToString();
                dr["CirSubject"] = dt.Rows[0]["CM_TOPIC"].ToString();
                dr["CirIssAuthority"] = dt.Rows[0]["CIA_NAME"].ToString();
                dr["CirDate"] = string.IsNullOrEmpty(dt.Rows[0]["CM_DATE"].ToString()) ? "" : dt.Rows[0]["CM_DATE"].ToString();
                //>>
                dtActionable.Rows.Add(dr);

                sendCircularActionableMail(dtActionable);

                #endregion

                txtClosureRemarks.Text = "";
                hfClosureRemarks.Value = "";
                txtCompDate.Text = "";
                hfClosureDate.Value = "";
                getActionableList();
            }
            catch (Exception ex)
            {
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
            }
        }
        public void sendCircularActionableMail(DataTable dtActionable)
        {
            DataTable dtSpocFromcompliance = new DataTable();
            string strMailTo = "", strMailCC = "";
            MailContent_Circulars mail = new MailContent_Circulars();

            if (dtActionable.Rows.Count > 0)
            {
                try
                {
                    DataRow dr = dtActionable.Rows[0];

                    strMailTo = dr["PerRespEmailId"].ToString();
                    strMailCC = dr["ReportMgrEmailId"].ToString();

                    //dtSpocFromcompliance = utilBLL.getDatasetWithConditionInString("getSpocFromComplianceFunction", " AND CCS_STATUS = 'A' and CCS_ID = " + dr["SpocFromComplianceFnId"].ToString() + "", mstrConnectionString);
                    //dtSpocFromcompliance = circUtilBLL.GetDataTable("getSpocFromComplianceFunction", new DBUtilityParameter("CCS_STATUS", "A"), new DBUtilityParameter("CCS_ID", dr["SpocFromComplianceFnId"].ToString()), sOrderBy: "CCS_NAME");
                    dtSpocFromcompliance = circUtilBLL.GetDataTable("getSpocFromComplianceFunction", new DBUtilityParameter("CCS_STATUS", "A"), new DBUtilityParameter("CCS_ID", dr["SpocFromComplianceFnId"].ToString(), "IN", null, "AND", 1), sOrderBy: "CCS_NAME");

                    foreach (DataRow dr1 in dtSpocFromcompliance.Rows)
                    {
                        strMailCC = (string.IsNullOrEmpty(strMailCC) ? "" : strMailCC + ",") + dr1["CCS_EMAIL_ID"].ToString();
                    }

                    mail.ParamMap.Add("ConfigId", 1092);
                    mail.ParamMap.Add("To", "ProvidedAsParam");
                    mail.ParamMap.Add("ToEmailIds", strMailTo);
                    mail.ParamMap.Add("cc", "CircularAdmin,CircularUser,ProvidedAsParam1");
                    mail.ParamMap.Add("CCEmailIds", strMailCC);
                    mail.ParamMap.Add("CirType", dr["CirType"].ToString());
                    mail.ParamMap.Add("CirSubject", dr["CirSubject"].ToString());
                    mail.ParamMap.Add("CirIssAuthority", dr["CirIssAuthority"].ToString());
                    mail.ParamMap.Add("CirDate", dr["CirDate"].ToString());
                    mail.setCircularMailContent(dtActionable);
                }
                catch (Exception ex)
                {
                    string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    writeError(F2FLog.F2FEnvironment == "PRODUCTION" ? sMessage : "System Exception in " + System.Reflection.MethodBase.GetCurrentMethod().Name + "():" + ex.Message);
                }
            }
        }

        protected void gvCircularActionableList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                HiddenField hfStatus = (HiddenField)(e.Row.FindControl("hfStatus"));
                LinkButton lbCloseActionable = (LinkButton)(e.Row.FindControl("lbCloseActionable"));

                if (hfStatus.Value.Equals("C"))
                    lbCloseActionable.Visible = false;
                else
                    lbCloseActionable.Visible = true;
            }
        }
        //>>
    }
}