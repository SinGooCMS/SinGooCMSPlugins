using System;
using System.IO;
using Aliyun.OSS;
using Aliyun.OSS.Common;

namespace SinGooCMS.Plugins.OSS
{
    /// <summary>
    /// 阿里云存储（OSS）可以提高文件（如图片）的访问响应速度
    /// </summary>
    public class AliyunOSS
    {
        private static OssClient client;

        /// <summary>
        /// OSS配置
        /// </summary>
        public AliyunOSSConfig Config { get; set; }

        /// <summary>
        /// 无参构造函数，必须要设置Config属性
        /// </summary>
        public AliyunOSS() : this(null) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_config"></param>
        public AliyunOSS(AliyunOSSConfig _config)
        {
            if (_config != null)
                this.Config = _config;

            if (client == null)
            {
                if (!string.IsNullOrEmpty(Config.CName))
                {
                    var conf = new ClientConfiguration();
                    conf.IsCname = true;
                    //使用自定义的域名
                    client = new OssClient(Config.CName, Config.AccessKeyId, Config.AccessKeySecret, conf);
                }
                else
                    client = new OssClient(Config.EndPoint, Config.AccessKeyId, Config.AccessKeySecret); //使用默认的域名
            }
        }

        #region 云存储操作

        #region 创建一个新的存储空间

        /// <summary>
        /// 在OSS中创建一个新的存储空间。
        /// </summary>
        /// <param name="bucketName">要创建的存储空间的名称</param>
        public Bucket CreateBucket(string bucketName)
        {
            return client.CreateBucket(bucketName);
        }

        /// <summary>
        /// 创建一个模拟的文件夹
        /// </summary>
        /// <param name="folderName">虚拟目录名称</param>
        public void CreateFolder(string folderName)
        {
            // 重要: 这时候作为目录的key必须以斜线（／）结尾
            string key = folderName + "/";
            // 此时的目录是一个内容为空的文件
            using (var stream = new MemoryStream())
            {
                client.PutObject(Config.BucketName, key, stream);
            }
        }

        #endregion

        #region 上传文件

        /// <summary>
        /// 上传指定的文件到指定的OSS的存储空间
        /// </summary>
        /// <param name="key">文件的在OSS上保存的名称</param>
        /// <param name="fileToUpload">指定上传文件的本地路径</param>
        public PutObjectResult PutObject(string key, string fileToUpload)
        {
            return client.PutObject(Config.BucketName, key, fileToUpload);
        }

        /// <summary>
        /// 上传指定的文件到指定的OSS的存储空间
        /// </summary>
        /// <param name="key">文件的在OSS上保存的名称</param>
        /// <param name="fileStream">上传的文件流</param>
        public PutObjectResult PutObject(string key, Stream fileStream)
        {
            return client.PutObject(Config.BucketName, key, fileStream);
        }

        #endregion

        #region 获取对象

        /// <summary>
        /// 列出指定存储空间的文件列表(每次最多100条)
        /// </summary>
        /// <param name="bucketName">存储空间的名称</param>
        public ObjectListing ListObjects(string bucketName = "")
        {
            if (string.IsNullOrEmpty(bucketName))
                bucketName = Config.BucketName; //一个网站指定一个存储空间

            var listObjectsRequest = new ListObjectsRequest(bucketName);
            return client.ListObjects(listObjectsRequest);
        }

        /// <summary>
        /// 从指定的OSS存储空间中获取指定的文件
        /// </summary>
        /// <param name="key">要获取的文件在OSS上的名称</param>
        /// <param name="fileToDownload">本地存储下载文件的目录<param>
        public void GetObject(string key, string fileToDownload)
        {
            try
            {
                var obj = client.GetObject(Config.BucketName, key);

                //将从OSS读取到的文件写到本地
                using (var requestStream = obj.Content)
                {
                    byte[] buf = new byte[1024];
                    using (var fs = File.Open(fileToDownload, FileMode.OpenOrCreate))
                    {
                        var len = 0;
                        while ((len = requestStream.Read(buf, 0, 1024)) != 0)
                        {
                            fs.Write(buf, 0, len);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 删除文件

        /// <summary>
        /// 删除指定的文件
        /// </summary>   
        /// <param name="key">待删除的文件名称</param>
        public void DeleteObject(string key)
        {
            client.DeleteObject(Config.BucketName, key);
        }

        #endregion

        #endregion
    }
}
