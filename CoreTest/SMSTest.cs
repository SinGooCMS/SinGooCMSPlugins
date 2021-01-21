using NUnit.Framework;
using SinGooCMS.Plugins.SMS;
using System.Threading.Tasks;

namespace CoreTest
{
    public class SMSTest
    {
        [Test]
        public async Task TestAliyunSMS()
        {
            ISMS sms = SMSProvider.Create("AliYunSMS");
            sms.Config = new SMSConfig()
            {
                SMSUId = "LTAI3R1111111", //私人账号信息有所改动
                SMSPwd = "1DnnagtroDvcka91L0000000000", //私人账号信息有所改动
                EndPoint = "dysmsapi.aliyuncs.com",
                SignName = "新谷",
                RegionId = "cn-hangzhou"
            };

            var result = await sms.SendMsgAsync("17788760902,18296860929", "SMS_25585959", "code:123456");
            Assert.AreEqual(result.isSuccess, true);
        }

        [Test]
        public async Task TestQCloudSMS()
        {
            /*
             账户->访问管理->访问密钥->API密钥管理里查看，如果没有就新建
            SecretId: AKIDoJuEHS81PfgXsz11111111111
            SecretKey: ay9lokbSCkFd3NRI0Z0000000000000

            短信->应用管理里面查看
            SDK AppID : 1400370664
             */
            ISMS sms = SMSProvider.Create("QcloudSMS");
            sms.Config = new SMSConfig()
            {
                SMSUId = "AKIDoJuEHS81PfgXszA1111111111111", //私人账号信息有所改动
                SMSPwd = "ay9lokbSCkFd3NRI0ZHo0000000000000", //私人账号信息有所改动
                APPID = "1400370664",
                EndPoint = "sms.tencentcloudapi.com",
                SignName = "singootop",
                RegionId = "ap-guangzhou"
            };

            var result = await sms.SendMsgAsync("17788760902", "850464", "123456");
            Assert.AreEqual(result.isSuccess, true);
        }
    }
}
