using System;
using System.Threading.Tasks;

namespace SinGooCMS.Plugins.Email
{
    public interface IEmail
    {
        /// <summary>
        /// 邮件服务器配置
        /// </summary>
        MailConfig Config { get; set; }

        /// <summary>
        /// 异步发送邮件
        /// </summary>
        /// <param name="rctMail">接收邮箱，多个用英文逗号分隔</param>
        /// <param name="subject">标题</param>
        /// <param name="body">内容</param>
        /// <returns>返回结果 .isSuccess=true表示发送成功,.errMsg可查看失败原因</returns>
        Task<(bool isSuccess, string errMsg)> SendEmailAsync(string rctMail, string subject, string body);
    }
}
