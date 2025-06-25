using System.Net.Mail;
using System.Net;
using System.Configuration;
using System;
using System.ComponentModel;
using System.Web;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.Legacy_App_Code;
using Fiction2Fact.App_Code;
using System.Linq;
using System.IO;

public class Mail
{
    MailLogVO mlVo = new MailLogVO();
    MailLogBLL mlBll = new MailLogBLL();
    SendMail mailObj = new SendMail();
    string[] strTo = null;
    int intPortNo, retVal, intMailId = 0, retUpdVal = 0, i;
    string[] distinctToArray = null, distinctCCArray = null;
    string strToListVal = null, strCCListVal = null, strBCCListVal = null;
    string strAttachmentsVal = null;

    string strMailSentType = ConfigurationManager.AppSettings.AllKeys.Any(key => key.ToUpper() == "MAILSENTTYPE") ?
                            ConfigurationManager.AppSettings["MailSentType"].ToString().ToUpper() : "0";

    string strUseDefaultCredentialForMail = ConfigurationManager.AppSettings.AllKeys.Any(key => key.ToUpper() == "USEDEFAULTCREDENTIALFORMAIL") ?
                                                ConfigurationManager.AppSettings["UseDefaultCredentialForMail"].ToString().ToUpper() : "N";

    string strIsTLSEnabled = ConfigurationManager.AppSettings.AllKeys.Any(key => key.ToUpper() == "ISTLSENABLED") ?
                                                ConfigurationManager.AppSettings["IsTLSEnabled"].ToString().ToUpper() : "N";

    string strMailToBeSent = (ConfigurationManager.AppSettings["MailToBeSent"].ToString());
    string strMailTo = (ConfigurationManager.AppSettings["MailTo"].ToString());
    string strsmtpClient = (ConfigurationManager.AppSettings["SmtpClient"].ToString());
    string strPortNo = (ConfigurationManager.AppSettings["PortNo"].ToString());
    string strUserName = (ConfigurationManager.AppSettings["MailUserName"].ToString());
    string strPassword = (ConfigurationManager.AppSettings["MailPassword"].ToString());
    string strMailFrom = (ConfigurationManager.AppSettings["MailAddress"].ToString());
    string strIsSSLEnabled = (ConfigurationManager.AppSettings["IsSSLEnabled"].ToString());

    public void sendAsyncMail(string[] strToList, string[] strCCList, string[] strBCCList, string strSubject,
        string strContent)
    {
        try
        {
            SmtpClient smtp;
            MailMessage Mail;

            intPortNo = Convert.ToInt32(strPortNo);
            smtp = new SmtpClient(strsmtpClient, intPortNo);

            if (strIsSSLEnabled.Equals("true"))
            {
                smtp.EnableSsl = true;
            }
            else
            {
                smtp.EnableSsl = false;
            }

            smtp.Credentials = new NetworkCredential(strUserName, strPassword);
            Mail = new MailMessage();
            Mail.From = new MailAddress(strMailFrom);

            if (strToList != null)
            {
                if (strToList.Length > 0)
                {
                    distinctToArray = RemoveDuplicates(strToList);
                }
            }

            if (strCCList != null)
            {
                if (strCCList.Length > 0)
                {
                    distinctCCArray = RemoveDuplicates(strCCList);
                }
            }

            try
            {
                if (distinctToArray != null)
                {
                    strToListVal = string.Join(",", distinctToArray);
                }
                if (distinctCCArray != null)
                {
                    strCCListVal = string.Join(",", distinctCCArray);
                }
                if (strBCCList != null)
                {
                    strBCCListVal = string.Join(",", strBCCList);
                }

                mlVo.setMailTO(strToListVal);
                mlVo.setMailCC(strCCListVal);
                mlVo.setMailBCC(strBCCListVal);
                mlVo.setMailStatus("P");
                mlVo.setMailSubject(strSubject);
                mlVo.setMailContent(strContent);
                mlVo.setCreatedBy(System.Web.HttpContext.Current.User.Identity.Name.ToString());
                mlVo.setMailSendOn(System.DateTime.Today.ToString());
                mlVo.setMailType("No Attachment");
                mlVo.setMailAttachments("");
                retVal = mlBll.insertIntoMailLog(mlVo);
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }

            if (strMailToBeSent.Equals("0"))
            {

            }
            else if (strMailToBeSent.Equals("1"))
            {
                if (!(distinctToArray == null))
                {
                    for (i = 0; i <= distinctToArray.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(distinctToArray[i]))
                            Mail.To.Add(new MailAddress(distinctToArray[i]));
                    }
                }

                if (!(distinctCCArray == null))
                {
                    for (i = 0; i <= distinctCCArray.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(distinctCCArray[i]))
                            Mail.CC.Add(new MailAddress(distinctCCArray[i]));
                    }
                }
                if (!(strBCCList == null))
                {
                    for (i = 0; i <= strBCCList.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(strBCCList[i]))
                            Mail.Bcc.Add(new MailAddress(strBCCList[i]));
                    }
                }

