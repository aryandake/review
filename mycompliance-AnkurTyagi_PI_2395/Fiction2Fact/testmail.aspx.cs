using System;
using System.Net;
using System.ComponentModel;
using System.Net.Mail;
using System.Configuration;

namespace ContraST
{
    public partial class testmail : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string strMail = txtMail.Text;
            string[] strTo = strMail.Split(',');
            sendAsyncMail(strTo, null, null, "test subject", "test mail content");
        }

        //private void sendAsyncMail(string[] strToList, string[] strCCList, string[] strBCCList,
        //    string strSubject, string strContent)
        //{
        //    try
        //    {
        //        strSmtpClient = (ConfigurationManager.AppSettings["SmtpClient"].ToString());
        //        strPortNo = (ConfigurationManager.AppSettings["PortNo"].ToString());
        //        strUserName = (ConfigurationManager.AppSettings["MailUserName"].ToString());
        //        strPassword = (ConfigurationManager.AppSettings["MailPassword"].ToString());
        //        strMailFrom = (ConfigurationManager.AppSettings["MailAddress"].ToString());
        //        strIsSSLEnabled = (ConfigurationManager.AppSettings["IsSSLEnabled"].ToString());

        //        intPortNo = Convert.ToInt32(strPortNo);

        //        MimeMessage message = new MimeMessage();
        //        message.From.Add(new MailboxAddress("", strMailFrom));

        //        if (!(strToList == null))
        //        {
        //            for (int i = 0; i <= strToList.GetUpperBound(0); i++)
        //            {
        //                message.To.Add(new MailboxAddress("", strToList[i]));
        //            }
        //        }

        //        if (!(strCCList == null))
        //        {
        //            for (int i = 0; i <= strCCList.GetUpperBound(0); i++)
        //            {
        //                message.Cc.Add(new MailboxAddress("", strCCList[i]));
        //            }
        //        }

        //        if (!(strBCCList == null))
        //        {
        //            for (int i = 0; i <= strBCCList.GetUpperBound(0); i++)
        //            {
        //                message.Bcc.Add(new MailboxAddress("", strBCCList[i]));
        //            }
        //        }

        //        message.Subject = strSubject;
        //        message.Body = new TextPart("html")
        //        {
        //            Text = strContent
        //        };

        //        using (var client = new SmtpClient())
        //        {
        //            client.Connect(strSmtpClient, intPortNo, MailKit.Security.SecureSocketOptions.StartTls);

        //            if (strIsSSLEnabled.Equals("true"))
        //            {
        //                client.RequireTLS = true;
        //            }
        //            else
        //            {
        //                client.RequireTLS = false;
        //            }

        //            client.Authenticate(strUserName, strPassword);
        //            client.Send(message);
        //            client.Disconnect(true);

        //            lblMsg.Text = "Mail sent successfully.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMsg.Text = ex.ToString();
        //    }
        //}

        private void sendAsyncMail(string[] strToList, string[] strCCList, string[] strBCCList,
            string strSubject, string strContent)
        {
            try
            {
                int intPortNo, retVal = 0;
                string strSmtpClient, strPortNo, strUserName, strPassword, strMailFrom, strIsSSLEnabled;

                SmtpClient smtp;
                MailMessage Mail;
                string token;
                int i;
                token = "";

                strSmtpClient = (ConfigurationManager.AppSettings["SmtpClient"].ToString());
                strPortNo = (ConfigurationManager.AppSettings["PortNo"].ToString());
                strUserName = (ConfigurationManager.AppSettings["MailUserName"].ToString());
                strPassword = (ConfigurationManager.AppSettings["MailPassword"].ToString());
                strMailFrom = (ConfigurationManager.AppSettings["MailAddress"].ToString());
                strIsSSLEnabled = (ConfigurationManager.AppSettings["IsSSLEnabled"].ToString());
                intPortNo = Convert.ToInt32(strPortNo);
                smtp = new SmtpClient(strSmtpClient, intPortNo);

                if (strIsSSLEnabled.Equals("true"))
                {
                    smtp.EnableSsl = true;
                }
                else
                {
                    smtp.EnableSsl = false;
                }
                smtp.Credentials = new NetworkCredential(strUserName, strPassword);
                //smtp.Credentials = new NetworkCredential("secmtest", "p@ssw0rd");
                Mail = new MailMessage();
                Mail.From = new MailAddress(strMailFrom);
                if (!(strToList == null))
                {
                    for (i = 0; i <= strToList.GetUpperBound(0); i++)
                    {
                        Mail.To.Add(new MailAddress(strToList[i]));
                    }
                }
                if (!(strCCList == null))
                {
                    for (i = 0; i <= strCCList.GetUpperBound(0); i++)
                    {
                        Mail.CC.Add(new MailAddress(strCCList[i]));
                    }
                }
                if (!(strBCCList == null))
                {
                    for (i = 0; i <= strBCCList.GetUpperBound(0); i++)
                    {
                        Mail.Bcc.Add(new MailAddress(strBCCList[i]));
                    }
                }

                Mail.IsBodyHtml = true;
                Mail.Body = strContent;
                Mail.Subject = strSubject;
                Mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                object userState = Mail + "~" + retVal;
                smtp.SendCompleted += new SendCompletedEventHandler(MailSendCompleted);
                smtp.SendAsync(Mail, userState);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MailSendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string strUserState = e.UserState.ToString().Split('~')[0].ToString();
            string strId = e.UserState.ToString().Split('~')[1].ToString();

            //write out the subject
            //string subject = mail.Subject;

            if (e.Cancelled)
            {

            }
            if (e.Error != null)
            {
                try
                {
                    writeError(e.Error.ToString());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //>>
            }
            else
            {
                ////<< Modify By Bhavik Patel @22Sep2015 to Update the Mail Status(Failure/Success)
                try
                {
                    writeError("Mail sent successfully...");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //>>
            }
        }

        private void writeError(string str)
        {
            lblMsg.Text = str;
        }
    }
}