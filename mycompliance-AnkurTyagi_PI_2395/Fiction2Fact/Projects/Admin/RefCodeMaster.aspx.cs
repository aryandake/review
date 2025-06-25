using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using System.IO;
using System.Data.SqlClient;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Admin
{
    public partial class RefCodeMaster : Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        RefCodesBLL rcbBL = new RefCodesBLL();
        CommonMethods cm = new CommonMethods();
        UtilitiesBLL utilBL = new UtilitiesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvRefCode.ActiveViewIndex = 0;
                getReferenceDDL();
                ddlSearchRefType.Focus();
            }
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                lblMsg.Text = "";
                int intRefID = 0;
                string strRefType;
                string strRefName;
                string strRefCode;
                string strCreateBy, strSortOrder = "", strStatus = "";
                int intRefRowID;
                string strRefId;

                strRefId = lblID.Text.ToString();
                if (!strRefId.Equals(""))
                    intRefID = Convert.ToInt32(strRefId);

                //strRefType = cm.getSanitizedString(txtRefType.Text);
                strRefType = ddlRefType.SelectedValue;
                strRefName = cm.getSanitizedString(txtRefName.Text);
                strRefCode = cm.getSanitizedString(txtRefCode.Text);
                strSortOrder = cm.getSanitizedString(txtSortOrder.Text);
                strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                strStatus = ddlStatus.SelectedValue;

                intRefRowID = rcbBL.saveRefCode(intRefID, strRefType, strRefName, strRefCode, strCreateBy,
                    strSortOrder, strStatus);

                hfDoubleClickFlag.Value = "";
                updateReferenceGrid();
                writeError("Record saved successfully.");
                mvRefCode.ActiveViewIndex = 0;
            }
            catch (SqlException ex1)
            {
                if (ex1.Number.Equals(2601)) // unique key index
                {
                    writeError("Duplicate values are not allowed.");
                    F2FLog.Log(ex1, CommonCodes.GetCurrentUrlFileName(this), System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
            }
            catch (Exception exp)
            {
                hfDoubleClickFlag.Value = "";
                throw exp;
            }
        }

        protected void btnEditCancel_Click(object sender, System.EventArgs e)
        {
            mvRefCode.ActiveViewIndex = 0;
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            updateReferenceGrid();
            ddlSearchRefType.Focus();
        }

        private void updateReferenceGrid()
        {
            try
            {
                lblMsg.Text = "";
                lblID.Text = "";
                DataTable dtReference;
                string strRefID = null;
                string strRefType = "";
                if (!ddlSearchRefType.SelectedItem.Text.Equals("Select"))
                {
                    strRefType = ddlSearchRefType.SelectedItem.Text;
                }
                string strRefName = cm.getSanitizedString(txtSearchRefName.Text);
                string strRefCode = cm.getSanitizedString(txtSearchRefCode.Text);

                dtReference = rcbBL.searchRefCode(strRefID, strRefType.Replace("'", "''"), strRefName.Replace("'", "''"),
                    strRefCode.Replace("'", "''"), mstrConnectionString);

                Session["RefCode"] = dtReference;
                gvRefCode.DataSource = dtReference;
                gvRefCode.DataBind();

                if (gvRefCode.Rows.Count == 0)
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
                //writeError("Exception in updateReferenceGrid()" + exp);
                throw exp;
            }
        }
        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        protected void gvRefCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strRefID;
                DataTable dtReference = new DataTable();
                DataRow drReference;

                lblMsg.Text = "";
                if (hfSelectedOperation.Value == "Edit")
                {
                    mvRefCode.ActiveViewIndex = 1;

                    ddlRefType.DataSource = utilBL.getDataSet("getReferences");
                    ddlRefType.DataBind();
                    ddlRefType.Items.Insert(0, new ListItem("-- Select --", ""));

                    strRefID = gvRefCode.SelectedValue.ToString();
                    dtReference = rcbBL.searchRefCode(strRefID, "", "", "", mstrConnectionString);

                    if (dtReference.Rows.Count > 0)
                    {
                        drReference = dtReference.Rows[0];

                        lblID.Text = drReference["RC_ID"].ToString();
                        ddlRefType.SelectedValue = drReference["RC_TYPE"].ToString();
                        //txtRefType.Text = drReference["RC_TYPE"].ToString();
                        txtRefName.Text = drReference["RC_NAME"].ToString();
                        txtRefCode.Text = drReference["RC_CODE"].ToString();
                        txtSortOrder.Text = drReference["RC_SORT_ORDER"].ToString();
                        ddlStatus.SelectedValue = drReference["RC_STATUS"].ToString();

                        ddlRefType.Enabled = false;
                        txtRefCode.ReadOnly = true;
                    }
                }
                if (hfSelectedOperation.Value == "Delete")
                {
                    try
                    {
                        int intRefID = Convert.ToInt32(gvRefCode.SelectedDataKey.Value);
                        string strCreateBy = (new Authentication()).getUserFullName(Page.User.Identity.Name);
                        DataTable dt = new DataTable();

                        rcbBL.deleteRefCode(intRefID, strCreateBy, mstrConnectionString);
                        updateReferenceGrid();
                        writeError("Reference removed successfully.");
                    }
                    catch (Exception exp)
                    {
                        //writeError("Exception in gvRefCode_SelectedIndexChanged()" + exp.Message);
                        throw exp;
                    }
                }
            }
            catch (Exception exp)
            {
                //writeError("Exception in SelectedIndexChange()" + exp);
                throw exp;
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            mvRefCode.ActiveViewIndex = 0;
        }

        protected void btnAddReference_Click(object sender, System.EventArgs e)
        {
            lblMsg.Text = "";
            mvRefCode.ActiveViewIndex = 1;
            lblID.Text = "";
            //txtRefType.Text = "";
            ddlRefType.SelectedIndex = -1;
            txtRefName.Text = "";
            txtRefCode.Text = "";
            txtSortOrder.Text = "";
            ddlStatus.SelectedIndex = -1;
            //txtRefType.Focus();
            ddlRefType.Focus();

            ddlRefType.DataSource = utilBL.getDataset("getDistinctReferences", mstrConnectionString);
            ddlRefType.DataBind();
            ddlRefType.Items.Insert(0, new ListItem("-- Select --", ""));
        }

        private void getReferenceDDL()
        {
            ddlSearchRefType.DataSource = utilBL.getDataset("getDistinctReferences", mstrConnectionString);
            ddlSearchRefType.DataBind();
            ddlSearchRefType.Items.Insert(0, new ListItem("Select", ""));
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvRefCode.AllowPaging = false;
            gvRefCode.AllowSorting = false;
            gvRefCode.Columns[6].Visible = false;
            gvRefCode.DataSource = (DataTable)(Session["RefCode"]);
            gvRefCode.DataBind();
            PrepareGridViewForExport(gvRefCode);
            string attachment = "attachment; filename=ReferenceCodeMaster.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";

            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvRefCode.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
            gvRefCode.AllowPaging = true;
            gvRefCode.AllowSorting = true;
            gvRefCode.DataBind();
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
                    l.Text = (gv.Controls[i] as LinkButton).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    //gv.Controls.AddAt(i, l);
                }

                if (gv.Controls[i].HasControls())
                {
                    PrepareGridViewForExport(gv.Controls[i]);
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void gvRefCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRefCode.PageIndex = e.NewPageIndex;
            gvRefCode.DataSource = (DataTable)(Session["RefCode"]);
            gvRefCode.DataBind();
        }
    }
}