using System;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using System.Threading.Tasks;

namespace SinGooCMS.Plugins.Email
{
    /// <summary>
    /// 第三方KitMail邮件组件
    /// </summary>
    public class KitMail : IEmail
    {
        /*
         * MailKit 是一款免费开源的.net邮件发送组件，支持跨平台，发送邮件主要使用这个组件
         * git：https://github.com/jstedfast/MailKit
         */

        public MailConfig Config { get; set; }

        public async Task<(bool isSuccess, string errMsg)> SendEmailAsync(string rctMail, string subject, string body)
        {
            try
            {
                if (rctMail.Trim().Length == 0)
                    return (false, "没有找到收件箱地址");

                #region 发送邮件

                var message = new MimeMessage();
                var addrFrom = MailboxAddress.Parse(Config.ServMailAccount);
                if (!string.IsNullOrEmpty(Config.FromDisplayName))
                    addrFrom = new MailboxAddress(Config.FromDisplayName, Config.ServMailAccount);
                message.From.Add(addrFrom);

                foreach (string item in rctMail.Split(','))
                {
                    if (!string.IsNullOrEmpty(item) && item.IsEmail())
                    {
                        message.To.Add(MailboxAddress.Parse(item));
                    }
                }
                message.Subject = subject;
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(Config.ServMailSMTP, Config.ServMailPort, Config.ServMailIsSSL);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(Config.ServMailUserName, Config.ServMailUserPwd);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);

                    return (true, "");
                }

                #endregion
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
