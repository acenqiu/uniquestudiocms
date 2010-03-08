using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    internal interface ICategory
    {
        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="category">分类信息</param>
        /// <returns>创建成功返回包含分类ID的实例，否则返回null</returns>
        CategoryInfo CreateCategory(CategoryInfo category);

        bool DeleteCategory(int categoryId, bool isDeleteChildCategories);

        CategoryCollection GetAllCategories();

        CategoryInfo GetCategory(int categoryId);

        CategoryInfo GetCategory(string catNiceName);

        CategoryInfo GetCategoryPath(int categoryId);

        CategoryCollection GetChildCategories(int categoryId);

        CategoryCollection GetChildCategories(string categoryNiceName);

        bool UpdateCategory(CategoryInfo category);
    }
}
