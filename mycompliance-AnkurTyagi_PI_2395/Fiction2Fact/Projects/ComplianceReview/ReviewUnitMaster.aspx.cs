using Fiction2Fact.Legacy_App_Code.Compliance.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fiction2Fact.Projects.Compliance
{
    public partial class ReviewUnitMaster : System.Web.UI.Page
    {
        ComplianceReviewBLL oBLL = new ComplianceReviewBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillGrid();
                mvReviewUnitMaster.ActiveViewIndex = 0;
                txtUnitSPOCID.Attributes["onchange"] = " return populateUserDetsByCode('UnitSPOCID','0')";
                txtUnitHeadId.Attributes["onchange"] = " return populateUserDetsByCode('UnitHeadID','0')";
            }
        }

        void FillGrid()
        {
            string strunitname = txtSearchUnitName.Text, strunitcode = txtSearchUnitCode.Text;
            string strStatus = ddlSearchStatus.SelectedIndex > 0 ? ddlSearchStatus.SelectedItem.Value : "";
            DataTable dt = new DataTable();
            dt = oBLL.Search_SubFunction_Master(0, strunitname, strunitcode, strStatus);
            if (dt.Rows.Count > 0)
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
            mvReviewUnitMaster.ActiveViewIndex = 1;
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int intCSFM_ID = 0;
            string strCSFM_NAME = txtUnitName.Text;
            string strCSFM_CODE = txtUnitCode.Text;
            string strCSFM_HEAD = txtUnitHeadName.Text;
            string strCSFM_UNIT_HEAD_CODE = txtUnitHeadId.Text;
            string strCSFM_UNIT_HEAD_EMAIL = txtUnitHeadEmail.Text;
            string strCSFM_UNIT_SPOC = txtUnitSPOCName.Text;
            string strCSFM_UNIT_SPOC_CODE = txtUnitSPOCID.Text;
            string strCSFM_UNIT_SPOC_EMAIL = txtUnitSPOCEmail.Text;
            string strCSFM_STATUS = ddlStatus.SelectedItem.Value;
            string strCSFM_CREATE_BY = Page.User.Identity.Name;
            string strCSFM_UPDATE_BY = Page.User.Identity.Name;
            if (hfSelectedOperation.Value == "")
            {
                if (!check_unitname(txtUnitName.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MYaert", "alert('Unit Name already exists.');", true);
                    txtUnitName.Focus();
                    return;
                }

                int a = oBLL.Save_SUBFunction_Master(intCSFM_ID, strCSFM_NAME, strCSFM_CODE, strCSFM_HEAD,
           strCSFM_UNIT_HEAD_CODE, strCSFM_UNIT_HEAD_EMAIL, strCSFM_UNIT_SPOC, strCSFM_UNIT_SPOC_CODE, strCSFM_UNIT_SPOC_EMAIL
          , strCSFM_STATUS, strCSFM_CREATE_BY, null);
                if (a > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Myalert", "alert('Unit added successfully..');", true);
                }
            }
            else
            {
                intCSFM_ID = Convert.ToInt32(hfId.Value);
                int a = oBLL.Save_SUBFunction_Master(intCSFM_ID, strCSFM_NAME, strCSFM_CODE, strCSFM_HEAD,
           strCSFM_UNIT_HEAD_CODE, strCSFM_UNIT_HEAD_EMAIL, strCSFM_UNIT_SPOC, strCSFM_UNIT_SPOC_CODE, strCSFM_UNIT_SPOC_EMAIL
          , strCSFM_STATUS, null, strCSFM_UPDATE_BY);
                if (a > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Myalert", "alert('Unit updated Successfully..');", true);
                }
            }
            ClearControls();
        }

        bool check_unitname(string unitname)
        {
            bool retval;
            if (string.IsNullOrEmpty(unitname))
            {
                retval = false;
            }
            else
            {
                DataTable dt = new DataTable();
                dt = oBLL.Search_SubFunction_Master(0, null, strFilter1: " and CSFM_NAME='" + unitname + "'");
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
            hfSelectedOperation.Value = string.Empty;
            hfId.Value = string.Empty;
            hfDoubleClickFlag.Value = string.Empty;
            txtUnitCode.Text = string.Empty;
            txtUnitHeadEmail.Text = string.Empty;
            txtUnitHeadId.Text = string.Empty;
            txtUnitHeadName.Text = string.Empty;
            txtUnitName.Text = string.Empty;
            txtUnitSPOCEmail.Text = string.Empty;
            txtUnitSPOCID.Text = string.Empty;
            txtUnitSPOCName.Text = string.Empty;
            ddlStatus.SelectedIndex = 0;
            mvReviewUnitMaster.ActiveViewIndex = 0;
            FillGrid();
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
                dt = oBLL.Search_SubFunction_Master(Convert.ToInt32(strid), null);
                if (dt.Rows.Count > 0)
                {
                    hfId.Value = dt.Rows[0]["CSFM_ID"].ToString();
                    txtUnitName.Text = dt.Rows[0]["CSFM_NAME"].ToString();
                    txtUnitCode.Text = dt.Rows[0]["CSFM_CODE"].ToString();
                    txtUnitHeadName.Text = dt.Rows[0]["CSFM_HEAD"].ToString();
                    txtUnitHeadId.Text = dt.Rows[0]["CSFM_UNIT_HEAD_CODE"].ToString();
                    txtUnitHeadEmail.Text = dt.Rows[0]["CSFM_UNIT_HEAD_EMAIL"].ToString();
                    txtUnitSPOCName.Text = dt.Rows[0]["CSFM_UNIT_SPOC"].ToString();
                    txtUnitSPOCID.Text = dt.Rows[0]["CSFM_UNIT_SPOC_CODE"].ToString();
                    txtUnitSPOCEmail.Text = dt.Rows[0]["CSFM_UNIT_SPOC_EMAIL"].ToString();
                    ddlStatus.SelectedValue = dt.Rows[0]["CSFM_STATUS"].ToString();
                    mvReviewUnitMaster.ActiveViewIndex = 1;
                }
            }
        }

    }
}