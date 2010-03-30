using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.ComContent.Model;
using UniqueStudio.ComContent.DAL;

namespace UniqueStudio.ComContent.BLL
{
    public class AutoSaveManager
    {
        public AutoSaveManager()
        {
        }
        AutoSaveProvider provider = new AutoSaveProvider();

        /// <summary>
        /// 文章自动保存
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="post">文章信息</param>
        /// <returns>是否自动保存成功</returns>
        public bool AutoSaveFile(Guid userID, PostInfo post, Int64 postUri)
        {
            if (post.Title == null)
            {
                post.Title = string.Empty;
            }
            if (post.SubTitle == null)
            {
                post.SubTitle = string.Empty;
            }
            if (post.Author == null)
            {
                post.Author = string.Empty;
            }
            if (post.AddUserName == null)
            {
                post.AddUserName = string.Empty;
            }
            if (post.Summary == null)
            {
                post.Summary = string.Empty;
            }
            if (post.Content.Trim().ToString() == "")
            {
                return false;
            }
            try
            {
                return provider.AutoSaveFile(userID, post, postUri);
            }
            catch
            {

                // throw;
                return false;
            }
        }

        /// <summary>
        /// 修改自动保存文章是否有效
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="isEffictive">是否有效</param>
        /// <returns>修改是否成功</returns>
        public bool SetAutoSaveFileEft(Guid userID, bool isEffictive)
        {
            try
            {
                return provider.SetAutoSaveFileEft(userID, isEffictive);
            }
            catch (Exception)
            {

                return false;
            }

        }

        /// <summary>
        /// 获得用户有效的自动保存文章
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns>自动保存的文章信息</returns>
        public PostInfo GetEftAutoSaveFile(Guid userID)
        {
            try
            {
                return provider.GetAutoSavedFile(userID);
            }
            catch
            {

                return null;
            }
        }
    }
}
