using System;

namespace SinGooCMS.Plugins.SMS
{
    /// <summary>
    /// 短信配置
    /// </summary>
    public class SMSConfig
    {
        /// <summary>
        /// 用户ID/AppKey
        /// </summary>
        public string SMSUId { get; set; }
        /// <summary>
        /// 用户密码/app Secirute
        /// </summary>
        public string SMSPwd { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string APPID { get; set; }
        /// <summary>
        /// 网络节点
        /// </summary>
        public string EndPoint { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string SignName { get; set; }
        /// <summary>
        /// 区域ID 如cn-hangzhou
        /// </summary>
        public string RegionId { get; set; }
    }
}
