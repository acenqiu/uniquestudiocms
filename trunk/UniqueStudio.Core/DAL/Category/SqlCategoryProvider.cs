using System;
using System.Data;
using System.Data.SqlClient;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.DAL.Category
{
    /// <summary>
    /// 提供分类管理在Sql Server上的实现方法
    /// </summary>
    internal class SqlCategoryProvider : ICategory
    {
        private const string CREATE_CATEGORY = "CreateCategory";
        private const string DELETE_CATEGORY = "DeleteCategory";
        private const string GET_ALL_CATEGORIES = "GetAllCategories";
        private const string GET_CATEGORY_BY_ID = "GetCategoryById";
        private const string GET_CATEGORY_BY_NICENAME = "GetCategoryByNiceName";
        private const string GET_CHILD_CATEGORIES_BY_ID = "GetChildCategoriesById";
        private const string GET_CHILD_CATEGORIES_BY_NICENAME = "GetChildCategoriesByNiceName";
        private const string UPDATE_CATEGORY = "UpdateCategory";

        /// <summary>
        /// 初始化<see cref="SqlCategoryProvider"/>类的实例
        /// </summary>
        public SqlCategoryProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="category">分类信息</param>
        /// <returns>创建成功返回包含分类ID的实例，否则返回空</returns>
        public CategoryInfo CreateCategory(CategoryInfo category)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@CategoryName",category.CategoryName),
                                                        new  SqlParameter("@CategoryNiceName",category.CategoryNiceName),
                                                        new SqlParameter("@Description",category.Description),
                                                        new SqlParameter("@SubOf",category.ParentCategoryId)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, CREATE_CATEGORY, parms);
            if (o != null && o != DBNull.Value)
            {
                category.CategoryId = Convert.ToInt32(o);
                return category;
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="categoryId">待删除分类ID</param>
        /// <param name="isDeleteChildCategories">是否删除子分类</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteCategory(int categoryId, bool isDeleteChildCategories)
        {
            //@IsDeleteChildCategories暂时无法使用，现处理方式为不删除
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@CategoryID",categoryId),
                                                    new SqlParameter("@IsDeleteChildCategories",isDeleteChildCategories)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_CATEGORY, parms) > 0;
        }

        /// <summary>
        /// 返回所有分类
        /// </summary>
        /// <returns>包含所有信息的分类集合，如果获取失败返回空</returns>
        public CategoryCollection GetAllCategories()
        {
            CategoryCollection collection = new CategoryCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_CATEGORIES, null))
            {
                while (reader.Read())
                {
                    CategoryInfo category = FillCategoryInfo(reader);
                    category.ParentCategoryName = reader["ParentCategoryName"].ToString();
                    collection.Add(category);
                }
            }
            return collection;
        }

        /// <summary>
        /// 根据分类ID返回分类信息
        /// </summary>
        /// <param name="categoryId">分类ID</param>
        /// <returns>分类信息，获取失败返回空</returns>
        public CategoryInfo GetCategory(int categoryId)
        {
            SqlParameter parm = new SqlParameter("@CategoryID", categoryId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_CATEGORY_BY_ID, parm))
            {
                if (reader.Read())
                {
                    return FillCategoryInfo(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 根据分类别名返回分类信息
        /// </summary>
        /// <param name="catNiceName">分类别名</param>
        /// <returns>分类信息，获取失败返回空</returns>
        public CategoryInfo GetCategory(string catNiceName)
        {
            SqlParameter parm = new SqlParameter("@CategoryNiceName", catNiceName);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_CATEGORY_BY_NICENAME, parm))
            {
                if (reader.Read())
                {
                    return FillCategoryInfo(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 返回分类路径
        /// </summary>
        /// <param name="categoryId">叶节点分类ID</param>
        /// <returns>分类路径根节点</returns>
        public CategoryInfo GetCategoryPath(int categoryId)
        {
            CategoryInfo category = null;
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GET_CATEGORY_BY_ID,conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CategoryID",SqlDbType.Int);
                    conn.Open();

                    CategoryInfo temp = null;
                    cmd.Parameters[0].Value = categoryId;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            temp = FillCategoryInfo(reader);
                            category = temp;
                        }

                        reader.Close();
                    }
                    while (temp !=null && temp.ParentCategoryId !=0)
                    {
                        cmd.Parameters[0].Value = temp.ParentCategoryId;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                temp = FillCategoryInfo(reader);
                            }
                            reader.Close();
                        }

                        temp.ChildCategory = category;
                        category = temp;
                    }
                    conn.Close();
                }
            }
            return category;
        }

        /// <summary>
        /// 根据分类ID返回其子分类信息
        /// </summary>
        /// <param name="categoryId">分类ID</param>
        /// <returns>包含所有信息的分类集合，获取失败返回空</returns>
        public CategoryCollection GetChildCategories(int categoryId)
        {
            SqlParameter parm = new SqlParameter("@CategoryID", categoryId);
            CategoryCollection collection = new CategoryCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_CHILD_CATEGORIES_BY_ID, parm))
            {
                while (reader.Read())
                {
                    collection.Add(FillCategoryInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 根据分类别名返回其子分类信息
        /// </summary>
        /// <param name="catNiceName">分类别名</param>
        /// <returns>包含所有信息的分类集合</returns>
        public CategoryCollection GetChildCategories(string catNiceName)
        {
            SqlParameter parm = new SqlParameter("@CategoryNiceName", catNiceName);
            CategoryCollection collection = new CategoryCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_CHILD_CATEGORIES_BY_NICENAME, parm))
            {
                while (reader.Read())
                {
                    collection.Add(FillCategoryInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 更新分类信息
        /// </summary>
        /// <param name="category">分类信息</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateCategory(CategoryInfo category)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@CategoryID",category.CategoryId),
                                                    new SqlParameter("@CategoryName",category.CategoryName),
                                                    new SqlParameter("@CategoryNiceName",category.CategoryNiceName),
                                                    new SqlParameter("@Description",category.Description),
                                                    new SqlParameter("@SubOf",category.ParentCategoryId)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UPDATE_CATEGORY, parms) > 0;
        }

        private CategoryInfo FillCategoryInfo(SqlDataReader reader)
        {
            CategoryInfo category = new CategoryInfo();
            category.CategoryId = (int)reader["CategoryID"];
            category.CategoryName = reader["CategoryName"].ToString();
            category.CategoryNiceName = reader["CategoryNiceName"].ToString();
            if (DBNull.Value != reader["Description"])
            {
                category.Description = reader["Description"].ToString();
            }
            category.ParentCategoryId = (int)reader["SubOf"];
            return category;
        }
    }
}
