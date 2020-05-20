using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Xray.Tools.HttpToolsLib
{
    /// <summary>
    /// 邮件发送帮助类
    /// </summary>
    public class SMTPHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool SendEmail(MailEntity entity)
        {
            if (!entity.CanSend)
            {
                return false;
            }
            SmtpClient smtpClient = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,//指定电子邮件发送方式
                Host = entity.Host, //指定SMTP服务器
                Credentials = new System.Net.NetworkCredential(entity.FromEmail, entity.PassOrCode),//用户名和密码
                Timeout = 15000,
            };
            // 发送邮件设置        
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(entity.FromEmail, entity.DisplayName),
                Subject = entity.Subject,//主题
                Body = entity.Content,//内容
                BodyEncoding = Encoding.UTF8,//正文编码
                IsBodyHtml = true,//设置为HTML格式
                Priority = MailPriority.Low//优先级
            };
            // 发送人和收件人
            entity.ToEmail?.ForEach(to =>
            {
                mailMessage.To.Add(to);
            });
            entity.CCEmail?.ForEach(cc =>
            {
                mailMessage.CC.Add(cc);
            });
            entity.Attachments?.ForEach(att =>
            {
                mailMessage.Attachments.Add(new Attachment(att));
            });

            try
            {
                smtpClient.Send(mailMessage); // 发送邮件
                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }

    public class MailEntity
    {
        /// <summary>
        /// 自动识别邮件服务器 目前支持163
        /// </summary>
        /// <param name="userEmailAddress"></param>
        /// <returns></returns>
        private static String GetServerHost(String userEmailAddress)
        {
            String host = String.Empty;
            if (Convert.ToBoolean(userEmailAddress?.EndsWith("163.com")))
            {
                host = "smtp.163.com";
            }
            return host;
        }
        /// <summary>
        /// 发送人邮箱
        /// </summary>
        public String FromEmail { get; set; }
        public String DisplayName { get; set; } = "Xray";
        /// <summary>
        /// 密码或授权码
        /// </summary>
        public String PassOrCode { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public String Subject { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public String Content { get; set; }
        /// <summary>
        /// 目标邮箱地址
        /// </summary>
        public List<String> ToEmail { get; set; }
        /// <summary>
        /// 抄送邮箱地址
        /// </summary>
        public List<String> CCEmail { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<String> Attachments { get; set; }
        public String Host { get => GetServerHost(FromEmail); }
        public bool CanSend { get => !(String.IsNullOrEmpty(FromEmail) || String.IsNullOrEmpty(PassOrCode) || ToEmail?.Count == 0); }

    }
}
