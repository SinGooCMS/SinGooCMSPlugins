using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SinGooCMS.Plugins.SMS;
using System.Threading.Tasks;

namespace NTFxTest
{
    [TestClass]
    public class SMSTest
    {
        [TestMethod]
        public async Task TestAliyunSMS()
        {
            ISMS sms = SMSProvider.Create("AliYunSMS");
            sms.Config = new SMSConfig()
            {
                SMSUId = "LTAI3RE11111111111", //私人账号信息有所改动
                SMSPwd = "1DnnagtroDvcka9100000000000", //私人账号信息有所改动
                EndPoint = "dysmsapi.aliyuncs.com",
                SignName= "新谷",
                RegionId= "cn-hangzhou"
            };

            var result =await sms.SendMsgAsync("17788760902", "SMS_25585959", "code:123456");
            Assert.AreEqual(result.isSuccess, true);
        }

        [TestMethod]
        public async Task TestQCloudSMS()
        {
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
