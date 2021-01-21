using System;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Sms.V20190711.Models;
using TencentCloud.Sms.V20190711;
using System.Collections.Generic;

namespace SinGooCMS.Plugins.SMS
{
    /// <summary>
    /// 腾讯云短信（早期还提供自由短信发送，后来也改成了必须有模板才能发送）
    /// </summary>
    public class QcloudSMS : ISMS
    {
        /// <summary>
        /// 短信配置
        /// </summary>
        public SMSConfig Config { get; set; }

        public async Task<(bool isSuccess, string errMsg)> SendMsgAsync(string mobile, string content)
        {
            return (false, "Templates Send Only/腾讯云仅支持发送模板短信");
        }

        /// <summary>
        /// 发送模板短信
        /// </summary>
        /// <param name="mobile">多个手机号用英文状态下的逗号分隔</param>
        /// <param name="tempCode">模板ID</param>
        /// <param name="tempParams">多个参数英文状态下的逗号分隔，模板中的{数字}代表一个参数</param>
        /// <returns></returns>
        public async Task<(bool isSuccess, string errMsg)> SendMsgAsync(string mobile, string tempCode, string tempParams)
        {
            try
            {
                /* 必要步骤：
                 * 实例化一个认证对象，入参需要传入腾讯云账户密钥对 secretId 和 secretKey
                 * 本示例采用从环境变量读取的方式，需要预先在环境变量中设置这两个值
                 * 您也可以直接在代码中写入密钥对，但需谨防泄露，不要将代码复制、上传或者分享给他人
                 * CAM 密匙查询：https://console.cloud.tencent.com/cam/capi
                 */
                Credential cred = new Credential
                {
                    SecretId = Config.SMSUId,
                    SecretKey = Config.SMSPwd
                };
                /*
                Credential cred = new Credential {
                    SecretId = Environment.GetEnvironmentVariable("TENCENTCLOUD_SECRET_ID"),
                    SecretKey = Environment.GetEnvironmentVariable("TENCENTCLOUD_SECRET_KEY")
                };*/

                /* 非必要步骤:
                 * 实例化一个客户端配置对象，可以指定超时时间等配置 */
                ClientProfile clientProfile = new ClientProfile();
                /* SDK 默认用 TC3-HMAC-SHA256 进行签名
                 * 非必要请不要修改该字段 */
                clientProfile.SignMethod = ClientProfile.SIGN_TC3SHA256;
                /* 非必要步骤
                 * 实例化一个客户端配置对象，可以指定超时时间等配置 */
                HttpProfile httpProfile = new HttpProfile();
                /* SDK 默认使用 POST 方法
                 * 如需使用 GET 方法，可以在此处设置，但 GET 方法无法处理较大的请求 */
                httpProfile.ReqMethod = "POST";
                /* SDK 有默认的超时时间，非必要请不要进行调整
                 * 如有需要请在代码中查阅以获取最新的默认值 */
                httpProfile.Timeout = 10; // 请求连接超时时间，单位为秒(默认60秒)
                /* SDK 会自动指定域名，通常无需指定域名，但访问金融区的服务时必须手动指定域名
                * 例如 SMS 的上海金融区域名为 sms.ap-shanghai-fsi.tencentcloudapi.com */
                httpProfile.Endpoint = Config.EndPoint;
                // 代理服务器，当您的环境下有代理服务器时设定
                //httpProfile.WebProxy = Environment.GetEnvironmentVariable("HTTPS_PROXY");

                clientProfile.HttpProfile = httpProfile;
                /* 实例化 SMS 的 client 对象
                 * 第二个参数是地域信息，可以直接填写字符串 ap-guangzhou，或者引用预设的常量 */
                SmsClient client = new SmsClient(cred, Config.RegionId, clientProfile);

                /* 实例化一个请求对象，根据调用的接口和实际情况，可以进一步设置请求参数
                 * 您可以直接查询 SDK 源码确定 SendSmsRequest 有哪些属性可以设置
                 * 属性可能是基本类型，也可能引用了另一个数据结构
                 * 推荐使用 IDE 进行开发，可以方便地跳转查阅各个接口和数据结构的文档说明 */
                SendSmsRequest req = new SendSmsRequest();

                /* 基本类型的设置:
                * SDK 采用的是指针风格指定参数，即使对于基本类型也需要用指针来对参数赋值
                * SDK 提供对基本类型的指针引用封装函数
                * 帮助链接：
                * 短信控制台：https://console.cloud.tencent.com/sms/smslist
                * sms helper：https://cloud.tencent.com/document/product/382/3773
                */

                req.SmsSdkAppid = Config.APPID;
                /* 短信签名内容: 使用 UTF-8 编码，必须填写已审核通过的签名，可登录 [短信控制台] 查看签名信息 */
                req.Sign = Config.SignName;
                /* 短信码号扩展号: 默认未开通，如需开通请联系 [sms helper] */
                //req.ExtendCode = "x";
                /* 国际/港澳台短信 senderid: 国内短信填空，默认未开通，如需开通请联系 [sms helper] */
                req.SenderId = "";
                /* 用户的 session 内容: 可以携带用户侧 ID 等上下文信息，server 会原样返回 */
                req.SessionContext = System.DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(10000, 100000 - 1).ToString(); ;
                /* 下发手机号码，采用 e.164 标准，+[国家或地区码][手机号]
                 * 例如+8613711112222， 其中前面有一个+号 ，86为国家码，13711112222为手机号，最多不要超过200个手机号*/
                var arrPhones = mobile.Split(',');
                var lstPhone = new List<string>();
                foreach (var item in arrPhones)
                {
                    if (item.Trim().Length > 0)
                        lstPhone.Add("+86" + item);
                }
                req.PhoneNumberSet = lstPhone.ToArray();
                /* 模板 ID: 必须填写已审核通过的模板 ID，可登录 [短信控制台] 查看模板 ID */
                req.TemplateID = tempCode;
                /* 模板参数: 若无模板参数，则设置为空*/
                var arrParams = tempParams.Split(',');
                var lstParams = new List<string>();
                foreach (var item in arrParams)
                {
                    if (item.Trim().Length > 0)
                        lstParams.Add(item);
                }
                req.TemplateParamSet = lstParams.ToArray();


                // 通过 client 对象调用 SendSms 方法发起请求，注意请求方法名与请求对象是对应的
                // 返回的 resp 是一个 SendSmsResponse 类的实例，与请求对象对应
                SendSmsResponse resp = await client.SendSms(req);

                // 输出 JSON 格式的字符串回包
                if (resp.SendStatusSet.Length > 0)
                {
                    if (resp.SendStatusSet[0].Code == "Ok")
                        return (true, "");
                    else
                        return (false, resp.SendStatusSet[0].Message);
                }
                else
                    return (false, "未返回值");

                //Console.WriteLine(AbstractModel.ToJsonString(resp));
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
                //Console.WriteLine(e.ToString());
            }
        }
    }
}
