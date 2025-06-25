using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.App_Code;
using Fiction2Fact.F2FControls;

namespace Fiction2Fact.Projects.Admin
{
    public partial class ConfigParams : System.Web.UI.Page
    {
        UtilitiesBLL utilBL = new UtilitiesBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();
                lblMsg.Text = "";
            }
            else
            {
                lblMsg.Text = "";
            }
        }

        protected void BindGrid()
        {
            DataSet ds = utilBL.getDataset("getConfigParams", null);
            gvConfigueParams.DataSource = ds;
            gvConfigueParams.DataBind();
            Session["configParams"] = ds;
        }

        protected void gvConfigueParams_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strId = gvConfigueParams.SelectedValue.ToString();
            GridViewRow gvr = gvConfigueParams.Rows[gvConfigueParams.SelectedIndex];
            F2FTextBox txtValue = ((F2FTextBox)(gvr.FindControl("txtValue")));
            string strValue = txtValue.Text;
            utilBL.updateConfigParams(Convert.ToInt32(strId), strValue, null);
            lblMsg.Text = "Record has been updated successfully for Id: " + strId;
            BindGrid();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            gvConfigueParams.Columns[2].Visible = false;
            gvConfigueParams.DataSource = (DataSet)Session["configParams"];
            gvConfigueParams.DataBind();

            F2FExcelExport.F2FExportGridViewToExcel(gvConfigueParams, "configParams.xlsx", new int[] { 2 });
            
            gvConfigueParams.Columns[2].Visible = true;
        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        //protected void gvConfigueParams_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        //{
        //    string strParent;
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        strParent = ((Label)(e.Row.FindControl("lblParent"))).Text;
        //        if (strParent.Equals("0"))
        //        {
        //            ((F2FTextBox)(e.Row.FindControl("txtValue"))).Visible = false;
        //            ((LinkButton)(e.Row.FindControl("lbSave"))).Visible = false;
        //            ((Label)(e.Row.FindControl("lblName"))).Font.Bold = true;
        //            //e.Row.Cells[1].ColumnSpan = 3;
        //        }
        //    }
        //    e.Row.Cells[0].Visible = false;

        //}
    }
}