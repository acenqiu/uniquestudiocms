using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace UniqueStudio.Common.RESTful
{
    /// <summary>
    /// REST方式的HTTP请求客户端。
    /// </summary>
    public sealed class RESTClient
    {
        private IDictionary<string, string> parameters;
        private string host;

        /// <summary>
        /// 获取用于请求的参数。
        /// </summary>
        public IDictionary<string, string> Parameters
        {
            get
            {
                return parameters;
            }
            private set
            {
                parameters = value;
            }
        }

        /// <summary>
        /// 主机
        /// </summary>
        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        /// <summary>
        /// 初始化<see cref="RESTClient"/>类的实例
        /// </summary>
        public RESTClient()
        {
            parameters = new Dictionary<string, string>();
        }

        /// <summary>
        /// 以参数、主机初始化<see cref="RESTClient"/>类的实例
        /// </summary>
        /// <param name="parameters">参数</param>
        /// <param name="host">主机</param>
        public RESTClient(IDictionary<string, string> parameters, string host)
        {
            this.parameters = parameters;
            this.host = host;
        }

        /// <summary>
        /// 返回响应的数据流
        /// </summary>
        /// <returns>响应的数据流</returns>
        public Stream GetResponseStream()
        {
            string query = CollectionUtil.ToQueryString(this.Parameters);
            byte[] data = Encoding.UTF8.GetBytes(query);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            return request.GetResponse().GetResponseStream();
        }

        /// <summary>
        /// 返回响应的文本
        /// </summary>
        /// <returns>响应的文本</returns>
        public string GetResponseText()
        {
            string text = string.Empty;

            using (StreamReader reader = new StreamReader(this.GetResponseStream(), Encoding.UTF8))
            {
                text = reader.ReadToEnd();
            }

            return text;
        }
    }
}