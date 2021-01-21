using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SinGooCMS.Plugins.Email;
using System.Threading.Tasks;

namespace NTFxTest
{
    [TestClass]
    public class EmailTest
    {
        [TestMethod]
        public async Task TestEmail()
        {
            IEmail email = MailProvider.Instance;
            email.Config = new MailConfig()
            {
                ServMailAccount = "16826375@qq.com",
                ServMailSMTP = "smtp.qq.com",
                ServMailPort = 465,
                ServMailUserName = "16826375",
                ServMailUserPwd = "nqfxginv111111111111", //私人账号信息有所改动
                ServMailIsSSL = true,
                FromDisplayName = "测试的"
            };
            var result = await email.SendEmailAsync("16826375@qq.com", "这是一个测试", "this<br/>is<br>a<br/>test");
            Assert.AreEqual(result.isSuccess, true);
        }
    }
}
