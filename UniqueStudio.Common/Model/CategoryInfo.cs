﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示一个分类的实体类。
    /// </summary>
    [Serializable]
    public class CategoryInfo
    {
        private int categoryId = 0;
        private string categoryName;
        private string categoryNiceName;
        private string description = string.Empty;
        private int subOf = 0;
        private string parentCategoryName = string.Empty;

        private CategoryInfo parentCategory = null;
        private CategoryInfo childCategory = null;

        /// <summary>
        /// 初始化CategoryInfo类的实例。
        /// </summary>
        public CategoryInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        /// <summary>
        /// 对程序友好的分类名称（请使用英文或拼音）
        /// </summary>
        public string CategoryNiceName
        {
            get { return categoryNiceName; }
            set { categoryNiceName = value; }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 父分类ID
        /// </summary>
        public int SubOf
        {
            get { return subOf; }
            set { subOf = value; }
        }

        /// <summary>
        /// 父分类名
        /// </summary>
        public string ParentCategoryName
        {
            get { return parentCategoryName; }
            set { parentCategoryName = value; }
        }

        /// <summary>
        /// 父分类信息
        /// </summary>
        public CategoryInfo ParentCategory
        {
            get { return parentCategory; }
            set { parentCategory = value; }
        }

        /// <summary>
        /// 子分类信息
        /// </summary>
        public CategoryInfo ChildCategory
        {
            get { return childCategory; }
            set { childCategory = value; }
        }
    }
}
