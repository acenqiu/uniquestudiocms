using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    public class PageVisitInfo
    {
        private int id;
        private string rawUrl;
        private string userHostAddress;
        private string userHostName;
        private string userAgent;
        private string urlReferrer=string.Empty;
        private DateTime time;

        public PageVisitInfo()
        {
        }

        public PageVisitInfo(string rawUrl, string userHostAddress, string userHostName, string userAgent, string urlReferrer)
        {
            this.rawUrl = rawUrl;
            this.userHostAddress = userHostAddress;
            this.userHostName = userHostName;
            this.userAgent = userAgent;
            this.urlReferrer = urlReferrer;
        }

        public PageVisitInfo(string rawUrl, string userHostAddress, string userHostName, string userAgent,string urlReferrer,DateTime time)
            :this(rawUrl,userHostAddress,userHostName,userAgent,urlReferrer)
        {
            this.time = time;
        }

        public PageVisitInfo(int id,string rawUrl, string userHostAddress, string userHostName, string userAgent,string urlReferrer, DateTime time)
            :this(rawUrl,userHostAddress,userHostName,userAgent,urlReferrer)
        {
            this.id = id;
            this.time = time;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string RawUrl
        {
            get { return rawUrl; }
            set { rawUrl = value; }
        }
        
        public string UserHostAddress
        {
            get { return userHostAddress; }
            set { userHostAddress = value; }
        }
        public string UserHostName
        {
            get { return userHostName; }
            set { userHostName = value; }
        }
        public string UserAgent
        {
            get { return userAgent; }
            set { userAgent = value; }
        }
        public string UrlReferrer
        {
            get { return urlReferrer; }
            set { urlReferrer = value; }
        }
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
       
    }
}
