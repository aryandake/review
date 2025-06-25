using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using Fiction2Fact.Legacy_App_Code.Outward.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;

namespace Fiction2Fact.Projects.Outward
{
    public partial class ViewOutward : System.Web.UI.Page
    {
        OutwardUtilitiesBLL outUtilBLL = new OutwardUtilitiesBLL();
        //<< Added by ramesh more on 13-Mar-2024 CR_1991
        SHA256EncryptionDecryption encdec = new SHA256EncryptionDecryption();
        //>>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    string strId = Request.QueryString["Id"].ToString();
                    //<< Modified by ramesh more on 13-Mar-2024 CR_1991
                    hfOTId.Value = encdec.Decrypt(strId);
                    //>>
                    DataTable dt = new DataTable();
                    DataRow dr;
                    int intOTId = Convert.ToInt32(hfOTId.Value);
                    dt = outUtilBLL.GetDataTable("getOutwardDetails", new DBUtilityParameter("OT_ID", intOTId));
                    dr = dt.Rows[0];
                    lbltypeofoutward.Text = dr["OTM_NAME"].ToString();
                    lblAddressor.Text = dr["AddressorName"].ToString();
                    lblAddressee.Text = dr["OT_ADRESSEE"].ToString();
                    lblDocName.Text = dr["OT_DOC_NAME"].ToString();
                    if (dr["OT_DATE"].Equals(DBNull.Value))
                    {
                        lblOutwardDate.Text = "";
                    }
                    else
                    {
                        lblOutwardDate.Text = Convert.ToDateTime(dr["OT_DATE"]).ToString("dd-MMM-yyyy");
                    }
                    lblAuthority.Text = dr["ORAM_NAME"].ToString();
                    lblDept.Text = dr["ODM_NAME"].ToString();
                    lbllinkedOutward.Text= dr["OT_BASE_OUTWARD"].ToString();
                    hfoldId.Value = dr["OT_BASE_OUTWARD_ID"].ToString();
                    lblDocNumber.Text = dr["OT_DOCUMENT_NO"].ToString();
                    lblremark.Text = dr["OT_REMARKS"].ToString().Replace(Environment.NewLine, "<br/>");
                    lblclosureRemarks.Text = dr["OT_CLOSED_REMARK"].ToString().Replace(Environment.NewLine, "<br/>");
                    lblclosedBy.Text= dr["OT_CLOSED_BY"].ToString();  
                    if (dr["OT_CLOSED_ON"].Equals(DBNull.Value))
                    {
                        lblclosedOn.Text = "";
                    }
                    else
                    {
                        lblclosedOn.Text = Convert.ToDateTime(dr["OT_CLOSED_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    }
                    lblCancelRemarks.Text = dr["OT_CANCEL_REMARKS"].ToString().Replace(Environment.NewLine, "<br/>");
                    lblcancelBy.Text = dr["OT_CANCEL_BY"].ToString();
                    if (dr["OT_CANCEL_ON"].Equals(DBNull.Value))
                    {
                        lblcancelOn.Text = "";
                    }
                    else
                    {
                        lblcancelOn.Text = Convert.ToDateTime(dr["OT_CANCEL_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    }
                    lblStatus.Text = dr["OT_STAUTS"].ToString();
                    lblCreatedBy.Text = dr["OT_CREATE_BY"].ToString();
                    lblCreatedOn.Text = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", Convert.ToDateTime(dr["OT_CREATE_DATE"]));

                    lblPODNumber.Text = dr["OT_POD_NUMBER"].ToString();
                    lblCourierName.Text = dr["OT_COURIER_NAME"].ToString();
                    if (dr["OT_EMAIL_SENT_DT"].Equals(DBNull.Value))
                    {
                        lblEmailSentDate.Text = "";
                    }
                    else
                    {
                        lblEmailSentDate.Text = Convert.ToDateTime(dr["OT_EMAIL_SENT_DT"]).ToString("dd-MMM-yyyy");
                    }
                    lblHardCopySent.Text = dr["HardCopy"].ToString();
                    if (dr["OT_DISPATCH_DT"].Equals(DBNull.Value))
                    {
                        lblCourierSentDt.Text = "";
                    }
                    else
                    {
                        lblCourierSentDt.Text = Convert.ToDateTime(dr["OT_DISPATCH_DT"]).ToString("dd-MMM-yyyy");
                    }
                    lblRepresentationStatus.Text = dr["OT_REPRESENTATION_STATUS"].ToString();
                    if (dr["OT_REPRESENTATION_DT"].Equals(DBNull.Value))
                    {
                        lblRepresentationDt.Text = "";
                    }
                    else
                    {
                        lblRepresentationDt.Text = Convert.ToDateTime(dr["OT_REPRESENTATION_DT"]).ToString("dd-MMM-yyyy");
                    }
                    lblSentTo.Text = dr["SentTo"].ToString();
                    lblsuggestRevision.Text= dr["OT_SUGGEST_REVISION_REMARK"].ToString();
                    lblsuggestRevisionBy.Text= dr["OT_SUGGEST_REVISION_BY"].ToString(); 
                    if (dr["OT_SUGGEST_REVISION_DT"].Equals(DBNull.Value))
                    {
                        lblsuggestRevisionOn.Text = "";
                    }
                    else
                    {
                        lblsuggestRevisionOn.Text = Convert.ToDateTime(dr["OT_SUGGEST_REVISION_DT"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    }

                    lbldeleteRemark.Text = dr["OT_DELETE_REMARK"].ToString();
                    lbldeleteBy.Text = dr["OT_DELETEED_BY"].ToString();
                    if (dr["OT_DELETED_ON"].Equals(DBNull.Value))
                    {
                        lbldeleteOn.Text = "";
                    }
                    else
                    {
                        lbldeleteOn.Text = Convert.ToDateTime(dr["OT_DELETED_ON"]).ToString("dd-MMM-yyyy HH:mm:ss");
                    }

                    DataTable dtFiles = outUtilBLL.GetDataTable("getOutwardFiles", new DBUtilityParameter("OF_OT_ID", intOTId));
                    gvFileUpload.DataSource = dtFiles;
                    gvFileUpload.DataBind();
                    if(lblStatus.Text=="Closed")
                    {
                        btnaddUpdate.Visible = true;
                        //if (Roles.IsUserInRole("Outward_Compliance_User"))
                        //{
                        //    btnaddUpdate.Visible = false;
                        //}
                        //else
                        //{

                        //}
                    }
                    getoutwardUpdates(hfOTId.Value);
                    //getLinkedOutward(hfOTId.Value);
                }
                else
                {
                    hfOTId.Value = "0";
                }
            }
        }

        void getoutwardUpdates(string Id)
        {
            DataTable dtOutwardUpdates = outUtilBLL.GetDataTable("getOutwardUpdates", new DBUtilityParameter("OTU_OT_ID", Id));
            gvUpdates.DataSource = dtOutwardUpdates;
            gvUpdates.DataBind();
            if (dtOutwardUpdates.Rows.Count > 0)
            {
                gvUpdates.Visible = true;
            }
        }
        //void getLinkedOutward(string Id)
        //{

        //}
        protected void gvInsertFileUpload_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (((e.Row.RowType == DataControlRowType.DataRow)
                        || (e.Row.RowType == DataControlRowType.Header)))
            {

                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
            }
        }

        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            //if (Request.QueryString["Id"] != null)
            //    Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx"));
            //else
            //    Response.Redirect(Global.site_url("Default.aspx"));
            if (Request.QueryString["source"] != null)
            {
                string source = Convert.ToString(Request.QueryString["source"]);
                if(source=="SO")
                {
                    Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx"));
                }
                else if(source == "SOC")
                {
                    Response.Redirect(Global.site_url("Projects/Outward/SearchOutward.aspx?type='Closure'"));
                }
                
            }
            
        }

        protected void btnaddUpdate_Click(object sender, EventArgs e)
        {
            string url = "AddUpdates.aspx?Id=" + hfOTId.Value;
            string s = "window.open('" + url + "', 'popup_window', 'width=800,height=400,left=0,top=0,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }

        protected void lnklinkedOutward_Click(object sender, EventArgs e)
        {
            string source = Convert.ToString(Request.QueryString["source"]);
            if (source == "SO")
            {
                //Response.Redirect("ViewOutward.aspx?Id=" + hfoldId.Value + "&source=SO");
                string url_To_Open = "ViewOutward.aspx?Id=" + encdec.Encrypt(hfoldId.Value) + "&source=SO";
                string modified_URL = "window.open('" + url_To_Open + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", modified_URL, true);
            }
            else if (source == "SOC")
            {
                string url_To_Open = "ViewOutward.aspx?Id=" + encdec.Encrypt(hfoldId.Value) + "&source=SOC";
                string modified_URL = "window.open('" + url_To_Open + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", modified_URL, true);
                // Response.Redirect("ViewOutward.aspx?Id=" + hfoldId.Value + "&source=SOC");
            }
            
        }
    }
}