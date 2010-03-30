using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.DAL.Uri
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResourceType : byte
    {
        /// <summary>
        /// 文章
        /// </summary>
        Article = 1,
        /// <summary>
        /// 图片
        /// </summary>
        Photo = 2,
        /// <summary>
        /// 视频
        /// </summary>
        Video = 3,
        /// <summary>
        /// 书籍
        /// </summary>
        Book = 4,
        /// <summary>
        /// 任务
        /// </summary>
        Task = 5,
        /// <summary>
        /// 自动保存
        /// </summary>
        AutoSave = 6
    }
}
