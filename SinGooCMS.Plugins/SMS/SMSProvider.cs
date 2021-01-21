using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SinGooCMS.Plugins.SMS
{
    /// <summary>
    /// 短信服务提供者
    /// </summary>
    public class SMSProvider
    {
        /// <summary>
        /// 获取默认邮件服务实例
        /// </summary>
        public static ISMS Instance => Create("AliYunSMS");

        /// <summary>
        /// 创建一个指定的短信接口类
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static ISMS Create(string className)
        {
            Assembly tempAssembly = Assembly.LoadFrom(Utils.GetMapPath("SinGooCMS.Plugins.dll"));
            Type type = tempAssembly.GetType("SinGooCMS.Plugins.SMS." + className);
            return (ISMS)(Activator.CreateInstance(type));
        }
    }
}
