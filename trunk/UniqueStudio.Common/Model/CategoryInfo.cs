//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：表示一个分类的实体类。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
// 
// 修改记录1：
// 修改日期：2010年03月26日
// 版本号：v1.0alpha
// 修改人：邱江毅
// 修改内容：*）ChildCategory ==>CategoryCollection ChildCategories
//=================================================================
using System;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示一个分类的实体类。
    /// </summary>
    [Serializable]
    public class CategoryInfo
    {
        private int categoryId = 0;
        private int siteId = 1;
        private string categoryName;
        private string categoryNiceName;
        private string description = string.Empty;
        private int parentCategoryId = 0;
        private string parentCategoryName = string.Empty;

        private CategoryInfo parentCategory = null;
        private CategoryCollection childCategories = null;

        /// <summary>
        /// 初始化CategoryInfo类的实例。
        /// </summary>
        public CategoryInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 分类ID。
        /// </summary>
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }
        /// <summary>
        /// 网站ID。
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// 分类名称。
        /// </summary>
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }
        /// <summary>
        /// 分类别名，对程序友好的分类名称（请使用英文或拼音）。
        /// </summary>
        public string CategoryNiceName
        {
            get { return categoryNiceName; }
            set { categoryNiceName = value; }
        }
        /// <summary>
        /// 说明。
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        /// <summary>
        /// 父分类ID。
        /// </summary>
        public int ParentCategoryId
        {
            get { return parentCategoryId; }
            set { parentCategoryId = value; }
        }
        /// <summary>
        /// 父分类名。
        /// </summary>
        public string ParentCategoryName
        {
            get { return parentCategoryName; }
            set { parentCategoryName = value; }
        }
        /// <summary>
        /// 父分类信息。
        /// </summary>
        public CategoryInfo ParentCategory
        {
            get { return parentCategory; }
            set { parentCategory = value; }
        }
        /// <summary>
        /// 子分类的集合。
        /// </summary>
        public CategoryCollection ChildCategories
        {
            get { return childCategories; }
            set { childCategories = value; }
        }
    }
}
