using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.CommonEntities;

namespace Warranty.Common.Utility
{
    public class EmailSender
    {
        #region Variables
        private static readonly string SMTPHost = "smtp.gmail.com";
        private static readonly int SMTPPort = 587;
        private static readonly string SMTPUserName = "kinjalr.arudhait@gmail.com";
        private static readonly string SMTPPassword = "wyhg xwzn rlkh jkyp";
        //private static readonly string SMTPAvaUserName = "orders@avamedsupply.com"; 
        #endregion


        #region Methods

        /// <summary>
        /// Method for sending email
        /// </summary>
        /// <param name="mailMessage"></param>
        public static void SendEmail(string ToEmail, string Name, string Subject, string Body, string Filepath = "", string ccEmail = "", string bccEmail = "")//string FromMail,
        {
            SmtpClient smtpClient = new SmtpClient(SMTPHost, SMTPPort);
            try
            {
                MailMessage mailMessage = new MailMessage
                {
                    IsBodyHtml = true,
                    Body = Body,
                    From = new MailAddress(SMTPUserName, Name)
                };
                mailMessage.To.Add(ToEmail);
                if (!string.IsNullOrEmpty(ccEmail))
                    mailMessage.CC.Add(ccEmail);
                if (!string.IsNullOrEmpty(bccEmail))
                    mailMessage.Bcc.Add(bccEmail);
                mailMessage.Subject = Subject;
                if (!string.IsNullOrEmpty(Filepath))
                    mailMessage.Attachments.Add(new Attachment(Filepath));
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(SMTPUserName, SMTPPassword);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for sending email
        /// </summary>
        /// <param name="mailMessage"></param>
        public static ResponseModel SendEmailToCompany(string ToEmail, string Name, string Subject, string Body, string password, string[] Filepath = null, string ccEmail = "")
        {
            ResponseModel model = new ResponseModel();
            SmtpClient smtpClient = new SmtpClient(SMTPHost, SMTPPort);
            try
            {
                MailMessage mailMessage = new MailMessage
                {
                    IsBodyHtml = true,
                    Body = Body,
                    From = new MailAddress(SMTPUserName, Name)
                };
                mailMessage.To.Add(ToEmail);
                if (!string.IsNullOrEmpty(ccEmail))
                    mailMessage.CC.Add(ccEmail);
                mailMessage.Subject = Subject;
                if (Filepath != null && Filepath.Length > 0)
                {
                    foreach (var item in Filepath)
                    {
                        if (!string.IsNullOrEmpty(item))
                            mailMessage.Attachments.Add(new Attachment(item));
                    }
                }
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(SMTPUserName, SMTPPassword);
                smtpClient.Send(mailMessage);
                model.Result = true;
            }
            catch (Exception ex)
            {
                model.Result = false;
                model.Message = ex.Message;
            }
            return model;
        }
        #endregion
    }
}
