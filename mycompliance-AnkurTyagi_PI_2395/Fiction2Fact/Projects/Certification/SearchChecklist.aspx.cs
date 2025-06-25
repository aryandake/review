using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Certification
{
    public partial class Certification_SearchChecklist : System.Web.UI.Page
    {
        string strConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ToString();
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CertificationMasterBL certBL = new CertificationMasterBL();
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();
        string strUser, strCreateBy;
        int intChecklistRowId = 0, intChecklistID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                strUser = Authentication.GetUserID(Page.User.Identity.Name);
                ListItem li = new ListItem();
                li.Text = "(Select)";
                li.Value = "";
                ddlSearchDeptName.DataSource = utilBL.getDatasetWithConditionInString("getCertDeptById", "", strConnectionString);
                ddlSearchDeptName.DataBind();
                ddlSearchDeptName.Items.Insert(0, li);
                if (!string.IsNullOrEmpty(Convert.ToString(Session["GridFilter"])))
                {
                    ddlSearchDeptName.SelectedValue = Convert.ToString(Session["GridFilter"]);
                    gvChecklist.DataSource = (DataTable)Session["GridData"];
                    gvChecklist.DataBind();
                }
            }
        }

        protected void gvChecklist_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string strId = null;
            try
            {
                strId = gvChecklist.SelectedValue.ToString();
                if (hfSelectedOperation.Value == "Edit")
                {
                    Response.Redirect(Global.site_url("Projects/Certification/ChecklistAddEdit.aspx?CID=") + encdec.Encrypt(strId), false);
                }
                else if (hfSelectedOperation.Value == "View")
                {
                    Response.Redirect(Global.site_url("Projects/Certification/ChecklistView.aspx?CID=") + encdec.Encrypt(strId), false);
                }
                else if (hfSelectedOperation.Value == "Copy")
                {
                    Response.Redirect(Global.site_url("Projects/Certification/ChecklistAddEdit.aspx?Type=Copy&CID=") + encdec.Encrypt(strId), false);
                }
                else if (hfSelectedOperation.Value == "Delete")
                {
                    DataTable dt_DeleteCL = new DataTable();
                    dt_DeleteCL = utilBL.getDatasetWithCondition("DeleteComplianceChecklist", Convert.ToInt32(strId), strConnectionString);
                    writeError("Checklist ID " + strId + " Deleted Sucessfully");
                    //BindChecklist();
                }
            }
            catch (Exception ex)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(ex, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("Error In Select ID" + ex);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        //public void BindChecklist()
        //{
        //    DataSet ds_checklist = new DataSet();
        //    DataTable dt_cl= new DataTable ();

        //    ds_checklist = utilBL.getDatasetWithConditionInString("getCertChecklistDetailById", 
        //        ddlSearchDeptName.SelectedValue.ToString(),  strConnectionString);
        //    dt_cl = ds_checklist.Tables[0];
        //    if (dt_cl.Rows.Count > 0)
        //    {
        //        gvChecklist.DataSource = dt_cl;
        //    }
        //    else
        //    {
        //        gvChecklist.DataSource = null;
        //    }
        //    gvChecklist.DataBind();
        //}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            DataTable dt_checklist = new DataTable();
            //dt_checklist = utilBL.getDatasetWithTwoConditionInString("getCertChecklistDetailById",
            //               ddlSearchDeptName.SelectedValue, "",  strConnectionString);
            //Added By Milan yadav on 01-June-2016
            //>>
            dt_checklist = certBL.GetCertChecklistDetailById(ddlSearchDeptName.SelectedValue, "", strConnectionString);
            //<<
            if (dt_checklist.Rows.Count > 0)
            {
                pnlDeactivate.Visible = true;
                gvChecklist.DataSource = dt_checklist;
                Session["GridData"] = dt_checklist;
                Session["GridFilter"] = ddlSearchDeptName.SelectedValue;
            }
            else
            {
                gvChecklist.DataSource = null;
            }
            gvChecklist.DataBind();
        }
        public void getChecklist()
        {
            DataTable dtchecklistBind = new DataTable();
            dtchecklistBind = certBL.GetCertChecklistDetailById(ddlSearchDeptName.SelectedValue, "", strConnectionString);
            if (dtchecklistBind.Rows.Count > 0)
            {
                pnlDeactivate.Visible = true;
                gvChecklist.DataSource = dtchecklistBind;
                Session["GridData"] = dtchecklistBind;
                gvChecklist.DataBind();
                txtEffectiveToDate.Text = "";
                txtRemarks.Text = "";
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(Global.site_url("Projects/Certification/ChecklistAddEdit.aspx"), false);
        }

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            strCreateBy = Authentication.GetUserID(Page.User.Identity.Name);
            GridViewRow gvr;

            for (int intIndex = 0; intIndex < gvChecklist.Rows.Count; intIndex++)
            {
                gvr = gvChecklist.Rows[intIndex];
                CheckBox RowLevelCheckBox = (CheckBox)(gvr.FindControl("RowLevelCheckBox"));
                if (RowLevelCheckBox.Checked)
                {
                    HiddenField hfChecklistMasId = (HiddenField)(gvr.FindControl("hfChecklistMasId"));
                    if (!hfChecklistMasId.Value.Equals(null))
                    {
                        intChecklistID = Convert.ToInt32(hfChecklistMasId.Value);
                    }
                    intChecklistRowId = certBL.deActivateCertificationChecklist(intChecklistID, "I", txtEffectiveToDate.Text, strCreateBy, txtRemarks.Text, strConnectionString);
                }
            }
            writeError("Checklist Deactivated Successfully...");
            getChecklist();
        }

        protected void gvChecklist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkViewCirc = (LinkButton)(e.Row.FindControl("lnkViewCirc"));
                HiddenField hfCircularId = (HiddenField)(e.Row.FindControl("hfCircularId"));

                if (hfCircularId.Value.Equals(""))
                {
                    lnkViewCirc.Visible = false;
                }
                else
                {
                    lnkViewCirc.Visible = true;
                }

                lnkViewCirc.OnClientClick = "onClientViewCircClick('" + (new SHA256EncryptionDecryption()).Encrypt(hfCircularId.Value) + "');";
            }
        }

        //Added By Milan Yadav on 27-Sep-2016
        //>>
        protected void gvChecklist_DataBound(object sender, EventArgs e)
        {
            if (gvChecklist.Rows.Count == 0)
            {
                return;
            }
            CheckBox HeaderLevelCheckBox = (CheckBox)(gvChecklist.HeaderRow.FindControl("HeaderLevelCheckBox"));
            HeaderLevelCheckBox.Attributes["onclick"] = "ChangeAllCheckBoxStates(this.checked);";
            List<string> ArrayValues = new List<string>();
            ArrayValues.Add(String.Concat("'", HeaderLevelCheckBox.ClientID, "'"));

            GridViewRow gvr;
            for (int intIndex = 0; intIndex < gvChecklist.Rows.Count; intIndex++)
            {
                string strId = Convert.ToString(intIndex);
                gvr = gvChecklist.Rows[intIndex];
                CheckBox RowLevelCheckBox = (CheckBox)(gvr.FindControl("RowLevelCheckBox"));
                RowLevelCheckBox.Attributes["onclick"] = "onRowCheckedUnchecked('" + RowLevelCheckBox.ClientID + "');";
                ArrayValues.Add(string.Concat("'", RowLevelCheckBox.ClientID, "'"));
            }

            CheckBoxIDsArray.Text = ("<script type=\"text/javascript\">" + ("\r\n" + ("<!--" + ("\r\n"
                        + (string.Concat("var CheckBoxIDs =  new Array(", string.Join(",", ArrayValues.ToArray()), ");")
                        + ("\r\n" + ("// -->" + ("\r\n" + "</script>"))))))));
        }
        //<<
        //Start - added by Hari on 12 Oct 2016
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            int uniqueRowId = 0;
            string strChecklistTable = "", strHtmlTableChecklistDetsRows = "";
            DataTable dtChecklistDets;
            DataRow drChecklistDets;

            dtChecklistDets = (DataTable)Session["GridData"];

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
                           //" <th class='tabhead' align='center'> " +
                           //" Department Name " +
                           //" </th> " +
                           " <th class='tabhead' align='center'> " +
                          " Department Name" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Reference Circular/Notification/Act" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Section/Clause" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Compliance of/Heading of Compliance checklist" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Description" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Consequences of non Compliance" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Frequency" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Effective From" +
                          " </th> " +
                           " <th class='tabhead' align='center'> " +
                          " Effective To" +
                          " </th> " +
                           " <th class='tabhead' align='center'> " +
                          " Status" +
                          " </th> " +
                           " <th class='tabhead' align='center'> " +
                          " Deactivation Remarks" +
                          " </th> " +
                          " </tr> " +
                          " </thead> ";
            int intChecklistDetsCnt = dtChecklistDets.Rows.Count;
            for (int intCnt = 0; intCnt < intChecklistDetsCnt; intCnt++)
            {
                uniqueRowId = uniqueRowId + 1;
                drChecklistDets = dtChecklistDets.Rows[intCnt];

                strHtmlTableChecklistDetsRows = strHtmlTableChecklistDetsRows + "<tr>" +
                "<td>" + uniqueRowId +
                //"<td>" + drChecklistDets["CCM_ID"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CSSDM_NAME"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_REFERENCE"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_CLAUSE"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_CHECK_POINTS"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_PARTICULARS"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_PENALTY"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_FREQUENCY"].ToString() + "</td>" +
                "<td>" + drChecklistDets["Effective From"].ToString() + "</td>" +
                "<td>" + drChecklistDets["Effective To"].ToString() + "</td>" +
                "<td>" + drChecklistDets["Status"].ToString() + "</td>" +
                "<td>" + drChecklistDets["Remark"].ToString() + "</td>" +
                "</tr>";
            }
            strChecklistTable = strHtmlTable + strHtmlTableChecklistDetsRows + "</table> " +
            "</BODY>" +
            "</HTML>";
            //<<Modified by Ankur Tyagi on 16-May-2024 for CR_2070
            string attachment = "";
            if (ddlSearchDeptName.SelectedIndex > 0)
            {
                attachment = "attachment; filename=" + ddlSearchDeptName.SelectedItem.Text + "_Checklist_Details.xls";
            }
            else
            {
                attachment = "attachment; filename=All_Checklist_Details.xls";
            }
            //>>
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write(strChecklistTable.ToString());
            Response.End();
        }
        //End - added by Hari on 12 Oct 2016

        //<<Added by Ankur Tyagi on 16-May-2024 for CR_2070
        protected void lnkExportAllData_Click(object sender, EventArgs e)
        {
            int uniqueRowId = 0;
            string strChecklistTable = "", strHtmlTableChecklistDetsRows = "";
            DataTable dtChecklistDets;
            DataRow drChecklistDets;

            dtChecklistDets = certBL.GetCertChecklistDetailById("", "", strConnectionString);

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
                           //" <th class='tabhead' align='center'> " +
                           //" Department Name " +
                           //" </th> " +
                           " <th class='tabhead' align='center'> " +
                          " Department Name" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Reference Circular/Notification/Act" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Section/Clause" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Compliance of/Heading of Compliance checklist" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Description" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Consequences of non Compliance" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Frequency" +
                          " </th> " +
                          " <th class='tabhead' align='center'> " +
                          " Effective From" +
                          " </th> " +
                           " <th class='tabhead' align='center'> " +
                          " Effective To" +
                          " </th> " +
                           " <th class='tabhead' align='center'> " +
                          " Status" +
                          " </th> " +
                           " <th class='tabhead' align='center'> " +
                          " Deactivation Remarks" +
                          " </th> " +
                          " </tr> " +
                          " </thead> ";
            int intChecklistDetsCnt = dtChecklistDets.Rows.Count;
            for (int intCnt = 0; intCnt < intChecklistDetsCnt; intCnt++)
            {
                uniqueRowId = uniqueRowId + 1;
                drChecklistDets = dtChecklistDets.Rows[intCnt];

                strHtmlTableChecklistDetsRows = strHtmlTableChecklistDetsRows + "<tr>" +
                "<td>" + uniqueRowId +
                //"<td>" + drChecklistDets["CCM_ID"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CSSDM_NAME"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_REFERENCE"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_CLAUSE"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_CHECK_POINTS"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_PARTICULARS"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_PENALTY"].ToString() + "</td>" +
                "<td>" + drChecklistDets["CCM_FREQUENCY"].ToString() + "</td>" +
                "<td>" + drChecklistDets["Effective From"].ToString() + "</td>" +
                "<td>" + drChecklistDets["Effective To"].ToString() + "</td>" +
                "<td>" + drChecklistDets["Status"].ToString() + "</td>" +
                "<td>" + drChecklistDets["Remark"].ToString() + "</td>" +
                "</tr>";
            }
            strChecklistTable = strHtmlTable + strHtmlTableChecklistDetsRows + "</table> " +
            "</BODY>" +
            "</HTML>";
            
            string  attachment = "attachment; filename=All_Checklist_Details.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            Response.Write(strChecklistTable.ToString());
            Response.End();
        }
        //>>

    }
}