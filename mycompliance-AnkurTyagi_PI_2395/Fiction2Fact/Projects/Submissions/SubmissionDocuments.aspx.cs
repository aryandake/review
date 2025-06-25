using System;
using System.Data;
using System.Web.UI;
using Fiction2Fact.Legacy_App_Code.BLL;

namespace Fiction2Fact.Projects.Submissions
{
    public partial class Submissions_SubmissionDocuments : System.Web.UI.Page
    {
        string mstrConnectionString = null;
        UtilitiesBLL utilityBL = new UtilitiesBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            //<<Added by Ankur Tyagi on 20Mar2024 for CR_1991
            if (Page.User.Identity.Name.Equals(""))
            {
                Response.Redirect(Global.site_url("Login.aspx"));
                return;
            }
            //>>
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["Id"] != null)
                {
                    string strId = Request.QueryString["Id"].ToString();
                    if (!strId.Equals(""))
                        bindGrid(strId);
                }

            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page != null)
            {
                Page.RegisterRequiresViewStateEncryption();
            }
        }


        protected void btnClose_Click(object sender, EventArgs e)
        {
            string strScript = "\r\n <script type=\"text/javascript\">\r\n" +
                   "window.close();\r\n" +
                       "</script>\r\n";
            ClientScript.RegisterStartupScript(this.GetType(), "return script", strScript);
        }


        private void writeError(string strError)
        {
            lblMsg.Text = strError;
            lblMsg.Visible = true;
        }

        private void bindGrid(string strId)
        {
            try
            {
                DataTable dtFiles = utilityBL.getDatasetWithCondition("getSubmisssionMasFiles", Convert.ToInt32(strId), mstrConnectionString);
                gvFileUpload.DataSource = dtFiles;
                gvFileUpload.DataBind();
            }
            catch (Exception ex)
            {
                writeError(ex.Message);
            }
        }
        protected String getFileName(object inputFileName)
        {
            string strFileName = inputFileName.ToString();
            return strFileName.Replace("'", "\\'");
        }
    }
}