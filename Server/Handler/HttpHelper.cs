using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EADS.Server.Handler
{
    public class HttpHelper
    {
        public static string doPostXml(string url,string postData) {
            Stream requestStream = null;
            Stream responsetream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            // 要注意的这是这个编码方式，还有内容的Xml内容的编码方式
            Encoding encoding = Encoding.GetEncoding("UTF-8");
            byte[] data = encoding.GetBytes(postData);

            // 准备请求,设置参数
            request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = data.Length; 
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;

            requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Flush();
            requestStream.Close();
            //发送请求并获取相应回应数据

            response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            responsetream = response.GetResponseStream();

            sr = new StreamReader(responsetream, encoding);
            //返回结果网页(html)代码 
            string content = sr.ReadToEnd();
            return content;
        }
    }
}
