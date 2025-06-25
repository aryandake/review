using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Admin
{
    public partial class RefCodeMaster_Old : Page
    {
        string mstrConnectionString = ConfigurationManager.ConnectionStrings["MsSQL"].ConnectionString;
        RefCodesBLL rcbBL = new RefCodesBLL();
        UtilitiesBLL utilBL = new UtilitiesBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvRefCode.ActiveViewIndex = 0;
                getReferenceDDL();
                Session["RefCodeMaster"] = Server.UrlEncode(DateTime.Now.ToString());
            }
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            try
            {
                if (ViewState["RefCodeMaster"].ToString() == Session["RefCodeMaster"].ToString())
                {
                    lblMsg.Text = "";
                    int intRefID = 0;
                    string strRefType;
                    string strRefName;
                    string strRefCode;
                    string strCreateBy;
                    int intRefRowID;
                    string strRefId;

                    strRefId = lblID.Text.ToString();
                    if (!strRefId.Equals(""))
                        intRefID = Convert.ToInt32(strRefId);


                    strRefType = txtRefType.Text;
                    strRefName = txtRefName.Text;
                    strRefCode = txtRefCode.Text;

                    strCreateBy = Convert.ToString(Page.User.Identity.Name);

                    intRefRowID = rcbBL.saveRefCode(intRefID, strRefType, strRefName, strRefCode,
                                             strCreateBy, mstrConnectionString);

                    updateReferenceGrid();
                    writeError("Record Saved Successfully.");

                    Session["RefCodeMaster"] = Server.UrlEncode(DateTime.Now.ToString());
                }
                else
                {
                    updateReferenceGrid();
                    writeError("Your attempt to refresh the page was blocked as it would lead to duplication of data.");
                }
                mvRefCode.ActiveViewIndex = 0;
            }
            catch (Exception exp)
            {
                writeError("Exception in btnSave_Click :" + exp);
            }
        }

        protected void btnEditCancel_Click(object sender, System.EventArgs e)
        {
            mvRefCode.ActiveViewIndex = 0;
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            updateReferenceGrid();
        }

        private void updateReferenceGrid()
        {
            try
            {
                lblMsg.Text = "";
                lblID.Text = "";
                DataTable dtReference;
                string strRefID = null;
                string strRefType = ddlSearchRefType.Text;
                string strRefName = txtSearchRefName.Text;
                string strRefCode = txtSearchRefCode.Text;

                dtReference = rcbBL.searchRefCode(strRefID, strRefType.Replace("'", "''"), strRefName.Replace("'", "''"),
                    strRefCode.Replace("'", "''"), mstrConnectionString);

                gvRefCode.DataSource = dtReference;
                Session["RefCode"] = dtReference;
                gvRefCode.DataBind();

                if (gvRefCode.Rows.Count == 0)
                {
                    writeError("No Records Found Satisfying this Criteria.");

                }
            }
            catch (Exception exp)
            {
                writeError("Exception in updateReferenceGrid()" + exp);
            }
        }

        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            //lblMsg.Visible = true;
            string script = "\r\n<script language=\"javascript\">\r\n" +
                       " alert('" + strError.Replace("'", "\\'") + "');" +
                       "   </script>\r\n";

            ClientScript.RegisterStartupScript(this.GetType(), "script", script);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["RefCodeMaster"] = Session["RefCodeMaster"];
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
                    getReferenceDDL();

                    strRefID = gvRefCode.SelectedValue.ToString();
                    dtReference = rcbBL.searchRefCode(strRefID, "", "", "", mstrConnectionString);
                    drReference = dtReference.Rows[0];
                    mvRefCode.ActiveViewIndex = 1;

                    for (int i = 0; i < dtReference.Rows.Count; i++)
                    {
                        lblID.Text = drReference["RC_ID"].ToString();
                        txtRefType.Text = drReference["RC_TYPE"].ToString();
                        txtRefName.Text = drReference["RC_NAME"].ToString();
                        txtRefCode.Text = drReference["RC_CODE"].ToString();
                    }

                }
                if (hfSelectedOperation.Value == "Delete")
                {
                    try
                    {
                        int intRefID = Convert.ToInt32(gvRefCode.SelectedDataKey.Value);
                        string strCreateBy = Convert.ToString(Page.User.Identity.Name);
                        DataTable dt = new DataTable();


                        rcbBL.deleteRefCode(intRefID, strCreateBy, mstrConnectionString);
                        updateReferenceGrid();
                        writeError("Reference Removed Successfully.");
                    }
                    catch (Exception exp)
                    {
                        writeError("Exception in gvRefCode_SelectedIndexChanged()" + exp.Message);
                    }
                }
            }
            catch (Exception exp)
            {
                writeError("Exception in SelectedIndexChange()" + exp);
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
            txtRefType.Text = "";
            txtRefName.Text = "";
            txtRefCode.Text = "";

        }

        private void getReferenceDDL()
        {
            ddlSearchRefType.DataSource = utilBL.getDataSet("getReferences");
            ddlSearchRefType.DataBind();
            ddlSearchRefType.Items.Insert(0, new ListItem("Select", ""));
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            CommonCodes.PrepareGridViewForExport(gvRefCode);
            string attachment = "attachment; filename=RefCodes.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";

            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvRefCode.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();

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