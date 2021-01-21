using System;
using System.Reflection;

namespace SinGooCMS.Plugins.Email
{
    /// <summary>
    /// 邮件服务提供者
    /// </summary>
    public class MailProvider
    {
        /// <summary>
        /// 获取默认邮件服务实例
        /// </summary>
        public static IEmail Instance => Create("KitMail");

        /// <summary>
        /// 创建邮件服务对象
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static IEmail Create(string className)
        {
            Assembly tempAssembly = Assembly.LoadFrom(Utils.GetMapPath("SinGooCMS.Plugins.dll"));
            Type type = tempAssembly.GetType("SinGooCMS.Plugins.Email." + className);
            return (IEmail)(Activator.CreateInstance(type));
        }
    }
}
