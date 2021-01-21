using System;
using System.Threading.Tasks;

namespace SinGooCMS.Plugins.SMS
{
    /// <summary>
    /// 发送短信接口
    /// </summary>
    public interface ISMS
    {
        /// <summary>
        /// 短信接口配置
        /// </summary>
        SMSConfig Config { get; set; }

        /// <summary>
        /// 异步发送短信(不用模板)
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="content">短信内容</param>
        /// <returns></returns>
        Task<(bool isSuccess, string errMsg)> SendMsgAsync(string mobile, string content);

        /// <summary>
        /// 异步发送短信(使用模板)
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="tempCode">模板代号</param>
        /// <param name="tempParams">模板参数</param>
        /// <returns></returns>
        Task<(bool isSuccess, string errMsg)> SendMsgAsync(string mobile, string tempCode, string tempParams);
    }
}
