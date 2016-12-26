using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EADS.Common
{
    public class CommonConst
    {
        public static string GetIniFilePath() {
            return Environment.CurrentDirectory + "\\SystemSetting.ini";
        }
        public static string GetPhotoResUrlForWeb() {
            return ConfigurationManager.AppSettings["photoResUrl"];
        }

        public static string GetPhotoResUrlForWeb(string imgUrl)
        {
            return ConfigurationManager.AppSettings["photoResUrl"]+ imgUrl;
        }

        public static string MD5(string data) {
            //MD5 md5 = new MD5CryptoServiceProvider();
            //byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(data));
            //return System.Text.Encoding.Default.GetString(result);

           // //获取加密服务
           //MD5CryptoServiceProvider md5CSP = new MD5CryptoServiceProvider();

           // //获取要加密的字段，并转化为Byte[]数组
           // byte[] encrypt = System.Text.Encoding.Unicode.GetBytes(data);

           // //加密Byte[]数组
           // byte[] resultEncrypt = md5CSP.ComputeHash(encrypt);

           // //将加密后的数组转化为字段(普通加密)
           // string resutl = System.Text.Encoding.Unicode.GetString(resultEncrypt); 
          //  return resutl; 
            string sss = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(data, "MD5");
            //作为密码方式加密 
            return sss;
        }
    }
}
