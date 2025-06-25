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
using System.Collections.Generic;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.Legacy_App_Code.BLL;
using System.Text.RegularExpressions;
using Fiction2Fact.App_Code;
using Fiction2Fact.F2FControls;

namespace Fiction2Fact.Projects.Admin
{
    public partial class UnderWritingNew_RecentFailMails : System.Web.UI.Page
    {
        MailLogVO mailVO = new MailLogVO();
        MailLogBLL mailBLL = new MailLogBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getData();
            }
        }
        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            if (!CommonCodes.CheckInputValidity(this)) { return; }
            writeError("");
            getData();
        }
        protected void getData()
        {
            CommonCodes.IsCsvVulnerable("tet");
            try
            {
                DataTable dtResult = new DataTable();
                mailVO.setMailStatus(ddlStatus.SelectedValue.ToString());
                mailVO.setMailId(0);
                mailVO.setmailFromDate(txtFromDate.Text);
                mailVO.setmailToDate(txtToDate.Text);

                mailVO.setMailSubject(txtSearchSubject.Text);
                mailVO.setMailContent(txtSearchContent.Text);

                dtResult = mailBLL.getFailureMails(mailVO);
                gvSearchMailSend.DataSource = dtResult;
                Session["FailureMails"] = dtResult;
                gvSearchMailSend.DataBind();
                if (dtResult.Rows.Count > 0)
                {
                    if (ddlStatus.SelectedValue.Equals("P") || ddlStatus.SelectedValue.Equals("F") || ddlStatus.SelectedValue.Equals(""))
                        btnSendMail.Visible = true;
                    else
                        btnSendMail.Visible = false;
                }
                else
                    btnSendMail.Visible = false;
            }
            catch (Exception ex)
            {
                writeError("Sytem Exception in getData(): " + ex.Message);
            }
        }
        protected DataTable LoadAttachment(object res)
        {
            DataTable dt_attach = new DataTable();
            DataRow dr;
            try
            {
                string[] strAttachment = res.ToString().Split('~');
                if (strAttachment.Length > 0)
                {
                    dt_attach.Columns.Add(new DataColumn("ServerFileName", typeof(string)));
                    dt_attach.Columns.Add(new DataColumn("ClientFileName", typeof(string)));
                    dt_attach.Columns.Add(new DataColumn("FullFilePath", typeof(string)));
                    dt_attach.Columns.Add(new DataColumn("type", typeof(string)));

                    for (int i = 0; i < strAttachment.Length; i++)
                    {
                        dr = dt_attach.NewRow();
                        string str = strAttachment[i].ToString();
                        if (!str.Equals(""))
                        {
                            if (str.Contains("\\PO_FSCN_Files\\"))
                                str = str.Remove(0, str.IndexOf("\\PO_FSCN_Files\\") + 14);
                            else if (str.Contains("\\PI_SCN_Files\\"))
                                str = str.Remove(0, str.IndexOf("\\PI_SCN_Files\\") + 14);
                            dr["ClientFileName"] = str;
                            dr["ServerFileName"] = str;
                            dr["FullFilePath"] = strAttachment[i].ToString();
                            dr["type"] = "PI_SCN";
                            dt_attach.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                writeError("System Exception in LoadAttachment() :" + ex.Message);
            }
            return dt_attach;
        }
        protected void gvSearchMailSend_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSearchMailSend.PageIndex = e.NewPageIndex;
            gvSearchMailSend.DataSource = Session["FailureMails"];
            gvSearchMailSend.DataBind();

        }
        public void writeError(string errorMsg)
        {
            lblMsg.Text = errorMsg;
        }
        protected void btnSendMail_OnClick(object sender, EventArgs e)
        {
            Mail mm = new Mail();
            GridViewRow gvr;
            //Boolean blnFlage = true;
            for (int intIndex = 0; intIndex < gvSearchMailSend.Rows.Count; intIndex++)
            {
                string[] strMailTO = null;
                string[] strMailCC = null;
                string[] strMailBCC = null;
                string[] strMailAttachment = null;

                gvr = gvSearchMailSend.Rows[intIndex];
                CheckBox RowLevelCheckBox = (CheckBox)(gvr.FindControl("RowLevelCheckBox"));

                if (RowLevelCheckBox.Checked)
                {
                    Label lblMailId = (Label)(gvr.FindControl("lblMailId"));
                    Label lblMailFailCount = (Label)(gvr.FindControl("lblMailFailCount"));
                    Label lblMailTo = (Label)(gvr.FindControl("lblMailTo"));
                    Label lblMailCC = (Label)(gvr.FindControl("lblMailCC"));
                    Label lblMailBCC = (Label)(gvr.FindControl("lblMailBCC"));
                    Label lblMailSubject = (Label)(gvr.FindControl("lblMailSubject"));
                    Label lblMailStatus = (Label)(gvr.FindControl("lblMailStatus"));
                    Label lblMailSendDate = (Label)(gvr.FindControl("lblMailSendDate"));
                    Label lblMailContent = (Label)(gvr.FindControl("lblMailContentDialog"));
                    Label lblMailType = (Label)(gvr.FindControl("lblMailType"));
                    Label lblAttachmentsVal = (Label)(gvr.FindControl("lblAttachmentsVal"));

                    if (lblMailTo.Text != "")
                    {
                        strMailTO = lblMailTo.Text.Split(',');
                    }

                    if (lblMailCC.Text != "")
                    {
                        strMailCC = lblMailCC.Text.Split(',');
                    }
                    if (lblMailBCC.Text != "")
                    {
                        strMailBCC = lblMailBCC.Text.Split(',');
                    }
                    if (!lblAttachmentsVal.Text.Equals(""))
                    {
                        string strAttach = lblAttachmentsVal.Text;
                        string[] strAttachedFiles = strAttach.Split('~');
                        strMailAttachment = new string[strAttachedFiles.Length];
                        for (int i = 0; i < strAttachedFiles.Length; i++)
                        {
                            strMailAttachment[i] = strAttachedFiles[i].ToString();
                        }
                    }

                    if (lblMailType.Text.Equals("Attachment"))
                    {
                        mm.sendAsyncMailFailureWithAttach(strMailTO, strMailCC, strMailBCC, lblMailSubject.Text, lblMailContent.Text,
                            strMailAttachment, lblMailId.Text);
                    }
                    else if (lblMailType.Text.Equals("No Attachment"))
                    {
                        mm.sendAsyncMailFailure(strMailTO, strMailCC, strMailBCC, lblMailSubject.Text, lblMailContent.Text, lblMailId.Text);
                    }
                }
            }
            writeError("Mail Sent Successfully...");
            getData();

        }
        protected void gvSearchMailSend_DataBound(object sender, EventArgs e)
        {
            if (gvSearchMailSend.Rows.Count == 0)
            {
                return;
            }
            CheckBox HeaderLevelCheckBox = (CheckBox)(gvSearchMailSend.HeaderRow.FindControl("HeaderLevelCheckBox"));
            HeaderLevelCheckBox.Attributes["onclick"] = "ChangeAllCheckBoxStates(this.checked);";
            List<string> ArrayValues = new List<string>();
            ArrayValues.Add(String.Concat("'", HeaderLevelCheckBox.ClientID, "'"));

            GridViewRow gvr;
            //F2FTextBox txtComments;
            for (int intIndex = 0; intIndex < gvSearchMailSend.Rows.Count; intIndex++)
            {
                string strId = Convert.ToString(intIndex);
                gvr = gvSearchMailSend.Rows[intIndex];
                CheckBox RowLevelCheckBox = (CheckBox)(gvr.FindControl("RowLevelCheckBox"));
                RowLevelCheckBox.Attributes["onclick"] = "onRowCheckedUnchecked('" + RowLevelCheckBox.ClientID + "');";
                ArrayValues.Add(string.Concat("'", RowLevelCheckBox.ClientID, "'"));
            }

            CheckBoxIDsArray.Text = ("<script type=\"text/javascript\">" + ("\r\n" + ("<!--" + ("\r\n"
                        + (string.Concat("var CheckBoxIDs =  new Array(", string.Join(",", ArrayValues.ToArray()), ");")
                        + ("\r\n" + ("// -->" + ("\r\n" + "</script>"))))))));
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            //gvSearchMailSend.Columns[2].Visible = false;
            //gvSearchMailSend.Columns[3].Visible = false;
            //gvSearchMailSend.Columns[4].Visible = false;
            //gvSearchMailSend.Columns[5].Visible = false;
            gvSearchMailSend.AllowPaging = false;
            gvSearchMailSend.AllowSorting = false;
            gvSearchMailSend.DataSource = (DataTable)Session["FailureMails"];
            gvSearchMailSend.DataBind();
            gvSearchMailSend.Columns[0].Visible = false;
            gvSearchMailSend.Columns[7].Visible = false;
            gvSearchMailSend.Columns[10].Visible = false;
            gvSearchMailSend.Columns[11].Visible = false;
            CommonCodes.PrepareGridViewForExport(gvSearchMailSend);
            string attachment = "attachment; filename=MailLog.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";

            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvSearchMailSend.RenderControl(htw);

            //<<Added by Vivek on 16-Jan-2020
            string html2 = Regex.Replace(sw.ToString(), @"(<input type=""image""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<input class=""checkbox""\/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<a \/?[^>]+>)", @"", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);
            html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;'>", RegexOptions.IgnoreCase);

            //html2 = Regex.Replace(html2, @"(<br />)", @"<br style='mso-data-placement:same-cell;'>"+Environment.NewLine, RegexOptions.IgnoreCase);
            //html2 = Regex.Replace(html2, @"(<br/>)", @"<br style='mso-data-placement:same-cell;'>"+Environment.NewLine, RegexOptions.IgnoreCase);

            Response.Write(html2.ToString());
            //>>

            //Response.Write(sw.ToString());
            Response.End();
            gvSearchMailSend.DataSource = (DataTable)Session["FailureMails"];
            gvSearchMailSend.AllowPaging = true;
            gvSearchMailSend.AllowSorting = true;
            gvSearchMailSend.DataBind();
            gvSearchMailSend.Columns[0].Visible = true;
            gvSearchMailSend.Columns[7].Visible = true;
            gvSearchMailSend.Columns[10].Visible = true;
            gvSearchMailSend.Columns[11].Visible = true;
        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}