﻿using System;
using System.IO;
using System.Xml;

namespace SinGooCMS.Plugins.OSS
{
    /// <summary>
    /// 阿里云OSS配置
    /// </summary>
    public class AliyunOSSConfig
    {
        #region 公共属性

        /// <summary>
        /// 网络节点 oss-cn-shenzhen.aliyuncs.com
        /// </summary>
        public string EndPoint { get; set; }
        /// <summary>
        /// Access Key ID（在AccessKey管理中查找）
        /// </summary>
        public string AccessKeyId { get; set; }
        /// <summary>
        /// Access Key Secret（在AccessKey管理中查找）
        /// </summary>
        public string AccessKeySecret { get; set; }
        /// <summary>
        /// 存储空间名称
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// 自定义域名 如file.singoo.top（在传输管理-域名管理中配置域名）
        /// </summary>
        public string CName { get; set; }

        #endregion

        #region 加载和保存配置

        /// <summary>
        /// 从文件中加载配置
        /// </summary>
        /// <param name="configPath">阿里云oss配置，默认路径/config/aliyunoss.config</param>
        /// <returns></returns>
        public static AliyunOSSConfig Load(string configPath = "/config/aliyunoss.config")
        {
            string xmlString = File.ReadAllText(Utils.GetMapPath(configPath));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            return new AliyunOSSConfig()
            {
                EndPoint = doc.SelectSingleNode("AliYunOSS/EndPoint").InnerText,
                AccessKeyId = doc.SelectSingleNode("AliYunOSS/AccessKeyId").InnerText,
                AccessKeySecret = doc.SelectSingleNode("AliYunOSS/AccessKeySecret").InnerText,
                BucketName = doc.SelectSingleNode("AliYunOSS/BucketName").InnerText,
                CName = doc.SelectSingleNode("AliYunOSS/CName").InnerText
            };
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="ossConfig">配置信息</param>
        /// <param name="configPath">阿里云oss配置，默认路径/config/aliyunoss.config</param>
        public static void Save(AliyunOSSConfig ossConfig, string configPath = "/config/aliyunoss.config")
        {
            string str = string.Format("<AliYunOSS><EndPoint>{0}</EndPoint><AccessKeyId>{1}</AccessKeyId><AccessKeySecret>{2}</AccessKeySecret><BucketName>{3}</BucketName><CName>{4}</CName></AliYunOSS>",
                ossConfig.EndPoint, ossConfig.AccessKeyId, ossConfig.AccessKeySecret, ossConfig.BucketName, ossConfig.CName);
            File.WriteAllText(Utils.GetMapPath(configPath), str);
        }

        #endregion
    }
}
