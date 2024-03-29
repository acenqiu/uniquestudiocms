﻿using System;
using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 分类管理提供类需实现的接口。
    /// </summary>
    internal interface ICategory
    {
        /// <summary>
        /// 创建分类。
        /// </summary>
        /// <param name="category">分类信息。</param>
        /// <returns>创建成功返回包含分类ID的实例，否则返回空。</returns>
        CategoryInfo CreateCategory(CategoryInfo category);

        /// <summary>
        /// 删除分类。
        /// </summary>
        /// <param name="categoryId">待删除分类ID。</param>
        /// <param name="isDeleteChildCategories">是否删除子分类。</param>
        /// <returns>是否删除成功。</returns>
        bool DeleteCategory(int categoryId, bool isDeleteChildCategories);

        /// <summary>
        /// 删除多个分类。
        /// </summary>
        /// <param name="categoryIds">待删除分类ID的集合。</param>
        /// <param name="isDeleteChildCategories">是否删除子分类。</param>
        /// <returns>是否删除成功。</returns>
        bool DeleteCategories(int[] categoryIds, bool isDeleteChildCategories);

        /// <summary>
        /// 返回所有分类。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <returns>包含所有信息的分类集合，如果获取失败返回空。</returns>
        CategoryCollection GetAllCategories(int siteId);

        /// <summary>
        /// 根据分类ID返回分类信息。
        /// </summary>
        /// <param name="categoryId">分类ID。</param>
        /// <returns>分类信息，获取失败返回空。</returns>
        CategoryInfo GetCategory(int categoryId);

        /// <summary>
        /// 根据分类别名返回分类信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="catNiceName">分类别名。</param>
        /// <returns>分类信息，获取失败返回空。</returns>
        CategoryInfo GetCategory(int siteId, string catNiceName);

        /// <summary>
        /// 返回分类链。
        /// </summary>
        /// <param name="MenuItemId">该分类链中任一分类的ID。</param>
        /// <returns>分类链中各分类的集合。</returns>
        CategoryCollection GetCategoryChain(int categoryId);

        /// <summary>
        /// 返回分类链。
        /// </summary>
        /// <param name="chainId">该菜单链的ID。</param>
        /// <returns>分类链中各分类的集合。</returns>
        CategoryCollection GetCategoryChain(Guid chainId);

        /// <summary>
        /// 返回分类路径。
        /// </summary>
        /// <param name="categoryId">叶节点分类ID。</param>
        /// <returns>分类路径根节点。</returns>
        CategoryInfo GetCategoryPath(int categoryId);

        /// <summary>
        /// 根据分类ID返回其子分类信息。
        /// </summary>
        /// <param name="categoryId">分类ID。</param>
        /// <returns>包含所有信息的分类集合，获取失败返回空。</returns>
        CategoryCollection GetChildCategories(int categoryId);

        /// <summary>
        /// 根据分类别名返回其子分类信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="catNiceName">分类别名。</param>
        /// <returns>包含所有信息的分类集合。</returns>
        CategoryCollection GetChildCategories(int siteId, string catNiceName);

        /// <summary>
        /// 判断特定的分类别名是否存在。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="catNiceName">分类别名。</param>
        /// <returns>是否存在。</returns>
        bool IsCategoryNiceNameExists(int siteId, string catNiceName);

        /// <summary>
        /// 更新分类信息。
        /// </summary>
        /// <param name="category">分类信息。</param>
        /// <returns>是否更新成功。</returns>
        bool UpdateCategory(CategoryInfo category);
    }
}
