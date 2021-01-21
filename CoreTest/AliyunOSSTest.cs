using System;
using NUnit.Framework;
using System.Threading.Tasks;
using SinGooCMS.Plugins.OSS;

namespace CoreTest
{
    public class AliyunOSSTest
    {
        private AliyunOSSConfig config = new AliyunOSSConfig() {
            EndPoint= "oss-cn-shenzhen.aliyuncs.com",
            AccessKeyId= "LTAI3REva111111111111",  //私人账号信息有所改动
            AccessKeySecret = "1DnnagtroDvcka91LOua0000000000",  //私人账号信息有所改动
            BucketName = "singoocms",
            CName="file.singoo.top"
        };

        /// <summary>
        /// 测试OSS
        /// </summary>
        [Test]
        public void TestOSSUpload()
        {
            //上传文件
            AliyunOSS oss = new AliyunOSS(config);
            var result = oss.PutObject("1.png", @"F:\qrcode.png");

            //下载
            oss.GetObject("1.png", @"F:\2.png");
            Assert.AreEqual(true, System.IO.File.Exists(@"F:\2.png"));

            //删除
            oss.DeleteObject("1.png");
        }
    }
}
