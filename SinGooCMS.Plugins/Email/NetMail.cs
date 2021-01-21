using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SinGooCMS.Plugins.Email
{
    /// <summary>
    /// .net自带的邮件组件
    /// </summary>
    public class NetMail : IEmail
    {
        public MailConfig Config { get; set; }

        #region 发送邮件

        public async Task<(bool isSuccess, string errMsg)> SendEmailAsync(string rctMail, string subject, string body)
        {
            try
            {
                if (rctMail.Trim().Length == 0)
                    return (false, "没有找到收件箱地址");

                #region 发送邮件

                MailMessage msg = new MailMessage();
                MailAddress addrFrom = new MailAddress(Config.ServMailAccount);
                if (!string.IsNullOrEmpty(Config.FromDisplayName))
                    addrFrom = new MailAddress(Config.ServMailAccount, Config.FromDisplayName);
                msg.From = addrFrom;

                foreach (string item in rctMail.Split(','))
                {
                    if (!string.IsNullOrEmpty(item) && item.IsEmail())
                    {
                        MailAddress addrTo = new MailAddress(item);
                        msg.To.Add(addrTo);
                    }
                }

                msg.Subject = subject;
                msg.Body = body;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = true; //启用html
                msg.Priority = MailPriority.Normal; //优先级为普通

                SmtpClient sc = new SmtpClient(Config.ServMailSMTP, Config.ServMailPort)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false, //不用默认的凭据
                    Credentials = new NetworkCredential(Config.ServMailUserName, Config.ServMailUserPwd), //身份凭据
                    EnableSsl = Config.ServMailIsSSL //System.Net.Mail支持Explicit SSL但是不支持Implicit SSL，所以正常25端口用此方法发送
                };

                await sc.SendMailAsync(msg);
                return (true, "");

                #endregion
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        #endregion
    }
}
