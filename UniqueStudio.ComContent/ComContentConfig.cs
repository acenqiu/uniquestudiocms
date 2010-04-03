using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Config;

namespace UniqueStudio.ComContent
{
    public class ComContentConfig : SystemConfig
    {
        /// <summary>
        /// 初始化<see cref="ComContentConfig"/>类的实例。
        /// </summary>
        public ComContentConfig()
        {

        }

        //string
        private string timeFormatOfSectionPostList = "yyyy-MM-dd";

        //int
        private int pageSizeOfSectionPostList = 20;
        private int newImageThreshold = 7;
        private int pictureNewsCategoryId = 1;
        private int pictureNewsInterval = 4000;
        private int pictureNewsNumber = 3;

        //bool
        private bool pictureNewsIsRandom = true;

        //string
        /// <summary>
        /// 子页面文章列表时间显示格式。
        /// </summary>
        public string TimeFormatOfSectionPostList
        {
            get { return timeFormatOfSectionPostList; }
            set { timeFormatOfSectionPostList = value; }
        }

        //int
        /// <summary>
        /// 子页面文章列表显示数量。
        /// </summary>
        public int PageSizeOfSectionPostList
        {
            get { return pageSizeOfSectionPostList; }
            set { pageSizeOfSectionPostList = value; }
        }
        /// <summary>
        /// 定义为新文章的天数。
        /// </summary>
        public int NewImageThreshold
        {
            get { return newImageThreshold; }
            set { newImageThreshold = value; }
        }
        /// <summary>
        /// 图片新闻分类ID。
        /// </summary>
        public int PictureNewsCategoryId
        {
            get { return pictureNewsCategoryId; }
            set { pictureNewsCategoryId = value; }
        }
        /// <summary>
        /// 图片新闻切换间隔时间（单位：毫秒）。
        /// </summary>
        public int PictureNewsInterval
        {
            get { return pictureNewsInterval; }
            set { pictureNewsInterval = value; }
        }
        /// <summary>
        /// 图片新闻显示条数。
        /// </summary>
        public int PictureNewsNumber
        {
            get { return pictureNewsNumber; }
            set { pictureNewsNumber = value; }
        }
        /// <summary>
        /// 图片新闻是否随机切换。
        /// </summary>
        public bool PictureNewsIsRandom
        {
            get { return pictureNewsIsRandom; }
            set { pictureNewsIsRandom = value; }
        }

    }
}
