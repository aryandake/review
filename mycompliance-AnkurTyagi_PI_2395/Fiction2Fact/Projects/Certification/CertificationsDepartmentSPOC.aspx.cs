using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code.Certification.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using Fiction2Fact.App_Code.BLL;

namespace Fiction2Fact.Projects.Certification
{
    public partial class CertificationsDepartmentSPOC : System.Web.UI.Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        CertificationMasterBL certBL = new CertificationMasterBL();
        UtilitiesBLL utilBL = new UtilitiesBLL();
        CommonMethods cm = new CommonMethods();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvCertDeptMaster.ActiveViewIndex = 0;

                ListItem li = new ListItem();
                li.Text = "(Select)";
                li.Value = "";
                ddlSearchCertDept.DataSource = utilBL.getDataset("CERTIFICATEDEPT", mstrConnectionString);
                ddlSearchCertDept.DataBind();
                ddlSearchCertDept.Items.Insert(0, li);

                txtCertDeptUserId.Attributes["onchange"] = "populateUserDetsByCode('CertUnitHeadEmpCode','0')";
            }
        }

        private DataTable getMembdersDetsdt()
        {
            string strMembdersDetails = hfMembersDetsData.Value;
            string[] strarrMembdersDetails, strarrFields;
            string strTemp;
            DataRow dr;

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", typeof(string)));
            dt.Columns.Add(new DataColumn("DeptName", typeof(string)));
            dt.Columns.Add(new DataColumn("UserId", typeof(string)));
            dt.Columns.Add(new DataColumn("UserName", typeof(string)));
            dt.Columns.Add(new DataColumn("EmailId", typeof(string)));

            strarrMembdersDetails = strMembdersDetails.Split('~');
            for (int i = 0; i < strarrMembdersDetails.Length - 1; i++)
            {
                strTemp = strarrMembdersDetails[i];
                strarrFields = strTemp.Split('|');
                dr = dt.NewRow();
                dr["Id"] = strarrFields[0];
                dr["DeptName"] = strarrFields[1];
                dr["UserId"] = strarrFields[2];
                dr["EmailId"] = strarrFields[3];
                dr["UserName"] = strarrFields[4];
                dt.Rows.Add(dr);
            }
            return dt;
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            try
            {
                int intretVal, intId;
                string strCertDept = "", strUserId = "", strUserName = "", strEmailId = "", strCertSubDeptName = "";
                string strCreateBy = Page.User.Identity.Name.ToString();

                if (!lblID.Text.Equals(""))
                    intId = Convert.ToInt32(lblID.Text);
                else
                    intId = 0;
                //<< Modified by ramesh more on 14-Mar-2024 CR_1991 for Sql Injection
                strCertDept = ddlCertDept.SelectedValue.ToString();
                strCertSubDeptName = cm.getSanitizedString(txtCertDeptName.Text.ToString());
                strUserId = cm.getSanitizedString(txtCertDeptUserId.Text.ToString());
                strUserName = cm.getSanitizedString(txtCertDeptUserName.Text.ToString());
                strEmailId = cm.getSanitizedString(txtCertDeptEmailID.Text.ToString());
                //>>
                if (lblID.Text.Equals(""))
                {
                    if (cm.checkDuplicate("TBL_CERT_SUB_DEPT_MAS", "CSDM_NAME", strCertSubDeptName, " AND CSDM_CDM_ID = '" + strCertDept + "'") == true)
                    {
                        writeError("Duplicate entry. Please enter different certifying unit.");
                        return;
                    }
                }

                intretVal = certBL.saveSubCertificationDeptMas(intId, strCertDept, strCertSubDeptName, strUserId, strUserName, strEmailId, strCreateBy, getMembdersDetsdt(), mstrConnectionString);

                if (!intretVal.Equals(0))
                    writeError("Details save successfully.");
                else
                    writeError("Details updated successfully.");

                updateGridView();
                mvCertDeptMaster.ActiveViewIndex = 0;
            }
            catch (Exception exp)
            {
                //<<Added by Ankur Tyagi on 08-July-2024 for CR_2114
                string sMessage = F2FLog.Log(exp, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                //>>
                writeError("btnSave_Click :" + exp);
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            lblMsg.Text = "";
            updateGridView();

        }

        private void updateGridView()
        {
            try
            {
                lblID.Text = "";
                DataTable dt1;

                //dt1 = utilBL.getDatasetWithThreeConditionInString("getCertSubDeptById",
                //        ddlSearchCertDept.SelectedValue.ToString(), "", txtSearchCertDeptName.Text.ToString(), mstrConnectionString);
                dt1 = (new CertUtilitiesBLL()).GetDataTable("getCertSubDeptById"
                    , new DBUtilityParameter("CDM_ID", ddlSearchCertDept.SelectedValue.ToString())
                    , new DBUtilityParameter("CSDM_NAME", txtSearchCertDeptName.Text.ToString(), "like"));


                Session["CertDeptMaster"] = dt1;
                gvCertDeptMaster.DataSource = dt1;
                gvCertDeptMaster.DataBind();

                if (gvCertDeptMaster.Rows.Count == 0)
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
        }

        protected void gvCertDeptMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strID;
                DataTable dt2 = new DataTable();
                DataRow dr;
                string strCSSClass = "tabbody3";
                string strHtmlTableMembersDetailsRows = "";
                int uniqueRowId = 0;

                lblMsg.Text = "";
                if (hfSelectedOperation.Value == "Edit")
                {
                    string strHtmlTable = "<table id='tblMembersDets' class='table table-bordered footable' width='100%'> " +
                              " <thead> " +
                              " <tr> " +
                              " <th class='tabhead3' align='center'> " +
                              " <input type='checkbox' ID='HeaderLevelCheckBoxMembersDets' onclick = 'return onMembersDetsHeaderRowChecked()'/> " +
                              " </th>  " +
                              " <th class='tabhead3' align='center'> " +
                              " Sub-unit Name * " +
                              " </th> " +
                              " <th class='tabhead3' align='center'> " +
                              " SPOC NT Id *" +
                              " </th> " +
                              " <th class='tabhead3' align='center'> " +
                              " SPOC Name *" +
                              " </th> " +
                              " <th class='tabhead3' align='center'> " +
                              " SPOC Email Id *" +
                              " </th> " +
                              " </tr> " +
                              " </thead> ";

                    lblID.Visible = true;
                    strID = gvCertDeptMaster.SelectedValue.ToString();

                    dt2 = utilBL.getDatasetWithThreeConditionInString("getCertSubDeptById",
                         "", strID, "", mstrConnectionString);

                    //dt2 = (new CertUtilitiesBLL()).GetDataTable("getCertSubDeptById"
                    //, new DBUtilityParameter("CSDM_ID", strID));

                    if (dt2.Rows.Count > 0)
                    {
                        dr = dt2.Rows[0];
                        mvCertDeptMaster.ActiveViewIndex = 1;

                        ListItem li = new ListItem();
                        li.Text = "(Select)";
                        li.Value = "";
                        ddlCertDept.DataSource = utilBL.getDataset("CERTIFICATEDEPT", mstrConnectionString);
                        ddlCertDept.DataBind();
                        ddlCertDept.Items.Insert(0, li);

                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            lblID.Text = dr["CSDM_ID"].ToString();
                            ddlCertDept.SelectedValue = dr["CSDM_CDM_ID"].ToString();
                            txtCertDeptName.Text = dr["CSDM_NAME"].ToString();
                            txtCertDeptUserId.Text = dr["CSDM_USER_ID"].ToString();
                            txtCertDeptUserName.Text = dr["CSDM_EMP_NAME"].ToString();
                            txtCertDeptEmailID.Text = dr["CSDM_EMAIL_ID"].ToString();
                        }

                        DataTable dtMembersDetails = utilBL.getDatasetWithCondition("getSubSubCertDetsByCertSubId", Convert.ToInt32(lblID.Text), mstrConnectionString);
                        DataRow drMembersDetails;
                        int intMembersDetailsCnt = dtMembersDetails.Rows.Count;
                        for (int intCnt = 0; intCnt < intMembersDetailsCnt; intCnt++)
                        {
                            uniqueRowId = uniqueRowId + 1;
                            drMembersDetails = dtMembersDetails.Rows[intCnt];

                            strHtmlTableMembersDetailsRows = strHtmlTableMembersDetailsRows + "<tr><td class='" + strCSSClass + "'>" +
                                "<input type='hidden' ID='uniqueRowId" + uniqueRowId + "' value='" + uniqueRowId + "' /><input type='checkbox' ID='cbMembersDets" + uniqueRowId + "' />" +
                                "<input type='hidden' ID='txtMembersDetsId" + uniqueRowId + "'value='" + drMembersDetails["CSSDM_ID"].ToString() + "' /></td>" +
                                "<td class='" + strCSSClass + "'><input type='text' ID='txtDeptName" + uniqueRowId + "' maxLength = '200' size='30' class = 'form-control' value='" + drMembersDetails["CSSDM_NAME"].ToString() + "'/></td>" +
                                "<td class='" + strCSSClass + "'><input type='text' ID='txtUserId" + uniqueRowId + "' maxLength = '100' size='30' class = 'form-control' value='" + drMembersDetails["CSSDM_USER_ID"].ToString() + "'/></td>" +
                                "<td class='" + strCSSClass + "'><input type='text' ID='txtUserName" + uniqueRowId + "' maxLength = '100' size='30' class = 'form-control' value='" + drMembersDetails["CSSDM_EMP_NAME"].ToString() + "'/></td>" +
                                "<td class='" + strCSSClass + "'><input type='text' ID='txtEmailId" + uniqueRowId + "' maxLength = '100' size='30' class = 'form-control' value='" + drMembersDetails["CSSDM_EMAIL_ID"].ToString() + "'/></td>" +
                                "</tr>";
                        }

                        litMembersDetails.Text = strHtmlTable + strHtmlTableMembersDetailsRows + "</table> ";
                    }

                }
                else if (hfSelectedOperation.Value == "Delete")
                {
                    try
                    {
                        int intID = Convert.ToInt32(gvCertDeptMaster.SelectedDataKey.Value);

                        // Added by Supriya on 22-May-2015
                        string sqlquery = "select * from TBL_CERTIFICATIONS " +
                                " inner join  TBL_CERT_SUB_SUB_DEPT_MAS on CERT_CSSDM_ID=  CSSDM_ID " +
                                " inner join TBL_CERT_SUB_DEPT_MAS on CSSDM_CSDM_ID = CSDM_ID where CSDM_ID = '" + intID + "'";
                        DataTable dt = F2FDatabase.F2FGetDataTable(sqlquery);

                        if (dt.Rows.Count > 0)
                        {
                            writeError("Details cannot be deleted because there are certification(s) associated with it.");
                        }
                        else
                        {
                            DataTable dt1 = utilBL.getDatasetWithConditionInString("DeleteSubCertDept", gvCertDeptMaster.SelectedDataKey.Value.ToString(), mstrConnectionString);

                            updateGridView();
                            writeError("Details deleted successfully.");
                        }
                    }
                    catch (Exception exp)
                    {
                        throw;
                        writeError("Exception in gvCertDeptMaster_SelectedIndexChanged()" + exp.Message);
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
            mvCertDeptMaster.ActiveViewIndex = 0;
        }

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            lblMsg.Text = "";
            mvCertDeptMaster.ActiveViewIndex = 1;
            lblID.Text = "";
            txtCertDeptName.Text = "";
            txtCertDeptUserId.Text = "";
            txtCertDeptUserName.Text = "";
            txtCertDeptEmailID.Text = "";

            ListItem li = new ListItem();
            li.Text = "(Select)";
            li.Value = "";
            ddlCertDept.DataSource = utilBL.getDataset("CERTIFICATEDEPT", mstrConnectionString);
            ddlCertDept.DataBind();
            ddlCertDept.Items.Insert(0, li);


            litMembersDetails.Text = "<table id='tblMembersDets' class='table table-bordered footable' width='100%'> " +
                              " <thead> " +
                              " <tr> " +
                              " <th class='tabhead3' align='center'> " +
                              " <input type='checkbox' ID='HeaderLevelCheckBoxMembersDets' onclick = 'return onMembersDetsHeaderRowChecked()'/> " +
                              " </th>  " +
                              " <th class='tabhead3' align='center'> " +
                              " Sub-unit Name * " +
                              " </th> " +
                              " <th class='tabhead3' align='center'> " +
                              " SPOC NT Id *" +
                              " </th> " +
                              " <th class='tabhead3' align='center'> " +
                              " SPOC Name *" +
                              " </th> " +
                              " <th class='tabhead3' align='center'> " +
                              " SPOC Email Id *" +
                              " </th> " +
                              " </tr> " +
                              " </thead> " +
                              " </table> ";
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvCertDeptMaster.AllowPaging = false;
            gvCertDeptMaster.AllowSorting = false;
            //This 4 & 5 is Edit and Delete visible false 
            gvCertDeptMaster.Columns[6].Visible = false;
            gvCertDeptMaster.Columns[7].Visible = false;
            gvCertDeptMaster.DataSource = (DataTable)(Session["CertDeptMaster"]);
            gvCertDeptMaster.DataBind();
            CommonCodes.PrepareGridViewForExport(gvCertDeptMaster);
            string attachment = "attachment; filename=CertDeptMaster.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";

            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvCertDeptMaster.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
            gvCertDeptMaster.AllowPaging = true;
            gvCertDeptMaster.AllowSorting = true;
            gvCertDeptMaster.Columns[6].Visible = true;
            gvCertDeptMaster.Columns[7].Visible = true;
            gvCertDeptMaster.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }


        protected void gvCertDeptMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCertDeptMaster.PageIndex = e.NewPageIndex;
            gvCertDeptMaster.DataSource = (DataTable)(Session["CertDeptMaster"]);
            gvCertDeptMaster.DataBind();
        }
    }
}