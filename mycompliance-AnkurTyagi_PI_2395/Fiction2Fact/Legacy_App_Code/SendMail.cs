using System;
using System.Configuration;
using System.Web;
using System.Linq;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net;
using System.IO;
using Fiction2Fact.Legacy_App_Code.VO;
using Fiction2Fact.Legacy_App_Code.BLL;
using Fiction2Fact.App_Code;

//<< Added by Vivek on 14-Sep-2024 for CR_2167 for configuring MailKit to send the mails
public class SendMail
{
    MailLogVO mlVo = new MailLogVO();
    MailLogBLL mlBll = new MailLogBLL();
    string[] strTo = null;
    int intPortNo, intMailLogId = 0, intMailId = 0, retUpdVal = 0, i;
    string[] distinctToArray = null, distinctCCArray = null;
    string strToListVal = null, strCCListVal = null, strBCCListVal = null;
    string strAttachmentsVal = null;

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
        string strContent, string strModulewiseMailTo = null)
    {
        try
        {
            MimeMessage message = new MimeMessage();
            intPortNo = Convert.ToInt32(strPortNo);

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
                intMailLogId = mlBll.insertIntoMailLog(mlVo);
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }

            message.From.Add(new MailboxAddress("", strMailFrom));

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
                            message.To.Add(new MailboxAddress("", distinctToArray[i]));
                    }
                }

                if (!(distinctCCArray == null))
                {
                    for (i = 0; i <= distinctCCArray.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(distinctCCArray[i]))
                            message.Cc.Add(new MailboxAddress("", distinctCCArray[i]));
                    }
                }

                if (!(strBCCList == null))
                {
                    for (i = 0; i <= strBCCList.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(strBCCList[i]))
                            message.Bcc.Add(new MailboxAddress("", strBCCList[i]));
                    }
                }

                message.Subject = strSubject;
                message.Body = new TextPart("html")
                {
                    Text = strContent
                };

                if (strIsTLSEnabled.Equals("Y"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }

                using (var client = new SmtpClient())
                {
                    client.Connect(strsmtpClient, intPortNo, MailKit.Security.SecureSocketOptions.StartTls);
                    if (strIsSSLEnabled.Equals("true"))
                        client.RequireTLS = true;
                    else
                        client.RequireTLS = false;

                    client.Authenticate(strUserName, strPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            else if (strMailToBeSent.Equals("2"))
            {
                strContent = "To: " + strToListVal + "<br/><br/>" + "CC: " + strCCListVal + "<br/><br/>" + strContent;

                if (string.IsNullOrEmpty(strModulewiseMailTo))
                    strTo = strMailTo.Split(',');
                else
                    strTo = strModulewiseMailTo.Split(',');

                for (int j = 0; j <= strTo.GetUpperBound(0); j++)
                {
                    if (!string.IsNullOrEmpty(strTo[j]))
                        message.To.Add(new MailboxAddress("", strTo[j]));
                }

                message.Subject = strSubject;
                message.Body = new TextPart("html")
                {
                    Text = strContent
                };

                if (strIsTLSEnabled.Equals("Y"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }

                using (var client = new SmtpClient())
                {
                    client.Connect(strsmtpClient, intPortNo, MailKit.Security.SecureSocketOptions.StartTls);
                    if (strIsSSLEnabled.Equals("true"))
                        client.RequireTLS = true;
                    else
                        client.RequireTLS = false;

                    client.Authenticate(strUserName, strPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }

            mlVo.setMailId(intMailLogId);
            mlVo.setMailStatus("S");
            retUpdVal = mlBll.insertIntoMailLog(mlVo);
        }
        catch (Exception ex)
        {
            mlVo.setMailId(intMailLogId);
            mlVo.setMailStatus("F");
            mlVo.setErrorMsg(ex.ToString());
            retUpdVal = mlBll.insertIntoMailLog(mlVo);

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
            distinctArray[i] = inputArray[i];

        return distinctArray;
    }

    public void sendMailWithAttach(string[] strToList, string[] strCCList, string[] strBCCList,
        string strSubject, string strContent, string[] strAttach)
    {
        string strCompleteFilePath = "", strClientFileName = "";
        string[] strAttachmentNames = new string[1];
        try
        {
            MimeMessage message = new MimeMessage();
            intPortNo = Convert.ToInt32(strPortNo);

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
                intMailLogId = mlBll.insertIntoMailLog(mlVo);
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

            message.From.Add(new MailboxAddress("", strMailFrom));

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
                            message.To.Add(new MailboxAddress("", distinctToArray[i]));
                    }
                }

                if (!(distinctCCArray == null))
                {
                    for (i = 0; i <= distinctCCArray.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(distinctCCArray[i]))
                            message.Cc.Add(new MailboxAddress("", distinctCCArray[i]));
                    }
                }
                if (!(strBCCList == null))
                {
                    for (i = 0; i <= strBCCList.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(strBCCList[i]))
                            message.Bcc.Add(new MailboxAddress("", strBCCList[i]));
                    }
                }

                message.Subject = strSubject;

                BodyBuilder builder = new BodyBuilder();
                builder.HtmlBody = strContent;
                if (!(strAttach == null))
                {
                    for (i = 0; i <= strAttach.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(strAttach[i]))
                        {
                            FileInfo fileInfo = new FileInfo(strAttach[i]);
                            builder.Attachments.Add(fileInfo.Name, fileInfo.OpenRead());
                        }
                    }
                }
                message.Body = builder.ToMessageBody();

                if (strIsTLSEnabled.Equals("Y"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }

                using (var client = new SmtpClient())
                {
                    client.Connect(strsmtpClient, intPortNo, MailKit.Security.SecureSocketOptions.StartTls);
                    if (strIsSSLEnabled.Equals("true"))
                        client.RequireTLS = true;
                    else
                        client.RequireTLS = false;

                    client.Authenticate(strUserName, strPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            else if (strMailToBeSent.Equals("2"))
            {
                strContent = "To: " + strToListVal + "<br/><br/>" + "CC: " + strCCListVal + "<br/><br/>" + strContent;

                strTo = strMailTo.Split(',');
                for (int j = 0; j <= strTo.GetUpperBound(0); j++)
                {
                    if (!string.IsNullOrEmpty(strTo[j]))
                        message.To.Add(new MailboxAddress("", strTo[j]));
                }

                message.Subject = strSubject;

                BodyBuilder builder = new BodyBuilder();
                builder.HtmlBody = strContent;
                if (!(strAttach == null))
                {
                    for (i = 0; i <= strAttach.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(strAttach[i]))
                        {
                            FileInfo fileInfo = new FileInfo(strAttach[i]);
                            builder.Attachments.Add(fileInfo.Name, fileInfo.OpenRead());
                        }
                    }
                }
                message.Body = builder.ToMessageBody();

                if (strIsTLSEnabled.Equals("Y"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }

                using (var client = new SmtpClient())
                {
                    client.Connect(strsmtpClient, intPortNo, MailKit.Security.SecureSocketOptions.StartTls);
                    if (strIsSSLEnabled.Equals("true"))
                        client.RequireTLS = true;
                    else
                        client.RequireTLS = false;

                    client.Authenticate(strUserName, strPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }

            mlVo.setMailId(intMailLogId);
            mlVo.setMailStatus("S");
            retUpdVal = mlBll.insertIntoMailLog(mlVo);
        }
        catch (Exception ex)
        {
            mlVo.setMailId(intMailLogId);
            mlVo.setMailStatus("F");
            mlVo.setErrorMsg(ex.ToString());
            retUpdVal = mlBll.insertIntoMailLog(mlVo);

            F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            throw ex;
        }
    }

    public void sendAsyncMailFailure(string[] strToList, string[] strCCList, string[] strBCCList,
       string strSubject, string strContent, string mailId)
    {
        try
        {
            MimeMessage message = new MimeMessage();
            intPortNo = Convert.ToInt32(strPortNo);

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
                    intMailLogId = intMailId;
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }

            message.From.Add(new MailboxAddress("", strMailFrom));

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
                            message.To.Add(new MailboxAddress("", strToList[i]));
                    }
                }
                if (!(strCCList == null))
                {
                    for (i = 0; i <= strCCList.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(strCCList[i]))
                            message.Cc.Add(new MailboxAddress("", strCCList[i]));
                    }
                }
                if (!(strBCCList == null))
                {
                    for (i = 0; i <= strBCCList.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(strBCCList[i]))
                            message.Bcc.Add(new MailboxAddress("", strBCCList[i]));
                    }
                }

                message.Subject = strSubject;
                message.Body = new TextPart("html")
                {
                    Text = strContent
                };

                if (strIsTLSEnabled.Equals("Y"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }

                using (var client = new SmtpClient())
                {
                    client.Connect(strsmtpClient, intPortNo, MailKit.Security.SecureSocketOptions.StartTls);
                    if (strIsSSLEnabled.Equals("true"))
                        client.RequireTLS = true;
                    else
                        client.RequireTLS = false;

                    client.Authenticate(strUserName, strPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            else if (strMailToBeSent.Equals("2"))
            {
                message.To.Add(new MailboxAddress("", strMailTo));
                message.Subject = strSubject;
                message.Body = new TextPart("html")
                {
                    Text = strContent
                };

                if (strIsTLSEnabled.Equals("Y"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }

                using (var client = new SmtpClient())
                {
                    client.Connect(strsmtpClient, intPortNo, MailKit.Security.SecureSocketOptions.StartTls);
                    if (strIsSSLEnabled.Equals("true"))
                        client.RequireTLS = true;
                    else
                        client.RequireTLS = false;

                    client.Authenticate(strUserName, strPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }

            mlVo.setMailId(intMailLogId);
            mlVo.setMailStatus("S");
            retUpdVal = mlBll.insertIntoMailLog(mlVo);
        }
        catch (Exception ex)
        {
            mlVo.setMailId(intMailLogId);
            mlVo.setMailStatus("F");
            mlVo.setErrorMsg(ex.ToString());
            retUpdVal = mlBll.insertIntoMailLog(mlVo);

            F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            throw ex;
        }
    }

    public void sendAsyncMailFailureWithAttach(string[] strToList, string[] strCCList, string[] strBCCList,
        string strSubject, string strContent, string[] strAttach, string strMailId)
    {
        string strCompleteFilePath = "", strClientFileName = "";
        string[] strAttachmentNames = new string[1];
        try
        {
            //Attachment attach;

            MimeMessage message = new MimeMessage();
            intPortNo = Convert.ToInt32(strPortNo);

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
                    intMailLogId = intMailId;
                }
            }
            catch (Exception ex)
            {
                F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }

            message.From.Add(new MailboxAddress("", strMailFrom));

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
                            message.To.Add(new MailboxAddress("", strToList[i]));
                    }
                }
                if (!(strCCList == null))
                {
                    for (i = 0; i <= strCCList.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(strCCList[i]))
                            message.Cc.Add(new MailboxAddress("", strCCList[i]));
                    }
                }
                if (!(strBCCList == null))
                {
                    for (i = 0; i <= strBCCList.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(strBCCList[i]))
                            message.Bcc.Add(new MailboxAddress("", strBCCList[i]));
                    }
                }

                message.Subject = strSubject;

                BodyBuilder builder = new BodyBuilder();
                builder.HtmlBody = strContent;
                if (!(strAttach == null))
                {
                    for (i = 0; i <= strAttach.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(strAttach[i]))
                        {
                            strAttachmentNames = strAttach[i].Split('|');
                            strCompleteFilePath = strAttachmentNames[0];
                            strClientFileName = strAttachmentNames[1];
                            FileInfo fileInfo = new FileInfo(strCompleteFilePath);
                            builder.Attachments.Add(strClientFileName, fileInfo.OpenRead());
                        }
                    }
                }
                message.Body = builder.ToMessageBody();

                if (strIsTLSEnabled.Equals("Y"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }

                using (var client = new SmtpClient())
                {
                    client.Connect(strsmtpClient, intPortNo, MailKit.Security.SecureSocketOptions.StartTls);
                    if (strIsSSLEnabled.Equals("true"))
                        client.RequireTLS = true;
                    else
                        client.RequireTLS = false;

                    client.Authenticate(strUserName, strPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            else if (strMailToBeSent.Equals("2"))
            {
                message.To.Add(new MailboxAddress("", strMailTo));
                message.Subject = strSubject;

                BodyBuilder builder = new BodyBuilder();
                builder.HtmlBody = strContent;
                if (!(strAttach == null))
                {
                    for (i = 0; i <= strAttach.GetUpperBound(0); i++)
                    {
                        if (!string.IsNullOrEmpty(strAttach[i]))
                        {
                            strAttachmentNames = strAttach[i].Split('|');
                            strCompleteFilePath = strAttachmentNames[0];
                            strClientFileName = strAttachmentNames[1];
                            FileInfo fileInfo = new FileInfo(strCompleteFilePath);
                            builder.Attachments.Add(strClientFileName, fileInfo.OpenRead());
                        }
                    }
                }
                message.Body = builder.ToMessageBody();

                if (strIsTLSEnabled.Equals("Y"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }

                using (var client = new SmtpClient())
                {
                    client.Connect(strsmtpClient, intPortNo, MailKit.Security.SecureSocketOptions.StartTls);
                    if (strIsSSLEnabled.Equals("true"))
                        client.RequireTLS = true;
                    else
                        client.RequireTLS = false;

                    client.Authenticate(strUserName, strPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }

            mlVo.setMailId(intMailLogId);
            mlVo.setMailStatus("S");
            retUpdVal = mlBll.insertIntoMailLog(mlVo);
        }
        catch (Exception ex)
        {
            mlVo.setMailId(intMailLogId);
            mlVo.setMailStatus("F");
            mlVo.setErrorMsg(ex.ToString());
            retUpdVal = mlBll.insertIntoMailLog(mlVo);

            F2FLog.Log(ex, (new System.Diagnostics.StackTrace(true)).GetFrame(0).GetFileName(), System.Reflection.MethodBase.GetCurrentMethod().Name);
            throw ex;
        }
    }

}
