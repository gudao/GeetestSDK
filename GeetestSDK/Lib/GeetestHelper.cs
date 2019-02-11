using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GeetestSDK.Lib
{
    public class GeetestHelper
    {
        /// <summary>
        /// SDK版本号
        /// </summary>
        private const String version = "3.2.0";

        private const String publicKey = "e3a12ada076daaba7cabc989b0faa995";
        private const String privateKey = "6bc181a912fc2007f87c8dfbb7d7d0ca";

        //public const String publicKey = "48a6ebac4ebc6642d68c217fca33eb4d";
        //public const String privateKey = "4f1c085290bec5afdc54df73535fc361";

        public static ChallengeResult registerChallenge(string clientType = "web", string userID = "")
        {
            var parameter = new Dictionary<string, string>();
            //gt={2}&user_id={3}&client_type={4}&ip_address={5}
            parameter.Add("gt", publicKey);
            //parameter.Add("user_id", userID);
            parameter.Add("client_type", clientType);
            parameter.Add("ip_address", "unknnow");
            string strChallenge = HttpHelper.Current.DoGet("http://api.geetest.com/register.php", parameter);

            
            //检查结果
            if (strChallenge.Length == 32)
            {
                return new ChallengeResult()
                {
                    challenge = GetMd5HashStr(strChallenge + privateKey),
                    gt = publicKey,
                    new_captcha = true,
                    success = 1

                };
                
            }
            else
            {
                return new ChallengeResult()
                {
                    challenge = GetMd5HashStr(new Random().Next().ToString()),
                    gt = publicKey,
                    new_captcha = true,
                    success = 0
                };
            }

        }

        public static bool enhencedValidateRequest(string challenge, string validate, string seccode, bool isOnline=true, string userID="")
        {
            if (!isOnline)
            {
                return validate == GetMd5HashStr(challenge);
            }

            String encodeStr = GetMd5HashStr(privateKey + "geetest" + challenge);

            if (validate.Length > 0 && validate == encodeStr)
            {
                var parameter = new Dictionary<string, string>();
                parameter.Add("seccode", seccode);
                //parameter.Add("user_id", userID);
                parameter.Add("sdk", "csharp_" + version);
                string strChallenge = HttpHelper.Current.DoPost("http://api.geetest.com/validate.php", parameter);
                var dd = GetMd5HashStr(seccode);
                if (strChallenge.Equals(dd))
                {
                    return true;
                }
            }

            return false;

        }

        private static string GetMd5HashStr(string str)
        {
            string pwd = string.Empty;

            //实例化一个md5对像
            MD5 md5 = MD5.Create();

            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("x");
            }
            return pwd;
        }
    }

    public class ChallengeResult
    {
        public int success { get; set; }
        public string gt { get; set; }
        public string challenge { get; set; }
        public bool new_captcha { get; set; }
    }
}
