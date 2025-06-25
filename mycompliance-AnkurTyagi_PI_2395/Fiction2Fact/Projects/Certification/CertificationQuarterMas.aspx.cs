using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.Legacy_App_Code.DAL;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class CertificationQuarterMas : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        CertificationMasterBL certBL = new CertificationMasterBL();
        UtilitiesBLL utilBL = new UtilitiesBLL();
        UtilitiesBLL utilBLL = new UtilitiesBLL();
        UtilitiesVO utilVO = new UtilitiesVO();
        CommonMethods cm = new CommonMethods();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                    imgQuarterStarDate.ImageUrl = imgDueDt.ImageUrl = imgToDt.ImageUrl = ImageButton2.ImageUrl = ImageButton1.ImageUrl = Global.site_url("Content/images/legacy/calendar.jpg");
                mvQuarterMaster.ActiveViewIndex = 0;
            }
        }
        protected void cvQuarterEndDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }
        protected void cvQuarterEndDate1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }


        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            try
            {
                if (ddlStatus.SelectedValue.Equals("A"))
                {
                    int intNoOfActiveCnt = 0;
                    //Modify By Milan Yadav on 13-Oct-2016
                    //>>
                    //DataSet dsQuarter = utilBL.getDataset("CERTQUARTERS", strConnectionString);
                    DataTable dtQuarter = utilBL.getDatasetWithConditionInString("CERTACTIVEQUARTERS",
                        hfQuarterId.Value.ToString(), strConnectionString);
                    // DataTable dtQuarter = dsQuarter.Tables[0]; 
                    intNoOfActiveCnt = dtQuarter.Rows.Count;
                    if (intNoOfActiveCnt == 0)
                    {
                        saveQuarter();
                    }
                    else
                    {
                        writeError("Only 1 quarter will be kept active at a time. So Kindly inactive previous quarter and add new quarter.");
                        return;
                    }
                }
                else
                {
                    saveQuarter();
                }

            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("btnSave_Click :" + exp);
            }
        }
        private void saveQuarter()
        {
            try
            {
                string strFromDt = "", strToDt = "", strDueDt = "", strStatus = "";//, strCreatedBy = "",strID = "",
                int intretVal, intId = 0;
                string strCreateBy = Page.User.Identity.Name.ToString();
                DataSet dsQuarter = new DataSet();
                DataTable dtQuarter = new DataTable();

                if (!lblID.Text.Equals(""))
                    intId = Convert.ToInt32(lblID.Text);
                else
                    intId = 0;

                strFromDt = txtQuarterStartDate.Text.ToString();
                strToDt = txtQuarterEndDate.Text.ToString();
                strStatus = ddlStatus.SelectedValue.ToString();
                //Added By Milan Yadav on 13-Sep-2016
                //>>
                strDueDt = txtQuarterDueDate.Text.ToString();
                //<<
                if (lblID.Text.Equals(""))
                {
                    if (cm.checkDuplicate("TBL_CERT_QUARTER_MAS", "CQM_FROM_DATE", strFromDt, " AND CQM_TO_DATE = '" + strToDt + "' AND CQM_DUE_DATE = '" + strDueDt + "'") == true)
                    {
                        writeError("Duplicate entry. Please enter different quater details.");
                        return;
                    }
                }

                intretVal = certBL.saveCertificationQuarterMas(intId, strFromDt, strToDt, strDueDt, strStatus, strCreateBy, strConnectionString);


                if (!lblID.Text.Equals(""))
                {
                    writeError("Quarter updated successfully.");
                }
                else
                {
                    //Commented By Milan yadav on 29-Jun-2016
                    //>>

                    //dsQuarter = utilBL.getDataset("CERTQUARTERS",strConnectionString);
                    //dtQuarter = dsQuarter.Tables[0];
                    //strQuarter = dtQuarter.Rows[0]["Quarter"].ToString();
                    //          writeError("Quarter saved successfully.");
                }
                updateGridView();
                mvQuarterMaster.ActiveViewIndex = 0;
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("btnSave_Click :" + exp);
            }
        }

        private void sendCertificateMailAllDept(string strQuarter)
        {
            try
            {
                CommonCode cc = new CommonCode();
                Authentication auth = new Authentication();

                cc.ParamMap.Add("ConfigId", 15);
                cc.ParamMap.Add("To", "Level1AllDept");
                cc.ParamMap.Add("cc", "");
                cc.ParamMap.Add("SubmittedBy", auth.GetUserDetsByEmpCode(Page.User.Identity.Name).Split('|')[0]);
                cc.ParamMap.Add("CertDepartmentId", "");
                cc.ParamMap.Add("CertDepartment", "");
                cc.ParamMap.Add("Quarter", strQuarter);
                cc.setCertificationMailContent();
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in sendCertificateMail:" + exp);
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            lblMsg.Text = "";
            updateGridView();
        }

        private void updateGridView()
        {
            try
            {
                lblID.Text = "";
                string strID = "", strQuarterFromDt = "", strQuarterEndDt = "", strStatus = "";
                DataTable dt1;

                strQuarterFromDt = txtSearchQuarterStartDate.Text.ToString();
                strQuarterEndDt = txtSearchQuarterEndDate.Text.ToString();
                strStatus = ddlSearchStatus.SelectedValue.ToString();

                dt1 = certBL.searchCertificationQuarterMas(strID, strQuarterFromDt, strQuarterEndDt, strStatus, strConnectionString);

                Session["QuarterMaster"] = dt1;
                gvQuarterMaster.DataSource = dt1;
                gvQuarterMaster.DataBind();

                if (gvQuarterMaster.Rows.Count == 0)
                {
                    writeError("No Records Found Satisfying this Criteria.");
                    imgExcel.Visible = false;
                }
                else
                {
                    imgExcel.Visible = true;

                }
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in updateGridView()" + exp);
            }

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

        protected void gvQuarterMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strID = "", strQuarterFromDt = "", strQuarterEndDt = "", strStatus = "";
                DataTable dt2 = new DataTable();
                DataRow dr;

                lblMsg.Text = "";
                if (hfSelectedOperation.Value == "Edit")
                {
                    lblID.Visible = true;
                    strID = gvQuarterMaster.SelectedValue.ToString();
                    hfQuarterId.Value = gvQuarterMaster.SelectedValue.ToString();
                    dt2 = certBL.searchCertificationQuarterMas(strID, strQuarterFromDt, strQuarterEndDt, strStatus, strConnectionString);

                    dr = dt2.Rows[0];
                    mvQuarterMaster.ActiveViewIndex = 1;

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        lblID.Text = dr["CQM_ID"].ToString();
                        ddlStatus.SelectedValue = dr["CQM_STATUS"].ToString();


                        if (dr["CQM_TO_DATE"].ToString().Equals(""))
                            txtQuarterEndDate.Text = "";
                        else
                            txtQuarterEndDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dr["CQM_TO_DATE"]));


                        if (dr["CQM_FROM_DATE"].ToString().Equals(""))
                            txtQuarterStartDate.Text = "";
                        else
                            txtQuarterStartDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dr["CQM_FROM_DATE"]));
                        //Added By Milan Yadav on 13-Oct-2016
                        //>>
                        if (dr["CQM_DUE_DATE"].ToString().Equals(""))
                            txtQuarterDueDate.Text = "";
                        else
                            txtQuarterDueDate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dr["CQM_DUE_DATE"]));

                        //<<
                    }
                }
                else if (hfSelectedOperation.Value == "Delete")
                {
                    try
                    {
                        int intID = Convert.ToInt32(gvQuarterMaster.SelectedDataKey.Value);

                        // Added by Supriya on 22-May-2015
                        string sqlquery = "select * from TBL_CERTIFICATIONS where CERT_CQM_ID = '" + intID + "'";
                        DataTable dt = F2FDatabase.F2FGetDataTable(sqlquery);
                        if (dt.Rows.Count > 0)
                        {
                            writeError("Quarter cannot be deleted because there are certification(s) associated with it.");
                        }
                        else
                        {
                            utilVO.setCode(" CQM_ID = " + intID);
                            DataTable dt1 = utilBLL.getData("deleteCertificationQuarter", utilVO);
                            updateGridView();
                            writeError("Quarter deleted successfully.");
                        }

                    }
                    catch (Exception exp)
                    {
                        //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                        string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                        //>>
                        writeError("Exception in gvQuarterMaster_SelectedIndexChanged()" + exp.Message);
                    }
                }
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Exception in SelectedIndexChange()" + exp);
            }
        }
        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            mvQuarterMaster.ActiveViewIndex = 0;
        }

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            lblMsg.Text = "";
            mvQuarterMaster.ActiveViewIndex = 1;
            lblID.Text = "";
            txtQuarterStartDate.Text = "";
            txtQuarterEndDate.Text = "";
            ddlStatus.SelectedValue = "";
            hfQuarterId.Value = "";
            txtQuarterDueDate.Text = "";
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvQuarterMaster.AllowPaging = false;
            gvQuarterMaster.AllowSorting = false;
            gvQuarterMaster.DataSource = (DataTable)(Session["QuarterMaster"]);
            gvQuarterMaster.DataBind();
            CommonCodes.PrepareGridViewForExport(gvQuarterMaster);
            string attachment = "attachment; filename=QuarterMaster.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";

            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvQuarterMaster.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
            gvQuarterMaster.AllowPaging = true;
            gvQuarterMaster.AllowSorting = true;
            gvQuarterMaster.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }


        protected void gvQuarterMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvQuarterMaster.PageIndex = e.NewPageIndex;
            gvQuarterMaster.DataSource = (DataTable)(Session["QuarterMaster"]);
            gvQuarterMaster.DataBind();
        }
    }
}