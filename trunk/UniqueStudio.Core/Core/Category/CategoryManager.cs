using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.Permission;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.Core.Category
{
    /// <summary>
    /// 提供分类管理的方法
    /// </summary>
    public class CategoryManager
    {
        private static Regex rNiceName = new Regex("^[a-z,_,1-9]+$");
        private static readonly ICategory provider = DALFactory.CreateCategory();

        /// <summary>
        /// 初始化<see cref="CategoryManager"/>类的实例
        /// </summary>
        public CategoryManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <remarks>
        /// <paramref name="category"/>必须包含分类名和分类别名，且分类别名仅由字母、下划线和数字构成。
        /// </remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="category">分类信息</param>
        /// <returns>创建成功返回包含分类ID的实例，否则返回空</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建分类的权限时抛出该异常</exception>
        public CategoryInfo CreateCategory(UserInfo currentUser, CategoryInfo category)
        {
            if (object.ReferenceEquals(category, null))
            {
                throw new ArgumentNullException("category");
            }
            if (string.IsNullOrEmpty(category.CategoryName) || string.IsNullOrEmpty(category.CategoryNiceName))
            {
                throw new ArgumentException("分类名和分类别名不能为空");
            }
            if (!rNiceName.IsMatch(category.CategoryNiceName))
            {
                throw new ArgumentException("分类别名只能由字母、下划线、数字构成");
            }
            if (category.ParentCategoryId < 0)
            {
                throw new ArgumentException("父分类ID不能小于0");
            }

            if (!PermissionManager.HasPermission(currentUser, "CreateCategory"))
            {
                throw new InvalidPermissionException("当前用户没有创建分类的权限，请与管理员联系。");
            }

            try
            {
                return provider.CreateCategory(category);
            }
            catch (Exception ex)
            {
                Common.ErrorLogging.ErrorLogger.LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <remarks>根据全局配置决定是否删除子分类</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="categoryId">待删除分类ID</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除分类的权限时抛出该异常</exception>
        public bool DeleteCategory(UserInfo currentUser, int categoryId)
        {
            return DeleteCategory(currentUser, categoryId, WebSiteConfig.IsDeleteChildCategories);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="categoryId">待删除分类ID</param>
        /// <param name="isDeleteChildCategories">是否删除子分类</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除分类的权限时抛出该异常</exception>
        public bool DeleteCategory(UserInfo currentUser, int categoryId, bool isDeleteChildCategories)
        {
            if (categoryId <= 0)
            {
                throw new ArgumentException("分类ID不能小于0", "categoryId");
            }

            if (!PermissionManager.HasPermission(currentUser, "DeleteCategory"))
            {
                throw new InvalidPermissionException("当前用户没有创建分类的权限，请与管理员联系。");
            }

            try
            {
                return DeleteCategory(currentUser, categoryId, isDeleteChildCategories);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                return false;
            }
        }

        /// <summary>
        /// 删除多个分类
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="categoryIds">待删除分类ID的集合</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除分类的权限时抛出该异常</exception>
        public bool DeleteCategories(UserInfo currentUser, int[] categoryIds)
        {
            return DeleteCategories(currentUser, categoryIds, WebSiteConfig.IsDeleteChildCategories);
        }

        /// <summary>
        /// 删除多个分类
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="categoryIds">待删除分类ID的集合</param>
        /// <param name="isDeleteChildCategories">是否删除子分类</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除分类的权限时抛出该异常</exception>
        public bool DeleteCategories(UserInfo currentUser, int[] categoryIds,bool isDeleteChildCategories)
        {
            if (categoryIds == null || categoryIds.Length == 0)
            {
                throw new ArgumentNullException("categoryIds");
            }
            for (int i = 0; i < categoryIds.Length; i++)
            {
                if (categoryIds[i] <= 0)
                {
                    throw new ArgumentException("分类ID不能小于0");
                }
            }

            if (!PermissionManager.HasPermission(currentUser, "DeleteCategory"))
            {
                throw new InvalidPermissionException("当前用户没有创建分类的权限，请与管理员联系。");
            }

            List<int> errorList = new List<int>();
            for (int i = 0; i < categoryIds.Length; i++)
            {
                try
                {
                    if (provider.DeleteCategory(categoryIds[i],isDeleteChildCategories) == false)
                    {
                        errorList.Add(categoryIds[i]);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogError(ex);
                    throw new DatabaseException();
                }
            }

            if (errorList.Count == 0)
            {
                return true;
            }
            else
            {
                StringBuilder sb = new StringBuilder("以下分类删除失败：");
                for (int i = 0; i < errorList.Count - 1; i++)
                {
                    sb.Append(errorList[i].ToString()).Append("，");
                }
                sb.Append(errorList[errorList.Count - 1].ToString()).Append("。");
                throw new Exception(sb.ToString());
            }
        }

        /// <summary>
        /// 返回所有分类
        /// </summary>
        /// <returns>包含所有信息的分类集合，如果获取失败返回空</returns>
        public CategoryCollection GetAllCategories()
        {
            try
            {
                return provider.GetAllCategories();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据分类ID返回分类信息
        /// </summary>
        /// <param name="categoryId">分类ID</param>
        /// <returns>分类信息，获取失败返回空</returns>
        public CategoryInfo GetCategory(int categoryId)
        {
            if (categoryId <= 0)
            {
                throw new ArgumentException("分类ID不能小于0", "categoryId");
            }

            try
            {
                return provider.GetCategory(categoryId);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据分类别名返回分类信息
        /// </summary>
        /// <param name="catNiceName">分类别名</param>
        /// <returns>分类信息，获取失败返回空</returns>
        public CategoryInfo GetCategory(string catNiceName)
        {
            if (string.IsNullOrEmpty(catNiceName))
            {
                throw new ArgumentNullException("catNiceName");
            }
            if (!rNiceName.IsMatch(catNiceName))
            {
                throw new ArgumentException("分类别名格式不正确");
            }

            try
            {
                return provider.GetCategory(catNiceName);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// 返回分类路径
        /// </summary>
        /// <param name="categoryId">叶节点分类ID</param>
        /// <returns>分类路径根节点</returns>
        public CategoryInfo GetCategoryPath(int categoryId)
        {
            return provider.GetCategoryPath(categoryId);
        }

        /// <summary>
        /// 根据分类ID返回其子分类信息
        /// </summary>
        /// <param name="categoryId">分类ID</param>
        /// <returns>包含所有信息的分类集合，获取失败返回空</returns>
        public CategoryCollection GetChildCategories(int categoryId)
        {
            if (categoryId <= 0)
            {
                throw new ArgumentException("分类ID不能小于0", "categoryId");
            }

            try
            {
                return provider.GetChildCategories(categoryId);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据分类别名返回其子分类信息
        /// </summary>
        /// <param name="catNiceName">分类别名</param>
        /// <returns>包含所有信息的分类集合</returns>
        public CategoryCollection GetChildCategories(string catNiceName)
        {
            if (string.IsNullOrEmpty(catNiceName))
            {
                throw new ArgumentNullException("catNiceName");
            }
            if (!rNiceName.IsMatch(catNiceName))
            {
                throw new ArgumentException("分类别名格式不正确");
            }

            try
            {
                return provider.GetChildCategories(catNiceName);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// 更新分类信息
        /// </summary>
        /// <remarks>需提供分类的ID</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="category">分类信息</param>
        /// <returns>是否更新成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有更新分类信息的权限时抛出该异常</exception>
        public bool UpdateCategory(UserInfo currentUser, CategoryInfo category)
        {
            if (object.ReferenceEquals(category,null))
            {
                throw new ArgumentNullException("category");
            }
            if (category.CategoryId <= 0)
            {
                throw new ArgumentException("分类ID没有指定或错误");
            }
            if (string.IsNullOrEmpty(category.CategoryName) || string.IsNullOrEmpty(category.CategoryNiceName))
            {
                throw new ArgumentException("分类名和分类别名不能为空");
            }
            if (!rNiceName.IsMatch(category.CategoryNiceName))
            {
                throw new ArgumentException("分类别名只能由字母、下划线、数字构成");
            }
            if (category.ParentCategoryId < 0)
            {
                throw new ArgumentException("父分类ID不能小于0");
            }

            if (!PermissionManager.HasPermission(currentUser, "EditCategory"))
            {
                throw new InvalidPermissionException("当前用户没有编辑分类的权限，请与管理员联系。");
            }

            try
            {
                return provider.UpdateCategory(category);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                return false;
            }
        }
    }
}
