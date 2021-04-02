using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.InteropServices;

#if NETSTANDARD2_1
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
#endif

namespace SinGooCMS.Plugins
{
    /// <summary>
    /// 内部工具
    /// </summary>
    internal class Utils
    {
#if NETSTANDARD2_1
        public static IConfiguration Configuration { get; set; }
#endif

        #region 读取绝对路径

        /// <summary>
        /// 读取绝对路径
        /// </summary>
        /// <param name="path">虚拟路径 如 /config/log4net.config</param>
        /// <returns></returns>
        public static string GetMapPath(string path = "/")
        {
#if NETSTANDARD2_1
            if (path.StartsWith(@"/"))
                path = path.TrimStart('/');

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path).Replace(@"\", "/"); //linux 要求所有路径都是 /
            else
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path).Replace("/", @"\"); //windows 路径是 \
#else
            if (System.Web.HttpContext.Current != null)
                return System.Web.HttpContext.Current.Server.MapPath(path);
            else
            {
                path = path.Replace("~/", "/");
                if (path.StartsWith("/"))
                    path = path.TrimStart('/');

                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path).Replace("/", @"\");
            }
#endif
        }

        #endregion

        #region 读取邮件、短信等配置信息

        /// <summary>
        /// 读取AppSetting配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appSettingName"></param>
        /// <returns></returns>
        public static T GetAppSetting<T>(string appSettingName) where T : IConvertible
        {
#if NETSTANDARD2_1
            Configuration = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
            .Build();

            IConfigurationSection AppSettings = Configuration.GetSection("AppSettings");
            string value = AppSettings[appSettingName];
            if (value == null)
            {
                throw new Exception(string.Format("{0} not defined in appSettings.", appSettingName));
            }

            return (T)Convert.ChangeType(value, typeof(T));
#else
            string value = ConfigurationManager.AppSettings[appSettingName];
            if (value == null)
            {
                throw new Exception(string.Format("{0} not defined in appSettings.", appSettingName));
            }

            return (T)Convert.ChangeType(value, typeof(T));
#endif
        }

        #endregion
    }

    /// <summary>
    /// 内部扩展类
    /// </summary>
    internal static class StringExtension
    {
        /// <summary>
        /// 是否为null或者空串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null)
                return true;
            else if (obj.ToString().Trim().Length == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否Email
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static bool IsEmail(this object thisValue)
        {
            if (thisValue == null) return false;
            return Regex.IsMatch(thisValue.ToString(), @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T t)
        {
            if (t == null)
                return "{}";

            return JsonConvert.SerializeObject(t, new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            });
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(this string strJson) =>
            JsonConvert.DeserializeObject<T>(strJson);
    }
}
