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
        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        /// <summary>
        /// 创建一个 RESTClient 类的实例。
        /// </summary>
        public RESTClient()
        {
            parameters = new Dictionary<string, string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="host"></param>
        public RESTClient(IDictionary<string, string> parameters, string host)
        {
            this.parameters = parameters;
            this.host = host;
        }

        /// <summary>
        /// 获取响应的数据流。
        /// </summary>
        /// <returns></returns>
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
        /// 获取响应的文本。
        /// </summary>
        /// <returns></returns>
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