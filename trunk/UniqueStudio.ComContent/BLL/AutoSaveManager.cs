using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.ComContent.Model;
using UniqueStudio.ComContent.DAL;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using System.Data.Common;

namespace UniqueStudio.ComContent.BLL
{
    public class AutoSaveManager
    {
        private AutoSaveProvider provider = new AutoSaveProvider();

        public AutoSaveManager()
        {

        }

        /// <summary>
        /// 文章自动保存
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="post">文章信息</param>
        /// <returns>是否自动保存成功</returns>
        public bool AutoSavePost(Guid userID, PostInfo post)
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
                long userUri = Convert.ToInt64("6" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                return provider.AutoSaveFile(userID, post, userUri);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
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
        /// 为添加文章页面获得用户有效的自动保存文章
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns>自动保存的文章信息</returns>
        public PostInfo GetAutoSavedPost(Guid userID)
        {
            Validator.CheckGuid(userID, "userID");

            try
            {
                return provider.GetAutoSavedPost(userID);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }
        /// <summary>
        /// 为编辑页面获得用户有效的自动保存文章
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="postUri">保存文章的Uri</param>
        /// <returns>自动保存的文章信息</returns>
        public PostInfo GetEftAutoSavedFileForEdit(Guid userID, Int64 postUri)
        {
            return provider.GetAutoSavedFileForEdit(userID, postUri);
        }
    }
}
