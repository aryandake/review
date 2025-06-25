using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Fiction2Fact.Legacy_App_Code.Compliance.BLL;

namespace Fiction2Fact.Projects.Compliance
{
    public partial class ReviewerMaster : System.Web.UI.Page
    {
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mvReviewMaster.ActiveViewIndex = 0;
                FillGrid();

                txtRId.Attributes["onchange"] = " return populateUserDetsByCode('L0Reviewer','0')";
                txtL1RId.Attributes["onchange"] = " return populateUserDetsByCode('L1Reviewer','0')";
                txtL2RId.Attributes["onchange"] = " return populateUserDetsByCode('L2Reviewer','0')";
            }
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            int intCRM_ID = 0;
            string strCRM_L0_REVIEWER_NT_ID = "", strCRM_L0_REVIEWER_NAME = "", strCRM_L0_REVIEWER_EMAIL = "",
            strCRM_L1_REVIEWER_NT_ID = "", strCRM_L1_REVIEWER_NAME = "", strCRM_L1_REVIEWER_EMAIL = "", strCRM_L2_REVIEWER_NT_ID = "",
            strCRM_L2_REVIEWER_NAME = "", strCRM_L2_REVIEWER_EMAIL = "", strCRM_STATUS = "", strloggeduser = "";


            strCRM_L0_REVIEWER_EMAIL = txtREmail.Text;
            strCRM_L0_REVIEWER_NAME = txtRName.Text;
            strCRM_L0_REVIEWER_NT_ID = txtRId.Text;
            strCRM_L1_REVIEWER_EMAIL = txtL1REmail.Text;
            strCRM_L1_REVIEWER_NAME = txtL1RName.Text;
            strCRM_L1_REVIEWER_NT_ID = txtL1RId.Text;
            strCRM_L2_REVIEWER_EMAIL = txtL2REmail.Text;
            strCRM_L2_REVIEWER_NAME = txtL2RName.Text;
            strCRM_L2_REVIEWER_NT_ID = txtL2RId.Text;
            strCRM_STATUS = ddlStatus.SelectedItem.Value;
            strloggeduser = Page.User.Identity.Name;

            if (string.IsNullOrEmpty(hfSelectedOperation.Value))
            {
                if (!check_reviewername(txtRName.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MYaert", "alert('Reviewer already exists.');", true);
                    txtRName.Focus();
                    return;
                }

                int x = oBLL.Save_Reviewer_Master(intCRM_ID, strCRM_L0_REVIEWER_NT_ID, strCRM_L0_REVIEWER_NAME, strCRM_L0_REVIEWER_EMAIL,
            strCRM_L1_REVIEWER_NT_ID, strCRM_L1_REVIEWER_NAME, strCRM_L1_REVIEWER_EMAIL, strCRM_L2_REVIEWER_NT_ID, strCRM_L2_REVIEWER_NAME
           , strCRM_L2_REVIEWER_EMAIL, strCRM_STATUS, strloggeduser, null);
                if (x > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mYalert", "alert('Reviewer added successfully..');", true);
                }
            }
            else
            {
                intCRM_ID = Convert.ToInt32(hfId.Value);
                int x = oBLL.Save_Reviewer_Master(intCRM_ID, strCRM_L0_REVIEWER_NT_ID, strCRM_L0_REVIEWER_NAME, strCRM_L0_REVIEWER_EMAIL,
          strCRM_L1_REVIEWER_NT_ID, strCRM_L1_REVIEWER_NAME, strCRM_L1_REVIEWER_EMAIL, strCRM_L2_REVIEWER_NT_ID, strCRM_L2_REVIEWER_NAME
         , strCRM_L2_REVIEWER_EMAIL, strCRM_STATUS, null, strloggeduser);
                if (x > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mYalert", "alert('Reviewer updated successfully..');", true);
                }
            }
            ClearControls();
        }

        bool check_reviewername(string reviewername)
        {
            bool retval;
            if (string.IsNullOrEmpty(reviewername))
            {
                retval = false;
            }
            else
            {
                DataTable dt = new DataTable();
                dt = oBLL.Search_Reviewer_Master(0,null,null,null, null, strFilter1: " and CRM_L0_REVIEWER_NAME='" + reviewername + "'");
                if (dt.Rows.Count > 0)
                {
                    retval = false;
                }
                else
                {
                    retval = true;
                }
            }
            return retval;
        }
        void ClearControls()
        {
            txtL1REmail.Text = string.Empty;
            txtL1RId.Text = string.Empty;
            txtL1RName.Text = string.Empty;
            txtL2REmail.Text = string.Empty;
            txtL2RId.Text = string.Empty;
            txtL2RName.Text = string.Empty;
            txtREmail.Text = string.Empty;
            txtRId.Text = string.Empty;
            txtRName.Text = string.Empty;
            ddlStatus.SelectedIndex = 0;
            hfSelectedOperation.Value = string.Empty;
            hfId.Value = string.Empty;
            hfDoubleClickFlag.Value = string.Empty;
            FillGrid();
            mvReviewMaster.ActiveViewIndex = 0;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        protected void gvReviewerMas_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strid = gvReviewerMas.SelectedValue.ToString();
            if (hfSelectedOperation.Value.ToLower() == "edit")
            {
                DataTable dt = new DataTable();
                dt = oBLL.Search_Reviewer_Master(Convert.ToInt32(strid), null, null, null, null);
                if (dt.Rows.Count > 0)
                {
                    hfId.Value = dt.Rows[0]["CRM_ID"].ToString();
                    txtREmail.Text = dt.Rows[0]["CRM_L0_REVIEWER_EMAIL"].ToString();
                    txtRName.Text = dt.Rows[0]["CRM_L0_REVIEWER_NAME"].ToString();
                    txtRId.Text = dt.Rows[0]["CRM_L0_REVIEWER_NT_ID"].ToString();
                    txtL1REmail.Text = dt.Rows[0]["CRM_L1_REVIEWER_EMAIL"].ToString();
                    txtL1RName.Text = dt.Rows[0]["CRM_L1_REVIEWER_NAME"].ToString();
                    txtL1RId.Text = dt.Rows[0]["CRM_L1_REVIEWER_NT_ID"].ToString();
                    txtL2REmail.Text = dt.Rows[0]["CRM_L2_REVIEWER_EMAIL"].ToString();
                    txtL2RName.Text = dt.Rows[0]["CRM_L2_REVIEWER_NAME"].ToString();
                    txtL2RId.Text = dt.Rows[0]["CRM_L2_REVIEWER_NT_ID"].ToString();
                    ddlStatus.SelectedValue = dt.Rows[0]["CRM_STATUS"].ToString();
                    mvReviewMaster.ActiveViewIndex = 1;
                }
            }
        }

        protected void gvReviewerMas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReviewerMas.PageIndex = e.NewPageIndex;
            FillGrid();
        }


        void FillGrid()
        {
            string strL0= string.IsNullOrEmpty(txtSearchL0Reviewer.Text)?"":txtSearchL0Reviewer.Text;
            string strStatus = ddlSearchStatus.SelectedIndex > 0 ? ddlSearchStatus.SelectedItem.Value : "";
            DataTable dt = new DataTable();
            dt = oBLL.Search_Reviewer_Master(0, null, null, null, strStatus, strL0);
            if(dt.Rows.Count>0)
            {
                gvReviewerMas.DataSource = dt;
            }
            else
            {
                gvReviewerMas.DataSource = null;
            }
            gvReviewerMas.EditIndex = -1;
            gvReviewerMas.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            mvReviewMaster.ActiveViewIndex = 1;
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

        }
    }
}