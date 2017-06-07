using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using weixin.Models;

namespace weixin.Controllers
{
    public class weixinController : Controller
    {
        public LogUtil Log = LogUtil.GetTodayLog(false);
        public readonly string Token = "weixin";//与微信公众账号后台的Token设置保持一致，区分大小写。
        public ActionResult Index(string signature, string timestamp, string nonce, string echostr)
        {
            try
            {
                Log.Info("测试输出: echostr = " + echostr);
                Log.Info("测试输出: nonce = " + nonce);
                Log.Info("测试输出: timestamp = " + timestamp);
                Log.Info("测试输出: signature = " + signature);

                string EchoStr = Valid(signature, timestamp, nonce, echostr);

                if (!string.IsNullOrEmpty(EchoStr))
                {
                    Log.Info("验证成功！");
                    return Content(echostr);
                }
                else
                {
                    Log.Info("验证失败！");                    
                    return Content("验证失败！");
                }
            }
            catch (Exception ex)
            {
                Log.Info("Log 测试输出：异常！"+ ex.Message);
                return Content(ex.ToString());
            }
        }
        private string Valid(string signature, string timestamp, string nonce, string echostr)
        {
            if (CheckSignature(signature, timestamp, nonce))
            {
                Log.Info("CheckSignature passed!");
                if (!string.IsNullOrEmpty(echostr))
                {
                    return echostr;
                }
            }

            return "";
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        private bool CheckSignature(string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { Token, timestamp, nonce };
            Array.Sort(ArrTmp); //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            Log.Info("in CheckSignature...tmpStr value is:" + tmpStr);
            Log.Info("in CheckSignature...signature value is:" + signature);
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private HttpResponseMessage ToHttpMsgForWeChat(string strMsg)
        {
            return new HttpResponseMessage { Content = new StringContent(strMsg, Encoding.GetEncoding("UTF-8"), "application/x-www-form-encoded") };     
    }
    }
}
