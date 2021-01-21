using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;

namespace SinGooCMS.Plugins.SMS
{
    /// <summary>
    /// 阿里云短信
    /// </summary>
    public class AliYunSMS : ISMS
    {
        //产品名称:云通信短信API产品,开发者无需替换
        const String product = "Dysmsapi";
        //产品域名,开发者无需替换
        const String domain = "dysmsapi.aliyuncs.com";

        public SMSConfig Config { get; set; }

        /// <summary>
        /// 阿里云仅支持发送模板短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<(bool isSuccess, string errMsg)> SendMsgAsync(string mobile, string content)
        {
            return (false, "Templates Send Only/阿里云仅支持发送模板短信");
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile">手机号码（多个手机号用英文逗号分隔）</param>
        /// <param name="tempCode">云模板代号</param>
        /// <param name="tempParams">模板参数，格式如 code:123456,name:张三</param>
        /// <returns></returns>
        public async Task<(bool isSuccess, string errMsg)> SendMsgAsync(string mobile, string tempCode, string tempParams)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] arrParams = tempParams.Split(',');
            foreach (string item in arrParams)
            {
                if (item.IndexOf(":") != -1)
                    dict.Add(item.Split(':')[0], item.Split(':')[1]);
            }

            return SendSms(mobile, tempCode, dict);
        }

        #region 发送短信功能

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNums">手机号码，多个手机号码用英文逗号间隔</param>
        /// <param name="tempCode">模板代码</param>
        /// <param name="param">模板参数</param>
        /// <returns></returns>
        private (bool isSuccess, string errMsg) SendSms(string phoneNums, string tempCode, Dictionary<string, string> param)
        {
            IClientProfile profile = DefaultProfile.GetProfile(Config.EndPoint, Config.SMSUId, Config.SMSPwd);
            profile.AddEndpoint(Config.EndPoint, Config.EndPoint, product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            SendSmsResponse response = null;
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = phoneNums;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = Config.SignName;
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = tempCode;
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = param.ToJson(); //"{\"customer\":\"123\"}";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = System.DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(10000, 100000 - 1).ToString();
                request.RegionId = Config.RegionId;
                //请求失败这里会抛ClientException异常
                response = acsClient.GetAcsResponse(request);

                if (response.Message == "OK")
                    return (true, "");
                else
                    return (false, response.Message);

            }
            catch (ServerException e)
            {
                return (false, e.ErrorCode + "|" + e.Message);
            }
            catch (ClientException e)
            {
                return (false, e.ErrorCode + "|" + e.Message);
            }
        }

        /// <summary>
        /// 批量发送短信
        /// </summary>
        /// <param name="phoneNums"></param>
        /// <param name="signs"></param>
        /// <param name="tempCode"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private (bool isSuccess, string errMsg) SendSms(IList<string> phoneNums, IList<string> signs, string tempCode, Dictionary<string, string> param)
        {
            IClientProfile profile = DefaultProfile.GetProfile(Config.EndPoint, Config.SMSUId, Config.SMSPwd);
            profile.AddEndpoint(Config.EndPoint, Config.EndPoint, product, domain);

            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendBatchSmsRequest request = new SendBatchSmsRequest();
            //request.Protocol = ProtocolType.HTTPS;
            //request.TimeoutInMilliSeconds = 1;

            SendBatchSmsResponse response = null;
            try
            {
                //必填:待发送手机号。支持JSON格式的批量调用，批量上限为100个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumberJson = phoneNums.ToJson(); //"[\"1500000000\",\"1500000001\"]";
                //必填:短信签名-支持不同的号码发送不同的短信签名
                request.SignNameJson = signs.ToJson(); //"[\"云通信\",\"云通信\"]";
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = tempCode; //"SMS_1000000";
                //必填:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                //友情提示:如果JSON中需要带换行符,请参照标准的JSON协议对换行符的要求,比如短信内容中包含\r\n的情况在JSON中需要表示成\\r\\n,否则会导致JSON在服务端解析失败
                //request.TemplateParamJson = "[{\"name\":\"Tom\", \"code\":\"123\"},{\"name\":\"Jack\", \"code\":\"456\"}]";
                //StringBuilder builder = new StringBuilder();
                //foreach (var item in param)
                //    builder.Append("{\"" + item.Key + "\":\"" + item.Value + "\"},");
                //request.TemplateParamJson = "[" + builder.ToString().TrimEnd(',') + "]";
                request.TemplateParamJson = param.ToJson();
                //可选-上行短信扩展码(扩展码字段控制在7位或以下，无特殊需求用户请忽略此字段)
                //request.SmsUpExtendCodeJson = "[\"90997\",\"90998\"]";

                //请求失败这里会抛ClientException异常
                response = acsClient.GetAcsResponse(request);
                if (response.Message == "OK")
                    return (true, "");
                else
                    return (false, response.Message);

            }
            catch (ServerException e)
            {
                return (false, e.ErrorCode + "|" + e.Message);
            }
            catch (ClientException e)
            {
                return (false, e.ErrorCode + "|" + e.Message);
            }
        }

        #endregion
    }
}
