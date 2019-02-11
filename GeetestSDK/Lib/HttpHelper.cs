using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GeetestSDK.Lib
{
    public class HttpHelper
    {
        private static int _timeout = 100000;
        private static string _agentName = "gudao";

        private CookieContainer cookies;

        private static HttpHelper _current = new HttpHelper() { };

        public static HttpHelper Current { get { return _current; } }
        
        private HttpWebRequest GetWebRequest(string url, string method, string headerStr = null)
        {
            try
            {
                url = HttpUtility.HtmlDecode(url);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.ServicePoint.Expect100Continue = false;
                req.Method = method;
                req.KeepAlive = true;
                req.UserAgent = _agentName;
                req.Timeout = _timeout;
                req.AllowAutoRedirect = true;
                req.CookieContainer = cookies;
                if (headerStr != null)
                {
                    var headers = new WebHeaderCollection();
                    headers.Add(headerStr.Split(':')[0], headerStr.Split(':')[1]);
                    req.Headers = headers;
                }

                return req;
            }
            catch (Exception e)
            {
                throw new Exception("GetWebRequest:" + e);
            }
        }
        /// <summary>
        /// 组装GET请求URL。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>带参数的GET请求URL</returns>
        private string BuildGetUrl(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters);
                }
            }
            return url;
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        private string BuildQuery(IDictionary<string, string> parameters)
        {
            if (parameters == null)
                return "";

            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名空或参数值为Null的参数
                if (!string.IsNullOrEmpty(name) && value != null)
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(Uri.EscapeDataString(value));
                    hasParam = true;
                }
            }

            return postData.ToString();
        }


        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        private string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public string DoGet(string url, IDictionary<string, string> parameters, string charset = "utf-8")
        {
            HttpWebRequest req = GetWebRequest(BuildGetUrl(url, parameters), "GET");

            req.ContentType = "application/x-www-form-urlencoded;charset=" + charset;

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            //cookies.Add(rsp.Cookies);//获取返回值cookie
            Encoding encoding = string.IsNullOrEmpty(rsp.ContentEncoding) ? Encoding.UTF8 : Encoding.GetEncoding(rsp.ContentEncoding);
            return GetResponseAsString(rsp, encoding);
        }


        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="contentString">body内容，form表单用IDictionary</param>
        /// <param name="contentType">form,json,xml</param>
        /// <param name="charset">默认utf-8</param>
        /// <returns></returns>
        public string DoPost(string url, string contentString, string contentType = "form", string charset = "utf-8")
        {
            HttpWebRequest req = GetWebRequest(url, "POST");
            byte[] postData = postData = Encoding.UTF8.GetBytes(contentString);
            switch (contentType)
            {
                case "form":
                    req.ContentType = "application/x-www-form-urlencoded;charset=" + charset;
                    break;

                case "json":
                    req.ContentType = "application/json;charset=" + charset;
                    break;
                case "xml":
                    req.ContentType = "application/xml;charset=" + charset;
                    break;
            }

            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            //cookies.Add(rsp.Cookies);
            Encoding encoding = string.IsNullOrEmpty(rsp.CharacterSet) ? Encoding.UTF8 : Encoding.GetEncoding(rsp.CharacterSet);

            return GetResponseAsString(rsp, encoding);

        }
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">form表单</param>
        /// <returns></returns>
        public string DoPost(string url, IDictionary<string, string> parameters)
        {
            return DoPost(url, BuildQuery(parameters));
        }
        /// <summary>
        /// Multipart 类型POST
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string DoPostMultipart(string url, IDictionary<string, string> parameters)
        {
            // log.Debug(string.Format("Uploading {0} to {1}", file, url));
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in parameters.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, parameters[key]);
                byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);


            byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                //logger.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
                return reader2.ReadToEnd();
            }
            catch (Exception ex)
            {
                //log.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                throw new Exception("请求出错：" + ex.Message);
            }
            finally
            {
                wr = null;
            }
        }

    }
}
