﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.ComContent.Model
{
    [Serializable]
    public class Enclosure
    {
        private string title;
        private long length;
        private string type;
        private string url;
        public Enclosure()
        {
        }
        /// <summary>
        /// 附件名
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        /// <summary>
        /// 附件大小
        /// </summary>
        public long Length
        {
            get { return length; }
            set { length = value; }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Url
        {
            get
            { return url; }
            set { url = value; }
        }
    }
}
