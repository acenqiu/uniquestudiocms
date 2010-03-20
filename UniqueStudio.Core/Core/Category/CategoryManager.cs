//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供分类管理的方法。
// 完成日期：2010年03月19日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Data.Common;
using System.Text.RegularExpressions;

using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Permission;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.Core.Category
{
    /// <summary>
    /// 提供分类管理的方法。
    /// </summary>
    public class CategoryManager
    {
        private static Regex rNiceName = new Regex(RegularExpressions.SPECIAL_STRING);
        private static readonly ICategory provider = DALFactory.CreateCategory();

        private UserInfo currentUser;

        /// <summary>
        /// 初始化<see cref="CategoryManager"/>类的实例。
        /// </summary>
        public CategoryManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以当前用户初始化<see cref="CategoryManager"/>类的实例。
        /// </summary>
        /// <param name="currentUser">当前用户。</param>
        public CategoryManager(UserInfo currentUser)
        {
            Validator.CheckNull(currentUser, "currentUser");
            this.currentUser = currentUser;
        }

        /// <summary>
        /// 创建分类。
        /// </summary>
        /// <remarks>
        /// <paramref name="category"/>必须包含分类名和分类别名，且分类别名仅由字母、下划线和数字构成。
        /// </remarks>
        /// <param name="category">分类信息。</param>
        /// <returns>创建成功返回包含分类ID的实例，否则返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建分类的权限时抛出该异常。</exception>
        public CategoryInfo CreateCategory(CategoryInfo category)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CategoryManager(UserInfo)实例化该类。");
            }
            return CreateCategory(currentUser, category);
        }

        /// <summary>
        /// 创建分类。
        /// </summary>
        /// <remarks>
        /// <paramref name="category"/>必须包含分类名和分类别名，且分类别名仅由字母、下划线和数字构成。
        /// </remarks>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="category">分类信息。</param>
        /// <returns>创建成功返回包含分类ID的实例，否则返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建分类的权限时抛出该异常。</exception>
        public CategoryInfo CreateCategory(UserInfo currentUser, CategoryInfo category)
        {
            Validator.CheckNull(category, "category");
            Validator.CheckNotPositive(category.SiteId, "category.SiteId");
            Validator.CheckStringNull(category.CategoryName, "category.CategoryName");
            Validator.CheckStringNull(category.CategoryNiceName, "category.CategoryNiceName");
            Validator.CheckNegative(category.ParentCategoryId, "category.ParentCategoryId");
            if (!rNiceName.IsMatch(category.CategoryNiceName))
            {
                throw new ArgumentException("分类别名只能由字母、下划线、数字构成。");
            }

            PermissionManager.CheckPermission(currentUser, "CreateCategory", "创建分类");

            if (IsCategoryNiceNameExists(category.SiteId, category.CategoryNiceName))
            {
                throw new Exception("该分类别名已存在，请重新设置！");
            }

            try
            {
                return provider.CreateCategory(category);
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
        /// 删除分类。
        /// </summary>
        /// <remarks>根据全局配置决定是否删除子分类。</remarks>
        /// <param name="categoryId">待删除分类ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除分类的权限时抛出该异常。</exception>
        public bool DeleteCategory(int categoryId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CategoryManager(UserInfo)实例化该类。");
            }
            return DeleteCategory(currentUser, categoryId);
        }

        /// <summary>
        /// 删除分类。
        /// </summary>
        /// <remarks>根据全局配置决定是否删除子分类。</remarks>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="categoryId">待删除分类ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除分类的权限时抛出该异常。</exception>
        public bool DeleteCategory(UserInfo currentUser, int categoryId)
        {
            return DeleteCategory(currentUser, categoryId, false);
        }

        /// <summary>
        /// 删除分类。
        /// </summary>
        /// <param name="categoryId">待删除分类ID。</param>
        /// <param name="isDeleteChildCategories">是否删除子分类。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除分类的权限时抛出该异常。</exception>
        public bool DeleteCategory(int categoryId, bool isDeleteChildCategories)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CategoryManager(UserInfo)实例化该类。");
            }
            return DeleteCategory(currentUser, categoryId, isDeleteChildCategories);
        }

        /// <summary>
        /// 删除分类。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="categoryId">待删除分类ID。</param>
        /// <param name="isDeleteChildCategories">是否删除子分类。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除分类的权限时抛出该异常。</exception>
        public bool DeleteCategory(UserInfo currentUser, int categoryId, bool isDeleteChildCategories)
        {
            Validator.CheckNotPositive(categoryId, "categoryId");
            PermissionManager.CheckPermission(currentUser, "DeleteCategory", "删除分类");

            try
            {
                return DeleteCategory(currentUser, categoryId, isDeleteChildCategories);
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
        /// 删除多个分类。
        /// </summary>
        /// <param name="categoryIds">待删除分类ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除分类的权限时抛出该异常。</exception>
        public bool DeleteCategories(int[] categoryIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CategoryManager(UserInfo)实例化该类。");
            }
            return DeleteCategories(currentUser, categoryIds);
        }

        /// <summary>
        /// 删除多个分类。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="categoryIds">待删除分类ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除分类的权限时抛出该异常。</exception>
        public bool DeleteCategories(UserInfo currentUser, int[] categoryIds)
        {
            return DeleteCategories(currentUser, categoryIds, false);
        }

        /// <summary>
        /// 删除多个分类。
        /// </summary>
        /// <param name="categoryIds">待删除分类ID的集合。</param>
        /// <param name="isDeleteChildCategories">是否删除子分类。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除分类的权限时抛出该异常。</exception>
        public bool DeleteCategories(int[] categoryIds, bool isDeleteChildCategories)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CategoryManager(UserInfo)实例化该类。");
            }
            return DeleteCategories(currentUser, categoryIds, isDeleteChildCategories);
        }

        /// <summary>
        /// 删除多个分类。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="categoryIds">待删除分类ID的集合。</param>
        /// <param name="isDeleteChildCategories">是否删除子分类。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除分类的权限时抛出该异常。</exception>
        public bool DeleteCategories(UserInfo currentUser, int[] categoryIds, bool isDeleteChildCategories)
        {
            Validator.CheckNull(categoryIds, "categoryIds");
            for (int i = 0; i < categoryIds.Length; i++)
            {
                Validator.CheckNotPositive(categoryIds[i], "categoryIds");
            }
            PermissionManager.CheckPermission(currentUser, "DeleteCategory", "删除分类");

            try
            {
                return provider.DeleteCategories(categoryIds, isDeleteChildCategories);
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
        /// 返回所有分类。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <returns>包含所有信息的分类集合，如果获取失败返回空。</returns>
        public CategoryCollection GetAllCategories(int siteId)
        {
            Validator.CheckNotPositive(siteId, "siteId");

            try
            {
                return provider.GetAllCategories(siteId);
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
        /// 根据分类ID返回分类信息。
        /// </summary>
        /// <param name="categoryId">分类ID。</param>
        /// <returns>分类信息，获取失败返回空。</returns>
        public CategoryInfo GetCategory(int categoryId)
        {
            Validator.CheckNotPositive(categoryId, "categoryId");

            try
            {
                return provider.GetCategory(categoryId);
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
        /// 根据分类别名返回分类信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="catNiceName">分类别名。</param>
        /// <returns>分类信息，获取失败返回空。</returns>
        public CategoryInfo GetCategory(int siteId, string catNiceName)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            Validator.CheckStringNull(catNiceName, "catNiceName");
            if (!rNiceName.IsMatch(catNiceName))
            {
                throw new ArgumentException("分类别名格式不正确");
            }

            try
            {
                return provider.GetCategory(siteId, catNiceName);
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
        /// 返回分类路径。
        /// </summary>
        /// <param name="categoryId">叶节点分类ID。</param>
        /// <returns>分类路径根节点。</returns>
        public CategoryInfo GetCategoryPath(int categoryId)
        {
            Validator.CheckNotPositive(categoryId, "categoryId");

            try
            {
                return provider.GetCategoryPath(categoryId);
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
        /// 根据分类ID返回其子分类信息。
        /// </summary>
        /// <param name="categoryId">分类ID。</param>
        /// <returns>包含所有信息的分类集合，获取失败返回空。</returns>
        public CategoryCollection GetChildCategories(int categoryId)
        {
            Validator.CheckNotPositive(categoryId, "categoryId");

            try
            {
                return provider.GetChildCategories(categoryId);
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
        /// 根据分类别名返回其子分类信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="catNiceName">分类别名。</param>
        /// <returns>包含所有信息的分类集合。</returns>
        public CategoryCollection GetChildCategories(int siteId, string catNiceName)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            Validator.CheckStringNull(catNiceName, "catNiceName");
            if (!rNiceName.IsMatch(catNiceName))
            {
                throw new ArgumentException("分类别名格式不正确");
            }

            try
            {
                return provider.GetChildCategories(siteId, catNiceName);
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
        /// 判断特定的分类别名是否存在。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="catNiceName">分类别名。</param>
        /// <returns>是否存在。</returns>
        public bool IsCategoryNiceNameExists(int siteId, string catNiceName)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            Validator.CheckStringNull(catNiceName, "catNiceName");
            if (!rNiceName.IsMatch(catNiceName))
            {
                throw new ArgumentException("分类别名只能由字母、下划线、数字构成。");
            }

            try
            {
                return provider.IsCategoryNiceNameExists(siteId, catNiceName);
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
        /// 更新分类信息。
        /// </summary>
        /// <param name="category">分类信息。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有更新分类信息的权限时抛出该异常。</exception>
        public bool UpdateCategory(CategoryInfo category)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CategoryManager(UserInfo)实例化该类。");
            }
            return UpdateCategory(currentUser, category);
        }

        /// <summary>
        /// 更新分类信息。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="category">分类信息。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有更新分类信息的权限时抛出该异常。</exception>
        public bool UpdateCategory(UserInfo currentUser, CategoryInfo category)
        {
            Validator.CheckNull(category, "category");
            Validator.CheckNotPositive(category.CategoryId, "category.CategoryId");
            Validator.CheckStringNull(category.CategoryName, "category.CategoryName");
            Validator.CheckStringNull(category.CategoryNiceName, "category.CategoryNiceName");
            Validator.CheckNegative(category.ParentCategoryId, "category.ParentCategoryId");
            if (!rNiceName.IsMatch(category.CategoryNiceName))
            {
                throw new ArgumentException("分类别名只能由字母、下划线、数字构成");
            }

            PermissionManager.CheckPermission(currentUser, "EditCategory", "编辑分类");

            if (IsCategoryNiceNameExists(category.SiteId, category.CategoryNiceName))
            {
                throw new Exception("该分类别名已存在，请重新设置！");
            }

            try
            {
                return provider.UpdateCategory(category);
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
    }
}