                Mail.IsBodyHtml = true;
                Mail.Body = strContent;
                Mail.Subject = strSubject;
                Mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                object userState = Mail + "~" + retVal;

                if (strUseDefaultCredentialForMail.Equals("Y"))
                {
                    smtp.UseDefaultCredentials = true;
                }

                smtp.SendCompleted += new SendCompletedEventHandler(MailSendCompleted);

                if (strIsTLSEnabled.Equals("Y"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }

                smtp.SendAsync(Mail, userState);
            }
            else if (strMailToBeSent.Equals("2"))
            {
                strContent = "To: " + strToListVal + "<br/><br/>" + "CC: " + strCCListVal + "<br/><br/>" + strContent;
                strTo = strMailTo.Split(',');

                for (int j = 0; j <= strTo.GetUpperBound(0); j++)
                {
                    if (!string.IsNullOrEmpty(strTo[j]))
                        Mail.To.Add(new MailAddress(strTo[j]));
                }

                Mail.IsBodyHtml = true;
                Mail.Body = strContent;
                Mail.Subject = strSubject;
                Mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                object userState = Mail + "~" + retVal;

                if (strUseDefaultCredentialForMail.Equals("Y"))
                {
                    smtp.UseDefaultCredentials = true;
                }

                smtp.SendCompleted += new SendCompletedEventHandler(MailSendCompleted);

                if (strIsTLSEnabled.Equals("Y"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }

                smtp.SendAsync(Mail, userState);
            }
        }
        catch (Exception ex)
        {
            F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            throw ex;
        }
    }

    public string[] RemoveDuplicates(string[] inputArray)
    {
        int length = inputArray.Length;
        for (int i = 0; i < length; i++)
        {
            for (int j = (i + 1); j < length;)
            {
                if (inputArray[i] == inputArray[j])
                {
                    for (int k = j; k < length - 1; k++)
                        inputArray[k] = inputArray[k + 1];
                    length--;
                }
                else
                    j++;
            }
        }

        string[] distinctArray = new string[length];
        for (int i = 0; i < length; i++)
            distinctArray[i] = inputArray[i].Trim();

        return distinctArray;
    }

    private void MailSendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        string strUserState = e.UserState.ToString().Split('~')[0].ToString();
        string strId = e.UserState.ToString().Split('~')[1].ToString();
        if (e.Cancelled)
        {

        }
        if (e.Error != null)
        {
            //<< Added By Milan Yadav 17-Mar-2016 to Update the Mail Status(Failure/Success)
            try
            {
                mlVo.setMailId(Convert.ToInt32(strId));
                mlVo.setMailStatus("F");
                mlVo.setErrorMsg(e.Error.ToString());
                retUpdVal = mlBll.insertIntoMailLog(mlVo);
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
            //>>
        }
        else
        {
            //Added By Milan Yadav on 17-Mar-2016 Mail Status(Failure/Success)
            try
            {
                mlVo.setMailId(Convert.ToInt32(strId));
                mlVo.setMailStatus("S");
                retUpdVal = mlBll.insertIntoMailLog(mlVo);
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
            //>>
        }
    }

    public void sendMailWithAttach(string[] strToList, string[] strCCList, string[] strBCCList,
        string strSubject, string strContent, string[] strAttach, string[] strAttachNames)
    {
        //<< Added by Vivek on 14-Sep-2024 for CR_2167 for configuring MailKit to send the mails
        if (strMailSentType.Equals("1")) //Using Mail Kit
        {
            mailObj.sendMailWithAttach(strToList, strCCList, strBCCList, strSubject, strContent, strAttach);
        }
        //>>
        else
        {
            SmtpClient smtp;
            MailMessage Mail;
            Attachment attach;
            string strCompleteFilePath = "", strClientFileName = "";

            intPortNo = Convert.ToInt32(strPortNo);
            smtp = new SmtpClient(strsmtpClient, intPortNo);

            if (strIsSSLEnabled.Equals("true"))
            {
                smtp.EnableSsl = true;
            }
            else
            {
                smtp.EnableSsl = false;
            }

            smtp.Credentials = new NetworkCredential(strUserName, strPassword);
            Mail = new MailMessage();
            Mail.From = new MailAddress(strMailFrom);

            if (strToList != null)
            {
                if (strToList.Length > 0)
                {
                    distinctToArray = RemoveDuplicates(strToList);
                }
            }

            if (strCCList != null)
            {
                if (strCCList.Length > 0)
                {
                    distinctCCArray = RemoveDuplicates(strCCList);
                }
            }

            try
            {

                if (distinctToArray != null)
                {
                    strToListVal = string.Join(",", distinctToArray);
                }

                if (distinctCCArray != null)
                {
                    strCCListVal = string.Join(",", distinctCCArray);
                }

                if (strBCCList != null)
                {
                    strBCCListVal = string.Join(",", strBCCList);
                }

                if (strAttach != null)
                {
                    strAttachmentsVal = string.Join("~", strAttach);
                }

                mlVo.setMailTO(strToListVal);
                mlVo.setMailCC(strCCListVal);
                mlVo.setMailBCC(strBCCListVal);
                mlVo.setMailStatus("P");
                mlVo.setMailSubject(strSubject);
                mlVo.setMailContent(strContent);
                mlVo.setCreatedBy(HttpContext.Current.User.Identity.Name.ToString());
                mlVo.setMailSendOn(System.DateTime.Today.ToString());
                mlVo.setMailType("Attachment");
                mlVo.setMailAttachments(strAttachmentsVal);
                retVal = mlBll.insertIntoMailLog(mlVo);
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

            //<< Modified by Vivek on 04-Sep-2021
            if (!(strAttach == null))
            {
                for (i = 0; i <= strAttach.GetUpperBound(0); i++)
                {
                    if (File.Exists(strAttach[i]))
                    {
                        Mail.Attachments.Add(new Attachment(strAttach[i]));

                        if (strAttachNames != null && i < strAttachNames.Length)
                        {
                            Mail.Attachments[i].Name = strAttachNames[i];
                        }
                    }
                }
            }
            //>>

            if (strMailToBeSent.Equals("0"))
            {
            }
            else if (strMailToBeSent.Equals("1"))
            {
                if (!(distinctToArray == null))
                {
                    for (i = 0; i <= distinctToArray.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(distinctToArray[i]))
                            Mail.To.Add(new MailAddress(distinctToArray[i]));
                    }
                }

                if (!(distinctCCArray == null))
                {
                    for (i = 0; i <= distinctCCArray.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(distinctCCArray[i]))
                            Mail.CC.Add(new MailAddress(distinctCCArray[i]));
                    }
                }
                if (!(strBCCList == null))
                {
                    for (i = 0; i <= strBCCList.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(strBCCList[i]))
                            Mail.Bcc.Add(new MailAddress(strBCCList[i]));
                    }
                }
                Mail.IsBodyHtml = true;
                Mail.Body = strContent;
                Mail.Subject = strSubject;
                Mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                object userState = Mail + "~" + retVal;

                if (strUseDefaultCredentialForMail.Equals("Y"))
                {
                    smtp.UseDefaultCredentials = true;
                }

                smtp.SendCompleted += new SendCompletedEventHandler(MailSendCompletedAttachments);

                if (strIsTLSEnabled.Equals("Y"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }

                smtp.SendAsync(Mail, userState);
            }
            else if (strMailToBeSent.Equals("2"))
            {
                Mail.To.Add(new MailAddress(strMailTo));
                Mail.IsBodyHtml = true;
                Mail.Body = strContent;
                Mail.Subject = strSubject;
                Mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                object userState = Mail + "~" + retVal;

                if (strUseDefaultCredentialForMail.Equals("Y"))
                {
                    smtp.UseDefaultCredentials = true;
                }

                smtp.SendCompleted += new SendCompletedEventHandler(MailSendCompletedAttachments);

                if (strIsTLSEnabled.Equals("Y"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }

                smtp.SendAsync(Mail, userState);
            }
        }
    }

    public void sendAsyncMailFailure(string[] strToList, string[] strCCList, string[] strBCCList,
       string strSubject, string strContent, string mailId)
    {
        //<< Added by Vivek on 14-Sep-2024 for CR_2167 for configuring MailKit to send the mails
        if (strMailSentType.Equals("1")) //Using Mail Kit
        {
            mailObj.sendAsyncMailFailure(strToList, strCCList, strBCCList, strSubject, strContent, mailId);
        }
        //>>
        else
        {
            try
            {
                SmtpClient smtp;
                MailMessage Mail;

                intPortNo = Convert.ToInt32(strPortNo);
                smtp = new SmtpClient(strsmtpClient, intPortNo);

                if (strIsSSLEnabled.Equals("true"))
                {
                    smtp.EnableSsl = true;
                }
                else
                {
                    smtp.EnableSsl = false;
                }

                smtp.Credentials = new NetworkCredential(strUserName, strPassword);
                Mail = new MailMessage();
                Mail.From = new MailAddress(strMailFrom);

                try
                {
                    if (strToList != null)
                    {
                        strToListVal = string.Join(",", strToList);
                    }
                    if (strCCList != null)
                    {
                        strCCListVal = string.Join(",", strCCList);
                    }
                    if (strBCCList != null)
                    {
                        strBCCListVal = string.Join(",", strBCCList);
                    }

                    if (mailId != "")
                    {
                        intMailId = Convert.ToInt32(mailId);
                        retVal = intMailId;
                    }
                }
                catch (Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    throw ex;
                }

                if (strMailToBeSent.Equals("0"))
                {

                }
                else if (strMailToBeSent.Equals("1"))
                {
                    if (!(strToList == null))
                    {
                        for (i = 0; i <= strToList.GetUpperBound(0); i++)
                        {
                            if (!string.IsNullOrEmpty(strToList[i]))
                                Mail.To.Add(new MailAddress(strToList[i]));
                        }
                    }
                    if (!(strCCList == null))
                    {
                        for (i = 0; i <= strCCList.GetUpperBound(0); i++)
                        {
                            if (!string.IsNullOrEmpty(strCCList[i]))
                                Mail.CC.Add(new MailAddress(strCCList[i]));
                        }
                    }
                    if (!(strBCCList == null))
                    {
                        for (i = 0; i <= strBCCList.GetUpperBound(0); i++)
                        {
                            if (!string.IsNullOrEmpty(strBCCList[i]))
                                Mail.Bcc.Add(new MailAddress(strBCCList[i]));
                        }
                    }

                    Mail.IsBodyHtml = true;
                    Mail.Body = strContent;
                    Mail.Subject = strSubject;
                    Mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    object userState = Mail + "~" + retVal;

                    if (strUseDefaultCredentialForMail.Equals("Y"))
                    {
                        smtp.UseDefaultCredentials = true;
                    }

                    smtp.SendCompleted += new SendCompletedEventHandler(MailSendCompletedFailure);

                    if (strIsTLSEnabled.Equals("Y"))
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    }

                    smtp.SendAsync(Mail, userState);
                }
                else if (strMailToBeSent.Equals("2"))
                {
                    Mail.To.Add(new MailAddress(strMailTo));
                    Mail.IsBodyHtml = true;
                    Mail.Body = strContent;
                    Mail.Subject = strSubject;
                    Mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    object userState = Mail + "~" + retVal;

                    if (strUseDefaultCredentialForMail.Equals("Y"))
                    {
                        smtp.UseDefaultCredentials = true;
                    }

                    smtp.SendCompleted += new SendCompletedEventHandler(MailSendCompletedFailure);

                    if (strIsTLSEnabled.Equals("Y"))
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    }

                    smtp.SendAsync(Mail, userState);
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }
    }

    public void sendAsyncMailFailureWithAttach(string[] strToList, string[] strCCList, string[] strBCCList,
        string strSubject, string strContent, string[] strAttach, string strMailId)
    {
        //<< Added by Vivek on 14-Sep-2024 for CR_2167 for configuring MailKit to send the mails
        if (strMailSentType.Equals("1")) //Using Mail Kit
        {
            mailObj.sendAsyncMailFailureWithAttach(strToList, strCCList, strBCCList, strSubject, strContent, strAttach, strMailId);
        }
        //>>
        else
        {
            try
            {
                SmtpClient smtp;
                MailMessage Mail;
                Attachment attach;
                string strCompleteFilePath = "", strClientFileName = "";
                string[] strAttachmentNames = new string[1];

                intPortNo = Convert.ToInt32(strPortNo);
                smtp = new SmtpClient(strsmtpClient, intPortNo);

                if (strIsSSLEnabled.Equals("true"))
                {
                    smtp.EnableSsl = true;
                }
                else
                {
                    smtp.EnableSsl = false;
                }

                smtp.Credentials = new NetworkCredential(strUserName, strPassword);
                Mail = new MailMessage();
                Mail.From = new MailAddress(strMailFrom);

                try
                {
                    if (strToList != null)
                    {
                        strToListVal = string.Join(",", strToList);
                    }
                    if (strCCList != null)
                    {
                        strCCListVal = string.Join(",", strCCList);
                    }
                    if (strBCCList != null)
                    {
                        strBCCListVal = string.Join(",", strBCCList);
                    }
                    if (strAttach != null)
                    {
                        strAttachmentsVal = string.Join("~", strAttach);
                    }
                    if (strMailId != "")
                    {
                        intMailId = Convert.ToInt32(strMailId);
                        retVal = intMailId;
                    }
                }
                catch (Exception ex)
                {
                    F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                    throw ex;
                }

                //<< Modified by Vivek on 04-Sep-2021
                if (!(strAttach == null))
                {
                    for (i = 0; i <= strAttach.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(strAttach[i]))
                        {
                            strAttachmentNames = strAttach[i].Split('|');
                            strCompleteFilePath = strAttachmentNames[0];
                            strClientFileName = strAttachmentNames[1];
                            attach = new Attachment(strCompleteFilePath);
                            attach.ContentDisposition.FileName = strClientFileName;
                            Mail.Attachments.Add(attach);
                        }
                    }
                }
                //>>

                if (strMailToBeSent.Equals("0"))
                {

                }
                else if (strMailToBeSent.Equals("1"))
                {

                    if (!(strToList == null))
                    {
                        for (i = 0; i <= strToList.GetUpperBound(0); i++)
                        {
                            if (!string.IsNullOrEmpty(strToList[i]))
                                Mail.To.Add(new MailAddress(strToList[i]));
                        }
                    }
                    if (!(strCCList == null))
                    {
                        for (i = 0; i <= strCCList.GetUpperBound(0); i++)
                        {
                            if (!string.IsNullOrEmpty(strCCList[i]))
                                Mail.CC.Add(new MailAddress(strCCList[i]));
                        }
                    }
                    if (!(strBCCList == null))
                    {
                        for (i = 0; i <= strBCCList.GetUpperBound(0); i++)
                        {
                            if (!string.IsNullOrEmpty(strBCCList[i]))
                                Mail.Bcc.Add(new MailAddress(strBCCList[i]));
                        }
                    }

                    Mail.IsBodyHtml = true;
                    Mail.Body = strContent;
                    Mail.Subject = strSubject;
                    Mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    object userState = Mail + "~" + retVal;

                    if (strUseDefaultCredentialForMail.Equals("Y"))
                    {
                        smtp.UseDefaultCredentials = true;
                    }

                    smtp.SendCompleted += new SendCompletedEventHandler(MailSendCompletedFailure);

                    if (strIsTLSEnabled.Equals("Y"))
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    }

                    smtp.SendAsync(Mail, userState);
                }
                else if (strMailToBeSent.Equals("2"))
                {
                    Mail.To.Add(new MailAddress(strMailTo));
                    Mail.IsBodyHtml = true;
                    Mail.Body = strContent;
                    Mail.Subject = strSubject;
                    Mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    object userState = Mail + "~" + retVal;

                    if (strUseDefaultCredentialForMail.Equals("Y"))
                    {
                        smtp.UseDefaultCredentials = true;
                    }
                    smtp.SendCompleted += new SendCompletedEventHandler(MailSendCompleted);

                    if (strIsTLSEnabled.Equals("Y"))
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    }

                    smtp.SendAsync(Mail, userState);
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }
    }

    private void MailSendCompletedAttachments(object sender, AsyncCompletedEventArgs e)
    {
        string strUserState = "", strId = "", strUWCaseId = "";
        int intMailConId = 0;
        string[] strUserData = e.UserState.ToString().Split('~');
        int count = strUserData.GetUpperBound(0);
        string userId = Authentication.GetUserID(HttpContext.Current.User.Identity.Name.ToString());
        strUserState = e.UserState.ToString().Split('~')[0].ToString();
        strId = e.UserState.ToString().Split('~')[1].ToString();

        if (count >= 3)
        {
            intMailConId = Convert.ToInt32(e.UserState.ToString().Split('~')[2].ToString());
            strUWCaseId = e.UserState.ToString().Split('~')[3].ToString();
        }

        if (e.Cancelled)
        {
        }
        if (e.Error != null)
        {
            try
            {
                mlVo.setMailId(Convert.ToInt32(strId));
                mlVo.setMailStatus("F");
                mlVo.setErrorMsg(e.Error.ToString());
                retUpdVal = mlBll.insertIntoMailLog(mlVo);
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }
        else
        {
            try
            {
                mlVo.setMailId(Convert.ToInt32(strId));
                mlVo.setMailStatus("S");
                retUpdVal = mlBll.insertIntoMailLog(mlVo);
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }
    }

    private void MailSendCompletedFailure(object sender, AsyncCompletedEventArgs e)
    {
        string strUserState = e.UserState.ToString().Split('~')[0].ToString();
        string strId = e.UserState.ToString().Split('~')[1].ToString();

        if (e.Cancelled)
        {

        }
        if (e.Error != null)
        {
            //<< Added By Milan Yadav  @1 Octo 2015 to Update the Mail Status(Failure/Success)
            try
            {
                mlVo.setMailId(Convert.ToInt32(strId));
                mlVo.setMailStatus("F");
                mlVo.setErrorMsg(e.Error.ToString());
                retUpdVal = mlBll.insertIntoMailLog(mlVo);
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
            //>>
        }
        else
        {
            ////<< Added By Milan Yadav  on 17-Mar-2016 to Update the Mail Status(Failure/Success)
            try
            {
                //mlVo.setMailId(retVal);
                mlVo.setMailId(Convert.ToInt32(strId));
                mlVo.setMailStatus("S");
                retUpdVal = mlBll.insertIntoMailLog(mlVo);
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
            //>>
        }
    }

}

