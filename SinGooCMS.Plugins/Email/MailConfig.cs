using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinGooCMS.Plugins.Email
{
    /// <summary>
    /// 邮件服务配置
    /// </summary>
    public class MailConfig
    {
        /// <summary>
        /// SMTP服务器
        /// </summary>
        public string ServMailSMTP { get; set; }
        /// <summary>
        /// SMTP端口 默认25，为了保证安全，建议启用465等SSL端口
        /// </summary>
        public int ServMailPort { get; set; }
        /// <summary>
        /// 邮箱账户
        /// </summary>
        public string ServMailAccount { get; set; }
        /// <summary>
        /// 邮箱用户名
        /// </summary>
        public string ServMailUserName { get; set; }
        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string ServMailUserPwd { get; set; }
        /// <summary>
        /// 发送方别名，这时使用站点名称
        /// </summary>
        public string FromDisplayName { get; set; }
        /// <summary>
        /// 是否启用SSL，启用用使用465端口/否则使用25端口
        /// </summary>
        public bool ServMailIsSSL { get; set; }
    }
}
