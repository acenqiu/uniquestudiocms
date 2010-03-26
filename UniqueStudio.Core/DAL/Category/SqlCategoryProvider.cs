//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供分类管理在Sql Server上的实现方法。
// 完成日期：2010年03月19日
// 版本：v0.8
// 作者：邱江毅
//
// 修改记录1：
// 修改日期：2010年03月26日
// 版本号：v0.8.5
// 修改人：邱江毅
// 修改内容：+)CategoryCollection GetCategoryChain(int categoryId);
//                 +)CategoryCollection GetCategoryChain(Guid chainId);
//                 *)CategoryInfo GetCategoryPath(int categoryId);
//
// 说明：
//    1.不删除子分类时，Depth,ChainID没更新；
//    2.无法自动删除子分类；
//    3.无法级联修改。（1.0版）
//=================================================================
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
    /// 提供分类管理在Sql Server上的实现方法。
    /// </summary>
    internal class SqlCategoryProvider : ICategory
    {
        private const string CREATE_CATEGORY = "CreateCategory";
        private const string DELETE_CATEGORY = "DeleteCategory";
        private const string GET_ALL_CATEGORIES = "GetAllCategories";
        private const string GET_CATEGORY_BY_ID = "GetCategoryById";
        private const string GET_CATEGORY_BY_NICENAME = "GetCategoryByNiceName";
        private const string GET_CATEGORY_CHAIN_BY_CATID = "GetCategoryChainByCatId";
        private const string GET_CATEGORY_CHAIN_BY_CHAINID = "GetCategoryChainByChainId";
        private const string GET_CHILD_CATEGORIES_BY_ID = "GetChildCategoriesById";
        private const string GET_CHILD_CATEGORIES_BY_NICENAME = "GetChildCategoriesByNiceName";
        private const string IS_CATEGORY_NICENAME_EXISTS = "IsCategoryNiceNameExists";
        private const string UPDATE_CATEGORY = "UpdateCategory";

        /// <summary>
        /// 初始化<see cref="SqlCategoryProvider"/>类的实例。
        /// </summary>
        public SqlCategoryProvider()
        {
            //默认构造函数
        }

        /// <summary>
        /// 创建分类。
        /// </summary>
        /// <param name="category">分类信息。</param>
        /// <returns>创建成功返回包含分类ID的实例，否则返回空。</returns>
        public CategoryInfo CreateCategory(CategoryInfo category)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@SiteID",category.SiteId),
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
        /// 删除分类。
        /// </summary>
        /// <param name="categoryId">待删除分类ID。</param>
        /// <param name="isDeleteChildCategories">是否删除子分类。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeleteCategory(int categoryId, bool isDeleteChildCategories)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@CategoryID",categoryId),
                                                    new SqlParameter("@IsDeleteChildCategories",isDeleteChildCategories)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_CATEGORY, parms) > 0;
        }

        /// <summary>
        /// 删除多个分类。
        /// </summary>
        /// <param name="categoryIds">待删除分类ID的集合。</param>
        /// <param name="isDeleteChildCategories">是否删除子分类。</param>
        /// <returns>是否删除成功。</returns>
        public bool DeleteCategories(int[] categoryIds, bool isDeleteChildCategories)
        {
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(DELETE_CATEGORY, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int);
                    cmd.Parameters.AddWithValue("@IsDeleteChildCategories", isDeleteChildCategories);

                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (int categoryId in categoryIds)
                            {
                                cmd.Parameters["@CategoryID"].Value = categoryId;
                                cmd.ExecuteNonQuery();
                            }
                            trans.Commit();
                            return true;
                        }
                        catch
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 返回所有分类。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <returns>包含所有信息的分类集合，如果获取失败返回空。</returns>
        public CategoryCollection GetAllCategories(int siteId)
        {
            CategoryCollection collection = new CategoryCollection();
            SqlParameter parm = new SqlParameter("@SiteID", siteId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_CATEGORIES, parm))
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
        /// 根据分类ID返回分类信息。
        /// </summary>
        /// <param name="categoryId">分类ID。</param>
        /// <returns>分类信息，获取失败返回空。</returns>
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
        /// 根据分类别名返回分类信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="catNiceName">分类别名。</param>
        /// <returns>分类信息，获取失败返回空。</returns>
        public CategoryInfo GetCategory(int siteId, string catNiceName)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@SiteID",siteId),
                                                        new SqlParameter("@CategoryNiceName", catNiceName)};
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_CATEGORY_BY_NICENAME, parms))
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
        /// 返回分类链。
        /// </summary>
        /// <param name="MenuItemId">该分类链中任一分类的ID。</param>
        /// <returns>分类链中各分类的集合。</returns>
        public CategoryCollection GetCategoryChain(int categoryId)
        {
            CategoryCollection collection = new CategoryCollection();
            SqlParameter parm = new SqlParameter("@CategoryID", categoryId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_CATEGORY_CHAIN_BY_CATID, parm))
            {
                while (reader.Read())
                {
                    CategoryInfo item = FillCategoryInfo(reader);
                    collection.Add(item);
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回分类链。
        /// </summary>
        /// <param name="chainId">该菜单链的ID。</param>
        /// <returns>分类链中各分类的集合。</returns>
        public CategoryCollection GetCategoryChain(Guid chainId)
        {
            CategoryCollection collection = new CategoryCollection();
            SqlParameter parm = new SqlParameter("@ChainID", chainId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_CATEGORY_CHAIN_BY_CHAINID, parm))
            {
                while (reader.Read())
                {
                    CategoryInfo item = FillCategoryInfo(reader);
                    collection.Add(item);
                }
            }
            return collection;
        }

        /// <summary>
        /// 返回分类路径。
        /// </summary>
        /// <param name="categoryId">叶节点分类ID。</param>
        /// <returns>分类路径根节点。</returns>
        public CategoryInfo GetCategoryPath(int categoryId)
        {
            CategoryInfo category = null;
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GET_CATEGORY_BY_ID, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int);
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
                    while (temp != null && temp.ParentCategoryId != 0)
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

                        temp.ChildCategories = new CategoryCollection();
                        temp.ChildCategories.Add(category);
                        category = temp;
                    }
                    conn.Close();
                }
            }
            return category;
        }

        /// <summary>
        /// 根据分类ID返回其子分类信息。
        /// </summary>
        /// <param name="categoryId">分类ID。</param>
        /// <returns>包含所有信息的分类集合，获取失败返回空。</returns>
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
        /// 根据分类别名返回其子分类信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="catNiceName">分类别名。</param>
        /// <returns>包含所有信息的分类集合。</returns>
        public CategoryCollection GetChildCategories(int siteId, string catNiceName)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new  SqlParameter("@SiteID",siteId),
                                                        new SqlParameter("@CategoryNiceName", catNiceName)};
            CategoryCollection collection = new CategoryCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_CHILD_CATEGORIES_BY_NICENAME, parms))
            {
                while (reader.Read())
                {
                    collection.Add(FillCategoryInfo(reader));
                }
            }
            return collection;
        }

        /// <summary>
        /// 判断特定的分类别名是否存在。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="catNiceName">分类别名。</param>
        /// <returns>是否存在。</returns>
        public bool IsCategoryNiceNameExists(int siteId, string catNiceName)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                            new SqlParameter("@SiteID",siteId),
                                                            new SqlParameter("@CategoryNiceName",catNiceName)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, IS_CATEGORY_NICENAME_EXISTS, parms);
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToBoolean((int)o);
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 更新分类信息。
        /// </summary>
        /// <param name="category">分类信息。</param>
        /// <returns>是否更新成功。</returns>
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
            category.SiteId = (int)reader["SiteID"];
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
